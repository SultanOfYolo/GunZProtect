Public Class Login_Account
    Private Sub OK_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles OK.Click
        RunLogin()
    End Sub
    Sub RunLogin()
        Dim User As String = UsernameTextBox.Text
        Dim pass As String = PasswordTextBox.Text
        Dim is_accunt As Boolean
        Try
            is_accunt = acctype.SelectedItem.ToString.Equals("User name")
        Catch ex As Exception
            Log("Incorrect selction item on Account Login")
            MsgBox("Please, you can only use your Account/User name or your Character name to Login!" & vbCrLf & "Error code: 003", MsgBoxStyle.Critical)
            Exit Sub
        End Try

        Dim str As String = "Character name"
        If is_accunt Then str = "User name"
        If User.Equals("") Then
            MsgBox("Please, Enter your " & str, MsgBoxStyle.Exclamation)
        ElseIf User.Length < 3 Then
            MsgBox("Please, Enter your correct " & str, MsgBoxStyle.Exclamation)
        ElseIf pass.Equals("") Then
            MsgBox("Please, Enter your Password", MsgBoxStyle.Exclamation)
        ElseIf pass.Length < 6 Then
            MsgBox("Please, Enter your correct Password", MsgBoxStyle.Exclamation)
        ElseIf User.Equals(pass) Then
            MsgBox("Please, check your " & str & " and Password, they can't be the same", MsgBoxStyle.Exclamation)
        Else
            OK.Enabled = False
            UsernameTextBox.Enabled = False
            PasswordTextBox.Enabled = False
            If Not Settings.Login(User, pass, is_accunt) Then
                OK.Enabled = True
                UsernameTextBox.Enabled = True
                PasswordTextBox.Enabled = True
            Else
                Timer1.Start()
            End If
        End If
    End Sub
    Private Sub Cancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Cancel.Click
        End
    End Sub
    Private Sub Timer1_Tick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Timer1.Tick
        If Not AccountState.Equals(AccountInfo.Firstrun) Then
            Timer1.Stop()
            If Loggedin Then
                If Settings.AccountState = AccountInfo.Administrator Or Settings.AccountState = AccountInfo.Developer_GM Then
                    Admin_Console.Show()
                End If
                Log("Logged in with " & AccountState.ToString)
                Me.Hide()
                Launcher.Show()
                If CheckBox1.Checked Then
                    If Not IO.Directory.Exists(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) & "\SSGunZ") Then
                        Try
                            IO.Directory.CreateDirectory(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) & "\SSGunZ")
                        Catch ex As Exception
                            Log(ex.Message)
                            MsgBox("Please run the Launcher as an Administrator!" & vbCrLf & "Error code: 006", MsgBoxStyle.Exclamation)
                            End
                        End Try
                    End If
                    Try
                        IO.File.WriteAllLines(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) & "\SSGunZ\launcher.dat", _
                        New String() {"Account/Char=" & Int(acctype.SelectedIndex = 1).ToString, "Login_name=" & UsernameTextBox.Text, "Login_password=" & PasswordTextBox.Text})
                    Catch ex As Exception
                        Log(ex.Message)
                        MsgBox("Please run the Launcher as an Administrator!" & vbCrLf & "Error code: 004", MsgBoxStyle.Exclamation)
                        End
                    End Try
                Else
                    If IO.File.Exists(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) & "\SSGunZ\launcher.dat") Then
                        Try
                            IO.File.Delete(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) & "\SSGunZ\launcher.dat")
                        Catch ex As Exception
                            Log(ex.Message)
                            MsgBox("Please run the Launcher as an Administrator!" & vbCrLf & "Error code: 005", MsgBoxStyle.Exclamation)
                            End
                        End Try
                    End If
                End If
            Else
                If Settings.LoginError.Length > 0 Then
                    MsgBox(LoginError, MsgBoxStyle.Exclamation)
                    LoginError = ""
                    UsernameTextBox.Enabled = True
                    PasswordTextBox.Enabled = True
                    OK.Enabled = True
                    Cancel.Enabled = True
                End If
            End If
            Timer1.Enabled = False
        End If
    End Sub

    Private Sub Login_Account_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Dim CurrentProcesses() As Process
        CurrentProcesses = Process.GetProcessesByName(Process.GetCurrentProcess.ProcessName)
        If CurrentProcesses.GetUpperBound(0) > 0 Then
            CurrentProcesses(0).Kill()
            functions.EndGunZ()
        End If
        If IO.File.Exists("Alog.txt") Then
            Try
                IO.File.Delete("Alog.txt")
            Catch ex As Exception
                MsgBox("Failed to delete file: Alog.txt!" & vbCrLf & vbCrLf & ex.Message & vbCrLf & "Error code: 002", MsgBoxStyle.Critical)
                End
            End Try
        End If
        If IO.File.Exists(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) & "\SSGunZ\launcher.dat") Then
            Try
                Dim loginbuff As String() = IO.File.ReadAllLines(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) & "\SSGunZ\launcher.dat")
                acctype.SelectedIndex = Int(loginbuff(0).Split("=")(1))
                UsernameTextBox.Text = loginbuff(1).Split("=")(1)
                PasswordTextBox.Text = loginbuff(2).Split("=")(1)
                CheckBox1.Checked = CheckState.Checked
            Catch ex As Exception
                Log("Launcher info file is broken!")
                UsernameTextBox.Clear()
                PasswordTextBox.Clear()
                Try
                    IO.File.Delete(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) & "\SSGunZ\launcher.dat")
                Catch exs As Exception
                    Log(exs.Message)
                    MsgBox("Please run the Launcher as an Administrator!" & vbCrLf & "Error code: 001", MsgBoxStyle.Exclamation)
                    End
                End Try
            End Try
        End If
    End Sub

End Class
