<%@ Page Title="" Language="C#" MasterPageFile="~/ERPMaster.master" AutoEventWireup="true" CodeFile="BackOrder.aspx.cs" Inherits="Sales_BackOrder" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Src="~/WebUserControl/ContactInfo.ascx" TagName="ViewContact" TagPrefix="AT1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="Head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="HeaderContent" runat="Server">
    <h1>
        <i class="far fa-file-alt"></i>&nbsp;&nbsp;
        <asp:Label ID="lblHeader" runat="server" Text="<%$ Resources:Attendance,Sales Back Order%>"></asp:Label>
    </h1>
    <ol class="breadcrumb">
        <li><a href="#"><i class="fa fa-dashboard"></i>
            <asp:Label ID="Label6" runat="server" Text="<%$ Resources:Attendance,Inventory%>"></asp:Label></a></li>
        <li><a href="#"><i class="fa fa-dashboard"></i>
            <asp:Label ID="Label10" runat="server" Text="<%$ Resources:Attendance,Sales%>"></asp:Label></a></li>
        <li class="active">
            <asp:Label ID="Label5" runat="server" Text="<%$ Resources:Attendance,Sales Back Order%>"></asp:Label></li>
    </ol>
    <script>
        function resetPosition1() {

        }
    </script>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="Server">

    <asp:UpdatePanel ID="Update_List" runat="server">
        <ContentTemplate>

            <asp:Button ID="Btn_CustomerInfo_Modal" Style="display: none;" runat="server" data-toggle="modal" data-target="#modelContactDetail" />


            <div id="PnlList" runat="server">
                <div class="row">
                    <div class="col-md-12">
                        <div class="box box-primary">
                            <div class="box-body">
                                <div class="form-group">
                                    <div class="col-md-6">
                                        <asp:Label ID="lblBrandsearch" runat="server" Text="Customer Name"></asp:Label>
                                        <asp:TextBox ID="txtCustomer" runat="server" CssClass="form-control" ClientIDMode="Static"
                                            BackColor="#eeeeee" onchange="getContactIdFromNameNIdNSetSession(this,'ContactID')" />

                                        <cc1:AutoCompleteExtender ID="txtCustomerName_AutoCompleteExtender" runat="server"
                                            DelimiterCharacters="" Enabled="True" CompletionInterval="100" CompletionSetCount="1"
                                            MinimumPrefixLength="1" ServiceMethod="GetCompletionListCustomer" ServicePath=""
                                            TargetControlID="txtCustomer" UseContextKey="True" CompletionListCssClass="autoCompleteList" CompletionListItemCssClass="autoCompleteListItem" CompletionListHighlightedItemCssClass="autoCompleteSelectedListItem" OnClientShown="resetPosition">
                                        </cc1:AutoCompleteExtender>
                                        <br />
                                    </div>
                                    <div class="col-md-6">
                                        <asp:Label ID="Label8" runat="server" Text="Location" />
                                        <asp:DropDownList ID="ddlLocation" runat="server" CssClass="form-control" OnSelectedIndexChanged="ddlLocation_SelectedIndexChanged" AutoPostBack="true"></asp:DropDownList>
                                        <br />
                                    </div>
                                    <asp:UpdatePanel runat="server" ID="upButtons">
                                        <Triggers>
                                            <asp:PostBackTrigger ControlID="btnExportExcel" />
                                        </Triggers>
                                        <ContentTemplate>
                                            <div class="col-md-12">
                                                <asp:Button ID="btngo" runat="server" CausesValidation="False" Text="<%$ Resources:Attendance,Get Order %>"
                                                    CssClass="btn btn-primary" OnClick="btngo_Click" />
                                                <asp:Button ID="btnAddCustomer" runat="server" CssClass="btn btn-primary" OnClick="btnAddCustomer_Click"
                                                    Text="Add New Contact Person" CausesValidation="False" />
                                                <asp:Button ID="btnReset" runat="server" CausesValidation="False" Text="<%$ Resources:Attendance,Reset %>"
                                                    CssClass="btn btn-primary" OnClick="btnReset_Click" />
                                                <asp:HiddenField ID="hdnCheckedValues" runat="server" ClientIDMode="Static" />
                                                <a class="btn btn-primary" id="btnSendEmail" onclick="sendEmailClick();" style="cursor: pointer">Send Email</a>
                                                <asp:Button ID="btnExportExcel" runat="server" CausesValidation="False" Text="Export to Excel"
                                                    CssClass="btn btn-primary" OnClick="btnExportExcel_Click" />

                                                <br />
                                            </div>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                    <div class="col-md-12">
                                        <br />
                                    </div>
                                    <asp:HiddenField runat="server" ID="hdnEmailIDS" ClientIDMode="Static" />
                                    <div class="col-md-12" runat="server" id="divEmpDetail" visible="false">
                                        <div class="box box-primary collapsed-box">
                                            <div class="box-header with-border">
                                                <h3 class="box-title">
                                                    <asp:Label ID="Label11" runat="server" Font-Bold="true" Text="Contact Person List"></asp:Label>
                                                    &nbsp : &nbsp
                                            <asp:Label ID="Label12" runat="server"></asp:Label></h3>
                                                <div class="box-tools pull-right">
                                                    <button type="button" class="btn btn-box-tool" data-widget="collapse"><i class="fa fa-plus"></i></button>
                                                </div>
                                            </div>
                                            <div class="box-body">
                                                <div class="row">

                                                    <div class="col-lg-12">
                                                        <table id="tblContactEmail" class="table table-striped" style="width: 100%">
                                                            <thead>
                                                                <tr>
                                                                    <td></td>
                                                                    <td>Contact Person</td>
                                                                    <td>Email</td>
                                                                    <td>Mobile</td>
                                                                </tr>
                                                            </thead>
                                                            <tbody>
                                                            </tbody>
                                                        </table>
                                                    </div>

                                                </div>
                                            </div>
                                        </div>
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
				<asp:Label ID="lblTotalRecords" runat="server" Font-Bold="true" Text="<%$ Resources:Attendance,Total Records: 0 %>"></asp:Label>

                                <div class="box-tools pull-right">
                                    <button type="button" class="btn btn-box-tool" data-widget="collapse">
                                        <i id="I1" runat="server" class="fa fa-plus"></i>
                                    </button>
                                </div>
                            </div>
                            <div class="box-body">
                                <div class="col-lg-3">
                                    <asp:DropDownList ID="ddlFieldName" runat="server" CssClass="form-control" OnSelectedIndexChanged="ddlFieldName_SelectedIndexChanged" AutoPostBack="true">
                                        <asp:ListItem Text="<%$ Resources:Attendance, Order No. %>" Value="SalesOrderNo" Selected="True"></asp:ListItem>
                                        <asp:ListItem Text="<%$ Resources:Attendance, Order Date %>" Value="SalesOrderDate"></asp:ListItem>
                                        <asp:ListItem Text="Customer Name" Value="CustomerName"></asp:ListItem>
                                        <asp:ListItem Text="Customer Exp Delivery Date" Value="EstimateDeliveryDate"></asp:ListItem>
                                        <asp:ListItem Text="<%$ Resources:Attendance,Product Id %>" Value="ProductCode"></asp:ListItem>
                                        <asp:ListItem Text="<%$ Resources:Attendance,Product Name %>" Value="EProductName"></asp:ListItem>
                                        <asp:ListItem Text="<%$ Resources:Attendance,Created By %>" Value="CreatedUser"></asp:ListItem>
                                        <asp:ListItem Text="<%$ Resources:Attendance,Modified By %>" Value="ModifiedUser"></asp:ListItem>
                                    </asp:DropDownList>
                                    <br />
                                </div>
                                <div class="col-lg-2">
                                    <asp:DropDownList ID="ddlOption" runat="server" CssClass="form-control">
                                        <asp:ListItem Text="<%$ Resources:Attendance,--Select-- %>"></asp:ListItem>
                                        <asp:ListItem Text="<%$ Resources:Attendance,Equal %>"></asp:ListItem>
                                        <asp:ListItem Selected="True" Text="<%$ Resources:Attendance,Contains %>"></asp:ListItem>
                                        <asp:ListItem Text="<%$ Resources:Attendance,Like %>"></asp:ListItem>
                                    </asp:DropDownList>
                                    <br />
                                </div>
                                <div class="col-lg-5">
                                    <asp:Panel ID="Panel1" runat="server" DefaultButton="btnbind">
                                        <asp:TextBox ID="txtValue" runat="server" CssClass="form-control" placeholder="Search From Content"></asp:TextBox>
                                        <asp:TextBox ID="txtValueDate" runat="server" CssClass="form-control" Visible="false" placeholder="Search From Date"></asp:TextBox>
                                        <cc1:CalendarExtender OnClientShown="showCalendar" Format="dd-MMM-yyyy" ID="CalendarExtendertxtValueDate" runat="server" TargetControlID="txtValueDate" />
                                    </asp:Panel>
                                    <br />
                                </div>
                                <div class="col-lg-2">
                                    <asp:LinkButton ID="btnbind" runat="server" CausesValidation="False" OnClick="btnbindrpt_Click" ToolTip="<%$ Resources:Attendance,Search %>"><span class="fa fa-search"  style="font-size:25px;"></span></asp:LinkButton>&nbsp;&nbsp;&nbsp;
                                    <asp:LinkButton ID="btnRefresh" runat="server" CausesValidation="False" OnClick="btnRefreshReport_Click" ToolTip="<%$ Resources:Attendance,Refresh %>"><span class="fa fa-repeat"  style="font-size:25px;"></span></asp:LinkButton>&nbsp;&nbsp;&nbsp;
                                    <br />
                                </div>

                            </div>
                        </div>
                    </div>
                </div>
                <div class="box box-warning box-solid" <%= GvBackOrder.Rows.Count>0?"style='display:block'":"style='display:none'"%>>
                    <div class="box-body">
                        <div class="row">
                            <div class="col-md-12">
                                <div style="overflow: auto; max-height: 350px">
                                    <asp:GridView CssClass="table-striped table-bordered table table-hover" ID="GvBackOrder" PageSize='<%# int.Parse(Session["GridSize"].ToString()) %>' runat="server"
                                        AutoGenerateColumns="False" Width="100%" AllowPaging="false" OnPageIndexChanging="GvBackOrder_PageIndexChanging"
                                        AllowSorting="True" OnSorting="GvBackOrder_Sorting">

                                        <Columns>
                                            <asp:TemplateField>
                                                <ItemTemplate>
                                                    <asp:CheckBox ID="chkSelect" runat="server" CausesValidation="False" onchange="getCheckBoxValue(this)" />
                                                    <asp:HiddenField runat="server" Value='<%# Eval("Trans_Id") %>' />
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Center" />
                                            </asp:TemplateField>

                                            <asp:TemplateField Visible="false">
                                                <ItemTemplate>
                                                    <asp:LinkButton ToolTip="<%$ Resources:Attendance,Edit %>" ID="btnEdit" runat="server" CommandArgument='<%# Eval("Trans_Id") %>' OnCommand="btnEdit_Command" CausesValidation="False"><i class="fa fa-pencil" style="font-size:15px"></i></asp:LinkButton>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Center" />
                                            </asp:TemplateField>

                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,Customer Name %>" SortExpression="CustomerName">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblgvCustomer" runat="server" Text='<%#Eval("CustomerName") %>' />
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Left" />
                                            </asp:TemplateField>


                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,Order No. %>" SortExpression="SalesOrderNo">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblgvSOID" runat="server" Text='<%#Eval("SOID") %>' Visible="false" />
                                                    <asp:Label ID="lblgvSONo" runat="server" Text='<%#Eval("SalesOrderNo") %>' Visible="false" />
                                                    <a style="cursor: pointer" onclick="openOrder('<%#Eval("SOID") %>')"><%#Eval("SalesOrderNo") %></a>

                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Center" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,Order Date %>" SortExpression="SalesOrderDate">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblgvSODate" runat="server" Text='<%#GetDate(Eval("SalesOrderDate").ToString()) %>' />
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Center" Width="80px" />
                                            </asp:TemplateField>


                                            <asp:TemplateField HeaderText="Purchase Order No" SortExpression="PONO">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblgvPONO" runat="server" Text='<%#Eval("PONO") %>' />
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Center" />
                                            </asp:TemplateField>

                                            <asp:TemplateField HeaderText="Purchase Exp Delivery Date" SortExpression="DeliveryDate">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblgvPurchaseDeliveryDate" runat="server" Text='<%# Eval("DeliveryDate").ToString()==""?"":Convert.ToDateTime(Eval("DeliveryDate").ToString()).ToString("dd-MMM-yyyy") %>' />
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Center" Width="80px" />
                                            </asp:TemplateField>

                                            <asp:TemplateField HeaderText="Customer Exp Delivery Date" SortExpression="EstimateDeliveryDate">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblgvDeliveryDate" runat="server" Text='<%#GetDate(Eval("EstimateDeliveryDate").ToString()) %>' />
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Center" Width="80px" />
                                            </asp:TemplateField>


                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,Product Id%>" SortExpression="ProductCode">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblgvproductid" runat="server" Text='<%#Eval("ProductCode") %>' />
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Left" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,Product Name%>" SortExpression="EProductName">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblgvproductname" runat="server" Text='<%#Eval("EProductName") %>' />
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Left" />
                                            </asp:TemplateField>

                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,Unit Name%>" SortExpression="Unit_Name">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblunitname" runat="server" Text='<%# Eval("Unit_Name").ToString() %>' />
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Left" />
                                            </asp:TemplateField>

                                            <asp:TemplateField HeaderText="Order Qty">
                                                <ItemTemplate>
                                                    <asp:Label runat="server" Text='<%# GetAmountDecimal(Eval("OrderQty").ToString()) %>' />
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Right" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Delivered Qty">
                                                <ItemTemplate>
                                                    <asp:Label runat="server" Text='<%# GetAmountDecimal(Eval("DeliveredQty").ToString()) %>' />
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Right" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Balanced Qty" SortExpression="BackOrderQty">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblqty" runat="server" Text='<%# GetAmountDecimal(Eval("BackOrderQty").ToString()) %>' />
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Right" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="System Qty" SortExpression="quantity">
                                                <ItemTemplate>
                                                    <a style="cursor: pointer" onclick="lnkStockInfo_Command1('<%#Eval("product_Id") %>')"><%# GetAmountDecimal(Eval("quantity").ToString()) %></a>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Right" />
                                            </asp:TemplateField>


                                            <asp:TemplateField HeaderText="Remarks">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblNotes" runat="server" Text='<%# Eval("field4") %>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Left" Width="10%" />
                                            </asp:TemplateField>

                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,Modified By %>" SortExpression="ModifiedUser">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblModified_User" runat="server" Text='<%# Eval("ModifiedUser") %>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Left" Width="10%" />
                                            </asp:TemplateField>

                                        </Columns>

                                        <PagerStyle CssClass="pagination-ys" />

                                    </asp:GridView>
                                    <asp:HiddenField ID="hdnTransId" runat="server" />
                                    <asp:HiddenField ID="hdnSoId" runat="server" />
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
            <div id="PnlNewEdit" runat="server" visible="false">
                <div class="row">
                    <div class="col-md-12">
                        <div class="box box-primary">
                            <div class="box-body">
                                <div class="form-group">
                                    <div class="col-md-4">
                                        <asp:Label ID="lblSODate" runat="server" Text="<%$ Resources:Attendance,Order No. %>"></asp:Label>
                                        <asp:TextBox ID="txtsoNo" runat="server" CssClass="form-control" ReadOnly="true" />
                                        <br />
                                    </div>
                                    <div class="col-md-4">
                                        <asp:Label ID="lblSONo" runat="server" Text="<%$ Resources:Attendance,Order Date %>"></asp:Label>
                                        <asp:TextBox ID="txtSODate" runat="server" CssClass="form-control" ReadOnly="true" />
                                        <br />
                                    </div>
                                    <div class="col-md-4">
                                        <asp:Label ID="Label4" runat="server" Text="Customer Expected Delivery Date"></asp:Label>
                                        <asp:TextBox ID="txtCustExpDeliveryDate" runat="server" CssClass="form-control" />
                                        <cc1:CalendarExtender OnClientShown="showCalendar" Format="dd-MMM-yyyy" ID="CalendarExtender1" runat="server" TargetControlID="txtCustExpDeliveryDate" />
                                        <br />
                                    </div>
                                    <div class="col-md-3">
                                        <asp:Label ID="Label7" runat="server" Text="Product Id"></asp:Label>
                                        <asp:TextBox ID="txtProductCode" runat="server" CssClass="form-control" ReadOnly="true" />
                                        <br />
                                    </div>
                                    <div class="col-md-9">
                                        <asp:Label ID="Label1" runat="server" Text="<%$ Resources:Attendance,Product name%>"></asp:Label>
                                        <asp:TextBox ID="txtprouctname" runat="server" CssClass="form-control" ReadOnly="true" />
                                        <br />
                                    </div>
                                    <div class="col-md-6">
                                        <asp:Label ID="Label2" runat="server" Text="<%$ Resources:Attendance,Unit name%>"></asp:Label>
                                        <asp:TextBox ID="txtUnitName" runat="server" CssClass="form-control" ReadOnly="true" />
                                        <br />
                                    </div>
                                    <div class="col-md-6">
                                        <asp:Label ID="Label3" runat="server" Text="Back Order Quantity"></asp:Label>
                                        <asp:TextBox ID="txtquantity" runat="server" CssClass="form-control" />
                                        <asp:HiddenField ID="hdnQuantity" runat="server" />
                                        <br />
                                    </div>
                                    <div class="col-md-12">
                                        <asp:TextBox ID="txtDescription" runat="server" CssClass="form-control" TextMode="MultiLine" placeholder="Remarks" />
                                        <br />
                                    </div>

                                    <div class="col-md-12" style="text-align: center">
                                        <asp:CheckBox ID="chkSelectall" runat="server" Text="Update for all items" />
                                        <asp:Button ID="btnSOrderSave" runat="server" Text="<%$ Resources:Attendance,Save %>" Visible="false"
                                            CssClass="btn btn-success" OnClick="btnSOrderSave_Click" />&nbsp;&nbsp;&nbsp;
                                                                      <asp:Button ID="btnSOrderCancel" runat="server" CssClass="btn btn-danger" Text="<%$ Resources:Attendance,Cancel %>"
                                                                          CausesValidation="False" OnClick="btnSOrderCancel_Click" />
                                        <asp:HiddenField ID="editid" runat="server" />
                                        <br />
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>

    <asp:UpdateProgress ID="UpdateProgress1" runat="server" AssociatedUpdatePanelID="Update_List">
        <ProgressTemplate>
            <div class="modal_Progress">
                <div class="center_Progress">
                    <img alt="" src="../Images/ajax-loader2.gif" style="vertical-align: middle; background-color: White; border-radius: 10px;" />
                </div>
            </div>
        </ProgressTemplate>
    </asp:UpdateProgress>

    <div class="modal fade" id="modelContactDetail" tabindex="-1" role="dialog" aria-labelledby="myModalLabel"
        aria-hidden="true">
        <div class="modal-dialog modal-lg">
            <div class="modal-content">

                <div class="modal-body">
                    <AT1:ViewContact ID="UcContactList" runat="server" />
                </div>


                <div class="modal-footer">
                    <button type="button" id="" class="btn btn-danger" data-dismiss="modal">
                        Close</button>
                </div>
            </div>
        </div>
    </div>

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

        function openOrder(so) {
            window.open('../Sales/SalesOrder1.aspx?Id=' + so + '', '_blank', 'width=1024, ');
        }

        function getCheckBoxValue(chk) {
            var checkedValue = $('#<%=hdnCheckedValues.ClientID%>').val();
            var customerValidate = '';
            var validate = '0';

            var soID = chk.offsetParent.children[1].value;

            if (chk.children[0].checked == true) {
                checkedValue += soID + ',';
            }
            else {
                try {
                    var newList = '';
                    var empList = checkedValue.split(',');
                    if (empList.length > 0) {
                        for (var i = 0; i < empList.length - 1 ; i++) {
                            if (empList[i] == soID) {
                                continue;
                            }
                            else {
                                newList += empList[i] + ',';
                            }
                        }
                    }
                }
                catch (data) {

                }
                checkedValue = newList;
            }
            $('#<%=hdnCheckedValues.ClientID%>').val(checkedValue);
        }

        function data() {
            debugger;
            var customerValidate = '';
            var validate = '0';
            var check = true;
            var header = "0";
            $('#<%=GvBackOrder.ClientID%> tbody tr').each(function () {
                if (header == "0") {
                    header = "1";
                }
                else {
                    if (validate == '0') {
                        var row = $(this);
                        if (row[0].className != "Invgridheader" && row[0].className != 'pagination-ys') {
                            var checkbox = row[0].cells[0].children[0].childNodes[0];
                            if (checkbox.checked == true) {
                                debugger;
                                if (customerValidate == '') {
                                    customerValidate = row[0].cells[5].innerText;
                                }
                                else {
                                    if (customerValidate != row[0].cells[5].innerText) {
                                        alert('Please Select Single Order Record To Send Email');
                                        validate = '1';
                                        check = false;
                                    }
                                }
                            }
                        }
                    }
                }
            });
            return check;
        }
        function getEmpData() {
            debugger;
            var custid = setCustomerId();
            if (custid == "") {
                alert("Please select customer id");
                return false;
            }
            PageMethods.getEmployeeData(custid, function (data) {
                var vico = JSON.parse(data);
                $(vico).each(function () {
                    var row = $(this)[0];
                    var htmlRow = "<tr><td><input type='checkbox' /></td>";
                    htmlRow = htmlRow + "<td>" + row.Name + "</td>";
                    htmlRow = htmlRow + "<td>" + row.email + "</td>";
                    htmlRow = htmlRow + "<td>" + row.contact + "</td>";
                    htmlRow = htmlRow + "</tr>";
                    $('#tblContactEmail > tbody:last-child').append(htmlRow);
                });
            }, function (data) { alert(data); });
            return true;
        }
        function getEmpData(jsondata) {
            var vico = JSON.parse(jsondata);
            $(vico).each(function () {
                var row = $(this)[0];
                var htmlRow = "<tr><td><input type='checkbox' onchange='enableSendMail()' /></td>";
                htmlRow = htmlRow + "<td>" + row.Name + "</td>";
                htmlRow = htmlRow + "<td>" + row.email + "</td>";
                htmlRow = htmlRow + "<td>" + row.contact + "</td>";
                htmlRow = htmlRow + "</tr>";
                $('#tblContactEmail > tbody:last-child').append(htmlRow);
            });
        }

        function setCustomerId() {
            var ctl = $('#txtCustomer').val();
            if (ctl == "") {
                return "";
            }
            return ctl.split('/')[1];
        }
        function enableSendMail() {
            document.getElementById('btnSendEmail').disabled = true;
            var emailIDS = '';
            $('#tblContactEmail tbody tr').each(function () {
                var row = $(this)[0].cells;
                if (row[0].children[0].checked) {
                    document.getElementById('btnSendEmail').disabled = false;
                    emailIDS += row[2].innerText + ';';
                }
            });
            $('#hdnEmailIDS').val(emailIDS);
        }

        function sendEmailClick() {
            debugger;
            if (!data()) {
                return;
            }

            var hdnEmailIDS = $('#hdnEmailIDS').val();
            var hdnCheckedValues = $('#hdnCheckedValues').val();

            if (hdnEmailIDS.length < 4) {
                alert("Please select contact person to send the email");
                return;
            }
            if (hdnCheckedValues == "") {
                alert("Please select record to send the mail");
                return;
            }
            PageMethods.btnSendEmail_Click(hdnCheckedValues, hdnEmailIDS, function (data) {
                if (data == "1") {
                    window.open('../EmailSystem/SendMail.aspx?HD=1', '_blank', 'width=1024, ');
                }
                else {
                    alert('something went worng please try after some time');
                }
            }, function (data) { alert(data); });
        }
        function Modal_CustomerInfo_Open() {
            document.getElementById('<%= Btn_CustomerInfo_Modal.ClientID %>').click();
        }

        function lnkStockInfo_Command1(productId) {
            window.open('../Inventory/Magic_Stock_Analysis.aspx?ProductId=' + productId + '&&Type=SALES&&Contact=');
        }

    </script>
    <script src="../Script/customer.js"></script>
</asp:Content>
