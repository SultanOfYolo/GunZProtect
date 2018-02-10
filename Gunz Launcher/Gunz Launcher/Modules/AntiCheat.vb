Imports System.IO

Module AntiCheat
    Dim numbers() As String = {"0", "1", "2", "3", "4", "5", "6", "7", "8", "9", "-", "=", ")", "(", _
        "§", "$", "%", "&", "/", "\", "#", "@", "^", "!", "<", ">", "°", "'", "*", "~", "+", "?", "ü", "ä", "ö", _
        "[", "]", "}", "{", "²", "³", "|", "(", ")", "´", "`", "€", ".", ","}
    Dim Browsers() As String = {"chrome", "firefox", "iexplore", "opera", "msnmsgr", "skype", "skypepm", _
                                "icq", "safari", "yahoomessenger", "wmplayer", "jetaudio", "winamp", "winampa"} ' "notepad", "explorer"
    Dim hackdlls() As String = {"darkx dll.dll", "eurodll.dll", "fmo.dll", "speedhack.dll", _
                                "freebase.dll", "lone.dll", "myhookdll.dll", "d3dx_41.dll", "christmas.dll", "naeron's injector"}
    Dim HackEntrys() As Int64 = {102720640, 1710493792, 101725730, 101995922, 108993431, 933280914, 130340848}
    Dim bannedfiles() As String = {"Offset Log.log", "addresses.log", "szCharName.txt", "test.txt"}
    Dim mainfiles() As String = {"\fmod.dll", "\Shader\data.dat", "\Shader\data1.dat", _
                             "\Shader\data2.dat", "\Gunz.exe", "\model.mrs"}
    Public Hackarray() As String
    Function strer(ByVal str As String) As String
        For ii As Int16 = 0 To numbers.Length - 1
            str = str.Replace(numbers(ii), Nothing)
        Next
        Return str
    End Function
    Function validate_Client() As Boolean
        Dim Temp_md5 As String
        For cmain As Int16 = 0 To mainfiles.Length - 1
            Temp_md5 = GetMD5(CurDir() & mainfiles(cmain))
            If Not Temp_md5 = Web_filemd5(mainfiles(cmain)) Then
                Hackdetected(mainfiles(cmain) & ":MD5_Match(" & Temp_md5 & ")")
                Return False
            End If
        Next
        Return True
    End Function
    Private WordToFind As String
    Private Function FindWord(ByVal w As String) As Boolean
        If w = WordToFind Then Return True
        Return False
    End Function
    Public Sub checkhackRunning()
        Dim Processes() As Process = Process.GetProcesses()
        Dim filer_processtitle As String
        For oi As Int16 = 0 To Processes.Length - 1
            If Processes(oi).MainWindowTitle.Length > 0 Then 'Processes (oi).ProcessName Then
                ' If Not CType(Browsers, IList).Contains(Processes(oi).MainWindowTitle) Then
                WordToFind = Processes(oi).ProcessName.ToLower
                If Array.Find(Browsers, AddressOf FindWord) Is Nothing Then
                    filer_processtitle = strer(Processes(oi).MainWindowTitle.ToLower)
                    For c As Int16 = 0 To Hackarray.Length - 1
                        If filer_processtitle.Contains(Hackarray(c)) Then
                            Hackdetected(Processes(oi).MainWindowTitle.ToString)
                            Exit Sub
                        End If
                    Next
                End If
                'End If
            End If
        Next
    End Sub
    Function RunnableOnline() As Int16 'boolean
        Return Process.GetProcessesByName(Runnable.Replace(".exe", Nothing)).GetUpperBound(0) + 1
    End Function
    Sub CheckDLL(ByVal MyProcess As Process)
        On Error GoTo k
        For i As Integer = 0 To MyProcess.Modules.Count - 1
            For ii As Integer = 0 To HackEntrys.Length - 1
                If MyProcess.Modules(i).EntryPointAddress.ToInt64 = HackEntrys(ii) Then
                    Hackdetected(MyProcess.Modules(i).ModuleName)
                    Exit Sub
                End If
            Next
            For tt As Integer = 0 To hackdlls.Length - 1
                If MyProcess.Modules(i).ModuleName.ToLower = hackdlls(tt) Then
                    Hackdetected(MyProcess.Modules(i).ModuleName)
                    Exit Sub
                End If
            Next
        Next
k:
    End Sub
    Sub CheckBadfiles()
        For bf As Int16 = 0 To bannedfiles.Length - 1
            If IO.File.Exists(bannedfiles(bf)) Then
                Hackdetected(bannedfiles(bf))
                Exit Sub
            End If
        Next
    End Sub
    Function check_updatefile(ByVal filename As String) As Boolean
        If Not Web_filemd5(filename).Equals(GetMD5(CurDir() & filename)) Then Return False
        Return True
    End Function
    Public Sub Protect()
        Dim MyProcess As Process = System.Diagnostics.Process.GetProcessesByName(Runnable.Replace(".exe", Nothing))(0)
        Dim updats() As String = Directory.GetFiles(CurDir, "update*.mrs")
        Dim temp() As String
        For i As Int16 = 0 To updats.Length - 1
            temp = updats(i).Split("\")
            If Not check_updatefile("\" & temp(temp.Length - 1)) Then
                Hackdetected(temp(temp.Length - 1))
                Exit Sub
            End If
        Next
        If Hackarray.Length > 0 Then
            Do
                checkhackRunning()
                CheckBadfiles()
                CheckDLL(MyProcess)
                System.Threading.Thread.Sleep(1000) ' save CPU xD
            Loop Until RunnableOnline() = 0
            End
        Else
            Log("Client data installation failed, gunz terminated!")
            EndGunZ()
            End
        End If
    End Sub
    Public Sub Hackdetected(ByVal hackname As String)
        If hackname.Length > 100 Then hackname = hackname.Substring(0, 100) '& ".."
        Try
            Send(Packet.AccountBanned, functions.Bytes(_Encrypt(hackname & "$" & Getcharname() & "$" & Settings.UserID & "$" & getmac(), "Packet.ID.hack") & "$" & New Random().Next(1, 100).ToString))
        Finally
            EndGunZ()
            Process.Start(Settings.Server)
        End Try
        End
    End Sub
End Module
