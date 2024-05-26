Imports Microsoft.VisualBasic
Imports System.Web
Imports System.Web.Services
Imports System.Web.Services.Protocols
Imports System.Data
Imports System.Collections
Imports System.Data.SqlClient
Imports System.Web.Configuration
Imports System.Web.UI
Imports System.Web.UI.WebControls
Imports System.IO
Imports System
Imports System.Text
Imports System.Security.Cryptography
Imports System.Net.Mail
Imports System.Net.Mime

Public Class clsEnt

    Public Class ListItemComparer
        Implements IComparer(Of ListItem)

        Public Function Compare(ByVal x As ListItem, ByVal y As ListItem) As Integer _
            Implements IComparer(Of ListItem).Compare

            Dim c As New CaseInsensitiveComparer
            Return c.Compare(x.Text, y.Text)
        End Function

    End Class

    Public Shared sConns As String = "Data Source=DENNYJUNAIDI\MSSQLSERVER2019;Initial Catalog=Leasing;Trusted_Connection=false;User Id=sa;Password=sasa"

    Public Shared Function bBln(ByVal abln As Integer) As String
        Dim xbln As String = ""
        If abln = 1 Then
            xbln = "Januari"
        ElseIf abln = 2 Then
            xbln = "Februari"
        ElseIf abln = 3 Then
            xbln = "Maret"
        ElseIf abln = 4 Then
            xbln = "April"
        ElseIf abln = 5 Then
            xbln = "Mei"
        ElseIf abln = 6 Then
            xbln = "Juni"
        ElseIf abln = 7 Then
            xbln = "Juli"
        ElseIf abln = 8 Then
            xbln = "Agustus"
        ElseIf abln = 9 Then
            xbln = "September"
        ElseIf abln = 10 Then
            xbln = "Oktober"
        ElseIf abln = 11 Then
            xbln = "November"
        ElseIf abln = 12 Then
            xbln = "Desember"
        Else
            xbln = ""
        End If
        Return xbln
    End Function
    Public Shared Function formatBln(ByVal aTgl As Date) As String
        If Len(Trim(Str(Month(aTgl)))) = 1 Then
            formatBln = "0" & Format(Month(aTgl), "#0")
        Else
            formatBln = Format(Month(aTgl), "#0")
        End If
        Return formatBln
    End Function

    Public Shared Sub ExportDTExcel(ByVal FileName As String, ByVal SavePath As String, ByVal objDataReader As DataTable)
        ' Dim i As Integer
        Dim sb As New System.Text.StringBuilder
        Try
            Dim intColumn, intColumnValue As Integer
            Dim row As DataRow
            For intColumn = 0 To objDataReader.Columns.Count - 1
                sb.Append(objDataReader.Columns(intColumn).ColumnName)
                If intColumnValue <> objDataReader.Columns.Count - 1 Then
                    sb.Append(vbTab)
                End If
            Next
            sb.Append(vbCrLf)
            For Each row In objDataReader.Rows
                For intColumnValue = 0 To objDataReader.Columns.Count - 1
                    sb.Append(StrConv(IIf(IsDBNull(row.Item(intColumnValue)), "", row.Item(intColumnValue)), VbStrConv.ProperCase))
                    If intColumnValue <> objDataReader.Columns.Count - 1 Then
                        sb.Append(vbTab)
                    End If
                Next
                sb.Append(vbCrLf)
            Next
            'clsEnt.SaveExcel(SavePath & "\" & FileName & ".xls", sb)
            'Response.Clear()
            'Response.Buffer = True
            'Response.ContentType = "application/vnd.ms-excel"
            'Response.Charset = ""
            'Response.ContentEncoding = System.Text.Encoding.UTF8
            'Response.AddHeader("Content-Disposition", "attachment;filename=Workorder.xls")
            'Response.Write(sb.ToString())
            'Response.End()
        Catch ex As Exception
            Throw
        Finally
            objDataReader = Nothing
            sb = Nothing
        End Try
    End Sub

    Public Shared Sub SaveExcel(ByVal fpath As String, ByVal sb As System.Text.StringBuilder)
        Dim fsFile As New FileStream(fpath, FileMode.Create, FileAccess.Write)
        Dim strWriter As New StreamWriter(fsFile)
        Try
            With strWriter
                .BaseStream.Seek(0, SeekOrigin.End)
                .WriteLine(sb)
                .Close()
            End With
        Catch e As Exception
            Throw
        Finally
            sb = Nothing
            strWriter = Nothing
            fsFile = Nothing
        End Try
    End Sub

    Public Shared Function cmdDecrypt(ByVal aTxt As String, ByVal aPass As String) As String
        Dim pass As String
        Dim encstr As String
        Dim newstr As String
        Dim tmpstr As Integer
        Dim decrypted As String = ""
        Dim letter As String
        Dim i%

        pass = Len(aPass) 'this is the exact same For the Encrypt Function
        tmpstr = Len(aTxt) 'the only difference is that instead of adding the lenght of password.text
        'it is subtracted

        If tmpstr = 0 Then
            'MsgBox ("You must first Type In something To Decrypt")
            Return ""
        End If

        For i = 1 To tmpstr
            letter = Mid$(aTxt, i, 1)
            encstr = Asc(letter) - pass
            newstr = Chr(encstr)
            decrypted = decrypted & newstr
        Next i
        Return decrypted
    End Function

    Public Shared Function cmdEncrypt(ByVal aTxt As String, ByVal aPass As String) As String
        Dim pass As String
        Dim encstr As String
        Dim newstr As String
        Dim tmpstr As Integer
        Dim encrypted As String = ""
        Dim letter As String
        Dim i%

        pass = Len(aPass) 'the number you shift Each letter To encrypt
        tmpstr = Len(aTxt)

        If tmpstr = 0 Then
            'You can't encrypt nothing
            Return ""
        End If

        For i = 1 To tmpstr
            letter = Mid$(aTxt, i, 1) 'takes the ascii value and adds the length of the password To it
            encstr = Asc(letter) + pass
            newstr = Chr(encstr) 'changes ascii value To a character
            encrypted = encrypted & newstr 'puts all the encrypted characters together
        Next i
        Return encrypted 'puts the encrypted String in text box
    End Function

    Public Shared Function ConvertStr(ByVal aValue As String) As String
        aValue = Replace(aValue, "'", "`")
        aValue = Replace(aValue, "--", "")
        Return aValue
    End Function

    Public Shared Function ConvertDate(ByVal aValue As Date) As String
        Dim jDate As String = aValue.ToString("yyyy-MM-dd")
        Return jDate
    End Function

    Public Shared Function ConvertTime(ByVal aValue As Date) As String
        Dim jTime As String = aValue.ToString("hh:mm")
        Return jTime
    End Function

    Public Shared Function ToQ(ByVal aQ1 As Double, ByVal aQ2 As Double, ByVal aIsi As Double) As Double
        If aIsi = 0 Then
            aIsi = 1
        Else
            aIsi = aIsi
        End If
        ToQ = (aQ1 * aIsi) + aQ2
    End Function

    Public Shared Sub ExecSqlTransaction(ByVal connectionString As String, ByVal ParamArray cmdScript() As String)
        Dim strFile As String = "C:\ErrorLog_" & DateTime.Today.ToString("dd-MMM-yyyy") & ".txt"
        Dim sw As StreamWriter
        Using connection As New SqlConnection(connectionString)
            connection.Open()

            Dim command As SqlCommand = connection.CreateCommand()
            Dim transaction As SqlTransaction
            Dim iRec As Integer
            ' Start a local transaction
            transaction = connection.BeginTransaction("EnterpriseTransaction")

            ' Must assign both transaction object and connection
            ' to Command object for a pending local transaction.
            command.Connection = connection
            command.Transaction = transaction

            Try
                For iRec = 0 To UBound(cmdScript)
                    command.CommandText = cmdScript(iRec)
                    command.ExecuteNonQuery()
                Next iRec
                ' Attempt to commit the transaction.
                transaction.Commit()
            Catch ex As Exception

                ' Attempt to roll back the transaction.
                Try
                    transaction.Rollback()
                Catch ex2 As Exception
                    ' This catch block will handle any errors that may have occurred
                    ' on the server that would cause the rollback to fail, such as
                    ' a closed connection.

                    Try
                        If (Not File.Exists(strFile)) Then
                            sw = File.CreateText(strFile)
                            sw.WriteLine("Start Error Transaction Log for today")
                        Else
                            sw = File.AppendText(strFile)
                        End If
                        sw.WriteLine("RollBack Exception Type: {0}" & DateTime.Now)
                        sw.Close()
                    Catch ex3 As IOException
                        MsgBox("Error writing to log file.")
                    End Try

                End Try
            End Try
            connection.Close()
        End Using
    End Sub

    Public Shared Sub ExecSqlTransactionNonLog(ByVal connectionString As String, ByVal ParamArray cmdScript() As String)
        Using connection As New SqlConnection(connectionString)
            connection.Open()

            Dim command As SqlCommand = connection.CreateCommand()
            Dim transaction As SqlTransaction
            Dim iRec As Integer
            ' Start a local transaction
            transaction = connection.BeginTransaction("EnterpriseTransaction")

            ' Must assign both transaction object and connection
            ' to Command object for a pending local transaction.
            command.Connection = connection
            command.Transaction = transaction

            Try
                For iRec = 0 To UBound(cmdScript)
                    command.CommandText = cmdScript(iRec)
                    command.ExecuteNonQuery()
                Next iRec
                ' Attempt to commit the transaction.
                transaction.Commit()
            Catch ex As Exception

                ' Attempt to roll back the transaction.
                Try
                    transaction.Rollback()
                Catch ex2 As Exception
                    ' This catch block will handle any errors that may have occurred
                    ' on the server that would cause the rollback to fail, such as
                    ' a closed connection.
                End Try
            Finally
                connection.Close()
            End Try
        End Using
    End Sub

    Public Shared Sub RunMultiSqlTransaction(ByVal connectionString As String, ByVal trxName As String, ByVal ParamArray multiQuery() As String)
        Using connection As New SqlConnection(connectionString)
            connection.Open()

            Dim command As SqlCommand = connection.CreateCommand()
            Dim transaction As SqlTransaction

            ' Start a local transaction.
            transaction = connection.BeginTransaction(
              IsolationLevel.ReadCommitted, trxName)

            ' Must assign both transaction object and connection
            ' to Command object for a pending local transaction.
            command.Connection = connection
            command.Transaction = transaction

            Try
                For i As Integer = 0 To UBound(multiQuery, 1)    ' Use UBound to determine largest subscript of the array.
                    'Debug.WriteLine("Score " & i & ": " & scores(i))
                    command.CommandText = multiQuery(i)
                    command.ExecuteNonQuery()
                Next i
                transaction.Commit()
                System.Diagnostics.Debug.WriteLine("Records are written to database.")
            Catch e As Exception
                Try
                    transaction.Rollback()
                Catch ex As SqlException
                    If Not transaction.Connection Is Nothing Then
                        System.Diagnostics.Debug.WriteLine("An exception of type " & ex.GetType().ToString() &
                          " was encountered while attempting to roll back the transaction.")
                    End If
                End Try
                System.Diagnostics.Debug.WriteLine("An exception of type " & e.GetType().ToString() &
                  "was encountered while writing the data.")
                System.Diagnostics.Debug.WriteLine("Neither record was written to database.")
            Finally
                connection.Close()
            End Try

        End Using
    End Sub

    Public Shared Function FillDataTable(ByVal table As String, ByVal fields As String) As DataTable
        Dim query As String = "SELECT " & fields & " FROM " & table
        Dim sqlConn As New SqlConnection(strKoneksi("connuq"))
        sqlConn.Open()
        Dim cmd As New SqlCommand(query, sqlConn)

        Dim dt As New DataTable()
        dt.Load(cmd.ExecuteReader())
        sqlConn.Close()
        Return dt
    End Function

    Public Shared Function ReturnCcur(ByVal aRangeFilter As String, ByVal aFieldSum As String, ByVal aTbl As String) As Double
        'ReturnCcur = 0
        'Dim con As New SqlConnection
        'Dim cmd As New SqlCommand

        'Dim reader As SqlDataReader
        'Dim ad As SqlDataAdapter
        'Dim ds As New DataSet()
        'Dim Str As String = "select " & aFieldSum & " as hasil from " & aTbl & IIf(aRangeFilter = "", " ", " where " & aRangeFilter)
        'ad = New SqlClient.SqlDataAdapter(Str, strkoneksi("connuq"))
        'If ad.Fill(ds, "sumdata") = 0 Then
        '    ReturnCcur = 0
        'Else
        '    con = New SqlConnection(strkoneksi("connuq"))
        '    Try
        '        con.Open()
        '    Catch ex As Exception
        '        'MessageBox.Show("Database Connection Failed !!!")
        '    Finally
        '        cmd = New SqlCommand(Str, con)
        '        reader = cmd.ExecuteReader
        '        If reader.Read Then
        '            If IsDBNull(reader("hasil")) Then
        '                ReturnCcur = 0
        '            Else
        '                ReturnCcur = CDbl(reader("hasil"))
        '            End If
        '            reader.Close()
        '        Else
        '            reader.Close()
        '        End If
        '    End Try
        '    con.Close()
        'End If
        'ad.Dispose()
        'ds.Clear()
        'Return ReturnCcur
        Dim dData As DataTable = GetDataTabel("select " & aFieldSum & " as hasil from " & aTbl & IIf(aRangeFilter = "", " ", " where " & aRangeFilter & ""))
        If dData.Rows.Count >= 1 Then
            ReturnCcur = CDbl(dData.Rows(0).Item("hasil"))
        Else
            ReturnCcur = 0
        End If
        Return ReturnCcur
    End Function

    Public Shared Function ReturnAngka(ByVal aRangeFilter As String, ByVal aFieldSum As String, ByVal aTbl As String) As Double
        ReturnAngka = 0
        Dim con As New SqlConnection
        Dim cmd As New SqlCommand

        Dim reader As SqlDataReader
        Dim ad As SqlDataAdapter
        Dim ds As New DataSet()
        Dim Str As String = "select SUM(" & aFieldSum & ") as hasil from " & aTbl & IIf(aRangeFilter = "", " ", " where " & aRangeFilter)
        ad = New SqlClient.SqlDataAdapter(Str, strKoneksi("connuq"))
        If ad.Fill(ds, "sumdata") = 0 Then
            ReturnAngka = 0
        Else
            con = New SqlConnection(strKoneksi("connuq"))
            Try
                con.Open()
            Catch ex As Exception
                'MessageBox.Show("Database Connection Failed !!!")
            Finally
                cmd = New SqlCommand(Str, con)
                reader = cmd.ExecuteReader
                If reader.Read Then
                    If IsDBNull(reader("hasil")) Then
                        ReturnAngka = 0
                    Else
                        ReturnAngka = CDbl(reader("hasil"))
                    End If
                    reader.Close()
                Else
                    reader.Close()
                End If
                con.Close()
            End Try
            con.Close()
        End If
        ad.Dispose()
        ds.Clear()
        Return ReturnAngka
    End Function

    Public Shared Function ReturnFieldOneConn(ByVal aSelect As String, ByVal aWhere As String, ByVal aAnd As String, ByVal aRetField As String, ByVal aConString As String) As Double
        Dim con As New SqlConnection
        Dim cmd As SqlCommand
        Dim reader As SqlDataReader
        Dim Str As String = aSelect & IIf(aWhere = "", "", " where ") & aWhere & IIf(Trim(aAnd) = "", "", " and ") & aAnd
        Dim ds As New DataSet()
        Dim da As New SqlClient.SqlDataAdapter(Str, strConnect(aConString))
        Dim dataReturn As String = ""
        If da.Fill(ds, "wsdatax") = 0 Then
            dataReturn = 0
        Else
            con = New SqlConnection(strConnect(aConString))
            Try
                con.Open()
            Catch ex As Exception
                Console.WriteLine("Database Connection Failed !!!")
            Finally
                cmd = New SqlCommand(Str, con)
                reader = cmd.ExecuteReader
                If reader.Read Then
                    dataReturn = reader(aRetField)
                    reader.Close()
                Else
                    reader.Close()
                End If
            End Try
            con.Close()
        End If
        da.Dispose()
        ds.Clear()
        ds.Dispose()
        ReturnFieldOneConn = dataReturn
        Return ReturnFieldOneConn
    End Function

    Public Shared Function ReturnOneFieldConn(ByVal aSelect As String, ByVal aWhere As String, ByVal aAnd As String, ByVal aRetField As String, ByVal aConString As String) As String
        Dim con As New SqlConnection
        Dim cmd As SqlCommand
        Dim reader As SqlDataReader
        Dim Str As String = aSelect & IIf(aWhere = "", "", " where ") & aWhere & IIf(Trim(aAnd) = "", "", " and ") & aAnd
        Dim ds As New DataSet()
        Dim da As New SqlClient.SqlDataAdapter(Str, strConnect(aConString))
        Dim dataReturn As String = ""
        If da.Fill(ds, "sdata") = 0 Then
            dataReturn = ""
        Else
            con = New SqlConnection(strConnect(aConString))
            Try
                con.Open()
            Catch ex As Exception
                Console.WriteLine("Database Connection Failed !!!")
            Finally
                cmd = New SqlCommand(Str, con)
                reader = cmd.ExecuteReader
                If reader.Read Then
                    dataReturn = reader(aRetField)
                    reader.Close()
                Else
                    reader.Close()
                End If
            End Try
            con.Close()
        End If
        da.Dispose()
        ds.Clear()
        ds.Dispose()
        ReturnOneFieldConn = dataReturn
        Return ReturnOneFieldConn
    End Function
    Public Shared Sub RunSqlScript(ByVal myConnString As String, ByVal aQuery As String, ByVal aTransName As String)
        Dim myConnection As New SqlConnection(myConnString)
        myConnection.Open()
        Dim myCommand As SqlCommand = myConnection.CreateCommand()
        Dim myTrans As SqlTransaction
        ' Start a local transaction
        myTrans = myConnection.BeginTransaction(IsolationLevel.ReadCommitted, aTransName)
        ' Must assign both transaction object and connection
        ' to Command object for a pending local transaction
        myCommand.Connection = myConnection
        myCommand.Transaction = myTrans
        Try
            myCommand.CommandText = aQuery
            myCommand.ExecuteNonQuery()
            myTrans.Commit()
            Console.WriteLine("Record(s) are written to database.")
        Catch e As Exception
            Try
                myTrans.Rollback(aTransName)
            Catch ex As SqlException
                If Not myTrans.Connection Is Nothing Then
                    Console.WriteLine("An exception of type " & ex.GetType().ToString() &
                        " was encountered while attempting to roll back the transaction.")
                End If
            End Try
            Console.WriteLine("An exception of type " & e.GetType().ToString() &
                 "was encountered while inserting the data.")
            Console.WriteLine("Record(s) Not Written Successfull")
        Finally
            myConnection.Close()
        End Try

    End Sub 'RunSqlTransaction
    Public Shared Function ReturnFieldOne(ByVal aSelect As String, ByVal aWhere As String, ByVal aAnd As String, ByVal aRetField As String) As Double
        Dim con As New SqlConnection
        Dim cmd As SqlCommand
        Dim reader As SqlDataReader
        Dim Str As String = aSelect & IIf(aWhere = "", "", " where ") & aWhere & IIf(Trim(aAnd) = "", "", " and ") & aAnd
        Dim ds As New DataSet()
        Dim da As New SqlClient.SqlDataAdapter(Str, strKoneksi("connuq"))
        Dim dataReturn As String = ""
        If da.Fill(ds, "wsdatax") = 0 Then
            dataReturn = 0
        Else
            con = New SqlConnection(strKoneksi("connuq"))
            Try
                con.Open()
            Catch ex As Exception
                Console.WriteLine("Database Connection Failed !!!")
            Finally
                cmd = New SqlCommand(Str, con)
                reader = cmd.ExecuteReader
                If reader.Read Then
                    dataReturn = reader(aRetField)
                    reader.Close()
                Else
                    reader.Close()
                End If
            End Try
            con.Close()
        End If
        da.Dispose()
        ds.Clear()
        ds.Dispose()
        ReturnFieldOne = dataReturn
        Return ReturnFieldOne
    End Function

    Public Shared Function ReturnOneField(ByVal aSelect As String, ByVal aWhere As String, ByVal aAnd As String, ByVal aRetField As String) As String
        Dim con As New SqlConnection
        Dim cmd As SqlCommand
        Dim reader As SqlDataReader
        Dim Str As String = aSelect & IIf(aWhere = "", "", " where ") & aWhere & IIf(Trim(aAnd) = "", "", " and ") & aAnd
        Dim ds As New DataSet()
        Dim da As New SqlClient.SqlDataAdapter(Str, strKoneksi("connuq"))
        Dim dataReturn As String = ""
        If da.Fill(ds, "sdata") = 0 Then
            dataReturn = ""
        Else
            con = New SqlConnection(strKoneksi("connuq"))
            Try
                con.Open()
            Catch ex As Exception
                Console.WriteLine("Database Connection Failed !!!")
            Finally
                cmd = New SqlCommand(Str, con)
                reader = cmd.ExecuteReader
                If reader.Read Then
                    dataReturn = reader(aRetField)
                    reader.Close()
                Else
                    reader.Close()
                End If
            End Try
            con.Close()
        End If
        da.Dispose()
        ds.Clear()
        ds.Dispose()
        ReturnOneField = dataReturn
        Return ReturnOneField
    End Function

    Public Shared Function Terbilang(ByVal Angka As Double) As String
        Dim strAngka, strDiurai, Urai, Tbl1, Tbl2 As String
        Dim x, y, z As Short
        Dim arrBelasan() As String = {"SEPULUH ", "SEBELAS ",
            "DUA BELAS ", "TIGA BELAS ", "EMPAT BELAS ",
            "LIMA BELAS ", "ENAM BELAS ", "TUJUH BELAS ",
            "DELAPAN BELAS ", "SEMBILAN BELAS "}
        Dim arrSatuan() As String = {"DUA ", "TIGA ", "EMPAT ",
            "LIMA ", "ENAM ", "TUJUH ", "DELAPAN ", "SEMBILAN "}
        Urai = ""
        'Angka yang akan dibuat terbilang dibulatkan dulu Jika ada desimalnya

        Angka = Math.Round(Angka)

        'Angka tipe Double diubah menjadi string Dihilangkan spasi dikiri atau kanan angka jika ada
        strAngka = Trim(CStr(Angka))

        'Perulangan While ...End While akan mengevaluasi angka satu per satu dan dimulai dari angka paling kiri
        'x menunjukkan iterasi ke berapa dimulai dari 1

        While (x < Len(strAngka))
            x += 1
            strDiurai = Mid(strAngka, x, 1)

            'y menunjukkan angka yang sedang dievaluasi
            y += Val(strDiurai)

            'z menunjukkan posisi digit ke berapa
            z = Len(strAngka) - x + 1

            ' Jika yang dievaluasi angka 1
            If Val(strDiurai) = 1 Then
                If (z = 1 Or z = 7 Or z = 10 Or z = 13) Then
                    Tbl1 = "SATU "
                ElseIf (z = 4) Then
                    If (x = 1) Then
                        Tbl1 = "SE"
                    Else
                        Tbl1 = "SATU "
                    End If
                ElseIf (z = 2 Or z = 5 Or z = 8 Or z = 11 Or z = 14) Then
                    'Ditambahkan iterasi angka berikutnya
                    x += 1
                    strDiurai = Mid(strAngka, x, 1)
                    z = Len(strAngka) - x + 1
                    Tbl2 = ""
                    Tbl1 = arrBelasan(Val(strDiurai))
                Else
                    Tbl1 = "SE"
                End If
                'Yang dievaluasi angka 2 sampai 9
            ElseIf Val(strDiurai) > 1 And Val(strDiurai) < 10 Then
                Tbl1 = arrSatuan((Val(strDiurai)) - 2)
            Else
                Tbl1 = ""
            End If

            If (Val(strDiurai) > 0) Then
                If (z = 2 Or z = 5 Or z = 8 Or z = 11 Or
                        z = 14) Then
                    Tbl2 = "PULUH "
                ElseIf (z = 3 Or z = 6 Or z = 9 Or z = 12 _
                        Or z = 15) Then
                    Tbl2 = "RATUS "
                Else
                    Tbl2 = ""
                End If
            Else
                Tbl2 = ""
            End If

            If (y > 0) Then
                Select Case z
                    Case 4
                        Tbl2 &= "RIBU "
                        y = 0
                    Case 7
                        Tbl2 &= "JUTA "
                        y = 0
                    Case 10
                        Tbl2 &= "MILYAR "
                        y = 0
                    Case 13
                        Tbl2 &= "TRILYUN "
                        y = 0
                End Select
            End If

            Urai = Urai & Tbl1 & Tbl2
        End While

        Terbilang = Urai & "RUPIAH"
        'Terbilang(CDbl(txtInput.Text))
    End Function

    Public Shared Function strKoneksi(ByVal aConnName As String) As String
        strKoneksi = WebConfigurationManager.ConnectionStrings(aConnName).ConnectionString
    End Function

    Public Shared Function strConnect(ByVal aConnName As String) As String
        strConnect = WebConfigurationManager.ConnectionStrings(aConnName).ConnectionString
    End Function

    Public Shared Function getNumeric(ByVal value As String) As String
        Dim output As StringBuilder = New StringBuilder
        Dim i As Integer
        For i = 0 To value.Length - 1
            If IsNumeric(value(i)) Then
                output.Append(value(i))
            End If
        Next
        Return output.ToString()
    End Function

    Public Shared Function GetIPAddress() As String
        Dim context As System.Web.HttpContext = System.Web.HttpContext.Current
        Dim sIPAddress As String = context.Request.ServerVariables("HTTP_X_FORWARDED_FOR")
        If String.IsNullOrEmpty(sIPAddress) Then
            Return context.Request.ServerVariables("REMOTE_ADDR")
        Else
            Dim ipArray As String() = sIPAddress.Split(New [Char]() {","c})
            Return ipArray(0)
        End If
    End Function

    Public Shared Function logMhsLogin(ByVal aNpm As String, ByVal aDt As String, ByVal aAction As String, ByVal aModul As String, ByVal vIpAdd As String) As String
        logMhsLogin = "insert into logPortal (USERID,TIPE,TGL,WAKTU,STATUSACTION,MODUL,IPADD) values ('" & aNpm & "','Mahasiswa','" & aDt & "','" & Microsoft.VisualBasic.Right(aDt, 5) & "', '" & aAction & "','" & aModul & "','" & vIpAdd & "')"
    End Function

    Public Shared Function RetFormatTgl(ByVal aDate As String, ByVal aMth As Integer, ByVal optdigit As Integer) As String
        RetFormatTgl = ""
        Dim sBln As String = ""
        If aMth = 1 Then
            sBln = "Januari"
        ElseIf aMth = 2 Then
            sBln = "Februari"
        ElseIf aMth = 3 Then
            sBln = "Maret"
        ElseIf aMth = 4 Then
            sBln = "April"
        ElseIf aMth = 5 Then
            sBln = "Mei"
        ElseIf aMth = 6 Then
            sBln = "Juni"
        ElseIf aMth = 7 Then
            sBln = "Juli"
        ElseIf aMth = 8 Then
            sBln = "Agustus"
        ElseIf aMth = 9 Then
            sBln = "September"
        ElseIf aMth = 10 Then
            sBln = "Oktober"
        ElseIf aMth = 11 Then
            sBln = "November"
        ElseIf aMth = 12 Then
            sBln = "Desember"
        Else
            sBln = ""
        End If
        If optdigit = 3 Then
            RetFormatTgl = Microsoft.VisualBasic.Right(aDate, 2) & " " & Microsoft.VisualBasic.Left(sBln, 3) & " " & Microsoft.VisualBasic.Right(aDate, 4)
        Else
            RetFormatTgl = Microsoft.VisualBasic.Right(aDate, 2) & " " & sBln & " " & Microsoft.VisualBasic.Left(aDate, 4)
        End If
        Return RetFormatTgl
    End Function

    Public Shared Function RetFormatTgl2(ByVal aDate As String, ByVal aMth As Integer, ByVal optdigit As Integer) As String
        RetFormatTgl2 = ""
        Dim sBln As String = ""
        If aMth = 1 Then
            sBln = "Januari"
        ElseIf aMth = 2 Then
            sBln = "Februari"
        ElseIf aMth = 3 Then
            sBln = "Maret"
        ElseIf aMth = 4 Then
            sBln = "April"
        ElseIf aMth = 5 Then
            sBln = "Mei"
        ElseIf aMth = 6 Then
            sBln = "Juni"
        ElseIf aMth = 7 Then
            sBln = "Juli"
        ElseIf aMth = 8 Then
            sBln = "Agustus"
        ElseIf aMth = 9 Then
            sBln = "September"
        ElseIf aMth = 10 Then
            sBln = "Oktober"
        ElseIf aMth = 11 Then
            sBln = "November"
        ElseIf aMth = 12 Then
            sBln = "Desember"
        Else
            sBln = ""
        End If
        If optdigit = 3 Then
            RetFormatTgl2 = Microsoft.VisualBasic.Right(aDate, 2) & " " & Microsoft.VisualBasic.Left(sBln, 3) & " " & Microsoft.VisualBasic.Left(aDate, 4)
        Else
            RetFormatTgl2 = Microsoft.VisualBasic.Right(aDate, 2) & " " & sBln & " " & Microsoft.VisualBasic.Left(aDate, 4)
        End If
        Return RetFormatTgl2
    End Function

    Public Shared Function ReturnHari(ByVal aHari As String) As String
        ReturnHari = ""
        If aHari = "Monday" Then
            ReturnHari = "Senin"
        ElseIf aHari = "Tuesday" Then
            ReturnHari = "Selasa"
        ElseIf aHari = "Wednesday" Then
            ReturnHari = "Rabu"
        ElseIf aHari = "Thursday" Then
            ReturnHari = "Kamis"
        ElseIf aHari = "Friday" Then
            ReturnHari = "Jumat"
        ElseIf aHari = "Saturday" Then
            ReturnHari = "Sabtu"
        ElseIf aHari = "Sunday" Then
            ReturnHari = "Minggu"
        End If
        Return ReturnHari
    End Function

    Public Shared Function ConvertDataTabletoJSON(ByVal dt As DataTable) As String
        Dim Jserializer As New System.Web.Script.Serialization.JavaScriptSerializer()
        Dim rowsList As New List(Of Dictionary(Of String, Object))()
        Dim row As Dictionary(Of String, Object)
        For Each dr As DataRow In dt.Rows
            row = New Dictionary(Of String, Object)()
            For Each col As DataColumn In dt.Columns
                row.Add(col.ColumnName, dr(col))
            Next
            rowsList.Add(row)
        Next
        Return Jserializer.Serialize(rowsList)
    End Function

    Public Shared Function ConvertJSONToDataTable(ByVal jsonString As String) As DataTable
        Dim dt As New DataTable
        'strip out bad characters
        Dim jsonParts As String() = jsonString.Replace("[", "").Replace("]", "").Split("},{")
        'hold column names
        Dim dtColumns As New List(Of String)
        'get columns
        For Each jp As String In jsonParts
            'only loop thru once to get column names
            Dim propData As String() = jp.Replace("{", "").Replace("}", "").Split(New Char() {","}, StringSplitOptions.RemoveEmptyEntries)
            For Each rowData As String In propData
                Try
                    Dim idx As Integer = rowData.IndexOf(":")
                    Dim n As String = rowData.Substring(0, idx - 1)
                    Dim v As String = rowData.Substring(idx + 1)
                    If Not dtColumns.Contains(n) Then
                        dtColumns.Add(n.Replace("""", ""))
                    End If
                Catch ex As Exception
                    Throw New Exception(String.Format("Error Parsing Column Name : {0}", rowData))
                End Try
            Next
            Exit For
        Next
        'build dt
        For Each c As String In dtColumns
            dt.Columns.Add(c)
        Next
        'get table data
        For Each jp As String In jsonParts
            Dim propData As String() = jp.Replace("{", "").Replace("}", "").Split(New Char() {","}, StringSplitOptions.RemoveEmptyEntries)
            Dim nr As DataRow = dt.NewRow
            For Each rowData As String In propData
                Try
                    Dim idx As Integer = rowData.IndexOf(":")
                    Dim n As String = rowData.Substring(0, idx - 1).Replace("""", "")
                    Dim v As String = rowData.Substring(idx + 1).Replace("""", "")
                    nr(n) = v
                Catch ex As Exception
                    Continue For
                End Try

            Next
            dt.Rows.Add(nr)
        Next
        Return dt
    End Function

    Public Shared Function CDateME5(ByVal aDate As String) As String
        CDateME5 = Microsoft.VisualBasic.Right(Trim(aDate), 4) & "-" & Microsoft.VisualBasic.Mid(Trim(aDate), 4, 2) & "-" & Left(Trim(aDate), 2)
    End Function

    Public Shared Function CDateME3(ByVal aDate As String) As String
        CDateME3 = Microsoft.VisualBasic.Right(aDate, 4) & Microsoft.VisualBasic.Mid(aDate, 4, 2) & Left(aDate, 2)
    End Function

    Public Shared Function CDateME2(ByVal aDate As String) As String
        CDateME2 = Microsoft.VisualBasic.Mid(aDate, 7, 4) & "-" & Microsoft.VisualBasic.Mid(aDate, 4, 2) & "-" & Left(aDate, 2)
    End Function

    Public Shared Function CToDate(ByVal aDate As String) As Date
        CToDate = DateSerial(Microsoft.VisualBasic.Right(aDate, 4), Microsoft.VisualBasic.Mid(aDate, 4, 2), Left(aDate, 2))
    End Function

    Public Shared Function StringCrystal() As String
        StringCrystal = "Data Source=DENNYJUNAIDI\MSSQLSERVER2019;Initial Catalog=Leasing;User ID=sa;Password=sasa;Max Pool Size=400;Connect Timeout=600;"
        Return StringCrystal
    End Function

    Public Shared Function StringConn() As String
        StringConn = "Data Source=DENNYJUNAIDI\MSSQLSERVER2019;Initial Catalog=Leasing;User ID=sa;Password=sasa;Max Pool Size=400;Connect Timeout=600;"
        Return StringConn
    End Function

    Public Shared Function StringOle() As String
        Dim psdb As String = iEncrypt("sasa")
        StringOle = "Provider=SQLOLEDB.1;Data Source=DENNYJUNAIDI\MSSQLSERVER2019;Initial Catalog=Leasing;User ID=sa;Password=sasa;User ID=sa;Password=" & iDecrypt(psdb)
    End Function

    Public Shared Function StringOleExt() As String
        Dim psdb As String = iEncrypt("sasa")
        StringOleExt = "Provider=SQLOLEDB.1;Data Source=DENNYJUNAIDI\MSSQLSERVER2019;Initial Catalog=Leasing;User ID=sa;Password=sasa;User ID=sa;Password=" & iDecrypt(psdb)
    End Function

    Public Shared Function StringSql() As String
        StringSql = "Data Source=DENNYJUNAIDI\MSSQLSERVER2019;Initial Catalog=Leasing;User ID=sa;Password=sasa;Persist Security Info=false"
    End Function

    Public Shared Function objGetData() As String
        Return "Data Source=DENNYJUNAIDI\MSSQLSERVER2019;Initial Catalog=Leasing;User ID=sa;Password=sasa;Persist Security Info=false"
    End Function

    Public Shared Function iDecrypt(cipherText As String) As String
        'Dim EncryptionKey As String = "MAKV2SPBNI99212"
        Dim klienid = ConfigurationManager.AppSettings("ClientId").ToString()
        Dim EncryptionKey As String = "00C335F" & klienid
        Dim cipherBytes As Byte() = Convert.FromBase64String(cipherText)
        Using encryptor As Aes = Aes.Create()
            Dim pdb As New Rfc2898DeriveBytes(EncryptionKey, New Byte() {&H49, &H76, &H61, &H6E, &H20, &H4D,
             &H65, &H64, &H76, &H65, &H64, &H65,
             &H76})
            encryptor.Key = pdb.GetBytes(32)
            encryptor.IV = pdb.GetBytes(16)
            Using ms As New MemoryStream()
                Using cs As New CryptoStream(ms, encryptor.CreateDecryptor(), CryptoStreamMode.Write)
                    cs.Write(cipherBytes, 0, cipherBytes.Length)
                    cs.Close()
                End Using
                cipherText = Encoding.Unicode.GetString(ms.ToArray())
            End Using
        End Using
        Return cipherText
    End Function

    Public Shared Function iEncrypt(clearText As String) As String

        Dim klienid = ConfigurationManager.AppSettings("ClientId").ToString()
        Dim EncryptionKey As String = "00C335F" & klienid
        Dim clearBytes As Byte() = Encoding.Unicode.GetBytes(clearText)
        Using encryptor As Aes = Aes.Create()
            Dim pdb As New Rfc2898DeriveBytes(EncryptionKey, New Byte() {&H49, &H76, &H61, &H6E, &H20, &H4D,
             &H65, &H64, &H76, &H65, &H64, &H65,
             &H76})
            encryptor.Key = pdb.GetBytes(32)
            encryptor.IV = pdb.GetBytes(16)
            Using ms As New MemoryStream()
                Using cs As New CryptoStream(ms, encryptor.CreateEncryptor(), CryptoStreamMode.Write)
                    cs.Write(clearBytes, 0, clearBytes.Length)
                    cs.Close()
                End Using
                clearText = Convert.ToBase64String(ms.ToArray())
            End Using
        End Using
        Return clearText
    End Function

    Public Shared Function ChkDateSys(aStrDate As String) As String
        Dim Str As String = "select TGL from INTERFERENSITGL where TGL >= '" & aStrDate & "'"
        'Dim ds As New DataSet
        'Dim ad As New SqlClient.SqlDataAdapter(Str, objGetData().ToString)
        'Dim readerx As SqlDataReader
        'If ad.Fill(ds, "xdata") = 0 Then
        '    ChkDateSys = "Salah Tanggal ! Interferensi Tanggal Input Mundur ! "
        'Else
        '    Using m_connection As New SqlConnection(objGetData.ToString())
        '        Try
        '            Using MyCommand As New SqlCommand(Str, m_connection)
        '                Try
        '                    readerx = MyCommand.ExecuteReader
        '                    If readerx.Read Then
        '                        ChkDateSys = "Found"
        '                        readerx.Close()
        '                    Else
        '                        ChkDateSys = "NotFound"
        '                        readerx.Close()
        '                    End If
        '                Catch ex As Exception
        '                    ChkDateSys = "Query Error"
        '                End Try
        '            End Using
        '        Catch ex As Exception
        '            ChkDateSys = "Database Connection Failed"
        '        End Try
        '    End Using
        'End If
        'ad.Dispose()
        'ds.Clear()
        'Return ChkDateSys
        ChkDateSys = "NotFound"
        Dim dkd As DataTable = GetDataTabel(Str)
        If dkd.Rows.Count >= 1 Then
            ChkDateSys = "Found"
        ElseIf dkd.Rows.Count = 0 Then
            ChkDateSys = "NotFound"
        End If
        dkd = Nothing
        Return ChkDateSys
    End Function

    Public Shared Function GetDataTabel(query As String) As DataTable
        Dim conString As String = clsEnt.strKoneksi("connuq")
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

    Public Shared Function GetDataTabelWPI(query As String) As DataTable
        Dim conString As String = ConfigurationManager.ConnectionStrings("connWPI").ConnectionString
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

    Public Shared Function FillDropDownList(astr As String) As DataSet
        Dim con As New SqlConnection(clsEnt.strKoneksi("connuq"))

        Dim cmd As New SqlCommand(astr, con)

        Dim da As New SqlDataAdapter(cmd)

        Dim ds As New DataSet()
        da.Fill(ds)
        Return ds
    End Function

    Public Shared Sub SortDropDown(ByVal cbo As DropDownList)
        Dim lstListItems As New List(Of ListItem)
        For Each li As ListItem In cbo.Items
            lstListItems.Add(li)
        Next
        lstListItems.Sort(New ListItemComparer)
        cbo.Items.Clear()
        cbo.Items.AddRange(lstListItems.ToArray)
    End Sub

    Public Shared Function getAccPPNM() As String
        getAccPPNM = ReturnOneField("select ACCPPNM from GLFSYS ", "", "", "ACCPPNM")
    End Function

    Public Shared Function getAccPPNK() As String
        getAccPPNK = ReturnOneField("select ACCPPNK from GLFSYS ", "", "", "ACCPPNK")
    End Function

    Public Shared Function getAccJual(sCabang As String) As String
        getAccJual = ReturnOneField("select ACCJUAL from PLINE where PL = '" & sCabang & "' ", "", "", "ACCJUAL")
    End Function

    Public Shared Function getAccHutang(sCabang As String) As String
        getAccHutang = ReturnOneField("select ACCHUT from PLINE where PL = '" & sCabang & "' ", "", "", "ACCHUT")
    End Function

    Public Shared Function getAccHut() As String
        getAccHut = ReturnOneField("select ACCHUT from GLFSYS ", "", "", "ACCHUT")
    End Function

    'clsEnt.getAccPersediaan(txtCabang.Value)
    'clsEnt.getAccHPP
    Public Shared Function getAccPersediaan(sCabang As String) As String
        getAccPersediaan = ReturnOneField("select ACCPERSEDIAAN from PLINE where PL = '" & sCabang & "' ", "", "", "ACCPERSEDIAAN")
    End Function

    Public Shared Function getAccHPP(sCabang As String) As String
        getAccHPP = ReturnOneField("select ACCHPP from PLINE where PL = '" & sCabang & "' ", "", "", "ACCHPP")
    End Function

    Public Shared Function GetAccPiut() As String
        GetAccPiut = clsEnt.ReturnOneField("select ACCPIUT  from  GLFSYS ", "", "", "ACCPIUT")
    End Function

    Public Shared Function getAccPiutang(sCabang As String) As String
        getAccPiutang = ReturnOneField("select ACCPIUT from PLINE where PL = '" & sCabang & "' ", "", "", "ACCPIUT")
    End Function

    Public Shared Function getAccPotAP(sCabang As String) As String
        getAccPotAP = ReturnOneField("select ACCPOTBAYARAP from PLINE where PL = '" & sCabang & "' ", "", "", "ACCPOTBAYARAP")
    End Function

    Public Shared Function getAccPotAR(sCabang As String) As String
        getAccPotAR = ReturnOneField("select ACCDISCPIUT from PLINE where PL = '" & sCabang & "' ", "", "", "ACCDISCPIUT")
    End Function

    Public Shared Function getAccPotongAR(aPL As String) As String
        getAccPotongAR = ReturnOneField("select ACCDISCPIUT from PLINE WHERE PL = '" & aPL & "'  ", "", "", "ACCDISCPIUT")
    End Function

    Public Shared Function getAccPotongAP() As String
        getAccPotongAP = ReturnOneField("SELECT ACCDISCTRANSFER FROM GLFSYS", "", "", "ACCDISCTRANSFER")
    End Function

    Public Shared Function getAccPCG() As String
        getAccPCG = ReturnOneField("select ACCPCG from GLFSYS", "", "", "ACCPCG")
    End Function

    Public Shared Function getAccHCG() As String
        getAccHCG = ReturnOneField("select ACCHCG from GLFSYS ", "", "", "ACCHCG")
    End Function

    Public Shared Function getAccPCGSrc(aGiro As String) As String
        getAccPCGSrc = ReturnOneField("select AccountD from TransaksiGiroTerima where NoGiro = '" & aGiro & "' ", "", "", "AccountD")
    End Function

    Public Shared Function getAccHCGSrc(aGiro As String) As String
        getAccHCGSrc = ReturnOneField("select AccountK from TransaksiGiroBayar where NoGiro = '" & aGiro & "' ", "", "", "AccountK")
    End Function

    Public Shared Function CheckPriv(aUser As String, aBossCode As String, xMenuId As Integer) As Boolean
        CheckPriv = False
        Dim sBospunya As Boolean = False
        Dim mId As String = ""
        Dim sRoles As String = ""
        If LCase(Trim$(aUser)) = "admin" Then
            CheckPriv = True
        Else
            mId = ReturnOneField("select MENU_ID from SMUSR_PRIVS where SM_OPEN = 'R' and UserId = '" & aUser & "' and MENU_ID = " & xMenuId & " ", "", "", "MENU_ID")
            If mId <> "" Then
                If xMenuId = CInt(mId) Then
                    sBospunya = IIf(aBossCode = ReturnOneField("select code__bos from sysjual ", "", "", "code__bos"), True, False)
                    If sBospunya = True Then
                        CheckPriv = True
                    Else
                        CheckPriv = False
                    End If

                End If
            ElseIf mId = "" Then
                CheckPriv = False
            End If
        End If
    End Function

    Public Shared Function CheckPrivModify(aUser As String, xMenuId As Integer) As Boolean
        CheckPrivModify = False
        Dim modId As String = ""
        Dim modRoles As String = ""
        If LCase(Trim$(aUser)) = "admin" Then
            CheckPrivModify = True
        Else
            modId = ReturnOneField("select MENU_ID from SMUSR_PRIVS where SM_MODIFY = 'R' and USERID = '" & aUser & "' and MENU_ID = " & xMenuId & " ", "", "", "MENU_ID")
            If modId <> "" Then
                If xMenuId = CInt(modId) Then
                    CheckPrivModify = True
                End If
            ElseIf modId = "" Then
                CheckPrivModify = False
            End If
        End If
    End Function

    Public Shared Function CheckPrivDel(aUser As String, xMenuId As Integer) As Boolean
        CheckPrivDel = False
        Dim modDelId As String = ""
        Dim delRoles As String = ""
        If LCase(Trim$(aUser)) = "admin" Then
            CheckPrivDel = True
        Else
            modDelId = ReturnOneField("select MENU_ID from SMUSR_PRIVS where SM_DELETE = 'R' and UserId = '" & aUser & "' and MENU_ID = " & xMenuId & " ", "", "", "MENU_ID")
            If modDelId <> "" Then
                If xMenuId = CInt(modDelId) Then
                    CheckPrivDel = True
                End If
            ElseIf modDelId = "" Then
                CheckPrivDel = False
            End If
        End If
    End Function

    Public Shared Function CheckPrivInput(aUser As String, xMenuId As Integer) As Boolean
        CheckPrivInput = False
        Dim modInsId As String = ""
        Dim InsRoles As String = ""
        If LCase(Trim$(aUser)) = "admin" Then
            CheckPrivInput = True
        Else
            modInsId = ReturnOneField("select MENU_ID from SMUSR_PRIVS where SM_INPUT = 'R' and Userid = '" & aUser & "' and MENU_ID = " & xMenuId & " ", "", "", "MENU_ID")
            If modInsId <> "" Then
                If xMenuId = CInt(modInsId) Then
                    CheckPrivInput = True
                End If
            ElseIf modInsId = "" Then
                CheckPrivInput = False
            End If
        End If

    End Function

    Public Shared Function CheckPrivConn(aUser As String, aBossCode As String, xMenuId As Integer, bConn As String) As Boolean
        CheckPrivConn = False
        Dim sBospunya As Boolean = False
        Dim mId As String = ""
        Dim sRoles As String = ""
        sRoles = ReturnOneFieldConn("select roleid from SMUSR_ROLE where userid = '" & aUser & "'", "", "", "roleid", bConn)
        If sRoles <> "" Then
            mId = ReturnOneFieldConn("select MENU_ID from SMUSR_PRIVS where SM_OPEN = 'R' and RoleId = '" & sRoles & "' and MENU_ID = " & xMenuId & " ", "", "", "MENU_ID", bConn)
            If mId <> "" Then
                If xMenuId = CInt(mId) Then
                    sBospunya = IIf(aBossCode = ReturnOneFieldConn("select code__bos from sysjual ", "", "", "code__bos", bConn), True, False)
                    If sBospunya = True Then
                        CheckPrivConn = True
                    Else
                        CheckPrivConn = False
                    End If

                End If
            ElseIf mId = "" Then
                CheckPrivConn = False
            End If
        End If
    End Function

    Public Shared Function CheckPrivModifyConn(aUser As String, xMenuId As Integer, bConn As String) As Boolean
        CheckPrivModifyConn = False
        Dim modId As String = ""
        Dim modRoles As String = ""
        modRoles = ReturnOneFieldConn("select roleid from SMUSR_ROLE where userid = '" & aUser & "'", "", "", "roleid", bConn)
        If modRoles <> "" Then
            modId = ReturnOneFieldConn("select MENU_ID from SMUSR_PRIVS where SM_MODIFY = 'R' and RoleId = '" & modRoles & "' and MENU_ID = " & xMenuId & " ", "", "", "MENU_ID", bConn)
            If modId <> "" Then
                If xMenuId = CInt(modId) Then
                    CheckPrivModifyConn = True
                End If
            ElseIf modId = "" Then
                CheckPrivModifyConn = False
            End If
        End If
    End Function

    Public Shared Function CheckPrivDelConn(aUser As String, xMenuId As Integer, bConn As String) As Boolean
        CheckPrivDelConn = False
        Dim modDelId As String = ""
        Dim delRoles As String = ""
        delRoles = ReturnOneFieldConn("select roleid from SMUSR_ROLE where userid = '" & aUser & "'", "", "", "roleid", bConn)
        If delRoles <> "" Then
            modDelId = ReturnOneFieldConn("select MENU_ID from SMUSR_PRIVS where SM_DELETE = 'R' and RoleId = '" & delRoles & "' and MENU_ID = " & xMenuId & " ", "", "", "MENU_ID", bConn)
            If modDelId <> "" Then
                If xMenuId = CInt(modDelId) Then
                    CheckPrivDelConn = True
                End If
            ElseIf modDelId = "" Then
                CheckPrivDelConn = False
            End If
        End If
    End Function

    Public Shared Function CheckPrivInputConn(aUser As String, xMenuId As Integer, bConn As String) As Boolean
        CheckPrivInputConn = False
        Dim modInsId As String = ""
        Dim InsRoles As String = ""
        InsRoles = ReturnOneFieldConn("select roleid from SMUSR_ROLE where userid = '" & aUser & "'", "", "", "roleid", bConn)
        If InsRoles <> "" Then
            modInsId = ReturnOneFieldConn("select MENU_ID from SMUSR_PRIVS where SM_INPUT = 'R' and RoleId = '" & InsRoles & "' and MENU_ID = " & xMenuId & " ", "", "", "MENU_ID", bConn)
            If modInsId <> "" Then
                If xMenuId = CInt(modInsId) Then
                    CheckPrivInputConn = True
                End If
            ElseIf modInsId = "" Then
                CheckPrivInputConn = False
            End If
        End If
    End Function

    Public Shared Function GetSysDate() As Date
        GetSysDate = DateSerial(ReturnOneField("select TH from GLFSYS", "", "", "TH"), ReturnOneField("select BL from GLFSYS", "", "", "BL"), 1)
    End Function

    Public Shared Function RetArrayDiskon(ByVal aDisc As String, ByVal aSimbol As String) As String()
        Dim arrayDisc() As String = Split(aDisc, "+")
        'kasus di bwh yg spasi dianggap bukan array:
        'arrayDisc holds {"apple", "", "", "", "pear", "banana", "", ""}
        Dim LastNonEmpty As Integer = -1
        For i As Integer = 0 To arrayDisc.Length - 1
            If arrayDisc(i) <> "" Then
                If getNumeric(arrayDisc(i)) <> "" Then
                    LastNonEmpty += 1
                    arrayDisc(LastNonEmpty) = arrayDisc(i)
                End If
            End If
        Next
        ReDim Preserve arrayDisc(LastNonEmpty)
        RetArrayDiskon = arrayDisc
        Return RetArrayDiskon
    End Function

    Public Shared Function PersenTingkat(ByVal aAngka As Double, ByVal aTingkat() As String) As Double
        Dim aDiscPersen As Double
        Dim jAngka As Double = aAngka
        For i = 0 To aTingkat.Length - 1
            If IsNumeric(aTingkat(i)) Then
                jAngka = jAngka - (jAngka * aTingkat(i) / 100)
            End If
        Next
        Dim xAngka As Double = aAngka - jAngka

        aDiscPersen = (xAngka / aAngka) * 100
        PersenTingkat = aDiscPersen
        Return PersenTingkat
    End Function

    Public Shared Function RetArrayQty(ByVal aQty As String, ByVal aSimbol As String) As String()
        Dim arrayQty() As String = Split(aQty, aSimbol)
        'kasus di bwh yg spasi dianggap bukan array:
        'arrayDisc holds {"apple", "", "", "", "pear", "banana", "", ""}
        Dim LastNotEmpty As Integer = -1
        For i As Integer = 0 To arrayQty.Length - 1
            If arrayQty(i) <> "" Then
                If getNumeric(arrayQty(i)) <> "" Then
                    LastNotEmpty += 1
                    arrayQty(LastNotEmpty) = arrayQty(i)
                End If
            End If
        Next
        ReDim Preserve arrayQty(LastNotEmpty)
        RetArrayQty = arrayQty
        Return RetArrayQty
    End Function

    Public Shared Function CDateME(ByVal aDate As String) As String
        CDateME = Microsoft.VisualBasic.Right(Trim(aDate), 4) & "-" & Microsoft.VisualBasic.Mid(Trim(aDate), 4, 2) & "-" & Left(Trim(aDate), 2)
    End Function

    'Public Shared Function CDateME3(ByVal aDate As String) As String
    '    CDateME3 = Microsoft.VisualBasic.Right(aDate, 4) & Microsoft.VisualBasic.Mid(aDate, 4, 2) & Left(aDate, 2)
    'End Function

    'Public Shared Function CDateME2(ByVal aDate As String) As String
    '    CDateME2 = Microsoft.VisualBasic.Mid(aDate, 7, 4) & "-" & Microsoft.VisualBasic.Mid(aDate, 4, 2) & "-" & Left(aDate, 2)
    'End Function

    'Public Shared Function CToDate(ByVal aDate As String) As Date
    '    CToDate = DateSerial(Microsoft.VisualBasic.Right(aDate, 4), Microsoft.VisualBasic.Mid(aDate, 4, 2), Left(aDate, 2))
    'End Function

    'Shared Sub RunSqlScript(sConnstring As String, p2 As String)
    '    Throw New NotImplementedException
    'End Sub



    Public Shared Function kirimemailattach(provider As String, portnya As Integer, messagebodynya As String, subjek As String, apasscred As String, fromemail As String, toemail As String, myfileattachment As String, Optional ccadd As String = "") As String
        Try
            Dim message As New MailMessage(fromemail, toemail)
            message.Subject = subjek
            message.Body = messagebodynya
            message.BodyEncoding = Encoding.UTF8
            message.IsBodyHtml = True
            If Trim$(ccadd) <> "" Then
                message.CC.Add(ccadd)
            End If
            'Dim client As New SmtpClient("mail." & dmain, 26) 'ok  'Dim client As New SmtpClient("mitsui.xinergix.com", 465) 'Gmail smtp    '[XU;_]G9i@#]
            'smtp.gmail.com
            'mail.universitasquality.ac.id 
            '"ybhippsajvvadnpk"
            Dim client As New SmtpClient(provider, portnya) 'ok 
            Dim basicCredential1 As New System.Net.NetworkCredential(fromemail, apasscred)
            If provider = "smtp.gmail.com" Then
                client.EnableSsl = True
            Else
                client.EnableSsl = True
                client.UseDefaultCredentials = True
            End If

            client.Credentials = basicCredential1
            'attachment
            Dim data As New Attachment(myfileattachment, MediaTypeNames.Application.Octet)
            Dim disposition As ContentDisposition = data.ContentDisposition
            disposition.CreationDate = System.IO.File.GetCreationTime(myfileattachment)
            disposition.ModificationDate = System.IO.File.GetLastWriteTime(myfileattachment)
            disposition.ReadDate = System.IO.File.GetLastAccessTime(myfileattachment)
            message.Attachments.Add(data)

            client.Send(message)
            kirimemailattach = "Sukses"
        Catch ex As Exception
            kirimemailattach = "Gagal"
        End Try
        Return kirimemailattach
    End Function

    Public Shared Function kirimemailpop3(provider As String, portnya As Integer, messagebodynya As String, subjek As String, apasscred As String, fromemail As String, toemail As String, Optional ccadd As String = "") As String
        Try

            Dim kepada As String = clsEnt.ConvertStr(toemail)
            Dim message As New MailMessage(fromemail, kepada) 'From address  

            Dim mailbody As String = messagebodynya
            message.Subject = subjek
            message.Body = mailbody
            message.BodyEncoding = Encoding.UTF8
            message.IsBodyHtml = True
            Dim client As New SmtpClient("mail." & provider, portnya) 'ok  'Dim client As New SmtpClient("mitsui.xinergix.com", 465) 'Gmail smtp    '[XU;_]G9i@#]
            Dim basicCredential1 As New System.Net.NetworkCredential("webadm@" & provider, apasscred)
            'client.EnableSsl = True

            If Trim$(ccadd) <> "" Then
                message.CC.Add(ccadd)
            End If
            client.EnableSsl = False
            client.UseDefaultCredentials = False
            client.Credentials = basicCredential1
            client.Send(message)

            kirimemailpop3 = "Sukses"
        Catch ex As Exception
            kirimemailpop3 = "Gagal"
        End Try
        Return kirimemailpop3
    End Function
    Public Shared Function kirimemailpop3attach(provider As String, portnya As Integer, messagebodynya As String, subjek As String, apasscred As String, fromemail As String, toemail As String, myfileattachment As String, Optional ccadd As String = "") As String
        Try

            Dim kepada As String = clsEnt.ConvertStr(toemail)
            Dim message As New MailMessage(fromemail, kepada) 'From address  

            Dim mailbody As String = messagebodynya
            message.Subject = subjek
            message.Body = mailbody
            message.BodyEncoding = Encoding.UTF8
            message.IsBodyHtml = True
            Dim client As New SmtpClient("mail." & provider, portnya) 'ok  'Dim client As New SmtpClient("mitsui.xinergix.com", 465) 'Gmail smtp    '[XU;_]G9i@#]
            Dim basicCredential1 As New System.Net.NetworkCredential("webadm@" & provider, apasscred)
            'client.EnableSsl = True

            If Trim$(ccadd) <> "" Then
                message.CC.Add(ccadd)
            End If
            client.EnableSsl = False
            client.UseDefaultCredentials = False
            client.Credentials = basicCredential1

            Dim data As New Attachment(myfileattachment, MediaTypeNames.Application.Octet)
            Dim disposition As ContentDisposition = data.ContentDisposition
            disposition.CreationDate = System.IO.File.GetCreationTime(myfileattachment)
            disposition.ModificationDate = System.IO.File.GetLastWriteTime(myfileattachment)
            disposition.ReadDate = System.IO.File.GetLastAccessTime(myfileattachment)
            message.Attachments.Add(data)

            client.Send(message)

            kirimemailpop3attach = "Sukses"
        Catch ex As Exception
            kirimemailpop3attach = "Gagal"
        End Try
        Return kirimemailpop3attach
    End Function
    Public Shared Function kirimemail(provider As String, portnya As Integer, messagebodynya As String, subjek As String, apasscred As String, fromemail As String, toemail As String, Optional ccadd As String = "") As String
        Try
            'fromemail = "pmbuqmedan@gmail.com" and networkcred username the same
            Dim message As New MailMessage(fromemail, toemail)
            message.Subject = subjek
            message.Body = messagebodynya
            message.BodyEncoding = Encoding.UTF8
            message.IsBodyHtml = True
            If Trim$(ccadd) <> "" Then
                message.CC.Add(ccadd)
            End If
            'Dim client As New SmtpClient("mail." & dmain, 26) 'ok  'Dim client As New SmtpClient("mitsui.xinergix.com", 465) 'Gmail smtp    '[XU;_]G9i@#]
            'smtp.gmail.com
            'mail.universitasquality.ac.id 
            '"ybhippsajvvadnpk"
            Dim client As New SmtpClient(provider, portnya) 'ok 
            Dim basicCredential1 As New System.Net.NetworkCredential(fromemail, apasscred)
            If provider = "smtp.gmail.com" Then
                client.EnableSsl = True
                client.UseDefaultCredentials = True
            Else
                client.EnableSsl = True
                client.UseDefaultCredentials = True
            End If
            client.Credentials = basicCredential1
            client.Send(message)
            kirimemail = "Sukses"
        Catch ex As Exception
            kirimemail = "Gagal"
        End Try
        Return kirimemail
    End Function
End Class