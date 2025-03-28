<%@ Page Title="" Language="C#" MasterPageFile="~/ERPMaster.master" AutoEventWireup="true" CodeFile="OptionCategoryMaster.aspx.cs" Inherits="Inventory_OptionCategoryMaster" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="Head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="HeaderContent" runat="Server">
    <h1>
        <i class="fas fa-sitemap"></i>&nbsp;&nbsp;
        <asp:Label ID="lblHeader" runat="server" Text="<%$ Resources:Attendance,Option Category Setup%>"></asp:Label>
    </h1>
    <ol class="breadcrumb">
        <li><a href="#"><i class="fa fa-dashboard"></i>
            <asp:Label ID="Label6" runat="server" Text="<%$ Resources:Attendance,Inventory%>"></asp:Label></a></li>
        <li><a href="#"><i class="fa fa-dashboard"></i>
            <asp:Label ID="Label10" runat="server" Text="<%$ Resources:Attendance,Inventory%>"></asp:Label></a></li>
        <li class="active">
            <asp:Label ID="Label5" runat="server" Text="<%$ Resources:Attendance,Option Category%>"></asp:Label></li>
    </ol>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="Server">
    <asp:UpdatePanel ID="Update_Button" runat="server">
        <ContentTemplate>
            <asp:Button ID="Btn_Bin" Style="display: none;" runat="server" OnClick="btnBin_Click" Text="Bin" />
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
                    <li id="Li_Bin"><a href="#Bin" onclick="Li_Tab_Bin()" data-toggle="tab">
                        <i class="fa fa-trash"></i>&nbsp;&nbsp;<asp:Label ID="Label1" runat="server" Text="<%$ Resources:Attendance,Bin %>"></asp:Label></a></li>

                    <li id="Li_New" class="active"><a href="#New" data-toggle="tab">
                        <asp:UpdatePanel ID="Update_Li" runat="server">
                            <ContentTemplate>
                                <i class="fa fa-file"></i>&nbsp;&nbsp;
                                <asp:Label ID="Lbl_Tab_New" runat="server" Text="<%$ Resources:Attendance,New%>"></asp:Label>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </a></li>
                </ul>
                <div class="tab-content">
                    <div class="tab-pane active" id="New">
                        <asp:UpdatePanel ID="Update_New" runat="server">
                            <ContentTemplate>
                                <div class="row">
                                    <div class="col-md-12">
                                        <div class="box box-primary">
                                            <div class="box-body">
                                                <div class="form-group">
                                                    <div class="col-md-6">
                                                        <asp:Label ID="lblOpCateName" runat="server" Text="<%$ Resources:Attendance,Option Category Name %>" />
                                                        <a style="color: Red">*</a>
                                                        <asp:RequiredFieldValidator EnableClientScript="true" Style="float: right;" runat="server" ID="RequiredFieldValidator3" ValidationGroup="Save"
                                                            Display="Dynamic" SetFocusOnError="true" ControlToValidate="txtOpCateName" ErrorMessage="<%$ Resources:Attendance,Enter Option Category Name %>"></asp:RequiredFieldValidator>

                                                        <asp:TextBox ID="txtOpCateName" runat="server" AutoPostBack="true" OnTextChanged="txtOpCateName_TextChanged"
                                                            CssClass="form-control" BackColor="#eeeeee"></asp:TextBox>
                                                        <cc1:AutoCompleteExtender ID="txtOpCateName_AutoCompleteExtender" runat="server"
                                                            DelimiterCharacters="" Enabled="True" ServiceMethod="GetCompletionList" ServicePath=""
                                                            CompletionInterval="100" MinimumPrefixLength="1" CompletionSetCount="1" TargetControlID="txtOpCateName"
                                                            UseContextKey="True" CompletionListCssClass="autoCompleteList" CompletionListItemCssClass="autoCompleteListItem" CompletionListHighlightedItemCssClass="autoCompleteSelectedListItem" OnClientShown="resetPosition">
                                                        </cc1:AutoCompleteExtender>
                                                        <br />
                                                    </div>
                                                    <div class="col-md-6">
                                                        <asp:Label ID="lblOpCateNameLocal" runat="server" Text="<%$ Resources:Attendance,Option Category Name(Local) %>" />
                                                        <asp:TextBox ID="txtlblOpCateNameLocal" runat="server" CssClass="form-control"></asp:TextBox>
                                                        <br />
                                                    </div>
                                                    <div class="col-md-6">
                                                        <asp:CheckBox ID="chkPeramter" runat="server" Text="<%$ Resources:Attendance,Add Detail Size and Calculation for label%>"
                                                            CssClass="form-control" OnCheckedChanged="chkPeramter_CheckedChanged" AutoPostBack="true" />
                                                        <br />
                                                    </div>
                                                    <div class="row" id="pnlPerameter" runat="server" visible="false">
                                                        <div class="col-md-12">
                                                            <div class="box box-primary">
                                                                <div class="box-body">
                                                                    <div class="form-group">
                                                                        <div class="col-md-6">
                                                                            <asp:Label ID="Label2" runat="server" Text="<%$ Resources:Attendance,Material Category (For Price in M2)%>" />
                                                                            <asp:TextBox ID="txtMeterialCategory" runat="server" CssClass="form-control" BackColor="#eeeeee"
                                                                                AutoPostBack="true" OnTextChanged="txtComman_TextChanged"></asp:TextBox>
                                                                            <cc1:AutoCompleteExtender ID="AutoCompleteExtender1" runat="server" DelimiterCharacters=""
                                                                                Enabled="True" ServiceMethod="GetCompletionList1" ServicePath="" CompletionInterval="100"
                                                                                MinimumPrefixLength="1" CompletionSetCount="1" TargetControlID="txtMeterialCategory"
                                                                                UseContextKey="True" CompletionListCssClass="autoCompleteList" CompletionListItemCssClass="autoCompleteListItem" CompletionListHighlightedItemCssClass="autoCompleteSelectedListItem" OnClientShown="resetPosition">
                                                                            </cc1:AutoCompleteExtender>
                                                                            <br />
                                                                        </div>
                                                                        <div class="col-md-6">
                                                                            <asp:Label ID="Label3" runat="server" Text="<%$ Resources:Attendance,Color Category (For Price in M2)%>" />
                                                                            <asp:TextBox ID="txtColorCategory" runat="server" CssClass="form-control" BackColor="#eeeeee"
                                                                                AutoPostBack="true" OnTextChanged="txtComman_TextChanged"></asp:TextBox>
                                                                            <cc1:AutoCompleteExtender ID="AutoCompleteExtender2" runat="server" DelimiterCharacters=""
                                                                                Enabled="True" ServiceMethod="GetCompletionList1" ServicePath="" CompletionInterval="100"
                                                                                MinimumPrefixLength="1" CompletionSetCount="1" TargetControlID="txtColorCategory"
                                                                                UseContextKey="True" CompletionListCssClass="autoCompleteList" CompletionListItemCssClass="autoCompleteListItem" CompletionListHighlightedItemCssClass="autoCompleteSelectedListItem" OnClientShown="resetPosition">
                                                                            </cc1:AutoCompleteExtender>
                                                                            <br />
                                                                        </div>
                                                                        <div class="col-md-6">
                                                                            <asp:Label ID="Label7" runat="server" Text="<%$ Resources:Attendance,Quantity Category (For Qty)%>" />
                                                                            <asp:TextBox ID="txtQuantityCategory" runat="server" CssClass="form-control" BackColor="#eeeeee"
                                                                                AutoPostBack="true" OnTextChanged="txtComman_TextChanged"></asp:TextBox>
                                                                            <cc1:AutoCompleteExtender ID="AutoCompleteExtender3" runat="server" DelimiterCharacters=""
                                                                                Enabled="True" ServiceMethod="GetCompletionList1" ServicePath="" CompletionInterval="100"
                                                                                MinimumPrefixLength="1" CompletionSetCount="1" TargetControlID="txtQuantityCategory"
                                                                                UseContextKey="True" CompletionListCssClass="autoCompleteList" CompletionListItemCssClass="autoCompleteListItem" CompletionListHighlightedItemCssClass="autoCompleteSelectedListItem" OnClientShown="resetPosition">
                                                                            </cc1:AutoCompleteExtender>
                                                                            <br />
                                                                        </div>
                                                                        <div class="col-md-6">
                                                                            <asp:Label ID="Label4" runat="server" Text="<%$ Resources:Attendance,Default Value For usable area M2%>" />
                                                                            <asp:TextBox ID="txtDefaultValue" runat="server" CssClass="form-control"></asp:TextBox>
                                                                            <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender39" runat="server" Enabled="True"
                                                                                TargetControlID="txtDefaultValue" ValidChars="1,2,3,4,5,6,7,8,9,0,.">
                                                                            </cc1:FilteredTextBoxExtender>
                                                                            <br />
                                                                        </div>
                                                                        <div class="col-md-6">
                                                                            <asp:Label ID="Label8" runat="server" Text="<%$ Resources:Attendance,Packing List Category%>" />
                                                                            <asp:TextBox ID="txtPackingCategory" runat="server" CssClass="form-control" BackColor="#eeeeee"
                                                                                AutoPostBack="true" OnTextChanged="txtComman_TextChanged"></asp:TextBox>
                                                                            <cc1:AutoCompleteExtender ID="AutoCompleteExtender4" runat="server" DelimiterCharacters=""
                                                                                Enabled="True" ServiceMethod="GetCompletionList1" ServicePath="" CompletionInterval="100"
                                                                                MinimumPrefixLength="1" CompletionSetCount="1" TargetControlID="txtPackingCategory"
                                                                                UseContextKey="True" CompletionListCssClass="autoCompleteList" CompletionListItemCssClass="autoCompleteListItem" CompletionListHighlightedItemCssClass="autoCompleteSelectedListItem" OnClientShown="resetPosition">
                                                                            </cc1:AutoCompleteExtender>
                                                                            <br />
                                                                        </div>
                                                                        <div class="col-md-6">
                                                                            <asp:Label ID="Label9" runat="server" Text="<%$ Resources:Attendance,Core Category%>" />
                                                                            <asp:TextBox ID="txtCore" runat="server" CssClass="form-control" BackColor="#eeeeee"
                                                                                AutoPostBack="true" OnTextChanged="txtComman_TextChanged"></asp:TextBox>
                                                                            <cc1:AutoCompleteExtender ID="AutoCompleteExtender5" runat="server" DelimiterCharacters=""
                                                                                Enabled="True" ServiceMethod="GetCompletionList1" ServicePath="" CompletionInterval="100"
                                                                                MinimumPrefixLength="1" CompletionSetCount="1" TargetControlID="txtCore"
                                                                                UseContextKey="True" CompletionListCssClass="autoCompleteList" CompletionListItemCssClass="autoCompleteListItem" CompletionListHighlightedItemCssClass="autoCompleteSelectedListItem" OnClientShown="resetPosition">
                                                                            </cc1:AutoCompleteExtender>
                                                                            <br />
                                                                        </div>
                                                                        <div class="col-md-6">
                                                                            <asp:Label ID="Label11" runat="server" Text="<%$ Resources:Attendance,Perforation Category%>" />
                                                                            <asp:TextBox ID="txtPerforation" runat="server" CssClass="form-control" BackColor="#eeeeee"
                                                                                AutoPostBack="true" OnTextChanged="txtComman_TextChanged"></asp:TextBox>
                                                                            <cc1:AutoCompleteExtender ID="AutoCompleteExtender6" runat="server" DelimiterCharacters=""
                                                                                Enabled="True" ServiceMethod="GetCompletionList1" ServicePath="" CompletionInterval="100"
                                                                                MinimumPrefixLength="1" CompletionSetCount="1" TargetControlID="txtPerforation"
                                                                                UseContextKey="True" CompletionListCssClass="autoCompleteList" CompletionListItemCssClass="autoCompleteListItem" CompletionListHighlightedItemCssClass="autoCompleteSelectedListItem" OnClientShown="resetPosition">
                                                                            </cc1:AutoCompleteExtender>
                                                                            <br />
                                                                        </div>
                                                                        <div id="tr1" runat="server" visible="false">
                                                                            <div class="col-md-6">
                                                                                <asp:TextBox ID="txtwidth" runat="server" CssClass="form-control"></asp:TextBox>
                                                                                <cc1:TextBoxWatermarkExtender ID="TextBoxWatermarkExtender2" runat="server" WatermarkText="Width(MM)"
                                                                                    TargetControlID="txtwidth">
                                                                                </cc1:TextBoxWatermarkExtender>
                                                                                <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" runat="server" Enabled="True"
                                                                                    TargetControlID="txtwidth" ValidChars="1,2,3,4,5,6,7,8,9,0,.">
                                                                                </cc1:FilteredTextBoxExtender>
                                                                                <br />
                                                                            </div>
                                                                            <div class="col-md-6">
                                                                                <asp:TextBox ID="txtHeight" runat="server" CssClass="form-control"></asp:TextBox>
                                                                                <cc1:TextBoxWatermarkExtender ID="TextBoxWatermarkExtender1" runat="server" WatermarkText="Height(MM)"
                                                                                    TargetControlID="txtHeight">
                                                                                </cc1:TextBoxWatermarkExtender>
                                                                                <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender2" runat="server" Enabled="True"
                                                                                    TargetControlID="txtHeight" ValidChars="1,2,3,4,5,6,7,8,9,0,.">
                                                                                </cc1:FilteredTextBoxExtender>
                                                                                <br />
                                                                            </div>
                                                                            <div class="col-md-6">
                                                                                <asp:TextBox ID="txtgap" runat="server" CssClass="form-control"></asp:TextBox>
                                                                                <cc1:TextBoxWatermarkExtender ID="TextBoxWatermarkExtender3" runat="server" WatermarkText="gap(MM)"
                                                                                    TargetControlID="txtgap">
                                                                                </cc1:TextBoxWatermarkExtender>
                                                                                <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender3" runat="server" Enabled="True"
                                                                                    TargetControlID="txtgap" ValidChars="1,2,3,4,5,6,7,8,9,0,.">
                                                                                </cc1:FilteredTextBoxExtender>
                                                                                <br />
                                                                            </div>
                                                                            <div class="col-md-6">
                                                                                <asp:ImageButton runat="server" CausesValidation="False" ImageUrl="~/Images/add.png"
                                                                                    ToolTip="<%$ Resources:Attendance,Add %>" ID="btnAdd"
                                                                                    OnClick="btnAdd_Click"></asp:ImageButton>
                                                                                <asp:ImageButton runat="server" CausesValidation="False" ImageUrl="~/Images/Erase.png"
                                                                                    ToolTip="<%$ Resources:Attendance,Delete %>" ID="imgDelete" OnClick="imgDelete_Click"></asp:ImageButton>
                                                                                <br />
                                                                            </div>
                                                                            <div class="col-md-6">
                                                                                <asp:ListBox ID="chkSelectedItems" runat="server" RepeatColumns="1" RepeatDirection="Vertical"
                                                                                    Font-Names="Trebuchet MS" Font-Size="Large" ForeColor="Gray" Width="100%"></asp:ListBox>
                                                                                <br />
                                                                            </div>
                                                                        </div>
                                                                    </div>
                                                                </div>
                                                            </div>
                                                        </div>
                                                    </div>

                                                    <div class="col-md-12" style="text-align: center">
                                                        <asp:Button ID="btnCSave" runat="server" ValidationGroup="Save" Text="<%$ Resources:Attendance,Save %>"
                                                            CssClass="btn btn-success" OnClick="btnCSave_Click" Visible="False" />

                                                        <asp:Button ID="BtnReset" runat="server" Text="<%$ Resources:Attendance,Reset %>"
                                                            CssClass="btn btn-primary" CausesValidation="False" OnClick="BtnReset_Click" />
                                                        <asp:HiddenField ID="editid" runat="server" />
                                                        <br />
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>


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
                                                        <asp:ListItem Text="<%$ Resources:Attendance, Option Category ID %>" Value="OptionCategoryID" />
                                                        <asp:ListItem Text="<%$ Resources:Attendance, Option Category Name %>" Value="EName"
                                                            Selected="True" />
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
                                                <div class="col-lg-3">
                                                    <asp:Panel ID="Panel1" runat="server" DefaultButton="btnbind">
                                                        <asp:TextBox ID="txtValue" runat="server" CssClass="form-control"></asp:TextBox>
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
                                                    <asp:GridView CssClass="table-striped table-bordered table table-hover" ID="GvOptionCategory" PageSize="<%# PageControlCommon.GetPageSize() %>"
                                                        runat="server" AutoGenerateColumns="False" Width="100%" AllowPaging="True" OnPageIndexChanging="GvOptionCategory_PageIndexChanging"
                                                        AllowSorting="True" OnSorting="GvOptionCategory_Sorting">

                                                        <Columns>

                                                            <asp:TemplateField HeaderText="Action">
                                                                <ItemTemplate>
                                                                    <div class="dropdown" style="position: absolute;">
                                                                        <button class="btn btn-default dropdown-toggle"  type="button" data-toggle="dropdown">
                                                                            <i class="fa fa-ellipsis-h" aria-hidden="true"></i>
                                                                        </button>
                                                                        <ul class="dropdown-menu">
                                                                            <li <%= hdnCanEdit.Value=="true"?"style='display:block'":"style='display:none'" %>>
                                                                                <asp:LinkButton ID="btnEdit" runat="server" CommandArgument='<%# Eval("OptionCategoryID") %>' OnCommand="btnEdit_Command" CausesValidation="False"><i class="fa fa-pencil"></i>Edit </asp:LinkButton>
                                                                            </li>
                                                                            <li <%= hdnCanDelete.Value=="true"?"style='display:block'":"style='display:none'" %>>
                                                                                <asp:LinkButton ID="IbtnDelete" runat="server" CausesValidation="False" CommandArgument='<%# Eval("OptionCategoryID") %>' OnCommand="IbtnDelete_Command"><i class="fa fa-trash"></i>Delete</asp:LinkButton>
                                                                                <cc1:ConfirmButtonExtender ID="confirm2" runat="server" ConfirmText="<%$ Resources:Attendance,Are you sure you want to delete the record?%>" TargetControlID="IbtnDelete"></cc1:ConfirmButtonExtender>
                                                                            </li>

                                                                        </ul>
                                                                    </div>

                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:BoundField DataField="OptionCategoryId" HeaderText="<%$ Resources:Attendance,Option Category ID %>"
                                                                SortExpression="OptionCategoryID">
                                                                <ItemStyle />
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="EName" HeaderText="<%$ Resources:Attendance,Option Category Name %>"
                                                                SortExpression="EName"></asp:BoundField>
                                                            <asp:BoundField DataField="LName" HeaderText="<%$ Resources:Attendance,Option Category Name(Local) %>"
                                                                SortExpression="LName">
                                                                <ItemStyle />
                                                            </asp:BoundField>
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
                    <div class="tab-pane" id="Bin">
                        <asp:UpdatePanel ID="Update_Bin" runat="server">
                            <ContentTemplate>

                                <div class="row">
                                    <div class="col-md-12">
                                        <div id="Div2" runat="server" class="box box-info collapsed-box">
                                            <div class="box-header with-border">
                                                <h3 class="box-title">
                                                    <asp:Label ID="Label12" runat="server" Text="Advance Search"></asp:Label></h3>
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
                                                        <asp:ListItem Text="<%$ Resources:Attendance, Option Category ID %>" Value="OptionCategoryID" />
                                                        <asp:ListItem Text="<%$ Resources:Attendance, Option Category Name %>" Value="EName"
                                                            Selected="True" />
                                                        <asp:ListItem Text="<%$ Resources:Attendance,Created By %>" Value="Created_User"></asp:ListItem>
                                                        <asp:ListItem Text="<%$ Resources:Attendance,Modified By %>" Value="Modified_User"></asp:ListItem>
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
                                                    </asp:Panel>
                                                </div>
                                                <div class="col-lg-3">
                                                    <asp:LinkButton ID="btnbindBin" runat="server" CausesValidation="False" OnClick="btnbindBin_Click" ToolTip="<%$ Resources:Attendance,Search %>"><span class="fa fa-search"  style="font-size:25px;"></span></asp:LinkButton>&nbsp;&nbsp;&nbsp;
                                                    <asp:LinkButton ID="btnRefreshBin" runat="server" CausesValidation="False" OnClick="btnRefreshBin_Click" ToolTip="<%$ Resources:Attendance,Refresh %>"><span class="fa fa-repeat"  style="font-size:25px;"></span></asp:LinkButton>&nbsp;&nbsp;&nbsp;
                                                    <asp:LinkButton ID="imgBtnRestore" CausesValidation="False" runat="server" OnClick="btnRestoreSelected_Click" ToolTip="<%$ Resources:Attendance, Active %>"><span class="far fa-lightbulb"  style="font-size:25px;"></span></asp:LinkButton>&nbsp;&nbsp;&nbsp;
                                                    <asp:LinkButton ID="ImgbtnSelectAll" Visible="false" runat="server" OnClick="ImgbtnSelectAll_Click" ToolTip="<%$ Resources:Attendance, Select All %>"><span class="fas fa-th"  style="font-size:25px;"></span></asp:LinkButton>
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
                                                    <asp:Label ID="lblSelectedRecord" runat="server" Visible="false"></asp:Label>
                                                    <asp:GridView CssClass="table-striped table-bordered table table-hover" ID="GvOptionCategoryBin" PageSize="<%# PageControlCommon.GetPageSize() %>"
                                                        runat="server" AutoGenerateColumns="False" Width="100%" AllowPaging="True" OnPageIndexChanging="GvOptionCategoryBin_PageIndexChanging"
                                                        OnSorting="GvOptionCategoryBin_OnSorting" AllowSorting="true">
                                                        <Columns>
                                                            <asp:TemplateField>
                                                                <HeaderTemplate>
                                                                    <asp:CheckBox ID="chkgvSelectAll" runat="server" OnCheckedChanged="chkgvSelectAll_CheckedChanged"
                                                                        AutoPostBack="true" />
                                                                </HeaderTemplate>
                                                                <ItemTemplate>
                                                                    <asp:CheckBox ID="chkgvSelect" runat="server" AutoPostBack="true" OnCheckedChanged="chkgvSelect_CheckedChanged" />
                                                                </ItemTemplate>
                                                                <ItemStyle HorizontalAlign="Center" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,Option Category ID%>" SortExpression="OptionCategoryID">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblgvOptionCategoryId" runat="server" Text='<%# Eval("OptionCategoryID") %>'></asp:Label>
                                                                </ItemTemplate>
                                                                <ItemStyle />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,Option Category Name %>"
                                                                SortExpression="EName">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblgvOptionCategoryName" runat="server" Text='<%# Eval("EName") %>'></asp:Label>
                                                                </ItemTemplate>
                                                                <ItemStyle />
                                                            </asp:TemplateField>
                                                            <asp:BoundField DataField="LName" HeaderText="<%$ Resources:Attendance,Option Category Name(Local) %>"
                                                                SortExpression="LName">
                                                                <ItemStyle />
                                                            </asp:BoundField>
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
        function Li_Tab_Bin() {
            document.getElementById('<%= Btn_Bin.ClientID %>').click();
        }
    </script>
</asp:Content>
