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

Partial Class Nasabah
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
                txtidnasabah.Text = HttpUtility.UrlDecode(Request.QueryString("idNasabah"))
                If Not String.IsNullOrEmpty(txtidnasabah.Text.Trim()) Then
                    QueryAccount(txtidnasabah.Text.Trim(), True)
                    lnkSave.Text = "Update"
                    txtidnasabah.Attributes.Add("readonly", "readonly")
                    btnNew2.Visible = False
                    lnkDelete.Visible = True

                Else
                    NOOTOMATIS()
                    lnkDelete.Visible = False
                End If

            End If
            txtidnasabah.Attributes.Add("readonly", "readonly")
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
            Dim successScript As String = "setTimeout(function(){ Swal.fire({ icon: 'warning', title: 'Cabang Tidak Boleh Kosong', showConfirmButton: false, showCancelButton: false, timer: 2000, timerProgressBar: true }); }, 100);"
            ScriptManager.RegisterStartupScript(Me, Me.[GetType](), "Popup", successScript, True)
            Exit Sub
            Exit Sub
        ElseIf String.IsNullOrEmpty(ddlunit.Text.Trim()) Then
            Dim successScript As String = "setTimeout(function(){ Swal.fire({ icon: 'warning', title: 'Unit Tidak Boleh Kosong', showConfirmButton: false, showCancelButton: false, timer: 2000, timerProgressBar: true }); }, 100);"
            ScriptManager.RegisterStartupScript(Me, Me.[GetType](), "Popup", successScript, True)
            Exit Sub
        ElseIf String.IsNullOrEmpty(txttglgabung.Value.Trim()) Then
            Dim successScript As String = "setTimeout(function(){ Swal.fire({ icon: 'warning', title: 'Tanggal Gabung Tidak Boleh Kosong', showConfirmButton: false, showCancelButton: false, timer: 2000, timerProgressBar: true }); }, 100);"
            ScriptManager.RegisterStartupScript(Me, Me.[GetType](), "Popup", successScript, True)
            Exit Sub
        ElseIf String.IsNullOrEmpty(dtpTgllahir.Value.Trim()) Then
            Dim successScript As String = "setTimeout(function(){ Swal.fire({ icon: 'warning', title: 'Tanggal Lahir Tidak Boleh Kosong', showConfirmButton: false, showCancelButton: false, timer: 2000, timerProgressBar: true }); }, 100);"
            ScriptManager.RegisterStartupScript(Me, Me.[GetType](), "Popup", successScript, True)
            Exit Sub
        End If

        Dim strUID As String = Trim(Session.Item("UserAuthentication").ToString)
        Dim ascabang As String = Left(ddlcabang.Text, 2)
        Dim asunit As String = Left(ddlunit.Text, 4)

        'Check untuk melakukan tambah data
        If Not CheckAdaData(txtidnasabah.Text.Trim(), "idNasabah", "TblNasabah") Then
            connectDB()
            mySqlCon.Open()
            mySqlCmd = New System.Data.OleDb.OleDbCommand()

            Dim Transaction = mySqlCon.BeginTransaction(IsolationLevel.ReadCommitted)
            mySqlCmd.Connection = mySqlCon
            mySqlCmd.Transaction = Transaction

            Try
                mySqlCmd.CommandText = "INSERT INTO Tblnasabah (IdNasabah,IdCabang,IdUnit, Nama,Alamat,TempatLahir,TGLLAHIR,IdKTP,NoKK,TglDaftar, Domisili,HP,LastUpdate,UseridUpdate) VALUES ('" _
                    & txtidnasabah.Text & "', '" & ddlcabang.Text & "', '" & ddlunit.Text & "','" & txtnama.Text & "','" & txtalamat.Text & "', '" & txttempat.Text & "', '" & clsEnt.CDateME5(dtpTgllahir.Value) & "', '" & txtktp.Text & "', '" & txtkk.Text & "', '" & clsEnt.CDateME5(txttglgabung.Value) & "', '" & txtdomisili.Text & "','" & txthp.Text & "',Getdate(),'" & strUID & "'); "


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
                Response.Redirect("NasabahList.aspx")
            End Try
        Else
            connectDB()
            mySqlCon.Open()
            mySqlCmd = New System.Data.OleDb.OleDbCommand()

            Dim Transaction = mySqlCon.BeginTransaction(IsolationLevel.ReadCommitted)
            mySqlCmd.Connection = mySqlCon
            mySqlCmd.Transaction = Transaction

            Try
                mySqlCmd.CommandText = "UPDATE TblNasabah SET Nama = '" & txtnama.Text & "',Alamat = '" & txtalamat.Text & "',TEMPATLAHIR = '" & txttempat.Text & "',TglLahir ='" & clsEnt.CDateME5(dtpTgllahir.Value) & "',HP = '" & txthp.Text & "',Domisili = '" & txtdomisili.Text & "',idKTP = '" & txtktp.Text & "',NoKK='" & txtkk.Text & "', TglDaftar='" & clsEnt.CDateME5(dtpTgllahir.Value) & "',IDCABANG='" & ddlcabang.Text & "', IDUNIT='" & ddlunit.Text & "',LastUpdate=getdate(),UseridUpdate='" & strUID & "' WHERE idNasabah= '" & txtidnasabah.Text.Trim() & "'; "
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
                Response.Redirect("NasabahList.aspx")
            End Try
        End If
    End Sub



    Protected Sub btnNew_Click(sender As Object, e As System.EventArgs) Handles btnNew.Click
        Response.Redirect("NasabahList.aspx")
        Session("Mode") = "New"
    End Sub


#End Region

    Private Sub ClearForm()
        txtidnasabah.Text = ""
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
            command = New System.Data.OleDb.OleDbCommand("SELECT IdNasabah as IdNasabah, IdCabang as IdCabang, IdUnit as IdUnit, Nama as Nama, Alamat as Alamat, TempatLahir as TempatLahir, CONVERT(VARCHAR(10),TGLLAHIR,103) AS TGLLAHIR, IdKTP as IdKTP, NoKK as NoKK, CONVERT(VARCHAR(10),TglDaftar,103)  AS TglDaftar, Domisili as Domisili, HP as HP FROM TblNasabah WHERE idNasabah = '" & a_Str & "'; ", mySqlCon)
            mySqlReader = command.ExecuteReader
            If mySqlReader.Read Then
                txtidnasabah.Text = mySqlReader("idNasabah").ToString()
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
                BindGridNasabah()
                'txtid.Attributes.Add("readonly", "readonly")
                lnkDelete.Visible = True
                mySqlReader.Close()
                mySqlCon.Close()
                mySqlCon.Dispose()
                command.Parameters.Clear()

            Else
                txtidnasabah.Text = ""
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
        Dim sTrue As DataTable = GetDataTbl("select Max(idnasabah) + 1 as total from TblNasabah")
        Dim nourut As String = CInt(sTrue.Rows(0).Item("total"))
        '     Dim NO As String = clsEnt.ReturnOneFieldConn("select top(1)idbarang + 1 as nnn from Tblbarang order by idbarang desc ", "", "", "nnn", "connuq")
        txtidnasabah.Text = IIf(nourut = "", "00001", "0000" + nourut)

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
            mySqlCmd.CommandText = "DELETE FROM TblNasabah WHERE idNasabah = '" & txtidnasabah.Text.Trim() & "'; "
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
            Response.Redirect("NasabahList.aspx")
        End Try
    End Sub

    Protected Sub UploadNasabah(Sender As [Object], e As EventArgs)
        Dim sUsr As String = Session("UserAuthentication")
        Dim sTrue2 As DataTable
        Dim sTrue3 As DataTable
        Dim sTrue As DataTable




        sTrue = GetDataTbl("select Count(idNasabah) as cnt from TblNasabah where idNasabah='" & txtidnasabah.Text & "'")
        sTrue3 = GetDataTbl("select idNasabah as cnt from TblNasabah where idNasabah='" & txtidnasabah.Text & "'")
        sTrue2 = GetDataTbl("select Max(idNasabah) + 1 as cnt from TblNasabah where idNasabah='" & txtidnasabah.Text & "'")

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
                    Dim query As String = "insert into tblFilesNasabah   values (@Name,@ContentType,@Data,@idNasabah,@LastUpdate,@ketPrimary)"
                    Using cmd As New SqlCommand(query)
                        cmd.Connection = con
                        cmd.Parameters.AddWithValue("@Name", filename)
                        cmd.Parameters.AddWithValue("@ContentType", contentType)
                        cmd.Parameters.AddWithValue("@Data", bytes)
                        cmd.Parameters.AddWithValue("@idNasabah", txtidnasabah.Text)
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
        BindGridNasabah()

    End Sub

    Protected Sub DownloadFile(sender As Object, e As EventArgs)
        Dim Name As String = TryCast(sender, LinkButton).CommandArgument.ToString
        Dim bytes As Byte()
        Dim filename As String, contentType As String
        Dim constr As String = ConfigurationManager.ConnectionStrings("connuq").ConnectionString
        Using con As New SqlConnection(constr)
            Using cmd As New SqlCommand()
                cmd.CommandText = "select top 1 Name,ContentType,Data,LastUpdate from  tblFilesNasabah  where Name=@Name and idNasabah= @idNasabah order by LastUpdate Desc "
                cmd.Parameters.AddWithValue("@Name", Name)
                cmd.Parameters.AddWithValue("@idNasabah", txtidnasabah.Text)
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
        BindGridNasabah()
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


    Protected Sub DeleteMyFileNasabah(sender As Object, e As EventArgs)
        Dim Name As String = TryCast(sender, LinkButton).CommandArgument.ToString
        Dim constr As String = ConfigurationManager.ConnectionStrings("connuq").ConnectionString
        Using con As New SqlConnection(constr)
            Using cmd As New SqlCommand()
                cmd.CommandText = "delete from  tblFilesNasabah where Name=@Name and idNasabah= @idNasabah ;"
                cmd.Parameters.AddWithValue("@Name", Name)
                cmd.Parameters.AddWithValue("@idNasabah", txtidnasabah.Text)
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

        BindGridNasabah()
    End Sub
    Public Sub BindGridNasabah()
        Dim constr As String = ConfigurationManager.ConnectionStrings("connuq").ConnectionString
        Using con As New SqlConnection(constr)
            Using cmd As New SqlCommand()
                cmd.CommandText = "Select Name AS Name,LastUpdate AS LastUpdate from tblFilesNasabah  where idNasabah= '" & txtidnasabah.Text & "' order by LastUpdate Desc; "
                cmd.Connection = con
                con.Open()
                gvDokumen.DataSource = cmd.ExecuteReader()
                gvDokumen.DataBind()

                con.Close()
            End Using
        End Using
    End Sub

End Class
