Imports System.Net.Sockets
Imports System.Text

Module Program
    Public Client_list As New List(Of TcpClient)
    Public User_Password_IP As New List(Of String)
    Public Staff_User_IP As New List(Of String)
    Public bannedaccounts As New List(Of String)
    Public BannedStreamList As New List(Of Int64)
    Sub Main()
        Console.Title = "Loading......."
        Console.WindowWidth = Console.LargestWindowWidth - 20
        Dim clientSocket As TcpClient
        Dim DDOS_IPs As New List(Of Int64)
        Dim DDoS_block As New List(Of Int64)
        Dim DDoS_time(255, 255, 255) As Date
        Console.ForegroundColor = ConsoleColor.DarkCyan
        Console.WriteLine(" [" & Date.Now.ToString & "] This Application is written by Demantor@live.com")
        Functions.Show("Loading Engine...", "Program.Main.Iniziale")
        Functions.Show("Loading Server Settings...", "Program.Main.Load")
        Load_settings()
        Dim db_thread As New Threading.Thread(AddressOf Database.DB__Thread)
        db_thread.Start()
        Dim serverSocket As TcpListener
        Try
            serverSocket = New TcpListener(Net.IPAddress.Parse(Functions.IP), Functions.Port) ' or .Any for multi adapters/IPs
            serverSocket.Start()
        Catch ex As Exception
            Log("Network init. error, " & ex.Message)
            Process.Start("Alog.txt")
            End
        End Try
        Functions.Show("Login Server Started on IP: " & Functions.IP & " - Port: " & Functions.Port.ToString, "Program.Main", ConsoleColor.Green)
        Console.Beep()
        Do
            Dim IP_address As String, _IP As Int64, _IP_ As String
            Try
                clientSocket = serverSocket.AcceptTcpClient()
                IP_address = clientSocket.Client.RemoteEndPoint.ToString
            Catch ex As Exception
                Log(ex.Message)
                Show(ex.Message, "Program.Main", ConsoleColor.Red)
                Continue Do
            End Try
            _IP_ = IP_address.Split(":")(0)
            _IP = _IP_.Replace(".", Nothing)
            If DDoS_block.Contains(_IP) Then
                clientSocket.Client.Close()
                Continue Do
            End If
            Dim Date_IP() As Int16 = get_3d_ip(_IP_), now As Date = Date.Now
            If BannedStreamList.Contains(_IP) Then
                Functions.Send_Packet(Packet.IPbanned, New Byte() {}, clientSocket, "Program.Main")
                clientSocket.Client.Close()
                If DDOS_IPs.Contains(_IP) Then
                    If DDoS_time(Date_IP(0), Date_IP(1), Date_IP(2)).AddSeconds(2) > now Then
                        DDoS_block.Add(_IP) ' must be as a sub program!!!!
                        Dim packetcapc As Single = Math.Round((DDoS_time(Date_IP(0), Date_IP(1), Date_IP(2)).ToBinary / now.ToBinary) * 100, 2)
                        Dim curm_clientm As Single = Math.Round((now.ToBinary / Date.Now.ToBinary) * 100, 2)
                        Dim approxstr As String, caser As Single = Convert.ToSingle(packetcapc / curm_clientm)
                        Select Case caser
                            Case 1.0
                                approxstr = "Low %" & caser & "%]"
                            Case 1.0 To 1.01
                                approxstr = "High %" & caser & "%]"
                            Case 1.01 To 1.02
                                approxstr = "Ultra High %" & caser & "%]"
                            Case Else
                                approxstr = "Too High: %" & caser & "%]"
                                Functions.Show("**************** WARNING: SERVER OVERLOADED ****************", "Program.Main", ConsoleColor.Red)
                        End Select
                        Functions.Show("DDoS Attack from: " & _IP_ & _
                  " - %CPU(Add.%Examin.): [" & curm_clientm.ToString _
               & "%] - %Cycle [" & packetcapc & "%] -  %CPU%Usage [" & approxstr & "]", "Program.Main", ConsoleColor.Red)
                        Log("DDoS Attack from: " & _IP_ & _
                  " - %CPU(Add.%Examin.): [" & curm_clientm.ToString _
               & "%] - %Cycle [" & packetcapc & "%] - %CPU%Usage [" & approxstr & "]")
                        Continue Do
                    Else
                        DDoS_time(Date_IP(0), Date_IP(1), Date_IP(2)) = Date.Now
                        Functions.Show("Dropped Banned [NON-Blocked.DDoS IP]: " & _IP_, "Program.Main", ConsoleColor.DarkRed)
                        Continue Do
                    End If
                End If
                DDoS_time(Date_IP(0), Date_IP(1), Date_IP(2)) = Date.Now
                If Not DDOS_IPs.Contains(_IP) Then DDOS_IPs.Add(_IP)
                Functions.Show("Dropped Banned IP: " & _IP_, "Program.Main", ConsoleColor.DarkRed)
                Continue Do
            End If
            If get_warning_level(_IP_) > 3 Then
                If get_warning_time(_IP_).AddSeconds(1) > Date.Now Then
                    BanIP(_IP_, "Program.Main")
                    Functions.addbannedstream(_IP_, True)
                    clientSocket.Client.Close()
                    Continue Do
                Else
                    reset_warning_level(_IP_)
                    ' add_warning_level(_IP_, -1)
                End If
            End If
            Dim p As New Thread_Protect
            p.Readbyter(Date.Now, _IP_, clientSocket)
            Dim bytesFrom(10024) As Byte 'Too much, will check that soon (soon = sometime xD)
            Try
                clientSocket.GetStream().Read(bytesFrom, 0, 1)
                If bytesFrom(0) = 0 Then
                    add_warning_level(_IP_)
                    clientSocket.Client.Close()
                    Continue Do
                ElseIf bytesFrom(0).Equals(Packet.LoginAcc) Then '1 Then
                    clientSocket.GetStream().Read(bytesFrom, 1, CInt(clientSocket.ReceiveBufferSize))
                    p.endbyter()
                    Dim pl As New Process_Login
                    pl.StartProcess(get_gstring(bytesFrom), clientSocket, IP_address)
                    Continue Do
                Else
                    add_warning_level(_IP_)
                    clientSocket.Client.Close()
                    Continue Do
                End If
            Catch
                add_warning_level(_IP_)
                clientSocket.Client.Close()
                Continue Do
            End Try
            'Process_LoginPacket(get_gstring(bytesFrom), clientSocket, IP_address)
        Loop
    End Sub
    Sub ClientListTimersub()
        Dim IP_client As String = "Client.IP.Error", IP_Clientx As String
        Dim i As Integer = 0
        Do While i <= Client_list.Count - 1
            Try
                IP_client = Client_list(i).Client.RemoteEndPoint.ToString
                IP_Clientx = IP_client.Replace(":", ".")
            Catch
                Client_list(i).Client.Close()
                Functions.removeclient_clientlist(Client_list(i))
                Functions.Show(IP_client & " → Client.TimedOut", "Program.SocketCleaner", ConsoleColor.Yellow)
                Console.Title = Client_list.Count.ToString & " Connected Clients"
                Continue Do
            End Try
            If Client_list(i).Client.IsBound And Client_list(i).Connected Then
                Try
                    Dim MyNetworkStream As NetworkStream = Client_list(i).GetStream
                    If Not MyNetworkStream.CanRead Or Not MyNetworkStream.CanWrite Then
                        Functions.Show(IP_client & " → Client.TimedOut", "Program.SocketCleaner", ConsoleColor.Yellow)
                        Client_list(i).Client.Close()
                        Functions.removeclient_clientlist(Client_list(i))
                        Functions.update_clienttime(IP_Clientx, True)
                        Console.Title = Client_list.Count.ToString & " Connected Clients"
                        Continue Do
                    Else
                        If check_clienttime(IP_Clientx) Then
                            Functions.set_clientcode(RandomString(New Random().Next(5, 10)).ToString & "$".ToString & RandomString(New Random().Next(10, 15)).ToString, IP_Clientx)
                            Functions.Send_Packet(Packet.Key, Functions.Bytes(Functions.get_clientcode(IP_Clientx)), Client_list(i), "KeyMaster.SetKey")
                        Else
                            Functions.Show(IP_client & " → Client.TimedOut", "Program.SocketCleaner", ConsoleColor.Yellow)
                            Client_list(i).Client.Close()
                            Functions.removeclient_clientlist(Client_list(i))
                            Functions.update_clienttime(IP_Clientx, True)
                            Console.Title = Client_list.Count.ToString & " Connected Clients"
                            Continue Do
                        End If
                    End If
                Catch
                    Functions.Show(IP_client & " → Client.TimedOut", "Program.SocketCleaner", ConsoleColor.Yellow)
                    Client_list(i).Client.Close()
                    Functions.removeclient_clientlist(Client_list(i))
                    Functions.update_clienttime(IP_Clientx, True)
                    Console.Title = Client_list.Count.ToString & " Connected Clients"
                    Continue Do
                End Try
            Else
                Functions.Show(IP_client & " → Client.TimedOut", "Program.SocketCleaner", ConsoleColor.Yellow)
                Client_list(i).Client.Close()
                Functions.removeclient_clientlist(Client_list(i))
                Functions.update_clienttime(IP_Clientx, True)
                Console.Title = Client_list.Count.ToString & " Connected Clients"
                Continue Do
            End If
            i += 1
        Loop
    End Sub
    Public Class Client_thread
        Dim User_data As String()
        Dim cIP As String
        Dim Client As TcpClient
        Dim Mac As String
        Dim IS_account As Boolean
        Dim FullIP As String
        Sub Start_Client(ByVal userdata As String(), ByVal qip As String, ByVal bclient As TcpClient, ByVal bmac As String, ByVal isacc As Boolean)
            Me.FullIP = qip
            Me.cIP = qip.Split(":")(0)
            Me.User_data = userdata
            Me.Client = bclient
            Me.Mac = bmac
            Me.IS_account = isacc
            Database.Locator_access(cIP, True)
            Functions.MatchServer_access(cIP, True)
            Dim cClient As New Threading.Thread(AddressOf RunClient)
            cClient.Start()
        End Sub
        Sub RunClient()
            Dim lastpackettime As Date = Date.MinValue
            Dim Timestampcheck(6) As Date
            Dim TempKey(6) As String
            Dim i As Int16 = -1
            Dim lvl As Int16 = 0
            Dim _IP As String = FullIP.Replace(":", ".")
            Functions.Show(Functions.rank(Int(User_data(2)), IS_account) & User_data(0) & " - IP: " & FullIP & " Logged in", "Thread.RunClient→Start", ConsoleColor.Green)
            Functions.update_clienttime(_IP)
            Functions.add_ClientList(Client)
            Console.Title = Program.Client_list.Count.ToString & " Connected Clients"
            Do
                Dim bytesFrom(10024) As Byte
                Try
                    Client.GetStream().Read(bytesFrom, 0, CInt(Client.ReceiveBufferSize))
                Catch
                    Exit Do
                End Try
                If Validate_CommandID(bytesFrom(0), Int(User_data(2))) Then
                    If lastpackettime.AddSeconds(1) > Date.Now Then
                        Dim warnlvl As Int16 = Functions.get_warning_level(cIP)
                        Dim wait_time As Int16 = 5000
                        If Int(User_data(2)) >= 254 Then wait_time = 500
                        If Functions.get_warning_time(cIP).AddMilliseconds(wait_time) > Date.Now Then
                            If warnlvl > 4 Then
                                If warnlvl > 10 Then
                                    Database.BanIP(cIP, "Thread.RunClient.Compare.Time")
                                    Database.BanAccount(User_data(0), IS_account, "Thread.RunClient.Compare.Time")
                                    Exit Do
                                End If
                                Functions.Show(Functions.rank(Int(User_data(2)), IS_account) & User_data(0) & " - IP: " & FullIP & " - Too Many requests!", "Thread.RunClient.Compare.Time", ConsoleColor.Red)
                                Log(Functions.rank(Int(User_data(2)), IS_account) & User_data(0) & " - IP: " & FullIP & " - Too Many requests!")
                            End If
                            Functions.add_warning_level(cIP)
                        Else
                            Functions.reset_warning_level(cIP)
                        End If
                    End If
                    lastpackettime = Date.Now
                    Dim datastr As String = get_gstring(bytesFrom)
                    If datastr.Contains("$") Then
                        Dim aData As String() = datastr.Split("$")
                        If aData.Length = 5 Or bytesFrom(0) = Packet.Key Or bytesFrom(0) = AdminPacketCommand.RefreshPlayerList Then ' Or bytesFrom(0) = Packet.Ping Then
                            Dim tB As Boolean, tU As String = ""
                            If Not bytesFrom(0) = Packet.Key And Not bytesFrom(0) = AdminPacketCommand.RefreshPlayerList Then 'And Not bytesFrom(0) = Packet.Ping Then
                                For d As Int16 = 0 To aData.Length - 1 ' 4
                                    If Not aData(d).Equals(Functions.Clean(aData(d), cIP, "CleanPacketData")) Then
                                        Functions.Log("Warning: User: " & Functions.rank(Int(User_data(2)), IS_account) & User_data(0) & " Sent illegal packet data!!")
                                        Functions.add_warning_level(cIP)
                                        Exit Do
                                    End If
                                Next
                                Try
                                    tB = Convert.ToBoolean(Int(aData(2)))
                                Catch ex As Exception
                                    Functions.Show("Boolean Type (Acc/Char) Parse Error", "Thread.RunClient.IsAcc", ConsoleColor.Red)
                                    Exit Do
                                End Try
                                If tB Then
                                    tU = " Account: " & aData(3) & " - IP: " & aData(4)
                                Else
                                    tU = " Character: " & aData(3) & " - IP: " & aData(4)
                                End If
                            End If
                            Select Case bytesFrom(0)
                                Case AdminPacketCommand.AccountBan
                                    Functions.Show("Staff: " & Functions.rank(Int(User_data(2)), IS_account) & User_data(0) & "→ Banned the" & tU, "Thread.RunClient→ExecuteCommand", ConsoleColor.Red)
                                    Functions.Log("Staff: " & Functions.rank(Int(User_data(2)), IS_account) & User_data(0) & "→ Banned the" & tU)
                                    Database.BanAccount(aData(3), Convert.ToBoolean(Int(aData(2))), "Thread.RunClient→ExecuteCommand")
                                    Functions.DC(Functions._ObjectClient(aData(4)))
                                    Functions.RefreshPlayerlist("The" & tU & " Account Banned", Client)
                                    Continue Do
                                Case AdminPacketCommand.ClientBan
                                    Functions.Show("Staff: " & Functions.rank(Int(User_data(2)), IS_account) & User_data(0) & "→ Hardware Banned the" & tU, "Thread.RunClient→ExecuteCommand", ConsoleColor.Red)
                                    Functions.Log("Staff: " & Functions.rank(Int(User_data(2)), IS_account) & User_data(0) & "→ Hardware Banned the" & tU)
                                    Database.banClient(Functions._ObjectMac(aData(4)), aData(4).Split(":")(0), "Thread.RunClient→ExecuteCommand")
                                    Database.BanAccount(aData(3), Convert.ToBoolean(Int(aData(2))), "Thread.RunClient→ExecuteCommand")
                                    Database.BanIP(aData(4).Split(":")(0), "Thread.RunClient→ExecuteCommand")
                                    Functions.DC(Functions._ObjectClient(aData(4)))
                                    Functions.RefreshPlayerlist("The" & tU & " Hardware Banned", Client)
                                    Continue Do
                                Case AdminPacketCommand.ClientDisconnect
                                    Functions.Show("Staff: " & Functions.rank(Int(User_data(2)), IS_account) & User_data(0) & "→ Disconnected the" & tU, "Thread.RunClient→ExecuteCommand", ConsoleColor.Red)
                                    Functions.Log("Staff: " & Functions.rank(Int(User_data(2)), IS_account) & User_data(0) & "→ Disconnected the" & tU)
                                    Functions.DC(Functions._ObjectClient(aData(4)))
                                    Functions.RefreshPlayerlist("The" & tU & " Client Disconnected", Client)
                                    Continue Do
                                Case AdminPacketCommand.IPBan
                                    Functions.Show("Staff: " & Functions.rank(Int(User_data(2)), IS_account) & User_data(0) & "→ IP Banned the" & tU, "Thread.RunClient→ExecuteCommand", ConsoleColor.Red)
                                    Functions.Log("Staff: " & Functions.rank(Int(User_data(2)), IS_account) & User_data(0) & "→ IP Banned the" & tU)
                                    Database.BanIP(aData(4).Split(":")(0), "Thread.RunClient→ExecuteCommand")
                                    Functions.DC(Functions._ObjectClient(aData(4))) '.Client.Close()
                                    Functions.RefreshPlayerlist("The" & tU & " IP Banned", Client)
                                    Continue Do
                                Case AdminPacketCommand.RefreshPlayerList
                                    Functions.Show("Staff: " & Functions.rank(Int(User_data(2)), IS_account) & User_data(0) & "→ Requested Player list", "Thread.RunClient→ExecuteCommand")
                                    Functions.Log("Staff: " & Functions.rank(Int(User_data(2)), IS_account) & User_data(0) & "→ Requested Playerlist")
                                    Functions.RefreshPlayerlist("Online Player list refreshed", Client, True)
                                    Continue Do
                                Case AdminPacketCommand.UnbanAccount
                                    Functions.Show("Staff: " & Functions.rank(Int(User_data(2)), IS_account) & User_data(0) & "→ Unbanned the" & tU, "Thread.RunClient→ExecuteCommand", ConsoleColor.Red)
                                    Functions.Log("Staff: " & Functions.rank(Int(User_data(2)), IS_account) & User_data(0) & "→ Unbanned the" & tU)
                                    Database.UnbanAccount(aData(3), Convert.ToBoolean(Int(aData(2))), "Thread.RunClient→ExecuteCommand")
                                    Functions.DC(Functions._ObjectClient(aData(4)))
                                    Functions.RefreshPlayerlist("The" & tU & " Account Unbanned", Client)
                                    Continue Do
                                Case AdminPacketCommand.UnbanClient
                                    Functions.Show("Staff: " & Functions.rank(Int(User_data(2)), IS_account) & User_data(0) & "→ Hardware Unbanned the" & tU, "Thread.RunClient→ExecuteCommand", ConsoleColor.Red)
                                    Functions.Log("Staff: " & Functions.rank(Int(User_data(2)), IS_account) & User_data(0) & "→ Hardware Unbanned the" & tU)
                                    Database.unbanClient(Functions._ObjectMac(aData(4)), aData(4).Split(":")(0), "Thread.RunClient→ExecuteCommand")
                                    Database.UnbanAccount(aData(3), Convert.ToBoolean(Int(aData(2))), "Thread.RunClient→ExecuteCommand")
                                    Database.unbanIP(aData(4).Split(":")(0), "Thread.RunClient→ExecuteCommand")
                                    Functions.addbannedplayer(User_data(0) & "$" & User_data(1) & "$" & aData(4) & "$" & Mac & "$" & Int(IS_account).ToString, False)
                                    Functions.RefreshPlayerlist("The" & tU & " Client Unbanned", Client)
                                    Continue Do
                                Case AdminPacketCommand.UnbanIP
                                    Functions.Show("Staff: " & Functions.rank(Int(User_data(2)), IS_account) & User_data(0) & "→ IP Unbanned the" & tU, "Thread.RunClient→ExecuteCommand", ConsoleColor.Red)
                                    Functions.Log("Staff: " & Functions.rank(Int(User_data(2)), IS_account) & User_data(0) & "→ IP Unbanned the" & tU)
                                    Database.unbanIP(aData(4).Split(":")(0), "Thread.RunClient→ExecuteCommand")
                                    Functions.RefreshPlayerlist("The" & tU & " IP Unbanned", Client)
                                    Continue Do
                                Case Packet.Key
                                    If get_clienttime(_IP) = Nothing Then Exit Do
                                    Dim CLient_Code As String = get_clientcode(_IP)
                                    i += 1
                                    TempKey(i) = datastr.Split("$")(1)
                                    If Not TempKey(i).Equals(CLient_Code.Split("$")(1)) Then
                                        Dim ii As Int16 = 0, TempBool As Boolean = False
                                        Do While i - ii > -1
                                            If Not TempKey(i - ii) = Nothing Then
                                                If TempKey(i - ii).Equals(datastr.Split("$")(1)) Then
                                                    TempBool = True
                                                    Timestampcheck(lvl) = Date.Now
                                                    lvl += 1
                                                    Exit Do
                                                End If
                                            End If
                                            ii += 1
                                        Loop
                                        If lvl > 5 Then
                                            If Timestampcheck(3).AddSeconds(3) > Timestampcheck(5) Then
                                                Functions.Show("Error on Client.Key delay, Level: " & lvl.ToString & " → Disconnecting Client", "Thread.RunClient→Process.Key", ConsoleColor.Red)
                                                Functions.add_warning_level(cIP)
                                                Exit Do
                                            Else
                                                Functions.Show("Error on Client.Key delay, Level: " & lvl.ToString, "Thread.RunClient→Process.Key", ConsoleColor.Red)
                                                lvl += 1
                                            End If
                                        Else
                                            If Not TempBool Then
                                                lvl += 1
                                                Functions.Show(_IP & " → Client.[Packet.ClientKey.ID].IsFake(" & (i - ii).ToString & ")].Level(" & lvl.ToString & ").UnAuthenticated - [Client.ID.IsFake]... [" & get_clienttime(_IP).Second.ToString & "%" & Date.Now.Second.ToString & "]", "Thread.RunClient→Process.Key", ConsoleColor.Red)
                                            Else
                                                Functions.Show(_IP & " Client.[Packet.ClientKey.ID].IsOld(" & (i - ii).ToString & ")].Level(" & lvl.ToString & ").UnAuthenticated - [Client.IsLaging]... [" & get_clienttime(_IP).Second & "%" & Date.Now.Second & "]", "Thread.RunClient→Process.Key", ConsoleColor.Red)
                                            End If
                                            Continue Do
                                        End If
                                    ElseIf Functions.get_clienttime(_IP).AddSeconds(5) > Date.Now Then
                                        If lvl > 5 Then
                                            If Timestampcheck(3).AddSeconds(3) > Timestampcheck(5) Then
                                                Functions.Send_Packet(Packet.ClientDisconnected, {}, Client, "Thread.RunClient→Process.Key") '(Packet.Disconnect, clientSocket)
                                                Functions.Show(_IP & " → Client.[Packet.ClientKey].Delay.UnAuthenticated [" & Functions.get_clienttime(_IP).Second.ToString & "%" & Date.Now.Second.ToString & "]", "Thread.RunClient→Process.Key", ConsoleColor.Red)
                                                Functions.Log(_IP & " → Client.[Packet.ClientKey].Delay.UnAuthenticated [" & Functions.get_clienttime(_IP).Second.ToString & "%" & Date.Now.Second.ToString & "]!")
                                                Functions.add_warning_level(cIP)
                                                Exit Do
                                            Else
                                                Functions.Log(_IP & " → A lot Delay Errors!")
                                                Functions.Show(_IP & " Client.[Packet.ClientKey].Delay.Level(" & lvl.ToString & ").UnAuthenticated!!!!!!!!!!! [" & get_clienttime(_IP).Second.ToString & "%" & Date.Now.Second.ToString & "]", "Thread.RunClient→Process.Key", ConsoleColor.Red)
                                                lvl = 0
                                                Functions.add_warning_level(cIP)
                                                Continue Do
                                            End If
                                        Else
                                            If lvl > 1 Then Functions.Show(_IP & " Client.[Packet.ClientKey].Delay.Level(" & lvl.ToString & ").UnAuthenticated [" & get_clienttime(_IP).Second.ToString & "%" & Date.Now.Second.ToString & "]", "Thread.RunClient→Process.Key", ConsoleColor.Red)
                                            Timestampcheck(lvl) = Date.Now
                                            lvl += 1
                                            Functions.add_warning_level(cIP)
                                            Continue Do
                                        End If
                                    ElseIf Not Functions._Decrypt(datastr.Split("$")(0), TempKey(i)).Equals(CLient_Code.Split("$")(0)) Then
                                        Functions.Send_Packet(Packet.ClientDisconnected, {}, Client, "Thread.RunClient→Process.Key")
                                        Functions.Show(_IP & " → Client.[Packet.ClientKey.Cryption].UnAuthenticated .. [Disconnecting.Client]", "Thread.RunClient→Process.Key", ConsoleColor.Red)
                                        Functions.Log(_IP & " → Client.[Packet.ClientKey.Cryption].UnAuthenticated!")
                                        Functions.add_warning_level(cIP)
                                        Exit Do
                                    Else
                                        Functions.update_clienttime(_IP)
                                        i = -1
                                        If Not lvl < 1 Then lvl -= 1
                                        Continue Do
                                    End If
                                    ' Case Packet.Ping
                                    '    Exit Do
                                    'PING removed due to problems with many users!
                                    'Case Else
                                    '    Functions.Show("Error Parsing CommandID from " & Functions.rank(Int(User_data(2)), IS_account) & User_data(0) & " - IP: " & cIP, "Thread.RunClient→ReadCommand", ConsoleColor.Red)
                                    '   Functions.add_warning_level(cIP)
                                    '  Exit Do
                            End Select
                        ElseIf aData.Length = 2 Then
                            If Not Int(Val(aData(1))) > 0 Or Not Int(Val(aData(1))) < 101 Then 'ban packet
                                Functions.add_warning_level(cIP)
                                Database.BanIP(cIP, "Thread.RunClient→Fake.Packet")
                                Exit Do
                            End If
                            If bytesFrom(0) = Packet.AccountBanned Then
                                Dim tmp_hack As String = _Decrypt(aData(0), "Packet.ID.hack")
                                If tmp_hack.Contains("$") Then
                                    Dim hack_data() As String = tmp_hack.Split("$")
                                    If hack_data.Length = 4 Then
                                        If hack_data(0).Length < 101 And _
                                            hack_data(1).Length < 15 And _
                                            hack_data(2).Length < 19 And _
                                            hack_data(3).Equals(Mac) Then
                                            Functions.add_warning_level(cIP)
                                            Database.BanAccount(User_data(0), IS_account, "Thread.RunClient→Ban.me")
                                            Database.BanIP(cIP, "Thread.RunClient→Ban.me")
                                            Database.banClient(Mac, cIP, "Thread.RunClient→Ban.me")
                                            IO.File.AppendAllLines("hack names.txt", New String() _
                                            {"-------------------- Hack Report --------------------", _
                     "Hack: " & hack_data(0), "Character: " & hack_data(1), Functions.rank(Int(User_data(2)), IS_account) & User_data(0), "IP: " & cIP, "Date: " & Date.Now.ToString, _
                                             "-------------------- End of Hack Report -------------", ""})
                                            Database.Report(Functions.rank(Int(User_data(2)), IS_account), IS_account, hack_data(1), cIP, hack_data(0))
                                            Exit Do
                                        Else
                                            Functions.add_warning_level(cIP)
                                            Database.BanIP(cIP, "Thread.RunClient→Fake.Packet")
                                            Exit Do
                                        End If
                                    Else
                                        Functions.add_warning_level(cIP)
                                        Database.BanIP(cIP, "Thread.RunClient→Fake.Packet")
                                        Exit Do
                                    End If
                                Else
                                    Functions.add_warning_level(cIP)
                                    Database.BanIP(cIP, "Thread.RunClient→Fake.Packet")
                                    Exit Do
                                End If
                            Else
                                Functions.add_warning_level(cIP)
                                Database.BanIP(cIP, "Thread.RunClient→Fake.Packet")
                                Exit Do
                            End If
                        Else
                            Functions.add_warning_level(cIP)
                            Database.BanIP(cIP, "Thread.RunClient→Fake.Packet")
                            Exit Do
                        End If
                    End If
                ElseIf bytesFrom(0).Equals(Byte.MinValue) Then
                    Functions.add_warning_level(cIP)
                    Exit Do
                Else
                    If Not Int(User_data(2)) >= 254 Then
                        Select Case bytesFrom(0)
                            Case AdminPacketCommand.AccountBan
                            Case AdminPacketCommand.ClientBan
                            Case AdminPacketCommand.ClientDisconnect
                            Case AdminPacketCommand.IPBan
                            Case AdminPacketCommand.RefreshPlayerList
                            Case AdminPacketCommand.UnbanAccount
                            Case AdminPacketCommand.UnbanClient
                            Case AdminPacketCommand.UnbanIP
                            Case Else
                                Functions.Show("Invalid Command ID: 0x" & bytesFrom(0).ToString & " from " & rank(Int(User_data(2)), IS_account) & User_data(0) & " - IP: " & cIP, "Thread.RunClient→Validate.CommandID", ConsoleColor.Red)
                                Functions.add_warning_level(cIP)
                                Exit Do
                        End Select
                        Functions.Show("Spoof Admin Command: " & CType(bytesFrom(0), AdminPacketCommand).ToString & " from " & rank(Int(User_data(2)), IS_account) & User_data(0) & " - IP: " & cIP, "Thread.RunClient→Validate.CommandID", ConsoleColor.Red)
                    Else
                        Functions.Show("Invalid Command ID: 0x" & bytesFrom(0).ToString & " from " & rank(Int(User_data(2)), IS_account) & User_data(0) & " - IP: " & cIP, "Thread.RunClient→Validate.CommandID", ConsoleColor.Red)
                    End If
                    Functions.add_warning_level(cIP)
                    Exit Do
                End If
            Loop
            Client.Client.Close()
            Functions.Show(Functions.rank(Int(User_data(2)), IS_account) & User_data(0) & " - IP: " & cIP & "→ Client.Closed", "Thread.RunClient→Abort", ConsoleColor.Yellow)
            Select Case Int(User_data(2))
                Case 255, 254
                    Functions.addplayer(User_data(0) & "$" & User_data(1) & "$" & FullIP & "$" & Mac & "$" & Int(IS_account).ToString, True, False)
                Case Else
                    Functions.addplayer(User_data(0) & "$" & User_data(1) & "$" & FullIP & "$" & Mac & "$" & Int(IS_account).ToString, False, False)
            End Select
            Database.Locator_access(cIP, False)
            Functions.MatchServer_access(cIP, False)
            Database.Login_access(User_data(0), IS_account, False)
            Functions.update_clienttime(FullIP.Replace(":", "."), True)
            Functions.removeclient_clientlist(Client)
            Console.Title = Program.Client_list.Count.ToString & " Connected Clients"
            Threading.Thread.CurrentThread.Abort()
        End Sub
    End Class
End Module
