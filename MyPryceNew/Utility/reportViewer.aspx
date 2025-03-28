<%@ Page Language="C#" AutoEventWireup="true" CodeFile="reportViewer.aspx.cs" Inherits="reportViewer" %>

<%@ Register Assembly="DevExpress.XtraReports.v18.1.Web.WebForms, Version=18.1.3.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.XtraReports.Web" TagPrefix="dx" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title runat="server">
        <asp:Literal runat="server" ID="lblHeader"></asp:Literal></title>
     <script type="text/javascript">
         function onInit(s, e) {
            window.onbeforeunload = function (etv) {
                debugger;
                s.GetPreviewModel().reportPreview.deactivate();
            }
        }
    </script>
</head>
<body>
   
    <form id="form1" runat="server">
        <asp:ScriptManager runat="server"></asp:ScriptManager>
        <asp:UpdatePanel runat="server" ID="upExport">
            <ContentTemplate>
                <asp:Button runat="server" ID="btnsendEmail" Text="Send Mail" OnClick="btnsendEmail_Click" />             
            </ContentTemplate>
            <Triggers>
                <asp:PostBackTrigger ControlID="btnsendEmail" />
            </Triggers>
        </asp:UpdatePanel>

        <div>
            <dx:ASPxWebDocumentViewer ID="ASPxWebDocumentViewer1" runat="server" ClientSideEvents-Init="onInit"></dx:ASPxWebDocumentViewer>
        </div>
      
    </form>
</body>
</html>
