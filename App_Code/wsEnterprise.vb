Imports System.Web
Imports System.Web.Services
Imports System.Web.Services.Protocols
Imports System.Data
Imports System.Linq
Imports System.Data.SqlClient
Imports System.Web.Script.Services
Imports System.Web.Script.Serialization
Imports System.Collections.Generic

' To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line.
<System.Web.Script.Services.ScriptService()> _
<WebService(Namespace:="http://tempuri.org/")> _
<WebServiceBinding(ConformsTo:=WsiProfiles.BasicProfile1_1)> _
<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Public Class wsEnterprise
    Inherits System.Web.Services.WebService
    Private myconndb As String = System.Configuration.ConfigurationManager.ConnectionStrings("connuq").ConnectionString.ToString
    Private con As New SqlConnection(ConfigurationManager.ConnectionStrings("connuq").ToString())
    Private cmd As SqlCommand = Nothing
    Private dt As DataTable = Nothing 



#Region "Tahun Kurikulum"
    ' <WebMethod(Description:="dtTahunKurikulum")> _
    '<Script.Services.ScriptMethod(ResponseFormat:=Script.Services.ResponseFormat.Json)> _
    ' Public Function TahunKurikulum(ByVal input As String) As List(Of datakurikulum)
    '     Dim cnstr As String = System.Configuration.ConfigurationManager.ConnectionStrings("connQuality").ConnectionString
    '     Dim dt As DataTable = New DataTable()
    '     Dim rows As DataRow()
    '     Dim strSql As String = "select kurikulum from kurikulum order by tahunajaran1;"

    '     Using cnn As New SqlConnection(cnstr)
    '         cnn.Open()
    '         Using dad As New SqlDataAdapter(strSql, cnn)
    '             dad.Fill(dt)
    '         End Using
    '         cnn.Close()
    '     End Using

    '     rows = dt.Select(String.Format("kurikulum like '{0}%'", input))
    '     Dim resultdatakurikulum As New List(Of datakurikulum)
    '     For Each row In rows
    '         Dim r As New datakurikulum
    '         r.rkurikulum = row("kurikulum")
    '         resultdatakurikulum.Add(r)
    '     Next
    '     Return resultdatakurikulum
    ' End Function
    ' Public Class datakurikulum1
    '     Public rkurikulum As String
    ' End Class
#End Region

#Region "Validasi Kelas"
    <WebMethod(Description:="dtKelasvalidasi")> _
  <Script.Services.ScriptMethod(ResponseFormat:=Script.Services.ResponseFormat.Json)> _
    Public Function GetKelasvalidasi(ByVal input As String) As List(Of datakelas)
        Dim cnstr As String = System.Configuration.ConfigurationManager.ConnectionStrings("connQuality").ConnectionString
        Dim dt As DataTable = New DataTable()
        Dim rows As DataRow()
        Dim strSql As String = "SELECT KELAS FROM kelas ORDER BY Kelas"

        Using cnn As New SqlConnection(cnstr)
            cnn.Open()
            Using dad As New SqlDataAdapter(strSql, cnn)
                dad.Fill(dt)
            End Using
            cnn.Close()
        End Using

        rows = dt.Select(String.Format("KELAS like '%{0}%'", input))
        Dim resultdatakelas As New List(Of datakelas)
        For Each row In rows
            Dim r As New datakelas
            r.rkelas = row("KELAS")
            resultdatakelas.Add(r)
        Next
        Return resultdatakelas
    End Function
#End Region

#Region "Dosen"

    <WebMethod(Description:="dtTa")> _
    <Script.Services.ScriptMethod(ResponseFormat:=Script.Services.ResponseFormat.Json)> _
    Public Function GetdtTa(ByVal input As String) As List(Of rekdtTa)
        Dim cnstr As String = myconndb
        Dim dt As DataTable = New DataTable()
        Dim rows As DataRow()
        Dim strSql As String = "select kurikulum,TAHUNAJARAN1 from kurikulum order by kurikulum"

        Using cnn As New SqlConnection(cnstr)
            cnn.Open()
            Using dad As New SqlDataAdapter(strSql, cnn)
                dad.Fill(dt)
            End Using
            cnn.Close()
        End Using

        rows = dt.Select(String.Format("kurikulum+TAHUNAJARAN1 like '%{0}%'", input))
        Dim resultdtTa As New List(Of rekdtTa)
        For Each row In rows
            Dim r As New rekdtTa
            r.kodedtTa = row("kurikulum")
            r.namadtTa = row("TAHUNAJARAN1")
            resultdtTa.Add(r)
        Next
        Return resultdtTa
    End Function
    Public Class rekdtTa
        Public kodedtTa As String
        Public namadtTa As String
    End Class

    <WebMethod(Description:="dtTaa")> _
    <Script.Services.ScriptMethod(ResponseFormat:=Script.Services.ResponseFormat.Json)> _
    Public Function GetdtTaa(ByVal input As String) As List(Of rekdtTaa)
        Dim cnstr As String = myconndb
        Dim dt As DataTable = New DataTable()
        Dim rows As DataRow()
        Dim strSql As String = "select TAHUNAJARAN1 from TahunAjaranKurikulum order by idPrimary"

        Using cnn As New SqlConnection(cnstr)
            cnn.Open()
            Using dad As New SqlDataAdapter(strSql, cnn)
                dad.Fill(dt)
            End Using
            cnn.Close()
        End Using

        rows = dt.Select(String.Format("TAHUNAJARAN1 like '%{0}%'", input))
        Dim resultdtTaa As New List(Of rekdtTaa)
        For Each row In rows
            Dim r As New rekdtTaa
            r.kodedtTaa = row("TAHUNAJARAN1")
            resultdtTaa.Add(r)
        Next
        Return resultdtTaa
    End Function
    Public Class rekdtTaa
        Public kodedtTaa As String
    End Class

    <WebMethod(Description:="dtKelas")>
    <Script.Services.ScriptMethod(ResponseFormat:=Script.Services.ResponseFormat.Json)>
    Public Function GetdtKelas(ByVal input As String) As List(Of rekdtKelas)
        Dim cnstr As String = myconndb
        Dim dt As DataTable = New DataTable()
        Dim rows As DataRow()
        Dim strSql As String = "select kelas from kelas order by kelas"

        Using cnn As New SqlConnection(cnstr)
            cnn.Open()
            Using dad As New SqlDataAdapter(strSql, cnn)
                dad.Fill(dt)
            End Using
            cnn.Close()
        End Using

        rows = dt.Select(String.Format("kelas like '%{0}%'", input))
        Dim resultdtKelas As New List(Of rekdtKelas)
        For Each row In rows
            Dim r As New rekdtKelas
            r.kodedtKelas = row("kelas")
            resultdtKelas.Add(r)
        Next
        Return resultdtKelas
    End Function
    Public Class rekdtKelas
        Public kodedtKelas As String
    End Class
#End Region

#Region "Jam Ruang Jadwal"
    <WebMethod(Description:="dtJam")> _
    <Script.Services.ScriptMethod(ResponseFormat:=Script.Services.ResponseFormat.Json)> _
    Public Function GetdJam(ByVal input As String) As List(Of rekdtTaa)
        Dim cnstr As String = myconndb
        Dim dt As DataTable = New DataTable()
        Dim rows As DataRow()
        Dim strSql As String = "select JamRuang from Tbljam order by idPrimary"

        Using cnn As New SqlConnection(cnstr)
            cnn.Open()
            Using dad As New SqlDataAdapter(strSql, cnn)
                dad.Fill(dt)
            End Using
            cnn.Close()
        End Using

        rows = dt.Select(String.Format("JamRuang like '%{0}%'", input))
        Dim resultdtTaa As New List(Of rekdtTaa)
        For Each row In rows
            Dim r As New rekdtTaa
            r.kodedtTaa = row("JamRuang")
            resultdtTaa.Add(r)
        Next
        Return resultdtTaa
    End Function
#End Region

#Region "GetDosen2"

    <WebMethod(Description:="dtGetDosen2")> _
    <Script.Services.ScriptMethod(ResponseFormat:=Script.Services.ResponseFormat.Json)> _
    Public Function GetDosen2(ByVal input As String) As List(Of rekGetDosen2)
        Dim cnstr As String = myconndb
        Dim dt As DataTable = New DataTable()
        Dim rows As DataRow()
        Dim strSql As String = "SELECT IDDOSEN,NAMA FROM DOSEN ORDER BY IDDOSEN; "

        Using cnn As New SqlConnection(cnstr)
            cnn.Open()
            Using dad As New SqlDataAdapter(strSql, cnn)
                dad.Fill(dt)
            End Using
            cnn.Close()
        End Using

        rows = dt.Select(String.Format("IDDOSEN+Nama like '%{0}%'", input))
        Dim resultGetDosen2 As New List(Of rekGetDosen2)
        For Each row In rows
            Dim r As New rekGetDosen2
            r.kodeGetDosen2 = row("IDDOSEN")
            r.namaGetDosen2 = row("Nama")
            resultGetDosen2.Add(r)
        Next
        Return resultGetDosen2
    End Function
    Public Class rekGetDosen2
        Public kodeGetDosen2 As String
        Public namaGetDosen2 As String
    End Class


    <WebMethod(Description:="dtGetKampus2")>
    <Script.Services.ScriptMethod(ResponseFormat:=Script.Services.ResponseFormat.Json)>
    Public Function GetKampus2(ByVal input As String) As List(Of rekGetKampus2)
        Dim cnstr As String = myconndb
        Dim dt As DataTable = New DataTable()
        Dim rows As DataRow()
        Dim strSql As String = "SELECT IDKampus,Lokasi FROM Kampus ORDER BY IDKampus; "

        Using cnn As New SqlConnection(cnstr)
            cnn.Open()
            Using dad As New SqlDataAdapter(strSql, cnn)
                dad.Fill(dt)
            End Using
            cnn.Close()
        End Using

        rows = dt.Select(String.Format("IDKampus+Lokasi like '%{0}%'", input))
        Dim resultGetKampus2 As New List(Of rekGetKampus2)
        For Each row In rows
            Dim r As New rekGetKampus2
            r.kodeGetKampus2 = row("IDKampus")
            r.namaGetKampus2 = row("Lokasi")
            resultGetKampus2.Add(r)
        Next
        Return resultGetKampus2
    End Function
    Public Class rekGetKampus2
        Public kodeGetKampus2 As String
        Public namaGetKampus2 As String
    End Class

    <WebMethod(Description:="dtGetMK2")>
    <Script.Services.ScriptMethod(ResponseFormat:=Script.Services.ResponseFormat.Json)>
    Public Function GetMK2(ByVal input As String) As List(Of rekGetMK2)
        Dim cnstr As String = myconndb
        Dim dt As DataTable = New DataTable()
        Dim rows As DataRow()
        Dim strSql As String = "SELECT idmk,matakuliah FROM matakuliah ORDER BY idmk; "

        Using cnn As New SqlConnection(cnstr)
            cnn.Open()
            Using dad As New SqlDataAdapter(strSql, cnn)
                dad.Fill(dt)
            End Using
            cnn.Close()
        End Using

        rows = dt.Select(String.Format("idmk+matakuliah like '%{0}%'", input))
        Dim resultGetMK2 As New List(Of rekGetMK2)
        For Each row In rows
            Dim r As New rekGetMK2
            r.IDMKGet2 = row("idmk")
            r.namaGetMK2 = row("matakuliah")
            resultGetMK2.Add(r)
        Next
        Return resultGetMK2
    End Function
    Public Class rekGetMK2
        Public IDMKGet2 As String
        Public namaGetMK2 As String
    End Class
#End Region

    <WebMethod(Description:="dtGetMkKonversi")>
    <Script.Services.ScriptMethod(ResponseFormat:=Script.Services.ResponseFormat.Json)>
    Public Function GetMkKonversi2(ByVal input As String) As List(Of rekGetMkKonversi2)
        Dim cnstr As String = myconndb
        Dim dt As DataTable = New DataTable()
        Dim rows As DataRow()
        Dim strSql As String = "SELECT Distinct IDCABANG, LOKASI FROM TblCabang ORDER BY idCabang; "

        Using cnn As New SqlConnection(cnstr)
            cnn.Open()
            Using dad As New SqlDataAdapter(strSql, cnn)
                dad.Fill(dt)
            End Using
            cnn.Close()
        End Using

        rows = dt.Select(String.Format("IDCABANG+LOKASI like '%{0}%'", input))
        Dim resultGetMkKonversi2 As New List(Of rekGetMkKonversi2)
        For Each row In rows
            Dim r As New rekGetMkKonversi2
            r.kodeGetMkKonversi2 = row("IDCABANG")
            r.namaGetMkKonversi2 = row("LOKASI")
            resultGetMkKonversi2.Add(r)
        Next
        Return resultGetMkKonversi2
    End Function
    Public Class rekGetMkKonversi2
        Public kodeGetMkKonversi2 As String
        Public namaGetMkKonversi2 As String
    End Class

    <WebMethod(Description:="dtGetSupplier")>
    <Script.Services.ScriptMethod(ResponseFormat:=Script.Services.ResponseFormat.Json)>
    Public Function GetSupplier2(ByVal input As String) As List(Of rekGetSupplier2)
        Dim cnstr As String = myconndb
        Dim dt As DataTable = New DataTable()
        Dim rows As DataRow()
        Dim strSql As String = "SELECT Distinct idSupplier, Nama, Alamat  FROM TblSuplier ORDER BY Nama; "

        Using cnn As New SqlConnection(cnstr)
            cnn.Open()
            Using dad As New SqlDataAdapter(strSql, cnn)
                dad.Fill(dt)
            End Using
            cnn.Close()
        End Using

        rows = dt.Select(String.Format("IDSupplier+NAMA like '%{0}%'", input))
        Dim resultGetSupplier2 As New List(Of rekGetSupplier2)
        For Each row In rows
            Dim r As New rekGetSupplier2
            r.kodeGetSupplier2 = row("IDSupplier")
            r.namaGetSupplier2 = row("NAMA")
            r.alamatGetSupplier2 = row("ALAMAT")
            resultGetSupplier2.Add(r)
        Next
        Return resultGetSupplier2
    End Function
    Public Class rekGetSupplier2
        Public kodeGetSupplier2 As String
        Public namaGetSupplier2 As String
        Public alamatGetSupplier2 As String
    End Class


    <WebMethod(Description:="dtGetRekondisiBarang")>
    <Script.Services.ScriptMethod(ResponseFormat:=Script.Services.ResponseFormat.Json)>
    Public Function GetRekondisiBarang(ByVal input As String) As List(Of rekGetRekondisi)
        Dim cnstr As String = myconndb
        Dim dt As DataTable = New DataTable()
        Dim rows As DataRow()
        Dim strSql As String = "SELECT NoPlat, IdSupplier, NamaPemilik, Merk, Jenis, NoMesin, NoRangka  FROM TblBarang ORDER BY NoPlat; "

        Using cnn As New SqlConnection(cnstr)
            cnn.Open()
            Using dad As New SqlDataAdapter(strSql, cnn)
                dad.Fill(dt)
            End Using
            cnn.Close()
        End Using

        rows = dt.Select(String.Format("NoPlat+NamaPemilik like '%{0}%'", input))
        Dim resultGetRekondisi As New List(Of rekGetRekondisi)
        For Each row In rows
            Dim r As New rekGetRekondisi
            r.noplatGetRekondisi = row("NoPlat")
            r.idsupplierGetRekondisi = row("IdSupplier")
            r.namaGetRekondisi = row("NamaPemilik")
            r.merkGetRekondisi = row("Merk")
            r.jenisGetRekondisi = row("Jenis")
            r.nomesinGetRekondisi = row("NoMesin")
            r.norangkaGetRekondisi = row("NoRangka")
            resultGetRekondisi.Add(r)
        Next
        Return resultGetRekondisi
    End Function
    Public Class rekGetRekondisi
        Public noplatGetRekondisi As String
        Public idsupplierGetRekondisi As String
        Public namaGetRekondisi As String
        Public merkGetRekondisi As String
        Public jenisGetRekondisi As String
        Public nomesinGetRekondisi As String
        Public norangkaGetRekondisi As String
    End Class

    <WebMethod(Description:="dtGetMkKonversi")>
    <Script.Services.ScriptMethod(ResponseFormat:=Script.Services.ResponseFormat.Json)>
    Public Function GetPembelianBarang(ByVal input As String) As List(Of rekGetPembelian)
        Dim cnstr As String = myconndb
        Dim dt As DataTable = New DataTable()
        Dim rows As DataRow()
        Dim strSql As String = "SELECT idSupplier, NamaPemilik, NoPlat, Merk, Jenis, NoMesin, NoRangka, Total FROM TblRekondisi ORDER BY NoPlat; "

        Using cnn As New SqlConnection(cnstr)
            cnn.Open()
            Using dad As New SqlDataAdapter(strSql, cnn)
                dad.Fill(dt)
            End Using
            cnn.Close()
        End Using

        rows = dt.Select(String.Format("NoPlat+Merk like '%{0}%'", input))
        Dim resultGetPembelian As New List(Of rekGetPembelian)
        For Each row In rows
            Dim r As New rekGetPembelian
            r.idsupplierGetPembelian = row("idSupplier")
            r.namaGetPembelian = row("NamaPemilik")
            r.noplatGetPembelian = row("NoPlat")
            r.merkGetPembelian = row("Merk")
            r.jenisGetPembelian = row("Jenis")
            r.nomesinGetPembelian = row("NoMesin")
            r.norangkaGetPembelian = row("NoRangka")
            r.nototalGetPembelian = row("Total")
            resultGetPembelian.Add(r)
        Next
        Return resultGetPembelian
    End Function
    Public Class rekGetPembelian
        Public idsupplierGetPembelian As String
        Public namaGetPembelian As String
        Public noplatGetPembelian As String
        Public merkGetPembelian As String
        Public jenisGetPembelian As String
        Public nomesinGetPembelian As String
        Public norangkaGetPembelian As String
        Public nototalGetPembelian As Integer
    End Class

    <WebMethod(Description:="dtPenjualanBarang")>
    <Script.Services.ScriptMethod(ResponseFormat:=Script.Services.ResponseFormat.Json)>
    Public Function GetPenjualanBarang(ByVal input As String) As List(Of rekGetPenjualan)
        Dim cnstr As String = myconndb
        Dim dt As DataTable = New DataTable()
        Dim rows As DataRow()
        Dim strSql As String = "SELECT NoPlat, Merk, Jenis, NoMesin, NoRangka, Total FROM Tblbeli ORDER BY NoPlat; "

        Using cnn As New SqlConnection(cnstr)
            cnn.Open()
            Using dad As New SqlDataAdapter(strSql, cnn)
                dad.Fill(dt)
            End Using
            cnn.Close()
        End Using

        rows = dt.Select(String.Format("NoPlat+Merk like '%{0}%'", input))
        Dim resultGetPenjualan As New List(Of rekGetPenjualan)
        For Each row In rows
            Dim r As New rekGetPenjualan
            r.noplatGetPenjualan = row("NoPlat")
            r.merkGetPenjualan = row("Merk")
            r.jenisGetPenjualan = row("Jenis")
            r.nomesinGetPenjualan = row("NoMesin")
            r.norangkaGetPenjualan = row("NoRangka")
            r.nototalGetPenjualan = row("Total")
            resultGetPenjualan.Add(r)
        Next
        Return resultGetPenjualan
    End Function
    Public Class rekGetPenjualan
        Public noplatGetPenjualan As String
        Public merkGetPenjualan As String
        Public jenisGetPenjualan As String
        Public nomesinGetPenjualan As String
        Public norangkaGetPenjualan As String
        Public nototalGetPenjualan As Integer
    End Class


    <WebMethod(Description:="dtAngsuran")>
    <Script.Services.ScriptMethod(ResponseFormat:=Script.Services.ResponseFormat.Json)>
    Public Function GetAngsuranNasabah(ByVal input As String) As List(Of rekGetAngsuran)
        Dim cnstr As String = myconndb
        Dim dt As DataTable = New DataTable()
        Dim rows As DataRow()
        Dim strSql As String = "SELECT a.idnasabah,b.nama, a.idTrans, a.idCabang, a.idUnit, a.Wipem, a.NoPlat, a.Merk, a.Jenis, a.idsales, a.idsurvey, a.norangka, a.nomesin FROM TblJual a inner join tblnasabah b on b.idnasabah=a.idnasabah ORDER BY NoPlat; "

        Using cnn As New SqlConnection(cnstr)
            cnn.Open()
            Using dad As New SqlDataAdapter(strSql, cnn)
                dad.Fill(dt)
            End Using
            cnn.Close()
        End Using

        rows = dt.Select(String.Format("NoPlat+nama like '%{0}%'", input))
        Dim resultGetAngsuran As New List(Of rekGetAngsuran)
        For Each row In rows
            Dim r As New rekGetAngsuran
            r.idnasabahGetAngsuran = row("idnasabah")
            r.namanasabahGetAngsuran = row("nama")
            r.nopinjamanGetAngsuran = row("idTrans")
            r.idCabangGetAngsuran = row("idCabang")
            r.idUnitGetAngsuran = row("idUnit")
            r.WipemGetAngsuran = row("Wipem")
            r.idsalesGetAngsuran = row("idsales")
            r.idsurveyGetAngsuran = row("idsurvey")
            r.noplatGetAngsuran = row("NoPlat")
            r.merkGetAngsuran = row("Merk")
            r.jenisGetAngsuran = row("Jenis")
            r.rangkasGetAngsuran = row("norangka")
            r.mesinGetAngsuran = row("nomesin")
            resultGetAngsuran.Add(r)
        Next
        Return resultGetAngsuran
    End Function
    Public Class rekGetAngsuran
        Public idnasabahGetAngsuran As String
        Public namanasabahGetAngsuran As String
        Public nopinjamanGetAngsuran As String
        Public idCabangGetAngsuran As String
        Public idUnitGetAngsuran As String
        Public WipemGetAngsuran As String
        Public idsalesGetAngsuran As String
        Public idsurveyGetAngsuran As String
        Public noplatGetAngsuran As String
        Public merkGetAngsuran As String
        Public jenisGetAngsuran As String
        Public rangkasGetAngsuran As String
        Public mesinGetAngsuran As String
    End Class

    <WebMethod(Description:="dtNasabahPiutang")>
    <Script.Services.ScriptMethod(ResponseFormat:=Script.Services.ResponseFormat.Json)>
    Public Function GetNasabahNasabah(ByVal input As String) As List(Of rekGetNasabahPiutang)
        Dim cnstr As String = myconndb
        Dim dt As DataTable = New DataTable()
        Dim rows As DataRow()
        Dim strSql As String = "SELECT Distinct a.idnasabah,b.nama,b.alamat as alamat,  a.idTrans, c.noplat as noplat, c.kondisi as kondisi, c.merk as merk, c.jenis as jenis, c.type as type, c.warna as warna,CONVERT(VARCHAR(10),a.tanggal,103) as tanggal, a.angsuran  as angsuran,  a.dp  as dp, saldoawal as saldoawal, a.Total as HargaPokok, a.LamaAngsuran as LamaAngsuran FROM Tbljual a inner join tblbarang c on c.noplat=a.noplat inner join tblnasabah b on b.idnasabah=a.idnasabah  ORDER BY idnasabah; "

        Using cnn As New SqlConnection(cnstr)
            cnn.Open()
            Using dad As New SqlDataAdapter(strSql, cnn)
                dad.Fill(dt)
            End Using
            cnn.Close()
        End Using

        rows = dt.Select(String.Format("Nama+noplat like '%{0}%'", input))
        Dim resultGetNasabah As New List(Of rekGetNasabahPiutang)
        For Each row In rows
            Dim r As New rekGetNasabahPiutang
            r.idnasabahGetNasabah = row("idnasabah")
            r.namanasabahGetNasabah = row("nama")
            r.alamatGetNasabah = row("alamat")
            r.nopinjamanGetNasabah = row("idTrans")
            r.noplatGetNasabah = row("noplat")
            r.jenisGetNasabah = row("jenis")
            r.merkGetNasabah = row("merk")
            r.typeGetNasabah = row("Type")
            r.warnaGetNasabah = row("Warna")
            r.saldoawalGetNasabah = row("SaldoAwal")
            r.dpGetNasabah = row("dp")
            r.hargapokokGetNasabah = row("HargaPokok")
            r.angsuranGetNasabah = row("Angsuran")
            r.LamaangsuranGetNasabah = row("LamaAngsuran")
            r.tanggalGetNasabah = row("Tanggal")
            r.kondisiGetNasabah = row("kondisi")
            resultGetNasabah.Add(r)
        Next
        Return resultGetNasabah
    End Function
    Public Class rekGetNasabahPiutang
        Public nopinjamanGetNasabah As String
        Public idnasabahGetNasabah As String
        Public namanasabahGetNasabah As String
        Public alamatGetNasabah As String
        Public noplatGetNasabah As String
        Public jenisGetNasabah As String
        Public merkGetNasabah As String
        Public typeGetNasabah As String
        Public warnaGetNasabah As String
        Public saldoawalGetNasabah As Integer
        Public dpGetNasabah As Integer
        Public hargapokokGetNasabah As Integer
        Public angsuranGetNasabah As Integer
        Public LamaangsuranGetNasabah As Integer
        Public tanggalGetNasabah As Date
        Public kondisiGetNasabah As String
    End Class




    <WebMethod(Description:="dtDenda")>
    <Script.Services.ScriptMethod(ResponseFormat:=Script.Services.ResponseFormat.Json)>
    Public Function GetDendaNasabah(ByVal input As String) As List(Of rekGetDenda)
        Dim cnstr As String = myconndb
        Dim dt As DataTable = New DataTable()
        Dim rows As DataRow()
        Dim strSql As String = "SELECT a.IdTrans as IdTrans, a.IdCabang as IdCabang, a.IdUnit as IdUnit, a.Wipem as Wipem, a.idNasabah as idNasabah, b.nama as nama from Tbljual a inner join tblnasabah b on b.idnasabah=a.idnasabah ORDER BY b.nama Asc; "

        Using cnn As New SqlConnection(cnstr)
            cnn.Open()
            Using dad As New SqlDataAdapter(strSql, cnn)
                dad.Fill(dt)
            End Using
            cnn.Close()
        End Using

        rows = dt.Select(String.Format("nama+IdTrans like '%{0}%'", input))
        Dim resultGetDenda As New List(Of rekGetDenda)
        For Each row In rows
            Dim r As New rekGetDenda
            r.nopinjamanGetDenda = row("IdTrans")
            r.idnasabahGetDenda = row("idnasabah")
            r.namanasabahGetDenda = row("nama")
            r.idCabangGetDenda = row("idCabang")
            r.idUnitGetDenda = row("idUnit")
            r.WipemGetDenda = row("Wipem")
            resultGetDenda.Add(r)
        Next
        Return resultGetDenda
    End Function
    Public Class rekGetDenda
        Public idnasabahGetDenda As String
        Public namanasabahGetDenda As String
        Public nopinjamanGetDenda As String
        Public idCabangGetDenda As String
        Public idUnitGetDenda As String
        Public WipemGetDenda As String
    End Class


    <WebMethod(Description:="dtGetMkKonversi")>
    <Script.Services.ScriptMethod(ResponseFormat:=Script.Services.ResponseFormat.Json)>
    Public Function GetMkKonversiunit(ByVal input As String) As List(Of rekGetMkKonversi3)
        Dim cnstr As String = myconndb
        Dim dt As DataTable = New DataTable()
        Dim rows As DataRow()
        Dim strSql As String = "SELECT Distinct Idunit, LOKASI FROM TblUnitKerja ORDER BY idunit; "

        Using cnn As New SqlConnection(cnstr)
            cnn.Open()
            Using dad As New SqlDataAdapter(strSql, cnn)
                dad.Fill(dt)
            End Using
            cnn.Close()
        End Using

        rows = dt.Select(String.Format("IDUNIT+LOKASI like '%{0}%'", input))
        Dim resultGetMkKonversi3 As New List(Of rekGetMkKonversi3)
        For Each row In rows
            Dim r As New rekGetMkKonversi3
            r.kodeGetMkKonversi3 = row("IDUNIT")
            r.namaGetMkKonversi3 = row("LOKASI")
            resultGetMkKonversi3.Add(r)
        Next
        Return resultGetMkKonversi3
    End Function
    Public Class rekGetMkKonversi3
        Public kodeGetMkKonversi3 As String
        Public namaGetMkKonversi3 As String
    End Class

    <WebMethod(Description:="dtGetKaryawan")>
    <Script.Services.ScriptMethod(ResponseFormat:=Script.Services.ResponseFormat.Json)>
    Public Function GetKaryawan(ByVal input As String) As List(Of rekGetKaryawan)
        Dim cnstr As String = myconndb
        Dim dt As DataTable = New DataTable()
        Dim rows As DataRow()
        Dim strSql As String = "SELECT Distinct IDKaryawan, NAMA FROM TblPetugas ORDER BY IDKaryawan; "

        Using cnn As New SqlConnection(cnstr)
            cnn.Open()
            Using dad As New SqlDataAdapter(strSql, cnn)
                dad.Fill(dt)
            End Using
            cnn.Close()
        End Using

        rows = dt.Select(String.Format("IDKaryawan+NAMA like '%{0}%'", input))
        Dim resultGetKaryawan As New List(Of rekGetKaryawan)
        For Each row In rows
            Dim r As New rekGetKaryawan
            r.kodeGetKaryawan = row("IDKaryawan")
            r.namaGetKaryawan = row("NAMA")
            resultGetKaryawan.Add(r)
        Next
        Return resultGetKaryawan
    End Function
    Public Class rekGetKaryawan
        Public kodeGetKaryawan As String
        Public namaGetKaryawan As String
    End Class

    <WebMethod(Description:="dtGetWipem")>
    <Script.Services.ScriptMethod(ResponseFormat:=Script.Services.ResponseFormat.Json)>
    Public Function GetWipem(ByVal input As String) As List(Of rekGetWipem)
        Dim cnstr As String = myconndb
        Dim dt As DataTable = New DataTable()
        Dim rows As DataRow()
        Dim strSql As String = "SELECT Distinct IDWIPEM, LOKASI FROM TblWipem ORDER BY IDWIPEM; "

        Using cnn As New SqlConnection(cnstr)
            cnn.Open()
            Using dad As New SqlDataAdapter(strSql, cnn)
                dad.Fill(dt)
            End Using
            cnn.Close()
        End Using

        rows = dt.Select(String.Format("IDWIPEM+LOKASI like '%{0}%'", input))
        Dim resultGetWipem As New List(Of rekGetWipem)
        For Each row In rows
            Dim r As New rekGetWipem
            r.kodeGetWipem = row("IDWIPEM")
            r.namaGetWipem = row("LOKASI")
            resultGetWipem.Add(r)
        Next
        Return resultGetWipem
    End Function
    Public Class rekGetWipem
        Public kodeGetWipem As String
        Public namaGetWipem As String
    End Class

    <WebMethod(Description:="dtGetNasabah")>
    <Script.Services.ScriptMethod(ResponseFormat:=Script.Services.ResponseFormat.Json)>
    Public Function GetNasabah(ByVal input As String) As List(Of rekGetNasabah)
        Dim cnstr As String = myconndb
        Dim dt As DataTable = New DataTable()
        Dim rows As DataRow()
        Dim strSql As String = "SELECT Distinct IDNasabah, Nama FROM TblNasabah ORDER BY IDNasabah; "

        Using cnn As New SqlConnection(cnstr)
            cnn.Open()
            Using dad As New SqlDataAdapter(strSql, cnn)
                dad.Fill(dt)
            End Using
            cnn.Close()
        End Using

        rows = dt.Select(String.Format("IDNasabah+Nama like '%{0}%'", input))
        Dim resultGetNasabah As New List(Of rekGetNasabah)
        For Each row In rows
            Dim r As New rekGetNasabah
            r.kodeGetNasabah = row("IDNasabah")
            r.namaGetNasabah = row("Nama")
            resultGetNasabah.Add(r)
        Next
        Return resultGetNasabah
    End Function
    Public Class rekGetNasabah
        Public kodeGetNasabah As String
        Public namaGetNasabah As String
    End Class

    <WebMethod(Description:="dtMahasiswa")>
    <Script.Services.ScriptMethod(ResponseFormat:=Script.Services.ResponseFormat.Json)>
    Public Function GetMahasiswaKHS(ByVal input As String) As List(Of rekapmahasiswa)
        Dim cnstr As String = System.Configuration.ConfigurationManager.ConnectionStrings("connQuality").ConnectionString
        Dim dt As DataTable = New DataTable()
        Dim rows As DataRow()
        'Dim Typekelas As String = "PINDAHAN REGULER"
        'Dim tahunmasuk As Integer = 2022
        'Dim tahunkeluar As Integer = 2029
        'Dim pindahanmahasiswa As String = "Y"
        '    Dim strSql As String = "SELECT NPM,NAMA FROM MAHASISWA WHERE pindahan='" & pindahanmahasiswa & "' and tipekelas='" & Typekelas & "' and ta between " & tahunmasuk & " and " & tahunkeluar & "  order by NAMA"
        Dim strSql As String = "SELECT NPM,NAMA FROM MAHASISWA order by NAMA"
        Using cnn As New SqlConnection(cnstr)
            cnn.Open()
            Using dad As New SqlDataAdapter(strSql, cnn)
                dad.Fill(dt)
            End Using
            cnn.Close()
        End Using

        rows = dt.Select(String.Format("NPM+NAMA like '%{0}%'", input))
        Dim resultdatamahasiswa As New List(Of rekapmahasiswa)
        For Each row In rows
            Dim r As New rekapmahasiswa
            r.nimmahas = row("NPM")
            r.namamahas = row("Nama")
            resultdatamahasiswa.Add(r)
        Next
        Return resultdatamahasiswa
    End Function
    Public Class rekapmahasiswa
        Public nimmahas As String
        Public namamahas As String
    End Class


    <WebMethod(Description:="dtMahasiswa")>
    <Script.Services.ScriptMethod(ResponseFormat:=Script.Services.ResponseFormat.Json)>
    Public Function GetMahasiswaReportKHS(ByVal input As String) As List(Of rekapmahasiswaKhsKhs)
        Dim cnstr As String = System.Configuration.ConfigurationManager.ConnectionStrings("connQuality").ConnectionString
        Dim dt As DataTable = New DataTable()
        Dim rows As DataRow()
        Dim Typekelas As String = "PINDAHAN REGULER"
        Dim tahunmasuk As Integer = 2022
        Dim tahunkeluar As Integer = 2029
        Dim pindahanmahasiswa As String = "Y"
        Dim strSql As String = "SELECT NPM,NAMA FROM MAHASISWA order by NAMA"

        Using cnn As New SqlConnection(cnstr)
            cnn.Open()
            Using dad As New SqlDataAdapter(strSql, cnn)
                dad.Fill(dt)
            End Using
            cnn.Close()
        End Using

        rows = dt.Select(String.Format("NPM+NAMA like '%{0}%'", input))
        Dim resultdatamahasiswakhs As New List(Of rekapmahasiswaKhsKhs)
        For Each row In rows
            Dim r As New rekapmahasiswaKhsKhs
            r.nimmahas = row("NPM")
            r.namamahas = row("Nama")
            resultdatamahasiswakhs.Add(r)
        Next
        Return resultdatamahasiswakhs
    End Function
    Public Class rekapmahasiswaKhsKhs
        Public nimmahas As String
        Public namamahas As String
    End Class

    <WebMethod(Description:="dtkampus")>
    <Script.Services.ScriptMethod(ResponseFormat:=Script.Services.ResponseFormat.Json)>
    Public Function GetKampus(ByVal input As String) As List(Of rekapkampus)
        Dim cnstr As String = System.Configuration.ConfigurationManager.ConnectionStrings("connQuality").ConnectionString
        Dim dt As DataTable = New DataTable()
        Dim rows As DataRow()

        Dim strSql As String = "SELECT idkampus,lokasi FROM Kampus order by lokasi"

        Using cnn As New SqlConnection(cnstr)
            cnn.Open()
            Using dad As New SqlDataAdapter(strSql, cnn)
                dad.Fill(dt)
            End Using
            cnn.Close()
        End Using

        rows = dt.Select(String.Format("idkampus+lokasi like '%{0}%'", input))
        Dim resultdatakampus As New List(Of rekapkampus)
        For Each row In rows
            Dim r As New rekapkampus
            r.idkampusmahas = row("idkampus")
            r.lokasikampus = row("lokasi")
            resultdatakampus.Add(r)
        Next
        Return resultdatakampus
    End Function
    Public Class rekapkampus
        Public idkampusmahas As String
        Public lokasikampus As String
    End Class

    <WebMethod(Description:="dtprodi")>
    <Script.Services.ScriptMethod(ResponseFormat:=Script.Services.ResponseFormat.Json)>
    Public Function Getprodipmb(ByVal input As String) As List(Of rekapprodi)
        Dim cnstr As String = System.Configuration.ConfigurationManager.ConnectionStrings("connQuality").ConnectionString
        Dim dt As DataTable = New DataTable()
        Dim rows As DataRow()

        Dim strSql As String = "SELECT prodi,fakultas FROM prodipmb order by prodi"

        Using cnn As New SqlConnection(cnstr)
            cnn.Open()
            Using dad As New SqlDataAdapter(strSql, cnn)
                dad.Fill(dt)
            End Using
            cnn.Close()
        End Using

        rows = dt.Select(String.Format("prodi+fakultas like '%{0}%'", input))
        Dim resultdataprodi As New List(Of rekapprodi)
        For Each row In rows
            Dim r As New rekapprodi
            r.idprodi = row("prodi")
            r.lokasifakultas = row("fakultas")
            resultdataprodi.Add(r)
        Next
        Return resultdataprodi
    End Function
    Public Class rekapprodi
        Public idprodi As String
        Public lokasifakultas As String
    End Class

    <WebMethod(Description:="dtMahasiswa")> _
    <Script.Services.ScriptMethod(ResponseFormat:=Script.Services.ResponseFormat.Json)> _
    Public Function GetMahasiswa(ByVal input As String) As List(Of datamahasiswa)
        Dim cnstr As String = System.Configuration.ConfigurationManager.ConnectionStrings("connQuality").ConnectionString
        Dim dt As DataTable = New DataTable()
        Dim rows As DataRow()
        Dim strSql As String = "SELECT NPM,NAMA FROM MAHASISWA order by NAMA"

        Using cnn As New SqlConnection(cnstr)
            cnn.Open()
            Using dad As New SqlDataAdapter(strSql, cnn)
                dad.Fill(dt)
            End Using
            cnn.Close()
        End Using

        rows = dt.Select(String.Format("NAMA like '%{0}%'", input))
        Dim resultdatamahasiswa As New List(Of datamahasiswa)
        For Each row In rows
            Dim r As New datamahasiswa
            r.nim = row("NPM")
            r.namamhs = row("Nama")
            resultdatamahasiswa.Add(r)
        Next
        Return resultdatamahasiswa
    End Function
    Public Class datamahasiswa
        Public nim As String
        Public namamhs As String
    End Class

    <WebMethod(Description:="dtJabatanDosen")> _
    <Script.Services.ScriptMethod(ResponseFormat:=Script.Services.ResponseFormat.Json)> _
    Public Function GetJabatanDosen(ByVal input As String) As List(Of datajabatan)
        Dim cnstr As String = System.Configuration.ConfigurationManager.ConnectionStrings("connQuality").ConnectionString
        Dim dt As DataTable = New DataTable()
        Dim rows As DataRow()
        Dim strSql As String = "select JABATAN from JABATAN order by ItemNo"

        Using cnn As New SqlConnection(cnstr)
            cnn.Open()
            Using dad As New SqlDataAdapter(strSql, cnn)
                dad.Fill(dt)
            End Using
            cnn.Close()
        End Using

        rows = dt.Select(String.Format("JABATAN like '{0}%'", input))
        Dim resultdatajabatan As New List(Of datajabatan)
        For Each row In rows
            Dim r As New datajabatan
            r.rjabatan = row("JABATAN")
            resultdatajabatan.Add(r)
        Next
        Return resultdatajabatan
    End Function
    Public Class datajabatan
        Public rjabatan As String
    End Class

    <WebMethod(Description:="dtAsalKota")> _
    <Script.Services.ScriptMethod(ResponseFormat:=Script.Services.ResponseFormat.Json)> _
    Public Function GetAsalKotaDosen(ByVal input As String) As List(Of dataasalkota)
        Dim cnstr As String = System.Configuration.ConfigurationManager.ConnectionStrings("connQuality").ConnectionString
        Dim dt As DataTable = New DataTable()
        Dim rows As DataRow()
        Dim strSql As String = "SELECT LOKASI  FROM KAMPUS order by LOKASI"

        Using cnn As New SqlConnection(cnstr)
            cnn.Open()
            Using dad As New SqlDataAdapter(strSql, cnn)
                dad.Fill(dt)
            End Using
            cnn.Close()
        End Using

        rows = dt.Select(String.Format("LOKASI like '%{0}%'", input))
        Dim resultasalkota As New List(Of dataasalkota)
        For Each row In rows
            Dim r As New dataasalkota
            r.rLokasi = row("LOKASI")
            resultasalkota.Add(r)
        Next
        Return resultasalkota
    End Function
    Public Class dataasalkota
        Public rLokasi As String
    End Class

    <WebMethod(Description:="dtJenjangDosen")> _
    <Script.Services.ScriptMethod(ResponseFormat:=Script.Services.ResponseFormat.Json)> _
    Public Function GetJenjangDosen(ByVal input As String) As List(Of datajenjangakademik)
        Dim cnstr As String = System.Configuration.ConfigurationManager.ConnectionStrings("connQuality").ConnectionString
        Dim dt As DataTable = New DataTable()
        Dim rows As DataRow()
        Dim strSql As String = "SELECT JENJANGAKADEMIK  FROM JENJANGAKADEMIK order by ItemNo "

        Using cnn As New SqlConnection(cnstr)
            cnn.Open()
            Using dad As New SqlDataAdapter(strSql, cnn)
                dad.Fill(dt)
            End Using
            cnn.Close()
        End Using

        rows = dt.Select(String.Format("JENJANGAKADEMIK like '%{0}%'", input))
        Dim resultrek As New List(Of datajenjangakademik)
        For Each row In rows
            Dim r As New datajenjangakademik
            r.rdatajenjangakademik = row("JENJANGAKADEMIK")
            resultrek.Add(r)
        Next
        Return resultrek
    End Function
    Public Class datajenjangakademik
        Public rdatajenjangakademik As String
    End Class

    <WebMethod(Description:="dtProgramStudiDosen")> _
    <Script.Services.ScriptMethod(ResponseFormat:=Script.Services.ResponseFormat.Json)> _
    Public Function GetProdiDosen(ByVal input As String) As List(Of dataprodidosen)
        Dim cnstr As String = System.Configuration.ConfigurationManager.ConnectionStrings("connQuality").ConnectionString
        Dim dt As DataTable = New DataTable()
        Dim rows As DataRow()
        Dim strSql As String = "SELECT PRODI  FROM ProdiFakultas order by IdFakultas,ItemNo "

        Using cnn As New SqlConnection(cnstr)
            cnn.Open()
            Using dad As New SqlDataAdapter(strSql, cnn)
                dad.Fill(dt)
            End Using
            cnn.Close()
        End Using

        rows = dt.Select(String.Format("PRODI like '%{0}%'", input))
        Dim resultprodi As New List(Of dataprodidosen)
        For Each row In rows
            Dim r As New dataprodidosen
            r.rprodi = row("PRODI")
            resultprodi.Add(r)
        Next
        Return resultprodi
    End Function
    Public Class dataprodidosen
        Public rprodi As String
    End Class

    <WebMethod(Description:="dtMataKuliah")> _
    <Script.Services.ScriptMethod(ResponseFormat:=Script.Services.ResponseFormat.Json)> _
    Public Function GetMataKuliah(ByVal input As String) As List(Of datamatakuliah)
        Dim cnstr As String = System.Configuration.ConfigurationManager.ConnectionStrings("connQuality").ConnectionString
        Dim dt As DataTable = New DataTable()
        Dim rows As DataRow()
        Dim strSql As String = "SELECT IDMK,MATAKULIAH FROM MATAKULIAH "

        Using cnn As New SqlConnection(cnstr)
            cnn.Open()
            Using dad As New SqlDataAdapter(strSql, cnn)
                dad.Fill(dt)
            End Using
            cnn.Close()
        End Using

        rows = dt.Select(String.Format("IDMK+MATAKULIAH like '%{0}%'", input))
        Dim resultrektipe As New List(Of datamatakuliah)
        For Each row In rows
            Dim r As New datamatakuliah
            r.ridmk = row("IDMK")
            r.rmatakuliah = row("MATAKULIAH")
            resultrektipe.Add(r)
        Next
        Return resultrektipe
    End Function
    Public Class datamatakuliah
        Public ridmk As String
        Public rmatakuliah As String
    End Class

    <WebMethod(Description:="dtTipeKelas")> _
    <Script.Services.ScriptMethod(ResponseFormat:=Script.Services.ResponseFormat.Json)> _
    Public Function GetTipeKelas(ByVal input As String) As List(Of datatipekelas)
        Dim cnstr As String = System.Configuration.ConfigurationManager.ConnectionStrings("connQuality").ConnectionString
        Dim dt As DataTable = New DataTable()
        Dim rows As DataRow()
        Dim strSql As String = "SELECT TIPEKEAS FROM TIPEKELAS ORDER BY TIPEKELAS "

        Using cnn As New SqlConnection(cnstr)
            cnn.Open()
            Using dad As New SqlDataAdapter(strSql, cnn)
                dad.Fill(dt)
            End Using
            cnn.Close()
        End Using

        rows = dt.Select(String.Format("TIPEKELAS like '%{0}%'", input))
        Dim resulttipekelas As New List(Of datatipekelas)
        For Each row In rows
            Dim r As New datatipekelas
            r.rtipekelas = row("TIPEKELAS")
            resulttipekelas.Add(r)
        Next
        Return resulttipekelas
    End Function
    Public Class datatipekelas
        Public rtipekelas As String
    End Class


    <WebMethod(Description:="dtGedung")> _
    <Script.Services.ScriptMethod(ResponseFormat:=Script.Services.ResponseFormat.Json)> _
    Public Function GetGedung(ByVal input As String) As List(Of datagedung)
        Dim cnstr As String = System.Configuration.ConfigurationManager.ConnectionStrings("connQuality").ConnectionString
        Dim dt As DataTable = New DataTable()
        Dim rows As DataRow()
        Dim strSql As String = "SELECT IDKAMPUS,LOKASI FROM KAMPUS ORDER BY IDKAMPUS "

        Using cnn As New SqlConnection(cnstr)
            cnn.Open()
            Using dad As New SqlDataAdapter(strSql, cnn)
                dad.Fill(dt)
            End Using
            cnn.Close()
        End Using

        rows = dt.Select(String.Format("IDKAMPUS+LOKASI like '%{0}%'", input))
        Dim resultdatagedung As New List(Of datagedung)
        For Each row In rows
            Dim r As New datagedung
            r.ridkampus = row("IDKAMPUS")
            r.rnamakampus = row("LOKASI")
            resultdatagedung.Add(r)
        Next
        Return resultdatagedung
    End Function
    Public Class datagedung
        Public ridkampus As String
        Public rnamakampus As String
    End Class
     
    <WebMethod(Description:="dtKelas")> _
    <Script.Services.ScriptMethod(ResponseFormat:=Script.Services.ResponseFormat.Json)> _
    Public Function GetCabang(ByVal input As String) As List(Of datakelas)
        Dim cnstr As String = System.Configuration.ConfigurationManager.ConnectionStrings("connQuality").ConnectionString
        Dim dt As DataTable = New DataTable()
        Dim rows As DataRow()
        Dim strSql As String = "SELECT KELAS FROM KELAS ORDER BY kelas "

        Using cnn As New SqlConnection(cnstr)
            cnn.Open()
            Using dad As New SqlDataAdapter(strSql, cnn)
                dad.Fill(dt)
            End Using
            cnn.Close()
        End Using

        rows = dt.Select(String.Format("KELAS like '%{0}%'", input))
        Dim resultdatakelas As New List(Of datakelas)
        For Each row In rows
            Dim r As New datakelas
            r.rkelas = row("KELAS")
            resultdatakelas.Add(r)
        Next
        Return resultdatakelas
    End Function
    Public Class datakelas
        Public rkelas As String
    End Class
    Private Function GetJson(ByVal dt As DataTable) As String
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
    Public Function GetJsonSimple(ByVal dt As DataTable) As String
        Return New JavaScriptSerializer().Serialize(From dr As DataRow In dt.Rows Select dt.Columns.Cast(Of DataColumn)().ToDictionary(Function(col) col.ColumnName, Function(col) dr(col)))
    End Function
    Public Function DataSetToJSON(ds As DataSet) As String
        Dim dict As New Dictionary(Of String, Object)

        For Each dt As DataTable In ds.Tables
            Dim arr(dt.Rows.Count) As Object

            For i As Integer = 0 To dt.Rows.Count - 1
                arr(i) = dt.Rows(i).ItemArray
            Next

            dict.Add(dt.TableName, arr)
        Next

        Dim json As New JavaScriptSerializer
        Return json.Serialize(dict)
    End Function
End Class

 