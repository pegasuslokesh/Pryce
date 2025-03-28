<%@ Page Title="" Language="C#" MasterPageFile="~/ERPMaster.master" AutoEventWireup="true" CodeFile="GroupMaster.aspx.cs" Inherits="EMS_GroupMaster" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="Head" runat="Server">
    <link href="../Bootstrap_Files/Additional/Popup_Style.css" rel="stylesheet" type="text/css" />
    <link href="../Bootstrap_Files/Additional/Button_Style.css" rel="stylesheet" type="text/css" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="HeaderContent" runat="Server">
    <h1>
       <i class="fas fa-id-card"></i>&nbsp;&nbsp;
        <asp:Label ID="lblHeader" runat="server" Text="<%$ Resources:Attendance,Contact Group Setup%>"></asp:Label>
    </h1>
    <ol class="breadcrumb">
        <li><a href="#"><i class="fa fa-dashboard"></i>
            <asp:Label ID="Label6" runat="server" Text="<%$ Resources:Attendance,Master Setup%>"></asp:Label></a></li>
        <li><a href="#"><i class="fa fa-dashboard"></i>
            <asp:Label ID="Label10" runat="server" Text="<%$ Resources:Attendance,Master SetUp%>"></asp:Label></a></li>
        <li class="active">
            <asp:Label ID="Label5" runat="server" Text="<%$ Resources:Attendance,Contact Group Setup%>"></asp:Label></li>
    </ol>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="Server">

    <asp:UpdatePanel ID="Update_Button" runat="server">
        <ContentTemplate>
            <asp:Button ID="Btn_Bin" Style="display: none;" runat="server" OnClick="btnBin_Click" Text="Bin" />
            <asp:Button ID="Btn_Address_Modal" Style="display: none;" runat="server" data-toggle="modal" data-target="#myModal" Text="Address" />
            <asp:HiddenField runat="server" ID="hdnCanEdit" />
            <asp:HiddenField runat="server" ID="hdnCanDelete" />
        </ContentTemplate>
    </asp:UpdatePanel>
    <div class="row">
        <div class="col-md-12">
            <div class="nav-tabs-custom">
                <ul class="nav nav-tabs pull-right bg-blue-gradient">
                    <li id="Li_Bin"><a onclick="Li_Tab_Bin()" href="#Bin" data-toggle="tab">
                        <i class="fa fa-trash"></i>&nbsp;&nbsp;<asp:Label ID="Label3" runat="server" Text="<%$ Resources:Attendance,Bin%>"></asp:Label></a></li>
                    <li id="Li_New"><a href="#New" data-toggle="tab">
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
                                        <div class="row">
                                            <div class="col-md-12">
                                                <div id="Div1" runat="server" class="box box-info collapsed-box">
                                                    <div class="box-header with-border">
                                                        <h3 class="box-title">
                                                            <asp:Label ID="Label22" runat="server" Text="Advance Search"></asp:Label></h3>
                                                        &nbsp;&nbsp;|&nbsp;&nbsp;
					<asp:Label ID="lblTotalRecords" Text="<%$ Resources:Attendance,Total Records: 0 %>" runat="server"></asp:Label>

                                                        <div class="box-tools pull-right">
                                                            <button type="button" class="btn btn-box-tool" data-widget="collapse">
                                                                <i id="I1" runat="server" class="fa fa-plus"></i>
                                                            </button>
                                                        </div>
                                                    </div>
                                                    <div class="box-body">
                                                        <div class="col-lg-3">
                                                            <asp:DropDownList ID="ddlFieldName" runat="server" CssClass="form-control">
                                                                <asp:ListItem Text="<%$ Resources:Attendance,Group Id %>" Value="Group_Id"></asp:ListItem>
                                                                <asp:ListItem Text="<%$ Resources:Attendance,Group Name %>" Value="Group_Name" Selected="True"></asp:ListItem>
                                                                <asp:ListItem Text="<%$ Resources:Attendance,Parent Group %>" Value="ParentGroupName"></asp:ListItem>
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
                                                            <asp:Panel ID="Panel2" runat="server" DefaultButton="btnbind">
                                                                <asp:TextBox ID="txtValue" runat="server" placeholder="Search from Content" CssClass="form-control"></asp:TextBox>
                                                            </asp:Panel>
                                                        </div>
                                                        <div class="col-lg-2" style="text-align: center;">
                                                            <asp:LinkButton ID="btnbind" runat="server" CausesValidation="False" OnClick="btnbind_Click" ToolTip="<%$ Resources:Attendance,Search %>"> <span class="fa fa-search"  style="font-size:25px;"></span></asp:LinkButton>&nbsp;&nbsp;&nbsp;
                                                            <asp:LinkButton ID="btnRefresh" runat="server" CausesValidation="False" OnClick="btnRefreshReport_Click" ToolTip="<%$ Resources:Attendance,Refresh %>"><span class="fa fa-repeat"  style="font-size:25px;"></span></asp:LinkButton>&nbsp;&nbsp;&nbsp;
                                                            <asp:LinkButton ID="btnGridView" runat="server" CausesValidation="False" Visible="false" OnClick="btnGridView_Click" ToolTip="<%$ Resources:Attendance, Tree View %>"><span class="fa fa-sitemap"  style="font-size:25px;"></span></asp:LinkButton>
                                                        </div>
                                                        <asp:HiddenField ID="HDFSort" runat="server" />
                                                        <asp:HiddenField ID="hdntxtaddressid" runat="server" />
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="box box-warning box-solid" <%= GvGroup.Rows.Count>0?"style='display:block'":"style='display:none'"%>>
                                            <div class="box-body">
                                                <div class="row">
                                                    <div class="col-md-12">
                                                        <div style="overflow: auto">
                                                            <asp:GridView CssClass="table-striped table-bordered table table-hover" ID="GvGroup" PageSize="<%# PageControlCommon.GetPageSize() %>" runat="server"
                                                                AutoGenerateColumns="False" Width="100%" AllowPaging="True" OnPageIndexChanging="GvGroup_PageIndexChanging"
                                                                OnSorting="GvGroup_OnSorting" AllowSorting="true">
                                                                <Columns>


                                                                    <asp:TemplateField HeaderText="Action">
                                                                        <ItemTemplate>
                                                                            <div class="dropdown" style="position: absolute;">
                                                                                <button class="btn btn-default dropdown-toggle" type="button" data-toggle="dropdown">
                                                                                    <i class="fa fa-ellipsis-h" aria-hidden="true"></i>
                                                                                </button>
                                                                                <ul class="dropdown-menu">
                                                                                    <li <%= hdnCanEdit.Value=="true"?"style='display:block'":"style='display:none'" %>>
                                                                                        <asp:LinkButton ID="btnEdit" runat="server" CommandArgument='<%# Eval("Group_Id") %>' CausesValidation="False" OnCommand="btnEdit_Command"><i class="fa fa-pencil"></i>Edit </asp:LinkButton>
                                                                                    </li>
                                                                                    <li <%= hdnCanDelete.Value=="true"?"style='display:block'":"style='display:none'" %>>
                                                                                        <asp:LinkButton ID="IbtnDelete" runat="server" CausesValidation="False" CommandArgument='<%# Eval("Group_Id") %>' OnCommand="IbtnDelete_Command"><i class="fa fa-trash"></i>Delete</asp:LinkButton>
                                                                                        <cc1:ConfirmButtonExtender ID="confirm1" runat="server" ConfirmText="<%$ Resources:Attendance,Are you sure you want to delete the record?%>" TargetControlID="IbtnDelete"></cc1:ConfirmButtonExtender>
                                                                                    </li>
                                                                                </ul>
                                                                            </div>

                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>

                                                                    <asp:TemplateField HeaderText="<%$ Resources:Attendance,Group Id%>" SortExpression="Group_Id">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblGroupId" runat="server" Text='<%# Eval("Group_Id") %>'></asp:Label>
                                                                        </ItemTemplate>
                                                                        <ItemStyle HorizontalAlign="Center" />
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="<%$ Resources:Attendance,Group Name %>" SortExpression="Group_Name">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblGroupName" runat="server" Text='<%# Eval("Group_Name") %>'></asp:Label>
                                                                        </ItemTemplate>
                                                                        <ItemStyle HorizontalAlign="Center" />
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="<%$ Resources:Attendance,Parent Group %>" SortExpression="ParentGroupName">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblParentGName" runat="server" Text='<%# Eval("ParentGroupName") %>'></asp:Label>
                                                                        </ItemTemplate>
                                                                        <ItemStyle HorizontalAlign="Center" />
                                                                    </asp:TemplateField>
                                                                </Columns>

                                                                <PagerStyle CssClass="pagination-ys" />

                                                            </asp:GridView>
                                                            <asp:HiddenField ID="HiddenField1" runat="server" />
                                                            <asp:TreeView ID="TreeViewCategory" runat="server" Visible="false" OnSelectedNodeChanged="TreeViewCategory_SelectedNodeChanged"
                                                                ExpandDepth="0">
                                                            </asp:TreeView>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>

                                <asp:Panel ID="panelOverlay" runat="server" class="Overlay" Visible="false">
                                    <asp:Panel ID="panelPopUpPanel" runat="server" class="PopUpPanel" Visible="false">
                                        <div class="row">
                                            <div class="col-md-12">
                                                <table width="100%" style="background-image: url(../Images/bg_repeat.jpg); background-repeat: repeat;">
                                                    <tr>
                                                        <td align="left">
                                                            <asp:Label ID="lblDeletePanelHeader" runat="server" Font-Size="14px" Font-Bold="true"
                                                                Text="<%$ Resources:Attendance, Delete Group %>"></asp:Label>
                                                        </td>
                                                        <td align="right">
                                                            <asp:ImageButton ID="btnClosePanel" runat="server" ImageUrl="~/Images/close.png"
                                                                CausesValidation="False" OnClick="btnClosePanel_Click" Height="20px" Width="20px" />
                                                        </td>
                                                    </tr>
                                                </table>
                                                <div id="trgv" runat="server" visible="false">
                                                    <asp:Label ID="Label4" runat="server" Text="<%$ Resources:Attendance,Group Id%>"></asp:Label>
                                                    &nbsp : &nbsp
                                        <asp:Label ID="lblDelGroupId" runat="server" />
                                                    <br />
                                                </div>
                                                <asp:Label ID="Label8" runat="server" Text="<%$ Resources:Attendance,Group Name%>"></asp:Label>
                                                &nbsp : &nbsp
                                        <asp:Label ID="lblDelGroupName" runat="server" />
                                                <br />
                                                <div id="rowDelParentCategory" runat="server">
                                                    <asp:Label ID="Label9" runat="server" Text="<%$ Resources:Attendance,Parent Group%>"></asp:Label>
                                                    &nbsp : &nbsp
                                        <asp:Label ID="lblDelParentGroup" runat="server" />
                                                    <br />
                                                </div>
                                                <div id="trdel" runat="server">
                                                    <div id="Td2" runat="server">
                                                        <asp:RadioButton ID="rdbtndelchild" runat="server" AutoPostBack="True"
                                                            OnCheckedChanged="rdbtndelchild_CheckedChanged" Text="<%$ Resources:Attendance,Delete Child Also%>"
                                                            GroupName="a" Visible="false" />
                                                        <asp:RadioButton ID="rbtnmovechild" runat="server" AutoPostBack="True"
                                                            OnCheckedChanged="rbtnmovechild_CheckedChanged" Text="<%$ Resources:Attendance,Move Child%>"
                                                            GroupName="a" Visible="false" />
                                                    </div>
                                                </div>
                                                <div id="trdel2" runat="server">
                                                    <div id="Td3" runat="server" style="text-align: center;">
                                                        <asp:DropDownList ID="ddlgroup0" runat="server" CssClass="form-control" Width="200px"
                                                            Visible="false" />
                                                    </div>
                                                </div>



                                                <div style="text-align: center;">
                                                    <asp:Button ID="btnDeleteNode" runat="server" Text="<%$ Resources:Attendance, Delete %>"
                                                        Visible="true" CssClass="btn btn-primary" OnClick="btnDeleteNode_Click" CausesValidation="false" />
                                                    <asp:Button ID="btnCancelDelete" runat="server" Text="<%$ Resources:Attendance, Cancel %>"
                                                        CssClass="btn btn-primary" OnClick="btnCancelDelete_Click" CausesValidation="False" />
                                                </div>
                                            </div>
                                        </div>
                                    </asp:Panel>
                                </asp:Panel>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </div>

                    <div class="tab-pane" id="New">
                        <asp:UpdatePanel ID="Update_New" runat="server">
                            <ContentTemplate>
                                <div runat="server" id="pnlLoc" class="row">
                                    <div class="col-md-12">
                                        <div class="box box-danger">
                                            <div class="box-header with-border">
                                                <div class="col-md-6">
                                                    <asp:Label ID="lblHolidayName" runat="server" Text="<%$ Resources:Attendance,Group Name %>"></asp:Label>
                                                    <a style="color: Red">*</a>
                                                    <asp:RequiredFieldValidator EnableClientScript="true" Style="float: right;" runat="server" ID="RequiredFieldValidator1" ValidationGroup="Group_Save"
                                                        Display="Dynamic" SetFocusOnError="true" ControlToValidate="txtGroupName" ErrorMessage="<%$ Resources:Attendance,Enter Group Name %>" />
                                                    <asp:TextBox ID="txtGroupName" runat="server" BackColor="#eeeeee"
                                                        CssClass="form-control" AutoPostBack="true" OnTextChanged="txtGroupName_TextChanged" />
                                                    <cc1:AutoCompleteExtender ID="txtGroupName_AutoCompleteExtender" runat="server" DelimiterCharacters=""
                                                        Enabled="True" ServiceMethod="GetCompletionList" ServicePath="" CompletionInterval="100"
                                                        MinimumPrefixLength="1" CompletionSetCount="1" TargetControlID="txtGroupName"
                                                        UseContextKey="True" CompletionListCssClass="autoCompleteList" CompletionListItemCssClass="autoCompleteListItem" CompletionListHighlightedItemCssClass="autoCompleteSelectedListItem" OnClientShown="resetPosition">
                                                    </cc1:AutoCompleteExtender>
                                                    <br />
                                                </div>
                                                <div class="col-md-6">
                                                    <asp:Label ID="Lbl_group_name" runat="server" Text="<%$ Resources:Attendance,Group Name(Local) %>"></asp:Label>
                                                    <asp:TextBox ID="txtLGroupName" runat="server" CssClass="form-control" />
                                                    <br />
                                                </div>
                                                <div class="col-md-6">

                                                    <asp:Label ID="lblHolidayNameL" runat="server" Text="<%$ Resources:Attendance,Parent Group %>"></asp:Label>
                                                    <asp:DropDownList ID="ddlPerentGroup" runat="server" CssClass="form-control"></asp:DropDownList>
                                                    <br />
                                                </div>
                                                <div class="col-md-6">
                                                    <br />
                                                    <asp:CheckBox ID="chkShowInPartner" runat="server" Text="Show In Partner Management" />
                                                    <br />
                                                </div>
                                                <div class="col-md-12" style="text-align: center">
                                                    <br />
                                                    <asp:Button ID="btnSave" runat="server" Text="<%$ Resources:Attendance, Save %>"
                                                        CssClass="btn btn-success" ValidationGroup="Group_Save" OnClick="btnSave_Click" Visible="false" />

                                                    <asp:Button ID="btnReset" runat="server" Text="<%$ Resources:Attendance, Reset %>"
                                                        CssClass="btn btn-primary" OnClick="btnReset_Click" CausesValidation="False" />
                                                    <asp:Button ID="btnCancel" runat="server" Text="<%$ Resources:Attendance, Cancel %>"
                                                        CssClass="btn btn-danger" OnClick="btnCancel_Click" CausesValidation="False" />
                                                    <asp:HiddenField ID="editid" runat="server" />
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
                                        <div class="row">
                                            <div class="col-md-12">
                                                <div id="Div2" runat="server" class="box box-info collapsed-box">
                                                    <div class="box-header with-border">
                                                        <h3 class="box-title">
                                                            <asp:Label ID="Label1" runat="server" Text="Advance Search"></asp:Label></h3>
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
                                                                <asp:ListItem Text="<%$ Resources:Attendance,Group Id %>" Value="Group_Id"></asp:ListItem>
                                                                <asp:ListItem Text="<%$ Resources:Attendance,Group Name %>" Value="Group_Name" Selected="True"></asp:ListItem>
                                                                <asp:ListItem Text="<%$ Resources:Attendance,Parent Group %>" Value="ParentGroupName"></asp:ListItem>
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
                                                            <asp:Panel ID="Panel1" runat="server" DefaultButton="btnbindBin">
                                                                <asp:TextBox ID="txtValueBin" class="form-control" placeholder="Search from Content"  runat="server"></asp:TextBox>
                                                            </asp:Panel>

                                                        </div>
                                                        <div class="col-lg-2" style="text-align: center;">
                                                            <asp:LinkButton ID="btnbindBin" runat="server" CausesValidation="False" OnClientClick="tab_3_open()" OnClick="btnbindBin_Click" ToolTip="<%$ Resources:Attendance,Search %>"><span class="fa fa-search"  style="font-size:25px;"></span></asp:LinkButton>&nbsp;&nbsp;&nbsp;
                                                            <asp:LinkButton ID="btnRefreshBin" runat="server"  CausesValidation="False"  OnClientClick="tab_3_open()" OnClick="btnRefreshBin_Click" ToolTip="<%$ Resources:Attendance,Refresh %>"><span class="fa fa-repeat"  style="font-size:25px;"></span></asp:LinkButton>&nbsp;&nbsp;&nbsp;
                                                            <asp:LinkButton ID="btnRestoreSelected" runat="server" CausesValidation="False" OnCommand="btnRestoreSelected_Click" ToolTip="<%$ Resources:Attendance, Active %>" Visible="false" ><span class="far fa-lightbulb"  style="font-size:25px;"></span></asp:LinkButton>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="box box-warning box-solid" <%= GvGroupBin.Rows.Count>0?"style='display:block'":"style='display:none'"%>>
                                             <div class="box-body">
                                                <div class="row">
                                                    <div class="col-md-12">
                                                        <div style="overflow: auto">
                                                            <asp:Label ID="lblSelectedRecord" runat="server" Visible="false"></asp:Label>
                                                            <asp:GridView CssClass="table-striped table-bordered table table-hover" ID="GvGroupBin" PageSize="<%# PageControlCommon.GetPageSize() %>" runat="server" AutoGenerateColumns="False"
                                                                Width="100%" AllowPaging="True" OnPageIndexChanging="GvGroupBin_PageIndexChanging"
                                                                OnSorting="GvGroupBin_OnSorting" AllowSorting="true">
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
                                                                    <asp:TemplateField HeaderText="<%$ Resources:Attendance,Group Id%>" SortExpression="Group_Id">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblgvBGroupId" runat="server" Text='<%# Eval("Group_Id") %>'></asp:Label>
                                                                        </ItemTemplate>
                                                                        <ItemStyle />
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="<%$ Resources:Attendance,Group Name %>" SortExpression="Group_Name">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblGroupName" runat="server" Text='<%# Eval("Group_Name") %>'></asp:Label>
                                                                        </ItemTemplate>
                                                                        <ItemStyle />
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="<%$ Resources:Attendance,Parent Group %>" SortExpression="ParentGroupName">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblParentGName" runat="server" Text='<%# Eval("ParentGroupName") %>'></asp:Label>
                                                                        </ItemTemplate>
                                                                        <ItemStyle HorizontalAlign="Center" />
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
    </div>


    <asp:UpdateProgress ID="UpdateProgress5" runat="server" AssociatedUpdatePanelID="Update_Button">
        <ProgressTemplate>
            <div class="modal_Progress">
                <div class="center_Progress">
                    <img alt="" src="../Images/ajax-loader2.gif" style="vertical-align: middle; background-color: White; border-radius: 10px;" />
                </div>
            </div>
        </ProgressTemplate>
    </asp:UpdateProgress>

    <asp:UpdateProgress ID="UpdateProgress8" runat="server" AssociatedUpdatePanelID="Update_List">
        <ProgressTemplate>
            <div class="modal_Progress">
                <div class="center_Progress">
                    <img alt="" src="../Images/ajax-loader2.gif" style="vertical-align: middle; background-color: White; border-radius: 10px;" />
                </div>
            </div>
        </ProgressTemplate>
    </asp:UpdateProgress>

    <asp:UpdateProgress ID="UpdateProgress6" runat="server" AssociatedUpdatePanelID="Update_New">
        <ProgressTemplate>
            <div class="modal_Progress">
                <div class="center_Progress">
                    <img alt="" src="../Images/ajax-loader2.gif" style="vertical-align: middle; background-color: White; border-radius: 10px;" />
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

        function Li_Tab_Bin() {
            document.getElementById('<%= Btn_Bin.ClientID %>').click();
        }
        function Add_Address() {
            document.getElementById('<%= Btn_Address_Modal.ClientID %>').click();
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

    </script>
</asp:Content>
