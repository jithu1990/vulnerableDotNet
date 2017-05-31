Option Strict On
Option Explicit On 

Imports System
Imports System.IO
Imports System.Drawing
Imports System.Collections
Imports System.ComponentModel
Imports System.Windows.Forms
Imports System.Data

Namespace HacmeBankDBSetup
    '/ <summary>
    '/ Summary description for Form1.
    '/ </summary>
    Public Class frmSetup

#Region "User Interface members"
        Inherits System.Windows.Forms.Form
        Private panel2 As System.Windows.Forms.Panel
        Private WithEvents backButton As System.Windows.Forms.Button
        Private WithEvents nextButton As System.Windows.Forms.Button
        Private panel1 As System.Windows.Forms.Panel
        Private WithEvents configDBPanel2 As System.Windows.Forms.Panel
        Private panel5 As System.Windows.Forms.Panel
        Private label9 As System.Windows.Forms.Label
        Private label10 As System.Windows.Forms.Label
        Private label11 As System.Windows.Forms.Label
        Private dbServerNameTextBox As System.Windows.Forms.TextBox
        Private dbConnectionGroupBox As System.Windows.Forms.GroupBox
        Private label12 As System.Windows.Forms.Label
        Private label13 As System.Windows.Forms.Label
        Private WithEvents setupFailedPanel As System.Windows.Forms.Panel
        Private label18 As System.Windows.Forms.Label
        Private panel8 As System.Windows.Forms.Panel
        Private label19 As System.Windows.Forms.Label
        Private label20 As System.Windows.Forms.Label
        Private failureReasonTextBox As System.Windows.Forms.TextBox
        Private dbTrustedConnectionRadioButton As System.Windows.Forms.RadioButton
        Private dbSqlAuthenticationRadioButton As System.Windows.Forms.RadioButton
        Private dbUserNametextBox As System.Windows.Forms.TextBox
        Private dbPasswordtextBox As System.Windows.Forms.TextBox
        Private m_currentPage As Panel = Nothing
        Private m_install As Install = Nothing
        Private WithEvents installingPanel As System.Windows.Forms.Panel
        Private installStepLabel As System.Windows.Forms.Label
        Private panel10 As System.Windows.Forms.Panel
        Private label25 As System.Windows.Forms.Label
        Private progressBar1 As System.Windows.Forms.ProgressBar
#End Region

#Region "Config properties"

        ' The directory where the application to be configured is installed.
        Private appDir As String
        Property ApplicationDirectory() As String
            Get
                Return appDir
            End Get
            Set(ByVal Value As String)
                appDir = Value
            End Set
        End Property
#End Region
        '/ <summary>
        '/ Required designer variable.
        '/ </summary>
        Private components As System.ComponentModel.Container = Nothing
        Public Shared usernameParameter As String
        Public Shared passwordParameter As String
        Private m_unInstall As Boolean = False

        Public Sub New(ByVal appDir As String, ByVal unInstall As Boolean)
            '
            ' Required for Windows Form Designer support
            '
            InitializeComponent()

            m_unInstall = unInstall
            m_install = New Install(appDir, unInstall)

            AddHandler m_install.Step, AddressOf Me.InstallStep
        End Sub

        Public Sub New(ByVal unInstall As Boolean)
            Me.New(Path.GetDirectoryName(System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName), unInstall)
        End Sub 'New

        '/ <summary>
        '/ Clean up any resources being used.
        '/ </summary>
        Protected Overloads Overrides Sub Dispose(ByVal disposing As Boolean)
            If disposing Then
                If Not (components Is Nothing) Then
                    components.Dispose()
                End If
            End If
            MyBase.Dispose(disposing)
        End Sub 'Dispose

#Region "Windows Form Designer generated code"
        Friend WithEvents m_CancelButton As System.Windows.Forms.Button

        Private Sub InitializeComponent()
            Dim resources As System.Resources.ResourceManager = New System.Resources.ResourceManager(GetType(frmSetup))
            Me.panel2 = New System.Windows.Forms.Panel
            Me.m_CancelButton = New System.Windows.Forms.Button
            Me.nextButton = New System.Windows.Forms.Button
            Me.backButton = New System.Windows.Forms.Button
            Me.panel1 = New System.Windows.Forms.Panel
            Me.installingPanel = New System.Windows.Forms.Panel
            Me.progressBar1 = New System.Windows.Forms.ProgressBar
            Me.installStepLabel = New System.Windows.Forms.Label
            Me.panel10 = New System.Windows.Forms.Panel
            Me.label25 = New System.Windows.Forms.Label
            Me.configDBPanel2 = New System.Windows.Forms.Panel
            Me.dbConnectionGroupBox = New System.Windows.Forms.GroupBox
            Me.dbPasswordtextBox = New System.Windows.Forms.TextBox
            Me.dbUserNametextBox = New System.Windows.Forms.TextBox
            Me.label13 = New System.Windows.Forms.Label
            Me.label12 = New System.Windows.Forms.Label
            Me.dbSqlAuthenticationRadioButton = New System.Windows.Forms.RadioButton
            Me.dbTrustedConnectionRadioButton = New System.Windows.Forms.RadioButton
            Me.dbServerNameTextBox = New System.Windows.Forms.TextBox
            Me.label11 = New System.Windows.Forms.Label
            Me.label10 = New System.Windows.Forms.Label
            Me.panel5 = New System.Windows.Forms.Panel
            Me.label9 = New System.Windows.Forms.Label
            Me.setupFailedPanel = New System.Windows.Forms.Panel
            Me.failureReasonTextBox = New System.Windows.Forms.TextBox
            Me.label20 = New System.Windows.Forms.Label
            Me.label18 = New System.Windows.Forms.Label
            Me.panel8 = New System.Windows.Forms.Panel
            Me.label19 = New System.Windows.Forms.Label
            Me.panel2.SuspendLayout()
            Me.panel1.SuspendLayout()
            Me.installingPanel.SuspendLayout()
            Me.panel10.SuspendLayout()
            Me.configDBPanel2.SuspendLayout()
            Me.dbConnectionGroupBox.SuspendLayout()
            Me.panel5.SuspendLayout()
            Me.setupFailedPanel.SuspendLayout()
            Me.panel8.SuspendLayout()
            Me.SuspendLayout()
            '
            'panel2
            '
            Me.panel2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
            Me.panel2.Controls.Add(Me.m_CancelButton)
            Me.panel2.Controls.Add(Me.nextButton)
            Me.panel2.Controls.Add(Me.backButton)
            Me.panel2.Dock = System.Windows.Forms.DockStyle.Bottom
            Me.panel2.Location = New System.Drawing.Point(0, 349)
            Me.panel2.Name = "panel2"
            Me.panel2.Size = New System.Drawing.Size(520, 48)
            Me.panel2.TabIndex = 2
            '
            'm_CancelButton
            '
            Me.m_CancelButton.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
            Me.m_CancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel
            Me.m_CancelButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat
            Me.m_CancelButton.Location = New System.Drawing.Point(433, 8)
            Me.m_CancelButton.Name = "m_CancelButton"
            Me.m_CancelButton.TabIndex = 2
            Me.m_CancelButton.Text = "&Cancel"
            '
            'nextButton
            '
            Me.nextButton.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
            Me.nextButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat
            Me.nextButton.Location = New System.Drawing.Point(351, 8)
            Me.nextButton.Name = "nextButton"
            Me.nextButton.TabIndex = 0
            Me.nextButton.Text = "&Next >"
            '
            'backButton
            '
            Me.backButton.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
            Me.backButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat
            Me.backButton.Location = New System.Drawing.Point(271, 8)
            Me.backButton.Name = "backButton"
            Me.backButton.TabIndex = 1
            Me.backButton.Text = "<< &Back"
            '
            'panel1
            '
            Me.panel1.Controls.Add(Me.installingPanel)
            Me.panel1.Controls.Add(Me.configDBPanel2)
            Me.panel1.Controls.Add(Me.setupFailedPanel)
            Me.panel1.Dock = System.Windows.Forms.DockStyle.Fill
            Me.panel1.Location = New System.Drawing.Point(0, 0)
            Me.panel1.Name = "panel1"
            Me.panel1.Size = New System.Drawing.Size(520, 349)
            Me.panel1.TabIndex = 3
            '
            'installingPanel
            '
            Me.installingPanel.Controls.Add(Me.progressBar1)
            Me.installingPanel.Controls.Add(Me.installStepLabel)
            Me.installingPanel.Controls.Add(Me.panel10)
            Me.installingPanel.Location = New System.Drawing.Point(8, 128)
            Me.installingPanel.Name = "installingPanel"
            Me.installingPanel.Size = New System.Drawing.Size(100, 50)
            Me.installingPanel.TabIndex = 7
            Me.installingPanel.Visible = False
            '
            'progressBar1
            '
            Me.progressBar1.Location = New System.Drawing.Point(24, 152)
            Me.progressBar1.Maximum = 15
            Me.progressBar1.Name = "progressBar1"
            Me.progressBar1.Size = New System.Drawing.Size(448, 23)
            Me.progressBar1.TabIndex = 4
            '
            'installStepLabel
            '
            Me.installStepLabel.Location = New System.Drawing.Point(24, 128)
            Me.installStepLabel.Name = "installStepLabel"
            Me.installStepLabel.Size = New System.Drawing.Size(448, 23)
            Me.installStepLabel.TabIndex = 3
            '
            'panel10
            '
            Me.panel10.BackColor = System.Drawing.Color.White
            Me.panel10.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
            Me.panel10.Controls.Add(Me.label25)
            Me.panel10.Dock = System.Windows.Forms.DockStyle.Top
            Me.panel10.Location = New System.Drawing.Point(0, 0)
            Me.panel10.Name = "panel10"
            Me.panel10.Size = New System.Drawing.Size(100, 60)
            Me.panel10.TabIndex = 1
            '
            'label25
            '
            Me.label25.Font = New System.Drawing.Font("Tahoma", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
            Me.label25.Location = New System.Drawing.Point(24, 16)
            Me.label25.Name = "label25"
            Me.label25.Size = New System.Drawing.Size(280, 23)
            Me.label25.TabIndex = 0
            Me.label25.Text = "Configuring the Hacme Bank Database"
            '
            'configDBPanel2
            '
            Me.configDBPanel2.Controls.Add(Me.dbConnectionGroupBox)
            Me.configDBPanel2.Controls.Add(Me.dbServerNameTextBox)
            Me.configDBPanel2.Controls.Add(Me.label11)
            Me.configDBPanel2.Controls.Add(Me.label10)
            Me.configDBPanel2.Controls.Add(Me.panel5)
            Me.configDBPanel2.Location = New System.Drawing.Point(16, 24)
            Me.configDBPanel2.Name = "configDBPanel2"
            Me.configDBPanel2.Size = New System.Drawing.Size(536, 312)
            Me.configDBPanel2.TabIndex = 2
            Me.configDBPanel2.Visible = False
            '
            'dbConnectionGroupBox
            '
            Me.dbConnectionGroupBox.Controls.Add(Me.dbPasswordtextBox)
            Me.dbConnectionGroupBox.Controls.Add(Me.dbUserNametextBox)
            Me.dbConnectionGroupBox.Controls.Add(Me.label13)
            Me.dbConnectionGroupBox.Controls.Add(Me.label12)
            Me.dbConnectionGroupBox.Controls.Add(Me.dbSqlAuthenticationRadioButton)
            Me.dbConnectionGroupBox.Controls.Add(Me.dbTrustedConnectionRadioButton)
            Me.dbConnectionGroupBox.Location = New System.Drawing.Point(16, 176)
            Me.dbConnectionGroupBox.Name = "dbConnectionGroupBox"
            Me.dbConnectionGroupBox.Size = New System.Drawing.Size(440, 128)
            Me.dbConnectionGroupBox.TabIndex = 5
            Me.dbConnectionGroupBox.TabStop = False
            '
            'dbPasswordtextBox
            '
            Me.dbPasswordtextBox.Location = New System.Drawing.Point(120, 96)
            Me.dbPasswordtextBox.Name = "dbPasswordtextBox"
            Me.dbPasswordtextBox.PasswordChar = Microsoft.VisualBasic.ChrW(42)
            Me.dbPasswordtextBox.Size = New System.Drawing.Size(120, 22)
            Me.dbPasswordtextBox.TabIndex = 5
            Me.dbPasswordtextBox.Text = ""
            '
            'dbUserNametextBox
            '
            Me.dbUserNametextBox.Location = New System.Drawing.Point(120, 72)
            Me.dbUserNametextBox.Name = "dbUserNametextBox"
            Me.dbUserNametextBox.Size = New System.Drawing.Size(120, 22)
            Me.dbUserNametextBox.TabIndex = 4
            Me.dbUserNametextBox.Text = "sa"
            '
            'label13
            '
            Me.label13.Location = New System.Drawing.Point(32, 104)
            Me.label13.Name = "label13"
            Me.label13.Size = New System.Drawing.Size(72, 18)
            Me.label13.TabIndex = 3
            Me.label13.Text = "Password:"
            '
            'label12
            '
            Me.label12.Location = New System.Drawing.Point(32, 80)
            Me.label12.Name = "label12"
            Me.label12.Size = New System.Drawing.Size(80, 16)
            Me.label12.TabIndex = 2
            Me.label12.Text = "User Name:"
            '
            'dbSqlAuthenticationRadioButton
            '
            Me.dbSqlAuthenticationRadioButton.Checked = True
            Me.dbSqlAuthenticationRadioButton.Location = New System.Drawing.Point(16, 48)
            Me.dbSqlAuthenticationRadioButton.Name = "dbSqlAuthenticationRadioButton"
            Me.dbSqlAuthenticationRadioButton.Size = New System.Drawing.Size(216, 24)
            Me.dbSqlAuthenticationRadioButton.TabIndex = 1
            Me.dbSqlAuthenticationRadioButton.TabStop = True
            Me.dbSqlAuthenticationRadioButton.Text = "SQL Server Authentication"
            '
            'dbTrustedConnectionRadioButton
            '
            Me.dbTrustedConnectionRadioButton.Location = New System.Drawing.Point(16, 16)
            Me.dbTrustedConnectionRadioButton.Name = "dbTrustedConnectionRadioButton"
            Me.dbTrustedConnectionRadioButton.Size = New System.Drawing.Size(152, 24)
            Me.dbTrustedConnectionRadioButton.TabIndex = 0
            Me.dbTrustedConnectionRadioButton.Text = "Trusted Connection"
            '
            'dbServerNameTextBox
            '
            Me.dbServerNameTextBox.Location = New System.Drawing.Point(176, 136)
            Me.dbServerNameTextBox.Name = "dbServerNameTextBox"
            Me.dbServerNameTextBox.Size = New System.Drawing.Size(160, 22)
            Me.dbServerNameTextBox.TabIndex = 4
            Me.dbServerNameTextBox.Text = "(local)"
            '
            'label11
            '
            Me.label11.Location = New System.Drawing.Point(88, 144)
            Me.label11.Name = "label11"
            Me.label11.Size = New System.Drawing.Size(80, 23)
            Me.label11.TabIndex = 3
            Me.label11.Text = "Server Name:"
            '
            'label10
            '
            Me.label10.Location = New System.Drawing.Point(16, 72)
            Me.label10.Name = "label10"
            Me.label10.Size = New System.Drawing.Size(456, 56)
            Me.label10.TabIndex = 2
            Me.label10.Text = "Enter the name of the SQL server to use and an account with permissions to create" & _
            " databases on that machine. If you choose the Windows Authentication option, you" & _
            "r Windows account will need to have administrative privileges on that machine."
            '
            'panel5
            '
            Me.panel5.BackColor = System.Drawing.Color.White
            Me.panel5.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
            Me.panel5.Controls.Add(Me.label9)
            Me.panel5.Dock = System.Windows.Forms.DockStyle.Top
            Me.panel5.Location = New System.Drawing.Point(0, 0)
            Me.panel5.Name = "panel5"
            Me.panel5.Size = New System.Drawing.Size(536, 60)
            Me.panel5.TabIndex = 1
            '
            'label9
            '
            Me.label9.Font = New System.Drawing.Font("Tahoma", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
            Me.label9.Location = New System.Drawing.Point(24, 16)
            Me.label9.Name = "label9"
            Me.label9.Size = New System.Drawing.Size(128, 23)
            Me.label9.TabIndex = 0
            Me.label9.Text = "Database Setup"
            '
            'setupFailedPanel
            '
            Me.setupFailedPanel.Controls.Add(Me.failureReasonTextBox)
            Me.setupFailedPanel.Controls.Add(Me.label20)
            Me.setupFailedPanel.Controls.Add(Me.label18)
            Me.setupFailedPanel.Controls.Add(Me.panel8)
            Me.setupFailedPanel.Location = New System.Drawing.Point(147, 105)
            Me.setupFailedPanel.Name = "setupFailedPanel"
            Me.setupFailedPanel.TabIndex = 5
            Me.setupFailedPanel.Visible = False
            '
            'failureReasonTextBox
            '
            Me.failureReasonTextBox.Location = New System.Drawing.Point(24, 96)
            Me.failureReasonTextBox.Multiline = True
            Me.failureReasonTextBox.Name = "failureReasonTextBox"
            Me.failureReasonTextBox.Size = New System.Drawing.Size(448, 112)
            Me.failureReasonTextBox.TabIndex = 4
            Me.failureReasonTextBox.Text = ""
            '
            'label20
            '
            Me.label20.Location = New System.Drawing.Point(24, 80)
            Me.label20.Name = "label20"
            Me.label20.TabIndex = 3
            Me.label20.Text = "Reason:"
            '
            'label18
            '
            Me.label18.Location = New System.Drawing.Point(24, 248)
            Me.label18.Name = "label18"
            Me.label18.Size = New System.Drawing.Size(440, 48)
            Me.label18.TabIndex = 2
            Me.label18.Text = "Click Back to review the setup parameters or Cancel to exit the Wizard."
            '
            'panel8
            '
            Me.panel8.BackColor = System.Drawing.Color.White
            Me.panel8.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
            Me.panel8.Controls.Add(Me.label19)
            Me.panel8.Dock = System.Windows.Forms.DockStyle.Top
            Me.panel8.Location = New System.Drawing.Point(0, 0)
            Me.panel8.Name = "panel8"
            Me.panel8.Size = New System.Drawing.Size(200, 60)
            Me.panel8.TabIndex = 1
            '
            'label19
            '
            Me.label19.Font = New System.Drawing.Font("Tahoma", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
            Me.label19.Location = New System.Drawing.Point(24, 16)
            Me.label19.Name = "label19"
            Me.label19.Size = New System.Drawing.Size(280, 23)
            Me.label19.TabIndex = 0
            Me.label19.Text = "Sample Setup Failed"
            '
            'frmSetup
            '
            Me.AcceptButton = Me.nextButton
            Me.AutoScaleBaseSize = New System.Drawing.Size(6, 15)
            Me.CancelButton = Me.m_CancelButton
            Me.ClientSize = New System.Drawing.Size(520, 397)
            Me.ControlBox = False
            Me.Controls.Add(Me.panel1)
            Me.Controls.Add(Me.panel2)
            Me.Font = New System.Drawing.Font("Tahoma", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
            Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
            Me.MaximizeBox = False
            Me.MinimizeBox = False
            Me.Name = "frmSetup"
            Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
            Me.Text = "Foundstone Hacme Bank Setup Wizard"
            Me.panel2.ResumeLayout(False)
            Me.panel1.ResumeLayout(False)
            Me.installingPanel.ResumeLayout(False)
            Me.panel10.ResumeLayout(False)
            Me.configDBPanel2.ResumeLayout(False)
            Me.dbConnectionGroupBox.ResumeLayout(False)
            Me.panel5.ResumeLayout(False)
            Me.setupFailedPanel.ResumeLayout(False)
            Me.panel8.ResumeLayout(False)
            Me.ResumeLayout(False)

        End Sub 'InitializeComponent
#End Region
        '/ <summary>
        '/ The main entry point for the application.
        '/ </summary>
        <STAThread()> Overloads Shared _
        Sub Main(ByVal CmdArgs() As String)
            If CmdArgs.Length = 0 Then
                Application.Run(New frmSetup(False))
            Else
                If CmdArgs(0).ToUpper() = "/UNINSTALL" Then
                    Application.Run(New frmSetup(True))
                End If
            End If
        End Sub 'Main

        Public Sub Install()
            m_install.Run()
        End Sub 'Install

        Private Sub ChangePage(ByVal panel As Panel)
            If Not (m_currentPage Is Nothing) Then
                m_currentPage.Visible = False
                m_currentPage.Dock = DockStyle.None
            End If
            m_currentPage = panel
            m_currentPage.Visible = True
            m_currentPage.Dock = DockStyle.Fill
        End Sub 'ChangePage

        Private Sub InstallStep(ByVal sender As Object, ByVal e As InstallEventArgs)
            progressBar1.Value = e.Step
            installStepLabel.Text = e.Message
            Application.DoEvents()
        End Sub 'InstallStep

        Private Sub Form1_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Load
            ChangePage(configDBPanel2)
        End Sub 'Form1_Load

        Private Sub cancelButton_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles m_CancelButton.Click
            If MessageBox.Show(Me, "Are you sure you want to exit the Setup Wizard before setting up the database?", "Foundstone Hacme Bank Setup Wizard", MessageBoxButtons.YesNo) = DialogResult.Yes Then
                Close()
            End If
        End Sub 'cancelButton_Click

        Private Sub nextButton_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles nextButton.Click
            If m_currentPage Is configDBPanel2 Then
                If CheckDatabaseConnection() Then
                    usernameParameter = dbUserNametextBox.Text.ToString()
                    passwordParameter = dbPasswordtextBox.Text.ToString()
                    Me.Enabled = False
                    Try
                        ChangePage(installingPanel)
                        Application.DoEvents()
                        Install()
                    Catch ex As Exception
                        failureReasonTextBox.Text = ex.Message
                        ChangePage(setupFailedPanel)
                        Exit Sub
                    Finally
                        Me.Enabled = True
                        Me.Close()
                    End Try
                End If
            Else
                Me.Close()
            End If
        End Sub 'nextButton_Click

        Private Sub backButton_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles backButton.Click
            If m_currentPage Is setupFailedPanel Then
                ChangePage(configDBPanel2)
            End If
        End Sub 'backButton_Click

        Private Function CheckDatabaseConnection() As Boolean
            Try
                If dbTrustedConnectionRadioButton.Checked Then
                    m_install.ConnectionData = New ConnectionData(dbServerNameTextBox.Text, "FoundStone_Bank")
                Else
                    m_install.ConnectionData = New ConnectionData(dbServerNameTextBox.Text, "FoundStone_Bank", dbUserNametextBox.Text, dbPasswordtextBox.Text)
                End If

                SqlServer.CheckOSQLConnection(m_install.ConnectionData)

                If (Not m_unInstall) Then
                    If SqlServer.DatabaseExists(m_install.ConnectionData, "FoundStone_Bank") Then
                        Dim dr As DialogResult = MessageBox.Show(Me, "The database for Foundstone Hacme Bank already exists. Would you like to recreate it?", "Foundstone Hacme Bank Setup Wizard", MessageBoxButtons.YesNo)
                        If dr = DialogResult.Yes Then
                            m_install.CreateDatabase = True
                        Else
                            m_install.CreateDatabase = False
                        End If
                    Else
                        m_install.CreateDatabase = True
                    End If
                End If

            Catch e As Exception
                MessageBox.Show(Me, [String].Format("Error connecting to the SQL Server machine '{0}'." + ControlChars.Lf + "Error message:" + ControlChars.Lf + "{1}", dbServerNameTextBox.Text, e.Message), "Foundstone Hacme Bank Setup Wizard")
                Return False
            End Try
            Return True
        End Function 'CheckDatabaseConnection

        Private Sub configDBPanel2_VisibleChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles configDBPanel2.VisibleChanged
            nextButton.Enabled = True
            backButton.Visible = False
            m_CancelButton.Enabled = True
        End Sub 'configDBPanel2_VisibleChanged

        Private Sub installingPanel_VisibleChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles installingPanel.VisibleChanged
            nextButton.Enabled = False
            backButton.Visible = True
            backButton.Enabled = False
            m_CancelButton.Enabled = False
        End Sub 'installingPanel_VisibleChanged

        Private Sub setupFailedPanel_VisibleChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles setupFailedPanel.VisibleChanged
            nextButton.Enabled = False
            backButton.Enabled = True
            m_CancelButton.Enabled = True
        End Sub 'setupFailedPanel_VisibleChanged
    End Class 'frmSetup 
End Namespace