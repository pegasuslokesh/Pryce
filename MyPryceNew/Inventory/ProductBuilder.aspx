<%@ Page Title="" Language="C#" MasterPageFile="~/ERPMaster.master" AutoEventWireup="true" CodeFile="ProductBuilder.aspx.cs" Inherits="Inventory_ProductBuilder" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="Head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="HeaderContent" runat="Server">
    <h1>
        <i class="fas fa-pallet"></i>&nbsp;&nbsp;
        <asp:Label ID="lblHeader" runat="server" Text="<%$ Resources:Attendance,Product Builder %>"></asp:Label>
    </h1>
    <ol class="breadcrumb">
        <li><a href="#"><i class="fa fa-dashboard"></i>
            <asp:Label ID="Label6" runat="server" Text="<%$ Resources:Attendance,Inventory%>"></asp:Label></a></li>
        <li><a href="#"><i class="fa fa-dashboard"></i>
            <asp:Label ID="Label10" runat="server" Text="<%$ Resources:Attendance,Inventory%>"></asp:Label></a></li>
        <li class="active">
            <asp:Label ID="Label5" runat="server" Text="<%$ Resources:Attendance,Product Builder%>"></asp:Label></li>
    </ol>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="Server">

    <div class="row">
        <div class="col-md-12">
            <div class="nav-tabs-custom">
                <ul class="nav nav-tabs pull-right bg-blue-gradient">
                    <li id="Li_Cat"><a href="#Category" data-toggle="tab">
                        <i class="fa fa-list"></i>&nbsp;&nbsp;<asp:Label ID="Label3" runat="server" Text="<%$ Resources:Attendance,Category %>"></asp:Label></a></li>
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

                <asp:HiddenField runat="server" ID="hdnCanEdit" />
                <asp:HiddenField runat="server" ID="hdnCanPrint" />


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
				<asp:Label ID="lblTotalRecord" Font-Bold="true" runat="server" Text="<%$ Resources:Attendance,Total Records: 0 %>"></asp:Label>

                                                <div class="box-tools pull-right">
                                                    <button type="button" class="btn btn-box-tool" data-widget="collapse">
                                                        <i id="I2" runat="server" class="fa fa-plus"></i>
                                                    </button>
                                                </div>
                                            </div>
                                            <div class="box-body">

                                                <div class="col-md-6">
                                                    <asp:Label ID="lblModelCategory" runat="server" Text="<%$ Resources:Attendance,Model Category %>"></asp:Label>
                                                    <asp:DropDownList ID="ddlcategorysearch" runat="server" CssClass="form-control"></asp:DropDownList>
                                                    <br />
                                                </div>
                                                <div class="col-md-6">
                                                    <asp:Label ID="lblSuppliersearch" runat="server" Text="<%$ Resources:Attendance,Model Supplier %>" />
                                                    <asp:TextBox ID="txtSupplierSearch" runat="server" BackColor="#eeeeee" OnTextChanged="txtSupplierSearch_OnTextChanged"
                                                        AutoPostBack="True" CssClass="form-control"></asp:TextBox>
                                                    <cc1:AutoCompleteExtender ID="AutoCompleteExtender3" runat="server" CompletionInterval="100"
                                                        DelimiterCharacters="" Enabled="True" MinimumPrefixLength="1" ServiceMethod="GetCompletionList_Supplier"
                                                        ServicePath="" TargetControlID="txtSupplierSearch" UseContextKey="True" CompletionListCssClass="autoCompleteList" CompletionListItemCssClass="autoCompleteListItem" CompletionListHighlightedItemCssClass="autoCompleteSelectedListItem" OnClientShown="resetPosition">
                                                    </cc1:AutoCompleteExtender>
                                                    <br />
                                                </div>

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
                                                    <asp:LinkButton ID="btnRefresh" runat="server" CausesValidation="False" OnClick="btnRefresh_Click" ToolTip="<%$ Resources:Attendance,Refresh %>"><span class="fa fa-repeat"  style="font-size:25px;"></span></asp:LinkButton>&nbsp;&nbsp;&nbsp;
                                                    <asp:LinkButton ID="imbBtnGrid" CausesValidation="False" runat="server" OnClick="imbBtnGrid_Click" ToolTip="<%$ Resources:Attendance, Grid View %>"><span class="fa fa-list"  style="font-size:25px;"></span></asp:LinkButton>&nbsp;&nbsp;&nbsp;
                                                    <asp:LinkButton ID="imgBtnDatalist" CausesValidation="False" runat="server" OnClick="imgBtnDatalist_Click" ToolTip="<%$ Resources:Attendance,List View %>" Visible="False"><span class="fa fa-table"  style="font-size:25px;"></span></asp:LinkButton>
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
                                                    <asp:HiddenField ID="hdnTrans_id" runat="server" />
                                                    <asp:DataList ID="dtlistProduct" runat="server" RepeatColumns="2" RepeatDirection="Horizontal" Width="100%">
                                                        <ItemTemplate>
                                                            <div class="col-md-6" style="min-width: 500px;">
                                                                <div class="box box-primary ">

                                                                    <div class="box-footer no-padding">
                                                                        <div class="col-md-12" style="background-color: ghostwhite">
                                                                            <div class="col-md-4">
                                                                                <br />
                                                                                <asp:ImageButton ID="btnEdit" runat="server" OnCommand="btnEdit_Command" CommandArgument='<%# Eval("Trans_Id") %>'
                                                                                    Width="120px" Height="120px" Enabled='<%# hdnCanEdit.Value=="true"?true:false %>' ImageUrl='<%#string.IsNullOrEmpty(Eval("Field2").ToString()) ? "~/Login/Images/PryceLogo.png" :"~/CompanyResource/"+Eval("Company_Id")+"/Model/" +Eval("Field2")%>' />
                                                                            </div>
                                                                            <div class="col-md-8">
                                                                                <br />
                                                                                <table>
                                                                                    <tr>
                                                                                        <td>
                                                                                            <asp:Label ID="lblProductId" Width="100px" runat="server" Text="<%$ Resources:Attendance,Model No %>"></asp:Label>
                                                                                        </td>
                                                                                        <td width="5px">:
                                                                                        </td>
                                                                                        <td>
                                                                                            <asp:Label ID="lbldlProductId" runat="server" Text='<%# Eval("Model_No") %>'></asp:Label>
                                                                                        </td>
                                                                                    </tr>
                                                                                    <tr>
                                                                                        <td>
                                                                                            <asp:Label ID="Label8" runat="server" Text="<%$ Resources:Attendance,Model Name %>"></asp:Label>
                                                                                        </td>
                                                                                        <td>:
                                                                                        </td>
                                                                                        <td>
                                                                                            <asp:LinkButton ID="lbldlProductName" runat="server" Font-Bold="true" ForeColor="#1886b9"
                                                                                                Style="text-decoration: none;" OnCommand="btnEdit_Command"
                                                                                                CommandArgument='<%# Eval("Trans_Id") %>' Text='<%# Eval("Model_Name") %>'
                                                                                                Enabled='<%# hdnCanEdit.Value=="true"?true:false %>'></asp:LinkButton>
                                                                                        </td>
                                                                                    </tr>
                                                                                    <tr>
                                                                                        <td>
                                                                                            <asp:Label ID="lblModelNo" runat="server" Text="<%$ Resources:Attendance,Basic Price %>"></asp:Label>
                                                                                        </td>
                                                                                        <td>:
                                                                                        </td>
                                                                                        <td>
                                                                                            <asp:Label ID="lblBasicPrice" runat="server" Text='<%# GetCurrencySymbol(Eval("Field1").ToString(),Eval("Field4").ToString()) %>'></asp:Label>

                                                                                        </td>
                                                                                    </tr>
                                                                                    <tr>
                                                                                        <td>
                                                                                            <asp:Label ID="Label9" runat="server" Text="<%$ Resources:Attendance,Created By %>"></asp:Label>
                                                                                        </td>
                                                                                        <td>:
                                                                                        </td>
                                                                                        <td>
                                                                                            <asp:Label ID="Label10" runat="server" Text='<%# Eval("Created_User") %>'></asp:Label>
                                                                                        </td>
                                                                                    </tr>
                                                                                    <tr>
                                                                                        <td>
                                                                                            <asp:Label ID="Label11" runat="server" Text="<%$ Resources:Attendance,Modified By %>"></asp:Label>
                                                                                        </td>
                                                                                        <td>:
                                                                                        </td>
                                                                                        <td>
                                                                                            <asp:Label ID="Label12" runat="server" Text='<%# Eval("Modified_User") %>'></asp:Label>
                                                                                        </td>
                                                                                    </tr>
                                                                                </table>
                                                                            </div>
                                                                        </div>
                                                                    </div>
                                                                </div>
                                                            </div>
                                                            <br />
                                                        </ItemTemplate>
                                                    </asp:DataList>
                                                    <asp:GridView CssClass="table-striped table-bordered table table-hover" ID="gvModelMaster" PageSize="<%# PageControlCommon.GetPageSize() %>"
                                                        runat="server" AutoGenerateColumns="False" Width="100%" AllowPaging="True" AllowSorting="True" Visible="false"
                                                        OnPageIndexChanging="gvModelMaster_PageIndexChanging" OnSorting="gvModelMaster_OnSorting">
                                                        <Columns>
                                                            <asp:TemplateField HeaderText="Action">
                                                                <ItemTemplate>
                                                                    <div class="dropdown" style="position: absolute;">
                                                                        <button class="btn btn-default dropdown-toggle" type="button" data-toggle="dropdown">
                                                                            <i class="fa fa-ellipsis-h" aria-hidden="true"></i>
                                                                        </button>
                                                                        <ul class="dropdown-menu">
                                                                            <li <%= hdnCanPrint.Value=="true"?"style='display:block'":"style='display:none'" %>>
                                                                                <asp:LinkButton ID="IbtnPrint" runat="server" CausesValidation="False" CommandArgument='<%# Eval("Trans_Id") %>' OnCommand="IbtnPrint_Command"><i class="fa fa-print"></i>Price List</asp:LinkButton>
                                                                            </li>
                                                                            <li <%= hdnCanEdit.Value=="true"?"style='display:block'":"style='display:none'" %>>
                                                                                <asp:LinkButton ID="btnEdit" runat="server" CommandArgument='<%# Eval("Trans_Id") %>' CausesValidation="False" OnCommand="btnEdit_Command"><i class="fa fa-pencil"></i>Edit </asp:LinkButton>
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
                                                                    <asp:Label ID="lblModelCurrency" Visible="false" runat="server" Text='<%# Eval("Field4") %>'></asp:Label>
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
                                                    <asp:HiddenField ID="hdnModelImage" runat="server" />
                                                    <asp:HiddenField ID="hdnCurrencyId" runat="server" />
                                                    <asp:HiddenField ID="hdnModelDesc" runat="server" />
                                                    <asp:HiddenField ID="hdnModelPrice" runat="server" />
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
                            <Triggers>
                                <asp:PostBackTrigger ControlID="btnExporttoExcel" />
                            </Triggers>
                            <ContentTemplate>
                                <div class="row">
                                    <div class="col-md-12">
                                        <div class="box box-primary">
                                            <div class="box-body">
                                                <div class="form-group">
                                                    <div class="col-md-6">
                                                        <asp:Label ID="lblModelNo" runat="server" Text="<%$ Resources:Attendance,Model No %>"></asp:Label>
                                                        <a style="color: Red">*</a>
                                                        <asp:RequiredFieldValidator EnableClientScript="true" Style="float: right;" runat="server" ID="RequiredFieldValidator3" ValidationGroup="Save"
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
                                                        <asp:RequiredFieldValidator EnableClientScript="true" Style="float: right;" runat="server" ID="RequiredFieldValidator1" ValidationGroup="Save"
                                                            Display="Dynamic" SetFocusOnError="true" ControlToValidate="txtModelName" ErrorMessage="<%$ Resources:Attendance,Enter Model Name %>"></asp:RequiredFieldValidator>

                                                        <asp:TextBox ID="txtModelName" runat="server" CssClass="form-control" AutoPostBack="true"
                                                            BackColor="#eeeeee" OnTextChanged="txtModelName_TextChanged"></asp:TextBox>
                                                        <cc1:AutoCompleteExtender ID="AutoCompleteExtender2" runat="server" CompletionInterval="100"
                                                            DelimiterCharacters="" Enabled="True" MinimumPrefixLength="1" ServiceMethod="GetCompletionListModelName"
                                                            ServicePath="" TargetControlID="txtModelName" UseContextKey="True" CompletionListCssClass="autoCompleteList" CompletionListItemCssClass="autoCompleteListItem" CompletionListHighlightedItemCssClass="autoCompleteSelectedListItem" OnClientShown="resetPosition">
                                                        </cc1:AutoCompleteExtender>
                                                        <br />
                                                    </div>
                                                    <div class="col-md-3" style="display: none">
                                                        <asp:Label ID="lblPartNo" runat="server" Text="<%$ Resources:Attendance,Product Part No%>"> </asp:Label>
                                                        <asp:TextBox ID="txtProductPartNo" runat="server" AutoPostBack="True" CssClass="form-control" Visible="false"
                                                            OnTextChanged="txtProductPartNo_TextChanged"></asp:TextBox>
                                                        <br />
                                                    </div>
                                                    <div class="col-md-6">
                                                        <br />
                                                        <div class="input-group">
                                                            <asp:TextBox ID="txtOptionPartNo" runat="server" CssClass="form-control"
                                                                ReadOnly="true" Visible="false"></asp:TextBox>
                                                            <asp:TextBox ID="txtNewPartNo" runat="server" OnTextChanged="txtNewPartNo_TextChanged" AutoPostBack="true" CssClass="form-control"></asp:TextBox>
                                                            <cc1:AutoCompleteExtender ID="AutoCompleteExtender4" runat="server" CompletionInterval="100"
                                                                DelimiterCharacters="" Enabled="True" MinimumPrefixLength="1" ServiceMethod="GetCompletionListProductCode"
                                                                ServicePath="" TargetControlID="txtNewPartNo" UseContextKey="True" CompletionListCssClass="autoCompleteList" CompletionListItemCssClass="autoCompleteListItem" CompletionListHighlightedItemCssClass="autoCompleteSelectedListItem" OnClientShown="resetPosition">
                                                            </cc1:AutoCompleteExtender>

                                                            <div class="input-group-btn">
                                                                <asp:LinkButton ID="btnCopyPartNo" CssClass="form-control" runat="server" Text="<%$ Resources:Attendance, Copy %>"
                                                                    OnClick="btnCopyPartNo_Click" ToolTip="Copy Part No" />
                                                            </div>
                                                        </div>
                                                        <br />
                                                    </div>
                                                    <div class="col-md-6">
                                                        <asp:Label ID="lblPrice" runat="server" Text="<%$ Resources:Attendance,Price%>"> </asp:Label>
                                                        <asp:TextBox ID="txtPrice" runat="server" CssClass="form-control"></asp:TextBox>
                                                        <br />
                                                    </div>

                                                    <div class="col-md-12">
                                                        <asp:Label ID="lblDes" runat="server" Text="<%$ Resources:Attendance,Description%>"> </asp:Label>
                                                        <asp:TextBox ID="txtDesc" runat="server" CssClass="form-control" TextMode="MultiLine"></asp:TextBox>
                                                        <br />
                                                    </div>

                                                    <div class="col-md-12" style="text-align: center">
                                                        <asp:Button ID="btnReset" runat="server" Text="<%$ Resources:Attendance,Reset %>"
                                                            CssClass="btn btn-primary" OnClick="btnReset_Click" />

                                                        <asp:Button ID="btnCancel" runat="server" Text="<%$ Resources:Attendance,Cancel %>"
                                                            CssClass="btn btn-primary" OnClick="btnCancel_Click" />

                                                        <asp:Button ID="BtnReport" ValidationGroup="Save" runat="server" Text="<%$ Resources:Attendance,Report%>"
                                                            CssClass="btn btn-primary" OnClick="btnReport_Click" />

                                                        <asp:Button ID="btnPriceList" ValidationGroup="Save" runat="server" Text="<%$ Resources:Attendance,Price List%>"
                                                            CssClass="btn btn-primary" OnClick="btnPriceList_Click" />

                                                        <asp:Button ID="btnAddProduct" runat="server" Text="<%$ Resources:Attendance,Add Product%>"
                                                            CssClass="btn btn-primary" OnClick="btnAddProduct_Click" />
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                                <div id="tblgrid" runat="server" visible="false" class="row">
                                    <div class="col-md-12">
                                        <div class="box box-primary">
                                            <div class="box-body">
                                                <div class="form-group">
                                                    <div class="col-md-6">
                                                        <div id="Div_optionCategory" runat="server" class="box box-info collapsed-box">
                                                            <div class="box-header with-border">
                                                                <h3 class="box-title">
                                                                    <asp:Label ID="lbloptionCategory" runat="server" Text="<%$ Resources:Attendance,Option Category %>"></asp:Label></h3>
                                                                <div class="box-tools pull-right">
                                                                    <button type="button" class="btn btn-box-tool" data-widget="collapse">
                                                                        <i id="Btn_Add_Div" runat="server" class="fa fa-plus"></i>
                                                                    </button>
                                                                </div>
                                                            </div>
                                                            <div class="box-body">
                                                                <div class="form-group">
                                                                    <div style="overflow: scroll; height: 300px">
                                                                        <asp:GridView CssClass="table-striped table-bordered table table-hover" ID="gvOptionCategory" runat="server" AutoGenerateColumns="False"
                                                                            OnDataBound="gvOptionCategory_DataBound" Width="100%" ShowHeader="false">
                                                                            <Columns>
                                                                                <asp:TemplateField HeaderText="<%$ Resources:Attendance,Option Category %>" ItemStyle-Width="150px">
                                                                                    <ItemTemplate>
                                                                                        <asp:Label ID="lblOptionCategoryName" Font-Bold="true" runat="server" Text='<%# GetOpCateName(Eval("OptionCategoryId").ToString()) %>'></asp:Label>
                                                                                        <br />
                                                                                        <asp:Label ID="lblOptionCategoryId" runat="server" Visible="false" Text='<%# Eval("OptionCategoryId") %>'></asp:Label>
                                                                                        <asp:RadioButtonList ID="rdoOption" runat="server" Width="100%"
                                                                                            AutoPostBack="True" OnSelectedIndexChanged="rdoOption_SelectedIndexChanged">
                                                                                        </asp:RadioButtonList>
                                                                                    </ItemTemplate>
                                                                                    <ItemStyle />
                                                                                </asp:TemplateField>
                                                                            </Columns>


                                                                            <PagerStyle CssClass="pagination-ys" />

                                                                        </asp:GridView>
                                                                    </div>
                                                                    <br />
                                                                </div>
                                                            </div>
                                                        </div>
                                                    </div>


                                                    <div class="col-md-6">
                                                        <div id="Div_SuggestedPartNo" runat="server" class="box box-info collapsed-box">
                                                            <div class="box-header with-border">
                                                                <h3 class="box-title">
                                                                    <asp:Label ID="Label2" runat="server" Text="<%$ Resources:Attendance,Suggested Part No. %>"></asp:Label></h3>
                                                                <div class="box-tools pull-right">
                                                                    <button type="button" class="btn btn-box-tool" data-widget="collapse">
                                                                        <i id="I1" runat="server" class="fa fa-plus"></i>
                                                                    </button>
                                                                </div>
                                                            </div>
                                                            <div class="box-body">
                                                                <div class="form-group">
                                                                    <div class="col-md-6">
                                                                        <%--  <div style="overflow: scroll; height: 300px; width: 100%;">--%>
                                                                        <asp:GridView CssClass="table-striped table-bordered table table-hover" ID="gvPartNo" ShowHeader="true" runat="server" AutoGenerateColumns="False"
                                                                            Width="100%">
                                                                            <Columns>
                                                                                <asp:TemplateField HeaderText="<%$ Resources:Attendance,Product Part No .%>">
                                                                                    <ItemTemplate>
                                                                                        <asp:LinkButton ID="lblPartNo" Font-Bold="true" runat="server" Text='<%# Eval("PartNo").ToString() %>'
                                                                                            OnClick="lblPartNo_Click"></asp:LinkButton>
                                                                                    </ItemTemplate>
                                                                                    <ItemStyle />
                                                                                </asp:TemplateField>
                                                                                <asp:TemplateField HeaderText="<%$ Resources:Attendance,Stock%>">
                                                                                    <ItemTemplate>
                                                                                        <asp:Label ID="lblstock" Font-Bold="true" runat="server" Text='<%# Eval("Stock").ToString() %>'></asp:Label>
                                                                                    </ItemTemplate>
                                                                                    <ItemStyle />
                                                                                </asp:TemplateField>



                                                                            </Columns>


                                                                            <PagerStyle CssClass="pagination-ys" />

                                                                        </asp:GridView>
                                                                        <%--   </div>--%>
                                                                        <br />
                                                                    </div>
                                                                </div>
                                                            </div>
                                                        </div>
                                                    </div>


                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                                <div id="Div_Add_Product" runat="server" visible="false" class="row">
                                    <div class="col-md-12">
                                        <div class="box box-primary">
                                            <div class="box-body">
                                                <div class="form-group">
                                                    <div class="col-md-12">
                                                        <div style="overflow: auto">
                                                            <asp:GridView CssClass="table-striped table-bordered table table-hover" ID="gvRelatedProduct" runat="server" AllowPaging="false" AutoGenerateColumns="False" Caption="Related Product"
                                                                BorderStyle="Solid" Width="100%">
                                                                <Columns>
                                                                    <asp:TemplateField HeaderText="<%$ Resources:Attendance,Delete %>">
                                                                        <ItemTemplate>
                                                                            <asp:CheckBox ID="chkRelatedProduct" runat="server" AutoPostBack="true" OnCheckedChanged="chkRelatedProduct_OnCheckedChanged" />
                                                                        </ItemTemplate>
                                                                        <ItemStyle HorizontalAlign="Center" Width="10%" />
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="<%$ Resources:Attendance,Category Name %>" SortExpression="CategoryName">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblgvCategoryName" runat="server" Text='<%#Eval("CategoryName") %>' />
                                                                            <asp:Label ID="lblgvCategoryid" runat="server" Text='<%#Eval("CategoryId") %>' Visible="false" />
                                                                        </ItemTemplate>
                                                                        <ItemStyle Width="25%" />
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="<%$ Resources:Attendance,Product Id %>" SortExpression="ProductCode">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblgvProductCode" runat="server" Text='<%#Eval("ProductCode") %>' />
                                                                            <asp:Label ID="lblgvProductid" runat="server" Text='<%#Eval("ProductId") %>' Visible="false"></asp:Label>
                                                                        </ItemTemplate>
                                                                        <ItemStyle Width="20%" />
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="<%$ Resources:Attendance,Product Name %>" SortExpression="ProductName">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblgvProductName" runat="server" Text='<%#Eval("ProductName") %>' />
                                                                        </ItemTemplate>
                                                                        <ItemStyle Width="35%" />
                                                                    </asp:TemplateField>
                                                                </Columns>


                                                                <PagerStyle CssClass="pagination-ys" />

                                                            </asp:GridView>
                                                        </div>
                                                        <br />
                                                    </div>
                                                    <div class="col-md-12">
                                                        <div style="overflow: auto">
                                                            <asp:UpdatePanel runat="server" ID="upProduct" UpdateMode="Conditional">
                                                                <Triggers>
                                                                    <asp:AsyncPostBackTrigger ControlID="btnAddProduct" EventName="Click" />
                                                                    <asp:AsyncPostBackTrigger ControlID="btnAddToItemList" EventName="Click" />
                                                                </Triggers>
                                                                <ContentTemplate>
                                                                    <asp:GridView CssClass="table-striped table-bordered table table-hover" ID="gvProduct" runat="server" AllowPaging="false" AutoGenerateColumns="False"
                                                                        Width="100%" DataKeyNames="ProductId" PageSize="<%# PageControlCommon.GetPageSize() %>">
                                                                        <Columns>
                                                                            <asp:TemplateField HeaderText="S.No">
                                                                                <ItemTemplate>
                                                                                    <asp:Label ID="lblSerialno" runat="server" Text='<%#Container.DataItemIndex+1 %>'></asp:Label>
                                                                                </ItemTemplate>
                                                                                <ItemStyle HorizontalAlign="Center" Width="4%" />
                                                                                <ItemStyle />
                                                                            </asp:TemplateField>
                                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,Delete %>">
                                                                                <ItemTemplate>
                                                                                    <asp:ImageButton ID="IbtnDelete" runat="server" CausesValidation="False" CommandArgument='<%# Eval("Trans_Id") %>'
                                                                                        ImageUrl="~/Images/Erase.png" OnCommand="IbtnDelete_Command" ToolTip="<%$ Resources:Attendance,Delete %>" />
                                                                                </ItemTemplate>
                                                                                <ItemStyle HorizontalAlign="Center" Width="10%" />
                                                                                <ItemStyle />
                                                                            </asp:TemplateField>

                                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,image %>">
                                                                                <ItemTemplate>
                                                                                    <asp:Image runat="server" Height="50px" Width="50px" ImageUrl='<%# getProductImage(Eval("ProductId").ToString())%>' ID="imgProduct" />
                                                                                </ItemTemplate>
                                                                                <ItemStyle HorizontalAlign="Center" Width="10%" />
                                                                                <ItemStyle />
                                                                            </asp:TemplateField>

                                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,Product Id %>">
                                                                                <ItemTemplate>
                                                                                    <asp:Label ID="lblProductCode" runat="server" Text='<%# Eval("ProductCode") %>'></asp:Label>
                                                                                    <asp:HiddenField ID="hdnProductid" runat="server" Value='<%# Eval("ProductId") %>' />
                                                                                    <asp:HiddenField ID="hdnunitId" runat="server" Value='<%# Eval("UnitId") %>' />
                                                                                    <asp:Label ID="lblDesc" Visible="false" runat="server" Text='<%# Eval("ProductDescription") %>' />
                                                                                    <asp:Label ID="lblProductName" Visible="false" runat="server" Text='<%# Eval("SuggestedProductName") %>' />
                                                                                </ItemTemplate>
                                                                                <ItemStyle Width="10%" />
                                                                            </asp:TemplateField>
                                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,Product Name %>">
                                                                                <ItemTemplate>
                                                                                    <asp:Label ID="lblProductName1" runat="server" Text='<%# Eval("ProductDescription") %>'></asp:Label>
                                                                                </ItemTemplate>
                                                                                <ItemStyle Width="60" />
                                                                            </asp:TemplateField>
                                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,Unit Price %>">
                                                                                <ItemTemplate>
                                                                                    <asp:Label ID="lblUnitPrice" runat="server" Text='<%# Eval("EstimatedUnitPrice") %>'></asp:Label>
                                                                                </ItemTemplate>
                                                                                <ItemStyle Width="10%" />
                                                                            </asp:TemplateField>
                                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,Quantity %>">
                                                                                <ItemTemplate>
                                                                                    <asp:Label ID="lblQuantity" runat="server" Text='<%# Eval("Quantity") %>'></asp:Label>
                                                                                </ItemTemplate>
                                                                                <ItemStyle Width="10%" HorizontalAlign="Right" />
                                                                            </asp:TemplateField>
                                                                        </Columns>
                                                                        <PagerStyle CssClass="pagination-ys" />
                                                                    </asp:GridView>
                                                                </ContentTemplate>
                                                            </asp:UpdatePanel>

                                                        </div>
                                                        <br />
                                                    </div>
                                                    <div class="col-md-12" style="text-align: center">
                                                        <asp:Button ID="btnTranferInquotaion" runat="server" Text="Transfer in Quotation" Visible="false" CssClass="btn btn-primary" OnClick="btnTranferInquotaion_Click" />
                                                        <asp:Button ID="btnExporttoExcel" runat="server" Text="Export To Excel" Visible="false" CssClass="btn btn-primary" OnClick="btnExporttoExcel_Click" />
                                                        <asp:Button ID="btnSaveItemToProduct" runat="server" Text="Save Products" CssClass="btn btn-primary" OnClick="btnSaveItemToProduct_Click" />
                                                        <asp:Button ID="btnClose" runat="server" Text="Close" CssClass="btn btn-primary" OnClientClick="javascript:window.close();" />
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </div>
                    <div class="tab-pane" id="Category">
                        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                            <ContentTemplate>
                                <div class="row">
                                    <div class="col-md-12">
                                        <div class="box box-primary">
                                            <div class="box-body">
                                                <div class="form-group">
                                                    <div class="col-md-6">
                                                        <asp:Label runat="server" ID="lblCategory" Text="Category List"></asp:Label>
                                                        <asp:DropDownList runat="server" ID="ddlModelCategory" CssClass="form-control" OnSelectedIndexChanged="ddlModelCategory_SelectedIndexChanged" AutoPostBack="true"></asp:DropDownList>
                                                        <br />
                                                    </div>
                                                    <div class="col-md-6">
                                                        <br />
                                                        <asp:Button runat="server" ID="btnAddToItemList" CssClass="btn btn-primary" Text="Add Items to List" OnClick="btnAddToItemList_Click" />
                                                        <br />
                                                    </div>
                                                    <div class="col-md-12" style="overflow: auto">

                                                        <asp:GridView CssClass="table-striped table-bordered table table-hover" runat="server" ID="gvCategoryProduct" AutoGenerateColumns="false" Width="100%">
                                                            <Columns>
                                                                <asp:TemplateField>
                                                                    <ItemTemplate>
                                                                        <asp:HiddenField runat="server" ID="gvhdnProductId" Value='<%# Eval("ProductId") %>' />
                                                                        <asp:HiddenField runat="server" ID="gvhdnItemType" Value='<%# Eval("ItemType") %>' />
                                                                        <asp:CheckBox runat="server" ID="chkProductSelect" />
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="Product Code">
                                                                    <ItemTemplate>
                                                                        <asp:Label runat="server" ID="gvlblProductCode" Text='<%# Eval("ProductCode") %>'></asp:Label>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="Product Name">
                                                                    <ItemTemplate>
                                                                        <asp:Label runat="server" ID="gvlblProductName" Text='<%# Eval("EProductName") %>'></asp:Label>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="Description" Visible="false">
                                                                    <ItemTemplate>
                                                                        <asp:Label runat="server" ID="gvlblProductDescription" Text='<%# Eval("Description") %>'></asp:Label>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="Price">
                                                                    <ItemTemplate>
                                                                        <asp:Label runat="server" ID="gvlblProductPrice" Text='<%# Eval("SalesPrice1") %>'></asp:Label>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="Unit">
                                                                    <ItemTemplate>
                                                                        <asp:HiddenField runat="server" ID="gvhdnUnitId" Value='<%# Eval("Unitid") %>' />
                                                                        <asp:Label runat="server" ID="gvlblUnitName" Text='<%# Eval("Unit_Name") %>'></asp:Label>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="Stock">
                                                                    <ItemTemplate>
                                                                        <asp:Label runat="server" ID="gvlblStock" Text='<%# Eval("Quantity") %>'></asp:Label>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                            </Columns>
                                                        </asp:GridView>
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


    <asp:UpdateProgress ID="UpdateProgress1" runat="server" AssociatedUpdatePanelID="UpdatePanel1">
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

    <asp:UpdateProgress ID="UpdateProgress3" runat="server" AssociatedUpdatePanelID="Update_Li">
        <ProgressTemplate>
            <div class="modal_Progress">
                <div class="center_Progress">
                    <img alt="" src="../Images/ajax-loader2.gif" style="vertical-align: middle; background-color: White; border-radius: 10px;" />
                </div>
            </div>
        </ProgressTemplate>
    </asp:UpdateProgress>


    <asp:UpdateProgress ID="UpdateProgress4" runat="server" AssociatedUpdatePanelID="Update_List">
        <ProgressTemplate>
            <div class="modal_Progress">
                <div class="center_Progress">
                    <img alt="" src="../Images/ajax-loader2.gif" style="vertical-align: middle; background-color: White; border-radius: 10px;" />
                </div>
            </div>
        </ProgressTemplate>
    </asp:UpdateProgress>

    <asp:UpdateProgress ID="UpdateProgress5" runat="server" AssociatedUpdatePanelID="upProduct">
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

            $("#Li_Cat").removeClass("active");
            $("#Category").removeClass("active");
        }
        function LI_List_Active() {
            $("#Li_List").addClass("active");
            $("#List").addClass("active");

            $("#Li_New").removeClass("active");
            $("#New").removeClass("active");
        }
    </script>
</asp:Content>

