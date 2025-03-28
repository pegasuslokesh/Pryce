<%@ Page Title="" Language="C#" MasterPageFile="~/ERPMaster.master" MaintainScrollPositionOnPostback="true" AutoEventWireup="true" CodeFile="CustomerMaster.aspx.cs" Inherits="MasterSetUp_CustomerMaster" %>


<%@ Register Src="~/WebUserControl/ContactInfo.ascx" TagName="ViewContact" TagPrefix="AT1" %>
<%@ Register Src="~/WebUserControl/AddressControl.ascx" TagName="Addressmaster" TagPrefix="UC" %>
<%@ Register Src="~/WebUserControl/FileUpload.ascx" TagPrefix="AT1" TagName="FileUpload1" %>
<%@ Register Src="~/WebUserControl/AccountMaster.ascx" TagPrefix="uc1" TagName="AccountMaster" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Src="~/WebUserControl/ucControlsSetting.ascx" TagName="ucCtlSetting" TagPrefix="UC" %>


<asp:Content ID="Content1" ContentPlaceHolderID="Head" runat="Server">
    <link href="../Bootstrap_Files/Additional/Button_Style.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript" src="http://maps.google.com/maps/api/js?sensor=false"></script>

    <style type="text/css">
        .ratingEmpty {
            background-image: url(../Images/ratingStarEmpty.gif);
            width: 18px;
            height: 18px;
        }

        .ratingFilled {
            background-image: url(../Images/ratingStarFilled.gif);
            width: 18px;
            height: 18px;
        }

        .ratingSaved {
            background-image: url(../Images/ratingStarSaved.gif);
            width: 18px;
            height: 18px;
        }

        .Selected {
            color: orange;
        }
    </style>
    <script>
        function resetPosition1() {

        }
    </script>
    <script src="../Script/customer.js"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="HeaderContent" runat="Server">
    <h1>
        <i class="fas fa-users-cog"></i>&nbsp;&nbsp;
        <asp:Label ID="lblHeader" runat="server" Text="<%$ Resources:Attendance,Customer Setup%>"></asp:Label>
    </h1>
    <ol class="breadcrumb">
        <li><a href="#"><i class="fa fa-dashboard"></i>
            <asp:Label ID="Label6" runat="server" Text="<%$ Resources:Attendance,Master Setup%>"></asp:Label></a></li>
        <li><a href="#"><i class="fa fa-dashboard"></i>
            <asp:Label ID="Label10" runat="server" Text="<%$ Resources:Attendance,Master SetUp%>"></asp:Label></a></li>
        <li class="active">
            <asp:Label ID="Label5" runat="server" Text="<%$ Resources:Attendance,Customer Setup%>"></asp:Label></li>
    </ol>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="Server">
    <asp:UpdatePanel ID="Update_Button" runat="server">
        <ContentTemplate>
            <asp:Button ID="Btn_List" Style="display: none;" runat="server" OnClick="btnList_Click" Text="List" />
            <asp:Button ID="Btn_Bin" Style="display: none;" runat="server" OnClick="btnBin_Click" Text="Bin" />
            <asp:Button ID="Btn_Add_Address" Style="display: none;" data-toggle="modal" data-target="#Modal_Address" runat="server" Text="Address Details" />
            <asp:Button ID="Btn_Bank_Account" Style="display: none;" data-toggle="modal" data-target="#Modal_Bank" runat="server" Text="Bank Details" />
            <asp:Button ID="Btn_Product_Details" Style="display: none;" data-toggle="modal" data-target="#Modal_Product" runat="server" Text="Product Details" />
            <asp:Button ID="Btn_Number" Style="display: none;" data-toggle="modal" data-target="#NumberData" runat="server" Text="Number Data" />
            <asp:Button ID="Btn_CustomerInfo_Modal" Style="display: none;" runat="server" data-toggle="modal" data-target="#modelContactDetail" />
            <asp:Button ID="Btn_Modal_FileUpload" Style="display: none;" runat="server" data-toggle="modal" data-target="#Fileupload123" Text="FileUpload" />
            <asp:Button ID="Btn_ucAcMaster" Style="display: none;" runat="server" data-toggle="modal" data-target="#ModelAcMaster" Text="Add Accounts Detail" />
            <asp:HiddenField runat="server" ID="hdnCanEdit" />
            <asp:HiddenField runat="server" ID="hdnCanDelete" />
            <asp:HiddenField runat="server" ID="hdnCanPrint" />
            <asp:HiddenField runat="server" ID="hdnCanView" />
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
                        <i class="fa fa-upload"></i>&nbsp;&nbsp;<asp:Label ID="Label11" runat="server" Text="Upload Ob"></asp:Label></a></li>
                    <li style="display: none;" id="Li_New"><a href="#New" data-toggle="tab">
                        <asp:UpdatePanel ID="Update_Li" runat="server">
                            <ContentTemplate>
                                <i class="fa fa-file"></i>&nbsp;&nbsp;
                                <asp:Label ID="Lbl_Tab_New" runat="server" Text="<%$ Resources:Attendance,Edit%>"></asp:Label>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </a></li>
                    <li id="Li_List" class="active"><a onclick="Li_Tab_List()" href="#List" data-toggle="tab">
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
                                                    <asp:Label ID="Label2" runat="server" Text="Advance Search"></asp:Label></h3>
                                                &nbsp;&nbsp;|&nbsp;&nbsp;
                                               <asp:Label ID="lblTotalRecords" Text="<%$ Resources:Attendance,Total Records: 0 %>" runat="server"></asp:Label>

                                                <div class="box-tools pull-right">
                                                    <button type="button" class="btn btn-box-tool" data-widget="collapse">
                                                        <i id="I1" runat="server" class="fa fa-plus"></i>
                                                    </button>
                                                </div>
                                            </div>
                                            <div class="box-body">

                                                <div class="col-md-6">
                                                    <asp:Label ID="lblBrandsearch" runat="server" Text="<%$ Resources:Attendance,Customer Type %>"></asp:Label>
                                                    <asp:DropDownList ID="ddlGroupSearch" runat="server" class="form-control select2" Style="width: 100%;">
                                                    </asp:DropDownList>
                                                </div>
                                                <div style="text-align: center;" class="col-md-6">
                                                    <br />
                                                    <asp:LinkButton ID="btngo" runat="server" CausesValidation="False" OnClick="btngo_Click" ToolTip="<%$ Resources:Attendance,Search %>"><span class="fa fa-search"  style="font-size:25px;"></span></asp:LinkButton>&nbsp;&nbsp;&nbsp;
                                                    <asp:LinkButton ID="btnResetSreach" runat="server" CausesValidation="False" OnClick="btnResetSreach_Click" ToolTip="<%$ Resources:Attendance,Reset %>"><span class="fa fa-repeat"  style="font-size:25px;"></span></asp:LinkButton>&nbsp;&nbsp;&nbsp;

                                                </div>
                                                <div class="col-lg-12">
                                                    <br />
                                                </div>
                                                <div class="col-lg-2">
                                                    <asp:DropDownList ID="ddlCreditStatus" runat="server" class="form-control select2" Style="width: 100%;" AutoPostBack="true"
                                                        OnSelectedIndexChanged="ddlPosted_SelectedIndexChanged">
                                                        <asp:ListItem Text="<%$ Resources:Attendance,All %>" Value="0" Selected="True"></asp:ListItem>
                                                        <asp:ListItem Text="<%$ Resources:Attendance,Credit %>" Value="True"></asp:ListItem>
                                                        <asp:ListItem Text="<%$ Resources:Attendance,Cash %>" Value="False"></asp:ListItem>
                                                    </asp:DropDownList>
                                                </div>

                                                <div class="col-lg-2">
                                                    <asp:DropDownList ID="ddlFieldName" runat="server" class="form-control select2" Style="width: 100%;">
                                                        <asp:ListItem Text="<%$ Resources:Attendance, Customer Id %>" Value="Customer_Code"></asp:ListItem>
                                                        <asp:ListItem Text="<%$ Resources:Attendance, Customer Name %>" Value="Name" Selected="True"></asp:ListItem>
                                                        <asp:ListItem Text="<%$ Resources:Attendance, Customer Name(Local) %>" Value="Name_L"></asp:ListItem>
                                                        <asp:ListItem Text="<%$ Resources:Attendance, Email %>" Value="Field1"></asp:ListItem>
                                                        <asp:ListItem Text="Mobile" Value="Phone_no"></asp:ListItem>
                                                    </asp:DropDownList>
                                                </div>
                                                <div class="col-lg-2">
                                                    <asp:DropDownList ID="ddlOption" runat="server" class="form-control select2" Style="width: 100%;">
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
                                                <div class="col-lg-2">
                                                    <asp:LinkButton ID="btnbind" runat="server" CausesValidation="False" OnClick="btnbindrpt_Click" ToolTip="<%$ Resources:Attendance,Search %>"><span class="fa fa-search"  style="font-size:25px;"></span></asp:LinkButton>&nbsp;&nbsp;&nbsp;
                                                    <asp:LinkButton ID="btnRefresh" runat="server" CausesValidation="False" OnClick="btnRefreshReport_Click" ToolTip="<%$ Resources:Attendance,Refresh %>"><span class="fa fa-repeat"  style="font-size:25px;"></span></asp:LinkButton>&nbsp;&nbsp;&nbsp;
                                                    <asp:LinkButton ID="btnGvListSetting" ImageAlign="Right" ToolTip="List Settings" runat="server" OnClick="btnGvListSetting_Click" Visible="false"><span class="fa fa-wrench"  style="font-size:25px;"></span></asp:LinkButton>
                                                </div>

                                                <asp:HiddenField ID="hdntxtaddressid" runat="server" />
                                            </div>
                                        </div>

                                    </div>
                                </div>


                                <div class="box box-warning box-solid">
                                    <div class="box-body">
                                        <div class="row">
                                            <div class="col-md-12">
                                                <div style="overflow: auto">
                                                    <asp:HiddenField ID="HDFSort" runat="server" />
                                                    <asp:GridView CssClass="table-striped table-bordered table table-hover" ID="GvCustomer" PageSize="<%# PageControlCommon.GetPageSize() %>" runat="server"
                                                        AutoGenerateColumns="False" Width="100%" AllowPaging="True" OnPageIndexChanging="GvCustomer_PageIndexChanging"
                                                        AllowSorting="True" OnSorting="GvCustomer_Sorting">
                                                        <Columns>
                                                            <asp:TemplateField HeaderText="Action">
                                                                <ItemTemplate>
                                                                    <div class="dropdown" style="position: absolute;">
                                                                        <button class="btn btn-default dropdown-toggle" type="button" data-toggle="dropdown">
                                                                            <i class="fa fa-ellipsis-h" aria-hidden="true"></i>
                                                                        </button>
                                                                        <ul class="dropdown-menu">
                                                                            <li <%= hdnCanPrint.Value=="true"?"style='display:block'":"style='display:none'" %>>
                                                                                <asp:LinkButton ID="IbtnPrint" runat="server" CausesValidation="False" CommandArgument='<%# Eval("Customer_Id") %>' OnCommand="IbtnPrint_Command"><i class="fa fa-print"></i>Print</asp:LinkButton>
                                                                            </li>
                                                                            <li <%= hdnCanView.Value=="true"?"style='display:block'":"style='display:none'" %>>
                                                                                <asp:LinkButton ID="imgBtnDetail" runat="server" CommandArgument='<%# Eval("Customer_Id") %>' OnCommand="lnkViewDetail_Command"><i class="fa fa-eye"></i>View</asp:LinkButton>
                                                                            </li>

                                                                            <li <%= hdnCanEdit.Value=="true"?"style='display:block'":"style='display:none'" %>>
                                                                                <asp:LinkButton ID="btnEdit" runat="server" CommandArgument='<%# Eval("Customer_Id") %>' OnCommand="btnEdit_Command" CausesValidation="False"><i class="fa fa-pencil"></i>Edit </asp:LinkButton>
                                                                            </li>
                                                                            <li <%= hdnCanDelete.Value=="true"?"style='display:block'":"style='display:none'" %>>
                                                                                <asp:LinkButton ID="IbtnDelete" runat="server" CausesValidation="False" CommandArgument='<%# Eval("Customer_Id") %>' OnCommand="IbtnDelete_Command"><i class="fa fa-trash"></i>Delete</asp:LinkButton>
                                                                            </li>

                                                                            <li>
                                                                                <a id="lnkcustomerHistory" style="cursor: pointer" onclick="window.open('../Purchase/CustomerHistory.aspx?ContactId=<%# Eval("Customer_Id") %>&&Page=SINQ','_blank','width=1024, ');return false;"><i class="fa fa-history"></i>Customer History</a>
                                                                            </li>
                                                                            <li <%= hdnCanUpload.Value=="true"?"style='display:block'":"style='display:none'" %>>
                                                                                <asp:LinkButton ID="ImageButton1" runat="server" CommandArgument='<%# Eval("Customer_Id") %>' CommandName='<%# Eval("Customer_Code") %>' OnCommand="btnFileUpload_Command" CausesValidation="False"><i class="fa fa-upload"></i>File Upload</asp:LinkButton>
                                                                            </li>
                                                                        </ul>
                                                                    </div>

                                                                </ItemTemplate>
                                                            </asp:TemplateField>


                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,Customer Id %>" SortExpression="Customer_Code">
                                                                <ItemTemplate>
                                                                    <asp:LinkButton ID="lnkAccountMaster" Text='<%# Eval("Customer_Code") %>' runat="server" CommandArgument='<%# Eval("Customer_Id") + "," + Eval("Name")  %>'
                                                                        OnCommand="lnkAccountMaster_Command" ToolTip="Account Master" />
                                                                </ItemTemplate>
                                                                <ItemStyle HorizontalAlign="Center" />
                                                            </asp:TemplateField>
                                                            <%--<asp:BoundField DataField="Customer_Code" HeaderText="<%$ Resources:Attendance,Customer Id %>"
                                                                SortExpression="Customer_Code" />--%>
                                                            <%--<asp:BoundField DataField="Name" HeaderText="<%$ Resources:Attendance,Customer Name %>"
                                                                SortExpression="Name" />--%>

                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,Customer Name %>" SortExpression="Name">
                                                                <ItemTemplate>
                                                                    <asp:LinkButton ID="lBtnCustomerInfo" ToolTip="Click To See Contact Information" runat="server" Text='<%# Eval("Name").ToString() %>' CommandArgument='<%# Eval("Customer_Id") %>' OnCommand="lBtnCustomerInfo_Command"></asp:LinkButton>
                                                                </ItemTemplate>
                                                                <ItemStyle HorizontalAlign="Center" />
                                                            </asp:TemplateField>

                                                            <asp:BoundField DataField="Name_L" HeaderText="<%$ Resources:Attendance,Customer Name(Local) %>"
                                                                SortExpression="Name_L" />

                                                            <asp:BoundField DataField="Field1" HeaderText="<%$ Resources:Attendance,Email%>"
                                                                SortExpression="Field1" />

                                                            <asp:TemplateField HeaderText="Mobile No" SortExpression="Phone_no">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblMobileNo" runat="server" Text='<%# Eval("Phone_no") %>'></asp:Label><br />
                                                                    <asp:LinkButton ID="lBtnMoreNum" runat="server" Text='<%# Eval("More").ToString() %>' CommandArgument='<%# Eval("Customer_Id") %>' OnCommand="lBtnMoreNum_List_Command"></asp:LinkButton>
                                                                </ItemTemplate>
                                                                <ItemStyle HorizontalAlign="Center" />
                                                            </asp:TemplateField>

                                                            <asp:BoundField Visible="false" DataField="Account_No" HeaderText="<%$ Resources:Attendance,Account No %>"
                                                                SortExpression="Account_No" />




                                                            <asp:BoundField DataField="MarketingEmpName" HeaderText="Marketing By"
                                                                SortExpression="MarketingEmpName" />

                                                            <asp:BoundField DataField="Coutry_Name" HeaderText="<%$ Resources:Attendance,Country Name %>"
                                                                SortExpression="Coutry_Name" />





                                                           <%-- <asp:TemplateField HeaderText="<%$ Resources:Attendance,Credit Limit %>" SortExpression="Credit_Limit">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblgvCreditLimit" runat="server" Text='<%#  SystemParameter.GetAmountWithDecimal(Eval("Credit_Limit").ToString(),Session["LoginLocDecimalCount"].ToString()) %>' />
                                                                </ItemTemplate>
                                                                <ItemStyle HorizontalAlign="Center" />
                                                            </asp:TemplateField>
                                                            <asp:BoundField DataField="ApprovalStatus" HeaderText="<%$ Resources:Attendance,Status %>"
                                                                SortExpression="ApprovalStatus" />--%>
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
                                                        <asp:ImageButton ID="btnControlsSetting" ImageAlign="Right" ToolTip="Controls Setting" runat="server" ImageUrl="~/Images/setting.png" OnClick="btnControlsSetting_Click" Style="width: 32px; height: 32px" Visible="false" />
                                                    </div>
                                                    <div class="col-md-12">
                                                        <div class="col-md-6">
                                                            <asp:Label ID="lblId" runat="server" Text="<%$Resources:Attendance,Customer Id %>"></asp:Label>
                                                            <asp:TextBox ID="txtId" runat="server" CssClass="form-control" ReadOnly="true"></asp:TextBox>
                                                            <br />
                                                            <span id="ctlCustomerNameLocal" runat="server">
                                                                <asp:Label ID="lblLCustomerName" runat="server" Text="<%$ Resources:Attendance,Customer Name(Local) %>"></asp:Label>
                                                                <asp:TextBox ID="txtLCustomerName" runat="server" CssClass="form-control" BackColor="#eeeeee"></asp:TextBox>
                                                                <br />
                                                            </span>
                                                        </div>
                                                        <div class="col-md-6">
                                                            <asp:Label ID="lblCustomerName" runat="server" Text="<%$ Resources:Attendance,Customer Name %>"></asp:Label>
                                                            <a style="color: Red">*</a>
                                                            <asp:RequiredFieldValidator EnableClientScript="true" Style="float: right;" runat="server" ID="RequiredFieldValidator3" ValidationGroup="Cust_Save"
                                                                Display="Dynamic" SetFocusOnError="true" ControlToValidate="txtCustomerName" ErrorMessage="<%$ Resources:Attendance,Enter Customer Name %>"></asp:RequiredFieldValidator>

                                                            <asp:TextBox ID="txtCustomerName" runat="server" CssClass="form-control"
                                                                BackColor="#eeeeee"></asp:TextBox>

                                                            <cc1:AutoCompleteExtender ID="AutoCompleteExtender9" ServiceMethod="GetCompletionList"
                                                                runat="server" DelimiterCharacters="" Enabled="True" TargetControlID="txtCustomerName"
                                                                ServicePath="" CompletionInterval="100" MinimumPrefixLength="1" CompletionSetCount="1"
                                                                CompletionListCssClass="autoCompleteList" CompletionListItemCssClass="autoCompleteListItem"
                                                                CompletionListHighlightedItemCssClass="autoCompleteSelectedListItem" OnClientShown="resetPosition"
                                                                UseContextKey="True">
                                                            </cc1:AutoCompleteExtender>


                                                            <br />
                                                            <span id="ctlCivilId" runat="server">
                                                                <asp:Label ID="lblCivilId" runat="server" Text="<%$ Resources:Attendance,Civil Id %>"></asp:Label>
                                                                <asp:TextBox ID="txtCivilId" runat="server" CssClass="form-control"></asp:TextBox>
                                                            </span>
                                                            <br />
                                                        </div>
                                                    </div>
                                                    <div id="trlogo" runat="server" visible="false" class="col-md-12">
                                                        <asp:Label ID="lblUploadImage" runat="server" Text="<%$ Resources:Attendance,Upload Image %>"></asp:Label>
                                                        <asp:FileUpload ID="FileUploadImage" runat="server" />
                                                        <asp:ImageButton ID="ImgFileUploadAdd" runat="server" CausesValidation="False" ImageUrl="~/Images/add.png"
                                                            OnClick="ImgLogoAdd_Click" ToolTip="<%$ Resources:Attendance,Add %>" ImageAlign="Middle" />
                                                    </div>
                                                    <div class="col-md-12">
                                                        <div class="col-md-6">
                                                            <div style="overflow: auto">
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
                                                        <br />
                                                    </div>
                                                    <div class="col-md-12">
                                                        <div class="col-md-6">
                                                            <asp:Label ID="lblAddressName" runat="server" Text="<%$ Resources:Attendance,Address Name %>"></asp:Label>
                                                            <asp:RequiredFieldValidator EnableClientScript="true" Style="float: right;" runat="server" ID="RequiredFieldValidator1" ValidationGroup="Address"
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
                                                                    <asp:HiddenField ID="Hdn_Edit_ID" runat="server" />
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
                                                                                <asp:LinkButton ID="imgBtnAddressEdit" runat="server" CommandArgument='<%# Eval("Trans_Id") %>' OnCommand="btnAddressEdit_Command"><i class="fa fa-pencil"></i></asp:LinkButton>
                                                                            </ItemTemplate>
                                                                            <ItemStyle HorizontalAlign="Center" />
                                                                        </asp:TemplateField>
                                                                        <asp:TemplateField HeaderText="Delete">
                                                                            <ItemTemplate>
                                                                                <asp:LinkButton ID="imgBtnDelete" runat="server" CommandArgument='<%# Eval("Trans_Id") %>' OnCommand="btnAddressDelete_Command"><i class="fa fa-trash "></i></asp:LinkButton>
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
                                                            <asp:Label ID="Label8" runat="server" Text="<%$ Resources:Attendance,Bank Name %>"></asp:Label>
                                                            <%-- <asp:RequiredFieldValidator EnableClientScript="true" style="float:right;" runat="server" id="RequiredFieldValidator2" ValidationGroup="Bank_Save" 
        Display="Dynamic" SetFocusOnError="true" controltovalidate="txtContactBankName" errormessage="<%$ Resources:Attendance,Enter Bank Name %>" />--%>

                                                            <div class="input-group input-group-sm">
                                                                <asp:TextBox ID="txtContactBankName" AutoPostBack="true" BackColor="#eeeeee" OnTextChanged="txtContactBankName_TextChanged"
                                                                    runat="server" CssClass="form-control"></asp:TextBox>
                                                                <cc1:AutoCompleteExtender ID="AutoCompleteExtender10" ServiceMethod="GetCompletionListBankName"
                                                                    runat="server" DelimiterCharacters="" Enabled="True" TargetControlID="txtContactBankName"
                                                                    ServicePath="" CompletionInterval="100" MinimumPrefixLength="1" CompletionSetCount="1"
                                                                    CompletionListCssClass="autoCompleteList" CompletionListItemCssClass="autoCompleteListItem"
                                                                    CompletionListHighlightedItemCssClass="autoCompleteSelectedListItem" OnClientShown="resetPosition"
                                                                    UseContextKey="True">
                                                                </cc1:AutoCompleteExtender>
                                                                <span class="input-group-btn">
                                                                    <asp:Button ID="BtnContactBank" Style="margin-left: 10px;" OnClick="BtnContactBank_click"
                                                                        class="btn btn-info btn-flat" runat="server" Text="<%$ Resources:Attendance,Add %>" />
                                                                </span>
                                                            </div>
                                                            <br />
                                                        </div>
                                                    </div>
                                                    <div class="col-md-12">
                                                        <div class="col-md-12">
                                                            <div class="flow">
                                                                <asp:GridView CssClass="table-striped table-bordered table table-hover" ID="GvContactBankDetail" Width="100%" runat="server"
                                                                    AutoGenerateColumns="False">

                                                                    <Columns>
                                                                        <asp:TemplateField HeaderText="<%$ Resources:Attendance,Edit %>">
                                                                            <ItemTemplate>
                                                                                <asp:ImageButton ID="imgBtnContactBankEdit" runat="server" CommandArgument='<%# Eval("Trans_Id") %>'
                                                                                    ImageUrl="~/Images/edit.png" OnClientClick="show_Bank_Popup()" OnCommand="btnContactBankEdit_Command"
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
                                                    <div class="col-md-6" style="display: none">
                                                        <asp:Label ID="lblCurrency" runat="server" Text="<%$ Resources:Attendance,Currency Name %>"></asp:Label>
                                                        <asp:DropDownList ID="ddlCurrency" runat="server" class="form-control select2" Style="width: 100%;" />
                                                        <br />
                                                        <asp:Label ID="lblCustomerType" runat="server" Text="<%$ Resources:Attendance,Customer Type %>" />
                                                        <asp:DropDownList ID="ddlCustomerType" runat="server" class="form-control select2" Style="width: 100%;">
                                                            <asp:ListItem Text="--Select--" Value="0" />
                                                            <asp:ListItem Text="Reseller" Value="1" />
                                                            <asp:ListItem Text="Distributor" Value="2" />
                                                        </asp:DropDownList>
                                                        <br />
                                                    </div>
                                                    <div class="col-md-6" style="display: none">
                                                        <asp:Label ID="lblAccountNo" runat="server" Text="<%$ Resources:Attendance,Account Name %>" />
                                                        <asp:TextBox ID="txtAccountNo" ReadOnly="true" runat="server" CssClass="form-control"
                                                            AutoPostBack="true" OnTextChanged="txtAccountName_TextChanged" />
                                                        <cc1:AutoCompleteExtender ID="AutoCompleteExtender5" ServiceMethod="GetCompletionListAccountName"
                                                            runat="server" DelimiterCharacters="" Enabled="True" TargetControlID="txtAccountNo"
                                                            ServicePath="" CompletionInterval="100" MinimumPrefixLength="1" CompletionSetCount="1"
                                                            CompletionListCssClass="autoCompleteList" CompletionListItemCssClass="autoCompleteListItem"
                                                            CompletionListHighlightedItemCssClass="autoCompleteSelectedListItem" OnClientShown="resetPosition"
                                                            UseContextKey="True">
                                                        </cc1:AutoCompleteExtender>
                                                        <br />
                                                    </div>
                                                    <div class="col-md-12">
                                                        <div class="col-md-6">

                                                            <div id="ctlHandledEmp" runat="server">
                                                                <asp:Label ID="lblHandledEmployee" runat="server" Text="Sales Employee" />
                                                                <asp:TextBox ID="txtHandledEmployee" BackColor="#eeeeee" runat="server" CssClass="form-control"
                                                                    OnTextChanged="txtHandledEmployee_TextChanged" AutoPostBack="true" />
                                                                <cc1:AutoCompleteExtender ID="AutoCompleteExtender6" ServiceMethod="GetCompletionListEmployeeName"
                                                                    runat="server" DelimiterCharacters="" Enabled="True" TargetControlID="txtHandledEmployee"
                                                                    ServicePath="" CompletionInterval="100" MinimumPrefixLength="1" CompletionSetCount="1"
                                                                    CompletionListCssClass="autoCompleteList" CompletionListItemCssClass="autoCompleteListItem"
                                                                    CompletionListHighlightedItemCssClass="autoCompleteSelectedListItem" OnClientShown="resetPosition"
                                                                    UseContextKey="True">
                                                                </cc1:AutoCompleteExtender>
                                                                <br />
                                                            </div>
                                                        </div>
                                                        <div class="col-md-6">
                                                            <div id="ctlFoundEmployee" runat="server">
                                                                <asp:Label ID="lblFoundEmployee" runat="server" Text="<%$ Resources:Attendance,Found Employee %>" />
                                                                <asp:TextBox ID="txtFoundEmployee" BackColor="#eeeeee" runat="server" CssClass="form-control"
                                                                    OnTextChanged="txtFoundEmployee_TextChanged" AutoPostBack="true" />
                                                                <cc1:AutoCompleteExtender ID="AutoCompleteExtender11" ServiceMethod="GetCompletionListEmployeeName"
                                                                    runat="server" DelimiterCharacters="" Enabled="True" TargetControlID="txtFoundEmployee"
                                                                    ServicePath="" CompletionInterval="100" MinimumPrefixLength="1" CompletionSetCount="1"
                                                                    CompletionListCssClass="autoCompleteList" CompletionListItemCssClass="autoCompleteListItem"
                                                                    CompletionListHighlightedItemCssClass="autoCompleteSelectedListItem" OnClientShown="resetPosition"
                                                                    UseContextKey="True">
                                                                </cc1:AutoCompleteExtender>
                                                                <br />
                                                            </div>
                                                            <div id="ctlMarketingEmp" runat="server">
                                                                <asp:Label ID="lblMarketingEmp" runat="server" Text="Marketing Employee"></asp:Label>
                                                                <br />
                                                                <asp:TextBox ID="txtMarketingEmp" BackColor="#eeeeee" BorderColor="#eeeeee" runat="server" CssClass="col-md-8"></asp:TextBox>
                                                                <cc1:AutoCompleteExtender ID="AutoCompleteExtender7" ServiceMethod="GetCompletionListEmployeeName"
                                                                    runat="server" DelimiterCharacters="" Enabled="True" TargetControlID="txtMarketingEmp"
                                                                    ServicePath="" CompletionInterval="100" MinimumPrefixLength="1" CompletionSetCount="1"
                                                                    CompletionListCssClass="autoCompleteList" CompletionListItemCssClass="autoCompleteListItem"
                                                                    CompletionListHighlightedItemCssClass="autoCompleteSelectedListItem" OnClientShown="resetPosition"
                                                                    UseContextKey="True">
                                                                </cc1:AutoCompleteExtender>
                                                                &nbsp;
                                                                <asp:Button ID="btnNewEmployee" runat="server" CssClass="btn btn-primary" Text="New" OnClientClick="CreateNewEmployee(); return false;" />
                                                                
                                                                <br />
                                                            </div>
                                                        </div>
                                                    </div>

                                                    <div class="col-md-12">

                                                        <div class="col-md-6">
                                                        </div>
                                                    </div>
                                                    <div id="tr1" visible="false" runat="server" class="col-md-12">
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
                                                    <div id="tr2" visible="false" runat="server" class="col-md-12">
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
                                                    <div class="col-md-12">
                                                        <div class="col-md-6">
                                                            <asp:Label ID="lblSalesQuota" runat="server" Text="<%$ Resources:Attendance,Sales Quota %>" />
                                                            <asp:TextBox ID="txtSalesQuota" runat="server" CssClass="form-control" />
                                                            <br />
                                                        </div>
                                                        <div class="col-md-6">
                                                            <asp:Label ID="lblPriceLevel" runat="server" Text="<%$ Resources:Attendance,Price Level %>" />
                                                            <asp:TextBox ID="txtPriceLevel" runat="server" CssClass="form-control" />
                                                            <br />
                                                        </div>
                                                    </div>
                                                    <div class="col-md-12">
                                                        <div class="col-md-6">
                                                            <asp:Label ID="Label25" runat="server" Text="<%$ Resources:Attendance,Sales Price %>" />
                                                            <td colspan="3">
                                                                <asp:RadioButton ID="rbtnSystemsalesprice" runat="server"
                                                                    Text="<%$Resources:Attendance,System Sales Price %>" OnCheckedChanged="rbtnSystemsalesprice_CheckedChanged"
                                                                    AutoPostBack="true" GroupName="a" />&nbsp;
                                                <asp:RadioButton ID="rbtnCustomerpricelist" runat="server"
                                                    Text="<%$Resources:Attendance,Customer Price List %>" OnCheckedChanged="rbtnCustomerpricelist_CheckedChanged"
                                                    AutoPostBack="true" GroupName="a" />
                                                                &nbsp;
                                                <asp:Button ID="BtnShowCpriceList" runat="server" Text="<%$ Resources:Attendance,Customer Price List %>"
                                                    CssClass="btn btn-primary" OnClick="BtnShowCpriceList_click" OnClientClick="show_Customer_Price_Popup()" />
                                                                <br />
                                                        </div>
                                                        <div class="col-md-6" runat="server" id="ctlGrade">
                                                            <asp:Label ID="lblGrade" runat="server" Text="Customer Grade"></asp:Label>
                                                            <cc1:Rating ID="ratingControl" OnChanged="ratingControl_Changed" AutoPostBack="true" runat="server" StarCssClass="ratingEmpty" WaitingStarCssClass="ratingSaved" EmptyStarCssClass="ratingEmpty" FilledStarCssClass="ratingFilled"></cc1:Rating>

                                                            <asp:CheckBox ID="chkIsTaxable" Visible="false" runat="server" Text="<%$ Resources:Attendance,Is Taxable %>" />
                                                            <%--   <asp:DropDownList ID="ddlGrade" runat="server" CssClass="form-control">
                                                                <asp:ListItem Text="Select"></asp:ListItem>
                                                                <asp:ListItem Text="A"></asp:ListItem>
                                                                <asp:ListItem Text="B"></asp:ListItem>
                                                                <asp:ListItem Text="C"></asp:ListItem>
                                                                <asp:ListItem Text="D"></asp:ListItem>
                                                            </asp:DropDownList>--%>
                                                            <br />
                                                        </div>
                                                    </div>
                                                    <%--<div class="col-md-12">--%>
                                                    <div class="col-md-6" runat="server" id="ctlIsCredit">
                                                        <br />
                                                        <asp:Label ID="Label79" runat="server" Text="Is Credit"></asp:Label>
                                                        <asp:DropDownList ID="ddlIsCredit" runat="server" class="form-control select2" Style="width: 100%;" Enabled="false">
                                                            <asp:ListItem Text="False" Value="False" Selected="True"></asp:ListItem>
                                                            <asp:ListItem Text="True" Value="True"></asp:ListItem>
                                                        </asp:DropDownList>
                                                        <br />
                                                    </div>
                                                    <div class="col-md-6" runat="server" id="ctlCurrentBalance">
                                                        <br />
                                                        <asp:Label ID="Label1" runat="server" Text="Current Balance"></asp:Label>
                                                        <asp:LinkButton ID="lnkCustomerBalance" runat="server" Text='0.00' OnCommand="lnkCustomerBalance_Command"> </asp:LinkButton>
                                                        <br />
                                                    </div>


                                                    <%--</div>--%>
                                                    <div class="col-md-12">
                                                        <cc1:TabContainer ID="TabContainer2" runat="server" ActiveTabIndex="0" Width="100%"
                                                            CssClass="ajax__tab_yuitabview-theme">
                                                            <cc1:TabPanel ID="TabProductPaymentMode" runat="server" HeaderText="Customer Detail">
                                                                <ContentTemplate>
                                                                    <div class="row">

                                                                        <div class="col-md-12">
                                                                            <div id="Div_Company_Detail" class="box box-primary">
                                                                                <div class="box-header with-border">
                                                                                    <h3 class="box-title">
                                                                                        <asp:Label ID="Label3" runat="server" CssClass="LableHeaderTitle" ForeColor="Gray"
                                                                                            Font-Bold="true" Text="Company Detail"></asp:Label></h3>
                                                                                    <div class="box-tools pull-right">
                                                                                        <button type="button" class="btn btn-box-tool" data-widget="collapse">
                                                                                            <i id="Btn_Company_Detail" class="fa fa-minus"></i>
                                                                                        </button>
                                                                                    </div>
                                                                                </div>
                                                                                <div class="box-body">
                                                                                    <div class="form-group">
                                                                                        <div class="col-md-12">
                                                                                        </div>
                                                                                        <div class="col-md-12">
                                                                                            <div class="col-md-12">
                                                                                                <asp:Label ID="Label4" runat="server" Text="<%$ Resources:Attendance,Company Name %>" />
                                                                                                <asp:TextBox ID="txtCompanyName" runat="server" CssClass="form-control" BackColor="#eeeeee"
                                                                                                    OnTextChanged="txtCompanyName_TextChanged" AutoPostBack="true" /><a style="color: Red;">*</a>
                                                                                                <cc1:AutoCompleteExtender ID="AutoCompleteExtender4" ServiceMethod="GetCompletionListCustomer"
                                                                                                    runat="server" DelimiterCharacters="" Enabled="True" TargetControlID="txtCompanyName"
                                                                                                    ServicePath="" CompletionInterval="100" MinimumPrefixLength="1" CompletionSetCount="1"
                                                                                                    CompletionListCssClass="autoCompleteList" CompletionListItemCssClass="autoCompleteListItem"
                                                                                                    CompletionListHighlightedItemCssClass="autoCompleteSelectedListItem" OnClientShown="resetPosition"
                                                                                                    UseContextKey="True">
                                                                                                </cc1:AutoCompleteExtender>
                                                                                            </div>
                                                                                        </div>
                                                                                        <div class="col-md-12">
                                                                                            <div class="col-md-6">
                                                                                                <asp:Label ID="Label234" runat="server" Text="<%$ Resources:Attendance,Address%>" />
                                                                                                <div class="input-group-btn">
                                                                                                    <asp:TextBox ID="txtCompanyAddress" runat="server" CssClass="form-control" OnTextChanged="txtCompanyAddress_TextChanged" AutoPostBack="true" />
                                                                                                    <cc1:AutoCompleteExtender ID="AutoCompleteExtender13" runat="server" CompletionInterval="100"
                                                                                                        CompletionSetCount="1" DelimiterCharacters="" Enabled="True" MinimumPrefixLength="1"
                                                                                                        ServiceMethod="GetCompletionListAddressName" ServicePath="" TargetControlID="txtCompanyAddress"
                                                                                                        UseContextKey="True" CompletionListCssClass="autoCompleteList" CompletionListItemCssClass="autoCompleteListItem" CompletionListHighlightedItemCssClass="autoCompleteSelectedListItem" OnClientShown="resetPosition">
                                                                                                    </cc1:AutoCompleteExtender>

                                                                                                    <asp:Button ID="btnAddNewAddress1" OnClick="btnAddNewAddress_Click" CssClass="btn btn-info"
                                                                                                        runat="server" Text="<%$ Resources:Attendance,New %>" />
                                                                                                </div>
                                                                                            </div>
                                                                                        </div>
                                                                                        <div class="col-md-12">
                                                                                            <div class="col-md-6" id="ctlMobileNo" runat="server">
                                                                                                <asp:Label ID="lblPermanentMobileNo" runat="server" Text="<%$ Resources:Attendance,Permanent MobileNo.%>" />
                                                                                                <div class="input-group">
                                                                                                    <div class="input-group-btn">
                                                                                                        <asp:DropDownList ID="ddlCompanyMobileNoCountryCode" Style="width: 150px;" runat="server" class="form-control select2"></asp:DropDownList>
                                                                                                    </div>
                                                                                                    <asp:TextBox ID="txtCompanyPermanentMobileNo" runat="server" class="form-control"></asp:TextBox>
                                                                                                    <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender111" runat="server" Enabled="true" TargetControlID="txtCompanyPermanentMobileNo" FilterType="Numbers"></cc1:FilteredTextBoxExtender>
                                                                                                    <cc1:AutoCompleteExtender ID="AutoCompleteExtender18" runat="server" DelimiterCharacters=""
                                                                                                        Enabled="True" ServiceMethod="GetCompletionListContactNumber" ServicePath="" CompletionInterval="100"
                                                                                                        MinimumPrefixLength="1" CompletionSetCount="1" TargetControlID="txtCompanyPermanentMobileNo"
                                                                                                        UseContextKey="True" CompletionListCssClass="autoCompleteList" CompletionListItemCssClass="autoCompleteListItem" CompletionListHighlightedItemCssClass="autoCompleteSelectedListItem" OnClientShown="resetPosition1">
                                                                                                    </cc1:AutoCompleteExtender>
                                                                                                </div>
                                                                                                <%-- <div style="width:100%;" class="input-group">
                                                                        <asp:DropDownList  ID="" style="width:30%;" runat="server" CssClass="form-control">
                                                                        </asp:DropDownList>
                                                                        <asp:TextBox ID="" style="width:70%;" runat="server" CssClass="form-control"></asp:TextBox>
                                                                        <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" runat="server" Enabled="True"
                                                                            TargetControlID="txtCompanyPermanentMobileNo" FilterType="Numbers">
                                                                        </cc1:FilteredTextBoxExtender>
                                                                </div>--%>
                                                                                                <br />
                                                                                                <div id="ctlEmail" runat="server">
                                                                                                    <asp:Label ID="Label238" runat="server" Text="<%$ Resources:Attendance,Email Id%>" />
                                                                                                    <asp:TextBox ID="txtCompanyEmailId" runat="server" CssClass="form-control" />
                                                                                                    <cc1:AutoCompleteExtender ID="AutoCompleteExtender23" runat="server" DelimiterCharacters=""
                                                                                                        Enabled="True" ServiceMethod="GetCompletionListEmailMaster" ServicePath="" CompletionInterval="100"
                                                                                                        MinimumPrefixLength="1" CompletionSetCount="1" TargetControlID="txtCompanyEmailId"
                                                                                                        UseContextKey="True" CompletionListCssClass="autoCompleteList" CompletionListItemCssClass="autoCompleteListItem" CompletionListHighlightedItemCssClass="autoCompleteSelectedListItem" OnClientShown="resetPosition">
                                                                                                    </cc1:AutoCompleteExtender>
                                                                                                    <br />
                                                                                                </div>
                                                                                                <asp:Label ID="Label13" runat="server" Text="Nature Of Business" />
                                                                                                <asp:DropDownList ID="ddlBusinessNature" runat="server" class="form-control select2" Style="width: 100%;">
                                                                                                    <asp:ListItem Text="Corporate" Value="Corporate"></asp:ListItem>
                                                                                                    <asp:ListItem Text="Limited" Value="Limited"></asp:ListItem>
                                                                                                    <asp:ListItem Text="Partner Ship" Value="Partner Ship"></asp:ListItem>
                                                                                                    <asp:ListItem Text="Proprietor ship" Value="Proprietor ship"></asp:ListItem>
                                                                                                    <asp:ListItem Text="Extra" Value="Extra"></asp:ListItem>
                                                                                                </asp:DropDownList>
                                                                                                <br />
                                                                                            </div>
                                                                                            <div class="col-md-6">
                                                                                                <asp:Label ID="Label7" runat="server" Text="<%$ Resources:Attendance,Fax No.%>" />
                                                                                                <div style="width: 100%;" class="input-group">
                                                                                                    <asp:DropDownList Style="width: 30%;" ID="ddlCompanyFaxNoCountryCode" runat="server" class="form-control select2">
                                                                                                    </asp:DropDownList>
                                                                                                    <asp:TextBox ID="txtCompanyFaxNo" Style="width: 70%;" runat="server" CssClass="form-control"></asp:TextBox>
                                                                                                    <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender2" runat="server" Enabled="True"
                                                                                                        TargetControlID="txtCompanyFaxNo" FilterType="Numbers">
                                                                                                    </cc1:FilteredTextBoxExtender>
                                                                                                    <cc1:AutoCompleteExtender ID="AutoCompleteExtender22" runat="server" DelimiterCharacters=""
                                                                                                        Enabled="True" ServiceMethod="GetCompletionListContactNumber" ServicePath="" CompletionInterval="100"
                                                                                                        MinimumPrefixLength="1" CompletionSetCount="1" TargetControlID="txtCompanyFaxNo"
                                                                                                        UseContextKey="True" CompletionListCssClass="autoCompleteList" CompletionListItemCssClass="autoCompleteListItem" CompletionListHighlightedItemCssClass="autoCompleteSelectedListItem" OnClientShown="resetPosition1">
                                                                                                    </cc1:AutoCompleteExtender>
                                                                                                </div>
                                                                                                <br />
                                                                                                <asp:Label ID="Label12" runat="server" Text="<%$ Resources:Attendance,Website%>"></asp:Label>
                                                                                                <asp:TextBox ID="txtCompanyWebsite" runat="server" CssClass="form-control"></asp:TextBox>
                                                                                                <br />
                                                                                            </div>
                                                                                        </div>
                                                                                    </div>
                                                                                </div>
                                                                            </div>
                                                                        </div>

                                                                        <div class="col-md-12">
                                                                            <div id="Div_Company_Owner_or_Sponser_detail" class="box box-primary collapsed-box">
                                                                                <div class="box-header with-border">
                                                                                    <h3 class="box-title">
                                                                                        <asp:Label ID="Label14" runat="server" CssClass="LableHeaderTitle" ForeColor="Gray"
                                                                                            Font-Bold="true" Text="Company Owner or Sponser detail"></asp:Label></h3>
                                                                                    <div class="box-tools pull-right">
                                                                                        <button type="button" class="btn btn-box-tool" data-widget="collapse">
                                                                                            <i id="Btn_Company_Owner_or_Sponser_detail" class="fa fa-plus"></i>
                                                                                        </button>
                                                                                    </div>
                                                                                </div>
                                                                                <div class="box-body">
                                                                                    <div class="form-group">
                                                                                        <div class="col-md-12">
                                                                                            <div class="col-md-6">
                                                                                                <asp:Label ID="Label15" runat="server" Text="Contact Person(Accounts)" />
                                                                                                <asp:TextBox ID="txtContactPerson_Accounts" runat="server" CssClass="form-control"
                                                                                                    BackColor="#eeeeee" OnTextChanged="txtContactPerson_Accounts_TextChanged" AutoPostBack="true" />
                                                                                                <cc1:AutoCompleteExtender ID="AutoCompleteExtender1" ServiceMethod="GetContactListCustomer"
                                                                                                    runat="server" DelimiterCharacters="" Enabled="True" TargetControlID="txtContactPerson_Accounts"
                                                                                                    ServicePath="" CompletionInterval="100" MinimumPrefixLength="1" CompletionSetCount="1"
                                                                                                    CompletionListCssClass="autoCompleteList" CompletionListItemCssClass="autoCompleteListItem"
                                                                                                    CompletionListHighlightedItemCssClass="autoCompleteSelectedListItem" OnClientShown="resetPosition"
                                                                                                    UseContextKey="True">
                                                                                                </cc1:AutoCompleteExtender>
                                                                                                <br />
                                                                                                <asp:Label ID="Label19" runat="server" Text="Contact No.(Accounts)" />
                                                                                                <div style="width: 100%;" class="input-group">
                                                                                                    <asp:DropDownList Style="width: 30%;" ID="ddlContactNo_Accounts_CountryCode" runat="server" class="form-control select2">
                                                                                                    </asp:DropDownList>
                                                                                                    <asp:TextBox ID="txtContactNo_Accounts" Style="width: 70%;" runat="server" CssClass="form-control" Enabled="false" />
                                                                                                    <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender3" runat="server" Enabled="True"
                                                                                                        TargetControlID="txtContactNo_Accounts" FilterType="Numbers">
                                                                                                    </cc1:FilteredTextBoxExtender>
                                                                                                </div>
                                                                                                <br />
                                                                                                <asp:Label ID="Label29" runat="server" Text="Email Id(Accounts)" />
                                                                                                <asp:TextBox ID="txtEmailId_Accounts" runat="server" CssClass="form-control" Enabled="false" />
                                                                                                <br />
                                                                                            </div>
                                                                                            <div class="col-md-6">
                                                                                                <asp:Label ID="Label17" runat="server" Text="Contact Person(Sales)" />
                                                                                                <asp:TextBox ID="txtContactPerson_Sales" runat="server" CssClass="form-control" BackColor="#eeeeee"
                                                                                                    OnTextChanged="txtContactPerson_Sales_TextChanged" AutoPostBack="true" />
                                                                                                <cc1:AutoCompleteExtender ID="AutoCompleteExtender12" ServiceMethod="GetContactListCustomer"
                                                                                                    runat="server" DelimiterCharacters="" Enabled="True" TargetControlID="txtContactPerson_Sales"
                                                                                                    ServicePath="" CompletionInterval="100" MinimumPrefixLength="1" CompletionSetCount="1"
                                                                                                    CompletionListCssClass="autoCompleteList" CompletionListItemCssClass="autoCompleteListItem"
                                                                                                    CompletionListHighlightedItemCssClass="autoCompleteSelectedListItem" OnClientShown="resetPosition"
                                                                                                    UseContextKey="True">
                                                                                                </cc1:AutoCompleteExtender>
                                                                                                <br />
                                                                                                <asp:Label ID="Label28" runat="server" Text="Contact No.(Sales)" />
                                                                                                <div style="width: 100%" class="input-group">
                                                                                                    <asp:DropDownList Style="width: 30%" ID="ddlContactNo_Sales_CountryCode" runat="server" class="form-control select2">
                                                                                                    </asp:DropDownList>
                                                                                                    <asp:TextBox ID="txtContactNo_Sales" Style="width: 70%" runat="server" CssClass="form-control" Enabled="false" />
                                                                                                    <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender4" runat="server" Enabled="True"
                                                                                                        TargetControlID="txtContactNo_Sales" FilterType="Numbers">
                                                                                                    </cc1:FilteredTextBoxExtender>
                                                                                                </div>
                                                                                                <br />
                                                                                                <asp:Label ID="Label31" runat="server" Text="Email Id(Sales)" />
                                                                                                <asp:TextBox ID="txtEmailId_Sales" runat="server" CssClass="form-control" Enabled="false" />
                                                                                                <br />
                                                                                            </div>
                                                                                        </div>
                                                                                    </div>
                                                                                </div>
                                                                            </div>
                                                                        </div>

                                                                        <div class="col-md-12" style="display: none">
                                                                            <div id="Div_Credit_Approval" class="box box-primary collapsed-box">
                                                                                <div class="box-header with-border">
                                                                                    <h3 class="box-title">
                                                                                        <asp:Label ID="Label32" runat="server" CssClass="LableHeaderTitle" ForeColor="Gray"
                                                                                            Font-Bold="true" Text="Credit Approval"></asp:Label></h3>
                                                                                    <div class="box-tools pull-right">
                                                                                        <button type="button" class="btn btn-box-tool" data-widget="collapse">
                                                                                            <i id="Btn_Credit_Approval" class="fa fa-plus"></i>
                                                                                        </button>
                                                                                    </div>
                                                                                </div>
                                                                                <div class="box-body">
                                                                                    <div class="form-group">
                                                                                        <div class="col-md-12">
                                                                                            <div class="col-md-6">
                                                                                                <asp:Label ID="lblCreditLimit" runat="server" Text="<%$ Resources:Attendance,Credit Limit %>" />
                                                                                                <div style="width: 100%" class="input-group">
                                                                                                    <asp:TextBox ID="txtCreditLimit" Style="width: 40%" runat="server" CssClass="form-control" />
                                                                                                    <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender15txtCreditLimit" runat="server"
                                                                                                        Enabled="True" TargetControlID="txtCreditLimit" ValidChars="1,2,3,4,5,6,7,8,9,0,.">
                                                                                                    </cc1:FilteredTextBoxExtender>
                                                                                                    <asp:DropDownList Style="width: 60%" ID="ddlCurrencyCreditLimit" runat="server" class="form-control select2" />
                                                                                                </div>
                                                                                                <br />
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
                                                                                            <div class="col-md-12">
                                                                                                <asp:Label ID="Label33" runat="server" Text="Credit Parameter" />

                                                                                                <asp:RadioButton ID="rbtnAdvanceCheque" runat="server" Text="Advance Cheque Basis"
                                                                                                    GroupName="Parameter" />
                                                                                                <asp:RadioButton ID="rbtnInvoicetoInvoice" runat="server" GroupName="Parameter" Text="Invoice to Invoice(First Clear Old Invoice than Issue New invoice)" />
                                                                                                <br />
                                                                                                <asp:RadioButton ID="rbtnAdvanceHalfpayment" GroupName="Parameter" runat="server"
                                                                                                    Text="50% Advance and 50% on Delivery" />
                                                                                                <asp:RadioButton ID="rbtnNone" GroupName="Parameter" runat="server" Checked="true"
                                                                                                    Text="None" />
                                                                                                <br />
                                                                                            </div>
                                                                                        </div>
                                                                                        <div class="col-md-12">
                                                                                            <div class="col-md-6">
                                                                                                <br />
                                                                                                <asp:Label ID="Label37" runat="server" Text="Financial Statement" />
                                                                                                <div class="input-group" style="width: 100%;">
                                                                                                    <cc1:AsyncFileUpload ID="FileUploadFinancilaStatement"
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
                                                                                            <div class="col-md-6">
                                                                                                <br />
                                                                                                <asp:Button ID="btnuploadFiancialstatement" Style="margin-top: 17px;" runat="server" Text="<%$ Resources:Attendance,Load %>"
                                                                                                    CssClass="btn btn-primary" OnClick="btnuploadFiancialstatement_Click" />&nbsp;&nbsp;
                                                                <asp:LinkButton ID="lnkDownloadFiancialstatement" runat="server" ToolTip="Download"
                                                                    OnClick="btnDownloadFiancialstatement_Click"
                                                                    ForeColor="Blue" />
                                                                                                <br />
                                                                                            </div>
                                                                                        </div>
                                                                                    </div>
                                                                                </div>
                                                                            </div>
                                                                        </div>

                                                                        <div class="col-md-12">
                                                                            <div class="box box-primary collapsed-box">
                                                                                <div class="box-header with-border">
                                                                                    <h3 class="box-title">
                                                                                        <asp:Label ID="Label34" runat="server" CssClass="LableHeaderTitle" ForeColor="Gray"
                                                                                            Font-Bold="true" Text="Black List"></asp:Label></h3>
                                                                                    <div class="box-tools pull-right">
                                                                                        <button type="button" class="btn btn-box-tool" data-widget="collapse">
                                                                                            <i class="fa fa-plus"></i>
                                                                                        </button>
                                                                                    </div>
                                                                                </div>
                                                                                <div class="box-body">
                                                                                    <div class="form-group">
                                                                                        <div class="col-md-12">
                                                                                            <div class="col-md-2">
                                                                                                <asp:CheckBox ID="chkBlockReason" runat="server" Text="Is Block" />
                                                                                                <br />
                                                                                            </div>
                                                                                            <div class="col-md-10">
                                                                                                <asp:TextBox ID="txtBlockReason" runat="server" TextMode="MultiLine" CssClass="form-control" />
                                                                                                <cc1:TextBoxWatermarkExtender ID="txtWater" runat="server" TargetControlID="txtBlockReason"
                                                                                                    WatermarkText="Enter Reason">
                                                                                                </cc1:TextBoxWatermarkExtender>
                                                                                                <br />
                                                                                            </div>
                                                                                        </div>
                                                                                    </div>
                                                                                </div>
                                                                            </div>
                                                                        </div>

                                                                        <div class="col-md-12">
                                                                            <div id="Div_Personal_Authorized_Signature_For_purchase_Order" class="box box-primary collapsed-box">
                                                                                <div class="box-header with-border">
                                                                                    <h3 class="box-title">
                                                                                        <asp:Label ID="Label35" runat="server" CssClass="LableHeaderTitle" ForeColor="Gray"
                                                                                            Font-Bold="true" Text="Personal Authorized Signature For purchase Order"></asp:Label></h3>
                                                                                    <div class="box-tools pull-right">
                                                                                        <button type="button" class="btn btn-box-tool" data-widget="collapse">
                                                                                            <i id="Btn_Personal_Authorized_Signature_For_purchase_Order" class="fa fa-plus"></i>
                                                                                        </button>
                                                                                    </div>
                                                                                </div>
                                                                                <div class="box-body">
                                                                                    <div class="form-group">
                                                                                        <div class="col-md-12">
                                                                                            <div class="col-md-6">
                                                                                                <asp:Label ID="Label36" runat="server" Text="Name" />
                                                                                                <asp:TextBox ID="txtAuthorizedpersonName" runat="server" CssClass="form-control" AutoPostBack="true" OnTextChanged="txtAuthorizedpersonName_TextChanged" />
                                                                                                <cc1:AutoCompleteExtender ID="AutoCompleteExtender14" ServiceMethod="GetContactListCustomer"
                                                                                                    runat="server" DelimiterCharacters="" Enabled="True" TargetControlID="txtAuthorizedpersonName"
                                                                                                    ServicePath="" CompletionInterval="100" MinimumPrefixLength="1" CompletionSetCount="1"
                                                                                                    CompletionListCssClass="autoCompleteList" CompletionListItemCssClass="autoCompleteListItem"
                                                                                                    CompletionListHighlightedItemCssClass="autoCompleteSelectedListItem" OnClientShown="resetPosition"
                                                                                                    UseContextKey="True">
                                                                                                </cc1:AutoCompleteExtender>
                                                                                                <br />
                                                                                                <div style="display: none">
                                                                                                    <asp:Label ID="Label39" runat="server" Text="Designation" />
                                                                                                    <asp:TextBox ID="txtDesignationName" runat="server" Font-Names="Verdana" AutoPostBack="true"
                                                                                                        OnTextChanged="txtDesignationName_TextChanged" CssClass="form-control" BackColor="#eeeeee"></asp:TextBox>
                                                                                                    <cc1:AutoCompleteExtender ID="AutoCompleteExtender3" ServiceMethod="GetCompletionListDesg"
                                                                                                        runat="server" DelimiterCharacters="" Enabled="True" TargetControlID="txtDesignationName"
                                                                                                        ServicePath="" CompletionInterval="100" MinimumPrefixLength="1" CompletionSetCount="1"
                                                                                                        CompletionListCssClass="autoCompleteList" CompletionListItemCssClass="autoCompleteListItem"
                                                                                                        CompletionListHighlightedItemCssClass="autoCompleteSelectedListItem" OnClientShown="resetPosition"
                                                                                                        UseContextKey="True">
                                                                                                    </cc1:AutoCompleteExtender>
                                                                                                    <br />
                                                                                                </div>
                                                                                            </div>
                                                                                            <div class="col-md-6" style="text-align: center">
                                                                                                <asp:Label ID="lblSingnature" runat="server" Text="<%$ Resources:Attendance,Signature  %>"></asp:Label>
                                                                                                <div class="input-group" style="width: 100%;">
                                                                                                    <cc1:AsyncFileUpload ID="FULogoPath"
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
                                                                                                        <asp:LinkButton ID="FULogo_Img_Right" runat="server" Width="30px" Height="30px" Style="display: none"><i class="fa fa-check" style="font-size:30px;color:#22cb33"></i></asp:LinkButton>
                                                                                                        <asp:LinkButton ID="FULogo_Img_Wrong" runat="server" Width="30px" Height="30px" Style="display: none"><i class="fa fa-remove" style="font-size:30px"></i></asp:LinkButton>
                                                                                                        <asp:Image ID="FULogo_ImgLoader" runat="server" ImageUrl="../Images/loader.gif" />
                                                                                                    </div>
                                                                                                </div>
                                                                                                <br />
                                                                                                <asp:Image ID="imgLogo" Width="90px" ImageUrl="../Bootstrap_Files/dist/img/NoImage.jpg" Height="120px" runat="server" />
                                                                                                <br />
                                                                                                <br />
                                                                                                <asp:Button ID="btnUpload" runat="server" Text="<%$ Resources:Attendance,Load %>"
                                                                                                    CssClass="btn btn-primary" OnClick="btnUpload1_Click" />
                                                                                                <asp:Button ID="btndownlodSignature" runat="server" Text="Download"
                                                                                                    CssClass="btn btn-primary" OnClick="btndownlodSignature_Click" />
                                                                                                <br />
                                                                                            </div>
                                                                                        </div>
                                                                                    </div>
                                                                                </div>
                                                                            </div>
                                                                        </div>

                                                                        <div class="col-md-12">
                                                                            <div class="box box-primary collapsed-box">
                                                                                <div class="box-header with-border">
                                                                                    <h3 class="box-title">
                                                                                        <asp:Label ID="Label38" runat="server" CssClass="LableHeaderTitle" ForeColor="Gray"
                                                                                            Font-Bold="true" Text="Trade Reference"></asp:Label></h3>
                                                                                    <div class="box-tools pull-right">
                                                                                        <button type="button" class="btn btn-box-tool" data-widget="collapse">
                                                                                            <i class="fa fa-plus"></i>
                                                                                        </button>
                                                                                    </div>
                                                                                </div>
                                                                                <div class="box-body">
                                                                                    <div class="form-group">
                                                                                        <div class="col-md-12">
                                                                                            <div class="col-md-4">
                                                                                                <asp:Label ID="Label55" runat="server" Text="Supplier 1" Font-Bold="true" />
                                                                                                <br />
                                                                                                <asp:Label ID="Label40" runat="server" Text="<%$ Resources:Attendance,Company Name %>" />
                                                                                                <asp:TextBox ID="txtSupplier1CompanyName" runat="server" CssClass="form-control" onchange="validateContactPerson(this)" />
                                                                                                <cc1:AutoCompleteExtender ID="AutoCompleteExtender15" runat="server" CompletionInterval="100"
                                                                                                    DelimiterCharacters="" Enabled="True" MinimumPrefixLength="1" ServiceMethod="GetCompletionList_Contacts"
                                                                                                    ServicePath="" TargetControlID="txtSupplier1CompanyName" UseContextKey="True" CompletionListCssClass="autoCompleteList" CompletionListItemCssClass="autoCompleteListItem" CompletionListHighlightedItemCssClass="autoCompleteSelectedListItem" OnClientShown="resetPosition">
                                                                                                </cc1:AutoCompleteExtender>
                                                                                                <br />
                                                                                                <asp:Label ID="Label41" runat="server" Text="Contact Person" />
                                                                                                <asp:TextBox ID="txtSupplier1ContactPerson" runat="server" CssClass="form-control" onchange="validateContactPerson(this)" />
                                                                                                <cc1:AutoCompleteExtender ID="AutoCompleteExtender20" runat="server" CompletionInterval="100"
                                                                                                    DelimiterCharacters="" Enabled="True" MinimumPrefixLength="1" ServiceMethod="GetCompletionList_Contacts"
                                                                                                    ServicePath="" TargetControlID="txtSupplier1ContactPerson" UseContextKey="True" CompletionListCssClass="autoCompleteList" CompletionListItemCssClass="autoCompleteListItem" CompletionListHighlightedItemCssClass="autoCompleteSelectedListItem" OnClientShown="resetPosition">
                                                                                                </cc1:AutoCompleteExtender>
                                                                                                <br />
                                                                                                <div style="display: none">
                                                                                                    <asp:Label ID="Label44" runat="server" Text="Contact No." />
                                                                                                    <div style="width: 100%" class="input-group">
                                                                                                        <asp:DropDownList ID="ddlSupplier1ContactNo_CountryCode" Enabled="false" Style="width: 30%" runat="server" class="form-control select2">
                                                                                                        </asp:DropDownList>
                                                                                                        <asp:TextBox ID="txtSupplier1ContactNo" Style="width: 70%" runat="server" Enabled="false" CssClass="form-control" />
                                                                                                        <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender5" runat="server" Enabled="True"
                                                                                                            TargetControlID="txtSupplier1ContactNo" FilterType="Numbers">
                                                                                                        </cc1:FilteredTextBoxExtender>
                                                                                                    </div>
                                                                                                    <br />
                                                                                                    <asp:Label ID="Label49" runat="server" Text="Fax No." />
                                                                                                    <div style="width: 100%" class="input-group">
                                                                                                        <asp:DropDownList ID="ddlSupplier1FaxNo_CountryCode" Style="width: 30%" runat="server" Enabled="false" class="form-control select2">
                                                                                                        </asp:DropDownList>
                                                                                                        <asp:TextBox ID="txtSupplier1FaxNo" runat="server" Style="width: 70%" Enabled="false" CssClass="form-control" />
                                                                                                        <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender8" runat="server" Enabled="True"
                                                                                                            TargetControlID="txtSupplier1FaxNo" FilterType="Numbers">
                                                                                                        </cc1:FilteredTextBoxExtender>
                                                                                                    </div>
                                                                                                    <br />
                                                                                                    <asp:Label ID="Label52" runat="server" Text="Email Id" />
                                                                                                    <asp:TextBox ID="txtSupplier1EmailId" runat="server" Enabled="false" CssClass="form-control" />
                                                                                                    <br />
                                                                                                </div>
                                                                                            </div>
                                                                                            <div class="col-md-4">
                                                                                                <asp:Label ID="Label80" runat="server" Text="Supplier 2" Font-Bold="true" />
                                                                                                <br />
                                                                                                <asp:Label ID="Label56" runat="server" Text="<%$ Resources:Attendance,Company Name %>" />
                                                                                                <asp:TextBox ID="txtSupplier2CompanyName" runat="server" CssClass="form-control" onchange="validateContactPerson(this)" />
                                                                                                <cc1:AutoCompleteExtender ID="AutoCompleteExtender16" runat="server" CompletionInterval="100"
                                                                                                    DelimiterCharacters="" Enabled="True" MinimumPrefixLength="1" ServiceMethod="GetCompletionList_Contacts"
                                                                                                    ServicePath="" TargetControlID="txtSupplier2CompanyName" UseContextKey="True" CompletionListCssClass="autoCompleteList" CompletionListItemCssClass="autoCompleteListItem" CompletionListHighlightedItemCssClass="autoCompleteSelectedListItem" OnClientShown="resetPosition">
                                                                                                </cc1:AutoCompleteExtender>
                                                                                                <br />
                                                                                                <asp:Label ID="Label47" runat="server" Text="Contact Person" />
                                                                                                <asp:TextBox ID="txtSupplier2ContactPerson" runat="server" CssClass="form-control" onchange="validateContactPerson(this)" />
                                                                                                <cc1:AutoCompleteExtender ID="AutoCompleteExtender19" runat="server" CompletionInterval="100"
                                                                                                    DelimiterCharacters="" Enabled="True" MinimumPrefixLength="1" ServiceMethod="GetCompletionList_Contacts"
                                                                                                    ServicePath="" TargetControlID="txtSupplier2ContactPerson" UseContextKey="True" CompletionListCssClass="autoCompleteList" CompletionListItemCssClass="autoCompleteListItem" CompletionListHighlightedItemCssClass="autoCompleteSelectedListItem" OnClientShown="resetPosition">
                                                                                                </cc1:AutoCompleteExtender>
                                                                                                <br />
                                                                                                <div style="display: none">
                                                                                                    <asp:Label ID="Label45" runat="server" Text="Contact No." />
                                                                                                    <div style="width: 100%" class="input-group">
                                                                                                        <asp:DropDownList ID="ddlSupplier2ContactNo_CountryCode" Style="width: 30%" runat="server" class="form-control select2" Enabled="false">
                                                                                                        </asp:DropDownList>
                                                                                                        <asp:TextBox ID="txtSupplier2ContactNo" runat="server" Enabled="false" Style="width: 70%" CssClass="form-control" />
                                                                                                        <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender6" runat="server" Enabled="True"
                                                                                                            TargetControlID="txtSupplier2ContactNo" FilterType="Numbers">
                                                                                                        </cc1:FilteredTextBoxExtender>
                                                                                                    </div>
                                                                                                    <br />
                                                                                                    <asp:Label ID="Label50" runat="server" Text="Fax No." />
                                                                                                    <div style="width: 100%" class="input-group">
                                                                                                        <asp:DropDownList ID="ddlSupplier2FaxNo_CountryCode" Enabled="false" Style="width: 30%" runat="server" class="form-control select2">
                                                                                                        </asp:DropDownList>
                                                                                                        <asp:TextBox ID="txtSupplier2FaxNo" runat="server" Style="width: 70%" Enabled="false" CssClass="form-control" />
                                                                                                        <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender9" runat="server" Enabled="True"
                                                                                                            TargetControlID="txtSupplier2FaxNo" FilterType="Numbers">
                                                                                                        </cc1:FilteredTextBoxExtender>
                                                                                                    </div>
                                                                                                    <br />
                                                                                                    <asp:Label ID="Label53" runat="server" Text="Email Id" />
                                                                                                    <asp:TextBox ID="txtSupplier2EmailId" runat="server" Enabled="false" CssClass="form-control"></asp:TextBox>
                                                                                                </div>
                                                                                            </div>
                                                                                            <div class="col-md-4">
                                                                                                <asp:Label ID="Label81" runat="server" Text="Supplier 3" Font-Bold="true" />
                                                                                                <br />
                                                                                                <asp:Label ID="Label57" runat="server" Text="<%$ Resources:Attendance,Company Name %>" />
                                                                                                <asp:TextBox ID="txtSupplier3CompanyName" runat="server" CssClass="form-control" onchange="validateContactPerson(this)" />
                                                                                                <cc1:AutoCompleteExtender ID="AutoCompleteExtender17" runat="server" CompletionInterval="100"
                                                                                                    DelimiterCharacters="" Enabled="True" MinimumPrefixLength="1" ServiceMethod="GetCompletionList_Contacts"
                                                                                                    ServicePath="" TargetControlID="txtSupplier3CompanyName" UseContextKey="True" CompletionListCssClass="autoCompleteList" CompletionListItemCssClass="autoCompleteListItem" CompletionListHighlightedItemCssClass="autoCompleteSelectedListItem" OnClientShown="resetPosition">
                                                                                                </cc1:AutoCompleteExtender>
                                                                                                <br />
                                                                                                <asp:Label ID="Label48" runat="server" Text="Contact Person" />
                                                                                                <asp:TextBox ID="txtSupplier3ContactPerson" runat="server" CssClass="form-control" onchange="validateContactPerson(this)"></asp:TextBox>
                                                                                                <cc1:AutoCompleteExtender ID="AutoCompleteExtender21" runat="server" CompletionInterval="100"
                                                                                                    DelimiterCharacters="" Enabled="True" MinimumPrefixLength="1" ServiceMethod="GetCompletionList_Contacts"
                                                                                                    ServicePath="" TargetControlID="txtSupplier3ContactPerson" UseContextKey="True" CompletionListCssClass="autoCompleteList" CompletionListItemCssClass="autoCompleteListItem" CompletionListHighlightedItemCssClass="autoCompleteSelectedListItem" OnClientShown="resetPosition">
                                                                                                </cc1:AutoCompleteExtender>
                                                                                                <br />
                                                                                                <div style="display: none">
                                                                                                    <asp:Label ID="Label46" runat="server" Text="Contact No." />
                                                                                                    <div style="width: 100%" class="input-group">
                                                                                                        <asp:DropDownList ID="ddlSupplier3ContactNo_CountryCode" Enabled="false" Style="width: 30%" runat="server" class="form-control select2">
                                                                                                        </asp:DropDownList>
                                                                                                        <asp:TextBox ID="txtSupplier3ContactNo" Enabled="false" runat="server" Style="width: 70%" CssClass="form-control" />
                                                                                                        <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender7" runat="server" Enabled="True"
                                                                                                            TargetControlID="txtSupplier3ContactNo" FilterType="Numbers">
                                                                                                        </cc1:FilteredTextBoxExtender>
                                                                                                    </div>
                                                                                                    <br />
                                                                                                    <asp:Label ID="Label51" runat="server" Text="Fax No." />
                                                                                                    <div style="width: 100%" class="input-group">
                                                                                                        <asp:DropDownList ID="ddlSupplier3FaxNo_CountryCode" Enabled="false" Style="width: 30%" runat="server" class="form-control select2">
                                                                                                        </asp:DropDownList>
                                                                                                        <asp:TextBox ID="txtSupplier3FaxNo" runat="server" Style="width: 70%" Enabled="false" CssClass="form-control" />
                                                                                                        <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender10" runat="server" Enabled="True"
                                                                                                            TargetControlID="txtSupplier3FaxNo" FilterType="Numbers">
                                                                                                        </cc1:FilteredTextBoxExtender>
                                                                                                    </div>
                                                                                                    <br />
                                                                                                    <asp:Label ID="Label54" runat="server" Text="Email Id" />
                                                                                                    <asp:TextBox ID="txtSupplier3EmailId" runat="server" CssClass="form-control" Enabled="false"></asp:TextBox>
                                                                                                    <br />
                                                                                                </div>
                                                                                            </div>
                                                                                        </div>
                                                                                    </div>
                                                                                </div>
                                                                            </div>
                                                                        </div>

                                                                        <div class="col-md-12">
                                                                            <div class="box box-primary collapsed-box">
                                                                                <div class="box-header with-border">
                                                                                    <h3 class="box-title">
                                                                                        <asp:Label ID="Label58" runat="server" CssClass="LableHeaderTitle" ForeColor="Gray"
                                                                                            Font-Bold="true" Text="Bank Reference"></asp:Label></h3>
                                                                                    <div class="box-tools pull-right">
                                                                                        <button type="button" class="btn btn-box-tool" data-widget="collapse">
                                                                                            <i class="fa fa-plus"></i>
                                                                                        </button>
                                                                                    </div>
                                                                                </div>
                                                                                <div class="box-body">
                                                                                    <div class="form-group">
                                                                                        <div class="col-md-12">
                                                                                            <div class="col-md-6">
                                                                                                <asp:Label ID="Label71" runat="server" Text="Account-1" Font-Bold="true" />
                                                                                                <br />
                                                                                                <asp:Label ID="Label59" runat="server" Text="Account Name" />
                                                                                                <asp:TextBox ID="txtAccountName1" runat="server" CssClass="form-control" />
                                                                                                <br />
                                                                                                <asp:Label ID="Label61" runat="server" Text="Account No." />
                                                                                                <asp:TextBox ID="txtAccountNo1" runat="server" CssClass="form-control" />
                                                                                                <br />
                                                                                                <asp:Label ID="Label63" runat="server" Text="Banker's Name" />
                                                                                                <asp:TextBox ID="txtAccountbankerName1" runat="server" CssClass="form-control" />
                                                                                                <br />
                                                                                                <asp:Label ID="Label65" runat="server" Text="Banker's Address" />
                                                                                                <asp:TextBox ID="txtAccountbankerAddress1" runat="server" CssClass="form-control" TextMode="MultiLine" />
                                                                                                <br />
                                                                                                <asp:Label ID="Label67" runat="server" Text="Contact No." />
                                                                                                <div style="width: 100%" class="input-group">
                                                                                                    <asp:DropDownList ID="ddlAccount1ContactNo_CountryCode" Style="width: 30%" runat="server" class="form-control select2">
                                                                                                    </asp:DropDownList>
                                                                                                    <asp:TextBox ID="txtAccountContactNo1" Style="width: 70%" runat="server" CssClass="form-control" />
                                                                                                    <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender11" runat="server" Enabled="True"
                                                                                                        TargetControlID="txtAccountContactNo1" FilterType="Numbers">
                                                                                                    </cc1:FilteredTextBoxExtender>
                                                                                                </div>
                                                                                                <br />
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
                                                                                            <div class="col-md-6">
                                                                                                <asp:Label ID="Label72" runat="server" Text="Account-2" Font-Bold="true" />
                                                                                                <br />
                                                                                                <asp:Label ID="Label60" runat="server" Text="Account Name" />
                                                                                                <asp:TextBox ID="txtAccountName2" runat="server" CssClass="form-control" />
                                                                                                <br />
                                                                                                <asp:Label ID="Label62" runat="server" Text="Account No." />
                                                                                                <asp:TextBox ID="txtAccountNo2" runat="server" CssClass="form-control" />
                                                                                                <br />
                                                                                                <asp:Label ID="Label64" runat="server" Text="Banker's Name" />
                                                                                                <asp:TextBox ID="txtAccountbankerName2" runat="server" CssClass="form-control" />
                                                                                                <br />
                                                                                                <asp:Label ID="Label66" runat="server" Text="Banker's Address" />
                                                                                                <asp:TextBox ID="txtAccountbankerAddress2" runat="server" CssClass="form-control" TextMode="MultiLine" />
                                                                                                <br />
                                                                                                <asp:Label ID="Label68" runat="server" Text="Contact No." />
                                                                                                <div style="width: 100%" class="input-group">
                                                                                                    <asp:DropDownList ID="ddlAccount2ContactNo_CountryCode" Style="width: 30%" runat="server" class="form-control select2">
                                                                                                    </asp:DropDownList>
                                                                                                    <asp:TextBox ID="txtAccountContactNo2" runat="server" Style="width: 70%" CssClass="form-control" />
                                                                                                    <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender12" runat="server" Enabled="True"
                                                                                                        TargetControlID="txtAccountContactNo2" FilterType="Numbers">
                                                                                                    </cc1:FilteredTextBoxExtender>
                                                                                                </div>
                                                                                                <br />
                                                                                                <asp:Label ID="Label70" runat="server" Text="Fax No." />
                                                                                                <div style="width: 100%" class="input-group">
                                                                                                    <asp:DropDownList ID="ddlAccount2FaxNo_CountryCode" Style="width: 30%" runat="server" class="form-control select2">
                                                                                                    </asp:DropDownList>
                                                                                                    <asp:TextBox ID="txtAccountFaxNo2" runat="server" Style="width: 70%" CssClass="form-control" />
                                                                                                    <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender14" runat="server" Enabled="True"
                                                                                                        TargetControlID="txtAccountFaxNo2" FilterType="Numbers">
                                                                                                    </cc1:FilteredTextBoxExtender>
                                                                                                </div>
                                                                                                <br />
                                                                                            </div>
                                                                                        </div>
                                                                                    </div>
                                                                                </div>
                                                                            </div>
                                                                        </div>
                                                                    </div>
                                                                </ContentTemplate>
                                                            </cc1:TabPanel>

                                                            <cc1:TabPanel ID="TabCategory" runat="server" HeaderText="<%$ Resources:Attendance,Product Category %>"
                                                                BorderColor="#C4C4C4">
                                                                <ContentTemplate>
                                                                    <div class="row">
                                                                        <div class="col-md-12">
                                                                            <asp:UpdatePanel ID="UpdatePanel4" runat="server">
                                                                                <ContentTemplate>
                                                                                    <asp:ListBox ID="lstProductCategory" runat="server" class="form-control" SelectionMode="Multiple"
                                                                                        Font-Names="Trebuchet MS" Font-Size="Small" ForeColor="Gray"></asp:ListBox>
                                                                                </ContentTemplate>
                                                                            </asp:UpdatePanel>
                                                                        </div>
                                                                    </div>
                                                                </ContentTemplate>
                                                            </cc1:TabPanel>
                                                            <cc1:TabPanel ID="TabLocation" runat="server" HeaderText="<%$ Resources:Attendance,Opening Balance%>"
                                                                BorderColor="#C4C4C4" Visible="false">
                                                                <ContentTemplate>
                                                                    <div class="row">
                                                                        <div class="col-md-12">
                                                                            <div class="flow">
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
                                                                    </div>
                                                                </ContentTemplate>
                                                            </cc1:TabPanel>
                                                        </cc1:TabContainer>
                                                        <br />
                                                        <div style="text-align: center">
                                                            <asp:Button ID="btnCustomerSave" runat="server" Text="<%$ Resources:Attendance,Save %>"
                                                                CssClass="btn btn-success" Visible="false" OnClick="btnCustomerSave_Click" />
                                                            <asp:Button ID="BtnReset" runat="server" Text="<%$ Resources:Attendance,Reset %>"
                                                                CssClass="btn btn-primary" CausesValidation="False" OnClick="BtnReset_Click" />
                                                            <asp:Button ID="btnCustomerCancel" runat="server" CssClass="btn btn-danger" Text="<%$ Resources:Attendance,Cancel %>"
                                                                CausesValidation="False" OnClick="btnCustomerCancel_Click" />
                                                            <asp:HiddenField ID="hdnCustomerId" runat="server" />
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
                                                <asp:HiddenField ID="hdnTotalExcelRecords" runat="server" Value="0" />
                                                <asp:HiddenField ID="hdnInvalidExcelRecords" runat="server" Value="0" />
                                                <asp:HiddenField ID="hdnValidExcelRecords" runat="server" Value="0" />
                                            </div>
                                            <div class="box-body">
                                                <div class="row">
                                                    <div class="col-md-4">
                                                        <%--<asp:LinkButton ID="lnkDownloadObData" runat="server" Text="Download Ob Data" OnClick="lnkDownloadData_Click" Font-Bold="true" Visible="true" Font-Size="15px"></asp:LinkButton>
                                                        <asp:LinkButton ID="lnkDownloadAgeingData" runat="server" Text="Download Ageing Data" OnClick="lnkDownloadAgeingData_Click" Font-Bold="true" Visible="false" Font-Size="15px"></asp:LinkButton>--%>
                                                        <asp:HyperLink ID="lnkDownloadObData" runat="server" Font-Bold="true" Font-Size="15px"
                                                            NavigateUrl="~/CompanyResource/customer_ob.xls" Text="Download sample/data" Font-Underline="true" Visible="true"></asp:HyperLink>
                                                        <asp:HyperLink ID="lnkDownloadAgeingData" runat="server" Font-Bold="true" Font-Size="15px"
                                                            NavigateUrl="~/CompanyResource/customer_ob_ageing.xls" Text="Download sample/data" Font-Underline="true" Visible="false"></asp:HyperLink>
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

                    <div class="tab-pane" id="Bin">
                        <asp:UpdatePanel ID="Update_Bin" runat="server">
                            <ContentTemplate>

                                <div class="row">
                                    <div class="col-md-12">
                                        <div id="Div2" runat="server" class="box box-info collapsed-box">
                                            <div class="box-header with-border">
                                                <h3 class="box-title">
                                                    <asp:Label ID="Label9" runat="server" Text="Advance Search"></asp:Label></h3>
                                                &nbsp;&nbsp;|&nbsp;&nbsp;
                                                 <asp:Label ID="lblTotalRecordsBin" Text="<%$ Resources:Attendance,Total Records: 0 %>" runat="server"></asp:Label>

                                                <div class="box-tools pull-right">
                                                    <button type="button" class="btn btn-box-tool" data-widget="collapse">
                                                        <i id="I2" runat="server" class="fa fa-plus"></i>
                                                    </button>
                                                </div>
                                            </div>
                                            <div class="box-body">
                                                <div class="col-lg-2">
                                                    <asp:DropDownList ID="ddlFieldNameBin" runat="server" class="form-control select2" Style="width: 100%;">
                                                        <asp:ListItem Text="<%$ Resources:Attendance, Customer Id %>" Value="Customer_Code"></asp:ListItem>
                                                        <asp:ListItem Text="<%$ Resources:Attendance, Customer Name %>" Value="Name" Selected="True"></asp:ListItem>
                                                        <asp:ListItem Text="<%$ Resources:Attendance, Customer Name(Local) %>" Value="Name_L"></asp:ListItem>
                                                    </asp:DropDownList>
                                                </div>
                                                <div class="col-lg-2">
                                                    <asp:DropDownList ID="ddlOptionBin" runat="server" class="form-control select2" Style="width: 100%;">
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
                                                <div class="col-lg-2">
                                                    <asp:LinkButton ID="btnbindBin" runat="server" CausesValidation="False" OnClientClick="tab_3_open()" OnClick="btnbindBin_Click" ToolTip="<%$ Resources:Attendance,Search %>"><span class="fa fa-search"  style="font-size:25px;"></span></asp:LinkButton>&nbsp;&nbsp;&nbsp;
                                                    <asp:LinkButton ID="btnRefreshBin" runat="server" CausesValidation="False" OnClientClick="tab_3_open()" OnClick="btnRefreshBin_Click" ToolTip="<%$ Resources:Attendance,Refresh %>"><span class="fa fa-repeat"  style="font-size:25px;"></span></asp:LinkButton>&nbsp;&nbsp;&nbsp;
                                                    <asp:LinkButton ID="imgBtnRestore" CausesValidation="False" Visible="false" runat="server" OnClick="imgBtnRestore_Click" ToolTip="<%$ Resources:Attendance, Active %>"><span class="far fa-lightbulb"  style="font-size:25px;"></span></asp:LinkButton>&nbsp;&nbsp;&nbsp;
                                                    <asp:LinkButton ID="ImgbtnSelectAll" Visible="false" runat="server" OnClick="ImgbtnSelectAll_Click" ToolTip="<%$ Resources:Attendance, Select All %>" AutoPostBack="true"><span class="fas fa-th"  style="font-size:25px;"></span></asp:LinkButton>
                                                </div>

                                            </div>
                                        </div>

                                    </div>
                                </div>
                                <div class="box box-warning box-solid">
                                    <div class="box-body">
                                        <div class="row">
                                            <div class="col-md-12">
                                                <div style="overflow: auto">
                                                    <asp:GridView CssClass="table-striped table-bordered table table-hover" ID="GvCustomerBin" PageSize="<%# PageControlCommon.GetPageSize() %>"
                                                        runat="server" AutoGenerateColumns="False" DataKeyNames="Customer_Id" Width="100%"
                                                        AllowPaging="True" OnPageIndexChanging="GvCustomerBin_PageIndexChanging" OnSorting="GvCustomerBin_OnSorting"
                                                        AllowSorting="true">
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
                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,Customer Id%>" SortExpression="Code">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblgvCustomerCode" runat="server" Text='<%# Eval("Customer_Code") %>' />
                                                                    <asp:Label ID="lblgvCustomerId" runat="server" Visible="false" Text='<%# Eval("Customer_Id") %>' />
                                                                </ItemTemplate>
                                                                <ItemStyle />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,Customer Name %>" SortExpression="Name">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblgvCustomerName" runat="server" Text='<%# Eval("Name") %>' />
                                                                </ItemTemplate>
                                                                <ItemStyle />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,Customer Name(Local) %>"
                                                                SortExpression="Name_L">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblgvLCustomerName" runat="server" Text='<%# Eval("Name_L") %>' />
                                                                </ItemTemplate>
                                                                <ItemStyle />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField Visible="false" HeaderText="<%$ Resources:Attendance,Account No %>"
                                                                SortExpression="Account_No">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblgvAccountNo" runat="server" Text='<%# Eval("Account_No") %>' />
                                                                </ItemTemplate>
                                                                <ItemStyle />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,Credit Limit %>" SortExpression="Credit_Limit">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblgvCreditLimit" runat="server" Text='<%#Eval("Credit_Limit") %>' />
                                                                </ItemTemplate>
                                                                <ItemStyle HorizontalAlign="Center" />
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
        <div class="modal-dialog">
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal">
                        <span aria-hidden="true">&times;</span><span class="sr-only">Close</span></button>
                    <h4 class="modal-title" id="Modal_BankLabel">Add Bank Account</h4>
                </div>
                <div class="modal-body">
                    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                        <ContentTemplate>
                            <div class="row">
                                <div class="col-md-12">
                                    <div class="col-md-6">
                                        <asp:Label ID="Label1238" runat="server" Text="<%$ Resources:Attendance,Bank Name %>" />
                                        <asp:UpdatePanel ID="UpdatePanel6" runat="server">
                                            <ContentTemplate>
                                                <asp:TextBox ID="txtBankName" runat="server" CssClass="form-control" AutoPostBack="true"
                                                    OnTextChanged="txtBankName_TextChanged" BackColor="#eeeeee" />
                                                <cc1:AutoCompleteExtender ID="AutoCompleteExtender8" ServiceMethod="GetCompletionListBankName"
                                                    runat="server" DelimiterCharacters="" Enabled="True" TargetControlID="txtBankName"
                                                    ServicePath="" CompletionInterval="100" MinimumPrefixLength="1" CompletionSetCount="1"
                                                    CompletionListCssClass="autoCompleteList" CompletionListItemCssClass="autoCompleteListItem"
                                                    CompletionListHighlightedItemCssClass="autoCompleteSelectedListItem" OnClientShown="resetPosition"
                                                    UseContextKey="True">
                                                </cc1:AutoCompleteExtender>
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                        <asp:UpdateProgress ID="UpdateProgress6" runat="server" AssociatedUpdatePanelID="UpdatePanel6">
                                            <ProgressTemplate>
                                                <div class="modal_Progress">
                                                    <div class="center_Progress">
                                                        <img alt="" src="../Images/ajax-loader2.gif" style="vertical-align: middle; background-color: White; border-radius: 10px;" />
                                                    </div>
                                                </div>
                                            </ProgressTemplate>
                                        </asp:UpdateProgress>
                                        <br />
                                    </div>
                                    <asp:UpdatePanel ID="Update_Bank_Account" runat="server">
                                        <ContentTemplate>
                                            <asp:UpdateProgress ID="UpdateProgress7" runat="server" AssociatedUpdatePanelID="Update_Bank_Account">
                                                <ProgressTemplate>
                                                    <div class="modal_Progress">
                                                        <div class="center_Progress">
                                                            <img alt="" src="../Images/ajax-loader2.gif" style="vertical-align: middle; background-color: White; border-radius: 10px;" />
                                                        </div>
                                                    </div>
                                                </ProgressTemplate>
                                            </asp:UpdateProgress>
                                            <div class="col-md-6">
                                                <asp:Label ID="Label20" runat="server" Text="<%$ Resources:Attendance,Account No %>" />
                                                <asp:TextBox ID="txtCBAccountNo" runat="server" CssClass="form-control" />
                                                <br />
                                            </div>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </div>
                                <asp:UpdatePanel ID="UpdatePanel12" runat="server">
                                    <ContentTemplate>
                                        <asp:UpdateProgress ID="UpdateProgress8" runat="server" AssociatedUpdatePanelID="UpdatePanel12">
                                            <ProgressTemplate>
                                                <div class="modal_Progress">
                                                    <div class="center_Progress">
                                                        <img alt="" src="../Images/ajax-loader2.gif" style="vertical-align: middle; background-color: White; border-radius: 10px;" />
                                                    </div>
                                                </div>
                                            </ProgressTemplate>
                                        </asp:UpdateProgress>
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
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                                <div style="text-align: center;" class="col-md-12">
                                    <asp:UpdatePanel ID="UpdatePanel7" runat="server">
                                        <ContentTemplate>
                                            <asp:UpdateProgress ID="UpdateProgress9" runat="server" AssociatedUpdatePanelID="UpdatePanel7">
                                                <ProgressTemplate>
                                                    <div class="modal_Progress">
                                                        <div class="center_Progress">
                                                            <img alt="" src="../Images/ajax-loader2.gif" style="vertical-align: middle; background-color: White; border-radius: 10px;" />
                                                        </div>
                                                    </div>
                                                </ProgressTemplate>
                                            </asp:UpdateProgress>

                                            <asp:HiddenField ID="hdnContactBankId" runat="server" />
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </div>
                            </div>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
                <div class="modal-footer">
                    <asp:UpdatePanel ID="Update_Bank_Buuton" runat="server">
                        <ContentTemplate>
                            <asp:Button ID="BtnCBSave" runat="server" Text="<%$ Resources:Attendance,Save %>"
                                CssClass="btn btn-success" OnClick="btnCBSave_Click" />
                            <asp:Button ID="BtnCBCancel" runat="server" Text="<%$ Resources:Attendance,Reset %>"
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
                    <h4 class="modal-title" id="Modal_ProductLabel">Customer Details</h4>
                </div>
                <div class="modal-body">
                    <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                        <ContentTemplate>
                            <div class="row">
                                <div class="col-md-12">
                                    <asp:Label ID="Label26" runat="server" Text="<%$ Resources:Attendance,Customer Name %>" />
                                    &nbsp : &nbsp
                        <asp:Label ID="pnllblCustomerName" runat="server" Font-Bold="true" />
                                </div>
                                <div class="col-md-12">
                                    <div class="alert alert-info ">
                                        <div class="row">
                                            <div class="col-md-12">
                                                <div class="form-group">
                                                    <div class="col-lg-3">
                                                        <asp:DropDownList ID="ddlCpriceField" runat="server" class="form-control select2" Style="width: 100%;">
                                                            <asp:ListItem Text="<%$ Resources:Attendance, Product Id %>" Value="ProductId"></asp:ListItem>
                                                            <asp:ListItem Text="<%$ Resources:Attendance, Product Name %>" Value="EProductName"
                                                                Selected="True"></asp:ListItem>
                                                            <asp:ListItem Text="<%$ Resources:Attendance,Product Code %>" Value="ProductCode"></asp:ListItem>
                                                        </asp:DropDownList>
                                                    </div>
                                                    <div class="col-lg-2">
                                                        <asp:DropDownList ID="ddlCpriceFieldoption" runat="server" class="form-control select2" Style="width: 100%;">
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
                                                        <asp:UpdatePanel ID="UpdatePanel8" runat="server">
                                                            <ContentTemplate>
                                                                <asp:UpdateProgress ID="UpdateProgress5" runat="server" AssociatedUpdatePanelID="UpdatePanel8">
                                                                    <ProgressTemplate>
                                                                        <div class="modal_Progress">
                                                                            <div class="center_Progress">
                                                                                <img alt="" src="../Images/ajax-loader2.gif" style="vertical-align: middle; background-color: White; border-radius: 10px;" />
                                                                            </div>
                                                                        </div>
                                                                    </ProgressTemplate>
                                                                </asp:UpdateProgress>
                                                                <asp:ImageButton ID="btnBindCPricelist" Style="width: 35px; margin-top: -1px;" runat="server"
                                                                    CausesValidation="False" ImageUrl="~/Images/search.png" OnClick="btnBindCPricelist_Click"
                                                                    ToolTip="<%$ Resources:Attendance,Search %>"></asp:ImageButton>
                                                                <asp:ImageButton ID="btnRefreshCPricelist" runat="server" Style="width: 30px;" CausesValidation="False"
                                                                    ImageUrl="~/Images/refresh.png" OnClick="btnRefreshCPricelist_Click" ToolTip="<%$ Resources:Attendance,Refresh %>"></asp:ImageButton>
                                                            </ContentTemplate>
                                                        </asp:UpdatePanel>
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
                                                    <%-- <div style="overflow: auto">--%>
                                                    <asp:UpdatePanel ID="UpdatePanel9" runat="server">
                                                        <ContentTemplate>
                                                            <asp:UpdateProgress ID="UpdateProgress10" runat="server" AssociatedUpdatePanelID="UpdatePanel9">
                                                                <ProgressTemplate>
                                                                    <div class="modal_Progress">
                                                                        <div class="center_Progress">
                                                                            <img alt="" src="../Images/ajax-loader2.gif" style="vertical-align: middle; background-color: White; border-radius: 10px;" />
                                                                        </div>
                                                                    </div>
                                                                </ProgressTemplate>
                                                            </asp:UpdateProgress>

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

                                                        </ContentTemplate>
                                                    </asp:UpdatePanel>
                                                    <%--</div>--%>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                                <div style="text-align: center;" class="col-md-12">
                                    <asp:UpdatePanel ID="UpdatePanel10" runat="server">
                                        <ContentTemplate>
                                            <asp:UpdateProgress ID="UpdateProgress11" runat="server" AssociatedUpdatePanelID="UpdatePanel10">
                                                <ProgressTemplate>
                                                    <div class="modal_Progress">
                                                        <div class="center_Progress">
                                                            <img alt="" src="../Images/ajax-loader2.gif" style="vertical-align: middle; background-color: White; border-radius: 10px;" />
                                                        </div>
                                                    </div>
                                                </ProgressTemplate>
                                            </asp:UpdateProgress>

                                            <asp:HiddenField ID="hdnCustomerpricelist" runat="server" />
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </div>
                            </div>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
                <div class="modal-footer">
                    <asp:UpdatePanel ID="Update_Modal_Button" runat="server">
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

    <div class="modal fade" id="modelContactDetail" tabindex="-1" role="dialog" aria-labelledby="myModalLabel"
        aria-hidden="true">
        <div class="modal-dialog modal-lg">
            <div class="modal-content">

                <div class="modal-body">
                    <AT1:ViewContact ID="UcContactList" runat="server" />
                </div>


                <div class="modal-footer">
                    <button type="button" id="" class="btn btn-danger" data-dismiss="modal">
                        Close</button>
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



    <asp:UpdateProgress ID="UpdateProgress1" runat="server" AssociatedUpdatePanelID="Update_Button">
        <ProgressTemplate>
            <div class="modal_Progress">
                <div class="center_Progress">
                    <img alt="" src="../Images/ajax-loader2.gif" style="vertical-align: middle; background-color: White; border-radius: 10px;" />
                </div>
            </div>
        </ProgressTemplate>
    </asp:UpdateProgress>

    <asp:UpdateProgress ID="UpdateProgress12" runat="server" AssociatedUpdatePanelID="Update_List">
        <ProgressTemplate>
            <div class="modal_Progress">
                <div class="center_Progress">
                    <img alt="" src="../Images/ajax-loader2.gif" style="vertical-align: middle; background-color: White; border-radius: 10px;" />
                </div>
            </div>
        </ProgressTemplate>
    </asp:UpdateProgress>

    <asp:UpdateProgress ID="UpdateProgress13" runat="server" AssociatedUpdatePanelID="Update_New">
        <ProgressTemplate>
            <div class="modal_Progress">
                <div class="center_Progress">
                    <img alt="" src="../Images/ajax-loader2.gif" style="vertical-align: middle; background-color: White; border-radius: 10px;" />
                </div>
            </div>
        </ProgressTemplate>
    </asp:UpdateProgress>

    <asp:UpdateProgress ID="UpdateProgress16" runat="server" AssociatedUpdatePanelID="Update_Bin">
        <ProgressTemplate>
            <div class="modal_Progress">
                <div class="center_Progress">
                    <img alt="" src="../Images/ajax-loader2.gif" style="vertical-align: middle; background-color: White; border-radius: 10px;" />
                </div>
            </div>
        </ProgressTemplate>
    </asp:UpdateProgress>

    <asp:UpdateProgress ID="UpdateProgress22" runat="server" AssociatedUpdatePanelID="UpdatePanel1">
        <ProgressTemplate>
            <div class="modal_Progress">
                <div class="center_Progress">
                    <img alt="" src="../Images/ajax-loader2.gif" style="vertical-align: middle; background-color: White; border-radius: 10px;" />
                </div>
            </div>
        </ProgressTemplate>
    </asp:UpdateProgress>

    <asp:UpdateProgress ID="UpdateProgress23" runat="server" AssociatedUpdatePanelID="UpdatePanel6">
        <ProgressTemplate>
            <div class="modal_Progress">
                <div class="center_Progress">
                    <img alt="" src="../Images/ajax-loader2.gif" style="vertical-align: middle; background-color: White; border-radius: 10px;" />
                </div>
            </div>
        </ProgressTemplate>
    </asp:UpdateProgress>

    <asp:UpdateProgress ID="UpdateProgress24" runat="server" AssociatedUpdatePanelID="Update_Bank_Account">
        <ProgressTemplate>
            <div class="modal_Progress">
                <div class="center_Progress">
                    <img alt="" src="../Images/ajax-loader2.gif" style="vertical-align: middle; background-color: White; border-radius: 10px;" />
                </div>
            </div>
        </ProgressTemplate>
    </asp:UpdateProgress>

    <asp:UpdateProgress ID="UpdateProgress25" runat="server" AssociatedUpdatePanelID="UpdatePanel12">
        <ProgressTemplate>
            <div class="modal_Progress">
                <div class="center_Progress">
                    <img alt="" src="../Images/ajax-loader2.gif" style="vertical-align: middle; background-color: White; border-radius: 10px;" />
                </div>
            </div>
        </ProgressTemplate>
    </asp:UpdateProgress>

    <asp:UpdateProgress ID="UpdateProgress26" runat="server" AssociatedUpdatePanelID="UpdatePanel7">
        <ProgressTemplate>
            <div class="modal_Progress">
                <div class="center_Progress">
                    <img alt="" src="../Images/ajax-loader2.gif" style="vertical-align: middle; background-color: White; border-radius: 10px;" />
                </div>
            </div>
        </ProgressTemplate>
    </asp:UpdateProgress>
    
    <asp:UpdateProgress ID="UpdateProgress2" runat="server" AssociatedUpdatePanelID="update_import">
        <ProgressTemplate>
            <div class="modal_Progress">
                <div class="center_Progress">
                    <img alt="" src="../Images/ajax-loader2.gif" style="vertical-align: middle; background-color: White; border-radius: 10px;" />
                </div>
            </div>
        </ProgressTemplate>
    </asp:UpdateProgress>
    
    <asp:UpdateProgress ID="UpdateProgress27" runat="server" AssociatedUpdatePanelID="Update_Bank_Buuton">
        <ProgressTemplate>
            <div class="modal_Progress">
                <div class="center_Progress">
                    <img alt="" src="../Images/ajax-loader2.gif" style="vertical-align: middle; background-color: White; border-radius: 10px;" />
                </div>
            </div>
        </ProgressTemplate>
    </asp:UpdateProgress>


    <asp:Panel ID="PanelView1" runat="server" Visible="False">
        <asp:Panel ID="PanelView2" runat="server" Visible="False">
            <asp:Panel ID="PanelViewDetail" runat="server" Style="width: 1050px; height: 100%;">
                <table>
                    <tr>
                        <td>
                            <asp:Label ID="lblViewCustomerCode" runat="server" Text="<%$ Resources:Attendance,Customer Id %>" />
                        </td>
                        <td align="center">:
                        </td>
                        <td>
                            <asp:Label ID="txtViewCustomerCode" runat="server" Font-Bold="true" />
                            <asp:HiddenField ID="hdnContactIdView" runat="server" Value='<%#Eval("Trans_Id") %>' />
                        </td>
                        <td>
                            <asp:Label ID="lblViewCustomerName" runat="server" Text="<%$ Resources:Attendance,Customer Name %>" />
                        </td>
                        <td align="center">:
                        </td>
                        <td>
                            <asp:Label ID="txtCustomernameview" runat="server" Font-Bold="true" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="lblViewCustomerLocalName" runat="server" Text="<%$ Resources:Attendance,Supplier Name(Local) %>" />
                        </td>
                        <td align="center">:
                        </td>
                        <td>
                            <asp:Label ID="txtViewCustomerLocalName" Font-Bold="true"
                                runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="Label16" runat="server" Text="<%$ Resources:Attendance,Permanent EmailId %>" />
                        </td>
                        <td align="center">:
                        </td>
                        <td>
                            <asp:Label ID="lblViewEmailId" Font-Bold="true" runat="server"
                                Text='<%#Eval("Field1") %>' />
                        </td>
                        <td>
                            <asp:Label ID="Label18" runat="server" Text="<%$ Resources:Attendance,Permanent MobileNo. %>" />
                        </td>
                        <td align="center">:
                        </td>
                        <td>
                            <asp:Label ID="lblViewMobileNo" Font-Bold="true" runat="server"
                                Text='<%#Eval("Field2") %>' />
                        </td>
                    </tr>
                    <tr>
                        <td colspan="6">
                            <br />
                        </td>
                    </tr>
                    <tr>
                        <td colspan="6">
                            <div class="flow">
                                <asp:GridView CssClass="table-striped table-bordered table table-hover" ID="GvAddressNameView" runat="server"
                                    AutoGenerateColumns="False">

                                    <Columns>
                                        <asp:TemplateField HeaderText="<%$ Resources:Attendance, Address Name %>">
                                            <ItemTemplate>
                                                <asp:Label ID="lblgvAddressName" runat="server" Text='<%#Eval("Address_Name") %>' />
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Center" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ Resources:Attendance, Address %>">
                                            <ItemTemplate>
                                                <asp:Label ID="lblgvAddress" runat="server" Text='<%#Eval("Address") %>' />
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Center" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ Resources:Attendance,EmailId %>">
                                            <ItemTemplate>
                                                <asp:Label ID="lblgvEmailId" runat="server" Text='<%#Eval("EmailId1") %>' />
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Center" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ Resources:Attendance,FaxNo. %>">
                                            <ItemTemplate>
                                                <asp:Label ID="lblgvFaxNo" runat="server" Text='<%#Eval("FaxNo") %>' />
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Center" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ Resources:Attendance,PhoneNo. 1 %>">
                                            <ItemTemplate>
                                                <asp:Label ID="lblgvPhoneNo1" runat="server" Text='<%#Eval("PhoneNo1") %>' />
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Center" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ Resources:Attendance,PhoneNo. 2 %>">
                                            <ItemTemplate>
                                                <asp:Label ID="lblgvPhoneNo2" runat="server" Text='<%#Eval("PhoneNo2") %>' />
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Center" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ Resources:Attendance,MobileNo. 1 %>">
                                            <ItemTemplate>
                                                <asp:Label ID="lblgvMobileNo1" runat="server" Text='<%#Eval("MobileNo1") %>' />
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Center" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ Resources:Attendance,MobileNo. 2 %>">
                                            <ItemTemplate>
                                                <asp:Label ID="lblgvMobileNo2" runat="server" Text='<%#Eval("MobileNo2") %>' />
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Center" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ Resources:Attendance,Is Default%>">
                                            <ItemTemplate>
                                                <asp:CheckBox ID="chkdefault" Enabled="false" runat="server" Checked='<%#Eval("Is_Default") %>'
                                                    AutoPostBack="true" OnCheckedChanged="chkgvSelect_CheckedChangedDefault" />
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Center" Width="30%" />
                                        </asp:TemplateField>
                                    </Columns>

                                    <PagerStyle CssClass="pagination-ys" />

                                </asp:GridView>
                            </div>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="6">
                            <br />
                        </td>
                    </tr>
                    <tr>
                        <td colspan="6">
                            <div class="flow">
                                <asp:GridView CssClass="table-striped table-bordered table table-hover" ID="GvContactBankDetailView" runat="server" AutoGenerateColumns="False">

                                    <Columns>
                                        <asp:TemplateField HeaderText="<%$ Resources:Attendance, Bank Name %>">
                                            <ItemTemplate>
                                                <asp:Label ID="lblgvBankId" runat="server" Text='<%#GetBankName(Eval("Bank_Id").ToString()) %>' />
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
                                        <asp:TemplateField HeaderText="<%$ Resources:Attendance,Branch Address %>">
                                            <ItemTemplate>
                                                <asp:Label ID="lblgvBankBranchAddress" runat="server" Text='<%#Eval("Branch_Address") %>' />
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Center" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ Resources:Attendance,IBAN Number %>">
                                            <ItemTemplate>
                                                <asp:Label ID="lblgvIBankNum" runat="server" Text='<%#Eval("IBAN_NUMBER") %>' />
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Center" />
                                        </asp:TemplateField>
                                    </Columns>

                                    <PagerStyle CssClass="pagination-ys" />

                                </asp:GridView>
                                <asp:HiddenField ID="HiddenField1view" runat="server" />
                                <asp:HiddenField ID="HiddenField2view" runat="server" />
                            </div>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="6">
                            <br />
                        </td>
                    </tr>
                    <tr>
                        <td colspan="6">
                            <br />
                            <asp:Panel ID="pnlViewContactList" runat="server" ScrollBars="Auto">
                                <asp:DataList ID="dlViewcontactlist" runat="server" RepeatLayout="Table" RepeatColumns="3"
                                    RepeatDirection="Horizontal">
                                    <ItemTemplate>
                                        <fieldset>
                                            <table>
                                                <tr>
                                                    <td>
                                                        <asp:Label ID="lblContactNameView" runat="server" Text="<%$ Resources:Attendance,Contact Name %>"></asp:Label>
                                                    </td>
                                                    <td align="center">:
                                                    </td>
                                                    <td>
                                                        <asp:Label ID="txtContactNameView" Font-Bold="true" runat="server"
                                                            Text='<%# Eval("Name") %>' />
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <asp:Label ID="lblTerm" runat="server" Text="<%$ Resources:Attendance,Department %>" />
                                                    </td>
                                                    <td align="center">:
                                                    </td>
                                                    <td>
                                                        <asp:Label ID="lblDepartmentView" Font-Bold="true" runat="server"
                                                            Text='<%#GetDepartmentName(Eval("Dep_Id").ToString()) %>' />
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <asp:Label ID="Label10" runat="server" Text="<%$ Resources:Attendance,Designation %>" />
                                                    </td>
                                                    <td align="center">:
                                                    </td>
                                                    <td>
                                                        <asp:Label ID="lblDesignation" Font-Bold="true" runat="server"
                                                            Text='<%#Eval("Designation").ToString() %>' />
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <asp:Label ID="lblContactMobileNo" runat="server" Text="<%$ Resources:Attendance,Mobile No 1 %>" />
                                                    </td>
                                                    <td align="center">:
                                                    </td>
                                                    <td>
                                                        <asp:Label ID="txtContactMobileNo" Font-Bold="true" runat="server"
                                                            Text='<%#Eval("Field2") %>' />
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <asp:Label ID="lblContactEmailId" runat="server" Text="<%$ Resources:Attendance,Email Id%>" />
                                                    </td>
                                                    <td align="center">:
                                                    </td>
                                                    <td>
                                                        <asp:Label ID="txtContactEmailId" Font-Bold="true" runat="server"
                                                            Text='<%#Eval("Field1") %>' />
                                                    </td>
                                                </tr>
                                            </table>
                                        </fieldset>
                                    </ItemTemplate>
                                </asp:DataList>
                            </asp:Panel>
                        </td>
                    </tr>
                </table>
                <table width="100%" style="margin-top: 10px;">
                    <tr>
                        <td colspan="6">
                            <br />
                        </td>
                    </tr>
                    <tr>
                        <td colspan="6" align="center" style="padding-left: 10px">
                            <asp:Button ID="BtnCancelView" runat="server" CssClass="btn btn-primary" Text="<%$ Resources:Attendance,Close %>"
                                CausesValidation="False" OnClick="BtnCancelView_Click" />
                            <asp:HiddenField ID="hdneditidView" runat="server" />
                        </td>
                    </tr>
                </table>
            </asp:Panel>
        </asp:Panel>
    </asp:Panel>

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
        function checkDec(el) {
            if (el.value > 1000) {
                alert('Credit limit is greater than 1000 so financial statement should be required');
            }
        }

        function isNumber(evt) {
            evt = (evt) ? evt : window.event;
            var charCode = (evt.which) ? evt.which : evt.keyCode;
            if (charCode > 31 && (charCode < 48 || charCode > 57)) {
                return false;
            }
            return true;
        }

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
        }

        function Li_Tab_List() {
            document.getElementById('<%= Btn_List.ClientID %>').click();
        }
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
        function Modal_Address_Close() {
            document.getElementById('<%= Btn_Add_Address.ClientID %>').click();
        }

        function Modal_Bank_Close() {
            document.getElementById('<%= Btn_Bank_Account.ClientID %>').click();
        }

        function Modal_Product_Close() {
            document.getElementById('<%= Btn_Product_Details.ClientID %>').click();
        }

        function Company_Detail_Div() {
            $("#Btn_Company_Detail").removeClass("fa fa-plus");
            $("#Div_Company_Detail").removeClass("box box-primary collapsed-box");

            $("#Btn_Company_Detail").addClass("fa fa-minus");
            $("#Div_Company_Detail").addClass("box box-primary");
        }

        function Company_Owner_or_Sponser_detail_Div() {
            $("#Btn_Company_Owner_or_Sponser_detail").removeClass("fa fa-plus");
            $("#Div_Company_Owner_or_Sponser_detail").removeClass("box box-primary collapsed-box");

            $("#Btn_Company_Owner_or_Sponser_detail").addClass("fa fa-minus");
            $("#Div_Company_Owner_or_Sponser_detail").addClass("box box-primary");
        }

        function Credit_Approval_Div() {
            $("#Btn_Credit_Approval").removeClass("fa fa-plus");
            $("#Div_Credit_Approval").removeClass("box box-primary collapsed-box");

            $("#Btn_Credit_Approval").addClass("fa fa-minus");
            $("#Div_Credit_Approval").addClass("box box-primary");
        }

        function Personal_Authorized_Signature_For_purchase_Order_Div() {
            $("#Btn_Personal_Authorized_Signature_For_purchase_Order").removeClass("fa fa-plus");
            $("#Div_Personal_Authorized_Signature_For_purchase_Order").removeClass("box box-primary collapsed-box");

            $("#Btn_Personal_Authorized_Signature_For_purchase_Order").addClass("fa fa-minus");
            $("#Div_Personal_Authorized_Signature_For_purchase_Order").addClass("box box-primary");
        }
    </script>
    <script type="text/javascript">
        function FuLogo_UploadComplete(sender, args) {
            document.getElementById('<%= FULogo_Img_Wrong.ClientID %>').style.display = "none";
            document.getElementById('<%= FULogo_Img_Right.ClientID %>').style.display = ""; 

            <%--var img = document.getElementById('<%= imgLogo.ClientID %>');
            img.src = "<%=ResolveUrl(FuLogo_UploadFolderPath) %>" + args.get_fileName();--%>
        }
        function FuLogo_UploadError(sender, args) {
            document.getElementById('<%= FULogo_Img_Right.ClientID %>').style.display = "none";
            document.getElementById('<%= FULogo_Img_Wrong.ClientID %>').style.display = "";
            var img = document.getElementById('<%= imgLogo.ClientID %>');
            img.src = "../Bootstrap_Files/dist/img/NoImage.jpg";
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

        function ArcFUAll_UploadStarted(sender, args) {

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


        function Modal_Number_Open(data) {
            document.getElementById('AllNumberData').innerHTML = data;
            document.getElementById('<%= Btn_Number.ClientID %>').click();
        }
        function Modal_CustomerInfo_Open() {
            document.getElementById('<%= Btn_CustomerInfo_Modal.ClientID %>').click();
        }
        function Modal_Open_FileUpload() {
            document.getElementById('<%= Btn_Modal_FileUpload.ClientID %>').click();
        }


        function On_Load_Modal() {
            $('#Div_Conversation').animate({
                scrollTop: $('#Div_Conversation')[0].scrollHeight
            }, 5);
        }
        function Modal_AcMaster_Open() {
            document.getElementById('<%= Btn_ucAcMaster.ClientID %>').click();
        }
        function showUcControlsSettings() {
            $('#ControlSettingModal').modal('show');
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
        function CreateNewEmployee() {
            window.open('../MasterSetUp/EmployeeMaster.aspx');
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
