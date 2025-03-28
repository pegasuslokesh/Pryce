<%@ Page Title="" Language="C#" MasterPageFile="~/ERPMaster.master" AutoEventWireup="true" CodeFile="SupplierMaster.aspx.cs" Inherits="MasterSetUp_SupplierMaster" %>

<%@ Register Src="~/WebUserControl/AddressControl.ascx" TagName="Addressmaster" TagPrefix="UC" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Src="~/WebUserControl/AccountMaster.ascx" TagPrefix="uc1" TagName="AccountMaster" %>
<%@ Register Src="~/WebUserControl/ucControlsSetting.ascx" TagName="ucCtlSetting" TagPrefix="UC" %>
<%@ Register Src="~/WebUserControl/FileUpload.ascx" TagPrefix="AT1" TagName="FileUpload1" %>
<%@ Register Src="~/WebUserControl/ContactInfo.ascx" TagName="ViewContact" TagPrefix="ucContact" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Head" runat="Server">
    <script type="text/javascript">
        function isNumber(evt) {
            evt = (evt) ? evt : window.event;
            var charCode = (evt.which) ? evt.which : evt.keyCode;
            if (charCode > 31 && (charCode < 48 || charCode > 57)) {
                return false;
            }
            return true;
        }
    </script>
    <script type="text/javascript">
        var lasttab = 0;
        function tabChanged(sender, args) {
            // do what ever i want with lastTab value
            lasttab = sender.get_activeTabIndex();
        }
        function resetPosition1() {

        }
    </script>
    <script type="text/javascript">
        function NumberFloatAndOneDOTSign(CurrentElement) {
            var charCode = (event.which) ? event.which : event.keyCode;

            if (charCode != 46 && charCode > 31 && (charCode < 48 || charCode > 57))
                return false;

            //if dot sign entered more than once then don't allow to enter dot sign again. 46 is the code for dot sign
            if (charCode == 46 && (elementRef.value.indexOf('.') >= 0))
                return false;
            else if (charCode == 46 && (elementRef.value.indexOf('.') <= 0)) //allow to enter dot sign if not entered.
                return true;

            return true;
        } </script>
    <script type="text/javascript" src="http://maps.google.com/maps/api/js?sensor=false"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="HeaderContent" runat="Server">
    <h1>
        <i class="fas fa-people-carry"></i>&nbsp;&nbsp;
        <asp:Label ID="lblHeader" runat="server" Text="<%$ Resources:Attendance,Supplier Master%>"></asp:Label>
    </h1>
    <ol class="breadcrumb">
        <li><a href="#"><i class="fa fa-dashboard"></i>
            <asp:Label ID="Label6" runat="server" Text="<%$ Resources:Attendance,Master Setup%>"></asp:Label></a></li>
        <li><a href="#"><i class="fa fa-dashboard"></i>
            <asp:Label ID="Label10" runat="server" Text="<%$ Resources:Attendance,Master SetUp%>"></asp:Label></a></li>
        <li class="active">
            <asp:Label ID="Label5" runat="server" Text="<%$ Resources:Attendance,Supplier Master%>"></asp:Label></li>
    </ol>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="Server">
    <asp:UpdatePanel ID="Update_Button" runat="server">
        <ContentTemplate>

            <asp:Button ID="Btn_Bin" Style="display: none;" runat="server" OnClick="btnBin_Click" Text="Bin" />
            <asp:Button ID="Btn_Add_Address" Style="display: none;" data-toggle="modal" data-target="#Modal_Address" runat="server" Text="Address Details" />
            <asp:Button ID="Btn_Bank_Account" Style="display: none;" data-toggle="modal" data-target="#Modal_Bank" runat="server" Text="Bank Details" />
            <asp:Button ID="Btn_Product_Details" Style="display: none;" data-toggle="modal" data-target="#Modal_Product" runat="server" Text="Product Details" />
            <asp:Button ID="Btn_Number" Style="display: none;" data-toggle="modal" data-target="#NumberData" runat="server" Text="Number Data" />
            <asp:Button ID="Btn_ucAcMaster" Style="display: none;" runat="server" data-toggle="modal" data-target="#ModelAcMaster" Text="Add Accounts Detail" />
            <asp:Button ID="Btn_Modal_FileUpload" Style="display: none;" runat="server" data-toggle="modal" data-target="#Fileupload123" Text="FileUpload" />
            <asp:HiddenField runat="server" ID="hdnCanView" />
            <asp:HiddenField runat="server" ID="hdnCanEdit" />
            <asp:HiddenField runat="server" ID="hdnCanDelete" />
            <asp:HiddenField runat="server" ID="hdnCanUpload" />
        </ContentTemplate>
    </asp:UpdatePanel>
    <div class="row">
        <div class="col-md-12">
            <div class="nav-tabs-custom">
                <ul class="nav nav-tabs pull-right bg-blue-gradient">
                    <li id="Li_Bin"><a onclick="Li_Tab_Bin()" href="#Bin" data-toggle="tab">
                        <i class="fa fa-trash"></i>&nbsp;&nbsp;<asp:Label ID="Label82" runat="server" Text="<%$ Resources:Attendance,Bin%>"></asp:Label></a></li>
                     <li id="Li_Import"><a href="#import" data-toggle="tab">
                        <i class="fa fa-upload"></i>&nbsp;&nbsp;<asp:Label ID="Label7" runat="server" Text="Upload Ob"></asp:Label></a></li>
                    <li style="display: none;" id="Li_New"><a href="#New" data-toggle="tab">
                        <asp:UpdatePanel ID="Update_Li" runat="server">
                            <ContentTemplate>
                                <i class="fa fa-file"></i>&nbsp;&nbsp;
                                <asp:Label ID="Lbl_Tab_New" runat="server" Text="<%$ Resources:Attendance,Edit%>"></asp:Label>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </a></li>
                    <li id="Li_List" class="active"><a href="#List" data-toggle="tab">
                        <i class="fa fa-list"></i>&nbsp;&nbsp;<asp:Label ID="Label165" runat="server" Text="<%$ Resources:Attendance,List %>"></asp:Label></a></li>
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
                                                    <asp:Label ID="Label1" runat="server" Text="Advance Search"></asp:Label></h3>
                                                &nbsp;&nbsp;|&nbsp;&nbsp;
					<asp:Label ID="lblTotalRecords" Text="<%$ Resources:Attendance,Total Records: 0 %>"
                        runat="server"></asp:Label>

                                                <div class="box-tools pull-right">
                                                    <button type="button" class="btn btn-box-tool" data-widget="collapse">
                                                        <i id="I1" runat="server" class="fa fa-plus"></i>
                                                    </button>
                                                </div>
                                            </div>
                                            <div class="box-body">

                                                <div class="col-md-6">
                                                    <asp:Label ID="lblBrandsearch" runat="server" Text="<%$ Resources:Attendance,Supplier Type %>"></asp:Label>
                                                    <asp:DropDownList ID="ddlGroupSearch" runat="server" class="form-control">
                                                    </asp:DropDownList>

                                                </div>
                                                <div class="col-md-6">
                                                    <br />
                                                    <asp:LinkButton ID="btngo" runat="server" CausesValidation="False" OnClick="btngo_Click" ToolTip="<%$ Resources:Attendance,Search %>"><span class="fa fa-search"  style="font-size:25px;"></span></asp:LinkButton>&nbsp;&nbsp;&nbsp;
                                                        <asp:LinkButton ID="btnResetSreach" runat="server" CausesValidation="False" OnClick="btnResetSreach_Click" ToolTip="<%$ Resources:Attendance,Reset %>"><span class="fa fa-repeat"  style="font-size:25px;"></span></asp:LinkButton>

                                                </div>
                                                <div class="col-lg-12">
                                                    <br />
                                                </div>

                                                <div class="col-lg-2">
                                                    <asp:DropDownList ID="ddlCreditStatus" runat="server" CssClass="form-control" AutoPostBack="true"
                                                        OnSelectedIndexChanged="ddlPosted_SelectedIndexChanged">
                                                        <asp:ListItem Text="<%$ Resources:Attendance,All %>" Value="0" Selected="True"></asp:ListItem>
                                                        <asp:ListItem Text="<%$ Resources:Attendance,Credit %>" Value="True"></asp:ListItem>
                                                        <asp:ListItem Text="<%$ Resources:Attendance,Cash %>" Value="False"></asp:ListItem>
                                                    </asp:DropDownList>
                                                </div>
                                                <div class="col-lg-2">
                                                    <asp:DropDownList ID="ddlFieldName" runat="server" CssClass="form-control">
                                                        <asp:ListItem Text="<%$ Resources:Attendance, Supplier Code %>" Value="Supplier_Code"></asp:ListItem>
                                                        <asp:ListItem Text="<%$ Resources:Attendance, Supplier Name %>" Value="Name" Selected="True"></asp:ListItem>
                                                        <asp:ListItem Text="<%$ Resources:Attendance, Supplier Name(Local) %>" Value="Name_L"></asp:ListItem>
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
                                                        <asp:TextBox ID="txtValue" runat="server" CssClass="form-control"></asp:TextBox>
                                                    </asp:Panel>
                                                </div>
                                                <div class="col-lg-2" style="text-align: center; padding-left: 1px; padding-right: 1px;">
                                                    <asp:LinkButton ID="btnbind" runat="server" CausesValidation="False" OnClick="btnbindrpt_Click" ToolTip="<%$ Resources:Attendance,Search %>"><span class="fa fa-search"  style="font-size:25px;"></span></asp:LinkButton>&nbsp;&nbsp;&nbsp;
                                                    <asp:LinkButton ID="btnRefresh" runat="server" CausesValidation="False" OnClick="btnRefreshReport_Click" ToolTip="<%$ Resources:Attendance,Refresh %>"><span class="fa fa-repeat"  style="font-size:25px;"></span></asp:LinkButton>&nbsp;&nbsp;&nbsp;
                                                    <asp:LinkButton ID="btnGvListSetting" ToolTip="List Settings" runat="server" OnClick="btnGvListSetting_Click" Visible="false"><span class="fa fa-wrench"  style="font-size:25px;"></span></asp:LinkButton>
                                                </div>

                                                <asp:HiddenField ID="hdntxtaddressid" runat="server" />

                                            </div>
                                        </div>
                                    </div>
                                </div>


                                <div class="box box-warning box-solid" <%= GvSupplier.Rows.Count>0?"style='display:block'":"style='display:none'"%>>
                                    <div class="box-body">
                                        <div class="row">
                                            <div class="col-md-12">
                                                <div style="overflow: auto">
                                                    <asp:GridView CssClass="table-striped table-bordered table table-hover" ID="GvSupplier" Style="width: 100%;" PageSize="<%# PageControlCommon.GetPageSize() %>" runat="server"
                                                        AutoGenerateColumns="False" AllowPaging="True" OnPageIndexChanging="GvSupplier_PageIndexChanging"
                                                        AllowSorting="True" OnSorting="GvSupplier_Sorting">

                                                        <Columns>

                                                            <asp:TemplateField HeaderText="Action">
                                                                <ItemTemplate>
                                                                    <div class="dropdown" style="position: absolute;">
                                                                        <button class="btn btn-default dropdown-toggle" type="button" data-toggle="dropdown">
                                                                            <i class="fa fa-ellipsis-h" aria-hidden="true"></i>
                                                                        </button>
                                                                        <ul class="dropdown-menu">
                                                                            <li <%= hdnCanView.Value=="true"?"style='display:block'":"style='display:none'" %>>
                                                                                <asp:LinkButton ID="imgBtnDetail" runat="server" CommandArgument='<%# Eval("Supplier_Id") %>' OnCommand="lnkViewDetail_Command"><i class="fa fa-eye"></i>View</asp:LinkButton>
                                                                            </li>

                                                                            <li <%= hdnCanEdit.Value=="true"?"style='display:block'":"style='display:none'" %>>
                                                                                <asp:LinkButton ID="btnEdit" runat="server" CommandArgument='<%# Eval("Supplier_Id") %>' OnCommand="btnEdit_Command" CausesValidation="False"><i class="fa fa-pencil"></i>Edit </asp:LinkButton>
                                                                            </li>
                                                                            <li <%= hdnCanDelete.Value=="true"?"style='display:block'":"style='display:none'" %>>
                                                                                <asp:LinkButton ID="IbtnDelete" runat="server" CausesValidation="False" CommandArgument='<%# Eval("Supplier_Id") %>' OnCommand="IbtnDelete_Command"><i class="fa fa-trash"></i>Delete</asp:LinkButton>
                                                                                <cc1:ConfirmButtonExtender ID="confirm1" runat="server" ConfirmText="<%$ Resources:Attendance,Are you sure you want to delete the record?%>" TargetControlID="IbtnDelete"></cc1:ConfirmButtonExtender>
                                                                            </li>
                                                                            <li <%= hdnCanUpload.Value=="true"?"style='display:block'":"style='display:none'" %>>
                                                                                <asp:LinkButton ID="lbtnFileUpload" runat="server" CommandArgument='<%# Eval("Supplier_Id") %>' CommandName='<%# Eval("Supplier_Code") %>' OnCommand="lbtnFileUpload_Command" CausesValidation="False"><i class="fa fa-upload"></i>File Upload</asp:LinkButton>
                                                                            </li>
                                                                        </ul>
                                                                    </div>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>

                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,Supplier Code %>">
                                                                <ItemTemplate>
                                                                    <asp:LinkButton ID="lnkAccountMaster" Text='<%# Eval("Supplier_Code") %>' runat="server" CommandArgument='<%# Eval("Supplier_Id") %>'
                                                                        OnCommand="lnkAccountMaster_Command" ToolTip="Account Master" />
                                                                </ItemTemplate>
                                                                <ItemStyle HorizontalAlign="Center" />
                                                            </asp:TemplateField>

                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,Supplier Name %>">
                                                                <ItemTemplate>
                                                                    <asp:LinkButton ID="lnkContactDetail" Text='<%# Eval("Name") %>' runat="server" CommandArgument='<%# Eval("Supplier_Id") %>'
                                                                        OnCommand="lnkContactDetail_Command" ToolTip="Contact Detail" />
                                                                </ItemTemplate>
                                                                <ItemStyle HorizontalAlign="Center" />
                                                            </asp:TemplateField>

                                                            <asp:BoundField DataField="Name_L" HeaderText="<%$ Resources:Attendance,Supplier Name(Local) %>"
                                                                SortExpression="Name_L" />
                                                            <asp:BoundField DataField="ISCredit" HeaderText="Is Credit" SortExpression="ISCredit" />
                                                            <asp:BoundField DataField="ApprovalStatus" HeaderText="<%$ Resources:Attendance,Status %>"
                                                                SortExpression="ApprovalStatus" />
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
                                                    <div class="col-md-12">
                                                        <asp:ImageButton ID="btnControlsSetting" ImageAlign="Right" ToolTip="Product List Setting" runat="server" ImageUrl="~/Images/setting.png" OnClick="btnControlsSetting_Click" Style="width: 32px; height: 32px" Visible="false" />
                                                    </div>
                                                    <div class="col-md-12">
                                                        <div class="col-md-6">
                                                            <asp:Label ID="lblLocationCode" runat="server" Text="<%$Resources:Attendance,Supplier Id %>"></asp:Label><a
                                                                style="color: Red"> *</a>
                                                            <asp:TextBox ID="txtId" TabIndex="1" runat="server" CssClass="form-control"></asp:TextBox>
                                                            <br />
                                                            <span id="ctlLSupplierName" runat="server">
                                                                <asp:Label ID="lblLocationName" runat="server" Text="<%$ Resources:Attendance,Supplier Name(Local) %>"></asp:Label>
                                                                <asp:TextBox ID="txtLSupplierName" TabIndex="3" runat="server" CssClass="form-control"></asp:TextBox>
                                                                <br />
                                                            </span>
                                                        </div>
                                                        <div class="col-md-6">
                                                            <asp:Label ID="lblLocationLogo" runat="server" Text="<%$ Resources:Attendance,Supplier Name %>"></asp:Label>
                                                            <asp:TextBox ID="txtSupplierName" TabIndex="2" runat="server" CssClass="form-control" BackColor="#eeeeee"></asp:TextBox>
                                                            <cc1:AutoCompleteExtender ID="txtSupplierName_AutoCompleteExtender" runat="server"
                                                                DelimiterCharacters="" Enabled="True" CompletionInterval="100" CompletionSetCount="1"
                                                                MinimumPrefixLength="1" ServiceMethod="GetCompletionList" ServicePath="" TargetControlID="txtSupplierName"
                                                                UseContextKey="True" CompletionListCssClass="autoCompleteList" CompletionListItemCssClass="autoCompleteListItem" CompletionListHighlightedItemCssClass="autoCompleteSelectedListItem" OnClientShown="resetPosition">
                                                            </cc1:AutoCompleteExtender>
                                                            <br />
                                                            <span id="ctlCivilId" runat="server">
                                                                <asp:Label ID="lblCivilId" runat="server" Text="<%$ Resources:Attendance,Civil Id %>" />
                                                                <asp:TextBox ID="txtCivilId" TabIndex="4" runat="server" CssClass="form-control" />
                                                                <br />
                                                            </span>
                                                        </div>
                                                    </div>
                                                    <div id="trlogo" runat="server" visible="false" class="col-md-12">
                                                        <asp:Label ID="lblUploadImage" runat="server" Text="<%$ Resources:Attendance,Upload Image %>"></asp:Label>

                                                        <asp:FileUpload ID="FileUploadImage" runat="server" />
                                                        <asp:ImageButton ID="ImgFileUploadAdd" runat="server" CausesValidation="False"
                                                            ImageUrl="~/Images/add.png" OnClick="ImgLogoAdd_Click" ToolTip="<%$ Resources:Attendance,Add %>"
                                                            ImageAlign="Middle" />

                                                    </div>
                                                    <div class="col-md-12">
                                                        <div class="col-md-6">
                                                            <asp:GridView CssClass="table-striped table-bordered table table-hover" ID="gvLogo" Width="100%" PageSize="5" runat="server" AutoGenerateColumns="False">
                                                                <Columns>
                                                                    <asp:TemplateField HeaderText="<%$ Resources:Attendance,Delete %>">
                                                                        <ItemTemplate>
                                                                            <asp:ImageButton ID="IbtnDeleteLogo" runat="server" CausesValidation="False" CommandArgument='<%# Eval("Trans_Id") %>'
                                                                                ImageUrl="~/Images/Erase.png" OnCommand="IbtnDeleteLogo_Command" ToolTip="<%$ Resources:Attendance,Delete %>" />
                                                                            <asp:Label runat="server" ID="lblLogoId" Text='<%# Eval("Trans_Id") %>' Visible="false"></asp:Label>
                                                                        </ItemTemplate>
                                                                        <ItemStyle HorizontalAlign="Center" />
                                                                        <ItemStyle />
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="<%$ Resources:Attendance,Image%>" ItemStyle-HorizontalAlign="Center">
                                                                        <ItemTemplate>
                                                                            <asp:ImageButton ID="ImgLogo" runat="server" CausesValidation="False" ImageUrl='<%# getImageUrl(Eval("ImagePath")) %>' />
                                                                        </ItemTemplate>
                                                                        <ItemStyle HorizontalAlign="Center" />
                                                                    </asp:TemplateField>
                                                                </Columns>


                                                                <PagerStyle CssClass="pagination-ys" />

                                                            </asp:GridView>
                                                            <asp:HiddenField ID="hdnLogo" runat="server" />
                                                        </div>
                                                    </div>
                                                    <div class="col-md-12">
                                                        <div class="col-md-6">
                                                            <asp:Label ID="lblAddressName" runat="server" Text="<%$ Resources:Attendance,Address Name %>"></asp:Label>
                                                            <asp:RequiredFieldValidator EnableClientScript="true" Style="float: right;" runat="server" ID="RequiredFieldValidator3" ValidationGroup="Address"
                                                                Display="Dynamic" SetFocusOnError="true" ControlToValidate="txtAddressName" ErrorMessage="<%$ Resources:Attendance,Enter Address Name %>" />
                                                            <div class="input-group">
                                                                <asp:TextBox ID="txtAddressName" TabIndex="5" AutoPostBack="true" BackColor="#eeeeee" OnTextChanged="txtAddressName_TextChanged"
                                                                    runat="server" CssClass="form-control"></asp:TextBox>
                                                                <cc1:AutoCompleteExtender ID="AutoCompleteExtender2" runat="server" CompletionInterval="100"
                                                                    CompletionSetCount="1" DelimiterCharacters="" Enabled="True" MinimumPrefixLength="1"
                                                                    ServiceMethod="GetCompletionListAddressName" ServicePath="" TargetControlID="txtAddressName"
                                                                    UseContextKey="True" CompletionListCssClass="autoCompleteList" CompletionListItemCssClass="autoCompleteListItem" CompletionListHighlightedItemCssClass="autoCompleteSelectedListItem" OnClientShown="resetPosition">
                                                                </cc1:AutoCompleteExtender>
                                                                <div class="input-group-btn">
                                                                    <asp:Button ID="imgAddAddressName" TabIndex="6" ValidationGroup="Address" Style="margin-left: 10px;" Visible="false"
                                                                        OnClick="imgAddAddressName_Click"
                                                                        CssClass="btn btn-info" runat="server" Text="<%$ Resources:Attendance,Add %>" />
                                                                    <asp:Button ID="btnAddNewAddress" TabIndex="7" Style="margin-left: 10px;"
                                                                        Visible="false" OnClick="btnAddNewAddress_Click" CssClass="btn btn-info"
                                                                        runat="server" Text="<%$ Resources:Attendance,New %>" />
                                                                    <asp:HiddenField ID="Hdn_Address_ID" runat="server" />
                                                                    <asp:HiddenField ID="hdnAddressId" runat="server" />
                                                                    <asp:HiddenField ID="hdnAddressName" runat="server" />
                                                                </div>
                                                            </div>
                                                            <br />
                                                        </div>
                                                    </div>
                                                    <div class="col-md-12">
                                                        <div class="col-md-12">
                                                            <div style="overflow: auto">
                                                                <asp:GridView CssClass="table-striped table-bordered table table-hover" ID="GvAddressName" runat="server" AutoGenerateColumns="False" Width="100%">

                                                                    <Columns>
                                                                        <asp:TemplateField HeaderText="Edit">
                                                                            <ItemTemplate>
                                                                                <asp:LinkButton ID="imgBtnAddressEdit" runat="server" Visible='<%# hdnCanEdit.Value=="true"?true:false%>' CommandArgument='<%# Eval("Trans_Id") %>' OnCommand="btnAddressEdit_Command"><i class="fa fa-pencil"></i></asp:LinkButton>
                                                                            </ItemTemplate>
                                                                            <ItemStyle HorizontalAlign="Center" />
                                                                        </asp:TemplateField>
                                                                        <asp:TemplateField HeaderText="Delete">
                                                                            <ItemTemplate>
                                                                                <asp:LinkButton ID="imgBtnDelete" runat="server" CommandArgument='<%# Eval("Trans_Id") %>' Visible='<%# hdnCanDelete.Value=="true"?true:false%>' OnCommand="btnAddressDelete_Command"><i class="fa fa-trash"></i></asp:LinkButton>
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
                                                                                <asp:Label ID="lblgvAddress" runat="server" Text='<%# Eval("FullAddress") %>' />
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
                                                    </div>
                                                    <div class="col-md-12">
                                                        <div class="col-md-6">
                                                            <asp:Label ID="Label4" runat="server" Text="<%$ Resources:Attendance,Bank Name %>"></asp:Label>
                                                            <div style="width: 100%;" class="input-group">
                                                                <asp:TextBox ID="txtContactBankName" TabIndex="8" AutoPostBack="true" BackColor="#eeeeee" OnTextChanged="txtContactBankName_TextChanged"
                                                                    Style="width: 65%;" runat="server" CssClass="form-control"></asp:TextBox>
                                                                <cc1:AutoCompleteExtender ID="AutoCompleteExtender6" runat="server" DelimiterCharacters=""
                                                                    Enabled="True" ServiceMethod="GetCompletionListBankName" ServicePath="" CompletionInterval="100"
                                                                    MinimumPrefixLength="1" CompletionSetCount="1" TargetControlID="txtContactBankName"
                                                                    UseContextKey="True" CompletionListCssClass="autoCompleteList" CompletionListItemCssClass="autoCompleteListItem" CompletionListHighlightedItemCssClass="autoCompleteSelectedListItem" OnClientShown="resetPosition">
                                                                </cc1:AutoCompleteExtender>
                                                                <asp:Button ID="BtnContactBank" TabIndex="9" Style="width: 15%; margin-left: 10px;" OnClientClick="show_Bank_Popup()" OnClick="BtnContactBank_click"
                                                                    CssClass="btn btn-info" runat="server" Text="<%$ Resources:Attendance,Add %>" />
                                                            </div>
                                                            <br />
                                                        </div>
                                                    </div>
                                                    <div class="col-md-12">
                                                        <div class="col-md-12">
                                                            <div style="overflow: auto; max-height: 500px;">



                                                                <asp:GridView CssClass="table-striped table-bordered table table-hover" ID="GvContactBankDetail" Width="100%" runat="server"
                                                                    AutoGenerateColumns="False">

                                                                    <Columns>
                                                                        <asp:TemplateField HeaderText="<%$ Resources:Attendance,Edit %>">
                                                                            <ItemTemplate>
                                                                                <asp:ImageButton ID="imgBtnContactBankEdit" OnClientClick="show_Bank_Popup()" runat="server" CommandArgument='<%# Eval("Trans_Id") %>'
                                                                                    ImageUrl="~/Images/edit.png" OnCommand="btnContactBankEdit_Command"
                                                                                    ToolTip="<%$ Resources:Attendance,Edit %>" />
                                                                            </ItemTemplate>
                                                                            <ItemStyle HorizontalAlign="Center" />
                                                                        </asp:TemplateField>
                                                                        <asp:TemplateField HeaderText="<%$ Resources:Attendance,Delete %>">
                                                                            <ItemTemplate>
                                                                                <asp:ImageButton ID="imgBtnContactBankDelete" runat="server" CommandArgument='<%# Eval("Trans_Id") %>'
                                                                                    ImageUrl="~/Images/Erase.png" OnCommand="btnContactBankDelete_Command"
                                                                                    ToolTip="<%$ Resources:Attendance,Delete %>" />
                                                                            </ItemTemplate>
                                                                            <ItemStyle HorizontalAlign="Center" />
                                                                        </asp:TemplateField>
                                                                        <asp:TemplateField HeaderText="<%$ Resources:Attendance, Bank Name %>">
                                                                            <ItemTemplate>
                                                                                <asp:Label ID="lblgvBankId" runat="server" Text='<%#Eval("Bank_Name") %>' />
                                                                            </ItemTemplate>
                                                                            <ItemStyle HorizontalAlign="Center" />
                                                                        </asp:TemplateField>
                                                                         <asp:TemplateField HeaderText="<%$ Resources:Attendance, Currency %>">
                                                                            <ItemTemplate>
                                                                                <asp:Label ID="lblGvCurrencyId" runat="server" Text='<%# GetCurrencyName(Eval("Currency_id").ToString()) %>' />
                                                                            </ItemTemplate>
                                                                            <ItemStyle HorizontalAlign="Center" />
                                                                        </asp:TemplateField>
                                                                        <asp:TemplateField HeaderText="<%$ Resources:Attendance,Account No %>">
                                                                            <ItemTemplate>
                                                                                <asp:Label ID="lblgvBankAccount" runat="server" Text='<%#Eval("Account_No") %>' />
                                                                            </ItemTemplate>
                                                                            <ItemStyle HorizontalAlign="Center" />
                                                                        </asp:TemplateField>
                                                                        <asp:TemplateField HeaderText="<%$ Resources:Attendance,IFSC Code %>">
                                                                            <ItemTemplate>
                                                                                <asp:Label ID="lblgvBankIFSCCode" runat="server" Text='<%#Eval("IFSC_Code") %>' />
                                                                            </ItemTemplate>
                                                                            <ItemStyle HorizontalAlign="Center" />
                                                                        </asp:TemplateField>
                                                                        <asp:TemplateField HeaderText="<%$ Resources:Attendance,MICR Code %>">
                                                                            <ItemTemplate>
                                                                                <asp:Label ID="lblgvBankMICRCode" runat="server" Text='<%#Eval("MICR_Code") %>' />
                                                                            </ItemTemplate>
                                                                            <ItemStyle HorizontalAlign="Center" />
                                                                        </asp:TemplateField>
                                                                        <asp:TemplateField HeaderText="<%$ Resources:Attendance,Branch Code %>">
                                                                            <ItemTemplate>
                                                                                <asp:Label ID="lblgvBankBranchCode" runat="server" Text='<%#Eval("Branch_Code") %>' />
                                                                            </ItemTemplate>
                                                                            <ItemStyle HorizontalAlign="Center" />
                                                                        </asp:TemplateField>
                                                                        <asp:TemplateField HeaderText="<%$ Resources:Attendance,IBAN NUMBER %>">
                                                                            <ItemTemplate>
                                                                                <asp:Label ID="lblgvIBANNUMBER" runat="server" Text='<%#Eval("IBAN_NUMBER") %>' />
                                                                            </ItemTemplate>
                                                                            <ItemStyle HorizontalAlign="Center" />
                                                                        </asp:TemplateField>
                                                                    </Columns>

                                                                    <PagerStyle CssClass="pagination-ys" />

                                                                </asp:GridView>
                                                                <asp:HiddenField ID="HiddenField1" runat="server" />
                                                                <asp:HiddenField ID="HiddenField2" runat="server" />
                                                            </div>
                                                        </div>
                                                    </div>
                                                    <div class="col-md-12" style="display:none">
                                                        <div class="col-md-6">
                                                            <asp:Label ID="lblAccountNo" runat="server" Text="<%$ Resources:Attendance,Account No. %>" />
                                                            <asp:TextBox ID="txtAccountNo" runat="server" CssClass="form-control" BackColor="#eeeeee"
                                                                AutoPostBack="true" OnTextChanged="txtAccountName_TextChanged" />
                                                            <cc1:AutoCompleteExtender ID="AutoComplete_Account" runat="server" CompletionInterval="100"
                                                                CompletionSetCount="1" DelimiterCharacters="" Enabled="True" MinimumPrefixLength="1"
                                                                ServiceMethod="GetCompletionListAccountName" ServicePath="" TargetControlID="txtAccountNo"
                                                                UseContextKey="True" CompletionListCssClass="autoCompleteList" CompletionListItemCssClass="autoCompleteListItem" CompletionListHighlightedItemCssClass="autoCompleteSelectedListItem" OnClientShown="resetPosition">
                                                            </cc1:AutoCompleteExtender>
                                                            <br />
                                                        </div>
                                                        <div class="col-md-6">
                                                            <asp:Label ID="lblCurrency" runat="server" Text="<%$ Resources:Attendance,Currency Name %>"></asp:Label>
                                                            <asp:DropDownList ID="ddlCurrency" runat="server" CssClass="form-control" />
                                                            <br />
                                                        </div>
                                                    </div>
                                                    <div id="tr1" runat="server" class="col-md-12" visible="false">
                                                        <div class="col-md-6">
                                                            <asp:Label ID="lblDebitAmount" runat="server" Text="<%$ Resources:Attendance,Debit Amount %>" />
                                                            <asp:TextBox ID="txtDebitAmount" Text="0.00" runat="server" ReadOnly="true" CssClass="form-control" />
                                                            <br />
                                                        </div>
                                                        <div class="col-md-6">
                                                            <asp:Label ID="lblCreditAmount" runat="server" Text="<%$ Resources:Attendance,Credit Amount %>" />
                                                            <asp:TextBox ID="txtCreditAmount" Text="0.00" runat="server" ReadOnly="true" CssClass="form-control" />
                                                            <br />
                                                        </div>
                                                    </div>
                                                    <div id="tr2" runat="server" class="col-md-12" visible="false">
                                                        <div class="col-md-6">
                                                            <asp:Label ID="lblODebitAmount" runat="server" Text="<%$ Resources:Attendance,Opening Debit Amount %>" />
                                                            <asp:TextBox ID="txtODebitAmount" Text="0.00" runat="server" CssClass="form-control" />
                                                            <br />
                                                        </div>
                                                        <div class="col-md-6">
                                                            <asp:Label ID="lblOCreditAmount" runat="server" Text="<%$ Resources:Attendance,Opening Credit Amount %>" />
                                                            <asp:TextBox ID="txtOCreditAmount" Text="0.00" runat="server" CssClass="form-control" />
                                                            <br />
                                                        </div>
                                                    </div>
                                                    <div id="tr3" runat="server" class="col-md-12" visible="false">
                                                        <div class="col-md-6">
                                                            <asp:Label ID="Label9" runat="server" Text="<%$ Resources:Attendance,Foreign Opening Debit %>" />
                                                            <asp:TextBox ID="txtFODebitAmount" Text="0.00" runat="server" CssClass="form-control" />
                                                            <br />
                                                        </div>
                                                        <div class="col-md-6">
                                                            <asp:Label ID="Label11" runat="server" Text="<%$ Resources:Attendance,Foreign Opening Credit %>" />
                                                            <asp:TextBox ID="txtFOCreditAmount" Text="0.00" runat="server" CssClass="form-control" />
                                                            <br />
                                                        </div>
                                                    </div>
                                                    <div class="col-md-12">
                                                        <div class="col-md-6">
                                                            <asp:Label ID="lblPurchaseLimit" runat="server" Text="<%$ Resources:Attendance,Purchase Limit %>" />
                                                            <asp:TextBox ID="txtPurchaseLimit" TabIndex="10" runat="server" CssClass="form-control" />
                                                            <br />
                                                            <asp:CheckBox ID="chkSpriceList" TabIndex="12" runat="server" Text="<%$ Resources:Attendance,Supplier Price List %>"
                                                                OnCheckedChanged="chkSpriceList_checkedChanged" AutoPostBack="true" />
                                                            <br />
                                                        </div>
                                                        <div class="col-md-6" id="ctlReturnDays" runat="server">
                                                            <asp:Label ID="lblReturnDays" runat="server" Text="<%$ Resources:Attendance,Return Days %>" />
                                                            <asp:TextBox ID="txtReturnDays" TabIndex="11" runat="server" CssClass="form-control" />
                                                            <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender2" runat="server" Enabled="True"
                                                                TargetControlID="txtReturnDays" FilterType="Numbers">
                                                            </cc1:FilteredTextBoxExtender>
                                                            <br />

                                                        </div>
                                                    </div>
                                                    <div class="col-md-12">
                                                        <div class="col-md-6">
                                                            <asp:Label ID="Label73" runat="server" Text="Is Credit" Visible="false"></asp:Label>
                                                            <asp:DropDownList ID="ddlIsCredit" runat="server" CssClass="form-control" Enabled="false" Visible="false">
                                                                <asp:ListItem Text="False" Value="False" Selected="True"></asp:ListItem>
                                                                <asp:ListItem Text="True" Value="True"></asp:ListItem>
                                                            </asp:DropDownList>
                                                            <br />
                                                            <asp:Button ID="BtnShowCpriceList" runat="server" Text="<%$ Resources:Attendance,Supplier Price List %>"
                                                                CssClass="btn btn-primary" OnClick="BtnShowCpriceList_click" OnClientClick="show_Customer_Price_Popup()" />
                                                            <asp:CheckBox ID="chkIsNonRegistered" TabIndex="13" runat="server" Text="Is Non Registered Supplier" />
                                                        </div>
                                                        <div class="col-md-6">
                                                            <br />
                                                            
                                                        </div>
                                                        <br />
                                                    </div>
                                                    <div class="col-md-12">
                                                        <cc1:TabContainer ID="TabContainer2" runat="server" OnClientActiveTabChanged="tabChanged"
                                                            ActiveTabIndex="0" style="height:auto" CssClass="ajax__tab_yuitabview-theme">
                                                            <cc1:TabPanel ID="TabCategory" runat="server" HeaderText="<%$ Resources:Attendance,Product Category %>">
                                                                <ContentTemplate>
                                                                    <asp:UpdatePanel ID="Update_Product_Tab" runat="server">
                                                                        <ContentTemplate>
                                                                            <asp:ListBox ID="lstProductCategory" runat="server" class="form-control" SelectionMode="Multiple"
                                                                                Font-Names="Trebuchet MS" Font-Size="Small" ForeColor="Gray"></asp:ListBox>
                                                                        </ContentTemplate>
                                                                    </asp:UpdatePanel>
                                                                </ContentTemplate>
                                                            </cc1:TabPanel>
                                                            <cc1:TabPanel ID="TabPanel1" runat="server" HeaderText="<%$ Resources:Attendance,Arca Wing %>">
                                                                <ContentTemplate>
                                                                    <asp:UpdatePanel ID="Update_Arca_Tab" runat="server">
                                                                        <Triggers>
                                                                            <asp:PostBackTrigger ControlID="gvFileMaster" />
                                                                        </Triggers>
                                                                        <ContentTemplate>
                                                                            <div class="row">
                                                                                <div class="col-md-12">
                                                                                    <div class="col-md-6">
                                                                                        <asp:Label ID="lblDocName" runat="server" Text="<%$ Resources:Attendance,Document Name %>"></asp:Label>
                                                                                        <asp:DropDownList ID="ddlDocumentName" runat="server" CssClass="form-control" />
                                                                                    </div>
                                                                                    <div class="col-md-6">
                                                                                        <asp:Label ID="Label12" runat="server" Text="<%$ Resources:Attendance,File Upload %>" />
                                                                                        <div class="input-group" style="width: 100%;">
                                                                                            <cc1:AsyncFileUpload ID="UploadFile"
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
                                                                                                <asp:LinkButton ID="FUAll_Img_Right" runat="server" Width="30px" Height="30px" Style="display: none"><i class="fa fa-check" style="font-size:30px;color:#22cb33"></i></asp:LinkButton>
                                                                                                <asp:LinkButton ID="FUAll_Img_Wrong" runat="server" Width="30px" Height="30px" Style="display: none"><i class="fa fa-remove" style="font-size:30px"></i></asp:LinkButton>
                                                                                                <asp:Image ID="FUAll_ImgLoader" runat="server" ImageUrl="../Images/loader.gif" />
                                                                                            </div>
                                                                                            <div class="input-group-btn">
                                                                                                <asp:ImageButton ID="BtnDocumentAdd" Style="width: 6%; margin-top: 5px;" runat="server" CausesValidation="False"
                                                                                                    ImageUrl="~/Images/add.png" OnClick="ImgButtonDocumentAdd_Click"
                                                                                                    ToolTip="<%$ Resources:Attendance,Add %>" />
                                                                                            </div>
                                                                                        </div>
                                                                                        <br />
                                                                                    </div>
                                                                                </div>

                                                                                <div class="col-md-12">
                                                                                    <asp:GridView CssClass="table-striped table-bordered table table-hover" ID="gvFileMaster" PageSize="5" runat="server" AutoGenerateColumns="False" Width="100%">
                                                                                        <Columns>
                                                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,Download %>" ItemStyle-HorizontalAlign="Center">
                                                                                                <ItemTemplate>
                                                                                                    <asp:LinkButton ID="lnkDownload" runat="server" Text="Download" OnCommand="OnDownloadCommand"
                                                                                                        CommandArgument='<%#Eval("Trans_id") %>'></asp:LinkButton>
                                                                                                </ItemTemplate>
                                                                                                <ItemStyle />
                                                                                            </asp:TemplateField>
                                                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,Delete %>">
                                                                                                <ItemTemplate>
                                                                                                    <asp:ImageButton ID="IbtnDelete" runat="server" CausesValidation="False" CommandArgument='<%# Eval("Trans_Id") %>'
                                                                                                        ImageUrl="~/Images/Erase.png" OnCommand="IbtnDeleteDocument_Command" ToolTip="<%$ Resources:Attendance,Delete %>" />
                                                                                                </ItemTemplate>
                                                                                                <ItemStyle HorizontalAlign="Center" />
                                                                                                <ItemStyle />
                                                                                            </asp:TemplateField>
                                                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,Document Name %>">
                                                                                                <ItemTemplate>
                                                                                                    <asp:Label ID="lblDocumentName" runat="server" Text='<%# Eval("DocumentName") %>'></asp:Label>
                                                                                                </ItemTemplate>
                                                                                                <ItemStyle />
                                                                                            </asp:TemplateField>
                                                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,File Name %>">
                                                                                                <ItemTemplate>
                                                                                                    <asp:Label ID="lblFileName" runat="server" Text='<%# Eval("File_Name") %>'></asp:Label>
                                                                                                </ItemTemplate>
                                                                                                <ItemStyle />
                                                                                            </asp:TemplateField>
                                                                                        </Columns>


                                                                                        <PagerStyle CssClass="pagination-ys" />

                                                                                    </asp:GridView>
                                                                                    <asp:HiddenField ID="HiddenField4" runat="server" />
                                                                                </div>
                                                                            </div>
                                                                        </ContentTemplate>
                                                                    </asp:UpdatePanel>
                                                                </ContentTemplate>
                                                            </cc1:TabPanel>
                                                            <cc1:TabPanel ID="TabLocation" runat="server" HeaderText="<%$ Resources:Attendance,Opening Balance %>" Visible="false">
                                                                <ContentTemplate>
                                                                    <asp:UpdatePanel ID="Update_Opening" runat="server">
                                                                        <ContentTemplate>
                                                                            <div class="row">
                                                                                <div class="col-md-12">
                                                                                    <asp:GridView CssClass="table-striped table-bordered table table-hover" ID="GvLocation" PageSize="5" runat="server" AutoGenerateColumns="False">
                                                                                        <Columns>
                                                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,Location %>">
                                                                                                <ItemTemplate>
                                                                                                    <asp:Label ID="lblgvLocationName" runat="server" Text='<%#GetLocationName(Eval("Location_Id").ToString()) %>' />
                                                                                                    <asp:HiddenField ID="hdngvLocationId" runat="server" Value='<%#Eval("Location_Id") %>' />
                                                                                                </ItemTemplate>
                                                                                                <ItemStyle HorizontalAlign="Center" />
                                                                                            </asp:TemplateField>
                                                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,Currency %>">
                                                                                                <ItemTemplate>
                                                                                                    <asp:Label ID="lblgvCurrencyName" runat="server" Text='<%#GetCurrencyName(Eval("Field1").ToString()) %>' />
                                                                                                    <asp:HiddenField ID="hdngvCurrencyId" runat="server" Value='<%#Eval("Field1") %>' />
                                                                                                </ItemTemplate>
                                                                                                <ItemStyle HorizontalAlign="Center" />
                                                                                            </asp:TemplateField>
                                                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,Debit%>">
                                                                                                <ItemTemplate>
                                                                                                    <asp:TextBox ID="txtgvDebit" runat="server" />
                                                                                                    <cc1:FilteredTextBoxExtender ID="FilteredtxtgvDebit" runat="server" Enabled="True"
                                                                                                        TargetControlID="txtgvDebit" ValidChars="1,2,3,4,5,6,7,8,9,0,.">
                                                                                                    </cc1:FilteredTextBoxExtender>
                                                                                                </ItemTemplate>
                                                                                                <ItemStyle HorizontalAlign="Center" />
                                                                                            </asp:TemplateField>
                                                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,Credit%>">
                                                                                                <ItemTemplate>
                                                                                                    <asp:TextBox ID="txtgvCredit" runat="server" />
                                                                                                    <cc1:FilteredTextBoxExtender ID="FilteredtxtgvCredit" runat="server" Enabled="True"
                                                                                                        TargetControlID="txtgvCredit" ValidChars="1,2,3,4,5,6,7,8,9,0,.">
                                                                                                    </cc1:FilteredTextBoxExtender>
                                                                                                </ItemTemplate>
                                                                                                <ItemStyle HorizontalAlign="Center" />
                                                                                            </asp:TemplateField>
                                                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,Foreign Debit%>">
                                                                                                <ItemTemplate>
                                                                                                    <asp:TextBox ID="txtgvForeignDebit" runat="server" />
                                                                                                    <cc1:FilteredTextBoxExtender ID="FilteredtxtgvForeignDebit" runat="server" Enabled="True"
                                                                                                        TargetControlID="txtgvForeignDebit" ValidChars="1,2,3,4,5,6,7,8,9,0,.">
                                                                                                    </cc1:FilteredTextBoxExtender>
                                                                                                </ItemTemplate>
                                                                                                <ItemStyle HorizontalAlign="Center" />
                                                                                            </asp:TemplateField>
                                                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,Foreign Credit%>">
                                                                                                <ItemTemplate>
                                                                                                    <asp:TextBox ID="txtgvForeignCredit" runat="server" />
                                                                                                    <cc1:FilteredTextBoxExtender ID="FilteredtxtgvForeignCredit" runat="server" Enabled="True"
                                                                                                        TargetControlID="txtgvForeignCredit" ValidChars="1,2,3,4,5,6,7,8,9,0,.">
                                                                                                    </cc1:FilteredTextBoxExtender>
                                                                                                </ItemTemplate>
                                                                                                <ItemStyle HorizontalAlign="Center" />
                                                                                            </asp:TemplateField>
                                                                                        </Columns>


                                                                                        <PagerStyle CssClass="pagination-ys" />

                                                                                    </asp:GridView>
                                                                                </div>
                                                                            </div>
                                                                        </ContentTemplate>
                                                                    </asp:UpdatePanel>
                                                                </ContentTemplate>
                                                            </cc1:TabPanel>
                                                            <%--due after this point--%>
                                                            <cc1:TabPanel ID="TabPanel3" runat="server" HeaderText="<%$ Resources:Attendance,Credit Information %>" Visible="false">
                                                                <ContentTemplate>
                                                                    <asp:UpdatePanel ID="Update_Credit_Tab" runat="server">
                                                                        <ContentTemplate>
                                                                            <div class="row">
                                                                                <div class="col-md-12">
                                                                                    <div class="col-md-6">
                                                                                        <asp:Label ID="lblCreditLimit" runat="server" Text="<%$ Resources:Attendance,Credit Limit %>" />
                                                                                        <div class="input-group">
                                                                                            <asp:TextBox ID="txtCreditLimit" runat="server" CssClass="form-control" Style="width: 70%;" />
                                                                                            <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender15txtCreditLimit" runat="server"
                                                                                                Enabled="True" TargetControlID="txtCreditLimit" ValidChars="1,2,3,4,5,6,7,8,9,0,.">
                                                                                            </cc1:FilteredTextBoxExtender>
                                                                                            <asp:DropDownList ID="ddlCurrencyCreditLimit" runat="server" CssClass="form-control"
                                                                                                Style="width: 30%;" />
                                                                                        </div>
                                                                                    </div>
                                                                                    <div class="col-md-6">
                                                                                        <asp:Label ID="lblCreditDays" runat="server" Text="<%$ Resources:Attendance,Credit Days %>" />
                                                                                        <asp:TextBox ID="txtCreditDays" runat="server" CssClass="form-control" />
                                                                                        <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender16" runat="server" Enabled="True"
                                                                                            TargetControlID="txtCreditDays" FilterType="Numbers">
                                                                                        </cc1:FilteredTextBoxExtender>
                                                                                        <br />
                                                                                    </div>
                                                                                </div>
                                                                                <div class="col-md-12">
                                                                                    <asp:Label ID="Label33" runat="server" Text="Credit Parameter" />
                                                                                    <asp:RadioButton ID="rbtnAdvanceCheque" runat="server" Text="Advance Cheque Basis"
                                                                                        GroupName="Parameter" />
                                                                                    <asp:RadioButton ID="rbtnInvoicetoInvoice" runat="server" GroupName="Parameter" Text="Invoice to Invoice Payment" />
                                                                                    <asp:RadioButton ID="rbtnAdvanceHalfpayment" GroupName="Parameter" runat="server"
                                                                                        Text="50% Advance and 50% on Delivery" />
                                                                                    <asp:RadioButton ID="rbtnNone" GroupName="Parameter" runat="server" Checked="true"
                                                                                        Text="None" />
                                                                                </div>
                                                                            </div>
                                                                        </ContentTemplate>
                                                                    </asp:UpdatePanel>
                                                                </ContentTemplate>
                                                            </cc1:TabPanel>
                                                            <cc1:TabPanel ID="TabBankDetail" HeaderText="Bank Reference" runat="server">
                                                                <ContentTemplate>
                                                                      <asp:UpdatePanel ID="Update_Panel_BankDetail" runat="server">
                                                                          <Triggers>
                                                                            <asp:AsyncPostBackTrigger ControlID="gvBankDetails" />
                                                                          </Triggers>
                                                                          <ContentTemplate>
                                                                              <div class="row">
                                                                    <div class="form-group">
                                                                                        <div class="col-md-12">
                                                                                            <div class="col-md-6">                                                                                                
                                                                                                <br />
                                                                                                <asp:Label ID="Label8" runat="server" Text="<%$ Resources:Attendance,Currency Name %>"></asp:Label>
                                                                                                <asp:DropDownList ID="ddlBankCurrency" runat="server" class="form-control select2" Style="width: 100%;" />
                                                                                                <asp:HiddenField ID="hdnbankEditId" runat="server" />
                                                                                                 </div>
                                                                                            <div class="col-md-6">
                                                                                                <br />
                                                                                                <asp:Label ID="trnsEmp" runat="server" Text="Transaction By"></asp:Label>
                                                                                                <asp:TextBox ID="textTransEmp" AutoPostBack="true" OnTextChanged="txtTransPerson_TextChanged" runat="server" CssClass="form-control"></asp:TextBox>
                                                                                                <cc1:AutoCompleteExtender ID="txtEmpPerson_AutoCompleteExtender" runat="server"
                                                                                                    DelimiterCharacters="" Enabled="True" CompletionInterval="100" CompletionSetCount="1"
                                                                                                    MinimumPrefixLength="1" ServiceMethod="GetCompletionListEmployeeName" ServicePath=""
                                                                                                    TargetControlID="textTransEmp" UseContextKey="True" CompletionListCssClass="autoCompleteList" CompletionListItemCssClass="autoCompleteListItem" CompletionListHighlightedItemCssClass="autoCompleteSelectedListItem" OnClientShown="resetPosition">
                                                                                                </cc1:AutoCompleteExtender>
                                                                                            </div>
                                                                                              <div class="col-md-6"> 
                                                                                                <br />
                                                                                                <asp:Label ID="Label16" runat="server" Text="Exchange Name" />
                                                                                                <asp:TextBox ID="txtExcnageName" runat="server" CssClass="form-control" />
                                                                                                </div>
                                                                                            <div class="col-md-6"> 
                                                                                                <br />
                                                                                                <asp:Label ID="Label59" runat="server" Text="Benefeciary Name" />
                                                                                                <asp:TextBox ID="txtAccountName1" runat="server" CssClass="form-control" />
                                                                                                </div>
                                                                                            <div class="col-md-12">
                                                                                                <br />
                                                                                                <asp:Label ID="Label17" runat="server" Text="Benefeciary Address" />
                                                                                                <asp:TextBox ID="txtBankSupplerAddress" runat="server" CssClass="form-control" TextMode="MultiLine" />

                                                                                            </div>
                                                                                             <div class="col-md-6"> 
                                                                                                <br />
                                                                                                <asp:Label ID="Label61" runat="server" Text="Benefeciary Account No." />
                                                                                                <asp:TextBox ID="txtAccountNo1" runat="server" CssClass="form-control" />                                                                                             
                                                                                                  </div>
                                                                                            <div class="col-md-6">  
                                                                                                 <br />                                                                                               
                                                                                                <asp:Label ID="Label13" runat="server" Text="IFSC Code" />
                                                                                                <asp:TextBox ID="txtIfscCode" runat="server" CssClass="form-control" />                                                                                                
                                                                                              </div>
                                                                                            <div class="col-md-6"> 
                                                                                                 <br /> 
                                                                                                 <asp:Label ID="Label14" runat="server" Text="IBAN Code" />
                                                                                                <asp:TextBox ID="txtibanCode" runat="server" CssClass="form-control" />                                                                                               
                                                                                                 </div>
                                                                                            <div class="col-md-6"> 
                                                                                                 <br />   
                                                                                                 <asp:Label ID="Label15" runat="server" Text="SWIFT Code" />
                                                                                                <asp:TextBox ID="txtSwiftCode" runat="server" CssClass="form-control" />                                                                                               
                                                                                                </div> 
                                                                                                <div class="col-md-6">     
                                                                                                    <br />                                                                                     
                                                                                                <asp:Label ID="Label63" runat="server" Text="Banker's Name" />
                                                                                                <asp:TextBox ID="txtAccountbankerName1" runat="server" CssClass="form-control" />
                                                                                            
                                                                                                     </div>
                                                                                            <div class="col-md-12"> 
                                                                                                <br />
                                                                                                <asp:Label ID="Label65" runat="server" Text="Banker's Address" />
                                                                                                <asp:TextBox ID="txtAccountbankerAddress1" runat="server" CssClass="form-control" TextMode="MultiLine" />
                                                                                            
                                                                                                    </div>
                                                                                            <div class="col-md-6">   
                                                                                                <asp:Label ID="Label67" runat="server" Text="Contact No." />
                                                                                                <div style="width: 100%" class="input-group">
                                                                                                    <asp:DropDownList ID="ddlAccount1ContactNo_CountryCode" Style="width: 30%" runat="server" class="form-control select2">
                                                                                                    </asp:DropDownList>
                                                                                                    <asp:TextBox ID="txtAccountContactNo1" Style="width: 70%" runat="server" CssClass="form-control" />
                                                                                                    <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender11" runat="server" Enabled="True"
                                                                                                        TargetControlID="txtAccountContactNo1" FilterType="Numbers">
                                                                                                    </cc1:FilteredTextBoxExtender>
                                                                                                </div>
                                                                                                 </div>
                                                                                            <div class="col-md-6">                                                                                               
                                                                                                <asp:Label ID="Label69" runat="server" Text="Fax No." />
                                                                                                <div style="width: 100%" class="input-group">
                                                                                                    <asp:DropDownList ID="ddlAccount1FaxNo_CountryCode" runat="server" Style="width: 30%" class="form-control select2">
                                                                                                    </asp:DropDownList>
                                                                                                    <asp:TextBox ID="txtAccountFaxNo1" runat="server" Style="width: 70%" CssClass="form-control" />
                                                                                                    <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender13" runat="server" Enabled="True"
                                                                                                        TargetControlID="txtAccountFaxNo1" FilterType="Numbers">
                                                                                                    </cc1:FilteredTextBoxExtender>
                                                                                                </div>
                                                                                                <br />
                                                                                            </div>  
                                                                                            <div class="col-md-12"style="align-items:center" >
                                                                                                <asp:Button ID="btnAddBankDetail" OnClick="btnAddBankDetail_Click"  CssClass="btn btn-primary" runat="server" Text="Add Bank" />
                                                                                            </div>

                                                                                            <div class="col-md-12">
                                                                                                <br />
                                                                                                <br />
                                                                                                <asp:UpdatePanel runat="server" ID="UpbankDetail" UpdateMode="Conditional">
                                                                                                    <Triggers>
                                                                                                     
                                                                                                    </Triggers>
                                                                                                    <ContentTemplate>
                                                                                                        <div style="overflow: auto">
                                                                                                            <asp:GridView ID="gvBankDetails"  CssClass="table-striped table-bordered table table-hover" OnRowEditing="gvBankDetails_RowEditing" runat="server" AutoGenerateColumns="false">

                                                                                                                <Columns>
                                                                                                                    <asp:TemplateField HeaderText="Actions">
                                                                                                                        <ItemTemplate>
                                                                                                                           <%-- <asp:Button ID="lnkDelete" CssClass="center" runat="server" Text="Delete" CommandName="Delete" CommandArgument="<%# Container.DataItemIndex %>" OnCommand="gvBankDetails_RowCommand" />
                                                                                                                            <asp:Button ID="lnkEditgv" CssClass="center"  runat="server" Text="Edit" CommandName="Edit" CommandArgument="<%# Container.DataItemIndex %>" OnCommand="gvBankDetails_RowCommand" />--%>
                                                                                                                            <asp:ImageButton ID="lnkEditgv" runat="server" CausesValidation="False" CommandName="Edit" CommandArgument="<%# Container.DataItemIndex %>" ImageUrl="~/Images/edit.png" OnCommand="gvBankDetails_RowCommand" Width="16px" /> &nbsp;
                                                                                                                           <asp:ImageButton ID="lnkDelete" runat="server" CausesValidation="False" CommandName="Delete" CommandArgument="<%# Container.DataItemIndex %>" ImageUrl="~/Images/Erase.png" OnCommand="gvBankDetails_RowCommand" Width="16px" />
                                                                                                                        </ItemTemplate>
                                                                                                                    </asp:TemplateField>
                                                                                                                    <asp:TemplateField HeaderText="Currency" SortExpression="Currency">
                                                                                                                        <ItemTemplate>
                                                                                                                            <asp:Label ID="gvlbCurrencyName" runat="server" Text='<%# Eval("CurrencyName") %>'></asp:Label>
                                                                                                                        </ItemTemplate>
                                                                                                                        <ItemStyle />
                                                                                                                    </asp:TemplateField>
                                                                                                                    <asp:TemplateField HeaderText="BENEFECIARY Name" SortExpression="AccountName">
                                                                                                                        <ItemTemplate>
                                                                                                                            <asp:Label ID="gvlbAccountName" runat="server" Text='<%# Eval("AccountName") %>'></asp:Label>
                                                                                                                        </ItemTemplate>
                                                                                                                        <ItemStyle />
                                                                                                                    </asp:TemplateField>
                                                                                                                    <asp:TemplateField HeaderText="BENEFECIARY Address" SortExpression="BeneFeciaryAddress">
                                                                                                                        <ItemTemplate>
                                                                                                                            <asp:Label ID="gvlbAccountAddress" runat="server" Text='<%# Eval("BeneFeciaryAddress") %>'></asp:Label>
                                                                                                                        </ItemTemplate>
                                                                                                                        <ItemStyle />
                                                                                                                    </asp:TemplateField>
                                                                                                                    <asp:TemplateField HeaderText="Account No." SortExpression="AccountNo">
                                                                                                                        <ItemTemplate>
                                                                                                                            <asp:Label ID="gvlbAccountNo" runat="server" Text='<%# Eval("AccountNo") %>'></asp:Label>
                                                                                                                        </ItemTemplate>
                                                                                                                        <ItemStyle />
                                                                                                                    </asp:TemplateField>
                                                                                                                    <asp:TemplateField HeaderText="IFSC Code" SortExpression="IFSCCode">
                                                                                                                        <ItemTemplate>
                                                                                                                            <asp:Label ID="gvlbIFSCCode" runat="server" Text='<%# Eval("IFSCCode") %>'></asp:Label>
                                                                                                                        </ItemTemplate>
                                                                                                                        <ItemStyle />
                                                                                                                    </asp:TemplateField>
                                                                                                                    <asp:TemplateField HeaderText="IBAN Code" SortExpression="IBANCode">
                                                                                                                        <ItemTemplate>
                                                                                                                            <asp:Label ID="gvlbIBANCode" runat="server" Text='<%# Eval("IBANCode") %>'></asp:Label>
                                                                                                                        </ItemTemplate>
                                                                                                                        <ItemStyle />
                                                                                                                    </asp:TemplateField>
                                                                                                                    <asp:TemplateField HeaderText="SWIFT Code" SortExpression="SWIFTCode">
                                                                                                                        <ItemTemplate>
                                                                                                                            <asp:Label ID="gvlblSWIFTCode" runat="server" Text='<%# Eval("SWIFTCode") %>'></asp:Label>
                                                                                                                        </ItemTemplate>
                                                                                                                        <ItemStyle />
                                                                                                                    </asp:TemplateField>
                                                                                                                    <asp:TemplateField HeaderText="Banker Name " SortExpression="BankerName">
                                                                                                                        <ItemTemplate>
                                                                                                                            <asp:Label ID="gvlblBankerName" runat="server" Text='<%# Eval("BankerName") %>'></asp:Label>
                                                                                                                        </ItemTemplate>
                                                                                                                        <ItemStyle />
                                                                                                                    </asp:TemplateField>
                                                                                                                    <asp:TemplateField HeaderText="Banker Address " SortExpression="BankerAddress">
                                                                                                                        <ItemTemplate>
                                                                                                                            <asp:Label ID="gvlblBankerAddress" runat="server" Text='<%# Eval("BankerAddress") %>'></asp:Label>
                                                                                                                        </ItemTemplate>
                                                                                                                        <ItemStyle />
                                                                                                                    </asp:TemplateField>
                                                                                                                    <asp:TemplateField HeaderText="Contact No." SortExpression="ContactNo">
                                                                                                                        <ItemTemplate>
                                                                                                                            <asp:Label ID="gvlblContactNo" runat="server" Text='<%# Eval("ContactNo") %>'></asp:Label>
                                                                                                                        </ItemTemplate>
                                                                                                                        <ItemStyle />
                                                                                                                    </asp:TemplateField>
                                                                                                                    <asp:TemplateField HeaderText="Fax No" SortExpression="FaxNo">
                                                                                                                        <ItemTemplate>
                                                                                                                            <asp:Label ID="gvlblFaxNo" runat="server" Text='<%# Eval("FaxNo") %>'></asp:Label>
                                                                                                                        </ItemTemplate>
                                                                                                                        <ItemStyle />
                                                                                                                    </asp:TemplateField>
                                                                                                                    <asp:TemplateField HeaderText="Transaction By" SortExpression="trnsEmpName">
                                                                                                                        <ItemTemplate>
                                                                                                                            <asp:Label ID="gvlbltrnsEmpName" runat="server" Text='<%# Eval("trnsEmpName") %>'></asp:Label>
                                                                                                                        </ItemTemplate>
                                                                                                                        <ItemStyle />
                                                                                                                    </asp:TemplateField>
                                                                                                                    <asp:TemplateField HeaderText="Exchange Name" SortExpression="ExchangeName">
                                                                                                                        <ItemTemplate>
                                                                                                                            <asp:Label ID="gvlblExchangeName" runat="server" Text='<%# Eval("ExchangeName") %>'></asp:Label>
                                                                                                                        </ItemTemplate>
                                                                                                                        <ItemStyle />
                                                                                                                    </asp:TemplateField>
                                                                                                                </Columns>
                                                                                                            </asp:GridView>
                                                                                                        </div>
                                                                                                    </ContentTemplate>
                                                                                                </asp:UpdatePanel>
                                                                                            </div>
                                                                                        </div>
                                                                                        


                                                                        </div>
                                                                                  </div>
                                                                              </ContentTemplate>
                                                                          </asp:UpdatePanel>
                                                                     </ContentTemplate>
                                                                                    
                                                            </cc1:TabPanel>
                                                        </cc1:TabContainer>
                                                        <br />
                                                        <div style="text-align: center">
                                                            <asp:Button ID="btnSupplierSave" TabIndex="14" runat="server" Text="<%$ Resources:Attendance,Save %>"
                                                                CssClass="btn btn-success" Visible="false" OnClick="btnSupplierSave_Click" />
                                                            <asp:Button ID="Button1" TabIndex="15" runat="server" Text="<%$ Resources:Attendance,Reset %>"
                                                                CssClass="btn btn-primary" CausesValidation="False" OnClick="BtnReset_Click" />
                                                            <asp:Button ID="btnSupplierCancel" TabIndex="16" runat="server" CssClass="btn btn-danger" Text="<%$ Resources:Attendance,Cancel %>"
                                                                CausesValidation="False" OnClick="btnSupplierCancel_Click" />
                                                            <asp:HiddenField ID="hdnSupplierId" runat="server" />
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

                     <div class="tab-pane" id="import">
                        <asp:UpdatePanel ID="update_import" runat="server">
                            <Triggers>
                                <%--<asp:PostBackTrigger ControlID="lnkDownloadData" />--%>
                                <asp:AsyncPostBackTrigger ControlID="btnUploadExcel" />

                            </Triggers>
                            <ContentTemplate>
                                <div class="row">
                                    <div class="col-md-12">
                                        <div id="Div4" runat="server" class="box box-info">
                                            <div class="box-header with-border">
                                                <h3 class="box-title">
                                                    <asp:Label ID="Label27" runat="server" Text="Upload Using Excel"></asp:Label></h3>
                                                &nbsp;&nbsp;&nbsp;&nbsp;
                                                <asp:RadioButton ID="rbtnUploadOb" Style="margin-left: 20px; margin-right: 20px;" runat="server" GroupName="upd_type" AutoPostBack="true" Checked="true" OnCheckedChanged="rbtnUploadOb_CheckedChanged" Text="Upload Opening Balance" />
                                                <asp:RadioButton ID="rbtnUploadAgeing" Style="margin-left: 20px; margin-right: 20px;" runat="server" GroupName="upd_type" AutoPostBack="true" Checked="false" OnCheckedChanged="rbtnUploadAgeing_CheckedChanged" Text="Upload Ageing" />
                                                &nbsp;&nbsp;&nbsp;&nbsp;
                                                <%--<asp:HiddenField ID="hdnTotalExcelRecords" runat="server" Value="0" />
                                                <asp:HiddenField ID="hdnInvalidExcelRecords" runat="server" Value="0" />
                                                <asp:HiddenField ID="hdnValidExcelRecords" runat="server" Value="0" />--%>
                                            </div>
                                            <div class="box-body">
                                                <div class="row">
                                                    <div class="col-md-4">
                                                        <%--<asp:LinkButton ID="lnkDownloadObData" runat="server" Text="Download Ob Data" OnClick="lnkDownloadData_Click" Font-Bold="true" Visible="true" Font-Size="15px"></asp:LinkButton>
                                                        <asp:LinkButton ID="lnkDownloadAgeingData" runat="server" Text="Download Ageing Data" OnClick="lnkDownloadAgeingData_Click" Font-Bold="true" Visible="false" Font-Size="15px"></asp:LinkButton>--%>
                                                        <asp:HyperLink ID="lnkDownloadObData" runat="server" Font-Bold="true" Font-Size="15px"
                                                            NavigateUrl="~/CompanyResource/supplier_ob.xlsx" Text="Download sample/data" Font-Underline="true" Visible="true"></asp:HyperLink>
                                                        <asp:HyperLink ID="lnkDownloadAgeingData" runat="server" Font-Bold="true" Font-Size="15px"
                                                            NavigateUrl="~/CompanyResource/supplier_ob_ageing.xlsx" Text="Download sample/data" Font-Underline="true" Visible="false"></asp:HyperLink>
                                                    </div>
                                                    <div class="col-md-4">
                                                        <asp:Label runat="server" Text="Browse Excel File" ID="Label169"></asp:Label>
                                                        <div class="input-group" style="width: 100%;">
                                                            <cc1:AsyncFileUpload ID="fileLoad"
                                                                OnClientUploadStarted="FUExcel_UploadStarted"
                                                                OnClientUploadError="FUExcel_UploadError"
                                                                OnClientUploadComplete="FUExcel_UploadComplete"
                                                                OnUploadedComplete="FUExcel_FileUploadComplete"
                                                                runat="server" CssClass="form-control"
                                                                CompleteBackColor="White"
                                                                UploaderStyle="Traditional"
                                                                UploadingBackColor="#CCFFFF"
                                                                ThrobberID="FUExcel_ImgLoader" Width="100%" />
                                                            <div class="input-group-btn" style="border: solid 1px #d2d6de; width: 35px;">
                                                                <asp:LinkButton ID="FUExcel_Img_Right" runat="server" Width="30px" Height="30px" Style="display: none"><i class="fa fa-check" style="font-size:30px;color:#22cb33"></i></asp:LinkButton>
                                                                <asp:LinkButton ID="FUExcel_Img_Wrong" runat="server" Width="30px" Height="30px" Style="display: none"><i class="fa fa-remove" style="font-size:30px"></i></asp:LinkButton>
                                                                <asp:Image ID="FUExcel_ImgLoader" runat="server" ImageUrl="../Images/loader.gif" />
                                                            </div>
                                                        </div>
                                                    </div>
                                                    <div class="col-md-4">
                                                        <asp:Button ID="btnUploadExcel" runat="server" CssClass="btn btn-primary" OnClick="btnUploadExcel_Click" Text="Import" />
                                                        <asp:Button ID="btnSaveExcelData" runat="server" CssClass="btn btn-primary" OnClick="btnSaveExcelData_Click" Text="Update" Enabled="false" />
                                                        <asp:Button ID="btnResetUpload" runat="server" CssClass="btn btn-primary" OnClick="btnResetUpload_Click" Text="Reset" />
                                                    </div>
                                                </div>
                                                <br />

                                                <div class="row" id="uploadOb" runat="server" visible="false">

                                                    <div class="col-md-6" style="text-align: left">
                                                        <asp:RadioButton ID="rbtnupdall" Style="margin-left: 20px; margin-right: 20px;" runat="server" GroupName="upd" AutoPostBack="true" Checked="true" OnCheckedChanged="rbtnupdall_CheckedChanged" Text="All" />
                                                        <asp:RadioButton ID="rbtnupdValid" Style="margin-left: 20px; margin-right: 20px;" runat="server" GroupName="upd" AutoPostBack="true" Text="Valid" OnCheckedChanged="rbtnupdValid_CheckedChanged" />
                                                        <asp:RadioButton ID="rbtnupdInValid" Style="margin-left: 20px; margin-right: 20px;" runat="server" GroupName="upd" AutoPostBack="true" Text="Invalid" OnCheckedChanged="rbtnupdInValid_CheckedChanged" />
                                                    </div>
                                                    
                                                    <div class="col-md-6" style="text-align: right;">
                                                        <asp:Label ID="lbltotaluploadRecord" runat="server"></asp:Label>
                                                    </div>
                                                    <div class="col-md-12">
                                                        <div style="overflow: auto; max-height: 300px;">
                                                            <asp:GridView CssClass="table-striped table-bordered table table-hover" ID="gvImport" runat="server" Width="100%">
                                                                <PagerStyle CssClass="pagination-ys" />
                                                            </asp:GridView>
                                                        </div>
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



                    <div class="tab-pane" id="Bin">
                        <asp:UpdatePanel ID="Update_Bin" runat="server">
                            <ContentTemplate>




                                <div class="row">
                                    <div class="col-md-12">
                                        <div id="Div2" runat="server" class="box box-info collapsed-box">
                                            <div class="box-header with-border">
                                                <h3 class="box-title">
                                                    <asp:Label ID="Label2" runat="server" Text="Advance Search"></asp:Label></h3>
                                                &nbsp;&nbsp;|&nbsp;&nbsp;
					   <asp:Label ID="lblTotalRecordsBin" Text="<%$ Resources:Attendance,Total Records: 0 %>" runat="server"></asp:Label>

                                                <div class="box-tools pull-right">
                                                    <button type="button" class="btn btn-box-tool" data-widget="collapse">
                                                        <i id="I2" runat="server" class="fa fa-plus"></i>
                                                    </button>
                                                </div>
                                            </div>
                                            <div class="box-body">
                                                <div class="col-lg-3">
                                                    <asp:DropDownList ID="ddlFieldNameBin" runat="server" CssClass="form-control">
                                                        <asp:ListItem Text="<%$ Resources:Attendance, Supplier Id %>" Value="Code"></asp:ListItem>
                                                        <asp:ListItem Text="<%$ Resources:Attendance, Supplier Name %>" Value="Name" Selected="True"></asp:ListItem>
                                                        <asp:ListItem Text="<%$ Resources:Attendance, Supplier Name(Local) %>" Value="Name_L"></asp:ListItem>
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
                                                <div class="col-lg-5">
                                                    <asp:Panel ID="Panel2" runat="server" DefaultButton="btnbindBin">
                                                        <asp:TextBox ID="txtValueBin" placeholder="Search from Content" class="form-control" runat="server"></asp:TextBox>
                                                    </asp:Panel>
                                                </div>
                                                <div class="col-lg-2" style="text-align: center;">
                                                    <asp:LinkButton ID="btnbindBin" runat="server" CausesValidation="False" OnClientClick="tab_3_open()" OnClick="btnbindBin_Click" ToolTip="<%$ Resources:Attendance,Search %>"><span class="fa fa-search"  style="font-size:25px;"></span></asp:LinkButton>&nbsp;&nbsp;&nbsp;
                                                    <asp:LinkButton ID="btnRefreshBin" runat="server" CausesValidation="False" OnClientClick="tab_3_open()" OnClick="btnRefreshBin_Click" ToolTip="<%$ Resources:Attendance,Refresh %>"><span class="fa fa-repeat"  style="font-size:25px;"></span></asp:LinkButton>&nbsp;&nbsp;&nbsp;
                                                    <asp:LinkButton ID="imgBtnRestore" CausesValidation="False" Visible="false" runat="server" OnClick="imgBtnRestore_Click" ToolTip="<%$ Resources:Attendance, Active %>"><span class="fas fa-th"  style="font-size:25px;"></span></asp:LinkButton>
                                                </div>

                                            </div>
                                        </div>
                                    </div>
                                </div>
                                <div class="box box-warning box-solid" <%= GvSupplierBin.Rows.Count>0?"style='display:block'":"style='display:none'"%>>
                                    <div class="box-body">
                                        <div class="row">
                                            <div class="col-md-12">
                                                <div style="overflow: auto">
                                                    <asp:GridView CssClass="table-striped table-bordered table table-hover" ID="GvSupplierBin" PageSize="<%# PageControlCommon.GetPageSize() %>" Width="100%"
                                                        runat="server" AutoGenerateColumns="False" AllowPaging="True" OnPageIndexChanging="GvSupplierBin_PageIndexChanging"
                                                        OnSorting="GvSupplierBin_OnSorting" AllowSorting="true">
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
                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,Supplier Id%>" SortExpression="Code">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblgvSupplierCode" runat="server" Text='<%# Eval("Code") %>' />
                                                                    <asp:Label ID="lblgvSupplierId" Visible="false" runat="server" Text='<%# Eval("Supplier_Id") %>' />
                                                                </ItemTemplate>
                                                                <ItemStyle />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,Supplier Name %>" SortExpression="Name">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblgvSupplierName" runat="server" Text='<%# Eval("Name") %>' />
                                                                </ItemTemplate>
                                                                <ItemStyle />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,Supplier Name(Local) %>"
                                                                SortExpression="Name_L">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblgvLCustomerName" runat="server" Text='<%# Eval("Name_L") %>' />
                                                                </ItemTemplate>
                                                                <ItemStyle />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,Account No %>" SortExpression="Account_No">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblgvAccountNo" runat="server" Text='<%# Eval("Account_No") %>' />
                                                                </ItemTemplate>
                                                                <ItemStyle />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,Purchase Limit %>" SortExpression="Purchase_Limit">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblgvPurchaseLimit" runat="server" Text='<%# Eval("Purchase_Limit") %>' />
                                                                </ItemTemplate>
                                                                <ItemStyle />
                                                            </asp:TemplateField>
                                                        </Columns>


                                                        <PagerStyle CssClass="pagination-ys" />

                                                    </asp:GridView>
                                                    <asp:HiddenField ID="HDFSortbin" runat="server" />
                                                    <asp:Label ID="lblSelectedRecord" runat="server" Visible="false"></asp:Label>
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


    <div class="modal fade" id="Modal_Address" tabindex="-1" role="dialog" aria-labelledby="Modal_AddressLabel"
        aria-hidden="true">
        <div class="modal-dialog modal-lg">
            <div class="modal-content">

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

    <div class="modal fade" id="Modal_Bank" tabindex="-1" role="dialog" aria-labelledby="Modal_BankLabel"
        aria-hidden="true">
        <div class="modal-dialog modal-lg">
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal">
                        <span aria-hidden="true">&times;</span><span class="sr-only">Close</span></button>
                    <h4 class="modal-title" id="Modal_BankLabel">Add Bank</h4>
                </div>
                <div class="modal-body">
                    <asp:UpdatePanel ID="Update_Bank_Modal" runat="server">
                        <ContentTemplate>
                            <div class="row">
                                <div class="col-md-12">
                                    <div class="col-md-6">
                                        <asp:Label ID="Label1238" runat="server" Text="<%$ Resources:Attendance,Bank Name %>" />
                                        <asp:TextBox ID="txtBankName" runat="server" CssClass="form-control" AutoPostBack="true"
                                            OnTextChanged="txtBankName_TextChanged" BackColor="#eeeeee" />
                                        <cc1:AutoCompleteExtender ID="AutoCompleteExtender7" runat="server" DelimiterCharacters=""
                                            Enabled="True" ServiceMethod="GetCompletionListBankName" ServicePath="" CompletionInterval="100"
                                            MinimumPrefixLength="1" CompletionSetCount="1" TargetControlID="txtBankName"
                                            UseContextKey="True" CompletionListCssClass="autoCompleteList" CompletionListItemCssClass="autoCompleteListItem" CompletionListHighlightedItemCssClass="autoCompleteSelectedListItem" OnClientShown="resetPosition">
                                        </cc1:AutoCompleteExtender>
                                        <br />
                                    </div>
                                    <div class="col-md-6">
                                        <asp:Label ID="Label20" runat="server" Text="<%$ Resources:Attendance,Account No %>" />
                                        <asp:TextBox ID="txtCBAccountNo" runat="server" CssClass="form-control" />
                                        <br />
                                    </div>
                                </div>
                                <div class="col-md-12">
                                    <div class="col-md-6">
                                        <asp:Label ID="Label3" runat="server" Text="<%$ Resources:Attendance,Currency Name %>"></asp:Label>
                                        <asp:DropDownList ID="ddlBankAcCurrency" runat="server" CssClass="form-control" />
                                        <br />
                                    </div>
                                </div>
                                <div class="col-md-12">
                                    <div class="col-md-12">
                                        <asp:Label ID="Label22" runat="server" Text="<%$ Resources:Attendance,Branch Address %>" />
                                        <asp:TextBox ID="txtCBBrachAddress" TextMode="MultiLine" runat="server"
                                            CssClass="form-control" />
                                        <br />
                                    </div>
                                </div>

                                <div class="col-md-12">
                                    <div class="col-md-6">
                                        <asp:Label ID="Label23" runat="server" Text="<%$ Resources:Attendance,IFSC Code %>" />
                                        <asp:TextBox ID="txtCBIFSCCode" runat="server" CssClass="form-control" />
                                        <br />
                                        <asp:Label ID="Label24" runat="server" Text="<%$ Resources:Attendance,MICR Code %>" />
                                        <asp:TextBox ID="txtCBMICRCode" runat="server" CssClass="form-control" />
                                        <br />
                                    </div>
                                    <div class="col-md-6">
                                        <asp:Label ID="Label21" runat="server" Text="<%$ Resources:Attendance,Branch Code %>" />
                                        <asp:TextBox ID="txtCBBranchCode" runat="server" CssClass="form-control" />
                                        <br />
                                        <asp:Label ID="lblIBanNum" runat="server" Text="<%$ Resources:Attendance,IBAN NUMBER %>" />
                                        <asp:TextBox ID="txtIBANNUMBER" runat="server" CssClass="form-control" />
                                        <br />
                                    </div>
                                </div>
                                <asp:HiddenField ID="hdnContactBankId" runat="server" />
                            </div>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
                <div class="modal-footer">
                    <asp:UpdatePanel ID="Update_Bank_Button" runat="server">
                        <ContentTemplate>
                            <asp:Button ID="BtnCBSave" runat="server" Text="<%$ Resources:Attendance,Save %>"
                                CssClass="btn btn-success" OnClick="btnCBSave_Click" />
                            <asp:Button ID="BtnCBCancel" runat="server" Style="margin-left: 15px;" Text="<%$ Resources:Attendance,Reset %>"
                                CssClass="btn btn-primary" OnClick="BtnCBCancel_Click" />
                            <button type="button" class="btn btn-danger" data-dismiss="modal">
                                Close</button>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
            </div>
        </div>
    </div>

    <div class="modal fade" id="Modal_Product" tabindex="-1" role="dialog" aria-labelledby="Modal_ProductLabel"
        aria-hidden="true">
        <div class="modal-dialog modal-lg">
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal">
                        <span aria-hidden="true">&times;</span><span class="sr-only">Close</span></button>
                    <h4 class="modal-title" id="Modal_ProductLabel">Supplier Price List</h4>
                </div>
                <div class="modal-body">
                    <asp:UpdatePanel ID="Update_Product_Modal" runat="server">
                        <ContentTemplate>
                            <div class="row">
                                <div class="col-md-12">
                                    <div class="col-md-6">
                                        <asp:Label ID="Label26" runat="server" Text="<%$ Resources:Attendance,Supplier Name %>" />
                                        &nbsp : &nbsp
                                                                <asp:Label ID="pnllblSupplierName" runat="server" Font-Bold="true" />
                                        <br />
                                    </div>
                                    <div class="col-md-6">
                                        <asp:Label ID="Label31" runat="server" CssClass="labelComman" Text="<%$ Resources:Attendance,Company Name %>" />
                                        &nbsp : &nbsp
                                                                <asp:Label ID="pnllblCompanyName" runat="server" CssClass="labelComman" Font-Bold="true" />
                                        <br />
                                    </div>
                                </div>
                            </div>
                            <div class="alert alert-info ">
                                <div class="row">
                                    <div class="col-md-12">
                                        <div class="form-group">
                                            <div class="col-lg-3">
                                                <asp:DropDownList ID="ddlCpriceField" runat="server" CssClass="form-control">
                                                    <asp:ListItem Text="<%$ Resources:Attendance, Product Id %>" Value="ProductId"></asp:ListItem>
                                                    <asp:ListItem Text="<%$ Resources:Attendance, Product Name %>" Value="EProductName"
                                                        Selected="True"></asp:ListItem>
                                                    <asp:ListItem Text="<%$ Resources:Attendance,Product Code %>" Value="ProductCode"></asp:ListItem>
                                                </asp:DropDownList>
                                            </div>
                                            <div class="col-lg-2">
                                                <asp:DropDownList ID="ddlCpriceFieldoption" runat="server" CssClass="form-control">
                                                    <asp:ListItem Text="<%$ Resources:Attendance,--Select-- %>"></asp:ListItem>
                                                    <asp:ListItem Text="<%$ Resources:Attendance,Equal %>"></asp:ListItem>
                                                    <asp:ListItem Selected="True" Text="<%$ Resources:Attendance,Contains %>"></asp:ListItem>
                                                    <asp:ListItem Text="<%$ Resources:Attendance,Like %>"></asp:ListItem>
                                                </asp:DropDownList>
                                            </div>
                                            <div class="col-lg-3">
                                                <asp:TextBox ID="txtCpriceSearch" runat="server" CssClass="form-control"></asp:TextBox>
                                            </div>
                                            <div class="col-lg-2" style="text-align: center; padding-left: 1px; padding-right: 1px;">
                                                <asp:ImageButton ID="btnBindCPricelist" Style="width: 35px; margin-top: -1px;" runat="server"
                                                    CausesValidation="False" ImageUrl="~/Images/search.png" OnClick="btnBindCPricelist_Click"
                                                    ToolTip="<%$ Resources:Attendance,Search %>"></asp:ImageButton>
                                                <asp:ImageButton ID="btnRefreshCPricelist" runat="server" Style="width: 30px;" CausesValidation="False"
                                                    ImageUrl="~/Images/refresh.png" OnClick="btnRefreshCPricelist_Click" ToolTip="<%$ Resources:Attendance,Refresh %>"></asp:ImageButton>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div class="box box-warning box-solid">
                                <div class="box-header with-border">
                                    <h3 class="box-title"></h3>
                                </div>
                                <div style="max-height: 250px; overflow: auto;" class="box-body">
                                    <div class="row">
                                        <div class="col-md-12">

                                            <asp:GridView CssClass="table-striped table-bordered table table-hover" ID="GvProduct" runat="server" Width="100%" AutoGenerateColumns="False">

                                                <Columns>
                                                    <asp:TemplateField HeaderText="<%$ Resources:Attendance,Product Id %>">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblgvProductId" runat="server" Text='<%#Eval("ProductId") %>' />
                                                        </ItemTemplate>
                                                        <ItemStyle HorizontalAlign="Center" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="<%$ Resources:Attendance, Product Name %>">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblgvProductName" runat="server" Text='<%#Eval("EProductName") %>' />
                                                        </ItemTemplate>
                                                        <ItemStyle HorizontalAlign="Center" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="<%$ Resources:Attendance,Product Code %>">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblgvProductcode" runat="server" Text='<%#Eval("ProductCode") %>' />
                                                        </ItemTemplate>
                                                        <ItemStyle HorizontalAlign="Center" />
                                                    </asp:TemplateField>
                                                    <%--<asp:TemplateField HeaderText="<%$ Resources:Attendance,Part No. %>">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblgvPartNo" runat="server" Text='<%#Eval("PartNo") %>' />
                                                        </ItemTemplate>
                                                        <ItemStyle  HorizontalAlign="Center" />
                                                    </asp:TemplateField>--%>
                                                    <asp:TemplateField HeaderText="<%$ Resources:Attendance, Unit %>">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblgvUnitId" runat="server" Visible="false" Text='<%#Eval("UnitId") %>' />
                                                            <asp:Label ID="lblgvUnit" runat="server" Text='<%#GetUnitName(Eval("UnitId").ToString()) %>' />
                                                        </ItemTemplate>
                                                        <ItemStyle HorizontalAlign="Center" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="<%$ Resources:Attendance,Sales Price %>">
                                                        <ItemTemplate>
                                                            <asp:TextBox ID="txtGvsalesPrice" runat="server"></asp:TextBox>
                                                        </ItemTemplate>
                                                        <ItemStyle HorizontalAlign="Center" />
                                                    </asp:TemplateField>
                                                </Columns>

                                                <PagerStyle CssClass="pagination-ys" />

                                            </asp:GridView>
                                        </div>
                                    </div>
                                </div>
                            </div>

                            <asp:HiddenField ID="hdnCustomerpricelist" runat="server" />

                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
                <div class="modal-footer">
                    <asp:UpdatePanel ID="Update_Product_Button" runat="server">
                        <ContentTemplate>
                            <asp:Button ID="BtnCustomerPriceListSave" runat="server" Text="<%$ Resources:Attendance,Save %>"
                                CssClass="btn btn-success" OnClick="BtnCustomerPriceListSave_Click" />&nbsp;&nbsp;&nbsp;
                                                            <asp:Button ID="BtnCustomerPriceListCancel" runat="server" Text="<%$ Resources:Attendance,Reset %>"
                                                                CssClass="btn btn-primary" OnClick="BtnCustomerPriceListCancel_Click" />
                            <button type="button" class="btn btn-danger" data-dismiss="modal">
                                Close</button>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
            </div>
        </div>
    </div>


    <asp:UpdateProgress ID="UpdateProgress5" runat="server" AssociatedUpdatePanelID="Update_Bank_Button">
        <ProgressTemplate>
            <div class="modal_Progress">
                <div class="center_Progress">
                    <img alt="" src="../Images/ajax-loader2.gif" style="vertical-align: middle; background-color: White; border-radius: 10px;" />
                </div>
            </div>
        </ProgressTemplate>
    </asp:UpdateProgress>

    

    <asp:UpdateProgress ID="UpdateProgress4" runat="server" AssociatedUpdatePanelID="update_import">
        <ProgressTemplate>
            <div class="modal_Progress">
                <div class="center_Progress">
                    <img alt="" src="../Images/ajax-loader2.gif" style="vertical-align: middle; background-color: White; border-radius: 10px;" />
                </div>
            </div>
        </ProgressTemplate>
    </asp:UpdateProgress>

    <asp:UpdateProgress ID="UpdateProgress6" runat="server" AssociatedUpdatePanelID="Update_Product_Button">
        <ProgressTemplate>
            <div class="modal_Progress">
                <div class="center_Progress">
                    <img alt="" src="../Images/ajax-loader2.gif" style="vertical-align: middle; background-color: White; border-radius: 10px;" />
                </div>
            </div>
        </ProgressTemplate>
    </asp:UpdateProgress>

    <asp:UpdateProgress ID="UpdateProgress2" runat="server" AssociatedUpdatePanelID="Update_Button">
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

    <asp:UpdateProgress ID="UpdateProgress3" runat="server" AssociatedUpdatePanelID="Update_New">
        <ProgressTemplate>
            <div class="modal_Progress">
                <div class="center_Progress">
                    <img alt="" src="../Images/ajax-loader2.gif" style="vertical-align: middle; background-color: White; border-radius: 10px;" />
                </div>
            </div>
        </ProgressTemplate>
    </asp:UpdateProgress>


    <asp:UpdateProgress ID="UpdateProgress8" runat="server" AssociatedUpdatePanelID="Update_Bin">
        <ProgressTemplate>
            <div class="modal_Progress">
                <div class="center_Progress">
                    <img alt="" src="../Images/ajax-loader2.gif" style="vertical-align: middle; background-color: White; border-radius: 10px;" />
                </div>
            </div>
        </ProgressTemplate>
    </asp:UpdateProgress>


    <asp:UpdateProgress ID="UpdateProgress10" runat="server" AssociatedUpdatePanelID="Update_Bank_Modal">
        <ProgressTemplate>
            <div class="modal_Progress">
                <div class="center_Progress">
                    <img alt="" src="../Images/ajax-loader2.gif" style="vertical-align: middle; background-color: White; border-radius: 10px;" />
                </div>
            </div>
        </ProgressTemplate>
    </asp:UpdateProgress>

    <asp:UpdateProgress ID="UpdateProgress11" runat="server" AssociatedUpdatePanelID="Update_Product_Modal">
        <ProgressTemplate>
            <div class="modal_Progress">
                <div class="center_Progress">
                    <img alt="" src="../Images/ajax-loader2.gif" style="vertical-align: middle; background-color: White; border-radius: 10px;" />
                </div>
            </div>
        </ProgressTemplate>
    </asp:UpdateProgress>
    <div class="modal fade" id="ModelAcMaster" tabindex="-1" role="dialog" aria-labelledby="myModalLabel"
        aria-hidden="true">
        <div class="modal-dialog modal-lg">
            <div class="modal-content">
                <div class="modal-body">
                    <uc1:AccountMaster runat="server" ID="UcAcMaster" />
                </div>
                <div class="modal-footer">
                    <button type="button" id="" class="btn btn-danger" data-dismiss="modal">
                        Close</button>
                </div>
            </div>
        </div>
    </div>

    <div class="modal fade" id="modelContactDetail" tabindex="-1" role="dialog" aria-labelledby="myModalLabel"
        aria-hidden="true">
        <div class="modal-dialog modal-lg">
            <div class="modal-content">

                <div class="modal-body">
                    <ucContact:ViewContact ID="UcContactList" runat="server" />
                </div>


                <div class="modal-footer">
                    <button type="button" id="" class="btn btn-danger" data-dismiss="modal">
                        Close</button>
                </div>
            </div>
        </div>
    </div>

    <div class="modal fade" id="ControlSettingModal" tabindex="-1" role="dialog" aria-labelledby="ControlSetting_ModalLabel" aria-hidden="true">
        <div class="modal-dialog modal-md">
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal">
                        <span aria-hidden="true">&times;</span><span class="sr-only">Close</span></button>
                    <h4 class="modal-title">
                        <asp:Label ID="lblUcSettingsTitle" runat="server" Text="Set Columns Visibility" />
                    </h4>

                </div>
                <div class="modal-body">
                    <UC:ucCtlSetting ID="ucCtlSetting" runat="server" />
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

        function Li_Tab_Bin() {
            document.getElementById('<%= Btn_Bin.ClientID %>').click();
        }

        function Modal_Address_Open() {
            document.getElementById('<%= Btn_Add_Address.ClientID %>').click();
        }
        function Modal_Address_Close() {
            document.getElementById('<%= Btn_Add_Address.ClientID %>').click();
            document.getElementById('<%= imgAddAddressName.ClientID %>').click();
        }

        function Modal_Add_Bank() {
            document.getElementById('<%= Btn_Bank_Account.ClientID %>').click();
        }

        function Modal_Show_Product() {
            document.getElementById('<%= Btn_Product_Details.ClientID %>').click();
        }

        function LI_Edit_Active() {
            $("#Li_List").removeClass("active");
            $("#List").removeClass("active");

            $("#Li_New").addClass("active");
            $("#New").addClass("active");

            var yourUl = document.getElementById("Li_New");
            yourUl.style.display = yourUl.style.display = '';
        }

        function LI_List_Active() {
            $("#Li_New").removeClass("active");
            $("#New").removeClass("active");

            $("#Li_List").addClass("active");
            $("#List").addClass("active");

            var yourUl = document.getElementById("Li_New");
            yourUl.style.display = yourUl.style.display = 'none';
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
        function Modal_Close_Address() {
            document.getElementById('<%= Btn_Add_Address.ClientID %>').click();
        }

        function Modal_Close_Bank() {
            document.getElementById('<%= Btn_Bank_Account.ClientID %>').click();
        }

        function Modal_Close_Product() {
            document.getElementById('<%= Btn_Product_Details.ClientID %>').click();
        }

        function Modal_ContactInfo_Open() {
            $('#modelContactDetail').modal('show');
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
        function Modal_AcMaster_Open() {
            document.getElementById('<%= Btn_ucAcMaster.ClientID %>').click();
        }
        function showUcControlsSettings() {
            $('#ControlSettingModal').modal('show');
        }
        function Modal_Open_FileUpload() {
            document.getElementById('<%= Btn_Modal_FileUpload.ClientID %>').click();
        }

        function FUExcel_UploadComplete(sender, args) {
            document.getElementById('<%= FUExcel_Img_Wrong.ClientID %>').style.display = "none";
            document.getElementById('<%= FUExcel_Img_Right.ClientID %>').style.display = "";
        }
        function FUExcel_UploadError(sender, args) {
            document.getElementById('<%= FUExcel_Img_Right.ClientID %>').style.display = "none";
            document.getElementById('<%= FUExcel_Img_Wrong.ClientID %>').style.display = "";
            alert('Invalid File Type, Select Only .xls, .xlsx, .mdb, .accdb extension file');
        }
        function FUExcel_UploadStarted(sender, args) {
            var filename = args.get_fileName();

            var filext = filename.substring(filename.lastIndexOf(".") + 1);
            filext = filext.toLowerCase();
            if (filext == "xls" || filext == "xlsx" || filext == "mdb" || filext == "accdb") {
                return true;
            }
            else {
                throw {
                    name: "Invalid File Type",
                    level: "Error",
                    message: "Invalid File Type, Select Only .xls, .xlsx, .mdb, .accdb extension file",
                    htmlMessage: "Invalid File Type, Select Only .xls, .xlsx, .mdb, .accdb extension file"
                }
                return false;
            }
        }

    </script>
</asp:Content>
