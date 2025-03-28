<%@ Page Title="" Language="C#" MasterPageFile="~/ERPMaster.master" AutoEventWireup="true" CodeFile="EmployeeWorkHistory.aspx.cs" Inherits="CRM_EmployeeWorkHistory" %>

<%@ Register Assembly="DevExpress.Web.v18.1, Version=18.1.3.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.XtraReports.v18.1.Web.WebForms, Version=18.1.3.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.XtraReports.Web" TagPrefix="dx" %>


<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit.HTMLEditor" TagPrefix="cc1" %>





<asp:Content ID="Content1" ContentPlaceHolderID="Head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="HeaderContent" runat="Server">
    <h1>
        <i class="fas fa-history"></i>&nbsp;&nbsp;
        <asp:Label ID="lblHeader" runat="server" Text="Employee Work History"></asp:Label>
    </h1>
    <ol class="breadcrumb">
        <li><a href="#"><i class="fa fa-dashboard"></i>
            <asp:Label ID="Label6" runat="server" Text="<%$ Resources:Attendance,CRM%>"></asp:Label></a></li>
        <li class="active">
            <asp:Label ID="Label5" runat="server" Text="<%$ Resources:Attendance,Followup%>"></asp:Label></li>
    </ol>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="Server">
    <div class="row">
        <div class="col-md-12">
            <div class="nav-tabs-custom">
               
                <div class="tab-content">
                    <div class="tab-pane active" id="List">
                        <asp:UpdatePanel ID="Update_List" runat="server">
                            <ContentTemplate>
                                <div class="row">
                                    <div class="col-md-12">
                                        <div class="box box-primary">
                                            <div class="box-body">
                                                <div class="form-group">
                                                    <div class="col-md-3">
                                                        <asp:Label ID="lblLocations" runat="server" Text="Location"> </asp:Label>
                                                        <asp:DropDownList ID="ddlLocation" runat="server" Class="form-control" OnSelectedIndexChanged="ddlLocation_SelectedIndexChanged" AutoPostBack="true">
                                                        </asp:DropDownList>
                                                    </div>
                                                    <div class="col-md-2">
                                                        <asp:Label ID="lblDepartment" runat="server" Text="Department"> </asp:Label>
                                                        <asp:DropDownList ID="ddlDepartment" runat="server" Class="form-control" OnSelectedIndexChanged="ddlDepartment_SelectedIndexChanged" AutoPostBack="true">
                                                        </asp:DropDownList>
                                                    </div>
                                                    <div class="col-md-2">
                                                        <asp:Label ID="lblUser" runat="server" Text="Employee"> </asp:Label>
                                                        <asp:DropDownList ID="ddlEmployee" runat="server" Class="form-control" OnSelectedIndexChanged="ddlEmployee_SelectedIndexChanged" AutoPostBack="true">
                                                        </asp:DropDownList>
                                                    </div>
                                                    <div class="col-md-2">
                                                        <asp:Label ID="lblFromdt" runat="server" Text="From Date"> </asp:Label>
                                                        <asp:TextBox ID="txtFromDt" runat="server" Class="form-control" AutoPostBack="true"></asp:TextBox>
                                                        <cc1:CalendarExtender OnClientShown="showCalendar" ID="CalendarExtendertxtValueBinDate" runat="server" Format="dd-MMM-yyyy" TargetControlID="txtFromDt" />
                                                        <br />
                                                    </div>
                                                    <div class="col-md-2">
                                                        <asp:Label ID="lblToDt" runat="server" Text="To Date"> </asp:Label>
                                                        <asp:TextBox ID="txtTodt" runat="server" Class="form-control" AutoPostBack="true" ></asp:TextBox>
                                                        <cc1:CalendarExtender OnClientShown="showCalendar" ID="CalendarExtender1" runat="server" Format="dd-MMM-yyyy" TargetControlID="txtTodt" />
                                                        <br />
                                                    </div>
                                                    <div class="col-lg-2">
                                                        <asp:Label ID="lblReferenceType" runat="server" Text="Reference Type"></asp:Label>
                                                        <asp:DropDownList ID="ddlReferenceType" runat="server" Class="form-control" OnSelectedIndexChanged="ddlReferenceType_SelectedIndexChanged" AutoPostBack="true">
                                                            <asp:ListItem Text="All" Value="All"></asp:ListItem>
                                                            <asp:ListItem Text="Campaign" Value="Campaign"></asp:ListItem>
                                                            <asp:ListItem Text="Call" Value="Call"></asp:ListItem>
                                                            <asp:ListItem Text="Visit" Value="Visit"></asp:ListItem>
                                                            <asp:ListItem Text="FollowUp" Value="FollowUp"></asp:ListItem>
                                                            <asp:ListItem Text="Opportunity" Value="Opportunity"></asp:ListItem>
                                                            <asp:ListItem Text="Quotation" Value="Quotation"></asp:ListItem>
                                                            <asp:ListItem Text="Sales Order" Value="Sales Order"></asp:ListItem>
                                                            <asp:ListItem Text="Sales Invoice" Value="Sales Invoice"></asp:ListItem>
                                                        </asp:DropDownList>
                                                    </div>
                                                    <div class="col-lg-2">
                                                        <br />

                                                        <asp:LinkButton ID="btnRefresh" runat="server" CausesValidation="False" OnClick="btnRefresh_Click"  ToolTip="<%$ Resources:Attendance,Refresh %>"><span class="fa fa-repeat"  style="font-size:25px;"></span></asp:LinkButton>
                                                        <br />

                                                    </div>

                                                    <div class="col-lg-3">
                                                        <asp:UpdatePanel ID="export" runat="server">
                                                            <Triggers>
                                                                <asp:PostBackTrigger ControlID="ExportExcel" />
                                                                <asp:PostBackTrigger ControlID="ExportPDF" />
                                                                 <asp:PostBackTrigger ControlID="EmportReportPDF" />
                                                            </Triggers>
                                                        </asp:UpdatePanel>
                                                        <br />
                                                        <asp:Button ID="ExportExcel" Visible="false"
                                                            runat="server" Text="Excel" OnClick="ExportExcel_Click" CssClass="btn btn-primary" />
                                                        <asp:Button ID="ExportPDF" Visible="false"
                                                            runat="server" Text="PDF" OnClick="ExportPDF_Click" CssClass="btn btn-primary" />
                                                        <asp:Button ID="EmportReportPDF"
                                                            runat="server" Text="History Report" OnClick="EmportReportPDF_Click" CssClass="btn btn-primary" />
                                                    </div>
                                              </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                               
                                <div class="box box-warning box-solid">
                                    <asp:HiddenField ID="hdnSalesLeadID" runat="server" />

                                    <div class="box-body">
                                        <div class="row">
                                            <div class="col-md-12">
                                                <div class="flow">
                                                    <dx:ASPxGridViewExporter ID="ASPxGridViewExporter1" runat="server" GridViewID="GvFollowupData"></dx:ASPxGridViewExporter>
                                                    <dx:ASPxGridView ID="GvFollowupData" ClientInstanceName="grid" runat="server" AutoGenerateColumns="False">
                                                        <Columns>
                                                            <dx:GridViewDataTextColumn FieldName="Trans_no" Settings-AutoFilterCondition="Contains" Caption="Transaction No" VisibleIndex="0">
                                                            </dx:GridViewDataTextColumn>
                                                            <dx:GridViewDataDateColumn Caption="Created Date" FieldName="Created_date"
                                                                ShowInCustomizationForm="True" VisibleIndex="1" PropertiesDateEdit-EditFormatString="dd-MMM-yyyy" PropertiesDateEdit-DisplayFormatString="dd-MMM-yyyy" ReadOnly="True">
                                                            </dx:GridViewDataDateColumn>

                                                            <dx:GridViewDataTextColumn FieldName="Customer_name" Settings-AutoFilterCondition="Contains" Caption="Customer" VisibleIndex="2">
                                                            </dx:GridViewDataTextColumn>
                                                            <dx:GridViewDataTextColumn FieldName="Remark" Settings-AutoFilterCondition="Contains" Caption="Remark" VisibleIndex="3">
                                                            </dx:GridViewDataTextColumn>
                                                            <dx:GridViewDataTextColumn FieldName="Amount" Caption="Amount" VisibleIndex="4">
                                                                <DataItemTemplate>
                                                                    <asp:Label ID="lblamt" runat="server" Text='<%# Common.GetAmountDecimal(Eval("Amount").ToString(),Session["DBConnection"].ToString(),Session["LocCurrencyId"].ToString()) %>'></asp:Label>
                                                                </DataItemTemplate>
                                                            </dx:GridViewDataTextColumn>
                                                            <dx:GridViewDataTextColumn FieldName="TableFrom" Settings-AutoFilterCondition="Contains" Caption="Type" VisibleIndex="5">
                                                            </dx:GridViewDataTextColumn>
                                                            <dx:GridViewDataTextColumn FieldName="Ref_table_name" Caption="Linked With" VisibleIndex="5" Settings-AutoFilterCondition="Contains">
                                                            </dx:GridViewDataTextColumn>
                                                            <dx:GridViewDataTextColumn FieldName="Ref_table_pk" Caption="Linked With No" VisibleIndex="6" Settings-AutoFilterCondition="Contains">
                                                            </dx:GridViewDataTextColumn>
                                                            <dx:GridViewDataTextColumn FieldName="Created_by_name" Settings-AutoFilterCondition="Contains" Caption="Created By" VisibleIndex="7">
                                                            </dx:GridViewDataTextColumn>
                                                        </Columns>
                                                        <Settings ShowGroupPanel="true" ShowFilterRow="true" />
                                                    </dx:ASPxGridView>


                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </div>
                </div>
            </div>
        </div>
    </div>

    <asp:UpdateProgress ID="UpdateProgress6" runat="server" AssociatedUpdatePanelID="Update_List">
        <ProgressTemplate>
            <div class="modal_Progress">
                <div class="center_Progress">
                    <img alt="" src="../Images/ajax-loader2.gif" style="vertical-align: middle; background-color: White; border-radius: 10px;" />
                </div>
            </div>
        </ProgressTemplate>
    </asp:UpdateProgress>


    <asp:UpdateProgress ID="UpdateProgress1" runat="server" AssociatedUpdatePanelID="export">
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
        }
        function LI_Edit_Active() {
            $("#Li_List").removeClass("active");
            $("#List").removeClass("active");

            $("#Li_New").addClass("active");
            $("#New").addClass("active");
        }
        function LI_Customer_Active() {
            $("#Li_Customer_Call").removeClass("active");
            $("#Customer_Call").removeClass("active");

            $("#Li_New").addClass("active");
            $("#New").addClass("active");
        }
        function LI_List_Active() {
            $("#Li_List").addClass("active");
            $("#List").addClass("active");

            $("#Li_New").removeClass("active");
            $("#New").removeClass("active");
        }
        function RedirectOpportunity(id) {
            window.open("../Sales/SalesInquiry.aspx?ReminderID=" + id);
        }

        function RedirectQuotation(id) {
            window.open("../Sales/SalesQuotation.aspx?ReminderID=" + id);
        }


    </script>
</asp:Content>


