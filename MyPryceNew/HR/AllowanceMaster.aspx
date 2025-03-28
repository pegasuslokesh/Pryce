<%@ Page Title="" Language="C#" MasterPageFile="~/ERPMaster.master" AutoEventWireup="true" CodeFile="AllowanceMaster.aspx.cs" Inherits="HR_AllowanceMaster" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="HeaderContent" runat="Server">
    <h1>
        <i class="fas fa-coins"></i>
        <asp:Label ID="lblHeader" runat="server" Text="<%$ Resources:Attendance,Allowance Setup%>"></asp:Label>
    </h1>
    <ol class="breadcrumb">
        <li><a href="#"><i class="fa fa-dashboard"></i>
            <asp:Label ID="Label6" runat="server" Text="<%$ Resources:Attendance,HR%>"></asp:Label></a></li>
        <li><a href="#"><i class="fa fa-dashboard"></i>
            <asp:Label ID="Label10" runat="server" Text="<%$ Resources:Attendance,Payroll%>"></asp:Label></a></li>
        <li class="active">
            <asp:Label ID="Label5" runat="server" Text="<%$ Resources:Attendance,Allowance Master%>"></asp:Label></li>
    </ol>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="Server">
    <asp:UpdatePanel ID="Update_Button" runat="server">
        <ContentTemplate>
            <asp:HiddenField runat="server" ID="hdnCanEdit" />
            <asp:HiddenField runat="server" ID="hdnCanDelete" />
            <asp:Button ID="Btn_New" Style="display: none;" runat="server" OnClick="btnNew_Click" Text="New" />
            <asp:Button ID="Btn_Bin" Style="display: none;" runat="server" OnClick="btnBin_Click" Text="Bin" />
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
            <li id="Li_Bin"><a href="#Bin" onclick="Li_Tab_Bin()" data-toggle="tab">
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
        </ul>
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
                                                    <asp:Label ID="lblApplicationName" runat="server" Text="<%$ Resources:Attendance,Allowance Name %>" />
                                                    <a style="color: Red">*</a>
                                                    <asp:RequiredFieldValidator EnableClientScript="true" Style="float: right;" runat="server"
                                                        ID="RequiredFieldValidator3" ValidationGroup="Save" Display="Dynamic" SetFocusOnError="true"
                                                        ControlToValidate="txtAllowanceName" ErrorMessage="<%$ Resources:Attendance,Enter Allowance Name %>"></asp:RequiredFieldValidator>
                                                    <asp:TextBox ID="txtAllowanceName" runat="server" class="form-control" BackColor="#eeeeee"></asp:TextBox>


                                                    <cc1:AutoCompleteExtender ID="txtAllowanceName_AutoCompleteExtender" runat="server"
                                                        DelimiterCharacters="" Enabled="True" ServiceMethod="GetCompletionList" ServicePath=""
                                                        CompletionInterval="100" MinimumPrefixLength="1" CompletionSetCount="1" TargetControlID="txtAllowanceName"
                                                        UseContextKey="True" CompletionListCssClass="autoCompleteList" CompletionListItemCssClass="autoCompleteListItem"
                                                        CompletionListHighlightedItemCssClass="autoCompleteSelectedListItem" OnClientShown="resetPosition">
                                                    </cc1:AutoCompleteExtender>
                                                    <%-- <asp:TextBox ID="txtAllowanceName" runat="server" CssClass="form-control" BackColor="#eeeeee"></asp:TextBox>
                                                        <cc1:AutoCompleteExtender ID="txtAllowanceName_AutoCompleteExtender" runat="server"
                                                            DelimiterCharacters="" Enabled="True" ServiceMethod="GetCompletionList" ServicePath=""
                                                            CompletionInterval="100" MinimumPrefixLength="1" CompletionSetCount="1" TargetControlID="txtAllowanceName"
                                                            UseContextKey="True" CompletionListCssClass="autoCompleteList" CompletionListItemCssClass="autoCompleteListItem"
                                                            CompletionListHighlightedItemCssClass="autoCompleteSelectedListItem" OnClientShown="resetPosition">
                                                        </cc1:AutoCompleteExtender>--%>
                                                    <br />
                                                </div>
                                                <div class="col-md-6">
                                                    <asp:Label ID="lblApplicationNameL" runat="server" Text="<%$ Resources:Attendance,Allowance Name(Local) %>" />
                                                    <asp:TextBox ID="txtAllowanceNameL" runat="server" CssClass="form-control"></asp:TextBox>
                                                    <br />
                                                </div>
                                                <div class="col-md-6">
                                                    <asp:Label ID="lblAccountNo" runat="server" Text="<%$ Resources:Attendance,Account No%>" />
                                                    <asp:TextBox ID="txtAccountNo" runat="server" CssClass="form-control" BackColor="#eeeeee"
                                                        AutoPostBack="true" OnTextChanged="txtcmnAccount_textChnaged"></asp:TextBox>
                                                    <cc1:AutoCompleteExtender ID="AutoCompleteExtender14" runat="server" DelimiterCharacters=""
                                                        Enabled="True" ServiceMethod="GetCompletionListAccountName" ServicePath="" CompletionInterval="100"
                                                        MinimumPrefixLength="1" CompletionSetCount="1" TargetControlID="txtAccountNo"
                                                        UseContextKey="True" CompletionListCssClass="autoCompleteList" CompletionListItemCssClass="autoCompleteListItem" CompletionListHighlightedItemCssClass="autoCompleteSelectedListItem" OnClientShown="resetPosition">
                                                    </cc1:AutoCompleteExtender>
                                                    <br />
                                                </div>



                                                <div class="col-md-6">
                                                    <asp:Label ID="Label1" runat="server" Text="<%$ Resources:Attendance,Calculation Type%>" />

                                                    <asp:DropDownList ID="ddlCalculationType" runat="server" CssClass="form-control"
                                                        AutoPostBack="true" OnTextChanged="ddlCalculationType_TextChanged">

                                                        <asp:ListItem Text="Basis of attendance salary" Value="0"></asp:ListItem>
                                                        <asp:ListItem Text="Basis of attendance day" Value="1"></asp:ListItem>

                                                    </asp:DropDownList>

                                                    <br />
                                                </div>

                                                <div class="col-md-12" runat="server" id="Div_IncludeDay" visible="false">
                                                    <asp:Label ID="Label2" runat="server" Text="<%$ Resources:Attendance,Include Day%>" />
                                                    :
                                                    
                                                    <asp:CheckBox ID="chkPresent" runat="server" Text="Present" />&nbsp;&nbsp;
                                                    <asp:CheckBox ID="chkweekoff" runat="server" Text="Week Off" />&nbsp;&nbsp;
                                                    <asp:CheckBox ID="chkHoliday" runat="server" Text="Holiday" />&nbsp;&nbsp;
                                                    <asp:CheckBox ID="chkabsent" runat="server" Text="Absent" />&nbsp;&nbsp;
                                                    <asp:CheckBox ID="chkPaidLeave" runat="server" Text="Paid Leave" />&nbsp;&nbsp;
                                                    <asp:CheckBox ID="chkUnpaidLeave" runat="server" Text="UnPaid Leave" />&nbsp;&nbsp;
                                                    <asp:CheckBox ID="chkHalfday" runat="server" Text="Half Day" />&nbsp;&nbsp;
                                                    <%--<asp:CheckBox ID="chkunPaidLeave"  runat="server" Text="UnPaid Leave" />--%>
                                                </div>

                                                <div class="col-md-12" style="text-align: center;">
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
                                <asp:HiddenField ID="hdnBrand_id" runat="server" />
                                <asp:HiddenField ID="hdnLocation_id" runat="server" />

                                <div class="col-md-12">
                                    <div id="Div1" runat="server" class="box box-info collapsed-box">
                                        <div class="box-header with-border">
                                            <h3 class="box-title">
                                                <asp:Label ID="Label22" runat="server" Text="Advance Search"></asp:Label></h3>
                                            &nbsp;&nbsp;|&nbsp;&nbsp;
					<asp:Label ID="lblTotalRecords" Font-Bold="true" runat="server" Text="<%$ Resources:Attendance,Total Records: 0 %>"></asp:Label>

                                            <div class="box-tools pull-right">
                                                <button type="button" class="btn btn-box-tool" data-widget="collapse">
                                                    <i id="I1" runat="server" class="fa fa-plus"></i>
                                                </button>
                                            </div>
                                        </div>
                                        <div class="box-body">
                                            <div class="col-lg-6">
                                                <asp:DropDownList ID="ddlLocation" runat="server" CssClass="form-control" OnSelectedIndexChanged="ddlLocation_SelectedIndexChanged" AutoPostBack="true">
                                                </asp:DropDownList>
                                            </div>

                                            <div class="col-lg-12">
                                                <br />
                                            </div>
                                            <div class="col-lg-3">
                                                <asp:DropDownList ID="ddlFieldName" runat="server" class="form-control">
                                                    <asp:ListItem Text="<%$ Resources:Attendance, Allowance Id %>" Value="Allowance_Id" />
                                                    <asp:ListItem Text="<%$ Resources:Attendance, Allowance Name %>" Value="Allowance"
                                                        Selected="True" />
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
                                                <asp:Panel ID="Panel2" runat="server" DefaultButton="btnbind">
                                                    <asp:TextBox ID="txtValue" placeholder="Search from Content" runat="server" class="form-control"></asp:TextBox>
                                                </asp:Panel>
                                            </div>
                                            <div class="col-lg-2" style="text-align: center;">
                                                <asp:LinkButton ID="btnbind" runat="server"
                                                    CausesValidation="False" OnClick="btnbindrpt_Click"
                                                    ToolTip="<%$ Resources:Attendance,Search %>"><span class="fa fa-search"  style="font-size:25px;"></span></asp:LinkButton>&nbsp;&nbsp;&nbsp;
                                                <asp:LinkButton ID="btnRefresh" runat="server" CausesValidation="False"
                                                    OnClick="btnRefresh_Click" ToolTip="<%$ Resources:Attendance,Refresh %>"><span class="fa fa-repeat"  style="font-size:25px;"></span></asp:LinkButton>&nbsp;&nbsp;&nbsp;
                                            </div>


                                        </div>
                                    </div>
                                </div>
                            </div>


                            <div class="box box-warning box-solid" <%= GvAllowance.Rows.Count>0?"style='display:block'":"style='display:none'"%>>

                                <div class="box-body">
                                    <div class="row">
                                        <div class="col-md-12">
                                            <div class="flow">
                                                <asp:HiddenField ID="HDFSort" runat="server" />
                                                <asp:GridView CssClass="table-striped table-bordered table table-hover" ID="GvAllowance" runat="server" AutoGenerateColumns="False"
                                                    Width="100%" AllowPaging="True" OnPageIndexChanging="GvAllowance_PageIndexChanging"
                                                    AllowSorting="True" PageSize='<%# int.Parse(Session["GridSize"].ToString()) %>'
                                                    OnSorting="GvAllowance_Sorting">

                                                    <Columns>


                                                        <asp:TemplateField HeaderText="Action">
                                                            <ItemTemplate>
                                                                <div class="dropdown" style="position: absolute;">
                                                                    <button class="btn btn-default dropdown-toggle" type="button" data-toggle="dropdown">
                                                                        <i class="fa fa-ellipsis-h" aria-hidden="true"></i>
                                                                    </button>
                                                                    <ul class="dropdown-menu">
                                                                       

                                                                        <li <%= hdnCanEdit.Value=="true"?"style='display:block'":"style='display:none'" %>>
                                                                              <asp:LinkButton ID="btnEdit" runat="server" CommandArgument='<%# Eval("Allowance_Id") %>'
                                                                     OnCommand="btnEdit_Command"  CausesValidation="False"
                                                                     ><i class="fa fa-pencil"></i>Edit </asp:LinkButton>
                                                                        </li>
                                                                        <li <%= hdnCanDelete.Value=="true"?"style='display:block'":"style='display:none'" %>>
                                                                             <asp:LinkButton ID="IbtnDelete" runat="server" CausesValidation="False" CommandArgument='<%# Eval("Allowance_Id") %>'
                                                                    OnCommand="IbtnDelete_Command"
                                                                    ToolTip="<%$ Resources:Attendance,Delete %>" ><i class="fa fa-trash"></i>Delete</asp:LinkButton>
                                                                            <cc1:ConfirmButtonExtender ID="confirm1" runat="server" ConfirmText="<%$ Resources:Attendance,Are you sure you want to delete the record?%>" TargetControlID="IbtnDelete"></cc1:ConfirmButtonExtender>
                                                                        </li>

                                                                     
                                                                    </ul>
                                                                </div>

                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                       
                                                        <asp:BoundField DataField="Allowance_Id" HeaderText="<%$ Resources:Attendance,Allowance Id %>"
                                                            SortExpression="Allowance_Id">
                                                            <ItemStyle />
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="Allowance" HeaderText="<%$ Resources:Attendance,Allowance Name %>"
                                                            SortExpression="Allowance">
                                                            <ItemStyle />
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="Allowance_L" HeaderText="<%$ Resources:Attendance,Allowance Name(Local) %>"
                                                            SortExpression="Allowance_L">
                                                            <ItemStyle />
                                                        </asp:BoundField>
                                                        <asp:TemplateField HeaderText="<%$ Resources:Attendance,Account No %>">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblaccountname" runat="server" Text='<%#GetAccountNamebyTransId(Eval("Field1").ToString()) %>'></asp:Label>
                                                            </ItemTemplate>
                                                            <ItemStyle HorizontalAlign="Center" />
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
						<asp:Label ID="Label3" runat="server" Text="Advance Search"></asp:Label></h3>
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
                                        <asp:DropDownList ID="ddlFieldNameBin" runat="server" class="form-control">
                                            <asp:ListItem Text="<%$ Resources:Attendance, Allowance Id %>" Value="Allowance_Id" />
                                            <asp:ListItem Text="<%$ Resources:Attendance, Allowance Name %>" Value="Allowance"
                                                Selected="True" />
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
                                    <div class="col-lg-5">
                                        <asp:Panel ID="Panel1" runat="server" DefaultButton="btnbindBin">
                                            <asp:TextBox ID="txtValueBin" placeholder="Search from Content" runat="server" CssClass="form-control"></asp:TextBox>
                                        </asp:Panel>
                                    </div>
                                    <div class="col-lg-2" style="text-align: center;">
                                        <asp:LinkButton ID="btnbindBin"  runat="server"
                                            CausesValidation="False" ImageUrl="~/Images/search.png" OnClick="btnbindBin_Click"
                                            ToolTip="<%$ Resources:Attendance,Search %>"><span class="fa fa-search"  style="font-size:25px;"></span></asp:LinkButton>&nbsp;&nbsp;&nbsp;
                                        <asp:LinkButton ID="btnRefreshBin" runat="server" CausesValidation="False"
                                             OnClick="btnRefreshBin_Click" ToolTip="<%$ Resources:Attendance,Refresh %>"><span class="fa fa-repeat"  style="font-size:25px;"></span></asp:LinkButton>&nbsp;&nbsp;&nbsp;
                                        <asp:LinkButton ID="imgBtnRestore" runat="server" CausesValidation="False"
                                            OnClick="btnRestoreSelected_Click" ToolTip="<%$ Resources:Attendance,Refresh %>"><span class="far fa-lightbulb"  style="font-size:25px;"></span></asp:LinkButton>&nbsp;&nbsp;&nbsp;
                                        <asp:ImageButton ID="ImgbtnSelectAll" Visible="false" runat="server" Style="width: 30px;" CausesValidation="False"
                                            ImageUrl="~/Images/selectAll.png" OnClick="ImgbtnSelectAll_Click" ToolTip="<%$ Resources:Attendance,Select All %>"></asp:ImageButton>
                                    </div>
                                   
				</div>
			</div>
		</div>
	</div>


                        <div class="box box-warning box-solid"  <%= GvAllowanceBin.Rows.Count>0?"style='display:block'":"style='display:none'"%> >
                           
                            <div class="box-body">
                                <div class="row">
                                    <div class="col-md-12">
                                        <div class="flow">
                                            <asp:Label ID="lblSelectedRecord" runat="server" Visible="false"></asp:Label>
                                            <asp:GridView CssClass="table-striped table-bordered table table-hover" ID="GvAllowanceBin" runat="server" AutoGenerateColumns="False"
                                                Width="100%" AllowPaging="True" OnPageIndexChanging="GvAllowanceBin_PageIndexChanging"
                                                OnSorting="GvAllowanceBin_Sorting" DataKeyNames="Allowance_Id" AllowSorting="true" PageSize='<%# int.Parse(Session["GridSize"].ToString()) %>'>
                                                <Columns>
                                                    <asp:TemplateField>
                                                        <HeaderTemplate>
                                                            <asp:CheckBox ID="chkgvSelectAll" runat="server"
                                                                AutoPostBack="true" OnCheckedChanged="chkgvSelectAll_CheckedChanged" />
                                                        </HeaderTemplate>
                                                        <ItemTemplate>
                                                            <asp:CheckBox ID="chkgvSelect" runat="server" />
                                                        </ItemTemplate>
                                                        <ItemStyle HorizontalAlign="Center" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="<%$ Resources:Attendance,Allowance Id%>" SortExpression="Allowance_Id">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblgvAllowanceId" runat="server" Text='<%# Eval("Allowance_Id") %>'></asp:Label>
                                                        </ItemTemplate>
                                                        <ItemStyle />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="<%$ Resources:Attendance,Allowance Name %>" SortExpression="Allowance">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblgvAllowanceName" runat="server" Text='<%# Eval("Allowance") %>'></asp:Label>
                                                        </ItemTemplate>
                                                        <ItemStyle />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="<%$ Resources:Attendance,Allowance Name(Local) %>" SortExpression="Allowance_L">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblgvAllowanceNameL" runat="server" Text='<%# Eval("Allowance_L") %>'></asp:Label>
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
        function Li_Tab_New() {
            document.getElementById('<%= Btn_New.ClientID %>').click();
        }
        function Li_Tab_Bin() {
            document.getElementById('<%= Btn_Bin.ClientID %>').click();
        }
    </script>
</asp:Content>
