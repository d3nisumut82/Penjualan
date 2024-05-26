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

Partial Class AngsuranNasabah
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
            End If

        End If

        txtidtrans.Attributes.Add("readonly", "readonly")
        txtnopinjaman.Attributes.Add("readonly", "readonly")
        txtidnasabah.Attributes.Add("readonly", "readonly")
        ddlunit.Attributes.Add("readonly", "readonly")
        ddlcabang.Attributes.Add("readonly", "readonly")
        txtnama.Attributes.Add("readonly", "readonly")
        txtmerk.Attributes.Add("readonly", "readonly")
        txtjenis.Attributes.Add("readonly", "readonly")
        txtnomesin.Attributes.Add("readonly", "readonly")
        txtnorangka.Attributes.Add("readonly", "readonly")
        txtwipem.Attributes.Add("readonly", "readonly")
        txtsales.Attributes.Add("readonly", "readonly")
        txtsurvey.Attributes.Add("readonly", "readonly")
        txtangsuranke.Attributes.Add("readonly", "readonly")
        dtgltempo.Attributes.Add("readonly", "readonly")
        txtjumlah.Attributes.Add("readonly", "readonly")
        txtpokok.Attributes.Add("readonly", "readonly")
        txtbunga.Attributes.Add("readonly", "readonly")
        txtpokok2.Attributes.Add("readonly", "readonly")
        txtbunga2.Attributes.Add("readonly", "readonly")
        dtgltempokontrak.Attributes.Add("readonly", "readonly")
        txtsisasaldo.Attributes.Add("readonly", "readonly")
    End Sub
#End Region



#Region "Button"

    'Protected Sub lnkDelete_Click(sender As Object, e As System.EventArgs)

    'End Sub

    Protected Sub lnkSave_Click(sender As Object, e As System.EventArgs) Handles lnkSave.Click
        'DeleteJurnal(Nothing, Nothing)
        'SimpanJurnal(Nothing, Nothing)
        '   CHECK REQUIRED FIELDS
        If rdBangsuran.Checked = True Then
            Dim kosong As Integer = 0

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
            ElseIf String.IsNullOrEmpty(ddlcabang.Text.Trim()) Then
                Dim successScript As String = "setTimeout(function(){ Swal.fire({ icon: 'warning', title: 'Cabang Silinder Tidak Boleh Kosong', showConfirmButton: false, showCancelButton: false, timer: 2000, timerProgressBar: true }); }, 100);"
                ScriptManager.RegisterStartupScript(Me, Me.[GetType](), "Popup", successScript, True)
                Exit Sub
            ElseIf String.IsNullOrEmpty(ddlunit.Text.Trim()) Then
                Dim successScript As String = "setTimeout(function(){ Swal.fire({ icon: 'warning', title: 'Unit Tidak Boleh Kosong', showConfirmButton: false, showCancelButton: false, timer: 2000, timerProgressBar: true }); }, 100);"
                ScriptManager.RegisterStartupScript(Me, Me.[GetType](), "Popup", successScript, True)
                Exit Sub
            ElseIf String.IsNullOrEmpty(dtglangsuran.Value.Trim()) Then
                Dim successScript As String = "setTimeout(function(){ Swal.fire({ icon: 'warning', title: 'Tanggal Tidak Boleh Kosong', showConfirmButton: false, showCancelButton: false, timer: 2000, timerProgressBar: true }); }, 100);"
                ScriptManager.RegisterStartupScript(Me, Me.[GetType](), "Popup", successScript, True)
                Exit Sub
            ElseIf String.IsNullOrEmpty(txtjumlah.Text.Trim()) Then
                Dim successScript As String = "setTimeout(function(){ Swal.fire({ icon: 'warning', title: 'DP Tidak Boleh Kosong', showConfirmButton: false, showCancelButton: false, timer: 2000, timerProgressBar: true }); }, 100);"
                ScriptManager.RegisterStartupScript(Me, Me.[GetType](), "Popup", successScript, True)
                Exit Sub
            ElseIf String.IsNullOrEmpty(txtjumlah2.Text.Trim()) Then
                Dim successScript As String = "setTimeout(function(){ Swal.fire({ icon: 'warning', title: 'Generate Ulang', showConfirmButton: false, showCancelButton: false, timer: 2000, timerProgressBar: true }); }, 100);"
                ScriptManager.RegisterStartupScript(Me, Me.[GetType](), "Popup", successScript, True)
                Exit Sub
            ElseIf txtsisasaldo.Text = kosong Then
                Dim successScript As String = "setTimeout(function(){ Swal.fire({ icon: 'warning', title: 'Pinjaman Sudah Selesai ', showConfirmButton: false, showCancelButton: false, timer: 2000, timerProgressBar: true }); }, 100);"
                ScriptManager.RegisterStartupScript(Me, Me.[GetType](), "Popup", successScript, True)
                Exit Sub
            ElseIf String.IsNullOrEmpty(dtgltempo.Value.Trim()) Then
                Dim successScript As String = "setTimeout(function(){ Swal.fire({ icon: 'warning', title: 'Tanggal Jatuh Tempo Tidak Boleh Kosong', showConfirmButton: false, showCancelButton: false, timer: 2000, timerProgressBar: true }); }, 100);"
                ScriptManager.RegisterStartupScript(Me, Me.[GetType](), "Popup", successScript, True)
                Exit Sub
            ElseIf String.IsNullOrEmpty(dtgltempokontrak.Value.Trim()) Then
                Dim successScript As String = "setTimeout(function(){ Swal.fire({ icon: 'warning', title: 'Tanggal Jatuh Kontrak Tidak Boleh Kosong', showConfirmButton: false, showCancelButton: false, timer: 2000, timerProgressBar: true }); }, 100);"
                ScriptManager.RegisterStartupScript(Me, Me.[GetType](), "Popup", successScript, True)
                Exit Sub
            End If



            Dim tglawal As Date = dtglangsuran.Value
            Dim tglakhir As Date = dtgltempo.Value
            Dim tglakhirkontrak As Date = dtgltempokontrak.Value
            Dim Ketstatusbayarangsuran As String = "Angsuran"

            Dim StatusAnsuranNasabah As String = ""
            Dim Bataslancar1 As Integer = 1
            Dim Bataslancar2 As Integer = 30
            Dim Bataskuranglancar1 As Integer = 31
            Dim Bataskuranglancar2 As Integer = 60
            Dim Batasdiragukan1 As Integer = 61
            Dim Batasdiragukan2 As Integer = 90
            Dim Batasmacet1 As Integer = 91
            Dim Batasmacet2 As Integer = 120
            Dim Batastarik1 As Integer = 121
            Dim Batastarik2 As Integer = 3000


            Dim sTrue5 As DataTable = GetDataTbl("select CONVERT(VARCHAR(10),Tanggal,103) as tglpinjam, CONVERT(VARCHAR(10),TglJatuhTempo,103) as jatuhtempo from tbljual where idtrans='" & txtnopinjaman.Text & "' And idnasabah='" & txtidnasabah.Text & "'")
            '  Dim jATUHtEMPO As Date = sTrue5.Rows(0).Item("jatuhtempo")
            Dim jATUHtEMPO As Date = clsEnt.CDateME5(dtgltempo.Value)
            Dim tglpinjaman As Date = sTrue5.Rows(0).Item("tglpinjam")

            Dim sTrue6 As DataTable = GetDataTbl("select Sum(total) as Jumlah from TblLiburSirkulasi where tanggal between '" & clsEnt.CDateME5(tglpinjaman) & "' And '" & clsEnt.CDateME5(dtglangsuran.Value) & "'")
            Dim Jumlahlibur As Integer = sTrue6.Rows(0).Item("Jumlah")


            Dim Selisihharikoontrak2 As Integer = CInt((jATUHtEMPO - tglpinjaman).TotalDays)

            Dim Selisihharikoontrak As Integer = Selisihharikoontrak2 - Jumlahlibur


            If Selisihharikoontrak >= Bataslancar1 And Selisihharikoontrak <= Bataslancar2 Then
                StatusAnsuranNasabah = "LANCAR"
            ElseIf Selisihharikoontrak >= Bataskuranglancar1 And Selisihharikoontrak <= Bataskuranglancar2 Then
                StatusAnsuranNasabah = "KURANG LANCAR"
            ElseIf Selisihharikoontrak >= Batasdiragukan1 And Selisihharikoontrak <= Batasdiragukan2 Then
                StatusAnsuranNasabah = "DI RAGUKAN"
            ElseIf Selisihharikoontrak >= Batasmacet1 And Selisihharikoontrak <= Batasmacet2 Then
                StatusAnsuranNasabah = "MACET"
            ElseIf Selisihharikoontrak >= Batastarik1 And Selisihharikoontrak <= Batastarik2 Then
                StatusAnsuranNasabah = "PENARIKAN KENDERAAN"
            Else
                StatusAnsuranNasabah = "LANCAR"
            End If


            Dim sTruemacet As DataTable = GetDataTbl("select CONVERT(VARCHAR(10),Tanggal,103) as tglpinjam, CONVERT(VARCHAR(10),TglJatuhTempo,103) as jatuhtempo, CONVERT(VARCHAR(10),TglCalonMacet,103) as Calonmacet, CONVERT(VARCHAR(10),TglMacetBaru,103) as MacetBaru, CONVERT(VARCHAR(10),TglMacetlama,103) as Macetlama from tbljual where idtrans='" & txtnopinjaman.Text & "' And idnasabah='" & txtidnasabah.Text & "'")
            '  Dim jATUHtEMPO As Date = sTrue5.Rows(0).Item("jatuhtempo")
            Dim jATUHtEMPO2 As Date = clsEnt.CDateME5(dtgltempo.Value)
            Dim tglpinjaman2 As Date = sTruemacet.Rows(0).Item("tglpinjam")
            Dim tglcalonmacet As Date = sTruemacet.Rows(0).Item("Calonmacet")
            Dim tglmacetbaru As Date = sTruemacet.Rows(0).Item("MacetBaru")
            Dim tglmacetlama As Date = sTruemacet.Rows(0).Item("Macetlama")

            Dim sTrueliburmacet As DataTable = GetDataTbl("select Sum(total) as Jumlah from TblLiburSirkulasi where tanggal between '" & clsEnt.CDateME5(tglpinjaman) & "' And '" & clsEnt.CDateME5(dtglangsuran.Value) & "'")
            Dim Jumlahlibur2 As Integer = sTrue6.Rows(0).Item("Jumlah")


            If Selisihharikoontrak >= Bataslancar1 And Selisihharikoontrak <= Bataslancar2 Then
                StatusAnsuranNasabah = "LANCAR"
            ElseIf Selisihharikoontrak >= Bataskuranglancar1 And Selisihharikoontrak <= Bataskuranglancar2 Then
                StatusAnsuranNasabah = "KURANG LANCAR"
            ElseIf Selisihharikoontrak >= Batasdiragukan1 And Selisihharikoontrak <= Batasdiragukan2 Then
                StatusAnsuranNasabah = "DI RAGUKAN"
            ElseIf Selisihharikoontrak >= Batasmacet1 And Selisihharikoontrak <= Batasmacet2 Then
                StatusAnsuranNasabah = "MACET"
            ElseIf Selisihharikoontrak >= Batastarik1 And Selisihharikoontrak <= Batastarik2 Then
                StatusAnsuranNasabah = "PENARIKAN KENDERAAN"
            Else
                StatusAnsuranNasabah = "LANCAR"
            End If


            Dim sTrue2 As DataTable = GetDataTbl("select isnull(jumlah,0) as jumlah from TblBungaDenda")
            Dim dendabunga As Decimal = sTrue2.Rows(0).Item("jumlah")

            Dim denda As Integer = 0

            Dim Batasdenda As Integer = 3
            Dim besardenda, haridenda, totaldenda, anhsurannasabah As Integer
            haridenda = txtjumlah2.Text

            Dim sTrue3 As DataTable = GetDataTbl("select jumlah as jumlah from TblTagihanNasabah where idTransJual='" & txtnopinjaman.Text & "' And idNasabah='" & txtidnasabah.Text & "'")
            Dim Jumlahangsuran As Integer = sTrue3.Rows(0).Item("jumlah")

            Dim NO As String = clsEnt.ReturnOneFieldConn("Select top(1)idtrans + 1 As nnn from TblAngsuran where idnasabah='" & txtidnasabah.Text & "' order by idtrans desc ", "", "", "nnn", "connuq")
            Dim NO2 As String = clsEnt.ReturnOneFieldConn("Select top(1)idtrans + 1 As nnn from TblAngsuran where idnasabah='" & txtidnasabah.Text & "' order by idtrans desc ", "", "", "nnn", "connuq")
            Dim pinjamanke As Integer = IIf(NO = "", "1", NO)
            '  Dim angsurankeberapa As String = NO2
            Dim Selisihhari As Integer = CInt((tglawal - tglakhir).TotalDays)
            Dim harilambat As Integer

            If Selisihhari <= Batasdenda Then
                totaldenda = 0
                Selisihhari = 0
            Else
                besardenda = haridenda * dendabunga / 100
                harilambat = besardenda * Selisihhari
                totaldenda = Round(harilambat)
                '  totaldenda = Round(besardenda * Selisihhari)
            End If


            anhsurannasabah = txtjumlah2.Text
            Dim strUID As String = Trim(Session.Item("UserAuthentication").ToString)


            'Check untuk melakukan tambah data
            If Not CheckAdaData(txtidtrans.Text.Trim(), "idTrans", "TblAngsuran") Then

                Dim sTruesaldoawal As DataTable = GetDataTbl("select isnull(Sum(Saldoawal),0) as jumlah from Tbljual where idtrans='" & txtnopinjaman.Text & "' And idNasabah='" & txtidnasabah.Text & "'")
                Dim sTruesaldo As DataTable = GetDataTbl("select isnull(Sum(jumlah),0) as jumlah from TblAngsuran where NoPinjaman='" & txtnopinjaman.Text & "' And idNasabah='" & txtidnasabah.Text & "'")
                Dim Jumlahsaldoakhir As Integer = sTruesaldo.Rows(0).Item("jumlah")
                Dim SaldoakhirAngsuran, Jumlahangsurannasabah, alljumlahsaldo As Integer
                Jumlahangsurannasabah = txtjumlah2.Text
                SaldoakhirAngsuran = Jumlahsaldoakhir + Jumlahangsurannasabah
                alljumlahsaldo = sTruesaldoawal.Rows(0).Item("jumlah") - SaldoakhirAngsuran

                Dim sTruesaldoawalpokok As DataTable = GetDataTbl("select isnull(Sum(total),0) as jumlah from Tbljual where idtrans='" & txtnopinjaman.Text & "' And idNasabah='" & txtidnasabah.Text & "'")
                Dim sTruesaldopokok As DataTable = GetDataTbl("select isnull(Sum(jumlahpokok),0) as jumlah from TblAngsuran where NoPinjaman='" & txtnopinjaman.Text & "' And idNasabah='" & txtidnasabah.Text & "'")
                Dim Jumlahsaldoakhirpokok As Integer = sTruesaldopokok.Rows(0).Item("jumlah")
                Dim SaldoakhirAngsuranpokok, Jumlahangsurannasabahpokok, alljumlahsaldopokok As Integer
                Jumlahangsurannasabahpokok = txtpokok.Text
                SaldoakhirAngsuranpokok = Jumlahsaldoakhirpokok + Jumlahangsurannasabahpokok
                alljumlahsaldopokok = sTruesaldoawalpokok.Rows(0).Item("jumlah") - SaldoakhirAngsuranpokok

                Dim sTruesaldoawalbunga As DataTable = GetDataTbl("select isnull(Sum(total),0) as jumlah from Tbljual where idtrans='" & txtnopinjaman.Text & "' And idNasabah='" & txtidnasabah.Text & "'")
                Dim sTruesaldobunga As DataTable = GetDataTbl("select isnull(Sum(jumlahbunga),0) as jumlah from TblAngsuran where NoPinjaman='" & txtnopinjaman.Text & "' And idNasabah='" & txtidnasabah.Text & "'")
                Dim Jumlahsaldoakhirbunga As Integer = sTruesaldobunga.Rows(0).Item("jumlah")
                Dim SaldoakhirAngsuranbunga, Jumlahangsurannasabahbunga, alljumlahsaldobunga As Integer
                Jumlahangsurannasabahbunga = txtbunga.Text
                SaldoakhirAngsuranbunga = Jumlahsaldoakhirbunga + Jumlahangsurannasabahbunga
                alljumlahsaldobunga = sTruesaldoawalbunga.Rows(0).Item("jumlah") - SaldoakhirAngsuranbunga

                Dim nojurnal As String = txtidtrans.Text + ddlunit.Text + ddlcabang.Text
                Dim Keterangan As String = "Pembayaran Angsuran Sepeda Motor No Plat" + txtplat.Text + "Dengan Jenis" + txtjenis.Text & "Atas Nama" + txtnama.Text + "Dengan Angsuran Ke" & txtangsuranke.Text
                Dim Saldo2 As Integer = 0
                Dim ketstatusangsuran As String = "LANCAR"
                Dim ketstatusangsuran2 As String = ""
                Dim ketangsurannasabah As String = "Sudah Bayar Angsuran Ke " & txtangsuranke.Text
                Dim Ketstatusbayar As String = "LUNAS"

                'DeleteJurnal(Nothing, Nothing)
                'SimpanJurnal(Nothing, Nothing)

                connectDB()
                mySqlCon.Open()
                mySqlCmd = New System.Data.OleDb.OleDbCommand()

                Dim Transaction = mySqlCon.BeginTransaction(IsolationLevel.ReadCommitted)
                mySqlCmd.Connection = mySqlCon
                mySqlCmd.Transaction = Transaction

                Try
                    mySqlCmd.CommandText = mySqlCmd.CommandText & "INSERT INTO TblAngsuran(idTrans,idSales,idSurvey,Wipem,IdNasabah,NoPinjaman,IdCabang,Idunit,NoPlat,Merk,Jenis,Tanggal,TglJatuhTempo,Angsuranke,JumlahPokok,JumlahBunga,Jumlah,denda,HariTerlambat,StatusAngsuran,SaldoAkhir,SaldoAkhirPokok,SaldoAkhirBunga,Keterangan, LastUpdate,UseridUpdate) VALUES ('" _
                        & txtidtrans.Text & "','" & txtsales.Text & "','" & txtsurvey.Text & "','" & txtwipem.Text & "','" & txtidnasabah.Text & "','" & txtnopinjaman.Text & "','" & ddlcabang.Text & "','" & ddlunit.Text & "','" & txtplat.Text & "','" & txtmerk.Text & "','" & txtjenis.Text & "','" _
                        & clsEnt.CDateME5(dtglangsuran.Value) & "','" & clsEnt.CDateME5(dtgltempo.Value) & "'," & txtangsuranke.Text & "," & txtpokok.Text & "," & txtbunga.Text & "," & Jumlahangsuran & "," & totaldenda & "," & Selisihhari & ",'" & StatusAnsuranNasabah & "'," & alljumlahsaldo & "," & alljumlahsaldopokok & "," & alljumlahsaldobunga & ",'" & ketangsurannasabah & "',Getdate(),'" & strUID & "');" _
                        & "INSERT INTO TblJurnalNasabah(NoJurnal,NoPinjaman,IdCabang,IdUnit,IdNasabah,tanggal,Keterangan,Debet,Kredit,SaldoAkhir,SaldoAkhirPokok,LastUpdate,UseridUpdate) VALUES ('" _
                        & nojurnal & "','" & txtnopinjaman.Text & "','" & ddlcabang.Text & "','" & ddlunit.Text & "','" & txtidnasabah.Text & "','" & clsEnt.CDateME5(dtglangsuran.Value) & "','" & Keterangan & "'," & Saldo2 & "," & Jumlahangsurannasabah & "," & alljumlahsaldo & "," & alljumlahsaldopokok & ", Getdate(),'" & strUID & "');" _
                        & "UPDATE TblTagihanNasabah SET StatusAngsuranBayar='" & Ketstatusbayarangsuran & "', RealisasiAngsuran=" & Jumlahangsuran & ",StatusAngsuran='" & ketstatusangsuran & "',tglAngsuran='" & clsEnt.CDateME5(dtglangsuran.Value) & "',StatusPembayaran='" & Ketstatusbayar & "', LastUpdate=Getdate(),UseridUpdate='" & strUID & "' WHERE idTransJual= '" & txtnopinjaman.Text & "' and idNasabah='" & txtidnasabah.Text & "' And TglTagihan='" & clsEnt.CDateME5(dtgltempo.Value) & "' And AngsuranKe=" & txtangsuranke.Text & ";" _
                        & "UPDATE TblTagihanNasabah SET StatusAngsuran='" & ketstatusangsuran2 & "', LastUpdate=Getdate(),UseridUpdate='" & strUID & "' WHERE idTransJual= '" & txtnopinjaman.Text & "' and idNasabah='" & txtidnasabah.Text & "' And TglTagihan>'" & clsEnt.CDateME5(dtglangsuran.Value) & "';"




                    mySqlCmd.ExecuteNonQuery()

                    Transaction.Commit()
                    Session("Mode") = "Edit"
                    Dim successScript As String = "setTimeout(Function(){ Swal.fire({ icon: 'success', title: 'Saved', showConfirmButton: false, showCancelButton: false, timer: 2000, timerProgressBar: true }); }, 100);"
                    ScriptManager.RegisterStartupScript(Me, Me.[GetType](), "Popup", successScript, True)
                    UpdateStatusAngsuran(Nothing, Nothing)
                Catch ex As Exception
                    Transaction.Rollback()
                    Dim successScript As String = "setTimeout(function(){ Swal.fire({ icon: 'error', title: 'Error saving data', showConfirmButton: false, showCancelButton: false, timer: 2000, timerProgressBar: true }); }, 100);"
                    ScriptManager.RegisterStartupScript(Me, Me.[GetType](), "Popup", successScript, True)

                Finally
                    mySqlCon.Close()
                    mySqlCon.Dispose()
                    mySqlCmd.Parameters.Clear()
                    Response.Redirect("AngsuranNasabahList.aspx")
                End Try
            Else
                Dim sTruesaldoawal As DataTable = GetDataTbl("select isnull(Sum(Saldoawal),0) as jumlah from Tbljual where idtrans='" & txtnopinjaman.Text & "' And idNasabah='" & txtidnasabah.Text & "'")
                Dim sTruesaldo As DataTable = GetDataTbl("select isnull(Sum(jumlah),0) as jumlah from TblAngsuran where NoPinjaman='" & txtnopinjaman.Text & "' And idNasabah='" & txtidnasabah.Text & "' and tanggal<'" & clsEnt.CDateME5(dtglangsuran.Value) & "'")
                Dim Jumlahsaldoakhir As Integer = sTruesaldo.Rows(0).Item("jumlah")
                Dim SaldoakhirAngsuran, Jumlahangsurannasabah, alljumlahsaldo As Integer
                Jumlahangsurannasabah = txtjumlah2.Text
                SaldoakhirAngsuran = Jumlahsaldoakhir - Jumlahangsurannasabah
                alljumlahsaldo = sTruesaldoawal.Rows(0).Item("jumlah") - SaldoakhirAngsuran

                Dim sTruesaldoawalpokok As DataTable = GetDataTbl("select isnull(Sum(total),0) as jumlah from Tbljual where idtrans='" & txtnopinjaman.Text & "' And idNasabah='" & txtidnasabah.Text & "'")
                Dim sTruesaldopokok As DataTable = GetDataTbl("select isnull(Sum(jumlahpokok),0) as jumlah from TblAngsuran where NoPinjaman='" & txtnopinjaman.Text & "' And idNasabah='" & txtidnasabah.Text & "' and tanggal<'" & clsEnt.CDateME5(dtglangsuran.Value) & "'")
                Dim Jumlahsaldoakhirpokok As Integer = sTruesaldopokok.Rows(0).Item("jumlah")
                Dim SaldoakhirAngsuranpokok, Jumlahangsurannasabahpokok, alljumlahsaldopokok As Integer
                Jumlahangsurannasabahpokok = txtpokok.Text
                SaldoakhirAngsuranpokok = Jumlahsaldoakhirpokok - Jumlahangsurannasabahpokok
                alljumlahsaldopokok = sTruesaldoawalpokok.Rows(0).Item("jumlah") - SaldoakhirAngsuranpokok

                Dim sTruesaldoawalbunga As DataTable = GetDataTbl("select isnull(Sum(total),0) as jumlah from Tbljual where idtrans='" & txtnopinjaman.Text & "' And idNasabah='" & txtidnasabah.Text & "'")
                Dim sTruesaldobunga As DataTable = GetDataTbl("select isnull(Sum(jumlahbunga),0) as jumlah from TblAngsuran where NoPinjaman='" & txtnopinjaman.Text & "' And idNasabah='" & txtidnasabah.Text & "' and tanggal<'" & clsEnt.CDateME5(dtglangsuran.Value) & "'")
                Dim Jumlahsaldoakhirbunga As Integer = sTruesaldobunga.Rows(0).Item("jumlah")
                Dim SaldoakhirAngsuranbunga, Jumlahangsurannasabahbunga, alljumlahsaldobunga As Integer
                Jumlahangsurannasabahbunga = txtbunga.Text
                SaldoakhirAngsuranbunga = Jumlahsaldoakhirbunga - Jumlahangsurannasabahbunga
                alljumlahsaldobunga = sTruesaldoawalbunga.Rows(0).Item("jumlah") - SaldoakhirAngsuranbunga

                connectDB()
                mySqlCon.Open()
                mySqlCmd = New System.Data.OleDb.OleDbCommand()

                Dim Transaction = mySqlCon.BeginTransaction(IsolationLevel.ReadCommitted)
                mySqlCmd.Connection = mySqlCon
                mySqlCmd.Transaction = Transaction

                Try
                    mySqlCmd.CommandText = "UPDATE TblAngsuran SET SaldoAkhirBunga=" & alljumlahsaldobunga & ", SaldoAkhirPokok=" & alljumlahsaldopokok & ", saldoakhir=" & alljumlahsaldo & ", Jumlah = " & txtjumlah2.Text & ", JumlahPokok=" & txtpokok.Text & ", JumlahBunga=" & txtbunga.Text & ", denda=" & totaldenda & ",HariTerlambat=" & Selisihhari & ",StatusAngsuran='" & StatusAnsuranNasabah & "',idcabang='" & ddlcabang.Text & "',idUnit='" & ddlunit.Text & "', noplat='" & txtplat.Text & "', Jenis='" & txtjenis.Text & "',Merk='" & txtmerk.Text & "',idSales='" & txtsales.Text & "', idSurvey='" & txtsurvey.Text & "',Wipem='" & txtwipem.Text & "',IdNasabah='" & txtidnasabah.Text & "', Tanggal ='" & clsEnt.CDateME5(dtglangsuran.Value) & "',LastUpdate=Getdate(),UseridUpdate='" & strUID & "' WHERE idtrans= '" & txtidtrans.Text.Trim() & "';"
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
                    Response.Redirect("AngsuranNasabahList.aspx")
                End Try

            End If
        ElseIf rdbtunggakan.Checked = True Then
            Dim kosong As Integer = 0

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
            ElseIf String.IsNullOrEmpty(ddlcabang.Text.Trim()) Then
                Dim successScript As String = "setTimeout(function(){ Swal.fire({ icon: 'warning', title: 'Cabang Silinder Tidak Boleh Kosong', showConfirmButton: false, showCancelButton: false, timer: 2000, timerProgressBar: true }); }, 100);"
                ScriptManager.RegisterStartupScript(Me, Me.[GetType](), "Popup", successScript, True)
                Exit Sub
            ElseIf String.IsNullOrEmpty(ddlunit.Text.Trim()) Then
                Dim successScript As String = "setTimeout(function(){ Swal.fire({ icon: 'warning', title: 'Unit Tidak Boleh Kosong', showConfirmButton: false, showCancelButton: false, timer: 2000, timerProgressBar: true }); }, 100);"
                ScriptManager.RegisterStartupScript(Me, Me.[GetType](), "Popup", successScript, True)
                Exit Sub
            ElseIf String.IsNullOrEmpty(dtglangsuran.Value.Trim()) Then
                Dim successScript As String = "setTimeout(function(){ Swal.fire({ icon: 'warning', title: 'Tanggal Tidak Boleh Kosong', showConfirmButton: false, showCancelButton: false, timer: 2000, timerProgressBar: true }); }, 100);"
                ScriptManager.RegisterStartupScript(Me, Me.[GetType](), "Popup", successScript, True)
                Exit Sub
            ElseIf String.IsNullOrEmpty(txtjumlah.Text.Trim()) Then
                Dim successScript As String = "setTimeout(function(){ Swal.fire({ icon: 'warning', title: 'DP Tidak Boleh Kosong', showConfirmButton: false, showCancelButton: false, timer: 2000, timerProgressBar: true }); }, 100);"
                ScriptManager.RegisterStartupScript(Me, Me.[GetType](), "Popup", successScript, True)
                Exit Sub
            ElseIf String.IsNullOrEmpty(txtjumlah2.Text.Trim()) Then
                Dim successScript As String = "setTimeout(function(){ Swal.fire({ icon: 'warning', title: 'Generate Ulang', showConfirmButton: false, showCancelButton: false, timer: 2000, timerProgressBar: true }); }, 100);"
                ScriptManager.RegisterStartupScript(Me, Me.[GetType](), "Popup", successScript, True)
                Exit Sub
            ElseIf txtsisasaldo.Text = kosong Then
                Dim successScript As String = "setTimeout(function(){ Swal.fire({ icon: 'warning', title: 'Pinjaman Sudah Selesai ', showConfirmButton: false, showCancelButton: false, timer: 2000, timerProgressBar: true }); }, 100);"
                ScriptManager.RegisterStartupScript(Me, Me.[GetType](), "Popup", successScript, True)
                Exit Sub
            ElseIf String.IsNullOrEmpty(dtgltempo.Value.Trim()) Then
                Dim successScript As String = "setTimeout(function(){ Swal.fire({ icon: 'warning', title: 'Tanggal Jatuh Tempo Tidak Boleh Kosong', showConfirmButton: false, showCancelButton: false, timer: 2000, timerProgressBar: true }); }, 100);"
                ScriptManager.RegisterStartupScript(Me, Me.[GetType](), "Popup", successScript, True)
                Exit Sub
            ElseIf String.IsNullOrEmpty(dtgltempokontrak.Value.Trim()) Then
                Dim successScript As String = "setTimeout(function(){ Swal.fire({ icon: 'warning', title: 'Tanggal Jatuh Kontrak Tidak Boleh Kosong', showConfirmButton: false, showCancelButton: false, timer: 2000, timerProgressBar: true }); }, 100);"
                ScriptManager.RegisterStartupScript(Me, Me.[GetType](), "Popup", successScript, True)
                Exit Sub
            End If

            UpdateStatusAngsuran(Nothing, Nothing)

            Dim tglawal As Date = dtglangsuran.Value
            Dim tglakhir As Date = dtgltempo.Value
            Dim tglakhirkontrak As Date = dtgltempokontrak.Value


            Dim StatusAnsuranNasabah As String = ""
            Dim Bataslancar1 As Integer = 1
            Dim Bataslancar2 As Integer = 30
            Dim Bataskuranglancar1 As Integer = 31
            Dim Bataskuranglancar2 As Integer = 60
            Dim Batasdiragukan1 As Integer = 61
            Dim Batasdiragukan2 As Integer = 90
            Dim Batasmacet1 As Integer = 91
            Dim Batasmacet2 As Integer = 120
            Dim Batastarik1 As Integer = 121
            Dim Batastarik2 As Integer = 6000
            Dim Ketstatusbayar As String = "LUNAS"
            Dim Ketstatusbayarangsuran As String = "Tunggakan"


            Dim sTrue5 As DataTable = GetDataTbl("select CONVERT(VARCHAR(10),Tanggal,103) as tglpinjam, CONVERT(VARCHAR(10),TglJatuhTempo,103) as jatuhtempo from tbljual where idtrans='" & txtnopinjaman.Text & "' And idnasabah='" & txtidnasabah.Text & "'")
            '   Dim jATUHtEMPO As Date = sTrue5.Rows(0).Item("jatuhtempo")
            Dim jATUHtEMPO As Date = clsEnt.CDateME5(dtgltempo.Value)
            Dim tglpinjaman As Date = sTrue5.Rows(0).Item("tglpinjam")

            Dim sTrue6 As DataTable = GetDataTbl("select Sum(total) as Jumlah from TblLiburSirkulasi where tanggal between '" & clsEnt.CDateME5(tglpinjaman) & "' And '" & clsEnt.CDateME5(dtglangsuran.Value) & "'")
            Dim Jumlahlibur As Integer = sTrue6.Rows(0).Item("Jumlah")


            Dim Selisihharikoontrak2 As Integer = CInt((jATUHtEMPO - tglpinjaman).TotalDays)

            Dim Selisihharikoontrak As Integer = Selisihharikoontrak2 - Jumlahlibur


            If Selisihharikoontrak >= Bataslancar1 And Selisihharikoontrak <= Bataslancar2 Then
                StatusAnsuranNasabah = "LANCAR"
            ElseIf Selisihharikoontrak >= Bataskuranglancar1 And Selisihharikoontrak <= Bataskuranglancar2 Then
                StatusAnsuranNasabah = "KURANG LANCAR"
            ElseIf Selisihharikoontrak >= Batasdiragukan1 And Selisihharikoontrak <= Batasdiragukan2 Then
                StatusAnsuranNasabah = "DI RAGUKAN"
            ElseIf Selisihharikoontrak >= Batasmacet1 And Selisihharikoontrak <= Batasmacet2 Then
                StatusAnsuranNasabah = "MACET"
            ElseIf Selisihharikoontrak >= Batastarik1 And Selisihharikoontrak <= Batastarik2 Then
                StatusAnsuranNasabah = "PENARIKAN KENDERAAN"
            Else
                StatusAnsuranNasabah = "LANCAR"
            End If


            Dim sTrue2 As DataTable = GetDataTbl("select isnull(jumlah,0) as jumlah from TblBungaDenda")
            Dim dendabunga As Decimal = sTrue2.Rows(0).Item("jumlah")

            Dim denda As Integer = 0

            Dim Batasdenda As Integer = 3
            Dim besardenda, haridenda, totaldenda, anhsurannasabah As Decimal
            haridenda = txtjumlah2.Text

            Dim sTrue3 As DataTable = GetDataTbl("select jumlah as jumlah from TblTagihanNasabah where idTransJual='" & txtnopinjaman.Text & "' And idNasabah='" & txtidnasabah.Text & "'")
            Dim Jumlahangsuran As Integer = sTrue3.Rows(0).Item("jumlah")

            Dim NO As String = clsEnt.ReturnOneFieldConn("Select top(1)idtrans + 1 As nnn from TblAngsuran where idnasabah='" & txtidnasabah.Text & "' order by idtrans desc ", "", "", "nnn", "connuq")
            Dim pinjamanke As Integer = IIf(NO = "", "1", NO)
            Dim Selisihhari As Integer = CInt((tglawal - tglakhir).TotalDays)
            Dim harilambat As Integer

            If Selisihhari <= Batasdenda Then
                totaldenda = 0
                Selisihhari = 0
            Else
                '    besardenda = haridenda * dendabunga / 100
                besardenda = Jumlahangsuran * dendabunga / 100
                harilambat = besardenda * Selisihhari
                totaldenda = Round(harilambat)
                '  totaldenda = Round(besardenda * Selisihhari)
            End If


            anhsurannasabah = txtjumlah2.Text
            Dim strUID As String = Trim(Session.Item("UserAuthentication").ToString)
            Dim kosongreal As Integer = 0

            'Check untuk melakukan tambah data
            If Not CheckAdaData(txtidtrans.Text.Trim(), "idTrans", "TblAngsuran") Then

                Dim sTruesaldoawal As DataTable = GetDataTbl("select isnull(Sum(Saldoawal),0) as jumlah from Tbljual where idtrans='" & txtnopinjaman.Text & "' And idNasabah='" & txtidnasabah.Text & "'")
                Dim sTruesaldo As DataTable = GetDataTbl("select isnull(Sum(jumlah),0) as jumlah from TblAngsuran where NoPinjaman='" & txtnopinjaman.Text & "' And idNasabah='" & txtidnasabah.Text & "'")
                Dim Jumlahsaldoakhir As Integer = sTruesaldo.Rows(0).Item("jumlah")
                Dim SaldoakhirAngsuran, Jumlahangsurannasabah, alljumlahsaldo As Integer
                Jumlahangsurannasabah = txtjumlah2.Text
                SaldoakhirAngsuran = Jumlahsaldoakhir + Jumlahangsurannasabah
                alljumlahsaldo = sTruesaldoawal.Rows(0).Item("jumlah") - SaldoakhirAngsuran

                Dim sTruesaldoawalpokok As DataTable = GetDataTbl("select isnull(Sum(total),0) as jumlah from Tbljual where idtrans='" & txtnopinjaman.Text & "' And idNasabah='" & txtidnasabah.Text & "'")
                Dim sTruesaldopokok As DataTable = GetDataTbl("select isnull(Sum(jumlahpokok),0) as jumlah from TblAngsuran where NoPinjaman='" & txtnopinjaman.Text & "' And idNasabah='" & txtidnasabah.Text & "'")
                Dim Jumlahsaldoakhirpokok As Integer = sTruesaldopokok.Rows(0).Item("jumlah")
                Dim SaldoakhirAngsuranpokok, Jumlahangsurannasabahpokok, alljumlahsaldopokok As Integer
                Jumlahangsurannasabahpokok = txtpokok.Text
                SaldoakhirAngsuranpokok = Jumlahsaldoakhirpokok + Jumlahangsurannasabahpokok
                alljumlahsaldopokok = sTruesaldoawalpokok.Rows(0).Item("jumlah") - SaldoakhirAngsuranpokok

                Dim sTruesaldoawalbunga As DataTable = GetDataTbl("select isnull(Sum(total),0) as jumlah from Tbljual where idtrans='" & txtnopinjaman.Text & "' And idNasabah='" & txtidnasabah.Text & "'")
                Dim sTruesaldobunga As DataTable = GetDataTbl("select isnull(Sum(jumlahbunga),0) as jumlah from TblAngsuran where NoPinjaman='" & txtnopinjaman.Text & "' And idNasabah='" & txtidnasabah.Text & "'")
                Dim Jumlahsaldoakhirbunga As Integer = sTruesaldobunga.Rows(0).Item("jumlah")
                Dim SaldoakhirAngsuranbunga, Jumlahangsurannasabahbunga, alljumlahsaldobunga As Integer
                Jumlahangsurannasabahbunga = txtbunga.Text
                SaldoakhirAngsuranbunga = Jumlahsaldoakhirbunga + Jumlahangsurannasabahbunga
                alljumlahsaldobunga = sTruesaldoawalbunga.Rows(0).Item("jumlah") - SaldoakhirAngsuranbunga

                Dim sTrueJumlahangsuran2 As DataTable = GetDataTbl("select sum(Jumlah) as Angsuran from TblTagihanNasabah where idtransjual='" & txtnopinjaman.Text & "' and idnasabah='" & txtidnasabah.Text & "' and TglTagihan<'" & clsEnt.CDateME5(dtglangsuran.Value) & "' and RealisasiAngsuran=" & kosongreal & "")
                Dim jumlahangsurantot As Integer = sTrueJumlahangsuran2.Rows(0).Item("Angsuran")

                Dim nojurnal As String = txtidtrans.Text + ddlunit.Text + ddlcabang.Text
                Dim Keterangan As String = "Pembayaran Angsuran Sepeda Motor No Plat" + txtplat.Text + "Dengan Jenis" + txtjenis.Text & "Atas Nama" + txtnama.Text + "Dengan Angsuran Ke" & txtangsuranke.Text
                Dim Saldo2 As Integer = 0
                Dim ketstatusangsuran As String = "LANCAR"
                Dim sTrueangsuranke As DataTable = GetDataTbl("select angsuran as jumlah from Tbljual where idtrans='" & txtnopinjaman.Text & "' And idNasabah='" & txtidnasabah.Text & "'")
                Dim jumlahangsuranke2 As Integer = 0
                Dim angsuran2 As Integer = 0
                Dim totalangsuran2 As Integer = 0
                jumlahangsuranke2 = txtjumlah2.Text
                angsuran2 = sTrueangsuranke.Rows(0).Item("jumlah")
                totalangsuran2 = jumlahangsuranke2 / angsuran2
                'DeleteJurnal(Nothing, Nothing)
                'SimpanJurnal(Nothing, Nothing)
                Dim NO2 As String = clsEnt.ReturnOneFieldConn("Select top(1)idtrans + 1 As nnn from TblAngsuran where nopinjaman='" & txtnopinjaman.Text & "' And idnasabah='" & txtidnasabah.Text & "' order by idtrans desc ", "", "", "nnn", "connuq")
                Dim pinjamanke2 As Integer = IIf(NO = "", "1", NO)
                Dim angsurankeberapa As Integer = pinjamanke2 + totalangsuran2 - 1
                Dim ketstatusangsuran2 As String = ""
                Dim ketangsurannasabah As String = "Sudah Bayar Angsuran Ke " & txtangsuranke.Text
                Dim Ketstatusbayar2 As String = "LUNAS"

                connectDB()
                mySqlCon.Open()
                mySqlCmd = New System.Data.OleDb.OleDbCommand()

                Dim Transaction = mySqlCon.BeginTransaction(IsolationLevel.ReadCommitted)
                mySqlCmd.Connection = mySqlCon
                mySqlCmd.Transaction = Transaction

                Try

                    mySqlCmd.CommandText = mySqlCmd.CommandText & "INSERT INTO TblAngsuran(idTrans,idSales,idSurvey,Wipem,IdNasabah,NoPinjaman,IdCabang,Idunit,NoPlat,Merk,Jenis,Tanggal,TglJatuhTempo,Angsuranke,JumlahPokok,JumlahBunga,Jumlah,denda,HariTerlambat,StatusAngsuran,SaldoAkhir,SaldoAkhirPokok,SaldoAkhirBunga,Keterangan,LastUpdate,UseridUpdate) VALUES ('" _
                        & txtidtrans.Text & "','" & txtsales.Text & "','" & txtsurvey.Text & "','" & txtwipem.Text & "','" & txtidnasabah.Text & "','" & txtnopinjaman.Text & "','" & ddlcabang.Text & "','" & ddlunit.Text & "','" & txtplat.Text & "','" & txtmerk.Text & "','" & txtjenis.Text & "','" _
                        & clsEnt.CDateME5(dtglangsuran.Value) & "','" & clsEnt.CDateME5(dtgltempo.Value) & "'," & angsurankeberapa & "," & txtpokok.Text & "," & txtbunga.Text & "," & Jumlahangsurannasabah & "," & totaldenda & "," & Selisihhari & ",'" & StatusAnsuranNasabah & "'," & alljumlahsaldo & "," & alljumlahsaldopokok & "," & alljumlahsaldobunga & ",'" & ketangsurannasabah & "',Getdate(),'" & strUID & "');" _
                        & "INSERT INTO TblJurnalNasabah(NoJurnal,NoPinjaman,IdCabang,IdUnit,IdNasabah,tanggal,Keterangan,Debet,Kredit,SaldoAkhir,SaldoAkhirPokok,LastUpdate,UseridUpdate) VALUES ('" _
                        & nojurnal & "','" & txtnopinjaman.Text & "','" & ddlcabang.Text & "','" & ddlunit.Text & "','" & txtidnasabah.Text & "','" & clsEnt.CDateME5(dtglangsuran.Value) & "','" & Keterangan & "'," & Saldo2 & "," & Jumlahangsurannasabah & "," & alljumlahsaldo & "," & alljumlahsaldopokok & ", Getdate(),'" & strUID & "');" _
                        & "UPDATE TblTagihanNasabah SET StatusAngsuranBayar='" & Ketstatusbayarangsuran & "',RealisasiAngsuran=" & jumlahangsurantot & ",StatusAngsuran='" & ketstatusangsuran & "',StatusPembayaran='" & Ketstatusbayar2 & "', LastUpdate=Getdate(),UseridUpdate='" & strUID & "',tglangsuran='" & clsEnt.CDateME5(dtglangsuran.Value) & "' WHERE idTransJual= '" & txtnopinjaman.Text & "' and idNasabah='" & txtidnasabah.Text & "' And TglTagihan='" & clsEnt.CDateME5(dtgltempo.Value) & "' And AngsuranKe=" & txtangsuranke.Text & ";" _
                        & "UPDATE TblTagihanNasabah SET StatusPembayaran='" & Ketstatusbayar2 & "', LastUpdate=Getdate(),UseridUpdate='" & strUID & "' WHERE idTransJual= '" & txtnopinjaman.Text & "' and idNasabah='" & txtidnasabah.Text & "' And TglTagihan<='" & clsEnt.CDateME5(dtglangsuran.Value) & "';"


                    mySqlCmd.ExecuteNonQuery()

                    Transaction.Commit()
                    Session("Mode") = "Edit"
                    Dim successScript As String = "setTimeout(Function(){ Swal.fire({ icon: 'success', title: 'Saved', showConfirmButton: false, showCancelButton: false, timer: 2000, timerProgressBar: true }); }, 100);"
                    ScriptManager.RegisterStartupScript(Me, Me.[GetType](), "Popup", successScript, True)
                    UpdateStatusAngsuran(Nothing, Nothing)
                Catch ex As Exception
                    Transaction.Rollback()
                    Dim successScript As String = "setTimeout(function(){ Swal.fire({ icon: 'error', title: 'Error saving data', showConfirmButton: false, showCancelButton: false, timer: 2000, timerProgressBar: true }); }, 100);"
                    ScriptManager.RegisterStartupScript(Me, Me.[GetType](), "Popup", successScript, True)

                Finally
                    mySqlCon.Close()
                    mySqlCon.Dispose()
                    mySqlCmd.Parameters.Clear()
                    Response.Redirect("AngsuranNasabahList.aspx")
                End Try
            Else
                Dim sTruesaldoawal As DataTable = GetDataTbl("select isnull(Sum(Saldoawal),0) as jumlah from Tbljual where idtrans='" & txtnopinjaman.Text & "' And idNasabah='" & txtidnasabah.Text & "'")
                Dim sTruesaldo As DataTable = GetDataTbl("select isnull(Sum(jumlah),0) as jumlah from TblAngsuran where NoPinjaman='" & txtnopinjaman.Text & "' And idNasabah='" & txtidnasabah.Text & "' and tanggal<'" & clsEnt.CDateME5(dtglangsuran.Value) & "'")
                Dim Jumlahsaldoakhir As Integer = sTruesaldo.Rows(0).Item("jumlah")
                Dim SaldoakhirAngsuran, Jumlahangsurannasabah, alljumlahsaldo As Integer
                Jumlahangsurannasabah = txtjumlah2.Text
                SaldoakhirAngsuran = Jumlahsaldoakhir - Jumlahangsurannasabah
                alljumlahsaldo = sTruesaldoawal.Rows(0).Item("jumlah") - SaldoakhirAngsuran

                Dim sTruesaldoawalpokok As DataTable = GetDataTbl("select isnull(Sum(total),0) as jumlah from Tbljual where idtrans='" & txtnopinjaman.Text & "' And idNasabah='" & txtidnasabah.Text & "'")
                Dim sTruesaldopokok As DataTable = GetDataTbl("select isnull(Sum(jumlahpokok),0) as jumlah from TblAngsuran where NoPinjaman='" & txtnopinjaman.Text & "' And idNasabah='" & txtidnasabah.Text & "' and tanggal<'" & clsEnt.CDateME5(dtglangsuran.Value) & "'")
                Dim Jumlahsaldoakhirpokok As Integer = sTruesaldopokok.Rows(0).Item("jumlah")
                Dim SaldoakhirAngsuranpokok, Jumlahangsurannasabahpokok, alljumlahsaldopokok As Integer
                Jumlahangsurannasabahpokok = txtpokok.Text
                SaldoakhirAngsuranpokok = Jumlahsaldoakhirpokok - Jumlahangsurannasabahpokok
                alljumlahsaldopokok = sTruesaldoawalpokok.Rows(0).Item("jumlah") - SaldoakhirAngsuranpokok

                Dim sTruesaldoawalbunga As DataTable = GetDataTbl("select isnull(Sum(total),0) as jumlah from Tbljual where idtrans='" & txtnopinjaman.Text & "' And idNasabah='" & txtidnasabah.Text & "'")
                Dim sTruesaldobunga As DataTable = GetDataTbl("select isnull(Sum(jumlahbunga),0) as jumlah from TblAngsuran where NoPinjaman='" & txtnopinjaman.Text & "' And idNasabah='" & txtidnasabah.Text & "' and tanggal<'" & clsEnt.CDateME5(dtglangsuran.Value) & "'")
                Dim Jumlahsaldoakhirbunga As Integer = sTruesaldobunga.Rows(0).Item("jumlah")
                Dim SaldoakhirAngsuranbunga, Jumlahangsurannasabahbunga, alljumlahsaldobunga As Integer
                Jumlahangsurannasabahbunga = txtbunga.Text
                SaldoakhirAngsuranbunga = Jumlahsaldoakhirbunga - Jumlahangsurannasabahbunga
                alljumlahsaldobunga = sTruesaldoawalbunga.Rows(0).Item("jumlah") - SaldoakhirAngsuranbunga

                connectDB()
                mySqlCon.Open()
                mySqlCmd = New System.Data.OleDb.OleDbCommand()

                Dim Transaction = mySqlCon.BeginTransaction(IsolationLevel.ReadCommitted)
                mySqlCmd.Connection = mySqlCon
                mySqlCmd.Transaction = Transaction

                Try
                    mySqlCmd.CommandText = "UPDATE TblAngsuran SET SaldoAkhirBunga=" & alljumlahsaldobunga & ", SaldoAkhirPokok=" & alljumlahsaldopokok & ", saldoakhir=" & alljumlahsaldo & ", Jumlah = " & txtjumlah2.Text & ", JumlahPokok=" & txtpokok.Text & ", JumlahBunga=" & txtbunga.Text & ", denda=" & totaldenda & ",HariTerlambat=" & Selisihhari & ",StatusAngsuran='" & StatusAnsuranNasabah & "',idcabang='" & ddlcabang.Text & "',idUnit='" & ddlunit.Text & "', noplat='" & txtplat.Text & "', Jenis='" & txtjenis.Text & "',Merk='" & txtmerk.Text & "',idSales='" & txtsales.Text & "', idSurvey='" & txtsurvey.Text & "',Wipem='" & txtwipem.Text & "',IdNasabah='" & txtidnasabah.Text & "', Tanggal ='" & clsEnt.CDateME5(dtglangsuran.Value) & "',LastUpdate=Getdate(),UseridUpdate='" & strUID & "' WHERE idtrans= '" & txtidtrans.Text.Trim() & "';"
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
                    Response.Redirect("AngsuranNasabahList.aspx")
                End Try
            End If
        ElseIf rdbpelunasan.Checked = True Then
            Dim kosong As Integer = 0

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
            ElseIf String.IsNullOrEmpty(ddlcabang.Text.Trim()) Then
                Dim successScript As String = "setTimeout(function(){ Swal.fire({ icon: 'warning', title: 'Cabang Silinder Tidak Boleh Kosong', showConfirmButton: false, showCancelButton: false, timer: 2000, timerProgressBar: true }); }, 100);"
                ScriptManager.RegisterStartupScript(Me, Me.[GetType](), "Popup", successScript, True)
                Exit Sub
            ElseIf String.IsNullOrEmpty(ddlunit.Text.Trim()) Then
                Dim successScript As String = "setTimeout(function(){ Swal.fire({ icon: 'warning', title: 'Unit Tidak Boleh Kosong', showConfirmButton: false, showCancelButton: false, timer: 2000, timerProgressBar: true }); }, 100);"
                ScriptManager.RegisterStartupScript(Me, Me.[GetType](), "Popup", successScript, True)
                Exit Sub
            ElseIf String.IsNullOrEmpty(dtglangsuran.Value.Trim()) Then
                Dim successScript As String = "setTimeout(function(){ Swal.fire({ icon: 'warning', title: 'Tanggal Tidak Boleh Kosong', showConfirmButton: false, showCancelButton: false, timer: 2000, timerProgressBar: true }); }, 100);"
                ScriptManager.RegisterStartupScript(Me, Me.[GetType](), "Popup", successScript, True)
                Exit Sub
            ElseIf String.IsNullOrEmpty(txtjumlah.Text.Trim()) Then
                Dim successScript As String = "setTimeout(function(){ Swal.fire({ icon: 'warning', title: 'DP Tidak Boleh Kosong', showConfirmButton: false, showCancelButton: false, timer: 2000, timerProgressBar: true }); }, 100);"
                ScriptManager.RegisterStartupScript(Me, Me.[GetType](), "Popup", successScript, True)
                Exit Sub
            ElseIf String.IsNullOrEmpty(txtjumlah2.Text.Trim()) Then
                Dim successScript As String = "setTimeout(function(){ Swal.fire({ icon: 'warning', title: 'Generate Ulang', showConfirmButton: false, showCancelButton: false, timer: 2000, timerProgressBar: true }); }, 100);"
                ScriptManager.RegisterStartupScript(Me, Me.[GetType](), "Popup", successScript, True)
                Exit Sub
            ElseIf txtsisasaldo.Text = kosong Then
                Dim successScript As String = "setTimeout(function(){ Swal.fire({ icon: 'warning', title: 'Pinjaman Sudah Selesai ', showConfirmButton: false, showCancelButton: false, timer: 2000, timerProgressBar: true }); }, 100);"
                ScriptManager.RegisterStartupScript(Me, Me.[GetType](), "Popup", successScript, True)
                Exit Sub
            ElseIf String.IsNullOrEmpty(dtgltempo.Value.Trim()) Then
                Dim successScript As String = "setTimeout(function(){ Swal.fire({ icon: 'warning', title: 'Tanggal Jatuh Tempo Tidak Boleh Kosong', showConfirmButton: false, showCancelButton: false, timer: 2000, timerProgressBar: true }); }, 100);"
                ScriptManager.RegisterStartupScript(Me, Me.[GetType](), "Popup", successScript, True)
                Exit Sub
            ElseIf String.IsNullOrEmpty(dtgltempokontrak.Value.Trim()) Then
                Dim successScript As String = "setTimeout(function(){ Swal.fire({ icon: 'warning', title: 'Tanggal Jatuh Kontrak Tidak Boleh Kosong', showConfirmButton: false, showCancelButton: false, timer: 2000, timerProgressBar: true }); }, 100);"
                ScriptManager.RegisterStartupScript(Me, Me.[GetType](), "Popup", successScript, True)
                Exit Sub
            End If

            UpdateStatusAngsuran(Nothing, Nothing)

            Dim tglawal As Date = dtglangsuran.Value
            Dim tglakhir As Date = dtgltempo.Value
            Dim tglakhirkontrak As Date = dtgltempokontrak.Value


            Dim StatusAnsuranNasabah As String = ""
            Dim Bataslancar1 As Integer = 1
            Dim Bataslancar2 As Integer = 30
            Dim Bataskuranglancar1 As Integer = 31
            Dim Bataskuranglancar2 As Integer = 60
            Dim Batasdiragukan1 As Integer = 61
            Dim Batasdiragukan2 As Integer = 90
            Dim Batasmacet1 As Integer = 91
            Dim Batasmacet2 As Integer = 120
            Dim Batastarik1 As Integer = 121
            Dim Batastarik2 As Integer = 7000


            Dim sTrue5 As DataTable = GetDataTbl("select CONVERT(VARCHAR(10),Tanggal,103) as tglpinjam, CONVERT(VARCHAR(10),TglJatuhTempo,103) as jatuhtempo from tbljual where idtrans='" & txtnopinjaman.Text & "' And idnasabah='" & txtidnasabah.Text & "' and noplat='" & txtplat.Text & "'")
            '  Dim jATUHtEMPO As Date = sTrue5.Rows(0).Item("jatuhtempo")
            Dim jATUHtEMPO As Date = clsEnt.CDateME5(dtgltempo.Value)
            Dim tglpinjaman As Date = sTrue5.Rows(0).Item("tglpinjam")

            Dim sTrue6 As DataTable = GetDataTbl("select Sum(total) as Jumlah from TblLiburSirkulasi where tanggal between '" & clsEnt.CDateME5(tglpinjaman) & "' And '" & clsEnt.CDateME5(dtglangsuran.Value) & "' and noplat='" & txtplat.Text & "'")
            Dim Jumlahlibur As Integer = sTrue6.Rows(0).Item("Jumlah")


            Dim Selisihharikoontrak2 As Integer = CInt((jATUHtEMPO - tglpinjaman).TotalDays)

            Dim Selisihharikoontrak As Integer = Selisihharikoontrak2 - Jumlahlibur


            If Selisihharikoontrak >= Bataslancar1 And Selisihharikoontrak <= Bataslancar2 Then
                StatusAnsuranNasabah = "LANCAR"
            ElseIf Selisihharikoontrak >= Bataskuranglancar1 And Selisihharikoontrak <= Bataskuranglancar2 Then
                StatusAnsuranNasabah = "KURANG LANCAR"
            ElseIf Selisihharikoontrak >= Batasdiragukan1 And Selisihharikoontrak <= Batasdiragukan2 Then
                StatusAnsuranNasabah = "DI RAGUKAN"
            ElseIf Selisihharikoontrak >= Batasmacet1 And Selisihharikoontrak <= Batasmacet2 Then
                StatusAnsuranNasabah = "MACET"
            ElseIf Selisihharikoontrak >= Batastarik1 And Selisihharikoontrak <= Batastarik2 Then
                StatusAnsuranNasabah = "PENARIKAN KENDERAAN"
            Else
                StatusAnsuranNasabah = "LANCAR"
            End If


            Dim sTrue2 As DataTable = GetDataTbl("select isnull(jumlah,0) as jumlah from TblBungaDenda")
            Dim dendabunga As Decimal = sTrue2.Rows(0).Item("jumlah")

            Dim denda As Integer = 0

            Dim Batasdenda As Integer = 3
            Dim besardenda, haridenda, totaldenda, anhsurannasabah As Integer
            haridenda = txtjumlah2.Text

            Dim sTrue3 As DataTable = GetDataTbl("select jumlah as jumlah from TblTagihanNasabah where idTransJual='" & txtnopinjaman.Text & "' And idNasabah='" & txtidnasabah.Text & "' and noplat='" & txtplat.Text & "'")
            Dim Jumlahangsuran As Integer = sTrue3.Rows(0).Item("jumlah")

            Dim NO As String = clsEnt.ReturnOneFieldConn("Select top(1)idtrans + 1 As nnn from TblAngsuran where idnasabah='" & txtidnasabah.Text & "' and noplat='" & txtplat.Text & "' order by idtrans desc ", "", "", "nnn", "connuq")
            Dim pinjamanke As Integer = IIf(NO = "", "1", NO)
            Dim Selisihhari As Integer = CInt((tglawal - tglakhir).TotalDays)
            Dim harilambat As Integer

            If Selisihhari <= Batasdenda Then
                totaldenda = 0
                Selisihhari = 0
            Else
                '    besardenda = haridenda * dendabunga / 100
                besardenda = Jumlahangsuran * dendabunga / 100
                harilambat = besardenda * Selisihhari
                totaldenda = Round(harilambat)
                '  totaldenda = Round(besardenda * Selisihhari)
            End If


            anhsurannasabah = txtjumlah2.Text
            Dim strUID As String = Trim(Session.Item("UserAuthentication").ToString)
            Dim kosongreal As Integer = 0


            'Check untuk melakukan tambah data
            If Not CheckAdaData(txtidtrans.Text.Trim(), "idTrans", "TblAngsuran") Then

                Dim sTruesaldoawal As DataTable = GetDataTbl("select isnull(Sum(Saldoawal),0) as jumlah from Tbljual where idtrans='" & txtnopinjaman.Text & "' And idNasabah='" & txtidnasabah.Text & "'")
                Dim sTruesaldo As DataTable = GetDataTbl("select isnull(Sum(jumlah),0) as jumlah from TblAngsuran where NoPinjaman='" & txtnopinjaman.Text & "' And idNasabah='" & txtidnasabah.Text & "'")
                Dim Jumlahsaldoakhir As Integer = sTruesaldo.Rows(0).Item("jumlah")
                Dim SaldoakhirAngsuran, Jumlahangsurannasabah, alljumlahsaldo As Integer
                Jumlahangsurannasabah = txtjumlah2.Text
                SaldoakhirAngsuran = Jumlahsaldoakhir + Jumlahangsurannasabah
                alljumlahsaldo = sTruesaldoawal.Rows(0).Item("jumlah") - SaldoakhirAngsuran

                Dim sTruesaldoawalpokok As DataTable = GetDataTbl("select isnull(Sum(total),0) as jumlah from Tbljual where idtrans='" & txtnopinjaman.Text & "' And idNasabah='" & txtidnasabah.Text & "' and noplat='" & txtplat.Text & "'")
                Dim sTruesaldopokok As DataTable = GetDataTbl("select isnull(Sum(jumlahpokok),0) as jumlah from TblAngsuran where NoPinjaman='" & txtnopinjaman.Text & "' And idNasabah='" & txtidnasabah.Text & "' and noplat='" & txtplat.Text & "'")
                Dim Jumlahsaldoakhirpokok As Integer = sTruesaldopokok.Rows(0).Item("jumlah")
                Dim SaldoakhirAngsuranpokok, Jumlahangsurannasabahpokok, alljumlahsaldopokok As Integer
                Jumlahangsurannasabahpokok = txtpokok.Text
                SaldoakhirAngsuranpokok = Jumlahsaldoakhirpokok + Jumlahangsurannasabahpokok
                alljumlahsaldopokok = sTruesaldoawalpokok.Rows(0).Item("jumlah") - SaldoakhirAngsuranpokok

                Dim sTruesaldoawalbunga As DataTable = GetDataTbl("select isnull(Sum(total),0) as jumlah from Tbljual where idtrans='" & txtnopinjaman.Text & "' And idNasabah='" & txtidnasabah.Text & "' and noplat='" & txtplat.Text & "'")
                Dim sTruesaldobunga As DataTable = GetDataTbl("select isnull(Sum(jumlahbunga),0) as jumlah from TblAngsuran where NoPinjaman='" & txtnopinjaman.Text & "' And idNasabah='" & txtidnasabah.Text & "' and noplat='" & txtplat.Text & "'")
                Dim Jumlahsaldoakhirbunga As Integer = sTruesaldobunga.Rows(0).Item("jumlah")
                Dim SaldoakhirAngsuranbunga, Jumlahangsurannasabahbunga, alljumlahsaldobunga As Integer
                Jumlahangsurannasabahbunga = txtbunga.Text
                SaldoakhirAngsuranbunga = Jumlahsaldoakhirbunga + Jumlahangsurannasabahbunga
                alljumlahsaldobunga = sTruesaldoawalbunga.Rows(0).Item("jumlah") - SaldoakhirAngsuranbunga

                Dim sTrueJumlahangsuran2 As DataTable = GetDataTbl("select sum(Jumlah) as Angsuran from TblTagihanNasabah where idtransjual='" & txtnopinjaman.Text & "' and idnasabah='" & txtidnasabah.Text & "'  and RealisasiAngsuran=" & kosongreal & " and noplat='" & txtplat.Text & "'")
                Dim jumlahangsurantot As Integer = sTrueJumlahangsuran2.Rows(0).Item("Angsuran")

                Dim nojurnal As String = txtidtrans.Text + ddlunit.Text + ddlcabang.Text
                Dim Keterangan As String = "Pembayaran Angsuran Sepeda Motor No Plat" + txtplat.Text + "Dengan Jenis" + txtjenis.Text & "Atas Nama" + txtnama.Text + "Dengan Angsuran Ke" & txtangsuranke.Text
                Dim Saldo2 As Integer = 0
                Dim ketstatusangsuran As String = "LANCAR"
                Dim sTrueangsuranke As DataTable = GetDataTbl("select angsuran as jumlah from Tbljual where idtrans='" & txtnopinjaman.Text & "' And idNasabah='" & txtidnasabah.Text & "' and noplat='" & txtplat.Text & "'")
                'Dim jumlahangsuranke, angsuran, totalangsuran As Integer
                'jumlahangsuranke = txtjumlah.Text
                'angsuran = sTrueangsuranke.Rows(0).Item("jumlah")
                'totalangsuran = jumlahangsuranke / angsuran

                Dim jumlahangsuranke2 As Integer = 0
                Dim angsuran2 As Integer = 0
                Dim totalangsuran2 As Integer = 0
                Dim ketstatusangsuran2 As String = ""
                Dim ketangsurannasabah As String = "Sudah Bayar Angsuran Ke " & txtangsuranke.Text
                Dim Ketstatusbayar As String = "LUNAS"
                Dim Ketstatusbayarangsuran As String = "Pelunasan"

                jumlahangsuranke2 = txtjumlah2.Text
                angsuran2 = sTrueangsuranke.Rows(0).Item("jumlah")
                totalangsuran2 = jumlahangsuranke2 / angsuran2

                Dim NO2 As Integer = clsEnt.ReturnOneFieldConn("Select top(1)idtrans + 1 As nnn from TblAngsuran where idnasabah='" & txtidnasabah.Text & "' and noplat='" & txtplat.Text & "' order by idtrans desc ", "", "", "nnn", "connuq")
                Dim angsurankeberapa As Integer = NO2 + totalangsuran2 - 1
                'DeleteJurnal(Nothing, Nothing)
                'SimpanJurnal(Nothing, Nothing)

                connectDB()
                mySqlCon.Open()
                mySqlCmd = New System.Data.OleDb.OleDbCommand()

                Dim Transaction = mySqlCon.BeginTransaction(IsolationLevel.ReadCommitted)
                mySqlCmd.Connection = mySqlCon
                mySqlCmd.Transaction = Transaction

                Try
                    mySqlCmd.CommandText = mySqlCmd.CommandText & "INSERT INTO TblAngsuran(idTrans,idSales,idSurvey,Wipem,IdNasabah,NoPinjaman,IdCabang,Idunit,NoPlat,Merk,Jenis,Tanggal,TglJatuhTempo,Angsuranke,JumlahPokok,JumlahBunga,Jumlah,denda,HariTerlambat,StatusAngsuran,SaldoAkhir,SaldoAkhirPokok,SaldoAkhirBunga,Keterangan, LastUpdate,UseridUpdate) VALUES ('" _
                        & txtidtrans.Text & "','" & txtsales.Text & "','" & txtsurvey.Text & "','" & txtwipem.Text & "','" & txtidnasabah.Text & "','" & txtnopinjaman.Text & "','" & ddlcabang.Text & "','" & ddlunit.Text & "','" & txtplat.Text & "','" & txtmerk.Text & "','" & txtjenis.Text & "','" _
                        & clsEnt.CDateME5(dtglangsuran.Value) & "','" & clsEnt.CDateME5(dtgltempo.Value) & "'," & angsurankeberapa & "," & txtpokok.Text & "," & txtbunga.Text & "," & Jumlahangsurannasabah & "," & totaldenda & "," & Selisihhari & ",'" & StatusAnsuranNasabah & "'," & alljumlahsaldo & "," & alljumlahsaldopokok & "," & alljumlahsaldobunga & ",'" & ketangsurannasabah & "',Getdate(),'" & strUID & "');" _
                        & "INSERT INTO TblJurnalNasabah(NoJurnal,NoPinjaman,IdCabang,IdUnit,IdNasabah,tanggal,Keterangan,Debet,Kredit,SaldoAkhir,SaldoAkhirPokok,LastUpdate,UseridUpdate) VALUES ('" _
                        & nojurnal & "','" & txtnopinjaman.Text & "','" & ddlcabang.Text & "','" & ddlunit.Text & "','" & txtidnasabah.Text & "','" & clsEnt.CDateME5(dtglangsuran.Value) & "','" & Keterangan & "'," & Saldo2 & "," & Jumlahangsurannasabah & "," & alljumlahsaldo & "," & alljumlahsaldopokok & ", Getdate(),'" & strUID & "');" _
                        & "UPDATE TblTagihanNasabah SET StatusAngsuranBayar='" & Ketstatusbayarangsuran & "',RealisasiAngsuran=" & jumlahangsurantot & ",StatusAngsuran='" & ketstatusangsuran & "',tglAngsuran='" & clsEnt.CDateME5(dtglangsuran.Value) & "', LastUpdate=Getdate(),UseridUpdate='" & strUID & "' WHERE idTransJual= '" & txtnopinjaman.Text & "' and idNasabah='" & txtidnasabah.Text & "' And TglTagihan='" & clsEnt.CDateME5(dtgltempo.Value) & "' And AngsuranKe=" & txtangsuranke.Text & ";" _
                        & "UPDATE TblTagihanNasabah SET StatusPembayaran='" & Ketstatusbayar & "', LastUpdate=Getdate(),UseridUpdate='" & strUID & "' WHERE idTransJual= '" & txtnopinjaman.Text & "' and idNasabah='" & txtidnasabah.Text & "' And TglTagihan<='" & clsEnt.CDateME5(dtglangsuran.Value) & "';"

                    mySqlCmd.ExecuteNonQuery()

                    Transaction.Commit()
                    Session("Mode") = "Edit"
                    Dim successScript As String = "setTimeout(Function(){ Swal.fire({ icon: 'success', title: 'Saved', showConfirmButton: false, showCancelButton: false, timer: 2000, timerProgressBar: true }); }, 100);"
                    ScriptManager.RegisterStartupScript(Me, Me.[GetType](), "Popup", successScript, True)
                    UpdateStatusAngsuran(Nothing, Nothing)
                Catch ex As Exception
                    Transaction.Rollback()
                    Dim successScript As String = "setTimeout(function(){ Swal.fire({ icon: 'error', title: 'Error saving data', showConfirmButton: false, showCancelButton: false, timer: 2000, timerProgressBar: true }); }, 100);"
                    ScriptManager.RegisterStartupScript(Me, Me.[GetType](), "Popup", successScript, True)

                Finally
                    mySqlCon.Close()
                    mySqlCon.Dispose()
                    mySqlCmd.Parameters.Clear()
                    Response.Redirect("AngsuranNasabahList.aspx")
                End Try
            Else
                Dim sTruesaldoawal As DataTable = GetDataTbl("select isnull(Sum(Saldoawal),0) as jumlah from Tbljual where idtrans='" & txtnopinjaman.Text & "' And idNasabah='" & txtidnasabah.Text & "'")
                Dim sTruesaldo As DataTable = GetDataTbl("select isnull(Sum(jumlah),0) as jumlah from TblAngsuran where NoPinjaman='" & txtnopinjaman.Text & "' And idNasabah='" & txtidnasabah.Text & "' and tanggal<'" & clsEnt.CDateME5(dtglangsuran.Value) & "'")
                Dim Jumlahsaldoakhir As Integer = sTruesaldo.Rows(0).Item("jumlah")
                Dim SaldoakhirAngsuran, Jumlahangsurannasabah, alljumlahsaldo As Integer
                Jumlahangsurannasabah = txtjumlah2.Text
                SaldoakhirAngsuran = Jumlahsaldoakhir - Jumlahangsurannasabah
                alljumlahsaldo = sTruesaldoawal.Rows(0).Item("jumlah") - SaldoakhirAngsuran

                Dim sTruesaldoawalpokok As DataTable = GetDataTbl("select isnull(Sum(total),0) as jumlah from Tbljual where idtrans='" & txtnopinjaman.Text & "' And idNasabah='" & txtidnasabah.Text & "'")
                Dim sTruesaldopokok As DataTable = GetDataTbl("select isnull(Sum(jumlahpokok),0) as jumlah from TblAngsuran where NoPinjaman='" & txtnopinjaman.Text & "' And idNasabah='" & txtidnasabah.Text & "' and tanggal<'" & clsEnt.CDateME5(dtglangsuran.Value) & "'")
                Dim Jumlahsaldoakhirpokok As Integer = sTruesaldopokok.Rows(0).Item("jumlah")
                Dim SaldoakhirAngsuranpokok, Jumlahangsurannasabahpokok, alljumlahsaldopokok As Integer
                Jumlahangsurannasabahpokok = txtpokok.Text
                SaldoakhirAngsuranpokok = Jumlahsaldoakhirpokok - Jumlahangsurannasabahpokok
                alljumlahsaldopokok = sTruesaldoawalpokok.Rows(0).Item("jumlah") - SaldoakhirAngsuranpokok

                Dim sTruesaldoawalbunga As DataTable = GetDataTbl("select isnull(Sum(total),0) as jumlah from Tbljual where idtrans='" & txtnopinjaman.Text & "' And idNasabah='" & txtidnasabah.Text & "'")
                Dim sTruesaldobunga As DataTable = GetDataTbl("select isnull(Sum(jumlahbunga),0) as jumlah from TblAngsuran where NoPinjaman='" & txtnopinjaman.Text & "' And idNasabah='" & txtidnasabah.Text & "' and tanggal<'" & clsEnt.CDateME5(dtglangsuran.Value) & "'")
                Dim Jumlahsaldoakhirbunga As Integer = sTruesaldobunga.Rows(0).Item("jumlah")
                Dim SaldoakhirAngsuranbunga, Jumlahangsurannasabahbunga, alljumlahsaldobunga As Integer
                Jumlahangsurannasabahbunga = txtbunga.Text
                SaldoakhirAngsuranbunga = Jumlahsaldoakhirbunga - Jumlahangsurannasabahbunga
                alljumlahsaldobunga = sTruesaldoawalbunga.Rows(0).Item("jumlah") - SaldoakhirAngsuranbunga

                connectDB()
                mySqlCon.Open()
                mySqlCmd = New System.Data.OleDb.OleDbCommand()

                Dim Transaction = mySqlCon.BeginTransaction(IsolationLevel.ReadCommitted)
                mySqlCmd.Connection = mySqlCon
                mySqlCmd.Transaction = Transaction

                Try
                    mySqlCmd.CommandText = "UPDATE TblAngsuran SET SaldoAkhirBunga=" & alljumlahsaldobunga & ", SaldoAkhirPokok=" & alljumlahsaldopokok & ", saldoakhir=" & alljumlahsaldo & ", Jumlah = " & txtjumlah2.Text & ", JumlahPokok=" & txtpokok.Text & ", JumlahBunga=" & txtbunga.Text & ", denda=" & totaldenda & ",HariTerlambat=" & Selisihhari & ",StatusAngsuran='" & StatusAnsuranNasabah & "',idcabang='" & ddlcabang.Text & "',idUnit='" & ddlunit.Text & "', noplat='" & txtplat.Text & "', Jenis='" & txtjenis.Text & "',Merk='" & txtmerk.Text & "',idSales='" & txtsales.Text & "', idSurvey='" & txtsurvey.Text & "',Wipem='" & txtwipem.Text & "',IdNasabah='" & txtidnasabah.Text & "', Tanggal ='" & clsEnt.CDateME5(dtglangsuran.Value) & "',LastUpdate=Getdate(),UseridUpdate='" & strUID & "' WHERE idtrans= '" & txtidtrans.Text.Trim() & "';"
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
                    Response.Redirect("AngsuranNasabahList.aspx")
                End Try
            End If
        End If

    End Sub



    Protected Sub btnNew_Click(sender As Object, e As System.EventArgs) Handles btnNew.Click
        Response.Redirect("AngsuranNasabahList.aspx")
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
        dtglangsuran.Value = ""
        dtgltempo.Value = ""
        ddlcabang.Text = ""
        ddlunit.Text = ""
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
            command = New System.Data.OleDb.OleDbCommand("SELECT  isnull(a.JumlahPokok,0) as JumlahPokok, isnull(JumlahBunga,0) as JumlahBunga,  a.wipem as wipem, a.idSales as idSales, a.idSurvey as idSurvey, a.idnasabah as idnasabah,b.nama as nama, a.idTrans as idTrans, a.IdCabang as IdCabang, a.Idunit as Idunit, a.NoPlat as NoPlat, a.Merk as Merk, a.Jenis as Jenis, FORMAT(Denda,'#,###,##0')  as Denda, FORMAT(a.Jumlah,'#,###,##0')  as Angsuran, FORMAT(a.Jumlahpokok,'#,###,##0')  as Angsuranpokok, FORMAT(a.Jumlahbunga,'#,###,##0')  as Angsuranbunga, CONVERT(VARCHAR(10),a.Tanggal,103) as Tanggal, CONVERT(VARCHAR(10),a.TglJatuhTempo,103) as TglJatuhTempo, a.Angsuranke as Angsuranke, a.HariTerlambat as HariTerlambat, NoPinjaman as NoPinjaman, Jumlah  as Jumlah2 FROM TblAngsuran a inner join TblNasabah b on b.idnasabah=a.idnasabah WHERE idtrans= '" & a_Str & "'; ", mySqlCon)
            mySqlReader = command.ExecuteReader
            If mySqlReader.Read Then
                txtidtrans.Text = mySqlReader("idtrans").ToString()
                txtidnasabah.Text = mySqlReader("idnasabah").ToString()
                txtnama.Text = mySqlReader("nama").ToString()
                txtplat.Text = mySqlReader("NoPlat").ToString()
                txtmerk.Text = mySqlReader("Merk").ToString()
                txtjenis.Text = mySqlReader("Jenis").ToString()
                dtglangsuran.Value = mySqlReader("tanggal").ToString()
                dtgltempo.Value = mySqlReader("TglJatuhTempo").ToString()
                ddlcabang.Text = mySqlReader("IdCabang").ToString()
                ddlunit.Text = mySqlReader("Idunit").ToString()
                txtsales.Text = mySqlReader("idSales").ToString()
                txtsurvey.Text = mySqlReader("idSurvey").ToString()
                txtnopinjaman.Text = mySqlReader("nopinjaman").ToString()
                txtangsuranke.Text = mySqlReader("Angsuranke").ToString()
                txtjumlah.Text = mySqlReader("Angsuran").ToString()
                txtwipem.Text = mySqlReader("Wipem").ToString()
                txtjumlah2.Text = mySqlReader("Jumlah2").ToString()
                txtpokok.Text = mySqlReader("JumlahPokok").ToString()
                txtbunga.Text = mySqlReader("JumlahBunga").ToString()
                txtpokok2.Text = mySqlReader("JumlahPokok").ToString()
                txtbunga2.Text = mySqlReader("JumlahBunga").ToString()

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
                dtglangsuran.Value = ""
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
        'Dim sTrue As DataTable = GetDataTbl("select Max(idtrans) + 1 as total from TblAngsuran")
        'Dim nourut As String = CInt(sTrue.Rows(0).Item("total"))
        Dim NO As String = clsEnt.ReturnOneFieldConn("select top(1)idtrans + 1 as nnn from TblAngsuran order by idtrans desc ", "", "", "nnn", "connuq")
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
            mySqlCmd.CommandText = "DELETE FROM TblAngsuran WHERE idtrans = '" & txtidtrans.Text.Trim() & "'; "
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
            Response.Redirect("AngsuranNasabahList.aspx")
        End Try
    End Sub

    Public Sub kosong()
        txtbunga2.Text = 0
        txtpokok2.Text = 0
        txtjumlah.Text = 0
        txtangsuranke.Text = 0
        txtsisasaldo.Text = 0
    End Sub

    Protected Sub Lngenerate_Click(sender As Object, e As EventArgs) Handles Lngenerate.Click
        If String.IsNullOrEmpty(dtglangsuran.Value.Trim()) Then
            Dim successScript As String = "setTimeout(function(){ Swal.fire({ icon: 'warning', title: 'Tanggal Angsuran Tidak Boleh Kosong', showConfirmButton: false, showCancelButton: false, timer: 2000, timerProgressBar: true }); }, 100);"
            ScriptManager.RegisterStartupScript(Me, Me.[GetType](), "Popup", successScript, True)
            Exit Sub

        End If

        If rdBangsuran.Checked = True Then
            Dim sisa2 As Integer
            Dim sTrue6 As DataTable = GetDataTbl("select isnull(Sum(saldoawal),0)  as Jumlah from Tbljual where idtrans='" & txtnopinjaman.Text & "' and idnasabah='" & txtidnasabah.Text & "'")
            Dim sTrue7 As DataTable = GetDataTbl("select isnull(Sum(jumlah),0) as Jumlah from Tblangsuran where nopinjaman='" & txtnopinjaman.Text & "' and idnasabah='" & txtidnasabah.Text & "'")
            sisa2 = sTrue6.Rows(0).Item("jumlah") - sTrue7.Rows(0).Item("jumlah")
            If sisa2 = 0 Then
                Dim successScript As String = "setTimeout(function(){ Swal.fire({ icon: 'warning', title: 'Pinjaman Sudah Selesai ', showConfirmButton: false, showCancelButton: false, timer: 2000, timerProgressBar: true }); }, 100);"
                ScriptManager.RegisterStartupScript(Me, Me.[GetType](), "Popup", successScript, True)
                kosong()
                Exit Sub
            Else
                Dim sisa As Integer
                Dim NO As String = clsEnt.ReturnOneFieldConn("select top(1)angsuranke + 1 as nnn from TblAngsuran where nopinjaman='" & txtnopinjaman.Text & "' and idnasabah='" & txtidnasabah.Text & "'  order by idtrans desc ", "", "", "nnn", "connuq")
                txtangsuranke.Text = IIf(NO = "", "1", NO)
                Dim sTrueMk As DataTable = GetDataTbl("select  count(noplat) as cnt from TblTagihanNasabah where idtransjual='" & txtnopinjaman.Text & "' and idnasabah='" & txtidnasabah.Text & "' and Angsuranke=" & txtangsuranke.Text & "")
                If CInt(sTrueMk.Rows(0).Item("cnt")) > 0 Then

                    Dim sTrue2 As DataTable = GetDataTbl("select JumlahBunga as JumlahBunga, JumlahPokok as JumlahPokok, CONVERT(VARCHAR(10),Tgltagihan,103)  as Tanggal, FORMAT(Jumlah,'#,###,##0')  as Angsuran, FORMAT(Jumlahpokok,'#,###,##0')  as JumlahPokok2, FORMAT(Jumlahbunga,'#,###,##0')  as JumlahBunga2, Jumlah as Jumlah2 from TblTagihanNasabah where idtransjual='" & txtnopinjaman.Text & "' and idnasabah='" & txtidnasabah.Text & "' and Angsuranke=" & txtangsuranke.Text & "")
                    dtgltempo.Value = sTrue2.Rows(0).Item("Tanggal")
                    txtjumlah.Text = sTrue2.Rows(0).Item("Angsuran")
                    txtjumlah2.Text = sTrue2.Rows(0).Item("Jumlah2")
                    txtpokok.Text = sTrue2.Rows(0).Item("JumlahPokok")
                    txtpokok2.Text = sTrue2.Rows(0).Item("JumlahPokok2")
                    txtbunga.Text = sTrue2.Rows(0).Item("JumlahBunga")
                    txtbunga2.Text = sTrue2.Rows(0).Item("JumlahBunga2")
                    Dim sTrue3 As DataTable = GetDataTbl("select CONVERT(VARCHAR(10),TglJatuhTempo,103)  as Tanggal from Tbljual where idtrans='" & txtnopinjaman.Text & "' and idnasabah='" & txtidnasabah.Text & "'")
                    dtgltempokontrak.Value = sTrue3.Rows(0).Item("Tanggal")
                    Dim sTrue4 As DataTable = GetDataTbl("select isnull(Sum(saldoawal),0)  as Jumlah from Tbljual where idtrans='" & txtnopinjaman.Text & "' and idnasabah='" & txtidnasabah.Text & "'")
                    Dim sTrue5 As DataTable = GetDataTbl("select isnull(Sum(jumlah),0) as Jumlah from Tblangsuran where idtrans='" & txtnopinjaman.Text & "' and idnasabah='" & txtidnasabah.Text & "'")
                    sisa = sTrue4.Rows(0).Item("jumlah") - sTrue5.Rows(0).Item("jumlah")
                    ' sisa = Round(sTrue4.Rows(0).Item("jumlah") - sTrue5.Rows(0).Item("jumlah")).ToString("c")
                    txtsisasaldo.Text = sisa
                Else
                    Dim successScript As String = "setTimeout(function(){ Swal.fire({ icon: 'info', title: 'Mohon Maaf Angsuran Telah Selesai...', showConfirmButton: true, showCancelButton: false, timer: false, timerProgressBar: false }); }, 100);"
                    ScriptManager.RegisterStartupScript(Me, Me.[GetType](), "Popup", successScript, True)
                    Exit Sub
                End If
            End If
        ElseIf rdbtunggakan.Checked = True Then
            Dim sisa2 As Integer
            Dim kosongreal As Integer = 0
            Dim sTrue6 As DataTable = GetDataTbl("select isnull(Sum(saldoawal),0)  as Jumlah from Tbljual where idtrans='" & txtnopinjaman.Text & "' and idnasabah='" & txtidnasabah.Text & "'")
            Dim sTrue7 As DataTable = GetDataTbl("select isnull(Sum(jumlah),0) as Jumlah from Tblangsuran where nopinjaman='" & txtnopinjaman.Text & "' and idnasabah='" & txtidnasabah.Text & "'")
            sisa2 = sTrue6.Rows(0).Item("jumlah") - sTrue7.Rows(0).Item("jumlah")
            If sisa2 = 0 Then
                Dim successScript As String = "setTimeout(function(){ Swal.fire({ icon: 'warning', title: 'Pinjaman Sudah Selesai ', showConfirmButton: false, showCancelButton: false, timer: 2000, timerProgressBar: true }); }, 100);"
                ScriptManager.RegisterStartupScript(Me, Me.[GetType](), "Popup", successScript, True)
                kosong()
                Exit Sub
            Else
                Dim sisa As Integer
                Dim NO As String = clsEnt.ReturnOneFieldConn("select top(1)angsuranke + 1 as nnn from TblAngsuran where nopinjaman='" & txtnopinjaman.Text & "' and idnasabah='" & txtidnasabah.Text & "'  order by idtrans desc ", "", "", "nnn", "connuq")
                txtangsuranke.Text = IIf(NO = "", "1", NO)
                Dim sTrueJumlahangsuran As DataTable = GetDataTbl("select Format(sum(Jumlah),'#,###,##0') as Angsuran from TblTagihanNasabah where idtransjual='" & txtnopinjaman.Text & "' and idnasabah='" & txtidnasabah.Text & "' and TglTagihan<'" & clsEnt.CDateME5(dtglangsuran.Value) & "' and RealisasiAngsuran=" & kosongreal & "")
                Dim sTrueJumlahangsuran2 As DataTable = GetDataTbl("select isnull(sum(Jumlah),0) as Angsuran from TblTagihanNasabah where idtransjual='" & txtnopinjaman.Text & "' and idnasabah='" & txtidnasabah.Text & "' and TglTagihan<'" & clsEnt.CDateME5(dtglangsuran.Value) & "' and RealisasiAngsuran=" & kosongreal & "")

                Dim sTruejumlahpokok As DataTable = GetDataTbl("select Format(sum(Jumlahpokok),'#,###,##0') as jumlah from TblTagihanNasabah where idtransjual='" & txtnopinjaman.Text & "' and idnasabah='" & txtidnasabah.Text & "' and TglTagihan<'" & clsEnt.CDateME5(dtglangsuran.Value) & "' and RealisasiAngsuran=" & kosongreal & "")
                Dim sTruejumlahpokok2 As DataTable = GetDataTbl("select isnull(sum(Jumlahpokok),0) as jumlah from TblTagihanNasabah where idtransjual='" & txtnopinjaman.Text & "' and idnasabah='" & txtidnasabah.Text & "' and TglTagihan<'" & clsEnt.CDateME5(dtglangsuran.Value) & "' and RealisasiAngsuran=" & kosongreal & "")


                Dim sTruejumlahbunga As DataTable = GetDataTbl("select FORMAT(Sum(Jumlahbunga),'#,###,##0')  as Jumlah from TblTagihanNasabah where idtransjual='" & txtnopinjaman.Text & "' and idnasabah='" & txtidnasabah.Text & "' and TglTagihan<'" & clsEnt.CDateME5(dtglangsuran.Value) & "' and RealisasiAngsuran=" & kosongreal & "")
                Dim sTruejumlahbunga2 As DataTable = GetDataTbl("select isnull(Sum(Jumlahbunga),0)  as Jumlah from TblTagihanNasabah where idtransjual='" & txtnopinjaman.Text & "' and idnasabah='" & txtidnasabah.Text & "' and TglTagihan<'" & clsEnt.CDateME5(dtglangsuran.Value) & "' and RealisasiAngsuran=" & kosongreal & "")

                Dim sTruejatuhtempo As DataTable = GetDataTbl("select CONVERT(VARCHAR(10),Tgltagihan,103)  as Tanggal from TblTagihanNasabah where idtransjual='" & txtnopinjaman.Text & "' and idnasabah='" & txtidnasabah.Text & "' and TglTagihan<'" & clsEnt.CDateME5(dtglangsuran.Value) & "' and RealisasiAngsuran=" & kosongreal & "")


                dtgltempo.Value = sTruejatuhtempo.Rows(0).Item("Tanggal")
                txtjumlah.Text = sTrueJumlahangsuran.Rows(0).Item("Angsuran")
                txtjumlah2.Text = sTrueJumlahangsuran2.Rows(0).Item("Angsuran")
                txtpokok2.Text = sTruejumlahpokok.Rows(0).Item("jumlah")
                txtpokok.Text = sTruejumlahpokok2.Rows(0).Item("jumlah")
                txtbunga2.Text = sTruejumlahbunga.Rows(0).Item("Jumlah")
                txtbunga.Text = sTruejumlahbunga2.Rows(0).Item("Jumlah")

                Dim sTrue3 As DataTable = GetDataTbl("select CONVERT(VARCHAR(10),TglJatuhTempo,103)  as Tanggal from Tbljual where idtrans='" & txtnopinjaman.Text & "' and idnasabah='" & txtidnasabah.Text & "'")
                dtgltempokontrak.Value = sTrue3.Rows(0).Item("Tanggal")
                Dim sTrue4 As DataTable = GetDataTbl("select isnull(Sum(saldoawal),0)  as Jumlah from Tbljual where idtrans='" & txtnopinjaman.Text & "' and idnasabah='" & txtidnasabah.Text & "'")
                Dim sTrue5 As DataTable = GetDataTbl("select isnull(Sum(jumlah),0) as Jumlah from Tblangsuran where idtrans='" & txtnopinjaman.Text & "' and idnasabah='" & txtidnasabah.Text & "'")
                sisa = sTrue4.Rows(0).Item("jumlah") - sTrue5.Rows(0).Item("jumlah")
                txtsisasaldo.Text = sisa
            End If
        ElseIf rdbpelunasan.Checked = True Then
            Dim sisa2 As Integer
            Dim kosongreal As Integer = 0
            Dim sTrue6 As DataTable = GetDataTbl("select isnull(Sum(saldoawal),0)  as Jumlah from Tbljual where idtrans='" & txtnopinjaman.Text & "' and idnasabah='" & txtidnasabah.Text & "'")
            Dim sTrue7 As DataTable = GetDataTbl("select isnull(Sum(jumlah),0) as Jumlah from Tblangsuran where nopinjaman='" & txtnopinjaman.Text & "' and idnasabah='" & txtidnasabah.Text & "'")
            sisa2 = sTrue6.Rows(0).Item("jumlah") - sTrue7.Rows(0).Item("jumlah")
            If sisa2 = 0 Then
                Dim successScript As String = "setTimeout(function(){ Swal.fire({ icon: 'warning', title: 'Pinjaman Sudah Selesai ', showConfirmButton: false, showCancelButton: false, timer: 2000, timerProgressBar: true }); }, 100);"
                ScriptManager.RegisterStartupScript(Me, Me.[GetType](), "Popup", successScript, True)
                kosong()
                Exit Sub
            Else
                Dim sisa As Integer
                Dim NO As String = clsEnt.ReturnOneFieldConn("select top(1)angsuranke + 1 as nnn from TblAngsuran where nopinjaman='" & txtnopinjaman.Text & "' and idnasabah='" & txtidnasabah.Text & "'  order by idtrans desc ", "", "", "nnn", "connuq")
                txtangsuranke.Text = IIf(NO = "", "1", NO)
                Dim sTrueJumlahangsuran As DataTable = GetDataTbl("select Format(sum(Jumlah),'#,###,##0') as Angsuran from TblTagihanNasabah where idtransjual='" & txtnopinjaman.Text & "' and idnasabah='" & txtidnasabah.Text & "' and RealisasiAngsuran=" & kosongreal & "")
                Dim sTrueJumlahangsuran2 As DataTable = GetDataTbl("select isnull(sum(Jumlah),0) as Angsuran from TblTagihanNasabah where idtransjual='" & txtnopinjaman.Text & "' and idnasabah='" & txtidnasabah.Text & "' and RealisasiAngsuran=" & kosongreal & "")
                Dim sTruejumlahpokok As DataTable = GetDataTbl("select Format(sum(Jumlahpokok),'#,###,##0') as jumlah from TblTagihanNasabah where idtransjual='" & txtnopinjaman.Text & "' and idnasabah='" & txtidnasabah.Text & "' and RealisasiAngsuran=" & kosongreal & "")
                Dim sTruejumlahpokok2 As DataTable = GetDataTbl("select isnull(sum(Jumlahpokok),0) as jumlah from TblTagihanNasabah where idtransjual='" & txtnopinjaman.Text & "' and idnasabah='" & txtidnasabah.Text & "' and RealisasiAngsuran=" & kosongreal & "")
                Dim sTruejumlahbunga As DataTable = GetDataTbl("select FORMAT(Sum(Jumlahbunga),'#,###,##0')  as Jumlah from TblTagihanNasabah where idtransjual='" & txtnopinjaman.Text & "' and idnasabah='" & txtidnasabah.Text & "' and RealisasiAngsuran=" & kosongreal & "")
                Dim sTruejumlahbunga2 As DataTable = GetDataTbl("select isnull(Sum(Jumlahbunga),0)  as Jumlah from TblTagihanNasabah where idtransjual='" & txtnopinjaman.Text & "' and idnasabah='" & txtidnasabah.Text & "' and RealisasiAngsuran=" & kosongreal & "")
                Dim sTruejatuhtempo As DataTable = GetDataTbl("select CONVERT(VARCHAR(10),Tgltagihan,103)  as Tanggal from TblTagihanNasabah where idtransjual='" & txtnopinjaman.Text & "' and idnasabah='" & txtidnasabah.Text & "'  and RealisasiAngsuran=" & kosongreal & "")



                dtgltempo.Value = sTruejatuhtempo.Rows(0).Item("Tanggal")
                txtjumlah.Text = sTrueJumlahangsuran.Rows(0).Item("Angsuran")
                txtjumlah2.Text = sTrueJumlahangsuran2.Rows(0).Item("Angsuran")
                txtpokok2.Text = sTruejumlahpokok.Rows(0).Item("jumlah")
                txtpokok.Text = sTruejumlahpokok2.Rows(0).Item("jumlah")
                txtbunga2.Text = sTruejumlahbunga.Rows(0).Item("Jumlah")
                txtbunga.Text = sTruejumlahbunga2.Rows(0).Item("Jumlah")

                Dim sTrue3 As DataTable = GetDataTbl("select CONVERT(VARCHAR(10),TglJatuhTempo,103)  as Tanggal from Tbljual where idtrans='" & txtnopinjaman.Text & "' and idnasabah='" & txtidnasabah.Text & "'")
                dtgltempokontrak.Value = sTrue3.Rows(0).Item("Tanggal")
                Dim sTrue4 As DataTable = GetDataTbl("select isnull(Sum(saldoawal),0)  as Jumlah from Tbljual where idtrans='" & txtnopinjaman.Text & "' and idnasabah='" & txtidnasabah.Text & "'")
                Dim sTrue5 As DataTable = GetDataTbl("select isnull(Sum(jumlah),0) as Jumlah from Tblangsuran where idtrans='" & txtnopinjaman.Text & "' and idnasabah='" & txtidnasabah.Text & "'")
                sisa = sTrue4.Rows(0).Item("jumlah") - sTrue5.Rows(0).Item("jumlah")
                txtsisasaldo.Text = sisa
            End If
        End If
    End Sub


    Public Sub SimpanJurnal(sender As Object, e As EventArgs)
        Dim hjum As String = 1
        Dim strUID As String = Trim(Session.Item("UserAuthentication").ToString)
        Dim Saldo2 As Integer = 0


        Dim nojurnal As String = txtidtrans.Text + ddlunit.Text + ddlcabang.Text
        Dim Keterangan As String = "Penjualan Sepeda Motor No Plat" + txtplat.Text + "Dengan Jenis" + txtjenis.Text & "Atas Nama" + txtnama.Text

        Dim sTruesaldoawal As DataTable = GetDataTbl("select isnull(Sum(Saldoawal),0) as jumlah from Tbljual where idtrans='" & txtnopinjaman.Text & "' And idNasabah='" & txtidnasabah.Text & "'")
        Dim sTruesaldo As DataTable = GetDataTbl("select isnull(Sum(jumlah),0) as jumlah from TblAngsuran where NoPinjaman='" & txtnopinjaman.Text & "' And idNasabah='" & txtidnasabah.Text & "'")
        Dim Jumlahsaldoakhir As Integer = sTruesaldo.Rows(0).Item("jumlah")
        Dim SaldoakhirAngsuran, Jumlahangsurannasabah, alljumlahsaldo As Integer
        Jumlahangsurannasabah = txtjumlah2.Text
        SaldoakhirAngsuran = Jumlahsaldoakhir - Jumlahangsurannasabah
        alljumlahsaldo = sTruesaldoawal.Rows(0).Item("jumlah") - SaldoakhirAngsuran


        connectDB()
        mySqlCon.Open()
        mySqlCmd = New System.Data.OleDb.OleDbCommand()

        Dim Transaction = mySqlCon.BeginTransaction(IsolationLevel.ReadCommitted)
        mySqlCmd.Connection = mySqlCon
        mySqlCmd.Transaction = Transaction
        Try
            mySqlCmd.CommandText = "INSERT INTO TblJurnalNasabah(NoJurnal,NoPinjaman,IdCabang,IdUnit,IdNasabah,tanggal,Keterangan,Debet,Kredit,SaldoAkhir,LastUpdate,UseridUpdate) VALUES ('" _
                   & nojurnal & "','" & txtnopinjaman.Text & "','" & ddlcabang.Text & "','" & ddlunit.Text & "','" & txtidnasabah.Text & "','" & clsEnt.CDateME5(dtglangsuran.Value) & "','" & Keterangan & "'," & Saldo2 & "," & Jumlahangsurannasabah & "," & alljumlahsaldo & ", Getdate(),'" & strUID & "'); "

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
            mySqlCmd.CommandText = "DELETE FROM TblJurnalNasabah WHERE Nojurnal = '" & nojurnal & "'; "

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

    Public Sub UpdateStatusAngsuran(sender As Object, e As EventArgs)

        Dim hjum As String = 1
        Dim strUID As String = Trim(Session.Item("UserAuthentication").ToString)
        Dim ketstatusangsuran2 As String = ""
        Dim Tanggal As Date
        Dim TanggalAkhir As Date
        Tanggal = dtgltempo.Value 'membuat tanggal 6 June 2021

        Dim tanggalTertentu As DateTime = dtgltempo.Value


        Dim awalBulan As DateTime = New DateTime(tanggalTertentu.Year, tanggalTertentu.Month, 1)
        TanggalAkhir = DateSerial(Year(Tanggal), Month(Tanggal) + 1, 0)

        connectDB()
        mySqlCon.Open()
        mySqlCmd = New System.Data.OleDb.OleDbCommand()

        Dim Transaction = mySqlCon.BeginTransaction(IsolationLevel.ReadCommitted)
        mySqlCmd.Connection = mySqlCon
        mySqlCmd.Transaction = Transaction

        Try
            mySqlCmd.CommandText = mySqlCmd.CommandText & "UPDATE TblTagihanNasabah SET StatusAngsuran='" & ketstatusangsuran2 & "', LastUpdate=Getdate(),UseridUpdate='" & strUID & "' WHERE idTransJual= '" & txtnopinjaman.Text & "' and idNasabah='" & txtidnasabah.Text & "' And TglTagihan>'" & clsEnt.CDateME5(dtglangsuran.Value) & "';"

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

End Class
