Imports System.ComponentModel
Imports System.Math
Imports System.Drawing.Drawing2D
Imports System.Drawing.Printing
Imports System.IO
Public Class TestGraph
    Implements IGraphInterface
    Public Event Event1() Implements IGraphInterface.Event1
    Private X_Inverse As Boolean
    Private Y_Inverse As Boolean
    Private GraphScale_Width As Single
    Private GraphScale_Height As Single
    'CAPTURE ARRAYS USED FOR PAINT EVENT
    '*******Drawing Pens And Hatch Styles*******
    Private Shared New_DrawingPen(0) As Pen
    '*******Hatch Brush*******
    Private Shared New_HatchBrush(0) As HatchBrush
    '*******DrawLine Method*******
    Private Shared DrawLine_Changed As Boolean
    Private Shared DrawLine_ArrayIndex As Integer = 0
    Private Shared New_DrawLineX1(0) As Single
    Private Shared New_DrawLineX2(0) As Single
    Private Shared New_DrawLineY1(0) As Single
    Private Shared New_DrawLineY2(0) As Single
    '*******DrawString Method*******
    Private Shared DrawString_Changed As Boolean
    Private Shared DrawString_ArrayIndex As Integer = 0
    Private Shared New_DrawStringText(0) As String
    Private Shared New_DrawStringTextRotation(0) As Single
    Private Shared New_DrawStringFont(0) As Font
    Private Shared New_DrawStringBrush(0) As SolidBrush
    Private Shared New_DrawStringX(0) As Single
    Private Shared New_DrawStringY(0) As Single
    Private Shared New_DrawStringFormat(0) As StringFormat
    '*******Custom & Standard X And Y Axis Labeling Methods*******
    Private Shared New_CustomXGridLine_Labels(0) As String
    Private Shared New_CustomYGridLine_Labels(0) As String
    '*******Pie Chart Method*******
    Private Shared PieIndex As Integer = 0
    Private Shared Pie_Changed As Boolean
    Private Shared New_PiePiece(0) As Single
    Private Shared New_PieBrush(0) As SolidBrush
    Private Shared New_PieLabels(0) As String
    Private Shared Pie_Font As Font
    '*******Print And Save Dialogs*******
    Private ReadOnly SaveFileDialog1 As New SaveFileDialog
    Private WithEvents PrintDocument1 As PrintDocument = New PrintDocument
    Private ReadOnly PrintDialog1 As New PrintDialog
    Private ReadOnly PageSetupDialog1 As New PageSetupDialog
    Private PrintPreview As Boolean
    Private ReadOnly PrintPreviewDialog1 As New PrintPreviewDialog
    Private Viewport_Rect As New RectangleF
    Public Interface IGraphInterface
        Property ScaleX_Left As Single
        Property ScaleX_Right As Single
        Property ScaleY_Bottom As Single
        Property ScaleY_Top As Single
        ReadOnly Property ControlLeft As Single
        ReadOnly Property ControlRight As Single
        ReadOnly Property ControlTop As Single
        ReadOnly Property ControlBottom As Single
        ReadOnly Property ControlWidth As Single
        ReadOnly Property ControlHeight As Single
        Property HeaderText As String
        Property XAxisText As String
        Property YAxisText As String
        Property XScaleStyle As ScaleStyleChoices
        Property YScaleStyle As ScaleStyleChoices
        Property GraphBackColor As Color
        Property GraphForeColor As Color
        Property GraphFillColor As Color
        Property GridLineColor As Color
        Property GridLabelColor As Color
        Property RandomHatchStyles As Boolean
        Property GraphStyle As GraphStyleChoices
        Property NumOfXGridLines As Integer
        Property NumOfYGridLines As Integer
        Property ShowMousePosition As Boolean
        Property ShowXLabels As Boolean
        Property ShowCustomXLabels As Boolean
        Property ShowYLabels As Boolean
        Property ShowCustomYLabels As Boolean
        Property ShowXGridLines As Boolean
        Property ShowYGridLines As Boolean
        Property RotateXGridLabels As Single
        Property RotateYGridLabels As Single
        Property Font As Font 'Original Control Font Used Only For Header Text, X-Axis Text And Y-Axis Text.
        Property GridLabelFont As Font
        Sub ResetGraph()
        Sub ClearGraphics()
        Sub DrawCustomXGrid_Labels(LabelCollection As Array)
        Sub DrawCustomYGrid_Labels(LabelCollection As Array)
        Sub SetViewportScale(xLeft As Single, xRight As Single, xStyle As ScaleStyleChoices, yTop As Single, yBottom As Single, yStyle As ScaleStyleChoices)
        Sub DrawLine(DrawPen As Pen, x1 As Single, y1 As Single, x2 As Single, y2 As Single, Optional hBrush As HatchBrush = Nothing)
        Function GenerateLogTestPoints(LogBase As Single, StartPoint As Single, EndPoint As Single, NumOfPoints As Integer) As Single()
        Sub DrawString(Text As String, TextRotation As Single, myFont As Font, myBrush As SolidBrush, PointX As Single, PointY As Single, AlignFormat As StringFormat)
        Sub DrawPieChart(PieBrush As SolidBrush, PieFont As Font, PiePercent As Single, PieLabels As String)
        Event Event1()
    End Interface
    Public Sub SetViewportScale(xLeft As Single, xRight As Single, xStyle As ScaleStyleChoices, yTop As Single, yBottom As Single, yStyle As ScaleStyleChoices) Implements IGraphInterface.SetViewportScale
        'POPULATE The SetViewportScale Arguments For Painting
        If IsNumeric(xLeft) And IsNumeric(xRight) And IsNumeric(yTop) And IsNumeric(yBottom) Then
            ScaleX_Left = xLeft
            GraphAreaX1 = xLeft
            ScaleX_Right = xRight
            GraphAreaX2 = xRight
            ScaleY_Top = yTop
            GraphAreaY2 = yTop
            ScaleY_Bottom = yBottom
            GraphAreaY1 = yBottom
            Select Case xStyle
                Case ScaleStyleChoices.Linear
                    XScaleStyle = xStyle
                    XAxisStyle = xStyle
                Case ScaleStyleChoices.Log10
                    XScaleStyle = xStyle
                    XAxisStyle = xStyle
                Case ScaleStyleChoices.Log2
                    XScaleStyle = xStyle
                    XAxisStyle = xStyle
                Case ScaleStyleChoices.Log20
                    XScaleStyle = xStyle
                    XAxisStyle = xStyle
                Case ScaleStyleChoices.Loge
                    XScaleStyle = xStyle
                    XAxisStyle = xStyle
                Case Else
                    MsgBox("xStyle Argument Is Missing Or Incorrect. Please Try Again.", vbOK)
                    Exit Sub
            End Select
            Select Case yStyle
                Case ScaleStyleChoices.Linear
                    YScaleStyle = yStyle
                    YAxisStyle = yStyle
                Case ScaleStyleChoices.Log10
                    YScaleStyle = yStyle
                    YAxisStyle = yStyle
                Case ScaleStyleChoices.Log2
                    YScaleStyle = yStyle
                    YAxisStyle = yStyle
                Case ScaleStyleChoices.Log20
                    YScaleStyle = yStyle
                    YAxisStyle = yStyle
                Case ScaleStyleChoices.Loge
                    YScaleStyle = yStyle
                    YAxisStyle = yStyle
                Case Else
                    MsgBox("yStyle Argument Is Missing Or Incorrect. Please Try Again.", vbOK)
                    Exit Sub
            End Select
        Else
            MsgBox("Some Arguments Are Missing Or Incorrect. Please Try Again.", vbOK)
        End If
        Invalidate()
    End Sub
    Public Sub ClearGraphics() Implements IGraphInterface.ClearGraphics
        'DESTROY ARRAY CONTENTS
        GarbageDisposal() 'Disposal Sub For Arrays
        '*******DrawLine Method*******
        DrawLine_Changed = False
        DrawLine_ArrayIndex = 0
        ReDim New_DrawingPen(0) 'Common Pen To Line And LineF
        ReDim New_DrawLineX1(0)
        ReDim New_DrawLineX2(0)
        ReDim New_DrawLineY1(0)
        ReDim New_DrawLineY2(0)
        '*******DrawString Method**********
        DrawString_Changed = False
        DrawString_ArrayIndex = 0
        ReDim New_DrawStringText(0)
        ReDim New_DrawStringTextRotation(0)
        ReDim New_DrawStringFont(0)
        ReDim New_DrawStringBrush(0)
        ReDim New_DrawStringX(0)
        ReDim New_DrawStringY(0)
        ReDim New_DrawStringFormat(0)
        ReDim New_CustomXGridLine_Labels(0)
        ReDim New_CustomYGridLine_Labels(0)
        '*******HatchStyles*******
        Random_HatchStyles = False
        ReDim New_HatchBrush(0)
        PieIndex = 0
        Pie_Changed = False
        ReDim New_PiePiece(0)
        ReDim New_PieBrush(0)
        ReDim New_PieLabels(0)
        Invalidate()
    End Sub
    Private Sub GarbageDisposal()
        If New_PieBrush.Length > 1 Then
            For br = 1 To New_PieBrush.Length - 1
                New_PieBrush(br).Dispose()
            Next
        End If
        If New_DrawStringBrush.Length > 1 Then
            For br = 1 To New_DrawStringBrush.Length - 1
                New_DrawStringBrush(br).Dispose()
            Next
        End If
        If New_HatchBrush.Length > 1 Then
            For hbr = 1 To New_HatchBrush.Length - 1
                New_HatchBrush(hbr).Dispose()
            Next
        End If
        If New_DrawStringFormat.Length > 1 Then
            For sf = 1 To New_DrawStringFormat.Length - 1
                New_DrawStringFormat(sf).Dispose()
            Next
        End If
        If New_DrawStringFont.Length > 1 Then
            For ft = 1 To New_DrawStringFont.Length - 1
                New_DrawStringFont(ft).Dispose()
            Next
        End If
        If New_DrawingPen.Length > 1 Then
            For pn = 1 To New_DrawingPen.Length - 1
                New_DrawingPen(pn).Dispose()
            Next
        End If
        If Pie_Font Is Nothing Then
            Exit Sub
        Else
            Pie_Font.Dispose()
        End If
        Invalidate()
    End Sub
    Public Sub ResetGraph() Implements IGraphInterface.ResetGraph
        ClearGraphics()
        NumOfXGridLines = 5
        NumOfYGridLines = 5
        DrawLine_Changed = False
        DrawString_Changed = False
        Pie_Changed = False
        ScaleX_Left = 0
        ScaleX_Right = 50
        ScaleY_Bottom = 0
        ScaleY_Top = 50
        HeaderText = "HeaderText"
        XAxisText = "XAxisText"
        YAxisText = "YAxisText"
        XScaleStyle = ScaleStyleChoices.Linear
        YScaleStyle = ScaleStyleChoices.Linear
        GraphBackColor = Color.DimGray
        GraphForeColor = Color.DimGray
        GraphFillColor = Color.DimGray
        GridLineColor = Color.White
        GridLabelColor = Color.White
        GraphStyle = GraphStyleChoices.PointToPoint
        RandomHatchStyles = False
        ShowMousePosition = True
        ShowCustomXLabels = False
        ShowCustomYLabels = False
        ShowXLabels = True
        ShowYLabels = True
        ShowXGridLines = True
        ShowYGridLines = True
        RotateXGridLabels = 0
        RotateYGridLabels = 0
        Invalidate()
    End Sub
    Private Function GenerateRandomNumbers(lowerbound As Single, upperbound As Single, NumOfPoints As Integer) As Single()
        Dim RandomNumbers(NumOfPoints) As Single
        Dim Pt As Integer
        For Pt = 0 To UBound(RandomNumbers, 1) - 1
            Randomize()
            RandomNumbers(Pt) = Math.Floor((upperbound - lowerbound + 1) * Rnd()) + lowerbound
        Next
        Return RandomNumbers
    End Function
    Public Function GenerateLogTestPoints(LogBase As Single, lowerbound As Single, upperbound As Single, NumOfPoints As Integer) As Single() Implements IGraphInterface.GenerateLogTestPoints
        'Generates And Returns An Array Of Log Numbers Based On The Log Base, Spectrum and Number Of Points.
        Dim LogNumbers(NumOfPoints) As Single
        'ReDim Preserve LogNumbers(NumOfPoints)
        Dim dX As Single = ((Log(upperbound) / Log(LogBase)) - (Log(lowerbound) / Log(LogBase))) / (UBound(LogNumbers, 1) - 1)
        Dim Pt As Integer
        For Pt = 0 To UBound(LogNumbers, 1) - 1
            LogNumbers(Pt) = 10 ^ ((Log(lowerbound) / Log(10)) + (dX * Pt))
        Next
        Return LogNumbers
    End Function
    Public Sub DrawLine(DrawPen As Pen, x1 As Single, y1 As Single, x2 As Single, y2 As Single, Optional hBrush As HatchBrush = Nothing) Implements IGraphInterface.DrawLine
        'Populate The DrawLine Arrays For Painting And RePainting
        If DrawPen Is Nothing Then
            DrawLine_Changed = False
            MsgBox("DrawPen Does Not Exist Or Is Incorrect. Please Try Again.", vbOK)
            Exit Sub
        End If
        Dim NewDrawPen As New Pen(DrawPen.Color, DrawPen.Width) With {
                .DashStyle = DrawPen.DashStyle
            }
        'Special Pen
        If DrawPen.DashStyle <> DashStyle.Solid Then
            If IsNumeric(DrawPen.DashPattern(0)) Then 'Check For Pattern
                NewDrawPen.DashPattern() = DrawPen.DashPattern()
            End If
            If DrawPen.DashCap <> Nothing Then 'Check For DashCap
                NewDrawPen.DashCap = DrawPen.DashCap
            End If
        End If
        If IsNumeric(x1) And IsNumeric(y1) And IsNumeric(x2) And IsNumeric(y2) Then
            'All Array Increments Are Referenced To New_DrawLineX1
            DrawLine_ArrayIndex = New_DrawLineX1.GetUpperBound(0) + 1
            ReDim Preserve New_DrawLineX1(DrawLine_ArrayIndex)
            New_DrawLineX1(DrawLine_ArrayIndex) = x1
            ReDim Preserve New_DrawLineY1(DrawLine_ArrayIndex)
            New_DrawLineY1(DrawLine_ArrayIndex) = y1
            ReDim Preserve New_DrawLineX2(DrawLine_ArrayIndex)
            New_DrawLineX2(DrawLine_ArrayIndex) = x2
            ReDim Preserve New_DrawLineY2(DrawLine_ArrayIndex)
            New_DrawLineY2(DrawLine_ArrayIndex) = y2
            ReDim Preserve New_DrawingPen(DrawLine_ArrayIndex)
            New_DrawingPen(DrawLine_ArrayIndex) = NewDrawPen
            If hBrush Is Nothing Then
                DrawLine_Changed = True
                Exit Sub
            Else
                Dim NewhBrush As New HatchBrush(hBrush.HatchStyle, hBrush.ForegroundColor, hBrush.BackgroundColor)
                ReDim Preserve New_HatchBrush(DrawLine_ArrayIndex)
                New_HatchBrush(DrawLine_ArrayIndex) = NewhBrush
            End If
            DrawLine_Changed = True
        Else
            MsgBox("Some Arguments Are Missing Or Incorrect. Please Try Again.", vbOK)
            DrawLine_Changed = False
        End If
        Invalidate()
    End Sub
    Public Sub DrawString(Text As String, TextRotation As Single, myFont As Font, myBrush As SolidBrush, PointX As Single, PointY As Single, AlignFormat As StringFormat) Implements IGraphInterface.DrawString
        'POPULATE AND CAPTURE The DrawString Arrays For Painting And Repainting
        If Text = vbNullString Then
            MsgBox("Argument Text Is Missing. Please Try Again.", vbOK)
            DrawString_Changed = False
            Exit Sub
        End If
        If myFont Is Nothing Then
            MsgBox("Font Is Invalid. Please Try Again.", vbOK)
            DrawString_Changed = False
            Exit Sub
        End If
        'ReConstruct Font
        Dim NewFont As New Font(myFont.FontFamily, myFont.Size, myFont.Style)
        If myBrush Is Nothing Then
            DrawString_Changed = False
            MsgBox("Brush Is Invalid. Please Try Again.", vbOK)
            Exit Sub
        End If
        'ReConstruct Brush
        Dim NewBrush As New SolidBrush(myBrush.Color)
        If AlignFormat Is Nothing Then
            MsgBox("AlignmentFormat Are Invalid. Please Try Again.", vbOK)
            DrawString_Changed = False
            Exit Sub
        End If
        'ReConstruct AlignmentFormat
        Dim NewAlignFormat As New StringFormat
        With NewAlignFormat
            .LineAlignment = AlignFormat.LineAlignment
            .Alignment = AlignFormat.Alignment
            .FormatFlags = AlignFormat.FormatFlags
        End With
        If IsNumeric(PointX) And IsNumeric(PointY) Then
            DrawString_ArrayIndex = New_DrawStringText.GetUpperBound(0) + 1
            ReDim Preserve New_DrawStringText(DrawString_ArrayIndex)
            New_DrawStringText(DrawString_ArrayIndex) = Text
            ReDim Preserve New_DrawStringTextRotation(DrawString_ArrayIndex)
            New_DrawStringTextRotation(DrawString_ArrayIndex) = TextRotation
            ReDim Preserve New_DrawStringFont(DrawString_ArrayIndex)
            New_DrawStringFont(DrawString_ArrayIndex) = NewFont
            ReDim Preserve New_DrawStringBrush(DrawString_ArrayIndex)
            New_DrawStringBrush(DrawString_ArrayIndex) = NewBrush
            ReDim Preserve New_DrawStringX(DrawString_ArrayIndex)
            New_DrawStringX(DrawString_ArrayIndex) = PointX
            ReDim Preserve New_DrawStringY(DrawString_ArrayIndex)
            New_DrawStringY(DrawString_ArrayIndex) = PointY
            ReDim Preserve New_DrawStringFormat(DrawString_ArrayIndex)
            New_DrawStringFormat(DrawString_ArrayIndex) = NewAlignFormat
            DrawString_Changed = True
        Else
            DrawString_Changed = False
            MsgBox("X Or Y Coordinances Are Invalid. Please Try Again.", vbOK)
        End If
        Invalidate()
    End Sub
    Public Sub DrawCustomXGrid_Labels(LabelCollection As Array) Implements IGraphInterface.DrawCustomXGrid_Labels
        'LabelCollection Uses NumOfXGridLines for indexing
        'ShowXGridLines Must Be True
        'ShowXLabels Must Be False
        'ShowCustomXLabels Must Be TRUE
        If XScaleStyle <> ScaleStyleChoices.Linear Then
            MsgBox("XScaleStyle Must Be Set to ScaleStyleChoices.Linear. This Option Is Not" &
                   " Available For Log Functions. Setting To ScaleStyleChoices.Linear.", vbOK, vbCancel)
            If vbOK Then
                XScaleStyle = ScaleStyleChoices.Linear
            Else
                Exit Sub
            End If
        End If
        If LabelCollection Is Nothing Then Exit Sub
        If LabelCollection.GetUpperBound(0) = -1 Then Exit Sub
        Dim x As Integer
        For x = 0 To LabelCollection.GetUpperBound(0) - 1
            ReDim Preserve New_CustomXGridLine_Labels(x)
            New_CustomXGridLine_Labels(x) = LabelCollection(x)
        Next
        ShowCustomXLabels = True
        Show_CustomXLabels = True
        Invalidate()
    End Sub
    Public Sub DrawCustomYGrid_Labels(LabelCollection As Array) Implements IGraphInterface.DrawCustomYGrid_Labels
        'LabelCollection Uses NumOfYGridLines for indexing
        'ShowYGridLines Must Be True
        'ShowCustomYLabels Must Be TRUE
        If YScaleStyle <> ScaleStyleChoices.Linear Then
            MsgBox("YScaleStyle Must Be Set to ScaleStyleChoices.Linear. This Option Is Not" &
                   " Available For Log Functions. Setting To ScaleStyleChoices.Linear.", vbOK, vbCancel)
            If vbOK Then
                YScaleStyle = ScaleStyleChoices.Linear
            Else
                Exit Sub
            End If
        End If
        If LabelCollection Is Nothing Then Exit Sub
        If LabelCollection.GetUpperBound(0) = -1 Then Exit Sub
        Dim y As Integer
        For y = 0 To LabelCollection.GetUpperBound(0) - 1
            ReDim Preserve New_CustomYGridLine_Labels(y)
            New_CustomYGridLine_Labels(y) = LabelCollection(y)
        Next
        ShowCustomYLabels = True
        Show_CustomYLabels = True
        Invalidate()
    End Sub
    Public Sub DrawPieChart(PieBrush As SolidBrush, PieFont As Font, PiePercent As Single, PieLabels As String) Implements IGraphInterface.DrawPieChart
        GraphStyle = GraphStyleChoices.Pie
        If PieBrush Is Nothing Then
            Pie_Changed = False
            MsgBox("SolidBrush Arguments Are Missing. Please Try Again.", vbOK)
            Exit Sub
        End If
        If PieFont Is Nothing Then
            Pie_Changed = False
            MsgBox("Font Argument Is Missing. Please Try Again.", vbOK)
            Exit Sub
        End If
        If PiePercent = Nothing Then
            Pie_Changed = False
            MsgBox("Pie Percent Arguments Are Missing. Please Try Again.", vbOK)
            Exit Sub
        End If
        If PieLabels = vbNullString Then
            Pie_Changed = False
            MsgBox("String Arguments Are Missing. Please Try Again.", vbOK)
            Exit Sub
        End If
        If IsNumeric(PiePercent) Then
            PieIndex = New_PiePiece.GetUpperBound(0) + 1
            ReDim Preserve New_PiePiece(PieIndex)
            New_PiePiece(PieIndex) = PiePercent
            ReDim Preserve New_PieBrush(PieIndex)
            'Recreate The Solid Brush
            Dim NewPieBrush As New SolidBrush(PieBrush.Color)
            New_PieBrush(PieIndex) = NewPieBrush
            ReDim Preserve New_PieLabels(PieIndex)
            New_PieLabels(PieIndex) = PieLabels
            Pie_Font = PieFont
            SetViewportScale(0, 50, ScaleStyleChoices.Linear, 0, 50, ScaleStyleChoices.Linear)
            ShowMousePosition = False
            Show_MousePosition = False
            LblMouseMove.Visible = False
            ShowXLabels = False
            Show_XLabels = False
            ShowYLabels = False
            Show_YLabels = False
            ShowCustomXLabels = False
            Show_CustomXLabels = False
            ShowCustomYLabels = False
            Show_CustomYLabels = False
            ShowXGridLines = False
            Show_XGrids = False
            ShowYGridLines = False
            Show_YGrids = False
            Pie_Changed = True
        Else
            Pie_Changed = False
            MsgBox("PiePercent Argument Is Missing Or Invalid. Please Try Again.", vbOK)
        End If
        Invalidate()
    End Sub
    '*******************************************BEGIN PROPERTIES********************************************
    Private Shared GraphAreaX1 As Single = 0
    <Browsable(True), Category("Layout"), Description("Sets The Graph Viewport X Axis Scale Left."), DefaultValue(0)>
    Public Property ScaleX_Left As Single Implements IGraphInterface.ScaleX_Left
        Get
            Return GraphAreaX1
        End Get
        Set(pbNewValue As Single)
            If XAxisStyle <> ScaleStyleChoices.Linear Then
                'Log Values Limited To Negative, = 0, 1E-24, 1E+24
                pbNewValue = Abs(pbNewValue)
                Select Case pbNewValue
                    Case Is = 0
                        GraphAreaX1 = 1
                    Case Is = GraphAreaX2
                        GraphAreaX1 = GraphAreaX2 + 1
                    Case Is > 1.0E+24
                        GraphAreaX1 = 1.0E+24
                    Case < 1.0E-24
                        GraphAreaX1 = 1.0E-24
                End Select
            Else
                'Linear Values Limited To -1E-24, >1E+24
                Select Case pbNewValue
                    Case Is > 1.0E+24
                        GraphAreaX1 = 1.0E+24
                    Case Is < -1.0E+24
                        GraphAreaX1 = -1.0E-24
                End Select
            End If
            GraphAreaX1 = pbNewValue
            Invalidate()
        End Set
    End Property
    Private Shared GraphAreaX2 As Single
    <Browsable(True)> <Category("Layout"), Description("Sets The Graph Viewport X Axis Scale Right."), DefaultValue(50)>
    Public Property ScaleX_Right As Single Implements IGraphInterface.ScaleX_Right
        Get
            Return GraphAreaX2
        End Get
        Set(pbNewValue As Single)
            If XAxisStyle <> ScaleStyleChoices.Linear Then
                'Log Values Limited To Negative, = 0, 1E-24, 1E+24
                pbNewValue = Abs(pbNewValue)
                Select Case pbNewValue
                    Case Is = 0
                        GraphAreaX2 = 1
                    Case Is = GraphAreaX1
                        GraphAreaX2 = GraphAreaX1 + 1
                    Case Is > 1.0E+24
                        GraphAreaX2 = 1.0E+24
                    Case < 1.0E-24
                        GraphAreaX2 = 1.0E-24
                End Select
            Else
                'Linear Values Limited To -1E-24, >1E+24
                Select Case pbNewValue
                    Case Is > 1.0E+24
                        GraphAreaX2 = 1.0E+24
                    Case Is < -1.0E-24
                        GraphAreaX2 = -1.0E-24
                End Select
            End If
            GraphAreaX2 = pbNewValue
            Invalidate()
        End Set
    End Property
    Private Shared GraphAreaY1 As Single = 0
    <Browsable(True), Category("Layout"), Description("Sets The Graph Viewport Y Axis Scale Bottom."), DefaultValue(0)>
    Public Property ScaleY_Bottom As Single Implements IGraphInterface.ScaleY_Bottom
        Get
            Return GraphAreaY1
        End Get
        Set(pbNewValue As Single)
            If YAxisStyle <> ScaleStyleChoices.Linear Then
                'Log Values Limited To Negative, = 0, 1E-24, 1E+24
                pbNewValue = Abs(pbNewValue)
                Select Case pbNewValue
                    Case Is = 0
                        GraphAreaY1 = 1
                    Case Is = GraphAreaY2
                        GraphAreaY1 = GraphAreaY2 + 1
                    Case Is > 1.0E+24
                        GraphAreaY1 = 1.0E+24
                    Case < 1.0E-24
                        GraphAreaY1 = 1.0E-24
                End Select
            Else
                'Linear Values Limited To -1E-24, >1E+24
                Select Case pbNewValue
                    Case Is > 1.0E+24
                        GraphAreaY1 = 1.0E+24
                    Case Is < -1.0E-24
                        GraphAreaY1 = -1.0E-24
                End Select
            End If
            GraphAreaY1 = pbNewValue
            Invalidate()
        End Set
    End Property
    Private Shared GraphAreaY2 As Single = 50
    <Browsable(True), Category("Layout"), Description("Sets The Graph Viewport Y Axis Scale Top."), DefaultValue(50)>
    Public Property ScaleY_Top As Single Implements IGraphInterface.ScaleY_Top
        Get
            Return GraphAreaY2
        End Get
        Set(pbNewValue As Single)
            If YAxisStyle <> ScaleStyleChoices.Linear Then
                'Log Values Limited To Negative, = 0, 1E-24, 1E+24
                pbNewValue = Abs(pbNewValue)
                Select Case pbNewValue
                    Case Is = 0
                        GraphAreaY2 = 1
                    Case Is = GraphAreaY1
                        GraphAreaY2 = GraphAreaY1 + 1
                    Case Is > 1.0E+24
                        GraphAreaY2 = 1.0E+24
                    Case < 1.0E-24
                        GraphAreaY2 = 1.0E-24
                End Select
            Else
                'Linear Values Limited To -1E-24, >1E+24
                Select Case pbNewValue
                    Case Is > 1.0E+24
                        GraphAreaY2 = 1.0E+24
                    Case Is < -1.0E-24
                        GraphAreaY2 = -1.0E-24
                End Select
            End If
            GraphAreaY2 = pbNewValue
            Invalidate()
        End Set
    End Property
    Private Shared Header_Text As String = "HeaderText"
    <Browsable(True), Category("Appearance"), Description("Sets The Header Caption For Thr Graph."), DefaultValue("HeaderText")>
    Public Property HeaderText As String Implements IGraphInterface.HeaderText
        Get
            Return Header_Text
        End Get
        Set(ptyNewStr As String)
            Header_Text = ptyNewStr
            Invalidate()
        End Set
    End Property
    Private Shared XAxis_Text As String = "XAxisText"
    <Browsable(True), Category("Appearance"), Description("Sets The Graph X-Axis Text."), DefaultValue("XAxisText")>
    Public Property XAxisText As String Implements IGraphInterface.XAxisText
        Get
            Return XAxis_Text
        End Get
        Set(ptyNewStr As String)
            XAxis_Text = ptyNewStr
            Invalidate()
        End Set
    End Property
    Private Shared YAxis_Text As String = "YAxisText"
    <Browsable(True), Category("Appearance"), Description("Sets The Graph Y-Axis Text."), DefaultValue("YAxisText")>
    Public Property YAxisText As String Implements IGraphInterface.YAxisText
        Get
            Return YAxis_Text
        End Get
        Set(ptyNewStr As String)
            YAxis_Text = ptyNewStr
            Invalidate()
        End Set
    End Property
    'Scale Styles For X And Y Axis Using ENUM
    Public Enum ScaleStyleChoices
        [Linear] = 0
        [Log2] = 2
        [Loge] = Math.E
        [Log10] = 10
        [Log20] = 20
    End Enum
    Private Shared XAxisStyle As ScaleStyleChoices = ScaleStyleChoices.Linear
    <Browsable(True), Category("Layout"), Description("Sets The Scale Style For The Graphs X-Axis."), DefaultValue(ScaleStyleChoices.Linear)>
    Public Property XScaleStyle As ScaleStyleChoices Implements IGraphInterface.XScaleStyle
        Get
            Return XAxisStyle
        End Get
        Set(pbNewChoice As ScaleStyleChoices)
            If pbNewChoice <> ScaleStyleChoices.Linear Then
                If ScaleX_Left <= 0 Or ScaleX_Right <= 0 Then
                    MsgBox("ScaleX_Left Or ScaleX_Right Log Value Cannot Be < Or = To 0. Please Change" &
                           " X Scale Style Value Or Try Setting The Scale Values Prior To Changing" &
                           " The Scale Style To Log.", vbOK)
                    Exit Property
                End If
            End If
            XAxisStyle = pbNewChoice
            Invalidate()
        End Set
    End Property
    Private Shared YAxisStyle As ScaleStyleChoices = ScaleStyleChoices.Linear
    <Browsable(True), Category("Layout"), Description("Sets The Scale Style For The Graphs Y-Axis."), DefaultValue(ScaleStyleChoices.Linear)>
    Public Property YScaleStyle As ScaleStyleChoices Implements IGraphInterface.YScaleStyle
        Get
            Return YAxisStyle
        End Get
        Set(pbNewChoice As ScaleStyleChoices)
            If pbNewChoice <> ScaleStyleChoices.Linear Then
                If ScaleY_Top <= 0 Or ScaleY_Bottom <= 0 Then
                    MsgBox("ScaleY_Top Or ScaleY_Bottom Log Value Cannot Be < Or = To 0. Please Change" &
                           " Y Scale Style Value Or Try Setting The Scale Values Prior To Changing" &
                           " The Scale Style To Log.", vbOK)
                    Exit Property
                End If
            End If
            YAxisStyle = pbNewChoice
            Invalidate()
        End Set
    End Property
    Private Shared Back_Color As Color = Color.DimGray
    <Browsable(True), Category("Appearance"), Description("Sets The BackColor For The Area Outside The Graph."), DefaultValue(GetType(Color), "DimGray")>
    Public Property GraphBackColor As Color Implements IGraphInterface.GraphBackColor
        Get
            Return Back_Color
        End Get
        Set(ColorVal As Color)
            Back_Color = ColorVal
            Me.BackColor = ColorVal
            Invalidate()
        End Set
    End Property
    Private Shared Fore_Color As Color = Color.DimGray
    <Browsable(True), Category("Appearance"), Description("Sets The ForeColor For Header Text, X-Axis Text And Y-Axis Text."), DefaultValue(GetType(Color), "DimGray")>
    Public Property GraphForeColor As Color Implements IGraphInterface.GraphForeColor
        Get
            Return Fore_Color
        End Get
        Set(ColorVal As Color)
            Fore_Color = ColorVal
            ForeColor = ColorVal
            Invalidate()
        End Set
    End Property
    Private Shared Fill_Color As Color = Color.DimGray
    <Browsable(True), Category("Appearance"), Description("Sets The Fill Color For The Graph ViewPort Area."), DefaultValue(GetType(Color), "DimGray")>
    Public Property GraphFillColor As Color Implements IGraphInterface.GraphFillColor
        Get
            Return Fill_Color
        End Get
        Set(ColorVal As Color)
            Fill_Color = ColorVal
            Invalidate()
        End Set
    End Property
    Private Shared GridLine_Color As Color = Color.White
    <Browsable(True), Category("Appearance"), Description("Sets The Color Of The X And Y Grid Lines."), DefaultValue(GetType(Color), "White")>
    Public Property GridLineColor As Color Implements IGraphInterface.GridLineColor
        Get
            Return GridLine_Color
        End Get
        Set(ColorVal As Color)
            GridLine_Color = ColorVal
            Invalidate()
        End Set
    End Property
    Private Shared GridLabel_Color As Color = Color.White
    <Browsable(True), Category("Appearance"), Description("Sets The Drawing Pen Color For X And Y Grid Labels."), DefaultValue(GetType(Color), "White")>
    Public Property GridLabelColor As Color Implements IGraphInterface.GridLabelColor
        Get
            Return GridLabel_Color
        End Get
        Set(ColorVal As Color)
            GridLabel_Color = ColorVal
            Invalidate()
        End Set
    End Property
    'Scale Styles For X Axis Only.
    'Only Use Bar Style For X Axis
    Public Enum GraphStyleChoices
        [PointToPoint] = 0
        [Bar] = 1
        [Pie] = 2
    End Enum
    Private Shared Graph_Style As GraphStyleChoices = GraphStyleChoices.PointToPoint
    <Browsable(True), Category("Layout"), Description("Sets The Graph Style Choices."), DefaultValue(GraphStyleChoices.PointToPoint)>
    Public Property GraphStyle As GraphStyleChoices Implements IGraphInterface.GraphStyle
        Get
            Return Graph_Style
        End Get
        Set(pbNewChoice As GraphStyleChoices)
            Graph_Style = pbNewChoice
            Invalidate()
        End Set
    End Property
    Private Shared NumOfX_GridLines As Integer = 5
    <Browsable(True), Category("Layout"), Description("Sets The Number Of X-Axis Grid Lines To Display. Maximum = 25."), DefaultValue(5)>
    Public Property NumOfXGridLines As Integer Implements IGraphInterface.NumOfXGridLines
        Get
            Return NumOfX_GridLines
        End Get
        Set(pbNewValue As Integer)
            If pbNewValue > 25 Then pbNewValue = 25
            If pbNewValue < 0 Then pbNewValue = 0
            NumOfX_GridLines = pbNewValue
            Invalidate()
        End Set
    End Property
    Private Shared NumOfY_GridLines As Integer = 5
    <Browsable(True), Category("Layout"), Description("Sets The Number Of Y-Axis Grid Lines To Display. Maximum = 25."), DefaultValue(5)>
    Public Property NumOfYGridLines As Integer Implements IGraphInterface.NumOfYGridLines
        Get
            Return NumOfY_GridLines
        End Get
        Set(pbNewValue As Integer)
            If pbNewValue > 25 Then pbNewValue = 25
            If pbNewValue < 0 Then pbNewValue = 0
            NumOfY_GridLines = pbNewValue
            Invalidate()
        End Set
    End Property
    Private Shared Random_HatchStyles As Boolean = True
    <Browsable(True), Category("Appearance"), Description("Sets The Randon HatchStyle Pattern On Or Off For The Bar Graph."), DefaultValue(True)>
    Public Property RandomHatchStyles As Boolean Implements IGraphInterface.RandomHatchStyles
        Get
            Return Random_HatchStyles
        End Get
        Set(pbNewChoice As Boolean)
            Random_HatchStyles = pbNewChoice
            Invalidate()
        End Set
    End Property
    Private Shared Show_MousePosition As Boolean = True
    <Browsable(True), Category("Appearance"), Description("Shows Or Hides The Mouse Coordinance Inside The Graph Viewport."), DefaultValue(True)>
    Public Property ShowMousePosition As Boolean Implements IGraphInterface.ShowMousePosition
        Get
            Return Show_MousePosition
        End Get
        Set(pbNewChoice As Boolean)
            Show_MousePosition = pbNewChoice
            LblMouseMove.Visible = pbNewChoice
            Invalidate()
        End Set
    End Property
    Private Shared Show_XLabels As Boolean = True
    <Browsable(True), Category("Appearance"), Description("Shows Or Hides The X-Axis Labels On The Graph."), DefaultValue(True)>
    Public Property ShowXLabels As Boolean Implements IGraphInterface.ShowXLabels
        Get
            Return Show_XLabels
        End Get
        Set(pbNewChoice As Boolean)
            Show_XLabels = pbNewChoice
            If Show_XLabels Then ShowCustomXLabels = False
            Invalidate()
        End Set
    End Property
    Private Shared Show_CustomXLabels As Boolean = False
    <Browsable(True), Category("Appearance"), Description("Shows Or Hides The Custom X-Axis Labels On The Graph."), DefaultValue(False)>
    Public Property ShowCustomXLabels As Boolean Implements IGraphInterface.ShowCustomXLabels
        Get
            Return Show_CustomXLabels
        End Get
        Set(pbNewChoice As Boolean)
            Show_CustomXLabels = pbNewChoice
            If Show_CustomXLabels Then ShowXLabels = False
            If Not Show_CustomXLabels Then ReDim New_CustomXGridLine_Labels(0)
            Invalidate()
        End Set
    End Property
    Private Shared Show_YLabels As Boolean = True
    <Browsable(True), Category("Appearance"), Description("Shows Or Hides The Y-Axis Labels On The Graph."), DefaultValue(True)>
    Public Property ShowYLabels As Boolean Implements IGraphInterface.ShowYLabels
        Get
            Return Show_YLabels
        End Get
        Set(pbNewChoice As Boolean)
            Show_YLabels = pbNewChoice
            If Show_YLabels Then ShowCustomYLabels = False
            Invalidate()
        End Set
    End Property
    Private Shared Show_CustomYLabels As Boolean = False
    <Browsable(True), Category("Appearance"), Description("Shows Or Hides The Custom Y-Axis Labels On The Graph."), DefaultValue(False)>
    Public Property ShowCustomYLabels As Boolean Implements IGraphInterface.ShowCustomYLabels
        Get
            Return Show_CustomYLabels
        End Get
        Set(pbNewChoice As Boolean)
            Show_CustomYLabels = pbNewChoice
            If Show_CustomYLabels Then ShowYLabels = False
            If Not Show_CustomYLabels Then ReDim New_CustomYGridLine_Labels(0)
            Invalidate()
        End Set
    End Property
    Private Shared Show_XGrids As Boolean = True
    <Browsable(True), Category("Appearance"), Description("Shows Or Hides The X-Axis Grid Lines On The Graph."), DefaultValue(True)>
    Public Property ShowXGridLines As Boolean Implements IGraphInterface.ShowXGridLines
        Get
            Return Show_XGrids
        End Get
        Set(pbNewChoice As Boolean)
            Show_XGrids = pbNewChoice
            Invalidate()
        End Set
    End Property
    Private Shared Show_YGrids As Boolean = True
    <Browsable(True), Category("Appearance"), Description("Shows Or Hides The Y-Axis Grid Lines On The Graph."), DefaultValue(True)>
    Public Property ShowYGridLines As Boolean Implements IGraphInterface.ShowYGridLines
        Get
            Return Show_YGrids
        End Get
        Set(pbNewChoice As Boolean)
            Show_YGrids = pbNewChoice
            Invalidate()
        End Set
    End Property
    Private Shared Default_Font = New Font("Times New Roman", 18, FontStyle.Bold Or FontStyle.Italic)
    <Browsable(True), Category("Appearance"), Description("The Controls Font Used For HeaderText, YAxisText And XAxis Text.")>
    Public Overrides Property Font As Font Implements IGraphInterface.Font
        Get
            Return MyBase.Font
        End Get
        Set(pbNewFont As Font)
            If pbNewFont Is Nothing Then
                MyBase.Font = Default_Font
            Else
                MyBase.Font = pbNewFont
            End If
            Default_Font = pbNewFont
            Invalidate()
        End Set
    End Property
    Private Shared Grid_Font = New Font("Times New Roman", 9, FontStyle.Regular)
    <Browsable(True), Category("Appearance"), Description("The Controls Font Used For HeaderText, YAxisText And XAxis Text.")>
    Public Property GridLabelFont As Font Implements IGraphInterface.GridLabelFont
        Get
            Return Grid_Font
        End Get
        Set(pbNewFont As Font)
            Grid_Font = pbNewFont
            Invalidate()
        End Set
    End Property
    Private Shared Rotate_XGridLabels As Single = 0
    <Browsable(True), Category("Appearance"), Description("Rotates The X Grid Labels Between -90 To 90 Degrees."), DefaultValue(0)>
    Public Property RotateXGridLabels As Single Implements IGraphInterface.RotateXGridLabels
        Get
            Return Rotate_XGridLabels
        End Get
        Set(pbNewChoice As Single)
            If pbNewChoice < -90 Or pbNewChoice > 90 Then
                MsgBox("RotateXGridLabels Allowable Limits Are From -90 To 90 Degrees. Please Try Again", vbOK)
                Exit Property
            End If
            Rotate_XGridLabels = pbNewChoice
            Invalidate()
        End Set
    End Property
    Private Shared Rotate_YGridLabels As Single = 0
    <Browsable(True), Category("Appearance"), Description("Rotates The Y Grid Labels Between -90 To 90 Degrees."), DefaultValue(0)>
    Public Property RotateYGridLabels As Single Implements IGraphInterface.RotateYGridLabels
        Get
            Return Rotate_YGridLabels
        End Get
        Set(pbNewChoice As Single)
            If pbNewChoice < -90 Or pbNewChoice > 90 Then
                MsgBox("RotateYGridLabels Allowable Limits Are From -90 To 90 Degrees. Please Try Again", vbOK)
                Exit Property
            End If
            Rotate_YGridLabels = pbNewChoice
            Invalidate()
        End Set
    End Property
    Private Shared Control_LeftX As Single
    <Browsable(True), Category("Layout"), Description("(Read Only) Returns The Control Left In Custom Coordinances.")>
    Public ReadOnly Property ControlLeft As Single Implements IGraphInterface.ControlLeft
        Get
            Return Control_LeftX
            Invalidate()
        End Get
    End Property
    Private Shared Control_RightX As Single
    <Browsable(True), Category("Layout"), Description("(Read Only) Returns The Control Right In Custom Coordinances.")>
    Public ReadOnly Property ControlRight As Single Implements IGraphInterface.ControlRight
        Get
            Return Control_RightX
            Invalidate()
        End Get
    End Property
    Private Shared Control_TopY As Single
    <Browsable(True), Category("Layout"), Description("(Read Only) Returns The Control Top In Custom Coordinances.")>
    Public ReadOnly Property ControlTop As Single Implements IGraphInterface.ControlTop
        Get
            Return Control_TopY
            Invalidate()
        End Get
    End Property
    Private Shared Control_BottomY As Single
    <Browsable(True), Category("Layout"), Description("(Read Only) Returns The Control Bottom In Custom Coordinances.")>
    Public ReadOnly Property ControlBottom As Single Implements IGraphInterface.ControlBottom
        Get
            Return Control_BottomY
            Invalidate()
        End Get
    End Property
    Private Shared Control_Width As Single
    <Browsable(True), Category("Layout"), Description("(Read Only) Returns The Control Width In Custom Coordinances.")>
    Public ReadOnly Property ControlWidth As Single Implements IGraphInterface.ControlWidth
        Get
            Return Control_Width
            Invalidate()
        End Get
    End Property
    Private Shared Control_Height As Single
    <Browsable(True), Category("Layout"), Description("(Read Only) Returns The Control Height In Custom Coordinances.")>
    Public ReadOnly Property ControlHeight As Single Implements IGraphInterface.ControlHeight
        Get
            Return Control_Height
            Invalidate()
        End Get
    End Property
    Private Sub TestGraphCtl_Load(sender As Object, e As EventArgs) Handles Me.Load
        'Specify A PrintDocument Instance For The PrintPreviewDialog Component.
        PrintPreviewDialog1.Document = PrintDocument1
        PrintDialog1.Document = PrintDocument1
        PrintDocument1.PrinterSettings = PageSetupDialog1.PrinterSettings
        PrintDocument1.DefaultPageSettings.Landscape = True
        PageSetupDialog1.Document = PrintDocument1
        PrintPreviewDialog1.PrintPreviewControl.Zoom = 0.35
        BorderStyle = BorderStyle.FixedSingle
        GridLabelFont = New Font("Times New Roman", 9, FontStyle.Regular)
        'Create The Folder To Place All TestGraph JPEG And Data Files
        Dim TestGraphFolder As String = "\TestGraph Documents"
        Dim tgPath As String = System.Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments)
        If (Not Directory.Exists(tgPath & TestGraphFolder)) Then
            My.Computer.FileSystem.CreateDirectory(tgPath & TestGraphFolder)
        End If
        SetStyle(ControlStyles.UserPaint, True)
        SetStyle(ControlStyles.AllPaintingInWmPaint, True)
        SetStyle(ControlStyles.OptimizedDoubleBuffer, True)
    End Sub
    Private Sub PicYAxis_Paint(sender As Object, e As PaintEventArgs) Handles picYAxis.Paint
        If YAxis_Text = vbNullString Then Exit Sub 'Don't Paint Picture Text
        'Rotate Font 270 Degrees Clockwise (Draw Upward)
        e.Graphics.RotateTransform(270)
        'Point To Center Of Picturebox Drawing Area
        e.Graphics.TranslateTransform(picYAxis.ClientSize.Width / 2, picYAxis.ClientSize.Height / 2, MatrixOrder.Append)
        'Match Font And Color With Other Label (LblFooter) and Draw The Text
        Dim YAxisBrush = New SolidBrush(Fore_Color)
        'Allign Text to Center of PictureBox.
        Dim String_format As New StringFormat(StringFormatFlags.NoClip)
        String_format.Alignment = StringAlignment.Center
        String_format.LineAlignment = StringAlignment.Center
        'Draw The Text
        picYAxis.Font = Default_Font
        e.Graphics.DrawString(YAxisText, Default_Font, YAxisBrush, 0, 0, String_format)
        'Restore Graphic State To Original
        e.Graphics.ResetTransform()
        e.Graphics.Transform.Dispose()
        String_format.Dispose()
        YAxisBrush.Dispose()
    End Sub
    Const LeftPercent As Integer = 8 'Percent Value Of Control On Left Side Reserved For Y Axis Test Units
    Const RightPercent As Integer = 8 'Percent Value Of Control On Right Side Reserved For Y Axis Test Units
    Const TopPercent As Integer = 10 'Percent Value Of Control Reserved For Header (Test Description)
    Const BottomPercent As Integer = 16 'Percent Value Of Control Reserved For Foot Notes (X Axis Test Units)
    Const ViewportHeightPercent As Integer = 74 'Percent Value = 74 Percent OF Controls Height Occupied By Viewport.
    Const ViewportWidthPercent As Integer = 84 'Percent Value = 84 Percent Of Controls Width Occupied By Viewport. 
    Private Sub TestGraphCtl_Paint(sender As Object, e As PaintEventArgs) Handles Me.Paint
        'Scale Is Being Changed
        If GraphAreaX1 = GraphAreaX2 Then Exit Sub
        If GraphAreaY2 = GraphAreaY1 Then Exit Sub
        '*******Position All Controls *******
        lblHeader.BackColor = Back_Color
        lblXAxis.BackColor = Back_Color
        picYAxis.BackColor = Back_Color
        BackColor = Back_Color
        lblHeader.ForeColor = Fore_Color
        lblXAxis.ForeColor = Fore_Color
        picYAxis.ForeColor = Fore_Color
        'Position MouseMove Caption Inside Graph Area
        LblMouseMove.Location = New Point(ClientSize.Width * ((LeftPercent + 0.5) / 100), ClientSize.Height * ((TopPercent + 0.5) / 100))
        LblMouseMove.Font = New Font(Font.Name, 9, FontStyle.Regular)
        LblMouseMove.BackColor = Fill_Color
        LblMouseMove.ForeColor = GridLine_Color
        LblMouseMove.Visible = Show_MousePosition
        'ShowMousePosition = Show_MousePosition
        'Position lblHeader.Text Outside Graph Area
        lblHeader.Font = Default_Font
        lblHeader.AutoSize = True
        lblHeader.Text = HeaderText
        lblHeader.Location = New Point((ClientSize.Width - lblHeader.Width) / 2, 0)
        'Position lblXAxis.Text Outside Graph Area
        lblXAxis.Font = Default_Font
        lblXAxis.AutoSize = True
        lblXAxis.Text = XAxisText
        lblXAxis.Location = New Point((ClientSize.Width - lblXAxis.Width) / 2, ClientSize.Height - lblXAxis.Height)
        'Position picYaxis.Location According To X Axis Inverse Or Not
        picYAxis.Width = lblXAxis.Height
        picYAxis.Height = ClientSize.Height * (ViewportHeightPercent / 100)
        If GraphAreaX1 < GraphAreaX2 Then
            picYAxis.Left = ClientSize.Width - (picYAxis.Width)
        Else
            picYAxis.Left = Left
        End If
        picYAxis.Location = New Point((picYAxis.Left), ((ClientSize.Height) - picYAxis.Height) - (BottomPercent / 100) * ClientSize.Height)
        picYAxis.Refresh()
        'Save The Current Graphics State
        Dim OriginalState As GraphicsState = e.Graphics.Save()
        SetControlScale(e.Graphics)
        Dim Xzero As Single
        Dim Xmax As Single
        Dim Yzero As Single
        Dim Ymax As Single
        If Not X_Inverse Then
            Xzero = GraphAreaX1
            Xmax = GraphAreaX2
        Else
            Xzero = GraphAreaX2
            Xmax = GraphAreaX1
        End If
        If Not Y_Inverse Then
            Yzero = GraphAreaY1
            Ymax = GraphAreaY2
        Else
            Yzero = GraphAreaY2
            Ymax = GraphAreaY1
        End If
        e.Graphics.SmoothingMode = SmoothingMode.HighQuality
        'Fill The Graph Area With Grid X And Y Grid Lines.
        If XAxisStyle = ScaleStyleChoices.Linear Then
            DrawLinearX_Grid(e.Graphics, GraphScale_Width, Xzero, Yzero, Xmax, Ymax)
        Else
            DrawLogX_Grid(e.Graphics, Xzero, Yzero, Xmax, Ymax)
        End If
        If YAxisStyle = ScaleStyleChoices.Linear Then
            DrawLinearY_Grid(e.Graphics, GraphScale_Height, Xzero, Yzero, Xmax, Ymax)
        Else
            DrawLogY_Grid(e.Graphics, Xzero, Yzero, Xmax, Ymax)
        End If
        If DrawLine_Changed Then New_DrawLine(e.Graphics) 'Interface Method
        If Pie_Changed Then New_DrawPie(e.Graphics) 'Interface Method
        'Restore Graphics State To Label Grid Line Values Outside Of Graph Area
        e.Graphics.Restore(OriginalState)
        'Restore Graphics And Label The Grid Lines.
        If XAxisStyle = ScaleStyleChoices.Linear Then
            DrawLinearX_GridLabel(e.Graphics, GraphScale_Width, Control_LeftX, Control_RightX, Xzero, Xmax)
        Else
            DrawLogX_GridLabel(e.Graphics, Control_LeftX, Control_RightX, Xzero, Xmax)
        End If
        If YAxisStyle = ScaleStyleChoices.Linear Then
            DrawLinearY_GridLabel(e.Graphics, GraphScale_Height, Control_TopY, Control_BottomY, Yzero, Ymax)
        Else
            DrawLogY_GridLabel(e.Graphics, GraphScale_Height, Control_TopY, Control_BottomY, Yzero, Ymax)
        End If
        '*****Draw Text*******
        If DrawString_Changed Then 'Interface Method
            New_DrawString(e.Graphics, Control_LeftX, Control_RightX, Control_TopY, Control_BottomY)
        End If
    End Sub
    Private Sub SetControlScale(eG As Graphics)
        If XAxisStyle = ScaleStyleChoices.Linear Then
            GraphScale_Width = Abs(GraphAreaX2 - GraphAreaX1) '-100,100 or -100,-50 or 50,100 ext...
            Control_Width = GraphScale_Width / (ViewportWidthPercent / 100)
            If GraphAreaX1 < GraphAreaX2 Then
                X_Inverse = False
                Control_RightX = GraphAreaX2 + (RightPercent / 100 * Control_Width)
                Control_LeftX = GraphAreaX1 - (LeftPercent / 100 * Control_Width)
            End If
            If GraphAreaX1 > GraphAreaX2 Then
                X_Inverse = True
                Control_RightX = GraphAreaX2 - (RightPercent / 100 * Control_Width)
                Control_LeftX = GraphAreaX1 + (LeftPercent / 100 * Control_Width)
            End If
        Else 'Log(e),Log(2),Log(10),Log(20
            GraphScale_Width = Abs(Log(GraphAreaX2) / Log(XAxisStyle) - Log(GraphAreaX1) / Log(XAxisStyle)) '-100,100 or -100,-50 or 50,100 ext...
            Control_Width = GraphScale_Width / (ViewportWidthPercent / 100)
            If GraphAreaX1 < GraphAreaX2 Then
                X_Inverse = False
                Control_RightX = Log(GraphAreaX2) / Log(XAxisStyle) + (RightPercent / 100 * Control_Width)
                Control_LeftX = Log(GraphAreaX1) / Log(XAxisStyle) - (LeftPercent / 100 * Control_Width)
            End If
            If GraphAreaX1 > GraphAreaX2 Then
                X_Inverse = True
                Control_RightX = Log(GraphAreaX2) / Log(XAxisStyle) - (RightPercent / 100 * Control_Width)
                Control_LeftX = Log(GraphAreaX1) / Log(XAxisStyle) + (LeftPercent / 100 * Control_Width)
            End If
        End If
        If YAxisStyle = ScaleStyleChoices.Linear Then
            GraphScale_Height = Abs(GraphAreaY2 - GraphAreaY1) '100,-100 or 50,-1000 or 0,-100 ext...
            Control_Height = GraphScale_Height / (ViewportHeightPercent / 100)
            If GraphAreaY1 < GraphAreaY2 Then
                Y_Inverse = False
                Control_BottomY = GraphAreaY1 - (BottomPercent / 100 * Control_Height)
                Control_TopY = GraphAreaY2 + (TopPercent / 100 * Control_Height)
            End If
            If GraphAreaY1 > GraphAreaY2 Then
                Y_Inverse = True
                Control_BottomY = GraphAreaY1 + (BottomPercent / 100 * Control_Height)
                Control_TopY = GraphAreaY2 - (TopPercent / 100 * Control_Height)
            End If
        Else 'Log(e),Log(2),Log(10),Log(20
            GraphScale_Height = Abs(Log(GraphAreaY2) / Log(YAxisStyle) - Log(GraphAreaY1) / Log(YAxisStyle)) '100,-100 or 50,-1000 or 0,-100 ext...
            Control_Height = GraphScale_Height / (ViewportHeightPercent / 100)
            If GraphAreaY1 < GraphAreaY2 Then
                Y_Inverse = False
                Control_BottomY = Log(GraphAreaY1) / Log(YAxisStyle) - (BottomPercent / 100 * Control_Height)
                Control_TopY = Log(GraphAreaY2) / Log(YAxisStyle) + (TopPercent / 100 * Control_Height)
            End If
            If GraphAreaY1 > GraphAreaY2 Then
                Y_Inverse = True
                Control_BottomY = Log(GraphAreaY1) / Log(YAxisStyle) + (BottomPercent / 100 * Control_Height)
                Control_TopY = Log(GraphAreaY2) / Log(YAxisStyle) - (TopPercent / 100 * Control_Height)
            End If
        End If
        'Reset The Graphics Transform
        eG.ResetTransform()
        'Scale The Control Dimensions To Accomidate Graph Dimensions
        'Map The Custom Dimensions To The Control Width And Height
        eG.ScaleTransform(ClientSize.Width / (Control_RightX - Control_LeftX), ClientSize.Height / (Control_BottomY - Control_TopY))
        Dim bounds As RectangleF = eG.ClipBounds
        'Translate X,Y Origins To The Control
        eG.TranslateTransform(-Control_LeftX, -Control_TopY)
        'Display The Mouse Transform Coordinances
        'On The Mouse Label
        Mouse_InverseTransform = eG.Transform
        Mouse_InverseTransform.Invert()
        'Check For Inverse Scales And Adjust Dimensional Variables Accordingly.
        Dim RecfX As Single
        Dim RecfY As Single
        If XAxisStyle = ScaleStyleChoices.Linear Then
            If Not X_Inverse Then
                RecfX = GraphAreaX1
            Else
                RecfX = GraphAreaX2
            End If
        Else
            If Not X_Inverse Then
                RecfX = Log(GraphAreaX1) / Log(XAxisStyle)
            Else
                RecfX = Log(GraphAreaX2) / Log(XAxisStyle)
            End If
        End If
        If YAxisStyle = ScaleStyleChoices.Linear Then
            If Not Y_Inverse Then
                RecfY = GraphAreaY1
            Else
                RecfY = GraphAreaY2
            End If
        Else
            If Not Y_Inverse Then
                RecfY = Log(GraphAreaY1) / Log(YAxisStyle)
            Else
                RecfY = Log(GraphAreaY2) / Log(YAxisStyle)
            End If
        End If
        'Draw A Border Around Graph Area Using BorderPen  
        'Size BorderPen To The Smal1est Dimension (Width vs Height)
        'Without Resizing BorderPen, The Pen Size Increases as Dimensions Decrease and Vice Versa.
        Dim BorderPen_Size As Single
        If GraphScale_Height <= GraphScale_Width Then
            BorderPen_Size = 0.5 * GraphScale_Height / 100
        Else
            BorderPen_Size = 0.5 * GraphScale_Width / 100
        End If
        Dim BorderPen As New Pen(GridLine_Color, BorderPen_Size)
        Dim FillBrush = New SolidBrush(Fill_Color)
        Viewport_Rect = New RectangleF(RecfX, RecfY, GraphScale_Width, GraphScale_Height)
        eG.SetClip(Viewport_Rect)
        eG.Clip = New Region(Viewport_Rect)
        eG.FillRegion(FillBrush, eG.Clip)
        eG.DrawRectangle(pen:=BorderPen, Viewport_Rect.Left, Viewport_Rect.Top, Viewport_Rect.Width, Viewport_Rect.Height)
        FillBrush.Dispose()
        BorderPen.Dispose()
    End Sub
    Private Sub New_DrawPie(eG As Graphics)
        'Make Pie Rectangle A Square To Produce A Symetrical Circle
        'Scale Height Is Used As Reference.
        Dim YFactor As Single = ClientSize.Height - (ClientSize.Height * ((TopPercent + BottomPercent) / 100))
        Dim XFactor As Single = ClientSize.Width - (ClientSize.Width * ((LeftPercent + RightPercent) / 100))
        Dim XY_Ratio As Single = YFactor / XFactor
        Dim SweepAngle As Single
        'Define The Pie Rectangle
        'RectangleF Structure
        Dim Pie_RectF As New RectangleF
        With Pie_RectF
            .Width = GraphScale_Width
            .Height = GraphScale_Height
            .X = 0
            .Y = 0
        End With
        eG.ScaleTransform(XY_Ratio, 1) 'Increases Or Decreases The Size Of The Coordinate System
        Dim WidthTransformed = Pie_RectF.Width * XY_Ratio
        Pie_RectF.Location = New Point(0, 0)
        'Used Only To Display The Rectangle For Coding
        Dim PiePen As New Pen(Color.Red, 0.1)
        eG.SmoothingMode = SmoothingMode.AntiAlias
        Dim StartAngle As Single = 0
        For Index = 1 To PieIndex
            SweepAngle = (New_PiePiece(Index) * 360) / 100
            eG.DrawPie(PiePen, Pie_RectF, StartAngle, SweepAngle)
            eG.FillPie(New_PieBrush(Index), Rectangle.Ceiling(Pie_RectF), StartAngle, SweepAngle)
            StartAngle += SweepAngle
        Next
        eG.ResetTransform()
        'Paint Pie Labels
        Dim PieLabelFont As New Font(Pie_Font.FontFamily, Pie_Font.Size, Pie_Font.Style)
        Dim Ypos As Single = GraphAreaY2 + PieLabelFont.Size / 3
        Dim Xpos As Single = GraphAreaX2 - 1
        Dim X_Point As Single = GetClientPoint(0, ClientSize.Width, ControlLeft, ControlRight, Xpos)
        Dim String_format As New StringFormat(StringFormatFlags.NoClip)
        String_format.Alignment = StringAlignment.Far
        String_format.LineAlignment = StringAlignment.Far
        String_format.FormatFlags = StringFormatFlags.NoWrap
        For Index = 1 To PieIndex
            Dim Y_Point As Single = GetClientPoint(0, ClientSize.Height, ControlTop, ControlBottom, Ypos)
            eG.DrawString(New_PieLabels(Index), PieLabelFont, New_PieBrush(Index), X_Point, Y_Point, String_format)
            Ypos += PieLabelFont.Size / 5
        Next
        WriteGraphData()
        String_format.Dispose()
        PiePen.Dispose()
        PieLabelFont.Dispose()
        eG.Transform.Dispose()
    End Sub
    Private Sub New_DrawLine(eG As Graphics)
        'No Arguments And Index Count Required Since TestPoints And Index Count Are Stored in Shared Arrays
        '*******Paint The Interface Method DrawLine*******
        Dim Index As Integer
        Dim LinePts(4) As PointF
        Dim HatchStyles As HatchStyle
        Dim X1 As Single
        Dim X2 As Single
        Dim Y1 As Single
        Dim Y2 As Single
        Dim Y3 As Single
        eG.SmoothingMode = SmoothingMode.AntiAlias
        For Index = 1 To DrawLine_ArrayIndex
            If XAxisStyle <> ScaleStyleChoices.Linear Then
                X1 = Log(New_DrawLineX1(Index)) / Log(XAxisStyle)
                X2 = Log(New_DrawLineX2(Index)) / Log(XAxisStyle)
                New_DrawingPen(Index).Width /= 100
            Else
                X1 = New_DrawLineX1(Index)
                X2 = New_DrawLineX2(Index)
            End If
            If YAxisStyle <> ScaleStyleChoices.Linear Then
                Y1 = Log(New_DrawLineY1(Index)) / Log(YAxisStyle)
                Y2 = Log(New_DrawLineY2(Index)) / Log(YAxisStyle)
                New_DrawingPen(Index).Width /= 100
                If Not Y_Inverse Then
                    Y3 = Log(GraphAreaY1) / Log(YAxisStyle)
                Else
                    Y3 = Log(GraphAreaY2) / Log(YAxisStyle)
                End If
            Else
                If Not Y_Inverse Then
                    Y3 = GraphAreaY1
                Else
                    Y3 = GraphAreaY2
                End If
                Y1 = New_DrawLineY1(Index)
                Y2 = New_DrawLineY2(Index)
            End If
            LinePts(0).X = X1
            LinePts(0).Y = Y1
            LinePts(1).X = X2
            LinePts(1).Y = Y2
            LinePts(2).X = X2
            LinePts(2).Y = Y3
            LinePts(3).X = X1
            LinePts(3).Y = Y3
            LinePts(4).X = X1
            LinePts(4).Y = Y1
            If Graph_Style = GraphStyleChoices.PointToPoint Then 'No Bar Style
                eG.DrawLine(New_DrawingPen(Index), X1, Y1, X2, Y2)
            Else 'GraphStyleChoices.Bar
                If Random_HatchStyles Then
                    'Generate Random Numbers To Be Used For Color Arguments
                    'Random Colors For Hatch Style Brush
                    Randomize()
                    Dim random As New Random()
                    HatchStyles = CInt(Math.Floor((52 - 1 + 1) * Rnd())) + 1 'Random HatchStyles
                    Dim FColor1 As Integer = CInt(Math.Floor((255 - 1 + 1) * Rnd())) + 1 'Random Forecolor
                    Dim FColor2 As Integer = CInt(Math.Floor((255 - 1 + 1) * Rnd())) + 1 'Random Forecolor
                    Dim BColor1 As Integer = CInt(Math.Floor((255 - 1 + 1) * Rnd())) + 1 'Random Backcolor
                    Dim BColor2 As Integer = CInt(Math.Floor((255 - 1 + 1) * Rnd())) + 1 'Random Backcolor
                    Dim Hatch_Brush As New HatchBrush(HatchStyles, Color.FromArgb(100, 255, FColor1, FColor2), Color.FromArgb(100, 255, BColor1, BColor2))
                    eG.DrawLine(New_DrawingPen(Index), X1, Y1, X2, Y2)
                    'Flood Polygon with Bright Solid Color First (Just Looks Better)
                    Dim Solid_Brush As New SolidBrush(Color.Gray)
                    eG.FillPolygon(Solid_Brush, LinePts)
                    eG.FillPolygon(Hatch_Brush, LinePts)
                    Hatch_Brush.Dispose()
                    Solid_Brush.Dispose()
                Else
                    eG.DrawLine(New_DrawingPen(Index), X1, Y1, X2, Y2)
                    eG.FillPolygon(New_HatchBrush(Index), LinePts)
                End If
            End If
        Next
        WriteGraphData()
    End Sub
    Private Sub New_DrawString(eG As Graphics, LeftX As Single, RightX As Single, TopY As Single, BottomY As Single)
        eG.SmoothingMode = SmoothingMode.AntiAlias
        Dim Index As Integer
        Dim Xpos As Single
        Dim Ypos As Single
        For Index = 1 To DrawString_ArrayIndex
            If XAxisStyle <> ScaleStyleChoices.Linear Then
                Xpos = Log(New_DrawStringX(Index)) / Log(XAxisStyle)
            Else
                Xpos = New_DrawStringX(Index)
            End If
            If YAxisStyle <> ScaleStyleChoices.Linear Then
                Ypos = Log(New_DrawStringY(Index)) / Log(YAxisStyle)
            Else
                Ypos = New_DrawStringY(Index)
            End If
            Dim X_Point As Single = GetClientPoint(0, ClientSize.Width, LeftX, RightX, Xpos)
            Dim Y_Point As Single = GetClientPoint(0, ClientSize.Height, TopY, BottomY, Ypos)
            'Actual RotationAngle = (Angle * 2), 22.5 = 45 etc...
            If New_DrawStringTextRotation(Index) <> 0 Then
                eG.ScaleTransform(1, 1) 'Increases Or Decreases The Size Of The Coordinate System
                eG.TranslateTransform(X_Point, Y_Point, MatrixOrder.Append)
                eG.RotateTransform(New_DrawStringTextRotation(Index))
                eG.DrawString(New_DrawStringText(Index), New_DrawStringFont(Index), New_DrawStringBrush(Index), 0, 0, New_DrawStringFormat(Index))
                eG.ResetTransform()
            Else
                eG.DrawString(New_DrawStringText(Index), New_DrawStringFont(Index), New_DrawStringBrush(Index), X_Point, Y_Point, New_DrawStringFormat(Index))
            End If
        Next
        eG.Transform.Dispose()
    End Sub
    Private Sub TestGraphCtl_ForeColorChanged(sender As Object, e As EventArgs) Handles Me.ForeColorChanged
        lblHeader.ForeColor = Fore_Color
        lblXAxis.ForeColor = Fore_Color
        picYAxis.ForeColor = Fore_Color
    End Sub
    Private Sub DrawLinearX_Grid(ByVal eG As Graphics, xWidth As Single, xMin As Single, yMin As Single, xMax As Single, yMax As Single)
        If XAxisStyle <> ScaleStyleChoices.Linear Then Exit Sub
        If Graph_Style = GraphStyleChoices.Pie Then Exit Sub
        If Not Show_XGrids Then Exit Sub
        'Determine The Grid Line Spacing
        Dim xGridLine_Spacing As Single = ReturnLinearGridSpacing(xWidth, 0)
        Dim Yrec_Max As Single
        Dim Yrec_Min As Single
        If YAxisStyle = ScaleStyleChoices.Linear Then
            Yrec_Max = yMax
            Yrec_Min = yMin
        Else
            Yrec_Max = Log(yMax) / Log(YAxisStyle)
            Yrec_Min = Log(yMin) / Log(YAxisStyle)
        End If
        'Draw The X Grid Lines Inside The Graphing Area
        eG.SmoothingMode = SmoothingMode.HighQuality
        Dim Grid_pen As New Pen(GridLine_Color, 0.001)
        Dim n As Integer = 0
        For X As Single = xMin To xMax Step xGridLine_Spacing
            Dim Points1 = New PointF(X, Yrec_Min)
            Dim Points2 = New PointF(X, Yrec_Max)
            'Draw The X Grid Lines Inside The Graphing Area
            eG.DrawLine(Grid_pen, Points1, Points2)
            n += 1
            If X >= xMax Or n > 100 Then Exit For
        Next X
        Grid_pen.Dispose()
    End Sub
    Private Sub DrawLinearX_GridLabel(ByVal eG As Graphics, xWidth As Single, xLeft As Single, xRight As Single, xMin As Single, xMax As Single)
        If XAxisStyle <> ScaleStyleChoices.Linear Then Exit Sub
        If Graph_Style = GraphStyleChoices.Pie Then Exit Sub
        If Not Show_XLabels And Not Show_CustomXLabels Then Exit Sub
        Dim yPos As Single
        If YAxisStyle = ScaleStyleChoices.Linear Then
            yPos = GetClientPoint(0, ClientSize.Height, ControlTop, ControlBottom, GraphAreaY1)
        Else 'Log(x)
            yPos = GetClientPoint(0, ClientSize.Height, ControlTop, ControlBottom, Log(GraphAreaY1) / Log(YAxisStyle))
        End If
        Dim Y_Pointx As Single
        Dim String_format As New StringFormat
        Select Case Rotate_XGridLabels
            Case = 90
                Y_Pointx = yPos + (yPos * 0.01)
                String_format.Alignment = StringAlignment.Near
                String_format.LineAlignment = StringAlignment.Center
                String_format.FormatFlags = StringFormatFlags.NoWrap
            Case >= 45
                Y_Pointx = yPos + (yPos * 0.02)
                String_format.Alignment = StringAlignment.Near
                String_format.LineAlignment = StringAlignment.Center
                String_format.FormatFlags = StringFormatFlags.NoWrap
            Case > 0
                Y_Pointx = yPos + (yPos * 0.025)
                String_format.Alignment = StringAlignment.Near
                String_format.LineAlignment = StringAlignment.Center
                String_format.FormatFlags = StringFormatFlags.NoWrap
            Case = 0
                Y_Pointx = yPos + (yPos * 0.03)
                String_format.Alignment = StringAlignment.Center
                String_format.LineAlignment = StringAlignment.Center
                String_format.FormatFlags = StringFormatFlags.DirectionRightToLeft
            Case > -45
                Y_Pointx = yPos + (yPos * 0.025)
                String_format.Alignment = StringAlignment.Far
                String_format.LineAlignment = StringAlignment.Center
                String_format.FormatFlags = StringFormatFlags.NoWrap
            Case > -90
                Y_Pointx = yPos + (yPos * 0.02)
                String_format.Alignment = StringAlignment.Far
                String_format.LineAlignment = StringAlignment.Center
                String_format.FormatFlags = StringFormatFlags.NoWrap
            Case = -90
                Y_Pointx = yPos + (yPos * 0.01)
                String_format.Alignment = StringAlignment.Far
                String_format.LineAlignment = StringAlignment.Center
                String_format.FormatFlags = StringFormatFlags.NoWrap
        End Select
        Dim myBrush = New SolidBrush(GridLabel_Color)
        'Draw The X Grid Lines Labels Outside The Graph Area
        'Determine The Grid Line Spacing
        Dim xGridLine_Spacing As Single = ReturnLinearGridSpacing(xWidth, 0)
        'Draw TheX Grid Line Labels Outside The Graph Area
        eG.SmoothingMode = SmoothingMode.HighQuality
        Dim n As Integer = 0
        For X As Single = xMin To xMax Step xGridLine_Spacing
            Dim X_Pointx As Single = GetClientPoint(0, ClientSize.Width, xLeft, xRight, X)
            Dim xLabelStr As String
            If Not Show_CustomXLabels Then
                xLabelStr = ReturnTruncated(X)
            Else
                If n > UBound(New_CustomXGridLine_Labels) Then Exit For
                xLabelStr = New_CustomXGridLine_Labels(n)
            End If
            Dim Points = New PointF(X_Pointx, Y_Pointx)
            eG.ScaleTransform(1, 1) 'Increases Or Decreases The Size Of The Coordinate System
            eG.TranslateTransform(X_Pointx, Y_Pointx, MatrixOrder.Append)
            eG.RotateTransform(Rotate_XGridLabels)
            eG.DrawString(xLabelStr, Grid_Font, myBrush, 0, 0, String_format)
            eG.ResetTransform()
            n += 1
            If X >= xMax Or n > 100 Then Exit For
        Next X
        String_format.Dispose()
        myBrush.Dispose()
        eG.Transform.Dispose()
    End Sub
    Private Sub DrawLinearY_Grid(ByVal eG As Graphics, yHeight As Single, xLeft As Single, yBottom As Single, xRight As Single, yTop As Single)
        If YAxisStyle <> ScaleStyleChoices.Linear Then Exit Sub
        If Graph_Style = GraphStyleChoices.Pie Then Exit Sub
        If Not Show_YGrids Then Exit Sub
        'Determine The Grid Line Spacing
        Dim yGridLine_Spacing As Single = ReturnLinearGridSpacing(0, yHeight)
        Dim Xrec_Left As Single
        Dim Xrec_Right As Single
        If XAxisStyle = ScaleStyleChoices.Linear Then
            Xrec_Left = xLeft
            Xrec_Right = xRight
        Else
            Xrec_Left = Log(xLeft) / Log(XAxisStyle)
            Xrec_Right = Log(xRight) / Log(XAxisStyle)
        End If
        'Draw The Linear Y Grid Lines Inside The Graphing Area
        Dim Grid_pen As New Pen(GridLine_Color, 0.001)
        eG.SmoothingMode = SmoothingMode.HighQuality
        For y As Single = yBottom To yTop Step yGridLine_Spacing
            'Draw The Y Grid Lines Inside The Graphing Area
            Dim Points1 = New PointF(Xrec_Left, y)
            Dim Points2 = New PointF(Xrec_Right, y)
            eG.DrawLine(Grid_pen, Points1, Points2)
        Next y
        'Make Sure First Last Line Gets Painted
        eG.DrawLine(Grid_pen, Xrec_Left, yTop, Xrec_Right, yTop)
        eG.DrawLine(Grid_pen, Xrec_Left, yBottom, Xrec_Right, yBottom)
        Grid_pen.Dispose()
    End Sub
    Private Sub DrawLinearY_GridLabel(ByVal eG As Graphics, yHeight As Single, yTop As Single, yBottom As Single, yStart As Single, yEnd As Single)
        If YAxisStyle <> ScaleStyleChoices.Linear Then Exit Sub
        If Graph_Style = GraphStyleChoices.Pie Then Exit Sub
        If Not Show_YLabels And Not Show_CustomYLabels Then Exit Sub
        Dim String_format As New StringFormat
        Dim X_Pointy As Single
        Dim XPos As Single
        If GraphAreaX1 < GraphAreaX2 Then 'Labels On Left Side
            Select Case XAxisStyle
                Case = ScaleStyleChoices.Linear
                    XPos = GetClientPoint(0, ClientSize.Width, ControlLeft, ControlRight, GraphAreaX1)
                Case Else 'Log(x) On Left
                    XPos = GetClientPoint(0, ClientSize.Width, ControlLeft, ControlRight, Log(GraphAreaX1) / Log(XAxisStyle))
            End Select
            Select Case Rotate_YGridLabels'Labels On Left Side
                Case = 90
                    X_Pointy = XPos - (XPos * 0.1)
                    String_format.Alignment = StringAlignment.Center
                    String_format.LineAlignment = StringAlignment.Center
                    String_format.FormatFlags = StringFormatFlags.NoWrap
                Case >= 45
                    X_Pointy = XPos - (XPos * 0.08)
                    String_format.Alignment = StringAlignment.Far
                    String_format.LineAlignment = StringAlignment.Center
                    String_format.FormatFlags = StringFormatFlags.NoWrap
                Case > 0
                    X_Pointy = XPos - (XPos * 0.04)
                    String_format.Alignment = StringAlignment.Far
                    String_format.LineAlignment = StringAlignment.Center
                    String_format.FormatFlags = StringFormatFlags.NoWrap
                Case = 0
                    X_Pointy = XPos - (XPos * 0.03)
                    String_format.Alignment = StringAlignment.Far
                    String_format.LineAlignment = StringAlignment.Center
                    String_format.FormatFlags = StringFormatFlags.NoWrap
                Case > -45
                    X_Pointy = XPos - (XPos * 0.05)
                    String_format.Alignment = StringAlignment.Far
                    String_format.LineAlignment = StringAlignment.Center
                    String_format.FormatFlags = StringFormatFlags.NoWrap
                Case > -90
                    X_Pointy = XPos - (XPos * 0.07)
                    String_format.Alignment = StringAlignment.Far
                    String_format.LineAlignment = StringAlignment.Center
                    String_format.FormatFlags = StringFormatFlags.NoWrap
                Case = -90
                    X_Pointy = XPos - (XPos * 0.1)
                    String_format.Alignment = StringAlignment.Center
                    String_format.LineAlignment = StringAlignment.Center
                    String_format.FormatFlags = StringFormatFlags.NoWrap
            End Select
        Else 'Labels On Right Side, GraphAreaX1 > GraphAreaX2
            Select Case XAxisStyle
                Case = ScaleStyleChoices.Linear
                    XPos = GetClientPoint(0, ClientSize.Width, ControlLeft, ControlRight, GraphAreaX2)
                Case Else 'Log(x) On Right
                    XPos = GetClientPoint(0, ClientSize.Width, ControlLeft, ControlRight, Log(GraphAreaX2) / Log(XAxisStyle))
            End Select
            Select Case Rotate_YGridLabels'Labels On Right Side
                Case = 90
                    X_Pointy = XPos + (XPos * 0.01)
                    String_format.Alignment = StringAlignment.Center
                    String_format.LineAlignment = StringAlignment.Center
                    String_format.FormatFlags = StringFormatFlags.DirectionRightToLeft
                Case >= 45
                    X_Pointy = XPos + (XPos * 0.007)
                    String_format.Alignment = StringAlignment.Far
                    String_format.LineAlignment = StringAlignment.Center
                    String_format.FormatFlags = StringFormatFlags.DirectionRightToLeft
                Case > 0
                    X_Pointy = XPos + (XPos * 0.005)
                    String_format.Alignment = StringAlignment.Far
                    String_format.LineAlignment = StringAlignment.Center
                    String_format.FormatFlags = StringFormatFlags.DirectionRightToLeft
                Case = 0
                    X_Pointy = XPos + (XPos * 0.005)
                    String_format.Alignment = StringAlignment.Far
                    String_format.LineAlignment = StringAlignment.Center
                    String_format.FormatFlags = StringFormatFlags.DirectionRightToLeft
                Case > -45
                    X_Pointy = XPos + (XPos * 0.005)
                    String_format.Alignment = StringAlignment.Far
                    String_format.LineAlignment = StringAlignment.Center
                    String_format.FormatFlags = StringFormatFlags.DirectionRightToLeft
                Case > -90
                    X_Pointy = XPos + (XPos * 0.008)
                    String_format.Alignment = StringAlignment.Far
                    String_format.LineAlignment = StringAlignment.Center
                    String_format.FormatFlags = StringFormatFlags.DirectionRightToLeft
                Case = -90
                    X_Pointy = XPos + (XPos * 0.012)
                    String_format.Alignment = StringAlignment.Center
                    String_format.LineAlignment = StringAlignment.Center
                    String_format.FormatFlags = StringFormatFlags.DirectionRightToLeft
            End Select
        End If
        'Determine The Grid Line Spacing
        Dim yGridLine_Spacing As Single = ReturnLinearGridSpacing(0, yHeight)
        'Draw TheX Grid Line Labels Outside The Graph Area
        eG.SmoothingMode = SmoothingMode.HighQuality
        Dim n As Integer = 0
        Dim myBrush As New SolidBrush(GridLabel_Color)
        For y = yStart To yEnd Step yGridLine_Spacing
            'Translate The Custom Scale Point To The Original Control Scale Point
            Dim Y_Pointy As Single = GetClientPoint(0, ClientSize.Height, yTop, yBottom, y)
            Dim yLabelStr As String
            If Not Show_CustomYLabels Then
                yLabelStr = ReturnTruncated(y)
            Else
                If n > UBound(New_CustomYGridLine_Labels) Then Exit For
                yLabelStr = New_CustomYGridLine_Labels(n)
            End If
            Dim Points = New PointF(X_Pointy, Y_Pointy)
            eG.ScaleTransform(1, 1) 'Increases Or Decreases The Size Of The Coordinate System
            eG.TranslateTransform(X_Pointy, Y_Pointy, MatrixOrder.Append)
            eG.RotateTransform(Rotate_YGridLabels)
            eG.DrawString(yLabelStr, Grid_Font, myBrush, 0, 0, String_format)
            eG.ResetTransform()
            n += 1
            If y >= yEnd Or n > 100 Then Exit For
        Next y
        String_format.Dispose()
        myBrush.Dispose()
        eG.Transform.Dispose()
    End Sub
    Private Sub DrawLogX_Grid(ByVal eG As Graphics, ByVal xMin As Single, ByVal yMin As Single, ByVal xMax As Single, ByVal yMax As Single)
        If XAxisStyle = ScaleStyleChoices.Linear Then Exit Sub
        If Graph_Style = GraphStyleChoices.Pie Then Exit Sub
        If Not Show_XGrids Then Exit Sub
        Dim yStart As Single
        Dim yEnd As Single
        If YAxisStyle <> ScaleStyleChoices.Linear Then
            yStart = Log(yMin) / Log(YAxisStyle)
            yEnd = Log(yMax) / Log(YAxisStyle)
        Else
            yStart = yMin
            yEnd = yMax
        End If
        Dim GridSpacing As Single = 0
        Dim LogXNow As Single
        Dim XNow As Single = xMin
        Dim MaxLines As Integer = 0
        Dim Grid_pen As New Pen(GridLine_Color, 0.001)
        Dim Octaves As Single = Round(((Log(xMax) / Log(10)) - 1) - ((Log(xMin) / Log(10)) - 1), 2)
        eG.SmoothingMode = SmoothingMode.HighQuality
        Do
            Select Case Octaves
                Case Is <= 1 'Draw 10 Grid Lines
                    GridSpacing = CSng((xMax - xMin) / 10)
                Case Is < 10
                    Select Case XNow
                        Case Is < 1.0E-24 '<10p
                            GridSpacing = 1.0E-25
                        Case Is < 1.0E-23 '<10p
                            GridSpacing = 1.0E-24
                        Case Is < 1.0E-22 '<10p
                            GridSpacing = 1.0E-23
                        Case Is < 1.0E-21 '<10p
                            GridSpacing = 1.0E-22
                        Case Is < 1.0E-20 '<10p
                            GridSpacing = 1.0E-21
                        Case Is < 1.0E-19 '<10p
                            GridSpacing = 1.0E-20
                        Case Is < 1.0E-18 '<10p
                            GridSpacing = 1.0E-19
                        Case Is < 1.0E-17 '<10p
                            GridSpacing = 1.0E-18
                        Case Is < 0.0000000000000001 '<10p
                            GridSpacing = 1.0E-17
                        Case Is < 0.000000000000001 '<10p
                            GridSpacing = 0.0000000000000001
                        Case Is < 0.00000000000001 '<10p
                            GridSpacing = 0.000000000000001
                        Case Is < 0.0000000000001 '<10p
                            GridSpacing = 0.00000000000001
                        Case Is < 0.000000000001 '<10p
                            GridSpacing = 0.0000000000001
                        Case Is < 0.00000000001 '<10p
                            GridSpacing = 0.000000000001
                        Case Is < 0.0000000001 '<100p
                            GridSpacing = 0.00000000001
                        Case Is < 0.000000001 '<1n
                            GridSpacing = 0.0000000001
                        Case Is < 0.00000001 '<10n
                            GridSpacing = 0.000000001
                        Case Is < 0.0000001 '<100n
                            GridSpacing = 0.00000001
                        Case Is < 0.000001 '<1u
                            GridSpacing = 0.0000001
                        Case Is < 0.00001 '<10u
                            GridSpacing = 0.000001
                        Case Is < 0.0001 '<100u
                            GridSpacing = 0.00001
                        Case Is < 0.001 '<1m
                            GridSpacing = 0.0001
                        Case Is < 0.01 '<10m
                            GridSpacing = 0.001
                        Case Is < 0.1 '<100m
                            GridSpacing = 0.01
                        Case Is < 1
                            GridSpacing = 0.1
                        Case Is < 10
                            GridSpacing = 1
                        Case Is < 100
                            GridSpacing = 10
                        Case Is < 1000
                            GridSpacing = 100
                        Case Is < 10000
                            GridSpacing = 1000
                        Case Is < 100000
                            GridSpacing = 10000
                        Case Is < 1000000
                            GridSpacing = 100000
                        Case Is < 10000000 '10M
                            GridSpacing = 1000000
                        Case Is < 100000000 '100M
                            GridSpacing = 10000000
                        Case Is < 1000000000 '1000M
                            GridSpacing = 100000000
                        Case Is < 10000000000.0# '10G
                            GridSpacing = 1000000000
                        Case Is < 100000000000.0# '100G
                            GridSpacing = 10000000000.0#
                        Case Is < 1000000000000.0# '1000G
                            GridSpacing = 100000000000.0#
                        Case Is < 10000000000000.0#
                            GridSpacing = 1000000000000.0#
                        Case Is < 100000000000000.0#
                            GridSpacing = 10000000000000.0#
                        Case Is < 1.0E+15#
                            GridSpacing = 100000000000000.0#
                        Case Is < 1.0E+16#
                            GridSpacing = 1.0E+15#
                        Case Is < 1.0E+17#
                            GridSpacing = 1.0E+16#
                        Case Is < 1.0E+18#
                            GridSpacing = 1.0E+17#
                        Case Is < 1.0E+19#
                            GridSpacing = 1.0E+18#
                        Case Is < 1.0E+20#
                            GridSpacing = 1.0E+19#
                        Case Is < 1.0E+21#
                            GridSpacing = 1.0E+20#
                        Case Is < 1.0E+22#
                            GridSpacing = 1.0E+21#
                        Case Is < 1.0E+23#
                            GridSpacing = 1.0E+22#
                        Case Is < 1.0E+24#
                            GridSpacing = 1.0E+23#
                        Case Is < 1.0E+25#
                            GridSpacing = 1.0E+24#
                    End Select
                Case Is >= 10
                    Select Case XNow
                        Case Is < 1.0E-24 '<10p
                            GridSpacing = 1.0E-25
                        Case Is < 1.0E-23 '<10p
                            GridSpacing = 1.0E-24
                        Case Is < 1.0E-22 '<10p
                            GridSpacing = 1.0E-23
                        Case Is < 1.0E-21 '<10p
                            GridSpacing = 1.0E-22
                        Case Is < 1.0E-20 '<10p
                            GridSpacing = 1.0E-21
                        Case Is < 1.0E-19 '<10p
                            GridSpacing = 1.0E-20
                        Case Is < 1.0E-18 '<10p
                            GridSpacing = 1.0E-19
                        Case Is < 1.0E-17 '<10p
                            GridSpacing = 1.0E-18
                        Case Is < 0.0000000000000001 '<10p
                            GridSpacing = 1.0E-17
                        Case Is < 0.000000000000001 '<10p
                            GridSpacing = 0.0000000000000001
                        Case Is < 0.00000000000001 '<10p
                            GridSpacing = 0.000000000000001
                        Case Is < 0.0000000000001 '<10p
                            GridSpacing = 0.00000000000001
                        Case Is < 0.000000000001 '<10p
                            GridSpacing = 0.0000000000001
                        Case Is < 0.00000000001 '<10p
                            GridSpacing = 0.000000000001
                        Case Is < 0.0000000001 '<100p
                            GridSpacing = 0.00000000001
                        Case Is < 0.000000001 '<1n
                            GridSpacing = 0.0000000001
                        Case Is < 0.00000001 '<10n
                            GridSpacing = 0.000000001
                        Case Is < 0.0000001 '<100n
                            GridSpacing = 0.00000001
                        Case Is < 0.000001 '<1u
                            GridSpacing = 0.0000001
                        Case Is < 0.00001 '<10u
                            GridSpacing = 0.000001
                        Case Is < 0.0001 '<100u
                            GridSpacing = 0.00001
                        Case Is < 0.001 '<1m
                            GridSpacing = 0.0001
                        Case Is < 0.01 '<10m
                            GridSpacing = 0.001
                        Case Is < 0.1 '<100m
                            GridSpacing = 0.01
                        Case Is < 1
                            GridSpacing = 1
                        Case Is < 10
                            GridSpacing = 10
                        Case Is < 100
                            GridSpacing = 100
                        Case Is < 1000
                            GridSpacing = 1000
                        Case Is < 10000
                            GridSpacing = 10000
                        Case Is < 100000
                            GridSpacing = 100000
                        Case Is < 1000000
                            GridSpacing = 1000000
                        Case Is < 10000000 '10M
                            GridSpacing = 10000000
                        Case Is < 100000000 '100M
                            GridSpacing = 100000000
                        Case Is < 1000000000 '1000M
                            GridSpacing = 1000000000
                        Case Is < 10000000000.0# '10G
                            GridSpacing = 10000000000
                        Case Is < 100000000000.0# '100G
                            GridSpacing = 100000000000.0#
                        Case Is < 1000000000000.0# '1000G
                            GridSpacing = 1000000000000.0#
                        Case Is < 10000000000000.0#
                            GridSpacing = 10000000000000.0#
                        Case Is < 100000000000000.0#
                            GridSpacing = 100000000000000.0#
                        Case Is < 1.0E+15#
                            GridSpacing = 1.0E+15#
                        Case Is < 1.0E+16#
                            GridSpacing = 1.0E+16#
                        Case Is < 1.0E+17#
                            GridSpacing = 1.0E+17#
                        Case Is < 1.0E+18#
                            GridSpacing = 1.0E+18#
                        Case Is < 1.0E+19#
                            GridSpacing = 1.0E+19#
                        Case Is < 1.0E+20#
                            GridSpacing = 1.0E+20#
                        Case Is < 1.0E+21#
                            GridSpacing = 1.0E+21#
                        Case Is < 1.0E+22#
                            GridSpacing = 1.0E+22#
                        Case Is < 1.0E+23#
                            GridSpacing = 1.0E+23#
                        Case Is < 1.0E+24#
                            GridSpacing = 1.0E+24#
                        Case Is < 1.0E+25#
                            GridSpacing = 1.0E+25#
                    End Select
            End Select
            'Give Some Tolorance, Makes Sure Last Grid Line Gets Labeled
            'Grid Spacing Sometimes Causes The Loop To Exit Early
            If XNow + GridSpacing > xMax + (xMax * 0.01) Then XNow = xMax
            LogXNow = CSng(Log(XNow) / Log(XAxisStyle))
            Dim Points1 = New PointF(LogXNow, yStart)
            Dim Points2 = New PointF(LogXNow, yEnd)
            eG.DrawLine(Grid_pen, Points1, Points2)
            If Octaves >= 10 Then XNow = GridSpacing
            XNow += GridSpacing
            MaxLines += 1
            If MaxLines > 250 Then Exit Do
        Loop While XNow < xMax + (xMax * 0.00001) 'Give Tolerance
        Grid_pen.Dispose()
    End Sub
    Private Sub DrawLogY_Grid(ByVal eG As Graphics, ByVal xMin As Single, ByVal yMin As Single, ByVal xMax As Single, ByVal yMax As Single)
        If YAxisStyle = ScaleStyleChoices.Linear Then Exit Sub
        If Graph_Style = GraphStyleChoices.Pie Then Exit Sub
        If Not Show_YGrids Then Exit Sub
        Dim xStart As Single
        Dim xEnd As Single
        If XAxisStyle <> ScaleStyleChoices.Linear Then
            xStart = Log(xMin) / Log(XAxisStyle)
            xEnd = Log(xMax) / Log(XAxisStyle)
        Else
            xStart = xMin
            xEnd = xMax
        End If
        Dim GridSpacing As Single = 0
        Dim LogYNow As Single
        Dim YNow As Single = yMin
        Dim Octaves As Single = Round(((Log(yMax) / Log(10)) - 1) - ((Log(yMin) / Log(10)) - 1), 2)
        Dim MaxLines As Integer = 0
        eG.SmoothingMode = SmoothingMode.HighQuality
        Dim Grid_pen As New Pen(GridLine_Color, 0.001)
        Do
            Select Case Octaves
                Case Is <= 1 'Draw 10 Grid Lines
                    GridSpacing = CSng((yMax - yMin) / 10)
                Case Is < 10
                    Select Case YNow
                        Case Is < 1.0E-24 '<10p
                            GridSpacing = 1.0E-25
                        Case Is < 1.0E-23 '<10p
                            GridSpacing = 1.0E-24
                        Case Is < 1.0E-22 '<10p
                            GridSpacing = 1.0E-23
                        Case Is < 1.0E-21 '<10p
                            GridSpacing = 1.0E-22
                        Case Is < 1.0E-20 '<10p
                            GridSpacing = 1.0E-21
                        Case Is < 1.0E-19 '<10p
                            GridSpacing = 1.0E-20
                        Case Is < 1.0E-18 '<10p
                            GridSpacing = 1.0E-19
                        Case Is < 1.0E-17 '<10p
                            GridSpacing = 1.0E-18
                        Case Is < 0.0000000000000001 '<10p
                            GridSpacing = 1.0E-17
                        Case Is < 0.000000000000001 '<10p
                            GridSpacing = 0.0000000000000001
                        Case Is < 0.00000000000001 '<10p
                            GridSpacing = 0.000000000000001
                        Case Is < 0.0000000000001 '<10p
                            GridSpacing = 0.00000000000001
                        Case Is < 0.000000000001 '<10p
                            GridSpacing = 0.0000000000001
                        Case Is < 0.00000000001 '<10p
                            GridSpacing = 0.000000000001
                        Case Is < 0.0000000001 '<100p
                            GridSpacing = 0.00000000001
                        Case Is < 0.000000001 '<1n
                            GridSpacing = 0.0000000001
                        Case Is < 0.00000001 '<10n
                            GridSpacing = 0.000000001
                        Case Is < 0.0000001 '<100n
                            GridSpacing = 0.00000001
                        Case Is < 0.000001 '<1u
                            GridSpacing = 0.0000001
                        Case Is < 0.00001 '<10u
                            GridSpacing = 0.000001
                        Case Is < 0.0001 '<100u
                            GridSpacing = 0.00001
                        Case Is < 0.001 '<1m
                            GridSpacing = 0.0001
                        Case Is < 0.01 '<10m
                            GridSpacing = 0.001
                        Case Is < 0.1 '<100m
                            GridSpacing = 0.01
                        Case Is < 1
                            GridSpacing = 0.1
                        Case Is < 10
                            GridSpacing = 1
                        Case Is < 100
                            GridSpacing = 10
                        Case Is < 1000
                            GridSpacing = 100
                        Case Is < 10000
                            GridSpacing = 1000
                        Case Is < 100000
                            GridSpacing = 10000
                        Case Is < 1000000
                            GridSpacing = 100000
                        Case Is < 10000000 '10M
                            GridSpacing = 1000000
                        Case Is < 100000000 '100M
                            GridSpacing = 10000000
                        Case Is < 1000000000 '1000M
                            GridSpacing = 100000000
                        Case Is < 10000000000.0# '10G
                            GridSpacing = 1000000000
                        Case Is < 100000000000.0# '100G
                            GridSpacing = 10000000000.0#
                        Case Is < 1000000000000.0# '1000G
                            GridSpacing = 100000000000.0#
                        Case Is < 10000000000000.0#
                            GridSpacing = 1000000000000.0#
                        Case Is < 100000000000000.0#
                            GridSpacing = 10000000000000.0#
                        Case Is < 1.0E+15#
                            GridSpacing = 100000000000000.0#
                        Case Is < 1.0E+16#
                            GridSpacing = 1.0E+15#
                        Case Is < 1.0E+17#
                            GridSpacing = 1.0E+16#
                        Case Is < 1.0E+18#
                            GridSpacing = 1.0E+17#
                        Case Is < 1.0E+19#
                            GridSpacing = 1.0E+18#
                        Case Is < 1.0E+20#
                            GridSpacing = 1.0E+19#
                        Case Is < 1.0E+21#
                            GridSpacing = 1.0E+20#
                        Case Is < 1.0E+22#
                            GridSpacing = 1.0E+21#
                        Case Is < 1.0E+23#
                            GridSpacing = 1.0E+22#
                        Case Is < 1.0E+24#
                            GridSpacing = 1.0E+23#
                        Case Is < 1.0E+25#
                            GridSpacing = 1.0E+24#
                    End Select
                Case Is >= 10
                    Select Case YNow
                        Case Is < 1.0E-24 '<10p
                            GridSpacing = 1.0E-25
                        Case Is < 1.0E-23 '<10p
                            GridSpacing = 1.0E-24
                        Case Is < 1.0E-22 '<10p
                            GridSpacing = 1.0E-23
                        Case Is < 1.0E-21 '<10p
                            GridSpacing = 1.0E-22
                        Case Is < 1.0E-20 '<10p
                            GridSpacing = 1.0E-21
                        Case Is < 1.0E-19 '<10p
                            GridSpacing = 1.0E-20
                        Case Is < 1.0E-18 '<10p
                            GridSpacing = 1.0E-19
                        Case Is < 1.0E-17 '<10p
                            GridSpacing = 1.0E-18
                        Case Is < 0.0000000000000001 '<10p
                            GridSpacing = 1.0E-17
                        Case Is < 0.000000000000001 '<10p
                            GridSpacing = 0.0000000000000001
                        Case Is < 0.00000000000001 '<10p
                            GridSpacing = 0.000000000000001
                        Case Is < 0.0000000000001 '<10p
                            GridSpacing = 0.00000000000001
                        Case Is < 0.000000000001 '<10p
                            GridSpacing = 0.0000000000001
                        Case Is < 0.00000000001 '<10p
                            GridSpacing = 0.000000000001
                        Case Is < 0.0000000001 '<100p
                            GridSpacing = 0.00000000001
                        Case Is < 0.000000001 '<1n
                            GridSpacing = 0.0000000001
                        Case Is < 0.00000001 '<10n
                            GridSpacing = 0.000000001
                        Case Is < 0.0000001 '<100n
                            GridSpacing = 0.00000001
                        Case Is < 0.000001 '<1u
                            GridSpacing = 0.0000001
                        Case Is < 0.00001 '<10u
                            GridSpacing = 0.000001
                        Case Is < 0.0001 '<100u
                            GridSpacing = 0.00001
                        Case Is < 0.001 '<1m
                            GridSpacing = 0.0001
                        Case Is < 0.01 '<10m
                            GridSpacing = 0.001
                        Case Is < 0.1 '<100m
                            GridSpacing = 0.01
                        Case Is < 1
                            GridSpacing = 1
                        Case Is < 10
                            GridSpacing = 10
                        Case Is < 100
                            GridSpacing = 100
                        Case Is < 1000
                            GridSpacing = 1000
                        Case Is < 10000
                            GridSpacing = 10000
                        Case Is < 100000
                            GridSpacing = 100000
                        Case Is < 1000000
                            GridSpacing = 1000000
                        Case Is < 10000000 '10M
                            GridSpacing = 10000000
                        Case Is < 100000000 '100M
                            GridSpacing = 100000000
                        Case Is < 1000000000 '1000M
                            GridSpacing = 1000000000
                        Case Is < 10000000000.0# '10G
                            GridSpacing = 10000000000
                        Case Is < 100000000000.0# '100G
                            GridSpacing = 100000000000.0#
                        Case Is < 1000000000000.0# '1000G
                            GridSpacing = 1000000000000.0#
                        Case Is < 10000000000000.0#
                            GridSpacing = 10000000000000.0#
                        Case Is < 100000000000000.0#
                            GridSpacing = 100000000000000.0#
                        Case Is < 1.0E+15#
                            GridSpacing = 1.0E+15#
                        Case Is < 1.0E+16#
                            GridSpacing = 1.0E+16#
                        Case Is < 1.0E+17#
                            GridSpacing = 1.0E+17#
                        Case Is < 1.0E+18#
                            GridSpacing = 1.0E+18#
                        Case Is < 1.0E+19#
                            GridSpacing = 1.0E+19#
                        Case Is < 1.0E+20#
                            GridSpacing = 1.0E+20#
                        Case Is < 1.0E+21#
                            GridSpacing = 1.0E+21#
                        Case Is < 1.0E+22#
                            GridSpacing = 1.0E+22#
                        Case Is < 1.0E+23#
                            GridSpacing = 1.0E+23#
                        Case Is < 1.0E+24#
                            GridSpacing = 1.0E+24#
                        Case Is < 1.0E+25#
                            GridSpacing = 1.0E+25#
                    End Select
            End Select
            'Give Some Tolorance, Makes Sure Last Grid Line Gets Labeled
            'Grid Spacing Sometimes Causes The Loop To Exit Early
            If YNow + GridSpacing > yMax + (yMax * 0.01) Then YNow = yMax
            'Draw The Log Grid Lines
            LogYNow = Log(YNow) / Log(YAxisStyle)
            Dim Points1 = New PointF(xStart, LogYNow)
            Dim Points2 = New PointF(xEnd, LogYNow)
            eG.DrawLine(Grid_pen, Points1, Points2)
            If Octaves >= 10 Then YNow = GridSpacing
            YNow += GridSpacing
            MaxLines += 1
            If MaxLines > 250 Then Exit Do
        Loop While YNow < yMax + (yMax * 0.00001) 'Give Tolerance
        Grid_pen.Dispose()
    End Sub
    Private Sub DrawLogY_GridLabel(ByVal eG As Graphics, ByVal Height As Single, ByVal ctlTop As Single, ByVal ctlBottom As Single, ByVal yMin As Single, ByVal yMax As Single)
        If YAxisStyle = ScaleStyleChoices.Linear Then Exit Sub
        If Graph_Style = GraphStyleChoices.Pie Then Exit Sub
        If Not Show_YLabels Then Exit Sub
        Dim String_format As New StringFormat
        Dim XPos As Single
        Dim X_Pointy As Single
        If GraphAreaX1 < GraphAreaX2 Then 'Labels On Left Side
            Select Case XAxisStyle
                Case = ScaleStyleChoices.Linear
                    XPos = GetClientPoint(0, ClientSize.Width, ControlLeft, ControlRight, GraphAreaX1)
                Case Else 'Log(x) On Left
                    XPos = GetClientPoint(0, ClientSize.Width, ControlLeft, ControlRight, Log(GraphAreaX1) / Log(XAxisStyle))
            End Select
            Select Case Rotate_YGridLabels'Labels On Left Side
                Case = 90
                    X_Pointy = XPos - (XPos * 0.1)
                    String_format.Alignment = StringAlignment.Center
                    String_format.LineAlignment = StringAlignment.Center
                    String_format.FormatFlags = StringFormatFlags.NoWrap
                Case >= 45
                    X_Pointy = XPos - (XPos * 0.08)
                    String_format.Alignment = StringAlignment.Far
                    String_format.LineAlignment = StringAlignment.Center
                    String_format.FormatFlags = StringFormatFlags.NoWrap
                Case > 0
                    X_Pointy = XPos - (XPos * 0.04)
                    String_format.Alignment = StringAlignment.Far
                    String_format.LineAlignment = StringAlignment.Center
                    String_format.FormatFlags = StringFormatFlags.NoWrap
                Case = 0
                    X_Pointy = XPos - (XPos * 0.03)
                    String_format.Alignment = StringAlignment.Far
                    String_format.LineAlignment = StringAlignment.Center
                    String_format.FormatFlags = StringFormatFlags.NoWrap
                Case > -45
                    X_Pointy = XPos - (XPos * 0.05)
                    String_format.Alignment = StringAlignment.Far
                    String_format.LineAlignment = StringAlignment.Center
                    String_format.FormatFlags = StringFormatFlags.NoWrap
                Case > -90
                    X_Pointy = XPos - (XPos * 0.07)
                    String_format.Alignment = StringAlignment.Far
                    String_format.LineAlignment = StringAlignment.Center
                    String_format.FormatFlags = StringFormatFlags.NoWrap
                Case = -90
                    X_Pointy = XPos - (XPos * 0.1)
                    String_format.Alignment = StringAlignment.Center
                    String_format.LineAlignment = StringAlignment.Center
                    String_format.FormatFlags = StringFormatFlags.NoWrap
            End Select
        Else 'Labels On Right Side, GraphAreaX1 > GraphAreaX2
            Select Case XAxisStyle
                Case = ScaleStyleChoices.Linear
                    XPos = GetClientPoint(0, ClientSize.Width, ControlLeft, ControlRight, GraphAreaX2)
                Case Else 'Log(x) On Right
                    XPos = GetClientPoint(0, ClientSize.Width, ControlLeft, ControlRight, Log(GraphAreaX2) / Log(XAxisStyle))
            End Select
            Select Case Rotate_YGridLabels'Labels On Right Side
                Case = 90
                    X_Pointy = XPos + (XPos * 0.01)
                    String_format.Alignment = StringAlignment.Center
                    String_format.LineAlignment = StringAlignment.Center
                    String_format.FormatFlags = StringFormatFlags.DirectionRightToLeft
                Case >= 45
                    X_Pointy = XPos + (XPos * 0.007)
                    String_format.Alignment = StringAlignment.Far
                    String_format.LineAlignment = StringAlignment.Center
                    String_format.FormatFlags = StringFormatFlags.DirectionRightToLeft
                Case > 0
                    X_Pointy = XPos + (XPos * 0.005)
                    String_format.Alignment = StringAlignment.Far
                    String_format.LineAlignment = StringAlignment.Center
                    String_format.FormatFlags = StringFormatFlags.DirectionRightToLeft
                Case = 0
                    X_Pointy = XPos + (XPos * 0.005)
                    String_format.Alignment = StringAlignment.Far
                    String_format.LineAlignment = StringAlignment.Center
                    String_format.FormatFlags = StringFormatFlags.DirectionRightToLeft
                Case > -45
                    X_Pointy = XPos + (XPos * 0.005)
                    String_format.Alignment = StringAlignment.Far
                    String_format.LineAlignment = StringAlignment.Center
                    String_format.FormatFlags = StringFormatFlags.DirectionRightToLeft
                Case > -90
                    X_Pointy = XPos + (XPos * 0.008)
                    String_format.Alignment = StringAlignment.Far
                    String_format.LineAlignment = StringAlignment.Center
                    String_format.FormatFlags = StringFormatFlags.DirectionRightToLeft
                Case = -90
                    X_Pointy = XPos + (XPos * 0.012)
                    String_format.Alignment = StringAlignment.Center
                    String_format.LineAlignment = StringAlignment.Center
                    String_format.FormatFlags = StringFormatFlags.DirectionRightToLeft
            End Select
        End If
        Dim Octaves As Single = Round(((Log(yMax) / Log(10)) - 1) - ((Log(yMin) / Log(10)) - 1), 2)
        Dim YNow As Single = yMin
        Dim YPos As Single 'Log Version
        Dim GridSpacing As Single
        Dim myBrush = New SolidBrush(GridLabel_Color)
        Dim N As Integer = 0
        Dim YMaxReached As Boolean = False
        eG.SmoothingMode = SmoothingMode.HighQuality
        Do
            Select Case Octaves
                Case Is <= 1 'Draw 10 Grid Lines
                    GridSpacing = CSng((yMax - yMin) / 10)
                Case Is < 10
                    Select Case YNow
                        Case Is < 1.0E-24 '<10p
                            GridSpacing = 1.0E-25
                        Case Is < 1.0E-23 '<10p
                            GridSpacing = 1.0E-24
                        Case Is < 1.0E-22 '<10p
                            GridSpacing = 1.0E-23
                        Case Is < 1.0E-21 '<10p
                            GridSpacing = 1.0E-22
                        Case Is < 1.0E-20 '<10p
                            GridSpacing = 1.0E-21
                        Case Is < 1.0E-19 '<10p
                            GridSpacing = 1.0E-20
                        Case Is < 1.0E-18 '<10p
                            GridSpacing = 1.0E-19
                        Case Is < 1.0E-17 '<10p
                            GridSpacing = 1.0E-18
                        Case Is < 0.0000000000000001 '<10p
                            GridSpacing = 1.0E-17
                        Case Is < 0.000000000000001 '<10p
                            GridSpacing = 0.0000000000000001
                        Case Is < 0.00000000000001 '<10p
                            GridSpacing = 0.000000000000001
                        Case Is < 0.0000000000001 '<10p
                            GridSpacing = 0.00000000000001
                        Case Is < 0.000000000001 '<10p
                            GridSpacing = 0.0000000000001
                        Case Is < 0.00000000001 '<10p
                            GridSpacing = 0.000000000001
                        Case Is < 0.0000000001 '<100p
                            GridSpacing = 0.00000000001
                        Case Is < 0.000000001 '<1n
                            GridSpacing = 0.0000000001
                        Case Is < 0.00000001 '<10n
                            GridSpacing = 0.000000001
                        Case Is < 0.0000001 '<100n
                            GridSpacing = 0.00000001
                        Case Is < 0.000001 '<1u
                            GridSpacing = 0.0000001
                        Case Is < 0.00001 '<10u
                            GridSpacing = 0.000001
                        Case Is < 0.0001 '<100u
                            GridSpacing = 0.00001
                        Case Is < 0.001 '<1m
                            GridSpacing = 0.0001
                        Case Is < 0.01 '<10m
                            GridSpacing = 0.001
                        Case Is < 0.1 '<100m
                            GridSpacing = 0.01
                        Case Is < 1
                            GridSpacing = 0.1
                        Case Is < 10
                            GridSpacing = 1
                        Case Is < 100
                            GridSpacing = 10
                        Case Is < 1000
                            GridSpacing = 100
                        Case Is < 10000
                            GridSpacing = 1000
                        Case Is < 100000
                            GridSpacing = 10000
                        Case Is < 1000000
                            GridSpacing = 100000
                        Case Is < 10000000 '10M
                            GridSpacing = 1000000
                        Case Is < 100000000 '100M
                            GridSpacing = 10000000
                        Case Is < 1000000000 '1000M
                            GridSpacing = 100000000
                        Case Is < 10000000000.0# '10G
                            GridSpacing = 1000000000
                        Case Is < 100000000000.0# '100G
                            GridSpacing = 10000000000.0#
                        Case Is < 1000000000000.0# '1000G
                            GridSpacing = 100000000000.0#
                        Case Is < 10000000000000.0#
                            GridSpacing = 1000000000000.0#
                        Case Is < 100000000000000.0#
                            GridSpacing = 10000000000000.0#
                        Case Is < 1.0E+15#
                            GridSpacing = 100000000000000.0#
                        Case Is < 1.0E+16#
                            GridSpacing = 1.0E+15#
                        Case Is < 1.0E+17#
                            GridSpacing = 1.0E+16#
                        Case Is < 1.0E+18#
                            GridSpacing = 1.0E+17#
                        Case Is < 1.0E+19#
                            GridSpacing = 1.0E+18#
                        Case Is < 1.0E+20#
                            GridSpacing = 1.0E+19#
                        Case Is < 1.0E+21#
                            GridSpacing = 1.0E+20#
                        Case Is < 1.0E+22#
                            GridSpacing = 1.0E+21#
                        Case Is < 1.0E+23#
                            GridSpacing = 1.0E+22#
                        Case Is < 1.0E+24#
                            GridSpacing = 1.0E+23#
                        Case Is < 1.0E+25#
                            GridSpacing = 1.0E+24#
                    End Select
                Case Is >= 10
                    Select Case YNow
                        Case Is < 1.0E-24 '<10p
                            GridSpacing = 1.0E-25
                        Case Is < 1.0E-23 '<10p
                            GridSpacing = 1.0E-24
                        Case Is < 1.0E-22 '<10p
                            GridSpacing = 1.0E-23
                        Case Is < 1.0E-21 '<10p
                            GridSpacing = 1.0E-22
                        Case Is < 1.0E-20 '<10p
                            GridSpacing = 1.0E-21
                        Case Is < 1.0E-19 '<10p
                            GridSpacing = 1.0E-20
                        Case Is < 1.0E-18 '<10p
                            GridSpacing = 1.0E-19
                        Case Is < 1.0E-17 '<10p
                            GridSpacing = 1.0E-18
                        Case Is < 0.0000000000000001 '<10p
                            GridSpacing = 1.0E-17
                        Case Is < 0.000000000000001 '<10p
                            GridSpacing = 0.0000000000000001
                        Case Is < 0.00000000000001 '<10p
                            GridSpacing = 0.000000000000001
                        Case Is < 0.0000000000001 '<10p
                            GridSpacing = 0.00000000000001
                        Case Is < 0.000000000001 '<10p
                            GridSpacing = 0.0000000000001
                        Case Is < 0.00000000001 '<10p
                            GridSpacing = 0.000000000001
                        Case Is < 0.0000000001 '<100p
                            GridSpacing = 0.00000000001
                        Case Is < 0.000000001 '<1n
                            GridSpacing = 0.0000000001
                        Case Is < 0.00000001 '<10n
                            GridSpacing = 0.000000001
                        Case Is < 0.0000001 '<100n
                            GridSpacing = 0.00000001
                        Case Is < 0.000001 '<1u
                            GridSpacing = 0.0000001
                        Case Is < 0.00001 '<10u
                            GridSpacing = 0.000001
                        Case Is < 0.0001 '<100u
                            GridSpacing = 0.00001
                        Case Is < 0.001 '<1m
                            GridSpacing = 0.0001
                        Case Is < 0.01 '<10m
                            GridSpacing = 0.001
                        Case Is < 0.1 '<100m
                            GridSpacing = 0.01
                        Case Is < 1
                            GridSpacing = 1
                        Case Is < 10
                            GridSpacing = 10
                        Case Is < 100
                            GridSpacing = 100
                        Case Is < 1000
                            GridSpacing = 1000
                        Case Is < 10000
                            GridSpacing = 10000
                        Case Is < 100000
                            GridSpacing = 100000
                        Case Is < 1000000
                            GridSpacing = 1000000
                        Case Is < 10000000 '10M
                            GridSpacing = 10000000
                        Case Is < 100000000 '100M
                            GridSpacing = 100000000
                        Case Is < 1000000000 '1000M
                            GridSpacing = 1000000000
                        Case Is < 10000000000.0# '10G
                            GridSpacing = 10000000000
                        Case Is < 100000000000.0# '100G
                            GridSpacing = 100000000000.0#
                        Case Is < 1000000000000.0# '1000G
                            GridSpacing = 1000000000000.0#
                        Case Is < 10000000000000.0#
                            GridSpacing = 10000000000000.0#
                        Case Is < 100000000000000.0#
                            GridSpacing = 100000000000000.0#
                        Case Is < 1.0E+15#
                            GridSpacing = 1.0E+15#
                        Case Is < 1.0E+16#
                            GridSpacing = 1.0E+16#
                        Case Is < 1.0E+17#
                            GridSpacing = 1.0E+17#
                        Case Is < 1.0E+18#
                            GridSpacing = 1.0E+18#
                        Case Is < 1.0E+19#
                            GridSpacing = 1.0E+19#
                        Case Is < 1.0E+20#
                            GridSpacing = 1.0E+20#
                        Case Is < 1.0E+21#
                            GridSpacing = 1.0E+21#
                        Case Is < 1.0E+22#
                            GridSpacing = 1.0E+22#
                        Case Is < 1.0E+23#
                            GridSpacing = 1.0E+23#
                        Case Is < 1.0E+24#
                            GridSpacing = 1.0E+24#
                        Case Is < 1.0E+25#
                            GridSpacing = 1.0E+25#
                    End Select
            End Select
            'Give Some Tolorance, Makes Sure Last Grid Line Gets Labeled
            'Grid Spacing Sometimes Causes The Loop To Exit Early
            If YNow + GridSpacing > yMax + (yMax * 0.01) Then
                YNow = yMax
                YMaxReached = True
            End If
            Dim Count As Integer = 0
            Dim YMinFirstChar As String
            'XMinFirstChar Along With Octaves Determines The GridLabeling Template 
            Do
                Count += 1
                YMinFirstChar = Mid(yMin, Count, 1)
                If YMinFirstChar = "." Then YMinFirstChar = ""
                If YMinFirstChar = "-" Then YMinFirstChar = ""
                If YMinFirstChar = "0" Then YMinFirstChar = ""
            Loop While YMinFirstChar = "" Or Count > 24
            'Translate The Custom Scale Point To The Original Control Scale Point
            YPos = CSng(Log(YNow) / Log(YAxisStyle))
            Dim Y_Pointy As Single = GetClientPoint(0, ClientSize.Height, ctlTop, ctlBottom, YPos)
            Dim Points = New PointF(X_Pointy, Y_Pointy)
            eG.ScaleTransform(1, 1) 'Increases Or Decreases The Size Of The Coordinate System
            eG.TranslateTransform(X_Pointy, Y_Pointy, MatrixOrder.Append)
            eG.RotateTransform(Rotate_YGridLabels)
            Dim LblStr As String = ReturnTruncated(YNow)
            Select Case Octaves
                Case Is <= 1
                    Select Case N
                        Case 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10
                            eG.DrawString(LblStr, Grid_Font, myBrush, 0, 0, String_format)
                            If YNow = yMax And YMaxReached Then
                                eG.DrawString(LblStr, Grid_Font, myBrush, 0, 0, String_format)
                            End If
                    End Select
                Case Is <= 2
                    Select Case YMinFirstChar
                        Case 1
                            Select Case N
                                Case 0, 1, 2, 4, 6, 9, 10, 11, 13, 15, 18 'ok
                                    eG.DrawString(LblStr, Grid_Font, myBrush, 0, 0, String_format)
                                Case Else
                                    If YNow = yMax And YMaxReached Then
                                        eG.DrawString(LblStr, Grid_Font, myBrush, 0, 0, String_format)
                                    End If
                            End Select
                        Case 2
                            Select Case N
                                Case 0, 2, 4, 6, 8, 9, 11, 13, 15, 17, 18 'ok
                                    eG.DrawString(LblStr, Grid_Font, myBrush, 0, 0, String_format)
                                Case Else
                                    If YNow = yMax And YMaxReached Then
                                        eG.DrawString(LblStr, Grid_Font, myBrush, 0, 0, String_format)
                                    End If
                            End Select
                        Case 3
                            Select Case N
                                Case 0, 2, 4, 7, 8, 9, 11, 13, 16, 17, 18 'ok
                                    eG.DrawString(LblStr, Grid_Font, myBrush, 0, 0, String_format)
                                Case Else
                                    If YNow = yMax And YMaxReached Then
                                        eG.DrawString(LblStr, Grid_Font, myBrush, 0, 0, String_format)
                                    End If
                            End Select
                        Case 4
                            Select Case N
                                Case 0, 2, 4, 6, 7, 8, 9, 11, 13, 15, 16, 18 'ok
                                    eG.DrawString(LblStr, Grid_Font, myBrush, 0, 0, String_format)
                                Case Else
                                    If YNow = yMax And YMaxReached Then
                                        eG.DrawString(LblStr, Grid_Font, myBrush, 0, 0, String_format)
                                    End If
                            End Select
                        Case 5
                            Select Case N
                                Case 0, 2, 5, 6, 7, 9, 11, 14, 15, 16, 18 '0K
                                    eG.DrawString(LblStr, Grid_Font, myBrush, 0, 0, String_format)
                                Case Else
                                    If YNow = yMax And YMaxReached Then
                                        eG.DrawString(LblStr, Grid_Font, myBrush, 0, 0, String_format)
                                    End If
                            End Select
                        Case 6
                            Select Case N
                                Case 0, 2, 4, 5, 7, 9, 13, 14, 16, 18 'ok
                                    eG.DrawString(LblStr, Grid_Font, myBrush, 0, 0, String_format)
                                Case Else
                                    If YNow = yMax And YMaxReached Then
                                        eG.DrawString(LblStr, Grid_Font, myBrush, 0, 0, String_format)
                                    End If
                            End Select
                        Case 7
                            Select Case N
                                Case 0, 3, 4, 5, 7, 9, 12, 13, 14, 16, 18 'ok
                                    eG.DrawString(LblStr, Grid_Font, myBrush, 0, 0, String_format)
                                Case Else
                                    If YNow = yMax And YMaxReached Then
                                        eG.DrawString(LblStr, Grid_Font, myBrush, 0, 0, String_format)
                                    End If
                            End Select
                        Case 8
                            Select Case N
                                Case 0, 2, 3, 4, 5, 7, 9, 11, 12, 13, 14, 16, 18 'ok
                                    eG.DrawString(LblStr, Grid_Font, myBrush, 0, 0, String_format)
                                Case Else
                                    If YNow = yMax And YMaxReached Then
                                        eG.DrawString(LblStr, Grid_Font, myBrush, 0, 0, String_format)
                                    End If
                            End Select
                        Case 9
                            Select Case N
                                Case 0, 2, 3, 4, 6, 8, 10, 11, 12, 13, 15, 18 'ok
                                    eG.DrawString(LblStr, Grid_Font, myBrush, 0, 0, String_format)
                                Case Else
                                    If YNow = yMax And YMaxReached Then
                                        eG.DrawString(LblStr, Grid_Font, myBrush, 0, 0, String_format)
                                    End If
                            End Select
                    End Select
                Case Is <= 3
                    Select Case YMinFirstChar
                        Case 1
                            Select Case N
                                Case 0, 1, 2, 3, 5, 9, 10, 11, 12, 14, 18, 19, 20, 21, 23, 27 '0k
                                    eG.DrawString(LblStr, Grid_Font, myBrush, 0, 0, String_format)
                                Case Else
                                    If YNow = yMax And YMaxReached Then
                                        eG.DrawString(LblStr, Grid_Font, myBrush, 0, 0, String_format)
                                    End If
                            End Select
                        Case 2
                            Select Case N
                                Case 0, 1, 2, 4, 8, 9, 10, 11, 13, 17, 18, 19, 20, 22, 26, 27 'ok
                                    eG.DrawString(LblStr, Grid_Font, myBrush, 0, 0, String_format)
                                Case Else
                                    If YNow = yMax And YMaxReached Then
                                        eG.DrawString(LblStr, Grid_Font, myBrush, 0, 0, String_format)
                                    End If
                            End Select
                        Case 3
                            Select Case N
                                Case 0, 1, 3, 7, 8, 9, 10, 12, 16, 17, 18, 19, 21, 25, 27 'ok
                                    eG.DrawString(LblStr, Grid_Font, myBrush, 0, 0, String_format)
                                Case Else
                                    If YNow = yMax And YMaxReached Then
                                        eG.DrawString(LblStr, Grid_Font, myBrush, 0, 0, String_format)
                                    End If
                            End Select
                        Case 4
                            Select Case N
                                Case 0, 2, 6, 7, 8, 9, 11, 15, 16, 17, 18, 20, 24, 25, 27 'ok
                                    eG.DrawString(LblStr, Grid_Font, myBrush, 0, 0, String_format)
                                Case Else
                                    If YNow = yMax And YMaxReached Then
                                        eG.DrawString(LblStr, Grid_Font, myBrush, 0, 0, String_format)
                                    End If
                            End Select
                        Case 5
                            Select Case N
                                Case 0, 2, 5, 6, 7, 9, 11, 14, 15, 16, 18, 20, 23, 24, 27 'ok
                                    eG.DrawString(LblStr, Grid_Font, myBrush, 0, 0, String_format)
                                Case Else
                                    If YNow = yMax And YMaxReached Then
                                        eG.DrawString(LblStr, Grid_Font, myBrush, 0, 0, String_format)
                                    End If
                            End Select
                        Case 6
                            Select Case N
                                Case 0, 4, 5, 6, 7, 9, 13, 14, 15, 16, 18, 22, 23, 24, 27 'ok
                                    eG.DrawString(LblStr, Grid_Font, myBrush, 0, 0, String_format)
                                Case Else
                                    If YNow = yMax And YMaxReached Then
                                        eG.DrawString(LblStr, Grid_Font, myBrush, 0, 0, String_format)
                                    End If
                            End Select
                        Case 7
                            Select Case N
                                Case 0, 3, 4, 5, 7, 9, 12, 13, 14, 16, 18, 21, 22, 23, 27 'ok
                                    eG.DrawString(LblStr, Grid_Font, myBrush, 0, 0, String_format)
                                Case Else
                                    If YNow = yMax And YMaxReached Then
                                        eG.DrawString(LblStr, Grid_Font, myBrush, 0, 0, String_format)
                                    End If
                            End Select
                        Case 8
                            Select Case N
                                Case 0, 3, 4, 6, 8, 11, 12, 13, 15, 17, 20, 21, 22, 24, 27 'ok
                                    eG.DrawString(LblStr, Grid_Font, myBrush, 0, 0, String_format)
                                Case Else
                                    If YNow = yMax And YMaxReached Then
                                        eG.DrawString(LblStr, Grid_Font, myBrush, 0, 0, String_format)
                                    End If
                            End Select
                        Case 9
                            Select Case N
                                Case 0, 2, 3, 5, 7, 10, 11, 12, 14, 16, 19, 20, 21, 23, 27 'ok
                                    eG.DrawString(LblStr, Grid_Font, myBrush, 0, 0, String_format)
                                Case Else
                                    If YNow = yMax And YMaxReached Then
                                        eG.DrawString(LblStr, Grid_Font, myBrush, 0, 0, String_format)
                                    End If
                            End Select
                    End Select
                Case Is <= 4
                    Select Case YMinFirstChar
                        Case 1
                            Select Case N
                                Case 0, 1, 3, 9, 10, 12, 18, 19, 21, 27, 28, 30, 36 'ok
                                    eG.DrawString(LblStr, Grid_Font, myBrush, 0, 0, String_format)
                                Case Else
                                    If YNow = yMax And YMaxReached Then
                                        eG.DrawString(LblStr, Grid_Font, myBrush, 0, 0, String_format)
                                    End If
                            End Select
                        Case 2
                            Select Case N
                                Case 0, 2, 8, 9, 11, 17, 18, 20, 26, 27, 29, 35, 36 'ok
                                    eG.DrawString(LblStr, Grid_Font, myBrush, 0, 0, String_format)
                                Case Else
                                    If YNow = yMax And YMaxReached Then
                                        eG.DrawString(LblStr, Grid_Font, myBrush, 0, 0, String_format)
                                    End If
                            End Select
                        Case 3
                            Select Case N
                                Case 0, 2, 7, 8, 11, 16, 17, 20, 25, 26, 29, 34, 36 'ok
                                    eG.DrawString(LblStr, Grid_Font, myBrush, 0, 0, String_format)
                                Case Else
                                    If YNow = yMax And YMaxReached Then
                                        eG.DrawString(LblStr, Grid_Font, myBrush, 0, 0, String_format)
                                    End If
                            End Select
                        Case 4
                            Select Case N
                                Case 0, 2, 6, 7, 10, 15, 16, 19, 24, 25, 28, 33, 34, 36 'ok
                                    eG.DrawString(LblStr, Grid_Font, myBrush, 0, 0, String_format)
                                Case Else
                                    If YNow = yMax And YMaxReached Then
                                        eG.DrawString(LblStr, Grid_Font, myBrush, 0, 0, String_format)
                                    End If
                            End Select
                        Case 5
                            Select Case N
                                Case 0, 5, 6, 9, 14, 15, 18, 23, 24, 27, 32, 33, 36 'ok
                                    eG.DrawString(LblStr, Grid_Font, myBrush, 0, 0, String_format)
                                Case Else
                                    If YNow = yMax And YMaxReached Then
                                        eG.DrawString(LblStr, Grid_Font, myBrush, 0, 0, String_format)
                                    End If
                            End Select
                        Case 6
                            Select Case N
                                Case 0, 4, 5, 8, 13, 14, 17, 22, 23, 26, 31, 32, 36 'ok
                                    eG.DrawString(LblStr, Grid_Font, myBrush, 0, 0, String_format)
                                Case Else
                                    If YNow = yMax And YMaxReached Then
                                        eG.DrawString(LblStr, Grid_Font, myBrush, 0, 0, String_format)
                                    End If
                            End Select
                        Case 7
                            Select Case N
                                Case 0, 4, 7, 12, 13, 16, 21, 22, 25, 30, 31, 36 'ok
                                    eG.DrawString(LblStr, Grid_Font, myBrush, 0, 0, String_format)
                                Case Else
                                    If YNow = yMax And YMaxReached Then
                                        eG.DrawString(LblStr, Grid_Font, myBrush, 0, 0, String_format)
                                    End If
                            End Select
                        Case 8
                            Select Case N
                                Case 0, 3, 6, 11, 12, 15, 20, 21, 24, 29, 30, 36 'ok
                                    eG.DrawString(LblStr, Grid_Font, myBrush, 0, 0, String_format)
                                Case Else
                                    If YNow = yMax And YMaxReached Then
                                        eG.DrawString(LblStr, Grid_Font, myBrush, 0, 0, String_format)
                                    End If
                            End Select
                        Case 9
                            Select Case N
                                Case 0, 2, 5, 10, 11, 14, 19, 20, 23, 28, 29, 36 'ok
                                    eG.DrawString(LblStr, Grid_Font, myBrush, 0, 0, String_format)
                                Case Else
                                    If YNow = yMax And YMaxReached Then
                                        eG.DrawString(LblStr, Grid_Font, myBrush, 0, 0, String_format)
                                    End If
                            End Select
                    End Select
                Case Is <= 5
                    Select Case YMinFirstChar
                        Case 1
                            Select Case N
                                Case 0, 1, 3, 9, 10, 12, 18, 19, 21, 27, 28, 30, 36, 37, 39 'ok
                                    eG.DrawString(LblStr, Grid_Font, myBrush, 0, 0, String_format)
                                Case Else
                                    If YNow = yMax And YMaxReached Then
                                        eG.DrawString(LblStr, Grid_Font, myBrush, 0, 0, String_format)
                                    End If
                            End Select
                        Case 2
                            Select Case N
                                Case 0, 2, 8, 9, 11, 17, 18, 20, 26, 27, 29, 35, 36, 38, 45 'ok
                                    eG.DrawString(LblStr, Grid_Font, myBrush, 0, 0, String_format)
                                Case Else
                                    If YNow = yMax And YMaxReached Then
                                        eG.DrawString(LblStr, Grid_Font, myBrush, 0, 0, String_format)
                                    End If
                            End Select
                        Case 3
                            Select Case N
                                Case 0, 2, 7, 8, 11, 16, 17, 20, 25, 26, 29, 34, 35, 38, 43, 45 'ok
                                    eG.DrawString(LblStr, Grid_Font, myBrush, 0, 0, String_format)
                                Case Else
                                    If YNow = yMax And YMaxReached Then
                                        eG.DrawString(LblStr, Grid_Font, myBrush, 0, 0, String_format)
                                    End If
                            End Select
                        Case 4
                            Select Case N
                                Case 0, 6, 7, 10, 15, 16, 19, 24, 25, 28, 33, 34, 37, 42, 45 'ok
                                    eG.DrawString(LblStr, Grid_Font, myBrush, 0, 0, String_format)
                                Case Else
                                    If YNow = yMax And YMaxReached Then
                                        eG.DrawString(LblStr, Grid_Font, myBrush, 0, 0, String_format)
                                    End If
                            End Select
                        Case 5
                            Select Case N
                                Case 0, 5, 6, 9, 14, 15, 18, 23, 24, 27, 32, 33, 36, 41, 42, 45 'ok
                                    eG.DrawString(LblStr, Grid_Font, myBrush, 0, 0, String_format)
                                Case Else
                                    If YNow = yMax And YMaxReached Then
                                        eG.DrawString(LblStr, Grid_Font, myBrush, 0, 0, String_format)
                                    End If
                            End Select
                        Case 6
                            Select Case N
                                Case 0, 4, 5, 8, 13, 14, 17, 22, 23, 26, 31, 32, 35, 40, 41, 41, 45 'ok
                                    eG.DrawString(LblStr, Grid_Font, myBrush, 0, 0, String_format)
                                Case Else
                                    If YNow = yMax And YMaxReached Then
                                        eG.DrawString(LblStr, Grid_Font, myBrush, 0, 0, String_format)
                                    End If
                            End Select
                        Case 7
                            Select Case N
                                Case 0, 4, 7, 12, 13, 16, 21, 22, 25, 30, 31, 34, 39, 40, 45 'ok
                                    eG.DrawString(LblStr, Grid_Font, myBrush, 0, 0, String_format)
                                Case Else
                                    If YNow = yMax And YMaxReached Then
                                        eG.DrawString(LblStr, Grid_Font, myBrush, 0, 0, String_format)
                                    End If
                            End Select
                        Case 8
                            Select Case N
                                Case 0, 3, 6, 11, 12, 15, 20, 21, 24, 29, 30, 33, 38, 39, 45 'ok
                                    eG.DrawString(LblStr, Grid_Font, myBrush, 0, 0, String_format)
                                Case Else
                                    If YNow = yMax And YMaxReached Then
                                        eG.DrawString(LblStr, Grid_Font, myBrush, 0, 0, String_format)
                                    End If
                            End Select
                        Case 9
                            Select Case N
                                Case 0, 2, 5, 10, 11, 14, 19, 20, 23, 29, 32, 37, 38, 45 'ok
                                    eG.DrawString(LblStr, Grid_Font, myBrush, 0, 0, String_format)
                                Case Else
                                    If YNow = yMax And YMaxReached Then
                                        eG.DrawString(LblStr, Grid_Font, myBrush, 0, 0, String_format)
                                    End If
                            End Select
                    End Select
                Case Is <= 6
                    Select Case YMinFirstChar
                        Case 1
                            Select Case N
                                Case 0, 1, 3, 9, 10, 12, 18, 19, 21, 27, 28, 30, 36, 37, 39, 45, 46, 48, 54 'ok
                                    eG.DrawString(LblStr, Grid_Font, myBrush, 0, 0, String_format)
                                Case Else
                                    If YNow = yMax And YMaxReached Then
                                        eG.DrawString(LblStr, Grid_Font, myBrush, 0, 0, String_format)
                                    End If
                            End Select
                        Case 2
                            Select Case N
                                Case 0, 2, 8, 9, 11, 17, 18, 20, 26, 27, 29, 35, 36, 38, 44, 45, 47, 53, 54 'ok
                                    eG.DrawString(LblStr, Grid_Font, myBrush, 0, 0, String_format)
                                Case Else
                                    If YNow = yMax And YMaxReached Then
                                        eG.DrawString(LblStr, Grid_Font, myBrush, 0, 0, String_format)
                                    End If
                            End Select
                        Case 3
                            Select Case N
                                Case 0, 2, 7, 8, 11, 16, 17, 20, 25, 26, 29, 34, 35, 38, 43, 44, 47, 52, 54 'ok
                                    eG.DrawString(LblStr, Grid_Font, myBrush, 0, 0, String_format)
                                Case Else
                                    If YNow = yMax And YMaxReached Then
                                        eG.DrawString(LblStr, Grid_Font, myBrush, 0, 0, String_format)
                                    End If
                            End Select
                        Case 4
                            Select Case N
                                Case 0, 6, 7, 10, 15, 16, 19, 24, 25, 28, 33, 34, 37, 42, 43, 46, 51 '0k
                                    eG.DrawString(LblStr, Grid_Font, myBrush, 0, 0, String_format)
                                Case Else
                                    If YNow = yMax And YMaxReached Then
                                        eG.DrawString(LblStr, Grid_Font, myBrush, 0, 0, String_format)
                                    End If
                            End Select
                        Case 5
                            Select Case N
                                Case 0, 5, 6, 9, 14, 15, 18, 23, 24, 27, 32, 33, 36, 41, 42, 45, 50, 51, 54 'ok
                                    eG.DrawString(LblStr, Grid_Font, myBrush, 0, 0, String_format)
                                Case Else
                                    If YNow = yMax And YMaxReached Then
                                        eG.DrawString(LblStr, Grid_Font, myBrush, 0, 0, String_format)
                                    End If
                            End Select
                        Case 6
                            Select Case N
                                Case 0, 4, 5, 8, 13, 14, 17, 22, 23, 26, 31, 32, 35, 40, 41, 44, 49, 50, 54 'ok
                                    eG.DrawString(LblStr, Grid_Font, myBrush, 0, 0, String_format)
                                Case Else
                                    If YNow = yMax And YMaxReached Then
                                        eG.DrawString(LblStr, Grid_Font, myBrush, 0, 0, String_format)
                                    End If
                            End Select
                        Case 7
                            Select Case N
                                Case 0, 4, 7, 12, 13, 16, 21, 22, 25, 30, 31, 34, 39, 40, 43, 48, 49, 54 'ok
                                    eG.DrawString(LblStr, Grid_Font, myBrush, 0, 0, String_format)
                                Case Else
                                    If YNow = yMax And YMaxReached Then
                                        eG.DrawString(LblStr, Grid_Font, myBrush, 0, 0, String_format)
                                    End If
                            End Select
                        Case 8
                            Select Case N
                                Case 0, 3, 6, 11, 12, 15, 20, 21, 24, 29, 30, 33, 38, 39, 42, 47, 48, 54 'ok
                                    eG.DrawString(LblStr, Grid_Font, myBrush, 0, 0, String_format)
                                Case Else
                                    If YNow = yMax And YMaxReached Then
                                        eG.DrawString(LblStr, Grid_Font, myBrush, 0, 0, String_format)
                                    End If
                            End Select
                        Case 9
                            Select Case N
                                Case 0, 2, 5, 10, 11, 14, 19, 20, 23, 29, 32, 37, 38, 41, 46, 47, 54 'ok
                                    eG.DrawString(LblStr, Grid_Font, myBrush, 0, 0, String_format)
                                Case Else
                                    If YNow = yMax And YMaxReached Then
                                        eG.DrawString(LblStr, Grid_Font, myBrush, 0, 0, String_format)
                                    End If
                            End Select
                    End Select
                Case Is <= 7
                    Select Case YMinFirstChar
                        Case 1
                            Select Case N
                                Case 0, 9, 18, 27, 36, 45, 54, 63 'OK
                                    eG.DrawString(LblStr, Grid_Font, myBrush, 0, 0, String_format)
                                Case Else
                                    If YNow = yMax And YMaxReached Then
                                        eG.DrawString(LblStr, Grid_Font, myBrush, 0, 0, String_format)
                                    End If
                            End Select
                        Case 2
                            Select Case N
                                Case 0, 9, 18, 27, 36, 45, 54, 63 'ok
                                    eG.DrawString(LblStr, Grid_Font, myBrush, 0, 0, String_format)
                                Case Else
                                    If YNow = yMax And YMaxReached Then
                                        eG.DrawString(LblStr, Grid_Font, myBrush, 0, 0, String_format)
                                    End If
                            End Select
                        Case 3
                            Select Case N
                                Case 0, 8, 17, 26, 35, 44, 53, 63 'ok
                                    eG.DrawString(LblStr, Grid_Font, myBrush, 0, 0, String_format)
                                Case Else
                                    If YNow = yMax And YMaxReached Then
                                        eG.DrawString(LblStr, Grid_Font, myBrush, 0, 0, String_format)
                                    End If
                            End Select
                        Case 4
                            Select Case N
                                Case 0, 7, 16, 25, 34, 43, 52, 63 'ok
                                    eG.DrawString(LblStr, Grid_Font, myBrush, 0, 0, String_format)
                                Case Else
                                    If YNow = yMax And YMaxReached Then
                                        eG.DrawString(LblStr, Grid_Font, myBrush, 0, 0, String_format)
                                    End If
                            End Select
                        Case 5
                            Select Case N
                                Case 0, 14, 23, 32, 41, 50, 59, 63 'ok
                                    eG.DrawString(LblStr, Grid_Font, myBrush, 0, 0, String_format)
                                Case Else
                                    If YNow = yMax And YMaxReached Then
                                        eG.DrawString(LblStr, Grid_Font, myBrush, 0, 0, String_format)
                                    End If
                            End Select
                        Case 6
                            Select Case N
                                Case 0, 13, 22, 31, 40, 49, 58, 63 'ok
                                    eG.DrawString(LblStr, Grid_Font, myBrush, 0, 0, String_format)
                                Case Else
                                    If YNow = yMax And YMaxReached Then
                                        eG.DrawString(LblStr, Grid_Font, myBrush, 0, 0, String_format)
                                    End If
                            End Select
                        Case 7
                            Select Case N
                                Case 0, 12, 21, 30, 39, 48, 57, 63 'ok
                                    eG.DrawString(LblStr, Grid_Font, myBrush, 0, 0, String_format)
                                Case Else
                                    If YNow = yMax And YMaxReached Then
                                        eG.DrawString(LblStr, Grid_Font, myBrush, 0, 0, String_format)
                                    End If
                            End Select
                        Case 8
                            Select Case N
                                Case 0, 11, 20, 29, 38, 47, 56, 63 'ok
                                    eG.DrawString(LblStr, Grid_Font, myBrush, 0, 0, String_format)
                                Case Else
                                    If YNow = yMax And YMaxReached Then
                                        eG.DrawString(LblStr, Grid_Font, myBrush, 0, 0, String_format)
                                    End If
                            End Select
                        Case 9
                            Select Case N
                                Case 0, 10, 19, 28, 37, 46, 55, 63 'ok
                                    eG.DrawString(LblStr, Grid_Font, myBrush, 0, 0, String_format)
                                Case Else
                                    If YNow = yMax And YMaxReached Then
                                        eG.DrawString(LblStr, Grid_Font, myBrush, 0, 0, String_format)
                                    End If
                            End Select
                    End Select
                Case Is <= 8
                    Select Case YMinFirstChar
                        Case 1
                            Select Case N
                                Case 0, 9, 18, 27, 36, 45, 54, 63, 72 'ok
                                    eG.DrawString(LblStr, Grid_Font, myBrush, 0, 0, String_format)
                                Case Else
                                    If YNow = yMax And YMaxReached Then
                                        eG.DrawString(LblStr, Grid_Font, myBrush, 0, 0, String_format)
                                    End If
                            End Select
                        Case 2
                            Select Case N
                                Case 0, 9, 18, 27, 36, 45, 54, 63, 72 'ok
                                    eG.DrawString(LblStr, Grid_Font, myBrush, 0, 0, String_format)
                                Case Else
                                    If YNow = yMax And YMaxReached Then
                                        eG.DrawString(LblStr, Grid_Font, myBrush, 0, 0, String_format)
                                    End If
                            End Select
                        Case 3
                            Select Case N
                                Case 0, 8, 17, 26, 35, 44, 53, 62, 72 'ok
                                    eG.DrawString(LblStr, Grid_Font, myBrush, 0, 0, String_format)
                                Case Else
                                    If YNow = yMax And YMaxReached Then
                                        eG.DrawString(LblStr, Grid_Font, myBrush, 0, 0, String_format)
                                    End If
                            End Select
                        Case 4
                            Select Case N
                                Case 0, 7, 16, 25, 34, 43, 52, 61, 72 'ok
                                    eG.DrawString(LblStr, Grid_Font, myBrush, 0, 0, String_format)
                                Case Else
                                    If YNow = yMax And YMaxReached Then
                                        eG.DrawString(LblStr, Grid_Font, myBrush, 0, 0, String_format)
                                    End If
                            End Select
                        Case 5
                            Select Case N
                                Case 0, 14, 23, 32, 41, 50, 59, 68, 72 'ok
                                    eG.DrawString(LblStr, Grid_Font, myBrush, 0, 0, String_format)
                                Case Else
                                    If YNow = yMax And YMaxReached Then
                                        eG.DrawString(LblStr, Grid_Font, myBrush, 0, 0, String_format)
                                    End If
                            End Select
                        Case 6
                            Select Case N
                                Case 0, 13, 22, 31, 40, 49, 58, 67, 72 'ok
                                    eG.DrawString(LblStr, Grid_Font, myBrush, 0, 0, String_format)
                                Case Else
                                    If YNow = yMax And YMaxReached Then
                                        eG.DrawString(LblStr, Grid_Font, myBrush, 0, 0, String_format)
                                    End If
                            End Select
                        Case 7
                            Select Case N
                                Case 0, 12, 21, 30, 39, 48, 57, 66, 72 'ok
                                    eG.DrawString(LblStr, Grid_Font, myBrush, 0, 0, String_format)
                                Case Else
                                    If YNow = yMax And YMaxReached Then
                                        eG.DrawString(LblStr, Grid_Font, myBrush, 0, 0, String_format)
                                    End If
                            End Select
                        Case 8
                            Select Case N
                                Case 0, 11, 20, 29, 38, 47, 56, 65, 72 'ok
                                    eG.DrawString(LblStr, Grid_Font, myBrush, 0, 0, String_format)
                                Case Else
                                    If YNow = yMax And YMaxReached Then
                                        eG.DrawString(LblStr, Grid_Font, myBrush, 0, 0, String_format)
                                    End If
                            End Select
                        Case 9
                            Select Case N
                                Case 0, 10, 19, 28, 37, 46, 55, 64, 72 'ok
                                    eG.DrawString(LblStr, Grid_Font, myBrush, 0, 0, String_format)
                                Case Else
                                    If YNow = yMax And YMaxReached Then
                                        eG.DrawString(LblStr, Grid_Font, myBrush, 0, 0, String_format)
                                    End If
                            End Select
                    End Select
                Case Is < 10
                    Select Case YMinFirstChar
                        Case 1
                            Select Case N
                                Case 0, 9, 18, 27, 36, 45, 54, 63, 72, 81 'ok
                                    eG.DrawString(LblStr, Grid_Font, myBrush, 0, 0, String_format)
                                Case Else
                                    If YNow = yMax And YMaxReached Then
                                        eG.DrawString(LblStr, Grid_Font, myBrush, 0, 0, String_format)
                                    End If
                            End Select
                        Case 2
                            Select Case N
                                Case 0, 9, 18, 27, 36, 45, 54, 63, 72, 81 'ok
                                    eG.DrawString(LblStr, Grid_Font, myBrush, 0, 0, String_format)
                                Case Else
                                    If YNow = yMax And YMaxReached Then
                                        eG.DrawString(LblStr, Grid_Font, myBrush, 0, 0, String_format)
                                    End If
                            End Select
                        Case 3
                            Select Case N
                                Case 0, 8, 17, 26, 35, 44, 53, 62, 71, 81 'ok
                                    eG.DrawString(LblStr, Grid_Font, myBrush, 0, 0, String_format)
                                Case Else
                                    If YNow = yMax And YMaxReached Then
                                        eG.DrawString(LblStr, Grid_Font, myBrush, 0, 0, String_format)
                                    End If
                            End Select
                        Case 4
                            Select Case N
                                Case 0, 7, 16, 25, 34, 43, 52, 61, 70, 81 'ok
                                    eG.DrawString(LblStr, Grid_Font, myBrush, 0, 0, String_format)
                                Case Else
                                    If YNow = yMax And YMaxReached Then
                                        eG.DrawString(LblStr, Grid_Font, myBrush, 0, 0, String_format)
                                    End If
                            End Select
                        Case 5
                            Select Case N
                                Case 0, 14, 23, 32, 41, 50, 59, 68, 77, 81 'ok
                                    eG.DrawString(LblStr, Grid_Font, myBrush, 0, 0, String_format)
                                Case Else
                                    If YNow = yMax And YMaxReached Then
                                        eG.DrawString(LblStr, Grid_Font, myBrush, 0, 0, String_format)
                                    End If
                            End Select
                        Case 6
                            Select Case N
                                Case 0, 13, 22, 31, 40, 49, 58, 67, 76, 81 'ok
                                    eG.DrawString(LblStr, Grid_Font, myBrush, 0, 0, String_format)
                                Case Else
                                    If YNow = yMax And YMaxReached Then
                                        eG.DrawString(LblStr, Grid_Font, myBrush, 0, 0, String_format)
                                    End If
                            End Select
                        Case 7
                            Select Case N
                                Case 0, 12, 21, 30, 39, 48, 57, 66, 75, 81 'ok
                                    eG.DrawString(LblStr, Grid_Font, myBrush, 0, 0, String_format)
                                Case Else
                                    If YNow = yMax And YMaxReached Then
                                        eG.DrawString(LblStr, Grid_Font, myBrush, 0, 0, String_format)
                                    End If
                            End Select
                        Case 8
                            Select Case N
                                Case 0, 11, 20, 29, 38, 47, 56, 65, 74, 81 'ok
                                    eG.DrawString(LblStr, Grid_Font, myBrush, 0, 0, String_format)
                                Case Else
                                    If YNow = yMax And YMaxReached Then
                                        eG.DrawString(LblStr, Grid_Font, myBrush, 0, 0, String_format)
                                    End If
                            End Select
                        Case 9
                            Select Case N
                                Case 0, 10, 19, 28, 37, 46, 55, 64, 73, 81 'ok
                                    eG.DrawString(LblStr, Grid_Font, myBrush, 0, 0, String_format)
                                Case Else
                                    If YNow = yMax And YMaxReached Then
                                        eG.DrawString(LblStr, Grid_Font, myBrush, 0, 0, String_format)
                                    End If
                            End Select
                    End Select
                Case Is >= 10
                    Select Case N
                        Case 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24, 25, 26 'ok
                            eG.DrawString(LblStr, Grid_Font, myBrush, 0, 0, String_format)
                        Case Else
                            If YNow = yMax And YMaxReached Then
                                eG.DrawString(LblStr, Grid_Font, myBrush, 0, 0, String_format)
                            End If
                    End Select
            End Select
            YNow += GridSpacing
            'Give A Little Tolerance To XMax Due To Rounding
            N += 1
            eG.ResetTransform()
            If N > 250 Or YMaxReached Then Exit Do
        Loop While YNow < yMax + (yMax * 0.00001) 'Give Tolerance
        myBrush.Dispose()
        String_format.Dispose()
        eG.Transform.Dispose()
    End Sub
    Private Sub DrawLogX_GridLabel(eG As Graphics, ctlLeft As Single, ctlRight As Single, xMin As Single, xMax As Single)
        If XAxisStyle = ScaleStyleChoices.Linear Then Exit Sub
        If Graph_Style = GraphStyleChoices.Pie Then Exit Sub
        If Not Show_XLabels Then Exit Sub
        Dim yPos As Single
        If YAxisStyle = ScaleStyleChoices.Linear Then
            yPos = GetClientPoint(0, ClientSize.Height, ControlTop, ControlBottom, GraphAreaY1)
        Else 'Log(x)
            yPos = GetClientPoint(0, ClientSize.Height, ControlTop, ControlBottom, Log(GraphAreaY1) / Log(YAxisStyle))
        End If
        Dim Y_Pointx As Single
        Dim String_format As New StringFormat
        Select Case Rotate_XGridLabels
            Case = 90
                Y_Pointx = yPos + (yPos * 0.01)
                String_format.Alignment = StringAlignment.Near
                String_format.LineAlignment = StringAlignment.Center
                String_format.FormatFlags = StringFormatFlags.NoWrap
            Case >= 45
                Y_Pointx = yPos + (yPos * 0.02)
                String_format.Alignment = StringAlignment.Near
                String_format.LineAlignment = StringAlignment.Center
                String_format.FormatFlags = StringFormatFlags.NoWrap
            Case > 0
                Y_Pointx = yPos + (yPos * 0.025)
                String_format.Alignment = StringAlignment.Near
                String_format.LineAlignment = StringAlignment.Center
                String_format.FormatFlags = StringFormatFlags.NoWrap
            Case = 0
                Y_Pointx = yPos + (yPos * 0.03)
                String_format.Alignment = StringAlignment.Center
                String_format.LineAlignment = StringAlignment.Center
                String_format.FormatFlags = StringFormatFlags.DirectionRightToLeft
            Case > -45
                Y_Pointx = yPos + (yPos * 0.025)
                String_format.Alignment = StringAlignment.Far
                String_format.LineAlignment = StringAlignment.Center
                String_format.FormatFlags = StringFormatFlags.NoWrap
            Case > -90
                Y_Pointx = yPos + (yPos * 0.02)
                String_format.Alignment = StringAlignment.Far
                String_format.LineAlignment = StringAlignment.Center
                String_format.FormatFlags = StringFormatFlags.NoWrap
            Case = -90
                Y_Pointx = yPos + (yPos * 0.01)
                String_format.Alignment = StringAlignment.Far
                String_format.LineAlignment = StringAlignment.Center
                String_format.FormatFlags = StringFormatFlags.NoWrap
        End Select
        'XOctaves Are Used For X-Axis Log Grid Line Label Placement.
        'Only Use Log10 For Octaves Calculation To Maintain Persistent Labeling.
        Dim Octaves As Single = Round(((Log(xMax) / Log(10)) - 1) - ((Log(xMin) / Log(10)) - 1), 2)
        eG.SmoothingMode = SmoothingMode.HighQuality
        Dim myBrush = New SolidBrush(GridLabel_Color)
        Dim XNow As Single = xMin
        Dim N As Integer = 0
        Dim GridSpacing As Single = 0
        Dim LblStr As String
        Dim XMaxReached As Boolean = False
        Do
            Select Case Octaves
                Case Is <= 1 'Draw 10 Grid Lines
                    GridSpacing = CSng((xMax - xMin) / 10)
                Case Is < 10
                    Select Case XNow
                        Case Is < 1.0E-24 '<10p
                            GridSpacing = 1.0E-25
                        Case Is < 1.0E-23 '<10p
                            GridSpacing = 1.0E-24
                        Case Is < 1.0E-22 '<10p
                            GridSpacing = 1.0E-23
                        Case Is < 1.0E-21 '<10p
                            GridSpacing = 1.0E-22
                        Case Is < 1.0E-20 '<10p
                            GridSpacing = 1.0E-21
                        Case Is < 1.0E-19 '<10p
                            GridSpacing = 1.0E-20
                        Case Is < 1.0E-18 '<10p
                            GridSpacing = 1.0E-19
                        Case Is < 1.0E-17 '<10p
                            GridSpacing = 1.0E-18
                        Case Is < 0.0000000000000001 '<10p
                            GridSpacing = 1.0E-17
                        Case Is < 0.000000000000001 '<10p
                            GridSpacing = 0.0000000000000001
                        Case Is < 0.00000000000001 '<10p
                            GridSpacing = 0.000000000000001
                        Case Is < 0.0000000000001 '<10p
                            GridSpacing = 0.00000000000001
                        Case Is < 0.000000000001 '<10p
                            GridSpacing = 0.0000000000001
                        Case Is < 0.00000000001 '<10p
                            GridSpacing = 0.000000000001
                        Case Is < 0.0000000001 '<100p
                            GridSpacing = 0.00000000001
                        Case Is < 0.000000001 '<1n
                            GridSpacing = 0.0000000001
                        Case Is < 0.00000001 '<10n
                            GridSpacing = 0.000000001
                        Case Is < 0.0000001 '<100n
                            GridSpacing = 0.00000001
                        Case Is < 0.000001 '<1u
                            GridSpacing = 0.0000001
                        Case Is < 0.00001 '<10u
                            GridSpacing = 0.000001
                        Case Is < 0.0001 '<100u
                            GridSpacing = 0.00001
                        Case Is < 0.001 '<1m
                            GridSpacing = 0.0001
                        Case Is < 0.01 '<10m
                            GridSpacing = 0.001
                        Case Is < 0.1 '<100m
                            GridSpacing = 0.01
                        Case Is < 1
                            GridSpacing = 0.1
                        Case Is < 10
                            GridSpacing = 1
                        Case Is < 100
                            GridSpacing = 10
                        Case Is < 1000
                            GridSpacing = 100
                        Case Is < 10000
                            GridSpacing = 1000
                        Case Is < 100000
                            GridSpacing = 10000
                        Case Is < 1000000
                            GridSpacing = 100000
                        Case Is < 10000000 '10M
                            GridSpacing = 1000000
                        Case Is < 100000000 '100M
                            GridSpacing = 10000000
                        Case Is < 1000000000 '1000M
                            GridSpacing = 100000000
                        Case Is < 10000000000.0# '10G
                            GridSpacing = 1000000000
                        Case Is < 100000000000.0# '100G
                            GridSpacing = 10000000000.0#
                        Case Is < 1000000000000.0# '1000G
                            GridSpacing = 100000000000.0#
                        Case Is < 10000000000000.0#
                            GridSpacing = 1000000000000.0#
                        Case Is < 100000000000000.0#
                            GridSpacing = 10000000000000.0#
                        Case Is < 1.0E+15#
                            GridSpacing = 100000000000000.0#
                        Case Is < 1.0E+16#
                            GridSpacing = 1.0E+15#
                        Case Is < 1.0E+17#
                            GridSpacing = 1.0E+16#
                        Case Is < 1.0E+18#
                            GridSpacing = 1.0E+17#
                        Case Is < 1.0E+19#
                            GridSpacing = 1.0E+18#
                        Case Is < 1.0E+20#
                            GridSpacing = 1.0E+19#
                        Case Is < 1.0E+21#
                            GridSpacing = 1.0E+20#
                        Case Is < 1.0E+22#
                            GridSpacing = 1.0E+21#
                        Case Is < 1.0E+23#
                            GridSpacing = 1.0E+22#
                        Case Is < 1.0E+24#
                            GridSpacing = 1.0E+23#
                        Case Is < 1.0E+25#
                            GridSpacing = 1.0E+24#
                    End Select
                Case Is >= 10
                    Select Case XNow
                        Case Is < 1.0E-24 '<10p
                            GridSpacing = 1.0E-25
                        Case Is < 1.0E-23 '<10p
                            GridSpacing = 1.0E-24
                        Case Is < 1.0E-22 '<10p
                            GridSpacing = 1.0E-23
                        Case Is < 1.0E-21 '<10p
                            GridSpacing = 1.0E-22
                        Case Is < 1.0E-20 '<10p
                            GridSpacing = 1.0E-21
                        Case Is < 1.0E-19 '<10p
                            GridSpacing = 1.0E-20
                        Case Is < 1.0E-18 '<10p
                            GridSpacing = 1.0E-19
                        Case Is < 1.0E-17 '<10p
                            GridSpacing = 1.0E-18
                        Case Is < 0.0000000000000001 '<10p
                            GridSpacing = 1.0E-17
                        Case Is < 0.000000000000001 '<10p
                            GridSpacing = 0.0000000000000001
                        Case Is < 0.00000000000001 '<10p
                            GridSpacing = 0.000000000000001
                        Case Is < 0.0000000000001 '<10p
                            GridSpacing = 0.00000000000001
                        Case Is < 0.000000000001 '<10p
                            GridSpacing = 0.0000000000001
                        Case Is < 0.00000000001 '<10p
                            GridSpacing = 0.000000000001
                        Case Is < 0.0000000001 '<100p
                            GridSpacing = 0.00000000001
                        Case Is < 0.000000001 '<1n
                            GridSpacing = 0.0000000001
                        Case Is < 0.00000001 '<10n
                            GridSpacing = 0.000000001
                        Case Is < 0.0000001 '<100n
                            GridSpacing = 0.00000001
                        Case Is < 0.000001 '<1u
                            GridSpacing = 0.0000001
                        Case Is < 0.00001 '<10u
                            GridSpacing = 0.000001
                        Case Is < 0.0001 '<100u
                            GridSpacing = 0.00001
                        Case Is < 0.001 '<1m
                            GridSpacing = 0.0001
                        Case Is < 0.01 '<10m
                            GridSpacing = 0.001
                        Case Is < 0.1 '<100m
                            GridSpacing = 0.01
                        Case Is < 1
                            GridSpacing = 1
                        Case Is < 10
                            GridSpacing = 10
                        Case Is < 100
                            GridSpacing = 100
                        Case Is < 1000
                            GridSpacing = 1000
                        Case Is < 10000
                            GridSpacing = 10000
                        Case Is < 100000
                            GridSpacing = 100000
                        Case Is < 1000000
                            GridSpacing = 1000000
                        Case Is < 10000000 '10M
                            GridSpacing = 10000000
                        Case Is < 100000000 '100M
                            GridSpacing = 100000000
                        Case Is < 1000000000 '1000M
                            GridSpacing = 1000000000
                        Case Is < 10000000000.0# '10G
                            GridSpacing = 10000000000
                        Case Is < 100000000000.0# '100G
                            GridSpacing = 100000000000.0#
                        Case Is < 1000000000000.0# '1000G
                            GridSpacing = 1000000000000.0#
                        Case Is < 10000000000000.0#
                            GridSpacing = 10000000000000.0#
                        Case Is < 100000000000000.0#
                            GridSpacing = 100000000000000.0#
                        Case Is < 1.0E+15#
                            GridSpacing = 1.0E+15#
                        Case Is < 1.0E+16#
                            GridSpacing = 1.0E+16#
                        Case Is < 1.0E+17#
                            GridSpacing = 1.0E+17#
                        Case Is < 1.0E+18#
                            GridSpacing = 1.0E+18#
                        Case Is < 1.0E+19#
                            GridSpacing = 1.0E+19#
                        Case Is < 1.0E+20#
                            GridSpacing = 1.0E+20#
                        Case Is < 1.0E+21#
                            GridSpacing = 1.0E+21#
                        Case Is < 1.0E+22#
                            GridSpacing = 1.0E+22#
                        Case Is < 1.0E+23#
                            GridSpacing = 1.0E+23#
                        Case Is < 1.0E+24#
                            GridSpacing = 1.0E+24#
                        Case Is < 1.0E+25#
                            GridSpacing = 1.0E+25#
                    End Select
            End Select
            'Give Some Tolorance, Makes Sure Last Grid Line Gets Labeled
            'Grid Spacing Sometimes Causes The Loop To Exit Early
            If XNow + GridSpacing > xMax + (xMax * 0.01) Then
                XNow = xMax
                XMaxReached = True
            End If
            Dim Count As Integer = 0
            Dim xMinFirstChar As String
            'XMinFirstChar Along With Octaves Determines The GridLabeling Template 
            Do
                Count += 1
                xMinFirstChar = Mid(xMin, Count, 1)
                If xMinFirstChar = "." Then xMinFirstChar = ""
                If xMinFirstChar = "-" Then xMinFirstChar = ""
                If xMinFirstChar = "0" Then xMinFirstChar = ""
            Loop While xMinFirstChar = "" Or Count > 24
            Dim XPos As Single
            'Translate The Custom Scale Point To The Original Control Scale Point
            XPos = Log(XNow) / Log(XAxisStyle)
            Dim X_Pointx As Single = GetClientPoint(0, ClientSize.Width, ctlLeft, ctlRight, XPos)
            Dim Points = New PointF(X_Pointx, Y_Pointx)
            eG.ScaleTransform(1, 1) 'Increases Or Decreases The Size Of The Coordinate System
            eG.TranslateTransform(X_Pointx, Y_Pointx, MatrixOrder.Append)
            eG.RotateTransform(Rotate_XGridLabels)
            LblStr = ReturnTruncated(XNow)
            Select Case Octaves
                Case Is <= 1
                    Select Case N
                        Case 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10
                            eG.DrawString(LblStr, Grid_Font, myBrush, 0, 0, String_format)
                            If XNow = xMax And XMaxReached Then
                                eG.DrawString(LblStr, Grid_Font, myBrush, 0, 0, String_format)
                            End If
                    End Select
                Case Is <= 2
                    Select Case xMinFirstChar
                        Case 1
                            Select Case N
                                Case 0, 1, 2, 4, 6, 9, 10, 11, 13, 15, 18 'ok
                                    eG.DrawString(LblStr, Grid_Font, myBrush, 0, 0, String_format)
                                Case Else
                                    If XNow = xMax And XMaxReached Then
                                        eG.DrawString(LblStr, Grid_Font, myBrush, 0, 0, String_format)
                                    End If
                            End Select
                        Case 2
                            Select Case N
                                Case 0, 2, 4, 6, 8, 9, 11, 13, 15, 17, 18 'ok
                                    eG.DrawString(LblStr, Grid_Font, myBrush, 0, 0, String_format)
                                Case Else
                                    If XNow = xMax And XMaxReached Then
                                        eG.DrawString(LblStr, Grid_Font, myBrush, 0, 0, String_format)
                                    End If
                            End Select
                        Case 3
                            Select Case N
                                Case 0, 2, 4, 7, 8, 9, 11, 13, 16, 17, 18 'ok
                                    eG.DrawString(LblStr, Grid_Font, myBrush, 0, 0, String_format)
                                Case Else
                                    If XNow = xMax And XMaxReached Then
                                        eG.DrawString(LblStr, Grid_Font, myBrush, 0, 0, String_format)
                                    End If
                            End Select
                        Case 4
                            Select Case N
                                Case 0, 2, 4, 6, 7, 8, 9, 11, 13, 15, 16, 18 'ok
                                    eG.DrawString(LblStr, Grid_Font, myBrush, 0, 0, String_format)
                                Case Else
                                    If XNow = xMax And XMaxReached Then
                                        eG.DrawString(LblStr, Grid_Font, myBrush, 0, 0, String_format)
                                    End If
                            End Select
                        Case 5
                            Select Case N
                                Case 0, 2, 5, 6, 7, 9, 11, 14, 15, 16, 18 '0K
                                    eG.DrawString(LblStr, Grid_Font, myBrush, 0, 0, String_format)
                                Case Else
                                    If XNow = xMax And XMaxReached Then
                                        eG.DrawString(LblStr, Grid_Font, myBrush, 0, 0, String_format)
                                    End If
                            End Select
                        Case 6
                            Select Case N
                                Case 0, 2, 4, 5, 7, 9, 13, 14, 16, 18 'ok
                                    eG.DrawString(LblStr, Grid_Font, myBrush, 0, 0, String_format)
                                Case Else
                                    If XNow = xMax And XMaxReached Then
                                        eG.DrawString(LblStr, Grid_Font, myBrush, 0, 0, String_format)
                                    End If
                            End Select
                        Case 7
                            Select Case N
                                Case 0, 3, 4, 5, 7, 9, 12, 13, 14, 16, 18 'ok
                                    eG.DrawString(LblStr, Grid_Font, myBrush, 0, 0, String_format)
                                Case Else
                                    If XNow = xMax And XMaxReached Then
                                        eG.DrawString(LblStr, Grid_Font, myBrush, 0, 0, String_format)
                                    End If
                            End Select
                        Case 8
                            Select Case N
                                Case 0, 2, 3, 4, 5, 7, 9, 11, 12, 13, 14, 16, 18 'ok
                                    eG.DrawString(LblStr, Grid_Font, myBrush, 0, 0, String_format)
                                Case Else
                                    If XNow = xMax And XMaxReached Then
                                        eG.DrawString(LblStr, Grid_Font, myBrush, 0, 0, String_format)
                                    End If
                            End Select
                        Case 9
                            Select Case N
                                Case 0, 2, 3, 4, 6, 8, 10, 11, 12, 13, 15, 18 'ok
                                    eG.DrawString(LblStr, Grid_Font, myBrush, 0, 0, String_format)
                                Case Else
                                    If XNow = xMax And XMaxReached Then
                                        eG.DrawString(LblStr, Grid_Font, myBrush, 0, 0, String_format)
                                    End If
                            End Select
                    End Select
                Case Is <= 3
                    Select Case xMinFirstChar
                        Case 1
                            Select Case N
                                Case 0, 1, 2, 3, 5, 9, 10, 11, 12, 14, 18, 19, 20, 21, 23, 27 '0k
                                    eG.DrawString(LblStr, Grid_Font, myBrush, 0, 0, String_format)
                                Case Else
                                    If XNow = xMax And XMaxReached Then
                                        eG.DrawString(LblStr, Grid_Font, myBrush, 0, 0, String_format)
                                    End If
                            End Select
                        Case 2
                            Select Case N
                                Case 0, 1, 2, 4, 8, 9, 10, 11, 13, 17, 18, 19, 20, 22, 26, 27 'ok
                                    eG.DrawString(LblStr, Grid_Font, myBrush, 0, 0, String_format)
                                Case Else
                                    If XNow = xMax And XMaxReached Then
                                        eG.DrawString(LblStr, Grid_Font, myBrush, 0, 0, String_format)
                                    End If
                            End Select
                        Case 3
                            Select Case N
                                Case 0, 1, 3, 7, 8, 9, 10, 12, 16, 17, 18, 19, 21, 25, 27 'ok
                                    eG.DrawString(LblStr, Grid_Font, myBrush, 0, 0, String_format)
                                Case Else
                                    If XNow = xMax And XMaxReached Then
                                        eG.DrawString(LblStr, Grid_Font, myBrush, 0, 0, String_format)
                                    End If
                            End Select
                        Case 4
                            Select Case N
                                Case 0, 2, 6, 7, 8, 9, 11, 15, 16, 17, 18, 20, 24, 25, 27 'ok
                                    eG.DrawString(LblStr, Grid_Font, myBrush, 0, 0, String_format)
                                Case Else
                                    If XNow = xMax And XMaxReached Then
                                        eG.DrawString(LblStr, Grid_Font, myBrush, 0, 0, String_format)
                                    End If
                            End Select
                        Case 5
                            Select Case N
                                Case 0, 2, 5, 6, 7, 9, 11, 14, 15, 16, 18, 20, 23, 24, 27 'ok
                                    eG.DrawString(LblStr, Grid_Font, myBrush, 0, 0, String_format)
                                Case Else
                                    If XNow = xMax And XMaxReached Then
                                        eG.DrawString(LblStr, Grid_Font, myBrush, 0, 0, String_format)
                                    End If
                            End Select
                        Case 6
                            Select Case N
                                Case 0, 4, 5, 6, 7, 9, 13, 14, 15, 16, 18, 22, 23, 24, 27 'ok
                                    eG.DrawString(LblStr, Grid_Font, myBrush, 0, 0, String_format)
                                Case Else
                                    If XNow = xMax And XMaxReached Then
                                        eG.DrawString(LblStr, Grid_Font, myBrush, 0, 0, String_format)
                                    End If
                            End Select
                        Case 7
                            Select Case N
                                Case 0, 3, 4, 5, 7, 9, 12, 13, 14, 16, 18, 21, 22, 23, 27 'ok
                                    eG.DrawString(LblStr, Grid_Font, myBrush, 0, 0, String_format)
                                Case Else
                                    If XNow = xMax And XMaxReached Then
                                        eG.DrawString(LblStr, Grid_Font, myBrush, 0, 0, String_format)
                                    End If
                            End Select
                        Case 8
                            Select Case N
                                Case 0, 3, 4, 6, 8, 11, 12, 13, 15, 17, 20, 21, 22, 24, 27 'ok
                                    eG.DrawString(LblStr, Grid_Font, myBrush, 0, 0, String_format)
                                Case Else
                                    If XNow = xMax And XMaxReached Then
                                        eG.DrawString(LblStr, Grid_Font, myBrush, 0, 0, String_format)
                                    End If
                            End Select
                        Case 9
                            Select Case N
                                Case 0, 2, 3, 5, 7, 10, 11, 12, 14, 16, 19, 20, 21, 23, 27 'ok
                                    eG.DrawString(LblStr, Grid_Font, myBrush, 0, 0, String_format)
                                Case Else
                                    If XNow = xMax And XMaxReached Then
                                        eG.DrawString(LblStr, Grid_Font, myBrush, 0, 0, String_format)
                                    End If
                            End Select
                    End Select
                Case Is <= 4
                    Select Case xMinFirstChar
                        Case 1
                            Select Case N
                                Case 0, 1, 3, 9, 10, 12, 18, 19, 21, 27, 28, 30, 36 'ok
                                    eG.DrawString(LblStr, Grid_Font, myBrush, 0, 0, String_format)
                                Case Else
                                    If XNow = xMax And XMaxReached Then
                                        eG.DrawString(LblStr, Grid_Font, myBrush, 0, 0, String_format)
                                    End If
                            End Select
                        Case 2
                            Select Case N
                                Case 0, 2, 8, 9, 11, 17, 18, 20, 26, 27, 29, 35, 36 'ok
                                    eG.DrawString(LblStr, Grid_Font, myBrush, 0, 0, String_format)
                                Case Else
                                    If XNow = xMax And XMaxReached Then
                                        eG.DrawString(LblStr, Grid_Font, myBrush, 0, 0, String_format)
                                    End If
                            End Select
                        Case 3
                            Select Case N
                                Case 0, 2, 7, 8, 11, 16, 17, 20, 25, 26, 29, 34, 36 'ok
                                    eG.DrawString(LblStr, Grid_Font, myBrush, 0, 0, String_format)
                                Case Else
                                    If XNow = xMax And XMaxReached Then
                                        eG.DrawString(LblStr, Grid_Font, myBrush, 0, 0, String_format)
                                    End If
                            End Select
                        Case 4
                            Select Case N
                                Case 0, 2, 6, 7, 10, 15, 16, 19, 24, 25, 28, 33, 34, 36 'ok
                                    eG.DrawString(LblStr, Grid_Font, myBrush, 0, 0, String_format)
                                Case Else
                                    If XNow = xMax And XMaxReached Then
                                        eG.DrawString(LblStr, Grid_Font, myBrush, 0, 0, String_format)
                                    End If
                            End Select
                        Case 5
                            Select Case N
                                Case 0, 5, 6, 9, 14, 15, 18, 23, 24, 27, 32, 33, 36 'ok
                                    eG.DrawString(LblStr, Grid_Font, myBrush, 0, 0, String_format)
                                Case Else
                                    If XNow = xMax And XMaxReached Then
                                        eG.DrawString(LblStr, Grid_Font, myBrush, 0, 0, String_format)
                                    End If
                            End Select
                        Case 6
                            Select Case N
                                Case 0, 4, 5, 8, 13, 14, 17, 22, 23, 26, 31, 32, 36 'ok
                                    eG.DrawString(LblStr, Grid_Font, myBrush, 0, 0, String_format)
                                Case Else
                                    If XNow = xMax And XMaxReached Then
                                        eG.DrawString(LblStr, Grid_Font, myBrush, 0, 0, String_format)
                                    End If
                            End Select
                        Case 7
                            Select Case N
                                Case 0, 4, 7, 12, 13, 16, 21, 22, 25, 30, 31, 36 'ok
                                    eG.DrawString(LblStr, Grid_Font, myBrush, 0, 0, String_format)
                                Case Else
                                    If XNow = xMax And XMaxReached Then
                                        eG.DrawString(LblStr, Grid_Font, myBrush, 0, 0, String_format)
                                    End If
                            End Select
                        Case 8
                            Select Case N
                                Case 0, 3, 6, 11, 12, 15, 20, 21, 24, 29, 30, 36 'ok
                                    eG.DrawString(LblStr, Grid_Font, myBrush, 0, 0, String_format)
                                Case Else
                                    If XNow = xMax And XMaxReached Then
                                        eG.DrawString(LblStr, Grid_Font, myBrush, 0, 0, String_format)
                                    End If
                            End Select
                        Case 9
                            Select Case N
                                Case 0, 2, 5, 10, 11, 14, 19, 20, 23, 28, 29, 36 'ok
                                    eG.DrawString(LblStr, Grid_Font, myBrush, 0, 0, String_format)
                                Case Else
                                    If XNow = xMax And XMaxReached Then
                                        eG.DrawString(LblStr, Grid_Font, myBrush, 0, 0, String_format)
                                    End If
                            End Select
                    End Select
                Case Is <= 5
                    Select Case xMinFirstChar
                        Case 1
                            Select Case N
                                Case 0, 1, 3, 9, 10, 12, 18, 19, 21, 27, 28, 30, 36, 37, 39 'ok
                                    eG.DrawString(LblStr, Grid_Font, myBrush, 0, 0, String_format)
                                Case Else
                                    If XNow = xMax And XMaxReached Then
                                        eG.DrawString(LblStr, Grid_Font, myBrush, 0, 0, String_format)
                                    End If
                            End Select
                        Case 2
                            Select Case N
                                Case 0, 2, 8, 9, 11, 17, 18, 20, 26, 27, 29, 35, 36, 38, 45 'ok
                                    eG.DrawString(LblStr, Grid_Font, myBrush, 0, 0, String_format)
                                Case Else
                                    If XNow = xMax And XMaxReached Then
                                        eG.DrawString(LblStr, Grid_Font, myBrush, 0, 0, String_format)
                                    End If
                            End Select
                        Case 3
                            Select Case N
                                Case 0, 2, 7, 8, 11, 16, 17, 20, 25, 26, 29, 34, 35, 38, 43, 45 'ok
                                    eG.DrawString(LblStr, Grid_Font, myBrush, 0, 0, String_format)
                                Case Else
                                    If XNow = xMax And XMaxReached Then
                                        eG.DrawString(LblStr, Grid_Font, myBrush, 0, 0, String_format)
                                    End If
                            End Select
                        Case 4
                            Select Case N
                                Case 0, 6, 7, 10, 15, 16, 19, 24, 25, 28, 33, 34, 37, 42, 45 'ok
                                    eG.DrawString(LblStr, Grid_Font, myBrush, 0, 0, String_format)
                                Case Else
                                    If XNow = xMax And XMaxReached Then
                                        eG.DrawString(LblStr, Grid_Font, myBrush, 0, 0, String_format)
                                    End If
                            End Select
                        Case 5
                            Select Case N
                                Case 0, 5, 6, 9, 14, 15, 18, 23, 24, 27, 32, 33, 36, 41, 42, 45 'ok
                                    eG.DrawString(LblStr, Grid_Font, myBrush, 0, 0, String_format)
                                Case Else
                                    If XNow = xMax And XMaxReached Then
                                        eG.DrawString(LblStr, Grid_Font, myBrush, 0, 0, String_format)
                                    End If
                            End Select
                        Case 6
                            Select Case N
                                Case 0, 4, 5, 8, 13, 14, 17, 22, 23, 26, 31, 32, 35, 40, 41, 41, 45 'ok
                                    eG.DrawString(LblStr, Grid_Font, myBrush, 0, 0, String_format)
                                Case Else
                                    If XNow = xMax And XMaxReached Then
                                        eG.DrawString(LblStr, Grid_Font, myBrush, 0, 0, String_format)
                                    End If
                            End Select
                        Case 7
                            Select Case N
                                Case 0, 4, 7, 12, 13, 16, 21, 22, 25, 30, 31, 34, 39, 40, 45 'ok
                                    eG.DrawString(LblStr, Grid_Font, myBrush, 0, 0, String_format)
                                Case Else
                                    If XNow = xMax And XMaxReached Then
                                        eG.DrawString(LblStr, Grid_Font, myBrush, 0, 0, String_format)
                                    End If
                            End Select
                        Case 8
                            Select Case N
                                Case 0, 3, 6, 11, 12, 15, 20, 21, 24, 29, 30, 33, 38, 39, 45 'ok
                                    eG.DrawString(LblStr, Grid_Font, myBrush, 0, 0, String_format)
                                Case Else
                                    If XNow = xMax And XMaxReached Then
                                        eG.DrawString(LblStr, Grid_Font, myBrush, 0, 0, String_format)
                                    End If
                            End Select
                        Case 9
                            Select Case N
                                Case 0, 2, 5, 10, 11, 14, 19, 20, 23, 29, 32, 37, 38, 45 'ok
                                    eG.DrawString(LblStr, Grid_Font, myBrush, 0, 0, String_format)
                                Case Else
                                    If XNow = xMax And XMaxReached Then
                                        eG.DrawString(LblStr, Grid_Font, myBrush, 0, 0, String_format)
                                    End If
                            End Select
                    End Select
                Case Is <= 6
                    Select Case xMinFirstChar
                        Case 1
                            Select Case N
                                Case 0, 1, 3, 9, 10, 12, 18, 19, 21, 27, 28, 30, 36, 37, 39, 45, 46, 48, 54 'ok
                                    eG.DrawString(LblStr, Grid_Font, myBrush, 0, 0, String_format)
                                Case Else
                                    If XNow = xMax And XMaxReached Then
                                        eG.DrawString(LblStr, Grid_Font, myBrush, 0, 0, String_format)
                                    End If
                            End Select
                        Case 2
                            Select Case N
                                Case 0, 2, 8, 9, 11, 17, 18, 20, 26, 27, 29, 35, 36, 38, 44, 45, 47, 53, 54 'ok
                                    eG.DrawString(LblStr, Grid_Font, myBrush, 0, 0, String_format)
                                Case Else
                                    If XNow = xMax And XMaxReached Then
                                        eG.DrawString(LblStr, Grid_Font, myBrush, 0, 0, String_format)
                                    End If
                            End Select
                        Case 3
                            Select Case N
                                Case 0, 2, 7, 8, 11, 16, 17, 20, 25, 26, 29, 34, 35, 38, 43, 44, 47, 52, 54 'ok
                                    eG.DrawString(LblStr, Grid_Font, myBrush, 0, 0, String_format)
                                Case Else
                                    If XNow = xMax And XMaxReached Then
                                        eG.DrawString(LblStr, Grid_Font, myBrush, 0, 0, String_format)
                                    End If
                            End Select
                        Case 4
                            Select Case N
                                Case 0, 6, 7, 10, 15, 16, 19, 24, 25, 28, 33, 34, 37, 42, 43, 46, 51 '0k
                                    eG.DrawString(LblStr, Grid_Font, myBrush, 0, 0, String_format)
                                Case Else
                                    If XNow = xMax And XMaxReached Then
                                        eG.DrawString(LblStr, Grid_Font, myBrush, 0, 0, String_format)
                                    End If
                            End Select
                        Case 5
                            Select Case N
                                Case 0, 5, 6, 9, 14, 15, 18, 23, 24, 27, 32, 33, 36, 41, 42, 45, 50, 51, 54 'ok
                                    eG.DrawString(LblStr, Grid_Font, myBrush, 0, 0, String_format)
                                Case Else
                                    If XNow = xMax And XMaxReached Then
                                        eG.DrawString(LblStr, Grid_Font, myBrush, 0, 0, String_format)
                                    End If
                            End Select
                        Case 6
                            Select Case N
                                Case 0, 4, 5, 8, 13, 14, 17, 22, 23, 26, 31, 32, 35, 40, 41, 44, 49, 50, 54 'ok
                                    eG.DrawString(LblStr, Grid_Font, myBrush, 0, 0, String_format)
                                Case Else
                                    If XNow = xMax And XMaxReached Then
                                        eG.DrawString(LblStr, Grid_Font, myBrush, 0, 0, String_format)
                                    End If
                            End Select
                        Case 7
                            Select Case N
                                Case 0, 4, 7, 12, 13, 16, 21, 22, 25, 30, 31, 34, 39, 40, 43, 48, 49, 54 'ok
                                    eG.DrawString(LblStr, Grid_Font, myBrush, 0, 0, String_format)
                                Case Else
                                    If XNow = xMax And XMaxReached Then
                                        eG.DrawString(LblStr, Grid_Font, myBrush, 0, 0, String_format)
                                    End If
                            End Select
                        Case 8
                            Select Case N
                                Case 0, 3, 6, 11, 12, 15, 20, 21, 24, 29, 30, 33, 38, 39, 42, 47, 48, 54 'ok
                                    eG.DrawString(LblStr, Grid_Font, myBrush, 0, 0, String_format)
                                Case Else
                                    If XNow = xMax And XMaxReached Then
                                        eG.DrawString(LblStr, Grid_Font, myBrush, 0, 0, String_format)
                                    End If
                            End Select
                        Case 9
                            Select Case N
                                Case 0, 2, 5, 10, 11, 14, 19, 20, 23, 29, 32, 37, 38, 41, 46, 47, 54 'ok
                                    eG.DrawString(LblStr, Grid_Font, myBrush, 0, 0, String_format)
                                Case Else
                                    If XNow = xMax And XMaxReached Then
                                        eG.DrawString(LblStr, Grid_Font, myBrush, 0, 0, String_format)
                                    End If
                            End Select
                    End Select
                Case Is <= 7
                    Select Case xMinFirstChar
                        Case 1
                            Select Case N
                                Case 0, 9, 18, 27, 36, 45, 54, 63 'OK
                                    eG.DrawString(LblStr, Grid_Font, myBrush, 0, 0, String_format)
                                Case Else
                                    If XNow = xMax And XMaxReached Then
                                        eG.DrawString(LblStr, Grid_Font, myBrush, 0, 0, String_format)
                                    End If
                            End Select
                        Case 2
                            Select Case N
                                Case 0, 9, 18, 27, 36, 45, 54, 63 'ok
                                    eG.DrawString(LblStr, Grid_Font, myBrush, 0, 0, String_format)
                                Case Else
                                    If XNow = xMax And XMaxReached Then
                                        eG.DrawString(LblStr, Grid_Font, myBrush, 0, 0, String_format)
                                    End If
                            End Select
                        Case 3
                            Select Case N
                                Case 0, 8, 17, 26, 35, 44, 53, 63 'ok
                                    eG.DrawString(LblStr, Grid_Font, myBrush, 0, 0, String_format)
                                Case Else
                                    If XNow = xMax And XMaxReached Then
                                        eG.DrawString(LblStr, Grid_Font, myBrush, 0, 0, String_format)
                                    End If
                            End Select
                        Case 4
                            Select Case N
                                Case 0, 7, 16, 25, 34, 43, 52, 63 'ok
                                    eG.DrawString(LblStr, Grid_Font, myBrush, 0, 0, String_format)
                                Case Else
                                    If XNow = xMax And XMaxReached Then
                                        eG.DrawString(LblStr, Grid_Font, myBrush, 0, 0, String_format)
                                    End If
                            End Select
                        Case 5
                            Select Case N
                                Case 0, 14, 23, 32, 41, 50, 59, 63 'ok
                                    eG.DrawString(LblStr, Grid_Font, myBrush, 0, 0, String_format)
                                Case Else
                                    If XNow = xMax And XMaxReached Then
                                        eG.DrawString(LblStr, Grid_Font, myBrush, 0, 0, String_format)
                                    End If
                            End Select
                        Case 6
                            Select Case N
                                Case 0, 13, 22, 31, 40, 49, 58, 63 'ok
                                    eG.DrawString(LblStr, Grid_Font, myBrush, 0, 0, String_format)
                                Case Else
                                    If XNow = xMax And XMaxReached Then
                                        eG.DrawString(LblStr, Grid_Font, myBrush, 0, 0, String_format)
                                    End If
                            End Select
                        Case 7
                            Select Case N
                                Case 0, 12, 21, 30, 39, 48, 57, 63 'ok
                                    eG.DrawString(LblStr, Grid_Font, myBrush, 0, 0, String_format)
                                Case Else
                                    If XNow = xMax And XMaxReached Then
                                        eG.DrawString(LblStr, Grid_Font, myBrush, 0, 0, String_format)
                                    End If
                            End Select
                        Case 8
                            Select Case N
                                Case 0, 11, 20, 29, 38, 47, 56, 63 'ok
                                    eG.DrawString(LblStr, Grid_Font, myBrush, 0, 0, String_format)
                                Case Else
                                    If XNow = xMax And XMaxReached Then
                                        eG.DrawString(LblStr, Grid_Font, myBrush, 0, 0, String_format)
                                    End If
                            End Select
                        Case 9
                            Select Case N
                                Case 0, 10, 19, 28, 37, 46, 55, 63 'ok
                                    eG.DrawString(LblStr, Grid_Font, myBrush, 0, 0, String_format)
                                Case Else
                                    If XNow = xMax And XMaxReached Then
                                        eG.DrawString(LblStr, Grid_Font, myBrush, 0, 0, String_format)
                                    End If
                            End Select
                    End Select
                Case Is <= 8
                    Select Case xMinFirstChar
                        Case 1
                            Select Case N
                                Case 0, 9, 18, 27, 36, 45, 54, 63, 72 'ok
                                    eG.DrawString(LblStr, Grid_Font, myBrush, 0, 0, String_format)
                                Case Else
                                    If XNow = xMax And XMaxReached Then
                                        eG.DrawString(LblStr, Grid_Font, myBrush, 0, 0, String_format)
                                    End If
                            End Select
                        Case 2
                            Select Case N
                                Case 0, 9, 18, 27, 36, 45, 54, 63, 72 'ok
                                    eG.DrawString(LblStr, Grid_Font, myBrush, 0, 0, String_format)
                                Case Else
                                    If XNow = xMax And XMaxReached Then
                                        eG.DrawString(LblStr, Grid_Font, myBrush, 0, 0, String_format)
                                    End If
                            End Select
                        Case 3
                            Select Case N
                                Case 0, 8, 17, 26, 35, 44, 53, 62, 72 'ok
                                    eG.DrawString(LblStr, Grid_Font, myBrush, 0, 0, String_format)
                                Case Else
                                    If XNow = xMax And XMaxReached Then
                                        eG.DrawString(LblStr, Grid_Font, myBrush, 0, 0, String_format)
                                    End If
                            End Select
                        Case 4
                            Select Case N
                                Case 0, 7, 16, 25, 34, 43, 52, 61, 72 'ok
                                    eG.DrawString(LblStr, Grid_Font, myBrush, 0, 0, String_format)
                                Case Else
                                    If XNow = xMax And XMaxReached Then
                                        eG.DrawString(LblStr, Grid_Font, myBrush, 0, 0, String_format)
                                    End If
                            End Select
                        Case 5
                            Select Case N
                                Case 0, 14, 23, 32, 41, 50, 59, 68, 72 'ok
                                    eG.DrawString(LblStr, Grid_Font, myBrush, 0, 0, String_format)
                                Case Else
                                    If XNow = xMax And XMaxReached Then
                                        eG.DrawString(LblStr, Grid_Font, myBrush, 0, 0, String_format)
                                    End If
                            End Select
                        Case 6
                            Select Case N
                                Case 0, 13, 22, 31, 40, 49, 58, 67, 72 'ok
                                    eG.DrawString(LblStr, Grid_Font, myBrush, 0, 0, String_format)
                                Case Else
                                    If XNow = xMax And XMaxReached Then
                                        eG.DrawString(LblStr, Grid_Font, myBrush, 0, 0, String_format)
                                    End If
                            End Select
                        Case 7
                            Select Case N
                                Case 0, 12, 21, 30, 39, 48, 57, 66, 72 'ok
                                    eG.DrawString(LblStr, Grid_Font, myBrush, 0, 0, String_format)
                                Case Else
                                    If XNow = xMax And XMaxReached Then
                                        eG.DrawString(LblStr, Grid_Font, myBrush, 0, 0, String_format)
                                    End If
                            End Select
                        Case 8
                            Select Case N
                                Case 0, 11, 20, 29, 38, 47, 56, 65, 72 'ok
                                    eG.DrawString(LblStr, Grid_Font, myBrush, 0, 0, String_format)
                                Case Else
                                    If XNow = xMax And XMaxReached Then
                                        eG.DrawString(LblStr, Grid_Font, myBrush, 0, 0, String_format)
                                    End If
                            End Select
                        Case 9
                            Select Case N
                                Case 0, 10, 19, 28, 37, 46, 55, 64, 72 'ok
                                    eG.DrawString(LblStr, Grid_Font, myBrush, 0, 0, String_format)
                                Case Else
                                    If XNow = xMax And XMaxReached Then
                                        eG.DrawString(LblStr, Grid_Font, myBrush, 0, 0, String_format)
                                    End If
                            End Select
                    End Select
                Case Is < 10
                    Select Case xMinFirstChar
                        Case 1
                            Select Case N
                                Case 0, 9, 18, 27, 36, 45, 54, 63, 72, 81 'ok
                                    eG.DrawString(LblStr, Grid_Font, myBrush, 0, 0, String_format)
                                Case Else
                                    If XNow = xMax And XMaxReached Then
                                        eG.DrawString(LblStr, Grid_Font, myBrush, 0, 0, String_format)
                                    End If
                            End Select
                        Case 2
                            Select Case N
                                Case 0, 9, 18, 27, 36, 45, 54, 63, 72, 81 'ok
                                    eG.DrawString(LblStr, Grid_Font, myBrush, 0, 0, String_format)
                                Case Else
                                    If XNow = xMax And XMaxReached Then
                                        eG.DrawString(LblStr, Grid_Font, myBrush, 0, 0, String_format)
                                    End If
                            End Select
                        Case 3
                            Select Case N
                                Case 0, 8, 17, 26, 35, 44, 53, 62, 71, 81 'ok
                                    eG.DrawString(LblStr, Grid_Font, myBrush, 0, 0, String_format)
                                Case Else
                                    If XNow = xMax And XMaxReached Then
                                        eG.DrawString(LblStr, Grid_Font, myBrush, 0, 0, String_format)
                                    End If
                            End Select
                        Case 4
                            Select Case N
                                Case 0, 7, 16, 25, 34, 43, 52, 61, 70, 81 'ok
                                    eG.DrawString(LblStr, Grid_Font, myBrush, 0, 0, String_format)
                                Case Else
                                    If XNow = xMax And XMaxReached Then
                                        eG.DrawString(LblStr, Grid_Font, myBrush, 0, 0, String_format)
                                    End If
                            End Select
                        Case 5
                            Select Case N
                                Case 0, 14, 23, 32, 41, 50, 59, 68, 77, 81 'ok
                                    eG.DrawString(LblStr, Grid_Font, myBrush, 0, 0, String_format)
                                Case Else
                                    If XNow = xMax And XMaxReached Then
                                        eG.DrawString(LblStr, Grid_Font, myBrush, 0, 0, String_format)
                                    End If
                            End Select
                        Case 6
                            Select Case N
                                Case 0, 13, 22, 31, 40, 49, 58, 67, 76, 81 'ok
                                    eG.DrawString(LblStr, Grid_Font, myBrush, 0, 0, String_format)
                                Case Else
                                    If XNow = xMax And XMaxReached Then
                                        eG.DrawString(LblStr, Grid_Font, myBrush, 0, 0, String_format)
                                    End If
                            End Select
                        Case 7
                            Select Case N
                                Case 0, 12, 21, 30, 39, 48, 57, 66, 75, 81 'ok
                                    eG.DrawString(LblStr, Grid_Font, myBrush, 0, 0, String_format)
                                Case Else
                                    If XNow = xMax And XMaxReached Then
                                        eG.DrawString(LblStr, Grid_Font, myBrush, 0, 0, String_format)
                                    End If
                            End Select
                        Case 8
                            Select Case N
                                Case 0, 11, 20, 29, 38, 47, 56, 65, 74, 81 'ok
                                    eG.DrawString(LblStr, Grid_Font, myBrush, 0, 0, String_format)
                                Case Else
                                    If XNow = xMax And XMaxReached Then
                                        eG.DrawString(LblStr, Grid_Font, myBrush, 0, 0, String_format)
                                    End If
                            End Select
                        Case 9
                            Select Case N
                                Case 0, 10, 19, 28, 37, 46, 55, 64, 73, 81 'ok
                                    eG.DrawString(LblStr, Grid_Font, myBrush, 0, 0, String_format)
                                Case Else
                                    If XNow = xMax And XMaxReached Then
                                        eG.DrawString(LblStr, Grid_Font, myBrush, 0, 0, String_format)
                                    End If
                            End Select
                    End Select
                Case Is >= 10
                    Select Case N
                        Case 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24, 25, 26 'ok
                            eG.DrawString(LblStr, Grid_Font, myBrush, 0, 0, String_format)
                        Case Else
                            If XNow = xMax And XMaxReached Then
                                eG.DrawString(LblStr, Grid_Font, myBrush, 0, 0, String_format)
                            End If
                    End Select
            End Select
            If Octaves >= 10 Then XNow = GridSpacing
            XNow += GridSpacing
            N += 1
            eG.ResetTransform()
            If N > 250 Or XMaxReached Then Exit Do
        Loop While XNow < xMax + (xMax * 0.00001) 'Give Tolerance
        myBrush.Dispose()
        String_format.Dispose()
        eG.Transform.Dispose()
    End Sub
    Dim Mouse_InverseTransform As Matrix
    Private Sub TestGraphCtl_MouseMove(sender As Object, e As MouseEventArgs) Handles Me.MouseMove
        If Not Show_MousePosition Then Exit Sub
        'Accomidate Mouse Positions For Inverse Scales
        Dim Xzero As Single
        Dim Xmax As Single
        Dim Yzero As Single
        Dim Ymax As Single
        Dim X_Inverse As Boolean
        Dim Y_Inverse As Boolean
        If GraphAreaX1 < GraphAreaX2 Then
            X_Inverse = False
        End If
        If GraphAreaX1 > GraphAreaX2 Then
            X_Inverse = True
        End If
        If GraphAreaY1 < GraphAreaY2 Then
            Y_Inverse = False
        End If
        If GraphAreaY1 > GraphAreaY2 Then
            Y_Inverse = True
        End If
        If Not X_Inverse And Not Y_Inverse Then
            Xzero = GraphAreaX1
            Xmax = GraphAreaX2
            Yzero = GraphAreaY1
            Ymax = GraphAreaY2
        ElseIf X_Inverse And Not Y_Inverse Then
            Xzero = GraphAreaX2
            Xmax = GraphAreaX1
            Yzero = GraphAreaY1
            Ymax = GraphAreaY2
        ElseIf X_Inverse And Y_Inverse Then
            Xzero = GraphAreaX2
            Xmax = GraphAreaX1
            Yzero = GraphAreaY2
            Ymax = GraphAreaY1
        ElseIf Y_Inverse And Not X_Inverse Then
            Xzero = GraphAreaX1
            Xmax = GraphAreaX2
            Yzero = GraphAreaY2
            Ymax = GraphAreaY1
        End If
        Dim XStyle As String = ""
        Dim YStyle As String = ""
        Select Case XAxisStyle
            Case ScaleStyleChoices.Linear
                XStyle = "X-Axis = Linear,  X = "
            Case ScaleStyleChoices.Log2
                XStyle = "X-Axis = Log2,  X = "
            Case ScaleStyleChoices.Loge
                XStyle = "X-Axis = Loge,  X = "
            Case ScaleStyleChoices.Log10
                XStyle = "X-Axis = Log10,  X = "
            Case ScaleStyleChoices.Log20
                XStyle = "X-Axis = Log20,  X = "
        End Select
        Select Case YAxisStyle
            Case ScaleStyleChoices.Linear
                YStyle = "  /  Y-Axis = Linear,  Y = "
            Case ScaleStyleChoices.Log2
                YStyle = "  /  Y-Axis = Log2,  Y = "
            Case ScaleStyleChoices.Loge
                YStyle = "  /  Y-Axis = Loge,  Y = "
            Case ScaleStyleChoices.Log10
                YStyle = "  /  Y-Axis = Log10,  Y = "
            Case ScaleStyleChoices.Log20
                YStyle = "  /  Y-Axis = Log20,  Y = "
        End Select
        'If Mouse Moves Before Paint Event Finishes Then Exit Sub
        'Because Mouse_InverseTransform.TransformPoints() Not Defined
        If Mouse_InverseTransform Is Nothing Then Exit Sub
        'Apply The Inverted Transformation To The Point.
        'Get Mouse Position Inside Custom Scaled Object
        Dim new_pos() As PointF = {New PointF(e.X, e.Y)}
        Mouse_InverseTransform.TransformPoints(new_pos)
        'Retrieves The Individual Mouse Pointer Positions
        Dim X As Single = new_pos.GetValue(0).X
        If XAxisStyle <> ScaleStyleChoices.Linear Then X = XAxisStyle ^ X
        Dim Y As Single = new_pos.GetValue(0).Y
        If YAxisStyle <> ScaleStyleChoices.Linear Then Y = YAxisStyle ^ Y
        'Restrict The Mouse Movement Text To Inside Graph Area Only.
        If X > Xmax Or X < Xzero Or Y > Ymax Or Y < Yzero Then Exit Sub
        Dim xMouseStr As String = ReturnTruncated(X)
        Dim yMouseStr As String = ReturnTruncated(Y)
        LblMouseMove.Text = XStyle & xMouseStr & YStyle & yMouseStr
    End Sub
    Private Shared Function GetClientPoint(ByVal ClientAStartPt As Single, ByVal ClientAEndPt As Single, ByVal ClientBStartPt As Single,
                                    ByVal ClientBEndPt As Single, PointToFind As Single) As Single
        'This Functions Return The Posiition Of The ClientA Object To Any Given Scaled Position On The ClientB Object.
        'ClientA Is The Reference Scale And ClientB Represents The Scale That The Unknown Point Is Part Off.
        ' PointToFind Is The Point On The Custom Scale (ClientB) That Represent Point On ClientA.
        'This Point On ClientA Is The Point In Question.
        Dim A_Inverse As Boolean
        Dim B_Inverse As Boolean
        Dim Inverse As Boolean
        Dim ClientBSize As Single
        Dim ClientASize As Single
        Dim This_Point As Single
        If ClientAStartPt > ClientAEndPt Then A_Inverse = True
        If ClientBStartPt > ClientBEndPt Then B_Inverse = True
        If A_Inverse And Not B_Inverse Then Inverse = True
        If Not A_Inverse And B_Inverse Then Inverse = True
        If Not A_Inverse And Not B_Inverse Then Inverse = False
        If A_Inverse And B_Inverse Then Inverse = False
        If ClientAStartPt >= 0 And ClientAEndPt > 0 Then
            ClientASize = Abs(ClientAEndPt - ClientAStartPt) '0,100 or 10,100 
            ClientBSize = Abs(ClientBStartPt - ClientBEndPt)
        ElseIf ClientBStartPt > 0 And ClientBEndPt >= 0 Then
            ClientASize = Abs(ClientAStartPt - ClientAEndPt)
            ClientBSize = Abs(ClientBEndPt - ClientBStartPt) '0,100 or 10,100
        Else
            ClientASize = Abs(ClientAStartPt - ClientAEndPt) '0,-100 or -10,-100 Examples
            ClientBSize = Abs(ClientBStartPt - ClientBEndPt) '0,-100 or 10,-100
        End If
        Dim K_Factor As Single = Abs(ClientASize / ClientBSize) 'X Amount Of A's Per X Amount Of B's
        'Calculate The Point
        Select Case Inverse
            Case False
                This_Point = ClientAStartPt - (K_Factor * (ClientBStartPt - PointToFind))
            Case True
                This_Point = ClientAStartPt - ((PointToFind - ClientBStartPt) * K_Factor)
        End Select
        Return This_Point
    End Function
    Private Function ReturnTruncated(ByVal NumToTruncate As Single) As String
        Dim TruncatedStr As String = ""
        Select Case Abs(NumToTruncate)
            Case >= 1.0E+24
                TruncatedStr = Round(NumToTruncate / 1.0E+24, 3) & "Y"
            Case >= 1.0E+21
                TruncatedStr = Round(NumToTruncate / 1.0E+21, 3) & "Z"
            Case >= 1.0E+18
                TruncatedStr = Round(NumToTruncate / 1.0E+18, 3) & "E"
            Case >= 1.0E+15
                TruncatedStr = Round(NumToTruncate / 1.0E+15, 3) & "P"
            Case >= 1000000000000.0
                TruncatedStr = Round(NumToTruncate / 1000000000000.0, 3) & "T"
            Case >= 1000000000.0
                TruncatedStr = Round(NumToTruncate / 1000000000, 3) & "G"
            Case >= 1000000.0
                TruncatedStr = Round(NumToTruncate / 1000000, 3) & "M"
            Case >= 1000.0
                TruncatedStr = Round(NumToTruncate / 1000, 3) & "K"
            Case >= 100
                TruncatedStr = CStr(Round(NumToTruncate, 3))
            Case >= 10
                TruncatedStr = CStr(Round(NumToTruncate, 3))
            Case >= 1
                TruncatedStr = CStr(Round(NumToTruncate, 3))
            Case = 0
                TruncatedStr = "0"
            Case >= 0.1
                TruncatedStr = CStr(Round(NumToTruncate, 3))
            Case >= 0.01
                TruncatedStr = Round(NumToTruncate * 1000, 3) & "m"
            Case >= 0.001
                TruncatedStr = Round(NumToTruncate * 1000, 3) & "m"
            Case >= 0.0001
                TruncatedStr = Round(NumToTruncate * 1000000, 3) & Chr(181)
            Case >= 0.000001
                TruncatedStr = Round(NumToTruncate * 1000000, 3) & Chr(181)
            Case >= 0.000000001
                TruncatedStr = Round(NumToTruncate * 1000000000, 3) & "n"
            Case >= 0.000000000001
                TruncatedStr = Round(NumToTruncate * 1000000000000, 3) & "p"
            Case >= 0.000000000000001
                TruncatedStr = Round(NumToTruncate * 1000000000000000, 3) & "f"
            Case >= 1.0E-18
                TruncatedStr = Round(NumToTruncate * 1.0E+18, 3) & "a"
            Case >= 1.0E-21
                TruncatedStr = Round(NumToTruncate * 1.0E+21, 3) & "z"
            Case >= 1.0E-24
                TruncatedStr = Round(NumToTruncate * 1.0E+24, 3) & "y"
        End Select
        Return TruncatedStr
    End Function
    Private Function ReturnLinearGridSpacing(ByVal WidthToSpace As Single, HeightToSpace As Single) As Single
        Dim GridLine_Spacing As Single
        If WidthToSpace <> 0 Then
            NumOfX_GridLines = NumOfX_GridLines
            Select Case WidthToSpace
                Case >= 1
                    GridLine_Spacing = Round((WidthToSpace + ScaleX_Left) / NumOfX_GridLines, 1)
                Case <= 0.00000001
                    GridLine_Spacing = Round(WidthToSpace / NumOfX_GridLines, 9)
                Case <= 0.0000001
                    GridLine_Spacing = Round(WidthToSpace / NumOfX_GridLines, 8)
                Case <= 0.000001
                    GridLine_Spacing = Round(WidthToSpace / NumOfX_GridLines, 7)
                Case <= 0.00001
                    GridLine_Spacing = Round(WidthToSpace / NumOfX_GridLines, 6)
                Case <= 0.0001
                    GridLine_Spacing = Round(WidthToSpace / NumOfX_GridLines, 5)
                Case <= 0.001
                    GridLine_Spacing = Round(WidthToSpace / NumOfX_GridLines, 4)
                Case <= 0.01
                    GridLine_Spacing = Round(WidthToSpace / NumOfX_GridLines, 3)
                Case <= 0.1
                    GridLine_Spacing = Round(WidthToSpace / NumOfX_GridLines, 2)
                Case <= 1
                    GridLine_Spacing = Round(WidthToSpace / NumOfX_GridLines, 1)
                Case Else
                    Exit Select
            End Select
        End If
        If HeightToSpace <> 0 Then
            NumOfY_GridLines = NumOfY_GridLines
            Select Case WidthToSpace
                Case >= 1
                    GridLine_Spacing = Round(HeightToSpace / NumOfY_GridLines, 1)
                Case <= 0.00000001
                    GridLine_Spacing = Round(HeightToSpace / NumOfY_GridLines, 9)
                Case <= 0.0000001
                    GridLine_Spacing = Round(HeightToSpace / NumOfY_GridLines, 8)
                Case <= 0.000001
                    GridLine_Spacing = Round(HeightToSpace / NumOfY_GridLines, 7)
                Case <= 0.00001
                    GridLine_Spacing = Round(HeightToSpace / NumOfY_GridLines, 6)
                Case <= 0.0001
                    GridLine_Spacing = Round(HeightToSpace / NumOfY_GridLines, 5)
                Case <= 0.001
                    GridLine_Spacing = Round(HeightToSpace / NumOfY_GridLines, 4)
                Case <= 0.01
                    GridLine_Spacing = Round(HeightToSpace / NumOfY_GridLines, 3)
                Case <= 0.1
                    GridLine_Spacing = Round(HeightToSpace / NumOfY_GridLines, 2)
                Case <= 1
                    GridLine_Spacing = Round(HeightToSpace / NumOfY_GridLines, 1)
                Case Else
                    Exit Select
            End Select
        End If
        Return GridLine_Spacing
    End Function
    Private Sub PrintDocument1_PrintPage(sender As Object, e As PrintPageEventArgs) Handles PrintDocument1.PrintPage
        'Create A Bitmap Sized To The Form
        Using MyImage As Bitmap = New Bitmap(Width, Height)
            'Draw The Bitmap To The Form
            Dim rect As New Rectangle(0, 0, Width, Height)
            DrawToBitmap(MyImage, rect)
            'Fit Bitmap To Page And Center
            Dim mLeft, mTop, mWidth, mHeight As Integer
            Dim ratio As Single = MyImage.Width / MyImage.Height
            If ratio > e.MarginBounds.Width / e.MarginBounds.Height Then
                mWidth = e.MarginBounds.Width
                mHeight = CInt(mWidth / ratio)
                mTop = CInt(e.MarginBounds.Top + (e.MarginBounds.Height / 2) - (mHeight / 2))
                mLeft = e.MarginBounds.Left
            Else
                mHeight = e.MarginBounds.Height
                mWidth = CInt(mHeight * ratio)
                mLeft = CInt(e.MarginBounds.Left + (e.MarginBounds.Width / 2) - (mWidth / 2))
                mTop = e.MarginBounds.Top
            End If
            If PrintPreview Then 'Position PrintPreviewDialog1.Location
                Dim DialogSize = PrintPreviewDialog1.PrintPreviewControl.Size
                Dim CenterscreenWidth As Integer = Screen.PrimaryScreen.Bounds.Width / 2
                Dim CenterscreenHeight As Integer = Screen.PrimaryScreen.Bounds.Height / 2
                PrintPreviewDialog1.Location = New Point(CenterscreenWidth - 0.5 * DialogSize.Width, CenterscreenHeight - DialogSize.Height)
                PrintPreviewDialog1.Dock = DockStyle.Fill
            End If
            'Draw The Form Image On The Printer Graphics
            e.Graphics.DrawImage(MyImage, mLeft, mTop, mWidth, mHeight)
            MyImage.Dispose()
        End Using
        PrintPreview = False
    End Sub
    Private Sub PreviewAndPrintDefaultPrinterToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles PreviewAndPrintDefaultPrinterToolStripMenuItem.Click
        'Show Print Preview Dialog
        PrintPreview = True
        PrintDialog1.Document = PrintDocument1 'PrintDialog associate with PrintDocument.
        Try
            PrintPreviewDialog1.ShowDialog()
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
    End Sub
    Private Sub SelectPrinterAndPrintToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles SelectPrinterAndPrintToolStripMenuItem.Click
        PrintPreview = False
        PrintDialog1.Document = PrintDocument1 'PrintDialog associate with PrintDocument.
        If PrintDialog1.ShowDialog() = DialogResult.OK Then
            PrintDocument1.Print()
        End If
    End Sub
    Private Sub SaveGraphOnlyToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles SaveGraphOnlyToolStripMenuItem.Click
        Dim tgFolder As String = "\TestGraph Documents"
        Dim tgPath As String = System.Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments)
        Dim tgCompletePath As String = tgPath & tgFolder
        Dim BorderWidth As Integer = SystemInformation.FrameBorderSize.Width
        Dim TitlebarHeight As Integer = SystemInformation.CaptionHeight + BorderWidth
        Dim frmleft As Point = Parent.Bounds.Location
        Dim MyImage As New Bitmap(Bounds.Width, Bounds.Height)
        Dim Mygraph = Graphics.FromImage(MyImage)
        Dim screenx As Integer = frmleft.X + Left + BorderWidth * 2
        Dim screeny As Integer = frmleft.Y + Top + TitlebarHeight
        Mygraph.CopyFromScreen(screenx, screeny, 0, 0, MyImage.Size)
        Dim saveFileDialog1 As New SaveFileDialog
        With saveFileDialog1
            .InitialDirectory = tgCompletePath
            .Title = "Save As .jpg File"
            .CheckFileExists = False
            .CheckPathExists = True
            .DefaultExt = "jpg"
            .Filter = "JPEG File | *.jpg"
            .FilterIndex = 2
            .RestoreDirectory = True
        End With
        Try
            If (saveFileDialog1.ShowDialog() = DialogResult.OK) Then
                MyImage.Save(saveFileDialog1.FileName, Imaging.ImageFormat.Jpeg)
            End If
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
        MyImage.Dispose()
        Mygraph.Dispose()
        saveFileDialog1.Dispose()
    End Sub
    Private Sub SaveGraphWithParentFormToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles SaveGraphWithParentFormToolStripMenuItem.Click
        Dim tgFolder As String = "\TestGraph Documents"
        Dim tgPath As String = System.Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments)
        Dim tgCompletePath As String = tgPath & tgFolder
        Dim BorderWidth As Integer = SystemInformation.FrameBorderSize.Width
        Dim TitlebarHeight As Integer = SystemInformation.CaptionHeight + BorderWidth
        Dim frmleft As Point = Parent.Bounds.Location
        Dim MyImage As New Bitmap(Parent.Bounds.Width - 2 * BorderWidth, Parent.Bounds.Height - BorderWidth)
        Dim Mygraph = Graphics.FromImage(MyImage)
        Dim screenx As Integer = frmleft.X + BorderWidth
        Dim screeny As Integer = frmleft.Y
        Mygraph.CopyFromScreen(screenx, screeny, 0, 0, MyImage.Size)
        Dim saveFileDialog1 As New SaveFileDialog
        With saveFileDialog1
            .InitialDirectory = tgCompletePath
            .Title = "Save As .jpg File"
            .CheckFileExists = False
            .CheckPathExists = True
            .DefaultExt = "jpg"
            .Filter = "JPEG File | *.jpg"
            .FilterIndex = 2
            .RestoreDirectory = True
        End With
        Try
            If (saveFileDialog1.ShowDialog() = DialogResult.OK) Then
                MyImage.Save(saveFileDialog1.FileName, Imaging.ImageFormat.Jpeg)
            End If
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
        MyImage.Dispose()
        Mygraph.Dispose()
        saveFileDialog1.Dispose()
    End Sub
    Private Sub WriteGraphData()
        Dim GraphStyleText As String = ""
        Dim XAxisStyleText As String = ""
        Dim YAxisStyleText As String = ""
        Select Case Graph_Style
            Case = 0
                GraphStyleText = "Point To Point"
            Case = 1
                GraphStyleText = "Bar"
            Case = 2
                GraphStyleText = "Pie Chart"
        End Select
        Select Case XAxisStyle
            Case = 0
                XAxisStyleText = "Linear"
            Case = 2
                XAxisStyleText = "Log(2)"
            Case = Math.E
                XAxisStyleText = "Log(e)"
            Case = 10
                XAxisStyleText = "Log(10)"
            Case = 20
                XAxisStyleText = "Log(20)"
        End Select
        Select Case YAxisStyle
            Case = 0
                YAxisStyleText = "Linear"
            Case = 2
                YAxisStyleText = "Log(2)"
            Case = Math.E
                YAxisStyleText = "Log(e)"
            Case = 10
                YAxisStyleText = "Log(10)"
            Case = 20
                YAxisStyleText = "Log(20)"
        End Select
        'Create The Folder To Place All TestGraph JPEG And Data Files
        Dim tgFolder As String = "\TestGraph Documents"
        Dim tgPath As String = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments)
        If (Not Directory.Exists(tgPath & tgFolder)) Then
            My.Computer.FileSystem.CreateDirectory(tgPath & tgFolder)
        End If
        Dim tgFileName As String = tgPath & tgFolder & "\TestGraph Data.txt"
        Dim tgFile As New FileInfo(tgFileName)
        'If File Exist Then Delete Old File And Create New.
        If tgFile.Exists Then tgFile.Delete()
        'If Old File Deleted Then Create New File
        Using tgWriter As StreamWriter = tgFile.CreateText()
            tgWriter.WriteLine("   File Created: {0}", DateTime.Now.ToString())
            tgWriter.WriteLine("   Location: " & tgFileName)
            tgWriter.WriteLine("   User: " & SystemInformation.UserName)
            tgWriter.WriteLine("   TestGraph Header: " & Header_Text)
            tgWriter.WriteLine("   X-Axis Caption: " & XAxis_Text)
            tgWriter.WriteLine("   Y-Axis Caption: " & YAxis_Text)
            tgWriter.WriteLine("   TestGraph Style: " & GraphStyleText)
            tgWriter.WriteLine("   X-Axis Style: " & XAxisStyleText)
            tgWriter.WriteLine("   Y-Axis Style: " & YAxisStyleText)
            tgWriter.WriteLine("   *******Graph Data*******")
            'Write The Data To The Text File
            If Graph_Style = GraphStyleChoices.Pie Then
                For Index = 1 To PieIndex
                    tgWriter.WriteLine("   " & New_PieLabels(Index))
                Next
            Else
                For Index = 1 To DrawLine_ArrayIndex
                    tgWriter.WriteLine("   X = " & New_DrawLineX1(Index) & ", Y = " & New_DrawLineY1(Index))
                    If Index = DrawLine_ArrayIndex Then
                        tgWriter.WriteLine("   X = " & New_DrawLineX2(Index) & ", Y = " & New_DrawLineY2(Index))
                    End If
                Next
            End If
            tgWriter.Close()
            tgWriter.Dispose()
        End Using
    End Sub
    Private Sub SaveGraphDataToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles SaveGraphDataToolStripMenuItem.Click
        'Create The Folder To Place All TestGraph JPEG And Data Files
        Dim tgPath As String = System.Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments)
        Dim tgFolder As String = "\TestGraph Documents"
        Dim tgFileName As String = "\TestGraph Data.txt"
        'If Folder Doesn't Exist Then Create
        If (Not Directory.Exists(tgPath & tgFolder)) Then
            My.Computer.FileSystem.CreateDirectory(tgPath & tgFolder)
        End If
        Dim tgCompletePath As String = tgPath & tgFolder & tgFileName
        'Create A File Path Wrapper
        Dim tgFile As New FileInfo(tgCompletePath)
        'If File Exist Then Open File
        If File.Exists(tgCompletePath) = True Then
            Using Process As New Process
                Process.Start(tgCompletePath, tgFileName)
            End Using
        Else
            MsgBox(tgCompletePath & " File Does Not Exist", 1)
        End If
    End Sub
    Private Sub AboutToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles AboutToolStripMenuItem.Click
        MessageBox.Show("Created By Ross Waters (RossWatersjr@gmail.com)" & vbCrLf & "Created In VB.Net Using Visual Studio 2022 Version 17.1.1" & vbCrLf & ".NET Framework 4.8" & vbCrLf & "Last Revision Date 03/21,2022" & vbCrLf & "Revision 1.6.0", "About", MessageBoxButtons.OK, MessageBoxIcon.Information)
    End Sub
    Private Sub CloseApplicationToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles CloseApplicationToolStripMenuItem.Click
        Dispose()
        Application.Exit()
    End Sub
    Private Sub ClearGraphToolStripMenuItem1_Click(sender As Object, e As EventArgs) Handles ClearGraphToolStripMenuItem1.Click
        ClearGraphics()
    End Sub
    Private Sub LineGraphToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles LineGraphToolStripMenuItem.Click
        ClearGraphics()
        GraphStyle = GraphStyleChoices.PointToPoint
        'Control Font Used For HeaderText, XAxisText And YAxisText
        Font = New Font("Times New Roman", 20, FontStyle.Bold Or FontStyle.Italic)
        HeaderText = "Log(10) X-Axis And Log(10) Y-Axis Graph Example"
        XAxisText = "Frequency (Hz)"
        YAxisText = "Amplitude (dB)"
        SetViewportScale(20, 20000, TestGraph.ScaleStyleChoices.Log10, 20, 1, TestGraph.ScaleStyleChoices.Log10)
        ShowMousePosition = True
        GraphBackColor = Color.LightGray
        GraphForeColor = Color.DarkRed
        GraphFillColor = Color.Black
        GridLineColor = Color.DimGray
        ShowXLabels = True
        ShowXGridLines = True
        RotateXGridLabels = 0
        ShowCustomXLabels = False
        '*******Y=Axis*******
        'NumOfYGridLines Is Determined Automatically In Log Function
        ShowYGridLines = True
        ShowYLabels = True
        RotateYGridLabels = 0
        ShowCustomYLabels = False
        'Set Font And Color For Grid Line Labeling
        GridLabelColor = Color.Black
        GridLabelFont = New Font("Times New Roman", 9, FontStyle.Regular)
        'Generate 100 Sequencially Random Log Test Pattern Frequencies
        Dim Frequency() As Single = GenerateLogTestPoints(XScaleStyle, 20, 20000, 100)
        Dim Amplitude(Frequency.Length) As Single
        Dim Pt As Integer
        'Generate 100 Random Log amplitudes Between 1.3 To 17
        For Pt = 0 To UBound(Amplitude, 1)
            Randomize()
            Amplitude(Pt) = Floor((17 - 1.3 + 1) * Rnd()) + 1.3
        Next
        Dim LinePen As New Pen(Color.Aqua, 0.1)
        For Pt = 0 To UBound(Frequency) - 1
            If Pt > 0 Then DrawLine(LinePen, Frequency(Pt - 1), Amplitude(Pt - 1), Frequency(Pt), Amplitude(Pt))
            Refresh()
        Next
        LinePen.Dispose()
    End Sub
    Private Sub RestoreToDefaultStateToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles RestoreToDefaultStateToolStripMenuItem.Click
        ResetGraph()
    End Sub
    Private Sub BarGraph1ToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles BarGraph1ToolStripMenuItem.Click
        ClearGraphics()
        ShowMousePosition = False
        ShowXGridLines = True
        ShowYGridLines = True
        ShowYLabels = True
        RotateXGridLabels = -15
        RotateYGridLabels = -45
        'Control Font Used For HeaderText, XAxisText And YAxisText
        Font = New Font("Times New Roman", 18, FontStyle.Bold Or FontStyle.Italic)
        HeaderText = "Linear Bar Graph With Random Hatch Styles And Custom Labeling"
        XAxisText = "Months Of 2021"
        YAxisText = "Sales Projection"
        'Imuns Must Be Referenced Using Control Name 
        GraphStyle = TestGraph.GraphStyleChoices.Bar
        GraphBackColor = Color.DimGray
        GraphForeColor = Color.Khaki
        GraphFillColor = Color.Black
        SetViewportScale(0, 12, TestGraph.ScaleStyleChoices.Linear, 30, 0, TestGraph.ScaleStyleChoices.Linear)
        '******How To Display Custom Labels Along X Grid*******
        'XScaleStyle Has To Be Set To Linear
        'This Method Is Not Available For Log Functions
        NumOfXGridLines = 12
        ShowCustomXLabels = True
        'Index Custom Labels Array With NumOfXGridLines
        Dim ArrayOfLabels(NumOfXGridLines)
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
        DrawCustomXGrid_Labels(ArrayOfLabels)
        'Next Do The Same For Custom Y-Axis Grid Labels
        ShowCustomYLabels = True
        NumOfYGridLines = 6
        'Index Custom Labels Array With NumOfYGridLines
        ReDim ArrayOfLabels(0)
        ReDim Preserve ArrayOfLabels(NumOfYGridLines)
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
        DrawCustomYGrid_Labels(ArrayOfLabels)
        '**************************************************************
        'Populate GridLabelFont With Font And Color For Grid Line Labeling
        GridLabelColor = Color.Aqua
        GridLabelFont = New Font("Times New Roman", 9, FontStyle.Regular)
        'Create New Pen For Grid Lines
        GridLineColor = Color.Gray
        'Also Use GridLine Color FOR Line Data Color
        Dim GridLinePen As New Pen(GridLineColor, 0.1)
        Dim Month As Integer
        'Create The Random Bar Patterns And Lable Each With Month Data
        ' HatchStyles = Ramdon
        RandomHatchStyles = True
        For Month = 0 To 11 Step 1
            Dim Y1 As Integer = CInt(Math.Floor((29 - 1 + 1) * Rnd())) + 1
            Dim Y2 As Integer = Y1
            DrawLine(GridLinePen, Month, Y1, Month + 1, Y2)
        Next
        GridLinePen.Dispose()
    End Sub
    Private Sub BarGraph2ToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles BarGraph2ToolStripMenuItem.Click
        ClearGraphics()
        'Control Font Used For HeaderText, XAxisText And YAxisText
        Font = New Font("Times New Roman", 22, FontStyle.Bold Or FontStyle.Italic)
        HeaderText = " X Axis Log(10) Bar Graph With HatchStyle = Wave"
        XAxisText = "Frequency (Hz)"
        YAxisText = "Amplitude (dB)"
        SetViewportScale(20, 20000, TestGraph.ScaleStyleChoices.Log10, 10, -10, TestGraph.ScaleStyleChoices.Linear)
        GraphBackColor = Color.LightGray
        GraphForeColor = Color.DarkRed
        GraphFillColor = Color.Black
        GridLineColor = Color.DimGray
        NumOfYGridLines = 10
        ShowMousePosition = True
        ShowXLabels = True
        ShowYLabels = True
        ShowXGridLines = True
        ShowYGridLines = True
        RotateXGridLabels = 0
        RotateYGridLabels = 0
        ShowCustomXLabels = False
        ShowCustomYLabels = False
        'Populate GridLabelFont With Font And Color For Grid Line Labeling
        GridLabelColor = Color.Black
        GridLabelFont = New Font("Times New Roman", 9, FontStyle.Regular)
        'Set Color For Grid Lines
        GridLineColor = Color.LightGray
        'Set The Graph To Display Bar Style With HatchStyle
        GraphStyle = GraphStyleChoices.Bar
        RandomHatchStyles = False
        Dim hBrush As New HatchBrush(HatchStyle.Wave, Color.Aqua, Color.White)
        'Generate 100 Log Test Pattern Frequencies
        Dim Frequency() As Single = GenerateLogTestPoints(XScaleStyle, 20, 20000, 100)
        Dim Amplitude(UBound(Frequency)) As Single
        Dim Pt As Integer
        'Generate 100 Random amplitudes Between @ -10 To 10
        For Pt = 0 To UBound(Amplitude, 1)
            Randomize()
            Amplitude(Pt) = Math.Floor((10 - -10 + 1) * Rnd()) + -10
        Next
        Dim LinePen As New Pen(Color.Aqua, 0.1)
        For Pt = 0 To UBound(Frequency) - 1
            If Pt > 0 Then DrawLine(LinePen, Frequency(Pt - 1), Amplitude(Pt - 1), Frequency(Pt), Amplitude(Pt), hBrush)
        Next
        hBrush.Dispose()
        LinePen.Dispose()
    End Sub
    Private Sub RefreshGraphToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles RefreshGraphToolStripMenuItem.Click
        Refresh()
    End Sub
    Private Sub PieChartToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles PieChartToolStripMenuItem.Click
        ClearGraphics()
        'All Graph Properties Are Automatically Setup For This Method.
        'Graph Options Available Are All Color Options, Font Family And Style, HeaderText And XAxisText.
        GraphFillColor = Color.DimGray
        GraphForeColor = Color.Black 'Sets The HeaderText And XAxisText Forecolors  
        GraphBackColor = Color.LightGray
        GraphStyle = GraphStyleChoices.Pie
        'Control Font Used For HeaderText, XAxisText And YAxisText
        Font = New Font("Times New Roman", 22, FontStyle.Bold Or FontStyle.Italic)
        HeaderText = "Pie Chart Example"
        XAxisText = "My Company Output / Countries"
        YAxisText = ""
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
            DrawPieChart(PieBrush(Index), StringFont, PiePercent(Index), PieLabel(Index))
            RunningTotal += PiePercent(Index)
            'Summation Must Not Be > 100%
            If RunningTotal >= 100 Then Exit For
        Next
        StringFont.Dispose()
        Dim br As Brush
        For Each br In PieBrush
            br.Dispose()
        Next
    End Sub
    Private Sub TextExampleToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles TextExampleToolStripMenuItem.Click
        ClearGraphics()
        'Control Font Used For HeaderText, XAxisText And YAxisText
        Font = New Font("Times New Roman", 22, FontStyle.Bold Or FontStyle.Italic)
        HeaderText = "DrawString Example With Different StringFormats And Rotated Text "
        XAxisText = "X-Axis Text"
        YAxisText = "Y-Axis Text"
        SetViewportScale(0, 50, TestGraph.ScaleStyleChoices.Linear, 50, 0, TestGraph.ScaleStyleChoices.Linear)
        GraphStyle = GraphStyleChoices.PointToPoint
        GraphBackColor = Color.LightGray
        GraphForeColor = Color.DarkRed
        GraphFillColor = Color.Black
        GridLineColor = Color.DimGray
        ShowXGridLines = True
        NumOfXGridLines = 10
        ShowXLabels = True
        ShowYGridLines = True
        NumOfYGridLines = 5
        ShowYLabels = True
        ShowMousePosition = True
        RotateXGridLabels = 0
        RotateYGridLabels = 0
        ShowCustomXLabels = False
        ShowCustomYLabels = False
        RandomHatchStyles = False
        'Populate GridLabelFont With Font And Color For Grid Line Labeling
        GridLabelColor = Color.Black
        GridLabelFont = New Font("Times New Roman", 9, FontStyle.Regular)
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
        DrawString("StringFormat = Center/Center/NoWrap (X = 25,Y = 25), Font Size = 14, Rotation = 22.5 Degrees", 22.5, Fonts(0), Brushes(0), 25, 25, Formats(0))
        '*******************************************************************
        Fonts(1) = New Font("Times New Roman", 12, FontStyle.Bold)
        Brushes(1) = New SolidBrush(Color.LimeGreen)
        Formats(1) = New StringFormat
        With Formats(1)
            .LineAlignment = StringAlignment.Center
            .Alignment = StringAlignment.Near
            .FormatFlags = StringFormatFlags.NoWrap
        End With
        DrawString("StringFormat = Center/Near/NoWrap (X = 0,Y = 10), Font Size = 12, Rotation = 0 Degrees", 0, Fonts(1), Brushes(1), 0, 10, Formats(1))
        '*******************************************************************
        Fonts(2) = New Font("Times New Roman", 12, FontStyle.Bold)
        Brushes(2) = New SolidBrush(Color.Yellow)
        Formats(2) = New StringFormat
        With Formats(2)
            .LineAlignment = StringAlignment.Center
            .Alignment = StringAlignment.Far
            .FormatFlags = StringFormatFlags.NoWrap
        End With
        DrawString("StringFormat = Center/Far/NoWrap (X = 50,Y = 40), Font Size = 12, Rotation = 0 Degrees", 0, Fonts(2), Brushes(2), 50, 40, Formats(2))
        '*******************************************************************
        Fonts(3) = New Font("Times New Roman", 9, FontStyle.Bold)
        Brushes(3) = New SolidBrush(Color.Aqua)
        Formats(3) = New StringFormat
        With Formats(3)
            .LineAlignment = StringAlignment.Center
            .Alignment = StringAlignment.Far
            .FormatFlags = StringFormatFlags.NoWrap
        End With
        DrawString("X = 7, Y = 40, Rotation = -65 Degrees", -65, Fonts(3), Brushes(3), 7, 40, Formats(3))
        '*******************************************************************
        Fonts(4) = New Font("Times New Roman", 9, FontStyle.Bold)
        Brushes(4) = New SolidBrush(Color.White)
        Formats(4) = New StringFormat
        With Formats(4)
            .LineAlignment = StringAlignment.Center
            .Alignment = StringAlignment.Far
            .FormatFlags = StringFormatFlags.NoWrap
        End With
        DrawString("X = 50, Y = 39, Rotation = -65 Degrees", -65, Fonts(4), Brushes(4), 50, 39, Formats(4))
        '*******************************************************************
        For d = 0 To Brushes.Length - 1
            Fonts(d).Dispose()
            Brushes(d).Dispose()
            Formats(d).Dispose()
        Next
    End Sub
    Private Sub TestGraph_Resize(sender As Object, e As EventArgs) Handles Me.Resize
        lblHeader.Location = New Point((ClientSize.Width - lblHeader.Width) / 2, 0)
        lblXAxis.Location = New Point((ClientSize.Width - lblXAxis.Width) / 2, ClientSize.Height - lblXAxis.Height)
        picYAxis.Width = lblXAxis.Height
        picYAxis.Height = ClientSize.Height * (ViewportHeightPercent / 100)
        If ScaleX_Left < ScaleX_Right Then
            picYAxis.Left = ClientSize.Width - picYAxis.Width
        Else
            picYAxis.Left = Left
        End If
        picYAxis.Location = New Point((picYAxis.Left), ((ClientSize.Height) - picYAxis.Height) - (BottomPercent / 100) * ClientSize.Height)
        Refresh()
    End Sub
    Private Sub LineGraph2ToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles LineGraph2ToolStripMenuItem.Click
        ClearGraphics()
        SetViewportScale(100, 0, TestGraph.ScaleStyleChoices.Linear, 50, 0, TestGraph.ScaleStyleChoices.Linear)
        GraphStyle = TestGraph.GraphStyleChoices.PointToPoint
        RandomHatchStyles = True
        'Control Font Used For HeaderText, XAxisText And YAxisText
        Font = New Font("Times New Roman", 22, FontStyle.Bold Or FontStyle.Italic)
        HeaderText = "Point To Point Graph Style Example"
        XAxisText = "Graph X-Axis"
        YAxisText = "Graph Y-Axis"
        ShowMousePosition = True
        GraphBackColor = Color.LightGray
        GraphForeColor = Color.DarkRed
        GraphFillColor = Color.Black
        GridLineColor = Color.DimGray
        NumOfXGridLines = 10
        '*******X=Axis*******
        ShowXLabels = True
        ShowXGridLines = True
        RotateXGridLabels = 0
        ShowCustomXLabels = False
        '*******Y=Axis*******
        ShowYGridLines = True
        ShowYLabels = True
        RotateYGridLabels = 0
        ShowCustomYLabels = False
        NumOfYGridLines = 10
        'Populate GridLabelFont With Font And Color For Grid Line Labeling
        GridLabelColor = Color.Black
        GridLabelFont = New Font("Times New Roman", 9, FontStyle.Regular)
        Dim Dash_Pattern1 As Single() = {3, 2, 3, 2}
        Dim DrawPen1 As New Pen(Color.Red, 0.1)
        With DrawPen1
            .Width = 1
            .DashCap = Drawing2D.DashCap.Triangle
            .DashPattern = Dash_Pattern1
        End With
        DrawLine(DrawPen1, 0, 0, 50, 25)
        '**************************************************
        Dim Dash_Pattern2 As Single() = {4, 2, 4, 2}
        Dim DrawPen2 As New Pen(Color.Blue, 0.1)
        With DrawPen2
            .Width = 1.5
            .DashCap = Drawing2D.DashCap.Round
            .DashPattern = Dash_Pattern2
        End With
        DrawLine(DrawPen2, 50, 25, 100, 50)
        '**************************************************
        Dim Dash_Pattern3 As Single() = {3, 1, 3, 1}
        Dim DrawPen3 As New Pen(Color.Aqua, 0.1)
        With DrawPen3
            .Width = 2
            .DashCap = Drawing2D.DashCap.Triangle
            .DashPattern = Dash_Pattern3
        End With
        DrawLine(DrawPen3, 0, 50, 50, 25)
        '**************************************************
        Dim Dash_Pattern4 As Single() = {2, 1, 2, 1}
        Dim DrawPen4 As New Pen(Color.Gold, 0.1)
        With DrawPen4
            .Width = 2.5
            .DashCap = Drawing2D.DashCap.Flat
            .DashPattern = Dash_Pattern4
        End With
        DrawLine(DrawPen4, 50, 25, 100, 0)
        'Do Not Dispose Of Brushes Or Pens Before Refresh().
        DrawPen1.Dispose()
        DrawPen2.Dispose()
        DrawPen3.Dispose()
        DrawPen4.Dispose()
    End Sub
End Class
