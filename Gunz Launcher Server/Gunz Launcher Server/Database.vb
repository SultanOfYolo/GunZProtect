Imports System.Data.SqlClient
Imports System.Threading
Module Database
    Dim SQLConn As New SqlConnection
    Function Open_Connection(ByVal Connstr As String) As Boolean
        SQLConn.ConnectionString = Connstr
        Try
            SQLConn.Open()
            Return True
        Catch ex As Exception
            Log("DB connection open failed!, " & ex.Message)
        End Try
        Return False
    End Function
    Function Check_ip(ByVal bip As String) As Boolean
        Dim cmd As SqlDataReader = New SqlCommand() With {.Connection = SQLConn, .CommandText = "SELECT * FROM IPBans WHERE IP = '" & bip & "'"}.ExecuteReader
        Dim result As Boolean = cmd.HasRows
        cmd.Close()
        Return result
    End Function
    Function Login_access(ByVal User As String, ByVal isaccount As Boolean, ByVal addorremove As Boolean) As Boolean
        Dim cmd As New SqlCommand With {.Connection = SQLConn}
        Dim datareader As SqlDataReader
        Dim result As Boolean
        If isaccount Then
            If addorremove Then
                cmd.CommandText = "UPDATE Account SET LoginAllowed = 1 WHERE UserID = '" & User & "' and LoginAllowed = 0"
                datareader = cmd.ExecuteReader
                result = Convert.ToBoolean(Int(datareader.RecordsAffected))
                datareader.Close()
                Return result
            Else
                cmd.CommandText = "UPDATE Account SET LoginAllowed = 0 WHERE UserID = '" & User & "' and LoginAllowed = 1"
                cmd.ExecuteNonQuery()
                Return True
            End If
        Else
            Dim Aid As Integer
            cmd.CommandText = "SELECT AID FROM Character WHERE Name = '" & User & "'"
            datareader = cmd.ExecuteReader
            datareader.Read()
            Aid = datareader(0)
            datareader.Close()
            If addorremove Then
                cmd.CommandText = "UPDATE Account SET LoginAllowed = 1 WHERE AID = " & Aid.ToString & " and LoginAllowed = 0"
                datareader = cmd.ExecuteReader
                result = Convert.ToBoolean(Int(datareader.RecordsAffected))
                datareader.Close()
                Return result
            Else
                cmd.CommandText = "UPDATE Account SET LoginAllowed = 0 WHERE AID = " & Aid.ToString & " and LoginAllowed = 1"
                cmd.ExecuteNonQuery()
                Return True
            End If
        End If
    End Function
    Sub Locator_access(ByVal pip As String, ByVal addorremore As Boolean)
        Dim mycmd As New SqlCommand With {.Connection = SQLConn}
        If addorremore Then
            mycmd.CommandText = "INSERT INTO LoginIP (IP) VALUES ('" & pip & "')"
        Else
            mycmd.CommandText = "DELETE TOP (1) FROM LoginIP WHERE IP = '" & pip & "'"
        End If
        mycmd.ExecuteNonQuery()
    End Sub
    Sub unbanIP(ByVal bip As String, ByVal calledby As String)
        Dim cmd As SqlDataReader = New SqlCommand() With {.Connection = SQLConn, .CommandText = "SELECT * FROM IPBans WHERE ip = '" & bip & "'"}.ExecuteReader
        Dim result As Boolean = cmd.HasRows
        cmd.Close()
        If result Then
            Dim Bancmd As New SqlCommand With {.Connection = SQLConn, .CommandText = "DELETE FROM IPBans WHERE ip = '" & bip & "'"}
            Bancmd.ExecuteNonQuery()
        End If
        Dim Bannedip As New List(Of String)
        Bannedip.AddRange(IO.File.ReadAllLines("banned ip.txt"))
        If Bannedip.Contains(bip) Then
            Bannedip.Remove(bip)
            If Bannedip.Count < 1 Then
                IO.File.WriteAllText("banned ip.txt", "")
            Else
                IO.File.WriteAllLines("banned ip.txt", Bannedip)
            End If
        End If
        addbannedstream(bip, False)
        Show("IP: " & bip & " has been Unbanned!", calledby & "→Database.UnbanIP", ConsoleColor.Red)
    End Sub
    Sub BanIP(ByVal bIP As String, ByVal calledby As String)
        Dim cmd As SqlDataReader = New SqlCommand() With {.Connection = SQLConn, .CommandText = "SELECT * FROM IPBans WHERE ip = '" & bIP & "'"}.ExecuteReader
        Dim result As Boolean = cmd.HasRows
        cmd.Close()
        If Not result Then
            Dim Bancmd As New SqlCommand With {.Connection = SQLConn, .CommandText = "INSERT INTO IPBans (ip, Opened) VALUES ('" & bIP & "', 1)"}
            Bancmd.ExecuteNonQuery()
        End If
        If Not IO.File.ReadAllLines("banned ip.txt").Contains(bIP) Then
            IO.File.AppendAllLines("banned ip.txt", New String() {bIP})
        End If
        'addbannedstream(bIP, True)
        Show("IP: " & bIP & " has been Banned!", calledby & "→Database.BanIP", ConsoleColor.Red)
    End Sub
    Sub banClient(ByVal Mac As String, ByVal bIP As String, ByVal calledby As String)
        Dim cmd As SqlDataReader = New SqlCommand() With {.Connection = SQLConn, .CommandText = "SELECT * FROM Bannedhardware WHERE Mac = '" & Mac & "'"}.ExecuteReader
        Dim result As Boolean = cmd.HasRows
        cmd.Close()
        If Not result Then
            Dim Bancmd As New SqlCommand With {.Connection = SQLConn, .CommandText = "INSERT INTO Bannedhardware (Mac, IP) VALUES ('" & Mac & "', '" & bIP & "')"}
            Bancmd.ExecuteNonQuery()
        End If
        If Not IO.File.ReadAllLines("banned mac.txt").Contains(Mac) Then
            IO.File.AppendAllLines("banned mac.txt", New String() {Mac})
        End If
        Show("MAC: " & Mac & " has been Banned!", calledby & "→Database.BanHardware", ConsoleColor.Red)
    End Sub
    Sub unbanClient(ByVal mac As String, ByVal bip As String, ByVal calledby As String)
        Dim cmd As SqlDataReader = New SqlCommand() With {.Connection = SQLConn, .CommandText = "SELECT * FROM Bannedhardware WHERE Mac = '" & mac & "'"}.ExecuteReader
        Dim result As Boolean = cmd.HasRows
        cmd.Close()
        If result Then
            Dim Bancmd As New SqlCommand With {.Connection = SQLConn, .CommandText = "DELETE FROM Bannedhardware WHERE Mac = '" & mac & "' or IP = '" & bip & "'"}
            Bancmd.ExecuteNonQuery()
        End If
        Dim Bannedmac As New List(Of String)
        Bannedmac.AddRange(IO.File.ReadAllLines("banned mac.txt"))
        If Bannedmac.Contains(mac) Then
            Bannedmac.Remove(mac)
            If Bannedmac.Count > 0 Then
                IO.File.WriteAllLines("banned mac.txt", Bannedmac) '.ToArray)
            Else
                IO.File.WriteAllText("banned mac.txt", "")
            End If
        End If
        'addbannedstream(bip, False)
        Show("MAC: " & mac & " - With IP: " & bip & " has been Unbanned!", calledby & "→Database.UnbanClient", ConsoleColor.Red)
    End Sub
    Sub UnbanAccount(ByVal User As String, ByVal isacc As Boolean, ByVal calledby As String)
        Dim cmd As SqlCommand, str As String
        If isacc Then
            str = "The Account: " & User
            cmd = New SqlCommand With {.Connection = SQLConn, .CommandText = "UPDATE Account SET UGradeID = 0 WHERE UserID = '" & User & "'"}
        Else
            str = "The Character: " & User
            Dim Aid As Integer
            Dim datareader As SqlDataReader = New SqlCommand() With {.Connection = SQLConn, .CommandText = "SELECT AID FROM Character WHERE Name = '" & User & "' and DeleteFlag = 0"}.ExecuteReader
            If datareader.HasRows Then
                datareader.Read()
                Aid = Int(datareader(0))
            End If
            datareader.Close()
            cmd = New SqlCommand With {.Connection = SQLConn, .CommandText = "UPDATE Account SET UGradeID = 0 WHERE AID = " & Aid}
        End If
        cmd.ExecuteNonQuery()
        Show(str & " has been Unbanned!", calledby & "→Database.UnbanAccount", ConsoleColor.Red)
    End Sub
    Function is_mac_db_banned(ByVal mac As String) As Boolean
        Dim mc As SqlDataReader = New SqlCommand() With {.Connection = SQLConn, .CommandText = "SELECT * FROM Bannedhardware WHERE Mac = '" & mac & "'"}.ExecuteReader
        Dim damn As Boolean = mc.HasRows
        mc.Close()
        Return damn
    End Function
    Function Is_mac_ok(ByVal mac As String) As Boolean
        Dim bmacs As String() = IO.File.ReadAllLines("banned mac.txt")
        If Not bmacs.Contains(mac) Then
            If Not is_mac_db_banned(mac) Then
                Return True
            End If
        End If
        Return False
    End Function
    Sub Report(ByVal user As String, ByVal isacc As Boolean, ByVal character As String, ByVal bip As String, ByVal hackused As String)
        Dim cmd As New SqlCommand With {.Connection = SQLConn, .CommandText = "INSERT INTO Reports (LoginUser, Character, IP, Cheat, hacktime) " & _
                                       "VALUES ('" & user & "', '" & character & "', '" & bip & "', '" & hackused & "', '" & Date.Now & "')"}
        cmd.ExecuteNonQuery()
    End Sub
    Function is_ip_db_banned(ByVal bIP As String) As Boolean
        Dim mc As SqlDataReader = New SqlCommand() With {.Connection = SQLConn, .CommandText = "SELECT * FROM IPBans WHERE IP = '" & bIP & "'"}.ExecuteReader
        Dim damn As Boolean = mc.HasRows
        mc.Close()
        Return damn
    End Function
    Function Is_ip_ok(ByVal bIP As String) As Boolean
        Dim bips As String() = IO.File.ReadAllLines("banned ip.txt")
        If Not bips.Contains(bIP) Then
            Return Not is_ip_db_banned(bIP)
        End If
        Return False
    End Function
    Sub BanAccount(ByVal User As String, ByVal isacc As Boolean, ByVal calledby As String)
        Dim cmd As SqlCommand, str As String
        If isacc Then
            str = "The Account: " & User
            cmd = New SqlCommand With {.Connection = SQLConn, .CommandText = "UPDATE Account SET UGradeID = 253 WHERE UserID = '" & User & "'"}
            cmd.ExecuteNonQuery()
        Else
            str = "The Character: " & User
            cmd = New SqlCommand With {.Connection = SQLConn, .CommandText = "SELECT AID FROM Character WHERE DeleteFlag = 0 and Name = '" & User & "'"}
            Dim Aid As Integer
            Dim aidq As SqlDataReader = cmd.ExecuteReader
            If aidq.HasRows Then
                aidq.Read()
                Aid = Int(aidq(0))
            End If
            aidq.Close()
            If Aid > 0 Then
                Dim bcmd As SqlCommand = New SqlCommand() With {.Connection = SQLConn, .CommandText = "UPDATE Account SET UGradeID = 253 WHERE AID = " & Aid}
                bcmd.ExecuteNonQuery()
            End If
        End If
        Functions.Show(str & " has been Banned!", calledby & "→Database.BanAccount", ConsoleColor.Red)
    End Sub
    Function check_login(ByVal user As String, ByVal pass As String, ByVal isacc As Boolean) As Int16
        Dim ck As SqlDataReader
        If isacc Then
            ck = New SqlCommand() With {.Connection = SQLConn, .CommandText = "SELECT UGradeID, Opened FROM Account WHERE UserID = '" & user & "' and Password = '" & pass & "'"}.ExecuteReader
        Else
            ck = New SqlCommand() With {.Connection = SQLConn, .CommandText = "SELECT a.UGradeID, a.Opened FROM Account a inner join Character c on a.AID = c.AID WHERE c.Deleteflag = 0 and c.Name = '" & user & "' and a.Password = '" & pass & "'"}.ExecuteReader
        End If
        If ck.HasRows Then
            ck.Read()
            Dim Opened As Boolean = Convert.ToBoolean(ck(1))
            If Not Opened Then
                ck.Close()
                Return 1 ' Account Not active
            Else
                Dim gr As Int16 = Int(ck(0))
                ck.Close()
                Return gr
            End If
        Else ' not found, incorrect user/pass
            ck.Close()
            Return 6
        End If
    End Function
    Sub clean_loginallowed()
        Dim cl As New SqlCommand() With { _
                .Connection = SQLConn, _
                .CommandText = "UPDATE Account SET LoginAllowed = 0 Where Opened = 1" _
            }
        cl.ExecuteNonQuery()
    End Sub
    Sub Clean_LoginIP()
        Dim query As SqlDataReader = New SqlCommand() With { _
                .Connection = SQLConn, _
                .CommandText = "DELETE FROM LoginIP" _
            }.ExecuteReader
        query.Close()
        Show("LoginIP Table Cleared, " & query.RecordsAffected.ToString & " rows affected!", "Database.CleanLoginIP")
    End Sub
    Public Sub DB__Thread() 'used da decompiled code of me LOLOLOL
        Dim millisecondsTimeout As Integer = &H112A880
        Dim strArray2 As String() = New String() {"ServerLog", "GameLog", "QuestGameLog", "KillLog", "PlayerLog", "BringAccountItemLog", "CharacterMakingLog", "ClanGameLog", "ConnLog"}
        Dim strArray As String() = New String() {"ftp://backup.ssgunz.com/", "db_backup_user@crazygunz.tk", "Z~r%?s^DTFxI"}
        Functions.Show(String.Concat(New String() {"DB Backup Thread Alive, Host: ", strArray(0), ", Protocol: FTP, Delay: ", millisecondsTimeout.ToString, " Mill./", (((CDbl(millisecondsTimeout) / 1000) / 60) / 60).ToString, " Hours"}), "Database.DB__Thread", ConsoleColor.Red)
Label_0119:
        Thread.Sleep(millisecondsTimeout)
        millisecondsTimeout = &H112A880
        Dim command As New SqlCommand With { _
            .Connection = SQLConn _
        }
        Dim num4 As Short = CShort((strArray2.Length - 1))
        Dim i As Short = 0
        Do While (i <= num4)
            command.CommandText = ("DELETE FROM " & strArray2(i))
            Try
                command.ExecuteNonQuery()
            Catch exception1 As Exception
            End Try
            i = CShort((i + 1))
        Loop
        Dim command2 As New SqlCommand With { _
            .Connection = SQLConn _
        }
        Dim str As String = String.Concat(New String() {DateTime.Now.Day.ToString, ".", DateTime.Now.Month.ToString, ".", DateTime.Now.Year.ToString, "_", DateTime.Now.Hour.ToString, "-", DateTime.Now.Minute.ToString})
        command2.CommandText = ("BACKUP DATABASE [GunzDB] TO  DISK = N'C:\backups\GunzDB_" & str & ".bak' WITH NOFORMAT, NOINIT,  NAME = N'GunzDB-Full Database Backup', SKIP, NOREWIND, NOUNLOAD,  STATS = 10")
        Try
            command2.ExecuteNonQuery()
        Catch exception3 As Exception
            Functions.Show("Database Backup Command execution failed!!!", "Database.DB__Thread", ConsoleColor.Red)
            Functions.Log(("Database Backup command execution failed, " & exception3.Message))
            millisecondsTimeout = &H927C0
            GoTo Label_0119
        End Try
        Try
            My.Computer.Network.UploadFile(("C:\backups\GunzDB_" & str & ".bak"), (strArray(0) & "GunzDB_" & str & ".bak"), strArray(1), strArray(2))
            Functions.Show(String.Concat(New String() {"Database Backuped as: GunzDB_", str, ".bak, size: ", (CDbl(My.Computer.FileSystem.GetFileInfo(("C:\backups\GunzDB_" & str & ".bak")).Length) / 1024).ToString, " MB"}), "Database.DB__Thread", ConsoleColor.Green)
        Catch exception4 As Exception
            Functions.Log(("DB Backup Thread error: " & exception4.Message))
            Functions.Show("Error in uploading the Database Backup, retry in 10 mins!", "Database.DB__Thread", ConsoleColor.Red)
            millisecondsTimeout = &H927C0
        End Try
        GoTo Label_0119
    End Sub
End Module
