Option Strict On
Option Explicit On
Option Infer On

Imports System.Diagnostics
Imports System.IO
Imports System.Text

Public Class MainForm

    Private Const DefaultHeaderText As String = "Confidential Document"
    Private Const DefaultWatermarkText As String = "CONFIDENTIAL"
    Private Const DefaultOutputFolderName As String = "Output"
    Private Const DefaultInputFolderName As String = "input"

    Private ReadOnly _conversionService As New DocumentConversionService()
    Private ReadOnly _addedFilePaths As New HashSet(Of String)(StringComparer.OrdinalIgnoreCase)
    Private _isConverting As Boolean

    Private Sub MainForm_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        headerTextBox.Text = DefaultHeaderText
        watermarkTextBox.Text = DefaultWatermarkText
        ' Default to an "Output" folder next to the executable (the bin folder); it is created on first conversion.
        outputFolderTextBox.Text = Path.Combine(AppContext.BaseDirectory, DefaultOutputFolderName)

        LoadDefaultInputFiles()
    End Sub

    ''' <summary>Pre-loads the supported files found in the "input" folder next to the executable.</summary>
    Private Sub LoadDefaultInputFiles()
        Dim inputFolder As String = Path.Combine(AppContext.BaseDirectory, DefaultInputFolderName)
        If Not Directory.Exists(inputFolder) Then
            Return
        End If

        Dim files = Directory.EnumerateFiles(inputFolder).
            Where(Function(path) SupportedFileTypes.IsSupported(path)).
            OrderBy(Function(path) path, StringComparer.OrdinalIgnoreCase).
            ToList()
        If files.Count > 0 Then
            AddFiles(files)
            statusLabel.Text = $"Loaded {files.Count} file(s) from the input folder. Add more files or use Clear Files."
        End If
    End Sub

    Private Sub MainForm_FormClosing(sender As Object, e As FormClosingEventArgs) Handles MyBase.FormClosing
        If _isConverting Then
            e.Cancel = True
            MessageBox.Show(Me, "A conversion is in progress. Please wait until it finishes.", Text,
                            MessageBoxButtons.OK, MessageBoxIcon.Information)
        End If
    End Sub

#Region "Adding files"

    Private Sub DropTarget_DragEnter(sender As Object, e As DragEventArgs) _
        Handles dropPanel.DragEnter, dropLabel.DragEnter, filesGrid.DragEnter

        If Not _isConverting AndAlso e.Data IsNot Nothing AndAlso e.Data.GetDataPresent(DataFormats.FileDrop) Then
            e.Effect = DragDropEffects.Copy
        Else
            e.Effect = DragDropEffects.None
        End If
    End Sub

    Private Sub DropTarget_DragDrop(sender As Object, e As DragEventArgs) _
        Handles dropPanel.DragDrop, dropLabel.DragDrop, filesGrid.DragDrop

        If _isConverting OrElse e.Data Is Nothing Then
            Return
        End If

        Dim droppedPaths = TryCast(e.Data.GetData(DataFormats.FileDrop), String())
        If droppedPaths IsNot Nothing Then
            AddFiles(droppedPaths)
        End If
    End Sub

    Private Sub BrowseFilesButton_Click(sender As Object, e As EventArgs) Handles browseFilesButton.Click
        Using dialog As New OpenFileDialog()
            dialog.Title = "Select files to convert"
            dialog.Multiselect = True
            dialog.CheckFileExists = True
            dialog.Filter = SupportedFileTypes.BuildDialogFilter()

            If dialog.ShowDialog(Me) = DialogResult.OK Then
                AddFiles(dialog.FileNames)
            End If
        End Using
    End Sub

    ''' <summary>Shared entry point for drag-and-drop and Browse Files.</summary>
    Private Sub AddFiles(filePaths As IEnumerable(Of String))
        Dim added As Integer = 0
        Dim duplicates As New List(Of String)()
        Dim unsupported As New List(Of String)()

        For Each rawPath As String In filePaths
            If Directory.Exists(rawPath) Then
                unsupported.Add(Path.GetFileName(rawPath) & " (folders are not supported)")
                Continue For
            End If

            Dim fullPath As String = Path.GetFullPath(rawPath)
            If Not SupportedFileTypes.IsSupported(fullPath) Then
                unsupported.Add(Path.GetFileName(fullPath))
            ElseIf Not _addedFilePaths.Add(fullPath) Then
                duplicates.Add(Path.GetFileName(fullPath))
            Else
                AddGridRow(fullPath)
                added += 1
            End If
        Next

        statusLabel.Text = $"{added} file(s) added. Total in list: {filesGrid.Rows.Count}."
        ShowAddFilesWarnings(unsupported, duplicates)
    End Sub

    Private Sub AddGridRow(fullPath As String)
        Dim rowIndex As Integer = filesGrid.Rows.Add(
            Path.GetFileName(fullPath),
            SupportedFileTypes.GetFormat(fullPath).ToString(),
            fullPath,
            "Pending",
            String.Empty)
        filesGrid.Rows(rowIndex).Tag = fullPath
    End Sub

    Private Sub ShowAddFilesWarnings(unsupported As List(Of String), duplicates As List(Of String))
        If unsupported.Count = 0 AndAlso duplicates.Count = 0 Then
            Return
        End If

        Dim message As New StringBuilder()
        If unsupported.Count > 0 Then
            message.AppendLine("These files were not added because their type is not supported:")
            unsupported.ForEach(Sub(name) message.AppendLine("  • " & name))
            message.AppendLine($"Supported extensions: {SupportedFileTypes.GetSupportedExtensionsText()}")
        End If
        If duplicates.Count > 0 Then
            If message.Length > 0 Then message.AppendLine()
            message.AppendLine("These files are already in the list and were skipped:")
            duplicates.ForEach(Sub(name) message.AppendLine("  • " & name))
        End If

        MessageBox.Show(Me, message.ToString(), "Some files were skipped", MessageBoxButtons.OK, MessageBoxIcon.Information)
    End Sub

    Private Sub ClearFilesButton_Click(sender As Object, e As EventArgs) Handles clearFilesButton.Click
        filesGrid.Rows.Clear()
        _addedFilePaths.Clear()
        progressBar.Value = 0
        statusLabel.Text = "Ready"
    End Sub

#End Region

#Region "Options"

    Private Sub BrowseOutputFolderButton_Click(sender As Object, e As EventArgs) Handles browseOutputFolderButton.Click
        Using dialog As New FolderBrowserDialog()
            dialog.Description = "Select the folder for the generated PDF files"
            dialog.ShowNewFolderButton = True
            If Directory.Exists(outputFolderTextBox.Text) Then
                dialog.SelectedPath = outputFolderTextBox.Text
            End If

            If dialog.ShowDialog(Me) = DialogResult.OK Then
                outputFolderTextBox.Text = dialog.SelectedPath
            End If
        End Using
    End Sub

    Private Sub AddHeaderCheckBox_CheckedChanged(sender As Object, e As EventArgs) Handles addHeaderCheckBox.CheckedChanged
        headerTextBox.Enabled = addHeaderCheckBox.Checked
    End Sub

    Private Sub AddWatermarkCheckBox_CheckedChanged(sender As Object, e As EventArgs) Handles addWatermarkCheckBox.CheckedChanged
        watermarkTextBox.Enabled = addWatermarkCheckBox.Checked
    End Sub

    Private Sub OpenOutputFolderButton_Click(sender As Object, e As EventArgs) Handles openOutputFolderButton.Click
        Dim folder As String = outputFolderTextBox.Text.Trim()
        If folder.Length = 0 OrElse Not Directory.Exists(folder) Then
            MessageBox.Show(Me, "The output folder does not exist yet. Convert at least one file first or choose an existing folder.",
                            Text, MessageBoxButtons.OK, MessageBoxIcon.Information)
            Return
        End If

        Process.Start(New ProcessStartInfo("explorer.exe", $"""{folder}"""))
    End Sub

#End Region

#Region "Conversion"

    Private Async Sub ConvertButton_Click(sender As Object, e As EventArgs) Handles convertButton.Click
        Dim validationMessage As String = ValidateInputs()
        If validationMessage.Length > 0 Then
            MessageBox.Show(Me, validationMessage, "Cannot convert", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Return
        End If

        Dim outputFolder As String = outputFolderTextBox.Text.Trim()
        Try
            Directory.CreateDirectory(outputFolder)
        Catch ex As Exception When TypeOf ex Is IOException OrElse TypeOf ex Is UnauthorizedAccessException OrElse TypeOf ex Is ArgumentException OrElse TypeOf ex Is NotSupportedException
            MessageBox.Show(Me, $"The output folder could not be created: {ex.Message}", "Cannot convert",
                            MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Return
        End Try

        SetConverting(True)
        Try
            Dim summary As ConversionSummary = Await ConvertAllFilesAsync(outputFolder)
            ShowSummary(summary, outputFolder)
        Finally
            SetConverting(False)
        End Try
    End Sub

    Private Async Function ConvertAllFilesAsync(outputFolder As String) As Task(Of ConversionSummary)
        Dim total As Integer = filesGrid.Rows.Count
        Dim summary As New ConversionSummary With {.Total = total}

        progressBar.Minimum = 0
        progressBar.Maximum = Math.Max(total, 1)
        progressBar.Value = 0

        For Each row As DataGridViewRow In filesGrid.Rows
            row.Cells(statusColumn.Index).Value = "Waiting"
            row.Cells(outputPathColumn.Index).Value = String.Empty
            row.DefaultCellStyle.ForeColor = SystemColors.ControlText
        Next

        For index As Integer = 0 To total - 1
            Dim row As DataGridViewRow = filesGrid.Rows(index)
            Dim options As ConversionOptions = BuildOptions(CStr(row.Tag), outputFolder)

            row.Cells(statusColumn.Index).Value = "Converting..."
            statusLabel.Text = $"Converting {index + 1} of {total}: {Path.GetFileName(options.InputFilePath)}"

            Dim result As ConversionResult = Await Task.Run(Function() _conversionService.ConvertFile(options))
            ApplyResultToRow(row, result)

            If result.Success Then summary.Succeeded += 1 Else summary.Failed += 1
            progressBar.Value = index + 1
        Next

        Return summary
    End Function

    Private Function BuildOptions(inputFilePath As String, outputFolder As String) As ConversionOptions
        Dim pdfFileName As String = Path.GetFileNameWithoutExtension(inputFilePath) & ".pdf"
        Return New ConversionOptions With {
            .InputFilePath = inputFilePath,
            .OutputFilePath = Path.Combine(outputFolder, pdfFileName),
            .AddHeader = addHeaderCheckBox.Checked,
            .HeaderText = headerTextBox.Text.Trim(),
            .AddWatermark = addWatermarkCheckBox.Checked,
            .WatermarkText = watermarkTextBox.Text.Trim()
        }
    End Function

    Private Sub ApplyResultToRow(row As DataGridViewRow, result As ConversionResult)
        If result.Success Then
            row.Cells(statusColumn.Index).Value = "Success"
            row.Cells(outputPathColumn.Index).Value = result.OutputFilePath
            row.DefaultCellStyle.ForeColor = Color.FromArgb(0, 110, 40)
        Else
            row.Cells(statusColumn.Index).Value = "Failed: " & result.Message
            row.DefaultCellStyle.ForeColor = Color.Firebrick
        End If
    End Sub

    Private Function ValidateInputs() As String
        If filesGrid.Rows.Count = 0 Then
            Return "Add at least one file to convert."
        End If
        If String.IsNullOrWhiteSpace(outputFolderTextBox.Text) Then
            Return "Select an output folder."
        End If
        If addHeaderCheckBox.Checked AndAlso String.IsNullOrWhiteSpace(headerTextBox.Text) Then
            Return "Enter the header text or clear the 'Add header' option."
        End If
        If addWatermarkCheckBox.Checked AndAlso String.IsNullOrWhiteSpace(watermarkTextBox.Text) Then
            Return "Enter the watermark text or clear the 'Add watermark' option."
        End If
        Return String.Empty
    End Function

    Private Sub SetConverting(isConverting As Boolean)
        _isConverting = isConverting

        convertButton.Enabled = Not isConverting
        browseFilesButton.Enabled = Not isConverting
        clearFilesButton.Enabled = Not isConverting
        browseOutputFolderButton.Enabled = Not isConverting
        outputFolderTextBox.Enabled = Not isConverting
        addHeaderCheckBox.Enabled = Not isConverting
        addWatermarkCheckBox.Enabled = Not isConverting
        headerTextBox.Enabled = Not isConverting AndAlso addHeaderCheckBox.Checked
        watermarkTextBox.Enabled = Not isConverting AndAlso addWatermarkCheckBox.Checked
    End Sub

    Private Sub ShowSummary(summary As ConversionSummary, outputFolder As String)
        statusLabel.Text = $"Finished: {summary.Succeeded} succeeded, {summary.Failed} failed."

        Dim message As New StringBuilder()
        message.AppendLine($"Total files: {summary.Total}")
        message.AppendLine($"Successfully converted: {summary.Succeeded}")
        message.AppendLine($"Failed: {summary.Failed}")
        message.AppendLine($"Output folder: {outputFolder}")
        If summary.Failed > 0 Then
            message.AppendLine()
            message.AppendLine($"Error details were written to: {ConversionErrorLogger.LogFilePath}")
        End If

        MessageBox.Show(Me, message.ToString(), "Conversion summary", MessageBoxButtons.OK,
                        If(summary.Failed > 0, MessageBoxIcon.Warning, MessageBoxIcon.Information))
    End Sub

    Private Class ConversionSummary
        Public Property Total As Integer
        Public Property Succeeded As Integer
        Public Property Failed As Integer
    End Class

#End Region

End Class
