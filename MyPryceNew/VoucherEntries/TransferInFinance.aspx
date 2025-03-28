<%@ Page Title="" Language="C#" MasterPageFile="~/ERPMaster.master" AutoEventWireup="true" CodeFile="TransferInFinance.aspx.cs" Inherits="VoucherEntries_TransferInFinance" %>

<%@ Register Src="~/WebUserControl/ucControlsSetting.ascx" TagName="ucCtlSetting" TagPrefix="UC" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="Head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="HeaderContent" runat="Server">
    <h1>
        <i class="fas fa-random"></i>
        <asp:Label ID="lblHeader" runat="server" Text="<%$ Resources:Attendance,Transfer In Finance%>"></asp:Label>
    </h1>
    <ol class="breadcrumb">
        <li><a href="#"><i class="fa fa-dashboard"></i>
            <asp:Label ID="Label6" runat="server" Text="<%$ Resources:Attendance,Finance Setup%>"></asp:Label></a></li>
        <li><a href="#"><i class="fa fa-dashboard"></i>
            <asp:Label ID="Label10" runat="server" Text="<%$ Resources:Attendance,Voucher Entries%>"></asp:Label></a></li>
        <li class="active">
            <asp:Label ID="Label5" runat="server" Text="<%$ Resources:Attendance,Transfer In Finance%>"></asp:Label></li>
    </ol>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="Server">
    <asp:UpdatePanel ID="Update_Button" runat="server">
        <ContentTemplate>
            <asp:Button ID="Btn_List" Style="display: none;" runat="server" OnClick="btnList_Click" Text="List" />
            <asp:Button ID="Btn_New" Style="display: none;" runat="server" OnClick="btnNew_Click" Text="New" />
            <asp:Button ID="Btn_Bin" Style="display: none;" runat="server" OnClick="btnBin_Click" Text="Bin" />
            <asp:Button ID="Btn_Voucher_Detail" Style="display: none;" runat="server" data-toggle="modal" data-target="#Voucher_Detail" Text="View Modal" />
            <asp:HiddenField runat="server" ID="hdnCanView" />
            <asp:HiddenField runat="server" ID="hdnCanEdit" />
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
                    <li id="Li_Bin"><a href="#Bin" onclick="Li_Tab_Bin()" data-toggle="tab">
                        <i class="fa fa-trash"></i>&nbsp;&nbsp;<asp:Label ID="Label13" runat="server" Text="<%$ Resources:Attendance,Bin %>"></asp:Label></a></li>
                    <li id="Li_New" runat="server" visible="false"><a onclick="Li_Tab_New()" href="#New" data-toggle="tab">
                        <asp:UpdatePanel ID="Update_Li" runat="server">
                            <ContentTemplate>
                                <i class="fa fa-file"></i>&nbsp;&nbsp;
                                <asp:Label ID="Lbl_Tab_New" runat="server" Text="<%$ Resources:Attendance,New%>"></asp:Label>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </a></li>
                    <li id="Li_List" class="active"><a href="#List" onclick="Li_Tab_List()" data-toggle="tab">
                        <i class="fa fa-list"></i>&nbsp;&nbsp;<asp:Label ID="Label1" runat="server" Text="<%$ Resources:Attendance,List %>"></asp:Label></a></li>

                </ul>
                <div class="tab-content">
                    <div class="tab-pane active" id="List">
                        <asp:UpdatePanel ID="Update_List" runat="server">
                            <ContentTemplate>
                                <div class="row">
                                    <div class="col-md-12">
                                        <div id="Div1" runat="server" class="box box-info collapsed-box">
                                            <div class="box-header with-border">
                                                <h3 class="box-title">
                                                    <asp:Label ID="Label2" runat="server" Text="Advance Search"></asp:Label></h3>
                                                &nbsp;&nbsp;|&nbsp;&nbsp;
					<asp:Label ID="lblTotalRecords" runat="server" Font-Bold="true" Text="<%$ Resources:Attendance,Total Records: 0 %>"></asp:Label>

                                                <div class="box-tools pull-right">
                                                    <button type="button" class="btn btn-box-tool" data-widget="collapse">
                                                        <i id="I1" runat="server" class="fa fa-plus"></i>
                                                    </button>
                                                </div>
                                            </div>
                                            <div class="box-body">
                                                <div class="col-md-4">
                                                    <asp:Label ID="lblLocationList" runat="server" Text="<%$ Resources:Attendance,Location %>"></asp:Label>
                                                    <div class="input-group">
                                                        <asp:DropDownList ID="ddlLocationList" runat="server" AutoPostBack="true" CssClass="form-control" OnSelectedIndexChanged="ddlLocationList_SelectedIndexChanged" />
                                                    </div>
                                                </div>
                                                <div class="col-md-4">
                                                    <asp:Label ID="Label21" runat="server" Text="<%$ Resources:Attendance,From Date %>"></asp:Label>
                                                    <asp:TextBox ID="txtFromDate" runat="server" CssClass="form-control" />
                                                    <cc1:CalendarExtender OnClientShown="showCalendar" ID="CalendarExtender1" runat="server" TargetControlID="txtFromDate"
                                                        Format="dd-MMM-yyyy" />
                                                    <br />
                                                </div>
                                                <div class="col-md-4">
                                                    <asp:Label ID="Label22" runat="server" Text="<%$ Resources:Attendance,To Date %>"></asp:Label>
                                                    <asp:TextBox ID="txtToDate" runat="server" CssClass="form-control" />
                                                    <cc1:CalendarExtender OnClientShown="showCalendar" ID="CalendarExtender2" runat="server" TargetControlID="txtToDate"
                                                        Format="dd-MMM-yyyy" />
                                                    <br />
                                                </div>

                                                <div class="col-lg-3">
                                                    <asp:DropDownList ID="ddlFieldName" runat="server" CssClass="form-control"
                                                        OnSelectedIndexChanged="ddlFieldName_SelectedIndexChanged" AutoPostBack="true">
                                                        <asp:ListItem Text="<%$ Resources:Attendance, Voucher No %>" Value="Voucher_No"></asp:ListItem>
                                                        <asp:ListItem Text="<%$ Resources:Attendance, Voucher Type %>" Value="Voucher_Type"
                                                            Selected="True"></asp:ListItem>
                                                        <asp:ListItem Text="<%$ Resources:Attendance, Finance Code %>" Value="Finance_Code"></asp:ListItem>
                                                        <asp:ListItem Text="<%$ Resources:Attendance,Other Account %>" Value="Other_Account_Name"></asp:ListItem>
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
                                                <div class="col-lg-3">
                                                    <asp:Panel ID="Panel6" runat="server" DefaultButton="btnbind">
                                                        <asp:TextBox ID="txtValue" Visible="false" runat="server" CssClass="form-control"></asp:TextBox>
                                                        <asp:DropDownList ID="ddlVoucherValue" runat="server" Visible="false" CssClass="form-control">
                                                            <asp:ListItem Text="--Select--" Value="--Select--"></asp:ListItem>
                                                            <asp:ListItem Text="Purchase Invoice" Value="PI"></asp:ListItem>
                                                            <asp:ListItem Text="Purchase Return" Value="PR"></asp:ListItem>
                                                            <asp:ListItem Text="Journal Vouchers" Value="JV"></asp:ListItem>
                                                            <asp:ListItem Text="Payment Vouchers" Value="PV"></asp:ListItem>
                                                            <asp:ListItem Text="Sales Invoice" Value="SI"></asp:ListItem>
                                                            <asp:ListItem Text="Sales Return" Value="SR"></asp:ListItem>
                                                            <asp:ListItem Text="Receive Vouchers" Value="RV"></asp:ListItem>
                                                        </asp:DropDownList>
                                                    </asp:Panel>
                                                </div>
                                                <div class="col-lg-4">
                                                    <asp:Button ID="btnPostInFinance" runat="server" Visible="false" ValidationGroup="Grid"
                                                        Text="<%$ Resources:Attendance,Post In Finance %>" CssClass="btn btn-primary pull-left" OnClick="btnPostInFinance_Click" />
                                                    <cc1:ConfirmButtonExtender ID="ConfirmButtonExtender1" runat="server" ConfirmText="<%$ Resources:Attendance,Are you sure to Post record ?%>"
                                                        TargetControlID="btnPostInFinance">
                                                    </cc1:ConfirmButtonExtender>
                                                    &nbsp;&nbsp;
                                              
                                                    <asp:LinkButton ID="btnbind" runat="server" CausesValidation="False"
                                                        OnClick="btnbindrpt_Click" ToolTip="<%$ Resources:Attendance,Search %>"><span class="fa fa-search"  style="font-size:25px;"></span></asp:LinkButton>&nbsp;&nbsp;&nbsp;
                                                    &nbsp;&nbsp;
                                                    <asp:LinkButton ID="btnRefresh" runat="server" CausesValidation="False"
                                                        OnClick="btnRefreshReport_Click"
                                                        ToolTip="<%$ Resources:Attendance,Refresh %>"><span class="fa fa-repeat"  style="font-size:25px;"></span></asp:LinkButton>&nbsp;&nbsp;&nbsp;
                                                    &nbsp;&nbsp;
                                                    <asp:LinkButton ID="ImgbtnSelectAll" Visible="false" runat="server" OnClick="ImgbtnSelectAll_Click"
                                                        ToolTip="<%$ Resources:Attendance, Select All %>" AutoPostBack="true" ImageUrl="~/Images/selectAll.png"><span class="fas fa-th"  style="font-size:25px;"></span></asp:LinkButton>
                                                    &nbsp;&nbsp;
                                                    <asp:LinkButton ID="ImgButtonDelete" runat="server" CausesValidation="False" Visible="false"
                                                        OnClick="ImgButtonDelete_Click" ToolTip="<%$ Resources:Attendance,Delete %>"><span class="fa fa-remove"  style="font-size:25px;"></span></asp:LinkButton>&nbsp;&nbsp;
                                                    <cc1:ConfirmButtonExtender ID="confirm2" runat="server" ConfirmText="<%$ Resources:Attendance,Are you sure you want to delete the record?%>"
                                                        TargetControlID="ImgButtonDelete">
                                                    </cc1:ConfirmButtonExtender>
                                                    <asp:LinkButton ID="btnGvListSetting" ImageAlign="Right" ToolTip="List Settings" runat="server" OnClick="btnGvListSetting_Click" Visible="false"><span class="fa fa-wrench"  style="font-size:25px;"></span></asp:LinkButton>

                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>

                                <div class="box box-warning box-solid">

                                    <div class="box-body">
                                        <div class="row">
                                            <div class="col-md-12" style="text-align: center">
                                                <asp:CustomValidator ID="CustomValidator1" ValidationGroup="Grid" runat="server" Font-Bold="true" ErrorMessage="Please select at least one record."
                                                    ClientValidationFunction="Validate_Grid" ForeColor="Red"></asp:CustomValidator>
                                            </div>
                                            <div class="col-md-12">
                                                <div class="flow">
                                                    <asp:Label ID="lblSelectedRecord" runat="server" Visible="false"></asp:Label>
                                                    <asp:GridView CssClass="table-striped table-bordered table table-hover" ID="GvVoucher" PageSize='<%# int.Parse(Session["GridSize"].ToString()) %>' runat="server"
                                                        AutoGenerateColumns="False" Width="100%" AllowPaging="True" OnPageIndexChanging="GvVoucher_PageIndexChanging"
                                                        AllowSorting="True" OnSorting="GvVoucher_Sorting">

                                                        <Columns>
                                                            <asp:TemplateField>
                                                                <HeaderTemplate>
                                                                    <asp:CheckBox ID="chkCurrent" runat="server" OnCheckedChanged="chkCurrent_CheckedChanged"
                                                                        AutoPostBack="true" />
                                                                </HeaderTemplate>
                                                                <ItemTemplate>
                                                                    <asp:CheckBox ID="chkSelect" runat="server" AutoPostBack="true" Visible="true" OnCheckedChanged="chkSelect_CheckedChanged" />
                                                                </ItemTemplate>
                                                                <ItemStyle HorizontalAlign="Center" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Action">
                                                                <ItemTemplate>
                                                                    <div class="dropdown" style="position: absolute;">
                                                                        <button class="btn btn-default dropdown-toggle" type="button" data-toggle="dropdown">
                                                                            <i class="fa fa-ellipsis-h" aria-hidden="true"></i>
                                                                        </button>
                                                                        <ul class="dropdown-menu">
                                                                            <li <%= hdnCanView.Value=="true"?"style='display:block'":"style='display:none'" %>>
                                                                                <asp:LinkButton ID="lnkViewDetail" runat="server" CommandArgument='<%# Eval("Trans_Id") %>'
                                                                                    ToolTip="<%$ Resources:Attendance,View %>"
                                                                                    OnCommand="lnkViewDetail_Command" CausesValidation="False"><i class="fa fa-eye"></i>View</asp:LinkButton>
                                                                            </li>
                                                                            <li <%= hdnCanEdit.Value=="true"?"style='display:block'":"style='display:none'" %>>
                                                                                <asp:LinkButton ID="btnEdit" runat="server" CommandArgument='<%# Eval("Trans_Id") %>'
                                                                                    Visible="true" OnCommand="btnEdit_Command" CausesValidation="False"
                                                                                    ToolTip="<%$ Resources:Attendance,Edit %>"><i class="fa fa-pencil"></i>Edit </asp:LinkButton>
                                                                            </li>
                                                                        </ul>
                                                                    </div>

                                                                </ItemTemplate>
                                                            </asp:TemplateField>

                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,Voucher No %>" SortExpression="Voucher_No">
                                                                <ItemTemplate>
                                                                    <asp:LinkButton ID="lnkgvVoucherNo" runat="server" Text='<%# Eval("Voucher_No") %>'
                                                                        OnClick="lblgvVoucherNoCommon_Click" Font-Bold="true" CommandArgument='<%#Eval("Ref_Id") + "," + Eval("Ref_Type")+ "," + Eval("Trans_Id")+ "," + Eval("Location_Id")%>' />
                                                                    <asp:Label ID="lblgvTransId" Visible="false" runat="server" Text='<%#Eval("Trans_Id") %>' />
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance, Voucher Date %>" SortExpression="Voucher_Date">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblgvVoucherDate" Width="80px" runat="server" Text='<%#GetDate(Eval("Voucher_Date").ToString()) %>' />
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance, Voucher Type %>" SortExpression="Voucher_Type">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblgvVoucherType" runat="server" Text='<%#Eval("Voucher_Type") %>' />
                                                                    <asp:Label ID="lblgvrefType" runat="server" Text='<%#Eval("Ref_Type") %>' Visible="false" />
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance, Other Account%>" SortExpression="Other_Account">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblgvOtherAccount" runat="server" Text='<%# Eval("Other_Account_Name")%>' />
                                                                    <%--<asp:Label ID="Label2" runat="server" Text='<%#Ac_ParameterMaster.GetOtherAccountNameForDetail(Eval("Other_Account").ToString(),Eval("account_no").ToString(),Session["CompId"].ToString()) %>' />--%>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance, Narration %>" SortExpression="Narration">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblgvFinanceCode" runat="server" Text='<%#Eval("Narration") %>' />
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,Voucher Amount%>">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblgvExchangerate" runat="server" Text='<%#GetVoucherAmount(Eval("Trans_Id").ToString()) %>' />
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance, Currency %>" SortExpression="Currency_Id">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblgvCurrency" runat="server" Text='<%# GetCurrencyName(Eval("Currency_Id").ToString()) %>' />
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance, Status %>" SortExpression="Field3">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblApprovalStatus" runat="server" Text='<%# Eval("Field3") %>' />
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance, Created By %>">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblgvCreatedBy" runat="server" Text='<%#Created_By_Emp_Name(Eval("CreatedBy").ToString()) %>' />
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance, Modified By %>">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblgvModifiedBy" runat="server" Text='<%#Created_By_Emp_Name(Eval("ModifiedBy").ToString()) %>' />
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                        </Columns>

                                                        <PagerStyle CssClass="pagination-ys" />

                                                    </asp:GridView>
                                                    <asp:HiddenField ID="HDFSortbin" runat="server" />
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </div>

                    <div class="tab-pane" id="New">
                        <asp:UpdatePanel ID="Update_New" runat="server">
                            <ContentTemplate>
                                <div class="row">
                                    <div class="col-md-12">
                                        <div class="box box-primary">
                                            <div class="box-body">
                                                <div class="form-group">
                                                    <div class="col-md-12">
                                                        <asp:ImageButton ID="btnControlsSetting" ImageAlign="Right" ToolTip="Controls Setting" runat="server" ImageUrl="~/Images/setting.png" OnClick="btnControlsSetting_Click" Style="width: 32px; height: 32px" Visible="false" />
                                                    </div>
                                                    <div class="col-md-6">
                                                        <asp:Label ID="lblFinanceCode" runat="server" Text="<%$ Resources:Attendance,Finance Code %>" />
                                                        <asp:TextBox ID="txtFinanceCode" runat="server" CssClass="form-control" />
                                                        <br />
                                                    </div>
                                                    <div class="col-md-12">
                                                    </div>
                                                    <div class="col-md-6">
                                                        <asp:Label ID="lblLocation" runat="server" Text="<%$ Resources:Attendance,Location To %>" />
                                                        <a style="color: Red">*</a>
                                                        <asp:RequiredFieldValidator EnableClientScript="true" Style="float: right;" runat="server" ID="RequiredFieldValidator3" ValidationGroup="Save"
                                                            Display="Dynamic" SetFocusOnError="true" ControlToValidate="txtToLocation" ErrorMessage="<%$ Resources:Attendance,Enter Location To%>"></asp:RequiredFieldValidator>

                                                        <asp:TextBox ID="txtToLocation" runat="server" CssClass="form-control" BackColor="#eeeeee"
                                                            OnTextChanged="txtToLocation_TextChanged" AutoPostBack="true" Enabled="false" />
                                                        <cc1:AutoCompleteExtender ID="txtToLocation_AutoCompleteExtender" runat="server"
                                                            DelimiterCharacters="" Enabled="True" CompletionInterval="100" CompletionSetCount="1"
                                                            MinimumPrefixLength="1" ServiceMethod="GetCompletionListLocation" ServicePath=""
                                                            TargetControlID="txtToLocation" UseContextKey="True" CompletionListCssClass="autoCompleteList" CompletionListItemCssClass="autoCompleteListItem" CompletionListHighlightedItemCssClass="autoCompleteSelectedListItem" OnClientShown="resetPosition">
                                                        </cc1:AutoCompleteExtender>
                                                        <br />
                                                    </div>
                                                    <div class="col-md-6">
                                                        <asp:Label ID="lblDepartment" runat="server" Text="<%$ Resources:Attendance,Department %>" />
                                                        <a style="color: Red">*</a>
                                                        <asp:RequiredFieldValidator EnableClientScript="true" Style="float: right;" runat="server" ID="RequiredFieldValidator1" ValidationGroup="Save"
                                                            Display="Dynamic" SetFocusOnError="true" ControlToValidate="txtDepartment" ErrorMessage="<%$ Resources:Attendance,Enter Department%>"></asp:RequiredFieldValidator>

                                                        <asp:TextBox ID="txtDepartment" runat="server" CssClass="form-control" BackColor="#eeeeee"
                                                            OnTextChanged="txtDepartment_TextChanged" AutoPostBack="true" />
                                                        <cc1:AutoCompleteExtender ID="txtDepartment_AutoCompleteExtender" runat="server"
                                                            DelimiterCharacters="" Enabled="True" CompletionInterval="100" CompletionSetCount="1"
                                                            MinimumPrefixLength="1" ServiceMethod="GetCompletionListDepartment" ServicePath=""
                                                            TargetControlID="txtDepartment" UseContextKey="True" CompletionListCssClass="autoCompleteList" CompletionListItemCssClass="autoCompleteListItem" CompletionListHighlightedItemCssClass="autoCompleteSelectedListItem" OnClientShown="resetPosition">
                                                        </cc1:AutoCompleteExtender>
                                                        <br />
                                                    </div>
                                                    <div class="col-md-6">
                                                        <asp:Label ID="lblVoucherType" runat="server" Text="<%$ Resources:Attendance,Voucher Type %>" />
                                                        <a style="color: Red">*</a>
                                                        <asp:RequiredFieldValidator EnableClientScript="true" Style="float: right;" runat="server" ID="RequiredFieldValidator11" ValidationGroup="Add" Display="Dynamic"
                                                            SetFocusOnError="true" ControlToValidate="ddlVoucherType" InitialValue="--Select--" ErrorMessage="<%$ Resources:Attendance,Select Voucher Type %>" />

                                                        <asp:DropDownList ID="ddlVoucherType" runat="server" CssClass="form-control"
                                                            AutoPostBack="True" OnSelectedIndexChanged="ddlVoucherType_SelectedIndexChanged">
                                                            <asp:ListItem Text="--Select--" Value="--Select--"></asp:ListItem>
                                                            <asp:ListItem Text="Purchase Invoice" Value="PI"></asp:ListItem>
                                                            <asp:ListItem Text="Purchase Return" Value="PR"></asp:ListItem>
                                                            <asp:ListItem Text="Journal Vouchers" Value="JV"></asp:ListItem>
                                                            <asp:ListItem Text="Payment Vouchers" Value="PV"></asp:ListItem>
                                                            <asp:ListItem Text="Sales Invoice" Value="SI"></asp:ListItem>
                                                            <asp:ListItem Text="Receive Vouchers" Value="RV"></asp:ListItem>
                                                            <asp:ListItem Text="Sales Return" Value="SR"></asp:ListItem>
                                                            <asp:ListItem Text="Supplier Payment Vouchers" Value="SPV"></asp:ListItem>
                                                            <asp:ListItem Text="Customer Receive Vouchers" Value="CRV"></asp:ListItem>
                                                            <asp:ListItem Text="Customer Debit Note" Value="CDN"></asp:ListItem>
                                                            <asp:ListItem Text="Supplier Debit Note" Value="SDN"></asp:ListItem>
                                                            <asp:ListItem Text="Customer Credit Note" Value="CCN"></asp:ListItem>
                                                            <asp:ListItem Text="Supplier Credit Note" Value="SCN"></asp:ListItem>
                                                            <asp:ListItem Text="PDC Customer" Value="PDC"></asp:ListItem>
                                                            <asp:ListItem Text="PDC Supplier" Value="PDS"></asp:ListItem>
                                                        </asp:DropDownList>
                                                        <%--<asp:TextBox ID="txtVoucherType" runat="server" CssClass="form-control" />--%>
                                                        <br />
                                                    </div>
                                                    <div class="col-md-12">
                                                    </div>
                                                    <div class="col-md-6">
                                                        <asp:Label ID="lblVoucherNo" runat="server" Text="<%$ Resources:Attendance,Voucher No. %>" />
                                                        <a style="color: Red">*</a>
                                                        <asp:RequiredFieldValidator EnableClientScript="true" Style="float: right;" runat="server" ID="RequiredFieldValidator2" ValidationGroup="Save"
                                                            Display="Dynamic" SetFocusOnError="true" ControlToValidate="txtVoucherNo" ErrorMessage="<%$ Resources:Attendance,Enter Voucher No%>"></asp:RequiredFieldValidator>

                                                        <asp:TextBox ID="txtVoucherNo" runat="server" CssClass="form-control" />
                                                        <br />
                                                    </div>
                                                    <div class="col-md-6">
                                                        <asp:Label ID="lblVoucherDate" runat="server" Text="<%$ Resources:Attendance,Voucher Date %>" />
                                                        <a style="color: Red">*</a>
                                                        <asp:RequiredFieldValidator EnableClientScript="true" Style="float: right;" runat="server" ID="RequiredFieldValidator4" ValidationGroup="Save"
                                                            Display="Dynamic" SetFocusOnError="true" ControlToValidate="txtVoucherDate" ErrorMessage="<%$ Resources:Attendance,Enter Voucher Date%>"></asp:RequiredFieldValidator>

                                                        <asp:TextBox ID="txtVoucherDate" runat="server" CssClass="form-control" />
                                                        <cc1:CalendarExtender OnClientShown="showCalendar" ID="Calender_VoucherDate" runat="server" TargetControlID="txtVoucherDate"
                                                            Format="dd/MMM/yyyy" />
                                                        <br />
                                                    </div>
                                                    <div class="col-md-12" style="text-align: center">
                                                        <asp:RadioButton ID="rbCashPayment" Checked="true" runat="server" Text="Cash Payment"
                                                            GroupName="Pay" OnCheckedChanged="rbCashPayment_CheckedChanged" AutoPostBack="true" />
                                                        <asp:RadioButton ID="rbChequePayment" runat="server" Text="Cheque Payment"
                                                            GroupName="Pay" OnCheckedChanged="rbCashPayment_CheckedChanged" AutoPostBack="true" />
                                                        <asp:CheckBox ID="chkReconcile" Visible="false" runat="server" Text="<%$ Resources:Attendance, Reconcile%>" />
                                                        <br />
                                                    </div>
                                                    <div id="trCheque1" runat="server" visible="false">
                                                        <div class="col-md-6">
                                                            <asp:Label ID="lblChequeIssueDate" runat="server" Text="Cheque Issue Date" />
                                                            <asp:TextBox ID="txtChequeIssueDate" runat="server" CssClass="form-control" />
                                                            <cc1:CalendarExtender OnClientShown="showCalendar" ID="CalendarExtender_txtChequeIssueDate" runat="server" TargetControlID="txtChequeIssueDate"
                                                                Format="dd/MMM/yyyy" />
                                                            <br />
                                                        </div>
                                                        <div class="col-md-6">
                                                            <asp:Label ID="lblChequeClearDate" runat="server" Text="Cheque Clear Date" />
                                                            <asp:TextBox ID="txtChequeClearDate" runat="server" CssClass="form-control" />
                                                            <cc1:CalendarExtender OnClientShown="showCalendar" ID="CalendarExtender_ChequeClearDate" runat="server" TargetControlID="txtChequeClearDate"
                                                                Format="dd/MMM/yyyy" />
                                                            <br />
                                                        </div>
                                                    </div>
                                                    <div class="col-md-6" id="trCheque2" runat="server" visible="false">
                                                        <asp:Label ID="lblChequeNo" runat="server" Text="Cheque No." />
                                                        <asp:TextBox ID="txtChequeNo" runat="server" CssClass="form-control" />
                                                        <br />
                                                    </div>
                                                    <div class="col-md-12">
                                                        <asp:Label ID="lblReference" runat="server" Text="<%$ Resources:Attendance,Refrence%>" />
                                                        <asp:TextBox ID="txtReference" runat="server" CssClass="form-control" />
                                                        <br />
                                                    </div>
                                                    <div class="col-md-12">

                                                        <div class="row">
                                                            <div class="col-md-12">
                                                                <div class="box box-primary">
                                                                    <div class="box-header with-border">
                                                                        <h3 class="box-title">
                                                                            <asp:Label ID="Label23" runat="server" Text="<%$ Resources:Attendance,Detail View%>" /></h3>
                                                                        <div class="box-tools pull-right">
                                                                            <button type="button" class="btn btn-box-tool" data-widget="collapse">
                                                                                <i class="fa fa-minus"></i>
                                                                            </button>
                                                                        </div>
                                                                    </div>
                                                                    <div class="box-body">
                                                                        <div class="form-group">
                                                                            <div class="col-md-6">
                                                                                <asp:Label ID="lblAccountNo" runat="server" Text="<%$ Resources:Attendance,Account No. %>" />
                                                                                <a style="color: Red">*</a>
                                                                                <asp:RequiredFieldValidator EnableClientScript="true" Style="float: right;" runat="server" ID="RequiredFieldValidator5" ValidationGroup="Add"
                                                                                    Display="Dynamic" SetFocusOnError="true" ControlToValidate="txtAccountNo" ErrorMessage="<%$ Resources:Attendance,Enter Account No%>"></asp:RequiredFieldValidator>

                                                                                <asp:TextBox ID="txtAccountNo" runat="server" CssClass="form-control" AutoPostBack="true"
                                                                                    OnTextChanged="txtAccountNo_TextChanged" BackColor="#eeeeee" />
                                                                                <cc1:AutoCompleteExtender ID="AutoCompleteExtender2" runat="server" DelimiterCharacters=""
                                                                                    Enabled="True" ServiceMethod="GetCompletionListAccountNo" ServicePath="" CompletionInterval="100"
                                                                                    MinimumPrefixLength="1" CompletionSetCount="1" TargetControlID="txtAccountNo"
                                                                                    UseContextKey="True" CompletionListCssClass="autoCompleteList" CompletionListItemCssClass="autoCompleteListItem" CompletionListHighlightedItemCssClass="autoCompleteSelectedListItem" OnClientShown="resetPosition">
                                                                                </cc1:AutoCompleteExtender>
                                                                                <asp:HiddenField ID="hdnNewAccountNo" runat="server" Value="0" />
                                                                                <br />
                                                                            </div>
                                                                            <div class="col-md-6" id="trSupplier" runat="server" visible="false">
                                                                                <asp:Label ID="lblSupplierName" runat="server" Text="<%$ Resources:Attendance,Supplier %>" />
                                                                                <a style="color: Red">*</a>
                                                                                <asp:RequiredFieldValidator EnableClientScript="true" Style="float: right;" runat="server" ID="RequiredFieldValidator6" ValidationGroup="Save"
                                                                                    Display="Dynamic" SetFocusOnError="true" ControlToValidate="txtSupplierName" ErrorMessage="<%$ Resources:Attendance,Enter Supplier%>"></asp:RequiredFieldValidator>

                                                                                <asp:TextBox ID="txtSupplierName" runat="server" CssClass="form-control" BackColor="#eeeeee"
                                                                                    AutoPostBack="True" OnTextChanged="txtSupplierName_TextChanged" />
                                                                                <cc1:AutoCompleteExtender ID="txtSupplierName_AutoCompleteExtender" runat="server"
                                                                                    CompletionInterval="100" DelimiterCharacters="" Enabled="True" MinimumPrefixLength="1"
                                                                                    ServiceMethod="GetCompletionList" ServicePath="" TargetControlID="txtSupplierName"
                                                                                    UseContextKey="True" CompletionListCssClass="autoCompleteList" CompletionListItemCssClass="autoCompleteListItem" CompletionListHighlightedItemCssClass="autoCompleteSelectedListItem" OnClientShown="resetPosition">
                                                                                </cc1:AutoCompleteExtender>
                                                                                <br />
                                                                            </div>
                                                                            <div class="col-md-6" id="trCustomer" runat="server" visible="false">
                                                                                <asp:Label ID="lblCustomer" runat="server" Text="<%$ Resources:Attendance,Customer %>" />
                                                                                <a style="color: Red">*</a>
                                                                                <asp:RequiredFieldValidator EnableClientScript="true" Style="float: right;" runat="server" ID="RequiredFieldValidator7" ValidationGroup="Save"
                                                                                    Display="Dynamic" SetFocusOnError="true" ControlToValidate="txtCustomerName" ErrorMessage="<%$ Resources:Attendance,Enter Customer%>"></asp:RequiredFieldValidator>

                                                                                <asp:TextBox ID="txtCustomerName" runat="server" CssClass="form-control" BackColor="#eeeeee"
                                                                                    AutoPostBack="True" OnTextChanged="txtCustomerName_TextChanged" />
                                                                                <cc1:AutoCompleteExtender ID="txtCustomerName_AutoCompleteExtender" runat="server"
                                                                                    CompletionInterval="100" DelimiterCharacters="" Enabled="True" MinimumPrefixLength="1"
                                                                                    ServiceMethod="GetCompletionListCustomer" ServicePath="" TargetControlID="txtCustomerName"
                                                                                    UseContextKey="True" CompletionListCssClass="autoCompleteList" CompletionListItemCssClass="autoCompleteListItem" CompletionListHighlightedItemCssClass="autoCompleteSelectedListItem" OnClientShown="resetPosition">
                                                                                </cc1:AutoCompleteExtender>
                                                                                <br />
                                                                            </div>

                                                                            <div class="col-md-12">
                                                                            </div>
                                                                            <div class="col-md-6">
                                                                                <asp:Label ID="lblDebitAmount" runat="server" Text="<%$ Resources:Attendance,Debit Amount %>" />
                                                                                <asp:TextBox ID="txtDebitAmount" runat="server" CssClass="form-control" OnTextChanged="txtDebitAmount_OnTextChanged"
                                                                                    AutoPostBack="true" />
                                                                                <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" runat="server" Enabled="True"
                                                                                    TargetControlID="txtDebitAmount" ValidChars="1,2,3,4,5,6,7,8,9,0,.">
                                                                                </cc1:FilteredTextBoxExtender>
                                                                                <br />
                                                                            </div>
                                                                            <div class="col-md-6">
                                                                                <asp:Label ID="lblCreditAmount" runat="server" Text="<%$ Resources:Attendance,Credit Amount %>" />
                                                                                <asp:TextBox ID="txtCreditAmount" runat="server" CssClass="form-control" OnTextChanged="txtCreditAmount_OnTextChanged"
                                                                                    AutoPostBack="true" />
                                                                                <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender3" runat="server" Enabled="True"
                                                                                    TargetControlID="txtCreditAmount" ValidChars="1,2,3,4,5,6,7,8,9,0,.">
                                                                                </cc1:FilteredTextBoxExtender>
                                                                                <br />
                                                                            </div>
                                                                            <div class="col-md-12">
                                                                                <asp:Label ID="lblNarration" runat="server" Text="<%$ Resources:Attendance,Narration %>" />
                                                                                <asp:TextBox ID="txtNarration" TextMode="MultiLine" runat="server"
                                                                                    CssClass="form-control" />
                                                                                <br />
                                                                            </div>
                                                                            <div id="trEmp" runat="server" visible="false">
                                                                                <div class="col-md-6">
                                                                                    <asp:Label ID="lblCostCenter" runat="server" Text="<%$ Resources:Attendance,Cost Center %>" />
                                                                                    <asp:TextBox ID="txtCostCenter" runat="server" CssClass="form-control" />
                                                                                    <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender2" runat="server" Enabled="True"
                                                                                        TargetControlID="txtCostCenter" ValidChars="1,2,3,4,5,6,7,8,9,0,.">
                                                                                    </cc1:FilteredTextBoxExtender>
                                                                                    <br />
                                                                                </div>
                                                                                <div class="col-md-6">
                                                                                    <asp:Label ID="lblEmployee" runat="server" Text="<%$ Resources:Attendance,Employee %>" />
                                                                                    <a style="color: Red">*</a>
                                                                                    <asp:RequiredFieldValidator EnableClientScript="true" Style="float: right;" runat="server" ID="RequiredFieldValidator8" ValidationGroup="Save"
                                                                                        Display="Dynamic" SetFocusOnError="true" ControlToValidate="txtEmployee" ErrorMessage="<%$ Resources:Attendance,Enter Employee%>"></asp:RequiredFieldValidator>

                                                                                    <asp:TextBox ID="txtEmployee" runat="server" CssClass="form-control" AutoPostBack="true"
                                                                                        OnTextChanged="txtEmployee_TextChanged" BackColor="#eeeeee" />
                                                                                    <cc1:AutoCompleteExtender ID="AutoCompleteExtender_Employee" runat="server" DelimiterCharacters=""
                                                                                        Enabled="True" ServiceMethod="GetCompletionListEmployee" ServicePath="" CompletionInterval="100"
                                                                                        MinimumPrefixLength="1" CompletionSetCount="1" TargetControlID="txtEmployee"
                                                                                        UseContextKey="True" CompletionListCssClass="autoCompleteList" CompletionListItemCssClass="autoCompleteListItem" CompletionListHighlightedItemCssClass="autoCompleteSelectedListItem" OnClientShown="resetPosition">
                                                                                    </cc1:AutoCompleteExtender>
                                                                                    <asp:HiddenField ID="hdnEmployeeId" runat="server" Value="0" />
                                                                                    <br />
                                                                                </div>
                                                                            </div>

                                                                            <div class="col-md-6">
                                                                                <asp:Label ID="lblDCurrency" runat="server" Text="<%$ Resources:Attendance,Currency %>" />
                                                                                <asp:DropDownList ID="ddlDCurrency" runat="server" CssClass="form-control"
                                                                                    AutoPostBack="True" OnSelectedIndexChanged="ddlDCurrency_SelectedIndexChanged" />
                                                                                <asp:HiddenField ID="hdnCurrencyId" runat="server" Value="0" />
                                                                                <br />
                                                                            </div>
                                                                            <div class="col-md-6">
                                                                                <asp:Label ID="lblDExchangeRate" runat="server" Text="<%$ Resources:Attendance,Exchange Rate %>" />
                                                                                <a style="color: Red">*</a>
                                                                                <asp:RequiredFieldValidator EnableClientScript="true" Style="float: right;" runat="server" ID="RequiredFieldValidator9" ValidationGroup="Add"
                                                                                    Display="Dynamic" SetFocusOnError="true" ControlToValidate="txtDExchangeRate" ErrorMessage="<%$ Resources:Attendance,Enter Exchange Rate%>"></asp:RequiredFieldValidator>

                                                                                <asp:TextBox ID="txtDExchangeRate" runat="server" OnTextChanged="txtDExchangeRate_OnTextChanged"
                                                                                    AutoPostBack="true" CssClass="form-control" />
                                                                                <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender5" runat="server" Enabled="True"
                                                                                    TargetControlID="txtDExchangeRate" ValidChars="1,2,3,4,5,6,7,8,9,0,.">
                                                                                </cc1:FilteredTextBoxExtender>
                                                                                <br />
                                                                            </div>
                                                                            <div class="col-md-6">
                                                                                <asp:Label ID="lblForeignAmount" runat="server" Text="<%$ Resources:Attendance,Foreign Amount %>" />
                                                                                <asp:TextBox ID="txtForeignAmount" runat="server" CssClass="form-control" />
                                                                                <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender4" runat="server" Enabled="True"
                                                                                    TargetControlID="txtForeignAmount" ValidChars="1,2,3,4,5,6,7,8,9,0,.">
                                                                                </cc1:FilteredTextBoxExtender>
                                                                                <br />
                                                                            </div>
                                                                            <div class="col-md-6">
                                                                                <br />
                                                                                <asp:ImageButton ID="btnDetailSave" runat="server" ValidationGroup="Add" ImageUrl="~/Images/add.png"
                                                                                    Height="29px" ToolTip="<%$ Resources:Attendance,Add %>" Width="35px" OnClick="btnDetailSave_Click"></asp:ImageButton>
                                                                                <br />
                                                                            </div>
                                                                            <div class="col-md-12">
                                                                                <div style="overflow: auto; max-height: 500px;">
                                                                                    <asp:GridView CssClass="table-striped table-bordered table table-hover" ID="GvDetail" ShowFooter="true" runat="server" Width="100%"
                                                                                        AutoGenerateColumns="False">

                                                                                        <Columns>
                                                                                            <asp:TemplateField HeaderText="Edit">
                                                                                                <ItemTemplate>
                                                                                                    <asp:ImageButton ID="imgBtnDetailEdit" runat="server" CommandArgument='<%# Eval("Trans_Id") %>'
                                                                                                        ImageUrl="~/Images/edit.png" Width="16px" OnCommand="imgBtnDetailEdit_Command" />
                                                                                                </ItemTemplate>
                                                                                                <ItemStyle HorizontalAlign="Center" />
                                                                                            </asp:TemplateField>
                                                                                            <asp:TemplateField HeaderText="Delete">
                                                                                                <ItemTemplate>
                                                                                                    <asp:ImageButton ID="imgBtnDetailDelete" runat="server" CommandArgument='<%# Eval("Trans_Id") %>'
                                                                                                        Height="16px" ImageUrl="~/Images/Erase.png" Width="16px" OnCommand="imgBtnDetailDelete_Command" />
                                                                                                </ItemTemplate>
                                                                                                <ItemStyle HorizontalAlign="Center" />
                                                                                            </asp:TemplateField>
                                                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,S No. %>">
                                                                                                <ItemTemplate>
                                                                                                    <%#Container.DataItemIndex+1 %>
                                                                                                    <asp:Label ID="lblSNo" Visible="false" runat="server" Text='<%#Eval("Trans_Id") %>' />
                                                                                                </ItemTemplate>
                                                                                                <ItemStyle HorizontalAlign="Center" />
                                                                                            </asp:TemplateField>
                                                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance, Account Name %>">
                                                                                                <ItemTemplate>
                                                                                                    <asp:HiddenField ID="hdngvAccountNo" runat="server" Value='<%#Eval("Account_No") %>' />
                                                                                                    <asp:Label ID="lblgvAccountName" runat="server" Text='<%#GetAccountNameByTransId(Eval("Account_No").ToString())%>' />
                                                                                                </ItemTemplate>
                                                                                                <ItemStyle HorizontalAlign="Center" />
                                                                                            </asp:TemplateField>
                                                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance, Other Account No. %>">
                                                                                                <ItemTemplate>
                                                                                                    <asp:Label ID="lblgvOtherAccountNo" runat="server" Text='<%#Ac_ParameterMaster.GetOtherAccountNameForDetail(Eval("Other_Account_No").ToString(),Eval("Account_No").ToString(),Session["CompId"].ToString(),Session["DBConnection"].ToString())%>' />
                                                                                                    <asp:HiddenField ID="hdnOtherAccountNo" runat="server" Value='<%#Eval("Other_Account_No") %>' />
                                                                                                </ItemTemplate>
                                                                                                <FooterTemplate>
                                                                                                    <asp:Label ID="lblgvTotal" runat="server" Font-Bold="true"
                                                                                                        Text="<%$ Resources:Attendance,Total%>" />
                                                                                                </FooterTemplate>
                                                                                                <ItemStyle HorizontalAlign="Center" />
                                                                                                <FooterStyle HorizontalAlign="Center" />
                                                                                            </asp:TemplateField>
                                                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance, Narration %>">
                                                                                                <ItemTemplate>
                                                                                                    <asp:Label ID="lblgvNarration" runat="server" Text='<%#Eval("Narration") %>' />
                                                                                                </ItemTemplate>
                                                                                                <ItemStyle HorizontalAlign="Center" />
                                                                                            </asp:TemplateField>
                                                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance, Debit Amount %>">
                                                                                                <ItemTemplate>
                                                                                                    <asp:Label ID="lblgvDebitAmount" runat="server" Text='<%#Eval("Debit_Amount") %>' />
                                                                                                </ItemTemplate>
                                                                                                <FooterTemplate>
                                                                                                    <asp:Label ID="lblgvDebitTotal" Font-Bold="true" runat="server" />
                                                                                                </FooterTemplate>
                                                                                                <ItemStyle HorizontalAlign="Center" />
                                                                                                <FooterStyle HorizontalAlign="Center" />
                                                                                            </asp:TemplateField>
                                                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance, Credit Amount %>">
                                                                                                <ItemTemplate>
                                                                                                    <asp:Label ID="lblgvCreditAmount" runat="server" Text='<%#Eval("Credit_Amount") %>' />
                                                                                                </ItemTemplate>
                                                                                                <FooterTemplate>
                                                                                                    <asp:Label ID="lblgvCreditTotal" Font-Bold="true" runat="server" />
                                                                                                </FooterTemplate>
                                                                                                <FooterStyle HorizontalAlign="Center" />
                                                                                                <ItemStyle HorizontalAlign="Center" />
                                                                                            </asp:TemplateField>
                                                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance, Cost Center %>" Visible="false">
                                                                                                <ItemTemplate>
                                                                                                    <asp:Label ID="lblgvCostCenter" runat="server" Text='<%#Eval("CostCenter_ID") %>' />
                                                                                                </ItemTemplate>
                                                                                                <ItemStyle HorizontalAlign="Center" />
                                                                                            </asp:TemplateField>
                                                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance, Employee %>" Visible="false">
                                                                                                <ItemTemplate>
                                                                                                    <asp:HiddenField ID="hdngvEmployeeId" runat="server" Value='<%#Eval("Emp_Id") %>' />
                                                                                                    <asp:Label ID="lblgvEmployee" runat="server" Text='<%#GetEmployeeName(Eval("Emp_Id").ToString())%>' />
                                                                                                </ItemTemplate>
                                                                                                <ItemStyle HorizontalAlign="Center" />
                                                                                            </asp:TemplateField>
                                                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance, Currency %>">
                                                                                                <ItemTemplate>
                                                                                                    <asp:HiddenField ID="hdngvCurrencyId" runat="server" Value='<%#Eval("Currency_Id") %>' />
                                                                                                    <asp:Label ID="lblgvCurrency" runat="server" Text='<%#GetCurrencyName(Eval("Currency_Id").ToString()) %>' />
                                                                                                </ItemTemplate>
                                                                                                <ItemStyle HorizontalAlign="Center" />
                                                                                            </asp:TemplateField>
                                                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance, Foreign Amount %>">
                                                                                                <ItemTemplate>
                                                                                                    <asp:Label ID="lblgvForeignAmount" runat="server" Text='<%#Eval("Foreign_Amount") %>' />
                                                                                                </ItemTemplate>
                                                                                                <ItemStyle HorizontalAlign="Center" />
                                                                                            </asp:TemplateField>
                                                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance, Exchange Rate %>">
                                                                                                <ItemTemplate>
                                                                                                    <asp:Label ID="lblgvExchangeRate" runat="server" Text='<%#Eval("Exchange_Rate") %>' />
                                                                                                </ItemTemplate>
                                                                                                <ItemStyle HorizontalAlign="Center" />
                                                                                            </asp:TemplateField>
                                                                                        </Columns>

                                                                                        <PagerStyle CssClass="pagination-ys" />

                                                                                    </asp:GridView>
                                                                                    <asp:HiddenField ID="hdnAccountNo" runat="server" />
                                                                                    <asp:HiddenField ID="hdnAccountName" runat="server" />
                                                                                </div>
                                                                            </div>
                                                                        </div>
                                                                    </div>
                                                                </div>
                                                            </div>
                                                        </div>



                                                        <br />
                                                    </div>
                                                    <div id="PnlOperationButton" runat="server" class="col-md-12" style="text-align: center">
                                                        <asp:Button ID="btnVoucherSave" runat="server" Text="<%$ Resources:Attendance,Save %>"
                                                            CssClass="btn btn-success" OnClick="btnVoucherSave_Click" UseSubmitBehavior="false" OnClientClick="this.disabled='true'; this.value='Please wait...';" />
                                                        <asp:HiddenField ID="hdnRef_Id" runat="server" Value="0" />
                                                        <asp:HiddenField ID="hdnRef_Type" runat="server" Value="0" />
                                                        <asp:HiddenField ID="hdnInvoiceNumber" runat="server" Value="0" />
                                                        <asp:HiddenField ID="hdnInvoiceDate" runat="server" Value="0" />

                                                        <asp:Button ID="BtnReset" runat="server" Text="<%$ Resources:Attendance,Reset %>"
                                                            CssClass="btn btn-primary" CausesValidation="False" OnClick="BtnReset_Click" />

                                                        <asp:Button ID="btnVoucherCancel" runat="server" CssClass="btn btn-danger" Text="<%$ Resources:Attendance,Cancel %>"
                                                            CausesValidation="False" OnClick="btnVoucherCancel_Click" />

                                                        <asp:HiddenField ID="hdnVoucherId" runat="server" Value="0" />
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
                        <asp:UpdatePanel ID="Update_Bin" runat="server">
                            <ContentTemplate>

                                <div class="row">
                                    <div class="col-md-12">
                                        <div id="Div2" runat="server" class="box box-info collapsed-box">
                                            <div class="box-header with-border">
                                                <h3 class="box-title">
                                                    <asp:Label ID="Label3" runat="server" Text="Advance Search"></asp:Label></h3>
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
                                                    <asp:DropDownList ID="ddlFieldNameBin" runat="server" CssClass="form-control">
                                                        <asp:ListItem Text="<%$ Resources:Attendance, Voucher No %>" Value="Voucher_No"></asp:ListItem>
                                                        <asp:ListItem Text="<%$ Resources:Attendance, Voucher Type %>" Value="Voucher_Type"
                                                            Selected="True"></asp:ListItem>
                                                        <asp:ListItem Text="<%$ Resources:Attendance, Finance Code %>" Value="Finance_Code"></asp:ListItem>

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
                                                <div class="col-lg-2">
                                                    <asp:Panel ID="Panel7" runat="server" DefaultButton="btnbindBin">
                                                        <asp:TextBox ID="txtValueBin" runat="server" CssClass="form-control"></asp:TextBox>
                                                    </asp:Panel>
                                                </div>
                                                <div class="col-lg-3">
                                                    <asp:LinkButton ID="btnbindBin" runat="server" CausesValidation="False"
                                                        OnClick="btnbindBin_Click" ToolTip="<%$ Resources:Attendance,Search %>"><span class="fa fa-search"  style="font-size:25px;"></span></asp:LinkButton>&nbsp;&nbsp;&nbsp;

                                                    <asp:LinkButton ID="btnRefreshBin" runat="server" CausesValidation="False"
                                                        OnClick="btnRefreshBin_Click" ToolTip="<%$ Resources:Attendance,Refresh %>"><span class="fa fa-repeat"  style="font-size:25px;"></span></asp:LinkButton>&nbsp;&nbsp;&nbsp;

                                                    <asp:LinkButton ID="imgBtnRestore" CausesValidation="False" Style="width: 33px;"
                                                        runat="server" OnClick="imgBtnRestore_Click" ToolTip="<%$ Resources:Attendance, Active %>"><span class="far fa-lightbulb"  style="font-size:25px;"></span></asp:LinkButton>&nbsp;&nbsp;&nbsp;

                                                </div>


                                            </div>
                                        </div>
                                    </div>
                                </div>
                                <div class="box box-warning box-solid">
                                    <div class="box-body">
                                        <div class="row">
                                            <div class="col-md-12">
                                                <div class="flow">
                                                    <asp:Label ID="Label24" runat="server" Visible="false"></asp:Label>
                                                    <asp:GridView CssClass="table-striped table-bordered table table-hover" ID="GvVoucherBin" PageSize='<%# int.Parse(Session["GridSize"].ToString()) %>' runat="server"
                                                        AutoGenerateColumns="False" Width="100%" AllowPaging="True" OnPageIndexChanging="GvVoucherBin_PageIndexChanging"
                                                        OnSorting="GvVoucherBin_OnSorting" AllowSorting="true">
                                                        <Columns>
                                                            <asp:TemplateField>
                                                                <HeaderTemplate>
                                                                    <asp:CheckBox ID="chkCurrent" runat="server" OnCheckedChanged="chkBCurrent_CheckedChanged"
                                                                        AutoPostBack="true" />
                                                                </HeaderTemplate>
                                                                <ItemTemplate>
                                                                    <asp:CheckBox ID="chkSelect" runat="server" AutoPostBack="true" OnCheckedChanged="chkBSelect_CheckedChanged" />
                                                                </ItemTemplate>
                                                                <ItemStyle HorizontalAlign="Center" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,Voucher No %>" SortExpression="Voucher_No">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblgvVoucherNo" runat="server" Text='<%#Eval("Voucher_No") %>' />
                                                                    <asp:Label ID="lblgvTransId" runat="server" Visible="false" Text='<%#Eval("Trans_Id") %>' />
                                                                </ItemTemplate>
                                                                <ItemStyle HorizontalAlign="Center" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance, Voucher Date %>" SortExpression="Voucher_Date">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblgvVoucherDate" runat="server" Text='<%#GetDate(Eval("Voucher_Date").ToString()) %>' />
                                                                </ItemTemplate>
                                                                <ItemStyle HorizontalAlign="Center" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance, Voucher Type %>" SortExpression="Voucher_Type">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblgvVoucherType" runat="server" Text='<%#Eval("Voucher_Type") %>' />
                                                                </ItemTemplate>
                                                                <ItemStyle HorizontalAlign="Center" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance, Narration %>" SortExpression="Narration">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblgvFinanceCode" runat="server" Text='<%#Eval("Narration") %>' />
                                                                </ItemTemplate>
                                                                <ItemStyle HorizontalAlign="Center" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,Voucher Amount%>">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblgvExchangerate" runat="server" Text='<%#GetVoucherAmount(Eval("Trans_Id").ToString()) %>' />
                                                                </ItemTemplate>
                                                                <ItemStyle HorizontalAlign="Center" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance, Currency %>" SortExpression="Currency_Id">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblgvCurrency" runat="server" Text='<%# GetCurrencyName(Eval("Currency_Id").ToString()) %>' />
                                                                </ItemTemplate>
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
    <div class="modal fade" id="Voucher_Detail" tabindex="-1" role="dialog" aria-labelledby="Voucher_DetailLabel" aria-hidden="true">
        <div class="modal-dialog modal-lg">
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal">
                        <span aria-hidden="true">&times;</span><span class="sr-only">Close</span></button>
                    <h4 class="modal-title" id="Voucher_DetailLabel">
                        <asp:Label ID="Label25" runat="server" Text="Voucher Detail" Font-Bold="true"></asp:Label></h4>
                </div>
                <div class="modal-body">
                    <asp:UpdatePanel ID="Update_Voucher_Detail" runat="server">
                        <ContentTemplate>
                            <div class="row">
                                <div class="col-md-12">
                                    <div class="box box-primary">
                                        <div class="box-body">
                                            <div class="form-group">
                                                <div class="col-md-6">
                                                    <asp:Label ID="lblVFinanceCode" runat="server" Text="<%$ Resources:Attendance,Finance Code %>" />
                                                    <asp:TextBox ID="txtVFinanceCode" ReadOnly="true" runat="server" CssClass="form-control" />
                                                    <br />
                                                </div>
                                                <div class="col-md-12">
                                                </div>
                                                <div class="col-md-6">
                                                    <asp:Label ID="lblVLocation" runat="server" Text="<%$ Resources:Attendance,Location To %>" />
                                                    <asp:TextBox ID="txtVToLocation" ReadOnly="true" runat="server" CssClass="form-control" />
                                                    <br />
                                                </div>
                                                <div class="col-md-6">
                                                    <asp:Label ID="lblVDepartment" runat="server" Text="<%$ Resources:Attendance,Department %>" />
                                                    <asp:TextBox ID="txtVDepartment" ReadOnly="true" runat="server" CssClass="form-control" />
                                                    <br />
                                                </div>
                                                <div class="col-md-6">
                                                    <asp:Label ID="lblVVoucherType" runat="server" Text="<%$ Resources:Attendance,Voucher Type %>" />
                                                    <asp:DropDownList ID="ddlVVoucherType" runat="server" CssClass="form-control">
                                                        <asp:ListItem Text="--Select--" Value="--Select--"></asp:ListItem>
                                                        <asp:ListItem Text="Purchase Invoice" Value="PI"></asp:ListItem>
                                                        <asp:ListItem Text="Purchase Return" Value="PR"></asp:ListItem>
                                                        <asp:ListItem Text="Journal Vouchers" Value="JV"></asp:ListItem>
                                                        <asp:ListItem Text="Payment Vouchers" Value="PV"></asp:ListItem>
                                                        <asp:ListItem Text="Sales Invoice" Value="SI"></asp:ListItem>
                                                        <asp:ListItem Text="Receive Vouchers" Value="RV"></asp:ListItem>
                                                        <asp:ListItem Text="Sales Return" Value="SR"></asp:ListItem>
                                                        <asp:ListItem Text="Supplier Payment Vouchers" Value="SPV"></asp:ListItem>
                                                        <asp:ListItem Text="Customer Receive Vouchers" Value="CRV"></asp:ListItem>
                                                        <asp:ListItem Text="Customer Debit Note" Value="CDN"></asp:ListItem>
                                                        <asp:ListItem Text="Supplier Debit Note" Value="SDN"></asp:ListItem>
                                                        <asp:ListItem Text="Customer Credit Note" Value="CCN"></asp:ListItem>
                                                        <asp:ListItem Text="Supplier Credit Note" Value="SCN"></asp:ListItem>
                                                        <asp:ListItem Text="PDC Customer" Value="PDC"></asp:ListItem>
                                                        <asp:ListItem Text="PDC Supplier" Value="PDS"></asp:ListItem>
                                                    </asp:DropDownList>
                                                    <%--<asp:TextBox ID="txtVoucherType" runat="server" CssClass="form-control" />--%>
                                                    <br />
                                                </div>
                                                <div class="col-md-12"></div>
                                                <div class="col-md-6">
                                                    <asp:Label ID="lblVVoucherNo" runat="server" Text="<%$ Resources:Attendance,Voucher No. %>" />
                                                    <a style="color: Red">*</a>
                                                    <asp:RequiredFieldValidator EnableClientScript="true" Style="float: right;" runat="server" ID="RequiredFieldValidator10" ValidationGroup="Save"
                                                        Display="Dynamic" SetFocusOnError="true" ControlToValidate="txtVVoucherNo" ErrorMessage="<%$ Resources:Attendance,Enter Voucher No%>"></asp:RequiredFieldValidator>

                                                    <asp:TextBox ID="txtVVoucherNo" ReadOnly="true" runat="server" CssClass="form-control" />
                                                    <br />
                                                </div>
                                                <div class="col-md-6">
                                                    <asp:Label ID="lblVVoucherDate" runat="server" Text="<%$ Resources:Attendance,Voucher Date %>" />
                                                    <asp:TextBox ID="txtVVoucherDate" ReadOnly="true" runat="server" CssClass="form-control" />
                                                    <cc1:CalendarExtender OnClientShown="showCalendar" ID="CalendarExtender13" runat="server" TargetControlID="txtVVoucherDate"
                                                        Format="dd/MMM/yyyy" />
                                                    <br />
                                                </div>
                                                <div class="col-md-12">
                                                    <asp:RadioButton ID="rbVCashPayment" runat="server" Text="Cash Payment"
                                                        GroupName="Pay" Enabled="false" OnCheckedChanged="rbVCashPayment_CheckedChanged"
                                                        AutoPostBack="true" />
                                                    <asp:RadioButton ID="rbVChequePayment" Style="margin-left: 20px;" runat="server" Text="Cheque Payment"
                                                        GroupName="Pay" Enabled="false" OnCheckedChanged="rbVCashPayment_CheckedChanged"
                                                        AutoPostBack="true" />
                                                    <asp:CheckBox ID="chkVReconcile" Visible="false" Style="margin-left: 20px;" runat="server" Text="<%$ Resources:Attendance, Reconcile%>" />
                                                    <br />
                                                </div>
                                                <div id="trVCheque1" runat="server" visible="false">
                                                    <div class="col-md-6">
                                                        <asp:Label ID="lblVChequeIssueDate" runat="server" Text="Cheque Issue Date" />
                                                        <asp:TextBox ID="txtVChequeIssueDate" ReadOnly="true" runat="server" CssClass="form-control" />
                                                        <cc1:CalendarExtender OnClientShown="showCalendar" ID="CalendarExtender14" runat="server" TargetControlID="txtVChequeIssueDate"
                                                            Format="dd/MMM/yyyy" />
                                                        <br />
                                                    </div>
                                                    <div class="col-md-6">
                                                        <asp:Label ID="lblVChequeClearDate" runat="server" Text="Cheque Clear Date" />
                                                        <asp:TextBox ID="txtVChequeClearDate" ReadOnly="true" runat="server" CssClass="form-control" />
                                                        <cc1:CalendarExtender OnClientShown="showCalendar" ID="CalendarExtender15" runat="server" TargetControlID="txtVChequeClearDate"
                                                            Format="dd/MMM/yyyy" />
                                                        <br />
                                                    </div>
                                                </div>
                                                <div id="trVCheque2" runat="server" visible="false" class="col-md-6">
                                                    <asp:Label ID="lblVChequeNo" runat="server" Text="Cheque No." />
                                                    <asp:TextBox ID="txtVChequeNo" ReadOnly="true" runat="server" CssClass="form-control" />
                                                    <br />
                                                </div>
                                                <div class="col-md-12">
                                                    <asp:Label ID="lblVReference" runat="server" Text="<%$ Resources:Attendance,Refrence%>" />
                                                    <asp:TextBox ID="txtVReference" ReadOnly="true" runat="server" CssClass="form-control" />
                                                    <br />
                                                </div>
                                                <div class="col-md-12">
                                                    <div class="box box-warning box-solid">
                                                        <div class="box-header with-border">
                                                            <h3 class="box-title">
                                                                <asp:Label ID="Label26" runat="server" Text="<%$ Resources:Attendance,Detail View%>" />
                                                        </div>
                                                        <div class="box-body">
                                                            <div class="row">
                                                                <div class="col-md-12">
                                                                    <div style="overflow: auto; max-height: 500px;">
                                                                        <asp:HiddenField ID="hdnVAccountNo" runat="server" />
                                                                        <asp:HiddenField ID="hdnVAccountName" runat="server" />
                                                                        <asp:GridView CssClass="table-striped table-bordered table table-hover" ID="GvVDetail" ShowFooter="true" runat="server" Width="100%"
                                                                            AutoGenerateColumns="False">

                                                                            <Columns>
                                                                                <asp:TemplateField HeaderText="<%$ Resources:Attendance,S No. %>">
                                                                                    <ItemTemplate>
                                                                                        <%#Container.DataItemIndex+1 %>
                                                                                        <asp:Label ID="lblSNo" Visible="false" runat="server" Text='<%#Eval("Trans_Id") %>' />
                                                                                    </ItemTemplate>
                                                                                    <ItemStyle HorizontalAlign="Center" />
                                                                                </asp:TemplateField>
                                                                                <asp:TemplateField HeaderText="<%$ Resources:Attendance, Account Name %>">
                                                                                    <ItemTemplate>
                                                                                        <asp:HiddenField ID="hdngvAccountNo" runat="server" Value='<%#Eval("Account_No") %>' />
                                                                                        <asp:Label ID="lblgvAccountName" runat="server" Text='<%#GetAccountNameByTransId(Eval("Account_No").ToString())%>' />
                                                                                    </ItemTemplate>
                                                                                    <ItemStyle HorizontalAlign="Center" />
                                                                                </asp:TemplateField>
                                                                                <asp:TemplateField HeaderText="<%$ Resources:Attendance, Other Account No. %>">
                                                                                    <ItemTemplate>
                                                                                        <asp:Label ID="lblgvOtherAccountNo" runat="server" Text='<%#Ac_ParameterMaster.GetOtherAccountNameForDetail(Eval("Other_Account_No").ToString(),Eval("Account_No").ToString(),Session["CompId"].ToString(),Session["DBConnection"].ToString())%>' />
                                                                                        <asp:HiddenField ID="hdnOtherAccountNo" runat="server" Value='<%#Eval("Other_Account_No") %>' />
                                                                                    </ItemTemplate>
                                                                                    <FooterTemplate>
                                                                                        <asp:Label ID="lblgvTotal" runat="server" Font-Bold="true"
                                                                                            Text="<%$ Resources:Attendance,Total%>" />
                                                                                    </FooterTemplate>
                                                                                    <ItemStyle HorizontalAlign="Center" />
                                                                                    <FooterStyle HorizontalAlign="Center" />
                                                                                </asp:TemplateField>
                                                                                <asp:TemplateField HeaderText="<%$ Resources:Attendance, Narration %>">
                                                                                    <ItemTemplate>
                                                                                        <asp:Label ID="lblgvNarration" runat="server" Text='<%#Eval("Narration") %>' />
                                                                                    </ItemTemplate>
                                                                                    <ItemStyle HorizontalAlign="Center" />
                                                                                </asp:TemplateField>
                                                                                <asp:TemplateField HeaderText="<%$ Resources:Attendance, Debit Amount %>">
                                                                                    <ItemTemplate>
                                                                                        <asp:Label ID="lblgvDebitAmount" runat="server" Text='<%#Eval("Debit_Amount") %>' />
                                                                                    </ItemTemplate>
                                                                                    <FooterTemplate>
                                                                                        <asp:Label ID="lblgvDebitTotal" Font-Bold="true" runat="server" />
                                                                                    </FooterTemplate>
                                                                                    <ItemStyle HorizontalAlign="Center" />
                                                                                    <FooterStyle HorizontalAlign="Center" />
                                                                                </asp:TemplateField>
                                                                                <asp:TemplateField HeaderText="<%$ Resources:Attendance, Credit Amount %>">
                                                                                    <ItemTemplate>
                                                                                        <asp:Label ID="lblgvCreditAmount" runat="server" Text='<%#Eval("Credit_Amount") %>' />
                                                                                    </ItemTemplate>
                                                                                    <FooterTemplate>
                                                                                        <asp:Label ID="lblgvCreditTotal" Font-Bold="true" runat="server" />
                                                                                    </FooterTemplate>
                                                                                    <FooterStyle HorizontalAlign="Center" />
                                                                                    <ItemStyle HorizontalAlign="Center" />
                                                                                </asp:TemplateField>
                                                                                <asp:TemplateField HeaderText="<%$ Resources:Attendance, Cost Center %>" Visible="false">
                                                                                    <ItemTemplate>
                                                                                        <asp:Label ID="lblgvCostCenter" runat="server" Text='<%#Eval("CostCenter_ID") %>' />
                                                                                    </ItemTemplate>
                                                                                    <ItemStyle HorizontalAlign="Center" />
                                                                                </asp:TemplateField>
                                                                                <asp:TemplateField HeaderText="<%$ Resources:Attendance, Employee %>" Visible="false">
                                                                                    <ItemTemplate>
                                                                                        <asp:HiddenField ID="hdngvEmployeeId" runat="server" Value='<%#Eval("Emp_Id") %>' />
                                                                                        <asp:Label ID="lblgvEmployee" runat="server" Text='<%#GetEmployeeName(Eval("Emp_Id").ToString())%>' />
                                                                                    </ItemTemplate>
                                                                                    <ItemStyle HorizontalAlign="Center" />
                                                                                </asp:TemplateField>
                                                                                <asp:TemplateField HeaderText="<%$ Resources:Attendance, Currency %>">
                                                                                    <ItemTemplate>
                                                                                        <asp:HiddenField ID="hdngvCurrencyId" runat="server" Value='<%#Eval("Currency_Id") %>' />
                                                                                        <asp:Label ID="lblgvCurrency" runat="server" Text='<%#GetCurrencyName(Eval("Currency_Id").ToString()) %>' />
                                                                                    </ItemTemplate>
                                                                                    <ItemStyle HorizontalAlign="Center" />
                                                                                </asp:TemplateField>
                                                                                <asp:TemplateField HeaderText="<%$ Resources:Attendance, Foreign Amount %>">
                                                                                    <ItemTemplate>
                                                                                        <asp:Label ID="lblgvForeignAmount" runat="server" Text='<%#Eval("Foreign_Amount") %>' />
                                                                                    </ItemTemplate>
                                                                                    <ItemStyle HorizontalAlign="Center" />
                                                                                </asp:TemplateField>
                                                                                <asp:TemplateField HeaderText="<%$ Resources:Attendance, Exchange Rate %>">
                                                                                    <ItemTemplate>
                                                                                        <asp:Label ID="lblgvExchangeRate" runat="server" Text='<%#Eval("Exchange_Rate") %>' />
                                                                                    </ItemTemplate>
                                                                                    <ItemStyle HorizontalAlign="Center" />
                                                                                </asp:TemplateField>
                                                                            </Columns>

                                                                            <PagerStyle CssClass="pagination-ys" />

                                                                        </asp:GridView>
                                                                    </div>
                                                                </div>
                                                            </div>
                                                        </div>
                                                    </div>
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
                <div class="modal-footer">
                    <asp:UpdatePanel ID="Update_Voucher_Detail_Button" runat="server">
                        <ContentTemplate>
                            <button type="button" class="btn btn-danger" data-dismiss="modal">
                                Close</button>
                            <asp:Button ID="Button2" runat="server" CausesValidation="False" CssClass="btn btn-primary" Visible="false"
                                OnClick="btnCancelPopLeave_Click" TabIndex="9" Text="<%$ Resources:Attendance,Cancel %>" />
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
            </div>
        </div>
    </div>

    <asp:UpdateProgress ID="UpdateProgress9" runat="server" AssociatedUpdatePanelID="Update_Voucher_Detail">
        <ProgressTemplate>
            <div class="modal_Progress">
                <div class="center_Progress">
                    <img alt="" src="../Images/ajax-loader2.gif" style="vertical-align: middle; background-color: White; border-radius: 10px;" />
                </div>
            </div>
        </ProgressTemplate>
    </asp:UpdateProgress>

    <asp:UpdateProgress ID="UpdateProgress10" runat="server" AssociatedUpdatePanelID="Update_Voucher_Detail_Button">
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

    <asp:Panel ID="pnlMenuList" runat="server" Visible="false"></asp:Panel>
    <asp:Panel ID="pnlMenuNew" runat="server" Visible="false"></asp:Panel>
    <asp:Panel ID="pnlMenuBin" runat="server" Visible="false"></asp:Panel>
    <asp:Panel ID="PnlList" runat="server" Visible="false"></asp:Panel>
    <asp:Panel ID="PnlNewEdit" runat="server" Visible="false"></asp:Panel>
    <asp:Panel ID="PnlBin" runat="server" Visible="false"></asp:Panel>
    <asp:Panel ID="PnlNewContant" runat="server" Visible="false"></asp:Panel>

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
        function LI_Edit_Active() {
            $("#Li_List").removeClass("active");
            $("#List").removeClass("active");

            $("#Li_New").addClass("active");
            $("#New").addClass("active");
        }
        function LI_List_Active() {
            $("#Li_List").addClass("active");
            $("#List").addClass("active");

            $("#Li_New").removeClass("active");
            $("#New").removeClass("active");
        }

        function Li_Tab_List() {
            document.getElementById('<%= Btn_List.ClientID %>').click();
        }
        function Li_Tab_New() {
            document.getElementById('<%= Btn_New.ClientID %>').click();
        }
        function Li_Tab_Bin() {
            document.getElementById('<%= Btn_Bin.ClientID %>').click();
        }

        function Voucher_Detail_Popup() {
            document.getElementById('<%= Btn_Voucher_Detail.ClientID %>').click();
        }
        function Validate_Grid(sender, args) {
            var gridView = document.getElementById("<%=GvVoucher.ClientID %>");
            var checkBoxes = gridView.getElementsByTagName("input");
            for (var i = 0; i < checkBoxes.length; i++) {
                if (checkBoxes[i].type == "checkbox" && checkBoxes[i].checked) {
                    args.IsValid = true;
                    return;
                }
            }
            args.IsValid = false;
        }
        function showUcControlsSettings() {
            $('#ControlSettingModal').modal('show');
        }
    </script>
</asp:Content>
