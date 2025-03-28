<%@ Page Title="" Language="C#" MasterPageFile="~/ERPMaster.master" AutoEventWireup="true" CodeFile="HR_EmployeePayment.aspx.cs" Inherits="HR_HR_EmployeePayment" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Src="~/WebUserControl/ReportSystem.ascx" TagName="ReportSystem" TagPrefix="RS" %>
<asp:Content ID="Content1" ContentPlaceHolderID="Head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="HeaderContent" runat="Server">
    <h1>
        <i class="fas fa-dollar-sign"></i>
        <asp:Label ID="lblHeader" runat="server" Text="Employee Payment"></asp:Label>
        <%--<small>Control panel</small>--%>
    </h1>
    <ol class="breadcrumb">
        <asp:HiddenField runat="server" ID="hdnCanPrint" />
        <li><a href="#"><i class="fa fa-dashboard"></i>
            <asp:Label ID="Label6" runat="server" Text="<%$ Resources:Attendance,HR%>"></asp:Label></a></li>
        <li><a href="#"><i class="fa fa-dashboard"></i>
            <asp:Label ID="Label10" runat="server" Text="<%$ Resources:Attendance,Payroll%>"></asp:Label></a></li>
        <li class="active">
            <asp:Label ID="Label5" runat="server" Text="Employee Payment"></asp:Label></li>
    </ol>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="Server">
    <asp:UpdatePanel ID="Update_Button" runat="server">
        <ContentTemplate>
            <asp:Button ID="Btn_Salary_Modal" Style="display: none;" runat="server" data-toggle="modal" data-target="#Payment_Account_Update" Text="Salary" />
        </ContentTemplate>
    </asp:UpdatePanel>

    <div class="row">
        <div class="col-md-12">
            <div class="nav-tabs-custom">
                <ul class="nav nav-tabs pull-right bg-blue-gradient">
                    <li id="Li_New"><a onclick="Li_Tab_New()" href="#New" data-toggle="tab">
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
                                        <div class="box box-primary">
                                            <div class="box-body">
                                                <div class="form-group">
                                                    <div class="col-md-6">
                                                        <asp:Label ID="lblLocationSalary" runat="server" Text="<%$ Resources:Attendance,Location %>"></asp:Label>
                                                        <asp:DropDownList ID="ddl_Location" runat="server" CssClass="form-control"
                                                            AutoPostBack="true" OnSelectedIndexChanged="ddl_Location_SelectedIndexChanged">
                                                        </asp:DropDownList>
                                                        <br />
                                                    </div>
                                                    <div class="col-md-6">
                                                        <asp:Label ID="lblDeptSalary" runat="server" Text="<%$ Resources:Attendance,Department %>"></asp:Label>
                                                        <asp:DropDownList ID="ddl_Department" runat="server" CssClass="form-control"
                                                            AutoPostBack="true" OnSelectedIndexChanged="ddl_Department_SelectedIndexChanged">
                                                        </asp:DropDownList>
                                                        <br />
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
                                                    <asp:Label ID="Label22" runat="server" Text="Advance Search"></asp:Label></h3>
                                                &nbsp;&nbsp;|&nbsp;&nbsp;
					<asp:Label ID="Lbl_TotalRecords" Font-Bold="true" Text="<%$ Resources:Attendance,Total Records: 0 %>" runat="server"></asp:Label>

                                                <div class="box-tools pull-right">
                                                    <button type="button" class="btn btn-box-tool" data-widget="collapse">
                                                        <i id="I1" runat="server" class="fa fa-plus"></i>
                                                    </button>
                                                </div>
                                            </div>
                                            <div class="box-body">

                                                <div class="col-lg-2">
                                                    <asp:DropDownList ID="ddlFieldName" runat="server" OnSelectedIndexChanged="ddlFieldName_SelectedIndexChanged" AutoPostBack="true" class="form-control">
                                                        <asp:ListItem Selected="True" Text="<%$ Resources:Attendance, Voucher No%>" Value="Voucher_No" />
                                                        <asp:ListItem Text="<%$ Resources:Attendance, Voucher Date%>" Value="Voucher_Date" />
                                                        <asp:ListItem Text="<%$ Resources:Attendance, Voucher Amount%>" Value="Voucher_Amount" />
                                                        <asp:ListItem Text="<%$ Resources:Attendance, Created By%>" Value="Created_By" />
                                                        <asp:ListItem Text="<%$ Resources:Attendance, Modified By%>" Value="Modified_By" />
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
                                                    <asp:Panel ID="Pnl_Value" runat="server" DefaultButton="btnbind">
                                                        <asp:TextBox ID="txtValue" placeholder="Search from Content" runat="server" class="form-control"></asp:TextBox>
                                                        <asp:TextBox ID="txtValueDate" placeholder="Search from Date" runat="server" CssClass="form-control" Visible="false"></asp:TextBox>
                                                        <cc1:CalendarExtender OnClientShown="showCalendar" ID="CalendarExtendertxtValueDate" Format="dd-MMM-yyyy" runat="server" TargetControlID="txtValueDate" />
                                                    </asp:Panel>
                                                </div>
                                                <div class="col-lg-3">
                                                    <asp:LinkButton ID="btnbind" runat="server" CausesValidation="False"
                                                        ToolTip="<%$ Resources:Attendance,Search %>" OnClick="btnbind_Click"><span class="fa fa-search"  style="font-size:25px;"></span></asp:LinkButton>&nbsp;&nbsp;&nbsp;
                                                <asp:LinkButton ID="btnRefresh" runat="server" CausesValidation="False" OnClick="btnRefresh_Click"
                                                    ToolTip="<%$ Resources:Attendance,Refresh %>"><span class="fa fa-repeat"  style="font-size:25px;"></span></asp:LinkButton>&nbsp;&nbsp;&nbsp;
                                                <asp:LinkButton ID="Img_Emp_List_Select_All" runat="server"
                                                    ToolTip="<%$ Resources:Attendance, Select All %>" AutoPostBack="true" OnClick="Img_Emp_List_Select_All_Click" Visible="false"><span class="fas fa-th"  style="font-size:25px;" ></span></asp:LinkButton>
                                                </div>

                                            </div>
                                        </div>
                                    </div>
                                </div>

                                <div class="box box-warning box-solid" <%= Gv_Payment_Voucher.Rows.Count>0?"style='display:block'":"style='display:none'"%>>

                                    <div class="box-body">
                                        <div class="row">
                                            <div class="col-md-12">
                                                <div style="overflow: auto; max-height: 500px;">
                                                    <asp:GridView CssClass="table-striped table-bordered table table-hover" ID="Gv_Payment_Voucher" runat="server" AllowPaging="false" AutoGenerateColumns="False"
                                                        Width="100%" DataKeyNames="Trans_Id" PageSize='<%# int.Parse(Session["GridSize"].ToString()) %>'>
                                                        <Columns>
                                                            <asp:TemplateField HeaderText="Print">
                                                                <ItemTemplate>
                                                                    <asp:LinkButton ID="IbtnPrint" runat="server" CausesValidation="False" CommandArgument='<%# Eval("Trans_Id") %>'
                                                                        OnCommand="IbtnPrint_Command" ToolTip="<%$ Resources:Attendance,Print %>" Visible='<%# hdnCanPrint.Value=="true"?true:false%>'
                                                                        Width="16px"><i class="fa fa-print"></i></asp:LinkButton>
                                                                </ItemTemplate>
                                                                <ItemStyle HorizontalAlign="Center" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="">
                                                                <ItemTemplate>
                                                                    <a style="cursor: pointer" title="Report System" onclick="getReportData('<%# Eval("Trans_Id") %>')"><i class="fa fa-print"></i></a>
                                                                </ItemTemplate>
                                                                <ItemStyle HorizontalAlign="Center" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="S.No">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblSerialno" runat="server" Text='<%#Container.DataItemIndex+1 %>'></asp:Label>
                                                                </ItemTemplate>
                                                                <ItemStyle HorizontalAlign="Center" />
                                                                <ItemStyle />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,Voucher No %>">
                                                                <ItemTemplate>
                                                                    <asp:HiddenField ID="Hdn_Trans_ID" runat="server" Value='<%# Eval("Trans_Id") %>' />
                                                                    <asp:Label ID="Lbl_Voucher_No" runat="server" Text='<%# Eval("Voucher_No") %>'></asp:Label>
                                                                </ItemTemplate>
                                                                <ItemStyle HorizontalAlign="Center" />
                                                                <ItemStyle />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,Voucher Date %>">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="Lbl_Voucher_Date" runat="server" Text='<%#GetDate(Eval("Voucher_Date").ToString()) %>'></asp:Label>
                                                                </ItemTemplate>
                                                                <ItemStyle />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,Voucher Amount %>">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="Lbl_Voucher_Amount" runat="server" Text='<%# SetDecimal(Eval("Voucher_Amount").ToString(),Eval("Currency_Id").ToString()) %>'></asp:Label>
                                                                </ItemTemplate>
                                                                <ItemStyle HorizontalAlign="Right" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,Payment By%>">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="Lbl_Payment_By" runat="server" Text='<%# Eval("Account_Name") %>'></asp:Label>
                                                                </ItemTemplate>
                                                                <ItemStyle />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,Created By%>">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="Lbl_Created_By" runat="server" Text='<%# Eval("Created_By") %>'></asp:Label>
                                                                </ItemTemplate>
                                                                <ItemStyle />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,Modified By%>">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="Lbl_Modified_By" runat="server" Text='<%# Eval("Modified_By") %>'></asp:Label>
                                                                </ItemTemplate>
                                                                <ItemStyle />
                                                            </asp:TemplateField>
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
                                <div class="row">
                                    <div class="col-md-12">
                                        <div class="box box-primary">
                                            <div class="box-body">
                                                <div class="form-group">
                                                    <div class="col-md-6">
                                                        <asp:Label ID="Label3" runat="server" Text="<%$ Resources:Attendance,Location %>"></asp:Label>
                                                        <asp:DropDownList ID="DDL_Location_New" runat="server" CssClass="form-control"
                                                            AutoPostBack="true" OnSelectedIndexChanged="DDL_Location_New_SelectedIndexChanged">
                                                        </asp:DropDownList>
                                                        <br />
                                                    </div>
                                                    <div class="col-md-6">
                                                        <asp:Label ID="Label4" runat="server" Text="<%$ Resources:Attendance,Department %>"></asp:Label>
                                                        <asp:DropDownList ID="DDL_Department_New" runat="server" CssClass="form-control"
                                                            AutoPostBack="true" OnSelectedIndexChanged="DDL_Department_New_SelectedIndexChanged">
                                                        </asp:DropDownList>
                                                        <br />
                                                    </div>
                                                    <div class="col-md-10">
                                                        <br />
                                                    </div>
                                                    <div class="col-md-2">
                                                        <asp:CheckBox ID="chkEmployeeSelect" runat="server" AutoPostBack="true" OnCheckedChanged="chkEmployeeSelect_CheckedChanged" Text="All Employees" />
                                                        <br />
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
					<asp:Label ID="lblbinTotalRecords" Font-Bold="true" runat="server" Text="<%$ Resources:Attendance,Total Records: 0 %>"></asp:Label>

                                                <div class="box-tools pull-right">
                                                    <button type="button" class="btn btn-box-tool" data-widget="collapse">
                                                        <i id="I2" runat="server" class="fa fa-plus"></i>
                                                    </button>
                                                </div>
                                            </div>
                                            <div class="box-body">
                                                <div class="col-lg-10">
                                                    <asp:Label ID="Label162" runat="server" Text="<%$ Resources:Attendance,Payment Option%>"></asp:Label>
                                                    <div class="input-group input-group-sm">
                                                        <asp:DropDownList ID="Ddl_Payment_Option" runat="server" CssClass="form-control" AutoPostBack="true"
                                                            OnSelectedIndexChanged="Ddl_Payment_Option_SelectedIndexChanged">
                                                        </asp:DropDownList>
                                                        <span class="input-group-btn">
                                                            <asp:LinkButton ID="Img_Payment_Option"
                                                                runat="server" CausesValidation="False"
                                                                OnClick="Img_Payment_Option_Click" ToolTip="<%$ Resources:Attendance,Refresh %>"><span class="fa fa-repeat"  style="font-size:25px; margin-left:5px;"></span></asp:LinkButton>

                                                        </span>

                                                    </div>
                                                    <br />

                                                </div>

                                                <div class="col-lg-3">
                                                    <asp:DropDownList ID="ddlbinFieldName" runat="server" CssClass="form-control">
                                                        <asp:ListItem Text="Employee Name" Value="Emp_Name"></asp:ListItem>
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
                                                    <asp:Panel ID="Panel1" runat="server" DefaultButton="btnbinbind">
                                                        <asp:TextBox ID="txtbinValue" placeholder="Search from Content" runat="server" CssClass="form-control"></asp:TextBox>
                                                    </asp:Panel>
                                                </div>
                                                <div class="col-lg-2">
                                                    <asp:LinkButton ID="btnbinbind" runat="server" CausesValidation="False"
                                                        OnClick="btnbinbind_Click" ToolTip="<%$ Resources:Attendance,Search %>"><span class="fa fa-search"  style="font-size:25px;"></span></asp:LinkButton>&nbsp;&nbsp;&nbsp;
                                                <asp:LinkButton ID="btnbinRefresh" runat="server" CausesValidation="False"
                                                    OnClick="btnbinRefresh_Click" ToolTip="<%$ Resources:Attendance,Refresh %>"><span class="fa fa-repeat"  style="font-size:25px;"></span></asp:LinkButton>&nbsp;&nbsp;&nbsp;

                                                <asp:LinkButton ID="ImgbtnSelectAll" Visible="false" runat="server" OnClick="ImgbtnSelectAll_Click"
                                                    ToolTip="<%$ Resources:Attendance, Select All %>" AutoPostBack="true"><span class="fas fa-th"  style="font-size:25px;"></span></asp:LinkButton>
                                                </div>


                                            </div>
                                        </div>
                                    </div>
                                </div>


                                <div class="box box-warning box-solid">

                                    <div class="box-body">
                                        <div class="col-md-4">
                                            <asp:CustomValidator ID="CustomValidator1" ValidationGroup="Save" runat="server"
                                                ErrorMessage="Please select at least one record." ClientValidationFunction="Grid_Validate"
                                                ForeColor="Red"></asp:CustomValidator>

                                        </div>



                                        <div class="row">
                                            <div class="col-md-12">
                                                <asp:Label ID="lblSelectedRecord" runat="server" Visible="false"></asp:Label>
                                                <div class="flow">
                                                    <asp:GridView CssClass="table-striped table-bordered table table-hover" ID="gvPayment" runat="server" AutoGenerateColumns="False" Width="100%"
                                                        AllowPaging="false" DataKeyNames="Emp_Id" PageSize='<%# int.Parse(Session["GridSize"].ToString()) %>'
                                                        OnPageIndexChanging="GvsalaryPlanBin_PageIndexChanging" OnSorting="GvsalaryPlanBin_Sorting"
                                                        AllowSorting="true">
                                                        <Columns>
                                                            <asp:TemplateField>
                                                                <HeaderTemplate>
                                                                    <asp:CheckBox ID="chkgvSelectAll" runat="server" AutoPostBack="true" OnCheckedChanged="chkgvSelectAll_CheckedChanged" />
                                                                </HeaderTemplate>
                                                                <ItemTemplate>
                                                                    <asp:CheckBox ID="chkgvSelect" runat="server" AutoPostBack="true" OnCheckedChanged="chkgvSelect_CheckedChanged" />
                                                                </ItemTemplate>
                                                                <ItemStyle HorizontalAlign="Center" />
                                                                <HeaderStyle HorizontalAlign="Center" />
                                                            </asp:TemplateField>



                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,Employee Code %>" SortExpression="Emp_Code">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblEmpCode" runat="server" Text='<%# Eval("Emp_Code") %>'></asp:Label>
                                                                    <asp:Label ID="lblEmpId" Visible="false" runat="server" Text='<%# Eval("Emp_Id") %>'></asp:Label>
                                                                </ItemTemplate>
                                                                <ItemStyle />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,Employee Name %>" SortExpression="Emp_Name">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblEmpName" runat="server" Text='<%# Eval("Emp_Name") %>'></asp:Label>

                                                                </ItemTemplate>
                                                                <ItemStyle />
                                                            </asp:TemplateField>

                                                            <asp:BoundField DataField="Emp_Name_L" HeaderText="<%$ Resources:Attendance,Employee Name Local%>"
                                                                SortExpression="Emp_Name_L" ItemStyle-Width="20%" />
                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,Date of Joining %>" HeaderStyle-HorizontalAlign="Center" SortExpression="DateOfjoining">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblDoj" runat="server" Text='<%# GetDate(Eval("DOJ").ToString()) %>'></asp:Label>
                                                                </ItemTemplate>
                                                                <ItemStyle />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,Opening Balance %>" SortExpression="Opening_Final">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblOpBalance" runat="server" Text='<%# Math.Abs(Convert.ToDecimal(Common.GetAmountDecimal(Eval("Opening_Final").ToString(),Session["DBConnection"].ToString(),Session["LocCurrencyId"].ToString()))) %>'></asp:Label>
                                                                    <asp:Label ID="lblOpBalanceType" runat="server" Text='<%# Eval("opening_balance_type") %>'></asp:Label>
                                                                </ItemTemplate>
                                                                <ItemStyle />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,Credit Amount %>" SortExpression="Credit_Final">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblCreditamt" runat="server" Text='<%#Common.GetAmountDecimal(Eval("Credit_Final").ToString(),Session["DBConnection"].ToString(),Session["LocCurrencyId"].ToString()) %>'></asp:Label>
                                                                </ItemTemplate>
                                                                <ItemStyle />
                                                            </asp:TemplateField>

                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,Debit Amount %>" SortExpression="Debit_Final">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lbldebitamt" runat="server" Text='<%# Common.GetAmountDecimal(Eval("Debit_Final").ToString(),Session["DBConnection"].ToString(),Session["LocCurrencyId"].ToString()) %>'></asp:Label>
                                                                </ItemTemplate>
                                                                <ItemStyle />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,Balance %>" SortExpression="Closing_final">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblBalance" runat="server" Text='<%# Math.Abs(Convert.ToDecimal(Common.GetAmountDecimal(Eval("Closing_final").ToString(),Session["DBConnection"].ToString(),Session["LocCurrencyId"].ToString()))) %>'></asp:Label>
                                                                    <asp:Label ID="lblBalanceType" runat="server" Text='<%# Eval("balance_type") %>'></asp:Label>
                                                                </ItemTemplate>
                                                                <ItemStyle />
                                                            </asp:TemplateField>

                                                            <asp:TemplateField>
                                                                <ItemTemplate>
                                                                    <asp:TextBox ID="txtPayment" runat="server" CssClass="form-control" Text='<%# Common.GetAmountDecimal(Eval("Closing_final").ToString(),Session["DBConnection"].ToString(),Session["LocCurrencyId"].ToString()) %>' OnTextChanged="txtPayment_OnTextChanged" AutoPostBack="true"></asp:TextBox>
                                                                    <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender3" runat="server" Enabled="True"
                                                                        TargetControlID="txtPayment" ValidChars="0,1,2,3,4,5,6,7,8,9,.,-">
                                                                    </cc1:FilteredTextBoxExtender>


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
                                        <br />
                                        <div class="col-md-12">
                                            <%--<div class="col-md-6" >
                                                <asp:Label ID="Label8" runat="server" Text="<%$ Resources:Attendance,For Debit Account %>"></asp:Label>
                                                 <a style="color: Red">*</a>
                                                            <asp:RequiredFieldValidator EnableClientScript="true" Style="float: right;" runat="server"
                                                                ID="RequiredFieldValidator2" ValidationGroup="Save" Display="Dynamic" SetFocusOnError="true"
                                                                ControlToValidate="txtpaymentdebitaccount" ErrorMessage="Enter Debit Account"></asp:RequiredFieldValidator>
                                                <asp:TextBox ID="txtpaymentdebitaccount" runat="server" CssClass="form-control" BackColor="#eeeeee"
                                                    AutoPostBack="true" OnTextChanged="txtcmnAccount_textChnaged"></asp:TextBox>
                                                <cc1:AutoCompleteExtender ID="AutoCompleteExtender1" runat="server" DelimiterCharacters=""
                                                    Enabled="True" ServiceMethod="GetCompletionListAccountName" ServicePath="" CompletionInterval="100"
                                                    MinimumPrefixLength="1" CompletionSetCount="1" TargetControlID="txtpaymentdebitaccount"
                                                    UseContextKey="True"  
                                                      CompletionListCssClass="autoCompleteList" CompletionListItemCssClass="autoCompleteListItem" CompletionListHighlightedItemCssClass="autoCompleteSelectedListItem" OnClientShown="resetPosition">
                                                </cc1:AutoCompleteExtender>
                                                <br />
                                            </div>--%>
                                            <div class="col-md-6">
                                                <asp:Label ID="Label9" runat="server" Text="<%$ Resources:Attendance,For Credit Account %>"></asp:Label>
                                                <a style="color: Red">*</a>
                                                <asp:RequiredFieldValidator EnableClientScript="true" Style="float: right;" runat="server"
                                                    ID="RequiredFieldValidator1" ValidationGroup="Save" Display="Dynamic" SetFocusOnError="true"
                                                    ControlToValidate="txtpaymentCreditaccount" ErrorMessage="Enter Credit Account"></asp:RequiredFieldValidator>
                                                <asp:TextBox ID="txtpaymentCreditaccount" runat="server" BackColor="#eeeeee"
                                                    AutoPostBack="true" OnTextChanged="txtcmnAccount_textChnaged" CssClass="form-control"></asp:TextBox>
                                                <cc1:AutoCompleteExtender ID="AutoCompleteExtender2" runat="server" DelimiterCharacters=""
                                                    Enabled="True" ServiceMethod="GetCompletionListAccountName" ServicePath="" CompletionInterval="100"
                                                    MinimumPrefixLength="1" CompletionSetCount="1" TargetControlID="txtpaymentCreditaccount"
                                                    UseContextKey="True"
                                                    CompletionListCssClass="autoCompleteList" CompletionListItemCssClass="autoCompleteListItem" CompletionListHighlightedItemCssClass="autoCompleteSelectedListItem" OnClientShown="resetPosition">
                                                </cc1:AutoCompleteExtender>
                                                <br />
                                            </div>
                                            <div class="col-md-6">
                                                <asp:Label ID="Label2" runat="server" Text="<%$ Resources:Attendance,Total Amount%>"></asp:Label>
                                                <asp:TextBox ID="Txt_Total_Amount" ReadOnly="true" CssClass="form-control" Text="0.00" runat="server"></asp:TextBox>
                                                <asp:HiddenField ID="Hdn_Total_Amount" Value="0.00" runat="server" />
                                            </div>
                                        </div>
                                        <div class="col-md-12">
                                            <div class="col-md-6">
                                                <br />
                                                <asp:CheckBox ID="Chk_Cheque_Payment" Style="width: 37px;" AutoPostBack="true" OnCheckedChanged="Chk_Cheque_Payment_CheckedChanged" runat="server" Text="Cheque Payment" />

                                                <br />
                                            </div>
                                            <div class="col-md-6">
                                                <asp:Label ID="lblVoucherDate" runat="server" Text="Voucher Date"> </asp:Label>
                                                <asp:TextBox ID="txtVoucherDate" runat="server" CssClass="form-control"></asp:TextBox>
                                                <cc1:CalendarExtender ID="CalendarExtender1" runat="server" TargetControlID="txtVoucherDate" Format="dd-MMM-yyyy" OnClientShown="showCalendar"></cc1:CalendarExtender>
                                            </div>
                                            <div class="col-md-12">
                                                <asp:Label ID="Label8" runat="server" Text="Narration"></asp:Label>
                                                <asp:TextBox ID="txtNarration" CssClass="form-control" Text="Employee Salary Payment for the month of " runat="server"></asp:TextBox>
                                                <br />
                                            </div>
                                            <div id="trCheque1" runat="server" visible="false">
                                                <div class="col-md-6">
                                                    <asp:Label ID="lblChequeIssueDate" runat="server" Text="<%$ Resources:Attendance,Cheque Issue Date%>" />
                                                    <a style="color: Red">*</a>
                                                    <asp:RequiredFieldValidator EnableClientScript="true" Style="float: right;" runat="server"
                                                        ID="RequiredFieldValidator2" ValidationGroup="Not_Save" Display="Dynamic" SetFocusOnError="true"
                                                        ControlToValidate="txtChequeIssueDate" ErrorMessage="Enter Cheque Issue Date"></asp:RequiredFieldValidator>
                                                    <asp:TextBox ID="txtChequeIssueDate" runat="server" ReadOnly="true" CssClass="form-control" />
                                                    <cc1:CalendarExtender OnClientShown="showCalendar" ID="CalendarExtender_txtchequeissuedate" runat="server" TargetControlID="txtChequeIssueDate"
                                                        Format="dd/MMM/yyyy" />
                                                    <br />
                                                </div>
                                                <div class="col-md-6">
                                                    <asp:Label ID="lblChequeNo" runat="server" Text="<%$ Resources:Attendance,Cheque No.%>" />
                                                    <a style="color: Red">*</a>
                                                    <asp:RequiredFieldValidator EnableClientScript="true" Style="float: right;" runat="server"
                                                        ID="RequiredFieldValidator4" ValidationGroup="Not_Save" Display="Dynamic" SetFocusOnError="true"
                                                        ControlToValidate="txtChequeNo" ErrorMessage="Enter Cheque Number"></asp:RequiredFieldValidator>
                                                    <asp:TextBox ID="txtChequeNo" runat="server" MaxLength="12" CssClass="form-control" />
                                                    <asp:RegularExpressionValidator ID="RangeValidator1" runat="server" ValidationGroup="Save"
                                                        ControlToValidate="txtChequeNo" Display="Dynamic"
                                                        ErrorMessage="Enter Numbers Only"
                                                        ValidationExpression="^\d+$"
                                                        SetFocusOnError="True"></asp:RegularExpressionValidator>
                                                    <br />
                                                </div>
                                            </div>
                                            <%--<div class="col-md-6">
                                                <div class="input-group">
                                                    <asp:CheckBox ID="Chk_Payment_Update" Style="width: 37px;" CssClass="form-control" AutoPostBack="true" OnCheckedChanged="Chk_Payment_Update_CheckedChanged" runat="server" />
                                                    <div class="input-group-addon" style="width: 100%; text-align: left">
                                                        <asp:Label ID="label7" runat="server" Text="Update Employee Payment Option"></asp:Label>
                                                    </div>
                                                </div>
                                                <br />
                                            </div>                                            --%>
                                        </div>

                                        <div class="col-md-12">
                                            <div id="Div_Additional_Info" runat="server" class="box box-primary collapsed-box">
                                                <div class="box-header with-border">
                                                    <h3 class="box-title">Update Employee Payment Option</h3>
                                                    <div class="box-tools pull-right">
                                                        <button type="button" class="btn btn-box-tool" data-widget="collapse">
                                                            <i id="Btn_Div_Additional_Info" runat="server" class="fa fa-plus"></i>
                                                        </button>
                                                    </div>
                                                </div>
                                                <div class="box-body">
                                                    <div class="form-group">
                                                        <div class="col-md-6">
                                                            <asp:Label ID="Label164" runat="server" Text="<%$ Resources:Attendance,Salary Payment Option%>"></asp:Label>
                                                            <a style="color: Red">*</a>
                                                            <asp:RequiredFieldValidator EnableClientScript="true" Style="float: right;" runat="server"
                                                                ID="RequiredFieldValidator3" ValidationGroup="Update" Display="Dynamic" SetFocusOnError="true"
                                                                ControlToValidate="Txt_Salary_Payment_Option_M" ErrorMessage="Enter Salary Payment Option"></asp:RequiredFieldValidator>
                                                            <div class="input-group">
                                                                <asp:TextBox onkeydown="return (event.keyCode!=13);" ID="Txt_Salary_Payment_Option_M" runat="server" CssClass="form-control" BackColor="#eeeeee"
                                                                    AutoPostBack="true" OnTextChanged="Txt_Salary_Payment_Option_TextChanged"></asp:TextBox>
                                                                <cc1:AutoCompleteExtender ID="AutoCompleteExtender5" runat="server" DelimiterCharacters=""
                                                                    Enabled="True" ServiceMethod="GetCompletionListAccountName_Payment" ServicePath="" CompletionInterval="100"
                                                                    MinimumPrefixLength="1" CompletionSetCount="1" TargetControlID="Txt_Salary_Payment_Option_M"
                                                                    UseContextKey="True" CompletionListCssClass="autoCompleteList" CompletionListItemCssClass="autoCompleteListItem" CompletionListHighlightedItemCssClass="autoCompleteSelectedListItem" OnClientShown="resetPosition">
                                                                </cc1:AutoCompleteExtender>
                                                                <div class="input-group-btn">
                                                                    <asp:Button ID="Btn_Payment_Update" ValidationGroup="Update" OnClick="Btn_Payment_Update_Click" Text="Update" runat="server" CssClass="btn btn-info" />
                                                                </div>
                                                            </div>
                                                            <br />
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>


                                        <div class="col-md-12" style="text-align: center">
                                            <br />
                                            <asp:Button ID="Btn_Save" runat="server" ValidationGroup="Save" Text="Pay Salary" Visible="false"
                                                class="btn btn-success" OnClick="Btn_Save_Click" />
                                            <%--<cc1:ConfirmButtonExtender ID="ConfirmButtonExtender1" runat="server" ConfirmText="Are you sure to pay Salary ?"
                                                TargetControlID="Btn_Save">
                                            </cc1:ConfirmButtonExtender>--%>


                                            <asp:Button ID="Btn_Cancel" runat="server" Text="<%$ Resources:Attendance,Cancel%>"
                                                class="btn btn-primary" OnClick="Btn_Cancel_Click" />
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
    <div class="modal fade" id="Payment_Account_Update" tabindex="-1" role="dialog" aria-labelledby="Payment_Account_UpdateLabel" aria-hidden="true">
        <div class="modal-dialog modal-lg">
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal">
                        <span aria-hidden="true">&times;</span><span class="sr-only">Close</span></button>
                    <h4 class="modal-title" id="Payment_Account_UpdateLabel">Salary Payment</h4>
                </div>
                <div class="modal-body">
                    <asp:UpdatePanel ID="Update_Modal" runat="server">
                        <ContentTemplate>
                            <div class="row">
                                <div class="col-md-12">
                                    <div class="box box-primary">
                                        <div class="box-body">
                                            <div class="form-group">
                                                <div class="col-md-12">
                                                    <%--<asp:Label ID="Label164" runat="server" Text="<%$ Resources:Attendance,Salary Payment Option%>"></asp:Label>
                                                    <asp:TextBox onkeydown="return (event.keyCode!=13);" ID="Txt_Salary_Payment_Option_M" runat="server" CssClass="form-control" BackColor="#eeeeee"
                                                        AutoPostBack="true" OnTextChanged="Txt_Salary_Payment_Option_TextChanged"></asp:TextBox>
                                                    <cc1:AutoCompleteExtender ID="AutoCompleteExtender5" runat="server" DelimiterCharacters=""
                                                        Enabled="True" ServiceMethod="GetCompletionListAccountName_Payment" ServicePath="" CompletionInterval="100"
                                                        MinimumPrefixLength="1" CompletionSetCount="1" TargetControlID="Txt_Salary_Payment_Option_M"
                                                        UseContextKey="True" CompletionListCssClass="autoCompleteList" CompletionListItemCssClass="autoCompleteListItem" CompletionListHighlightedItemCssClass="autoCompleteSelectedListItem" OnClientShown="resetPosition">
                                                    </cc1:AutoCompleteExtender>--%>
                                                    <br />
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
                            <button type="button" class="btn btn-danger" data-dismiss="modal">
                                Close</button>
                            <button type="button" class="btn btn-primary">
                                Update</button>
                        </ContentTemplate>
                    </asp:UpdatePanel>
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

    <div class="modal fade" id="ReportSystem" tabindex="-1" role="dialog" aria-labelledby="ReportSystem_Web_Control"
        aria-hidden="true">
        <div class="modal-dialog modal-lg">
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal">
                        <span aria-hidden="true">&times;</span><span class="sr-only">Close</span></button>
                    <h4 class="modal-title" id="ReportSystem_Web_Control">Report System
                    </h4>
                </div>
                <div class="modal-body">
                    <RS:ReportSystem runat="server" ID="reportSystem" />
                </div>
                <div class="modal-footer">
                </div>

            </div>
        </div>
    </div>
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

        function Grid_Validate(sender, args) {

            var gridView = document.getElementById("<%=gvPayment.ClientID %>");
            var checkBoxes = gridView.getElementsByTagName("input");
            for (var i = 0; i < checkBoxes.length; i++) {
                if (checkBoxes[i].type == "checkbox" && checkBoxes[i].checked) {
                    args.IsValid = true;
                    return;
                }
            }
            args.IsValid = false;

        }
        function Show_Modal_Salary() {
            document.getElementById('<%= Btn_Salary_Modal.ClientID %>').click();
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
        function Div_Additional_Info_Open() {
            $("#Btn_Div_Additional_Info").removeClass("fa fa-plus");
            $("#Div_Additional_Info").removeClass("box box-primary collapsed-box");

            $("#Btn_Div_Additional_Info").addClass("fa fa-minus");
            $("#Div_Additional_Info").addClass("box box-primary");
        }
        function getReportData(transId) {
            $("#prgBar").css("display", "block");
            document.getElementById('<%= reportSystem.FindControl("hdnTransId").ClientID %>').value = transId;
            setReportData();
        }
    </script>
    <script src="../Script/ReportSystem.js"></script>
</asp:Content>
