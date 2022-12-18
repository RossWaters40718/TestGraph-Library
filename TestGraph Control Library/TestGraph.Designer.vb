Imports System.ComponentModel
<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class TestGraph
    Inherits System.Windows.Forms.UserControl

    'UserControl1 overrides dispose to clean up the component list.
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

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()>
    Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container()
        Me.lblHeader = New System.Windows.Forms.Label()
        Me.lblXAxis = New System.Windows.Forms.Label()
        Me.LblMouseMove = New System.Windows.Forms.Label()
        Me.GraphMenuStrip = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.PrintGraphToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.PreviewAndPrintDefaultPrinterToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.SelectPrinterAndPrintToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.SaveGraphToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.SaveGraphOnlyToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.SaveGraphWithParentFormToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.SaveGraphDataToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.RefreshGraphToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ClearGraphToolStripMenuItem1 = New System.Windows.Forms.ToolStripMenuItem()
        Me.GraphExamplesToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.BarGraph1ToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.BarGraph2ToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.LineGraphToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.LineGraph2ToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.PieChartToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.TextExampleToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.RestoreToDefaultStateToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.CloseApplicationToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.AboutToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.picYAxis = New System.Windows.Forms.PictureBox()
        Me.GraphMenuStrip.SuspendLayout()
        CType(Me.picYAxis, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'lblHeader
        '
        Me.lblHeader.AutoSize = True
        Me.lblHeader.Font = New System.Drawing.Font("Times New Roman", 21.75!, CType((System.Drawing.FontStyle.Bold Or System.Drawing.FontStyle.Italic), System.Drawing.FontStyle), System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblHeader.ForeColor = System.Drawing.Color.DarkRed
        Me.lblHeader.Location = New System.Drawing.Point(49, 6)
        Me.lblHeader.Margin = New System.Windows.Forms.Padding(7, 0, 7, 0)
        Me.lblHeader.Name = "lblHeader"
        Me.lblHeader.Size = New System.Drawing.Size(0, 33)
        Me.lblHeader.TabIndex = 0
        Me.lblHeader.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'lblXAxis
        '
        Me.lblXAxis.Font = New System.Drawing.Font("Times New Roman", 18.0!, CType((System.Drawing.FontStyle.Bold Or System.Drawing.FontStyle.Italic), System.Drawing.FontStyle), System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblXAxis.ForeColor = System.Drawing.Color.DarkRed
        Me.lblXAxis.Location = New System.Drawing.Point(50, 359)
        Me.lblXAxis.Margin = New System.Windows.Forms.Padding(7, 0, 7, 0)
        Me.lblXAxis.Name = "lblXAxis"
        Me.lblXAxis.Size = New System.Drawing.Size(50, 10)
        Me.lblXAxis.TabIndex = 1
        Me.lblXAxis.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'LblMouseMove
        '
        Me.LblMouseMove.AutoSize = True
        Me.LblMouseMove.Location = New System.Drawing.Point(58, 41)
        Me.LblMouseMove.Name = "LblMouseMove"
        Me.LblMouseMove.Size = New System.Drawing.Size(0, 28)
        Me.LblMouseMove.TabIndex = 5
        Me.LblMouseMove.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'GraphMenuStrip
        '
        Me.GraphMenuStrip.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.PrintGraphToolStripMenuItem, Me.SaveGraphToolStripMenuItem, Me.SaveGraphDataToolStripMenuItem, Me.RefreshGraphToolStripMenuItem, Me.ClearGraphToolStripMenuItem1, Me.GraphExamplesToolStripMenuItem, Me.RestoreToDefaultStateToolStripMenuItem, Me.CloseApplicationToolStripMenuItem, Me.AboutToolStripMenuItem})
        Me.GraphMenuStrip.Name = "GraphMenuStrip"
        Me.GraphMenuStrip.Size = New System.Drawing.Size(209, 202)
        '
        'PrintGraphToolStripMenuItem
        '
        Me.PrintGraphToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.PreviewAndPrintDefaultPrinterToolStripMenuItem, Me.SelectPrinterAndPrintToolStripMenuItem})
        Me.PrintGraphToolStripMenuItem.Name = "PrintGraphToolStripMenuItem"
        Me.PrintGraphToolStripMenuItem.Size = New System.Drawing.Size(208, 22)
        Me.PrintGraphToolStripMenuItem.Text = "Print Graph As JPEG"
        '
        'PreviewAndPrintDefaultPrinterToolStripMenuItem
        '
        Me.PreviewAndPrintDefaultPrinterToolStripMenuItem.Name = "PreviewAndPrintDefaultPrinterToolStripMenuItem"
        Me.PreviewAndPrintDefaultPrinterToolStripMenuItem.Size = New System.Drawing.Size(255, 22)
        Me.PreviewAndPrintDefaultPrinterToolStripMenuItem.Text = "Preview And Print (Default Printer)"
        '
        'SelectPrinterAndPrintToolStripMenuItem
        '
        Me.SelectPrinterAndPrintToolStripMenuItem.Name = "SelectPrinterAndPrintToolStripMenuItem"
        Me.SelectPrinterAndPrintToolStripMenuItem.Size = New System.Drawing.Size(255, 22)
        Me.SelectPrinterAndPrintToolStripMenuItem.Text = "Select Printer And Print"
        '
        'SaveGraphToolStripMenuItem
        '
        Me.SaveGraphToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.SaveGraphOnlyToolStripMenuItem, Me.SaveGraphWithParentFormToolStripMenuItem})
        Me.SaveGraphToolStripMenuItem.Name = "SaveGraphToolStripMenuItem"
        Me.SaveGraphToolStripMenuItem.Size = New System.Drawing.Size(208, 22)
        Me.SaveGraphToolStripMenuItem.Text = "Save Graph As JPEG"
        '
        'SaveGraphOnlyToolStripMenuItem
        '
        Me.SaveGraphOnlyToolStripMenuItem.Name = "SaveGraphOnlyToolStripMenuItem"
        Me.SaveGraphOnlyToolStripMenuItem.Size = New System.Drawing.Size(229, 22)
        Me.SaveGraphOnlyToolStripMenuItem.Text = "Save Graph Only"
        '
        'SaveGraphWithParentFormToolStripMenuItem
        '
        Me.SaveGraphWithParentFormToolStripMenuItem.Name = "SaveGraphWithParentFormToolStripMenuItem"
        Me.SaveGraphWithParentFormToolStripMenuItem.Size = New System.Drawing.Size(229, 22)
        Me.SaveGraphWithParentFormToolStripMenuItem.Text = "Save Graph With Parent Form"
        '
        'SaveGraphDataToolStripMenuItem
        '
        Me.SaveGraphDataToolStripMenuItem.Name = "SaveGraphDataToolStripMenuItem"
        Me.SaveGraphDataToolStripMenuItem.Size = New System.Drawing.Size(208, 22)
        Me.SaveGraphDataToolStripMenuItem.Text = "Rename Or Save Data File"
        '
        'RefreshGraphToolStripMenuItem
        '
        Me.RefreshGraphToolStripMenuItem.Name = "RefreshGraphToolStripMenuItem"
        Me.RefreshGraphToolStripMenuItem.Size = New System.Drawing.Size(208, 22)
        Me.RefreshGraphToolStripMenuItem.Text = "Refresh Graph"
        '
        'ClearGraphToolStripMenuItem1
        '
        Me.ClearGraphToolStripMenuItem1.Name = "ClearGraphToolStripMenuItem1"
        Me.ClearGraphToolStripMenuItem1.Size = New System.Drawing.Size(208, 22)
        Me.ClearGraphToolStripMenuItem1.Text = "Clear Graph"
        '
        'GraphExamplesToolStripMenuItem
        '
        Me.GraphExamplesToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.BarGraph1ToolStripMenuItem, Me.BarGraph2ToolStripMenuItem, Me.LineGraphToolStripMenuItem, Me.LineGraph2ToolStripMenuItem, Me.PieChartToolStripMenuItem, Me.TextExampleToolStripMenuItem})
        Me.GraphExamplesToolStripMenuItem.Name = "GraphExamplesToolStripMenuItem"
        Me.GraphExamplesToolStripMenuItem.Size = New System.Drawing.Size(208, 22)
        Me.GraphExamplesToolStripMenuItem.Text = "Graph Examples"
        '
        'BarGraph1ToolStripMenuItem
        '
        Me.BarGraph1ToolStripMenuItem.Name = "BarGraph1ToolStripMenuItem"
        Me.BarGraph1ToolStripMenuItem.Size = New System.Drawing.Size(143, 22)
        Me.BarGraph1ToolStripMenuItem.Text = "Bar Graph 1"
        '
        'BarGraph2ToolStripMenuItem
        '
        Me.BarGraph2ToolStripMenuItem.Name = "BarGraph2ToolStripMenuItem"
        Me.BarGraph2ToolStripMenuItem.Size = New System.Drawing.Size(143, 22)
        Me.BarGraph2ToolStripMenuItem.Text = "Bar Graph 2"
        '
        'LineGraphToolStripMenuItem
        '
        Me.LineGraphToolStripMenuItem.Name = "LineGraphToolStripMenuItem"
        Me.LineGraphToolStripMenuItem.Size = New System.Drawing.Size(143, 22)
        Me.LineGraphToolStripMenuItem.Text = "Line Graph 1"
        '
        'LineGraph2ToolStripMenuItem
        '
        Me.LineGraph2ToolStripMenuItem.Name = "LineGraph2ToolStripMenuItem"
        Me.LineGraph2ToolStripMenuItem.Size = New System.Drawing.Size(143, 22)
        Me.LineGraph2ToolStripMenuItem.Text = "Line Graph 2"
        '
        'PieChartToolStripMenuItem
        '
        Me.PieChartToolStripMenuItem.Name = "PieChartToolStripMenuItem"
        Me.PieChartToolStripMenuItem.Size = New System.Drawing.Size(143, 22)
        Me.PieChartToolStripMenuItem.Text = "Pie Chart"
        '
        'TextExampleToolStripMenuItem
        '
        Me.TextExampleToolStripMenuItem.Name = "TextExampleToolStripMenuItem"
        Me.TextExampleToolStripMenuItem.Size = New System.Drawing.Size(143, 22)
        Me.TextExampleToolStripMenuItem.Text = "Text Example"
        '
        'RestoreToDefaultStateToolStripMenuItem
        '
        Me.RestoreToDefaultStateToolStripMenuItem.Name = "RestoreToDefaultStateToolStripMenuItem"
        Me.RestoreToDefaultStateToolStripMenuItem.Size = New System.Drawing.Size(208, 22)
        Me.RestoreToDefaultStateToolStripMenuItem.Text = "Restore To Default State"
        '
        'CloseApplicationToolStripMenuItem
        '
        Me.CloseApplicationToolStripMenuItem.Name = "CloseApplicationToolStripMenuItem"
        Me.CloseApplicationToolStripMenuItem.Size = New System.Drawing.Size(208, 22)
        Me.CloseApplicationToolStripMenuItem.Text = "Close Application"
        '
        'AboutToolStripMenuItem
        '
        Me.AboutToolStripMenuItem.Name = "AboutToolStripMenuItem"
        Me.AboutToolStripMenuItem.Size = New System.Drawing.Size(208, 22)
        Me.AboutToolStripMenuItem.Text = "About"
        '
        'picYAxis
        '
        Me.picYAxis.Location = New System.Drawing.Point(0, 0)
        Me.picYAxis.Name = "picYAxis"
        Me.picYAxis.Size = New System.Drawing.Size(10, 10)
        Me.picYAxis.TabIndex = 6
        Me.picYAxis.TabStop = False
        '
        'TestGraph
        '
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None
        Me.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
        Me.BackColor = System.Drawing.Color.DimGray
        Me.ContextMenuStrip = Me.GraphMenuStrip
        Me.Controls.Add(Me.picYAxis)
        Me.Controls.Add(Me.LblMouseMove)
        Me.Controls.Add(Me.lblXAxis)
        Me.Controls.Add(Me.lblHeader)
        Me.Font = New System.Drawing.Font("Times New Roman", 18.0!, CType((System.Drawing.FontStyle.Bold Or System.Drawing.FontStyle.Italic), System.Drawing.FontStyle), System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ForeColor = System.Drawing.Color.Khaki
        Me.Margin = New System.Windows.Forms.Padding(7, 6, 7, 6)
        Me.Name = "TestGraph"
        Me.Size = New System.Drawing.Size(100, 50)
        Me.GraphMenuStrip.ResumeLayout(False)
        CType(Me.picYAxis, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Private Sub TestGraphCtl_BackColorChanged(sender As Object, e As EventArgs) Handles Me.BackColorChanged
        lblHeader.BackColor = Me.BackColor
        lblXAxis.BackColor = Me.BackColor
    End Sub
    Private Sub TestGraphCtl_FontChanged(sender As Object, e As EventArgs) Handles Me.FontChanged
        lblHeader.Font = Me.Font
        lblHeader.Height = Me.Font.Size
        lblXAxis.Font = Me.Font
        lblXAxis.Height = Me.Font.Size
    End Sub

    Friend WithEvents lblHeader As Label
    Friend WithEvents lblXAxis As Label

    Public Sub New()
        'Add any initialization after the InitializeComponent() call
        ' This call is required by the designer.
        InitializeComponent()
    End Sub
    Protected Overrides Sub Finalize()
        MyBase.Finalize()
    End Sub

    Friend WithEvents LblMouseMove As Label
    Friend WithEvents GraphMenuStrip As ContextMenuStrip
    Friend WithEvents ClearGraphToolStripMenuItem1 As ToolStripMenuItem
    Friend WithEvents SaveGraphToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents PrintGraphToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents AboutToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents CloseApplicationToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents SaveGraphOnlyToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents SaveGraphWithParentFormToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents PreviewAndPrintDefaultPrinterToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents SelectPrinterAndPrintToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents RestoreToDefaultStateToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents RefreshGraphToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents GraphExamplesToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents BarGraph1ToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents BarGraph2ToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents LineGraphToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents PieChartToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents TextExampleToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents LineGraph2ToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents SaveGraphDataToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents picYAxis As PictureBox
End Class