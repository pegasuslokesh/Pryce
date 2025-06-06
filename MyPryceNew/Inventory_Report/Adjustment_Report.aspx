﻿<%@ Page Title="" Language="C#" MasterPageFile="~/ERPMaster.master" AutoEventWireup="true" CodeFile="Adjustment_Report.aspx.cs" Inherits="Inventory_Report_Adjustment_Report" %>

<%@ Register Assembly="DevExpress.XtraReports.v18.1.Web.WebForms, Version=18.1.3.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.XtraReports.Web" TagPrefix="dx" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit.HTMLEditor"
    TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="Head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="HeaderContent" runat="Server">
    <h1>
        <img src="../Images/inventory_report.png" alt="" style="width:25px;"/>
        <asp:Label ID="lblHeader" runat="server" Text="<%$ Resources:Attendance,Adjustment Report%>"></asp:Label>
    </h1>
    <ol class="breadcrumb">
        <li><a href="#"><i class="fa fa-dashboard"></i>
            <asp:Label ID="Label6" runat="server" Text="<%$ Resources:Attendance,Inventory%>"></asp:Label></a></li>
        <li><a href="#"><i class="fa fa-dashboard"></i>
            <asp:Label ID="Label10" runat="server" Text="<%$ Resources:Attendance,Inventory Report%>"></asp:Label></a></li>
        <li class="active">
            <asp:Label ID="Label5" runat="server" Text="<%$ Resources:Attendance,Adjustment Report%>"></asp:Label></li>
    </ol>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="Server">
    <asp:UpdatePanel ID="Update_Filter" runat="server">
        <ContentTemplate>
            <div class="box box-primary">
                <div class="box-header with-border">
                    <h3 class="box-title">Filter</h3>
                    <div class="box-tools pull-right">
                        <button type="button" class="btn btn-box-tool" data-widget="collapse" data-toggle="tooltip">
                            <i class="fa fa-minus"></i>
                        </button>
                    </div>
                </div>
                <div class="box-body">
                    <div class="row">
                        <div class="col-md-12" style="text-align: center">
                            <asp:RadioButton Checked="true" AutoPostBack="true" ID="rbtnheader"
                                runat="server" GroupName="a" Text="<%$ Resources:Attendance,Header Report %>"
                                OnCheckedChanged="rbtnheader_CheckedChanged" />&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                                    <asp:RadioButton GroupName="a" AutoPostBack="true" ID="RbtnDetail"
                                                        runat="server" Text="<%$ Resources:Attendance,Detail Report %>" OnCheckedChanged="RbtnDetail_CheckedChanged" />
                            <br />
                        </div>
                        <div id="Tr1" runat="server">
                            <div class="col-md-6">
                                <asp:Label ID="Label27" runat="server" Text="<%$ Resources:Attendance,From Date %>"></asp:Label>
                                <asp:TextBox ID="txtFromDate" runat="server" CssClass="form-control"></asp:TextBox>
                                <cc1:CalendarExtender OnClientShown="showCalendar"  ID="txtFrom_CalendarExtender" runat="server" Enabled="True"
                                    TargetControlID="txtFromDate">
                                </cc1:CalendarExtender>
                                <br />
                            </div>
                            <div class="col-md-6">
                                <asp:Label ID="Label3" runat="server" Text="<%$ Resources:Attendance,To Date %>"></asp:Label>
                                <asp:TextBox ID="txtToDate" runat="server" CssClass="form-control"></asp:TextBox>
                                <cc1:CalendarExtender OnClientShown="showCalendar"  ID="CalendarExtender1" runat="server" Enabled="True" TargetControlID="txtToDate">
                                </cc1:CalendarExtender>
                                <br />
                            </div>
                        </div>
                        <div class="col-md-6">
                            <asp:Label ID="Label1" runat="server" Text="<%$ Resources:Attendance,To Location %>"></asp:Label>
                            <asp:TextBox ID="txtToLocation" runat="server" CssClass="form-control"
                                BackColor="#eeeeee" OnTextChanged="txtToLocation_TextChanged" AutoPostBack="true" />
                            <cc1:AutoCompleteExtender ID="AutoCompleteExtender1" runat="server" DelimiterCharacters=""
                                Enabled="True" ServiceMethod="GetCompletionListLocation" ServicePath="" CompletionInterval="100"
                                MinimumPrefixLength="1" CompletionSetCount="1" TargetControlID="txtToLocation"
                                UseContextKey="True"  CompletionListCssClass="autoCompleteList" CompletionListItemCssClass="autoCompleteListItem" CompletionListHighlightedItemCssClass="autoCompleteSelectedListItem" OnClientShown="resetPosition">
                            </cc1:AutoCompleteExtender>
                            <asp:HiddenField ID="hdnToLocationId" runat="server" Value="0" />
                            <br />
                        </div>
                        <div id="trProduct" class="col-md-6">
                            <asp:Label ID="lblProductName" Visible="false" runat="server" CssClass="labelComman"
                                Text="<%$ Resources:Attendance,Product Name %>"></asp:Label>
                            <asp:TextBox ID="txtProductName" runat="server" CssClass="form-control" AutoPostBack="true"
                                OnTextChanged="txtProductName_TextChanged" Visible="false" BackColor="#eeeeee"/>
                            <cc1:AutoCompleteExtender ID="AutoCompleteExtender2" runat="server" DelimiterCharacters=""
                                Enabled="True" ServiceMethod="GetCompletionListProductName" ServicePath="" CompletionInterval="100"
                                MinimumPrefixLength="1" CompletionSetCount="1" TargetControlID="txtProductName"
                                UseContextKey="True"  CompletionListCssClass="autoCompleteList" CompletionListItemCssClass="autoCompleteListItem" CompletionListHighlightedItemCssClass="autoCompleteSelectedListItem" OnClientShown="resetPosition">
                            </cc1:AutoCompleteExtender>
                            <asp:HiddenField ID="hdnProductId" runat="server" Value="0" />
                            <br />
                        </div>
                    </div>
                </div>
                <div class="box-footer">
                    <div class="col-md-12" style="text-align:center">
                        <asp:Button ID="btngo" runat="server" CausesValidation="False" Text="<%$ Resources:Attendance,Go %>" CssClass="btn btn-primary" OnClick="btngo_Click" />
					</div>
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>

    <asp:UpdatePanel ID="Update_Report" runat="server">
        <ContentTemplate>
            <div class="box box-primary">
                <div class="box-header with-border">
                    <h3 class="box-title">Report</h3>
                    <div class="box-tools pull-right">
                        <button type="button" class="btn btn-box-tool" data-widget="collapse" data-toggle="tooltip">
                            <i class="fa fa-minus"></i>
                        </button>
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
                                <dx:ReportViewer ID="rptViewer" runat="server" AutoSize="False" Width="98%" Height="500px">
                                    </dx:ReportViewer>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>

    <asp:UpdateProgress ID="UpdateProgress1" runat="server" AssociatedUpdatePanelID="Update_Filter">
        <ProgressTemplate>
            <div class="modal_Progress">
                <div class="center_Progress">
                    <img alt="" src="../Images/ajax-loader2.gif" style="vertical-align: middle; background-color: White; border-radius: 10px;" />
                </div>
            </div>
        </ProgressTemplate>
    </asp:UpdateProgress>

    <asp:UpdateProgress ID="UpdateProgress2" runat="server" AssociatedUpdatePanelID="Update_Report">
        <ProgressTemplate>
            <div class="modal_Progress">
                <div class="center_Progress">
                    <img alt="" src="../Images/ajax-loader2.gif" style="vertical-align: middle; background-color: White; border-radius: 10px;" />
                </div>
            </div>
        </ProgressTemplate>
    </asp:UpdateProgress>

</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="FooterContent" runat="Server">
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="AfterLoadScriptContent" runat="Server"><script src="../Script/common.js"></script>
    <script type="text/javascript">
      
        function findPositionWithScrolling(oElement) {
            if (typeof (oElement.offsetParent) != 'undefined') {
                var originalElement = oElement;
                for (var posX = 0, posY = 0; oElement; oElement = oElement.offsetParent) {
                    posX += oElement.offsetLeft;
                    posY += oElement.offsetTop;
                    if (oElement != originalElement && oElement != document.body && oElement != document.documentElement) {
                        posX -= oElement.scrollLeft;
                        posY -= oElement.scrollTop;
                    }
                }
                return [posX, posY];
            } else {
                return [oElement.x, oElement.y];
            }
        }
        function showCalendar(sender, args) {

            var ctlName = sender._textbox._element.name;

            ctlName = ctlName.replace('$', '_');
            ctlName = ctlName.replace('$', '_');

            var processingControl = $get(ctlName);
            //var targetCtlHeight = processingControl.clientHeight;
            sender._popupDiv.parentElement.style.top = processingControl.offsetTop + processingControl.clientHeight + 'px';
            sender._popupDiv.parentElement.style.left = processingControl.offsetLeft + 'px';

            var positionTop = processingControl.clientHeight + processingControl.offsetTop;
            var positionLeft = processingControl.offsetLeft;
            var processingParent;
            var continueLoop = false;

            do {
                // If the control has parents continue loop.
                if (processingControl.offsetParent != null) {
                    processingParent = processingControl.offsetParent;
                    positionTop += processingParent.offsetTop;
                    positionLeft += processingParent.offsetLeft;
                    processingControl = processingParent;
                    continueLoop = true;
                }
                else {
                    continueLoop = false;
                }
            } while (continueLoop);

            sender._popupDiv.parentElement.style.top = positionTop + 2 + 'px';
            sender._popupDiv.parentElement.style.left = positionLeft + 'px';
            sender._popupBehavior._element.style.zIndex = 10005;
        }
    </script>
</asp:Content>
