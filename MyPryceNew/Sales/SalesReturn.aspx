<%@ Page Title="" Language="C#" MasterPageFile="~/ERPMaster.master" AutoEventWireup="true" CodeFile="SalesReturn.aspx.cs" Inherits="Sales_SalesReturn" %>

<%@ Register Src="~/WebUserControl/ReportSystem.ascx" TagName="ReportSystem" TagPrefix="RS" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Src="~/WebUserControl/ucControlsSetting.ascx" TagName="ucCtlSetting" TagPrefix="UC" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Head" runat="Server">
    <script>
        function LI_Edit_Active() {
            $("#Li_List").removeClass("active");
            $("#List").removeClass("active");

            $("#Li_New").addClass("active");
            $("#New").addClass("active");
        }
        function Li_Import_Active() {
            $("#Li_Import").addClass("active");
            $("#import_invoice").addClass("active");
            document.getElementById('<%= Btn_Li_Import.ClientID %>').click();
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="HeaderContent" runat="Server">
    <h1>
        <i class="fa fa-reply"></i>&nbsp;&nbsp;
        <asp:Label ID="lblHeader" runat="server" Text="<%$ Resources:Attendance,Sales Return %>"></asp:Label>
    </h1>
    <ol class="breadcrumb">
        <li><a href="#"><i class="fa fa-dashboard"></i>
            <asp:Label ID="Label6" runat="server" Text="<%$ Resources:Attendance,Inventory%>"></asp:Label></a></li>
        <li><a href="#"><i class="fa fa-dashboard"></i>
            <asp:Label ID="Label10" runat="server" Text="<%$ Resources:Attendance,Sales%>"></asp:Label></a></li>
        <li class="active">
            <asp:Label ID="Label5" runat="server" Text="<%$ Resources:Attendance,Sales Return%>"></asp:Label></li>
    </ol>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="Server">
    <asp:UpdatePanel ID="Update_Button" runat="server">
        <ContentTemplate>
            <asp:Button ID="Btn_Bin" Style="display: none;" runat="server" OnClick="btnCancelMenu_Click" Text="Bin" />
            <asp:Button ID="Btn_Sales_Invoice" Style="display: none;" runat="server" OnClick="btnInvoiceList_Click" Text="Sales Invoice" />
            <asp:Button ID="Btn_Li_Import" Style="display: none;" runat="server" OnClick="Btn_Li_Import_Click" Text="Import" />
            <asp:Button ID="Btn_View_Modal" Style="display: none;" runat="server" data-toggle="modal" data-target="#Return_Modal" Text="View Modal" />
            <asp:HiddenField runat="server" ID="hdnCanPrint" />
            <asp:HiddenField runat="server" ID="hdnCanView" />
            <asp:HiddenField runat="server" ID="hdnCanEdit" />
            <asp:HiddenField runat="server" ID="hdnCanDelete" />
        </ContentTemplate>
    </asp:UpdatePanel>

    <asp:UpdateProgress ID="UpdateProgress11" runat="server" AssociatedUpdatePanelID="Update_Button">
        <ProgressTemplate>
            <div class="modal_Progress">
                <div class="center_Progress">
                    <img alt="" src="../Images/ajax-loader2.gif" style="vertical-align: middle; background-color: White; border-radius: 10px;" />
                </div>
            </div>
        </ProgressTemplate>
    </asp:UpdateProgress>

    <div class="row">
        <div class="col-md-12">
            <div class="nav-tabs-custom">
                <ul class="nav nav-tabs pull-right bg-blue-gradient">
                    <li id="Li_Import"><a href="#import_invoice" onclick="Li_Tab_Import()" data-toggle="tab">
                        <i class="fa fa-upload"></i>&nbsp;&nbsp;<asp:Label ID="Label19" runat="server" Text="<%$ Resources:Attendance,Import %>"></asp:Label></a></li>
                    <li id="Li_Sales_Invoice"><a href="#Sales_Invoice" onclick="Li_Tab_Sales_Invoice()" data-toggle="tab">
                        <i class="fas fa-file-invoice-dollar"></i>&nbsp;&nbsp;
                        <asp:Label ID="Label2" runat="server" Text="<%$ Resources:Attendance,Sales Invoice %>"></asp:Label></a></li>
                    <li id="Li_Bin"><a href="#Bin" onclick="Li_Tab_Bin()" data-toggle="tab">
                        <i class="fa fa-trash"></i>&nbsp;&nbsp;<asp:Label ID="Label13" runat="server" Text="<%$ Resources:Attendance,Bin %>"></asp:Label></a></li>
                    <li id="Li_New"><a href="#New" data-toggle="tab">
                        <asp:UpdatePanel ID="Update_Li" runat="server">
                            <ContentTemplate>
                                <i class="fa fa-file"></i>&nbsp;&nbsp;
                                <asp:Label ID="Lbl_Tab_New" runat="server" Text="<%$ Resources:Attendance,New%>"></asp:Label>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </a></li>
                    <li id="Li_List" class="active"><a href="#List" data-toggle="tab">
                        <i class="fa fa-list"></i>&nbsp;&nbsp;<asp:Label ID="Label1" runat="server" Text="<%$ Resources:Attendance,List %>"></asp:Label></a></li>
                </ul>
                <div class="tab-content">
                    <div class="tab-pane active" id="List">
                        <asp:UpdatePanel ID="Update_List" runat="server" UpdateMode="Conditional">
                            <ContentTemplate>


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
                                                    <asp:DropDownList ID="ddlFieldName" runat="server" CssClass="form-control" AutoPostBack="true" OnSelectedIndexChanged="ddlFieldName_SelectedIndexChanged">
                                                        <asp:ListItem Text="<%$ Resources:Attendance, Return No. %>" Value="Return_No"></asp:ListItem>
                                                        <asp:ListItem Text="<%$ Resources:Attendance, Return Date %>" Value="Return_Date"></asp:ListItem>
                                                        <asp:ListItem Text="<%$ Resources:Attendance, Invoice No %>" Value="Invoice_No" Selected="True"></asp:ListItem>
                                                        <asp:ListItem Text="<%$ Resources:Attendance, Invoice Date %>" Value="Invoice_Date"></asp:ListItem>
                                                        <asp:ListItem Text="<%$ Resources:Attendance, Transfer Type %>" Value="Transtype"></asp:ListItem>
                                                        <asp:ListItem Text="<%$ Resources:Attendance, Customer Name %>" Value="CustomerName"></asp:ListItem>
                                                        <asp:ListItem Text="<%$ Resources:Attendance,Sales Person %>" Value="SalesPerson_Name"></asp:ListItem>
                                                        <asp:ListItem Text="<%$ Resources:Attendance,Created By %>" Value="CreatedUser"></asp:ListItem>
                                                        <asp:ListItem Text="<%$ Resources:Attendance,Modified By %>" Value="ModifiedUser"></asp:ListItem>
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
                                                    <asp:Panel ID="Panel2" runat="server" DefaultButton="btnbind">
                                                        <asp:TextBox ID="txtValue" runat="server" CssClass="form-control" placeholder="Search From Content"></asp:TextBox>
                                                        <asp:TextBox ID="txtValueDate" runat="server" CssClass="form-control" Visible="false" placeholder="Search From Date" />
                                                        <cc1:CalendarExtender OnClientShown="showCalendar" ID="CalendartxtValueDate" runat="server" TargetControlID="txtValueDate" />
                                                    </asp:Panel>
                                                </div>
                                                <div class="col-lg-2">
                                                    <asp:LinkButton ID="btnbind" runat="server" CausesValidation="False" OnClick="btnbindrpt_Click" ToolTip="<%$ Resources:Attendance,Search %>"><span class="fa fa-search"  style="font-size:25px;"></span></asp:LinkButton>&nbsp;&nbsp;&nbsp;
                                                    <asp:LinkButton ID="btnRefresh" runat="server" CausesValidation="False" OnClick="btnRefreshReport_Click" ToolTip="<%$ Resources:Attendance,Refresh %>"><span class="fa fa-repeat"  style="font-size:25px;"></span></asp:LinkButton>&nbsp;&nbsp;&nbsp;
                                                    <asp:LinkButton ID="btnGvListSetting" ImageAlign="Right" ToolTip="List Settings" runat="server" OnClick="btnGvListSetting_Click" Visible="false"><span class="fa fa-wrench"  style="font-size:25px;"></span></asp:LinkButton>
                                                </div>

                                            </div>
                                        </div>
                                    </div>
                                </div>
                                <div class="box box-warning box-solid" <%= GvSalesReturn.Rows.Count>0?"style='display:block'":"style='display:none'"%>>
                                    <div class="box-body">
                                        <div class="row">
                                            <div class="col-md-12">
                                                <div class="flow">
                                                    <asp:GridView CssClass="table-striped table-bordered table table-hover" ID="GvSalesReturn" PageSize="<%# PageControlCommon.GetPageSize() %>"
                                                        runat="server" AutoGenerateColumns="False" Width="100%" AllowPaging="false" CurrentSortField="Return_Date" CurrentSortDirection="DESC"
                                                        AllowSorting="True" OnSorting="GvSalesReturn_Sorting">

                                                        <Columns>

                                                            <asp:TemplateField HeaderText="Action">
                                                                <ItemTemplate>
                                                                    <div class="dropdown" style="position: absolute;">
                                                                        <button class="btn btn-default dropdown-toggle" type="button" data-toggle="dropdown">
                                                                            <i class="fa fa-ellipsis-h" aria-hidden="true"></i>
                                                                        </button>
                                                                        <ul class="dropdown-menu">
                                                                            <li <%= hdnCanPrint.Value=="true"?"style='display:block'":"style='display:none'" %>>
                                                                                <a onclick="IbtnPrint_Command('<%# Eval("Trans_Id") %>')" title="<%= Resources.Attendance.Print %>"><i class="fa fa-print"></i>Print </a>
                                                                            </li>

                                                                            <li <%= hdnCanPrint.Value=="true"?"style='display:block'":"style='display:none'" %>>
                                                                                <a onclick="getReportDataWithLoc('<%# Eval("Trans_Id") %>','<%# Eval("Location_Id") %>')"><i class="fa fa-print" style="cursor: pointer"></i>Report System</a>
                                                                            </li>


                                                                            <li <%= hdnCanEdit.Value=="true"?"style='display:block'":"style='display:none'" %>>
                                                                                <asp:LinkButton ID="lnkViewDetail" runat="server" CommandArgument='<%# Eval("Trans_Id") %>' OnCommand="lnkViewDetail_Command" CausesValidation="False"><i class="fa fa-eye"></i>View</asp:LinkButton>
                                                                            </li>

                                                                            <li <%= hdnCanEdit.Value=="true"?"style='display:block'":"style='display:none'" %>>
                                                                                <asp:LinkButton ID="btnEdit" runat="server" CommandArgument='<%# Eval("Trans_Id") %>' OnCommand="btnEdit_Command" CausesValidation="False"><i class="fa fa-pencil"></i>Edit </asp:LinkButton>
                                                                            </li>
                                                                            <li <%= hdnCanDelete.Value=="true"?"style='display:block'":"style='display:none'" %>>
                                                                                <asp:LinkButton ID="IbtnDelete" runat="server" CausesValidation="False" CommandArgument='<%# Eval("Trans_Id") %>' OnCommand="IbtnDelete_Command"><i class="fa fa-trash"></i>Delete</asp:LinkButton>
                                                                                <cc1:ConfirmButtonExtender ID="confirm1" runat="server" ConfirmText="<%$ Resources:Attendance,Are you sure you want to delete the record?%>" TargetControlID="IbtnDelete"></cc1:ConfirmButtonExtender>
                                                                            </li>
                                                                        </ul>
                                                                    </div>

                                                                </ItemTemplate>
                                                            </asp:TemplateField>


                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,Return No. %>" SortExpression="Return_No">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblgvReturnNo" runat="server" Text='<%#Eval("Return_No") %>' />
                                                                </ItemTemplate>
                                                                <ItemStyle HorizontalAlign="Center" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,Return Date %>" SortExpression="Return_Date">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblgvReturnDate" runat="server" Text='<%#GetDate(Eval("Return_Date").ToString()) %>' />
                                                                </ItemTemplate>
                                                                <ItemStyle HorizontalAlign="Center" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,Invoice No. %>" SortExpression="Invoice_No">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblgvSInvNo" runat="server" Text='<%#Eval("Invoice_No") %>' />
                                                                </ItemTemplate>
                                                                <ItemStyle HorizontalAlign="Center" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,Invoice Date %>" SortExpression="Invoice_Date">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblgvSInvDate" runat="server" Text='<%#GetDate(Eval("Invoice_Date").ToString()) %>' />
                                                                </ItemTemplate>
                                                                <ItemStyle HorizontalAlign="Center" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,Customer Name%>" SortExpression="CustomerName">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblgvCustomerName" runat="server" Text='<%#Eval("CustomerName") %>' />
                                                                    <asp:Label ID="lblgvPost" runat="server" Text='<%#Eval("Post") %>' Visible="false" />
                                                                </ItemTemplate>
                                                                <FooterStyle HorizontalAlign="Right" />
                                                                <ItemStyle HorizontalAlign="Left" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,Sales Person %>" SortExpression="SalesPerson_Name">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblgvSalesPerson" runat="server" Text='<%#Eval("SalesPerson_Name")%>' />
                                                                </ItemTemplate>
                                                                <ItemStyle HorizontalAlign="Left" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,Transfer Type%>" SortExpression="Transtype">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblgvTransType" runat="server" Text='<%#Eval("Transtype") %>' />
                                                                </ItemTemplate>
                                                                <ItemStyle HorizontalAlign="Left" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,Created By %>" SortExpression="CreatedUser">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblgvUser" runat="server" Text='<%#Eval("CreatedUser") %>' />
                                                                </ItemTemplate>
                                                                <ItemStyle HorizontalAlign="Left" Width="10%" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,Modified By %>" SortExpression="ModifiedUser">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblModified_User" runat="server" Text='<%# Eval("ModifiedUser") %>'></asp:Label>
                                                                </ItemTemplate>
                                                                <ItemStyle HorizontalAlign="Left" Width="10%" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,Amount%>" SortExpression="GrandTotal">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblgvGrandTotal" runat="server" Visible="false" Text='<%#Eval("GrandTotal")%>'></asp:Label>
                                                                    <asp:Label ID="lblgvGrandTotalSI" runat="server" Text='<%# GetCurrencySymbol(Eval("GrandTotal").ToString(),Eval("Currency_Id").ToString()) %>' />
                                                                </ItemTemplate>
                                                                <FooterStyle HorizontalAlign="Center" />
                                                                <ItemStyle HorizontalAlign="Right" />
                                                            </asp:TemplateField>
                                                        </Columns>

                                                        <PagerStyle CssClass="pagination-ys" />

                                                    </asp:GridView>
                                                    <asp:Repeater ID="rptPager" runat="server">
                                                        <ItemTemplate>
                                                            <ul class="pagination">
                                                                <li class='<%# Convert.ToBoolean(Eval("Enabled")) ? "page-item" : "page-item active" %>'>
                                                                    <asp:LinkButton ID="lnkPage" runat="server" Text='<%#Eval("Text") %>' CommandArgument='<%# Eval("Value") %>'
                                                                        CssClass="page-link"
                                                                        OnClick="Page_Changed" OnClientClick='<%# !Convert.ToBoolean(Eval("Enabled")) ? "return false;" : "" %>'></asp:LinkButton>
                                                                </li>
                                                            </ul>
                                                        </ItemTemplate>
                                                    </asp:Repeater>
                                                </div>
                                                <asp:HiddenField ID="hdnGvSalesreturnCurrentPageIndex" runat="server" Value="1" />
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </ContentTemplate>
                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="btnPost" EventName="Click" />
                                <asp:AsyncPostBackTrigger ControlID="btnSInvSave" EventName="Click" />
                                <asp:AsyncPostBackTrigger ControlID="BtnReset" EventName="Click" />
                                <asp:AsyncPostBackTrigger ControlID="btnSInvCancel" EventName="Click" />
                            </Triggers>
                        </asp:UpdatePanel>
                    </div>
                    <div class="tab-pane" id="New">
                        <asp:UpdatePanel ID="Update_New" runat="server" UpdateMode="Conditional">
                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="GvSalesReturn" />
                            </Triggers>
                            <ContentTemplate>
                                <div class="row">
                                    <div class="col-md-12">
                                        <div class="box box-primary">
                                            <div class="box-body">
                                                <div class="form-group">
                                                    <asp:HiddenField ID="hdnMerchant" Value="" runat="server" />
                                                    <asp:HiddenField ID="hdnOrderId" Value="" runat="server" />



                                                    <div class="col-md-12">
                                                        <asp:ImageButton ID="btnControlsSetting" ImageAlign="Right" ToolTip="Controls Setting" runat="server" ImageUrl="~/Images/setting.png" OnClick="btnControlsSetting_Click" Style="width: 32px; height: 32px" Visible="false" />
                                                    </div>
                                                    <div class="col-md-6">
                                                        <asp:Label ID="Label3" runat="server" Text="<%$ Resources:Attendance,Return No. %>"></asp:Label>
                                                        <a style="color: Red">*</a>
                                                        <asp:RequiredFieldValidator EnableClientScript="true" Style="float: right;" runat="server" ID="RequiredFieldValidator3" ValidationGroup="Save"
                                                            Display="Dynamic" SetFocusOnError="true" ControlToValidate="txtSreturnNo" ErrorMessage="<%$ Resources:Attendance,Enter Return No %>"></asp:RequiredFieldValidator>

                                                        <asp:TextBox ID="txtSreturnNo" runat="server" CssClass="form-control" ReadOnly="true" />
                                                        <br />
                                                    </div>
                                                    <div class="col-md-6">
                                                        <asp:Label ID="Label4" runat="server" Text="<%$ Resources:Attendance,Return Date %>"></asp:Label>
                                                        <a style="color: Red">*</a>
                                                        <asp:RequiredFieldValidator EnableClientScript="true" Style="float: right;" runat="server" ID="RequiredFieldValidator1" ValidationGroup="Save"
                                                            Display="Dynamic" SetFocusOnError="true" ControlToValidate="txtsReturnDate" ErrorMessage="<%$ Resources:Attendance,Enter Return Date %>"></asp:RequiredFieldValidator>

                                                        <asp:TextBox ID="txtsReturnDate" runat="server" CssClass="form-control" />
                                                        <cc1:CalendarExtender OnClientShown="showCalendar" ID="Calender" runat="server" TargetControlID="txtsReturnDate" />
                                                        <br />
                                                    </div>
                                                    <div class="col-md-6">
                                                        <asp:Label ID="lblSInvNo" runat="server" Text="<%$ Resources:Attendance,Invoice No. %>"></asp:Label>
                                                        <a style="color: Red">*</a>
                                                        <asp:RequiredFieldValidator EnableClientScript="true" Style="float: right;" runat="server" ID="RequiredFieldValidator2" ValidationGroup="Save"
                                                            Display="Dynamic" SetFocusOnError="true" ControlToValidate="txtSInvNo" ErrorMessage="<%$ Resources:Attendance,Enter Invoice No %>"></asp:RequiredFieldValidator>

                                                        <asp:TextBox ID="txtSInvNo" runat="server" CssClass="form-control" BackColor="#eeeeee"
                                                            OnTextChanged="txtSInvNo_TextChanged" AutoPostBack="true" ReadOnly="true" />
                                                        <cc1:AutoCompleteExtender ID="txtSInvNo_AutoCompleteExtender" runat="server" DelimiterCharacters=""
                                                            Enabled="True" CompletionInterval="100" CompletionSetCount="1" MinimumPrefixLength="1"
                                                            ServiceMethod="GetCompletionListInvoiceNo" ServicePath="" TargetControlID="txtSInvNo"
                                                            UseContextKey="True" CompletionListCssClass="autoCompleteList" CompletionListItemCssClass="autoCompleteListItem" CompletionListHighlightedItemCssClass="autoCompleteSelectedListItem" OnClientShown="resetPosition">
                                                        </cc1:AutoCompleteExtender>
                                                        <br />
                                                    </div>
                                                    <div class="col-md-6">
                                                        <asp:Label ID="lblSInvDate" runat="server" Text="<%$ Resources:Attendance,Invoice Date %>"></asp:Label>
                                                        <a style="color: Red">*</a>
                                                        <asp:RequiredFieldValidator EnableClientScript="true" Style="float: right;" runat="server" ID="RequiredFieldValidator4" ValidationGroup="Save"
                                                            Display="Dynamic" SetFocusOnError="true" ControlToValidate="txtSInvDate" ErrorMessage="<%$ Resources:Attendance,Enter Invoice Date %>"></asp:RequiredFieldValidator>

                                                        <asp:TextBox ID="txtSInvDate" runat="server" CssClass="form-control" ReadOnly="True" />
                                                        <br />
                                                    </div>
                                                    <div id="trTransfer" runat="server" visible="false">
                                                        <div class="col-md-6">
                                                            <asp:Label ID="lblSalesOrderNo" runat="server" Text="<%$ Resources:Attendance,Sales Order No. %>" />
                                                            <asp:TextBox ID="txtSalesOrderNo" runat="server" CssClass="form-control" ReadOnly="True" />
                                                            <br />
                                                        </div>
                                                        <div class="col-md-6">
                                                            <asp:Label ID="lblSalesOrderDate" runat="server" Text="<%$ Resources:Attendance,Sales Order Date %>" />
                                                            <asp:TextBox ID="txtSalesOrderDate" runat="server" ReadOnly="true" CssClass="form-control" />
                                                            <br />
                                                        </div>
                                                    </div>
                                                    <div class="col-md-6">
                                                        <asp:Label ID="lblCustomerName" runat="server" Text="<%$ Resources:Attendance,Customer Name %>" />
                                                        <a style="color: Red">*</a>
                                                        <asp:RequiredFieldValidator EnableClientScript="true" Style="float: right;" runat="server" ID="RequiredFieldValidator5" ValidationGroup="Save"
                                                            Display="Dynamic" SetFocusOnError="true" ControlToValidate="txtCustomerName" ErrorMessage="<%$ Resources:Attendance,Enter Customer Name %>"></asp:RequiredFieldValidator>

                                                        <asp:TextBox ID="txtCustomerName" runat="server" ReadOnly="true" CssClass="form-control" />
                                                        <br />
                                                    </div>
                                                    <div class="col-md-6">
                                                        <asp:Label ID="lblPaymentMode" runat="server" Text="Invoice Type" />
                                                        <a style="color: Red">*</a>
                                                        <asp:RequiredFieldValidator EnableClientScript="true" Style="float: right;" runat="server" ID="RequiredFieldValidator7" ValidationGroup="Save" Display="Dynamic"
                                                            SetFocusOnError="true" ControlToValidate="ddlPaymentMode" InitialValue="--Select--" ErrorMessage="<%$ Resources:Attendance,Select Invoice Type %>" />

                                                        <asp:DropDownList ID="ddlPaymentMode" runat="server" CssClass="form-control">
                                                        </asp:DropDownList>
                                                        <br />
                                                    </div>
                                                    <div class="col-md-6">
                                                        <asp:Label ID="lblCurrency" runat="server" Text="<%$ Resources:Attendance,Currency %>" />
                                                        <a style="color: Red">*</a>
                                                        <asp:RequiredFieldValidator EnableClientScript="true" Style="float: right;" runat="server" ID="RequiredFieldValidator6" ValidationGroup="Save"
                                                            Display="Dynamic" SetFocusOnError="true" ControlToValidate="txtCurrency" ErrorMessage="<%$ Resources:Attendance,Enter Currency %>"></asp:RequiredFieldValidator>
                                                        <asp:TextBox ID="txtCurrency" runat="server" ReadOnly="true" CssClass="form-control" />
                                                        <asp:HiddenField ID="Hdn_Exchange_Rate" Value="1" runat="server" />
                                                        <br />
                                                    </div>
                                                    <div class="col-md-6">
                                                        <asp:Label ID="lblSalesPerson" runat="server" Text="<%$ Resources:Attendance,Sales Person%>" />
                                                        <asp:TextBox ID="txtSalesPerson" runat="server" ReadOnly="true" CssClass="form-control" />
                                                        <br />
                                                    </div>
                                                    <div class="col-md-6">
                                                        <asp:Label ID="lblPOSNo" runat="server" Text="<%$ Resources:Attendance,POS No.%>" />
                                                        <asp:TextBox ID="txtPOSNo" runat="server" ReadOnly="true" CssClass="form-control" />
                                                        <br />
                                                    </div>
                                                    <div style="display: none;" class="col-md-6">
                                                        <asp:Label ID="lblAccountNo" runat="server" Text="<%$ Resources:Attendance,Account No%>" />
                                                        <asp:TextBox ID="txtAccountNo" ReadOnly="true" runat="server" CssClass="form-control" />
                                                        <br />
                                                    </div>
                                                    <div style="display: none;" class="col-md-6">
                                                        <asp:Label ID="lblInvoiceCosting" runat="server" Text="<%$ Resources:Attendance,Invoice Costing%>" />
                                                        <asp:TextBox ID="txtInvoiceCosting" ReadOnly="true" runat="server" CssClass="form-control" />
                                                        <br />
                                                    </div>
                                                    <div style="display: none;" class="col-md-6">
                                                        <asp:Label ID="lblShift" runat="server" Text="<%$ Resources:Attendance,Shift%>" />
                                                        <asp:TextBox ID="txtShift" runat="server" CssClass="form-control" ReadOnly="true" />
                                                        <br />
                                                    </div>
                                                    <div style="display: none;" class="col-md-6">
                                                        <asp:Label ID="lblTender" runat="server" Text="<%$ Resources:Attendance,Tender%>" />
                                                        <asp:TextBox ID="txtTender" runat="server" CssClass="form-control" ReadOnly="true" />
                                                        <br />
                                                    </div>
                                                    <div class="col-md-12">
                                                        <div style="overflow: auto">
                                                            <asp:GridView CssClass="table-striped table-bordered table table-hover" ID="GvSalesInvoiceDetailDirect" runat="server" AutoGenerateColumns="False"
                                                                Width="100%" OnRowCreated="GvSalesOrderDetail_RowCreated" OnRowDataBound="GvSalesInvoiceDetailDirect_OnRowDataBound">

                                                                <Columns>
                                                                    <asp:TemplateField HeaderStyle-HorizontalAlign="Center">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblsalesordreno" runat="server" Text='<%#Eval("SalesOrderNo") %>' />
                                                                            <asp:HiddenField ID="hdnSOId" runat="server" Value='<%#Eval("SIFromTransNo") %>' />
                                                                        </ItemTemplate>
                                                                        <ItemStyle HorizontalAlign="Center" />
                                                                        <ItemStyle />
                                                                        <FooterTemplate>
                                                                            <asp:Label ID="lblTotalQuantity" runat="server" Text="<%$ Resources:Attendance,Total %>"
                                                                                CssClass="labelComman"></asp:Label>
                                                                        </FooterTemplate>
                                                                        <FooterStyle BorderStyle="None" />
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField>
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblgvSerialNo" Width="30px" runat="server" Text='<%#Eval("Serial_No") %>'
                                                                                Visible="false" />
                                                                            <asp:Label ID="lblgvsNo" Width="30px" runat="server" Text='<%#Container.DataItemIndex+1 %>' />
                                                                        </ItemTemplate>
                                                                        <ItemStyle HorizontalAlign="Center" Width="30px" />
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="<%$ Resources:Attendance,Id%>">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblgvProductCode" runat="server" Text='<%#new Inv_ProductMaster(Session["DBConnection"].ToString()).GetProductCodebyProductId(Eval("Product_Id").ToString()) %>' />
                                                                        </ItemTemplate>
                                                                        <ItemStyle HorizontalAlign="Center" Width="70px" />
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="<%$ Resources:Attendance,Name %>">
                                                                        <ItemTemplate>
                                                                            <table width="100%">
                                                                                <tr>
                                                                                    <td>
                                                                                        <asp:HiddenField ID="hdngvTransId" runat="server" Value='<%#Eval("Trans_Id") %>' />
                                                                                        <asp:HiddenField ID="hdngvProductId" runat="server" Value='<%#Eval("Product_Id") %>' />
                                                                                        <asp:Label ID="lblgvProductName" runat="server" Text='<%#new Inv_ProductMaster(Session["DBConnection"].ToString()).GetProductNamebyProductId(Eval("Product_Id").ToString()) %>' />
                                                                                    </td>
                                                                                    <td align='<%= PageControlCommon.ChangeTDForDefaultRight()%>'>
                                                                                        <asp:ImageButton ID="lnkDes" runat="server" ImageUrl="~/Images/detail.png" />
                                                                                    </td>
                                                                                </tr>
                                                                            </table>
                                                                            <br />
                                                                            <asp:Panel ID="PopupMenu1" Width="100%" runat="server">
                                                                                <table border="1" cellpadding="0" cellspacing="0" bordercolor="#c6c6c6">
                                                                                    <tr>
                                                                                        <td>
                                                                                            <table width="314" height="110" cellspacing="0" bgcolor="#F9F9F9">
                                                                                                <tr>
                                                                                                    <td height="21" colspan="2">
                                                                                                        <div align="center" style="background: url(../Images/InvGridHdr.jpg) repeat">
                                                                                                            <asp:Label ID="lblDetail1" runat="server" Text="<%$ Resources:Attendance,Details %>"></asp:Label>
                                                                                                        </div>
                                                                                                    </td>
                                                                                                </tr>
                                                                                                <tr style="background-color: whitesmoke">
                                                                                                    <td colspan="2" align="left" valign="top">
                                                                                                        <asp:Panel ID="pnl" runat="server" Width="100%" Height="300px" ScrollBars="Vertical">
                                                                                                            <asp:Label ID="lblgvProductDescription" runat="server" Text='<%#new Inv_ProductMaster(Session["DBConnection"].ToString()).GetProductDescriptionbyProductId(Eval("Product_Id").ToString()) %>' />
                                                                                                        </asp:Panel>
                                                                                                        <br />
                                                                                                    </td>
                                                                                                </tr>
                                                                                            </table>
                                                                                        </td>
                                                                                    </tr>
                                                                                </table>
                                                                            </asp:Panel>
                                                                            <cc1:HoverMenuExtender ID="hme37" runat="Server" TargetControlID="lnkDes" PopupControlID="PopupMenu1"
                                                                                HoverCssClass="popupHover" PopupPosition="Left" OffsetX="0" OffsetY="0" PopDelay="50" />
                                                                        </ItemTemplate>
                                                                        <ItemStyle HorizontalAlign="Center" />
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="<%$ Resources:Attendance,Name %>">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblgvUnit" runat="server" Text='<%#Inv_UnitMaster.GetUnitCode(Eval("UnitId").ToString(),Session["DBConnection"].ToString(),Session["CompId"].ToString()) %>' />
                                                                            <asp:HiddenField ID="hdngvUnitId" runat="server" Value='<%#Eval("UnitId") %>' />
                                                                        </ItemTemplate>
                                                                        <ItemStyle HorizontalAlign="Center" />
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="<%$ Resources:Attendance,Price %>">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblgvUnitPrice" runat="server" Text='<%#Eval("UnitPrice") %>' />
                                                                        </ItemTemplate>
                                                                        <ItemStyle HorizontalAlign="Center" Width="80px" />
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="<%$ Resources:Attendance,Free %>" Visible="false">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblFree" runat="server" />
                                                                        </ItemTemplate>
                                                                        <ItemStyle HorizontalAlign="Center" />
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="<%$ Resources:Attendance,Sys. %>">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblgvSystemQuantity" runat="server" Text='<%#GetProductStock(Eval("Product_Id").ToString()) %>' />
                                                                        </ItemTemplate>
                                                                        <ItemStyle HorizontalAlign="Center" />
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="<%$ Resources:Attendance,Order %>">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblgvOrderqty" runat="server" Text='<%#Eval("OrderQty") %>' />
                                                                        </ItemTemplate>
                                                                        <ItemStyle HorizontalAlign="Center" />
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="<%$ Resources:Attendance,Sales %>">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblgvQuantity" runat="server" Text='<%#Eval("Quantity") %>' />
                                                                        </ItemTemplate>
                                                                        <ItemStyle HorizontalAlign="Center" />
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="<%$ Resources:Attendance,Return %>">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblgvReturnQuantity" runat="server" Text='<%#Eval("TotalReturnQty") %>' />
                                                                            <asp:Label ID="lblgvInvoice_Id" runat="server" Text='<%#Eval("Invoice_No") %>' Visible="false" />
                                                                        </ItemTemplate>
                                                                        <ItemStyle HorizontalAlign="Center" />
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="<%$ Resources:Attendance,Remain %>">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblgvRemaningQuantity" runat="server" />
                                                                        </ItemTemplate>
                                                                        <ItemStyle HorizontalAlign="Center" />
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Return">
                                                                        <ItemTemplate>
                                                                            <asp:TextBox ID="txtgvReturnQuantity" runat="server" Width="80px" OnTextChanged="txtgvReturnQuantity_TextChanged"
                                                                                Text='<%#Eval("ReturnQty") %>' AutoPostBack="true" />
                                                                            <asp:LinkButton ID="lnkAddSerialDirect" AutoPostBack="True" runat="server" Text="<%$ Resources:Attendance,Add %>"
                                                                                OnCommand="lnkAddSerialDirect_Command" CommandArgument='<%# Eval("Product_Id") %>'
                                                                                ForeColor="Blue" Font-Underline="true"></asp:LinkButton>
                                                                            <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender5" runat="server" Enabled="True"
                                                                                TargetControlID="txtgvReturnQuantity" ValidChars="1,2,3,4,5,6,7,8,9,0,.">
                                                                            </cc1:FilteredTextBoxExtender>
                                                                        </ItemTemplate>
                                                                        <ItemStyle HorizontalAlign="Left" />
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="<%$ Resources:Attendance,Price %>">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblgvQuantityPrice" runat="server" />
                                                                        </ItemTemplate>
                                                                        <ItemStyle HorizontalAlign="Center" />
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="<%$ Resources:Attendance,% %>">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="txtgvDiscountP" runat="server" Text='<%#Eval("DiscountP") %>' />
                                                                        </ItemTemplate>
                                                                        <ItemStyle HorizontalAlign="Center" />
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="<%$ Resources:Attendance,Value %>">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="txtgvDiscountV" runat="server" Text='<%#Eval("DiscountV") %>' />
                                                                        </ItemTemplate>
                                                                        <ItemStyle HorizontalAlign="Center" />
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="<%$ Resources:Attendance,% %>">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="txtgvTaxP" runat="server" Text='<%# GetTotalTaxByProductId(Eval("Product_Id").ToString()) %>' />
                                                                        </ItemTemplate>
                                                                        <ItemStyle HorizontalAlign="Center" />
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="<%$ Resources:Attendance,Value %>">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="txtgvTaxV" runat="server" Text='<%#Eval("TaxV") %>' />
                                                                        </ItemTemplate>
                                                                        <ItemStyle HorizontalAlign="Center" />
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField>
                                                                        <ItemTemplate>
                                                                            <asp:TextBox ID="txtgvTotal" Width="50px" ReadOnly="true" runat="server" />
                                                                            <cc1:FilteredTextBoxExtender ID="FilterTotal" runat="server" Enabled="True" TargetControlID="txtgvTotal"
                                                                                ValidChars="1,2,3,4,5,6,7,8,9,0,.">
                                                                            </cc1:FilteredTextBoxExtender>
                                                                        </ItemTemplate>
                                                                        <ItemStyle HorizontalAlign="Center" />
                                                                    </asp:TemplateField>
                                                                </Columns>

                                                                <PagerStyle CssClass="pagination-ys" />

                                                            </asp:GridView>
                                                        </div>
                                                        <br />
                                                    </div>
                                                    <div class="col-md-6">
                                                        <asp:Label ID="lblAmount" runat="server" Text="<%$ Resources:Attendance,Gross Amount %>" />
                                                        <asp:TextBox ID="txtAmount" runat="server" ReadOnly="true" CssClass="form-control" />
                                                        <br />
                                                    </div>
                                                    <div class="col-md-6">
                                                        <asp:Label ID="lblTotalQuantity" runat="server" Text="<%$ Resources:Attendance, Total Quantity %>" />
                                                        <asp:TextBox ID="txtTotalQuantity" runat="server" CssClass="form-control" ReadOnly="true" />
                                                        <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender4" runat="server" Enabled="True"
                                                            TargetControlID="txtTotalQuantity" ValidChars="1,2,3,4,5,6,7,8,9,0,.">
                                                        </cc1:FilteredTextBoxExtender>
                                                        <br />
                                                    </div>
                                                    <div id="trDiscount" runat="server" visible="false" class="col-md-6">
                                                        <asp:Label ID="lblDiscountP" runat="server" Text="<%$ Resources:Attendance,Discount(%) %>" />
                                                        <div class="input-group">
                                                            <asp:TextBox ID="txtDiscountP" runat="server" CssClass="form-control" ReadOnly="true" />
                                                            <div style="width: 50%" class="input-group-btn">
                                                                <asp:Label ID="Label8" runat="server" Style="width: 70px;" CssClass="form-control" Text="<%$ Resources:Attendance,Value %>" />
                                                                <div class="input-group-btn">
                                                                    <asp:TextBox ID="txtDiscountV" runat="server" CssClass="form-control" ReadOnly="true" />
                                                                </div>
                                                            </div>
                                                        </div>
                                                        <br />
                                                    </div>
                                                    <div id="trTax" runat="server" visible="false" class="col-md-6">
                                                        <asp:Label ID="lblTaxP" runat="server" Text="<%$ Resources:Attendance,Tax(%) %>" />
                                                        <div class="input-group">
                                                            <asp:TextBox ID="txtTaxP" runat="server" CssClass="form-control" ReadOnly="true" />
                                                            <div style="width: 50%" class="input-group-btn">
                                                                <asp:Label ID="Label11" runat="server" Style="width: 70px;" CssClass="form-control" Text="<%$ Resources:Attendance,Value %>" />
                                                                <div class="input-group-btn">
                                                                    <asp:TextBox ID="txtTaxV" runat="server" CssClass="form-control" ReadOnly="true" />
                                                                </div>
                                                            </div>
                                                        </div>
                                                        <br />
                                                    </div>
                                                    <div class="col-md-6">
                                                        <asp:Label ID="lblGrandTotal" runat="server" Text="<%$ Resources:Attendance,Net Amount %>" />
                                                        <asp:TextBox ID="txtNetAmount" runat="server" CssClass="form-control" ReadOnly="true" />
                                                        <br />
                                                    </div>
                                                    <div class="col-md-12" id="ctlRemark" runat="server">
                                                        <asp:Label ID="lblRemark" runat="server" Text="<%$ Resources:Attendance,Remark %>" />
                                                        <asp:TextBox ID="txtRemark" runat="server" CssClass="form-control" TextMode="MultiLine" Font-Names="Arial" />
                                                        <br />
                                                    </div>
                                                    <div class="col-md-6">
                                                        <asp:CheckBox ID="chkPost" runat="server" Text="<%$ Resources:Attendance,Post %>"
                                                            Visible="false" />
                                                        <br />
                                                    </div>
                                                    <div class="col-md-12" style="text-align: center">
                                                        <asp:Button ID="btnPost" runat="server" ValidationGroup="Save" CssClass="btn btn-primary" OnClick="btnPostSave_Click"
                                                            Visible="false" Text="<%$ Resources:Attendance,Post & Save %>" />
                                                        <cc1:ConfirmButtonExtender ID="confirm1" runat="server" ConfirmText="Are you sure to post the record ?"
                                                            TargetControlID="btnPost">
                                                        </cc1:ConfirmButtonExtender>

                                                        <asp:Button ID="btnSInvSave" ValidationGroup="Save" runat="server" CssClass="btn btn-success" OnClick="btnSInvSave_Click"
                                                            Visible="false" Text="<%$ Resources:Attendance,Save %>" />

                                                        <asp:Button ID="BtnReset" runat="server" Text="<%$ Resources:Attendance,Reset %>"
                                                            CssClass="btn btn-primary" CausesValidation="False" OnClick="BtnReset_Click" />

                                                        <asp:Button ID="btnSInvCancel" runat="server" CssClass="btn btn-danger" Text="<%$ Resources:Attendance,Cancel %>"
                                                            CausesValidation="False" OnClick="btnSInvCancel_Click" />
                                                        <asp:HiddenField ID="editid" runat="server" />
                                                        <br />
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </div>
                    <div class="tab-pane" id="Bin">
                        <asp:UpdatePanel ID="Update_Bin" runat="server" UpdateMode="Conditional">
                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="Btn_Bin" EventName="Click" />
                            </Triggers>
                            <ContentTemplate>



                                <div class="row">
                                    <div class="col-md-12">
                                        <div id="Div2" runat="server" class="box box-info collapsed-box">
                                            <div class="box-header with-border">
                                                <h3 class="box-title">
                                                    <asp:Label ID="Label7" runat="server" Text="Advance Search"></asp:Label></h3>
                                                &nbsp;&nbsp;|&nbsp;&nbsp;
				 <asp:Label ID="lblTotalRecordsBin" runat="server" Font-Bold="true" Text="<%$ Resources:Attendance,Total Records: 0 %>"></asp:Label>

                                                <div class="box-tools pull-right">
                                                    <button type="button" class="btn btn-box-tool" data-widget="collapse">
                                                        <i id="I2" runat="server" class="fa fa-plus"></i>
                                                    </button>
                                                </div>
                                            </div>
                                            <div class="box-body">

                                                <div class="col-lg-3">
                                                    <asp:DropDownList ID="ddlFieldNameBin" runat="server" CssClass="form-control" OnSelectedIndexChanged="SetCustomerTextBoxBin" AutoPostBack="true">
                                                        <asp:ListItem Selected="True" Text="<%$ Resources:Attendance, Return No. %>" Value="Return_No"></asp:ListItem>
                                                        <asp:ListItem Text="<%$ Resources:Attendance, Return Date %>" Value="Return_Date"></asp:ListItem>
                                                        <asp:ListItem Text="<%$ Resources:Attendance, Invoice No %>" Value="Invoice_No"></asp:ListItem>
                                                        <asp:ListItem Text="<%$ Resources:Attendance, Invoice Date %>" Value="Invoice_Date"></asp:ListItem>
                                                        <asp:ListItem Text="<%$ Resources:Attendance, Transfer Type %>" Value="Transtype"></asp:ListItem>
                                                        <asp:ListItem Text="<%$ Resources:Attendance, Customer Name %>" Value="CustomerName"></asp:ListItem>
                                                        <asp:ListItem Text="<%$ Resources:Attendance,Sales Person %>" Value="SalesPerson_Name"></asp:ListItem>
                                                        <asp:ListItem Text="<%$ Resources:Attendance,Created By %>" Value="CreatedUser"></asp:ListItem>
                                                        <asp:ListItem Text="<%$ Resources:Attendance,Modified By %>" Value="ModifiedUser"></asp:ListItem>
                                                    </asp:DropDownList>
                                                </div>
                                                <div class="col-lg-2">
                                                    <asp:DropDownList ID="ddlOptionBin" runat="server" CssClass="form-control">
                                                        <asp:ListItem Text="<%$ Resources:Attendance,--Select-- %>"></asp:ListItem>
                                                        <asp:ListItem Text="<%$ Resources:Attendance,Equal %>"></asp:ListItem>
                                                        <asp:ListItem Selected="True" Text="<%$ Resources:Attendance,Contains %>"></asp:ListItem>
                                                        <asp:ListItem Text="<%$ Resources:Attendance,Like %>"></asp:ListItem>
                                                    </asp:DropDownList>
                                                </div>
                                                <div class="col-lg-4">
                                                    <asp:Panel ID="Panel3" runat="server" DefaultButton="btnbindBin">
                                                        <asp:TextBox ID="txtValueBin" runat="server" CssClass="form-control" placeholder="Search From Content"></asp:TextBox>
                                                        <asp:TextBox ID="txtValueBinDate" runat="server" CssClass="form-control" Visible="false" placeholder="Search From Date"></asp:TextBox>
                                                        <cc1:CalendarExtender OnClientShown="showCalendar" ID="CalendarExtendertxtValueBinDate" runat="server" TargetControlID="txtValueBinDate" />
                                                    </asp:Panel>
                                                </div>
                                                <div class="col-lg-3">
                                                    <asp:LinkButton ID="btnbindBin" runat="server" CausesValidation="False" OnClick="btnbindBin_Click" ToolTip="<%$ Resources:Attendance,Search %>"><span class="fa fa-search"  style="font-size:25px;"></span></asp:LinkButton>&nbsp;&nbsp;&nbsp;
                                                    <asp:LinkButton ID="btnRefreshBin" runat="server" CausesValidation="False" OnClick="btnRefreshBin_Click" ToolTip="<%$ Resources:Attendance,Refresh %>"><span class="fa fa-repeat"  style="font-size:25px;"></span></asp:LinkButton>&nbsp;&nbsp;&nbsp;
                                                    <asp:LinkButton ID="imgBtnRestore" CausesValidation="False" Visible="false" runat="server" OnClick="imgBtnRestore_Click" ToolTip="<%$ Resources:Attendance, Active %>"><span class="far fa-lightbulb"  style="font-size:25px;"></span></asp:LinkButton>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>


                                <div class="box box-warning box-solid" <%= gvCancel.Rows.Count>0?"style='display:block'":"style='display:none'"%>>

                                    <div class="box-body">
                                        <div class="row">
                                            <div class="col-md-12">
                                                <div class="flow">
                                                    <asp:Label ID="lblSelectedRecord" runat="server" Visible="false"></asp:Label>
                                                    <asp:Label ID="Label9" runat="server" Visible="false"></asp:Label>
                                                    <asp:GridView CssClass="table-striped table-bordered table table-hover" ID="gvCancel" PageSize="<%# PageControlCommon.GetPageSize() %>" runat="server"
                                                        AutoGenerateColumns="False" Width="100%" AllowPaging="True" OnPageIndexChanging="gvCancel_PageIndexChanging"
                                                        OnSorting="gvCancel_OnSorting" AllowSorting="true">
                                                        <Columns>
                                                            <asp:TemplateField>
                                                                <HeaderTemplate>
                                                                    <asp:CheckBox ID="chkCurrent" runat="server" OnCheckedChanged="chkCurrent_CheckedChanged"
                                                                        AutoPostBack="true" />
                                                                </HeaderTemplate>
                                                                <ItemTemplate>
                                                                    <asp:CheckBox ID="chkSelect" runat="server" AutoPostBack="true" OnCheckedChanged="chkSelect_CheckedChanged" />
                                                                </ItemTemplate>
                                                                <ItemStyle HorizontalAlign="Center" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,Return No. %>" SortExpression="ReturnNo">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblgvReturnNo" runat="server" Text='<%#Eval("Return_No") %>' />
                                                                    <asp:HiddenField ID="hdnTransId" runat="server" Value='<%#Eval("Trans_Id") %>' />
                                                                </ItemTemplate>
                                                                <ItemStyle HorizontalAlign="Center" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,Return Date %>" SortExpression="Return_Date">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblgvReturnDate" runat="server" Text='<%#GetDate(Eval("Return_Date").ToString()) %>' />
                                                                </ItemTemplate>
                                                                <ItemStyle HorizontalAlign="Center" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,Invoice No. %>" SortExpression="Invoice_No">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblgvSInvNo" runat="server" Text='<%#Eval("Invoice_No") %>' />
                                                                </ItemTemplate>
                                                                <ItemStyle HorizontalAlign="Center" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,Invoice Date %>" SortExpression="Invoice_Date">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblgvSInvDate" runat="server" Text='<%#GetDate(Eval("Invoice_Date").ToString()) %>' />
                                                                </ItemTemplate>
                                                                <ItemStyle HorizontalAlign="Center" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,Customer Name%>" SortExpression="CustomerName">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblgvCustomerName" runat="server" Text='<%#Eval("CustomerName") %>' />
                                                                    <asp:Label ID="lblgvPost" runat="server" Text='<%#Eval("Post") %>' Visible="false" />
                                                                </ItemTemplate>
                                                                <FooterStyle HorizontalAlign="Right" />
                                                                <ItemStyle HorizontalAlign="Center" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,Sales Person %>" SortExpression="SalesPerson_Name">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblgvSalesPerson" runat="server" Text='<%#Eval("SalesPerson_Name")%>' />
                                                                </ItemTemplate>
                                                                <ItemStyle HorizontalAlign="Center" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,Transfer Type%>" SortExpression="Transtype">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblgvTransType" runat="server" Text='<%#Eval("Transtype") %>' />
                                                                </ItemTemplate>
                                                                <ItemStyle HorizontalAlign="Center" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,Created By %>" SortExpression="CreatedUser">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblgvUser" runat="server" Text='<%#Eval("CreatedUser") %>' />
                                                                </ItemTemplate>
                                                                <ItemStyle HorizontalAlign="Left" Width="10%" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,Modified By %>" SortExpression="ModifiedUser">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblModified_User" runat="server" Text='<%# Eval("ModifiedUser") %>'></asp:Label>
                                                                </ItemTemplate>
                                                                <ItemStyle HorizontalAlign="Left" Width="10%" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,Amount%>" SortExpression="GrandTotal">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblgvGrandTotal" runat="server" Visible="false" Text='<%#Eval("GrandTotal")%>'></asp:Label>
                                                                    <asp:Label ID="lblgvGrandTotalSI" runat="server" Text='<%# GetCurrencySymbol(Eval("GrandTotal").ToString(),Eval("Currency_Id").ToString()) %>' />
                                                                </ItemTemplate>
                                                                <FooterStyle HorizontalAlign="Center" />
                                                                <ItemStyle HorizontalAlign="Center" />
                                                            </asp:TemplateField>
                                                        </Columns>


                                                        <PagerStyle CssClass="pagination-ys" />

                                                    </asp:GridView>
                                                    <asp:HiddenField ID="HiddenField1" runat="server" />
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </div>
                    <div class="tab-pane" id="Sales_Invoice">
                        <asp:UpdatePanel ID="Update_Sales_Invoice" runat="server" UpdateMode="Conditional">
                            <ContentTemplate>



                                <div class="row">
                                    <div class="col-md-12">
                                        <div id="Div3" runat="server" class="box box-info collapsed-box">
                                            <div class="box-header with-border">
                                                <h3 class="box-title">
                                                    <asp:Label ID="Label12" runat="server" Text="Advance Search"></asp:Label></h3>
                                                &nbsp;&nbsp;|&nbsp;&nbsp;
				  <asp:Label ID="lblInvoiceTotalRecord" Font-Bold="true" runat="server" Text="<%$ Resources:Attendance,Total Records: 0 %>"></asp:Label>

                                                <div class="box-tools pull-right">
                                                    <button type="button" class="btn btn-box-tool" data-widget="collapse">
                                                        <i id="I3" runat="server" class="fa fa-plus"></i>
                                                    </button>
                                                </div>
                                            </div>
                                            <div class="box-body">

                                                <div class="col-lg-3">
                                                    <asp:DropDownList ID="ddlInvoiceFieldName" runat="server" CssClass="form-control" OnSelectedIndexChanged="SetCustomerTextBox" AutoPostBack="true">
                                                        <asp:ListItem Text="<%$ Resources:Attendance, Invoice Id %>" Value="Trans_Id"></asp:ListItem>
                                                        <asp:ListItem Text="<%$ Resources:Attendance, Invoice No %>" Value="Invoice_No" Selected="True"></asp:ListItem>
                                                        <asp:ListItem Text="Customer Order No" Value="ref_order_number"></asp:ListItem>
                                                        <asp:ListItem Text="<%$ Resources:Attendance, Invoice Date %>" Value="Invoice_Date"></asp:ListItem>
                                                        <asp:ListItem Text="<%$ Resources:Attendance,Sales Person %>" Value="EmployeeName"></asp:ListItem>
                                                        <asp:ListItem Text="<%$ Resources:Attendance, Customer Name %>" Value="CustomerName"></asp:ListItem>
                                                        <asp:ListItem Text="<%$ Resources:Attendance,Serial No %>" Value="SerialNo"></asp:ListItem>
                                                        <asp:ListItem Text="<%$ Resources:Attendance,Created By %>" Value="InvoiceCreatedBy"></asp:ListItem>

                                                    </asp:DropDownList>
                                                </div>
                                                <div class="col-lg-2">
                                                    <asp:DropDownList ID="ddlInvoiceOption" runat="server" CssClass="form-control">
                                                        <asp:ListItem Text="<%$ Resources:Attendance,--Select-- %>"></asp:ListItem>
                                                        <asp:ListItem Text="<%$ Resources:Attendance,Equal %>"></asp:ListItem>
                                                        <asp:ListItem Selected="True" Text="<%$ Resources:Attendance,Contains %>"></asp:ListItem>
                                                        <asp:ListItem Text="<%$ Resources:Attendance,Like %>"></asp:ListItem>
                                                    </asp:DropDownList>
                                                </div>
                                                <div class="col-lg-5">
                                                    <asp:Panel ID="Panel" runat="server" DefaultButton="btnInvoicebind">
                                                        <asp:TextBox ID="txtInvoiceValue" runat="server" CssClass="form-control" placeholder="Search From Content"></asp:TextBox>
                                                        <asp:TextBox ID="txtCustValue" runat="server" CssClass="form-control" Visible="false" placeholder="Search From Content"></asp:TextBox>
                                                        <cc1:AutoCompleteExtender ID="AutoCompleteExtender1" runat="server" DelimiterCharacters=""
                                                            Enabled="True" CompletionInterval="100" CompletionSetCount="1" MinimumPrefixLength="0"
                                                            ServiceMethod="GetCompletionListCustomer" ServicePath="" TargetControlID="txtCustValue"
                                                            UseContextKey="True" CompletionListCssClass="autoCompleteList" CompletionListItemCssClass="autoCompleteListItem" CompletionListHighlightedItemCssClass="autoCompleteSelectedListItem" OnClientShown="resetPosition">
                                                        </cc1:AutoCompleteExtender>
                                                        <asp:TextBox ID="txtInvoiceValueDate" placeholder="Search From Date" runat="server" CssClass="form-control" Visible="false"></asp:TextBox>
                                                        <cc1:CalendarExtender OnClientShown="showCalendar" Format="dd-MMM-yyyy" ID="CalendarExtendertxtValueDate" runat="server" TargetControlID="txtInvoiceValueDate" />
                                                    </asp:Panel>
                                                </div>
                                                <div class="col-lg-2">
                                                    <asp:LinkButton ID="btnInvoicebind" runat="server" CausesValidation="False" OnClick="btnInvoicebind_Click" ToolTip="<%$ Resources:Attendance,Search %>"><span class="fa fa-search"  style="font-size:25px;"></span></asp:LinkButton>&nbsp;&nbsp;&nbsp;
                                                    <asp:LinkButton ID="btnInvoiceRefresh" runat="server" CausesValidation="False" OnClick="btnInvoiceRefresh_Click" ToolTip="<%$ Resources:Attendance,Refresh %>"><span class="fa fa-repeat"  style="font-size:25px;"></span></asp:LinkButton>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                                <div class="box box-warning box-solid" <%= GvSalesInvoice.Rows.Count>0?"style='display:block'":"style='display:none'"%>>
                                    <div class="box-body">
                                        <div class="row">
                                            <div class="col-md-12">
                                                <div class="flow">
                                                    <asp:GridView CssClass="table-striped table-bordered table table-hover" ID="GvSalesInvoice" runat="server" AutoGenerateColumns="False" Width="100%" CurrentSortField="Invoice_Date" CurrentSortDirection="DESC"
                                                        AllowPaging="false" PageSize="<%# PageControlCommon.GetPageSize() %>"
                                                        AllowSorting="true" OnSorting="GvSalesInvoice_Sorting">
                                                        <Columns>
                                                            <asp:TemplateField>
                                                                <ItemTemplate>
                                                                    <asp:Button ID="btnpullBrand" runat="server" BorderStyle="None" BackColor="Transparent"
                                                                        CssClass="btnPull" CommandArgument='<%# Eval("Trans_Id") %>' OnCommand="btnInvoice_Command" />
                                                                </ItemTemplate>
                                                                <ItemStyle HorizontalAlign="Center" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,Invoice No. %>" SortExpression="Invoice_No">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblgvSInvNo" runat="server" Text='<%#Eval("Invoice_No") %>' />
                                                                </ItemTemplate>
                                                                <ItemStyle HorizontalAlign="Center" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,Invoice Date %>" SortExpression="Invoice_Date">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblgvSInvDate" runat="server" Text='<%#GetDate(Eval("Invoice_Date").ToString())%>' />
                                                                </ItemTemplate>
                                                                <ItemStyle HorizontalAlign="Center" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,Sales Person %>" SortExpression="EmployeeName">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblgvSalesPerson" runat="server" Text='<%#Eval("EmployeeName") %>' />
                                                                </ItemTemplate>
                                                                <ItemStyle HorizontalAlign="Center" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,Customer Name%>" SortExpression="CustomerName">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblgvCustomerName" runat="server" Text='<%#Eval("CustomerName") %>' />
                                                                </ItemTemplate>
                                                                <FooterStyle HorizontalAlign="Right" />
                                                                <ItemStyle HorizontalAlign="Center" />
                                                            </asp:TemplateField>

                                                            <asp:TemplateField HeaderText="Ref Order No" SortExpression="Ref_Order_Number">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblRefOrderNo" runat="server" Text='<%#Eval("Ref_Order_Number") %>' />
                                                                </ItemTemplate>
                                                                <FooterStyle HorizontalAlign="Right" />
                                                                <ItemStyle HorizontalAlign="Center" />
                                                            </asp:TemplateField>


                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,Created By%>" SortExpression="InvoiceCreatedBy">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblgvCreatedBy" runat="server" Text='<%#Eval("InvoiceCreatedBy")%>'></asp:Label>
                                                                </ItemTemplate>
                                                                <FooterStyle HorizontalAlign="Center" />
                                                                <ItemStyle HorizontalAlign="Center" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,Amount%>" SortExpression="GrandTotal">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblgvGrandTotal" runat="server" Visible="false" Text='<%#Eval("GrandTotal")%>'></asp:Label>
                                                                    <asp:Label ID="lblgvGrandTotalSI" runat="server" Text='<%# GetCurrencySymbol(Eval("GrandTotal").ToString(),Eval("Currency_Id").ToString()) %>' />
                                                                </ItemTemplate>
                                                                <FooterStyle HorizontalAlign="Right" />
                                                                <ItemStyle HorizontalAlign="Right" />
                                                            </asp:TemplateField>
                                                        </Columns>


                                                        <PagerStyle CssClass="pagination-ys" />

                                                    </asp:GridView>

                                                    <asp:Repeater ID="rptPager_Invoice" runat="server">
                                                        <ItemTemplate>
                                                            <ul class="pagination">
                                                                <li class='<%# Convert.ToBoolean(Eval("Enabled")) ? "page-item" : "page-item active" %>'>
                                                                    <asp:LinkButton ID="lnkPage" runat="server" Text='<%#Eval("Text") %>' CommandArgument='<%# Eval("Value") %>'
                                                                        CssClass="page-link"
                                                                        OnClick="InvoicePage_Changed" OnClientClick='<%# !Convert.ToBoolean(Eval("Enabled")) ? "return false;" : "" %>'></asp:LinkButton>
                                                                </li>
                                                            </ul>
                                                        </ItemTemplate>
                                                    </asp:Repeater>

                                                </div>
                                                <asp:HiddenField ID="hdnGvSalesInvoiceCurrentPageIndex" runat="server" Value="1" />
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </ContentTemplate>
                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="Btn_Sales_Invoice" EventName="Click" />
                            </Triggers>
                        </asp:UpdatePanel>
                    </div>
                    <div class="tab-pane" id="import_invoice">
                        <asp:UpdatePanel ID="update_import_invoice" runat="server">
                            <ContentTemplate>
                                <div class="row">
                                    <div class="col-md-12">
                                        <div id="Div4" runat="server" class="box box-info">
                                            <div class="box-header with-border">
                                                <h3 class="box-title">
                                                    <asp:Label ID="Label20" runat="server" Text="Import Invoice Using Excel"></asp:Label></h3>
                                                &nbsp;&nbsp;&nbsp;&nbsp;
                                                <asp:LinkButton ID="lnkTotalExcelImportRecords" runat="server" OnClick="lnkTotalExcelImportRecords_Click" CssClass="btn btn-default" Text="Total Records:0"></asp:LinkButton>
                                                <asp:HiddenField ID="hdnTotalExcelRecords" runat="server" Value="0" />
                                                &nbsp;&nbsp;&nbsp;&nbsp;
                                                <asp:LinkButton ID="lnkInvalidRecords" runat="server" OnClick="lnkInvalidRecords_Click" CssClass="btn btn-danger" Text="InValid:0"></asp:LinkButton>
                                                <asp:HiddenField ID="hdnInvalidExcelRecords" runat="server" Value="0" />
                                                &nbsp;&nbsp;&nbsp;&nbsp;
                                                <asp:LinkButton ID="lnkValidRecords" runat="server" OnClick="lnkValidRecords_Click" CssClass="btn btn-success" Text="Valid:0"></asp:LinkButton>
                                                <asp:HiddenField ID="hdnValidExcelRecords" runat="server" Value="0" />
                                            </div>
                                            <div class="box-body">
                                                <div class="row">
                                                    <div class="col-md-4">
                                                        <asp:HyperLink ID="lnkDownloadSameExcel" runat="server" Font-Bold="true" Font-Size="15px"
                                                            NavigateUrl="~/CompanyResource/import_sr_invoice_sample.xls" Text="Download sample excel format" Font-Underline="true"></asp:HyperLink>
                                                    </div>
                                                    <div class="col-md-4">
                                                        <asp:Label runat="server" Text="Browse Excel File" ID="Label169"></asp:Label>
                                                        <div class="input-group" style="width: 100%;">
                                                            <cc1:AsyncFileUpload ID="fileLoad"
                                                                OnClientUploadStarted="FUExcel_UploadStarted"
                                                                OnClientUploadError="FUExcel_UploadError"
                                                                OnClientUploadComplete="FUExcel_UploadComplete"
                                                                OnUploadedComplete="FUExcel_FileUploadComplete"
                                                                runat="server" CssClass="form-control"
                                                                CompleteBackColor="White"
                                                                UploaderStyle="Traditional"
                                                                UploadingBackColor="#CCFFFF"
                                                                ThrobberID="FUExcel_ImgLoader" Width="100%" />
                                                            <div class="input-group-btn" style="border: solid 1px #d2d6de; width: 35px;">
                                                                <asp:LinkButton ID="FUExcel_Img_Right" runat="server" Width="30px" Height="30px" Style="display: none"><i class="fa fa-check" style="font-size:30px;color:#22cb33"></i></asp:LinkButton>
                                                                <asp:LinkButton ID="FUExcel_Img_Wrong" runat="server" Width="30px" Height="30px" Style="display: none"><i class="fa fa-remove" style="font-size:30px"></i></asp:LinkButton>
                                                                <asp:Image ID="FUExcel_ImgLoader" runat="server" ImageUrl="../Images/loader.gif" />
                                                            </div>
                                                        </div>
                                                    </div>
                                                    <div class="col-md-4">
                                                        <asp:Button ID="btnUploadExcel" runat="server" CssClass="btn btn-primary" OnClick="btnUploadExcel_Click" Text="Import Invoice" />
                                                        <asp:Button ID="btnSaveExcelInvoice" runat="server" CssClass="btn btn-primary" OnClick="btnSaveExcelInvoice_Click" Text="Update" Enabled="false" />
                                                    </div>
                                                </div>
                                                <br />
                                                <div style="overflow: auto; max-height: 300px;">
                                                    <asp:GridView ID="gvImportSRList" DataKeyNames="validation_remark,merchant,invoice_no" runat="server"></asp:GridView>

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

    <div class="modal fade" id="ReportSystem" tabindex="-1" role="dialog" aria-labelledby="ReportSystem_Web_Control"
        aria-hidden="true">
        <div class="modal-dialog modal-lg">
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal">
                        <span aria-hidden="true">&times;</span><span class="sr-only">Close</span></button>
                    <h4 class="modal-title" id="ReportSystem_Web_Control">Report System
                    </h4>
                </div>
                <div class="modal-body">
                    <RS:ReportSystem runat="server" ID="reportSystem" />
                </div>
                <div class="modal-footer">
                </div>

            </div>
        </div>
    </div>

    <div class="modal fade" id="Return_Modal" tabindex="-1" role="dialog" aria-labelledby="Return_ModalLabel" aria-hidden="true">
        <div class="modal-dialog modal-lg">
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal">
                        <span aria-hidden="true">&times;</span><span class="sr-only">Close</span></button>
                    <h4 class="modal-title" id="Return_ModalLabel">Serial No</h4>
                </div>
                <div class="modal-body">
                    <asp:UpdatePanel ID="Update_Modal" runat="server">
                        <ContentTemplate>
                            <asp:HiddenField ID="HDFSortbin" runat="server" />
                            <div class="row">
                                <div class="col-md-12">
                                    <div class="box box-primary">
                                        <div class="box-body">
                                            <div class="form-group">
                                                <div class="col-md-12">
                                                    <asp:Label ID="lblproductId" runat="server" Text="Product Id"></asp:Label>
                                                    &nbsp:&nbsp<asp:Label ID="lblProductIdvalue" runat="server" Text="0"></asp:Label>
                                                    <br />
                                                </div>
                                                <div class="col-md-12">
                                                    <asp:Label ID="lblserialProductname" runat="server" Text="Product Name"></asp:Label>
                                                    &nbsp:&nbsp<asp:Label ID="lblProductNameValue" runat="server"
                                                        Text="0"></asp:Label>
                                                    <br />
                                                </div>
                                                <div id="pnlSerialNumber" runat="server" class="col-md-12">
                                                    <div class="col-md-6">
                                                        <asp:Label ID="Label15" runat="server" Text="<%$ Resources:Attendance, File Upload%>"></asp:Label>
                                                        <div class="input-group" style="width: 100%;">
                                                            <cc1:AsyncFileUpload ID="FULogoPath"
                                                                OnClientUploadStarted="FUAll_UploadStarted"
                                                                OnClientUploadError="FUAll_UploadError"
                                                                OnClientUploadComplete="FUAll_UploadComplete"
                                                                OnUploadedComplete="FUAll_FileUploadComplete"
                                                                runat="server" CssClass="form-control"
                                                                CompleteBackColor="White"
                                                                UploaderStyle="Traditional"
                                                                UploadingBackColor="#CCFFFF"
                                                                ThrobberID="FUAll_ImgLoader" Width="100%" />
                                                            <div class="input-group-btn" style="border: solid 1px #d2d6de; width: 35px;">
                                                                <asp:Image ID="FUAll_Img_Right" runat="server" Width="30px" Height="30px" ImageUrl="../Images/Allow.png" Style="display: none" />
                                                                <asp:Image ID="FUAll_Img_Wrong" runat="server" Width="30px" Height="30px" ImageUrl="../Images/Delete1.png" Style="display: none" />
                                                                <asp:Image ID="FUAll_ImgLoader" runat="server" ImageUrl="../Images/loader.gif" />
                                                            </div>
                                                        </div>
                                                        <br />
                                                    </div>
                                                    <div class="col-md-6" style="text-align: center">
                                                        <asp:Button ID="Btnloadfile" runat="server" Text="Load" CssClass="btn btn-primary"
                                                            OnClick="Btnloadfile_Click" />

                                                        <asp:Button ID="btnexecute" runat="server" Text="Execute" CssClass="btn btn-primary"
                                                            OnClick="btnexecute_Click" />

                                                        <asp:Button ID="btnLoadTempSerial" runat="server" Text="Load Temp Serial" CssClass="btn btn-primary"
                                                            OnClick="btnLoadTempSerial_Click" />
                                                        <br />
                                                    </div>
                                                    <div class="col-md-12">
                                                        <div class="alert alert-info ">
                                                            <div class="row">
                                                                <div class="form-group">
                                                                    <div class="col-lg-3">
                                                                        <asp:TextBox ID="txtserachserialnumber" runat="server" CssClass="form-control"></asp:TextBox>
                                                                        <cc1:TextBoxWatermarkExtender ID="txtwatermarkup" runat="server" TargetControlID="txtserachserialnumber"
                                                                            WatermarkText="Search Serial Number">
                                                                        </cc1:TextBoxWatermarkExtender>
                                                                    </div>
                                                                    <div class="col-lg-2">
                                                                        <asp:ImageButton ID="btnsearchserial" runat="server" CausesValidation="False"
                                                                            ImageUrl="~/Images/search.png" OnClick="btnsearchserial_Click" Style="margin-top: -5px;" ToolTip="<%$ Resources:Attendance,Search %>"></asp:ImageButton>

                                                                        <asp:ImageButton ID="btnRefreshserial" runat="server" CausesValidation="False"
                                                                            ImageUrl="~/Images/refresh.png" OnClick="btnRefreshserial_Click" Style="width: 33px;"
                                                                            ToolTip="<%$ Resources:Attendance,Refresh %>"></asp:ImageButton>
                                                                    </div>
                                                                    <div class="col-lg-2">
                                                                        <h5>
                                                                            <asp:Label ID="Label17" runat="server" Text="Total :"></asp:Label>
                                                                            <asp:Label ID="txtselectedSerialNumber" runat="server" Text="0"></asp:Label></h5>
                                                                    </div>
                                                                    <div class="col-lg-2">
                                                                        <h5>
                                                                            <asp:Label ID="lblCount" runat="server"></asp:Label>
                                                                            <asp:Label ID="txtCount" runat="server" Text="0"></asp:Label></h5>
                                                                    </div>
                                                                </div>
                                                            </div>
                                                        </div>
                                                        <div class="box box-warning box-solid">
                                                            <div class="box-header with-border">
                                                                <h3 class="box-title"></h3>
                                                            </div>
                                                            <div class="box-body">
                                                                <div class="row">
                                                                    <div class="col-md-8">
                                                                        <div class="flow">
                                                                            <asp:GridView CssClass="table-striped table-bordered table table-hover" ID="gvSerialNumber" runat="server" AllowPaging="false" AutoGenerateColumns="False"
                                                                                AllowSorting="true" BorderStyle="Solid" Width="100%" Height="320px"
                                                                                PageSize="5" OnSorting="gvSerialNumber_OnSorting">
                                                                                <Columns>
                                                                                    <asp:TemplateField HeaderText="<%$ Resources:Attendance,Delete %>">
                                                                                        <ItemTemplate>
                                                                                            <asp:ImageButton ID="IbtnDelete" runat="server" CausesValidation="False" CommandArgument='<%# Eval("SerialNo") %>'
                                                                                                ImageUrl="~/Images/Erase.png" OnCommand="IbtnDeleteserialNumber_Command" Width="16px" />
                                                                                        </ItemTemplate>
                                                                                        <ItemStyle HorizontalAlign="Center" />
                                                                                    </asp:TemplateField>
                                                                                    <asp:TemplateField HeaderText="Serial Number" SortExpression="SerialNo">
                                                                                        <ItemTemplate>
                                                                                            <asp:Label ID="lblsrno" runat="server" Text='<%#Eval("SerialNo") %>'></asp:Label>
                                                                                        </ItemTemplate>
                                                                                        <ItemStyle HorizontalAlign="Center" />
                                                                                    </asp:TemplateField>
                                                                                    <asp:TemplateField HeaderText="Manufacturing Date">
                                                                                        <ItemTemplate>
                                                                                            <asp:Label ID="lblmfg" runat="server" Text='<%#Eval("ManufacturerDate") %>'></asp:Label>
                                                                                        </ItemTemplate>
                                                                                        <ItemStyle HorizontalAlign="Center" />
                                                                                    </asp:TemplateField>
                                                                                    <asp:TemplateField HeaderText="Batch No.">
                                                                                        <ItemTemplate>
                                                                                            <asp:Label ID="lblBatchNo" runat="server" Text='<%#Eval("BatchNo") %>'></asp:Label>
                                                                                        </ItemTemplate>
                                                                                        <ItemStyle HorizontalAlign="Center" />
                                                                                    </asp:TemplateField>
                                                                                </Columns>


                                                                                <PagerStyle CssClass="pagination-ys" />

                                                                            </asp:GridView>
                                                                        </div>
                                                                    </div>
                                                                    <div class="col-md-4">
                                                                        <asp:TextBox ID="txtSerialNo" Height="350px" runat="server" CssClass="form-control"
                                                                            TextMode="MultiLine"></asp:TextBox>
                                                                    </div>
                                                                </div>

                                                                <div class="row">
                                                                    <div class="col-md-8">
                                                                        <div class="flow">
                                                                            <asp:GridView CssClass="table-striped table-bordered table table-hover" ID="gvSerialIssue" runat="server"
                                                                                BorderStyle="Solid" Width="100%">
                                                                            </asp:GridView>
                                                                        </div>
                                                                    </div>
                                                                    <div class="col-md-4">
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
                            </div>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
                <div class="modal-footer">
                    <asp:UpdatePanel ID="Update_Modal_Button" runat="server">
                        <ContentTemplate>
                            <asp:Button ID="BtnSerialSave" runat="server" Text="<%$ Resources:Attendance,Save %>"
                                CssClass="btn btn-success" OnClick="BtnSerialSave_Click" />
                            <asp:Button ID="btnResetSerial" runat="server" Text="<%$ Resources:Attendance,Reset %>"
                                CssClass="btn btn-primary" OnClick="btnResetSerial_Click" />

                            <button type="button" class="btn btn-danger" data-dismiss="modal">
                                Close</button>
                            <asp:Button ID="btnDefault" runat="server" Style="visibility: hidden" />

                            <asp:Button ID="btnNotFound" runat="server" Text="Save Serial Issue"
                                Visible="false" CssClass="btn btn-success" OnClick="btnNotFound_Click" />
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
            </div>
        </div>
    </div>
    <div class="modal fade" id="ControlSettingModal" tabindex="-1" role="dialog" aria-labelledby="ControlSetting_ModalLabel" aria-hidden="true">
        <div class="modal-dialog modal-md">
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal">
                        <span aria-hidden="true">&times;</span><span class="sr-only">Close</span></button>
                    <h4 class="modal-title">
                        <asp:Label ID="lblUcSettingsTitle" runat="server" Text="Set Columns Visibility" />
                    </h4>
                </div>
                <div class="modal-body">
                    <UC:ucCtlSetting ID="ucCtlSetting" runat="server" />
                </div>
            </div>
        </div>
    </div>
    <asp:UpdateProgress ID="UpdateProgress6" runat="server" AssociatedUpdatePanelID="Update_Modal">
        <ProgressTemplate>
            <div class="modal_Progress">
                <div class="center_Progress">
                    <img alt="" src="../Images/ajax-loader2.gif" style="vertical-align: middle; background-color: White; border-radius: 10px;" />
                </div>
            </div>
        </ProgressTemplate>
    </asp:UpdateProgress>

    <asp:UpdateProgress ID="UpdateProgress7" runat="server" AssociatedUpdatePanelID="Update_Modal_Button">
        <ProgressTemplate>
            <div class="modal_Progress">
                <div class="center_Progress">
                    <img alt="" src="../Images/ajax-loader2.gif" style="vertical-align: middle; background-color: White; border-radius: 10px;" />
                </div>
            </div>
        </ProgressTemplate>
    </asp:UpdateProgress>

    <asp:UpdateProgress ID="UpdateProgress1" runat="server" AssociatedUpdatePanelID="Update_List">
        <ProgressTemplate>
            <div class="modal_Progress">
                <div class="center_Progress">
                    <img alt="" src="../Images/ajax-loader2.gif" style="vertical-align: middle; background-color: White; border-radius: 10px;" />
                </div>
            </div>
        </ProgressTemplate>
    </asp:UpdateProgress>

    <asp:UpdateProgress ID="UpdateProgress2" runat="server" AssociatedUpdatePanelID="Update_New">
        <ProgressTemplate>
            <div class="modal_Progress">
                <div class="center_Progress">
                    <img alt="" src="../Images/ajax-loader2.gif" style="vertical-align: middle; background-color: White; border-radius: 10px;" />
                </div>
            </div>
        </ProgressTemplate>
    </asp:UpdateProgress>

    <asp:UpdateProgress ID="UpdateProgress3" runat="server" AssociatedUpdatePanelID="Update_Bin">
        <ProgressTemplate>
            <div class="modal_Progress">
                <div class="center_Progress">
                    <img alt="" src="../Images/ajax-loader2.gif" style="vertical-align: middle; background-color: White; border-radius: 10px;" />
                </div>
            </div>
        </ProgressTemplate>
    </asp:UpdateProgress>

    <asp:UpdateProgress ID="UpdateProgress4" runat="server" AssociatedUpdatePanelID="Update_Sales_Invoice">
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
    <script src="../Script/ReportSystem.js"></script>
    <script src="../Script/master.js"></script>

    <script type="text/javascript">
        function getReportDataWithLoc(transId, locId) {
            $("#prgBar").css("display", "block");
            document.getElementById('<%= reportSystem.FindControl("hdnTransId").ClientID %>').value = transId;
            document.getElementById('<%= reportSystem.FindControl("hdnLocId").ClientID %>').value = locId;
            setReportData();
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

        function LI_New_Active() {
            $("#Li_Sales_Invoice").removeClass("active");
            $("#Sales_Invoice").removeClass("active");

            $("#Li_New").addClass("active");
            $("#New").addClass("active");
        }
        function LI_List_Active() {
            $("#Li_List").addClass("active");
            $("#List").addClass("active");

            $("#Li_New").removeClass("active");
            $("#New").removeClass("active");
        }
        function Hide_List() {
            var List_LI = document.getElementById("Li_List");
            List_LI.style.display = List_LI.style.display = 'none';

            var List = document.getElementById("List");
            List.style.display = List.style.display = 'none';
        }
        function Show_List() {
            var List_LI = document.getElementById("Li_List");
            List_LI.style.display = List_LI.style.display = '';

            var List = document.getElementById("List");
            List.style.display = List.style.display = '';
        }
        function Li_Tab_Bin() {
            document.getElementById('<%= Btn_Bin.ClientID %>').click();
        }
        function Li_Tab_Sales_Invoice() {
            document.getElementById('<%= Btn_Sales_Invoice.ClientID %>').click();
        }

        function Close_Modal_Popup() {
            document.getElementById('<%= Btn_View_Modal.ClientID %>').click();
        }
        function Show_Modal_Popup() {
            document.getElementById('<%= Btn_View_Modal.ClientID %>').click();
        }

        function count(clientId) {
            var txtInput = document.getElementById(clientId);
            if (event.keyCode == 13) {
                document.getElementById('<%= txtCount.ClientID %>').innerHTML = lineBreakCount(txtInput.value);
                PageMethods.BatchLotExpiry(txtInput.value, onSucess);
            }
            if (event.keyCode == 8 || event.keyCode == 46) {
                document.getElementById('<%= txtCount.ClientID %>').innerHTML = lineDelBreakCount(txtInput.value);
                PageMethods.BatchLotExpiry(txtInput.value, onSucess);
            }
        }
        function lineBreakCount(str) {
            try {
                return ((str.match(/[^\n]*\n[^\n]*/gi).length) + 1);
            } catch (e) {
                return 1;
            }
        }
        function lineDelBreakCount(str) {
            try {
                return ((str.match(/[^\n]*\n[^\n]*/gi).length) - 1);
            } catch (e) {
                return 0;
            }
        }
    </script>
    <script type="text/javascript">
        function FUAll_UploadComplete(sender, args) {
            document.getElementById('<%= FUAll_Img_Wrong.ClientID %>').style.display = "none";
            document.getElementById('<%= FUAll_Img_Right.ClientID %>').style.display = "";
        }
        function FUAll_UploadError(sender, args) {
            document.getElementById('<%= FUAll_Img_Right.ClientID %>').style.display = "none";
            document.getElementById('<%= FUAll_Img_Wrong.ClientID %>').style.display = "";
        }
        function FUAll_UploadStarted(sender, args) {

        }
        function IbtnPrint_Command(returnId) {
            window.open('../Sales/SalesReturnPrint.aspx?Id=' + returnId + '', 'window', 'width=1024, ');
        }
        function showUcControlsSettings() {
            $('#ControlSettingModal').modal('show');
        }

        function FUExcel_UploadComplete(sender, args) {
            document.getElementById('<%= FUExcel_Img_Wrong.ClientID %>').style.display = "none";
            document.getElementById('<%= FUExcel_Img_Right.ClientID %>').style.display = "";
        }
        function FUExcel_UploadError(sender, args) {
            document.getElementById('<%= FUExcel_Img_Right.ClientID %>').style.display = "none";
            document.getElementById('<%= FUExcel_Img_Wrong.ClientID %>').style.display = "";
            alert('Invalid File Type, Select Only .xls, .xlsx, .mdb, .accdb extension file');
        }
        function FUExcel_UploadStarted(sender, args) {
            var filename = args.get_fileName();

            var filext = filename.substring(filename.lastIndexOf(".") + 1);
            filext = filext.toLowerCase();
            if (filext == "xls" || filext == "xlsx" || filext == "mdb" || filext == "accdb") {
                return true;
            }
            else {
                throw {
                    name: "Invalid File Type",
                    level: "Error",
                    message: "Invalid File Type, Select Only .xls, .xlsx, .mdb, .accdb extension file",
                    htmlMessage: "Invalid File Type, Select Only .xls, .xlsx, .mdb, .accdb extension file"
                }
                return false;
            }
        }
    </script>
</asp:Content>
