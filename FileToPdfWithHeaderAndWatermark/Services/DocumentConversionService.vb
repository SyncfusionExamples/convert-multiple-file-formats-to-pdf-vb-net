Option Strict On
Option Explicit On
Option Infer On

Imports System.IO
Imports System.Net.Http
Imports System.Text
Imports Syncfusion.DocIO
Imports Syncfusion.DocIO.DLS
Imports Syncfusion.DocToPDFConverter
Imports System.Drawing
Imports Syncfusion.HtmlConverter
Imports Syncfusion.Pdf
Imports Syncfusion.Pdf.Graphics
Imports Syncfusion.Office.Markdown
Imports Syncfusion.Presentation
Imports Syncfusion.PresentationToPdfConverter
Imports Syncfusion.XlsIO
Imports Syncfusion.ExcelToPdfConverter

''' <summary>
''' Detects the input format, converts the file to a PdfDocument, applies the optional
''' header/watermark and saves the result as a separate PDF.
''' </summary>
Public Class DocumentConversionService

    Private Const ImagePageMargin As Single = 48.0F

    Private ReadOnly _customizationService As PdfCustomizationService

    Shared Sub New()
        ' Legacy Office formats (.doc, .ppt, .xls) need code-page encodings that .NET does not register by default.
        Encoding.RegisterProvider(CodePagesEncodingProvider.Instance)
    End Sub

    Public Sub New()
        Me.New(New PdfCustomizationService())
    End Sub

    Public Sub New(customizationService As PdfCustomizationService)
        _customizationService = customizationService
    End Sub

    ''' <summary>Converts one file. Never throws; failures are returned and logged.</summary>
    Public Function ConvertFile(options As ConversionOptions) As ConversionResult
        Dim format As InputFormat = SupportedFileTypes.GetFormat(options.InputFilePath)

        Try
            Dim pdfDocument As PdfDocument = ConvertToPdfDocument(options.InputFilePath)
            Try
                _customizationService.Apply(pdfDocument, options)

                Dim outputPath As String = GetUniqueFilePath(options.OutputFilePath)
                Using outputStream As New FileStream(outputPath, FileMode.CreateNew, FileAccess.Write, FileShare.None)
                    pdfDocument.Save(outputStream)
                End Using

                Return ConversionResult.Succeeded(options.InputFilePath, outputPath)
            Finally
                pdfDocument.Close(True)
            End Try
        Catch ex As Exception
            Dim result As ConversionResult = ConversionResult.Failed(options.InputFilePath, ex)
            ConversionErrorLogger.Write(options.InputFilePath, format, result.ExceptionDetails, result.Message)
            Return result
        End Try
    End Function

    ''' <summary>Routes the file to the format-specific converter based on its extension.</summary>
    Public Function ConvertToPdfDocument(inputFilePath As String) As PdfDocument
        Select Case SupportedFileTypes.GetFormat(inputFilePath)
            Case InputFormat.Word
                Return ConvertWordToPdf(inputFilePath)
            Case InputFormat.Excel
                Return ConvertExcelToPdf(inputFilePath)
            Case InputFormat.PowerPoint
                Return ConvertPowerPointToPdf(inputFilePath)
            Case InputFormat.Markdown
                Return ConvertMarkdownToPdf(inputFilePath)
            Case InputFormat.Image
                Return ConvertImageToPdf(inputFilePath)
            Case InputFormat.Xps
                Return ConvertXpsToPdf(inputFilePath)
            Case InputFormat.Html
                Return ConvertHtmlToPdf(inputFilePath)
            Case Else
                Throw New NotSupportedException(
                    $"'{Path.GetExtension(inputFilePath)}' files are not supported. Supported types: {SupportedFileTypes.GetSupportedExtensionsText()}")
        End Select
    End Function

    Public Function ConvertWordToPdf(inputFilePath As String) As PdfDocument
        Using inputStream As MemoryStream = ReadFileIntoMemory(inputFilePath)
            Using wordDocument As New WordDocument(inputStream, Syncfusion.DocIO.FormatType.Automatic)
                Using renderer As New DocToPDFConverter()
                    Return renderer.ConvertToPDF(wordDocument)
                End Using
            End Using
        End Using
    End Function

    Public Function ConvertExcelToPdf(inputFilePath As String) As PdfDocument
        Using inputStream As MemoryStream = ReadFileIntoMemory(inputFilePath)
            Using excelEngine As New ExcelEngine()
                Dim workbook As IWorkbook = excelEngine.Excel.Workbooks.Open(inputStream)
                Try
                    ' Every worksheet that contains data is rendered by default.
                    Dim converter As New ExcelToPdfConverter(workbook)
                    Return converter.Convert()
                Finally
                    workbook.Close()
                End Try
            End Using
        End Using
    End Function

    Public Function ConvertPowerPointToPdf(inputFilePath As String) As PdfDocument
        Using inputStream As MemoryStream = ReadFileIntoMemory(inputFilePath)
            Using presentation As IPresentation = Syncfusion.Presentation.Presentation.Open(inputStream)
                Return PresentationToPdfConverter.Convert(presentation)
            End Using
        End Using
    End Function

    ''' <summary>Imports Markdown into a DocIO WordDocument (UTF-8), then renders it to PDF.</summary>
    Public Function ConvertMarkdownToPdf(inputFilePath As String) As PdfDocument
        Dim markdownFolder As String = Path.GetDirectoryName(Path.GetFullPath(inputFilePath))

        Using inputStream As MemoryStream = ReadFileIntoMemory(inputFilePath)
            Using wordDocument As New WordDocument()
                wordDocument.MdImportSettings.Encoding = Encoding.UTF8
                AddHandler wordDocument.MdImportSettings.ImageNodeVisited,
                    Sub(sender As Object, args As MdImageNodeVisitedEventArgs) LoadMarkdownImage(args, markdownFolder)

                wordDocument.Open(inputStream, Syncfusion.DocIO.FormatType.Markdown)

                Using converter As New DocToPDFConverter()
                    Return converter.ConvertToPDF(wordDocument)
                End Using
            End Using
        End Using
    End Function

    ''' <summary>Supplies image data for Markdown images: local paths resolve against the .md folder; missing images are skipped.</summary>
    Private Shared Sub LoadMarkdownImage(args As MdImageNodeVisitedEventArgs, markdownFolder As String)
        Try
            Dim imageUri As String = args.Uri
            If String.IsNullOrWhiteSpace(imageUri) Then
                Return
            End If

            If imageUri.StartsWith("http://", StringComparison.OrdinalIgnoreCase) OrElse
               imageUri.StartsWith("https://", StringComparison.OrdinalIgnoreCase) Then
                Using client As New HttpClient()
                    client.Timeout = TimeSpan.FromSeconds(15)
                    args.ImageStream = New MemoryStream(client.GetByteArrayAsync(imageUri).GetAwaiter().GetResult())
                End Using
            Else
                Dim imagePath As String = Path.Combine(markdownFolder, System.Uri.UnescapeDataString(imageUri))
                If File.Exists(imagePath) Then
                    args.ImageStream = New MemoryStream(File.ReadAllBytes(imagePath))
                End If
            End If
        Catch ex As Exception When TypeOf ex Is IOException OrElse TypeOf ex Is HttpRequestException OrElse TypeOf ex Is TaskCanceledException OrElse TypeOf ex Is UnauthorizedAccessException OrElse TypeOf ex Is ArgumentException
            ' An image that cannot be loaded is left out instead of failing the whole document.
        End Try
    End Sub

    ''' <summary>Creates one PDF page per image frame, fitting the frame inside the margins without stretching.</summary>
    Public Function ConvertImageToPdf(inputFilePath As String) As PdfDocument
        Dim pdfDocument As New PdfDocument()
        Try
            Using inputStream As MemoryStream = ReadFileIntoMemory(inputFilePath)
                Using bitmap As New PdfBitmap(inputStream)
                    ' Multi-frame TIFFs expose each frame through ActiveFrame; other formats have one frame.
                    For frameIndex As Integer = 0 To Math.Max(1, bitmap.FrameCount) - 1
                        bitmap.ActiveFrame = frameIndex
                        AddImagePage(pdfDocument, bitmap)
                    Next
                End Using
            End Using
            Return pdfDocument
        Catch
            pdfDocument.Close(True)
            Throw
        End Try
    End Function

    Public Function ConvertXpsToPdf(inputFilePath As String) As PdfDocument
        Using inputStream As MemoryStream = ReadFileIntoMemory(inputFilePath)
            Dim converter As New Syncfusion.XPS.XPSToPdfConverter()
            Return converter.Convert(inputStream)
        End Using
    End Function

    ''' <summary>Renders a local HTML file with the Blink engine, resolving CSS/images relative to the file's folder.</summary>
    Public Function ConvertHtmlToPdf(inputFilePath As String) As PdfDocument
        Dim html As String = File.ReadAllText(inputFilePath, Encoding.UTF8)
        Dim directoryPath As String = Path.GetDirectoryName(Path.GetFullPath(inputFilePath)) & Path.DirectorySeparatorChar
        Dim baseUrl As String = New Uri(directoryPath).AbsoluteUri

        Dim converter As New HtmlToPdfConverter(HtmlRenderingEngine.Blink)
        Dim settings As New BlinkConverterSettings()
        settings.Margin.All = 20.0F
        settings.Margin.Top = 50.0F
        converter.ConverterSettings = settings

        Return converter.Convert(html, baseUrl)
    End Function

    Private Shared Sub AddImagePage(pdfDocument As PdfDocument, image As PdfImage)
        Dim imageWidth As Single = image.Width
        Dim imageHeight As Single = image.Height

        ' Use A4 in the orientation that matches the image.
        Dim a4 As SizeF = PdfPageSize.A4
        Dim pageSize As SizeF = If(imageWidth > imageHeight, New SizeF(a4.Height, a4.Width), a4)

        Dim section As PdfSection = pdfDocument.Sections.Add()
        section.PageSettings.Margins.All = 0.0F
        section.PageSettings.Size = pageSize
        Dim page As PdfPage = section.Pages.Add()

        Dim availableWidth As Single = pageSize.Width - 2.0F * ImagePageMargin
        Dim availableHeight As Single = pageSize.Height - 2.0F * ImagePageMargin
        Dim scale As Single = Math.Min(availableWidth / imageWidth, availableHeight / imageHeight)
        Dim drawWidth As Single = imageWidth * scale
        Dim drawHeight As Single = imageHeight * scale
        Dim x As Single = (pageSize.Width - drawWidth) / 2.0F
        Dim y As Single = (pageSize.Height - drawHeight) / 2.0F

        page.Graphics.DrawImage(image, x, y, drawWidth, drawHeight)
    End Sub

    ''' <summary>Copies the file into memory so the input file is released immediately.</summary>
    Private Shared Function ReadFileIntoMemory(filePath As String) As MemoryStream
        Dim buffer As New MemoryStream()
        Using fileStream As New FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite)
            fileStream.CopyTo(buffer)
        End Using
        buffer.Position = 0
        Return buffer
    End Function

    Private Shared Function GetUniqueFilePath(desiredPath As String) As String
        If Not File.Exists(desiredPath) Then
            Return desiredPath
        End If

        Dim folder As String = Path.GetDirectoryName(desiredPath)
        Dim baseName As String = Path.GetFileNameWithoutExtension(desiredPath)
        Dim extension As String = Path.GetExtension(desiredPath)

        Dim counter As Integer = 1
        Dim candidate As String
        Do
            candidate = Path.Combine(folder, $"{baseName}_{counter}{extension}")
            counter += 1
        Loop While File.Exists(candidate)

        Return candidate
    End Function

End Class
