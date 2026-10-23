Option Strict On
Option Explicit On
Option Infer On

''' <summary>Settings that describe how a single input file is converted.</summary>
Public Class ConversionOptions

    Public Property InputFilePath As String = String.Empty

    ''' <summary>Preferred output path. The service picks a unique name if the file already exists.</summary>
    Public Property OutputFilePath As String = String.Empty

    Public Property AddHeader As Boolean

    Public Property HeaderText As String = String.Empty

    Public Property AddWatermark As Boolean

    Public Property WatermarkText As String = String.Empty

End Class
