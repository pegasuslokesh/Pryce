<%@ Page Title="" Language="C#" MasterPageFile="~/ERPMaster.master" AutoEventWireup="true" CodeFile="ShiftManagement.aspx.cs" Inherits="Attendance_ShiftManagement" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="Head" runat="Server">
    <style>
        .GridStyle {
            border: 1px solid black;
            background-color: White;
            font-family: arial;
            font-size: 12px;
            border-collapse: collapse;
            margin-bottom: 0px;
        }

            .GridStyle tr {
                border: 1px solid black;
                color: Black;
                height: 25px;
            }
            /* Your grid header column style */
            .GridStyle th {
                background-color: rgb(217, 231, 255);
                /*border: none;*/
                /*text-align: left;*/
                font-weight: bold;
                /*font-size: 15px;*/
                padding: 4px;
                color: Black;
                text-align: center;
                width: 100%;
            }
            /* Your grid header link style */
            .GridStyle tr th a, .GridStyle tr th a:visited {
                color: Black;
            }

            .GridStyle tr th, .GridStyle tr td table tr td {
                /*border: none;*/
            }

            .GridStyle td {
                border-bottom: 1px solid black;
                padding: 2px;
            }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="HeaderContent" runat="Server">
    <h1>
 <i class="fas fa-calendar-day"></i>&nbsp;&nbsp;
        <asp:Label ID="lblHeader" runat="server" Text="<%$ Resources:Attendance,Shift Management Setup%>"></asp:Label>
    </h1>
    <ol class="breadcrumb">
        <li><a href="#"><i class="fa fa-dashboard"></i>
            <asp:Label ID="Label6" runat="server" Text="<%$ Resources:Attendance,Attendance Module%>"></asp:Label></a></li>
        <li><a href="#"><i class="fa fa-dashboard"></i>
            <asp:Label ID="Label10" runat="server" Text="<%$ Resources:Attendance,Attendance Module%>"></asp:Label></a></li>
        <li class="active">
            <asp:Label ID="Label5" runat="server" Text="<%$ Resources:Attendance,Shift Management Setup%>"></asp:Label></li>
    </ol>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="Server">
    <asp:UpdatePanel ID="Update_Button" runat="server">
        <ContentTemplate>
            <asp:Button ID="Btn_List" Style="display: none;" runat="server" OnClick="btnList_Click" Text="List" />
            <asp:Button ID="Btn_Bin" Style="display: none;" runat="server" OnClick="btnBin_Click" Text="Bin" />
            <asp:Button ID="Btn_Upload" Style="display: none;" runat="server" OnClick="Btn_Upload_Click" Text="Upload" />
            <asp:Button ID="Btn_Modal_Shift_View" Style="display: none;" runat="server" data-toggle="modal" data-target="#Modal_Shift_View" Text="Modal Shift View" />
            <asp:HiddenField runat="server" ID="hdnCanView" />
            <asp:HiddenField runat="server" ID="hdnCanEdit" />
            <asp:HiddenField runat="server" ID="hdnCanDelete" />
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
    <div class="row">
        <div class="col-md-12">
            <div class="nav-tabs-custom">
                <ul class="nav nav-tabs pull-right bg-blue-gradient">
                    <li id="Li_Upload"><a href="#Upload" onclick="Li_Tab_Upload()" data-toggle="tab">
                        <i class="fa fa-upload"></i>&nbsp;&nbsp;
                        <asp:Label ID="Label11" runat="server" Text="<%$ Resources:Attendance,Upload %>"></asp:Label></a></li>
                    <li id="Li_Bin"><a href="#Bin" onclick="Li_Tab_Bin()" data-toggle="tab">
                        <i class="fa fa-trash"></i>&nbsp;&nbsp;<asp:Label ID="Label13" runat="server" Text="<%$ Resources:Attendance,Bin %>"></asp:Label></a></li>
                    <li id="Li_New"><a href="#New" data-toggle="tab">
                        <asp:UpdatePanel ID="Update_Li" runat="server">
                            <ContentTemplate>
                                <i class="fa fa-file"></i>&nbsp;&nbsp;
                                <asp:Label ID="Lbl_Tab_New" runat="server" Text="<%$ Resources:Attendance,New%>"></asp:Label>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </a></li>
                    <li id="Li_List" class="active"><a href="#List" onclick="Li_Tab_List()" data-toggle="tab">
                        <i class="fa fa-list"></i>&nbsp;&nbsp;<asp:Label ID="Label1" runat="server" Text="<%$ Resources:Attendance,List %>"></asp:Label></a></li>
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
                                                    <asp:Label ID="Label22" runat="server" Text="<%$ Resources:Attendance,Advance Search%>"></asp:Label></h3>
                                                &nbsp;&nbsp;|&nbsp;&nbsp;
				  <asp:Label ID="lblTotalRecords" runat="server" Font-Bold="true" Text="<%$ Resources:Attendance,Total Records: 0 %>"></asp:Label>
                                                <asp:Label ID="lblSelectedRecord" runat="server" Visible="false"></asp:Label>
                                                <asp:HiddenField ID="HDFSort" runat="server" />
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
                                                    <asp:DropDownList ID="ddlFieldName" runat="server" CssClass="form-control">
                                                        <asp:ListItem Selected="True" Text="<%$ Resources:Attendance,Shift Name %>" Value="Shift_Name"></asp:ListItem>
                                                        <asp:ListItem Text="<%$ Resources:Attendance,Shift Name(Local) %>" Value="Shift_Name_L"></asp:ListItem>
                                                        <asp:ListItem Text="<%$ Resources:Attendance,Cycle Unit %>" Value="Cycle_Unit"></asp:ListItem>
                                                        <asp:ListItem Text="<%$ Resources:Attendance,Cycle No. %>" Value="Cycle_No"></asp:ListItem>
                                                        <asp:ListItem Text="<%$ Resources:Attendance,Shift Id %>" Value="Shift_Id"></asp:ListItem>
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
                                                        <asp:TextBox placeholder="Search From Content" ID="txtValue" runat="server" CssClass="form-control"></asp:TextBox>
                                                    </asp:Panel>
                                                </div>
                                                <div class="col-lg-2">
                                                    <asp:LinkButton ID="btnbind" runat="server" CausesValidation="False" OnClick="btnbind_Click" ToolTip="<%$ Resources:Attendance,Search %>"><span class="fa fa-search"  style="font-size:25px;"></span></asp:LinkButton>&nbsp;&nbsp;&nbsp;
                                                    <asp:LinkButton ID="btnRefresh" runat="server" CausesValidation="False" OnClick="btnRefresh_Click" ToolTip="<%$ Resources:Attendance,Refresh %>"><span class="fa fa-repeat"  style="font-size:25px;"></span></asp:LinkButton>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>

                                <div class="box box-warning box-solid" <%= gvShift.Rows.Count>0?"style='display:block'":"style='display:none'"%>>
                                    <div class="box-body">
                                        <div class="row">
                                            <div class="col-md-12">
                                                <div class="flow">

                                                    <asp:HiddenField ID="hdn_locID" runat="server" />

                                                    <asp:GridView CssClass="table-striped table-bordered table table-hover" ID="gvShift" PageSize="<%# PageControlCommon.GetPageSize() %>" runat="server"
                                                        AutoGenerateColumns="False" Width="100%" AllowPaging="True" AllowSorting="True"
                                                        OnPageIndexChanging="gvShift_PageIndexChanging" OnSorting="gvShift_OnSorting">
                                                        <Columns>

                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,Action%>">
                                                                <ItemTemplate>
                                                                    <div class="dropdown" style="position: absolute;">
                                                                        <button class="btn btn-default dropdown-toggle" type="button" data-toggle="dropdown">
                                                                            <i class="fa fa-ellipsis-h" aria-hidden="true"></i>
                                                                        </button>
                                                                        <ul class="dropdown-menu">

                                                                            <li <%= hdnCanView.Value=="true"?"style='display:block'":"style='display:none'" %>>
                                                                                <asp:LinkButton ID="btnview" runat="server" CommandArgument='<%# Eval("Shift_Id") %>' CausesValidation="False" OnCommand="btnView_Command"></i><%# Resources.Attendance.View%></asp:LinkButton>
                                                                            </li>

                                                                            <li <%= hdnCanEdit.Value=="true"?"style='display:block'":"style='display:none'" %>>
                                                                                <asp:LinkButton ID="btnEdit" runat="server" CommandArgument='<%# Eval("Shift_Id") %>' CausesValidation="False" OnCommand="btnEdit_Command"><i class="fa fa-pencil"></i><%# Resources.Attendance.Edit%> </asp:LinkButton>
                                                                            </li>
                                                                            <li <%= hdnCanDelete.Value=="true"?"style='display:block'":"style='display:none'" %>>
                                                                                <asp:LinkButton ID="IbtnDelete" runat="server" CausesValidation="False" CommandArgument='<%# Eval("Shift_Id") %>' OnCommand="IbtnDelete_Command"><i class="fa fa-trash"></i><%# Resources.Attendance.Delete%></asp:LinkButton>
                                                                                <cc1:ConfirmButtonExtender ID="confirm1" runat="server" ConfirmText="<%$ Resources:Attendance,Are you sure you want to delete the record?%>" TargetControlID="IbtnDelete"></cc1:ConfirmButtonExtender>
                                                                            </li>
                                                                        </ul>
                                                                    </div>

                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,Shift Name %>" SortExpression="Shift_Name">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lbShiftName" runat="server" Text='<%# Eval("Shift_Name") %>'></asp:Label>
                                                                </ItemTemplate>
                                                                <ItemStyle />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,Shift Name(Local) %>" SortExpression="Shift_Name_L">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblShiftNameL" runat="server" Text='<%# Eval("Shift_Name_L") %>'></asp:Label>
                                                                </ItemTemplate>
                                                                <ItemStyle />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,Cycle No. %>" SortExpression="Cycle_No">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblCycleNo" runat="server" Text='<%# Eval("Cycle_No") %>'></asp:Label>
                                                                </ItemTemplate>
                                                                <ItemStyle />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,Cycle Unit %>" SortExpression="Cycle_Unit">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblCycleUnit" runat="server" Text='<%# Eval("Cycle_Unit") %>'></asp:Label>
                                                                </ItemTemplate>
                                                                <ItemStyle />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,Apply From %>" SortExpression="Apply_From">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblApplyDate" runat="server" Text='<%# GetDate(Eval("Apply_From")) %>'></asp:Label>
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
                                    <!-- /.box-body -->
                                </div>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </div>
                    <div class="tab-pane" id="New">
                        <asp:UpdatePanel ID="Update_New" runat="server">
                            <ContentTemplate>
                                <div id="Div_Main" runat="server" class="row">
                                    <div class="col-md-12">
                                        <div class="box box-primary">
                                            <div class="box-body">
                                                <div class="form-group">

                                                     <div class="col-md-12">
                                                        <asp:Label ID="Label17" runat="server" Text="<%$ Resources:Attendance,Location Name %>"></asp:Label>
                                                        <asp:DropDownList ID="ddlLocNew" runat="server" Class="form-control">
                                                        </asp:DropDownList>
                                                          <br />
                                                    </div>

                                                    <div class="col-md-6">
                                                        <asp:HiddenField ID="editid" runat="server" />
                                                        <asp:Label ID="lblShiftName" runat="server" Text="<%$ Resources:Attendance,Shift Name %>"></asp:Label>
                                                        <a style="color: Red">*</a>
                                                        <asp:RequiredFieldValidator EnableClientScript="true" Style="float: right;" runat="server" ID="RequiredFieldValidator3" ValidationGroup="Save"
                                                            Display="Dynamic" SetFocusOnError="true" ControlToValidate="txtShiftName" ErrorMessage="<%$ Resources:Attendance,Enter Shift Name %>"></asp:RequiredFieldValidator>

                                                        <asp:TextBox ID="txtShiftName" BackColor="#eeeeee" runat="server" AutoPostBack="true" OnTextChanged="txtShiftName_OnTextChanged"
                                                            CssClass="form-control" />
                                                        <cc1:AutoCompleteExtender ID="AutoCompleteExtender1" runat="server" DelimiterCharacters=""
                                                            Enabled="True" ServiceMethod="GetCompletionListShiftName" ServicePath="" CompletionInterval="100"
                                                            MinimumPrefixLength="1" CompletionSetCount="1" TargetControlID="txtShiftName"
                                                            UseContextKey="True" CompletionListCssClass="autoCompleteList" CompletionListItemCssClass="autoCompleteListItem" CompletionListHighlightedItemCssClass="autoCompleteSelectedListItem" OnClientShown="resetPosition">
                                                        </cc1:AutoCompleteExtender>
                                                        <br />
                                                    </div>
                                                    <div class="col-md-6">
                                                        <asp:Label ID="lblShiftNameL" runat="server" Text="<%$ Resources:Attendance,Shift Name(Local) %>"></asp:Label>
                                                        <asp:TextBox ID="txtShiftNameL" runat="server" CssClass="form-control" />
                                                        <br />
                                                    </div>
                                                    <div class="col-md-6">
                                                        <asp:Label ID="lblShiftCode" runat="server" Text="<%$ Resources:Attendance,Cycle No. %>"></asp:Label>
                                                        <a style="color: Red">*</a>
                                                        <asp:RequiredFieldValidator EnableClientScript="true" Style="float: right;" runat="server" ID="RequiredFieldValidator1" ValidationGroup="Save"
                                                            Display="Dynamic" SetFocusOnError="true" ControlToValidate="txtCycleNo" ErrorMessage="<%$ Resources:Attendance,Enter Cycle No. %>"></asp:RequiredFieldValidator>

                                                        <asp:TextBox ID="txtCycleNo" runat="server" CssClass="form-control" />
                                                        <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" runat="server" Enabled="True"
                                                            TargetControlID="txtCycleNo" ValidChars="1,2,3,4,5,6,7,8,9">
                                                        </cc1:FilteredTextBoxExtender>
                                                        <br />
                                                    </div>
                                                    <div class="col-md-6">
                                                        <asp:Label ID="Label2" runat="server" Text="<%$ Resources:Attendance,Cycle Unit %>"></asp:Label>
                                                        <asp:DropDownList ID="ddlCycleUnit" CssClass="form-control" runat="server">
                                                            <asp:ListItem Selected="True" Value="7">Week</asp:ListItem>
                                                            <asp:ListItem Value="1">Day</asp:ListItem>
                                                            <asp:ListItem Value="31">Month</asp:ListItem>
                                                        </asp:DropDownList>
                                                        <br />
                                                    </div>
                                                    <div class="col-md-6">

                                                        <asp:Label ID="Label3" runat="server" Text="<%$ Resources:Attendance,Apply From %>"></asp:Label>
                                                        <a style="color: Red">*</a>
                                                        <asp:RequiredFieldValidator EnableClientScript="true" Style="float: right;" runat="server" ID="RequiredFieldValidator2" ValidationGroup="Save"
                                                            Display="Dynamic" SetFocusOnError="true" ControlToValidate="txtApplyFrom" ErrorMessage="<%$ Resources:Attendance,Enter Apply From %>"></asp:RequiredFieldValidator>

                                                        <asp:TextBox ID="txtApplyFrom" runat="server" CssClass="form-control" />
                                                        <cc1:CalendarExtender OnClientShown="showCalendar" ID="CalendarExtender2" runat="server" Enabled="True" TargetControlID="txtApplyFrom">
                                                        </cc1:CalendarExtender>
                                                        <br />
                                                    </div>
                                                    <div class="col-md-12" style="text-align: center">

                                                        <asp:Button ID="btnSave" runat="server" Text="<%$ Resources:Attendance,Save %>"
                                                            Visible="false" CssClass="btn btn-success" ValidationGroup="Save" OnClick="btnSave_Click" />

                                                        <asp:Button ID="btnReset" runat="server" Text="<%$ Resources:Attendance,Reset %>"
                                                            CssClass="btn btn-primary" CausesValidation="False" OnClick="btnReset_Click" />

                                                        <asp:Button ID="btnCancel" runat="server" Text="<%$ Resources:Attendance,Cancel %>"
                                                            CssClass="btn btn-danger" CausesValidation="False" OnClick="btnCancel_Click" />

                                                        <asp:Button ID="btnAddTime" runat="server" ValidationGroup="Save" Text="<%$ Resources:Attendance,Add Time %>" OnClick="btnAddShift_Click"
                                                            CssClass="btn btn-primary" />

                                                        <asp:Button ID="btnClearAll" runat="server" Text="<%$ Resources:Attendance,Clear All %>" OnClick="btnClearAll_OnClick"
                                                            CssClass="btn btn-primary" />


                                                        <asp:Button ID="btnDelete" runat="server" CausesValidation="False" Text="<%$ Resources:Attendance,Delete %>"
                                                            OnClick="btnDelete_Click" CssClass="btn btn-primary" />
                                                        <br />
                                                    </div>
                                                    <div id="PnlViewShift" visible="false" runat="server" class="col-md-12">
                                                        <div class="flow">
                                                            <br />
                                                            <asp:GridView CssClass="table-striped table-bordered table table-hover" ID="gvShiftView" runat="server" AutoGenerateColumns="false" PageSize="<%# PageControlCommon.GetPageSize() %>"
                                                                Width="100%">
                                                                <Columns>
                                                                    <asp:TemplateField>
                                                                        <HeaderTemplate>
                                                                            <asp:CheckBox ID="chkAllDay" runat="server" AutoPostBack="true" OnCheckedChanged="ChkAllDay_CheckedChanged" />
                                                                        </HeaderTemplate>
                                                                        <HeaderStyle HorizontalAlign="Left" />
                                                                        <ItemTemplate>
                                                                            <asp:CheckBox ID="chkDay" runat="server" />
                                                                            <asp:HiddenField ID="hdDate" runat="server" Value='<%# Eval("EDutyTime") %>' />
                                                                            <asp:HiddenField ID="hdnCycle_Type" runat="server" Value='<%# Eval("Cycle_Type") %>' />
                                                                            <asp:HiddenField ID="hdnCycle_Day" runat="server" Value='<%# Eval("Cycle_Day") %>' />
                                                                        </ItemTemplate>
                                                                        <ItemStyle HorizontalAlign="Left" />
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="<%$ Resources:Attendance,Day %>">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblDAy" runat="server" Text='<%# WriteDays(Eval("EDutyTime")) %>' />
                                                                        </ItemTemplate>
                                                                        <ItemStyle HorizontalAlign="Left" />
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="<%$ Resources:Attendance,Time Table %>">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblTimeTable" runat="server" Text='<%# Eval("TimeTable_Id") %>' />
                                                                        </ItemTemplate>
                                                                        <ItemStyle HorizontalAlign="Left" />
                                                                    </asp:TemplateField>
                                                                </Columns>


                                                                <PagerStyle CssClass="pagination-ys" />


                                                            </asp:GridView>
                                                        </div>
                                                        <div class="flow">
                                                            <br />
                                                            <asp:GridView CssClass="table-striped table-bordered table table-hover" ID="gvShiftNew" OnRowDataBound="gvShiftNew_RowDataBound" runat="server" AutoGenerateColumns="true" PageSize="<%# PageControlCommon.GetPageSize() %>"
                                                                Width="100%">
                                                                <Columns>
                                                                    <asp:TemplateField>
                                                                        <HeaderTemplate>
                                                                            <asp:CheckBox ID="chkAllDay" runat="server" AutoPostBack="true" OnCheckedChanged="ChkAllDay_CheckedChanged1" />
                                                                        </HeaderTemplate>
                                                                        <HeaderStyle HorizontalAlign="Left" />
                                                                        <ItemTemplate>
                                                                            <asp:CheckBox ID="chkDay" runat="server" />

                                                                            <asp:HiddenField ID="hdnCycle_Type" runat="server" Value='<%# Eval("Cycle_Type") %>' />
                                                                            <asp:HiddenField ID="hdnCycle_Day" runat="server" Value='<%# Eval("Cycle_Day") %>' />
                                                                        </ItemTemplate>
                                                                        <ItemStyle HorizontalAlign="Left" />
                                                                    </asp:TemplateField>

                                                                </Columns>
                                                                <SelectedRowStyle CssClass="Invgridrow" />


                                                                <PagerStyle CssClass="pagination-ys" />


                                                            </asp:GridView>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>

                                <div id="PanelShiftAss" runat="server" visible="False" class="row">
                                    <div class="col-md-12">
                                        <div class="box box-primary">
                                            <div class="box-header with-border">
                                            </div>
                                            <div class="box-body">
                                                <div class="form-group">
                                                    <div class="col-md-3">
                                                        <asp:Label ID="Label4" runat="server" Font-Bold="True" Text="<%$ Resources:Attendance,Shift Id %>"></asp:Label>
                                                        &nbsp:&nbsp<asp:Label ID="txtShiftId" runat="server" Style="margin-left: 10px"></asp:Label>
                                                    </div>
                                                    <div class="col-md-5">
                                                        <asp:Label ID="Label7" runat="server" Font-Bold="True" Text="<%$ Resources:Attendance,Shift Name %>"></asp:Label>
                                                        &nbsp:&nbsp<asp:Label ID="lblShiftNameIs" runat="server" Style="margin-left: 10px" Font-Bold="True"></asp:Label>
                                                    </div>
                                                    <div class="col-md-4">
                                                        <asp:Button ID="btnOk" runat="server" CausesValidation="False" CssClass="btn btn-success"
                                                            ImageUrl="~/Images/buttonSave.png" OnClick="btnOk_Click" Text="<%$ Resources:Attendance,Save %>" />

                                                        <asp:Button ID="btnCancelPanel" runat="server" CausesValidation="False" CssClass="btn btn-danger"
                                                            ImageUrl="~/Images/buttonCancel.png" OnClick="btnCancel_Click"
                                                            Text="<%$ Resources:Attendance,Cancel %>" />

                                                        <asp:Button ID="Button3" runat="server" Text="<%$ Resources:Attendance,Select All %>" OnClick="btnSelectAll_OnClick"
                                                            CssClass="btn btn-primary" />

                                                        <asp:Button ID="Button1" runat="server" CausesValidation="False" CssClass="btn btn-primary"
                                                            OnClick="Button1_Click" Text="<%$ Resources:Attendance,Clear All %>" />
                                                    </div>
                                                    <br />
                                                    <div class="row">
                                                        <br />
                                                        <div class="col-md-6">
                                                            <div class="box box-primary">
                                                                <div class="box-header with-border">
                                                                    <h3 class="box-title">
                                                                        <asp:Label ID="Label9" runat="server" Font-Bold="True" Font-Names="Verdana"
                                                                            Font-Size="12pt" Style="font-weight: 700" Text="Time Table"></asp:Label></h3>
                                                                </div>
                                                                <div class="box-body">
                                                                    <div class="form-group">
                                                                        <asp:Label ID="lblSelectShift" runat="server" Text="<%$ Resources:Attendance,Select Time Table %>"></asp:Label>
                                                                        <div class="input-group">
                                                                            <asp:TextBox ID="txtTimeTable" runat="server" CssClass="form-control" BackColor="#eeeeee"
                                                                                AutoPostBack="true" OnTextChanged="txtTimeTable_textChanged" />
                                                                            <cc1:AutoCompleteExtender ID="AutoCompleteExtender5" runat="server" DelimiterCharacters=""
                                                                                Enabled="True" ServiceMethod="GetCompletionListTimeTableName" ServicePath="" CompletionInterval="100"
                                                                                MinimumPrefixLength="1" CompletionSetCount="1" TargetControlID="txtTimeTable" UseContextKey="True"
                                                                                CompletionListCssClass="autoCompleteList" CompletionListItemCssClass="autoCompleteListItem" CompletionListHighlightedItemCssClass="autoCompleteSelectedListItem" OnClientShown="resetPosition">
                                                                            </cc1:AutoCompleteExtender>
                                                                            <div class="input-group-btn">
                                                                                <asp:Button ID="btnAdd" runat="server" CssClass="btn btn-primary" OnClick="btnAdd_Click"
                                                                                    Text="Add" />
                                                                            </div>
                                                                        </div>
                                                                        <br />
                                                                        <asp:CheckBoxList ID="chkTimeTableList" runat="server" AutoPostBack="True" CellPadding="5"
                                                                            CellSpacing="5" OnSelectedIndexChanged="chkTimeTableList_SelectedIndexChanged"
                                                                            RepeatColumns="1">
                                                                        </asp:CheckBoxList>
                                                                    </div>
                                                                </div>
                                                            </div>
                                                        </div>
                                                        <div class="col-md-6">
                                                            <div class="box box-primary">
                                                                <div class="box-header with-border">
                                                                    <h3 class="box-title">
                                                                        <asp:Label ID="Label89" runat="server" Font-Bold="True" Font-Names="Verdana"
                                                                            Font-Size="12pt" Style="font-weight: 700" Text="Days For Time Table"></asp:Label>
                                                                    </h3>
                                                                </div>
                                                                <div class="box-body">
                                                                    <div class="form-group">
                                                                        <%--<asp:Label ID="lblselectdate" runat="server" Text="<%$ Resources:Attendance,Select Days For Time Table %>"></asp:Label>--%>
                                                                        <div style="overflow: auto">
                                                                            <asp:CheckBoxList ID="chkDayUnderPeriod" runat="server" RepeatDirection="Vertical"
                                                                                RepeatColumns="2" Width="100%">
                                                                            </asp:CheckBoxList>
                                                                        </div>

                                                                        <asp:Button ID="btnBack" runat="server" CssClass="btn btn-primary" OnClick="btnBack_Click"
                                                                            Text="Back" Visible="False" />
                                                                        <asp:Button ID="btnNext" runat="server" CssClass="btn btn-primary" OnClick="btnNext_Click"
                                                                            Text="Next" Visible="False" />
                                                                        <asp:Button ID="Button2" runat="server" CausesValidation="False" CssClass="btn btn-primary"
                                                                            ImageUrl="~/Images/buttonCancel.png" OnClick="Button2_Click" Text="View"
                                                                            Visible="False" />

                                                                        <div id="PanView" visible="false" runat="server">
                                                                            <div id="Td8" runat="server">
                                                                                <asp:Label ID="lbls" runat="server" Text="Days"></asp:Label>
                                                                                <asp:Label ID="Label8" runat="server" Text="Time Table"></asp:Label>

                                                                                <asp:DataList ID="dlView" runat="server" Width="100%">
                                                                                    <ItemTemplate>

                                                                                        <asp:Label ID="lblDays" runat="server" Text='<%#Eval("Days") %>' />

                                                                                        <asp:DataList ID="GvTime" runat="server" RepeatDirection="Horizontal">
                                                                                            <ItemTemplate>
                                                                                                <asp:Label ID="lblTimeId" runat="server" Text='<%#Eval("TimeTableId") %>' />
                                                                                            </ItemTemplate>
                                                                                        </asp:DataList>
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
                                                    <asp:Label ID="Label15" runat="server" Text="<%$ Resources:Attendance,Advance Search%>"></asp:Label></h3>
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
                                                    <asp:DropDownList ID="ddlbinFieldName" runat="server" CssClass="form-control">
                                                        <asp:ListItem Selected="True" Text="<%$ Resources:Attendance,Shift Name %>" Value="Shift_Name"></asp:ListItem>
                                                        <asp:ListItem Text="<%$ Resources:Attendance,Shift Name(Local) %>" Value="Shift_Name_L"></asp:ListItem>
                                                        <asp:ListItem Text="<%$ Resources:Attendance,Cycle Unit %>" Value="Cycle_Unit"></asp:ListItem>
                                                        <asp:ListItem Text="<%$ Resources:Attendance,Cycle No. %>" Value="Cycle_No"></asp:ListItem>
                                                        <asp:ListItem Text="<%$ Resources:Attendance,Shift Id %>" Value="Shift_Id"></asp:ListItem>
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
                                                <div class="col-lg-2">
                                                    <asp:Panel ID="Panel2" runat="server" DefaultButton="btnbinbind">
                                                        <asp:TextBox ID="txtbinValue" runat="server" CssClass="form-control"></asp:TextBox>
                                                    </asp:Panel>
                                                </div>
                                                <div class="col-lg-3">
                                                    <asp:LinkButton ID="btnbinbind" runat="server" CausesValidation="False"  OnClick="btnbinbind_Click" ToolTip="<%$ Resources:Attendance,Search %>"></asp:LinkButton>

                                                    <asp:LinkButton ID="btnbinRefresh"  runat="server" CausesValidation="False" OnClick="btnbinRefresh_Click" ToolTip="<%$ Resources:Attendance,Refresh %>"></asp:LinkButton>
                                                    <%--Visible="false"--%>
                                                    <asp:LinkButton ID="imgBtnRestore" Visible="true" CausesValidation="False"  runat="server" OnClick="imgBtnRestore_Click" ToolTip="<%$ Resources:Attendance, Active %>" >
                                                        <span class="far fa-lightbulb"  style="font-size:30px;margin-right:15px;"></span>
                                                        </asp:LinkButton>
                                                    <%--Visible="false"--%>
                                                    <asp:ImageButton ID="ImgbtnSelectAll"  Style="width: 33px; margin-top: -5px;" runat="server" OnClick="ImgbtnSelectAll_Click"
                                                        ToolTip="<%$ Resources:Attendance, Select All %>" ImageUrl="~/Images/selectAll.png" />
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>


                                <div class="box box-warning box-solid" <%= gvShiftBin.Rows.Count>0?"style='display:block'":"style='display:none'"%>>
                                    <div class="box-body">
                                        <div class="row">
                                            <div class="col-md-12">
                                                <div class="flow">
                                                    <asp:GridView CssClass="table-striped table-bordered table table-hover" ID="gvShiftBin" PageSize="<%# PageControlCommon.GetPageSize() %>" runat="server"
                                                        AutoGenerateColumns="False" DataKeyNames="Shift_Id" Width="100%" AllowPaging="True" OnPageIndexChanging="gvShiftBin_PageIndexChanging"
                                                        OnSorting="gvShiftBin_OnSorting" AllowSorting="true">
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
                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,Shift Id %>" SortExpression="Shift_Id">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblShiftId1" runat="server" Text='<%# Eval("Shift_Id") %>'></asp:Label>
                                                                </ItemTemplate>
                                                                <ItemStyle />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,Shift Name %>" SortExpression="Shift_Name">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lbShiftName" runat="server" Text='<%# Eval("Shift_Name") %>'></asp:Label>
                                                                    <asp:Label ID="lblShiftId" Visible="false" runat="server" Text='<%# Eval("Shift_Id") %>'></asp:Label>
                                                                </ItemTemplate>
                                                                <ItemStyle />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,Shift Name(Local) %>"
                                                                SortExpression="Shift_Name_L">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblShiftNameL" runat="server" Text='<%# Eval("Shift_Name_L") %>'></asp:Label>
                                                                </ItemTemplate>
                                                                <ItemStyle />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,Cycle No. %>" SortExpression="Cycle_No">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblCycleNo" runat="server" Text='<%# Eval("Cycle_No") %>'></asp:Label>
                                                                </ItemTemplate>
                                                                <ItemStyle />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,Cycle Unit %>" SortExpression="Cycle_Unit">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblCycleUnit" runat="server" Text='<%# Eval("Cycle_Unit") %>'></asp:Label>
                                                                </ItemTemplate>
                                                                <ItemStyle />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,Apply From %>" SortExpression="Apply_From">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblApplyDate" runat="server" Text='<%# GetDate(Eval("Apply_From")) %>'></asp:Label>
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
                    <div class="tab-pane" id="Upload">
                        <asp:UpdatePanel ID="Update_Upload" runat="server">
                            <ContentTemplate>
                                <div class="row">
                                    <div class="col-md-12">
                                        <div class="box box-primary">
                                            <div class="box-body">
                                                <div class="form-group">
                                                    <div class="col-md-12">
                                                        <asp:LinkButton ID="Lnk_Demo_Shift_Upload" OnClick="Lnk_Demo_Shift_Upload_Click" Font-Bold="true" runat="server" Text="<%$ Resources:Attendance,Sample Excel File For Shift Management Upload%>"></asp:LinkButton>
                                                        <br />
                                                        <br />
                                                    </div>
                                                    <div class="col-md-12">
                                                        <div class="col-md-6">
                                                            <asp:Label ID="Lbl_Browse_Excel" runat="server" Text="<%$ Resources:Attendance,Browse Excel File%>"></asp:Label>
                                                            <div class="input-group" style="width: 100%;">
                                                                <cc1:AsyncFileUpload ID="FU_Shift_Upload"
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
                                                            <asp:Label runat="server" Text="<%$ Resources:Attendance,Select Sheet%>" ID="Label12"></asp:Label>
                                                            <asp:DropDownList ID="DDl_Excel_Sheet" CssClass="form-control" runat="server">
                                                            </asp:DropDownList>
                                                            <br />
                                                        </div>
                                                        <div class="col-md-6">
                                                            <br />
                                                            <asp:Button ID="btnConnect" CssClass="btn btn-primary" runat="server" CausesValidation="False"
                                                                OnClick="btnConnect_Click" Visible="true" Text="<%$ Resources:Attendance,Get Record%>" />
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
                                                                                <asp:RadioButton ID="Rbt_All" Style="margin-left: 20px; margin-right: 20px;" runat="server" GroupName="upd" AutoPostBack="true" Checked="true" OnCheckedChanged="Rbt_All_CheckedChanged" Text="<%$ Resources:Attendance,All%>" />
                                                                                <asp:RadioButton ID="Rbt_Valid" Style="margin-left: 20px; margin-right: 20px;" runat="server" GroupName="upd" AutoPostBack="true" Text="<%$ Resources:Attendance,Valid%>" OnCheckedChanged="Rbt_All_CheckedChanged" />
                                                                                <asp:RadioButton ID="Rbt_Invalid" Style="margin-left: 20px; margin-right: 20px;" runat="server" GroupName="upd" AutoPostBack="true" Text="<%$ Resources:Attendance,Invalid%>" OnCheckedChanged="Rbt_All_CheckedChanged" />
                                                                            </div>
                                                                            <div class="col-md-12" style="text-align: right;">
                                                                                <asp:Label ID="lbltotaluploadRecord" runat="server"></asp:Label>
                                                                            </div>
                                                                            <div class="col-md-12" style="max-height: 500px; overflow: auto;">
                                                                                <asp:GridView CssClass="table-striped table-bordered table table-hover" ID="GV_Sheet_Upload" runat="server" Width="100%">
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
                        </asp:UpdatePanel>
                    </div>
                </div>
            </div>
        </div>
    </div>


    <div class="modal fade" id="Modal_Shift_View" role="dialog" aria-labelledby="Modal_Shift_ViewLabel" aria-hidden="true">
        <div class="modal-dialog">
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal">
                        <span aria-hidden="true">&times;</span><span class="sr-only">Close</span></button>
                    <h4 class="modal-title" id="Modal_Shift_ViewLabel">Cycle Of Weeks</h4>
                </div>
                <div class="modal-body">
                    <asp:UpdatePanel ID="Update_Modal_Shift_View" runat="server">
                        <ContentTemplate>
                            <div class="row">
                                <div class="col-md-12">
                                    <asp:Label ID="Label14" runat="server" Text="Cycle Of Weeks"></asp:Label>
                                    <asp:DropDownList ID="DDl_Cycle_Of_Week" CssClass="form-control" runat="server">
                                        <asp:ListItem Selected="True" Text="1" Value="1"></asp:ListItem>
                                        <asp:ListItem Text="2" Value="2"></asp:ListItem>
                                        <asp:ListItem Text="3" Value="3"></asp:ListItem>
                                        <asp:ListItem Text="4" Value="4"></asp:ListItem>
                                    </asp:DropDownList>
                                </div>
                            </div>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
                <div class="modal-footer">
                    <asp:UpdatePanel ID="Update_Modal_Shift_Button" runat="server">
                        <%--<Triggers>
                            <asp:PostBackTrigger ControlID="Btn_Download_Excel" />
                        </Triggers>--%>
                        <ContentTemplate>
                            <asp:Button ID="Btn_Download_Excel" runat="server" CssClass="btn btn-primary" OnClick="Btn_Download_Excel_Click" Text="<%$ Resources:Attendance,Download %>" />
                            <button type="button" class="btn btn-danger" data-dismiss="modal">Close</button>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
            </div>
        </div>
    </div>





    <asp:UpdateProgress ID="UpdateProgress5" runat="server" AssociatedUpdatePanelID="Update_Modal_Shift_Button">
        <ProgressTemplate>
            <div class="modal_Progress">
                <div class="center_Progress">
                    <img alt="" src="../Images/ajax-loader2.gif" style="vertical-align: middle; background-color: White; border-radius: 10px;" />
                </div>
            </div>
        </ProgressTemplate>
    </asp:UpdateProgress>

    <asp:UpdateProgress ID="UpdateProgress6" runat="server" AssociatedUpdatePanelID="Update_Modal_Shift_View">
        <ProgressTemplate>
            <div class="modal_Progress">
                <div class="center_Progress">
                    <img alt="" src="../Images/ajax-loader2.gif" style="vertical-align: middle; background-color: White; border-radius: 10px;" />
                </div>
            </div>
        </ProgressTemplate>
    </asp:UpdateProgress>


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

    <asp:UpdateProgress ID="UpdateProgress4" runat="server" AssociatedUpdatePanelID="Update_Upload">
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

        function Li_Tab_List() {
            document.getElementById('<%= Btn_List.ClientID %>').click();
        }
        function Li_Tab_Bin() {
            document.getElementById('<%= Btn_Bin.ClientID %>').click();
        }
        function Li_Tab_Upload() {
            document.getElementById('<%= Btn_Upload.ClientID %>').click();
        }
        function LI_Edit_Active() {
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

        function LI_List_Active_UPDATE() {
            $("#Li_List").addClass("active");
            $("#List").addClass("active");

            $("#Li_Upload").removeClass("active");
            $("#Upload").removeClass("active");
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

        function Modal_Shift_View_Show() {
            document.getElementById('<%= Btn_Modal_Shift_View.ClientID %>').click();
        }
    </script>
</asp:Content>
