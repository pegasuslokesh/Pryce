<%@ Page Title="" Language="C#" MasterPageFile="~/ERPMaster.master" AutoEventWireup="true" EnableEventValidation="false" CodeFile="ShiftAssignToEmp.aspx.cs" Inherits="Attendance_ShiftAssignToEmp" %>

<%@ Register Assembly="DayPilot" Namespace="DayPilot.Web.Ui" TagPrefix="DayPilot" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="Head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="HeaderContent" runat="Server">
    <h1>
        <i class="fas fa-calendar-check"></i>&nbsp;&nbsp;
        <asp:Label ID="lblHeader" runat="server" Text="<%$ Resources:Attendance,Shift/Leave Assign To Employee%>"></asp:Label>
    </h1>
    <ol class="breadcrumb">
        <li><a href="#"><i class="fa fa-dashboard"></i>
            <asp:Label ID="Label6" runat="server" Text="<%$ Resources:Attendance,Attendance Module%>"></asp:Label></a></li>
        <li><a href="#"><i class="fa fa-dashboard"></i>
            <asp:Label ID="Label10" runat="server" Text="<%$ Resources:Attendance,Attendance Module%>"></asp:Label></a></li>
        <li class="active">
            <asp:Label ID="Label5" runat="server" Text="<%$ Resources:Attendance,Shift Assign To Employee%>"></asp:Label></li>
    </ol>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="Server">
    <asp:UpdatePanel ID="Update_Button" runat="server">
        <ContentTemplate>
            <asp:Button ID="Btn_Report" Style="display: none;" runat="server" OnClick="Btn_Report_Click" Text="List" />
            <asp:Button ID="Btn_List" Style="display: none;" runat="server" OnClick="btnList_Click" Text="List" />
            <asp:Button ID="Btn_Upload" Style="display: none;" runat="server" OnClick="btnUpload_Click" Text="Bin" />
            <asp:Button ID="Btn_Modal_View" Style="display: none;" runat="server" data-toggle="modal" data-target="#Modal_View" Text="Modal View" />
            <asp:Button ID="Btn_Modal_Shift_View" Style="display: none;" runat="server" data-toggle="modal" data-target="#Modal_Shift_View" Text="Modal Shift View" />
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
                    <li id="Li_Report"><a href="#Report" onclick="Li_Tab_Report()" data-toggle="tab">
                        <i class="fa fa-list"></i>&nbsp;&nbsp;<asp:Label ID="Label18" runat="server" Text="<%$ Resources:Attendance,Report %>"></asp:Label></a></li>
                    <li id="Li_Upload"><a href="#Upload" data-toggle="tab">
                        <i class="fa fa-upload"></i>&nbsp;&nbsp;<asp:Label ID="Label9" runat="server" Text="<%$ Resources:Attendance,Upload %>"></asp:Label></a></li>
                    <li id="Li_New"><a href="#New" data-toggle="tab">
                        <asp:UpdatePanel ID="Update_Li" runat="server">
                            <ContentTemplate>
                                <i class="fa fa-file"></i>&nbsp;&nbsp;
                                <asp:Label ID="Lbl_Tab_New" runat="server" Text="<%$ Resources:Attendance,New%>"></asp:Label>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </a></li>
                    <li class="active" id="Li_List"><a href="#List" onclick="Li_Tab_List()" data-toggle="tab">
                        <i class="fa fa-list"></i>&nbsp;&nbsp;<asp:Label ID="Lbl_List" runat="server" Text="<%$ Resources:Attendance,List %>"></asp:Label></a></li>
                </ul>
                <div class="tab-content">
                    <div class="tab-pane active" id="List">
                        <asp:UpdatePanel ID="Update_List" runat="server">
                            <Triggers>
                                <asp:PostBackTrigger ControlID="DayPilotScheduler1" />
                            </Triggers>
                            <ContentTemplate>
                                <div class="row">
                                    <div class="col-md-12">
                                        <div class="box box-primary">
                                            <div class="box-body">
                                                <div class="form-group">
                                                    <div class="col-md-2">
                                                        <asp:ImageButton ID="btnbind" Style="margin-top: -5px;" runat="server" CausesValidation="False" Visible="false"
                                                            ImageUrl="~/Images/search.png" OnClick="btnbind_Click" ToolTip="<%$ Resources:Attendance,Search %>" />
                                                        <asp:Label ID="Label4" runat="server" Text="<%$ Resources:Attendance,From Date %>"></asp:Label>
                                                        <a style="color: Red">*</a>
                                                        <asp:TextBox ID="txtFDate" runat="server" CssClass="form-control"></asp:TextBox>
                                                        <cc1:CalendarExtender OnClientShown="showCalendar"
                                                            ID="CalendarExtender1" runat="server" Enabled="True"
                                                            TargetControlID="txtFDate">
                                                        </cc1:CalendarExtender>
                                                        <asp:RequiredFieldValidator EnableClientScript="true" Style="float: right;" runat="server" ID="RequiredFieldValidator3" ValidationGroup="Save"
                                                            Display="Dynamic" SetFocusOnError="true" ControlToValidate="txtFDate" ErrorMessage="<%$ Resources:Attendance,Enter From Date %>"></asp:RequiredFieldValidator>
                                                    </div>
                                                    <div class="col-md-2">
                                                        <asp:Label ID="Label2" runat="server" Text="<%$ Resources:Attendance,To Date %>"></asp:Label>
                                                        <a style="color: Red">*</a>
                                                        <asp:TextBox ID="txtTDate" runat="server" CssClass="form-control"></asp:TextBox>
                                                        <cc1:CalendarExtender OnClientShown="showCalendar"
                                                            ID="CalendarExtender2" runat="server" Enabled="True"
                                                            TargetControlID="txtTDate">
                                                        </cc1:CalendarExtender>
                                                        <asp:RequiredFieldValidator EnableClientScript="true" Style="float: right;" runat="server" ID="RequiredFieldValidator1" ValidationGroup="Save"
                                                            Display="Dynamic" SetFocusOnError="true" ControlToValidate="txtTDate" ErrorMessage="<%$ Resources:Attendance,Enter To Date %>"></asp:RequiredFieldValidator>
                                                    </div>
                                                    <div class="col-md-6">
                                                        <asp:Label ID="Label3" runat="server" Text="<%$ Resources:Attendance,Employee %>"></asp:Label>
                                                        <a style="color: Red">*</a>
                                                        <asp:TextBox ID="txtEmp" OnTextChanged="ddlEmp_TextChanged" AutoPostBack="true"
                                                            BackColor="#eeeeee" runat="server" CssClass="form-control" />
                                                        <cc1:AutoCompleteExtender ID="AutoCompleteExtender2" runat="server" DelimiterCharacters="" EnableCaching="true"
                                                            Enabled="True" ServiceMethod="GetCompletionList" ServicePath="" CompletionInterval="100"
                                                            MinimumPrefixLength="1" CompletionSetCount="1" TargetControlID="txtEmp" UseContextKey="True"
                                                            CompletionListCssClass="autoCompleteList" CompletionListItemCssClass="autoCompleteListItem" CompletionListHighlightedItemCssClass="autoCompleteSelectedListItem" OnClientShown="resetPosition">
                                                        </cc1:AutoCompleteExtender>
                                                        <asp:RequiredFieldValidator EnableClientScript="true" Style="float: right;" runat="server" ID="RequiredFieldValidator2" ValidationGroup="Save"
                                                            Display="Dynamic" SetFocusOnError="true" ControlToValidate="txtEmp" ErrorMessage="<%$ Resources:Attendance,Enter Employee Name %>"></asp:RequiredFieldValidator>
                                                    </div>
                                                    <div class="col-md-2">
                                                        <br />
                                                        <asp:Button ID="btngo" ValidationGroup="Save" runat="server" Text="<%$ Resources:Attendance,GO %>" CssClass="btn btn-primary" OnClick="btngo_Click" />
                                                        &nbsp;&nbsp;
                                                        <asp:Button ID="btnChart" ValidationGroup="Save" runat="server" Text="<%$ Resources:Attendance,Chart View%>"
                                                            OnClick="btnChart_Click" CssClass="btn btn-primary" />
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                                <div class="box box-warning box-solid" <%= GvShiftReport.Rows.Count>0?"style='display:block'":"style='display:none'"%>>
                                    <div class="box-body">
                                        <div class="row">
                                            <div class="col-md-12">
                                                <div class="flow">
                                                    <asp:GridView CssClass="table-striped table-bordered table table-hover" ID="GvShiftReport" runat="server" DataKeyNames="Att_Date,Emp_Id,OnDuty_Time,OffDuty_Time"
                                                        OnRowDataBound="GvShiftreport_RowDataBound" AutoGenerateColumns="False" Width="100%">
                                                        <Columns>
                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,Employee Code %>">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblEmpCode" runat="server" Text='<%#Eval("Emp_Code") %>' />
                                                                    <asp:Label ID="lblEmpID" Visible="false" runat="server" Text='<%# Eval("Emp_Id") %>' />
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:BoundField DataField="EmpName" HeaderText="<%$ Resources:Attendance,Name %>" />
                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,Date %>">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblDate" runat="server" Text='<%# GetDate(Eval("Att_Date")) %>'></asp:Label>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,Shift %>">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblshift0" runat="server" Text='<%# Eval("Shift_Name") %>'> </asp:Label>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,On Duty Time %>">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblOnDuty" runat="server" Text='<%# Eval("OnDuty_Time") %>'></asp:Label>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,Off Duty Time %>">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lbloffduty" runat="server" Text='<%# Eval("OffDuty_Time") %>'></asp:Label>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,IS Off %>">
                                                                <ItemTemplate>
                                                                    <asp:CheckBox ID="ChkIsOff" runat="server" Enabled="false" />
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,Is Temp %>">
                                                                <ItemTemplate>
                                                                    <asp:CheckBox ID="Is_Temp" runat="server" Enabled="false" />
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,Is Holiday %>">
                                                                <ItemTemplate>
                                                                    <asp:CheckBox ID="chkHoliday" runat="server" Enabled="false" />
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                        </Columns>
                                                        <PagerStyle CssClass="pagination-ys" />
                                                    </asp:GridView>
                                                    <DayPilot:DayPilotScheduler ID="DayPilotScheduler1" runat="server" Visible="false"
                                                        HeaderFontSize="8pt" HeaderHeight="25" DataStartField="start"
                                                        DataEndField="end" DataTextField="name" DataValueField="id" DataResourceField="resource" EventFontSize="11px"
                                                        CellDuration="60" CellWidth="40" Days="1" EventHeight="25"
                                                        Style="top: 0px; left: 0px">
                                                    </DayPilot:DayPilotScheduler>
                                                    <asp:DataList ID="dtlistshift" runat="server" Visible="false" RepeatColumns="7" RepeatDirection="Horizontal">
                                                        <ItemTemplate>
                                                            <div class="col-md-3" style="width: 150px;">
                                                                <div class="box box-widget widget-user-2" style="background-color: #f39c12;">
                                                                    <div class="widget-user-image">
                                                                        <asp:Label ID="lbldayname" runat="server" ForeColor="blue" Text='<%# Eval("DayName") %>'></asp:Label>&nbsp;
                                                                            <asp:Label ID="lblDayValue" runat="server" ForeColor="White" Text='<%# Eval("Day") %>'></asp:Label>
                                                                        <asp:Label ID="lblMonthName" runat="server" ForeColor="White" Text='<%# GetMonthName(Convert.ToDateTime(Eval("attDate").ToString())) %>'></asp:Label>
                                                                        <asp:Label ID="LBLyEAR" runat="server" ForeColor="White" Text='<%# Convert.ToDateTime(Eval("attDate")).Year.ToString() %>'></asp:Label>
                                                                    </div>
                                                                    <div style="text-align: center;">
                                                                        <asp:LinkButton ID="lnkshiftName" runat="server" ForeColor="Black" Font-Underline="true" Text='<%# Eval("shiftName") %>' CommandArgument='<%# Eval("Trans_Id") %>' CommandName='<%# Eval("attDate") %>' ToolTip='<%# Eval("shiftName_1") %>' OnCommand="lnkshiftName_Command"></asp:LinkButton></li>
                                                                    </div>
                                                                </div>
                                                            </div>
                                                        </ItemTemplate>
                                                    </asp:DataList>
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
                                <div id="Div_Before_Next" runat="server">
                                    <div class="row">
                                        <div class="col-md-12">
                                            <div class="box box-primary">
                                                <div class="box-body">
                                                    <div class="form-group">
                                                        <div class="col-md-12">
                                                            <div class="col-md-4">
                                                                <asp:HiddenField ID="editid" runat="server" />
                                                                <asp:RadioButton ID="rbtnGroup" OnCheckedChanged="EmpGroup_CheckedChanged"
                                                                    runat="server" Text="<%$ Resources:Attendance,Group %>" Font-Bold="true" GroupName="EmpGroup"
                                                                    AutoPostBack="true" />
                                                                <asp:RadioButton ID="rbtnEmp" Style="margin-left: 25px;" runat="server" AutoPostBack="true"
                                                                    Text="<%$ Resources:Attendance,Employee %>" GroupName="EmpGroup" Font-Bold="true"
                                                                    OnCheckedChanged="EmpGroup_CheckedChanged" />
                                                            </div>
                                                            <div class="col-md-2">
                                                                <asp:Label ID="Label7" runat="server" Text="<%$ Resources:Attendance,From date %>"></asp:Label>
                                                                <asp:RequiredFieldValidator EnableClientScript="true" Style="float: right;" runat="server"
                                                                    ID="RequiredFieldValidator4" ValidationGroup="GridEmp_Save" Display="Dynamic" SetFocusOnError="true" ForeColor="Red"
                                                                    ControlToValidate="txtFrom" ErrorMessage="*"></asp:RequiredFieldValidator>
                                                                <asp:TextBox ID="txtFrom" runat="server" placeholder="Enter From Date" CssClass="form-control"></asp:TextBox>
                                                                <cc1:CalendarExtender OnClientShown="showCalendar"
                                                                    ID="CalendarExtender_txtNewFromDate" runat="server" Enabled="True"
                                                                    TargetControlID="txtFrom">
                                                                </cc1:CalendarExtender>
                                                            </div>
                                                            <div class="col-md-2">
                                                                <asp:Label ID="Label8" runat="server" Text="<%$ Resources:Attendance,To date %>"></asp:Label>
                                                                <asp:RequiredFieldValidator EnableClientScript="true" Style="float: right;" runat="server"
                                                                    ID="RequiredFieldValidator5" ValidationGroup="GridEmp_Save" Display="Dynamic" SetFocusOnError="true" ForeColor="Red"
                                                                    ControlToValidate="txtTo" ErrorMessage="*"></asp:RequiredFieldValidator>
                                                                <asp:TextBox ID="txtTo" runat="server" placeholder="Enter To Date" CssClass="form-control"></asp:TextBox>
                                                                <cc1:CalendarExtender OnClientShown="showCalendar"
                                                                    ID="CalendarExtender_txtNewToDate" runat="server" Enabled="True"
                                                                    TargetControlID="txtTo">
                                                                </cc1:CalendarExtender>
                                                            </div>
                                                            <div class="col-md-2">
                                                                <br />
                                                                <asp:Button ID="btnGetShiftdate" ValidationGroup="Save_Date" runat="server" Text="<%$ Resources:Attendance,Get Date List%>" CssClass="btn btn-primary" OnClick="imggetshiftDate_Click" />
                                                                <br />
                                                            </div>
                                                            <div class="col-md-12">
                                                                <br />
                                                            </div>
                                                            <div class="col-md-6">
                                                                <asp:Label ID="lblLocation" runat="server" Text="<%$ Resources:Attendance,Location %>"></asp:Label>
                                                                <asp:DropDownList ID="ddlLocation" runat="server" CssClass="form-control" AutoPostBack="true" OnSelectedIndexChanged="dpLocation_SelectedIndexChanged">
                                                                </asp:DropDownList>
                                                            </div>
                                                            <div class="col-md-6">
                                                                <asp:Label ID="lblGroupByDept" runat="server" Text="<%$ Resources:Attendance,Department %>"></asp:Label>
                                                                <div class="input-group">
                                                                    <asp:DropDownList ID="dpDepartment" runat="server" CssClass="form-control" AutoPostBack="true" OnSelectedIndexChanged="dpDepartment_SelectedIndexChanged">
                                                                    </asp:DropDownList>
                                                                    <div class="input-group-btn">
                                                                        <asp:LinkButton ID="ImageButton1" runat="server" CausesValidation="False" OnClick="btnAllRefresh_Click" ToolTip="<%$ Resources:Attendance,Refresh %>"> <span class="fa fa-repeat"  style="font-size:25px;margin-left:5px"></span> </asp:LinkButton>
                                                                    </div>
                                                                </div>
                                                            </div>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <div id="Div_Employee" runat="server">
                                        <div class="row">
                                            <div class="col-md-12">
                                                <div id="Div1" runat="server" class="box box-info collapsed-box">
                                                    <div class="box-header with-border">
                                                        <h3 class="box-title">
                                                            <asp:Label ID="Label22" runat="server" Text="<%$ Resources:Attendance,Advance Search%>"></asp:Label></h3>
                                                        &nbsp;&nbsp;|&nbsp;&nbsp;
					 <asp:Label ID="lblSelectRecord" Font-Bold="true" Visible="false" runat="server" Text="<%$ Resources:Attendance,Total Records: 0 %>"></asp:Label>
                                                        <div class="box-tools pull-right">
                                                            <button type="button" class="btn btn-box-tool" data-widget="collapse">
                                                                <i id="I1" runat="server" class="fa fa-plus"></i>
                                                            </button>
                                                        </div>
                                                    </div>
                                                    <div class="box-body">
                                                        <div class="col-lg-2">
                                                            <asp:DropDownList ID="ddlSelectField" runat="server" CssClass="form-control">
                                                                <asp:ListItem Text="<%$ Resources:Attendance,Employee Code%>" Value="Emp_Code"></asp:ListItem>
                                                                <asp:ListItem Text="<%$ Resources:Attendance,Employee Name %>" Value="Emp_Name" Selected="True"></asp:ListItem>
                                                                <asp:ListItem Text="<%$ Resources:Attendance,Employee Name Local %>" Value="Emp_Name_L"></asp:ListItem>
                                                            </asp:DropDownList>
                                                        </div>
                                                        <div class="col-lg-2">
                                                            <asp:DropDownList ID="ddlSelectOption" runat="server" CssClass="form-control">
                                                                <asp:ListItem Text="<%$ Resources:Attendance,--Select--%>" Value="--Select one--"></asp:ListItem>
                                                                <asp:ListItem Text="<%$ Resources:Attendance,Equal %>" Value="Equal"></asp:ListItem>
                                                                <asp:ListItem Text="<%$ Resources:Attendance,Contains %>" Selected="True" Value="Contains"></asp:ListItem>
                                                                <asp:ListItem Text="<%$ Resources:Attendance,Like%>" Value="Like"></asp:ListItem>
                                                            </asp:DropDownList>
                                                        </div>
                                                        <div class="col-lg-5">
                                                            <asp:TextBox ID="txtval" placeholder="Search from Content" runat="server" CssClass="form-control"></asp:TextBox>
                                                        </div>
                                                        <div class="col-lg-3">
                                                            <asp:LinkButton ID="btnEmp" runat="server" CausesValidation="False" OnClick="btnEmp_Click"><span class="fa fa-search"  style="font-size:25px;"></span></asp:LinkButton>&nbsp;&nbsp;&nbsp;
                                                            <asp:LinkButton ID="btnrefresh2" runat="server" CausesValidation="False" OnClick="btnRefresh2Report_Click"><span class="fa fa-repeat"  style="font-size:25px;"></span></asp:LinkButton>&nbsp;&nbsp;&nbsp;
                                                            <asp:LinkButton ID="btnAddList" runat="server" ValidationGroup="GridEmp_Save" OnClick="btnAddList_Click" ToolTip="Add Employee"><i class="fa fa-plus-square" style="font-size:25px;padding:5px;margin-top: -5px;"></i></asp:LinkButton>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="box box-warning box-solid">
                                            <div class="box-body">
                                                <div class="col-md-12" style="max-height: 200px; overflow: auto;">
                                                    <asp:CustomValidator ID="CustomValidator1" ValidationGroup="GridEmp_Save" runat="server" ErrorMessage="Please select at least one record."
                                                        ClientValidationFunction="Grid_Validate" ForeColor="Red"></asp:CustomValidator>
                                                    <asp:HiddenField ID="hdnFldSelectedValues" runat="server" />
                                                    <asp:GridView CssClass="table-striped table-bordered table table-hover" ID="GvEmpList" runat="server" AutoGenerateColumns="False" DataKeyNames="Emp_Code" Width="100%">
                                                        <Columns>
                                                            <asp:TemplateField HeaderText="Select">
                                                                <ItemTemplate>
                                                                    <asp:CheckBox ID="chkBxSelect" onclick="checkItem_All(this,0)" runat="server" />
                                                                    <asp:HiddenField ID="hdnFldId" runat="server" Value='<%# Eval("Emp_Id") %>' />
                                                                </ItemTemplate>
                                                                <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                                                <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="5%" />
                                                                <HeaderTemplate>
                                                                    <asp:CheckBox ID="chkBxHeader" onclick="checkAll(this,0);" runat="server" />
                                                                </HeaderTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,Employee Code %>">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblEmpCode" runat="server" Text='<%# Eval("Emp_Code") %>'></asp:Label>
                                                                    <asp:Label ID="lblEmpId" Visible="false" runat="server" Text='<%# Eval("Emp_Id") %>'></asp:Label>
                                                                </ItemTemplate>
                                                                <ItemStyle Width="10%" />
                                                            </asp:TemplateField>
                                                            <asp:BoundField DataField="Emp_Name" HeaderText="<%$ Resources:Attendance,Employee Name %>"
                                                                SortExpression="Emp_Name" ItemStyle-Width="85%" />
                                                        </Columns>
                                                        <PagerStyle CssClass="pagination-ys" />
                                                    </asp:GridView>
                                                    <br />
                                                </div>
                                                <div class="col-md-12" style="text-align: center;">
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <div id="Div_Group" runat="server" visible="false">
                                        <div class="row">
                                            <div class="col-md-12">
                                                <div class="box box-primary">
                                                    <div class="box-body">
                                                        <div class="form-group">
                                                            <div class="col-md-3">
                                                                <asp:ListBox ID="lbxGroup" Width="100%" runat="server" Height="200px" SelectionMode="Multiple"
                                                                    AutoPostBack="true" OnSelectedIndexChanged="lbxGroup_SelectedIndexChanged" CssClass="list"
                                                                    Font-Names="Trebuchet MS" Font-Size="Small" ForeColor="Gray"></asp:ListBox>
                                                                <asp:Label ID="lblEmp" runat="server" Visible="false"></asp:Label>
                                                            </div>
                                                            <div class="col-md-9">
                                                                <div style="overflow: auto">
                                                                    <asp:GridView CssClass="table-striped table-bordered table table-hover" ID="gvEmployee" runat="server" AllowPaging="True" AutoGenerateColumns="False"
                                                                        OnPageIndexChanging="gvEmp1_PageIndexChanging" Width="100%" PageSize="<%# PageControlCommon.GetPageSize() %>">
                                                                        <Columns>
                                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,Employee Code %>">
                                                                                <ItemTemplate>
                                                                                    <asp:Label ID="lblEmpCode" runat="server" Text='<%# Eval("Emp_Code") %>'></asp:Label>
                                                                                    <asp:Label ID="lblEmpId" Visible="false" runat="server" Text='<%# Eval("Emp_Id") %>'></asp:Label>
                                                                                </ItemTemplate>
                                                                                <ItemStyle />
                                                                            </asp:TemplateField>
                                                                            <asp:BoundField DataField="Emp_Name" HeaderText="<%$ Resources:Attendance,Employee Name %>"
                                                                                SortExpression="Emp_Name" ItemStyle-Width="40%" />
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
                                    </div>
                                    <div class="row" id="Div_Emp_List" runat="server" visible="false">
                                        <div class="col-md-12">
                                            <div class="box box-primary">
                                                <div class="box-body">
                                                    <div class="form-group">
                                                        <div class="col-md-12">
                                                            <div style="overflow: auto; max-height: 500px">
                                                            </div>
                                                            <br />
                                                        </div>
                                                        <div class="col-md-12">
                                                            <asp:Panel ID="paneldaterange" runat="server" Style="height: 500px;" ScrollBars="Auto">
                                                                <asp:CheckBox ID="chkselectAllshiftdate" runat="server" Text="Select All Date" AutoPostBack="false" OnCheckedChanged="chkselectAll_OnCheckedChanged" OnClick="CheckAll_List()" />
                                                                <asp:CustomValidator ID="CustomValidator2" ValidationGroup="Grid_Save" runat="server" ErrorMessage="Please select at least one date."
                                                                    ClientValidationFunction="chkshiftRange_Validate" ForeColor="Red"></asp:CustomValidator>
                                                                <asp:CheckBoxList ID="chkshiftdateRange" CellPadding="3" CellSpacing="3" RepeatColumns="3" RepeatDirection="Horizontal" runat="server" CssClass="form-control"></asp:CheckBoxList>
                                                                <%-- </div>--%>
                                                            </asp:Panel>
                                                        </div>
                                                        <div class="col-md-12">
                                                            <a style="color: Red">*</a>
                                                            <div class="input-group">
                                                                <div class="input-group-btn">
                                                                    <br />
                                                                    <asp:Button ID="btnNext" runat="server" Visible="false" ValidationGroup="Grid_Save" CssClass="btn btn-primary"
                                                                        ImageUrl="~/Images/buttonCancel.png" OnClick="btnNext1_Click"
                                                                        Text="<%$ Resources:Attendance,Next %>" />
                                                                    <br />
                                                                </div>
                                                            </div>
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
                                                    <div class="col-md-12" style="max-height: 200px; overflow: auto;">
                                                        <asp:UpdatePanel ID="upd" runat="server">
                                                            <ContentTemplate>
                                                                <asp:GridView CssClass="table-striped table-bordered table table-hover" ID="GvEmpListSelected" Enabled="true" runat="server" AutoGenerateColumns="true" DataKeyNames="Code"
                                                                    Width="100%" ClientIDMode="Static">
                                                                    <PagerStyle CssClass="pagination-ys" />
                                                                    <Columns>
                                                                        <asp:TemplateField>
                                                                            <ItemTemplate>
                                                                                <asp:CheckBox ID="chkitemselect" onclick="checkItem_All1(this)" runat="server" />
                                                                            </ItemTemplate>
                                                                            <HeaderTemplate>
                                                                                <asp:CheckBox ID="chkHeaderitemselect" runat="server" onclick="checkItem_All1_Header(this)" />
                                                                            </HeaderTemplate>
                                                                        </asp:TemplateField>
                                                                    </Columns>
                                                                </asp:GridView>
                                                            </ContentTemplate>
                                                        </asp:UpdatePanel>
                                                        <br />
                                                    </div>
                                                    <div class="form-group" id="div_Save" runat="server" visible="false">
                                                        <div class="col-md-6">
                                                            <asp:RadioButton ID="rbtnshiftName" runat="server" Text="<%$ Resources:Attendance,Shift Name %>" GroupName="shift" Checked="true" AutoPostBack="true" OnCheckedChanged="rbtnshiftName_CheckedChanged" />
                                                            <asp:RadioButton ID="rbtnweekOff" runat="server" Text="<%$ Resources:Attendance,Week Off %>" GroupName="shift" AutoPostBack="true" OnCheckedChanged="rbtnshiftName_CheckedChanged" />
                                                            <asp:TextBox ID="txtShiftName" runat="server" CssClass="form-control" BackColor="#eeeeee"
                                                                AutoPostBack="true" OnTextChanged="txtShiftName_textChanged" Placeholder="Enter Shift Name" />
                                                            <cc1:AutoCompleteExtender ID="AutoCompleteExtender4" runat="server" DelimiterCharacters="" EnableCaching="true"
                                                                Enabled="True" ServiceMethod="GetCompletionListShiftName" ServicePath="" CompletionInterval="100"
                                                                MinimumPrefixLength="1" CompletionSetCount="1" TargetControlID="txtShiftName" UseContextKey="True"
                                                                CompletionListCssClass="autoCompleteList" CompletionListItemCssClass="autoCompleteListItem" CompletionListHighlightedItemCssClass="autoCompleteSelectedListItem" OnClientShown="resetPosition">
                                                            </cc1:AutoCompleteExtender>
                                                            <asp:RequiredFieldValidator EnableClientScript="true" Style="float: right;" runat="server"
                                                                ID="RequiredFieldValidator_txtShiftName" ValidationGroup="Grid_Save" Display="Dynamic" SetFocusOnError="true"
                                                                ControlToValidate="txtShiftName" ErrorMessage="Enter Shift Name"></asp:RequiredFieldValidator>
                                                        </div>
                                                        <div class="col-md-6">
                                                            <asp:Label ID="Label15" runat="server" Text="<%$ Resources:Attendance,Reference No %>"></asp:Label>
                                                            <asp:TextBox ID="txtReferenceNo" runat="server" CssClass="form-control" ReadOnly="true" />
                                                            <br />
                                                        </div>
                                                        <div class="col-md-12" style="text-align: center;">
                                                            <asp:Button ID="btnsave" runat="server" Text="<%$ Resources:Attendance,Save %>" OnClick="btnsave_Click"
                                                                CssClass="btn btn-success" Visible="true" ValidationGroup="Grid_Save" /><asp:HiddenField
                                                                    ID="HiddenField1" runat="server" />
                                                            <asp:Button ID="btncancel1" runat="server" Text="<%$ Resources:Attendance,Cancel %>"
                                                                OnClick="btnCancel1_Click" CssClass="btn btn-danger" Visible="true" /><asp:HiddenField ID="HiddenField2" runat="server" />
                                                            <asp:Button ID="btndeleteshift" Visible="false" runat="server" CssClass="btn btn-primary" OnClick="btnDeleteShift_Click"
                                                                Text="<%$ Resources:Attendance,Delete %>" />
                                                            <cc1:ConfirmButtonExtender ID="confirm1" runat="server" ConfirmText="Are you sure to delete record ?"
                                                                TargetControlID="btndeleteshift">
                                                            </cc1:ConfirmButtonExtender>
                                                            <br />
                                                        </div>
                                                    </div>
                                                    <div class="col-md-12" style="max-height: 250px; overflow: auto;">
                                                        <br />
                                                        <asp:GridView CssClass="table-striped table-bordered table table-hover" ID="gvAssgnList" runat="server" AutoGenerateColumns="False" Enabled="true" Width="100%">
                                                            <Columns>
                                                                <asp:TemplateField HeaderText="<%$ Resources:Attendance,Employee Code %>">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblEmpID" runat="server" Text='<%# Eval("Emp_Code") %>'></asp:Label>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:BoundField DataField="EmpName" HeaderText="<%$ Resources:Attendance,Employee Name %>" />
                                                                <asp:TemplateField HeaderText="<%$ Resources:Attendance,Date %>">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblDate" runat="server" Text='<%# GetDate(Eval("Att_Date")) %>'></asp:Label>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="<%$ Resources:Attendance,Shift %>">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblshift0" runat="server" Text='<%# Eval("Shift_Name") %>'> </asp:Label>
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
                                <div id="Div_After_Next" runat="server" visible="false">
                                    <div id="After_Next" runat="server">
                                        <div class="alert alert-info ">
                                            <div class="row">
                                                <div class="form-group">
                                                    <div class="col-lg-2">
                                                        <h5>
                                                            <asp:Label ID="Label1" Font-Bold="true" runat="server"></asp:Label></h5>
                                                    </div>
                                                    <div class="col-lg-3">
                                                        <asp:DropDownList ID="ddlSelectField0" runat="server" CssClass="form-control">
                                                            <asp:ListItem Text="<%$ Resources:Attendance,Employee Code%>" Value="Emp_Code"></asp:ListItem>
                                                            <asp:ListItem Text="<%$ Resources:Attendance,Employee Name %>" Value="Emp_Name" Selected="True"></asp:ListItem>
                                                            <asp:ListItem Text="<%$ Resources:Attendance,Employee Name Local %>" Value="Emp_Name_L"></asp:ListItem>
                                                        </asp:DropDownList>
                                                    </div>
                                                    <div class="col-lg-2">
                                                        <asp:DropDownList ID="ddlSelectOption0" CssClass="form-control" runat="server">
                                                            <asp:ListItem Text="<%$ Resources:Attendance,--Select--%>"></asp:ListItem>
                                                            <asp:ListItem Text="<%$ Resources:Attendance,Equal %>" Value="Equal"></asp:ListItem>
                                                            <asp:ListItem Text="<%$ Resources:Attendance,Contains %>" Selected="True" Value="Contains"></asp:ListItem>
                                                            <asp:ListItem Text="<%$ Resources:Attendance,Like%>" Value="Like"></asp:ListItem>
                                                        </asp:DropDownList>
                                                    </div>
                                                    <div class="col-lg-3">
                                                        <asp:TextBox ID="txtval0" CssClass="form-control" runat="server"></asp:TextBox>
                                                    </div>
                                                    <div class="col-lg-2">
                                                        <asp:ImageButton ID="btnEmp0" Style="margin-top: -5px" runat="server" CausesValidation="False"
                                                            ImageUrl="~/Images/search.png" OnClick="btnEmp0_Click" />
                                                        <asp:ImageButton ID="btnRefresh3" Style="width: 33px;" runat="server" CausesValidation="False"
                                                            ImageUrl="~/Images/refresh.png" OnClick="btnRefresh3Report_Click"></asp:ImageButton>
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
                                                        <div class="col-md-4">
                                                            <asp:Label ID="lblDateFrom" runat="server" Font-Names="Verdana" Font-Size="12px"
                                                                Text="<%$ Resources:Attendance,From Date %>"></asp:Label>
                                                        </div>
                                                        <div class="col-md-4">
                                                            <asp:Label ID="lblTo" runat="server" Text="<%$ Resources:Attendance,To Date %>" Font-Names="Verdana"
                                                                Font-Size="12px"></asp:Label>
                                                            <asp:Button ID="btnIsTemprary" Visible="false" Style="display: none;" runat="server" CssClass="btn btn-primary"
                                                                OnClick="btnIsTemprary_Click" Text="<%$ Resources:Attendance,Temporary Shift %>" />
                                                        </div>
                                                        <div class="col-md-4">
                                                            <%-- <asp:Label ID="lblShift1" runat="server" Text="<%$ Resources:Attendance,Shift %>"
                                                                Font-Names="Verdana" Font-Size="12px"></asp:Label>
                                                            <asp:TextBox ID="txtShiftName" runat="server" CssClass="form-control" BackColor="#eeeeee"
                                                                AutoPostBack="true" OnTextChanged="txtShiftName_textChanged" />
                                                            <cc1:AutoCompleteExtender ID="AutoCompleteExtender5" runat="server" DelimiterCharacters="" EnableCaching="true"
                                                                Enabled="True" ServiceMethod="GetCompletionListShiftName" ServicePath="" CompletionInterval="100"
                                                                MinimumPrefixLength="1" CompletionSetCount="1" TargetControlID="txtShiftName" UseContextKey="True"
                                                                CompletionListCssClass="autoCompleteList" CompletionListItemCssClass="autoCompleteListItem" CompletionListHighlightedItemCssClass="autoCompleteSelectedListItem" OnClientShown="resetPosition">
                                                            </cc1:AutoCompleteExtender>--%>
                                                        </div>
                                                        <div class="col-md-12" style="text-align: center">
                                                            <br />
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <div id="Div_Grid" runat="server" visible="false">
                                        <div class="box box-warning box-solid">
                                            <div class="box-header with-border">
                                                <h3 class="box-title"></h3>
                                            </div>
                                            <div style="max-height: 500px; overflow: auto;" class="box-body">
                                                <div class="row">
                                                    <div class="col-md-12">
                                                        <%--<div>--%>
                                                        <%--</div>--%>
                                                    </div>
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
                            <Triggers>
                                <asp:PostBackTrigger ControlID="fileLoad" />
                                <asp:PostBackTrigger ControlID="btndownloadInvalid" />
                            </Triggers>
                            <ContentTemplate>
                                <div class="row">
                                    <div class="col-md-12">
                                        <div class="box box-info">
                                            <div class="box-header with-border">
                                                <h3 class="box-title">
                                                    <asp:Label ID="Label17" runat="server" Text="<%$ Resources:Attendance,Upload%>"></asp:Label></h3>
                                                <div class="box-tools pull-right">
                                                    <button type="button" class="btn btn-box-tool" data-widget="collapse">
                                                        <i class="fa fa-minus"></i>
                                                    </button>
                                                </div>
                                            </div>
                                            <div class="box-body">
                                                <div class="form-group">
                                                    <div class="col-md-4">
                                                        <asp:LinkButton ID="Lnk_Shift_Assignment_Yearly" Font-Bold="true" runat="server" Text="<%$ Resources:Attendance,Sample Excel File For Leave Upload%>" OnClick="Lnk_Shift_Assignment_Yearly_OnClick"></asp:LinkButton>
                                                    </div>
                                                    <div class="col-md-4">
                                                        <asp:LinkButton ID="lbkuploadMonthly" Font-Bold="true" runat="server" Text="<%$ Resources:Attendance,Sample Excel File For Shift Assignment Monthly%>" OnClick="lbkuploadMonthly_OnClick"></asp:LinkButton>
                                                    </div>
                                                    <div class="col-md-4">
                                                        <asp:LinkButton ID="lbkuploadWeekly" Font-Bold="true" runat="server" Text="<%$ Resources:Attendance,Sample Excel File For Shift Assignment Weekly%>" OnClick="lbkuploadMonthly_OnClick"></asp:LinkButton>
                                                        <br />
                                                    </div>
                                                    <div class="col-md-12">
                                                        <br />
                                                        <div class="col-md-6">
                                                            <asp:Label runat="server" Text="<%$ Resources:Attendance,Browse Excel File%>" ID="Label66"></asp:Label>
                                                            <div class="input-group" style="width: 100%;">
                                                                <cc1:AsyncFileUpload ID="fileLoad" OnUploadedComplete="FileUploadComplete" OnClientUploadError="uploadError" OnClientUploadStarted="uploadStarted" OnClientUploadComplete="uploadComplete"
                                                                    runat="server" CssClass="form-control" CompleteBackColor="White" UploaderStyle="Traditional" UploadingBackColor="#CCFFFF" ThrobberID="imgLoader" Width="100%" />
                                                                <div class="input-group-btn" style="border: solid 1px #d2d6de; width: 35px;">
                                                                    <asp:Image ID="Img_Right" runat="server" Width="30px" Height="30px" ImageUrl="../Images/Allow.png" Style="display: none" />
                                                                    <asp:Image ID="Img_Wrong" runat="server" Width="30px" Height="30px" ImageUrl="../Images/Delete1.png" Style="display: none" />
                                                                    <asp:Image ID="imgLoader" runat="server" ImageUrl="../Images/loader.gif" />
                                                                </div>
                                                            </div>
                                                            <br />
                                                            <asp:Button ID="btnGetSheet" CssClass="btn btn-primary" runat="server" CausesValidation="False"
                                                                OnClick="btnGetSheet_Click" Visible="true" Text="<%$ Resources:Attendance,Connect To DataBase %>" />
                                                        </div>
                                                        <div class="col-md-6" style="text-align: center;">
                                                            <asp:Label runat="server" Text="<%$ Resources:Attendance,Select Sheet%>" ID="Label13"></asp:Label>
                                                            <asp:DropDownList ID="ddlTables" CssClass="form-control" runat="server">
                                                            </asp:DropDownList>
                                                            <br />
                                                            <asp:RadioButtonList ID="rOperationType" runat="server" RepeatDirection="Horizontal">
      <asp:ListItem Value="1">Employee Schdule With Shift</asp:ListItem>
      <asp:ListItem Value="2">Employee Schdule With TimeTable</asp:ListItem>
      <asp:ListItem Value="3">Timetable Upload</asp:ListItem>
      
</asp:RadioButtonList>
                                                            <br />
                                                            <asp:Button ID="btnConnect" CssClass="btn btn-primary" runat="server" CausesValidation="False"
                                                                OnClick="btnConnect_Click" Visible="true" Text="<%$ Resources:Attendance,Get Record%>" />
                                                        </div>
                                                        <div runat="server" id="div_show_upload" style="text-align: center; display: none;">
                                                            <div class="col-md-6">
                                                                <asp:Label ID="Label67" runat="server" Text="Select Sheet"></asp:Label>
                                                                <br />
                                                            </div>
                                                            <div class="col-md-6" style="text-align: center;">
                                                                <asp:Button ID="btnviewcolumns" CssClass="btn btn-primary" runat="server" CausesValidation="False"
                                                                    OnClick="btnviewcolumns_Click" Visible="true" Text="<%$ Resources:Attendance,Map Column %>" />
                                                                <br />
                                                            </div>
                                                            <div class="col-md-12">
                                                                <div id="div_Grid_1" visible="false" runat="server" style="height: 500px; overflow: auto;">
                                                                    <asp:GridView CssClass="table-striped table-bordered table table-hover" ID="gvFieldMapping" runat="server" AutoGenerateColumns="False"
                                                                        DataKeyNames="Nec" OnRowDataBound="gvFieldMapping_RowDataBound">
                                                                        <Columns>
                                                                            <asp:TemplateField>
                                                                                <ItemTemplate>
                                                                                    <asp:Label ID="lblCompulsery" runat="server"></asp:Label>
                                                                                </ItemTemplate>
                                                                            </asp:TemplateField>
                                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,Column %>">
                                                                                <ItemTemplate>
                                                                                    <asp:Label ID="lblColName" runat="server" Text='<%# Eval("Column_Name") %>'></asp:Label>
                                                                                </ItemTemplate>
                                                                            </asp:TemplateField>
                                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,Type %>">
                                                                                <ItemTemplate>
                                                                                    <asp:DropDownList ID="ddlExcelCol" runat="server">
                                                                                    </asp:DropDownList>
                                                                                </ItemTemplate>
                                                                            </asp:TemplateField>
                                                                        </Columns>
                                                                        <PagerStyle CssClass="pagination-ys" />
                                                                    </asp:GridView>
                                                                </div>
                                                                <br />
                                                            </div>
                                                            <div class="col-md-12" style="text-align: center">
                                                                <asp:Label ID="lblMessage" runat="server" ForeColor="Red"></asp:Label>
                                                                <asp:Button ID="btnUploadTemp" CssClass="btn btn-primary" runat="server"
                                                                    Text="<%$ Resources:Attendance,Show Data %>" />
                                                                <asp:Button ID="Button1" CssClass="btn btn-primary" runat="server" OnClick="btncancel_Click"
                                                                    Text="<%$ Resources:Attendance,Reset %>" />
                                                            </div>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                            <div id="Div_showdata" runat="server" visible="false" class="box box-warning box-solid">
                                                <div class="box-header with-border">
                                                    <h3 class="box-title"></h3>
                                                </div>
                                                <div class="box-body">
                                                    <div class="row">
                                                        <div class="col-md-12" style="text-align: center">
                                                            <asp:RadioButton ID="rbtnupdall" Style="margin-left: 20px; margin-right: 20px;" runat="server" GroupName="upd" AutoPostBack="true" Checked="true" OnCheckedChanged="rbtnupdall_OnCheckedChanged" Text="<%$ Resources:Attendance,All%>" />
                                                            <asp:RadioButton ID="rbtnupdValid" Style="margin-left: 20px; margin-right: 20px;" runat="server" GroupName="upd" AutoPostBack="true" Text="<%$ Resources:Attendance,Valid%>" OnCheckedChanged="rbtnupdall_OnCheckedChanged" />
                                                            <asp:RadioButton ID="rbtnupdInValid" Style="margin-left: 20px; margin-right: 20px;" runat="server" GroupName="upd" AutoPostBack="true" Text="<%$ Resources:Attendance,Invalid%>" OnCheckedChanged="rbtnupdall_OnCheckedChanged" />
                                                        </div>
                                                        <div class="col-md-12" style="text-align: right;">
                                                            <asp:Label ID="lbltotaluploadRecord" runat="server"></asp:Label>
                                                        </div>
                                                        <div class="col-md-12">
                                                            <div style="overflow: auto; max-height: 300px;">
                                                                <asp:GridView CssClass="table-striped table-bordered table table-hover" ID="gvSelected" runat="server" Width="100%">
                                                                    <PagerStyle CssClass="pagination-ys" />
                                                                </asp:GridView>
                                                            </div>
                                                            <br />
                                                        </div>
                                                        <div class="col-md-6">
                                                            <asp:Label ID="Label16" runat="server" Text="<%$ Resources:Attendance,Reference No %>"></asp:Label>
                                                            <asp:TextBox ID="txtuploadReferenceNo" runat="server" CssClass="form-control" ReadOnly="true" />
                                                            <br />
                                                        </div>
                                                        <div class="col-md-6" style="text-align: center">
                                                            <br />
                                                            <asp:Button ID="Button21" runat="server" CssClass="btn btn-primary" OnClick="btnUpload_Click1"
                                                                Text="<%$ Resources:Attendance,Upload Data %>" />
                                                            <asp:Button ID="btnBackToMapData" CssClass="btn btn-primary" runat="server" Text="<%$ Resources:Attendance,Back To FileUpload %>"
                                                                OnClick="btnBackToMapData_Click" />
                                                            <asp:Button ID="btndownloadInvalid" CssClass="btn btn-primary" runat="server" Text="<%$ Resources:Attendance,Download Invalid Record%>" CausesValidation="False"
                                                                OnClick="btndownloadInvalid_Click" />
                                                            <cc1:ConfirmButtonExtender ID="ConfirmButtonExtender1" runat="server" TargetControlID="Button21"
                                                                ConfirmText="<%$ Resources:Attendance,Are you sure to Save Records in Database%>">
                                                            </cc1:ConfirmButtonExtender>

                                                            


                                                            <asp:DropDownList ID="ddlFiltercol" Visible="false" CssClass="form-control"
                                                                runat="server">
                                                            </asp:DropDownList>
                                                            <asp:TextBox ID="txtfiltercol" Visible="false" CssClass="form-control" runat="server"></asp:TextBox>
                                                            &nbsp;&nbsp;
                                                        <asp:Button ID="btnFilter" CssClass="btn btn-primary" Visible="false" runat="server"
                                                            OnClick="btnFilter_Click" Text="<%$ Resources:Attendance,Filter %>" />
                                                            &nbsp;&nbsp;
                                                        <asp:Button ID="btnresetgv" CssClass="btn btn-primary" Visible="false" runat="server"
                                                            OnClick="btnresetgv_Click" Text="<%$ Resources:Attendance,Reset %>" />
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
                    <div class="tab-pane" id="Report">
                        <asp:UpdatePanel ID="Updpanel_Report" runat="server">
                            <Triggers>
                                <asp:PostBackTrigger ControlID="btnDownload" />
                            </Triggers>
                            <ContentTemplate>
                                <div class="row">
                                    <div class="col-md-12">
                                        <div class="box box-primary">
                                            <div class="box-body">
                                                <div class="form-group">
                                                    <div class="col-md-12">
                                                        <div class="col-md-6">
                                                            <asp:Label ID="Label19" runat="server" class="control-label" Text="<%$ Resources:Attendance,Status %>"></asp:Label>
                                                            <asp:DropDownList ID="ddlshiftstatus" runat="server" CssClass="form-control" OnSelectedIndexChanged="ddlshiftstatus_SelectedIndexChanged" AutoPostBack="true">
                                                                <asp:ListItem Selected="True" Text="Pending" Value="Pending"></asp:ListItem>
                                                                <asp:ListItem Text="Approved" Value="Approved"></asp:ListItem>
                                                                <asp:ListItem Text="Rejected" Value="Rejected"></asp:ListItem>
                                                            </asp:DropDownList>
                                                        </div>
                                                        <div class="col-md-6">
                                                            <asp:Label ID="lblLocationreport" runat="server" class="control-label" Text="<%$ Resources:Attendance,Location %>"></asp:Label>
                                                            <asp:DropDownList ID="ddlLocationReport" runat="server" CssClass="form-control" OnSelectedIndexChanged="ddlLocationReport_SelectedIndexChanged" AutoPostBack="true">
                                                            </asp:DropDownList>
                                                            <br />
                                                        </div>
                                                        <div class="col-md-6">
                                                            <asp:Label ID="Label68" runat="server" class="control-label" Text="<%$ Resources:Attendance,Reference No %>"></asp:Label>
                                                            <%--  <asp:DropDownList ID="DDLReferenceReport" runat="server" CssClass="form-control">
                                                            </asp:DropDownList>--%>
                                                            <asp:TextBox ID="txtReferenceReport" runat="server" CssClass="form-control"></asp:TextBox>
                                                            <%-- <cc1:AutoCompleteExtender ID="AutoCompleteExtender3" runat="server" DelimiterCharacters="" EnableCaching="true"
                                                                Enabled="True" ServiceMethod="GetCompletionListRefName" ServicePath="" CompletionInterval="100"
                                                                MinimumPrefixLength="1" CompletionSetCount="1" TargetControlID="txtReferenceReport" UseContextKey="True"
                                                                CompletionListCssClass="autoCompleteList" CompletionListItemCssClass="autoCompleteListItem" CompletionListHighlightedItemCssClass="autoCompleteSelectedListItem" OnClientShown="resetPosition">
                                                            </cc1:AutoCompleteExtender>--%>
                                                        </div>
                                                        <div class="col-md-6">
                                                            <br />
                                                            <asp:Button ID="btnGet" runat="server" Text="<%$ Resources:Attendance,Get%>" CssClass="btn btn-primary" OnClick="btnGet_Click" />
                                                            <asp:Button ID="btnRefRefresh" runat="server" Text="<%$ Resources:Attendance,Reset%>" CssClass="btn btn-primary" OnClick="btnRefRefresh_Click" />
                                                            <asp:Button ID="btnDownload" runat="server" Text="<%$ Resources:Attendance,Export%>" CssClass="btn btn-primary" OnClick="btnDownload_Click" />
                                                        </div>
                                                    </div>
                                                    <div class="col-md-12" style="max-height: 400px; overflow: auto;">
                                                        <asp:Panel runat="server" ID="pnlExport">
                                                            <br />
                                                            <div class="flow">
                                                                <div runat="server" id="div_shiftinfo"></div>
                                                                <asp:Literal ID="litshhift" runat="server"></asp:Literal>
                                                                <asp:GridView CssClass="table-striped table-bordered table table-hover" ID="gvShiftView" AllowPaging="false" Width="100%" ClientIDMode="Static" runat="server" AutoGenerateColumns="true" DataKeyNames="Code">
                                                                    <Columns>
                                                                    </Columns>
                                                                </asp:GridView>
                                                            </div>
                                                            <br />
                                                            <div class="flow">
                                                                <asp:GridView CssClass="table-striped table-bordered table table-hover" ID="gvshiftsummary" AllowPaging="false" Width="100%" ClientIDMode="Static" runat="server" AutoGenerateColumns="true">
                                                                    <Columns>
                                                                    </Columns>
                                                                </asp:GridView>
                                                            </div>
                                                        </asp:Panel>
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
    <div style="display: none">
        <div style="display: none;" id="Tr1" runat="server">
            <asp:Button ID="btnOk0" runat="server" CausesValidation="False" CssClass="btn btn-success"
                ImageUrl="~/Images/buttonSave.png" OnClick="btnsaveTempShift_Click"
                Text="<%$ Resources:Attendance,Save %>" Visible="true" />
            <asp:Button ID="btnClearAll" runat="server" CausesValidation="False" CssClass="btn btn-primary"
                ImageUrl="~/Images/buttonSave.png" OnClick="btnClearAll_Click"
                Text="Clear All" />
            <asp:Button ID="btnCancelPanel" runat="server" CausesValidation="False" CssClass="btn btn-danger"
                ImageUrl="~/Images/buttonCancel.png" OnClick="btnCancelPanel_Click1"
                Text="<%$ Resources:Attendance,Cancel %>" />
        </div>
        <div id="Tr2" runat="server">
            <asp:Label ID="lblSelectShift" runat="server" Text="<%$ Resources:Attendance,selectshiftcat %>"
                Font-Bold="true"></asp:Label>
            <div id="PnlTimeTableList" runat="server">
                <asp:CheckBoxList ID="chkTimeTableList" runat="server" AutoPostBack="True" OnSelectedIndexChanged="chkTimeTableList_SelectedIndexChanged"
                    RepeatColumns="1" Font-Names="Verdana" Font-Size="10pt">
                </asp:CheckBoxList>
            </div>
            <asp:Label ID="lblselectdate" runat="server" Font-Bold="true"
                Text="<%$ Resources:Attendance,selectdate %>"></asp:Label>
            <div id="pnlAddDays" runat="server">
                <asp:CheckBoxList ID="chkDayUnderPeriod" runat="server" RepeatColumns="1"
                    Font-Names="Verdana" Font-Size="10pt">
                </asp:CheckBoxList>
            </div>
        </div>
    </div>
    <div class="modal fade" id="Modal_View" tabindex="-1" role="dialog" aria-labelledby="Modal_ViewLabel"
        aria-hidden="true">
        <div class="modal-dialog modal-lg">
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal">
                        <span aria-hidden="true">&times;</span><span class="sr-only">Close</span></button>
                    <h4 class="modal-title" id="Modal_ViewLabel">Shift View</h4>
                </div>
                <div class="modal-body">
                    <asp:UpdatePanel ID="Update_Modal_View" runat="server">
                        <ContentTemplate>
                            <div class="row">
                                <div class="col-md-12">
                                    <div class="box box-primary">
                                        <div class="box-body">
                                            <div class="form-group">
                                                <div class="col-md-12">
                                                    <asp:LinkButton ID="lnkBackToList" runat="server" CausesValidation="False" OnClick="lnkBackToList_Click"
                                                        Text="<%$ Resources:Attendance,Back To List %>"></asp:LinkButton>
                                                    <br />
                                                    <asp:Label ID="lblschheader" Font-Bold="true" runat="server" Text="<%$ Resources:Attendance,Schedule For %>"></asp:Label>
                                                    &nbsp:&nbsp<asp:Label ID="lblempname" Font-Bold="true" runat="server"></asp:Label>
                                                </div>
                                                <div class="col-md-6">
                                                    <asp:Label ID="lblFromDate" runat="server" Text="<%$ Resources:Attendance,From Date  %>"></asp:Label>
                                                    <asp:TextBox ID="txtFromDate" runat="server" CssClass="form-control"></asp:TextBox>
                                                    <cc1:CalendarExtender OnClientShown="showCalendar"
                                                        ID="txtFromDate_CalendarExtender" runat="server" Enabled="True" TargetControlID="txtFromDate">
                                                    </cc1:CalendarExtender>
                                                </div>
                                                <div class="col-md-6">
                                                    <asp:Label ID="lblToDate" runat="server" Text="<%$ Resources:Attendance,To Date  %>"></asp:Label>
                                                    <asp:TextBox ID="txtToDate" runat="server" CssClass="form-control"></asp:TextBox>
                                                    <cc1:CalendarExtender OnClientShown="showCalendar"
                                                        ID="txtToDate_CalendarExtender" runat="server" Enabled="True" TargetControlID="txtToDate">
                                                    </cc1:CalendarExtender>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
                <div class="modal-footer">
                    <asp:UpdatePanel ID="Update_Modal_View_Button" runat="server">
                        <ContentTemplate>
                            <asp:Button ID="btnsubmit" runat="server" Text="<%$ Resources:Attendance,Go %>"
                                OnClick="btnsubmit_Click" CssClass="btn btn-primary" CausesValidation="False" />
                            <button type="button" class="btn btn-danger" data-dismiss="modal">
                                Close</button>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
            </div>
        </div>
    </div>
    <div class="modal fade" id="Modal_Shift_View" role="dialog" aria-labelledby="Modal_Shift_ViewLabel"
        aria-hidden="true">
        <div class="modal-dialog modal-lg">
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal">
                        <span aria-hidden="true">&times;</span><span class="sr-only">Close</span></button>
                    <h4 class="modal-title" id="Modal_Shift_ViewLabel">Filter Date</h4>
                </div>
                <div class="modal-body">
                    <asp:UpdatePanel ID="Update_Modal_Shift_View" runat="server">
                        <ContentTemplate>
                            <div class="row">
                                <div class="col-md-12">
                                    <div class="box box-primary">
                                        <asp:Label ID="Label14" runat="server" Text="<%$ Resources:Attendance,Department%>"></asp:Label>
                                        <br />
                                        <asp:CheckBox ID="chkSelectAll" runat="server" CssClass="labelComman" onClick="new_validation();" Text="<%$ Resources:Attendance,Select All %>" />
                                        <div class="box-body">
                                            <div class="form-group">
                                                <div class="col-md-12" style="overflow: auto; max-height: 300px">
                                                    <asp:TreeView ID="TreeViewDepartment" runat="server"></asp:TreeView>
                                                    <br />
                                                </div>
                                                <div class="col-md-6">
                                                    <asp:ImageButton ID="ImageButton2" Style="margin-top: -5px;" runat="server" CausesValidation="False" Visible="false"
                                                        ImageUrl="~/Images/search.png" OnClick="btnbind_Click" ToolTip="<%$ Resources:Attendance,Search %>" />
                                                    <asp:Label ID="Label11" runat="server" Text="<%$ Resources:Attendance,From Date %>"></asp:Label>
                                                    <a style="color: Red">*</a>
                                                    <asp:RequiredFieldValidator EnableClientScript="true" Style="float: right;" runat="server" ID="RequiredFieldValidator7" ValidationGroup="upload_save"
                                                        Display="Dynamic" SetFocusOnError="true" ControlToValidate="txtFDate_upload" ErrorMessage="<%$ Resources:Attendance,Enter From Date %>"></asp:RequiredFieldValidator>
                                                    <asp:TextBox ID="txtFDate_upload" runat="server" CssClass="form-control"></asp:TextBox>
                                                    <cc1:CalendarExtender OnClientShown="showCalendar"
                                                        ID="CalendarExtender3" runat="server" Enabled="True"
                                                        TargetControlID="txtFDate_upload">
                                                    </cc1:CalendarExtender>
                                                </div>
                                                <div class="col-md-6">
                                                    <asp:Label ID="Label12" runat="server" Text="<%$ Resources:Attendance,To Date %>"></asp:Label>
                                                    <a style="color: Red">*</a>
                                                    <asp:RequiredFieldValidator EnableClientScript="true" Style="float: right;" runat="server" ID="RequiredFieldValidator8" ValidationGroup="upload_save"
                                                        Display="Dynamic" SetFocusOnError="true" ControlToValidate="txtTDate_upload" ErrorMessage="<%$ Resources:Attendance,Enter To Date %>"></asp:RequiredFieldValidator>
                                                    <asp:TextBox ID="txtTDate_upload" runat="server" CssClass="form-control"></asp:TextBox>
                                                    <cc1:CalendarExtender OnClientShown="showCalendar"
                                                        ID="CalendarExtender4" runat="server" Enabled="True"
                                                        TargetControlID="txtTDate_upload">
                                                    </cc1:CalendarExtender>
                                                </div>
                                                <div style="display: none;" class="col-md-12">
                                                    <asp:HiddenField ID="hdnShiftId" runat="server" />
                                                    <asp:Label ID="lblShiftName" runat="server" Text="<%$ Resources:Attendance,Shift Name %>" />
                                                    <asp:TextBox ID="txtEditShiftName" data-modalfocus="" runat="server" CssClass="form-control" BackColor="#eeeeee"
                                                        AutoPostBack="true" OnTextChanged="txtShiftName_textChanged" />
                                                    <cc1:AutoCompleteExtender ID="AutoCompleteExtender1" runat="server" DelimiterCharacters="" EnableCaching="true"
                                                        Enabled="True" ServiceMethod="GetCompletionListShiftName" ServicePath="" CompletionInterval="100"
                                                        MinimumPrefixLength="1" CompletionSetCount="1" TargetControlID="txtEditShiftName" UseContextKey="True"
                                                        CompletionListCssClass="autoCompleteList" CompletionListItemCssClass="autoCompleteListItem" CompletionListHighlightedItemCssClass="autoCompleteSelectedListItem" OnClientShown="resetPosition">
                                                    </cc1:AutoCompleteExtender>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
                <div class="modal-footer">
                    <asp:UpdatePanel ID="Update_Modal_Shift_Button" runat="server">
                        <Triggers>
                            <asp:PostBackTrigger ControlID="btnSaveShfitEmployee" />
                        </Triggers>
                        <ContentTemplate>
                            <asp:HiddenField ID="Hdn_File_Download" runat="server" />
                            <asp:Button ID="btnSaveShfitEmployee" runat="server" ValidationGroup="upload_save" CssClass="btn btn-primary" OnClick="btnSaveShfitEmployee_Click"
                                Text="<%$ Resources:Attendance,Download %>" />
                            <button type="button" class="btn btn-danger" data-dismiss="modal">
                                Close</button>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
            </div>
        </div>
    </div>
    <asp:UpdateProgress ID="UpdateProgress3" runat="server" AssociatedUpdatePanelID="Update_Modal_View">
        <ProgressTemplate>
            <div class="modal_Progress">
                <div class="center_Progress">
                    <img alt="" src="../Images/ajax-loader2.gif" style="vertical-align: middle; background-color: White; border-radius: 10px;" />
                </div>
            </div>
        </ProgressTemplate>
    </asp:UpdateProgress>
    <asp:UpdateProgress ID="UpdateProgress8" runat="server" AssociatedUpdatePanelID="Updpanel_Report">
        <ProgressTemplate>
            <div class="modal_Progress">
                <div class="center_Progress">
                    <img alt="" src="../Images/ajax-loader2.gif" style="vertical-align: middle; background-color: White; border-radius: 10px;" />
                </div>
            </div>
        </ProgressTemplate>
    </asp:UpdateProgress>
    <asp:UpdateProgress ID="UpdateProgress4" runat="server" AssociatedUpdatePanelID="Update_Modal_View_Button">
        <ProgressTemplate>
            <div class="modal_Progress">
                <div class="center_Progress">
                    <img alt="" src="../Images/ajax-loader2.gif" style="vertical-align: middle; background-color: White; border-radius: 10px;" />
                </div>
            </div>
        </ProgressTemplate>
    </asp:UpdateProgress>
    <asp:UpdateProgress ID="UpdateProgress5" runat="server" AssociatedUpdatePanelID="Update_Modal_Shift_View">
        <ProgressTemplate>
            <div class="modal_Progress">
                <div class="center_Progress">
                    <img alt="" src="../Images/ajax-loader2.gif" style="vertical-align: middle; background-color: White; border-radius: 10px;" />
                </div>
            </div>
        </ProgressTemplate>
    </asp:UpdateProgress>
    <asp:UpdateProgress ID="UpdateProgress6" runat="server" AssociatedUpdatePanelID="Update_Modal_Shift_Button">
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
    <asp:UpdateProgress ID="UpdateProgress7" runat="server" AssociatedUpdatePanelID="Update_Upload">
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
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="AfterLoadScriptContent" runat="Server">
    <script src="../Script/common.js"></script>
    <script type="text/javascript">
        $('#Modal_Shift_View').on('shown.bs.modal', function () {
            $("[data-modalfocus]", this).focus();
        })
        function Li_Tab_List() {
            document.getElementById('<%= Btn_List.ClientID %>').click();
        }
        function Li_Tab_Report() {
            document.getElementById('<%= Btn_Report.ClientID %>').click();
        }
        function Modal_View_Show() {
            document.getElementById('<%= Btn_Modal_View.ClientID %>').click();
            document.getElementById('<%=txtEditShiftName.ClientID%>').focus();
        }
        function Modal_Shift_View_Show() {
            document.getElementById('<%= Btn_Modal_Shift_View.ClientID %>').click();
            document.getElementById('<%=txtEditShiftName.ClientID%>').focus();
        }
        function LI_Edit_Active() {
            $("#Li_List").removeClass("active");
            $("#List").removeClass("active");
            $("#Li_New").addClass("active");
            $("#New").addClass("active");
        }
    </script>
    <script>
        function checkAll(GvEmpList, colIndex) {
            var GridView = GvEmpList.parentNode.parentNode.parentNode;
            var text = [];
            for (var i = 1; i < GridView.rows.length; i++) {
                var chb = GridView.rows[i].cells[colIndex].getElementsByTagName("input")[0];
                chb.checked = GvEmpList.checked;
                document.getElementById('<%=hdnFldSelectedValues.ClientID %>').value = "True";
            }
        }
        function CheckAll_List() {
            var intIndex = 0;
            var rowCount = document.getElementById('ctl00_MainContent_chkshiftdateRange').getElementsByTagName("input").length;
            for (i = 0; i < rowCount; i++) {
                if (document.getElementById('ctl00_MainContent_chkselectAllshiftdate').checked == true) {
                    //if (document.getElementById("ctl00_MainContent_chkshiftdateRange" + "_" + i)) {
                    document.getElementById("ctl00_MainContent_chkshiftdateRange" + "_" + i).checked = true;
                    //}
                }
                else {
                    //if (document.getElementById("ctl00_MainContent_chkshiftdateRange" + "_" + i)) {
                    document.getElementById("ctl00_MainContent_chkshiftdateRange" + "_" + i).checked = false;
                    //}
                }
            }
        }
        function checkMonthValidation() {
            var a = document.getElementById('<%= txtFDate_upload.ClientID %>').value;
            var b = document.getElementById('<%= txtTDate_upload.ClientID %>').value;
            var second = 1000, minute = second * 60, hour = minute * 60, day = hour * 24, week = day * 7;
            var dtCustOrder = new Date(a);
            var dtDelvDt = new Date(b);
            var diffDays = dtDelvDt - dtCustOrder;
            //alert(Math.floor(diffDays / day));
            if (Math.floor(diffDays / day) > 31) {
                alert("day difference should be equal or less then 31 days");
                return false;
            }
                //if (dtCustOrder.getMonth() != dtDelvDt.getMonth() || dtCustOrder.getFullYear() != dtDelvDt.getFullYear()) {
                //    alert("Date range allow for single month only");
                //    return false;
                //}
            else {
                document.getElementById('<%= Btn_Modal_Shift_View.ClientID %>').click();
                return true;
            }
        }
        function checkItem_All1(objRef) {
            // $('#GvEmpListSelected tbody tr td input[type="checkbox"]').each(function () {
            if (objRef.checked) {
                $(objRef).parents('tr').find(':checkbox').prop('checked', true);
            }
            else {
                // $(this).closest('tr').prop('checked', false);
                $(objRef).parents('tr').find(':checkbox').prop('checked', false);
            }
            // });
        }
        function checkItem_All1_Header(objRef) {
            $('#GvEmpListSelected tbody tr td input[type="checkbox"]').each(function () {
                if (objRef.checked) {
                    $(this).parents('tr').find(':checkbox').prop('checked', true);
                }
                else {
                    // $(this).closest('tr').prop('checked', false);
                    $(this).parents('tr').find(':checkbox').prop('checked', false);
                }
            });
        }
        function checkItem_All(objRef, colIndex) {
            document.getElementById('<%=hdnFldSelectedValues.ClientID %>').value = "True";
            var GridView = objRef.parentNode.parentNode.parentNode;
            var selectAll = GridView.rows[0].cells[colIndex].getElementsByTagName("input")[0];
            if (!objRef.checked) {
                selectAll.checked = false;
            }
            else {
                var checked = true;
                for (var i = 1; i < GridView.rows.length; i++) {
                    var chb = GridView.rows[i].cells[colIndex].getElementsByTagName("input")[0];
                    if (!chb.checked) {
                        checked = false;
                        break;
                    }
                }
                selectAll.checked = checked;
            }
        }
        function Grid_Validate(sender, args) {
            if (document.getElementById("<%=rbtnEmp.ClientID %>").checked) {
                var gridView = document.getElementById("<%=GvEmpList.ClientID %>");
                var checkBoxes = gridView.getElementsByTagName("input");
                for (var i = 0; i < checkBoxes.length; i++) {
                    if (checkBoxes[i].type == "checkbox" && checkBoxes[i].checked) {
                        args.IsValid = true;
                        return;
                    }
                }
                args.IsValid = false;
            }
        }
        function chkshiftRange_Validate(sender, args) {
            var rowCount = document.getElementById('ctl00_MainContent_chkshiftdateRange').getElementsByTagName("input").length;
            for (i = 0; i < rowCount; i++) {
                if (document.getElementById("ctl00_MainContent_chkshiftdateRange" + "_" + i).checked == true) {
                    args.IsValid = true;
                    return;
                }
            }
            args.IsValid = false;
        }
        function Modal_Shift_View_Close() {
            document.getElementById('<%= Btn_Modal_Shift_View.ClientID %>').click();
        }
        function toggleCheckboxes(ctrl, chk) {
            $('#<%=GvEmpListSelected.ClientID %> :checkbox[id$=' + chk + ']').attr('checked', ctrl.checked);
        }
    </script>
    <script type="text/javascript">
        function uploadComplete(sender, args) {
            document.getElementById('<%= Img_Wrong.ClientID %>').style.display = "none";
            document.getElementById('<%= Img_Right.ClientID %>').style.display = "";
        }
        function uploadError(sender, args) {
            document.getElementById('<%= Img_Right.ClientID %>').style.display = "none";
            document.getElementById('<%= Img_Wrong.ClientID %>').style.display = "";
            alert('Invalid File Type, Select Only .xls, .xlsx, .mdb, .accdb extension file');
        }
        function uploadStarted(sender, args) {
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
        function checkall(data) {
            debugger;
            var table = data.offsetParent.offsetParent.children[0].children;
            $(table).each(function () {
                debugger;
                var gg = $(this)[0].cells[0];
                if (data.checked) {
                    $(this)[0].cells[0].children[0].checked = true;
                }
                else {
                    // $(this).closest('tr').prop('checked', false);
                    $(this)[0].cells[0].children[0].checked = false;
                }
            });
        }
    </script>
</asp:Content>
