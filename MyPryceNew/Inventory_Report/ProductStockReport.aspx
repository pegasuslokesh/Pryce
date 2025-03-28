<%@ Page Title="" Language="C#" MasterPageFile="~/ERPMaster.master" AutoEventWireup="true" CodeFile="ProductStockReport.aspx.cs" Inherits="Inventory_Report_ProductStockReport" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit.HTMLEditor" TagPrefix="cc1" %>
<%@ Register Assembly="DevExpress.XtraReports.v18.1.Web.WebForms, Version=18.1.3.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.XtraReports.Web" TagPrefix="dx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="Head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="HeaderContent" runat="Server">
    <h1>
        <img src="../Images/product_ledger.png" alt="" style="width: 25px;" />
        <asp:Label ID="lblHeader" runat="server" Text="<%$ Resources:Attendance,Product Stock Report%>"></asp:Label>
    </h1>
    <ol class="breadcrumb">
        <li><a href="#"><i class="fa fa-dashboard"></i>
            <asp:Label ID="Label6" runat="server" Text="<%$ Resources:Attendance,Inventory%>"></asp:Label></a></li>
        <li><a href="#"><i class="fa fa-dashboard"></i>
            <asp:Label ID="Label10" runat="server" Text="<%$ Resources:Attendance,Inventory Report%>"></asp:Label></a></li>
        <li class="active">
            <asp:Label ID="Label5" runat="server" Text="<%$ Resources:Attendance,Product Stock Report%>"></asp:Label></li>
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
                        <div class="col-md-12">
                            <asp:Label ID="Label1" runat="server"  Text="<%$ Resources:Attendance,Group By%>" />:
                            <asp:RadioButton ID="rbtnNone" style="margin-left:20px;" runat="server" Text="None" Checked="true" GroupName="a"  />
                            <asp:RadioButton ID="rbtnByBrand" style="margin-left:20px;" runat="server" Text="Product Brand" GroupName="a"  />
                            <asp:RadioButton ID="rbtngroupByCategoryName" style="margin-left:20px;" runat="server" Text="Product Category" GroupName="a"  />
                            <asp:RadioButton ID="rbtnGroupByRackName" runat="server" Text="Rack Name" style="margin-left:20px;" GroupName="a"  />
                            <asp:RadioButton ID="rbtnbysupplier" runat="server" Text="Supplier" GroupName="a" style="margin-left:20px;"  />
                            <asp:RadioButton ID="rbtnbysupplierandBrand" runat="server" Text="Supplier and Brand" Visible="false" style="margin-left:20px;" GroupName="a"  />
                        </div>
                        <div class="col-md-12">
                            <br />
                        </div>
                        <div class="col-md-6">
                            <asp:Label ID="Label9" runat="server"  Text="<%$ Resources:Attendance,Product Id%>" />
                            <asp:TextBox ID="txtProductcode" runat="server" BackColor="#eeeeee" CssClass="form-control" AutoPostBack="True"
                                OnTextChanged="txtProductCode_TextChanged"/>

                            <cc1:AutoCompleteExtender ID="AutoCompleteExtender1" runat="server" CompletionInterval="100"
                                DelimiterCharacters="" Enabled="True" MinimumPrefixLength="1" ServiceMethod="GetCompletionListProductCode"
                                ServicePath="" TargetControlID="txtProductcode" UseContextKey="True"  CompletionListCssClass="autoCompleteList" CompletionListItemCssClass="autoCompleteListItem" CompletionListHighlightedItemCssClass="autoCompleteSelectedListItem" OnClientShown="resetPosition">
                            </cc1:AutoCompleteExtender>
                            <br />
                        </div>
                        <div class="col-md-6">
                            <asp:Label ID="lblProductName" runat="server"  Text="<%$ Resources:Attendance,Product Name %>" />
                            <asp:TextBox ID="txtProductName" BackColor="#eeeeee" runat="server" CssClass="form-control" AutoPostBack="true"
                                OnTextChanged="txtProductName_TextChanged" />
                            <cc1:AutoCompleteExtender ID="AutoCompleteExtender2" runat="server" DelimiterCharacters=""
                                Enabled="True" ServiceMethod="GetCompletionListProductName" ServicePath="" CompletionInterval="100"
                                MinimumPrefixLength="1" CompletionSetCount="1" TargetControlID="txtProductName"
                                UseContextKey="True"  CompletionListCssClass="autoCompleteList" CompletionListItemCssClass="autoCompleteListItem" CompletionListHighlightedItemCssClass="autoCompleteSelectedListItem" OnClientShown="resetPosition">
                            </cc1:AutoCompleteExtender>
                            <asp:HiddenField ID="hdnNewProductId" runat="server" Value="0" />
                            <br />
                        </div>
                        <div class="col-md-6">
                            <asp:Label ID="Label12" runat="server"  Text="<%$ Resources:Attendance,Location  %>"></asp:Label>
                            <asp:DropDownList ID="ddlLocation" runat="server" CssClass="form-control">
                            </asp:DropDownList>
                            <br />
                        </div>
                        <div class="col-md-6">
                            <asp:Label ID="lblSupplier" runat="server"  Text="<%$ Resources:Attendance,Supplier Name %>" />
                            <asp:TextBox ID="txtSuppliers" runat="server" BackColor="#eeeeee" OnTextChanged="txtSuppliers_OnTextChanged"
                                AutoPostBack="True" CssClass="form-control"></asp:TextBox>
                            <cc1:AutoCompleteExtender ID="txtSuppliers_AutoCompleteExtender" runat="server" CompletionInterval="100"
                                DelimiterCharacters="" Enabled="True" MinimumPrefixLength="1" ServiceMethod="GetCompletionList_Supplier"
                                ServicePath="" TargetControlID="txtSuppliers" UseContextKey="True"  CompletionListCssClass="autoCompleteList" CompletionListItemCssClass="autoCompleteListItem" CompletionListHighlightedItemCssClass="autoCompleteSelectedListItem" OnClientShown="resetPosition">
                            </cc1:AutoCompleteExtender>
                            <br />
                        </div>
                        <div class="col-md-6">
                            <asp:Label ID="lblBrandsearch" runat="server" Text="<%$ Resources:Attendance,Manufacturing Brand %>"
                                ></asp:Label>
                            <asp:DropDownList ID="ddlbrandsearch" runat="server" CssClass="form-control">
                            </asp:DropDownList>
                            <br />
                        </div>
                        <div class="col-md-6">
                            <asp:Label ID="lbllocationsearch" runat="server" Text="<%$ Resources:Attendance,Category %>"/>
                            <asp:DropDownList ID="ddlcategorysearch" runat="server" CssClass="form-control">
                            </asp:DropDownList>
                            <br />
                        </div>
                        <div class="col-md-6">
                            <asp:CheckBox ID="chkNegativestock" runat="server" Text="Negative Stock Report"  CssClass="form-control"/>
                            <br />
                        </div>
                        <div class="col-md-6">
                            <asp:CheckBox ID="chkZeroCostPrice" Text="Zero Cost Price" runat="server" CssClass="form-control" />
                            <br />
                        </div>
                    </div>
                </div>
                <div class="box-footer">
                    <div class="col-md-12" style="text-align: center">
                        <asp:Button ID="btngo" runat="server" CausesValidation="False" Text="<%$ Resources:Attendance,Go %>"
                            CssClass="btn btn-primary" OnClick="btngo_Click" />

                        <asp:Button ID="btnReset" runat="server" CausesValidation="False" Text="<%$ Resources:Attendance,Reset %>"
                            CssClass="btn btn-primary" OnClick="btnReset_Click" />
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
                                <dx:ReportViewer ID="rptViewer" runat="server" AutoSize="true" Height="500px">
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
