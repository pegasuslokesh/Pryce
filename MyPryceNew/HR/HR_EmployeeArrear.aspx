<%@ Page Title="" Language="C#" MasterPageFile="~/ERPMaster.master" AutoEventWireup="true" CodeFile="HR_EmployeeArrear.aspx.cs" Inherits="HR_HR_EmployeeArrear" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="Head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="HeaderContent" runat="Server">
    <h1>
        <i class="fas fa-exclamation-circle"></i>
        <asp:Label ID="lblHeader" runat="server" Text="Employee Arrear"></asp:Label>
        <%--<small>Control panel</small>--%>
    </h1>
    <ol class="breadcrumb">
        <li><a href="#"><i class="fa fa-dashboard"></i>
            <asp:Label ID="Label6" runat="server" Text="<%$ Resources:Attendance,HR%>"></asp:Label></a></li>
        <li><a href="#"><i class="fa fa-dashboard"></i>
            <asp:Label ID="Label10" runat="server" Text="<%$ Resources:Attendance,Payroll%>"></asp:Label></a></li>
        <li class="active">
            <asp:Label ID="Label5" runat="server" Text="Arrear Information"></asp:Label></li>
    </ol>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="Server">
    <asp:UpdatePanel ID="Update_Button" runat="server">
        <ContentTemplate>
            <asp:HiddenField runat="server" ID="hdnCanDelete" />
            <asp:Button ID="Btn_List" Style="display: none;" runat="server" OnClick="Btn_List_Click"
                Text="list" />
            <asp:Button ID="Btn_New" Style="display: none;" runat="server" OnClick="Btn_New_Click"
                Text="list" />
            <asp:Button ID="Btn_Bin" Style="display: none;" runat="server" OnClick="Btn_Bin_Click"
                Text="list" />
        </ContentTemplate>
    </asp:UpdatePanel>
    <div class="row">
        <div class="col-md-12">
            <div class="nav-tabs-custom">
                <ul class="nav nav-tabs pull-right bg-blue-gradient">
                    <li><a onclick="Li_Tab_Bin()" href="#Bin" data-toggle="tab">
                        <i class="fa fa-trash"></i>&nbsp;&nbsp;<asp:Label ID="Label13"
                            runat="server" Text="<%$ Resources:Attendance,Bin %>"></asp:Label></a></li>
                    <li id="Li_New"><a onclick="Li_Tab_New()" href="#New" data-toggle="tab">
                        <asp:UpdatePanel ID="Update_Li" runat="server">
                            <ContentTemplate>
                                <i class="fa fa-file"></i>&nbsp;&nbsp;
                                <asp:Label ID="Lbl_Tab_New" runat="server" Text="<%$ Resources:Attendance,New%>"></asp:Label>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </a></li>
                    <li id="Li_List" class="active"><a onclick="Li_Tab_List()" href="#List" data-toggle="tab">
                        <i class="fa fa-list"></i>&nbsp;&nbsp;<asp:Label ID="Label3"
                            runat="server" Text="<%$ Resources:Attendance,List %>"></asp:Label></a></li>
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
                                                <asp:HiddenField ID="HDFSort" runat="server" />
                                                <div class="col-lg-3">
                                                    <asp:DropDownList ID="ddlFieldName" runat="server" CssClass="form-control">
                                                        <asp:ListItem Text="Employee Name" Value="Emp_Name"></asp:ListItem>
                                                        <asp:ListItem Text="From Month" Value="FromMonth"></asp:ListItem>
                                                        <asp:ListItem Text="To Month" Value="ToMonth"></asp:ListItem>
                                                        <asp:ListItem Text="<%$ Resources:Attendance,Created By %>" Value="Created_User"></asp:ListItem>
                                                        <asp:ListItem Text="<%$ Resources:Attendance,Modified By %>" Value="Modified_User"></asp:ListItem>
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
                                                    <asp:Panel ID="Panel2" runat="server" DefaultButton="btnbind">
                                                        <asp:TextBox ID="txtValue" placeholder="Search from Content" runat="server" CssClass="form-control"></asp:TextBox>
                                                    </asp:Panel>
                                                </div>
                                                <div class="col-lg-2">
                                                    <asp:Label ID="lblSelectedRecord" runat="server" Visible="false"></asp:Label>
                                                    <asp:LinkButton ID="btnbind" runat="server" CausesValidation="False"
                                                        OnClick="btnbind_Click" ToolTip="<%$ Resources:Attendance,Search %>"><span class="fa fa-search"  style="font-size:25px;"></span></asp:LinkButton>&nbsp;&nbsp;&nbsp;
                                                    <asp:LinkButton ID="btnRefresh" runat="server" CausesValidation="False"
                                                        OnClick="btnRefresh_Click" ToolTip="<%$ Resources:Attendance,Refresh %>"><span class="fa fa-repeat"  style="font-size:25px;"></span></asp:LinkButton>&nbsp;&nbsp;&nbsp;
                                                </div>

                                            </div>
                                        </div>
                                    </div>
                                </div>


                                <div class="box box-warning box-solid" <%= GvsalaryPlan.Rows.Count>0?"style='display:block'":"style='display:none'"%>>

                                    <div class="box-body">
                                        <div class="row">
                                            <div class="col-md-12">
                                                <br />
                                                <div class="flow">

                                                    <asp:HiddenField ID="hdnEditId" runat="server" />
                                                    <asp:GridView CssClass="table-striped table-bordered table table-hover" ID="GvsalaryPlan" runat="server" AutoGenerateColumns="False" Width="100%"
                                                        AllowPaging="True" OnPageIndexChanging="GvsalaryPlan_PageIndexChanging" PageSize='<%# int.Parse(Session["GridSize"].ToString()) %>'
                                                        AllowSorting="True" OnSorting="GvsalaryPlan_Sorting">

                                                        <Columns>

                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,Delete %>">
                                                                <ItemTemplate>
                                                                    <asp:LinkButton ID="IbtnDelete" runat="server" CausesValidation="False" CommandArgument='<%# Eval("Trans_Id") %>'
                                                                        Visible='<%# hdnCanDelete.Value=="true"?true:false%>'	 OnCommand="IbtnDelete_Command"
                                                                        ToolTip="<%$ Resources:Attendance,Delete %>"><i class="fa fa-trash"></i>Delete</asp:LinkButton>
                                                                </ItemTemplate>
                                                                <ItemStyle HorizontalAlign="Center" />
                                                                <ItemStyle />
                                                            </asp:TemplateField>

                                                            <asp:BoundField DataField="Emp_Name" HeaderText="Employee Name" SortExpression="Emp_Name">
                                                                <ItemStyle />
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="FromMonth" HeaderText="From Month" SortExpression="FromMonth">
                                                                <ItemStyle />
                                                            </asp:BoundField>


                                                            <asp:BoundField DataField="ToMonth" HeaderText="To Month" SortExpression="ToMonth">
                                                                <ItemStyle />
                                                            </asp:BoundField>


                                                            <asp:TemplateField HeaderText="Arrear Amount">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblArrear" runat="server" Text='<%# GetAmountDecimal(Eval("Arrear_amount").ToString()) %>'></asp:Label>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>


                                                            <asp:BoundField DataField="currency_Name" HeaderText="Currency" SortExpression="currency_Name">
                                                                <ItemStyle />
                                                            </asp:BoundField>

                                                            <asp:BoundField DataField="Created_User" HeaderText="<%$ Resources:Attendance,Created By%>"
                                                                SortExpression="Created_User">
                                                                <ItemStyle />
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="Modified_User" HeaderText="<%$ Resources:Attendance,Modified By %>"
                                                                SortExpression="Modified_User">
                                                                <ItemStyle />
                                                            </asp:BoundField>

                                                            <asp:BoundField DataField="Is_Adjusted" HeaderText="Is Adjusted"
                                                                SortExpression="Is_Adjusted">
                                                                <ItemStyle />
                                                            </asp:BoundField>
                                                        </Columns>

                                                        

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
                                <div class="col-md-12">
                                    <div id="pnlEmpSal" runat="server" class="row">
                                        <div class="row">
                                            <div class="col-md-12">
                                                <div class="box box-primary">
                                                    <div class="box-body">
                                                        <div class="form-group">


                                                            <div class="col-md-6">
                                                                <asp:Label ID="lblDeptSalary" runat="server" Text="<%$ Resources:Attendance,Department %>"></asp:Label>
                                                                <div class="input-group" style="width: 100%">
                                                                    <asp:DropDownList ID="ddlDeptSalary" Style="width: 92%" runat="server" CssClass="form-control"
                                                                        AutoPostBack="true" OnSelectedIndexChanged="ddlDeptSalary_SelectedIndexChanged">
                                                                    </asp:DropDownList>

                                                                    <asp:LinkButton ID="btnAllRefreshSalary"  runat="server" CausesValidation="False"
                                                                         OnClick="btnAllRefreshSalary_Click"
                                                                        ToolTip="<%$ Resources:Attendance,Refresh %>"><span class="fa fa-repeat"  style="font-size:25px; margin-left:10px;"></span></asp:LinkButton>&nbsp;&nbsp;&nbsp;
                                                                    <br />
                                                                </div>
                                                            </div>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                       

                                        <div class="row">
		<div class="col-md-12">
			<div id="Div2" runat="server" class="box box-info collapsed-box">
				<div class="box-header with-border">
					<h3 class="box-title">
						<asp:Label ID="Label7" runat="server" Text="Advance Search"></asp:Label></h3>
					&nbsp;&nbsp;|&nbsp;&nbsp;
					<asp:Label ID="lblTotalRecordSal" runat="server" Text="<%$ Resources:Attendance,Total Records: 0 %>"></asp:Label>

					<div class="box-tools pull-right">
						<button type="button" class="btn btn-box-tool" data-widget="collapse">
							<i id="I2" runat="server" class="fa fa-plus"></i>
						</button>
					</div>
				</div>
				<div class="box-body">
                      <div class="col-lg-3">
                                                        <asp:DropDownList ID="ddlFieldSal" runat="server" CssClass="form-control">
                                                            <asp:ListItem Text="<%$ Resources:Attendance,Employee Code %>" Value="Emp_Code"></asp:ListItem>
                                                            <asp:ListItem Text="<%$ Resources:Attendance,Employee Name %>" Value="Emp_Name" Selected="True"></asp:ListItem>
                                                            <asp:ListItem Text="<%$ Resources:Attendance,Employee Name Local %>" Value="Emp_Name_L"></asp:ListItem>
                                                            <asp:ListItem Text="<%$ Resources:Attendance,Email Id %>" Value="Email_Id"></asp:ListItem>
                                                            <asp:ListItem Text="<%$ Resources:Attendance,Phone No. %>" Value="Phone_No"></asp:ListItem>
                                                            <asp:ListItem Text="<%$ Resources:Attendance,Civil Id %>" Value="Civil_Id"></asp:ListItem>
                                                        </asp:DropDownList>
                                                    </div>
                                                    <div class="col-lg-2">
                                                        <asp:DropDownList ID="ddlOptionSal" runat="server" CssClass="form-control">
                                                            <asp:ListItem Text="<%$ Resources:Attendance,--Select-- %>"></asp:ListItem>
                                                            <asp:ListItem Text="<%$ Resources:Attendance,Equal %>"></asp:ListItem>
                                                            <asp:ListItem Selected="True" Text="<%$ Resources:Attendance,Contains %>"></asp:ListItem>
                                                            <asp:ListItem Text="<%$ Resources:Attendance,Like %>"></asp:ListItem>
                                                        </asp:DropDownList>
                                                    </div>
                                                    <div class="col-lg-5">
                                                        <asp:Panel ID="Panel1" runat="server" DefaultButton="btn_Salarybind">
                                                            <asp:TextBox ID="txtValueSal"  placeholder="Search from Content" runat="server" CssClass="form-control"></asp:TextBox>
                                                            <asp:RequiredFieldValidator EnableClientScript="true" runat="server" ID="RequiredFieldValidator11" ValidationGroup="S_Search" Display="Dynamic" SetFocusOnError="true" ControlToValidate="txtValueSal" ErrorMessage="<%$ Resources:Attendance,Please enter value %>" />
                                                        </asp:Panel>

                                                    </div>
                                                    <div class="col-lg-2" style="text-align: center; padding-left: 1px; padding-right: 1px;">
                                                        <asp:LinkButton ID="btn_Salarybind" runat="server"
                                                            CausesValidation="true" ValidationGroup="S_Search"  OnClick="btnSalarybind_Click"
                                                            ToolTip="<%$ Resources:Attendance,Search %>"><span class="fa fa-search"  style="font-size:25px;"></span></asp:LinkButton>&nbsp;&nbsp;&nbsp;

                                                        <asp:LinkButton ID="btn_SalaryRefresh" runat="server"  CausesValidation="False"
                                                            OnClick="btnSalaryRefresh_Click" ToolTip="<%$ Resources:Attendance,Refresh %>"><span class="fa fa-repeat"  style="font-size:25px;"></span></asp:LinkButton>&nbsp;&nbsp;&nbsp;

                                                        <asp:LinkButton ID="Img_btnSelectAll"  runat="server" OnClick="ImgbtnSelectAll_ClickSalary"
                                                            ToolTip="<%$ Resources:Attendance, Select All %>" AutoPostBack="true"  ><span class="fas fa-th"  style="font-size:25px;"></span></asp:LinkButton>



                                                    </div>
                                                  
				</div>
			</div>
		</div>
	</div>


                                        <div class="box box-warning box-solid"  <%= gvEmpSalary.Rows.Count>0?"style='display:block'":"style='display:none'"%> >
                                            
                                            <div class="box-body">
                                                <div class="row">
                                                    <div class="col-md-12">
                                                        <div class="flow">
                                                            <asp:Label ID="lblSelectRecordSal" runat="server" Visible="false"></asp:Label>
                                                            <asp:CustomValidator ID="CustomValidator4" ValidationGroup="Save" runat="server" ErrorMessage="Please select at least one record."
                                                                ClientValidationFunction="Salary_Validate" ForeColor="Red"></asp:CustomValidator>

                                                            <asp:GridView CssClass="table-striped table-bordered table table-hover" ID="gvEmpSalary" runat="server" AllowPaging="false" AutoGenerateColumns="False"
                                                                OnPageIndexChanging="gvEmpSal_PageIndexChanging" Width="100%"
                                                                DataKeyNames="Emp_Id" PageSize='<%# int.Parse(Session["GridSize"].ToString()) %>'>
                                                                <Columns>
                                                                    <asp:TemplateField>
                                                                        <ItemTemplate>
                                                                            <asp:CheckBox ID="chkgvSelect" runat="server" />
                                                                        </ItemTemplate>
                                                                        <HeaderTemplate>
                                                                            <asp:CheckBox ID="chkgvSelectAll" runat="server" OnCheckedChanged="chkgvSelectAll_CheckedChangedSal"
                                                                                AutoPostBack="true" />
                                                                        </HeaderTemplate>
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
                                                                        SortExpression="Emp_Name" ItemStyle-Width="20%" />
                                                                    <asp:BoundField DataField="Emp_Name_L" HeaderText="<%$ Resources:Attendance,Employee Name Local %>"
                                                                        SortExpression="Emp_Name_L" ItemStyle-Width="20%" />
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

                                                                    <asp:TemplateField HeaderText="Arrear Amount">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblarrear" runat="server" Text="0"></asp:Label>
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


                                <div class="col-md-12">

                                    <div class="col-md-6">
                                        <asp:Label ID="Label1" runat="server" Text="From Month"></asp:Label>
                                        <a style="color: Red">*</a>
                                        <asp:RequiredFieldValidator EnableClientScript="true" Style="float: right;" runat="server"
                                            ID="RequiredFieldValidator1" ValidationGroup="Save" Display="Dynamic" SetFocusOnError="true"
                                            ControlToValidate="ddlFromMonth" InitialValue="0" ErrorMessage="Select From Month" />

                                        <asp:DropDownList ID="ddlFromMonth" runat="server" CssClass="form-control">
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
                                    </div>

                                    <div class="col-md-6">
                                        <asp:Label ID="LblYear" runat="server" Text="From Year"></asp:Label>
                                        <a style="color: Red">*</a>
                                        <asp:RequiredFieldValidator EnableClientScript="true" Style="float: right;" runat="server"
                                            ID="RequiredFieldValidator6" ValidationGroup="Save" Display="Dynamic" SetFocusOnError="true"
                                            ControlToValidate="TxtFromYear" ErrorMessage="<%$ Resources:Attendance,Enter Year %>"></asp:RequiredFieldValidator>
                                        <asp:TextBox ID="TxtFromYear" runat="server" CssClass="form-control"></asp:TextBox>
                                        <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender4" runat="server" Enabled="True"
                                            TargetControlID="TxtFromYear" FilterType="Numbers">
                                        </cc1:FilteredTextBoxExtender>
                                        <br />
                                    </div>

                                </div>






                                <div class="col-md-12">

                                    <div class="col-md-6">
                                        <asp:Label ID="Label2" runat="server" Text="To Month"></asp:Label>
                                        <a style="color: Red">*</a>
                                        <asp:RequiredFieldValidator EnableClientScript="true" Style="float: right;" runat="server"
                                            ID="RequiredFieldValidator7" ValidationGroup="Save" Display="Dynamic" SetFocusOnError="true"
                                            ControlToValidate="ddlToMonth" InitialValue="0" ErrorMessage="Select to Month" />



                                        <asp:DropDownList ID="ddlToMonth" runat="server" CssClass="form-control">
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





                                    </div>

                                    <div class="col-md-6">
                                        <asp:Label ID="Label4" runat="server" Text="To year"></asp:Label>
                                        <a style="color: Red">*</a>
                                        <asp:RequiredFieldValidator EnableClientScript="true" Style="float: right;" runat="server"
                                            ID="RequiredFieldValidator2" ValidationGroup="Save" Display="Dynamic" SetFocusOnError="true"
                                            ControlToValidate="txttoYear" ErrorMessage="<%$ Resources:Attendance,Enter Year %>"></asp:RequiredFieldValidator>
                                        <div class="input-group">
                                            <asp:TextBox ID="txttoYear" runat="server" CssClass="form-control"></asp:TextBox>
                                            <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" runat="server" Enabled="True"
                                                TargetControlID="txttoYear" FilterType="Numbers">
                                            </cc1:FilteredTextBoxExtender>
                                            <div class="input-group-btn">
                                                <asp:Button ID="btnGetArrear" runat="server" ValidationGroup="Save" Text="Get Arrear"
                                                    class="btn btn-info" OnClick="btnGetArrear_Click" />
                                                <br />

                                            </div>

                                        </div>


                                    </div>







                                </div>


                                <div class="col-md-12" style="text-align: center;">

                                    <br />
                                    <asp:Button ID="Btn_Save" runat="server" ValidationGroup="Save" Text="Save" Visible="false"
                                        class="btn btn-success" OnClick="Btn_Save_Click" />
                                    <asp:Button ID="btn_reset" runat="server" Text="Reset"
                                        class="btn btn-primary" OnClick="Btn_Reset_Click" />

                                    <asp:Button ID="Btn_Cancel" runat="server" Text="<%$ Resources:Attendance,Cancel%>"
                                        class="btn btn-danger" OnClick="Btn_Cancel_Click" />

                                </div>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </div>
                    <div class="tab-pane" id="Bin">
                        <asp:UpdatePanel ID="Update_Bin" runat="server">
                            <ContentTemplate>
                             

                                <div class="row">
		<div class="col-md-12">
			<div id="Div3" runat="server" class="box box-info collapsed-box">
				<div class="box-header with-border">
					<h3 class="box-title">
						<asp:Label ID="Label8" runat="server" Text="Advance Search"></asp:Label></h3>
					&nbsp;&nbsp;|&nbsp;&nbsp;
					<asp:Label ID="lblbinTotalRecords" Font-Bold="true" runat="server" Text="<%$ Resources:Attendance,Total Records: 0 %>"></asp:Label>

					<div class="box-tools pull-right">
						<button type="button" class="btn btn-box-tool" data-widget="collapse">
							<i id="I3" runat="server" class="fa fa-plus"></i>
						</button>
					</div>
				</div>
				<div class="box-body">
                         <div class="col-lg-3">
                                                <asp:DropDownList ID="ddlbinFieldName" runat="server" CssClass="form-control">
                                                    <asp:ListItem Text="Employee Name" Value="Emp_Name"></asp:ListItem>
                                                    <asp:ListItem Text="From Month" Value="FromMonth"></asp:ListItem>
                                                    <asp:ListItem Text="To Month" Value="ToMonth"></asp:ListItem>
                                                    <asp:ListItem Text="<%$ Resources:Attendance,Created By %>" Value="Created_User"></asp:ListItem>
                                                    <asp:ListItem Text="<%$ Resources:Attendance,Modified By %>" Value="Modified_User"></asp:ListItem>
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
                                            <div class="col-lg-5">
                                                <asp:Panel ID="Panel3" runat="server" DefaultButton="btnbinbind">
                                                    <asp:TextBox ID="txtbinValue" placeholder="Search from Content" runat="server" CssClass="form-control"></asp:TextBox>
                                                </asp:Panel>
                                            </div>
                                            <div class="col-lg-2">
                                                <asp:LinkButton ID="btnbinbind"  runat="server" CausesValidation="False"
                                                     OnClick="btnbinbind_Click" ToolTip="<%$ Resources:Attendance,Search %>"><span class="fa fa-search"  style="font-size:25px;"></span></asp:LinkButton>&nbsp;&nbsp;&nbsp;
                                                <asp:LinkButton ID="btnbinRefresh"  runat="server" CausesValidation="False"
                                                     OnClick="btnbinRefresh_Click" ToolTip="<%$ Resources:Attendance,Refresh %>"><span class="fa fa-repeat"  style="font-size:25px;"></span></asp:LinkButton>&nbsp;&nbsp;&nbsp;
                                                <asp:LinkButton ID="imgBtnRestore"  CausesValidation="False" 
                                                    runat="server"  OnClick="imgBtnRestore_Click" ToolTip="<%$ Resources:Attendance, Active %>" ><span class="far fa-lightbulb"  style="font-size:25px;"></span></asp:LinkButton>&nbsp;&nbsp;&nbsp;
                                                <asp:LinkButton ID="ImgbtnSelectAll"  runat="server" OnClick="ImgbtnSelectAll_Click"
                                                    ToolTip="<%$ Resources:Attendance, Select All %>" AutoPostBack="true"  ><span class="fas fa-th"  style="font-size:25px;"></span></asp:LinkButton>
                                            </div>
                                          

				</div>
			</div>
		</div>
	</div>
                                <div class="box box-warning box-solid"  <%= GvsalaryPlanBin.Rows.Count>0?"style='display:block'":"style='display:none'"%> >
                                    
                                    <div class="box-body">
                                        <div class="row">
                                            <div class="col-md-12">
                                                <div class="flow">
                                                    <asp:GridView CssClass="table-striped table-bordered table table-hover" ID="GvsalaryPlanBin" runat="server" AutoGenerateColumns="False" Width="100%"
                                                        AllowPaging="True" DataKeyNames="Trans_Id" PageSize='<%# int.Parse(Session["GridSize"].ToString()) %>'
                                                        OnPageIndexChanging="GvsalaryPlanBin_PageIndexChanging" OnSorting="GvsalaryPlanBin_Sorting"
                                                        AllowSorting="true">
                                                        <Columns>
                                                            <asp:TemplateField Visible="false">
                                                                <HeaderTemplate>
                                                                    <asp:CheckBox ID="chkgvSelectAll" runat="server" AutoPostBack="true" OnCheckedChanged="chkgvSelectAll_CheckedChanged" />
                                                                </HeaderTemplate>
                                                                <ItemTemplate>
                                                                    <asp:CheckBox ID="chkgvSelect" runat="server" />
                                                                </ItemTemplate>
                                                                <ItemStyle HorizontalAlign="Center" />
                                                                <HeaderStyle HorizontalAlign="Center" />
                                                            </asp:TemplateField>
                                                            <asp:BoundField DataField="Emp_Name" HeaderText="Employee Name" SortExpression="Emp_Name">
                                                                <ItemStyle />
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="FromMonth" HeaderText="From Month" SortExpression="FromMonth">
                                                                <ItemStyle />
                                                            </asp:BoundField>


                                                            <asp:BoundField DataField="ToMonth" HeaderText="To Month" SortExpression="ToMonth">
                                                                <ItemStyle />
                                                            </asp:BoundField>


                                                            <asp:TemplateField HeaderText="Arrear Amount">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblArrear" runat="server" Text='<%# GetAmountDecimal(Eval("Arrear_amount").ToString()) %>'></asp:Label>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>


                                                            <asp:BoundField DataField="currency_Name" HeaderText="Currency" SortExpression="currency_Name">
                                                                <ItemStyle />
                                                            </asp:BoundField>

                                                            <asp:BoundField DataField="Created_User" HeaderText="<%$ Resources:Attendance,Created By%>"
                                                                SortExpression="Created_User">
                                                                <ItemStyle />
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="Modified_User" HeaderText="<%$ Resources:Attendance,Modified By %>"
                                                                SortExpression="Modified_User">
                                                                <ItemStyle />
                                                            </asp:BoundField>

                                                            <asp:BoundField DataField="Is_Adjusted" HeaderText="Is Adjusted"
                                                                SortExpression="Is_Adjusted">
                                                                <ItemStyle />
                                                            </asp:BoundField>
                                                        </Columns>


                                                        

                                                    </asp:GridView>
                                                    <asp:HiddenField ID="HDFSortbin" runat="server" />
                                                </div>
                                            </div>
                                        </div>
                                        <!-- /.box-body -->
                                    </div>
                                    <!-- /.box -->
                                </div>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <asp:UpdateProgress ID="UpdateProgress1" runat="server" AssociatedUpdatePanelID="Update_New">
        <ProgressTemplate>
            <div class="modal_Progress">
                <div class="center_Progress">
                    <img alt="" src="../Images/ajax-loader2.gif" style="vertical-align: middle; background-color: White; border-radius: 10px;" />
                </div>
            </div>
        </ProgressTemplate>
    </asp:UpdateProgress>
    <asp:UpdateProgress ID="UpdateProgress2" runat="server" AssociatedUpdatePanelID="Update_List">
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
    <asp:UpdateProgress ID="UpdateProgress4" runat="server" AssociatedUpdatePanelID="Update_Button">
        <ProgressTemplate>
            <div class="modal_Progress">
                <div class="center_Progress">
                    <img alt="" src="../Images/ajax-loader2.gif" style="vertical-align: middle; background-color: White; border-radius: 10px;" />
                </div>
            </div>
        </ProgressTemplate>
    </asp:UpdateProgress>
    <asp:Panel ID="pnlMoveChild" runat="server" Visible="false">
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

        function Li_Tab_List() {
            document.getElementById('<%= Btn_List.ClientID %>').click();
        }
        function Li_Tab_New() {
            document.getElementById('<%= Btn_New.ClientID %>').click();
        }
        function Li_Tab_Bin() {
            document.getElementById('<%= Btn_Bin.ClientID %>').click();
        }

        function LI_Edit_Active() {
            $("#Li_List").removeClass("active");
            $("#List").removeClass("active");

            $("#Li_New").addClass("active");
            $("#New").addClass("active");
        }

        function LI_List_Active() {
            $("#Li_New").removeClass("active");
            $("#New").removeClass("active");

            $("#Li_List").addClass("active");
            $("#List").addClass("active");
        }

        function LI_Bin_Active() {
            $("#Li_Bin").removeClass("active");
            $("#Bin").removeClass("active");

            $("#Li_List").addClass("active");
            $("#List").addClass("active");
        }



        function Salary_Validate(sender, args) {

            var gridView = document.getElementById("<%=gvEmpSalary.ClientID %>");
            var checkBoxes = gridView.getElementsByTagName("input");
            for (var i = 0; i < checkBoxes.length; i++) {
                if (checkBoxes[i].type == "checkbox" && checkBoxes[i].checked) {
                    args.IsValid = true;
                    return;
                }
            }
            args.IsValid = false;

        }



    </script>
</asp:Content>
