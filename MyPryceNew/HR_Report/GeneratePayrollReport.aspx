<%@ Page Title="" Language="C#" MasterPageFile="~/ERPMaster.master" AutoEventWireup="true" CodeFile="GeneratePayrollReport.aspx.cs" Inherits="HR_Report_GeneratePayrollReport" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="Head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="HeaderContent" runat="Server">
    <h1>
        <img src="../Images/InvStyle.png" alt="" />
        <asp:Label ID="lblHeader" runat="server" Text="<%$ Resources:Attendance,HR Report %>"></asp:Label>
    </h1>
    <ol class="breadcrumb">
        <li><a href="#"><i class="fa fa-dashboard"></i>
            <asp:Label ID="Label6" runat="server" Text="<%$ Resources:Attendance,HR Report%>"></asp:Label></a></li>
        <li><a href="#"><i class="fa fa-dashboard"></i>
            <asp:Label ID="Label10" runat="server" Text="<%$ Resources:Attendance,HR Report%>"></asp:Label></a></li>
        <li class="active">
            <asp:Label ID="Label5" runat="server" Text="<%$ Resources:Attendance,HR Report%>"></asp:Label></li>
    </ol>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="Server">
    <asp:UpdatePanel ID="Update_New" runat="server">
        <ContentTemplate>
            <div class="row">
                <div class="col-md-12">
                    <div class="box box-primary">
                        <div class="box-body">
                            <div class="form-group">
                                <asp:Panel ID="PnlEmployeePayrollReport" runat="server">

                                    <div class="col-md-12">

                                        <div class="col-md-6" style="text-align: center">
                                            <asp:RadioButton ID="rbtnGroup" Style="margin-left: 20px; margin-right: 20px;" OnCheckedChanged="EmpGroup_CheckedChanged"
                                                runat="server" Text="<%$ Resources:Attendance,Group %>" Font-Bold="true" GroupName="EmpGroup"
                                                AutoPostBack="true" />
                                            <asp:RadioButton ID="rbtnEmp" runat="server" Style="margin-left: 20px; margin-right: 20px;" AutoPostBack="true"
                                                Text="<%$ Resources:Attendance,Employee %>" GroupName="EmpGroup" Font-Bold="true"
                                                OnCheckedChanged="EmpGroup_CheckedChanged" />
                                            <br />
                                        </div>

                                        <div style="text-align: center;" class="col-md-6" id="Div_Terminated_Report" runat="server" visible="false">
                                            <asp:CheckBox ID="chkterminatedemployee" runat="server" Text="<%$ Resources:Attendance,Show terminated employee%>" AutoPostBack="true" OnCheckedChanged="chkterminatedemployee_CheckedChanged" />
                                        </div>

                                    </div>




                                    <div class="col-md-12">
                                        <asp:Panel ID="pnlGroup" runat="server">
                                            <br />
                                            <div class="col-md-6">
                                                <asp:ListBox ID="lbxGroup" runat="server" Style="width: 100%; max-height: 300px;" SelectionMode="Multiple"
                                                    AutoPostBack="true" OnSelectedIndexChanged="lbxGroup_SelectedIndexChanged"
                                                    Font-Names="Trebuchet MS" Font-Size="Small" ForeColor="Gray"></asp:ListBox>
                                                <br />
                                            </div>
                                            <div class="col-md-6" style="overflow: auto; max-height: 300px;">
                                                <asp:GridView CssClass="table-striped table-bordered table table-hover" ID="gvEmployee" runat="server" AllowPaging="True" AutoGenerateColumns="False"
                                                    OnPageIndexChanging="gvEmp1_PageIndexChanging"  Width="100%" PageSize="<%# PageControlCommon.GetPageSize() %>">
                                                    <Columns>
                                                        <asp:TemplateField HeaderText="<%$ Resources:Attendance,Employee Code %>">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblEmpCode" runat="server" Text='<%# Eval("Emp_Code") %>'></asp:Label>
                                                                <asp:Label ID="lblEmpId" Visible="false" runat="server" Text='<%# Eval("Emp_Id") %>'></asp:Label>
                                                            </ItemTemplate>
                                                            <ItemStyle  />
                                                        </asp:TemplateField>
                                                        <asp:BoundField DataField="Emp_Name" HeaderText="<%$ Resources:Attendance,Employee Name %>"
                                                            SortExpression="Emp_Name" ItemStyle-Width="20%" />
                                                        <asp:BoundField DataField="Emp_Name_L" HeaderText="<%$ Resources:Attendance,Employee Name Local%>"
                                                            SortExpression="Emp_Name_L" ItemStyle-Width="20%" />
                                                        <asp:TemplateField HeaderText="<%$ Resources:Attendance,Email Id %>">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblProductCode" runat="server" Text='<%# Eval("Email_Id") %>'></asp:Label>
                                                            </ItemTemplate>
                                                            <ItemStyle  />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="<%$ Resources:Attendance,Designation Name %>">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblItemType" runat="server" Text='<%# Eval("Designation") %>'></asp:Label>
                                                            </ItemTemplate>
                                                            <ItemStyle  />
                                                        </asp:TemplateField>
                                                    </Columns>
                                                    
                                                    
                                                    <PagerStyle CssClass="pagination-ys" />
                                                    
                                                </asp:GridView>
                                                <asp:Label ID="lblEmp" runat="server" Visible="false"></asp:Label>
                                                <br />
                                            </div>
                                        </asp:Panel>
                                        <div class="col-md-12">
                                            <br />
                                        </div>
                                    </div>
                                    <div class="col-md-12">
                                        <asp:Panel ID="Panel4" runat="server">
                                            <div class="col-md-6">
                                                <asp:Label ID="Label12" runat="server" Text="<%$ Resources:Attendance,Location %>"></asp:Label>
                                                <asp:DropDownList ID="ddlLocation" runat="server" CssClass="form-control" AutoPostBack="true"
                                                    OnSelectedIndexChanged="dpLocation_SelectedIndexChanged">
                                                </asp:DropDownList>
                                                <br />
                                            </div>
                                            <div class="col-md-6">
                                                <asp:Label ID="lblGroupByDept" runat="server" Text="<%$ Resources:Attendance,Department %>"></asp:Label>
                                                <div class="input-group">
                                                    <asp:DropDownList ID="dpDepartment" runat="server" CssClass="form-control" AutoPostBack="true"
                                                        OnSelectedIndexChanged="dpDepartment_SelectedIndexChanged">
                                                    </asp:DropDownList>
                                                    <div class="input-group-btn">
                                                        <asp:ImageButton ID="ImageButton1" runat="server" CausesValidation="False" Style="width: 37px;"
                                                            ImageUrl="~/Images/refresh.png" OnClick="btnAllRefresh_Click" ToolTip="<%$ Resources:Attendance,Refresh %>"></asp:ImageButton>
                                                    </div>
                                                </div>
                                                <br />
                                            </div>
                                            <div class="alert alert-info ">
                                                <div class="row">
                                                    <div class="form-group">
                                                        <div class="col-lg-3">
                                                            <asp:DropDownList ID="ddlField1" runat="server" CssClass="form-control">
                                                                <asp:ListItem Text="<%$ Resources:Attendance,Employee Code %>" Value="Emp_Code"></asp:ListItem>
                                                                <asp:ListItem Text="<%$ Resources:Attendance,Employee Name %>" Value="Emp_Name" Selected="True"></asp:ListItem>
                                                                <asp:ListItem Text="<%$ Resources:Attendance,Employee Name Local%>" Value="Emp_Name_L"></asp:ListItem>
                                                            </asp:DropDownList>
                                                        </div>
                                                        <div class="col-lg-2">
                                                            <asp:DropDownList ID="ddlOption1" runat="server" CssClass="form-control">
                                                                <asp:ListItem Text="<%$ Resources:Attendance,--Select-- %>"></asp:ListItem>
                                                                <asp:ListItem Text="<%$ Resources:Attendance,Equal %>"></asp:ListItem>
                                                                <asp:ListItem Selected="True" Text="<%$ Resources:Attendance,Contains %>"></asp:ListItem>
                                                                <asp:ListItem Text="<%$ Resources:Attendance,Like %>"></asp:ListItem>
                                                            </asp:DropDownList>
                                                        </div>
                                                        <div class="col-lg-2">
                                                            <asp:Panel ID="Panel1" runat="server" DefaultButton="imgBtnPayrollReportBind">
                                                                <asp:TextBox ID="txtValue1" runat="server" CssClass="form-control" />
                                                            </asp:Panel>
                                                        </div>
                                                        <div class="col-lg-3">
                                                            <asp:ImageButton ID="imgBtnPayrollReportBind" runat="server" CausesValidation="False" Style="margin-top: -5px; width: 43px;"
                                                                ImageUrl="~/Images/search.png" OnClick="btnPayrollReportbind_Click"
                                                                ToolTip="<%$ Resources:Attendance,Search %>" />
                                                            <asp:ImageButton ID="ImageButton2" runat="server" CausesValidation="False" Style="width: 37px;"
                                                                ImageUrl="~/Images/refresh.png" OnClick="btnPayrollReportRefresh_Click"
                                                                ToolTip="<%$ Resources:Attendance,Refresh %>"></asp:ImageButton>
                                                            <asp:ImageButton ID="ImageButton6" runat="server" OnClick="ImgbtnSelectAll_ClickPayrollReport" Style="width: 37px;"
                                                                ToolTip="<%$ Resources:Attendance, Select All %>" AutoPostBack="true" ImageUrl="~/Images/selectAll.png" />
                                                        </div>
                                                        <div class="col-lg-2">
                                                            <h5>
                                                                <asp:Label ID="lblTotalRecordsPayrollReport" runat="server" Text="<%$ Resources:Attendance,Total Records: 0 %>"></asp:Label></h5>
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
                                                                <asp:Label ID="lblSelectRecd" runat="server" Visible="false"></asp:Label>
                                                                <asp:GridView CssClass="table-striped table-bordered table table-hover" ID="gvEmpPayrollReport" runat="server" AllowPaging="True" AutoGenerateColumns="False"
                                                                    OnPageIndexChanging="gvEmpPayrollReport_PageIndexChanging"  Width="100%"
                                                                    OnSorting="GridViewEmpPayrollReport_Sorting" AllowSorting="true" DataKeyNames="Emp_Id"
                                                                    PageSize="<%# PageControlCommon.GetPageSize() %>" Visible="false">
                                                                    <Columns>
                                                                        <asp:TemplateField>
                                                                            <ItemTemplate>
                                                                                <asp:CheckBox ID="chkgvSelect" runat="server" />
                                                                            </ItemTemplate>
                                                                            <HeaderTemplate>
                                                                                <asp:CheckBox ID="chkgvSelectAll" runat="server" OnCheckedChanged="chkgvSelectAll_CheckedChangedPayrollReport"
                                                                                    AutoPostBack="true" />
                                                                            </HeaderTemplate>
                                                                            <ItemStyle HorizontalAlign="Center" />
                                                                        </asp:TemplateField>
                                                                        <asp:TemplateField HeaderText="<%$ Resources:Attendance,Employee Code %>" SortExpression="Emp_Code">
                                                                            <ItemTemplate>
                                                                                <asp:Label ID="lblEmpCode" runat="server" Text='<%# Eval("Emp_Code") %>'></asp:Label>
                                                                                <asp:Label ID="lblEmpId" Visible="false" runat="server" Text='<%# Eval("Emp_Id") %>'></asp:Label>
                                                                            </ItemTemplate>
                                                                            <ItemStyle  />
                                                                        </asp:TemplateField>
                                                                        <asp:BoundField DataField="Emp_Name" HeaderText="<%$ Resources:Attendance,Employee Name %>"
                                                                            SortExpression="Emp_Name" ItemStyle-Width="20%" />
                                                                        <asp:BoundField DataField="Emp_Name_L" HeaderText="<%$ Resources:Attendance,Employee Name Local%>"
                                                                            SortExpression="Emp_Name_L" ItemStyle-Width="20%" />
                                                                        <asp:TemplateField HeaderText="<%$ Resources:Attendance,Date of Joining %>" HeaderStyle-HorizontalAlign="Center"
                                                                            SortExpression="Doj">
                                                                            <ItemTemplate>
                                                                                <asp:Label ID="lblDoj" runat="server" Text='<%# DateFormat(Eval("Doj").ToString()) %>'></asp:Label>
                                                                            </ItemTemplate>
                                                                            <ItemStyle  HorizontalAlign="Center" />
                                                                        </asp:TemplateField>

                                                                         <asp:TemplateField HeaderText="<%$ Resources:Attendance,Department %>" HeaderStyle-HorizontalAlign="Center"
                                                                            SortExpression="Department">
                                                                            <ItemTemplate>
                                                                                <asp:Label ID="lbldepartment" runat="server" Text='<%#Eval("Department") %>'></asp:Label>
                                                                            </ItemTemplate>
                                                                            <ItemStyle  HorizontalAlign="Center" />
                                                                        </asp:TemplateField>
                                                                        <asp:TemplateField HeaderText="<%$ Resources:Attendance,Email Id %>">
                                                                            <ItemTemplate>
                                                                                <asp:Label ID="lblProductCode" runat="server" Text='<%# Eval("Email_Id") %>'></asp:Label>
                                                                            </ItemTemplate>
                                                                            <ItemStyle  />
                                                                        </asp:TemplateField>
                                                                        <asp:TemplateField HeaderText="<%$ Resources:Attendance,Phone No. %>">
                                                                            <ItemTemplate>
                                                                                <asp:Label ID="lblItemType" runat="server" Text='<%# Eval("Phone_No") %>'></asp:Label>
                                                                            </ItemTemplate>
                                                                            <ItemStyle  />
                                                                        </asp:TemplateField>
                                                                    </Columns>
                                                                    
                                                                    
                                                                    <PagerStyle CssClass="pagination-ys" />
                                                                    
                                                                </asp:GridView>
                                                                <asp:HiddenField ID="hdFSortgvEmpPayrollReport" runat="server" />
                                                            </div>
                                                        </div>

                                                    </div>
                                                </div>
                                            </div>
                                        </asp:Panel>
                                    </div>
                                    <div class="col-md-12">
                                        <asp:Panel ID="PanelInsertClaim" runat="server">
                                            <div class="col-md-6">
                                                <asp:Label ID="lblMonth" runat="server" Text="<%$ Resources:Attendance,Month %>"></asp:Label>
                                                <asp:DropDownList ID="ddlMonth" runat="server" CssClass="form-control">
                                                    <asp:ListItem Text="<%$ Resources:Attendance,- - select - - %>" Value="0" />
                                                    <asp:ListItem Text="<%$ Resources:Attendance,January %>" Value="1" />
                                                    <asp:ListItem Text="<%$ Resources:Attendance,February %>" Value="2" />
                                                    <asp:ListItem Text="<%$ Resources:Attendance,March %>" Value="3" />
                                                    <asp:ListItem Text="<%$ Resources:Attendance,April %>" Value="4" />
                                                    <asp:ListItem Text="<%$ Resources:Attendance,May %>" Value="5" />
                                                    <asp:ListItem Text="<%$ Resources:Attendance,June %>" Value="6" />
                                                    <asp:ListItem Text="<%$ Resources:Attendance,July %>" Value="7" />
                                                    <asp:ListItem Text="<%$ Resources:Attendance,August %>" Value="8" />
                                                    <asp:ListItem Text="<%$ Resources:Attendance,September %>" Value="9" />
                                                    <asp:ListItem Text="<%$ Resources:Attendance,October %>" Value="10" />
                                                    <asp:ListItem Text="<%$ Resources:Attendance,November %>" Value="11" />
                                                    <asp:ListItem Text="<%$ Resources:Attendance,December %>" Value="12" />
                                                </asp:DropDownList>
                                                <br />
                                            </div>
                                            <div class="col-md-6">
                                                <asp:Label ID="LblYear" runat="server" Text="<%$ Resources:Attendance,Year %>"></asp:Label>
                                                <asp:TextBox ID="TxtYear" runat="server" CssClass="form-control"></asp:TextBox>
                                                <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender3" runat="server" Enabled="True"
                                                    TargetControlID="TxtYear" FilterType="Numbers">
                                                </cc1:FilteredTextBoxExtender>
                                                <br />
                                            </div>
                                            <div runat="server" visible="false" class="col-md-6">
                                                <asp:Label ID="Label1" runat="server" Text="<%$ Resources:Attendance,Claim %>"></asp:Label>
                                                <asp:DropDownList ID="ddlClaimType" runat="server" CssClass="form-control">
                                                    <asp:ListItem Text="--Select--" Value="0" />
                                                    <asp:ListItem Text="Approved" Value="1" />
                                                    <asp:ListItem Text="Cancelled" Value="2" />
                                                    <asp:ListItem Text="Pending" Value="3" />
                                                </asp:DropDownList>
                                                <br />
                                            </div>
                                            <div runat="server" visible="false" class="col-md-6">
                                                <asp:Label ID="Label2" runat="server" Text="<%$ Resources:Attendance,Loan %>"></asp:Label>
                                                <asp:DropDownList ID="Ddlloantype" runat="server" CssClass="form-control">
                                                    <asp:ListItem Text="--Select--" Value="0" />
                                                    <asp:ListItem Text="Loan Detail Report" Value="1" />
                                                    <asp:ListItem Text="Loan Installment Report" Value="2" />
                                                </asp:DropDownList>
                                                <br />
                                            </div>
                                            <div runat="server" visible="false" class="col-md-6">
                                                <asp:RadioButton ID="rbtnClaimType" runat="server" Text="Claim Report" GroupName="aa" />
                                                <asp:RadioButton ID="rbtnLoan" runat="server" Text="Loan Report" GroupName="aa" />
                                                <asp:RadioButton ID="RbtnloanDetail" runat="server" Text="LoanDetail Report" GroupName="aa" />
                                                <asp:RadioButton ID="rbtnPenalty" runat="server" Text="Penalty Report" GroupName="aa" />
                                                <asp:RadioButton ID="RbtnDirectory" runat="server" Text="Employee Directory Report"
                                                    GroupName="aa" />
                                                <br />
                                            </div>
                                            <div runat="server" visible="false" class="col-md-6">
                                                <asp:RadioButton ID="Rbtndocument" runat="server" Text="Document Expiry Report" GroupName="aa" />
                                                <asp:Button ID="btnSave" runat="server" CssClass="btn btn-primary" Text="<%$ Resources:Attendance,Next %>"
                                                    ValidationGroup="PayrollReportsave" OnClick="btnSave_Click" />
                                                <asp:Button ID="btnReset" runat="server" CssClass="btn btn-primary" Text="<%$ Resources:Attendance,Reset %>"
                                                    ValidationGroup="PayrollReportsave" OnClick="btnReset_Click" />
                                                <br />
                                            </div>
                                            <div class="col-md-12" style="text-align: center">
                                                

                                                <asp:Button ID="Button2" runat="server" CssClass="btn btn-primary" Text="<%$ Resources:Attendance,Reset %>"
                                                    ValidationGroup="PayrollReportsave" OnClick="btnReset_Click" />

                                                <asp:Button ID="Button1" runat="server" CssClass="btn btn-primary" Text="<%$ Resources:Attendance,Next %>"
                                                    ValidationGroup="PayrollReportsave" OnClick="btnSave_Click" />
                                                <br />
                                            </div>
                                        </asp:Panel>
                                    </div>
                                </asp:Panel>
                                <div id="PanelReport" runat="server">
                                    <div class="row">
                                        <div class="col-md-12">
                                            <div style="text-align: center;" class="form-control">
                                                <asp:LinkButton ID="LinkButton18" OnClick="ChangeFilter_OnClick" Text="<%$ Resources:Attendance,Change Filter Criteria%>"
                                                    runat="server"></asp:LinkButton>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="row">
                                        <div class="col-md-12">
                                            <div class="box box-warning box-solid">
                                                <div class="box-header with-border">
                                                    <h3 class="box-title">Before Post Payroll</h3>
                                                    <div class="box-tools pull-right">
                                                        <button type="button" class="btn btn-box-tool" data-widget="collapse">
                                                            <i class="fa fa-minus"></i>
                                                        </button>
                                                    </div>
                                                </div>
                                                <div class="box-body">

                                                    <div class="row">
                                                        <div class="col-md-4">
                                                            <div class="box box-primary">
                                                                <div class="box-header with-border">
                                                                    <h3 class="box-title">Claim & Penalty</h3>
                                                                    <div class="box-tools pull-right">
                                                                        <button type="button" class="btn btn-box-tool" data-widget="collapse">
                                                                            <i class="fa fa-minus"></i>
                                                                        </button>
                                                                    </div>
                                                                </div>
                                                                <div class="box-body">
                                                                    <asp:LinkButton ID="lnkClaimReport" Visible="false" OnClick="lnkClaim_OnClick"
                                                                        Text="<%$ Resources:Attendance,Claim Report%>" runat="server"></asp:LinkButton>
                                                                    <br />

                                                                    <asp:LinkButton ID="lnkPenaltyReport" Visible="false" OnClick="lnkPenalty_OnClick"
                                                                        Text="<%$  Resources:Attendance,Penalty Report%>" runat="server"></asp:LinkButton>
                                                                    <br />
                                                                </div>
                                                            </div>
                                                        </div>

                                                        <div class="col-md-4">
                                                            <div class="box box-primary">
                                                                <div class="box-header with-border">
                                                                    <h3 class="box-title">Loan</h3>
                                                                    <div class="box-tools pull-right">
                                                                        <button type="button" class="btn btn-box-tool" data-widget="collapse">
                                                                            <i class="fa fa-minus"></i>
                                                                        </button>
                                                                    </div>
                                                                </div>
                                                                <div class="box-body">

                                                                    <div style="margin-right: 10%;">
                                                                        <asp:LinkButton ID="lnkLOanApprovedReport" Visible="false" OnClick="lnkLoanApproval_OnClick"
                                                                            Text="<%$ Resources:Attendance,Loan Approval Report %>" runat="server"></asp:LinkButton>
                                                                        <br />
                                                                        <asp:LinkButton ID="lnkLOanSummaryReport" Visible="false" OnClick="lnkloanSummary_OnClick"
                                                                            Text="<%$ Resources:Attendance,Loan Summary Report%>" runat="server"></asp:LinkButton>
                                                                        <br />
                                                                        <asp:LinkButton ID="lnkLOanInstallmentReport" Visible="false" OnClick="lnkLoanInstallment_OnClick"
                                                                            Text="<%$ Resources:Attendance,Loan Installment Report%>" runat="server"></asp:LinkButton>
                                                                        <br />
                                                                    </div>




                                                                </div>
                                                            </div>
                                                        </div>

                                                        <div class="col-md-4">
                                                            <div class="box box-primary">
                                                                <div class="box-header with-border">
                                                                    <h3 class="box-title">Settlement & Adjustment</h3>
                                                                    <div class="box-tools pull-right">
                                                                        <button type="button" class="btn btn-box-tool" data-widget="collapse">
                                                                            <i class="fa fa-minus"></i>
                                                                        </button>
                                                                    </div>
                                                                </div>
                                                                <div class="box-body">

                                                                    <div style="margin-right: 10%;">
                                                                        <asp:LinkButton ID="lnksettlementReport" Visible="false" OnClick="lnkSettlement_OnClick"
                                                                            Text="<%$ Resources:Attendance,Settlement Report%>" runat="server"></asp:LinkButton>
                                                                        <br />
                                                                        <asp:LinkButton ID="lnknonadjustmentReport" Visible="false" OnClick="lnkNonAdjustment_OnClick"
                                                                            Text="<%$  Resources:Attendance,Non Adjustment Report%>" runat="server"></asp:LinkButton>
                                                                        <br />
                                                                    </div>




                                                                </div>
                                                            </div>
                                                        </div>
                                                    </div>
                                                    <div class="row">
                                                        <div class="col-md-4">
                                                            <div class="box box-primary">
                                                                <div class="box-header with-border">
                                                                    <h3 class="box-title">PF & ESIC</h3>
                                                                    <div class="box-tools pull-right">
                                                                        <button type="button" class="btn btn-box-tool" data-widget="collapse">
                                                                            <i class="fa fa-minus"></i>
                                                                        </button>
                                                                    </div>
                                                                </div>
                                                                <div class="box-body">
                                                                    <asp:LinkButton ID="lnkPFReport" Visible="false" OnClick="lnkPF_OnClick"
                                                                        Text="<%$ Resources:Attendance,PF Report%>" runat="server"></asp:LinkButton>
                                                                    <br />
                                                                    <asp:LinkButton ID="lnkESICReport" Visible="false" OnClick="lnkESIC_OnClick"
                                                                        Text="<%$  Resources:Attendance,ESIC Report%>" runat="server"></asp:LinkButton>
                                                                    <br />
                                                                </div>
                                                            </div>
                                                        </div>

                                                        <div class="col-md-4">
                                                            <div class="box box-primary">
                                                                <div class="box-header with-border">
                                                                    <h3 class="box-title">Document</h3>
                                                                    <div class="box-tools pull-right">
                                                                        <button type="button" class="btn btn-box-tool" data-widget="collapse">
                                                                            <i class="fa fa-minus"></i>
                                                                        </button>
                                                                    </div>
                                                                </div>
                                                                <div class="box-body">
                                                                    <asp:LinkButton ID="lnkDirectoryReport" Visible="false" OnClick="lnkDirectory_OnClick"
                                                                        Text="<%$ Resources:Attendance,Employee Directory Report %>" runat="server"></asp:LinkButton>
                                                                    <br />
                                                                    <asp:LinkButton ID="lnkDocExpiryReport" Visible="false" OnClick="lnkDocExpiry_OnClick"
                                                                        Text="<%$ Resources:Attendance,Document Expiry Report%>" runat="server"></asp:LinkButton>
                                                                    <br />
                                                                </div>
                                                            </div>
                                                        </div>

                                                        <div class="col-md-4">
                                                            <div class="box box-primary">
                                                                <div class="box-header with-border">
                                                                    <h3 class="box-title">Salary</h3>
                                                                    <div class="box-tools pull-right">
                                                                        <button type="button" class="btn btn-box-tool" data-widget="collapse">
                                                                            <i class="fa fa-minus"></i>
                                                                        </button>
                                                                    </div>
                                                                </div>
                                                                <div class="box-body">
                                                                    <asp:LinkButton ID="LnkPaySummaryBeforepost" Visible="false" OnClick="LnkPaySummaryBeforepost_OnClick"
                                                                        Text="<%$  Resources:Attendance,Payroll Summary Report%>" runat="server"></asp:LinkButton>
                                                                    <br />
                                                                    <asp:LinkButton ID="Lnk_Projected_Salary_Transfer" Visible="false" OnClick="Lnk_Projected_Salary_Transfer_Click"
                                                                        Text="<%$  Resources:Attendance,Projected Salary Transfer Report%>" runat="server"></asp:LinkButton>
                                                                    <br />
                                                                </div>
                                                            </div>
                                                        </div>
                                                    </div>
                                                    <div class="row" style="display: none;" runat="server" visible="false">
                                                        <div class="col-md-4">
                                                            <div class="box box-primary">
                                                                <div class="box-header with-border">
                                                                    <h3 class="box-title">Leave</h3>
                                                                    <div class="box-tools pull-right">
                                                                        <button type="button" class="btn btn-box-tool" data-widget="collapse">
                                                                            <i class="fa fa-minus"></i>
                                                                        </button>
                                                                    </div>
                                                                </div>
                                                                <div class="box-body">
                                                                    <asp:LinkButton ID="linkLeaveReport" Visible="true" OnClick="linkLeaveReport_OnClick"
                                                                        Text="<%$ Resources:Attendance,Leave Report%>" runat="server"></asp:LinkButton>
                                                                    <br />
                                                                    <asp:LinkButton ID="linkStatus" Visible="true" OnClick="linkStatus_OnClick"
                                                                        Text="<%$  Resources:Attendance,Leave Status Report%>" runat="server"></asp:LinkButton>
                                                                    <br />
                                                                    <asp:LinkButton ID="linkLeaveRemaninig" Visible="true" OnClick="linkLeaveRemaninig_OnClick"
                                                                        Text="<%$  Resources:Attendance,Leave Remaining Report%>" runat="server"></asp:LinkButton>
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
                                            <div class="box box-warning box-solid">
                                                <div class="box-header with-border">
                                                    <h3 class="box-title">After Post Payroll</h3>
                                                    <div class="box-tools pull-right">
                                                        <button type="button" class="btn btn-box-tool" data-widget="collapse">
                                                            <i class="fa fa-minus"></i>
                                                        </button>
                                                    </div>
                                                </div>
                                                <div class="box-body">

                                                    <div class="row">
                                                        <div class="col-md-4">
                                                            <div class="box box-primary">
                                                                <div class="box-header with-border">
                                                                    <h3 class="box-title">Allowance</h3>
                                                                    <div class="box-tools pull-right">
                                                                        <button type="button" class="btn btn-box-tool" data-widget="collapse">
                                                                            <i class="fa fa-minus"></i>
                                                                        </button>
                                                                    </div>
                                                                </div>
                                                                <div class="box-body">
                                                                    <asp:LinkButton ID="lnkAllowanceReport" Visible="false" OnClick="lnkAllowance_OnClick"
                                                                        Text="<%$ Resources:Attendance,Allowance Report%>" runat="server"></asp:LinkButton>
                                                                    <br />
                                                                    <asp:LinkButton ID="LinkButton20" Visible="false" OnClick="lnkAllowanceDetail_OnClick"
                                                                        Text="<%$ Resources:Attendance,Allowance Group Report%>" runat="server"></asp:LinkButton>
                                                                    <br />
                                                                </div>
                                                            </div>
                                                        </div>

                                                        <div class="col-md-4">
                                                            <div class="box box-primary">
                                                                <div class="box-header with-border">
                                                                    <h3 class="box-title">Deduction</h3>
                                                                    <div class="box-tools pull-right">
                                                                        <button type="button" class="btn btn-box-tool" data-widget="collapse">
                                                                            <i class="fa fa-minus"></i>
                                                                        </button>
                                                                    </div>
                                                                </div>
                                                                <div class="box-body">
                                                                    <asp:LinkButton ID="lnkDeductionReport" Visible="false" OnClick="lnkDeduction_OnClick"
                                                                        Text="<%$ Resources:Attendance,Deduction Report%>" runat="server"></asp:LinkButton>
                                                                    <br />
                                                                    <asp:LinkButton ID="LinkButton23" Visible="false" OnClick="lnkDeductionDetail_OnClick"
                                                                        Text="<%$  Resources:Attendance,Deduction Group Report%>" runat="server"></asp:LinkButton>
                                                                    <br />
                                                                </div>
                                                            </div>
                                                        </div>

                                                        <div class="col-md-4">
                                                            <div class="box box-primary">
                                                                <div class="box-header with-border">
                                                                    <h3 class="box-title">Salary</h3>
                                                                    <div class="box-tools pull-right">
                                                                        <button type="button" class="btn btn-box-tool" data-widget="collapse">
                                                                            <i class="fa fa-minus"></i>
                                                                        </button>
                                                                    </div>
                                                                </div>
                                                                <div class="box-body">
                                                                    <asp:LinkButton ID="Lnk_Pay_Slip" Visible="false" OnClick="Lnk_Pay_Slip_Click"
                                                                        Text="<%$ Resources:Attendance,Pay Slip%>" runat="server"></asp:LinkButton>
                                                                    <br />

                                                                    <asp:LinkButton ID="lnkPayslipReport" Visible="false" OnClick="lnkPayslip_OnClick"
                                                                        Text="<%$ Resources:Attendance,Pay Slip Report%>" runat="server"></asp:LinkButton>
                                                                    <br />
                                                                    <asp:LinkButton ID="lnkTotalSalaryReport" Visible="false" OnClick="lnkTotalSalary_OnClick"
                                                                        Text="<%$  Resources:Attendance,Total Salary Report%>" runat="server"></asp:LinkButton>
                                                                    <br />
                                                                    <asp:LinkButton ID="lnkPayrollsummaryReport" Visible="false" OnClick="lnkPayrollSummary_OnClick"
                                                                        Text="<%$  Resources:Attendance,Payroll Summary Report%>" runat="server"></asp:LinkButton>
                                                                    <br />
                                                                    <asp:LinkButton ID="lnkSummarySalaryStatement" Visible="false" OnClick="lnkSummarySalaryStatement_OnClick"
                                                                        Text="Summary Salary Statement" runat="server"></asp:LinkButton>
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
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>

    <asp:UpdateProgress ID="UpdateProgress2" runat="server" AssociatedUpdatePanelID="Update_New">
        <ProgressTemplate>
            <div class="modal_Progress">
                <div class="center_Progress">
                    <img alt="" src="../Images/ajax-loader2.gif" style="vertical-align: middle; background-color: White; border-radius: 10px;" />
                </div>
            </div>
        </ProgressTemplate>
    </asp:UpdateProgress>
    <div style="display: none;">
        <asp:Panel ID="pnlPayrollReport" runat="server"></asp:Panel>
    </div>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="FooterContent" runat="Server">
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="AfterLoadScriptContent" runat="Server"><script src="../Script/common.js"></script>
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
