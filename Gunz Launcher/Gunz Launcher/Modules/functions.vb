Imports System.IO
Imports System.Text
Imports System.Net

Module functions
    Public Function GetMD5(ByVal filepath As String) As String
        Try
            Using reader As New System.IO.FileStream(filepath, IO.FileMode.Open, IO.FileAccess.Read)
                Using md5 As New System.Security.Cryptography.MD5CryptoServiceProvider
                    Dim hash() As Byte = md5.ComputeHash(reader)
                    Return ByteArrayToString(hash)
                End Using
            End Using
        Catch
            Return ""
        End Try
    End Function
    Private Function ByteArrayToString(ByVal arrInput() As Byte) As String
        Try
            Dim sb As New System.Text.StringBuilder(arrInput.Length * 2)
            For i As Integer = 0 To arrInput.Length - 1
                sb.Append(arrInput(i).ToString("X2"))
            Next
            Return sb.ToString().ToUpper
        Catch
            Return ""
        End Try
    End Function
    Function getmac() As String
        Dim theNetworkInterfaces() As System.Net.NetworkInformation.NetworkInterface = System.Net.NetworkInformation.NetworkInterface.GetAllNetworkInterfaces()
        For Each currentInterface As System.Net.NetworkInformation.NetworkInterface In theNetworkInterfaces
            If currentInterface.OperationalStatus = Net.NetworkInformation.OperationalStatus.Up And Not currentInterface.Name = "Hamachi" Then
                Return currentInterface.GetPhysicalAddress().ToString()
            End If
        Next
        Log("Error while getting NetworkInterface information!")
        MsgBox("Error while getting NetworkInterface information!", MsgBoxStyle.Exclamation)
        End
    End Function
    Function Getcharname() As String
        Try
            Return IO.File.ReadAllText("lastchar.dat")
        Catch 'ex As Exception
            Return "no chr"
        End Try
    End Function
    Public Sub EndGunZ()
        Try
            Process.GetProcessesByName(Runnable.Replace(".exe", Nothing))(0).Kill()
        Catch
        End Try
    End Sub
    Sub Clean_players()
        SyncLock Settings.PlayerList
            Settings.PlayerList.Clear()
        End SyncLock
    End Sub
    Function Get_players() As String()
        SyncLock Settings.PlayerList
            Dim templist As List(Of String) = Settings.PlayerList
            Return templist.ToArray
        End SyncLock
    End Function
    Function get_gstring(ByVal MyBytes As Byte()) As String
        Return System.Text.Encoding.ASCII.GetString(MyBytes, 1, Array.IndexOf(Of Byte)(MyBytes, Byte.MinValue) - 1)
    End Function
    Function Bytes(ByVal str As String) As Byte()
        Return Encoding.ASCII.GetBytes(str)
    End Function
    Public Sub Log(ByVal v As String)
        IO.File.AppendAllText("Alog.txt", "[" & Date.Now & "] " & v & vbCrLf)
    End Sub
    Sub deletor(ByVal file As String)
        Try
            'IO.File.Delete(file)
        Catch ex As Exception
            Log(ex.Message)
            MsgBox("Error, couldn't delete file: " & vbCrLf & file & vbCrLf & vbCrLf & ex.Message, MsgBoxStyle.Exclamation)
            End
        End Try
    End Sub
    Function Get_downloadurl(ByVal urled As String) As String
        Try
            Dim url As New System.Uri(Settings.Update_Server & urled)
            Dim req As System.Net.WebRequest = System.Net.WebRequest.Create(url)
            Dim resp As System.Net.WebResponse
            resp = req.GetResponse()
            resp.Close()
            req = Nothing
            Return Settings.Update_Server & urled
        Catch
            Try
                Dim url As New System.Uri(Settings.Server & "update/" & urled)
                Dim req As System.Net.WebRequest = System.Net.WebRequest.Create(url)
                Dim resp As System.Net.WebResponse
                resp = req.GetResponse()
                resp.Close()
                req = Nothing
                Return Settings.Server & "update/" & urled
            Catch ex As Exception
                Return ex.Message
            End Try
        End Try
    End Function
    Function downloadstring(ByVal a As String) As String
        Return New WebClient().DownloadString(a)
    End Function
    Function Web_filemd5(ByVal file As String) As String
        For r As Int16 = 0 To Settings.filelist.Length - 1
            If file = Settings.filelist(r).Split("=")(0) Then Return Settings.filelist(r).Split("=")(1)
        Next
        Return ""
    End Function
    Function fx(ByVal a As Int16, ByVal b As Int16) As String
        Return (1 + (4 * a + 2 * b)).ToString
    End Function
End Module