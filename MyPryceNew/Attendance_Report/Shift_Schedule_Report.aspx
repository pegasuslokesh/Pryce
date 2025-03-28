<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Shift_Schedule_Report.aspx.cs" Inherits="Attendance_Report_Shift_Schedule_Report" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register TagPrefix="exp1" Namespace="ControlFreak" Assembly="ExportPanel" %>
<%@ Register Assembly="DevExpress.XtraReports.v18.1.Web.WebForms, Version=18.1.3.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.XtraReports.Web" TagPrefix="dx" %>
<!DOCTYPE html>
<html>
<head runat="server">
    <meta charset="utf-8">
    <meta http-equiv="X-UA-Compatible" content="IE=edge">
    <title>Report</title>
    <meta content="width=device-width, initial-scale=1, maximum-scale=1, user-scalable=no" name="viewport">
    <link rel="SHORTCUT ICON" href="Images/favicon.ico" />
    <link rel="stylesheet" href="../Bootstrap_Files/bootstrap/css/bootstrap.min.css">
    <link href="../Bootstrap_Files/font-awesome-4.7.0/font-awesome-4.7.0/css/font-awesome.min.css" rel="stylesheet" type="text/css" />
    <link href="../Bootstrap_Files/ionicons-2.0.1/ionicons-2.0.1/css/ionicons.min.css" rel="stylesheet" type="text/css" />
    <link rel="stylesheet" href="../Bootstrap_Files/dist/css/AdminLTE.min.css">
    <link rel="stylesheet" href="../Bootstrap_Files/dist/css/skins/_all-skins.min.css">
</head>
<body class="skin-blue" style="background-color: #ECF0F5">
    <form runat="server">
        <header class="main-header">
            <a class="logo">
                <span class="logo-lg">
                    <img style="width: 25px; margin-top: -5px;" src="../Images/compare.png" alt="" />
                    <asp:Label ID="Lbl_Page_Name" Style="margin-left: 10px;" runat="server" Text="<%$ Resources:Attendance,Shift Schedule Report%>"></asp:Label>
                </span>
            </a>
            <nav class="navbar navbar-static-top">
            </nav>
        </header>
        <div class="content">
            <section class="content">
                <div class="box box-primary">
                    <div class="box-header with-border">
                        <h3 class="box-title">Report</h3>
                        <div class="box-tools pull-right">
                            <%--<button type="button" class="btn btn-box-tool" data-widget="collapse" data-toggle="tooltip">
                                <i class="fa fa-minus"></i>
                            </button>--%>
                        </div>
                    </div>
                    <div class="box-body">
                        <div class="row">
                            <div class="col-md-12">
                               <asp:ImageButton ID="btnExportPdf" runat="server" CommandArgument="1" CommandName="OP"
                                                        Height="23px" ImageUrl="~/Images/pdfIcon.jpg" OnCommand="btnExportPdf_Command" />
                                <asp:ImageButton
                                                            ID="btnExportToExcel" runat="server" CommandArgument="2" CommandName="OP" Height="21px"
                                                            ImageUrl="~/Images/excel-icon.gif" OnCommand="btnExportPdf_Command" />
                            </div>                               
                            <div class="col-md-12">
                                   <exp1:ExportPanel ID="ExportPanel1" runat="server" ScrollBars="Auto" Width="100%" Height="390px"
                                                        ExportType="HTML" OpenInBrowser="True">
                                                        <asp:Label runat="server" ID="lblTitle" Font-Bold="true" CssClass="labelComman" Style="margin-left: 260px;"></asp:Label>
                                       <br />
                                       <br />
                                                       <%-- <table>
                                                            <tr style="width: 100%">
                                                                <td style="width: 40%">
                                                                    <asp:Label runat="server" ID="lblBrandName" Font-Bold="true" CssClass="labelComman" Text="<%$ Resources:Attendance,Brand%>"></asp:Label>
                                                                    : &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                             <asp:Label runat="server" ID="lblBrand" Font-Bold="true" CssClass="labelComman"></asp:Label>
                                                                </td>
                                                                <td style="width: 40%">
                                                                    <asp:Label runat="server" ID="lblLocName" Font-Bold="true" CssClass="labelComman" Text="<%$ Resources:Attendance,Location%>"></asp:Label>
                                                                    :&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                              <asp:Label runat="server" ID="lblLocation" Font-Bold="true" CssClass="labelComman"></asp:Label>
                                                                </td>
                                                                <td style="width: 33%">
                                                                    <asp:Label runat="server" ID="lblDeptName" Font-Bold="true" CssClass="labelComman" Text="<%$ Resources:Attendance,Department%>"></asp:Label>
                                                                    : &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                               <asp:Label runat="server" ID="lblDept" Font-Bold="true" CssClass="labelComman"></asp:Label>
                                                                </td>
                                                            </tr>
                                                        </table>--%>
                                                        <asp:Table ID="Table1" Font-Names="Verdana" BackColor="White" Font-Size="12px" runat="server" Font BorderStyle="Solid" CellPadding="1" CellSpacing="1"
                                                            GridLines="Both" Height="22px" Width="100%">
                                                        </asp:Table>
                                                    </exp1:ExportPanel>
                            </div>
                            <div class="col-md-12">
                                <br />
                            </div>
                            <div class="col-md-12">
                                <br />
                            </div>
                        </div>
                    </div>
                </div>
            </section>
        </div>
    </form>
    <script src="../Bootstrap_Files/plugins/jQuery/jquery-2.2.3.min.js"></script>
    <script src="../Bootstrap_Files/bootstrap/js/bootstrap.min.js"></script>
    <script src="../Bootstrap_Files/plugins/slimScroll/jquery.slimscroll.min.js"></script>
    <script src="../Bootstrap_Files/plugins/fastclick/fastclick.js"></script>
    <script src="../Bootstrap_Files/dist/js/app.min.js"></script>
    <script src="../Bootstrap_Files/dist/js/demo.js"></script>
</body>
</html>
