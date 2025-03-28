<%@ Page Language="C#" AutoEventWireup="true" CodeFile="AddContact.aspx.cs" Inherits="Sales_Add_Contact" %>

<%@ Register Src="~/WebUserControl/AddressControl.ascx" TagName="Addressmaster" TagPrefix="UC" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<!DOCTYPE html>
<html>
<head>
    <meta charset="utf-8">
    <meta http-equiv="X-UA-Compatible" content="IE=edge">
    <title>Add Item</title>
    <link rel="SHORTCUT ICON" href="../Images/favicon.ico" />
    <meta content="width=device-width, initial-scale=1, maximum-scale=1, user-scalable=no" name="viewport">
    <link href="../Bootstrap_Files/Additional/Ajax_Tab.css" rel="stylesheet" type="text/css" />
    <link href="../Bootstrap_Files/Additional/Button_Style.css" rel="stylesheet" type="text/css" />
    <link href="../Bootstrap_Files/Additional/Popup_Style.css" rel="stylesheet" type="text/css" />

    <link href="../Bootstrap_Files/bootstrap/css/bootstrap.min.css" rel="stylesheet" />
    <link href="../Bootstrap_Files/font-awesome-4.7.0/font-awesome-4.7.0/css/font-awesome.min.css" rel="stylesheet" />
    <link href="../Bootstrap_Files/ionicons-2.0.1/ionicons-2.0.1/css/ionicons.min.css" rel="stylesheet" />
    <link href="../Bootstrap_Files/dist/css/AdminLTE.min.css" rel="stylesheet" />
    <link href="../Bootstrap_Files/dist/css/skins/_all-skins.min.css" rel="stylesheet" />
</head>
<body class="hold-transition skin-blue sidebar-mini">
    <form id="Form" runat="server">
        <asp:ScriptManager ID="ScriptManager" runat="server"></asp:ScriptManager>
        <div class="wrapper">
            <div class="content-wrapper" style="margin-left: 0px;">
                <section class="content">
                    <div class="box box-primary box-solid">
                        <div class="box-header with-border">
                            <h3 class="box-title">
                                <img src="../Images/contact_master.png" width="31" height="30" alt="D" />
                                <asp:Label ID="lblHeader" runat="server" Text="<%$ Resources:Attendance,Contact Setup %>" CssClass="LableHeaderTitle"></asp:Label></h3>
                        </div>
                        <div class="box-body">
                            <asp:UpdatePanel ID="Update_Button" runat="server">
                                <ContentTemplate>
                                    <asp:Button ID="Btn_Address_Modal" Style="display: none;" runat="server" data-toggle="modal" data-target="#myModal" Text="Address" />
                                    <asp:Button ID="Btn_Number" Style="display: none;" data-toggle="modal" data-target="#NumberData" runat="server" Text="Number Data" />
                                </ContentTemplate>
                            </asp:UpdatePanel>
                            <asp:UpdatePanel ID="Update_List" runat="server">
                                <ContentTemplate>
                                    <div class="row">
                                        <div class="col-md-12">
                                            <div class="box box-primary">
                                                <div class="box-body">
                                                    <div class="form-group">
                                                        <asp:HiddenField ID="hdntxtaddressid" runat="server" />
                                                        <asp:HiddenField ID="hdnContactId" runat="server" />
                                                        <asp:HiddenField ID="hdnCompId" runat="server" />
                                                        <asp:HiddenField ID="HiddenField6" runat="server" />
                                                        <div class="col-md-12" style="text-align: center">
                                                            <asp:RadioButtonList ID="RdolistSelect" runat="server" RepeatDirection="Horizontal"
                                                                CellSpacing="0" CellPadding="0" AutoPostBack="true" OnSelectedIndexChanged="RdolistSelect_SelectedIndexChanged">
                                                                <asp:ListItem Selected="True" Text="Individual" Value="Individual">
                                                                </asp:ListItem>
                                                                <asp:ListItem style="margin-left: 10px; margin-right: 10px;" Text="Company" Value="Company">
                                                                </asp:ListItem>
                                                            </asp:RadioButtonList>
                                                            <br />
                                                        </div>
                                                        <div class="col-md-6">
                                                            <asp:Label ID="lblId" runat="server" Text="<%$Resources:Attendance,Id %>"></asp:Label>
                                                            <asp:TextBox ID="txtId" runat="server" CssClass="form-control" BackColor="#eeeeee"
                                                                AutoPostBack="true" OnTextChanged="txtId_TextChanged"></asp:TextBox>
                                                            <cc1:AutoCompleteExtender ID="AutoCompleteExtender5" runat="server" DelimiterCharacters=""
                                                                Enabled="True" ServiceMethod="GetCompletionListContactId" ServicePath="" CompletionInterval="100"
                                                                MinimumPrefixLength="1" CompletionSetCount="1" TargetControlID="txtId" UseContextKey="True"
                                                                CompletionListCssClass="autoCompleteList" CompletionListItemCssClass="autoCompleteListItem" CompletionListHighlightedItemCssClass="autoCompleteSelectedListItem" OnClientShown="resetPosition">
                                                            </cc1:AutoCompleteExtender>
                                                            <br />
                                                        </div>
                                                        <div class="col-md-6">
                                                            <asp:Label ID="lblName" runat="server" Text="<%$Resources:Attendance, Name %>"></asp:Label>

                                                            <div style="width: 100%" class="input-group">
                                                                <asp:DropDownList ID="ddlNamePrefix" runat="server" Style="width: 30%" class="form-control">
                                                                    <asp:ListItem Text="<%$ Resources:Attendance,Mr. %>" Selected="True" Value="Mr."></asp:ListItem>
                                                                    <asp:ListItem Text="<%$ Resources:Attendance,Miss %>" Value="Miss"></asp:ListItem>
                                                                    <asp:ListItem Text="<%$ Resources:Attendance,Mrs. %>" Value="Mrs."></asp:ListItem>
                                                                    <asp:ListItem Text="<%$ Resources:Attendance,Dr. %>" Value="Dr."></asp:ListItem>
                                                                </asp:DropDownList>
                                                                <asp:TextBox ID="txtName" Style="max-width: 100%; width: 70%; min-width: 70%;" runat="server" Class="form-control"></asp:TextBox>
                                                            </div>

                                                            <br />
                                                        </div>
                                                        <div class="col-md-6">
                                                            <asp:Label ID="lblNameL" runat="server" Text="<%$Resources:Attendance, Name (Local) %>"></asp:Label>
                                                            <asp:TextBox ID="txtNameL" runat="server" CssClass="form-control"></asp:TextBox>
                                                            <br />
                                                        </div>
                                                        <div class="col-md-6">
                                                            <asp:Label ID="lblAddressName" runat="server" Text="<%$ Resources:Attendance,Address Name %>"></asp:Label>
                                                            <asp:RequiredFieldValidator EnableClientScript="true" Style="float: right;" runat="server" ID="RequiredFieldValidator3" ValidationGroup="Address"
                                                                Display="Dynamic" SetFocusOnError="true" ControlToValidate="txtAddressName" ErrorMessage="<%$ Resources:Attendance,Enter Address Name %>" />
                                                            <div class="input-group">
                                                                <asp:TextBox ID="txtAddressName" AutoPostBack="true" BackColor="#eeeeee" OnTextChanged="txtAddressName_TextChanged"
                                                                    runat="server" CssClass="form-control"></asp:TextBox>
                                                                <cc1:AutoCompleteExtender ID="AutoCompleteExtender2" runat="server" CompletionInterval="100"
                                                                    CompletionSetCount="1" DelimiterCharacters="" Enabled="True" MinimumPrefixLength="1"
                                                                    ServiceMethod="GetCompletionListAddressName" ServicePath="" TargetControlID="txtAddressName"
                                                                    UseContextKey="True" CompletionListCssClass="autoCompleteList" CompletionListItemCssClass="autoCompleteListItem" CompletionListHighlightedItemCssClass="autoCompleteSelectedListItem" OnClientShown="resetPosition">
                                                                </cc1:AutoCompleteExtender>
                                                                <div class="input-group-btn">
                                                                    <asp:Button ID="imgAddAddressName" ValidationGroup="Address" Style="margin-left: 10px;" Visible="false"
                                                                        OnClick="imgAddAddressName_Click"
                                                                        CssClass="btn btn-info" runat="server" Text="<%$ Resources:Attendance,Add %>" />
                                                                    <asp:Button ID="btnAddNewAddress" Style="margin-left: 10px;"
                                                                        Visible="false" OnClick="btnAddNewAddress_Click" CssClass="btn btn-info"
                                                                        runat="server" Text="<%$ Resources:Attendance,New %>" />
                                                                    <asp:HiddenField ID="Hdn_Address_ID" runat="server" />
                                                                    <asp:HiddenField ID="hdnAddressId" runat="server" />
                                                                    <asp:HiddenField ID="hdnAddressName" runat="server" />
                                                                </div>
                                                            </div>
                                                            <br />
                                                        </div>
                                                        <div class="col-md-12">
                                                            <div style="overflow: auto">
                                                                <asp:GridView CssClass="table-striped table-bordered table table-hover" ID="GvAddressName" runat="server" AutoGenerateColumns="False" Width="100%">

                                                                    <Columns>
                                                                        <asp:TemplateField HeaderText="Edit">
                                                                            <ItemTemplate>
                                                                                <asp:ImageButton ID="imgBtnAddressEdit" runat="server" CommandArgument='<%# Eval("Trans_Id") %>'
                                                                                    ImageUrl="~/Images/edit.png" Width="16px" OnCommand="btnAddressEdit_Command" />
                                                                            </ItemTemplate>
                                                                            <ItemStyle HorizontalAlign="Center" />
                                                                        </asp:TemplateField>
                                                                        <asp:TemplateField HeaderText="Delete">
                                                                            <ItemTemplate>
                                                                                <asp:ImageButton ID="imgBtnDelete" runat="server" CommandArgument='<%# Eval("Trans_Id") %>'
                                                                                    Height="16px" ImageUrl="~/Images/Erase.png" Width="16px" OnCommand="btnAddressDelete_Command" />
                                                                            </ItemTemplate>
                                                                            <ItemStyle HorizontalAlign="Center" />
                                                                        </asp:TemplateField>
                                                                        <asp:TemplateField HeaderText="<%$ Resources:Attendance,S No. %>">
                                                                            <ItemTemplate>
                                                                                <asp:Label ID="lblSNo" Width="40px" runat="server" Text='<%#Eval("Trans_Id") %>' />
                                                                            </ItemTemplate>
                                                                            <ItemStyle HorizontalAlign="Center" Width="40px" />
                                                                        </asp:TemplateField>
                                                                        <asp:TemplateField HeaderText="<%$ Resources:Attendance, Address Name %>">
                                                                            <ItemTemplate>
                                                                                <asp:Label ID="lblgvAddressName" Width="100px" runat="server" Text='<%#Eval("Address_Name") %>' />
                                                                            </ItemTemplate>
                                                                            <ItemStyle HorizontalAlign="Center" Width="100px" />
                                                                        </asp:TemplateField>
                                                                        <asp:TemplateField HeaderText="<%$ Resources:Attendance, Address %>">
                                                                            <ItemTemplate>
                                                                                <asp:Label ID="lblgvAddress" runat="server" Text='<%# Eval("Address") %>' />
                                                                            </ItemTemplate>
                                                                            <ItemStyle HorizontalAlign="Center" />
                                                                        </asp:TemplateField>
                                                                        <asp:TemplateField HeaderText="<%$ Resources:Attendance,EmailId %>">
                                                                            <ItemTemplate>
                                                                                <asp:Label ID="lblgvEmailId" runat="server" CssClass="labelComman" Text='<%#GetContactEmailId(Eval("Address_Name").ToString()) %>' />
                                                                            </ItemTemplate>
                                                                            <ItemStyle HorizontalAlign="Center" Width="100px" />
                                                                        </asp:TemplateField>
                                                                        <asp:TemplateField HeaderText="<%$ Resources:Attendance,FaxNo. %>">
                                                                            <ItemTemplate>
                                                                                <asp:Label ID="lblgvFaxNo" Width="80px" runat="server" CssClass="labelComman" Text='<%#GetContactFaxNo(Eval("Address_Name").ToString()) %>' />
                                                                            </ItemTemplate>
                                                                            <ItemStyle HorizontalAlign="Center" Width="80px" />
                                                                        </asp:TemplateField>
                                                                        <asp:TemplateField HeaderText="<%$ Resources:Attendance,PhoneNo.%>">
                                                                            <ItemTemplate>
                                                                                <asp:Label ID="lblgvPhoneNo" runat="server" CssClass="labelComman" Text='<%#GetContactPhoneNo(Eval("Address_Name").ToString()) %>' />
                                                                            </ItemTemplate>
                                                                            <ItemStyle HorizontalAlign="Center" Width="100px" />
                                                                        </asp:TemplateField>
                                                                        <asp:TemplateField HeaderText="<%$ Resources:Attendance,MobileNo.%>">
                                                                            <ItemTemplate>
                                                                                <asp:Label ID="lblgvMobileNo" runat="server" CssClass="labelComman" Text='<%#GetContactMobileNo(Eval("Address_Name").ToString()) %>' />
                                                                                <asp:LinkButton ID="BtnMoreNumber" runat="server" OnCommand="BtnMoreNumber_Command" CommandArgument='<%#Eval("Address_Name") %>' Text='<%#IsVisible(Eval("Address_Name").ToString())%>' />
                                                                            </ItemTemplate>
                                                                            <ItemStyle HorizontalAlign="Center" Width="100px" />
                                                                        </asp:TemplateField>
                                                                        <asp:TemplateField HeaderText="<%$ Resources:Attendance,Is Default%>">
                                                                            <ItemTemplate>
                                                                                <asp:CheckBox ID="chkdefault" runat="server" Checked='<%#Eval("Is_Default") %>' AutoPostBack="true"
                                                                                    OnCheckedChanged="chkgvSelect_CheckedChangedDefault" />
                                                                            </ItemTemplate>
                                                                            <ItemStyle HorizontalAlign="Center" Width="100px" />
                                                                        </asp:TemplateField>
                                                                    </Columns>

                                                                    <PagerStyle CssClass="pagination-ys" />

                                                                </asp:GridView>
                                                            </div>
                                                        </div>
                                                        <div class="col-md-6">
                                                            <asp:Label ID="lblPermanentMobileNo" runat="server" Text="<%$ Resources:Attendance,Permanent MobileNo.%>" />
                                                            <div style="width: 100%" class="input-group">
                                                                <asp:DropDownList ID="ddlCountryCode" Style="width: 30%;" runat="server" CssClass="form-control"></asp:DropDownList>
                                                                <asp:TextBox ID="txtPermanentMobileNo" Style="width: 70%;" runat="server" CssClass="form-control" />
                                                                <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" runat="server" Enabled="True"
                                                                    TargetControlID="txtPermanentMobileNo" FilterType="Numbers">
                                                                </cc1:FilteredTextBoxExtender>
                                                            </div>
                                                            <br />
                                                        </div>
                                                        <div class="col-md-6">
                                                            <asp:Label ID="lblPermanentEMailId" runat="server" Text="<%$ Resources:Attendance,Permanent EmailId%>" />
                                                            <div class="input-group">
                                                                <asp:TextBox ID="txtPermanentMailId" runat="server" CssClass="form-control" BackColor="#eeeeee" />
                                                                <%--<cc1:AutoCompleteExtender ID="AutoCompleteExtender1" runat="server" DelimiterCharacters=""
                                                                    Enabled="True" ServiceMethod="GetCompletionListEmailMaster" ServicePath="" CompletionInterval="100"
                                                                    MinimumPrefixLength="1" CompletionSetCount="1" TargetControlID="txtPermanentMailId" UseContextKey="True"
                                                                    CompletionListCssClass="completionList" CompletionListItemCssClass="listItem"
                                                                    CompletionListHighlightedItemCssClass="itemHighlighted">
                                                                </cc1:AutoCompleteExtender>--%>
                                                                <cc1:AutoCompleteExtender ID="AutoCompleteExtender1" runat="server" CompletionInterval="100"
                                                                    CompletionSetCount="1" DelimiterCharacters="" Enabled="True" MinimumPrefixLength="1"
                                                                    ServiceMethod="GetCompletionListEmailMaster" ServicePath="" TargetControlID="txtPermanentMailId"
                                                                    UseContextKey="True" CompletionListCssClass="autoCompleteList" CompletionListItemCssClass="autoCompleteListItem" CompletionListHighlightedItemCssClass="autoCompleteSelectedListItem" OnClientShown="resetPosition">
                                                                </cc1:AutoCompleteExtender>
                                                                <div class="input-group-btn">
                                                                    <asp:ImageButton ID="btnAddEmail" runat="server" CausesValidation="False" Width="33" Height="33"
                                                                        ImageUrl="~/Images/add.png" OnClick="btnAddEmail_Click" ToolTip="<%$ Resources:Attendance,Add %>" ImageAlign="Middle" />
                                                                    &nbsp;&nbsp;<asp:ImageButton ID="btnRemoveEmail" runat="server" CausesValidation="False" ImageUrl="~/Images/Erase.png" OnClick="btnRemoveEmail_Click" ToolTip="<%$ Resources:Attendance,Delete %>"
                                                                        Visible="false" ImageAlign="Middle" />
                                                                </div>
                                                            </div>
                                                            <br />
                                                        </div>
                                                        <div class="col-md-6">
                                                            <asp:RadioButtonList ID="rbnEmailList" runat="server"></asp:RadioButtonList>
                                                        </div>
                                                        <div class="col-md-6">
                                                            <asp:Label ID="lblCurrency" runat="server" Text="<%$ Resources:Attendance,Currency Name %>"></asp:Label>
                                                            <a style="color: Red">*</a>
                                                            <asp:RequiredFieldValidator EnableClientScript="true" Style="float: right;" runat="server"
                                                                ID="RequiredFieldValidator4" ValidationGroup="a" Display="Dynamic"
                                                                SetFocusOnError="true" ControlToValidate="ddlCurrency" InitialValue="--Select--"
                                                                ErrorMessage="<%$ Resources:Attendance,Select Currency Name %>" />
                                                            <asp:DropDownList ID="ddlCurrency" TabIndex="9" runat="server" Class="form-control" />
                                                            <br />
                                                        </div>
                                                        <div class="col-md-12" id="TbRefContact" runat="server">

                                                            <div class="col-md-6">
                                                                <asp:Label ID="Label8" runat="server" Text="<%$ Resources:Attendance,Country Name%>" />
                                                                <asp:TextBox ID="txtCountryName" runat="server" CssClass="form-control" OnTextChanged="txtCountryName_TextChanged" AutoPostBack="true" BackColor="#eeeeee" />
                                                                <cc1:AutoCompleteExtender ID="AutoCompleteExtendercountry" runat="server" DelimiterCharacters=""
                                                                    Enabled="True" ServiceMethod="GetCompletionListCountryName" ServicePath="" CompletionInterval="100"
                                                                    MinimumPrefixLength="1" CompletionSetCount="1" TargetControlID="txtCountryName"
                                                                    UseContextKey="True" CompletionListCssClass="completionList" CompletionListItemCssClass="listItem"
                                                                    CompletionListHighlightedItemCssClass="itemHighlighted">
                                                                </cc1:AutoCompleteExtender>
                                                                <asp:HiddenField ID="hdncountryid" runat="server" />
                                                                <br />
                                                            </div>
                                                            <div class="col-md-12">
                                                                <asp:Label ID="lblCompanyName" runat="server" Text="<%$Resources:Attendance,Company Name %>"></asp:Label>
                                                                <asp:TextBox ID="txtCompany" BackColor="#eeeeee" runat="server" CssClass="form-control"
                                                                    AutoPostBack="true" OnTextChanged="txtCompany_TextChanged"></asp:TextBox>
                                                                <cc1:AutoCompleteExtender ID="AutoCompleteExtender3" runat="server" DelimiterCharacters=""
                                                                    Enabled="True" CompletionInterval="100" CompletionSetCount="1" MinimumPrefixLength="1"
                                                                    ServiceMethod="GetCompletionListComapnyName" ServicePath="" TargetControlID="txtCompany"
                                                                    UseContextKey="True" CompletionListCssClass="autoCompleteList" CompletionListItemCssClass="autoCompleteListItem" CompletionListHighlightedItemCssClass="autoCompleteSelectedListItem" OnClientShown="resetPosition">
                                                                </cc1:AutoCompleteExtender>
                                                                <br />
                                                            </div>
                                                            <div class="col-md-6">
                                                                <asp:Label ID="lblDepartment" runat="server" Text="<%$ Resources:Attendance,Department %>"></asp:Label>
                                                                <asp:DropDownList ID="ddlDepartment" runat="server" CssClass="form-control" />
                                                                <br />
                                                            </div>
                                                            <div class="col-md-6">
                                                                <asp:Label ID="lblDesignation" runat="server" Text="<%$ Resources:Attendance,Designation %>" />
                                                                <asp:DropDownList ID="ddlDesignation" runat="server" CssClass="form-control" />
                                                                <br />
                                                            </div>
                                                            <div class="col-md-6">
                                                                <asp:Label ID="lblReligion" runat="server" Text="<%$ Resources:Attendance,Religion %>" />
                                                                <asp:DropDownList ID="ddlReligion" runat="server" CssClass="form-control" />
                                                                <br />
                                                            </div>
                                                            <div class="col-md-6">
                                                                <asp:Label ID="lblCivilId" runat="server" Text="<%$ Resources:Attendance,Civil Id %>" />
                                                                <asp:TextBox ID="txtCivilId" runat="server" CssClass="form-control" />
                                                                <br />
                                                            </div>
                                                            <div class="col-md-12"></div>
                                                            <div id="tr_tincst" runat="server" visible="false">
                                                                <div class="col-md-6">
                                                                    <asp:Label ID="Label3" runat="server" Text="TIN No." />
                                                                    <asp:TextBox ID="txtTinno" runat="server" CssClass="form-control" />
                                                                    <br />
                                                                </div>
                                                                <div class="col-md-6">
                                                                    <asp:Label ID="Label4" runat="server" Text="CST No." />
                                                                    <asp:TextBox ID="txtCstNo" runat="server" CssClass="form-control" />
                                                                    <br />
                                                                </div>
                                                            </div>
                                                        </div>
                                                        <div class="col-md-12" style="text-align: center">
                                                            <asp:CheckBox ID="chkIsReseller" runat="server" Text="<%$ Resources:Attendance,Is Reseller %>" />
                                                            <asp:CheckBox ID="chkIsEmail" runat="server" Text="<%$ Resources:Attendance,Send Email %>" />
                                                            <asp:CheckBox ID="chkIsSMS" runat="server" Text="<%$ Resources:Attendance,Send SMS %>" />
                                                            <asp:CheckBox ID="chkVerify" runat="server" Text="Verify" Visible="true" />
                                                            <br />
                                                        </div>
                                                        <div class="col-md-12" style="text-align: center;">
                                                            <br />
                                                        </div>
                                                        <div class="col-md-12" style="text-align: center;">
                                                            <asp:Button ID="btnsave" ValidationGroup="a" runat="server" Text="<%$ Resources:Attendance,Save %>"
                                                                CssClass="btn btn-success" OnClick="btnSave_Click" />
                                                            <asp:Button ID="btnReset" runat="server" Text="<%$ Resources:Attendance,Reset %>"
                                                                CssClass="btn btn-primary" OnClick="btnReset_Click" />
                                                            <br />
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </ContentTemplate>
                            </asp:UpdatePanel>

                            <div class="modal fade" id="myModal" tabindex="-1" role="dialog" aria-labelledby="myModalLabel"
                                aria-hidden="true">
                                <div class="modal-dialog modal-lg">
                                    <div class="modal-content">
                                        <div class="modal-header">
                                            <button type="button" class="close" data-dismiss="modal">
                                                <span aria-hidden="true">&times;</span><span class="sr-only">Close</span></button>
                                            <h4 class="modal-title" id="myModalLabel">New Address</h4>
                                        </div>
                                        <div class="modal-body">
                                            <UC:Addressmaster ID="addaddress" runat="server" />
                                        </div>
                                        <div class="modal-footer">
                                            <button type="button" id="" class="btn btn-danger" data-dismiss="modal">
                                                Close</button>
                                        </div>
                                    </div>
                                </div>
                            </div>


                            <div class="modal fade" id="NumberData" tabindex="-1" role="dialog" aria-labelledby="NumberData_ModalLabel" aria-hidden="true">
                                <div class="modal-dialog modal-mg">
                                    <div class="modal-content">
                                        <div class="modal-header">
                                            <button type="button" class="close" data-dismiss="modal">
                                                <span aria-hidden="true">&times;</span><span class="sr-only">Close</span></button>
                                            <h4 class="modal-title" id="myNumbers">All Number List:</h4>
                                        </div>
                                        <div class="modal-body">
                                            <div id="AllNumberData">
                                            </div>
                                        </div>
                                        <div class="modal-footer">
                                            <button type="button" id="" class="btn btn-danger" data-dismiss="modal">
                                                Close</button>
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
                        </div>
                    </div>
                </section>
            </div>
            <footer class="main-footer" style="margin-left: 0px;">
                <asp:Literal ID="Ltr_Footer_Content" runat="server"></asp:Literal>
            </footer>

            <div class="control-sidebar-bg"></div>
        </div>
        <asp:Panel ID="pnlbasic" runat="server" Visible="false"></asp:Panel>
        <asp:Panel ID="pnlAddress1" runat="server" Visible="false"></asp:Panel>
        <asp:Panel ID="pnlAddress2" runat="server" Visible="false"></asp:Panel>
        <asp:Panel ID="Panel3" runat="server" Visible="false"></asp:Panel>
    </form>
    <script src="../Bootstrap_Files/plugins/jQuery/jquery-2.2.3.min.js"></script>
    <script src="../Bootstrap_Files/plugins/jQuery/jquery-2.2.3.min.js"></script>
    <script src="../Bootstrap_Files/bootstrap/js/bootstrap.min.js"></script>
    <script src="../Bootstrap_Files/dist/js/app.min.js"></script>
    <script>

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

        function Modal_Address_Open() {
            document.getElementById('<%= Btn_Address_Modal.ClientID %>').click();
        }
        function Modal_Address_Close() {
            document.getElementById('<%= Btn_Address_Modal.ClientID %>').click();
            document.getElementById('<%= imgAddAddressName.ClientID %>').click();
        }
        function Modal_Number_Open(data) {
            document.getElementById('AllNumberData').innerHTML = data;
            document.getElementById('<%= Btn_Number.ClientID %>').click();
        }
        function validation1() {

            var valIsCheck = document.getElementById('<%= addaddress.FindControl("ContactNo").FindControl("ddlContactType").ClientID %>').value;

            if (valIsCheck == "LandLine") {
                document.getElementById('<%=addaddress.FindControl("ContactNo").FindControl("txtExtensionNumber").ClientID%>').disabled = false;
            }
            else {
                document.getElementById('<%=addaddress.FindControl("ContactNo").FindControl("txtExtensionNumber").ClientID%>').disabled = true;
                document.getElementById('<%=addaddress.FindControl("ContactNo").FindControl("txtExtensionNumber").ClientID%>').value = "";
            }
        }

    </script>
</body>
</html>
