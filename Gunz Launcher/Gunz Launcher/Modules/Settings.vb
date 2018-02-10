Imports System.Net.Sockets
Imports System.Text
Imports System.Threading
Module Settings
    Public UserID As String
    Public Password As String
    Public isacc As Boolean
    Public refreshed As Boolean = False
    Public Client As TcpClient
    Public serverStream As NetworkStream
    Dim ListenT As Thread
    Dim Loginattempts As Int16
    Public AccountState As AccountInfo = AccountInfo.Firstrun
    Public Loggedin As Boolean = False
    Public LoginError As String = "Error, please Login"
    Public Update_Server As String = "http://update.ssgunz.com/"
    Public Runnable As String = "Gunz.exe"
    Public Server As String = "http://ssgunz.com/" '"http://5.100.171.19/" '
    Private Port As Int16 = 55
    Public filelist As String()
    Dim loginattempt_date As Date
    Public commandtext As String = ""
    Public PlayerList As New List(Of String)
    Public Function Login(ByVal User As String, ByVal pass As String, ByVal acc As Boolean) As Boolean
        If Not Loginattempts > 4 Then
ll:
            If Connect() Then
                Loginattempts += 1
                isacc = acc
                UserID = User
                Password = pass
                Try
                    Send(Packet.LoginAcc, Bytes(_Encrypt(User & "$" & pass & "$" & Int(acc).ToString & "$" & fx(User.Length, pass.Length) & "$" & getmac(), "Login.Info")))
                    loginattempt_date = Date.Now
                    Return True
                Catch ex As Exception
                    Login_Account.Timer1.Enabled = False
                    ListenT.Abort()
                    Client.Client.Close()
                    Log("Login Packet Sending failed!" & vbCrLf & ex.Message)
                    MsgBox("Error while connecting and logging to server, please try again later!", MsgBoxStyle.Exclamation)
                End Try
            Else
                MsgBox("Error in connecting to server, please try again later", MsgBoxStyle.Exclamation)
            End If
        Else
            If loginattempt_date.AddMinutes(30) > Date.Now Then
                MsgBox("You tried to login 5 times, you may need to wait 30 mins to retry to login!", MsgBoxStyle.Exclamation)
            Else
                Loginattempts = 0
                GoTo ll
            End If
        End If
        Return False
    End Function
    Public Function Connect() As Boolean
        Client = New TcpClient
        Try
            'Client.Connect("127.0.0.1", 55)
            Client.Connect(Server.Split("/")(2), Port)
            ListenT = New Thread(AddressOf Incoming_Packets)
            ListenT.Start()
        Catch ex As Exception
            Log(ex.Message)
            Return False
        End Try
        Return True
    End Function
    Public Sub Send(ByVal ID As Byte, ByVal data As Byte())
        Dim ID_byte(data.Length) As Byte
        ID_byte(0) = ID
        data.CopyTo(ID_byte, 1)
        serverStream = Client.GetStream()
        serverStream.Write(ID_byte, 0, ID_byte.Length)
        serverStream.Flush()
    End Sub
    Public Sub Incoming_Packets()
        Do
            Dim inStream As Byte() = {}
            Try
                inStream = New Byte(Client.ReceiveBufferSize) {}
                serverStream = Client.GetStream
                serverStream.Read(inStream, 0, inStream.Length)
            Catch ex As Exception
                Log("Stream Read Error, " & ex.Message)
                Client.Client.Close()
                MsgBox("Connection lost to server!", MsgBoxStyle.Critical)
                EndGunZ()
                End
            End Try

            Select Case inStream(0)
                Case 0
                    LoginError = "Server Error, please try again later, connection failed"
                    Log("NULL return")
                Case Packet.LoginAcc
                    Loggedin = True
                    AccountState = CType(inStream(1), AccountInfo)
                    If isacc Then
                        Log("Welcome, your logged with your User: " & UserID)
                    Else
                        Log("Welcome, your logged with your Character " & UserID)
                    End If
                    Continue Do
                Case Packet.FailedLoginAcc
                    AccountState = AccountInfo.FailedLogging
                    LoginError = "Failed to login, please check your user name and password!"
                    Log(LoginError)
                    Exit Sub
                Case Packet.ClientDisconnected
                    ' AccountState = AccountInfo.FailedLogging
                    Client.Client.Close()
                    LoginError = "Connection to server disconnected!"
                    Log(LoginError)
                Case Packet.AdminBannedClient
                    Client.Client.Close()
                    'AccountState = AccountInfo.Banned
                    LoginError = get_gstring(inStream)
                    Log(LoginError)
                Case Packet.hardwarebanned
                    'AccountState = AccountInfo.FailedLogging
                    Client.Client.Close()
                    LoginError = "Your PC is banned, you can't play SSGunZ!"
                    Log(LoginError)
                Case Packet.AccountNotActive
                    Client.Client.Close()
                    ' AccountState = AccountInfo.NotActive
                    LoginError = "Your account is not activated, please acivate your account first!"
                    Log(LoginError)
                Case Packet.AccountBanned
                    Client.Client.Close()
                    ' AccountState = AccountInfo.Banned
                    LoginError = "Your account is Banned!"
                    Log(LoginError)
                Case Packet.Key
                    Dim Key_data() As String = get_gstring(inStream).Split("$")
                    Try
                        Send(Packet.Key, Bytes(_Encrypt(Key_data(0), Key_data(1)) & "$" & Key_data(1)))
                    Catch ex As Exception
                        EndGunZ()
                        Log("Refresh CodeKey Failed, connection terminated!")
                        MsgBox("Error while sending data to server, you have been disconnected!", MsgBoxStyle.Exclamation)
                        End
                    End Try
                    Continue Do
                Case AdminPacketCommand.AccountBan
                    Client.Client.Close()
                    ' AccountState = AccountInfo.Banned
                    LoginError = get_gstring(inStream)
                    Log(LoginError)
                Case AdminPacketCommand.AddConnectedPlayer
                    Dim tmp_str As String = get_gstring(inStream)
                    If tmp_str.Contains("%") Then
                        PlayerList.AddRange(tmp_str.Split("%"))
                        PlayerList.RemoveAt(PlayerList.Count - 1)
                    Else
                        PlayerList.Add(tmp_str)
                    End If
                    Continue Do
                Case AdminPacketCommand.ClientBan
                    Client.Client.Close()
                    ' AccountState = AccountInfo.Banned
                    LoginError = "Your Client is Banned!"
                    Log(LoginError)
                Case AdminPacketCommand.ClientDisconnect
                    Client.Client.Close()
                    ' AccountState = AccountInfo.Banned
                    LoginError = "You have been disconnected!"
                    Log(LoginError)
                Case AdminPacketCommand.IPBan
                    Client.Client.Close()
                    ' AccountState = AccountInfo.Banned
                    LoginError = "Your IP has been blacklisted!"
                    Log(LoginError)
                Case AdminPacketCommand.RemoveConnectedPlayer
                    PlayerList.Remove(get_gstring(inStream))
                    Continue Do
                Case AdminPacketCommand.RefreshPlayerList
                    commandtext = get_gstring(inStream)
                    refreshed = True
                    Continue Do
                Case Packet.IPbanned
                    Client.Client.Close()
                    ' AccountState = AccountInfo.Banned
                    LoginError = "Your IP is Banned!"
                    Log(LoginError)
                Case Packet.hackinfo
                    AntiCheat.Hackarray = Cryption.Decrypt(get_gstring(inStream).Split(",")(0)).Split(",")
                    Continue Do
            End Select
            EndGunZ()
            MsgBox(LoginError, MsgBoxStyle.Exclamation)
            End
        Loop
    End Sub
End Module
