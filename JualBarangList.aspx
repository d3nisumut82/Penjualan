<%@ Page Title="Jual Barang List | Quality" Language="VB" MasterPageFile="~/MasterPage.master" AutoEventWireup="false" CodeFile="JualBarangList.aspx.vb" Inherits="JualBarangList" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">

  <style type="text/css">
        .btn-grad {
            color: white !important;
            border: 1px solid white;
            background-color: grey;
            background: -webkit-gradient(linear, left top, left bottom, color-stop(0%, grey), color-stop(100%, grey));
            background: -webkit-linear-gradient(top, grey 0%, grey 100%);
            background: -moz-linear-gradient(top, grey 0%, grey 100%);
            background: -ms-linear-gradient(top, grey 0%, grey 100%);
            background: -o-linear-gradient(top, grey 0%, grey 100%);
            background: linear-gradient(to bottom, grey 0%, grey 100%)
        }

            .btn-grad:hover {
                color: white !important;
                border: 1px solid white;
                background-color: grey;
                background: -webkit-gradient(linear, left top, left bottom, color-stop(0%, grey), color-stop(100%, white));
                background: -webkit-linear-gradient(top, grey 0%, white 100%);
                background: -moz-linear-gradient(top, grey 0%, white 100%);
                background: -ms-linear-gradient(top, grey 0%, white 100%);
                background: -o-linear-gradient(top, grey 0%, white 100%);
                background: linear-gradient(to bottom, grey 0%, white 100%)
            }

        .dataTables_wrapper .dataTables_paginate .paginate_button.current, .dataTables_wrapper .dataTables_paginate .paginate_button.current:hover {
            background-color: #337ab7;
            border-color: #2e6da4;
            background: linear-gradient(to bottom, #337ab7 0%, #337ab7 100%);
            transition: 0.3s all;
        }

        .dataTables_wrapper .dataTables_paginate .paginate_button:hover {
            background-color: #337ab7;
            border-color: #2e6da4;
            background: linear-gradient(to bottom, #337ab7 0%, #337ab7 50%);
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
       <script type="text/javascript">
    if (window.history.replaceState) {
        window.history.replaceState(null, null, window.location.href);
    }
</script>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div class="content-wrapper">
        <!-- Content Header (Page header) -->
        <section class="content-header">
            <div class="container-fluid">
                <div class="row mb-2">
                    <div class="col-sm-6">
                        <h1>Jual Barang List</h1>
                    </div>
                    <div class="col-sm-6">
                        <ol class="breadcrumb float-sm-right">
                            <li class="breadcrumb-item"><a href="EnterpriseHome.aspx">Home</a></li>
                            <li class="breadcrumb-item active">Jual Barang List</li>
                        </ol>
                    </div>
                </div>
            </div>
        </section>

        <!-- Main content -->
        <section class="content">

            <!-- Default box -->
            <div class="card">
                <!-- Card Header -->
                <div class="card-header">
                    <asp:LinkButton ID="btnAddMataKuliah" CssClass="btn btn-primary" ForeColor="white" runat="server"><span class="fas fa-plus"></span><asp:label ID="Label4" text="Add Jual" CssClass="d-none d-sm-inline-block" style="padding-left: 5px;" runat="server" /></asp:LinkButton><div class="card-tools">
                        <button type="button" class="btn btn-tool" data-card-widget="collapse" data-toggle="tooltip" title="Collapse">
                            <i class="fas fa-minus"></i>
                        </button>
                        <button type="button" class="btn btn-tool" data-card-widget="remove" data-toggle="tooltip" title="Remove">
                            <i class="fas fa-times"></i>
                        </button>
                       
                    </div>    
                </div>

                <div class="card-body">
         <%--  <div class="panel panel-primary" style="margin-top:2px !important;margin-left:2px;margin-right:2px;margin-bottom:5px;"> 
                  <asp:Panel ID="Panel1" runat="server" ScrollBars="Both">--%>
                    <asp:GridView ID="GridView1" runat="server" CssClass="table table-bordered table-hover"
                        AutoGenerateColumns="false" OnRowDataBound="GridView1_RowDataBound" DataSourceID="GridDataSource" class="table table-bordered table-hover table-responsive table-striped" EmptyDataText="No Data" GridLines="Both" Font-Names="Arial" Font-Size="XX-Small" AllowSorting="True">
                        <Columns>
                            <asp:TemplateField HeaderText="Kode">
                                <ItemTemplate>
                                    <asp:HyperLink ID="hyperLink1" runat="server" NavigateUrl='<%#String.Format("~/JualBarang.aspx?idTrans={0}", HttpUtility.UrlEncode(Eval("idTrans").ToString())) %>'
                                        Text='<%# Eval("idTrans") %>' />
                                </ItemTemplate>
                            </asp:TemplateField>
                             <asp:BoundField DataField="NoPlat" HeaderText="NOMOR PLAT" NullDisplayText="-" />
                             <asp:BoundField DataField="Merk" HeaderText="MERK" NullDisplayText="-" />
                             <asp:BoundField DataField="Jenis" HeaderText="JENIS" NullDisplayText="-" />
                             <asp:BoundField DataField="JenisPenjualan" HeaderText="JENIS PENJUALAN" NullDisplayText="-" />
                             <asp:BoundField DataField="HargaJual" HeaderText="HARGA JUAL" NullDisplayText="-" />
                             <asp:BoundField DataField="SaldoAwal" HeaderText="SALDO AWAL" NullDisplayText="-" />
                             <asp:BoundField DataField="HargaPokok" HeaderText="HARGA POKOK" NullDisplayText="-" />
                             <asp:BoundField DataField="DP" HeaderText="DP" NullDisplayText="-" />
                             <asp:BoundField DataField="Total" HeaderText="TOTAL" NullDisplayText="-" />
                             <asp:BoundField DataField="Bunga" HeaderText="BUNGA" NullDisplayText="-" />
                             <asp:BoundField DataField="Angsuran" HeaderText="ANGSURAN" NullDisplayText="-" />
                             <asp:BoundField DataField="HargaPokok" HeaderText="HARGA POKOK" NullDisplayText="-" />
                             <asp:BoundField DataField="TglJatuhTempo" HeaderText="TANGGAL JATUH TEMPO" NullDisplayText="-" /> 
                        </Columns>
                        <SortedAscendingCellStyle />
                        <SortedAscendingHeaderStyle />
                        <SortedDescendingCellStyle />
                        <SortedDescendingHeaderStyle />
                        <FooterStyle />
                        <PagerStyle HorizontalAlign="Left" />
                    </asp:GridView>
              <%--  </asp:Panel>
            </div>--%>
                </div>
            </div>
        </section>
    </div>

    <asp:SqlDataSource ID="GridDataSource" runat="server" ConnectionString="<%$ ConnectionStrings:connUQ %>"
        SelectCommand="SELECT FORMAT(Hargajual,'#,###,##0')  as Hargajual, FORMAT(AngsuranLaba,'#,###,##0')  as angsuranbunga, FORMAT(angsuranpokok,'#,###,##0')  as angsuranpokok, FORMAT(MarkupLaba,'#,###,##0')  as MarkupLaba, idTrans as idTrans, IdCabang as IdCabang, Idunit as Idunit, NoPlat as NoPlat, Merk as Merk, Jenis as Jenis, FORMAT(SaldoAwal,'#,###,##0') as SaldoAwal, FORMAT(HargaPokok,'#,###,##0') as HargaPokok, FORMAT(DP,'#,###,##0')  as DP,FORMAT(Total,'#,###,##0')  as Total, FORMAT(SaldoAwal,'#,###,##0')  as SaldoAwal, FORMAT(Bunga,'#,###,##0')  as Bunga, FORMAT(Angsuran,'#,###,##0')  as Angsuran, NoMesin as NoMesin, NoRangka as NoRangka, CONVERT(VARCHAR(10),TglJatuhTempo,103) as TglJatuhTempo,Keterangan AS Keterangan, JenisPenjualan AS JenisPenjualan from TblJual WHERE idTrans <> '' ORDER BY idTrans"></asp:SqlDataSource>

</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder2" runat="Server">
</asp:Content>