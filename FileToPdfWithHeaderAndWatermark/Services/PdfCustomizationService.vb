Option Strict On
Option Explicit On
Option Infer On

Imports System.Drawing
Imports Syncfusion.Pdf
Imports Syncfusion.Pdf.Graphics

''' <summary>Adds the optional header and watermark to every page of an already converted PdfDocument.</summary>
Public Class PdfCustomizationService

    Private Const HeaderFontSize As Single = 10.0F
    Private Const HeaderTop As Single = 14.0F
    Private Const HeaderSidePadding As Single = 36.0F
    Private Const WatermarkOpacity As Single = 0.3F
    Private Const WatermarkFill As Single = 0.9F
    Private Const WatermarkAngle As Single = -45.0F ' counter-clockwise: bottom-left to top-right
    Private Const WatermarkReferenceSize As Single = 100.0F
    Private Const WatermarkMaxSize As Single = 150.0F

    Public Sub Apply(pdfDocument As PdfDocument, options As ConversionOptions)
        If Not options.AddHeader AndAlso Not options.AddWatermark Then
            Return
        End If

        Dim headerFont As New PdfStandardFont(PdfFontFamily.Helvetica, HeaderFontSize)

        For pageIndex As Integer = 0 To pdfDocument.Pages.Count - 1
            Dim page As PdfPage = pdfDocument.Pages(pageIndex)
            Dim graphics As PdfGraphics = CreateFullPageGraphics(page)

            If options.AddHeader Then
                DrawHeader(page, graphics, options.HeaderText, headerFont)
            End If
            If options.AddWatermark Then
                DrawWatermark(page, graphics, options.WatermarkText)
            End If
        Next
    End Sub

    ''' <summary>
    ''' Page graphics are inset by the section margins and clipped to them, which would hide anything drawn in the
    ''' margin. A new layer created while the margins are zero covers the whole page; the margins are restored afterwards.
    ''' </summary>
    Private Shared Function CreateFullPageGraphics(page As PdfPage) As PdfGraphics
        Dim margins As PdfMargins = page.Section.PageSettings.Margins
        Dim left As Single = margins.Left
        Dim top As Single = margins.Top
        Dim right As Single = margins.Right
        Dim bottom As Single = margins.Bottom

        margins.All = 0.0F
        Try
            Return page.Layers.Add().Graphics
        Finally
            margins.Left = left
            margins.Top = top
            margins.Right = right
            margins.Bottom = bottom
        End Try
    End Function

    ''' <summary>Draws centered header text and a thin separator inside the top margin.</summary>
    Private Shared Sub DrawHeader(page As PdfPage, graphics As PdfGraphics, headerText As String, font As PdfFont)
        Dim pageSize As SizeF = page.Size
        Dim textWidth As Single = pageSize.Width - 2.0F * HeaderSidePadding
        Dim textHeight As Single = font.Height

        Dim textBrush As New PdfSolidBrush(New PdfColor(89, 89, 89))
        Dim format As New PdfStringFormat(PdfTextAlignment.Center, PdfVerticalAlignment.Middle)
        graphics.DrawString(headerText, font, textBrush,
                            New RectangleF(HeaderSidePadding, HeaderTop, textWidth, textHeight), format)

        Dim separatorY As Single = HeaderTop + textHeight + 4.0F
        Dim separatorPen As New PdfPen(New PdfColor(200, 200, 200), 0.75F)
        graphics.DrawLine(separatorPen, HeaderSidePadding, separatorY, pageSize.Width - HeaderSidePadding, separatorY)
    End Sub

    ''' <summary>Draws light-gray, semi-transparent text rotated 45 degrees across the page center.</summary>
    Private Shared Sub DrawWatermark(page As PdfPage, graphics As PdfGraphics, watermarkText As String)
        Dim pageSize As SizeF = page.Size

        ' A box rotated by 45 degrees stays on the page when (textWidth + textHeight) <= min(pageWidth, pageHeight) * sqrt(2).
        ' Measure at a reference size, then scale the font so the box fills WatermarkFill of that limit.
        Dim availableSum As Single = Math.Min(pageSize.Width, pageSize.Height) * CSng(Math.Sqrt(2.0)) * WatermarkFill
        Dim referenceFont As New PdfStandardFont(PdfFontFamily.Helvetica, WatermarkReferenceSize, PdfFontStyle.Bold)
        Dim referenceSize As SizeF = referenceFont.MeasureString(watermarkText)
        Dim fontSize As Single = Math.Min(
            WatermarkReferenceSize * availableSum / Math.Max(referenceSize.Width + referenceSize.Height, 1.0F),
            WatermarkMaxSize)

        Dim watermarkFont As New PdfStandardFont(PdfFontFamily.Helvetica, fontSize, PdfFontStyle.Bold)
        Dim textSize As SizeF = watermarkFont.MeasureString(watermarkText)
        Dim watermarkBrush As New PdfSolidBrush(New PdfColor(192, 192, 192))

        Dim state As PdfGraphicsState = graphics.Save()
        Try
            graphics.SetTransparency(WatermarkOpacity)
            graphics.TranslateTransform(pageSize.Width / 2.0F, pageSize.Height / 2.0F)
            graphics.RotateTransform(WatermarkAngle)
            graphics.DrawString(watermarkText, watermarkFont, watermarkBrush,
                                -textSize.Width / 2.0F, -textSize.Height / 2.0F)
        Finally
            graphics.Restore(state)
        End Try
    End Sub

End Class
