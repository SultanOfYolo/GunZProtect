<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
<Global.System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1726")> _
Partial Class Login_Account
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
    Friend WithEvents PasswordLabel As System.Windows.Forms.Label
    Friend WithEvents UsernameTextBox As System.Windows.Forms.TextBox
    Friend WithEvents PasswordTextBox As System.Windows.Forms.TextBox
    Friend WithEvents OK As System.Windows.Forms.Button
    Friend WithEvents Cancel As System.Windows.Forms.Button

    'Wird vom Windows Form-Designer benötigt.
    Private components As System.ComponentModel.IContainer

    'Hinweis: Die folgende Prozedur ist für den Windows Form-Designer erforderlich.
    'Das Bearbeiten ist mit dem Windows Form-Designer möglich.  
    'Das Bearbeiten mit dem Code-Editor ist nicht möglich.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(Login_Account))
        Me.PasswordLabel = New System.Windows.Forms.Label()
        Me.UsernameTextBox = New System.Windows.Forms.TextBox()
        Me.PasswordTextBox = New System.Windows.Forms.TextBox()
        Me.OK = New System.Windows.Forms.Button()
        Me.Cancel = New System.Windows.Forms.Button()
        Me.acctype = New System.Windows.Forms.ComboBox()
        Me.CheckBox1 = New System.Windows.Forms.CheckBox()
        Me.Timer1 = New System.Windows.Forms.Timer(Me.components)
        Me.SuspendLayout()
        '
        'PasswordLabel
        '
        Me.PasswordLabel.Location = New System.Drawing.Point(9, 76)
        Me.PasswordLabel.Name = "PasswordLabel"
        Me.PasswordLabel.Size = New System.Drawing.Size(220, 23)
        Me.PasswordLabel.TabIndex = 2
        Me.PasswordLabel.Text = "&Password"
        Me.PasswordLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'UsernameTextBox
        '
        Me.UsernameTextBox.Location = New System.Drawing.Point(11, 39)
        Me.UsernameTextBox.MaxLength = 18
        Me.UsernameTextBox.Name = "UsernameTextBox"
        Me.UsernameTextBox.Size = New System.Drawing.Size(188, 20)
        Me.UsernameTextBox.TabIndex = 1
        '
        'PasswordTextBox
        '
        Me.PasswordTextBox.Location = New System.Drawing.Point(11, 96)
        Me.PasswordTextBox.MaxLength = 64
        Me.PasswordTextBox.Name = "PasswordTextBox"
        Me.PasswordTextBox.PasswordChar = Global.Microsoft.VisualBasic.ChrW(42)
        Me.PasswordTextBox.Size = New System.Drawing.Size(188, 20)
        Me.PasswordTextBox.TabIndex = 3
        '
        'OK
        '
        Me.OK.Location = New System.Drawing.Point(11, 151)
        Me.OK.Name = "OK"
        Me.OK.Size = New System.Drawing.Size(94, 29)
        Me.OK.TabIndex = 4
        Me.OK.Text = "&OK"
        '
        'Cancel
        '
        Me.Cancel.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.Cancel.Location = New System.Drawing.Point(105, 151)
        Me.Cancel.Name = "Cancel"
        Me.Cancel.Size = New System.Drawing.Size(94, 29)
        Me.Cancel.TabIndex = 5
        Me.Cancel.Text = "&Cancel"
        '
        'acctype
        '
        Me.acctype.FormattingEnabled = True
        Me.acctype.Items.AddRange(New Object() {"User name", "Character name"})
        Me.acctype.Location = New System.Drawing.Point(12, 12)
        Me.acctype.Name = "acctype"
        Me.acctype.Size = New System.Drawing.Size(99, 21)
        Me.acctype.TabIndex = 6
        Me.acctype.Text = "User name"
        '
        'CheckBox1
        '
        Me.CheckBox1.AutoSize = True
        Me.CheckBox1.Location = New System.Drawing.Point(12, 122)
        Me.CheckBox1.Name = "CheckBox1"
        Me.CheckBox1.Size = New System.Drawing.Size(134, 17)
        Me.CheckBox1.TabIndex = 7
        Me.CheckBox1.Text = "Save Login information"
        Me.CheckBox1.UseVisualStyleBackColor = True
        '
        'Timer1
        '
        '
        'Login_Account
        '
        Me.AcceptButton = Me.OK
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.CancelButton = Me.Cancel
        Me.ClientSize = New System.Drawing.Size(210, 192)
        Me.Controls.Add(Me.CheckBox1)
        Me.Controls.Add(Me.acctype)
        Me.Controls.Add(Me.Cancel)
        Me.Controls.Add(Me.OK)
        Me.Controls.Add(Me.PasswordTextBox)
        Me.Controls.Add(Me.UsernameTextBox)
        Me.Controls.Add(Me.PasswordLabel)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "Login_Account"
        Me.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "SSGunZ Login"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents acctype As System.Windows.Forms.ComboBox
    Friend WithEvents CheckBox1 As System.Windows.Forms.CheckBox
    Friend WithEvents Timer1 As System.Windows.Forms.Timer

End Class
