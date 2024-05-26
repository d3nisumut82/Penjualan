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

Partial Class Suplier
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
                txtidsupplier.Text = HttpUtility.UrlDecode(Request.QueryString("IdSupplier"))
                If Not String.IsNullOrEmpty(txtidsupplier.Text.Trim()) Then
                    QueryAccount(txtidsupplier.Text.Trim(), True)
                    lnkSave.Text = "Update"
                    txtidsupplier.Attributes.Add("readonly", "readonly")
                    btnNew2.Visible = False
                    lnkDelete.Visible = True

                Else
                    NOOTOMATIS()
                    lnkDelete.Visible = False
                End If
            End If
            txtidsupplier.Attributes.Add("readonly", "readonly")
        End If
    End Sub
#End Region



#Region "Button"

    'Protected Sub lnkDelete_Click(sender As Object, e As System.EventArgs)

    'End Sub

    Protected Sub lnkSave_Click(sender As Object, e As System.EventArgs) Handles lnkSave.Click
        'CHECK REQUIRED FIELDS
        If String.IsNullOrEmpty(txtidsupplier.Text.Trim()) Then
            Dim successScript As String = "setTimeout(function(){ Swal.fire({ icon: 'warning', title: 'Id Kampus Tidak Boleh Kosong', showConfirmButton: false, showCancelButton: false, timer: 2000, timerProgressBar: true }); }, 100);"
            ScriptManager.RegisterStartupScript(Me, Me.[GetType](), "Popup", successScript, True)
            Exit Sub
        ElseIf String.IsNullOrEmpty(txtnama.Text.Trim()) Then
            Dim successScript As String = "setTimeout(function(){ Swal.fire({ icon: 'warning', title: 'Nama Supplier Tidak Boleh Kosong', showConfirmButton: false, showCancelButton: false, timer: 2000, timerProgressBar: true }); }, 100);"
            ScriptManager.RegisterStartupScript(Me, Me.[GetType](), "Popup", successScript, True)
            Exit Sub
        ElseIf String.IsNullOrEmpty(txtalamat.Text.Trim()) Then
            Dim successScript As String = "setTimeout(function(){ Swal.fire({ icon: 'warning', title: 'Alamat Tidak Boleh Kosong', showConfirmButton: false, showCancelButton: false, timer: 2000, timerProgressBar: true }); }, 100);"
            ScriptManager.RegisterStartupScript(Me, Me.[GetType](), "Popup", successScript, True)
            Exit Sub
        ElseIf String.IsNullOrEmpty(txttempat.Text.Trim()) Then
            Dim successScript As String = "setTimeout(function(){ Swal.fire({ icon: 'warning', title: 'Tempat Lahir Tidak Boleh Kosong', showConfirmButton: false, showCancelButton: false, timer: 2000, timerProgressBar: true }); }, 100);"
            ScriptManager.RegisterStartupScript(Me, Me.[GetType](), "Popup", successScript, True)
            Exit Sub
        ElseIf String.IsNullOrEmpty(txthp.Text.Trim()) Then
            Dim successScript As String = "setTimeout(function(){ Swal.fire({ icon: 'warning', title: 'HP Tidak Boleh Kosong', showConfirmButton: false, showCancelButton: false, timer: 2000, timerProgressBar: true }); }, 100);"
            ScriptManager.RegisterStartupScript(Me, Me.[GetType](), "Popup", successScript, True)
            Exit Sub
        ElseIf String.IsNullOrEmpty(txtktp.Text.Trim()) Then
            Dim successScript As String = "setTimeout(function(){ Swal.fire({ icon: 'warning', title: 'KTP Tidak Boleh Kosong', showConfirmButton: false, showCancelButton: false, timer: 2000, timerProgressBar: true }); }, 100);"
            ScriptManager.RegisterStartupScript(Me, Me.[GetType](), "Popup", successScript, True)
            Exit Sub
        ElseIf String.IsNullOrEmpty(txtkk.Text.Trim()) Then
            Dim successScript As String = "setTimeout(function(){ Swal.fire({ icon: 'warning', title: 'KK Tidak Boleh Kosong', showConfirmButton: false, showCancelButton: false, timer: 2000, timerProgressBar: true }); }, 100);"
            ScriptManager.RegisterStartupScript(Me, Me.[GetType](), "Popup", successScript, True)
            Exit Sub
        ElseIf String.IsNullOrEmpty(ddlcabang.Text.Trim()) Then
            Dim successScript As String = "setTimeout(function(){ Swal.fire({ icon: 'warning', title: 'Cabang Unit Tidak Boleh Kosong', showConfirmButton: false, showCancelButton: false, timer: 2000, timerProgressBar: true }); }, 100);"
            ScriptManager.RegisterStartupScript(Me, Me.[GetType](), "Popup", successScript, True)
            Exit Sub
        ElseIf String.IsNullOrEmpty(ddlunit.Text.Trim()) Then
            Dim successScript As String = "setTimeout(function(){ Swal.fire({ icon: 'warning', title: 'Unit Tidak Boleh Kosong', showConfirmButton: false, showCancelButton: false, timer: 2000, timerProgressBar: true }); }, 100);"
            ScriptManager.RegisterStartupScript(Me, Me.[GetType](), "Popup", successScript, True)
            Exit Sub
        End If

        Dim strUID As String = Trim(Session.Item("UserAuthentication").ToString)
        Dim ascabang As String = Left(ddlcabang.Text, 2)
        Dim asunit As String = Left(ddlunit.Text, 4)

        'Check untuk melakukan tambah data
        If Not CheckAdaData(txtidsupplier.Text.Trim(), "IdSupplier", "TblSuplier") Then
            connectDB()
            mySqlCon.Open()
            mySqlCmd = New System.Data.OleDb.OleDbCommand()

            Dim Transaction = mySqlCon.BeginTransaction(IsolationLevel.ReadCommitted)
            mySqlCmd.Connection = mySqlCon
            mySqlCmd.Transaction = Transaction

            Try
                mySqlCmd.CommandText = "INSERT INTO TblSuplier (IdSupplier,IdCabang,IdUnit, Nama,Alamat,TempatLahir,TGLLAHIR,IdKTP,NoKK,TglDaftar, Domisili,HP,LastUpdate,UseridUpdate) VALUES ('" _
                    & txtidsupplier.Text & "', '" & ddlcabang.Text & "', '" & ddlunit.Text & "','" & txtnama.Text & "','" & txtalamat.Text & "', '" & txttempat.Text & "', '" & clsEnt.CDateME5(dtpTgllahir.Value) & "', '" & txtktp.Text & "', '" & txtkk.Text & "', '" & clsEnt.CDateME5(txttglgabung.Value) & "', '" & txtdomisili.Text & "','" & txthp.Text & "',Getdate(),'" & strUID & "'); "


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
                Response.Redirect("SuplierList.aspx")
            End Try
        Else
            connectDB()
            mySqlCon.Open()
            mySqlCmd = New System.Data.OleDb.OleDbCommand()

            Dim Transaction = mySqlCon.BeginTransaction(IsolationLevel.ReadCommitted)
            mySqlCmd.Connection = mySqlCon
            mySqlCmd.Transaction = Transaction

            Try
                mySqlCmd.CommandText = "UPDATE TblSuplier SET Nama = '" & txtnama.Text & "',Alamat = '" & txtalamat.Text & "',TEMPATLAHIR = '" & txttempat.Text & "',TglLahir ='" & clsEnt.CDateME5(dtpTgllahir.Value) & "',HP = '" & txthp.Text & "',Domisili = '" & txtdomisili.Text & "',idKTP = '" & txtktp.Text & "',NoKK='" & txtkk.Text & "', TglDaftar='" & clsEnt.CDateME5(dtpTgllahir.Value) & "',IDCABANG='" & ddlcabang.Text & "', IDUNIT='" & ddlunit.Text & "',LastUpdate=getdate(),UseridUpdate='" & strUID & "' WHERE IdSupplier= '" & txtidsupplier.Text.Trim() & "'; "
                mySqlCmd.ExecuteNonQuery()

                Transaction.Commit()
                Session("Mode") = "Edit"
                Dim successScript As String = "setTimeout(function(){ Swal.fire({ icon: 'success', title: 'Saved', showConfirmButton: false, showCancelButton: false, timer: 2000, timerProgressBar: true }); }, 100);"
                ScriptManager.RegisterStartupScript(Me, Me.[GetType](), "Popup", successScript, True)
                QueryAccount(txtidsupplier.Text.Trim(), True)

            Catch ex As Exception
                Transaction.Rollback()
                Dim successScript As String = "setTimeout(function(){ Swal.fire({ icon: 'error', title: 'Error updating data', showConfirmButton: false, showCancelButton: false, timer: 2000, timerProgressBar: true }); }, 100);"
                ScriptManager.RegisterStartupScript(Me, Me.[GetType](), "Popup", successScript, True)
            Finally
                mySqlCon.Close()
                mySqlCon.Dispose()
                mySqlCmd.Parameters.Clear()
                ClearForm()
                Response.Redirect("SuplierList.aspx")
            End Try
        End If
    End Sub



    Protected Sub btnNew_Click(sender As Object, e As System.EventArgs) Handles btnNew.Click
        Response.Redirect("SuplierList.aspx")
        Session("Mode") = "New"
    End Sub


#End Region

    Private Sub ClearForm()
        txtidsupplier.Text = ""
        txtnama.Text = ""
        txtalamat.Text = ""
        txttempat.Text = ""
        dtpTgllahir.Value = ""
        txttglgabung.Value = ""
        txtktp.Text = ""
        txtkk.Text = ""
        txtdomisili.Text = ""
        txthp.Text = ""
        ddlcabang.Text = ""
        ddlunit.Text = ""

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
            command = New System.Data.OleDb.OleDbCommand("SELECT IdSupplier as IdSupplier, IdCabang as IdCabang, IdUnit as IdUnit, Nama as Nama, Alamat as Alamat, TempatLahir as TempatLahir, CONVERT(VARCHAR(10),TGLLAHIR,103) AS TGLLAHIR, IdKTP as IdKTP, NoKK as NoKK, CONVERT(VARCHAR(10),TglDaftar,103)  AS TglDaftar, Domisili as Domisili, HP as HP FROM TblSuplier WHERE IdSupplier = '" & a_Str & "'; ", mySqlCon)
            mySqlReader = command.ExecuteReader
            If mySqlReader.Read Then
                txtidsupplier.Text = mySqlReader("IdSupplier").ToString()
                txtnama.Text = mySqlReader("Nama").ToString()
                txtalamat.Text = mySqlReader("Alamat").ToString()
                txttempat.Text = mySqlReader("TEMPATLAHIR").ToString()
                dtpTgllahir.Value = mySqlReader("TGLLAHIR").ToString()
                txttglgabung.Value = mySqlReader("TGLDAFTAR").ToString()
                txtktp.Text = mySqlReader("IdKTP").ToString()
                txtkk.Text = mySqlReader("NoKK").ToString()
                txtdomisili.Text = mySqlReader("Domisili").ToString()
                txthp.Text = mySqlReader("HP").ToString()
                ddlcabang.Text = mySqlReader("IDCABANG").ToString()
                ddlunit.Text = mySqlReader("IDUNIT").ToString()
                'txtid.Attributes.Add("readonly", "readonly")
                lnkDelete.Visible = True
                mySqlReader.Close()
                mySqlCon.Close()
                mySqlCon.Dispose()
                command.Parameters.Clear()

            Else
                txtidsupplier.Text = ""
                txtnama.Text = ""
                txtalamat.Text = ""
                txttempat.Text = ""
                txtktp.Text = ""
                txtkk.Text = ""
                txthp.Text = ""
                ddlcabang.Text = ""
                ddlunit.Text = ""
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
        'Dim sTrue As DataTable = GetDataTbl("select Max(IdSupplier) + 1 as total from TblSuplier")
        'Dim nourut As String = CInt(sTrue.Rows(0).Item("total"))
        Dim NO As String = clsEnt.ReturnOneFieldConn("select top(1)IdSupplier + 1 as nnn from TblSuplier order by IdSupplier desc ", "", "", "nnn", "connuq")
        txtidsupplier.Text = IIf(NO = "", "00001", "0000" + NO)

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
            mySqlCmd.CommandText = "DELETE FROM TblSuplier WHERE IdSupplier = '" & txtidsupplier.Text.Trim() & "'; "
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
            Response.Redirect("SuplierList.aspx")
        End Try
    End Sub



End Class
