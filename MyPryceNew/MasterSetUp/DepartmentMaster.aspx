<%@ Page Title="" Language="C#" MasterPageFile="~/ERPMaster.master" AutoEventWireup="true" CodeFile="DepartmentMaster.aspx.cs" Inherits="MasterSetUp_DepartmentMaster" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="Head" runat="Server">
    <script>
        function LI_List_Active()
        {

        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="HeaderContent" runat="Server">
    <h1>
        <i class="fas fa-city"></i>
        <asp:Label ID="lblHeader" runat="server" Text="<%$ Resources:Attendance,Department Setup%>"></asp:Label>

        <%--<small>Control panel</small>--%>
    </h1>
    <ol class="breadcrumb">
        <li><a href="#"><i class="fa fa-dashboard"></i>
            <asp:Label ID="Label6" runat="server" Text="<%$ Resources:Attendance,System SetUp%>"></asp:Label></a></li>
        <li><a href="#"><i class="fa fa-dashboard"></i>
            <asp:Label ID="Label10" runat="server" Text="<%$ Resources:Attendance,System SetUp%>"></asp:Label></a></li>
        <li class="active">
            <asp:Label ID="Label5" runat="server" Text="<%$ Resources:Attendance,Department Setup%>"></asp:Label></li>
    </ol>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="Server">
    <div class="row">
        <div class="col-md-12">
            <asp:HiddenField runat="server" ID="hdnCanEdit" />
            <asp:HiddenField runat="server" ID="hdnCanDelete" />
            <div class="nav-tabs-custom">
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
                        <i class="fa fa-list"></i>&nbsp;&nbsp;<asp:Label ID="Label3" runat="server" Text="<%$ Resources:Attendance,List %>"></asp:Label></a></li>
                </ul>
                <div class="tab-content">

                    <div class="tab-pane active" id="List">
                        <asp:UpdatePanel ID="Update_List" runat="server">
                            <ContentTemplate>


                                <div class="row">
                                    <div class="col-md-12">
                                        <asp:HiddenField ID="HDFSort" runat="server" />
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
                                                    <asp:DropDownList ID="ddlFieldName" runat="server" CssClass="form-control">
                                                        <asp:ListItem Text="<%$ Resources:Attendance,Department Name %>" Selected="True" Value="Dep_Name"></asp:ListItem>
                                                        <asp:ListItem Text="<%$ Resources:Attendance,Department Name(Local) %>" Value="Dep_Name_L"></asp:ListItem>
                                                        <asp:ListItem Text="<%$ Resources:Attendance,Department Code %>" Value="Dep_Code"></asp:ListItem>
                                                        <asp:ListItem Text="<%$ Resources:Attendance,Created By %>" Value="Created_User"></asp:ListItem>
                                                        <asp:ListItem Text="<%$ Resources:Attendance,Modified By %>" Value="Modified_User"></asp:ListItem>
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
                                                    <asp:LinkButton ID="btnbind" runat="server" CausesValidation="False"
                                                        OnClick="btnbind_Click" ToolTip="<%$ Resources:Attendance,Search %>"><span class="fa fa-search"  style="font-size:25px;"></span></asp:LinkButton>&nbsp;&nbsp;&nbsp;

                                                    <asp:LinkButton ID="btnRefresh" runat="server" CausesValidation="False"
                                                        OnClick="btnRefresh_Click" ToolTip="<%$ Resources:Attendance,Refresh %>"><span class="fa fa-repeat"  style="font-size:25px;"></span></asp:LinkButton>&nbsp;&nbsp;&nbsp;

                                                    <asp:LinkButton ID="btnGridView" runat="server" CausesValidation="False"
                                                        Visible="true" OnClick="btnGridView_Click" ToolTip="<%$ Resources:Attendance, Tree View %>"><span class="fa fa-sitemap"  style="font-size:25px;"></span></asp:LinkButton>&nbsp;&nbsp;&nbsp;
                                                    <asp:LinkButton ID="btnTreeView" runat="server" CausesValidation="False"
                                                        Visible="false" OnClick="btnTreeView_Click"><span class="fa fa-table"  style="font-size:25px;"></span></asp:LinkButton>&nbsp;&nbsp;&nbsp;
                                                </div>


                                            </div>
                                        </div>
                                    </div>
                                </div>

                                <div class="box box-warning box-solid" <%= gvDepMaster.Rows.Count>0?"style='display:block'":"style='display:none'"%>>

                                    <div class="box-body">
                                        <div class="row">

                                            <div class="col-md-12">
                                                <br />
                                                <div class="flow">
                                                    <asp:GridView CssClass="table-striped table-bordered table table-hover" ID="gvDepMaster" PageSize="<%# PageControlCommon.GetPageSize() %>" runat="server"
                                                        AutoGenerateColumns="False" Width="100%" AllowPaging="True" AllowSorting="True"
                                                        OnPageIndexChanging="gvDepMaster_PageIndexChanging" OnSorting="gvDepMaster_OnSorting">
                                                        <Columns>

                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,Action%>">
                                                                <ItemTemplate>
                                                                    <div class="dropdown" style="position: absolute;">
                                                                        <button class="btn btn-default dropdown-toggle" type="button" data-toggle="dropdown">
                                                                            <i class="fa fa-ellipsis-h" aria-hidden="true"></i>
                                                                        </button>
                                                                        <ul class="dropdown-menu">


                                                                            <li <%= hdnCanEdit.Value=="true"?"style='display:block'":"style='display:none'" %>>
                                                                                <asp:LinkButton ID="btnEdit" runat="server" CommandArgument='<%# Eval("Dep_Id") %>'
                                                                                    CausesValidation="False" OnCommand="btnEdit_Command"><i class="fa fa-pencil"></i><%# Resources.Attendance.Edit%> </asp:LinkButton>
                                                                            </li>
                                                                            <li <%= hdnCanDelete.Value=="true"?"style='display:block'":"style='display:none'" %>>
                                                                                <asp:LinkButton ID="IbtnDelete" runat="server" CausesValidation="False" CommandArgument='<%# Eval("Dep_Id") %>'
                                                                                    OnCommand="IbtnDelete_Command"><i class="fa fa-trash"></i><%# Resources.Attendance.Delete%></asp:LinkButton>
                                                                                <cc1:ConfirmButtonExtender ID="confirm2" runat="server" ConfirmText="<%$ Resources:Attendance,Are you sure you want to delete the record?%>"
                                                                                    TargetControlID="IbtnDelete">
                                                                                </cc1:ConfirmButtonExtender>
                                                                            </li>


                                                                        </ul>
                                                                    </div>

                                                                </ItemTemplate>
                                                            </asp:TemplateField>

                                                            <asp:TemplateField Visible="false" HeaderText="<%$ Resources:Attendance,Department Id %>"
                                                                SortExpression="Dep_Id">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblDepartmentId1" runat="server" Text='<%# Eval("Dep_Id") %>'></asp:Label>
                                                                </ItemTemplate>
                                                                <ItemStyle />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,Department Name %>" SortExpression="Dep_Name">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lbEDepName" runat="server" Text='<%# Eval("Dep_Name") %>'></asp:Label>
                                                                </ItemTemplate>
                                                                <ItemStyle />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,Department Name(Local) %>"
                                                                SortExpression="Dep_Name_L">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblDepName" runat="server" Text='<%# Eval("Dep_Name_L") %>'></asp:Label>
                                                                </ItemTemplate>
                                                                <ItemStyle />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,Department Code %>" SortExpression="Dep_Code">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblLocCode" runat="server" Text='<%# Eval("Dep_Code") %>'></asp:Label>
                                                                </ItemTemplate>
                                                                <ItemStyle />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField Visible="false" HeaderText="<%$ Resources:Attendance,Parent Department Name %>"
                                                                SortExpression="ParentDepartment">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblParentLoc" runat="server" Text='<%# Eval("ParentDepartment") %>'></asp:Label>
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
                                                    <asp:TreeView ID="TreeViewDepartment" runat="server" Visible="false" OnSelectedNodeChanged="TreeViewDepartment_SelectedNodeChanged">
                                                    </asp:TreeView>
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
                                    <div id="Main_Div" runat="server">
                                        <div class="col-md-12">
                                            <div class="col-md-6">
                                                <asp:Label ID="lblDeptCode" runat="server" Text="<%$ Resources:Attendance,Department Code%>"></asp:Label>
                                                <a style="color: Red">*</a>
                                                <asp:RequiredFieldValidator EnableClientScript="true" Style="float: right;" runat="server" ID="RequiredFieldValidator3" ValidationGroup="Save"
                                                    Display="Dynamic" SetFocusOnError="true" ControlToValidate="txtDeptCode" ErrorMessage="<%$ Resources:Attendance,Enter Department Code%>"></asp:RequiredFieldValidator>

                                                <asp:TextBox ID="txtDeptCode" runat="server" AutoPostBack="true" OnTextChanged="txtDeptCode_OnTextChanged"
                                                    BackColor="#eeeeee" CssClass="form-control" />
                                                <cc1:AutoCompleteExtender ID="AutoCompleteExtender2" runat="server" DelimiterCharacters=""
                                                    Enabled="True" ServiceMethod="GetCompletionListDepCode" ServicePath="" CompletionInterval="100"
                                                    MinimumPrefixLength="1" CompletionSetCount="1" TargetControlID="txtDeptCode"
                                                    UseContextKey="True" CompletionListCssClass="autoCompleteList" CompletionListItemCssClass="autoCompleteListItem" CompletionListHighlightedItemCssClass="autoCompleteSelectedListItem" OnClientShown="resetPosition">
                                                </cc1:AutoCompleteExtender>
                                            </div>
                                            <div class="col-md-6">
                                                <asp:Label ID="lblDeptName" runat="server" Text="<%$ Resources:Attendance,Department Name%>"></asp:Label>
                                                <a style="color: Red">*</a>
                                                <asp:RequiredFieldValidator EnableClientScript="true" Style="float: right;" runat="server" ID="RequiredFieldValidator1" ValidationGroup="Save"
                                                    Display="Dynamic" SetFocusOnError="true" ControlToValidate="txtDeptName" ErrorMessage="<%$ Resources:Attendance,Enter Department Name%>"></asp:RequiredFieldValidator>

                                                <asp:TextBox ID="txtDeptName" OnTextChanged="txtDepName_OnTextChanged" runat="server"
                                                    CssClass="form-control"></asp:TextBox>
                                                <cc1:AutoCompleteExtender ID="AutoCompleteExtender1" runat="server" DelimiterCharacters=""
                                                    Enabled="True" ServiceMethod="GetCompletionListDepName" ServicePath="" CompletionInterval="100"
                                                    MinimumPrefixLength="1" CompletionSetCount="1" TargetControlID="txtDeptName"
                                                    UseContextKey="True" CompletionListCssClass="autoCompleteList" CompletionListItemCssClass="autoCompleteListItem" CompletionListHighlightedItemCssClass="autoCompleteSelectedListItem" OnClientShown="resetPosition">
                                                </cc1:AutoCompleteExtender>
                                            </div>
                                        </div>
                                        <div class="col-md-12">
                                            <div class="col-md-6">
                                                <asp:Label ID="lblDeptNameL" runat="server" Text="<%$ Resources:Attendance,Department Name(Local)%>"></asp:Label>
                                                <asp:TextBox ID="txtDeptNameL" runat="server" CssClass="form-control"></asp:TextBox>
                                            </div>
                                            <div class="col-md-6">
                                                <asp:Label ID="lblParentDep" runat="server" Text="<%$ Resources:Attendance,Parent Department Name%>"></asp:Label>
                                                <asp:DropDownList ID="ddlParentDep" runat="server" CssClass="form-control">
                                                </asp:DropDownList>

                                            </div>
                                        </div>
                                        <div style="display: none">
                                            <asp:Label ID="lblManager" Visible="false" runat="server" Text="<%$ Resources:Attendance,Manager%>"></asp:Label>
                                            <asp:TextBox ID="txtregistrationNo" Visible="false" runat="server" CssClass="form-control"></asp:TextBox>
                                            <asp:Label ID="Label9" runat="server" Visible="false" Text="<%$ Resources:Attendance,Driver Name%>"></asp:Label>
                                            <asp:TextBox ID="txtManagerName" Visible="false" runat="server" CssClass="form-control"
                                                BackColor="#eeeeee"></asp:TextBox>
                                            <cc1:AutoCompleteExtender ID="AutoCompleteExtender5" runat="server" DelimiterCharacters=""
                                                Enabled="True" ServiceMethod="GetCompletionListEmployeeName" ServicePath="" CompletionInterval="100"
                                                MinimumPrefixLength="1" CompletionSetCount="1" TargetControlID="txtManagerName"
                                                UseContextKey="True" CompletionListCssClass="autoCompleteList" CompletionListItemCssClass="autoCompleteListItem" CompletionListHighlightedItemCssClass="autoCompleteSelectedListItem" OnClientShown="resetPosition">
                                            </cc1:AutoCompleteExtender>
                                            <asp:Label ID="lblPhoneNo" Visible="false" runat="server" Text="<%$ Resources:Attendance,Phone No.%>"></asp:Label>
                                            <asp:TextBox ID="txtPhoneNo" Visible="false" runat="server" CssClass="form-control"></asp:TextBox>
                                            <asp:Label ID="lblFax" Visible="false" runat="server" Text="<%$ Resources:Attendance,Fax No.%>"></asp:Label>
                                            <asp:TextBox ID="txtFax" Visible="false" runat="server" CssClass="form-control"></asp:TextBox>



                                        </div>

                                        <div style="text-align: center" class="col-md-12">
                                            <br />
                                            <asp:Button ID="btnSave" runat="server" Text="<%$ Resources:Attendance,Save %>" Visible="false"
                                                CssClass="btn btn-success" ValidationGroup="Save" OnClick="btnSave_Click" />
                                            <asp:Button ID="btnReset" runat="server" Text="<%$ Resources:Attendance,Reset %>"
                                                CssClass="btn btn-primary" CausesValidation="False" OnClick="btnReset_Click" />
                                            <asp:Button ID="btnCancel" runat="server" Text="<%$ Resources:Attendance,Cancel %>"
                                                CssClass="btn btn-danger" CausesValidation="False" OnClick="btnCancel_Click" />
                                            <asp:HiddenField ID="editid" runat="server" />
                                        </div>

                                        <div style="display: none">
                                            <asp:Button ID="btnDeleteChild" runat="server" Text="<%$ Resources:Attendance, Delete Child %>"
                                                CssClass="btn btn-primary" OnClick="btnDeleteChild_Click" />
                                            <asp:Button ID="btnBack" runat="server" Text="<%$ Resources:Attendance, Back %>"
                                                CssClass="btn btn-primary" OnClick="btnBack_Click" />
                                            <asp:Button ID="btnMoveChild" runat="server" Text="<%$ Resources:Attendance, Move Child %>"
                                                CssClass="btn btn-primary" OnClick="btnMoveChild_Click" />
                                            <asp:Label ID="Label16" runat="server" Text="<%$ Resources:Attendance,Parent Category %>"></asp:Label>
                                            <asp:DropDownList ID="ddlMoveCategory" runat="server" CssClass="form-control">
                                            </asp:DropDownList>
                                            <asp:Button ID="btnUpdateParent" runat="server" Text="<%$ Resources:Attendance, Move %>"
                                                CssClass="btn btn-primary" OnClick="btnUpdateParent_Click" />
                                        </div>
                                    </div>

                                    <div id="Div_Move" runat="server" visible="false" class="col-md-12">
                                        <div class="col-md-6" style="text-align: center">
                                            <asp:Label runat="server" Font-Bold="true"
                                                ID="Label1" Text="<%$ Resources:Attendance,Department Code %>"></asp:Label>
                                            &nbsp:&nbsp<asp:Label runat="server" Style="margin-left: 10px;" Font-Bold="true"
                                                ID="lblDeptId1"></asp:Label>
                                        </div>
                                        <div class="col-md-6" style="text-align: center">
                                            <asp:Label runat="server" Font-Bold="true"
                                                ID="Label2" Text="<%$ Resources:Attendance,Department %>"></asp:Label>
                                            &nbsp:&nbsp<asp:Label runat="server" Style="margin-left: 10px;" Font-Bold="true"
                                                ID="lblDeptName1"></asp:Label>
                                            <br />
                                        </div>
                                        <div class="col-md-12" style="text-align: center">
                                            <br />
                                            <hr />
                                            <asp:Label runat="server" Font-Bold="true" ID="lblSelectDept" Text="<%$ Resources:Attendance,Select Location %>"></asp:Label>
                                            <br />
                                        </div>
                                        <div class="col-md-12" style="text-align: center">
                                            <br />
                                        </div>
                                        <div class="col-md-12">
                                            <div class="col-md-2"></div>
                                            <div class="col-md-3">
                                                <asp:ListBox ID="lstLocation" runat="server" Style="width: 100%;" Height="200px"
                                                    SelectionMode="Multiple" Font-Names="Trebuchet MS" Font-Size="Small" ForeColor="Gray"></asp:ListBox>
                                            </div>
                                            <div class="col-lg-2" style="text-align: center">
                                                <div style="margin-top: 30px; margin-bottom: 30px;" class="btn-group-vertical">
                                                    <asp:Button ID="btnPushLoc" class="btn btn-info" runat="server" Text=">" OnClick="btnPushLoc_Click" />
                                                    <asp:Button ID="btnPullLoc" runat="server" class="btn btn-info" Text="<" OnClick="btnPullLoc_Click" />
                                                    <asp:Button ID="btnPushAllLoc" runat="server" class="btn btn-info" Text=">>" OnClick="btnPushAllLoc_Click" />
                                                    <asp:Button ID="btnPullAllLoc" runat="server" class="btn btn-info" Text="<<" OnClick="btnPullAllLoc_Click" />
                                                </div>
                                            </div>
                                            <div class="col-md-3">
                                                <asp:ListBox ID="lstLocationSelect" Style="width: 100%;" runat="server" Height="200px"
                                                    SelectionMode="Multiple" Font-Bold="true" Font-Names="Trebuchet MS" Font-Size="Small"
                                                    ForeColor="Gray"></asp:ListBox>
                                            </div>
                                            <div class="col-md-2"></div>
                                        </div>
                                        <div class="col-md-12" style="text-align: center">
                                            <br />
                                            <asp:Button ID="btnSaveLoc" runat="server" Text="<%$ Resources:Attendance,Save %>"
                                                CssClass="btn btn-success" ValidationGroup="Save" OnClick="btnSaveLoc_Click" />
                                            <asp:Button ID="btnResetLoc" runat="server" Text="<%$ Resources:Attendance,Reset %>"
                                                CssClass="btn btn-primary" CausesValidation="False" OnClick="btnResetLoc_Click" />
                                            <asp:Button ID="btnCancelLoc" runat="server" Text="<%$ Resources:Attendance,Cancel %>"
                                                CssClass="btn btn-danger" CausesValidation="False" OnClick="btnCancelLoc_Click" />

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
                                                    <asp:Label ID="Label4" runat="server" Text="Advance Search"></asp:Label></h3>
                                                &nbsp;&nbsp;|&nbsp;&nbsp;
					<asp:Label ID="lblbinTotalRecords" runat="server" Text="<%$ Resources:Attendance,Total Records: 0 %>"></asp:Label>

                                                <div class="box-tools pull-right">
                                                    <button type="button" class="btn btn-box-tool" data-widget="collapse">
                                                        <i id="I2" runat="server" class="fa fa-plus"></i>
                                                    </button>
                                                </div>
                                            </div>
                                            <div class="box-body">
                                                <div class="col-lg-3">
                                                    <asp:DropDownList ID="ddlbinFieldName" runat="server" CssClass="form-control">
                                                        <asp:ListItem Selected="True" Text="<%$ Resources:Attendance,Department Name %>"
                                                            Value="Dep_Name"></asp:ListItem>
                                                        <asp:ListItem Text="<%$ Resources:Attendance,Department Name(Local) %>" Value="Dep_Name_L"></asp:ListItem>
                                                        <asp:ListItem Text="<%$ Resources:Attendance,Department Code %>" Value="Dep_Code"></asp:ListItem>
                                                        <asp:ListItem Text="<%$ Resources:Attendance,Created By %>" Value="Created_User"></asp:ListItem>
                                                        <asp:ListItem Text="<%$ Resources:Attendance,Modified By %>" Value="Modified_User"></asp:ListItem>
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
                                                <div class="col-lg-4">
                                                    <asp:Panel ID="Panel2" runat="server" DefaultButton="btnbinbind">
                                                        <asp:TextBox ID="txtbinValue" placeholder="Search from Content" runat="server" CssClass="form-control"></asp:TextBox>
                                                    </asp:Panel>
                                                </div>
                                                <div class="col-lg-3">
                                                    <asp:LinkButton ID="btnbinbind" runat="server" CausesValidation="False"
                                                        OnClick="btnbinbind_Click"><span class="fa fa-search"  style="font-size:25px;"></span></asp:LinkButton>&nbsp;&nbsp;&nbsp;

                                                    <asp:LinkButton ID="btnbinRefresh" runat="server" CausesValidation="False"
                                                        OnClick="btnbinRefresh_Click"><span class="fa fa-repeat"  style="font-size:25px;"></span></asp:LinkButton>&nbsp;&nbsp;&nbsp;

                                                    <asp:LinkButton ID="imgBtnRestore" CausesValidation="False"
                                                        Visible="false" runat="server" OnClick="imgBtnRestore_Click"><span class="far fa-lightbulb"  style="font-size:25px;"></span></asp:LinkButton>&nbsp;&nbsp;&nbsp;

                                                    <asp:ImageButton ID="ImgbtnSelectAll" Visible="false" Style="width: 33px;" runat="server" OnClick="ImgbtnSelectAll_Click"
                                                        ToolTip="<%$ Resources:Attendance, Select All %>" AutoPostBack="true"
                                                        ImageUrl="~/Images/selectAll.png" />
                                                </div>


                                            </div>
                                        </div>
                                    </div>
                                </div>


                                <div class="box box-warning box-solid" <%= gvDepMasterBin.Rows.Count>0?"style='display:block'":"style='display:none'"%>>

                                    <div class="box-body">
                                        <div class="row">
                                            <div class="col-md-12">
                                                <div class="flow">

                                                    <asp:GridView CssClass="table-striped table-bordered table table-hover" ID="gvDepMasterBin" PageSize="<%# PageControlCommon.GetPageSize() %>"
                                                        runat="server" AutoGenerateColumns="False" Width="100%" AllowPaging="True" OnPageIndexChanging="gvDepMasterBin_PageIndexChanging"
                                                        OnSorting="gvDepMasterBin_OnSorting" DataKeyNames="Dep_Id" AllowSorting="true">
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
                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,Department Id %>" SortExpression="Dep_Id">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblDepartmentId1" runat="server" Text='<%# Eval("Dep_Id") %>'></asp:Label>
                                                                </ItemTemplate>
                                                                <ItemStyle />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,Department Name %>" SortExpression="Dep_Name">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lbEDepName" runat="server" Text='<%# Eval("Dep_Name") %>'></asp:Label>
                                                                    <asp:Label ID="lblDepId" runat="server" Visible="false" Text='<%# Eval("Dep_Id") %>'></asp:Label>
                                                                </ItemTemplate>
                                                                <ItemStyle />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,Department Name(Local) %>"
                                                                SortExpression="Dep_Name_L">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblDepName" runat="server" Text='<%# Eval("Dep_Name_L") %>'></asp:Label>
                                                                </ItemTemplate>
                                                                <ItemStyle />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,Department Code %>" SortExpression="Dep_Code">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblLocCode" runat="server" Text='<%# Eval("Dep_Code") %>'></asp:Label>
                                                                </ItemTemplate>
                                                                <ItemStyle />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField Visible="false" HeaderText="<%$ Resources:Attendance,Parent Department Name %>"
                                                                SortExpression="ParentDepartment">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblParentLoc" runat="server" Text='<%# Eval("ParentDepartment") %>'></asp:Label>
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
            </div>
        </div>
    </div>


    <asp:UpdateProgress ID="UpdateProgress10" runat="server" AssociatedUpdatePanelID="Update_New">
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

    <asp:UpdateProgress ID="UpdateProgress3" runat="server" AssociatedUpdatePanelID="Update_Bin">
        <ProgressTemplate>
            <div class="modal_Progress">
                <div class="center_Progress">
                    <img alt="" src="../Images/ajax-loader2.gif" style="vertical-align: middle; background-color: White; border-radius: 10px;" />
                </div>
            </div>
        </ProgressTemplate>
    </asp:UpdateProgress>

    <asp:Panel ID="pnlMoveChild" runat="server" Visible="false"></asp:Panel>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="FooterContent" runat="Server">
    
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="AfterLoadScriptContent" runat="Server">
    <script src="../Script/common.js"></script>
    <script type="text/javascript">
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

    </script>
</asp:Content>




