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
Imports System.Math
Imports System.Text
Imports System.Drawing

Imports System
Imports System.Collections.Generic
Imports System.Linq

Partial Class JualBarang
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
                    Using cmd As New SqlCommand("select Lama from TblLamaAngsuran order by IdPrimary")
                        cmd.CommandType = CommandType.Text
                        cmd.Connection = con
                        con.Open()
                        ddllama.DataSource = cmd.ExecuteReader()
                        ddllama.DataTextField = "Lama"
                        ddllama.DataValueField = "Lama"
                        ddllama.DataBind()
                        con.Close()
                    End Using
                End Using
                ddllama.Items.Insert(0, New System.Web.UI.WebControls.ListItem("Pilih Lama Angsuran...", ""))

                Using con As New SqlConnection(constr)
                    Using cmd As New SqlCommand("select Nama from TblJenisKredit order by IdPrimary")
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
                ddjenis.Items.Insert(0, New System.Web.UI.WebControls.ListItem("Pilih Jenis Penjualan...", ""))
            End If

        End If

        txtidtrans.Attributes.Add("readonly", "readonly")
        txtsaldo.Attributes.Add("readonly", "readonly")
        txtangsuran.Attributes.Add("readonly", "readonly")
        txtbunga.Attributes.Add("readonly", "readonly")
        txtnama.Attributes.Add("readonly", "readonly")
        txtmerk.Attributes.Add("readonly", "readonly")
        txtnomesin.Attributes.Add("readonly", "readonly")
        txtnorangka.Attributes.Add("readonly", "readonly")
        txtpokok.Attributes.Add("readonly", "readonly")
        txttotal2.Attributes.Add("readonly", "readonly")
        txtsaldo2.Attributes.Add("readonly", "readonly")
        txtangsuran2.Attributes.Add("readonly", "readonly")
        txtbunga2.Attributes.Add("readonly", "readonly")
        txttotal.Attributes.Add("readonly", "readonly")
        dtgltempo.Attributes.Add("readonly", "readonly")
        dtglcalonmacet.Attributes.Add("readonly", "readonly")
        dtglmacetbaru.Attributes.Add("readonly", "readonly")
        dtglmacetlama.Attributes.Add("readonly", "readonly")
        txtangsuranbunga.Attributes.Add("readonly", "readonly")
        txtangsuranpokok.Attributes.Add("readonly", "readonly")
        txtlaba.Attributes.Add("readonly", "readonly")
    End Sub
#End Region



#Region "Button"

    'Protected Sub lnkDelete_Click(sender As Object, e As System.EventArgs)

    'End Sub

    Protected Sub lnkSave_Click(sender As Object, e As System.EventArgs) Handles lnkSave.Click
        'CHECK REQUIRED FIELDS

        '  SimpanJadwalTagihan(Nothing, Nothing)

        Dim nojurnal As String = ddlunit.Text + ddlcabang.Text + "Jurnal Penjualan" + txtidtrans.Text
        Dim Keterangan As String = "Penjualan Sepeda Motor No Plat" + txtplat.Text + "Dengan Jenis" + txtjenis.Text & "Atas Nama" + txtnama.Text
        Dim Saldo2 As Integer = 0


        If ddjenis.Text = "Kredit" Then
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
            ElseIf String.IsNullOrEmpty(txttotal.Text.Trim()) Then
                Dim successScript As String = "setTimeout(function(){ Swal.fire({ icon: 'warning', title: 'TOTAL Tidak Boleh Kosong', showConfirmButton: false, showCancelButton: false, timer: 2000, timerProgressBar: true }); }, 100);"
                ScriptManager.RegisterStartupScript(Me, Me.[GetType](), "Popup", successScript, True)
                Exit Sub
            ElseIf String.IsNullOrEmpty(txtbunga.Text.Trim()) Then
                Dim successScript As String = "setTimeout(function(){ Swal.fire({ icon: 'warning', title: 'Bunga Tidak Boleh Kosong', showConfirmButton: false, showCancelButton: false, timer: 2000, timerProgressBar: true }); }, 100);"
                ScriptManager.RegisterStartupScript(Me, Me.[GetType](), "Popup", successScript, True)
                Exit Sub
            ElseIf String.IsNullOrEmpty(ddllama.Text.Trim()) Then
                Dim successScript As String = "setTimeout(function(){ Swal.fire({ icon: 'warning', title: 'Lama Angsuran Tidak Boleh Kosong', showConfirmButton: false, showCancelButton: false, timer: 2000, timerProgressBar: true }); }, 100);"
                ScriptManager.RegisterStartupScript(Me, Me.[GetType](), "Popup", successScript, True)
                Exit Sub
            ElseIf String.IsNullOrEmpty(dtgl.Value.Trim()) Then
                Dim successScript As String = "setTimeout(function(){ Swal.fire({ icon: 'warning', title: 'Tanggal Tidak Boleh Kosong', showConfirmButton: false, showCancelButton: false, timer: 2000, timerProgressBar: true }); }, 100);"
                ScriptManager.RegisterStartupScript(Me, Me.[GetType](), "Popup", successScript, True)
                Exit Sub
            ElseIf String.IsNullOrEmpty(txtdp.Text.Trim()) Then
                Dim successScript As String = "setTimeout(function(){ Swal.fire({ icon: 'warning', title: 'DP Tidak Boleh Kosong', showConfirmButton: false, showCancelButton: false, timer: 2000, timerProgressBar: true }); }, 100);"
                ScriptManager.RegisterStartupScript(Me, Me.[GetType](), "Popup", successScript, True)
                Exit Sub
            End If



            Dim NO As String = clsEnt.ReturnOneFieldConn("select top(1)idtrans + 1 as nnn from Tbljual where idnasabah='" & txtidnasabah.Text & "' order by idtrans desc ", "", "", "nnn", "connuq")
            Dim pinjamanke As Integer = IIf(NO = "", "1", NO)


            Dim strUID As String = Trim(Session.Item("UserAuthentication").ToString)



            'Check untuk melakukan tambah data
            If Not CheckAdaData(txtidtrans.Text.Trim(), "idtrans", "Tbljual") Then

                Dim sTrueMk As DataTable = GetDataTbl("select count(idnasabah) as cnt from tbljual where  noplat='" & txtplat.Text & "'")
                If CInt(sTrueMk.Rows(0).Item("cnt")) > 0 Then
                    Dim successScript As String = "setTimeout(function(){ Swal.fire({ icon: 'warning', title: 'Nomor Plat Sudah Ada', showConfirmButton: false, showCancelButton: false, timer: 2000, timerProgressBar: true }); }, 100);"
                    ScriptManager.RegisterStartupScript(Me, Me.[GetType](), "Popup", successScript, True)
                    Exit Sub
                End If

                SimpanJadwalTagihan(Nothing, Nothing)
                DeleteJurnal(Nothing, Nothing)
                SimpanJurnal(Nothing, Nothing)

                connectDB()
                mySqlCon.Open()
                mySqlCmd = New System.Data.OleDb.OleDbCommand()

                Dim Transaction = mySqlCon.BeginTransaction(IsolationLevel.ReadCommitted)
                mySqlCmd.Connection = mySqlCon
                mySqlCmd.Transaction = Transaction

                Try
                    mySqlCmd.CommandText = mySqlCmd.CommandText & "INSERT INTO Tbljual(idTrans,Keterangan,idSales,idSurvey,Wipem,IdNasabah,IdCabang,Idunit,NoPlat,Merk,Jenis,HargaPokok,HargaJual,MarkupLaba,AngsuranPokok,AngsuranLaba,DP,Total,LamaAngsuran,Angsuran,Bunga,SaldoAwal,NoMesin,NoRangka,Tanggal,PinjamanKe,JenisPenjualan,TglJatuhTempo,TglCalonMacet,TglMacetBaru,TglMacetlama,LastUpdate,UseridUpdate) VALUES ('" _
                        & txtidtrans.Text & "', '" & Keterangan & "','" & txtsales.Text & "', '" & txtsurvey.Text & "','" & txtwipem.Text & "','" & txtidnasabah.Text & "', '" & ddlcabang.Text & "', '" & ddlunit.Text & "', '" & txtplat.Text & "', '" & txtmerk.Text & "', '" & txtjenis.Text & "', " _
                        & txtpokok.Text & "," & txtjual2.Text & "," & txtlaba2.Text & "," & txtangsuranpokok2.Text & "," & txtangsuranbunga2.Text & "," & txtdp.Text & "," & txttotal2.Text & ", '" & ddllama.Text & "'," & txtangsuran2.Text & "," & txtbunga2.Text & ", " & txtsaldo2.Text & ",'" & txtnomesin.Text & "','" & txtnorangka.Text & "','" & clsEnt.CDateME5(dtgl.Value) & "'," & pinjamanke & ",'" & ddjenis.Text & "','" & clsEnt.CDateME5(dtgltempo.Value) & "','" & clsEnt.CDateME5(dtglcalonmacet.Value) & "','" & clsEnt.CDateME5(dtglmacetbaru.Value) & "','" & clsEnt.CDateME5(dtglmacetlama.Value) & "',Getdate(),'" & strUID & "');" _
                        & "INSERT INTO TblMutasiNasabah(NoPinjaman,idNasabah,SaldoAwal,Pokok,Bunga,TanggalPinjam,idCabang,idUnit,LastUpdate,UseridUpdate) VALUES ('" _
                        & txtidtrans.Text & "','" & txtidnasabah.Text & "'," & txtsaldo2.Text & "," & txtpokok.Text & "," & txtbunga2.Text & ",'" & clsEnt.CDateME5(dtgl.Value) & "','" & ddlcabang.Text & "','" & ddlunit.Text & "', Getdate(),'" & strUID & "'); "


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
                    Response.Redirect("JualBarangList.aspx")
                End Try
            Else
                connectDB()
                mySqlCon.Open()
                mySqlCmd = New System.Data.OleDb.OleDbCommand()

                Dim Transaction = mySqlCon.BeginTransaction(IsolationLevel.ReadCommitted)
                mySqlCmd.Connection = mySqlCon
                mySqlCmd.Transaction = Transaction

                Try
                    mySqlCmd.CommandText = mySqlCmd.CommandText & "UPDATE Tbljual SET Keterangan='" & Keterangan & "', HargaJual=" & txtjual2.Text & " ,MarkupLaba=" & txtlaba2.Text & ",AngsuranPokok=" & txtangsuranpokok2.Text & ", AngsuranLaba=" & txtangsuranbunga2.Text & ",Wipem='" & txtwipem.Text & "', idnasabah='" & txtidnasabah.Text & "', idSales='" & txtsales.Text & "', idSurvey='" & txtsurvey.Text & "', IdCabang ='" & ddlcabang.Text & "',Idunit='" & ddlunit.Text & "' ,NoPlat='" & txtplat.Text & "',Merk='" & txtmerk.Text & "',Jenis='" & txtjenis.Text & "',NoMesin='" & txtnomesin.Text & "',NoRangka='" & txtnorangka.Text & "', SaldoAwal=" & txtsaldo2.Text & ",Total=" & txttotal2.Text & ", LamaAngsuran=" & ddllama.Text & ",Angsuran=" & txtangsuran2.Text & ",bunga=" & txtbunga2.Text & ", DP=" & txtdp.Text & ", Tanggal ='" & clsEnt.CDateME5(dtgl.Value) & "',LastUpdate=Getdate(),UseridUpdate='" & strUID & "' WHERE idtrans= '" & txtidtrans.Text.Trim() & "';" _
                                                                & "UPDATE TblJurnalNasabah SET NoJurnal='" & txtnojurnal.Text & "', idNasabah='" & txtidnasabah.Text & "', Tanggal='" & clsEnt.CDateME5(dtgl.Value) & "',Keterangan='" & Keterangan & "',Debet=" & txtangsuran2.Text & ",SaldoAkhir=" & txtangsuran2.Text & ", LastUpdate=Getdate(),UseridUpdate='" & strUID & "' WHERE NoPinjaman= '" & txtidtrans.Text.Trim() & "';"
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
                    Response.Redirect("JualBarangList.aspx")
                End Try
            End If

        ElseIf txtjenis.Text = "Leasing" Then
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
            ElseIf String.IsNullOrEmpty(txttotal.Text.Trim()) Then
                Dim successScript As String = "setTimeout(function(){ Swal.fire({ icon: 'warning', title: 'TOTAL Tidak Boleh Kosong', showConfirmButton: false, showCancelButton: false, timer: 2000, timerProgressBar: true }); }, 100);"
                ScriptManager.RegisterStartupScript(Me, Me.[GetType](), "Popup", successScript, True)
                Exit Sub
            ElseIf String.IsNullOrEmpty(txtbunga.Text.Trim()) Then
                Dim successScript As String = "setTimeout(function(){ Swal.fire({ icon: 'warning', title: 'Bunga Tidak Boleh Kosong', showConfirmButton: false, showCancelButton: false, timer: 2000, timerProgressBar: true }); }, 100);"
                ScriptManager.RegisterStartupScript(Me, Me.[GetType](), "Popup", successScript, True)
                Exit Sub
            ElseIf String.IsNullOrEmpty(ddllama.Text.Trim()) Then
                Dim successScript As String = "setTimeout(function(){ Swal.fire({ icon: 'warning', title: 'Lama Angsuran Tidak Boleh Kosong', showConfirmButton: false, showCancelButton: false, timer: 2000, timerProgressBar: true }); }, 100);"
                ScriptManager.RegisterStartupScript(Me, Me.[GetType](), "Popup", successScript, True)
                Exit Sub
            ElseIf String.IsNullOrEmpty(dtgl.Value.Trim()) Then
                Dim successScript As String = "setTimeout(function(){ Swal.fire({ icon: 'warning', title: 'Tanggal Tidak Boleh Kosong', showConfirmButton: false, showCancelButton: false, timer: 2000, timerProgressBar: true }); }, 100);"
                ScriptManager.RegisterStartupScript(Me, Me.[GetType](), "Popup", successScript, True)
                Exit Sub
            ElseIf String.IsNullOrEmpty(txtdp.Text.Trim()) Then
                Dim successScript As String = "setTimeout(function(){ Swal.fire({ icon: 'warning', title: 'DP Tidak Boleh Kosong', showConfirmButton: false, showCancelButton: false, timer: 2000, timerProgressBar: true }); }, 100);"
                ScriptManager.RegisterStartupScript(Me, Me.[GetType](), "Popup", successScript, True)
                Exit Sub
            End If


            Dim NO As String = clsEnt.ReturnOneFieldConn("select top(1)idtrans + 1 as nnn from Tbljual where idnasabah='" & txtidnasabah.Text & "' order by idtrans desc ", "", "", "nnn", "connuq")
            Dim pinjamanke As Integer = IIf(NO = "", "1", NO)


            Dim strUID As String = Trim(Session.Item("UserAuthentication").ToString)
            SimpanJadwalTagihan(Nothing, Nothing)

            'Check untuk melakukan tambah data
            If Not CheckAdaData(txtidtrans.Text.Trim(), "idtrans", "Tbljual") Then

                Dim sTrueMk As DataTable = GetDataTbl("select count(idnasabah) as cnt from tbljual where  noplat='" & txtplat.Text & "'")
                If CInt(sTrueMk.Rows(0).Item("cnt")) > 0 Then
                    Dim successScript As String = "setTimeout(function(){ Swal.fire({ icon: 'warning', title: 'Nomor Plat Sudah Ada', showConfirmButton: false, showCancelButton: false, timer: 2000, timerProgressBar: true }); }, 100);"
                    ScriptManager.RegisterStartupScript(Me, Me.[GetType](), "Popup", successScript, True)
                    Exit Sub
                End If
                DeleteJurnal(Nothing, Nothing)
                SimpanJurnal(Nothing, Nothing)
                connectDB()
                mySqlCon.Open()
                mySqlCmd = New System.Data.OleDb.OleDbCommand()

                Dim Transaction = mySqlCon.BeginTransaction(IsolationLevel.ReadCommitted)
                mySqlCmd.Connection = mySqlCon
                mySqlCmd.Transaction = Transaction

                Try
                    mySqlCmd.CommandText = mySqlCmd.CommandText & "INSERT INTO Tbljual(idTrans,Keterangan,idSales,idSurvey,Wipem,IdNasabah,IdCabang,Idunit,NoPlat,Merk,Jenis,HargaPokok,HargaJual,MarkupLaba,AngsuranPokok,AngsuranLaba,DP,Total,LamaAngsuran,Angsuran,Bunga,SaldoAwal,NoMesin,NoRangka,Tanggal,PinjamanKe,JenisPenjualan,TglJatuhTempo,TglCalonMacet,TglMacetBaru,TglMacetlama,LastUpdate,UseridUpdate) VALUES ('" _
                        & txtidtrans.Text & "', '" & Keterangan & "','" & txtsales.Text & "', '" & txtsurvey.Text & "','" & txtwipem.Text & "','" & txtidnasabah.Text & "', '" & ddlcabang.Text & "', '" & ddlunit.Text & "', '" & txtplat.Text & "', '" & txtmerk.Text & "', '" & txtjenis.Text & "', " _
                        & txtpokok.Text & "," & txtjual2.Text & "," & txtlaba2.Text & "," & txtangsuranpokok2.Text & "," & txtangsuranbunga2.Text & "," & txtdp.Text & "," & txttotal2.Text & ", '" & ddllama.Text & "'," & txtangsuran2.Text & "," & txtbunga2.Text & ", " & txtsaldo2.Text & ",'" & txtnomesin.Text & "','" & txtnorangka.Text & "','" & clsEnt.CDateME5(dtgl.Value) & "'," & pinjamanke & ",'" & ddjenis.Text & "','" & clsEnt.CDateME5(dtgltempo.Value) & "','" & clsEnt.CDateME5(dtglcalonmacet.Value) & "','" & clsEnt.CDateME5(dtglmacetbaru.Value) & "','" & clsEnt.CDateME5(dtglmacetlama.Value) & "',Getdate(),'" & strUID & "');" _
                        & "INSERT INTO TblMutasiNasabah(NoPinjaman,idNasabah,SaldoAwal,Pokok,Bunga,TanggalPinjam,idCabang,idUnit,LastUpdate,UseridUpdate) VALUES ('" _
                        & txtidtrans.Text & "','" & txtidnasabah.Text & "'," & txtsaldo2.Text & "," & txtpokok.Text & "," & txtbunga2.Text & ",'" & clsEnt.CDateME5(dtgl.Value) & "','" & ddlcabang.Text & "','" & ddlunit.Text & "', Getdate(),'" & strUID & "'); "


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
                    Response.Redirect("JualBarangList.aspx")
                End Try
            Else
                connectDB()
                mySqlCon.Open()
                mySqlCmd = New System.Data.OleDb.OleDbCommand()

                Dim Transaction = mySqlCon.BeginTransaction(IsolationLevel.ReadCommitted)
                mySqlCmd.Connection = mySqlCon
                mySqlCmd.Transaction = Transaction

                Try
                    mySqlCmd.CommandText = mySqlCmd.CommandText & "UPDATE Tbljual SET Keterangan='" & Keterangan & "', HargaJual=" & txtjual2.Text & " ,MarkupLaba=" & txtlaba2.Text & ",AngsuranPokok=" & txtangsuranpokok2.Text & ", AngsuranLaba=" & txtangsuranbunga2.Text & ",Wipem='" & txtwipem.Text & "', idnasabah='" & txtidnasabah.Text & "', idSales='" & txtsales.Text & "', idSurvey='" & txtsurvey.Text & "', IdCabang ='" & ddlcabang.Text & "',Idunit='" & ddlunit.Text & "' ,NoPlat='" & txtplat.Text & "',Merk='" & txtmerk.Text & "',Jenis='" & txtjenis.Text & "',NoMesin='" & txtnomesin.Text & "',NoRangka='" & txtnorangka.Text & "', SaldoAwal=" & txtsaldo2.Text & ",Total=" & txttotal2.Text & ", LamaAngsuran=" & ddllama.Text & ",Angsuran=" & txtangsuran2.Text & ",bunga=" & txtbunga2.Text & ", DP=" & txtdp.Text & ", Tanggal ='" & clsEnt.CDateME5(dtgl.Value) & "',LastUpdate=Getdate(),UseridUpdate='" & strUID & "' WHERE idtrans= '" & txtidtrans.Text.Trim() & "';" _
                                                                & "UPDATE TblJurnalNasabah SET NoJurnal='" & txtnojurnal.Text & "', idNasabah='" & txtidnasabah.Text & "', Tanggal='" & clsEnt.CDateME5(dtgl.Value) & "',Keterangan='" & Keterangan & "',Debet=" & txtangsuran2.Text & ",SaldoAkhir=" & txtangsuran2.Text & ", LastUpdate=Getdate(),UseridUpdate='" & strUID & "' WHERE NoPinjaman= '" & txtidtrans.Text.Trim() & "';"
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
                    Response.Redirect("JualBarangList.aspx")
                End Try
            End If

        ElseIf ddjenis.Text = "Tunai" Then
            If String.IsNullOrEmpty(dtgl.Value.Trim()) Then
                Dim successScript As String = "setTimeout(function(){ Swal.fire({ icon: 'warning', title: 'Tanggal Tidak Boleh Kosong', showConfirmButton: false, showCancelButton: false, timer: 2000, timerProgressBar: true }); }, 100);"
                ScriptManager.RegisterStartupScript(Me, Me.[GetType](), "Popup", successScript, True)
                Exit Sub
            ElseIf String.IsNullOrEmpty(txtjual.Text.Trim()) Then
                Dim successScript As String = "setTimeout(function(){ Swal.fire({ icon: 'warning', title: 'Harga Jual Tidak Boleh Kosong', showConfirmButton: false, showCancelButton: false, timer: 2000, timerProgressBar: true }); }, 100);"
                ScriptManager.RegisterStartupScript(Me, Me.[GetType](), "Popup", successScript, True)
                Exit Sub
            ElseIf String.IsNullOrEmpty(ddjenis.Text.Trim()) Then
                Dim successScript As String = "setTimeout(function(){ Swal.fire({ icon: 'warning', title: 'Jenis Penjualan Tidak Boleh Kosong', showConfirmButton: false, showCancelButton: false, timer: 2000, timerProgressBar: true }); }, 100);"
                ScriptManager.RegisterStartupScript(Me, Me.[GetType](), "Popup", successScript, True)
                Exit Sub
            End If

            Dim NO As String = clsEnt.ReturnOneFieldConn("select top(1)idtrans + 1 as nnn from Tbljual where idnasabah='" & txtidnasabah.Text & "' order by idtrans desc ", "", "", "nnn", "connuq")
            Dim pinjamanke As Integer = IIf(NO = "", "1", NO)
            Dim lamatunai As String = ""

            Dim strUID As String = Trim(Session.Item("UserAuthentication").ToString)

            'Check untuk melakukan tambah data
            If Not CheckAdaData(txtidtrans.Text.Trim(), "idtrans", "Tbljual") Then

                Dim sTrueMk As DataTable = GetDataTbl("select count(idnasabah) as cnt from tbljual where  noplat='" & txtplat.Text & "'")
                If CInt(sTrueMk.Rows(0).Item("cnt")) > 0 Then
                    Dim successScript As String = "setTimeout(function(){ Swal.fire({ icon: 'warning', title: 'Nomor Plat Sudah Ada', showConfirmButton: false, showCancelButton: false, timer: 2000, timerProgressBar: true }); }, 100);"
                    ScriptManager.RegisterStartupScript(Me, Me.[GetType](), "Popup", successScript, True)
                    Exit Sub
                End If

                connectDB()
                mySqlCon.Open()
                mySqlCmd = New System.Data.OleDb.OleDbCommand()

                Dim Transaction = mySqlCon.BeginTransaction(IsolationLevel.ReadCommitted)
                mySqlCmd.Connection = mySqlCon
                mySqlCmd.Transaction = Transaction

                Try
                    mySqlCmd.CommandText = "INSERT INTO Tbljual(idTrans,Keterangan,idSales,idSurvey,Wipem,IdNasabah,IdCabang,Idunit,NoPlat,Merk,Jenis,HargaPokok,HargaJual,MarkupLaba,AngsuranPokok,AngsuranLaba,DP,Total,LamaAngsuran,Angsuran,Bunga,SaldoAwal,NoMesin,NoRangka,Tanggal,PinjamanKe,JenisPenjualan,TglJatuhTempo,LastUpdate,UseridUpdate) VALUES ('" _
                         & txtidtrans.Text & "', '" & Keterangan & "','" & txtsales.Text & "', '" & txtsurvey.Text & "','" & txtwipem.Text & "','" & txtidnasabah.Text & "', '" & ddlcabang.Text & "', '" & ddlunit.Text & "', '" & txtplat.Text & "', '" & txtmerk.Text & "', '" & txtjenis.Text & "', " _
                         & txtpokok.Text & "," & txtjual.Text & "," & txtlaba2.Text & "," & txtangsuranpokok2.Text & "," & txtangsuranbunga2.Text & "," & txtdp.Text & "," & txttotal2.Text & ", '" & lamatunai & "'," & txtangsuran2.Text & "," & txtbunga2.Text & ", " & txtsaldo2.Text & ",'" & txtnomesin.Text & "','" & txtnorangka.Text & "','" & clsEnt.CDateME5(dtgl.Value) & "'," & pinjamanke & ",'" & ddjenis.Text & "','" & clsEnt.CDateME5(dtgltempo.Value) & "',Getdate(),'" & strUID & "');" _
                         & "INSERT INTO TblMutasiNasabah(NoPinjaman,idNasabah,SaldoAwal,Pokok,Bunga,TanggalPinjam,idCabang,idUnit,LastUpdate,UseridUpdate) VALUES ('" _
                         & txtidtrans.Text & "','" & txtidnasabah.Text & "'," & txtsaldo2.Text & "," & txtpokok.Text & "," & txtbunga2.Text & ",'" & clsEnt.CDateME5(dtgl.Value) & "','" & ddlcabang.Text & "','" & ddlunit.Text & "', Getdate(),'" & strUID & "'); "

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
                    Response.Redirect("JualBarangList.aspx")
                End Try
            Else
                connectDB()
                mySqlCon.Open()
                mySqlCmd = New System.Data.OleDb.OleDbCommand()

                Dim Transaction = mySqlCon.BeginTransaction(IsolationLevel.ReadCommitted)
                mySqlCmd.Connection = mySqlCon
                mySqlCmd.Transaction = Transaction

                Try
                    mySqlCmd.CommandText = mySqlCmd.CommandText & "UPDATE Tbljual SET JenisPenjualan='" & ddjenis.Text & "',Keterangan ='" & Keterangan & "', HargaJual=" & txtjual2.Text & " ,MarkupLaba=" & txtlaba2.Text & ",AngsuranPokok=" & txtangsuranpokok2.Text & ", AngsuranLaba=" & txtangsuranbunga2.Text & ",Wipem='" & txtwipem.Text & "', idnasabah='" & txtidnasabah.Text & "', idSales='" & txtsales.Text & "', idSurvey='" & txtsurvey.Text & "', IdCabang ='" & ddlcabang.Text & "',Idunit='" & ddlunit.Text & "' ,NoPlat='" & txtplat.Text & "',Merk='" & txtmerk.Text & "',Jenis='" & txtjenis.Text & "',NoMesin='" & txtnomesin.Text & "',NoRangka='" & txtnorangka.Text & "', SaldoAwal=" & txtsaldo2.Text & ",Total=" & txttotal2.Text & ", LamaAngsuran=" & ddllama.Text & ",Angsuran=" & txtangsuran2.Text & ",bunga=" & txtbunga2.Text & ", DP=" & txtdp.Text & ", Tanggal ='" & clsEnt.CDateME5(dtgl.Value) & "',LastUpdate=Getdate(),UseridUpdate='" & strUID & "' WHERE idtrans= '" & txtidtrans.Text.Trim() & "';" _
                    & "UPDATE TblJurnalNasabah SET NoJurnal='" & txtnojurnal.Text & "', idNasabah='" & txtidnasabah.Text & "', Tanggal='" & clsEnt.CDateME5(dtgl.Value) & "',Keterangan='" & Keterangan & "',Debet=" & txtangsuran2.Text & ",SaldoAkhir=" & txtangsuran2.Text & ", LastUpdate=Getdate(),UseridUpdate='" & strUID & "' WHERE NoPinjaman= '" & txtidtrans.Text.Trim() & "';"
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
                    Response.Redirect("JualBarangList.aspx")
                End Try
            End If
        End If
    End Sub



    Protected Sub btnNew_Click(sender As Object, e As System.EventArgs) Handles btnNew.Click
        Response.Redirect("JualBarangList.aspx")
        Session("Mode") = "New"
    End Sub


#End Region

    Private Sub ClearForm()
        txtidtrans.Text = ""
        txtplat.Text = ""
        txtmerk.Text = ""
        txtjenis.Text = ""
        txtnomesin.Text = ""
        txtnorangka.Text = ""
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
            command = New System.Data.OleDb.OleDbCommand("SELECT MarkupLaba as MarkupLaba2, angsuranpokok as angsuranpokok2, AngsuranLaba as angsuranbunga2, hargajual as hargajual2, FORMAT(a.Hargajual,'#,###,##0')  as Hargajual, FORMAT(a.AngsuranLaba,'#,###,##0')  as angsuranbunga, FORMAT(a.angsuranpokok,'#,###,##0')  as angsuranpokok, FORMAT(a.MarkupLaba,'#,###,##0')  as MarkupLaba, a.Wipem as Wipem, a.LamaAngsuran as LamaAngsuran, a.idSales as idSales, a.idSurvey as idSurvey, a.idnasabah as idnasabah,b.nama as nama, a.idTrans as idTrans, a.IdCabang as IdCabang, a.Idunit as Idunit, a.NoPlat as NoPlat, a.Merk as Merk, a.Jenis as Jenis,a.HargaPokok as Hargapokok2, a.DP as DP2, a.Total as total2,a.Angsuran as angsuran2, a.Bunga as Bunga2, a.SaldoAwal as Saldoawal2, FORMAT(SaldoAwal,'#,###,##0') as SaldoAwal, FORMAT(HargaPokok,'#,###,##0') as HargaPokok, FORMAT(DP,'#,###,##0')  as DP,FORMAT(Total,'#,###,##0')  as Total, FORMAT(SaldoAwal,'#,###,##0')  as SaldoAwal, FORMAT(Bunga,'#,###,##0')  as Bunga, FORMAT(Angsuran,'#,###,##0')  as Angsuran, a.NoMesin as NoMesin, a.NoRangka as NoRangka, CONVERT(VARCHAR(10),a.Tanggal,103) as Tanggal,a.Keterangan AS Keterangan, a.JenisPenjualan AS JenisPenjualan, CONVERT(VARCHAR(10),a.TglJatuhTempo,103) as TglJatuhTempo FROM Tbljual a inner join TblNasabah b on b.idnasabah=a.idnasabah WHERE idtrans= '" & a_Str & "'; ", mySqlCon)
            mySqlReader = command.ExecuteReader
            If mySqlReader.Read Then
                txtidtrans.Text = mySqlReader("idtrans").ToString()
                txtidnasabah.Text = mySqlReader("idnasabah").ToString()
                txtnama.Text = mySqlReader("nama").ToString()
                txtplat.Text = mySqlReader("NoPlat").ToString()
                txtmerk.Text = mySqlReader("Merk").ToString()
                txtjenis.Text = mySqlReader("Jenis").ToString()
                txtnomesin.Text = mySqlReader("NoMesin").ToString()
                txtnorangka.Text = mySqlReader("NoRangka").ToString()

                txtpokok.Text = mySqlReader("HargaPokok2").ToString()
                txtdp.Text = mySqlReader("DP2").ToString()
                txttotal.Text = mySqlReader("Total").ToString()
                txtsaldo.Text = mySqlReader("SaldoAwal").ToString()
                txtangsuran.Text = mySqlReader("angsuran").ToString()
                txtbunga.Text = mySqlReader("bunga").ToString()
                txtlaba.Text = mySqlReader("MarkupLaba").ToString()
                txtangsuranpokok.Text = mySqlReader("angsuranpokok").ToString()
                txtangsuranbunga.Text = mySqlReader("angsuranbunga").ToString()
                txtjual.Text = mySqlReader("hargajual2").ToString()

                txttotal2.Text = mySqlReader("Total2").ToString()
                txtsaldo2.Text = mySqlReader("SaldoAwal2").ToString()
                txtangsuran2.Text = mySqlReader("angsuran2").ToString()
                txtbunga2.Text = mySqlReader("bunga2").ToString()
                txtlaba2.Text = mySqlReader("MarkupLaba2").ToString()
                txtangsuranpokok2.Text = mySqlReader("angsuranpokok2").ToString()
                txtangsuranbunga2.Text = mySqlReader("angsuranbunga2").ToString()
                txtjual2.Text = mySqlReader("Hargajual2").ToString()
                ddllama.Text = mySqlReader("LamaAngsuran").ToString()


                dtgl.Value = mySqlReader("tanggal").ToString()
                ddlcabang.Text = mySqlReader("IdCabang").ToString()
                ddlunit.Text = mySqlReader("Idunit").ToString()
                ddjenis.Text = mySqlReader("JenisPenjualan").ToString()
                txtsales.Text = mySqlReader("idSales").ToString()
                txtsurvey.Text = mySqlReader("idSurvey").ToString()
                txtwipem.Text = mySqlReader("Wipem").ToString()
                dtgltempo.Value = mySqlReader("TglJatuhTempo").ToString()

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
                txtnomesin.Text = ""
                txtnorangka.Text = ""
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

    Public Sub Clearrumus()
        txtpokok.Text = 0
        txtjual.Text = 0
        txtdp.Text = 0
        txtangsuran.Text = 0
        txtangsuranbunga.Text = 0
        txtsaldo.Text = 0
        txtangsuranpokok.Text = 0
        txtbunga.Text = 0
        txtlaba.Text = 0
        txttotal.Text = 0


        txtjual2.Text = 0
        txtangsuran2.Text = 0
        txtangsuranbunga2.Text = 0
        txtsaldo2.Text = 0
        txtangsuranpokok2.Text = 0
        txtbunga2.Text = 0
        txtlaba2.Text = 0
        txttotal2.Text = 0
    End Sub

#Region "POP UP"
    Private Sub ShowSweetAlert2(ByVal icon As String, ByVal titleString As String)
        Dim sweetAlertString As String = "setTimeout(function(){ Swal.fire({ icon: '" & icon & "', title: '" & titleString & "', showConfirmButton: false, showCancelButton: false, timer: 2000, timerProgressBar: true }); }, 100);"
        ScriptManager.RegisterStartupScript(Me, Me.[GetType](), "Popup", sweetAlertString, True)
    End Sub
#End Region

    Public Sub NOOTOMATIS()
        'Dim sTrue As DataTable = GetDataTbl("select Max(idtrans) + 1 as total from Tbljual")
        'Dim nourut As String = CInt(sTrue.Rows(0).Item("total"))
        Dim NO As String = clsEnt.ReturnOneFieldConn("select top(1)idtrans + 1 as nnn from Tbljual order by idtrans desc ", "", "", "nnn", "connuq")
        txtidtrans.Text = IIf(NO = "", "00001", "0000" + NO)
    End Sub

    Protected Sub btnNew2_Click(sender As Object, e As System.EventArgs) Handles btnNew2.Click
        ClearForm()
    End Sub

    Protected Sub lnkDelete_Click(sender As Object, e As System.EventArgs) Handles lnkDelete.Click
        ''CHECK REQUIRED FIELDS

        Dim sTrueMk As DataTable = GetDataTbl("select count(idnasabah) as cnt from TblAngsuran where Nopinjaman='" & txtidtrans.Text & "'")
        If CInt(sTrueMk.Rows(0).Item("cnt")) > 0 Then
            Dim successScript As String = "setTimeout(function(){ Swal.fire({ icon: 'info', title: 'Mohon Maaf Angsuran Sudah Pernah Terinput Sudah Terinput...', showConfirmButton: true, showCancelButton: false, timer: false, timerProgressBar: false }); }, 100);"
            ScriptManager.RegisterStartupScript(Me, Me.[GetType](), "Popup", successScript, True)
            Exit Sub
        End If

        connectDB()
        mySqlCon.Open()
        mySqlCmd = New System.Data.OleDb.OleDbCommand()

        Dim Transaction = mySqlCon.BeginTransaction(IsolationLevel.ReadCommitted)
        mySqlCmd.Connection = mySqlCon
        mySqlCmd.Transaction = Transaction

        Try
            mySqlCmd.CommandText = mySqlCmd.CommandText & "DELETE FROM Tbljual WHERE idtrans = '" & txtidtrans.Text.Trim() & "';" _
                                 & "DELETE FROM TblTagihanNasabah WHERE idTransJual='" & txtidtrans.Text.Trim() & "';" _
                                 & "DELETE FROM TblJurnalNasabah WHERE nopinjaman='" & txtidtrans.Text.Trim() & "';"
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
            Response.Redirect("JualBarangList.aspx")
        End Try
    End Sub

    Protected Sub Lbtnhitung_Click(sender As Object, e As EventArgs) Handles Lbtnhitung.Click
        '  Clearrumus()


        '    Clearrumus()

        If ddjenis.Text = "Kredit" Then
            If String.IsNullOrEmpty(ddllama.Text.Trim()) Then
                Dim successScript As String = "setTimeout(function(){ Swal.fire({ icon: 'warning', title: 'Lama Angsuran Tidak Boleh Kosong', showConfirmButton: false, showCancelButton: false, timer: 2000, timerProgressBar: true }); }, 100);"
                ScriptManager.RegisterStartupScript(Me, Me.[GetType](), "Popup", successScript, True)
                Exit Sub
            ElseIf String.IsNullOrEmpty(dtgl.Value.Trim()) Then
                Dim successScript As String = "setTimeout(function(){ Swal.fire({ icon: 'warning', title: 'Tanggal Tidak Boleh Kosong', showConfirmButton: false, showCancelButton: false, timer: 2000, timerProgressBar: true }); }, 100);"
                ScriptManager.RegisterStartupScript(Me, Me.[GetType](), "Popup", successScript, True)
                Exit Sub
            ElseIf String.IsNullOrEmpty(txtdp.Text.Trim()) Then
                Dim successScript As String = "setTimeout(function(){ Swal.fire({ icon: 'warning', title: 'DP Tidak Boleh Kosong', showConfirmButton: false, showCancelButton: false, timer: 2000, timerProgressBar: true }); }, 100);"
                ScriptManager.RegisterStartupScript(Me, Me.[GetType](), "Popup", successScript, True)
                Exit Sub
            ElseIf String.IsNullOrEmpty(txtjual.Text.Trim()) Then
                Dim successScript As String = "setTimeout(function(){ Swal.fire({ icon: 'warning', title: 'Harga Jual Tidak Boleh Kosong', showConfirmButton: false, showCancelButton: false, timer: 2000, timerProgressBar: true }); }, 100);"
                ScriptManager.RegisterStartupScript(Me, Me.[GetType](), "Popup", successScript, True)
                Exit Sub
            End If

            Dim hargapokok, uangdp, total, lama, totallama, saldowal, bunga, angsuran, hargajual As Decimal
            Dim lamaangsuran As Integer = ddllama.Text
            Dim lamacalonmacet As Integer = ddllama.Text - 1
            Dim lamamacetbaru As Integer = ddllama.Text + 1
            Dim lamamacetlama As Integer = ddllama.Text + 2

            Dim tglpinjam As Date = dtgl.Value
            Dim sTrue2 As DataTable = GetDataTbl("select isnull(jumlah,0) as jumlah from TblBungaPinjaman")
            Dim sukubanga As Decimal = sTrue2.Rows(0).Item("jumlah")
            Dim tgl_reg_sip As Date = tglpinjam.AddMonths(lamaangsuran)
            Dim tgl_calonMacet As Date = tglpinjam.AddMonths(lamacalonmacet)
            Dim tgl_macetbaru As Date = tglpinjam.AddMonths(lamamacetbaru)
            Dim tgl_macetlama As Date = tglpinjam.AddMonths(lamamacetlama)

            Dim saldohargajual As Integer = 0

            hargajual = txtjual.Text
            txtjual2.Text = hargajual
            hargapokok = txtpokok.Text
            uangdp = txtdp.Text
            lama = ddllama.Text
            saldohargajual = txtjual.Text
            totallama = sukubanga * lama
            total = saldohargajual - uangdp
            bunga = (total * totallama) / 100
            saldowal = Round(total + bunga)
            angsuran = Round(saldowal / lama)
            txtangsuran.Text = angsuran.ToString("c")
            txtsaldo.Text = saldowal.ToString("c")
            txtbunga.Text = (Round(total * totallama) / 100).ToString("c")
            txttotal.Text = total.ToString("c")
            txtangsuran2.Text = angsuran
            txtsaldo2.Text = saldowal
            txtbunga2.Text = (Round((total * totallama) / 100))
            txttotal2.Text = total
            dtgltempo.Value = tgl_reg_sip
            dtglcalonmacet.Value = tgl_calonMacet
            dtglmacetbaru.Value = tgl_macetbaru
            dtglmacetlama.Value = tgl_macetlama
            txtangsuranpokok.Text = (Round(txttotal.Text / lama)).ToString("c")
            txtangsuranpokok2.Text = (Round(txttotal.Text / lama))
            txtangsuranbunga.Text = (Round(txtbunga.Text / lama)).ToString("c")
            txtangsuranbunga2.Text = (Round(txtbunga.Text / lama))
            txtlaba.Text = (Round(saldohargajual - hargapokok)).ToString("c")
            txtlaba2.Text = Round(saldohargajual - hargapokok)
        ElseIf ddjenis.Text = "Leasing" Then
            If String.IsNullOrEmpty(ddllama.Text.Trim()) Then
                Dim successScript As String = "setTimeout(function(){ Swal.fire({ icon: 'warning', title: 'Lama Angsuran Tidak Boleh Kosong', showConfirmButton: false, showCancelButton: false, timer: 2000, timerProgressBar: true }); }, 100);"
                ScriptManager.RegisterStartupScript(Me, Me.[GetType](), "Popup", successScript, True)
                Exit Sub
            ElseIf String.IsNullOrEmpty(dtgl.Value.Trim()) Then
                Dim successScript As String = "setTimeout(function(){ Swal.fire({ icon: 'warning', title: 'Tanggal Tidak Boleh Kosong', showConfirmButton: false, showCancelButton: false, timer: 2000, timerProgressBar: true }); }, 100);"
                ScriptManager.RegisterStartupScript(Me, Me.[GetType](), "Popup", successScript, True)
                Exit Sub
            ElseIf String.IsNullOrEmpty(txtdp.Text.Trim()) Then
                Dim successScript As String = "setTimeout(function(){ Swal.fire({ icon: 'warning', title: 'DP Tidak Boleh Kosong', showConfirmButton: false, showCancelButton: false, timer: 2000, timerProgressBar: true }); }, 100);"
                ScriptManager.RegisterStartupScript(Me, Me.[GetType](), "Popup", successScript, True)
                Exit Sub
            ElseIf String.IsNullOrEmpty(txtjual.Text.Trim()) Then
                Dim successScript As String = "setTimeout(function(){ Swal.fire({ icon: 'warning', title: 'Harga Jual Tidak Boleh Kosong', showConfirmButton: false, showCancelButton: false, timer: 2000, timerProgressBar: true }); }, 100);"
                ScriptManager.RegisterStartupScript(Me, Me.[GetType](), "Popup", successScript, True)
                Exit Sub
            End If

            Dim hargapokok, uangdp, total, lama, totallama, saldowal, bunga, angsuran, hargajual As Decimal
            Dim lamaangsuran As Integer = ddllama.Text

            Dim lamacalonmacet As Integer = ddllama.Text - 1
            Dim lamamacetbaru As Integer = ddllama.Text + 1
            Dim lamamacetlama As Integer = ddllama.Text + 2

            Dim tglpinjam As Date = dtgl.Value
            Dim sTrue2 As DataTable = GetDataTbl("select isnull(jumlah,0) as jumlah from TblBungaPinjaman")
            Dim sukubanga As Decimal = sTrue2.Rows(0).Item("jumlah")
            Dim tgl_reg_sip As Date = tglpinjam.AddMonths(lamaangsuran)
            Dim tgl_calonMacet As Date = tglpinjam.AddMonths(lamacalonmacet)
            Dim tgl_macetbaru As Date = tglpinjam.AddMonths(lamamacetbaru)
            Dim tgl_macetlama As Date = tglpinjam.AddMonths(lamamacetlama)

            Dim saldohargajual As Integer = 0

            hargajual = txtjual.Text
            txtjual2.Text = hargajual
            hargapokok = txtpokok.Text
            uangdp = txtdp.Text
            lama = ddllama.Text
            saldohargajual = txtjual.Text
            totallama = sukubanga * lama
            total = saldohargajual - uangdp
            bunga = (total * totallama) / 100
            saldowal = Round(total + bunga)
            angsuran = Round(saldowal / lama)
            txtangsuran.Text = angsuran.ToString("c")
            txtsaldo.Text = saldowal.ToString("c")
            txtbunga.Text = (Round(total * totallama) / 100).ToString("c")
            txttotal.Text = total.ToString("c")
            txtangsuran2.Text = angsuran
            txtsaldo2.Text = saldowal
            txtbunga2.Text = (Round((total * totallama) / 100))
            txttotal2.Text = total
            dtgltempo.Value = tgl_reg_sip
            dtglcalonmacet.Value = tgl_calonMacet
            dtglmacetbaru.Value = tgl_macetbaru
            dtglmacetlama.Value = tgl_macetlama

            txtangsuranpokok.Text = (Round(txttotal.Text / lama)).ToString("c")
            txtangsuranpokok2.Text = (Round(txttotal.Text / lama))
            txtangsuranbunga.Text = (Round(txtbunga.Text / lama)).ToString("c")
            txtangsuranbunga2.Text = (Round(txtbunga.Text / lama))
            txtlaba.Text = (Round(saldohargajual - hargapokok)).ToString("c")
            txtlaba2.Text = Round(saldohargajual - hargapokok)
        ElseIf ddjenis.Text = "Tunai" Then
            If String.IsNullOrEmpty(dtgl.Value.Trim()) Then
                Dim successScript As String = "setTimeout(function(){ Swal.fire({ icon: 'warning', title: 'Tanggal Tidak Boleh Kosong', showConfirmButton: false, showCancelButton: false, timer: 2000, timerProgressBar: true }); }, 100);"
                ScriptManager.RegisterStartupScript(Me, Me.[GetType](), "Popup", successScript, True)
                Exit Sub
            ElseIf String.IsNullOrEmpty(txtjual.Text.Trim()) Then
                Dim successScript As String = "setTimeout(function(){ Swal.fire({ icon: 'warning', title: 'Harga Jual Tidak Boleh Kosong', showConfirmButton: false, showCancelButton: false, timer: 2000, timerProgressBar: true }); }, 100);"
                ScriptManager.RegisterStartupScript(Me, Me.[GetType](), "Popup", successScript, True)
                Exit Sub
            ElseIf String.IsNullOrEmpty(ddjenis.Text.Trim()) Then
                Dim successScript As String = "setTimeout(function(){ Swal.fire({ icon: 'warning', title: 'Jenis Penjualan Tidak Boleh Kosong', showConfirmButton: false, showCancelButton: false, timer: 2000, timerProgressBar: true }); }, 100);"
                ScriptManager.RegisterStartupScript(Me, Me.[GetType](), "Popup", successScript, True)
                Exit Sub
            End If
            '   Dim hargapokok, saldohargajual As Integer = 0
            Dim hargapokok, saldohargajual As Integer
            '   Dim saldohargajual As Integer

            hargapokok = txtpokok.Text
            saldohargajual = txtjual.Text
            txtlaba.Text = (Round(saldohargajual - hargapokok)).ToString("c")
            txtlaba2.Text = Round(saldohargajual - hargapokok)
            txtdp.Text = 0
            txtangsuran.Text = 0
            txtangsuranbunga.Text = 0
            txtsaldo.Text = 0
            txtangsuranpokok.Text = 0
            txtbunga.Text = 0
            txttotal.Text = 0
            txtangsuran2.Text = 0
            txtangsuranbunga2.Text = 0
            txtsaldo2.Text = 0
            txtangsuranpokok2.Text = 0
            txtbunga2.Text = 0
            txttotal2.Text = 0
            txtdp.Attributes.Add("readonly", "readonly")
            dtgltempo.Value = dtgl.Value
        End If
    End Sub

    Public Sub SimpanJadwalTagihan(sender As Object, e As EventArgs)
        'Dim mjum As Integer = ddllama.Text
        'Dim hjum As String = 1
        'Dim strUID As String = Trim(Session.Item("UserAuthentication").ToString)
        'Dim real As Integer = 0
        'Dim ketangsuran As String = ""


        ''  mySqlCmd.CommandText = "DELETE FROM TblTagihanNasabah WHERE NoPlat = '" & txtplat.Text.Trim() & "'; "

        'DeleteTagihan(Nothing, Nothing)
        'Do While hjum <= mjum
        '    Dim tanggalAwal As Date = dtgl.Value
        '    Dim yc As Integer = yc + 1
        '    Dim sur As Integer = sur + 1
        '    Dim tgl_reg_sip As Date = tanggalAwal.AddMonths(yc)

        '    connectDB()
        '    mySqlCon.Open()
        '    Dim mySqlCmd As Data.OleDb.OleDbCommand = New Data.OleDb.OleDbCommand()
        '    Dim Transaction = mySqlCon.BeginTransaction(IsolationLevel.ReadCommitted)
        '    mySqlCmd.Connection = mySqlCon
        '    mySqlCmd.Transaction = Transaction
        '    Try
        '        mySqlCmd.CommandType = CommandType.StoredProcedure
        '        mySqlCmd.CommandText = "USP_InsertJadwalTagihan"
        '        mySqlCmd.Parameters.AddWithValue("@SidTransJual", SqlDbType.VarChar).Value = txtidtrans.Text
        '        mySqlCmd.Parameters.AddWithValue("@SidCabang", SqlDbType.VarChar).Value = ddlcabang.Text
        '        mySqlCmd.Parameters.AddWithValue("@SidUnit", SqlDbType.VarChar).Value = ddlunit.Text
        '        mySqlCmd.Parameters.AddWithValue("@SidWipem", SqlDbType.VarChar).Value = txtwipem.Text
        '        mySqlCmd.Parameters.AddWithValue("@SidNasabah", SqlDbType.VarChar).Value = txtidnasabah.Text


        '        'mySqlCmd.CommandText = mySqlCmd.CommandText & "INSERT INTO TblTagihanNasabah(idTransJual,IdCabang,IdUnit,Wipem,IdNasabah) VALUES ('" _
        '        '& txtidtrans.Text & "', '" & ddlcabang.Text & "','" & ddlunit.Text & "','" & txtwipem.Text & "','" & txtidnasabah.Text & "');"

        '        'mySqlCmd.CommandText = mySqlCmd.CommandText & "INSERT INTO TblTagihanNasabah(idTransJual,IdCabang,IdUnit,Wipem,IdNasabah,NoPlat,TglPinjam,TglTagihan,AngsuranKe,JumlahPokok,JumlahBunga,Jumlah,RealisasiAngsuran,LastUpdate,UseridUpdate) VALUES ('" _
        '        '& txtidtrans.Text & "', '" & ddlcabang.Text & "','" & ddlunit.Text & "','" & txtwipem.Text & "','" & txtidnasabah.Text & "','" & txtplat.Text & "','" & clsEnt.CDateME5(dtgl.Value) & "','" & clsEnt.CDateME5(tgl_reg_sip) & "'," & sur & "," & txtangsuranpokok2.Text & "," & txtangsuranbunga2.Text & "," & txtangsuran2.Text & "," & real & ", Getdate(),'" & strUID & "');"

        '        mySqlCmd.ExecuteNonQuery()

        '        Transaction.Commit()
        '        Session("Mode") = "Edit"
        '        Dim successScript As String = "setTimeout(function(){ Swal.fire({ icon: 'success', title: 'Saved', showConfirmButton: false, showCancelButton: false, timer: 2000, timerProgressBar: true }); }, 100);"
        '        ScriptManager.RegisterStartupScript(Me, Me.[GetType](), "Popup", successScript, True)

        '    Catch ex As Exception
        '        Transaction.Rollback()
        '        Dim successScript As String = "setTimeout(function(){ Swal.fire({ icon: 'error', title: 'Error saving data', showConfirmButton: false, showCancelButton: false, timer: 2000, timerProgressBar: true }); }, 100);"
        '        ScriptManager.RegisterStartupScript(Me, Me.[GetType](), "Popup", successScript, True)

        '    Finally
        '        mySqlCon.Close()
        '        mySqlCon.Dispose()
        '        mySqlCmd.Parameters.Clear()
        '    End Try
        '    hjum = hjum + 1
        'Loop

        Dim mjum As Integer = ddllama.Text
        Dim hjum As String = 1
        Dim strUID As String = Trim(Session.Item("UserAuthentication").ToString)
        Dim real As Integer = 0
        Dim ketangsuran As String = ""


        '  mySqlCmd.CommandText = "DELETE FROM TblTagihanNasabah WHERE NoPlat = '" & txtplat.Text.Trim() & "'; "

        DeleteTagihan(Nothing, Nothing)
        For i As Integer = 1 To ddllama.Text
            Dim tanggalAwal As Date = dtgl.Value
            Dim yc As Integer = yc + 1
            Dim sur As Integer = sur + 1
            Dim tgl_reg_sip As Date = tanggalAwal.AddMonths(yc)

            connectDB()
            mySqlCon.Open()
            Dim mySqlCmd As Data.OleDb.OleDbCommand = New Data.OleDb.OleDbCommand()
            Dim Transaction = mySqlCon.BeginTransaction(IsolationLevel.ReadCommitted)
            mySqlCmd.Connection = mySqlCon
            mySqlCmd.Transaction = Transaction
            Try

                mySqlCmd.CommandText = mySqlCmd.CommandText & "INSERT INTO TblTagihanNasabah(idTransJual,IdCabang,IdUnit,Wipem,IdNasabah,NoPlat,TglPinjam,TglTagihan,AngsuranKe,JumlahPokok,JumlahBunga,Jumlah,RealisasiAngsuran,StatusPembayaran,UseridUpdate,LastUpdate) VALUES ('" _
                & txtidtrans.Text & "', '" & ddlcabang.Text & "','" & ddlunit.Text & "','" & txtwipem.Text & "','" & txtidnasabah.Text & "','" & txtplat.Text & "','" & clsEnt.CDateME5(dtgl.Value) & "','" & clsEnt.CDateME5(tgl_reg_sip) & "'," & sur & "," & txtangsuranpokok2.Text & "," & txtangsuranbunga2.Text & "," & txtangsuran2.Text & "," & real & ",'" & ketangsuran & "','" & strUID & "',getdate());"

                'mySqlCmd.CommandText = mySqlCmd.CommandText & "INSERT INTO TblTagihanNasabah(idTransJual,IdCabang,IdUnit,Wipem,IdNasabah,NoPlat,TglPinjam,TglTagihan,AngsuranKe,JumlahPokok,JumlahBunga,Jumlah,RealisasiAngsuran,LastUpdate,UseridUpdate) VALUES ('" _
                '& txtidtrans.Text & "', '" & ddlcabang.Text & "','" & ddlunit.Text & "','" & txtwipem.Text & "','" & txtidnasabah.Text & "','" & txtplat.Text & "','" & clsEnt.CDateME5(dtgl.Value) & "','" & clsEnt.CDateME5(tgl_reg_sip) & "'," & sur & "," & txtangsuranpokok2.Text & "," & txtangsuranbunga2.Text & "," & txtangsuran2.Text & "," & real & ", Getdate(),'" & strUID & "');"

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
            End Try
            hjum = hjum + 1
        Next

    End Sub

    Public Sub SimpanJurnal(sender As Object, e As EventArgs)
        Dim mjum As Integer = ddllama.Text
        Dim hjum As String = 1
        Dim strUID As String = Trim(Session.Item("UserAuthentication").ToString)
        Dim Saldo2 As Integer = 0


        Dim nojurnal As String = ddlunit.Text + ddlcabang.Text + txtidtrans.Text
        Dim Keterangan As String = "Penjualan Sepeda Motor No Plat" + txtplat.Text + "Dengan Jenis" + txtjenis.Text & "Atas Nama" + txtnama.Text


        connectDB()
        mySqlCon.Open()
        mySqlCmd = New System.Data.OleDb.OleDbCommand()

        Dim Transaction = mySqlCon.BeginTransaction(IsolationLevel.ReadCommitted)
        mySqlCmd.Connection = mySqlCon
        mySqlCmd.Transaction = Transaction
        Try
            mySqlCmd.CommandText = "INSERT INTO TblJurnalNasabah(NoJurnal,NoPinjaman,IdCabang,IdUnit,IdNasabah,tanggal,Keterangan,Debet,Kredit,SaldoAkhir,LastUpdate,UseridUpdate) VALUES ('" _
                   & nojurnal & "','" & txtidtrans.Text & "','" & ddlcabang.Text & "','" & ddlunit.Text & "','" & txtidnasabah.Text & "','" & clsEnt.CDateME5(dtgl.Value) & "','" & Keterangan & "'," & txtsaldo2.Text & "," & Saldo2 & "," & txtsaldo2.Text & ", Getdate(),'" & strUID & "'); "

            mySqlCmd.ExecuteNonQuery()

            Transaction.Commit()
            Session("Mode") = "Edit"
            'Dim successScript As String = "setTimeout(function(){ Swal.fire({ icon: 'success', title: 'Saved', showConfirmButton: false, showCancelButton: false, timer: 2000, timerProgressBar: true }); }, 100);"
            'ScriptManager.RegisterStartupScript(Me, Me.[GetType](), "Popup", successScript, True)

        Catch ex As Exception
            Transaction.Rollback()
            Dim successScript As String = "setTimeout(function(){ Swal.fire({ icon: 'error', title: 'Error saving data', showConfirmButton: false, showCancelButton: false, timer: 2000, timerProgressBar: true }); }, 100);"
            ScriptManager.RegisterStartupScript(Me, Me.[GetType](), "Popup", successScript, True)
            Transaction.Rollback()
        Finally
            mySqlCon.Close()
            mySqlCon.Dispose()
            'mySqlCmd.Parameters.Clear()
            'Response.Redirect("JualBarangList.aspx")
        End Try
    End Sub

    Public Sub DeleteJurnal(sender As Object, e As EventArgs)
        Dim mjum As Integer = ddllama.Text
        Dim hjum As String = 1
        Dim strUID As String = Trim(Session.Item("UserAuthentication").ToString)
        Dim Saldo2 As Integer = 0


        Dim nojurnal As String = ddlunit.Text + ddlcabang.Text + txtidtrans.Text
        Dim Keterangan As String = "Penjualan Sepeda Motor No Plat" + txtplat.Text + "Dengan Jenis" + txtjenis.Text & "Atas Nama" + txtnama.Text


        connectDB()
        mySqlCon.Open()
        mySqlCmd = New System.Data.OleDb.OleDbCommand()

        Dim Transaction = mySqlCon.BeginTransaction(IsolationLevel.ReadCommitted)
        mySqlCmd.Connection = mySqlCon
        mySqlCmd.Transaction = Transaction

        Try
            mySqlCmd.CommandText = "DELETE FROM TblJurnalNasabah WHERE nopinjaman = '" & txtidtrans.Text.Trim() & "'; "

            mySqlCmd.ExecuteNonQuery()

            Transaction.Commit()
            Session("Mode") = "Edit"
            'Dim successScript As String = "setTimeout(function(){ Swal.fire({ icon: 'success', title: 'Saved', showConfirmButton: false, showCancelButton: false, timer: 2000, timerProgressBar: true }); }, 100);"
            'ScriptManager.RegisterStartupScript(Me, Me.[GetType](), "Popup", successScript, True)

        Catch ex As Exception
            Transaction.Rollback()
            Dim successScript As String = "setTimeout(function(){ Swal.fire({ icon: 'error', title: 'Error saving data', showConfirmButton: false, showCancelButton: false, timer: 2000, timerProgressBar: true }); }, 100);"
            ScriptManager.RegisterStartupScript(Me, Me.[GetType](), "Popup", successScript, True)
            Transaction.Rollback()
        Finally
            mySqlCon.Close()
            mySqlCon.Dispose()
            'mySqlCmd.Parameters.Clear()
            'Response.Redirect("JualBarangList.aspx")
        End Try
    End Sub
    Public Sub DeleteTagihan(sender As Object, e As System.EventArgs)

        connectDB()
        mySqlCon.Open()
        mySqlCmd = New System.Data.OleDb.OleDbCommand()

        Dim Transaction = mySqlCon.BeginTransaction(IsolationLevel.ReadCommitted)
        mySqlCmd.Connection = mySqlCon
        mySqlCmd.Transaction = Transaction

        Try
            mySqlCmd.CommandType = CommandType.StoredProcedure
            mySqlCmd.CommandText = "USP_DeleteTagihan"
            mySqlCmd.Parameters.AddWithValue("@SidTransJual", SqlDbType.VarChar).Value = txtidtrans.Text

            mySqlCmd.ExecuteNonQuery()
            Transaction.Commit()

        Catch ex As Exception
            Dim successScript As String = "setTimeout(function(){ Swal.fire({ icon: 'error', title: 'Error saving data', showConfirmButton: false, showCancelButton: false, timer: 2000, timerProgressBar: true }); }, 100);"
            ScriptManager.RegisterStartupScript(Me, Me.[GetType](), "Popup", successScript, True)

        Finally
            mySqlCon.Close()
            mySqlCon.Dispose()
            mySqlCmd.Parameters.Clear()

        End Try

    End Sub



End Class
