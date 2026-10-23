Option Strict On
Option Explicit On
Option Infer On

Friend Module Program

    <STAThread>
    Friend Sub Main()
        ' Replace the placeholder with your Syncfusion license key (never commit a real key).
        Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense(
            "YOUR SYNCFUSION LICENSE KEY")

        Application.EnableVisualStyles()
        Application.SetCompatibleTextRenderingDefault(False)
        Application.SetHighDpiMode(HighDpiMode.SystemAware)
        Application.Run(New MainForm())
    End Sub

End Module
