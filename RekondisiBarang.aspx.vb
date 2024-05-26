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

Partial Class RekondisiBarang
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
                txtidbarang.Text = HttpUtility.UrlDecode(Request.QueryString("idTrans"))
                If Not String.IsNullOrEmpty(txtidbarang.Text.Trim()) Then
                    QueryAccount(txtidbarang.Text.Trim(), True)
                    lnkSave.Text = "Update"
                    txtidbarang.Attributes.Add("readonly", "readonly")
                    btnNew2.Visible = False
                    lnkDelete.Visible = True

                Else
                    NOOTOMATIS()
                    lnkDelete.Visible = False
                End If
                txtidsupplier.Attributes.Add("readonly", "readonly")
                txtnama.Attributes.Add("readonly", "readonly")
                txtmerk.Attributes.Add("readonly", "readonly")
                txtjenis.Attributes.Add("readonly", "readonly")
                txtnomesin.Attributes.Add("readonly", "readonly")
                txtnorangka.Attributes.Add("readonly", "readonly")

            End If
        End If
    End Sub
#End Region



#Region "Button"

    'Protected Sub lnkDelete_Click(sender As Object, e As System.EventArgs)

    'End Sub

    Protected Sub lnkSave_Click(sender As Object, e As System.EventArgs) Handles lnkSave.Click
        'CHECK REQUIRED FIELDS
        If String.IsNullOrEmpty(txtidbarang.Text.Trim()) Then
            Dim successScript As String = "setTimeout(function(){ Swal.fire({ icon: 'warning', title: 'Id Kampus Tidak Boleh Kosong', showConfirmButton: false, showCancelButton: false, timer: 2000, timerProgressBar: true }); }, 100);"
            ScriptManager.RegisterStartupScript(Me, Me.[GetType](), "Popup", successScript, True)
            Exit Sub
        ElseIf String.IsNullOrEmpty(txtnama.Text.Trim()) Then
            Dim successScript As String = "setTimeout(function(){ Swal.fire({ icon: 'warning', title: 'Nama Tidak Boleh Kosong', showConfirmButton: false, showCancelButton: false, timer: 2000, timerProgressBar: true }); }, 100);"
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
        End If

        Dim strUID As String = Trim(Session.Item("UserAuthentication").ToString)
        Dim ascabang As String = Left(ddlcabang.Text, 2)
        Dim asunit As String = Left(ddlunit.Text, 4)
        Dim biaya1, biaya2, biaya3, hasil As Decimal
        biaya1 = txtbiayarekondisi.Text
        biaya2 = txtbiayakomisi.Text
        biaya3 = txtbiayaadm.Text
        hasil = biaya1 + biaya2 + biaya3
        'Check untuk melakukan tambah data
        If Not CheckAdaData(txtidbarang.Text.Trim(), "idTrans", "TblRekondisi") Then
            connectDB()
            mySqlCon.Open()
            mySqlCmd = New System.Data.OleDb.OleDbCommand()

            Dim Transaction = mySqlCon.BeginTransaction(IsolationLevel.ReadCommitted)
            mySqlCmd.Connection = mySqlCon
            mySqlCmd.Transaction = Transaction

            Try
                mySqlCmd.CommandText = "INSERT INTO TblRekondisi (idTrans,IdCabang,Idunit,NoPlat,IdSupplier,NamaPemilik,Merk,Jenis,NoMesin,NoRangka,BiayaRekondisi,BiayaAdministrasi,BiayaKomisi,Total,Tanggal,LastUpdate,UseridUpdate,Keterangan) VALUES ('" _
                    & txtidbarang.Text & "', '" & ddlcabang.Text & "', '" & ddlunit.Text & "','" & txtplat.Text & "','" & txtidsupplier.Text & "','" & txtnama.Text & "', '" & txtmerk.Text & "', '" & txtjenis.Text & "','" & txtnomesin.Text & "','" & txtnorangka.Text & "'," & txtbiayarekondisi.Text & "," & txtbiayaadm.Text & "," & txtbiayakomisi.Text & "," & hasil & ",'" & clsEnt.CDateME5(dtgl1.Value) & "',Getdate(),'" & strUID & "','" & txtarea.Text & "'); "


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
                Response.Redirect("RekondisiBarangList.aspx")
            End Try
        Else
            connectDB()
            mySqlCon.Open()
            mySqlCmd = New System.Data.OleDb.OleDbCommand()

            Dim Transaction = mySqlCon.BeginTransaction(IsolationLevel.ReadCommitted)
            mySqlCmd.Connection = mySqlCon
            mySqlCmd.Transaction = Transaction

            Try
                mySqlCmd.CommandText = "UPDATE TblRekondisi SET keterangan='" & txtarea.Text & "',txtidsupplier='" & txtidsupplier.Text & "',NamaPemilik='" & txtnama.Text & "',Merk='" & txtmerk.Text & "' ,Jenis='" & txtjenis.Text & "',NoMesin='" & txtnomesin.Text & "' ,NoRangka='" & txtnorangka.Text & "',Tanggal ='" & clsEnt.CDateME5(dtgl1.Value) & "',BiayaRekondisi=" & txtbiayarekondisi.Text & ",BiayaAdministrasi=" & txtbiayaadm.Text & " ,BiayaKomisi=" & txtbiayakomisi.Text & ",Total=" & hasil & ",LastUpdate=Getdate(),UseridUpdate='" & strUID & "' WHERE NoPlat= '" & txtplat.Text.Trim() & "'; "
                mySqlCmd.ExecuteNonQuery()

                Transaction.Commit()
                Session("Mode") = "Edit"
                Dim successScript As String = "setTimeout(function(){ Swal.fire({ icon: 'success', title: 'Saved', showConfirmButton: false, showCancelButton: false, timer: 2000, timerProgressBar: true }); }, 100);"
                ScriptManager.RegisterStartupScript(Me, Me.[GetType](), "Popup", successScript, True)
                QueryAccount(txtidbarang.Text.Trim(), True)

            Catch ex As Exception
                Transaction.Rollback()
                Dim successScript As String = "setTimeout(function(){ Swal.fire({ icon: 'error', title: 'Error updating data', showConfirmButton: false, showCancelButton: false, timer: 2000, timerProgressBar: true }); }, 100);"
                ScriptManager.RegisterStartupScript(Me, Me.[GetType](), "Popup", successScript, True)
            Finally
                mySqlCon.Close()
                mySqlCon.Dispose()
                mySqlCmd.Parameters.Clear()
                ClearForm()
                Response.Redirect("RekondisiBarangList.aspx")
            End Try
        End If
    End Sub



    Protected Sub btnNew_Click(sender As Object, e As System.EventArgs) Handles btnNew.Click
        Response.Redirect("RekondisiBarangList.aspx")
        Session("Mode") = "New"
    End Sub


#End Region

    Private Sub ClearForm()
        txtidbarang.Text = ""
        txtplat.Text = ""
        txtnama.Text = ""
        txtmerk.Text = ""
        txtjenis.Text = ""
        txtnomesin.Text = ""
        txtnorangka.Text = ""
        dtgl1.Value = ""
        ddlcabang.Text = ""
        ddlunit.Text = ""
        txtarea.Text = ""
        txtbiayaadm.Text = 0
        txtbiayakomisi.Text = 0
        txtbiayarekondisi.Text = 0
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
            command = New System.Data.OleDb.OleDbCommand("SELECT idTrans as idTrans, IdCabang as IdCabang, Idunit as Idunit, NoPlat as NoPlat, NamaPemilik as NamaPemilik, Merk as Merk, Jenis as Jenis,BiayaRekondisi as BiayaRekondisi, BiayaAdministrasi as BiayaAdministrasi,BiayaKomisi as BiayaKomisi, NoMesin as NoMesin, NoRangka as NoRangka, CONVERT(VARCHAR(10),Tanggal,103) as Tanggal,Keterangan AS Keterangan FROM TblRekondisi WHERE idTrans= '" & a_Str & "'; ", mySqlCon)
            mySqlReader = command.ExecuteReader
            If mySqlReader.Read Then
                txtidbarang.Text = mySqlReader("idTrans").ToString()
                txtplat.Text = mySqlReader("NoPlat").ToString()
                txtnama.Text = mySqlReader("NamaPemilik").ToString()
                txtmerk.Text = mySqlReader("Merk").ToString()
                txtjenis.Text = mySqlReader("Jenis").ToString()
                txtnomesin.Text = mySqlReader("NoMesin").ToString()
                txtbiayaadm.Text = mySqlReader("BiayaAdministrasi").ToString()
                txtbiayakomisi.Text = mySqlReader("BiayaKomisi").ToString()
                txtbiayarekondisi.Text = mySqlReader("BiayaRekondisi").ToString()
                txtnorangka.Text = mySqlReader("NoRangka").ToString()
                ddlcabang.Text = mySqlReader("IdCabang").ToString()
                ddlunit.Text = mySqlReader("Idunit").ToString()
                txtarea.Text = mySqlReader("keterangan").ToString()
                dtgl1.Value = mySqlReader("Tanggal").ToString()
                BindGrnoplat()
                lnkDelete.Visible = True
                mySqlReader.Close()
                mySqlCon.Close()
                mySqlCon.Dispose()
                command.Parameters.Clear()

            Else
                txtidbarang.Text = ""
                txtplat.Text = ""
                txtnama.Text = ""
                txtmerk.Text = ""
                txtjenis.Text = ""
                txtnomesin.Text = ""
                txtnorangka.Text = ""
                dtgl1.Value = ""
                ddlcabang.Text = ""
                ddlunit.Text = ""
                txtbiayaadm.Text = 0
                txtbiayakomisi.Text = 0
                txtbiayarekondisi.Text = 0
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
        Dim sTrue As DataTable = GetDataTbl("select Max(idBarang) + 1 as total from Tblbarang")
        Dim nourut As String = CInt(sTrue.Rows(0).Item("total"))
        '     Dim NO As String = clsEnt.ReturnOneFieldConn("select top(1)idbarang + 1 as nnn from Tblbarang order by idbarang desc ", "", "", "nnn", "connuq")
        txtidbarang.Text = IIf(nourut = "", "00001", "0000" + nourut)
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
            mySqlCmd.CommandText = "DELETE FROM Tblbarang WHERE idBarang = '" & txtidbarang.Text.Trim() & "'; "
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
            Response.Redirect("RekondisiBarangList.aspx")
        End Try
    End Sub

    Protected Sub UploadRekondisi(Sender As [Object], e As EventArgs)
        Dim sUsr As String = Session("UserAuthentication")
        Dim sTrue2 As DataTable
        Dim sTrue3 As DataTable
        Dim sTrue As DataTable




        sTrue = GetDataTbl("select Count(idtrans) as cnt from TblRekondisi where noplat='" & txtplat.Text & "'")
        sTrue3 = GetDataTbl("select noplat as cnt from TblRekondisi where noplat='" & txtplat.Text & "'")
        sTrue2 = GetDataTbl("select Max(idtrans) + 1 as cnt from TblRekondisi where noplat='" & txtplat.Text & "'")

        Dim Keterangan As String = ""
        If CInt(sTrue.Rows(0).Item("cnt")) > 0 Then
            Keterangan = sTrue3.Rows(0).Item("cnt") & "" & sTrue2.Rows(0).Item("cnt")
        Else
            Keterangan = sUsr & "" & 1
        End If



        Dim xTgl As String = Format(Now, "yyyy-MM-dd HH:mm:ss")
        Dim filename As String = Path.GetFileName(FileUploadDok.PostedFile.FileName)
        Dim contentType As String = FileUploadDok.PostedFile.ContentType
        Using fs As Stream = FileUploadDok.PostedFile.InputStream
            Using br As New BinaryReader(fs)
                'Dim bytes As Byte() = br.ReadBytes(DirectCast(fs.Length, Int32))
                Dim bytes As Byte() = br.ReadBytes(fs.Length)
                Dim constr As String = ConfigurationManager.ConnectionStrings("connuq").ConnectionString
                Using con As New SqlConnection(constr)
                    Dim query As String = "insert into tblFilesRekondisi   values (@Name,@ContentType,@Data,@noplat,@LastUpdate,@ketPrimary)"
                    Using cmd As New SqlCommand(query)
                        cmd.Connection = con
                        cmd.Parameters.AddWithValue("@Name", filename)
                        cmd.Parameters.AddWithValue("@ContentType", contentType)
                        cmd.Parameters.AddWithValue("@Data", bytes)
                        cmd.Parameters.AddWithValue("@noplat", txtplat.Text)
                        cmd.Parameters.AddWithValue("@LastUpdate", xTgl)
                        cmd.Parameters.AddWithValue("@ketPrimary", Keterangan)
                        con.Open()
                        cmd.ExecuteNonQuery()
                        con.Close()
                    End Using


                End Using

            End Using
        End Using
        'Response.Redirect(Request.Url.AbsoluteUri)
        BindGrnoplat()

    End Sub

    Protected Sub DownloadFile(sender As Object, e As EventArgs)
        Dim Name As String = TryCast(sender, LinkButton).CommandArgument.ToString
        Dim bytes As Byte()
        Dim filename As String, contentType As String
        Dim constr As String = ConfigurationManager.ConnectionStrings("connuq").ConnectionString
        Using con As New SqlConnection(constr)
            Using cmd As New SqlCommand()
                cmd.CommandText = "select top 1 Name,ContentType,Data,LastUpdate from  tblFilesRekondisi  where Name=@Name and noplat= @noplat order by LastUpdate Desc "
                cmd.Parameters.AddWithValue("@Name", Name)
                cmd.Parameters.AddWithValue("@noplat", txtplat.Text)
                cmd.Connection = con
                con.Open()
                Using str As SqlDataReader = cmd.ExecuteReader()
                    str.Read()
                    bytes = DirectCast(str("Data"), Byte())
                    contentType = str("contentType").ToString()
                    filename = str("Name").ToString()
                End Using
                con.Close()
            End Using
        End Using
        BindGrnoplat()
        Response.Clear()
        Response.Buffer = True
        Response.Charset = ""
        Response.Cache.SetCacheability(HttpCacheability.NoCache)
        Response.ContentType = contentType
        Response.AppendHeader("Content-Disposition", "attachment;filename=" + filename)
        Response.BinaryWrite(bytes)
        Response.Flush()
        Response.End()

    End Sub


    Protected Sub DeleteMyFileRekondisi(sender As Object, e As EventArgs)
        Dim Name As String = TryCast(sender, LinkButton).CommandArgument.ToString
        Dim constr As String = ConfigurationManager.ConnectionStrings("connuq").ConnectionString
        Using con As New SqlConnection(constr)
            Using cmd As New SqlCommand()
                cmd.CommandText = "delete from  tblFilesRekondisi where Name=@Name and noplat= @noplat ;"
                cmd.Parameters.AddWithValue("@Name", Name)
                cmd.Parameters.AddWithValue("@noplat", txtplat.Text)
                cmd.Connection = con
                con.Open()
                Try
                    cmd.ExecuteNonQuery()
                    cmd.Dispose()
                Catch ex As Exception

                End Try

                con.Close()
            End Using
        End Using

        BindGrnoplat()
    End Sub
    Public Sub BindGrnoplat()
        Dim constr As String = ConfigurationManager.ConnectionStrings("connuq").ConnectionString
        Using con As New SqlConnection(constr)
            Using cmd As New SqlCommand()
                cmd.CommandText = "Select Name AS Name,LastUpdate AS LastUpdate from tblFilesRekondisi  where noplat= '" & txtplat.Text & "' order by LastUpdate Desc; "
                cmd.Connection = con
                con.Open()
                gvDokumen.DataSource = cmd.ExecuteReader()
                gvDokumen.DataBind()

                con.Close()
            End Using
        End Using
    End Sub

End Class
