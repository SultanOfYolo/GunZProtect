<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class Admin_Console
    Inherits System.Windows.Forms.Form

    'Das Formular überschreibt den Löschvorgang, um die Komponentenliste zu bereinigen.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Wird vom Windows Form-Designer benötigt.
    Private components As System.ComponentModel.IContainer

    'Hinweis: Die folgende Prozedur ist für den Windows Form-Designer erforderlich.
    'Das Bearbeiten ist mit dem Windows Form-Designer möglich.  
    'Das Bearbeiten mit dem Code-Editor ist nicht möglich.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(Admin_Console))
        Me.GroupBox1 = New System.Windows.Forms.GroupBox()
        Me.ListBox1 = New System.Windows.Forms.ListBox()
        Me.GroupBox2 = New System.Windows.Forms.GroupBox()
        Me.Button8 = New System.Windows.Forms.Button()
        Me.Button7 = New System.Windows.Forms.Button()
        Me.Button6 = New System.Windows.Forms.Button()
        Me.Button5 = New System.Windows.Forms.Button()
        Me.Button4 = New System.Windows.Forms.Button()
        Me.Button3 = New System.Windows.Forms.Button()
        Me.Button2 = New System.Windows.Forms.Button()
        Me.Button1 = New System.Windows.Forms.Button()
        Me.ListBox2 = New System.Windows.Forms.ListBox()
        Me.Timer1 = New System.Windows.Forms.Timer(Me.components)
        Me.GroupBox4 = New System.Windows.Forms.GroupBox()
        Me.Button10 = New System.Windows.Forms.Button()
        Me.Button9 = New System.Windows.Forms.Button()
        Me.ListBox3 = New System.Windows.Forms.ListBox()
        Me.TextBox1 = New System.Windows.Forms.TextBox()
        Me.ComboBox1 = New System.Windows.Forms.ComboBox()
        Me.NotifyIcon1 = New System.Windows.Forms.NotifyIcon(Me.components)
        Me.GroupBox1.SuspendLayout()
        Me.GroupBox2.SuspendLayout()
        Me.GroupBox4.SuspendLayout()
        Me.SuspendLayout()
        '
        'GroupBox1
        '
        Me.GroupBox1.Controls.Add(Me.ListBox1)
        Me.GroupBox1.Location = New System.Drawing.Point(12, 12)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Size = New System.Drawing.Size(357, 431)
        Me.GroupBox1.TabIndex = 0
        Me.GroupBox1.TabStop = False
        Me.GroupBox1.Text = "Connected Accounts"
        '
        'ListBox1
        '
        Me.ListBox1.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ListBox1.FormattingEnabled = True
        Me.ListBox1.ItemHeight = 16
        Me.ListBox1.Location = New System.Drawing.Point(6, 19)
        Me.ListBox1.Name = "ListBox1"
        Me.ListBox1.Size = New System.Drawing.Size(345, 404)
        Me.ListBox1.TabIndex = 0
        '
        'GroupBox2
        '
        Me.GroupBox2.Controls.Add(Me.Button8)
        Me.GroupBox2.Controls.Add(Me.Button7)
        Me.GroupBox2.Controls.Add(Me.Button6)
        Me.GroupBox2.Controls.Add(Me.Button5)
        Me.GroupBox2.Controls.Add(Me.Button4)
        Me.GroupBox2.Controls.Add(Me.Button3)
        Me.GroupBox2.Controls.Add(Me.Button2)
        Me.GroupBox2.Controls.Add(Me.Button1)
        Me.GroupBox2.ForeColor = System.Drawing.Color.Maroon
        Me.GroupBox2.Location = New System.Drawing.Point(403, 144)
        Me.GroupBox2.Name = "GroupBox2"
        Me.GroupBox2.Size = New System.Drawing.Size(280, 299)
        Me.GroupBox2.TabIndex = 1
        Me.GroupBox2.TabStop = False
        Me.GroupBox2.Text = "Administration"
        '
        'Button8
        '
        Me.Button8.Location = New System.Drawing.Point(6, 264)
        Me.Button8.Name = "Button8"
        Me.Button8.Size = New System.Drawing.Size(268, 25)
        Me.Button8.TabIndex = 7
        Me.Button8.Text = "Unban selected player's Hardware"
        Me.Button8.UseVisualStyleBackColor = True
        '
        'Button7
        '
        Me.Button7.Location = New System.Drawing.Point(6, 234)
        Me.Button7.Name = "Button7"
        Me.Button7.Size = New System.Drawing.Size(268, 24)
        Me.Button7.TabIndex = 6
        Me.Button7.Text = "Unban selelected player's Account"
        Me.Button7.UseVisualStyleBackColor = True
        '
        'Button6
        '
        Me.Button6.Location = New System.Drawing.Point(6, 204)
        Me.Button6.Name = "Button6"
        Me.Button6.Size = New System.Drawing.Size(268, 24)
        Me.Button6.TabIndex = 5
        Me.Button6.Text = "Unban selected player's IP"
        Me.Button6.UseVisualStyleBackColor = True
        '
        'Button5
        '
        Me.Button5.Location = New System.Drawing.Point(6, 174)
        Me.Button5.Name = "Button5"
        Me.Button5.Size = New System.Drawing.Size(268, 24)
        Me.Button5.TabIndex = 4
        Me.Button5.Text = "Ban selected player's Hardware"
        Me.Button5.UseVisualStyleBackColor = True
        '
        'Button4
        '
        Me.Button4.Location = New System.Drawing.Point(6, 144)
        Me.Button4.Name = "Button4"
        Me.Button4.Size = New System.Drawing.Size(268, 24)
        Me.Button4.TabIndex = 3
        Me.Button4.Text = "Ban selected player's Account"
        Me.Button4.UseVisualStyleBackColor = True
        '
        'Button3
        '
        Me.Button3.Location = New System.Drawing.Point(6, 115)
        Me.Button3.Name = "Button3"
        Me.Button3.Size = New System.Drawing.Size(268, 23)
        Me.Button3.TabIndex = 2
        Me.Button3.Text = "Ban selected player's IP"
        Me.Button3.UseVisualStyleBackColor = True
        '
        'Button2
        '
        Me.Button2.ForeColor = System.Drawing.Color.Red
        Me.Button2.Location = New System.Drawing.Point(6, 63)
        Me.Button2.Name = "Button2"
        Me.Button2.Size = New System.Drawing.Size(268, 36)
        Me.Button2.TabIndex = 1
        Me.Button2.Text = "Disconnect selected player"
        Me.Button2.UseVisualStyleBackColor = True
        '
        'Button1
        '
        Me.Button1.ForeColor = System.Drawing.Color.Black
        Me.Button1.Location = New System.Drawing.Point(6, 19)
        Me.Button1.Name = "Button1"
        Me.Button1.Size = New System.Drawing.Size(268, 38)
        Me.Button1.TabIndex = 0
        Me.Button1.Text = "Refresh Connected Players"
        Me.Button1.UseVisualStyleBackColor = True
        '
        'ListBox2
        '
        Me.ListBox2.FormattingEnabled = True
        Me.ListBox2.Location = New System.Drawing.Point(12, 449)
        Me.ListBox2.Name = "ListBox2"
        Me.ListBox2.Size = New System.Drawing.Size(671, 56)
        Me.ListBox2.TabIndex = 0
        '
        'Timer1
        '
        Me.Timer1.Interval = 200
        '
        'GroupBox4
        '
        Me.GroupBox4.Controls.Add(Me.Button10)
        Me.GroupBox4.Controls.Add(Me.Button9)
        Me.GroupBox4.Controls.Add(Me.ListBox3)
        Me.GroupBox4.Controls.Add(Me.TextBox1)
        Me.GroupBox4.Controls.Add(Me.ComboBox1)
        Me.GroupBox4.Location = New System.Drawing.Point(403, 12)
        Me.GroupBox4.Name = "GroupBox4"
        Me.GroupBox4.Size = New System.Drawing.Size(274, 126)
        Me.GroupBox4.TabIndex = 3
        Me.GroupBox4.TabStop = False
        Me.GroupBox4.Text = "Search Online Players"
        '
        'Button10
        '
        Me.Button10.Location = New System.Drawing.Point(136, 19)
        Me.Button10.Name = "Button10"
        Me.Button10.Size = New System.Drawing.Size(133, 23)
        Me.Button10.TabIndex = 4
        Me.Button10.Text = "Clear search results"
        Me.Button10.UseVisualStyleBackColor = True
        '
        'Button9
        '
        Me.Button9.Location = New System.Drawing.Point(136, 46)
        Me.Button9.Name = "Button9"
        Me.Button9.Size = New System.Drawing.Size(133, 23)
        Me.Button9.TabIndex = 3
        Me.Button9.Text = "Search"
        Me.Button9.UseVisualStyleBackColor = True
        '
        'ListBox3
        '
        Me.ListBox3.FormattingEnabled = True
        Me.ListBox3.Location = New System.Drawing.Point(6, 72)
        Me.ListBox3.Name = "ListBox3"
        Me.ListBox3.Size = New System.Drawing.Size(263, 43)
        Me.ListBox3.TabIndex = 2
        '
        'TextBox1
        '
        Me.TextBox1.Location = New System.Drawing.Point(6, 46)
        Me.TextBox1.Name = "TextBox1"
        Me.TextBox1.Size = New System.Drawing.Size(121, 20)
        Me.TextBox1.TabIndex = 1
        '
        'ComboBox1
        '
        Me.ComboBox1.FormattingEnabled = True
        Me.ComboBox1.Items.AddRange(New Object() {"IP", "UserID/Character"})
        Me.ComboBox1.Location = New System.Drawing.Point(6, 19)
        Me.ComboBox1.Name = "ComboBox1"
        Me.ComboBox1.Size = New System.Drawing.Size(121, 21)
        Me.ComboBox1.TabIndex = 0
        Me.ComboBox1.Text = "UserID/Character"
        '
        'NotifyIcon1
        '
        Me.NotifyIcon1.Icon = CType(resources.GetObject("NotifyIcon1.Icon"), System.Drawing.Icon)
        Me.NotifyIcon1.Text = "Sraff Console"
        '
        'Admin_Console
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(686, 517)
        Me.Controls.Add(Me.ListBox2)
        Me.Controls.Add(Me.GroupBox4)
        Me.Controls.Add(Me.GroupBox2)
        Me.Controls.Add(Me.GroupBox1)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.MaximizeBox = False
        Me.Name = "Admin_Console"
        Me.Text = "SSGunZ Staff Console"
        Me.GroupBox1.ResumeLayout(False)
        Me.GroupBox2.ResumeLayout(False)
        Me.GroupBox4.ResumeLayout(False)
        Me.GroupBox4.PerformLayout()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents GroupBox1 As System.Windows.Forms.GroupBox
    Friend WithEvents ListBox1 As System.Windows.Forms.ListBox
    Friend WithEvents GroupBox2 As System.Windows.Forms.GroupBox
    Friend WithEvents Button5 As System.Windows.Forms.Button
    Friend WithEvents Button4 As System.Windows.Forms.Button
    Friend WithEvents Button3 As System.Windows.Forms.Button
    Friend WithEvents Button2 As System.Windows.Forms.Button
    Friend WithEvents Button1 As System.Windows.Forms.Button
    Friend WithEvents ListBox2 As System.Windows.Forms.ListBox
    Friend WithEvents Timer1 As System.Windows.Forms.Timer
    Friend WithEvents Button8 As System.Windows.Forms.Button
    Friend WithEvents Button7 As System.Windows.Forms.Button
    Friend WithEvents Button6 As System.Windows.Forms.Button
    Friend WithEvents GroupBox4 As System.Windows.Forms.GroupBox
    Friend WithEvents ComboBox1 As System.Windows.Forms.ComboBox
    Friend WithEvents Button9 As System.Windows.Forms.Button
    Friend WithEvents ListBox3 As System.Windows.Forms.ListBox
    Friend WithEvents TextBox1 As System.Windows.Forms.TextBox
    Friend WithEvents Button10 As System.Windows.Forms.Button
    Friend WithEvents NotifyIcon1 As System.Windows.Forms.NotifyIcon
End Class
