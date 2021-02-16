Imports System.Net
Imports System.Net.Sockets

Public Class TaskManager
    Public hotkey As String
    Private Declare Function GetAsyncKeyState Lib "user32" (ByVal vKey As Int32) As Short
    Public ignorekey As Boolean = False
    Public Delegate Sub addlistview(ByVal item As ListViewItem)

    Private Sub Timer1_Tick(sender As Object, e As EventArgs) Handles Timer1.Tick
        If Not ignorekey = True Then
            Select Case currentmode
                Case 1
                    ' My.Computer.Audio.PlaySystemSound(Media.SystemSounds.Beep)
                    If GetAsyncKeyState(_A) Then
                        killall()
                    End If
                Case 2
                    '   My.Computer.Audio.PlaySystemSound(Media.SystemSounds.Beep)
                    If GetAsyncKeyState(_A) And GetAsyncKeyState(_B) Then
                        killall()
                    End If
                Case 3
                    If GetAsyncKeyState(_A) And GetAsyncKeyState(_B) And GetAsyncKeyState(_C) Then
                        killall()
                    End If
                Case 4
                    If GetAsyncKeyState(_A) And GetAsyncKeyState(_B) And GetAsyncKeyState(_C) And GetAsyncKeyState(_D) Then
                        killall()
                    End If
            End Select
        End If
    End Sub
    Private Sub killall()
        For Each item In ListBox1.Items
            If Not item = "" Then
                For Each OneProcess As Process In Process.GetProcesses

                    If item = OneProcess.MainWindowTitle Then
                        OneProcess.Kill()
                    End If
                Next
            End If
        Next
        For Each item In ListBox3.Items
            If Not item = "" Then
                For Each OneProcess As Process In Process.GetProcesses
                    If item = OneProcess.ProcessName Then
                        OneProcess.Kill()
                    End If
                Next
            End If
        Next
    End Sub

    Private Sub Button1_PreviewKeyDown(sender As Object, e As PreviewKeyDownEventArgs) Handles Button1.PreviewKeyDown
    End Sub
    Dim currentmode As Integer
    Dim _A As Integer
    Dim _B As Integer
    Dim _C As Integer
    Dim _D As Integer
    Public Sub setnewkey()
        Try
            Dim splitterstring() As String = Button1.Text.ToString.Replace(" ", "").Split("+")
            currentmode = splitterstring.Count
            Select Case currentmode
                Case 1
                    _A = GetKeyCode1(splitterstring(0))
                Case 2
                    _A = GetKeyCode1(splitterstring(0))
                    _B = GetKeyCode1(splitterstring(1))
                Case 3
                    _A = GetKeyCode1(splitterstring(0))
                    _B = GetKeyCode1(splitterstring(1))
                    _C = GetKeyCode1(splitterstring(2))
                Case 4
                    _A = GetKeyCode1(splitterstring(0))
                    _B = GetKeyCode1(splitterstring(1))
                    _C = GetKeyCode1(splitterstring(2))
                    _D = GetKeyCode1(splitterstring(4))
            End Select
            ignorekey = False
        Catch ex As Exception

        End Try
    End Sub

    Private Function GetKeyCode1(ByVal S As String) As Keys
        Dim kc As KeysConverter = New KeysConverter()
        Try
            Return CType(kc.ConvertFromString(S), Keys)
        Catch
            Return Nothing
        End Try
    End Function

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        ignorekey = True
        HotKeyHandler.LoadHotKeyHandler(sender)
    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        Dim justinput As String = InputBox("What is the process name you would like to add. To find out a processes' name click the 'List all processes' button and copy & paste")
        If Not justinput = "" Then
            ListBox1.Items.Add(justinput)
        End If
    End Sub

    Private Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click
        Try
            ListBox1.Items.Remove(ListBox1.SelectedItem)
        Catch ex As Exception

        End Try
    End Sub

    Private Sub Button5_Click(sender As Object, e As EventArgs) Handles Button5.Click
        Try
            My.Computer.Clipboard.SetText(ListView1.SelectedItems(0).Text)
        Catch ex As Exception

        End Try
    End Sub

    Private Sub Form1_FormClosing(sender As Object, e As FormClosingEventArgs) Handles MyBase.FormClosing
        My.Settings.HotKey = Button1.Text
        My.Settings.HotKeyNumber = hotkey
        My.Settings.ListOfProcesses = ""
        For Each item In ListBox1.Items
            My.Settings.ListOfProcesses += item.ToString + Environment.NewLine
        Next
        My.Settings.ListOfProcID = ""
        For Each item In ListBox3.Items
            My.Settings.ListOfProcID += item.ToString + Environment.NewLine
        Next

    End Sub

    Private Sub Form1_Shown(sender As Object, e As EventArgs) Handles MyBase.Shown
        Button1.Text = My.Settings.HotKey
        hotkey = My.Settings.HotKeyNumber
        Dim splitterstring() As String = My.Settings.ListOfProcesses.Split(Environment.NewLine)
        For Each item In splitterstring
            If Not item.ToString.Replace(Chr(10), "") = "" Then
                ListBox1.Items.Add(item.ToString.Replace(Chr(10), ""))
            End If
        Next
        splitterstring = My.Settings.ListOfProcID.Split(Environment.NewLine)
        For Each item In splitterstring
            If Not item.ToString.Replace(Chr(10), "") = "" Then
                ListBox3.Items.Add(item.ToString.Replace(Chr(10), ""))
            End If
        Next
        setnewkey()
    End Sub
    Private Sub runtask()

        For Each OneProcess As Process In Process.GetProcesses
            If Not OneProcess.MainWindowTitle = "" Then
                Dim newitem As ListViewItem = New ListViewItem(OneProcess.MainWindowTitle)
                Try
                    ImageList1.Images.Add(OneProcess.MainModule.FileName, Icon.ExtractAssociatedIcon(OneProcess.MainModule.FileName).ToBitmap)
                Catch ex As Exception

                End Try

                Try
                    newitem.ImageIndex = ImageList1.Images.IndexOfKey(OneProcess.MainModule.FileName)
                Catch ex As Exception

                End Try
                Me.Invoke(New addlistview(AddressOf alvi1), newitem)
            End If
            If Not OneProcess.ProcessName = "" Then
                Dim newitem As ListViewItem = New ListViewItem(OneProcess.ProcessName)
                Try
                    ImageList1.Images.Add(OneProcess.MainModule.FileName, Icon.ExtractAssociatedIcon(OneProcess.MainModule.FileName).ToBitmap)
                Catch ex As Exception

                End Try

                Try
                    newitem.ImageIndex = ImageList1.Images.IndexOfKey(OneProcess.MainModule.FileName)
                Catch ex As Exception

                End Try
                Me.Invoke(New addlistview(AddressOf alvi2), newitem)
            End If

        Next
    End Sub
    Private Sub alvi1(ByVal item As ListViewItem)
        ListView1.Items.Add(item)
    End Sub
    Private Sub alvi2(ByVal item As ListViewItem)
        ListView2.Items.Add(item)
    End Sub
    Private Sub Button4_Click(sender As Object, e As EventArgs) Handles Button4.Click
        ListView1.Items.Clear()
        ListView2.Items.Clear()
        Dim thread As New Threading.Thread(AddressOf runtask)
        thread.Start()
    End Sub

    Private Sub Button7_Click(sender As Object, e As EventArgs) Handles Button7.Click
        Dim justinput As String = InputBox("What is the process name you would like to add. To find out a processes' name click the 'List all processes' button and copy & paste.")
        If Not justinput = "" Then
            ListBox3.Items.Add(justinput)
        End If
    End Sub

    Private Sub Button6_Click(sender As Object, e As EventArgs) Handles Button6.Click
        Try
            ListBox3.Items.Remove(ListBox3.SelectedItem)
        Catch ex As Exception

        End Try
    End Sub

    Private Sub Button8_Click(sender As Object, e As EventArgs) Handles Button8.Click
        Try
            My.Computer.Clipboard.SetText(ListView2.SelectedItems(0).Text)
        Catch ex As Exception

        End Try
    End Sub

    Private Sub Button10_Click(sender As Object, e As EventArgs) Handles Button10.Click
        ListBox1.Items.Add(ListView1.SelectedItems(0).Text)
    End Sub

    Private Sub Button11_Click(sender As Object, e As EventArgs) Handles Button11.Click
        ListBox3.Items.Add(ListView2.SelectedItems(0).Text)
    End Sub

    Private Sub Button12_Click(sender As Object, e As EventArgs) Handles Button12.Click
        MsgBox("break")
    End Sub

    Private Sub Button13_Click(sender As Object, e As EventArgs) Handles Button13.Click
        For Each OneProcess As Process In Process.GetProcesses
            If ListView1.SelectedItems(0).Text = OneProcess.MainWindowTitle Then
                OneProcess.Kill()
            End If

        Next
        Button4.PerformClick()
    End Sub

    Private Sub Button14_Click(sender As Object, e As EventArgs) Handles Button14.Click
        For Each OneProcess As Process In Process.GetProcesses
            If ListView2.SelectedItems(0).Text = OneProcess.ProcessName Then
                OneProcess.Kill()
            End If
        Next
        Button4.PerformClick()
    End Sub
End Class
