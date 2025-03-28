<%@ Page Title="" Language="C#" MasterPageFile="~/ERPMaster.master" EnableEventValidation="false" AutoEventWireup="true" CodeFile="PurchaseQuatation.aspx.cs" Inherits="Purchase_PurchaseQuatation" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Src="~/WebUserControl/FileUpload.ascx" TagPrefix="AT1" TagName="FileUpload1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="Head" runat="Server">
    <script type="text/javascript"></script>
       function DL_Tax_Hide() {
            $('.Div_Tax_P').toggleClass('hidden');
            $('.Div_Tax_V').toggleClass('hidden');
        }
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="HeaderContent" runat="Server">
    <h1>
        <i class="fas fa-money-check-alt"></i>&nbsp;&nbsp;
        <asp:Label ID="lblHeader" runat="server" Text="<%$ Resources:Attendance,Purchase Quotation%>"></asp:Label>
    </h1>
    <ol class="breadcrumb">
        <li><a href="#"><i class="fa fa-dashboard"></i>
            <asp:Label ID="Label6" runat="server" Text="<%$ Resources:Attendance,Inventory%>"></asp:Label></a></li>
        <li><a href="#"><i class="fa fa-dashboard"></i>
            <asp:Label ID="Label10" runat="server" Text="<%$ Resources:Attendance,Purchase%>"></asp:Label></a></li>
        <li class="active">
            <asp:Label ID="Label5" runat="server" Text="<%$ Resources:Attendance,Purchase Quotation%>"></asp:Label></li>
    </ol>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="Server">
    <asp:UpdatePanel ID="Update_Button" runat="server">
        <ContentTemplate>
            <asp:Panel ID="Pnl_Tab_Button" runat="server">
                <asp:Button ID="Btn_List" Style="display: none;" UseSubmitBehavior="false" runat="server" OnClick="btnList_Click" Text="List" />
                <asp:Button ID="Btn_New" Style="display: none;" UseSubmitBehavior="false" runat="server" OnClick="btnNew_Click" Text="New" />
                <asp:Button ID="Btn_Bin" Style="display: none;" UseSubmitBehavior="false" runat="server" OnClick="btnBin_Click" Text="Bin" />
                <asp:Button ID="Btn_Inquiry" Style="display: none;" UseSubmitBehavior="false" runat="server" OnClick="btnPRequest_Click" Text="Inquiry" />
                <asp:Button ID="Btn_Purchase_Quotation" Style="display: none;" runat="server" UseSubmitBehavior="false" data-toggle="modal" data-target="#Purchase_Quotation" Text=" Purchase Quotation Moddal" />
                <asp:Button ID="Btn_GST_Modal" Style="display: none;" runat="server" data-toggle="modal" data-target="#Modal_GST" Text="GST" />
                <asp:Button ID="Btn_Modal_FileUpload" Style="display: none;" runat="server" data-toggle="modal" data-target="#Fileupload123" Text="FileUpload" />
                <asp:HiddenField runat="server" ID="hdnCanView" />
                <asp:HiddenField runat="server" ID="hdnCanEdit" />
                <asp:HiddenField runat="server" ID="hdnCanDelete" />
                <asp:HiddenField runat="server" ID="hdnCanPrint" />
                <asp:HiddenField runat="server" ID="hdnCanUpload" />
                <asp:HiddenField ID="hfScrollPosition" runat="server" Value="0" />
                <asp:HiddenField ID="hdfCurrentRow" runat="server" />
            </asp:Panel>
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
                    <li id="Li_Inquiry"><a href="#Inquiry" onclick="Li_Tab_Inquiry()" data-toggle="tab">
                        <i class="fas fa-user-check"></i>&nbsp;&nbsp;<asp:Label ID="Label2" runat="server" Text="<%$ Resources:Attendance,Inquiry %>"></asp:Label></a></li>
                    <li id="Li_Bin"><a href="#Bin" onclick="Li_Tab_Bin()" data-toggle="tab">
                        <i class="fa fa-trash"></i>&nbsp;&nbsp;<asp:Label ID="Label13" runat="server" Text="<%$ Resources:Attendance,Bin %>"></asp:Label></a></li>
                    <li id="Li_New"><a onclick="Li_Tab_New()" href="#New" data-toggle="tab">
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
                        <asp:UpdatePanel ID="Update_List" runat="server" UpdateMode="Conditional">
                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="btnQuoteSave" />
                                <asp:AsyncPostBackTrigger ControlID="Btn_New" EventName="Click" />
                                <asp:AsyncPostBackTrigger ControlID="imgBtnRestore" />
                                <asp:AsyncPostBackTrigger ControlID="btnQuoteCancel" EventName="Click" />

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
                                                <div class="col-lg-12">
                                                    <asp:DropDownList ID="ddlLocation" runat="server" Class="form-control" OnSelectedIndexChanged="ddlLocation_SelectedIndexChanged"
                                                        AutoPostBack="true">
                                                    </asp:DropDownList>
                                                    <br />
                                                </div>
                                                <div class="col-lg-2">
                                                    <asp:DropDownList ID="ddlPosted" runat="server" CssClass="form-control" AutoPostBack="true" OnSelectedIndexChanged="ddlPosted_SelectedIndexChanged">
                                                        <asp:ListItem Selected="True" Text="<%$ Resources:Attendance,Open %>" Value="Pending"></asp:ListItem>
                                                        <asp:ListItem Text="<%$ Resources:Attendance,Close %>" Value="Created"></asp:ListItem>
                                                        <asp:ListItem Text="<%$ Resources:Attendance,All %>" Value="0"></asp:ListItem>
                                                    </asp:DropDownList>
                                                </div>
                                                <div class="col-lg-2">
                                                    <asp:DropDownList ID="ddlFieldName" runat="server" CssClass="form-control" AutoPostBack="true" OnSelectedIndexChanged="ddlFieldName_SelectedIndexChanged">
                                                        <asp:ListItem Text="<%$ Resources:Attendance, Quotation No. %>" Value="RPQ_No" Selected="True"></asp:ListItem>
                                                        <asp:ListItem Text="<%$ Resources:Attendance, Quotation Date %>" Value="RPQ_Date"></asp:ListItem>
                                                        <asp:ListItem Text="<%$ Resources:Attendance, Inquiry No. %>" Value="Pinq_No"></asp:ListItem>
                                                        <asp:ListItem Text="<%$ Resources:Attendance,Supplier Name %>" Value="Supplier_Name"></asp:ListItem>
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
                                                        <asp:TextBox placeholder="Search From Content" ID="txtValue" runat="server" CssClass="form-control"></asp:TextBox>
                                                        <asp:TextBox placeholder="Search From Date" ID="txtValueDate" runat="server" CssClass="form-control" Visible="false"></asp:TextBox>
                                                        <cc1:CalendarExtender OnClientShown="showCalendar" Format="dd-MMM-yyyy" ID="CalendartxtValueDate" runat="server" TargetControlID="txtValueDate" />
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
                                <div class="box box-warning box-solid" <%= GvPurchaseQuote.Rows.Count>0?"style='display:block'":"style='display:none'"%>>
                                    <div class="box-body">
                                        <div class="row">
                                            <div class="col-md-12">
                                                <div class="flow">
                                                    <asp:GridView CssClass="table-striped table-bordered table table-hover " ID="GvPurchaseQuote" PageSize="<%# PageControlCommon.GetPageSize() %>" CurrentSortField="RPQ_Date" CurrentSortDirection="DESC"
                                                        runat="server" AutoGenerateColumns="False" Width="100%" AllowPaging="false"
                                                        AllowSorting="True" OnSorting="GvPurchaseQuote_Sorting">

                                                        <Columns>

                                                            <asp:TemplateField HeaderText="Action">
                                                                <ItemTemplate>
                                                                    <div class="dropdown" style="position: absolute;">
                                                                        <button class="btn btn-default dropdown-toggle" type="button" data-toggle="dropdown">
                                                                            <i class="fa fa-ellipsis-h" aria-hidden="true"></i>
                                                                        </button>
                                                                        <ul class="dropdown-menu">
                                                                            <li <%= hdnCanPrint.Value=="true"?"style='display:block'":"style='display:none'" %>>
                                                                                <a onclick="IbtnQC_Command('<%# HttpUtility.UrlEncode(Encrypt(Eval("RPQ_No").ToString())) %>')"><i class="fa fa-print"></i>Print </a>
                                                                            </li>

                                                                            <li <%= hdnCanView.Value=="true"?"style='display:block'":"style='display:none'" %>>
                                                                                <asp:LinkButton ID="lnkViewDetail" runat="server" CommandArgument='<%# Eval("Trans_Id") %>' CommandName='<%# Eval("Location_Id") %>' OnCommand="lnkViewDetail_Command" CausesValidation="False"><i class="fa fa-eye"></i>View</asp:LinkButton>
                                                                            </li>

                                                                            <li <%= hdnCanEdit.Value=="true"?"style='display:block'":"style='display:none'" %>>
                                                                                <asp:LinkButton ID="btnEdit" runat="server" CommandArgument='<%# Eval("Trans_Id") %>' CommandName='<%# Eval("Location_Id") %>' OnCommand="btnEdit_Command" CausesValidation="False"><i class="fa fa-pencil"></i>Edit </asp:LinkButton>
                                                                            </li>
                                                                            <li <%= hdnCanDelete.Value=="true"?"style='display:block'":"style='display:none'" %>>
                                                                                <asp:LinkButton ID="IbtnDelete" runat="server" CausesValidation="False" CommandArgument='<%# Eval("Trans_Id") %>' OnCommand="IbtnDelete_Command"><i class="fa fa-trash"></i>Delete</asp:LinkButton>
                                                                                <cc1:ConfirmButtonExtender ID="confirm2" runat="server" ConfirmText="<%$ Resources:Attendance,Are you sure you want to delete the record?%>" TargetControlID="IbtnDelete"></cc1:ConfirmButtonExtender>
                                                                            </li>
                                                                            <li <%= hdnCanUpload.Value=="true"?"style='display:block'":"style='display:none'" %>>
                                                                                <asp:LinkButton ID="lbtnFileUpload" runat="server" CommandArgument='<%# Eval("Trans_Id")+"/"+Eval("Location_Id") %>' CommandName='<%# Eval("RPQ_No") %>' OnCommand="lbtnFileUpload_Command" CausesValidation="False"><i class="fa fa-upload"></i>File Upload</asp:LinkButton>
                                                                            </li>
                                                                        </ul>
                                                                    </div>

                                                                </ItemTemplate>
                                                            </asp:TemplateField>


                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,Quotation No. %>" SortExpression="RPQ_No">
                                                                <ItemTemplate>
                                                                    <%#Eval("RPQ_No") %>
                                                                </ItemTemplate>
                                                                <ItemStyle HorizontalAlign="Left" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,Quotation Date %>" SortExpression="RPQ_Date">
                                                                <ItemTemplate>
                                                                    <%#Convert.ToDateTime(Eval("RPQ_Date").ToString()).ToString("dd-MMM-yyyy") %>
                                                                </ItemTemplate>
                                                                <ItemStyle HorizontalAlign="Center" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance, Inquiry No. %>"
                                                                SortExpression="Pinq_No">
                                                                <ItemTemplate>
                                                                    <%#Eval("Pinq_No") %>
                                                                </ItemTemplate>
                                                                <ItemStyle HorizontalAlign="Left" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,Supplier %>" SortExpression="Supplier_Name">
                                                                <ItemTemplate>
                                                                    <%#Eval("Supplier_Name") %>
                                                                </ItemTemplate>
                                                                <ItemStyle HorizontalAlign="Left" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,Created By %>" SortExpression="CreatedUser">
                                                                <ItemTemplate>
                                                                    <%#Eval("CreatedUser") %>
                                                                </ItemTemplate>
                                                                <ItemStyle HorizontalAlign="Left" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,Modified By %>" SortExpression="ModifiedUser">
                                                                <ItemTemplate>
                                                                    <%# Eval("ModifiedUser") %>
                                                                </ItemTemplate>
                                                                <ItemStyle HorizontalAlign="Left" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,Total Amount %>" SortExpression="TotalAmount">
                                                                <ItemTemplate>
                                                                    <%# Eval("Currency_Code").ToString()+" "+SystemParameter.GetAmountWithDecimal(Eval("TotalAmount").ToString(),Eval("CurrencyDecimalCount").ToString()) %>
                                                                </ItemTemplate>
                                                                <ItemStyle HorizontalAlign="Right" />
                                                            </asp:TemplateField>
                                                        </Columns>



                                                    </asp:GridView>
                                                </div>
                                                <div class="col-md-12">
                                                    <asp:HiddenField ID="hdnquotationListIndex" runat="server" Value="1" />
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
                                <asp:AsyncPostBackTrigger ControlID="Btn_New" EventName="Click" />
                                <asp:AsyncPostBackTrigger ControlID="GvPurchaseQuote" />
                                <asp:AsyncPostBackTrigger ControlID="GvPurchaseInquiry" />
                            </Triggers>
                            <ContentTemplate>
                                <asp:HiddenField ID="editid" runat="server" />
                                <asp:HiddenField ID="hdnPIId" runat="server" Value="0" />
                                <div class="row">
                                    <div class="col-md-12">
                                        <div class="box box-primary">
                                            <div class="box-body">
                                                <div class="form-group">
                                                    <div id="PnlNewEdit" runat="server">
                                                        <div class="col-md-6">
                                                            <asp:Label ID="lblRPQDate" runat="server" Text="<%$ Resources:Attendance,Quotation Date %>"></asp:Label>
                                                            <a style="color: Red">*</a>
                                                            <asp:RequiredFieldValidator EnableClientScript="true" Style="float: right;" runat="server" ID="RequiredFieldValidator3" ValidationGroup="Save"
                                                                Display="Dynamic" SetFocusOnError="true" ControlToValidate="txtRPQDate" ErrorMessage="<%$ Resources:Attendance,Enter Quotation Date%>"></asp:RequiredFieldValidator>
                                                            <asp:TextBox ID="txtRPQDate" runat="server" CssClass="form-control" />
                                                            <cc1:CalendarExtender OnClientShown="showCalendar" ID="Calender" runat="server" TargetControlID="txtRPQDate" />
                                                            <br />
                                                        </div>
                                                        <div class="col-md-6">
                                                            <asp:Label ID="lblRPQNo" runat="server" Text="<%$ Resources:Attendance,Quotation No. %>"></asp:Label>
                                                            <a style="color: Red">*</a>
                                                            <asp:RequiredFieldValidator EnableClientScript="true" Style="float: right;" runat="server" ID="RequiredFieldValidator1" ValidationGroup="Save"
                                                                Display="Dynamic" SetFocusOnError="true" ControlToValidate="txtRPQNo" ErrorMessage="<%$ Resources:Attendance,Enter Quotation No%>"></asp:RequiredFieldValidator>
                                                            <asp:TextBox ID="txtRPQNo" runat="server" CssClass="form-control" />
                                                            <br />
                                                        </div>
                                                        <div class="col-md-6">
                                                            <asp:Label ID="lblInquiryNo" runat="server" Text="<%$ Resources:Attendance,Inquiry No. %>" />
                                                            <a style="color: Red">*</a>
                                                            <asp:RequiredFieldValidator EnableClientScript="true" Style="float: right;" runat="server" ID="RequiredFieldValidator2" ValidationGroup="Save"
                                                                Display="Dynamic" SetFocusOnError="true" ControlToValidate="txtInquiryNo" ErrorMessage="<%$ Resources:Attendance,Enter Inquiry No%>"></asp:RequiredFieldValidator>
                                                            <asp:TextBox ID="txtInquiryNo" runat="server" CssClass="form-control" ReadOnly="True" />
                                                            <br />
                                                        </div>
                                                        <div class="col-md-6">
                                                            <asp:Label ID="lblInquiryDate" runat="server" Text="<%$ Resources:Attendance,Inquiry Date%>" />
                                                            <a style="color: Red">*</a>
                                                            <asp:RequiredFieldValidator EnableClientScript="true" Style="float: right;" runat="server" ID="RequiredFieldValidator4" ValidationGroup="Save"
                                                                Display="Dynamic" SetFocusOnError="true" ControlToValidate="txtInquiryDate" ErrorMessage="<%$ Resources:Attendance,Enter Inquiry Date%>"></asp:RequiredFieldValidator>
                                                            <asp:TextBox ID="txtInquiryDate" runat="server" CssClass="form-control" ReadOnly="True" />
                                                            <br />
                                                        </div>
                                                        <div class="col-md-12">
                                                            <div style="overflow: auto; max-height: 500px;">
                                                                <asp:GridView CssClass="table-striped table-bordered table table-hover" ID="GvSupplier" runat="server" Width="100%" AutoGenerateColumns="False"
                                                                    DataKeyNames="Supplier_Id">

                                                                    <Columns>
                                                                        <asp:TemplateField HeaderText="<%$ Resources:Attendance,S No. %>">
                                                                            <ItemTemplate>
                                                                                <%#Container.DataItemIndex+1 %>
                                                                            </ItemTemplate>
                                                                            <ItemStyle HorizontalAlign="Center" />
                                                                        </asp:TemplateField>
                                                                        <asp:TemplateField HeaderText="<%$ Resources:Attendance, Detail %>">
                                                                            <ItemTemplate>
                                                                                <asp:LinkButton ID="ImgSupplierDetail" runat="server" CommandArgument='<%# Eval("Supplier_Id") %>' OnCommand="ImgSupplierDetail_Command" CausesValidation="False"><i class="fas fa-file-invoice"></i></asp:LinkButton>
                                                                            </ItemTemplate>
                                                                            <ItemStyle HorizontalAlign="Center" />
                                                                        </asp:TemplateField>
                                                                        <asp:TemplateField HeaderText="<%$ Resources:Attendance, Supplier %>">
                                                                            <ItemTemplate>
                                                                                <table width="100%">
                                                                                    <tr>
                                                                                        <td width="80%">
                                                                                            <asp:HiddenField ID="hdngvSupplierId" runat="server" Value='<%#Eval("Supplier_Id") %>' />
                                                                                            <asp:Label ID="lblgvSupplierName" runat="server" Text='<%#Set_Suppliers.GetSupplierName(Eval("Supplier_Id").ToString(),Session["DBConnection"].ToString()) %>' />
                                                                                        </td>
                                                                                        <td align="right" width="20%">
                                                                                            <a onclick="lnkSupplierdetail_Command('<%# Eval("Supplier_Id") %>')" style="cursor: pointer; color: blue">More..
                                                                                            </a>
                                                                                            <%--<asp:LinkButton ID="lnkSupplierdetail" runat="server" Text="<%$ Resources:Attendance,More.. %>" ForeColor="Blue"
                                                                                                CommandArgument='<%# Eval("Supplier_Id") %>' OnCommand="lnkSupplierdetail_Command"
                                                                                                ToolTip="Supplier History" />--%>
                                                                                        </td>
                                                                                    </tr>
                                                                                </table>
                                                                            </ItemTemplate>
                                                                            <ItemStyle HorizontalAlign="Center" />
                                                                        </asp:TemplateField>
                                                                        <asp:TemplateField HeaderText="<%$ Resources:Attendance, Amount %>">
                                                                            <ItemTemplate>
                                                                                <asp:Label ID="lblgvAmount" runat="server" Text='<%#  GetAmountDecimal(Eval("Amount").ToString()) %>' />
                                                                            </ItemTemplate>
                                                                            <ItemStyle HorizontalAlign="Center" />
                                                                        </asp:TemplateField>
                                                                        <asp:TemplateField HeaderText="<%$ Resources:Attendance,Vendor Quotation No.%>">
                                                                            <ItemTemplate>
                                                                                <asp:Label ID="txtVendorQuotationNo" runat="server" Text='<%#Eval("Field1") %>'></asp:Label>
                                                                            </ItemTemplate>
                                                                            <ItemStyle HorizontalAlign="Center" />
                                                                        </asp:TemplateField>
                                                                    </Columns>



                                                                </asp:GridView>
                                                                <asp:HiddenField ID="hdnSupplierId" runat="server" Value="0" />
                                                            </div>
                                                            <br />
                                                        </div>
                                                        <div class="col-md-12" style="text-align: center">
                                                            <asp:Button ID="btnQuoteSave" ValidationGroup="Save" runat="server" Text="<%$ Resources:Attendance,Save %>"
                                                                CssClass="btn btn-success" OnClick="btnQuoteSave_Click" Visible="False" />
                                                            <asp:Button ID="BtnReset" runat="server" Text="<%$ Resources:Attendance,Reset %>"
                                                                CssClass="btn btn-primary" CausesValidation="False" OnClick="BtnReset_Click" />
                                                            <asp:Button ID="btnQuoteCancel" runat="server" CssClass="btn btn-danger" Text="<%$ Resources:Attendance,Cancel %>"
                                                                CausesValidation="False" OnClick="btnQuoteCancel_Click" />

                                                            <br />
                                                        </div>
                                                    </div>
                                                    <div id="pnlProduct" runat="server" visible="false">
                                                        <asp:HiddenField ID="Hdn_Tax_By" runat="server" />
                                                        <div class="col-md-6">
                                                            <asp:Label ID="lblSup" Font-Size="14px" runat="server" Text="<%$ Resources:Attendance,Supplier Name %>" />
                                                            <asp:Label ID="Label12" Font-Size="14px" runat="server" Text=":" />
                                                            <asp:Label ID="lblsupplierName" Font-Size="14px" Font-Bold="true" runat="server" />
                                                            <br />
                                                        </div>
                                                        <div class="col-md-6">
                                                            <asp:Label ID="Label11" Font-Size="14px" runat="server" Text="<%$ Resources:Attendance,Supplier Code %>" />
                                                            <asp:Label ID="Label15" Font-Size="14px" runat="server" Text=":" />
                                                            <asp:Label ID="lblSuppliercode" Font-Size="14px" Font-Bold="true" runat="server" />
                                                            <br />
                                                        </div>
                                                        <div class="col-md-12">
                                                            <br />
                                                        </div>
                                                        <div class="col-md-6">
                                                            <asp:Label ID="Label26" runat="server" Text="<%$ Resources:Attendance,Vendor Quotation No. %>" />
                                                            <asp:TextBox ID="txtVendorQuotationNo" CssClass="form-control" runat="server" />
                                                            <br />
                                                        </div>
                                                        <div class="col-md-6">
                                                            <asp:Label ID="Label16" runat="server" Text="<%$ Resources:Attendance,Currency %>"></asp:Label>
                                                            <asp:DropDownList ID="ddlCurrency" runat="server" CssClass="form-control">
                                                            </asp:DropDownList>
                                                            <br />
                                                        </div>
                                                        <div class="col-md-6" id="Trans_Div" runat="server">
                                                            <asp:Label ID="lblTransType" runat="server" Text="Transaction Type"></asp:Label>
                                                            <%--<a style="color: Red">*</a>--%>
                                                            <asp:DropDownList ID="ddlTransType" Enabled="false" OnSelectedIndexChanged="ddlTransType_SelectedIndexChanged" AutoPostBack="true" runat="server" CssClass="form-control"></asp:DropDownList>
                                                            <%--<asp:RequiredFieldValidator EnableClientScript="true" Style="float: right;" runat="server" ID="RequiredFieldValidator5" ValidationGroup="Save" Display="Dynamic"
                                                                SetFocusOnError="true" ControlToValidate="ddlTransType" InitialValue="-1" ErrorMessage="<%$ Resources:Attendance,Select Transaction Type%>" />--%>
                                                            <br />
                                                        </div>
                                                        <div class="col-md-12">
                                                            <div class="col-md-12" runat="server" id="scrollArea" onscroll="SetDivPosition()" style="overflow: auto; max-height: 900px;">
                                                                <asp:DataList ID="dlProductDetail" ClientIDMode="AutoID" Width="100%" runat="server">
                                                                    <ItemTemplate>
                                                                        <div class="col-md-12">
                                                                            <div class="box box-primary">
                                                                                <div class="box-header with-border">
                                                                                    <h3 class="box-title">
                                                                                        <asp:Label ID="Label35" runat="server" Text='<%#new Inv_ProductMaster(Session["DBConnection"].ToString()).GetProductNamebyProductId(Eval("Product_Id").ToString()) %>'></asp:Label></h3>
                                                                                    <%--<div class="box-tools pull-right">
                                                                                        <button type="button" class="btn btn-box-tool" data-widget="collapse">
                                                                                            <i class="fa fa-minus"></i>
                                                                                        </button>
                                                                                    </div>--%>
                                                                                </div>
                                                                                <div class="box-body">
                                                                                    <div class="form-group">
                                                                                        <div class="col-md-6">
                                                                                            <asp:Label ID="Label14" runat="server" Text="<%$ Resources:Attendance,Product Id %>" />
                                                                                            &nbsp:&nbsp
                                                                                            <asp:Label ID="txtproductCode" Font-Bold="true" runat="server" Text='<%#new Inv_ProductMaster(Session["DBConnection"].ToString()).GetProductCodebyProductId(Eval("Product_Id").ToString()) %>' />
                                                                                            <asp:Label ID="Lbl_Product_ID" Visible="false" Text='<%#Eval("Product_Id").ToString() %>' runat="server" />
                                                                                            <br />
                                                                                        </div>
                                                                                        <div class="col-md-6">
                                                                                            <asp:Label ID="Label15" runat="server" Text="Stock qty" />
                                                                                            &nbsp:&nbsp
                                                                                            <a onclick="lnkStockInfo_Command('<%# Eval("Product_Id") %>')" style="cursor: pointer; color: blue;"><%#GetProductStock(Eval("Product_Id").ToString()) %></a>

                                                                                            <%--<asp:LinkButton ID="lnkStockInfo" runat="server" Text='<%#GetProductStock(Eval("Product_Id").ToString()) %>'
                                                                                                Font-Underline="true" ToolTip="View Detail" OnCommand="lnkStockInfo_Command"
                                                                                                ForeColor="Blue" CommandArgument='<%# Eval("Product_Id") %>'></asp:LinkButton>--%>
                                                                                            <br />
                                                                                        </div>
                                                                                        <div class="col-md-12">
                                                                                            <br />
                                                                                        </div>
                                                                                        <div class="col-md-12">
                                                                                            <asp:Label ID="lblProductName" runat="server" Text="<%$ Resources:Attendance,Product Name %>" />
                                                                                            &nbsp:&nbsp<asp:Label ID="txtProductName" runat="server" Font-Bold="true" Text='<%#new Inv_ProductMaster(Session["DBConnection"].ToString()).GetProductNamebyProductId(Eval("Product_Id").ToString()) %>' />
                                                                                            <asp:HiddenField ID="hdngvProductId" runat="server" Value='<%#Eval("Product_Id") %>' />
                                                                                            <asp:HiddenField ID="hdnSerialNo" runat="server" Value='<%#Eval("Serial_No") %>' />
                                                                                            <asp:HiddenField ID="hdnPINO" runat="server" Value='<%#Eval("PI_No") %>' />
                                                                                            <asp:HiddenField ID="hdnSuggestedProductName" runat="server" Value='<%# Eval("SuggestedProductName") %>' />
                                                                                            <br />
                                                                                        </div>
                                                                                        <div class="col-md-12">
                                                                                            <br />
                                                                                        </div>
                                                                                        <div class="col-md-6">
                                                                                            <asp:Label ID="lblRefProductName" runat="server" Text="<%$ Resources:Attendance,Referenced Product Name %>" />
                                                                                            <asp:TextBox ID="txtRefProductName" runat="server" CssClass="form-control" />
                                                                                            <br />
                                                                                        </div>
                                                                                        <div class="col-md-6">
                                                                                            <asp:Label ID="lblRefPartNo" runat="server" Text="<%$ Resources:Attendance,Referenced Part No. %>" />
                                                                                            <asp:TextBox ID="txtRefPartNo" runat="server" CssClass="form-control" />
                                                                                            <br />
                                                                                        </div>
                                                                                        <div class="col-md-3">
                                                                                            <asp:Label ID="lblUnitPrice" runat="server" Text='<%$ Resources:Attendance,Unit Price %>' />
                                                                                            <asp:TextBox ID="txtUnitPrice" runat="server" CssClass="form-control" onchange="SetSelectedRow(this)"
                                                                                                OnTextChanged="txtUnitPrice_TextChanged" AutoPostBack="true" Text="0" />
                                                                                            <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" runat="server" Enabled="True"
                                                                                                TargetControlID="txtUnitPrice" ValidChars="1,2,3,4,5,6,7,8,9,0,.">
                                                                                            </cc1:FilteredTextBoxExtender>
                                                                                            <br />
                                                                                        </div>
                                                                                        <div class="col-md-3">
                                                                                            <asp:Label ID="lblUnit" runat="server" Text="<%$ Resources:Attendance,Unit %>" />
                                                                                            <asp:Label ID="txtUnit" runat="server" CssClass="form-control" Text='<%# Inv_UnitMaster.GetUnitCode(Eval("UnitId").ToString(),Session["DBConnection"].ToString(),HttpContext.Current.Session["CompId"].ToString()) %>' />
                                                                                            <asp:HiddenField ID="hdngvUnitId" runat="server" Value='<%#Eval("UnitId") %>' />
                                                                                            <br />
                                                                                        </div>
                                                                                        <div class="col-md-3">
                                                                                            <asp:Label ID="lblRequiredQty" runat="server" Text="<%$ Resources:Attendance,Required Quantity %>" />
                                                                                            <asp:Label ID="txtRequiredQuantity" CssClass="form-control" runat="server" Text='<%#Eval("ReqQty") %>' />
                                                                                            <br />
                                                                                        </div>
                                                                                        <div class="col-md-3">
                                                                                            <asp:Label ID="lblQtyAmount" runat="server" Text='<%$ Resources:Attendance,Gross Price %>' />
                                                                                            <asp:Label ID="txtQtyPrice" CssClass="form-control" runat="server" Font-Bold="true"></asp:Label>
                                                                                            <br />
                                                                                        </div>

                                                                                        <div class="col-md-3 Div_Discount_P">
                                                                                            <asp:Label ID="lblDiscountP" runat="server" Text='<%$ Resources:Attendance,Discount(%) %>' />
                                                                                            <div class="input-group">
                                                                                                <asp:TextBox ID="txtDiscountP" runat="server" onchange="SetSelectedRow(this)" CssClass="form-control" OnTextChanged="txtDiscountP_TextChanged" AutoPostBack="true" Text="0" />
                                                                                                <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender5" runat="server" Enabled="True"
                                                                                                    TargetControlID="txtDiscountP" ValidChars="1,2,3,4,5,6,7,8,9,0,.">
                                                                                                </cc1:FilteredTextBoxExtender>
                                                                                                <div class="input-group-addon">
                                                                                                    <asp:Label ID="Label10" runat="server" Text="%" />
                                                                                                </div>
                                                                                            </div>
                                                                                            <br />
                                                                                        </div>

                                                                                        <div class="col-md-3 Div_Discount_V">
                                                                                            <asp:Label ID="Label18" Width="100px" runat="server" Text="<%$ Resources:Attendance,Discount Value %>" />
                                                                                            <asp:TextBox ID="txtDiscountV" runat="server" onchange="SetSelectedRow(this)" CssClass="form-control" OnTextChanged="txtDiscountV_TextChanged" AutoPostBack="true" Text="0" />
                                                                                            <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender6" runat="server" Enabled="True"
                                                                                                TargetControlID="txtDiscountV" ValidChars="1,2,3,4,5,6,7,8,9,0,.">
                                                                                            </cc1:FilteredTextBoxExtender>
                                                                                        </div>

                                                                                        <div class="col-md-3 Div_Tax_P">
                                                                                            <asp:Label ID="lblTaxP" runat="server" Text='<%$ Resources:Attendance,Tax(%)%>' />
                                                                                            <div class="input-group">
                                                                                                <asp:TextBox ID="txtTaxP" onchange="SetSelectedRow(this)" runat="server" Enabled="false" CssClass="form-control" Text="0" />
                                                                                                <%--<asp:TextBox ID="txtTaxP" runat="server" CssClass="form-control" OnTextChanged="txtTaxP_TextChanged" AutoPostBack="true" Text="0" />--%>
                                                                                                <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender2" runat="server" Enabled="True"
                                                                                                    TargetControlID="txtTaxP" ValidChars="1,2,3,4,5,6,7,8,9,0,.">
                                                                                                </cc1:FilteredTextBoxExtender>
                                                                                                <div class="input-group-btn">
                                                                                                    <asp:ImageButton ID="BtnAddTax" runat="server" CommandName="dlProductDetail" CommandArgument='<%# Eval("Product_Id") %>' Style="border: 1px solid #ccc" OnCommand="BtnAddTax_Command" ImageUrl="~/Images/plus.png" ToolTip="View Tax" />
                                                                                                </div>
                                                                                                <div class="input-group-addon">
                                                                                                    <asp:Label ID="Label2" runat="server" Text="%" />
                                                                                                </div>
                                                                                            </div>
                                                                                            <br />
                                                                                        </div>

                                                                                        <div class="col-md-3 Div_Tax_V">
                                                                                            <asp:Label ID="Label19" runat="server" Text="<%$ Resources:Attendance,Tax Value %>" />
                                                                                            <asp:TextBox ID="txtTaxV" onchange="SetSelectedRow(this)" runat="server" Enabled="false" CssClass="form-control" Text="0" />
                                                                                            <%--<asp:TextBox ID="txtTaxV" runat="server" CssClass="form-control" OnTextChanged="txtTaxV_TextChanged" AutoPostBack="true" Text="0" />--%>
                                                                                            <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender3" runat="server" Enabled="True"
                                                                                                TargetControlID="txtTaxV" ValidChars="1,2,3,4,5,6,7,8,9,0,.">
                                                                                            </cc1:FilteredTextBoxExtender>
                                                                                            <br />
                                                                                        </div>


                                                                                        <div class="col-md-6" visible="false" runat="server" id="Div_Pryce_After_Tax">
                                                                                            <asp:Label ID="lblPriceAfterTax" runat="server" Visible="false" Text="<%$ Resources:Attendance,Price After Tax %>" />
                                                                                            <asp:TextBox ID="txtPriceAfterTax" onchange="SetSelectedRow(this)" runat="server" CssClass="form-control" ReadOnly="True"
                                                                                                Visible="false" Text="0" />
                                                                                            <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender4" runat="server" Enabled="True"
                                                                                                TargetControlID="txtPriceAfterTax" ValidChars="1,2,3,4,5,6,7,8,9,0,.">
                                                                                            </cc1:FilteredTextBoxExtender>
                                                                                            <br />
                                                                                        </div>

                                                                                        <div class="col-md-6" id="Div_Net_Price" runat="server" visible="false">
                                                                                            <asp:Label ID="lblAmount" runat="server" Text="<%$ Resources:Attendance,Net Price %>" />
                                                                                            <asp:TextBox ID="txtAmount" onchange="SetSelectedRow(this)" runat="server" CssClass="form-control" ReadOnly="True" />
                                                                                            <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender7" runat="server" Enabled="True"
                                                                                                TargetControlID="txtAmount" ValidChars="1,2,3,4,5,6,7,8,9,0,.">
                                                                                            </cc1:FilteredTextBoxExtender>
                                                                                            <br />
                                                                                        </div>
                                                                                        <div class="col-md-6">
                                                                                            <asp:Label ID="Label17" runat="server" Text='<%#SystemParameter.GetCurrencySmbol(Session["LocCurrencyId"].ToString(), "Net Price",Session["DBConnection"].ToString()) %>' />
                                                                                            <asp:TextBox ID="txtNetAmountPICurrency" onchange="SetSelectedRow(this)" runat="server" CssClass="form-control" ReadOnly="True" />
                                                                                            <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender8" runat="server" Enabled="True"
                                                                                                TargetControlID="txtNetAmountPICurrency" ValidChars="1,2,3,4,5,6,7,8,9,0,.">
                                                                                            </cc1:FilteredTextBoxExtender>
                                                                                            <br />
                                                                                        </div>
                                                                                        <div class="col-md-12">
                                                                                            <asp:Label ID="lblTermsCondition" runat="server" Text="<%$ Resources:Attendance,Terms & Conditions %>" />
                                                                                            <asp:TextBox ID="txtTermsCondition" Style="resize: vertical; max-height: 200px; min-height: 50px;" runat="server" CssClass="form-control" TextMode="MultiLine" Height="90px" />
                                                                                            <br />
                                                                                        </div>
                                                                                        <div class="col-md-12">
                                                                                            <asp:Label ID="lblProductDescription" runat="server" Text="<%$ Resources:Attendance,Product Description %>" />
                                                                                            &nbsp:&nbsp<asp:Label ID="txtProductDescription" Font-Bold="true" runat="server" Text='<%#Eval("ProductDescription") %>' />
                                                                                            <br />
                                                                                        </div>
                                                                                    </div>
                                                                                </div>
                                                                            </div>
                                                                        </div>
                                                                    </ItemTemplate>
                                                                </asp:DataList>
                                                            </div>
                                                            <br />
                                                        </div>
                                                        <div class="col-md-12" style="text-align: center">
                                                            <asp:Button ID="btnProductSave" runat="server" ValidationGroup="Save" Text="<%$ Resources:Attendance,Save %>"
                                                                CssClass="btn btn-success" OnClick="btnProductSave_Click" Visible="False" />
                                                            <asp:Button ID="btnProductCancel" runat="server" CssClass="btn btn-primary" Text="<%$ Resources:Attendance,Close %>"
                                                                CausesValidation="False" OnClick="btnProductCancel_Click" />
                                                            <asp:HiddenField ID="HiddenField3" runat="server" />
                                                            <br />
                                                        </div>
                                                    </div>
                                                    <div id="pnlSalesQuotation" runat="server" visible="false">
                                                        <div class="col-md-12">
                                                            <div style="overflow: auto; max-height: 500px;">
                                                                <asp:DataList ID="datalistProduct" runat="server" OnDataBinding="datalistProduct_DataBinding"
                                                                    OnItemDataBound="datalistProduct_ItemDataBound">
                                                                    <ItemTemplate>
                                                                        <div class="col-md-12">
                                                                            <asp:Label ID="lblProductName" runat="server" Text="<%$ Resources:Attendance,Product Name %>"></asp:Label>
                                                                            &nbsp:&nbsp<asp:Label ID="txtProductName" runat="server" Font-Bold="true"
                                                                                Text='<%# ProductName(Eval("Product_Id").ToString(),Eval("Product_description").ToString()) %>'></asp:Label>
                                                                            <asp:Label ID="lblProductId" Visible="false" runat="server"
                                                                                Font-Bold="true" Text='<%# Eval("Product_Id") %>'></asp:Label>
                                                                            <br />
                                                                        </div>
                                                                        <div class="col-md-12">
                                                                            <br />
                                                                        </div>
                                                                        <div class="col-md-12">
                                                                            <div style="overflow: auto; max-height: 500px;">
                                                                                <asp:GridView CssClass="table-striped table-bordered table table-hover" ID="gvSupplier" runat="server" BackColor="White" CellPadding="4" ForeColor="Black"
                                                                                    GridLines="Horizontal" AutoGenerateColumns="false" Width="100%">
                                                                                    <Columns>
                                                                                        <asp:TemplateField FooterStyle-VerticalAlign="Middle" HeaderText="<%$ Resources:Attendance,Serial No %>"
                                                                                            ItemStyle-Width="70px" ItemStyle-BorderStyle="Solid" ItemStyle-BorderWidth="1px">
                                                                                            <ItemTemplate>
                                                                                                <asp:Label ID="lblgvSupplierName1" Font-Size="13px" ForeColor="#474646" Font-Names="Trebuchet MS"
                                                                                                    runat="server" Text="<%#Container.DataItemIndex+1 %>"></asp:Label>
                                                                                            </ItemTemplate>
                                                                                            <HeaderStyle BorderStyle="Solid" BorderWidth="1px" Font-Size="13px" ForeColor="#474646"
                                                                                                Font-Names="Trebuchet MS" Font-Bold="true" />
                                                                                        </asp:TemplateField>
                                                                                        <asp:TemplateField FooterStyle-VerticalAlign="Middle" HeaderText="<%$ Resources:Attendance,Supplier Name %>"
                                                                                            ItemStyle-Width="120px" ItemStyle-BorderStyle="Solid" ItemStyle-BorderWidth="1px">
                                                                                            <ItemTemplate>
                                                                                                <table width="100%">
                                                                                                    <tr>
                                                                                                        <td width="80%">
                                                                                                            <asp:HiddenField ID="hdngvSupplierId" runat="server" Value='<%#Eval("Supplier_Id") %>' />
                                                                                                            <asp:Label ID="lblgvSupplierName2" runat="server" Font-Size="13px" ForeColor="#474646"
                                                                                                                Font-Names="Trebuchet MS" Text='<%#Eval("SupplierName").ToString() %>' />
                                                                                                        </td>
                                                                                                        <td align="right" width="20%">
                                                                                                            <asp:LinkButton ID="lnkSupplierdetail" runat="server" Text="More.." ForeColor="Blue"
                                                                                                                CommandArgument='<%# Eval("Supplier_Id") %>' OnCommand="lnkSupplierdetail_Command"
                                                                                                                ToolTip="Supplier History" />
                                                                                                        </td>
                                                                                                    </tr>
                                                                                                </table>
                                                                                            </ItemTemplate>
                                                                                            <HeaderStyle BorderStyle="Solid" BorderWidth="1px" Font-Size="13px" Font-Bold="true"
                                                                                                ForeColor="#474646" Font-Names="Trebuchet MS" />
                                                                                        </asp:TemplateField>
                                                                                        <asp:TemplateField HeaderText="<%$ Resources:Attendance,Terms & Conditions %>" ItemStyle-Width="400px"
                                                                                            ItemStyle-BorderStyle="Solid" ItemStyle-BorderWidth="1px">
                                                                                            <ItemTemplate>
                                                                                                <asp:Label ID="lblgvTermCondition" runat="server" Font-Size="13px" ForeColor="#474646"
                                                                                                    Font-Names="Trebuchet MS" Text='<%# Eval("TermsCondition") %>' />
                                                                                            </ItemTemplate>
                                                                                            <HeaderStyle BorderStyle="Solid" BorderWidth="1px" Font-Size="13px" Font-Bold="true"
                                                                                                ForeColor="#474646" Font-Names="Trebuchet MS" />
                                                                                        </asp:TemplateField>
                                                                                        <asp:TemplateField HeaderText="<%$ Resources:Attendance,Amount %>" ItemStyle-Width="80px"
                                                                                            ItemStyle-BorderStyle="Solid" ItemStyle-BorderWidth="1px">
                                                                                            <ItemTemplate>
                                                                                                <asp:Label ID="lblAmmount" runat="server" Font-Size="13px" ForeColor="#474646" Font-Names="Trebuchet MS"
                                                                                                    Text='<%# Eval("Amount") %>' />
                                                                                            </ItemTemplate>
                                                                                            <HeaderStyle BorderStyle="Solid" BorderWidth="1px" Font-Size="13px" ForeColor="#474646"
                                                                                                Font-Names="Trebuchet MS" Font-Bold="true" />
                                                                                        </asp:TemplateField>
                                                                                    </Columns>
                                                                                    <FooterStyle BackColor="#CCCC99" ForeColor="Black" />
                                                                                    <PagerStyle BackColor="White" ForeColor="Black" HorizontalAlign="Right" />
                                                                                    <SelectedRowStyle BackColor="#CC3333" Font-Bold="True" ForeColor="White" />
                                                                                    <HeaderStyle Font-Bold="True" ForeColor="Black" />
                                                                                </asp:GridView>
                                                                            </div>
                                                                            <br />
                                                                        </div>
                                                                        <div class="col-md-12">
                                                                            <asp:Label ID="Label3" runat="server" Text="<%$ Resources:Attendance,Sales Price %>"></asp:Label>
                                                                            <asp:TextBox ID="txtSalesPrice" runat="server" CssClass="form-control" />
                                                                            <br />
                                                                        </div>
                                                                        <div class="col-md-12">
                                                                            <asp:Label ID="Label4" runat="server" Text="<%$ Resources:Attendance,Terms & Conditions %>"></asp:Label>
                                                                            <asp:TextBox ID="txtSalesDescription" runat="server" TextMode="MultiLine" Height="50px" CssClass="form-control" Font-Names="Arial" />
                                                                            <br />
                                                                        </div>
                                                                    </ItemTemplate>
                                                                </asp:DataList>
                                                            </div>
                                                            <br />
                                                        </div>
                                                        <div class="col-md-12" style="text-align: center">
                                                            <asp:Button ID="btnSalesSave" runat="server" Text="<%$ Resources:Attendance,Save %>"
                                                                CssClass="btn btn-success" OnClick="btnSalesSave_Click" />&nbsp;&nbsp;&nbsp;
                                                            <asp:Button ID="btnCancel" runat="server" Text="<%$ Resources:Attendance,Cancel %>"
                                                                CssClass="btn btn-danger" OnClick="btnCancel_Click" />
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
                                                    <asp:Label ID="Label20" runat="server" Text="Advance Search"></asp:Label></h3>
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
                                                    <asp:DropDownList ID="ddlFieldNameBin" runat="server" CssClass="form-control" AutoPostBack="true" OnSelectedIndexChanged="ddlFieldNameBin_SelectedIndexChanged">
                                                        <asp:ListItem Text="<%$ Resources:Attendance, Quotation No. %>" Value="RPQ_No" Selected="True"></asp:ListItem>
                                                        <asp:ListItem Text="<%$ Resources:Attendance, Quotation Date %>" Value="RPQ_Date"></asp:ListItem>
                                                        <asp:ListItem Text="<%$ Resources:Attendance, Inquiry No. %>" Value="Pinq_No"></asp:ListItem>
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
                                                    <asp:Panel ID="Panel2" runat="server" DefaultButton="btnbindBin">
                                                        <asp:TextBox ID="txtValueBin" runat="server" CssClass="form-control" placeholder="Search From Content"></asp:TextBox>
                                                        <asp:TextBox ID="txtValueBinDate" runat="server" CssClass="form-control" Visible="false" placeholder="Search From Date"></asp:TextBox>
                                                        <cc1:CalendarExtender OnClientShown="showCalendar" Format="dd-MMM-yyyy" ID="CalendarExtendertxtValueBinDate" runat="server" TargetControlID="txtValueBinDate" />
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
                                <div class="box box-warning box-solid" <%= GvPurchaseQuoteBin.Rows.Count>0?"style='display:block'":"style='display:none'"%>>
                                    <div class="box-body">
                                        <div class="row">
                                            <div class="col-md-12">
                                                <div class="flow">
                                                    <asp:Label ID="lblSelectedRecord" runat="server" Visible="false"></asp:Label>
                                                    <asp:GridView CssClass="table-striped table-bordered table table-hover" ID="GvPurchaseQuoteBin" PageSize="<%# PageControlCommon.GetPageSize() %>"
                                                        runat="server" AutoGenerateColumns="False" Width="100%" AllowPaging="True" OnPageIndexChanging="GvPurchaseQuoteBin_PageIndexChanging"
                                                        OnSorting="GvPurchaseQuoteBin_OnSorting" AllowSorting="true">
                                                        <Columns>
                                                            <asp:TemplateField>
                                                                <HeaderTemplate>
                                                                    <asp:CheckBox ID="chkCurrent" runat="server" OnCheckedChanged="chkCurrent_CheckedChanged"
                                                                        AutoPostBack="true" />
                                                                </HeaderTemplate>
                                                                <ItemTemplate>
                                                                    <asp:CheckBox ID="chkSelect" runat="server" AutoPostBack="true" OnCheckedChanged="chkSelect_CheckedChanged" />
                                                                </ItemTemplate>
                                                                <ItemStyle HorizontalAlign="Center" Width="5%" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,Quotation No. %>" SortExpression="RPQ_No">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblgvRPQNo" runat="server" Text='<%#Eval("RPQ_No") %>' />
                                                                    <asp:HiddenField ID="hdnTransId" runat="server" Value='<%#Eval("Trans_Id") %>' />
                                                                </ItemTemplate>
                                                                <ItemStyle HorizontalAlign="Center" Width="8%" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,Quotation Date %>" SortExpression="RPQ_Date">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblgvRPQDate" runat="server" Text='<%#Convert.ToDateTime(Eval("RPQ_Date").ToString()).ToString("dd-MMM-yyyy") %>' />
                                                                </ItemTemplate>
                                                                <ItemStyle HorizontalAlign="Center" Width="8%" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,Purchase Inquiry No. %>"
                                                                SortExpression="Pinq_No">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblgvPINo" runat="server" Text='<%#Eval("Pinq_No") %>' />
                                                                </ItemTemplate>
                                                                <ItemStyle HorizontalAlign="Center" Width="10%" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,Supplier %>" SortExpression="Supplier_Name">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblgvBinSupplier" runat="server" Text='<%#Eval("Supplier_Name") %>' />
                                                                </ItemTemplate>
                                                                <ItemStyle HorizontalAlign="Center" Width="25%" />
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
                                                                <ItemStyle Width="10%" HorizontalAlign="Left" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,Total Amount %>" SortExpression="TotalAmount">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblgvTotalAmount" runat="server" Text='<%# GetCurrencySymbol(Eval("TotalAmount").ToString(),Eval("Field1").ToString()) %>' />
                                                                </ItemTemplate>
                                                                <ItemStyle HorizontalAlign="Right" Width="10%" />
                                                            </asp:TemplateField>
                                                        </Columns>




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

                    <div class="tab-pane" id="Inquiry">
                        <asp:UpdatePanel ID="Update_Inquiry" runat="server" UpdateMode="Conditional">
                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="Btn_Inquiry" EventName="Click" />
                            </Triggers>
                            <ContentTemplate>

                                <div class="row">
                                    <div class="col-md-12">
                                        <div id="Div3" runat="server" class="box box-info collapsed-box">
                                            <div class="box-header with-border">
                                                <h3 class="box-title">
                                                    <asp:Label ID="Label21" runat="server" Text="Advance Search"></asp:Label></h3>
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
                                                    <asp:DropDownList ID="ddlFieldNameRequest" runat="server" CssClass="form-control"
                                                        OnSelectedIndexChanged="ddlFieldNameRequest_SelectedIndexChanged"
                                                        AutoPostBack="true">
                                                        <asp:ListItem Text="<%$ Resources:Attendance, Inquiry No. %>" Value="PI_No" Selected="True"></asp:ListItem>
                                                        <asp:ListItem Text="<%$ Resources:Attendance, Inquiry Date %>" Value="PIDate"></asp:ListItem>
                                                        <asp:ListItem Text="<%$ Resources:Attendance,Transfer From %>" Value="RefType"></asp:ListItem>
                                                        <asp:ListItem Text="<%$ Resources:Attendance,Created By %>" Value="CreatedUser"></asp:ListItem>
                                                        <asp:ListItem Text="<%$ Resources:Attendance,Modified By %>" Value="ModifiedUser"></asp:ListItem>
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
                                                        <cc1:CalendarExtender OnClientShown="showCalendar" Format="dd-MMM-yyyy" ID="CalendarExtendertxtValueRequestDate" runat="server" TargetControlID="txtValueRequestDate" />
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


                                <div class="box box-warning box-solid" <%= GvPurchaseInquiry.Rows.Count>0?"style='display:block'":"style='display:none'"%>>
                                    <div class="box-body">
                                        <div class="row">
                                            <div class="col-md-12">
                                                <div class="flow">
                                                    <asp:HiddenField ID="HDFSort" runat="server" />
                                                    <asp:GridView CssClass="table-striped table-bordered table table-hover" ID="GvPurchaseInquiry" PageSize="<%# PageControlCommon.GetPageSize() %>" CurrentSortField="PIDate" CurrentSortDirection="DESC"
                                                        runat="server" AutoGenerateColumns="False" Width="100%" AllowPaging="false"
                                                        AllowSorting="True" OnSorting="GvPurchaseInquiry_Sorting">

                                                        <Columns>
                                                            <asp:TemplateField>
                                                                <ItemTemplate>
                                                                    <asp:Button ID="btnEdit" runat="server" BorderStyle="None" BackColor="Transparent"
                                                                        CausesValidation="False" CssClass="btnPull" CommandArgument='<%# Eval("Trans_Id") %>'
                                                                        OnCommand="btnPREdit_Command" />
                                                                </ItemTemplate>
                                                                <ItemStyle HorizontalAlign="Center" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,Inquiry Id %>" SortExpression="Trans_Id"
                                                                Visible="false">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblgvInquiryId" runat="server" Text='<%#Eval("Trans_Id") %>' />
                                                                </ItemTemplate>
                                                                <ItemStyle HorizontalAlign="Left" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,Inquiry No. %>" SortExpression="PI_No">
                                                                <ItemTemplate>
                                                                    <%#Eval("PI_No") %>
                                                                </ItemTemplate>
                                                                <ItemStyle HorizontalAlign="Left" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,Inquiry Date %>" SortExpression="PIDate">
                                                                <ItemTemplate>
                                                                    <%#Convert.ToDateTime(Eval("PIDate").ToString()).ToString("dd-MMM-yyyy") %>
                                                                </ItemTemplate>
                                                                <ItemStyle HorizontalAlign="Left" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField Visible="true" HeaderText="<%$ Resources:Attendance,Transfer From %>"
                                                                SortExpression="RefType">
                                                                <ItemTemplate>
                                                                    <%# Eval("RefType")%>
                                                                </ItemTemplate>
                                                                <ItemStyle HorizontalAlign="Left" />
                                                            </asp:TemplateField>

                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,Created By %>" SortExpression="CreatedUser">
                                                                <ItemTemplate>
                                                                    <%#Eval("CreatedUser").ToString() %>
                                                                </ItemTemplate>
                                                                <ItemStyle HorizontalAlign="Left" />
                                                            </asp:TemplateField>

                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,Modified By %>" SortExpression="ModifiedUser">
                                                                <ItemTemplate>
                                                                    <%#Eval("ModifiedUser").ToString() %>
                                                                </ItemTemplate>
                                                                <ItemStyle HorizontalAlign="Left" />
                                                            </asp:TemplateField>

                                                        </Columns>



                                                    </asp:GridView>
                                                </div>
                                                <div class="col-md-12">
                                                    <asp:HiddenField ID="hdnInquiryIndexValue" runat="server" Value="1" />

                                                    <asp:Repeater ID="RPTPager_Inquiry" runat="server">
                                                        <ItemTemplate>
                                                            <ul class="pagination">
                                                                <li class='<%# Convert.ToBoolean(Eval("Enabled")) ? "page-item" : "page-item active" %>'>
                                                                    <asp:LinkButton ID="lnkPage" runat="server" Text='<%#Eval("Text") %>' CommandArgument='<%# Eval("Value") %>'
                                                                        CssClass="page-link"
                                                                        OnClick="RPTPager_Inquiry_Changed" OnClientClick='<%# !Convert.ToBoolean(Eval("Enabled")) ? "return false;" : "" %>'></asp:LinkButton>
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
    <div class="modal fade" id="Purchase_Quotation" tabindex="-1" role="dialog" aria-labelledby="Purchase_QuotationLabel" aria-hidden="true">
        <div class="modal-dialog modal-lg">
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal">
                        <span aria-hidden="true">&times;</span><span class="sr-only">Close</span></button>
                    <h4 class="modal-title" id="Purchase_QuotationLabel">View Quotation</h4>
                </div>
                <div class="modal-body">
                    <asp:UpdatePanel ID="update_Modal" runat="server">
                        <ContentTemplate>
                            <div class="row">
                                <div class="col-md-12">
                                    <div class="box box-primary">
                                        <div class="box-body">
                                            <div class="form-group">
                                                <div class="col-md-12">
                                                    <div class="col-md-6">
                                                        <asp:Label ID="Label3" runat="server" Text="<%$ Resources:Attendance,Quotation No. %>"></asp:Label>
                                                        &nbsp:&nbsp
                                                    <asp:Label ID="lblQuotationNoView" Font-Bold="true" runat="server"></asp:Label>
                                                        <br />
                                                    </div>
                                                    <div class="col-md-6">
                                                        <asp:Label ID="Label4" runat="server" Text="<%$ Resources:Attendance,Quotation Date %>"></asp:Label>
                                                        &nbsp:&nbsp
                                                    <asp:Label ID="lblRPQDateView" Font-Bold="true" runat="server"></asp:Label>
                                                        <br />
                                                    </div>
                                                    <div class="col-md-6">
                                                        <asp:Label ID="Label7" runat="server" Text="<%$ Resources:Attendance,Inquiry No. %>" />
                                                        &nbsp:&nbsp
                                                    <asp:Label ID="lblInquiryNoView" Font-Bold="true" runat="server"></asp:Label>
                                                        <br />
                                                    </div>
                                                    <div class="col-md-6">
                                                        <asp:Label ID="Label8" runat="server" Text="<%$ Resources:Attendance,Inquiry Date%>" />
                                                        &nbsp:&nbsp
                                                    <asp:Label ID="lblInquiryDateView" runat="server" Font-Bold="true"></asp:Label>
                                                        <br />
                                                    </div>
                                                </div>
                                                <div id="pnldatalistDetail" runat="server" class="col-md-12">
                                                    <div style="overflow: auto; max-height: 500px;">
                                                        <asp:DataList ID="DataListView" runat="server" Width="100%" OnItemDataBound="datalistProductView_ItemDataBound">
                                                            <ItemTemplate>
                                                                <div class="col-md-12">
                                                                    <asp:Label ID="Label14" runat="server" Text="<%$ Resources:Attendance,Product Id %>" />
                                                                    &nbsp:&nbsp<asp:Label ID="txtproductCode" Font-Bold="true" runat="server" Text='<%#new Inv_ProductMaster(Session["DBConnection"].ToString()).GetProductCodebyProductId(Eval("Product_Id").ToString()) %>' />
                                                                    <br />
                                                                </div>
                                                                <div class="col-md-12">
                                                                    <asp:Label ID="lblProductName" runat="server" Text="<%$ Resources:Attendance,Product Name %>" />
                                                                    &nbsp:&nbsp<asp:Label ID="txtProductName" runat="server" Font-Bold="true" Text='<%#SuggestedProductName(Eval("Product_Id").ToString(),Eval("ProductDescription").ToString()) %>' />
                                                                    <asp:HiddenField ID="hdngvProductId" runat="server" Value='<%#Eval("Product_Id") %>' />
                                                                    <asp:HiddenField ID="hdnSuggestedProductName" runat="server" Value='<%# Eval("SuggestedProductName") %>' />
                                                                    <br />
                                                                </div>
                                                                <div class="col-md-6">
                                                                    <asp:Label ID="lblRefProductName" runat="server" Text="<%$ Resources:Attendance,Ref Product Name %>" />
                                                                    &nbsp:&nbsp<asp:Label ID="txtRefProductName" Font-Bold="true" Text='<%#Eval("RefrencedProductName") %>' runat="server"></asp:Label>
                                                                    <br />
                                                                </div>
                                                                <div class="col-md-6">
                                                                    <asp:Label ID="lblRefPartNo" runat="server" Text="<%$ Resources:Attendance,Referenced Part No. %>" />
                                                                    &nbsp:&nbsp<asp:Label ID="txtRefPartNo" Font-Bold="true" Text='<%#Eval("RefrencedPartNo") %>' runat="server"></asp:Label>
                                                                    <br />
                                                                </div>
                                                                <div class="col-md-12">
                                                                    <asp:Label ID="lblDetailView" runat="server" Text="<%$ Resources:Attendance,Detail %>"></asp:Label>
                                                                    <br />
                                                                </div>
                                                                <div class="col-md-12">
                                                                    <div style="overflow: auto; max-height: 500px;">
                                                                        <asp:GridView CssClass="table-striped table-bordered table table-hover" ID="gvSupplierView" runat="server" GridLines="Horizontal" AutoGenerateColumns="false"
                                                                            Width="100%">
                                                                            <Columns>
                                                                                <asp:TemplateField>
                                                                                    <ItemTemplate>
                                                                                        <table width="100%">
                                                                                            <tr>
                                                                                                <td>
                                                                                                    <asp:GridView CssClass="table-striped table-bordered table table-hover" ID="gvSupplierDetailView" runat="server" GridLines="Horizontal" AutoGenerateColumns="false"
                                                                                                        Width="100%">
                                                                                                        <Columns>
                                                                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,Serial No %>" HeaderStyle-Font-Size="12px">
                                                                                                                <ItemTemplate>
                                                                                                                    <asp:Label ID="lblgvSupplierName1" Font-Size="13px" ForeColor="#474646" Font-Names="Trebuchet MS"
                                                                                                                        runat="server" Text="<%#Container.DataItemIndex+1 %>"></asp:Label>
                                                                                                                </ItemTemplate>
                                                                                                                <ItemStyle HorizontalAlign="Center" Width="100px" />
                                                                                                            </asp:TemplateField>
                                                                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,Supplier Name %>" HeaderStyle-Font-Size="12px">
                                                                                                                <ItemTemplate>
                                                                                                                    <asp:Label ID="lblgvSupplierName2" Width="100px" runat="server" Font-Size="13px"
                                                                                                                        ForeColor="#474646" Font-Names="Trebuchet MS" Text='<%#Eval("SupplierName").ToString()%>' />
                                                                                                                </ItemTemplate>
                                                                                                                <ItemStyle HorizontalAlign="Center" Width="100px" />
                                                                                                            </asp:TemplateField>
                                                                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,Unit %>" HeaderStyle-Font-Size="12px">
                                                                                                                <ItemTemplate>
                                                                                                                    <asp:Label ID="lblunitview" runat="server" Width="50px" Font-Size="13px" ForeColor="#474646"
                                                                                                                        Font-Names="Trebuchet MS" Text='<%#Inv_UnitMaster.GetUnitCode(Eval("UnitId").ToString(),Session["DBConnection"].ToString(),HttpContext.Current.Session["CompId"].ToString()) %>' />
                                                                                                                </ItemTemplate>
                                                                                                                <ItemStyle HorizontalAlign="Center" Width="50px" />
                                                                                                            </asp:TemplateField>
                                                                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,Required Quantity  %>" HeaderStyle-Font-Size="12px">
                                                                                                                <ItemTemplate>
                                                                                                                    <asp:Label ID="lblreqqtyView" runat="server" Font-Size="13px" ForeColor="#474646"
                                                                                                                        Font-Names="Trebuchet MS" Text='<%#Eval("ReqQty") %>' />
                                                                                                                </ItemTemplate>
                                                                                                                <ItemStyle HorizontalAlign="Center" Width="150px" />
                                                                                                            </asp:TemplateField>
                                                                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,Unit Price %>" HeaderStyle-Font-Size="12px">
                                                                                                                <ItemTemplate>
                                                                                                                    <asp:Label ID="lblunitpriceView" runat="server" Font-Size="13px" ForeColor="#474646"
                                                                                                                        Font-Names="Trebuchet MS" Text='<%#GetAmountDecimal(Eval("UnitPrice").ToString()) %>' />
                                                                                                                </ItemTemplate>
                                                                                                                <ItemStyle HorizontalAlign="Center" Width="100px" />
                                                                                                            </asp:TemplateField>
                                                                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,Gross Price %>" HeaderStyle-Font-Size="12px">
                                                                                                                <ItemTemplate>
                                                                                                                    <asp:Label ID="lblQtypriceView" runat="server" Font-Size="13px" ForeColor="#474646"
                                                                                                                        Font-Names="Trebuchet MS" Text='<%#GetAmountDecimal((Convert.ToDouble(Eval("UnitPrice").ToString())*Convert.ToDouble(Eval("ReqQty").ToString())).ToString()) %>' />
                                                                                                                </ItemTemplate>
                                                                                                                <ItemStyle HorizontalAlign="Center" Width="100px" />
                                                                                                            </asp:TemplateField>
                                                                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,Tax(%) %>" HeaderStyle-Font-Size="12px">
                                                                                                                <ItemTemplate>
                                                                                                                    <asp:Label ID="lbltaxpview" runat="server" Font-Size="13px" ForeColor="#474646" Font-Names="Trebuchet MS"
                                                                                                                        Text='<%#Eval("TaxPercentage") %>' />
                                                                                                                </ItemTemplate>
                                                                                                                <ItemStyle HorizontalAlign="Center" Width="100px" />
                                                                                                            </asp:TemplateField>
                                                                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,Value %>" HeaderStyle-Font-Size="12px">
                                                                                                                <ItemTemplate>
                                                                                                                    <asp:Label ID="lbltaxvview" runat="server" Font-Size="13px" ForeColor="#474646" Font-Names="Trebuchet MS"
                                                                                                                        Text='<%# GetAmountDecimal(Eval("TaxValue").ToString()) %>' />
                                                                                                                </ItemTemplate>
                                                                                                                <ItemStyle HorizontalAlign="Center" Width="100px" />
                                                                                                            </asp:TemplateField>
                                                                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,Price After Tax%>" HeaderStyle-Font-Size="12px">
                                                                                                                <ItemTemplate>
                                                                                                                    <asp:Label ID="lbltaxafterpview" runat="server" Font-Size="13px" ForeColor="#474646"
                                                                                                                        Font-Names="Trebuchet MS" Text='<%#GetAmountDecimal(Eval("PriceAfterTax").ToString()) %>' />
                                                                                                                </ItemTemplate>
                                                                                                                <ItemStyle HorizontalAlign="Center" Width="120px" />
                                                                                                            </asp:TemplateField>
                                                                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,Discount(%)%>" HeaderStyle-Font-Size="12px">
                                                                                                                <ItemTemplate>
                                                                                                                    <asp:Label ID="lblDispview" runat="server" Font-Size="13px" ForeColor="#474646" Font-Names="Trebuchet MS"
                                                                                                                        Text='<%#Eval("DisPercentage") %>' />
                                                                                                                </ItemTemplate>
                                                                                                                <ItemStyle HorizontalAlign="Center" Width="80px" />
                                                                                                            </asp:TemplateField>
                                                                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,Value%>" HeaderStyle-Font-Size="12px">
                                                                                                                <ItemTemplate>
                                                                                                                    <asp:Label ID="lblDisvview" runat="server" Font-Size="13px" ForeColor="#474646" Font-Names="Trebuchet MS"
                                                                                                                        Text='<%#GetAmountDecimal(Eval("DiscountValue").ToString()) %>' />
                                                                                                                </ItemTemplate>
                                                                                                                <ItemStyle HorizontalAlign="Center" Width="100px" />
                                                                                                            </asp:TemplateField>
                                                                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,Total Price%>" HeaderStyle-Font-Size="12px">
                                                                                                                <ItemTemplate>
                                                                                                                    <asp:Label ID="lblAmountview" runat="server" Font-Size="13px" ForeColor="#474646"
                                                                                                                        Font-Names="Trebuchet MS" Text='<%#GetAmountDecimal(Eval("Amount").ToString()) %>' />
                                                                                                                </ItemTemplate>
                                                                                                                <ItemStyle HorizontalAlign="Center" Width="100px" />
                                                                                                            </asp:TemplateField>
                                                                                                        </Columns>
                                                                                                    </asp:GridView>
                                                                                                </td>
                                                                                            </tr>
                                                                                            <tr>
                                                                                                <td>
                                                                                                    <asp:HiddenField ID="hdngvSupplierId" runat="server" Value='<%#Eval("Supplier_Id") %>' />
                                                                                                    <asp:TextBox ID="txtTermsCondition" Enabled="false" Width="100%" runat="server" TextMode="MultiLine"
                                                                                                        Text='<%#Eval("TermsCondition") %>'></asp:TextBox>
                                                                                                </td>
                                                                                            </tr>
                                                                                        </table>
                                                                                    </ItemTemplate>
                                                                                </asp:TemplateField>
                                                                            </Columns>
                                                                            <FooterStyle BackColor="#CCCC99" ForeColor="Black" />
                                                                            <PagerStyle BackColor="White" ForeColor="Black" HorizontalAlign="Right" />
                                                                            <SelectedRowStyle BackColor="#CC3333" Font-Bold="True" />
                                                                        </asp:GridView>
                                                                    </div>
                                                                    <br />
                                                                </div>
                                                                <div class="col-md-12">
                                                                    <asp:Label ID="lblProductDescription" runat="server" Text="<%$ Resources:Attendance,Product Description %>" />
                                                                    &nbsp:&nbsp<asp:Label ID="txtProductDescription" Font-Bold="true" runat="server" Text='<%#Eval("ProductDescription") %>' />
                                                                    <br />
                                                                </div>
                                                            </ItemTemplate>
                                                        </asp:DataList>
                                                    </div>
                                                    <br />
                                                </div>
                                                <div class="col-md-6">
                                                    <asp:Label ID="Label9" Visible="false" runat="server" Text="<%$ Resources:Attendance,Total Amount %>" />
                                                    <asp:Label ID="lblTotalAmountView" Visible="false" Font-Bold="true"
                                                        runat="server" />
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
                            <asp:Button ID="BtnCancelView" runat="server" CssClass="btn btn-primary" Text="<%$ Resources:Attendance,Reset %>" Visible="false"
                                CausesValidation="False" OnClick="BtnCancelView_Click" />
                        </ContentTemplate>
                    </asp:UpdatePanel>
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
                            <%--<asp:Button ID="btnSaveGST" Visible="false" runat="server" CssClass="btn btn-primary" ValidationGroup="S_update"
                                Text="<%$ Resources:Attendance,Save %>" OnClick="btnSaveGST_Click" />

                            <asp:Button ID="btnCancelGST" runat="server" CausesValidation="False" CssClass="btn btn-primary"
                                Text="<%$ Resources:Attendance,Reset %>" Visible="false" />--%>
                            <asp:HiddenField ID="Hdn_Serial_No_Tax" runat="server" />
                            <asp:HiddenField ID="Hdn_Product_Id_Tax" runat="server" />
                            <asp:HiddenField ID="Hdn_unit_Price_Tax" runat="server" />
                            <asp:HiddenField ID="Hdn_Discount_Tax" runat="server" />
                            <asp:Button ID="Btn_Update_Tax" runat="server" CssClass="btn btn-primary"
                                Text="<%$ Resources:Attendance,Update %>" OnClick="Btn_Update_Tax_Click" />
                            <button id="btnClosePopup" type="button" class="btn btn-danger" data-dismiss="modal">Close</button>
                        </ContentTemplate>
                    </asp:UpdatePanel>
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

    <asp:UpdateProgress ID="UpdateProgress5" runat="server" AssociatedUpdatePanelID="update_Modal">
        <ProgressTemplate>
            <div class="modal_Progress">
                <div class="center_Progress">
                    <img alt="" src="../Images/ajax-loader2.gif" style="vertical-align: middle; background-color: White; border-radius: 10px;" />
                </div>
            </div>
        </ProgressTemplate>
    </asp:UpdateProgress>

    <asp:UpdateProgress ID="UpdateProgress8" runat="server" AssociatedUpdatePanelID="Update_Modal_GST">
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

    <asp:UpdateProgress ID="UpdateProgress6" runat="server" AssociatedUpdatePanelID="Update_Modal_Button">
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
    <asp:UpdateProgress ID="UpdateProgress7" runat="server" AssociatedUpdatePanelID="Update_Li">
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
    <asp:UpdateProgress ID="UpdateProgress4" runat="server" AssociatedUpdatePanelID="Update_Inquiry">
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
        function LI_New_Request_Active() {
            $("#Li_Inquiry").removeClass("active");
            $("#Inquiry").removeClass("active");
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
        function Li_Tab_Inquiry() {
            document.getElementById('<%= Btn_Inquiry.ClientID %>').click();
        }
        function Purchase_Quotation_Popup() {
            document.getElementById('<%= Btn_Purchase_Quotation.ClientID %>').click();
        }

     

        function DL_Discount_Hide() {
            $('.Div_Discount_P').toggleClass('hidden');
            $('.Div_Discount_V').toggleClass('hidden');
        }
        function Show_Modal_GST() {
            document.getElementById('<%= Btn_GST_Modal.ClientID %>').click();
        }
        function IbtnQC_Command(key) {
            window.open('../Purchase/CompareQuatation.aspx?RPQId=' + key + '&&C=F');
        }
        function lnkSupplierdetail_Command(contactId) {
            window.open('../Purchase/CustomerHistory.aspx?ContactId=' + contactId + '&&Page=PQ', 'window', 'width=1024, ');
        }
        function lnkStockInfo_Command(productId) {
            var CustomerName = "";
            try {
                CustomerName = document.getElementById('<%=lblsupplierName.ClientID%>').innerHTML;
            }
            catch (er) {
            }
            window.open('../Inventory/Magic_Stock_Analysis.aspx?ProductId=' + productId + '&&Type=PURCHASE&&Contact=' + CustomerName + '');
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
        function setScrollAndRow() {
            try {
                debugger;
               <%-- var rowIndex = $('#<%= hdfCurrentRow.ClientID %>').val();
                var parent = document.getElementById('<%= dlProductDetail.ClientID %>');
                var rowIndex = parseInt(rowIndex);
                parent.rows[rowIndex + 1].style.backgroundColor = "#A1DCF2";--%>
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

        function Modal_Open_FileUpload() {
            document.getElementById('<%= Btn_Modal_FileUpload.ClientID %>').click();
        }


    </script>
</asp:Content>
