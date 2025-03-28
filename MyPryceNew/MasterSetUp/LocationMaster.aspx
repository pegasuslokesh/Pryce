<%@ Page Title="" Language="C#" MasterPageFile="~/ERPMaster.master" EnableEventValidation="false" MaintainScrollPositionOnPostback="true" AutoEventWireup="true" CodeFile="LocationMaster.aspx.cs" Inherits="MasterSetUp_LocationMaster" %>
<%@ Register Assembly="DevExpress.Web.v18.1, Version=18.1.3.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web" TagPrefix="dx" %>
<%@ Register Src="~/WebUserControl/AddressControl.ascx" TagName="Addressmaster" TagPrefix="UC" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Src="~/WebUserControl/ucControlsSetting.ascx" TagName="ucCtlSetting" TagPrefix="UC" %>
<%@ Register Src="~/WebUserControl/AddEmployee.ascx" TagName="EmployeeMaster" TagPrefix="EPM" %>
<asp:Content ID="Content1" ContentPlaceHolderID="Head" runat="Server">
    <link href="../Bootstrap_Files/Additional/Popup_Style.css" rel="stylesheet" type="text/css" />
    <link href="../Bootstrap_Files/Additional/Button_Style.css" rel="stylesheet" type="text/css" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="HeaderContent" runat="Server">
    <h1>
        <i class="fas fa-globe-europe"></i>&nbsp;&nbsp;
        <asp:Label ID="lblHeader" runat="server" Text="<%$ Resources:Attendance,Location Master%>"></asp:Label>
    </h1>
    <ol class="breadcrumb">
        <li><a href="#"><i class="fa fa-dashboard"></i>
            <asp:Label ID="Label6" runat="server" Text="<%$ Resources:Attendance,Master Setup%>"></asp:Label></a></li>
        <li><a href="#"><i class="fa fa-dashboard"></i>
            <asp:Label ID="Label10" runat="server" Text="<%$ Resources:Attendance,Master SetUp%>"></asp:Label></a></li>
        <li class="active">
            <asp:Label ID="Label5" runat="server" Text="<%$ Resources:Attendance,Location Master%>"></asp:Label></li>
    </ol>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="Server">
    <asp:UpdatePanel ID="Update_Button" runat="server">
        <ContentTemplate>
            <asp:Button ID="Btn_New" Style="display: none;" runat="server" OnClick="btnNew_Click" Text="New" />
            <asp:Button ID="Btn_Bin" Style="display: none;" runat="server" OnClick="btnBin_Click" Text="Bin" />
            <asp:Button ID="Btn_Address_Modal" Style="display: none;" runat="server" data-toggle="modal" data-target="#myModal" Text="Address" />
            <asp:Button ID="Btn_Employee_New" Style="display:none" runat="server" data-toggle="modal" data-target="#EmployeeModal" Text="Employee" />
            <asp:Button ID="Btn_Number" Style="display: none;" data-toggle="modal" data-target="#NumberData" runat="server" Text="Number Data" />
            <asp:HiddenField runat="server" ID="hdnCanEdit" />
            <asp:HiddenField runat="server" ID="hdnCanDelete" />
        </ContentTemplate>
    </asp:UpdatePanel>
    <div class="row">
        <div class="col-md-12">
            <div class="nav-tabs-custom">
                <ul class="nav nav-tabs pull-right bg-blue-gradient">
                    <li id="Li_Bin"><a onclick="Li_Tab_Bin()" href="#Bin" data-toggle="tab">
                        <i class="fa fa-trash"></i>&nbsp;&nbsp;<asp:Label ID="Label9" runat="server" Text="<%$ Resources:Attendance,Bin%>"></asp:Label></a></li>
                    <li id="Li_New"><a onclick="Li_Tab_New()" href="#New" data-toggle="tab">
                        <asp:UpdatePanel ID="Update_Tab_Name" runat="server">
                            <ContentTemplate>
                                <i class="fa fa-file"></i>&nbsp;&nbsp;
                                <asp:Label ID="Lbl_Tab_New" runat="server" Text="<%$ Resources:Attendance,New%>"></asp:Label>
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
                                                    <asp:Label ID="Label22" runat="server" Text="<%$ Resources:Attendance,Advance Search%>"></asp:Label></h3>
                                                &nbsp;&nbsp;|&nbsp;&nbsp;
					<asp:Label ID="lblTotalRecords" Text="<%$ Resources:Attendance,Total Records: 0 %>" runat="server"></asp:Label>
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
                                                        <asp:ListItem Selected="True" Text="<%$ Resources:Attendance,Location Name %>" Value="Location_Name"></asp:ListItem>
                                                        <asp:ListItem Text="<%$ Resources:Attendance,Location Name(Local) %>" Value="Location_Name_L"></asp:ListItem>
                                                        <asp:ListItem Text="<%$ Resources:Attendance,Parent Location Name %>" Value="ParentLocation"></asp:ListItem>
                                                        <asp:ListItem Text="<%$ Resources:Attendance,Location Code%>" Value="Location_Code"></asp:ListItem>
                                                        <%--<asp:ListItem Text="<%$ Resources:Attendance,Location Type%>" Value="Location_Type"></asp:ListItem>--%>
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
                                                        <asp:TextBox placeholder="Search from Content" ID="txtValue" runat="server" CssClass="form-control"></asp:TextBox>
                                                    </asp:Panel>
                                                </div>
                                                <div class="col-lg-2" style="text-align: center;">
                                                    <asp:LinkButton ID="btnbind" runat="server" CausesValidation="False" OnClick="btnbind_Click" ToolTip="<%$ Resources:Attendance,Search %>"><span class="fa fa-search"  style="font-size:25px;"></span></asp:LinkButton>&nbsp;&nbsp;
                                                            <asp:LinkButton ID="ImageButton1" runat="server" CausesValidation="False" OnClick="btnRefresh_Click" ToolTip="<%$ Resources:Attendance,Refresh %>"><span class="fa fa-repeat"  style="font-size:25px;"></span></asp:LinkButton>&nbsp;&nbsp
                                                    <asp:LinkButton ID="btnGvListSetting" ImageAlign="Right" ToolTip="List Settings" runat="server" OnClick="btnGvListSetting_Click" Visible="false"><span class="fa fa-wrench"  style="font-size:25px;"></span></asp:LinkButton>
                                                </div>
                                                <asp:HiddenField ID="HDFSort" runat="server" />
                                                <asp:HiddenField ID="hdntxtaddressid" runat="server" />
                                                <asp:HiddenField ID="hdnEmployeeId" runat="server" />
                                            </div>
                                        </div>
                                    </div>
                                </div>
                                <div class="box box-warning box-solid" <%= gvLocationMaster.Rows.Count>0?"style='display:block'":"style='display:none'"%>>
                                    <div class="box-body">
                                        <div class="row">
                                            <div class="col-md-12">
                                                <div style="overflow: auto">
                                                    <asp:GridView CssClass="table-striped table-bordered table table-hover" ID="gvLocationMaster" PageSize="<%# PageControlCommon.GetPageSize() %>"
                                                        runat="server" AutoGenerateColumns="False" Width="100%" AllowPaging="True" AllowSorting="True"
                                                        OnPageIndexChanging="gvLocationMaster_PageIndexChanging" OnSorting="gvLocationMaster_OnSorting">
                                                        <Columns>

                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,Action%>">
                                                                <ItemTemplate>
                                                                    <div class="dropdown" style="position: absolute;">
                                                                        <button class="btn btn-default dropdown-toggle" type="button" data-toggle="dropdown">
                                                                            <i class="fa fa-ellipsis-h" aria-hidden="true"></i>
                                                                        </button>
                                                                        <ul class="dropdown-menu">

                                                                            <li <%= hdnCanEdit.Value=="true"?"style='display:block'":"style='display:none'" %>>
                                                                                <asp:LinkButton ID="btnEdit" runat="server" CommandArgument='<%# Eval("Location_Id") %>' CausesValidation="False" OnCommand="btnEdit_Command"><i class="fa fa-pencil"></i><%# Resources.Attendance.Edit%></asp:LinkButton>
                                                                            </li>
                                                                            <li <%= hdnCanDelete.Value=="true"?"style='display:block'":"style='display:none'" %>>
                                                                                <asp:LinkButton ID="IbtnDelete" runat="server" CausesValidation="False" CommandArgument='<%# Eval("Location_Id") %>' OnCommand="IbtnDelete_Command"><i class="fa fa-trash"></i><%# Resources.Attendance.Delete%></asp:LinkButton>
                                                                                <cc1:ConfirmButtonExtender ID="ConfirmButtonExtender1" runat="server" TargetControlID="IbtnDelete" ConfirmText="Are you sure to Delete Location?"></cc1:ConfirmButtonExtender>
                                                                            </li>
                                                                        </ul>
                                                                    </div>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,Location Name %>" SortExpression="Location_Name">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblLocationId1" runat="server" Text='<%# Eval("Location_Id") %>' Visible="false"></asp:Label>
                                                                    <asp:Label ID="lbELocationName" runat="server" Text='<%# Eval("Location_Name") %>'></asp:Label>
                                                                </ItemTemplate>
                                                                <ItemStyle />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,Location Name(Local) %>"
                                                                SortExpression="Location_Name_L">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblLocationName" runat="server" Text='<%# Eval("Location_Name_L") %>'></asp:Label>
                                                                </ItemTemplate>
                                                                <ItemStyle />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,Location Code %>" SortExpression="Location_Code">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblLocCode" runat="server" Text='<%# Eval("Location_Code") %>'></asp:Label>
                                                                </ItemTemplate>
                                                                <ItemStyle />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,Parent Location Name %>"
                                                                SortExpression="ParentLocation">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblLocCode1" runat="server" Text='<%# Eval("ParentLocation") %>'></asp:Label>
                                                                </ItemTemplate>
                                                                <ItemStyle />
                                                            </asp:TemplateField>
                                                            <%-- <asp:TemplateField HeaderText="<%$ Resources:Attendance,Location Type %>" SortExpression="Location_Type">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblConversion_Location" runat="server" Text='<%# Eval("Location_Type") %>'></asp:Label>
                                                                </ItemTemplate>
                                                                <ItemStyle />
                                                            </asp:TemplateField>--%>
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
                                <div runat="server" id="pnlLoc" class="row">
                                    <div class="col-md-12">
                                        <div class="col-md-12">
                                            <div class="box box-danger">
                                                <div class="box-header with-border">
                                                    <div class="col-md-12">
                                                        <asp:ImageButton ID="btnControlsSetting" ImageAlign="Right" ToolTip="Controls Setting" runat="server" ImageUrl="~/Images/setting.png" OnClick="btnControlsSetting_Click" Style="width: 32px; height: 32px" Visible="false" />
                                                    </div>
                                                    <div class="col-md-6">
                                                        <asp:Label ID="lblLocationCode" runat="server" Text="<%$ Resources:Attendance,Location Code %>"></asp:Label><a style="color: Red">*</a>
                                                        <asp:RequiredFieldValidator EnableClientScript="true" Style="float: right;" runat="server" ID="RequiredFieldValidator1" ValidationGroup="Location_Save"
                                                            Display="Dynamic" SetFocusOnError="true" ControlToValidate="txtLocationCode" ErrorMessage="<%$ Resources:Attendance,Enter Location Code %>" />
                                                        <asp:TextBox ID="txtLocationCode" BackColor="#eeeeee" MaxLength="11" runat="server" CssClass="form-control"></asp:TextBox>
                                                        <cc1:AutoCompleteExtender ID="AutoCompleteExtender3" runat="server" DelimiterCharacters=""
                                                            Enabled="True" ServiceMethod="GetCompletionListLocationCode" ServicePath="" CompletionInterval="100"
                                                            MinimumPrefixLength="1" CompletionSetCount="1" TargetControlID="txtLocationCode"
                                                            UseContextKey="True" CompletionListCssClass="autoCompleteList" CompletionListItemCssClass="autoCompleteListItem" CompletionListHighlightedItemCssClass="autoCompleteSelectedListItem" OnClientShown="resetPosition">
                                                        </cc1:AutoCompleteExtender>
                                                        <br />
                                                        <asp:Label ID="lblLocationName" runat="server" Text="<%$ Resources:Attendance,Location Name %>"></asp:Label><a style="color: Red">*</a>
                                                        <asp:RequiredFieldValidator EnableClientScript="true" Style="float: right;" runat="server" ID="RequiredFieldValidator2" ValidationGroup="Location_Save"
                                                            Display="Dynamic" SetFocusOnError="true" ControlToValidate="txtLocationName" ErrorMessage="<%$ Resources:Attendance,Enter Location Name %>" />
                                                        <asp:TextBox ID="txtLocationName" AutoPostBack="true" OnTextChanged="txtLocation_OnTextChanged"
                                                            BackColor="#eeeeee" runat="server" CssClass="form-control"></asp:TextBox>
                                                        <cc1:AutoCompleteExtender ID="AutoCompleteExtender1" runat="server" DelimiterCharacters=""
                                                            Enabled="True" ServiceMethod="GetCompletionListLocationName" ServicePath="" CompletionInterval="100"
                                                            MinimumPrefixLength="1" CompletionSetCount="1" TargetControlID="txtLocationName"
                                                            UseContextKey="True" CompletionListCssClass="autoCompleteList" CompletionListItemCssClass="autoCompleteListItem" CompletionListHighlightedItemCssClass="autoCompleteSelectedListItem" OnClientShown="resetPosition">
                                                        </cc1:AutoCompleteExtender>
                                                        <br />
                                                        <asp:Label ID="lblLocationNameL" runat="server" Text="<%$ Resources:Attendance,Location Name(Local) %>"></asp:Label>
                                                        <asp:TextBox ID="txtLocationNameL" runat="server" CssClass="form-control"></asp:TextBox>
                                                        <br />
                                                        <asp:Label ID="Label12" runat="server" Text="<%$ Resources:Attendance,Brand/Division %>"></asp:Label>
                                                        <asp:DropDownList ID="ddlBrand" class="form-control select2" Style="width: 100%;" runat="server" />

                                                        <br />
                                                        <asp:Label ID="lblParentLocation" runat="server" Text="<%$ Resources:Attendance,Parent Location Name %>"></asp:Label>
                                                        <asp:DropDownList ID="ddlParentLocation" runat="server" CssClass="form-control" />
                                                        <br />
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
                                                    <div class="col-md-6">
                                                        <div style="text-align: center;">
                                                            <asp:Label ID="lblLocationLogo" runat="server" Text="<%$ Resources:Attendance,Location Logo %>"></asp:Label>
                                                            <div class="input-group" style="width: 100%; display: none">
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
                                                                    <asp:Image ID="FULogo_Img_Right" runat="server" Width="30px" Height="30px" ImageUrl="../Images/Allow.png" Style="display: none" />
                                                                    <asp:Image ID="FULogo_Img_Wrong" runat="server" Width="30px" Height="30px" ImageUrl="../Images/Delete1.png" Style="display: none" />
                                                                    <asp:Image ID="FULogo_ImgLoader" runat="server" ImageUrl="../Images/loader.gif" />
                                                                </div>
                                                            </div>
                                                            <br />
                                                            <asp:Image ID="imgLogo" ClientIDMode="Static" runat="server" ImageUrl="../Bootstrap_Files/dist/img/NoImage.jpg" Width="90px" Height="90px" />
                                                            <br />
                                                            <br />
                                                            <br />
                                                            &nbsp;
                                                            <a onclick="popup.Show();" runat="server" id="File_Manager_Product" class="btn btn-primary" style="cursor: pointer">File Manager</a>

                                                            <asp:Button ID="btnUpload" Visible="false" OnClick="btnUpload_Click" runat="server" Text="<%$ Resources:Attendance,Load %>"
                                                                class="btn btn-primary" />
                                                        </div>
                                                        <div>
                                                            <br />
                                                            <br />
                                                            <asp:Label ID="Label3" runat="server" Text="<%$ Resources:Attendance,Manager %>"></asp:Label>
                                                            <br />
                                                            <asp:TextBox ID="txtManagerName" BackColor="#eeeeee" 
                                                                CssClass="col-md-10" Height="35px" runat="server" />
                                                            <cc1:AutoCompleteExtender ID="AutoCompleteExtender5" runat="server" DelimiterCharacters=""
                                                                Enabled="True" ServiceMethod="GetCompletionListEmployeeName" ServicePath="" CompletionInterval="100"
                                                                MinimumPrefixLength="1" CompletionSetCount="1" TargetControlID="txtManagerName"
                                                                UseContextKey="True" CompletionListCssClass="autoCompleteList" CompletionListItemCssClass="autoCompleteListItem" CompletionListHighlightedItemCssClass="autoCompleteSelectedListItem" OnClientShown="resetPosition">
                                                            </cc1:AutoCompleteExtender>
                                                            &nbsp;
                                                                <asp:Button ID="btnNewEmployee" runat="server" CssClass="btn btn-primary" Text="New"  OnClick="btnNewEmployee_Click" />
                                                        </div>
                                                        <div>
                                                            <br />
                                                            <asp:Label ID="lblManager" runat="server" Text="<%$ Resources:Attendance,Location Type %>"></asp:Label>


                                                            <asp:DropDownList ID="ddlLocationType" runat="server" CssClass="form-control">
                                                            </asp:DropDownList>

                                                        </div>

                                                    </div>

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
                                                                            <asp:LinkButton ID="imgBtnDelete" runat="server" CommandArgument='<%# Eval("Trans_Id") %>' OnCommand="btnAddressDelete_Command"><i class="fa fa-trash"></i></asp:LinkButton>
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



                                                    <div class="col-md-6">
                                                        <asp:Label ID="Label4" runat="server" Text="<%$ Resources:Attendance,Currency Name %>"></asp:Label>
                                                        <a style="color: Red">*</a>
                                                        <asp:RequiredFieldValidator EnableClientScript="true" Style="float: right;" runat="server" ID="RequiredFieldValidator5" ValidationGroup="Location_Save" Display="Dynamic"
                                                            SetFocusOnError="true" ControlToValidate="ddlCurrency" InitialValue="--Select--" ErrorMessage="<%$ Resources:Attendance,Select Currency Name %>" />
                                                        <asp:DropDownList ID="ddlCurrency" runat="server" CssClass="form-control" />

                                                    </div>

                                                    <div class="col-md-6">
                                                        <asp:Label ID="Label1" runat="server" Text="Labour Law"></asp:Label>
                                                        <asp:DropDownList ID="ddlLabour" runat="server" CssClass="form-control"></asp:DropDownList>

                                                        <br />
                                                    </div>
                                                    <div class="col-md-6" runat="server" visible="false" id="ctlGSTIN">
                                                        <asp:Label ID="lblGSTIN" runat="server" Text="<%$ Resources:Attendance,GSTIN No%>" />
                                                        <asp:TextBox ID="txtGSTIN" runat="server" CssClass="form-control" MaxLength="15" />
                                                        <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender10" runat="server" Enabled="True"
                                                            TargetControlID="txtGSTIN" FilterType="Custom" ValidChars="1234567890abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ">
                                                        </cc1:FilteredTextBoxExtender>
                                                        <br />
                                                    </div>
                                                    <div class="col-md-6" runat="server" visible="false" id="ctlTRN_No">
                                                        <asp:Label ID="Label2" runat="server" Text="<%$ Resources:Attendance,TRN No%>" />
                                                        <asp:TextBox ID="Txt_TRN_No" runat="server" CssClass="form-control" MaxLength="20" />
                                                        <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" runat="server" Enabled="True"
                                                            TargetControlID="Txt_TRN_No" FilterType="Custom" ValidChars="1234567890abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ">
                                                        </cc1:FilteredTextBoxExtender>
                                                        <br />
                                                    </div>
                                                    <div class="col-md-12">
                                                        <div style="text-align: center">
                                                            <br />
                                                           
                                                            <asp:Button ID="btnReset" OnClick="btnReset_Click" CausesValidation="False" Text="<%$ Resources:Attendance,Reset %>"
                                                                CssClass="btn btn-primary" runat="server" />
                                                            <asp:Button ID="btnCancel" OnClick="btnCancel_Click" CausesValidation="False" Text="<%$ Resources:Attendance,Cancel %>"
                                                                CssClass="btn btn-danger" runat="server" />
                                                             <asp:Button ID="btnSave" OnClick="btnSave_Click" CssClass="btn btn-success" runat="server"
                                                                Text="<%$ Resources:Attendance,Next %>" ValidationGroup="Location_Save" Visible="false" />
                                                            <asp:HiddenField ID="editid" runat="server" />
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                        <asp:UpdatePanel ID="Update_Department" runat="server">
                            <ContentTemplate>

                                <div id="pnlLocDept" visible="false" runat="server" class="row">
                                    <div class="col-md-12">
                                        <div class="col-md-12">
                                            <div class="box box-danger">
                                                <div class="box-header with-border">
                                                    <div class="col-md-12">
                                                        <div class="col-md-6">
                                                            <div style="text-align: center;">
                                                                <asp:Label runat="server" Font-Bold="true" ID="Label7" Text="<%$ Resources:Attendance,Location Code %>">:</asp:Label>
                                                                &nbsp : &nbsp
                                                        <asp:Label runat="server" Font-Bold="true" ID="lblLocationId1"></asp:Label>
                                                                <br />

                                                            </div>
                                                        </div>
                                                        <div class="col-md-6">

                                                            <div style="text-align: center;">
                                                                <asp:Label runat="server" Font-Bold="true" ID="Label8" Text="<%$ Resources:Attendance,Location %>">:</asp:Label>
                                                                &nbsp : &nbsp
                                                        <asp:Label runat="server" Font-Bold="true" ID="lblLocationName1"></asp:Label>
                                                            </div>
                                                        </div>
                                                    </div>
                                                    <div class="col-md-12" style="text-align: center">
                                                        <br />
                                                        <asp:Label runat="server" Font-Bold="true" ID="lblSelectDept" Text="<%$ Resources:Attendance,Select Department %>"></asp:Label>
                                                        <br />
                                                    </div>
                                                    <div class="col-md-6">
                                                        <div class="flow">
                                                            <br />
                                                            <asp:CheckBox ID="chkSelectAll" runat="server" Text="<%$ Resources:Attendance,Select All %>"
                                                                AutoPostBack="true" OnCheckedChanged="ChkSelectAll_OnCheckedChanged" />

                                                            <asp:TreeView ID="TreeViewDepartment" runat="server" OnSelectedNodeChanged="navTree_SelectedNodeChanged1" OnTreeNodeCheckChanged="navTree_TreeNodeCheckChanged"></asp:TreeView>
                                                            <br />
                                                        </div>
                                                    </div>
                                                    <div class="col-md-6">

                                                        <br />
                                                        <br />
                                                        <br />
                                                        <asp:ListBox ID="lstDepartment" runat="server" Height="155px" Width="100%" SelectionMode="Multiple" CssClass="list" Visible="false"></asp:ListBox>
                                                        <br />
                                                        <br />
                                                        <div style="text-align: center;">
                                                            <asp:Button ID="btnPushDept" runat="server" Visible="false" Style="transform: rotate(90deg); width: 20px; height: 20px;" BorderStyle="None" BackColor="Transparent" CssClass="btnPush" OnClick="btnPushDept_Click" />

                                                            <asp:Button ID="btnPullDept" runat="server" Visible="false" Style="transform: rotate(90deg); width: 20px; height: 20px;" BorderStyle="None" BackColor="Transparent" CssClass="btnPull" OnClick="btnPullDept_Click" />

                                                            <asp:Button ID="btnPushAllDept" runat="server" Visible="false" Style="transform: rotate(90deg); width: 20px; height: 20px;" BorderStyle="None" BackColor="Transparent" CssClass="btnPushAll" OnClick="btnPushAllDept_Click" />

                                                            <asp:Button ID="btnPullAllDept" runat="server" Visible="false" Style="transform: rotate(90deg); width: 20px; height: 20px;" BorderStyle="None" BackColor="Transparent" CssClass="btnPullAll" OnClick="btnPullAllDept_Click" />
                                                        </div>
                                                        <br />
                                                        <asp:ListBox ID="lstDepartmentSelect" Visible="false" runat="server" Height="155px" Width="100%" SelectionMode="Multiple" CssClass="list"></asp:ListBox>
                                                        <br />
                                                        <br />
                                                        <div style="text-align: center;">
                                                            <asp:Button ID="btnSaveDept" runat="server" Text="<%$ Resources:Attendance,Save %>" CssClass="btn btn-success" ValidationGroup="a" OnClick="btnSaveDept_Click" />
                                                            <asp:Button ID="btnResetDept" runat="server" Text="<%$ Resources:Attendance,Reset %>" CssClass="btn btn-primary" CausesValidation="False" OnClick="btnResetDept_Click" />
                                                            <asp:Button ID="btnCancelDept" runat="server" Text="<%$ Resources:Attendance,Cancel %>" CssClass="btn btn-danger" CausesValidation="False" OnClick="btnCancelDept_Click" />
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                        <dx:ASPxPopupControl ID="ASPxPopupControl1" ClientInstanceName="popup" runat="server" Height="500px" Width="1000px" Modal="true" PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter">
                            <ClientSideEvents Shown="function(s,e){
                    fileManager.AdjustControl();
                }" />

                            <ContentCollection>
                                <dx:PopupControlContentControl runat="server" SupportsDisabledAttribute="True">

                                    <dx:ASPxFileManager ID="ASPxFileManager1" runat="server" ClientInstanceName="fileManager">
                                        <Settings AllowedFileExtensions=".jpg,.jpeg,.gif,.rtf,.txt,.avi,.png,.mp3,.xml,.doc,.pdf" EnableMultiSelect="false" />
                                        <SettingsEditing AllowCreate="true" AllowDelete="true" AllowMove="true" AllowRename="true" AllowCopy="true" AllowDownload="true" />
                                        <SettingsToolbar ShowDownloadButton="true" />
                                        <SettingsFileList View="Thumbnails">
                                            <ThumbnailsViewSettings ThumbnailSize="100" />
                                            <DetailsViewSettings AllowColumnResize="true" AllowColumnDragDrop="true" AllowColumnSort="true" ShowHeaderFilterButton="false">
                                                <Columns>
                                                    <dx:FileManagerDetailsColumn FileInfoType="Thumbnail" />
                                                    <dx:FileManagerDetailsColumn FileInfoType="FileName" />
                                                    <dx:FileManagerDetailsColumn FileInfoType="LastWriteTime" />
                                                    <dx:FileManagerDetailsColumn FileInfoType="Size" />
                                                </Columns>
                                            </DetailsViewSettings>
                                        </SettingsFileList>
                                        <SettingsUpload Enabled="true" />
                                        <ClientSideEvents SelectedFileOpened="function(s, e) {
    // Check if e.file and e.file.extension are defined
                                  debugger;
    if (e.file.name) {
       
        var fileExtension = e.file.name.substring(e.file.name.lastIndexOf('.') + 1);

        // Check if the file is an image (you can customize the list of allowed extensions)
        var isImage = ['jpg', 'jpeg', 'png', 'gif'].includes(fileExtension);


        if (!isImage) {
            alert('Invalid file type. Only images are allowed.');
            return false;
        }
    } else {
        // Handle the case where e.file or e.file.extension is undefined
        alert('Unable to determine file type.');
        return false;
    }

    var image = $('#imgLogo')[0];
    image.src = e.file.imageSrc;

    var subproduct = s.currentPath;
    if (subproduct != '') {
        subproduct = '/' + subproduct;
        subproduct = subproduct.replaceAll('\\', '/');
    }

    var dataa = '~/'+ s.rootFolderName + subproduct + '/' + e.file.name;
    dataa = dataa.replaceAll('/', '//');

    // Call the server-side method only if it's an image
    PageMethods.ASPxFileManager1_SelectedFileOpened(dataa, e.file.name, function(data) {}, function(data) {});

    popup.Hide();
    return false;
}" />

                                    </dx:ASPxFileManager>

                                </dx:PopupControlContentControl>
                            </ContentCollection>
                        </dx:ASPxPopupControl>
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



                                        <div class="row">
                                            <div class="col-md-12">
                                                <div id="Div2" runat="server" class="box box-info collapsed-box">
                                                    <div class="box-header with-border">
                                                        <h3 class="box-title">
                                                            <asp:Label ID="Label11" runat="server" Text="<%$ Resources:Attendance,Advance Search%>"></asp:Label></h3>
                                                        &nbsp;&nbsp;|&nbsp;&nbsp;
					  <asp:Label ID="lblbinTotalRecords" Text="<%$ Resources:Attendance,Total Records: 0 %>" runat="server"></asp:Label>

                                                        <div class="box-tools pull-right">
                                                            <button type="button" class="btn btn-box-tool" data-widget="collapse">
                                                                <i id="I2" runat="server" class="fa fa-plus"></i>
                                                            </button>
                                                        </div>
                                                    </div>
                                                    <div class="box-body">

                                                        <div class="col-lg-3">
                                                            <asp:DropDownList ID="ddlbinFieldName" runat="server" CssClass="form-control">
                                                                <asp:ListItem Selected="True" Text="<%$ Resources:Attendance,Location Name %>" Value="Location_Name"></asp:ListItem>
                                                                <asp:ListItem Text="<%$ Resources:Attendance,Location Name(Local) %>" Value="Location_Name_L"></asp:ListItem>
                                                                <asp:ListItem Text="<%$ Resources:Attendance,Parent Location Name %>" Value="ParentLocation"></asp:ListItem>
                                                                <asp:ListItem Text="<%$ Resources:Attendance,Location Code%>" Value="Location_Code"></asp:ListItem>
                                                                <asp:ListItem Text="<%$ Resources:Attendance,Location Type%>" Value="Location_Type"></asp:ListItem>
                                                                <asp:ListItem Text="<%$ Resources:Attendance,Location Id %>" Value="Location_Id"></asp:ListItem>
                                                            </asp:DropDownList>
                                                        </div>
                                                        <div class="col-lg-2">
                                                            <asp:DropDownList ID="ddlbinOption" runat="server" CssClass="form-control">
                                                                <asp:ListItem Text="<%$ Resources:Attendance,--Select-- %>"></asp:ListItem>
                                                                <asp:ListItem Text="<%$ Resources:Attendance,Equal %>"></asp:ListItem>
                                                                <asp:ListItem Selected="True" Text="<%$ Resources:Attendance,Contains %>"></asp:ListItem>
                                                                <asp:ListItem Text="<%$ Resources:Attendance,Like %>"></asp:ListItem>
                                                            </asp:DropDownList>
                                                        </div>
                                                        <div class="col-lg-5">
                                                            <asp:Panel ID="Panel2" runat="server" DefaultButton="btnbinbind">
                                                                <asp:TextBox ID="txtbinValue" placeholder="Search from Content" class="form-control" runat="server"></asp:TextBox>
                                                            </asp:Panel>
                                                        </div>
                                                        <div class="col-lg-2" style="text-align: center;">
                                                            <asp:LinkButton ID="btnbinbind" runat="server" CausesValidation="False" OnClientClick="tab_3_open()" OnClick="btnbinbind_Click" ToolTip="<%$ Resources:Attendance,Search %>"><span class="fa fa-search"  style="font-size:25px;"></span></asp:LinkButton>&nbsp;&nbsp;&nbsp;
                                                            <asp:LinkButton ID="btnbinRefresh" runat="server" CausesValidation="False" OnClientClick="tab_3_open()" OnClick="btnbinRefresh_Click" ToolTip="<%$ Resources:Attendance,Refresh %>"><span class="fa fa-repeat"  style="font-size:25px;"></span></asp:LinkButton>&nbsp;&nbsp;&nbsp;
                                                            <asp:LinkButton ID="imgBtnRestore" CausesValidation="False" Visible="false" runat="server" OnClick="imgBtnRestore_Click" ToolTip="<%$ Resources:Attendance, Active %>"> <span class="far fa-lightbulb"  style="font-size:25px;"></span></asp:LinkButton>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>

                                        <div class="box box-warning box-solid" <%= gvLocationMasterBin.Rows.Count>0?"style='display:block'":"style='display:none'"%>>
                                            <div class="box-header with-border">
                                                <h3 class="box-title"></h3>
                                            </div>
                                            <div class="box-body">
                                                <div class="row">
                                                    <div class="col-md-12">
                                                        <div style="overflow: auto">
                                                            <asp:GridView CssClass="table-striped table-bordered table table-hover" ID="gvLocationMasterBin" PageSize="<%# PageControlCommon.GetPageSize() %>"
                                                                runat="server" AutoGenerateColumns="False" Width="100%" AllowPaging="True" OnPageIndexChanging="gvLocationMasterBin_PageIndexChanging"
                                                                OnSorting="gvLocationMasterBin_OnSorting" AllowSorting="true" DataKeyNames="Location_Id">
                                                                <Columns>
                                                                    <asp:TemplateField>
                                                                        <ItemTemplate>
                                                                            <asp:CheckBox ID="chkgvSelect" runat="server" />
                                                                        </ItemTemplate>
                                                                        <HeaderTemplate>
                                                                            <asp:CheckBox ID="chkgvSelectAll" runat="server" OnCheckedChanged="chkgvSelectAll_CheckedChanged"
                                                                                AutoPostBack="true" />
                                                                        </HeaderTemplate>
                                                                        <ItemStyle HorizontalAlign="Center" />
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="<%$ Resources:Attendance,Location Id %>" SortExpression="Location_Id">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblLocationId1" runat="server" Text='<%# Eval("Location_Id") %>'></asp:Label>
                                                                        </ItemTemplate>
                                                                        <ItemStyle />
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="<%$ Resources:Attendance,Location Name %>" SortExpression="Location_Name">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lbELocationName" runat="server" Text='<%# Eval("Location_Name") %>'></asp:Label>
                                                                            <asp:Label ID="lblLocationId" Visible="false" runat="server" Text='<%# Eval("Location_Id") %>'></asp:Label>
                                                                        </ItemTemplate>
                                                                        <ItemStyle />
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="<%$ Resources:Attendance,Location Name(Local) %>"
                                                                        SortExpression="Location_Name_L">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblLocationName" runat="server" Text='<%# Eval("Location_Name_L") %>'></asp:Label>
                                                                        </ItemTemplate>
                                                                        <ItemStyle />
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="<%$ Resources:Attendance,Parent Location Name %>"
                                                                        SortExpression="ParentLocation">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblLocCode1" runat="server" Text='<%# Eval("ParentLocation") %>'></asp:Label>
                                                                        </ItemTemplate>
                                                                        <ItemStyle />
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="<%$ Resources:Attendance,Location Code %>" SortExpression="Location_Code">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblLocCode" runat="server" Text='<%# Eval("Location_Code") %>'></asp:Label>
                                                                        </ItemTemplate>
                                                                        <ItemStyle />
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="<%$ Resources:Attendance,Location Type %>" SortExpression="Location_Type">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblConversion_Location" runat="server" Text='<%# Eval("Location_Type") %>'></asp:Label>
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
                                    </div>
                                </div>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                   
                    </div>
                </div>
            </div>
        </div>
             <div class="modal fade" id="EmployeeModal" tabindex="-1" role="dialog" aria-labelledby="EmployeeLabel" aria-hidden="true">
                            <div class="modal-dialog modal-lg">
                                <div class="modal-content">
                                    <div class="modal-body">
                                        <asp:UpdatePanel ID="Update_Employee" runat="server">
                                            <ContentTemplate>
                                                <EPM:EmployeeMaster ID="addEmployee" runat="server" />
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </div>
                                    <div class="modal-footer">
                                        <button type="button" class="btn btn-danger" data-dismiss="modal">
                                            Close
                                        </button>
                                    </div>
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
    <div class="modal fade" id="myModal" tabindex="-1" role="dialog" aria-labelledby="myModalLabel"
        aria-hidden="true">
        <div class="modal-dialog modal-lg">
            <div class="modal-content">
                <div class="modal-body">
                    <asp:UpdatePanel ID="Update_Modal" runat="server">
                        <ContentTemplate>
                            <UC:Addressmaster ID="addaddress" runat="server" />
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
                <div class="modal-footer">
                    <button type="button" class="btn btn-danger" data-dismiss="modal">
                        Close</button>
                </div>
            </div>
        </div>
    </div>
    
    <asp:UpdateProgress ID="UpdateProgress5" runat="server" AssociatedUpdatePanelID="Update_Department">
        <ProgressTemplate>
            <div class="modal_Progress">
                <div class="center_Progress">
                    <img alt="" src="../Images/ajax-loader2.gif" style="vertical-align: middle; background-color: White; border-radius: 10px;" />
                </div>
            </div>
        </ProgressTemplate>
    </asp:UpdateProgress>
        <asp:UpdateProgress ID="UpdateProgress6" runat="server" AssociatedUpdatePanelID="Update_Employee">
        <ProgressTemplate>
            <div class="modal_Progress">
                <div class="center_Progress">
                    <img alt="" src="../Images/ajax-loader2.gif" style="vertical-align: middle; background-color: White; border-radius: 10px;" />
                </div>
            </div>
        </ProgressTemplate>
    </asp:UpdateProgress>

    <asp:UpdateProgress ID="UpdateProgress1" runat="server" AssociatedUpdatePanelID="Update_Button">
        <ProgressTemplate>
            <div class="modal_Progress">
                <div class="center_Progress">
                    <img alt="" src="../Images/ajax-loader2.gif" style="vertical-align: middle; background-color: White; border-radius: 10px;" />
                </div>
            </div>
        </ProgressTemplate>
    </asp:UpdateProgress>

    <asp:UpdateProgress ID="UpdateProgress3" runat="server" AssociatedUpdatePanelID="Update_Tab_Name">
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

    <asp:UpdateProgress ID="UpdateProgress8" runat="server" AssociatedUpdatePanelID="Update_New">
        <ProgressTemplate>
            <div class="modal_Progress">
                <div class="center_Progress">
                    <img alt="" src="../Images/ajax-loader2.gif"  style="vertical-align: middle; background-color: White; border-radius: 10px;" />
                </div>
            </div>
        </ProgressTemplate>
    </asp:UpdateProgress>

    <asp:UpdateProgress ID="UpdateProgress4" runat="server" AssociatedUpdatePanelID="Update_Bin">
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
            if (charCode == 46 && (elementRef.value.indexOf('.') >= 0))
                return false;
            else if (charCode == 46 && (elementRef.value.indexOf('.') <= 0))
                return true;
            return true;
        }

        function Li_Tab_New() {
            document.getElementById('<%= Btn_New.ClientID %>').click();
        }
        function Li_Tab_Bin() {
            document.getElementById('<%= Btn_Bin.ClientID %>').click();
        }
        function Modal_Address_Open() {
            document.getElementById('<%= Btn_Address_Modal.ClientID %>').click();
        }
        function Modal_Address_Close() {
            document.getElementById('<%= Btn_Address_Modal.ClientID %>').click();
            document.getElementById('<%= imgAddAddressName.ClientID %>').click();
        }
        function Modal_Employee_Open() {
            document.getElementById('<%= Btn_Employee_New.ClientID %>').click();
         }
        function LI_New_Active() {
            $("#Li_List").removeClass("active");
            $("#List").removeClass("active");

            $("#Li_New").addClass("active");
            $("#New").addClass("active");
        }


        function LI_List_Active() {
            $("#Li_New").removeClass("active");
            $("#New").removeClass("active");

            $("#Li_List").addClass("active");
            $("#List").addClass("active");
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

        function postBackByObject() {
            var o = window.event.srcElement;
            if (o.tagName == "INPUT" && o.type == "checkbox") {
                __doPostBack("<%= TreeViewDepartment.ClientID%>", "");
            }
        }
        function Modal_Close() {
            document.getElementById('<%= Btn_Address_Modal.ClientID %>').click();
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
            if (filext == "png" || filext == "jpg" || filext == "jpeg") {
                return true;
            }
            else {
                throw {
                    name: "Invalid File Type",
                    level: "Error",
                    message: "Invalid File Type, Select Only .png, .jpg, .jpeg extension file",
                    htmlMessage: "Invalid File Type, Select Only .png, .jpg, .jpeg extension file"
                }
                return false;
            }
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
        function showUcControlsSettings() {
            $('#ControlSettingModal').modal('show');
        }
        function resetPosition1() {

        }
        function CreateNewEmployee() {
            window.open('../MasterSetUp/EmployeeMaster.aspx');
        }
    </script>
</asp:Content>
