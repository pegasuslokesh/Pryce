<%@ Page Title="" Language="C#" MasterPageFile="~/ERPMaster.master" AutoEventWireup="true" CodeFile="PurchaseInquiry.aspx.cs" Inherits="Purchase_PurchaseInquiry" %>

<%@ Register Src="~/WebUserControl/ReportSystem.ascx" TagName="ReportSystem" TagPrefix="RS" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit.HTMLEditor" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="Head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="HeaderContent" runat="Server">
    <h1>
        <i class="fas fa-info-circle"></i>&nbsp;&nbsp;
        <asp:Label ID="lblHeader" runat="server" Text="<%$ Resources:Attendance,Purchase Inquiry%>"></asp:Label>
    </h1>
    <ol class="breadcrumb">
        <li><a href="#"><i class="fa fa-dashboard"></i>
            <asp:Label ID="Label6" runat="server" Text="<%$ Resources:Attendance,Inventory%>"></asp:Label></a></li>
        <li><a href="#"><i class="fa fa-dashboard"></i>
            <asp:Label ID="Label10" runat="server" Text="<%$ Resources:Attendance,Purchase%>"></asp:Label></a></li>
        <li class="active">
            <asp:Label ID="Label5" runat="server" Text="<%$ Resources:Attendance,Purchase Inquiry%>"></asp:Label></li>
    </ol>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="Server">
    <asp:UpdatePanel ID="Update_Button" runat="server">
        <ContentTemplate>
            <asp:Button ID="Btn_Bin" Style="display: none;" runat="server" OnClick="btnBin_Click" Text="Bin" />
            <asp:Button ID="Btn_Request" Style="display: none;" runat="server" OnClick="btnPRequest_Click" Text="Request" />
            <asp:Button ID="btnFillUnits" Style="display: none;" runat="server" OnClick="btnFillUnits_Click" Text="Request" />
            <asp:HiddenField runat="server" ID="hdnCanView" />
            <asp:HiddenField runat="server" ID="hdnCanEdit" />
            <asp:HiddenField runat="server" ID="hdnCanDelete" />
            <asp:HiddenField runat="server" ID="hdnCanPrint" />
            <asp:HiddenField runat="server" ID="hdnLocationId" />
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
                    <li id="Li_Request"><a href="#Request" onclick="Li_Tab_Request()" data-toggle="tab">
                        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                            <ContentTemplate>
                                <i class="fas fa-user-check"></i>&nbsp;&nbsp;<asp:Label ID="Lbl_Request" runat="server" Text="<%$ Resources:Attendance,Request %>"></asp:Label>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </a></li>
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
                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="btnSaveSupplier" EventName="Click" />
                                <asp:AsyncPostBackTrigger ControlID="imgBtnRestore" />
                            </Triggers>
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
                                                <div class="col-md-12">
                                                    <asp:DropDownList ID="ddlLocation" runat="server" Class="form-control" OnSelectedIndexChanged="ddlLocation_SelectedIndexChanged" AutoPostBack="true">
                                                    </asp:DropDownList>
                                                    <br />
                                                </div>
                                                <div class="col-lg-3">
                                                    <asp:DropDownList ID="ddlFieldName" runat="server" CssClass="form-control" AutoPostBack="true" OnSelectedIndexChanged="ddlFieldName_SelectedIndexChanged">
                                                        <asp:ListItem Text="<%$ Resources:Attendance, Inquiry No. %>" Value="PI_No" Selected="True"></asp:ListItem>
                                                        <asp:ListItem Text="<%$ Resources:Attendance, Inquiry Date %>" Value="PIDate"></asp:ListItem>
                                                        <asp:ListItem Text="<%$ Resources:Attendance, Ref. Type %>" Value="RefType"></asp:ListItem>
                                                        <asp:ListItem Text="<%$ Resources:Attendance, Ref No. %>" Value="TransNo"></asp:ListItem>
                                                        <asp:ListItem Text="<%$ Resources:Attendance, Customer %>" Value="Customer_Name"></asp:ListItem>
                                                        <asp:ListItem Text="<%$ Resources:Attendance, Status %>" Value="Status"></asp:ListItem>
                                                        <asp:ListItem Text="<%$ Resources:Attendance, Created By  %>" Value="UserName"></asp:ListItem>
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
                                                <div class="col-lg-5">
                                                    <asp:Panel ID="Panel1" runat="server" DefaultButton="btnbind">
                                                        <asp:TextBox ID="txtValue" runat="server" CssClass="form-control" placeholder="Search From Content"></asp:TextBox>
                                                        <asp:TextBox ID="txtValueDate" runat="server" CssClass="form-control" Visible="false" placeholder="Search From Date"></asp:TextBox>
                                                        <cc1:CalendarExtender OnClientShown="showCalendar" ID="CalendartxtValueDate" runat="server" TargetControlID="txtValueDate" />
                                                    </asp:Panel>
                                                </div>
                                                <div class="col-lg-2">
                                                    <asp:LinkButton ID="btnbind" runat="server" CausesValidation="False" OnClick="btnbindrpt_Click" ToolTip="<%$ Resources:Attendance,Search %>"><span class="fa fa-search"  style="font-size:25px;"></span></asp:LinkButton>&nbsp;&nbsp;&nbsp;
                                                    <asp:LinkButton ID="btnRefresh" runat="server" CausesValidation="False" OnClick="btnRefreshReport_Click" ToolTip="<%$ Resources:Attendance,Refresh %>"><span class="fa fa-repeat"  style="font-size:25px;"></span></asp:LinkButton>
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
                                                    <asp:GridView CssClass="table-striped table-bordered table table-hover" ID="GvPurchaseInquiry" PageSize="<%# PageControlCommon.GetPageSize() %>" CurrentSortField="PIDate" CurrentSortDirection="DESC"
                                                        runat="server" AutoGenerateColumns="False" Width="100%" AllowPaging="True" OnPageIndexChanging="GvPurchaseInquiry_PageIndexChanging"
                                                        AllowSorting="True" OnSorting="GvPurchaseInquiry_Sorting">
                                                        <Columns>
                                                            <asp:TemplateField HeaderText="Action">
                                                                <ItemTemplate>
                                                                    <div class="dropdown" style="position: absolute;">
                                                                        <button class="btn btn-default dropdown-toggle" type="button" data-toggle="dropdown">
                                                                            <i class="fa fa-ellipsis-h" aria-hidden="true"></i>
                                                                        </button>
                                                                        <ul class="dropdown-menu">
                                                                            <li <%= hdnCanPrint.Value=="true"?"style='display:block'":"style='display:none'" %>>
                                                                                <a onclick="IbtnPrint_Command('<%# Eval("Trans_Id") %>')" style="cursor: pointer"><i class="fa fa-print"></i>Print</a>
                                                                            </li>

                                                                            <li <%= hdnCanPrint.Value=="true"?"style='display:block'":"style='display:none'" %>>
                                                                                <a onclick="getReportDataWithLoc('<%# Eval("Trans_Id") %>','<%# Eval("Location_Id") %>')"><i class="fa fa-print" style="cursor: pointer"></i>Report System</a>
                                                                            </li>

                                                                            <li <%= hdnCanView.Value=="true"?"style='display:block'":"style='display:none'" %>>
                                                                                <asp:LinkButton ID="lnkViewDetail" runat="server" CommandArgument='<%# Eval("Trans_Id") %>' CommandName='<%# Eval("Location_Id") %>' OnCommand="lnkViewDetail_Command" CausesValidation="False"><i class="fa fa-eye"></i>View</asp:LinkButton>
                                                                            </li>

                                                                            <li <%= hdnCanEdit.Value=="true"?"style='display:block'":"style='display:none'" %>>
                                                                                <asp:LinkButton ID="btnEdit" runat="server" CommandArgument='<%# Eval("Trans_Id") %>' OnCommand="btnEdit_Command" CommandName='<%# Eval("Location_Id") %>' CausesValidation="False"><i class="fa fa-pencil"></i>Edit </asp:LinkButton>
                                                                            </li>
                                                                            <li <%= hdnCanDelete.Value=="true"?"style='display:block'":"style='display:none'" %>>
                                                                                <asp:LinkButton ID="IbtnDelete" runat="server" CausesValidation="False" CommandArgument='<%# Eval("Trans_Id") %>' OnCommand="IbtnDelete_Command"><i class="fa fa-trash"></i>Delete</asp:LinkButton>
                                                                                <cc1:ConfirmButtonExtender ID="confirm2" runat="server" ConfirmText="<%$ Resources:Attendance,Are you sure you want to delete the record?%>" TargetControlID="IbtnDelete"></cc1:ConfirmButtonExtender>
                                                                            </li>
                                                                        </ul>
                                                                    </div>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>

                                                            <asp:BoundField DataField="PI_No" HeaderText="<%$ Resources:Attendance,Inquiry No. %>"
                                                                SortExpression="PI_No" />
                                                            <asp:TemplateField SortExpression="PIDate" HeaderText="<%$ Resources:Attendance,Inquiry Date %>">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblgvDate" runat="server" Text='<%# Convert.ToDateTime(Eval("PIDate").ToString()).ToString("dd-MMM-yyyy") %>' />
                                                                </ItemTemplate>
                                                                <ItemStyle HorizontalAlign="Center" Width="90px" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField SortExpression="TransFrom" HeaderText="<%$ Resources:Attendance,Ref. Type %>">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblRefType" runat="server" Text='<%#Eval("RefType") %>' />
                                                                </ItemTemplate>
                                                                <ItemStyle HorizontalAlign="Left" Width="100px" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField SortExpression="TransNo" HeaderText="<%$ Resources:Attendance,Ref No. %>">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblRefNo" runat="server" Text='<%#Eval("TransNo")%>' />
                                                                </ItemTemplate>
                                                                <ItemStyle HorizontalAlign="Left" />
                                                            </asp:TemplateField>

                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,Department %>">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblgvDept" runat="server" Text='<%# Eval("DepartmentName") %>' />
                                                                </ItemTemplate>
                                                                <ItemStyle HorizontalAlign="Left" Width="70px" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,Customer%>">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblgvcustIdInq" runat="server" Text='<%# Eval("Customer_Name").ToString()%>' />
                                                                </ItemTemplate>
                                                                <ItemStyle HorizontalAlign="Left" />
                                                            </asp:TemplateField>

                                                            <asp:BoundField DataField="Status" HeaderText="<%$ Resources:Attendance,Status %>"
                                                                SortExpression="Status" ItemStyle-Width="120px" />
                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,Created By %>" SortExpression="UserName">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblCreated_User" runat="server" Text='<%# Eval("UserName") %>'></asp:Label>
                                                                </ItemTemplate>
                                                                <ItemStyle />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,Modified By %>" SortExpression="ModifiedUserName">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblModified_User" runat="server" Text='<%# Eval("ModifiedUserName") %>'></asp:Label>
                                                                </ItemTemplate>
                                                                <ItemStyle />
                                                            </asp:TemplateField>
                                                        </Columns>
                                                    </asp:GridView>
                                                    <div class="col-md-12">
                                                        <asp:HiddenField ID="hdnGvPurchaseInquiryCurrentPageIndex" runat="server" Value="1" />
                                                        <asp:Repeater ID="rptPager" runat="server">
                                                            <ItemTemplate>
                                                                <ul class="pagination">
                                                                    <li class='<%# Convert.ToBoolean(Eval("Enabled")) ? "page-item" : "page-item active" %>'>
                                                                        <asp:LinkButton ID="LinklnkPageButton1" runat="server" Text='<%#Eval("Text") %>' CommandArgument='<%# Eval("Value") %>'
                                                                            CssClass="page-link"
                                                                            OnClick="lnkPage_Click" OnClientClick='<%# !Convert.ToBoolean(Eval("Enabled")) ? "return false;" : "" %>'></asp:LinkButton>
                                                                    </li>
                                                                </ul>
                                                            </ItemTemplate>
                                                        </asp:Repeater>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </div>
                    <div class="tab-pane" id="New">
                        <asp:UpdatePanel ID="Update_New" runat="server" UpdateMode="Conditional">
                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="GvPurchaseInquiry" />
                                <asp:AsyncPostBackTrigger ControlID="btnFillUnits" EventName="Click" />
                                <asp:AsyncPostBackTrigger ControlID="GvPurchaseRequest" />

                            </Triggers>
                            <ContentTemplate>
                                <div id="Main_Div" runat="server">
                                    <div class="row">
                                        <div class="col-md-12">
                                            <div class="box box-primary">
                                                <div class="box-body">
                                                    <div class="form-group">
                                                        <div class="col-md-6">
                                                            <asp:Label ID="lblPIDate" runat="server" Text="<%$ Resources:Attendance,Purchase Inquiry Date %>"></asp:Label>
                                                            <a style="color: Red">*</a>
                                                            <asp:RequiredFieldValidator EnableClientScript="true" Style="float: right;" runat="server" ID="RequiredFieldValidator3" ValidationGroup="Save"
                                                                Display="Dynamic" SetFocusOnError="true" ControlToValidate="txtPIDate" ErrorMessage="<%$ Resources:Attendance,Enter Purchase Inquiry Date%>"></asp:RequiredFieldValidator>

                                                            <asp:TextBox ID="txtPIDate" runat="server" CssClass="form-control" />
                                                            <cc1:CalendarExtender OnClientShown="showCalendar" ID="Calender" runat="server" TargetControlID="txtPIDate" />
                                                            <br />
                                                        </div>
                                                        <div class="col-md-6">
                                                            <asp:Label ID="lblPINo" runat="server" Text="<%$ Resources:Attendance,Purchase Inquiry No. %>"></asp:Label>
                                                            <a style="color: Red">*</a>
                                                            <asp:RequiredFieldValidator EnableClientScript="true" Style="float: right;" runat="server" ID="RequiredFieldValidator1" ValidationGroup="Save"
                                                                Display="Dynamic" SetFocusOnError="true" ControlToValidate="txtPINo" ErrorMessage="<%$ Resources:Attendance,Enter Purchase Inquiry No%>"></asp:RequiredFieldValidator>

                                                            <asp:TextBox ID="txtPINo" runat="server" CssClass="form-control" />
                                                            <br />
                                                        </div>
                                                        <div id="pnlTrans" runat="server" visible="false">
                                                            <div class="col-md-6">
                                                                <asp:Label ID="lblTransFrom" runat="server" Text="<%$ Resources:Attendance,Reference Type %>" />
                                                                <asp:TextBox ID="txtTransFrom" runat="server" CssClass="form-control" ReadOnly="True" />
                                                                <br />
                                                            </div>
                                                            <div class="col-md-6">
                                                                <asp:Label ID="lblTransNo" runat="server" Text="<%$ Resources:Attendance,Reference No %>" />
                                                                <asp:TextBox ID="txtTransNo" runat="server" CssClass="form-control" ReadOnly="true" />
                                                                <br />
                                                            </div>
                                                            <div class="col-md-6">
                                                                <asp:Label ID="lblDepartment" runat="server" Visible="false"
                                                                    Text="<%$ Resources:Attendance,Department %>"></asp:Label>
                                                                <asp:TextBox ID="txtDepartment" runat="server" ReadOnly="true" CssClass="form-control"
                                                                    Visible="false" />
                                                                <br />
                                                            </div>
                                                            <div class="col-md-6">
                                                                <asp:Label ID="lblEmployeeName" runat="server" Visible="false"
                                                                    Text="<%$ Resources:Attendance,User %>"></asp:Label>
                                                                <asp:TextBox ID="txtEmployeeName" runat="server" ReadOnly="true" CssClass="form-control"
                                                                    Visible="false" />
                                                                <br />
                                                            </div>
                                                            <div class="col-md-6">
                                                                <asp:Label ID="lblTender" runat="server" Visible="false" Text="<%$ Resources:Attendance,Tender %>"></asp:Label>
                                                                <asp:TextBox ID="txtTender" runat="server" ReadOnly="true" CssClass="form-control"
                                                                    Visible="false" />
                                                                <br />
                                                            </div>
                                                            <div class="col-md-6">
                                                                <asp:Label ID="lblOCDate" runat="server" Visible="false" Text="<%$ Resources:Attendance,Closing Date %>"></asp:Label>
                                                                <asp:TextBox ID="txtOCDt" runat="server" ReadOnly="true" CssClass="form-control" Visible="false" />
                                                                <br />
                                                            </div>
                                                        </div>
                                                        <div class="col-md-6">
                                                            <asp:RadioButton ID="rbtnFormView" Font-Bold="true" runat="server" Text="<%$ Resources:Attendance,Form View%>" AutoPostBack="true" GroupName="Product" OnCheckedChanged="rbtnFormView_OnCheckedChanged" />&nbsp;&nbsp;&nbsp;
                                                            <asp:RadioButton ID="rbtnAdvancesearchView" Font-Bold="true" runat="server" Text="<%$ Resources:Attendance,Advance Search View%>" AutoPostBack="true" GroupName="Product" OnCheckedChanged="rbtnFormView_OnCheckedChanged" />&nbsp;&nbsp;&nbsp;
                                                            <asp:RadioButton ID="rbtnSalesOrder" Font-Bold="true" runat="server" Text="From Sales Order" AutoPostBack="true" GroupName="Product" OnCheckedChanged="rbtnFormView_OnCheckedChanged" />
                                                            <br />
                                                        </div>
                                                        <div class="col-md-6">
                                                            <asp:Button ID="btnAddNewProduct" Style="display: none" runat="server" Text="<%$ Resources:Attendance,Add Product %>"
                                                                CssClass="btn btn-info" OnClick="btnAddNewProduct_Click" Visible="False" />

                                                            <asp:Button ID="btnAddProductScreen" Visible="false" runat="server" Text="<%$ Resources:Attendance,Add Product List %>" CssClass="btn btn-info" OnClick="btnAddProductScreen_Click" />

                                                            <asp:Button ID="btnAddtoList" runat="server" Text="<%$ Resources:Attendance,Fill Your Product %>" CssClass="btn btn-info" Visible="false" OnClick="btnAddtoList_Click" />
                                                            <br />
                                                        </div>
                                                        <div class="col-lg-12">
                                                            <br />
                                                        </div>
                                                        <div id="pnlProduct1" runat="server" class="col-md-12">
                                                            <%--<div class="row">
                                                            <div class="col-md-12">--%>
                                                            <div class="box box-primary">
                                                                <div class="box-header with-border">
                                                                    <h3 class="box-title">
                                                                        <asp:Label ID="lbladdproduct" runat="server" Text="<%$ Resources:Attendance,Add Product %>"></asp:Label></h3>
                                                                    <div class="box-tools pull-right">
                                                                        <button type="button" class="btn btn-box-tool" data-widget="collapse">
                                                                            <i class="fa fa-minus"></i>
                                                                        </button>
                                                                    </div>
                                                                </div>
                                                                <div class="box-body">
                                                                    <div class="form-group">
                                                                        <div class="col-md-6">
                                                                            <asp:HiddenField runat="server" ID="hdnSOID" />
                                                                            <asp:HiddenField runat="server" ID="hdnSONO" />

                                                                            <asp:Label ID="Label3" runat="server" Text="<%$ Resources:Attendance,Product Id%>" />
                                                                            <a style="color: Red">*</a>
                                                                            <asp:RequiredFieldValidator EnableClientScript="true" Style="float: right;" runat="server" ID="RequiredFieldValidator4" ValidationGroup="Add"
                                                                                Display="Dynamic" SetFocusOnError="true" ControlToValidate="txtProductcode" ErrorMessage="<%$ Resources:Attendance,Enter Product Id%>"></asp:RequiredFieldValidator>

                                                                            <asp:TextBox ID="txtProductcode" runat="server" CssClass="form-control" OnTextChanged="txtProductCode_TextChanged" AutoPostBack="true" BackColor="#eeeeee" />
                                                                            <cc1:AutoCompleteExtender ID="AutoCompleteExtender2" runat="server" CompletionInterval="100"
                                                                                DelimiterCharacters="" Enabled="True" MinimumPrefixLength="1" ServiceMethod="GetCompletionListProductCode"
                                                                                TargetControlID="txtProductcode" UseContextKey="True" CompletionListCssClass="autoCompleteList" CompletionListItemCssClass="autoCompleteListItem" CompletionListHighlightedItemCssClass="autoCompleteSelectedListItem" OnClientShown="resetPosition">
                                                                            </cc1:AutoCompleteExtender>
                                                                            <br />
                                                                        </div>
                                                                        <div class="col-md-6">
                                                                            <asp:Label ID="lblProductName" runat="server" Text="<%$ Resources:Attendance,Product Name %>" />
                                                                            <a style="color: Red">*</a>
                                                                            <asp:RequiredFieldValidator EnableClientScript="true" Style="float: right;" runat="server" ID="RequiredFieldValidator5" ValidationGroup="Add"
                                                                                Display="Dynamic" SetFocusOnError="true" ControlToValidate="txtProductName" ErrorMessage="<%$ Resources:Attendance,Enter Product Name%>"></asp:RequiredFieldValidator>

                                                                            <asp:TextBox ID="txtProductName" runat="server" CssClass="form-control" OnTextChanged="txtProductName_TextChanged" AutoPostBack="true" BackColor="#eeeeee" />
                                                                            <cc1:AutoCompleteExtender ID="AutoCompleteExtender1" runat="server" CompletionInterval="100"
                                                                                DelimiterCharacters="" Enabled="True" MinimumPrefixLength="1" ServiceMethod="GetCompletionListProductName"
                                                                                TargetControlID="txtProductName" UseContextKey="True" CompletionListCssClass="autoCompleteList" CompletionListItemCssClass="autoCompleteListItem" CompletionListHighlightedItemCssClass="autoCompleteSelectedListItem" OnClientShown="resetPosition">
                                                                            </cc1:AutoCompleteExtender>
                                                                            <asp:HiddenField ID="hdnNewProductId" runat="server" Value="0" />
                                                                            <asp:HiddenField ID="hdnProductDesc" runat="server" />

                                                                            <br />
                                                                        </div>
                                                                        <div class="col-md-6">
                                                                            <asp:Label ID="lblUnit" runat="server" Text="<%$ Resources:Attendance,Unit %>" />
                                                                            <a style="color: Red">*</a>
                                                                            <asp:RequiredFieldValidator EnableClientScript="true" Style="float: right;" runat="server" ID="RequiredFieldValidator7" ValidationGroup="Add" Display="Dynamic"
                                                                                SetFocusOnError="true" ControlToValidate="ddlUnit" InitialValue="--Select--" ErrorMessage="<%$ Resources:Attendance,Select Unit %>" />

                                                                            <asp:DropDownList ID="ddlUnit" runat="server" CssClass="form-control" />
                                                                            <asp:TextBox ID="txtUnit" runat="server" CssClass="form-control" Visible="False" />
                                                                            <asp:HiddenField ID="hdnNewUnitId" runat="server" Value="0" />
                                                                            <br />
                                                                        </div>
                                                                        <div class="col-md-6">
                                                                            <asp:Label ID="lblRequestQty" runat="server" Text="<%$ Resources:Attendance,Request Quantity %>" />
                                                                            <a style="color: Red">*</a>
                                                                            <asp:RequiredFieldValidator EnableClientScript="true" Style="float: right;" runat="server" ID="RequiredFieldValidator6" ValidationGroup="Add"
                                                                                Display="Dynamic" SetFocusOnError="true" ControlToValidate="txtRequestQty" ErrorMessage="<%$ Resources:Attendance,Enter Request Quantity%>"></asp:RequiredFieldValidator>

                                                                            <asp:TextBox ID="txtRequestQty" runat="server" CssClass="form-control" Text="1" />

                                                                            <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" runat="server" Enabled="True"
                                                                                TargetControlID="txtRequestQty" ValidChars="1,2,3,4,5,6,7,8,9,0,.">
                                                                            </cc1:FilteredTextBoxExtender>
                                                                            <br />
                                                                        </div>
                                                                        <div class="col-md-12">
                                                                            <asp:Label ID="lblPDescription" runat="server" Text="<%$ Resources:Attendance,Product Description %>" />
                                                                            <%--<a style="color: Red">*</a>
                                                                            <asp:RequiredFieldValidator EnableClientScript="true" Style="float: right;" runat="server" ID="RequiredFieldValidator8" ValidationGroup="Add"
                                                                                Display="Dynamic" SetFocusOnError="true" ControlToValidate="txtPDesc" ErrorMessage="<%$ Resources:Attendance,Enter Product Description%>"></asp:RequiredFieldValidator>--%>

                                                                            <asp:Panel ID="pnlPDescription0" runat="server" Style="max-height: 300px; height: 50px;" CssClass="form-control"
                                                                                BorderColor="#8ca7c1" BackColor="#ffffff" ScrollBars="Vertical" Visible="false">
                                                                                <asp:Literal ID="txtPDescription" runat="server"></asp:Literal>
                                                                            </asp:Panel>
                                                                            <asp:TextBox ID="txtPDesc" runat="server" TextMode="MultiLine"
                                                                                CssClass="form-control"></asp:TextBox>
                                                                            <br />
                                                                        </div>
                                                                        <div class="col-md-12" style="text-align: center">
                                                                            <asp:Button ID="btnProductSave" runat="server" Text="<%$ Resources:Attendance,Add Product %>" ValidationGroup="Add"
                                                                                CssClass="btn btn-success" Visible="false" OnClick="btnProductSave_Click" />

                                                                            <%--<asp:Button ID="btnProductCancel" runat="server" CssClass="btn btn-primary" Text="<%$ Resources:Attendance,Reset %>"
                                                                                CausesValidation="False" OnClick="btnProductCancel_Click" />--%>
                                                                            <a onclick="btnProductReset_Click()" class="btn btn-primary"><%= Resources.Attendance.Reset %></a>

                                                                            <asp:Button ID="btnProductClose" runat="server" CssClass="btn btn-danger" Text="<%$ Resources:Attendance,Close %>"
                                                                                CausesValidation="False" OnClick="btnProductClose_Click" />

                                                                            <asp:HiddenField ID="hidProduct" runat="server" />
                                                                            <br />
                                                                        </div>
                                                                    </div>
                                                                </div>
                                                            </div>

                                                            <br />
                                                        </div>
                                                        <div runat="server" id="div_salesOrder" visible="false" class="col-md-12" style="min-height: 50px; max-height: 300px; overflow: auto">
                                                            <asp:GridView CssClass="table-striped table-bordered table table-hover" runat="server" ID="gvPendingSalesOrder" AutoGenerateColumns="false" Width="100%">
                                                                <Columns>
                                                                    <asp:TemplateField>
                                                                        <ItemTemplate>
                                                                            <asp:CheckBox runat="server" ID="chkAddSOToPO" OnCheckedChanged="chkAddSOToPO_CheckedChanged" AutoPostBack="true" />
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="SO No">
                                                                        <ItemTemplate>
                                                                            <asp:Label runat="server" ID="lblOrderNo" Text='<%# Eval("SalesOrderNo") %>'></asp:Label>
                                                                            <asp:HiddenField runat="server" ID="gvOrderId" Value='<%# Eval("trans_id") %>'></asp:HiddenField>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="SO Date">
                                                                        <ItemTemplate>
                                                                            <%#  Eval("salesorderdate").ToString()==""?"":Convert.ToDateTime( Eval("salesorderdate").ToString()).ToString("dd-MMM-yyyy") %>
                                                                        </ItemTemplate>
                                                                        <ItemStyle Width="80px" />
                                                                    </asp:TemplateField>

                                                                    <asp:TemplateField HeaderText="Customer">
                                                                        <ItemTemplate>
                                                                            <%# Eval("Name") %>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>

                                                                    <asp:TemplateField HeaderText="Product ID">
                                                                        <ItemTemplate>
                                                                            <asp:Label runat="server" ID="lblProductCode" Text='<%# Eval("ProductCode") %>'></asp:Label>
                                                                            <asp:HiddenField runat="server" ID="gvhdnProductId" Value='<%# Eval("Product_Id") %>'></asp:HiddenField>
                                                                            <asp:HiddenField runat="server" ID="gvHdnUnitCost" Value='<%# Eval("SalesPrice1") %>'></asp:HiddenField>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Product Name">
                                                                        <ItemTemplate>
                                                                            <asp:Label runat="server" ID="lblProductName" Text='<%# Eval("EproductName") %>'></asp:Label>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Order Qty">
                                                                        <ItemTemplate>
                                                                            <asp:Label runat="server" ID="lblQuantity" Text='<%# Eval("Quantity") %>'></asp:Label>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="System Qty">
                                                                        <ItemTemplate>
                                                                            <asp:Label runat="server" ID="lblSysQuantity" Text='<%# Eval("sysqty") %>'></asp:Label>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Unit Name">
                                                                        <ItemTemplate>
                                                                            <asp:Label runat="server" ID="lblUnit" Text='<%# Eval("Unit_name") %>'></asp:Label>
                                                                            <asp:HiddenField runat="server" ID="gvhdnUnitId" Value='<%# Eval("Unit_Id") %>'></asp:HiddenField>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>

                                                                </Columns>
                                                            </asp:GridView>
                                                        </div>
                                                        <div class="col-lg-12">
                                                            <br />
                                                        </div>
                                                        <div class="col-md-12">
                                                            <div style="overflow: auto; max-height: 500px;">
                                                                <asp:GridView CssClass="table-striped table-bordered table table-hover" ID="GvProduct" runat="server" Width="100%" AutoGenerateColumns="False">

                                                                    <Columns>
                                                                        <asp:TemplateField HeaderText="Edit" Visible="false">
                                                                            <ItemTemplate>
                                                                                <asp:LinkButton ID="imgBtnProductEdit" Visible='<%# hdnCanEdit.Value=="true"?true:false%>' runat="server" CommandArgument='<%# Eval("Serial_No") %>' OnCommand="imgBtnProductEdit_Command"><i class="fa fa-pencil"></i></asp:LinkButton>
                                                                            </ItemTemplate>
                                                                            <ItemStyle HorizontalAlign="Center" />
                                                                        </asp:TemplateField>
                                                                        <asp:TemplateField HeaderText="Delete" Visible="false">
                                                                            <ItemTemplate>
                                                                                <asp:LinkButton ID="imgBtnProductDelete" Visible='<%# hdnCanDelete.Value=="true"?true:false%>' runat="server" CommandArgument='<%# Eval("Serial_No") %>' OnCommand="imgBtnProductDelete_Command"><i class="fa fa-trash"></i></asp:LinkButton>
                                                                            </ItemTemplate>
                                                                            <ItemStyle HorizontalAlign="Center" />
                                                                        </asp:TemplateField>
                                                                        <asp:TemplateField HeaderText="<%$ Resources:Attendance,S No. %>">
                                                                            <ItemTemplate>
                                                                                <asp:Label ID="lblSNo" runat="server" Text='<%#Eval("Serial_No") %>' Visible="false" />
                                                                                <asp:Label ID="lblSerialNo" runat="server" Text='<%#Container.DataItemIndex+1 %>' />
                                                                            </ItemTemplate>
                                                                            <ItemStyle HorizontalAlign="Center" Width="50px" />
                                                                        </asp:TemplateField>

                                                                        <asp:TemplateField HeaderText="SO No.">
                                                                            <ItemTemplate>
                                                                                <asp:Label ID="gvlblSONo" runat="server" Text='<%#Eval("SONO") %>' />
                                                                                <asp:HiddenField ID="gvhdnSOID" runat="server" Value='<%# Eval("SOID").ToString()==""?"0":Eval("SOID").ToString() %>' />
                                                                            </ItemTemplate>
                                                                            <ItemStyle HorizontalAlign="Center" />
                                                                        </asp:TemplateField>

                                                                        <asp:TemplateField HeaderText="<%$ Resources:Attendance,Product Id%>">
                                                                            <ItemTemplate>
                                                                                <asp:Label ID="lblproductcode" runat="server" Text='<%# ProductCode(Eval("Product_Id").ToString()) %>'></asp:Label>
                                                                            </ItemTemplate>
                                                                            <ItemStyle />
                                                                        </asp:TemplateField>
                                                                        <asp:TemplateField HeaderText="<%$ Resources:Attendance, Product Name %>">
                                                                            <ItemTemplate>
                                                                                <table width="100%">
                                                                                    <tr>
                                                                                        <td>
                                                                                            <asp:HiddenField ID="hdnSuggestedProductName" runat="server" Value='<%#Eval("SuggestedProductName") %>' />
                                                                                            <asp:Label ID="lblgvProductId" runat="server" Text='<%#Eval("Product_Id") %>' Visible="false" />
                                                                                            <asp:Label ID="lblgvProductName" runat="server" Text='<%#SuggestedProductName(Eval("Product_Id").ToString(),Eval("Serial_No").ToString()) %>' />
                                                                                            <asp:Literal ID="lblgvProductDescription" runat="server" Text='<%# Eval("ProductDescription") %>'></asp:Literal>
                                                                                        </td>
                                                                                    </tr>
                                                                                </table>
                                                                            </ItemTemplate>
                                                                            <ItemStyle HorizontalAlign="Center" />
                                                                        </asp:TemplateField>
                                                                        <asp:TemplateField HeaderText="<%$ Resources:Attendance, Unit %>">
                                                                            <ItemTemplate>
                                                                                <asp:Label ID="lblgvUnitId" runat="server" Visible="false" Text='<%#Eval("UnitId") %>' />
                                                                                <asp:Label ID="lblgvUnit" runat="server" Text='<%#GetUnitName(Eval("UnitId").ToString()) %>' />
                                                                            </ItemTemplate>
                                                                            <ItemStyle HorizontalAlign="Center" />
                                                                        </asp:TemplateField>
                                                                        <asp:TemplateField HeaderText="<%$ Resources:Attendance, Required Quantity %>">
                                                                            <ItemTemplate>
                                                                                <asp:Label ID="lblgvRequiredQty" runat="server" Text='<%# Eval("ReqQty") %>' />
                                                                            </ItemTemplate>
                                                                            <ItemStyle HorizontalAlign="Center" />
                                                                        </asp:TemplateField>
                                                                    </Columns>

                                                                    <PagerStyle CssClass="pagination-ys" />

                                                                </asp:GridView>
                                                                <asp:HiddenField ID="hdnProductId" runat="server" />
                                                                <asp:HiddenField ID="hdnProductName" runat="server" />
                                                            </div>
                                                            <br />
                                                        </div>
                                                        <div class="col-md-6">
                                                            <asp:Label ID="Label4" runat="server" Text="<%$ Resources:Attendance,Budget Price%>"></asp:Label>
                                                            <asp:TextBox ID="txtBudgetPrice" runat="server" CssClass="form-control" />
                                                            <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender2" runat="server" Enabled="True"
                                                                TargetControlID="txtBudgetPrice" ValidChars="1,2,3,4,5,6,7,8,9,0,.">
                                                            </cc1:FilteredTextBoxExtender>
                                                            <br />
                                                        </div>
                                                        <div class="col-md-6">
                                                            <asp:Label ID="Label11" runat="server" Text="<%$ Resources:Attendance,Currency %>"></asp:Label>
                                                            <a style="color: Red">*</a>
                                                            <asp:RequiredFieldValidator EnableClientScript="true" Style="float: right;" runat="server" ID="RequiredFieldValidator2" ValidationGroup="Save" Display="Dynamic"
                                                                SetFocusOnError="true" ControlToValidate="ddlCurrency" InitialValue="--Select--" ErrorMessage="<%$ Resources:Attendance,Select Currency %>" />

                                                            <asp:DropDownList ID="ddlCurrency" runat="server" CssClass="form-control">
                                                            </asp:DropDownList>
                                                            <br />
                                                        </div>
                                                        <div class="col-md-12">
                                                            <asp:Label ID="lblDesription" runat="server" Text="<%$ Resources:Attendance,Remark %>" />
                                                            <asp:TextBox ID="txtDescription" runat="server" TextMode="MultiLine"
                                                                CssClass="form-control" Font-Names="Arial" />
                                                            <br />
                                                        </div>
                                                        <div class="col-md-12">
                                                            <cc1:TabContainer ID="TabContainer2" runat="server" ActiveTabIndex="0" CssClass="ajax__tab_yuitabview-theme">
                                                                <cc1:TabPanel ID="TabPanel2" runat="server" HeaderText="<%$ Resources:Attendance,Header %>">
                                                                    <ContentTemplate>
                                                                        <asp:UpdatePanel ID="Update_TabPanel2" runat="server">
                                                                            <ContentTemplate>
                                                                                <div class="row">
                                                                                    <div class="col-md-12">
                                                                                        <cc1:Editor ID="txtHeader" runat="server" Width="100%"></cc1:Editor>
                                                                                    </div>
                                                                                </div>
                                                                            </ContentTemplate>
                                                                        </asp:UpdatePanel>
                                                                        <asp:UpdateProgress ID="UpdateProgress6" runat="server" AssociatedUpdatePanelID="Update_TabPanel2">
                                                                            <ProgressTemplate>
                                                                                <div class="modal_Progress">
                                                                                    <div class="center_Progress">
                                                                                        <img alt="" src="../Images/ajax-loader2.gif" style="vertical-align: middle; background-color: White; border-radius: 10px;" />
                                                                                    </div>
                                                                                </div>
                                                                            </ProgressTemplate>
                                                                        </asp:UpdateProgress>
                                                                    </ContentTemplate>
                                                                </cc1:TabPanel>
                                                                <cc1:TabPanel ID="TabPanel1" runat="server" HeaderText="<%$ Resources:Attendance,Footer %>">
                                                                    <ContentTemplate>
                                                                        <asp:UpdatePanel ID="Update_TabPanel1" runat="server">
                                                                            <ContentTemplate>
                                                                                <div class="row">
                                                                                    <div class="col-md-12">
                                                                                        <cc1:Editor ID="txtFooter" runat="server" Width="100%"></cc1:Editor>
                                                                                    </div>
                                                                                </div>
                                                                            </ContentTemplate>
                                                                        </asp:UpdatePanel>
                                                                        <asp:UpdateProgress ID="UpdateProgress5" runat="server" AssociatedUpdatePanelID="Update_TabPanel1">
                                                                            <ProgressTemplate>
                                                                                <div class="modal_Progress">
                                                                                    <div class="center_Progress">
                                                                                        <img alt="" src="../Images/ajax-loader2.gif" style="vertical-align: middle; background-color: White; border-radius: 10px;" />
                                                                                    </div>
                                                                                </div>
                                                                            </ProgressTemplate>
                                                                        </asp:UpdateProgress>
                                                                    </ContentTemplate>
                                                                </cc1:TabPanel>
                                                            </cc1:TabContainer>
                                                            <br />
                                                        </div>
                                                        <div class="col-md-12" style="text-align: center">
                                                            <br />
                                                            <asp:Button ID="btnPISave" runat="server" Text="<%$ Resources:Attendance,Next %>"
                                                                CssClass="btn btn-primary" OnClick="btnPISave_Click" />

                                                            <asp:Button ID="BtnReset" runat="server" Text="<%$ Resources:Attendance,Reset %>"
                                                                CssClass="btn btn-primary" CausesValidation="False" OnClick="BtnReset_Click" />

                                                            <asp:Button ID="btnPICancel" runat="server" CssClass="btn btn-danger" Text="<%$ Resources:Attendance,Cancel %>"
                                                                CausesValidation="False" OnClick="btnPICancel_Click" />

                                                            <asp:HiddenField ID="editid" runat="server" />
                                                            <br />
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                                <div id="Next_Div" runat="server" visible="false">
                                    <div class="row">
                                        <div class="col-md-12">
                                            <div class="box box-primary">
                                                <div class="box-body">
                                                    <div class="form-group">
                                                        <div id="tr1" runat="server">
                                                            <div class="col-md-6">
                                                                <asp:Label ID="lblSInquiryDate" runat="server" Text="<%$ Resources:Attendance, Inquiry Date %>" />
                                                                <asp:Label ID="lblSIdtColon" Text=":" runat="server"></asp:Label>
                                                                <asp:TextBox ID="txtSInquiryDate" runat="server" CssClass="form-control" ReadOnly="True" />
                                                                <br />
                                                            </div>
                                                            <div class="col-md-6">
                                                                <asp:Label ID="lblSInquiryNo" runat="server" Text="<%$ Resources:Attendance,Inquiry No. %>" />
                                                                <asp:Label ID="lblSuppNoColon" Text=":" runat="server"></asp:Label>
                                                                <asp:TextBox ID="txtSInquiryNo" runat="server" CssClass="form-control" ReadOnly="True" />
                                                                <br />
                                                            </div>
                                                        </div>
                                                        <div id="tr2" runat="server">
                                                            <div class="col-md-6">
                                                                <asp:Label ID="lblSupplierGroup" runat="server" Text="<%$ Resources:Attendance, Supplier Group %>" />
                                                                <asp:Label ID="sgrp" runat="server" Text=":"></asp:Label>
                                                                <asp:DropDownList ID="ddlSupplierGroup" runat="server" CssClass="form-control"
                                                                    OnSelectedIndexChanged="ddlSupplierGroup_SelectedIndexChanged" AutoPostBack="true" />
                                                                <br />
                                                            </div>
                                                            <div class="col-md-6" style="text-align: center">
                                                                <br />
                                                                <asp:Button ID="BtnShowCpriceList" runat="server" Text="<%$ Resources:Attendance,Supplier Price List %>" CssClass="btn btn-info" OnClick="BtnShowCpriceList_click" />
                                                                <br />
                                                            </div>
                                                        </div>
                                                        <div class="col-md-12"></div>
                                                        <div id="traddsup" runat="server">
                                                            <div class="col-md-6">
                                                                <asp:Label ID="lblSupplier" runat="server" Text="<%$ Resources:Attendance,Select Supplier %>" />
                                                                <div class="input-group">
                                                                    <asp:TextBox ID="txtSuppliers" runat="server" BackColor="#eeeeee" OnTextChanged="txtSuppliers_OnTextChanged"
                                                                        AutoPostBack="True" CssClass="form-control"></asp:TextBox>
                                                                    <cc1:AutoCompleteExtender ID="txtSuppliers_AutoCompleteExtender" runat="server" CompletionInterval="100"
                                                                        DelimiterCharacters="" Enabled="True" MinimumPrefixLength="1" ServiceMethod="GetCompletionList_Supplier"
                                                                        ServicePath="" TargetControlID="txtSuppliers" UseContextKey="True" CompletionListCssClass="autoCompleteList" CompletionListItemCssClass="autoCompleteListItem" CompletionListHighlightedItemCssClass="autoCompleteSelectedListItem" OnClientShown="resetPosition">
                                                                    </cc1:AutoCompleteExtender>
                                                                    <div class="input-group-btn">
                                                                        <asp:LinkButton ID="IbtnAddProductSupplierCode" runat="server" CausesValidation="False" OnClick="IbtnAddProductSupplierCode_Click" ToolTip="<%$ Resources:Attendance,Add %>"><i class="fa fa-plus-square" style="font-size:35px;padding:5px;margin-top: -5px;"></i></asp:LinkButton>
                                                                    </div>
                                                                </div>
                                                                <asp:HiddenField ID="hdnProductSupplierCode" runat="server" Value="0" />
                                                                <br />
                                                            </div>
                                                            <div class="col-md-6" style="text-align: center">
                                                                <br />
                                                                <asp:Button ID="btnGetRecommendedsupplier" runat="server" CssClass="btn btn-primary" OnClick="btnGetRecommendedsupplier_Click" Text="Get Recommended Supplier" />
                                                                <br />
                                                            </div>
                                                        </div>
                                                        <div class="col-md-12">
                                                            <div style="overflow: auto; max-height: 500px;">
                                                                <asp:GridView CssClass="table-striped table-bordered table table-hover" ID="GridProductSupplierCode" runat="server" AllowPaging="false" AutoGenerateColumns="False"
                                                                    BorderStyle="Solid" Width="100%" PageSize="5" OnPageIndexChanging="GridProductSupplierCode_PageIndexChanging">
                                                                    <Columns>
                                                                        <asp:TemplateField>
                                                                            <ItemTemplate>
                                                                                <asp:CheckBox ID="chkSupplier" runat="server" />
                                                                            </ItemTemplate>
                                                                            <ItemStyle HorizontalAlign="Center" />
                                                                        </asp:TemplateField>
                                                                        <asp:TemplateField HeaderText="<%$ Resources:Attendance,Delete %>">
                                                                            <ItemTemplate>
                                                                                <asp:LinkButton ID="IbtnDeleteSupplier" runat="server" CausesValidation="False"
                                                                                    CommandArgument='<%# Eval("Supplier_Id") %>' ToolTip="<%$ Resources:Attendance,Delete %>" OnCommand="IbtnDeleteSupplier_Command"><i class="fa fa-trash"></i> </asp:LinkButton>
                                                                            </ItemTemplate>
                                                                            <ItemStyle HorizontalAlign="Center" />
                                                                        </asp:TemplateField>
                                                                        <asp:TemplateField HeaderText="<%$ Resources:Attendance,Supplier Name %>" SortExpression="Name">
                                                                            <ItemTemplate>
                                                                                <asp:Label ID="lblgvSupplierName" runat="server" Text='<%#Eval("Name") %>' />
                                                                                <asp:Label ID="lblgvsupId" runat="server" Text='<%#Eval("Supplier_Id") %>' Visible="false" />
                                                                            </ItemTemplate>
                                                                            <ItemStyle Width="60%" />
                                                                        </asp:TemplateField>
                                                                    </Columns>


                                                                    <PagerStyle CssClass="pagination-ys" />

                                                                </asp:GridView>
                                                                <asp:HiddenField ID="hdnfPSC" runat="server" />
                                                            </div>
                                                            <br />
                                                        </div>
                                                        <div class="col-md-12">
                                                            <asp:Panel ID="pnlChkSupplier" runat="server" Style="max-height: 300px;" BorderStyle="Solid"
                                                                BorderWidth="1px" BorderColor="#abadb3" BackColor="White" ScrollBars="Auto" Visible="false">
                                                                <asp:CheckBoxList ID="ChkSupplier" runat="server" RepeatColumns="1" RepeatDirection="Vertical"
                                                                    Font-Names="Trebuchet MS" Font-Size="Small" ForeColor="Gray" />
                                                            </asp:Panel>
                                                            <br />
                                                        </div>
                                                        <div class="col-md-12">
                                                            <hr />
                                                            <br />
                                                        </div>
                                                        <div class="col-md-12" style="text-align: center">
                                                            <asp:Button ID="btnSaveSupplier" runat="server" Text="<%$ Resources:Attendance,Save %>"
                                                                CssClass="btn btn-success" OnClick="btnSaveSupplier_Click" Visible="False" />

                                                            <asp:Button ID="btnResetSupplier" runat="server" Text="<%$ Resources:Attendance,Reset %>"
                                                                CssClass="btn btn-primary" CausesValidation="False" OnClick="btnResetSupplier_Click" />

                                                            <asp:Button ID="btnCancelSupplier" runat="server" CssClass="btn btn-danger" Text="<%$ Resources:Attendance,Cancel %>"
                                                                CausesValidation="False" OnClick="btnCancelSupplier_Click" />

                                                            <asp:HiddenField ID="hdnSupplierId" runat="server" />
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
                                                    <asp:Label ID="Label2" runat="server" Text="Advance Search"></asp:Label></h3>
                                                &nbsp;&nbsp;|&nbsp;&nbsp;
				<asp:Label ID="lblTotalRecordsBin" Font-Bold="true" runat="server" Text="<%$ Resources:Attendance,Total Records: 0 %>"></asp:Label>
                                                <div class="box-tools pull-right">
                                                    <button type="button" class="btn btn-box-tool" data-widget="collapse">
                                                        <i id="I2" runat="server" class="fa fa-plus"></i>
                                                    </button>
                                                </div>
                                            </div>
                                            <div class="box-body">
                                                <div class="col-lg-3">
                                                    <asp:DropDownList ID="ddlFieldNameBin" runat="server" CssClass="form-control" OnSelectedIndexChanged="ddlFieldNameBin_SelectedIndexChanged" AutoPostBack="true">
                                                        <asp:ListItem Text="<%$ Resources:Attendance, Inquiry No. %>" Value="PI_No" Selected="True"></asp:ListItem>
                                                        <asp:ListItem Text="<%$ Resources:Attendance, Inquiry Date %>" Value="PIDate"></asp:ListItem>
                                                        <asp:ListItem Text="<%$ Resources:Attendance, Ref. Type %>" Value="RefType"></asp:ListItem>
                                                        <asp:ListItem Text="<%$ Resources:Attendance, Ref No. %>" Value="TransNo"></asp:ListItem>
                                                        <asp:ListItem Text="<%$ Resources:Attendance, Customer %>" Value="Customer_Name"></asp:ListItem>
                                                        <asp:ListItem Text="<%$ Resources:Attendance, Status %>" Value="Status"></asp:ListItem>
                                                        <asp:ListItem Text="<%$ Resources:Attendance, Created By %>" Value="UserName"></asp:ListItem>
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
                                                    <asp:Panel ID="Panel2" runat="server" DefaultButton="btnbindBin">
                                                        <asp:TextBox ID="txtValueBin" runat="server" CssClass="form-control" placeholder="Search From Content"></asp:TextBox>
                                                        <asp:TextBox ID="txtValueBinDate" runat="server" CssClass="form-control" Visible="false" placeholder="Search From Date"></asp:TextBox>
                                                        <cc1:CalendarExtender OnClientShown="showCalendar" ID="CalendarExtendertxtValueBinDate" runat="server" TargetControlID="txtValueBinDate" />
                                                    </asp:Panel>
                                                </div>
                                                <div class="col-lg-3">
                                                    <asp:LinkButton ID="btnbindBin" runat="server" CausesValidation="False" OnClick="btnbindBin_Click" ToolTip="<%$ Resources:Attendance,Search %>"><span class="fa fa-search"  style="font-size:25px;"></span></asp:LinkButton>&nbsp;&nbsp;&nbsp;
                                                    <asp:LinkButton ID="btnRefreshBin" runat="server" CausesValidation="False" OnClick="btnRefreshBin_Click" ToolTip="<%$ Resources:Attendance,Refresh %>"><span class="fa fa-repeat"  style="font-size:25px;"></span></asp:LinkButton>&nbsp;&nbsp;&nbsp;
                                                    <asp:LinkButton ID="imgBtnRestore" CausesValidation="False" runat="server" OnClick="imgBtnRestore_Click" ToolTip="<%$ Resources:Attendance, Active %>"><span class="far fa-lightbulb"  style="font-size:25px;"></span></asp:LinkButton>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                                <div class="box box-warning box-solid" <%= GvPurchaseInquiryBin.Rows.Count>0?"style='display:block'":"style='display:none'"%>>
                                    <div class="box-body">
                                        <div class="row">
                                            <div class="col-md-12">
                                                <div class="flow">
                                                    <asp:Label ID="lblSelectedRecord" runat="server" Visible="false"></asp:Label>
                                                    <asp:GridView CssClass="table-striped table-bordered table table-hover" ID="GvPurchaseInquiryBin" PageSize="<%# PageControlCommon.GetPageSize() %>"
                                                        runat="server" AutoGenerateColumns="False" Width="100%" AllowPaging="True" OnPageIndexChanging="GvPurchaseInquiryBin_PageIndexChanging"
                                                        OnSorting="GvPurchaseInquiryBin_OnSorting" AllowSorting="true">
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
                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,Purchase Inquiry No.%>" SortExpression="PI_No">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblgvPINoId" runat="server" Text='<%# Eval("PI_No") %>' />
                                                                    <asp:HiddenField ID="hdnTransId" runat="server" Value='<%#Eval("Trans_Id") %>' />
                                                                </ItemTemplate>
                                                                <ItemStyle />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,Purchase Inquiry Date %>"
                                                                SortExpression="PIDate">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblgvPIDate" runat="server" Text='<%#GetDate(Eval("PIDate").ToString()) %>' />
                                                                </ItemTemplate>
                                                                <ItemStyle />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField SortExpression="TransFrom" HeaderText="<%$ Resources:Attendance,Ref. Type %>">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblRefType" runat="server" Text='<%#Eval("RefType") %>' />
                                                                </ItemTemplate>
                                                                <ItemStyle HorizontalAlign="Center" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField SortExpression="TransNo" HeaderText="<%$ Resources:Attendance,Ref No. %>">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblRefNo" runat="server" Text='<%#Eval("TransNo") %>' />
                                                                </ItemTemplate>
                                                                <ItemStyle HorizontalAlign="Center" />
                                                            </asp:TemplateField>

                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,Department %>">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblgvDept" runat="server" Text='<%# Eval("DepartmentName") %>' />
                                                                </ItemTemplate>
                                                                <ItemStyle HorizontalAlign="Center" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,Customer Name%>" SortExpression="Customer_Name">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblgvcustIdInq" runat="server" Text='<%# Eval("Customer_Name").ToString() %>' />
                                                                </ItemTemplate>
                                                                <ItemStyle HorizontalAlign="Center" />
                                                            </asp:TemplateField>
                                                            <%--<asp:TemplateField HeaderText="<%$ Resources:Attendance,Tender %>">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblgvTenderInq" runat="server" Text='<%# Eval("TenderNo").ToString() %>' />
                                                    </ItemTemplate>
                                                    <ItemStyle  HorizontalAlign="Center" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="<%$ Resources:Attendance,Closing Date %>">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblgvComDateInq" runat="server" Text='<%# Eval("OrderCompletionDate").ToString() %>' />
                                                    </ItemTemplate>
                                                    <ItemStyle  HorizontalAlign="Center" />
                                                </asp:TemplateField>--%>
                                                            <asp:BoundField DataField="Status" HeaderText="<%$ Resources:Attendance,Status %>"
                                                                SortExpression="Status" />
                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,Created By %>" SortExpression="UserName">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblCreated_User" runat="server" Text='<%# Eval("UserName") %>'></asp:Label>
                                                                </ItemTemplate>
                                                                <ItemStyle />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,Modified By %>" SortExpression="ModifiedUserName">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblModified_User" runat="server" Text='<%# Eval("ModifiedUserName") %>'></asp:Label>
                                                                </ItemTemplate>
                                                                <ItemStyle />
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
                    <div class="tab-pane" id="Request">
                        <asp:UpdatePanel ID="Update_Request" runat="server" UpdateMode="Conditional">
                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="Btn_Request" EventName="Click" />
                            </Triggers>
                            <ContentTemplate>

                                <div class="row">
                                    <div class="col-md-12">
                                        <div id="Div3" runat="server" class="box box-info collapsed-box">
                                            <div class="box-header with-border">
                                                <h3 class="box-title">
                                                    <asp:Label ID="Label7" runat="server" Text="Advance Search"></asp:Label></h3>
                                                &nbsp;&nbsp;|&nbsp;&nbsp;
				<asp:Label ID="lblTotalRecordsRequest" Font-Bold="true" runat="server" Text="<%$ Resources:Attendance,Total Records: 0 %>"></asp:Label>
                                                <asp:Label ID="lblSelectedRecordRequest" runat="server" Visible="false"></asp:Label>

                                                <div class="box-tools pull-right">
                                                    <button type="button" class="btn btn-box-tool" data-widget="collapse">
                                                        <i id="I3" runat="server" class="fa fa-plus"></i>
                                                    </button>
                                                </div>
                                            </div>
                                            <div class="box-body">
                                                <div class="col-lg-3">
                                                    <asp:DropDownList ID="ddlFieldNameRequest" runat="server" CssClass="form-control" OnSelectedIndexChanged="ddlFieldNameRequest_SelectedIndexChanged" AutoPostBack="true">
                                                        <asp:ListItem Selected="True" Text="<%$ Resources:Attendance,Request No %>" Value="RequestNo"></asp:ListItem>
                                                        <asp:ListItem Text="<%$ Resources:Attendance,Request Date %>" Value="RequestDate"></asp:ListItem>

                                                        <asp:ListItem Text="<%$ Resources:Attendance,Closing Date%>" Value="OrderCompletionDate"></asp:ListItem>
                                                        <asp:ListItem Text="<%$ Resources:Attendance,Expected Delivery Date %>" Value="ExpDelDate"></asp:ListItem>


                                                        <asp:ListItem Text="<%$ Resources:Attendance,Department%>" Value="DepartmentName"></asp:ListItem>
                                                        <asp:ListItem Text="<%$ Resources:Attendance,Created By %>" Value="UserName"></asp:ListItem>
                                                    </asp:DropDownList>
                                                </div>
                                                <div class="col-lg-2">
                                                    <asp:DropDownList ID="ddlOptionRequest" runat="server" CssClass="form-control">
                                                        <asp:ListItem Text="<%$ Resources:Attendance,--Select-- %>"></asp:ListItem>
                                                        <asp:ListItem Text="<%$ Resources:Attendance,Equal %>"></asp:ListItem>
                                                        <asp:ListItem Selected="True" Text="<%$ Resources:Attendance,Contains %>"></asp:ListItem>
                                                        <asp:ListItem Text="<%$ Resources:Attendance,Like %>"></asp:ListItem>

                                                    </asp:DropDownList>
                                                </div>
                                                <div class="col-lg-4">
                                                    <asp:Panel ID="Panel3" runat="server" DefaultButton="btnbindRequest">
                                                        <asp:TextBox ID="txtValueRequest" runat="server" CssClass="form-control" placeholder="Search From Content"></asp:TextBox>
                                                        <asp:TextBox ID="txtValueRequestDate" runat="server" CssClass="form-control" Visible="false" placeholder="Search From Date"></asp:TextBox>
                                                        <cc1:CalendarExtender OnClientShown="showCalendar" ID="CalendarExtendertxtValueRequestDate" runat="server" TargetControlID="txtValueRequestDate" />
                                                    </asp:Panel>
                                                </div>
                                                <div class="col-lg-3">
                                                    <asp:LinkButton ID="btnbindRequest" runat="server" CausesValidation="False" OnClick="btnbindRequest_Click" ToolTip="<%$ Resources:Attendance,Search %>"><span class="fa fa-search"  style="font-size:25px;"></span></asp:LinkButton>&nbsp;&nbsp;&nbsp;
                                                    <asp:LinkButton ID="btnRefreshRequest" runat="server" CausesValidation="False" OnClick="btnRefreshRequest_Click" ToolTip="<%$ Resources:Attendance,Refresh %>"><span class="fa fa-repeat"  style="font-size:25px;"></span></asp:LinkButton>
                                                </div>

                                            </div>
                                        </div>
                                    </div>
                                </div>
                                <div class="box box-warning box-solid" <%= GvPurchaseRequest.Rows.Count>0?"style='display:block'":"style='display:none'"%>>
                                    <div class="box-body">
                                        <div class="row">
                                            <div class="col-md-12">
                                                <div class="flow">
                                                    <asp:HiddenField ID="HDFSort" runat="server" />
                                                    <asp:GridView CssClass="table-striped table-bordered table table-hover" ID="GvPurchaseRequest" runat="server" AllowPaging="True" AllowSorting="True"
                                                        AutoGenerateColumns="False" OnPageIndexChanging="GvPurchaseRequest_PageIndexChanging"
                                                        OnSorting="GvPurchaseRequest_Sorting" PageSize="<%# PageControlCommon.GetPageSize() %>"
                                                        Width="100%">

                                                        <Columns>
                                                            <asp:TemplateField>
                                                                <ItemTemplate>
                                                                    <asp:Button ID="btnEdit" runat="server" BackColor="Transparent" BorderStyle="None"
                                                                        CausesValidation="False" CommandArgument='<%# Eval("Trans_Id") %>' CssClass="btnPull"
                                                                        OnCommand="btnPREdit_Command" />
                                                                </ItemTemplate>
                                                                <ItemStyle HorizontalAlign="Center" />
                                                            </asp:TemplateField>

                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,Reject %>">
                                                                <ItemTemplate>
                                                                    <asp:ImageButton ID="IbtnDelete" runat="server" CausesValidation="False" CommandArgument='<%# Eval("Trans_Id") %>' CommandName='<%# Eval("RequestType") %>'
                                                                        ToolTip="<%$ Resources:Attendance,Reject %>" ImageUrl="~/Images/disapprove.png"
                                                                        OnCommand="IbtnUpdateRequestStatus_Command" Width="16px" Visible="true" />
                                                                </ItemTemplate>
                                                                <ItemStyle HorizontalAlign="Center" />
                                                            </asp:TemplateField>


                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,Request Id %>" SortExpression="Trans_Id">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblgvRequestId" runat="server" Text='<%#Eval("Trans_Id") %>' />
                                                                </ItemTemplate>
                                                                <ItemStyle HorizontalAlign="Center" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,Request No %>" SortExpression="RequestNo">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblgvRequestNo" runat="server" Text='<%#Eval("RequestNo") %>' />
                                                                </ItemTemplate>
                                                                <ItemStyle HorizontalAlign="Center" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,Customer Name%>">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblgvcustId" runat="server" Text='<%#Eval("Name") %>' />
                                                                </ItemTemplate>
                                                                <ItemStyle HorizontalAlign="Center" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,Request Date %>" SortExpression="RequestDate">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblgvRequestDate" runat="server" Text='<%#GetDate(Eval("RequestDate").ToString()) %>' />
                                                                </ItemTemplate>
                                                                <ItemStyle HorizontalAlign="Center" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,Request Type %>">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblgvRequestType" runat="server" Text='<%#Eval("RequestType").ToString() %>' />
                                                                </ItemTemplate>
                                                                <ItemStyle HorizontalAlign="Center" />
                                                            </asp:TemplateField>

                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,Tender %>">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblgvTender" runat="server" Text='<%# Eval("TenderNo").ToString() %>' />
                                                                </ItemTemplate>
                                                                <ItemStyle HorizontalAlign="Center" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,Closing Date %>">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblgvComDate" runat="server" Text='<%#GetDate(Eval("OrderCompletionDate").ToString()) %>' />
                                                                </ItemTemplate>
                                                                <ItemStyle HorizontalAlign="Center" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,Expected Delivery Date %>"
                                                                SortExpression="ExpDelDate">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblExpDelDate" runat="server" Text='<%# GetDate(Eval("ExpDelDate").ToString()) %>'></asp:Label>
                                                                </ItemTemplate>
                                                                <ItemStyle />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,Department %>" SortExpression="DepartmentName">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblgvDept" runat="server" Text='<%# Eval("DepartmentName") %>' />
                                                                </ItemTemplate>
                                                                <ItemStyle HorizontalAlign="Center" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,Created By %>" SortExpression="UserName">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblgvUser" runat="server" Text='<%#Eval("UserName") %>' />
                                                                </ItemTemplate>
                                                                <ItemStyle HorizontalAlign="Center" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,Modified By %>" SortExpression="Modified_User">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblModified_User" runat="server" Text='<%# Eval("Modified_User") %>'></asp:Label>
                                                                </ItemTemplate>
                                                                <ItemStyle />
                                                            </asp:TemplateField>
                                                        </Columns>

                                                        <PagerStyle CssClass="pagination-ys" />

                                                    </asp:GridView>
                                                    <asp:HiddenField ID="hdnTransNo" runat="server" Value="0" />
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

    <asp:UpdateProgress ID="UpdateProgress4" runat="server" AssociatedUpdatePanelID="Update_Request">
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

        function LI_New_Request_Active() {
            $("#Li_Request").removeClass("active");
            $("#Request").removeClass("active");

            $("#Li_New").addClass("active");
            $("#New").addClass("active");
        }
        function Li_Tab_Bin() {
            document.getElementById('<%= Btn_Bin.ClientID %>').click();
        }
        function Li_Tab_Request() {
            document.getElementById('<%= Btn_Request.ClientID %>').click();
        }
    </script>
    <script>

        function checkDec(el) {
            var ex = /^[0-9]+\.?[0-9]*$/;
            if (ex.test(el.value) == false) {
                el.value = "";
                alert('Incorrect Number');
            }
        }

        function txtProductCode_TextChanged(ctrl) {
            debugger;
            $.ajax({
                url: '../Sales/SalesInquiry.aspx/txtProduct_TextChanged',
                method: 'post',
                async: false,
                contentType: "application/json; charset=utf-8",
                data: "{'product':'" + ctrl.value + "'}",
                success: function (data) {
                    debugger;
                    if (data.d == null) {
                        alert("Select From Suggestions only");
                        document.getElementById('<%=txtProductName.ClientID%>').value = "";
                        document.getElementById('<%=txtProductcode.ClientID%>').value = "";
                        return;
                    }
                    else {
                        var dd = JSON.parse(data.d);
                        document.getElementById('<%=txtProductName.ClientID%>').value = dd[0];
                        document.getElementById('<%=txtProductcode.ClientID%>').value = dd[1];
                        //document.getElementById('<%=ddlUnit.ClientID%>').selectedIndex = parseInt(dd[2]);
                        document.getElementById('<%=hdnNewProductId.ClientID%>').value = dd[4];
                        document.getElementById('<%=btnFillUnits.ClientID%>').click();
                        document.getElementById('<%=hdnProductDesc.ClientID%>').value = dd[3];

                    }
                }
            });
        }

        function btnProductReset_Click() {
            var txtProductName_ = document.getElementById('<%=txtProductName.ClientID%>');
            var ddlUnit_ = $('#<%=ddlUnit.ClientID%>');
            var txtRequestQty_ = document.getElementById('<%= txtRequestQty.ClientID%>');
            var hdnProductId_ = document.getElementById('<%=hdnProductId.ClientID%>');
            var hdnProductName_ = document.getElementById('<%=hdnProductName.ClientID%>');
            var hdnNewProductId_ = document.getElementById('<%=hdnNewProductId.ClientID%>');
            var txtPDesc_ = document.getElementById('<%=txtPDesc.ClientID%>');
            var txtProductcode_ = document.getElementById('<%=txtProductcode.ClientID%>');

            txtProductName_.value = "";
            txtRequestQty_.value = "1";
            ddlUnit_.empty();
            hdnProductId_.value = "";
            hdnProductName_.value = "";
            hdnNewProductId_.value = "";
            txtPDesc_.defaultValue = "";
            txtProductcode_.value = "";
            txtProductcode_.focus();
        }
        function IbtnPrint_Command(inquiryId) {
            window.open('../Purchase/PurchaseInquiryPrint.aspx?RId=' + inquiryId + '', 'window', 'width=1024');
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
         function getReportDataWithLoc(transId, locId) {
            $("#prgBar").css("display", "block");
            document.getElementById('<%= reportSystem.FindControl("hdnTransId").ClientID %>').value = transId;
            document.getElementById('<%= reportSystem.FindControl("hdnLocId").ClientID %>').value = locId;
             setReportData();
         }
    </script>
    <script src="../Script/ReportSystem.js"></script>
</asp:Content>
