Imports System.Threading
Imports System.Net

Public Class Launcher
    Dim bytesIn As Int64
    Dim totalBytes As Int64
    Dim percentage As Int64
    Dim b_t As Int64
    Dim speed As Int32
    Dim gFile As String
    Dim Downloading As Boolean = False
    Dim i As Int16
    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        gFile = "Patch info"
        Timer1.Start()
        Button1.Text = "Starting.."
        Button1.Enabled = False
        BackgroundWorker1.RunWorkerAsync()
    End Sub
    Public Sub filedownloader(ByVal url As String, ByVal location As String)
        Dim client As WebClient = New WebClient
        AddHandler client.DownloadProgressChanged, AddressOf client_ProgressChanged
        AddHandler client.DownloadFileCompleted, AddressOf client_DownloadCompleted
        If location.Contains("Gunz Launcher.exe") Then location = location.Replace("Gunz Launcher.exe", "Gunz Launcher_new.exe")
        client.DownloadFileAsync(New Uri(url), location)
    End Sub
    Private Sub client_ProgressChanged(ByVal sender As Object, ByVal e As DownloadProgressChangedEventArgs)
        bytesIn = e.BytesReceived
        totalBytes = e.TotalBytesToReceive
        percentage = bytesIn / totalBytes * 100
    End Sub
    Private Sub client_DownloadCompleted(ByVal sender As Object, ByVal e As System.ComponentModel.AsyncCompletedEventArgs)
        If gFile.Equals("Gunz Launcher.exe") Then
            ' IO.File.WriteAllText("xchange.txt", CurDir() & "\Gunz Launcher.exe")
            Process.Start("Shader\replacer.exe")
            End
        End If
        bytesIn = 0
        b_t = 0
        totalBytes = 0
        percentage = 0
        speed = 0
        Downloading = False
    End Sub
    Private Sub Timer1_Tick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Timer1.Tick
        Dim prog As Single
        On Error Resume Next
        If Downloading Then
            If b_t = 0 Then
                b_t = bytesIn
            Else
                speed = Math.Round((bytesIn - b_t) / 512, 1)
                b_t = 0
            End If
            ProgressBar2.Visible = True
            ProgressBar2.Value = percentage
            Label1.Text = "Downloading file: " & gFile & " - (" & Math.Round(bytesIn / 1048576, 2) & " MB/" & Math.Round(totalBytes / 1048576, 2) & " MB) - " & speed.ToString & "Kb/s"
        Else
            ProgressBar2.Visible = False
            Label1.Text = "Checking: " & gFile
            speed = 0
            b_t = 0
            bytesIn = 0
        End If
        If Not i = 0 Then
            prog = Math.Round((i) * 100 / CInt(filelist.Length), 2)
            ProgressBar1.Value = Int(prog)
            Me.Text = "SSGunZ Launcher - Total Progressed: " & prog.ToString
        End If
    End Sub

    Private Sub BackgroundWorker1_DoWork(ByVal sender As System.Object, ByVal e As System.ComponentModel.DoWorkEventArgs) Handles BackgroundWorker1.DoWork
        gFile = "Patch info.."
        Try
            filelist = downloadstring(Settings.Update_Server & "Patch.txt").Replace(vbLf, Nothing).Replace(vbCr, Nothing).Split(",")
        Catch
            Try
                filelist = downloadstring(Settings.Server & "Patch.txt").Split(vbCrLf)
            Catch ex As Exception
                Log("Patch info failed, " & ex.Message)
                MsgBox("Error while downloading Patch info, please try again later!", MsgBoxStyle.Exclamation)
                Exit Sub
            End Try
        End Try
        If Not filelist(0).Equals("ALog.txt=0") Then
            Log("Patch info error, please try again later")
            Process.Start("Alog.txt")
            End
        End If
        gFile = "Client files for an Update"
        Do 'While filelist.Length - 1 >= i
            If Not Downloading Then
                If filelist.Length = i Then Exit Do
                Dim info_file As String() = filelist(i).Split("=")
                If Not info_file(1).Equals("0") Then
                    Dim fileidir() As String = info_file(0).Split("\")
                    gFile = fileidir(fileidir.Length - 1)
                    Dim info_slocated As String = info_file(0).Replace("\", "/").Substring(1)
                    If Not GetMD5(CurDir() & info_file(0)).Equals(info_file(1)) Then
                        If Not gFile.Equals("Gunz Launcher.exe") Then
                            If IO.File.Exists(CurDir() & info_file(0)) Then
                                deletor(CurDir() & info_file(0))
                            End If
                        End If

                        Dim Download_addr As String = Get_downloadurl(info_slocated)
                        If Not Download_addr.StartsWith("http://") Then
                            Log(Download_addr)
                            MsgBox("Error while downloading file: " & gFile, MsgBoxStyle.Exclamation)
                            End
                        Else
                            If Not IO.Directory.Exists(CurDir() & info_file(0).Replace("\" & gFile, "")) Then
                                Try
                                    IO.Directory.CreateDirectory(CurDir() & info_file(0).Replace("\" & gFile, ""))
                                Catch ex As Exception
                                    Log("Error in creating dir for update file, error: " & ex.Message)
                                    MsgBox("Error in creating a new directory for file: " & gFile & vbCrLf & vbCrLf & "Error Description: " & ex.Message)
                                    End
                                End Try
                            End If
                            Downloading = True
                            filedownloader(Download_addr, CurDir() & info_file(0))
                        End If
                    End If
                End If
                i += 1
            Else
                Thread.Sleep(100)
            End If
        Loop
        gFile = "Starting SSGunZ... This might take a few moments"
        Dim curr_files As New List(Of IO.FileInfo)
        Dim colExtensions As String() = {"*.mrs", "*.txt", "*.log", "*.exe", "*.dll"}
        For Each strExtension As String In colExtensions
            For Each aFile As IO.FileInfo In New IO.DirectoryInfo(CurDir).GetFiles(strExtension, IO.SearchOption.AllDirectories)
                curr_files.Add(aFile)
            Next
        Next
        For xi As Integer = 0 To curr_files.Count - 1
            For Each pfile As String In filelist
                Dim pPatchfile As String() = pfile.Split("=")(0).Split("\")
                If pPatchfile(pPatchfile.Length - 1).Equals(curr_files(xi).Name) Then GoTo k
            Next
            deletor(curr_files(xi).FullName)
            Log("File: " & curr_files(xi).Name & " Deleted!")
k:
        Next
    End Sub

    Private Sub BackgroundWorker1_RunWorkerCompleted(ByVal sender As Object, ByVal e As System.ComponentModel.RunWorkerCompletedEventArgs) Handles BackgroundWorker1.RunWorkerCompleted
        Timer1.Enabled = False
        'Label1.Text = "Launching SSGunZ..."
        Launchgunz()
    End Sub
    Sub Launchgunz()
        Dim myProcess As New Process()
        myProcess.StartInfo.UseShellExecute = False
        myProcess.StartInfo.FileName = My.Computer.FileSystem.CurrentDirectory + "\" & Settings.Runnable
        myProcess.StartInfo.CreateNoWindow = True
        If AntiCheat.validate_Client() Then
            If myProcess.Start() Then
                If Inject.Inject(CurDir() & "\Shader\data1.dat", Runnable.Replace(".exe", Nothing)) Then
                    Log(Runnable & " has been launched successfully!")
                    If Inject.Inject(CurDir() & "\Shader\data2.dat", Runnable.Replace(".exe", Nothing)) Then
                        Dim tThread As Threading.Thread = New Threading.Thread(AddressOf Protect)
                        tThread.Start()
                        Log("Plugins Loaded successfully!")
                        Me.Hide()
                    Else
                        EndGunZ()
                        Log("Failed loading plugins")
                        End
                    End If
                Else
                    EndGunZ()
                    Log(Runnable & " failed to launch correctly!")
                    End
                End If
            Else
                Log(Runnable & " failed to launch!")
                End
            End If
        End If
    End Sub

    Private Sub Launcher_FormClosing(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing
        End
    End Sub

    Private Sub Launcher_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'Hackdetected("Injector hahahahahaha bla ")
        If Settings.isacc Then
            WebBrowser1.Navigate(Settings.Server & "Launcher.php?&user=" & Settings.UserID)
        Else
            WebBrowser1.Navigate(Settings.Server & "Launcher.php?&char=" & Settings.UserID)
        End If
    End Sub

    Private Sub WebBrowser1_DocumentCompleted(ByVal sender As Object, ByVal e As System.Windows.Forms.WebBrowserDocumentCompletedEventArgs) Handles WebBrowser1.DocumentCompleted
        WebBrowser1.Visible = True
    End Sub
End Class
