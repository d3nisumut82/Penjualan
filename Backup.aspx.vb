Imports System.Data
Imports System.Data.SqlClient
Imports System.IO
Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Web
Imports System.Text
Imports iTextSharp.text
Imports iTextSharp.text.pdf
Imports System.Web.UI
Imports System.Web.UI.WebControls
Imports System.Collections
Partial Class Backup
    Inherits System.Web.UI.Page
    Dim mySqlCon As Data.OleDb.OleDbConnection
    Dim mySqlCmd As Data.OleDb.OleDbCommand
    Dim mySqlReader As Data.OleDb.OleDbDataReader
    Dim SQL As String
    Private li_menu As HtmlGenericControl
    Private anchor_menu As HtmlGenericControl
    Private dtSelectMenu As DataTable
    Private MenuDataView As DataView

#Region "PageError"

    Private Shared _getDataTabel As DataTable

    Private Shared Property GetDataTabel(Str As String) As DataTable
        Get
            Return _getDataTabel
        End Get
        Set(value As DataTable)
            _getDataTabel = value
        End Set
    End Property

    Protected Sub Page_Error(sender As Object, e As System.EventArgs) Handles Me.Error
        WriteError("Page Level Error Handled : " + Server.GetLastError().Message)
        Server.ClearError()
    End Sub

    Private Sub WriteError(messg As String)
        Response.Write("<div><h1>Page Error </h1><div class='UserControlDiv'>Error on the Page : <b>" + messg + "</b></div></div>")
    End Sub

#End Region

#Region "Button"

    Protected Sub lnkSave_Click(sender As Object, e As System.EventArgs)
        If dtpTgl.Value = "" Then
            ScriptManager.RegisterClientScriptBlock(Me, GetType(Page), UniqueID, "javascript:BootstrapDialog.alert('Tanggal backup tidak boleh Kosong!');", True)
            dtpTgl.Focus()
            Exit Sub
        End If

        connectDB()
        mySqlCon.Open()
        Dim command As Data.OleDb.OleDbCommand = New Data.OleDb.OleDbCommand()
        Dim Transaction = mySqlCon.BeginTransaction(IsolationLevel.ReadCommitted)
        Dim satinfo As Integer = 3
        command.Connection = mySqlCon
        command.Transaction = Transaction                           '*** Command & Transaction ***'
        Try

            command.Parameters.Clear()
            Dim dDate As String = dtpTgl.Value.Replace("/", "").Replace(" ", "")
            Dim dDay As String = dDate.Substring(0, 2) '30
            Dim dMonth As String = dDate.Substring(2, 2) '10
            Dim dYear As String = dDate.Substring(dDate.Length - 4, 4) '2020
            command.CommandText = command.CommandText & "backup database Ptbas to disk='E:\backup\ptbas_" & dDate & ".bak'; "

            command.ExecuteNonQuery()
            Transaction.Commit() '*** Commit Transaction ***' 
            ScriptManager.RegisterClientScriptBlock(Me, GetType(Page), UniqueID, "javascript:BootstrapDialog.alert('Backup Data Succesfully ! ');", True)
        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Me, GetType(Page), UniqueID, "javascript:BootstrapDialog.alert('Error Saving Data ! ');", True)
        End Try
        mySqlCon.Close()
    End Sub
#End Region

#Region "Setting"

    Public Shared Function ChkDateSys(aStrDate As String) As String
        Dim Str As String = "select TGL from INTERFERENSITGL where TGL >= '" & aStrDate & "'"
        ChkDateSys = "NotFound"
        Dim dkd As DataTable = GetDataTabel(Str)
        If dkd.Rows.Count >= 1 Then
            ChkDateSys = "Found"
        ElseIf dkd.Rows.Count = 0 Then
            ChkDateSys = "NotFound"
        End If
        dkd = Nothing
        Return ChkDateSys
    End Function


    Private Sub Page_PreInit(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreInit
        System.Threading.Thread.CurrentThread.CurrentCulture = System.Globalization.CultureInfo.CreateSpecificCulture("en-US")
        System.Threading.Thread.CurrentThread.CurrentUICulture = System.Threading.Thread.CurrentThread.CurrentCulture
    End Sub

    Public Sub connectDB()
        Dim sConnString As String
        sConnString = clsEnt.StringOle
        mySqlCon = New Data.OleDb.OleDbConnection(sConnString)
    End Sub

    Private Function GetDataTbl(query As String) As DataTable
        Dim conString As String = ConfigurationManager.ConnectionStrings("connJP").ConnectionString
        Dim cmd As New SqlCommand(query)
        Using con As New SqlConnection(conString)
            con.Open()
            Using sda As New SqlDataAdapter()
                cmd.Connection = con
                sda.SelectCommand = cmd
                Using dt As New DataTable()
                    sda.Fill(dt)
                    Return dt
                    sda.Dispose()
                End Using
            End Using
            con.Close()
        End Using
        cmd.Dispose()
    End Function

#End Region


End Class
