<%@ Page Language="C#" AutoEventWireup="true" CodeFile="PartialViolaionReport.aspx.cs" Inherits="Attendance_Report_PartialViolaionReportNew" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<%@ Register Assembly="DevExpress.XtraReports.v18.1.Web.WebForms, Version=18.1.3.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.XtraReports.Web" TagPrefix="dx" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Pegasus::ERP</title>
</head>
<body>
    <form id="form1" runat="server">
        <link href="../CSS/InvStyle.css" rel="stylesheet" type="text/css" />
        <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
        <asp:UpdatePanel ID="Update1" runat="server">
            <ContentTemplate>
                <table width="100%">
                    <tr>
                        <td>
                            <table width="100%" cellpadding="0" cellspacing="0" bordercolor="#F0F0F0">
                                <tr bgcolor="#90BDE9">
                                    <td align='<%= PageControlCommon.ChangeTDForDefaultLeft()%>'>
                                        <table>
                                            <tr>
                                                <td>
                                                    <img src="../Images/product_icon.png" width="31" height="30" alt="D" />
                                                </td>
                                                <td>
                                                    <img src="../Images/seperater.png" width="2" height="43" alt="SS" />
                                                </td>
                                                <td style="padding-left: 5px">
                                                    <asp:Label ID="lblHeader" runat="server" Text="<%$ Resources:Attendance,Partial Violation Report %>"
                                                        CssClass="LableHeaderTitle"></asp:Label>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>

                                </tr>
                                <tr>
                                    <td bgcolor="#ccddee" width="100%" height="500px" valign="top">


                                        <table width="100%">
                                            <tr>
                                                <td>
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
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:Panel ID="pnlrptviewer" runat="server" Width="100%" Height="100%">
                                                        <dx:ReportViewer ID="rptViewer" OnCacheReportDocument="DocumentViewer1_CacheReportDocument" OnRestoreReportDocumentFromCache="DocumentViewer1_RestoreReportDocumentFromCache" runat="server" AutoSize="False" Width="98%" Height="500px">
                                                        </dx:ReportViewer>
                                                    </asp:Panel>
                                                </td>
                                            </tr>
                                        </table>

                                    </td>
                                </tr>

                            </table>
                        </td>
                    </tr>
                </table>
            </ContentTemplate>
        </asp:UpdatePanel>
    </form>
</body>
</html>
