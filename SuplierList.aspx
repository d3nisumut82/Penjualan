<%@ Page Title="Supplier List | Quality" Language="VB" MasterPageFile="~/MasterPage.master" AutoEventWireup="false" CodeFile="SuplierList.aspx.vb" Inherits="SuplierList" %>

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
                        <h1>Supplier List</h1>
                    </div>
                    <div class="col-sm-6">
                        <ol class="breadcrumb float-sm-right">
                            <li class="breadcrumb-item"><a href="EnterpriseHome.aspx">Home</a></li>
                            <li class="breadcrumb-item active">Supplier List</li>
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
                    <asp:LinkButton ID="btnAddMataKuliah" CssClass="btn btn-primary" ForeColor="white" runat="server"><span class="fas fa-plus"></span><asp:label ID="Label4" text="Add Supplier" CssClass="d-none d-sm-inline-block" style="padding-left: 5px;" runat="server" /></asp:LinkButton><div class="card-tools">
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
                                    <asp:HyperLink ID="hyperLink1" runat="server" NavigateUrl='<%#String.Format("~/Suplier.aspx?IdSupplier={0}", HttpUtility.UrlEncode(Eval("IdSupplier").ToString())) %>'
                                        Text='<%# Eval("IdSupplier") %>' />
                                </ItemTemplate>
                            </asp:TemplateField>
                             <asp:BoundField DataField="NAMA" HeaderText="NAMA" NullDisplayText="-" />
                             <asp:BoundField DataField="ALAMAT" HeaderText="ALAMAT" NullDisplayText="-" />
                             <asp:BoundField DataField="TEMPATLAHIR" HeaderText="TEMPAT LAHIR" NullDisplayText="-" />
                             <asp:BoundField DataField="TGLLAHIR" HeaderText="TGL LAHIR" NullDisplayText="-" />
                             <asp:BoundField DataField="IDKTP" HeaderText="ID KTP" NullDisplayText="-" />
                             <asp:BoundField DataField="NoKK" HeaderText="NO KK" NullDisplayText="-" />
                             <asp:BoundField DataField="Tgldaftar" HeaderText="TGL DAFTAR" NullDisplayText="-" />
                             <asp:BoundField DataField="Domisili" HeaderText="DOMISILI" NullDisplayText="-" />
                             <asp:BoundField DataField="HP" HeaderText="HAND PHONE" NullDisplayText="-" />
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
        SelectCommand="SELECT IdSupplier as IdSupplier, IdCabang as IdCabang, IdUnit as IdUnit, Nama as Nama, Alamat as Alamat, TempatLahir as TempatLahir, CONVERT(VARCHAR(10),TGLLAHIR,103) AS TGLLAHIR, IdKTP as IdKTP, NoKK as NoKK, CONVERT(VARCHAR(10),TglDaftar,103)  AS TglDaftar, Domisili as Domisili, HP as HP  from TblSuplier WHERE IdSupplier <> '' ORDER BY IdSupplier"></asp:SqlDataSource>

</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder2" runat="Server">
</asp:Content>