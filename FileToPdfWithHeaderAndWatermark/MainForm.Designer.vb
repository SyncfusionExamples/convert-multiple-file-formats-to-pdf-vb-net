<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class MainForm
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()>
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    Private components As System.ComponentModel.IContainer

    <System.Diagnostics.DebuggerStepThrough()>
    Private Sub InitializeComponent()
        Me.dropPanel = New System.Windows.Forms.Panel()
        Me.dropLabel = New System.Windows.Forms.Label()
        Me.browseFilesButton = New System.Windows.Forms.Button()
        Me.clearFilesButton = New System.Windows.Forms.Button()
        Me.filesGrid = New System.Windows.Forms.DataGridView()
        Me.fileNameColumn = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.fileTypeColumn = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.inputPathColumn = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.statusColumn = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.outputPathColumn = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.outputFolderLabel = New System.Windows.Forms.Label()
        Me.outputFolderTextBox = New System.Windows.Forms.TextBox()
        Me.browseOutputFolderButton = New System.Windows.Forms.Button()
        Me.optionsGroupBox = New System.Windows.Forms.GroupBox()
        Me.addHeaderCheckBox = New System.Windows.Forms.CheckBox()
        Me.headerTextBox = New System.Windows.Forms.TextBox()
        Me.addWatermarkCheckBox = New System.Windows.Forms.CheckBox()
        Me.watermarkTextBox = New System.Windows.Forms.TextBox()
        Me.convertButton = New System.Windows.Forms.Button()
        Me.openOutputFolderButton = New System.Windows.Forms.Button()
        Me.progressBar = New System.Windows.Forms.ProgressBar()
        Me.statusLabel = New System.Windows.Forms.Label()
        Me.dropPanel.SuspendLayout()
        CType(Me.filesGrid, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.optionsGroupBox.SuspendLayout()
        Me.SuspendLayout()
        '
        'dropPanel
        '
        Me.dropPanel.AllowDrop = True
        Me.dropPanel.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.dropPanel.BackColor = System.Drawing.Color.FromArgb(CType(CType(240, Byte), Integer), CType(CType(246, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.dropPanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.dropPanel.Controls.Add(Me.dropLabel)
        Me.dropPanel.Location = New System.Drawing.Point(12, 12)
        Me.dropPanel.Name = "dropPanel"
        Me.dropPanel.Size = New System.Drawing.Size(860, 80)
        Me.dropPanel.TabIndex = 0
        '
        'dropLabel
        '
        Me.dropLabel.AllowDrop = True
        Me.dropLabel.Dock = System.Windows.Forms.DockStyle.Fill
        Me.dropLabel.Font = New System.Drawing.Font("Segoe UI", 11.0!)
        Me.dropLabel.ForeColor = System.Drawing.Color.FromArgb(CType(CType(40, Byte), Integer), CType(CType(80, Byte), Integer), CType(CType(140, Byte), Integer))
        Me.dropLabel.Name = "dropLabel"
        Me.dropLabel.Text = "Drop Word, Excel, PowerPoint, Markdown, image, XPS, or HTML files here"
        Me.dropLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'browseFilesButton
        '
        Me.browseFilesButton.Location = New System.Drawing.Point(12, 100)
        Me.browseFilesButton.Name = "browseFilesButton"
        Me.browseFilesButton.Size = New System.Drawing.Size(120, 30)
        Me.browseFilesButton.TabIndex = 1
        Me.browseFilesButton.Text = "Browse Files"
        Me.browseFilesButton.UseVisualStyleBackColor = True
        '
        'clearFilesButton
        '
        Me.clearFilesButton.Location = New System.Drawing.Point(140, 100)
        Me.clearFilesButton.Name = "clearFilesButton"
        Me.clearFilesButton.Size = New System.Drawing.Size(120, 30)
        Me.clearFilesButton.TabIndex = 2
        Me.clearFilesButton.Text = "Clear Files"
        Me.clearFilesButton.UseVisualStyleBackColor = True
        '
        'filesGrid
        '
        Me.filesGrid.AllowDrop = True
        Me.filesGrid.AllowUserToAddRows = False
        Me.filesGrid.AllowUserToDeleteRows = False
        Me.filesGrid.AllowUserToResizeRows = False
        Me.filesGrid.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.filesGrid.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill
        Me.filesGrid.BackgroundColor = System.Drawing.SystemColors.Window
        Me.filesGrid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.filesGrid.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.fileNameColumn, Me.fileTypeColumn, Me.inputPathColumn, Me.statusColumn, Me.outputPathColumn})
        Me.filesGrid.Location = New System.Drawing.Point(12, 138)
        Me.filesGrid.Name = "filesGrid"
        Me.filesGrid.ReadOnly = True
        Me.filesGrid.RowHeadersVisible = False
        Me.filesGrid.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect
        Me.filesGrid.Size = New System.Drawing.Size(860, 250)
        Me.filesGrid.TabIndex = 3
        '
        'fileNameColumn
        '
        Me.fileNameColumn.FillWeight = 18.0!
        Me.fileNameColumn.HeaderText = "File name"
        Me.fileNameColumn.Name = "fileNameColumn"
        Me.fileNameColumn.ReadOnly = True
        '
        'fileTypeColumn
        '
        Me.fileTypeColumn.FillWeight = 10.0!
        Me.fileTypeColumn.HeaderText = "File type"
        Me.fileTypeColumn.Name = "fileTypeColumn"
        Me.fileTypeColumn.ReadOnly = True
        '
        'inputPathColumn
        '
        Me.inputPathColumn.FillWeight = 28.0!
        Me.inputPathColumn.HeaderText = "Input path"
        Me.inputPathColumn.Name = "inputPathColumn"
        Me.inputPathColumn.ReadOnly = True
        '
        'statusColumn
        '
        Me.statusColumn.FillWeight = 16.0!
        Me.statusColumn.HeaderText = "Status"
        Me.statusColumn.Name = "statusColumn"
        Me.statusColumn.ReadOnly = True
        '
        'outputPathColumn
        '
        Me.outputPathColumn.FillWeight = 28.0!
        Me.outputPathColumn.HeaderText = "Generated PDF"
        Me.outputPathColumn.Name = "outputPathColumn"
        Me.outputPathColumn.ReadOnly = True
        '
        'outputFolderLabel
        '
        Me.outputFolderLabel.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.outputFolderLabel.AutoSize = True
        Me.outputFolderLabel.Location = New System.Drawing.Point(12, 405)
        Me.outputFolderLabel.Name = "outputFolderLabel"
        Me.outputFolderLabel.Size = New System.Drawing.Size(81, 15)
        Me.outputFolderLabel.TabIndex = 4
        Me.outputFolderLabel.Text = "Output Folder:"
        '
        'outputFolderTextBox
        '
        Me.outputFolderTextBox.Anchor = CType(((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.outputFolderTextBox.Location = New System.Drawing.Point(110, 402)
        Me.outputFolderTextBox.Name = "outputFolderTextBox"
        Me.outputFolderTextBox.Size = New System.Drawing.Size(610, 23)
        Me.outputFolderTextBox.TabIndex = 5
        '
        'browseOutputFolderButton
        '
        Me.browseOutputFolderButton.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.browseOutputFolderButton.Location = New System.Drawing.Point(728, 400)
        Me.browseOutputFolderButton.Name = "browseOutputFolderButton"
        Me.browseOutputFolderButton.Size = New System.Drawing.Size(144, 27)
        Me.browseOutputFolderButton.TabIndex = 6
        Me.browseOutputFolderButton.Text = "Browse Output Folder"
        Me.browseOutputFolderButton.UseVisualStyleBackColor = True
        '
        'optionsGroupBox
        '
        Me.optionsGroupBox.Anchor = CType(((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.optionsGroupBox.Controls.Add(Me.addHeaderCheckBox)
        Me.optionsGroupBox.Controls.Add(Me.headerTextBox)
        Me.optionsGroupBox.Controls.Add(Me.addWatermarkCheckBox)
        Me.optionsGroupBox.Controls.Add(Me.watermarkTextBox)
        Me.optionsGroupBox.Location = New System.Drawing.Point(12, 435)
        Me.optionsGroupBox.Name = "optionsGroupBox"
        Me.optionsGroupBox.Size = New System.Drawing.Size(860, 80)
        Me.optionsGroupBox.TabIndex = 7
        Me.optionsGroupBox.TabStop = False
        Me.optionsGroupBox.Text = "PDF options"
        '
        'addHeaderCheckBox
        '
        Me.addHeaderCheckBox.AutoSize = True
        Me.addHeaderCheckBox.Location = New System.Drawing.Point(12, 25)
        Me.addHeaderCheckBox.Name = "addHeaderCheckBox"
        Me.addHeaderCheckBox.Size = New System.Drawing.Size(88, 19)
        Me.addHeaderCheckBox.TabIndex = 0
        Me.addHeaderCheckBox.Text = "Add header"
        Me.addHeaderCheckBox.UseVisualStyleBackColor = True
        '
        'headerTextBox
        '
        Me.headerTextBox.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.headerTextBox.Enabled = False
        Me.headerTextBox.Location = New System.Drawing.Point(140, 23)
        Me.headerTextBox.Name = "headerTextBox"
        Me.headerTextBox.Size = New System.Drawing.Size(706, 23)
        Me.headerTextBox.TabIndex = 1
        Me.headerTextBox.Text = "Confidential Document"
        '
        'addWatermarkCheckBox
        '
        Me.addWatermarkCheckBox.AutoSize = True
        Me.addWatermarkCheckBox.Location = New System.Drawing.Point(12, 53)
        Me.addWatermarkCheckBox.Name = "addWatermarkCheckBox"
        Me.addWatermarkCheckBox.Size = New System.Drawing.Size(113, 19)
        Me.addWatermarkCheckBox.TabIndex = 2
        Me.addWatermarkCheckBox.Text = "Add watermark"
        Me.addWatermarkCheckBox.UseVisualStyleBackColor = True
        '
        'watermarkTextBox
        '
        Me.watermarkTextBox.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.watermarkTextBox.Enabled = False
        Me.watermarkTextBox.Location = New System.Drawing.Point(140, 51)
        Me.watermarkTextBox.Name = "watermarkTextBox"
        Me.watermarkTextBox.Size = New System.Drawing.Size(706, 23)
        Me.watermarkTextBox.TabIndex = 3
        Me.watermarkTextBox.Text = "CONFIDENTIAL"
        '
        'convertButton
        '
        Me.convertButton.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.convertButton.Font = New System.Drawing.Font("Segoe UI", 9.0!, System.Drawing.FontStyle.Bold)
        Me.convertButton.Location = New System.Drawing.Point(12, 525)
        Me.convertButton.Name = "convertButton"
        Me.convertButton.Size = New System.Drawing.Size(140, 32)
        Me.convertButton.TabIndex = 8
        Me.convertButton.Text = "Convert to PDF"
        Me.convertButton.UseVisualStyleBackColor = True
        '
        'openOutputFolderButton
        '
        Me.openOutputFolderButton.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.openOutputFolderButton.Location = New System.Drawing.Point(160, 525)
        Me.openOutputFolderButton.Name = "openOutputFolderButton"
        Me.openOutputFolderButton.Size = New System.Drawing.Size(140, 32)
        Me.openOutputFolderButton.TabIndex = 9
        Me.openOutputFolderButton.Text = "Open Output Folder"
        Me.openOutputFolderButton.UseVisualStyleBackColor = True
        '
        'progressBar
        '
        Me.progressBar.Anchor = CType(((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.progressBar.Location = New System.Drawing.Point(12, 568)
        Me.progressBar.Name = "progressBar"
        Me.progressBar.Size = New System.Drawing.Size(860, 20)
        Me.progressBar.TabIndex = 10
        '
        'statusLabel
        '
        Me.statusLabel.Anchor = CType(((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.statusLabel.Location = New System.Drawing.Point(12, 595)
        Me.statusLabel.Name = "statusLabel"
        Me.statusLabel.Size = New System.Drawing.Size(860, 20)
        Me.statusLabel.TabIndex = 11
        Me.statusLabel.Text = "Ready"
        '
        'MainForm
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(7.0!, 15.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(884, 621)
        Me.Controls.Add(Me.statusLabel)
        Me.Controls.Add(Me.progressBar)
        Me.Controls.Add(Me.openOutputFolderButton)
        Me.Controls.Add(Me.convertButton)
        Me.Controls.Add(Me.optionsGroupBox)
        Me.Controls.Add(Me.browseOutputFolderButton)
        Me.Controls.Add(Me.outputFolderTextBox)
        Me.Controls.Add(Me.outputFolderLabel)
        Me.Controls.Add(Me.filesGrid)
        Me.Controls.Add(Me.clearFilesButton)
        Me.Controls.Add(Me.browseFilesButton)
        Me.Controls.Add(Me.dropPanel)
        Me.MinimumSize = New System.Drawing.Size(700, 600)
        Me.Name = "MainForm"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "File to PDF with Header and Watermark"
        Me.dropPanel.ResumeLayout(False)
        CType(Me.filesGrid, System.ComponentModel.ISupportInitialize).EndInit()
        Me.optionsGroupBox.ResumeLayout(False)
        Me.optionsGroupBox.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents dropPanel As System.Windows.Forms.Panel
    Friend WithEvents dropLabel As System.Windows.Forms.Label
    Friend WithEvents browseFilesButton As System.Windows.Forms.Button
    Friend WithEvents clearFilesButton As System.Windows.Forms.Button
    Friend WithEvents filesGrid As System.Windows.Forms.DataGridView
    Friend WithEvents fileNameColumn As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents fileTypeColumn As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents inputPathColumn As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents statusColumn As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents outputPathColumn As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents outputFolderLabel As System.Windows.Forms.Label
    Friend WithEvents outputFolderTextBox As System.Windows.Forms.TextBox
    Friend WithEvents browseOutputFolderButton As System.Windows.Forms.Button
    Friend WithEvents optionsGroupBox As System.Windows.Forms.GroupBox
    Friend WithEvents addHeaderCheckBox As System.Windows.Forms.CheckBox
    Friend WithEvents headerTextBox As System.Windows.Forms.TextBox
    Friend WithEvents addWatermarkCheckBox As System.Windows.Forms.CheckBox
    Friend WithEvents watermarkTextBox As System.Windows.Forms.TextBox
    Friend WithEvents convertButton As System.Windows.Forms.Button
    Friend WithEvents openOutputFolderButton As System.Windows.Forms.Button
    Friend WithEvents progressBar As System.Windows.Forms.ProgressBar
    Friend WithEvents statusLabel As System.Windows.Forms.Label

End Class
