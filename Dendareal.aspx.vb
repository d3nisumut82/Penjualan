Imports System.Data
Imports System.Configuration
Imports System.Web
Imports System.Web.Security
Imports System.Web.UI
Imports System.Web.UI.WebControls
Imports System.Web.UI.WebControls.WebParts
Imports System.Web.UI.HtmlControls
Imports System.Data.SqlClient
Imports System.Web.Services
Imports System.IO

Imports System.Text
Imports System.Drawing

Imports System
Imports System.Collections.Generic
Imports System.Linq

Partial Class Dendareal
    Inherits System.Web.UI.Page

#Region "Variables & Connect DB"

    Dim mySqlCon As Data.OleDb.OleDbConnection
    Dim mySqlCmd As Data.OleDb.OleDbCommand
    Dim SQL As String
    Dim DataLineItemAccount As DataTable
    Dim editLineItemAccount As Integer
    Private sConnstring As String = clsEnt.strKoneksi("connuq")
    Private li_menu As HtmlGenericControl
    Private anchor_menu As HtmlGenericControl
    Private dtSelectMenu As DataTable
    Dim mySqlReader As Data.OleDb.OleDbDataReader
    Private MenuDataView As DataView

    Public Sub connectDB()
        Dim sConnString As String
        sConnString = clsEnt.StringOle
        mySqlCon = New Data.OleDb.OleDbConnection(sConnString)
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

#End Region

#Region "Page Load"
    'Set Ambil Data Dari List SetDefaultBank
    Protected Sub Page_Load(sender As Object, e As System.EventArgs) Handles Me.Load
        Dim sUsr As String = Session("UserAuthentication")
        If sUsr = "" Then
            ShowSweetAlert2("info", "Anda Login Terdahulu!")
            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "Redirect", "setTimeout(function () {  window.location.href ='Default.aspx' }, 2000);", True)
        Else


            'txtModalNamaAccount.Attributes.Add("Readonly", "Readonly")

            'SET JAVASCRIPT POSTBACK DETECTION
            Page.ClientScript.RegisterClientScriptBlock([GetType](), "isPostBack", String.Format("var isPostback = {0};", IsPostBack.ToString().ToLower()), True)

            If Not IsPostBack Then
                txtidtrans.Text = HttpUtility.UrlDecode(Request.QueryString("idtrans"))
                If Not String.IsNullOrEmpty(txtidtrans.Text.Trim()) Then
                    QueryAccount(txtidtrans.Text.Trim(), True)
                    lnkSave.Text = "Update"
                    btnNew2.Visible = False
                    lnkDelete.Visible = True

                Else
                    NOOTOMATIS()
                    lnkDelete.Visible = False
                End If
                txtidtrans.Attributes.Add("readonly", "readonly")
                txtnama.Attributes.Add("readonly", "readonly")
                txtnopinjaman.Attributes.Add("readonly", "readonly")
                txtwipem.Attributes.Add("readonly", "readonly")
                ddlcabang.Attributes.Add("readonly", "readonly")
                ddlunit.Attributes.Add("readonly", "readonly")
                txthari.Attributes.Add("readonly", "readonly")
                txtjumlahdenda.Attributes.Add("readonly", "readonly")
                txtreal.Attributes.Add("readonly", "readonly")
                txtsaldoakhir.Attributes.Add("readonly", "readonly")
            End If
        End If
    End Sub
#End Region



#Region "Button"

    'Protected Sub lnkDelete_Click(sender As Object, e As System.EventArgs)

    'End Sub

    Protected Sub lnkSave_Click(sender As Object, e As System.EventArgs) Handles lnkSave.Click
        'CHECK REQUIRED FIELDS
        If String.IsNullOrEmpty(txtidnasabah.Text.Trim()) Then
            Dim successScript As String = "setTimeout(function(){ Swal.fire({ icon: 'warning', title: 'Id Kampus Tidak Boleh Kosong', showConfirmButton: false, showCancelButton: false, timer: 2000, timerProgressBar: true }); }, 100);"
            ScriptManager.RegisterStartupScript(Me, Me.[GetType](), "Popup", successScript, True)
            Exit Sub
        ElseIf String.IsNullOrEmpty(txtnama.Text.Trim()) Then
            Dim successScript As String = "setTimeout(function(){ Swal.fire({ icon: 'warning', title: 'Nama Tidak Boleh Kosong', showConfirmButton: false, showCancelButton: false, timer: 2000, timerProgressBar: true }); }, 100);"
            ScriptManager.RegisterStartupScript(Me, Me.[GetType](), "Popup", successScript, True)
            Exit Sub
        ElseIf String.IsNullOrEmpty(txtnopinjaman.Text.Trim()) Then
            Dim successScript As String = "setTimeout(function(){ Swal.fire({ icon: 'warning', title: 'Nomor Pinjaman Tidak Boleh Kosong', showConfirmButton: false, showCancelButton: false, timer: 2000, timerProgressBar: true }); }, 100);"
            ScriptManager.RegisterStartupScript(Me, Me.[GetType](), "Popup", successScript, True)
            Exit Sub
        ElseIf String.IsNullOrEmpty(txtjumlah.Text.Trim()) Then
            Dim successScript As String = "setTimeout(function(){ Swal.fire({ icon: 'warning', title: 'Jumlah Tidak Boleh Kosong', showConfirmButton: false, showCancelButton: false, timer: 2000, timerProgressBar: true }); }, 100);"
            ScriptManager.RegisterStartupScript(Me, Me.[GetType](), "Popup", successScript, True)
            Exit Sub
        ElseIf String.IsNullOrEmpty(ddlunit.Text.Trim()) Then
            Dim successScript As String = "setTimeout(function(){ Swal.fire({ icon: 'warning', title: 'Unit Tidak Boleh Kosong', showConfirmButton: false, showCancelButton: false, timer: 2000, timerProgressBar: true }); }, 100);"
            ScriptManager.RegisterStartupScript(Me, Me.[GetType](), "Popup", successScript, True)
            Exit Sub
        ElseIf String.IsNullOrEmpty(ddlcabang.Text.Trim()) Then
            Dim successScript As String = "setTimeout(function(){ Swal.fire({ icon: 'warning', title: 'Cabang Tidak Boleh Kosong', showConfirmButton: false, showCancelButton: false, timer: 2000, timerProgressBar: true }); }, 100);"
            ScriptManager.RegisterStartupScript(Me, Me.[GetType](), "Popup", successScript, True)
            Exit Sub
        End If

        Dim strUID As String = Trim(Session.Item("UserAuthentication").ToString)
        Dim ascabang As String = Left(ddlcabang.Text, 2)
        Dim asunit As String = Left(ddlunit.Text, 4)

        'Check untuk melakukan tambah data
        If Not CheckAdaData(txtidnasabah.Text.Trim(), "idtrans", "Tbldenda") Then
            connectDB()
            mySqlCon.Open()
            mySqlCmd = New System.Data.OleDb.OleDbCommand()

            Dim Transaction = mySqlCon.BeginTransaction(IsolationLevel.ReadCommitted)
            mySqlCmd.Connection = mySqlCon
            mySqlCmd.Transaction = Transaction

            Try
                mySqlCmd.CommandText = "INSERT INTO Tbldenda(IdTrans,NoPinjaman,IdNasabah,IdCabang,IdUnit,Wipem,Tanggal,Jumlah,LastUpdate,UseridUpdate) VALUES ('" _
                    & txtidtrans.Text & "', '" & txtnopinjaman.Text & "', '" & txtidnasabah.Text & "','" & ddlcabang.Text & "','" & ddlunit.Text & "', '" & txtwipem.Text & "', '" & clsEnt.CDateME5(dtpTgl.Value) & "'," & txtjumlah.Text & ",Getdate(),'" & strUID & "');"
                mySqlCmd.ExecuteNonQuery()

                Transaction.Commit()
                Session("Mode") = "Edit"
                Dim successScript As String = "setTimeout(function(){ Swal.fire({ icon: 'success', title: 'Saved', showConfirmButton: false, showCancelButton: false, timer: 2000, timerProgressBar: true }); }, 100);"
                ScriptManager.RegisterStartupScript(Me, Me.[GetType](), "Popup", successScript, True)

            Catch ex As Exception
                Transaction.Rollback()
                Dim successScript As String = "setTimeout(function(){ Swal.fire({ icon: 'error', title: 'Error saving data', showConfirmButton: false, showCancelButton: false, timer: 2000, timerProgressBar: true }); }, 100);"
                ScriptManager.RegisterStartupScript(Me, Me.[GetType](), "Popup", successScript, True)

            Finally
                mySqlCon.Close()
                mySqlCon.Dispose()
                mySqlCmd.Parameters.Clear()
                Response.Redirect("DendarealList.aspx")
            End Try
        Else
            connectDB()
            mySqlCon.Open()
            mySqlCmd = New System.Data.OleDb.OleDbCommand()

            Dim Transaction = mySqlCon.BeginTransaction(IsolationLevel.ReadCommitted)
            mySqlCmd.Connection = mySqlCon
            mySqlCmd.Transaction = Transaction

            Try
                mySqlCmd.CommandText = "UPDATE TblDenda SET Tanggal='" & clsEnt.CDateME5(dtpTgl.Value) & "',Jumlah = '" & txtjumlah.Text & "',idnasabah = '" & txtidnasabah.Text & "',nopinjaman = '" & txtnopinjaman.Text & "',idcabang='" & ddlcabang.Text & "',idunit='" & ddlunit.Text & "',wipem='" & txtwipem.Text & "',LastUpdate=getdate(),UseridUpdate='" & strUID & "' WHERE idtrans= '" & txtidtrans.Text.Trim() & "'; "
                mySqlCmd.ExecuteNonQuery()

                Transaction.Commit()
                Session("Mode") = "Edit"
                Dim successScript As String = "setTimeout(function(){ Swal.fire({ icon: 'success', title: 'Saved', showConfirmButton: false, showCancelButton: false, timer: 2000, timerProgressBar: true }); }, 100);"
                ScriptManager.RegisterStartupScript(Me, Me.[GetType](), "Popup", successScript, True)
                QueryAccount(txtidnasabah.Text.Trim(), True)

            Catch ex As Exception
                Transaction.Rollback()
                Dim successScript As String = "setTimeout(function(){ Swal.fire({ icon: 'error', title: 'Error updating data', showConfirmButton: false, showCancelButton: false, timer: 2000, timerProgressBar: true }); }, 100);"
                ScriptManager.RegisterStartupScript(Me, Me.[GetType](), "Popup", successScript, True)
            Finally
                mySqlCon.Close()
                mySqlCon.Dispose()
                mySqlCmd.Parameters.Clear()
                ClearForm()
                Response.Redirect("DendarealList.aspx")
            End Try
        End If
    End Sub



    Protected Sub btnNew_Click(sender As Object, e As System.EventArgs) Handles btnNew.Click
        Response.Redirect("DendarealList.aspx")
        Session("Mode") = "New"
    End Sub


#End Region

    Private Sub ClearForm()
        txtidnasabah.Text = ""
        txtnama.Text = ""
        txtidtrans.Text = ""
        ddlcabang.Text = ""
        dtpTgl.Value = ""
        ddlunit.Text = ""
        txtwipem.Text = ""
        txtnopinjaman.Text = ""
        txtjumlah.Text = 0
    End Sub



    Private Function CheckAdaData(a_Str As String, str_Field As String, str_Tbl As String) As Boolean
        Return Convert.ToBoolean(clsEnt.ReturnFieldOne("SELECT COUNT(*) AS CNT FROM " & str_Tbl & " WHERE " & str_Field & " = '" & a_Str & "'; ", "", "", "CNT"))
    End Function


    Private Sub QueryAccount(ByVal a_Str As String, showmsg As Boolean)

        connectDB()

        Try
            mySqlCon.Open()
        Catch ex As Exception
            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "PopUp", "setTimeout(function () {  callAlert2('error', 'Error Connecting to Database', '2000'); }, 10);", True)
            Exit Sub
        Finally
            Dim command As System.Data.OleDb.OleDbCommand = New System.Data.OleDb.OleDbCommand
            command = New System.Data.OleDb.OleDbCommand("SELECT a.IdTrans as IdTrans, a.IdCabang as IdCabang, a.IdUnit as IdUnit, a.Wipem as Wipem, a.NoPinjaman as NoPinjaman, a.idNasabah as idNasabah, FORMAT(a.Jumlah,'#,###,##0') as Jumlah, CONVERT(VARCHAR(10),a.Tanggal,103) as Tanggal, b.nama as nama from TblDenda a inner join tblnasabah b on b.idnasabah=a.idnasabah WHERE idtrans = '" & a_Str & "'; ", mySqlCon)
            mySqlReader = command.ExecuteReader
            If mySqlReader.Read Then
                txtidnasabah.Text = mySqlReader("idNasabah").ToString()
                txtnama.Text = mySqlReader("Nama").ToString()
                ddlcabang.Text = mySqlReader("IDCABANG").ToString()
                ddlunit.Text = mySqlReader("IDUNIT").ToString()
                txtidtrans.Text = mySqlReader("IdTrans").ToString()
                dtpTgl.Value = mySqlReader("tanggal").ToString()
                txtwipem.Text = mySqlReader("Wipem").ToString()
                txtnopinjaman.Text = mySqlReader("NoPinjaman").ToString()
                txtjumlah.Text = mySqlReader("JUMLAH").ToString()
                dtpTgl.Value = mySqlReader("Tanggal").ToString()
                lnkDelete.Visible = True
                mySqlReader.Close()
                mySqlCon.Close()
                mySqlCon.Dispose()
                command.Parameters.Clear()

            Else
                txtidnasabah.Text = ""
                txtnama.Text = ""
                txtidtrans.Text = ""
                ddlcabang.Text = ""
                dtpTgl.Value = ""
                ddlunit.Text = ""
                txtwipem.Text = ""
                txtnopinjaman.Text = ""
                txtjumlah.Text = 0
                mySqlReader.Close()
                mySqlCon.Close()
                mySqlCon.Dispose()

                'txtid.Attributes.Remove("readonly")
            End If

        End Try
    End Sub



#Region "POP UP"
    Private Sub ShowSweetAlert2(ByVal icon As String, ByVal titleString As String)
        Dim sweetAlertString As String = "setTimeout(function(){ Swal.fire({ icon: '" & icon & "', title: '" & titleString & "', showConfirmButton: false, showCancelButton: false, timer: 2000, timerProgressBar: true }); }, 100);"
        ScriptManager.RegisterStartupScript(Me, Me.[GetType](), "Popup", sweetAlertString, True)
    End Sub
#End Region

    Public Sub NOOTOMATIS()
        'Dim sTrue As DataTable = GetDataTbl("select Max(idnasabah) + 1 as total from TblDenda")
        'Dim nourut As String = CInt(sTrue.Rows(0).Item("total"))
        Dim NO As String = clsEnt.ReturnOneFieldConn("select top(1)idtrans + 1 as nnn from TblDenda order by idtrans desc ", "", "", "nnn", "connuq")
        txtidtrans.Text = IIf(NO = "", "00001", "0000" + NO)

    End Sub

    Protected Sub btnNew2_Click(sender As Object, e As System.EventArgs) Handles btnNew2.Click
        ClearForm()
    End Sub

    Protected Sub lnkDelete_Click(sender As Object, e As System.EventArgs) Handles lnkDelete.Click
        ''CHECK REQUIRED FIELDS

        connectDB()
        mySqlCon.Open()
        mySqlCmd = New System.Data.OleDb.OleDbCommand()

        Dim Transaction = mySqlCon.BeginTransaction(IsolationLevel.ReadCommitted)
        mySqlCmd.Connection = mySqlCon
        mySqlCmd.Transaction = Transaction

        Try
            mySqlCmd.CommandText = "DELETE FROM TblDenda WHERE idtrans='" & txtidtrans.Text.Trim() & "'; "
            mySqlCmd.ExecuteNonQuery()

            Transaction.Commit()
            Session("Mode") = "Edit"
            Dim successScript As String = "setTimeout(function(){ Swal.fire({ icon: 'success', title: 'Deleted', showConfirmButton: false, showCancelButton: false, timer: 2000, timerProgressBar: true }); }, 100);"
            ScriptManager.RegisterStartupScript(Me, Me.[GetType](), "Popup", successScript, True)
            btnNew_Click(Nothing, Nothing)

        Catch ex As Exception
            Dim successScript As String = "setTimeout(function(){ Swal.fire({ icon: 'error', title: 'Error deleting data', showConfirmButton: false, showCancelButton: false, timer: 2000, timerProgressBar: true }); }, 100);"
            ScriptManager.RegisterStartupScript(Me, Me.[GetType](), "Popup", successScript, True)

        Finally
            mySqlCon.Close()
            mySqlCon.Dispose()
            mySqlCmd.Parameters.Clear()
            Response.Redirect("DendarealList.aspx")
        End Try
    End Sub

    Protected Sub Lngenerate_Click(sender As Object, e As EventArgs) Handles Lngenerate.Click
        Dim jumlahdenda, realdenda, saldoakhidenda As Integer
        Dim sTrue2 As DataTable = GetDataTbl("select isnull(Sum(denda),0) as jumlah  from Tblangsuran where nopinjaman='" & txtnopinjaman.Text & "'")
        Dim sTrue3 As DataTable = GetDataTbl("select isnull(Sum(hariterlambat),0) as jumlah  from Tblangsuran where nopinjaman='" & txtnopinjaman.Text & "'")
        Dim sTruerealdenda As DataTable = GetDataTbl("select isnull(Sum(jumlah),0) as jumlah  from Tbldenda where nopinjaman='" & txtnopinjaman.Text & "'")

        txthari.Text = sTrue2.Rows(0).Item("jumlah")
        txtjumlahdenda.Text = sTrue3.Rows(0).Item("jumlah")
        txtreal.Text = sTruerealdenda.Rows(0).Item("jumlah")
        jumlahdenda = txtjumlahdenda.Text
        realdenda = txtreal.Text
        saldoakhidenda = jumlahdenda - realdenda
        txtsaldoakhir.Text = saldoakhidenda
    End Sub
End Class
