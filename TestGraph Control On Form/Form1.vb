Imports System.Math
Imports System.Drawing.Drawing2D
Imports TestGraphControl
Public Class Form1
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
        tg.Font = New Font("Times New Roman", 20, FontStyle.Bold Or FontStyle.Italic)
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
        'Sent Array To Be Used For X-Axis Labels Instead Of Standard Labels
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
        'Sent Array To Be Used For Y-Axis Labels Instead Of Standard Labels
        tg.DrawCustomYGrid_Labels(ArrayOfLabels)
        '**************************************************************
        'Populate GridLabelFont With Font And Color For Grid Line Labeling
        tg.GridLabelColor = Color.Aqua
        tg.GridLabelFont = New Font("Times New Roman", 9, FontStyle.Regular)
        'Create New Pen For Grid Lines
        tg.GridLineColor = Color.White
        'Also Use GridLine Color FOR Line Data Color
        Dim GridLinePen As New Pen(tg.GridLineColor, 0)
        'Create The Random Bar Patterns And Lable Each With Month Data
        'HatchStyles = Ramdon
        tg.RandomHatchStyles = True
        Dim Month As Integer
        For Month = 0 To 11
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
    Private Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click
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
            Refresh()
        Next
        LinePen.Dispose()

    End Sub
    Private Sub Button4_Click(sender As Object, e As EventArgs) Handles Button4.Click
        Dim tg As New TestGraph
        tg.ClearGraphics()
        tg.SetViewportScale(100, 0, TestGraph.ScaleStyleChoices.Linear, 50, 0, TestGraph.ScaleStyleChoices.Linear)
        tg.GraphStyle = TestGraph.GraphStyleChoices.PointToPoint
        tg.RandomHatchStyles = True
        'Control Font Used For HeaderText, XAxisText And YAxisText
        tg.Font = New Font("Times New Roman", 22, FontStyle.Bold Or FontStyle.Italic)
        tg.HeaderText = "Point To Point Graph Style Example"
        tg.XAxisText = "Graph X-Axis"
        tg.YAxisText = "Graph Y-Axis"
        tg.ShowMousePosition = True
        tg.GraphBackColor = Color.LightGray
        tg.GraphForeColor = Color.DarkRed
        tg.GraphFillColor = Color.Black
        tg.GridLineColor = Color.DimGray
        tg.NumOfXGridLines = 10
        '*******X=Axis*******
        tg.ShowXLabels = True
        tg.ShowXGridLines = True
        tg.RotateXGridLabels = 0
        tg.ShowCustomXLabels = False
        '*******Y=Axis*******
        tg.ShowYGridLines = True
        tg.ShowYLabels = True
        tg.RotateYGridLabels = 0
        tg.ShowCustomYLabels = False
        tg.NumOfYGridLines = 10
        'Populate GridLabelFont With Font And Color For Grid Line Labeling
        tg.GridLabelColor = Color.Black
        tg.GridLabelFont = New Font("Times New Roman", 9, FontStyle.Regular)
        Dim Dash_Pattern1 As Single() = {3, 2, 3, 2}
        Dim DrawPen1 As New Pen(Color.Red)
        With DrawPen1
            .Width = 1
            .DashCap = Drawing2D.DashCap.Triangle
            .DashPattern = Dash_Pattern1
        End With
        tg.DrawLine(DrawPen1, 0, 0, 50, 25)
        Refresh()
        '**************************************************
        Dim Dash_Pattern2 As Single() = {4, 2, 4, 2}
        Dim DrawPen2 As New Pen(Color.Blue)
        With DrawPen2
            .Width = 1.5
            .DashCap = Drawing2D.DashCap.Round
            .DashPattern = Dash_Pattern2
        End With
        tg.DrawLine(DrawPen2, 50, 25, 100, 50)
        Refresh()
        '**************************************************
        Dim Dash_Pattern3 As Single() = {3, 1, 3, 1}
        Dim DrawPen3 As New Pen(Color.Aqua, 2)
        With DrawPen3
            .Width = 2
            .DashCap = Drawing2D.DashCap.Triangle
            .DashPattern = Dash_Pattern3
        End With
        tg.DrawLine(DrawPen3, 0, 50, 50, 25)
        Refresh()
        '**************************************************
        Dim Dash_Pattern4 As Single() = {2, 1, 2, 1}
        Dim DrawPen4 As New Pen(Color.Gold, 2)
        With DrawPen4
            .Width = 2.5
            .DashCap = Drawing2D.DashCap.Flat
            .DashPattern = Dash_Pattern4
        End With
        tg.DrawLine(DrawPen4, 50, 25, 100, 0)
        Refresh()
        'Do Not Dispose Of Brushes Or Pens Before Refresh().
        DrawPen1.Dispose()
        DrawPen2.Dispose()
        DrawPen3.Dispose()
        DrawPen4.Dispose()

    End Sub
    Private Sub Button5_Click(sender As Object, e As EventArgs) Handles Button5.Click
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
            'Most Graph Properties Are Automatically Setup For This Method Including Scale.
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
    Private Sub Button6_Click(sender As Object, e As EventArgs) Handles Button6.Click
        Dim tg As New TestGraph
        tg.ClearGraphics()
        'Control Font Used For HeaderText, XAxisText And YAxisText
        tg.Font = New Font("Times New Roman", 20, FontStyle.Bold Or FontStyle.Italic)
        tg.HeaderText = "DrawString Example With Different StringFormats And Rotated Text "
        tg.XAxisText = "X-Axis Text"
        tg.YAxisText = "Y-Axis Text"
        tg.SetViewportScale(0, 50, TestGraph.ScaleStyleChoices.Linear, 50, 0, TestGraph.ScaleStyleChoices.Linear)
        tg.GraphStyle = TestGraph.GraphStyleChoices.PointToPoint
        tg.GraphBackColor = Color.LightGray
        tg.GraphForeColor = Color.DarkRed
        tg.GraphFillColor = Color.Black
        tg.GridLineColor = Color.DimGray
        tg.ShowXGridLines = True
        tg.NumOfXGridLines = 10
        tg.ShowXLabels = True
        tg.ShowYGridLines = True
        tg.NumOfYGridLines = 5
        tg.ShowYLabels = True
        tg.ShowMousePosition = True
        tg.RotateXGridLabels = 0
        tg.RotateYGridLabels = 0
        tg.ShowCustomXLabels = False
        tg.ShowCustomYLabels = False
        tg.RandomHatchStyles = False
        'Populate GridLabelFont With Font And Color For Grid Line Labeling
        tg.GridLabelColor = Color.Black
        tg.GridLabelFont = New Font("Times New Roman", 9, FontStyle.Regular)
        Dim Fonts(4) As Font
        Dim Brushes(4) As SolidBrush
        Dim Formats(4) As StringFormat
        '*******************************************************************
        Fonts(0) = New Font("Times New Roman", 14, FontStyle.Bold)
        Brushes(0) = New SolidBrush(Color.Red)
        Formats(0) = New StringFormat
        With Formats(0)
            .LineAlignment = StringAlignment.Center
            .Alignment = StringAlignment.Center
            .FormatFlags = StringFormatFlags.NoWrap
        End With
        tg.DrawString("StringFormat = Center/Center/NoWrap (X = 25,Y = 25), Font Size = 14, Rotation = 22.5 Degrees", 22.5, Fonts(0), Brushes(0), 25, 25, Formats(0))
        Refresh()
        '*******************************************************************
        Fonts(1) = New Font("Times New Roman", 12, FontStyle.Bold)
        Brushes(1) = New SolidBrush(Color.LimeGreen)
        Formats(1) = New StringFormat
        With Formats(1)
            .LineAlignment = StringAlignment.Center
            .Alignment = StringAlignment.Near
            .FormatFlags = StringFormatFlags.NoWrap
        End With
        tg.DrawString("StringFormat = Center/Near/NoWrap (X = 0,Y = 10), Font Size = 12, Rotation = 0 Degrees", 0, Fonts(1), Brushes(1), 0, 10, Formats(1))
        Refresh()
        '*******************************************************************
        Fonts(2) = New Font("Times New Roman", 12, FontStyle.Bold)
        Brushes(2) = New SolidBrush(Color.Yellow)
        Formats(2) = New StringFormat
        With Formats(2)
            .LineAlignment = StringAlignment.Center
            .Alignment = StringAlignment.Far
            .FormatFlags = StringFormatFlags.NoWrap
        End With
        tg.DrawString("StringFormat = Center/Far/NoWrap (X = 50,Y = 40), Font Size = 12, Rotation = 0 Degrees", 0, Fonts(2), Brushes(2), 50, 40, Formats(2))
        Refresh()
        '*******************************************************************
        Fonts(3) = New Font("Times New Roman", 9, FontStyle.Bold)
        Brushes(3) = New SolidBrush(Color.Aqua)
        Formats(3) = New StringFormat
        With Formats(3)
            .LineAlignment = StringAlignment.Center
            .Alignment = StringAlignment.Far
            .FormatFlags = StringFormatFlags.NoWrap
        End With
        tg.DrawString("X = 7, Y = 40, Rotation = -65 Degrees", -65, Fonts(3), Brushes(3), 7, 40, Formats(3))
        Refresh()
        '*******************************************************************
        Fonts(4) = New Font("Times New Roman", 9, FontStyle.Bold)
        Brushes(4) = New SolidBrush(Color.White)
        Formats(4) = New StringFormat
        With Formats(4)
            .LineAlignment = StringAlignment.Center
            .Alignment = StringAlignment.Far
            .FormatFlags = StringFormatFlags.NoWrap
        End With
        tg.DrawString("X = 50, Y = 39, Rotation = -65 Degrees", -65, Fonts(4), Brushes(4), 50, 39, Formats(4))
        Refresh()
        '*******************************************************************
        For d = 0 To Brushes.Length - 1
            Fonts(d).Dispose()
            Brushes(d).Dispose()
            Formats(d).Dispose()
        Next
    End Sub

    Private Sub TestGraph1_Load(sender As Object, e As EventArgs) Handles TestGraph1.Load

    End Sub
End Class
