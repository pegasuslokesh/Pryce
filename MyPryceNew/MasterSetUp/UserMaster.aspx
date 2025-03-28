<%@ Page Title="" Language="C#" MasterPageFile="~/ERPMaster.master" AutoEventWireup="true"
    CodeFile="UserMaster.aspx.cs" Inherits="MasterSetUp_UserMaster" %>

<%@ Register Src="~/WebUserControl/TimeManLicense.ascx" TagPrefix="uc1" TagName="UpdateLicense" %>
<%@ Register Src="~/WebUserControl/EmailConfiguration.ascx" TagName="EmailConfig" TagPrefix="uc" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="HeaderContent" runat="Server">
    <h1>
        <i class="fas fa-users-cog"></i>&nbsp;&nbsp;
        <asp:Label ID="lblHeader" runat="server" Text="<%$ Resources:Attendance,User Setup%>"></asp:Label>
    </h1>
    <ol class="breadcrumb">
        <li><a href="#"><i class="fa fa-dashboard"></i>
            <asp:Label ID="Label6" runat="server" Text="<%$ Resources:Attendance,IT Security%>"></asp:Label></a></li>
        <li><a href="#"><i class="fa fa-dashboard"></i>
            <asp:Label ID="Label10" runat="server" Text="<%$ Resources:Attendance,IT Security%>"></asp:Label></a></li>
        <li class="active">
            <asp:Label ID="Label5" runat="server" Text="<%$ Resources:Attendance,User%>"></asp:Label></li>
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



        function new_validation1() {
            var val = document.getElementById("<%= chkdepartment.ClientID  %>").checked;
            if (val) {
                $('[id*=TreeViewDepartment] input[type=checkbox]').prop('checked', true);
            }
            else {
                $('[id*=TreeViewDepartment] input[type=checkbox]').prop('checked', false);
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
            if (!check) {
                return;
            }
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
                    checkUncheckSwitch = false;
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
            <asp:Button ID="Btn_Modal_Popup" Style="display: none;" data-toggle="modal" data-target="#myModal" runat="server" Text="Modal Popup" />
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
    <div id="myTab" class="nav-tabs-custom">
        <ul class="nav nav-tabs pull-right bg-blue-gradient">
            <li id="Li_Search"><a href="#Search" data-toggle="tab">
                <i class="fa fa-search"></i>&nbsp;&nbsp;
                <asp:Label ID="Label2" runat="server" Text="<%$ Resources:Attendance,Search %>"></asp:Label></a></li>
            <li id="Li_Bin"><a href="#Bin" data-toggle="tab">
                <i class="fa fa-trash"></i>&nbsp;&nbsp;
                <asp:Label ID="Label13" runat="server" Text="<%$ Resources:Attendance,Bin %>"></asp:Label></a></li>
            <li id="Li_New"><a href="#New" data-toggle="tab">
                <asp:UpdatePanel ID="Update_Li" runat="server">
                    <ContentTemplate>
                        <i class="fa fa-file"></i>&nbsp;&nbsp;
                        <asp:Label ID="Lbl_New_tab" runat="server" Text="<%$ Resources:Attendance,New%>"></asp:Label>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </a></li>
            <li class="active" id="Li_List"><a href="#List" data-toggle="tab">
                <i class="fa fa-list"></i>&nbsp;&nbsp;
                <asp:Label ID="Label1" runat="server" Text="<%$ Resources:Attendance,List %>"></asp:Label></a></li>
        </ul>
        <asp:HiddenField runat="server" ID="hdnCanEdit" />
        <asp:HiddenField runat="server" ID="hdnCanDelete" />

        <div class="tab-content">
            <div class="tab-pane active" id="List">
                <asp:UpdatePanel ID="Update_List" runat="server">
                    <ContentTemplate>

                        <div class="row">
                            <div class="col-md-12">
                                <div id="Div1" runat="server" class="box box-info collapsed-box">
                                    <div class="box-header with-border">
                                        <h3 class="box-title">
                                            <asp:Label ID="Label23" runat="server" Text="<%$ Resources:Attendance,Advance Search%>"></asp:Label></h3>
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

                                        <div class="col-md-6">
                                            <asp:DropDownList ID="ddlLocation" runat="server" Class="form-control" OnSelectedIndexChanged="ddlLocation_SelectedIndexChanged" AutoPostBack="true">
                                            </asp:DropDownList>
                                        </div>
                                        <div class="col-md-12">
                                            <br />
                                        </div>
                                        <div class="col-lg-3">
                                            <asp:DropDownList ID="ddlFieldName" runat="server" class="form-control">
                                                <asp:ListItem Text="<%$ Resources:Attendance,User Id %>" Value="User_Id"></asp:ListItem>
                                                <asp:ListItem Text="<%$ Resources:Attendance,Role Name %>" Value="Role_Name"></asp:ListItem>
                                                <asp:ListItem Text="<%$ Resources:Attendance,User Name %>" Value="Emp_Name"></asp:ListItem>
                                            </asp:DropDownList>
                                        </div>
                                        <div class="col-lg-2">
                                            <asp:DropDownList ID="ddlOption" runat="server" class="form-control">
                                                <asp:ListItem Text="<%$ Resources:Attendance,--Select-- %>"></asp:ListItem>
                                                <asp:ListItem Text="<%$ Resources:Attendance,Equal %>"></asp:ListItem>
                                                <asp:ListItem Selected="True" Text="<%$ Resources:Attendance,Contains %>"></asp:ListItem>
                                                <asp:ListItem Text="<%$ Resources:Attendance,Like %>"></asp:ListItem>
                                            </asp:DropDownList>
                                        </div>
                                        <div class="col-lg-5">
                                            <asp:Panel ID="Panel1" runat="server" DefaultButton="btnbind">
                                                <asp:TextBox ID="txtValue" runat="server" class="form-control" placeholder="Search From Content"></asp:TextBox>
                                            </asp:Panel>
                                        </div>
                                        <div class="col-lg-2" style="text-align: center;">
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
                                            <asp:GridView CssClass="table-striped table-bordered table table-hover" ID="gvUserMaster" PageSize="<%# PageControlCommon.GetPageSize() %>" runat="server"
                                                AutoGenerateColumns="False" Width="100%" AllowPaging="True" AllowSorting="True"
                                                OnPageIndexChanging="gvUserMaster_PageIndexChanging" OnSorting="gvUserMaster_OnSorting">
                                                <Columns>
                                                    <asp:TemplateField HeaderText="<%$ Resources:Attendance,Action%>">
                                                        <ItemTemplate>
                                                            <div class="dropdown" style="position: absolute;">
                                                                <button class="btn btn-default dropdown-toggle" type="button" data-toggle="dropdown">
                                                                    <i class="fa fa-ellipsis-h" aria-hidden="true"></i>
                                                                </button>
                                                                <ul class="dropdown-menu">

                                                                    <li <%= hdnCanEdit.Value=="true"?"style='display:block'":"style='display:none'" %>>
                                                                        <asp:LinkButton ID="btnEdit" runat="server" CommandArgument='<%# Eval("User_Id") %>' CausesValidation="False" OnCommand="btnEdit_Command" ToolTip="<%$ Resources:Attendance,Edit %>"> <i class="fa fa-pencil"></i><%# Resources.Attendance.Edit%> </asp:LinkButton>
                                                                    </li>
                                                                    <li <%= hdnCanDelete.Value=="true"?"style='display:block'":"style='display:none'" %>>
                                                                        <asp:LinkButton ID="IbtnDelete" runat="server" CausesValidation="False" CommandArgument='<%# Eval("User_Id") %>' OnCommand="IbtnDelete_Command" ToolTip="<%$ Resources:Attendance,Delete %>"><i class="fa fa-trash"></i><%# Resources.Attendance.Delete%></asp:LinkButton>
                                                                        <cc1:ConfirmButtonExtender ID="confirm1" runat="server" ConfirmText="<%$ Resources:Attendance,Are you sure you want to delete the record?%>" TargetControlID="IbtnDelete"></cc1:ConfirmButtonExtender>
                                                                    </li>
                                                                </ul>
                                                            </div>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>

                                                    <asp:TemplateField HeaderText="<%$ Resources:Attendance,User Id %>" SortExpression="User_Id">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblUserId1" runat="server" Text='<%# Eval("User_Id") %>'></asp:Label>
                                                        </ItemTemplate>
                                                        <ItemStyle />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="<%$ Resources:Attendance,User Name %>" SortExpression="Emp_Name">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblUserName" runat="server" Text='<%# Eval("Emp_name") %>'></asp:Label>
                                                        </ItemTemplate>
                                                        <ItemStyle />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="<%$ Resources:Attendance,Role Name %>" SortExpression="Role_Name">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lbUserName" runat="server" Text='<%# Eval("Role_Name") %>'></asp:Label>
                                                            <asp:Label ID="lblRoleId" Visible="false" runat="server" Text='<%# Eval("Role_Id") %>'></asp:Label>
                                                        </ItemTemplate>
                                                        <ItemStyle />
                                                    </asp:TemplateField>
                                                     <asp:TemplateField HeaderText="<%$ Resources:Attendance,Location Name %>" SortExpression="Role_Name">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblLocationName" runat="server" Text='<%# Eval("Location_Name") %>'></asp:Label>
                                                          
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
            <div class="tab-pane" id="New">
                <asp:UpdatePanel ID="Update_New" runat="server">

                    <ContentTemplate>
                        <div class="row">
                            <div class="col-md-12">
                                <div class="box box-info">
                                    <div class="box-header with-border">
                                        <h3 class="box-title">
                                            <asp:Label ID="Label35" runat="server" Text="<%$ Resources:Attendance,User%>"></asp:Label></h3>
                                        <div class="box-tools pull-right">
                                            <button type="button" class="btn btn-box-tool" data-widget="collapse">
                                                <i class="fa fa-minus"></i>
                                            </button>
                                        </div>
                                    </div>
                                    <div class="box-body">
                                        <div class="form-group">
                                            <div id="trSuperUser" runat="server" class="col-md-12" style="text-align: center; display: none;">
                                                <asp:RadioButton ID="rbtnSuperAdmin" runat="server" GroupName="admin" Text="<%$ Resources:Attendance,Super Admin %>"
                                                    AutoPostBack="true" OnCheckedChanged="rbtnUserCheckedChanged" />
                                                <asp:RadioButton ID="rbtnUser" Style="margin-left: 15px;" runat="server" GroupName="admin"
                                                    Text="<%$ Resources:Attendance,User %>" AutoPostBack="true" OnCheckedChanged="rbtnUserCheckedChanged" />
                                            </div>
                                            <div class="col-md-6">
                                                <div id="trEmp" runat="server">
                                                    <asp:Label ID="Label11" runat="server" Text="<%$ Resources:Attendance,Employee %>"></asp:Label>
                                                    <a style="color: Red">*</a>
                                                    <asp:RequiredFieldValidator EnableClientScript="true" Style="float: right;" runat="server"
                                                        ID="RequiredFieldValidator1" ValidationGroup="Next" Display="Dynamic" SetFocusOnError="true"
                                                        ControlToValidate="txtEmp" ErrorMessage="Enter User Name"></asp:RequiredFieldValidator>
                                                    <asp:TextBox ID="txtEmp" OnTextChanged="ddlEmp_TextChanged" AutoPostBack="true" BackColor="#eeeeee"
                                                        runat="server" CssClass="form-control" />
                                                    <asp:HiddenField ID="hdnempid" runat="server" Value="0" />
                                                    <cc1:AutoCompleteExtender ID="AutoCompleteExtender2" runat="server" DelimiterCharacters=""
                                                        Enabled="True" ServiceMethod="GetCompletionList" ServicePath="" CompletionInterval="100"
                                                        MinimumPrefixLength="1" CompletionSetCount="1" TargetControlID="txtEmp" UseContextKey="True"
                                                        CompletionListCssClass="autoCompleteList" CompletionListItemCssClass="autoCompleteListItem"
                                                        CompletionListHighlightedItemCssClass="autoCompleteSelectedListItem" OnClientShown="resetPosition">
                                                    </cc1:AutoCompleteExtender>
                                                    <br />
                                                </div>
                                                <asp:Label ID="lblRoleName" runat="server" Text="<%$ Resources:Attendance,User Id %>"></asp:Label>
                                                <a style="color: Red">*</a>
                                                <asp:RequiredFieldValidator EnableClientScript="true" Style="float: right;" runat="server"
                                                    ID="RequiredFieldValidator3" ValidationGroup="Next" Display="Dynamic" SetFocusOnError="true"
                                                    ControlToValidate="txtUserName" ErrorMessage="<%$ Resources:Attendance,Enter User ID %>"></asp:RequiredFieldValidator>
                                                <asp:TextBox ID="txtUserName" BackColor="#eeeeee" runat="server" CssClass="form-control" />
                                                <cc1:AutoCompleteExtender ID="AutoCompleteExtender3" runat="server" DelimiterCharacters=""
                                                    Enabled="True" ServiceMethod="GetCompletionListUser" ServicePath="" CompletionInterval="100"
                                                    MinimumPrefixLength="1" CompletionSetCount="1" TargetControlID="txtUserName"
                                                    UseContextKey="True" CompletionListCssClass="autoCompleteList" CompletionListItemCssClass="autoCompleteListItem"
                                                    CompletionListHighlightedItemCssClass="autoCompleteSelectedListItem" OnClientShown="resetPosition">
                                                </cc1:AutoCompleteExtender>
                                                <br />
                                                <div id="trRole" runat="server" visible="false">
                                                    <asp:Label ID="Label14" runat="server" Text="<%$ Resources:Attendance,Role Id  %>"></asp:Label>
                                                    <a style="color: Red">*</a>
                                                    <asp:RequiredFieldValidator EnableClientScript="true" Style="float: right;" runat="server"
                                                        ID="RequiredFieldValidator4" ValidationGroup="Next" Display="Dynamic" SetFocusOnError="true"
                                                        ControlToValidate="ddlRole" InitialValue="--Select--" ErrorMessage="<%$ Resources:Attendance,Select Role Id %>" />
                                                    <asp:DropDownList ID="ddlRole" runat="server" CssClass="form-control" AutoPostBack="true"
                                                        OnSelectedIndexChanged="ddlRole_SelectIndexChanged" />
                                                    <br />
                                                </div>
                                                <div id="trEditRole" runat="server" visible="false">
                                                </div>
                                                 <br />
                                                   <asp:Label ID="lblIsStandaloneUser" Visible="false" runat="server" Text="<%$ Resources:Attendance,Is Stand Alone User %>"></asp:Label>
                                                <asp:CheckBox ID="chkIsStandAloneUser" runat="server" CssClass="form-control" Text="<%$ Resources:Attendance,Is Stand Alone User %>" />
                                                   <br />
                                                 <asp:Label ID="lblAndroidDevice" runat="server" Text="<%$ Resources:Attendance,Device Id  %>"></asp:Label>
                                                <asp:DropDownList ID="ddlAndroidDevice" runat="server" CssClass="form-control" />
                                                <br />                                                
                                                <asp:CheckBox ID="chkEmailConfig" runat="server" CssClass="form-control" Text="Is Email Configuration" />
                                              
                                               
                                             

                                               
                                            </div>
                                            <div class="col-md-6">
                                                <asp:Label ID="Label12" runat="server" Text="<%$ Resources:Attendance,Password %>"></asp:Label>
                                                <a style="color: Red">*</a>
                                                <asp:RequiredFieldValidator EnableClientScript="true" Style="float: right;" runat="server"
                                                    ID="RequiredFieldValidator2" ValidationGroup="Next" Display="Dynamic" SetFocusOnError="true"
                                                    ControlToValidate="txtPassword" ErrorMessage="<%$ Resources:Attendance,Enter Password %>"></asp:RequiredFieldValidator>
                                                <asp:TextBox ID="txtPassword" TextMode="Password" runat="server" CssClass="form-control" />
                                                <br />
                                                <asp:Label ID="lblLanguage" runat="server" Text="<%$ Resources:Attendance,Language %>"></asp:Label>
                                                <asp:DropDownList ID="ddlLanguage" runat="server" CssClass="form-control">
                                                    <asp:ListItem Value="1">US English</asp:ListItem>
                                                    <asp:ListItem Value="2">Arabic</asp:ListItem>
                                                </asp:DropDownList>
                                                <br />

                                               
                                                   <asp:CheckBox ID="chkEditRole" Text="<%$ Resources:Attendance,Edit Role %>" runat="server" CssClass="form-control"
                                                    AutoPostBack="true" OnCheckedChanged="chkEditRoleCheckedChanged" />
                                                
                                              <br />
                                                
                                            </div>
                                            <div class="col-md-6">
                                                <br />

                                               <asp:CheckBox ID="chkIsGlobalAccess" runat="server" CssClass="form-control" Text="Is Global Access" />
                                                
                                                
                                            </div>
                                            <div class="col-md-6">
                                                <br />
                                                <asp:Button ID="btnEmailConfiguration" runat="server" Visible="false" CssClass="btn btn-info" OnClick="btnEmailConfiguration_Click" Text="Email Configuration" />
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-md-3">
                                <div class="box box-info">
                                    <div class="box-header with-border">
                                        <h3 class="box-title">
                                            <asp:Label ID="Label3" runat="server" Text="<%$ Resources:Attendance,Company%>"></asp:Label></h3>
                                        <div class="box-tools pull-right">
                                            <button type="button" class="btn btn-box-tool" data-widget="collapse">
                                                <i class="fa fa-minus"></i>
                                            </button>
                                        </div>
                                    </div>
                                    <div class="box-body">
                                        <div class="form-group">
                                            <div id="inner-content-Company" style="height: 200px; overflow: auto;">
                                                <asp:CheckBoxList ID="chkCompany" runat="server" RepeatColumns="1" AutoPostBack="True"
                                                    OnSelectedIndexChanged="chkCompany_SelectedIndexChanged" Font-Names="Trebuchet MS"
                                                    Font-Size="Small" ForeColor="Gray" />
                                            </div>
                                        </div>
                                    </div>
                                </div>



                            </div>
                            <div class="col-md-3">
                                <div class="box box-info">
                                    <div class="box-header with-border">
                                        <h3 class="box-title">
                                            <asp:Label ID="Label4" runat="server" Text="<%$ Resources:Attendance,Brand%>"></asp:Label></h3>
                                        <div class="box-tools pull-right">
                                            <button type="button" class="btn btn-box-tool" data-widget="collapse">
                                                <i class="fa fa-minus"></i>
                                            </button>
                                        </div>
                                    </div>
                                    <div class="box-body">
                                        <div class="form-group">
                                            <div id="inner-content-Brand" style="height: 200px; overflow: auto;">
                                                <asp:CheckBoxList ID="chkBrand" runat="server" RepeatColumns="1" AutoPostBack="True"
                                                    OnSelectedIndexChanged="chkBrand_SelectedIndexChanged" Font-Names="Trebuchet MS"
                                                    Font-Size="Small" ForeColor="Gray" />
                                            </div>
                                        </div>
                                    </div>
                                </div>


                            </div>
                            <div class="col-md-3">
                                <div class="box box-info">
                                    <div class="box-header with-border">
                                        <h3 class="box-title">
                                            <asp:Label ID="Label7" runat="server" Text="<%$ Resources:Attendance,Location%>"></asp:Label>

                                            <asp:CheckBox ID="chklocation_SelectAll" Text="<%$ Resources:Attendance,select all%>" AutoPostBack="true" OnCheckedChanged="chklocation_SelectAll_CheckedChanged" runat="server" />



                                        </h3>
                                        <div class="box-tools pull-right">
                                            <button type="button" class="btn btn-box-tool" data-widget="collapse">
                                                <i class="fa fa-minus"></i>
                                            </button>
                                        </div>
                                    </div>
                                    <div class="box-body">
                                        <div class="form-group">
                                            <div id="inner-content-Location" style="height: 200px; overflow: auto;">
                                                <asp:CheckBoxList ID="chkLocation" runat="server" RepeatColumns="1" AutoPostBack="True"
                                                    OnSelectedIndexChanged="chkLocation_SelectedIndexChanged" Font-Names="Trebuchet MS"
                                                    Font-Size="Small" ForeColor="Gray">
                                                </asp:CheckBoxList>
                                            </div>
                                        </div>
                                    </div>
                                </div>


                            </div>
                            <div class="col-md-3">
                                <div class="box box-info">
                                    <div class="box-header with-border">
                                        <h3 class="box-title">
                                            <asp:Label ID="Label8" runat="server" Text="<%$ Resources:Attendance,Department%>"></asp:Label>
                                            <asp:CheckBox ID="chkdepartment" onClick="new_validation1();" Text="<%$ Resources:Attendance,select all%>" runat="server" />



                                        </h3>
                                        <div class="box-tools pull-right">
                                            <button type="button" class="btn btn-box-tool" data-widget="collapse">
                                                <i class="fa fa-minus"></i>
                                            </button>
                                        </div>
                                    </div>
                                    <div class="box-body">
                                        <div class="form-group">
                                            <div id="inner-content-Department" style="height: 200px; overflow: auto;">
                                                <asp:TreeView ID="TreeViewDepartment" runat="server" CssClass="labelComman" Height="100%" ShowCheckBoxes="All">
                                                </asp:TreeView>
                                            </div>
                                        </div>
                                    </div>
                                </div>

                            </div>
                        </div>
                        <div id="Div_Rol_Permission" runat="server" style="display: none;" class="row">
                            <div class="col-md-4">
                            </div>
                            <div class="col-md-4">
                                <div class="box box-info">
                                    <div class="box-header with-border">
                                        <h3 class="box-title">
                                            <asp:Label ID="Label9" runat="server" Text="<%$ Resources:Attendance,User Permission %>"></asp:Label></h3>
                                        <div class="box-tools pull-right">
                                            <button type="button" class="btn btn-box-tool" data-widget="collapse">
                                                <i class="fa fa-minus"></i>
                                            </button>
                                        </div>
                                    </div>
                                    <div class="box-body">
                                        <div class="form-group">
                                            <asp:TextBox ID="txtRoleName" BackColor="#eeeeee" runat="server" placeholder="Enter Role Name"
                                                AutoPostBack="true" OnTextChanged="txtRoleName_OnTextChanged" CssClass="form-control" />
                                            <cc1:AutoCompleteExtender ID="AutoCompleteExtender1" runat="server" DelimiterCharacters=""
                                                Enabled="True" ServiceMethod="GetCompletionListRoleName" ServicePath="" CompletionInterval="100"
                                                MinimumPrefixLength="1" CompletionSetCount="1" TargetControlID="txtRoleName"
                                                UseContextKey="True" CompletionListCssClass="autoCompleteList" CompletionListItemCssClass="autoCompleteListItem"
                                                CompletionListHighlightedItemCssClass="autoCompleteSelectedListItem" OnClientShown="resetPosition">
                                            </cc1:AutoCompleteExtender>
                                            <br />
                                            <div style="text-align: center;">
                                                <asp:Button ID="btnAddRole" runat="server" Text="Add" CssClass="btn btn-primary" CausesValidation="False"
                                                    OnClick="btnAddRole_Click" />
                                                <asp:Button ID="BtnDeleteRole" runat="server" Text="Delete" CssClass="btn btn-primary" CausesValidation="False"
                                                    OnClick="BtnDeleteRole_Click" />
                                            </div>
                                            <br />
                                            <asp:CheckBox ID="chkSelectAll" onClick="new_validation();" Text="select all" runat="server" />
                                            <br />
                                            <div id="inner-content-Tree" style="height: 200px; overflow: auto;">
                                                <asp:TreeView ID="navTree" runat="server" Height="100%" ShowCheckBoxes="All">
                                                </asp:TreeView>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div class="col-md-4">
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-md-12" style="text-align: center;">
                                <asp:Button ID="btnSave" runat="server" CssClass="btn btn-success" OnClick="btnSave_Click"
                                    Text="<%$ Resources:Attendance,Save %>" ValidationGroup="Next" />
                                <asp:Button ID="btnReset" Style="margin-left: 15px;" runat="server" CausesValidation="False"
                                    CssClass="btn btn-primary" OnClick="btnReset_Click" Text="<%$ Resources:Attendance,Reset %>" />
                                <asp:Button ID="btnCancel" Style="margin-left: 15px;" runat="server" CausesValidation="False"
                                    CssClass="btn btn-danger" OnClick="btnCancel_Click" Text="<%$ Resources:Attendance,Cancel %>" />
                                <asp:HiddenField ID="editid" runat="server" />
                                <asp:HiddenField ID="hdnRoleId" runat="server" />
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
                                            <asp:Label ID="Label24" runat="server" Text="<%$ Resources:Attendance,Advance Search%>"></asp:Label></h3>
                                        &nbsp;&nbsp;|&nbsp;&nbsp;
                                                <asp:Label ID="lblbinTotalRecords" Font-Bold="true" runat="server" Text="<%$ Resources:Attendance,Total Records: 0 %>"></asp:Label>

                                        <div class="box-tools pull-right">
                                            <button type="button" class="btn btn-box-tool" data-widget="collapse">
                                                <i id="I2" runat="server" class="fa fa-plus"></i>
                                            </button>
                                        </div>
                                    </div>
                                    <div class="box-body">
                                        <div class="col-lg-3">
                                            <asp:DropDownList ID="ddlbinFieldName" runat="server" class="form-control">
                                                <asp:ListItem Text="<%$ Resources:Attendance,User Id %>" Value="User_Id"></asp:ListItem>
                                                <asp:ListItem Text="<%$ Resources:Attendance,Role Name %>" Value="Role_Name"></asp:ListItem>
                                                <asp:ListItem Text="<%$ Resources:Attendance,User Name %>" Value="Emp_Name"></asp:ListItem>
                                            </asp:DropDownList>
                                        </div>
                                        <div class="col-lg-2">
                                            <asp:DropDownList ID="ddlbinOption" runat="server" class="form-control">
                                                <asp:ListItem Text="<%$ Resources:Attendance,--Select-- %>"></asp:ListItem>
                                                <asp:ListItem Text="<%$ Resources:Attendance,Equal %>"></asp:ListItem>
                                                <asp:ListItem Selected="True" Text="<%$ Resources:Attendance,Contains %>"></asp:ListItem>
                                                <asp:ListItem Text="<%$ Resources:Attendance,Like %>"></asp:ListItem>
                                            </asp:DropDownList>
                                        </div>
                                        <div class="col-lg-5">
                                            <asp:Panel ID="Panel2" runat="server" DefaultButton="btnbinbind">
                                                <asp:TextBox ID="txtbinValue" runat="server" CssClass="form-control"></asp:TextBox>
                                            </asp:Panel>
                                        </div>
                                        <div class="col-lg-2" style="text-align: center;">
                                            <asp:LinkButton ID="btnbinbind" runat="server" CausesValidation="False" OnClick="btnbinbind_Click" ToolTip="<%$ Resources:Attendance,Search %>"><span class="fa fa-search"  style="font-size:25px;"></span></asp:LinkButton>&nbsp;&nbsp;&nbsp;
                                            <asp:LinkButton ID="btnbinRefresh" runat="server" CausesValidation="False" OnClick="btnbinRefresh_Click" ToolTip="<%$ Resources:Attendance,Refresh %>"><span class="fa fa-repeat"  style="font-size:25px;"></span></asp:LinkButton>&nbsp;&nbsp;&nbsp;
                                            <asp:LinkButton ID="imgBtnRestore" runat="server" CausesValidation="False" OnClick="imgBtnRestore_Click" ToolTip="<%$ Resources:Attendance,Refresh %>"><span class="far fa-lightbulb"  style="font-size:25px;"></span></asp:LinkButton>&nbsp;&nbsp;&nbsp;
                                            <asp:LinkButton ID="ImgbtnSelectAll" Visible="false" runat="server" CausesValidation="False" OnClick="ImgbtnSelectAll_Click" ToolTip="<%$ Resources:Attendance,Select All %>"><span class="fas fa-th"  style="font-size:25px;"></span></asp:LinkButton>
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
                                            <asp:GridView CssClass="table-striped table-bordered table table-hover" ID="gvUserMasterBin" PageSize="<%# PageControlCommon.GetPageSize() %>"
                                                runat="server" AutoGenerateColumns="False" DataKeyNames="User_Id" Width="100%"
                                                AllowPaging="True" OnPageIndexChanging="gvUserMasterBin_PageIndexChanging" OnSorting="gvUserMasterBin_OnSorting"
                                                AllowSorting="true">
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
                                                    <asp:TemplateField HeaderText="<%$ Resources:Attendance,User Id %>" SortExpression="User_Id">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblUserId" runat="server" Text='<%# Eval("User_Id") %>'></asp:Label>
                                                        </ItemTemplate>
                                                        <ItemStyle />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="<%$ Resources:Attendance,User Name %>" SortExpression="Emp_Name">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblUserName" runat="server" Text='<%# Eval("Emp_name") %>'></asp:Label>
                                                        </ItemTemplate>
                                                        <ItemStyle />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="<%$ Resources:Attendance,Role Name %>" SortExpression="Role_Name">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lbUserName" runat="server" Text='<%# Eval("Role_Name") %>'></asp:Label>
                                                            <asp:Label ID="lblRoleId" Visible="false" runat="server" Text='<%# Eval("Role_Id") %>'></asp:Label>
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
            <div class="tab-pane" id="Search">
                <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                    <ContentTemplate>
                        <div class="row">
                            <div class="col-md-12">
                                <div class="box box-info">
                                    <div class="box-header with-border">
                                        <h3 class="box-title">
                                            <asp:Label ID="Label17" runat="server" Text="<%$ Resources:Attendance,Search%>"></asp:Label></h3>
                                        <div class="box-tools pull-right">
                                            <button type="button" class="btn btn-box-tool" data-widget="collapse">
                                                <i class="fa fa-minus"></i>
                                            </button>
                                        </div>
                                    </div>
                                    <div class="box-body">
                                        <div class="form-group">
                                            <div class="col-md-6">
                                                <asp:Label ID="Label18" runat="server" Text="<%$ Resources:Attendance,User Name %>"></asp:Label>
                                                <asp:TextBox ID="txtSearchUserName" OnTextChanged="txtSearchUserName_TextChanged"
                                                    AutoPostBack="true" BackColor="#eeeeee" runat="server" CssClass="form-control" />
                                                <cc1:AutoCompleteExtender ID="AutoCompleteExtender4" runat="server" DelimiterCharacters=""
                                                    Enabled="True" ServiceMethod="GetCompletionListSearchEmployeeName" ServicePath=""
                                                    CompletionInterval="100" MinimumPrefixLength="1" CompletionSetCount="1" TargetControlID="txtSearchUserName"
                                                    UseContextKey="True" CompletionListCssClass="autoCompleteList" CompletionListItemCssClass="autoCompleteListItem"
                                                    CompletionListHighlightedItemCssClass="autoCompleteSelectedListItem" OnClientShown="resetPosition">
                                                </cc1:AutoCompleteExtender>
                                                <br />
                                                <asp:Label ID="Label20" runat="server" Text="<%$ Resources:Attendance,Module Name %>"></asp:Label>
                                                <asp:DropDownList ID="ddlSearchModuleName" runat="server" CssClass="form-control"
                                                    OnSelectedIndexChanged="ddlSearchModuleName_OnSelectedIndexChanged" AutoPostBack="true">
                                                </asp:DropDownList>
                                                <br />
                                                <asp:Label ID="Label22" runat="server" Text="<%$ Resources:Attendance,Operation%>"></asp:Label>
                                                <asp:DropDownList ID="ddlOperation" runat="server" CssClass="form-control">
                                                </asp:DropDownList>
                                                <br />
                                            </div>
                                            <div class="col-md-6">
                                                <asp:Label ID="Label19" runat="server" Text="<%$ Resources:Attendance,Role Name %>"></asp:Label>
                                                <asp:TextBox ID="txtSearchRolename" BackColor="#eeeeee" runat="server" AutoPostBack="true"
                                                    OnTextChanged="txtRoleName_OnTextChanged" CssClass="form-control" />
                                                <cc1:AutoCompleteExtender ID="AutoCompleteExtender5" runat="server" DelimiterCharacters=""
                                                    Enabled="True" ServiceMethod="GetCompletionListRoleName" ServicePath="" CompletionInterval="100"
                                                    MinimumPrefixLength="1" CompletionSetCount="1" TargetControlID="txtSearchRolename"
                                                    UseContextKey="True" CompletionListCssClass="autoCompleteList" CompletionListItemCssClass="autoCompleteListItem"
                                                    CompletionListHighlightedItemCssClass="autoCompleteSelectedListItem" OnClientShown="resetPosition">
                                                </cc1:AutoCompleteExtender>
                                                <br />
                                                <asp:Label ID="Label21" runat="server" Text="<%$ Resources:Attendance,Object Name %>"></asp:Label>
                                                <asp:DropDownList ID="ddlserachObjectname" runat="server" CssClass="form-control">
                                                </asp:DropDownList>
                                                <br />
                                                <br />
                                                <asp:Button ID="btngo" runat="server" CausesValidation="False" Text="<%$ Resources:Attendance,Go %>"
                                                    Visible="true" CssClass="btn btn-primary" OnClick="btngo_Click" />
                                            </div>
                                        </div>
                                    </div>
                                </div>
                                <div class="box box-warning box-solid" <%= gvRoleUser.Rows.Count>0?"style='display:block'":"style='display:none'"%>>
                                    <div class="box-body">
                                        <div class="row">
                                            <div class="col-md-12">
                                                <div style="height: 500px; overflow: auto;">
                                                    <asp:GridView CssClass="table-striped table-bordered table table-hover" ID="gvRoleUser" runat="server" Width="100%" AutoGenerateColumns="false">
                                                        <Columns>
                                                            <asp:TemplateField>
                                                                <ItemTemplate>
                                                                    <asp:ImageButton runat="server" ID="btnRoleEdit" ImageUrl="~/Images/edit.png" OnCommand="btnRoleEdit_Command" OnClientClick="$('#modalPermissionList').modal('show');" />
                                                                </ItemTemplate>
                                                            </asp:TemplateField>

                                                            <asp:TemplateField>
                                                                <ItemTemplate>
                                                                    <asp:ImageButton runat="server" ID="btnRoleDelete" ImageUrl="~/Images/Erase.png" OnCommand="btnRoleDelete_Command" />
                                                                </ItemTemplate>
                                                            </asp:TemplateField>

                                                            <asp:TemplateField HeaderText="User Name">
                                                                <ItemTemplate>
                                                                    <asp:Label runat="server" ID="lbluserName" Text='<%# Eval("UserName") %>' />
                                                                </ItemTemplate>
                                                            </asp:TemplateField>

                                                            <asp:TemplateField HeaderText="Role Name">
                                                                <ItemTemplate>
                                                                    <asp:Label runat="server" ID="lblRoleName" Text='<%# Eval("RoleName") %>' />
                                                                </ItemTemplate>
                                                            </asp:TemplateField>

                                                            <asp:TemplateField HeaderText="Module Name">
                                                                <ItemTemplate>
                                                                    <asp:Label runat="server" ID="lblModuleName" Text='<%# Eval("ModuleName") %>' />
                                                                </ItemTemplate>
                                                            </asp:TemplateField>

                                                            <asp:TemplateField HeaderText="Object Name">
                                                                <ItemTemplate>
                                                                    <asp:Label runat="server" ID="lblObjectName" Text='<%# Eval("ObjectName") %>' />
                                                                </ItemTemplate>
                                                            </asp:TemplateField>

                                                            <asp:TemplateField HeaderText="Assigned Role">
                                                                <ItemTemplate>
                                                                    <asp:Label runat="server" ID="lblOperation" Text='<%# Eval("Operation") %>' />
                                                                </ItemTemplate>
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
                </asp:UpdatePanel>
            </div>
        </div>
    </div>

    <div class="modal fade" id="modalPermissionList" tabindex="-1" role="dialog" aria-labelledby="myModalLabel" data-backdrop="static" aria-hidden="true">
        <div class="modal-dialog modal-lg">
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal" onclick="resetReportField()">
                        <span aria-hidden="true">&times;</span><span class="sr-only">Close</span></button>
                </div>
                <div class="modal-body">
                    <div class="col-md-12">


                        <asp:UpdatePanel runat="server" ID="upPermissionList">
                            <ContentTemplate>
                                <asp:HiddenField runat="server" ID="hdnUserName" />
                                <asp:HiddenField runat="server" ID="hdnModuleName" />
                                <asp:HiddenField runat="server" ID="hdnObjectName" />
                                <asp:HiddenField runat="server" ID="hdnUserTransID" />
                                <asp:Panel ID="pnlBrand" runat="server" Style="max-height: 500px;" Width="100%" BorderStyle="Solid"
                                    BorderWidth="1px" BorderColor="#eeeeee" BackColor="White" ScrollBars="Auto">
                                    <asp:CheckBoxList ID="chkPermissionList" runat="server" RepeatColumns="3" CellPadding="5"
                                        CellSpacing="10" Font-Names="Trebuchet MS" Font-Size="Small" ForeColor="Gray" />
                                </asp:Panel>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </div>
                    <div class="col-md-12">
                        <asp:Button runat="server" ID="btnSavePermission" OnClick="btnSavePermission_Click" Text="Save Premission" />
                    </div>
                </div>
                <div class="modal-footer">
                    <button type="button" id="" class="btn btn-danger" data-dismiss="modal" onclick="resetReportField()">
                        Close</button>
                </div>
            </div>
        </div>
    </div>


    <div class="modal fade" id="ModelUpdateLicense" role="dialog">
        <div class="modal-dialog modal-lg">

            <!-- Modal content-->
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal">&times;</button>
                    <div class="modal-title" style="text-align: left;">
                        <img src="https://www.pegasustech.net/image/catalog/logo.png" class="img-responsive" width="120px" />
                    </div>
                </div>

                <div>
                    <uc1:UpdateLicense ID="UC_LicenseInfo" runat="server" />
                </div>
                <div class="modal-footer">
                </div>
            </div>

        </div>
    </div>

    <div class="modal fade" id="myModal" tabindex="-1" role="dialog" aria-labelledby="myModalLabel"
        aria-hidden="true">
        <div class="modal-dialog modal-lg">
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal">
                        <span aria-hidden="true">&times;</span><span class="sr-only"><asp:Label ID="Label25" runat="server" Text="<%$ Resources:Attendance,Close%>"></asp:Label></span></button>
                    <h4 class="modal-title" id="myModalLabel">
                        <asp:Label ID="Label26" runat="server" Text="Email Configuration"></asp:Label></h4>
                </div>
                <div class="modal-body">
                    <uc:EmailConfig ID="Email_Config" runat="server"></uc:EmailConfig>
                </div>
                <div class="modal-footer">
                    <button type="button" class="btn btn-default" data-dismiss="modal">
                        <asp:Label ID="Label27" runat="server" Text="<%$ Resources:Attendance,Close%>"></asp:Label></button>
                </div>
            </div>
        </div>
    </div>


    <asp:UpdateProgress ID="UpdateProgress5" runat="server" AssociatedUpdatePanelID="upPermissionList">
        <ProgressTemplate>
            <div class="modal_Progress">
                <div class="center_Progress">
                    <img alt="" src="../Images/ajax-loader2.gif" style="vertical-align: middle; background-color: White;" />
                </div>
            </div>
        </ProgressTemplate>
    </asp:UpdateProgress>

    <asp:UpdateProgress ID="UpdateProgress1" runat="server" AssociatedUpdatePanelID="Update_List">
        <ProgressTemplate>
            <div class="modal_Progress">
                <div class="center_Progress">
                    <img alt="" src="../Images/ajax-loader2.gif" style="vertical-align: middle; background-color: White;" />
                </div>
            </div>
        </ProgressTemplate>
    </asp:UpdateProgress>
    <asp:UpdateProgress ID="UpdateProgress2" runat="server" AssociatedUpdatePanelID="Update_New">
        <ProgressTemplate>
            <div class="modal_Progress">
                <div class="center_Progress">
                    <img alt="" src="../Images/ajax-loader2.gif" style="vertical-align: middle; background-color: White;" />
                </div>
            </div>
        </ProgressTemplate>
    </asp:UpdateProgress>
    <asp:UpdateProgress ID="UpdateProgress4" runat="server" AssociatedUpdatePanelID="UpdatePanel1">
        <ProgressTemplate>
            <div class="modal_Progress">
                <div class="center_Progress">
                    <img alt="" src="../Images/ajax-loader2.gif" style="vertical-align: middle; background-color: White;" />
                </div>
            </div>
        </ProgressTemplate>
    </asp:UpdateProgress>
    <asp:UpdateProgress ID="UpdateProgress3" runat="server" AssociatedUpdatePanelID="Update_Bin">
        <ProgressTemplate>
            <div class="modal_Progress">
                <div class="center_Progress">
                    <img alt="" src="../Images/ajax-loader2.gif" style="vertical-align: middle; background-color: White;" />
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

        function show_modal() {
            document.getElementById('<%=Btn_Modal_Popup.ClientID %>').click();
        }
        function Modal_Close() {
            document.getElementById('<%= Btn_Modal_Popup.ClientID %>').click();
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

        function on_Edit_tab_position() {
            $("#Li_List").removeClass("active");
            $("#List").removeClass("active");

            $("#Li_New").addClass("active");
            $("#New").addClass("active");
        }

    </script>
    <script type="text/javascript">




        function Company(sender, args) {
            var chkBox = document.getElementById('<%= chkCompany.ClientID %>');
            var options = chkBox.getElementsByTagName('input');
            var listOfSpans = chkBox.getElementsByTagName('span');
            for (var i = 0; i < options.length; i++) {
                if (options[i].checked) {
                    args.IsValid = true;
                    return;
                }
            }
            args.IsValid = false;
        }

        function Brand(sender, args) {
            var chkBox = document.getElementById('<%= chkBrand.ClientID %>');
            var options = chkBox.getElementsByTagName('input');
            var listOfSpans = chkBox.getElementsByTagName('span');
            for (var i = 0; i < options.length; i++) {
                if (options[i].checked) {
                    args.IsValid = true;
                    return;
                }
            }
            args.IsValid = false;
        }

        function Location(sender, args) {
            var chkBox = document.getElementById('<%= chkLocation.ClientID %>');
            var options = chkBox.getElementsByTagName('input');
            var listOfSpans = chkBox.getElementsByTagName('span');
            for (var i = 0; i < options.length; i++) {
                if (options[i].checked) {
                    args.IsValid = true;
                    return;
                }
            }
            args.IsValid = false;
        }

        //function DisableCheckBoxes(treeviewID) {
        //    TREEVIEW_ID = treeviewID;

        //    var treeView = document.getElementById(TREEVIEW_ID);

        //    if (treeView) {
        //        var childCheckBoxes = treeView.getElementsByTagName("input");
        //        for (var i = 0; i < childCheckBoxes.length; i++) {
        //            var textSpan = GetCheckBoxTextSpan(childCheckBoxes[i]);

        //            if (textSpan.firstChild)
        //                if (textSpan.firstChild.className == "disabledTreeviewNode")
        //                    childCheckBoxes[i].disabled = true;
        //        }
        //    }
        //}

        //function GetCheckBoxTextSpan(checkBox) {
        //    // Set label text to node name
        //    var parentDiv = checkBox.parentNode;
        //    var nodeSpan = parentDiv.getElementsByTagName("span");

        //    return nodeSpan[0];
        //}


        function postBackByObject() {
            var o = window.event.srcElement;
            if (o.tagName == "INPUT" && o.type == "checkbox") {
                __doPostBack("<%= navTree.ClientID%>", "");
            }
        }



        function LI_List_Active() {
            $("#Li_List").addClass("active");
            $("#List").addClass("active");

            $("#Li_New").removeClass("active");
            $("#New").removeClass("active");
        }

        function Modal_UpdateLicense_Open() {
            $('#ModelUpdateLicense').modal('show')
        }




    </script>

</asp:Content>
