<%@ Page Title="" Language="C#" MasterPageFile="~/ERPMaster.master" AutoEventWireup="true"
    CodeFile="ApplicationMaster.aspx.cs" Inherits="ITSetUp_ApplicationMaster" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="HeaderContent" runat="Server">
    <h1>
       <i class="fas fa-window-restore"></i>&nbsp;&nbsp;
        <asp:Label ID="lblHeader" runat="server" Text="<%$ Resources:Attendance,Application Setup%>"></asp:Label>
    </h1>
    <ol class="breadcrumb">
        <li><a href="#"><i class="fa fa-dashboard"></i>
            <asp:Label ID="Label6" runat="server" Text="<%$ Resources:Attendance,IT Security%>"></asp:Label></a></li>
        <li><a href="#"><i class="fa fa-dashboard"></i>
            <asp:Label ID="Label10" runat="server" Text="<%$ Resources:Attendance,IT Security%>"></asp:Label></a></li>
        <li class="active">
            <asp:Label ID="Label5" runat="server" Text="<%$ Resources:Attendance,Application Setup%>"></asp:Label></li>
    </ol>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="Server">
    <asp:UpdatePanel ID="Update_Button" runat="server">
        <ContentTemplate>
            <%--<asp:Button ID="Btn_List" Style="display: none;" runat="server" OnClick="btnList_Click" Text="List" />--%>
            <asp:Button ID="Btn_New" Style="display: none;" runat="server" OnClick="btnNew_Click" Text="New" />
            <asp:Button ID="Btn_Bin" Style="display: none;" runat="server" OnClick="btnBin_Click" Text="Bin" />
        </ContentTemplate>
    </asp:UpdatePanel>
    <asp:UpdateProgress ID="UpdateProgress5" runat="server" AssociatedUpdatePanelID="Update_Button">
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
            <li class="active" id="Li_New"><a onclick="Li_Tab_New()" href="#New" data-toggle="tab">
                <asp:UpdatePanel ID="Update_Li" runat="server">
                    <ContentTemplate>
                        <i class="fa fa-file"></i>&nbsp;&nbsp;
                        <asp:Label ID="Lbl_New_tab" runat="server" Text="<%$ Resources:Attendance,New%>"></asp:Label>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </a></li>
            <li style="display: none;" id="Li_List"><a href="#List" data-toggle="tab">
                <i class="fa fa-list"></i>&nbsp;&nbsp;
                <asp:Label ID="Label1" runat="server" Text="<%$ Resources:Attendance,List %>"></asp:Label></a></li>
        </ul>

          <asp:HiddenField runat="server" ID="hdnCanEdit" />
        <asp:HiddenField runat="server" ID="hdnCanDelete" />


        <div class="tab-content">
            <div class="tab-pane active" id="New">
                <asp:UpdatePanel ID="Update_New" runat="server">
                    <ContentTemplate>
                        <div id="Div_New" runat="server">
                            <div class="row">
                                <div class="col-md-12">
                                    <div class="box box-primary">
                                        <div class="box-body">
                                            <div class="form-group">
                                                <div class="col-md-6">
                                                    <asp:Label ID="lblApplicationName" runat="server" Text="<%$ Resources:Attendance,Application Name %>" />
                                                    <a style="color: Red">*</a>
                                                    <asp:RequiredFieldValidator EnableClientScript="true" Style="float: right;" runat="server"
                                                        ID="RequiredFieldValidator3" ValidationGroup="Save" Display="Dynamic" SetFocusOnError="true"
                                                        ControlToValidate="txtApplicationName" ErrorMessage="<%$ Resources:Attendance,Enter Application Name %>"></asp:RequiredFieldValidator>
                                                    <asp:TextBox ID="txtApplicationName" runat="server" AutoPostBack="true" OnTextChanged="txtApplicationName_TextChanged"
                                                        CssClass="form-control" BackColor="#eeeeee"></asp:TextBox>
                                                    <cc1:AutoCompleteExtender ID="autoComplete1" runat="server" DelimiterCharacters=""
                                                        Enabled="True" ServiceMethod="GetCompletionListApplication" ServicePath="" CompletionInterval="100"
                                                        MinimumPrefixLength="1" CompletionSetCount="1" TargetControlID="txtApplicationName"
                                                        UseContextKey="True" CompletionListCssClass="autoCompleteList" CompletionListItemCssClass="autoCompleteListItem"
                                                        CompletionListHighlightedItemCssClass="autoCompleteSelectedListItem" OnClientShown="resetPosition">
                                                    </cc1:AutoCompleteExtender>
                                                    <br />
                                                </div>
                                                <div class="col-md-6">
                                                    <asp:Label ID="lblApplicationNameL" runat="server" Text="<%$ Resources:Attendance,Application Name(Local) %>" />
                                                    <asp:TextBox ID="txtApplicationNameL" runat="server" CssClass="form-control"></asp:TextBox>
                                                    <br />
                                                </div>
                                                <div style="text-align: center;">
                                                    <asp:UpdatePanel ID="Update_Save" runat="server">
                                                        <ContentTemplate>
                                                            <asp:Button ID="btnCSave" runat="server" ValidationGroup="Save" Text="<%$ Resources:Attendance,Save %>"
                                                                CssClass="btn btn-success" OnClick="btnCSave_Click" Visible="false" />
                                                            <asp:Button ID="BtnReset" runat="server" Text="<%$ Resources:Attendance,Reset %>"
                                                                CssClass="btn btn-primary" CausesValidation="False" OnClick="BtnReset_Click" />
                                                            <asp:HiddenField ID="editid" runat="server" />
                                                        </ContentTemplate>
                                                    </asp:UpdatePanel>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>

                            <div class="row">
                                <div class="col-md-12">
                                    <div id="Div1" runat="server" class="box box-info collapsed-box">
                                        <div class="box-header with-border">
                                            <h3 class="box-title">
                                                <asp:Label ID="Label18" runat="server" Text="Advance Search"></asp:Label></h3>
                                            &nbsp;&nbsp;|&nbsp;&nbsp;
                                             <asp:Label ID="lblTotalRecords" Font-Bold="true" runat="server" Text="<%$ Resources:Attendance,Total Records: 0 %>"></asp:Label>

                                            <div class="box-tools pull-right">
                                                <button type="button" class="btn btn-box-tool" data-widget="collapse">
                                                    <i id="I1" runat="server" class="fa fa-plus"></i>
                                                </button>
                                            </div>
                                        </div>
                                        <div class="box-body">
                                            <div class="col-lg-3">
                                                <asp:DropDownList ID="ddlFieldName" runat="server" class="form-control">
                                                    <asp:ListItem Text="<%$ Resources:Attendance, Application Name %>" Value="Application_Name" />
                                                    <asp:ListItem Text="<%$ Resources:Attendance, Application Name(Local) %>" Value="Application_L" />
                                                    <asp:ListItem Text="<%$ Resources:Attendance,Application Id %>" Value="Application_Id"></asp:ListItem>
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
                                                    <asp:TextBox ID="txtValue" runat="server" class="form-control" placeholder="Search from Content"></asp:TextBox>
                                                </asp:Panel>
                                            </div>
                                            <div class="col-lg-2" style="text-align: center;">
                                                <asp:LinkButton ID="btnbind"  runat="server" CausesValidation="False"  OnClick="btnbindrpt_Click" ToolTip="<%$ Resources:Attendance,Search %>"><span class="fa fa-search"  style="font-size:25px;"></span></asp:LinkButton>&nbsp;&nbsp;&nbsp;
                                                <asp:LinkButton ID="btnRefresh" runat="server" CausesValidation="False" OnClick="btnRefreshReport_Click" ToolTip="<%$ Resources:Attendance,Refresh %>"><span class="fa fa-repeat"  style="font-size:25px;"></span></asp:LinkButton>
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
                                                <asp:GridView CssClass="table-striped table-bordered table table-hover" ID="GvApplication" PageSize="<%# PageControlCommon.GetPageSize() %>"
                                                    runat="server" AutoGenerateColumns="False" Width="100%" AllowPaging="True" OnPageIndexChanging="GvApplication_PageIndexChanging"
                                                    AllowSorting="True" OnSorting="GvApplication_Sorting">

                                                    <Columns>
                                                        <asp:TemplateField HeaderText="Action">
                                                        <ItemTemplate>
                                                            <div class="dropdown" style="position: absolute;">
                                                                <button class="btn btn-default dropdown-toggle"  type="button" data-toggle="dropdown">
                                                                    <i class="fa fa-ellipsis-h" aria-hidden="true"></i>
                                                                </button>
                                                                <ul class="dropdown-menu">

                                                                    <li <%= hdnCanEdit.Value=="true"?"style='display:block'":"style='display:none'" %>>
                                                                        <asp:LinkButton ID="btnEdit" runat="server" CommandArgument='<%# Eval("Application_Id") %>' CausesValidation="False" OnCommand="btnEdit_Command" ToolTip="<%$ Resources:Attendance,Edit %>"> <i class="fa fa-pencil"></i>Edit </asp:LinkButton>
                                                                    </li>
                                                                    <li <%= hdnCanDelete.Value=="true"?"style='display:block'":"style='display:none'" %>>
                                                                        <asp:LinkButton ID="IbtnDelete" runat="server" CausesValidation="False" CommandArgument='<%# Eval("Application_ID") %>' OnCommand="IbtnDelete_Command" ToolTip="<%$ Resources:Attendance,Delete %>"><i class="fa fa-trash"></i>Delete</asp:LinkButton>
                                                                        <cc1:ConfirmButtonExtender ID="confirm1" runat="server" ConfirmText="<%$ Resources:Attendance,Are you sure you want to delete the record?%>" TargetControlID="IbtnDelete"></cc1:ConfirmButtonExtender>
                                                                    </li>
                                                                </ul>
                                                            </div>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                       
                                                        <asp:TemplateField HeaderText="<%$ Resources:Attendance,Application Id %>" SortExpression="Application_Id">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblDesigId1" runat="server" Text='<%# Eval("Application_Id") %>'></asp:Label>
                                                            </ItemTemplate>
                                                            <ItemStyle />
                                                        </asp:TemplateField>
                                                        <asp:BoundField DataField="Application_Name" HeaderText="<%$ Resources:Attendance,Application Name %>"
                                                            SortExpression="Application_Name" />
                                                        <asp:BoundField DataField="Application_Name_L" HeaderText="<%$ Resources:Attendance,Application Name(Local) %>"
                                                            SortExpression="Application_Name_L" />
                                                    </Columns>

                                                    <PagerStyle CssClass="pagination-ys" />

                                                </asp:GridView>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div style="display: none;" runat="server" id="Div_Tree">
                            <asp:UpdatePanel ID="Update_Tree" runat="server">
                                <ContentTemplate>
                                    <div class="row">
                                        <div class="col-md-12">
                                            <div class="box box-primary">
                                                <div role="form">
                                                    <div class="box-body">
                                                        <div class="form-group">
                                                            <div class="col-md-12">
                                                                <div class="col-md-6">
                                                                    <h4>
                                                                        <asp:Label ID="Label7" runat="server" Text="<%$ Resources:Attendance,Application ID %>" />
                                                                        &nbsp:&nbsp<asp:Label ID="lblAppId" runat="server" /></h4>
                                                                    <br />
                                                                </div>
                                                                <div class="col-md-6">
                                                                    <h4>
                                                                        <asp:Label ID="Label8" runat="server" Text="<%$ Resources:Attendance,Application Name %>" />
                                                                        &nbsp:&nbsp<asp:Label ID="lblAppName" runat="server" /></h4>
                                                                    <br />
                                                                </div>
                                                                <div style="text-align: center;">
                                                                    <asp:Button ID="btnSaveAPP" runat="server" CssClass="btn btn-success" OnClick="btnSaveApp_Click"
                                                                        TabIndex="107" Text="<%$ Resources:Attendance,Save %>" ValidationGroup="Tree_Save" />
                                                                    <asp:Button ID="btnResetAPP" runat="server" Style="margin-left: 15px;" CausesValidation="False"
                                                                        CssClass="btn btn-primary" OnClick="btnResetApp_Click" Text="<%$ Resources:Attendance,Reset %>" />
                                                                    <asp:Button ID="btnCancelAPP" runat="server" Style="margin-left: 15px;" CausesValidation="False"
                                                                        CssClass="btn btn-danger" OnClick="btnCancelApp_Click" Text="<%$ Resources:Attendance,Cancel %>" />
                                                                </div>
                                                            </div>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="box box-warning box-solid">
                                        <div class="box-header with-border">
                                            <h3 class="box-title">
                                                <asp:CheckBox ID="chkSelectAll" runat="server" Text="<%$ Resources:Attendance,Select All %>"
                                                    AutoPostBack="true" OnCheckedChanged="ChkSelectAll_OnCheckedChanged" />
                                            </h3>
                                        </div>
                                        <div class="box-body">
                                            <div class="row">
                                                <div class="col-md-12">
                                                    <asp:CustomValidator ID="CustomValidator3" ValidationGroup="Tree_Save" runat="server"
                                                        ErrorMessage="Please select at least one Module." ClientValidationFunction="Module"
                                                        ForeColor="Red"></asp:CustomValidator>
                                                    <div class="flow">
                                                        <asp:TreeView ID="navTree" runat="server" Height="100%" CssClass="form-control" NodeStyle-CssClass="treeNode"
                                                            RootNodeStyle-CssClass="rootNode" LeafNodeStyle-CssClass="leafNode" ShowCheckBoxes="All">
                                                        </asp:TreeView>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </ContentTemplate>
                            </asp:UpdatePanel>
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
                                        <asp:DropDownList ID="ddlFieldNameBin" runat="server" class="form-control">
                                            <asp:ListItem Text="<%$ Resources:Attendance, Application Name%>" Value="Application" />
                                            <asp:ListItem Text="<%$ Resources:Attendance, Application Name(Local) %>" Value="Application_L" />
                                            <asp:ListItem Text="<%$ Resources:Attendance,Application Id %>" Value="Application_Id"></asp:ListItem>
                                        </asp:DropDownList>
                                    </div>
                                    <div class="col-lg-2">
                                        <asp:DropDownList ID="ddlOptionBin" runat="server" class="form-control">
                                            <asp:ListItem Text="<%$ Resources:Attendance,--Select-- %>"></asp:ListItem>
                                            <asp:ListItem Text="<%$ Resources:Attendance,Equal %>"></asp:ListItem>
                                            <asp:ListItem Selected="True" Text="<%$ Resources:Attendance,Contains %>"></asp:ListItem>
                                            <asp:ListItem Text="<%$ Resources:Attendance,Like %>"></asp:ListItem>
                                        </asp:DropDownList>
                                    </div>
                                    <div class="col-lg-3">
                                        <asp:Panel ID="Panel2" runat="server" DefaultButton="btnbindBin">
                                            <asp:TextBox ID="txtValueBin" runat="server" CssClass="form-control"></asp:TextBox>
                                        </asp:Panel>
                                    </div>
                                    <div class="col-lg-2" style="text-align: center;">
                                        <asp:ImageButton ID="btnbindBin" Style="width: 35px; margin-top: -1px;" runat="server"
                                            CausesValidation="False" ImageUrl="~/Images/search.png" OnClick="btnbindBin_Click"
                                            ToolTip="<%$ Resources:Attendance,Search %>"></asp:ImageButton>
                                        <asp:ImageButton ID="btnRefreshBin" runat="server" Style="width: 30px;" CausesValidation="False"
                                            ImageUrl="~/Images/refresh.png" OnClick="btnRefreshBin_Click" ToolTip="<%$ Resources:Attendance,Refresh %>"></asp:ImageButton>
                                        <asp:ImageButton ID="imgBtnRestore" runat="server" Style="width: 30px;" CausesValidation="False"
                                            ImageUrl="~/Images/active.png" OnClick="btnRestoreSelected_Click" ToolTip="<%$ Resources:Attendance,Refresh %>"></asp:ImageButton>
                                        <asp:ImageButton ID="ImgbtnSelectAll" Visible="false" runat="server" Style="width: 30px;" CausesValidation="False"
                                            ImageUrl="~/Images/selectAll.png" OnClick="ImgbtnSelectAll_Click" ToolTip="<%$ Resources:Attendance,Select All %>"></asp:ImageButton>
                                    </div>
                                    <div class="col-lg-2">
                                        <h5 class="text-center">
                                            <asp:Label ID="lblTotalRecordsBin" Font-Bold="true" runat="server" Text="<%$ Resources:Attendance,Total Records: 0 %>"></asp:Label></h5>
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
                                            <asp:Label ID="lblSelectedRecord" runat="server" Visible="false"></asp:Label>
                                            <asp:GridView CssClass="table-striped table-bordered table table-hover" ID="GvApplicationBin" PageSize="<%# PageControlCommon.GetPageSize() %>"
                                                runat="server" AutoGenerateColumns="False" Width="100%" AllowPaging="True" OnPageIndexChanging="GvApplicationBin_PageIndexChanging"
                                                OnSorting="GvApplicationBin_OnSorting" AllowSorting="true">
                                                <Columns>
                                                    <asp:TemplateField>
                                                        <HeaderTemplate>
                                                            <asp:CheckBox ID="chkgvSelectAll" runat="server" OnCheckedChanged="chkgvSelectAll_CheckedChanged"
                                                                AutoPostBack="true" />
                                                        </HeaderTemplate>
                                                        <ItemTemplate>
                                                            <asp:CheckBox ID="chkgvSelect" runat="server" AutoPostBack="true" OnCheckedChanged="chkgvSelect_CheckedChanged" />
                                                        </ItemTemplate>
                                                        <ItemStyle HorizontalAlign="Center" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="<%$ Resources:Attendance,Application Id %>" SortExpression="Application_Id">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblDesigId1" runat="server" Text='<%# Eval("Application_Id") %>'></asp:Label>
                                                        </ItemTemplate>
                                                        <ItemStyle />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="<%$ Resources:Attendance,Application Name %>" SortExpression="Application">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblgvApplicationName" runat="server" Text='<%# Eval("Application_Name") %>'></asp:Label>
                                                            <asp:Label ID="lblApplicationId" Visible="false" runat="server" Text='<%# Eval("Application_Id") %>'></asp:Label>
                                                        </ItemTemplate>
                                                        <ItemStyle />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="<%$ Resources:Attendance,Application Name(Local) %>"
                                                        SortExpression="Application_L">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblgvApplicationNameL" runat="server" Text='<%# Eval("Application_Name_L") %>'></asp:Label>
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
    <asp:UpdateProgress ID="UpdateProgress1" runat="server" AssociatedUpdatePanelID="Update_Save">
        <ProgressTemplate>
            <div class="modal_Progress">
                <div class="center_Progress">
                    <img alt="" src="../Images/ajax-loader2.gif" style="vertical-align: middle; background-color: White; border-radius: 10px;" />
                </div>
            </div>
        </ProgressTemplate>
    </asp:UpdateProgress>
    <asp:UpdateProgress ID="UpdateProgress4" runat="server" AssociatedUpdatePanelID="Update_Tree">
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
    <asp:Panel ID="pnlMenuBin" runat="server" Visible="false">
    </asp:Panel>
    <asp:Panel ID="pnlMenuNew" runat="server" Visible="false">
    </asp:Panel>
    <asp:Panel ID="pnlList" runat="server" Visible="false">
    </asp:Panel>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="FooterContent" runat="Server">
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="AfterLoadScriptContent" runat="Server">
    <script src="../Script/common.js"></script>
    <script type="text/javascript">
        function Module(sender, args) {
            var chkBox = document.getElementById('<%= navTree.ClientID %>');
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

        function Li_Tab_New() {
            document.getElementById('<%= Btn_New.ClientID %>').click();
        }
        function Li_Tab_Bin() {
            document.getElementById('<%= Btn_Bin.ClientID %>').click();
        }

        function on_Bin_tab_position() {
            $("#Li_New").removeClass("active");
            $("#New").removeClass("active");

            $("#Li_Bin").addClass("active");
            $("#Bin").addClass("active");
            document.getElementById("Li_Bin").style.display = "";
        }

        function on_Bin_Hide_tab() {
            $("#Li_Bin").removeClass("active");
            $("#Bin").removeClass("active");

            $("#Li_New").addClass("active");
            $("#New").addClass("active");
            document.getElementById("Li_Bin").style.display = "none";
        }

    </script>
</asp:Content>
