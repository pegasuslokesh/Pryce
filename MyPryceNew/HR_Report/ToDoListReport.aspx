﻿<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ToDoListReport.aspx.cs" Inherits="HR_Report_ToDoListReport" %>

<%@ Register Assembly="DevExpress.XtraReports.v18.1.Web.WebForms, Version=18.1.3.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.XtraReports.Web" TagPrefix="dx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>To Do List</title>
</head>
<body>
    <form id="form1" runat="server">
        <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
        <asp:UpdatePanel ID="Update1" runat="server">
            <ContentTemplate>
                <div>
                    <center>
                        <table width="100%" cellpadding="0" cellspacing="0">
                            <tr style="background-color: #90BDE9">
                                <td>
                                    <table>
                                        <tr>
                                            <td>
                                                <img src="../Images/compare.png" width="31" height="30" alt="D" />
                                            </td>
                                            <td>
                                                <img src="../Images/seperater.png" width="2" height="43" alt="SS" />
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                                <td style="padding-left: 5px" align="left">
                                    <asp:Label ID="lblHeader" runat="server" Text="Employee Task Report"
                                        CssClass="LableHeaderTitle"></asp:Label>
                                </td>
                                <td align="right" style="padding-right: 10px"></td>
                            </tr>
                            <tr style="background-color: #fff">
                                <td></td>
                                <td colspan="2">
                                    <table width="100%">
                                        <tr>
                                            <td align="left" colspan="6">
                                                <dx:ReportToolbar ID="rptToolBar" runat="server" ShowDefaultButtons="False" ReportViewer="<%# rptViewer %>"
                                                    Width="93%" AccessibilityCompliant="True">
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
                                                <dx:ReportViewer ID="rptViewer" OnCacheReportDocument="DocumentViewer1_CacheReportDocument" OnRestoreReportDocumentFromCache="DocumentViewer1_RestoreReportDocumentFromCache" runat="server">
                                                </dx:ReportViewer>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                        </table>
                    </center>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </form>
</body>
</html>
