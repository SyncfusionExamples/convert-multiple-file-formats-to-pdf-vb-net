Option Strict On
Option Explicit On
Option Infer On

''' <summary>Outcome of converting a single input file.</summary>
Public Class ConversionResult

    Public Property InputFilePath As String = String.Empty

    Public Property OutputFilePath As String = String.Empty

    Public Property Success As Boolean

    ''' <summary>Short, user-facing message shown in the file list.</summary>
    Public Property Message As String = String.Empty

    ''' <summary>Full exception text (type, message, stack trace) when conversion fails.</summary>
    Public Property ExceptionDetails As String = String.Empty

    Public Shared Function Succeeded(inputFilePath As String, outputFilePath As String) As ConversionResult
        Return New ConversionResult With {
            .InputFilePath = inputFilePath,
            .OutputFilePath = outputFilePath,
            .Success = True,
            .Message = "Converted"
        }
    End Function

    Public Shared Function Failed(inputFilePath As String, ex As Exception) As ConversionResult
        Return New ConversionResult With {
            .InputFilePath = inputFilePath,
            .Success = False,
            .Message = ex.Message,
            .ExceptionDetails = ex.ToString()
        }
    End Function

End Class
