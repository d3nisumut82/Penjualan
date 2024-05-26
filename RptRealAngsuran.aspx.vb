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

Partial Class RptRealAngsuran
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
            'ElseIf String.IsNullOrEmpty(txtidwipem.Text.Trim()) Then
            '    Dim successScript As String = "setTimeout(function(){ Swal.fire({ icon: 'warning', title: 'Id Wipem Tidak Boleh Kosong', showConfirmButton: false, showCancelButton: false, timer: 2000, timerProgressBar: true }); }, 100);"
            '    ScriptManager.RegisterStartupScript(Me, Me.[GetType](), "Popup", successScript, True)
            '    Exit Sub
            'ElseIf String.IsNullOrEmpty(dtpTgl.Value.Trim()) Then
            '    Dim successScript As String = "setTimeout(function(){ Swal.fire({ icon: 'warning', title: 'Tanggal Tidak Boleh Kosong', showConfirmButton: false, showCancelButton: false, timer: 2000, timerProgressBar: true }); }, 100);"
            '    ScriptManager.RegisterStartupScript(Me, Me.[GetType](), "Popup", successScript, True)
            '    Exit Sub
        End If



        Dim dtRuang As DataTable
        Dim dtHead As DataTable
        Dim dtHariJadwal As DataTable


        dtRuang = GetDataTbl("select idUnit as idUnit, Wipem as IdWipem from TblJual where idCabang='" & txtidcabang.Text & "'  and idUnit='" & txtidunit.Text & "'    group by idUnit,Wipem order by idUnit,Wipem ")
        dtHead = GetDataTbl("SELECT  idUnit as idUnit, Wipem as IdWipem from TblJual where idCabang='" & txtidcabang.Text & "'  and idUnit='" & txtidunit.Text & "'")



        'jam = GetDataTbl("select idruang,SUBSTRING(convert(varchar(16),jammasuk,121), 12, 5) as jammasuk,SUBSTRING(convert(varchar(16),jamkeluar,121), 12, 5) as jamkeluar from JadwalUASSenin where Kurikulum = " & DataLineDetail.Rows(i).Item("Kurikulum") & " and IdMk = '" & DataLineDetail.Rows(i).Item("IDMK") & "' and Prodi = '" & ddlProdi.Text & "' and IdKampus = '" & txtIdKampus.Text & "' and IdFakultas = '" & txtIdFakultas.Text & "' and Kelas = '" & DataLineDetail.Rows(i).Item("Kelas") & "' and Semester = " & CInt(ddlSemester.Text) & " and TA = " & CInt(txtTA.Text) & " ")
        'str = GetDataTbl("select NPM,Nama,HadirUjianAkhir,LayakIkutUjianAkhir,0 as Status,0 as Piutang,0 as Bayar from JadwalUASMhsSenin2 where Kurikulum = '" & DataLineDetail.Rows(i).Item("Kurikulum") & "' and IdMk = '" & DataLineDetail.Rows(i).Item("IDMK") & "' and Prodi = '" & ddlProdi.Text & "' and IdKampus = '" & txtIdKampus.Text & "' and IdFakultas = '" & txtIdFakultas.Text & "' and Kelas = '" & DataLineDetail.Rows(i).Item("Kelas") & "' and Semester = " & CInt(ddlSemester.Text) & " and TA = " & CInt(txtTA.Text) & " and itemnomk=" & DataLineDetail.Rows(i).Item("itemno") & " and JadwalUASMhsSenin2.layakikutujianakhir='R'")
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
        Dim fntTahoma As iTextSharp.text.Font = FontFactory.GetFont("Tahoma", 9, iTextSharp.text.Font.NORMAL, iTextSharp.text.Color.BLACK)
        Dim fntArial12N As iTextSharp.text.Font = FontFactory.GetFont("Arial", 8, iTextSharp.text.Font.NORMAL, iTextSharp.text.Color.BLACK)
        Dim fntArial12U As iTextSharp.text.Font = FontFactory.GetFont("Arial", 8, iTextSharp.text.Font.UNDERLINE, iTextSharp.text.Color.BLACK)
        Dim fntArial12B As iTextSharp.text.Font = FontFactory.GetFont("Arial", 8, iTextSharp.text.Font.BOLD, iTextSharp.text.Color.BLACK)
        'Dim fntArial12C As iTextSharp.text.Font = FontFactory.GetFont("Arial", 8, iTextSharp.text.Font.UNDEFINED, iTextSharp.text.Color.BLACK)

        Dim fntArial10N As iTextSharp.text.Font = FontFactory.GetFont("Arial", 8, iTextSharp.text.Font.NORMAL, iTextSharp.text.Color.BLACK)
        Dim fntArial10U As iTextSharp.text.Font = FontFactory.GetFont("Arial", 10, iTextSharp.text.Font.UNDERLINE, iTextSharp.text.Color.BLACK)
        Dim fntArial10B As iTextSharp.text.Font = FontFactory.GetFont("Arial", 10, iTextSharp.text.Font.BOLD, iTextSharp.text.Color.BLACK)
        Dim fntArial10Bw As iTextSharp.text.Font = FontFactory.GetFont("Arial", 10, iTextSharp.text.Font.BOLD, iTextSharp.text.Color.WHITE)
        Dim fntArial14U As iTextSharp.text.Font = FontFactory.GetFont("Arial", 14, iTextSharp.text.Font.NORMAL, iTextSharp.text.Color.BLACK)
        Dim BaskervileOldFace36 As iTextSharp.text.Font = FontFactory.GetFont("Arial", 36, iTextSharp.text.Font.BOLD, iTextSharp.text.Color.BLACK)
        Dim BaskervileOldFace As iTextSharp.text.Font = FontFactory.GetFont("Arial", 20, iTextSharp.text.Font.BOLD, iTextSharp.text.Color.BLACK)


        If dtHead.Rows.Count >= 1 Then
            'Dim myWidth As Single
            'Dim myHeight As Single
            Dim leftMargin As Single = 11
            Dim rightMargin As Single = 11
            Dim topMargin As Single = 11
            Dim bottomMargin As Single = 11
            Dim filepath As String = Server.MapPath("PDF/RealisasiAngsuran" & Format(Now, "dd-MM-yyyy") & ".pdf")
            Using fs As New FileStream(Server.MapPath("PDF/RealisasiAngsuran" & Format(Now, "dd-MM-yyyy") & ".pdf"), FileMode.Create)
                Dim doc As New iTextSharp.text.Document(iTextSharp.text.PageSize.LEGAL.Rotate, leftMargin, rightMargin, topMargin, bottomMargin)
                PdfWriter.GetInstance(doc, fs)
                doc.AddTitle("Daftar Realisasi Angsuran Nasabah ")
                doc.Open()


                Dim myTableCol As PdfPTable = New PdfPTable(7)
                Dim sglTblHdWidthsCol As Single() = New Single(6) {}

                sglTblHdWidthsCol(0) = 2.0F 'No
                sglTblHdWidthsCol(1) = 8.0F 'No Pinjaman
                sglTblHdWidthsCol(2) = 8.0F 'Tanggal Pinjaman
                sglTblHdWidthsCol(3) = 25.0F 'Nama Nasabah
                sglTblHdWidthsCol(4) = 8.0F ' Saldo Awal
                sglTblHdWidthsCol(5) = 8.0F 'Realiasasi
                sglTblHdWidthsCol(6) = 8.0F 'Saldo Akhir

                'myTable.LockedWidth = True
                myTableCol.HorizontalAlignment = Element.ALIGN_CENTER
                myTableCol.WidthPercentage = 100
                myTableCol.SetWidths(sglTblHdWidthsCol)

                phrase = New Phrase("", fntTahoma) : cell = PhraseCell(phrase, PdfPCell.ALIGN_CENTER, iTextSharp.text.Color.BLACK) : cell.Border = PdfCell.NO_BORDER : cell.Colspan = 10 : myTableCol.AddCell(cell)
                phrase = New Phrase("", fntTahoma) : cell = PhraseCell(phrase, PdfPCell.ALIGN_CENTER, iTextSharp.text.Color.BLACK) : cell.Border = PdfCell.NO_BORDER : cell.Colspan = 10 : myTableCol.AddCell(cell)
                phrase = New Phrase("", fntTahoma) : cell = PhraseCell(phrase, PdfPCell.ALIGN_CENTER, iTextSharp.text.Color.BLACK) : cell.Border = PdfCell.NO_BORDER : cell.Colspan = 10 : myTableCol.AddCell(cell)
                phrase = New Phrase("", fntTahoma) : cell = PhraseCell(phrase, PdfPCell.ALIGN_CENTER, iTextSharp.text.Color.BLACK) : cell.Border = PdfCell.NO_BORDER : cell.Colspan = 10 : myTableCol.AddCell(cell)



                Dim akmp As String = ""
                akmp = txtnamacabang.Text
                Dim akmp2 As String = txtnamaUnit.Text
                Dim akmp3 As String = "REALISASI ANGSURAN NASABAH"
                Dim uq As Paragraph = New Paragraph(akmp, BaskervileOldFace36)
                uq.Alignment = Element.ALIGN_CENTER
                doc.Add(uq)
                doc.Add(New Paragraph(" "))

                Dim uq2 As Paragraph = New Paragraph(akmp2, BaskervileOldFace)
                uq2.Alignment = Element.ALIGN_CENTER
                doc.Add(uq2)
                doc.Add(New Paragraph(" "))

                Dim uq3 As Paragraph = New Paragraph(akmp3, BaskervileOldFace)
                uq3.Alignment = Element.ALIGN_CENTER
                doc.Add(uq3)
                doc.Add(New Paragraph(" "))

                For x = 0 To dtRuang.Rows.Count - 1
                    'Kelas grupping 
                    phrase = New Phrase("", fntTahoma) : cell = PhraseCell(phrase, PdfPCell.ALIGN_CENTER, iTextSharp.text.Color.BLACK) : cell.Border = PdfCell.NO_BORDER : cell.Colspan = 11 : myTableCol.AddCell(cell)
                    phrase = New Phrase("", fntTahoma) : cell = PhraseCell(phrase, PdfPCell.ALIGN_CENTER, iTextSharp.text.Color.BLACK) : cell.Border = PdfCell.NO_BORDER : cell.Colspan = 11 : myTableCol.AddCell(cell)
                    phrase = New Phrase("Wipem : " & dtRuang.Rows(x).Item("idWipem"), fntArial10N) : cell = PhraseCell(phrase, PdfPCell.ALIGN_LEFT, iTextSharp.text.Color.BLACK) : cell.Border = PdfCell.NO_BORDER : cell.Colspan = 10 : myTableCol.AddCell(cell)
                    phrase = New Phrase("", fntTahoma) : cell = PhraseCell(phrase, PdfPCell.ALIGN_CENTER, iTextSharp.text.Color.BLACK) : cell.Border = PdfCell.NO_BORDER : cell.Colspan = 11 : myTableCol.AddCell(cell)
                    phrase = New Phrase("", fntTahoma) : cell = PhraseCell(phrase, PdfPCell.ALIGN_CENTER, iTextSharp.text.Color.BLACK) : cell.Border = PdfCell.NO_BORDER : cell.Colspan = 11 : myTableCol.AddCell(cell)

                    ''Header Tabel
                    phrase = New Phrase("No.", fntArial10N) : cell = PhraseCell(phrase, PdfPCell.ALIGN_CENTER, iTextSharp.text.Color.BLACK) : cell.FixedHeight = 15.0F : myTableCol.AddCell(cell)
                    phrase = New Phrase("NOMOR PINJAMAN ", fntArial10N) : cell = PhraseCell(phrase, PdfPCell.ALIGN_CENTER, iTextSharp.text.Color.BLACK) : myTableCol.AddCell(cell)
                    phrase = New Phrase("TANGGAL ", fntArial10N) : cell = PhraseCell(phrase, PdfPCell.ALIGN_CENTER, iTextSharp.text.Color.BLACK) : myTableCol.AddCell(cell)
                    phrase = New Phrase("NAMA NASABAH", fntArial10N) : cell = PhraseCell(phrase, PdfPCell.ALIGN_CENTER, iTextSharp.text.Color.BLACK) : myTableCol.AddCell(cell)
                    phrase = New Phrase("SALDO AWAL ", fntArial10N) : cell = PhraseCell(phrase, PdfPCell.ALIGN_CENTER, iTextSharp.text.Color.BLACK) : myTableCol.AddCell(cell)
                    phrase = New Phrase("ANGSURAN  ", fntArial10N) : cell = PhraseCell(phrase, PdfPCell.ALIGN_CENTER, iTextSharp.text.Color.BLACK) : myTableCol.AddCell(cell)
                    phrase = New Phrase("SALDO AKHIR ", fntArial10N) : cell = PhraseCell(phrase, PdfPCell.ALIGN_CENTER, iTextSharp.text.Color.BLACK) : myTableCol.AddCell(cell)



                    dtHariJadwal = GetDataTbl("select idunit as idunit, wipem as idwipem from Tbljual where Wipem='" & dtRuang.Rows(x).Item("idwipem") & "' and idUnit='" & dtRuang.Rows(x).Item("idUnit") & "' group by idUnit,Wipem order by idUnit,Wipem")

                    For y = 0 To dtHariJadwal.Rows.Count - 1
                        dtHead = GetDataTbl("SELECT CONVERT(VARCHAR(10),a.Tanggal,103) as tanggal,isnull(a.idTrans,'') as nopinjaman,isnull(b.nama,'') as Nama, isnull(a.saldoawal,0) as saldoawal,realisasi=(Select sum(e.jumlah) from TblAngsuran e where e.NoPinjaman=a.idTrans and e.wipem='" & dtRuang.Rows(x).Item("idwipem") & "' and e.idUnit='" & dtRuang.Rows(x).Item("idUnit") & "'),Saldoakhir=(Select a.SaldoAwal from tbljual c where c.idTrans=a.idTrans And c.wipem='" & dtRuang.Rows(x).Item("idwipem") & "' and c.idUnit='" & dtRuang.Rows(x).Item("idUnit") & "') - (Select sum(d.jumlah) from TblAngsuran d where d.NoPinjaman=a.idTrans And d.wipem='" & dtRuang.Rows(x).Item("idwipem") & "' and d.idUnit='" & dtRuang.Rows(x).Item("idUnit") & "')  from Tbljual  a inner join tblnasabah b on b.idnasabah=a.idnasabah  where  a.wipem='" & dtRuang.Rows(x).Item("idwipem") & "' and a.idUnit='" & dtRuang.Rows(x).Item("idUnit") & "'  order by a.idPrimary asc;")

                        For z = 0 To dtHead.Rows.Count - 1
                            phrase = New Phrase((z + 1).ToString() & ".", fntTahoma) : cell = PhraseCell(phrase, PdfPCell.ALIGN_LEFT, iTextSharp.text.Color.BLACK) : cell.FixedHeight = 15.0F : myTableCol.AddCell(cell)
                            phrase = New Phrase(dtHead.Rows(z).Item("nopinjaman"), fntTahoma) : cell = PhraseCell(phrase, PdfPCell.ALIGN_LEFT, iTextSharp.text.Color.BLACK) : myTableCol.AddCell(cell)
                            phrase = New Phrase(dtHead.Rows(z).Item("tanggal"), fntTahoma) : cell = PhraseCell(phrase, PdfPCell.ALIGN_LEFT, iTextSharp.text.Color.BLACK) : myTableCol.AddCell(cell)
                            phrase = New Phrase(dtHead.Rows(z).Item("nama"), fntTahoma) : cell = PhraseCell(phrase, PdfPCell.ALIGN_LEFT, iTextSharp.text.Color.BLACK) : myTableCol.AddCell(cell)
                            phrase = New Phrase(FormatNumber(CDbl(dtHead.Rows(z).Item("saldoawal")), 0), fntTahoma) : cell = PhraseCell(phrase, PdfPCell.ALIGN_LEFT, iTextSharp.text.Color.BLACK) : myTableCol.AddCell(cell)
                            phrase = New Phrase(FormatNumber(CDbl(dtHead.Rows(z).Item("realisasi")), 0), fntTahoma) : cell = PhraseCell(phrase, PdfPCell.ALIGN_LEFT, iTextSharp.text.Color.BLACK) : myTableCol.AddCell(cell)
                            phrase = New Phrase(FormatNumber(CDbl(dtHead.Rows(z).Item("Saldoakhir")), 0), fntTahoma) : cell = PhraseCell(phrase, PdfPCell.ALIGN_LEFT, iTextSharp.text.Color.BLACK) : myTableCol.AddCell(cell)

                            If z < dtHead.Rows.Count - 1 Then
                                'phrase = New Phrase("", fntArial10N) : cell = PhraseCell(phrase, PdfPCell.ALIGN_CENTER, iTextSharp.text.Color.BLACK) : cell.Colspan = 10 : myTableCol.AddCell(cell)
                            End If
                        Next z
                    Next y
                Next x

                phrase = New Phrase("", fntTahoma) : cell = PhraseCell(phrase, PdfPCell.ALIGN_LEFT, iTextSharp.text.Color.BLACK) : cell.Border = PdfCell.NO_BORDER : cell.Colspan = 9 : myTableCol.AddCell(cell)
                phrase = New Phrase("", fntTahoma) : cell = PhraseCell(phrase, PdfPCell.ALIGN_CENTER, iTextSharp.text.Color.BLACK) : cell.Border = PdfCell.NO_BORDER : cell.Colspan = 9 : myTableCol.AddCell(cell)
                phrase = New Phrase("", fntTahoma) : cell = PhraseCell(phrase, PdfPCell.ALIGN_CENTER, iTextSharp.text.Color.BLACK) : cell.Border = PdfCell.NO_BORDER : cell.Colspan = 9 : myTableCol.AddCell(cell)

                phrase = New Phrase("Cetak Tanggal", fntTahoma) : cell = PhraseCell(phrase, PdfPCell.ALIGN_LEFT, iTextSharp.text.Color.BLACK) : cell.Border = PdfCell.NO_BORDER : cell.Colspan = 4 : myTableCol.AddCell(cell)
                phrase = New Phrase(":", fntTahoma) : cell = PhraseCell(phrase, PdfPCell.ALIGN_LEFT, iTextSharp.text.Color.BLACK) : cell.Border = PdfCell.NO_BORDER : cell.Colspan = 1 : myTableCol.AddCell(cell)
                phrase = New Phrase(Format(Now, "dd/MM/yyyy"), fntTahoma) : cell = PhraseCell(phrase, PdfPCell.ALIGN_LEFT, iTextSharp.text.Color.BLACK) : cell.Border = PdfCell.NO_BORDER : cell.Colspan = 10 : myTableCol.AddCell(cell)
                phrase = New Phrase("", fntArial12N) : cell = PhraseCell(phrase, PdfPCell.ALIGN_LEFT, iTextSharp.text.Color.BLACK) : cell.Border = PdfCell.NO_BORDER : cell.Colspan = 5 : myTableCol.AddCell(cell)

                phrase = New Phrase("", fntTahoma) : cell = PhraseCell(phrase, PdfPCell.ALIGN_LEFT, iTextSharp.text.Color.BLACK) : cell.Border = PdfCell.NO_BORDER : cell.Colspan = 20 : myTableCol.AddCell(cell)
                phrase = New Phrase("", fntTahoma) : cell = PhraseCell(phrase, PdfPCell.ALIGN_CENTER, iTextSharp.text.Color.BLACK) : cell.Border = PdfCell.NO_BORDER : cell.Colspan = 20 : myTableCol.AddCell(cell)
                phrase = New Phrase("", fntTahoma) : cell = PhraseCell(phrase, PdfPCell.ALIGN_CENTER, iTextSharp.text.Color.BLACK) : cell.Border = PdfCell.NO_BORDER : cell.Colspan = 20 : myTableCol.AddCell(cell)

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
