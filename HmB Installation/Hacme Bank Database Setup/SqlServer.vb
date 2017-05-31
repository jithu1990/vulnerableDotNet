Imports System.Diagnostics
Imports System.Data
Imports System.Data.SqlClient
Imports System.Text

Public Class ConnectionData
    Private m_serverName As String
    Private m_databaseName As String
    Private m_userName As String
    Private m_password As String
    Private m_trustedConnection As String

    Public Sub New()
        m_serverName = "(local)"
        m_databaseName = "master"
        m_trustedConnection = True
    End Sub

    Public Sub New(ByVal serverName As String, ByVal databaseName As String)
        m_serverName = serverName
        m_databaseName = databaseName
        m_trustedConnection = True
    End Sub

    Public Sub New(ByVal serverName As String, ByVal databaseName As String, _
        ByVal userName As String, ByVal password As String)
        m_serverName = serverName
        m_databaseName = databaseName
        m_userName = userName
        m_password = password
        m_trustedConnection = False
    End Sub

    Public ReadOnly Property ConnectionString() As String
        Get
            If m_trustedConnection Then
                Return FormatConnectionString(m_serverName, m_databaseName)
            Else
                Return FormatConnectionString(m_serverName, m_databaseName, m_userName, m_password)
            End If
        End Get
    End Property

    Public ReadOnly Property ServerName() As String
        Get
            Return m_serverName
        End Get
    End Property

    Public ReadOnly Property DatabaseName() As String
        Get
            Return m_databaseName
        End Get
    End Property

    Public ReadOnly Property UserName() As String
        Get
            Return m_userName
        End Get
    End Property

    Public ReadOnly Property Password() As String
        Get
            Return m_password
        End Get
    End Property

    Public ReadOnly Property TrustedConnection() As Boolean
        Get
            Return m_trustedConnection
        End Get
    End Property

    '
    ' Create an ADO.NET connection string
    '
    Private Function CreateConnectionStringStringBuilder(ByVal serverName As String, _
        ByVal databaseName As String)
        Dim connectionString As StringBuilder = New StringBuilder
        With connectionString
            .Append("Server=")
            .Append(serverName)
            .Append(";")
            .Append("Database=")
            .Append(databaseName)
            .Append(";")
        End With
        Return connectionString
    End Function

    Private Function FormatConnectionString(ByVal serverName As String, _
        ByVal databaseName As String) As String
        Dim connectionString As StringBuilder
        connectionString = CreateConnectionStringStringBuilder(serverName, databaseName)
        With connectionString
            .Append("Trusted_Connection=true")
        End With
        Return connectionString.ToString()
    End Function

    Private Function FormatConnectionString(ByVal serverName As String, _
        ByVal databaseName As String, _
        ByVal userName As String, _
        ByVal password As String) As String
        Dim connectionString As StringBuilder
        connectionString = CreateConnectionStringStringBuilder(serverName, databaseName)
        With connectionString
            .Append("User Id=")
            .Append(userName)
            .Append(";")
            .Append("password=")
            .Append(password)
        End With
        Return connectionString.ToString()
    End Function
End Class

Public Class SqlServer

    Private Class OSqlParameters
        Private m_connectionData As ConnectionData
        Private m_onBatchAbort As Boolean = True
        Private m_scriptFileName As String = Nothing
        Private m_commandText As String = Nothing

        Public Sub New(ByVal connectionData As ConnectionData)
            m_connectionData = connectionData
        End Sub

        Public Property ScriptFileName() As String
            Get
                Return m_scriptFileName
            End Get
            Set(ByVal Value As String)
                m_scriptFileName = Value
            End Set
        End Property

        Public Property CommandText() As String
            Get
                Return m_commandText
            End Get
            Set(ByVal Value As String)
                m_commandText = Value
            End Set
        End Property

        Public Function GetArguments() As String
            Dim sb As StringBuilder = New StringBuilder()
            sb.Append(" -S ")
            sb.Append(m_connectionData.ServerName)
            sb.Append(" -d ")
            sb.Append(m_connectionData.DatabaseName)
            If m_connectionData.TrustedConnection Then
                sb.Append(" -E ")
            Else
                sb.Append(" -U ")
                sb.Append(m_connectionData.UserName)
                sb.Append(" -P ")
                sb.Append(m_connectionData.Password)
            End If

            If m_onBatchAbort Then
                sb.Append(" -b ")
            End If

            If Not m_scriptFileName Is Nothing Then
                sb.Append(" -i """)
                sb.Append(m_scriptFileName)
                sb.Append(""" ")
            Else
                If Not m_commandText Is Nothing Then
                    sb.Append(" -Q """)
                    sb.Append(m_commandText)
                    sb.Append(""" ")

                End If
            End If
            Return sb.ToString()
        End Function

    End Class

    '
    ' RunOSQL executes OSQL.EXE
    '
    Public Shared Sub RunOSQL(ByVal arguments As String)
        Dim psi As ProcessStartInfo = New ProcessStartInfo("OSQL.EXE", arguments)
        psi.UseShellExecute = False
        psi.RedirectStandardOutput = True
        psi.CreateNoWindow = True

        Dim p As Process
        Dim errorMessage As String
        p = Process.Start(psi)
        Try
            errorMessage = p.StandardOutput.ReadToEnd()
            p.WaitForExit() ' wait a 1/10th of a second
            ' process has ended, throw exception if 
            ' exit code not zero
            If p.ExitCode <> 0 Then
                Throw New Exception( _
                    String.Format("OSQL.EXE exit code {0}" & vbCrLf & "{1}", _
                        p.ExitCode, _
                        errorMessage))
            End If
        Finally
            ' should never happen, but let's play it safe here
            If Not p.HasExited Then
                p.Kill()
            End If
        End Try
    End Sub

    '
    ' RunOSQL executes OSQL.EXE
    ' this version returns true only if the script executed without an error. It
    ' does not throw any exceptions
    '
    Public Shared Sub RunOSQL(ByVal arguments As String, ByRef successful As Boolean)
        Try
            RunOSQL(arguments)
            successful = True
        Catch
            successful = False
        End Try
    End Sub

    '
    ' CheckOSQL checks if OSQL.EXE is present in this machine
    '
    Public Shared Function CheckOSQL() As Boolean
        Dim p As Process
        Dim successful As Boolean = True
        Try
            p = Process.Start("OSQL.EXE", "-?")
        Catch
            successful = False
        Finally
            If Not p.HasExited Then
                p.Kill()
            End If
        End Try
        Return successful
    End Function

    '
    ' CheckOSQLConnection verifies if OSQL.EXE can connect to the specified
    ' database server
    '
    Public Shared Sub CheckOSQLConnection(ByVal connectionData As ConnectionData)
        Dim parameters As OSqlParameters = New OSqlParameters(GetMasterConnectionData(connectionData))
        parameters.CommandText = "SELECT @@VERSION"
        RunOSQL(parameters.GetArguments())
    End Sub

    ' 
    ' ExecuteScript executes a SQL script using OSQL.EXE
    ' Anything but success raises an exception
    '
    Public Shared Sub ExecuteScript(ByVal connectionData As ConnectionData, _
        ByVal scriptName As String)

        Dim parameters As OSqlParameters = New OSqlParameters(connectionData)
        parameters.ScriptFileName = scriptName
        RunOSQL(parameters.GetArguments(), 30000)
    End Sub

    Private Shared Function GetMasterConnectionData(ByVal connectionData As ConnectionData) As ConnectionData
        Dim cd As connectionData
        If connectionData.TrustedConnection Then
            cd = New connectionData(connectionData.ServerName, "master")
        Else
            cd = New connectionData(connectionData.ServerName, "master", connectionData.UserName, connectionData.Password)
        End If
        Return cd
    End Function

    '
    ' Database manipulation methods. These methods use ADO.NET instead of OSQL.EXE
    '
    Public Shared Sub CreateDatabase(ByVal connectionData As ConnectionData, _
        ByVal databaseName As String)
        Dim command As StringBuilder = New StringBuilder()
        command.Append("CREATE DATABASE ")
        command.Append(databaseName)
        ExecuteNonQuery(GetMasterConnectionData(connectionData), command.ToString())
    End Sub

    Public Shared Sub DropDatabase(ByVal connectionData As ConnectionData, _
        ByVal databaseName As String)
        Dim command As StringBuilder = New StringBuilder()
        command.Append("DROP DATABASE ")
        command.Append(databaseName)

        ExecuteNonQuery(GetMasterConnectionData(connectionData), command.ToString())
    End Sub

    Public Shared Function DatabaseExists(ByVal connectionData As ConnectionData, _
        ByVal databaseName As String) As Boolean
        Dim command As StringBuilder = New StringBuilder()
        command.Append("select count(*) from master..sysdatabases where name = '")
        command.Append(databaseName)
        command.Append("'")

        Dim count As Integer
        count = CType(ExecuteScalar(GetMasterConnectionData(connectionData), command.ToString()), Integer)
        Return count > 0
    End Function

    '
    ' Runs a SQL command using ADO.NET
    '
    Public Shared Sub ExecuteNonQuery(ByVal connectionData As ConnectionData, ByVal command As String)
        Dim cn As SqlConnection = New SqlConnection(connectionData.ConnectionString)
        cn.Open()
        Try
            Dim cmd As SqlCommand = New SqlCommand()
            cmd.Connection = cn
            cmd.CommandText = command
            cmd.CommandType = CommandType.Text
            cmd.ExecuteNonQuery()
        Finally
            cn.Close()
        End Try
    End Sub

    Public Shared Function ExecuteScalar(ByVal connectionData As ConnectionData, ByVal command As String) As Object
        Dim cn As SqlConnection = New SqlConnection(connectionData.ConnectionString)
        Dim returnValue As Object
        cn.Open()
        Try
            Dim cmd As SqlCommand = New SqlCommand()
            cmd.Connection = cn
            cmd.CommandText = command
            cmd.CommandType = CommandType.Text
            returnValue = cmd.ExecuteScalar()
        Finally
            cn.Close()
        End Try
        Return returnValue
    End Function

    Public Shared Sub GrantLogin( _
        ByVal connectionData As ConnectionData, _
        ByVal windowsUserName As String)
        Dim command As StringBuilder
        command = New StringBuilder()
        command.Append("sp_grantlogin '")
        command.Append(windowsUserName)
        command.Append("'")
        ExecuteNonQuery(connectionData, command.ToString())
    End Sub

    Public Shared Sub GrantDBAccess( _
        ByVal connectionData As ConnectionData, _
        ByVal windowsUserName As String, _
        ByVal sqlUserName As String)

        Dim command As StringBuilder
        command = New StringBuilder()
        command.Append("sp_grantdbaccess '")
        command.Append(windowsUserName)
        command.Append("', '")
        command.Append(sqlUserName)
        command.Append("'")
        ExecuteNonQuery(connectionData, command.ToString())
    End Sub

    Public Shared Sub AddRoleMember( _
        ByVal connectionData As ConnectionData, _
        ByVal roleName As String, _
        ByVal sqlUserName As String)

        Dim command As StringBuilder
        command = New StringBuilder()
        command.Append("sp_addrolemember '")
        command.Append(roleName)
        command.Append("', '")
        command.Append(sqlUserName)
        command.Append("'")
        ExecuteNonQuery(connectionData, command.ToString())
    End Sub
End Class

