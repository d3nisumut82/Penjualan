<%@ Page Title="Barang List | Quality" Language="VB" MasterPageFile="~/MasterPage.master" AutoEventWireup="false" CodeFile="BarangList.aspx.vb" Inherits="BarangList" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
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
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div class="content-wrapper">
        <!-- Content Header (Page header) -->
        <section class="content-header">
            <div class="container-fluid">
                <div class="row mb-2">
                    <div class="col-sm-6">
                        <h1>Barang List</h1>
                    </div>
                    <div class="col-sm-6">
                        <ol class="breadcrumb float-sm-right">
                            <li class="breadcrumb-item"><a href="EnterpriseHome.aspx">Home</a></li>
                            <li class="breadcrumb-item active">Barang List</li>
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
                    <asp:LinkButton ID="btnAddMataKuliah" CssClass="btn btn-primary" ForeColor="white" runat="server"><span class="fas fa-plus"></span><asp:label ID="Label4" text="Add Barang" CssClass="d-none d-sm-inline-block" style="padding-left: 5px;" runat="server" /></asp:LinkButton><div class="card-tools">
                        <button type="button" class="btn btn-tool" data-card-widget="collapse" data-toggle="tooltip" title="Collapse">
                            <i class="fas fa-minus"></i>
                        </button>
                        <button type="button" class="btn btn-tool" data-card-widget="remove" data-toggle="tooltip" title="Remove">
                            <i class="fas fa-times"></i>
                        </button>
                       
                    </div>    
                </div>

                <div class="card-body">
                    <asp:GridView ID="GridView1" runat="server" CssClass="table table-bordered table-hover"
                        AutoGenerateColumns="false" OnRowDataBound="GridView1_RowDataBound" DataSourceID="GridDataSource">
                        <Columns>
                            <asp:TemplateField HeaderText="Kode">
                                <ItemTemplate>
                                    <asp:HyperLink ID="hyperLink1" runat="server" NavigateUrl='<%#String.Format("~/Barang.aspx?IdBarang={0}", HttpUtility.UrlEncode(Eval("IdBarang").ToString())) %>'
                                        Text='<%# Eval("IdBarang") %>' />
                                </ItemTemplate>
                            </asp:TemplateField>
                             <asp:BoundField DataField="NoPlat" HeaderText="NOMOR PLAT" NullDisplayText="-" />
                             <asp:BoundField DataField="NamaPemilik" HeaderText="NAMA PEMILIK" NullDisplayText="-" />
                             <asp:BoundField DataField="Alamat" HeaderText="ALAMAT" NullDisplayText="-" />
                             <asp:BoundField DataField="Merk" HeaderText="MERK" NullDisplayText="-" />
                             <asp:BoundField DataField="Type" HeaderText="TYPE" NullDisplayText="-" />
                             <asp:BoundField DataField="Jenis" HeaderText="JENIS" NullDisplayText="-" />
                             <asp:BoundField DataField="Model" HeaderText="MODEL" NullDisplayText="-" />
                             <asp:BoundField DataField="TahunPembuatan" HeaderText="TAHUN PEMBUATAN" NullDisplayText="-" />
                             <asp:BoundField DataField="CCSilinder" HeaderText="CC SILINDER" NullDisplayText="-" />
                             <asp:BoundField DataField="NoMesin" HeaderText="NOMOR MESIN" NullDisplayText="-" />
                             <asp:BoundField DataField="NoRangka" HeaderText="NOMOR RANGKA" NullDisplayText="-" />
                             <asp:BoundField DataField="NoBPKB" HeaderText="NOMOR BPKB" NullDisplayText="-" />
                             <asp:BoundField DataField="NoSTNK" HeaderText="NOMOR STNK" NullDisplayText="-" />
                             <asp:BoundField DataField="TglPajak" HeaderText="TANGGAL PAJAK" NullDisplayText="-" />
                        </Columns>
                        <SortedAscendingCellStyle />
                        <SortedAscendingHeaderStyle />
                        <SortedDescendingCellStyle />
                        <SortedDescendingHeaderStyle />
                        <FooterStyle />
                        <PagerStyle HorizontalAlign="Left" />
                    </asp:GridView>
                </div>
            </div>
        </section>
    </div>

    <asp:SqlDataSource ID="GridDataSource" runat="server" ConnectionString="<%$ ConnectionStrings:connUQ %>"
        SelectCommand="SELECT IdBarang as IdBarang, IdCabang as IdCabang, Idunit as Idunit, NoPlat as NoPlat, Alamat as Alamat, NamaPemilik as NamaPemilik, Merk as Merk, Type as Type, Jenis as Jenis, Model as Model, TahunPembuatan as TahunPembuatan, CCSilinder as CCSilinder, NoMesin as NoMesin, NoRangka as NoRangka, NoBPKB as NoBPKB, NoSTNK as NoSTNK, CONVERT(VARCHAR(10),TglPajak,103) as TglPajak    from TblBarang WHERE IdBarang <> '' ORDER BY IdBarang"></asp:SqlDataSource>

</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder2" runat="Server">
</asp:Content>