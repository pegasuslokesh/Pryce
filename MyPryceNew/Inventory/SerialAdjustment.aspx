<%@ Page Title="" Language="C#" MasterPageFile="~/ERPMaster.master" AutoEventWireup="true" CodeFile="SerialAdjustment.aspx.cs" Inherits="Inventory_SerialAdjustment" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Src="~/WebUserControl/ReportSystem.ascx" TagName="ReportSystem" TagPrefix="RS" %>
<asp:Content ID="Content1" ContentPlaceHolderID="Head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="HeaderContent" runat="Server">
    <h1>
        <i class="fas fa-sliders-h"></i>&nbsp;&nbsp;
        <asp:Label ID="lblHeader" runat="server" Text="<%$ Resources:Attendance,Serial Adjustment%>"></asp:Label>
    </h1>
    <ol class="breadcrumb">
        <li><a href="#"><i class="fa fa-dashboard"></i>
            <asp:Label ID="Label6" runat="server" Text="<%$ Resources:Attendance,Inventory%>"></asp:Label></a></li>
        <li><a href="#"><i class="fa fa-dashboard"></i>
            <asp:Label ID="Label10" runat="server" Text="<%$ Resources:Attendance,Inventory%>"></asp:Label></a></li>
        <li class="active">
            <asp:Label ID="Label5" runat="server" Text="<%$ Resources:Attendance,Serial Adjustment%>"></asp:Label></li>
    </ol>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="Server">
    <asp:UpdatePanel ID="Update_Button" runat="server">
        <ContentTemplate>
            <asp:Button ID="Btn_Serial_Number" Style="display: none;" runat="server" data-toggle="modal" data-target="#Serial_Number" Text="View Modal" />
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
                                                    <asp:DropDownList ID="ddlFieldName" runat="server" CssClass="form-control" OnSelectedIndexChanged="ddlFieldName_SelectedIndexChanged" AutoPostBack="true">
                                                        <asp:ListItem Text="<%$ Resources:Attendance, Voucher No. %>" Value="VoucherNo" Selected="True"></asp:ListItem>
                                                        <asp:ListItem Text="<%$ Resources:Attendance, Voucher Date %>" Value="Vdate"></asp:ListItem>
                                                        <asp:ListItem Text="<%$ Resources:Attendance, Net Amount %>" Value="NetAmount"></asp:ListItem>
                                                        <asp:ListItem Text="<%$ Resources:Attendance,Remark %>" Value="Remark"></asp:ListItem>
                                                        <asp:ListItem Text="<%$ Resources:Attendance,Created By %>" Value="Created_User"></asp:ListItem>
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
                                                        <asp:TextBox ID="txtValueDate" runat="server" CssClass="form-control" Visible="false" placeholder="Search From Date"></asp:TextBox>
                                                        <cc1:CalendarExtender OnClientShown="showCalendar" ID="CalendarExtendertxtValueDate" runat="server" TargetControlID="txtValueDate" />
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
                                <div class="box box-warning box-solid" <%= GvStockAdjustment.Rows.Count>0?"style='display:block'":"style='display:none'" %>>
                                    <div class="box-body">
                                        <div class="row">
                                            <div class="col-md-12">
                                                <div class="flow">
                                                    <asp:GridView CssClass="table-striped table-bordered table table-hover" ID="GvStockAdjustment" PageSize="<%# PageControlCommon.GetPageSize() %>"
                                                        runat="server" AutoGenerateColumns="False" Width="100%" AllowPaging="True" OnPageIndexChanging="GvStockAdjustment_PageIndexChanging"
                                                        AllowSorting="True" OnSorting="GvStockAdjustment_Sorting">

                                                        <Columns>

                                                            <asp:TemplateField HeaderText="Action">
                                                                <ItemTemplate>
                                                                    <div class="dropdown" style="position: absolute;">
                                                                        <button class="btn btn-default dropdown-toggle" type="button" data-toggle="dropdown">
                                                                            <i class="fa fa-ellipsis-h" aria-hidden="true"></i>
                                                                        </button>
                                                                        <ul class="dropdown-menu">
                                                                             <li <%= hdnCanView.Value=="true"?"style='display:block'":"style='display:none'" %>>
                                                                                <a onclick="getReportDataWithLoc('<%# Eval("TransId") %>','<%# Eval("FromLocationId") %>')"><i class="fa fa-print" style="cursor: pointer"></i>Report System</a>
                                                                            </li>
                                                                            <li <%= hdnCanView.Value=="true"?"style='display:block'":"style='display:none'" %>>
                                                                                <asp:LinkButton ID="btnView" runat="server" CommandArgument='<%# Eval("TransId") %>' CausesValidation="False" OnCommand="btnView_Command"><i class="fa fa-eye"></i>View</asp:LinkButton>
                                                                            </li>

                                                                            <li <%= hdnCanEdit.Value=="true"?"style='display:block'":"style='display:none'" %>>
                                                                                <asp:LinkButton ID="btnEdit" runat="server" CommandArgument='<%# Eval("TransId") %>' OnCommand="btnEdit_Command" CausesValidation="False"><i class="fa fa-pencil"></i>Edit </asp:LinkButton>
                                                                            </li>
                                                                            <li <%= hdnCanDelete.Value=="true"?"style='display:block'":"style='display:none'" %>>
                                                                                <asp:LinkButton ID="IbtnDelete" runat="server" CausesValidation="False" CommandArgument='<%# Eval("TransId") %>' OnCommand="IbtnDelete_Command"><i class="fa fa-trash"></i>Delete</asp:LinkButton>
                                                                                <cc1:ConfirmButtonExtender ID="confirm2" runat="server" ConfirmText="<%$ Resources:Attendance,Are you sure you want to delete the record?%>" TargetControlID="IbtnDelete"></cc1:ConfirmButtonExtender>
                                                                            </li>
                                                                        </ul>
                                                                    </div>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance, Voucher No.%>" SortExpression="VoucherNo">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblgvVoucherNo" runat="server" Text='<%#Eval("VoucherNo") %>' />
                                                                </ItemTemplate>
                                                                <ItemStyle HorizontalAlign="Left" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance, Voucher Date %>" SortExpression="Vdate">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblgvVoucherDate" runat="server" Text='<%#GetDate(Eval("Vdate").ToString()) %>' />
                                                                </ItemTemplate>

                                                            </asp:TemplateField>

                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,Remark %>" SortExpression="Remark">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblgvRemarks" runat="server" Text='<%#Eval("Remark") %>' />
                                                                </ItemTemplate>
                                                                <ItemStyle HorizontalAlign="Left" Width="35%" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,Created By %>" SortExpression="Created_User">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblCreated_User" runat="server" Text='<%# Eval("Created_User") %>'></asp:Label>
                                                                </ItemTemplate>
                                                                <ItemStyle />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,Modified By %>" SortExpression="Modified_User">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblModified_User" runat="server" Text='<%# Eval("Modified_User") %>'></asp:Label>
                                                                </ItemTemplate>
                                                                <ItemStyle />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance, Net Amount %>" SortExpression="NetAmount">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblgvNetAmount" runat="server" Text='<%#GetAmountDecimal(Eval("NetAmount").ToString()) %>' />
                                                                </ItemTemplate>
                                                                <ItemStyle HorizontalAlign="Right" />
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
                                                        <asp:Label ID="lblVoucherNo" runat="server" Text="<%$ Resources:Attendance,Voucher No. %>" />
                                                        <a style="color: Red">*</a>
                                                        <asp:RequiredFieldValidator EnableClientScript="true" Style="float: right;" runat="server" ID="RequiredFieldValidator3" ValidationGroup="Save"
                                                            Display="Dynamic" SetFocusOnError="true" ControlToValidate="txtVoucherNo" ErrorMessage="<%$ Resources:Attendance,Enter Voucher No%>"></asp:RequiredFieldValidator>

                                                        <asp:TextBox ID="txtVoucherNo" runat="server" CssClass="form-control" />
                                                        <br />
                                                    </div>
                                                    <div class="col-md-6">
                                                        <asp:Label ID="lblVoucherDate" runat="server" Text="<%$ Resources:Attendance,Voucher Date %>" />
                                                        <a style="color: Red">*</a>
                                                        <asp:RequiredFieldValidator EnableClientScript="true" Style="float: right;" runat="server" ID="RequiredFieldValidator1" ValidationGroup="Save"
                                                            Display="Dynamic" SetFocusOnError="true" ControlToValidate="txtVoucherDate" ErrorMessage="<%$ Resources:Attendance,Enter Voucher Date%>"></asp:RequiredFieldValidator>

                                                        <asp:TextBox ID="txtVoucherDate" runat="server" CssClass="form-control" />
                                                        <cc1:CalendarExtender OnClientShown="showCalendar" ID="Calender" runat="server" TargetControlID="txtVoucherDate" />
                                                        <br />
                                                    </div>
                                                    <div class="col-md-6">
                                                        <asp:Button ID="btnAddNewProduct" runat="server" Text="<%$ Resources:Attendance,Add Product %>" Visible="false" CssClass="btn btn-info" OnClick="btnAddNewProduct_Click" />
                                                        <br />
                                                    </div>
                                                    <div class="col-md-12">
                                                        <div class="row">
                                                            <div class="col-md-12">
                                                                <div class="box box-primary">
                                                                    <div class="box-header with-border">
                                                                        <h3 class="box-title">
                                                                            <asp:Label ID="lblProductHeader" runat="server" Font-Size="14px" Font-Bold="true"
                                                                                Text="<%$ Resources:Attendance, Product Setup %>"></asp:Label></h3>
                                                                        <div class="box-tools pull-right">
                                                                            <button type="button" class="btn btn-box-tool" data-widget="collapse">
                                                                                <i class="fa fa-minus"></i>
                                                                            </button>
                                                                        </div>
                                                                    </div>
                                                                    <div class="box-body">
                                                                        <div class="form-group">
                                                                            <div class="col-md-6">
                                                                                <asp:Label ID="Label38" runat="server" Text="<%$ Resources:Attendance,Product Id%>" />
                                                                                <a style="color: Red">*</a>
                                                                                <asp:RequiredFieldValidator EnableClientScript="true" Style="float: right;" runat="server" ID="RequiredFieldValidator2" ValidationGroup="Add"
                                                                                    Display="Dynamic" SetFocusOnError="true" ControlToValidate="txtProductcode" ErrorMessage="<%$ Resources:Attendance,Enter Product Id%>"></asp:RequiredFieldValidator>

                                                                                <asp:TextBox ID="txtProductcode" runat="server" CssClass="form-control" AutoPostBack="True"
                                                                                    OnTextChanged="txtProductCode_TextChanged" BackColor="#eeeeee" />
                                                                                <cc1:AutoCompleteExtender ID="AutoCompleteExtender5" runat="server" CompletionInterval="100"
                                                                                    DelimiterCharacters="" Enabled="True" MinimumPrefixLength="1" ServiceMethod="GetCompletionListProductCode"
                                                                                    ServicePath="" TargetControlID="txtProductcode" UseContextKey="True" CompletionListCssClass="autoCompleteList" CompletionListItemCssClass="autoCompleteListItem" CompletionListHighlightedItemCssClass="autoCompleteSelectedListItem" OnClientShown="resetPosition">
                                                                                </cc1:AutoCompleteExtender>
                                                                                <br />
                                                                            </div>
                                                                            <div class="col-md-12">
                                                                                <asp:Label ID="lblProductName" runat="server" Text="<%$ Resources:Attendance,Product Name %>" />
                                                                                <a style="color: Red">*</a>
                                                                                <asp:RequiredFieldValidator EnableClientScript="true" Style="float: right;" runat="server" ID="RequiredFieldValidator4" ValidationGroup="Add"
                                                                                    Display="Dynamic" SetFocusOnError="true" ControlToValidate="txtProductName" ErrorMessage="<%$ Resources:Attendance,Enter Product Name%>"></asp:RequiredFieldValidator>

                                                                                <asp:TextBox ID="txtProductName" runat="server" CssClass="form-control" AutoPostBack="true"
                                                                                    OnTextChanged="txtProductName_TextChanged" BackColor="#eeeeee" />
                                                                                <cc1:AutoCompleteExtender ID="AutoCompleteExtender2" runat="server" DelimiterCharacters=""
                                                                                    Enabled="True" ServiceMethod="GetCompletionListProductName" ServicePath="" CompletionInterval="100"
                                                                                    MinimumPrefixLength="1" CompletionSetCount="1" TargetControlID="txtProductName"
                                                                                    UseContextKey="True" CompletionListCssClass="autoCompleteList" CompletionListItemCssClass="autoCompleteListItem" CompletionListHighlightedItemCssClass="autoCompleteSelectedListItem" OnClientShown="resetPosition">
                                                                                </cc1:AutoCompleteExtender>
                                                                                <asp:HiddenField ID="hdnNewProductId" runat="server" Value="0" />
                                                                                <br />
                                                                            </div>
                                                                            <div class="col-md-6">
                                                                                <asp:Label ID="lblUnit" runat="server" Text="<%$ Resources:Attendance,Unit %>" />
                                                                                <a style="color: Red">*</a>
                                                                                <asp:RequiredFieldValidator EnableClientScript="true" Style="float: right;" runat="server" ID="RequiredFieldValidator5" ValidationGroup="Add" Display="Dynamic"
                                                                                    SetFocusOnError="true" ControlToValidate="ddlUnit" InitialValue="--Select--" ErrorMessage="<%$ Resources:Attendance,Select Unit %>" />

                                                                                <asp:DropDownList ID="ddlUnit" runat="server" CssClass="form-control" />

                                                                                <asp:HiddenField ID="hdnUnitId" runat="server" Value="0" />
                                                                                <br />
                                                                            </div>
                                                                            <div class="col-md-6">
                                                                                <asp:Label ID="lblQuantity" runat="server" Text="<%$ Resources:Attendance,Quantity %>" />
                                                                                <a style="color: Red">*</a>
                                                                                <asp:RequiredFieldValidator EnableClientScript="true" Style="float: right;" runat="server" ID="RequiredFieldValidator6" ValidationGroup="Add"
                                                                                    Display="Dynamic" SetFocusOnError="true" ControlToValidate="txtQuantity" ErrorMessage="<%$ Resources:Attendance,Enter Quantity%>"></asp:RequiredFieldValidator>

                                                                                <asp:TextBox ID="txtQuantity" runat="server" CssClass="form-control"
                                                                                    OnTextChanged="txtQuantity_TextChanged" AutoPostBack="true" />
                                                                                <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender2" runat="server" Enabled="True"
                                                                                    TargetControlID="txtQuantity" ValidChars="1,2,3,4,5,6,7,8,9,0,.">
                                                                                </cc1:FilteredTextBoxExtender>
                                                                                <br />
                                                                            </div>
                                                                            <div id="trcost" runat="server">
                                                                                <div class="col-md-6">
                                                                                    <asp:Label ID="lblUnitCost" runat="server" Text="<%$ Resources:Attendance,Unit Cost %>" />
                                                                                    <asp:TextBox ID="txtUnitCost" runat="server" CssClass="form-control"
                                                                                        OnTextChanged="txtUnitCost_TextChanged" AutoPostBack="true" />
                                                                                    <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender3" runat="server" Enabled="True"
                                                                                        TargetControlID="txtUnitCost" ValidChars="1,2,3,4,5,6,7,8,9,0,.">
                                                                                    </cc1:FilteredTextBoxExtender>
                                                                                    <br />
                                                                                </div>
                                                                                <div class="col-md-6">
                                                                                    <asp:Label ID="lblTotal" runat="server" Text="<%$ Resources:Attendance,Total %>" />
                                                                                    <asp:TextBox ID="txtTotal" runat="server" CssClass="form-control" ReadOnly="true" />
                                                                                    <br />
                                                                                </div>
                                                                            </div>
                                                                            <div class="col-md-6">
                                                                                <asp:Label ID="lblTypeOfAdjustment" runat="server" Text="<%$ Resources:Attendance,Type Of Adjustment %>" />
                                                                                <a style="color: Red">*</a>
                                                                                <asp:RequiredFieldValidator EnableClientScript="true" Style="float: right;" runat="server" ID="RequiredFieldValidator7" ValidationGroup="Add" Display="Dynamic"
                                                                                    SetFocusOnError="true" ControlToValidate="ddlTypeOfAdjustment" InitialValue="0" ErrorMessage="<%$ Resources:Attendance,Select Type Of Adjustment %>" />

                                                                                <asp:DropDownList ID="ddlTypeOfAdjustment" runat="server" CssClass="form-control">
                                                                                    <asp:ListItem Text="<%$ Resources:Attendance, --Select--%>" Value="0"></asp:ListItem>
                                                                                    <asp:ListItem Text="<%$ Resources:Attendance, In%>" Value="I"></asp:ListItem>
                                                                                    <asp:ListItem Text="<%$ Resources:Attendance, Out%>" Value="O"></asp:ListItem>
                                                                                </asp:DropDownList>
                                                                                <br />
                                                                            </div>
                                                                            <div class="col-md-12">
                                                                                <asp:Label ID="lblPDescription" runat="server" Text="<%$ Resources:Attendance,Product Description %>" />
                                                                                <asp:Panel ID="pnlPDescription" runat="server" Height="150px" CssClass="form-control"
                                                                                    BorderColor="#8ca7c1" BackColor="#ffffff" ScrollBars="Vertical">
                                                                                    <asp:Literal ID="txtPDescription" runat="server"></asp:Literal>
                                                                                </asp:Panel>
                                                                                <br />
                                                                            </div>
                                                                            <div class="col-md-12" style="text-align: center">
                                                                                <asp:Button ID="btnProductSave" ValidationGroup="Add" runat="server" Text="<%$ Resources:Attendance,Add Product %>"
                                                                                    CssClass="btn btn-primary" OnClick="btnProductSave_Click" />

                                                                                <asp:Button ID="btnProductCancel" runat="server" CssClass="btn btn-danger" Text="<%$ Resources:Attendance,Cancel %>"
                                                                                    CausesValidation="False" OnClick="btnProductCancel_Click" />

                                                                                <asp:HiddenField ID="HiddenField3" runat="server" />
                                                                                <br />
                                                                            </div>
                                                                        </div>
                                                                    </div>
                                                                </div>
                                                            </div>
                                                        </div>
                                                        <br />
                                                    </div>
                                                    <div class="col-md-12" class="flow">
                                                        <asp:GridView CssClass="table-striped table-bordered table table-hover" ID="GvProduct" runat="server" Width="100%" AutoGenerateColumns="False" OnRowDataBound="GvProductDetail_OnRowDataBound">

                                                            <Columns>
                                                                <asp:TemplateField HeaderText="<%$ Resources:Attendance, Edit %>">
                                                                    <ItemTemplate>
                                                                        <asp:ImageButton ID="imgBtnProductEdit" runat="server" CommandArgument='<%#Eval("SerialNo") %>'
                                                                            ImageUrl="~/Images/edit.png" Width="16px" OnCommand="imgBtnProductEdit_Command" />
                                                                    </ItemTemplate>
                                                                    <ItemStyle HorizontalAlign="Center" />
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="<%$ Resources:Attendance, Delete%>">
                                                                    <ItemTemplate>
                                                                        <asp:ImageButton ID="imgBtnProductDelete" runat="server" CommandArgument='<%# Eval("SerialNo") %>'
                                                                            Height="16px" ImageUrl="~/Images/Erase.png" Width="16px" OnCommand="imgBtnProductDelete_Command" />
                                                                    </ItemTemplate>
                                                                    <ItemStyle HorizontalAlign="Center" />
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="<%$ Resources:Attendance,S No. %>">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblSNo" runat="server" Text='<%#Eval("SerialNo") %>' Width="10px" />
                                                                    </ItemTemplate>
                                                                    <ItemStyle HorizontalAlign="Center" Width="20px" />
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="<%$ Resources:Attendance,Product Id%>">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblGvProductCode" runat="server" Text='<%# ProductCode(Eval("ProductId").ToString()) %>'></asp:Label>
                                                                    </ItemTemplate>
                                                                    <ItemStyle />
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="<%$ Resources:Attendance, Product Name %>">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblgvProductId" runat="server" Text='<%#Eval("ProductId") %>' Visible="false" />
                                                                        <asp:Label ID="lblgvProductName" runat="server" Text='<%#GetProductName(Eval("ProductId").ToString()) %>' />
                                                                    </ItemTemplate>
                                                                    <ItemStyle HorizontalAlign="Center" />
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="<%$ Resources:Attendance, Unit %>">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblgvUnitId" runat="server" Visible="false" Text='<%#Eval("UnitID") %>' />
                                                                        <asp:Label ID="lblgvUnit" runat="server" Text='<%#GetUnitName(Eval("UnitID").ToString()) %>' />
                                                                    </ItemTemplate>
                                                                    <ItemStyle HorizontalAlign="Center" />
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="<%$ Resources:Attendance, Quantity %>">
                                                                    <ItemTemplate>
                                                                        <asp:TextBox ID="txtQuantity" runat="server" Width="60px"
                                                                            Text='<%# GetAmountDecimal(Eval("Quantity").ToString()) %>'></asp:TextBox>
                                                                        <asp:LinkButton ID="lnkAddSerial" AutoPostBack="True" runat="server" Text="<%$ Resources:Attendance,Add %>"
                                                                            OnCommand="lnkAddSerial_Command" ForeColor="Blue" Font-Underline="true" CommandArgument='<%# Eval("ProductId") %>' CommandName='<%# Eval("InOut") %>'></asp:LinkButton>
                                                                    </ItemTemplate>
                                                                    <ItemStyle HorizontalAlign="Center" />
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="<%$ Resources:Attendance, Unit Cost %>">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblgvUnitCost" runat="server" Text='<%#GetAmountDecimal(Eval("UnitCost").ToString()) %>' />
                                                                    </ItemTemplate>
                                                                    <ItemStyle HorizontalAlign="Center" />
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="<%$ Resources:Attendance, Total%>">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblgvTotal" runat="server" />
                                                                    </ItemTemplate>
                                                                    <ItemStyle HorizontalAlign="Center" />
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="<%$ Resources:Attendance, Type Of Adjustment%>">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblgvInOut" runat="server" Text='<%#GetType(Eval("InOut").ToString()) %>' />
                                                                        <asp:HiddenField ID="hdnInOutValue" runat="server" Value='<%#Eval("InOut") %>' />
                                                                    </ItemTemplate>
                                                                    <ItemStyle HorizontalAlign="Center" />
                                                                </asp:TemplateField>
                                                            </Columns>

                                                            <PagerStyle CssClass="pagination-ys" />

                                                        </asp:GridView>
                                                        <asp:HiddenField ID="hdnProductId" runat="server" />
                                                        <asp:HiddenField ID="hdnProductName" runat="server" />
                                                    </div>
                                                    <div id="trNettotal" runat="server">
                                                        <div class="col-md-6">
                                                            <asp:Label ID="lblNetAmount" runat="server" Text="<%$ Resources:Attendance,Net Amount%>" />
                                                            <a style="color: Red">*</a>
                                                            <asp:RequiredFieldValidator EnableClientScript="true" Style="float: right;" runat="server" ID="RequiredFieldValidator8" ValidationGroup="Save"
                                                                Display="Dynamic" SetFocusOnError="true" ControlToValidate="txtNetAmount" ErrorMessage="<%$ Resources:Attendance,Enter Net Amount%>"></asp:RequiredFieldValidator>

                                                            <asp:TextBox ID="txtNetAmount" runat="server" CssClass="form-control" ReadOnly="true" />
                                                            <br />
                                                        </div>
                                                        <div class="col-md-6">
                                                            <asp:CheckBox ID="ChkPost" runat="server" Text="<%$ Resources:Attendance, Post%>"
                                                                Visible="false" />
                                                            <br />
                                                        </div>
                                                    </div>
                                                    <div class="col-md-12">
                                                        <asp:Label ID="lblRemark" runat="server" Text="<%$ Resources:Attendance,Remark %>" />
                                                        <asp:TextBox ID="txtRemark" runat="server" CssClass="form-control" TextMode="MultiLine" />
                                                        <br />
                                                    </div>
                                                    <div id="PnlOperationButton" runat="server" class="col-md-12" style="text-align: center">
                                                        <asp:Button ID="btnSInquirySave" runat="server" Text="<%$ Resources:Attendance,Save %>"
                                                            Visible="false" CssClass="btn btn-success" OnClick="btnSInquirySave_Click" />

                                                        <asp:Button ID="btnPostSave" runat="server" Text="<%$ Resources:Attendance,Post & Save %>"
                                                            CssClass="btn btn-success" Visible="false" OnClick="btnPostSave_Click" />

                                                        <cc1:ConfirmButtonExtender ID="confirm1" runat="server" ConfirmText="<%$ Resources:Attendance,Are you sure to post the record ?%>" TargetControlID="btnPostSave"></cc1:ConfirmButtonExtender>

                                                        <asp:Button ID="BtnReset" runat="server" Text="<%$ Resources:Attendance,Reset %>"
                                                            CssClass="btn btn-primary" CausesValidation="False" OnClick="BtnReset_Click" />

                                                        <asp:Button ID="btnSInquiryCancel" runat="server" CssClass="btn btn-danger" Text="<%$ Resources:Attendance,Cancel %>"
                                                            CausesValidation="False" OnClick="btnSInquiryCancel_Click" />

                                                        <asp:HiddenField ID="hdnStockTransId" runat="server" Value="0" />
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

    <div class="modal fade" id="Serial_Number" tabindex="-1" role="dialog" aria-labelledby="Serial_NumberLabel" aria-hidden="true">
        <div class="modal-dialog modal-lg">
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal">
                        <span aria-hidden="true">&times;</span><span class="sr-only">Close</span></button>
                    <h4 class="modal-title" id="Serial_NumberLabel">
                        <asp:Label ID="Label2" runat="server" Font-Size="14px" Font-Bold="true"
                            Text="<%$ Resources:Attendance, Serial No %>"></asp:Label></h4>
                </div>
                <div class="modal-body" style="overflow: auto; max-height: 450px;">
                    <asp:UpdatePanel ID="Update_Serial_Number" runat="server">
                        <ContentTemplate>
                            <div class="row">
                                <div class="col-md-12">
                                    <div class="box box-primary">
                                        <div class="box-body">
                                            <div class="form-group">
                                                <div class="col-md-6">
                                                    <asp:Label ID="lblproductId" runat="server" Text="Product Id"></asp:Label>
                                                    &nbsp:&nbsp<asp:Label ID="lblProductIdvalue" Font-Bold="true" runat="server" Text="0"></asp:Label>
                                                    <br />
                                                </div>
                                                <div class="col-md-12">
                                                    <br />
                                                </div>
                                                <div class="col-md-12">
                                                    <asp:Label ID="lblserialProductname" runat="server" Text="Product Name"></asp:Label>
                                                    &nbsp:&nbsp<asp:Label ID="lblProductNameValue" Font-Bold="true" runat="server" Text="0"></asp:Label>
                                                    <br />
                                                </div>
                                                <div class="col-md-12">
                                                    <br />
                                                </div>
                                                <div id="pnlSerialNumber" runat="server">
                                                    <div class="col-md-6" style="display:none">
                                                        <asp:Label ID="Label15" runat="server" Text="<%$ Resources:Attendance, File Upload%>"></asp:Label>
                                                        <div class="input-group" style="width: 100%;">
                                                          <cc1:AsyncFileUpload ID="FULogoPath"
                                                                
                                                                runat="server" CssClass="form-control"
                                                                CompleteBackColor="White"
                                                                UploaderStyle="Traditional"
                                                                UploadingBackColor="#CCFFFF"
                                                                ThrobberID="FULogo_ImgLoader" Width="100%" />
                                                            <div class="input-group-btn" style="border: solid 1px #d2d6de; width: 35px;">
                                                                <asp:Image ID="FULogo_Img_Right" runat="server" Width="30px" Height="30px" ImageUrl="../Images/Allow.png" Style="display: none" />
                                                                <asp:Image ID="FULogo_Img_Wrong" runat="server" Width="30px" Height="30px" ImageUrl="../Images/Delete1.png" Style="display: none" />
                                                                <asp:Image ID="FULogo_ImgLoader" runat="server" ImageUrl="../Images/loader.gif" />
                                                            </div>
                                                        </div>
                                                    </div>
                                                    <div class="col-md-6" style="text-align: center;display:none">
                                                        <asp:Image ID="imgLogo" runat="server" Width="90px" Height="120px" /><br />
                                                        <br />
                                                        <asp:Button ID="Btnloadfile" runat="server" Text="Load" CssClass="btn btn-primary" OnClick="Btnloadfile_Click" />
                                                        <br />
                                                    </div>
                                                    <div class="col-md-12">
                                                        <br />
                                                    </div>
                                                    <div class="col-md-12">
                                                        <div class="alert alert-info ">
                                                            <div class="row">
                                                                <div class="form-group">
                                                                    <div class="col-lg-6">
                                                                        <asp:TextBox ID="txtserachserialnumber" runat="server" CssClass="form-control"></asp:TextBox>
                                                                        <cc1:TextBoxWatermarkExtender ID="txtwatermarkup" runat="server" TargetControlID="txtserachserialnumber"
                                                                            WatermarkText="Search Serial Number">
                                                                        </cc1:TextBoxWatermarkExtender>
                                                                    </div>
                                                                    <div class="col-lg-2">
                                                                        <asp:ImageButton ID="btnsearchserial" runat="server" CausesValidation="False" Style="margin-top: -5px;"
                                                                            ImageUrl="~/Images/search.png" OnClick="btnsearchserial_Click" ToolTip="<%$ Resources:Attendance,Search %>"></asp:ImageButton>

                                                                        <asp:ImageButton ID="btnRefreshserial" runat="server" CausesValidation="False" Style="width: 33px;"
                                                                            ImageUrl="~/Images/refresh.png" OnClick="btnRefreshserial_Click"
                                                                            ToolTip="<%$ Resources:Attendance,Refresh %>"></asp:ImageButton>
                                                                    </div>
                                                                    <div class="col-lg-2">
                                                                        <h5>
                                                                            <asp:Label ID="Label17" runat="server" Text="Total :"></asp:Label>
                                                                            <asp:Label ID="txtselectedSerialNumber" Font-Bold="true" runat="server" Text="0"></asp:Label></h5>
                                                                    </div>
                                                                    <div class="col-lg-2">
                                                                        <h5>
                                                                            <asp:Label ID="lblCount" runat="server"></asp:Label>
                                                                            <asp:Label ID="txtCount" Font-Bold="true" runat="server" Text="0"></asp:Label></h5>
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
                                                                        <div style="overflow: auto; max-height: 350px;">
                                                                            <asp:GridView CssClass="table-striped table-bordered table table-hover" ID="gvSerialNumber" runat="server" AllowPaging="false" AutoGenerateColumns="False"
                                                                                AllowSorting="true" BorderStyle="Solid" Width="100%"
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
                                                                                    <asp:TemplateField HeaderText="Width">
                                                                                        <ItemTemplate>
                                                                                            <asp:Label ID="lblmfg" runat="server" Text='<%#Eval("Width") %>'></asp:Label>
                                                                                        </ItemTemplate>
                                                                                        <ItemStyle HorizontalAlign="Center" />
                                                                                    </asp:TemplateField>
                                                                                    <asp:TemplateField HeaderText="Length">
                                                                                        <ItemTemplate>
                                                                                            <asp:Label ID="lblBatchNo" runat="server" Text='<%#Eval("Length") %>'></asp:Label>
                                                                                        </ItemTemplate>
                                                                                        <ItemStyle HorizontalAlign="Center" />
                                                                                    </asp:TemplateField>
                                                                                    <asp:TemplateField HeaderText="Quantity">
                                                                                        <ItemTemplate>
                                                                                            <asp:Label ID="lblqty" runat="server" Text='<%#Eval("Quantity") %>'></asp:Label>
                                                                                        </ItemTemplate>
                                                                                        <ItemStyle HorizontalAlign="Center" />
                                                                                    </asp:TemplateField>
                                                                                    <asp:TemplateField HeaderText="Pallet Id">
                                                                                        <ItemTemplate>
                                                                                            <asp:Label ID="lblpalletId" runat="server" Text='<%#Eval("Pallet_ID") %>'></asp:Label>
                                                                                        </ItemTemplate>
                                                                                        <ItemStyle HorizontalAlign="Center" />
                                                                                    </asp:TemplateField>
                                                                                </Columns>


                                                                                <PagerStyle CssClass="pagination-ys" />

                                                                            </asp:GridView>
                                                                            <asp:HiddenField ID="HDFSort" runat="server" />
                                                                        </div>
                                                                    </div>
                                                                    <div class="col-md-4">
                                                                        <div class="flow">
                                                                            <asp:TextBox ID="txtSerialNo" Height="350px" runat="server" CssClass="form-control"
                                                                                TextMode="MultiLine"></asp:TextBox>
                                                                        </div>
                                                                    </div>
                                                                </div>
                                                            </div>
                                                        </div>
                                                        <br />
                                                    </div>
                                                </div>

                                                <div class="col-md-12" id="pnlexp_and_Manf" runat="server" visible="false" style="overflow: auto; max-height: 500px">
                                                    <asp:GridView CssClass="table-striped table-bordered table table-hover" ID="gvStockwithManf_and_expiry" ShowHeader="true" runat="server" AutoGenerateColumns="false"
                                                        Width="100%" DataKeyNames="ProductId" ShowFooter="true" OnRowDeleting="gridView_RowDeleting"
                                                        OnRowCommand="gridView_RowCommand">

                                                        <Columns>
                                                            <asp:TemplateField HeaderText="Quantity">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblQuantity" runat="server" Text='<%#setRoundValue(Eval("Quantity").ToString()) %>'></asp:Label>
                                                                    <asp:HiddenField ID="hdnProductId" runat="server" Value='<%#Eval("ProductId") %>' />
                                                                    <asp:HiddenField ID="hdnOrderId" runat="server" Value='<%#Eval("POID") %>' />
                                                                </ItemTemplate>
                                                                <EditItemTemplate>
                                                                </EditItemTemplate>
                                                                <FooterTemplate>
                                                                    <asp:TextBox ID="txtQuantity" runat="server" Font-Names="Verdana"
                                                                        CausesValidation="true" Width="250px"></asp:TextBox>
                                                                    <cc1:FilteredTextBoxExtender ID="filtertextbox" runat="server" TargetControlID="txtQuantity"
                                                                        FilterType="Numbers">
                                                                    </cc1:FilteredTextBoxExtender>
                                                                </FooterTemplate>
                                                                <ItemStyle Width="10%" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Expiry Date">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblExpiryDate" runat="server" Text='<%#setDateTime(Eval("ExpiryDate").ToString()) %>'></asp:Label>
                                                                </ItemTemplate>
                                                                <EditItemTemplate>
                                                                </EditItemTemplate>
                                                                <FooterTemplate>
                                                                    <asp:TextBox ID="txtExpiryFooter" runat="server" Font-Names="Verdana"
                                                                        Text='<%#Eval("ExpiryDate") %>' CausesValidation="true" Width="250px"></asp:TextBox>
                                                                    <cc1:CalendarExtender OnClientShown="showCalendar" ID="txtExpiryFooter_CalenderExtender" runat="server" Enabled="True"
                                                                        TargetControlID="txtExpiryFooter" Format="dd-MMM-yyyy">
                                                                    </cc1:CalendarExtender>
                                                                </FooterTemplate>
                                                                <ItemStyle Width="8%" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Manufacturing Date">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblTaxValue" runat="server" Text='<%#setDateTime(Eval("ManufacturerDate").ToString()) %>'></asp:Label>
                                                                </ItemTemplate>
                                                                <EditItemTemplate>
                                                                </EditItemTemplate>
                                                                <FooterTemplate>
                                                                    <asp:TextBox ID="txtManfacturerFooter" runat="server" Font-Names="Verdana"
                                                                        Text='<%#Eval("ManufacturerDate") %>' CausesValidation="true" Width="250px"></asp:TextBox>
                                                                    <cc1:CalendarExtender OnClientShown="showCalendar" ID="txtManfacturerFooter_CalenderExtender" runat="server" Enabled="True"
                                                                        TargetControlID="txtManfacturerFooter" Format="dd-MMM-yyyy">
                                                                    </cc1:CalendarExtender>
                                                                </FooterTemplate>
                                                                <ItemStyle Width="8%" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField>
                                                                <EditItemTemplate>
                                                                    <asp:Button ID="ButtonUpdate" runat="server" CommandName="Update" Text="Update" CausesValidation="true"
                                                                        CommandArgument='<%#Eval("Trans_Id") %>' />
                                                                    <asp:Button ID="ButtonCancel" runat="server" CommandName="Cancel" Text="Cancel" />
                                                                </EditItemTemplate>
                                                                <ItemTemplate>
                                                                    <asp:Button ID="ButtonEdit" runat="server" CommandName="Edit" Text="Edit" Visible="false" />
                                                                    <asp:ImageButton ID="IbtnDeletePay" runat="server" CausesValidation="False" CommandArgument='<%# Eval("Trans_Id") %>'
                                                                        CommandName="Delete" ImageUrl="~/Images/Erase.png" ToolTip="<%$ Resources:Attendance,Delete %>"
                                                                        Width="16px" />
                                                                    <%--<asp:Button ID="ButtonDelete" runat="server" CommandName="Delete"  Text="Delete" CommandArgument='<%#Eval("Trans_Id") %>'  />--%>
                                                                </ItemTemplate>
                                                                <FooterTemplate>
                                                                    <asp:ImageButton ID="IbtnAddProductSupplierCode" runat="server" CausesValidation="False"
                                                                        CommandName="AddNew" Height="29px" ImageUrl="~/Images/add.png" Width="35px" ToolTip="<%$ Resources:Attendance,Add %>" />
                                                                    <%--<asp:Button ID="ButtonAdd" runat="server" CommandName="AddNew"  Text="Add New Row"  />--%>
                                                                </FooterTemplate>
                                                                <ItemStyle HorizontalAlign="Center" />
                                                            </asp:TemplateField>
                                                        </Columns>

                                                        <PagerStyle CssClass="pagination-ys" />

                                                    </asp:GridView>
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
                    <asp:UpdatePanel ID="Update_Serial_Number_Button" runat="server">
                        <ContentTemplate>
                            <asp:Button ID="BtnSerialSave" runat="server" Text="<%$ Resources:Attendance,Save %>"
                                CssClass="btn btn-success" OnClick="BtnSerialSave_Click" />

                            <asp:Button ID="btnResetSerial" runat="server" Text="<%$ Resources:Attendance,Reset %>"
                                CssClass="btn btn-primary" OnClick="btnResetSerial_Click" />

                            <button type="button" class="btn btn-danger" data-dismiss="modal">
                                Close</button>

                            <asp:Button ID="btnDefault" runat="server" Style="visibility: hidden" />
                            <asp:Button ID="btncloseserial" runat="server" Text="<%$ Resources:Attendance,Close %>" Visible="false"
                                CssClass="btn btn-primary" OnClick="btncloseserial_Click" />
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
            </div>
        </div>
    </div>

    <asp:UpdateProgress ID="UpdateProgress4" runat="server" AssociatedUpdatePanelID="Update_Serial_Number">
        <ProgressTemplate>
            <div class="modal_Progress">
                <div class="center_Progress">
                    <img alt="" src="../Images/ajax-loader2.gif" style="vertical-align: middle; background-color: White; border-radius: 10px;" />
                </div>
            </div>
        </ProgressTemplate>
    </asp:UpdateProgress>

    <asp:UpdateProgress ID="UpdateProgress5" runat="server" AssociatedUpdatePanelID="Update_Serial_Number_Button">
        <ProgressTemplate>
            <div class="modal_Progress">
                <div class="center_Progress">
                    <img alt="" src="../Images/ajax-loader2.gif" style="vertical-align: middle; background-color: White; border-radius: 10px;" />
                </div>
            </div>
        </ProgressTemplate>
    </asp:UpdateProgress>

      <asp:UpdateProgress ID="UpdateProgress3" runat="server" AssociatedUpdatePanelID="Update_List">
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
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="FooterContent" runat="Server">
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="AfterLoadScriptContent" runat="Server">
    <script src="../Script/common.js"></script>
    <script type="text/javascript">
        function getReportData(transId) {
            $("#prgBar").css("display", "block");
            document.getElementById('<%= reportSystem.FindControl("hdnTransId").ClientID %>').value = transId;
            document.getElementById('<%= reportSystem.FindControl("hdnPageRef").ClientID %>').value = "SA";
            debugger;
            setReportData();
         }

        
function getReportDataWithLoc(transId, locId) {
    $("#prgBar").css("display", "block");
    document.getElementById('<%= reportSystem.FindControl("hdnTransId").ClientID %>').value = transId;
    document.getElementById('<%= reportSystem.FindControl("hdnLocId").ClientID %>').value = locId;
    document.getElementById('<%= reportSystem.FindControl("hdnPageRef").ClientID %>').value = "SA";
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

        function LI_List_Hide() {
            $("#Li_List").removeClass("active");
            $("#List").removeClass("active");

            $("#Li_New").addClass("active");
            $("#New").addClass("active");

            document.getElementById('Li_List').style.display = 'none';
            document.getElementById('List').style.display = 'none';
        }
        function Serial_Number_Popup() {
            document.getElementById('<%= Btn_Serial_Number.ClientID %>').click();
        }
        function Serial_Number_Popup()
        {
            document.getElementById('<%= Btn_Serial_Number.ClientID %>').click();            
        }
    </script>
    <script type="text/javascript">        
        <%--function FuLogo_UploadComplete(sender, args) {
            document.getElementById('<%= FULogo_Img_Wrong.ClientID %>').style.display = "none";
            document.getElementById('<%= FULogo_Img_Right.ClientID %>').style.display = ""; 

            var img = document.getElementById('<%= imgLogo.ClientID %>');
            img.src = "<%=ResolveUrl(FuLogo_UploadFolderPath) %>" + args.get_fileName();
        }--%>
        function FuLogo_UploadError(sender, args) {
            document.getElementById('<%= FULogo_Img_Right.ClientID %>').style.display = "none";
            document.getElementById('<%= FULogo_Img_Wrong.ClientID %>').style.display = "";
            ar img = document.getElementById('<%= imgLogo.ClientID %>');
            img.src = "../Bootstrap_Files/dist/img/NoImage.jpg";
            alert('Invalid File Type, Select Only .png, .jpg, .jpge extension file');
        }
        function FuLogo_UploadStarted(sender, args) {
            var filename = args.get_fileName();

            var filext = filename.substring(filename.lastIndexOf(".") + 1);
            filext = filext.toLowerCase();
            if (filext == "png" || filext == "jpg" || filext == "jpge") {
                return true;
            }
            else {
                throw {
                    name: "Invalid File Type",
                    level: "Error",
                    message: "Invalid File Type, Select Only .png, .jpg, .jpge extension file",
                    htmlMessage: "Invalid File Type, Select Only .png, .jpg, .jpge extension file"
                }
                return false;
            }
        }
    </script>
      <script src="../Script/ReportSystem.js"></script>
</asp:Content>
