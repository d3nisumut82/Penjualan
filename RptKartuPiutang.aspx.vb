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

Partial Class RptKartuPiutang
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


        txtnama.Attributes.Add("Readonly", "Readonly")
        txtnopinjam.Attributes.Add("Readonly", "Readonly")
        txtmerk.Attributes.Add("Readonly", "Readonly")
        txtjenis.Attributes.Add("Readonly", "Readonly")
        txttype.Attributes.Add("Readonly", "Readonly")
        txtplat.Attributes.Add("Readonly", "Readonly")
        txtwarna.Attributes.Add("Readonly", "Readonly")
        txtkondisi.Attributes.Add("Readonly", "Readonly")
        txtharga.Attributes.Add("Readonly", "Readonly")
        txtdp.Attributes.Add("Readonly", "Readonly")
        txtangsuran.Attributes.Add("Readonly", "Readonly")
        txtjangka.Attributes.Add("Readonly", "Readonly")
        dtgl.Attributes.Add("Readonly", "Readonly")
        txtpokok.Attributes.Add("Readonly", "Readonly")
        txtalamat.Attributes.Add("Readonly", "Readonly")
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
        If String.IsNullOrEmpty(txtidnasabah.Text.Trim()) Then
            Dim successScript As String = "setTimeout(function(){ Swal.fire({ icon: 'warning', title: 'Id Nasabah Tidak Boleh Kosong', showConfirmButton: false, showCancelButton: false, timer: 2000, timerProgressBar: true }); }, 100);"
            ScriptManager.RegisterStartupScript(Me, Me.[GetType](), "Popup", successScript, True)
            Exit Sub
        ElseIf String.IsNullOrEmpty(txtnopinjam.Text.Trim()) Then
            Dim successScript As String = "setTimeout(function(){ Swal.fire({ icon: 'warning', title: 'Nomor Kontrak Tidak Boleh Kosong', showConfirmButton: false, showCancelButton: false, timer: 2000, timerProgressBar: true }); }, 100);"
            ScriptManager.RegisterStartupScript(Me, Me.[GetType](), "Popup", successScript, True)
            Exit Sub
        End If


        Dim totalsaldoawal As Integer = 0
        Dim realangsuranlancar As Integer = 0
        Dim realangsuranmacet As Integer = 0
        Dim totasaldoakhir As Integer = 0
        Dim mdbttotal As Integer = 0
        Dim mdbtsaldoawal As Integer = 0
        Dim mdbttotalangsuran As Integer = 0
        Dim keterangan As String = "Sudah Bayar"
        Dim dtHead As DataTable


        dtHead = GetDataTbl("SELECT idTransJual as idTransJual, idNasabah as idNasabah, angsuranke as angsuranke,  CONVERT(VARCHAR(10),a.TglTagihan,103)  as tgljatuhtempo, a.jumlah as jumlahangsuran,a.jumlahpokok as jumlahpokok, a.jumlahbunga as jumlahbunga  " _
                                & " From TblTagihanNasabah a  where a.idTransJual='" & txtnopinjam.Text & "'  Order by a.idtrans Asc ")


        Dim sepLine As iTextSharp.text.pdf.draw.LineSeparator = New iTextSharp.text.pdf.draw.LineSeparator()
        sepLine.LineWidth = 1
        sepLine.Percentage = 60
        sepLine.LineColor = New iTextSharp.text.Color(System.Drawing.Color.Black)
        Dim chunkLine As Chunk = New Chunk(sepLine)

        Dim paragrp As Paragraph = Nothing
        Dim phrase As Phrase = Nothing
        Dim cell As PdfPCell = Nothing
        Dim table As PdfPTable = Nothing
        Dim color__1 As iTextSharp.text.Color = Nothing
        Dim fntArial8 As iTextSharp.text.Font = FontFactory.GetFont("Arial", 8, iTextSharp.text.Font.NORMAL, iTextSharp.text.Color.BLACK)
        Dim fntTahoma As iTextSharp.text.Font = FontFactory.GetFont("Tahoma", 10, iTextSharp.text.Font.NORMAL, iTextSharp.text.Color.BLACK)
        Dim fntArial12N As iTextSharp.text.Font = FontFactory.GetFont("Arial", 8, iTextSharp.text.Font.NORMAL, iTextSharp.text.Color.BLACK)
        Dim fntArial12U As iTextSharp.text.Font = FontFactory.GetFont("Arial", 8, iTextSharp.text.Font.UNDERLINE, iTextSharp.text.Color.BLACK)
        Dim fntArial12B As iTextSharp.text.Font = FontFactory.GetFont("Arial", 8, iTextSharp.text.Font.BOLD, iTextSharp.text.Color.BLACK)
        'Dim fntArial12C As iTextSharp.text.Font = FontFactory.GetFont("Arial", 8, iTextSharp.text.Font.UNDEFINED, iTextSharp.text.Color.BLACK)

        Dim fntArial10N As iTextSharp.text.Font = FontFactory.GetFont("Arial", 12, iTextSharp.text.Font.NORMAL, iTextSharp.text.Color.BLACK)
        Dim fntArial10U As iTextSharp.text.Font = FontFactory.GetFont("Arial", 10, iTextSharp.text.Font.UNDERLINE, iTextSharp.text.Color.BLACK)
        Dim fntArial10B As iTextSharp.text.Font = FontFactory.GetFont("Arial", 18, iTextSharp.text.Font.BOLD, iTextSharp.text.Color.BLACK)
        Dim fntArial10Bw As iTextSharp.text.Font = FontFactory.GetFont("Arial", 10, iTextSharp.text.Font.BOLD, iTextSharp.text.Color.WHITE)
        Dim fntArial14U As iTextSharp.text.Font = FontFactory.GetFont("Arial", 14, iTextSharp.text.Font.NORMAL, iTextSharp.text.Color.BLACK)
        Dim fntArial10Bajar As iTextSharp.text.Font = FontFactory.GetFont("Arial", 14, iTextSharp.text.Font.BOLD, iTextSharp.text.Color.BLACK)
        Dim BaskervileOldFace36 As iTextSharp.text.Font = FontFactory.GetFont("Arial", 36, iTextSharp.text.Font.BOLD, iTextSharp.text.Color.BLACK)
        Dim BaskervileOldFace28 As iTextSharp.text.Font = FontFactory.GetFont("Arial", 24, iTextSharp.text.Font.BOLD, iTextSharp.text.Color.BLACK)
        Dim BaskervileOldFace As iTextSharp.text.Font = FontFactory.GetFont("Arial", 12, iTextSharp.text.Font.BOLD, iTextSharp.text.Color.BLACK)
        Dim tahoma As iTextSharp.text.Font = FontFactory.GetFont("Arial", 12, iTextSharp.text.Font.BOLD, iTextSharp.text.Color.BLACK)


        If dtHead.Rows.Count >= 1 Then
            'Dim myWidth As Single
            'Dim myHeight As Single
            Dim leftMargin As Single = 10
            Dim rightMargin As Single = 10
            Dim topMargin As Single = 5
            Dim bottomMargin As Single = 5
            Dim filepath As String = Server.MapPath("PDF/Kartu Piutang Nasabah" & Format(Now, "dd-MM-yyyy") & ".pdf")
            Using fs As New FileStream(Server.MapPath("PDF/Kartu Piutang Nasabah" & Format(Now, "dd-MM-yyyy") & ".pdf"), FileMode.Create)
                Dim doc As New iTextSharp.text.Document(iTextSharp.text.PageSize.LEGAL.Rotate, leftMargin, rightMargin, topMargin, bottomMargin)
                PdfWriter.GetInstance(doc, fs)
                doc.AddTitle("Kartu Piutang Nasabah")
                doc.Open()

                Dim sTrue5 As DataTable = GetDataTbl("select b.lokasi as lokasicabang, c.lokasi as lokasiunit from tbljual a inner join tblcabang b on b.IDCABANG=a.idcabang inner join tblunitkerja c on c.idunit=a.idunit  where idtrans='" & txtnopinjam.Text & "' And idnasabah='" & txtidnasabah.Text & "'")
                Dim akmp As String = sTrue5.Rows(0).Item("lokasicabang")
                Dim akmp2 As String = sTrue5.Rows(0).Item("lokasiunit")
                'If jATUHtEMPO > dtglangsuran.Value Then
                '    StatusAnsuranNasabah = "LANCAR"
                'Else
                '    StatusAnsuranNasabah = "MACET"
                'End If

                '  Dim akmp As String = "UNIVERSITAS QUALITY"
                'If Left(ketidkampus, 1) = "1" Then
                '    akmp = "UNIVERSITAS QUALITY BRASTAGI"
                'End If


                Dim uq As Paragraph = New Paragraph(akmp, BaskervileOldFace36)
                uq.Alignment = Element.ALIGN_CENTER
                doc.Add(uq)
                doc.Add(New Paragraph(" "))

                Dim uq2 As Paragraph = New Paragraph(akmp2, BaskervileOldFace28)
                uq2.Alignment = Element.ALIGN_CENTER
                doc.Add(uq2)
                doc.Add(New Paragraph(" "))

                paragrp = New Paragraph("KARTU PIUTANG NASABAH ", fntArial10B)
                paragrp.Alignment = Element.ALIGN_CENTER
                doc.Add(paragrp)
                doc.Add(New Paragraph(" "))


                Dim myReportHeaderTable As PdfPTable = New PdfPTable(9)
                Dim RptHeaderTblHdWidths As Single() = New Single(8) {}
                RptHeaderTblHdWidths(0) = 120.0F
                RptHeaderTblHdWidths(1) = 10.0F
                RptHeaderTblHdWidths(2) = 200.0F
                RptHeaderTblHdWidths(3) = 150.0F
                RptHeaderTblHdWidths(4) = 10.0F
                RptHeaderTblHdWidths(5) = 200.0F
                RptHeaderTblHdWidths(6) = 150.0F
                RptHeaderTblHdWidths(7) = 10.0F
                RptHeaderTblHdWidths(8) = 300.0F
                'myTable.LockedWidth = True
                myReportHeaderTable.SetWidths(RptHeaderTblHdWidths)
                myReportHeaderTable.HorizontalAlignment = Element.ALIGN_LEFT

                phrase = New Phrase("No Kontrak", tahoma) : cell = PhraseCell(phrase, PdfPCell.ALIGN_LEFT, iTextSharp.text.Color.WHITE) : cell.VerticalAlignment = Element.ALIGN_CENTER : myReportHeaderTable.AddCell(cell)
                phrase = New Phrase(":", tahoma) : cell = PhraseCell(phrase, PdfPCell.ALIGN_LEFT, iTextSharp.text.Color.WHITE) : cell.VerticalAlignment = Element.ALIGN_CENTER : myReportHeaderTable.AddCell(cell)
                phrase = New Phrase(txtnopinjam.Text, tahoma) : cell = PhraseCell(phrase, PdfPCell.ALIGN_LEFT, iTextSharp.text.Color.WHITE) : cell.VerticalAlignment = Element.ALIGN_CENTER : myReportHeaderTable.AddCell(cell)
                phrase = New Phrase("Nama ", tahoma) : cell = PhraseCell(phrase, PdfPCell.ALIGN_LEFT, iTextSharp.text.Color.WHITE) : cell.VerticalAlignment = Element.ALIGN_CENTER : myReportHeaderTable.AddCell(cell)
                phrase = New Phrase(":", tahoma) : cell = PhraseCell(phrase, PdfPCell.ALIGN_LEFT, iTextSharp.text.Color.WHITE) : cell.VerticalAlignment = Element.ALIGN_CENTER : myReportHeaderTable.AddCell(cell)
                phrase = New Phrase(txtnama.Text, tahoma) : cell = PhraseCell(phrase, PdfPCell.ALIGN_LEFT, iTextSharp.text.Color.WHITE) : cell.VerticalAlignment = Element.ALIGN_CENTER : myReportHeaderTable.AddCell(cell)
                phrase = New Phrase("Alamat ", tahoma) : cell = PhraseCell(phrase, PdfPCell.ALIGN_LEFT, iTextSharp.text.Color.WHITE) : cell.VerticalAlignment = Element.ALIGN_CENTER : myReportHeaderTable.AddCell(cell)
                phrase = New Phrase(":", tahoma) : cell = PhraseCell(phrase, PdfPCell.ALIGN_LEFT, iTextSharp.text.Color.WHITE) : cell.VerticalAlignment = Element.ALIGN_CENTER : myReportHeaderTable.AddCell(cell)
                phrase = New Phrase(txtalamat.Text, tahoma) : cell = PhraseCell(phrase, PdfPCell.ALIGN_LEFT, iTextSharp.text.Color.WHITE) : cell.VerticalAlignment = Element.ALIGN_CENTER : myReportHeaderTable.AddCell(cell)

                phrase = New Phrase("Jenis ", tahoma) : cell = PhraseCell(phrase, PdfPCell.ALIGN_LEFT, iTextSharp.text.Color.WHITE) : cell.VerticalAlignment = Element.ALIGN_CENTER : myReportHeaderTable.AddCell(cell)
                phrase = New Phrase(":", tahoma) : cell = PhraseCell(phrase, PdfPCell.ALIGN_LEFT, iTextSharp.text.Color.WHITE) : cell.VerticalAlignment = Element.ALIGN_CENTER : myReportHeaderTable.AddCell(cell)
                phrase = New Phrase(txtjenis.Text, tahoma) : cell = PhraseCell(phrase, PdfPCell.ALIGN_LEFT, iTextSharp.text.Color.WHITE) : cell.VerticalAlignment = Element.ALIGN_CENTER : myReportHeaderTable.AddCell(cell)
                phrase = New Phrase("Harga Nominal", tahoma) : cell = PhraseCell(phrase, PdfPCell.ALIGN_LEFT, iTextSharp.text.Color.WHITE) : cell.VerticalAlignment = Element.ALIGN_CENTER : myReportHeaderTable.AddCell(cell)
                phrase = New Phrase(":", tahoma) : cell = PhraseCell(phrase, PdfPCell.ALIGN_LEFT, iTextSharp.text.Color.WHITE) : cell.VerticalAlignment = Element.ALIGN_CENTER : myReportHeaderTable.AddCell(cell)
                phrase = New Phrase(FormatNumber(CDbl(txtharga.Text), 0), tahoma) : cell = PhraseCell(phrase, PdfPCell.ALIGN_LEFT, iTextSharp.text.Color.WHITE) : cell.VerticalAlignment = Element.ALIGN_CENTER : myReportHeaderTable.AddCell(cell)
                phrase = New Phrase("Merk ", tahoma) : cell = PhraseCell(phrase, PdfPCell.ALIGN_LEFT, iTextSharp.text.Color.WHITE) : cell.VerticalAlignment = Element.ALIGN_CENTER : myReportHeaderTable.AddCell(cell)
                phrase = New Phrase(":", tahoma) : cell = PhraseCell(phrase, PdfPCell.ALIGN_LEFT, iTextSharp.text.Color.WHITE) : cell.VerticalAlignment = Element.ALIGN_CENTER : myReportHeaderTable.AddCell(cell)
                phrase = New Phrase(txtmerk.Text, tahoma) : cell = PhraseCell(phrase, PdfPCell.ALIGN_LEFT, iTextSharp.text.Color.WHITE) : cell.VerticalAlignment = Element.ALIGN_CENTER : myReportHeaderTable.AddCell(cell)

                phrase = New Phrase("DP", tahoma) : cell = PhraseCell(phrase, PdfPCell.ALIGN_LEFT, iTextSharp.text.Color.WHITE) : cell.VerticalAlignment = Element.ALIGN_CENTER : myReportHeaderTable.AddCell(cell)
                phrase = New Phrase(":", tahoma) : cell = PhraseCell(phrase, PdfPCell.ALIGN_LEFT, iTextSharp.text.Color.WHITE) : cell.VerticalAlignment = Element.ALIGN_CENTER : myReportHeaderTable.AddCell(cell)
                phrase = New Phrase(FormatNumber(CDbl(txtdp.Text), 0), tahoma) : cell = PhraseCell(phrase, PdfPCell.ALIGN_LEFT, iTextSharp.text.Color.WHITE) : cell.VerticalAlignment = Element.ALIGN_CENTER : myReportHeaderTable.AddCell(cell)
                phrase = New Phrase("Type ", tahoma) : cell = PhraseCell(phrase, PdfPCell.ALIGN_LEFT, iTextSharp.text.Color.WHITE) : cell.VerticalAlignment = Element.ALIGN_CENTER : myReportHeaderTable.AddCell(cell)
                phrase = New Phrase(":", tahoma) : cell = PhraseCell(phrase, PdfPCell.ALIGN_LEFT, iTextSharp.text.Color.WHITE) : cell.VerticalAlignment = Element.ALIGN_CENTER : myReportHeaderTable.AddCell(cell)
                phrase = New Phrase(txttype.Text, tahoma) : cell = PhraseCell(phrase, PdfPCell.ALIGN_LEFT, iTextSharp.text.Color.WHITE) : cell.VerticalAlignment = Element.ALIGN_CENTER : myReportHeaderTable.AddCell(cell)
                phrase = New Phrase("Pokok Kredit ", tahoma) : cell = PhraseCell(phrase, PdfPCell.ALIGN_LEFT, iTextSharp.text.Color.WHITE) : cell.VerticalAlignment = Element.ALIGN_CENTER : myReportHeaderTable.AddCell(cell)
                phrase = New Phrase(":", tahoma) : cell = PhraseCell(phrase, PdfPCell.ALIGN_LEFT, iTextSharp.text.Color.WHITE) : cell.VerticalAlignment = Element.ALIGN_CENTER : myReportHeaderTable.AddCell(cell)
                phrase = New Phrase(FormatNumber(CDbl(txtpokok.Text), 0), tahoma) : cell = PhraseCell(phrase, PdfPCell.ALIGN_LEFT, iTextSharp.text.Color.WHITE) : cell.VerticalAlignment = Element.ALIGN_CENTER : myReportHeaderTable.AddCell(cell)

                phrase = New Phrase("Nomor Plat", tahoma) : cell = PhraseCell(phrase, PdfPCell.ALIGN_LEFT, iTextSharp.text.Color.WHITE) : cell.VerticalAlignment = Element.ALIGN_CENTER : myReportHeaderTable.AddCell(cell)
                phrase = New Phrase(":", tahoma) : cell = PhraseCell(phrase, PdfPCell.ALIGN_LEFT, iTextSharp.text.Color.WHITE) : cell.VerticalAlignment = Element.ALIGN_CENTER : myReportHeaderTable.AddCell(cell)
                phrase = New Phrase(txtplat.Text, tahoma) : cell = PhraseCell(phrase, PdfPCell.ALIGN_LEFT, iTextSharp.text.Color.WHITE) : cell.VerticalAlignment = Element.ALIGN_CENTER : myReportHeaderTable.AddCell(cell)
                phrase = New Phrase("Angsuran/Bulan ", tahoma) : cell = PhraseCell(phrase, PdfPCell.ALIGN_LEFT, iTextSharp.text.Color.WHITE) : cell.VerticalAlignment = Element.ALIGN_CENTER : myReportHeaderTable.AddCell(cell)
                phrase = New Phrase(":", tahoma) : cell = PhraseCell(phrase, PdfPCell.ALIGN_LEFT, iTextSharp.text.Color.WHITE) : cell.VerticalAlignment = Element.ALIGN_CENTER : myReportHeaderTable.AddCell(cell)
                phrase = New Phrase(FormatNumber(CDbl(txtangsuran.Text), 0), tahoma) : cell = PhraseCell(phrase, PdfPCell.ALIGN_LEFT, iTextSharp.text.Color.WHITE) : cell.VerticalAlignment = Element.ALIGN_CENTER : myReportHeaderTable.AddCell(cell)
                phrase = New Phrase("Warna  ", tahoma) : cell = PhraseCell(phrase, PdfPCell.ALIGN_LEFT, iTextSharp.text.Color.WHITE) : cell.VerticalAlignment = Element.ALIGN_CENTER : myReportHeaderTable.AddCell(cell)
                phrase = New Phrase(":", tahoma) : cell = PhraseCell(phrase, PdfPCell.ALIGN_LEFT, iTextSharp.text.Color.WHITE) : cell.VerticalAlignment = Element.ALIGN_CENTER : myReportHeaderTable.AddCell(cell)
                phrase = New Phrase(txtwarna.Text, tahoma) : cell = PhraseCell(phrase, PdfPCell.ALIGN_LEFT, iTextSharp.text.Color.WHITE) : cell.VerticalAlignment = Element.ALIGN_CENTER : myReportHeaderTable.AddCell(cell)

                phrase = New Phrase("Lama", tahoma) : cell = PhraseCell(phrase, PdfPCell.ALIGN_LEFT, iTextSharp.text.Color.WHITE) : cell.VerticalAlignment = Element.ALIGN_CENTER : myReportHeaderTable.AddCell(cell)
                phrase = New Phrase(":", tahoma) : cell = PhraseCell(phrase, PdfPCell.ALIGN_LEFT, iTextSharp.text.Color.WHITE) : cell.VerticalAlignment = Element.ALIGN_CENTER : myReportHeaderTable.AddCell(cell)
                phrase = New Phrase(txtjangka.Text, tahoma) : cell = PhraseCell(phrase, PdfPCell.ALIGN_LEFT, iTextSharp.text.Color.WHITE) : cell.VerticalAlignment = Element.ALIGN_CENTER : myReportHeaderTable.AddCell(cell)
                phrase = New Phrase("Kondisi ", tahoma) : cell = PhraseCell(phrase, PdfPCell.ALIGN_LEFT, iTextSharp.text.Color.WHITE) : cell.VerticalAlignment = Element.ALIGN_CENTER : myReportHeaderTable.AddCell(cell)
                phrase = New Phrase(":", tahoma) : cell = PhraseCell(phrase, PdfPCell.ALIGN_LEFT, iTextSharp.text.Color.WHITE) : cell.VerticalAlignment = Element.ALIGN_CENTER : myReportHeaderTable.AddCell(cell)
                phrase = New Phrase(txtkondisi.Text, tahoma) : cell = PhraseCell(phrase, PdfPCell.ALIGN_LEFT, iTextSharp.text.Color.WHITE) : cell.VerticalAlignment = Element.ALIGN_CENTER : myReportHeaderTable.AddCell(cell)
                phrase = New Phrase("Tgl.Pembayaran  ", tahoma) : cell = PhraseCell(phrase, PdfPCell.ALIGN_LEFT, iTextSharp.text.Color.WHITE) : cell.VerticalAlignment = Element.ALIGN_CENTER : myReportHeaderTable.AddCell(cell)
                phrase = New Phrase(":", tahoma) : cell = PhraseCell(phrase, PdfPCell.ALIGN_LEFT, iTextSharp.text.Color.WHITE) : cell.VerticalAlignment = Element.ALIGN_CENTER : myReportHeaderTable.AddCell(cell)
                phrase = New Phrase(txtwarna.Text, tahoma) : cell = PhraseCell(phrase, PdfPCell.ALIGN_LEFT, iTextSharp.text.Color.WHITE) : cell.VerticalAlignment = Element.ALIGN_CENTER : myReportHeaderTable.AddCell(cell)


                doc.Add(myReportHeaderTable)

                Dim myTableCol As PdfPTable = New PdfPTable(9)
                Dim sglTblHdWidthsCol As Single() = New Single(8) {}

                sglTblHdWidthsCol(0) = 1.0F 'No
                sglTblHdWidthsCol(1) = 2.0F 'tanggal jatuh tempo
                sglTblHdWidthsCol(2) = 2.0F 'nominal angsuran
                sglTblHdWidthsCol(3) = 2.0F 'nominal pokok
                sglTblHdWidthsCol(4) = 2.0F 'nominal bunga
                sglTblHdWidthsCol(5) = 2.0F 'Saldo akhir pokok
                sglTblHdWidthsCol(6) = 2.0F 'Saldo Akhir pokok+bunga
                sglTblHdWidthsCol(7) = 2.0F 'Keterangan
                sglTblHdWidthsCol(8) = 2.0F 'Tanggal Bayar


                'myTable.LockedWidth = True
                myTableCol.HorizontalAlignment = Element.ALIGN_LEFT
                myTableCol.WidthPercentage = 100
                myTableCol.SetWidths(sglTblHdWidthsCol)

                'paragrp = New Paragraph(txtnamacabang.Text, fntArial10B)
                'paragrp.Alignment = Element.ALIGN_CENTER
                'doc.Add(paragrp)

                'paragrp = New Paragraph(txtnamaUnit.Text, fntArial10B)
                'paragrp.Alignment = Element.ALIGN_CENTER
                'doc.Add(paragrp)

                phrase = New Phrase("", fntTahoma) : cell = PhraseCell(phrase, PdfPCell.ALIGN_LEFT, iTextSharp.text.Color.BLACK) : cell.Border = PdfCell.NO_BORDER : cell.Colspan = 9 : myTableCol.AddCell(cell)
                phrase = New Phrase("", fntTahoma) : cell = PhraseCell(phrase, PdfPCell.ALIGN_CENTER, iTextSharp.text.Color.BLACK) : cell.Border = PdfCell.NO_BORDER : cell.Colspan = 9 : myTableCol.AddCell(cell)
                phrase = New Phrase("", fntTahoma) : cell = PhraseCell(phrase, PdfPCell.ALIGN_CENTER, iTextSharp.text.Color.BLACK) : cell.Border = PdfCell.NO_BORDER : cell.Colspan = 9 : myTableCol.AddCell(cell)

                'Rows 1
                phrase = New Phrase("", fntArial14U) : cell = PhraseCell(phrase, PdfPCell.ALIGN_CENTER, iTextSharp.text.Color.BLACK) : cell = PhraseCell(phrase, PdfPCell.ALIGN_CENTER, iTextSharp.text.Color.BLACK) : cell.VerticalAlignment = Element.ALIGN_CENTER : cell.Colspan = 1 : myTableCol.AddCell(cell)
                phrase = New Phrase("", fntArial14U) : cell = PhraseCell(phrase, PdfPCell.ALIGN_CENTER, iTextSharp.text.Color.BLACK) : cell = PhraseCell(phrase, PdfPCell.ALIGN_CENTER, iTextSharp.text.Color.BLACK) : cell.VerticalAlignment = Element.ALIGN_CENTER : cell.Colspan = 1 : myTableCol.AddCell(cell)
                phrase = New Phrase("Angsuran ", fntArial14U) : cell = PhraseCell(phrase, PdfPCell.ALIGN_CENTER, iTextSharp.text.Color.BLACK) : cell = PhraseCell(phrase, PdfPCell.ALIGN_CENTER, iTextSharp.text.Color.BLACK) : cell.VerticalAlignment = Element.ALIGN_CENTER : cell.Colspan = 3 : myTableCol.AddCell(cell)
                phrase = New Phrase("Saldo Piutang", fntArial14U) : cell = PhraseCell(phrase, PdfPCell.ALIGN_CENTER, iTextSharp.text.Color.BLACK) : cell = PhraseCell(phrase, PdfPCell.ALIGN_CENTER, iTextSharp.text.Color.BLACK) : cell.VerticalAlignment = Element.ALIGN_CENTER : cell.Colspan = 2 : myTableCol.AddCell(cell)
                phrase = New Phrase("", fntArial14U) : cell = PhraseCell(phrase, PdfPCell.ALIGN_CENTER, iTextSharp.text.Color.BLACK) : cell = PhraseCell(phrase, PdfPCell.ALIGN_CENTER, iTextSharp.text.Color.BLACK) : cell.VerticalAlignment = Element.ALIGN_CENTER : cell.Colspan = 1 : myTableCol.AddCell(cell)
                phrase = New Phrase("", fntArial14U) : cell = PhraseCell(phrase, PdfPCell.ALIGN_LEFT, iTextSharp.text.Color.BLACK) : cell = PhraseCell(phrase, PdfPCell.ALIGN_CENTER, iTextSharp.text.Color.BLACK) : cell.VerticalAlignment = Element.ALIGN_CENTER : cell.Colspan = 1 : myTableCol.AddCell(cell)


                ''Header Tabel
                phrase = New Phrase("No", fntArial10N) : cell = PhraseCell(phrase, PdfPCell.ALIGN_CENTER, iTextSharp.text.Color.BLACK) : cell.FixedHeight = 10.0F : myTableCol.AddCell(cell)
                phrase = New Phrase("Jatuh Tempo ", fntArial10N) : cell = PhraseCell(phrase, PdfPCell.ALIGN_CENTER, iTextSharp.text.Color.BLACK) : myTableCol.AddCell(cell)
                phrase = New Phrase("Nominal ", fntArial10N) : cell = PhraseCell(phrase, PdfPCell.ALIGN_CENTER, iTextSharp.text.Color.BLACK) : myTableCol.AddCell(cell)
                phrase = New Phrase("Pokok ", fntArial10N) : cell = PhraseCell(phrase, PdfPCell.ALIGN_CENTER, iTextSharp.text.Color.BLACK) : myTableCol.AddCell(cell)
                phrase = New Phrase("Bunga ", fntArial10N) : cell = PhraseCell(phrase, PdfPCell.ALIGN_CENTER, iTextSharp.text.Color.BLACK) : myTableCol.AddCell(cell)
                phrase = New Phrase("Pokok", fntArial10N) : cell = PhraseCell(phrase, PdfPCell.ALIGN_CENTER, iTextSharp.text.Color.BLACK) : myTableCol.AddCell(cell)
                phrase = New Phrase("Bruto ", fntArial10N) : cell = PhraseCell(phrase, PdfPCell.ALIGN_CENTER, iTextSharp.text.Color.BLACK) : myTableCol.AddCell(cell)
                phrase = New Phrase("Keterangan ", fntArial10N) : cell = PhraseCell(phrase, PdfPCell.ALIGN_CENTER, iTextSharp.text.Color.BLACK) : myTableCol.AddCell(cell)
                phrase = New Phrase("Tanggal Bayar ", fntArial10N) : cell = PhraseCell(phrase, PdfPCell.ALIGN_CENTER, iTextSharp.text.Color.BLACK) : myTableCol.AddCell(cell)

                For z = 0 To dtHead.Rows.Count - 1
                    phrase = New Phrase(z + 1, fntTahoma) : cell = PhraseCell(phrase, PdfPCell.ALIGN_CENTER, iTextSharp.text.Color.BLACK) : cell.FixedHeight = 15.0F : myTableCol.AddCell(cell)
                    phrase = New Phrase(dtHead.Rows(z).Item("tgljatuhtempo"), fntTahoma) : cell = PhraseCell(phrase, PdfPCell.ALIGN_LEFT, iTextSharp.text.Color.BLACK) : cell.FixedHeight = 15.0F : myTableCol.AddCell(cell)
                    phrase = New Phrase(FormatNumber(CDbl(dtHead.Rows(0).Item("Jumlahangsuran")), 0), fntTahoma) : cell = PhraseCell(phrase, PdfPCell.ALIGN_LEFT, iTextSharp.text.Color.BLACK) : cell.FixedHeight = 15.0F : myTableCol.AddCell(cell)
                    phrase = New Phrase(FormatNumber(CDbl(dtHead.Rows(0).Item("Jumlahpokok")), 0), fntTahoma) : cell = PhraseCell(phrase, PdfPCell.ALIGN_LEFT, iTextSharp.text.Color.BLACK) : cell.FixedHeight = 15.0F : myTableCol.AddCell(cell)
                    phrase = New Phrase(FormatNumber(CDbl(dtHead.Rows(z).Item("Jumlahbunga")), 0), fntTahoma) : cell = PhraseCell(phrase, PdfPCell.ALIGN_LEFT, iTextSharp.text.Color.BLACK) : cell.FixedHeight = 15.0F : myTableCol.AddCell(cell)

                    'Saldo Akhir Pokok
                    Dim dtangsuranpokokall As DataTable = GetDataTbl("select isnull(Sum(saldoakhirpokok),0) as Jumlah  from Tblangsuran where nopinjaman='" & dtHead.Rows(z).Item("idtransJual") & "' And idNasabah='" & dtHead.Rows(z).Item("idNasabah") & "' And angsuranke=" & dtHead.Rows(z).Item("angsuranke") & "")
                    Dim dtangsuransaldoakhirall As DataTable = GetDataTbl("select isnull(Sum(saldoakhir),0) as Jumlah  from Tblangsuran where nopinjaman='" & dtHead.Rows(z).Item("idtransJual") & "' And idNasabah='" & dtHead.Rows(z).Item("idNasabah") & "' And angsuranke=" & dtHead.Rows(z).Item("angsuranke") & "")

                    phrase = New Phrase(FormatNumber(CDbl(dtangsuranpokokall.Rows(0).Item("Jumlah")), 0), fntTahoma) : cell = PhraseCell(phrase, PdfPCell.ALIGN_LEFT, iTextSharp.text.Color.BLACK) : cell.FixedHeight = 15.0F : myTableCol.AddCell(cell)
                    phrase = New Phrase(FormatNumber(CDbl(dtangsuransaldoakhirall.Rows(0).Item("Jumlah")), 0), fntTahoma) : cell = PhraseCell(phrase, PdfPCell.ALIGN_LEFT, iTextSharp.text.Color.BLACK) : cell.FixedHeight = 15.0F : myTableCol.AddCell(cell)

                    Dim sTruebayar2 As DataTable = GetDataTbl("select Count(nopinjaman) as no from Tblangsuran where nopinjaman='" & dtHead.Rows(z).Item("idtransJual") & "' And idNasabah='" & dtHead.Rows(z).Item("idNasabah") & "' And angsuranke=" & dtHead.Rows(z).Item("angsuranke") & "")
                    If sTruebayar2.Rows.Count > 0 Then
                        If CInt(sTruebayar2.Rows(0).Item("no")) > 0 Then
                            Dim sTruebayar As DataTable = GetDataTbl("select CONVERT(VARCHAR(10),tanggal,103) as tanggal, isnull(keterangan,'') as ket from Tblangsuran where nopinjaman='" & dtHead.Rows(z).Item("idtransJual") & "' And idNasabah='" & dtHead.Rows(z).Item("idNasabah") & "' And angsuranke=" & dtHead.Rows(z).Item("angsuranke") & "")
                            phrase = New Phrase(sTruebayar.Rows(0).Item("ket"), fntTahoma) : cell = PhraseCell(phrase, PdfPCell.ALIGN_LEFT, iTextSharp.text.Color.BLACK) : cell.FixedHeight = 15.0F : myTableCol.AddCell(cell)
                        Else
                            phrase = New Phrase("", fntTahoma) : cell = PhraseCell(phrase, PdfPCell.ALIGN_CENTER, iTextSharp.text.Color.BLACK) : myTableCol.AddCell(cell)
                        End If
                    End If

                    Dim sTruebayar23 As DataTable = GetDataTbl("select Count(nopinjaman) as no from Tblangsuran where nopinjaman='" & dtHead.Rows(z).Item("idtransJual") & "' And idNasabah='" & dtHead.Rows(z).Item("idNasabah") & "' And angsuranke=" & dtHead.Rows(z).Item("angsuranke") & "")
                    If sTruebayar23.Rows.Count > 0 Then
                        If CInt(sTruebayar23.Rows(0).Item("no")) > 0 Then
                            Dim sTruebayar As DataTable = GetDataTbl("select CONVERT(VARCHAR(10),tanggal,103) as tanggal, isnull(keterangan,'') as ket from Tblangsuran where nopinjaman='" & dtHead.Rows(z).Item("idtransJual") & "' And idNasabah='" & dtHead.Rows(z).Item("idNasabah") & "' And angsuranke=" & dtHead.Rows(z).Item("angsuranke") & "")
                            phrase = New Phrase(sTruebayar.Rows(0).Item("tanggal"), fntTahoma) : cell = PhraseCell(phrase, PdfPCell.ALIGN_LEFT, iTextSharp.text.Color.BLACK) : cell.FixedHeight = 15.0F : myTableCol.AddCell(cell)
                        Else
                            phrase = New Phrase("", fntTahoma) : cell = PhraseCell(phrase, PdfPCell.ALIGN_CENTER, iTextSharp.text.Color.BLACK) : myTableCol.AddCell(cell)
                        End If
                    End If

                    'phrase = New Phrase(FormatNumber(CDbl(dtHead.Rows(z).Item("jumlahangsuran")), 0), fntTahoma) : cell = PhraseCell(phrase, PdfPCell.ALIGN_CENTER, iTextSharp.text.Color.BLACK) : cell.FixedHeight = 15.0F : myTableCol.AddCell(cell)
                    'phrase = New Phrase(FormatNumber(CDbl(dtHead.Rows(z).Item("jumlahpokok")), 0), fntTahoma) : cell = PhraseCell(phrase, PdfPCell.ALIGN_CENTER, iTextSharp.text.Color.BLACK) : cell.FixedHeight = 15.0F : myTableCol.AddCell(cell)
                    'phrase = New Phrase(FormatNumber(CDbl(dtHead.Rows(z).Item("jumlahbunga")), 0), fntTahoma) : cell = PhraseCell(phrase, PdfPCell.ALIGN_CENTER, iTextSharp.text.Color.BLACK) : cell.FixedHeight = 15.0F : myTableCol.AddCell(cell)
                    'phrase = New Phrase(FormatNumber(CDbl(dtHead.Rows(z).Item("saldoakhirpokok")), 0), fntTahoma) : cell = PhraseCell(phrase, PdfPCell.ALIGN_CENTER, iTextSharp.text.Color.BLACK) : cell.FixedHeight = 15.0F : myTableCol.AddCell(cell)
                    'phrase = New Phrase(FormatNumber(CDbl(dtHead.Rows(z).Item("saldoakhir")), 0), fntTahoma) : cell = PhraseCell(phrase, PdfPCell.ALIGN_CENTER, iTextSharp.text.Color.BLACK) : cell.FixedHeight = 15.0F : myTableCol.AddCell(cell)
                    'phrase = New Phrase("Sudah Bayar", fntTahoma) : cell = PhraseCell(phrase, PdfPCell.ALIGN_LEFT, iTextSharp.text.Color.BLACK) : cell.FixedHeight = 15.0F : myTableCol.AddCell(cell)
                    'phrase = New Phrase(dtHead.Rows(z).Item("Tanggal"), fntTahoma) : cell = PhraseCell(phrase, PdfPCell.ALIGN_LEFT, iTextSharp.text.Color.BLACK) : cell.FixedHeight = 15.0F : myTableCol.AddCell(cell)

                    If z < dtHead.Rows.Count - 1 Then

                    End If
                Next z
                Dim sTruesaldoawal As DataTable = GetDataTbl("select isnull(Sum(jumlah),0) as jumlah from TblTagihanNasabah where idTransjual='" & txtnopinjam.Text & "' And idNasabah='" & txtidnasabah.Text & "'")
                Dim Jumlahsaldoawal As Integer = sTruesaldoawal.Rows(0).Item("jumlah")
                Dim sTruesaldopokok As DataTable = GetDataTbl("select isnull(Sum(jumlahpokok),0) as jumlah from TblTagihanNasabah where idTransjual='" & txtnopinjam.Text & "' And idNasabah='" & txtidnasabah.Text & "'")
                Dim Jumlahsaldoakhirpokok As Integer = sTruesaldopokok.Rows(0).Item("jumlah")
                Dim sTruesaldobunga As DataTable = GetDataTbl("select isnull(Sum(jumlahbunga),0) as jumlah from TblTagihanNasabah where idTransjual='" & txtnopinjam.Text & "' And idNasabah='" & txtidnasabah.Text & "'")
                Dim Jumlahsaldoakhirbunga As Integer = sTruesaldobunga.Rows(0).Item("jumlah")
                phrase = New Phrase("TOTAL : ", fntTahoma)
                cell = PhraseCell(phrase, PdfPCell.ALIGN_CENTER, iTextSharp.text.Color.BLACK)
                cell.Border = PdfPCell.BOTTOM_BORDER Or PdfPCell.TOP_BORDER Or PdfPCell.LEFT_BORDER Or PdfPCell.RIGHT_BORDER
                cell.Colspan = 2
                myTableCol.AddCell(cell)
                phrase = New Phrase(FormatNumber(CDbl(Jumlahsaldoawal), 0), fntTahoma)
                cell = PhraseCell(phrase, PdfPCell.ALIGN_CENTER, iTextSharp.text.Color.BLACK)
                cell.Border = PdfPCell.BOTTOM_BORDER Or PdfPCell.TOP_BORDER Or PdfPCell.LEFT_BORDER Or PdfPCell.RIGHT_BORDER
                cell.Colspan = 1
                myTableCol.AddCell(cell)
                phrase = New Phrase(FormatNumber(CDbl(Jumlahsaldoakhirpokok), 0), fntTahoma)
                cell = PhraseCell(phrase, PdfPCell.ALIGN_CENTER, iTextSharp.text.Color.BLACK)
                cell.Border = PdfPCell.BOTTOM_BORDER Or PdfPCell.TOP_BORDER Or PdfPCell.LEFT_BORDER Or PdfPCell.RIGHT_BORDER
                cell.Colspan = 1
                myTableCol.AddCell(cell)
                phrase = New Phrase(FormatNumber(CDbl(Jumlahsaldoakhirbunga), 0), fntTahoma)
                cell = PhraseCell(phrase, PdfPCell.ALIGN_CENTER, iTextSharp.text.Color.BLACK)
                cell.Border = PdfPCell.BOTTOM_BORDER Or PdfPCell.TOP_BORDER Or PdfPCell.LEFT_BORDER Or PdfPCell.RIGHT_BORDER
                cell.Colspan = 1
                myTableCol.AddCell(cell)
                phrase = New Phrase("Suku Rate Bunga Pertahun : 3.5% ", fntTahoma)
                cell = PhraseCell(phrase, PdfPCell.ALIGN_CENTER, iTextSharp.text.Color.BLACK)
                cell.Border = PdfPCell.BOTTOM_BORDER Or PdfPCell.TOP_BORDER Or PdfPCell.LEFT_BORDER Or PdfPCell.RIGHT_BORDER
                cell.Colspan = 4
                myTableCol.AddCell(cell)



                phrase = New Phrase("", fntTahoma) : cell = PhraseCell(phrase, PdfPCell.ALIGN_LEFT, iTextSharp.text.Color.BLACK) : cell.Border = PdfCell.NO_BORDER : cell.Colspan = 20 : myTableCol.AddCell(cell)
                phrase = New Phrase("", fntTahoma) : cell = PhraseCell(phrase, PdfPCell.ALIGN_CENTER, iTextSharp.text.Color.BLACK) : cell.Border = PdfCell.NO_BORDER : cell.Colspan = 20 : myTableCol.AddCell(cell)
                phrase = New Phrase("", fntTahoma) : cell = PhraseCell(phrase, PdfPCell.ALIGN_CENTER, iTextSharp.text.Color.BLACK) : cell.Border = PdfCell.NO_BORDER : cell.Colspan = 20 : myTableCol.AddCell(cell)

                phrase = New Phrase("Cetak Tanggal", fntTahoma) : cell = PhraseCell(phrase, PdfPCell.ALIGN_LEFT, iTextSharp.text.Color.BLACK) : cell.Border = PdfCell.NO_BORDER : cell.Colspan = 4 : myTableCol.AddCell(cell)
                phrase = New Phrase(":", fntTahoma) : cell = PhraseCell(phrase, PdfPCell.ALIGN_LEFT, iTextSharp.text.Color.BLACK) : cell.Border = PdfCell.NO_BORDER : cell.Colspan = 1 : myTableCol.AddCell(cell)
                phrase = New Phrase(Format(Now, "dd/MM/yyyy"), fntTahoma) : cell = PhraseCell(phrase, PdfPCell.ALIGN_LEFT, iTextSharp.text.Color.BLACK) : cell.Border = PdfCell.NO_BORDER : cell.Colspan = 10 : myTableCol.AddCell(cell)
                phrase = New Phrase("", fntArial12N) : cell = PhraseCell(phrase, PdfPCell.ALIGN_LEFT, iTextSharp.text.Color.BLACK) : cell.Border = PdfCell.NO_BORDER : cell.Colspan = 5 : myTableCol.AddCell(cell)
                doc.Add(myTableCol)

                Dim myReportHeaderTable2 As PdfPTable = New PdfPTable(3)
                Dim RptHeaderTblHdWidths2 As Single() = New Single(2) {}
                RptHeaderTblHdWidths2(0) = 120.0F
                RptHeaderTblHdWidths2(1) = 120.0F
                RptHeaderTblHdWidths2(2) = 600.0F

                myReportHeaderTable2.SetWidths(RptHeaderTblHdWidths2)
                myReportHeaderTable2.HorizontalAlignment = Element.ALIGN_LEFT

                phrase = New Phrase("Note", tahoma) : cell = PhraseCell(phrase, PdfPCell.ALIGN_LEFT, iTextSharp.text.Color.WHITE) : cell.VerticalAlignment = Element.ALIGN_CENTER : myReportHeaderTable2.AddCell(cell)
                phrase = New Phrase(":", tahoma) : cell = PhraseCell(phrase, PdfPCell.ALIGN_RIGHT, iTextSharp.text.Color.WHITE) : cell.VerticalAlignment = Element.ALIGN_CENTER : myReportHeaderTable2.AddCell(cell)
                phrase = New Phrase(" Bunga Menurun Dengan Rate Rata-Rata 3.5% Per Bulan", tahoma) : cell = PhraseCell(phrase, PdfPCell.ALIGN_LEFT, iTextSharp.text.Color.WHITE) : cell.VerticalAlignment = Element.ALIGN_CENTER : myReportHeaderTable2.AddCell(cell)

                phrase = New Phrase("", tahoma) : cell = PhraseCell(phrase, PdfPCell.ALIGN_LEFT, iTextSharp.text.Color.WHITE) : cell.VerticalAlignment = Element.ALIGN_CENTER : myReportHeaderTable2.AddCell(cell)
                phrase = New Phrase(":", tahoma) : cell = PhraseCell(phrase, PdfPCell.ALIGN_RIGHT, iTextSharp.text.Color.WHITE) : cell.VerticalAlignment = Element.ALIGN_CENTER : myReportHeaderTable2.AddCell(cell)
                phrase = New Phrase("- Angsuran Bunga Menurun", tahoma) : cell = PhraseCell(phrase, PdfPCell.ALIGN_LEFT, iTextSharp.text.Color.WHITE) : cell.VerticalAlignment = Element.ALIGN_CENTER : myReportHeaderTable2.AddCell(cell)

                phrase = New Phrase("", tahoma) : cell = PhraseCell(phrase, PdfPCell.ALIGN_LEFT, iTextSharp.text.Color.WHITE) : cell.VerticalAlignment = Element.ALIGN_CENTER : myReportHeaderTable2.AddCell(cell)
                phrase = New Phrase("", tahoma) : cell = PhraseCell(phrase, PdfPCell.ALIGN_RIGHT, iTextSharp.text.Color.WHITE) : cell.VerticalAlignment = Element.ALIGN_CENTER : myReportHeaderTable2.AddCell(cell)
                phrase = New Phrase("", tahoma) : cell = PhraseCell(phrase, PdfPCell.ALIGN_LEFT, iTextSharp.text.Color.WHITE) : cell.VerticalAlignment = Element.ALIGN_CENTER : myReportHeaderTable2.AddCell(cell)

                phrase = New Phrase("Pendapatan Penggenapan", tahoma) : cell = PhraseCell(phrase, PdfPCell.ALIGN_LEFT, iTextSharp.text.Color.WHITE) : cell.VerticalAlignment = Element.ALIGN_CENTER : myReportHeaderTable2.AddCell(cell)
                phrase = New Phrase(":", tahoma) : cell = PhraseCell(phrase, PdfPCell.ALIGN_RIGHT, iTextSharp.text.Color.WHITE) : cell.VerticalAlignment = Element.ALIGN_CENTER : myReportHeaderTable2.AddCell(cell)
                phrase = New Phrase("0 ", tahoma) : cell = PhraseCell(phrase, PdfPCell.ALIGN_RIGHT, iTextSharp.text.Color.WHITE) : cell.VerticalAlignment = Element.ALIGN_CENTER : myReportHeaderTable2.AddCell(cell)

                phrase = New Phrase("Grand Total", tahoma) : cell = PhraseCell(phrase, PdfPCell.ALIGN_LEFT, iTextSharp.text.Color.WHITE) : cell.VerticalAlignment = Element.ALIGN_CENTER : myReportHeaderTable2.AddCell(cell)
                phrase = New Phrase(":", tahoma) : cell = PhraseCell(phrase, PdfPCell.ALIGN_RIGHT, iTextSharp.text.Color.WHITE) : cell.VerticalAlignment = Element.ALIGN_CENTER : myReportHeaderTable2.AddCell(cell)
                phrase = New Phrase(FormatNumber(CDbl(Jumlahsaldoawal), 0), tahoma) : cell = PhraseCell(phrase, PdfPCell.ALIGN_RIGHT, iTextSharp.text.Color.WHITE) : cell.VerticalAlignment = Element.ALIGN_CENTER : myReportHeaderTable2.AddCell(cell)


                doc.Add(myReportHeaderTable2)
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

    Public Sub FormatReport()
        If String.IsNullOrEmpty(txtidnasabah.Text.Trim()) Then
            Dim successScript As String = "setTimeout(function(){ Swal.fire({ icon: 'warning', title: 'Id Nasabah Tidak Boleh Kosong', showConfirmButton: false, showCancelButton: false, timer: 2000, timerProgressBar: true }); }, 100);"
            ScriptManager.RegisterStartupScript(Me, Me.[GetType](), "Popup", successScript, True)
            Exit Sub
        ElseIf String.IsNullOrEmpty(txtnopinjam.Text.Trim()) Then
            Dim successScript As String = "setTimeout(function(){ Swal.fire({ icon: 'warning', title: 'Nomor Kontrak Tidak Boleh Kosong', showConfirmButton: false, showCancelButton: false, timer: 2000, timerProgressBar: true }); }, 100);"
            ScriptManager.RegisterStartupScript(Me, Me.[GetType](), "Popup", successScript, True)
            Exit Sub
        End If


        Dim totalsaldoawal As Integer = 0
        Dim realangsuranlancar As Integer = 0
        Dim realangsuranmacet As Integer = 0
        Dim totasaldoakhir As Integer = 0
        Dim mdbttotal As Integer = 0
        Dim mdbtsaldoawal As Integer = 0
        Dim mdbttotalangsuran As Integer = 0
        Dim keterangan As String = "Sudah Bayar"
        Dim dtHead As DataTable


        dtHead = GetDataTbl("SELECT idCabang, idUnit, Nama, TglTagihan, Jumlah, JumlahPokok, JumlahBunga,  " _
                            & " From TblTagihanNasabah where idTransJual='" & txtnopinjam.Text & "'  Order by idtrans Asc ")


        Dim sepLine As iTextSharp.text.pdf.draw.LineSeparator = New iTextSharp.text.pdf.draw.LineSeparator()
        sepLine.LineWidth = 1
        sepLine.Percentage = 60
        sepLine.LineColor = New iTextSharp.text.Color(System.Drawing.Color.Black)
        Dim chunkLine As Chunk = New Chunk(sepLine)

        Dim paragrp As Paragraph = Nothing
        Dim phrase As Phrase = Nothing
        Dim cell As PdfPCell = Nothing
        Dim table As PdfPTable = Nothing
        Dim color__1 As iTextSharp.text.Color = Nothing
        Dim fntArial8 As iTextSharp.text.Font = FontFactory.GetFont("Arial", 8, iTextSharp.text.Font.NORMAL, iTextSharp.text.Color.BLACK)
        Dim fntTahoma As iTextSharp.text.Font = FontFactory.GetFont("Tahoma", 10, iTextSharp.text.Font.NORMAL, iTextSharp.text.Color.BLACK)
        Dim fntArial12N As iTextSharp.text.Font = FontFactory.GetFont("Arial", 8, iTextSharp.text.Font.NORMAL, iTextSharp.text.Color.BLACK)
        Dim fntArial12U As iTextSharp.text.Font = FontFactory.GetFont("Arial", 8, iTextSharp.text.Font.UNDERLINE, iTextSharp.text.Color.BLACK)
        Dim fntArial12B As iTextSharp.text.Font = FontFactory.GetFont("Arial", 8, iTextSharp.text.Font.BOLD, iTextSharp.text.Color.BLACK)
        'Dim fntArial12C As iTextSharp.text.Font = FontFactory.GetFont("Arial", 8, iTextSharp.text.Font.UNDEFINED, iTextSharp.text.Color.BLACK)

        Dim fntArial10N As iTextSharp.text.Font = FontFactory.GetFont("Arial", 12, iTextSharp.text.Font.NORMAL, iTextSharp.text.Color.BLACK)
        Dim fntArial10U As iTextSharp.text.Font = FontFactory.GetFont("Arial", 10, iTextSharp.text.Font.UNDERLINE, iTextSharp.text.Color.BLACK)
        Dim fntArial10B As iTextSharp.text.Font = FontFactory.GetFont("Arial", 18, iTextSharp.text.Font.BOLD, iTextSharp.text.Color.BLACK)
        Dim fntArial10Bw As iTextSharp.text.Font = FontFactory.GetFont("Arial", 10, iTextSharp.text.Font.BOLD, iTextSharp.text.Color.WHITE)
        Dim fntArial14U As iTextSharp.text.Font = FontFactory.GetFont("Arial", 14, iTextSharp.text.Font.NORMAL, iTextSharp.text.Color.BLACK)
        Dim fntArial10Bajar As iTextSharp.text.Font = FontFactory.GetFont("Arial", 14, iTextSharp.text.Font.BOLD, iTextSharp.text.Color.BLACK)
        Dim BaskervileOldFace36 As iTextSharp.text.Font = FontFactory.GetFont("Arial", 36, iTextSharp.text.Font.BOLD, iTextSharp.text.Color.BLACK)
        Dim BaskervileOldFace28 As iTextSharp.text.Font = FontFactory.GetFont("Arial", 24, iTextSharp.text.Font.BOLD, iTextSharp.text.Color.BLACK)
        Dim BaskervileOldFace As iTextSharp.text.Font = FontFactory.GetFont("Arial", 12, iTextSharp.text.Font.BOLD, iTextSharp.text.Color.BLACK)
        Dim tahoma As iTextSharp.text.Font = FontFactory.GetFont("Arial", 12, iTextSharp.text.Font.BOLD, iTextSharp.text.Color.BLACK)


        If dtHead.Rows.Count >= 1 Then
            'Dim myWidth As Single
            'Dim myHeight As Single
            Dim leftMargin As Single = 10
            Dim rightMargin As Single = 10
            Dim topMargin As Single = 5
            Dim bottomMargin As Single = 5
            Dim filepath As String = Server.MapPath("PDF/Kartu Piutang Nasabah" & Format(Now, "dd-MM-yyyy") & ".pdf")
            Using fs As New FileStream(Server.MapPath("PDF/Kartu Piutang Nasabah" & Format(Now, "dd-MM-yyyy") & ".pdf"), FileMode.Create)
                Dim doc As New iTextSharp.text.Document(iTextSharp.text.PageSize.LEGAL.Rotate, leftMargin, rightMargin, topMargin, bottomMargin)
                PdfWriter.GetInstance(doc, fs)
                doc.AddTitle("Kartu Piutang Nasabah")
                doc.Open()

                Dim sTrue5 As DataTable = GetDataTbl("select b.lokasi as lokasicabang, c.lokasi as lokasiunit from tbljual a inner join tblcabang b on b.IDCABANG=a.idcabang inner join tblunitkerja c on c.idunit=a.idunit  where idtrans='" & txtnopinjam.Text & "' And idnasabah='" & txtidnasabah.Text & "'")
                Dim akmp As String = sTrue5.Rows(0).Item("lokasicabang")
                Dim akmp2 As String = sTrue5.Rows(0).Item("lokasiunit")
                'If jATUHtEMPO > dtglangsuran.Value Then
                '    StatusAnsuranNasabah = "LANCAR"
                'Else
                '    StatusAnsuranNasabah = "MACET"
                'End If

                '  Dim akmp As String = "UNIVERSITAS QUALITY"
                'If Left(ketidkampus, 1) = "1" Then
                '    akmp = "UNIVERSITAS QUALITY BRASTAGI"
                'End If


                Dim uq As Paragraph = New Paragraph(akmp, BaskervileOldFace36)
                uq.Alignment = Element.ALIGN_CENTER
                doc.Add(uq)
                doc.Add(New Paragraph(" "))

                Dim uq2 As Paragraph = New Paragraph(akmp2, BaskervileOldFace28)
                uq2.Alignment = Element.ALIGN_CENTER
                doc.Add(uq2)
                doc.Add(New Paragraph(" "))

                paragrp = New Paragraph("KARTU PIUTANG NASABAH ", fntArial10B)
                paragrp.Alignment = Element.ALIGN_CENTER
                doc.Add(paragrp)
                doc.Add(New Paragraph(" "))


                Dim myReportHeaderTable As PdfPTable = New PdfPTable(9)
                Dim RptHeaderTblHdWidths As Single() = New Single(8) {}
                RptHeaderTblHdWidths(0) = 120.0F
                RptHeaderTblHdWidths(1) = 10.0F
                RptHeaderTblHdWidths(2) = 200.0F
                RptHeaderTblHdWidths(3) = 150.0F
                RptHeaderTblHdWidths(4) = 10.0F
                RptHeaderTblHdWidths(5) = 200.0F
                RptHeaderTblHdWidths(6) = 150.0F
                RptHeaderTblHdWidths(7) = 10.0F
                RptHeaderTblHdWidths(8) = 300.0F
                'myTable.LockedWidth = True
                myReportHeaderTable.SetWidths(RptHeaderTblHdWidths)
                myReportHeaderTable.HorizontalAlignment = Element.ALIGN_LEFT

                phrase = New Phrase("No Kontrak", tahoma) : cell = PhraseCell(phrase, PdfPCell.ALIGN_LEFT, iTextSharp.text.Color.WHITE) : cell.VerticalAlignment = Element.ALIGN_CENTER : myReportHeaderTable.AddCell(cell)
                phrase = New Phrase(":", tahoma) : cell = PhraseCell(phrase, PdfPCell.ALIGN_LEFT, iTextSharp.text.Color.WHITE) : cell.VerticalAlignment = Element.ALIGN_CENTER : myReportHeaderTable.AddCell(cell)
                phrase = New Phrase(txtnopinjam.Text, tahoma) : cell = PhraseCell(phrase, PdfPCell.ALIGN_LEFT, iTextSharp.text.Color.WHITE) : cell.VerticalAlignment = Element.ALIGN_CENTER : myReportHeaderTable.AddCell(cell)
                phrase = New Phrase("Nama ", tahoma) : cell = PhraseCell(phrase, PdfPCell.ALIGN_LEFT, iTextSharp.text.Color.WHITE) : cell.VerticalAlignment = Element.ALIGN_CENTER : myReportHeaderTable.AddCell(cell)
                phrase = New Phrase(":", tahoma) : cell = PhraseCell(phrase, PdfPCell.ALIGN_LEFT, iTextSharp.text.Color.WHITE) : cell.VerticalAlignment = Element.ALIGN_CENTER : myReportHeaderTable.AddCell(cell)
                phrase = New Phrase(txtnama.Text, tahoma) : cell = PhraseCell(phrase, PdfPCell.ALIGN_LEFT, iTextSharp.text.Color.WHITE) : cell.VerticalAlignment = Element.ALIGN_CENTER : myReportHeaderTable.AddCell(cell)
                phrase = New Phrase("Alamat ", tahoma) : cell = PhraseCell(phrase, PdfPCell.ALIGN_LEFT, iTextSharp.text.Color.WHITE) : cell.VerticalAlignment = Element.ALIGN_CENTER : myReportHeaderTable.AddCell(cell)
                phrase = New Phrase(":", tahoma) : cell = PhraseCell(phrase, PdfPCell.ALIGN_LEFT, iTextSharp.text.Color.WHITE) : cell.VerticalAlignment = Element.ALIGN_CENTER : myReportHeaderTable.AddCell(cell)
                phrase = New Phrase(txtalamat.Text, tahoma) : cell = PhraseCell(phrase, PdfPCell.ALIGN_LEFT, iTextSharp.text.Color.WHITE) : cell.VerticalAlignment = Element.ALIGN_CENTER : myReportHeaderTable.AddCell(cell)

                phrase = New Phrase("Jenis ", tahoma) : cell = PhraseCell(phrase, PdfPCell.ALIGN_LEFT, iTextSharp.text.Color.WHITE) : cell.VerticalAlignment = Element.ALIGN_CENTER : myReportHeaderTable.AddCell(cell)
                phrase = New Phrase(":", tahoma) : cell = PhraseCell(phrase, PdfPCell.ALIGN_LEFT, iTextSharp.text.Color.WHITE) : cell.VerticalAlignment = Element.ALIGN_CENTER : myReportHeaderTable.AddCell(cell)
                phrase = New Phrase(txtjenis.Text, tahoma) : cell = PhraseCell(phrase, PdfPCell.ALIGN_LEFT, iTextSharp.text.Color.WHITE) : cell.VerticalAlignment = Element.ALIGN_CENTER : myReportHeaderTable.AddCell(cell)
                phrase = New Phrase("Harga Nominal", tahoma) : cell = PhraseCell(phrase, PdfPCell.ALIGN_LEFT, iTextSharp.text.Color.WHITE) : cell.VerticalAlignment = Element.ALIGN_CENTER : myReportHeaderTable.AddCell(cell)
                phrase = New Phrase(":", tahoma) : cell = PhraseCell(phrase, PdfPCell.ALIGN_LEFT, iTextSharp.text.Color.WHITE) : cell.VerticalAlignment = Element.ALIGN_CENTER : myReportHeaderTable.AddCell(cell)
                phrase = New Phrase(FormatNumber(CDbl(txtharga.Text), 0), tahoma) : cell = PhraseCell(phrase, PdfPCell.ALIGN_LEFT, iTextSharp.text.Color.WHITE) : cell.VerticalAlignment = Element.ALIGN_CENTER : myReportHeaderTable.AddCell(cell)
                phrase = New Phrase("Merk ", tahoma) : cell = PhraseCell(phrase, PdfPCell.ALIGN_LEFT, iTextSharp.text.Color.WHITE) : cell.VerticalAlignment = Element.ALIGN_CENTER : myReportHeaderTable.AddCell(cell)
                phrase = New Phrase(":", tahoma) : cell = PhraseCell(phrase, PdfPCell.ALIGN_LEFT, iTextSharp.text.Color.WHITE) : cell.VerticalAlignment = Element.ALIGN_CENTER : myReportHeaderTable.AddCell(cell)
                phrase = New Phrase(txtmerk.Text, tahoma) : cell = PhraseCell(phrase, PdfPCell.ALIGN_LEFT, iTextSharp.text.Color.WHITE) : cell.VerticalAlignment = Element.ALIGN_CENTER : myReportHeaderTable.AddCell(cell)

                phrase = New Phrase("DP", tahoma) : cell = PhraseCell(phrase, PdfPCell.ALIGN_LEFT, iTextSharp.text.Color.WHITE) : cell.VerticalAlignment = Element.ALIGN_CENTER : myReportHeaderTable.AddCell(cell)
                phrase = New Phrase(":", tahoma) : cell = PhraseCell(phrase, PdfPCell.ALIGN_LEFT, iTextSharp.text.Color.WHITE) : cell.VerticalAlignment = Element.ALIGN_CENTER : myReportHeaderTable.AddCell(cell)
                phrase = New Phrase(FormatNumber(CDbl(txtdp.Text), 0), tahoma) : cell = PhraseCell(phrase, PdfPCell.ALIGN_LEFT, iTextSharp.text.Color.WHITE) : cell.VerticalAlignment = Element.ALIGN_CENTER : myReportHeaderTable.AddCell(cell)
                phrase = New Phrase("Type ", tahoma) : cell = PhraseCell(phrase, PdfPCell.ALIGN_LEFT, iTextSharp.text.Color.WHITE) : cell.VerticalAlignment = Element.ALIGN_CENTER : myReportHeaderTable.AddCell(cell)
                phrase = New Phrase(":", tahoma) : cell = PhraseCell(phrase, PdfPCell.ALIGN_LEFT, iTextSharp.text.Color.WHITE) : cell.VerticalAlignment = Element.ALIGN_CENTER : myReportHeaderTable.AddCell(cell)
                phrase = New Phrase(txttype.Text, tahoma) : cell = PhraseCell(phrase, PdfPCell.ALIGN_LEFT, iTextSharp.text.Color.WHITE) : cell.VerticalAlignment = Element.ALIGN_CENTER : myReportHeaderTable.AddCell(cell)
                phrase = New Phrase("Pokok Kredit ", tahoma) : cell = PhraseCell(phrase, PdfPCell.ALIGN_LEFT, iTextSharp.text.Color.WHITE) : cell.VerticalAlignment = Element.ALIGN_CENTER : myReportHeaderTable.AddCell(cell)
                phrase = New Phrase(":", tahoma) : cell = PhraseCell(phrase, PdfPCell.ALIGN_LEFT, iTextSharp.text.Color.WHITE) : cell.VerticalAlignment = Element.ALIGN_CENTER : myReportHeaderTable.AddCell(cell)
                phrase = New Phrase(FormatNumber(CDbl(txtpokok.Text), 0), tahoma) : cell = PhraseCell(phrase, PdfPCell.ALIGN_LEFT, iTextSharp.text.Color.WHITE) : cell.VerticalAlignment = Element.ALIGN_CENTER : myReportHeaderTable.AddCell(cell)

                phrase = New Phrase("Nomor Plat", tahoma) : cell = PhraseCell(phrase, PdfPCell.ALIGN_LEFT, iTextSharp.text.Color.WHITE) : cell.VerticalAlignment = Element.ALIGN_CENTER : myReportHeaderTable.AddCell(cell)
                phrase = New Phrase(":", tahoma) : cell = PhraseCell(phrase, PdfPCell.ALIGN_LEFT, iTextSharp.text.Color.WHITE) : cell.VerticalAlignment = Element.ALIGN_CENTER : myReportHeaderTable.AddCell(cell)
                phrase = New Phrase(txtplat.Text, tahoma) : cell = PhraseCell(phrase, PdfPCell.ALIGN_LEFT, iTextSharp.text.Color.WHITE) : cell.VerticalAlignment = Element.ALIGN_CENTER : myReportHeaderTable.AddCell(cell)
                phrase = New Phrase("Angsuran/Bulan ", tahoma) : cell = PhraseCell(phrase, PdfPCell.ALIGN_LEFT, iTextSharp.text.Color.WHITE) : cell.VerticalAlignment = Element.ALIGN_CENTER : myReportHeaderTable.AddCell(cell)
                phrase = New Phrase(":", tahoma) : cell = PhraseCell(phrase, PdfPCell.ALIGN_LEFT, iTextSharp.text.Color.WHITE) : cell.VerticalAlignment = Element.ALIGN_CENTER : myReportHeaderTable.AddCell(cell)
                phrase = New Phrase(FormatNumber(CDbl(txtangsuran.Text), 0), tahoma) : cell = PhraseCell(phrase, PdfPCell.ALIGN_LEFT, iTextSharp.text.Color.WHITE) : cell.VerticalAlignment = Element.ALIGN_CENTER : myReportHeaderTable.AddCell(cell)
                phrase = New Phrase("Warna  ", tahoma) : cell = PhraseCell(phrase, PdfPCell.ALIGN_LEFT, iTextSharp.text.Color.WHITE) : cell.VerticalAlignment = Element.ALIGN_CENTER : myReportHeaderTable.AddCell(cell)
                phrase = New Phrase(":", tahoma) : cell = PhraseCell(phrase, PdfPCell.ALIGN_LEFT, iTextSharp.text.Color.WHITE) : cell.VerticalAlignment = Element.ALIGN_CENTER : myReportHeaderTable.AddCell(cell)
                phrase = New Phrase(txtwarna.Text, tahoma) : cell = PhraseCell(phrase, PdfPCell.ALIGN_LEFT, iTextSharp.text.Color.WHITE) : cell.VerticalAlignment = Element.ALIGN_CENTER : myReportHeaderTable.AddCell(cell)

                phrase = New Phrase("Lama", tahoma) : cell = PhraseCell(phrase, PdfPCell.ALIGN_LEFT, iTextSharp.text.Color.WHITE) : cell.VerticalAlignment = Element.ALIGN_CENTER : myReportHeaderTable.AddCell(cell)
                phrase = New Phrase(":", tahoma) : cell = PhraseCell(phrase, PdfPCell.ALIGN_LEFT, iTextSharp.text.Color.WHITE) : cell.VerticalAlignment = Element.ALIGN_CENTER : myReportHeaderTable.AddCell(cell)
                phrase = New Phrase(txtjangka.Text, tahoma) : cell = PhraseCell(phrase, PdfPCell.ALIGN_LEFT, iTextSharp.text.Color.WHITE) : cell.VerticalAlignment = Element.ALIGN_CENTER : myReportHeaderTable.AddCell(cell)
                phrase = New Phrase("Kondisi ", tahoma) : cell = PhraseCell(phrase, PdfPCell.ALIGN_LEFT, iTextSharp.text.Color.WHITE) : cell.VerticalAlignment = Element.ALIGN_CENTER : myReportHeaderTable.AddCell(cell)
                phrase = New Phrase(":", tahoma) : cell = PhraseCell(phrase, PdfPCell.ALIGN_LEFT, iTextSharp.text.Color.WHITE) : cell.VerticalAlignment = Element.ALIGN_CENTER : myReportHeaderTable.AddCell(cell)
                phrase = New Phrase(txtkondisi.Text, tahoma) : cell = PhraseCell(phrase, PdfPCell.ALIGN_LEFT, iTextSharp.text.Color.WHITE) : cell.VerticalAlignment = Element.ALIGN_CENTER : myReportHeaderTable.AddCell(cell)
                phrase = New Phrase("Tgl.Pembayaran  ", tahoma) : cell = PhraseCell(phrase, PdfPCell.ALIGN_LEFT, iTextSharp.text.Color.WHITE) : cell.VerticalAlignment = Element.ALIGN_CENTER : myReportHeaderTable.AddCell(cell)
                phrase = New Phrase(":", tahoma) : cell = PhraseCell(phrase, PdfPCell.ALIGN_LEFT, iTextSharp.text.Color.WHITE) : cell.VerticalAlignment = Element.ALIGN_CENTER : myReportHeaderTable.AddCell(cell)
                phrase = New Phrase(txtwarna.Text, tahoma) : cell = PhraseCell(phrase, PdfPCell.ALIGN_LEFT, iTextSharp.text.Color.WHITE) : cell.VerticalAlignment = Element.ALIGN_CENTER : myReportHeaderTable.AddCell(cell)


                doc.Add(myReportHeaderTable)

                Dim myTableCol As PdfPTable = New PdfPTable(9)
                Dim sglTblHdWidthsCol As Single() = New Single(8) {}

                sglTblHdWidthsCol(0) = 1.0F 'No
                sglTblHdWidthsCol(1) = 2.0F 'tanggal jatuh tempo
                sglTblHdWidthsCol(2) = 2.0F 'nominal angsuran
                sglTblHdWidthsCol(3) = 2.0F 'nominal pokok
                sglTblHdWidthsCol(4) = 2.0F 'nominal bunga
                sglTblHdWidthsCol(5) = 2.0F 'Saldo akhir pokok
                sglTblHdWidthsCol(6) = 2.0F 'Saldo Akhir pokok+bunga
                sglTblHdWidthsCol(7) = 2.0F 'Keterangan
                sglTblHdWidthsCol(8) = 2.0F 'Tanggal Bayar


                'myTable.LockedWidth = True
                myTableCol.HorizontalAlignment = Element.ALIGN_LEFT
                myTableCol.WidthPercentage = 100
                myTableCol.SetWidths(sglTblHdWidthsCol)

                'paragrp = New Paragraph(txtnamacabang.Text, fntArial10B)
                'paragrp.Alignment = Element.ALIGN_CENTER
                'doc.Add(paragrp)

                'paragrp = New Paragraph(txtnamaUnit.Text, fntArial10B)
                'paragrp.Alignment = Element.ALIGN_CENTER
                'doc.Add(paragrp)

                phrase = New Phrase("", fntTahoma) : cell = PhraseCell(phrase, PdfPCell.ALIGN_LEFT, iTextSharp.text.Color.BLACK) : cell.Border = PdfCell.NO_BORDER : cell.Colspan = 9 : myTableCol.AddCell(cell)
                phrase = New Phrase("", fntTahoma) : cell = PhraseCell(phrase, PdfPCell.ALIGN_CENTER, iTextSharp.text.Color.BLACK) : cell.Border = PdfCell.NO_BORDER : cell.Colspan = 9 : myTableCol.AddCell(cell)
                phrase = New Phrase("", fntTahoma) : cell = PhraseCell(phrase, PdfPCell.ALIGN_CENTER, iTextSharp.text.Color.BLACK) : cell.Border = PdfCell.NO_BORDER : cell.Colspan = 9 : myTableCol.AddCell(cell)

                'Rows 1
                phrase = New Phrase("", fntArial14U) : cell = PhraseCell(phrase, PdfPCell.ALIGN_CENTER, iTextSharp.text.Color.BLACK) : cell = PhraseCell(phrase, PdfPCell.ALIGN_CENTER, iTextSharp.text.Color.BLACK) : cell.VerticalAlignment = Element.ALIGN_CENTER : cell.Colspan = 1 : myTableCol.AddCell(cell)
                phrase = New Phrase("", fntArial14U) : cell = PhraseCell(phrase, PdfPCell.ALIGN_CENTER, iTextSharp.text.Color.BLACK) : cell = PhraseCell(phrase, PdfPCell.ALIGN_CENTER, iTextSharp.text.Color.BLACK) : cell.VerticalAlignment = Element.ALIGN_CENTER : cell.Colspan = 1 : myTableCol.AddCell(cell)
                phrase = New Phrase("Angsuran ", fntArial14U) : cell = PhraseCell(phrase, PdfPCell.ALIGN_CENTER, iTextSharp.text.Color.BLACK) : cell = PhraseCell(phrase, PdfPCell.ALIGN_CENTER, iTextSharp.text.Color.BLACK) : cell.VerticalAlignment = Element.ALIGN_CENTER : cell.Colspan = 3 : myTableCol.AddCell(cell)
                phrase = New Phrase("Saldo Piutang", fntArial14U) : cell = PhraseCell(phrase, PdfPCell.ALIGN_CENTER, iTextSharp.text.Color.BLACK) : cell = PhraseCell(phrase, PdfPCell.ALIGN_CENTER, iTextSharp.text.Color.BLACK) : cell.VerticalAlignment = Element.ALIGN_CENTER : cell.Colspan = 2 : myTableCol.AddCell(cell)
                phrase = New Phrase("", fntArial14U) : cell = PhraseCell(phrase, PdfPCell.ALIGN_CENTER, iTextSharp.text.Color.BLACK) : cell = PhraseCell(phrase, PdfPCell.ALIGN_CENTER, iTextSharp.text.Color.BLACK) : cell.VerticalAlignment = Element.ALIGN_CENTER : cell.Colspan = 1 : myTableCol.AddCell(cell)
                phrase = New Phrase("", fntArial14U) : cell = PhraseCell(phrase, PdfPCell.ALIGN_LEFT, iTextSharp.text.Color.BLACK) : cell = PhraseCell(phrase, PdfPCell.ALIGN_CENTER, iTextSharp.text.Color.BLACK) : cell.VerticalAlignment = Element.ALIGN_CENTER : cell.Colspan = 1 : myTableCol.AddCell(cell)


                ''Header Tabel
                phrase = New Phrase("No", fntArial10N) : cell = PhraseCell(phrase, PdfPCell.ALIGN_CENTER, iTextSharp.text.Color.BLACK) : cell.FixedHeight = 10.0F : myTableCol.AddCell(cell)
                phrase = New Phrase("Jatuh Tempo ", fntArial10N) : cell = PhraseCell(phrase, PdfPCell.ALIGN_CENTER, iTextSharp.text.Color.BLACK) : myTableCol.AddCell(cell)
                phrase = New Phrase("Nominal ", fntArial10N) : cell = PhraseCell(phrase, PdfPCell.ALIGN_CENTER, iTextSharp.text.Color.BLACK) : myTableCol.AddCell(cell)
                phrase = New Phrase("Pokok ", fntArial10N) : cell = PhraseCell(phrase, PdfPCell.ALIGN_CENTER, iTextSharp.text.Color.BLACK) : myTableCol.AddCell(cell)
                phrase = New Phrase("Bunga ", fntArial10N) : cell = PhraseCell(phrase, PdfPCell.ALIGN_CENTER, iTextSharp.text.Color.BLACK) : myTableCol.AddCell(cell)
                phrase = New Phrase("Pokok", fntArial10N) : cell = PhraseCell(phrase, PdfPCell.ALIGN_CENTER, iTextSharp.text.Color.BLACK) : myTableCol.AddCell(cell)
                phrase = New Phrase("Bruto ", fntArial10N) : cell = PhraseCell(phrase, PdfPCell.ALIGN_CENTER, iTextSharp.text.Color.BLACK) : myTableCol.AddCell(cell)
                phrase = New Phrase("Keterangan ", fntArial10N) : cell = PhraseCell(phrase, PdfPCell.ALIGN_CENTER, iTextSharp.text.Color.BLACK) : myTableCol.AddCell(cell)
                phrase = New Phrase("Tanggal Bayar ", fntArial10N) : cell = PhraseCell(phrase, PdfPCell.ALIGN_CENTER, iTextSharp.text.Color.BLACK) : myTableCol.AddCell(cell)

                For z = 0 To dtHead.Rows.Count - 1
                    phrase = New Phrase(z + 1, fntTahoma) : cell = PhraseCell(phrase, PdfPCell.ALIGN_CENTER, iTextSharp.text.Color.BLACK) : cell.FixedHeight = 15.0F : myTableCol.AddCell(cell)
                    phrase = New Phrase(dtHead.Rows(z).Item("tgljatuhtempo"), fntTahoma) : cell = PhraseCell(phrase, PdfPCell.ALIGN_LEFT, iTextSharp.text.Color.BLACK) : cell.FixedHeight = 15.0F : myTableCol.AddCell(cell)
                    phrase = New Phrase(FormatNumber(CDbl(dtHead.Rows(z).Item("jumlahangsuran")), 0), fntTahoma) : cell = PhraseCell(phrase, PdfPCell.ALIGN_CENTER, iTextSharp.text.Color.BLACK) : cell.FixedHeight = 15.0F : myTableCol.AddCell(cell)
                    phrase = New Phrase(FormatNumber(CDbl(dtHead.Rows(z).Item("jumlahpokok")), 0), fntTahoma) : cell = PhraseCell(phrase, PdfPCell.ALIGN_CENTER, iTextSharp.text.Color.BLACK) : cell.FixedHeight = 15.0F : myTableCol.AddCell(cell)
                    phrase = New Phrase(FormatNumber(CDbl(dtHead.Rows(z).Item("jumlahbunga")), 0), fntTahoma) : cell = PhraseCell(phrase, PdfPCell.ALIGN_CENTER, iTextSharp.text.Color.BLACK) : cell.FixedHeight = 15.0F : myTableCol.AddCell(cell)
                    phrase = New Phrase(FormatNumber(CDbl(dtHead.Rows(z).Item("saldoakhirpokok")), 0), fntTahoma) : cell = PhraseCell(phrase, PdfPCell.ALIGN_CENTER, iTextSharp.text.Color.BLACK) : cell.FixedHeight = 15.0F : myTableCol.AddCell(cell)
                    phrase = New Phrase(FormatNumber(CDbl(dtHead.Rows(z).Item("saldoakhir")), 0), fntTahoma) : cell = PhraseCell(phrase, PdfPCell.ALIGN_CENTER, iTextSharp.text.Color.BLACK) : cell.FixedHeight = 15.0F : myTableCol.AddCell(cell)
                    phrase = New Phrase("Sudah Bayar", fntTahoma) : cell = PhraseCell(phrase, PdfPCell.ALIGN_LEFT, iTextSharp.text.Color.BLACK) : cell.FixedHeight = 15.0F : myTableCol.AddCell(cell)
                    phrase = New Phrase(dtHead.Rows(z).Item("Tanggal"), fntTahoma) : cell = PhraseCell(phrase, PdfPCell.ALIGN_LEFT, iTextSharp.text.Color.BLACK) : cell.FixedHeight = 15.0F : myTableCol.AddCell(cell)

                    If z < dtHead.Rows.Count - 1 Then

                    End If
                Next z
                Dim sTruesaldoawal As DataTable = GetDataTbl("select isnull(Sum(jumlah),0) as jumlah from TblAngsuran where NoPinjaman='" & txtnopinjam.Text & "' And idNasabah='" & txtidnasabah.Text & "'")
                Dim Jumlahsaldoawal As Integer = sTruesaldoawal.Rows(0).Item("jumlah")
                Dim sTruesaldopokok As DataTable = GetDataTbl("select isnull(Sum(jumlahpokok),0) as jumlah from TblAngsuran where NoPinjaman='" & txtnopinjam.Text & "' And idNasabah='" & txtidnasabah.Text & "'")
                Dim Jumlahsaldoakhirpokok As Integer = sTruesaldopokok.Rows(0).Item("jumlah")
                Dim sTruesaldobunga As DataTable = GetDataTbl("select isnull(Sum(jumlahbunga),0) as jumlah from TblAngsuran where NoPinjaman='" & txtnopinjam.Text & "' And idNasabah='" & txtidnasabah.Text & "'")
                Dim Jumlahsaldoakhirbunga As Integer = sTruesaldobunga.Rows(0).Item("jumlah")
                phrase = New Phrase("TOTAL : ", fntTahoma)
                cell = PhraseCell(phrase, PdfPCell.ALIGN_CENTER, iTextSharp.text.Color.BLACK)
                cell.Border = PdfPCell.BOTTOM_BORDER Or PdfPCell.TOP_BORDER Or PdfPCell.LEFT_BORDER Or PdfPCell.RIGHT_BORDER
                cell.Colspan = 2
                myTableCol.AddCell(cell)
                phrase = New Phrase(FormatNumber(CDbl(Jumlahsaldoawal), 0), fntTahoma)
                cell = PhraseCell(phrase, PdfPCell.ALIGN_CENTER, iTextSharp.text.Color.BLACK)
                cell.Border = PdfPCell.BOTTOM_BORDER Or PdfPCell.TOP_BORDER Or PdfPCell.LEFT_BORDER Or PdfPCell.RIGHT_BORDER
                cell.Colspan = 1
                myTableCol.AddCell(cell)
                phrase = New Phrase(FormatNumber(CDbl(Jumlahsaldoakhirpokok), 0), fntTahoma)
                cell = PhraseCell(phrase, PdfPCell.ALIGN_CENTER, iTextSharp.text.Color.BLACK)
                cell.Border = PdfPCell.BOTTOM_BORDER Or PdfPCell.TOP_BORDER Or PdfPCell.LEFT_BORDER Or PdfPCell.RIGHT_BORDER
                cell.Colspan = 1
                myTableCol.AddCell(cell)
                phrase = New Phrase(FormatNumber(CDbl(Jumlahsaldoakhirbunga), 0), fntTahoma)
                cell = PhraseCell(phrase, PdfPCell.ALIGN_CENTER, iTextSharp.text.Color.BLACK)
                cell.Border = PdfPCell.BOTTOM_BORDER Or PdfPCell.TOP_BORDER Or PdfPCell.LEFT_BORDER Or PdfPCell.RIGHT_BORDER
                cell.Colspan = 1
                myTableCol.AddCell(cell)
                phrase = New Phrase("Suku Rate Bunga Pertahun : 3.5% ", fntTahoma)
                cell = PhraseCell(phrase, PdfPCell.ALIGN_CENTER, iTextSharp.text.Color.BLACK)
                cell.Border = PdfPCell.BOTTOM_BORDER Or PdfPCell.TOP_BORDER Or PdfPCell.LEFT_BORDER Or PdfPCell.RIGHT_BORDER
                cell.Colspan = 4
                myTableCol.AddCell(cell)



                phrase = New Phrase("", fntTahoma) : cell = PhraseCell(phrase, PdfPCell.ALIGN_LEFT, iTextSharp.text.Color.BLACK) : cell.Border = PdfCell.NO_BORDER : cell.Colspan = 20 : myTableCol.AddCell(cell)
                phrase = New Phrase("", fntTahoma) : cell = PhraseCell(phrase, PdfPCell.ALIGN_CENTER, iTextSharp.text.Color.BLACK) : cell.Border = PdfCell.NO_BORDER : cell.Colspan = 20 : myTableCol.AddCell(cell)
                phrase = New Phrase("", fntTahoma) : cell = PhraseCell(phrase, PdfPCell.ALIGN_CENTER, iTextSharp.text.Color.BLACK) : cell.Border = PdfCell.NO_BORDER : cell.Colspan = 20 : myTableCol.AddCell(cell)

                phrase = New Phrase("Cetak Tanggal", fntTahoma) : cell = PhraseCell(phrase, PdfPCell.ALIGN_LEFT, iTextSharp.text.Color.BLACK) : cell.Border = PdfCell.NO_BORDER : cell.Colspan = 4 : myTableCol.AddCell(cell)
                phrase = New Phrase(":", fntTahoma) : cell = PhraseCell(phrase, PdfPCell.ALIGN_LEFT, iTextSharp.text.Color.BLACK) : cell.Border = PdfCell.NO_BORDER : cell.Colspan = 1 : myTableCol.AddCell(cell)
                phrase = New Phrase(Format(Now, "dd/MM/yyyy"), fntTahoma) : cell = PhraseCell(phrase, PdfPCell.ALIGN_LEFT, iTextSharp.text.Color.BLACK) : cell.Border = PdfCell.NO_BORDER : cell.Colspan = 10 : myTableCol.AddCell(cell)
                phrase = New Phrase("", fntArial12N) : cell = PhraseCell(phrase, PdfPCell.ALIGN_LEFT, iTextSharp.text.Color.BLACK) : cell.Border = PdfCell.NO_BORDER : cell.Colspan = 5 : myTableCol.AddCell(cell)
                doc.Add(myTableCol)

                Dim myReportHeaderTable2 As PdfPTable = New PdfPTable(3)
                Dim RptHeaderTblHdWidths2 As Single() = New Single(2) {}
                RptHeaderTblHdWidths2(0) = 120.0F
                RptHeaderTblHdWidths2(1) = 120.0F
                RptHeaderTblHdWidths2(2) = 600.0F

                myReportHeaderTable2.SetWidths(RptHeaderTblHdWidths2)
                myReportHeaderTable2.HorizontalAlignment = Element.ALIGN_LEFT

                phrase = New Phrase("Note", tahoma) : cell = PhraseCell(phrase, PdfPCell.ALIGN_LEFT, iTextSharp.text.Color.WHITE) : cell.VerticalAlignment = Element.ALIGN_CENTER : myReportHeaderTable2.AddCell(cell)
                phrase = New Phrase(":", tahoma) : cell = PhraseCell(phrase, PdfPCell.ALIGN_RIGHT, iTextSharp.text.Color.WHITE) : cell.VerticalAlignment = Element.ALIGN_CENTER : myReportHeaderTable2.AddCell(cell)
                phrase = New Phrase(" Bunga Menurun Dengan Rate Rata-Rata 3.5% Per Bulan", tahoma) : cell = PhraseCell(phrase, PdfPCell.ALIGN_LEFT, iTextSharp.text.Color.WHITE) : cell.VerticalAlignment = Element.ALIGN_CENTER : myReportHeaderTable2.AddCell(cell)

                phrase = New Phrase("", tahoma) : cell = PhraseCell(phrase, PdfPCell.ALIGN_LEFT, iTextSharp.text.Color.WHITE) : cell.VerticalAlignment = Element.ALIGN_CENTER : myReportHeaderTable2.AddCell(cell)
                phrase = New Phrase(":", tahoma) : cell = PhraseCell(phrase, PdfPCell.ALIGN_RIGHT, iTextSharp.text.Color.WHITE) : cell.VerticalAlignment = Element.ALIGN_CENTER : myReportHeaderTable2.AddCell(cell)
                phrase = New Phrase("- Angsuran Bunga Menurun", tahoma) : cell = PhraseCell(phrase, PdfPCell.ALIGN_LEFT, iTextSharp.text.Color.WHITE) : cell.VerticalAlignment = Element.ALIGN_CENTER : myReportHeaderTable2.AddCell(cell)

                phrase = New Phrase("", tahoma) : cell = PhraseCell(phrase, PdfPCell.ALIGN_LEFT, iTextSharp.text.Color.WHITE) : cell.VerticalAlignment = Element.ALIGN_CENTER : myReportHeaderTable2.AddCell(cell)
                phrase = New Phrase("", tahoma) : cell = PhraseCell(phrase, PdfPCell.ALIGN_RIGHT, iTextSharp.text.Color.WHITE) : cell.VerticalAlignment = Element.ALIGN_CENTER : myReportHeaderTable2.AddCell(cell)
                phrase = New Phrase("", tahoma) : cell = PhraseCell(phrase, PdfPCell.ALIGN_LEFT, iTextSharp.text.Color.WHITE) : cell.VerticalAlignment = Element.ALIGN_CENTER : myReportHeaderTable2.AddCell(cell)

                phrase = New Phrase("Pendapatan Penggenapan", tahoma) : cell = PhraseCell(phrase, PdfPCell.ALIGN_LEFT, iTextSharp.text.Color.WHITE) : cell.VerticalAlignment = Element.ALIGN_CENTER : myReportHeaderTable2.AddCell(cell)
                phrase = New Phrase(":", tahoma) : cell = PhraseCell(phrase, PdfPCell.ALIGN_RIGHT, iTextSharp.text.Color.WHITE) : cell.VerticalAlignment = Element.ALIGN_CENTER : myReportHeaderTable2.AddCell(cell)
                phrase = New Phrase("0 ", tahoma) : cell = PhraseCell(phrase, PdfPCell.ALIGN_RIGHT, iTextSharp.text.Color.WHITE) : cell.VerticalAlignment = Element.ALIGN_CENTER : myReportHeaderTable2.AddCell(cell)

                phrase = New Phrase("Grand Total", tahoma) : cell = PhraseCell(phrase, PdfPCell.ALIGN_LEFT, iTextSharp.text.Color.WHITE) : cell.VerticalAlignment = Element.ALIGN_CENTER : myReportHeaderTable2.AddCell(cell)
                phrase = New Phrase(":", tahoma) : cell = PhraseCell(phrase, PdfPCell.ALIGN_RIGHT, iTextSharp.text.Color.WHITE) : cell.VerticalAlignment = Element.ALIGN_CENTER : myReportHeaderTable2.AddCell(cell)
                phrase = New Phrase(FormatNumber(CDbl(Jumlahsaldoawal), 0), tahoma) : cell = PhraseCell(phrase, PdfPCell.ALIGN_RIGHT, iTextSharp.text.Color.WHITE) : cell.VerticalAlignment = Element.ALIGN_CENTER : myReportHeaderTable2.AddCell(cell)


                doc.Add(myReportHeaderTable2)
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

End Class
