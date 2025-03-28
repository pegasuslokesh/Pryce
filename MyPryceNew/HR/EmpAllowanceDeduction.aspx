<%@ Page Title="" Language="C#" MasterPageFile="~/ERPMaster.master" AutoEventWireup="true"
    CodeFile="EmpAllowanceDeduction.aspx.cs" Inherits="HR_EmpAllowanceDeduction" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="HeaderContent" runat="Server">
    <h1>
       <i class="fas fa-cut"></i>
        <asp:Label ID="lblHeader" runat="server" Text="<%$ Resources:Attendance,Employee Allowance Deduction Setup%>"></asp:Label>
    </h1>
    <ol class="breadcrumb">
        <asp:HiddenField runat="server" ID="hdnCanEdit" />
        <li><a href="#"><i class="fa fa-dashboard"></i>
            <asp:Label ID="Label6" runat="server" Text="<%$ Resources:Attendance,HR%>"></asp:Label></a></li>
        <li><a href="#"><i class="fa fa-dashboard"></i>
            <asp:Label ID="Label10" runat="server" Text="<%$ Resources:Attendance,Payroll%>"></asp:Label></a></li>
        <li class="active">
            <asp:Label ID="Label5" runat="server" Text="<%$ Resources:Attendance,Employee Allowance Deduction%>"></asp:Label></li>
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
            <div class="row">
                <div class="col-md-12">
                    <div style="text-align: center;" class="form-control">
                        <asp:RadioButton ID="rbtnGroup" OnCheckedChanged="EmpGroup_CheckedChanged" runat="server"
                            Text="<%$ Resources:Attendance,Group %>" GroupName="EmpGroup" AutoPostBack="true" />
                        <asp:RadioButton ID="rbtnEmp" Style="margin-left: 25px;" runat="server" AutoPostBack="true"
                            Text="<%$ Resources:Attendance,Employee %>" GroupName="EmpGroup" OnCheckedChanged="EmpGroup_CheckedChanged" />
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

                                <asp:ListBox ID="lbxGroup" runat="server" Height="211px" SelectionMode="Multiple"
                                    AutoPostBack="true" OnSelectedIndexChanged="lbxGroup_SelectedIndexChanged" CssClass="form-control"></asp:ListBox>
                            </div>
                            <div class="col-md-8">
                                <div class="flow">
                                    <asp:Label ID="lblEmp" runat="server" Visible="false"></asp:Label>

                                    <asp:GridView CssClass="table-striped table-bordered table table-hover" ID="gvEmployee" runat="server" AllowPaging="True" AutoGenerateColumns="False"
                                        OnPageIndexChanging="gvEmp1_PageIndexChanging" Width="100%" PageSize='<%# int.Parse(Session["GridSize"].ToString()) %>'>
                                        <Columns>
                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,Employee Code %>">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblEmpCode" runat="server" Text='<%# Eval("Emp_Code") %>'></asp:Label>
                                                    <asp:Label ID="lblEmpId" Visible="false" runat="server" Text='<%# Eval("Emp_Id") %>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle />
                                            </asp:TemplateField>
                                            <asp:BoundField DataField="Emp_Name" HeaderText="<%$ Resources:Attendance,Employee Name %>"
                                                SortExpression="Emp_Name" ItemStyle-Width="20%" />
                                            <asp:BoundField DataField="Emp_Name_L" HeaderText="<%$ Resources:Attendance,Employee Name Local%>"
                                                SortExpression="Emp_Name_L" ItemStyle-Width="20%" />
                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance, Date Of Joining %>">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblDoj" runat="server" Text='<%# DateFormat(Eval("Doj").ToString()) %>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle />
                                            </asp:TemplateField>
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
            <div id="Div_Employee" runat="server" class="row">
                <div class="col-md-12">
                    <div class="box box-warning">
                        <div class="box-header with-border">
                            <h3 class="box-title">Employee</h3>
                            <div class="box-tools pull-right">
                                <button type="button" class="btn btn-box-tool" data-widget="collapse">
                                    <i class="fa fa-minus"></i>
                                </button>
                            </div>
                        </div>
                        <div class="box-body">
                            <div class="col-md-6">
                                <asp:Label ID="Label1" runat="server" Text="<%$ Resources:Attendance,Location %>"></asp:Label>
                                <asp:DropDownList ID="ddlLocation" runat="server" CssClass="form-control" AutoPostBack="true"
                                    OnSelectedIndexChanged="dpLocation_SelectedIndexChanged">
                                </asp:DropDownList>
                                <br />
                            </div>
                            <div class="col-md-6">
                                <asp:Label ID="lblGroupByDept" runat="server" Text="<%$ Resources:Attendance,Department %>"></asp:Label>
                                <div class="input-group input-group-sm">
                                    <asp:DropDownList ID="dpDepartment" runat="server" CssClass="form-control" AutoPostBack="true"
                                        OnSelectedIndexChanged="dpDepartment_SelectedIndexChanged">
                                    </asp:DropDownList>
                                    <span class="input-group-btn">
                                        <asp:ImageButton ID="ImgButtonAllRefresh" Style="margin: 10px; margin-top: 2px;"
                                            runat="server" CausesValidation="False" Height="25px" ImageUrl="~/Images/refresh.png"
                                            Width="25px" OnClick="btnAllRefresh_Click" ToolTip="<%$ Resources:Attendance,Refresh %>"></asp:ImageButton>
                                    </span>
                                </div>
                            </div>
                            <div class="col-md-12">
                               

                                <div class="row">
		<div class="col-md-12">
			<div id="Div1" runat="server" class="box box-info collapsed-box">
				<div class="box-header with-border">
					<h3 class="box-title">
						<asp:Label ID="Label22" runat="server" Text="Advance Search"></asp:Label></h3>
					&nbsp;&nbsp;|&nbsp;&nbsp;
					<asp:Label ID="lblTotalRecordsAllowDeduction" Font-Bold="true" runat="server" Text="<%$ Resources:Attendance,Total Records: 0 %>"></asp:Label>

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
                                                <asp:DropDownList ID="ddlOption1" runat="server" class="form-control">
                                                    <asp:ListItem Text="<%$ Resources:Attendance,--Select-- %>"></asp:ListItem>
                                                    <asp:ListItem Text="<%$ Resources:Attendance,Equal %>"></asp:ListItem>
                                                    <asp:ListItem Selected="True" Text="<%$ Resources:Attendance,Contains %>"></asp:ListItem>
                                                    <asp:ListItem Text="<%$ Resources:Attendance,Like %>"></asp:ListItem>
                                                </asp:DropDownList>
                                            </div>
                                            <div class="col-lg-5">
                                                <asp:Panel ID="Panel1" runat="server" DefaultButton="imgBtnAllowDeductionBind">
                                                    <asp:TextBox ID="txtValue1" placeholder="Search from Content" runat="server" class="form-control"></asp:TextBox>
                                                </asp:Panel>
                                            </div>
                                            <div class="col-lg-2" style="text-align: center;">
                                                <asp:LinkButton ID="imgBtnAllowDeductionBind" 
                                                    runat="server" CausesValidation="False" OnClick="btnbind_Click"
                                                    ToolTip="<%$ Resources:Attendance,Search %>"><span class="fa fa-search"  style="font-size:25px;"></span></asp:LinkButton>&nbsp;&nbsp;&nbsp;
                                                <asp:LinkButton ID="btnRefresh" runat="server" CausesValidation="False"
                                                     OnClick="btnRefresh_Click" ToolTip="<%$ Resources:Attendance,Refresh %>"><span class="fa fa-repeat"  style="font-size:25px;"></span></asp:LinkButton>&nbsp;&nbsp;&nbsp;
                                                <asp:LinkButton ID="ImageButton6"  runat="server" OnClick="ImgbtnSelectAll_ClickAllowDeduction"
                                                    ToolTip="<%$ Resources:Attendance, Select All %>" AutoPostBack="true" ><span class="fas fa-th"  style="font-size:25px;"></span></asp:LinkButton>
                                            </div>
                                           
				</div>
			</div>
		</div>
	</div>





                                <div class="box box-warning box-solid"  <%= gvEmployee_Allowancededuction.Rows.Count>0?"style='display:block'":"style='display:none'"%> >
                                    
                                    <div class="box-body">
                                        <div class="row">
                                            <div class="col-md-12">
                                                <div class="flow">
                                                    <asp:CustomValidator ID="CustomValidator6" ValidationGroup="Save" runat="server" ErrorMessage="Please select at least one record."
                                                        ClientValidationFunction="Grid_Validate" ForeColor="Red"></asp:CustomValidator>

                                                    <asp:Label ID="lblSelectRecd" runat="server" Visible="false"></asp:Label>
                                                    <asp:GridView CssClass="table-striped table-bordered table table-hover" ID="gvEmployee_Allowancededuction" runat="server" AllowPaging="True"
                                                        AutoGenerateColumns="False" TabIndex="1" OnPageIndexChanging="gvEmployee_Allowancededuction_PageIndexChanging"
                                                        OnSorting="gvEmployee_Allowancededuction_Sorting" AllowSorting="true"
                                                        Width="100%" DataKeyNames="Emp_Id" PageSize='<%# int.Parse(Session["GridSize"].ToString()) %>'>
                                                        <Columns>
                                                            <asp:TemplateField>
                                                                <ItemTemplate>
                                                                    <asp:CheckBox ID="chkgvSelect" runat="server" />
                                                                </ItemTemplate>
                                                                <HeaderTemplate>
                                                                    <asp:CheckBox ID="chkgvSelectAll" runat="server" OnCheckedChanged="chkgvSelectAll_CheckedChangedAllowDeduction"
                                                                        AutoPostBack="true" />
                                                                </HeaderTemplate>
                                                                <ItemStyle HorizontalAlign="Center" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField>
                                                                <ItemTemplate>

                                                                    <asp:LinkButton ID="imgBtnEmpEdit" Visible='<%# hdnCanEdit.Value=="true"?true:false%>'	
                                                                        runat="server"  OnCommand="btnEmpEdit_Command" ToolTip="<%$ Resources:Attendance,Edit %>"
                                                                        CommandArgument='<%# Eval("Emp_Id") %>' ><i class="fa fa-pencil"></i></asp:LinkButton>
                                                                </ItemTemplate>
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
                                                                SortExpression="Emp_Name" ItemStyle-Width="20%">
                                                                <ItemStyle Width="20%" />
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="Emp_Name_L" HeaderText="<%$ Resources:Attendance,Employee Name Local%>"
                                                                SortExpression="Emp_Name_L" ItemStyle-Width="20%">
                                                                <ItemStyle Width="20%" />
                                                            </asp:BoundField>
                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance, Date Of Joining %>" SortExpression="DOJ">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblDoj" runat="server" Text='<%# DateFormat(Eval("Doj").ToString()) %>'></asp:Label>
                                                                </ItemTemplate>
                                                                <ItemStyle HorizontalAlign="Center" />
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
                                                    <asp:HiddenField ID="hdFSortgvEmpAllowDeduction" runat="server" />
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
                                <asp:Label ID="lblAllowDeductionType" runat="server" Text="<%$ Resources:Attendance,Type %>"></asp:Label>
                                <a style="color: Red">*</a>
                                <asp:RequiredFieldValidator EnableClientScript="true" Style="float: right;" runat="server" ID="RequiredFieldValidator3" ValidationGroup="Save" Display="Dynamic"
                                    SetFocusOnError="true" ControlToValidate="ddlType" InitialValue="0" ErrorMessage="<%$ Resources:Attendance,Select Type %>" />

                                <asp:DropDownList ID="ddlType" runat="server" CssClass="form-control" AutoPostBack="True"
                                    OnSelectedIndexChanged="ddlType_SelectedIndexChanged1">
                                    <asp:ListItem Text="<%$ Resources:Attendance,--Select-- %>" Value="0" />
                                    <asp:ListItem Text="<%$ Resources:Attendance,Allowance %>" Value="1" />
                                    <asp:ListItem Text="<%$ Resources:Attendance,Deduction %>" Value="2" />
                                </asp:DropDownList>
                                <br />
                                <asp:Label ID="lblPaidAllowDeduction" runat="server" Text="<%$ Resources:Attendance,Value Type %>"></asp:Label>
                                <asp:DropDownList ID="ddlValueType" runat="server" CssClass="form-control">
                                    <asp:ListItem Text="<%$ Resources:Attendance,Fixed %>" Value="1" />
                                    <asp:ListItem Text="<%$ Resources:Attendance,Percentage %>" Value="2" />
                                </asp:DropDownList>
                                <br />
                                <asp:Label ID="lblSchType" runat="server" Text="<%$ Resources:Attendance,Calculation %>"></asp:Label>
                                <asp:DropDownList ID="ddlCalculation" runat="server" CssClass="form-control">
                                    <asp:ListItem Text="<%$ Resources:Attendance,Daily %>" Value="Daily" />
                                    <asp:ListItem Text="<%$ Resources:Attendance,Monthly %>" Value="Monthly" />
                                </asp:DropDownList>
                                <br />
                            </div>
                            <div class="col-md-6">
                                <asp:Label ID="lblAllowDeduc" runat="server" Text="<%$ Resources:Attendance,Allowance/Deduction %>"></asp:Label>
                                <a style="color: Red">*</a>
                                <asp:RequiredFieldValidator EnableClientScript="true" Style="float: right;" runat="server" ID="RequiredFieldValidator1" ValidationGroup="Save" Display="Dynamic"
                                    SetFocusOnError="true" ControlToValidate="ddlAllDeduc" InitialValue="--Select--" ErrorMessage="<%$ Resources:Attendance,Select Allowance/Deduction %>" />
                                <asp:DropDownList ID="ddlAllDeduc" runat="server" CssClass="form-control">
                                </asp:DropDownList>
                                <br />
                                <asp:Label ID="lblPerOfSal" runat="server" Text="<%$ Resources:Attendance,Value %>"></asp:Label>
                                <a style="color: Red">*</a>
                                <asp:RequiredFieldValidator EnableClientScript="true" Style="float: right;" runat="server" ID="RequiredFieldValidator2" ValidationGroup="Save"
                                    Display="Dynamic" SetFocusOnError="true" ControlToValidate="txtCalValue" ErrorMessage="<%$ Resources:Attendance,Enter Value %>"></asp:RequiredFieldValidator>

                                <asp:TextBox ID="txtCalValue" runat="server" CssClass="form-control"></asp:TextBox>
                                <cc1:FilteredTextBoxExtender ID="txtMobile_FilteredTextBoxExtender" runat="server"
                                    Enabled="True" TargetControlID="txtCalValue" ValidChars="1,2,3,4,5,6,7,8,9,0,.">
                                </cc1:FilteredTextBoxExtender>
                                <br />
                                <asp:CheckBox ID="chkMonthCarry" Visible="false" runat="server" TabIndex="7" Text="<%$ Resources:Attendance,IsMonthCarry %>" />
                                <asp:CheckBox ID="chkYearCarry" runat="server" TabIndex="8" Text="<%$ Resources:Attendance,IsYearCarry %>" />
                            </div>
                            <div style="text-align: center;" class="col-md-12">
                                <asp:Button ID="btnSaveAllowDeduction" runat="server" CssClass="btn btn-danger"
                                    TabIndex="9" Visible="false" Text="<%$ Resources:Attendance,Save %>" ValidationGroup="Save"
                                    OnClick="btnSaveAllowDeduction_Click" />
                                <asp:Button ID="btnResetAllowDeduction" runat="server" CausesValidation="False" CssClass="btn btn-primary"
                                    TabIndex="10" Text="<%$ Resources:Attendance,Reset %>" OnClick="btnResetAllowDeduction_Click" />
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
    <div class="modal fade" id="myModal" tabindex="-1" role="dialog" aria-labelledby="myModalLabel"
        aria-hidden="true">
        <div class="modal-dialog modal-lg">
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal">
                        <span aria-hidden="true">&times;</span><span class="sr-only">Close</span></button>
                    <h4 class="modal-title" id="myModalLabel">Edit Employee Allowance Deduction Setup</h4>
                </div>
                <div class="modal-body">
                    <asp:UpdatePanel ID="update_Modal" runat="server">
                        <ContentTemplate>
                            <div class="row">
                                <div class="col-md-12">
                                    <div class="box box-primary">
                                        <div class="box-body">
                                            <div class="form-group">
                                                <div class="col-md-6">
                                                    <asp:HiddenField ID="hdnEmpId" Visible="false" runat="server" Value='<%# Eval("Emp_Id") %>' />
                                                    <asp:Label ID="Label14" runat="server" Font-Size="14px" Font-Bold="true" Text="<%$ Resources:Attendance,Employee Code %>"></asp:Label>
                                                    &nbsp:&nbsp
                                <asp:Label ID="lblEmpCodeAllowDeduction" runat="server" Font-Size="14px" Font-Bold="true"></asp:Label>
                                                    <br />
                                                </div>
                                                <div class="col-md-6" style="text-align: right;">
                                                    <asp:Label ID="Label13" runat="server" Font-Size="14px" Font-Bold="true" Text="<%$ Resources:Attendance,Employee Name %>"></asp:Label>
                                                    &nbsp:&nbsp<asp:Label ID="lblEmpNameAllowDeduction" runat="server" Font-Size="14px"
                                                        Font-Bold="true"></asp:Label>
                                                    <br />
                                                </div>
                                                <div class="col-md-12" style="text-align: center">
                                                    <asp:RadioButton ID="RbtAllowance" runat="server" Text="<%$ Resources:Attendance,Allowance %>"
                                                        GroupName="A" OnCheckedChanged="RbtAllowance_CheckedChanged" AutoPostBack="True"
                                                        TabIndex="11" />
                                                    <asp:RadioButton ID="RbtDeduction" Style="margin-left: 15px;" Text="<%$ Resources:Attendance,Deduction %>" runat="server"
                                                        GroupName="A" OnCheckedChanged="RbtDeduction_CheckedChanged" AutoPostBack="True"
                                                        TabIndex="12" />
                                                    <asp:RadioButton ID="RbtBoth" Style="margin-left: 15px;" runat="server" Text="<%$ Resources:Attendance,Both %>"
                                                        GroupName="A" OnCheckedChanged="RbtBoth_CheckedChanged" AutoPostBack="True" Checked="True"
                                                        TabIndex="13" />
                                                    <br />
                                                </div>
                                                <div class="col-md-12">
                                                    <br />
                                                    <div class="flow">
                                                        <asp:GridView CssClass="table-striped table-bordered table table-hover" ID="gvEditAllowDeduction" runat="server" AutoGenerateColumns="False"
                                                            DataKeyNames="Emp_Id,Value" PageSize='<%# int.Parse(Session["GridSize"].ToString()) %>' TabIndex="14"
                                                            Width="100%" AllowPaging="True" OnPageIndexChanging="gvEditAllowDeduction_PageIndexChanging">
                                                            <Columns>
                                                                <asp:TemplateField>
                                                                    <ItemTemplate>
                                                                        <asp:CheckBox ID="ChkEmpCheck" runat="server" />
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="<%$ Resources:Attendance,Type %>">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblType" runat="server" Text='<%# GetType(Eval("Type").ToString()) %>'></asp:Label>
                                                                        <asp:HiddenField ID="hdntransId" runat="server" Value='<%# Eval("Trans_Id") %>' />
                                                                        <asp:HiddenField ID="hdngvType" runat="server" Value='<%#Eval("Type") %>' />
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="<%$ Resources:Attendance,Allowance/Deduction %>">
                                                                    <ItemTemplate>
                                                                        <asp:HiddenField ID="hdnRefId" runat="server" Value='<%# Eval("Ref_Id") %>' />
                                                                        <asp:Label ID="lblgvRefValue" runat="server" Text='<%# Eval("Refference_Name") %>' />
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="<%$ Resources:Attendance,Value Type %>">
                                                                    <ItemTemplate>
                                                                        <asp:DropDownList ID="ddlSchType0" runat="server" SelectedValue='<%#Eval("Value_Type") %>'
                                                                            Visible="true" Width="110px" AutoPostBack="true">
                                                                            <asp:ListItem Value="1">Fixed</asp:ListItem>
                                                                            <asp:ListItem Value="2">Percentage</asp:ListItem>
                                                                        </asp:DropDownList>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="<%$ Resources:Attendance,Value %>">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblValue" Visible="false" runat="server" Text='<%#Common.GetAmountDecimal(Eval("Value").ToString(),Session["DBConnection"].ToString(),Session["LocCurrencyId"].ToString()) %>'></asp:Label>
                                                                        <asp:TextBox ID="txtValue" runat="server" Visible="true" Text='<%# Common.GetAmountDecimal(Eval("Value").ToString(),Session["DBConnection"].ToString(),Session["LocCurrencyId"].ToString()) %>'
                                                                            Width="110px"></asp:TextBox>
                                                                        <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtenderPAidAllowDeduction" runat="server"
                                                                            TargetControlID="txtValue" ValidChars="0,1,2,3,4,5,6,7,8,9,.">
                                                                        </cc1:FilteredTextBoxExtender>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="<%$ Resources:Attendance,Calculation %>">
                                                                    <ItemTemplate>
                                                                        <asp:DropDownList ID="ddlCalcuationGrid" runat="server" SelectedValue='<%# Eval("Calculation_Method") %>'
                                                                            Visible="True" Width="110px" AutoPostBack="true">
                                                                            <asp:ListItem Value="Daily">Daily</asp:ListItem>
                                                                            <asp:ListItem Value="Monthly">Monthly</asp:ListItem>
                                                                        </asp:DropDownList>
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
                            </div>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
                <div class="modal-footer">
                    <asp:UpdatePanel ID="Update_Modal_Button" runat="server">
                        <ContentTemplate>
                            <asp:Button ID="btnSave" runat="server" CausesValidation="False" CssClass="btn btn-success"
                                OnClick="BtnSave_Click" Text="<%$ Resources:Attendance,Save %>" />
                            <asp:Button ID="btnDel" runat="server" CausesValidation="False" CssClass="btn btn-primary"
                                OnClick="BtnDel_Click" Text="<%$ Resources:Attendance,Delete %>" />
                            <button type="button" class="btn btn-danger" data-dismiss="modal">
                                Close</button>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
            </div>
        </div>
    </div>
    <asp:UpdateProgress ID="UpdateProgress3" runat="server" AssociatedUpdatePanelID="Update_Modal_Button">
        <ProgressTemplate>
            <div class="modal_Progress">
                <div class="center_Progress">
                    <img alt="" src="../Images/ajax-loader2.gif" style="vertical-align: middle; background-color: White; border-radius: 10px;" />
                </div>
            </div>
        </ProgressTemplate>
    </asp:UpdateProgress>
    <asp:UpdateProgress ID="UpdateProgress1" runat="server" AssociatedUpdatePanelID="Update_Form">
        <ProgressTemplate>
            <div class="modal_Progress">
                <div class="center_Progress">
                    <img alt="" src="../Images/ajax-loader2.gif" style="vertical-align: middle; background-color: White; border-radius: 10px;" />
                </div>
            </div>
        </ProgressTemplate>
    </asp:UpdateProgress>
    <asp:UpdateProgress ID="UpdateProgress2" runat="server" AssociatedUpdatePanelID="update_Modal">
        <ProgressTemplate>
            <div class="modal_Progress">
                <div class="center_Progress">
                    <img alt="" src="../Images/ajax-loader2.gif" style="vertical-align: middle; background-color: White; border-radius: 10px;" />
                </div>
            </div>
        </ProgressTemplate>
    </asp:UpdateProgress>
    <asp:Panel ID="pnlAllowDeduction" runat="server" Visible="false">
    </asp:Panel>
    <asp:Panel ID="pnl1" runat="server" Visible="false">
    </asp:Panel>
    <asp:Panel ID="pnl2" runat="server" Visible="false">
    </asp:Panel>
    <asp:Panel ID="PnlEmployeeAllowDeduction" runat="server" Visible="false">
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

        function Grid_Validate(sender, args) {
            if (document.getElementById("<%=rbtnGroup.ClientID %>").checked) {
                var groupbox = document.getElementById("<%=lbxGroup.ClientID %>");
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
                var gridView = document.getElementById("<%=gvEmployee_Allowancededuction.ClientID %>");
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

        function show_modal() {
            document.getElementById('<%=Btn_Modal_Popup.ClientID %>').click();
        }
        function Modal_Close() {
            document.getElementById('<%= Btn_Modal_Popup.ClientID %>').click();
        }
    </script>
</asp:Content>
