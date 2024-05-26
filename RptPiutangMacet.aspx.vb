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

Partial Class RptPiutangMacet
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
        If String.IsNullOrEmpty(txtidcabang.Text.Trim()) Then
            Dim successScript As String = "setTimeout(function(){ Swal.fire({ icon: 'warning', title: 'Id Cabang Tidak Boleh Kosong', showConfirmButton: false, showCancelButton: false, timer: 2000, timerProgressBar: true }); }, 100);"
            ScriptManager.RegisterStartupScript(Me, Me.[GetType](), "Popup", successScript, True)
            Exit Sub
        ElseIf String.IsNullOrEmpty(txtidunit.Text.Trim()) Then
            Dim successScript As String = "setTimeout(function(){ Swal.fire({ icon: 'warning', title: 'Id Unit Silinder Tidak Boleh Kosong', showConfirmButton: false, showCancelButton: false, timer: 2000, timerProgressBar: true }); }, 100);"
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
        Dim dtHead As DataTable


        dtHead = GetDataTbl("SELECT  a.idTrans as idTrans, a.idnasabah, b.Nama as nama, a.noplat as noplat, a.Tanggal as tanggal,a.TglJatuhTempo as TglJatuhTempo, FORMAT(a.SaldoAwal,'#,###,##0') as SaldoAwal " _
                                & " From TblJual a inner join Tblnasabah b on b.idnasabah=b.idnasabah Order by a.idTrans Asc ")


        Dim sepLine As iTextSharp.text.pdf.draw.LineSeparator = New iTextSharp.text.pdf.draw.LineSeparator()
            sepLine.LineWidth = 1
            sepLine.Percentage = 100
            sepLine.LineColor = New iTextSharp.text.Color(System.Drawing.Color.Black)
            Dim chunkLine As Chunk = New Chunk(sepLine)

            Dim paragrp As Paragraph = Nothing
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
            Dim fntArial10B As iTextSharp.text.Font = FontFactory.GetFont("Arial", 10, iTextSharp.text.Font.BOLD, iTextSharp.text.Color.BLACK)
            Dim fntArial10Bw As iTextSharp.text.Font = FontFactory.GetFont("Arial", 10, iTextSharp.text.Font.BOLD, iTextSharp.text.Color.WHITE)
            Dim fntArial14U As iTextSharp.text.Font = FontFactory.GetFont("Arial", 14, iTextSharp.text.Font.NORMAL, iTextSharp.text.Color.BLACK)

            If dtHead.Rows.Count >= 1 Then
                'Dim myWidth As Single
                'Dim myHeight As Single
                Dim leftMargin As Single = 10
                Dim rightMargin As Single = 10
                Dim topMargin As Single = 5
                Dim bottomMargin As Single = 5
            Dim filepath As String = Server.MapPath("PDF/Nasabah Macet" & Format(Now, "dd-MM-yyyy") & ".pdf")
            Using fs As New FileStream(Server.MapPath("PDF/Nasabah Macet" & Format(Now, "dd-MM-yyyy") & ".pdf"), FileMode.Create)
                Dim doc As New iTextSharp.text.Document(iTextSharp.text.PageSize.LEGAL.Rotate, leftMargin, rightMargin, topMargin, bottomMargin)
                PdfWriter.GetInstance(doc, fs)
                doc.AddTitle("Nasabah Macet")
                doc.Open()


                Dim myTableCol As PdfPTable = New PdfPTable(10)
                Dim sglTblHdWidthsCol As Single() = New Single(9) {}

                sglTblHdWidthsCol(0) = 1.0F 'No
                sglTblHdWidthsCol(1) = 4.0F 'noregister
                sglTblHdWidthsCol(2) = 8.0F 'Nama Nasabah
                sglTblHdWidthsCol(3) = 3.0F 'Nopalt
                sglTblHdWidthsCol(4) = 3.0F 'Tanggal Kontrak Pinjaman
                sglTblHdWidthsCol(5) = 3.0F 'Tanggal jatuh Tempo
                sglTblHdWidthsCol(6) = 3.0F 'Saldo Awal
                sglTblHdWidthsCol(7) = 3.0F 'Angsuran Lancar
                sglTblHdWidthsCol(8) = 3.0F 'Angsuran Macet
                sglTblHdWidthsCol(9) = 3.0F 'Saldo akhir


                'myTable.LockedWidth = True
                myTableCol.HorizontalAlignment = Element.ALIGN_LEFT
                myTableCol.WidthPercentage = 100
                myTableCol.SetWidths(sglTblHdWidthsCol)

                paragrp = New Paragraph(txtnamacabang.Text, fntArial10B)
                paragrp.Alignment = Element.ALIGN_CENTER
                doc.Add(paragrp)

                paragrp = New Paragraph(txtnamaUnit.Text, fntArial10B)
                paragrp.Alignment = Element.ALIGN_CENTER
                doc.Add(paragrp)

                paragrp = New Paragraph("LAPORAN NASABAH MACET ", fntArial10B)
                paragrp.Alignment = Element.ALIGN_CENTER
                doc.Add(paragrp)
                doc.Add(New Paragraph(" "))

                'Kelas grupping 
                phrase = New Phrase("", fntTahoma) : cell = PhraseCell(phrase, PdfPCell.ALIGN_CENTER, iTextSharp.text.Color.BLACK) : cell.Border = PdfCell.NO_BORDER : cell.Colspan = 16 : myTableCol.AddCell(cell)
                phrase = New Phrase("", fntTahoma) : cell = PhraseCell(phrase, PdfPCell.ALIGN_CENTER, iTextSharp.text.Color.BLACK) : cell.Border = PdfCell.NO_BORDER : cell.Colspan = 16 : myTableCol.AddCell(cell)
                phrase = New Phrase("", fntTahoma) : cell = PhraseCell(phrase, PdfPCell.ALIGN_CENTER, iTextSharp.text.Color.BLACK) : cell.Border = PdfCell.NO_BORDER : cell.Colspan = 16 : myTableCol.AddCell(cell)
                phrase = New Phrase("", fntTahoma) : cell = PhraseCell(phrase, PdfPCell.ALIGN_CENTER, iTextSharp.text.Color.BLACK) : cell.Border = PdfCell.NO_BORDER : cell.Colspan = 16 : myTableCol.AddCell(cell)
                phrase = New Phrase("", fntTahoma) : cell = PhraseCell(phrase, PdfPCell.ALIGN_CENTER, iTextSharp.text.Color.BLACK) : cell.Border = PdfCell.NO_BORDER : cell.Colspan = 16 : myTableCol.AddCell(cell)

                ''Header Tabel
                phrase = New Phrase("No", fntArial10N) : cell = PhraseCell(phrase, PdfPCell.ALIGN_CENTER, iTextSharp.text.Color.BLACK) : cell.FixedHeight = 10.0F : myTableCol.AddCell(cell)
                phrase = New Phrase("Nomor Kontrak ", fntArial10N) : cell = PhraseCell(phrase, PdfPCell.ALIGN_CENTER, iTextSharp.text.Color.BLACK) : myTableCol.AddCell(cell)
                phrase = New Phrase("Nama Nasabah ", fntArial10N) : cell = PhraseCell(phrase, PdfPCell.ALIGN_CENTER, iTextSharp.text.Color.BLACK) : myTableCol.AddCell(cell)
                phrase = New Phrase("No Plat ", fntArial10N) : cell = PhraseCell(phrase, PdfPCell.ALIGN_CENTER, iTextSharp.text.Color.BLACK) : myTableCol.AddCell(cell)
                phrase = New Phrase("Tanggal Kontrak", fntArial10N) : cell = PhraseCell(phrase, PdfPCell.ALIGN_CENTER, iTextSharp.text.Color.BLACK) : myTableCol.AddCell(cell)
                phrase = New Phrase("Tanggal Jatuh Tempo", fntArial10N) : cell = PhraseCell(phrase, PdfPCell.ALIGN_CENTER, iTextSharp.text.Color.BLACK) : myTableCol.AddCell(cell)
                phrase = New Phrase("Saldo Awal", fntArial10N) : cell = PhraseCell(phrase, PdfPCell.ALIGN_CENTER, iTextSharp.text.Color.BLACK) : myTableCol.AddCell(cell)
                phrase = New Phrase("Angsuran Lancar", fntArial10N) : cell = PhraseCell(phrase, PdfPCell.ALIGN_CENTER, iTextSharp.text.Color.BLACK) : myTableCol.AddCell(cell)
                phrase = New Phrase("Angsuran Macet", fntArial10N) : cell = PhraseCell(phrase, PdfPCell.ALIGN_CENTER, iTextSharp.text.Color.BLACK) : myTableCol.AddCell(cell)
                phrase = New Phrase("Saldo Akhir", fntArial10N) : cell = PhraseCell(phrase, PdfPCell.ALIGN_CENTER, iTextSharp.text.Color.BLACK) : myTableCol.AddCell(cell)





                For z = 0 To dtHead.Rows.Count - 1
                    phrase = New Phrase(z + 1, fntTahoma) : cell = PhraseCell(phrase, PdfPCell.ALIGN_CENTER, iTextSharp.text.Color.BLACK) : cell.FixedHeight = 15.0F : myTableCol.AddCell(cell)
                    phrase = New Phrase(dtHead.Rows(z).Item("idTrans"), fntTahoma) : cell = PhraseCell(phrase, PdfPCell.ALIGN_LEFT, iTextSharp.text.Color.BLACK) : cell.FixedHeight = 15.0F : myTableCol.AddCell(cell)
                    phrase = New Phrase(dtHead.Rows(z).Item("nama"), fntTahoma) : cell = PhraseCell(phrase, PdfPCell.ALIGN_LEFT, iTextSharp.text.Color.BLACK) : cell.FixedHeight = 15.0F : myTableCol.AddCell(cell)
                    phrase = New Phrase(dtHead.Rows(z).Item("noplat"), fntTahoma) : cell = PhraseCell(phrase, PdfPCell.ALIGN_LEFT, iTextSharp.text.Color.BLACK) : cell.FixedHeight = 15.0F : myTableCol.AddCell(cell)
                    phrase = New Phrase(dtHead.Rows(z).Item("tanggal"), fntTahoma) : cell = PhraseCell(phrase, PdfPCell.ALIGN_LEFT, iTextSharp.text.Color.BLACK) : cell.FixedHeight = 15.0F : myTableCol.AddCell(cell)
                    phrase = New Phrase(dtHead.Rows(z).Item("TglJatuhTempo"), fntTahoma) : cell = PhraseCell(phrase, PdfPCell.ALIGN_LEFT, iTextSharp.text.Color.BLACK) : cell.FixedHeight = 15.0F : myTableCol.AddCell(cell)
                    phrase = New Phrase(dtHead.Rows(z).Item("SaldoAwal"), fntTahoma) : cell = PhraseCell(phrase, PdfPCell.ALIGN_LEFT, iTextSharp.text.Color.BLACK) : cell.FixedHeight = 15.0F : myTableCol.AddCell(cell)

                    ''''''Angsuran Lancar'''''''
                    Dim KetLancar As String = "LANCAR"
                    Dim dtAngsuranLancar As DataTable = GetDataTbl("select Count(idnasabah) as cnt from tblangsuran a where  a.idcabang='" & txtidcabang.Text & "'  and a.idunit='" & txtidunit.Text & "' And a.idnasabah='" & dtHead.Rows(z).Item("idnasabah") & "' and a.idtrans='" & dtHead.Rows(z).Item("idtrans") & "' And a.StatusAngsuran='" & KetLancar & "'")
                    If dtAngsuranLancar.Rows.Count > 0 Then
                        If CInt(dtAngsuranLancar.Rows(0).Item("cnt")) > 0 Then
                            Dim dtAngsuranLancar2 As DataTable = GetDataTbl("Select isnull(sum(b.jumlah),0) As JumlahLancar from TblAngsuran b where b.idcabang='" & txtidcabang.Text & "' and b.idunit='" & txtidunit.Text & "' And b.idnasabah='" & dtHead.Rows(z).Item("idnasabah") & "' And b.nopinjaman='" & dtHead.Rows(z).Item("idtrans") & "' And b.StatusAngsuran='" & KetLancar & "'")
                            phrase = New Phrase(FormatNumber(CDbl(dtAngsuranLancar2.Rows(0).Item("JumlahLancar")), 0), fntTahoma) : cell = PhraseCell(phrase, PdfPCell.ALIGN_RIGHT, iTextSharp.text.Color.BLACK) : myTableCol.AddCell(cell)
                        Else
                            phrase = New Phrase("0", fntTahoma) : cell = PhraseCell(phrase, PdfPCell.ALIGN_CENTER, iTextSharp.text.Color.BLACK) : myTableCol.AddCell(cell)
                        End If
                    End If

                    ''''''Angsuran Macet'''''''
                    Dim KetMacet As String = "MACET"
                    Dim dtAngsuranMacet As DataTable = GetDataTbl("select Count(idnasabah) as cnt2 from tblangsuran a where  a.idcabang='" & txtidcabang.Text & "'  and a.idunit='" & txtidunit.Text & "' And a.idnasabah='" & dtHead.Rows(z).Item("idnasabah") & "' and a.idtrans='" & dtHead.Rows(z).Item("idtrans") & "' And a.StatusAngsuran='" & KetMacet & "'")
                    If CInt(dtAngsuranMacet.Rows(0).Item("cnt2")) > 0 Then
                        phrase = New Phrase("0", fntTahoma) : cell = PhraseCell(phrase, PdfPCell.ALIGN_CENTER, iTextSharp.text.Color.BLACK) : myTableCol.AddCell(cell)
                    Else
                        Dim dtAngsuranMacet2 As DataTable = GetDataTbl("Select isnull(sum(b.jumlah),0) As JumlahMacet from TblAngsuran b where b.idcabang='" & txtidcabang.Text & "' and b.idunit='" & txtidunit.Text & "' And b.idnasabah='" & dtHead.Rows(z).Item("idnasabah") & "' And b.nopinjaman='" & dtHead.Rows(z).Item("idtrans") & "' And b.StatusAngsuran='" & KetMacet & "'")
                        phrase = New Phrase(FormatNumber(CDbl(dtAngsuranMacet2.Rows(0).Item("JumlahMacet")), 0), fntTahoma) : cell = PhraseCell(phrase, PdfPCell.ALIGN_RIGHT, iTextSharp.text.Color.BLACK) : myTableCol.AddCell(cell)
                    End If


                    '''''SALDO AKHIR'''''''''''''''''''''''''''''
                    Dim dtsaldoawal As DataTable = GetDataTbl("select a.saldoawal as saldoawal from tbljual a where  a.idcabang='" & txtidcabang.Text & "'  and a.idunit='" & txtidunit.Text & "' And a.idnasabah='" & dtHead.Rows(z).Item("idnasabah") & "' And a.idtrans='" & dtHead.Rows(z).Item("idtrans") & "'")
                    Dim dtAngsuranLancar3 As DataTable = GetDataTbl("Select isnull(sum(b.jumlah),0) as JumlahLancar from TblAngsuran b where b.idcabang='" & txtidcabang.Text & "' and b.idunit='" & txtidunit.Text & "' And b.idnasabah='" & dtHead.Rows(z).Item("idnasabah") & "' And b.nopinjaman='" & dtHead.Rows(z).Item("idtrans") & "' And b.StatusAngsuran='" & KetLancar & "'")
                    Dim dtAngsuranMacet3 As DataTable = GetDataTbl("Select isnull(sum(b.jumlah),0) as JumlahMacet from TblAngsuran b where b.idcabang='" & txtidcabang.Text & "' and b.idunit='" & txtidunit.Text & "'  And b.idnasabah='" & dtHead.Rows(z).Item("idnasabah") & "' And b.nopinjaman='" & dtHead.Rows(z).Item("idtrans") & "' And b.StatusAngsuran='" & KetMacet & "'")
                    mdbtsaldoawal = CDbl(dtsaldoawal.Rows(0).Item("saldoawal"))
                    mdbttotalangsuran = CDbl(dtAngsuranLancar3.Rows(0).Item("JumlahLancar")) + CDbl(dtAngsuranMacet3.Rows(0).Item("JumlahMacet"))
                    mdbttotal = mdbtsaldoawal - mdbttotalangsuran
                    phrase = New Phrase(FormatNumber(CDbl(mdbttotal), 0), fntTahoma) : cell = PhraseCell(phrase, PdfPCell.ALIGN_RIGHT, iTextSharp.text.Color.BLACK) : myTableCol.AddCell(cell)

                    'dttotaljanuari = GetDataTbl("select isnull(Sum(jumlah),0) as JumlahJanuari  from TblTransBiaya where  idunit= '" & txtIdKampus.Text & "' and month(tanggal)= " & Januari & " and year(tanggal)=" & ddlThn.Text & "")
                    'mdbtjanuari = CDbl(dttotaljanuari.Rows(0).Item("JumlahJanuari"))


                    If z < dtHead.Rows.Count - 1 Then

                    End If
                Next z

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


End Class
