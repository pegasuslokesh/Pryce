<%@ Page Title="" Language="C#" MasterPageFile="~/ERPMaster.master" AutoEventWireup="true" CodeFile="RollBackPostedLog.aspx.cs" Inherits="Attendance_RollBackPostedLog" %>


<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="Head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="HeaderContent" runat="Server">
    <h1>
        <i class="fa fa-reply"></i>&nbsp;&nbsp;
        <asp:Label ID="lblHeader" runat="server" Text="<%$ Resources:Attendance,RollBack LogPost%>"></asp:Label>
    </h1>
    <ol class="breadcrumb">
        <li><a href="#"><i class="fa fa-dashboard"></i>
            <asp:Label ID="Label6" runat="server" Text="<%$ Resources:Attendance,Attendance Module%>"></asp:Label></a></li>
        <li><a href="#"><i class="fa fa-dashboard"></i>
            <asp:Label ID="Label10" runat="server" Text="<%$ Resources:Attendance,Attendance Module%>"></asp:Label></a></li>
        <li class="active">
            <asp:Label ID="Label5" runat="server" Text="<%$ Resources:Attendance,RollBack LogPost%>"></asp:Label></li>
    </ol>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="Server">
    <asp:UpdatePanel ID="Update_Page" runat="server">
        <ContentTemplate>
            <div class="row">
                <div class="col-md-12">
                    <div class="box box-primary">
                        <div class="box-body">
                            <div class="form-group">
                                <div style="display: none">
                                    <asp:RadioButton ID="RbtnListView" Visible="false" Checked="true" Font-Bold="true"
                                        runat="server" Text="<%$ Resources:Attendance,List View %>"
                                        GroupName="View" />
                                    <asp:RadioButton ID="RbtnFormView" Visible="false" runat="server" Text="<%$ Resources:Attendance,Form View %>"
                                        Font-Bold="true" GroupName="View" />
                                </div>
                                <div class="col-md-6">
                                    <asp:Label ID="Label33" runat="server" Text="<%$ Resources:Attendance,Location %>"></asp:Label>
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
                                            <asp:LinkButton ID="ImageButton1" runat="server" CausesValidation="False" OnClick="btnAllRefresh_Click" ToolTip="<%$ Resources:Attendance,Refresh %>"><span class="fa fa-repeat"  style="font-size:25px;margin-left:5px"></span></asp:LinkButton>
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
                    <div id="Div1" runat="server" class="box box-info collapsed-box">
                        <div class="box-header with-border">
                            <h3 class="box-title">
                                <asp:Label ID="Label22" runat="server" Text="<%$ Resources:Attendance,Advance Search%>"></asp:Label></h3>
                            &nbsp;&nbsp;|&nbsp;&nbsp;
					<asp:Label ID="lblTotalRecordsLeave" runat="server" Font-Bold="true" Text="<%$ Resources:Attendance,Total Records: 0 %>"></asp:Label>

                            <div class="box-tools pull-right">
                                <button type="button" class="btn btn-box-tool" data-widget="collapse">
                                    <i id="I1" runat="server" class="fa fa-plus"></i>
                                </button>
                            </div>
                        </div>
                        <div class="box-body">
                            <div class="col-lg-3">
                                <asp:DropDownList ID="ddlField1" runat="server" CssClass="form-control">
                                    <asp:ListItem Text="<%$ Resources:Attendance,Employee Code %>" Value="Emp_Code"></asp:ListItem>
                                    <asp:ListItem Text="<%$ Resources:Attendance,Employee Name %>" Value="Emp_Name" Selected="True"></asp:ListItem>
                                    <asp:ListItem Text="<%$ Resources:Attendance,Employee Name Local %>" Value="Emp_Name_L"></asp:ListItem>
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
                            <div class="col-lg-5">
                                <asp:Panel ID="Panel1" runat="server" DefaultButton="imgBtnLeaveBind">
                                    <asp:TextBox placeholder="Search from Content"   ID="txtValue1" runat="server" CssClass="form-control" ToolTip="<%$ Resources:Attendance,Search %>" />
                                </asp:Panel>
                            </div>
                            <div class="col-lg-2">
                                <asp:LinkButton  ID="imgBtnLeaveBind" runat="server" CausesValidation="False" OnClick="btnLeavebind_Click" ToolTip="<%$ Resources:Attendance,Search %>"><span class="fa fa-search"  style="font-size:25px;"></span></asp:LinkButton>&nbsp;&nbsp;&nbsp;
                                <asp:LinkButton ID="ImageButton2" runat="server" CausesValidation="False" OnClick="btnLeaveRefresh_Click" ToolTip="<%$ Resources:Attendance,Refresh %>"><span class="fa fa-repeat"  style="font-size:25px;"></span></asp:LinkButton>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
            <div class="box box-warning box-solid" <%= gvEmpLeave.Rows.Count>0?"style='display:block'":"style='display:none'"%> >
                <div class="box-body">
                    <div class="row">
                        <div class="col-md-12">
                            <div class="flow">
                                <asp:Label ID="lblSelectRecd" runat="server" Visible="false"></asp:Label>
                                <asp:GridView CssClass="table-striped table-bordered table table-hover" ID="gvEmpLeave" runat="server" AllowPaging="True" AutoGenerateColumns="False"
                                    OnPageIndexChanging="gvEmpLeave_PageIndexChanging" Width="100%"
                                    DataKeyNames="Emp_Id" PageSize="<%# PageControlCommon.GetPageSize() %>">
                                    <Columns>
                                        <asp:TemplateField>
                                            <ItemTemplate>
                                                <asp:CheckBox ID="chkgvSelect" runat="server" AutoPostBack="true" OnCheckedChanged="chkgvSelect_CheckedChangedLeave" />
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Center" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ Resources:Attendance,Employee Code %>">
                                            <ItemTemplate>
                                                <asp:Label ID="lblEmpCode" runat="server" Text='<%# Eval("Emp_Code") %>'></asp:Label>
                                                <asp:Label ID="lblEmpId" Visible="false" runat="server" Text='<%# Eval("Emp_Id") %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle />
                                        </asp:TemplateField>
                                        <asp:BoundField DataField="Emp_Name" HeaderText="<%$ Resources:Attendance,Employee Name %>"
                                            SortExpression="Emp_Name" ItemStyle-Width="20%">
                                            <ItemStyle Width="20%" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="Emp_Name_L" HeaderText="<%$ Resources:Attendance,Employee Name Local%>"
                                            SortExpression="Emp_Name_L" ItemStyle-Width="20%">
                                            <ItemStyle Width="20%" />
                                        </asp:BoundField>
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
            <div class="row">
                <div class="col-md-12">
                    <div class="box box-primary">
                        <div class="box-body">
                            <div class="form-group">
                                <div class="col-md-6">
                                    <asp:Label ID="lblmonth" runat="server" Font-Size="14px" Font-Bold="true"
                                        Text="<%$ Resources:Attendance,Month %>"></asp:Label>
                                    <asp:DropDownList ID="ddlMonth" Enabled="false" runat="server" CssClass="form-control">
                                        <asp:ListItem Text="--Select--" Value="0" />
                                        <asp:ListItem Text="January" Value="1" />
                                        <asp:ListItem Text="February" Value="2" />
                                        <asp:ListItem Text="March" Value="3" />
                                        <asp:ListItem Text="April" Value="4" />
                                        <asp:ListItem Text="May" Value="5" />
                                        <asp:ListItem Text="June" Value="6" />
                                        <asp:ListItem Text="July" Value="7" />
                                        <asp:ListItem Text="August" Value="8" />
                                        <asp:ListItem Text="September" Value="9" />
                                        <asp:ListItem Text="October" Value="10" />
                                        <asp:ListItem Text="November" Value="11" />
                                        <asp:ListItem Text="December" Value="12" />
                                    </asp:DropDownList>
                                    <br />
                                </div>
                                <div class="col-md-6">
                                    <asp:Label ID="lblyeay" runat="server" Font-Size="14px" Font-Bold="true"
                                        Text="<%$ Resources:Attendance,Year %>"></asp:Label>
                                    <asp:TextBox ID="TxtYear" Enabled="false" runat="server" CssClass="form-control"></asp:TextBox>
                                    <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender3" runat="server" Enabled="True"
                                        TargetControlID="TxtYear" ValidChars="1,2,3,4,5,6,7,8,9,0,.">
                                    </cc1:FilteredTextBoxExtender>
                                </div>
                                <div class="col-md-12" style="text-align: center">
                                    <asp:Button ID="btnGenratePayroll" runat="server" CssClass="btn btn-primary"
                                        Text="<%$ Resources:Attendance,RollBack LogPost %>" OnClick="BtnRollBackPayroll_Click" />
                                    <cc1:ConfirmButtonExtender ID="ConfirmButtonExtender1" runat="server" TargetControlID="btnGenratePayroll"
                                        ConfirmText="Are You Sure to RollBack LogPost?">
                                    </cc1:ConfirmButtonExtender>
                                    <asp:Button ID="rollbackPostedPayroll" runat="server" CssClass="btn btn-primary"
                                        Text="<%$ Resources:Attendance,RollBack Payroll %>" OnClick="rollbackPostedPayroll_Click" />
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
    <asp:UpdateProgress ID="UpdateProgress1" runat="server" AssociatedUpdatePanelID="Update_Page">
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

</asp:Content>
