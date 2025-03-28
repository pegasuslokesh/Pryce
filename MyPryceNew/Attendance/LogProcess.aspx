<%@ Page Title="" Language="C#" MasterPageFile="~/ERPMaster.master" AutoEventWireup="true" CodeFile="LogProcess.aspx.cs" EnableSessionState="True" Inherits="Attendance_LogProcess" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="Head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="HeaderContent" runat="Server">
    <h1>
        <i class="fas fa-sync"></i>&nbsp;&nbsp;
                <asp:Label ID="lblHeader" runat="server" Text="<%$ Resources:Attendance,Log Process Setup%>"></asp:Label>
    </h1>
    <ol class="breadcrumb">
        <li><a href="#"><i class="fa fa-dashboard"></i>
            <asp:Label ID="Label6" runat="server" Text="<%$ Resources:Attendance,Attendance Module%>"></asp:Label></a></li>
        <li><a href="#"><i class="fa fa-dashboard"></i>
            <asp:Label ID="Label10" runat="server" Text="<%$ Resources:Attendance,Attendance Module%>"></asp:Label></a></li>
        <li class="active">
            <asp:Label ID="Label5" runat="server" Text="<%$ Resources:Attendance,Log Process%>"></asp:Label></li>
    </ol>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="Server">
    <asp:UpdatePanel ID="Update_New" runat="server">
        <Triggers>
            <asp:PostBackTrigger ControlID="btnvalidationifo" />

        </Triggers>
        <ContentTemplate>

            <div class="row">
                <div class="col-md-12">
                    <div class="box box-primary">
                        <div class="box-body">
                            <div class="form-group">
                                <div class="col-md-6">
                                    <asp:Label ID="lblmonth" runat="server" Font-Size="14px" Text="<%$ Resources:Attendance,Month %>"></asp:Label>
                                    <asp:DropDownList ID="ddlMonth" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlMonth_SelectedIndexChanged" CssClass="form-control">
                                        <asp:ListItem Text="<%$ Resources:Attendance, --Select-- %>" Value="0" />
                                        <asp:ListItem Text="<%$ Resources:Attendance, January %>" Value="1" />
                                        <asp:ListItem Text="<%$ Resources:Attendance, February %>" Value="2" />
                                        <asp:ListItem Text="<%$ Resources:Attendance, March %>" Value="3" />
                                        <asp:ListItem Text="<%$ Resources:Attendance, April %>" Value="4" />
                                        <asp:ListItem Text="<%$ Resources:Attendance, May %>" Value="5" />
                                        <asp:ListItem Text="<%$ Resources:Attendance, June %>" Value="6" />
                                        <asp:ListItem Text="<%$ Resources:Attendance, July %>" Value="7" />
                                        <asp:ListItem Text="<%$ Resources:Attendance, August %>" Value="8" />
                                        <asp:ListItem Text="<%$ Resources:Attendance, September %>" Value="9" />
                                        <asp:ListItem Text="<%$ Resources:Attendance, October %>" Value="10" />
                                        <asp:ListItem Text="<%$ Resources:Attendance, November %>" Value="11" />
                                        <asp:ListItem Text="<%$ Resources:Attendance, December %>" Value="12" />
                                    </asp:DropDownList>
                                    <br />
                                </div>
                                <div class="col-md-6">
                                    <asp:Label ID="lblyeay" runat="server" Font-Size="14px" Text="<%$ Resources:Attendance,Year %>"></asp:Label>
                                    <div class="input-group">
                                        <asp:TextBox ID="txtYear" runat="server" CssClass="form-control"></asp:TextBox>
                                        <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender3" runat="server" Enabled="True"
                                            TargetControlID="TxtYear" FilterType="Numbers">
                                        </cc1:FilteredTextBoxExtender>
                                        <div class="input-group-btn">
                                            <asp:LinkButton ID="imgValidEmployee" runat="server" CausesValidation="False" OnClick="imgValidEmployee_Click" ToolTip="<%$ Resources:Attendance,Search %>"><span class="fa fa-search"  style="font-size:25px;margin-left:5px;"></span></asp:LinkButton>
                                        </div>
                                    </div>
                                    <br />
                                </div>
                                <div class="col-md-12" style="text-align: center">
                                    <asp:HiddenField ID="editid" runat="server" />
                                    <asp:RadioButton ID="rbtnGroupSal" OnCheckedChanged="EmpGroupSal_CheckedChanged"
                                        runat="server" Text="<%$ Resources:Attendance,Group %>" Font-Bold="true" GroupName="EmpGroup"
                                        AutoPostBack="true" />
                                    <asp:RadioButton ID="rbtnEmpSal" Style="margin-left: 25px;" runat="server" AutoPostBack="true"
                                        Text="<%$ Resources:Attendance,Employee %>" GroupName="EmpGroup" Font-Bold="true"
                                        OnCheckedChanged="EmpGroupSal_CheckedChanged" />
                                </div>
                                <div class="col-md-12">
                                    <br />
                                </div>
                                <div class="col-md-6">
                                    <asp:Label ID="lblLocation" runat="server" Text="<%$ Resources:Attendance,Location %>"></asp:Label>
                                    <asp:DropDownList ID="ddlLocation" runat="server" CssClass="form-control" AutoPostBack="true" OnSelectedIndexChanged="dpLocation_SelectedIndexChanged">
                                    </asp:DropDownList>
                                    <asp:Label ID="lblEmp" runat="server"></asp:Label>
                                </div>
                                <div class="col-md-6">
                                    <asp:Label ID="lblGroupByDept" runat="server" Text="<%$ Resources:Attendance,Department %>"></asp:Label>
                                    <div class="input-group">
                                        <asp:DropDownList ID="dpDepartment" runat="server" CssClass="form-control" AutoPostBack="true" OnSelectedIndexChanged="dpDepartment_SelectedIndexChanged">
                                        </asp:DropDownList>
                                        <div class="input-group-btn">
                                            <asp:LinkButton ID="ImageButton1" runat="server" CausesValidation="False" OnClick="btnAllRefresh_Click" ToolTip="<%$ Resources:Attendance,Refresh %>"><span class="fa fa-repeat"  style="font-size:25px;margin-left:5px"></span></asp:LinkButton>
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
  <asp:Label ID="lblTotalRecord" Font-Bold="true" runat="server" Text="<%$ Resources:Attendance,Total Records: 0 %>"></asp:Label>

                                <div class="box-tools pull-right">
                                    <button type="button" class="btn btn-box-tool" data-widget="collapse">
                                        <i id="I1" runat="server" class="fa fa-plus"></i>
                                    </button>
                                </div>
                            </div>
                            <div class="box-body">
                                <div class="col-lg-3">
                                    <asp:DropDownList ID="ddlField" runat="server" CssClass="form-control">
                                        <asp:ListItem Text="<%$ Resources:Attendance,Employee Code %>" Value="Emp_Code" Selected="True"></asp:ListItem>
                                        <asp:ListItem Text="<%$ Resources:Attendance,Employee Name %>" Value="Emp_Name" ></asp:ListItem>
                                        <asp:ListItem Text="<%$ Resources:Attendance,Employee Name Local %>" Value="Emp_Name_L"></asp:ListItem>
                                    </asp:DropDownList>
                                </div>
                                <div class="col-lg-2">
                                    <asp:DropDownList ID="ddlOption" runat="server" CssClass="form-control">
                                        <asp:ListItem Text="<%$ Resources:Attendance,--Select--%>" Value="--Select one--"></asp:ListItem>
                                        <asp:ListItem Text="<%$ Resources:Attendance,Equal %>" Value="Equal"></asp:ListItem>
                                        <asp:ListItem Text="<%$ Resources:Attendance,Contains %>" Selected="True" Value="Contains"></asp:ListItem>
                                        <asp:ListItem Text="<%$ Resources:Attendance,Like%>" Value="Like"></asp:ListItem>
                                    </asp:DropDownList>
                                </div>
                                <div class="col-lg-4">
                                    <asp:Panel ID="Panel1" runat="server" DefaultButton="btnEmp">
                                        <asp:TextBox placeholder="Search from Content" ID="txtValue" runat="server" CssClass="form-control"></asp:TextBox>
                                    </asp:Panel>
                                </div>
                                <div class="col-lg-3">
                                    <asp:LinkButton ID="btnEmp" runat="server" CausesValidation="False" OnClick="btnarybind_Click1"><span class="fa fa-search"  style="font-size:25px;"></span></asp:LinkButton>&nbsp;&nbsp;&nbsp;
                                    <asp:LinkButton ID="ImageButton9" runat="server" CausesValidation="False" OnClick="btnaryRefresh_Click1"><span class="fa fa-repeat"  style="font-size:25px;"></span></asp:LinkButton>&nbsp;&nbsp;&nbsp;
                                    <asp:LinkButton ID="ImageButton10" runat="server" OnClick="ImgbtnSelectAll_Clickary" ToolTip="<%$ Resources:Attendance, Select All %>"><span class="fas fa-th"  style="font-size:25px;"></span></asp:LinkButton>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="box box-warning box-solid" <%= gvEmployee.Rows.Count>0?"style='display:block'":"style='display:none'"%>>
                    <div class="box-body">
                        <div class="row">
                            <div class="col-md-12">
                                <div class="flow">
                                    <asp:HiddenField ID="HDFSort" runat="server" />
                                    <asp:Label ID="lblSelectRecord" runat="server" Visible="false"></asp:Label>
                                    <asp:GridView CssClass="table-striped table-bordered table table-hover" ID="gvEmployee" runat="server" AllowPaging="True" AutoGenerateColumns="False" AllowSorting="true"
                                        OnPageIndexChanging="gvEmp_PageIndexChanging" Width="100%" DataKeyNames="Emp_Id" OnSorting="gvEmployee_OnSorting"
                                        PageSize="<%# PageControlCommon.GetPageSize() %>" Visible="true">
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
                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,Employee Code %>" SortExpression="Emp_Code">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblEmpCode" runat="server" Text='<%# Eval("Emp_Code") %>'></asp:Label>
                                                    <asp:Label ID="lblEmpId" Visible="false" runat="server" Text='<%# Eval("Emp_Id") %>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle />
                                            </asp:TemplateField>
                                            <asp:BoundField DataField="Emp_Name" HeaderText="<%$ Resources:Attendance,Employee Name %>"
                                                SortExpression="Emp_Name" ItemStyle-Width="20%" />
                                            <asp:BoundField DataField="Emp_Name_L" HeaderText="<%$ Resources:Attendance,Employee Name Local %>"
                                                SortExpression="Emp_Name_L" ItemStyle-Width="20%" />
                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,Date of Joining %>">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblDOB" runat="server" Text='<%# DateFormat(Eval("DOJ").ToString()) %>'></asp:Label>
                                                    <asp:Label ID="lblJoiningMonth" runat="server" Visible="false" Text='<%# Eval("JoiningMonth") %>'></asp:Label>
                                                    <asp:Label ID="lblJoiningYear" runat="server" Visible="false" Text='<%# Eval("JoiningYear") %>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,Email Id %>">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblProductCode" runat="server" Text='<%# Eval("Email_Id") %>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,Phone No. %>">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblItemType" runat="server" Text='<%# Eval("Phone_No") %>'></asp:Label>
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
            </div>
            <div id="Div_Group" runat="server" visible="false">
                <div class="row">
                    <div class="col-md-12">
                        <div class="box box-primary">
                            <div class="box-body">
                                <div class="form-group">
                                    <div class="col-md-3" style="overflow: auto">
                                        <asp:ListBox ID="lbxGroupSal" runat="server" Height="200px" Style="width: 100%" SelectionMode="Multiple"
                                            AutoPostBack="true" OnSelectedIndexChanged="lbxGroupSal_SelectedIndexChanged" CssClass="list"
                                            Font-Names="Trebuchet MS" Font-Size="Small" ForeColor="Gray"></asp:ListBox>
                                    </div>
                                    <div class="col-md-9">
                                        <div style="overflow: auto">
                                            <asp:GridView CssClass="table-striped table-bordered table table-hover" ID="gvEmployeeSal" runat="server" AllowPaging="True" AutoGenerateColumns="False"
                                                OnPageIndexChanging="gvEmployeeSal_PageIndexChanging" Width="100%"
                                                PageSize="<%# PageControlCommon.GetPageSize() %>">
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
                                                    <asp:BoundField DataField="Emp_Name_L" HeaderText="<%$ Resources:Attendance,Employee Name Local %>"
                                                        SortExpression="Emp_Name_L" ItemStyle-Width="40%" />
                                                    <asp:TemplateField HeaderText="<%$ Resources:Attendance,Email Id %>">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblProductCode" runat="server" Text='<%# Eval("Email_Id") %>'></asp:Label>
                                                        </ItemTemplate>
                                                        <ItemStyle />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="<%$ Resources:Attendance,Designation Name %>">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblItemType" runat="server" Text='<%# Eval("Designation") %>'></asp:Label>
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
                    </div>
                </div>
            </div>
            <div class="row">
                <div class="col-md-12">
                    <div class="box box-primary">
                        <div class="box-body">
                            <div class="form-group">
                                <div id="Div_From_Date" runat="server" visible="false" class="col-md-6">
                                    <asp:Label ID="Label27" runat="server" Text="<%$ Resources:Attendance,From Date %>"></asp:Label>

                                    <a style="color: Red">*</a>
                                    <asp:RequiredFieldValidator EnableClientScript="true" Style="float: right;" runat="server"
                                        ID="RequiredFieldValidator2" ValidationGroup="Save" Display="Dynamic" SetFocusOnError="true"
                                        ControlToValidate="txtFromDate" ErrorMessage="Enter From date"></asp:RequiredFieldValidator>

                                    <asp:TextBox ID="txtFromDate" runat="server" CssClass="form-control"></asp:TextBox>
                                    <cc1:CalendarExtender OnClientShown="showCalendar" ID="txtFrom_CalendarExtender" runat="server" Enabled="True"
                                        TargetControlID="txtFromDate">
                                    </cc1:CalendarExtender>
                                    <br />
                                </div>
                                <div id="Div_To_Date" runat="server" visible="false" class="col-md-6">
                                    <asp:Label ID="Label3" runat="server" Text="<%$ Resources:Attendance,To Date %>"></asp:Label>

                                    <a style="color: Red">*</a>
                                    <asp:RequiredFieldValidator EnableClientScript="true" Style="float: right;" runat="server"
                                        ID="RequiredFieldValidator1" ValidationGroup="Save" Display="Dynamic" SetFocusOnError="true"
                                        ControlToValidate="txtToDate" ErrorMessage="Enter To date"></asp:RequiredFieldValidator>
                                    <asp:TextBox ID="txtToDate" runat="server" CssClass="form-control"></asp:TextBox>
                                    <cc1:CalendarExtender OnClientShown="showCalendar" ID="CalendarExtender1" runat="server" Enabled="True" TargetControlID="txtToDate">
                                    </cc1:CalendarExtender>
                                    <br />
                                </div>
                                <div class="col-md-12" style="text-align: center">
                                    <asp:Button ID="btnLogProcess" Width="100px" runat="server" OnClick="btnLogProcess_Click" ValidationGroup="Save"
                                        Visible="false" Text="<%$ Resources:Attendance,Log Process %>" CssClass="btn btn-primary" />

                                    <asp:Button ID="btnReset" runat="server" OnClick="btnReset_Click" Text="<%$ Resources:Attendance,Reset %>"
                                        Visible="false" CssClass="btn btn-primary" />

                                    <asp:Button ID="btnLogPost" Width="100px" runat="server" OnClick="btnLogPost1_Click" ValidationGroup="Save"
                                        Visible="false" Text="<%$ Resources:Attendance,Log Post %>" CssClass="btn btn-primary" />

                                    <asp:Button ID="btnPayroll" Width="120px" runat="server" OnClick="btnPayroll_Click" Visible="false"
                                        Text="<%$ Resources:Attendance,Generate Payroll %>" CssClass="btn btn-primary" />


                                    <asp:Button ID="btnvalidationifo" Width="120px" runat="server" OnClick="btnvalidationifo_Click"
                                        Text="<%$ Resources:Attendance,Validation Info%>" CssClass="btn btn-primary" />
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>

        </ContentTemplate>
    </asp:UpdatePanel>


    <asp:UpdateProgress ID="UpdateProgress11" runat="server" AssociatedUpdatePanelID="Update_New">
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
        function LI_Edit_Active() {

        }
    </script>
</asp:Content>

