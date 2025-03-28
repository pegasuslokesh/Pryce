<%@ Page Title="" Language="C#" MasterPageFile="~/ERPMaster.master" AutoEventWireup="true" CodeFile="LeaveTypeMaster.aspx.cs" Inherits="MasterSetUp_LeaveTypeMaster" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="Head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="HeaderContent" runat="Server">
    <h1>
        <i class="fas fa-calendar-alt"></i>   &nbsp;&nbsp;
        <asp:Label ID="lblHeader" runat="server" Text="<%$ Resources:Attendance,Leave Type Setup%>"></asp:Label>
    </h1>
    <ol class="breadcrumb">
        <li><a href="#"><i class="fa fa-dashboard"></i>
            <asp:Label ID="Label6" runat="server" Text="<%$ Resources:Attendance,Attendance Module%>"></asp:Label></a></li>
        <li><a href="#"><i class="fa fa-dashboard"></i>
            <asp:Label ID="Label10" runat="server" Text="<%$ Resources:Attendance,Attendance Module%>"></asp:Label></a></li>
        <li class="active">
            <asp:Label ID="Label5" runat="server" Text="<%$ Resources:Attendance,Leave Type Setup%>"></asp:Label></li>
    </ol>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="Server">
    <asp:UpdatePanel ID="Update_Button" runat="server">
        <ContentTemplate>
            <asp:Button ID="Btn_Bin" runat="server" Style="display: none;" Text="Bin" OnClick="btnBin_Click" />
            <asp:HiddenField runat="server" ID="hdnCanEdit" />
            <asp:HiddenField runat="server" ID="hdnCanDelete" />
        </ContentTemplate>
    </asp:UpdatePanel>
    <asp:UpdateProgress ID="Update_Progress1" runat="server" AssociatedUpdatePanelID="Update_Button">
        <ProgressTemplate>
            <div class="modal_Progress">
                <div class="center_Progress">
                    <img alt="" src="../Images/ajax-loader2.gif" style="vertical-align: middle; background-color: White; border-radius: 10px;" />
                </div>
            </div>
        </ProgressTemplate>
    </asp:UpdateProgress>
    <div class="row">
        <div class="col-md-12">
            <div class="nav-tabs-custom">
                <ul class="nav nav-tabs pull-right bg-blue-gradient">
                    <li id="Li_Bin"><a onclick="Li_Bin_Click()" href="#Bin" data-toggle="tab">
                        <i class="fa fa-trash"></i>&nbsp;&nbsp;<asp:Label ID="Label13" runat="server" Text="<%$ Resources:Attendance,Bin %>"></asp:Label></a></li>
                    <li id="Li_New" class="active"><a href="#New" data-toggle="tab">
                        <asp:UpdatePanel ID="Update_Li" runat="server">
                            <ContentTemplate>
                                <i class="fa fa-file"></i>&nbsp;&nbsp;
                                <asp:Label ID="Lbl_Tab_New" runat="server" Text="<%$ Resources:Attendance,New%>"></asp:Label>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </a></li>
                    <li id="Li_List"><a href="#List" data-toggle="tab">
                        <i class="fa fa-list"></i>&nbsp;&nbsp;<asp:Label ID="Label1" runat="server" Text="<%$ Resources:Attendance,List %>"></asp:Label></a></li>
                </ul>
                <div class="tab-content">
                    <div class="tab-pane" id="List">
                        <asp:UpdatePanel ID="Update_List" runat="server">
                            <ContentTemplate>


                                <div class="row">
                                    <div class="col-md-12">
                                        <div id="Div1" runat="server" class="box box-info collapsed-box">
                                            <div class="box-header with-border">
                                                <h3 class="box-title">
                                                    <asp:Label ID="Label22" runat="server" Text="<%$ Resources:Attendance,Advance Search%>"></asp:Label></h3>
                                                &nbsp;&nbsp;|&nbsp;&nbsp;
					 <asp:Label ID="lblTotalRecords" Font-Bold="true" Text="<%$ Resources:Attendance,Total Records: 0 %>" runat="server"></asp:Label>
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
                                                        <asp:ListItem Selected="True" Text="<%$ Resources:Attendance,Leave Type Name %>" Value="Leave_Name"></asp:ListItem>
                                                        <asp:ListItem Text="<%$ Resources:Attendance,Leave Type Name(Local) %>" Value="Leave_Name_L"></asp:ListItem>
                                                        <asp:ListItem Text="<%$ Resources:Attendance,Leave Type Id %>" Value="Leave_Id"></asp:ListItem>
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
                                                        <asp:TextBox placeholder="Search from Content" ID="txtValue" runat="server" class="form-control"></asp:TextBox>
                                                    </asp:Panel>
                                                </div>
                                                <div class="col-lg-2">
                                                    <asp:LinkButton ID="btnbind" runat="server" CausesValidation="False" OnClick="btnbind_Click" ToolTip="<%$ Resources:Attendance,Search %>"><span class="fa fa-search"  style="font-size:25px;"></span></asp:LinkButton>&nbsp;&nbsp;&nbsp;
                                                    <asp:LinkButton ID="btnRefresh" runat="server" CausesValidation="False" OnClick="btnRefresh_Click" ToolTip="<%$ Resources:Attendance,Refresh %>"><span class="fa fa-repeat"  style="font-size:25px;"></span></asp:LinkButton>&nbsp;&nbsp;&nbsp;
                                                </div>

                                            </div>
                                        </div>
                                    </div>
                                </div>
                                <div class="box box-warning box-solid" <%= gvLeaveMaster.Rows.Count>0?"style='display:block'":"style='display:none'"%>>
                                    <div class="box-body">
                                        <div class="row">
                                            <div class="col-md-12">
                                                <div class="flow">
                                                    <asp:HiddenField ID="HDFSort" runat="server" />
                                                    <asp:GridView CssClass="table-striped table-bordered table table-hover" ID="gvLeaveMaster" PageSize="<%# PageControlCommon.GetPageSize() %>"
                                                        runat="server" AutoGenerateColumns="False" Width="100%" AllowPaging="True" AllowSorting="True"
                                                        OnPageIndexChanging="gvLeaveMaster_PageIndexChanging" OnSorting="gvLeaveMaster_OnSorting">
                                                        <Columns>

                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,Action%>">
                                                                <ItemTemplate>
                                                                    <div class="dropdown" style="position: absolute;">
                                                                        <button class="btn btn-default dropdown-toggle" type="button" data-toggle="dropdown">
                                                                            <i class="fa fa-ellipsis-h" aria-hidden="true"></i>
                                                                        </button>
                                                                        <ul class="dropdown-menu">
                                                                            <li <%= hdnCanEdit.Value=="true"?"style='display:block'":"style='display:none'" %>>
                                                                                <asp:LinkButton ID="btnEdit" runat="server" CommandArgument='<%# Eval("Leave_Id") %>' CausesValidation="False" OnCommand="btnEdit_Command"><i class="fa fa-pencil"></i><asp:Label ID="lbledit" runat="server" Text = "<%$ Resources:Attendance,Edit %>"></asp:Label> </asp:LinkButton>
                                                                            </li>
                                                                            <li <%= hdnCanDelete.Value=="true"?"style='display:block'":"style='display:none'" %>>
                                                                                <asp:LinkButton ID="IbtnDelete" runat="server" CausesValidation="False" CommandArgument='<%# Eval("Leave_Id") %>' OnCommand="IbtnDelete_Command"><i class="fa fa-trash"></i><asp:Label ID="Label14" runat="server" Text = "<%$ Resources:Attendance,Delete %>"></asp:Label></asp:LinkButton>
                                                                                <cc1:ConfirmButtonExtender ID="confirm1" runat="server" ConfirmText="<%$ Resources:Attendance,Are you sure you want to delete the record?%>" TargetControlID="IbtnDelete"></cc1:ConfirmButtonExtender>
                                                                            </li>

                                                                        </ul>
                                                                    </div>

                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,Leave Type Id %>" SortExpression="Leave_Id">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblLeaveId1" runat="server" Text='<%# Eval("Leave_Id") %>'></asp:Label>
                                                                </ItemTemplate>
                                                                <ItemStyle />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,Leave Type Name %>" SortExpression="Leave_Name">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lbLeaveName" runat="server" Text='<%# Eval("Leave_Name") %>'></asp:Label>
                                                                </ItemTemplate>
                                                                <ItemStyle />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,Leave Type Name(Local) %>"
                                                                SortExpression="Leave_Name_L">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblLeaveNameL" runat="server" Text='<%# Eval("Leave_Name_L") %>'></asp:Label>
                                                                </ItemTemplate>
                                                                <ItemStyle />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField Visible="false" HeaderText="<%$ Resources:Attendance,Is Partial Leave %>"
                                                                SortExpression="Is_Partial">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblLeavecode" runat="server" Text='<%# Eval("Is_Partial") %>'></asp:Label>
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
                    <div class="tab-pane active" id="New">
                        <asp:UpdatePanel ID="Update_New" runat="server">
                            <ContentTemplate>
                                <div class="row">
                                    <div class="col-md-12">
                                        <div class="box box-primary">
                                            <div class="box-body">
                                                <div class="form-group">
                                                    <div class="col-md-6">
                                                        <asp:Label ID="lblLeaveName" runat="server" Text="<%$ Resources:Attendance,Leave Type Name %>"></asp:Label>
                                                        <asp:TextBox ID="txtLeaveName" BackColor="#eeeeee" runat="server"
                                                            CssClass="form-control" AutoPostBack="true" OnTextChanged="txtLeaveName_OnTextChanged" />
                                                        <cc1:AutoCompleteExtender ID="AutoCompleteExtender1" runat="server" DelimiterCharacters=""
                                                            Enabled="True" ServiceMethod="GetCompletionListLeaveName" ServicePath="" CompletionInterval="100"
                                                            MinimumPrefixLength="1" CompletionSetCount="1" TargetControlID="txtLeaveName"
                                                            UseContextKey="True" CompletionListCssClass="autoCompleteList" CompletionListItemCssClass="autoCompleteListItem" CompletionListHighlightedItemCssClass="autoCompleteSelectedListItem" OnClientShown="resetPosition">
                                                        </cc1:AutoCompleteExtender>
                                                        <br />
                                                    </div>
                                                    <div class="col-md-6">
                                                        <asp:Label ID="lblLeaveNameL" runat="server" Text="<%$ Resources:Attendance,Leave Type Name(Local) %>"></asp:Label>
                                                        <asp:TextBox ID="txtLeaveNameL" runat="server" CssClass="form-control" />
                                                        <br />
                                                    </div>
                                                    <div class="col-md-6">
                                                        <asp:Label ID="Label2" runat="server" Text="<%$ Resources:Attendance,Required Service Days%>"></asp:Label>
                                                        <asp:TextBox ID="txtRequiredservicedays" runat="server" CssClass="form-control" />
                                                        <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender18" runat="server" Enabled="True"
                                                            TargetControlID="txtRequiredservicedays" FilterType="Numbers">
                                                        </cc1:FilteredTextBoxExtender>
                                                        <br />
                                                    </div>
                                                    <div class="col-md-6">
                                                        <asp:Label ID="Label7" runat="server" Text="<%$ Resources:Attendance,Max leave attempt(one time)%>"></asp:Label>
                                                        <asp:TextBox ID="txtMaxAttempt" runat="server" CssClass="form-control" />
                                                        <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtendertxtMaxAttempt" runat="server" Enabled="True"
                                                            TargetControlID="txtMaxAttempt" FilterType="Numbers">
                                                        </cc1:FilteredTextBoxExtender>
                                                        <br />
                                                    </div>
                                                    <div class="col-md-6" id="div_maxleavebal" runat="server">
                                                        <asp:Label ID="Label8" runat="server" Text="<%$ Resources:Attendance,Max Leave Balance%>"></asp:Label>
                                                        <asp:TextBox ID="txtMaxLeaveBalance" runat="server" CssClass="form-control" />
                                                        <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtendertxtMaxLeaveBalance" runat="server" Enabled="True"
                                                            TargetControlID="txtMaxLeaveBalance" FilterType="Numbers">
                                                        </cc1:FilteredTextBoxExtender>
                                                        <br />
                                                    </div>
                                                    <div class="col-md-3" id="div_LeaveSalaryGiven" runat="server">
                                                        <br />
                                                        <%--<asp:Label ID="Label3" runat="server"  Text="<%$ Resources:Attendance,Is Leave Salary Given %>"></asp:Label>--%>
                                                        <asp:RadioButton ID="chkIsLeaveSalaryGiven" runat="server" CssClass="form-control" Text="<%$ Resources:Attendance,Is Leave Salary Given %>" GroupName="LeaveType" />

                                                        <br />
                                                    </div>
                                                    <div class="col-md-3" id="div_NegativeBalance" runat="server">
                                                        <br />
                                                        <asp:RadioButton ID="chkIsNegativeBalance" runat="server" CssClass="form-control" Text="<%$ Resources:Attendance,Allow negative balances%>" GroupName="LeaveType" />
                                                    </div>

                                                    <div class="col-md-6">
                                                        <br />
                                                        <%--<asp:Label ID="Label3" runat="server"  Text="<%$ Resources:Attendance,Is Leave Salary Given %>"></asp:Label>--%>
                                                        <asp:CheckBox ID="chkLeaveApproval" runat="server" CssClass="form-control" Text="<%$ Resources:Attendance,Leave Approval Functionality%>" />
                                                        <br />
                                                    </div>



                                                    <div class="col-md-6" id="div_LeaveTransaction" runat="server">
                                                        <br />
                                                        <%--<asp:Label ID="Label3" runat="server"  Text="<%$ Resources:Attendance,Is Leave Salary Given %>"></asp:Label>--%>
                                                        <asp:CheckBox ID="chkLeaveTransaction" runat="server" CssClass="form-control" Text="<%$ Resources:Attendance,Leave transaction on shift uploading using excel file%>" />
                                                        <br />
                                                    </div>


                                                    <div class="col-md-6" runat="server">
                                                        <br />

                                                        <br />
                                                    </div>


                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="col-md-12">
                                        <div class="box box-primary">
                                            <div class="box-body">
                                                <div class="form-group">
                                                    <div class="col-md-12">
                                                        <asp:Label ID="Label4" runat="server" Text="<%$ Resources:Attendance,Deduction slab after finish paid days%>" Font-Bold="true"></asp:Label>
                                                        <br />
                                                    </div>
                                                    <div class="col-md-6">
                                                        <asp:Label ID="Label9" runat="server" Text="<%$ Resources:Attendance,Exceed Days From%>"></asp:Label>
                                                        <asp:TextBox ID="txtexceedDays" runat="server" CssClass="form-control" Enabled="false" Text="0" />
                                                        <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtendertxtexceedDays" runat="server" Enabled="True"
                                                            TargetControlID="txtexceedDays" FilterType="Numbers">
                                                        </cc1:FilteredTextBoxExtender>
                                                        <br />
                                                    </div>
                                                    <div class="col-md-6">
                                                        <asp:Label ID="Label11" runat="server" Text="<%$ Resources:Attendance,Exceed Days To%>"></asp:Label>
                                                        <asp:TextBox ID="txtexceedDaysto" runat="server" CssClass="form-control" />
                                                        <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtendertxtexceedDaysto" runat="server" Enabled="True"
                                                            TargetControlID="txtexceedDaysto" FilterType="Numbers">
                                                        </cc1:FilteredTextBoxExtender>
                                                        <br />
                                                    </div>
                                                    <div class="col-md-6">
                                                        <asp:Label ID="Label12" runat="server" Text="<%$ Resources:Attendance,Deduction(%)%>"></asp:Label>
                                                        <asp:TextBox ID="txtdeduction" runat="server" CssClass="form-control" />
                                                        <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtendertxtdeduction" runat="server" Enabled="True"
                                                            TargetControlID="txtdeduction" FilterType="Numbers">
                                                        </cc1:FilteredTextBoxExtender>
                                                        <br />
                                                    </div>
                                                    <div class="col-md-6">
                                                        <br />
                                                        <asp:Button ID="btndeduction" runat="server" Text="<%$ Resources:Attendance,Add %>"
                                                            CssClass="btn btn-primary" OnClick="btndeduction_Click" />

                                                        <asp:Button ID="btndeductionCancel" runat="server" Text="<%$ Resources:Attendance,Cancel %>"
                                                            CssClass="btn btn-primary" OnClick="btndeductionCancel_Click" />
                                                        <br />
                                                    </div>
                                                    <div class="col-md-12" class="flow">
                                                        <asp:GridView CssClass="table-striped table-bordered table table-hover" ID="GvDeductionDetail" runat="server" AutoGenerateColumns="False"
                                                            Width="100%" DataKeyNames="Trans_Id" ShowFooter="false">

                                                            <Columns>
                                                                <asp:TemplateField HeaderText="Edit">
                                                                    <ItemTemplate>
                                                                        <asp:ImageButton ID="imgBtnEmployeeEdit" runat="server" CommandArgument='<%# Eval("Trans_Id") %>'
                                                                            Height="16px" ImageUrl="~/Images/Edit.png"
                                                                            Width="16px" OnCommand="imgBtnEmployeeEdit_Command" />
                                                                    </ItemTemplate>
                                                                    <ItemStyle HorizontalAlign="Center" Width="16px" />
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="Delete">
                                                                    <ItemTemplate>
                                                                        <asp:ImageButton ID="imgBtnEmpoloyeeDelete" runat="server" CommandArgument='<%# Eval("Trans_Id") %>'
                                                                            Height="16px" ImageUrl="~/Images/Erase.png" Width="16px" OnCommand="imgBtnEmpoloyeeDelete_Command" />
                                                                    </ItemTemplate>
                                                                    <ItemStyle HorizontalAlign="Center" Width="16px" />
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="<%$ Resources:Attendance,Exceed Days From%>">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblDaysFrom" runat="server" Text='<%#Eval("DaysFrom") %>' />
                                                                        <asp:Label ID="lblTransId" Visible="false" runat="server" Text='<%#Eval("Trans_Id") %>' />
                                                                    </ItemTemplate>
                                                                    <ItemStyle HorizontalAlign="Center" Width="60px" />
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="<%$ Resources:Attendance,Exceed Days To%>">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblDaysTo" runat="server" Text='<%#Eval("DaysTo") %>' />
                                                                    </ItemTemplate>
                                                                    <ItemStyle HorizontalAlign="Center" Width="60px" />
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="<%$ Resources:Attendance,Deduction(%)%>">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lbldeductionpercentage" runat="server" Text='<%#Eval("Deduction_Percentage") %>' />
                                                                    </ItemTemplate>
                                                                    <ItemStyle HorizontalAlign="Center" Width="60px" />
                                                                </asp:TemplateField>
                                                            </Columns>

                                                            <PagerStyle CssClass="pagination-ys" />

                                                        </asp:GridView>
                                                        <asp:HiddenField ID="hdndeductionTransId" runat="server" />
                                                        <br />
                                                    </div>


                                                    <hr />
                                                    <div class="col-md-12" style="text-align: center">
                                                        <asp:Button ID="btnSave" runat="server" Text="<%$ Resources:Attendance,Save %>"
                                                            Visible="false" CssClass="btn btn-success" ValidationGroup="a" OnClick="btnSave_Click" />

                                                        <asp:Button ID="btnReset" runat="server" Text="<%$ Resources:Attendance,Reset %>"
                                                            CssClass="btn btn-primary" CausesValidation="False" OnClick="btnReset_Click" />

                                                        <asp:Button ID="btnCancel" runat="server" Text="<%$ Resources:Attendance,Cancel %>"
                                                            CssClass="btn btn-danger" CausesValidation="False" OnClick="btnCancel_Click" />
                                                        <br />
                                                        <asp:HiddenField ID="editid" runat="server" />
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
					 <asp:Label ID="lblbinTotalRecords" Font-Bold="true" Text="<%$ Resources:Attendance,Total Records: 0 %>" runat="server"></asp:Label>

                                                <div class="box-tools pull-right">
                                                    <button type="button" class="btn btn-box-tool" data-widget="collapse">
                                                        <i id="I2" runat="server" class="fa fa-plus"></i>
                                                    </button>
                                                </div>
                                            </div>
                                            <div class="box-body">
                                                <div class="col-lg-3">
                                                    <asp:DropDownList ID="ddlbinFieldName" runat="server" class="form-control">
                                                        <asp:ListItem Selected="True" Text="<%$ Resources:Attendance,Leave Type Name %>" Value="Leave_Name"></asp:ListItem>
                                                        <asp:ListItem Text="<%$ Resources:Attendance,Leave Type Name(Local) %>" Value="Leave_Name_L"></asp:ListItem>
                                                        <asp:ListItem Text="<%$ Resources:Attendance,Leave Type Id %>" Value="Leave_Id"></asp:ListItem>
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
                                                <div class="col-lg-2">
                                                    <asp:Panel ID="Panel2" runat="server" DefaultButton="btnbinbind">
                                                        <asp:TextBox ID="txtbinValue" runat="server" class="form-control"></asp:TextBox>
                                                    </asp:Panel>
                                                </div>
                                                <div class="col-lg-3">
                                                    <asp:LinkButton ID="btnbinbind" runat="server"  CausesValidation="False"  OnClick="btnbinbind_Click" ToolTip="<%$ Resources:Attendance,Search %>"><span class="fa fa-search"  style="font-size:25px;"></span></asp:LinkButton>&nbsp;&nbsp;&nbsp;
                                                    <asp:LinkButton ID="btnbinRefresh" runat="server" CausesValidation="False"  OnClick="btnbinRefresh_Click" ToolTip="<%$ Resources:Attendance,Refresh %>"><span class="fa fa-repeat"  style="font-size:25px;"></span></asp:LinkButton>&nbsp;&nbsp;&nbsp;
                                                    <asp:LinkButton ID="imgBtnRestore" CausesValidation="False" runat="server"  Visible="false" OnClick="imgBtnRestore_Click" ToolTip="<%$ Resources:Attendance, Active %>"><span class="far fa-lightbulb"  style="font-size:25px;"></span></asp:LinkButton>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                                <div class="box box-warning box-solid"  <%= gvLeaveMasterBin.Rows.Count>0?"style='display:block'":"style='display:none'"%>  >
                                    <div class="box-body">
                                        <div class="row">
                                            <div class="col-md-12">
                                                <div class="flow">
                                                    <asp:GridView CssClass="table-striped table-bordered table table-hover" ID="gvLeaveMasterBin" PageSize="<%# PageControlCommon.GetPageSize() %>"
                                                        runat="server" AutoGenerateColumns="False" DataKeyNames="Leave_Id" Width="100%" AllowPaging="True" OnPageIndexChanging="gvLeaveMasterBin_PageIndexChanging"
                                                        OnSorting="gvLeaveMasterBin_OnSorting" AllowSorting="true">
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
                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,Leave Type Id %>" SortExpression="Leave_Id">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblLeaveId1" runat="server" Text='<%# Eval("Leave_Id") %>'></asp:Label>
                                                                </ItemTemplate>
                                                                <ItemStyle />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,Leave Type Name %>" SortExpression="Leave_Name">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lbLeaveName" runat="server" Text='<%# Eval("Leave_Name") %>'></asp:Label>
                                                                    <asp:Label ID="lblLeaveId" Visible="false" runat="server" Text='<%# Eval("Leave_Id") %>'></asp:Label>
                                                                </ItemTemplate>
                                                                <ItemStyle />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,Leave Type Name(Local) %>"
                                                                SortExpression="Leave_Name_L">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblLeaveNameL" runat="server" Text='<%# Eval("Leave_Name_L") %>'></asp:Label>
                                                                </ItemTemplate>
                                                                <ItemStyle />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField Visible="false" HeaderText="<%$ Resources:Attendance,Is Partial Leave %>"
                                                                SortExpression="Is_Partial">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblLeavecode" runat="server" Text='<%# Eval("Is_Partial") %>'></asp:Label>
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
        function Li_Bin_Click() {
            document.getElementById("<%=Btn_Bin.ClientID %>").click();
        }

        function Li_Edit_Active() {
            $('#Li_List').removeClass('active');
            $('#List').removeClass('active');

            $('#Li_New').addClass('active');
            $('#New').addClass('active');
        }

        function LI_List_Active() {
            $("#Li_List").addClass("active");
            $("#List").addClass("active");

            $("#Li_New").removeClass("active");
            $("#New").removeClass("active");
        }

        function showCalendar(sender, args) {

            var ctlName = sender._textbox._element.name;

            ctlName = ctlName.replace('$', '_');
            ctlName = ctlName.replace('$', '_');

            var processingControl = $get(ctlName);
            //var targetCtlHeight = processingControl.clientHeight;
            sender._popupDiv.parentElement.style.top = processingControl.offsetTop + processingControl.clientHeight + 'px';
            sender._popupDiv.parentElement.style.left = processingControl.offsetLeft + 'px';

            var positionTop = processingControl.clientHeight + processingControl.offsetTop;
            var positionLeft = processingControl.offsetLeft;
            var processingParent;
            var continueLoop = false;

            do {
                // If the control has parents continue loop.
                if (processingControl.offsetParent != null) {
                    processingParent = processingControl.offsetParent;
                    positionTop += processingParent.offsetTop;
                    positionLeft += processingParent.offsetLeft;
                    processingControl = processingParent;
                    continueLoop = true;
                }
                else {
                    continueLoop = false;
                }
            } while (continueLoop);

            sender._popupDiv.parentElement.style.top = positionTop + 2 + 'px';
            sender._popupDiv.parentElement.style.left = positionLeft + 'px';
            sender._popupBehavior._element.style.zIndex = 10005;
        }

    </script>
</asp:Content>

