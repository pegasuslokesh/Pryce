<%@ Page Title="" Language="C#" MasterPageFile="~/ERPMaster.master" AutoEventWireup="true" CodeFile="BillOfMaterial.aspx.cs" Inherits="Inventory_BillOfMaterial" %>

<%@ Register Assembly="DevExpress.Web.v18.1, Version=18.1.3.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web" TagPrefix="dx" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="Head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="HeaderContent" runat="Server">
    <h1>
        <i class="fasbtnSave fa-receipt"></i>&nbsp;&nbsp;
        <asp:Label ID="lblHeader" runat="server" Text="<%$ Resources:Attendance,Bill Of Material%>"></asp:Label>
    </h1>
    <ol class="breadcrumb">
        <li><a href="#"><i class="fa fa-dashboard"></i>
            <asp:Label ID="Label6" runat="server" Text="<%$ Resources:Attendance,Inventory%>"></asp:Label></a></li>
        <li><a href="#"><i class="fa fa-dashboard"></i>
            <asp:Label ID="Label10" runat="server" Text="<%$ Resources:Attendance,Inventory%>"></asp:Label></a></li>
        <li class="active">
            <asp:Label ID="Label5" runat="server" Text="<%$ Resources:Attendance,BOM%>"></asp:Label></li>
    </ol>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="Server">
    <asp:UpdatePanel ID="Update_Button" runat="server">
        <ContentTemplate>
            <asp:HiddenField runat="server" ID="hdnCanEdit" />            
            <asp:HiddenField runat="server" ID="hdnCanPrint" />            
            <asp:HiddenField runat="server" ID="hdnCanView" />
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
                        <asp:UpdatePanel ID="Update_List" runat="server">
                            <ContentTemplate>

                                <div class="row">
                                    <div class="col-md-12">
                                        <div id="Div1" runat="server" class="box box-info collapsed-box">
                                            <div class="box-header with-border">
                                                <h3 class="box-title">
                                                    <asp:Label ID="Label22" runat="server" Text="Advance Search"></asp:Label></h3>
                                                &nbsp;&nbsp;|&nbsp;&nbsp;
				<asp:Label ID="lblTotalRecord" runat="server" Font-Bold="true" Text="<%$ Resources:Attendance,Total Records: 0 %>"></asp:Label>
                                                <div class="box-tools pull-right">
                                                    <button type="button" class="btn btn-box-tool" data-widget="collapse">
                                                        <i id="I1" runat="server" class="fa fa-plus"></i>
                                                    </button>
                                                </div>
                                            </div>
                                            <div class="box-body">
                                                <div class="col-lg-3">
                                                    <asp:DropDownList ID="ddlFieldName" runat="server" CssClass="form-control">
                                                        <asp:ListItem Text="<%$ Resources:Attendance,Model No. %>" Value="Model_No"></asp:ListItem>
                                                        <asp:ListItem Text="<%$ Resources:Attendance,Model Name %>" Value="Model_Name" Selected="True"></asp:ListItem>
                                                        <asp:ListItem Text="<%$ Resources:Attendance,Created By %>" Value="Created_User"></asp:ListItem>
                                                        <asp:ListItem Text="<%$ Resources:Attendance,Modified By %>" Value="Modified_User"></asp:ListItem>
                                                    </asp:DropDownList>
                                                </div>
                                                <div class="col-lg-2">
                                                    <asp:DropDownList ID="ddlOption" runat="server" CssClass="form-control">
                                                        <asp:ListItem Text="<%$ Resources:Attendance,--Select-- %>"></asp:ListItem>
                                                        <asp:ListItem Text="<%$ Resources:Attendance,Equal %>"></asp:ListItem>
                                                        <asp:ListItem Text="<%$ Resources:Attendance,Like %>"></asp:ListItem>
                                                        <asp:ListItem Selected="True" Text="<%$ Resources:Attendance,Contains %>"></asp:ListItem>
                                                    </asp:DropDownList>
                                                </div>
                                                <div class="col-lg-4">
                                                    <asp:Panel ID="Panel1" runat="server" DefaultButton="btnbind">
                                                        <asp:TextBox ID="txtValue" runat="server" CssClass="form-control" placeholder="Search From Content" />
                                                    </asp:Panel>
                                                </div>
                                                <div class="col-lg-3">
                                                    <asp:LinkButton ID="btnbind" runat="server" CausesValidation="False" OnClick="btnbind_Click" ToolTip="<%$ Resources:Attendance,Search %>"><span class="fa fa-search"  style="font-size:25px;"></span></asp:LinkButton>&nbsp;&nbsp;&nbsp;
                                                    <asp:LinkButton ID="btnRefresh" runat="server" CausesValidation="False" OnClick="btnRefresh_Click" ToolTip="<%$ Resources:Attendance,Refresh %>"><span class="fa fa-repeat"  style="font-size:25px;"></span></asp:LinkButton>
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
                                                    <asp:HiddenField ID="HDFSort" runat="server" />
                                                    <asp:GridView CssClass="table-striped table-bordered table table-hover" ID="gvModelMaster" PageSize="<%# PageControlCommon.GetPageSize() %>"
                                                        runat="server" AutoGenerateColumns="False" Width="100%" AllowPaging="True" AllowSorting="True"
                                                        OnPageIndexChanging="gvModelMaster_PageIndexChanging" OnSorting="gvModelMaster_OnSorting">
                                                        <Columns>

                                                            <asp:TemplateField HeaderText="Action">
                                                                <ItemTemplate>
                                                                    <div class="dropdown" style="position: absolute;">
                                                                        <button class="btn btn-default dropdown-toggle"  type="button" data-toggle="dropdown">
                                                                            <i class="fa fa-ellipsis-h" aria-hidden="true"></i>
                                                                        </button>
                                                                        <ul class="dropdown-menu">
                                                                            <li <%= hdnCanPrint.Value=="true"?"style='display:block'":"style='display:none'" %>>
                                                                                <asp:LinkButton ID="IbtnPrint" runat="server" CausesValidation="False" CommandArgument='<%# Eval("Trans_Id") %>'  OnCommand="IbtnPrint_Command" ><i class="fa fa-print"></i>Print</asp:LinkButton>
                                                                            </li>
                                                                            <li <%= hdnCanView.Value=="true"?"style='display:block'":"style='display:none'" %>>
                                                                                <asp:LinkButton ID="btnView" runat="server" CommandArgument='<%# Eval("Trans_Id") %>' CausesValidation="False" OnCommand="btnView_Command"><i class="fa fa-eye"></i>View</asp:LinkButton>
                                                                            </li>

                                                                            <li <%= hdnCanEdit.Value=="true"?"style='display:block'":"style='display:none'" %>>
                                                                               <asp:LinkButton ID="btnEdit" runat="server" CommandArgument='<%# Eval("Trans_Id") %>' CausesValidation="False" OnCommand="btnEdit_Command"> <i class="fa fa-pencil"></i>Edit </asp:LinkButton>
                                                                            </li>
                                                                        </ul>
                                                                    </div>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,Model No %>" SortExpression="Model_No">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblModelNo" runat="server" Text='<%# Eval("Model_No") %>'></asp:Label>
                                                                </ItemTemplate>
                                                                <ItemStyle />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,Model Name %>" SortExpression="Model_Name">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblModelName" runat="server" Text='<%# Eval("Model_Name") %>'></asp:Label>
                                                                </ItemTemplate>
                                                                <ItemStyle />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,Model Name(Local) %>" SortExpression="Model_Name_L">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblModelNameL" runat="server" Text='<%# Eval("Model_Name_L") %>'></asp:Label>
                                                                    <asp:Label ID="lblModelCurrency" runat="server" Visible="false" Text='<%# Eval("Field4") %>'></asp:Label>
                                                                </ItemTemplate>
                                                                <ItemStyle />
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
                                                        <asp:Label ID="lblTransType" runat="server" Text="<%$ Resources:Attendance,Type %>"></asp:Label>
                                                        <asp:DropDownList ID="ddlTransType" runat="server" AutoPostBack="true"
                                                            OnSelectedIndexChanged="ddlTransType_SelectedIndexChanged" CssClass="form-control">
                                                            <asp:ListItem Text="<%$ Resources:Attendance,Assemble %>" Selected="True" Value="A"></asp:ListItem>
                                                            <asp:ListItem Text="<%$ Resources:Attendance,Kit %>" Value="K"></asp:ListItem>
                                                        </asp:DropDownList>
                                                        <br />
                                                    </div>
                                                    <div class="col-md-6">
                                                        <asp:Label ID="lblDate" runat="server" Text="<%$ Resources:Attendance,Date %>"></asp:Label>
                                                        <asp:TextBox ID="txtDate" runat="server" CssClass="form-control"></asp:TextBox>
                                                        <cc1:CalendarExtender OnClientShown="showCalendar" ID="CalendarExtender_date" runat="server" TargetControlID="txtDate"
                                                            Format="dd/MM/yyyy" Enabled="True">
                                                        </cc1:CalendarExtender>
                                                        <br />
                                                    </div>
                                                    <div class="col-md-6">
                                                        <asp:Label ID="lblModelNo" runat="server" Text="<%$ Resources:Attendance,Model No %>"></asp:Label>
                                                        <a style="color: Red">*</a>
                                                        <asp:RequiredFieldValidator EnableClientScript="true" Style="float: right;" runat="server" ID="RequiredFieldValidator3" ValidationGroup="Radio"
                                                            Display="Dynamic" SetFocusOnError="true" ControlToValidate="txtModelNo" ErrorMessage="<%$ Resources:Attendance,Enter Model No %>"></asp:RequiredFieldValidator>

                                                        <asp:TextBox ID="txtModelNo" runat="server" BackColor="#eeeeee" CssClass="form-control"
                                                            AutoPostBack="true" OnTextChanged="txtModelNo_TextChanged"></asp:TextBox>
                                                        <cc1:AutoCompleteExtender ID="AutoCompleteExtender1" runat="server" CompletionInterval="100"
                                                            DelimiterCharacters="" Enabled="True" MinimumPrefixLength="1" ServiceMethod="GetCompletionListModelNo"
                                                            ServicePath="" TargetControlID="txtModelNo" UseContextKey="True" CompletionListCssClass="autoCompleteList" CompletionListItemCssClass="autoCompleteListItem" CompletionListHighlightedItemCssClass="autoCompleteSelectedListItem" OnClientShown="resetPosition">
                                                        </cc1:AutoCompleteExtender>
                                                        <br />
                                                    </div>
                                                    <div class="col-md-6">
                                                        <asp:Label ID="lblModelName" runat="server" Text="<%$ Resources:Attendance,Model Name %>"> </asp:Label>
                                                        <a style="color: Red">*</a>
                                                        <asp:RequiredFieldValidator EnableClientScript="true" Style="float: right;" runat="server" ID="RequiredFieldValidator1" ValidationGroup="Radio"
                                                            Display="Dynamic" SetFocusOnError="true" ControlToValidate="txtModelName" ErrorMessage="<%$ Resources:Attendance,Enter Model Name %>"></asp:RequiredFieldValidator>

                                                        <asp:TextBox ID="txtModelName" runat="server" CssClass="form-control" BackColor="#eeeeee"
                                                            AutoPostBack="true" OnTextChanged="txtModelName_TextChanged"></asp:TextBox>
                                                        <cc1:AutoCompleteExtender ID="AutoCompleteExtender3" runat="server" CompletionInterval="100"
                                                            DelimiterCharacters="" Enabled="True" MinimumPrefixLength="1" ServiceMethod="GetCompletionListModelName"
                                                            ServicePath="" TargetControlID="txtModelName" UseContextKey="True" CompletionListCssClass="autoCompleteList" CompletionListItemCssClass="autoCompleteListItem" CompletionListHighlightedItemCssClass="autoCompleteSelectedListItem" OnClientShown="resetPosition">
                                                        </cc1:AutoCompleteExtender>
                                                        <br />
                                                    </div>
                                                    <div class="col-md-12">
                                                        <asp:RadioButton ID="rdoOption" ValidationGroup="Radio" runat="server" Text="<%$ Resources:Attendance,Assemble Item %>"
                                                            GroupName="a" AutoPostBack="True" OnCheckedChanged="rdoStockOption_CheckedChanged" />
                                                        <asp:RadioButton ID="rdoStock" runat="server" Style="margin-left: 20px;" ValidationGroup="Radio" Text="<%$ Resources:Attendance,Inventory Item %>"
                                                            AutoPostBack="True" GroupName="a" OnCheckedChanged="rdoStockOption_CheckedChanged" />
                                                        <br />
                                                        <br />
                                                    </div>
                                                    <div id="pnlOptStock" runat="server">
                                                        <div id="trStock" runat="server">
                                                            <div class="col-md-6">
                                                                <asp:Label ID="lblSupProductId" runat="server" Text="<%$ Resources:Attendance,Sub Product Name %>"></asp:Label>
                                                                <a style="color: Red">*</a>
                                                                <asp:RequiredFieldValidator EnableClientScript="true" Style="float: right;" runat="server" ID="RequiredFieldValidator8" ValidationGroup="Save"
                                                                    Display="Dynamic" SetFocusOnError="true" ControlToValidate="txtSubProduct" ErrorMessage="<%$ Resources:Attendance,Enter Sub Product Name%>"></asp:RequiredFieldValidator>

                                                                <asp:TextBox ID="txtSubProduct" runat="server" BackColor="#eeeeee" CssClass="form-control" AutoPostBack="true" OnTextChanged="txtSubProduct_OnTextChanged"></asp:TextBox>
                                                                <cc1:AutoCompleteExtender ID="AutoCompleteExtender2" runat="server" CompletionInterval="100"
                                                                    DelimiterCharacters="" Enabled="True" MinimumPrefixLength="1" ServiceMethod="GetCompletionListSubProductName"
                                                                    ServicePath="" TargetControlID="txtSubProduct" UseContextKey="True" CompletionListCssClass="autoCompleteList" CompletionListItemCssClass="autoCompleteListItem" CompletionListHighlightedItemCssClass="autoCompleteSelectedListItem" OnClientShown="resetPosition">
                                                                </cc1:AutoCompleteExtender>
                                                                <br />
                                                            </div>
                                                        </div>
                                                        <div class="col-md-6">
                                                            <asp:Label ID="lblOption" runat="server" Text="<%$ Resources:Attendance,Product Option %>"></asp:Label>
                                                            <a style="color: Red">*</a>
                                                            <asp:RequiredFieldValidator EnableClientScript="true" Style="float: right;" runat="server" ID="RequiredFieldValidator2" ValidationGroup="Save"
                                                                Display="Dynamic" SetFocusOnError="true" ControlToValidate="txtOption" ErrorMessage="<%$ Resources:Attendance,Enter Product Option %>"></asp:RequiredFieldValidator>

                                                            <asp:TextBox ID="txtOption" runat="server" MaxLength="1" CssClass="form-control"></asp:TextBox>
                                                            <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender6" runat="server" FilterType="Custom, UppercaseLetters, LowercaseLetters"
                                                                TargetControlID="txtOption" ValidChars="1,2,3,4,5,6,7,8,9" />
                                                            <br />
                                                        </div>
                                                        <div class="col-md-6">
                                                            <asp:Label ID="lblOptCatId" runat="server" Text="<%$ Resources:Attendance,Option Category Name %>"></asp:Label>
                                                            <a style="color: Red">*</a>
                                                            <asp:RequiredFieldValidator EnableClientScript="true" Style="float: right;" runat="server" ID="RequiredFieldValidator4" ValidationGroup="Save"
                                                                Display="Dynamic" SetFocusOnError="true" ControlToValidate="txtOptCatId" ErrorMessage="<%$ Resources:Attendance,Enter Option Category Name%>"></asp:RequiredFieldValidator>

                                                            <asp:TextBox ID="txtOptCatId" BackColor="#eeeeee" runat="server" CssClass="form-control"
                                                                AutoPostBack="true" OnTextChanged="txtOptCatId_TextChanged"></asp:TextBox>
                                                            <cc1:AutoCompleteExtender ID="txtOpCateName_AutoCompleteExtender" runat="server"
                                                                DelimiterCharacters="" Enabled="True" ServiceMethod="GetCompletionList" ServicePath=""
                                                                CompletionInterval="100" MinimumPrefixLength="1" CompletionSetCount="1" TargetControlID="txtOptCatId"
                                                                UseContextKey="True" CompletionListCssClass="autoCompleteList" CompletionListItemCssClass="autoCompleteListItem" CompletionListHighlightedItemCssClass="autoCompleteSelectedListItem" OnClientShown="resetPosition">
                                                            </cc1:AutoCompleteExtender>
                                                            <br />
                                                        </div>
                                                        <div class="col-md-12">
                                                            <asp:Label ID="lblShortOptionDesc" runat="server" Text="<%$ Resources:Attendance,Option Short Description %>"></asp:Label>
                                                            <a style="color: Red">*</a>
                                                            <asp:RequiredFieldValidator EnableClientScript="true" Style="float: right;" runat="server" ID="RequiredFieldValidator5" ValidationGroup="Save"
                                                                Display="Dynamic" SetFocusOnError="true" ControlToValidate="txtShortDesc" ErrorMessage="<%$ Resources:Attendance,Enter Option Short Description%>"></asp:RequiredFieldValidator>

                                                            <asp:TextBox ID="txtShortDesc" runat="server" TextMode="MultiLine" CssClass="form-control"
                                                                Font-Names="Arial"></asp:TextBox>
                                                            <br />
                                                        </div>
                                                        <div class="col-md-12">
                                                            <asp:Label ID="lblOptionDesc" runat="server" Text="<%$ Resources:Attendance,Option Description %>"></asp:Label>
                                                            <asp:TextBox ID="txtOptionDesc" runat="server" TextMode="MultiLine" CssClass="form-control"
                                                                Font-Names="Arial"></asp:TextBox>
                                                            <br />
                                                        </div>
                                                        <div class="col-md-6">
                                                            <asp:Label ID="lblUnitPrice" runat="server" Text="<%$ Resources:Attendance,Unit Price %>"></asp:Label>
                                                            <a style="color: Red">*</a>
                                                            <asp:RequiredFieldValidator EnableClientScript="true" Style="float: right;" runat="server" ID="RequiredFieldValidator6" ValidationGroup="Save"
                                                                Display="Dynamic" SetFocusOnError="true" ControlToValidate="txtUnitPrice" ErrorMessage="<%$ Resources:Attendance,Enter Unit Price%>"></asp:RequiredFieldValidator>

                                                            <asp:TextBox ID="txtUnitPrice" runat="server" CssClass="form-control"></asp:TextBox>
                                                            <cc1:FilteredTextBoxExtender ID="FilteredtxtUnitPrice" runat="server" Enabled="True"
                                                                TargetControlID="txtUnitPrice" ValidChars="1,2,3,4,5,6,7,8,9,0,.">
                                                            </cc1:FilteredTextBoxExtender>
                                                            <br />
                                                        </div>
                                                        <div class="col-md-6">
                                                            <asp:Label ID="lblQty" runat="server" Text="<%$ Resources:Attendance,Quantity %>"></asp:Label>
                                                            <a style="color: Red">*</a>
                                                            <asp:RequiredFieldValidator EnableClientScript="true" Style="float: right;" runat="server" ID="RequiredFieldValidator7" ValidationGroup="Save"
                                                                Display="Dynamic" SetFocusOnError="true" ControlToValidate="txtQty" ErrorMessage="<%$ Resources:Attendance,Enter Quantity%>"></asp:RequiredFieldValidator>

                                                            <asp:TextBox ID="txtQty" runat="server" CssClass="form-control"></asp:TextBox>
                                                            <cc1:FilteredTextBoxExtender ID="FilteredtxtQty" runat="server" Enabled="True" TargetControlID="txtQty"
                                                                ValidChars="1,2,3,4,5,6,7,8,9,0,.">
                                                            </cc1:FilteredTextBoxExtender>
                                                            <br />
                                                        </div>
                                                        <div class="col-md-6">
                                                            <asp:Label ID="Label2" runat="server" Text="<%$ Resources:Attendance,Default %>"></asp:Label>
                                                            <asp:CheckBox ID="chkDefault" runat="server" />
                                                            <br />  <br />
                                                            <asp:Label ID="lblprinter" runat="server" Text="Printer" Visible="false"></asp:Label>
                                                            <asp:DropDownList ID="ddlPrinter" runat="server" CssClass="form-control" Visible="false" />

                                                            <asp:CheckBoxList ID="chkPrinter" runat="server" RepeatColumns="2" RepeatDirection="Horizontal" Font-Names="Trebuchet MS" Font-Size="Large" ForeColor="Gray" Width="100%"></asp:CheckBoxList>
                                                        </div>
                                                        <div class="col-md-6">
                                                            <asp:Label ID="lblFiltertext" runat="server"></asp:Label>
                                                            <div id="pnlChkSupplier" runat="server" borderstyle="Solid"
                                                                borderwidth="1px" bordercolor="#abadb3" backcolor="White" style="overflow: auto">
                                                                <asp:CheckBoxList ID="chkSelectedItems" runat="server" RepeatColumns="2" RepeatDirection="Horizontal" Font-Names="Trebuchet MS" Font-Size="Large" ForeColor="Gray" Width="100%"></asp:CheckBoxList>
                                                            </div>
                                                            <br />
                                                        </div>
                                                        <div class="col-md-12" style="text-align: center">
                                                              <br />
                                                            <asp:Button ID="btnSave" ValidationGroup="Save" runat="server" Text="<%$ Resources:Attendance,Save %>" CssClass="btn btn-success"
                                                                OnClick="btnSave_Click" Visible="False" />
                                                            <asp:Button ID="btnReset" runat="server" Text="<%$ Resources:Attendance,Reset %>"
                                                                CssClass="btn btn-primary" OnClick="btnReset_Click" />
                                                            <asp:HiddenField ID="hdnDetailEdit" runat="server" />
                                                            <br />
                                                        </div>
                                                    </div>
                                                    <div id="pnlChlidGrid" runat="server" visible="false">
                                                        <div class="col-md-12">
                                                            <div style="overflow: auto">
                                                                <asp:GridView CssClass="table-striped table-bordered table table-hover" ID="gvProductSpecsChild" runat="server" CellPadding="4" ForeColor="#333333"
                                                                    AutoGenerateColumns="False" Width="100%" PageSize="<%# PageControlCommon.GetPageSize() %>"
                                                                    AllowPaging="True" OnPageIndexChanging="gvProductSpecsChild_PageIndexChanging">
                                                                    <Columns>
                                                                        <asp:TemplateField HeaderText="<%$ Resources:Attendance,Edit %>">
                                                                            <ItemTemplate>
                                                                                <asp:LinkButton ID="btnDetailEdit" runat="server" CommandArgument='<%# Eval("TransId") %>' OnCommand="btnDetailEdit_Command" ToolTip="<%$ Resources:Attendance,Edit %>"><i class="fa fa-pencil"></i></asp:LinkButton>
                                                                            </ItemTemplate>
                                                                            <ItemStyle HorizontalAlign="Center" />
                                                                        </asp:TemplateField>
                                                                        <asp:TemplateField HeaderText="<%$ Resources:Attendance,Delete %>">
                                                                            <ItemTemplate>
                                                                                <asp:LinkButton ID="IbtnDelete" runat="server" CommandArgument='<%# Eval("TransId") %>' ToolTip="<%$ Resources:Attendance,Delete %>" OnCommand="IbtnDelete_Command"><i class="fa fa-trash"></i></asp:LinkButton>
                                                                            </ItemTemplate>
                                                                            <ItemStyle HorizontalAlign="Center" />
                                                                        </asp:TemplateField>
                                                                        <asp:TemplateField HeaderText="<%$ Resources:Attendance,Id %>">
                                                                            <ItemTemplate>
                                                                                <asp:Label ID="lblOptionId" runat="server" Text='<%# Eval("OptionId") %>'></asp:Label>
                                                                            </ItemTemplate>
                                                                        </asp:TemplateField>
                                                                        <asp:TemplateField HeaderText="<%$ Resources:Attendance,Option Category %>">
                                                                            <ItemTemplate>
                                                                                <asp:Label ID="lblOptionCatId" runat="server" Text='<%# GetOpCateName(Eval("OptionCategoryId").ToString()) %>'></asp:Label>
                                                                            </ItemTemplate>
                                                                            <ItemStyle />
                                                                        </asp:TemplateField>
                                                                        <asp:TemplateField HeaderText="<%$ Resources:Attendance,Unit Price %>">
                                                                            <ItemTemplate>
                                                                                <asp:Label ID="lblUnitPrice" runat="server" Text='<%# SetDecimal(Eval("UnitPrice").ToString()) %>'></asp:Label>
                                                                            </ItemTemplate>
                                                                            <ItemStyle />
                                                                        </asp:TemplateField>
                                                                        <asp:TemplateField HeaderText="<%$ Resources:Attendance,Quantity %>">
                                                                            <ItemTemplate>
                                                                                <asp:Label ID="lblQty" runat="server" Text='<%# SetDecimal(Eval("Quantity").ToString()) %>'></asp:Label>
                                                                            </ItemTemplate>
                                                                            <ItemStyle />
                                                                        </asp:TemplateField>
                                                                        <asp:TemplateField HeaderText="<%$ Resources:Attendance,Sub Product %>">
                                                                            <ItemTemplate>
                                                                                <asp:Label ID="lblSubProdId" runat="server" Text='<%# getProductName(Eval("SubProductId").ToString()) %>'></asp:Label>
                                                                            </ItemTemplate>
                                                                            <ItemStyle />
                                                                        </asp:TemplateField>
                                                                        <asp:TemplateField HeaderText="<%$ Resources:Attendance,Short Description %>">
                                                                            <ItemTemplate>
                                                                                <asp:Label ID="lblShortDesc" runat="server" Text='<%# Eval("ShortDescription") %>'></asp:Label>
                                                                            </ItemTemplate>
                                                                            <ItemStyle />
                                                                        </asp:TemplateField>
                                                                        <asp:TemplateField HeaderText="<%$ Resources:Attendance,Is Default %>">
                                                                            <ItemTemplate>
                                                                                <asp:Label ID="lblDefault" runat="server" Text='<%# Eval("PDefault") %>'></asp:Label>
                                                                            </ItemTemplate>
                                                                            <ItemStyle />
                                                                        </asp:TemplateField>
                                                                    </Columns>


                                                                    <PagerStyle CssClass="pagination-ys" />
                                                                    <RowStyle CssClass="Invgridrow" />
                                                                </asp:GridView>
                                                            </div>
                                                            <br />
                                                        </div>
                                                        <div class="col-md-12" style="text-align: center">
                                                            <br />
                                                            <asp:Button ID="BtnPrint" runat="server" Text="<%$ Resources:Attendance,Print%>"
                                                                CssClass="btn btn-primary" Visible="false" OnClick="btnPrintReport_Click" />
                                                            <asp:Button ID="btnFinalSave" runat="server" Text="<%$ Resources:Attendance,Close %>"
                                                                CssClass="btn btn-primary" OnClick="btnFinalSave_Click" />
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

    <asp:Panel ID="pnlList" runat="server" Visible="false"></asp:Panel>
    <asp:Panel ID="pnlMenuList" runat="server" Visible="false"></asp:Panel>
    <asp:Panel ID="pnlMenuNew" runat="server" Visible="false"></asp:Panel>
    <asp:Panel ID="pnlNew" runat="server" Visible="false"></asp:Panel>
    <asp:Panel ID="Panel4" runat="server" Visible="false"></asp:Panel>
    <asp:Panel ID="Panel5" runat="server" Visible="false"></asp:Panel>
    <asp:Panel ID="Panel6" runat="server" Visible="false"></asp:Panel>
    <asp:Panel ID="Panel7" runat="server" Visible="false"></asp:Panel>

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
    </script>
</asp:Content>
