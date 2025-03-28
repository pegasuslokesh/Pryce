<%@ Page Title="" Language="C#" MasterPageFile="~/ERPMaster.master" AutoEventWireup="true" CodeFile="PurchaseGoodsRec.aspx.cs" Inherits="Purchase_PurchaseGoodsRec" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Src="~/WebUserControl/FileUpload.ascx" TagPrefix="AT1" TagName="FileUpload1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="Head" runat="Server">
    <script type="text/javascript">
        function LI_Edit_Active() {
            $("#Li_List").removeClass("active");
            $("#List").removeClass("active");

            $("#Li_New").addClass("active");
            $("#New").addClass("active");
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="HeaderContent" runat="Server">
    <h1>
        <i class="fas fa-truck-loading"></i>&nbsp;&nbsp;
        <asp:Label ID="lblHeader" runat="server" Text="<%$ Resources:Attendance,Purchase Goods Receive%>"></asp:Label>
    </h1>
    <ol class="breadcrumb">
        <li><a href="#"><i class="fa fa-dashboard"></i>
            <asp:Label ID="Label6" runat="server" Text="<%$ Resources:Attendance,Inventory%>"></asp:Label></a></li>
        <li><a href="#"><i class="fa fa-dashboard"></i>
            <asp:Label ID="Label10" runat="server" Text="<%$ Resources:Attendance,Purchase%>"></asp:Label></a></li>
        <li class="active">
            <asp:Label ID="Label5" runat="server" Text="<%$ Resources:Attendance,Goods Receive%>"></asp:Label></li>
    </ol>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="Server">
    <asp:UpdatePanel ID="Update_Button" runat="server">
        <ContentTemplate>
            <asp:Button ID="Btn_New" Style="display: none;" runat="server" OnClick="btnNew_Click" Text="New" />
            <asp:Button ID="Btn_Pending_Order" Style="display: none;" runat="server" OnClick="btnPendingOrder_Click" Text="Pending Order" />
            <asp:Button ID="Btn_View_Modal" Style="display: none;" runat="server" data-toggle="modal" data-target="#Return_Modal" Text="View Modal" />
            <asp:Button ID="Btn_Modal_FileUpload" Style="display: none;" runat="server" data-toggle="modal" data-target="#Fileupload123" Text="FileUpload" />
            <asp:HiddenField runat="server" ID="hdnCanPrint" />
            <asp:HiddenField runat="server" ID="hdnCanView" />
            <asp:HiddenField runat="server" ID="hdnCanEdit" />
            <asp:HiddenField runat="server" ID="hdnCanUpload" />
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
                    <li id="Li_Pending_Order"><a href="#Pending_Order" onclick="Li_Tab_Pending_Order()" data-toggle="tab">
                        <i class="fa fa-trash"></i>&nbsp;&nbsp;<asp:Label ID="Label13" runat="server" Text="<%$ Resources:Attendance,Pending Order%>"></asp:Label></a></li>
                    <li id="Li_New"><a onclick="Li_Tab_New()" href="#New" data-toggle="tab">
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
                                <asp:AsyncPostBackTrigger ControlID="btnSave" EventName="Click" />
                            </Triggers>
                            <ContentTemplate>


                                <div class="row">
                                    <div class="col-md-12">
                                        <div id="Div1" runat="server" class="box box-info collapsed-box">
                                            <div class="box-header with-border">
                                                <h3 class="box-title">
                                                    <asp:Label ID="Label27" runat="server" Text="Advance Search"></asp:Label></h3>
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
                                                    <asp:DropDownList ID="ddlLocList" runat="server" Class="form-control" OnSelectedIndexChanged="DropDownList1_SelectedIndexChanged" AutoPostBack="true">
                                                    </asp:DropDownList>
                                                    <br />
                                                </div>
                                                <div class="col-lg-2">
                                                    <asp:DropDownList ID="ddlPosted" runat="server" CssClass="form-control" AutoPostBack="true" OnSelectedIndexChanged="ddlPosted_SelectedIndexChanged">
                                                        <asp:ListItem Text="<%$ Resources:Attendance,Posted %>" Value="Posted"></asp:ListItem>
                                                        <asp:ListItem Text="<%$ Resources:Attendance,UnPosted %>" Value="UnPosted" Selected="True"></asp:ListItem>
                                                        <asp:ListItem Text="<%$ Resources:Attendance,All %>" Value="0"></asp:ListItem>
                                                    </asp:DropDownList>
                                                </div>
                                                <div class="col-lg-2">
                                                    <asp:DropDownList ID="ddlFieldName" runat="server" CssClass="form-control" OnSelectedIndexChanged="ddlFieldName_SelectedIndexChanged" AutoPostBack="true">
                                                        <asp:ListItem Selected="True" Text="<%$ Resources:Attendance,Invoice No %>" Value="InvoiceNo"></asp:ListItem>
                                                        <asp:ListItem Text="<%$ Resources:Attendance,Invoice Date %>" Value="InvoiceDate"></asp:ListItem>
                                                        <asp:ListItem Text="<%$ Resources:Attendance,Purchase Order No %>" Value="OrderList"></asp:ListItem>
                                                        <asp:ListItem Text="<%$ Resources:Attendance,Supplier Invoice No %>" Value="SupInvoiceNo"></asp:ListItem>
                                                        <asp:ListItem Text="<%$ Resources:Attendance,Supplier Invoice Date %>" Value="SupInvoiceDate"></asp:ListItem>
                                                        <asp:ListItem Text="<%$ Resources:Attendance,Supplier %>" Value="Supplier_Name"></asp:ListItem>
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
                                                    <asp:Panel ID="Panel1" runat="server" DefaultButton="btnbind">
                                                        <asp:TextBox ID="txtValue" runat="server" CssClass="form-control" placeholder="Search From Content"></asp:TextBox>
                                                        <asp:TextBox ID="txtValueDate" runat="server" CssClass="form-control" Visible="false" placeholder="Search From Date"></asp:TextBox>
                                                        <cc1:CalendarExtender OnClientShown="showCalendar" ID="CalendarExtendertxtValueDate" runat="server" TargetControlID="txtValueDate" />
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
                                <div class="box box-warning box-solid" <%= GvPurchaseInvoice.Rows.Count>0?"style='display:block'":"style='display:none'"%>>
                                    <div class="box-body">
                                        <div class="row">
                                            <div class="col-md-12">
                                                <asp:HiddenField ID="HDFSort" runat="server" />
                                                <asp:HiddenField ID="HdnEdit" runat="server" />
                                                <div class="flow">
                                                    <asp:GridView CssClass="table-striped table-bordered table table-hover" ID="GvPurchaseInvoice" PageSize="<%# PageControlCommon.GetPageSize() %>"
                                                        runat="server" AutoGenerateColumns="False" Width="100%" AllowPaging="false" AllowSorting="True" CurrentSortField="InvoiceDate" CurrentSortDirection="DESC"
                                                        OnSorting="GvPurchaseInvocie_OnSorting">
                                                        <Columns>
                                                            <asp:TemplateField HeaderText="Action">
                                                                <ItemTemplate>
                                                                    <div class="dropdown" style="position: absolute;">
                                                                        <button class="btn btn-default dropdown-toggle" type="button" data-toggle="dropdown">
                                                                            <i class="fa fa-ellipsis-h" aria-hidden="true"></i>
                                                                        </button>
                                                                        <ul class="dropdown-menu">
                                                                            <li <%= hdnCanPrint.Value=="true"?"style='display:block'":"style='display:none'" %>>
                                                                                <asp:LinkButton ID="IbtnPrint" runat="server" CausesValidation="False" CommandArgument='<%# Eval("TransId") %>' OnCommand="IbtnPrint_Command"><i class="fa fa-print"></i>Print</asp:LinkButton>
                                                                            </li>

                                                                            <li <%= hdnCanView.Value=="true"?"style='display:block'":"style='display:none'" %>>
                                                                                <asp:LinkButton ID="lnkViewDetail" runat="server" CommandArgument='<%# Eval("TransId")+","+Eval("Location_id") %>' OnCommand="lnkViewDetail_Command" CausesValidation="False" CommandName='<%# Eval("Post") %>'><i class="fa fa-eye"></i>View</asp:LinkButton>
                                                                            </li>

                                                                            <li <%= hdnCanEdit.Value=="true"?"style='display:block'":"style='display:none'" %>>
                                                                                <asp:LinkButton ID="btnEdit" runat="server" CommandArgument='<%# Eval("TransId")+","+Eval("Location_id") %>' CausesValidation="False" OnCommand="btnEdit_Command" CommandName='<%# Eval("Post") %>'><i class="fa fa-pencil"></i>Edit </asp:LinkButton>
                                                                            </li>
                                                                            <li <%= hdnCanUpload.Value=="true"?"style='display:block'":"style='display:none'" %>>
                                                                                <asp:LinkButton ID="lbtnFileUpload" runat="server" CommandArgument='<%# Eval("TransId") %>' CommandName='<%# Eval("Location_id") %>' CausesValidation="False" OnCommand="lbtnFileUpload_Command"><i class="fa fa-upload"></i>File Upload</asp:LinkButton>
                                                                            </li>
                                                                        </ul>
                                                                    </div>

                                                                </ItemTemplate>
                                                            </asp:TemplateField>

                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,Invoice No %>" SortExpression="InvoiceNo">
                                                                <ItemTemplate>
                                                                    <%# Eval("InvoiceNo") %>
                                                                </ItemTemplate>
                                                                <ItemStyle />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,Invoice Date %>" SortExpression="InvoiceDate">
                                                                <ItemTemplate>
                                                                    <%# Convert.ToDateTime(Eval("InvoiceDate").ToString()).ToString("dd-MMM-yyyy") %>
                                                                </ItemTemplate>
                                                                <ItemStyle />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,Purchase Order No %>" SortExpression="OrderList">
                                                                <ItemTemplate>
                                                                    <%# Eval("OrderList") %>
                                                                </ItemTemplate>
                                                                <ItemStyle />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,Supplier Invoice No %>" SortExpression="SupInvoiceNo">
                                                                <ItemTemplate>
                                                                    <%# Eval("SupInvoiceNo") %>
                                                                </ItemTemplate>
                                                                <ItemStyle />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,Supplier Invoice Date %>"
                                                                SortExpression="SupInvoiceDate">
                                                                <ItemTemplate>
                                                                    <%# Convert.ToDateTime(Eval("SupInvoiceDate").ToString()).ToString("dd-MMM-yyyy") %>
                                                                </ItemTemplate>
                                                                <ItemStyle />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,Supplier Name %>" SortExpression="Supplier_Name">
                                                                <ItemTemplate>
                                                                    <%# Eval("Supplier_Name").ToString() %>
                                                                </ItemTemplate>
                                                                <ItemStyle />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,Created By %>" SortExpression="CreatedUser">
                                                                <ItemTemplate>
                                                                    <%#Eval("CreatedUser") %>
                                                                </ItemTemplate>
                                                                <ItemStyle HorizontalAlign="Left" Width="10%" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,Modified By %>" SortExpression="ModifiedUser">
                                                                <ItemTemplate>
                                                                    <%# Eval("ModifiedUser") %>
                                                                </ItemTemplate>
                                                                <ItemStyle HorizontalAlign="Left" Width="10%" />
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
                                                <asp:HiddenField ID="hdnGvPurchaseInvoiceCurrentPageIndex" runat="server" Value="1" />
                                            </div>
                                        </div>
                                    </div>
                                    <!-- /.box-body -->
                                </div>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </div>
                    <div class="tab-pane" id="New">
                        <asp:UpdatePanel ID="Update_New" runat="server" UpdateMode="Conditional">
                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="GvPurchaseInvoice" />
                                <asp:AsyncPostBackTrigger ControlID="Btn_New" EventName="Click" />
                                <asp:AsyncPostBackTrigger ControlID="BtnSerialSave" EventName="Click" />
                                <asp:AsyncPostBackTrigger ControlID="btnResetSerial" EventName="Click" />
                            </Triggers>
                            <ContentTemplate>
                                <div class="row">
                                    <div class="col-md-12">
                                        <div class="box box-primary">
                                            <div class="box-body">
                                                <div class="form-group">
                                                    <div id="PnlProductSearching" runat="server" visible="false">
                                                        <div class="col-md-6">
                                                            <asp:Label ID="Label22" runat="server" Text="<%$ Resources:Attendance,Supplier Name %>"></asp:Label>
                                                            <a style="color: Red">*</a>
                                                            <asp:RequiredFieldValidator EnableClientScript="true" Style="float: right;" runat="server" ID="RequiredFieldValidator3" ValidationGroup="Save"
                                                                Display="Dynamic" SetFocusOnError="true" ControlToValidate="txtSupplierName" ErrorMessage="<%$ Resources:Attendance,Enter Supplier Name %>"></asp:RequiredFieldValidator>

                                                            <asp:TextBox ID="txtSupplierName" runat="server" CssClass="form-control" BackColor="#eeeeee"
                                                                AutoPostBack="True" OnTextChanged="txtSupplierName_TextChanged" />
                                                            <cc1:AutoCompleteExtender ID="txtSupplierName_AutoCompleteExtender" runat="server"
                                                                CompletionInterval="100" DelimiterCharacters="" Enabled="True" MinimumPrefixLength="1"
                                                                ServiceMethod="GetCompletionList" ServicePath="" TargetControlID="txtSupplierName"
                                                                UseContextKey="True" CompletionListCssClass="autoCompleteList" CompletionListItemCssClass="autoCompleteListItem" CompletionListHighlightedItemCssClass="autoCompleteSelectedListItem" OnClientShown="resetPosition">
                                                            </cc1:AutoCompleteExtender>
                                                            <br />
                                                        </div>
                                                        <div class="col-md-12">


                                                            <div class="row">
                                                                <div class="col-md-12">
                                                                    <div id="Div2" runat="server" class="box box-info collapsed-box">
                                                                        <div class="box-header with-border">
                                                                            <h3 class="box-title">
                                                                                <asp:Label ID="Label28" runat="server" Text="Product Search"></asp:Label></h3>
                                                                            <div class="box-tools pull-right">
                                                                                <button type="button" class="btn btn-box-tool" data-widget="collapse">
                                                                                    <i id="I2" runat="server" class="fa fa-plus"></i>
                                                                                </button>
                                                                            </div>
                                                                        </div>
                                                                        <div class="box-body">
                                                                            <div class="col-lg-4">
                                                                                <asp:DropDownList ID="ddlProductSerach" runat="server" CssClass="form-control">
                                                                                    <asp:ListItem Text="<%$ Resources:Attendance,Product Id %>" Value="ProductCode"></asp:ListItem>
                                                                                    <asp:ListItem Text="<%$ Resources:Attendance,Product Name %>" Value="ProductName" Selected="True"></asp:ListItem>
                                                                                    <asp:ListItem Text="<%$ Resources:Attendance,Order No. %>" Value="PurchaseOrderNo"></asp:ListItem>
                                                                                </asp:DropDownList>
                                                                            </div>
                                                                            <div class="col-lg-6">
                                                                                <asp:TextBox ID="txtProductSerachValue" runat="server" CssClass="form-control" placeholder="Search From Content" />
                                                                            </div>
                                                                            <div class="col-lg-2">
                                                                                <asp:LinkButton ID="imgbtnsearch" runat="server" CausesValidation="False" OnClick="imgbtnsearch_Click" ToolTip="<%$ Resources:Attendance,Search %>"><span class="fa fa-search"  style="font-size:25px;"></span></asp:LinkButton>&nbsp;&nbsp;&nbsp;
                                                                                <asp:LinkButton ID="ImgbtnRefresh" runat="server" CausesValidation="False" OnClick="ImgbtnRefresh_Click" ToolTip="<%$ Resources:Attendance,Refresh %>"><span class="fa fa-repeat"  style="font-size:25px;"></span></asp:LinkButton>
                                                                            </div>

                                                                        </div>
                                                                    </div>
                                                                </div>
                                                            </div>


                                                            <div style="overflow: auto; min-height: 0px; max-height: 300px">
                                                                <asp:HiddenField ID="Hdn_Invoice_Currency" Value="0" runat="server" />
                                                                <asp:HiddenField ID="Hdn_Currency_Match" Value="0" runat="server" />
                                                                <asp:GridView CssClass="table-striped table-bordered table table-hover" ID="gvSerachGrid" runat="server" AutoGenerateColumns="False" Width="100%">
                                                                    <Columns>
                                                                        <asp:TemplateField HeaderStyle-HorizontalAlign="Center">
                                                                            <ItemTemplate>
                                                                                <asp:CheckBox ID="chkTrandId" runat="server" AutoPostBack="true" OnCheckedChanged="chkTrandId_CheckedChanged" />
                                                                                <asp:Label ID="TransId" runat="server" Text='<%# Eval("Trans_Id") %>' Visible="false"></asp:Label>
                                                                            </ItemTemplate>
                                                                            <HeaderTemplate>
                                                                                <asp:CheckBox ID="chkgvSelectAll" runat="server" OnCheckedChanged="chkgvSelectAll_CheckedChanged"
                                                                                    AutoPostBack="true" />
                                                                            </HeaderTemplate>
                                                                            <ItemStyle HorizontalAlign="Center" />
                                                                            <ItemStyle />
                                                                            <FooterStyle BorderStyle="None" />
                                                                        </asp:TemplateField>
                                                                        <asp:TemplateField HeaderStyle-HorizontalAlign="Center" HeaderText="Purchase Order No">
                                                                            <ItemTemplate>
                                                                                <asp:Label ID="lblPONo" runat="server" Text='<%# Eval("PurchaseOrderNo") %>'></asp:Label>
                                                                            </ItemTemplate>
                                                                            <ItemStyle HorizontalAlign="Center" />
                                                                            <ItemStyle />
                                                                            <FooterStyle BorderStyle="None" />
                                                                        </asp:TemplateField>
                                                                        <asp:TemplateField HeaderText="<%$ Resources:Attendance,Product Id%>">
                                                                            <ItemTemplate>
                                                                                <asp:Label ID="lblproductcode" runat="server" Text='<%# new Inv_ProductMaster(Session["DBConnection"].ToString()).GetProductCodebyProductId(Eval("ProductId").ToString()) %>'></asp:Label>
                                                                            </ItemTemplate>
                                                                            <FooterStyle BorderStyle="None" />
                                                                            <ItemStyle />
                                                                        </asp:TemplateField>
                                                                        <asp:TemplateField HeaderStyle-HorizontalAlign="Center" HeaderText="<%$ Resources:Attendance,Product Name %>">
                                                                            <ItemTemplate>
                                                                                <asp:Label ID="lblProductId" runat="server" Text='<%# new Inv_ProductMaster(Session["DBConnection"].ToString()).GetProductNamebyProductId(Eval("ProductId").ToString()) %>'></asp:Label>
                                                                                <asp:Label ID="lblgvProductId" Visible="false" runat="server" Text='<%# Eval("ProductId") %>'></asp:Label>
                                                                            </ItemTemplate>
                                                                            <FooterStyle BorderStyle="None" />
                                                                            <ItemStyle />
                                                                        </asp:TemplateField>
                                                                        <asp:TemplateField HeaderStyle-HorizontalAlign="Center" HeaderText="<%$ Resources:Attendance, Unit %>">
                                                                            <ItemTemplate>
                                                                                <asp:Label ID="lblUnitID" runat="server" Text='<%# Eval("UnitId").ToString() %>'
                                                                                    Visible="false"></asp:Label>
                                                                                <asp:Label ID="lblUnit" runat="server" Text='<%#Inv_UnitMaster.GetUnitCode(Eval("UnitId").ToString(),Session["DBConnection"].ToString(),HttpContext.Current.Session["CompId"].ToString()) %>'></asp:Label>
                                                                            </ItemTemplate>
                                                                            <FooterStyle BorderStyle="None" />
                                                                            <ItemStyle />
                                                                        </asp:TemplateField>
                                                                        <asp:TemplateField HeaderStyle-HorizontalAlign="Center" HeaderText="<%$ Resources:Attendance,Order Quantity %>">
                                                                            <ItemTemplate>
                                                                                <asp:Label ID="lblorderqty" runat="server" Text='<%# GetAmountDecimal(Eval("Orderqty").ToString())%>'></asp:Label>
                                                                            </ItemTemplate>
                                                                            <FooterStyle BorderStyle="None" />
                                                                            <ItemStyle HorizontalAlign="Center" />
                                                                        </asp:TemplateField>
                                                                        <asp:TemplateField HeaderText="<%$ Resources:Attendance,Free Quantity %>" HeaderStyle-HorizontalAlign="Center">
                                                                            <ItemTemplate>
                                                                                <asp:Label ID="lblFreeQty" runat="server" Text='<%# GetAmountDecimal(Eval("FreeQty").ToString()) %>'></asp:Label>
                                                                            </ItemTemplate>
                                                                            <FooterStyle BorderStyle="None" />
                                                                            <ItemStyle HorizontalAlign="Center" />
                                                                        </asp:TemplateField>
                                                                    </Columns>
                                                                    <PagerStyle CssClass="pagination-ys" />
                                                                </asp:GridView>
                                                            </div>
                                                        </div>
                                                    </div>
                                                    <br />
                                                    <div class="col-md-12">
                                                        <div style="overflow: auto">
                                                            <asp:GridView CssClass="table-striped table-bordered table table-hover" ID="gvInvoice" PageSize="<%# PageControlCommon.GetPageSize() %>" runat="server"
                                                                AutoGenerateColumns="False" Width="100%" ShowHeader="false">
                                                                <Columns>
                                                                    <asp:TemplateField>
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lbInvNo" runat="server" Font-Bold="true" Font-Size="14px" Text="<%$ Resources:Attendance,Invoice No %>"></asp:Label>
                                                                            :
                                                        <asp:Label ID="lbPONo" runat="server" Font-Bold="true" Font-Size="14px" Text='<%# Eval("InvoiceNo") %>'></asp:Label>
                                                                        </ItemTemplate>
                                                                        <ItemStyle />
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField>
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lbInvDate" runat="server" Font-Bold="true" Font-Size="14px" Text="<%$ Resources:Attendance,Invoice Date %>"></asp:Label>
                                                                            :
                                                        <asp:Label ID="lblPODate" runat="server" Font-Bold="true" Font-Size="14px" Text='<%# Convert.ToDateTime(Eval("InvoiceDate").ToString()).ToString("dd-MMM-yyyy") %>'></asp:Label>
                                                                        </ItemTemplate>
                                                                        <ItemStyle />
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField>
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lbSupInvoiceNo" runat="server" Font-Bold="true" Font-Size="14px" Text="<%$ Resources:Attendance,Supplier Invoice No %>"></asp:Label>
                                                                            :
                                                        <asp:Label ID="lblDeliveryDate" runat="server" Font-Bold="true" Font-Size="14px"
                                                            Text='<%# Eval("SupInvoiceNo") %>'></asp:Label>
                                                                        </ItemTemplate>
                                                                        <ItemStyle />
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField>
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lbSupInvoiceDate" runat="server" Font-Bold="true" Font-Size="14px"
                                                                                Text="<%$ Resources:Attendance,Supplier Invoice Date %>"></asp:Label>
                                                                            :
                                                        <asp:Label ID="lblOrderType" runat="server" Font-Bold="true" Font-Size="14px" Text='<%# Convert.ToDateTime(Eval("SupInvoiceDate").ToString()).ToString("dd-MMM-yyyy") %>'></asp:Label>
                                                                        </ItemTemplate>
                                                                        <ItemStyle />
                                                                    </asp:TemplateField>
                                                                </Columns>


                                                                <PagerStyle CssClass="pagination-ys" />

                                                            </asp:GridView>
                                                        </div>
                                                        <br />
                                                        <div style="overflow: auto">
                                                            <asp:GridView CssClass="table-striped table-bordered table table-hover" ID="GvProduct" runat="server" AutoGenerateColumns="False" Width="100%"
                                                                OnRowDataBound="GvProduct_OnRowDataBound">
                                                                <Columns>
                                                                    <asp:TemplateField HeaderStyle-HorizontalAlign="Center" HeaderText="<%$ Resources:Attendance,Delete %>">
                                                                        <ItemTemplate>
                                                                            <asp:ImageButton ID="IbtnPDDelete" runat="server" CausesValidation="False" CommandArgument='<%# Eval("TransID") %>'
                                                                                ImageUrl="~/Images/Erase.png" OnCommand="IbtnPDDelete_Command" ToolTip="<%$ Resources:Attendance,Delete %>" />
                                                                        </ItemTemplate>
                                                                        <ItemStyle HorizontalAlign="Center" />
                                                                        <ItemStyle />
                                                                        <FooterStyle BorderStyle="None" />
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="<%$ Resources:Attendance,Order No. %>">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblPoNO" runat="server" Text='<%# Eval("PurchaseOrderNo") %>'></asp:Label>
                                                                            <asp:Label ID="lblPoID" Visible="false" runat="server" Text='<%# Eval("POId") %>'></asp:Label>
                                                                        </ItemTemplate>
                                                                        <FooterStyle BorderStyle="None" />
                                                                        <ItemStyle Width="6%" />
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="<%$ Resources:Attendance,Product Id%>">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblproductcode" runat="server" Text='<%#Eval("ProductCode")%>'></asp:Label>
                                                                        </ItemTemplate>
                                                                        <ItemStyle />
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="<%$ Resources:Attendance,Product Name %>">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblProductId" runat="server" Text='<%#Eval("ProductName")%>'></asp:Label>
                                                                            <asp:Label ID="lblTransID" Visible="false" runat="server" Text='<%# Eval("TransID") %>'></asp:Label>
                                                                            <asp:HiddenField ID="hdnProductId" runat="server" Value='<%# Eval("ProductId") %>' />
                                                                        </ItemTemplate>
                                                                        <FooterStyle BorderStyle="None" />
                                                                        <ItemStyle Width="30%" />
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="<%$ Resources:Attendance,Unit Name %>">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblUnit" runat="server" Text='<%#Inv_UnitMaster.GetUnitCode(Eval("UnitId").ToString(),Session["DBConnection"].ToString(),HttpContext.Current.Session["CompId"].ToString())%>'></asp:Label>
                                                                            <asp:HiddenField ID="hdnUnitId" runat="server" Value='<%# Eval("UnitId") %>' />
                                                                            <asp:HiddenField ID="hdnUnitCost" runat="server" Value='<%# Eval("UnitCost") %>' />
                                                                            <asp:HiddenField ID="hdnExchangeRate" runat="server" Value='<%# Eval("CurrencyRate") %>' />
                                                                        </ItemTemplate>
                                                                        <FooterStyle BorderStyle="None" />
                                                                        <ItemStyle />
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderStyle-HorizontalAlign="Center" HeaderText="Order Qty">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblorderqty" runat="server" Text='<%# GetAmountDecimal(Eval("OrderQty").ToString()) %>'></asp:Label>
                                                                        </ItemTemplate>
                                                                        <FooterStyle BorderStyle="None" />
                                                                        <ItemStyle />
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderStyle-HorizontalAlign="Center" HeaderText="<%$ Resources:Attendance,Free %>">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblfreeqty" runat="server" Text='<%# GetAmountDecimal(Eval("FreeQty").ToString()) %>'></asp:Label>
                                                                        </ItemTemplate>
                                                                        <FooterStyle BorderStyle="None" />
                                                                        <ItemStyle />
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="<%$ Resources:Attendance,Invoice Qty %>">
                                                                        <ItemTemplate>
                                                                            <asp:TextBox ID="QtyReceived" runat="server" Text='<%# GetAmountDecimal(Eval("InvoiceQty").ToString()) %>' Width="60px"></asp:TextBox>
                                                                            <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtenderFreeQty" runat="server" Enabled="True"
                                                                                TargetControlID="QtyReceived" ValidChars="1,2,3,4,5,6,7,8,9,0,.">
                                                                            </cc1:FilteredTextBoxExtender>
                                                                            <%--<asp:Label ID="QtyReceived" runat="server" Text='<%# Eval("InvoiceQty") %>'></asp:Label>--%>
                                                                        </ItemTemplate>
                                                                        <FooterStyle BorderStyle="None" />
                                                                        <ItemStyle />
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="<%$ Resources:Attendance,Total Received Qty %>">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="QtyField1" runat="server" Text='<%# GetAmountDecimal(Eval("RecQty").ToString())%>'></asp:Label>
                                                                        </ItemTemplate>
                                                                        <FooterStyle BorderStyle="None" />
                                                                        <ItemStyle />
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="<%$ Resources:Attendance,Received %>">
                                                                        <ItemTemplate>
                                                                            <%-- AutoPostBack="True" OnTextChanged="txtRecQty_TextChanged"--%>
                                                                            <asp:TextBox ID="txtRecQty" runat="server" Width="60px"></asp:TextBox>
                                                                            <asp:LinkButton ID="lnkAddSerial" AutoPostBack="True" runat="server" Text="<%$ Resources:Attendance,Add %>"
                                                                                OnCommand="lnkAddSerial_Command" CommandArgument='<%# Eval("ProductId") %>' ForeColor="Blue"
                                                                                Font-Underline="true"></asp:LinkButton>
                                                                            <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtenderQtyGoodReceived" runat="server"
                                                                                Enabled="True" TargetControlID="txtRecQty" ValidChars="1,2,3,4,5,6,7,8,9,0,.">
                                                                            </cc1:FilteredTextBoxExtender>
                                                                        </ItemTemplate>
                                                                        <FooterStyle BorderStyle="None" />
                                                                        <ItemStyle />
                                                                    </asp:TemplateField>

                                                                    <asp:TemplateField HeaderStyle-HorizontalAlign="Center" Visible="false" HeaderText="<%$ Resources:Attendance,Discount (%) %>">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblTaxAfterPrice" Visible="false" runat="server" Text="0"></asp:Label>
                                                                            <asp:TextBox ID="lblDiscount" runat="server" Enabled="false" Text='<%# GetAmountDecimal(Eval("DisPercentage").ToString()) %>'
                                                                                CssClass="form-control"></asp:TextBox>
                                                                        </ItemTemplate>
                                                                        <FooterStyle BorderStyle="None" />
                                                                        <ItemStyle />
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderStyle-HorizontalAlign="Center" Visible="false" HeaderText="<%$ Resources:Attendance,Discount Value %>">
                                                                        <ItemTemplate>
                                                                            <asp:TextBox ID="lblDiscountValue" runat="server" Enabled="false" Text='<%# GetAmountDecimal(Eval("DiscountValue").ToString()) %>'
                                                                                CssClass="form-control"></asp:TextBox>

                                                                        </ItemTemplate>
                                                                        <FooterStyle BorderStyle="None" />
                                                                        <ItemStyle />
                                                                    </asp:TemplateField>

                                                                    <asp:TemplateField HeaderStyle-HorizontalAlign="Center" Visible="false" HeaderText="">
                                                                        <ItemTemplate>
                                                                            <asp:TextBox ID="lblTax" runat="server" Text='<%# GetAmountDecimal(Eval("TaxPercentage").ToString()) %>'
                                                                                Enabled="false" CssClass="form-control" Width="50px" Height="10px"></asp:TextBox>
                                                                        </ItemTemplate>
                                                                        <ItemStyle />
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderStyle-HorizontalAlign="Center" Visible="false" HeaderText="<%$ Resources:Attendance,Value %>">
                                                                        <ItemTemplate>
                                                                            <asp:TextBox ID="lblTaxValue" runat="server" Text='<%# GetAmountDecimal(Eval("TaxValue").ToString()) %>'
                                                                                Enabled="false" CssClass="form-control" Width="70px" Height="10px"></asp:TextBox>
                                                                        </ItemTemplate>
                                                                        <ItemStyle />
                                                                    </asp:TemplateField>


                                                                    <asp:TemplateField HeaderStyle-HorizontalAlign="Center" HeaderText="Batch No.">
                                                                        <ItemTemplate>
                                                                            <asp:TextBox ID="txtgvBatchNo" runat="server" Text='<%# Eval("Field2") %>' Width="100px" CssClass="form-control" />
                                                                        </ItemTemplate>
                                                                        <ItemStyle />
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderStyle-HorizontalAlign="Center" HeaderText="Expiry Date">
                                                                        <ItemTemplate>
                                                                            <asp:TextBox ID="txtgvExpiryDate" Width="100px" runat="server" Text='<%# Eval("Field1") %>' CssClass="form-control" />
                                                                            <cc1:CalendarExtender OnClientShown="showCalendar" ID="txtCalenderExtender" runat="server"
                                                                                Format="dd-MMM-yyyy" TargetControlID="txtgvExpiryDate">
                                                                            </cc1:CalendarExtender>
                                                                        </ItemTemplate>
                                                                        <ItemStyle />
                                                                    </asp:TemplateField>

                                                                </Columns>

                                                                <PagerStyle CssClass="pagination-ys" />

                                                            </asp:GridView>
                                                            <asp:HiddenField ID="HDFSortbin" runat="server" />
                                                        </div>
                                                        <br />
                                                    </div>
                                                    <div class="col-md-6">
                                                        <asp:Label ID="Label16" runat="server" Text="Receive Location"></asp:Label>
                                                        <asp:DropDownList ID="ddlLocation" runat="server" CssClass="form-control">
                                                        </asp:DropDownList>
                                                        <br />
                                                    </div>
                                                    <asp:Panel ID="PanelTab" runat="server" Enabled="false">
                                                        <div class="col-md-6">
                                                            <asp:Label ID="Label8" runat="server" Text="<%$ Resources:Attendance,Shipping Line %>"></asp:Label>
                                                            <asp:TextBox ID="txtShippingLine" runat="server" BackColor="#eeeeee" CssClass="form-control"
                                                                AutoPostBack="True" OnTextChanged="txtShippingLine_TextChanged" />
                                                            <cc1:AutoCompleteExtender ID="AutoCompleteExtender1" runat="server" CompletionInterval="100"
                                                                DelimiterCharacters="" Enabled="True" MinimumPrefixLength="1" ServiceMethod="GetCompletionListShippingLine"
                                                                ServicePath="" TargetControlID="txtShippingLine" UseContextKey="True" CompletionListCssClass="autoCompleteList" CompletionListItemCssClass="autoCompleteListItem" CompletionListHighlightedItemCssClass="autoCompleteSelectedListItem" OnClientShown="resetPosition">
                                                            </cc1:AutoCompleteExtender>
                                                            <br />
                                                        </div>
                                                        <div class="col-md-6">
                                                            <asp:Label ID="Label9" runat="server" Text="<%$ Resources:Attendance,Ship By %>"></asp:Label>
                                                            <asp:DropDownList ID="ddlShipBy" runat="server" CssClass="form-control">
                                                                <%--  <asp:ListItem Text="<%$ Resources:Attendance, By Track %>" Value="By Track"></asp:ListItem>--%>
                                                                <asp:ListItem Text="Truck" Value="By Track"></asp:ListItem>
                                                                <asp:ListItem Text="<%$ Resources:Attendance, By Ship %>" Value="By Ship"></asp:ListItem>
                                                                <asp:ListItem Text="<%$ Resources:Attendance, By Train %>" Value="By Train"></asp:ListItem>
                                                                <asp:ListItem Text="<%$ Resources:Attendance, By Air Freight %>" Value="By Air Freight"></asp:ListItem>
                                                                <asp:ListItem Text="<%$ Resources:Attendance, By Courier %>" Value="By Courier"></asp:ListItem>
                                                            </asp:DropDownList>
                                                            <br />
                                                        </div>
                                                        <div class="col-md-6">
                                                            <asp:Label ID="Label18" runat="server" Text="<%$ Resources:Attendance,Airway Bill No. %>"></asp:Label>
                                                            <asp:TextBox ID="txtAirwaybillno" runat="server" CssClass="form-control"></asp:TextBox>
                                                            <br />
                                                        </div>
                                                        <div class="col-md-6">
                                                            <asp:Label ID="Label23" runat="server" Text="<%$ Resources:Attendance,Actual Weight %>"></asp:Label>
                                                            <asp:TextBox ID="txtTotalWeight" runat="server" CssClass="form-control" />
                                                            <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender4" runat="server" Enabled="True"
                                                                TargetControlID="txtTotalWeight" ValidChars="1,2,3,4,5,6,7,8,9,0,.">
                                                            </cc1:FilteredTextBoxExtender>
                                                            <br />
                                                        </div>
                                                        <div class="col-md-6">
                                                            <asp:Label ID="Label12" runat="server" Text="<%$ Resources:Attendance,Ship Unit %>"></asp:Label>
                                                            <asp:DropDownList ID="ddlShipUnit" runat="server" CssClass="form-control">
                                                            </asp:DropDownList>
                                                            <br />
                                                        </div>
                                                        <div class="col-md-6">
                                                            <asp:Label ID="Label19" runat="server" Text="<%$ Resources:Attendance,Volumetric weight %>"></asp:Label>
                                                            <asp:TextBox ID="txtvolumetricweight" runat="server" CssClass="form-control" />
                                                            <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender8" runat="server" Enabled="True"
                                                                TargetControlID="txtvolumetricweight" ValidChars="1,2,3,4,5,6,7,8,9,0,.">
                                                            </cc1:FilteredTextBoxExtender>
                                                            <br />
                                                        </div>
                                                        <div class="col-md-6">
                                                            <asp:Label ID="Label2" runat="server" Text="<%$ Resources:Attendance,Unit Rate %>"></asp:Label>
                                                            <asp:TextBox ID="txtUnitRate" runat="server" CssClass="form-control" />
                                                            <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender5" runat="server" Enabled="True"
                                                                TargetControlID="txtUnitRate" ValidChars="1,2,3,4,5,6,7,8,9,0,.">
                                                            </cc1:FilteredTextBoxExtender>
                                                            <br />
                                                        </div>
                                                        <div class="col-md-6">
                                                            <asp:Label ID="Label14" runat="server" Text="<%$ Resources:Attendance,Shipping Date %>"></asp:Label>
                                                            <asp:TextBox ID="txtShippingDate" runat="server" CssClass="form-control" />
                                                            <cc1:CalendarExtender OnClientShown="showCalendar" ID="CalendarExtender2" runat="server" TargetControlID="txtShippingDate">
                                                            </cc1:CalendarExtender>
                                                            <br />
                                                        </div>
                                                        <div class="col-md-6">
                                                            <asp:Label ID="Label3" runat="server" Text="<%$ Resources:Attendance,Receiving Date %>"></asp:Label>
                                                            <asp:TextBox ID="txtReceivingDate" runat="server" CssClass="form-control" />
                                                            <cc1:CalendarExtender OnClientShown="showCalendar" ID="CalendarExtender3" runat="server" TargetControlID="txtReceivingDate">
                                                            </cc1:CalendarExtender>
                                                            <br />
                                                        </div>
                                                        <div class="col-md-6">
                                                            <asp:Label ID="Label4" runat="server" Text="<%$ Resources:Attendance,Shipment Type %>"></asp:Label>
                                                            <asp:DropDownList ID="ddlShipmentType" runat="server" CssClass="form-control">
                                                                <asp:ListItem Text="<%$ Resources:Attendance, FOB (Freight On Board) %>" Value="FOB(Frieght On Board)"></asp:ListItem>
                                                                <asp:ListItem Text="<%$ Resources:Attendance, EX-Work %>" Value="EX-Work"></asp:ListItem>
                                                                <asp:ListItem Text="<%$ Resources:Attendance, C&F%>" Value="C&F"></asp:ListItem>
                                                            </asp:DropDownList>
                                                            <br />
                                                        </div>
                                                        <div class="col-md-6">
                                                            <asp:Label ID="Label11" runat="server" Text="<%$ Resources:Attendance,Freight Status %>"></asp:Label>
                                                            <asp:DropDownList ID="ddlFreightStatus" runat="server" CssClass="form-control">
                                                                <asp:ListItem Text="<%$ Resources:Attendance, Paid %>" Value="Y"></asp:ListItem>
                                                                <asp:ListItem Text="<%$ Resources:Attendance, UnPaid %>" Value="N"></asp:ListItem>
                                                            </asp:DropDownList>
                                                            <br />
                                                        </div>
                                                    </asp:Panel>
                                                    <div class="col-md-12">
                                                        <hr />
                                                        <h5 style="color: gray"><b>Shipment Size Details</b></h5>
                                                    </div>
                                                    <div class="col-md-6">
                                                        <asp:Label ID="Label7" runat="server" Text="Length"></asp:Label>
                                                        <asp:TextBox ID="txtLength" runat="server" CssClass="form-control"></asp:TextBox>
                                                        <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtendertxtLength" runat="server"
                                                            Enabled="True" TargetControlID="txtLength" ValidChars="1,2,3,4,5,6,7,8,9,0,.">
                                                        </cc1:FilteredTextBoxExtender>
                                                        <br />
                                                    </div>
                                                    <div class="col-md-6">
                                                        <asp:Label ID="Label15" runat="server" Text="Height"></asp:Label>
                                                        <asp:TextBox ID="txtheight" runat="server" CssClass="form-control"></asp:TextBox>
                                                        <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtendertxtheight" runat="server"
                                                            Enabled="True" TargetControlID="txtheight" ValidChars="1,2,3,4,5,6,7,8,9,0,.">
                                                        </cc1:FilteredTextBoxExtender>
                                                        <br />
                                                    </div>
                                                    <div class="col-md-6">
                                                        <asp:Label ID="Label20" runat="server" Text="Width"></asp:Label>
                                                        <asp:TextBox ID="txtwidth" runat="server" CssClass="form-control"></asp:TextBox>
                                                        <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtendertxtwidth" runat="server"
                                                            Enabled="True" TargetControlID="txtwidth" ValidChars="1,2,3,4,5,6,7,8,9,0,.">
                                                        </cc1:FilteredTextBoxExtender>
                                                        <br />
                                                    </div>
                                                    <div class="col-md-6">
                                                        <asp:Label ID="Label21" runat="server" Text="Cartons"></asp:Label>
                                                        <asp:TextBox ID="txtcartons" runat="server" CssClass="form-control" OnTextChanged="getTotalVolume"
                                                            AutoPostBack="true"></asp:TextBox>
                                                        <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtendertxtcartons" runat="server"
                                                            Enabled="True" TargetControlID="txtcartons" ValidChars="1,2,3,4,5,6,7,8,9,0,.">
                                                        </cc1:FilteredTextBoxExtender>
                                                        <br />
                                                    </div>
                                                    <div class="col-md-6">
                                                        <asp:Label ID="Label24" runat="server" Text="Total"></asp:Label>
                                                        <asp:TextBox ID="txttotal" runat="server" CssClass="form-control" ReadOnly="true"></asp:TextBox>
                                                        <br />
                                                    </div>
                                                    <div class="col-md-6">
                                                        <br />
                                                        <asp:Button ID="Btn_Add_Shipping_Info" CssClass="btn btn-primary" runat="server" Text="<%$ Resources:Attendance,Add %>" OnClick="IbtnAddShippingInfo_Click" />
                                                        <br />
                                                    </div>
                                                    <div class="col-md-12">
                                                        <div style="overflow: auto">
                                                            <asp:GridView CssClass="table-striped table-bordered table table-hover" ID="gvShippingInformation" ShowHeader="true" runat="server" AutoGenerateColumns="false"
                                                                Width="100%">
                                                                <Columns>
                                                                    <asp:TemplateField HeaderText="<%$ Resources:Attendance,Delete%>">
                                                                        <ItemTemplate>
                                                                            <asp:ImageButton ID="IbtnDeleteSupplier" runat="server" CausesValidation="False"
                                                                                CommandArgument='<%# Eval("Trans_Id") %>' ImageUrl="~/Images/Erase.png" Width="16px"
                                                                                ToolTip="<%$ Resources:Attendance,Delete %>" OnCommand="IbtnDeleteShipping_Command" />
                                                                        </ItemTemplate>
                                                                        <ItemStyle HorizontalAlign="Center" />

                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="<%$ Resources:Attendance,Serial No%>">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblproductCode" runat="server"
                                                                                Text='<%#Container.DataItemIndex+1%>'></asp:Label>
                                                                        </ItemTemplate>
                                                                        <ItemStyle HorizontalAlign="Center" />
                                                                        <FooterStyle HorizontalAlign="Center" />
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Length">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lbllength" runat="server" Text='<%#Eval("Length") %>'></asp:Label>
                                                                        </ItemTemplate>
                                                                        <ItemStyle HorizontalAlign="Center" />
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Height">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblheight" runat="server" Text='<%#Eval("Height") %>'></asp:Label>
                                                                        </ItemTemplate>
                                                                        <ItemStyle HorizontalAlign="Center" />
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Width">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblwidth" runat="server" Text='<%#Eval("Width") %>'></asp:Label>
                                                                        </ItemTemplate>
                                                                        <ItemStyle HorizontalAlign="Center" />
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Cartons">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblcartons" runat="server" Text='<%#Eval("Cartons") %>'></asp:Label>
                                                                        </ItemTemplate>
                                                                        <ItemStyle HorizontalAlign="Center" />
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Total">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblTotal" runat="server" Text='<%#Eval("Total") %>'></asp:Label>
                                                                        </ItemTemplate>
                                                                        <ItemStyle HorizontalAlign="Right" />
                                                                    </asp:TemplateField>

                                                                </Columns>

                                                                <PagerStyle CssClass="pagination-ys" />

                                                            </asp:GridView>
                                                        </div>
                                                        <br />
                                                    </div>
                                                    <div class="col-md-6" runat="server" id="Footer1" visible="false">
                                                        <asp:Label ID="lblTotalVolume" runat="server" Text="Total Volume"></asp:Label>
                                                        <asp:TextBox ID="txttotalVolume" runat="server" CssClass="form-control" Enabled="false"></asp:TextBox>
                                                        <br />
                                                    </div>
                                                    <div class="col-md-6" runat="server" id="Footer2" visible="false">
                                                        <asp:Label ID="Label17" runat="server" Text="Divide by"></asp:Label>
                                                        <asp:TextBox ID="txtdivideby" runat="server" CssClass="form-control"
                                                            AutoPostBack="true" OnTextChanged="txtdivideby_OnTextChanged"></asp:TextBox>
                                                        <br />
                                                    </div>
                                                    <div class="col-md-6" runat="server" id="Footer3" visible="false">
                                                        <asp:Label ID="Label25" runat="server" Text="Volumetric weight"></asp:Label>
                                                        <asp:TextBox ID="txttotalvolumetricweight" runat="server" CssClass="form-control" Enabled="false"></asp:TextBox>
                                                        <br />
                                                    </div>
                                                    <div class="col-md-12" style="text-align: center">
                                                        <asp:HiddenField ID="Hdn_Edit" runat="server" />
                                                        <asp:Button ID="btnSave" ValidationGroup="Save" runat="server" Text="<%$ Resources:Attendance,Save %>" CssClass="btn btn-success"
                                                            OnClick="btnSave_Click" Visible="false" />

                                                        <asp:Button ID="btnReset" runat="server" Text="<%$ Resources:Attendance,Reset %>"
                                                            CssClass="btn btn-primary" OnClick="btnReset_Click" />

                                                        <asp:Button ID="btnCancel" runat="server" Text="<%$ Resources:Attendance,Cancel %>"
                                                            CssClass="btn btn-danger" OnClick="btnCancel_Click" />
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
                    <div class="tab-pane" id="Pending_Order">
                        <asp:UpdatePanel ID="Update_Pending_Order" runat="server" UpdateMode="Conditional">
                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="Btn_Pending_Order" EventName="Click" />
                            </Triggers>
                            <ContentTemplate>

                                <div class="row">
                                    <div class="col-md-12">
                                        <div id="Div3" runat="server" class="box box-info collapsed-box">
                                            <div class="box-header with-border">
                                                <h3 class="box-title">
                                                    <asp:Label ID="Label29" runat="server" Text="Advance Search"></asp:Label></h3>
                                                &nbsp;&nbsp;|&nbsp;&nbsp;
				<asp:Label ID="lblQTotalRecords" Font-Bold="true" runat="server" Text="<%$ Resources:Attendance,Total Records: 0 %>"></asp:Label>

                                                <div class="box-tools pull-right">
                                                    <button type="button" class="btn btn-box-tool" data-widget="collapse">
                                                        <i id="I3" runat="server" class="fa fa-plus"></i>
                                                    </button>
                                                </div>
                                            </div>
                                            <div class="box-body">
                                                <div class="col-lg-3">
                                                    <asp:DropDownList ID="ddlQSeleclField" runat="server" CssClass="form-control" AutoPostBack="true" OnSelectedIndexChanged="ddlQSeleclField_SelectedIndexChanged">
                                                        <asp:ListItem Selected="True" Text="<%$ Resources:Attendance,Purchase Order No %>" Value="PONo"></asp:ListItem>
                                                        <asp:ListItem Text="<%$ Resources:Attendance,Purchase Order Date %>" Value="PODate"></asp:ListItem>
                                                        <asp:ListItem Text="<%$ Resources:Attendance,Delivery Date %>" Value="DeliveryDate"></asp:ListItem>
                                                        <asp:ListItem Text="<%$ Resources:Attendance,Order Type %>" Value="Order_Type"></asp:ListItem>
                                                        <asp:ListItem Text="<%$ Resources:Attendance,Supplier %>" Value="Supplier_Name"></asp:ListItem>
                                                        <asp:ListItem Text="<%$ Resources:Attendance,Status %>" Value="Field4"></asp:ListItem>
                                                        <asp:ListItem Text="<%$ Resources:Attendance,Created By %>" Value="CreatedUser"></asp:ListItem>
                                                        <asp:ListItem Text="<%$ Resources:Attendance,Modified By %>" Value="ModifiedUser"></asp:ListItem>
                                                    </asp:DropDownList>
                                                </div>
                                                <div class="col-lg-2">
                                                    <asp:DropDownList ID="ddlQOption" runat="server" CssClass="form-control">
                                                        <asp:ListItem Text="<%$ Resources:Attendance,--Select-- %>"></asp:ListItem>
                                                        <asp:ListItem Text="<%$ Resources:Attendance,Equal %>"></asp:ListItem>
                                                        <asp:ListItem Selected="True" Text="<%$ Resources:Attendance,Contains %>"></asp:ListItem>
                                                        <asp:ListItem Text="<%$ Resources:Attendance,Like %>"></asp:ListItem>
                                                    </asp:DropDownList>
                                                </div>
                                                <div class="col-lg-5">
                                                    <asp:Panel ID="Panel" runat="server" DefaultButton="ImgBtnQBind">
                                                        <asp:TextBox ID="txtQValue" runat="server" CssClass="form-control" placeholder="Search From Content"></asp:TextBox>
                                                        <asp:TextBox ID="txtQValueDate" runat="server" CssClass="form-control" placeholder="Search From Date" Visible="false"></asp:TextBox>
                                                        <cc1:CalendarExtender OnClientShown="showCalendar" Format="dd-MMM-yyyy" ID="CalendarExtendertxtQValue" runat="server" TargetControlID="txtQValueDate" />
                                                    </asp:Panel>
                                                </div>
                                                <div class="col-lg-2">
                                                    <asp:LinkButton ID="ImgBtnQBind" runat="server" CausesValidation="False" OnClick="ImgBtnQBind_Click" ToolTip="<%$ Resources:Attendance,Search %>"><span class="fa fa-search"  style="font-size:25px;"></span></asp:LinkButton>&nbsp;&nbsp;&nbsp;
                                                    <asp:LinkButton ID="ImgBtnQRefresh" runat="server" CausesValidation="False" OnClick="ImgBtnQRefresh_Click" ToolTip="<%$ Resources:Attendance,Refresh %>"><span class="fa fa-repeat"  style="font-size:25px;"></span></asp:LinkButton>
                                                </div>

                                            </div>
                                        </div>
                                    </div>
                                </div>
                                <div class="box box-warning box-solid" <%= gvPurchaseOrder.Rows.Count>0?"style='display:block'":"style='display:none'"%>>
                                    <div class="box-body">
                                        <div class="row">
                                            <div class="col-md-12">
                                                <div class="flow">
                                                    <asp:GridView CssClass="table-striped table-bordered table table-hover" ID="gvPurchaseOrder" PageSize="<%# PageControlCommon.GetPageSize() %>" CurrentSortField="PODate" CurrentSortDirection="DESC"
                                                        DataKeyNames="TransID" runat="server" AutoGenerateColumns="False" Width="100%"
                                                        AllowPaging="false" AllowSorting="True"
                                                        OnSorting="gvPurchaseOrder_OnSorting">
                                                        <Columns>
                                                            <asp:TemplateField HeaderText="Order No" SortExpression="PONo">
                                                                <ItemTemplate>
                                                                    <%# Eval("PONo") %>
                                                                </ItemTemplate>
                                                                <ItemStyle />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Order Date" SortExpression="PODate">
                                                                <ItemTemplate>
                                                                    <%# GetDateFromat(Eval("PODate").ToString()) %>
                                                                </ItemTemplate>
                                                                <ItemStyle />
                                                            </asp:TemplateField>

                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,Supplier %>" SortExpression="Supplier_Name">
                                                                <ItemTemplate>
                                                                    <%# Eval("Supplier_Name").ToString() %>
                                                                </ItemTemplate>
                                                                <ItemStyle HorizontalAlign="Left" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Product Code" SortExpression="ProductCode">
                                                                <ItemTemplate>
                                                                    <%#Eval("ProductCode") %>
                                                                </ItemTemplate>
                                                                <ItemStyle HorizontalAlign="Left" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Unit Name" SortExpression="Unit_Name">
                                                                <ItemTemplate>
                                                                    <%#Eval("Unit_Name").ToString() %>
                                                                </ItemTemplate>
                                                                <ItemStyle HorizontalAlign="Left" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Order Qty" SortExpression="OrderQty">
                                                                <ItemTemplate>
                                                                    <%# Convert.ToDouble(Eval("OrderQty").ToString()).ToString("0.00") %>
                                                                </ItemTemplate>
                                                                <ItemStyle HorizontalAlign="Right" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Remain Qty" SortExpression="RemainQty">
                                                                <ItemTemplate>
                                                                    <%#Convert.ToDouble(Eval("RemainQty").ToString()).ToString("0.00") %>
                                                                </ItemTemplate>
                                                                <ItemStyle HorizontalAlign="Right" />
                                                            </asp:TemplateField>
                                                        </Columns>


                                                        <PagerStyle CssClass="pagination-ys" />

                                                    </asp:GridView>
                                                </div>
                                                <asp:HiddenField ID="hdnGvPurchaseOrderCurrentPageIndex" runat="server" Value="1" />
                                                <div class="col-md-12">
                                                    <asp:Repeater ID="rptPagerPO" runat="server">
                                                        <ItemTemplate>
                                                            <ul class="pagination">
                                                                <li class='<%# Convert.ToBoolean(Eval("Enabled")) ? "page-item" : "page-item active" %>'>
                                                                    <asp:LinkButton ID="lnkPage" runat="server" Text='<%#Eval("Text") %>' CommandArgument='<%# Eval("Value") %>'
                                                                        CssClass="page-link"
                                                                        OnClick="PagePO_Changed" OnClientClick='<%# !Convert.ToBoolean(Eval("Enabled")) ? "return false;" : "" %>'></asp:LinkButton>
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
                </div>
            </div>
        </div>
    </div>

    <div class="modal fade" id="Fileupload123" tabindex="-1" role="dialog" aria-labelledby="myModalLabel"
        aria-hidden="true">
        <div class="modal-dialog modal-lg">
            <div class="modal-content">
                <div class="modal-body">
                    <AT1:FileUpload1 runat="server" ID="FUpload1" />
                </div>
                <div class="modal-footer">
                    <button type="button" id="" class="btn btn-danger" data-dismiss="modal">
                        Close</button>
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
                            <asp:HiddenField ID="hdnSalesOrderId" runat="server" Value="0" />
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
                                                <div class="col-md-12">
                                                    <asp:Label ID="Label26" runat="server" Text="<%$ Resources:Attendance,Invoice Qty %>"></asp:Label>
                                                    <asp:Label ID="lblpnlInvoiceQty" runat="server" Text="0"></asp:Label>
                                                    <br />
                                                </div>
                                                <div id="pnlSerialNumber" runat="server" class="col-md-12">
                                                    <div class="col-md-6">
                                                        <asp:Label ID="Label30" runat="server" Text="<%$ Resources:Attendance, File Upload%>"></asp:Label>
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
                                                        <asp:Button ID="btnGenerteSerial" runat="server" Text="Generate Serial" CssClass="btn btn-primary" OnClick="btnGenerteSerial_Click" />
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
                                                                            <asp:Label ID="Label31" runat="server" Text="Total :"></asp:Label>
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
                                                                            <asp:Panel ID="pnlGvserial" runat="server" ScrollBars="Auto" Height="320px">
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
                                                                                                <asp:Label ID="lblsrno" runat="server" CssClass="labelComman" Text='<%#Eval("SerialNo") %>'></asp:Label>
                                                                                            </ItemTemplate>
                                                                                            <ItemStyle HorizontalAlign="Center" />
                                                                                        </asp:TemplateField>
                                                                                        <asp:TemplateField HeaderText="Width">
                                                                                            <ItemTemplate>
                                                                                                <asp:Label ID="lblmfg" runat="server" CssClass="labelComman" Text='<%#Eval("Width") %>'></asp:Label>
                                                                                            </ItemTemplate>
                                                                                            <ItemStyle HorizontalAlign="Center" />
                                                                                        </asp:TemplateField>
                                                                                        <asp:TemplateField HeaderText="Length">
                                                                                            <ItemTemplate>
                                                                                                <asp:Label ID="lblBatchNo" runat="server" CssClass="labelComman" Text='<%#Eval("Length") %>'></asp:Label>
                                                                                            </ItemTemplate>
                                                                                            <ItemStyle HorizontalAlign="Center" />
                                                                                        </asp:TemplateField>
                                                                                        <asp:TemplateField HeaderText="Quantity">
                                                                                            <ItemTemplate>
                                                                                                <asp:Label ID="lblqty" runat="server" CssClass="labelComman" Text='<%#Eval("Quantity") %>'></asp:Label>
                                                                                            </ItemTemplate>
                                                                                            <ItemStyle HorizontalAlign="Center" />
                                                                                        </asp:TemplateField>
                                                                                        <asp:TemplateField HeaderText="Pallet Id">
                                                                                            <ItemTemplate>
                                                                                                <asp:Label ID="lblpalletId" runat="server" CssClass="labelComman" Text='<%#Eval("Pallet_ID") %>'></asp:Label>
                                                                                            </ItemTemplate>
                                                                                            <ItemStyle HorizontalAlign="Center" />
                                                                                        </asp:TemplateField>
                                                                                    </Columns>




                                                                                </asp:GridView>
                                                                            </asp:Panel>
                                                                        </div>
                                                                    </div>
                                                                    <div class="col-md-4">
                                                                        <asp:TextBox ID="txtSerialNo" Height="350px" runat="server" CssClass="form-control"
                                                                            TextMode="MultiLine"></asp:TextBox>
                                                                    </div>
                                                                </div>
                                                            </div>
                                                        </div>

                                                    </div>
                                                </div>
                                                <div id="pnlexp_and_Manf" runat="server" visible="false" class="col-md-12">
                                                    <div style="overflow: auto">
                                                        <asp:GridView CssClass="table-striped table-bordered table table-hover" ID="gvStockwithManf_and_expiry" ShowHeader="true" runat="server" AutoGenerateColumns="false"
                                                            Width="100%" DataKeyNames="ProductId" ShowFooter="true" OnRowDeleting="gridView_RowDeleting"
                                                            OnRowCommand="gridView_RowCommand">

                                                            <Columns>
                                                                <asp:TemplateField HeaderText="Quantity">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblQuantity" runat="server" CssClass="labelComman" Text='<%#setRoundValue(Eval("Quantity").ToString()) %>'></asp:Label>
                                                                        <asp:HiddenField ID="hdnProductId" runat="server" Value='<%#Eval("ProductId") %>' />
                                                                        <asp:HiddenField ID="hdnOrderId" runat="server" Value='<%#Eval("POID") %>' />
                                                                    </ItemTemplate>
                                                                    <EditItemTemplate>
                                                                    </EditItemTemplate>
                                                                    <FooterTemplate>
                                                                        <asp:TextBox ID="txtQuantity" runat="server" Font-Names="Verdana" CssClass="textComman"
                                                                            CausesValidation="true" Width="250px"></asp:TextBox>
                                                                        <cc1:FilteredTextBoxExtender ID="filtertextbox" runat="server" TargetControlID="txtQuantity"
                                                                            FilterType="Numbers">
                                                                        </cc1:FilteredTextBoxExtender>
                                                                    </FooterTemplate>
                                                                    <ItemStyle Width="10%" />
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="Expiry Date">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblExpiryDate" runat="server" CssClass="labelComman" Text='<%#Eval("ExpiryDate")%>'></asp:Label>
                                                                    </ItemTemplate>
                                                                    <EditItemTemplate>
                                                                    </EditItemTemplate>
                                                                    <FooterTemplate>
                                                                        <asp:TextBox ID="txtExpiryFooter" runat="server" Font-Names="Verdana" CssClass="textComman"
                                                                            Text='<%#Eval("ExpiryDate") %>' CausesValidation="true" Width="250px"></asp:TextBox>
                                                                        <cc1:CalendarExtender ID="txtExpiryFooter_CalenderExtender" runat="server" Enabled="True"
                                                                            TargetControlID="txtExpiryFooter" Format="dd-MMM-yyyy">
                                                                        </cc1:CalendarExtender>
                                                                    </FooterTemplate>
                                                                    <ItemStyle Width="8%" />
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="Manufacturing Date">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblTaxValue" runat="server" CssClass="labelComman" Text='<%#Eval("ManufacturerDate") %>'></asp:Label>
                                                                    </ItemTemplate>
                                                                    <EditItemTemplate>
                                                                    </EditItemTemplate>
                                                                    <FooterTemplate>
                                                                        <asp:TextBox ID="txtManfacturerFooter" runat="server" Font-Names="Verdana" CssClass="textComman"
                                                                            Text='<%#Eval("ManufacturerDate") %>' CausesValidation="true" Width="250px"></asp:TextBox>
                                                                        <cc1:CalendarExtender ID="txtManfacturerFooter_CalenderExtender" runat="server" Enabled="True"
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
                                                                        <asp:Panel ID="pnlIbtnAddProductSupplierCode" runat="server" DefaultButton="IbtnAddProductSupplierCode">
                                                                            <asp:ImageButton ID="IbtnAddProductSupplierCode" runat="server" CausesValidation="False"
                                                                                CommandName="AddNew" Height="29px" ImageUrl="~/Images/add.png" Width="35px" ToolTip="<%$ Resources:Attendance,Add %>" />
                                                                        </asp:Panel>
                                                                        <%--<asp:Button ID="ButtonAdd" runat="server" CommandName="AddNew"  Text="Add New Row"  />--%>
                                                                    </FooterTemplate>
                                                                    <ItemStyle HorizontalAlign="Center" />
                                                                </asp:TemplateField>
                                                            </Columns>



                                                        </asp:GridView>
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
                            <asp:HiddenField ID="hdnusercontrolProductid" runat="server" />
                            <asp:HiddenField ID="hdnusercontrolrowindex" runat="server" />
                            <asp:HiddenField ID="hdnusercontrolsalesorderid" runat="server" />

                            <asp:Button ID="BtnSerialSave" runat="server" Text="<%$ Resources:Attendance,Save %>"
                                CssClass="btn btn-success" OnClick="BtnSerialSave_Click" />
                            <asp:Button ID="btnResetSerial" runat="server" Text="<%$ Resources:Attendance,Reset %>"
                                CssClass="btn btn-primary" OnClick="btnResetSerial_Click" />

                            <button type="button" class="btn btn-danger" data-dismiss="modal">
                                Close</button>
                            <asp:Button ID="btnDefault" runat="server" Style="visibility: hidden" />
                        </ContentTemplate>
                    </asp:UpdatePanel>
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

    <asp:UpdateProgress ID="UpdateProgress3" runat="server" AssociatedUpdatePanelID="Update_Pending_Order">
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

        function LI_List_Active() {
            $("#Li_List").addClass("active");
            $("#List").addClass("active");

            $("#Li_New").removeClass("active");
            $("#New").removeClass("active");
        }
        function Li_Tab_New() {
            document.getElementById('<%= Btn_New.ClientID %>').click();
        }
        function Li_Tab_Pending_Order() {
            document.getElementById('<%= Btn_Pending_Order.ClientID %>').click();
        }

        function View_Modal_Popup() {
            document.getElementById('<%= Btn_View_Modal.ClientID %>').click();
        }
        function Close_Modal_Popup() {
            document.getElementById('<%= Btn_View_Modal.ClientID %>').click();
        }
    </script>
    <script type="text/javascript">
        function count(clientId) {
            var txtInput = document.getElementById(clientId);
            if (event.keyCode == 13) {
                document.getElementById('<%= txtCount.ClientID %>').innerHTML = lineBreakCount(txtInput.value);
            }
            if (event.keyCode == 8 || event.keyCode == 46) {
                document.getElementById('<%= txtCount.ClientID %>').innerHTML = lineDelBreakCount(txtInput.value); // The button id over here
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
                return ((str.match(/[^\n]*\n[^\n]*/gi).length - 1));
            } catch (e) {
                return 0;
            }
            function GetTotalCartoonSum() {
                var getLength = $("input[id*=txtLength]")
                var getHeight = $("input[id*=txtheight]")
                var getWidth = $("input[id*=txtwidth]")
                var getCartoon = $("input[id*=txtcartons]")
                document.getElementById('<%= txttotal.ClientID %>').Value = (parseFloat(getLength.value) * parseFloat(getHeight.value) * parseFloat(getWidth.value)) * parseFloat(getCartoon.value);
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
        function redirectToHome(msg) {
            if (confirm(msg)) {
                window.open('../MasterSetup/Home.aspx', 'window', 'width=1024,');
                return true;
            }
            else {
                return false;
            }
        }
        function Modal_Open_FileUpload() {
            document.getElementById('<%= Btn_Modal_FileUpload.ClientID %>').click();
        }

    </script>
</asp:Content>

