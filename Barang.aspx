<%@ Page Title="Barang" Language="VB" MasterPageFile="~/MasterPage.master" AutoEventWireup="false" CodeFile="Barang.aspx.vb" Inherits="Barang"%>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">

</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <div class="content-wrapper">
        <!-- Content Header (Page header) -->
        <section class="content-header">
            <div class="container-fluid">
                <div class="row mb-2">
                    <div class="col-sm-6">
                        <h1>DATA BARANG</h1>
                    </div>
                    <div class="col-sm-6">
                        <ol class="breadcrumb float-sm-right">
                            <li class="breadcrumb-item"><a href="EnterpriseHome.Aspx">Home</a></li>
                            <li class="breadcrumb-item active">DATA BARANG</li>
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
                        <label class="text-center">ID BARANG</label>
                        <asp:TextBox ID="txtidbarang" runat="server" class="form-control input-sm" autocomplete="off" />
                      </div>
                    </div>
                    <div class="col-sm-6">
                      <div class="form-group">
                        <label >NOMOR PLAT</label> 
                         <asp:TextBox ID="txtplat" runat="server" class="form-control input-sm" placeholder="Ketikan Mata Nama..."></asp:TextBox>
                      </div>
                    </div>
                   </div> 
                         <div class="row">        
                            <label class="col-sm-4">ID SUPPLIER</label>
                             <asp:TextBox ID="txtidsupplier" runat="server" class="form-control input-sm" placeholder="Ketikan ID ATAU NAMA SUPPLIER..."></asp:TextBox>
                         </div> 
                        <div class="row">        
                            <label class="col-sm-4">NAMA PEMILIK</label>
                             <asp:TextBox ID="txtnama" runat="server" class="form-control input-sm" placeholder="Ketikan Alamat..."></asp:TextBox>
                         </div> 
                          <div class="row">        
                            <label class="col-sm-4">ALAMAT</label>
                             <asp:TextBox ID="txtalamat" runat="server" class="form-control input-sm" placeholder="Ketikan Alamat..."></asp:TextBox>
                         </div> 
                      <div class="row">        
                    <div class="col-sm-6">
                      <!-- text input -->
                      <div class="form-group">
                        <label class="text-center">MERK </label>
                        <asp:TextBox ID="txtmerk" runat="server" class="form-control input-sm" autocomplete="off" />
                      </div>
                    </div>
                    <div class="col-sm-6">
                      <div class="form-group">
                        <label >TYPE </label> 
                           <asp:TextBox ID="txttype" runat="server" class="form-control input-sm" autocomplete="off" />
                      </div>
                    </div>
                   </div> 
                   
                    <div class="row">     
                     <div class="col-sm-6">
                      <!-- text input -->
                      <div class="form-group">
                        <label class="text-center">JENIS </label>
                        <asp:TextBox ID="txtjenis" runat="server" class="form-control input-sm" autocomplete="off" />
                      </div>
                    </div>
                    <div class="col-sm-6">
                      <div class="form-group">
                        <label >MODEL </label> 
                         <asp:TextBox ID="txtmodel" runat="server" class="form-control input-sm" placeholder="Ketikan Model ..."></asp:TextBox>
                      </div>
                    </div>
                   </div>
                
                 <div class="row">     
                     <div class="col-sm-6">
                      <!-- text input -->
                      <div class="form-group">
                        <label class="text-center">TAHUN PEMBUATAN </label>
                                       <asp:TextBox ID="txttahun" runat="server" class="form-control input-sm" placeholder="Ketikan Tahun Pembuatan ..."></asp:TextBox>
                      </div>
                    </div>
                    <div class="col-sm-6">
                      <div class="form-group">
                        <label >CC Silinder </label> 
                         <asp:TextBox ID="txtsilinder" runat="server" class="form-control input-sm" placeholder="Ketikan isi Silinder ..."></asp:TextBox>
                      </div>
                    </div>
                   </div> 

                    <div class="row">     
                     <div class="col-sm-6">
                      <!-- text input -->
                      <div class="form-group">
                        <label class="text-center">NOMOR MESIN </label>
                               <asp:TextBox ID="txtnomesin" runat="server" class="form-control input-sm" placeholder="Ketikan Nomor Mesin ..."></asp:TextBox>
                      </div>
                    </div>
                    <div class="col-sm-6">
                      <div class="form-group">
                        <label >NOMOR RANGKA </label> 
                         <asp:TextBox ID="txtnorangka" runat="server" class="form-control input-sm" placeholder="Ketikan Nomor Rangka ..."></asp:TextBox>
                      </div>
                    </div>
                   </div> 
                
                <div class="row">     
                     <div class="col-sm-6">
                      <!-- text input -->
                      <div class="form-group">
                        <label class="text-center">NOMOR BPKB </label>
                              <asp:TextBox ID="txtbpkb" runat="server" class="form-control input-sm" placeholder="NOMOR BPKB ..."></asp:TextBox>
                      </div>
                    </div>
                    <div class="col-sm-6">
                      <div class="form-group">
                        <label >NOMOR STNK </label> 
                         <asp:TextBox ID="txtnostnk" runat="server" class="form-control input-sm" placeholder="Ketikan Mata Kuliah ..."></asp:TextBox>
                      </div>
                    </div>
                   </div> 

                      <div class="row">        
                            <label class="col-sm-4">TANGGAL PAJAK</label>
                                  <div class="input-group">
                                                <input id="dtglpajak" runat="server" autocomplete="off" type="text" class="form-control datepicker" /> <div class="input-group-append">
                                                      <div class="input-group-text"><i class="fa fa-calendar"></i></div>
                                                   </div>
                                            </div>
                         </div> 

                      <div class="row">        
                        <div class="col-sm-6">
                            <!-- text input -->
                            <div class="form-group">
                                <label class="text-center">ID CABANG </label>
                                 <%--  <asp:DropDownList ID="ddlcabang" CssClass="form-control" EnableViewState="true" runat="server"  />  --%>   
                                <asp:TextBox ID="ddlcabang" runat="server" class="form-control input-sm" placeholder="Ketikan Id Cabang ..."></asp:TextBox>                   
                             </div>
                        </div>
                    <div class="col-sm-6">
                      <div class="form-group">
                        <label >ID UNIT </label> 
                               <asp:TextBox ID="ddlunit" runat="server" class="form-control input-sm" placeholder="Ketikan Id Unit ..."></asp:TextBox>    
                          <%-- <asp:DropDownList ID="ddlunit" CssClass="form-control" EnableViewState="true" runat="server"  />
  --%>                    </div>
                    </div>
                   </div> 

                                </div></div>
          
    
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

         <div style="height:20px;"></div>
         <section class="content"> 
            <!-- Default box -->
            <div class="card">

    <div class="main">
            <h2>
                Upload Dokumen 
            </h2>  
            <hr />
            <asp:FileUpload  ID="FileUploadDok"  runat="server" />  
            <asp:Button ID="btnUpload" runat="server" Text="Upload" Onclick="UploadBarang" />  
            <hr />  
            <asp:GridView ID="gvDokumen" runat="server" HeaderStyle-BackColor="#3AC0F2" HeaderStyle-ForeColor="White"  
                    RowStyle-BackColor="#A1DCF2" AlternatingRowStyle-BackColor="White" AlternatingRowStyle-ForeColor="#000"  
                    AutoGenerateColumns="false">  
                 <Columns>  
                        <asp:BoundField DataField="Name" HeaderText="File Name" />  
                        <asp:BoundField DataField="LastUpdate" HeaderText="Last Update" />  
                        <asp:TemplateField ItemStyle-HorizontalAlign="Center">  
                            <ItemTemplate>  
                                <asp:LinkButton ID="lnkDownload" runat="server" Text="Download" OnClick="DownloadFile"  
                                    CommandArgument='<%# Eval("Name") %>'></asp:LinkButton>  
                            </ItemTemplate>  
                        </asp:TemplateField>  
                        <asp:TemplateField ItemStyle-HorizontalAlign="Center">  
                            <ItemTemplate>  
                                <asp:LinkButton ID="lnkDelete" runat="server" Text="Delete" OnClick="DeleteMyFileBarang"  
                                    CommandArgument='<%# Eval("Name") %>'></asp:LinkButton>  
                            </ItemTemplate>  
                        </asp:TemplateField>  
                    </Columns>  
             </asp:GridView>  
        </div>
    </div>
</section>              
    </div>

    

</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder2" Runat="Server">

<script type="text/javascript">
    $(document).ready(function () {
        $("#<%=ddlcabang.ClientID %>").autocomplete({
            source: function (request, response) {
                $.ajax({
                    url: '<%=ResolveUrl("wsEnterprise.asmx/GetMkKonversi2") %>',
                    data: "{ 'input': '" + request.term + "'}",
                    dataType: "json",
                    type: "POST",
                    contentType: "application/json; charset=utf-8",
                    success: function (data) {
                        response($.map(data.d, function (item) {
                            return {
                                kodenya: item.kodeGetMkKonversi2,
                                namanya: item.namaGetMkKonversi2,
                                sksnya: item.sksGetMkKonversi2
                            }
                        }))
                    }
                });
            },
            minLength: 1,
            select: function (event, ui) {
                $("#<%=ddlcabang.ClientID %>").val(ui.item.kodenya);
               
                return false;
            }
        })
                    .autocomplete("instance")._renderItem = function (ul, item) {
                        return $("<li>")
                    .append("<p class='style5'>" + item.kodenya + " - " + item.namanya + " </p>")
                    .appendTo(ul);
                    };
    });
    </script>

<script type="text/javascript">
    $(document).ready(function () {
        $("#<%=ddlunit.ClientID %>").autocomplete({
            source: function (request, response) {
                $.ajax({
                    url: '<%=ResolveUrl("wsEnterprise.asmx/GetMkKonversiunit") %>',
                    data: "{ 'input': '" + request.term + "'}",
                    dataType: "json",
                    type: "POST",
                    contentType: "application/json; charset=utf-8",
                    success: function (data) {
                        response($.map(data.d, function (item) {
                            return {
                                kodenya: item.kodeGetMkKonversi3,
                                namanya: item.namaGetMkKonversi3,
                              
                            }
                        }))
                    }
                });
            },
            minLength: 1,
            select: function (event, ui) {
                $("#<%=ddlunit.ClientID %>").val(ui.item.kodenya);
               
                return false;
            }
        })
                    .autocomplete("instance")._renderItem = function (ul, item) {
                        return $("<li>")
                    .append("<p class='style5'>" + item.kodenya + " - " + item.namanya + " </p>")
                    .appendTo(ul);
                    };
    });
    </script>

<script type="text/javascript">
    $(document).ready(function () {
        $("#<%=txtidsupplier.ClientID %>").autocomplete({
            source: function (request, response) {
                $.ajax({
                    url: '<%=ResolveUrl("wsEnterprise.asmx/GetSupplier2") %>',
                    data: "{ 'input': '" + request.term + "'}",
                    dataType: "json",
                    type: "POST",
                    contentType: "application/json; charset=utf-8",
                    success: function (data) {
                        response($.map(data.d, function (item) {
                            return {
                                idsuppliernya: item.kodeGetSupplier2,
                                namanya: item.namaGetSupplier2,
                                alamatnya: item.alamatGetSupplier2
                            }
                        }))
                    }
                });
            },
            minLength: 1,
            select: function (event, ui) {
                $("#<%=txtidsupplier.ClientID %>").val(ui.item.idsuppliernya);
               $("#<%=txtnama.ClientID %>").val(ui.item.namanya);
                $("#<%=txtalamat.ClientID %>").val(ui.item.alamatnya);
                return false;
            }
        })
                    .autocomplete("instance")._renderItem = function (ul, item) {
                        return $("<li>")
                    .append("<p class='style5'>" + item.idsuppliernya + " - " + item.namanya + " </p>")
                    .appendTo(ul);
                    };
    });
    </script>

</asp:Content>

