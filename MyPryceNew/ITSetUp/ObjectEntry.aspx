<%@ Page Title="" Language="C#" MasterPageFile="~/ERPMaster.master" AutoEventWireup="true"
    CodeFile="ObjectEntry.aspx.cs" Inherits="ITSetUp_ObjectEntry" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="HeaderContent" runat="Server">
    <h1>
        <img src="../Images/object_entry_setup.png" alt="" />
        <asp:Label ID="lblHeader" runat="server" Text="<%$ Resources:Attendance,Object Entry Setup%>"></asp:Label>
    </h1>
    <ol class="breadcrumb">
        <li><a href="#"><i class="fa fa-dashboard"></i>
            <asp:Label ID="Label6" runat="server" Text="<%$ Resources:Attendance,IT Security%>"></asp:Label></a></li>
        <li><a href="#"><i class="fa fa-dashboard"></i>
            <asp:Label ID="Label10" runat="server" Text="<%$ Resources:Attendance,IT Security%>"></asp:Label></a></li>
        <li class="active">
            <asp:Label ID="Label5" runat="server" Text="<%$ Resources:Attendance,Object Entry Setup%>"></asp:Label></li>
    </ol>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="Server">
    <asp:UpdatePanel ID="Update_Button" runat="server">
        <ContentTemplate>
            <asp:Button ID="Btn_Bin" Style="display: none;" runat="server" OnClick="btnBin_Click" Text="Bin" />
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
            <li style="display: none;" id="Li_Bin"><a onclick="Li_Tab_Bin()" href="#Bin" data-toggle="tab">
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
                                            <asp:Label ID="Label18" runat="server" Text="<%$ Resources:Attendance,Advance Search%>"></asp:Label></h3>
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
                                            <asp:DropDownList ID="ddlFieldName" runat="server" class="form-control">
                                                <asp:ListItem Selected="True" Text="<%$ Resources:Attendance,Object Name %>" Value="Object_Name"></asp:ListItem>
                                                <asp:ListItem Text="<%$ Resources:Attendance,Object Name(Local) %>" Value="Object_Name_L"></asp:ListItem>
                                                <asp:ListItem Text="<%$ Resources:Attendance,Application Name %>" Value="Application_Name"></asp:ListItem>
                                                <asp:ListItem Text="<%$ Resources:Attendance,Module Name %>" Value="Module_Name"></asp:ListItem>
                                                <asp:ListItem Text="<%$ Resources:Attendance,Object Id %>" Value="Object_Id"></asp:ListItem>
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
                                            <asp:GridView CssClass="table-striped table-bordered table table-hover" ID="gvObjectMaster" PageSize="<%# PageControlCommon.GetPageSize() %>"
                                                runat="server" AutoGenerateColumns="False" Width="100%" AllowPaging="True" AllowSorting="True"
                                                OnPageIndexChanging="gvObjectMaster_PageIndexChanging" OnSorting="gvObjectMaster_OnSorting">
                                                <Columns>

                                                    <asp:TemplateField HeaderText="<%$ Resources:Attendance,Action%>">
                                                        <ItemTemplate>
                                                            <div class="dropdown" style="position: absolute;">
                                                                <button class="btn btn-default dropdown-toggle"  type="button" data-toggle="dropdown">
                                                                    <i class="fa fa-ellipsis-h" aria-hidden="true"></i>
                                                                </button>
                                                                <ul class="dropdown-menu">

                                                                    <li <%= hdnCanEdit.Value=="true"?"style='display:block'":"style='display:none'" %>>
                                                                        <asp:LinkButton ID="btnEdit" runat="server" CommandArgument='<%# Eval("Object_Id") %>' CausesValidation="False" OnCommand="btnEdit_Command" ToolTip="<%$ Resources:Attendance,Edit %>"> <i class="fa fa-pencil"></i>Edit </asp:LinkButton>
                                                                    </li>
                                                                    <li <%= hdnCanDelete.Value=="true"?"style='display:block'":"style='display:none'" %>>
                                                                        <asp:LinkButton ID="IbtnDelete" runat="server" CausesValidation="False" CommandArgument='<%# Eval("Object_Id") %>' OnCommand="IbtnDelete_Command" ToolTip="<%$ Resources:Attendance,Delete %>"><i class="fa fa-trash"></i>Delete</asp:LinkButton>
                                                                        <cc1:ConfirmButtonExtender ID="confirm1" runat="server" ConfirmText="<%$ Resources:Attendance,Are you sure you want to delete the record?%>" TargetControlID="IbtnDelete"></cc1:ConfirmButtonExtender>
                                                                    </li>
                                                                </ul>
                                                            </div>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="<%$ Resources:Attendance,Object Id %>" SortExpression="Object_Id">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblObjectId1" runat="server" Text='<%# Eval("Object_Id") %>'></asp:Label>
                                                        </ItemTemplate>
                                                        <ItemStyle />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="<%$ Resources:Attendance,Object Name %>" SortExpression="Object_Name">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lbObjectName" runat="server" Text='<%# Eval("Object_Name") %>'></asp:Label>
                                                        </ItemTemplate>
                                                        <ItemStyle />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="<%$ Resources:Attendance,Object Name(Local) %>" SortExpression="Object_Name_L">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblObjectNameL" runat="server" Text='<%# Eval("Object_Name_L") %>'></asp:Label>
                                                        </ItemTemplate>
                                                        <ItemStyle />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="<%$ Resources:Attendance,Module Name %>" SortExpression="Module_Name">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblModuleName" runat="server" Text='<%# Eval("Module_Name") %>'></asp:Label>
                                                        </ItemTemplate>
                                                        <ItemStyle />
                                                    </asp:TemplateField>
                                                    <%--   <asp:TemplateField HeaderText="<%$ Resources:Attendance,Application Name %>" SortExpression="Application_Name">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblApplicationName" runat="server" Text='<%# Eval("Application_Name") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <ItemStyle  />
                                                </asp:TemplateField>--%>
                                                </Columns>

                                                <HeaderStyle CssClass="pagination-ys" />
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
                        <div class="box box-info">
                            <div class="box-body">
                                <div class="row">
                                    <div class="col-md-6">
                                        <asp:Label ID="lblObjectName" runat="server" Text="<%$ Resources:Attendance,Object Name %>" />
                                        <a style="color: Red">*</a>
                                        <asp:RequiredFieldValidator EnableClientScript="true" Style="float: right;" runat="server" ID="RequiredFieldValidator3" ValidationGroup="Save"
                                            Display="Dynamic" SetFocusOnError="true" ControlToValidate="txtObjectName" ErrorMessage="<%$ Resources:Attendance,Enter Object Name %>"></asp:RequiredFieldValidator>

                                        <asp:TextBox ID="txtObjectName" runat="server" TabIndex="101" Font-Names="Verdana"
                                            AutoPostBack="true" OnTextChanged="txtObjectName_TextChanged" CssClass="form-control"
                                            BackColor="#eeeeee"></asp:TextBox>
                                        <cc1:AutoCompleteExtender ID="autoComplete1" runat="server" DelimiterCharacters=""
                                            Enabled="True" ServiceMethod="GetCompletionListObjectName" ServicePath="" CompletionInterval="100"
                                            MinimumPrefixLength="1" CompletionSetCount="1" TargetControlID="txtObjectName"
                                            UseContextKey="True" CompletionListCssClass="autoCompleteList" CompletionListItemCssClass="autoCompleteListItem"
                                            CompletionListHighlightedItemCssClass="autoCompleteSelectedListItem" OnClientShown="resetPosition">
                                        </cc1:AutoCompleteExtender>
                                        <br />
                                        <asp:Label ID="Label2" runat="server" Text="<%$ Resources:Attendance,Page URL %>" />
                                        <asp:TextBox ID="txtPageUrl" runat="server" Font-Names="Verdana" TabIndex="103" CssClass="form-control"></asp:TextBox>
                                        <br />
                                        <asp:Label ID="Label12" runat="server" Text="<%$ Resources:Attendance,Application Name %>" />
                                        <asp:CheckBoxList ID="chkApplication" runat="server" RepeatColumns="3" Font-Names="Trebuchet MS"
                                            Font-Size="Small" CellPadding="5" ForeColor="Gray">
                                        </asp:CheckBoxList>
                                        <br />
                                    </div>
                                    <div class="col-md-6">
                                        <asp:Label ID="lblObjectNameL" runat="server" Text="<%$ Resources:Attendance,Object Name(Local) %>" />
                                        <asp:TextBox ID="txtObjectNameL" runat="server" TabIndex="102" CssClass="form-control"></asp:TextBox>
                                        <br />
                                        <asp:Label ID="Label11" runat="server" Text="<%$ Resources:Attendance,Sequence No. %>" />
                                        <asp:TextBox ID="txtOrderNo" runat="server" TabIndex="104" CssClass="form-control"></asp:TextBox>
                                        <cc1:FilteredTextBoxExtender ID="FiltertxtSquence" runat="server" TargetControlID="txtOrderNo"
                                            ValidChars="0,1,2,3,4,5,6,7,8,9">
                                        </cc1:FilteredTextBoxExtender>
                                        <br />
                                        <asp:Label ID="Label14" runat="server" Text="<%$ Resources:Attendance,Module Name %>" />
                                        <a style="color: Red">*</a>
                                        <asp:RequiredFieldValidator EnableClientScript="true" Style="float: right;" runat="server" ID="RequiredFieldValidator1" ValidationGroup="Save" Display="Dynamic"
                                            SetFocusOnError="true" ControlToValidate="ddlModule" InitialValue="--Select--" ErrorMessage="<%$ Resources:Attendance,Select Module Name %>" />
                                        <asp:DropDownList ID="ddlModule" runat="server" CssClass="form-control">
                                        </asp:DropDownList>
                                        <br />
                                    </div>
                                    <div class="col-md-12">
                                        <asp:Label ID="Label15" runat="server" Text="<%$ Resources:Attendance,Table Name%>" />
                                        <asp:TextBox ID="txtTablename" runat="server" CssClass="form-control"
                                            TextMode="MultiLine"></asp:TextBox>
                                        <cc1:AutoCompleteExtender ID="txtTablename_AutoCompleteExtender" runat="server" DelimiterCharacters=";"
                                            Enabled="True" CompletionInterval="100" CompletionSetCount="1" MinimumPrefixLength="1"
                                            ShowOnlyCurrentWordInCompletionListItem="true" ServiceMethod="GetContactList"
                                            ServicePath="" TargetControlID="txtTablename" UseContextKey="True" CompletionListCssClass="autoCompleteList" CompletionListItemCssClass="autoCompleteListItem"
                                            CompletionListHighlightedItemCssClass="autoCompleteSelectedListItem" OnClientShown="resetPosition">
                                        </cc1:AutoCompleteExtender>
                                        <br />
                                    </div>
                                    <div class="col-md-4">
                                        <asp:Label ID="Label16" runat="server" Text="<%$ Resources:Attendance,Approval Name%>" />
                                        <asp:DropDownList ID="ddlApprovalType" runat="server" CssClass="form-control">
                                        </asp:DropDownList>
                                        <br />
                                    </div>
                                    <div class="col-md-4">
                                        <asp:Label ID="Label3" runat="server" Text="<%$ Resources:Attendance,Notification Type%>" />
                                        <asp:DropDownList ID="DdlNotificationType" runat="server" CssClass="form-control">
                                        </asp:DropDownList>
                                        <br />
                                    </div>
                                    <div class="col-md-4">
                                        <br />
                                        <asp:CheckBox runat="server" ID="ChkShowInNavigationMenu" Checked="true" Text="<%$ Resources:Attendance,Show In Navigation Menu%>" />
                                        <br />
                                    </div>
                                    <div class="col-md-12">
                                        <asp:Label ID="Label17" runat="server" Text="<%$ Resources:Attendance,Operation Permission%>" />
                                        <div class="flow">
                                            <asp:CheckBoxList ID="chkOpeartionLIst" Width="100%" runat="server" RepeatColumns="2"
                                                Font-Names="Trebuchet MS" Font-Size="Small" CellPadding="10" ForeColor="Gray">
                                            </asp:CheckBoxList>
                                        </div>
                                    </div>
                                    <div class="col-md-12" style="text-align: center;">
                                        <asp:Button ID="btnSave" runat="server" Text="<%$ Resources:Attendance,Save %>" TabIndex="109"
                                            Visible="false" CssClass="btn btn-success" ValidationGroup="Save" OnClick="btnSave_Click" />
                                        <asp:Button ID="btnReset" runat="server" Text="<%$ Resources:Attendance,Reset %>"
                                            CssClass="btn btn-primary" TabIndex="110" CausesValidation="False" OnClick="btnReset_Click" />
                                        <asp:Button ID="btnCancel" runat="server" Text="<%$ Resources:Attendance,Cancel %>"
                                            CssClass="btn btn-danger" TabIndex="111" CausesValidation="False" OnClick="btnCancel_Click" />
                                        <asp:HiddenField ID="editid" runat="server" />
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
                        <div class="alert alert-info ">
                            <div class="row">
                                <div class="form-group">
                                    <div class="col-lg-3">
                                        <asp:DropDownList ID="ddlbinFieldName" runat="server" class="form-control">
                                            <asp:ListItem Selected="True" Text="<%$ Resources:Attendance,Object Name %>" Value="Object_Name"></asp:ListItem>
                                            <asp:ListItem Text="<%$ Resources:Attendance,Object Name(Local) %>" Value="Object_Name_L"></asp:ListItem>
                                            <asp:ListItem Text="<%$ Resources:Attendance,Object Id %>" Value="Object_Id"></asp:ListItem>
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
                                    <div class="col-lg-3">
                                        <asp:Panel ID="Panel2" runat="server" DefaultButton="btnbinbind">
                                            <asp:TextBox ID="txtbinValue" runat="server" CssClass="form-control"></asp:TextBox>
                                        </asp:Panel>
                                    </div>
                                    <div class="col-lg-2" style="text-align: center;">
                                        <asp:ImageButton ID="btnbinbind" Style="width: 35px; margin-top: -1px;" runat="server"
                                            CausesValidation="False" ImageUrl="~/Images/search.png" OnClick="btnbinbind_Click"
                                            ToolTip="<%$ Resources:Attendance,Search %>"></asp:ImageButton>
                                        <asp:ImageButton ID="btnbinRefresh" runat="server" Style="width: 30px;" CausesValidation="False"
                                            ImageUrl="~/Images/refresh.png" OnClick="btnbinRefresh_Click" ToolTip="<%$ Resources:Attendance,Refresh %>"></asp:ImageButton>
                                        <asp:ImageButton ID="imgBtnRestore" runat="server" Style="width: 30px;" CausesValidation="False"
                                            ImageUrl="~/Images/active.png" OnClick="imgBtnRestore_Click" ToolTip="<%$ Resources:Attendance,Refresh %>"></asp:ImageButton>
                                        <asp:ImageButton ID="ImgbtnSelectAll" Visible="false" runat="server" Style="width: 30px;" CausesValidation="False"
                                            ImageUrl="~/Images/selectAll.png" OnClick="ImgbtnSelectAll_Click" ToolTip="<%$ Resources:Attendance,Select All %>"></asp:ImageButton>
                                    </div>
                                    <div class="col-lg-2">
                                        <h5 class="text-center">
                                            <asp:Label ID="lblbinTotalRecords" Font-Bold="true" runat="server" Text="<%$ Resources:Attendance,Total Records: 0 %>"></asp:Label></h5>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="box box-warning box-solid">
                            <div class="box-header with-border">
                                <h3 class="box-title"></h3>
                            </div>
                            <div class="box-body">
                                <div class="row">
                                    <div class="col-md-12">
                                        <div class="flow">
                                            <asp:GridView CssClass="table-striped table-bordered table table-hover" ID="gvObjectMasterBin" PageSize="<%# PageControlCommon.GetPageSize() %>"
                                                runat="server" AutoGenerateColumns="False" Width="100%" AllowPaging="True" OnPageIndexChanging="gvObjectMasterBin_PageIndexChanging"
                                                OnSorting="gvObjectMasterBin_OnSorting" AllowSorting="true">
                                                <Columns>
                                                    <asp:TemplateField>
                                                        <ItemTemplate>
                                                            <asp:CheckBox ID="chkgvSelect" runat="server" OnCheckedChanged="chkgvSelect_CheckedChanged"
                                                                AutoPostBack="true" />
                                                        </ItemTemplate>
                                                        <HeaderTemplate>
                                                            <asp:CheckBox ID="chkgvSelectAll" runat="server" OnCheckedChanged="chkgvSelectAll_CheckedChanged"
                                                                AutoPostBack="true" />
                                                        </HeaderTemplate>
                                                        <ItemStyle HorizontalAlign="Center" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="<%$ Resources:Attendance,Object Id %>" SortExpression="Object_Id">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblObjectId1" runat="server" Text='<%# Eval("Object_Id") %>'></asp:Label>
                                                        </ItemTemplate>
                                                        <ItemStyle />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="<%$ Resources:Attendance,Object Name %>" SortExpression="Object_Name">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lbObjectName" runat="server" Text='<%# Eval("Object_Name") %>'></asp:Label>
                                                            <asp:Label ID="lblObjectId" Visible="false" runat="server" Text='<%# Eval("Object_Id") %>'></asp:Label>
                                                        </ItemTemplate>
                                                        <ItemStyle />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="<%$ Resources:Attendance,Object Name(Local) %>" SortExpression="Object_Name_L">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblObjectNameL" runat="server" Text='<%# Eval("Object_Name_L") %>'></asp:Label>
                                                        </ItemTemplate>
                                                        <ItemStyle />
                                                    </asp:TemplateField>
                                                </Columns>

                                                <HeaderStyle CssClass="pagination-ys" />
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
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="AfterLoadScriptContent" runat="Server">
    <script src="../Script/common.js"></script>
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

        function on_View_tab_position() {
            $("#Li_List").removeClass("active");
            $("#List").removeClass("active");

            $("#Li_New").addClass("active");
            $("#New").addClass("active");
        }
        function Li_Tab_Bin() {
            document.getElementById('<%= Btn_Bin.ClientID %>').click();
        }


        function LI_List_Active() {
            $("#Li_List").addClass("active");
            $("#List").addClass("active");

            $("#Li_New").removeClass("active");
            $("#New").removeClass("active");
        }


    </script>
</asp:Content>
