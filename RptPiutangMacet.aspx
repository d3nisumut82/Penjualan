<%@ Page Title="Report Piutang Macet" Language="VB" MasterPageFile="~/MasterPage.master" AutoEventWireup="false" CodeFile="RptPiutangMacet.aspx.vb" Inherits="RptPiutangMacet" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    <script type="text/javascript">
    if (window.history.replaceState) {
        window.history.replaceState(null, null, window.location.href);
    }
</script>



    <script type="text/javascript">
        //Kampus
        $(document).ready(function () {
            $.ajax({
                type: "POST",
                contentType: "application/json; charset=utf-8",
                url: "methodBind.aspx/BindDataCabang",
                data: "",
                dataType: "json",
                success: function (data) {
                    //alert(data.d.length);
                    for (var i = 0; i < data.d.length; i++) {
                        $("#<%=tblCabang.ClientID%>").append("<tr><td id='kolom1' onclick='javascript:putvalueCabang1(this);'>" + data.d[i].IdCabang + "</td><td onclick='javascript:putvalueCabang2(this);'>" + data.d[i].lokasiCabang + "</td></tr>");
                    }
                },
                error: function (response) {
                    alert("Error");
                }
            });
        });

    </script>
 
     
<script type="text/javascript">
        //Unit
        $(document).ready(function () {
            $.ajax({
                type: "POST",
                contentType: "application/json; charset=utf-8",
                url: "methodBind.aspx/BindDataUnit",
                data: "",
                dataType: "json",
                success: function (data) {
                    //alert(data.d.length);
                    for (var i = 0; i < data.d.length; i++) {
                        $("#<%=Tblunit.ClientID%>").append("<tr><td id='kolom1' onclick='javascript:putvalueUnit1(this);'>" + data.d[i].IdUnit + "</td><td onclick='javascript:putvalueUnit2(this);'>" + data.d[i].lokasiUnit + "</td></tr>");
                    }
                },
                error: function (response) {
                    alert("Error");
                }
            });
        });

    </script>

   <%-- <script type="text/javascript">
        //Wipem
        $(document).ready(function () {
            $.ajax({
                type: "POST",
                contentType: "application/json; charset=utf-8",
                url: "methodBind.aspx/BindDataWipem",
                data: "",
                dataType: "json",
                success: function (data) {
                    //alert(data.d.length);
                    for (var i = 0; i < data.d.length; i++) {
                        $("#<%=TblWipem.ClientID%>").append("<tr><td id='kolom1' onclick='javascript:putvalueWipem1(this);'>" + data.d[i].IdWipem + "</td><td onclick='javascript:putvalueUnit2(this);'>" + data.d[i].lokasiWipem + "</td></tr>");
                    }
                },
                error: function (response) {
                    alert("Error");
                }
            });
        });

    </script>--%>


<style type="text/css">
   .HiddenCol{display:none;}                
</style>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <div class="content-wrapper">
        <!-- Content Header (Page header) -->
        <section class="content-header">
            <div class="container-fluid">
                <div class="row mb-2">
                    <div class="col-sm-6">
                        <h1>NASABAH MACET</h1>
                    </div>
                </div>
            </div>
        </section>
        <!-- Main content -->
        <section class="content"> 
        <div class="card">
                <!-- Card Header -->
            <div class="card-header">
                <asp:LinkButton ID="lnkPrintJadwal" CssClass="btn btn-info" ForeColor="white" runat="server" ><span class="fas fa-print"></span><asp:Label ID="Label4" Text="PRINT" CssClass="d-none d-sm-inline-block" Style="padding-left: 8px;" runat="server" /></asp:LinkButton>
        </div>
           <div class="card">
                <!-- Card Header -->
                    <div class="card-header">
            <!-- Default box -->
                <div class="card-body"> 
                     <div style="height:7px;"></div>
                      <div class="row">
                        <div class="col-sm-2">
                            <label>ID CABANG</label> </div><div class="col-sm-3">
                            <div class="input-group">
                                <div class="input-group-prepend">
                                    <button type="button" class="btn btn-default" data-toggle="modal" data-target="#myCabang"><span class="fas fa-search"></span></button>
                                </div>
                                <asp:TextBox ID="txtidcabang" runat="server" class="form-control input-sm" autocomplete="off" />
                            </div>
                        </div>
                          <div class="col-sm-1">
                            <label>NAMA </label> </div><div class="col-sm-2">
                            <asp:TextBox ID="txtnamacabang" runat="server" class="form-control input-sm" />
                        </div>
                    </div>
                    
                         <div style="height:7px;"></div>
                       <div class="row">
                          <label class="col-sm-2">ID UNIT</label> <div class="col-sm-3">
                             <div class="input-group">
                                 <div class="input-group-prepend">
                                    <button type="button" class="btn btn-default" data-toggle="modal" data-target="#myUnit"><span class="fas fa-search"></span></button>
                                  </div>
                                    <asp:TextBox ID="txtidunit" runat="server" class="form-control input-sm" autocomplete="off" />
                                  </div> 
                              </div>
                             <div class="col-sm-1">
                            <label>NAMA </label> </div><div class="col-sm-2">
                            <asp:TextBox ID="txtnamaUnit" runat="server" class="form-control input-sm" />
                        </div>
                    </div> 
                         
                    <%-- <div style="height:7px;"></div>
                       <div class="row">
                          <label class="col-sm-2">ID WIPEM</label> <div class="col-sm-3">
                             <div class="input-group">
                                 <div class="input-group-prepend">
                                    <button type="button" class="btn btn-default" data-toggle="modal" data-target="#myWipem"><span class="fas fa-search"></span></button>
                                  </div>
                                    <asp:TextBox ID="txtidwipem" runat="server" class="form-control input-sm" autocomplete="off" />
                                  </div> 
                              </div>
                             <div class="col-sm-1">
                            <label>NAMA </label> </div><div class="col-sm-2">
                            <asp:TextBox ID="txtnamawipem" runat="server" class="form-control input-sm" />
                        </div>
                    </div> --%>

                      <div style="height:7px;"></div>
                                    <div class="row">
                                        <div class="col-sm-2">
                                            <label style="height:33px;">TANGGAL JATUH TEMPO </label> </div><div class="col-sm-3">
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
                 <asp:Panel ID="lineItem" runat="server" ScrollBars="Vertical" Height="435px">
                            <div id="divDisplay" runat="server" class="panel panel-primary" style="margin-top: 2px !important; margin-left: 2px; margin-right: 2px; margin-bottom: 2px; display: none;"> 
                                <asp:PlaceHolder ID="placeHolderJadwal" runat="server"></asp:PlaceHolder>
                            </div>
                        </asp:Panel>                    
         <hr />
     
      <!-- Search Button Cabang --><div class="modal fade" id="myCabang">
        <div class="modal-dialog modal-xl">
            <div class="modal-content">
                <div class="modal-header" style="cursor: move;">
                    <h4 class="modal-title">ID.Cabang</h4><button type="button" class="close text-black" data-dismiss="modal" aria-label="Close">
                        <span aria-hidden="true">&times;</span></button></div><div class="modal-body">
                    <div class="input-group col-lg-4 mb-3">
                        <input id="txtSearchCabang" runat="server" type="text" style="text-transform: capitalize;" class="form-control" placeholder="Search" /> <div class="input-group-append">
                            <div class="input-group-text">
                                <span class="fas fa-search"></span>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="table-bordered table-responsive p-0 hide-scrollbar" style="max-height: 60vh;">
                    <table id="tblCabang" runat="server" class="table table-hover table-head-fixed text-nowrap" style="cursor: pointer;">
                        <thead>
                            <tr>
                                <td class="bg-primary">ID.Cabang</td><td class="bg-primary">Lokasi Cabang</td></tr></thead></table></div></div></div></div>

 <!-- Search Button Unit --><div class="modal fade" id="myUnit">
        <div class="modal-dialog modal-xl">
            <div class="modal-content">
                <div class="modal-header" style="cursor: move;">
                    <h4 class="modal-title">ID.Unit</h4><button type="button" class="close text-black" data-dismiss="modal" aria-label="Close">
                        <span aria-hidden="true">&times;</span></button></div><div class="modal-body">
                    <div class="input-group col-lg-4 mb-3">
                        <input id="txtSearchUnit" runat="server" type="text" style="text-transform: capitalize;" class="form-control" placeholder="Search" /> <div class="input-group-append">
                            <div class="input-group-text">
                                <span class="fas fa-search"></span>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="table-bordered table-responsive p-0 hide-scrollbar" style="max-height: 60vh;">
                    <table id="Tblunit" runat="server" class="table table-hover table-head-fixed text-nowrap" style="cursor: pointer;">
                        <thead>
                            <tr>
                                <td class="bg-primary">ID.Unit</td><td class="bg-primary">Lokasi Unit</td></tr></thead></table></div></div></div></div>

          <%--  <!-- Search Button Unit --><div class="modal fade" id="myWipem">
        <div class="modal-dialog modal-xl">
            <div class="modal-content">
                <div class="modal-header" style="cursor: move;">
                    <h4 class="modal-title">ID.Unit</h4><button type="button" class="close text-black" data-dismiss="modal" aria-label="Close">
                        <span aria-hidden="true">&times;</span></button></div><div class="modal-body">
                    <div class="input-group col-lg-4 mb-3">
                        <input id="txtSearchWipem" runat="server" type="text" style="text-transform: capitalize;" class="form-control" placeholder="Search" /> <div class="input-group-append">
                            <div class="input-group-text">
                                <span class="fas fa-search"></span>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="table-bordered table-responsive p-0 hide-scrollbar" style="max-height: 60vh;">
                    <table id="TblWipem" runat="server" class="table table-hover table-head-fixed text-nowrap" style="cursor: pointer;">
                        <thead>
                            <tr>
                                <td class="bg-primary">ID.Wipem</td><td class="bg-primary">Lokasi Wipem</td></tr></thead></table></div></div></div></div>--%>


  </div>
</asp:Content><asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder2" Runat="Server">
<script type="text/javascript">
     //Cabang
    function putvalueCabang1(avalue) {
        var phasilCabang1 = document.getElementById('<%=txtIdCabang.ClientID%>');
        var phasilCabang2 = document.getElementById('<%=txtnamacabang.ClientID%>');
        var thetableCabang = document.getElementById('<%=tblCabang.ClientID%>');
        phasilCabang1.value = avalue.textContent;
        phasilCabang2.value = thetableCabang.rows[avalue.parentNode.rowIndex].cells[avalue.cellIndex + 1].innerHTML;
        $('#myCabang').modal('hide');
    }
    function filterTableCabang(event, atbl) {
        var filterCabang = event.target.value.toUpperCase();
        var rowsCabang = document.querySelector("#<%=tblCabang.ClientID%> tbody").rows;
        for (var i = 1; i < rowsCabang.length; i++) {
            var firstColCabang = rowsCabang[i].cells[0].textContent.toUpperCase();
            var secondColCabang = rowsCabang[i].cells[1].textContent.toUpperCase();
            if (firstColCabang.indexOf(filterCabang) > -1 || secondColCabang.indexOf(filterCabang) > -1) {
                rowsCabang[i].style.display = "";
            } else {
                rowsCabang[i].style.display = "none";
            }
        }
    }

    document.querySelector('#<%=txtSearchCabang.ClientID%>').addEventListener('keyup', filterTableCabang, false);
</script>

    <script type="text/javascript">
     //Unit
    function putvalueUnit1(avalue) {
        var phasilUnit1 = document.getElementById('<%=txtIdUnit.ClientID%>');
        var phasilUnit2 = document.getElementById('<%=txtnamaUnit.ClientID%>');
        var thetableUnit = document.getElementById('<%=tblUnit.ClientID%>');
        phasilUnit1.value = avalue.textContent;
        phasilUnit2.value = thetableUnit.rows[avalue.parentNode.rowIndex].cells[avalue.cellIndex + 1].innerHTML;
        $('#myUnit').modal('hide');
    }
    function filterTableUnit(event, atbl) {
        var filterUnit = event.target.value.toUpperCase();
        var rowsUnit = document.querySelector("#<%=tblUnit.ClientID%> tbody").rows;
        for (var i = 1; i < rowsUnit.length; i++) {
            var firstColUnit = rowsUnit[i].cells[0].textContent.toUpperCase();
            var secondColUnit = rowsUnit[i].cells[1].textContent.toUpperCase();
            if (firstColUnit.indexOf(filterUnit) > -1 || secondColUnit.indexOf(filterUnit) > -1) {
                rowsUnit[i].style.display = "";
            } else {
                rowsUnit[i].style.display = "none";
            }
        }
    }

    document.querySelector('#<%=txtSearchUnit.ClientID%>').addEventListener('keyup', filterTableUnit, false);
</script>

 <%--<script type="text/javascript">
     //Wipem
    function putvalueWipem1(avalue) {
        var phasilWipem1 = document.getElementById('<%=txtIdWipem.ClientID%>');
        var phasilWipem2 = document.getElementById('<%=txtnamaWipem.ClientID%>');
        var thetableWipem = document.getElementById('<%=tblWipem.ClientID%>');
        phasilWipem1.value = avalue.textContent;
        phasilWipem2.value = thetableWipem.rows[avalue.parentNode.rowIndex].cells[avalue.cellIndex + 1].innerHTML;
        $('#myWipem').modal('hide');
    }
    function filterTableWipem(event, atbl) {
        var filterWipem = event.target.value.toUpperCase();
        var rowsWipem = document.querySelector("#<%=tblWipem.ClientID%> tbody").rows;
        for (var i = 1; i < rowsWipem.length; i++) {
            var firstColWipem = rowsWipem[i].cells[0].textContent.toUpperCase();
            var secondColWipem = rowsWipem[i].cells[1].textContent.toUpperCase();
            if (firstColWipem.indexOf(filterWipem) > -1 || secondColWipem.indexOf(filterWipem) > -1) {
                rowsWipem[i].style.display = "";
            } else {
                rowsWipem[i].style.display = "none";
            }
        }
    }

    document.querySelector('#<%=txtSearchWipem.ClientID%>').addEventListener('keyup', filterTableWipem, false);
</script>--%>

</asp:Content>

