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
Imports iTextSharp.text
Imports iTextSharp.text.pdf

Partial Class RptRealSirkulasi
    Inherits System.Web.UI.Page

#Region "Variables & Connect DB"

    Dim mySqlCon As Data.OleDb.OleDbConnection
    Dim mySqlCmd As Data.OleDb.OleDbCommand
    Dim SQL As String
    Dim DataLineItemPiutang As DataTable
    Dim editLineItemPiutang As Integer
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


#Region "page Error & load form"
    Protected Sub Page_Error(sender As Object, e As System.EventArgs) Handles Me.Error
        WriteError("Page Level Error Handled : " + Server.GetLastError().Message)
        Server.ClearError()

    End Sub

    Private Sub WriteError(messg As String)
        Response.Write("<div><h1>Page Error </h1><div class='UserControlDiv'>Error on the Page : <b>" + messg + "</b></div></div>")
    End Sub
    Protected Sub Page_Load(sender As Object, e As System.EventArgs) Handles Me.Load
        Dim sUsr As String = Session("UserAuthentication")
        If sUsr = "" Then
            ShowSweetAlert2("info", "Anda Login Terdahulu!")
            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "Redirect", "setTimeout(function () {  window.location.href ='Default.aspx' }, 2000);", True)
        End If

        If Not IsPostBack Then

        End If

        txtidcabang.Attributes.Add("Readonly", "Readonly")
        txtidunit.Attributes.Add("Readonly", "Readonly")
        txtnamacabang.Attributes.Add("Readonly", "Readonly")
        txtnamaUnit.Attributes.Add("Readonly", "Readonly")
        txtidwipem.Attributes.Add("Readonly", "Readonly")
        txtnamawipem.Attributes.Add("Readonly", "Readonly")

    End Sub
#End Region
    Private Sub ShowSweetAlert2(ByVal icon As String, ByVal titleString As String)
        Dim sweetAlertString As String = "setTimeout(function(){ Swal.fire({ icon: '" & icon & "', title: '" & titleString & "', showConfirmButton: false, showCancelButton: false, timer: 2000, timerProgressBar: true }); }, 100);"
        ScriptManager.RegisterStartupScript(Me, Me.[GetType](), "Popup", sweetAlertString, True)
    End Sub
#Region "PDF"

    Private Shared Sub DrawLine(writer As PdfWriter, x1 As Single, y1 As Single, x2 As Single, y2 As Single, color As iTextSharp.text.Color)
        Dim contentByte As PdfContentByte = writer.DirectContent
        contentByte.SetColorStroke(color)
        contentByte.MoveTo(x1, y1)
        contentByte.LineTo(x2, y2)
        contentByte.Stroke()
    End Sub
    Private Shared Function TabelCell(tbl As PdfPTable, align As Integer, bordercolor As iTextSharp.text.Color) As PdfPCell
        Dim cell As New PdfPCell(tbl)
        cell.BorderColor = bordercolor
        cell.VerticalAlignment = Element.ALIGN_TOP
        cell.HorizontalAlignment = align
        cell.PaddingBottom = 2.0F
        cell.PaddingTop = 0.0F
        Return cell
    End Function
    Private Shared Function PhraseCell(phrase As Phrase, align As Integer, bordercolor As iTextSharp.text.Color) As PdfPCell
        Dim cell As New PdfPCell(phrase)
        cell.BorderColor = bordercolor
        cell.VerticalAlignment = Element.ALIGN_TOP
        cell.HorizontalAlignment = align
        cell.PaddingBottom = 2.0F
        cell.PaddingTop = 1.0F
        Return cell
    End Function
    Private Shared Function ImageCell(path As String, scale As Single, align As Integer) As PdfPCell
        Dim image As iTextSharp.text.Image = iTextSharp.text.Image.GetInstance(HttpContext.Current.Server.MapPath(path))
        image.ScalePercent(scale)
        Dim cell As New PdfPCell(image)
        cell.BorderColor = iTextSharp.text.Color.WHITE
        cell.VerticalAlignment = Element.ALIGN_TOP
        cell.HorizontalAlignment = align
        cell.PaddingBottom = 0.0F
        cell.PaddingTop = 0.0F
        Return cell
    End Function
    Public Shared Function GetFont(fontName As String) As BaseFont
        Return BaseFont.CreateFont(HttpContext.Current.Server.MapPath("~/Bin/" + fontName), BaseFont.CP1252, BaseFont.EMBEDDED)
    End Function


#End Region


    Protected Sub lnkPrintJadwal_Click(sender As Object, e As System.EventArgs) Handles lnkPrintJadwal.Click

        ' PrintSirkulasi(Nothing,Nothing)


        If dtpTglawal.Value.Trim = "" Then
            Dim successScript As String = "setTimeout(function(){ Swal.fire({ icon: 'info', title: 'Tanggal Awal tidak boleh kosong...', showConfirmButton: false, showCancelButton: false, timer: 2000, timerProgressBar: true }); }, 100);"
            ScriptManager.RegisterStartupScript(Me, Me.[GetType](), "Popup", successScript, True)
            dtpTglawal.Focus()
            Exit Sub
        ElseIf dtpTglakhir.Value.Trim = "" Then
            Dim successScript As String = "setTimeout(function(){ Swal.fire({ icon: 'info', title: 'Tanggal Akhir tidak boleh kosong...', showConfirmButton: false, showCancelButton: false, timer: 2000, timerProgressBar: true }); }, 100);"
            ScriptManager.RegisterStartupScript(Me, Me.[GetType](), "Popup", successScript, True)
            dtpTglakhir.Focus()
        ElseIf Day(dtpTglawal.Value) <> 1 Then
            Dim successScript As String = "setTimeout(function(){ Swal.fire({ icon: 'info', title: 'Tanggal Akhir tidak boleh kosong...', showConfirmButton: false, showCancelButton: false, timer: 2000, timerProgressBar: true }); }, 100);"
            ScriptManager.RegisterStartupScript(Me, Me.[GetType](), "Popup", successScript, True)
            dtpTglawal.Focus()
            Exit Sub
        ElseIf txtidcabang.Text.Trim = "" Then
            Dim successScript As String = "setTimeout(function(){ Swal.fire({ icon: 'info', title: 'Id Cabang tidak boleh kosong...', showConfirmButton: false, showCancelButton: false, timer: 2000, timerProgressBar: true }); }, 100);"
            ScriptManager.RegisterStartupScript(Me, Me.[GetType](), "Popup", successScript, True)
            dtpTglawal.Focus()
            Exit Sub
        ElseIf txtidunit.Text.Trim = "" Then
            Dim successScript As String = "setTimeout(function(){ Swal.fire({ icon: 'info', title: 'Id Unit tidak boleh kosong...', showConfirmButton: false, showCancelButton: false, timer: 2000, timerProgressBar: true }); }, 100);"
            ScriptManager.RegisterStartupScript(Me, Me.[GetType](), "Popup", successScript, True)
            dtpTglawal.Focus()
            Exit Sub
        ElseIf String.IsNullOrEmpty(dtpTglawal.Value.Trim()) Then
            Dim successScript As String = "setTimeout(function(){ Swal.fire({ icon: 'warning', title: 'Tanggal Awal Tidak Boleh Kosong', showConfirmButton: false, showCancelButton: false, timer: 2000, timerProgressBar: true }); }, 100);"
            ScriptManager.RegisterStartupScript(Me, Me.[GetType](), "Popup", successScript, True)
            Exit Sub
        ElseIf String.IsNullOrEmpty(dtpTglakhir.Value.Trim()) Then
            Dim successScript As String = "setTimeout(function(){ Swal.fire({ icon: 'warning', title: 'Tanggal Akhir Tidak Boleh Kosong', showConfirmButton: false, showCancelButton: false, timer: 2000, timerProgressBar: true }); }, 100);"
            ScriptManager.RegisterStartupScript(Me, Me.[GetType](), "Popup", successScript, True)
            Exit Sub
        End If

        UpdateTagihan(Nothing, Nothing)
        GenerateSirkulasi(Nothing, Nothing)
        GenerateLancar(Nothing, Nothing)
        GenerateLancarJual(Nothing, Nothing)
        GenerateLancarTagihan(Nothing, Nothing)
        GenerateLancarTagihanJual(Nothing, Nothing)

        Dim Tanggal As Date
        Dim TanggalAkhir As Date
        Tanggal = dtpTglawal.Value 'membuat tanggal 6 June 2021
        'rumus mencari tanggal akhir dalam suatu bulan
        TanggalAkhir = DateSerial(Year(Tanggal), Month(Tanggal) + 1, 0)
        Dim tanggalTertentu As DateTime = dtpTglawal.Value
        Dim awalBulan As DateTime = New DateTime(tanggalTertentu.Year, tanggalTertentu.Month, 1)

        If dtpTglakhir.Value.Trim <> TanggalAkhir Then
            Dim successScript As String = "setTimeout(function(){ Swal.fire({ icon: 'info', title: 'Anda Salah Input Tanggal Akhir ...', showConfirmButton: false, showCancelButton: false, timer: 2000, timerProgressBar: true }); }, 100);"
            ScriptManager.RegisterStartupScript(Me, Me.[GetType](), "Popup", successScript, True)
            dtpTglakhir.Focus()
            Exit Sub
        ElseIf dtpTglawal.Value.Trim <> awalBulan Then
            Dim successScript As String = "setTimeout(function(){ Swal.fire({ icon: 'info', title: 'Tanggal Awal di Mulai Tanggal 1 ...', showConfirmButton: false, showCancelButton: false, timer: 2000, timerProgressBar: true }); }, 100);"
        ScriptManager.RegisterStartupScript(Me, Me.[GetType](), "Popup", successScript, True)
        dtpTglakhir.Focus()
        Exit Sub
        End If


        Dim KetLunas As String = ""

        Dim dtHead As DataTable
        Dim saldoawalpokok As Integer = 0
        Dim saldoawalbunga As Integer = 0
        Dim mutasimasukpokok As Integer = 0
        Dim mutasimasukbunga As Integer = 0
        Dim mutasikeluarpokok As Integer = 0
        Dim mutasikeluarbunga As Integer = 0
        Dim angsuranpokok As Integer = 0
        Dim angsuranbunga As Integer = 0
        Dim saldoakhirpokok As Integer = 0
        Dim saldoakhirbunga As Integer = 0
        Dim persentasepokok As Integer = 0
        Dim persentasebunga As Integer = 0
        Dim nol As Integer = 0


        Dim dtsaldoawalpokok As DataTable
        Dim dtsaldoawalpokok2 As DataTable
        Dim dtsaldoawalbunga As DataTable
        Dim dtsaldoawalbunga2 As DataTable
        Dim dtmasukpokok As DataTable
        Dim dtmasukpokok2 As DataTable
        Dim dtmasukbunga As DataTable
        Dim dtmasukbunga2 As DataTable
        Dim dtmutasimasukpokok As DataTable
        Dim dtmutasimasukbunga As DataTable
        Dim dtmutasikeluarpokok As DataTable
        Dim dtmutasikeluarbunga As DataTable
        Dim dtangsuranpokok As DataTable
        Dim dtangsuranbunga As DataTable
        Dim dtsaldoakhirpokok As DataTable
        Dim dtsaldoakhirbunga As DataTable
        Dim pdtersentasepokok As DataTable
        Dim dtpersentasebunga As DataTable




        dtHead = GetDataTbl("SELECT idCabang, idUnit, Nama, Nourut  " _
                            & " From TblReportSirkulasi Order by Kode Asc ")


        Dim sepLine As iTextSharp.text.pdf.draw.LineSeparator = New iTextSharp.text.pdf.draw.LineSeparator()
        sepLine.LineWidth = 1
        sepLine.Percentage = 100
        sepLine.LineColor = New iTextSharp.text.Color(System.Drawing.Color.Black)
        Dim chunkLine As Chunk = New Chunk(sepLine)

        Dim paragrp As Paragraph = Nothing
        Dim paragrp2 As Paragraph = Nothing
        Dim phrase As Phrase = Nothing
        Dim cell As PdfPCell = Nothing
        Dim table As PdfPTable = Nothing
        Dim color__1 As iTextSharp.text.Color = Nothing
        Dim fntArial8 As iTextSharp.text.Font = FontFactory.GetFont("Arial", 8, iTextSharp.text.Font.NORMAL, iTextSharp.text.Color.BLACK)
        Dim fntTahoma As iTextSharp.text.Font = FontFactory.GetFont("Tahoma", 8, iTextSharp.text.Font.NORMAL, iTextSharp.text.Color.BLACK)
        Dim fntArial12N As iTextSharp.text.Font = FontFactory.GetFont("Arial", 8, iTextSharp.text.Font.NORMAL, iTextSharp.text.Color.BLACK)
        Dim fntArial12U As iTextSharp.text.Font = FontFactory.GetFont("Arial", 8, iTextSharp.text.Font.UNDERLINE, iTextSharp.text.Color.BLACK)
        Dim fntArial12B As iTextSharp.text.Font = FontFactory.GetFont("Arial", 8, iTextSharp.text.Font.BOLD, iTextSharp.text.Color.BLACK)
        'Dim fntArial12C As iTextSharp.text.Font = FontFactory.GetFont("Arial", 8, iTextSharp.text.Font.UNDEFINED, iTextSharp.text.Color.BLACK)

        Dim fntArial10N As iTextSharp.text.Font = FontFactory.GetFont("Arial", 8, iTextSharp.text.Font.NORMAL, iTextSharp.text.Color.BLACK)
        Dim fntArial10U As iTextSharp.text.Font = FontFactory.GetFont("Arial", 10, iTextSharp.text.Font.UNDERLINE, iTextSharp.text.Color.BLACK)
        Dim fntArial10B As iTextSharp.text.Font = FontFactory.GetFont("Arial", 12, iTextSharp.text.Font.BOLD, iTextSharp.text.Color.BLACK)
        Dim fntArial10Bw As iTextSharp.text.Font = FontFactory.GetFont("Arial", 10, iTextSharp.text.Font.BOLD, iTextSharp.text.Color.WHITE)
        Dim fntArial14U As iTextSharp.text.Font = FontFactory.GetFont("Arial", 14, iTextSharp.text.Font.NORMAL, iTextSharp.text.Color.BLACK)

        Dim BaskervileOldFace36 As iTextSharp.text.Font = FontFactory.GetFont("Arial", 36, iTextSharp.text.Font.BOLD, iTextSharp.text.Color.BLACK)
        Dim BaskervileOldFace28 As iTextSharp.text.Font = FontFactory.GetFont("Arial", 24, iTextSharp.text.Font.BOLD, iTextSharp.text.Color.BLACK)
        Dim BaskervileOldFace As iTextSharp.text.Font = FontFactory.GetFont("Arial", 12, iTextSharp.text.Font.BOLD, iTextSharp.text.Color.BLACK)

        If dtHead.Rows.Count >= 1 Then
            'Dim myWidth As Single
            'Dim myHeight As Single
            Dim leftMargin As Single = 10
            Dim rightMargin As Single = 10
            Dim topMargin As Single = 5
            Dim bottomMargin As Single = 5
            Dim filepath As String = Server.MapPath("PDF/SirkulasiPinjaman" & Format(Now, "dd-MM-yyyy") & ".pdf")
            Using fs As New FileStream(Server.MapPath("PDF/SirkulasiPinjaman" & Format(Now, "dd-MM-yyyy") & ".pdf"), FileMode.Create)
                Dim doc As New iTextSharp.text.Document(iTextSharp.text.PageSize.LEGAL.Rotate, leftMargin, rightMargin, topMargin, bottomMargin)
                PdfWriter.GetInstance(doc, fs)
                doc.AddTitle("Sirkulasi Pinjaman")
                doc.Open()

                'Dim sTrue5 As DataTable = GetDataTbl("select b.lokasi as lokasicabang, c.lokasi as lokasiunit from tbljual a inner join tblcabang b on b.IDCABANG=a.idcabang inner join tblunitkerja c on c.idunit=a.idunit  where idcabang='" & txtidcabang.Text & "' And idunit='" & txtidunit.Text & "'")
                'Dim akmp As String = sTrue5.Rows(0).Item("lokasicabang")
                'Dim akmp2 As String = sTrue5.Rows(0).Item("lokasiunit")
                'If jATUHtEMPO > dtglangsuran.Value Then
                '    StatusAnsuranNasabah = "LANCAR"
                'Else
                '    StatusAnsuranNasabah = "MACET"
                'End If

                '  Dim akmp As String = "UNIVERSITAS QUALITY"
                'If Left(ketidkampus, 1) = "1" Then
                '    akmp = "UNIVERSITAS QUALITY BRASTAGI"
                'End If
                Dim akmp As String = txtnamacabang.Text
                Dim akmp2 As String = txtnamaUnit.Text
                Dim akmp3 As String = "Peroide Tanggal : "

                'Dim uq As Paragraph = New Paragraph(akmp, BaskervileOldFace36)
                'uq.Alignment = Element.ALIGN_CENTER
                'doc.Add(uq)
                '   doc.Add(New Paragraph(" "))

                Dim uq2 As Paragraph = New Paragraph(akmp2, BaskervileOldFace28)
                uq2.Alignment = Element.ALIGN_CENTER
                doc.Add(uq2)
                ' doc.Add(New Paragraph(" "))

                paragrp = New Paragraph("REKAPITULASI LAPORAN PIUTANG GLOBAL ", fntArial10B)
                paragrp.Alignment = Element.ALIGN_CENTER
                doc.Add(paragrp)

                paragrp = New Paragraph("Periode Tanggal :  " & dtpTglawal.Value & " S/d " & dtpTglakhir.Value, fntArial10B)
                paragrp.Alignment = Element.ALIGN_CENTER
                doc.Add(paragrp)
                '  doc.Add(New Paragraph(" "))

                'Dim uq3 As Paragraph = New Paragraph(akmp3, fntArial10Bw)
                'uq3.Alignment = Element.ALIGN_CENTER
                'doc.Add(uq3)
                'doc.Add(New Paragraph(" "))



                Dim myTableCol As PdfPTable = New PdfPTable(14)
                Dim sglTblHdWidthsCol As Single() = New Single(13) {}



                sglTblHdWidthsCol(0) = 2.0F    ' no
                sglTblHdWidthsCol(1) = 5.0F     ' keterangan
                sglTblHdWidthsCol(2) = 3.0F    ' saldo awal pokok
                sglTblHdWidthsCol(3) = 3.0F    ' saldo awal bunga
                sglTblHdWidthsCol(4) = 3.0F     ' mutasi masuk pokok
                sglTblHdWidthsCol(5) = 3.0F    ' mutasi masuk bunga
                sglTblHdWidthsCol(6) = 3.0F    ' Mutasi keluar pokok
                sglTblHdWidthsCol(7) = 3.0F     ' mutasi keluar bunga
                sglTblHdWidthsCol(8) = 3.0F    ' angsuran pokok
                sglTblHdWidthsCol(9) = 3.0F    ' angsuran bunga
                sglTblHdWidthsCol(10) = 3.0F    ' Saldo akhir pokok
                sglTblHdWidthsCol(11) = 3.0F    ' Saldo Akhir bunga
                sglTblHdWidthsCol(12) = 3.0F    ' persentase pokok
                sglTblHdWidthsCol(13) = 3.0F    ' persentase bunga 

                'myTable.LockedWidth = True
                myTableCol.HorizontalAlignment = Element.ALIGN_LEFT
                myTableCol.WidthPercentage = 100
                myTableCol.SetWidths(sglTblHdWidthsCol)



                'Kelas grupping 
                phrase = New Phrase("", fntTahoma) : cell = PhraseCell(phrase, PdfPCell.ALIGN_CENTER, iTextSharp.text.Color.BLACK) : cell.Border = PdfCell.NO_BORDER : cell.Colspan = 15 : myTableCol.AddCell(cell)
                phrase = New Phrase("", fntTahoma) : cell = PhraseCell(phrase, PdfPCell.ALIGN_CENTER, iTextSharp.text.Color.BLACK) : cell.Border = PdfCell.NO_BORDER : cell.Colspan = 15 : myTableCol.AddCell(cell)
                phrase = New Phrase("", fntTahoma) : cell = PhraseCell(phrase, PdfPCell.ALIGN_CENTER, iTextSharp.text.Color.BLACK) : cell.Border = PdfCell.NO_BORDER : cell.Colspan = 15 : myTableCol.AddCell(cell)
                phrase = New Phrase("", fntTahoma) : cell = PhraseCell(phrase, PdfPCell.ALIGN_CENTER, iTextSharp.text.Color.BLACK) : cell.Border = PdfCell.NO_BORDER : cell.Colspan = 15 : myTableCol.AddCell(cell)
                phrase = New Phrase("", fntTahoma) : cell = PhraseCell(phrase, PdfPCell.ALIGN_CENTER, iTextSharp.text.Color.BLACK) : cell.Border = PdfCell.NO_BORDER : cell.Colspan = 15 : myTableCol.AddCell(cell)

                'Rows 1
                phrase = New Phrase("Keterangan ", fntArial14U) : cell = PhraseCell(phrase, PdfPCell.ALIGN_CENTER, iTextSharp.text.Color.BLACK) : cell = PhraseCell(phrase, PdfPCell.ALIGN_CENTER, iTextSharp.text.Color.BLACK) : cell.VerticalAlignment = Element.ALIGN_CENTER : cell.Colspan = 2 : myTableCol.AddCell(cell)
                phrase = New Phrase("Saldo Awal", fntArial14U) : cell = PhraseCell(phrase, PdfPCell.ALIGN_CENTER, iTextSharp.text.Color.BLACK) : cell = PhraseCell(phrase, PdfPCell.ALIGN_CENTER, iTextSharp.text.Color.BLACK) : cell.VerticalAlignment = Element.ALIGN_CENTER : cell.Colspan = 2 : myTableCol.AddCell(cell)
                phrase = New Phrase("Mutasi Masuk", fntArial14U) : cell = PhraseCell(phrase, PdfPCell.ALIGN_CENTER, iTextSharp.text.Color.BLACK) : cell = PhraseCell(phrase, PdfPCell.ALIGN_CENTER, iTextSharp.text.Color.BLACK) : cell.VerticalAlignment = Element.ALIGN_CENTER : cell.Colspan = 2 : myTableCol.AddCell(cell)
                phrase = New Phrase("Mutasi Keluar", fntArial14U) : cell = PhraseCell(phrase, PdfPCell.ALIGN_CENTER, iTextSharp.text.Color.BLACK) : cell = PhraseCell(phrase, PdfPCell.ALIGN_CENTER, iTextSharp.text.Color.BLACK) : cell.VerticalAlignment = Element.ALIGN_CENTER : cell.Colspan = 2 : myTableCol.AddCell(cell)
                phrase = New Phrase("Angsuran ", fntArial14U) : cell = PhraseCell(phrase, PdfPCell.ALIGN_CENTER, iTextSharp.text.Color.BLACK) : cell = PhraseCell(phrase, PdfPCell.ALIGN_CENTER, iTextSharp.text.Color.BLACK) : cell.VerticalAlignment = Element.ALIGN_CENTER : cell.Colspan = 2 : myTableCol.AddCell(cell)
                phrase = New Phrase("Saldo Akhir ", fntArial14U) : cell = PhraseCell(phrase, PdfPCell.ALIGN_LEFT, iTextSharp.text.Color.BLACK) : cell = PhraseCell(phrase, PdfPCell.ALIGN_CENTER, iTextSharp.text.Color.BLACK) : cell.VerticalAlignment = Element.ALIGN_CENTER : cell.Colspan = 2 : myTableCol.AddCell(cell)
                phrase = New Phrase("Persentase ", fntArial14U) : cell = PhraseCell(phrase, PdfPCell.ALIGN_LEFT, iTextSharp.text.Color.BLACK) : cell = PhraseCell(phrase, PdfPCell.ALIGN_CENTER, iTextSharp.text.Color.BLACK) : cell.VerticalAlignment = Element.ALIGN_CENTER : cell.Colspan = 2 : myTableCol.AddCell(cell)

                ''Header Tabel
                phrase = New Phrase("No", fntArial10N) : cell = PhraseCell(phrase, PdfPCell.ALIGN_CENTER, iTextSharp.text.Color.BLACK) : cell.FixedHeight = 10.0F : myTableCol.AddCell(cell)
                phrase = New Phrase("Kelompok ", fntArial10N) : cell = PhraseCell(phrase, PdfPCell.ALIGN_CENTER, iTextSharp.text.Color.BLACK) : myTableCol.AddCell(cell)
                phrase = New Phrase("Pokok", fntArial10N) : cell = PhraseCell(phrase, PdfPCell.ALIGN_CENTER, iTextSharp.text.Color.BLACK) : myTableCol.AddCell(cell)
                phrase = New Phrase("Bunga", fntArial10N) : cell = PhraseCell(phrase, PdfPCell.ALIGN_CENTER, iTextSharp.text.Color.BLACK) : myTableCol.AddCell(cell)
                phrase = New Phrase("Pokok", fntArial10N) : cell = PhraseCell(phrase, PdfPCell.ALIGN_CENTER, iTextSharp.text.Color.BLACK) : myTableCol.AddCell(cell)
                phrase = New Phrase("Bunga", fntArial10N) : cell = PhraseCell(phrase, PdfPCell.ALIGN_CENTER, iTextSharp.text.Color.BLACK) : myTableCol.AddCell(cell)
                phrase = New Phrase("Pokok", fntArial10N) : cell = PhraseCell(phrase, PdfPCell.ALIGN_CENTER, iTextSharp.text.Color.BLACK) : myTableCol.AddCell(cell)
                phrase = New Phrase("Bunga", fntArial10N) : cell = PhraseCell(phrase, PdfPCell.ALIGN_CENTER, iTextSharp.text.Color.BLACK) : myTableCol.AddCell(cell)
                phrase = New Phrase("Pokok", fntArial10N) : cell = PhraseCell(phrase, PdfPCell.ALIGN_CENTER, iTextSharp.text.Color.BLACK) : myTableCol.AddCell(cell)
                phrase = New Phrase("Bunga", fntArial10N) : cell = PhraseCell(phrase, PdfPCell.ALIGN_CENTER, iTextSharp.text.Color.BLACK) : myTableCol.AddCell(cell)
                phrase = New Phrase("Pokok", fntArial10N) : cell = PhraseCell(phrase, PdfPCell.ALIGN_CENTER, iTextSharp.text.Color.BLACK) : myTableCol.AddCell(cell)
                phrase = New Phrase("Bunga", fntArial10N) : cell = PhraseCell(phrase, PdfPCell.ALIGN_CENTER, iTextSharp.text.Color.BLACK) : myTableCol.AddCell(cell)
                phrase = New Phrase("Pokok", fntArial10N) : cell = PhraseCell(phrase, PdfPCell.ALIGN_CENTER, iTextSharp.text.Color.BLACK) : myTableCol.AddCell(cell)
                phrase = New Phrase("Bunga", fntArial10N) : cell = PhraseCell(phrase, PdfPCell.ALIGN_CENTER, iTextSharp.text.Color.BLACK) : myTableCol.AddCell(cell)

                saldoawalpokok = 0
                saldoawalbunga = 0
                mutasimasukbunga = 0
                mutasimasukpokok = 0
                mutasikeluarpokok = 0
                mutasikeluarbunga = 0
                saldoakhirbunga = 0
                saldoakhirpokok = 0
                angsuranpokok = 0
                angsuranbunga = 0
                persentasepokok = 0
                persentasebunga = 0


                For z = 0 To dtHead.Rows.Count - 1
                    phrase = New Phrase(z + 1, fntTahoma) : cell = PhraseCell(phrase, PdfPCell.ALIGN_CENTER, iTextSharp.text.Color.BLACK) : cell.FixedHeight = 15.0F : myTableCol.AddCell(cell)
                    phrase = New Phrase(dtHead.Rows(z).Item("nama"), fntTahoma) : cell = PhraseCell(phrase, PdfPCell.ALIGN_LEFT, iTextSharp.text.Color.BLACK) : cell.FixedHeight = 15.0F : myTableCol.AddCell(cell)

                    'Saldo Awal Pokok
                    dtmasukpokok = GetDataTbl("select isnull(Sum(total),0) as Jumlah  from Tbljual where idcabang='" & dtHead.Rows(z).Item("idCabang") & "' And  idunit= '" & dtHead.Rows(z).Item("idUnit") & "' and Statusangsuran='" & dtHead.Rows(z).Item("Nama") & "' and Tanggal<'" & clsEnt.CDateME5(dtpTglawal.Value) & "'")
                    Dim dtmutasimasukpokokawal As DataTable = GetDataTbl("select isnull(Sum(Jumlah),0) as Jumlah  from TblTagihanNasabah where idcabang='" & dtHead.Rows(z).Item("idCabang") & "' And  idunit= '" & dtHead.Rows(z).Item("idUnit") & "' and tgltagihan<'" & clsEnt.CDateME5(dtpTglawal.Value) & "'")
                    Dim dtmasukpokokangsuran As DataTable = GetDataTbl("select isnull(Sum(Jumlahpokok),0) as Jumlah  from TblAngsuran where idcabang='" & dtHead.Rows(z).Item("idCabang") & "' And  idunit= '" & dtHead.Rows(z).Item("idUnit") & "' and Statusangsuran='" & dtHead.Rows(z).Item("Nama") & "' and Tanggal<'" & clsEnt.CDateME5(dtpTglawal.Value) & "'")
                    Dim totalpokoklalu As Integer = dtmasukpokok.Rows(0).Item("Jumlah") - dtmutasimasukpokokawal.Rows(0).Item("Jumlah") - dtmasukpokokangsuran.Rows(0).Item("Jumlah")
                    phrase = New Phrase(FormatNumber(CDbl(totalpokoklalu), 0), fntTahoma) : cell = PhraseCell(phrase, PdfPCell.ALIGN_RIGHT, iTextSharp.text.Color.BLACK) : myTableCol.AddCell(cell)

                    'Saldo Awal Bunga
                    dtsaldoawalbunga = GetDataTbl("select isnull(Sum(bunga),0) as Jumlah  from Tbljual where idcabang='" & dtHead.Rows(z).Item("idCabang") & "' And  idunit= '" & dtHead.Rows(z).Item("idUnit") & "' and Statusangsuran='" & dtHead.Rows(z).Item("Nama") & "' and Tanggal<'" & clsEnt.CDateME5(dtpTglawal.Value) & "'")
                    Dim dtsaldoawalbungaangsuran As DataTable = GetDataTbl("select isnull(Sum(Jumlahbunga),0) as Jumlah  from TblAngsuran where idcabang='" & dtHead.Rows(z).Item("idCabang") & "' And  idunit= '" & dtHead.Rows(z).Item("idUnit") & "' and Statusangsuran='" & dtHead.Rows(z).Item("Nama") & "' and Tanggal<'" & clsEnt.CDateME5(dtpTglawal.Value) & "'")
                    Dim totalbungalalu = dtsaldoawalbunga.Rows(0).Item("Jumlah") - dtsaldoawalbungaangsuran.Rows(0).Item("Jumlah")
                    phrase = New Phrase(FormatNumber(CDbl(totalbungalalu), 0), fntTahoma) : cell = PhraseCell(phrase, PdfPCell.ALIGN_RIGHT, iTextSharp.text.Color.BLACK) : myTableCol.AddCell(cell)

                    'Mutasi Masuk Pokok 
                    dtmutasimasukpokok = GetDataTbl("select isnull(Sum(TOTAL),0) as Jumlah  from Tbljual where idcabang='" & dtHead.Rows(z).Item("idCabang") & "' And  idunit= '" & dtHead.Rows(z).Item("idUnit") & "' and Statusangsuran='" & dtHead.Rows(z).Item("Nama") & "' and Tanggal between '" & clsEnt.CDateME5(dtpTglawal.Value) & "' And '" & clsEnt.CDateME5(dtpTglakhir.Value) & "'")
                    'Mutasi Masuk bunga 
                    dtmutasimasukbunga = GetDataTbl("select isnull(Sum(Bunga),0) as Jumlah  from Tbljual where idcabang='" & dtHead.Rows(z).Item("idCabang") & "' And  idunit= '" & dtHead.Rows(z).Item("idUnit") & "' and Statusangsuran='" & dtHead.Rows(z).Item("Nama") & "' and Tanggal between '" & clsEnt.CDateME5(dtpTglawal.Value) & "' And '" & clsEnt.CDateME5(dtpTglakhir.Value) & "'")

                    phrase = New Phrase(FormatNumber(CDbl(dtmutasimasukpokok.Rows(0).Item("Jumlah")), 0), fntTahoma) : cell = PhraseCell(phrase, PdfPCell.ALIGN_RIGHT, iTextSharp.text.Color.BLACK) : myTableCol.AddCell(cell)

                    phrase = New Phrase(FormatNumber(CDbl(dtmutasimasukbunga.Rows(0).Item("Jumlah")), 0), fntTahoma) : cell = PhraseCell(phrase, PdfPCell.ALIGN_RIGHT, iTextSharp.text.Color.BLACK) : myTableCol.AddCell(cell)


                    'Mutasi keluar Pokok 
                    dtmutasikeluarpokok = GetDataTbl("select isnull(Sum(JumlahPokok),0) as Jumlah  from TblTagihanNasabah where idcabang='" & dtHead.Rows(z).Item("idCabang") & "' And  idunit= '" & dtHead.Rows(z).Item("idUnit") & "' and Statusangsuran='" & dtHead.Rows(z).Item("Nama") & "' and TglTagihan between '" & clsEnt.CDateME5(dtpTglawal.Value) & "' And '" & clsEnt.CDateME5(dtpTglakhir.Value) & "'")
                    Dim dtmutasikeluarpokok2 As DataTable = GetDataTbl("select isnull(Sum(JumlahPokok),0) as Jumlah  from TblTagihanNasabah where idcabang='" & dtHead.Rows(z).Item("idCabang") & "' And  idunit= '" & dtHead.Rows(z).Item("idUnit") & "' and Statusangsuran='" & dtHead.Rows(z).Item("Nama") & "' and TglTagihan between '" & clsEnt.CDateME5(dtpTglawal.Value) & "' And '" & clsEnt.CDateME5(dtpTglakhir.Value) & "' and RealisasiAngsuran<>" & nol & "")
                    Dim TotalMutasiKeluarPokok As Integer = dtmutasikeluarpokok.Rows(0).Item("Jumlah") - dtmutasikeluarpokok2.Rows(0).Item("Jumlah")
                    'Mutasi Keluar bunga 
                    dtmutasikeluarbunga = GetDataTbl("select isnull(Sum(JumlahBunga),0) as Jumlah  from TblTagihanNasabah where idcabang='" & dtHead.Rows(z).Item("idCabang") & "' And  idunit= '" & dtHead.Rows(z).Item("idUnit") & "' and Statusangsuran='" & dtHead.Rows(z).Item("Nama") & "' and TglTagihan between '" & clsEnt.CDateME5(dtpTglawal.Value) & "' And '" & clsEnt.CDateME5(dtpTglakhir.Value) & "'")
                    Dim dtmutasikeluarbunga2 As DataTable = GetDataTbl("select isnull(Sum(JumlahBunga),0) as Jumlah  from TblTagihanNasabah where idcabang='" & dtHead.Rows(z).Item("idCabang") & "' And  idunit= '" & dtHead.Rows(z).Item("idUnit") & "' and Statusangsuran='" & dtHead.Rows(z).Item("Nama") & "' and TglTagihan between '" & clsEnt.CDateME5(dtpTglawal.Value) & "' And '" & clsEnt.CDateME5(dtpTglakhir.Value) & "' and RealisasiAngsuran<>" & nol & "")
                    Dim TotalMutasiKeluarBunga As Integer = dtmutasikeluarbunga.Rows(0).Item("Jumlah") - dtmutasikeluarbunga2.Rows(0).Item("Jumlah")

                    phrase = New Phrase(FormatNumber(TotalMutasiKeluarPokok, 0), fntTahoma) : cell = PhraseCell(phrase, PdfPCell.ALIGN_RIGHT, iTextSharp.text.Color.BLACK) : myTableCol.AddCell(cell)
                    phrase = New Phrase(FormatNumber(TotalMutasiKeluarBunga, 0), fntTahoma) : cell = PhraseCell(phrase, PdfPCell.ALIGN_RIGHT, iTextSharp.text.Color.BLACK) : myTableCol.AddCell(cell)

                    'Angsuran Pokok
                    dtangsuranpokok = GetDataTbl("select isnull(Sum(Jumlahpokok),0) as Jumlah  from Tblangsuran where idcabang='" & dtHead.Rows(z).Item("idCabang") & "' And  idunit= '" & dtHead.Rows(z).Item("idUnit") & "' and Statusangsuran='" & dtHead.Rows(z).Item("Nama") & "' and Tanggal between  '" & clsEnt.CDateME5(dtpTglawal.Value) & "' And '" & clsEnt.CDateME5(dtpTglakhir.Value) & "'")
                    phrase = New Phrase(FormatNumber(CDbl(dtangsuranpokok.Rows(0).Item("Jumlah")), 0), fntTahoma) : cell = PhraseCell(phrase, PdfPCell.ALIGN_RIGHT, iTextSharp.text.Color.BLACK) : myTableCol.AddCell(cell)
                    'Angsuran Bunga
                    dtangsuranbunga = GetDataTbl("select isnull(Sum(Jumlahbunga),0) as Jumlah  from Tblangsuran where idcabang='" & dtHead.Rows(z).Item("idCabang") & "' And  idunit= '" & dtHead.Rows(z).Item("idUnit") & "' and Statusangsuran='" & dtHead.Rows(z).Item("Nama") & "' And Tanggal between  '" & clsEnt.CDateME5(dtpTglawal.Value) & "' And '" & clsEnt.CDateME5(dtpTglakhir.Value) & "'")
                    phrase = New Phrase(FormatNumber(CDbl(dtangsuranbunga.Rows(0).Item("Jumlah")), 0), fntTahoma) : cell = PhraseCell(phrase, PdfPCell.ALIGN_RIGHT, iTextSharp.text.Color.BLACK) : myTableCol.AddCell(cell)

                    'Saldo Akhir Pokok

                    saldoakhirpokok = totalpokoklalu + dtmutasimasukpokok.Rows(0).Item("Jumlah") - dtangsuranpokok.Rows(0).Item("Jumlah")
                    Dim totalkul As Integer = dtmasukpokok.Rows(0).Item("Jumlah") + dtmutasimasukpokok.Rows(0).Item("Jumlah") - dtangsuranpokok.Rows(0).Item("Jumlah")
                    phrase = New Phrase(FormatNumber(CDbl(saldoakhirpokok), 0), fntTahoma) : cell = PhraseCell(phrase, PdfPCell.ALIGN_RIGHT, iTextSharp.text.Color.BLACK) : myTableCol.AddCell(cell)

                    'Saldo Akhir Bunga
                    '  saldoakhirbunga = saldoawalbunga + mutasimasukbunga - angsuranbunga
                    saldoakhirbunga = (totalbungalalu + CDbl(dtmutasimasukbunga.Rows(0).Item("Jumlah"))) - CDbl(dtangsuranbunga.Rows(0).Item("Jumlah"))

                    phrase = New Phrase(FormatNumber(CDbl(saldoakhirbunga), 0), fntTahoma) : cell = PhraseCell(phrase, PdfPCell.ALIGN_RIGHT, iTextSharp.text.Color.BLACK) : myTableCol.AddCell(cell)

                    '------------------------Persentas------------------------
                    Dim dtmasukpokokall1 As DataTable = GetDataTbl("select isnull(Sum(JumlahPokok),0) as Jumlah  from TblTagihanNasabah where idcabang='" & txtidcabang.Text & "' And  idunit= '" & txtidunit.Text & "'  and TglTagihan<'" & clsEnt.CDateME5(dtpTglawal.Value) & "'")
                    Dim masukpokokall1 As Integer = dtmasukpokokall1.Rows(0).Item("Jumlah")

                    Dim dtmasukbungaall1 As DataTable = GetDataTbl("select isnull(Sum(Jumlahbunga),0) as Jumlah  from TblTagihanNasabah where idcabang='" & txtidcabang.Text & "' And  idunit= '" & txtidunit.Text & "'  and TglTagihan<'" & clsEnt.CDateME5(dtpTglawal.Value) & "' ")
                    Dim masukbungaal1l As Integer = dtmasukbungaall1.Rows(0).Item("Jumlah")

                    Dim dtmutasipokokall1 As DataTable = GetDataTbl("select isnull(Sum(JumlahPokok),0) as Jumlah  from TblTagihanNasabah where idcabang='" & txtidcabang.Text & "' And  idunit= '" & txtidunit.Text & "'  and TglTagihan between '" & clsEnt.CDateME5(dtpTglawal.Value) & "' And '" & clsEnt.CDateME5(dtpTglakhir.Value) & "'")
                    Dim mutasipokokall1 As Integer = dtmutasipokokall1.Rows(0).Item("Jumlah")

                    Dim dtmutasibungaall1 As DataTable = GetDataTbl("select isnull(Sum(Jumlahbunga),0) as Jumlah  from TblTagihanNasabah where idcabang='" & txtidcabang.Text & "' And  idunit= '" & txtidunit.Text & "'  and TglTagihan between '" & clsEnt.CDateME5(dtpTglawal.Value) & "' And '" & clsEnt.CDateME5(dtpTglakhir.Value) & "'")
                    Dim mutasibungaall1 As Integer = dtmutasibungaall1.Rows(0).Item("Jumlah")

                    'Angsuran Pokok
                    Dim dtangsuranpokokall1 As DataTable = GetDataTbl("select isnull(Sum(Jumlahpokok),0) as Jumlah  from Tblangsuran where idcabang='" & txtidcabang.Text & "' And  idunit= '" & txtidunit.Text & "'  and tanggal between '" & clsEnt.CDateME5(dtpTglawal.Value) & "' And '" & clsEnt.CDateME5(dtpTglakhir.Value) & "'")
                    Dim Angsuranpokokall1 As Integer = dtangsuranpokokall1.Rows(0).Item("Jumlah")

                    ' Bunga
                    'Angsuran Bunga
                    Dim dtangsuranbungaall1 As DataTable = GetDataTbl("select isnull(Sum(Jumlahbunga),0) as Jumlah  from Tblangsuran where idcabang='" & txtidcabang.Text & "' And  idunit= '" & txtidunit.Text & "'  and tanggal between '" & clsEnt.CDateME5(dtpTglawal.Value) & "' And '" & clsEnt.CDateME5(dtpTglakhir.Value) & "'")
                    Dim Angsuranbungaall1 As Integer = dtangsuranbungaall1.Rows(0).Item("Jumlah")

                    Dim Saldoakhirpokokall1 As Integer = dtmasukpokokall1.Rows(0).Item("Jumlah") + dtmutasipokokall1.Rows(0).Item("Jumlah") - dtangsuranpokokall1.Rows(0).Item("Jumlah")
                    Dim Saldoakhirbungaall1 As Integer = dtmasukbungaall1.Rows(0).Item("Jumlah") + dtmutasibungaall1.Rows(0).Item("Jumlah") - dtangsuranbungaall1.Rows(0).Item("Jumlah")

                    Dim Totalpersentasepokok As String = Saldoakhirpokokall1 / totalkul * 100
                    Dim Totalpersentasebunga As String = Saldoakhirbungaall1 / saldoakhirbunga * 100

                    phrase = New Phrase(FormatNumber(CDbl(Totalpersentasepokok), 2), fntTahoma) : cell = PhraseCell(phrase, PdfPCell.ALIGN_RIGHT, iTextSharp.text.Color.BLACK) : myTableCol.AddCell(cell)
                    phrase = New Phrase(FormatNumber(CDbl(Totalpersentasebunga), 2), fntTahoma) : cell = PhraseCell(phrase, PdfPCell.ALIGN_RIGHT, iTextSharp.text.Color.BLACK) : myTableCol.AddCell(cell)



                    If z < dtHead.Rows.Count - 1 Then

                    End If
                Next z

                Dim dtmasukpokokall As DataTable = GetDataTbl("select isnull(Sum(total),0) as Jumlah  from Tbljual where idcabang='" & txtidcabang.Text & "' And  idunit= '" & txtidunit.Text & "' and Tanggal<'" & clsEnt.CDateME5(dtpTglawal.Value) & "'")
                Dim dtmasukpokokangsuranall As DataTable = GetDataTbl("select isnull(Sum(Jumlahpokok),0) as Jumlah  from TblAngsuran where idcabang='" & txtidcabang.Text & "' And  idunit= '" & txtidunit.Text & "' and Tanggal<'" & clsEnt.CDateME5(dtpTglawal.Value) & "'")
                Dim masukpokokall As Integer = dtmasukpokokall.Rows(0).Item("Jumlah") - dtmasukpokokangsuranall.Rows(0).Item("Jumlah")

                Dim dtmasukbungaall As DataTable = GetDataTbl("select isnull(Sum(bunga),0) as Jumlah  from Tbljual where idcabang='" & txtidcabang.Text & "' And  idunit= '" & txtidunit.Text & "'  and Tanggal<'" & clsEnt.CDateME5(dtpTglawal.Value) & "'")
                Dim dtmasukbungaangsuranall As DataTable = GetDataTbl("select isnull(Sum(Jumlahbunga),0) as Jumlah  from TblAngsuran where idcabang='" & txtidcabang.Text & "' And  idunit= '" & txtidunit.Text & "' and Tanggal<'" & clsEnt.CDateME5(dtpTglawal.Value) & "'")
                Dim masukbungaall As Integer = dtmasukbungaall.Rows(0).Item("Jumlah") - dtmasukbungaangsuranall.Rows(0).Item("Jumlah")

                'Total Mutasi Masuk Pokok 
                dtmutasimasukpokok = GetDataTbl("select isnull(Sum(TOTAL),0) as Jumlah  from Tbljual where idcabang='" & txtidcabang.Text & "' And  idunit= '" & txtidunit.Text & "' and Tanggal between '" & clsEnt.CDateME5(dtpTglawal.Value) & "' And '" & clsEnt.CDateME5(dtpTglakhir.Value) & "'")
                'Total Mutasi Masuk bunga 
                dtmutasimasukbunga = GetDataTbl("select isnull(Sum(Bunga),0) as Jumlah  from Tbljual where idcabang='" & txtidcabang.Text & "' And  idunit= '" & txtidunit.Text & "' and Tanggal between '" & clsEnt.CDateME5(dtpTglawal.Value) & "' And '" & clsEnt.CDateME5(dtpTglakhir.Value) & "'")
                Dim mutasipokokall As Integer = dtmutasimasukpokok.Rows(0).Item("Jumlah")
                Dim mutasibungaall As Integer = dtmutasimasukbunga.Rows(0).Item("Jumlah")

                'Mutasi keluar Pokok 
                Dim dtmutasikeluarpokokall As DataTable = GetDataTbl("select isnull(Sum(JumlahPokok),0) as Jumlah  from TblTagihanNasabah where idcabang='" & txtidcabang.Text & "' And  idunit= '" & txtidunit.Text & "' and TglTagihan between '" & clsEnt.CDateME5(dtpTglawal.Value) & "' And '" & clsEnt.CDateME5(dtpTglakhir.Value) & "' and RealisasiAngsuran=" & nol & " and statusPembayaran<>'" & KetLunas & "'")
                Dim mutasikeluarpokokall As Integer = dtmutasikeluarpokokall.Rows(0).Item("Jumlah")
                'Mutasi Keluar bunga 
                Dim dtmutasikeluarbungaall As DataTable = GetDataTbl("select isnull(Sum(JumlahBunga),0) as Jumlah  from TblTagihanNasabah where idcabang='" & txtidcabang.Text & "' And  idunit= '" & txtidunit.Text & "' and TglTagihan between '" & clsEnt.CDateME5(dtpTglawal.Value) & "' And '" & clsEnt.CDateME5(dtpTglakhir.Value) & "' and RealisasiAngsuran=" & nol & " and statusPembayaran<>'" & KetLunas & "'")
                Dim mutasikeluarbungaall As Integer = dtmutasikeluarbungaall.Rows(0).Item("Jumlah")

                'Angsuran Pokok
                Dim dtangsuranpokokall As DataTable = GetDataTbl("select isnull(Sum(Jumlahpokok),0) as Jumlah  from Tblangsuran where idcabang='" & txtidcabang.Text & "' And  idunit= '" & txtidunit.Text & "'  and tanggal between '" & clsEnt.CDateME5(dtpTglawal.Value) & "' And '" & clsEnt.CDateME5(dtpTglakhir.Value) & "'")
                Dim Angsuranpokokall As Integer = dtangsuranpokokall.Rows(0).Item("Jumlah")

                ' Bunga
                'Angsuran Bunga
                Dim dtangsuranbungaall As DataTable = GetDataTbl("select isnull(Sum(Jumlahbunga),0) as Jumlah  from Tblangsuran where idcabang='" & txtidcabang.Text & "' And  idunit= '" & txtidunit.Text & "'  and tanggal between '" & clsEnt.CDateME5(dtpTglawal.Value) & "' And '" & clsEnt.CDateME5(dtpTglakhir.Value) & "'")
                Dim Angsuranbungaall As Integer = dtangsuranbungaall.Rows(0).Item("Jumlah")

                Dim Saldoakhirpokokall As Integer = masukpokokall + dtmutasimasukpokok.Rows(0).Item("Jumlah") - dtangsuranpokokall.Rows(0).Item("Jumlah")
                Dim Saldoakhirbungaall As Integer = masukbungaall + dtmutasimasukbunga.Rows(0).Item("Jumlah") - dtangsuranbungaall.Rows(0).Item("Jumlah")

                phrase = New Phrase("Total ", fntTahoma)
                cell = PhraseCell(phrase, PdfPCell.ALIGN_CENTER, iTextSharp.text.Color.BLACK)
                cell.Border = PdfPCell.BOTTOM_BORDER Or PdfPCell.TOP_BORDER Or PdfPCell.LEFT_BORDER Or PdfPCell.RIGHT_BORDER
                cell.Colspan = 2
                myTableCol.AddCell(cell)

                phrase = New Phrase(FormatNumber(CDbl(masukpokokall), 0), fntTahoma)
                cell = PhraseCell(phrase, PdfPCell.ALIGN_CENTER, iTextSharp.text.Color.BLACK)
                cell.Border = PdfPCell.BOTTOM_BORDER Or PdfPCell.TOP_BORDER Or PdfPCell.LEFT_BORDER Or PdfPCell.RIGHT_BORDER
                cell.Colspan = 1
                myTableCol.AddCell(cell)

                phrase = New Phrase(FormatNumber(CDbl(masukbungaall), 0), fntTahoma)
                cell = PhraseCell(phrase, PdfPCell.ALIGN_CENTER, iTextSharp.text.Color.BLACK)
                cell.Border = PdfPCell.BOTTOM_BORDER Or PdfPCell.TOP_BORDER Or PdfPCell.LEFT_BORDER Or PdfPCell.RIGHT_BORDER
                cell.Colspan = 1
                myTableCol.AddCell(cell)

                phrase = New Phrase(FormatNumber(CDbl(mutasipokokall), 0), fntTahoma)
                cell = PhraseCell(phrase, PdfPCell.ALIGN_CENTER, iTextSharp.text.Color.BLACK)
                cell.Border = PdfPCell.BOTTOM_BORDER Or PdfPCell.TOP_BORDER Or PdfPCell.LEFT_BORDER Or PdfPCell.RIGHT_BORDER
                cell.Colspan = 1
                myTableCol.AddCell(cell)

                phrase = New Phrase(FormatNumber(CDbl(mutasibungaall), 0), fntTahoma)
                cell = PhraseCell(phrase, PdfPCell.ALIGN_CENTER, iTextSharp.text.Color.BLACK)
                cell.Border = PdfPCell.BOTTOM_BORDER Or PdfPCell.TOP_BORDER Or PdfPCell.LEFT_BORDER Or PdfPCell.RIGHT_BORDER
                cell.Colspan = 1
                myTableCol.AddCell(cell)

                phrase = New Phrase(FormatNumber(CDbl(mutasikeluarpokokall), 0), fntTahoma)
                cell = PhraseCell(phrase, PdfPCell.ALIGN_CENTER, iTextSharp.text.Color.BLACK)
                cell.Border = PdfPCell.BOTTOM_BORDER Or PdfPCell.TOP_BORDER Or PdfPCell.LEFT_BORDER Or PdfPCell.RIGHT_BORDER
                cell.Colspan = 1
                myTableCol.AddCell(cell)

                phrase = New Phrase(FormatNumber(CDbl(mutasikeluarbungaall), 0), fntTahoma)
                cell = PhraseCell(phrase, PdfPCell.ALIGN_CENTER, iTextSharp.text.Color.BLACK)
                cell.Border = PdfPCell.BOTTOM_BORDER Or PdfPCell.TOP_BORDER Or PdfPCell.LEFT_BORDER Or PdfPCell.RIGHT_BORDER
                cell.Colspan = 1
                myTableCol.AddCell(cell)

                phrase = New Phrase(FormatNumber(CDbl(Angsuranpokokall), 0), fntTahoma)
                cell = PhraseCell(phrase, PdfPCell.ALIGN_CENTER, iTextSharp.text.Color.BLACK)
                cell.Border = PdfPCell.BOTTOM_BORDER Or PdfPCell.TOP_BORDER Or PdfPCell.LEFT_BORDER Or PdfPCell.RIGHT_BORDER
                cell.Colspan = 1
                myTableCol.AddCell(cell)

                phrase = New Phrase(FormatNumber(CDbl(Angsuranbungaall), 0), fntTahoma)
                cell = PhraseCell(phrase, PdfPCell.ALIGN_CENTER, iTextSharp.text.Color.BLACK)
                cell.Border = PdfPCell.BOTTOM_BORDER Or PdfPCell.TOP_BORDER Or PdfPCell.LEFT_BORDER Or PdfPCell.RIGHT_BORDER
                cell.Colspan = 1
                myTableCol.AddCell(cell)

                phrase = New Phrase(FormatNumber(CDbl(Saldoakhirpokokall), 0), fntTahoma)
                cell = PhraseCell(phrase, PdfPCell.ALIGN_CENTER, iTextSharp.text.Color.BLACK)
                cell.Border = PdfPCell.BOTTOM_BORDER Or PdfPCell.TOP_BORDER Or PdfPCell.LEFT_BORDER Or PdfPCell.RIGHT_BORDER
                cell.Colspan = 1
                myTableCol.AddCell(cell)

                phrase = New Phrase(FormatNumber(CDbl(Saldoakhirbungaall), 0), fntTahoma)
                cell = PhraseCell(phrase, PdfPCell.ALIGN_CENTER, iTextSharp.text.Color.BLACK)
                cell.Border = PdfPCell.BOTTOM_BORDER Or PdfPCell.TOP_BORDER Or PdfPCell.LEFT_BORDER Or PdfPCell.RIGHT_BORDER
                cell.Colspan = 1
                myTableCol.AddCell(cell)

                phrase = New Phrase("", fntTahoma)
                cell = PhraseCell(phrase, PdfPCell.ALIGN_CENTER, iTextSharp.text.Color.BLACK)
                cell.Border = PdfPCell.BOTTOM_BORDER Or PdfPCell.TOP_BORDER Or PdfPCell.LEFT_BORDER Or PdfPCell.RIGHT_BORDER
                cell.Colspan = 1
                myTableCol.AddCell(cell)

                phrase = New Phrase("", fntTahoma)
                cell = PhraseCell(phrase, PdfPCell.ALIGN_CENTER, iTextSharp.text.Color.BLACK)
                cell.Border = PdfPCell.BOTTOM_BORDER Or PdfPCell.TOP_BORDER Or PdfPCell.LEFT_BORDER Or PdfPCell.RIGHT_BORDER
                cell.Colspan = 1
                myTableCol.AddCell(cell)






                phrase = New Phrase("", fntTahoma) : cell = PhraseCell(phrase, PdfPCell.ALIGN_LEFT, iTextSharp.text.Color.BLACK) : cell.Border = PdfCell.NO_BORDER : cell.Colspan = 20 : myTableCol.AddCell(cell)
                phrase = New Phrase("", fntTahoma) : cell = PhraseCell(phrase, PdfPCell.ALIGN_CENTER, iTextSharp.text.Color.BLACK) : cell.Border = PdfCell.NO_BORDER : cell.Colspan = 20 : myTableCol.AddCell(cell)
                phrase = New Phrase("", fntTahoma) : cell = PhraseCell(phrase, PdfPCell.ALIGN_CENTER, iTextSharp.text.Color.BLACK) : cell.Border = PdfCell.NO_BORDER : cell.Colspan = 20 : myTableCol.AddCell(cell)

                phrase = New Phrase("Cetak Tanggal", fntTahoma) : cell = PhraseCell(phrase, PdfPCell.ALIGN_LEFT, iTextSharp.text.Color.BLACK) : cell.Border = PdfCell.NO_BORDER : cell.Colspan = 4 : myTableCol.AddCell(cell)
                phrase = New Phrase(":", fntTahoma) : cell = PhraseCell(phrase, PdfPCell.ALIGN_LEFT, iTextSharp.text.Color.BLACK) : cell.Border = PdfCell.NO_BORDER : cell.Colspan = 1 : myTableCol.AddCell(cell)
                phrase = New Phrase(Format(Now, "dd/MM/yyyy"), fntTahoma) : cell = PhraseCell(phrase, PdfPCell.ALIGN_LEFT, iTextSharp.text.Color.BLACK) : cell.Border = PdfCell.NO_BORDER : cell.Colspan = 10 : myTableCol.AddCell(cell)
                phrase = New Phrase("", fntArial12N) : cell = PhraseCell(phrase, PdfPCell.ALIGN_LEFT, iTextSharp.text.Color.BLACK) : cell.Border = PdfCell.NO_BORDER : cell.Colspan = 5 : myTableCol.AddCell(cell)
                doc.Add(myTableCol)
                doc.Close()
                fs.Close()
            End Using

            Dim client As New System.Net.WebClient()
            Dim buffer As Byte() = client.DownloadData(filepath)

            If Not (buffer Is Nothing) Then
                Response.ContentType = "application/pdf"
                Response.AddHeader("content-length", buffer.Length.ToString())
                Response.BinaryWrite(buffer)
            End If

        End If
    End Sub

    Public Sub PrintSirkulasi(sender As Object, e As System.EventArgs)

    End Sub

    Public Sub GenerateSirkulasi(sender As Object, e As System.EventArgs)

        'Dim tglawal As Date = dtglangsuran.Value
        'Dim tglakhir As Date = dtgltempo.Value
        'Dim tglakhirkontrak As Date = dtgltempokontrak.Value
        Dim dtHead As DataTable
        Dim strUID As String = Trim(Session.Item("UserAuthentication").ToString)
        Dim StatusAnsuranNasabah As String = ""
        Dim Bataslancar1 As Integer = 1
        Dim Bataslancar2 As Integer = 30
        Dim Bataskuranglancar1 As Integer = 31
        Dim Bataskuranglancar2 As Integer = 60
        Dim Bataskuranglancartype1 As Integer = 32
        Dim Bataskuranglancartype2 As Integer = 61
        Dim Batasdiragukan1 As Integer = 61
        Dim Batasdiragukan2 As Integer = 90
        Dim Batasmacet1 As Integer = 91
        Dim Batasmacet2 As Integer = 120
        Dim Batastarik1 As Integer = 121
        Dim Batastarik2 As Integer = 4000

        dtHead = GetDataTbl("SELECT idtransJual, idNasabah, Noplat, TglPinjam, TglTagihan, Angsuranke, StatusAngsuran  " _
                            & " From TblTagihanNasabah Order by idtransJual Asc ")

        If dtHead.Rows.Count >= 1 Then

            For z = 0 To dtHead.Rows.Count - 1
                Dim sTrue5 As DataTable = GetDataTbl("select CONVERT(VARCHAR(10),tglpinjam,103) as tglpinjam, CONVERT(VARCHAR(10),Tgltagihan,103) as jatuhtempo from TblTagihanNasabah where idTransJual='" & dtHead.Rows(z).Item("idtransJual") & "' And idNasabah='" & dtHead.Rows(z).Item("idNasabah") & "' And angsuranke=" & dtHead.Rows(z).Item("angsuranke") & "")
                Dim tglpinjaman As Date = sTrue5.Rows(0).Item("tglpinjam")
                Dim jATUHtEMPO As Date = sTrue5.Rows(0).Item("jatuhtempo")

                Dim sTrue6 As DataTable = GetDataTbl("select Sum(total) as Jumlah from TblLiburSirkulasi where tanggal between '" & clsEnt.CDateME5(tglpinjaman) & "' And '" & clsEnt.CDateME5(dtpTglakhir.Value) & "'")
                Dim Jumlahlibur As Integer = sTrue6.Rows(0).Item("Jumlah")

                Dim tglakhirkontrak As Date
                tglakhirkontrak = dtpTglakhir.Value

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

                connectDB()
                mySqlCon.Open()
                mySqlCmd = New System.Data.OleDb.OleDbCommand()

                Dim Transaction = mySqlCon.BeginTransaction(IsolationLevel.ReadCommitted)
                mySqlCmd.Connection = mySqlCon
                mySqlCmd.Transaction = Transaction

                Try
                    mySqlCmd.CommandText = "UPDATE TblTagihanNasabah SET StatusAngsuran='" & StatusAnsuranNasabah & "', LastUpdate=Getdate(),UseridUpdate='" & strUID & "' WHERE idtransJual='" & dtHead.Rows(z).Item("idtransJual") & "' And idNasabah='" & dtHead.Rows(z).Item("idNasabah") & "' And AngsuranKe='" & dtHead.Rows(z).Item("AngsuranKe") & "' And Tgltagihan>='" & clsEnt.CDateME5(tglpinjaman) & "' And Tgltagihan<='" & clsEnt.CDateME5(dtpTglakhir.Value) & "';"
                    mySqlCmd.ExecuteNonQuery()

                    Transaction.Commit()
                    Session("Mode") = "Edit"
                    Dim successScript As String = "setTimeout(function(){ Swal.fire({ icon: 'success', title: 'Saved', showConfirmButton: false, showCancelButton: false, timer: 2000, timerProgressBar: true }); }, 100);"
                    ScriptManager.RegisterStartupScript(Me, Me.[GetType](), "Popup", successScript, True)


                Catch ex As Exception
                    Transaction.Rollback()
                    Dim successScript As String = "setTimeout(function(){ Swal.fire({ icon: 'error', title: 'Error updating data', showConfirmButton: false, showCancelButton: false, timer: 2000, timerProgressBar: true }); }, 100);"
                    ScriptManager.RegisterStartupScript(Me, Me.[GetType](), "Popup", successScript, True)
                Finally
                    mySqlCon.Close()
                    mySqlCon.Dispose()
                    mySqlCmd.Parameters.Clear()

                End Try


            Next z
        End If
    End Sub

    Public Sub GenerateLancar(sender As Object, e As System.EventArgs)

        'Dim tglawal As Date = dtglangsuran.Value
        'Dim tglakhir As Date = dtgltempo.Value
        'Dim tglakhirkontrak As Date = dtgltempokontrak.Value
        Dim dtHead As DataTable
        Dim strUID As String = Trim(Session.Item("UserAuthentication").ToString)
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
        Dim Batastarik2 As Integer = 4000
        Dim Lancarjaya As String = "LANCAR"
        Dim Nilai As Integer = 0

        dtHead = GetDataTbl("SELECT idtransJual, idNasabah, Noplat, TglPinjam, TglTagihan, Angsuranke, StatusAngsuran, RealisasiAngsuran  " _
                            & " From TblTagihanNasabah Order by idtransJual Asc ")

        If dtHead.Rows.Count >= 1 Then

            For z = 0 To dtHead.Rows.Count - 1
                Dim sTrue5 As DataTable = GetDataTbl("select CONVERT(VARCHAR(10),tglpinjam,103) as tglpinjam, CONVERT(VARCHAR(10),Tgltagihan,103) as jatuhtempo from TblTagihanNasabah where idTransJual='" & dtHead.Rows(z).Item("idtransJual") & "' And idNasabah='" & dtHead.Rows(z).Item("idNasabah") & "' And angsuranke='" & dtHead.Rows(z).Item("angsuranke") & "'")
                Dim tglpinjaman As Date = sTrue5.Rows(0).Item("tglpinjam")
                Dim jATUHtEMPO As Date = sTrue5.Rows(0).Item("jatuhtempo")

                Dim sTrue6 As DataTable = GetDataTbl("select Sum(total) as Jumlah from TblLiburSirkulasi where tanggal between '" & clsEnt.CDateME5(tglpinjaman) & "' And '" & clsEnt.CDateME5(dtpTglakhir.Value) & "'")
                Dim Jumlahlibur As Integer = sTrue6.Rows(0).Item("Jumlah")


                Dim tglakhirkontrak As Date
                tglakhirkontrak = clsEnt.CDateME5(dtpTglakhir.Value)

                Dim Selisihharikoontrak2 As Integer = CInt((tglakhirkontrak - tglpinjaman).TotalDays)

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

                connectDB()
                mySqlCon.Open()
                mySqlCmd = New System.Data.OleDb.OleDbCommand()

                Dim Transaction = mySqlCon.BeginTransaction(IsolationLevel.ReadCommitted)
                mySqlCmd.Connection = mySqlCon
                mySqlCmd.Transaction = Transaction

                Try
                    mySqlCmd.CommandText = "UPDATE TblTagihanNasabah SET StatusAngsuran='" & Lancarjaya & "', LastUpdate=Getdate(),UseridUpdate='" & strUID & "' WHERE idtransJual='" & dtHead.Rows(z).Item("idtransJual") & "' And idNasabah='" & dtHead.Rows(z).Item("idNasabah") & "' And AngsuranKe='" & dtHead.Rows(z).Item("AngsuranKe") & "' And Tgltagihan<='" & clsEnt.CDateME5(dtpTglakhir.Value) & "' And RealisasiAngsuran<>" & Nilai & ";"
                    mySqlCmd.ExecuteNonQuery()

                    Transaction.Commit()
                    Session("Mode") = "Edit"
                    Dim successScript As String = "setTimeout(function(){ Swal.fire({ icon: 'success', title: 'Saved', showConfirmButton: false, showCancelButton: false, timer: 2000, timerProgressBar: true }); }, 100);"
                    ScriptManager.RegisterStartupScript(Me, Me.[GetType](), "Popup", successScript, True)


                Catch ex As Exception
                    Transaction.Rollback()
                    Dim successScript As String = "setTimeout(function(){ Swal.fire({ icon: 'error', title: 'Error updating data', showConfirmButton: false, showCancelButton: false, timer: 2000, timerProgressBar: true }); }, 100);"
                    ScriptManager.RegisterStartupScript(Me, Me.[GetType](), "Popup", successScript, True)
                Finally
                    mySqlCon.Close()
                    mySqlCon.Dispose()
                    mySqlCmd.Parameters.Clear()

                End Try


            Next z
        End If
    End Sub

    Public Sub GenerateLancarJual(sender As Object, e As System.EventArgs)

        'Dim tglawal As Date = dtglangsuran.Value
        'Dim tglakhir As Date = dtgltempo.Value
        'Dim tglakhirkontrak As Date = dtgltempokontrak.Value
        Dim dtHead As DataTable
        Dim strUID As String = Trim(Session.Item("UserAuthentication").ToString)
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
        Dim Batastarik2 As Integer = 4000
        Dim Lancarjaya As String = "LANCAR"
        Dim Nilai As Integer = 0

        dtHead = GetDataTbl("SELECT idtransJual, idNasabah, Noplat, TglPinjam, TglTagihan, Angsuranke, StatusAngsuran, RealisasiAngsuran  " _
                            & " From TblTagihanNasabah Order by idtransJual Asc ")

        If dtHead.Rows.Count >= 1 Then

            For z = 0 To dtHead.Rows.Count - 1
                Dim sTrue5 As DataTable = GetDataTbl("select CONVERT(VARCHAR(10),tglpinjam,103) as tglpinjam, CONVERT(VARCHAR(10),Tgltagihan,103) as jatuhtempo from TblTagihanNasabah where idTransJual='" & dtHead.Rows(z).Item("idtransJual") & "' And idNasabah='" & dtHead.Rows(z).Item("idNasabah") & "' And angsuranke='" & dtHead.Rows(z).Item("angsuranke") & "'")
                Dim tglpinjaman As Date = sTrue5.Rows(0).Item("tglpinjam")
                Dim jATUHtEMPO As Date = sTrue5.Rows(0).Item("jatuhtempo")

                Dim sTrue6 As DataTable = GetDataTbl("select Sum(total) as Jumlah from TblLiburSirkulasi where tanggal between '" & clsEnt.CDateME5(tglpinjaman) & "' And '" & clsEnt.CDateME5(dtpTglakhir.Value) & "'")
                Dim Jumlahlibur As Integer = sTrue6.Rows(0).Item("Jumlah")

                Dim tglakhirkontrak As Date
                tglakhirkontrak = clsEnt.CDateME5(dtpTglakhir.Value)


                Dim Selisihharikoontrak2 As Integer = CInt((tglakhirkontrak - tglpinjaman).TotalDays)

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

                connectDB()
                mySqlCon.Open()
                mySqlCmd = New System.Data.OleDb.OleDbCommand()

                Dim Transaction = mySqlCon.BeginTransaction(IsolationLevel.ReadCommitted)
                mySqlCmd.Connection = mySqlCon
                mySqlCmd.Transaction = Transaction

                Try
                    mySqlCmd.CommandText = "UPDATE TblJual SET StatusAngsuran='" & StatusAnsuranNasabah & "', LastUpdate=Getdate(),UseridUpdate='" & strUID & "' WHERE idTrans='" & dtHead.Rows(z).Item("idtransJual") & "' And idNasabah='" & dtHead.Rows(z).Item("idNasabah") & "';"
                    mySqlCmd.ExecuteNonQuery()

                    Transaction.Commit()
                    Session("Mode") = "Edit"
                    Dim successScript As String = "setTimeout(function(){ Swal.fire({ icon: 'success', title: 'Saved', showConfirmButton: false, showCancelButton: false, timer: 2000, timerProgressBar: true }); }, 100);"
                    ScriptManager.RegisterStartupScript(Me, Me.[GetType](), "Popup", successScript, True)


                Catch ex As Exception
                    Transaction.Rollback()
                    Dim successScript As String = "setTimeout(function(){ Swal.fire({ icon: 'error', title: 'Error updating data', showConfirmButton: false, showCancelButton: false, timer: 2000, timerProgressBar: true }); }, 100);"
                    ScriptManager.RegisterStartupScript(Me, Me.[GetType](), "Popup", successScript, True)
                Finally
                    mySqlCon.Close()
                    mySqlCon.Dispose()
                    mySqlCmd.Parameters.Clear()

                End Try


            Next z
        End If
    End Sub

    Public Sub GenerateLancarTagihan(sender As Object, e As System.EventArgs)

        'Dim tglawal As Date = dtglangsuran.Value
        'Dim tglakhir As Date = dtgltempo.Value
        'Dim tglakhirkontrak As Date = dtgltempokontrak.Value
        Dim dtHead As DataTable
        Dim strUID As String = Trim(Session.Item("UserAuthentication").ToString)
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
        Dim Batastarik2 As Integer = 4000
        Dim Lancarjaya As String = "LANCAR"
        Dim Nilai As Integer = 0

        dtHead = GetDataTbl("SELECT idtransJual, idNasabah, Noplat, TglPinjam, TglTagihan, Angsuranke, StatusAngsuran, RealisasiAngsuran  " _
                            & " From TblTagihanNasabah Order by idtransJual Asc ")

        If dtHead.Rows.Count >= 1 Then

            For z = 0 To dtHead.Rows.Count - 1
                Dim sTrue7 As DataTable = GetDataTbl("select Max(idTrans) as Notrans from TblTagihanNasabah where idTransJual='" & dtHead.Rows(z).Item("idtransJual") & "' And idNasabah='" & dtHead.Rows(z).Item("idNasabah") & "' And StatusAngsuran='" & Lancarjaya & "'")
                Dim ketrans As String = sTrue7.Rows(0).Item("Notrans")

                Dim sTrue5 As DataTable = GetDataTbl("select CONVERT(VARCHAR(10),tglpinjam,103) as tglpinjam, CONVERT(VARCHAR(10),Tgltagihan,103) as jatuhtempo from TblTagihanNasabah where idTransJual='" & dtHead.Rows(z).Item("idtransJual") & "' And idNasabah='" & dtHead.Rows(z).Item("idNasabah") & "' And idTrans='" & ketrans & "'")
                Dim tglpinjaman As Date = sTrue5.Rows(0).Item("tglpinjam")
                Dim jATUHtEMPO As Date = sTrue5.Rows(0).Item("jatuhtempo")

                Dim sTrue6 As DataTable = GetDataTbl("select Sum(total) as Jumlah from TblLiburSirkulasi where tanggal between '" & clsEnt.CDateME5(tglpinjaman) & "' And '" & clsEnt.CDateME5(dtpTglakhir.Value) & "'")
                Dim Jumlahlibur As Integer = sTrue6.Rows(0).Item("Jumlah")


                Dim tglakhirkontrak As Date
                tglakhirkontrak = clsEnt.CDateME5(dtpTglakhir.Value)

                Dim Selisihharikoontrak2 As Integer = CInt((tglakhirkontrak - jATUHtEMPO).TotalDays)

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

                connectDB()
                mySqlCon.Open()
                mySqlCmd = New System.Data.OleDb.OleDbCommand()

                Dim Transaction = mySqlCon.BeginTransaction(IsolationLevel.ReadCommitted)
                mySqlCmd.Connection = mySqlCon
                mySqlCmd.Transaction = Transaction

                Try
                    mySqlCmd.CommandText = "UPDATE TblTagihanNasabah SET StatusAngsuran='" & Lancarjaya & "', LastUpdate=Getdate(),UseridUpdate='" & strUID & "' WHERE idtransJual='" & dtHead.Rows(z).Item("idtransJual") & "' And idNasabah='" & dtHead.Rows(z).Item("idNasabah") & "' And AngsuranKe='" & dtHead.Rows(z).Item("AngsuranKe") & "' And Tgltagihan<='" & clsEnt.CDateME5(dtpTglakhir.Value) & "' And RealisasiAngsuran<>" & Nilai & ";"
                    mySqlCmd.ExecuteNonQuery()

                    Transaction.Commit()
                    Session("Mode") = "Edit"
                    Dim successScript As String = "setTimeout(function(){ Swal.fire({ icon: 'success', title: 'Saved', showConfirmButton: false, showCancelButton: false, timer: 2000, timerProgressBar: true }); }, 100);"
                    ScriptManager.RegisterStartupScript(Me, Me.[GetType](), "Popup", successScript, True)


                Catch ex As Exception
                    Transaction.Rollback()
                    Dim successScript As String = "setTimeout(function(){ Swal.fire({ icon: 'error', title: 'Error updating data', showConfirmButton: false, showCancelButton: false, timer: 2000, timerProgressBar: true }); }, 100);"
                    ScriptManager.RegisterStartupScript(Me, Me.[GetType](), "Popup", successScript, True)
                Finally
                    mySqlCon.Close()
                    mySqlCon.Dispose()
                    mySqlCmd.Parameters.Clear()

                End Try


            Next z
        End If
    End Sub

    Public Sub GenerateLancarTagihanJual(sender As Object, e As System.EventArgs)

        'Dim tglawal As Date = dtglangsuran.Value
        'Dim tglakhir As Date = dtgltempo.Value
        'Dim tglakhirkontrak As Date = dtgltempokontrak.Value
        Dim dtHead As DataTable
        Dim strUID As String = Trim(Session.Item("UserAuthentication").ToString)
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
        Dim Batastarik2 As Integer = 4000
        Dim Lancarjaya As String = "LANCAR"
        Dim Nilai As Integer = 0

        dtHead = GetDataTbl("SELECT idtransJual, idNasabah, Noplat, TglPinjam, TglTagihan, Angsuranke, StatusAngsuran, RealisasiAngsuran  " _
                            & " From TblTagihanNasabah Order by idtransJual Asc ")

        If dtHead.Rows.Count >= 1 Then

            For z = 0 To dtHead.Rows.Count - 1
                Dim sTrue7 As DataTable = GetDataTbl("select Max(idTrans) as Notrans from TblTagihanNasabah where idTransJual='" & dtHead.Rows(z).Item("idtransJual") & "' And idNasabah='" & dtHead.Rows(z).Item("idNasabah") & "' And StatusAngsuran='" & Lancarjaya & "'")
                Dim ketrans As String = sTrue7.Rows(0).Item("Notrans")

                Dim sTrue5 As DataTable = GetDataTbl("select CONVERT(VARCHAR(10),tglpinjam,103) as tglpinjam, CONVERT(VARCHAR(10),Tgltagihan,103) as jatuhtempo from TblTagihanNasabah where idTransJual='" & dtHead.Rows(z).Item("idtransJual") & "' And idNasabah='" & dtHead.Rows(z).Item("idNasabah") & "' And idTrans='" & ketrans & "'")
                Dim tglpinjaman As Date = sTrue5.Rows(0).Item("tglpinjam")
                Dim jATUHtEMPO As Date = sTrue5.Rows(0).Item("jatuhtempo")

                Dim sTrue6 As DataTable = GetDataTbl("select Sum(total) as Jumlah from TblLiburSirkulasi where tanggal between '" & clsEnt.CDateME5(tglpinjaman) & "' And '" & clsEnt.CDateME5(dtpTglakhir.Value) & "'")
                Dim Jumlahlibur As Integer = sTrue6.Rows(0).Item("Jumlah")


                Dim tglakhirkontrak As Date
                tglakhirkontrak = clsEnt.CDateME5(dtpTglakhir.Value)

                Dim Selisihharikoontrak2 As Integer = CInt((tglakhirkontrak - jATUHtEMPO).TotalDays)

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

                connectDB()
                mySqlCon.Open()
                mySqlCmd = New System.Data.OleDb.OleDbCommand()

                Dim Transaction = mySqlCon.BeginTransaction(IsolationLevel.ReadCommitted)
                mySqlCmd.Connection = mySqlCon
                mySqlCmd.Transaction = Transaction

                Try
                    mySqlCmd.CommandText = "UPDATE TblJual SET StatusAngsuran='" & StatusAnsuranNasabah & "', LastUpdate=Getdate(),UseridUpdate='" & strUID & "' WHERE idTrans='" & dtHead.Rows(z).Item("idtransJual") & "' And idNasabah='" & dtHead.Rows(z).Item("idNasabah") & "';"

                    mySqlCmd.ExecuteNonQuery()

                    Transaction.Commit()
                    Session("Mode") = "Edit"
                    Dim successScript As String = "setTimeout(function(){ Swal.fire({ icon: 'success', title: 'Saved', showConfirmButton: false, showCancelButton: false, timer: 2000, timerProgressBar: true }); }, 100);"
                    ScriptManager.RegisterStartupScript(Me, Me.[GetType](), "Popup", successScript, True)


                Catch ex As Exception
                    Transaction.Rollback()
                    Dim successScript As String = "setTimeout(function(){ Swal.fire({ icon: 'error', title: 'Error updating data', showConfirmButton: false, showCancelButton: false, timer: 2000, timerProgressBar: true }); }, 100);"
                    ScriptManager.RegisterStartupScript(Me, Me.[GetType](), "Popup", successScript, True)
                Finally
                    mySqlCon.Close()
                    mySqlCon.Dispose()
                    mySqlCmd.Parameters.Clear()

                End Try


            Next z
        End If
    End Sub

    Public Sub UpdateTagihan(sender As Object, e As System.EventArgs)

        connectDB()
        mySqlCon.Open()
        mySqlCmd = New System.Data.OleDb.OleDbCommand()

        Dim Transaction = mySqlCon.BeginTransaction(IsolationLevel.ReadCommitted)
        mySqlCmd.Connection = mySqlCon
        mySqlCmd.Transaction = Transaction

        Try
            mySqlCmd.CommandType = CommandType.StoredProcedure
            mySqlCmd.CommandText = "USP_UpdateTagihanlancar"

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
