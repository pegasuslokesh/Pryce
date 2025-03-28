<%@ Control Language="C#" AutoEventWireup="true" CodeFile="UpdateLicense.ascx.cs" Inherits="WebUserControl_UpdateLicense" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:UpdatePanel ID="updPnlLicense" runat="server">
    <ContentTemplate>
        <div class="box-body">
            <asp:HiddenField ID="hdnLicMessage" Value="" runat="server" />
            <asp:HiddenField ID="hdnLicUrl" Value="" runat="server" />
            
            <div class="row">
                <div class="form-group">
                    <div class="col lg-12">
                        <asp:Label ID="lblKey" runat="server" Text="Please enter license key" />
                        <asp:TextBox ID="txtProductKey" runat="server" CssClass="form-control" />
                        <asp:RequiredFieldValidator runat="server" ID="RequiredFieldValidator2" ControlToValidate="txtProductKey" ErrorMessage="Please enter License Key!" />
                        <br />
                    </div>
                    <div class="col lg-12">
                        <asp:Button ID="btnSyncLicInfo" runat="server" OnClick="btnSyncLicInfo_Click" Visible="true" CssClass="btn btn-default" Text="Activate"  />
                        <br />
                    </div>
                    <div class="col lg-12">
                        <br />
                        <asp:Literal ID="lnkUpdateLicense" runat="server" />
                    </div>
                </div>
            </div>
        </div>
    </ContentTemplate>
</asp:UpdatePanel>
