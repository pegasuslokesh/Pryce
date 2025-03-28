<%@ Page Title="" Language="C#" MasterPageFile="~/ERPMaster.master" AutoEventWireup="true"
    CodeFile="GSTDetails_Report.aspx.cs" Inherits="GSTDetails_Report" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit.HTMLEditor"
    TagPrefix="cc1" %>
<%@ Register assembly="DevExpress.XtraReports.v18.1.Web.WebForms, Version=18.1.3.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.XtraReports.Web" tagprefix="dx" %>

<asp:Content ID="Content2" ContentPlaceHolderID="Head" runat="Server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="HeaderContent" runat="Server">
    <h1>
        <img src="../Images/product_category.png" alt="" />
        <asp:Label ID="Label2" runat="server" Text="Tax Transaction Report"></asp:Label>
    </h1>
    <ol class="breadcrumb">
        <li><a href="#"><i class="fa fa-dashboard"></i>
            <asp:Label ID="Label4" runat="server" Text="<%$ Resources:Attendance,Inventory%>"></asp:Label></a></li>
        <li><a href="#"><i class="fa fa-dashboard"></i>
            <asp:Label ID="Label10" runat="server" Text="<%$ Resources:Attendance,Sales Report%>"></asp:Label></a></li>
        <li class="active">
            <asp:Label ID="Label5" runat="server" Text="Tax Transaction Report"></asp:Label></li>
    </ol>
</asp:Content>

<asp:Content ID="Content4" ContentPlaceHolderID="MainContent" runat="Server">

    <asp:UpdatePanel ID="Update_New" runat="server">
        <ContentTemplate>
            <div id="pnlFilterRecords" runat="server" class="row">
                <div class="col-md-12">
                <div class="box box-primary">
                    <div class="box-header with-border">
                        <h3 class="box-title">Report Filter</h3>
                        <div class="box-tools pull-right">
                            <button type="button" class="btn btn-box-tool" data-widget="collapse" data-toggle="tooltip">
                                <i class="fa fa-minus"></i>
                            </button>
                        </div>
                    </div>
                    <div class="box-body">
                        <div class="row">
                             <div class="col-md-6">
                                    <asp:Label ID="Label27" runat="server" Text="<%$ Resources:Attendance,From Date %>"></asp:Label>
                                    <asp:TextBox ID="txtFromDate" runat="server" CssClass="form-control"></asp:TextBox>
                                     <cc1:CalendarExtender ID="txtFrom_CalendarExtender" runat="server" Enabled="True"
                                                        TargetControlID="txtFromDate">
                                      </cc1:CalendarExtender>
                                    <br />
                                </div>
                                
                                <div class="col-md-6">
                                     <asp:Label ID="Label3" runat="server" Text="<%$ Resources:Attendance,To Date %>"> </asp:Label>
                                      <asp:TextBox ID="txtToDate" runat="server" CssClass="form-control" ></asp:TextBox>
                                      <cc1:CalendarExtender ID="CalendarExtender1" runat="server" Enabled="True" TargetControlID="txtToDate">
                                      </cc1:CalendarExtender>
                                    <br />
                                </div>
                               
                              
                                <div class="col-md-6">
                                     <asp:Label ID="Label6" runat="server" Text="Tax Type"></asp:Label>
                                     <asp:DropDownList ID="ddlTaxCategory" CssClass="form-control" runat="server">
                                                               </asp:DropDownList>
                                    <br />
                                </div>
                                
                                <div class="col-md-6">
                                   <asp:Label ID="Label1" runat="server" Text="Transaction"></asp:Label>
                                    <asp:DropDownList ID="ddlTransaction" CssClass="form-control" runat="server">
                                                                   <asp:ListItem Text="ALL" Value="ALL"></asp:ListItem>
                                                                   <asp:ListItem Text="PURCHASE" Value="PINV"></asp:ListItem>
                                                                    <asp:ListItem Text="PURCHASE RETURN" Value="PR"></asp:ListItem>
                                                                   <asp:ListItem Text="SALES" Value="SINV"></asp:ListItem>
                                                                   <asp:ListItem Text="SALES RETURN" Value="SR"></asp:ListItem>

                                                               </asp:DropDownList>
                                   <br />
                                </div>
                                <div class="col-md-6">
                                    <asp:RadioButton ID="rbPosted" GroupName="recordType" runat="server" CssClass="form-control" Text="Posted Records" Checked="true" />
                                    <asp:RadioButton ID="rbUnposted" GroupName="recordType" runat="server" CssClass="form-control" Text="Unposted Records" Checked="false" />
                                </div>
                               
                                <br />
                                <div class="col-md-12" style="text-align:center">
                                    <br />
                                    <asp:Button ID="btngo" runat="server" CssClass="btn btn-primary" OnClick="btngo_Click"
                                        Text="<%$ Resources:Attendance,Go %>" />
                                    <asp:Button ID="btnReset" runat="server" CssClass="btn btn-primary" OnClick="btnReset_Click"
                                        Text="<%$ Resources:Attendance,Reset %>" />
                                    <br />
                                </div>
                        </div>
                    </div>
                </div>
                </div>
            </div>
            <div id="pnlReport" runat="server" class="row">
                <div class="col-md-12">
                    <div class="box box-primary">
                        <div class="box-body">
                            <div class="form-group">
                                <%--<div class="col-md-6">
                                    <asp:LinkButton ID="lnkback" runat="server" CssClass="acc" OnClick="lnkback_Click"
                                        Text="<%$ Resources:Attendance,Back %>"></asp:LinkButton>
                                    <br />
                                </div>--%>
                                <div style="overflow: auto; ">
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
                                    <br />
                                </div>
                                <div>
                                    <asp:Panel ID="pnlrptviewer" runat="server" Width="100%" Height="100%">
                                        <dx:ReportViewer ID="rptViewer" runat="server" AutoSize="false" Width="100%" Height="850px">
                                        </dx:ReportViewer>
                                    </asp:Panel>
                                    <br />
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>

    <asp:UpdateProgress ID="UpdateProgress2" runat="server" AssociatedUpdatePanelID="Update_New">
        <ProgressTemplate>
            <div class="modal_Progress">
                <div class="center_Progress">
                    <img alt="" src="../Images/ajax-loader2.gif" style="vertical-align: middle; background-color: White; border-radius: 10px;" />
                </div>
            </div>
        </ProgressTemplate>
    </asp:UpdateProgress>

</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="FooterContent" runat="Server">
</asp:Content>
<asp:Content ID="Content6" ContentPlaceHolderID="AfterLoadScriptContent" runat="Server">
    <script type="text/javascript">
        function resetPosition(object, args) {
            $(object._completionListElement.children).each(function () {
                var data = $(this)[0];
                if (data != null) {
                    data.style.paddingLeft = "10px";
                    data.style.cursor = "pointer";
                    data.style.borderBottom = "solid 1px #e7e7e7";
                }
            });
            object._completionListElement.className = "scrollbar scrollbar-primary force-overflow";
            var tb = object._element;
            var tbposition = findPositionWithScrolling(tb);
            var xposition = tbposition[0] + 2;
            var yposition = tbposition[1] + 25;
            var ex = object._completionListElement;
            if (ex)
                $common.setLocation(ex, new Sys.UI.Point(xposition, yposition));
        }
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
    <script type="text/javascript">
        function treeList_CustomDataCallback(s, e) {
            document.getElementById('treeListCountCell').innerHTML = e.result;
        }
        function treeList_SelectionChanged(s, e) {
            window.setTimeout(function () { s.PerformCustomDataCallback(''); }, 0)
        }
    </script>
</asp:Content>


