<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ProductStockPrint.aspx.cs" Inherits="Inventory_Report_ProductStockPrint" %>

<%@ Register Assembly="DevExpress.XtraReports.v18.1.Web.WebForms, Version=18.1.3.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.XtraReports.Web" TagPrefix="dx" %>
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
                    <img style="width: 25px; margin-top: -5px;" src="../Images/product_icon.png" alt="" />
                    <asp:Label ID="Lbl_Page_Name" Style="margin-left: 10px;" runat="server" Text="<%$ Resources:Attendance,Product Stock%>"></asp:Label>
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
                        <asp:Button ID="btnSend" runat="server" Text="Send"  CssClass="buttonCommman" OnClick="btnSend_Click"  Visible="false"/>
                        <div class="box-tools pull-right">
                            <%--<button type="button" class="btn btn-box-tool" data-widget="collapse" data-toggle="tooltip">
                                <i class="fa fa-minus"></i>
                            </button>--%>
                        </div>
                    </div>
                    <div class="box-body">
                        <div class="row">
                            <div class="col-md-12">
                                <div style="overflow: auto">
                                    <dx:ReportToolbar ID="rptToolBar" runat="server" ShowDefaultButtons="False" ReportViewer="<%# rptViewer %>"
                                        Width="100%" AccessibilityCompliant="True">
                                        <Items>
                                            <dx:ReportToolbarButton ItemKind="Search" />
                                            <dx:ReportToolbarSeparator />
                                            <dx:ReportToolbarButton ItemKind="PrintReport" />
                                            <dx:ReportToolbarButton ItemKind="PrintPage" />
                                            <dx:ReportToolbarSeparator />
                                            <dx:ReportToolbarButton Enabled="False" ItemKind="FirstPage" />
                                            <dx:ReportToolbarButton Enabled="False" ItemKind="PreviousPage" />
                                            <dx:ReportToolbarLabel ItemKind="PageLabel" />
                                            <dx:ReportToolbarComboBox ItemKind="PageNumber" Width="65px">
                                            </dx:ReportToolbarComboBox>
                                            <dx:ReportToolbarLabel ItemKind="OfLabel" />
                                            <dx:ReportToolbarTextBox IsReadOnly="True" ItemKind="PageCount" />
                                            <dx:ReportToolbarButton ItemKind="NextPage" />
                                            <dx:ReportToolbarButton ItemKind="LastPage" />
                                            <dx:ReportToolbarSeparator />
                                            <dx:ReportToolbarButton ItemKind="SaveToDisk" />
                                            <dx:ReportToolbarButton ItemKind="SaveToWindow" />
                                            <dx:ReportToolbarComboBox ItemKind="SaveFormat" Width="70px">
                                                <Elements>
                                                    <dx:ListElement Value="pdf" />
                                                    <dx:ListElement Value="xls" />
                                                    <dx:ListElement Value="xlsx" />
                                                    <dx:ListElement Value="rtf" />
                                                    <dx:ListElement Value="mht" />
                                                    <dx:ListElement Value="html" />
                                                    <dx:ListElement Value="txt" />
                                                    <dx:ListElement Value="csv" />
                                                    <dx:ListElement Value="png" />
                                                </Elements>
                                            </dx:ReportToolbarComboBox>
                                        </Items>
                                        <Styles>
                                            <LabelStyle>
                                                <Margins MarginLeft="3px" MarginRight="3px" />
                                            </LabelStyle>
                                        </Styles>
                                    </dx:ReportToolbar>
                                </div>
                            </div>
                            <div class="col-md-12">
                                <div style="overflow: auto">
                                    <dx:ReportViewer ID="rptViewer" Width="100%" runat="server">
                                    </dx:ReportViewer>
                                </div>
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
