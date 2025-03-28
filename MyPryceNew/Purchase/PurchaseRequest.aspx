<%@ Page Title="" Language="C#" MasterPageFile="~/ERPMaster.master" AutoEventWireup="true" CodeFile="PurchaseRequest.aspx.cs" Inherits="Purchase_PurchaseRequest" EnableEventValidation="false" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Src="~/WebUserControl/ReportSystem.ascx" TagName="ReportSystem" TagPrefix="RS" %>


<asp:Content ID="Content1" ContentPlaceHolderID="Head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="HeaderContent" runat="Server">
    <h1>
        <i class="fas fa-shopping-bag"></i>&nbsp;&nbsp;
        <asp:Label ID="lblHeader" runat="server" Text="<%$ Resources:Attendance,Purchase Request%>"></asp:Label>
    </h1>
    <ol class="breadcrumb">
        <li><a href="#"><i class="fa fa-dashboard"></i>
            <asp:Label ID="Label6" runat="server" Text="<%$ Resources:Attendance,Inventory%>"></asp:Label></a></li>
        <li><a href="#"><i class="fa fa-dashboard"></i>
            <asp:Label ID="Label10" runat="server" Text="<%$ Resources:Attendance,Purchase%>"></asp:Label></a></li>
        <li class="active">
            <asp:Label ID="Label5" runat="server" Text="<%$ Resources:Attendance,Purchase Request%>"></asp:Label></li>
    </ol>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="Server">
    <asp:UpdatePanel ID="Update_Button" runat="server">
        <ContentTemplate>
            <asp:Button ID="Btn_Bin" Style="display: none;" runat="server" OnClick="btnBin_Click" Text="Bin" />
            <asp:Button ID="Btn_Purchase_Request_Modal" Style="display: none;" runat="server" data-toggle="modal" data-target="#Purchase_Request" Text="View Modal" />
            <asp:HiddenField runat="server" ID="hdnCanView" />
            <asp:HiddenField runat="server" ID="hdnCanEdit" />
            <asp:HiddenField runat="server" ID="hdnCanDelete" />
            <asp:HiddenField runat="server" ID="hdnCanPrint" />
            <asp:HiddenField runat="server" ID="hdnLocationID" />

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
                                <asp:AsyncPostBackTrigger ControlID="imgBtnRestore" />
                                <%--<asp:AsyncPostBackTrigger ControlID="btnSave" EventName="Click" />--%>
                            </Triggers>
                            <ContentTemplate>

                                <div class="row">
                                    <div class="col-md-12">
                                        <div id="Div1" runat="server" class="box box-info collapsed-box">
                                            <div class="box-header with-border">
                                                <h3 class="box-title">
                                                    <asp:Label ID="Label22" runat="server" Text="Advance Search"></asp:Label></h3>
                                                &nbsp;&nbsp;|&nbsp;&nbsp;
				 <asp:Label ID="lblTotalRecords" runat="server" Font-Bold="true" Text="<%$ Resources:Attendance,Total Records: 0 %>"></asp:Label>
                                                <asp:Label ID="lblSelectedRecord" runat="server" Visible="false"></asp:Label>

                                                <div class="box-tools pull-right">
                                                    <button type="button" class="btn btn-box-tool" data-widget="collapse">
                                                        <i id="I1" runat="server" class="fa fa-plus"></i>
                                                    </button>
                                                </div>
                                            </div>
                                            <div class="box-body">
                                                <div class="col-lg-12">
                                                    <asp:DropDownList ID="ddlLocation" runat="server" Class="form-control" OnSelectedIndexChanged="ddlLocation_SelectedIndexChanged"
                                                        AutoPostBack="true">
                                                    </asp:DropDownList>
                                                    <br />
                                                </div>
                                                <div class="col-lg-2">
                                                    <asp:DropDownList ID="ddlStatus" runat="server" CssClass="form-control" AutoPostBack="true" OnSelectedIndexChanged="ddlStatus_SelectedIndexChanged">
                                                        <asp:ListItem Text="<%$ Resources:Attendance,Open %>" Value="Open" Selected="True"></asp:ListItem>
                                                        <asp:ListItem Text="<%$ Resources:Attendance,Close %>" Value="Close"></asp:ListItem>
                                                        <asp:ListItem Text="Reject" Value="Reject"></asp:ListItem>
                                                        <asp:ListItem Text="<%$ Resources:Attendance,All %>" Value="0"></asp:ListItem>
                                                    </asp:DropDownList>
                                                </div>
                                                <div class="col-lg-2">
                                                    <asp:DropDownList ID="ddlFieldName" runat="server" CssClass="form-control" AutoPostBack="true" OnSelectedIndexChanged="ddlFieldName_SelectedIndexChanged">
                                                        <asp:ListItem Selected="True" Text="<%$ Resources:Attendance,Request No %>" Value="RequestNo"></asp:ListItem>
                                                        <asp:ListItem Text="<%$ Resources:Attendance,Request Date %>" Value="RequestDate"></asp:ListItem>
                                                        <asp:ListItem Text="<%$ Resources:Attendance,Expected Delivery Date %>" Value="ExpDelDate"></asp:ListItem>
                                                        <asp:ListItem Text="<%$ Resources:Attendance,Status %>" Value="Status"></asp:ListItem>
                                                        <asp:ListItem Text="<%$ Resources:Attendance,Description %>" Value="TermCondition"></asp:ListItem>
                                                        <asp:ListItem Text="<%$ Resources:Attendance,Created By %>" Value="UserName"></asp:ListItem>
                                                        <asp:ListItem Text="<%$ Resources:Attendance,Modified By %>" Value="Modified_User"></asp:ListItem>
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
                                                        <asp:TextBox ID="txtValue" runat="server" CssClass="form-control" placeholder="Search From Content"></asp:TextBox>
                                                        <asp:TextBox ID="txtValueDate" runat="server" Visible="false" CssClass="form-control" placeholder="Search From Date"></asp:TextBox>
                                                        <cc1:CalendarExtender OnClientShown="showCalendar" ID="CalendartxtValueDate" runat="server" TargetControlID="txtValueDate" />
                                                    </asp:Panel>
                                                </div>
                                                <div class="col-lg-2">
                                                    <asp:LinkButton ID="btnbind" runat="server" CausesValidation="False" OnClick="btnbind_Click" ToolTip="<%$ Resources:Attendance,Search %>"><span class="fa fa-search"  style="font-size:25px;"></span></asp:LinkButton>&nbsp;&nbsp;&nbsp;
                                                    <asp:LinkButton ID="btnRefresh" runat="server" CausesValidation="False" OnClick="btnRefresh_Click" ToolTip="<%$ Resources:Attendance,Refresh %>"><span class="fa fa-repeat"  style="font-size:25px;"></span></asp:LinkButton>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                                <div class="box box-warning box-solid" <%= gvPurchaseRequest.Rows.Count>0?"style='display:block'":"style='display:none'"%>>
                                    <div class="box-body">
                                        <div class="row">
                                            <div class="col-md-12">
                                                <div class="flow">
                                                    <asp:HiddenField ID="HDFSort" runat="server" />
                                                    <asp:GridView CssClass="table-striped table-bordered table table-hover" ID="gvPurchaseRequest" PageSize="<%# PageControlCommon.GetPageSize() %>"
                                                        runat="server" AutoGenerateColumns="False" Width="100%" AllowSorting="True" CurrentSortField="RequestDate" CurrentSortDirection="DESC"
                                                        OnSorting="gvPurchaseRequest_Sorting">
                                                        <Columns>

                                                            <asp:TemplateField HeaderText="Action">
                                                                <ItemTemplate>
                                                                    <div class="dropdown" style="position: absolute;">
                                                                        <button class="btn btn-default dropdown-toggle" type="button" data-toggle="dropdown">
                                                                            <i class="fa fa-ellipsis-h" aria-hidden="true"></i>
                                                                        </button>
                                                                        <ul class="dropdown-menu">
                                                                            <%--  <li <%= hdnCanPrint.Value=="true"?"style='display:block'":"style='display:none'" %>>
                                                                                <a onclick="IbtnPrint_Command('<%# Eval("RequestNo") %>')" style="cursor: pointer"><i class="fa fa-print"></i>Print </a>
                                                                            </li>--%>

                                                                            <li <%= hdnCanPrint.Value=="true"?"style='display:block'":"style='display:none'" %>>
                                                                                <a onclick="getReportDataWithLoc('<%# Eval("Trans_Id") %>','<%# Eval("Location_Id") %>')"><i class="fa fa-print" style="cursor: pointer"></i>Report System</a>
                                                                            </li>


                                                                            <li <%= hdnCanView.Value=="true"?"style='display:block'":"style='display:none'" %>>
                                                                                <asp:LinkButton ID="lnkViewDetail" runat="server" CommandArgument='<%# Eval("Trans_Id") %>' CommandName='<%# Eval("Location_Id") %>' OnCommand="lnkViewDetail_Command" CausesValidation="False"><i class="fa fa-eye"></i>View</asp:LinkButton>
                                                                            </li>

                                                                            <li <%= hdnCanEdit.Value=="true"?"style='display:block'":"style='display:none'" %>>
                                                                                <asp:LinkButton ID="btnEdit" runat="server" CommandArgument='<%# Eval("Trans_Id") %>' CommandName='<%# Eval("Location_Id") %>' CausesValidation="False" OnCommand="btnEdit_Command"><i class="fa fa-pencil"></i>Edit </asp:LinkButton>
                                                                            </li>
                                                                            <li <%= hdnCanDelete.Value=="true"?"style='display:block'":"style='display:none'" %>>
                                                                                <asp:LinkButton ID="IbtnDelete" runat="server" CausesValidation="False" CommandArgument='<%# Eval("Trans_Id") %>' OnCommand="IbtnDelete_Command"><i class="fa fa-trash"></i>Delete</asp:LinkButton>
                                                                                <cc1:ConfirmButtonExtender ID="confirm2" runat="server" ConfirmText="<%$ Resources:Attendance,Are you sure you want to delete the record?%>" TargetControlID="IbtnDelete"></cc1:ConfirmButtonExtender>
                                                                            </li>
                                                                        </ul>
                                                                    </div>

                                                                </ItemTemplate>
                                                            </asp:TemplateField>

                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,Request No %>" SortExpression="RequestNo">
                                                                <ItemTemplate>
                                                                    <%# Eval("RequestNo") %>
                                                                </ItemTemplate>
                                                                <ItemStyle />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,Request Date %>" SortExpression="RequestDate">
                                                                <ItemTemplate>
                                                                    <%# Convert.ToDateTime(Eval("RequestDate").ToString()).ToString("dd-MMM-yyyy") %>
                                                                </ItemTemplate>
                                                                <ItemStyle />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,Expected Delivery Date %>"
                                                                SortExpression="ExpDelDate">
                                                                <ItemTemplate>
                                                                    <%# Convert.ToDateTime(Eval("ExpDelDate").ToString()).ToString("dd-MMM-yyyy") %>
                                                                </ItemTemplate>
                                                                <ItemStyle HorizontalAlign="Center" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,Description %>" SortExpression="TermCondition">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblDesc" runat="server" Text='<%# GetDesc(Eval("TermCondition").ToString()) %>'
                                                                        ToolTip='<%# Eval("TermCondition").ToString() %>'></asp:Label>
                                                                </ItemTemplate>
                                                                <ItemStyle />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,Approval Status%>" SortExpression="Status">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblStatus" runat="server" Text=' <%# Eval("Status") %>'></asp:Label>
                                                                </ItemTemplate>
                                                                <ItemStyle />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,Request Status%>" SortExpression="Field3">
                                                                <ItemTemplate>
                                                                    <%# Eval("Field3") %>
                                                                </ItemTemplate>
                                                                <ItemStyle />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,Created By %>" SortExpression="UserName">
                                                                <ItemTemplate>
                                                                    <%#Eval("UserName")%>
                                                                </ItemTemplate>
                                                                <ItemStyle HorizontalAlign="Left" Width="100px" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,Modified By %>" SortExpression="Modified_User">
                                                                <ItemTemplate>
                                                                    <%# Eval("Modified_User") %>
                                                                </ItemTemplate>
                                                                <ItemStyle />
                                                            </asp:TemplateField>
                                                        </Columns>


                                                        <PagerStyle CssClass="pagination-ys" />

                                                    </asp:GridView>
                                                </div>
                                                <div class="col-md-12">
                                                    <asp:HiddenField ID="hdnRequestListIndex" runat="server" Value="1" />
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
                                <asp:AsyncPostBackTrigger ControlID="gvPurchaseRequest" />
                            </Triggers>
                            <ContentTemplate>
                                <asp:HiddenField ID="Hdn_Edit_ID" runat="server" />
                                <div class="row">
                                    <div class="col-md-12">
                                        <div class="box box-primary">
                                            <div class="box-body">
                                                <div class="form-group">
                                                    <div class="col-md-6">
                                                        <asp:Label ID="lblRequestdate" runat="server" Text="<%$ Resources:Attendance,Request Date %>"></asp:Label>
                                                        <a style="color: Red">*</a>
                                                        <asp:RequiredFieldValidator EnableClientScript="true" Style="float: right;" runat="server" ID="RequiredFieldValidator3" ValidationGroup="Save"
                                                            Display="Dynamic" SetFocusOnError="true" ControlToValidate="txtRequestdate" ErrorMessage="<%$ Resources:Attendance,Enter Request Date%>"></asp:RequiredFieldValidator>
                                                        <asp:TextBox ID="txtRequestdate" runat="server" CssClass="form-control" />
                                                        <cc1:CalendarExtender OnClientShown="showCalendar" ID="txtCalenderExtender" runat="server" TargetControlID="txtRequestdate">
                                                        </cc1:CalendarExtender>
                                                        <br />
                                                    </div>
                                                    <div class="col-md-6">
                                                        <asp:Label ID="lblRequestNo" runat="server" Text="<%$ Resources:Attendance,Request No %>"></asp:Label>
                                                        <a style="color: Red">*</a>
                                                        <asp:RequiredFieldValidator EnableClientScript="true" Style="float: right;" runat="server" ID="RequiredFieldValidator1" ValidationGroup="Save"
                                                            Display="Dynamic" SetFocusOnError="true" ControlToValidate="txtlRequestNo" ErrorMessage="<%$ Resources:Attendance,Enter Request No%>"></asp:RequiredFieldValidator>
                                                        <asp:TextBox ID="txtlRequestNo" runat="server" CssClass="form-control" AutoPostBack="True" OnTextChanged="txtlRequestNo_TextChanged"></asp:TextBox>
                                                        <br />
                                                    </div>
                                                    <div class="col-md-6">
                                                        <asp:Label ID="lblExpDelDate" runat="server" Text="<%$ Resources:Attendance,Expected Delivery Date %>"></asp:Label>
                                                        <a style="color: Red">*</a>
                                                        <asp:RequiredFieldValidator EnableClientScript="true" Style="float: right;" runat="server" ID="RequiredFieldValidator2" ValidationGroup="Save"
                                                            Display="Dynamic" SetFocusOnError="true" ControlToValidate="txtExpDelDate" ErrorMessage="<%$ Resources:Attendance,Enter Expected Delivery Date%>"></asp:RequiredFieldValidator>
                                                        <asp:TextBox ID="txtExpDelDate" runat="server" CssClass="form-control" />
                                                        <cc1:CalendarExtender OnClientShown="showCalendar" ID="CalendarExtender1" runat="server" TargetControlID="txtExpDelDate">
                                                        </cc1:CalendarExtender>
                                                        <br />
                                                    </div>
                                                    <div class="col-md-6">
                                                        <br />
                                                        <asp:RadioButton ID="rbtnFormView" Font-Bold="true" runat="server" Text="<%$ Resources:Attendance,Form View%>"
                                                            AutoPostBack="true" GroupName="Product" OnCheckedChanged="rbtnFormView_OnCheckedChanged" />
                                                        <asp:RadioButton ID="rbtnAdvancesearchView" Font-Bold="true" runat="server" Text="<%$ Resources:Attendance,Advance Search View%>" Style="margin-left: 20px;"
                                                            AutoPostBack="true" GroupName="Product" OnCheckedChanged="rbtnFormView_OnCheckedChanged" />
                                                        <br />
                                                    </div>
                                                    <div class="col-md-12" style="text-align: center">
                                                        <asp:Button ID="btnAddProduct" Style="display: none;" runat="server" Text="<%$ Resources:Attendance,Add Product %>"
                                                            CssClass="btn btn-info" OnClick="btnAddProduct_Click" />
                                                        <asp:Button ID="btnAddProductScreen" Visible="false" runat="server"
                                                            Text="<%$ Resources:Attendance,Add Product List %>" CssClass="btn btn-info" OnClick="btnAddProductScreen_Click" />
                                                        <asp:Button ID="btnAddtoList" runat="server" Text="<%$ Resources:Attendance,Fill Your Product %>"
                                                            CssClass="btn btn-info" Visible="false" OnClick="btnAddtoList_Click" />
                                                        <br />
                                                    </div>
                                                    <div id="pnlproductpanel" runat="server" class="col-md-12">
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
                                                                        <asp:Label ID="Label2" runat="server" Text="<%$ Resources:Attendance,Product Id%>" />
                                                                        <a style="color: Red">*</a>
                                                                        <asp:RequiredFieldValidator EnableClientScript="true" Style="float: right;" runat="server" ID="RequiredFieldValidator4" ValidationGroup="Add"
                                                                            Display="Dynamic" SetFocusOnError="true" ControlToValidate="txtProductcode" ErrorMessage="<%$ Resources:Attendance,Enter Product Id%>"></asp:RequiredFieldValidator>
                                                                        <asp:TextBox ID="txtProductcode" runat="server" CssClass="form-control" AutoPostBack="True"
                                                                            OnTextChanged="txtProductCode_TextChanged" BackColor="#eeeeee" />
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
                                                                        <asp:TextBox ID="txtProductName" runat="server" CssClass="form-control" AutoPostBack="True"
                                                                            OnTextChanged="txtProductName_TextChanged" BackColor="#eeeeee" />
                                                                        <cc1:AutoCompleteExtender ID="AutoCompleteExtender1" runat="server" CompletionInterval="100"
                                                                            DelimiterCharacters="" Enabled="True" MinimumPrefixLength="1" ServiceMethod="GetCompletionListProductName"
                                                                            TargetControlID="txtProductName" UseContextKey="True" CompletionListCssClass="autoCompleteList" CompletionListItemCssClass="autoCompleteListItem" CompletionListHighlightedItemCssClass="autoCompleteSelectedListItem" OnClientShown="resetPosition">
                                                                        </cc1:AutoCompleteExtender>
                                                                        <br />
                                                                    </div>
                                                                    <div class="col-md-6">
                                                                        <asp:Label ID="lblUnit" runat="server" Text="<%$ Resources:Attendance,Unit %>" />
                                                                        <a style="color: Red">*</a>
                                                                        <asp:RequiredFieldValidator EnableClientScript="true" Style="float: right;" runat="server" ID="RequiredFieldValidator7" ValidationGroup="Add" Display="Dynamic"
                                                                            SetFocusOnError="true" ControlToValidate="ddlUnit" InitialValue="--Select--" ErrorMessage="<%$ Resources:Attendance,Select Unit %>" />
                                                                        <asp:DropDownList ID="ddlUnit" runat="server" CssClass="form-control" />
                                                                        <asp:TextBox ID="txtUnit" runat="server" CssClass="form-control" Visible="False" />
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
        <asp:RequiredFieldValidator EnableClientScript="true" style="float:right;" runat="server" id="RequiredFieldValidator8" ValidationGroup="Add"
        Display="Dynamic" SetFocusOnError="true" controltovalidate="txtPDesc" errormessage="<%$ Resources:Attendance,Enter Product Description%>"></asp:RequiredFieldValidator>--%>
                                                                        <asp:Panel ID="pnlPDescription" runat="server" Style="max-height: 300px;" CssClass="form-control"
                                                                            BorderColor="#8ca7c1" BackColor="#ffffff" ScrollBars="Vertical" Visible="false">
                                                                            <asp:Literal ID="txtPDescription" runat="server"></asp:Literal>
                                                                        </asp:Panel>
                                                                        <asp:TextBox ID="txtPDesc" runat="server" TextMode="MultiLine"
                                                                            CssClass="form-control"></asp:TextBox>
                                                                        <br />
                                                                    </div>
                                                                    <div class="col-md-12" style="text-align: center">
                                                                        <asp:Button ID="btnProductSave" runat="server" Text="<%$ Resources:Attendance,Add Product %>" ValidationGroup="Add"
                                                                            CssClass="btn btn-primary" Visible="false" OnClick="btnProductSave_Click" />
                                                                        <asp:Button ID="btnProductCancel" runat="server" CssClass="btn btn-primary" Text="<%$ Resources:Attendance,Reset %>"
                                                                            CausesValidation="False" OnClick="btnProductCancel_Click" />
                                                                        <asp:Button ID="btnProductClose" runat="server" CssClass="btn btn-danger" Text="<%$ Resources:Attendance,Close %>"
                                                                            CausesValidation="False" OnClick="btnProductClose_Click" />
                                                                        <asp:HiddenField ID="hidProduct" runat="server" />
                                                                        <br />
                                                                    </div>
                                                                </div>
                                                            </div>
                                                        </div>
                                                        <%--</div>
                                                        </div>--%>
                                                        <br />
                                                    </div>
                                                    <div class="col-md-12">
                                                        <div style="overflow: auto; max-height: 500px">
                                                            <asp:GridView CssClass="table-striped table-bordered table table-hover" ID="gvProductRequest" runat="server" AutoGenerateColumns="False" Width="100%">
                                                                <Columns>
                                                                    <asp:TemplateField HeaderText="<%$ Resources:Attendance,Edit %>">
                                                                        <ItemTemplate>
                                                                            <asp:HiddenField ID="HdnTransId" runat="server" Value='<%# Eval("Trans_Id") %>' />
                                                                            <asp:LinkButton ID="btnEdit" Visible='<%# hdnCanEdit.Value=="true"?true:false%>' runat="server" CommandArgument='<%# Eval("Trans_Id") %>' CausesValidation="False" ToolTip="<%$ Resources:Attendance,Edit %>" OnCommand="btnEdit_Command1"><i class="fa fa-pencil"></i></asp:LinkButton>
                                                                        </ItemTemplate>
                                                                        <ItemStyle HorizontalAlign="Center" />
                                                                        <ItemStyle />
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="<%$ Resources:Attendance,Delete %>">
                                                                        <ItemTemplate>
                                                                            <asp:LinkButton ID="IbtnDelete" Visible='<%# hdnCanDelete.Value=="true"?true:false%>' runat="server" CausesValidation="False" CommandArgument='<%# Eval("Trans_Id") %>' ToolTip="<%$ Resources:Attendance,Delete %>" OnCommand="IbtnDelete_Command1"><i class="fa fa-trash"></i></asp:LinkButton>
                                                                        </ItemTemplate>
                                                                        <ItemStyle HorizontalAlign="Center" />
                                                                        <ItemStyle />
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="<%$ Resources:Attendance,Serial No %>">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblSerialNO" runat="server" Text='<%#Container.DataItemIndex+1 %>'
                                                                                Visible="false"></asp:Label>
                                                                            <asp:Label ID="lblSNo" runat="server" Text='<%#Container.DataItemIndex+1 %>' />
                                                                        </ItemTemplate>
                                                                        <ItemStyle />
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="<%$ Resources:Attendance,Product Id %>">
                                                                        <ItemTemplate>

                                                                            <table width="100%">
                                                                                <tr>
                                                                                    <td colspan="2">
                                                                                        <asp:Label ID="lblproductcode" runat="server" Text='<%#new Inv_ProductMaster(Session["DBConnection"].ToString()).GetProductCodebyProductId(Eval("Product_Id").ToString()) %>'></asp:Label>
                                                                                    </td>
                                                                                </tr>
                                                                                <tr>
                                                                                    <td>
                                                                                        <asp:Label ID="lblstockinfo" runat="server" Text="Stock Qty :" Font-Bold="false"></asp:Label>
                                                                                    </td>
                                                                                    <td>
                                                                                        <asp:LinkButton ID="lnkStockInfo" runat="server" Text='<%#GetProductStock(Eval("Product_Id").ToString()) %>'
                                                                                            Font-Underline="true" ToolTip="View Detail" OnCommand="lnkStockInfo_Command"
                                                                                            ForeColor="Blue" CommandArgument='<%# Eval("Product_Id") %>'></asp:LinkButton>
                                                                                    </td>
                                                                                </tr>
                                                                            </table>


                                                                        </ItemTemplate>
                                                                        <ItemStyle />
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="<%$ Resources:Attendance,Product Name %>">
                                                                        <ItemTemplate>
                                                                            <table width="100%">
                                                                                <tr>
                                                                                    <td>
                                                                                        <asp:HiddenField ID="hdnSuggestedProductName" runat="server" Value='<%# Eval("SuggestedProductName") %>' />
                                                                                        <asp:Label ID="lblPID" Visible="false" runat="server" Text='<%# Eval("Product_Id") %>'></asp:Label>
                                                                                        <asp:Label ID="lblProductId" runat="server" Text='<%# SuggestedProductName(Eval("Product_Id").ToString(),Eval("Trans_Id").ToString()) %>'></asp:Label>
                                                                                    </td>
                                                                                    <td align='right'>
                                                                                        <asp:LinkButton ID="lnkDes" runat="server"><i class="fa fa-search"></i></asp:LinkButton>
                                                                                    </td>
                                                                                </tr>
                                                                            </table>
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
                                                                                                            <asp:Label ID="lblDescription" runat="server" Text='<%# Eval("ProductDescription") %>'></asp:Label>
                                                                                                        </asp:Panel>
                                                                                                    </td>
                                                                                                </tr>
                                                                                            </table>
                                                                                        </td>
                                                                                    </tr>
                                                                                </table>
                                                                            </asp:Panel>
                                                                            <cc1:HoverMenuExtender ID="hme3" runat="Server" TargetControlID="lnkDes" PopupControlID="PopupMenu1"
                                                                                HoverCssClass="popupHover" PopupPosition="Left" OffsetX="0" OffsetY="0" PopDelay="50" />
                                                                        </ItemTemplate>
                                                                        <ItemStyle />
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="<%$ Resources:Attendance,Unit Name %>">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblUID" Visible="false" runat="server" Text='<%# Eval("UnitId") %>'></asp:Label>
                                                                            <asp:Label ID="lblUnit" runat="server" Text='<%#Inv_UnitMaster .GetUnitCode(Eval("UnitId").ToString(),Session["DBConnection"].ToString(),HttpContext.Current.Session["CompId"].ToString()) %>'></asp:Label>
                                                                        </ItemTemplate>
                                                                        <ItemStyle />
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="<%$ Resources:Attendance,Quantity %>">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblReqQty" runat="server" Text='<%# Eval("ReqQty") %>'></asp:Label>
                                                                        </ItemTemplate>
                                                                        <ItemStyle />
                                                                    </asp:TemplateField>
                                                                </Columns>


                                                                <PagerStyle CssClass="pagination-ys" />

                                                            </asp:GridView>
                                                        </div>
                                                        <br />
                                                    </div>
                                                    <div class="col-md-12">
                                                        <asp:Label ID="lblTermCondition" runat="server" Text="<%$ Resources:Attendance,Description %>"></asp:Label>
                                                        <a style="color: Red">*</a>
                                                        <asp:RequiredFieldValidator EnableClientScript="true" Style="float: right;" runat="server" ID="RequiredFieldValidator9" ValidationGroup="Save"
                                                            Display="Dynamic" SetFocusOnError="true" ControlToValidate="txtTermCondition" ErrorMessage="<%$ Resources:Attendance,Enter Description%>"></asp:RequiredFieldValidator>
                                                        <asp:TextBox ID="txtTermCondition" runat="server" CssClass="form-control" TextMode="MultiLine" />
                                                        <br />
                                                    </div>
                                                    <div class="col-md-12" style="text-align: center">
                                                        <asp:Button ID="btnSave" runat="server" Text="<%$ Resources:Attendance,Save %>" CssClass="btn btn-success"
                                                            OnClick="btnSave_Click" ValidationGroup="Save" Visible="false" />
                                                        <asp:Button ID="btnReset" runat="server" Text="<%$ Resources:Attendance,Reset %>"
                                                            CssClass="btn btn-primary" OnClick="btnReset_Click" CausesValidation="False" />
                                                        <asp:Button ID="btnCancel" runat="server" Text="<%$ Resources:Attendance,Cancel %>"
                                                            CssClass="btn btn-danger" OnClick="btnCancel_Click" CausesValidation="False" />
                                                        <asp:Button ID="btnSavePrint" runat="server" Text="<%$ Resources:Attendance,Save & Print %>"
                                                            CssClass="btn btn-primary" Visible="false" OnClick="btnSavePrint_Click" />
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
                        <asp:UpdatePanel ID="Update_Bin" runat="server">
                            <ContentTemplate>

                                <div class="row">
                                    <div class="col-md-12">
                                        <div id="Div2" runat="server" class="box box-info collapsed-box">
                                            <div class="box-header with-border">
                                                <h3 class="box-title">
                                                    <asp:Label ID="Label3" runat="server" Text="Advance Search"></asp:Label></h3>
                                                &nbsp;&nbsp;|&nbsp;&nbsp;
				  <asp:Label ID="lblbinTotalRecords" runat="server" Font-Bold="true" Text="<%$ Resources:Attendance,Total Records: 0 %>"></asp:Label>

                                                <div class="box-tools pull-right">
                                                    <button type="button" class="btn btn-box-tool" data-widget="collapse">
                                                        <i id="I2" runat="server" class="fa fa-plus"></i>
                                                    </button>
                                                </div>
                                            </div>
                                            <div class="box-body">

                                                <div class="col-lg-3">
                                                    <asp:DropDownList ID="ddlbinFieldName" runat="server" CssClass="form-control" AutoPostBack="true" OnSelectedIndexChanged="ddlFieldNameBin_SelectedIndexChanged">
                                                        <asp:ListItem Selected="True" Text="<%$ Resources:Attendance,Request No %>" Value="RequestNo"></asp:ListItem>
                                                        <asp:ListItem Text="<%$ Resources:Attendance,Request Date %>" Value="RequestDate"></asp:ListItem>
                                                        <asp:ListItem Text="<%$ Resources:Attendance,Expected Delivery Date %>" Value="ExpDelDate"></asp:ListItem>
                                                        <asp:ListItem Text="<%$ Resources:Attendance,Status %>" Value="Status"></asp:ListItem>
                                                        <asp:ListItem Text="<%$ Resources:Attendance,Created By %>" Value="UserName"></asp:ListItem>
                                                        <asp:ListItem Text="<%$ Resources:Attendance,Modified By %>" Value="Modified_User"></asp:ListItem>
                                                    </asp:DropDownList>
                                                </div>
                                                <div class="col-lg-2">
                                                    <asp:DropDownList ID="ddlbinOption" runat="server" CssClass="form-control">
                                                        <asp:ListItem Text="<%$ Resources:Attendance,--Select-- %>"></asp:ListItem>
                                                        <asp:ListItem Text="<%$ Resources:Attendance,Equal %>"></asp:ListItem>
                                                        <asp:ListItem Selected="True" Text="<%$ Resources:Attendance,Contains %>"></asp:ListItem>
                                                        <asp:ListItem Text="<%$ Resources:Attendance,Like %>"></asp:ListItem>
                                                    </asp:DropDownList>
                                                </div>
                                                <div class="col-lg-4">
                                                    <asp:Panel ID="Panel2" runat="server" DefaultButton="btnbinbind">
                                                        <asp:TextBox ID="txtbinValue" runat="server" CssClass="form-control" placeholder="Search From Content"></asp:TextBox>
                                                        <asp:TextBox ID="txtValueBinDate" runat="server" CssClass="form-control" Visible="false" placeholder="Search From Date"></asp:TextBox>
                                                        <cc1:CalendarExtender OnClientShown="showCalendar" ID="CalendarExtendertxtValueBinDate" runat="server" TargetControlID="txtValueBinDate" />
                                                    </asp:Panel>
                                                </div>
                                                <div class="col-lg-3">
                                                    <asp:LinkButton ID="btnbinbind" runat="server" CausesValidation="False" OnClick="btnbinbind_Click" ToolTip="<%$ Resources:Attendance,Search %>"><span class="fa fa-search"  style="font-size:25px;"></span></asp:LinkButton>&nbsp;&nbsp;&nbsp;
                                                    <asp:LinkButton ID="btnbinRefresh" runat="server" CausesValidation="False" OnClick="btnbinRefresh_Click" ToolTip="<%$ Resources:Attendance,Refresh %>"><span class="fa fa-repeat"  style="font-size:25px;"></span></asp:LinkButton>&nbsp;&nbsp;&nbsp;
                                                    <asp:LinkButton ID="imgBtnRestore" CausesValidation="False" runat="server" OnClick="imgBtnRestore_Click" ToolTip="<%$ Resources:Attendance, Active %>"><span class="far fa-lightbulb"  style="font-size:25px;"></span></asp:LinkButton>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>

                                <div class="box box-warning box-solid" <%= gvBinPurchaseRequest.Rows.Count>0?"style='display:block'":"style='display:none'"%>>
                                    <div class="box-body">
                                        <div class="row">
                                            <div class="col-md-12">
                                                <div class="flow">
                                                    <asp:GridView CssClass="table-striped table-bordered table table-hover" ID="gvBinPurchaseRequest" PageSize="<%# PageControlCommon.GetPageSize() %>"
                                                        runat="server" AutoGenerateColumns="False" Width="100%" AllowPaging="True" AllowSorting="True"
                                                        OnPageIndexChanging="gvBinPurchaseRequest_PageIndexChanging"
                                                        OnSorting="gvBinPurchaseRequest_Sorting">
                                                        <Columns>
                                                            <asp:TemplateField>
                                                                <ItemTemplate>
                                                                    <asp:CheckBox ID="chkgvSelect" runat="server" OnCheckedChanged="chkgvSelect_CheckedChanged"
                                                                        AutoPostBack="true" />
                                                                </ItemTemplate>
                                                                <HeaderTemplate>
                                                                    <asp:CheckBox ID="chkgvSelectAll" runat="server" OnCheckedChanged="chkgvSelectAll_CheckedChanged"
                                                                        AutoPostBack="true" />
                                                                </HeaderTemplate>
                                                                <ItemStyle HorizontalAlign="Center" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,Request No %>" SortExpression="RequestNo">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblRequestNo" runat="server" Text='<%# Eval("RequestNo") %>'></asp:Label>
                                                                    <asp:Label ID="lblReqId" Visible="false" runat="server" Text='<%# Eval("Trans_Id") %>'></asp:Label>
                                                                </ItemTemplate>
                                                                <ItemStyle />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,Request Date %>" SortExpression="RequestDate">
                                                                <ItemTemplate>
                                                                    <%# Convert.ToDateTime(Eval("RequestDate").ToString()).ToString("dd-MMM-yyyy") %>
                                                                </ItemTemplate>
                                                                <ItemStyle />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,Expected Delivery Date %>"
                                                                SortExpression="ExpDelDate">
                                                                <ItemTemplate>
                                                                    <%# Convert.ToDateTime(Eval("ExpDelDate").ToString()).ToString("dd-MMM-yyyy") %>
                                                                </ItemTemplate>
                                                                <ItemStyle />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,User %>" SortExpression="UserName">
                                                                <ItemTemplate>
                                                                    <%#Eval("UserName") %>
                                                                </ItemTemplate>
                                                                <ItemStyle HorizontalAlign="Center" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,Description %>" SortExpression="ExpDelDate">
                                                                <ItemTemplate>
                                                                    <%# GetDesc(Eval("TermCondition").ToString()) %>
                                                                </ItemTemplate>
                                                                <ItemStyle />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,Status %>" SortExpression="Status">
                                                                <ItemTemplate>
                                                                    <%# Eval("Status") %>
                                                                </ItemTemplate>
                                                                <ItemStyle />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,Created By %>" SortExpression="UserName">
                                                                <ItemTemplate>
                                                                    <%#Eval("UserName")%>
                                                                </ItemTemplate>
                                                                <ItemStyle HorizontalAlign="Left" Width="100px" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,Modified By %>" SortExpression="Modified_User">
                                                                <ItemTemplate>
                                                                    <%# Eval("Modified_User") %>
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
        function Li_Tab_Bin() {
            document.getElementById('<%= Btn_Bin.ClientID %>').click();
        }
        function View_Modal_Popup() {
            document.getElementById('<%= Btn_Purchase_Request_Modal.ClientID %>').click();
        }
        function Close_Modal_Popup() {
            document.getElementById('<%= Btn_Purchase_Request_Modal.ClientID %>').click();
        }
        function IbtnPrint_Command(requistPrint) {
            window.open('../Purchase/PurchaseRequestPrint.aspx?RId=' + requistPrint + '', 'window', 'width=1024');
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

        function getReportData(transId) {
            $("#prgBar").css("display", "block");
            document.getElementById('<%= reportSystem.FindControl("hdnTransId").ClientID %>').value = transId;
            setReportData();
        }

        function getReportDataWithLoc(transId, locId) {
            $("#prgBar").css("display", "block");
            document.getElementById('<%= reportSystem.FindControl("hdnTransId").ClientID %>').value = transId;
            document.getElementById('<%= reportSystem.FindControl("hdnLocId").ClientID %>').value = locId;
               setReportData();
           }

    </script>
    <script src="../Script/ReportSystem.js"></script>
    <script src="../Script/master.js"></script>
</asp:Content>
