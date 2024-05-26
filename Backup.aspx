<%@ Page Title="Back up Data" Language="VB" MasterPageFile="~/MasterPage.master" AutoEventWireup="false" CodeFile="Backup.aspx.vb" Inherits="Backup"%>


<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div class="panel panel-primary hide-panel-bar" style="min-height: calc(100vh - 100px); overflow-y: auto; padding-left: 20px; padding-bottom: 20px; padding-right: 20px; padding-top: 20px;"> 
        <!-- Header -->
        <h4>Backup Database</h4>

        <hr style="display: block; margin-top: 0.5em; margin-bottom: 0.5em; margin-left: auto; margin-right: auto; border-style: inset; border-width: 1px;" />
        <div class="row">
            <div class="col-sm-1">
                <label style="font-size:small;height:26px;">Tgl.Backup</label>
            </div>
            <div class="col-sm-2"> 
                <div class="input-group date" id="div1" runat="server">
                    <input id="dtpTgl" type="text" runat="server" class="form-control input-sm" placeholder=""  autocomplete="off"/>
                    <span class="input-group-addon"><span class="glyphicon glyphicon-calendar"></span></span>
                </div>
            </div>
            <div class="col-sm-2">
                <asp:LinkButton id="btnSave" Class="btn btn-primary btn-sm" runat="server" OnClick="lnkSave_Click" OnClientClick="return getSaveConfirmation(this, 'Please confirm','Are you sure you want to backup this data ?');"  width=100px BorderColor=#3385ff><span class="glyphicon glyphicon-floppy-saved"></span>  Backup</asp:LinkButton> 
            </div>
        </div>
    </div> 
    <!-- Pop up -->
 
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder2" Runat="Server">
    <script type="text/javascript">
        $(function () {
            $('#<%=dtpTgl.ClientID%>').datepicker({
                autoclose: true,
                dateFormat: 'dd/mm/yy'
            });
        });
    </script>
</asp:Content>

