Imports System.Text
Imports System.IO
Imports System.Net.Sockets
Imports System.Security.Cryptography

Module Functions
    Public IP As String
    Public Port As Int16
    Public PingLimit As Int16
    Public Server_str As Byte()
    Dim Warning_IP(255, 255, 255) As Int16
    Dim warning_date(255, 255, 255) As Date
    Dim SQLArray() As String = {"chr(", "chr=", "chr ", " chr", "wget ", " wget", "wget(", _
    "cmd=", " cmd", "cmd ", "rush=", " rush", "rush ", _
    "union ", " union", "union(", "union=", "echr(", " echr", "echr ", "echr=", _
    "esystem(", "esystem ", "cp ", " cp", "cp(", "mdir ", " mdir", "mdir(", _
    "mcd ", "mrd ", "rm ", " mcd", " mrd", " rm", _
    "mcd(", "mrd(", "rm(", "mcd=", "mrd=", "mv ", "rmdir ", "mv(", "rmdir(", _
    "chmod(", "chmod ", " chmod", "chmod(", "chmod=", "chown ", "chgrp ", "chown(", "chgrp(", _
    "locate ", "grep ", "locate(", "grep(", "diff ", "kill ", "kill(", "killall", _
    "passwd ", " passwd", "passwd(", "telnet ", "vi(", "vi ", _
    "insert into", "select ", "fopen", "fwrite", " like", "like ", _
    "$_request", "$_get", "$request", "$get", ".system", "HTTP_PHP", "&aim", " getenv", "getenv ", _
    "new_password", "&icq", "/etc/password", "/etc/shadow", "/etc/groups", "/etc/gshadow", _
    "HTTP_USER_AGENT", "HTTP_HOST", "/bin/ps", "wget ", "uname\x20-a", "/usr/bin/id", _
    "/bin/echo", "/bin/kill", "/bin/", "/chgrp", "/chown", "/usr/bin", "g\+\+", "bin/python", _
    "bin/tclsh", "bin/nasm", "perl ", "traceroute ", "ping ", ".pl", "lsof ", _
    "/bin/mail", ".conf", "motd ", "HTTP/1.", ".inc.php", "config.php", "cgi-", ".eml", _
    "file\://", "window.open", "<script>", "javascript\://", "img src", "img src", ".jsp", "ftp.exe", _
    "xp_enumdsn", "xp_availablemedia", "xp_filelist", "xp_cmdshell", "nc.exe", ".htpasswd", _
    "servlet", "/etc/passwd", "wwwacl", "~root", "~ftp", ".js", ".jsp", "admin_", ".history", _
    "bash_history", ".bash_history", "~nobody", "server-info", "server-status", "reboot ", "halt ", _
    "powerdown ", "/home/ftp", "/home/www", "secure_site, ok", "chunked", "org.apache", "/servlet/con", _
    "<script", "update ", "select ", "/robot.txt", "/perl", "mod_gzip_status", "db_mysql.inc", ".inc", "select from", _
    "drop ", "declare", "delete ", "insert", "getenv", "http_", "_php", "php_", "phpinfo()", "<?php", "?>", "sql="}
    Sub Load_settings()
        Dim info() As String = IO.File.ReadAllText("info.ini").Split(vbCrLf)
        IP = info(0).Split("=")(1).Split(":")(0)
        Port = Int(info(0).Split("=")(1).Split(":")(1))
        PingLimit = Int(info(1).Split("=")(1))
        Dim MailServer As Boolean = Convert.ToBoolean(Int(info(2).Split("=")(1)))
        Dim Cleaner As Boolean = Convert.ToBoolean(Int(info(3).Split("=")(1)))
        If Database.Open_Connection(info(4).Split(":")(1)) Then
            Show("Database Connection Success", "Funtions.Load_Settings", ConsoleColor.Green)
            Database.Clean_LoginIP()
        Else
            Show("Database Connection Failed!!!!!!!!!", "Funtions.Load_Settings", ConsoleColor.Red)
        End If
        Console.Title = "0 Connected Clients"
        If IO.File.Exists("server.rsz") Then
            Show("Server Parameters loaded successfully!", "Functions.Load_Settings", ConsoleColor.Green)
            Server_str = Encoding.ASCII.GetBytes(IO.File.ReadAllText("server.rsz"))
        Else
            Show("Server Parameters Failed to load!!!!!!!!!!!!!!!!!!!!!!!!", "Functions.Load_Settings", ConsoleColor.Red)
        End If
        Dim bBanIP As String() = IO.File.ReadAllLines("banned ip.txt")
        For r As Int16 = 0 To bBanIP.Length - 1
            addbannedstream(Int(bBanIP(r).Replace(".", "")), True)
        Next
        Show((Program.BannedStreamList.Count - 1).ToString & " Banned IP Addresses Loaded into System Stream Filter!", "Functions.Load_Settings")
        If MailServer Then
            Dim Timer1 As System.Timers.Timer = New System.Timers.Timer
            AddHandler Timer1.Elapsed, AddressOf Ticker_email
            Timer1.Interval = New Random().Next(3000, 6000)
            Timer1.Start()
            Show("Mail Server Started with delay: " & (Timer1.Interval / 1000).ToString & " seconds", "Functions.Load_Settings")
        End If
        If Cleaner Then
            Dim Timer2 As System.Timers.Timer = New System.Timers.Timer
            AddHandler Timer2.Elapsed, AddressOf Ticker_cleaner
            Timer2.Interval = New Random().Next(15000, 45000)
            Timer2.Start()
            Show("Appache Cleaner Started with delay: " & (Timer2.Interval / 1000).ToString & " seconds", "Functions.Load_Settings")
        End If
        Database.clean_loginallowed()
        Show("Ping Limitaion set to max: " & PingLimit, "Functions.Load_Settings")
        Dim ClientListTimer As System.Timers.Timer = New System.Timers.Timer
        AddHandler ClientListTimer.Elapsed, AddressOf ClientListTimersub
        ClientListTimer.Interval = New Random().Next(6000, 14000)
        ClientListTimer.Start()
        Show("Client Key Generator started with delay " & (ClientListTimer.Interval / 1000).ToString & " seconds", "Functions.Load_Settings")
        IO.File.WriteAllText("access.txt", "")
        Dim files() As IO.FileInfo = New IO.DirectoryInfo(CurDir() & "\Clients").GetFiles
        For t As Int16 = 0 To files.Length - 1
            IO.File.Delete(files(t).FullName)
        Next
        Dim files1() As IO.FileInfo = New IO.DirectoryInfo(CurDir() & "\Clients\Codes").GetFiles
        For t As Int16 = 0 To files1.Length - 1
            IO.File.Delete(files1(t).FullName)
        Next
    End Sub
    Function _Decrypt(ByVal strText As String, ByVal sDecrKey As String) As String
        Dim IV() As Byte = {&H12, &H34, &H56, &H78, &H90, &HAB, &HCD, &HEF}
        Dim inputByteArray(strText.Length) As Byte
        Try
            Dim byKey() As Byte = System.Text.Encoding.UTF8.GetBytes(Left(sDecrKey, 8))
            Dim des As New DESCryptoServiceProvider
            inputByteArray = Convert.FromBase64String(strText)
            Dim ms As New MemoryStream
            Dim cs As New CryptoStream(ms, des.CreateDecryptor(byKey, IV), CryptoStreamMode.Write)
            cs.Write(inputByteArray, 0, inputByteArray.Length)
            cs.FlushFinalBlock()
            Dim encoding As System.Text.Encoding = System.Text.Encoding.UTF8
            Return encoding.GetString(ms.ToArray())
        Catch ex As Exception
            Return Nothing
        End Try
    End Function
    Function Clean(ByVal str As String, ByVal bIP As String, ByVal from As String) As String
        Dim TChar As String = str.ToLower '.Trim().Replace("'", "''")
        For i As Int16 = 0 To SQLArray.Length - 1
            TChar = TChar.Replace(SQLArray(i), Nothing)
        Next
        TChar = TChar.Trim()
        If Not TChar.Equals(str.ToLower) Then
            Show(bIP & " → [" & str & "] SQL Injected, filtered [" & TChar & "]", from & "→Functions.Clean", ConsoleColor.Red)
            Log(bIP & " → [" & str & "] SQL Injected, filtered [" & TChar & "]")
            Return "#"
        Else : Return str
        End If
    End Function
    Sub Log(ByVal v As String)
        IO.File.AppendAllText("ALog.txt", "[" & Date.Now & "] " & v & vbCrLf)
    End Sub
    Public Sub Show(ByVal str As String, ByVal from As String, Optional ByVal color As ConsoleColor = ConsoleColor.White)
        Console.ForegroundColor = color
        Console.WriteLine(" [" & TimeOfDay & "] - │" & from & "│ →" & vbTab & str)
    End Sub
    Sub addbannedstream(ByVal bip As String, ByVal addremove As Boolean)
        SyncLock Program.BannedStreamList
            If addremove Then
                Program.BannedStreamList.Add(Int(bip.Replace(".", "")))
            Else
                Program.BannedStreamList.Remove(Int(bip.Replace(".", "")))
            End If
        End SyncLock
    End Sub
    Function SEND(ByVal Name As String, ByVal UserID As String, ByVal Password As String, ByVal sq As String, _
                   ByVal sa As String, ByVal Aid As String, _
                   ByVal hash As String, ByVal email As String)
        Dim Emailer As New Net.WebClient
        Try
            Dim replyserve As String = Emailer.DownloadString("http://server.ssgunz.com/amail.php?Name=" & Name & _
     "&user=" & UserID & "&pass=" & Password & "&SecurityQuestion=" & sq & "&SecurityAnswer=" & sa & "&AID=" & Aid & _
     "&R4=" & hash & "&email=" & email)
            Return True
        Catch ex As Exception
            Log("Failed Mail to: " & email & vbCrLf & ex.Message)
            Return False
        End Try
    End Function
    Function RandomString(ByVal size As Integer) As String
        'Return "UIB78dsfZTFRbhjdsf" 'until i write a new packet cryption algorithm
        Dim builder As New StringBuilder()
        Dim random As New Random()
        Dim ch As Char
        Dim i As Integer
        For i = 0 To size - 1
            ch = Convert.ToChar(Convert.ToInt32((26 * random.NextDouble() + 65)))
            builder.Append(ch)
        Next
        Return builder.ToString()
    End Function 'RandomString
    Sub update_clienttime(ByVal clientaddr As String, Optional ByVal del As Boolean = False)
        If Not del Then
            IO.File.WriteAllText("Clients\" & clientaddr, Date.Now)
        Else
            If IO.File.Exists("Clients\" & clientaddr) Then IO.File.Delete("Clients\" & clientaddr)
            If IO.File.Exists("Clients\Codes\" & clientaddr) Then IO.File.Delete("Clients\Codes\" & clientaddr)
        End If
    End Sub
    Function check_clienttime(ByVal Clientaddr As String) As Boolean
        If IO.File.Exists("Clients\" & Clientaddr) Then
            Dim Client_time As Date = Convert.ToDateTime(IO.File.ReadAllText("Clients\" & Clientaddr))
            If Client_time.AddSeconds(30) < Date.Now Then Return False
            Return True
        End If
        Return False
    End Function
    Sub removeclient_clientlist(ByVal pClient As TcpClient)
        SyncLock Program.Client_list
            Program.Client_list.Remove(pClient)
        End SyncLock
    End Sub
    Sub add_ClientList(ByVal pClient As TcpClient)
        SyncLock Program.Client_list
            Program.Client_list.Add(pClient)
        End SyncLock
    End Sub
    Function get_clienttime(ByVal Clientaddr As String) As Date
        If IO.File.Exists("Clients\" & Clientaddr) Then Return IO.File.ReadAllText("Clients\" & Clientaddr)
        Return Nothing
    End Function
    Public Function GetFilesByExtensions(ByVal strPath As String, ByVal colExtensions() As String) As FileInfo()
        ' The collection we will be using to store the file information in
        Dim tmpCollection As New Collection

        ' Loop through all the extensions and files, and add them to the collection.
        For Each strExtension As String In colExtensions
            For Each aFile As FileInfo In New DirectoryInfo(strPath).GetFiles(strExtension)
                tmpCollection.Add(aFile)
            Next
        Next

        ' Variables to convert the collection to type FileInfo (more convenient to the programmer)
        Dim tmpFiles(tmpCollection.Count - 1) As FileInfo, i As Integer = 0

        ' Loop through the collection and convert it into FileInfo
        For Each aFile As FileInfo In tmpCollection
            tmpFiles(i) = aFile
            i += 1
        Next

        ' Return the files
        Return tmpFiles
    End Function
    Public Sub Ticker_email()
        Try
            Dim files() As IO.FileInfo = GetFilesByExtensions("C:\AppServ\www\66432modctzz1258a\email", {"*.txt"})
            If files.Length > 0 Then
                For i As Int16 = 0 To files.Length - 1
                    Dim file_data() As String = IO.File.ReadAllText(files(i).FullName).Split(",")
                    If SEND(file_data(0), file_data(1), file_data(2), file_data(3), file_data(4), file_data(5), file_data(6), file_data(7)) Then
                        Show("Activation Mail Sent to: " & file_data(7), "Mail Ticker")
                        Try
                            IO.File.Delete(files(i).FullName)
                        Catch ex As Exception
                            Show("Failed Deletion to: " & files(i).Name, "Mail Ticker", ConsoleColor.Red)
                        End Try
                    Else
                        Show("Mail Failed to: " & file_data(7), "Mail Ticker", ConsoleColor.Red)
                    End If
                Next
            End If
        Catch ex As Exception
            Log(ex.Message)
            Show(ex.Message, "Mail Ticker", ConsoleColor.Red)
        End Try
    End Sub
    Public Sub Ticker_cleaner()
        Try
            Dim files() As IO.FileInfo = New IO.DirectoryInfo("C:\Documents and Settings\Administrator\Local Settings\Temp\1").GetFiles()
            'Dim Count As Int64 = 0
            For i As Int16 = 0 To files.Length - 1
                If files(i).CreationTime.AddMinutes(30) < Date.Now Then
                    Try
                        'Count += 1
                        IO.File.Delete(files(i).FullName)
                    Catch ' ex As Exception
                        'smsg("Appache Cleaner: " & ex.Message, True)
                    End Try
                End If
            Next
            ' If Not Count = 0 Then smsg("Appache Sessions cleaned, " & Count & " files deleted", True)
        Catch ex As Exception
            Log(ex.Message)
            'smsg(ex.Message, True)
        End Try
    End Sub
    Function get_gstring(ByVal MyBytes As Byte()) As String
        If MyBytes(1) = 0 Then Return ""
        Return System.Text.Encoding.ASCII.GetString(MyBytes, 1, Array.IndexOf(MyBytes, Byte.MinValue) - 1)
    End Function
    Function get_3d_ip(ByVal IP As String) As Int16()
        Dim Int_IP(2) As Int16
        For i As Int16 = 0 To 2
            Int_IP(i) = Int(IP.Split(".")(i))
        Next
        Return Int_IP
    End Function
    Function get_warning_level(ByVal pIP As String) As Int16
        Dim Int_IP() As Int16 = get_3d_ip(pIP)
        Return Warning_IP(Int_IP(0), Int_IP(1), Int_IP(2))
    End Function
    Sub add_warning_level(ByVal pIP As String)
        Dim Int_IP() As Int16 = get_3d_ip(pIP)
        Warning_IP(Int_IP(0), Int_IP(1), Int_IP(2)) += 1
        warning_date(Int_IP(0), Int_IP(1), Int_IP(2)) = Date.Now
    End Sub
    Function get_warning_time(ByVal IP As String) As Date
        Dim Int_IP() As Int16 = get_3d_ip(IP)
        Return warning_date(Int_IP(0), Int_IP(1), Int_IP(2))
    End Function
    Function get_clientcode(ByVal Location As String) As String
        If IO.File.Exists("Clients\Codes\" & Location) Then Return IO.File.ReadAllText("Clients\Codes\" & Location)
        Return Nothing
    End Function
    Sub DC(ByVal obf As TcpClient)
        Try
            obf.Client.Close()
        Catch
        End Try
    End Sub
    Sub set_clientcode(ByVal code As String, ByVal location As String)
        IO.File.WriteAllText("Clients\Codes\" & location, code)
    End Sub
    Sub Send_Packet(ByVal cmd As Byte, ByVal data As Byte(), ByVal disClient As TcpClient, ByVal from As String)
        Dim ID_byte(data.Length) As Byte, serverStream As NetworkStream, ip As String = "IP.Error"
        ID_byte(0) = cmd
        data.CopyTo(ID_byte, 1)
        Try
            ip = disClient.Client.RemoteEndPoint.ToString
            serverStream = disClient.GetStream()
            serverStream.Write(ID_byte, 0, ID_byte.Length)
            serverStream.Flush()
        Catch ex As Exception
            Show("Error sending Packet 0x" & cmd & " to Client: " & ip, from & "→Send.Packet", ConsoleColor.Red)
            Exit Sub
        End Try
        If Not cmd.Equals(AdminPacketCommand.AddConnectedPlayer) Then
            If Not cmd.Equals(AdminPacketCommand.RemoveConnectedPlayer) Then
                If Not cmd.Equals(Packet.Key) Then
                    Show("Packet (0x" & Conversion.Hex(Int(cmd)).ToString & ") Sent to Client: " & ip, from & "→Send.Packet", ConsoleColor.Green)
                End If
            End If
        End If

    End Sub
    Sub reset_warning_level(ByVal pip As String)
        Dim intip As Int16() = get_3d_ip(pip)
        Warning_IP(intip(0), intip(1), intip(2)) = 0
        warning_date(intip(0), intip(1), intip(2)) = Date.Now.AddMinutes(-30)
    End Sub
    Public Class Process_Login
        Dim User_pass As String
        Dim pClient As TcpClient
        Dim pIP As String
        Dim fullIP As String
        Sub StartProcess(ByVal User_Pass1 As String, ByVal pClient1 As TcpClient, ByVal pIP1 As String)
            Me.User_pass = User_Pass1
            Me.pClient = pClient1
            Me.fullIP = pIP1
            Me.pIP = pIP1.Split(":")(0)
            Dim Loginprocessor As New Threading.Thread(AddressOf Process_LoginPacket)
            Loginprocessor.Start()
        End Sub
        Sub Process_LoginPacket()
            Dim ktime As Date = Functions.get_warning_time(pIP), klvl As Int16 = Functions.get_warning_level(pIP)
            If ktime.AddMinutes(30) < Date.Now Or klvl < 6 Then
                If ktime.AddMinutes(30) < Date.Now And klvl > 4 Then
                    Functions.reset_warning_level(pIP)
                End If
                Dim data As String = Functions._Decrypt(User_pass, "Login.Info")
                If data.Length > 0 And data.Contains("$") Then
                    Dim User_data As String() = data.Split("$")
                    If User_data.Length = 5 Then
                        If Functions.fx(User_data(0).Length, User_data(1).Length).Equals(User_data(3)) Then
                            If User_data(0).Length > 2 And User_data(0).Length < 19 And User_data(1).Length > 5 And User_data(1).Length < 65 And (User_data(2).Equals("1") Or User_data(2).Equals("0")) Then
                                If Validate_mac(User_data(4)) Then
                                    If Functions.Clean(User_data(0), fullIP, "Thread.Process.Login.User").Equals(User_data(0)) And Functions.Clean(User_data(1), fullIP, "Thread.Process.Login.Password").Equals(User_data(1)) Then
                                        If Database.Is_ip_ok(pIP) Then
                                            If Database.Is_mac_ok(User_data(4)) Then
                                                Dim pRank As Int16 = Int(Database.check_login(User_data(0), User_data(1), Convert.ToBoolean(Int(User_data(2)))))
                                                Select Case pRank
                                                    Case 0, 2, 3, 4, 5, 104, 252, 254, 255
                                                        If Database.Login_access(User_data(0), Convert.ToBoolean(Int(User_data(2))), True) Then
                                                            Functions.addbannedplayer(User_data(0) & "$" & User_data(1) & "$" & fullIP & "$" & User_data(4) & "$" & Int(User_data(2)).ToString, False)
                                                            Functions.Send_Packet(Packet.hackinfo, Server_str, pClient, "Thread.Process.Login.HackInfo")
                                                            Functions.Send_Packet(Packet.LoginAcc, New Byte() {pRank}, pClient, "Thread.Process.Login")
                                                            If pRank >= 254 Then
                                                                Functions.addplayer(User_data(0) & "$" & User_data(1) & "$" & fullIP & "$" & User_data(4) & "$" & Int(User_data(2)).ToString, True, True)
                                                            Else
                                                                Functions.addplayer(User_data(0) & "$" & User_data(1) & "$" & fullIP & "$" & User_data(4) & "$" & Int(User_data(2)).ToString, False, True)
                                                            End If
                                                            Dim mClient As New Program.Client_thread
                                                            mClient.Start_Client(New String() {User_data(0), User_data(1), pRank}, fullIP, pClient, User_data(4), Convert.ToBoolean(Int(User_data(2))))
                                                            Threading.Thread.CurrentThread.Abort()
                                                        Else
                                                            Functions.Send_Packet(Packet.FailedLoginAcc, New Byte() {}, pClient, "Thread.Process.Login")
                                                        End If
                                                    Case 1
                                                        Functions.Send_Packet(AccountInfo.NotActive, New Byte() {}, pClient, "Thread.Process.Login")
                                                    Case 6
                                                        Functions.Send_Packet(Packet.FailedLoginAcc, New Byte() {}, pClient, "Thread.Process.Login")
                                                    Case Else ' 253
                                                        Functions.Send_Packet(Packet.AccountBanned, New Byte() {}, pClient, "Thread.Process.Login")
                                                        Functions.addbannedstream(pIP, True)
                                                        Functions.addbannedplayer(User_data(0) & "$" & User_data(1) & "$" & fullIP & "$" & User_data(4) & "$" & Int(User_data(2)).ToString, True)
                                                End Select
                                            Else
                                                Functions.Send_Packet(Packet.hardwarebanned, New Byte() {}, pClient, "Thread.Process.Login")
                                                Functions.addbannedstream(pIP, True)
                                                Functions.addbannedplayer(User_data(0) & "$" & User_data(1) & "$" & fullIP & "$" & User_data(4) & "$" & Int(User_data(2)).ToString, True)
                                            End If
                                        Else
                                            Functions.Send_Packet(Packet.IPbanned, New Byte() {}, pClient, "Thread.Process.Login")
                                            Functions.addbannedstream(pIP, True)
                                            Functions.addbannedplayer(User_data(0) & "$" & User_data(1) & "$" & fullIP & "$" & User_data(4) & "$" & Int(User_data(2)).ToString, True)
                                        End If
                                    End If
                                Else
                                    Log("Invalid Mac: " & User_data(4))
                                End If
                            End If
                        End If
                    End If
                End If
            End If
            Functions.add_warning_level(pIP)
            pClient.Client.Close()
            Threading.Thread.CurrentThread.Abort()
        End Sub
    End Class
    Function fx(ByVal a As Int16, ByVal b As Int16) As String
        Return (1 + (4 * a + 2 * b)).ToString
    End Function
    Function Validate_mac(ByVal mac As String) As Boolean
        If mac.Length = 12 Then
            If System.Text.RegularExpressions.Regex.IsMatch(mac, "\A\b[0-9a-fA-F]+\b\Z") Then
                If Not mac.Equals("000000000000") Then
                    Return True
                End If
            End If
        End If
        Return False
    End Function
    Function Get_Clients() As TcpClient()
        Dim ClientLists As TcpClient()
        SyncLock Program.Client_list
            ClientLists = Program.Client_list.ToArray
        End SyncLock
        Return ClientLists
    End Function
    Function get_playerIds() As String()
        Dim returnarry As String()
        SyncLock Program.User_Password_IP
            returnarry = Program.User_Password_IP.ToArray
        End SyncLock
        Return returnarry
    End Function
    Function Get_allplayers() As String()
        Dim returnarry As New List(Of String)
        returnarry.AddRange(Functions.get_staffplayerIds())
        returnarry.AddRange(Functions.get_bannedplayerIds())
        returnarry.AddRange(Functions.get_playerIds())
        Return returnarry.ToArray
    End Function
    Function get_bannedplayerIds() As String()
        Dim returnarry As String()
        SyncLock Program.bannedaccounts
            returnarry = Program.bannedaccounts.ToArray
        End SyncLock
        Return returnarry
    End Function
    Function get_staffplayerIds() As String()
        Dim returnarry As String()
        SyncLock Program.Staff_User_IP
            returnarry = Program.Staff_User_IP.ToArray
        End SyncLock
        Return returnarry
    End Function
    Sub MatchServer_access(ByVal bIP As String, ByVal addorremove As Boolean)
        If addorremove Then
            Dim tm As New List(Of String)
            tm.Add(bIP)
            Do
                Try
                    IO.File.AppendAllLines("access.txt", tm)
                    Exit Do
                Catch ex As Exception
                End Try
            Loop
        Else
            Dim IP_list As New List(Of String), index As Integer
            IP_list.AddRange(IO.File.ReadAllLines("access.txt"))
            index = IP_list.IndexOf(bIP)
            If index > -1 Then
                If IP_list.Count < 1 Then
                    Do
                        Try
                            IO.File.WriteAllText("access.txt", "")
                            Exit Do
                        Catch ex As Exception
                        End Try
                    Loop
                Else
                    IP_list.RemoveAt(index)
                    Do
                        Try
                            IO.File.WriteAllLines("access.txt", IP_list.ToArray)
                            Exit Do
                        Catch ex As Exception
                        End Try
                    Loop
                End If
            End If
        End If
    End Sub
    Function Formatplayer(ByVal item As String) As String
        Dim data As String() = item.Split("$")
        If Int(data(4)) = 0 Then
            Return "] Character: " & data(0) & " - IP: " & data(2)
        Else
            Return "] UserID: " & data(0) & " - IP: " & data(2)
        End If
    End Function
    Function Get_playerList() As String()
        Dim Players As New List(Of String)
        Dim staff As String() = Functions.get_staffplayerIds()
        Dim normal As String() = Functions.get_playerIds()
        Dim banned As String() = Functions.get_bannedplayerIds()
        For i As Int16 = 0 To staff.Length - 1
            Players.Add("-[Staff" & Formatplayer(staff(i)))
        Next
        For i As Int16 = 0 To banned.Length - 1
            Players.Add("-[Banned" & Formatplayer(banned(i)))
        Next
        For i As Integer = 0 To normal.Length - 1
            Players.Add("-[Normal" & Formatplayer(normal(i)))
        Next
        Return Players.ToArray
    End Function
    Sub RefreshPlayerlist(ByVal cmg As String, ByVal iClient As TcpClient, Optional ByVal boardcast As Boolean = False)
        If boardcast Then
            Dim tmp_players As String() = Get_playerList()
            Dim sendready As New List(Of Byte)
            Dim t As Integer = 0, g As Integer = 0, max As Integer = tmp_players.Count - 1
            Do While g <= max
                If t < 50 Then
                    sendready.AddRange(Functions.Bytes(tmp_players(g) & "%"))
                    If max = g Then
                        Functions.Send_Packet(AdminPacketCommand.AddConnectedPlayer, sendready.ToArray, iClient, "Thread.RunClient→ExecuteCommand.Refresh")
                        Exit Do
                    End If
                    t += 1
                Else
                    sendready.AddRange(Functions.Bytes(tmp_players(g) & "%"))
                    Functions.Send_Packet(AdminPacketCommand.AddConnectedPlayer, sendready.ToArray, iClient, "Thread.RunClient→ExecuteCommand.Refresh")
                    t = 0
                End If
                g += 1
            Loop
        End If
        Functions.Send_Packet(AdminPacketCommand.RefreshPlayerList, Functions.Bytes(cmg), iClient, "Thread.RunClient→ExecuteCommand.Refresh")
    End Sub
    Function _ObjectMac(ByVal bIP As String) As String
        Dim players As String() = Functions.Get_allplayers() 'Functions.get_playerIds()
        For i As Int16 = 0 To players.Length - 1
            If players(i).Split("$")(2).Equals(bIP) Then
                Return players(i).Split("$")(3)
            End If
        Next
        Return ""
    End Function
    Function _ObjectClient(ByVal bIP As String) As TcpClient
        Dim connClients As TcpClient() = Functions.Get_Clients()
        For i As Int16 = 0 To connClients.Length - 1
            Try
                If connClients(i).Client.RemoteEndPoint.ToString.Equals(bIP) Then
                    Return connClients(i)
                End If
            Catch
                Continue For
            End Try
        Next
        Return Nothing
    End Function
    Sub addbannedplayer(ByVal item As String, ByVal addremove As Boolean)
        Dim item_data() As String = item.Split("$")
        SyncLock Program.bannedaccounts
            Dim newlist As List(Of String) = Program.bannedaccounts
            If Not addremove Then
                For q As Int16 = 0 To newlist.Count - 1
                    Dim temp_data() As String = newlist(q).Split("$")
                    If temp_data(0).Equals(item_data(0)) Then
                        If item_data(4).Equals(temp_data(4)) Then
                            Program.bannedaccounts.RemoveAt(q)
                            Exit Sub
                        End If
                    End If
                Next
            Else
                If newlist.Count = 0 Then
                    Program.bannedaccounts.Add(item)
                Else
                    For q As Int16 = 0 To newlist.Count - 1
                        Dim temp_data() As String = newlist(q).Split("$")
                        If temp_data(0).Equals(item_data(0)) Then
                            If item_data(4).Equals(temp_data(4)) Then
                                Exit Sub
                            End If
                        End If
                    Next
                    Program.bannedaccounts.Add(item)
                End If
            End If
        End SyncLock
    End Sub
    Sub addplayer(ByVal item As String, ByVal staff As Boolean, ByVal addremove As Boolean)
        If staff Then
            SyncLock Program.Staff_User_IP
                If addremove Then
                    Program.Staff_User_IP.Add(item)
                Else
                    Program.Staff_User_IP.Remove(item)
                End If
            End SyncLock
        Else
            SyncLock Program.User_Password_IP
                If addremove Then
                    Program.User_Password_IP.Add(item)
                Else
                    Program.User_Password_IP.Remove(item)
                End If
            End SyncLock
        End If
    End Sub
    Function Validate_CommandID(ByVal ID As Byte, ByVal grade As Int16) As Boolean
        Select Case ID
            '  Case Packet.Ping
            Case Packet.Key
            Case Packet.AccountBanned
            Case Else
                If grade >= 254 Then
                    Select Case ID
                        Case AdminPacketCommand.AccountBan
                        Case AdminPacketCommand.ClientBan
                        Case AdminPacketCommand.ClientDisconnect
                        Case AdminPacketCommand.IPBan
                        Case AdminPacketCommand.RefreshPlayerList
                        Case AdminPacketCommand.UnbanAccount
                        Case AdminPacketCommand.UnbanClient
                        Case AdminPacketCommand.UnbanIP
                        Case Else
                            Return False
                    End Select
                Else
                    Return False
                End If
        End Select
        Return True
    End Function
    Function Bytes(ByVal str As String) As Byte()
        Return Encoding.ASCII.GetBytes(str)
    End Function
    Function rank(ByVal grade As Int16, ByVal bob As Boolean) As String
        Dim str As String
        Select Case grade
            Case 255
                str = "Administrator "
            Case 254
                str = "GM/Developer "
            Case 252
                str = "Trial GM "
            Case 104
                str = "Chat Banned "
            Case 3, 4, 5
                str = "Donner "
            Case 2
                str = "Event Winner "
            Case 0
                str = "Normal "
            Case Else
                str = "Unknown! "
        End Select
        If bob Then
            Return str & "(UserID): "
        Else
            Return str & "(Character): "
        End If
    End Function
    Public Class Thread_Protect
        Dim nTime As Date ' = Date.Now
        Dim nIP As String
        Dim nClient As TcpClient
        Dim Protect_thread As Threading.Thread
        Sub Readbyter(ByVal time As Date, ByVal pip As String, ByVal client As TcpClient)
            Me.nIP = pip
            Me.nClient = client
            Me.nTime = time.AddSeconds(1)
            Protect_thread = New Threading.Thread(AddressOf Protect)
            Protect_thread.Start()
        End Sub
        Sub Protect()
            Do
                If nTime < Date.Now Then
                    Functions.add_warning_level(nIP)
                    nClient.Client.Close()
                    Functions.Show("Warning - Client: " & nIP & " disconnected for too long connection time (" & nTime.AddSeconds(-1).Minute.ToString & ":" & nTime.AddSeconds(-1).Second & "/" & Date.Now.Minute.ToString & ":" & Date.Now.Second.ToString & ")", "Program.Main.Thread_Protect.Protect", ConsoleColor.Red)
                    Protect_thread.Abort()
                End If
                System.Threading.Thread.Sleep(100)
            Loop
        End Sub
        Sub endbyter()
            Protect_thread.Abort()
        End Sub
    End Class
End Module