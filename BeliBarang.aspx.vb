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

Partial Class BeliBarang
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
                Dim constr As String = ConfigurationManager.ConnectionStrings("connuq").ConnectionString

                If Not String.IsNullOrEmpty(txtidtrans.Text.Trim()) Then
                    QueryAccount(txtidtrans.Text.Trim(), True)
                    lnkSave.Text = "Update"
                    txtidtrans.Attributes.Add("readonly", "readonly")
                    btnNew2.Visible = False
                    lnkDelete.Visible = True

                Else
                    NOOTOMATIS()
                    lnkDelete.Visible = False
                End If
                Using con As New SqlConnection(constr)
                    Using cmd As New SqlCommand("select JenisPembelian as nama from TblJenisPembelian order by IdPrimary")
                        cmd.CommandType = CommandType.Text
                        cmd.Connection = con
                        con.Open()
                        ddjenis.DataSource = cmd.ExecuteReader()
                        ddjenis.DataTextField = "Nama"
                        ddjenis.DataValueField = "Nama"
                        ddjenis.DataBind()
                        con.Close()
                    End Using
                End Using
                ddjenis.Items.Insert(0, New System.Web.UI.WebControls.ListItem("Pilih Jenis Pembelian...", ""))
            End If

            txtidtrans.Attributes.Add("readonly", "readonly")
            txtidsupplier.Attributes.Add("readonly", "readonly")
            txtnama.Attributes.Add("readonly", "readonly")
            txtmerk.Attributes.Add("readonly", "readonly")
            txtjenis.Attributes.Add("readonly", "readonly")
            txtnomesin.Attributes.Add("readonly", "readonly")
            txtnorangka.Attributes.Add("readonly", "readonly")
            txtrekondisi.Attributes.Add("readonly", "readonly")
        End If
    End Sub
#End Region



#Region "Button"

    'Protected Sub lnkDelete_Click(sender As Object, e As System.EventArgs)

    'End Sub

    Protected Sub lnkSave_Click(sender As Object, e As System.EventArgs) Handles lnkSave.Click
        'CHECK REQUIRED FIELDS
        If String.IsNullOrEmpty(txtidtrans.Text.Trim()) Then
            Dim successScript As String = "setTimeout(function(){ Swal.fire({ icon: 'warning', title: 'Id Kampus Tidak Boleh Kosong', showConfirmButton: false, showCancelButton: false, timer: 2000, timerProgressBar: true }); }, 100);"
            ScriptManager.RegisterStartupScript(Me, Me.[GetType](), "Popup", successScript, True)
            Exit Sub
        ElseIf String.IsNullOrEmpty(txtplat.Text.Trim()) Then
            Dim successScript As String = "setTimeout(function(){ Swal.fire({ icon: 'warning', title: 'Plat Tidak Boleh Kosong', showConfirmButton: false, showCancelButton: false, timer: 2000, timerProgressBar: true }); }, 100);"
            ScriptManager.RegisterStartupScript(Me, Me.[GetType](), "Popup", successScript, True)
            Exit Sub
        ElseIf String.IsNullOrEmpty(txtmerk.Text.Trim()) Then
            Dim successScript As String = "setTimeout(function(){ Swal.fire({ icon: 'warning', title: 'Merk Tidak Boleh Kosong', showConfirmButton: false, showCancelButton: false, timer: 2000, timerProgressBar: true }); }, 100);"
            ScriptManager.RegisterStartupScript(Me, Me.[GetType](), "Popup", successScript, True)
            Exit Sub
        ElseIf String.IsNullOrEmpty(txtjenis.Text.Trim()) Then
            Dim successScript As String = "setTimeout(function(){ Swal.fire({ icon: 'warning', title: 'Jenis Tidak Boleh Kosong', showConfirmButton: false, showCancelButton: false, timer: 2000, timerProgressBar: true }); }, 100);"
            ScriptManager.RegisterStartupScript(Me, Me.[GetType](), "Popup", successScript, True)
            Exit Sub
        ElseIf String.IsNullOrEmpty(ddjenis.Text.Trim()) Then
            Dim successScript As String = "setTimeout(function(){ Swal.fire({ icon: 'warning', title: 'Jenis Pembelian Tidak Boleh Kosong', showConfirmButton: false, showCancelButton: false, timer: 2000, timerProgressBar: true }); }, 100);"
            ScriptManager.RegisterStartupScript(Me, Me.[GetType](), "Popup", successScript, True)
            Exit Sub
        ElseIf String.IsNullOrEmpty(txtjenis.Text.Trim()) Then
            Dim successScript As String = "setTimeout(function(){ Swal.fire({ icon: 'warning', title: 'Jenis Tidak Boleh Kosong', showConfirmButton: false, showCancelButton: false, timer: 2000, timerProgressBar: true }); }, 100);"
            ScriptManager.RegisterStartupScript(Me, Me.[GetType](), "Popup", successScript, True)
            Exit Sub
        ElseIf String.IsNullOrEmpty(txtnomesin.Text.Trim()) Then
            Dim successScript As String = "setTimeout(function(){ Swal.fire({ icon: 'warning', title: 'Nomor Mesin Silinder Tidak Boleh Kosong', showConfirmButton: false, showCancelButton: false, timer: 2000, timerProgressBar: true }); }, 100);"
            ScriptManager.RegisterStartupScript(Me, Me.[GetType](), "Popup", successScript, True)
            Exit Sub
        ElseIf String.IsNullOrEmpty(txtnorangka.Text.Trim()) Then
            Dim successScript As String = "setTimeout(function(){ Swal.fire({ icon: 'warning', title: 'Rangka Tidak Boleh Kosong', showConfirmButton: false, showCancelButton: false, timer: 2000, timerProgressBar: true }); }, 100);"
            ScriptManager.RegisterStartupScript(Me, Me.[GetType](), "Popup", successScript, True)
            Exit Sub
        ElseIf String.IsNullOrEmpty(ddlcabang.Text.Trim()) Then
            Dim successScript As String = "setTimeout(function(){ Swal.fire({ icon: 'warning', title: 'Cabang Silinder Tidak Boleh Kosong', showConfirmButton: false, showCancelButton: false, timer: 2000, timerProgressBar: true }); }, 100);"
            ScriptManager.RegisterStartupScript(Me, Me.[GetType](), "Popup", successScript, True)
            Exit Sub
        ElseIf String.IsNullOrEmpty(ddlunit.Text.Trim()) Then
            Dim successScript As String = "setTimeout(function(){ Swal.fire({ icon: 'warning', title: 'Unit Tidak Boleh Kosong', showConfirmButton: false, showCancelButton: false, timer: 2000, timerProgressBar: true }); }, 100);"
            ScriptManager.RegisterStartupScript(Me, Me.[GetType](), "Popup", successScript, True)
            Exit Sub
        ElseIf String.IsNullOrEmpty(txtbeli.Text.Trim()) Then
            Dim successScript As String = "setTimeout(function(){ Swal.fire({ icon: 'warning', title: 'HARGA bELI Tidak Boleh Kosong', showConfirmButton: false, showCancelButton: false, timer: 2000, timerProgressBar: true }); }, 100);"
            ScriptManager.RegisterStartupScript(Me, Me.[GetType](), "Popup", successScript, True)
            Exit Sub
        End If

        Dim biaya1, biaya2, hasil As Decimal
        biaya1 = txtrekondisi.Text
        biaya2 = txtbeli.Text

        hasil = biaya1 + biaya2

        Dim strUID As String = Trim(Session.Item("UserAuthentication").ToString)
        Dim ascabang As String = Left(ddlcabang.Text, 2)
        Dim asunit As String = Left(ddlunit.Text, 4)

        'Check untuk melakukan tambah data
        If Not CheckAdaData(txtidtrans.Text.Trim(), "idtrans", "TblBeli") Then
            connectDB()
            mySqlCon.Open()
            mySqlCmd = New System.Data.OleDb.OleDbCommand()

            Dim Transaction = mySqlCon.BeginTransaction(IsolationLevel.ReadCommitted)
            mySqlCmd.Connection = mySqlCon
            mySqlCmd.Transaction = Transaction

            Try
                mySqlCmd.CommandText = "INSERT INTO TblBeli(idTrans,IdCabang,Idunit,NoPlat,Merk,Jenis,HargaBeli,BiayaRekondisi,Total,NoMesin,NoRangka,Tanggal,Keterangan,JenisPembelian,LastUpdate,UseridUpdate) VALUES ('" _
                    & txtidtrans.Text & "', '" & ddlcabang.Text & "', '" & ddlunit.Text & "','" & txtplat.Text & "','" & txtmerk.Text & "', '" & txtjenis.Text & "', " & biaya2 & ", " & biaya1 & ", " & hasil & ", '" & txtnomesin.Text & "', '" & txtnorangka.Text & "','" & clsEnt.CDateME5(dtgl.Value) & "', '" & txtarea.Text & "','" & ddjenis.Text & "',Getdate(),'" & strUID & "'); "


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
                Response.Redirect("BeliBarangList.aspx")
            End Try
        Else
            connectDB()
            mySqlCon.Open()
            mySqlCmd = New System.Data.OleDb.OleDbCommand()

            Dim Transaction = mySqlCon.BeginTransaction(IsolationLevel.ReadCommitted)
            mySqlCmd.Connection = mySqlCon
            mySqlCmd.Transaction = Transaction

            Try
                mySqlCmd.CommandText = "UPDATE TblBeli SET JenisPembelian='" & ddjenis.Text & "', idSupplier='" & txtidsupplier.Text & "', IdCabang ='" & ddlcabang.Text & "',Idunit='" & ddlunit.Text & "' ,NoPlat='" & txtplat.Text & "',Merk='" & txtmerk.Text & "',Jenis='" & txtjenis.Text & "',NoMesin='" & txtnomesin.Text & "',NoRangka='" & txtnorangka.Text & "', HargaBeli=" & txtbeli.Text & ", BiayaRekondisi=" & txtrekondisi.Text & ",Total=" & hasil & ",Keterangan='" & txtarea.Text & "', Tanggal ='" & clsEnt.CDateME5(dtgl.Value) & "',LastUpdate=Getdate(),UseridUpdate='" & strUID & "' WHERE idtrans= '" & txtidtrans.Text.Trim() & "'; "
                mySqlCmd.ExecuteNonQuery()

                Transaction.Commit()
                Session("Mode") = "Edit"
                Dim successScript As String = "setTimeout(function(){ Swal.fire({ icon: 'success', title: 'Saved', showConfirmButton: false, showCancelButton: false, timer: 2000, timerProgressBar: true }); }, 100);"
                ScriptManager.RegisterStartupScript(Me, Me.[GetType](), "Popup", successScript, True)
                QueryAccount(txtidtrans.Text.Trim(), True)

            Catch ex As Exception
                Transaction.Rollback()
                Dim successScript As String = "setTimeout(function(){ Swal.fire({ icon: 'error', title: 'Error updating data', showConfirmButton: false, showCancelButton: false, timer: 2000, timerProgressBar: true }); }, 100);"
                ScriptManager.RegisterStartupScript(Me, Me.[GetType](), "Popup", successScript, True)
            Finally
                mySqlCon.Close()
                mySqlCon.Dispose()
                mySqlCmd.Parameters.Clear()
                ClearForm()
                Response.Redirect("BeliBarangList.aspx")
            End Try
        End If
    End Sub



    Protected Sub btnNew_Click(sender As Object, e As System.EventArgs) Handles btnNew.Click
        Response.Redirect("BeliBarangList.aspx")
        Session("Mode") = "New"
    End Sub


#End Region

    Private Sub ClearForm()
        txtidtrans.Text = ""
        txtplat.Text = ""
        txtmerk.Text = ""
        txtjenis.Text = ""
        ddjenis.Text = ""
        txtnomesin.Text = ""
        txtnorangka.Text = ""
        txtrekondisi.Text = ""
        txtbeli.Text = ""
        dtgl.Value = ""
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
            command = New System.Data.OleDb.OleDbCommand("SELECT a.idsupplier as idsupplier,b.nama as nama, a.idTrans as idTrans, a.IdCabang as IdCabang, a.Idunit as Idunit, a.NoPlat as NoPlat, a.Merk as Merk, a.Jenis as Jenis,a.HargaBeli, a.BiayaRekondisi, a.Total, a.NoMesin as NoMesin, a.NoRangka as NoRangka, CONVERT(VARCHAR(10),a.Tanggal,103) as Tanggal,a.Keterangan AS Keterangan, a.JenisPembelian AS JenisPembelian FROM TblBeli a inner join TblSuplier b on b.idsupplier=a.idsupplier WHERE idtrans= '" & a_Str & "'; ", mySqlCon)
            mySqlReader = command.ExecuteReader
            If mySqlReader.Read Then
                txtidtrans.Text = mySqlReader("idtrans").ToString()
                txtidsupplier.Text = mySqlReader("idsupplier").ToString()
                txtnama.Text = mySqlReader("nama").ToString()
                txtplat.Text = mySqlReader("NoPlat").ToString()
                txtmerk.Text = mySqlReader("Merk").ToString()
                txtjenis.Text = mySqlReader("Jenis").ToString()
                txtrekondisi.Text = mySqlReader("BiayaRekondisi").ToString()
                txtbeli.Text = mySqlReader("HargaBeli").ToString()
                txtnomesin.Text = mySqlReader("NoMesin").ToString()
                txtnorangka.Text = mySqlReader("NoRangka").ToString()
                dtgl.Value = mySqlReader("tanggal").ToString()
                ddlcabang.Text = mySqlReader("IdCabang").ToString()
                ddlunit.Text = mySqlReader("Idunit").ToString()
                ddjenis.Text = mySqlReader("JenisPembelian").ToString()
                txtarea.Text = mySqlReader("Keterangan").ToString()
                lnkDelete.Visible = True
                mySqlReader.Close()
                mySqlCon.Close()
                mySqlCon.Dispose()
                command.Parameters.Clear()

            Else
                txtidtrans.Text = ""
                txtplat.Text = ""
                txtmerk.Text = ""
                txtjenis.Text = ""
                ddjenis.Text = ""
                txtnomesin.Text = ""
                txtnorangka.Text = ""
                txtrekondisi.Text = ""
                txtbeli.Text = ""
                dtgl.Value = ""
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
        'Dim sTrue As DataTable = GetDataTbl("select Max(idtrans) + 1 as total from TblBeli")
        'Dim nourut As String = CInt(sTrue.Rows(0).Item("total"))
        Dim NO As String = clsEnt.ReturnOneFieldConn("select top(1)idtrans + 1 as nnn from TblBeli order by idtrans desc ", "", "", "nnn", "connuq")
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
            mySqlCmd.CommandText = "DELETE FROM TblBeli WHERE idtrans = '" & txtidtrans.Text.Trim() & "'; "
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
            Response.Redirect("BeliBarangList.aspx")
        End Try
    End Sub



End Class
