<%@ Page Title="" Language="C#" MasterPageFile="~/ERPMaster.master" AutoEventWireup="true" CodeFile="ContactMaster.aspx.cs" Inherits="EMS_ContactMaster" EnableEventValidation="false" %>
<%@ Register Assembly="DevExpress.Web.v18.1, Version=18.1.3.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web" TagPrefix="dx" %>
<%@ Register Src="~/WebUserControl/AddressControl.ascx" TagName="Addressmaster" TagPrefix="UC" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit.HTMLEditor"
    TagPrefix="cc1" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Src="~/WebUserControl/ContactNo.ascx" TagPrefix="uc1" TagName="ContactNo1" %>
<%@ Register Src="~/WebUserControl/FileUpload.ascx" TagPrefix="AT1" TagName="FileUpload1" %>
<%@ Register Src="~/WebUserControl/ucControlsSetting.ascx" TagName="ucCtlSetting" TagPrefix="UC" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Head" runat="Server">
    <script type="text/javascript" src="http://maps.google.com/maps/api/js?sensor=false"></script>
    <script type="text/javascript">

        function ShowDatatables() {
            $("#ctl00_MainContent_gvContact").dataTable({
                "aLengthMenu": [[10, 20, 30, 40, 50, 60, 70, 80, 90, 100], [10, 20, 30, 40, 50, 60, 70, 80, 90, 100]],
                "iDisplayLength": 10
            });
            $("#ctl00_MainContent_gvContact_info").hide();
            $("#ctl00_MainContent_gvContact_paginate").hide();
            $("#ctl00_MainContent_gvContact_length").hide();
            SetPageSizeInDt();
        }

        function SetPageSizeInDt() {
            var myval = $("#ctl00_MainContent_ddlPageSize option:selected").val();
            $('#ctl00_MainContent_gvContact_length').find("select").val(myval).change();
        }

        function SetDropdownValue() {
            var IsAdd = false;;
            var IsRemove = false;
            $('#ctl00_MainContent_gvContact tr input[type="checkbox"]').each(function (i, chk) {
                if (chk.checked) {
                    var IsFav = $(this).parents('tr').find('input[type="hidden"]').val();
                    if (IsFav == "True") {
                        IsAdd = true;
                    }
                    else {
                        IsRemove = true;
                    }
                }
            });

            var SelectOption = "<option value='" + "Select" + "'>---Select---</option>";
            var AddOption = "<option value='" + "Add" + "'>Add as Favorite</option>";
            var RemoveOption = "<option value='" + "Remove" + "'>Remove from Favorite</option>";
            var FilterOption = "<option value='" + "Favorite" + "'>Favorite Only</option>";

            $("#<%= ddlMyFav.ClientID %>").empty();

            if ((IsAdd == true && IsRemove == true) || (IsAdd == false && IsRemove == false)) {
                //alert('Add & Remove');
                $("#<%= ddlMyFav.ClientID %>").append(SelectOption);
                $("#<%= ddlMyFav.ClientID %>").append(AddOption);
                $("#<%= ddlMyFav.ClientID %>").append(RemoveOption);
                $("#<%= ddlMyFav.ClientID %>").append(FilterOption);
            }
            else if (IsAdd == true && IsRemove == false) {
                //alert('Add');
                $("#<%= ddlMyFav.ClientID %>").append(SelectOption);
                $("#<%= ddlMyFav.ClientID %>").append(RemoveOption);
                $("#<%= ddlMyFav.ClientID %>").append(FilterOption);
            }
            else if (IsAdd == false && IsRemove == true) {
                //alert('Remove');
                $("#<%= ddlMyFav.ClientID %>").append(SelectOption);
                $("#<%= ddlMyFav.ClientID %>").append(AddOption);
                $("#<%= ddlMyFav.ClientID %>").append(FilterOption);
            }

}

    </script>
    <style type="text/css">
        .pager {
            background-color: #3AC0F2;
            color: white;
            height: 30px;
            min-width: 30px;
            line-height: 30px;
            display: block;
            text-align: center;
            text-decoration: none;
            border: 1px solid #2E99C1;
        }

        .active_pager {
            background-color: #2E99C1;
            color: white;
            height: 30px;
            min-width: 30px;
            line-height: 30px;
            display: block;
            text-align: center;
            text-decoration: none;
            border: 1px solid #2E99C1;
        }

        .grid td, .grid th {
            text-align: center;
        }
    </style>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="HeaderContent" runat="Server">

    <h1>
        <i class="fas fa-id-card"></i>&nbsp;&nbsp;
        <asp:Label ID="lblHeader" runat="server" Text="<%$ Resources:Attendance,Contact Setup%>"></asp:Label>
    </h1>
    <ol class="breadcrumb">
        <li><a href="#"><i class="fa fa-dashboard"></i>
            <asp:Label ID="Label6" runat="server" Text="<%$ Resources:Attendance,Master Setup%>"></asp:Label></a></li>
        <li><a href="#"><i class="fa fa-dashboard"></i>
            <asp:Label ID="Label10" runat="server" Text="<%$ Resources:Attendance,Master SetUp%>"></asp:Label></a></li>
        <li class="active">
            <asp:Label ID="Label5" runat="server" Text="<%$ Resources:Attendance,Contact Setup%>"></asp:Label></li>
    </ol>


</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="Server">
    <script type="text/javascript" src="~/js/jquery-1.4.1.min.js"></script>
    <script language="javascript" type="text/javascript">
        function new_validation() {
            var val = document.getElementById("<%= chkSelectAll.ClientID  %>").checked;
            if (val) {
                $('[id*=navTree] input[type=checkbox]').prop('checked', true);
            }
            else {
                $('[id*=navTree] input[type=checkbox]').prop('checked', false);
            }
        }

        function OnTreeClick(evt) {
            var src = window.event != window.undefined ? window.event.srcElement : evt.target;
            var isChkBoxClick = (src.tagName.toLowerCase() == "input" && src.type == "checkbox");
            if (isChkBoxClick) {
                var parentTable = GetParentByTagName("table", src);
                var nxtSibling = parentTable.nextSibling;
                if (nxtSibling && nxtSibling.nodeType == 1)//check if nxt sibling is not null & is an element node
                {
                    if (nxtSibling.tagName.toLowerCase() == "div") //if node has children
                    {
                        //check or uncheck children at all levels
                        CheckUncheckChildren(parentTable.nextSibling, src.checked);
                    }
                }
                //check or uncheck parents at all levels             
                CheckUncheckParents(src, src.checked);
            }
        }
        function CheckUncheckChildren(childContainer, check) {
            var childChkBoxes = childContainer.getElementsByTagName("input");
            var childChkBoxCount = childChkBoxes.length;
            for (var i = 0; i < childChkBoxCount; i++) {
                childChkBoxes[i].checked = check;
            }
        }

        function CheckUncheckParents(srcChild, check) {
            //if (!check) {
            //    return;
            //}
            var parentDiv = GetParentByTagName("div", srcChild);
            var parentNodeTable = parentDiv.previousSibling;
            if (parentNodeTable) {
                var checkUncheckSwitch;

                if (check) //checkbox checked
                {
                    var isAllSiblingsChecked = AreAllSiblingsChecked(srcChild);
                    if (isAllSiblingsChecked)
                        checkUncheckSwitch = true;
                    else
                        checkUncheckSwitch = true;
                    //return; //do not need to check parent if any(one or more) child not checked
                }
                else //checkbox unchecked
                {
                    if (AreAllSiblingsunChecked(srcChild))
                        checkUncheckSwitch = false;
                    else
                        checkUncheckSwitch = true;
                }

                var inpElemsInParentTable = parentNodeTable.getElementsByTagName("input");
                if (inpElemsInParentTable.length > 0) {
                    var parentNodeChkBox = inpElemsInParentTable[0];
                    parentNodeChkBox.checked = checkUncheckSwitch;
                    //do the same recursively
                    CheckUncheckParents(parentNodeChkBox, checkUncheckSwitch);
                }
            }
        }

        function AreAllSiblingsChecked(chkBox) {
            var parentDiv = GetParentByTagName("div", chkBox);
            var childCount = parentDiv.childNodes.length;
            for (var i = 0; i < childCount; i++) {
                if (parentDiv.childNodes[i].nodeType == 1) //check if the child node is an element node
                {
                    if (parentDiv.childNodes[i].tagName.toLowerCase() == "table") {
                        var prevChkBox = parentDiv.childNodes[i].getElementsByTagName("input")[0];
                        //if any of sibling nodes are not checked, return false
                        if (!prevChkBox.checked) {
                            return false;
                        }
                    }
                }
            }
            return true;
        }


        function AreAllSiblingsunChecked(chkBox) {
            var parentDiv = GetParentByTagName("div", chkBox);
            var childCount = parentDiv.childNodes.length;
            for (var i = 0; i < childCount; i++) {
                if (parentDiv.childNodes[i].nodeType == 1) //check if the child node is an element node
                {
                    if (parentDiv.childNodes[i].tagName.toLowerCase() == "table") {
                        var prevChkBox = parentDiv.childNodes[i].getElementsByTagName("input")[0];
                        //if any of sibling nodes are not checked, return false
                        if (prevChkBox.checked) {
                            return false;
                        }
                    }
                }
            }
            return true;
        }



        function Modal_Open_FileUpload() {
            document.getElementById('<%= Btn_Modal_FileUpload.ClientID %>').click();
        }


        //utility function to get the container of an element by tagname
        function GetParentByTagName(parentTagName, childElementObj) {
            var parent = childElementObj.parentNode;
            while (parent.tagName.toLowerCase() != parentTagName.toLowerCase()) {
                parent = parent.parentNode;
            }
            return parent;
        }
    </script>
    <asp:UpdatePanel ID="Update_Button" runat="server">
        <ContentTemplate>
            <asp:Button ID="Btn_List" Style="display: none;" runat="server" OnClick="btnList_Click" Text="List" />
            <asp:Button ID="Btn_New" Style="display: none;" runat="server" OnClick="btnNew_Click" Text="New" />
            <asp:Button ID="Btn_Bin" Style="display: none;" runat="server" OnClick="btnBin_Click" Text="Bin" />
            <asp:Button ID="Btn_Upload" Style="display: none;" runat="server" OnClick="btnUploadExcel_Click" Text="Bin" />
            <asp:Button ID="Btn_Verify" Style="display: none;" runat="server" OnClick="btnVerify_Click" Text="Bin" />
            <asp:Button ID="Btn_Add_Bank_Details" Style="display: none;" data-toggle="modal" data-target="#Bank_Modal" runat="server" Text="Bank Details" />
            <asp:Button ID="Btn_Add_Address_Details" Style="display: none;" data-toggle="modal" data-target="#Address_Modal" runat="server" Text="Address Details" />
            <asp:Button ID="Btn_Number" Style="display: none;" data-toggle="modal" data-target="#NumberData" runat="server" Text="Number Data" />
            <asp:Button ID="Btn_Modal_FileUpload" Style="display: none;" runat="server" data-toggle="modal" data-target="#Fileupload123" Text="FileUpload" />
            <asp:HiddenField runat="server" ID="hdnCanEdit" />
            <asp:HiddenField runat="server" ID="hdnCanDelete" />
            <asp:HiddenField runat="server" ID="hdnCanView" />
            <asp:HiddenField runat="server" ID="hdnCanUpload" />

        </ContentTemplate>
    </asp:UpdatePanel>
    <div class="row">
        <div class="col-md-12">
            <div class="nav-tabs-custom">
                <ul class="nav nav-tabs pull-right bg-blue-gradient">
                    <li id="Li_Verify_Request" runat="server" visible="false"><a onclick="Li_Tab_Verify_Request()" href="#Verify_Request" data-toggle="tab">
                        <i class="fas fa-user-check"></i>&nbsp;&nbsp;<asp:Label ID="Label31" runat="server" Text="<%$ Resources:Attendance,Verify Request%>"></asp:Label></a></li>
                    <li id="Li_Upload"><a onclick="Li_Tab_Upload()" href="#Upload" data-toggle="tab">
                        <i class="fa fa-upload"></i>&nbsp;&nbsp;<asp:Label ID="Label32" runat="server" Text="<%$ Resources:Attendance,Upload%>"></asp:Label></a></li>

                    <li id="Li_Bin"><a onclick="Li_Tab_Bin()" href="#Bin" data-toggle="tab">
                        <i class="fa fa-trash"></i>&nbsp;&nbsp;<asp:Label ID="Label29" runat="server" Text="<%$ Resources:Attendance,Bin%>"></asp:Label></a></li>
                    <li id="Li_New"><a onclick="Li_Tab_New()" href="#New" data-toggle="tab">
                        <asp:UpdatePanel ID="Update_Tab_Name" runat="server" UpdateMode="Conditional">
                            <ContentTemplate>
                                <i class="fa fa-file"></i>&nbsp;&nbsp;
                                <asp:Label ID="Lbl_Tab_New" runat="server" Text="<%$ Resources:Attendance,New%>"></asp:Label>
                            </ContentTemplate>
                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="btnCancel" EventName="click" />
                                <asp:AsyncPostBackTrigger ControlID="btnsave" EventName="click" />
                                <asp:AsyncPostBackTrigger ControlID="btnReset" EventName="click" />
                                <asp:AsyncPostBackTrigger ControlID="btnGCancel" EventName="click" />
                                <asp:AsyncPostBackTrigger ControlID="btnGroupNext" EventName="click" />
                            </Triggers>
                        </asp:UpdatePanel>
                    </a></li>
                    <li id="Li_List" class="active"><a onclick="Li_Tab_List()" href="#List" data-toggle="tab">
                        <i class="fa fa-list"></i>&nbsp;&nbsp;<asp:Label ID="Label165" runat="server" Text="<%$ Resources:Attendance,List %>"></asp:Label></a></li>
                </ul>
                <div class="tab-content">
                    <div class="tab-pane active" id="List">
                        <asp:UpdatePanel ID="Update_List" runat="server" UpdateMode="Conditional">
                            <Triggers>
                                <asp:PostBackTrigger ControlID="btnExport" />
                                <asp:AsyncPostBackTrigger ControlID="btnCancel" EventName="click" />
                                <asp:AsyncPostBackTrigger ControlID="btnsave" EventName="click" />
                                <asp:AsyncPostBackTrigger ControlID="btnReset" EventName="click" />
                                <asp:AsyncPostBackTrigger ControlID="btnGCancel" EventName="click" />
                                <asp:AsyncPostBackTrigger ControlID="btnGroupNext" EventName="click" />
                            </Triggers>
                            <ContentTemplate>
                                <div class="row">
                                    <div class="col-md-12">


                                        <div class="row">
                                            <div class="col-md-12">
                                                <div id="Div1" runat="server" class="box box-info collapsed-box">
                                                    <div class="box-header with-border">
                                                        <h3 class="box-title">
                                                            <asp:Label ID="Label11" runat="server" Text="Advance Search"></asp:Label></h3>
                                                        &nbsp;&nbsp;|&nbsp;&nbsp;
                                                <asp:Label ID="lblTotalRecordNumber" Font-Bold="true" runat="server" Text="<%$ Resources:Attendance,Total Records: 0 %>"></asp:Label>

                                                        <div class="box-tools pull-right">
                                                            <button type="button" class="btn btn-box-tool" data-widget="collapse">
                                                                <i id="I1" runat="server" class="fa fa-plus"></i>
                                                            </button>
                                                        </div>
                                                    </div>
                                                    <div class="box-body">
                                                        <div class="col-lg-12">
                                                            <div class="col-lg-3">
                                                                <asp:DropDownList ID="ddlGroupSearch" runat="server" Class="form-control">
                                                                </asp:DropDownList>
                                                            </div>
                                                            <div class="col-lg-2">
                                                                <asp:TextBox ID="txtCountryFilter" runat="server" Class="form-control" onchange="validateCountry(this)" BackColor="#eeeeee" />
                                                                <cc1:AutoCompleteExtender ID="AutoCompleteExtender3" runat="server" DelimiterCharacters=""
                                                                    Enabled="True" ServiceMethod="GetCompletionListCountryName" ServicePath="" CompletionInterval="100"
                                                                    MinimumPrefixLength="1" CompletionSetCount="1" TargetControlID="txtCountryFilter"
                                                                    UseContextKey="True" CompletionListCssClass="autoCompleteList" CompletionListItemCssClass="autoCompleteListItem" CompletionListHighlightedItemCssClass="autoCompleteSelectedListItem" OnClientShown="resetPosition">
                                                                </cc1:AutoCompleteExtender>
                                                            </div>
                                                            <div class="col-lg-2">
                                                                <asp:LinkButton ID="btngo" runat="server" CausesValidation="False" OnClick="btngo_Click" ToolTip="<%$ Resources:Attendance,Search %>"><span class="fa fa-search"  style="font-size:25px;"></span></asp:LinkButton>&nbsp;&nbsp;&nbsp;
                                                                <asp:LinkButton ID="btnResetSreach" runat="server" CausesValidation="False" OnClick="btnResetSreach_Click" ToolTip="<%$ Resources:Attendance,Reset %>"><span class="fa fa-repeat"  style="font-size:25px;"></span></asp:LinkButton>
                                                                <asp:LinkButton ID="btnGridView" runat="server" Visible="false" CausesValidation="False" OnClick="btnGridView_Click" ToolTip="<%$ Resources:Attendance, Grid View %>"><span class="fa fa-list"  style="font-size:25px;"></span></asp:LinkButton>
                                                                <asp:LinkButton ID="btnTreeView" runat="server" CausesValidation="False" Visible="false" OnClick="btnTreeView_Click" ToolTip="<%$ Resources:Attendance, Tree View %>"></asp:LinkButton>
                                                            </div>

                                                            <div class="col-lg-3" style="display: none">
                                                                <asp:CheckBox ID="chkLicContact" runat="server" Text="Show Lic Contact" class="form-control" AutoPostBack="true" Visible="false"
                                                                    OnCheckedChanged="chkLicContact_OnCheckedChanged" />
                                                            </div>
                                                            <div class="col-lg-2" style="text-align: center; padding-left: 1px; padding-right: 1px;">
                                                                <asp:Button ID="btnExport" runat="server" CausesValidation="False" Text="Export To Excel"
                                                                    class="btn btn-primary" OnClick="btnExport_Click" Visible="false" />
                                                            </div>
                                                        </div>

                                                        <div class="col-lg-12">
                                                            <br />
                                                        </div>
                                                        <div class="col-lg-2">
                                                            <asp:DropDownList ID="ddlOptionField" runat="server" CssClass="form-control" OnSelectedIndexChanged="SetCustomerTextBox" AutoPostBack="true" Style="margin-left: 13px;">
                                                                <asp:ListItem Text="Contact Type" Value="CM.Status"></asp:ListItem>
                                                                <asp:ListItem Text="Company Name" Value="Contact_Company_Name"></asp:ListItem>
                                                                <asp:ListItem Text="Name" Value="CM.Name"></asp:ListItem>
                                                                <asp:ListItem Text="Mobile No" Value="CM.Trans_Id"></asp:ListItem>
                                                                <asp:ListItem Text="Email Id" Value="CM.Field1"></asp:ListItem>
                                                                <asp:ListItem Text="<%$ Resources:Attendance,Created By %>" Value="CreatedEmployee"></asp:ListItem>
                                                            </asp:DropDownList>
                                                        </div>
                                                        <div class="col-lg-2">
                                                            <asp:DropDownList ID="ddlOptionType" runat="server" CssClass="form-control">
                                                                <asp:ListItem Text="<%$ Resources:Attendance,--Select-- %>"></asp:ListItem>
                                                                <asp:ListItem Text="<%$ Resources:Attendance,Equal %>"></asp:ListItem>
                                                                <asp:ListItem Selected="True" Text="<%$ Resources:Attendance,Contains %>"></asp:ListItem>
                                                                <asp:ListItem Text="<%$ Resources:Attendance,Like %>"></asp:ListItem>
                                                            </asp:DropDownList>
                                                        </div>
                                                        <div class="col-lg-6">
                                                            <asp:Panel ID="Panel1" runat="server" DefaultButton="ImageButton3">
                                                                <asp:TextBox ID="txtSearchValue" runat="server" CssClass="form-control" Width="70%"></asp:TextBox>
                                                                <asp:TextBox ID="txtCustValue" runat="server" CssClass="form-control" Visible="false"></asp:TextBox>
                                                                <cc1:AutoCompleteExtender ID="AutoCompleteExtender10" runat="server" DelimiterCharacters=""
                                                                    Enabled="True" CompletionInterval="100" CompletionSetCount="1" MinimumPrefixLength="0"
                                                                    ServiceMethod="GetCompletionListCustomer" ServicePath="" TargetControlID="txtCustValue"
                                                                    UseContextKey="True" CompletionListCssClass="autoCompleteList" CompletionListItemCssClass="autoCompleteListItem" CompletionListHighlightedItemCssClass="autoCompleteSelectedListItem" OnClientShown="resetPosition">
                                                                </cc1:AutoCompleteExtender>
                                                                <asp:TextBox ID="txtValueDate" runat="server" CssClass="form-control" Visible="false"></asp:TextBox>
                                                                <cc1:CalendarExtender OnClientShown="showCalendar" ID="CalendarExtendertxtValueDate" runat="server" TargetControlID="txtValueDate" />
                                                            </asp:Panel>
                                                        </div>
                                                        <div class="col-lg-2">
                                                            <asp:LinkButton ID="ImageButton3" runat="server" CausesValidation="False" OnClick="btnbindrpt_Click" ToolTip="<%$ Resources:Attendance,Search %>"><span class="fa fa-search"  style="font-size:25px;"></span></asp:LinkButton>&nbsp;&nbsp;&nbsp;
                                                            <asp:LinkButton ID="ImageButton4" runat="server" CausesValidation="False" OnClick="btnRefreshReport_Click" ToolTip="<%$ Resources:Attendance,Refresh %>"><span class="fa fa-repeat"  style="font-size:25px;"></span></asp:LinkButton>&nbsp;&nbsp;&nbsp;
                                                            <asp:LinkButton ID="btnGvListSetting" ImageAlign="Right" ToolTip="List Settings" runat="server" OnClick="btnGvListSetting_Click" Visible="false"><span class="fa fa-wrench"  style="font-size:25px;"></span></asp:LinkButton>
                                                        </div>
                                                    </div>
                                                </div>

                                            </div>
                                        </div>


                                        <div class="box box-primary box-solid">
                                            <div class="box-header with-border">
                                                <label>Favorite Action : </label>
                                                <asp:DropDownList ID="ddlMyFav" runat="server" OnSelectedIndexChanged="ddlMyFav_SelectedIndexChanged" AutoPostBack="true" Style="color: black !important;">
                                                    <asp:ListItem Text="---Select---" Value="Select"></asp:ListItem>
                                                    <asp:ListItem Text="Add as Favorite" Value="Add"></asp:ListItem>
                                                    <asp:ListItem Text="Remove from Favorite" Value="Remove"></asp:ListItem>
                                                    <asp:ListItem Text="Favorite Only" Value="Favorite"></asp:ListItem>
                                                </asp:DropDownList>
                                                <h3 class="box-title"></h3>
                                            </div>
                                            <div class="box-body">
                                                <div class="row">
                                                    <div class="col-md-12">

                                                        <asp:GridView CssClass="table-striped table-bordered table table-hover" ID="gvContact" runat="server" AutoGenerateColumns="False" Width="100%" AllowPaging="false" AllowSorting="true"
                                                            OnPreRender="gvContact_OnPreRender" OnSorting="gvContact_Sorting">
                                                            <Columns>
                                                                <asp:TemplateField>
                                                                    <ItemTemplate>
                                                                        <asp:CheckBox ID="chkFav" runat="server" OnClick="SetDropdownValue();"></asp:CheckBox>
                                                                    </ItemTemplate>
                                                                    <ItemStyle HorizontalAlign="Center" />
                                                                </asp:TemplateField>


                                                                <asp:TemplateField HeaderText="Action">
                                                                    <ItemTemplate>
                                                                        <div class="dropdown" style="position: absolute;">
                                                                            <button class="btn btn-default dropdown-toggle" type="button" data-toggle="dropdown">
                                                                                <i class="fa fa-ellipsis-h" aria-hidden="true"></i>
                                                                            </button>
                                                                            <ul class="dropdown-menu">

                                                                                <li <%= hdnCanView.Value=="true"?"style='display:block'":"style='display:none'" %>>
                                                                                    <asp:LinkButton ID="lnkViewDetail" runat="server" CommandArgument='<%# Eval("Trans_Id") %>' TabIndex="9" ToolTip="View" OnCommand="lnkViewDetail_Command" CausesValidation="False"><i class="fa fa-eye"></i>View</asp:LinkButton>
                                                                                </li>

                                                                                <li <%= hdnCanEdit.Value=="true"?"style='display:block'":"style='display:none'" %>>
                                                                                    <asp:LinkButton ID="btnEdit" runat="server" CommandArgument='<%# Eval("Trans_Id") %>' CausesValidation="False" OnCommand="BtnEdit_Command" ToolTip="<%$ Resources:Attendance,Edit %>"><i class="fa fa-pencil"></i>Edit </asp:LinkButton>
                                                                                </li>
                                                                                <li <%= hdnCanDelete.Value=="true"?"style='display:block'":"style='display:none'" %>>
                                                                                    <asp:LinkButton ID="IbtnDelete" runat="server" CausesValidation="False" CommandArgument='<%# Eval("Trans_Id") %>' OnCommand="imgBtnDelete_Command" ToolTip="<%$ Resources:Attendance,Delete %>"><i class="fa fa-trash"></i>Delete</asp:LinkButton>
                                                                                    <cc1:ConfirmButtonExtender ID="confirm2" runat="server" ConfirmText="<%$ Resources:Attendance,Are you sure you want to delete the record?%>" TargetControlID="IbtnDelete"></cc1:ConfirmButtonExtender>
                                                                                </li>

                                                                                <li <%= hdnCanUpload.Value=="true"?"style='display:block'":"style='display:none'" %>>
                                                                                    <asp:LinkButton ID="btnFileUpload" runat="server" CommandArgument='<%# Eval("Trans_Id") %>' CommandName='<%# Eval("Code") %>' OnCommand="btnFileUpload_Command" CausesValidation="False"><i class="fa fa-upload"></i>File Upload</asp:LinkButton>
                                                                                </li>
                                                                            </ul>
                                                                        </div>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="Trans_Id" SortExpression="Trans_Id" Visible="false">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblTransId" runat="server" Text='<%#  Eval("Trans_Id") %>'></asp:Label>
                                                                    </ItemTemplate>
                                                                    <ItemStyle HorizontalAlign="Center" />
                                                                </asp:TemplateField>

                                                                <asp:TemplateField HeaderText="Contact Type" SortExpression="Status">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblContactType" runat="server" Text='<%# Eval("Status") %>'></asp:Label>
                                                                    </ItemTemplate>
                                                                    <ItemStyle HorizontalAlign="Center" />
                                                                </asp:TemplateField>

                                                                <asp:TemplateField HeaderText="Contact Id" SortExpression="Code">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblContactId" runat="server" Text='<%# Eval("Code") %>'></asp:Label>
                                                                    </ItemTemplate>
                                                                    <ItemStyle HorizontalAlign="Center" />
                                                                </asp:TemplateField>

                                                                <asp:TemplateField HeaderText="Name" SortExpression="ContactName">
                                                                    <ItemTemplate>
                                                                        <asp:Image ID="imgFav" runat="server" ImageUrl="~/Images/favorite.png" Height="14px" Width="14px" Visible='<%# Convert.ToBoolean(Eval("FavActive").ToString()) == true ? true : false %>' />
                                                                        <asp:Label ID="lblName" runat="server" Text='<%# Eval("ContactName") %>'></asp:Label>
                                                                        <asp:HiddenField ID="hdnIsFav" runat="server" Value='<%# Eval("FavActive") %>'></asp:HiddenField>
                                                                    </ItemTemplate>
                                                                    <ItemStyle HorizontalAlign="Center" />
                                                                </asp:TemplateField>

                                                                <asp:TemplateField HeaderText="Mobile No" SortExpression="Phone_no">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblMobileNo" runat="server" Text='<%# Eval("Phone_no") %>'></asp:Label><br />
                                                                        <asp:LinkButton ID="lBtnMoreNum" runat="server" Text='<%# Eval("More").ToString() %>' CommandArgument='<%# Eval("Trans_Id") %>' OnCommand="lBtnMoreNum_Command"></asp:LinkButton>
                                                                    </ItemTemplate>
                                                                    <ItemStyle HorizontalAlign="Center" />
                                                                </asp:TemplateField>

                                                                <asp:TemplateField HeaderText="Email Id" SortExpression="Field1">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblEmailId" runat="server" Text='<%# Eval("Field1") %>'></asp:Label>
                                                                    </ItemTemplate>
                                                                    <ItemStyle HorizontalAlign="Center" />
                                                                </asp:TemplateField>

                                                                <asp:TemplateField HeaderText="Contact Company Name" SortExpression="Contact_Company_Name">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblContactCompanyName" runat="server" Text='<%# Eval("Contact_Company_Name") %>'></asp:Label>
                                                                    </ItemTemplate>
                                                                    <ItemStyle HorizontalAlign="Center" />
                                                                </asp:TemplateField>

                                                                <asp:TemplateField HeaderText="Parent Contact Name" SortExpression="ParentContactName">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblParentContactName" runat="server" Text='<%# Eval("ParentContactName") %>'></asp:Label>
                                                                    </ItemTemplate>
                                                                    <ItemStyle HorizontalAlign="Center" />
                                                                </asp:TemplateField>

                                                                <asp:TemplateField HeaderText="Remarks" SortExpression="Remarks">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblRemarks" runat="server" Text='<%# Eval("Remarks") %>'></asp:Label>
                                                                    </ItemTemplate>
                                                                    <ItemStyle HorizontalAlign="Center" />
                                                                </asp:TemplateField>

                                                                <asp:TemplateField HeaderText="Created Employee" SortExpression="CreatedEmployee">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblCreatedEmployee" runat="server" Text='<%# Eval("CreatedEmployee") %>'></asp:Label>
                                                                    </ItemTemplate>
                                                                    <ItemStyle HorizontalAlign="Center" />
                                                                </asp:TemplateField>

                                                            </Columns>
                                                        </asp:GridView>
                                                        <div style="max-height: 500px; overflow: auto" id="Div_Tree" visible="false" runat="server">
                                                            <asp:TreeView ID="TreeViewCategory" runat="server" Visible="false" OnSelectedNodeChanged="TreeViewCategory_SelectedNodeChanged">
                                                            </asp:TreeView>
                                                        </div>
                                                        <asp:HiddenField ID="hdnParentContactId" runat="server" />
                                                        <asp:HiddenField ID="HDFSort" runat="server" />

                                                        <br />
                                                        <div class="col-md-12" style="margin-left: -30px">
                                                            <div class="col-md-9">
                                                                <asp:DataList CellPadding="10" RepeatDirection="Horizontal" runat="server" ID="dlPager" OnItemCommand="dlPager_ItemCommand">
                                                                    <ItemTemplate>
                                                                        <ul class="pagination">
                                                                            <li class='<%# Convert.ToBoolean(Eval("Enabled")) ? "page-item" : "page-item active" %>'>
                                                                                <asp:LinkButton Enabled='<%#Eval("Enabled") %>' runat="server" ID="lnkPageNo" Text='<%#Eval("Text") %>' CommandArgument='<%#Eval("Value") %>' CommandName="PageNo" CssClass="page-link"></asp:LinkButton>
                                                                            </li>
                                                                        </ul>
                                                                    </ItemTemplate>
                                                                </asp:DataList>
                                                            </div>
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

                    <div class="tab-pane" id="New">
                        <div class="row">
                            <div class="col-md-12">
                                <div class="col-md-12">
                                    <asp:UpdatePanel ID="Update_New" runat="server" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <div runat="server" id="PnlNewEdit" class="row">
                                                <div id="Div_Box_Add" class="box box-info">
                                                    <div class="box-header with-border">
                                                        <h3 class="box-title">Basic Details</h3>
                                                        <div class="box-tools pull-right">
                                                            <button type="button" class="btn btn-box-tool" data-widget="collapse">
                                                                <i id="Btn_Add_Div" class="fa fa-minus"></i>
                                                            </button>
                                                        </div>
                                                    </div>
                                                    <div class="box-body">
                                                        <div class="form-group">
                                                            <div class="col-md-12">
                                                                <asp:ImageButton ID="btnControlsSetting" ImageAlign="Right" ToolTip="Control Setting" runat="server" ImageUrl="~/Images/setting.png" OnClick="btnControlsSetting_Click" Style="width: 32px; height: 32px" Visible="false" />
                                                            </div>
                                                            <div class="col-md-12">
                                                                <div class="col-md-12" style="text-align: center;">
                                                                    <br />
                                                                    <asp:RadioButtonList ID="RdolistSelect" runat="server" RepeatDirection="Horizontal" CellSpacing="0" CellPadding="0" AutoPostBack="true" OnSelectedIndexChanged="RdolistSelect_SelectedIndexChanged">
                                                                        <asp:ListItem Selected="True" Text="Individual" Value="Individual"></asp:ListItem>
                                                                        <asp:ListItem Text="Company" style="margin-left: 25px;" Value="Company"></asp:ListItem>
                                                                    </asp:RadioButtonList>
                                                                </div>
                                                            </div>
                                                            <div class="col-md-12">
                                                                <div class="col-md-6">
                                                                    <asp:Label ID="lblId" runat="server" Text="<%$Resources:Attendance,Id %>"></asp:Label>
                                                                    <asp:TextBox ID="txtId" runat="server" TabIndex="1" Class="form-control" BackColor="#eeeeee" AutoPostBack="true"
                                                                        OnTextChanged="txtId_TextChanged"></asp:TextBox>
                                                                    <cc1:AutoCompleteExtender ID="AutoCompleteExtender5" runat="server" DelimiterCharacters=""
                                                                        Enabled="True" ServiceMethod="GetCompletionListContactId" ServicePath="" CompletionInterval="100"
                                                                        MinimumPrefixLength="1" CompletionSetCount="1" TargetControlID="txtId" UseContextKey="True" CompletionListCssClass="autoCompleteList" CompletionListItemCssClass="autoCompleteListItem" CompletionListHighlightedItemCssClass="autoCompleteSelectedListItem" OnClientShown="resetPosition">
                                                                    </cc1:AutoCompleteExtender>
                                                                    <br />
                                                                </div>
                                                            </div>
                                                            <div class="col-md-12">
                                                                <div class="col-md-6">
                                                                    <asp:Label ID="lblName" runat="server" Text="<%$Resources:Attendance, Name %>"></asp:Label>
                                                                    <a style="color: Red">*</a>
                                                                    <asp:RequiredFieldValidator EnableClientScript="true" Style="float: right;" runat="server"
                                                                        ID="RequiredFieldValidator1" ValidationGroup="Contact_Save" Display="Dynamic"
                                                                        SetFocusOnError="true" ControlToValidate="txtName" ErrorMessage="<%$ Resources:Attendance,Enter Name %>" />
                                                                    <div style="width: 100%" class="input-group">
                                                                        <asp:DropDownList ID="ddlNamePrefix" runat="server" Style="width: 30%" class="form-control">
                                                                            <asp:ListItem Text="<%$ Resources:Attendance,Mr. %>" Selected="True" Value="Mr."></asp:ListItem>
                                                                            <asp:ListItem Text="<%$ Resources:Attendance,Miss %>" Value="Miss"></asp:ListItem>
                                                                            <asp:ListItem Text="<%$ Resources:Attendance,Mrs. %>" Value="Mrs."></asp:ListItem>
                                                                            <asp:ListItem Text="<%$ Resources:Attendance,Dr. %>" Value="Dr."></asp:ListItem>
                                                                        </asp:DropDownList>
                                                                        <asp:TextBox ID="txtName" TabIndex="2" Style="width: 70%" runat="server" Class="form-control"></asp:TextBox>
                                                                    </div>
                                                                </div>
                                                                <div class="col-md-6" id="ctlNameL" runat="server">
                                                                    <asp:Label ID="lblNameL" runat="server" Text="<%$Resources:Attendance, Name (Local) %>"></asp:Label>
                                                                    <asp:TextBox ID="txtNameL" TabIndex="3" runat="server" Class="form-control"></asp:TextBox>
                                                                </div>
                                                            </div>

                                                            <div class="col-md-12">
                                                                <div class="col-md-6">
                                                                    <br />
                                                                    <asp:Label ID="lblPermanentEMailId" runat="server" Text="<%$ Resources:Attendance,Permanent EmailId%>" />
                                                                    <a style="color: Red">*</a>
                                                                    <asp:RequiredFieldValidator EnableClientScript="true" Style="float: right;" runat="server"
                                                                        ID="RequiredFieldValidator5" ValidationGroup="Add_Email_ID" Display="Dynamic"
                                                                        SetFocusOnError="true" ControlToValidate="txtPermanentMailId" ErrorMessage="<%$ Resources:Attendance,Enter Permanent EmailId %>" />
                                                                    <div style="width: 100%" class="input-group">
                                                                        <asp:TextBox ID="txtPermanentMailId" TabIndex="4" Style="width: 74%;" runat="server" class="form-control"
                                                                            BackColor="#eeeeee" />
                                                                        <cc1:AutoCompleteExtender ID="AutoCompleteExtender1" runat="server" DelimiterCharacters=""
                                                                            Enabled="True" ServiceMethod="GetCompletionListEmailMaster" ServicePath="" CompletionInterval="100"
                                                                            MinimumPrefixLength="1" CompletionSetCount="1" TargetControlID="txtPermanentMailId"
                                                                            UseContextKey="True" CompletionListCssClass="autoCompleteList" CompletionListItemCssClass="autoCompleteListItem" CompletionListHighlightedItemCssClass="autoCompleteSelectedListItem" OnClientShown="resetPosition">
                                                                        </cc1:AutoCompleteExtender>
                                                                        <asp:Button ID="btnAddEmail" TabIndex="5" ValidationGroup="Add_Email_ID" Style="width: 15%; margin-left: 10px; margin-top: -20px;"
                                                                            runat="server" Visible="false" Text="<%$ Resources:Attendance,Add %>" Class="btn btn-info"
                                                                            OnClick="btnAddEmail_Click" />&nbsp;&nbsp;
                                                <asp:LinkButton ID="btnRemoveEmail" runat="server" CausesValidation="False" OnClick="btnRemoveEmail_Click" ToolTip="<%$ Resources:Attendance,Delete %>"><i class="fa fa-remove" style="font-size:35px;margin-top: -10px; padding-top: 11px;"></i></asp:LinkButton>
                                                                    </div>
                                                                    <br />
                                                                </div>
                                                                <div class="col-md-6">
                                                                    <br />
                                                                    <asp:Label ID="lblAddressName" runat="server" Text="<%$ Resources:Attendance,Address Name %>"></asp:Label>
                                                                    <asp:RequiredFieldValidator EnableClientScript="true" Style="float: right;" runat="server" ID="RequiredFieldValidator6" ValidationGroup="Address"
                                                                        Display="Dynamic" SetFocusOnError="true" ControlToValidate="txtAddressName" ErrorMessage="<%$ Resources:Attendance,Enter Address Name %>" />
                                                                    <div class="input-group">
                                                                        <asp:TextBox ID="txtAddressName" TabIndex="6" BackColor="#eeeeee" onchange="txtAddressName_TextChanged()"
                                                                            runat="server" CssClass="form-control"></asp:TextBox>
                                                                        <cc1:AutoCompleteExtender ID="AutoCompleteExtender2" runat="server" CompletionInterval="100"
                                                                            CompletionSetCount="1" DelimiterCharacters="" Enabled="True" MinimumPrefixLength="1"
                                                                            ServiceMethod="GetCompletionListAddressName" ServicePath="" TargetControlID="txtAddressName"
                                                                            UseContextKey="True" CompletionListCssClass="autoCompleteList" CompletionListItemCssClass="autoCompleteListItem" CompletionListHighlightedItemCssClass="autoCompleteSelectedListItem" OnClientShown="resetPosition">
                                                                        </cc1:AutoCompleteExtender>
                                                                        <div class="input-group-btn">
                                                                            <asp:Button ID="imgAddAddressName" TabIndex="7" ValidationGroup="Address" Style="margin-left: 10px;" Visible="false"
                                                                                OnClick="imgAddAddressName_Click"
                                                                                CssClass="btn btn-info" runat="server" Text="<%$ Resources:Attendance,Add %>" />
                                                                            <asp:Button ID="btnAddNewAddress" Style="margin-left: 10px;"
                                                                                Visible="false" OnClick="btnAddNewAddress_Click" CssClass="btn btn-info"
                                                                                runat="server" Text="<%$ Resources:Attendance,New %>" />
                                                                            <asp:HiddenField ID="Hdn_Address_ID" runat="server" />
                                                                            <asp:HiddenField ID="hdnAddressId" runat="server" />
                                                                            <asp:HiddenField ID="hdnAddressName" runat="server" />
                                                                            <asp:HiddenField ID="hdntxtaddressid" runat="server" />
                                                                        </div>
                                                                    </div>

                                                                    <br />
                                                                </div>
                                                            </div>
                                                            <div class="col-md-12">
                                                                <div class="col-md-6">
                                                                    <%-- <asp:CustomValidator ID="CustomValidator2" ValidationGroup="Contact_Save" runat="server" ErrorMessage="<%$ Resources:Attendance,Enter Permanent EmailId %>"
                                                            ClientValidationFunction="Mail_Group_Check" ForeColor="Red"></asp:CustomValidator>--%>
                                                                    <asp:RadioButtonList ID="rbnEmailList" runat="server">
                                                                    </asp:RadioButtonList>
                                                                </div>
                                                            </div>
                                                            <div class="col-md-12">
                                                                <div class="col-md-12">
                                                                    <div style="overflow: auto">
                                                                        <asp:GridView CssClass="table-striped table-bordered table table-hover" ID="GvAddressName" runat="server" AutoGenerateColumns="False" Width="100%">

                                                                            <Columns>
                                                                                <asp:TemplateField>
                                                                                    <ItemTemplate>
                                                                                        <asp:LinkButton ToolTip="Edit" ID="imgBtnAddressEdit" runat="server" CommandArgument='<%# Eval("Trans_Id") %>' OnCommand="btnAddressEdit_Command"><i class="fa fa-pencil"></i></asp:LinkButton>
                                                                                    </ItemTemplate>
                                                                                    <ItemStyle HorizontalAlign="Center" />
                                                                                </asp:TemplateField>
                                                                                <asp:TemplateField>
                                                                                    <ItemTemplate>
                                                                                        <asp:LinkButton ID="imgBtnDelete" ToolTip="Delete" runat="server" CommandArgument='<%# Eval("Trans_Id") %>' OnCommand="btnAddressDelete_Command"><i class="fa fa-trash"></i></asp:LinkButton>
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
                                                                                        <asp:Label ID="lblgvMobileNo" runat="server" CssClass="labelComman" Text='<%#GetContactMobileNo(Eval("Address_Name").ToString()) %>' /><br />
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
                                                                    <asp:Label ID="Label8" runat="server" Text="<%$ Resources:Attendance,Country Name%>" />
                                                                    <a style="color: Red">*</a>
                                                                    <asp:RequiredFieldValidator EnableClientScript="true" Style="float: right;" runat="server"
                                                                        ID="RequiredFieldValidator3" ValidationGroup="Contact_Save" Display="Dynamic"
                                                                        SetFocusOnError="true" ControlToValidate="txtCountryName" ErrorMessage="<%$ Resources:Attendance,Enter Country Name%>" />
                                                                    <asp:TextBox ID="txtCountryName" TabIndex="8" runat="server" class="form-control" OnTextChanged="txtCountryName_TextChanged"
                                                                        AutoPostBack="true" BackColor="#eeeeee" />
                                                                    <cc1:AutoCompleteExtender ID="AutoCompleteExtendercountry" runat="server" DelimiterCharacters=""
                                                                        Enabled="True" ServiceMethod="GetCompletionListCountryName" ServicePath="" CompletionInterval="100"
                                                                        MinimumPrefixLength="1" CompletionSetCount="1" TargetControlID="txtCountryName"
                                                                        UseContextKey="True" CompletionListCssClass="autoCompleteList" CompletionListItemCssClass="autoCompleteListItem" CompletionListHighlightedItemCssClass="autoCompleteSelectedListItem" OnClientShown="resetPosition">
                                                                    </cc1:AutoCompleteExtender>
                                                                    <asp:HiddenField ID="hdncountryid" runat="server" />
                                                                    <br />
                                                                    <%-- <asp:Label ID="lblPermanentMobileNo" runat="server" Text="<%$ Resources:Attendance,Permanent MobileNo.%>" />
                                                                    <div style="width: 100%;" class="input-group">
                                                                        <asp:DropDownList ID="ddlCountryCode" Style="width: 30%;" runat="server" CssClass="form-control">
                                                                        </asp:DropDownList>
                                                                        <asp:TextBox ID="txtPermanentMobileNo" Style="width: 70%;" runat="server" CssClass="form-control" />
                                                                        <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" runat="server" Enabled="True"
                                                                            TargetControlID="txtPermanentMobileNo" FilterType="Numbers">
                                                                        </cc1:FilteredTextBoxExtender>
                                                                    </div>--%>
                                                                    <asp:Label ID="Label26" runat="server" Text="<%$Resources:Attendance,Parent Contact Name %>"></asp:Label>
                                                                    <asp:TextBox ID="txtParentContactName" BackColor="#eeeeee" runat="server" CssClass="form-control"
                                                                        AutoPostBack="true" OnTextChanged="txtParentContactName_TextChanged"></asp:TextBox>
                                                                    <cc1:AutoCompleteExtender ID="AutoCompleteExtender8" runat="server" DelimiterCharacters=""
                                                                        Enabled="True" ServiceMethod="GetCompletionListComapnyName" ServicePath="" CompletionInterval="100"
                                                                        MinimumPrefixLength="1" CompletionSetCount="1" TargetControlID="txtParentContactName"
                                                                        UseContextKey="True" CompletionListCssClass="autoCompleteList" CompletionListItemCssClass="autoCompleteListItem" CompletionListHighlightedItemCssClass="autoCompleteSelectedListItem" OnClientShown="resetPosition">
                                                                    </cc1:AutoCompleteExtender>

                                                                    <asp:Label ID="lblCompanyName" runat="server" Text="<%$Resources:Attendance,Company Name %>"></asp:Label>
                                                                    <asp:TextBox ID="txtCompany" TabIndex="10" BackColor="#eeeeee" runat="server" CssClass="form-control"
                                                                        AutoPostBack="true" OnTextChanged="txtCompany_TextChanged"></asp:TextBox>
                                                                    <cc1:AutoCompleteExtender ID="AutoCompleteExtender4" runat="server" DelimiterCharacters=""
                                                                        Enabled="True" ServiceMethod="GetCompletionListComapnyName" ServicePath="" CompletionInterval="100"
                                                                        MinimumPrefixLength="1" CompletionSetCount="1" TargetControlID="txtCompany" UseContextKey="True" CompletionListCssClass="autoCompleteList" CompletionListItemCssClass="autoCompleteListItem" CompletionListHighlightedItemCssClass="autoCompleteSelectedListItem" OnClientShown="resetPosition">
                                                                    </cc1:AutoCompleteExtender>
                                                                    <br />
                                                                </div>

                                                                <div class="col-md-6">
                                                                    <asp:Label ID="lblCurrency" runat="server" Text="<%$ Resources:Attendance,Currency Name %>"></asp:Label>
                                                                    <a style="color: Red">*</a>
                                                                    <asp:RequiredFieldValidator EnableClientScript="true" Style="float: right;" runat="server"
                                                                        ID="RequiredFieldValidator4" ValidationGroup="Contact_Save" Display="Dynamic"
                                                                        SetFocusOnError="true" ControlToValidate="ddlCurrency" InitialValue="--Select--"
                                                                        ErrorMessage="<%$ Resources:Attendance,Select Currency Name %>" />
                                                                    <asp:DropDownList ID="ddlCurrency" TabIndex="9" runat="server" Class="form-control" />
                                                                    <br />
                                                                </div>
                                                            </div>
                                                            <div class="col-md-12" id="ctlNotes" runat="server">
                                                                <div class="col-md-12">
                                                                    <asp:Label ID="lblRemark" runat="server" Text="<%$ Resources:Attendance,Notes %>"></asp:Label>
                                                                    <a style="color: Red">*</a>
                                                                    <asp:RequiredFieldValidator EnableClientScript="true" Style="float: right;" runat="server"
                                                                        ID="RequiredFieldValidator2" ValidationGroup="Contact_Save" Display="Dynamic"
                                                                        SetFocusOnError="true" ControlToValidate="txtRemark" ErrorMessage="<%$ Resources:Attendance,Write about Business %>" />
                                                                    <asp:TextBox ID="txtRemark" TabIndex="12" runat="server" CssClass="form-control" TextMode="MultiLine" />
                                                                    <br />
                                                                </div>
                                                            </div>

                                                            <uc1:ContactNo1 runat="server" ID="ContactNo1" />

                                                        </div>
                                                    </div>
                                                </div>
                                                <div id="Div_Box_Additional" class="box box-info">
                                                    <div class="box-header with-border">
                                                        <h3 class="box-title">Additional Details</h3>
                                                        <div class="box-tools pull-right">
                                                            <button type="button" class="btn btn-box-tool" data-widget="collapse">
                                                                <i id="Btn_Additional_Div" class="fa fa-minus"></i>
                                                            </button>
                                                        </div>
                                                    </div>
                                                    <div class="box-body">
                                                        <div id="TbRefContact" runat="server">
                                                            <div class="col-md-12">
                                                                <div class="col-md-6">
                                                                    <div id="ctlDepartment" runat="server">
                                                                        <asp:Label ID="lblDepartment" runat="server" Text="<%$ Resources:Attendance,Department %>"></asp:Label>
                                                                        <asp:DropDownList ID="ddlDepartment" TabIndex="13" runat="server" CssClass="form-control" />
                                                                        <br />
                                                                    </div>
                                                                    <asp:Label ID="lblReligion" runat="server" Text="<%$ Resources:Attendance,Religion %>" />
                                                                    <asp:DropDownList ID="ddlReligion" TabIndex="15" runat="server" CssClass="form-control" />
                                                                    <br />
                                                                </div>
                                                                <div class="col-md-6">
                                                                    <div id="ctlDesignation" runat="server">
                                                                        <asp:Label ID="lblDesignation" runat="server" Text="<%$ Resources:Attendance,Designation %>" />
                                                                        <asp:DropDownList ID="ddlDesignation" TabIndex="14" runat="server" CssClass="form-control" />
                                                                        <br />
                                                                    </div>
                                                                    <asp:Label ID="lblCivilId" runat="server" Text="<%$ Resources:Attendance,Civil Id %>" />
                                                                    <asp:TextBox ID="txtCivilId" TabIndex="16" runat="server" CssClass="form-control" />
                                                                    <br />
                                                                </div>
                                                            </div>
                                                        </div>
                                                        <div class="col-md-12">
                                                            <div class="col-md-6">
                                                                <asp:Label ID="lblBankName" runat="server" Text="<%$ Resources:Attendance,Bank Name %>" />
                                                                <div style="width: 100%;" class="input-group">
                                                                    <asp:TextBox ID="txtContactBankName" TabIndex="16" Style="width: 82%" runat="server" class="form-control"
                                                                        AutoPostBack="true" OnTextChanged="txtContactBankName_TextChanged" BackColor="#eeeeee" />
                                                                    <cc1:AutoCompleteExtender ID="AutoCompleteExtender6" runat="server" DelimiterCharacters=""
                                                                        Enabled="True" ServiceMethod="GetCompletionListBankName" ServicePath="" CompletionInterval="100"
                                                                        MinimumPrefixLength="1" CompletionSetCount="1" TargetControlID="txtContactBankName"
                                                                        UseContextKey="True" CompletionListCssClass="autoCompleteList" CompletionListItemCssClass="autoCompleteListItem" CompletionListHighlightedItemCssClass="autoCompleteSelectedListItem" OnClientShown="resetPosition">
                                                                    </cc1:AutoCompleteExtender>
                                                                    <asp:Button ID="BtnContactBank" TabIndex="17" Style="width: 15%; margin-left: 10px;" runat="server"
                                                                        Visible="false" Text="<%$ Resources:Attendance,Add %>" Class="btn btn-info"
                                                                        OnClick="BtnContactBank_click" />
                                                                </div>
                                                                <br />
                                                            </div>
                                                            <div class="col-md-12">
                                                                <div style="overflow: auto; max-height: 300px;">
                                                                    <asp:GridView CssClass="table-striped table-bordered table table-hover" ID="GvContactBankDetail" Width="100%" runat="server"
                                                                        AutoGenerateColumns="False">

                                                                        <Columns>
                                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,Edit %>">
                                                                                <ItemTemplate>
                                                                                    <asp:ImageButton TabIndex="18" ID="imgBtnContactBankEdit" runat="server" CommandArgument='<%# Eval("Trans_Id") %>'
                                                                                        ImageUrl="~/Images/edit.png" Width="16px" ToolTip="<%$ Resources:Attendance,Edit %>"
                                                                                        OnCommand="imgBtnContactBankEdit_Command" Visible="false" />
                                                                                </ItemTemplate>
                                                                                <ItemStyle HorizontalAlign="Center" Width="16px" />
                                                                            </asp:TemplateField>
                                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,Delete %>">
                                                                                <ItemTemplate>
                                                                                    <asp:ImageButton ID="imgBtnContactBankDelete" runat="server" CommandArgument='<%# Eval("Trans_Id") %>'
                                                                                        Height="16px" ImageUrl="~/Images/Erase.png" Width="16px" ToolTip="<%$ Resources:Attendance,Delete %>"
                                                                                        OnCommand="imgBtnContactBankDelete_Command" Visible="false" />
                                                                                </ItemTemplate>
                                                                                <ItemStyle HorizontalAlign="Center" Width="16px" />
                                                                            </asp:TemplateField>
                                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance, Bank Name %>">
                                                                                <ItemTemplate>
                                                                                    <asp:Label ID="lblgvBankId" Width="100px" runat="server" Text='<%#Eval("Bank_Name") %>' />
                                                                                </ItemTemplate>
                                                                                <ItemStyle HorizontalAlign="Center" Width="100px" />
                                                                            </asp:TemplateField>
                                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,Account No %>">
                                                                                <ItemTemplate>
                                                                                    <asp:Label ID="lblgvBankAccount" Width="100px" runat="server" Text='<%#Eval("Account_No") %>' />
                                                                                </ItemTemplate>
                                                                                <ItemStyle HorizontalAlign="Center" Width="100px" />
                                                                            </asp:TemplateField>
                                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,IFSC Code %>">
                                                                                <ItemTemplate>
                                                                                    <asp:Label ID="lblgvBankIFSCCode" Width="100px" runat="server" Text='<%#Eval("IFSC_Code") %>' />
                                                                                </ItemTemplate>
                                                                                <ItemStyle HorizontalAlign="Center" Width="100px" />
                                                                            </asp:TemplateField>
                                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,MICR Code %>">
                                                                                <ItemTemplate>
                                                                                    <asp:Label ID="lblgvBankMICRCode" Width="100px" runat="server" Text='<%#Eval("MICR_Code") %>' />
                                                                                </ItemTemplate>
                                                                                <ItemStyle HorizontalAlign="Center" Width="100px" />
                                                                            </asp:TemplateField>
                                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,Branch Code %>">
                                                                                <ItemTemplate>
                                                                                    <asp:Label ID="lblgvBankBranchCode" Width="100px" runat="server" Text='<%#Eval("Branch_Code") %>' />
                                                                                </ItemTemplate>
                                                                                <ItemStyle HorizontalAlign="Center" Width="100px" />
                                                                            </asp:TemplateField>
                                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,IBAN Number %>">
                                                                                <ItemTemplate>
                                                                                    <asp:Label ID="lblgvIBankNo" Width="100px" runat="server" Text='<%#Eval("IBAN_NUMBER") %>' />
                                                                                </ItemTemplate>
                                                                                <ItemStyle HorizontalAlign="Center" Width="100px" />
                                                                            </asp:TemplateField>
                                                                        </Columns>

                                                                        <PagerStyle CssClass="pagination-ys" />

                                                                    </asp:GridView>
                                                                </div>
                                                                <br />
                                                            </div>
                                                        </div>
                                                        <div class="col-md-6">
                                                            <asp:Label ID="lblEmpLogo" runat="server" Text="<%$ Resources:Attendance,Image  %>"></asp:Label>
                                                            <div class="input-group" style="width: 100%; display:none">
                                                                <cc1:AsyncFileUpload ID="FULogoPath" OnUploadedComplete="FuLogo_FileUploadComplete" OnClientUploadError="FuLogoUploadError" OnClientUploadStarted="FuLogouploadStarted" OnClientUploadComplete="FuLogouploadComplete"
                                                                    runat="server" CssClass="form-control" CompleteBackColor="White" UploaderStyle="Traditional" UploadingBackColor="#CCFFFF" ThrobberID="imgLoader" Width="100%" />
                                                                <div class="input-group-btn" style="border: solid 1px #d2d6de; width: 35px;">
                                                                    <asp:Image ID="Img_Right" runat="server" Width="30px" Height="30px" ImageUrl="../Images/Allow.png" Style="display: none" />
                                                                    <asp:Image ID="Img_Wrong" runat="server" Width="30px" Height="30px" ImageUrl="../Images/Delete1.png" Style="display: none" />
                                                                    <asp:Image ID="imgLoader" runat="server" ImageUrl="../Images/loader.gif" />
                                                                </div>
                                                            </div>
                                                            <br />
                                                        </div>
                                                        <div class="col-md-6" style="text-align: center">
                                                            <asp:Image ID="imgLogo" ClientIDMode="Static" runat="server" Width="90px" Height="120px" /><br />
                                                            <br />
                                                             <br />&nbsp;
                                                            <a onclick="popup.Show();" runat="server" id="File_Manager_Product" class="btn btn-primary" style="cursor: pointer">File Manager</a>

                                                            <asp:Button ID="btnUpload" Visible="false" runat="server" Text="<%$ Resources:Attendance,Load %>"
                                                                class="btn btn-primary" OnClick="btnUpload1_Click" />
                                                            <br />
                                                        </div>
                                                    </div>
                                                </div>
                                                <div class="row">
                                                    <div class="col-md-12">
                                                        <div class="box box-primary">
                                                            <div class="box-body">
                                                                <div class="form-group">
                                                                    <div class="col-md-12">
                                                                        <asp:CheckBox ID="chkIsReseller" TabIndex="19" Style="margin-left: 10px; margin-right: 10px;" runat="server" Text="Is Allow for Invoice" Visible="false" />
                                                                        <asp:CheckBox ID="chkIsEmail" TabIndex="20" Style="margin-left: 10px; margin-right: 10px;" runat="server" Text="<%$ Resources:Attendance,Send Email %>" />
                                                                        <asp:CheckBox ID="chkIsSMS" TabIndex="21" Style="margin-left: 10px; margin-right: 10px;" runat="server" Text="<%$ Resources:Attendance,Send SMS %>" />
                                                                        <asp:CheckBox ID="chkVerify" TabIndex="22" Style="margin-left: 10px; margin-right: 10px;" runat="server" Text="Verify" Visible="false" />
                                                                    </div>
                                                                    <div class="col-md-12" style="text-align: center;">

                                                                        <asp:Button ID="btnReset" runat="server" Text="<%$ Resources:Attendance,Reset %>" CssClass="btn btn-primary" OnClick="btnReset_Click" />
                                                                        <asp:Button ID="btnCancel" runat="server" CausesValidation="False" CssClass="btn btn-danger" Text="<%$ Resources:Attendance,Cancel %>" OnClick="btnCancel_Click" />
                                                                        <asp:Button ID="btnsave" TabIndex="23" ValidationGroup="Contact_Save" runat="server" Text="<%$ Resources:Attendance,Next %>" CssClass="btn btn-primary" Visible="false" OnClick="btnSave_Click" />
                                                                        <asp:HiddenField ID="hdnContactId" runat="server" />
                                                                        <asp:HiddenField ID="hdnCompId" runat="server" />
                                                                    </div>
                                                                </div>
                                                            </div>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="row" id="PnlGroup" visible="false" runat="server">
                                                <div class="col-md-12">
                                                    <div class="col-md-6">
                                                        <asp:Label ID="lblPnlGroupContactId" Font-Size="14px" runat="server"
                                                            Text="<%$ Resources:Attendance, Id %>" />
                                                        &nbsp : &nbsp
                                            <asp:Label ID="txtPnlGroupContactId" Font-Size="14px" Font-Bold="true" runat="server" />
                                                        <br />
                                                    </div>
                                                    <div class="col-md-6">
                                                        <asp:Label ID="lblPnlGroupContactName" Font-Size="14px" runat="server"
                                                            Text="<%$ Resources:Attendance, Name %>" />
                                                        &nbsp : &nbsp
                                            <asp:Label ID="txtPnlGroupContactName" Font-Size="14px" Font-Bold="true" runat="server" />
                                                    </div>
                                                </div>
                                                <div class="col-md-12">
                                                    <cc1:TabContainer ID="TabContainer2" runat="server" CssClass="ajax__tab_yuitabview-theme">
                                                        <cc1:TabPanel ID="Group" runat="server">
                                                            <HeaderTemplate>
                                                                <asp:Label ID="Label3" runat="server" Text="<%$ Resources:Attendance,Group %>"></asp:Label>
                                                            </HeaderTemplate>
                                                            <ContentTemplate>
                                                                <div class="row">
                                                                    <div class="col-md-12" id="Tree_Div" runat="server">
                                                                        <%--<asp:CustomValidator ID="CustomValidator1" ValidationGroup="Group_Save" runat="server" ErrorMessage="<%$ Resources:Attendance,Please select at least one record %>"
                                                                            ClientValidationFunction="Validate_nav_tree" ForeColor="Red"></asp:CustomValidator>--%>
                                                                        <div style="height: 250px; overflow: auto;">
                                                                            <asp:CheckBox ID="chkSelectAll" onClick="new_validation();" Text="select all" runat="server" />
                                                                            <br />
                                                                            <div id="inner-content-Tree" style="height: 200px; overflow: auto;">
                                                                                <asp:TreeView ID="navTree" runat="server" Height="100%" ShowCheckBoxes="All">
                                                                                </asp:TreeView>
                                                                            </div>
                                                                        </div>
                                                                    </div>
                                                                    <div id="PnlCustomerId" runat="server" visible="False" class="col-md-12">
                                                                        <div class="col-xs-4" style="display: none">
                                                                            <asp:Label ID="lblCustomerId" runat="server" Text="<%$ Resources:Attendance,Customer Id %>"></asp:Label>
                                                                            <asp:TextBox ID="txtCustomerId" runat="server" CssClass="form-control" BackColor="#eeeeee"
                                                                                AutoPostBack="True" OnTextChanged="txtCustomerId_TextChanged"></asp:TextBox>
                                                                        </div>
                                                                        <div class="col-xs-4" style="display: none">
                                                                            <asp:Label ID="Label4" runat="server" Text="TIN No."></asp:Label>
                                                                            <asp:TextBox ID="txtcustomerTinNo" runat="server" CssClass="form-control"></asp:TextBox>
                                                                        </div>
                                                                        <div class="col-xs-4" style="display: none">
                                                                            <asp:Label ID="Label9" runat="server" Text="CST No."></asp:Label>
                                                                            <asp:TextBox ID="txtcustomerCstNo" runat="server" CssClass="form-control"></asp:TextBox>
                                                                        </div>
                                                                    </div>
                                                                    <div class="col-md-12" id="PnlSupplierId" runat="server" visible="False">
                                                                        <div class="col-xs-4" style="display: none">
                                                                            <asp:Label ID="lblSupplier" runat="server" Text="<%$ Resources:Attendance,Supplier Id %>"></asp:Label>
                                                                            <asp:TextBox ID="txtSupplierId" runat="server" CssClass="form-control" BackColor="#eeeeee"
                                                                                AutoPostBack="True" OnTextChanged="txtSupplierId_TextChanged"></asp:TextBox>
                                                                        </div>
                                                                        <div class="col-xs-4" style="display: none">
                                                                            <asp:Label ID="Label15" runat="server" Text="TIN No."></asp:Label>
                                                                            <asp:TextBox ID="txtsupplierTinNo" runat="server" CssClass="form-control"></asp:TextBox>
                                                                        </div>
                                                                        <div class="col-xs-4" style="display: none">
                                                                            <asp:Label ID="Label17" runat="server" Text="CST No."></asp:Label>
                                                                            <asp:TextBox ID="txtsupplierCstNo" runat="server" CssClass="form-control"></asp:TextBox>
                                                                        </div>
                                                                    </div>
                                                                    <div class="col-md-12" id="PnlGSTIN" runat="server">
                                                                        <div class="col-md-6">
                                                                            <asp:Label ID="lblGSTIN" runat="server" Text="GSTIN No."></asp:Label>
                                                                            <asp:TextBox ID="txtGSTIN" runat="server" CssClass="form-control" MaxLength="15"></asp:TextBox>
                                                                            <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender10" runat="server" Enabled="True"
                                                                                TargetControlID="txtGSTIN" ValidChars="1234567890abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ">
                                                                            </cc1:FilteredTextBoxExtender>
                                                                        </div>

                                                                        <div class="col-md-6">
                                                                            <asp:Label ID="Label1" runat="server" Text="TRN No."></asp:Label>
                                                                            <asp:TextBox ID="Txt_TRN_No" runat="server" CssClass="form-control" MaxLength="20"></asp:TextBox>
                                                                            <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender2" runat="server" Enabled="True"
                                                                                TargetControlID="Txt_TRN_No" ValidChars="1234567890abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ">
                                                                            </cc1:FilteredTextBoxExtender>
                                                                        </div>
                                                                    </div>
                                                                    <div class="col-md-12">
                                                                        <div class="col-xs-6">
                                                                            <asp:Label ID="lbl" runat="server" Font-Bold="True" Font-Names="arial" Font-Size="13px"
                                                                                ForeColor="#666666" Text="<%$ Resources:Attendance,Company %>"></asp:Label>
                                                                            <asp:Panel ID="pnlCompany" runat="server" Height="200px" BorderStyle="Solid" BorderWidth="1px"
                                                                                BorderColor="#ABADB3" BackColor="White" ScrollBars="Auto">
                                                                                <asp:CheckBoxList ID="chkCompany" runat="server" RepeatColumns="1" AutoPostBack="True"
                                                                                    OnSelectedIndexChanged="chkCompany_SelectedIndexChanged" Font-Names="Trebuchet MS"
                                                                                    Font-Size="Small" ForeColor="Gray" />
                                                                            </asp:Panel>
                                                                        </div>
                                                                        <div class="col-xs-6">
                                                                            <asp:Label ID="Label7" runat="server" Font-Bold="True" Font-Names="arial" Font-Size="13px"
                                                                                ForeColor="#666666" Text="<%$ Resources:Attendance,Brand %>"></asp:Label>
                                                                            <asp:Panel ID="pnlBrand" runat="server" Height="200px" BorderStyle="Solid" BorderWidth="1px"
                                                                                BorderColor="#ABADB3" BackColor="White" ScrollBars="Auto">
                                                                                <asp:CheckBoxList ID="chkBrand" runat="server" RepeatColumns="1" AutoPostBack="True"
                                                                                    Font-Names="Trebuchet MS" Font-Size="Small" ForeColor="Gray" />
                                                                            </asp:Panel>
                                                                        </div>
                                                                    </div>
                                                                </div>
                                                            </ContentTemplate>
                                                        </cc1:TabPanel>

                                                    </cc1:TabContainer>
                                                    <div class="row">
                                                        <div class="col-md-12" style="text-align: center;">
                                                            <br />
                                                            <asp:Button ID="btnGroupNext" ValidationGroup="Group_Save" runat="server" CssClass="btn btn-success" Text="<%$ Resources:Attendance,Save %>"
                                                                OnClick="btnGroupNext_Click" Visible="false" />
                                                            <asp:Button ID="btnGCancel" runat="server" CausesValidation="False" CssClass="btn btn-danger"
                                                                Text="<%$ Resources:Attendance,Cancel %>" OnClick="btnCancel_Click" />
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                        </ContentTemplate>
                                        <Triggers>
                                            <asp:AsyncPostBackTrigger ControlID="Btn_New" EventName="Click" />
                                        </Triggers>
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

    var dataa = '~/' + s.rootFolderName + subproduct + '/' + e.file.name;
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
                            </div>
                        </div>
                    </div>

                    <div class="tab-pane" id="Bin">
                        <asp:UpdatePanel ID="Update_Bin" runat="server" UpdateMode="Conditional">
                            <ContentTemplate>
                                <div class="row">
                                    <div class="col-md-12">


                                        <div class="row">
                                            <div class="col-md-12">
                                                <div id="Div3" runat="server" class="box box-info collapsed-box">
                                                    <div class="box-header with-border">
                                                        <h3 class="box-title">
                                                            <asp:Label ID="Label12" runat="server" Text="Advance Search"></asp:Label></h3>
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
                                                                <asp:ListItem Text="By Company" Value="C_Name" Selected="True"></asp:ListItem>
                                                                <asp:ListItem Text="<%$ Resources:Attendance,Contact ID %>" Value="Code"></asp:ListItem>
                                                                <asp:ListItem Text="<%$ Resources:Attendance,Name %>" Value="Name"></asp:ListItem>
                                                                <asp:ListItem Text="<%$ Resources:Attendance,Contact Type %>" Value="Status"></asp:ListItem>
                                                                <asp:ListItem Text="<%$ Resources:Attendance,Created By %>" Value="CreatedUser"></asp:ListItem>
                                                                <asp:ListItem Text="<%$ Resources:Attendance,Modified By %>" Value="ModifiedUser"></asp:ListItem>
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
                                                                <asp:TextBox ID="txtValueBin" class="form-control" runat="server" placeholder="Search From Content"></asp:TextBox>
                                                            </asp:Panel>

                                                        </div>
                                                        <div class="col-lg-2" style="text-align: center;">
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
                                                            <asp:HiddenField ID="HdnSortBin" runat="server" />
                                                            <asp:GridView CssClass="table-striped table-bordered table table-hover" ID="GvContactBin" runat="server" AllowPaging="True" PageSize="<%# PageControlCommon.GetPageSize() %>"
                                                                AutoGenerateColumns="False" Width="100%" AllowSorting="true" OnSorting="GvContactBin_Sorting"
                                                                OnPageIndexChanging="GvContactBin_PageIndexChanging">

                                                                <Columns>
                                                                    <asp:TemplateField>
                                                                        <HeaderTemplate>
                                                                            <asp:CheckBox ID="chkActiveAll" runat="server" OnCheckedChanged="chkActiveAll_CheckedChanged"
                                                                                AutoPostBack="true" />
                                                                        </HeaderTemplate>
                                                                        <ItemTemplate>
                                                                            <asp:CheckBox ID="chkActive" runat="server" OnCheckedChanged="chkActive_CheckedChanged"
                                                                                AutoPostBack="true" />
                                                                        </ItemTemplate>
                                                                        <ItemStyle HorizontalAlign="Center" />
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="<%$ Resources:Attendance,Contact Id %>" SortExpression="Code">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblgvContactCode" runat="server" Text='<%#Eval("Code") %>' />
                                                                        </ItemTemplate>
                                                                        <ItemStyle HorizontalAlign="Center" />
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="<%$ Resources:Attendance,Name %>" SortExpression="Name">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblPrefixBin" runat="server" Text='<%#Eval("Field3") %>' />
                                                                            <asp:Label ID="lblgvName" runat="server" Text='<%# Eval("Name") %>'></asp:Label>
                                                                            <asp:Label ID="Label5" runat="server" Visible="false" Text='<%# Eval("Trans_Id") %>'></asp:Label>
                                                                        </ItemTemplate>
                                                                        <ItemStyle HorizontalAlign="Left" />
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="<%$ Resources:Attendance,Name (Local)%>" SortExpression="Name_L">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblgvCNameL" runat="server" Text='<%#Eval("Name_L") %>' />
                                                                        </ItemTemplate>
                                                                        <ItemStyle HorizontalAlign="Left" />
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="<%$ Resources:Attendance,Contact Type %>" SortExpression="Status">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblgvStatus" runat="server" Text='<%#Eval("Status") %>'></asp:Label>
                                                                        </ItemTemplate>
                                                                        <ItemStyle HorizontalAlign="Left" />
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="<%$ Resources:Attendance,Created By %>" SortExpression="CreatedUser">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblgvUser" runat="server" Text='<%#Eval("CreatedUser") %>' />
                                                                        </ItemTemplate>
                                                                        <ItemStyle HorizontalAlign="Left" Width="10%" />
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="<%$ Resources:Attendance,Modified By %>" SortExpression="ModifiedUser">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblModified_User" runat="server" Text='<%# Eval("ModifiedUser") %>'></asp:Label>
                                                                        </ItemTemplate>
                                                                        <ItemStyle HorizontalAlign="Left" Width="10%" />
                                                                    </asp:TemplateField>
                                                                </Columns>

                                                                <PagerStyle CssClass="pagination-ys" />

                                                            </asp:GridView>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </ContentTemplate>
                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="Btn_Bin" EventName="Click" />
                            </Triggers>
                        </asp:UpdatePanel>
                    </div>

                    <div class="tab-pane" id="Upload">
                        <asp:UpdatePanel ID="Update_Upload" runat="server" UpdateMode="Conditional">
                            <ContentTemplate>
                                <div class="row">
                                    <div class="col-md-12">
                                        <div class="box box-primary">
                                            <div class="box-body">
                                                <div class="form-group">
                                                    <div class="col-md-12">
                                                        <asp:HyperLink ID="Lnk_Demo_Contact_Upload" runat="server" NavigateUrl="~/CompanyResource/Contact.xlsx" Text="Sample Excel File For Contact Upload"></asp:HyperLink>
                                                        <br />
                                                        <br />
                                                    </div>
                                                    <div class="col-md-12">
                                                        <div class="col-md-6">
                                                            <asp:Label ID="Lbl_Browse_Excel" runat="server" Text="Browse Excel File"></asp:Label>
                                                            <div class="input-group" style="width: 100%;">
                                                                <cc1:AsyncFileUpload ID="FU_Contact_Upload"
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
                                                                    <asp:Image ID="FUExcel_Img_Right" runat="server" Width="30px" Height="30px" ImageUrl="../Images/Allow.png" Style="display: none" />
                                                                    <asp:Image ID="FUExcel_Img_Wrong" runat="server" Width="30px" Height="30px" ImageUrl="../Images/Delete1.png" Style="display: none" />
                                                                    <asp:Image ID="FUExcel_ImgLoader" runat="server" ImageUrl="../Images/loader.gif" />
                                                                </div>
                                                            </div>
                                                            <br />
                                                        </div>
                                                        <div class="col-md-6">
                                                            <br />
                                                            <asp:Button ID="btnGetSheet" CssClass="btn btn-primary" runat="server" CausesValidation="False"
                                                                OnClick="btnGetSheet_Click" Visible="true" Text="<%$ Resources:Attendance,Connect To DataBase %>" />
                                                            <br />
                                                        </div>
                                                    </div>
                                                    <div class="col-md-12">
                                                        <div class="col-md-6">
                                                            <asp:Label runat="server" Text="Select Sheet" ID="Label2"></asp:Label>
                                                            <asp:DropDownList ID="DDl_Excel_Sheet" CssClass="form-control" runat="server">
                                                            </asp:DropDownList>
                                                            <br />
                                                        </div>
                                                        <div class="col-md-6">
                                                            <br />
                                                            <asp:Button ID="btnConnect" CssClass="btn btn-primary" runat="server" CausesValidation="False"
                                                                OnClick="btnConnect_Click" Visible="true" Text="GetRecord" />
                                                            <br />
                                                        </div>
                                                    </div>
                                                    <div id="Div_Upload_Grid" runat="server" visible="false">
                                                        <div class="row">
                                                            <div class="col-md-12">
                                                                <div class="box box-primary">
                                                                    <div class="box-body">
                                                                        <div class="form-group">
                                                                            <div class="col-md-12" style="text-align: center">
                                                                                <asp:RadioButton ID="Rbt_All" Style="margin-left: 20px; margin-right: 20px;" runat="server" GroupName="upd" AutoPostBack="true" Checked="true" OnCheckedChanged="Rbt_All_CheckedChanged" Text="All" />
                                                                                <asp:RadioButton ID="Rbt_Valid" Style="margin-left: 20px; margin-right: 20px;" runat="server" GroupName="upd" AutoPostBack="true" Text="Valid" OnCheckedChanged="Rbt_All_CheckedChanged" />
                                                                                <asp:RadioButton ID="Rbt_Invalid" Style="margin-left: 20px; margin-right: 20px;" runat="server" GroupName="upd" AutoPostBack="true" Text="Invalid" OnCheckedChanged="Rbt_All_CheckedChanged" />
                                                                            </div>
                                                                            <div class="col-md-12" style="text-align: right;">
                                                                                <asp:Label ID="lbltotaluploadRecord" runat="server"></asp:Label>
                                                                            </div>
                                                                            <div class="col-md-12" style="max-height: 500px; overflow: auto;">
                                                                                <asp:GridView CssClass="table-striped table-bordered table table-hover" ID="GV_Sheet_Upload" runat="server" Width="100%">

                                                                                    <HeaderStyle CssClass="Invgridheader" Width="300px" Wrap="false" BorderStyle="Solid" BorderColor="#6E6E6E" BorderWidth="1px" />
                                                                                    <PagerStyle CssClass="pagination-ys" />

                                                                                </asp:GridView>
                                                                                <br />
                                                                            </div>
                                                                            <div class="col-md-12">
                                                                                <br />
                                                                            </div>
                                                                            <div class="col-md-12" style="text-align: center">
                                                                                <asp:Button ID="Btn_Upload_Sheet" runat="server" CssClass="btn btn-primary" OnClick="Btn_Upload_Sheet_Click"
                                                                                    Text="<%$ Resources:Attendance,Upload Data %>" />
                                                                                <cc1:ConfirmButtonExtender ID="ConfirmButtonExtender1" runat="server" TargetControlID="Btn_Upload_Sheet"
                                                                                    ConfirmText="Are you sure to Save Records in Database.">
                                                                                </cc1:ConfirmButtonExtender>

                                                                                <asp:Button ID="btnBackToMapData" CssClass="btn btn-primary" runat="server" Text="<%$ Resources:Attendance,Back To FileUpload %>"
                                                                                    OnClick="btnBackToMapData_Click" />
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
                                    </div>
                                </div>
                            </ContentTemplate>
                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="Btn_Upload" EventName="Click" />
                            </Triggers>
                        </asp:UpdatePanel>

                    </div>

                    <div class="tab-pane" id="Verify_Request">
                        <asp:UpdatePanel ID="Update_Verify_Request" runat="server" UpdateMode="Conditional">
                            <ContentTemplate>

                                <div class="row">
                                    <div class="col-md-12">
                                        <div id="Div2" runat="server" class="box box-info collapsed-box">
                                            <div class="box-header with-border">
                                                <h3 class="box-title">
                                                    <asp:Label ID="Label13" runat="server" Text="Advance Search"></asp:Label></h3>
                                                &nbsp;&nbsp;|&nbsp;&nbsp;
                                             <asp:Label ID="lblVerifyTotalRecords" Text="<%$ Resources:Attendance,Total Records: 0 %>" runat="server"></asp:Label>

                                                <div class="box-tools pull-right">
                                                    <button type="button" class="btn btn-box-tool" data-widget="collapse">
                                                        <i id="I3" runat="server" class="fa fa-plus"></i>
                                                    </button>
                                                </div>
                                            </div>
                                            <div class="box-body">

                                                <div class="col-lg-3">
                                                    <asp:DropDownList ID="ddlVerifyProductFieldName" runat="server" class="form-control">
                                                        <asp:ListItem Text="<%$ Resources:Attendance,Contact Type %>" Value="Status"></asp:ListItem>
                                                        <asp:ListItem Text="<%$ Resources:Attendance,Contact Id %>" Value="Code"></asp:ListItem>
                                                        <asp:ListItem Text="<%$ Resources:Attendance,Name %>" Value="ContactName" Selected="True"></asp:ListItem>
                                                        <asp:ListItem Text="<%$ Resources:Attendance,Mobile No %>" Value="Field2"></asp:ListItem>
                                                        <asp:ListItem Text="<%$ Resources:Attendance,Email Id %>" Value="Field1"></asp:ListItem>
                                                        <asp:ListItem Text="Contact Company Name" Value="Contact_Company_Name"></asp:ListItem>
                                                        <asp:ListItem Text="<%$ Resources:Attendance,Created By %>" Value="CreatedEmpName"></asp:ListItem>
                                                    </asp:DropDownList>
                                                </div>
                                                <div class="col-lg-2">
                                                    <asp:DropDownList ID="ddlVerifyProductOption" runat="server" class="form-control">
                                                        <asp:ListItem Text="<%$ Resources:Attendance,--Select-- %>"></asp:ListItem>
                                                        <asp:ListItem Text="<%$ Resources:Attendance,Equal %>"></asp:ListItem>
                                                        <asp:ListItem Selected="True" Text="<%$ Resources:Attendance,Contains %>"></asp:ListItem>
                                                        <asp:ListItem Text="<%$ Resources:Attendance,Like %>"></asp:ListItem>
                                                    </asp:DropDownList>
                                                </div>
                                                <div class="col-lg-5">
                                                    <asp:Panel ID="Panel3" runat="server" DefaultButton="imgSearchVeryProduct">
                                                        <asp:TextBox ID="txtVerifyProduct" runat="server" class="form-control"></asp:TextBox>
                                                    </asp:Panel>
                                                </div>
                                                <div class="col-lg-2">
                                                    <asp:LinkButton ID="imgSearchVeryProduct" runat="server" CausesValidation="False" OnClientClick="tab_3_open()" OnClick="imgSearchVeryProduct_Click" ToolTip="<%$ Resources:Attendance,Search %>"><span class="fa fa-search"  style="font-size:25px;"></span></asp:LinkButton>&nbsp;&nbsp;&nbsp;
                                                    <asp:LinkButton ID="imgSearchVeryRefresh" runat="server" CausesValidation="False" OnClientClick="tab_3_open()" OnClick="imgSearchVeryRefresh_Click" ToolTip="<%$ Resources:Attendance,Refresh %>"><span class="fa fa-repeat"  style="font-size:25px;"></span></asp:LinkButton>
                                                </div>

                                                <div class="col-md-6">
                                                    <asp:Button ID="btnVerifyProduct" CssClass="btn btn-primary" runat="server" CausesValidation="False"
                                                        OnClick="btnVerifyProduct_Click" Text="<%$ Resources:Attendance,Verify %>" />

                                                    <asp:Button ID="btnSelectRecord" CssClass="btn btn-primary" runat="server" CausesValidation="False"
                                                        OnClick="btnSelectRecord_Click" Text="Select All" />
                                                    <br />
                                                </div>
                                                <div class="col-md-6">

                                                    <br />
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
                                                    <asp:GridView CssClass="table-striped table-bordered table table-hover" ID="gvVerifyProduct" PageSize="<%# PageControlCommon.GetPageSize() %>"
                                                        DataKeyNames="Trans_Id" runat="server" AutoGenerateColumns="False" Width="100%"
                                                        AllowPaging="True" AllowSorting="True" OnPageIndexChanging="gvVerifyProduct_PageIndexChanging"
                                                        OnSorting="gvVerifyProduct_Sorting">
                                                        <Columns>
                                                            <asp:TemplateField>
                                                                <HeaderTemplate>
                                                                    <asp:CheckBox ID="chkgvSelectAll" runat="server" AutoPostBack="true" OnCheckedChanged="chkgvVerifySelectAll_CheckedChanged" />
                                                                </HeaderTemplate>
                                                                <ItemTemplate>
                                                                    <asp:CheckBox ID="chkgvSelect" runat="server" />
                                                                </ItemTemplate>
                                                                <ItemStyle HorizontalAlign="Center" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,Contact Type %>" SortExpression="Status">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblgvStatus" runat="server" Text='<%#Eval("Status") %>'></asp:Label>
                                                                </ItemTemplate>
                                                                <ItemStyle HorizontalAlign="Center" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,Contact Id %>" SortExpression="Code">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblgvContactCode" runat="server" Text='<%#Eval("Code") %>' />
                                                                </ItemTemplate>
                                                                <ItemStyle HorizontalAlign="Center" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,Name %>" SortExpression="Name">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblgvName" runat="server" Text='<%# Eval("ContactName") %>'></asp:Label>
                                                                </ItemTemplate>
                                                                <ItemStyle HorizontalAlign="Center" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,Mobile No %>" SortExpression="Field2">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblgvmobileNo" runat="server" Text='<%# Eval("Field2") %>'></asp:Label>
                                                                </ItemTemplate>
                                                                <ItemStyle />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,Email Id %>" SortExpression="Field1">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblgvEmailId" runat="server" Text='<%#Eval("Field1") %>'></asp:Label>
                                                                </ItemTemplate>
                                                                <ItemStyle HorizontalAlign="Center" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Contact Company Name" SortExpression="Contact_Company_Name">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblgvcontactCompanyName" runat="server" Text='<%#Eval("Contact_Company_Name") %>'></asp:Label>
                                                                </ItemTemplate>
                                                                <ItemStyle HorizontalAlign="Center" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Created By" SortExpression="CreatedEmployee">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblgvcreatedBy" runat="server" Text='<%#Eval("CreatedEmployee") %>'></asp:Label>
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
                                </div>

                            </ContentTemplate>
                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="Btn_Verify" EventName="Click" />
                            </Triggers>
                        </asp:UpdatePanel>
                    </div>
                </div>
            </div>
        </div>
    </div>

    <div class="modal fade" id="Address_Modal" tabindex="-1" role="dialog" aria-labelledby="Address_ModalLabel"
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

    <div class="modal fade" id="Bank_Modal" tabindex="-1" role="dialog" aria-labelledby="Bank_ModalLabel"
        aria-hidden="true">
        <div class="modal-dialog modal-lg">
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal">
                        <span aria-hidden="true">&times;</span><span class="sr-only">Close</span></button>
                    <h4 class="modal-title" id="Bank_ModalLabel">Add Bank Details</h4>
                </div>
                <div class="modal-body">
                    <div class="row">
                        <asp:UpdatePanel ID="Update_Bank_Modal" runat="server">
                            <ContentTemplate>
                                <div class="row">
                                    <div class="col-md-12">
                                        <div class="col-md-6">
                                            <asp:Label ID="Label21" runat="server" Text="<%$ Resources:Attendance,Bank Name %>" />
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
                                            <asp:Label ID="Label22" runat="server" Text="<%$ Resources:Attendance,Account No %>" />
                                            <asp:TextBox ID="txtCBAccountNo" runat="server" CssClass="form-control" />
                                            <br />
                                        </div>
                                    </div>

                                    <div class="col-md-12">
                                        <div class="col-md-12">
                                            <asp:Label ID="Label23" runat="server" Text="<%$ Resources:Attendance,Branch Address %>" />
                                            <asp:TextBox ID="txtCBBrachAddress" TextMode="MultiLine" runat="server"
                                                CssClass="form-control" />
                                            <br />
                                        </div>
                                    </div>

                                    <div class="col-md-12">
                                        <div class="col-md-6">
                                            <asp:Label ID="Label24" runat="server" Text="<%$ Resources:Attendance,IFSC Code %>" />
                                            <asp:TextBox ID="txtCBIFSCCode" runat="server" CssClass="form-control" />
                                        </div>
                                        <div class="col-md-6">
                                            <asp:Label ID="Label25" runat="server" Text="<%$ Resources:Attendance,Branch Code %>" />
                                            <asp:TextBox ID="txtCBBranchCode" runat="server" CssClass="form-control" />
                                        </div>
                                    </div>

                                    <div class="col-md-12">
                                        <div class="col-md-6">
                                            <asp:Label ID="Label2815" runat="server" Text="<%$ Resources:Attendance,MICR Code %>" />
                                            <asp:TextBox ID="txtCBMICRCode" runat="server" CssClass="form-control" />
                                        </div>
                                        <div class="col-md-6">
                                            <asp:Label ID="lblIBanNum" runat="server" Text="<%$ Resources:Attendance,IBAN NUMBER %>" />
                                            <asp:TextBox ID="txtIBANNUMBER" runat="server" CssClass="form-control" />
                                        </div>
                                    </div>
                                </div>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </div>
                </div>
                <div class="modal-footer">
                    <asp:UpdatePanel ID="Update_Bank_Button" runat="server">
                        <ContentTemplate>
                            <asp:Button ID="BtnCBSave" runat="server" Text="<%$ Resources:Attendance,Save %>"
                                CssClass="btn btn-success" OnClick="btnCBSave_Click" />
                            <asp:Button ID="BtnCBCancel" runat="server" Text="<%$ Resources:Attendance,Reset %>"
                                CssClass="btn btn-primary" OnClick="BtnCBCancel_Click" />
                            <asp:HiddenField ID="hdnContactBankId" runat="server" />

                            <button type="button" class="btn btn-danger" data-dismiss="modal">
                                Close</button>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
            </div>
        </div>
    </div>

  


    <asp:UpdateProgress ID="UpdateProgress10" runat="server" AssociatedUpdatePanelID="Update_Bank_Modal">
        <ProgressTemplate>
            <div class="modal_Progress">
                <div class="center_Progress">
                    <img alt="" src="../Images/ajax-loader2.gif" style="vertical-align: middle; background-color: White; border-radius: 10px;" />
                </div>
            </div>
        </ProgressTemplate>
    </asp:UpdateProgress>

    <asp:UpdateProgress ID="UpdateProgress11" runat="server" AssociatedUpdatePanelID="Update_Bank_Button">
        <ProgressTemplate>
            <div class="modal_Progress">
                <div class="center_Progress">
                    <img alt="" src="../Images/ajax-loader2.gif" style="vertical-align: middle; background-color: White; border-radius: 10px;" />
                </div>
            </div>
        </ProgressTemplate>
    </asp:UpdateProgress>

    <asp:UpdateProgress ID="UpdateProgress3" runat="server" AssociatedUpdatePanelID="Update_Button">
        <ProgressTemplate>
            <div class="modal_Progress">
                <div class="center_Progress">
                    <img alt="" src="../Images/ajax-loader2.gif" style="vertical-align: middle; background-color: White; border-radius: 10px;" />
                </div>
            </div>
        </ProgressTemplate>
    </asp:UpdateProgress>

    <asp:UpdateProgress ID="UpdateProgress5" runat="server" AssociatedUpdatePanelID="Update_List">
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
                    <img alt="" src="../Images/ajax-loader2.gif" style="vertical-align: middle; background-color: White; border-radius: 10px;" />
                </div>
            </div>
        </ProgressTemplate>
    </asp:UpdateProgress>

    <%--<asp:UpdateProgress ID="UpdateProgress2" runat="server" AssociatedUpdatePanelID="Update_Group">
        <ProgressTemplate>
            <div class="modal_Progress">
                <div class="center_Progress">
                    <img alt="" src="../Images/ajax-loader2.gif" style="vertical-align: middle; background-color: White; border-radius: 10px;" />
                </div>
            </div>
        </ProgressTemplate>
    </asp:UpdateProgress>--%>

    <asp:UpdateProgress ID="UpdateProgress6" runat="server" AssociatedUpdatePanelID="Update_Bin">
        <ProgressTemplate>
            <div class="modal_Progress">
                <div class="center_Progress">
                    <img alt="" src="../Images/ajax-loader2.gif" style="vertical-align: middle; background-color: White; border-radius: 10px;" />
                </div>
            </div>
        </ProgressTemplate>
    </asp:UpdateProgress>

    <asp:UpdateProgress ID="UpdateProgress4" runat="server" AssociatedUpdatePanelID="Update_Upload">
        <ProgressTemplate>
            <div class="modal_Progress">
                <div class="center_Progress">
                    <img alt="" src="../Images/ajax-loader2.gif" style="vertical-align: middle; background-color: White; border-radius: 10px;" />
                </div>
            </div>
        </ProgressTemplate>
    </asp:UpdateProgress>

    <asp:UpdateProgress ID="UpdateProgress1" runat="server" AssociatedUpdatePanelID="Update_Verify_Request">
        <ProgressTemplate>
            <div class="modal_Progress">
                <div class="center_Progress">
                    <img alt="" src="../Images/ajax-loader2.gif" style="vertical-align: middle; background-color: White; border-radius: 10px;" />
                </div>
            </div>
        </ProgressTemplate>
    </asp:UpdateProgress>



    <asp:Panel ID="pnlUpload" runat="server" DefaultButton="btnbind" Visible="false">
        <div width="100%" style="background-image: url(../Images/bg_repeat.jpg); background-repeat: repeat;">
            <table width="100%" style="padding-left: 20px; height: 38px">
                <tr>
                    <td width="90px">
                        <asp:Label ID="lblSelectField" runat="server" Text="<%$ Resources:Attendance,Select Field %>"></asp:Label>
                    </td>
                    <td width="180px">
                        <asp:DropDownList ID="ddlFieldName" runat="server" CssClass="form-control"
                            Width="170px">
                            <asp:ListItem Text="By Company" Value="CompanyName" Selected="True"></asp:ListItem>
                            <asp:ListItem Text="<%$ Resources:Attendance,Contact Type %>" Value="Status"></asp:ListItem>
                            <asp:ListItem Text="<%$ Resources:Attendance,Contact ID %>" Value="Code"></asp:ListItem>
                            <asp:ListItem Text="<%$ Resources:Attendance,Name %>" Value="Name"></asp:ListItem>
                            <asp:ListItem Text="<%$ Resources:Attendance,Mobile No %>" Value="Field2"></asp:ListItem>
                            <asp:ListItem Text="<%$ Resources:Attendance,EmailId %>" Value="Field1"></asp:ListItem>
                        </asp:DropDownList>
                    </td>
                    <td width="135px">
                        <asp:DropDownList ID="ddlOption" runat="server" CssClass="form-control"
                            Width="120px">
                            <asp:ListItem Text="<%$ Resources:Attendance,--Select-- %>"></asp:ListItem>
                            <asp:ListItem Text="<%$ Resources:Attendance,Equal %>"></asp:ListItem>
                            <asp:ListItem Selected="True" Text="<%$ Resources:Attendance,Contains %>"></asp:ListItem>
                            <asp:ListItem Text="<%$ Resources:Attendance,Like %>"></asp:ListItem>
                        </asp:DropDownList>
                    </td>
                    <td width="24%">
                        <asp:Panel ID="Pnl_TxtValue" runat="server" DefaultButton="btnbind">
                            <asp:TextBox ID="txtValue" runat="server" CssClass="form-control"></asp:TextBox>
                        </asp:Panel>
                    </td>
                    <td width="50px" align="center">
                        <asp:ImageButton ID="btnbind" runat="server" CausesValidation="False"
                            ImageUrl="~/Images/search.png" OnClick="btnbind_Click" ToolTip="<%$ Resources:Attendance,Search %>"></asp:ImageButton>
                    </td>
                    <td>
                        <asp:Panel ID="PnlRefresh" runat="server" DefaultButton="btnRefresh">
                            <asp:ImageButton ID="btnRefresh" runat="server" CausesValidation="False"
                                ImageUrl="~/Images/refresh.png" OnClick="btnRefresh_Click" ToolTip="<%$ Resources:Attendance,Refresh %>"></asp:ImageButton>
                        </asp:Panel>
                    </td>
                    <td align="center">
                        <asp:Label ID="lblTotalRecords" runat="server" Text="<%$ Resources:Attendance,Total Records: 0 %>"></asp:Label>
                        <asp:Label ID="lblSelectedRecord" runat="server" Visible="false"></asp:Label>
                    </td>
                </tr>
            </table>
        </div>
    </asp:Panel>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="FooterContent" runat="Server">
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
<asp:Content ID="Content5" ContentPlaceHolderID="AfterLoadScriptContent" runat="Server">
    <script src="../Script/common.js"></script>
    <script type="text/javascript">

        function EditAddress(index) {

            var GridView1 = document.getElementById('<%= GvAddressName.ClientID %>');

            var $getRecQty = $("span[id*=lblgvAddressName]")

            document.getElementById('<%= txtAddressName.ClientID %>').Value = $getRecQty[index].innerHTML;

        }

    </script>
    <script type="text/javascript">

        function postBackByObject() {
            var o = window.event.srcElement;
            if (o.tagName == "INPUT" && o.type == "checkbox") {
                __doPostBack("<%= navTree.ClientID%>", "");
            }
        }
    </script>
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

        function checkLoad() {


            window.onunload = refreshParent;
            function refreshParent() {
                window.opener.location.reload();
            }

        }
    </script>
    <script>

        function Validate_nav_tree(sender, args) {
            var gridView = document.getElementById("<%=navTree.ClientID %>");
            var checkBoxes = gridView.getElementsByTagName("input");
            for (var i = 0; i < checkBoxes.length; i++) {
                if (checkBoxes[i].type == "checkbox" && checkBoxes[i].checked) {
                    args.IsValid = true;
                    return;
                }
            }
            args.IsValid = false;
        }

        function Mail_Group_Check(sender, args) {
            var My_radio_list = document.getElementById("<%=rbnEmailList.ClientID %>")
            alert(My_radio_list.Visible);
            if (My_radio_list.Visible == true) {
                args.IsValid = true;
            }
            else {
                args.IsValid = false;
            }
        }

        function Mail_Group(sender, args) {
            var MyRadio = document.getElementById("<%=rbnEmailList.ClientID%>")
            var options = MyRadio.getElementsByTagName("input")
            var somethingChecked = false;
            var s;
            for (x = 0; x < options.length; ++x) {
                if (options[x].checked) {
                    somethingChecked = true;
                    s = options[x].value;

                }
            }
            if (!somethingChecked) {

                args.IsValid = false;
            }
            else {

                args.IsValid = true;
                alert(s);
            }
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
        function Li_Tab_New() {
            document.getElementById('<%= Btn_New.ClientID %>').click();

            setTimeout(function () { jQuery('#<%=txtId.ClientID%>').focus() }, 500);
        }
        function Li_Tab_Bin() {
            document.getElementById('<%= Btn_Bin.ClientID %>').click();
        }

        function Li_Tab_Upload() {
            document.getElementById('<%= Btn_Upload.ClientID %>').click();
        }

        function Li_Tab_Verify_Request() {
            document.getElementById('<%= Btn_Verify.ClientID %>').click();
        }

        function Modal_Bank_Details() {
            document.getElementById('<%= Btn_Add_Bank_Details.ClientID %>').click();
        }

        function LI_New_Active() {
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
        function Modal_Address_Open() {
            document.getElementById('<%= Btn_Add_Address_Details.ClientID %>').click();
        }
        function Modal_Address_Close() {
            debugger;
            document.getElementById('<%= Btn_Add_Address_Details.ClientID %>').click();
            document.getElementById('<%= imgAddAddressName.ClientID %>').click();
        }

        function Modal_Number_Open(data) {
            document.getElementById('AllNumberData').innerHTML = data;
            document.getElementById('<%= Btn_Number.ClientID %>').click();
        }

        function Box_Additional_Div() {
            $("#Btn_Additional_Div").removeClass("fa fa-plus");
            $("#Div_Box_Additional").removeClass("box box-primary collapsed-box");

            $("#Btn_Additional_Div").addClass("fa fa-minus");
            $("#Div_Box_Additional").addClass("box box-primary");

            document.getElementById('<%= Btn_Add_Bank_Details.ClientID %>').click();
        }

        function postBackByObject() {
            var o = window.event.srcElement;
            if (o.tagName == "INPUT" && o.type == "checkbox") {
                __doPostBack("<%= navTree.ClientID%>", "");
            }
        }
    </script>
    <script type="text/javascript">
        function FuLogouploadStarted(sender, args) {
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
        function FuLogoUploadError(sender, args) {
            document.getElementById('<%= Img_Right.ClientID %>').style.display = "none";
            document.getElementById('<%= Img_Wrong.ClientID %>').style.display = "";
            var img = document.getElementById('<%= imgLogo.ClientID %>');
            img.src = "../Bootstrap_Files/dist/img/NoImage.jpg";
            alert('Invalid File Type, Select Only .png, .jpg, .jpge extension file');
        }

        function FuLogouploadComplete(sender, args) {
            document.getElementById('<%= Img_Wrong.ClientID %>').style.display = "none";
            document.getElementById('<%= Img_Right.ClientID %>').style.display = "";

            var img = document.getElementById('<%= imgLogo.ClientID %>');
            img.src = "<%=ResolveUrl(FuLogoUploadFolderPath) %>" + args.get_fileName();
        }




        function UploadFile_Upload_Started(sender, args) {

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

        function validation1() {
            var valIsCheck1 = document.getElementById('<%= ContactNo1.FindControl("ddlContactType").ClientID %>').value;

            if (valIsCheck1 == "LandLine") {
                document.getElementById('<%=ContactNo1.FindControl("txtExtensionNumber").ClientID%>').disabled = false;
            }
            else {
                document.getElementById('<%=ContactNo1.FindControl("txtExtensionNumber").ClientID%>').disabled = true;
                document.getElementById('<%=ContactNo1.FindControl("txtExtensionNumber").ClientID%>').value = "";
            }

            var valIsCheck = document.getElementById('<%= addaddress.FindControl("ContactNo").FindControl("ddlContactType").ClientID %>').value;

            if (valIsCheck == "LandLine") {
                document.getElementById('<%=addaddress.FindControl("ContactNo").FindControl("txtExtensionNumber").ClientID%>').disabled = false;
            }
            else {
                document.getElementById('<%=addaddress.FindControl("ContactNo").FindControl("txtExtensionNumber").ClientID%>').disabled = true;
                document.getElementById('<%=addaddress.FindControl("ContactNo").FindControl("txtExtensionNumber").ClientID%>').value = "";
            }
        }

        function alertmessage(data) {
            alert(data);

        }

        function txtAddressName_TextChanged() {
            debugger;
            var addressName = document.getElementById('<%= txtAddressName.ClientID %>');
            if (addressName.value.trim() == "") {
                return;
            }
            PageMethods.txtAddressNameNew_TextChanged(addressName.value, onSuccess_txtAddressName)
        }
        function onSuccess_txtAddressName(data) {
            if (data == 0) {
                alert("Select From Suggestions Only");
                document.getElementById('<%= txtAddressName.ClientID %>').value = "";
            }
        }
        function showUcControlsSettings() {
            $('#ControlSettingModal').modal('show');
        }
    </script>
    <script src="../Script/master.js"></script>

</asp:Content>
