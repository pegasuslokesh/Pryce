<%@ Page Title="" Language="C#" MasterPageFile="~/ERPMaster.master" AutoEventWireup="true" CodeFile="ModuleMaster.aspx.cs" Inherits="ITSetUp_ModuleMaster" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit.HTMLEditor" TagPrefix="cc2" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="HeaderContent" runat="Server">
    <h1>
        <i class="fas fa-cog"></i>&nbsp;&nbsp;
        <asp:Label ID="lblHeader" runat="server" Text="<%$ Resources:Attendance,Module Setup%>"></asp:Label>
    </h1>
    <ol class="breadcrumb">
        <li><a href="#"><i class="fa fa-dashboard"></i>
            <asp:Label ID="Label6" runat="server" Text="<%$ Resources:Attendance,IT Security%>"></asp:Label></a></li>
        <li><a href="#"><i class="fa fa-dashboard"></i>
            <asp:Label ID="Label10" runat="server" Text="<%$ Resources:Attendance,IT Security%>"></asp:Label></a></li>
        <li class="active">
            <asp:Label ID="Label5" runat="server" Text="<%$ Resources:Attendance,Module Setup%>"></asp:Label></li>
    </ol>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="Server">
    <asp:UpdatePanel ID="Update_Button" runat="server">
        <ContentTemplate>
            <asp:Button ID="Btn_Bin" Style="display: none;" runat="server" OnClick="btnBin_Click" Text="Bin" />
            <asp:HiddenField runat="server" ID="hdnCanEdit" />
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
            <li style="display: none;" id="Li_Bin"><a onclick="Li_Tab_Bin()" href="#Bin" data-toggle="tab">
                <i class="fa fa-trash"></i>&nbsp;&nbsp;
                <asp:Label ID="Label3" runat="server" Text="<%$ Resources:Attendance,Bin %>"></asp:Label></a></li>
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
                                            <asp:HiddenField ID="hdnParentModuleId" runat="server" Value="0" />
                                            <asp:DropDownList ID="ddlFieldName" runat="server" class="form-control">
                                                <asp:ListItem Selected="True" Text="<%$ Resources:Attendance,Module Name %>" Value="Module_Name"></asp:ListItem>
                                                <asp:ListItem Text="<%$ Resources:Attendance,Module Name(Local) %>" Value="Module_Name_L"></asp:ListItem>
                                                <asp:ListItem Text="<%$ Resources:Attendance,Application Name %>" Value="Application_Name"></asp:ListItem>
                                                <asp:ListItem Text="<%$ Resources:Attendance,Module Id %>" Value="Module_Id"></asp:ListItem>
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
                                            <asp:Panel ID="Pnl_TxtValue" runat="server" DefaultButton="btnbind">
                                                <asp:TextBox ID="txtValue" runat="server" class="form-control" placeholder="Search from Content"></asp:TextBox>
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
                                            <asp:GridView CssClass="table-striped table-bordered table table-hover" ID="gvModuleMaster" PageSize="<%# PageControlCommon.GetPageSize() %>"
                                                runat="server" AutoGenerateColumns="False" Width="100%" AllowPaging="True" AllowSorting="True"
                                                OnPageIndexChanging="gvModuleMaster_PageIndexChanging" OnSorting="gvModuleMaster_OnSorting">
                                                <Columns>
                                                    <asp:TemplateField HeaderText="<%$ Resources:Attendance,Action%>">
                                                        <ItemTemplate>
                                                            <div class="dropdown" style="position: absolute;">
                                                                <button class="btn btn-default dropdown-toggle"  type="button" data-toggle="dropdown">
                                                                    <i class="fa fa-ellipsis-h" aria-hidden="true"></i>
                                                                </button>
                                                                <ul class="dropdown-menu">

                                                                    <li <%= hdnCanEdit.Value=="true"?"style='display:block'":"style='display:none'" %>>
                                                                        <asp:LinkButton ID="btnEdit" runat="server" CommandArgument='<%# Eval("Module_Id") %>' CausesValidation="False" OnCommand="btnEdit_Command" ToolTip="<%$ Resources:Attendance,Edit %>"> <i class="fa fa-pencil"></i>Edit </asp:LinkButton>
                                                                    </li>
                                                                    <li <%= hdnCanDelete.Value=="true"?"style='display:block'":"style='display:none'" %>>
                                                                        <asp:LinkButton ID="IbtnDelete" runat="server" CausesValidation="False" CommandArgument='<%# Eval("Module_Id") %>' OnCommand="IbtnDelete_Command" ToolTip="<%$ Resources:Attendance,Delete %>"><i class="fa fa-trash"></i>Delete</asp:LinkButton>
                                                                        <cc1:ConfirmButtonExtender ID="confirm1" runat="server" ConfirmText="<%$ Resources:Attendance,Are you sure you want to delete the record?%>" TargetControlID="IbtnDelete"></cc1:ConfirmButtonExtender>
                                                                    </li>
                                                                </ul>
                                                            </div>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>

                                                    <asp:TemplateField HeaderText="<%$ Resources:Attendance,Module Id %>" SortExpression="Module_Id">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblModuleId1" runat="server" Text='<%# Eval("Module_Id") %>'></asp:Label>
                                                        </ItemTemplate>
                                                        <ItemStyle />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="<%$ Resources:Attendance,Module Name %>" SortExpression="Module_Name">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lbModuleName" runat="server" Text='<%# Eval("Module_Name") %>'></asp:Label>
                                                        </ItemTemplate>
                                                        <ItemStyle />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="<%$ Resources:Attendance,Module Name(Local) %>" SortExpression="Module_Name_L">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblModuleNameL" runat="server" Text='<%# Eval("Module_Name_L") %>'></asp:Label>
                                                        </ItemTemplate>
                                                        <ItemStyle />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField Visible="false" HeaderText="<%$ Resources:Attendance,Application Name %>"
                                                        SortExpression="Application_Name">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblApplicationName" runat="server" Text='<%# Eval("Application_Name") %>'></asp:Label>
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
                                            <asp:Label ID="Label35" runat="server" Text="<%$ Resources:Attendance,Module%>"></asp:Label></h3>
                                        <div class="box-tools pull-right">
                                            <button type="button" class="btn btn-box-tool" data-widget="collapse">
                                                <i class="fa fa-minus"></i>
                                            </button>
                                        </div>
                                    </div>
                                    <div class="box-body">
                                        <div class="form-group">
                                            <div class="col-md-6">
                                                <asp:Label ID="lblModuleName" runat="server" Text="<%$ Resources:Attendance,Module Name %>"></asp:Label>
                                                <a style="color: Red">*</a>
                                                <asp:RequiredFieldValidator EnableClientScript="true" Style="float: right;" runat="server"
                                                    ID="RequiredFieldValidator3" ValidationGroup="Save" Display="Dynamic" SetFocusOnError="true"
                                                    ControlToValidate="txtModuleName" ErrorMessage="<%$ Resources:Attendance,Enter Module Name %>"></asp:RequiredFieldValidator>
                                                <asp:TextBox ID="txtModuleName" BackColor="#eeeeee" runat="server" AutoPostBack="true"
                                                    OnTextChanged="txtModuleName_TextChanged" CssClass="form-control" />
                                                <cc1:AutoCompleteExtender ID="autoComplete1" runat="server" DelimiterCharacters=""
                                                    Enabled="True" ServiceMethod="GetCompletionListModuleName" ServicePath="" CompletionInterval="100"
                                                    MinimumPrefixLength="1" CompletionSetCount="1" TargetControlID="txtModuleName"
                                                    UseContextKey="True" CompletionListCssClass="autoCompleteList" CompletionListItemCssClass="autoCompleteListItem"
                                                    CompletionListHighlightedItemCssClass="autoCompleteSelectedListItem" OnClientShown="resetPosition">
                                                </cc1:AutoCompleteExtender>
                                                <br />
                                                <asp:Label ID="Label2" runat="server" Text="<%$ Resources:Attendance,Sequence No. %>"></asp:Label>
                                                <asp:TextBox ID="txtSquenceNo" runat="server" CssClass="form-control" />
                                                <cc1:FilteredTextBoxExtender ID="FiltertxtSquence" runat="server" TargetControlID="txtSquenceNo"
                                                    ValidChars="0,1,2,3,4,5,6,7,8,9">
                                                </cc1:FilteredTextBoxExtender>
                                                <br />
                                                <asp:Label ID="Label12" runat="server" Text="<%$ Resources:Attendance,Image %>" />
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
                                                        <asp:Image ID="FULogo_Img_Right" runat="server" Width="30px" Height="30px" ImageUrl="../Images/Allow.png" Style="display: none" />
                                                        <asp:Image ID="FULogo_Img_Wrong" runat="server" Width="30px" Height="30px" ImageUrl="../Images/Delete1.png" Style="display: none" />
                                                        <asp:Image ID="FULogo_ImgLoader" runat="server" ImageUrl="../Images/loader.gif" />
                                                    </div>
                                                </div>
                                                <br />
                                                <asp:Label ID="Label13" runat="server" Text="<%$ Resources:Attendance,Icon URL %>" />
                                                <asp:TextBox ID="txtIconUrl" runat="server" CssClass="form-control"></asp:TextBox>


                                            </div>
                                            <div class="col-md-6">
                                                <asp:Label ID="lblModuleNameL" runat="server" Text="<%$ Resources:Attendance,Module Name(Local) %>"></asp:Label>
                                                <asp:TextBox ID="txtModuleNameL" runat="server" CssClass="form-control" />
                                                <br />
                                                <asp:Label ID="Label11" runat="server" Text="<%$ Resources:Attendance,Dashboard URL %>"></asp:Label>
                                                <asp:TextBox ID="txtDashBoardUrl" runat="server" CssClass="form-control" />
                                                <br />
                                                <div style="text-align: center">
                                                    <asp:Image ID="imgLogo" runat="server" ImageUrl="../Bootstrap_Files/dist/img/NoImage.jpg" Width="90px" Height="120px" />
                                                    <asp:Button ID="btnUpload" runat="server" Text="<%$ Resources:Attendance,Load %>"
                                                        CssClass="btn btn-primary" OnClick="btnUpload_Click" />
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                                <div class="box box-info">
                                    <div class="box-header with-border">
                                        <h3 class="box-title">
                                            <asp:Label ID="Label14" runat="server" Text="<%$ Resources:Attendance,Module%>"></asp:Label></h3>
                                        <div class="box-tools pull-right">
                                            <button type="button" class="btn btn-box-tool" data-widget="collapse">
                                                <i class="fa fa-minus"></i>
                                            </button>
                                        </div>
                                    </div>
                                    <div class="box-body">
                                        <div class="form-group">
                                            <cc2:Editor ID="Editor1" AutoFocus="false" runat="server" />
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-md-12" style="text-align: center;">
                                <asp:Button ID="btnSave" runat="server" Text="<%$ Resources:Attendance,Save %>"
                                    Visible="false" CssClass="btn btn-success" ValidationGroup="Save" OnClick="btnSave_Click" />
                                <asp:Button ID="btnReset" runat="server" Text="<%$ Resources:Attendance,Reset %>"
                                    CssClass="btn btn-primary" TabIndex="110" CausesValidation="False" OnClick="btnReset_Click" />
                                <asp:Button ID="btnCancel" runat="server" Text="<%$ Resources:Attendance,Cancel %>"
                                    CssClass="btn btn-danger" TabIndex="111" CausesValidation="False" OnClick="btnCancel_Click" />
                                <asp:HiddenField ID="editid" runat="server" />
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
                                            <asp:ListItem Selected="True" Text="<%$ Resources:Attendance,Module Name %>" Value="Module_Name"></asp:ListItem>
                                            <asp:ListItem Text="<%$ Resources:Attendance,Module Name(Local) %>" Value="Module_Name_L"></asp:ListItem>
                                            <asp:ListItem Text="<%$ Resources:Attendance,Module Id %>" Value="Module_Id"></asp:ListItem>
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
                                        <asp:Panel ID="Panel1" runat="server" DefaultButton="btnbinbind">
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
                                            <asp:GridView CssClass="table-striped table-bordered table table-hover" ID="gvModuleMasterBin" PageSize="<%# PageControlCommon.GetPageSize() %>"
                                                runat="server" AutoGenerateColumns="False" Width="100%" AllowPaging="True" OnPageIndexChanging="gvModuleMasterBin_PageIndexChanging"
                                                OnSorting="gvModuleMasterBin_OnSorting" AllowSorting="true">
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
                                                    <asp:TemplateField HeaderText="<%$ Resources:Attendance,Module Id %>" SortExpression="Module_Id">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblModuleId1" runat="server" Text='<%# Eval("Module_Id") %>'></asp:Label>
                                                        </ItemTemplate>
                                                        <ItemStyle />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="<%$ Resources:Attendance,Module Name %>" SortExpression="Module_Name">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lbModuleName" runat="server" Text='<%# Eval("Module_Name") %>'></asp:Label>
                                                            <asp:Label ID="lblModuleId" Visible="false" runat="server" Text='<%# Eval("Module_Id") %>'></asp:Label>
                                                        </ItemTemplate>
                                                        <ItemStyle />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="<%$ Resources:Attendance,Module Name(Local) %>" SortExpression="Module_Name_L">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblModuleNameL" runat="server" Text='<%# Eval("Module_Name_L") %>'></asp:Label>
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
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="AfterLoadScriptContent" runat="Server">
    <script src="../Script/common.js"></script>
    <script>
       
        function Li_Tab_Bin() {
            document.getElementById('<%= Btn_Bin.ClientID %>').click();
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

    <script type="text/javascript">
        function FuLogo_UploadComplete(sender, args) {
            document.getElementById('<%= FULogo_Img_Wrong.ClientID %>').style.display = "none";
            document.getElementById('<%= FULogo_Img_Right.ClientID %>').style.display = "";

            var img = document.getElementById('<%= imgLogo.ClientID %>');
            img.src = "<%=ResolveUrl(FuLogo_UploadFolderPath) %>" + args.get_fileName();
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
    </script>
</asp:Content>
