Option Strict On
Option Explicit On
Option Infer On

Imports System.IO
Imports System.Text

''' <summary>Appends detailed conversion failures to conversion-errors.log next to the executable.</summary>
Public NotInheritable Class ConversionErrorLogger

    Private Shared ReadOnly SyncRoot As New Object()

    Private Sub New()
    End Sub

    Public Shared ReadOnly Property LogFilePath As String
        Get
            Return Path.Combine(AppContext.BaseDirectory, "conversion-errors.log")
        End Get
    End Property

    Public Shared Sub Write(inputFilePath As String, format As InputFormat, exceptionDetails As String, message As String)
        Dim entry As New StringBuilder()
        entry.AppendLine(New String("-"c, 80))
        entry.AppendLine("Date:      " & DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"))
        entry.AppendLine("File:      " & inputFilePath)
        entry.AppendLine("Format:    " & format.ToString())
        entry.AppendLine("Message:   " & message)
        entry.AppendLine("Details:")
        entry.AppendLine(exceptionDetails)

        Try
            SyncLock SyncRoot
                File.AppendAllText(LogFilePath, entry.ToString(), Encoding.UTF8)
            End SyncLock
        Catch ex As IOException
            ' Logging must never break the batch; the error is still shown in the file list.
        Catch ex As UnauthorizedAccessException
        End Try
    End Sub

End Class
