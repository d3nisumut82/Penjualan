<%@ Page Title="Bayar Denda" Language="VB" MasterPageFile="~/MasterPage.master" AutoEventWireup="false" CodeFile="Dendareal.aspx.vb" Inherits="Dendareal"%>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">

</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <div class="content-wrapper">
        <!-- Content Header (Page header) -->
        <section class="content-header">
            <div class="container-fluid">
                <div class="row mb-2">
                    <div class="col-sm-6">
                        <h1>DATA DENDA</h1>
                    </div>
                    <div class="col-sm-6">
                        <ol class="breadcrumb float-sm-right">
                            <li class="breadcrumb-item"><a href="EnterpriseHome.Aspx">Home</a></li>
                            <li class="breadcrumb-item active">DATA DENDA</li>
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
                    <asp:LinkButton ID="btnNew2" CssClass="btn btn-success" ForeColor="white" runat="server"><span class="fas fa-file"></span><asp:label ID="Label2" text="New" CssClass="d-none d-sm-inline-block" style="padding-left: 8px;" runat="server" /></asp:LinkButton>                      
                    <asp:LinkButton ID="btnNew" CssClass="btn btn-success" ForeColor="white" runat="server"><span class="fas fa-file"></span><asp:label ID="Label1" text="Back" CssClass="d-none d-sm-inline-block" style="padding-left: 8px;" runat="server" /></asp:LinkButton>                      
                    <asp:LinkButton ID="lnkSave" CssClass="btn btn-primary" ForeColor="white" runat="server" OnClick="lnkSave_Click" OnClientClick="return ShowSaveConfirm(this);"><span class="fas fa-save"></span><asp:label ID="Label3" text="Save" CssClass="d-none d-sm-inline-block" style="padding-left: 8px;" runat="server" /></asp:LinkButton>
                    <asp:LinkButton ID="lnkDelete" CssClass="btn btn-danger" ForeColor="white" runat="server" OnClick="lnkDelete_Click" OnClientClick="return ShowDeleteConfirm(this);"><span class="fas fa-trash"></span><asp:label ID="Label4" text="Delete" CssClass="d-none d-sm-inline-block" style="padding-left: 8px;" runat="server" /></asp:LinkButton>
                </div>
                <!-- Card Header -->
                <!-- Card Body -->
                <div class="card-body"> 
                
                       <div class="row">
                        <div class="col-sm-12"> 
                         <div style="height:13px;"></div>  
                 <div class="row">        
                    <div class="col-sm-6">
                      <!-- text input -->
                      <div class="form-group">
                        <label class="text-center">ID TRANS</label>
                        <asp:TextBox ID="txtidtrans" runat="server" class="form-control input-sm" autocomplete="off" />
                      </div>
                    </div>
                    <div class="col-sm-6">
                      <div class="form-group">
                        <label >ID NASABAH</label> 
                         <asp:TextBox ID="txtidnasabah" runat="server" class="form-control input-sm" placeholder="Ketikan Mata Nama..."></asp:TextBox>
                      </div>
                    </div>
                   </div> 
                        <div class="row">        
                            <label class="col-sm-4">NAMA NASABAH</label>
                             <asp:TextBox ID="txtnama" runat="server" class="form-control input-sm" placeholder="Ketikan Alamat..."></asp:TextBox>
                         </div>
                             
                      <div class="row">        
                        <div class="col-sm-6">
                            <div class="form-group">
                                <label class="text-center">ID CABANG </label> 
                                <asp:TextBox ID="ddlcabang" runat="server" class="form-control input-sm" placeholder="Ketikan Id Cabang ..."></asp:TextBox>                   
                             </div>
                        </div>
                    <div class="col-sm-6">
                      <div class="form-group">
                        <label >ID UNIT </label> 
                               <asp:TextBox ID="ddlunit" runat="server" class="form-control input-sm" placeholder="Ketikan Id Unit ..."></asp:TextBox>    
                       </div>
                    </div>
                   </div> 
                          <div class="row">        
                            <label class="col-sm-4">WIPEM</label>
                             <asp:TextBox ID="txtwipem" runat="server" class="form-control input-sm" placeholder="Ketikan Alamat..."></asp:TextBox>
                         </div> 
                         <div class="row">        
                            <label class="col-sm-4">NOMOR PINJAMAN</label>
                             <asp:TextBox ID="txtnopinjaman" runat="server" class="form-control input-sm" placeholder="Ketikan Alamat..."></asp:TextBox>
                         </div>

                      <div class="row">        
                    <div class="col-sm-6">
                      <!-- text input -->
                      <div class="form-group">
                        <label class="text-center">JUMLAH </label>
                        <asp:TextBox ID="txtjumlah" runat="server" class="form-control input-sm" autocomplete="off" />
                      </div>
                    </div>
                    <div class="col-sm-6">
                      <div class="form-group">
                        <label >TANGGAL </label> 
                           <div class="input-group">
                                                <input id="dtpTgl" runat="server" autocomplete="off" type="text" class="form-control datepicker" /> <div class="input-group-append">
                                                      <div class="input-group-text"><i class="fa fa-calendar"></i></div>
                                                   </div>
                                            </div>
                      </div>
                    </div>
                   </div> 

                   
                                </div>
                        </div>
          
    
                <hr />

                    <%--<button type="button" class="btn btn-primary btn-sm" data-toggle="modal" data-target="#addmodalBank"><span data-toggle="tooltip" title="Add" class="glyphicon glyphicon-plus"></span>Tambah Sub Account</button>
                    
                    <br /><br />--%>
     <!-- Search Button Kampus --><div class="modal fade" id="myKampus">
        <div class="modal-dialog modal-xl">
            <div class="modal-content">
                <div class="modal-header" style="cursor: move;">
                    <h4 class="modal-title">ID.Kampus</h4><button type="button" class="close text-black" data-dismiss="modal" aria-label="Close">
                        <span aria-hidden="true">&times;</span></button></div><div class="modal-body">
                    <div class="input-group col-lg-4 mb-3">
                        <input id="txtSearchKampus" runat="server" type="text" style="text-transform: capitalize;" class="form-control" placeholder="Search" /> <div class="input-group-append">
                            <div class="input-group-text">
                                <span class="fas fa-search"></span>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="table-bordered table-responsive p-0 hide-scrollbar" style="max-height: 60vh;">
                    <table id="tblKampus" runat="server" class="table table-hover table-head-fixed text-nowrap" style="cursor: pointer;">
                        <thead>
                            <tr>
                                <td class="bg-primary">ID.UNIT</td><td class="bg-primary">Lokasi Kampus</td></tr></thead></table></div></div></div></div>



                    
                    <div id="divDisplay" runat="server" class="panel panel-primary" style="margin-top:2px !important;margin-left:2px;margin-right:2px;margin-bottom:2px;display:none;" > 
                        <asp:Panel ID="lineItem" runat="server" ScrollBars="Vertical"  Height="435px" ><asp:PlaceHolder id="placeHolderWisuda" runat="server"></asp:PlaceHolder> </asp:Panel> 
                    </div>

                </div>
                <!-- Card Body -->
            </div>
        </section>
          <section class="content-header">
            <div class="container-fluid">
                <div class="row mb-2">
                    <div class="col-sm-6">
                            <asp:LinkButton ID="Lngenerate" CssClass="btn btn-success" ForeColor="white" runat="server"><span class="fas fa-file"></span><asp:label ID="Label6" text="Hitung Denda" CssClass="d-none d-sm-inline-block" style="padding-left: 8px;" runat="server" /></asp:LinkButton>  
                    </div>
                   
                </div>
             </div>   
        </section>
           <div class="card">
          <div class="card-body"> 
                          <div class="row">        
                            <label class="col-sm-4">HARI KETERLAMBATAN</label>
                                         <asp:TextBox ID="txthari" runat="server" class="form-control input-sm" ></asp:TextBox>
                         </div> 
                      <div class="row">        
                            <label class="col-sm-4">JUMLAH DENDA</label>
                                  <asp:TextBox ID="txtjumlahdenda" runat="server" class="form-control input-sm" ></asp:TextBox>
                         </div> 
                    
                <div class="row">     
                     <div class="col-sm-6">
                      <!-- text input -->
                      <div class="form-group">
                        <label class="text-center">REALISASI DENDA </label>
                              <asp:TextBox ID="txtreal" runat="server" class="form-control input-sm" placeholder="Realisasi Denda ..."></asp:TextBox>
                      </div>
                    </div>
                    <div class="col-sm-6">
                      <div class="form-group">
                         <div class="form-group">
                        <label class="text-center">SALDO AKHIR DENDA </label>
                              <asp:TextBox ID="txtsaldoakhir" runat="server" class="form-control input-sm" placeholder="Saldo akhir Denda ..."></asp:TextBox>
                      </div>
                      </div>
                    </div>
                   </div> 
              </div>
         </div>

    </div>

    

</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder2" Runat="Server">
   <script type="text/javascript">
    $(document).ready(function () {
        $("#<%=txtidnasabah.ClientID %>").autocomplete({
            source: function (request, response) {
                $.ajax({
                    url: '<%=ResolveUrl("wsEnterprise.asmx/GetDendaNasabah") %>',
                    data: "{ 'input': '" + request.term + "'}",
                    dataType: "json",
                    type: "POST",
                    contentType: "application/json; charset=utf-8",
                    success: function (data) {
                        response($.map(data.d, function (item) {
                            return {
                                nopinjamannya: item.nopinjamanGetDenda,
                                idcabangnya: item.idCabangGetDenda,
                                idunitnya: item.idUnitGetDenda,
                                idnasabahya: item.idnasabahGetDenda,
                                namanasabahya: item.namanasabahGetDenda,
                                wipemnya: item.WipemGetDenda
                            }
                        }))
                    }
                });
            },
            minLength: 1,
            select: function (event, ui) {
                $("#<%=txtnopinjaman.ClientID %>").val(ui.item.nopinjamannya);
                $("#<%=ddlcabang.ClientID %>").val(ui.item.idcabangnya);
                $("#<%=ddlunit.ClientID %>").val(ui.item.idunitnya);
                $("#<%=txtidnasabah.ClientID %>").val(ui.item.idnasabahya);
                $("#<%=txtnama.ClientID %>").val(ui.item.namanasabahya);
                $("#<%=txtwipem.ClientID %>").val(ui.item.wipemnya);  
                return false;
            }
        })
                    .autocomplete("instance")._renderItem = function (ul, item) {
                        return $("<li>")
                    .append("<p class='style5'>" + item.namanasabahya + " - " + item.nopinjamannya + " </p>")
                    .appendTo(ul);
                    };
    });
    </script>
</asp:Content>

