<%@ Page Title="" Language="C#" MasterPageFile="~/ERPMaster.master" AutoEventWireup="true" CodeFile="Sales_Commission.aspx.cs" Inherits="Sales_Sales_Commission" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="Head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="HeaderContent" runat="Server">
    <h1>
        <i class="fas fa-award"></i>&nbsp;&nbsp;
        <asp:Label ID="lblHeader" runat="server" Text="<%$ Resources:Attendance,Sales Reward Setup%>"></asp:Label>
    </h1>
    <ol class="breadcrumb">
        <li><a href="#"><i class="fa fa-dashboard"></i>
            <asp:Label ID="Label6" runat="server" Text="<%$ Resources:Attendance,Inventory%>"></asp:Label></a></li>
        <li><a href="#"><i class="fa fa-dashboard"></i>
            <asp:Label ID="Label10" runat="server" Text="<%$ Resources:Attendance,Sales%>"></asp:Label></a></li>
        <li class="active">
            <asp:Label ID="Label5" runat="server" Text="<%$ Resources:Attendance,Sales Reward Setup%>"></asp:Label></li>
    </ol>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="Server">
    <asp:UpdatePanel ID="Update_Button" runat="server">
        <ContentTemplate>
            <asp:Button ID="Btn_Configuration" Style="display: none;" runat="server" OnClick="btnConfiguration_Click" Text="Configuration" />
            <asp:Button ID="Btn_Serial_No_Modal" Style="display: none;" runat="server" data-toggle="modal" data-target="#Serial_No_Modal" Text="Serial No Modal" />
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
                    <li id="Li_Configuration"><a href="#Configuration" onclick="Li_Tab_Configuration()" data-toggle="tab">
                        <i class="fas fa-wrench"></i>&nbsp;&nbsp;
                        <asp:Label ID="Label2" runat="server" Text="<%$ Resources:Attendance,Configuration %>"></asp:Label></a></li>
                    <li id="Li_Report"><a href="#Report" data-toggle="tab">
                        <i class="fas fa-file-signature"></i>&nbsp;&nbsp;
                        <asp:Label ID="Label3" runat="server" Text="<%$ Resources:Attendance,Report %>"></asp:Label></a></li>
                    <li id="Li_Commission_Payment"><a href="#Commission_Payment" data-toggle="tab">
                        <i class="fas fa-dollar-sign"></i>&nbsp;&nbsp;
                        <asp:Label ID="Label13" runat="server" Text="<%$ Resources:Attendance,Commission Payment %>"></asp:Label></a></li>
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
                                                    <asp:Label ID="Label46" runat="server" Text="Advance Search"></asp:Label></h3>
                                                &nbsp;&nbsp;|&nbsp;&nbsp;
				 <asp:Label ID="lblTotalRecords" runat="server" Text="<%$ Resources:Attendance,Total Records: 0 %>"></asp:Label>
                                                <asp:Label ID="lblSelectedRecord" runat="server" Visible="false"></asp:Label>

                                                <div class="box-tools pull-right">
                                                    <button type="button" class="btn btn-box-tool" data-widget="collapse">
                                                        <i id="I1" runat="server" class="fa fa-plus"></i>
                                                    </button>
                                                </div>
                                            </div>
                                            <div class="box-body">
                                                <div class="col-lg-2">
                                                    <asp:DropDownList ID="ddlPosted" runat="server" CssClass="form-control"
                                                        AutoPostBack="true" OnSelectedIndexChanged="ddlPosted_SelectedIndexChanged">
                                                        <asp:ListItem Text="<%$ Resources:Attendance,Posted %>" Value="True"></asp:ListItem>
                                                        <asp:ListItem Text="<%$ Resources:Attendance,UnPosted %>" Value="False" Selected="True"></asp:ListItem>
                                                        <asp:ListItem Text="<%$ Resources:Attendance,All %>" Value="0"></asp:ListItem>
                                                    </asp:DropDownList>
                                                </div>
                                                <div class="col-lg-2">
                                                    <asp:DropDownList ID="ddlFieldName" runat="server" CssClass="form-control"
                                                        AutoPostBack="true" OnSelectedIndexChanged="ddlFieldName_SelectedIndexChanged">
                                                        <asp:ListItem Selected="True" Text="<%$ Resources:Attendance,Voucher No %>" Value="Voucher_No"></asp:ListItem>
                                                        <asp:ListItem Text="<%$ Resources:Attendance,Voucher Date %>" Value="Voucher_Date"></asp:ListItem>
                                                        <asp:ListItem Text="<%$ Resources:Attendance,Type%>" Value="Field1"></asp:ListItem>
                                                        <asp:ListItem Text="<%$ Resources:Attendance,Employee Name%>" Value="Emp_Name"></asp:ListItem>
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
                                <div class="box box-warning box-solid" <%= gvSalesCommission.Rows.Count>0?"style='display:block'":"style='display:none'"%>>
                                    <div class="box-body">
                                        <div class="row">
                                            <div class="col-md-12">
                                                <div class="flow">
                                                    <asp:HiddenField ID="HDFSort" runat="server" />
                                                    <asp:HiddenField ID="editid" runat="server" />
                                                    <asp:GridView CssClass="table-striped table-bordered table table-hover" ID="gvSalesCommission" PageSize='<%# int.Parse(Session["GridSize"].ToString()) %>'
                                                        runat="server" AutoGenerateColumns="False" Width="100%" AllowPaging="True" AllowSorting="True"
                                                        OnPageIndexChanging="gvSalesCommission_PageIndexChanging" OnSorting="gvSalesCommission_OnSorting">
                                                        <Columns>

                                                            <asp:TemplateField HeaderText="Action">
                                                                <ItemTemplate>
                                                                    <div class="dropdown" style="position: absolute;">
                                                                        <button class="btn btn-default dropdown-toggle"  type="button" data-toggle="dropdown">
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
                                                                                <asp:LinkButton ID="IbtnDelete" runat="server" CausesValidation="False" CommandArgument='<%# Eval("Trans_Id") %>' OnCommand="IbtnDelete_Command"><i class="fa fa-trash"></i>Delete</asp:LinkButton>
                                                                                <cc1:ConfirmButtonExtender ID="confirm2" runat="server" ConfirmText="<%$ Resources:Attendance,Are you sure you want to delete the record?%>" TargetControlID="IbtnDelete"></cc1:ConfirmButtonExtender>
                                                                            </li>

                                                                        </ul>
                                                                    </div>

                                                                </ItemTemplate>
                                                            </asp:TemplateField>

                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,Voucher No%>" SortExpression="Voucher_No">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblVoucherNo" runat="server" Text='<%# Eval("Voucher_No") %>'></asp:Label>
                                                                </ItemTemplate>
                                                                <ItemStyle HorizontalAlign="Center" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,Voucher Date %>" SortExpression="Voucher_Date">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblVoucherDate" runat="server" Text='<%# GeDate(Eval("Voucher_Date").ToString()) %>'></asp:Label>
                                                                </ItemTemplate>
                                                                <ItemStyle HorizontalAlign="Center" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,From Date %>" SortExpression="From_Date">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblFromdate" runat="server" Text='<%# GeDate(Eval("From_Date").ToString()) %>'></asp:Label>
                                                                </ItemTemplate>
                                                                <ItemStyle HorizontalAlign="Center" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,To Date %>" SortExpression="To_Date">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblTodate" runat="server" Text='<%# GeDate(Eval("To_Date").ToString()) %>'></asp:Label>
                                                                </ItemTemplate>
                                                                <ItemStyle HorizontalAlign="Center" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,Type%>" SortExpression="Field1">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblType" runat="server" Text='<%# Eval("Field1") %>'></asp:Label>
                                                                </ItemTemplate>
                                                                <ItemStyle />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,Employee Name%>" SortExpression="Emp_Name">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblEmpName" runat="server" Text='<%# Eval("Emp_Name") %>'></asp:Label>
                                                                    <asp:Label ID="lblPostStatus" runat="server" Text='<%# Eval("Post") %>' Visible="false"></asp:Label>
                                                                </ItemTemplate>
                                                                <ItemStyle />
                                                            </asp:TemplateField>

                                                            <asp:TemplateField HeaderText="Total Commission" SortExpression="Total_Comission_Amount">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lbltotalcommission" runat="server" Text='<%#Common.GetAmountDecimal(Eval("Total_Comission_Amount").ToString(),Session["DBConnection"].ToString(),Session["LocCurrencyId"].ToString()) %>'></asp:Label>
                                                                </ItemTemplate>
                                                                <ItemStyle HorizontalAlign="Right" />
                                                            </asp:TemplateField>

                                                            <asp:TemplateField HeaderText="Balance Commission" SortExpression="Field2">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lbltotalunpaidcommission" runat="server" Text='<%#Common.GetAmountDecimal(Eval("Field2").ToString(),Session["DBConnection"].ToString(),Session["LocCurrencyId"].ToString()) %>'></asp:Label>
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
                                                        <asp:Label ID="lblplanId" runat="server" Text="<%$ Resources:Attendance,Voucher No %>"></asp:Label>
                                                        <a style="color: Red">*</a>
                                                        <asp:RequiredFieldValidator EnableClientScript="true" Style="float: right;" runat="server" ID="RequiredFieldValidator3" ValidationGroup="Save"
                                                            Display="Dynamic" SetFocusOnError="true" ControlToValidate="txtVoucherNo" ErrorMessage="<%$ Resources:Attendance,Enter Voucher No%>"></asp:RequiredFieldValidator>

                                                        <asp:TextBox ID="txtVoucherNo" TabIndex="104" runat="server" CssClass="form-control" />
                                                        <br />
                                                    </div>
                                                    <div class="col-md-6">
                                                        <asp:Label ID="lblPlanName" runat="server" Text="<%$ Resources:Attendance,Voucher Date %>"></asp:Label>
                                                        <a style="color: Red">*</a>
                                                        <asp:RequiredFieldValidator EnableClientScript="true" Style="float: right;" runat="server" ID="RequiredFieldValidator1" ValidationGroup="Save"
                                                            Display="Dynamic" SetFocusOnError="true" ControlToValidate="txtVoucherDate" ErrorMessage="<%$ Resources:Attendance,Enter Voucher Date%>"></asp:RequiredFieldValidator>

                                                        <asp:TextBox ID="txtVoucherDate" TabIndex="105" runat="server" CssClass="form-control" />
                                                        <cc1:CalendarExtender OnClientShown="showCalendar" ID="Calender" runat="server" TargetControlID="txtVoucherDate" />
                                                        <br />
                                                    </div>
                                                    <div class="col-md-6">
                                                        <asp:Label ID="Label4" runat="server" Text="<%$ Resources:Attendance,From Date %>"></asp:Label>
                                                        <a style="color: Red">*</a>
                                                        <asp:RequiredFieldValidator EnableClientScript="true" Style="float: right;" runat="server" ID="RequiredFieldValidator2" ValidationGroup="Save"
                                                            Display="Dynamic" SetFocusOnError="true" ControlToValidate="txtfromdate" ErrorMessage="<%$ Resources:Attendance,Enter From Date%>"></asp:RequiredFieldValidator>

                                                        <asp:TextBox ID="txtfromdate" TabIndex="104" runat="server" CssClass="form-control"
                                                            AutoPostBack="true" OnTextChanged="txtfromdate_OnTextChanged" />
                                                        <cc1:CalendarExtender OnClientShown="showCalendar" ID="CalendarFrom" runat="server" TargetControlID="txtfromdate" />
                                                        <br />
                                                    </div>
                                                    <div class="col-md-6">
                                                        <asp:Label ID="Label7" runat="server" Text="<%$ Resources:Attendance,To Date %>"></asp:Label>
                                                        <a style="color: Red">*</a>
                                                        <asp:RequiredFieldValidator EnableClientScript="true" Style="float: right;" runat="server" ID="RequiredFieldValidator4" ValidationGroup="Save"
                                                            Display="Dynamic" SetFocusOnError="true" ControlToValidate="txttodate" ErrorMessage="<%$ Resources:Attendance,Enter To Date%>"></asp:RequiredFieldValidator>

                                                        <asp:TextBox ID="txttodate" TabIndex="105" runat="server" CssClass="form-control" AutoPostBack="true"
                                                            OnTextChanged="txttodate_OnTextChanged" />
                                                        <cc1:CalendarExtender OnClientShown="showCalendar" ID="CalendarTo" runat="server" TargetControlID="txttodate" />
                                                        <br />
                                                    </div>
                                                    <div class="col-md-6">
                                                        <asp:Label ID="Label16" runat="server" Text="<%$ Resources:Attendance,Type %>"></asp:Label>
                                                        <asp:DropDownList ID="ddlType" runat="server" CssClass="form-control"
                                                            AutoPostBack="true" OnSelectedIndexChanged="ddlType_OnSelectedIndexChanged">
                                                            <asp:ListItem Text="Sales" Value="Sales"></asp:ListItem>
                                                            <asp:ListItem Text="Technical" Value="Technical"></asp:ListItem>
                                                            <asp:ListItem Text="Agent" Value="Agent"></asp:ListItem>
                                                            <asp:ListItem Text="Developer" Value="Developer"></asp:ListItem>
                                                        </asp:DropDownList>
                                                        <br />
                                                    </div>

                                                    <div class="col-md-12">
                                                    </div>
                                                    <div class="col-md-6" runat="server" id="Div_Sales_Person">
                                                        <asp:Label ID="lblSalesPerson" runat="server" Text="<%$ Resources:Attendance,Employee Name%>" />
                                                        <a style="color: Red">*</a>
                                                        <asp:RequiredFieldValidator EnableClientScript="true" Style="float: right;" runat="server" ID="RequiredFieldValidator5" ValidationGroup="Save"
                                                            Display="Dynamic" SetFocusOnError="true" ControlToValidate="txtSalesPerson" ErrorMessage="<%$ Resources:Attendance,Enter Employee Name%>"></asp:RequiredFieldValidator>

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
                                                        <br />
                                                        <asp:Button ID="btngetCommissionRecord" runat="server" Text="Get Invoice Detail"
                                                            CssClass="btn btn-primary" OnClick="btngetCommissionRecord_Click" />
                                                        <br />
                                                    </div>
                                                    <div class="col-md-12">
                                                        <asp:Label ID="Label8" runat="server" Text="<%$ Resources:Attendance,Remark%>" />
                                                        <asp:TextBox ID="txtremarks" runat="server" CssClass="form-control" TextMode="MultiLine"></asp:TextBox>
                                                        <br />
                                                    </div>
                                                    <div class="col-md-6">
                                                        <asp:Label ID="lblinvoice" runat="server" Text="<%$ Resources:Attendance,Sales Invoice %>"></asp:Label>
                                                        <asp:TextBox ID="txtsalesinvoice" runat="server" CssClass="form-control" OnTextChanged="txtsalesinvoice_OnTextChanged"
                                                            AutoPostBack="true" BackColor="#eeeeee"></asp:TextBox>
                                                        <cc1:AutoCompleteExtender ID="AutoCompleteExtender3" runat="server" DelimiterCharacters=""
                                                            Enabled="True" ServiceMethod="GetCompletionListInvoiceNo" ServicePath="" CompletionInterval="100"
                                                            MinimumPrefixLength="1" CompletionSetCount="1" TargetControlID="txtsalesinvoice"
                                                            UseContextKey="True" CompletionListCssClass="autoCompleteList" CompletionListItemCssClass="autoCompleteListItem" CompletionListHighlightedItemCssClass="autoCompleteSelectedListItem" OnClientShown="resetPosition">
                                                        </cc1:AutoCompleteExtender>
                                                        <br />
                                                    </div>
                                                    <div class="col-md-6">
                                                        <asp:Label ID="Label9" runat="server" Text="<%$ Resources:Attendance,Invoice Date %>"></asp:Label>
                                                        <asp:TextBox ID="txtInvoicedate" runat="server" CssClass="form-control" Enabled="false"></asp:TextBox>
                                                        <br />
                                                    </div>
                                                    <div class="col-md-12">
                                                        <asp:Label ID="Label11" runat="server" Text="<%$ Resources:Attendance,Customer Name %>"></asp:Label>
                                                        <asp:TextBox ID="txtcutomerName" runat="server" CssClass="form-control"
                                                            Enabled="false"></asp:TextBox>
                                                        <br />
                                                    </div>
                                                    <div class="col-md-12">
                                                        <asp:Label ID="Label12" runat="server" Text="<%$ Resources:Attendance,Product Name %>"></asp:Label>
                                                        <asp:DropDownList ID="ddlProduct" runat="server" CssClass="form-control"
                                                            OnSelectedIndexChanged="ddlProduct_OnSelectedIndexChanged" AutoPostBack="true">
                                                        </asp:DropDownList>
                                                        <asp:HiddenField ID="hdnInvoiceId" runat="server" />
                                                        <br />
                                                    </div>
                                                    <div class="col-md-6">
                                                        <asp:Label ID="Label14" runat="server" Text="<%$ Resources:Attendance,Foreign Amount %>"></asp:Label>
                                                        <div class="input-group">
                                                            <asp:TextBox ID="txtAmount" runat="server" CssClass="form-control" Enabled="false"></asp:TextBox>
                                                            <div class="input-group-btn">
                                                                <asp:DropDownList ID="ddlCurrency" Width="120px" runat="server" CssClass="form-control" Enabled="false" />
                                                            </div>
                                                        </div>
                                                        <br />
                                                    </div>
                                                    <div class="col-md-6">
                                                        <asp:Label ID="lblLocalamount" runat="server" Text="Local Amount"></asp:Label>
                                                        <div class="input-group">
                                                            <asp:TextBox ID="txtLocalAmount" runat="server" CssClass="form-control" Enabled="false"></asp:TextBox>
                                                            <div class="input-group-addon">
                                                                <asp:Label ID="lblLocalCurrencyCode" runat="server"></asp:Label>
                                                            </div>
                                                        </div>
                                                        <br />
                                                    </div>
                                                    <div class="col-md-6">
                                                        <asp:Label ID="Label15" runat="server" Text="Commission(%)"></asp:Label>
                                                        <asp:TextBox ID="txtcommpercentage" runat="server" CssClass="form-control" OnTextChanged="txtcommpercentage_OnTextChanged"
                                                            AutoPostBack="true"></asp:TextBox>
                                                        <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender18" runat="server" Enabled="True"
                                                            TargetControlID="txtcommpercentage" ValidChars="1,2,3,4,5,6,7,8,9,0,.">
                                                        </cc1:FilteredTextBoxExtender>
                                                        <br />
                                                    </div>
                                                    <div class="col-md-6">
                                                        <asp:Label ID="Label17" runat="server" Text="Total Commission"></asp:Label>
                                                        <asp:TextBox ID="txttotalCommission" runat="server" CssClass="form-control"
                                                            OnTextChanged="txttotalCommission_OnTextChanged" AutoPostBack="true"></asp:TextBox>
                                                        <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" runat="server" Enabled="True"
                                                            TargetControlID="txttotalCommission" ValidChars="1,2,3,4,5,6,7,8,9,0,.">
                                                        </cc1:FilteredTextBoxExtender>
                                                        <asp:HiddenField ID="hdnReturnFlag" runat="server" />
                                                        <br />
                                                    </div>
                                                    <div id="pnldetail" runat="server" class="col-md-12" visible="false" style="text-align: center">
                                                        <asp:Button ID="IbtnAddCommission" runat="server" Text="<%$ Resources:Attendance,Add %>" OnClick="IbtnAddCommission_Click" CssClass="btn btn-primary" />
                                                        <br />
                                                    </div>
                                                    <div class="col-md-12">
                                                        <div style="overflow: auto; max-height: 500px;">
                                                            <asp:GridView CssClass="table-striped table-bordered table table-hover" ID="gvDetail" PageSize='<%# int.Parse(Session["GridSize"].ToString()) %>' runat="server"
                                                                AutoGenerateColumns="False" Width="100%" AllowPaging="false" AllowSorting="false">
                                                                <Columns>
                                                                    <asp:TemplateField HeaderText="<%$ Resources:Attendance,Delete %>" Visible="false">
                                                                        <ItemTemplate>
                                                                            <asp:LinkButton ID="IbtnDelete" runat="server" CausesValidation="False" CommandArgument='<%# Eval("Trans_Id") %>'  OnCommand="IbtnDeleteComm_Command" ToolTip="<%$ Resources:Attendance,Delete %>"><i class="fa fa-trash"></i></asp:LinkButton>
                                                                        </ItemTemplate>
                                                                        <ItemStyle HorizontalAlign="Center" />
                                                                        <ItemStyle />
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="<%$ Resources:Attendance,Invoice No%>">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblTransId" Width="120px" runat="server" Text='<%# Eval("Trans_Id") %>' Visible="false"></asp:Label>
                                                                            <asp:Label ID="lblInvoiceNo" Width="120px" runat="server" Text='<%# Eval("Invoice_No") %>' Visible="false"></asp:Label>
                                                                            <asp:Label ID="lblInvoiceId" runat="server" Width="120px" Text='<%# Eval("Invoice_Id") %>' Visible="false"></asp:Label>
                                                                            <asp:LinkButton ID="lblgvSInvNo" runat="server" Width="120px" Text='<%#Eval("Invoice_No") %>' CommandArgument='<%# Eval("Invoice_Id") %>' ForeColor="Blue" Font-Underline="true" OnCommand="lblgvSInvNo_Command" ToolTip="View Invoice Detail" />
                                                                            <asp:Label ID="lblProjectId" runat="server" Width="120px" Text='<%# Eval("projectid") %>' Visible="false"></asp:Label>
                                                                        </ItemTemplate>
                                                                        <ItemStyle />
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="<%$ Resources:Attendance,Invoice Date %>">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblInvoiceDate" Width="120px" runat="server" Text='<%# GeDate(Eval("Invoice_Date").ToString()) %>'></asp:Label>
                                                                        </ItemTemplate>
                                                                        <ItemStyle />
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="<%$ Resources:Attendance,Product Id %>">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblproductId" Width="120px" runat="server" Text='<%# Eval("Product_Id") %>' Visible="false"></asp:Label>
                                                                            <asp:Label ID="lblProuctCode" Width="120px" runat="server" Text='<%# Eval("ProductCode") %>'></asp:Label>
                                                                        </ItemTemplate>
                                                                        <ItemStyle />
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="<%$ Resources:Attendance,Product Name %>" Visible="false">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblProuctName" Width="120px" runat="server" Text='<%# Eval("EProductName") %>'></asp:Label>
                                                                        </ItemTemplate>
                                                                        <ItemStyle />
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="<%$ Resources:Attendance,Customer Name%>">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblCustomerId" Width="120px" runat="server" Text='<%# Eval("Customer_Id") %>' Visible="false"></asp:Label>
                                                                            <asp:Label ID="lblcustomerName" Width="120px" runat="server" Text='<%# Eval("Name") %>'></asp:Label>
                                                                        </ItemTemplate>
                                                                        <ItemStyle />
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="<%$ Resources:Attendance,Sales Person%>">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblSalespersonName" Width="120px" runat="server" Text='<%# GetSalesPerson(Eval("Invoice_Id").ToString()) %>'></asp:Label>
                                                                        </ItemTemplate>
                                                                        <ItemStyle />
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Foreign Amount">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblgvCurrencyId" Width="120px" runat="server" Text='<%# Eval("CurrencyId") %>' Visible="false"></asp:Label>
                                                                            <asp:Label ID="lblforeignAmt1" runat="server" Width="120px" Text='<%# GetCurrencySymbol(Eval("Amount").ToString(),Eval("CurrencyId").ToString()) %>'></asp:Label>
                                                                            <asp:Label ID="lblforeignAmt" Width="120px" runat="server" Text='<%# SetDecimal(Eval("Amount").ToString()) %>'
                                                                                Visible="false"></asp:Label>
                                                                        </ItemTemplate>
                                                                        <ItemStyle HorizontalAlign="Right" />
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Local Amount">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblAmt" Width="120px" runat="server" Text='<%# SetDecimal(Eval("LocalAmount").ToString()) %>'></asp:Label>
                                                                        </ItemTemplate>
                                                                        <ItemStyle HorizontalAlign="Right" />
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Comm %">
                                                                        <ItemTemplate>
                                                                            <asp:TextBox ID="txtper" Width="120px" runat="server" CssClass="form-control" Text='<%# SetDecimal(Eval("Comission_Percentage").ToString()) %>'
                                                                                OnTextChanged="txtper_OnTextChanged" AutoPostBack="true"></asp:TextBox>
                                                                            <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtendertxtper" runat="server" Enabled="True"
                                                                                TargetControlID="txtper" ValidChars="1,2,3,4,5,6,7,8,9,0,.">
                                                                            </cc1:FilteredTextBoxExtender>
                                                                            <asp:Label ID="lblPer" Width="120px" runat="server" Text='<%# Eval("Comission_Percentage") %>'
                                                                                Visible="false"></asp:Label>
                                                                        </ItemTemplate>
                                                                        <ItemStyle HorizontalAlign="Right" />
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Commission Person">
                                                                        <ItemTemplate>
                                                                            <table width="100%">
                                                                                <tr>
                                                                                    <td width="95%">
                                                                                        <asp:GridView CssClass="table-striped table-bordered table table-hover" ID="gvEmp" Width="100%" runat="server" AutoGenerateColumns="false">
                                                                                            <Columns>
                                                                                                <asp:TemplateField HeaderText="<%$ Resources:Attendance,Delete %>" HeaderStyle-HorizontalAlign="Center">
                                                                                                    <ItemTemplate>
                                                                                                        <asp:LinkButton ID="IbtnDelete" runat="server" CausesValidation="False" CommandArgument='<%# Eval("Trans_Id") %>' OnCommand="IbtnDeleteEmp_Command" ><i class="fa fa-trash"></i></asp:LinkButton>
                                                                                                        <asp:HiddenField ID="hdnGvTransId" runat="server" Value='<%# Eval("Trans_Id") %>' />
                                                                                                    </ItemTemplate>
                                                                                                    <ItemStyle HorizontalAlign="Center" />
                                                                                                </asp:TemplateField>
                                                                                                <asp:TemplateField HeaderText="Name" SortExpression="Emp_Name">
                                                                                                    <ItemTemplate>
                                                                                                        <asp:Label ID="lblempame" Width="120px" runat="server" Text='<%# GetEmployeeName(Eval("Commission_Person").ToString()) %>'></asp:Label>
                                                                                                        <asp:HiddenField ID="hdnGvEmpId" runat="server" Value='<%# Eval("Commission_Person") %>' />
                                                                                                    </ItemTemplate>
                                                                                                    <ItemStyle />
                                                                                                </asp:TemplateField>
                                                                                                <asp:TemplateField HeaderText="%" SortExpression="Commission_Percentage">
                                                                                                    <ItemTemplate>
                                                                                                        <asp:Label ID="lblcommissionper" Width="120px" runat="server" Text='<%# SetDecimal(Eval("Commission_Percentage").ToString()) %>'></asp:Label>
                                                                                                    </ItemTemplate>
                                                                                                    <ItemStyle />
                                                                                                </asp:TemplateField>
                                                                                                <asp:TemplateField HeaderText="Is Paid">
                                                                                                    <ItemTemplate>
                                                                                                        <asp:Label ID="lblispaid" Width="120px" runat="server" Text='<%# Eval("Is_Paid") %>'></asp:Label>
                                                                                                    </ItemTemplate>
                                                                                                    <ItemStyle HorizontalAlign="Center" />
                                                                                                </asp:TemplateField>
                                                                                                <asp:TemplateField HeaderText="Paid Date">
                                                                                                    <ItemTemplate>
                                                                                                        <asp:Label ID="lblpaiddate" Width="120px" runat="server" Text='<%# GeDate(Eval("Paid_Date").ToString()) %>'></asp:Label>
                                                                                                    </ItemTemplate>
                                                                                                    <ItemStyle HorizontalAlign="Center" />
                                                                                                </asp:TemplateField>
                                                                                            </Columns>


                                                                                            <PagerStyle CssClass="pagination-ys" />

                                                                                        </asp:GridView>
                                                                                    </td>
                                                                                    <td>
                                                                                        <asp:LinkButton ID="lnkAddEmp" Width="60px" runat="server" Text="<%$ Resources:Attendance,Add %>"
                                                                                            OnClick="lnkAddEmp_Click" ForeColor="Blue" Font-Underline="true"></asp:LinkButton>
                                                                                    </td>
                                                                                </tr>
                                                                            </table>
                                                                        </ItemTemplate>
                                                                        <ItemStyle />
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Commission Amount">
                                                                        <ItemTemplate>
                                                                            <asp:TextBox ID="lblcommamt" Width="120px" runat="server" CssClass="form-control" Text='<%# SetDecimal(Eval("Comission_Amount").ToString()) %>'
                                                                                OnTextChanged="lblcommamt_OnTextChanged" AutoPostBack="true"></asp:TextBox>
                                                                            <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtendertxtperlblcommamt" runat="server"
                                                                                Enabled="True" TargetControlID="lblcommamt" ValidChars="1,2,3,4,5,6,7,8,9,0,.">
                                                                            </cc1:FilteredTextBoxExtender>
                                                                            <%--   <asp:Label ID="lblcommamt" runat="server" Text='<%# SetDecimal(Eval("Comission_Amount").ToString()) %>'></asp:Label>--%>
                                                                        </ItemTemplate>
                                                                        <ItemStyle HorizontalAlign="Right" />
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Is Received">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblisreceived" Width="120px" runat="server" Text='<%# Eval("Is_Receive") %>'></asp:Label>
                                                                        </ItemTemplate>
                                                                        <ItemStyle HorizontalAlign="Center" />
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Is Return">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblisreturn" Width="120px" runat="server" Text='<%# Eval("Is_Return") %>'></asp:Label>
                                                                        </ItemTemplate>
                                                                        <ItemStyle HorizontalAlign="Center" />
                                                                    </asp:TemplateField>
                                                                </Columns>


                                                                <PagerStyle CssClass="pagination-ys" />

                                                            </asp:GridView>
                                                        </div>
                                                        <br />
                                                        <br />
                                                        <div style="overflow: auto; max-height: 500px;">
                                                            <asp:GridView CssClass="table-striped table-bordered table table-hover" ID="gvEmployee" PageSize='<%# int.Parse(Session["GridSize"].ToString()) %>' runat="server"
                                                                AutoGenerateColumns="False" Width="100%" AllowPaging="false" AllowSorting="false">
                                                                <Columns>
                                                                    <asp:TemplateField>
                                                                        <ItemTemplate>
                                                                            <asp:CheckBox ID="chkgvSelect" runat="server" Checked='<%# Eval("Is_Paid") %>' />
                                                                        </ItemTemplate>
                                                                        <ItemStyle HorizontalAlign="Center" />
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField FooterText="Print" Visible="false">
                                                                        <ItemTemplate>
                                                                            <asp:LinkButton ID="IbtnPrint" runat="server" CausesValidation="False" CommandArgument='<%# Eval("EmpId") %>' OnCommand="IbtnPrint_Command" ToolTip="<%$ Resources:Attendance,Print %>"><i class="fa fa-print"></i></asp:LinkButton>
                                                                        </ItemTemplate>
                                                                        <ItemStyle HorizontalAlign="Center" />
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="<%$ Resources:Attendance,Name %>">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblProuctCode" runat="server" Text='<%# GetEmployeeName(Eval("EmpId").ToString()) %>'></asp:Label>
                                                                            <asp:Label ID="lblEmpId" runat="server" Text='<%# Eval("EmpId") %>' Visible="false"></asp:Label>
                                                                        </ItemTemplate>
                                                                        <ItemStyle />
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Total Commission">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblcom" runat="server" Text='<%# Eval("TotalCommission") %>'></asp:Label>
                                                                        </ItemTemplate>
                                                                        <ItemStyle HorizontalAlign="Right" />
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Total Return">
                                                                        <ItemTemplate>
                                                                            <asp:LinkButton ID="lblreturncom" runat="server" Text='<%# Eval("Returncommission") %>'
                                                                                ForeColor="Blue" OnCommand="IbtnPrint_Command" CommandArgument='<%# Eval("EmpId") %>'
                                                                                ToolTip="Return Detail"></asp:LinkButton>
                                                                        </ItemTemplate>
                                                                        <ItemStyle HorizontalAlign="Right" />
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Adjusted Return">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lbladjustedReturn" runat="server" Text='<%# Eval("AdjustedReturn") %>'></asp:Label>
                                                                        </ItemTemplate>
                                                                        <ItemStyle HorizontalAlign="Right" />
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Actual Commission">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblactualcom" runat="server" Text='<%# Eval("Remaincommission") %>'></asp:Label>
                                                                        </ItemTemplate>
                                                                        <ItemStyle HorizontalAlign="Right" />
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Total Received">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblRec" runat="server" Text='<%# Eval("ReceivedAmt") %>'></asp:Label>
                                                                        </ItemTemplate>
                                                                        <ItemStyle HorizontalAlign="Right" />
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Total Paid">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblPaid" runat="server" Text='<%# Eval("PaidAmt") %>'></asp:Label>
                                                                        </ItemTemplate>
                                                                        <ItemStyle HorizontalAlign="Right" />
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Total Remain">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblRemain" runat="server" Text='<%# Eval("RemainAmt") %>'></asp:Label>
                                                                        </ItemTemplate>
                                                                        <ItemStyle HorizontalAlign="Right" />
                                                                    </asp:TemplateField>
                                                                </Columns>


                                                                <PagerStyle CssClass="pagination-ys" />

                                                            </asp:GridView>
                                                        </div>
                                                        <br />
                                                        <br />
                                                        <div style="overflow: auto; max-height: 500px;">
                                                            <asp:GridView CssClass="table-striped table-bordered table table-hover" ID="gvPaymentHistory" PageSize='<%# int.Parse(Session["GridSize"].ToString()) %>'
                                                                runat="server" AutoGenerateColumns="False" Width="100%" AllowPaging="false" AllowSorting="false">
                                                                <Columns>
                                                                    <asp:TemplateField FooterText="Print" Visible="false">
                                                                        <ItemTemplate>
                                                                            <asp:ImageButton ID="IbtnPrint" runat="server" CausesValidation="False" CommandArgument='<%# Eval("Commission_Person") %>'
                                                                                ImageUrl="~/Images/print.png" OnCommand="IbtnPrintPaymentHistory_Command" CommandName='<%# GeDate(Eval("Paid_Date").ToString()) %>'
                                                                                ToolTip="<%$ Resources:Attendance,Print %>" Width="16px" />
                                                                        </ItemTemplate>
                                                                        <ItemStyle HorizontalAlign="Center" />
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="<%$ Resources:Attendance,Name %>">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblsalesperson" runat="server" Text='<%# GetEmployeeName(Eval("Commission_Person").ToString()) %>'></asp:Label>
                                                                            <asp:Label ID="lblEmpId" runat="server" Text='<%# Eval("Commission_Person") %>' Visible="false"></asp:Label>
                                                                        </ItemTemplate>
                                                                        <ItemStyle HorizontalAlign="Center" />
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Paid date">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblPaiddate" runat="server" Text='<%# GeDate(Eval("Paid_Date").ToString()) %>'></asp:Label>
                                                                        </ItemTemplate>
                                                                        <ItemStyle HorizontalAlign="Center" />
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Paid Amount">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblPaid" runat="server" Text='<%# SetDecimal(Eval("PaidAmount").ToString()) %>'></asp:Label>
                                                                        </ItemTemplate>
                                                                        <ItemStyle HorizontalAlign="Right" />
                                                                    </asp:TemplateField>
                                                                </Columns>


                                                                <PagerStyle CssClass="pagination-ys" />

                                                            </asp:GridView>
                                                        </div>
                                                        <br />
                                                    </div>
                                                    <div class="col-md-6">
                                                        <asp:Label ID="Label18" runat="server" Text="Total Sales Amount"></asp:Label>
                                                        <asp:TextBox ID="txttotalsalesAmount" runat="server" CssClass="form-control" Enabled="false"></asp:TextBox>
                                                        <br />
                                                    </div>
                                                    <div class="col-md-6">
                                                        <asp:Label ID="Label19" runat="server" Text="Total Commission"></asp:Label>
                                                        <asp:TextBox ID="txtNetCommission" runat="server" CssClass="form-control" Enabled="false"></asp:TextBox>
                                                        <br />
                                                    </div>
                                                    <div class="col-md-6">
                                                        <asp:Label ID="Label20" runat="server" Text="Total Return Commission"></asp:Label>
                                                        <asp:TextBox ID="txttotalreturmCommission" runat="server" CssClass="form-control" Enabled="false"></asp:TextBox>
                                                        <br />
                                                    </div>
                                                    <div class="col-md-6">
                                                        <asp:Label ID="Label21" runat="server" Text="Total Actual Commission"></asp:Label>
                                                        <asp:TextBox ID="txtTotalActualCommission" runat="server" CssClass="form-control" Enabled="false"></asp:TextBox>
                                                        <br />
                                                    </div>
                                                    <div class="col-md-6">
                                                        <asp:Label ID="Label22" runat="server" Text="Total Received"></asp:Label>
                                                        <asp:TextBox ID="txttotalReceivedAmount" runat="server" CssClass="form-control" Enabled="false"></asp:TextBox>
                                                        <br />
                                                    </div>
                                                    <div class="col-md-6">
                                                        <asp:Label ID="Label23" runat="server" Text="Total Paid Amount"></asp:Label>
                                                        <asp:TextBox ID="txttotalPaidAmount" runat="server" CssClass="form-control" Enabled="false"></asp:TextBox>
                                                        <br />
                                                    </div>
                                                    <div class="col-md-6">
                                                        <asp:Label ID="Label24" runat="server" Text="Total Remain Amount"></asp:Label>
                                                        <asp:TextBox ID="txttotalRemainAmount" runat="server" CssClass="form-control" Enabled="false"></asp:TextBox>
                                                        <br />
                                                    </div>
                                                    <div class="col-md-6">
                                                        <asp:Label ID="Label25" runat="server" Text="Total Unpaid Balance"></asp:Label>
                                                        <asp:TextBox ID="txtUnpaidbalance" runat="server" CssClass="form-control" Enabled="false"></asp:TextBox>
                                                        <br />
                                                    </div>
                                                    <div id="trFinance" runat="server" visible="false">
                                                        <div class="col-md-6">
                                                            <asp:Label ID="lblforDebitAccount" runat="server" Text="<%$ Resources:Attendance,For Debit Account %>" />
                                                            <asp:TextBox ID="txtDebitAccount" runat="server" CssClass="form-control" BackColor="#eeeeee"
                                                                AutoPostBack="true" OnTextChanged="txtcmnAccount_textChnaged"></asp:TextBox>
                                                            <cc1:AutoCompleteExtender ID="AutoCompleteExtender1" runat="server" DelimiterCharacters=""
                                                                Enabled="True" ServiceMethod="GetCompletionListAccountName" ServicePath="" CompletionInterval="100"
                                                                MinimumPrefixLength="1" CompletionSetCount="1" TargetControlID="txtDebitAccount"
                                                                UseContextKey="True" CompletionListCssClass="autoCompleteList" CompletionListItemCssClass="autoCompleteListItem" CompletionListHighlightedItemCssClass="autoCompleteSelectedListItem" OnClientShown="resetPosition">
                                                            </cc1:AutoCompleteExtender>
                                                            <br />
                                                        </div>
                                                        <div class="col-md-6">
                                                            <asp:Label ID="lblforCredit" runat="server" Text="<%$ Resources:Attendance,For Credit Account %>" />
                                                            <asp:TextBox ID="txtCreditAccount" runat="server" CssClass="form-control" BackColor="#eeeeee"
                                                                AutoPostBack="true" OnTextChanged="txtcmnAccount_textChnaged"></asp:TextBox>
                                                            <cc1:AutoCompleteExtender ID="AutoCompleteExtender2" runat="server" DelimiterCharacters=""
                                                                Enabled="True" ServiceMethod="GetCompletionListAccountName" ServicePath="" CompletionInterval="100"
                                                                MinimumPrefixLength="1" CompletionSetCount="1" TargetControlID="txtCreditAccount"
                                                                UseContextKey="True" CompletionListCssClass="autoCompleteList" CompletionListItemCssClass="autoCompleteListItem" CompletionListHighlightedItemCssClass="autoCompleteSelectedListItem" OnClientShown="resetPosition">
                                                            </cc1:AutoCompleteExtender>
                                                            <br />
                                                        </div>
                                                    </div>
                                                    <div class="col-md-6">
                                                        <asp:CheckBox ID="chkPost" runat="server" Visible="false" />
                                                    </div>
                                                    <div class="col-md-12" style="text-align: center">
                                                        <asp:Button ID="btnPost" runat="server" OnClick="btnPost_Click" Text="<%$ Resources:Attendance,Post%>"
                                                            Visible="false" CssClass="btn btn-primary" />
                                                        <cc1:ConfirmButtonExtender ID="confirm1" runat="server" ConfirmText="Are you sure to post the record ?"
                                                            TargetControlID="btnPost">
                                                        </cc1:ConfirmButtonExtender>

                                                        <asp:Button ID="btnSave" runat="server" TabIndex="107" Text="<%$ Resources:Attendance,Save %>"
                                                            Visible="false" CssClass="btn btn-success" ValidationGroup="OnSave" OnClick="btnSave_Click" />

                                                        <asp:Button ID="btnReset" runat="server" TabIndex="108" Text="<%$ Resources:Attendance,Reset %>"
                                                            CssClass="btn btn-primary" CausesValidation="False" OnClick="btnReset_Click" />

                                                        <asp:Button ID="btnRefreshReceivestatus" runat="server" TabIndex="108" Text="<%$ Resources:Attendance,Refresh %>"
                                                            CssClass="btn btn-primary" CausesValidation="False" OnClick="btnRefreshReceivestatus_Click" />

                                                        <asp:Button ID="btnCancel" runat="server" TabIndex="109" Text="<%$ Resources:Attendance,Cancel %>"
                                                            CssClass="btn btn-danger" CausesValidation="False" OnClick="btnCancel_Click" />
                                                        <asp:Button ID="btnPayCommission" Visible="false" runat="server" TabIndex="109" Text="<%$ Resources:Attendance,Pay Commission %>"
                                                            CssClass="btn btn-primary" CausesValidation="False" OnClick="bnPayCommission_Click" />
                                                        <cc1:ConfirmButtonExtender ID="ConfirmButtonExtender3" runat="server" ConfirmText="Are you sure to pay commission ?"
                                                            TargetControlID="btnPayCommission">
                                                        </cc1:ConfirmButtonExtender>
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
                    <div class="tab-pane" id="Commission_Payment">
                        <asp:UpdatePanel ID="Update_Commission_Payment" runat="server">
                            <ContentTemplate>
                                <div class="row">
                                    <div class="col-md-12">
                                        <div class="box box-primary">
                                            <div class="box-body">
                                                <div class="form-group">
                                                    <div class="col-md-6">
                                                        <asp:Label ID="Label26" runat="server" Text="<%$ Resources:Attendance,From Date %>"></asp:Label>
                                                        <a style="color: Red">*</a>
                                                        <asp:RequiredFieldValidator EnableClientScript="true" Style="float: right;" runat="server" ID="RequiredFieldValidator6" ValidationGroup="Save"
                                                            Display="Dynamic" SetFocusOnError="true" ControlToValidate="txtPaymentFromDate" ErrorMessage="<%$ Resources:Attendance,Enter From Date%>"></asp:RequiredFieldValidator>

                                                        <asp:TextBox ID="txtPaymentFromDate" TabIndex="104" runat="server" CssClass="form-control" />
                                                        <cc1:CalendarExtender OnClientShown="showCalendar" ID="txtPaymentFromDate_CalendarExtender" runat="server" TargetControlID="txtPaymentFromDate" />
                                                        <br />
                                                    </div>
                                                    <div class="col-md-6">
                                                        <asp:Label ID="Label27" runat="server" Text="<%$ Resources:Attendance,To Date %>"></asp:Label>
                                                        <a style="color: Red">*</a>
                                                        <asp:RequiredFieldValidator EnableClientScript="true" Style="float: right;" runat="server" ID="RequiredFieldValidator7" ValidationGroup="Save"
                                                            Display="Dynamic" SetFocusOnError="true" ControlToValidate="txtPaymentToDate" ErrorMessage="<%$ Resources:Attendance,Enter To Date%>"></asp:RequiredFieldValidator>

                                                        <asp:TextBox ID="txtPaymentToDate" TabIndex="105" runat="server" CssClass="form-control" />
                                                        <cc1:CalendarExtender OnClientShown="showCalendar" ID="txtPaymentToDate_CalendarExtender" runat="server" TargetControlID="txtPaymentToDate" />
                                                        <br />
                                                    </div>
                                                    <div class="col-md-6">
                                                        <asp:Label ID="Label28" runat="server" Text="<%$ Resources:Attendance,Employee Name%>" />
                                                        <a style="color: Red">*</a>
                                                        <asp:RequiredFieldValidator EnableClientScript="true" Style="float: right;" runat="server" ID="RequiredFieldValidator8" ValidationGroup="Save"
                                                            Display="Dynamic" SetFocusOnError="true" ControlToValidate="txtCommissionPerson" ErrorMessage="<%$ Resources:Attendance,Enter Employee Name%>"></asp:RequiredFieldValidator>

                                                        <asp:TextBox ID="txtCommissionPerson" runat="server" CssClass="form-control" BackColor="#eeeeee"
                                                            OnTextChanged="txtCommissionPerson_TextChanged" AutoPostBack="true" />
                                                        <cc1:AutoCompleteExtender ID="AutoCompleteExtender4" runat="server" DelimiterCharacters=""
                                                            Enabled="True" CompletionInterval="100" CompletionSetCount="1" MinimumPrefixLength="1"
                                                            ServiceMethod="GetCompletionListEmployeeName" ServicePath="" TargetControlID="txtCommissionPerson"
                                                            UseContextKey="True" CompletionListCssClass="autoCompleteList" CompletionListItemCssClass="autoCompleteListItem" CompletionListHighlightedItemCssClass="autoCompleteSelectedListItem" OnClientShown="resetPosition">
                                                        </cc1:AutoCompleteExtender>
                                                        <br />
                                                    </div>
                                                    <div class="col-md-6">
                                                        <br />
                                                        <asp:Button ID="btnGetPaymentdetail" runat="server" TabIndex="107" Text="Get Commission Detail"
                                                            CssClass="btn btn-primary" OnClick="btnGetPaymentdetail_Click" />
                                                        <br />
                                                    </div>
                                                    <div class="col-md-12">
                                                        <div style="overflow: auto; max-height: 500px;">
                                                            <asp:GridView CssClass="table-striped table-bordered table table-hover" ID="gvPaymentdetail" PageSize='<%# int.Parse(Session["GridSize"].ToString()) %>'
                                                                runat="server" AutoGenerateColumns="False" Width="100%" AllowPaging="false" AllowSorting="false">
                                                                <Columns>
                                                                    <asp:TemplateField>
                                                                        <ItemTemplate>
                                                                            <asp:CheckBox ID="chkSelect" runat="server" AutoPostBack="true" OnCheckedChanged="chkSelect_OnCheckedChanged" />
                                                                        </ItemTemplate>
                                                                        <HeaderTemplate>
                                                                            <asp:CheckBox ID="chkHeader" runat="server" AutoPostBack="true" OnCheckedChanged="chkHeader_OnCheckedChanged" />
                                                                        </HeaderTemplate>
                                                                        <ItemStyle HorizontalAlign="Center" />
                                                                        <ItemStyle />
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="<%$ Resources:Attendance,Type%>">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblCommsiontype" runat="server" Text='<%# Eval("CommisionType") %>'></asp:Label>
                                                                        </ItemTemplate>
                                                                        <ItemStyle />
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="<%$ Resources:Attendance,Voucher No%>">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblVoucherNo" runat="server" Text='<%# Eval("Voucher_No") %>'></asp:Label>
                                                                        </ItemTemplate>
                                                                        <ItemStyle />
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="<%$ Resources:Attendance,Voucher Date%>">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblVoucheDate" runat="server" Text='<%# GeDate(Eval("Voucher_Date").ToString()) %>'></asp:Label>
                                                                        </ItemTemplate>
                                                                        <ItemStyle />
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="<%$ Resources:Attendance,Invoice No%>">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblTransId" runat="server" Text='<%# Eval("Trans_Id") %>' Visible="false"></asp:Label>
                                                                            <asp:Label ID="lblInvoiceNo" runat="server" Text='<%# Eval("Invoice_No") %>'></asp:Label>
                                                                            <asp:Label ID="lblInvoiceId" runat="server" Text='<%# Eval("Invoice_Id") %>' Visible="false"></asp:Label>
                                                                        </ItemTemplate>
                                                                        <ItemStyle />
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="<%$ Resources:Attendance,Invoice Date %>">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblInvoiceDate" runat="server" Text='<%# GeDate(Eval("Invoice_Date").ToString()) %>'></asp:Label>
                                                                        </ItemTemplate>
                                                                        <ItemStyle />
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="<%$ Resources:Attendance,Product Id %>">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblProuctCode" runat="server" Text='<%# Eval("ProductCode") %>'></asp:Label>
                                                                        </ItemTemplate>
                                                                        <ItemStyle />
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="<%$ Resources:Attendance,Product Name %>" Visible="false">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblProuctName" runat="server" Text='<%# Eval("EProductName") %>'></asp:Label>
                                                                        </ItemTemplate>
                                                                        <ItemStyle />
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="<%$ Resources:Attendance,Customer Name%>">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblcustomerName" runat="server" Text='<%# Eval("CustomerName") %>'></asp:Label>
                                                                        </ItemTemplate>
                                                                        <ItemStyle />
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="<%$ Resources:Attendance,Sales Person%>">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblSalespersonName" runat="server" Text='<%# GetSalesPerson(Eval("Invoice_Id").ToString()) %>'></asp:Label>
                                                                        </ItemTemplate>
                                                                        <ItemStyle />
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="<%$ Resources:Attendance,Invoice Amount%>">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblAmt" runat="server" Text='<%# SetDecimal(Eval("InvoiceAmount").ToString()) %>'></asp:Label>
                                                                        </ItemTemplate>
                                                                        <ItemStyle HorizontalAlign="Right" />
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Comm %">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblcommisionperAmt" runat="server" Text='<%# SetDecimal(Eval("Commission_Percentage").ToString()) %>'></asp:Label>
                                                                        </ItemTemplate>
                                                                        <ItemStyle HorizontalAlign="Right" />
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Commission Amount">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblcommissionAmt" runat="server" Text='<%# SetDecimal(Eval("Commission_Amount").ToString()) %>'></asp:Label>
                                                                        </ItemTemplate>
                                                                        <ItemStyle HorizontalAlign="Right" />
                                                                    </asp:TemplateField>
                                                                </Columns>


                                                                <PagerStyle CssClass="pagination-ys" />

                                                            </asp:GridView>
                                                        </div>
                                                        <br />
                                                    </div>

                                                    <div class="col-md-12">
                                                        <div style="overflow: auto; max-height: 500px;">
                                                            <asp:GridView CssClass="table-striped table-bordered table table-hover" ID="gvReturndetail" PageSize='<%# int.Parse(Session["GridSize"].ToString()) %>'
                                                                runat="server" AutoGenerateColumns="False" Width="100%" AllowPaging="false" AllowSorting="false">
                                                                <Columns>
                                                                    <asp:TemplateField>
                                                                        <ItemTemplate>
                                                                            <asp:CheckBox ID="chkSelect" runat="server" AutoPostBack="true" OnCheckedChanged="chkReturnSelect_OnCheckedChanged" />
                                                                        </ItemTemplate>
                                                                        <HeaderTemplate>
                                                                            <asp:CheckBox ID="chkHeader" runat="server" AutoPostBack="true" OnCheckedChanged="chkReturnHeader_OnCheckedChanged" />
                                                                        </HeaderTemplate>
                                                                        <ItemStyle HorizontalAlign="Center" />
                                                                        <ItemStyle />
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="<%$ Resources:Attendance,Type%>">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblCommsiontype" runat="server" Text='<%# Eval("CommisionType") %>'></asp:Label>
                                                                        </ItemTemplate>
                                                                        <ItemStyle />
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="<%$ Resources:Attendance,Voucher No%>">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblVoucherNo" runat="server" Text='<%# Eval("Voucher_No") %>'></asp:Label>
                                                                        </ItemTemplate>
                                                                        <ItemStyle />
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="<%$ Resources:Attendance,Voucher Date%>">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblVoucheDate" runat="server" Text='<%# GeDate(Eval("Voucher_Date").ToString()) %>'></asp:Label>
                                                                        </ItemTemplate>
                                                                        <ItemStyle />
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="<%$ Resources:Attendance,Invoice No%>">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblTransId" runat="server" Text='<%# Eval("Trans_Id") %>' Visible="false"></asp:Label>
                                                                            <asp:Label ID="lblInvoiceNo" runat="server" Text='<%# Eval("Invoice_No") %>'></asp:Label>
                                                                            <asp:Label ID="lblInvoiceId" runat="server" Text='<%# Eval("Invoice_Id") %>' Visible="false"></asp:Label>
                                                                        </ItemTemplate>
                                                                        <ItemStyle />
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="<%$ Resources:Attendance,Invoice Date %>">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblInvoiceDate" runat="server" Text='<%# GeDate(Eval("Invoice_Date").ToString()) %>'></asp:Label>
                                                                        </ItemTemplate>
                                                                        <ItemStyle />
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="<%$ Resources:Attendance,Product Id %>">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblProuctCode" runat="server" Text='<%# Eval("ProductCode") %>'></asp:Label>
                                                                        </ItemTemplate>
                                                                        <ItemStyle />
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="<%$ Resources:Attendance,Product Name %>" Visible="false">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblProuctName" runat="server" Text='<%# Eval("EProductName") %>'></asp:Label>
                                                                        </ItemTemplate>
                                                                        <ItemStyle />
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="<%$ Resources:Attendance,Customer Name%>">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblcustomerName" runat="server" Text='<%# Eval("CustomerName") %>'></asp:Label>
                                                                        </ItemTemplate>
                                                                        <ItemStyle />
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="<%$ Resources:Attendance,Sales Person%>">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblSalespersonName" runat="server" Text='<%# GetSalesPerson(Eval("Invoice_Id").ToString()) %>'></asp:Label>
                                                                        </ItemTemplate>
                                                                        <ItemStyle />
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="<%$ Resources:Attendance,Invoice Amount%>">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblAmt" runat="server" Text='<%# SetDecimal(Eval("InvoiceAmount").ToString()) %>'></asp:Label>
                                                                        </ItemTemplate>
                                                                        <ItemStyle HorizontalAlign="Right" />
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Comm %">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblcommisionperAmt" runat="server" Text='<%# SetDecimal(Eval("Commission_Percentage").ToString()) %>'></asp:Label>
                                                                        </ItemTemplate>
                                                                        <ItemStyle HorizontalAlign="Right" />
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Commission Amount">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblcommissionAmt" runat="server" Text='<%# SetDecimal(Eval("Commission_Amount").ToString()) %>'></asp:Label>
                                                                        </ItemTemplate>
                                                                        <ItemStyle HorizontalAlign="Right" />
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Return Amount">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblreturmAmount" runat="server" Text='<%# SetDecimal(Eval("Return_Amount").ToString()) %>'></asp:Label>
                                                                        </ItemTemplate>
                                                                        <ItemStyle HorizontalAlign="Right" />
                                                                    </asp:TemplateField>
                                                                </Columns>


                                                                <PagerStyle CssClass="pagination-ys" />

                                                            </asp:GridView>
                                                        </div>
                                                        <br />
                                                    </div>
                                                    <div class="row">
                                                        <div class="col-md-12">
                                                            <div class="box box-primary">
                                                                <div class="box-header with-border">
                                                                    <h3 class="box-title">
                                                                        <asp:Label ID="lblreturnDetail" runat="server" Text="Return Detail"></asp:Label></h3>
                                                                    <div class="box-tools pull-right">
                                                                        <button type="button" class="btn btn-box-tool" data-widget="collapse">
                                                                            <i class="fa fa-minus"></i>
                                                                        </button>
                                                                    </div>
                                                                </div>
                                                                <div class="box-body">
                                                                    <div class="form-group">
                                                                        <div class="col-md-6">
                                                                            <asp:Label ID="Label29" runat="server" Text="Total Payment Amount"></asp:Label>
                                                                            <asp:TextBox ID="txtTotalPaymentCommission" runat="server" CssClass="form-control"
                                                                                Enabled="false"></asp:TextBox>
                                                                            <br />
                                                                        </div>
                                                                        <div class="col-md-6">
                                                                            <asp:Label ID="Label30" runat="server" Text="Selected Payment"></asp:Label>
                                                                            <asp:TextBox ID="txtSelectedCommission" runat="server" CssClass="form-control" Enabled="false"></asp:TextBox>
                                                                            <br />
                                                                        </div>
                                                                        <div class="col-md-6">
                                                                            <asp:Label ID="Label36" runat="server" Text="Total Return Amount"></asp:Label>
                                                                            <asp:TextBox ID="txtTotalReturmAmount" runat="server" CssClass="form-control" Enabled="false"></asp:TextBox>
                                                                            <br />
                                                                        </div>
                                                                        <div class="col-md-6">
                                                                            <asp:Label ID="Label38" runat="server" Text="Selected Return Amount"></asp:Label>
                                                                            <asp:TextBox ID="txtSelectedReturnAmount" runat="server" CssClass="form-control" Enabled="false"></asp:TextBox>
                                                                            <br />
                                                                        </div>
                                                                        <div class="col-md-6">
                                                                            <asp:Label ID="Label37" runat="server" Text="Net Commission"></asp:Label>
                                                                            <asp:TextBox ID="txtNetpayCommision" runat="server" CssClass="form-control" Enabled="false"></asp:TextBox>
                                                                            <br />
                                                                        </div>
                                                                        <div class="col-md-6">
                                                                            <asp:Label ID="Label45" runat="server" Text="Net Commission(In Employee Currency)"></asp:Label>
                                                                            <asp:TextBox ID="txtNetpayCommisionLocal" runat="server" CssClass="form-control" Enabled="false"></asp:TextBox>
                                                                            <br />
                                                                        </div>
                                                                        <div class="col-md-12">
                                                                        </div>
                                                                        <div class="col-md-6">
                                                                            <asp:Label ID="Label31" runat="server" Text="<%$ Resources:Attendance,For Debit Account %>" />
                                                                            <asp:TextBox ID="txtpaymentdebitaccount" runat="server" CssClass="form-control" BackColor="#eeeeee"
                                                                                AutoPostBack="true" OnTextChanged="txtcmnAccount_textChnaged"></asp:TextBox>
                                                                            <cc1:AutoCompleteExtender ID="AutoCompleteExtender5" runat="server" DelimiterCharacters=""
                                                                                Enabled="True" ServiceMethod="GetCompletionListAccountName" ServicePath="" CompletionInterval="100"
                                                                                MinimumPrefixLength="1" CompletionSetCount="1" TargetControlID="txtpaymentdebitaccount"
                                                                                UseContextKey="True" CompletionListCssClass="autoCompleteList" CompletionListItemCssClass="autoCompleteListItem" CompletionListHighlightedItemCssClass="autoCompleteSelectedListItem" OnClientShown="resetPosition">
                                                                            </cc1:AutoCompleteExtender>
                                                                            <br />
                                                                        </div>
                                                                        <div class="col-md-6">
                                                                            <asp:Label ID="Label32" runat="server" Text="<%$ Resources:Attendance,For Credit Account %>" />
                                                                            <asp:TextBox ID="txtpaymentCreditaccount" runat="server" CssClass="form-control" BackColor="#eeeeee"
                                                                                AutoPostBack="true" OnTextChanged="txtcmnAccount_textChnaged"></asp:TextBox>
                                                                            <cc1:AutoCompleteExtender ID="AutoCompleteExtender6" runat="server" DelimiterCharacters=""
                                                                                Enabled="True" ServiceMethod="GetCompletionListAccountName" ServicePath="" CompletionInterval="100"
                                                                                MinimumPrefixLength="1" CompletionSetCount="1" TargetControlID="txtpaymentCreditaccount"
                                                                                UseContextKey="True" CompletionListCssClass="autoCompleteList" CompletionListItemCssClass="autoCompleteListItem" CompletionListHighlightedItemCssClass="autoCompleteSelectedListItem" OnClientShown="resetPosition">
                                                                            </cc1:AutoCompleteExtender>
                                                                            <br />
                                                                        </div>
                                                                    </div>
                                                                </div>
                                                            </div>
                                                        </div>
                                                    </div>
                                                    <div class="col-md-12" style="text-align: center">
                                                        <asp:Button ID="btnPayPaymentCommission" runat="server" TabIndex="109" Text="<%$ Resources:Attendance,Pay Commission %>"
                                                            CssClass="btn btn-primary" CausesValidation="False" OnClick="btnPayPaymentCommission_Click" />
                                                        <cc1:ConfirmButtonExtender ID="ConfirmButtonExtender1" runat="server" ConfirmText="Are you sure to pay commission ?"
                                                            TargetControlID="btnPayPaymentCommission">
                                                        </cc1:ConfirmButtonExtender>
                                                        <asp:Button ID="btnPayCommissionreset" runat="server" TabIndex="109" Text="<%$ Resources:Attendance,Reset %>"
                                                            CssClass="btn btn-primary" CausesValidation="False" OnClick="btnPayCommissionreset_Click" />
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
                    <div class="tab-pane" id="Report">
                        <asp:UpdatePanel ID="Update_Report" runat="server">
                            <ContentTemplate>
                                <div class="row">
                                    <div class="col-md-12">
                                        <div class="box box-primary">
                                            <div class="box-body">
                                                <div class="form-group">
                                                    <div class="col-md-12" style="border-style: solid; border-width: 1px; border-color: #eeeeee">
                                                        <asp:Label ID="LabelReportType" runat="server" Text="Report Type" />
                                                        <br />
                                                        <div class="col-md-4">
                                                            <asp:RadioButton ID="HeaderReportVoucher_Rb" runat="server" GroupName="RegularMenu"
                                                                Text="Header Report by Voucher Wise " AutoPostBack="True" Checked="True" OnCheckedChanged="HeaderReportVoucher_CheckedChanged" />

                                                            <br />
                                                        </div>
                                                        <div class="col-md-4">
                                                            <asp:RadioButton ID="HeaderReportInvoice_Rb" runat="server" GroupName="RegularMenu"
                                                                Text="Header Report by Invoice Wise" AutoPostBack="True" OnCheckedChanged="HeaderReportInvoice_CheckedChanged" />
                                                            <br />
                                                        </div>
                                                        <div class="col-md-4">
                                                            <asp:RadioButton ID="DetailReportCustomer_Rb" runat="server" GroupName="RegularMenu"
                                                                Text="Detail Report by Customer Wise" AutoPostBack="True" OnCheckedChanged="DetailReportCustomer_CheckedChanged" />
                                                            <br />
                                                        </div>
                                                        <div class="col-md-4">
                                                            <asp:RadioButton ID="DetailReportVoucher_Rb" runat="server" GroupName="RegularMenu"
                                                                Text="Detail Report by Voucher Wise" AutoPostBack="True" OnCheckedChanged="DetailReportVoucher_CheckedChanged" />
                                                            <br />
                                                        </div>
                                                        <div class="col-md-4">
                                                            <asp:RadioButton ID="DetailReportInvoice_Rb" runat="server" GroupName="RegularMenu"
                                                                Text="Detail Report by Invoice Wise" AutoPostBack="True" OnCheckedChanged="DetailReportInvoice_CheckedChanged" />
                                                            <br />
                                                        </div>
                                                        <div class="col-md-4">
                                                            <asp:RadioButton ID="SummaryReport_Rb" runat="server" AutoPostBack="True"
                                                                GroupName="RegularMenu" Text="Summary Report " OnCheckedChanged="SummaryReport_CheckedChanged" />
                                                            <br />
                                                        </div>
                                                    </div>
                                                    <div class="col-md-12">
                                                        <br />
                                                    </div>
                                                    <div class="col-md-6">
                                                        <asp:Label ID="Label33" runat="server" Text="<%$ Resources:Attendance,From Date %>"></asp:Label>
                                                        <a style="color: Red">*</a>
                                                        <asp:RequiredFieldValidator EnableClientScript="true" Style="float: right;" runat="server" ID="RequiredFieldValidator9" ValidationGroup="Save"
                                                            Display="Dynamic" SetFocusOnError="true" ControlToValidate="txtPaymentReportFromDate" ErrorMessage="<%$ Resources:Attendance,Enter From Date%>"></asp:RequiredFieldValidator>

                                                        <asp:TextBox ID="txtPaymentReportFromDate" TabIndex="104" runat="server" CssClass="form-control" />
                                                        <cc1:CalendarExtender OnClientShown="showCalendar" ID="CalendarExtender_txtPaymentReportFromDate" runat="server"
                                                            TargetControlID="txtPaymentReportFromDate" />
                                                        <br />
                                                    </div>
                                                    <div class="col-md-6">
                                                        <asp:Label ID="Label34" runat="server" Text="<%$ Resources:Attendance,To Date %>"></asp:Label>
                                                        <a style="color: Red">*</a>
                                                        <asp:RequiredFieldValidator EnableClientScript="true" Style="float: right;" runat="server" ID="RequiredFieldValidator10" ValidationGroup="Save"
                                                            Display="Dynamic" SetFocusOnError="true" ControlToValidate="txtPaymentReportToDate" ErrorMessage="<%$ Resources:Attendance,Enter To Date%>"></asp:RequiredFieldValidator>

                                                        <asp:TextBox ID="txtPaymentReportToDate" TabIndex="105" runat="server" CssClass="form-control" />
                                                        <cc1:CalendarExtender OnClientShown="showCalendar" ID="CalendarExtender_txtPaymentReportToDate" runat="server"
                                                            TargetControlID="txtPaymentReportToDate" />
                                                        <br />
                                                    </div>
                                                    <div class="col-md-6">
                                                        <asp:Label ID="Label35" runat="server" Text="<%$ Resources:Attendance,Employee Name%>" />
                                                        <a style="color: Red">*</a>
                                                        <asp:RequiredFieldValidator EnableClientScript="true" Style="float: right;" runat="server" ID="RequiredFieldValidator11" ValidationGroup="Save"
                                                            Display="Dynamic" SetFocusOnError="true" ControlToValidate="txtCommissionReportPerson" ErrorMessage="<%$ Resources:Attendance,Enter Employee Name%>"></asp:RequiredFieldValidator>

                                                        <asp:TextBox ID="txtCommissionReportPerson" runat="server" CssClass="form-control"
                                                            BackColor="#eeeeee" OnTextChanged="txtCommissionPerson_TextChanged" AutoPostBack="true" />
                                                        <cc1:AutoCompleteExtender ID="AutoCompleteExtender7" runat="server" DelimiterCharacters=""
                                                            Enabled="True" CompletionInterval="100" CompletionSetCount="1" MinimumPrefixLength="1"
                                                            ServiceMethod="GetCompletionListEmployeeName" ServicePath="" TargetControlID="txtCommissionReportPerson"
                                                            UseContextKey="True" CompletionListCssClass="autoCompleteList" CompletionListItemCssClass="autoCompleteListItem" CompletionListHighlightedItemCssClass="autoCompleteSelectedListItem" OnClientShown="resetPosition">
                                                        </cc1:AutoCompleteExtender>
                                                        <br />
                                                    </div>
                                                    <div class="col-md-6">
                                                        <asp:Label ID="LabelInvoiceNo" runat="server" Text="Invoice No."></asp:Label>
                                                        <asp:TextBox ID="txtInvoiceNo" runat="server" CssClass="form-control" Enabled="false"></asp:TextBox>
                                                        <asp:HiddenField ID="HiddenField1" runat="server" Value="0" />
                                                        <br />
                                                    </div>
                                                    <div class="col-md-6">
                                                        <asp:Label ID="Label271" runat="server" Text="Type" />
                                                        <asp:DropDownList ID="ddlCommissionReport" runat="server" CssClass="form-control"
                                                            AutoPostBack="true" OnSelectedIndexChanged="ddlPosted_SelectedIndexChanged">
                                                            <asp:ListItem Text="All" Value="All" Selected="True"></asp:ListItem>
                                                            <asp:ListItem Text="Sales" Value="Sales"></asp:ListItem>
                                                            <asp:ListItem Text="Technical" Value="Technical"></asp:ListItem>
                                                            <asp:ListItem Text="Agent" Value="Agent"></asp:ListItem>
                                                            <asp:ListItem Text="Developer" Value="Developer"></asp:ListItem>
                                                        </asp:DropDownList>
                                                        <br />
                                                    </div>
                                                    <div class="col-md-6">
                                                        <asp:Label ID="LabelVoucherNo" runat="server" Text="Voucher No."></asp:Label>
                                                        <asp:TextBox ID="TextReportVoucherNo" runat="server" CssClass="form-control"></asp:TextBox>
                                                        <asp:HiddenField ID="HiddenField2" runat="server" Value="0" />
                                                        <br />
                                                    </div>
                                                    <div class="col-md-6">
                                                        <asp:Label ID="LabelFilter" runat="server" Text="Filter" />
                                                        <br />
                                                        <div style="border-style: solid; border-width: 1px; border-color: #eeeeee">
                                                            <asp:RadioButton ID="AllReport_Rb" runat="server" AutoPostBack="True"
                                                                GroupName="RegularMenu1" Checked="True" Text="All" />
                                                            <asp:RadioButton ID="PendingReport_Rb" Style="margin-left: 15px" runat="server" GroupName="RegularMenu1"
                                                                Text="Pending" />
                                                            <asp:RadioButton ID="PaidVoucherReport_Rb" Style="margin-left: 15px" runat="server" GroupName="RegularMenu1"
                                                                Text="Paid Voucher" />
                                                        </div>
                                                        <br />
                                                    </div>
                                                    <div class="col-md-6">
                                                        <asp:Label ID="LabelReportcutomerName" runat="server" Text="<%$ Resources:Attendance,Customer Name %>"></asp:Label>
                                                        <asp:TextBox ID="txtReportcustomerName" runat="server" CssClass="form-control"
                                                            BackColor="#eeeeee" AutoPostBack="true" />

                                                        <cc1:AutoCompleteExtender ID="txtReportcustomerName_AutoCompleteExtender" runat="server"
                                                            DelimiterCharacters="" Enabled="True" CompletionInterval="100" CompletionSetCount="1"
                                                            MinimumPrefixLength="1" ServiceMethod="GetCompletionListCustomer" ServicePath=""
                                                            TargetControlID="txtReportcustomerName" UseContextKey="True" CompletionListCssClass="autoCompleteList" CompletionListItemCssClass="autoCompleteListItem" CompletionListHighlightedItemCssClass="autoCompleteSelectedListItem" OnClientShown="resetPosition">
                                                        </cc1:AutoCompleteExtender>
                                                        <br />
                                                    </div>
                                                    <div class="col-md-12" style="text-align: center">
                                                        <asp:Button ID="btnPayPaymentReportCommission" runat="server" TabIndex="109" Text="Show Report"
                                                            CssClass="btn btn-primary" CausesValidation="False" OnClick="btnPayPaymentCommissionReport_Click" />

                                                        <asp:Button ID="btnPayCommissionReportreset" runat="server" TabIndex="109" Text="<%$ Resources:Attendance,Reset %>"
                                                            CssClass="btn btn-primary" CausesValidation="False" OnClick="btnPayCommissionReportreset_Click" />
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
                    <div class="tab-pane" id="Configuration">
                        <asp:UpdatePanel ID="Update_Configuration" runat="server">
                            <ContentTemplate>
                                <div class="row">
                                    <div class="col-md-12">
                                        <div class="box box-primary">
                                            <div class="box-body">
                                                <div class="form-group">
                                                    <div class="col-md-6">
                                                        <asp:Label ID="Label39" runat="server" Text="<%$ Resources:Attendance,Sales Person%>" />
                                                        <a style="color: Red">*</a>
                                                        <asp:RequiredFieldValidator EnableClientScript="true" Style="float: right;" runat="server" ID="RequiredFieldValidator12" ValidationGroup="Save"
                                                            Display="Dynamic" SetFocusOnError="true" ControlToValidate="txtCommissionPersonConfig" ErrorMessage="<%$ Resources:Attendance,Enter Sales Person%>"></asp:RequiredFieldValidator>

                                                        <asp:TextBox ID="txtCommissionPersonConfig" runat="server" CssClass="form-control"
                                                            BackColor="#eeeeee" OnTextChanged="txtCommissionPersonConfig_TextChanged" AutoPostBack="true" />
                                                        <cc1:AutoCompleteExtender ID="AutoCompleteExtender9" runat="server" DelimiterCharacters=""
                                                            Enabled="True" CompletionInterval="100" CompletionSetCount="1" MinimumPrefixLength="1"
                                                            ServiceMethod="GetCompletionListEmployeeName" ServicePath="" TargetControlID="txtCommissionPersonConfig"
                                                            UseContextKey="True" CompletionListCssClass="autoCompleteList" CompletionListItemCssClass="autoCompleteListItem" CompletionListHighlightedItemCssClass="autoCompleteSelectedListItem" OnClientShown="resetPosition">
                                                        </cc1:AutoCompleteExtender>
                                                        <br />
                                                    </div>
                                                    <div class="col-md-6">
                                                        <asp:Label ID="Label40" runat="server" Text="<%$ Resources:Attendance,Parameter Level%>" />
                                                        <asp:DropDownList ID="ddlParameterLevel" runat="server" CssClass="form-control" AutoPostBack="true" OnSelectedIndexChanged="ddlParameterLevel_OnSelectedIndexChanged">
                                                            <asp:ListItem Text="On Sales" Value="On Sales"></asp:ListItem>
                                                            <asp:ListItem Text="On Category" Value="On Category"></asp:ListItem>
                                                        </asp:DropDownList>
                                                        <br />
                                                    </div>
                                                    <div class="col-md-12">
                                                        <asp:Label ID="Label41" runat="server" Text="<%$ Resources:Attendance,Notes %>" />
                                                        <asp:TextBox ID="txtNotesConfig" runat="server" CssClass="form-control" TextMode="MultiLine"></asp:TextBox>
                                                        <br />
                                                    </div>

                                                    <div id="trCategory" runat="server" visible="false" class="col-md-6">
                                                        <asp:Label ID="lbllocationsearch" runat="server" Text="<%$ Resources:Attendance,Category %>" />
                                                        <asp:DropDownList ID="ddlcategorysearch" runat="server" CssClass="form-control"></asp:DropDownList>
                                                        <asp:HiddenField ID="hdnConfigurationId" runat="server" />
                                                        <br />
                                                    </div>
                                                    <div class="col-md-6">
                                                        <asp:Label ID="Label42" runat="server" Text="<%$ Resources:Attendance,Sales Quota(Monthly)%>" />
                                                        <div class="input-group">
                                                            <asp:TextBox ID="txtTotalSalesConfig" runat="server" CssClass="form-control"></asp:TextBox>
                                                            <div class="input-group-addon">
                                                                <asp:Label ID="lblSalesQuotaCurrencyName" runat="server"></asp:Label>
                                                            </div>
                                                        </div>
                                                        <br />
                                                    </div>
                                                    <div class="col-md-6">
                                                        <asp:Label ID="Label43" runat="server" Text="<%$ Resources:Attendance,Commission(%)%>" />
                                                        <div class="input-group">
                                                            <asp:TextBox ID="txtCommPercentageConfig" runat="server" CssClass="form-control"></asp:TextBox>
                                                            <div class="input-group-addon">
                                                                <asp:Label ID="lblPer" runat="server" Text="%"></asp:Label>
                                                            </div>
                                                        </div>
                                                        <br />
                                                    </div>
                                                    <div class="col-md-6">
                                                        <asp:Label ID="Label44" runat="server" Text="<%$ Resources:Attendance,Commission Allows if not target%>" />
                                                        <asp:CheckBox ID="chkIsAllowCommission" runat="server" />
                                                        <br />
                                                    </div>
                                                    <div class="col-md-6">
                                                        <asp:Panel ID="pnlImgConfigDetailSave" runat="server" DefaultButton="ImgConfigDetailSave" Visible="false">
                                                            <asp:ImageButton runat="server" CausesValidation="False" ImageUrl="~/Images/add.png"
                                                                Height="29px" ToolTip="<%$ Resources:Attendance,Add %>" Width="35px" ID="ImgConfigDetailSave"
                                                                OnClick="ImgConfigDetailSave_Click"></asp:ImageButton>
                                                        </asp:Panel>
                                                        <br />
                                                    </div>
                                                    <div class="col-md-12">
                                                        <div style="overflow: auto; max-height: 500px;">
                                                            <asp:GridView CssClass="table-striped table-bordered table table-hover" ID="gvCategorySalesConfiguration" PageSize='<%# int.Parse(Session["GridSize"].ToString()) %>' runat="server"
                                                                AutoGenerateColumns="False" Width="100%" AllowPaging="false" AllowSorting="false">
                                                                <Columns>
                                                                    <asp:TemplateField HeaderText="<%$ Resources:Attendance,Delete %>">
                                                                        <ItemTemplate>
                                                                            <asp:ImageButton ID="IbtnDeleteConfig" runat="server" CausesValidation="False" CommandArgument='<%# Eval("Trans_Id") %>'
                                                                                ImageUrl="~/Images/Erase.png" OnCommand="IbtnDeleteConfig_Command" ToolTip="<%$ Resources:Attendance,Delete %>" />
                                                                        </ItemTemplate>
                                                                        <ItemStyle HorizontalAlign="Center" />
                                                                        <ItemStyle />
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="<%$ Resources:Attendance,Category Name%>">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblTransId" runat="server" Text='<%# Eval("Category_Name") %>'></asp:Label>
                                                                            <asp:HiddenField ID="hdncategoryid" runat="server" Value='<%# Eval("Category_Id") %>' />
                                                                        </ItemTemplate>
                                                                        <ItemStyle />
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="<%$ Resources:Attendance,Sales Quota(Monthly)%>">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblSalesquota" runat="server" Text='<%# SetDecimal(Eval("Sales_Quota").ToString()) %>'></asp:Label>
                                                                        </ItemTemplate>
                                                                        <ItemStyle />
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="<%$ Resources:Attendance,Commission(%)%>">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblconfigPer" runat="server" Text='<%# SetDecimal(Eval("Commission_Percentage").ToString()) %>'></asp:Label>
                                                                        </ItemTemplate>
                                                                        <ItemStyle />
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="<%$ Resources:Attendance,Commission Allows if not target%>">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblAllow" runat="server" Text='<%# Eval("Is_Allow") %>'></asp:Label>
                                                                        </ItemTemplate>
                                                                        <ItemStyle />
                                                                    </asp:TemplateField>
                                                                </Columns>


                                                                <PagerStyle CssClass="pagination-ys" />

                                                            </asp:GridView>
                                                        </div>

                                                        <br />
                                                    </div>
                                                    <div class="col-md-12" style="text-align: center">
                                                        <asp:Button ID="btnsaveConfiguration" runat="server" TabIndex="109" Text="<%$ Resources:Attendance,Save %>"
                                                            CssClass="btn btn-success" CausesValidation="False" OnClick="btnsaveConfiguration_Click" />

                                                        <asp:Button ID="btnResetConfiguration" runat="server" TabIndex="109" Text="<%$ Resources:Attendance,Reset %>"
                                                            CssClass="btn btn-primary" CausesValidation="False" OnClick="btnResetConfiguration_Click" />
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

    <div class="modal fade" id="Serial_No_Modal" tabindex="-1" role="dialog" aria-labelledby="Serial_No_ModalLabel" aria-hidden="true">
        <div class="modal-dialog modal-lg">
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal">
                        <span aria-hidden="true">&times;</span><span class="sr-only">Close</span></button>
                    <h4 class="modal-title" id="Serial_No_ModalLabel">Approval</h4>
                </div>
                <div class="modal-body">
                    <asp:UpdatePanel ID="Update_Serial_No_Modal" runat="server">
                        <ContentTemplate>
                            <div class="row">
                                <div class="col-md-12">
                                    <div class="box box-primary">
                                        <div class="box-body">
                                            <div class="form-group">
                                                <div class="col-md-12">
                                                    <asp:Label ID="lblHandledEmp" runat="server" Text="<%$ Resources:Attendance,Employee Name %>" />
                                                    <a style="color: Red">*</a>
                                                    <asp:RequiredFieldValidator EnableClientScript="true" Style="float: right;" runat="server" ID="RequiredFieldValidator13" ValidationGroup="Save"
                                                        Display="Dynamic" SetFocusOnError="true" ControlToValidate="txtHandledEmp" ErrorMessage="<%$ Resources:Attendance,Enter Employee Name%>"></asp:RequiredFieldValidator>
                                                    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                                                        <ContentTemplate>
                                                            <asp:TextBox ID="txtHandledEmp" runat="server" CssClass="form-control" BackColor="#eeeeee" />
                                                            <cc1:AutoCompleteExtender ID="txtHandledEmp_AutoCompleteExtender" runat="server"
                                                                DelimiterCharacters="" Enabled="True" CompletionInterval="100" CompletionSetCount="1"
                                                                MinimumPrefixLength="1" ServiceMethod="GetCompletionListEmployeeName" ServicePath=""
                                                                TargetControlID="txtHandledEmp" UseContextKey="True" CompletionListCssClass="autoCompleteList" CompletionListItemCssClass="autoCompleteListItem" CompletionListHighlightedItemCssClass="autoCompleteSelectedListItem">
                                                            </cc1:AutoCompleteExtender>
                                                        </ContentTemplate>
                                                    </asp:UpdatePanel>
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
                    <asp:UpdatePanel ID="Update_Serial_No_Modal_Button" runat="server">
                        <ContentTemplate>
                            <asp:Button ID="btnSavecommissionemp" runat="server" CssClass="btn btn-success" OnClick="btnSavecommissionemp_Click"
                                Text="<%$ Resources:Attendance,Save %>" />

                            <button type="button" class="btn btn-danger" data-dismiss="modal">
                                Close</button>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
            </div>
        </div>
    </div>

    <asp:UpdateProgress ID="UpdateProgress6" runat="server" AssociatedUpdatePanelID="Update_Serial_No_Modal">
        <ProgressTemplate>
            <div class="modal_Progress">
                <div class="center_Progress">
                    <img alt="" src="../Images/ajax-loader2.gif" style="vertical-align: middle; background-color: White; border-radius: 10px;" />
                </div>
            </div>
        </ProgressTemplate>
    </asp:UpdateProgress>

    <asp:UpdateProgress ID="UpdateProgress7" runat="server" AssociatedUpdatePanelID="Update_Serial_No_Modal_Button">
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

    <asp:UpdateProgress ID="UpdateProgress3" runat="server" AssociatedUpdatePanelID="Update_Commission_Payment">
        <ProgressTemplate>
            <div class="modal_Progress">
                <div class="center_Progress">
                    <img alt="" src="../Images/ajax-loader2.gif" style="vertical-align: middle; background-color: White; border-radius: 10px;" />
                </div>
            </div>
        </ProgressTemplate>
    </asp:UpdateProgress>

    <asp:UpdateProgress ID="UpdateProgress4" runat="server" AssociatedUpdatePanelID="Update_Report">
        <ProgressTemplate>
            <div class="modal_Progress">
                <div class="center_Progress">
                    <img alt="" src="../Images/ajax-loader2.gif" style="vertical-align: middle; background-color: White; border-radius: 10px;" />
                </div>
            </div>
        </ProgressTemplate>
    </asp:UpdateProgress>

    <asp:UpdateProgress ID="UpdateProgress5" runat="server" AssociatedUpdatePanelID="Update_Configuration">
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

        function Li_Tab_Configuration() {
            document.getElementById('<%= Btn_Configuration.ClientID %>').click();
        }

        function Serial_No_Modal_Show() {
            document.getElementById('<%= Btn_Serial_No_Modal.ClientID %>').click();
        }
    </script>
</asp:Content>
