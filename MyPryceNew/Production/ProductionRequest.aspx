<%@ Page Title="" Language="C#" MasterPageFile="~/ERPMaster.master" AutoEventWireup="true" CodeFile="ProductionRequest.aspx.cs" Inherits="Production_ProductionRequest" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="Head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="HeaderContent" runat="Server">
    <h1>
        <i class="fas fa-file-signature"></i>&nbsp;&nbsp;
        <asp:Label ID="lblHeader" runat="server" Text="<%$ Resources:Attendance,Production Request%>"></asp:Label>
    </h1>
    <ol class="breadcrumb">
        <li><a href="#"><i class="fa fa-dashboard"></i>
            <asp:Label ID="Label6" runat="server" Text="<%$ Resources:Attendance,Inventory%>"></asp:Label></a></li>
        <li><a href="#"><i class="fa fa-dashboard"></i>
            <asp:Label ID="Label10" runat="server" Text="<%$ Resources:Attendance,Production%>"></asp:Label></a></li>
        <li class="active">
            <asp:Label ID="Label5" runat="server" Text="<%$ Resources:Attendance,Production Request%>"></asp:Label></li>
    </ol>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="Server">
    <asp:UpdatePanel ID="Update_Button" runat="server">
        <ContentTemplate>
            <asp:Button ID="Btn_Bin" Style="display: none;" runat="server" OnClick="btnBin_Click" Text="Bin" />
            <asp:Button ID="Btn_Sales_Order" Style="display: none;" runat="server" OnClick="btnOrder_Click" Text="Sales Order" />
            <asp:Button ID="Btn_Purchase_Request_Modal" Style="display: none;" runat="server" data-toggle="modal" data-target="#Purchase_Request_Modal" Text="Purchase Request Modal" />
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
                    <li style="display: none" id="Li_Report"><a href="#Report" data-toggle="tab">
                        <img src="../Images/report.png" style="width: 25px;" alt="" /><asp:Label ID="Label3" runat="server" Text="<%$ Resources:Attendance,Report %>"></asp:Label></a></li>
                    <li id="Li_Sales_Order"><a href="#Sales_Order" onclick="Li_Tab_Sales_Order()" data-toggle="tab">
                        <i class="far fa-file-alt"></i>&nbsp;&nbsp;
                        <asp:Label ID="Label2" runat="server" Text="<%$ Resources:Attendance,Sales Order %>"></asp:Label></a></li>
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
                        <asp:UpdatePanel ID="Update_List" runat="server">
                            <ContentTemplate>



                                <div class="row">
                                    <div class="col-md-12">
                                        <div id="Div1" runat="server" class="box box-info collapsed-box">
                                            <div class="box-header with-border">
                                                <h3 class="box-title">
                                                    <asp:Label ID="Label22" runat="server" Text="Advance Search"></asp:Label></h3>
                                                &nbsp;&nbsp;|&nbsp;&nbsp;
				<asp:Label ID="lblTotalRecords" Font-Bold="true" runat="server" Text="<%$ Resources:Attendance,Total Records: 0 %>"></asp:Label>
                                                <asp:Label ID="lblSelectedRecord" runat="server" Visible="false"></asp:Label>

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
                                                        <asp:ListItem Selected="True" Text="<%$ Resources:Attendance,Request No %>" Value="Request_No"></asp:ListItem>
                                                        <asp:ListItem Text="<%$ Resources:Attendance,Request Date %>" Value="Request_Date"></asp:ListItem>
                                                        <asp:ListItem Text="<%$ Resources:Attendance,Order No. %>" Value="Order_No"></asp:ListItem>
                                                        <asp:ListItem Text="<%$ Resources:Attendance,Order Date %>" Value="Order_Date"></asp:ListItem>
                                                        <asp:ListItem Text="<%$ Resources:Attendance,Customer Name %>" Value="Customername"></asp:ListItem>
                                                        <asp:ListItem Text="<%$ Resources:Attendance,Sales Person %>" Value="SalesPersonName"></asp:ListItem>
                                                        <asp:ListItem Text="Is Cancel" Value="Is_Cancel"></asp:ListItem>
                                                        <asp:ListItem Text="From Location" Value="From_Location_Name"></asp:ListItem>
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
                                                        <asp:TextBox ID="txtValueDate" runat="server" CssClass="form-control" placeholder="Search From Date"></asp:TextBox>
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
                                                <div style="overflow: auto">
                                                    <asp:HiddenField ID="HDFSort" runat="server" />

                                                    <asp:GridView CssClass="table-striped table-bordered table table-hover" ID="gvPurchaseRequest" PageSize="<%# PageControlCommon.GetPageSize() %>"
                                                        runat="server" AutoGenerateColumns="False" Width="100%" AllowPaging="True"
                                                        AllowSorting="True" OnPageIndexChanging="gvPurchaseRequest_PageIndexChanging"
                                                        OnSorting="gvPurchaseRequest_Sorting">
                                                        <Columns>
                                                            <asp:TemplateField HeaderText="Action">
                                                                <ItemTemplate>
                                                                    <div class="dropdown" style="position: absolute;">
                                                                        <button class="btn btn-default dropdown-toggle" type="button" data-toggle="dropdown">
                                                                            <i class="fa fa-ellipsis-h" aria-hidden="true"></i>
                                                                        </button>
                                                                        <ul class="dropdown-menu">

                                                                            <li <%= hdnCanView.Value=="true"?"style='display:block'":"style='display:none'" %>>
                                                                                <asp:LinkButton ID="lnkViewDetail" runat="server" CommandArgument='<%# Eval("Trans_Id") %>' OnCommand="lnkViewDetail_Command" CausesValidation="False"><i class="fa fa-eye"></i>View</asp:LinkButton>
                                                                            </li>

                                                                            <li <%= hdnCanEdit.Value=="true"?"style='display:block'":"style='display:none'" %>>
                                                                                <asp:LinkButton ID="btnEdit" runat="server" CommandArgument='<%# Eval("Trans_Id") %>' CausesValidation="False" OnCommand="btnEdit_Command"><i class="fa fa-pencil"></i>Edit </asp:LinkButton>
                                                                            </li>
                                                                            <li <%= hdnCanDelete.Value=="true"?"style='display:block'":"style='display:none'" %>>
                                                                                <asp:LinkButton ID="IbtnDelete" runat="server" CausesValidation="False" CommandArgument='<%# Eval("Trans_Id") %>' CommandName='<%# Eval("Is_Post") %>' OnCommand="IbtnDelete_Command"><i class="fa fa-trash"></i>Delete</asp:LinkButton>
                                                                                <cc1:ConfirmButtonExtender ID="confirm1" runat="server" ConfirmText="<%$ Resources:Attendance,Are you sure you want to delete the record?%>" TargetControlID="IbtnDelete"></cc1:ConfirmButtonExtender>
                                                                            </li>

                                                                        </ul>
                                                                    </div>

                                                                </ItemTemplate>
                                                            </asp:TemplateField>


                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,Request No %>" SortExpression="Request_No">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblRequestNo" runat="server" Text='<%# Eval("Request_No") %>'></asp:Label>
                                                                </ItemTemplate>
                                                                <ItemStyle />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,Request Date %>" SortExpression="Request_Date">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblRequestDate" runat="server" Text='<%# GetDate(Eval("Request_Date").ToString()) %>'></asp:Label>
                                                                </ItemTemplate>
                                                                <ItemStyle />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,Order No. %>" SortExpression="Order_No">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblorderno" runat="server" Text='<%# Eval("Order_No") %>'></asp:Label>
                                                                </ItemTemplate>
                                                                <ItemStyle />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,Order Date %>" SortExpression="Order_Date">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblExpDelDate" runat="server" Text='<%# GetDate(Eval("Order_Date").ToString()) %>'></asp:Label>
                                                                </ItemTemplate>
                                                                <ItemStyle />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,Customer Name %>" SortExpression="Customername">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblCustomerName" runat="server" Text='<%#Eval("Customername")%>' />
                                                                </ItemTemplate>
                                                                <ItemStyle HorizontalAlign="Center" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,Sales Person %>" SortExpression="SalesPersonName">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblsalesperson" runat="server" Text='<%# Eval("SalesPersonName") %>' />
                                                                </ItemTemplate>
                                                                <ItemStyle HorizontalAlign="Center" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Is Process" SortExpression="Is_Production_Process">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblisProductionProcess" runat="server" Text='<%# Eval("Is_Production_Process") %>' />
                                                                </ItemTemplate>
                                                                <ItemStyle HorizontalAlign="Center" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Is Finish" SortExpression="Is_Production_Finish">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblisProductionfinish" runat="server" Text='<%# Eval("Is_Production_Finish") %>' />
                                                                </ItemTemplate>
                                                                <ItemStyle HorizontalAlign="Center" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="From Location" SortExpression="From_Location_Name">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblFromLocation" runat="server" Text='<%# Eval("From_Location_Name").ToString() %>' />
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
                                                    <div class="col-md-6">
                                                        <asp:Label ID="lblRequestdate" runat="server" Text="<%$ Resources:Attendance,Request Date %>"></asp:Label>
                                                        <a style="color: Red">*</a>
                                                        <asp:RequiredFieldValidator EnableClientScript="true" Style="float: right;" runat="server" ID="RequiredFieldValidator3" ValidationGroup="Save"
                                                            Display="Dynamic" SetFocusOnError="true" ControlToValidate="txtRequestdate" ErrorMessage="<%$ Resources:Attendance,Enter Request Date %>"></asp:RequiredFieldValidator>

                                                        <asp:TextBox ID="txtRequestdate" runat="server" CssClass="form-control" />
                                                        <cc1:CalendarExtender OnClientShown="showCalendar" ID="txtCalenderExtender" runat="server" TargetControlID="txtRequestdate">
                                                        </cc1:CalendarExtender>
                                                        <br />
                                                    </div>
                                                    <div class="col-md-6">
                                                        <asp:Label ID="lblRequestNo" runat="server" Text="<%$ Resources:Attendance,Request No %>"></asp:Label>
                                                        <a style="color: Red">*</a>
                                                        <asp:RequiredFieldValidator EnableClientScript="true" Style="float: right;" runat="server" ID="RequiredFieldValidator1" ValidationGroup="Save"
                                                            Display="Dynamic" SetFocusOnError="true" ControlToValidate="txtlRequestNo" ErrorMessage="<%$ Resources:Attendance,Enter Request No %>"></asp:RequiredFieldValidator>

                                                        <asp:TextBox ID="txtlRequestNo" runat="server" CssClass="form-control" AutoPostBack="True"
                                                            OnTextChanged="txtlRequestNo_TextChanged" />
                                                        <br />
                                                    </div>
                                                    <div class="col-md-6">
                                                        <asp:Label ID="lblCustomerName" runat="server" Text="<%$ Resources:Attendance,Customer Name %>" />
                                                        <a style="color: Red">*</a>
                                                        <asp:RequiredFieldValidator EnableClientScript="true" Style="float: right;" runat="server" ID="RequiredFieldValidator2" ValidationGroup="Save"
                                                            Display="Dynamic" SetFocusOnError="true" ControlToValidate="txtCustomer" ErrorMessage="<%$ Resources:Attendance,Enter Customer Name%>"></asp:RequiredFieldValidator>

                                                        <asp:TextBox ID="txtCustomer" runat="server" CssClass="form-control" OnTextChanged="txtCustomer_TextChanged"
                                                            BackColor="#eeeeee" AutoPostBack="true" />
                                                        <cc1:AutoCompleteExtender ID="txtCustomerName_AutoCompleteExtender" runat="server"
                                                            DelimiterCharacters="" Enabled="True" CompletionInterval="100" CompletionSetCount="1"
                                                            MinimumPrefixLength="1" ServiceMethod="GetCompletionListCustomer" ServicePath=""
                                                            TargetControlID="txtCustomer" UseContextKey="True" CompletionListCssClass="autoCompleteList" CompletionListItemCssClass="autoCompleteListItem" CompletionListHighlightedItemCssClass="autoCompleteSelectedListItem" OnClientShown="resetPosition">
                                                        </cc1:AutoCompleteExtender>
                                                        <br />
                                                    </div>
                                                    <div class="col-md-6">
                                                        <asp:Label ID="lblSONo" runat="server" Text="<%$ Resources:Attendance,Order No. %>"></asp:Label>
                                                        <asp:TextBox ID="txtSONo" runat="server" CssClass="form-control" Enabled="false" />
                                                        <br />
                                                    </div>
                                                    <div class="col-md-6">
                                                        <asp:Label ID="lblSODate" runat="server" Text="<%$ Resources:Attendance,Order Date %>"></asp:Label>
                                                        <asp:TextBox ID="txtSODate" runat="server" CssClass="form-control" Enabled="false" />
                                                        <cc1:CalendarExtender OnClientShown="showCalendar" ID="CalendarExtender1" runat="server" TargetControlID="txtSODate" />
                                                        <br />
                                                    </div>
                                                    <div class="col-md-6">
                                                        <asp:Label ID="lblSalesPerson" runat="server" Text="<%$ Resources:Attendance,Sales Person%>" />
                                                        <asp:TextBox ID="txtSalesPerson" runat="server" CssClass="form-control" BackColor="#eeeeee"
                                                            OnTextChanged="txtSalesPerson_TextChanged" AutoPostBack="true" />
                                                        <cc1:AutoCompleteExtender ID="txtSalesPerson_AutoCompleteExtender" runat="server"
                                                            DelimiterCharacters="" Enabled="True" CompletionInterval="100" CompletionSetCount="1"
                                                            MinimumPrefixLength="1" ServiceMethod="GetCompletionListEmployeeName" ServicePath=""
                                                            TargetControlID="txtSalesPerson" UseContextKey="True" CompletionListCssClass="autoCompleteList" CompletionListItemCssClass="autoCompleteListItem" CompletionListHighlightedItemCssClass="autoCompleteSelectedListItem" OnClientShown="resetPosition">
                                                        </cc1:AutoCompleteExtender>
                                                        <br />
                                                    </div>
                                                    <div class="col-md-6">
                                                        <asp:Label ID="Label16" runat="server" Text="<%$ Resources:Attendance,Location Name %>"></asp:Label>
                                                        <asp:DropDownList ID="ddlLocation" runat="server" CssClass="form-control"></asp:DropDownList>
                                                        <br />
                                                    </div>
                                                    <div id="pnlProduct1" runat="server" class="col-md-12">
                                                        <div class="row">
                                                            <div class="col-md-12">
                                                                <div class="box box-primary">
                                                                    <div class="box-header with-border">
                                                                        <h3 class="box-title">
                                                                            <asp:Label ID="lbladdproduct" runat="server" Text="<%$ Resources:Attendance,Add Product %>"></asp:Label>
                                                                        </h3>
                                                                        <div class="box-tools pull-right">
                                                                            <button type="button" class="btn btn-box-tool" data-widget="collapse">
                                                                                <i class="fa fa-minus"></i>
                                                                            </button>
                                                                        </div>
                                                                    </div>
                                                                    <div class="box-body">
                                                                        <div class="form-group">
                                                                            <div class="col-md-6">
                                                                                <asp:Label ID="Label4" runat="server" Text="<%$ Resources:Attendance,Product Id%>" />
                                                                                <a style="color: Red">*</a>
                                                                                <asp:RequiredFieldValidator EnableClientScript="true" Style="float: right;" runat="server" ID="RequiredFieldValidator4" ValidationGroup="Save"
                                                                                    Display="Dynamic" SetFocusOnError="true" ControlToValidate="txtProductcode" ErrorMessage="<%$ Resources:Attendance,Enter Product Id%>"></asp:RequiredFieldValidator>

                                                                                <asp:TextBox ID="txtProductcode" runat="server" CssClass="form-control" AutoPostBack="True"
                                                                                    OnTextChanged="txtProductCode_TextChanged" BackColor="#eeeeee" />
                                                                                <cc1:AutoCompleteExtender ID="AutoCompleteExtender2" runat="server" CompletionInterval="100"
                                                                                    DelimiterCharacters="" Enabled="True" MinimumPrefixLength="1" ServiceMethod="GetCompletionListProductCode"
                                                                                    ServicePath="" TargetControlID="txtProductcode" UseContextKey="True" CompletionListCssClass="autoCompleteList" CompletionListItemCssClass="autoCompleteListItem" CompletionListHighlightedItemCssClass="autoCompleteSelectedListItem" OnClientShown="resetPosition">
                                                                                </cc1:AutoCompleteExtender>
                                                                                <br />
                                                                            </div>
                                                                            <div class="col-md-6">
                                                                                <asp:Label ID="lblProductName" runat="server" Text="<%$ Resources:Attendance,Product Name %>" />
                                                                                <a style="color: Red">*</a>
                                                                                <asp:RequiredFieldValidator EnableClientScript="true" Style="float: right;" runat="server" ID="RequiredFieldValidator5" ValidationGroup="Save"
                                                                                    Display="Dynamic" SetFocusOnError="true" ControlToValidate="txtProductName" ErrorMessage="<%$ Resources:Attendance,Enter Product Name%>"></asp:RequiredFieldValidator>

                                                                                <asp:TextBox ID="txtProductName" runat="server" CssClass="form-control" AutoPostBack="True"
                                                                                    OnTextChanged="txtProductName_TextChanged" BackColor="#eeeeee" />
                                                                                <cc1:AutoCompleteExtender ID="AutoCompleteExtender1" runat="server" CompletionInterval="100"
                                                                                    DelimiterCharacters="" Enabled="True" MinimumPrefixLength="1" ServiceMethod="GetCompletionListProductName"
                                                                                    ServicePath="" TargetControlID="txtProductName" UseContextKey="True" CompletionListCssClass="autoCompleteList" CompletionListItemCssClass="autoCompleteListItem" CompletionListHighlightedItemCssClass="autoCompleteSelectedListItem" OnClientShown="resetPosition">
                                                                                </cc1:AutoCompleteExtender>
                                                                                <br />
                                                                            </div>
                                                                            <div class="col-md-6">
                                                                                <asp:Label ID="lblUnit" runat="server" Text="<%$ Resources:Attendance,Unit %>" />
                                                                                <a style="color: Red">*</a>
                                                                                <asp:RequiredFieldValidator EnableClientScript="true" Style="float: right;" runat="server" ID="RequiredFieldValidator6" ValidationGroup="Save" Display="Dynamic"
                                                                                    SetFocusOnError="true" ControlToValidate="ddlUnit" InitialValue="--Select--" ErrorMessage="<%$ Resources:Attendance,Select Unit %>" />

                                                                                <asp:DropDownList ID="ddlUnit" runat="server" CssClass="form-control" />
                                                                                <asp:TextBox ID="txtUnit" runat="server" CssClass="form-control" Visible="False" />
                                                                                <br />
                                                                            </div>
                                                                            <div class="col-md-6">
                                                                                <asp:Label ID="lblRequestQty" runat="server" Text="<%$ Resources:Attendance,Request Quantity %>" />
                                                                                <a style="color: Red">*</a>
                                                                                <asp:RequiredFieldValidator EnableClientScript="true" Style="float: right;" runat="server" ID="RequiredFieldValidator7" ValidationGroup="Save"
                                                                                    Display="Dynamic" SetFocusOnError="true" ControlToValidate="txtRequestQty" ErrorMessage="<%$ Resources:Attendance,Enter Request Quantity%>"></asp:RequiredFieldValidator>

                                                                                <asp:TextBox ID="txtRequestQty" runat="server" CssClass="form-control" Text="1" />
                                                                                <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" runat="server" Enabled="True"
                                                                                    TargetControlID="txtRequestQty" ValidChars="1,2,3,4,5,6,7,8,9,0,.">
                                                                                </cc1:FilteredTextBoxExtender>
                                                                                <br />
                                                                            </div>
                                                                            <div class="col-md-6">
                                                                                <asp:Label ID="Label7" runat="server" Text="<%$ Resources:Attendance,Unit Price %>" />
                                                                                <a style="color: Red">*</a>
                                                                                <asp:RequiredFieldValidator EnableClientScript="true" Style="float: right;" runat="server" ID="RequiredFieldValidator8" ValidationGroup="Save"
                                                                                    Display="Dynamic" SetFocusOnError="true" ControlToValidate="txtUnitPrice" ErrorMessage="<%$ Resources:Attendance,Enter Unit Price%>"></asp:RequiredFieldValidator>

                                                                                <asp:TextBox ID="txtUnitPrice" runat="server" CssClass="form-control" Text="1" />
                                                                                <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender2" runat="server" Enabled="True"
                                                                                    TargetControlID="txtUnitPrice" ValidChars="1,2,3,4,5,6,7,8,9,0,.">
                                                                                </cc1:FilteredTextBoxExtender>
                                                                                <br />
                                                                            </div>
                                                                            <div class="col-md-12">
                                                                                <asp:Label ID="lblPDescription" runat="server" Text="<%$ Resources:Attendance,Product Description %>" />
                                                                                <a style="color: Red">*</a>


                                                                                <asp:Panel ID="pnlPDescription" runat="server" CssClass="form-control"
                                                                                    BorderColor="#8ca7c1" BackColor="#ffffff" ScrollBars="Vertical" Visible="false">
                                                                                    <asp:Literal ID="txtPDescription" runat="server"></asp:Literal>
                                                                                </asp:Panel>
                                                                                <asp:TextBox ID="txtPDesc" runat="server" TextMode="MultiLine"
                                                                                    CssClass="form-control" Font-Names="Arial"></asp:TextBox>
                                                                                <br />
                                                                            </div>
                                                                            <div class="col-md-12" style="text-align: center">
                                                                                <asp:Button ID="btnProductSave" ValidationGroup="Save" runat="server" Text="<%$ Resources:Attendance,Add %>"
                                                                                    CssClass="btn btn-primary" Visible="false" OnClick="btnProductSave_Click" />

                                                                                <asp:Button ID="btnProductCancel" runat="server" CssClass="btn btn-primary" Text="<%$ Resources:Attendance,Reset %>"
                                                                                    CausesValidation="False" OnClick="btnProductCancel_Click" />

                                                                                <asp:Button ID="btnProductClose" runat="server" CssClass="btn btn-primary" Text="<%$ Resources:Attendance,Close %>"
                                                                                    CausesValidation="False" OnClick="btnProductClose_Click" />
                                                                                <asp:HiddenField ID="hidProduct" runat="server" />
                                                                                <br />
                                                                            </div>
                                                                        </div>
                                                                    </div>
                                                                </div>
                                                            </div>
                                                        </div>
                                                        <br />
                                                    </div>

                                                    <div class="col-md-12">
                                                        <div style="overflow: auto">
                                                            <asp:GridView CssClass="table-striped table-bordered table table-hover" ID="gvProductRequest" runat="server" AutoGenerateColumns="False" Width="100%">
                                                                <Columns>
                                                                    <asp:TemplateField HeaderText="<%$ Resources:Attendance,Edit %>" Visible="false">
                                                                        <ItemTemplate>
                                                                            <asp:HiddenField ID="HdnTransId" runat="server" Value='<%# Eval("Trans_Id") %>' />
                                                                            <asp:LinkButton ID="btnEdit" runat="server" CommandArgument='<%# Eval("Trans_Id") %>' CausesValidation="False" ToolTip="<%$ Resources:Attendance,Edit %>" OnCommand="btnEdit_Command1"><i class="fa fa-pencil" style="font-size:15px"></i></asp:LinkButton>
                                                                        </ItemTemplate>
                                                                        <ItemStyle HorizontalAlign="Center" />
                                                                        <ItemStyle />
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="<%$ Resources:Attendance,Delete %>" Visible="false">
                                                                        <ItemTemplate>
                                                                            <asp:LinkButton ID="IbtnDelete" runat="server" CausesValidation="False" CommandArgument='<%# Eval("Trans_Id") %>' ToolTip="<%$ Resources:Attendance,Delete %>" OnCommand="IbtnDelete_Command1"><i class="fa fa-trash" style="font-size:15px"></i></asp:LinkButton>
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
                                                                            <asp:Label ID="lblproductcode" runat="server" Text='<%# ProductCode(Eval("ProductId").ToString()) %>'></asp:Label>
                                                                        </ItemTemplate>
                                                                        <ItemStyle />
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="<%$ Resources:Attendance,Product Name %>">
                                                                        <ItemTemplate>
                                                                            <table width="100%">
                                                                                <tr>
                                                                                    <td>
                                                                                        <asp:Label ID="lblPID" Visible="false" runat="server" Text='<%# Eval("ProductId") %>'></asp:Label>
                                                                                        <asp:Label ID="lblProductId" runat="server" Text='<%# ProductName(Eval("ProductId").ToString())%>'></asp:Label>
                                                                                    </td>
                                                                                    <td align='Right'>
                                                                                        <asp:LinkButton ID="lnkDes" runat="server" CssClass="fa fa-search" />
                                                                                    </td>
                                                                                </tr>
                                                                            </table>
                                                                            <div id="divpanel" style="display: none; width: 100%;">
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
                                                                                                                <asp:Label ID="lblDescription" runat="server" Text='<%# ProductDescription(Eval("ProductId").ToString()) %>'></asp:Label>
                                                                                                            </asp:Panel>
                                                                                                        </td>
                                                                                                    </tr>
                                                                                                </table>
                                                                                            </td>
                                                                                        </tr>
                                                                                    </table>
                                                                                </asp:Panel>
                                                                            </div>
                                                                            <cc1:HoverMenuExtender ID="hme3" runat="Server" TargetControlID="lnkDes" PopupControlID="PopupMenu1"
                                                                                HoverCssClass="popupHover" PopupPosition="Left" OffsetX="0" OffsetY="0" PopDelay="50" />
                                                                        </ItemTemplate>
                                                                        <ItemStyle />
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="<%$ Resources:Attendance,Unit Name %>">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblUID" Visible="false" runat="server" Text='<%# Eval("UnitId") %>'></asp:Label>
                                                                            <asp:Label ID="lblUnit" runat="server" Text='<%# UnitName(Eval("UnitId").ToString()) %>'></asp:Label>
                                                                        </ItemTemplate>
                                                                        <ItemStyle />
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="<%$ Resources:Attendance,Quantity %>">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblReqQty" runat="server" Text='<%# SetDecimal(Eval("Quantity").ToString()) %>'></asp:Label>
                                                                        </ItemTemplate>
                                                                        <ItemStyle HorizontalAlign="Center" />
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="<%$ Resources:Attendance,Unit Price %>">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblunitPrice" runat="server" Text='<%# SetDecimal(Eval("UnitPrice").ToString()) %>'></asp:Label>
                                                                        </ItemTemplate>
                                                                        <ItemStyle HorizontalAlign="Center" />
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="<%$ Resources:Attendance,Line Total %>">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblLinetotal" runat="server" Text='<%#SetDecimal((Convert.ToDouble(Eval("UnitPrice").ToString())*Convert.ToDouble(Eval("Quantity").ToString())).ToString())%>'></asp:Label>
                                                                        </ItemTemplate>
                                                                        <ItemStyle HorizontalAlign="Center" />
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
                                                        <asp:RequiredFieldValidator EnableClientScript="true" Style="float: right;" runat="server" ID="RequiredFieldValidator10" ValidationGroup="Ssave"
                                                            Display="Dynamic" SetFocusOnError="true" ControlToValidate="txtTermCondition" ErrorMessage="<%$ Resources:Attendance,Enter Description%>"></asp:RequiredFieldValidator>

                                                        <asp:TextBox ID="txtTermCondition" runat="server" CssClass="form-control" TextMode="MultiLine" Font-Names="Arial" />
                                                        <br />
                                                    </div>
                                                    <div class="col-md-6">
                                                        <asp:CheckBox ID="chkCancel" runat="server" Text="Is Cancel"
                                                            Visible="false" />
                                                        <asp:CheckBox ID="chkpost" runat="server" Visible="false" />
                                                        <asp:HiddenField ID="hdnOrderId" runat="server" Value="0" />
                                                        <br />
                                                    </div>
                                                    <div class="col-md-12" style="text-align: center">
                                                        <asp:Button ID="btnPost" runat="server" Text="<%$ Resources:Attendance,Post %>" CssClass="btn btn-primary"
                                                            OnClick="btnPost_Click" ValidationGroup="a" Visible="false" />
                                                        <cc1:ConfirmButtonExtender ID="confirm1" runat="server" ConfirmText="Are you sure to post the record ?"
                                                            TargetControlID="btnPost">
                                                        </cc1:ConfirmButtonExtender>

                                                        <asp:Button ID="btnSave" runat="server" Text="<%$ Resources:Attendance,Save %>" CssClass="btn btn-success"
                                                            OnClick="btnSave_Click" ValidationGroup="a" Visible="false" />

                                                        <asp:Button ID="btnReset" runat="server" Text="<%$ Resources:Attendance,Reset %>"
                                                            CssClass="btn btn-primary" OnClick="btnReset_Click" CausesValidation="False" />

                                                        <asp:Button ID="btnCancel" runat="server" Text="<%$ Resources:Attendance,Cancel %>"
                                                            CssClass="btn btn-danger" OnClick="btnCancel_Click" CausesValidation="False" />

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
                                                    <asp:Label ID="Label18" runat="server" Text="Advance Search"></asp:Label></h3>
                                                &nbsp;&nbsp;|&nbsp;&nbsp;
				<asp:Label ID="lblbinTotalRecords" Font-Bold="true" runat="server" Text="<%$ Resources:Attendance,Total Records: 0 %>"></asp:Label>

                                                <div class="box-tools pull-right">
                                                    <button type="button" class="btn btn-box-tool" data-widget="collapse">
                                                        <i id="I2" runat="server" class="fa fa-plus"></i>
                                                    </button>
                                                </div>
                                            </div>
                                            <div class="box-body">

                                                <div class="col-lg-3">
                                                    <asp:DropDownList ID="ddlbinFieldName" runat="server" CssClass="form-control" AutoPostBack="true" OnSelectedIndexChanged="ddlFieldNameBin_SelectedIndexChanged">
                                                        <asp:ListItem Selected="True" Text="<%$ Resources:Attendance,Request No %>" Value="Request_No"></asp:ListItem>
                                                        <asp:ListItem Text="<%$ Resources:Attendance,Request Date %>" Value="Request_Date"></asp:ListItem>
                                                        <asp:ListItem Text="<%$ Resources:Attendance,Order No. %>" Value="Order_No"></asp:ListItem>
                                                        <asp:ListItem Text="<%$ Resources:Attendance,Order Date %>" Value="Order_Date"></asp:ListItem>
                                                        <asp:ListItem Text="<%$ Resources:Attendance,Customer Name %>" Value="Customername"></asp:ListItem>
                                                        <asp:ListItem Text="<%$ Resources:Attendance,Sales Person %>" Value="SalesPersonName"></asp:ListItem>
                                                        <asp:ListItem Text="From Location" Value="From_Location_Name"></asp:ListItem>
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
                                                    <asp:LinkButton ID="imgBtnRestore" CausesValidation="False" runat="server" Visible="false" OnClick="imgBtnRestore_Click" ToolTip="<%$ Resources:Attendance, Active %>"><span class="far fa-lightbulb"  style="font-size:25px;"></span></asp:LinkButton>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                                <div class="box box-warning box-solid" <%= gvBinPurchaseRequest.Rows.Count>0?"style='display:block'":"style='display:none'"%>>
                                    <div class="box-header with-border">
                                        <h3 class="box-title"></h3>
                                    </div>
                                    <div class="box-body">
                                        <div class="row">
                                            <div class="col-md-12">
                                                <div class="flow">
                                                    <asp:GridView CssClass="table-striped table-bordered table table-hover" ID="gvBinPurchaseRequest" PageSize="<%# PageControlCommon.GetPageSize() %>"
                                                        runat="server" AutoGenerateColumns="False" Width="100%" AllowPaging="True"
                                                        AllowSorting="True" OnPageIndexChanging="gvBinPurchaseRequest_PageIndexChanging"
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
                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,Request No %>" SortExpression="Request_No">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblRequestNo" runat="server" Text='<%# Eval("Request_No") %>'></asp:Label>
                                                                    <asp:Label ID="lblReqId" runat="server" Text='<%# Eval("Trans_Id") %>' Visible="false"></asp:Label>
                                                                </ItemTemplate>
                                                                <ItemStyle />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,Request Date %>" SortExpression="Request_Date">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblRequestDate" runat="server" Text='<%# GetDate(Eval("Request_Date").ToString()) %>'></asp:Label>
                                                                </ItemTemplate>
                                                                <ItemStyle />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,Order No. %>" SortExpression="Order_No">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblorderno" runat="server" Text='<%# Eval("Order_No") %>'></asp:Label>
                                                                </ItemTemplate>
                                                                <ItemStyle />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,Order Date %>" SortExpression="Order_Date">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblExpDelDate" runat="server" Text='<%# GetDate(Eval("Order_Date").ToString()) %>'></asp:Label>
                                                                </ItemTemplate>
                                                                <ItemStyle />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,Customer Name %>" SortExpression="Customername">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblCustomerName" runat="server" Text='<%#Eval("Customername")%>' />
                                                                </ItemTemplate>
                                                                <ItemStyle HorizontalAlign="Center" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,Sales Person %>" SortExpression="SalesPersonName">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblsalesperson" runat="server" Text='<%# Eval("SalesPersonName") %>' />
                                                                </ItemTemplate>
                                                                <ItemStyle HorizontalAlign="Center" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="From Location" SortExpression="From_Location_Name">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblFromLocation" runat="server" Text='<%# Eval("From_Location_Name").ToString() %>' />
                                                                </ItemTemplate>
                                                                <ItemStyle HorizontalAlign="Center" />
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
                    <div class="tab-pane" id="Sales_Order">
                        <asp:UpdatePanel ID="Update_Sales_Order" runat="server">
                            <ContentTemplate>


                                <div class="row">
                                    <div class="col-md-12">
                                        <div id="Div3" runat="server" class="box box-info collapsed-box">
                                            <div class="box-header with-border">
                                                <h3 class="box-title">
                                                    <asp:Label ID="Label19" runat="server" Text="Advance Search"></asp:Label></h3>
                                                &nbsp;&nbsp;|&nbsp;&nbsp;
				<asp:Label ID="lblOrderTotalRecords" runat="server" Text="<%$ Resources:Attendance,Total Records: 0 %>"></asp:Label>

                                                <div class="box-tools pull-right">
                                                    <button type="button" class="btn btn-box-tool" data-widget="collapse">
                                                        <i id="I3" runat="server" class="fa fa-plus"></i>
                                                    </button>
                                                </div>
                                            </div>
                                            <div class="box-body">
                                                <div class="col-lg-3">
                                                    <asp:DropDownList ID="ddlOrderFieldName" runat="server" CssClass="form-control" OnSelectedIndexChanged="ddlOrderFieldName_SelectedIndexChanged" AutoPostBack="true">

                                                        <asp:ListItem Text="<%$ Resources:Attendance, Order No. %>" Value="SalesOrderNo" Selected="True"></asp:ListItem>
                                                        <asp:ListItem Text="<%$ Resources:Attendance, Order Date %>" Value="SalesOrderDate"></asp:ListItem>
                                                        <asp:ListItem Text="<%$ Resources:Attendance,Customer Name %>" Value="Name"></asp:ListItem>
                                                        <asp:ListItem Text="<%$ Resources:Attendance,Product Id %>" Value="ProductCode"></asp:ListItem>
                                                        <%-- <asp:ListItem Text="<%$ Resources:Attendance, Transfer Type %>" Value="TransType"></asp:ListItem>
                                                        <asp:ListItem Text="<%$ Resources:Attendance,Transfer No. %>" Value="QauotationNo"></asp:ListItem>--%>
                                                    </asp:DropDownList>
                                                </div>
                                                <div class="col-lg-2">
                                                    <asp:DropDownList ID="ddlOrderOption" runat="server" CssClass="form-control">
                                                        <asp:ListItem Text="<%$ Resources:Attendance,--Select-- %>"></asp:ListItem>
                                                        <asp:ListItem Text="<%$ Resources:Attendance,Equal %>"></asp:ListItem>
                                                        <asp:ListItem Selected="True" Text="<%$ Resources:Attendance,Contains %>"></asp:ListItem>
                                                        <asp:ListItem Text="<%$ Resources:Attendance,Like %>"></asp:ListItem>
                                                    </asp:DropDownList>
                                                </div>
                                                <div class="col-lg-5">
                                                    <asp:TextBox ID="txtOrderValue" runat="server" CssClass="form-control" placeholder="Search From Content"></asp:TextBox>
                                                    <asp:TextBox ID="txtOrderValueDate" runat="server" CssClass="form-control" Visible="false" placeholder="Search From Date"></asp:TextBox>
                                                    <cc1:CalendarExtender OnClientShown="showCalendar" ID="CalendarExtendertxtValueDate" runat="server" TargetControlID="txtOrderValueDate" />
                                                </div>
                                                <div class="col-lg-2">
                                                    <asp:LinkButton ID="btnbindOrderrpt" runat="server" CausesValidation="False" OnClick="btnbindOrderrpt_Click" ToolTip="<%$ Resources:Attendance,Search %>"><span class="fa fa-search"  style="font-size:25px;"></span></asp:LinkButton>&nbsp;&nbsp;&nbsp;
                                                    <asp:LinkButton ID="btnOrderRefreshReport" runat="server" CausesValidation="False" OnClick="btnOrderRefreshReport_Click" ToolTip="<%$ Resources:Attendance,Refresh %>"><span class="fa fa-repeat"  style="font-size:25px;"></span></asp:LinkButton>
                                                </div>

                                            </div>
                                        </div>
                                    </div>
                                </div>



                                <div class="box box-primary box-solid" >

                                    <div class="box-header with-border">
                                        <label>Location : </label>
                                        <asp:DropDownList ID="ddlorderlocation" runat="server" OnSelectedIndexChanged="ddlorderlocation_SelectedIndexChanged" AutoPostBack="true" Style="color: black !important;">
                                        </asp:DropDownList>

                                         <label>Category : </label>
                                        <asp:DropDownList ID="ddlCategory" runat="server" OnSelectedIndexChanged="ddlCategory_SelectedIndexChanged" AutoPostBack="true" Style="color: black !important;">
                                        </asp:DropDownList>
                                        <asp:Button ID="btnGeneraterequest" runat="server" Text="Generate Request" CssClass="btn" BackColor="Green"
                                            OnClick="btnGeneraterequest_Click" ValidationGroup="a" Visible="false" />

                                        <h3 class="box-title"></h3>
                                    </div>
                                    <div class="box-body">
                                        <div class="row">
                                            <div class="col-md-12">
                                                <div class="flow" style="height: 300px;">
                                                    <asp:GridView CssClass="table-striped table-bordered table table-hover" ID="GvSalesOrder" PageSize="<%# PageControlCommon.GetPageSize() %>" runat="server"
                                                        AutoGenerateColumns="False" Width="100%" AllowPaging="True" OnPageIndexChanging="GvSalesOrder_PageIndexChanging"
                                                        AllowSorting="True" OnSorting="GvSalesOrder_Sorting" Visible="false">

                                                        <Columns>
                                                            <asp:TemplateField Visible="false">
                                                                <ItemTemplate>
                                                                    <asp:Button ID="btnEdit" runat="server" BorderStyle="None" BackColor="Transparent"
                                                                        CausesValidation="False" CssClass="btnPull" CommandArgument='<%# Eval("Trans_Id") %>'
                                                                        OnCommand="btnSIEdit_Command" CommandName='<%# Eval("Location_Id") %>' />
                                                                </ItemTemplate>
                                                                <ItemStyle HorizontalAlign="Center" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,Order No. %>" SortExpression="SalesOrderNo">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblgvSONo" runat="server" Text='<%#Eval("SalesOrderNo") %>' />
                                                                </ItemTemplate>
                                                                <ItemStyle HorizontalAlign="Center" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,Order Date %>" SortExpression="SalesOrderDate">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblgvSODate" runat="server" Text='<%#GetDate(Eval("SalesOrderDate").ToString()) %>' />
                                                                </ItemTemplate>
                                                                <ItemStyle HorizontalAlign="Center" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,Customer Name %>" SortExpression="CustomerName">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblgvCustomer" runat="server" Text='<%#Eval("CustomerName") %>' />
                                                                </ItemTemplate>
                                                                <ItemStyle HorizontalAlign="Center" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,Transfer Type%>" SortExpression="TransType">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblgvTransType" runat="server" Text='<%#Eval("TransType") %>' />
                                                                </ItemTemplate>
                                                                <ItemStyle HorizontalAlign="Center" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,Transfer No.%>" SortExpression="QauotationNo">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblgvTransNo" runat="server" Text='<%#Eval("QauotationNo") %>' />
                                                                </ItemTemplate>
                                                                <ItemStyle HorizontalAlign="Center" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,Status%>" SortExpression="Field4">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblst" runat="server" Text='<%# Eval("Field4").ToString() %>' />
                                                                </ItemTemplate>
                                                                <ItemStyle HorizontalAlign="Center" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="From Location" SortExpression="From_Location_Name">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblFromLocation" runat="server" Text='<%# Eval("From_Location_Name").ToString() %>' />
                                                                </ItemTemplate>
                                                                <ItemStyle HorizontalAlign="Center" />
                                                            </asp:TemplateField>
                                                        </Columns>

                                                        <PagerStyle CssClass="pagination-ys" />

                                                    </asp:GridView>

                                                    <asp:GridView CssClass="table-striped table-bordered table table-hover" runat="server" ID="gvPendingSalesOrder" AutoGenerateColumns="false" Width="100%">
                                                        <Columns>
                                                            <asp:TemplateField>
                                                                <ItemTemplate>
                                                                    <asp:CheckBox runat="server" ID="chkSO" />
                                                                </ItemTemplate>
                                                                <HeaderTemplate>
                                                                    <asp:CheckBox runat="server" ID="chkHeaderSO" AutoPostBack="true" OnCheckedChanged="chkHeaderSO_CheckedChanged" />
                                                                </HeaderTemplate>
                                                            </asp:TemplateField>

                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance, Location %>">
                                                                <ItemTemplate>
                                                                    <asp:Label runat="server" ID="lblLocationName" Text='<%# Eval("Location_Name") %>'></asp:Label>
                                                                    <asp:Label runat="server" ID="lbllocId" Text='<%# Eval("location_id") %>' Visible="false"></asp:Label>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>

                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance, Order No. %>">
                                                                <ItemTemplate>
                                                                    <asp:Label runat="server" ID="lblOrderNo" Text='<%# Eval("SalesOrderNo") %>'></asp:Label>
                                                                    <asp:HiddenField runat="server" ID="gvOrderId" Value='<%# Eval("trans_id") %>'></asp:HiddenField>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance, Order Date %>">
                                                                <ItemTemplate>
                                                                    <asp:Label runat="server" ID="lblOrderdate" Text='<%#  Eval("salesorderdate").ToString()==""?"":Convert.ToDateTime( Eval("salesorderdate").ToString()).ToString("dd-MMM-yyyy") %>'></asp:Label>
                                                                </ItemTemplate>
                                                                <ItemStyle Width="80px" />
                                                            </asp:TemplateField>

                                                            <asp:TemplateField HeaderText="Customer">
                                                                <ItemTemplate>
                                                                    <%# Eval("Name") %>
                                                                    <asp:Label runat="server" ID="lblCustomerId" Text='<%# Eval("CustomerId") %>' Visible="false"></asp:Label>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>

                                                            <asp:TemplateField HeaderText="Product ID">
                                                                <ItemTemplate>
                                                                    <asp:Label runat="server" ID="lblProductCode" Text='<%# Eval("ProductCode") %>'></asp:Label>
                                                                    <asp:HiddenField runat="server" ID="gvhdnProductId" Value='<%# Eval("Product_Id") %>'></asp:HiddenField>
                                                                    <asp:HiddenField runat="server" ID="gvHdnUnitCost" Value='<%# Eval("UnitPrice") %>'></asp:HiddenField>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Product Name">
                                                                <ItemTemplate>
                                                                    <asp:Label runat="server" ID="lblProductName" Text='<%# Eval("EproductName") %>'></asp:Label>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>

                                                            <asp:TemplateField HeaderText="Unit Name">
                                                                <ItemTemplate>
                                                                    <asp:Label runat="server" ID="lblUnit" Text='<%# Eval("Unit_name") %>'></asp:Label>
                                                                    <asp:HiddenField runat="server" ID="gvhdnUnitId" Value='<%# Eval("Unit_Id") %>'></asp:HiddenField>
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


                                                        </Columns>
                                                    </asp:GridView>


                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </div>
                    <div style="display: none" class="tab-pane" id="Report">
                        <asp:UpdatePanel ID="Update_Report" runat="server">
                            <ContentTemplate>
                                <div class="row">
                                    <div class="col-md-12">
                                        <div class="box box-primary">
                                            <div class="box-body">
                                                <div class="form-group">
                                                    <div class="col-md-6">
                                                        <asp:Label ID="Label27" runat="server" Text="<%$ Resources:Attendance,From Date %>"></asp:Label>
                                                        <asp:TextBox ID="txtFromDate" runat="server" CssClass="form-control"></asp:TextBox>
                                                        <cc1:CalendarExtender OnClientShown="showCalendar" ID="txtFrom_CalendarExtender" runat="server" Enabled="True"
                                                            TargetControlID="txtFromDate">
                                                        </cc1:CalendarExtender>
                                                        <br />
                                                    </div>
                                                    <div class="col-md-6">
                                                        <asp:Label ID="Label14" runat="server" Text="<%$ Resources:Attendance,To Date %>"></asp:Label>
                                                        <asp:TextBox ID="txtToDate" runat="server" CssClass="form-control"></asp:TextBox>
                                                        <cc1:CalendarExtender OnClientShown="showCalendar" ID="txtTo_CalendarExtender2" runat="server" Enabled="True" TargetControlID="txtToDate">
                                                        </cc1:CalendarExtender>
                                                        <br />
                                                    </div>
                                                    <div class="col-md-6">
                                                        <asp:Label ID="Label15" runat="server" Text="<%$ Resources:Attendance,Status %>"></asp:Label>
                                                        <asp:DropDownList ID="ddlStatus" runat="server" CssClass="form-control">
                                                            <asp:ListItem Text="All" Value="ALL"></asp:ListItem>
                                                            <asp:ListItem Text="Production Pending" Value="PR"></asp:ListItem>
                                                            <asp:ListItem Text="Production Process" Value="PP"></asp:ListItem>
                                                            <asp:ListItem Text="Production Finish" Value="PF"></asp:ListItem>
                                                        </asp:DropDownList>
                                                        <br />
                                                    </div>
                                                    <div class="col-md-12" style="text-align: center">
                                                        <asp:Button ID="btnShowReport" runat="server" Text="<%$ Resources:Attendance,Report %>" CssClass="btn btn-primary"
                                                            OnClick="btnShowReport_Click" />
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
                </div>
            </div>
        </div>
    </div>

    <div class="modal fade" id="Purchase_Request_Modal" tabindex="-1" role="dialog" aria-labelledby="Purchase_Request_ModalLabel" aria-hidden="true">
        <div class="modal-dialog modal-lg">
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal">
                        <span aria-hidden="true">&times;</span><span class="sr-only">Close</span></button>
                    <h4 class="modal-title" id="Purchase_Request_ModalLabel">
                        <asp:Label ID="Label17" runat="server" Font-Size="14px" Font-Bold="true"
                            Text="<%$ Resources:Attendance,Purchase Request %>"></asp:Label></h4>
                </div>
                <div class="modal-body">
                    <asp:UpdatePanel ID="update_Modal" runat="server">
                        <ContentTemplate>
                            <div class="row">
                                <div class="col-md-12">
                                    <div class="box box-primary">
                                        <div class="box-body">
                                            <div class="form-group">
                                                <div class="col-md-6">
                                                    <asp:Label ID="Label8" runat="server" Text="<%$ Resources:Attendance,Request Date %>"></asp:Label>
                                                    <asp:Label ID="lblRequestdateView" Font-Bold="true" runat="server"></asp:Label>
                                                    <br />
                                                </div>
                                                <div class="col-md-6">
                                                    <asp:Label ID="Label9" runat="server" Text="<%$ Resources:Attendance,Request No %>"></asp:Label>
                                                    <asp:Label ID="lblRequestNoView" Font-Bold="true" runat="server"></asp:Label>
                                                    <br />
                                                </div>
                                                <div class="col-md-6">
                                                    <asp:Label ID="Label11" runat="server" Text="<%$ Resources:Attendance,Expected Delivery Date %>"></asp:Label>
                                                    <asp:Label ID="lblExpDelDateView" Font-Bold="true" runat="server"></asp:Label>
                                                    <br />
                                                </div>
                                                <div class="col-md-12">
                                                    <hr />
                                                </div>
                                                <div class="col-md-12">
                                                    <div style="overflow: auto">
                                                        <asp:GridView CssClass="table-striped table-bordered table table-hover" ID="GridView_ViewDetail" runat="server" AutoGenerateColumns="False"
                                                            Width="100%">
                                                            <Columns>
                                                                <asp:TemplateField HeaderText="<%$ Resources:Attendance,Serial No %>">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblSerialNO" runat="server" Text='<%# Eval("Serial_No") %>'></asp:Label>
                                                                    </ItemTemplate>
                                                                    <ItemStyle />
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="<%$ Resources:Attendance,Product Id%>">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblproductcode" runat="server" Text='<%# ProductCode(Eval("ProductId").ToString()) %>'></asp:Label>
                                                                    </ItemTemplate>
                                                                    <ItemStyle />
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="<%$ Resources:Attendance,Product Name %>">
                                                                    <ItemTemplate>
                                                                        <table width="100%">
                                                                            <tr>
                                                                                <td>
                                                                                    <asp:Label ID="lblPID" Visible="false" runat="server" Text='<%# Eval("ProductId") %>'></asp:Label>
                                                                                    <%-- <asp:Label ID="lblProductId" runat="server" Text='<%# ProductName(Eval("Product_Id").ToString()) %>'></asp:Label>
                                                                                    --%>
                                                                                    <asp:Label ID="lblProductId" runat="server" Text='<%# ProductName(Eval("ProductId").ToString()) %>'></asp:Label>
                                                                                </td>
                                                                                <td align="right">
                                                                                    <asp:ImageButton ID="lnkDes" runat="server" ImageUrl="~/Images/detail.png" />
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
                                                                                                        <asp:Label ID="lblDescription" runat="server" Text='<%# ProductDescription(Eval("ProductId").ToString()) %>'></asp:Label>
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
                                                                        <asp:Label ID="lblUnit" runat="server" Text='<%# UnitName(Eval("UnitId").ToString()) %>'></asp:Label>
                                                                    </ItemTemplate>
                                                                    <ItemStyle />
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="<%$ Resources:Attendance,Quantity %>">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblReqQty" runat="server" Text='<%# Eval("Quantity") %>'></asp:Label>
                                                                    </ItemTemplate>
                                                                    <ItemStyle />
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="<%$ Resources:Attendance,Unit Price %>">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblunitPrice" runat="server" Text='<%# Eval("UnitPrice") %>'></asp:Label>
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
                                                    <asp:Label ID="Label12" runat="server" Text="<%$ Resources:Attendance,Description %>"></asp:Label>
                                                    <asp:Panel ID="panelDescView" runat="server" BackColor="White" BorderColor="#8ca7c1"
                                                        BorderStyle="Solid" BorderWidth="1px">
                                                        <asp:Literal ID="txtDescriptionView" runat="server"></asp:Literal>
                                                    </asp:Panel>
                                                    <br />
                                                </div>
                                                <div class="col-md-6">
                                                    <asp:Button ID="btnCancelView" runat="server" Text="<%$ Resources:Attendance,Close %>"
                                                        CssClass="btn btn-primary" OnClick="btnCancelView_Click" CausesValidation="False" />
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
                    <asp:UpdatePanel ID="Update_Modal_Button" runat="server">
                        <ContentTemplate>
                            <button type="button" class="btn btn-danger" data-dismiss="modal">
                                Close</button>
                            <button type="button" class="btn btn-primary">
                                Save changes</button>
                        </ContentTemplate>
                    </asp:UpdatePanel>
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

    <asp:UpdateProgress ID="UpdateProgress4" runat="server" AssociatedUpdatePanelID="Update_Sales_Order">
        <ProgressTemplate>
            <div class="modal_Progress">
                <div class="center_Progress">
                    <img alt="" src="../Images/ajax-loader2.gif" style="vertical-align: middle; background-color: White; border-radius: 10px;" />
                </div>
            </div>
        </ProgressTemplate>
    </asp:UpdateProgress>

    <asp:UpdateProgress ID="UpdateProgress5" runat="server" AssociatedUpdatePanelID="Update_Report">
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

        function LI_Edit_Request_Active() {
            $("#Li_Sales_Order").removeClass("active");
            $("#Sales_Order").removeClass("active");

            $("#Li_New").addClass("active");
            $("#New").addClass("active");
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
        function Li_Tab_Sales_Order()
        {

        }
    </script>
</asp:Content>
