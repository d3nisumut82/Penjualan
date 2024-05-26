Imports System.Data
Imports System.Configuration
Imports System.Web
Imports System.Web.Security
Imports System.Web.UI
Imports System.Web.UI.WebControls
Imports System.Web.UI.WebControls.WebParts
Imports System.Web.UI.HtmlControls
Imports System.Data.SqlClient
Imports System.Net
Imports System.IO
Imports System.Web.Services
Imports Newtonsoft.Json
Imports System.ServiceModel.Syndication

Partial Class EnterpriseHome
    Inherits System.Web.UI.Page
    Private sConnstring As String = clsEnt.strKoneksi("connuq")
    Dim mySqlCon As Data.OleDb.OleDbConnection
    Dim mySqlCmd As Data.OleDb.OleDbCommand
    Private li_menu As HtmlGenericControl
    Private anchor_menu As HtmlGenericControl

    Private dtSelectMenu As DataTable
    Private MenuDataView As DataView
    Private xverb As String = "GET"  'GET
    Private xusername As String = "X3SAPLpZASwKZP5uI7SDdgro1RSNQCTsduSY8JeGa4A="
    Private xpassword As String = "/BxNrv0OSg/pH7Puz+vNvev4hmFOzk8g5fhG6mk7jj1tl0B8/3sBlh43RdqTaox8"
    Private xid As String = "d7dcfba7-42e9-456c-9f2d-7cb953c4072b"

    Private xtoken_create As String = "username=" & xusername & "&password=" & xpassword & "&id_pengguna=" & xid
    Dim DosenlineItem As DataTable
    Dim mySqlReader As Data.OleDb.OleDbDataReader
    Dim Workspace As String

    Public Sub connectDB()
        Dim sConnString As String
        sConnString = clsEnt.StringOle
        mySqlCon = New Data.OleDb.OleDbConnection(sConnString)
    End Sub


    Public Shared Function GetDataTabel(query As String) As DataTable
        Dim conString As String = clsEnt.strKoneksi("connuq")
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



    Private Function GetData(query As String) As DataTable
        Dim dt As New DataTable()
        Dim constr As String = ConfigurationManager.ConnectionStrings("Connuq").ConnectionString
        Using con As New SqlConnection(constr)
            Using cmd As New SqlCommand(query)
                Using sda As New SqlDataAdapter()
                    cmd.CommandType = CommandType.Text
                    cmd.Connection = con
                    sda.SelectCommand = cmd
                    sda.Fill(dt)
                    sda.Dispose()
                End Using
            End Using
            con.Close()
            con.Dispose()
            Return dt
        End Using


    End Function




#Region "POP UP"
    Private Sub ShowSweetAlert2(ByVal icon As String, ByVal titleString As String)
        Dim sweetAlertString As String = "setTimeout(function(){ Swal.fire({ icon: '" & icon & "', title: '" & titleString & "', showConfirmButton: false, showCancelButton: false, timer: 3000, timerProgressBar: true }); }, 100);"
        ScriptManager.RegisterStartupScript(Me, Me.[GetType](), "Popup", sweetAlertString, True)
    End Sub
#End Region
    Public Sub Usekosong()
        Dim sUsr As String = Session("UserAuthentication")
        If sUsr = "" Then
            ShowSweetAlert2("info", "Anda Login Terdahulu!")
            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "Redirect", "setTimeout(function () {  window.location.href ='Default.aspx' }, 2000);", True)
        End If
    End Sub

    Public Sub KosongAja()
        Dim sUsr As String = Session("UserAuthentication")
        If Not String.IsNullOrWhiteSpace(sUsr) Then
            ShowSweetAlert2("info", "Anda Login Terdahulu!")
            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "Redirect", "setTimeout(function () {  window.location.href ='Default.aspx' }, 2000);", True)
        End If
    End Sub
    Protected Sub Page_Load(sender As Object, e As System.EventArgs) Handles Me.Load

        If Not IsPostBack Then
            Dim constr As String = ConfigurationManager.ConnectionStrings("connuq").ConnectionString
            '  KosongAja()
            Usekosong()


        Else

        End If


    End Sub

    Protected Sub imgbtn_Click(ByVal sender As Object, ByVal e As ImageClickEventArgs)
        Dim btndetails As ImageButton = TryCast(sender, ImageButton)
        Dim gvrow As GridViewRow = DirectCast(btndetails.NamingContainer, GridViewRow)
    End Sub
    Protected Sub imgbtn2_Click(ByVal sender As Object, ByVal e As ImageClickEventArgs)
        Dim btndetails As ImageButton = TryCast(sender, ImageButton)
    End Sub


    Private Function GetDataTbl(query As String) As DataTable
        Dim conString As String = ConfigurationManager.ConnectionStrings("connuq").ConnectionString
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





    Protected Sub whatsappMyFile(sender As Object, e As EventArgs)

        Dim sTrue2 As DataTable
        Dim sTrue As DataTable
        Dim waUQ As String = "UQ"
        Dim waUQB As String = "UQB"
        Dim ket As String = "Kepada Bapak/Ibu Mohon Di Validasi Data Saya"
        Dim sUsr As String = Session("UserAuthentication")


        sTrue2 = GetDataTbl("select  Homebase as Homebase from dosen where iddosen='" & sUsr & "'")
        Dim ketUniversitas As String = sTrue2.Rows(0).Item("Homebase")

        sTrue = GetDataTbl("select  HP as HPwa from TblBAKQuality where homeBase='" & ketUniversitas & "'")
        Dim kethp As String = sTrue.Rows(0).Item("HPwa")
        'Dim webwhatsapp As String = "https://api.whatsapp.com/send?phonee='" & kethp & "'&text='" & ket.Replace(" ", "+") & "'"

        'Dim uri As String = "https://api.whatsapp.com/send?phone='" & kethp & "'&text=" & ket.Replace(" ", "+") & ""
        'Dim request As Net.HttpWebRequest = Net.HttpWebRequest.Create(uri)
        Dim address As Uri = New Uri("https://api.whatsapp.com/send?phonee='" & kethp & "'&text='" & ket.Replace(" ", "+") & "'")
        MsgBox("http://" & address.Host)

    End Sub

    Protected Sub GridView1_RowDataBound(sender As Object, e As GridViewRowEventArgs) Handles GridView1.RowDataBound

    End Sub

    Protected Sub GridView2_RowDataBound(sender As Object, e As GridViewRowEventArgs) Handles GridView2.RowDataBound

    End Sub

    Protected Sub GridView3_RowDataBound(sender As Object, e As GridViewRowEventArgs) Handles GridView3.RowDataBound

    End Sub

    Protected Sub GridView4_RowDataBound(sender As Object, e As GridViewRowEventArgs) Handles GridView4.RowDataBound

    End Sub
End Class
