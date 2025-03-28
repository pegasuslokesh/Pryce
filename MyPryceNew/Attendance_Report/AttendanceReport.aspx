<%@ Page Title="" Language="C#" MasterPageFile="~/ERPMaster.master" AutoEventWireup="true" CodeFile="AttendanceReport.aspx.cs" Inherits="Attendance_Report_AttendanceReport" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="HeaderContent" runat="Server">
    <h1>
        <img src="../Images/attendance_report_setup.png" alt="" />
        <asp:Label ID="lblHeader" runat="server" Text="<%$ Resources:Attendance,Attendance Report Setup%>"></asp:Label>
    </h1>
    <ol class="breadcrumb">
        <li><a href="#"><i class="fa fa-dashboard"></i>
            <asp:Label ID="Label6" runat="server" Text="<%$ Resources:Attendance,Attendance Module%>"></asp:Label></a></li>
        <li><a href="#"><i class="fa fa-dashboard"></i>
            <asp:Label ID="Label10" runat="server" Text="<%$ Resources:Attendance,Attendance Report%>"></asp:Label></a></li>
        <li class="active">
            <asp:Label ID="Label5" runat="server" Text="<%$ Resources:Attendance,Attendance Report Setup%>"></asp:Label></li>
    </ol>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="Server">
    <asp:UpdatePanel ID="Update_Button" runat="server">
        <ContentTemplate>
            <asp:Button ID="Btn_Modal_Popup" Style="display: none;" data-toggle="modal" data-target="#myModal" runat="server" Text="Modal Popup" />
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

    <asp:UpdatePanel ID="Update_Form" runat="server">
        <ContentTemplate>
            <div id="Div_Main" runat="server">
                <div class="form-control">
                    <div class="col-md-12">
                        <div style="text-align: center;" class="col-md-6">
                            <asp:RadioButton ID="rbtnGroupSal" OnCheckedChanged="EmpGroupSal_CheckedChanged" runat="server"
                                Text="<%$ Resources:Attendance,Group %>" GroupName="EmpGroup" AutoPostBack="true" />
                            <asp:RadioButton ID="rbtnEmpSal" Style="margin-left: 25px;" runat="server" AutoPostBack="true"
                                Text="<%$ Resources:Attendance,Employee %>" GroupName="EmpGroup" OnCheckedChanged="EmpGroupSal_CheckedChanged" />


                            <asp:Label ID="lblEmp" runat="server"></asp:Label>
                        </div>
                        <div style="text-align: center;" class="col-md-6" id="Div_Terminated_Report" runat="server" visible="false">
                            <asp:CheckBox ID="chkterminatedemployee" runat="server" Text="<%$ Resources:Attendance,Show terminated employee%>" AutoPostBack="true" OnCheckedChanged="chkterminatedemployee_CheckedChanged" />
                        </div>
                    </div>




                </div>
                <div id="Div_Group" visible="false" runat="server" class="row">
                    <div class="col-md-12">
                        <div class="box box-warning">
                            <div class="box-header with-border">
                                <h3 class="box-title">Group</h3>
                                <div class="box-tools pull-right">
                                    <button type="button" class="btn btn-box-tool" data-widget="collapse">
                                        <i class="fa fa-minus"></i>
                                    </button>
                                </div>
                            </div>
                            <div class="box-body">
                                <div class="col-md-4">
                                    <asp:CustomValidator ID="CustomValidator1" ValidationGroup="Save" runat="server" ErrorMessage="Please select at least one record."
                                        ClientValidationFunction="Grid_Validate" ForeColor="Red"></asp:CustomValidator>

                                    <asp:ListBox ID="lbxGroupSal" runat="server" Style="width: 100%" Height="211px" SelectionMode="Multiple"
                                        AutoPostBack="true" OnSelectedIndexChanged="lbxGroupSal_SelectedIndexChanged" CssClass="form-control"></asp:ListBox>
                                </div>
                                <div class="col-md-8">
                                    <br />
                                    <div style="overflow: auto;">
                                        <asp:Label ID="Label52" runat="server" Visible="false"></asp:Label>
                                        <asp:GridView ID="gvEmployeeSal" runat="server" AllowPaging="True" AutoGenerateColumns="False"
                                            CssClass="grid" OnPageIndexChanging="gvEmployeeSal_PageIndexChanging" PageSize="<%# PageControlCommon.GetPageSize() %>"
                                            Width="100%">
                                            <Columns>
                                                <asp:TemplateField HeaderText="<%$ Resources:Attendance,Employee Code %>">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblEmpCode" runat="server" Text='<%# Eval("Emp_Code") %>'></asp:Label>
                                                        <asp:Label ID="lblEmpId" runat="server" Text='<%# Eval("Emp_Id") %>' Visible="false"></asp:Label>
                                                    </ItemTemplate>
                                                    <ItemStyle CssClass="grid" />
                                                </asp:TemplateField>
                                                <asp:BoundField DataField="Emp_Name" HeaderText="<%$ Resources:Attendance,Employee Name %>"
                                                    ItemStyle-CssClass="grid" ItemStyle-Width="20%" SortExpression="Emp_Name" />
                                                <asp:BoundField DataField="Emp_Name_L" HeaderText="<%$ Resources:Attendance,Employee Name Local %>"
                                                    ItemStyle-CssClass="grid" ItemStyle-Width="20%" SortExpression="Emp_Name_L" />
                                                <asp:TemplateField HeaderText="<%$ Resources:Attendance,Email Id %>">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblProductCode" runat="server" Text='<%# Eval("Email_Id") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <ItemStyle CssClass="grid" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="<%$ Resources:Attendance,Designation Name %>">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblItemType" runat="server" Text='<%# Eval("Designation") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <ItemStyle CssClass="grid" />
                                                </asp:TemplateField>
                                            </Columns>
                                            <AlternatingRowStyle CssClass="InvgridAltRow" />
                                            <HeaderStyle BorderColor="#6E6E6E" BorderStyle="Solid" BorderWidth="1px" CssClass="Invgridheader" />
                                            <PagerStyle CssClass="pagination-ys" />
                                            <RowStyle CssClass="Invgridrow" HorizontalAlign="Center" />
                                        </asp:GridView>

                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
                <div id="Div_Employee" runat="server" class="row">
                    <div class="col-md-12">
                        <div class="box box-warning">
                            <div class="box-header with-border">
                                <h3 class="box-title">
                                    <asp:Label ID="lbledit" runat="server" Text="<%$ Resources:Attendance,Employee %>"></asp:Label></h3>
                                <div class="box-tools pull-right">
                                    <button type="button" class="btn btn-box-tool" data-widget="collapse">
                                        <i class="fa fa-minus"></i>
                                    </button>
                                </div>
                            </div>
                            <div class="box-body">
                                <div class="col-md-6">
                                    <asp:Label ID="lblLocation" runat="server" Text="<%$ Resources:Attendance,Location %>"></asp:Label>
                                    <asp:DropDownList ID="ddlLocation" runat="server" CssClass="form-control" AutoPostBack="true"
                                        OnSelectedIndexChanged="dpLocation_SelectedIndexChanged">
                                    </asp:DropDownList>
                                    <br />
                                </div>
                                <div class="col-md-6">
                                    <br />
                                    <asp:Label ID="lblGroupByDept" Visible="false" runat="server" Text="<%$ Resources:Attendance,Department %>"></asp:Label>
                                    <div class="input-group">
                                        <asp:DropDownList ID="dpDepartment" Visible="false" runat="server" CssClass="form-control" AutoPostBack="true"
                                            OnSelectedIndexChanged="dpDepartment_SelectedIndexChanged">
                                        </asp:DropDownList>
                                        <span class="input-group-btn">



                                            <%--   <asp:ImageButton ID="ImageButton1" Style="width: 33px; margin: 10px; margin-top: 2px;"
                                                runat="server" CausesValidation="False" ImageUrl="~/Images/refresh.png"
                                                OnClick="btnAllRefresh_Click" ToolTip="<%$ Resources:Attendance,Refresh %>"></asp:ImageButton>--%>

                                            <asp:Button ID="btnfilterdepartment" Style="" runat="server" CssClass="btn btn-info" OnClick="btnfilterdepartment_Click" Text="<%$ Resources:Attendance,Filter Department%>" />
                                        </span>
                                    </div>
                                </div>
                                <div class="col-md-12">
                                    <div class="alert alert-info ">
                                        <div class="row">
                                            <div class="form-group">
                                                <div class="col-lg-3">
                                                    <asp:DropDownList ID="ddlField" runat="server" CssClass="form-control">
                                                        <asp:ListItem Selected="True"  Text="<%$ Resources:Attendance,Employee Code %>" Value="Emp_Code"></asp:ListItem>
                                                        <asp:ListItem Text="<%$ Resources:Attendance,Employee Name %>" Value="Emp_Name"></asp:ListItem>
                                                        <asp:ListItem Text="<%$ Resources:Attendance,Employee Name Local%>" Value="Emp_Name_L"></asp:ListItem>
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
                                                <div class="col-lg-3">
                                                    <asp:Panel ID="Panel1" runat="server" DefaultButton="ImageButton8">
                                                        <asp:TextBox ID="txtValue" runat="server" class="form-control"></asp:TextBox>
                                                    </asp:Panel>

                                                </div>
                                                <div class="col-lg-2" style="text-align: center;">
                                                    <asp:ImageButton ID="ImageButton8" Style="width: 35px; margin-top: -1px;"
                                                        runat="server" CausesValidation="False" ImageUrl="~/Images/search.png" OnClick="btnarybind_Click1"
                                                        ToolTip="<%$ Resources:Attendance,Search %>"></asp:ImageButton>
                                                    <asp:ImageButton ID="ImageButton9" runat="server" Style="width: 30px;" CausesValidation="False"
                                                        ImageUrl="~/Images/refresh.png" OnClick="btnaryRefresh_Click1" ToolTip="<%$ Resources:Attendance,Refresh %>"></asp:ImageButton>
                                                    <asp:ImageButton ID="ImageButton10" Style="width: 30px;" runat="server" OnClick="ImgbtnSelectAll_Clickary"
                                                        ToolTip="<%$ Resources:Attendance, Select All %>" AutoPostBack="true" ImageUrl="~/Images/selectAll.png" />
                                                </div>
                                                <div class="col-lg-2">
                                                    <h5 class="text-center">
                                                        <asp:Label ID="lblTotalRecord" Font-Bold="true" runat="server" Text="<%$ Resources:Attendance,Total Records: 0 %>"></asp:Label></h5>
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
                                                    <div style="overflow: auto;">
                                                        <asp:HiddenField ID="HDFSort" runat="server" />
                                                        <asp:CustomValidator ID="CustomValidator6" ValidationGroup="Save" runat="server" ErrorMessage="Please select at least one record."
                                                            ClientValidationFunction="Grid_Validate" ForeColor="Red"></asp:CustomValidator>
                                                        <asp:Label ID="lblSelectRecord" runat="server" Visible="false"></asp:Label>
                                                        <asp:GridView ID="gvEmployee" runat="server" AllowPaging="True" AutoGenerateColumns="False" OnSorting="gvEmployee_OnSorting"
                                                            CssClass="grid" DataKeyNames="Emp_Id" OnPageIndexChanging="gvEmp_PageIndexChanging" AllowSorting="true"
                                                            PageSize="<%# PageControlCommon.GetPageSize() %>" Visible="true" Width="100%">
                                                            <Columns>
                                                                <%--   <asp:TemplateField HeaderText="Select">
                                                                        <ItemTemplate>
                                                                            <asp:CheckBox ID="chkBxSelect" runat="server" />
                                                                            <asp:HiddenField ID="hdnFldId" runat="server" Value='<%# Eval("Emp_Id") %>' />
                                                                        </ItemTemplate>
                                                                        <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="50px" />
                                                                        <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="50px" />
                                                                        <HeaderTemplate>
                                                                            <asp:CheckBox ID="chkBxHeader" runat="server" 
                                                                                onclick="javascript:HeaderClick(this);" />
                                                                        </HeaderTemplate>
                                                                    </asp:TemplateField>--%>
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
                                                                <%--<asp:TemplateField HeaderText="<%$ Resources:Attendance,Edit %>">
                                                                    <ItemTemplate>
                                                                        <asp:ImageButton ID="btnEdit" runat="server" CausesValidation="False" CommandArgument='<%# Eval("Emp_Id") %>'
                                                                            ImageUrl="~/Images/edit.png" Visible="true" OnCommand="btnEditary_Command"
                                                                            Width="16px" />
                                                                    </ItemTemplate>
                                                                    <ItemStyle HorizontalAlign="Center" />
                                                                </asp:TemplateField>--%>
                                                                <asp:TemplateField HeaderText="<%$ Resources:Attendance,Employee Code %>" SortExpression="Emp_Code">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblEmpCode" runat="server" Text='<%# Eval("Emp_Code") %>'></asp:Label>
                                                                        <asp:Label ID="lblEmpId" runat="server" Text='<%# Eval("Emp_Id") %>' Visible="false"></asp:Label>
                                                                    </ItemTemplate>
                                                                    <ItemStyle CssClass="grid" />
                                                                </asp:TemplateField>
                                                                <asp:BoundField DataField="Emp_Name" HeaderText="<%$ Resources:Attendance,Employee Name %>"
                                                                    ItemStyle-CssClass="grid" ItemStyle-Width="20%" SortExpression="Emp_Name" />
                                                                <asp:BoundField DataField="Emp_Name_L" HeaderText="<%$ Resources:Attendance,Employee Name Local%>"
                                                                    ItemStyle-CssClass="grid" ItemStyle-Width="20%" SortExpression="Emp_Name_L" />
                                                                <%--Designation--%>
                                                                <asp:BoundField DataField="Department" HeaderText="<%$ Resources:Attendance,Department %>"
                                                                    ItemStyle-CssClass="grid" ItemStyle-Width="20%" SortExpression="Department" />

                                                                <asp:TemplateField HeaderText="<%$ Resources:Attendance,Email Id %>">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblProductCode" runat="server" Text='<%# Eval("Email_Id") %>'></asp:Label>
                                                                    </ItemTemplate>
                                                                    <ItemStyle CssClass="grid" />
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="<%$ Resources:Attendance,Phone No. %>">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblItemType" runat="server" Text='<%# Eval("Phone_No") %>'></asp:Label>
                                                                    </ItemTemplate>
                                                                    <ItemStyle CssClass="grid" />
                                                                </asp:TemplateField>
                                                            </Columns>
                                                            <AlternatingRowStyle CssClass="InvgridAltRow" />
                                                            <HeaderStyle BorderColor="#6E6E6E" BorderStyle="Solid" BorderWidth="1px" CssClass="Invgridheader" />
                                                            <PagerStyle CssClass="pagination-ys" />
                                                            <RowStyle CssClass="Invgridrow" HorizontalAlign="Center" />
                                                        </asp:GridView>
                                                        <asp:HiddenField ID="hdnFldSelectedValues" runat="server" />
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
                <div class="row">
                    <div class="col-md-12">
                        <div class="box box-warning">
                            <div class="box-header with-border">
                                <h3 class="box-title">Details</h3>
                                <div class="box-tools pull-right">
                                    <button type="button" class="btn btn-box-tool" data-widget="collapse">
                                        <i class="fa fa-minus"></i>
                                    </button>
                                </div>
                            </div>
                            <div class="box-body">
                                <div class="col-md-6">
                                    <asp:Label ID="Label27" runat="server" Text="<%$ Resources:Attendance,From Date %>"></asp:Label>
                                    <a style="color: Red">*</a>
                                    <asp:RequiredFieldValidator EnableClientScript="true" Style="float: right;" runat="server" ID="RequiredFieldValidator3" ValidationGroup="Next"
                                        Display="Dynamic" SetFocusOnError="true" ControlToValidate="txtFromDate" ErrorMessage="<%$ Resources:Attendance,Enter From Date %>"></asp:RequiredFieldValidator>

                                    <asp:TextBox ID="txtFromDate" runat="server" CssClass="form-control"></asp:TextBox>

                                    <cc1:CalendarExtender OnClientShown="showCalendar" ID="txtFrom_CalendarExtender" runat="server" Enabled="True"
                                        TargetControlID="txtFromDate">
                                    </cc1:CalendarExtender>
                                    <br />
                                </div>
                                <div class="col-md-6">
                                    <asp:Label ID="Label3" runat="server" Text="<%$ Resources:Attendance,To Date %>"></asp:Label>
                                    <a style="color: Red">*</a>
                                    <asp:RequiredFieldValidator EnableClientScript="true" Style="float: right;" runat="server" ID="RequiredFieldValidator1" ValidationGroup="Next"
                                        Display="Dynamic" SetFocusOnError="true" ControlToValidate="txtToDate" ErrorMessage="<%$ Resources:Attendance,Enter To Date %>"></asp:RequiredFieldValidator>

                                    <asp:TextBox ID="txtToDate" runat="server" CssClass="form-control"></asp:TextBox>
                                    <cc1:CalendarExtender OnClientShown="showCalendar" ID="CalendarExtender1" runat="server" Enabled="True" TargetControlID="txtToDate">
                                    </cc1:CalendarExtender>

                                    <br />
                                </div>
                                <div style="text-align: center;" class="col-md-12">


                                    <asp:Button ID="btnReset" runat="server" CssClass="btn btn-primary" OnClick="btnReset_Click"
                                        Text="<%$ Resources:Attendance,Reset %>" Visible="true" />
                                    <asp:Button ID="btnLogProcess" ValidationGroup="Next" runat="server" CssClass="btn btn-primary" OnClick="btnGenerate_Click"
                                        Text="<%$ Resources:Attendance,Next %>" Visible="true" />
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>

            <div id="pnlReport" visible="false" runat="server">
                <div class="row">
                    <div class="col-md-12">
                        <div style="text-align: center;" class="form-control">
                            <asp:LinkButton ID="lnkChangeFilter" runat="server" Font-Size="18px" 
                                Text="<%$ Resources:Attendance,Change Filter Criteria%>" OnClick="lnkChangeFilter_Click"></asp:LinkButton>
                        </div>
                    </div>
                </div>
                <div class="row">
                    <div class="col-md-12">
                        <div class="box box-warning box-solid">
                            <div class="box-header with-border">
                                <h3 class="box-title">
                                    <asp:Label ID="Label7" runat="server" Text="<%$ Resources:Attendance,Attendance Report %>"></asp:Label></h3>
                                <div class="box-tools pull-right">
                                    <button type="button" class="btn btn-box-tool" data-widget="collapse">
                                        <i class="fa fa-minus"></i>
                                    </button>
                                </div>
                            </div>
                            <div class="box-body">

                                <div class="row">
                                    <div class="col-md-12">
                                        <div class="box box-primary">
                                            <div class="box-header with-border">

                                                <div class="box-tools pull-right">
                                                    <button type="button" class="btn btn-box-tool" data-widget="collapse">
                                                        <i class="fa fa-minus"></i>
                                                    </button>
                                                </div>
                                            </div>
                                            <div class="box-body">

                                                 <asp:LinkButton ID="lnkWorkHourReport" OnClick="lnkWorkHourReport_Click"
                                                    Text="Work Hour Details Report" runat="server" Visible="false"></asp:LinkButton>
                                                <br />

                                                <asp:LinkButton ID="LNK_Access_Group_Summary_Detail" OnClick="LNK_Access_Group_Summary_Detail_Click"
                                                    Text="<%$ Resources:Attendance,Time Attendance Details Report%>" runat="server" Visible="false"></asp:LinkButton>
                                                <br />

                                                <asp:LinkButton ID="lnkExceptionCount" Text="<%$ Resources:Attendance,Exception Count Report%>"
                                                    runat="server" OnClick="lnkExceptionCount_Click" Visible="false"></asp:LinkButton>
                                                <br />

                                                <asp:LinkButton ID="lnkEmployeeTracking" Text="<%$ Resources:Attendance,Employee Tracking Report%>"
                                                    runat="server" OnClick="lnkEmployeeTracking_Click" Visible="false"></asp:LinkButton>
                                                <br />


                                                <asp:LinkButton ID="linkLogProcess" Text="<%$ Resources:Attendance,Log Detail Report %>"
                                                    runat="server" OnClick="linkLogProcess_Click" Visible="false"></asp:LinkButton>
                                                <br />

                                               
                                              

                                                <asp:LinkButton ID="linkInOutseperate" Text="<%$ Resources:Attendance,In Out Report%>"
                                                    runat="server" OnClick="linkInOutseperate_Click" Visible="false"></asp:LinkButton>
                                                <br />

                                                 <asp:LinkButton ID="linkInOutDriect" Text="In Out Report Direct"
                                                    runat="server" OnClick="linkInOutDriect_Click" Visible="false"></asp:LinkButton>
                                                <br />

                                            
                                                <asp:LinkButton ID="lnkShiftReport" OnClick="lnkShiftReport_Click" Visible="false"
                                                    Text="<%$ Resources:Attendance,Shift Report %>" runat="server" />
                                                <br />
                                                <asp:LinkButton ID="lnkWithoutShiftReport" OnClick="lnkWithoutShiftReport_Click" Visible="false"
                                                    Text="Shift Not Assigned Report" runat="server" />
                                                <br />
                                                <asp:LinkButton ID="lnkshiftscheduleReport" Text="<%$ Resources:Attendance,Shift Schedule Report%>"
                                                    Visible="false" runat="server" OnClick="lnkshiftscheduleReport_Click"></asp:LinkButton>
                                                <br />


                                                <asp:LinkButton ID="linkWeekOffReport" Text="<%$ Resources:Attendance,Week Off Report%>"
                                                    Visible="false" runat="server" OnClick="linkWeekOffReport_Click"></asp:LinkButton>
                                                <br />
                                                <%--<asp:LinkButton ID="lnkDutyResumtionReport"  Text="Duty Resumption Report"
                                                    runat="server" OnClick="lnkDutyResumtionReport_Click"></asp:LinkButton>
                                                <br />
                                                <asp:LinkButton ID="lnkSalaryCertificaterequestForm"  Text="Salary Certificate Request Form"
                                                    runat="server" OnClick="lnkSalaryCertificaterequestForm_Click"></asp:LinkButton>
                                                <br />--%>

                                                 <asp:LinkButton ID="linkEmpInfo" OnClick="linkEmpInfo_Click" Visible="false"
                                                    Text="<%$ Resources:Attendance,Employee Information Report%>" runat="server"></asp:LinkButton>
                                                <br />
                                                <asp:LinkButton ID="linkHolidayReport" Text="<%$ Resources:Attendance,Holiday Report%>"
                                                    Visible="false" runat="server" OnClick="linkHolidayReport_Click"></asp:LinkButton>
                                                <br />

                                                <asp:LinkButton ID="linkEmployeeHolidayReport" Text="Employee Holiday Report"
                                                    Visible="false" runat="server" OnClick="linkEmployeeHolidayReport_Click"></asp:LinkButton>
                                                <br />
                                                  <asp:LinkButton ID="LinkButton12" Text="<%$ Resources:Attendance,In Out Exception Report %>"
                                                    runat="server" OnClick="LinkButton12_Click" Visible="false"></asp:LinkButton>
                                                <br />

                                                <asp:LinkButton ID="linkPendingHolidayReport" Text="TSC Pending Public Holidays"
                                                    runat="server" Visible="false" OnClick="linkPendingHolidayReport_Click"></asp:LinkButton>

                                                <br />

                                                <asp:LinkButton ID="linkTimeDeductionReport" Text="TSC Times Deductions"
                                                    Visible="false" runat="server" OnClick="linkTimeDeductionReport_Click"></asp:LinkButton>
                                                <br />

                                                <asp:LinkButton ID="linkPendingoffReport" Text="TSC Pending Off Report"
                                                    runat="server" Visible="false" OnClick="linkPendingoffReport_Click"></asp:LinkButton>

                                                <br />

                                                <asp:LinkButton ID="linkTSCOvertimeReport" Text="TSC Overtime Report" Visible="false"
                                                    runat="server" OnClick="linkTSCOvertimeReport_Click"></asp:LinkButton>

                                                <br />
                                                    <asp:LinkButton ID="linkDailySalary"  OnClick="linkDailySalary_Click"
                                                    Visible="false" Text="<%$ Resources:Attendance,Salary Report%>" runat="server"></asp:LinkButton>
                                                <br />


                                            </div>
                                        </div>
                                    </div>


                                </div>
                                <div class="row" runat="server" >
                                    <div class="col-md-12">
                                        <div class="box box-primary">
                                            <div class="box-header with-border">
                                                <h3 class="box-title">Detail Report</h3>
                                                <div class="box-tools pull-right">
                                                    <button type="button" class="btn btn-box-tool" data-widget="collapse">
                                                        <i class="fa fa-minus"></i>
                                                    </button>
                                                </div>
                                            </div>
                                            <div class="box-body">

                                                <div style="margin-right: 10%;" class="col-md-3">
                                                    <asp:LinkButton ID="linkInOut"  Text="In Out Report(Continue)"
                                                        runat="server" OnClick="linkInOut_Click" Visible="false"></asp:LinkButton>
                                                    <br />

                                                    <asp:LinkButton ID="linkLatein"  Text="<%$ Resources:Attendance,Late In Report%>"
                                                        runat="server" OnClick="linkLatein_Click" Visible="false"></asp:LinkButton>
                                                    <br />
                                                    <asp:LinkButton ID="linkEarlyOut"  Text="<%$ Resources:Attendance,Early Out Report%>"
                                                        runat="server" OnClick="linkEarlyOut_Click" Visible="false"></asp:LinkButton>
                                                    <br />
                                                    <asp:LinkButton ID="linkLeaveReport"  Text="<%$ Resources:Attendance,Leave Report %>"
                                                        runat="server" OnClick="linkLeaveReport_Click" Visible="false"></asp:LinkButton>
                                                    <br />
                                                    <asp:LinkButton ID="linkLeaveStatus"  Text="<%$ Resources:Attendance,Leave Status Report %>"
                                                        runat="server" OnClick="linkLeaveStatus_Click" Visible="false"></asp:LinkButton>
                                                    <br />
                                                    <asp:LinkButton ID="linkLeaveRemaning"  Text="<%$ Resources:Attendance,Leave Remaining Report %>"
                                                        runat="server" OnClick="linkLeaveRemaning_Click" Visible="false"></asp:LinkButton>
                                                    <br />
                                                </div>

                                                <div style="margin-right: 10%;" class="col-md-3">
                                                    <asp:LinkButton ID="linkAbsentReport"  Text="<%$ Resources:Attendance,Absent Report%>"
                                                        runat="server" OnClick="linkAbsentReport_Click" Visible="false"></asp:LinkButton>
                                                    <br />
                                                    <asp:LinkButton ID="linkOverTime"  Text="<%$ Resources:Attendance,Over Time Report%>"
                                                        runat="server" OnClick="linkOverTime_Click" Visible="false"></asp:LinkButton>
                                                    <br />
                                                     <asp:LinkButton ID="lnkLateIn"  OnClick="lnkLateIn_Click" Text="<%$ Resources:Attendance,Late Count Report%>"
                                                        runat="server" Visible="false"></asp:LinkButton>
                                                    <br />
                                                     <asp:LinkButton ID="LinkButton1"  Text="Leave Location Report"
                                                        runat="server" OnClick="linkLocation_Click" Visible="false"></asp:LinkButton>
                                                    <br />

                                                    <asp:LinkButton ID="linkMailTransaction"  Text="<%$ Resources:Attendance,Mail Transaction Report%>"
                                                        runat="server" OnClick="linkMailTransaction_Click" Visible="false"></asp:LinkButton>
                                                    <br />
                                                   

                                                    <asp:LinkButton ID="linkBreakInOut"  Text="<%$ Resources:Attendance,Break Violation Report%>"
                                                        runat="server" OnClick="linkBreakInOut_Click" Visible="false"></asp:LinkButton>
                                                    <br />
                                                    <asp:LinkButton ID="linkHalfDay"  OnClick="linkHalfDay_Click" Text="<%$ Resources:Attendance,Half Day Leave Report%>"
                                                        runat="server" Visible="false"></asp:LinkButton>
                                                    <br />
                                                </div>

                                                <div class="col-md-3">
                                                    <asp:LinkButton ID="linkPartialLeave"  Text="<%$ Resources:Attendance,Partial Leave Report %>"
                                                        runat="server" OnClick="linkPartialLeave_Click" Visible="false"></asp:LinkButton>
                                                    <br />
                                             
                                                    <asp:LinkButton ID="linkTimeCardReport"  Text="Time Card Report"
                                                        runat="server" OnClick="linkTimeCardReport_Click" Visible="false"></asp:LinkButton>
                                                    <br />
                                                      <asp:LinkButton ID="lnkAbsentDetail"  Text="Absent Detail Report"
                                                        runat="server" OnClick="lnkAbsentDetail_Click" Visible="false"></asp:LinkButton>
                                                    <br />
                                                      <asp:LinkButton ID="lnlClocl10"  Text="10 O Clock Report"
                                                        runat="server" OnClick="lnlClocl10_Click" Visible="false"></asp:LinkButton>
                                                    <br />

                                                           <asp:LinkButton ID="linkPartialViolation"  Text="<%$ Resources:Attendance,Partial Violation Report %>"
                                                        runat="server" OnClick="linkPartialViolation_Click" Visible="false"></asp:LinkButton>
                                                    <br />
                                                    <asp:LinkButton ID="linkUserTransfer"  Text="<%$ Resources:Attendance,User Transfer Report%>"
                                                        runat="server" OnClick="linkUserTransfer_Click" Visible="false"></asp:LinkButton>
                                                    <br />
                                                    <asp:LinkButton ID="linkSmsTransaction"  OnClick="linkSmsTransaction_Click"
                                                        Visible="false" Text="<%$ Resources:Attendance,SMS Transaction Report%>" runat="server"></asp:LinkButton>
                                                    <br />
                                                    <asp:LinkButton ID="linkTourAttendance"  OnClick="linkTourAttendance_Click"
                                                        Text="<%$ Resources:Attendance,Tour Attendance Report%>" runat="server" Visible="false"></asp:LinkButton>
                                                    <br />


                                                    <br />
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                                <div class="row" runat="server" visible="false">
                                    <div class="col-md-3">
                                        <div class="box box-primary">
                                            <div class="box-header with-border">
                                                <h3 class="box-title">Chart Report</h3>
                                                <div class="box-tools pull-right">
                                                    <button type="button" class="btn btn-box-tool" data-widget="collapse">
                                                        <i class="fa fa-minus"></i>
                                                    </button>
                                                </div>
                                            </div>
                                            <div class="box-body">
                                                <asp:LinkButton ID="Lnk_Attendance_Register"  OnClick="Lnk_Attendance_Register_Click"
                                                    Text="<%$ Resources:Attendance,Attendance Register%>" runat="server" Visible="false"></asp:LinkButton>
                                                <br />

                                                <asp:LinkButton ID="LNK_Access_Group_Summary"  OnClick="LNK_Access_Group_Summary_Click"
                                                    Text="<%$ Resources:Attendance,Access Group Summary%>" runat="server" Visible="false"></asp:LinkButton>
                                                <br />

                                                <span style="font-size: 1px;"></span>
                                            </div>
                                        </div>
                                    </div>

                                    <div class="col-md-3">
                                        <div class="box box-primary">
                                            <div class="box-header with-border">
                                                <h3 class="box-title">Summary Report</h3>
                                                <div class="box-tools pull-right">
                                                    <button type="button" class="btn btn-box-tool" data-widget="collapse">
                                                        <i class="fa fa-minus"></i>
                                                    </button>
                                                </div>
                                            </div>
                                            <div class="box-body">

                                                <asp:LinkButton ID="lnkLogReport"  Text="<%$ Resources:Attendance,Log Report %>"
                                                    runat="server" OnClick="lnkLogReport_Click" Visible="false"></asp:LinkButton>

                                            </div>
                                        </div>
                                    </div>

                                    <div class="col-md-3">
                                        <div class="box box-primary">
                                            <div class="box-header with-border">
                                                <h3 class="box-title">Dril Report</h3>
                                                <div class="box-tools pull-right">
                                                    <button type="button" class="btn btn-box-tool" data-widget="collapse">
                                                        <i class="fa fa-minus"></i>
                                                    </button>
                                                </div>
                                            </div>
                                            <div class="box-body">
                                            </div>
                                        </div>
                                    </div>

                                    <div class="col-md-3">
                                        <div class="box box-primary">
                                            <div class="box-header with-border">
                                                <h3 class="box-title">Exception Report</h3>
                                                <div class="box-tools pull-right">
                                                    <button type="button" class="btn btn-box-tool" data-widget="collapse">
                                                        <i class="fa fa-minus"></i>
                                                    </button>
                                                </div>
                                            </div>
                                            <div class="box-body">

                                                <br />

                                                <br />
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>

                <div class="row" runat="server" visible="false">
                    <div class="col-md-12">
                        <div class="box box-warning box-solid">
                            <div class="box-header with-border">
                                <h3 class="box-title">After Log Post</h3>
                                <div class="box-tools pull-right">
                                    <button type="button" class="btn btn-box-tool" data-widget="collapse">
                                        <i class="fa fa-minus"></i>
                                    </button>
                                </div>
                            </div>
                            <div class="box-body">

                                <div class="row">
                                    <div class="col-md-12">
                                        <div class="box box-primary">
                                            <div class="box-header with-border">
                                                <h3 class="box-title">General Report</h3>
                                                <div class="box-tools pull-right">
                                                    <button type="button" class="btn btn-box-tool" data-widget="collapse">
                                                        <i class="fa fa-minus"></i>
                                                    </button>
                                                </div>
                                            </div>
                                            <div class="box-body">
                                                <asp:LinkButton ID="Lnk_Attendance_Summary_Report"  OnClick="Lnk_Attendance_Summary_Report_Click"
                                                    Text="<%$ Resources:Attendance,Attendance Summary Report%>" runat="server" Visible="false"></asp:LinkButton>
                                                <br />
                                                <asp:LinkButton ID="Lnl_Attendance_Salary_Report"  OnClick="Lnl_Attendance_Salary_Report_Click"
                                                    Text="<%$ Resources:Attendance,Attendance Salary Report%>" runat="server" Visible="false"></asp:LinkButton>
                                                <br />
                                                <asp:LinkButton ID="Lnl_Attendance_TimeSheet_Report"  OnClick="Lnl_Attendance_TimeSheet_Report_Click"
                                                    Text="<%$ Resources:Attendance,Attendance TimeSheet Report%>" runat="server" Visible="false"></asp:LinkButton>
                                                <br />
                                                <asp:LinkButton ID="Lnl_Attendance_Salary_Summary_Report"  OnClick="Lnl_Attendance_Salary_Summary_Report_Click"
                                                    Text="<%$ Resources:Attendance,Attendance Salary Summary Report%>" runat="server" Visible="false"></asp:LinkButton>
                                                <br />
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


    <asp:UpdateProgress ID="UpdateProgress1" runat="server" AssociatedUpdatePanelID="Update_Form">
        <ProgressTemplate>
            <div class="modal_Progress">
                <div class="center_Progress">
                    <img alt="" src="../Images/ajax-loader2.gif" style="vertical-align: middle; background-color: White; border-radius: 10px;" />
                </div>
            </div>
        </ProgressTemplate>
    </asp:UpdateProgress>

    <div class="modal fade" id="myModal" tabindex="-1" role="dialog" aria-labelledby="myModalLabel"
        aria-hidden="true">
        <div class="modal-dialog modal-lg">
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal">
                        <span aria-hidden="true">&times;</span><span class="sr-only"><asp:Label ID="Label2" runat="server" Text="<%$ Resources:Attendance,Close%>"></asp:Label></span></button>
                    <h4 class="modal-title" id="myModalLabel">
                        <asp:Label ID="Label1" runat="server" Text="<%$ Resources:Attendance,Filter Department%>"></asp:Label></h4>
                </div>
                <div class="modal-body">
                    <asp:UpdatePanel ID="update_Modal" runat="server">
                        <ContentTemplate>
                            <div class="row">
                                <div class="col-md-12">

                                    <asp:CheckBox ID="chkSelectAll" runat="server" CssClass="labelComman" onClick="new_validation();" Text="<%$ Resources:Attendance,Select All %>" />


                                    <div class="box box-primary">
                                        <div class="box-body">
                                            <div class="form-group">

                                                <div class="col-md-12" style="overflow: auto; max-height: 300px">


                                                    <asp:TreeView ID="TreeViewDepartment" runat="server"></asp:TreeView>

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
                    <asp:UpdatePanel ID="Update_Modal_Button" runat="server">
                        <ContentTemplate>
                            <asp:Button ID="btnSave" OnClick="btnsave_OnClick" runat="server" CausesValidation="False" CssClass="btn btn-primary"
                                Text="<%$ Resources:Attendance,Save %>" />

                            <button type="button" class="btn btn-default" data-dismiss="modal">
                                <asp:Label ID="Label4" runat="server" Text="<%$ Resources:Attendance,Close%>"></asp:Label></button>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
            </div>
        </div>
    </div>


    <asp:Panel ID="pnlEmpAtt" runat="server" Visible="false"></asp:Panel>
    <asp:Panel ID="pnlSearchdpl" runat="server" Visible="false"></asp:Panel>
    <asp:Panel ID="pnlEmp" runat="server" Visible="false"></asp:Panel>
    <asp:Panel ID="pnlGroupSal" runat="server" Visible="false"></asp:Panel>
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
    <script type="text/javascript">
        function resetPosition(object, args) {
            var tb = object._element;
            //var tbl = document.getElementById('txtProductcode')
            //alert(tbl.style.height);

            //alert(object._height);
            var tbposition = findPositionWithScrolling(tb);
            var xposition = tbposition[0] + 2;
            var yposition = tbposition[1] + 25; // 22 textbox height 
            var ex = object._completionListElement;
            if (ex)
                $common.setLocation(ex, new Sys.UI.Point(xposition, yposition));
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

        function Grid_Validate(sender, args) {
            if (document.getElementById("<%=rbtnGroupSal.ClientID %>").checked) {
                var groupbox = document.getElementById("<%=lbxGroupSal.ClientID %>");
                var Select_Index = groupbox.getElementsByTagName("option");
                for (var i = 0; i < Select_Index.length; i++) {
                    if (Select_Index[i].selected) {
                        args.IsValid = true;
                        return;
                    }
                }
                args.IsValid = false;
            }
            else {
                var gridView = document.getElementById("<%=gvEmployee.ClientID %>");
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



        function show_modal() {
            document.getElementById('<%=Btn_Modal_Popup.ClientID %>').click();
        }
        function Modal_Close() {
            document.getElementById('<%= Btn_Modal_Popup.ClientID %>').click();
        }
    </script>
</asp:Content>
