<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class Form1
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> _
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
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container()
        Me.Button1 = New System.Windows.Forms.Button()
        Me.Button2 = New System.Windows.Forms.Button()
        Me.Button3 = New System.Windows.Forms.Button()
        Me.Button4 = New System.Windows.Forms.Button()
        Me.Button5 = New System.Windows.Forms.Button()
        Me.Button6 = New System.Windows.Forms.Button()
        Me.TestGraph1 = New TestGraphControl.TestGraph()
        Me.SuspendLayout()
        '
        'Button1
        '
        Me.Button1.Location = New System.Drawing.Point(1135, 26)
        Me.Button1.Name = "Button1"
        Me.Button1.Size = New System.Drawing.Size(80, 25)
        Me.Button1.TabIndex = 1
        Me.Button1.Text = "Bar 1"
        Me.Button1.UseVisualStyleBackColor = True
        '
        'Button2
        '
        Me.Button2.Location = New System.Drawing.Point(1135, 64)
        Me.Button2.Name = "Button2"
        Me.Button2.Size = New System.Drawing.Size(80, 25)
        Me.Button2.TabIndex = 3
        Me.Button2.Text = "Bar 2"
        Me.Button2.UseVisualStyleBackColor = True
        '
        'Button3
        '
        Me.Button3.Location = New System.Drawing.Point(1135, 104)
        Me.Button3.Name = "Button3"
        Me.Button3.Size = New System.Drawing.Size(80, 25)
        Me.Button3.TabIndex = 4
        Me.Button3.Text = "Line 1"
        Me.Button3.UseVisualStyleBackColor = True
        '
        'Button4
        '
        Me.Button4.Location = New System.Drawing.Point(1135, 144)
        Me.Button4.Name = "Button4"
        Me.Button4.Size = New System.Drawing.Size(80, 25)
        Me.Button4.TabIndex = 5
        Me.Button4.Text = "Line 2"
        Me.Button4.UseVisualStyleBackColor = True
        '
        'Button5
        '
        Me.Button5.Location = New System.Drawing.Point(1135, 184)
        Me.Button5.Name = "Button5"
        Me.Button5.Size = New System.Drawing.Size(80, 25)
        Me.Button5.TabIndex = 6
        Me.Button5.Text = "Pie"
        Me.Button5.UseVisualStyleBackColor = True
        '
        'Button6
        '
        Me.Button6.Location = New System.Drawing.Point(1135, 225)
        Me.Button6.Name = "Button6"
        Me.Button6.Size = New System.Drawing.Size(80, 25)
        Me.Button6.TabIndex = 7
        Me.Button6.Text = "Text"
        Me.Button6.UseVisualStyleBackColor = True
        '
        'TestGraph1
        '
        Me.TestGraph1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
        Me.TestGraph1.BackColor = System.Drawing.Color.DimGray
        Me.TestGraph1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.TestGraph1.Font = New System.Drawing.Font("Times New Roman", 18.0!, CType((System.Drawing.FontStyle.Bold Or System.Drawing.FontStyle.Italic), System.Drawing.FontStyle), System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TestGraph1.ForeColor = System.Drawing.Color.Khaki
        Me.TestGraph1.GridLabelFont = New System.Drawing.Font("Times New Roman", 9.0!)
        Me.TestGraph1.Location = New System.Drawing.Point(3, 2)
        Me.TestGraph1.Margin = New System.Windows.Forms.Padding(7, 6, 7, 6)
        Me.TestGraph1.Name = "TestGraph1"
        Me.TestGraph1.RotateXGridLabels = 0!
        Me.TestGraph1.RotateYGridLabels = 0!
        Me.TestGraph1.ScaleX_Left = 0!
        Me.TestGraph1.ScaleX_Right = 50.0!
        Me.TestGraph1.ScaleY_Bottom = 0!
        Me.TestGraph1.ScaleY_Top = 50.0!
        Me.TestGraph1.Size = New System.Drawing.Size(1122, 586)
        Me.TestGraph1.TabIndex = 8
        '
        'Form1
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(1241, 593)
        Me.Controls.Add(Me.TestGraph1)
        Me.Controls.Add(Me.Button6)
        Me.Controls.Add(Me.Button5)
        Me.Controls.Add(Me.Button4)
        Me.Controls.Add(Me.Button3)
        Me.Controls.Add(Me.Button2)
        Me.Controls.Add(Me.Button1)
        Me.Name = "Form1"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Form1"
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents Button1 As Button
    Friend WithEvents Button2 As Button
    Friend WithEvents Button3 As Button
    Friend WithEvents Button4 As Button
    Friend WithEvents Button5 As Button
    Friend WithEvents Button6 As Button
    Friend WithEvents TestGraph1 As TestGraphControl.TestGraph
End Class
