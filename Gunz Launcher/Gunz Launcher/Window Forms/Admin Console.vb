Imports System.Net
Imports System.Net.Sockets
Imports Gunz_Launcher.Enums
Public Class Admin_Console
    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        Command(AdminPacketCommand.RefreshPlayerList)
    End Sub
    Private Sub Button3_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button3.Click
        Command(AdminPacketCommand.IPBan)
    End Sub

    Private Sub Button2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button2.Click
        Command(AdminPacketCommand.ClientDisconnect)
    End Sub

    Private Sub Button4_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button4.Click
        Command(AdminPacketCommand.AccountBan)
    End Sub

    Private Sub Button5_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button5.Click
        Command(AdminPacketCommand.ClientBan)
    End Sub

    Private Sub Button6_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button6.Click
        Command(AdminPacketCommand.UnbanIP)
    End Sub

    Private Sub Button7_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button7.Click
        Command(AdminPacketCommand.UnbanAccount)
    End Sub

    Private Sub Button8_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button8.Click
        Command(AdminPacketCommand.UnbanClient)
    End Sub
    Function get_user(ByVal id As Integer) As String
        If id > -1 Then
            Dim str As String = ListBox1.Items(id).ToString
            Dim str2 As String = ""
            If str.Contains("] UserID: ") Then
                str2 = "1$" & str.Split("]")(1).Replace(" UserID: ", "").Replace(" - IP: ", "$").Split("$")(0)
            Else
                str2 = "0$" & str.Split("]")(1).Replace(" Character: ", "").Replace(" - IP: ", "$").Split("$")(0)
            End If
            Return str2
        Else
            Return "0"
        End If
    End Function
    Function get_ip(ByVal id As Integer) As String
        If id > -1 Then
            Return ListBox1.Items(id).ToString.Replace(" - IP: ", "$").Split("$")(1)
        Else
            Return "0"
        End If
    End Function
    Private Sub Timer1_Tick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Timer1.Tick
        If Settings.refreshed Then
            Settings.refreshed = False
            If Settings.commandtext.Equals("Online Player list refreshed") Then
                ListBox1.Items.Clear()
                ListBox1.Items.AddRange(functions.Get_players)
            End If
            Alog(Settings.commandtext)
            Settings.commandtext = ""
            Timer1.Enabled = False
        End If
    End Sub
    Sub Alog(ByVal v As String)
        ListBox2.Items.Add("[" & TimeOfDay & "] " & v)
        ListBox2.SelectedIndex = ListBox2.Items.Count - 1
    End Sub
    Sub Command(ByVal com As Byte)
        If ListBox1.SelectedIndex > -1 Or com.Equals(AdminPacketCommand.RefreshPlayerList) Then
            functions.Clean_players()
            'ListBox1.Items.Clear()
            Timer1.Start()
            Try
                Send(com, functions.Bytes(Settings.UserID & "$" & Settings.Password & "$" & get_user(ListBox1.SelectedIndex) & "$" & get_ip(ListBox1.SelectedIndex)))
            Catch ex As Exception
                Dim str As Boolean = Convert.ToBoolean(Int(get_user(ListBox1.SelectedIndex).Split("$")(0)))
                If str Then
                    Alog("Error on " & CType(com, AdminPacketCommand).ToString & " on UserID " & get_user(ListBox1.SelectedIndex))
                Else
                    Alog("Error on " & CType(com, AdminPacketCommand).ToString & " on Character " & get_user(ListBox1.SelectedIndex))
                End If
                Log(ex.Message)
            End Try
        Else
            Alog("Please select a player from the list before executing a command!")
        End If
    End Sub

    Private Sub Button9_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button9.Click
        If Not Button9.Text = "Use Selected Result" Then
            Dim is_Acc As Boolean, text As String = TextBox1.Text.ToLower, results As New List(Of Integer)
            Try
                is_Acc = ComboBox1.SelectedItem.ToString.Equals("UserID/Character")
            Catch
                Alog("Incorrect search type selected!")
                Exit Sub
            End Try
            For i As Integer = 0 To ListBox1.Items.Count - 1
                If is_Acc Then
                    If get_user(i).ToLower.Substring(2).Equals(text) Then
                        results.Add(i)
                    End If
                Else
                    If get_ip(i).Split(":")(0).Equals(text) Then
                        results.Add(i)
                    End If
                End If
            Next
            If results.Count > 0 Then
                For i As Int16 = 0 To results.Count - 1
                    ListBox3.Items.Add(ListBox1.Items(results(i)).ToString)
                Next
                ListBox3.SelectedIndex = ListBox3.Items.Count - 1
                Button9.Text = "Use Selected Result"
                Alog(results.Count.ToString & " Matchs found!")
            Else
                Alog("No results Matched your data!")
            End If
        Else
            ListBox1.SelectedItem = ListBox3.SelectedItem
            Button9.Text = "Search"
            ListBox3.Items.Clear()
        End If
    End Sub

    Private Sub Button10_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button10.Click
        ListBox3.Items.Clear()
        Button9.Text = "Search"
    End Sub

    Private Sub Admin_Console_FormClosing(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing
        e.Cancel = True
        NotifyIcon1.Visible = True
        Me.Hide()
        'Alog("Admin Console can't be closed, it can only be minimized!")
    End Sub

    Private Sub Admin_Console_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Command(AdminPacketCommand.RefreshPlayerList)
    End Sub

    Private Sub NotifyIcon1_MouseDoubleClick(ByVal sender As System.Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles NotifyIcon1.MouseDoubleClick
        NotifyIcon1.Visible = False
        Me.Show()
    End Sub
End Class