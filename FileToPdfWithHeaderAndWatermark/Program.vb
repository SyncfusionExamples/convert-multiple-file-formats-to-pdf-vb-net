Option Strict On
Option Explicit On
Option Infer On

Friend Module Program

    <STAThread>
    Friend Sub Main()

        Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense(
            "YOUR SYNCFUSION LICENSE KEY")

        Application.EnableVisualStyles()
        Application.SetCompatibleTextRenderingDefault(False)
        Application.Run(New MainForm())

    End Sub

End Module
