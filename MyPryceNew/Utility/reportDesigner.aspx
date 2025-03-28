<%@ Page Language="C#" AutoEventWireup="true" CodeFile="reportDesigner.aspx.cs" Inherits="reportDesigner" %>

<%@ Register Assembly="DevExpress.XtraReports.v18.1.Web.WebForms, Version=18.1.3.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.XtraReports.Web" TagPrefix="dx" %>



<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <script type="text/javascript">
        // The CustomizeMenuActions event handler.
        function CustomizeMenuActions(s, e) {
            var actions = e.Actions;
            // Register the custom Close menu command.
            actions.push({
                text: "Close",
                imageClassName: "customButton",
                disabled: ko.observable(false),
                visible: true,
                // The clickAction function recieves the client-side report model
                // allowing you interact with the currently opened report.
                clickAction: function (report) {
                    window.location = "customizereport.aspx";
                },
                container: "menu"
            });
        }
    </script>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <dx:ASPxReportDesigner ID="ASPxReportDesigner1" runat="server" ClientSideEvents-CustomizeMenuActions="CustomizeMenuActions"></dx:ASPxReportDesigner>
        </div>
    </form>
</body>
</html>
