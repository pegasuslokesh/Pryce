<%@ Page Title="" Language="C#" MasterPageFile="~/ERPMaster.master" AutoEventWireup="true" CodeFile="AddressMaster.aspx.cs" Inherits="SystemSetUp_AddressMaster" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="Head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="HeaderContent" runat="Server">
    <h1>
        <i class="fas fa-map-pin"></i>
        <asp:Label ID="lblHeader" runat="server" Text="<%$ Resources:Attendance,Address Setup%>"></asp:Label>

        <%--<small>Control panel</small>--%>
    </h1>
    <ol class="breadcrumb">
        <li><a href="#"><i class="fa fa-dashboard"></i>
            <asp:Label ID="Label6" runat="server" Text="<%$ Resources:Attendance,System SetUp%>"></asp:Label></a></li>
        <li><a href="#"><i class="fa fa-dashboard"></i>
            <asp:Label ID="Label10" runat="server" Text="<%$ Resources:Attendance,System SetUp%>"></asp:Label></a></li>
        <li class="active">
            <asp:Label ID="Label5" runat="server" Text="<%$ Resources:Attendance,Address Setup%>"></asp:Label></li>
    </ol>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="Server">

    <div class="row">
        <div class="col-md-12">
            <div class="nav-tabs-custom">
                <asp:HiddenField runat="server" ID="hdnCanEdit" />
                <asp:HiddenField runat="server" ID="hdnCanDelete" />
                <ul class="nav nav-tabs pull-right bg-blue-gradient">
                    <li><a href="#Bin" data-toggle="tab">
                        <i class="fa fa-trash"></i>&nbsp;&nbsp;<asp:Label ID="Label13" runat="server" Text="<%$ Resources:Attendance,Bin %>"></asp:Label></a></li>
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
                                                    <asp:HiddenField ID="HDFSort" runat="server" />
                                                    <asp:DropDownList ID="ddlFieldName" runat="server" CssClass="form-control">
                                                        <asp:ListItem Text="<%$ Resources:Attendance, Address Category %>" Value="AddressCategoryName"></asp:ListItem>
                                                        <asp:ListItem Text="<%$ Resources:Attendance, Address Name %>" Value="Address_Name"
                                                            Selected="True"></asp:ListItem>
                                                        <asp:ListItem Text="<%$ Resources:Attendance,Email Id %>" Value="EmailId1"></asp:ListItem>
                                                        <asp:ListItem Text="<%$ Resources:Attendance,Website%>" Value="Website"></asp:ListItem>
                                                        <asp:ListItem Text="<%$ Resources:Attendance,Fax No%>" Value="FaxNo"></asp:ListItem>
                                                        <asp:ListItem Text="<%$ Resources:Attendance,City%>" Value="CityId"></asp:ListItem>
                                                        <asp:ListItem Text="<%$ Resources:Attendance,State%>" Value="StateId"></asp:ListItem>
                                                        <asp:ListItem Text="<%$ Resources:Attendance,Country%>" Value="CountryName"></asp:ListItem>
                                                        <asp:ListItem Text="<%$ Resources:Attendance,Created By%>" Value="Created_User"></asp:ListItem>
                                                        <asp:ListItem Text="<%$ Resources:Attendance,Modified By%>" Value="Modified_User"></asp:ListItem>
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
                                <div class="box box-warning box-solid" <%= GvAddress.Rows.Count>0?"style='display:block'":"style='display:none'"%>>

                                    <div class="box-body">
                                        <div class="row">
                                            <div class="col-md-12">
                                                <div class="flow">

                                                    <asp:GridView CssClass="table-striped table-bordered table table-hover" ID="GvAddress" PageSize='<%# int.Parse(Session["GridSize"].ToString()) %>' runat="server"
                                                        AutoGenerateColumns="False" Width="100%" AllowPaging="True" OnPageIndexChanging="GvAddress_PageIndexChanging"
                                                        AllowSorting="True" OnSorting="GvAddress_Sorting">

                                                        <Columns>


                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,Action%>">
                                                                <ItemTemplate>
                                                                    <div class="dropdown" style="position: absolute;">
                                                                        <button class="btn btn-default dropdown-toggle" type="button" data-toggle="dropdown">
                                                                            <i class="fa fa-ellipsis-h" aria-hidden="true"></i>
                                                                        </button>
                                                                        <ul class="dropdown-menu">


                                                                            <li <%= hdnCanEdit.Value=="true"?"style='display:block'":"style='display:none'" %>>
                                                                                <asp:LinkButton ID="btnEdit" runat="server" CommandArgument='<%# Eval("Trans_Id") %>'
                                                                                    OnCommand="btnEdit_Command" CausesValidation="False"><i class="fa fa-pencil"></i><%# Resources.Attendance.Edit%> </asp:LinkButton>
                                                                            </li>
                                                                            <li <%= hdnCanDelete.Value=="true"?"style='display:block'":"style='display:none'" %>>
                                                                                <asp:LinkButton ID="IbtnDelete" runat="server" CausesValidation="False" CommandArgument='<%# Eval("Trans_Id") %>'
                                                                                    OnCommand="IbtnDelete_Command"><i class="fa fa-trash"></i><%# Resources.Attendance.Delete%></asp:LinkButton>
                                                                                <cc1:ConfirmButtonExtender ID="confirm1" runat="server" ConfirmText="<%$ Resources:Attendance,Are you sure you want to delete the record?%>" TargetControlID="IbtnDelete"></cc1:ConfirmButtonExtender>
                                                                            </li>


                                                                        </ul>
                                                                    </div>

                                                                </ItemTemplate>
                                                            </asp:TemplateField>

                                                            <asp:BoundField DataField="AddressCategoryName" HeaderText="<%$ Resources:Attendance,Address Category %>"
                                                                SortExpression="AddressCategoryName" ItemStyle-Width="20%" />
                                                            <asp:BoundField DataField="Address_Name" HeaderText="<%$ Resources:Attendance,Address Name %>"
                                                                SortExpression="Address_Name" ItemStyle-Width="10%" />
                                                            <asp:BoundField DataField="EmailId1" HeaderText="<%$ Resources:Attendance,Email Id%>"
                                                                SortExpression="EmailId1" ItemStyle-Width="10%" />
                                                            <asp:BoundField DataField="Website" HeaderText="<%$ Resources:Attendance,Website%>"
                                                                SortExpression="Website" ItemStyle-Width="10%" />
                                                            <asp:BoundField DataField="FaxNo" HeaderText="<%$ Resources:Attendance,Fax No %>"
                                                                SortExpression="FaxNo" ItemStyle-Width="10%" />
                                                            <asp:BoundField DataField="City_Name" HeaderText="<%$ Resources:Attendance,City %>"
                                                                SortExpression="City_Name" ItemStyle-Width="10%" />
                                                            <asp:BoundField DataField="State_Name" HeaderText="<%$ Resources:Attendance,State %>"
                                                                SortExpression="State_Name" ItemStyle-Width="10%" />
                                                            <asp:BoundField DataField="CountryName" HeaderText="<%$ Resources:Attendance,Country %>"
                                                                SortExpression="CountryName" ItemStyle-Width="10%" />
                                                            <asp:BoundField DataField="Created_User" HeaderText="<%$ Resources:Attendance,Created By %>"
                                                                SortExpression="Created_User" ItemStyle-Width="10%" />
                                                            <asp:BoundField DataField="Modified_User" HeaderText="<%$ Resources:Attendance,Modified By %>"
                                                                SortExpression="Modified_User" ItemStyle-Width="10%" />
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
                                                    <div class="col-md-4">
                                                        <asp:Label ID="lblAddressCategory" runat="server" Text="<%$ Resources:Attendance,Address Category %>"></asp:Label>
                                                        <a style="color: Red">*</a>
                                                        <asp:RequiredFieldValidator EnableClientScript="true" Style="float: right;" Display="Dynamic" ID="RequiredFieldValidator7" runat="server" ControlToValidate="ddlAddressCategory"
                                                            InitialValue="--Select--" SetFocusOnError="true" ValidationGroup="Save" ErrorMessage="<%$ Resources:Attendance,Select Address Category%>"></asp:RequiredFieldValidator>

                                                        <asp:DropDownList ID="ddlAddressCategory" runat="server" CssClass="form-control">
                                                        </asp:DropDownList>
                                                        <br />
                                                    </div>

                                                    <div class="col-md-8">
                                                        <asp:Label ID="lblAddressName" runat="server" Text="<%$ Resources:Attendance,Address Name %>"></asp:Label>
                                                        <a style="color: Red">*</a>
                                                        <asp:RequiredFieldValidator EnableClientScript="true" Style="float: right;" Display="Dynamic" runat="server" ID="RequiredFieldValidator6" ValidationGroup="Save" SetFocusOnError="true" ControlToValidate="txtAddressName" ErrorMessage="<%$ Resources:Attendance,Enter Address Name%>" />

                                                        <asp:TextBox ID="txtAddressName" runat="server" BackColor="#eeeeee" onchange="txtAddressName_TextChanged()" CssClass="form-control"></asp:TextBox>
                                                        <cc1:AutoCompleteExtender ID="AutoCompleteExtender1" runat="server" DelimiterCharacters=""
                                                            Enabled="True" ServiceMethod="GetCompletionList" ServicePath="" CompletionInterval="100"
                                                            MinimumPrefixLength="1" CompletionSetCount="1" TargetControlID="txtAddressName"
                                                            UseContextKey="True" CompletionListCssClass="autoCompleteList" CompletionListItemCssClass="autoCompleteListItem" CompletionListHighlightedItemCssClass="autoCompleteSelectedListItem" OnClientShown="resetPosition">
                                                        </cc1:AutoCompleteExtender>
                                                        <br />
                                                    </div>

                                                    <div class="col-md-12">
                                                        <asp:Label ID="lblAddress" runat="server" Text="<%$ Resources:Attendance,Address %>"></asp:Label>
                                                        <a style="color: Red">*</a>
                                                        <asp:RequiredFieldValidator EnableClientScript="true" Style="float: right;" Display="Dynamic" runat="server" ID="RequiredFieldValidator1" ValidationGroup="Save" SetFocusOnError="true" ControlToValidate="txtAddress" ErrorMessage="<%$ Resources:Attendance,Enter Address%>" />

                                                        <asp:TextBox ID="txtAddress" Style="width: 100%; resize: vertical; max-height: 200px; min-height: 50px; height: 50px;" TextMode="MultiLine" runat="server" CssClass="form-control">
                                                        </asp:TextBox>
                                                        <br />
                                                    </div>

                                                    <div class="col-md-3">
                                                        <asp:Label ID="Label17" runat="server" Text="<%$ Resources:Attendance,Country %>"></asp:Label>
                                                        <asp:DropDownList ID="ddlCountry" onchange="DDLCountry_Change()" runat="server" CssClass="form-control"></asp:DropDownList>
                                                        <br />
                                                    </div>
                                                    <asp:HiddenField ID="hdnstateId" runat="server" />
                                                    <asp:HiddenField ID="hdncityId" runat="server" />
                                                    <div class="col-md-3">
                                                        <asp:Label ID="lblState" runat="server" Text="<%$ Resources:Attendance,State %>"></asp:Label>
                                                        <asp:TextBox ID="txtState" runat="server" CssClass="form-control" onchange="txtState_TextChanged()"></asp:TextBox>
                                                        <cc1:AutoCompleteExtender ID="AutoCompleteExtender3" runat="server" DelimiterCharacters=""
                                                            Enabled="True" ServiceMethod="GetCompletionListStateName" ServicePath="" CompletionInterval="100"
                                                            MinimumPrefixLength="1" CompletionSetCount="1" TargetControlID="txtState"
                                                            UseContextKey="True" CompletionListCssClass="autoCompleteList" CompletionListItemCssClass="autoCompleteListItem" CompletionListHighlightedItemCssClass="autoCompleteSelectedListItem" OnClientShown="resetPosition">
                                                        </cc1:AutoCompleteExtender>
                                                        <br />
                                                    </div>

                                                    <div class="col-md-3">
                                                        <asp:Label ID="lblCity" runat="server" Text="<%$ Resources:Attendance,City %>"></asp:Label>
                                                        <asp:TextBox ID="txtCity" runat="server" CssClass="form-control" onchange="txtCity_TextChanged()"></asp:TextBox>
                                                        <cc1:AutoCompleteExtender ID="AutoCompleteExtender4" runat="server" DelimiterCharacters=""
                                                            Enabled="True" ServiceMethod="GetCompletionListCityName" ServicePath="" CompletionInterval="100"
                                                            MinimumPrefixLength="1" CompletionSetCount="1" TargetControlID="txtCity"
                                                            UseContextKey="True" CompletionListCssClass="autoCompleteList" CompletionListItemCssClass="autoCompleteListItem" CompletionListHighlightedItemCssClass="autoCompleteSelectedListItem" OnClientShown="resetPosition">
                                                        </cc1:AutoCompleteExtender>
                                                        <br />
                                                    </div>

                                                    <div class="col-md-3">
                                                        <asp:Label ID="lblPinCode" runat="server" Text="<%$ Resources:Attendance,PinCode %>"></asp:Label>
                                                        <asp:TextBox ID="txtPinCode" runat="server" CssClass="form-control"></asp:TextBox>
                                                        <br />
                                                    </div>

                                                    <div class="col-md-4">
                                                        <asp:Label ID="lblPhoneNo1" runat="server" Text="<%$ Resources:Attendance,Phone No 1 %>"></asp:Label>
                                                        <div style="width: 100%;" class="input-group">
                                                            <asp:TextBox ID="txtCountryCode_Phoneno1" Style="width: 30%;" runat="server" CssClass="form-control">
                                                            </asp:TextBox>
                                                            <asp:TextBox ID="txtPhoneNo1" Width="70%" onkeypress="return isNumber(event)" runat="server" CssClass="form-control"></asp:TextBox>
                                                        </div>
                                                        <br />
                                                    </div>

                                                    <div class="col-md-4">
                                                        <asp:Label ID="lblMobileNo1" runat="server" Text="<%$ Resources:Attendance,Mobile No 1 %>"></asp:Label>
                                                        <div style="width: 100%;" class="input-group">
                                                            <asp:TextBox ID="txtCountryCode_MobileNo1" Style="width: 30%;" runat="server" CssClass="form-control">
                                                            </asp:TextBox>
                                                            <asp:TextBox ID="txtMobileNo1" Width="70%" onkeypress="return isNumber(event)" runat="server" CssClass="form-control"></asp:TextBox>
                                                        </div>
                                                        <br />
                                                    </div>

                                                    <div class="col-md-4">
                                                        <asp:Label ID="lblEmailId1" runat="server" Text="<%$ Resources:Attendance,Email Id 1 %>"></asp:Label>
                                                        <asp:TextBox ID="txtEmailId1" runat="server" CssClass="form-control"></asp:TextBox>
                                                        <br />
                                                    </div>

                                                    <div class="row">
                                                        <div class="col-md-12">
                                                            <div id="Div_Additional_Info" class="box box-primary collapsed-box">
                                                                <div class="box-header with-border">
                                                                    <h3 class="box-title"> <asp:Label ID="lbldelete" runat="server" Text = "<%$ Resources:Attendance,Additional Information %>"></asp:Label>            </h3>
                                                                    <div class="box-tools pull-right">
                                                                        <button type="button" class="btn btn-box-tool" data-widget="collapse">
                                                                            <i id="Btn_Div_Additional_Info" class="fa fa-plus"></i>
                                                                        </button>
                                                                    </div>
                                                                </div>
                                                                <div class="box-body">
                                                                    <div class="form-group">

                                                                        <div class="col-md-4">
                                                                            <asp:Label ID="lblPhoneNo2" runat="server" Text="<%$ Resources:Attendance,Phone No 2 %>"></asp:Label>
                                                                            <div style="width: 100%;" class="input-group">
                                                                                <asp:TextBox ID="txtCountryCode_Phoneno2" Style="width: 30%;" runat="server" CssClass="form-control">
                                                                                </asp:TextBox>
                                                                                <asp:TextBox ID="txtPhoneNo2" Width="70%" onkeypress="return isNumber(event)" runat="server" CssClass="form-control"></asp:TextBox>
                                                                            </div>
                                                                            <br />
                                                                        </div>
                                                                        <div class="col-md-4">
                                                                            <asp:Label ID="lblMobileNo2" runat="server" Text="<%$ Resources:Attendance,Mobile No 2 %>"></asp:Label>
                                                                            <div style="width: 100%;" class="input-group">
                                                                                <asp:TextBox ID="txtCountryCode_MobileNo2" Style="width: 30%;" runat="server" CssClass="form-control">
                                                                                </asp:TextBox>
                                                                                <asp:TextBox ID="txtMobileNo2" Width="70%" onkeypress="return isNumber(event)" runat="server" CssClass="form-control"></asp:TextBox>
                                                                            </div>
                                                                            <br />
                                                                        </div>
                                                                        <div class="col-md-4">
                                                                            <asp:Label ID="lblEmailId2" runat="server" Text="<%$ Resources:Attendance,Email Id 2 %>"></asp:Label>
                                                                            <asp:TextBox ID="txtEmailId2" runat="server" CssClass="form-control"></asp:TextBox>
                                                                            <br />
                                                                        </div>
                                                                        <div class="col-md-4">
                                                                            <asp:Label ID="lblFaxNo" runat="server" Text="<%$ Resources:Attendance,Fax No %>"></asp:Label>
                                                                            <asp:TextBox ID="txtFaxNo" runat="server" CssClass="form-control"></asp:TextBox>
                                                                            <br />
                                                                        </div>

                                                                        <div class="col-md-4">
                                                                            <asp:Label ID="lblStreet" runat="server" Text="<%$ Resources:Attendance,Street No.%>"></asp:Label>
                                                                            <asp:TextBox ID="txtStreet" runat="server" MaxLength="5" CssClass="form-control"></asp:TextBox>
                                                                            <br />
                                                                        </div>

                                                                        <div class="col-md-4">
                                                                            <asp:Label ID="lblBlock" runat="server" Text="<%$ Resources:Attendance,Block %>"></asp:Label>
                                                                            <asp:TextBox ID="txtBlock" runat="server" CssClass="form-control"></asp:TextBox>
                                                                            <br />
                                                                        </div>

                                                                        <div class="col-md-4">
                                                                            <asp:Label ID="lblAvenue" runat="server" Text="<%$ Resources:Attendance,Avenue %>"></asp:Label>
                                                                            <asp:TextBox ID="txtAvenue" runat="server" CssClass="form-control"></asp:TextBox>
                                                                            <br />
                                                                        </div>

                                                                        <div class="col-md-4">
                                                                            <asp:Label ID="Label16" runat="server" Text="<%$ Resources:Attendance,Area Name  %>"></asp:Label>
                                                                            <asp:TextBox ID="txtAreaName" runat="server" CssClass="form-control"></asp:TextBox>
                                                                            <cc1:AutoCompleteExtender ID="AutoCompleteExtender2" runat="server" DelimiterCharacters=""
                                                                                Enabled="True" ServiceMethod="GetCompletionListAreaName" ServicePath="" CompletionInterval="100"
                                                                                MinimumPrefixLength="1" CompletionSetCount="1" TargetControlID="txtAreaName"
                                                                                UseContextKey="True" CompletionListCssClass="autoCompleteList" CompletionListItemCssClass="autoCompleteListItem" CompletionListHighlightedItemCssClass="autoCompleteSelectedListItem" OnClientShown="resetPosition">
                                                                            </cc1:AutoCompleteExtender>
                                                                            <asp:HiddenField ID="hdnParentid" runat="server" Value="0" />
                                                                            <br />
                                                                        </div>

                                                                        <div class="col-md-4">
                                                                            <asp:Label ID="lblWebsite" runat="server" Text="<%$ Resources:Attendance,Website %>"></asp:Label>
                                                                            <asp:TextBox ID="txtWebsite" runat="server" CssClass="form-control"></asp:TextBox>
                                                                            <br />
                                                                        </div>
                                                                        <div class="col-md-12"></div>
                                                                        <div class="col-md-6">
                                                                            <asp:Label ID="lblLongitude" runat="server" Text="<%$ Resources:Attendance,Longitude %>"></asp:Label>
                                                                            <asp:TextBox ID="txtLongitude" runat="server" onkeypress="return NumberFloatAndOneDOTSign(this)" Enabled="false" CssClass="form-control"></asp:TextBox>
                                                                            <br />
                                                                        </div>

                                                                        <div class="col-md-6">
                                                                            <asp:Label ID="lblLatitude" runat="server" Text="<%$ Resources:Attendance,Latitude %>"></asp:Label>
                                                                            <asp:RequiredFieldValidator EnableClientScript="true" Style="float: right;" Display="Dynamic" runat="server" ID="RequiredFieldValidator2" ValidationGroup="L_Save" SetFocusOnError="true" ControlToValidate="txtLatitude" ErrorMessage="<%$ Resources:Attendance,Enter Latitude%>" />
                                                                            <div class="input-group">
                                                                                <asp:TextBox ID="txtLatitude" runat="server" CssClass="form-control" onkeypress="return NumberFloatAndOneDOTSign(this)" Enabled="false"></asp:TextBox>
                                                                                <%--<div class="input-group-btn">
                                                                                    <asp:Button ID="BtnUpdateLatLong" OnClick="BtnUpdateLatLong_Click" ValidationGroup="L_Save" Style="margin-left: 10px;" CssClass="btn btn-info" runat="server" Text="<%$ Resources:Attendance,Set Value %>" />
                                                                                </div>--%>
                                                                                <div class="input-group-btn">
                                                                               <%--     <asp:Button ID="btnGetLatLong" OnClick="btnGetLatLong_Click" Style="margin-left: 10px;" CssClass="btn btn-info" runat="server" Text="<%$ Resources:Attendance,Map %>" />
                                                                               --%> 
                                                                                     <button id="openModal" onclick="openModel();return false;" style="margin-left: 10px;" class="btn btn-info" >Map</button>
                                                                                </div>
                                                                            </div>
                                                                            <br />
                                                                        </div>                                                                       
                                                                    </div>
                                                                </div>
                                                            </div>
                                                        </div>
                                                    </div>




                                                    <div class="col-md-12" style="text-align: center;">
                                                        <asp:Button ID="btnAddressSave" CssClass="btn btn-success" ValidationGroup="Save" OnClick="btnAddressSave_Click" runat="server" Text="<%$ Resources:Attendance,Save %>" />
                                                        <asp:Button ID="btnAddressReset" Style="margin-left: 15px;" CausesValidation="False" OnClick="BtnReset_Click" CssClass="btn btn-primary" runat="server"
                                                            Text="<%$ Resources:Attendance,Reset %>" />
                                                        <asp:Button ID="btnAddressCancel" Style="margin-left: 15px;" CausesValidation="False" OnClick="btnAddressCancel_Click" CssClass="btn btn-danger" runat="server"
                                                            Text="<%$ Resources:Attendance,Cancel %>" />
                                                    </div>
                                                    <asp:HiddenField ID="editid" runat="server" />

                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </div>
                    <!-- The Modal -->
                    <div class="modal" id="myModal">
                        <div class="modal-dialog">
                            <div class="modal-content">

                                <!-- Modal Header -->
                                <div class="modal-header">
                                    <h4 class="modal-title">Take Address</h4>
                                    <button type="button" class="close" data-dismiss="modal">&times;</button>
                                </div>
                                <!-- Modal body -->
                                <div class="modal-body">
                                    <div id="map" style="width: 570px; height: 400px;"></div>
                                    <p>Latitude: <span id="latitude"></span></p>
                                    <p>Longitude: <span id="longitude"></span></p>
                                </div>
                                <!-- Modal footer -->
                                <div class="modal-footer">
                                    <button type="button" class="btn btn-danger" data-dismiss="modal">Close</button>
                                </div>

                            </div>
                        </div>
                    </div>
                    
                    <!-- /.tab-pane -->
                    <div class="tab-pane" id="Bin">
                        <asp:UpdatePanel ID="Update_Bin" runat="server">

                            <ContentTemplate>


                                <div class="row">
                                    <div class="col-md-12">
                                        <div id="Div2" runat="server" class="box box-info collapsed-box">
                                            <div class="box-header with-border">
                                                <h3 class="box-title">
                                                    <asp:Label ID="Label3" runat="server" Text="<%$ Resources:Attendance,Advance Search%>"></asp:Label></h3>
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
                                                        <asp:ListItem Text="<%$ Resources:Attendance, Address Id %>" Value="Trans_Id"></asp:ListItem>
                                                        <asp:ListItem Text="<%$ Resources:Attendance, Address Category %>" Value="AddressCategoryName"></asp:ListItem>
                                                        <asp:ListItem Text="<%$ Resources:Attendance, Address Name %>" Value="Address_Name"
                                                            Selected="True"></asp:ListItem>
                                                        <asp:ListItem Text="<%$ Resources:Attendance, Created By %>" Value="Created_User"></asp:ListItem>
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
                                                    <asp:LinkButton ID="btnbindBin"  runat="server" CausesValidation="False"  OnClick="btnbindBin_Click" ToolTip="<%$ Resources:Attendance,Search %>"><span class="fa fa-search"  style="font-size:25px;"></span></asp:LinkButton>&nbsp;&nbsp;&nbsp;

                                                    <asp:LinkButton ID="btnRefreshBin"  runat="server" CausesValidation="False" OnClick="btnRefreshBin_Click" ToolTip="<%$ Resources:Attendance,Refresh %>"><span class="fa fa-repeat"  style="font-size:25px;"></span></asp:LinkButton>&nbsp;&nbsp;&nbsp;

                                                    <asp:LinkButton ID="imgBtnRestore"  CausesValidation="False" Visible="false" runat="server"  OnClick="imgBtnRestore_Click" ToolTip="<%$ Resources:Attendance, Active %>" ><span class="far fa-lightbulb"  style="font-size:25px;"></span></asp:LinkButton>&nbsp;&nbsp;&nbsp;

                                                    <asp:ImageButton ID="ImgbtnSelectAll" Visible="false" Style="width: 33px;" runat="server" OnClick="ImgbtnSelectAll_Click" ToolTip="<%$ Resources:Attendance, Select All %>" AutoPostBack="true" ImageUrl="~/Images/selectAll.png" />
                                                </div>

                                            </div>
                                        </div>
                                    </div>
                                </div>


                                <div class="box box-warning box-solid">
                                   
                                    <div class="box-body">
                                        <div class="row">
                                            <div class="col-md-12">
                                                <div style="overflow-y: auto;">
                                                    <asp:Label ID="Label2" runat="server" Visible="false"></asp:Label>
                                                    <asp:GridView CssClass="table-striped table-bordered table table-hover" ID="GvAddressBin" PageSize='<%# int.Parse(Session["GridSize"].ToString()) %>' runat="server"
                                                        AutoGenerateColumns="False" Width="100%" AllowPaging="True" OnPageIndexChanging="GvAddressBin_PageIndexChanging"
                                                        OnSorting="GvAddressBin_OnSorting" DataKeyNames="Trans_Id" AllowSorting="true">
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
                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,Address Id%>" SortExpression="Trans_Id">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblgvAddressId" runat="server" Text='<%# Eval("Trans_Id") %>' />
                                                                </ItemTemplate>
                                                                <ItemStyle />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,Address Category %>" SortExpression="AddressCategoryName">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblgvAddressCatName" runat="server" Text='<%# Eval("AddressCategoryName") %>' />
                                                                </ItemTemplate>
                                                                <ItemStyle />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,Address Name %>" SortExpression="Address_Name">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblgvAddressName" runat="server" Text='<%# Eval("Address_Name") %>' />
                                                                </ItemTemplate>
                                                                <ItemStyle />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,Created By %>" SortExpression="Created_User">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblgvCreated_User" runat="server" Text='<%# Eval("Created_User") %>' />
                                                                </ItemTemplate>
                                                                <ItemStyle />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,Modified By %>" SortExpression="Modified_User">
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
                <!-- /.tab-content -->
            </div>

        </div>
    </div>

    <asp:UpdateProgress ID="UpdateProgress2" runat="server" AssociatedUpdatePanelID="Update_List">
        <ProgressTemplate>
            <div class="modal_Progress">
                <div class="center_Progress">
                    <img alt="" src="../Images/ajax-loader2.gif" style="vertical-align: middle; background-color: White; border-radius: 10px;" />
                </div>
            </div>
        </ProgressTemplate>
    </asp:UpdateProgress>

    <asp:UpdateProgress ID="UpdateProgress1" runat="server" AssociatedUpdatePanelID="Update_New">
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
    <link href="../CSS/InvStyle.css" rel="stylesheet" type="text/css" />
    <script>
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
        function LI_New_Active() {
            $("#Li_List").removeClass("active");
            $("#List").removeClass("active");

            $("#Li_New").addClass("active");
            $("#New").addClass("active");
        }

    </script>
     <script src="https://maps.googleapis.com/maps/api/js?key=AIzaSyD2XYFPF6bDNKsyTaLL03Pzds1KCpVZ-NE"></script>
    <script type="text/javascript">
        var defaultLocation = { lat: 24.59006541239175, lng: 73.70691619585239 }; // New York City

        // Function to open the modal
        function openModel() {
            $('#myModal').modal('show');           
            initMap();
        }       
        // Initialize Google Maps and get coordinates
        function initMap() {
            var map = new google.maps.Map(document.getElementById('map'), {
                center: defaultLocation, // Set the default location here
                zoom: 8 // Adjust the zoom level as needed
            });

            // Add a click event listener to get coordinates on map click
            google.maps.event.addListener(map, 'click', function (event) {
                var latitude = event.latLng.lat();
                var longitude = event.latLng.lng();
                document.getElementById('latitude').innerText = latitude;
                document.getElementById('longitude').innerText = longitude;
                $('#<%=txtLongitude.ClientID%>').val(longitude);
                $('#<%=txtLatitude.ClientID%>').val(latitude);
            });
        }       
    </script>
    <script>
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

        function LI_List_Active() {
            $("#Li_List").addClass("active");
            $("#List").addClass("active");

            $("#Li_New").removeClass("active");
            $("#New").removeClass("active");
        }


        function Div_Additional_Info_Open() {
            $("#Btn_Div_Additional_Info").removeClass("fa fa-plus");
            $("#Div_Additional_Info").removeClass("box box-primary collapsed-box");

            $("#Btn_Div_Additional_Info").addClass("fa fa-minus");
            $("#Div_Additional_Info").addClass("box box-primary");
        }


        function DDLCountry_Change() {
            var ddlcountry = document.getElementById('<%= ddlCountry.ClientID %>');
            if (ddlcountry.value != "--Select--") {
                PageMethods.ddlCountry_IndexChanged(ddlcountry.value, onSuccessChange, ErrorOnChange);
            }
        }
        function onSuccessChange(data) {
            document.getElementById('<%= txtState.ClientID %>').value = "";
            document.getElementById('<%= txtState.ClientID %>').focus();
            document.getElementById('<%= txtCity.ClientID %>').value = "";

            document.getElementById('<%= txtCountryCode_Phoneno1.ClientID %>').value = data;
            document.getElementById('<%= txtCountryCode_Phoneno2.ClientID %>').value = data;
            document.getElementById('<%= txtCountryCode_MobileNo1.ClientID %>').value = data;
            document.getElementById('<%= txtCountryCode_MobileNo2.ClientID %>').value = data;


        }
        function ErrorOnChange(error) {
            alert(error.getMessage());
        }

        function txtCity_TextChanged() {
            var stateId = document.getElementById('<%= hdnstateId.ClientID %>').value;
            var CityName = document.getElementById('<%= txtCity.ClientID %>').value;

            if (document.getElementById('<%= txtState.ClientID %>').value == "") {
                alert("Please Select State");
                document.getElementById('<%= txtState.ClientID %>').focus();
                document.getElementById('<%= txtCity.ClientID %>').value = "";
                return;
            }
            if (stateId == "0" || stateId == "") {
                alert("Please Select State");
                document.getElementById('<%= txtState.ClientID %>').focus();
                document.getElementById('<%= txtCity.ClientID %>').value = "";
                return;
            }
            PageMethods.txtCity_TextChanged(stateId, CityName, onSuccess_CityChange, ErrorOnChange);
        }
        function onSuccess_CityChange(CityId) {
            if (CityId == "" || CityId == "0") {
                alert('Select From Suggestions Only');
                document.getElementById('<%= txtCity.ClientID %>').focus();
                document.getElementById('<%= txtCity.ClientID %>').value = "";
                return;
            }
            document.getElementById('<%= hdncityId.ClientID %>').value = CityId;
        }

        function txtState_TextChanged() {
            var stateName = document.getElementById('<%= txtState.ClientID %>').value;
            var CountryId = document.getElementById('<%= ddlCountry.ClientID %>').value;
            if (CountryId != "--Select--") {
                PageMethods.txtState_TextChanged(CountryId, stateName, onSuccess_StateChange, ErrorOnChange);
            }
            else {
                alert('Please Select Country');
                document.getElementById('<%= ddlCountry.ClientID %>').focus();
                document.getElementById('<%= txtState.ClientID %>').value = "";
                return;
            }

        }

        function onSuccess_StateChange(StateId) {
            if (StateId == "" || StateId == "0") {
                alert('Select From Suggestions Only');
                document.getElementById('<%= txtState.ClientID %>').value = "";
                document.getElementById('<%= txtState.ClientID %>').focus();
                document.getElementById('<%= hdnstateId.ClientID %>').value = "";
            }
            document.getElementById('<%= txtCity.ClientID %>').value = "";
            document.getElementById('<%= hdnstateId.ClientID %>').value = StateId;
        }
        function txtAddressName_TextChanged() {
            var addressName = document.getElementById('<%= txtAddressName.ClientID %>');
            var addressid = document.getElementById('<%= editid.ClientID %>');

            if (addressid == "@NOTFOUND@") {
                addressid = "";
            }

            if (addressName.value != "") {
                PageMethods.txtAddressNameNew_TextChanged(addressName.value, addressid.value, OnSuccess_txtAddressNameNew, ErrorOnChange);
            }
        }

        function OnSuccess_txtAddressNameNew(data) {
            if (data == 1) {
                alert("Address Name Already Exists");
                document.getElementById('<%= txtAddressName.ClientID %>').focus();
            document.getElementById('<%= txtAddressName.ClientID %>').value = "";
        }
    }
    </script>
</asp:Content>




