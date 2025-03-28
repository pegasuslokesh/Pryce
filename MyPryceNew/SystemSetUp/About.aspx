<%@ Page Title="" Language="C#" MasterPageFile="~/ERPMaster.master" AutoEventWireup="true" CodeFile="About.aspx.cs" Inherits="SystemSetUp_About" %>

<%@ Register Src="~/WebUserControl/TimeManLicense.ascx" TagPrefix="uc1" TagName="UpdateLicense" %>
<asp:Content ID="Content1" ContentPlaceHolderID="Head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="HeaderContent" runat="Server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="Server">

    <div class="modal fade" id="ModelUpdateLicense" role="dialog">
        <div class="modal-dialog modal-lg">
            <!-- Modal content-->
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal">&times;</button>
                    <div class="modal-title" style="text-align: left;">
                        <img src="https://www.pegasustech.net/image/catalog/logo.png" class="img-responsive" width="120px" />
                    </div>
                </div>

                <div>
                    <uc1:UpdateLicense ID="UC_LicenseInfo" runat="server" />
                </div>
                <div class="modal-footer">
                </div>
            </div>

        </div>
    </div>

</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="FooterContent" runat="Server">
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="AfterLoadScriptContent" runat="Server"><script src="../Script/common.js"></script>
 
      <script type="text/javascript">
          function Modal_UpdateLicense_Open() {
              $('#ModelUpdateLicense').modal('show')
          }
          $(document).ready(function () {
              Modal_UpdateLicense_Open();
          });
           </script>
</asp:Content>

