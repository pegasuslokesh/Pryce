<%@ Page Title="" Language="C#" MasterPageFile="~/ERPMaster.master" AutoEventWireup="true" CodeFile="SalesInvoicePosting.aspx.cs" Inherits="Sales_SalesInvoicePosting" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Src="~/WebUserControl/AccountMaster.ascx" TagPrefix="uc1" TagName="AccountMaster" %>


<asp:Content ID="Content1" ContentPlaceHolderID="Head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="HeaderContent" runat="Server">
    <h1>
        <i class="fas fa-file-export"></i>&nbsp;&nbsp;
        <asp:Label ID="lblHeader" runat="server" Text="<%$ Resources:Attendance,Sales Invoice Posting %>"></asp:Label>
    </h1>
    <ol class="breadcrumb">
        <li><a href="#"><i class="fa fa-dashboard"></i>
            <asp:Label ID="Label6" runat="server" Text="<%$ Resources:Attendance,Inventory%>"></asp:Label></a></li>
        <li><a href="#"><i class="fa fa-dashboard"></i>
            <asp:Label ID="Label10" runat="server" Text="<%$ Resources:Attendance,Sales%>"></asp:Label></a></li>
        <li class="active">
            <asp:Label ID="Label5" runat="server" Text="<%$ Resources:Attendance,Sales Invoice Posting%>"></asp:Label></li>
    </ol>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="Server">

    <asp:UpdatePanel ID="Update_List" runat="server">
        <Triggers>
            <asp:PostBackTrigger ControlID="btnLoadSerial" />
        </Triggers>
        <ContentTemplate>
            <div class="row">
                <div class="col-md-12">
                    <div class="box box-primary">
                        <div class="box-body">
                            <div class="form-group">
                                <div class="col-md-6">
                                    <asp:DropDownList ID="ddlLocation" runat="server" Class="form-control" OnSelectedIndexChanged="ddlLocation_SelectedIndexChanged"
                                        AutoPostBack="true">
                                    </asp:DropDownList>
                                    <br />
                                </div>
                                <div class="col-md-6">
                                    <asp:DropDownList ID="ddlUser" runat="server" class="form-control" AutoPostBack="true" OnSelectedIndexChanged="ddlUser_Click">
                                    </asp:DropDownList>
                                    <br />
                                </div>

                                <div class="col-md-6">
                                    <asp:Label ID="Label27" runat="server" Text="<%$ Resources:Attendance,From Date %>"></asp:Label>
                                    <asp:TextBox ID="txtFromDate" runat="server" CssClass="form-control"></asp:TextBox>
                                    <cc1:CalendarExtender OnClientShown="showCalendar" ID="txtFrom_CalendarExtender" runat="server" Enabled="True"
                                        TargetControlID="txtFromDate">
                                    </cc1:CalendarExtender>
                                    <br />
                                </div>
                                <div class="col-md-6">
                                    <asp:Label ID="Label3" runat="server" Text="<%$ Resources:Attendance,To Date %>"></asp:Label>
                                    <asp:TextBox ID="txtToDate" runat="server" CssClass="form-control"></asp:TextBox>
                                    <cc1:CalendarExtender OnClientShown="showCalendar" ID="txtToDate_CalendarExtender" runat="server" Enabled="True" TargetControlID="txtToDate">
                                    </cc1:CalendarExtender>
                                    <br />
                                </div>
                                <div class="col-md-12" style="text-align: center">
                                    <br />
                                    <asp:Button ID="btngo" runat="server" CausesValidation="False" Text="<%$ Resources:Attendance,Go %>" Style="margin-top: -35px;"
                                        CssClass="btn btn-primary" OnClick="btngo_Click" />

                                    <asp:Button ID="btnResetSreach" runat="server" CausesValidation="False" Text="<%$ Resources:Attendance,Reset %>" Style="margin-top: -35px;"
                                        CssClass="btn btn-primary" OnClick="btnResetSreach_Click" />

                                    <asp:Button ID="btnPost" runat="server" OnClick="btnPost_Click" ValidationGroup="Save" Text="<%$ Resources:Attendance,Post%>" Style="margin-top: -35px;"
                                        CssClass="btn btn-primary" Visible="false" />
                                    <cc1:ConfirmButtonExtender ID="confirm1" runat="server" ConfirmText="Are you sure to post the record ?"
                                        TargetControlID="btnPost">
                                    </cc1:ConfirmButtonExtender>

                                    <asp:Button ID="btnunpostinvoice" runat="server" OnClick="btnunpostinvoice_Click" ValidationGroup="Save" Text="Stock Detail" Style="margin-top: -35px;"
                                        CssClass="btn btn-primary" />

                                    <asp:Button ID="btnLoadSerial" runat="server" CausesValidation="false"  OnClick="btnLoadSerial_Click" ValidationGroup="Save" Text="Load Serial" Style="margin-top: -35px;"
                                        CssClass="btn btn-primary" />

                                    <br />
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
            <div class="row">
                <div class="col-md-12">
                    <div id="Div1" runat="server" class="box box-info collapsed-box">
                        <div class="box-header with-border">
                            <h3 class="box-title">
                                <asp:Label ID="Label22" runat="server" Text="Advance Search"></asp:Label></h3>
                            &nbsp;&nbsp;|&nbsp;&nbsp;
				 <asp:Label ID="lblTotalRecords" Font-Bold="true" runat="server" Text="<%$ Resources:Attendance,Total Records: 0 %>"></asp:Label>

                            <div class="box-tools pull-right">
                                <button type="button" class="btn btn-box-tool" data-widget="collapse">
                                    <i id="I1" runat="server" class="fa fa-plus"></i>
                                </button>
                            </div>
                        </div>
                        <div class="box-body">
                            <div class="col-lg-2">
                                <asp:DropDownList ID="ddlPosted" runat="server" CssClass="form-control" AutoPostBack="true" OnSelectedIndexChanged="ddlPosted_SelectedIndexChanged">
                                    <asp:ListItem Text="<%$ Resources:Attendance,Posted %>" Value="Posted"></asp:ListItem>
                                    <asp:ListItem Text="<%$ Resources:Attendance,UnPosted %>" Value="UnPosted" Selected="True"></asp:ListItem>
                                    <asp:ListItem Text="<%$ Resources:Attendance,All %>" Value="0"></asp:ListItem>
                                </asp:DropDownList>
                            </div>
                            <div class="col-lg-2">
                                <asp:DropDownList ID="ddlFieldName" runat="server" CssClass="form-control">
                                    <asp:ListItem Text="<%$ Resources:Attendance, Invoice No %>" Value="Invoice_No" Selected="True"></asp:ListItem>
                                    <asp:ListItem Text="<%$ Resources:Attendance,Sales Person %>" Value="EmployeeName"></asp:ListItem>
                                    <asp:ListItem Text="<%$ Resources:Attendance, Customer Name %>" Value="CustomerName"></asp:ListItem>
                                    <asp:ListItem Text="<%$ Resources:Attendance,Serial No %>" Value="SerialNo"></asp:ListItem>
                                    <asp:ListItem Text="<%$ Resources:Attendance,Created By %>" Value="InvoiceCreatedBy"></asp:ListItem>
                                </asp:DropDownList>
                            </div>
                            <div class="col-lg-2">
                                <asp:DropDownList ID="ddlOption" runat="server" CssClass="form-control">
                                    <asp:ListItem Text="<%$ Resources:Attendance,--Select-- %>"></asp:ListItem>
                                    <asp:ListItem Text="<%$ Resources:Attendance,Equal %>"></asp:ListItem>
                                    <asp:ListItem Selected="True" Text="<%$ Resources:Attendance,Contains %>"></asp:ListItem>
                                    <asp:ListItem Text="<%$ Resources:Attendance,Like %>"></asp:ListItem>
                                </asp:DropDownList>
                            </div>
                            <div class="col-lg-4">
                                <asp:Panel ID="Panel1" runat="server" DefaultButton="btnbind">
                                    <asp:TextBox placeholder="Search From Content" ID="txtValue" runat="server" CssClass="form-control"></asp:TextBox>
                                </asp:Panel>
                            </div>
                            <div class="col-lg-2">
                                <asp:LinkButton ID="btnbind" runat="server" CausesValidation="False" OnClick="btnbindrpt_Click" ToolTip="<%$ Resources:Attendance,Search %>"><span class="fa fa-search"  style="font-size:25px;"></span></asp:LinkButton>&nbsp;&nbsp;&nbsp;
                                <asp:LinkButton ID="btnRefresh" runat="server" CausesValidation="False" OnClick="btnRefreshReport_Click" ToolTip="<%$ Resources:Attendance,Refresh %>"><span class="fa fa-repeat"  style="font-size:25px;"></span></asp:LinkButton>&nbsp;&nbsp;&nbsp;
                                <asp:LinkButton ID="ImgbtnSelectAll" Visible="false" runat="server" ToolTip="<%$ Resources:Attendance, Select All %>" OnClick="ImgbtnSelectAll_Click"><span class="fas fa-th"  style="font-size:25px;"></span></asp:LinkButton>
                            </div>

                        </div>
                    </div>
                </div>
            </div>

            <div class="box box-warning box-solid" <%= GvSalesInvoice.Rows.Count>0?"style='display:block'":"style='display:none'"%>>
                <div class="box-body">
                    <div class="row">
                        <div class="col-md-12">
                            <asp:CustomValidator ID="CustomValidator1" ValidationGroup="Save" runat="server" ErrorMessage="Please select at least one record."
                                ClientValidationFunction="Validate_Grid" ForeColor="Red"></asp:CustomValidator>
                            <br />
                            <div class="flow">
                                <asp:GridView CssClass="table-striped table-bordered table table-hover" ID="GvSalesInvoice" runat="server" AutoGenerateColumns="False" Width="100%"
                                    AllowPaging="true" PageSize="<%# PageControlCommon.GetPageSize() %>" DataKeyNames="Trans_Id"
                                    AllowSorting="true" OnSorting="GvSalesInvoice_Sorting" OnPageIndexChanging="GvSalesInvoice_PageIndexChanging">
                                    <Columns>

                                        <asp:TemplateField>
                                            <HeaderTemplate>
                                                <asp:CheckBox ID="chkgvSelectAll" runat="server" onchange="chkgvSelectAll_CheckedChanged(this)" />
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <asp:CheckBox ID="chkgvSelect" runat="server" />
                                                <input type="hidden" value="" />
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Center" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ Resources:Attendance,Invoice No. %>" SortExpression="Invoice_No">
                                            <ItemTemplate>
                                                <a title="View Invoice Detail" style="cursor: pointer" onclick="lblgvSInvNo_Command('<%# Eval("Trans_Id") %>')"><%# Eval("Invoice_No") %></a>
                                                <%--<asp:LinkButton ID="lblgvSInvNo" runat="server" Text='<%#Eval("Invoice_No") %>' CommandArgument='<%# Eval("Trans_Id") %>' ForeColor="Blue" Font-Underline="true" OnCommand="lblgvSInvNo_Command" ToolTip="View Invoice Detail" />--%>
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Left" Width="100px" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ Resources:Attendance,Invoice Date %>" SortExpression="Invoice_Date">
                                            <ItemTemplate>
                                                <%#Convert.ToDateTime(Eval("Invoice_Date").ToString()).ToString("dd-MMM-yyyy")%>
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Left" Width="80px" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ Resources:Attendance,Order No. %>" SortExpression="OrderList">
                                            <ItemTemplate>
                                                <%#Eval("OrderList") %>
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Center" Width="90px" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ Resources:Attendance,Sales Person %>" SortExpression="EmployeeName">
                                            <ItemTemplate>
                                                <%#Eval("EmployeeName") %>
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Center" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ Resources:Attendance,Customer Name%>" SortExpression="CustomerName">
                                            <ItemTemplate>
                                                <table width="100%">
                                                    <tr>
                                                        <td width="80%">

                                                            <asp:LinkButton ID="lnkCustomerAccountMaster" Text='<%# Eval("CustomerName") %>' runat="server" CommandArgument='<%# Eval("Customer_Id") %>'
                                                                OnCommand="lnkCustomerAccountMaster_Command" ToolTip="Account Master" />
                                                            <%--<asp:Label ID="lblgvCustomerName" runat="server" Text='<%#Eval("CustomerName") %>' />--%>

                                                        </td>
                                                        <td align="right" width="20%">
                                                            <a onclick="lblgvCustomerName_Command('<%# Eval("Customer_Id") %>')" title="Customer History" style="cursor: pointer; color: blue">More..</a>
                                                            <%--<asp:LinkButton ID="lnkcustomerdetail" runat="server" Text="More.." ForeColor="Blue" CommandArgument='<%# Eval("Customer_Id") %>' OnCommand="lblgvCustomerName_Command" ToolTip="Customer History" />--%>
                                                        </td>
                                                    </tr>

                                                </table>
                                            </ItemTemplate>
                                            <FooterStyle HorizontalAlign="Left" />
                                            <ItemStyle HorizontalAlign="Left" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ Resources:Attendance,Status%>" SortExpression="Field4">
                                            <ItemTemplate>
                                                <%# Eval("Field4").ToString() %>
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Left" />
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="<%$ Resources:Attendance,Created By%>" SortExpression="InvoiceCreatedBy">
                                            <ItemTemplate>
                                                <%#Eval("InvoiceCreatedBy")%>
                                            </ItemTemplate>
                                            <FooterStyle HorizontalAlign="Center" />
                                            <ItemStyle HorizontalAlign="Center" />
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="<%$ Resources:Attendance,Amount%>" SortExpression="GrandTotal">
                                            <ItemTemplate>
                                                <%# GetCurrencySymbol(Eval("GrandTotal").ToString(),Eval("Currency_Id").ToString()) %>
                                            </ItemTemplate>
                                            <FooterStyle HorizontalAlign="Center" />
                                            <ItemStyle HorizontalAlign="Right" />
                                        </asp:TemplateField>
                                    </Columns>


                                    <PagerStyle CssClass="pagination-ys" />

                                </asp:GridView>
                            </div>
                            <br />
                            <div style="text-align: right">
                                <asp:Label ID="lblTotalAmount_List" runat="server" Text="<%$ Resources:Attendance,Total %>"
                                    CssClass="labelComman"></asp:Label>
                            </div>
                        </div>
                        <asp:HiddenField runat="server" ID="hdnCheckedValue" />
                    </div>
                </div>
                <!-- /.box-body -->
            </div>
            <asp:UpdatePanel ID="up1" runat="server">
                <ContentTemplate>
                    <asp:Button ID="Btn_ucAcMaster" Style="display: none;" runat="server" data-toggle="modal" data-target="#ModelAcMaster" Text="Add Accounts Detail" />

                </ContentTemplate>
            </asp:UpdatePanel>

        </ContentTemplate>
    </asp:UpdatePanel>
    <div class="modal fade" id="ModelAcMaster" tabindex="-1" role="dialog" aria-labelledby="myModalLabel"
        aria-hidden="true">
        <div class="modal-dialog modal-lg">
            <div class="modal-content">
                <div class="modal-body">
                    <uc1:AccountMaster runat="server" ID="UcAcMaster" />
                </div>
                <div class="modal-footer">
                    <button type="button" id="" class="btn btn-danger" data-dismiss="modal">
                        Close</button>
                </div>
            </div>
        </div>
    </div>

    <asp:UpdateProgress ID="UpdateProgress1" runat="server" AssociatedUpdatePanelID="Update_List">
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
<asp:Content ID="Content5" ContentPlaceHolderID="AfterLoadScriptContent" runat="Server">
    <script src="../Script/common.js"></script>
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
        function Validate_Grid(sender, args) {
            var gridView = document.getElementById("<%=GvSalesInvoice.ClientID %>");
            var checkBoxes = gridView.getElementsByTagName("input");
            for (var i = 0; i < checkBoxes.length; i++) {
                if (checkBoxes[i].type == "checkbox" && checkBoxes[i].checked) {
                    args.IsValid = true;
                    return;
                }
            }
            args.IsValid = false;
        }
        function Modal_AcMaster_Open() {
            document.getElementById('<%= Btn_ucAcMaster.ClientID %>').click();
        }

        function lblgvSInvNo_Command(invoiceID) {
            PageMethods.hasSalesInvoicePermission(function (data) {
                if (!data) {
                    alert("User have no permission to view sales invoice ");
                    return;
                }
                else {
                    window.open("../Sales/SalesInvoice.aspx?Id=" + invoiceID + "");
                }
            });
        }
        function lblgvCustomerName_Command(customerId) {
            window.open('../Purchase/CustomerHistory.aspx?ContactId=' + customerId + '&&Page=SINV', 'window', 'width=1024, ');
        }

        function lnkCustomerAccountMaster_Command(customerName, customerId) {
            PageMethods.lnkCustomerAccountMaster_Command(customerId, customerName, function (data) {
                if (data) {
                    $('#Modal_AcMaster_Open').modal('show');
                }
                else {
                    $('#Modal_AcMaster_Open').modal('hide');
                }
            });
        }

        function chkgvSelectAll_CheckedChanged(ctrl) {
            var count = 0;
            $('#<%=GvSalesInvoice.ClientID%> tbody tr').each(function () {
                if (count % 2 != 0) {
                    var row = $(this);
                    if (row[0].cells[0].childNodes[1] != undefined) {
                        row[0].cells[0].childNodes[1].checked = ctrl.childNodes[0].checked;
                    }
                }
                count++;
            });
        }

        $(document).ready(function () {
            getSelectedInvoice();
        });

        function getSelectedInvoice() {
            debugger;
            var count = 0;
            var page = $('#<%=GvSalesInvoice.ClientID%> tbody tr:last td span').innerHTML;

            var hdnvalue = $('#<%=hdnCheckedValue.ClientID%>');

            var checkedItem = new Array();

            $('#<%=GvSalesInvoice.ClientID%> tbody tr').each(function () {
                if (count % 2 != 0) {
                    var row = $(this);
                    if (row[0].cells[0].childnodes[1] != undefined) {
                        row[0].cells[0].childnodes[1].checked = ctrl.childnodes[0].checked;
                    }
                }
                count++;
            });
        }
        function redirectToHome(msg) {
            if (confirm(msg)) {
                window.open('../MasterSetup/Home.aspx', 'window', 'width=1024,');
                return true;
            }
            else {
                return false;
            }
        }
    </script>
</asp:Content>

