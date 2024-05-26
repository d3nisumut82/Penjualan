<%@ Page Title="Leasing  | Duta Motor" Language="VB" MasterPageFile="~/MasterPage.master" AutoEventWireup="false" CodeFile="EnterpriseHome.aspx.vb" Inherits="EnterpriseHome" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
     <style type="text/css">
        .list-group {
            width: 85%;
            margin: auto;
        }
        .panel::-webkit-scrollbar {
            display: none;
        }
    </style>


    <script type="text/javascript">
        $(document).ready(function () {
//            $('#<%=GridView1.ClientID%> thead tr').clone(true).appendTo('#<%=GridView1.ClientID%> thead');
            $('#<%=GridView1.ClientID%> thead tr:eq(1) th').each(function (i) {
                var title = $(this).text();
//                $(this).html('<input type="text" class="form-control" />');
                $('input', this).on('keyup change', function () {
                    if (table.column(i).search() !== this.value) {
                        table
                            .column(i)
                            .search(this.value)
                            .draw();
                    }
                });
            });

            var table = $('#<%=GridView1.ClientID%>').prepend($("<thead></thead>").append($(this).find("tr:first"))).DataTable({
                lengthMenu: [[10, 20, 50, -1], [10, 20, 50, "All"]],
                into: true,
                autoWidth: false,
                dom: "<'form-group row'<'col-sm-6 pull-left'B><'col-sm-6 searchgrid pull-right'f>><'row'>tr<'row form-group'><'row'<'col-sm-6'l><'col-sm-6 pull-right'p>><'clear'><'row'<'col-sm-6'i><'col-sm-6 pull-right'>>",
                buttons: [
                    { extend: 'print', text: 'Export', className: 'btn btn-success', titleAttr: 'Export' },
                ]
            });

            $('.searchgrid label input').css('width', '300px');
            $('.searchgrid label input').removeClass('form-control-sm');
        });
    </script>

<%-- <script type="text/javascript">
        $(document).ready(function () {
//            $('#<%=GridView2.ClientID%> thead tr').clone(true).appendTo('#<%=GridView2.ClientID%> thead');
            $('#<%=GridView2.ClientID%> thead tr:eq(1) th').each(function (i) {
                var title = $(this).text();
//                $(this).html('<input type="text" class="form-control" />');
                $('input', this).on('keyup change', function () {
                    if (table.column(i).search() !== this.value) {
                        table
                            .column(i)
                            .search(this.value)
                            .draw();
                    }
                });
            });

            var table = $('#<%=GridView2.ClientID%>').prepend($("<thead></thead>").append($(this).find("tr:first"))).DataTable({
                lengthMenu: [[10, 20, 50, -1], [10, 20, 50, "All"]],
                into: true,
                autoWidth: false,
                dom: "<'form-group row'<'col-sm-6 pull-left'B><'col-sm-6 searchgrid pull-right'f>><'row'>tr<'row form-group'><'row'<'col-sm-6'l><'col-sm-6 pull-right'p>><'clear'><'row'<'col-sm-6'i><'col-sm-6 pull-right'>>",
                buttons: [
                    { extend: 'print', text: 'Export', className: 'btn btn-success', titleAttr: 'Export' },
                ]
            });

            $('.searchgrid label input').css('width', '300px');
            $('.searchgrid label input').removeClass('form-control-sm');
        });
    </script>

    <script type="text/javascript">
        $(document).ready(function () {
//            $('#<%=GridView3.ClientID%> thead tr').clone(true).appendTo('#<%=GridView3.ClientID%> thead');
            $('#<%=GridView3.ClientID%> thead tr:eq(1) th').each(function (i) {
                var title = $(this).text();
//                $(this).html('<input type="text" class="form-control" />');
                $('input', this).on('keyup change', function () {
                    if (table.column(i).search() !== this.value) {
                        table
                            .column(i)
                            .search(this.value)
                            .draw();
                    }
                });
            });

            var table = $('#<%=GridView3.ClientID%>').prepend($("<thead></thead>").append($(this).find("tr:first"))).DataTable({
                lengthMenu: [[10, 20, 50, -1], [10, 20, 50, "All"]],
                into: true,
                autoWidth: false,
                dom: "<'form-group row'<'col-sm-6 pull-left'B><'col-sm-6 searchgrid pull-right'f>><'row'>tr<'row form-group'><'row'<'col-sm-6'l><'col-sm-6 pull-right'p>><'clear'><'row'<'col-sm-6'i><'col-sm-6 pull-right'>>",
                buttons: [
                    { extend: 'print', text: 'Export', className: 'btn btn-success', titleAttr: 'Export' },
                ]
            });

            $('.searchgrid label input').css('width', '300px');
            $('.searchgrid label input').removeClass('form-control-sm');
        });
    </script>--%>
     <script type="text/javascript">
    if (window.history.replaceState) {
        window.history.replaceState(null, null, window.location.href);
    }
</script>
 <style type="text/css">
   .HiddenCol{display:none;}    
</style>

</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

<div class="content-wrapper">
    <!-- Content Header (Page header) -->
    <section class="content-header">
      <div class="container-fluid">



        <div class="row mb-2">
          <div class="col-sm-6">
            
          </div>
          <div class="col-sm-6">
            <ol class="breadcrumb float-sm-right">
              <li class="breadcrumb-item"><a href="#">Home</a></li>
              <li class="breadcrumb-item active">Dashboard</li>
            </ol>
          </div>
        </div>
      </div><!-- /.container-fluid -->
    </section>

      <section class="content">
            <div class="container-fluid">
             <!-- Main content -->
                <section class="content">
                    <div class="container-fluid">
                 <!-- Small boxes (Stat box) -->
                         <div class="row">
          <div class="col-lg-3 col-6">
            <!-- small box -->
            <div class="small-box bg-info">
              <div class="inner">
                <h3>
                    <asp:Label ID="lblkampus" runat="server" Font-Bold="true" Font-Size="30"></asp:Label>
                </h3>
                <p>NASABAH</p>
              </div>
              <div class="icon">
                <i class="ion ion-bag"></i>
              </div>
              <a href="NasabahList.aspx" class="small-box-footer">More info <i class="fas fa-arrow-circle-right"></i></a>
            </div>
          </div>
          <!-- ./col -->
          <div class="col-lg-3 col-6">
            <!-- small box -->
            <div class="small-box bg-success">
              <div class="inner">
                <h3>
                    <asp:Label ID="lblfakultas" runat="server" Font-Bold="true" Font-Size="30"></asp:Label>
                </h3>
                <p>REKONDISI </p>
              </div>
              <div class="icon">
                <i class="ion ion-stats-bars"></i>
              </div>
              <a href="RekondisiBarangList.aspx" class="small-box-footer">More info <i class="fas fa-arrow-circle-right"></i></a>
            </div>
          </div>
          <!-- ./col -->
          <div class="col-lg-3 col-6">
            <!-- small box -->
            <div class="small-box bg-warning">
              <div class="inner">
                 <h3>
                    <asp:Label ID="lblprodi" runat="server" Font-Bold="true" Font-Size="30"></asp:Label>
                </h3>
                <p>BELI </p>
              </div>
              <div class="icon">
                <i class="ion ion-person-add"></i>
              </div>
              <a href="BeliBarangList.aspx" class="small-box-footer">More info <i class="fas fa-arrow-circle-right"></i></a>
            </div>
          </div>
          <!-- ./col -->
          <div class="col-lg-3 col-6">
            <!-- small box -->
            <div class="small-box bg-danger">
              <div class="inner">
                     <h3>
                        <asp:Label ID="lblmata" runat="server" Font-Bold="true" Font-Size="30"></asp:Label>
                    </h3>
                <p>JUAL</p>
              </div>
              <div class="icon">
                <i class="ion ion-pie-graph"></i>
              </div>
              <a href="JualBarangList.aspx" class="small-box-footer">More info <i class="fas fa-arrow-circle-right"></i></a>
            </div>
          </div>
          <!-- ./col -->
                              <div class="col-lg-3 col-6">
            <!-- small box -->
            <div class="small-box bg-danger">
              <div class="inner">
                     <h3>
                        <asp:Label ID="lbldosen" runat="server" Font-Bold="true" Font-Size="30"></asp:Label>
                    </h3>
                <p>ANGSURAN</p>
              </div>
              <div class="icon">
                <i class="ion ion-pie-graph"></i>
              </div>
              <a href="AngsuranNasabahList.aspx" class="small-box-footer">More info <i class="fas fa-arrow-circle-right"></i></a>
            </div>
          </div>
          <!-- ./col -->
        </div>

                    </div><!-- /.container-fluid -->
                </section>
        </section>
        
     <div class="card card-primary collapsed-card">
              <div class="card-header">
                <h3 class="card-title">History Rekondisi Barang</h3>

                <div class="card-tools">
                  <button type="button" class="btn btn-tool" data-card-widget="collapse"><i class="fas fa-plus"></i>
                  </button>
                </div>
                <!-- /.card-tools -->
              </div>
            <!-- Default box -->
  <div class="card-body">
      <div class="card-body">
     <%--<asp:GridView ID="GridView1" runat="server" CssClass="table table-bordered table-hover"
                        AutoGenerateColumns="false" OnRowDataBound="GridView1_RowDataBound" CssClass="table table-bordered table-hover" HeaderStyle-BackColor="#2E3C59" HeaderStyle-ForeColor ="White"
                                AutoGenerateColumns="false" DataSourceID="GridDataSource">--%>
            <asp:GridView ID="GridView1" runat="server" CssClass="table table-bordered table-hover" HeaderStyle-BackColor="#2E3C59" HeaderStyle-ForeColor ="White"
                                AutoGenerateColumns="false" OnRowDataBound="GridView1_RowDataBound" DataSourceID="GridDataSource">
                        <Columns>                       
                            <asp:BoundField DataField="idTrans" HeaderText="KODE" NullDisplayText="-" />
                             <asp:BoundField DataField="NoPlat" HeaderText="NOMOR PLAT" NullDisplayText="-" />
                             <asp:BoundField DataField="NamaPemilik" HeaderText="NAMA PEMILIK" NullDisplayText="-" />
                             <asp:BoundField DataField="Merk" HeaderText="MERK" NullDisplayText="-" />
                             <asp:BoundField DataField="Jenis" HeaderText="JENIS" NullDisplayText="-" />
                             <asp:BoundField DataField="NoMesin" HeaderText="NOMOR MESIN" NullDisplayText="-" />
                             <asp:BoundField DataField="NoRangka" HeaderText="NOMOR RANGKA" NullDisplayText="-" />
                             <asp:BoundField DataField="BiayaRekondisi" HeaderText="BIAYA REKONDISI" NullDisplayText="-" />
                             <asp:BoundField DataField="BiayaAdministrasi" HeaderText="BIAYA ADMINISTRASI" NullDisplayText="-" />
                             <asp:BoundField DataField="BiayaKomisi" HeaderText="BIAYA KOMISI" NullDisplayText="-" />
                             <asp:BoundField DataField="Tanggal" HeaderText="TANGGAL" NullDisplayText="-" />
                             <asp:BoundField DataField="Keterangan" HeaderText="KETERANGAN" NullDisplayText="-" />
                        </Columns>
                        <SortedAscendingCellStyle />
                        <SortedAscendingHeaderStyle />
                        <SortedDescendingCellStyle />
                        <SortedDescendingHeaderStyle />
                        <FooterStyle />
                        <PagerStyle HorizontalAlign="Left" />
                    </asp:GridView>
                    
                    <asp:SqlDataSource ID="GridDataSource" runat="server" ConnectionString="<%$ ConnectionStrings:connUQ %>"
                        SelectCommand="SELECT idTrans as idTrans, IdCabang as IdCabang, Idunit as Idunit, NoPlat as NoPlat, NamaPemilik as NamaPemilik, Merk as Merk, Jenis as Jenis, FORMAT(BiayaRekondisi,'#,###,##0') as BiayaRekondisi, FORMAT(BiayaAdministrasi,'#,###,##0')  as BiayaAdministrasi,FORMAT(BiayaKomisi,'#,###,##0')  as BiayaKomisi, NoMesin as NoMesin, NoRangka as NoRangka, CONVERT(VARCHAR(10),Tanggal,103) as Tanggal,Keterangan AS Keterangan from TblRekondisi WHERE idTrans <> '' ORDER BY idTrans"></asp:SqlDataSource>

                 </div>
            </div>   
       </div>   
    
      <div class="card card-primary collapsed-card">
              <div class="card-header">
                <h3 class="card-title">History Pembelian Barang</h3>

                <div class="card-tools">
                  <button type="button" class="btn btn-tool" data-card-widget="collapse"><i class="fas fa-plus"></i>
                  </button>
                </div>
                <!-- /.card-tools -->
              </div>
            <!-- Default box -->
  <div class="card-body">
      <div class="card-body">
     <%--<asp:GridView ID="GridView2" runat="server" CssClass="table table-bordered table-hover"
                        AutoGenerateColumns="false" OnRowDataBound="GridView2_RowDataBound" CssClass="table table-bordered table-hover" HeaderStyle-BackColor="#2E3C59" HeaderStyle-ForeColor ="White"
                                AutoGenerateColumns="false" DataSourceID="GridDataSource2">--%>
            <asp:GridView ID="GridView2" runat="server" CssClass="table table-bordered table-hover" HeaderStyle-BackColor="#2E3C59" HeaderStyle-ForeColor ="White"
                                AutoGenerateColumns="false" OnRowDataBound="GridView2_RowDataBound" DataSourceID="GridDataSource2">
                        <Columns>
                            <asp:BoundField DataField="idTrans" HeaderText="KODE" NullDisplayText="-" />
                             <asp:BoundField DataField="NoPlat" HeaderText="NOMOR PLAT" NullDisplayText="-" />
                             <asp:BoundField DataField="Merk" HeaderText="MERK" NullDisplayText="-" />
                             <asp:BoundField DataField="Jenis" HeaderText="JENIS" NullDisplayText="-" />
                             <asp:BoundField DataField="NoMesin" HeaderText="NOMOR MESIN" NullDisplayText="-" />
                             <asp:BoundField DataField="NoRangka" HeaderText="NOMOR RANGKA" NullDisplayText="-" />
                             <asp:BoundField DataField="JenisPembelian" HeaderText="JENIS PEMBELIAN" NullDisplayText="-" />
                             <asp:BoundField DataField="HargaBeli" HeaderText="HARGA BELI" NullDisplayText="-" />
                             <asp:BoundField DataField="BiayaRekondisi" HeaderText="BIAYA REKONDISI" NullDisplayText="-" />
                             <asp:BoundField DataField="Total" HeaderText="TOTAL" NullDisplayText="-" />
                             <asp:BoundField DataField="Keterangan" HeaderText="KETERANGAN" NullDisplayText="-" /> 
                        </Columns>
                        <SortedAscendingCellStyle />
                        <SortedAscendingHeaderStyle />
                        <SortedDescendingCellStyle />
                        <SortedDescendingHeaderStyle />
                        <FooterStyle />
                        <PagerStyle HorizontalAlign="Left" />
                     </asp:GridView>
    <asp:SqlDataSource ID="GridDataSource2" runat="server" ConnectionString="<%$ ConnectionStrings:connUQ %>"
        SelectCommand="SELECT idTrans as idTrans, IdCabang as IdCabang, Idunit as Idunit, NoPlat as NoPlat, Merk as Merk, Jenis as Jenis, FORMAT(HargaBeli,'#,###,##0') as HargaBeli, FORMAT(BiayaRekondisi,'#,###,##0')  as BiayaRekondisi,FORMAT(Total,'#,###,##0')  as Total, NoMesin as NoMesin, NoRangka as NoRangka, CONVERT(VARCHAR(10),Tanggal,103) as Tanggal,Keterangan AS Keterangan, JenisPembelian AS JenisPembelian from TblBeli WHERE idTrans <> '' ORDER BY idTrans"></asp:SqlDataSource>

               </div>
            </div>
    </div>

        <div class="card card-primary collapsed-card">
              <div class="card-header">
                <h3 class="card-title">History Penjualan Barang</h3>

                <div class="card-tools">
                  <button type="button" class="btn btn-tool" data-card-widget="collapse"><i class="fas fa-plus"></i>
                  </button>
                </div>
                <!-- /.card-tools -->
              </div>
            <!-- Default box -->
  <div class="card-body">
      <div class="card-body">
           <asp:GridView ID="GridView3" runat="server" CssClass="table table-bordered table-hover" HeaderStyle-BackColor="#2E3C59" HeaderStyle-ForeColor ="White"
                                AutoGenerateColumns="false" OnRowDataBound="GridView3_RowDataBound" DataSourceID="GridDataSource3">
                        <Columns>
                            <asp:BoundField DataField="idTrans" HeaderText="KODE" NullDisplayText="-" />
                             <asp:BoundField DataField="NoPlat" HeaderText="NOMOR PLAT" NullDisplayText="-" />
                             <asp:BoundField DataField="Merk" HeaderText="MERK" NullDisplayText="-" />
                             <asp:BoundField DataField="Jenis" HeaderText="JENIS" NullDisplayText="-" />
                             <asp:BoundField DataField="NoMesin" HeaderText="NOMOR MESIN" NullDisplayText="-" />
                             <asp:BoundField DataField="NoRangka" HeaderText="NOMOR RANGKA" NullDisplayText="-" />
                             <asp:BoundField DataField="JenisPenjualan" HeaderText="JENIS PENJUALAN" NullDisplayText="-" />
                             <asp:BoundField DataField="SaldoAwal" HeaderText="SALDO AWAL" NullDisplayText="-" />
                             <asp:BoundField DataField="HargaPokok" HeaderText="HARGA POKOK" NullDisplayText="-" />
                             <asp:BoundField DataField="DP" HeaderText="DP" NullDisplayText="-" />
                             <asp:BoundField DataField="Total" HeaderText="TOTAL" NullDisplayText="-" />
                             <asp:BoundField DataField="Bunga" HeaderText="BUNGA" NullDisplayText="-" />
                             <asp:BoundField DataField="Angsuran" HeaderText="ANGSURAN" NullDisplayText="-" />
                             <asp:BoundField DataField="TglJatuhTempo" HeaderText="TANGGAL JATUH TEMPO" NullDisplayText="-" /> 
                        </Columns>
                        <SortedAscendingCellStyle />
                        <SortedAscendingHeaderStyle />
                        <SortedDescendingCellStyle />
                        <SortedDescendingHeaderStyle />
                        <FooterStyle />
                        <PagerStyle HorizontalAlign="Left" />
                     </asp:GridView>
                <asp:SqlDataSource ID="GridDataSource3" runat="server" ConnectionString="<%$ ConnectionStrings:connUQ %>"
        SelectCommand="SELECT idTrans as idTrans, IdCabang as IdCabang, Idunit as Idunit, NoPlat as NoPlat, Merk as Merk, Jenis as Jenis, FORMAT(SaldoAwal,'#,###,##0') as SaldoAwal, FORMAT(HargaPokok,'#,###,##0') as HargaPokok, FORMAT(DP,'#,###,##0')  as DP,FORMAT(Total,'#,###,##0')  as Total, FORMAT(SaldoAwal,'#,###,##0')  as SaldoAwal, FORMAT(Bunga,'#,###,##0')  as Bunga, FORMAT(Angsuran,'#,###,##0')  as Angsuran, NoMesin as NoMesin, NoRangka as NoRangka, CONVERT(VARCHAR(10),TglJatuhTempo,103) as TglJatuhTempo,Keterangan AS Keterangan, JenisPenjualan AS JenisPenjualan from TblJual WHERE idTrans <> '' ORDER BY idTrans"></asp:SqlDataSource>


               </div>
            </div>
        </div>

        <div class="card card-primary collapsed-card">
              <div class="card-header">
                <h3 class="card-title">History Angsuran Barang</h3>

                <div class="card-tools">
                  <button type="button" class="btn btn-tool" data-card-widget="collapse"><i class="fas fa-plus"></i>
                  </button>
                </div>
                <!-- /.card-tools -->
              </div>
            <!-- Default box -->
  <div class="card-body">
      <div class="card-body">
      <asp:GridView ID="GridView4" runat="server" CssClass="table table-bordered table-hover" HeaderStyle-BackColor="#2E3C59" HeaderStyle-ForeColor ="White"
                                AutoGenerateColumns="false" OnRowDataBound="GridView4_RowDataBound" DataSourceID="GridDataSource4">
                        <Columns>
                            <asp:BoundField DataField="idTrans" HeaderText="KODE" NullDisplayText="-" />
                             <asp:BoundField DataField="NoPlat" HeaderText="NOMOR PLAT" NullDisplayText="-" />
                             <asp:BoundField DataField="Merk" HeaderText="MERK" NullDisplayText="-" />
                             <asp:BoundField DataField="Jenis" HeaderText="JENIS" NullDisplayText="-" />
                             <asp:BoundField DataField="nopinjaman" HeaderText="NOMOR PINJAMAN" NullDisplayText="-" />
                             <asp:BoundField DataField="Angsuran" HeaderText="ANGSURAN" NullDisplayText="-" />
                             <asp:BoundField DataField="Angsuranke" HeaderText="ANGSURAN KE" NullDisplayText="-" />
                             <asp:BoundField DataField="HariTerlambat" HeaderText="TERLAMBAT HARI" NullDisplayText="-" />
                              <asp:BoundField DataField="Denda" HeaderText="DENDA" NullDisplayText="-" />
                              <asp:BoundField DataField="Tanggal" HeaderText="TANGGAL" NullDisplayText="-" />
                             <asp:BoundField DataField="TglJatuhTempo" HeaderText="TANGGAL JATUH TEMPO" NullDisplayText="-" /> 
                        </Columns>
                        <SortedAscendingCellStyle />
                        <SortedAscendingHeaderStyle />
                        <SortedDescendingCellStyle />
                        <SortedDescendingHeaderStyle />
                        <FooterStyle />
                        <PagerStyle HorizontalAlign="Left" />
                     </asp:GridView>
                <asp:SqlDataSource ID="GridDataSource4" runat="server" ConnectionString="<%$ ConnectionStrings:connUQ %>"
        SelectCommand="SELECT NoPinjaman AS NoPinjaman, idTrans as idTrans, IdCabang as IdCabang, Idunit as Idunit, NoPlat as NoPlat, Merk as Merk, Jenis as Jenis,FORMAT(Jumlah,'#,###,##0')  as Angsuran, Angsuranke as Angsuranke , FORMAT(Denda,'#,###,##0')  as Denda , HariTerlambat as HariTerlambat, CONVERT(VARCHAR(10),Tanggal,103) as Tanggal, CONVERT(VARCHAR(10),TglJatuhTempo,103) as TglJatuhTempo from TblAngsuran WHERE idTrans <> '' ORDER BY idTrans"></asp:SqlDataSource>

               </div>
            </div>
        </div>
    </div>
               
     
     

      <asp:HiddenField ID="DirtyBit" runat="server" />
       

<script type="text/javascript" src="plugins/jquery/jquery.min.js"></script> 
<!-- Bootstrap 4 -->
 <script type="text/javascript" src="plugins/bootstrap/js/bootstrap.bundle.min.js"></script> 
<!-- ChartJS -->
<script type="text/javascript" src="plugins/chart.js/Chart.min.js"></script> 
<!-- AdminLTE App -->
<script type="text/javascript" src="dist/js/adminlte.min.js"></script> 
<!-- AdminLTE for demo purposes -->
<script type="text/javascript" src="dist/js/demo.js"></script> 
<!-- Page specific script -->

<!-- Page specific script -->
 
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder2" runat="Server">

<script type="text/javascript">
    function openInNewTab() {
        window.document.forms[0].target = '_blank'; 
        setTimeout(function () { window.document.forms[0].target = ''; }, 0);
    }
</script>

 <script type="text/javascript">
function ResetTarget() {
   window.document.forms[0].target = '';
}
</script>

<script type = "text/javascript">
 function SetTarget() {
     document.forms[0].target = "_blank";
 }
 </script> 
     
</asp:Content>
