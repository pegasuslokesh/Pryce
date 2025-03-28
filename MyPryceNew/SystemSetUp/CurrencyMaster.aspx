<%@ Page Title="" Language="C#" MasterPageFile="~/ERPMaster.master" AutoEventWireup="true" CodeFile="CurrencyMaster.aspx.cs" Inherits="SystemSetUp_CurrencyMaster" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="Head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="HeaderContent" runat="Server">
    <h1>
        <i class="fas fa-dollar-sign"></i>
        <asp:Label ID="lblHeader" runat="server" Text="<%$ Resources:Attendance,Currency Setup%>"></asp:Label>

        <%--<small>Control panel</small>--%>
    </h1>
    <ol class="breadcrumb">
        <li><a href="#"><i class="fa fa-dashboard"></i>
            <asp:Label ID="Label6" runat="server" Text="<%$ Resources:Attendance,System SetUp%>"></asp:Label></a></li>
        <li><a href="#"><i class="fa fa-dashboard"></i>
            <asp:Label ID="Label10" runat="server" Text="<%$ Resources:Attendance,System SetUp%>"></asp:Label></a></li>
        <li class="active">
            <asp:Label ID="Label5" runat="server" Text="<%$ Resources:Attendance,Currency Setup%>"></asp:Label></li>
    </ol>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="Server">
    <asp:UpdatePanel ID="Update_Button" runat="server">
        <ContentTemplate>
            <asp:HiddenField runat="server" ID="hdnCanEdit" />
            <asp:HiddenField runat="server" ID="hdnCanDelete" />
            <asp:Button ID="Btn_List" Style="display: none;" runat="server" OnClick="btnList_Click" Text="List" />
            <asp:Button ID="Btn_New" Style="display: none;" runat="server" OnClick="btnNew_Click" Text="New" />
            <asp:Button ID="Btn_Bin" Style="display: none;" runat="server" OnClick="btnBin_Click" Text="Bin" />
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
                    <li><a href="#Bin" onclick="Li_Tab_Bin()" data-toggle="tab">
                        <i class="fa fa-trash"></i>&nbsp;&nbsp;<asp:Label ID="Label13" runat="server" Text="<%$ Resources:Attendance,Bin %>"></asp:Label></a></li>
                    <li id="Li_New"><a onclick="Li_Tab_New()" href="#New" data-toggle="tab">
                        <asp:UpdatePanel ID="Update_Li" runat="server">
                            <ContentTemplate>
                                <i class="fa fa-file"></i>&nbsp;&nbsp;
                                <asp:Label ID="Lbl_Tab_New" runat="server" Text="<%$ Resources:Attendance,New%>"></asp:Label>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </a></li>
                    <li id="Li_List" class="active"><a onclick="Li_Tab_List()" href="#List" data-toggle="tab">
                        <i class="fa fa-list"></i>&nbsp;&nbsp;<asp:Label ID="Label3" runat="server" Text="<%$ Resources:Attendance,List %>"></asp:Label></a></li>
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
                                                    <asp:HiddenField ID="HDFSort" runat="server" />
                                                    <asp:Label ID="Label22" runat="server" Text="<%$ Resources:Attendance,Advance Search%>"></asp:Label></h3>
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
                                                <div class="col-lg-3">
                                                    <asp:DropDownList ID="ddlFieldName" runat="server" CssClass="form-control">
                                                        <asp:ListItem Text="<%$ Resources:Attendance, Currency ID %>" Value="Currency_ID"></asp:ListItem>
                                                        <asp:ListItem Text="<%$ Resources:Attendance,Country Name %>" Value="Country_Name"></asp:ListItem>
                                                        <asp:ListItem Text="<%$ Resources:Attendance, Currency Code %>" Value="Currency_Code"></asp:ListItem>
                                                        <asp:ListItem Text="<%$ Resources:Attendance, Currency Name %>" Value="Currency_Name"
                                                            Selected="True"></asp:ListItem>
                                                        <asp:ListItem Text="<%$ Resources:Attendance, Created By %>" Value="CreatedBy"></asp:ListItem>
                                                        <asp:ListItem Text="<%$ Resources:Attendance, Modified By %>" Value="Modified_User"></asp:ListItem>
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
                                                        <asp:TextBox ID="txtValue" placeholder="Search from Content" runat="server" CssClass="form-control"></asp:TextBox>
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



                                <div class="box box-warning box-solid" <%= GvCurrency.Rows.Count>0?"style='display:block'":"style='display:none'"%>>

                                    <div class="box-body">
                                        <div class="row">
                                            <div class="col-md-12">
                                                <div class="flow">

                                                    <asp:GridView CssClass="table-striped table-bordered table table-hover" ID="GvCurrency" PageSize='<%# int.Parse(Session["GridSize"].ToString()) %>' runat="server"
                                                        AutoGenerateColumns="False" Width="100%" AllowPaging="True" OnPageIndexChanging="GvCurrency_PageIndexChanging"
                                                        AllowSorting="True" OnSorting="GvCurrency_Sorting">

                                                        <Columns>


                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,Action%>">
                                                                <ItemTemplate>
                                                                    <div class="dropdown" style="position: absolute;">
                                                                        <button class="btn btn-default dropdown-toggle" type="button" data-toggle="dropdown">
                                                                            <i class="fa fa-ellipsis-h" aria-hidden="true"></i>
                                                                        </button>
                                                                        <ul class="dropdown-menu">


                                                                            <li <%= hdnCanEdit.Value=="true"?"style='display:block'":"style='display:none'" %>>
                                                                                <asp:LinkButton ID="btnEdit" runat="server" CommandArgument='<%# Eval("Country_ID") %>'
                                                                                    CommandName='<%# Eval("Currency_ID") %>' OnCommand="btnEdit_Command"
                                                                                    CausesValidation="False"><i class="fa fa-pencil"></i><%# Resources.Attendance.Edit%> </asp:LinkButton>
                                                                            </li>
                                                                            <li <%= hdnCanDelete.Value=="true"?"style='display:block'":"style='display:none'" %>>
                                                                                <asp:LinkButton ID="IbtnDelete" runat="server" CausesValidation="False"
                                                                                    CommandArgument='<%# Eval("Currency_ID") %>' CommandName='<%# Eval("Country_ID") %>'
                                                                                    OnCommand="IbtnDelete_Command"><i class="fa fa-trash"></i><%# Resources.Attendance.Delete%></asp:LinkButton>
                                                                                <cc1:ConfirmButtonExtender ID="confirm2" runat="server" ConfirmText="<%$ Resources:Attendance,Are you sure you want to delete the record?%>"
                                                                                    TargetControlID="IbtnDelete">
                                                                                </cc1:ConfirmButtonExtender>
                                                                            </li>


                                                                        </ul>
                                                                    </div>

                                                                </ItemTemplate>
                                                            </asp:TemplateField>


                                                            <asp:BoundField DataField="Currency_ID" HeaderText="<%$ Resources:Attendance,Currency Id %>"
                                                                SortExpression="Currency_ID" />
                                                            <asp:BoundField DataField="Country_Name" HeaderText="<%$ Resources:Attendance,Country Name %>"
                                                                SortExpression="Country_Name" />
                                                            <asp:BoundField DataField="Currency_Code" HeaderText="<%$ Resources:Attendance,Currency Code %>"
                                                                SortExpression="Currency_Code" />
                                                            <asp:BoundField DataField="Currency_Name" HeaderText="<%$ Resources:Attendance,Currency Name %>"
                                                                SortExpression="Currency_Name" />
                                                            <asp:BoundField DataField="Currency_Name_L" HeaderText="<%$ Resources:Attendance,Currency Name(Local) %>"
                                                                SortExpression="Currency_Name_L" />
                                                            <asp:BoundField DataField="CreatedBy" HeaderText="<%$ Resources:Attendance,Created By %>"
                                                                SortExpression="CreatedBy" />
                                                            <asp:BoundField DataField="Modified_User" HeaderText="<%$ Resources:Attendance,Modified By %>"
                                                                SortExpression="Modified_User" />
                                                        </Columns>

                                                        <PagerStyle CssClass="pagination-ys" />

                                                    </asp:GridView>

                                                </div>

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
                                                        <asp:Label ID="Lbl_name" runat="server" Text="<%$ Resources:Attendance,Country Name %>"></asp:Label>
                                                        <a style="color: Red">*</a>
                                                        <asp:RequiredFieldValidator EnableClientScript="true" Style="float: right;" Display="Dynamic" ID="RequiredFieldValidator7" runat="server" ControlToValidate="ddlCountry"
                                                            InitialValue="--Select--" SetFocusOnError="true" ValidationGroup="Save" ErrorMessage="<%$ Resources:Attendance,Select Country Name%>"></asp:RequiredFieldValidator>

                                                        <asp:DropDownList ID="ddlCountry" runat="server" CssClass="form-control">
                                                        </asp:DropDownList>
                                                        <br />
                                                        <asp:Label ID="lblCurrencyName" runat="server" Text="<%$ Resources:Attendance,Currency Name %>"></asp:Label>
                                                        <a style="color: Red">*</a>
                                                        <asp:RequiredFieldValidator EnableClientScript="true" Style="float: right;" Display="Dynamic" runat="server" ID="RequiredFieldValidator2" ValidationGroup="Save" SetFocusOnError="true" ControlToValidate="txtCurrencyName" ErrorMessage="<%$ Resources:Attendance,Enter Currency Name%> " />

                                                        <asp:TextBox ID="txtCurrencyName" runat="server" AutoPostBack="true" OnTextChanged="txtCurrencyName_TextChanged" BackColor="#eeeeee" CssClass="form-control"></asp:TextBox>
                                                        <cc1:AutoCompleteExtender ID="AutoCompleteExtender1" runat="server" DelimiterCharacters=""
                                                            Enabled="True" ServiceMethod="GetCompletionList" ServicePath="" CompletionInterval="100"
                                                            MinimumPrefixLength="1" CompletionSetCount="1" TargetControlID="txtCurrencyName"
                                                            UseContextKey="True" CompletionListCssClass="autoCompleteList" CompletionListItemCssClass="autoCompleteListItem" CompletionListHighlightedItemCssClass="autoCompleteSelectedListItem" OnClientShown="resetPosition">
                                                        </cc1:AutoCompleteExtender>
                                                        <br />
                                                        <asp:Label ID="lblCurrencySymbol" runat="server" Text="<%$ Resources:Attendance,Currency Symbol %>"></asp:Label>
                                                        <asp:TextBox ID="txtCurrencySymbol" runat="server" CssClass="form-control"></asp:TextBox>
                                                        <br />
                                                        <asp:Label ID="lblCurrencyDecimalCount" runat="server" Text="<%$ Resources:Attendance,Decimal Count %>"></asp:Label>
                                                        <asp:TextBox ID="txtCurrencyDecimalCount" MaxLength="1" runat="server" OnTextChanged="txtCurrencyDecimalCount_TextChanged" AutoPostBack="true" CssClass="form-control"></asp:TextBox>
                                                        <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" runat="server" TargetControlID="txtCurrencyDecimalCount" FilterType="Numbers"></cc1:FilteredTextBoxExtender>
                                                        <br />
                                                        
                                                        <asp:Label ID="Label2" runat="server" Text="<%$ Resources:Attendance,Fractional Currency Name%>"></asp:Label>
                                                        <asp:TextBox ID="txtAfterDecimal" runat="server" CssClass="form-control"></asp:TextBox>
                                                        <br />
                                                        
                                                        <asp:Label ID="Label9" runat="server" Text="Statement threshold for credit limit"></asp:Label>
                                                        <asp:TextBox ID="txtCreditStatThreshold" MaxLength="10" runat="server" CssClass="form-control"></asp:TextBox>
                                                        <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender3" runat="server" TargetControlID="txtCreditStatThreshold" ValidChars="0,1,2,3,4,5,6,7,8,9,."></cc1:FilteredTextBoxExtender>
                                                        <br />
                                                    </div>

                                                    <div class="col-md-6">
                                                        <asp:Label ID="lblCurrencyCode" runat="server" Text="<%$ Resources:Attendance,Currency Code %>"></asp:Label>
                                                        <asp:TextBox ID="txtCurrencyCode" MaxLength="11" runat="server" CssClass="form-control"></asp:TextBox>
                                                        <br />
                                                        <asp:Label ID="lblLCurrencyName" runat="server" Text="<%$ Resources:Attendance,Currency Name(Local) %>"></asp:Label>
                                                        <asp:TextBox ID="txtLCurrencyName" runat="server" CssClass="form-control"></asp:TextBox>
                                                        <br />
                                                        <asp:Label ID="lblCurrencyValue" runat="server" Text="<%$ Resources:Attendance,Currency Value %>"></asp:Label>
                                                        <a style="color: Red">*</a>
                                                        <asp:RequiredFieldValidator EnableClientScript="true" Style="float: right;" Display="Dynamic" runat="server" ID="RequiredFieldValidator1" ValidationGroup="Save" SetFocusOnError="true" ControlToValidate="txtCurrencyValue" ErrorMessage="<%$ Resources:Attendance,Enter Currency Value%>" />

                                                        <asp:TextBox ID="txtCurrencyValue" runat="server" CssClass="form-control"></asp:TextBox>
                                                        <cc1:FilteredTextBoxExtender ID="filtetextbox" runat="server" TargetControlID="txtCurrencyValue" ValidChars="0,1,2,3,4,5,6,7,8,9,."></cc1:FilteredTextBoxExtender>
                                                        <br />
                                                        <asp:Label ID="Label4" runat="server" Text="<%$ Resources:Attendance,Base Currency %>"></asp:Label>
                                                        <asp:TextBox ID="txtBaseCurrency" runat="server" CssClass="form-control"></asp:TextBox>
                                                        <br />
                                                        <asp:Label ID="Label7" runat="server" Text="<%$ Resources:Attendance,Smallest Denomination %>"></asp:Label>
                                                        <a style="color: Red">*</a>
                                                        <asp:RequiredFieldValidator EnableClientScript="true" Style="float: right;" Display="Dynamic" runat="server" ID="RequiredFieldValidator3" ValidationGroup="Save" SetFocusOnError="true" ControlToValidate="Txt_Small_Denomination" ErrorMessage="<%$ Resources:Attendance,Enter Small Denomination%> " />

                                                        <asp:TextBox ID="Txt_Small_Denomination" MaxLength="1" runat="server" CssClass="form-control"></asp:TextBox>
                                                        <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender2" runat="server" TargetControlID="Txt_Small_Denomination" ValidChars="0,1,2,3,4,5,6,7,8,9,."></cc1:FilteredTextBoxExtender>

                                                        <asp:RegularExpressionValidator ID="RangeValidator1" runat="server" ValidationGroup="Save"
                                                            ControlToValidate="Txt_Small_Denomination" Display="Dynamic"
                                                            ErrorMessage="<%$ Resources:Attendance,Invalid Smallest Denomination%> "
                                                            ValidationExpression="^\d+(?:\.\d{0,9})?$"
                                                            SetFocusOnError="True"></asp:RegularExpressionValidator>

                                                        </br>

                                                       


                                                        <%--ValidationExpression="^[0-1]{1,1}(?:\.\d{0,11})?$"    --%>
                                                        <%--&nbsp
                                                        <asp:RangeValidator ID="val" runat="server" ErrorMessage="<%$ Resources:Attendance,Enter Between 0 to 1%> " SetFocusOnError="true" ValidationGroup="Save"
                                                            ControlToValidate="Txt_Small_Denomination" Display="Dynamic" ForeColor="red" Type="Double"
                                                            MinimumValue="0" MaximumValue="1">
                                                        </asp:RangeValidator>--%>
                                                        <br />
                                                    </div>

                                                    <div class="col-md-12" style="text-align: center">
                                                        <br />
                                                        <asp:Button ID="btnCSave" CssClass="btn btn-success" runat="server" ValidationGroup="Save" OnClick="btnCSave_Click" Text="<%$ Resources:Attendance,Save %>" />
                                                        <asp:Button ID="BtnReset" Style="margin-left: 15px;" CausesValidation="False" OnClick="BtnReset_Click" CssClass="btn btn-primary" runat="server" Text="<%$ Resources:Attendance,Reset %>" />
                                                        <asp:Button ID="BtnCCancel" Style="margin-left: 15px;" CssClass="btn btn-danger" runat="server" CausesValidation="False" OnClick="BtnCCancel_Click" Text="<%$ Resources:Attendance,Cancel %>" />
                                                        <asp:HiddenField ID="editid" runat="server" />
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
                                                    <asp:Label ID="Label8" runat="server" Text="<%$ Resources:Attendance,Advance Search%>"></asp:Label></h3>
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
                                                        <asp:ListItem Text="<%$ Resources:Attendance, Currency ID %>" Value="Currency_ID"></asp:ListItem>
                                                        <asp:ListItem Text="<%$ Resources:Attendance,Country Name %>" Value="Country_Name"></asp:ListItem>
                                                        <asp:ListItem Text="<%$ Resources:Attendance, Currency Code %>" Value="Currency_Code"></asp:ListItem>
                                                        <asp:ListItem Text="<%$ Resources:Attendance, Currency Name %>" Value="Currency_Name"
                                                            Selected="True"></asp:ListItem>
                                                        <asp:ListItem Text="<%$ Resources:Attendance, Created By %>" Value="CreatedBy"></asp:ListItem>
                                                        <asp:ListItem Text="<%$ Resources:Attendance, Modified By %>" Value="Modified_User"></asp:ListItem>
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
                                                        <asp:TextBox ID="txtValueBin" placeholder="Search from Content" runat="server" CssClass="form-control"></asp:TextBox>
                                                    </asp:Panel>
                                                </div>
                                                <div class="col-lg-3">
                                                    <asp:LinkButton ID="btnbindBin" runat="server" CausesValidation="False" OnClick="btnbindBin_Click" ToolTip="<%$ Resources:Attendance,Search %>"><span class="fa fa-search"  style="font-size:25px;"></span></asp:LinkButton>&nbsp;&nbsp;&nbsp;

                                                    <asp:LinkButton ID="btnRefreshBin" runat="server" CausesValidation="False" OnClick="btnRefreshBin_Click" ToolTip="<%$ Resources:Attendance,Refresh %>"><span class="fa fa-repeat"  style="font-size:25px;"></span></asp:LinkButton>&nbsp;&nbsp;&nbsp;

                                                    <asp:LinkButton ID="imgBtnRestore" CausesValidation="False" Visible="false" runat="server" OnClick="imgBtnRestore_Click" ToolTip="<%$ Resources:Attendance, Active %>"><span class="far fa-lightbulb"  style="font-size:25px;"></span></asp:LinkButton>&nbsp;&nbsp;&nbsp;

                                                    <asp:ImageButton ID="ImgbtnSelectAll" Visible="false" Style="width: 33px;" runat="server" OnClick="ImgbtnSelectAll_Click" ToolTip="<%$ Resources:Attendance, Select All %>" AutoPostBack="true" ImageUrl="~/Images/selectAll.png" />
                                                </div>


                                            </div>
                                        </div>
                                    </div>
                                </div>

                                <div class="box box-warning box-solid" <%= GvCurrencyBin.Rows.Count>0?"style='display:block'":"style='display:none'"%>>

                                    <div class="box-body">
                                        <div class="row">
                                            <div class="col-md-12">
                                                <div style="overflow-y: auto;">
                                                    <asp:Label ID="Label1" runat="server" Visible="false"></asp:Label>
                                                    <asp:GridView CssClass="table-striped table-bordered table table-hover" ID="GvCurrencyBin" PageSize='<%# int.Parse(Session["GridSize"].ToString()) %>'
                                                        runat="server" AutoGenerateColumns="False" Width="100%" DataKeyNames="Currency_ID" AllowPaging="True" OnPageIndexChanging="GvCurrencyBin_PageIndexChanging"
                                                        OnSorting="GvCurrencyBin_OnSorting" AllowSorting="true">
                                                        <Columns>
                                                            <asp:TemplateField>
                                                                <HeaderTemplate>
                                                                    <asp:CheckBox ID="chkCurrent" runat="server" OnCheckedChanged="chkCurrent_CheckedChanged"
                                                                        AutoPostBack="true" />
                                                                </HeaderTemplate>
                                                                <ItemTemplate>
                                                                    <asp:CheckBox ID="chkSelect" runat="server" />
                                                                </ItemTemplate>
                                                                <ItemStyle HorizontalAlign="Center" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,Currency Id%>" SortExpression="Currency_ID">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblgvCurrencyId" runat="server" Text='<%# Eval("Currency_ID") %>' />
                                                                </ItemTemplate>
                                                                <ItemStyle />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,Country Name%>" SortExpression="Country_Name">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblgvCountryName" runat="server" Text='<%# Eval("Country_Name") %>' />
                                                                </ItemTemplate>
                                                                <ItemStyle />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,Currency Code %>" SortExpression="Currency_Code">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblgvCurrencyCode" runat="server" Text='<%# Eval("Currency_Code") %>' />
                                                                </ItemTemplate>
                                                                <ItemStyle />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,Currency Name %>" SortExpression="Currency_Name">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblgvCurrencyName" runat="server" Text='<%# Eval("Currency_Name") %>' />
                                                                </ItemTemplate>
                                                                <ItemStyle />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,Currency Name(Local) %>"
                                                                SortExpression="Currency_Name_L">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblgvCurrencyNameL" runat="server" Text='<%# Eval("Currency_Name_L") %>' />
                                                                </ItemTemplate>
                                                                <ItemStyle />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,Created By %>"
                                                                SortExpression="Created_User">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblgvCreated_User" runat="server" Text='<%# Eval("CreatedBy") %>' />
                                                                </ItemTemplate>
                                                                <ItemStyle />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,Modified By %>"
                                                                SortExpression="Modified_User">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblgvModified_User" runat="server" Text='<%# Eval("Modified_User") %>' />
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
                                        <!-- /.box-body -->
                                    </div>
                                    <!-- /.box -->
                                </div>

                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </div>

                </div>
            </div>

        </div>
    </div>
    <asp:UpdateProgress ID="UpdateProgress1" runat="server" AssociatedUpdatePanelID="Update_New">
        <ProgressTemplate>
            <div class="modal_Progress">
                <div class="center_Progress">
                    <img alt="" src="../Images/ajax-loader2.gif" style="vertical-align: middle; background-color: White; border-radius: 10px;" />
                </div>
            </div>
        </ProgressTemplate>
    </asp:UpdateProgress>

    <asp:UpdateProgress ID="UpdateProgress2" runat="server" AssociatedUpdatePanelID="Update_List">
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
        function LI_New_Active() {
            $("#Li_List").removeClass("active");
            $("#List").removeClass("active");

            $("#Li_New").addClass("active");
            $("#New").addClass("active");
        }

    </script>
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

        function Li_Tab_List() {
            document.getElementById('<%= Btn_List.ClientID %>').click();
        }
        function Li_Tab_New() {
            document.getElementById('<%= Btn_New.ClientID %>').click();
        }
        function Li_Tab_Bin() {
            document.getElementById('<%= Btn_Bin.ClientID %>').click();
        }
        function LI_List_Active() {
            $("#Li_List").addClass("active");
            $("#List").addClass("active");

            $("#Li_New").removeClass("active");
            $("#New").removeClass("active");
        }
    </script>
</asp:Content>




