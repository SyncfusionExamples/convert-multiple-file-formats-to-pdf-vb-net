Option Strict On
Option Explicit On
Option Infer On

Imports System.IO

''' <summary>The input categories the sample can convert.</summary>
Public Enum InputFormat
    Unsupported
    Word
    Excel
    PowerPoint
    Markdown
    Image
    Xps
    Html
End Enum

''' <summary>Maps file extensions to input formats. Shared by the UI and the conversion service.</summary>
Public NotInheritable Class SupportedFileTypes

    Private Shared ReadOnly ExtensionMap As New Dictionary(Of String, InputFormat)(StringComparer.OrdinalIgnoreCase) From {
        {".doc", InputFormat.Word},
        {".docx", InputFormat.Word},
        {".rtf", InputFormat.Word},
        {".xls", InputFormat.Excel},
        {".xlsx", InputFormat.Excel},
        {".ppt", InputFormat.PowerPoint},
        {".pptx", InputFormat.PowerPoint},
        {".md", InputFormat.Markdown},
        {".markdown", InputFormat.Markdown},
        {".jpg", InputFormat.Image},
        {".jpeg", InputFormat.Image},
        {".png", InputFormat.Image},
        {".bmp", InputFormat.Image},
        {".tif", InputFormat.Image},
        {".tiff", InputFormat.Image},
        {".xps", InputFormat.Xps},
        {".html", InputFormat.Html},
        {".htm", InputFormat.Html}
    }

    Private Sub New()
    End Sub

    Public Shared Function GetFormat(filePath As String) As InputFormat
        Dim format As InputFormat
        If ExtensionMap.TryGetValue(Path.GetExtension(filePath), format) Then
            Return format
        End If
        Return InputFormat.Unsupported
    End Function

    Public Shared Function IsSupported(filePath As String) As Boolean
        Return GetFormat(filePath) <> InputFormat.Unsupported
    End Function

    ''' <summary>Filter string for OpenFileDialog, e.g. "Supported files|*.doc;*.docx;...|All files|*.*".</summary>
    Public Shared Function BuildDialogFilter() As String
        Dim patterns = ExtensionMap.Keys.Select(Function(extension) "*" & extension)
        Return "Supported files|" & String.Join(";", patterns) & "|All files|*.*"
    End Function

    Public Shared Function GetSupportedExtensionsText() As String
        Return String.Join(", ", ExtensionMap.Keys)
    End Function

End Class
