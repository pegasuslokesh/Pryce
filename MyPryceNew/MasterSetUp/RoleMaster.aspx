<%@ Page Title="" Language="C#" MasterPageFile="~/ERPMaster.master" AutoEventWireup="true"
    CodeFile="RoleMaster.aspx.cs" Inherits="MasterSetUp_RoleMaster" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="HeaderContent" runat="Server">
    <h1>
        <i class="fas fa-user-cog"></i>&nbsp;&nbsp;
        <asp:Label ID="lblHeader" runat="server" Text="<%$ Resources:Attendance,Role Setup%>"></asp:Label>
    </h1>
    <ol class="breadcrumb">
        <li><a href="#"><i class="fa fa-dashboard"></i>
            <asp:Label ID="Label6" runat="server" Text="<%$ Resources:Attendance,IT Security%>"></asp:Label></a></li>
        <li><a href="#"><i class="fa fa-dashboard"></i>
            <asp:Label ID="Label10" runat="server" Text="<%$ Resources:Attendance,IT Security%>"></asp:Label></a></li>
        <li class="active">
            <asp:Label ID="Label5" runat="server" Text="<%$ Resources:Attendance,Role Setup%>"></asp:Label></li>
    </ol>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="Server">
    <asp:UpdatePanel ID="Update_Button" runat="server">
        <ContentTemplate>
            <asp:Button ID="Btn_Bin" Style="display: none;" runat="server" OnClick="btnBin_Click" Text="Bin" />
            <asp:HiddenField runat="server" ID="hdnCanEdit" />
            <asp:HiddenField runat="server" ID="hdnCanView" />
            <asp:HiddenField runat="server" ID="hdnCanDelete" />
        </ContentTemplate>
    </asp:UpdatePanel>
    <asp:UpdateProgress ID="UpdateProgress4" runat="server" AssociatedUpdatePanelID="Update_Button">
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
            <li id="Li_Bin"><a onclick="Li_Tab_Bin()" href="#Bin" data-toggle="tab">
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
        <div class="tab-content">
            <div class="tab-pane active" id="List">
                <asp:UpdatePanel ID="Update_List" runat="server">
                    <ContentTemplate>



                        <div class="row">
                            <div class="col-md-12">
                                <div id="Div1" runat="server" class="box box-info collapsed-box">
                                    <div class="box-header with-border">
                                        <h3 class="box-title">
                                            <asp:Label ID="Label11" runat="server" Text="<%$ Resources:Attendance,Advance Search%>"></asp:Label></h3>
                                        &nbsp;&nbsp;|&nbsp;&nbsp;
                                               <asp:Label ID="lblTotalRecords" Font-Bold="true" runat="server" Text="<%$ Resources:Attendance,Total Records: 0 %>"></asp:Label>
                                        <asp:Label ID="lblSelectedRecord" runat="server" Visible="false"></asp:Label>

                                        <div class="box-tools pull-right">
                                            <button type="button" class="btn btn-box-tool" data-widget="collapse">
                                                <i id="I2" runat="server" class="fa fa-plus"></i>
                                            </button>
                                        </div>
                                    </div>
                                    <div class="box-body">
                                        <div class="col-lg-3">
                                            <asp:DropDownList ID="ddlFieldName" runat="server" class="form-control">
                                                <asp:ListItem Selected="True" Text="<%$ Resources:Attendance,Role Name %>" Value="Role_Name"></asp:ListItem>
                                                <asp:ListItem Text="<%$ Resources:Attendance,Role Name(Local) %>" Value="Role_Name_L"></asp:ListItem>
                                                <asp:ListItem Text="<%$ Resources:Attendance,Role Id %>" Value="Role_Id"></asp:ListItem>
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
                                            <asp:LinkButton ID="btnbind" runat="server" CausesValidation="False"  OnClick="btnbind_Click" ToolTip="<%$ Resources:Attendance,Search %>"><span class="fa fa-search"  style="font-size:25px;"></span></asp:LinkButton>&nbsp;&nbsp;&nbsp;
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
                                            <asp:GridView CssClass="table-striped table-bordered table table-hover" ID="gvRoleMaster" OnPageIndexChanging="gvRoleMaster_PageIndexChanging"
                                                PageSize="<%# PageControlCommon.GetPageSize() %>" runat="server" AutoGenerateColumns="False"
                                                Width="100%" AllowPaging="True" OnSorting="gvRoleMaster_OnSorting" AllowSorting="True">
                                                <Columns>
                                                    <asp:TemplateField HeaderText="<%$ Resources:Attendance,Action%>">
                                                        <ItemTemplate>
                                                            <div class="dropdown" style="position: absolute;">
                                                                <button class="btn btn-default dropdown-toggle"  type="button" data-toggle="dropdown">
                                                                    <i class="fa fa-ellipsis-h" aria-hidden="true"></i>
                                                                </button>
                                                                <ul class="dropdown-menu">
                                                                    <li <%= hdnCanView.Value=="true"?"style='display:block'":"style='display:none'" %>>
                                                                        <asp:LinkButton ID="btnView" runat="server" CommandArgument='<%# Eval("Role_Id") %>' OnCommand="btnView_Command" CausesValidation="False"> <i class="fa fa-eye"></i><%# Resources.Attendance.View%></asp:LinkButton>
                                                                    </li>
                                                                    <li <%= hdnCanEdit.Value=="true"?"style='display:block'":"style='display:none'" %>>
                                                                        <asp:LinkButton ID="btnEdit" runat="server" CommandArgument='<%# Eval("Role_Id") %>' CommandName="RoleEdit" OnCommand="btnEdit_Command" CausesValidation="False" ToolTip="<%$ Resources:Attendance,Edit %>"> <i class="fa fa-pencil"></i><%# Resources.Attendance.Edit%> </asp:LinkButton>
                                                                    </li>
                                                                    <li <%= hdnCanDelete.Value=="true"?"style='display:block'":"style='display:none'" %>>
                                                                        <asp:LinkButton ID="IbtnDelete" runat="server" CausesValidation="False" CommandArgument='<%# Eval("Role_Id") %>' OnCommand="IbtnDelete_Command" ToolTip="<%$ Resources:Attendance,Delete %>"><i class="fa fa-trash"></i><%# Resources.Attendance.Delete%> </asp:LinkButton>
                                                                        <cc1:ConfirmButtonExtender ID="confirm1" runat="server" ConfirmText="<%$ Resources:Attendance,Are you sure you want to delete the record?%>" TargetControlID="IbtnDelete"></cc1:ConfirmButtonExtender>
                                                                    </li>
                                                                </ul>
                                                            </div>

                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="<%$ Resources:Attendance,Role Id %>" SortExpression="Role_Id">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblRoleId1" runat="server" Text='<%# Eval("Role_Id") %>'></asp:Label>
                                                        </ItemTemplate>
                                                        <ItemStyle />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="<%$ Resources:Attendance,Role Name %>" SortExpression="Role_Name">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lbRoleName" runat="server" Text='<%# Eval("Role_Name") %>'></asp:Label>
                                                        </ItemTemplate>
                                                        <ItemStyle />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="<%$ Resources:Attendance,Role Name(Local) %>" SortExpression="Role_Name_L">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblRoleNameL" runat="server" Text='<%# Eval("Role_Name_L") %>'></asp:Label>
                                                        </ItemTemplate>
                                                        <ItemStyle />
                                                    </asp:TemplateField>
                                                     <asp:TemplateField HeaderText="<%$ Resources:Attendance,Modified By %>" SortExpression="ModifiedBy">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblModifiedBy" runat="server" Text='<%#GetUserName(Eval("ModifiedBy").ToString()) %>'></asp:Label>
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
                                            <asp:Label ID="Label35" runat="server" Text="<%$ Resources:Attendance,Role%>"></asp:Label></h3>
                                        <div class="box-tools pull-right">
                                            <button type="button" class="btn btn-box-tool" data-widget="collapse">
                                                <i class="fa fa-minus"></i>
                                            </button>
                                        </div>
                                    </div>
                                    <div class="box-body">
                                        <div class="form-group">
                                            <div class="col-md-6">
                                                <asp:Label ID="lblRoleName" runat="server" Text="<%$ Resources:Attendance,Role Name %>"></asp:Label>
                                                <a style="color: Red">*</a>
                                                <asp:RequiredFieldValidator EnableClientScript="true" Style="float: right;" runat="server"
                                                    ID="RequiredFieldValidator3" ValidationGroup="Next" Display="Dynamic" SetFocusOnError="true"
                                                    ControlToValidate="txtRoleName" ErrorMessage="Enter Role Name"></asp:RequiredFieldValidator>
                                                <asp:TextBox ID="txtRoleName" BackColor="#eeeeee" runat="server" AutoPostBack="true"
                                                    OnTextChanged="txtRoleName_OnTextChanged" CssClass="form-control" />
                                                <cc1:AutoCompleteExtender ID="AutoCompleteExtender1" runat="server" DelimiterCharacters=""
                                                    Enabled="True" ServiceMethod="GetCompletionListRoleName" ServicePath="" CompletionInterval="100"
                                                    MinimumPrefixLength="1" CompletionSetCount="1" TargetControlID="txtRoleName"
                                                    UseContextKey="True" CompletionListCssClass="autoCompleteList" CompletionListItemCssClass="autoCompleteListItem"
                                                    CompletionListHighlightedItemCssClass="autoCompleteSelectedListItem" OnClientShown="resetPosition">
                                                </cc1:AutoCompleteExtender>
                                            </div>
                                            <div class="col-md-6">
                                                <asp:Label ID="lblRoleNameL" runat="server" Text="<%$ Resources:Attendance,Role Name(Local) %>"></asp:Label>
                                                <asp:TextBox ID="txtRoleNameL" runat="server" CssClass="form-control" />
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="row" style="display: none;">
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
                                <asp:CustomValidator ID="CustomValidator8" ValidationGroup="Next" runat="server"
                                    ErrorMessage="Please select at least one Company." ClientValidationFunction="Company"
                                    ForeColor="Red"></asp:CustomValidator>
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
                                <asp:CustomValidator ID="CustomValidator1" ValidationGroup="Next" runat="server"
                                    ErrorMessage="Please select at least one Brand." ClientValidationFunction="Brand"
                                    ForeColor="Red"></asp:CustomValidator>
                            </div>
                            <div class="col-md-3">
                                <div class="box box-info">
                                    <div class="box-header with-border">
                                        <h3 class="box-title">
                                            <asp:Label ID="Label7" runat="server" Text="<%$ Resources:Attendance,Location%>"></asp:Label></h3>
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
                                                    Font-Size="Small" ForeColor="Gray" />
                                            </div>
                                        </div>
                                    </div>
                                </div>
                                <asp:CustomValidator ID="CustomValidator2" ValidationGroup="Next" runat="server"
                                    ErrorMessage="Please select at least one Location." ClientValidationFunction="Location"
                                    ForeColor="Red"></asp:CustomValidator>
                            </div>
                            <div class="col-md-3">
                                <div class="box box-info">
                                    <div class="box-header with-border">
                                        <h3 class="box-title">
                                            <asp:Label ID="Label8" runat="server" Text="<%$ Resources:Attendance,Department%>"></asp:Label></h3>
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

                                                <%--<asp:CheckBoxList ID="chkDepartment" runat="server" RepeatColumns="1" Font-Names="Trebuchet MS"
                                                    Font-Size="Small" ForeColor="Gray" />--%>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                                <asp:CustomValidator ID="CustomValidator3" ValidationGroup="Next" runat="server"
                                    ErrorMessage="Please select at least one Department." ClientValidationFunction="Department"
                                    ForeColor="Red"></asp:CustomValidator>
                            </div>
                        </div>
                        <div id="Div_Rol_Permission" runat="server" style="display: none;" class="row">
                            <div class="col-md-4">
                            </div>
                            <div class="col-md-4">
                                <div class="box box-info">
                                    <div class="box-header with-border">
                                        <h3 class="box-title">
                                            <asp:Label ID="Label9" runat="server" Text="<%$ Resources:Attendance,Role Permission %>"></asp:Label></h3>
                                        <div class="box-tools pull-right">
                                            <button type="button" class="btn btn-box-tool" data-widget="collapse">
                                                <i class="fa fa-minus"></i>
                                            </button>
                                        </div>
                                    </div>
                                    <div class="box-body">
                                        <div class="form-group">
                                            <asp:CheckBox ID="chkSelectAll" runat="server" CssClass="labelComman" onClick="new_validation();" Text="<%$ Resources:Attendance,Select All %>" />
                                            <br />
                                            <div id="inner-content-Tree" style="height: 200px; overflow: auto;">

                                                <asp:TreeView ID="navTree" runat="server" CssClass="labelComman" Height="100%" ShowCheckBoxes="All">
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
                                    Text="<%$ Resources:Attendance,Save %>" ValidationGroup="Next" Visible="false" />
                                <asp:Button ID="btnReset" Style="margin-left: 15px;" runat="server" CausesValidation="False"
                                    CssClass="btn btn-primary" OnClick="btnReset_Click" Text="<%$ Resources:Attendance,Reset %>" />
                                <asp:Button ID="btnCancel" Style="margin-left: 15px;" runat="server" CausesValidation="False"
                                    CssClass="btn btn-danger" OnClick="btnCancel_Click" Text="<%$ Resources:Attendance,Cancel %>" />
                                <asp:HiddenField ID="editid" runat="server" />
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
                                            <asp:Label ID="Label2" runat="server" Text="<%$ Resources:Attendance,Advance Search%>"></asp:Label></h3>
                                        &nbsp;&nbsp;|&nbsp;&nbsp;
                                              <asp:Label ID="lblbinTotalRecords" Font-Bold="true" runat="server" Text="<%$ Resources:Attendance,Total Records: 0 %>"></asp:Label>

                                        <div class="box-tools pull-right">
                                            <button type="button" class="btn btn-box-tool" data-widget="collapse">
                                                <i id="I1" runat="server" class="fa fa-plus"></i>
                                            </button>
                                        </div>
                                    </div>
                                    <div class="box-body">
                                        <div class="col-lg-3">
                                            <asp:DropDownList ID="ddlbinFieldName" runat="server" class="form-control">
                                                <asp:ListItem Selected="True" Text="<%$ Resources:Attendance,Role Name %>" Value="Role_Name"></asp:ListItem>
                                                <asp:ListItem Text="<%$ Resources:Attendance,Role Name(Local) %>" Value="Role_Name_L"></asp:ListItem>
                                                <asp:ListItem Text="<%$ Resources:Attendance,Role Id %>" Value="Role_Id"></asp:ListItem>
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
                                                <asp:TextBox ID="txtbinValue" runat="server" CssClass="form-control" placeholder="Search From Content"></asp:TextBox>
                                            </asp:Panel>
                                        </div>
                                        <div class="col-lg-2" style="text-align: center;">
                                            <asp:LinkButton ID="btnbinbind" runat="server" CausesValidation="False" OnClick="btnbinbind_Click" ToolTip="<%$ Resources:Attendance,Search %>"><span class="fa fa-search"  style="font-size:25px;"></span></asp:LinkButton>&nbsp;&nbsp;&nbsp;
                                            <asp:LinkButton ID="btnbinRefresh" runat="server" CausesValidation="False" OnClick="btnbinRefresh_Click" ToolTip="<%$ Resources:Attendance,Refresh %>"><span class="fa fa-repeat"  style="font-size:25px;"></span></asp:LinkButton>&nbsp;&nbsp;&nbsp;
                                            <asp:LinkButton ID="imgBtnRestore" runat="server" CausesValidation="False" OnClick="imgBtnRestore_Click" ToolTip="<%$ Resources:Attendance,Active %>"><span class="far fa-lightbulb"  style="font-size:25px;"></span></asp:LinkButton>&nbsp;&nbsp;&nbsp;
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
                                            <asp:GridView CssClass="table-striped table-bordered table table-hover" ID="gvRoleMasterBin" PageSize="<%# PageControlCommon.GetPageSize() %>"
                                                runat="server" AutoGenerateColumns="False" Width="100%" AllowPaging="True" OnPageIndexChanging="gvRoleMasterBin_PageIndexChanging"
                                                OnSorting="gvRoleMasterBin_OnSorting" DataKeyNames="Role_Id" AllowSorting="true">
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
                                                    <asp:TemplateField HeaderText="<%$ Resources:Attendance,Role Id %>" SortExpression="Role_Id">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblRoleId1" runat="server" Text='<%# Eval("Role_Id") %>'></asp:Label>
                                                        </ItemTemplate>
                                                        <ItemStyle />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="<%$ Resources:Attendance,Role Name %>" SortExpression="Role_Name">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lbRoleName" runat="server" Text='<%# Eval("Role_Name") %>'></asp:Label>
                                                            <asp:Label ID="lblRoleId" Visible="false" runat="server" Text='<%# Eval("Role_Id") %>'></asp:Label>
                                                        </ItemTemplate>
                                                        <ItemStyle />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="<%$ Resources:Attendance,Role Name(Local) %>" SortExpression="Role_Name_L">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblRoleNameL" runat="server" Text='<%# Eval("Role_Name_L") %>'></asp:Label>
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
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="AfterLoadScriptContent" runat="Server">
    <script src="../Script/common.js"></script>
    <script>

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
     
        function Li_Tab_Bin() {
            document.getElementById('<%= Btn_Bin.ClientID %>').click();
        }

        function on_View_tab_position() {
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



        //function postBackByObject() {
        //    var o = window.event.srcElement;
        //    if (o.tagName == "INPUT" && o.type == "checkbox") {
        //        __doPostBack("", "");
        //    }
        //}


    </script>



    <script type="text/javascript">
        $get("Panel1").onclick = function (e) {
            if (!e) e = window.event;
            e.cancelBubble = true;
        };
    </script>




</asp:Content>
