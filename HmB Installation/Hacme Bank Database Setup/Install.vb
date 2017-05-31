Option Strict On
Option Explicit On 

Imports System
Imports System.IO
Imports System.Data
Imports System.Data.SqlClient
Imports System.Xml
Imports System.Windows.Forms

Namespace HacmeBankDBSetup
    Public Class InstallEventArgs
        Inherits System.EventArgs
        Private m_message As String
        Public Shared Database_Name As String

        Public Property Message() As String
            Get
                Return m_message
            End Get
            Set(ByVal Value As String)
                m_message = Value
            End Set
        End Property

        Private m_step As Integer

        Public Property [Step]() As Integer
            Get
                Return m_step
            End Get
            Set(ByVal Value As Integer)
                m_step = Value
            End Set
        End Property
    End Class 'InstallEventArgs

    Public Delegate Sub InstallEventHandler(ByVal sender As Object, ByVal e As InstallEventArgs)

    '/ <summary>
    '/ Summary description for Install.
    '/ </summary>
    Public Class Install
        Private m_step As New InstallEventArgs
        Private m_basePath As String

        Property BasePath() As String
            Get
                Return m_basePath
            End Get
            Set(ByVal Value As String)
                m_basePath = Value
            End Set
        End Property
        Private m_unInstall As Boolean = False
        Private m_connectionData As ConnectionData
        Private m_createDatabase As Boolean
        Private m_createVirtualDirectories As Boolean = True

        Public Const DatabaseName As String = "FoundStone_Bank"
        Public Const CreateTablesScript As String = "FoundStoneBank_export.sql"

        ' Events
        Public Event [Step] As InstallEventHandler

        Protected Overridable Sub OnStep(ByVal e As InstallEventArgs)
            RaiseEvent [Step](Me, e)
        End Sub 'OnStep

        Public Sub New(ByVal basePath As String, ByVal unInstall As Boolean)
            m_basePath = basePath
            m_unInstall = unInstall
        End Sub 'New

        Public Property ConnectionData() As ConnectionData
            Get
                Return m_connectionData
            End Get
            Set(ByVal Value As ConnectionData)
                m_connectionData = Value
            End Set
        End Property

        Public Property CreateDatabase() As Boolean '
            Get
                Return m_createDatabase
            End Get
            Set(ByVal Value As Boolean)
                m_createDatabase = Value
            End Set
        End Property

        Public Sub Run()
            If m_unInstall Then
                If SqlServer.DatabaseExists(m_connectionData, Install.DatabaseName) Then
                    m_step.Message = "Dropping Database"
                    m_step.Step += 1
                    OnStep(m_step)
                    SqlServer.DropDatabase(m_connectionData, Install.DatabaseName)
                End If
            Else
                If m_createDatabase Then
                    Dim e As New InstallEventArgs
                    If SqlServer.DatabaseExists(m_connectionData, Install.DatabaseName) Then
                        m_step.Message = "Dropping Database"
                        m_step.Step += 1
                        OnStep(m_step)
                        SqlServer.DropDatabase(m_connectionData, Install.DatabaseName)
                    End If

                    m_step.Message = "Creating Database"
                    m_step.Step += 1
                    OnStep(m_step)
                    SqlServer.CreateDatabase(m_connectionData, Install.DatabaseName)

                    m_step.Message = "Creating Tables and Stored Procedures"
                    m_step.Step += 3
                    OnStep(m_step)
                    SqlServer.ExecuteScript(m_connectionData, Path.Combine(m_basePath, Install.CreateTablesScript))
                    If [String].Compare(System.Environment.MachineName, m_connectionData.ServerName, True) = 0 Or [String].Compare("(local)", m_connectionData.ServerName, True) = 0 Then
                        If m_connectionData.TrustedConnection Then
                            Dim windowsUserName As String
                            windowsUserName = System.Environment.MachineName + "\ASPNET"
                            SqlServer.GrantLogin(m_connectionData, windowsUserName)
                            SqlServer.GrantDBAccess(m_connectionData, windowsUserName, "ASPNET")
                            SqlServer.AddRoleMember(m_connectionData, "db_datareader", "ASPNET")
                            SqlServer.AddRoleMember(m_connectionData, "db_datawriter", "ASPNET")

                            windowsUserName = "NT AUTHORITY\NETWORK SERVICE"
                            SqlServer.GrantLogin(m_connectionData, windowsUserName)
                            SqlServer.GrantDBAccess(m_connectionData, windowsUserName, "NETWORK SERVICE")
                            SqlServer.AddRoleMember(m_connectionData, "db_datareader", "NETWORK SERVICE")
                            SqlServer.AddRoleMember(m_connectionData, "db_datawriter", "NETWORK SERVICE")

                        End If
                    End If
                End If

                If File.Exists(Application.StartupPath + "\\..\\Web.config") Then
                    Dim FS_R As New FileStream(Application.StartupPath + "\\..\\Web.config", FileMode.Open)
                    Dim FS_W As New FileStream(Application.StartupPath + "\\..\\WebTemp.config", FileMode.Create)
                    Dim R As New StreamReader(FS_R)
                    Dim W As New StreamWriter(FS_W)
                    Dim strFileContents As String = ""
                    Dim i As Integer
                    i = 0
                    Dim str As String
                    Do
                        str = R.ReadLine()
                        If Not str.IndexOf("FoundStone_Connection") = -1 Then
                            strFileContents = strFileContents + "<add key=""FoundStone_Connection"" value=""" + m_connectionData.ConnectionString + """/>"
                        Else
                            strFileContents = strFileContents + str
                        End If
                        If str = "</configuration>" Then
                            W.WriteLine(strFileContents.ToString)
                            W.Flush()
                            W.Close()
                            R.Close()
                            Exit Do
                        End If
                        strFileContents = strFileContents + vbCrLf
                    Loop
                    File.Delete(Application.StartupPath + "\\..\\Web.config")
                    File.Move(Application.StartupPath + "\\..\\WebTemp.config", Application.StartupPath + "\\..\\Web.config")
                Else
                    MessageBox.Show("Web.config File Not Found")
                End If
            End If
        End Sub 'Run
    End Class 'Install
End Namespace 'HacmeBankDBSetup