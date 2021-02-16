Public Class HotKeyHandler
    Private Declare Function GetAsyncKeyState Lib "user32" (ByVal vKey As Int32) As Short
    Private currentlyselected As ListViewItem
    Private returntext As Object
    Public Sub LoadHotKeyHandler(ByVal sender As Object)
        Dim PresetArray As New ArrayList
        Dim splitterstring() As String = My.Settings.Presets.Split(Environment.NewLine)
        For Each item In splitterstring
            If Not item.ToString.Replace(Chr(10), "") = "" Then
                PresetArray.Add(item.ToString.Replace(Chr(10), ""))
            End If
        Next
        For Each item In PresetArray
            Dim newitem As ListViewItem = New ListViewItem(item.ToString.Remove(item.ToString.IndexOf("|#|"), item.ToString.Length - item.ToString.IndexOf("|#|")))
            newitem.SubItems.Add(item.ToString.Remove(0, item.ToString.IndexOf("|#|") + 3))
            ListView1.Items.Add(newitem)
        Next
        returntext = sender
        For Each item As ListViewItem In ListView1.Items
            If item.SubItems(1).Text = sender.text Then
                ListView1.Items(ListView1.Items.IndexOf(item)).Selected = True
            End If
        Next
        Me.Show()
    End Sub
    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Dim hkcreate As String
        If Not Button3.Text = "" Then
            If Not TextBox1.Text = "" Then
                hkcreate = Button3.Text
                Dim newitem As ListViewItem ' = New ListViewItem(Button3.Text)
                If Not Button4.Text = "" Then
                    hkcreate += "  + " + Button4.Text
                    If Not Button5.Text = "" Then
                        hkcreate += "  + " + Button5.Text
                        If Not Button6.Text = "" Then
                            hkcreate += "  + " + Button6.Text
                        End If
                    End If
                End If
                newitem = New ListViewItem(TextBox1.Text)
                newitem.SubItems.Add(hkcreate)
                If (saving = True) Then
                    saving = False
                    ListView1.Items.Remove(ListView1.SelectedItems(0))
                End If
                ListView1.Items.Add(newitem)
                ListView1.Items(ListView1.Items.IndexOf(newitem)).Selected = True
            Else
                saving = False
                MsgBox("Please Name your preset")
                Exit Sub
            End If

        Else
            saving = False
            MsgBox("Please Create a hotkey before saving...")
            Exit Sub
        End If
        Button3.Text = ""
        Button4.Text = ""
        Button5.Text = ""
        Button6.Text = ""
        TextBox1.Text = "Default"
    End Sub

    Private Sub Button7_Click(sender As Object, e As EventArgs) Handles Button7.Click
        ListView1.Items.RemoveAt(ListView1.SelectedIndices(0))
        Label3.Text = ""
    End Sub
    Dim saving As Boolean = False
    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        Try
            If Not (ListView1.SelectedItems(0).Text = "") Then
                saving = True
                Button1.PerformClick()
                End If
        Catch ex As Exception
            MsgBox("Please select/edit an item before saving")
        End Try
    End Sub

    Private Sub ListView1_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ListView1.SelectedIndexChanged
        Button3.Text = ""
        Button4.Text = ""
        Button5.Text = ""
        Button6.Text = ""
        Try
            TextBox1.Text = ListView1.SelectedItems(0).Text
            Label3.Text = ListView1.SelectedItems(0).Text
            Dim splitterstring() As String = ListView1.SelectedItems(0).SubItems(1).Text.ToString.Replace(" ", "").Split("+")
            For i = 0 To splitterstring.Count - 1
                Select Case i
                    Case 0
                        Button3.Text = splitterstring(i)
                    Case 1
                        Button4.Text = splitterstring(i)
                    Case 2
                        Button5.Text = splitterstring(i)
                    Case 3
                        Button6.Text = splitterstring(i)


                End Select
            Next
        Catch ex As Exception

        End Try
    End Sub
    Private shiftdown() As Boolean = {False, False, False, False}
    Private Sub Button3_KeyDown(sender As Object, e As KeyEventArgs) Handles Button3.KeyDown
        ' MsgBox(e.KeyCode.ToString)
        sender.Text = e.KeyCode.ToString
    End Sub


    Private Sub Button4_KeyDown(sender As Object, e As KeyEventArgs) Handles Button4.KeyDown
        sender.Text = e.KeyCode.ToString
    End Sub


    Private Sub Button5_KeyDown(sender As Object, e As KeyEventArgs) Handles Button5.KeyDown
        sender.Text = e.KeyCode.ToString
    End Sub


    Private Sub Button6_KeyDown(sender As Object, e As KeyEventArgs) Handles Button6.KeyDown
        sender.Text = e.KeyCode.ToString
    End Sub


    Private Sub Button9_Click(sender As Object, e As EventArgs) Handles Button9.Click
        Button4.Text = ""

    End Sub

    Private Sub Button10_Click(sender As Object, e As EventArgs) Handles Button10.Click
        Button3.Text = ""
    End Sub

    Private Sub Button11_Click(sender As Object, e As EventArgs) Handles Button11.Click
        Button5.Text = ""
    End Sub

    Private Sub Button12_Click(sender As Object, e As EventArgs) Handles Button12.Click
        Button6.Text = ""
    End Sub

    Private Sub Button8_Click(sender As Object, e As EventArgs) Handles Button8.Click
        Me.Close()
    End Sub

    Private Sub HotKeyHandler_FormClosing(sender As Object, e As FormClosingEventArgs) Handles MyBase.FormClosing
        Try
            returntext.text = ListView1.SelectedItems(0).SubItems(1).Text
            My.Settings.Presets = ""
            For Each item As ListViewItem In ListView1.Items
                My.Settings.Presets += Environment.NewLine + item.Text + "|#|" + item.SubItems(1).Text
            Next
            TaskManager.setnewkey()
        Catch ex As Exception
            MsgBox("Please Set the hotkey preset before exiting")
            e.Cancel = True
        End Try
    End Sub

End Class