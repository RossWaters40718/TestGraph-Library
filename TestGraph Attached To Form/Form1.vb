Imports System.Math
Imports System.Drawing.Drawing2D
Public Class Form1
    Dim tg As New TestGraph()
    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Me.Controls.Add(tg)
        tg.Height = Me.ClientSize.Height
        tg.Width = Me.ClientSize.Width - 1.3 * Button1.Width
    End Sub
    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Dim tg As New TestGraph
        tg.ClearGraphics()
        tg.ShowMousePosition = False
        tg.ShowXGridLines = True
        tg.ShowYGridLines = True
        tg.ShowYLabels = True
        tg.RotateXGridLabels = -15
        tg.RotateYGridLabels = -45
        'Control Font Used For HeaderText, XAxisText And YAxisText
        tg.Font = New Font("Times New Roman", 22, FontStyle.Bold Or FontStyle.Italic)
        tg.HeaderText = "Linear Bar Graph With Random Hatch Styles And Custom Labeling"
        tg.XAxisText = "Months Of 2021"
        tg.YAxisText = "Sales Projection"
        'Imuns Must Be Referenced Using Control Name 
        tg.GraphStyle = TestGraph.GraphStyleChoices.Bar
        tg.GraphBackColor = Color.DimGray
        tg.GraphForeColor = Color.Khaki
        tg.GraphFillColor = Color.Black
        tg.GridLineColor = Color.LightGray
        tg.SetViewportScale(0, 12, TestGraph.ScaleStyleChoices.Linear, 30, 0, TestGraph.ScaleStyleChoices.Linear)
        '******How To Display Custom Labels Along X Grid*******
        'XScaleStyle Has To Be Set To Linear
        'This Method Is Not Available For Log Functions
        tg.NumOfXGridLines = 12
        tg.ShowCustomXLabels = True
        'Index Custom Labels Array With NumOfXGridLines
        Dim ArrayOfLabels(tg.NumOfXGridLines)
        'Populate A String Array With Custom X Grid Line Labels
        ArrayOfLabels(0) = "Sales For January"
        ArrayOfLabels(1) = "Sales For February"
        ArrayOfLabels(2) = "Sales For March"
        ArrayOfLabels(3) = "Sales For April"
        ArrayOfLabels(4) = "Sales For May"
        ArrayOfLabels(5) = "Sales For June"
        ArrayOfLabels(6) = "Sales For July"
        ArrayOfLabels(7) = "Sales For August"
        ArrayOfLabels(8) = "Sales For September"
        ArrayOfLabels(9) = "Sales For October"
        ArrayOfLabels(10) = "Sales For November"
        ArrayOfLabels(11) = "Sales For December"
        'Call The Method
        'This Method Is Not Available For X Log Functions
        'This Method Automatically Sets ShowXLabels = True) And (ShowCustomXLabels = True)
        'Just Incase It Is Forgotten
        tg.DrawCustomXGrid_Labels(ArrayOfLabels)
        'Next Do The Same For Custom Y-Axis Grid Labels
        tg.ShowCustomYLabels = True
        tg.NumOfYGridLines = 6
        'Index Custom Labels Array With NumOfYGridLines
        ReDim ArrayOfLabels(0)
        ReDim Preserve ArrayOfLabels(tg.NumOfYGridLines)
        'Populate A String Array With Custom Y Grid Line Labels
        'Set First Y-Axis Label To Null Due To Overlapping Angles
        ArrayOfLabels(0) = ""
        ArrayOfLabels(1) = "Projection 1"
        ArrayOfLabels(2) = "Projection 2"
        ArrayOfLabels(3) = "Projection 3"
        ArrayOfLabels(4) = "Projection 4"
        ArrayOfLabels(5) = "Projection 5"
        'Call The Method
        'This Method Is Not Available For Y Log Functions
        'This Method Automatically Sets ShowYLabels = True) And (ShowCustomYLabels = True)
        'Just Incase It Is Forgotten
        tg.DrawCustomYGrid_Labels(ArrayOfLabels)
        '**************************************************************
        'Populate GridLabelFont With Font And Color For Grid Line Labeling
        tg.GridLabelColor = Color.Aqua
        tg.GridLabelFont = New Font("Times New Roman", 9, FontStyle.Regular)
        'Create New Pen For Grid Lines
        tg.GridLineColor = Color.White
        'Also Use GridLine Color FOR Line Data Color
        Dim GridLinePen As New Pen(tg.GridLineColor, 0)
        Dim Month As Integer
        'Create The Random Bar Patterns And Lable Each With Month Data
        ' HatchStyles = Ramdon
        tg.RandomHatchStyles = True
        For Month = 0 To 12 Step 1
            Dim Y1 As Integer = CInt(Math.Floor((29 - 1 + 1) * Rnd())) + 1
            Dim Y2 As Integer = Y1
            tg.DrawLine(GridLinePen, Month, Y1, Month + 1, Y2)
        Next
        Refresh()
        GridLinePen.Dispose()
    End Sub
    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        Dim tg As New TestGraph
        tg.ClearGraphics()
        tg.GraphStyle = TestGraph.GraphStyleChoices.PointToPoint
        'Control Font Used For HeaderText, XAxisText And YAxisText
        tg.Font = New Font("Times New Roman", 20, FontStyle.Bold Or FontStyle.Italic)
        tg.HeaderText = "Log(10) X-Axis And Log(10) Y-Axis Graph Example"
        tg.XAxisText = "Frequency (Hz)"
        tg.YAxisText = "Amplitude (dB)"
        tg.SetViewportScale(20, 20000, TestGraph.ScaleStyleChoices.Log10, 20, 1, TestGraph.ScaleStyleChoices.Log10)
        tg.ShowMousePosition = True
        tg.GraphBackColor = Color.LightGray
        tg.GraphForeColor = Color.DarkRed
        tg.GraphFillColor = Color.Black
        tg.GridLineColor = Color.DimGray
        tg.ShowXLabels = True
        tg.ShowXGridLines = True
        tg.RotateXGridLabels = 0
        tg.ShowCustomXLabels = False
        '*******Y=Axis*******
        'NumOfYGridLines Is Determined Automatically In Log Function
        tg.ShowYGridLines = True
        tg.ShowYLabels = True
        tg.RotateYGridLabels = 0
        tg.ShowCustomYLabels = False
        'Set Font And Color For Grid Line Labeling
        tg.GridLabelColor = Color.Black
        tg.GridLabelFont = New Font("Times New Roman", 9, FontStyle.Regular)
        'Generate 100 Sequencially Random Log Test Pattern Frequencies
        Dim Frequency() As Single = tg.GenerateLogTestPoints(tg.XScaleStyle, 20, 20000, 100)
        Dim Amplitude(Frequency.Length) As Single
        Dim Pt As Integer
        'Generate 100 Random Log amplitudes Between 1.3 To 17
        For Pt = 0 To UBound(Amplitude, 1)
            Randomize()
            Amplitude(Pt) = Floor((17 - 1.3 + 1) * Rnd()) + 1.3
        Next
        Dim LinePen As New Pen(Color.Aqua, 0)
        For Pt = 0 To UBound(Frequency) - 1
            If Pt > 0 Then tg.DrawLine(LinePen, Frequency(Pt - 1), Amplitude(Pt - 1), Frequency(Pt), Amplitude(Pt))
        Next
        Refresh()
        LinePen.Dispose()
    End Sub
    Private Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click
        Dim tg As New TestGraph
        tg.ClearGraphics()
        'All Graph Properties Are Automatically Setup For This Method.
        'Graph Options Available Are All Color Options, Font Family And Style, HeaderText And XAxisText.
        tg.GraphFillColor = Color.DimGray
        tg.GraphForeColor = Color.Black 'Sets The HeaderText And XAxisText Forecolors  
        tg.GraphBackColor = Color.LightGray
        tg.GraphStyle = TestGraph.GraphStyleChoices.Pie
        'Control Font Used For HeaderText, XAxisText And YAxisText
        tg.Font = New Font("Times New Roman", 22, FontStyle.Bold Or FontStyle.Italic)
        tg.HeaderText = "Pie Chart Example"
        tg.XAxisText = "My Company Output / Countries"
        tg.YAxisText = ""
        'Populate An Array With Pie Percentages
        'Summation Must Not Be > 100%
        Dim PiePercent(4) As Single
        PiePercent(0) = 14.8
        PiePercent(1) = 15.2
        PiePercent(2) = 29.7
        PiePercent(3) = 14.8
        PiePercent(4) = 25.3
        'Populate A SolidBrush Array With Solid Brushes
        Dim PieBrush(UBound(PiePercent)) As SolidBrush
        PieBrush(0) = New SolidBrush(Color.DarkRed)
        PieBrush(1) = New SolidBrush(Color.LimeGreen)
        PieBrush(2) = New SolidBrush(Color.DarkOrange)
        PieBrush(3) = New SolidBrush(Color.Yellow)
        PieBrush(4) = New SolidBrush(Color.Aqua)
        'Populate A String Array With Pie Labels
        Dim PieLabel(UBound(PiePercent)) As String
        PieLabel(0) = "Bangkok Factory Output = " & PiePercent(0) & "%"
        PieLabel(1) = "Vietnam Factory Output = " & PiePercent(1) & "%"
        PieLabel(2) = "China Factory Output = " & PiePercent(2) & "%"
        PieLabel(3) = "Malaysia Factory Output = " & PiePercent(3) & "%"
        PieLabel(4) = "U.S.A. Factory Output = " & PiePercent(4) & "%"
        'Populate StringFont To Use For The Labels
        Dim StringFont As New Font("Times New Roman", 16, FontStyle.Regular)
        Dim Index As Integer
        Dim RunningTotal As Single
        For Index = 0 To UBound(PiePercent)
            'All Graph Properties Are Automatically Setup For This Method.
            tg.DrawPieChart(PieBrush(Index), StringFont, PiePercent(Index), PieLabel(Index))
            RunningTotal += PiePercent(Index)
            'Summation Must Not Be > 100%
            If RunningTotal >= 100 Then Exit For
        Next
        Refresh()
        StringFont.Dispose()
        Dim br As Brush
        For Each br In PieBrush
            br.Dispose()
        Next
    End Sub
    Private Sub Button4_Click(sender As Object, e As EventArgs) Handles Button4.Click
        Dim tg As New TestGraph
        tg.ClearGraphics()
        'Control Font Used For HeaderText, XAxisText And YAxisText
        tg.Font = New Font("Times New Roman", 22, FontStyle.Bold Or FontStyle.Italic)
        tg.HeaderText = " X Axis Log(10) Bar Graph With HatchStyle = Wave"
        tg.XAxisText = "Frequency (Hz)"
        tg.YAxisText = "Amplitude (dB)"
        tg.SetViewportScale(20, 20000, TestGraph.ScaleStyleChoices.Log10, 10, -10, TestGraph.ScaleStyleChoices.Linear)
        tg.GraphBackColor = Color.LightGray
        tg.GraphForeColor = Color.DarkRed
        tg.GraphFillColor = Color.Black
        tg.GridLineColor = Color.DimGray
        tg.NumOfYGridLines = 10
        tg.ShowMousePosition = True
        tg.ShowXLabels = True
        tg.ShowYLabels = True
        tg.ShowXGridLines = True
        tg.ShowYGridLines = True
        tg.RotateXGridLabels = 0
        tg.RotateYGridLabels = 0
        tg.ShowCustomXLabels = False
        tg.ShowCustomYLabels = False
        'Populate GridLabelFont With Font And Color For Grid Line Labeling
        tg.GridLabelColor = Color.Black
        tg.GridLabelFont = New Font("Times New Roman", 9, FontStyle.Regular)
        'Set Color For Grid Lines
        tg.GridLineColor = Color.LightGray
        'Set The Graph To Display Bar Style With HatchStyle
        tg.GraphStyle = TestGraph.GraphStyleChoices.Bar
        tg.RandomHatchStyles = False
        Dim hBrush As New HatchBrush(HatchStyle.Wave, Color.Aqua, Color.White)
        'Generate 100 Log Test Pattern Frequencies
        Dim Frequency() As Single = tg.GenerateLogTestPoints(tg.XScaleStyle, 20, 20000, 100)
        Dim Amplitude(UBound(Frequency)) As Single
        Dim Pt As Integer
        'Generate 100 Random amplitudes Between @ -10 To 10
        For Pt = 0 To UBound(Amplitude, 1)
            Randomize()
            Amplitude(Pt) = Math.Floor((10 - -10 + 1) * Rnd()) + -10
        Next
        Dim LinePen As New Pen(Color.Aqua, 0)
        For Pt = 0 To UBound(Frequency) - 1
            If Pt > 0 Then tg.DrawLine(LinePen, Frequency(Pt - 1), Amplitude(Pt - 1), Frequency(Pt), Amplitude(Pt), hBrush)
        Next
        Refresh()
        hBrush.Dispose()
        LinePen.Dispose()
    End Sub
End Class
