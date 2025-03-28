<%@ Page Title="" Language="C#" MasterPageFile="~/ERPMaster.master" AutoEventWireup="true" CodeFile="PreDesignQuotation.aspx.cs" Inherits="Sales_PreDesignQuotation" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit.HTMLEditor" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="Head" runat="Server">
    <style type="text/css">
        .page_enabled, .page_disabled {
            display: inline-block;
            height: 25px;
            min-width: 25px;
            line-height: 25px;
            text-align: center;
            text-decoration: none;
            border: 1px solid #ccc;
        }

        .page_enabled {
            background-color: #eee;
            color: #000;
        }

        .page_disabled {
            background-color: #6C6C6C;
            color: #fff !important;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="HeaderContent" runat="Server">
    <h1>
        <i class="fas fa-money-check-alt"></i>&nbsp;&nbsp;
        <asp:Label ID="lblHeader" runat="server" Text="<%$ Resources:Attendance,Predesign Sales Quotation%>"></asp:Label>
    </h1>
    <ol class="breadcrumb">
        <li><a href="#"><i class="fa fa-dashboard"></i>
            <asp:Label ID="Label6" runat="server" Text="<%$ Resources:Attendance,Inventory%>"></asp:Label></a></li>
        <li><a href="#"><i class="fa fa-dashboard"></i>
            <asp:Label ID="Label10" runat="server" Text="<%$ Resources:Attendance,Sales%>"></asp:Label></a></li>
        <li class="active">
            <asp:Label ID="Label5" runat="server" Text="<%$ Resources:Attendance,Predesign Sales Quotation%>"></asp:Label></li>
    </ol>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="Server">
    <asp:UpdatePanel ID="Update_Button" runat="server">
        <ContentTemplate>
            <asp:Button ID="Btn_Bin" Style="display: none;" runat="server" OnClick="btnBin_Click" Text="Bin" />
            <asp:Button ID="Btn_GST_Modal" Style="display: none;" runat="server" data-toggle="modal" data-target="#Modal_GST" Text="GST" />
            <asp:HiddenField runat="server" ID="hdnCanView" />
            <asp:HiddenField runat="server" ID="hdnCanEdit" />
            <asp:HiddenField runat="server" ID="hdnCanDelete" />
            <asp:HiddenField ID="hfScrollPosition" runat="server" Value="0" />
            <asp:HiddenField ID="hdfCurrentRow" runat="server" />
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
                                                <div class="col-lg-3">
                                                    <asp:DropDownList ID="ddlFieldName" runat="server" CssClass="form-control">
                                                        <asp:ListItem Text="<%$ Resources:Attendance,Template Name %>" Value="Condition2"></asp:ListItem>
                                                        <asp:ListItem Text="<%$ Resources:Attendance,category %>" Value="CategoryName"></asp:ListItem>

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
                                                        <cc1:CalendarExtender OnClientShown="showCalendar" ID="CalendarExtendertxtValueDate" runat="server" TargetControlID="txtValueDate" />
                                                    </asp:Panel>
                                                </div>
                                                <div class="col-lg-2">
                                                    <asp:LinkButton ID="btnbind" runat="server" CausesValidation="False" OnClick="btnbindrpt_Click" ToolTip="<%$ Resources:Attendance,Search %>"><span class="fa fa-search"  style="font-size:25px;"></span></asp:LinkButton>&nbsp;&nbsp;&nbsp;
                                                    <asp:LinkButton ID="btnRefresh" runat="server" CausesValidation="False" OnClick="btnRefreshReport_Click" ToolTip="<%$ Resources:Attendance,Refresh %>"><span class="fa fa-repeat"  style="font-size:25px;"></span></asp:LinkButton>&nbsp;&nbsp;&nbsp;
                                                </div>

                                            </div>
                                        </div>
                                    </div>
                                </div>
                                <div class="box box-warning box-solid" <%= GvSalesQuote.Rows.Count>0?"style='display:block'":"style='display:none'"%>>
                                    <div class="box-body">
                                        <div class="row">
                                            <div class="col-md-12">
                                                <div class="flow">
                                                    <asp:HiddenField ID="hdnGvSalesQuotationCurrentPageIndex" runat="server" Value="1" />
                                                    <asp:GridView CssClass="table-striped table-bordered table table-hover" ID="GvSalesQuote" PageSize='<%# int.Parse(Session["GridSize"].ToString()) %>' runat="server" CurrentSortField="Quotation_Date" CurrentSortDirection="DESC"
                                                        AutoGenerateColumns="False" Width="100%" AllowPaging="false"
                                                        AllowSorting="True" OnSorting="GvSalesQuote_Sorting">

                                                        <Columns>

                                                            <asp:TemplateField HeaderText="Action">
                                                                <ItemTemplate>
                                                                    <div class="dropdown" style="position: absolute;">
                                                                        <button class="btn btn-default dropdown-toggle" type="button" data-toggle="dropdown">
                                                                            <i class="fa fa-ellipsis-h" aria-hidden="true"></i>
                                                                        </button>
                                                                        <ul class="dropdown-menu">

                                                                            <li <%= hdnCanView.Value=="true"?"style='display:block'":"style='display:none'" %>>
                                                                                <asp:LinkButton ID="lnkViewDetail" runat="server" CommandArgument='<%# Eval("SQuotation_Id") %>' OnCommand="lnkViewDetail_Command" CausesValidation="False"><i class="fa fa-eye"></i>View</asp:LinkButton>
                                                                            </li>

                                                                            <li <%= hdnCanEdit.Value=="true"?"style='display:block'":"style='display:none'" %>>
                                                                                <asp:LinkButton ID="btnEdit" runat="server" CommandArgument='<%# Eval("SQuotation_Id") %>' OnCommand="btnEdit_Command" CausesValidation="False"><i class="fa fa-pencil"></i>Edit </asp:LinkButton>
                                                                            </li>
                                                                            <li <%= hdnCanDelete.Value=="true"?"style='display:block'":"style='display:none'" %>>
                                                                                <asp:LinkButton ID="IbtnDelete" runat="server" CausesValidation="False" CommandArgument='<%# Eval("SQuotation_Id") %>' OnCommand="IbtnDelete_Command"><i class="fa fa-trash"></i>Delete</asp:LinkButton>
                                                                                <cc1:ConfirmButtonExtender ID="confirm1" runat="server" ConfirmText="<%$ Resources:Attendance,Are you sure you want to delete the record?%>" TargetControlID="IbtnDelete"></cc1:ConfirmButtonExtender>
                                                                            </li>

                                                                        </ul>
                                                                    </div>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,Category%>" SortExpression="CategoryName">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblgvcategoryName" runat="server" Text='<%#Eval("CategoryName") %>' />
                                                                </ItemTemplate>
                                                                <ItemStyle HorizontalAlign="Left" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,Template Name %>" SortExpression="Condition2">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblgvtemplateName" runat="server" Text='<%#Eval("Condition2") %>' />
                                                                </ItemTemplate>
                                                                <ItemStyle HorizontalAlign="Left" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Template Name (Local)" SortExpression="Condition3">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblgvtemplateName_L" runat="server" Text='<%#Eval("Condition3") %>' />
                                                                </ItemTemplate>
                                                                <ItemStyle HorizontalAlign="Left" />
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

                                            </div>
                                            <div class="col-md-12" style="text-align: right">
                                                <asp:Label ID="lblTotal" runat="server" Text="<%$ Resources:Attendance,Total %>"
                                                    Visible="false"></asp:Label>
                                            </div>
                                        </div>
                                    </div>
                                    <!-- /.box-body -->
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
                                                        <asp:Label ID="lblTemplatename" runat="server" Text="<%$ Resources:Attendance,Template Name %>"></asp:Label>
                                                        <a style="color: Red">*</a>
                                                        <asp:RequiredFieldValidator EnableClientScript="true" Style="float: right;" runat="server" ID="RequiredFieldValidator3" ValidationGroup="Save"
                                                            Display="Dynamic" SetFocusOnError="true" ControlToValidate="txtTemplateName" ErrorMessage="<%$ Resources:Attendance,Enter Template Name%>"></asp:RequiredFieldValidator>

                                                        <asp:TextBox ID="txtTemplateName" runat="server" CssClass="form-control" />
                                                        <br />
                                                    </div>
                                                    <div class="col-md-6">
                                                        <asp:Label ID="Label296" runat="server" Text="Template Name (Local)"></asp:Label>
                                                        <a style="color: Red">*</a>
                                                        <asp:RequiredFieldValidator EnableClientScript="true" Style="float: right;" runat="server" ID="RequiredFieldValidator1" ValidationGroup="Save"
                                                            Display="Dynamic" SetFocusOnError="true" ControlToValidate="txtTemplateNameLocal" ErrorMessage="<%$ Resources:Attendance,Enter Template Name (Local)%>"></asp:RequiredFieldValidator>

                                                        <asp:TextBox ID="txtTemplateNameLocal" runat="server" CssClass="form-control" />
                                                        <br />
                                                    </div>
                                                    <div class="col-md-6">
                                                        <asp:Label ID="Label18" runat="server" Text="<%$ Resources:Attendance,Category %>" />
                                                        <asp:DropDownList ID="ddlProductcategory" runat="server" CssClass="form-control">
                                                        </asp:DropDownList>
                                                        <br />
                                                    </div>
                                                    <div class="col-md-6">
                                                        <asp:Label ID="Label7" runat="server" Text="<%$ Resources:Attendance,Currency %>" />
                                                        <asp:DropDownList ID="ddlCurrency" runat="server" CssClass="form-control"></asp:DropDownList>
                                                        <br />
                                                    </div>
                                                    <div class="col-md-6">
                                                        <br />
                                                        <asp:RadioButton ID="rbtnFormView" Font-Bold="true" runat="server" Text="<%$ Resources:Attendance,Form View%>"
                                                            AutoPostBack="true" GroupName="Product" OnCheckedChanged="rbtnFormView_OnCheckedChanged" />
                                                        <asp:RadioButton ID="rbtnAdvancesearchView" Style="margin-left: 20px;" Font-Bold="true" runat="server" Text="<%$ Resources:Attendance,Advance Search View%>"
                                                            AutoPostBack="true" GroupName="Product" OnCheckedChanged="rbtnFormView_OnCheckedChanged" />
                                                        <br />
                                                    </div>
                                                    <div class="col-md-6" id="Trans_Div" runat="server">
                                                        <asp:Label ID="lblTransType" runat="server" Text="Transaction Type"></asp:Label>
                                                        <%--<a style="color: Red">*</a>--%>
                                                        <asp:DropDownList ID="ddlTransType" runat="server" CssClass="form-control"></asp:DropDownList>
                                                        <%--<asp:RequiredFieldValidator EnableClientScript="true" Style="float: right;" runat="server" ID="RequiredFieldValidator14" ValidationGroup="Save" Display="Dynamic"
                                                                SetFocusOnError="true" ControlToValidate="ddlTransType" InitialValue="-1" ErrorMessage="<%$ Resources:Attendance,Select Transaction Type%>" />--%>
                                                        <br />
                                                    </div>
                                                    <div class="col-md-12" style="text-align: center">
                                                        <br />
                                                        <asp:Button ID="btnAddNewProduct" Style="display: none" runat="server" Text="<%$ Resources:Attendance,Add Product %>"
                                                            CssClass="btn btn-info" Visible="false" OnClick="btnAddNewProduct_Click" />
                                                        <asp:Button ID="btnAddProductScreen" Visible="false" runat="server" Text="<%$ Resources:Attendance,Add Product List %>" CssClass="btn btn-info" OnClick="btnAddProductScreen_Click" />
                                                        <asp:Button ID="btnAddtoList" runat="server" Text="<%$ Resources:Attendance,Fill Your Product %>" CssClass="btn btn-info" Visible="false" OnClick="btnAddtoList_Click" />
                                                        <br />
                                                    </div>
                                                    <div id="pnlProduct1" runat="server" class="row">
                                                        <div class="col-md-12">
                                                            <br />
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
                                                                            <asp:Label ID="Label9" runat="server" Text="<%$ Resources:Attendance,Product Id%>" />
                                                                            <a style="color: Red">*</a>
                                                                            <asp:RequiredFieldValidator EnableClientScript="true" Style="float: right;" runat="server" ID="RequiredFieldValidator2" ValidationGroup="Add"
                                                                                Display="Dynamic" SetFocusOnError="true" ControlToValidate="txtProductcode" ErrorMessage="<%$ Resources:Attendance,Enter Product Id%>"></asp:RequiredFieldValidator>

                                                                            <asp:TextBox ID="txtProductcode" runat="server" CssClass="form-control" AutoPostBack="True"
                                                                                OnTextChanged="txtProductCode_TextChanged" BackColor="#eeeeee" />
                                                                            <cc1:AutoCompleteExtender ID="AutoCompleteExtender2" runat="server" CompletionInterval="100"
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
                                                                            <cc1:AutoCompleteExtender ID="AutoCompleteExtender3" runat="server" DelimiterCharacters=""
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
                                                                            <asp:Label ID="lblRequiredQty" runat="server" Text="<%$ Resources:Attendance,Required Quantity %>" />
                                                                            <a style="color: Red">*</a>
                                                                            <asp:RequiredFieldValidator EnableClientScript="true" Style="float: right;" runat="server" ID="RequiredFieldValidator6" ValidationGroup="Add"
                                                                                Display="Dynamic" SetFocusOnError="true" ControlToValidate="txtRequiredQty" ErrorMessage="<%$ Resources:Attendance,Enter Required Quantity%>"></asp:RequiredFieldValidator>

                                                                            <asp:TextBox ID="txtRequiredQty" runat="server" CssClass="form-control" />
                                                                            <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender2" runat="server" Enabled="True"
                                                                                TargetControlID="txtRequiredQty" ValidChars="1,2,3,4,5,6,7,8,9,0,.">
                                                                            </cc1:FilteredTextBoxExtender>
                                                                            <br />
                                                                        </div>
                                                                        <div class="col-md-6">
                                                                            <asp:Label ID="lblEstimatedUnitPrice" runat="server" Text="<%$ Resources:Attendance,Estimated Unit Price %>" />
                                                                            <asp:TextBox ID="txtEstimatedUnitPrice" runat="server" CssClass="form-control" />
                                                                            <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" runat="server" Enabled="True"
                                                                                TargetControlID="txtEstimatedUnitPrice" ValidChars="1,2,3,4,5,6,7,8,9,0,.">
                                                                            </cc1:FilteredTextBoxExtender>
                                                                            <br />
                                                                        </div>
                                                                        <div class="col-md-6">
                                                                            <asp:Label ID="lblPCurrency" runat="server" Text="<%$ Resources:Attendance,Currency %>" />
                                                                            <a style="color: Red">*</a>
                                                                            <asp:RequiredFieldValidator EnableClientScript="true" Style="float: right;" runat="server" ID="RequiredFieldValidator7" ValidationGroup="Save" Display="Dynamic"
                                                                                SetFocusOnError="true" ControlToValidate="ddlPCurrency" InitialValue="--Select--" ErrorMessage="<%$ Resources:Attendance,Select Currency %>" />

                                                                            <asp:DropDownList ID="ddlPCurrency" runat="server" CssClass="form-control" Enabled="false" />

                                                                            <asp:HiddenField ID="hdnCurrencyId" runat="server" Value="0" />
                                                                            <br />
                                                                        </div>
                                                                        <div class="col-md-12">
                                                                            <asp:Label ID="lblPDescription" runat="server" Text="<%$ Resources:Attendance,Product Description %>" />
                                                                            <asp:Panel ID="pnlPDescription" runat="server" CssClass="form-control"
                                                                                BorderColor="#8ca7c1" BackColor="#ffffff" ScrollBars="Vertical" Visible="false">
                                                                                <asp:Literal ID="txtPDescription" runat="server"></asp:Literal>
                                                                            </asp:Panel>
                                                                            <asp:TextBox ID="txtPDesc" runat="server" TextMode="MultiLine" CssClass="form-control"></asp:TextBox>
                                                                            <br />
                                                                        </div>

                                                                        <div class="col-md-12">
                                                                            <div style="overflow: auto; max-height: 500px;">
                                                                                <asp:GridView CssClass="table-striped table-bordered table table-hover" ID="GvRelatedProduct" runat="server" Width="100%" AutoGenerateColumns="False">

                                                                                    <Columns>
                                                                                        <asp:TemplateField>
                                                                                            <ItemTemplate>
                                                                                                <asp:CheckBox ID="chk" runat="server" />
                                                                                            </ItemTemplate>
                                                                                            <ItemStyle HorizontalAlign="Center" Width="2%" />
                                                                                        </asp:TemplateField>
                                                                                        <asp:TemplateField HeaderText="<%$ Resources:Attendance,Product Id%>">
                                                                                            <ItemTemplate>
                                                                                                <asp:Label ID="lblgvProduct" runat="server" Text='<%#Eval("ProductCode")%>' />
                                                                                            </ItemTemplate>
                                                                                            <ItemStyle HorizontalAlign="Center" Width="8%" />
                                                                                        </asp:TemplateField>
                                                                                        <asp:TemplateField HeaderText="<%$ Resources:Attendance, Product Name %>">
                                                                                            <ItemTemplate>
                                                                                                <table width="100%">
                                                                                                    <tr>
                                                                                                        <td>
                                                                                                            <asp:Label ID="lblgvProductId" runat="server" Text='<%#Eval("SubProduct_Id") %>'
                                                                                                                Visible="false" />
                                                                                                            <asp:Label ID="lblgvProductName" runat="server" Text='<%#Eval("EProductName")%>' />
                                                                                                        </td>
                                                                                                        <td align='<%= PageControlCommon.ChangeTDForDefaultRight()%>'>
                                                                                                            <asp:ImageButton ID="lnkDes" runat="server" ImageUrl="~/Images/detail.png" Enabled="false" />
                                                                                                        </td>
                                                                                                    </tr>
                                                                                                </table>
                                                                                                <%--  <asp:LinkButton ID="lnkDes" runat="server" Text="<%$ Resources:Attendance,More %>"></asp:LinkButton>--%>
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
                                                                                                                                <asp:Label ID="lblgvProductDescription" runat="server" Text='<%#Eval("Description") %>' />
                                                                                                                            </asp:Panel>
                                                                                                                            <br />
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
                                                                                            <ItemStyle HorizontalAlign="Center" Width="27%" />
                                                                                        </asp:TemplateField>
                                                                                        <asp:TemplateField HeaderText="<%$ Resources:Attendance, Unit %>">
                                                                                            <ItemTemplate>
                                                                                                <asp:HiddenField ID="hdnUnitId" runat="server" Value='<%#Eval("UnitId") %>' />
                                                                                                <asp:DropDownList ID="ddlunit" CssClass="form-control" runat="server">
                                                                                                </asp:DropDownList>
                                                                                            </ItemTemplate>
                                                                                            <ItemStyle HorizontalAlign="Center" Width="8%" />
                                                                                        </asp:TemplateField>
                                                                                        <asp:TemplateField HeaderText="<%$ Resources:Attendance, Required Quantity %>">
                                                                                            <ItemTemplate>
                                                                                                <asp:TextBox ID="txtquantity" runat="server" CssClass="form-control"></asp:TextBox>
                                                                                                <cc1:FilteredTextBoxExtender ID="filter1" runat="server" TargetControlID="txtquantity"
                                                                                                    FilterType="Numbers">
                                                                                                </cc1:FilteredTextBoxExtender>
                                                                                            </ItemTemplate>
                                                                                            <ItemStyle HorizontalAlign="Center" Width="13%" />
                                                                                        </asp:TemplateField>
                                                                                        <asp:TemplateField HeaderText="<%$ Resources:Attendance, Currency %>" Visible="false">
                                                                                            <ItemTemplate>
                                                                                                <asp:DropDownList ID="ddlCurrency" CssClass="form-control" runat="server">
                                                                                                </asp:DropDownList>
                                                                                            </ItemTemplate>
                                                                                            <ItemStyle HorizontalAlign="Center" Width="17%" />
                                                                                        </asp:TemplateField>
                                                                                        <asp:TemplateField HeaderText="<%$ Resources:Attendance, Estimated Unit Price %>">
                                                                                            <ItemTemplate>
                                                                                                <asp:TextBox ID="txtEstimatedUnitPrice" runat="server" CssClass="form-control"></asp:TextBox>
                                                                                                <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtendertxtEstimatedUnitPrice" runat="server"
                                                                                                    Enabled="True" TargetControlID="txtEstimatedUnitPrice" ValidChars="1,2,3,4,5,6,7,8,9,0,.">
                                                                                                </cc1:FilteredTextBoxExtender>
                                                                                            </ItemTemplate>
                                                                                            <ItemStyle HorizontalAlign="Center" Width="15%" />
                                                                                        </asp:TemplateField>
                                                                                    </Columns>

                                                                                    <PagerStyle CssClass="pagination-ys" />

                                                                                </asp:GridView>
                                                                                <asp:HiddenField ID="hdnProductId" runat="server" />
                                                                                <asp:HiddenField ID="hdnProductName" runat="server" />
                                                                            </div>
                                                                            <br />
                                                                        </div>
                                                                        <div class="col-md-12" style="text-align: center">
                                                                            <asp:Button ID="btnProductSave" runat="server" ValidationGroup="Add" Text="<%$ Resources:Attendance,Add Product %>" CssClass="btn btn-primary" OnClick="btnProductSave_Click" />

                                                                            <asp:Button ID="btnProductCancel" runat="server" CssClass="btn btn-primary" Text="<%$ Resources:Attendance,Cancel %>"
                                                                                CausesValidation="False" OnClick="btnProductCancel_Click" />

                                                                            <asp:HiddenField ID="HiddenField3" runat="server" />
                                                                            <br />
                                                                        </div>
                                                                    </div>
                                                                </div>
                                                            </div>
                                                        </div>
                                                    </div>

                                                    <div class="col-md-12">
                                                        <br />
                                                        <div class="col-md-12" runat="server" id="scrollArea" onscroll="SetDivPosition()" style="overflow: auto; max-height: 500px;">
                                                            <asp:HiddenField ID="Hdn_Tax_By" runat="server" />
                                                            <asp:GridView CssClass="table-striped table-bordered table table-hover" ID="GvDetail" runat="server" AutoGenerateColumns="False" Width="100%"
                                                                OnRowCreated="GvDetail_RowCreated">

                                                                <Columns>
                                                                    <asp:TemplateField>
                                                                        <ItemTemplate>
                                                                            <asp:ImageButton ID="IbtnDelete" runat="server" CausesValidation="False" CommandArgument='<%#Eval("Serial_No") %>'
                                                                                ImageUrl="~/Images/Erase.png" OnCommand="IbtnDetailDelete_Command" Width="16px" />
                                                                        </ItemTemplate>
                                                                        <ItemStyle HorizontalAlign="Center" />
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField>
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblgvSerialNo" runat="server" Visible="false" Text='<%#Eval("Serial_No") %>' />
                                                                            <asp:Label ID="lblSerialNo" runat="server" Text='<%#Container.DataItemIndex+1 %>' />
                                                                        </ItemTemplate>
                                                                        <ItemStyle HorizontalAlign="Center" Width="30px" />
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="<%$ Resources:Attendance,Id %>">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblgvProductcode" runat="server" Text='<%#new Inv_ProductMaster(Session["DBConnection"].ToString()).GetProductCodebyProductId(Eval("Product_Id").ToString()) %>' />
                                                                        </ItemTemplate>
                                                                        <ItemStyle HorizontalAlign="Center" Width="30px" />
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="<%$ Resources:Attendance,Name %>">
                                                                        <ItemTemplate>
                                                                            <table width="200px">
                                                                                <tr>
                                                                                    <td width="180px">
                                                                                        <%-- <asp:Panel ID="pnldescdetail" runat="server" Width="200px" Height="20px" ScrollBars="Auto">--%>
                                                                                        <asp:HiddenField ID="hdnProductId" runat="server" Value='<%#Eval("Product_Id") %>' />
                                                                                        <cc1:Editor ID="hdnSuggestedProductdesc" Visible="false" runat="server" TopToolbarPreservePlace="false"
                                                                                            Content='<%#Eval("ProductDescription") %>' />
                                                                                        <asp:Label ID="lblgvProductName" runat="server" Text='<%#SuggestedProductName(Eval("Product_Id").ToString(),Eval("Serial_No").ToString()) %>' Width="100px" />
                                                                                        <%--</asp:Panel>--%>
                                                                                    </td>
                                                                                    <td width="20px">
                                                                                        <asp:Image ID="lnkDes" runat="server" ImageUrl="~/Images/detail.png" />
                                                                                        <%--  <asp:ImageButton ID="lnkDes" runat="server" ImageUrl="~/Images/detail.png"  />--%>
                                                                                    </td>
                                                                                </tr>
                                                                                <%--<tr>
                                                                    <td align="left">
                                                                     <asp:LinkButton ID="lnkDes" runat="server" Text="<%$ Resources:Attendance,Description %>"  Font-Underline="false"  Font-Bold="true"></asp:LinkButton>
                                                                  
                                                                    </td>
                                                                    </tr>--%>
                                                                            </table>
                                                                            <br />
                                                                            <asp:Panel ID="PopupMenu1" Width="100%" runat="server">
                                                                                <table border="1" cellpadding="0" cellspacing="0" bordercolor="#c6c6c6">
                                                                                    <tr>
                                                                                        <td>
                                                                                            <table width="314px" height="110" cellspacing="0" bgcolor="#F9F9F9">
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
                                                                                                            <cc1:Editor ID="lblgvProductDescription" runat="server" TopToolbarPreservePlace="false"
                                                                                                                Content='<%#SuggestedProductName(Eval("Product_Id").ToString(),Eval("Serial_No").ToString()) %>' />
                                                                                                        </asp:Panel>
                                                                                                        <br />
                                                                                                    </td>
                                                                                                </tr>
                                                                                            </table>
                                                                                        </td>
                                                                                    </tr>
                                                                                </table>
                                                                            </asp:Panel>
                                                                            <cc1:HoverMenuExtender ID="hme3" runat="Server" TargetControlID="lnkDes" PopupControlID="PopupMenu1"
                                                                                HoverCssClass="popupHover" PopupPosition="Right" OffsetX="0" OffsetY="0" PopDelay="50" />
                                                                        </ItemTemplate>
                                                                        <ItemStyle HorizontalAlign="Center" Width="200px" />
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="<%$ Resources:Attendance,Unit %>">
                                                                        <ItemTemplate>
                                                                            <asp:HiddenField ID="hdnUnitId" runat="server" Value='<%#Eval("UnitId") %>' />
                                                                            <asp:Label ID="lblgvUnit" runat="server" Text='<%#Inv_UnitMaster.GetUnitCode(Eval("UnitId").ToString(),Session["DBConnection"].ToString(),Session["CompId"].ToString())     %>' />
                                                                        </ItemTemplate>
                                                                        <ItemStyle HorizontalAlign="Center" />
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField Visible="false" HeaderText="<%$ Resources:Attendance,Description %>">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblgvProductDescription1" runat="server" Text='<%#Eval("ProductDescription") %>' />
                                                                        </ItemTemplate>
                                                                        <ItemStyle HorizontalAlign="Center" />
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="<%$ Resources:Attendance,Currency %>">
                                                                        <ItemTemplate>
                                                                            <asp:HiddenField ID="hdnCurrencyId" runat="server" Value='<%#Eval("Currency_Id") %>' />
                                                                            <asp:Label ID="lblgvCurrency" runat="server" Text='<%#CurrencyMaster.GetCurrencyNameByCurrencyId(Eval("Currency_Id").ToString(),Session["DBConnection"].ToString()) %>' />
                                                                        </ItemTemplate>
                                                                        <ItemStyle HorizontalAlign="Center" />
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="<%$ Resources:Attendance,Quantity %>">
                                                                        <ItemTemplate>
                                                                            <asp:TextBox ID="lblgvQuantity" onchange="SetSelectedRow(this)" Width="30px" ForeColor="#4d4c4c" runat="server" OnTextChanged="lblgvQuantity_TextChanged"
                                                                                AutoPostBack="true" Text='<%#Eval("Quantity") %>' />
                                                                            <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtenderquantity" runat="server"
                                                                                Enabled="True" TargetControlID="lblgvQuantity" ValidChars="1,2,3,4,5,6,7,8,9,0,.">
                                                                            </cc1:FilteredTextBoxExtender>
                                                                        </ItemTemplate>
                                                                        <ItemStyle HorizontalAlign="Center" />
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="<%$ Resources:Attendance,Estimated %>">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblgvEstimatedUnitPrice" runat="server" Text='<%#Eval("EstimatedUnitPrice") %>' />
                                                                            <br />
                                                                            <asp:LinkButton ID="lnkDeatil" runat="server" Text="<%$ Resources:Attendance,Suggested Sales Price%>"></asp:LinkButton><br />
                                                                            <asp:Panel CssClass="popupMenu" ID="PopupMenu" Width="350px" Height="100px" runat="server">
                                                                                <table border="1" cellpadding="0" cellspacing="0" bordercolor="#c6c6c6">
                                                                                    <tr>
                                                                                        <td>
                                                                                            <table width="314" height="110" cellspacing="0" bgcolor="#F9F9F9">
                                                                                                <tr>
                                                                                                    <td height="21" colspan="2">
                                                                                                        <div align="left" style="background: url(../Images/InvGridHdr.jpg) repeat">
                                                                                                            Details
                                                                                                        </div>
                                                                                                    </td>
                                                                                                </tr>
                                                                                                <tr style="background-color: whitesmoke">
                                                                                                    <td colspan="2" align="left" valign="top">
                                                                                                        <br />
                                                                                                        <table style="height: 100px; width: 300px;">
                                                                                                            <tr>
                                                                                                                <td align='<%= PageControlCommon.ChangeTDForDefaultLeft()%>' valign="top">
                                                                                                                    <asp:Label ID="Label6" runat="server" Text="<%$ Resources:Attendance,Price%>" />
                                                                                                                </td>
                                                                                                                <td align='<%= PageControlCommon.ChangeTDForDefaultLeft()%>' valign="top">:
                                                                                                                </td>
                                                                                                                <td align='<%= PageControlCommon.ChangeTDForDefaultLeft()%>' valign="top">
                                                                                                                    <asp:Label ID="Label8" runat="server" Height="50px" Text='<%#Eval("PurchaseProductPrice") %>' />
                                                                                                                </td>
                                                                                                            </tr>
                                                                                                            <tr>
                                                                                                                <td align='<%= PageControlCommon.ChangeTDForDefaultLeft()%>' valign="top">
                                                                                                                    <asp:Label ID="Label4" runat="server" Text="<%$ Resources:Attendance,Description%>" />
                                                                                                                </td>
                                                                                                                <td align='<%= PageControlCommon.ChangeTDForDefaultLeft()%>' valign="top">:
                                                                                                                </td>
                                                                                                                <td align='<%= PageControlCommon.ChangeTDForDefaultLeft()%>' valign="top">
                                                                                                                    <asp:Literal ID="txtdesc" runat="server" Text='<%#Eval("PurchaseProductDescription") %>'></asp:Literal>
                                                                                                                </td>
                                                                                                            </tr>
                                                                                                        </table>
                                                                                                        <%--<asp:Label ID="lblgvProductDescription" runat="server" Text='<%#Eval("ProductDescription") %>' />--%>
                                                                                                        <br />
                                                                                                    </td>
                                                                                                </tr>
                                                                                            </table>
                                                                                        </td>
                                                                                    </tr>
                                                                                </table>
                                                                            </asp:Panel>
                                                                            <cc1:HoverMenuExtender ID="hme2" runat="Server" TargetControlID="lnkDeatil" PopupControlID="PopupMenu"
                                                                                HoverCssClass="popupHover" PopupPosition="Right" OffsetX="0" OffsetY="0" PopDelay="50" />
                                                                        </ItemTemplate>
                                                                        <ItemStyle HorizontalAlign="Center" />
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="<%$ Resources:Attendance,Price %>">
                                                                        <ItemTemplate>
                                                                            <asp:TextBox ID="txtgvUnitPrice" onchange="SetSelectedRow(this)" Width="50px" runat="server" ForeColor="#4d4c4c"
                                                                                OnTextChanged="txtgvUnitPrice_TextChanged" Text='<%#Eval("SalesPrice") %>' AutoPostBack="true" />
                                                                            <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" runat="server" Enabled="True"
                                                                                TargetControlID="txtgvUnitPrice" ValidChars="1,2,3,4,5,6,7,8,9,0,.">
                                                                            </cc1:FilteredTextBoxExtender>
                                                                        </ItemTemplate>
                                                                        <ItemStyle HorizontalAlign="Center" />
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField>
                                                                        <ItemTemplate>
                                                                            <asp:TextBox ID="txtgvQuantityPrice" onchange="SetSelectedRow(this)" Width="70px" ReadOnly="true" runat="server"
                                                                                ForeColor="#4d4c4c" />
                                                                        </ItemTemplate>
                                                                        <ItemStyle HorizontalAlign="Center" />
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="<%$ Resources:Attendance,% %>">
                                                                        <ItemTemplate>
                                                                            <asp:TextBox ID="txtgvDiscountP" onchange="SetSelectedRow(this)" Width="30px" ForeColor="#4d4c4c" runat="server"
                                                                                OnTextChanged="txtgvDiscountP_TextChanged" AutoPostBack="true" Text='<%#Eval("DiscountPercent") %>' />
                                                                            <%--  <asp:TextBox ID="txtgvDiscountUnitP" Width="30px" ForeColor="#4d4c4c" runat="server"
                                                                            OnTextChanged="txtgvDiscountP_TextChanged" AutoPostBack="true" Text='<%#Eval("DiscountPercent") %>' />--%>
                                                                            <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender4" runat="server" Enabled="True"
                                                                                TargetControlID="txtgvDiscountP" ValidChars="1,2,3,4,5,6,7,8,9,0,.">
                                                                            </cc1:FilteredTextBoxExtender>
                                                                        </ItemTemplate>
                                                                        <ItemStyle HorizontalAlign="Center" />
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="<%$ Resources:Attendance,Value %>">
                                                                        <ItemTemplate>
                                                                            <asp:TextBox ID="txtgvDiscountV" onchange="SetSelectedRow(this)" Width="45px" ForeColor="#4d4c4c" runat="server"
                                                                                OnTextChanged="txtgvDiscountV_TextChanged" AutoPostBack="true" Text='<%#Eval("DiscountValue") %>' />
                                                                            <%-- <asp:TextBox ID="txtgvDiscountUnitV" Width="45px" ForeColor="#4d4c4c" runat="server"
                                                                            OnTextChanged="txtgvDiscountV_TextChanged" AutoPostBack="true" Text='<%# (Convert.ToDouble(Eval("DiscountValue").ToString())/Convert.ToDouble(Eval("Quantity").ToString())) %>' />--%>
                                                                            <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender5" runat="server" Enabled="True"
                                                                                TargetControlID="txtgvDiscountV" ValidChars="1,2,3,4,5,6,7,8,9,0,.">
                                                                            </cc1:FilteredTextBoxExtender>
                                                                        </ItemTemplate>
                                                                        <ItemStyle HorizontalAlign="Center" />
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="<%$ Resources:Attendance,After Price %>" Visible="false">
                                                                        <ItemTemplate>
                                                                            <asp:TextBox ID="txtgvPriceAfterDiscount" onchange="SetSelectedRow(this)" ForeColor="#4d4c4c" Width="60px" ReadOnly="true"
                                                                                runat="server" Text='<%#Eval("PriceAfterDiscount") %>' />
                                                                        </ItemTemplate>
                                                                        <ItemStyle HorizontalAlign="Center" />
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="<%$ Resources:Attendance,% %>">
                                                                        <ItemTemplate>
                                                                            <asp:TextBox ID="txtgvTaxP" Enabled="false" onchange="SetSelectedRow(this)" Width="30px" ForeColor="#4d4c4c" runat="server" OnTextChanged="txtgvTaxP_TextChanged"
                                                                                AutoPostBack="true" Text='<%#Eval("TaxPercent") %>' />
                                                                            <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender2" runat="server" Enabled="True"
                                                                                TargetControlID="txtgvTaxP" ValidChars="1,2,3,4,5,6,7,8,9,0,.">
                                                                            </cc1:FilteredTextBoxExtender>
                                                                            <asp:ImageButton ID="BtnAddTax" runat="server" CommandName="GvDetail" CommandArgument='<%# Eval("Product_Id") %>' OnCommand="BtnAddTax_Command" ImageUrl="~/Images/plus.png" Width="30px" Height="30px" ToolTip="Add Tax" />
                                                                        </ItemTemplate>
                                                                        <ItemStyle HorizontalAlign="Center" />
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="<%$ Resources:Attendance,Value %>">
                                                                        <ItemTemplate>
                                                                            <asp:TextBox ID="txtgvTaxV" onchange="SetSelectedRow(this)" Enabled="false" Width="45px" ForeColor="#4d4c4c" runat="server" OnTextChanged="txtgvTaxV_TextChanged"
                                                                                AutoPostBack="true" Text='<%#Eval("TaxValue") %>' />
                                                                            <%--  <asp:TextBox ID="txtgvTaxUnitV" Width="45px" ForeColor="#4d4c4c" runat="server" OnTextChanged="txtgvTaxV_TextChanged"
                                                                            AutoPostBack="true" Text='<%# (Convert.ToDouble(Eval("TaxValue").ToString())/Convert.ToDouble(Eval("Quantity").ToString())) %>' />--%>
                                                                            <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender3" runat="server" Enabled="True"
                                                                                TargetControlID="txtgvTaxV" ValidChars="1,2,3,4,5,6,7,8,9,0,.">
                                                                            </cc1:FilteredTextBoxExtender>
                                                                        </ItemTemplate>
                                                                        <ItemStyle HorizontalAlign="Center" />
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="<%$ Resources:Attendance,After Price %>" Visible="false">
                                                                        <ItemTemplate>
                                                                            <asp:TextBox ID="txtgvPriceAfterTax" onchange="SetSelectedRow(this)" Width="60px" ForeColor="#4d4c4c" ReadOnly="true"
                                                                                runat="server" Text='<%#Eval("PriceAfterTax") %>' />
                                                                        </ItemTemplate>
                                                                        <ItemStyle HorizontalAlign="Center" />
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="<%$Resources:Attendance,Net Amount %>">
                                                                        <ItemTemplate>
                                                                            <asp:TextBox ID="txtgvTotal" onchange="SetSelectedRow(this)" Width="60px" ForeColor="#4d4c4c" ReadOnly="true" runat="server"
                                                                                Text='<%#Eval("PriceAfterTax") %>' />
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
                                                        <asp:Label ID="lblAmount" runat="server" Text="<%$ Resources:Attendance,Gross Amount%>" />
                                                        <asp:TextBox ID="txtAmount" runat="server" ReadOnly="true" CssClass="form-control" />
                                                        <br />
                                                    </div>
                                                    <div class="col-md-6">
                                                        <asp:Label ID="lblDiscountP" runat="server" Text="<%$ Resources:Attendance,Discount(%) %>" />
                                                        <div class="input-group">
                                                            <asp:TextBox ID="txtDiscountP" runat="server" CssClass="form-control"
                                                                OnTextChanged="txtDiscountP_TextChanged" AutoPostBack="true" />
                                                            <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender8" runat="server" Enabled="True"
                                                                TargetControlID="txtDiscountP" ValidChars="1,2,3,4,5,6,7,8,9,0,.">
                                                            </cc1:FilteredTextBoxExtender>
                                                            <div class="input-group-addon">
                                                                <asp:Label ID="Label3" runat="server" Text="<%$ Resources:Attendance,Value %>" />
                                                            </div>
                                                            <div class="input-group-btn">
                                                                <asp:TextBox ID="txtDiscountV" Width="120px" runat="server" CssClass="form-control"
                                                                    OnTextChanged="txtDiscountV_TextChanged" AutoPostBack="true" />
                                                                <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender9" runat="server" Enabled="True"
                                                                    TargetControlID="txtDiscountV" ValidChars="1,2,3,4,5,6,7,8,9,0,.">
                                                                </cc1:FilteredTextBoxExtender>

                                                            </div>
                                                        </div>
                                                        <br />
                                                    </div>
                                                    <div class="col-md-6">
                                                        <asp:Label ID="lblPriceAfterTax" runat="server" Text="<%$ Resources:Attendance,Price After Tax %>" />
                                                        <asp:TextBox ID="txtPriceAfterTax" runat="server" CssClass="form-control" ReadOnly="True" />
                                                        <br />
                                                    </div>
                                                    <div class="col-md-6">
                                                        <asp:Label ID="lblTaxP" runat="server" Text="<%$ Resources:Attendance,Tax(%) %>" />
                                                        <asp:TextBox ID="txtTaxP" runat="server" CssClass="form-control" OnTextChanged="txtTaxP_TextChanged"
                                                            AutoPostBack="true" />
                                                        <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender6" runat="server" Enabled="True"
                                                            TargetControlID="txtTaxP" ValidChars="1,2,3,4,5,6,7,8,9,0,.">
                                                        </cc1:FilteredTextBoxExtender>
                                                        <asp:Label ID="Label2" runat="server" Text="<%$ Resources:Attendance,Value %>" />
                                                        <asp:TextBox ID="txtTaxV" runat="server" CssClass="form-control" OnTextChanged="txtTaxV_TextChanged"
                                                            AutoPostBack="true" />
                                                        <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender7" runat="server" Enabled="True"
                                                            TargetControlID="txtTaxV" ValidChars="1,2,3,4,5,6,7,8,9,0,.">
                                                        </cc1:FilteredTextBoxExtender>
                                                        <br />
                                                    </div>
                                                    <div class="col-md-6">
                                                        <asp:Label ID="lblTotalAmount" runat="server" Text="<%$ Resources:Attendance, Net Amount %>" />
                                                        <asp:TextBox ID="txtTotalAmount" runat="server" CssClass="form-control" ReadOnly="True" />
                                                        <br />
                                                    </div>
                                                    <div class="col-md-12">
                                                        <cc1:TabContainer ID="TabContainer2" runat="server" ActiveTabIndex="0" CssClass="ajax__tab_yuitabview-theme">
                                                            <cc1:TabPanel ID="TabPanel1" runat="server" HeaderText="<%$ Resources:Attendance,Header %>">
                                                                <ContentTemplate>
                                                                    <asp:UpdatePanel ID="Update_TabPanel1" runat="server">
                                                                        <ContentTemplate>
                                                                            <div class="row">
                                                                                <div class="col-md-12">
                                                                                    <cc1:Editor ID="txtHeader" runat="server" Class="form-control" Height="300px" />
                                                                                    <br />
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
                                                            <cc1:TabPanel ID="TabPanel2" runat="server" HeaderText="<%$ Resources:Attendance,Footer %>">
                                                                <ContentTemplate>
                                                                    <asp:UpdatePanel ID="Update_TabPanel2" runat="server">
                                                                        <ContentTemplate>
                                                                            <div class="row">
                                                                                <div class="col-md-12">
                                                                                    <cc1:Editor ID="txtFooter" runat="server" Class="form-control" Height="300px" />
                                                                                    <br />
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
                                                            <cc1:TabPanel ID="TabPanel3" runat="server" HeaderText="<%$ Resources:Attendance,Terms & Conditions %>">
                                                                <ContentTemplate>
                                                                    <asp:UpdatePanel ID="Update_TabPanel3" runat="server">
                                                                        <ContentTemplate>
                                                                            <div class="row">
                                                                                <div class="col-md-12">
                                                                                    <cc1:Editor ID="txtCondition1" runat="server" Class="form-control" Height="300px" />
                                                                                    <br />
                                                                                </div>
                                                                            </div>
                                                                        </ContentTemplate>
                                                                    </asp:UpdatePanel>
                                                                    <asp:UpdateProgress ID="UpdateProgress7" runat="server" AssociatedUpdatePanelID="Update_TabPanel3">
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
                                                            <cc1:TabPanel ID="TabPanel4" runat="server" HeaderText="<%$ Resources:Attendance,Upload Template%>">
                                                                <ContentTemplate>
                                                                    <asp:UpdatePanel ID="Update_TabPanel4" runat="server">
                                                                        <ContentTemplate>
                                                                            <div class="row">
                                                                                <div class="col-md-6">
                                                                                    <asp:Label ID="Label11" Text="<%$ Resources:Attendance,Image %>" runat="server"></asp:Label>
                                                                                    <div class="input-group" style="width: 100%;">
                                                                                        <cc1:AsyncFileUpload ID="FileUploadImage"
                                                                                            OnClientUploadStarted="FuLogo_UploadStarted"
                                                                                            OnClientUploadError="FuLogo_UploadError"
                                                                                            OnClientUploadComplete="FuLogo_UploadComplete"
                                                                                            OnUploadedComplete="FuLogo_FileUploadComplete"
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
                                                                                        <div class="input-group-btn" style="width: 35px;">
                                                                                            <asp:ImageButton ID="ImgFileUploadAdd" runat="server" CausesValidation="False" Height="30px" Style="margin-top: 5px;"
                                                                                                ImageUrl="~/Images/add.png" OnClick="ImgLogoAdd_Click" Width="30px" ToolTip="<%$ Resources:Attendance,Add %>"
                                                                                                ImageAlign="Middle" />
                                                                                        </div>
                                                                                    </div>
                                                                                    <br />
                                                                                </div>
                                                                                <div class="col-md-6">
                                                                                    <asp:Label ID="Label12" Text="<%$ Resources:Attendance,Template %>" runat="server"></asp:Label>
                                                                                    <div class="input-group" style="width: 100%;">
                                                                                        <cc1:AsyncFileUpload ID="UploadTemplate"
                                                                                            OnClientUploadStarted="FUHtml_UploadStarted"
                                                                                            OnClientUploadError="FUHtml_UploadError"
                                                                                            OnClientUploadComplete="FUHtml_UploadComplete"
                                                                                            OnUploadedComplete="FUHtml_FileUploadComplete"
                                                                                            runat="server" CssClass="form-control"
                                                                                            CompleteBackColor="White"
                                                                                            UploaderStyle="Traditional"
                                                                                            UploadingBackColor="#CCFFFF"
                                                                                            ThrobberID="FUHtml_ImgLoader" Width="100%" />
                                                                                        <div class="input-group-btn" style="border: solid 1px #d2d6de; width: 35px;">
                                                                                            <asp:Image ID="FUHtml_Img_Right" runat="server" Width="30px" Height="30px" ImageUrl="../Images/Allow.png" Style="display: none" />
                                                                                            <asp:Image ID="FUHtml_Img_Wrong" runat="server" Width="30px" Height="30px" ImageUrl="../Images/Delete1.png" Style="display: none" />
                                                                                            <asp:Image ID="FUHtml_ImgLoader" runat="server" ImageUrl="../Images/loader.gif" />
                                                                                        </div>
                                                                                        <div class="input-group-btn">
                                                                                            <asp:ImageButton ID="btnTemplate" runat="server" CausesValidation="False" Height="30px" Style="margin-top: 5px;"
                                                                                                ImageUrl="~/Images/add.png" OnClick="btnTemplate_Click" Width="30px" ToolTip="<%$ Resources:Attendance,Add %>"
                                                                                                ImageAlign="Middle" />
                                                                                        </div>
                                                                                    </div>
                                                                                    <asp:Label ID="lblTemplatePath" runat="server"></asp:Label>
                                                                                    <br />
                                                                                </div>
                                                                                <div class="col-md-12">
                                                                                    <cc1:Editor ID="Editor1" runat="server" Height="300px" />
                                                                                    <br />
                                                                                </div>
                                                                            </div>
                                                                        </ContentTemplate>
                                                                    </asp:UpdatePanel>
                                                                    <asp:UpdateProgress ID="UpdateProgress8" runat="server" AssociatedUpdatePanelID="Update_TabPanel4">
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
                                                    </div>
                                                    <div class="col-md-12" style="text-align: center">
                                                        <br />
                                                        <asp:Button ID="btnSQuoteSave" ValidationGroup="Save" runat="server" Text="<%$ Resources:Attendance,Save %>"
                                                            Visible="false" CssClass="btn btn-success" OnClick="btnSQuoteSave_Click" />

                                                        <asp:Button ID="BtnReset" runat="server" Text="<%$ Resources:Attendance,Reset %>"
                                                            CssClass="btn btn-primary" CausesValidation="False" OnClick="BtnReset_Click" />

                                                        <asp:Button ID="btnSQuoteCancel" runat="server" CssClass="btn btn-danger" Text="<%$ Resources:Attendance,Cancel %>"
                                                            CausesValidation="False" OnClick="btnSQuoteCancel_Click" />

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
                                                    <asp:Label ID="Label14" runat="server" Text="Advance Search"></asp:Label></h3>
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
                                                    <asp:DropDownList ID="ddlFieldNameBin" runat="server" CssClass="form-control">
                                                        <asp:ListItem Text="<%$ Resources:Attendance,Template Name %>" Value="Condition2"></asp:ListItem>
                                                        <asp:ListItem Text="<%$ Resources:Attendance,category %>" Value="CategoryName"></asp:ListItem>
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

                                <div class="box box-warning box-solid" <%= GvSalesQuoteBin.Rows.Count>0?"style='display:block'":"style='display:none'"%>>
                                    <div class="box-body">
                                        <div class="row">
                                            <div class="col-md-12">
                                                <div class="flow">
                                                    <asp:Label ID="lblSelectedRecord" runat="server" Visible="false"></asp:Label>
                                                    <asp:GridView CssClass="table-striped table-bordered table table-hover"
                                                        ID="GvSalesQuoteBin" PageSize='<%# int.Parse(Session["GridSize"].ToString()) %>' runat="server"
                                                        AutoGenerateColumns="False" Width="100%" AllowPaging="True" OnPageIndexChanging="GvSalesQuoteBin_PageIndexChanging"
                                                        OnSorting="GvSalesQuoteBin_OnSorting" AllowSorting="true">
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
                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,Category%>" SortExpression="CategoryName">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblgvcategoryName" runat="server" Text='<%#Eval("CategoryName") %>' />
                                                                </ItemTemplate>
                                                                <ItemStyle HorizontalAlign="Left" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,Template Name %>" SortExpression="Condition2">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblgvtemplateName" runat="server" Text='<%#Eval("Condition2") %>' />
                                                                    <asp:HiddenField ID="hdnTransId" runat="server" Value='<%#Eval("SQuotation_Id") %>' />
                                                                </ItemTemplate>
                                                                <ItemStyle HorizontalAlign="Left" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Template Name (Local)" SortExpression="Condition3">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblgvtemplateName_L" runat="server" Text='<%#Eval("Condition3") %>' />
                                                                </ItemTemplate>
                                                                <ItemStyle HorizontalAlign="Left" />
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


    <div class="modal fade" id="Modal_GST" tabindex="-1" role="dialog" aria-labelledby="Modal_GSTLabel"
        aria-hidden="true">
        <div class="modal-dialog">
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal">
                        <span aria-hidden="true">&times;</span><span class="sr-only">Close</span></button>
                    <h4 class="modal-title" id="Modal_GSTLabel">TAX Calculation
                    </h4>
                </div>
                <div class="modal-body">
                    <asp:UpdatePanel ID="Update_Modal_GST" runat="server">
                        <ContentTemplate>
                            <div class="row">
                                <div class="col-md-12">
                                    <asp:GridView CssClass="table-striped table-bordered table table-hover" ID="gvTaxCalculation" runat="server" Width="100%" AutoGenerateColumns="false">
                                        <Columns>
                                            <asp:TemplateField HeaderText="Tax Name">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblTaxName" runat="server" Text='<%# Eval("Tax_Name") %>' />
                                                    <asp:HiddenField ID="lblgvTaxId" runat="server" Value='<%# Eval("Tax_Id") %>' />
                                                    <asp:HiddenField ID="lblgvProductId" runat="server" Value='<%# Eval("Product_Id") %>' />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Tax Value (%)">
                                                <ItemTemplate>
                                                    <asp:TextBox ID="txtTaxValueInPer" CssClass="form-control" Style="text-align: center;" runat="server" Text='<%# Eval("Tax_Value") %>' AutoPostBack="true" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>



                                    </asp:GridView>
                                </div>
                            </div>
                        </ContentTemplate>
                    </asp:UpdatePanel>

                </div>
                <div class="modal-footer">
                    <asp:UpdatePanel ID="Update_Modal_Button_GST" runat="server">
                        <ContentTemplate>
                            <asp:HiddenField ID="Hdn_unit_Price_Tax" runat="server" />
                            <asp:HiddenField ID="Hdn_Discount_Tax" runat="server" />
                            <asp:HiddenField ID="Hdn_Serial_No_Tax" runat="server" />
                            <asp:HiddenField ID="Hdn_Product_Id_Tax" runat="server" />
                            <asp:Button ID="Btn_Update_Tax" runat="server" CssClass="btn btn-primary"
                                Text="<%$ Resources:Attendance,Update %>" OnClick="Btn_Update_Tax_Click" />
                            <button id="btnClosePopup" type="button" class="btn btn-danger" data-dismiss="modal">Close</button>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
            </div>
        </div>
    </div>

    <asp:UpdateProgress ID="UpdateProgress4" runat="server" AssociatedUpdatePanelID="Update_Modal_GST">
        <ProgressTemplate>
            <div class="modal_Progress">
                <div class="center_Progress">
                    <img alt="" src="../Images/ajax-loader2.gif" style="vertical-align: middle; background-color: White; border-radius: 10px;" />
                </div>
            </div>
        </ProgressTemplate>
    </asp:UpdateProgress>

    <asp:UpdateProgress ID="UpdateProgress9" runat="server" AssociatedUpdatePanelID="Update_Modal_Button_GST">
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
    </script>
    <script type="text/javascript">
        function postBackByObject() {
            var o = window.event.srcElement;
            if (o.tagName == "INPUT" && o.type == "checkbox") {
                __doPostBack("", "");
            }
        }
        var lasttab = 0;
        function tabChanged(sender, args) {
            lasttab = sender.get_activeTabIndex();
        }
    </script>
    <script type="text/javascript">
        function FuLogo_UploadComplete(sender, args) {
            document.getElementById('<%= FULogo_Img_Wrong.ClientID %>').style.display = "none";
            document.getElementById('<%= FULogo_Img_Right.ClientID %>').style.display = "";
        }
        function FuLogo_UploadError(sender, args) {
            document.getElementById('<%= FULogo_Img_Right.ClientID %>').style.display = "none";
            document.getElementById('<%= FULogo_Img_Wrong.ClientID %>').style.display = "";
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

        function FUHtml_UploadComplete(sender, args) {
            document.getElementById('<%= FUHtml_Img_Wrong.ClientID %>').style.display = "none";
            document.getElementById('<%= FUHtml_Img_Right.ClientID %>').style.display = "";
        }
        function FUHtml_UploadError(sender, args) {
            document.getElementById('<%= FUHtml_Img_Right.ClientID %>').style.display = "none";
            document.getElementById('<%= FUHtml_Img_Wrong.ClientID %>').style.display = "";
            alert('Invalid File Type, Select Only .html extension file');
        }
        function FUHtml_UploadStarted(sender, args) {
            var filename = args.get_fileName();

            var filext = filename.substring(filename.lastIndexOf(".") + 1);
            filext = filext.toLowerCase();
            if (filext == "html") {
                return true;
            }
            else {
                throw {
                    name: "Invalid File Type",
                    level: "Error",
                    message: "Invalid File Type, Select Only .html extension file",
                    htmlMessage: "Invalid File Type, Select Only .html extension file"
                }
                return false;
            }
        }
        function setScrollAndRow() {
            try {
                debugger;
                var rowIndex = $('#<%= hdfCurrentRow.ClientID %>').val();
                var parent = document.getElementById('<%= GvDetail.ClientID %>');
                var rowIndex = parseInt(rowIndex);
                parent.rows[rowIndex + 1].style.backgroundColor = "#A1DCF2";
                var h = document.getElementById("<%=hfScrollPosition.ClientID%>");
                document.getElementById("<%=scrollArea.ClientID%>").scrollTop = h.value;

            }
            catch (e) {

            }
        }

function SetDivPosition() {
            var intY = document.getElementById("<%=scrollArea.ClientID%>").scrollTop;
            var h = document.getElementById("<%=hfScrollPosition.ClientID%>");
            h.value = intY;
        }


function SetSelectedRow(lnk) {
            //Reference the GridView Row.
            var row = lnk.parentNode.parentNode;
            $('#<%= hdfCurrentRow.ClientID %>').val(row.rowIndex - 1);
            row.style.backgroundColor = "#A1DCF2";
        }

        function Show_Modal_GST() {
            document.getElementById('<%= Btn_GST_Modal.ClientID %>').click();
        }
    </script>


</asp:Content>
