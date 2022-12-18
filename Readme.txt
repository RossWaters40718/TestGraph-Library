                                        About TestGraph

Parent Form Requirements:
Imports System.Math
Imports System.Drawing.Drawing2D
Imports TestGraphControl

This Control Was Created In Visual Basic (VB.Net) And Last Tested And Upgraded Using Visual Studio 2022
Community Edition Version 17.4.2 On Windows 11 Operating System. Template: Windows Forms Control Library
(.Net Framework 4.8.00037) For Use With: Windows Forms App (.Net Framework). This Control Has An Interface 
To Access It's Properties And Methods From a Parent Control Which Is Described In Another Text File And Shown
In The Examples.

This Graph Has A MenuStrip Which Becomes Visible With A Right-Click On The Graph During RunTime.
All Graph Raw Data Is Written To A Temporary Text File That Is Overwritten Each Time A New Graph Is Drawn.
Only The Raw Data Associated With The Graph Is Saved Automatically But Is Also Overwritten By The Next 
Graph. The Text File Location Is "C:\Users\UserName\Documents\TestGraph Documents\TestGraph Data.txt". If You
Wish To Preserve This File, Open The File And Save To A Different Name Or Location Before A New Graph Is Drawn. 
This Option Is Available On The MenuStrip. If You Wish To Save Or Print A Graph As A JPEG File, You  Must Do  
So Before A New Graph Is Drawn. Default Directory:  "C:\Users\UserName\Documents\TestGraph Documents\filename".
There Are Several Graph Examples Present On The MenuStrip. The Associated Text Files For These Examples Can 
Be Found In The Same Folder Where This File Is Located.

All Methods And Properties Use Only Custom Scale Coordinances Defined By The SetViewportScale() Method.
This Method Sets Properties ScaleX_Left, ScaleX_Right, ScaleY_Bottom And ScaleY_Top
Which Defines The Drawing Area(Viewport) Of The Control. The Controls Client Area In Custom Units Is
Read Only Properties ControlLeft, ControlRight, ControlTop, ControlBottom, ControlWidth And ControlHeight.
There Is No Need For Any Coordinance Conversions Or Argument Conversions. All Arguments Using Custom
Scale Coordinance Including Log Functions Are Automatically Converted To Proper Format. Simply Use Custom Scale
Coordinances Even For DrawString Method And You Are Good To Go. Just Follow The Examples!

The Popular e.Graphics Is Not Needed For DrawLine Or DrawString Methods Since These Methods Are Calls
To The Original Methods And Almost All Graphics Are Handled By The Controls Paint Event.   
Imports System.Drawing.Drawing2D Is Only Necessary On The Parent Form For Construction Of HatchStyle, DashStyle,
DashCaps, LineCaps, ...etc.  And Passing These Arguments To The These Methods Where They Are Reconstructed To
Be Used By The Original Methods.

About Using Refresh(): Refreshing The Parent Form Also Triggers A Paint Event For The Control. Refresh() May Be
Used Inside A Loop To Paint New Data Points Or outside The Loop At The End Of A Procedure. All Data Points Will
Be Present On The Graph Whichever The Choice. If The Graph Being Drawn Is Super Fast Then You Might Consider 
Using Refresh Only Once At The End Of The Prodecure. However If The Data Is Slow coming Then The Recomendated Method
Is To Use Refresh() Inside The Methods Loop So The Graphs Display Is Current. Bar Graph 2 Example And Line Graph
1 Example Illustrates The Differences Using Refresh() Outside And Inside A Loop Respectively With 100 Test Points.
 


