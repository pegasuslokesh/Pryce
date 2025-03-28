<%@ Page Title="" Language="C#" MasterPageFile="~/ERPMaster.master" AutoEventWireup="true"
    CodeFile="EditPayroll.aspx.cs" Inherits="HR_EditPayroll" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="HeaderContent" runat="Server">
    <h1>
        <i class="far fa-edit"></i>
        <asp:Label ID="lblHeader" runat="server" Text="<%$ Resources:Attendance,Edit Payroll%>"></asp:Label>
    </h1>
    <ol class="breadcrumb">
        <li><a href="#"><i class="fa fa-dashboard"></i>
            <asp:Label ID="Label6" runat="server" Text="<%$ Resources:Attendance,HR%>"></asp:Label></a></li>
        <li><a href="#"><i class="fa fa-dashboard"></i>
            <asp:Label ID="Label10" runat="server" Text="<%$ Resources:Attendance,Payroll%>"></asp:Label></a></li>
        <li class="active">
            <asp:Label ID="Label5" runat="server" Text="<%$ Resources:Attendance,Edit Payroll%>"></asp:Label></li>
    </ol>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="Server">
    <div>
        <asp:UpdatePanel ID="Update_Form" runat="server">
            <Triggers>
             <asp:PostBackTrigger ControlID="btnExport" />
            </Triggers>
            <ContentTemplate>
                <div id="Div_Head" runat="server" class="row">
                    <div class="col-md-12">
                        <div class="box box-primary">
                            <div class="box-body">
                                <div class="form-group">
                                    <div style="display: none;" class="col-md-12">
                                        <div style="text-align: center;" class="form-control">
                                            <asp:RadioButton ID="RbtnListView" Checked="true" Font-Bold="true" runat="server"
                                                Text="<%$ Resources:Attendance,List View %>" GroupName="View" />&nbsp;
                                            <asp:RadioButton ID="RbtnFormView" runat="server" Text="<%$ Resources:Attendance,Form View %>"
                                                Font-Bold="true" Style="margin-left: 50px;" GroupName="View" />
                                        </div>
                                    </div>
                                    <div class="col-md-6">
                                        <asp:Label ID="Label1" runat="server" Text="<%$ Resources:Attendance,Location %>"></asp:Label>
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
                                                <asp:LinkButton ID="ImgbuttonallRefresh"
                                                    runat="server" CausesValidation="False"
                                                    OnClick="btnAllRefresh_Click" ToolTip="<%$ Resources:Attendance,Refresh %>"><span class="fa fa-repeat"  style="font-size:25px; margin-left:10px;"></span></asp:LinkButton>&nbsp;&nbsp;&nbsp;
                                            </div>
                                        </div>
                                        <br />
                                    </div>
                                    <div style="display: none;" class="col-md-12">
                                        <div style="text-align: center;" class="form-control">
                                            <asp:RadioButton ID="rbtnGroup" OnCheckedChanged="EmpGroup_CheckedChanged" runat="server"
                                                Text="<%$ Resources:Attendance,Group %>" GroupName="EmpGroup" AutoPostBack="true" />
                                            <asp:RadioButton ID="rbtnEmp" Style="margin-left: 25px;" runat="server" AutoPostBack="true"
                                                Text="<%$ Resources:Attendance,Employee %>" GroupName="EmpGroup" OnCheckedChanged="EmpGroup_CheckedChanged" />
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
                <div runat="server" visible="false" id="Div_Group" class="row">
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
                                    <asp:CustomValidator ID="CustomValidator1" ValidationGroup="Save" runat="server"
                                        ErrorMessage="Please select at least one record." ClientValidationFunction="Grid_Validate"></asp:CustomValidator>
                                    <asp:ListBox ID="lbxGroup" runat="server" Height="211px" SelectionMode="Multiple"
                                        AutoPostBack="true" OnSelectedIndexChanged="lbxGroup_SelectedIndexChanged" CssClass="form-control"></asp:ListBox>
                                    <asp:Label ID="lblEmp" runat="server" Visible="false"></asp:Label>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
                <div id="Emp_Payroll_upload" runat="server">
                    <div class="col-md-12">
                        <div class="box box-warning">
                            <div class="box-header with-border">
                                <h3 class="box-title">Upload Payroll</h3>
                                <div class="box-tools pull-right">
                                    <button type="button" class="btn btn-box-tool" data-widget="collapse">
                                        <i class="fa fa-minus"></i>
                                    </button>
                                </div>
                            </div>
                            <div class="box-body">

                                   <asp:UpdatePanel ID="Update_Upload" runat="server">
       <Triggers>
           <asp:PostBackTrigger ControlID="btnExport" />
       </Triggers>
       <ContentTemplate>
           <div class="row">
               <div class="col-md-12">
                   <div class="box box-primary">
                       <div class="box-body">
                           <div class="form-group">
                               <div class="col-md-6">
                                   <asp:Label runat="server" Text="Browse Excel File" ID="Label169"></asp:Label>
                                   <div class="input-group" style="width: 100%;">
                                       <cc1:AsyncFileUpload ID="fileLoad"
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
                                           <asp:LinkButton ID="FUExcel_Img_Right" runat="server" Width="30px" Height="30px" Style="display: none"><i class="fa fa-check" style="font-size:30px;color:#22cb33"></i></asp:LinkButton>
                                           <asp:LinkButton ID="FUExcel_Img_Wrong" runat="server" Width="30px" Height="30px" Style="display: none"><i class="fa fa-remove" style="font-size:30px"></i></asp:LinkButton>
                                           <asp:Image ID="FUExcel_ImgLoader" runat="server" ImageUrl="../Images/loader.gif" />
                                       </div>
                                   </div>
                               </div>
                               <div class="col-md-6">
                                   <br />
                                   <asp:Button ID="btnUpload" runat="server" Text="Upload" OnClick="btnUploadExcel_Click" CssClass="btn btn-primary" />
                                   &nbsp; 
                                       <%--                                                <asp:linkbutton id="btnexport" runat="server" onclick="btnexportdata_clcik" tooltip="export grid data">
<span class="fa fa-file"  style="font-size:25px;"></span>
                                                    </asp:linkbutton>--%>
                                   <asp:Button ID="btnexport" runat="server" OnClick="btnExportData_Clcik" CssClass="btn btn-primary"  Text="Download" ToolTip="Export grid data" />
                               </div>
                               <div class="row" id="uploadOb" runat="server" visible="false">
                                   <br />
                                   <div class="col-md-6" style="text-align: left">
                                       <asp:RadioButton ID="rbtnupdall" Style="margin-left: 20px; margin-right: 20px;" runat="server" GroupName="upd" AutoPostBack="true" Checked="true" OnCheckedChanged="rbtnupd_CheckedChanged" Text="All" />
                                       <asp:RadioButton ID="rbtnupdValid" Style="margin-left: 20px; margin-right: 20px;" runat="server" GroupName="upd" AutoPostBack="true" Text="Valid" OnCheckedChanged="rbtnupd_CheckedChanged" />
                                       <asp:RadioButton ID="rbtnupdInValid" Style="margin-left: 20px; margin-right: 20px;" runat="server" GroupName="upd" AutoPostBack="true" Text="Invalid" OnCheckedChanged="rbtnupd_CheckedChanged" />
                                   </div>

                                   <div class="col-md-6" style="text-align: right;">
                                       <asp:Label ID="lbltotaluploadRecord" runat="server"></asp:Label>
                                   </div>
                                   <div class="col-md-12">
                                       <div style="overflow: auto; max-height: 300px;">
                                           <asp:GridView CssClass="table-striped table-bordered table table-hover" AutoGenerateColumns="False" ID="gvImport" runat="server" Width="100%">
                                               <Columns>
                                                   <asp:TemplateField HeaderText="Emp_Code" SortExpression="Emp_Code">
                                                       <ItemTemplate>
                                                           <asp:Label ID="lblgvImportEmp_Code" runat="server" Text='<%# Eval("Emp_Code") %>'></asp:Label>
                                                       </ItemTemplate>
                                                       <ItemStyle />
                                                   </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="emp_name" SortExpression="emp_name">
                                                       <ItemTemplate>
                                                           <asp:Label ID="lblgvImportemp_name" runat="server" Text='<%# Eval("emp_name") %>'></asp:Label>
                                                       </ItemTemplate>
                                                       <ItemStyle />
                                                   </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="worked_min_salary" SortExpression="worked_min_salary">
                                                       <ItemTemplate>
                                                           <asp:Label ID="lblgvImportworked_min_salary" runat="server" Text='<%# Eval("worked_min_salary") %>'></asp:Label>
                                                       </ItemTemplate>
                                                       <ItemStyle />
                                                   </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="normal_ot_min_salary" SortExpression="normal_ot_min_salary">
                                                       <ItemTemplate>
                                                           <asp:Label ID="lblgvImportnormal_ot_min_salary" runat="server" Text='<%# Eval("normal_ot_min_salary") %>'></asp:Label>
                                                       </ItemTemplate>
                                                       <ItemStyle />
                                                   </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="week_off_ot_min_salary" SortExpression="week_off_ot_min_salary">
                                                       <ItemTemplate>
                                                           <asp:Label ID="lblgvImportweek_off_ot_min_salary" runat="server" Text='<%# Eval("week_off_ot_min_salary") %>'></asp:Label>
                                                       </ItemTemplate>
                                                       <ItemStyle />
                                                   </asp:TemplateField>
                                                   <asp:TemplateField HeaderText="holiday_ot_min_salary" SortExpression="holiday_ot_min_salary">
                                                       <ItemTemplate>
                                                           <asp:Label ID="lblgvImportholiday_ot_min_salary" runat="server" Text='<%# Eval("holiday_ot_min_salary") %>'></asp:Label>
                                                       </ItemTemplate>
                                                       <ItemStyle />
                                                   </asp:TemplateField>
                                                   <asp:TemplateField HeaderText="leave_days_salary" SortExpression="leave_days_salary">
                                                       <ItemTemplate>
                                                           <asp:Label ID="lblgvImportleave_days_salary" runat="server" Text='<%# Eval("leave_days_salary") %>'></asp:Label>
                                                       </ItemTemplate>
                                                       <ItemStyle />
                                                   </asp:TemplateField>
                                                   <asp:TemplateField HeaderText="week_off_salary" SortExpression="week_off_salary">
                                                       <ItemTemplate>
                                                           <asp:Label ID="lblgvImportweek_off_salary" runat="server" Text='<%# Eval("week_off_salary") %>'></asp:Label>
                                                       </ItemTemplate>
                                                       <ItemStyle />
                                                   </asp:TemplateField>
                                                   <asp:TemplateField HeaderText="holidays_salary" SortExpression="holidays_salary">
                                                       <ItemTemplate>
                                                           <asp:Label ID="lblgvImportholidays_salary" runat="server" Text='<%# Eval("holidays_salary") %>'></asp:Label>
                                                       </ItemTemplate>
                                                       <ItemStyle />
                                                   </asp:TemplateField>
                                                   <asp:TemplateField HeaderText="late_min_penalty" SortExpression="late_min_penalty">
                                                       <ItemTemplate>
                                                           <asp:Label ID="lblgvImportlate_min_penalty" runat="server" Text='<%# Eval("late_min_penalty") %>'></asp:Label>
                                                       </ItemTemplate>
                                                       <ItemStyle />
                                                   </asp:TemplateField>
                                                   <asp:TemplateField HeaderText="early_min_penalty" SortExpression="early_min_penalty">
                                                       <ItemTemplate>
                                                           <asp:Label ID="lblgvImportearly_min_penalty" runat="server" Text='<%# Eval("early_min_penalty") %>'></asp:Label>
                                                       </ItemTemplate>
                                                       <ItemStyle />
                                                   </asp:TemplateField>
                                                   <asp:TemplateField HeaderText="patial_violation_penalty" SortExpression="patial_violation_penalty">
                                                       <ItemTemplate>
                                                           <asp:Label ID="lblgvImportpatial_violation_penalty" runat="server" Text='<%# Eval("patial_violation_penalty") %>'></asp:Label>
                                                       </ItemTemplate>
                                                       <ItemStyle />
                                                   </asp:TemplateField>
                                                   <asp:TemplateField HeaderText="MonthName" SortExpression="MonthName">
                                                       <ItemTemplate>
                                                           <asp:Label ID="lblgvImportMonthName" runat="server" Text='<%# Eval("MonthName") %>'></asp:Label>
                                                       </ItemTemplate>
                                                       <ItemStyle />
                                                   </asp:TemplateField>
                                                   <asp:TemplateField HeaderText="Gross_Salary" SortExpression="Gross_Salary">
                                                       <ItemTemplate>
                                                           <asp:Label ID="lblgvImportGross_Salary" runat="server" Text='<%# Eval("Gross_Salary") %>'></asp:Label>
                                                       </ItemTemplate>
                                                       <ItemStyle />
                                                   </asp:TemplateField>
                                                
                                                   <asp:TemplateField HeaderText="BasicSalary" SortExpression="BasicSalary">
                                                       <ItemTemplate>
                                                           <asp:Label ID="lblgvImportBasicSalary" runat="server" Text='<%# Eval("BasicSalary") %>'></asp:Label>
                                                       </ItemTemplate>
                                                       <ItemStyle />
                                                   </asp:TemplateField>
                                                   <asp:TemplateField HeaderText="Total Gross Salary" SortExpression="TotalGrossSalary">
                                                       <ItemTemplate>
                                                           <asp:Label ID="lblgvTotalGrossSalary" runat="server" Text='<%# Eval("TotalGrossSalary") %>'></asp:Label>
                                                       </ItemTemplate>
                                                       <ItemStyle />
                                                   </asp:TemplateField>
                                                   <asp:TemplateField HeaderText="Message" SortExpression="Messagey">
                                                       <ItemTemplate>
                                                           <asp:Label ID="lblgvMessage" runat="server" Text='<%# Eval("Message") %>'></asp:Label>
                                                       </ItemTemplate>
                                                       <ItemStyle />
                                                   </asp:TemplateField>
                                                   <asp:TemplateField HeaderText="Is Valid" SortExpression="IsValid">
                                                       <ItemTemplate>
                                                           <asp:Label ID="lblgvImportIsValid" runat="server" Text='<%# Eval("IsValid") %>'></asp:Label>
                                                       </ItemTemplate>
                                                       <ItemStyle />
                                                   </asp:TemplateField>
                                               </Columns>

                                               <PagerStyle CssClass="pagination-ys" />
                                           </asp:GridView>
                                       </div>
                                       <br />
                                   </div>                                  
                                   <div class="col-md-12" style="text-align:center" >
                                       <asp:Button ID="btnSaveUpload" runat="server" Text="Save" CssClass="btn btn-success" OnClick="btnUploadSave_Click" />
                                       &nbsp;
                                       <asp:Button ID="btnResetUpload" runat="server" Text="Reset" CssClass="btn btn-danger" OnClick="btnUploadReset_Click" />
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
                                <div class="col-md-12">
                                    <div class="row">
                                        <div class="col-md-12">
                                            <div id="Div1" runat="server" class="box box-info collapsed-box">
                                                <div class="box-header with-border">
                                                    <h3 class="box-title">
                                                        <asp:Label ID="Label49" runat="server" Text="Advance Search"></asp:Label></h3>
                                                    &nbsp;&nbsp;|&nbsp;&nbsp;
					<asp:Label ID="lblTotalRecordsPayroll" Style="font-size: large;" Font-Bold="true" runat="server" Text="<%$ Resources:Attendance,Total Records: 0 %>"></asp:Label>

                                                    <div class="box-tools pull-right">
                                                        <button type="button" class="btn btn-box-tool" data-widget="collapse">
                                                            <i id="I1" runat="server" class="fa fa-plus"></i>
                                                        </button>
                                                    </div>
                                                </div>
                                                <div class="box-body">
                                                    <div class="col-lg-2">
                                                        <asp:DropDownList ID="ddlField1" runat="server" CssClass="form-control">
                                                            <asp:ListItem Text="<%$ Resources:Attendance,Employee Code %>" Value="Emp_Code"></asp:ListItem>
                                                            <asp:ListItem Text="<%$ Resources:Attendance,Employee Name %>" Value="Emp_Name" Selected="True"></asp:ListItem>
                                                            <asp:ListItem Text="<%$ Resources:Attendance,Employee Name Local %>" Value="Emp_Name_L"></asp:ListItem>
                                                        </asp:DropDownList>
                                                    </div>
                                                    <div class="col-lg-2">
                                                        <asp:HiddenField runat="server" ID="hdnCanEdit" />
                                                        <asp:HiddenField runat="server" ID="hdnCanDelete" />
                                                        <asp:DropDownList ID="ddlOption1" runat="server" class="form-control">
                                                            <asp:ListItem Text="<%$ Resources:Attendance,--Select-- %>"></asp:ListItem>
                                                            <asp:ListItem Text="<%$ Resources:Attendance,Equal %>"></asp:ListItem>
                                                            <asp:ListItem Selected="True" Text="<%$ Resources:Attendance,Contains %>"></asp:ListItem>
                                                            <asp:ListItem Text="<%$ Resources:Attendance,Like %>"></asp:ListItem>
                                                        </asp:DropDownList>
                                                    </div>
                                                    <div class="col-lg-4">
                                                        <asp:Panel ID="Panel1" runat="server" DefaultButton="imgBtnPayrollBind">
                                                            <asp:TextBox ID="txtValue1" placeholder="Search from Content" CssClass="form-control" runat="server" class="form-control"></asp:TextBox>
                                                        </asp:Panel>
                                                    </div>
                                                    <div class="col-lg-2">
                                                        <asp:LinkButton ID="imgBtnPayrollBind" runat="server"
                                                            CausesValidation="False" OnClick="btnbind_Click"
                                                            ToolTip="<%$ Resources:Attendance,Search %>"><span class="fa fa-search"  style="font-size:25px;"></span></asp:LinkButton>&nbsp;&nbsp;&nbsp;
                                                        <asp:LinkButton ID="ImageButton2" runat="server" CausesValidation="False"
                                                            OnClick="btnRefresh_Click" ToolTip="<%$ Resources:Attendance,Refresh %>"><span class="fa fa-repeat"  style="font-size:25px;"></span></asp:LinkButton>&nbsp;&nbsp;&nbsp;

                                                        <asp:LinkButton ID="ImgButtonDelete" runat="server" CausesValidation="False" Visible='<%# hdnCanDelete.Value=="true"?true:false%>'
                                                            OnClick="ImgButtonDelete_Click" ToolTip="<%$ Resources:Attendance,Delete %>"><span class="fa fa-remove"  style="font-size:30px;"></span></asp:LinkButton>&nbsp;&nbsp;&nbsp;
                                                        <cc1:ConfirmButtonExtender ID="confirm2" runat="server" ConfirmText="<%$ Resources:Attendance,Are you sure you want to delete the record?%>"
                                                            TargetControlID="ImgButtonDelete">
                                                        </cc1:ConfirmButtonExtender>

                                                        <asp:LinkButton ID="ImageButton6" runat="server" OnClick="ImgbtnSelectAll_ClickPayroll"
                                                            ToolTip="<%$ Resources:Attendance, Select All %>" AutoPostBack="true"><span class="fas fa-th"  style="font-size:25px;"></span></asp:LinkButton>

                                                        &nbsp;
                                                      

                                                    </div>
                                                    <div class="col-lg-2">
                                                             
                                                  
                                                        <asp:Button ID="btnpostpayroll" runat="server" Visible="false" Style="margin-top: 5px;" CssClass="btn btn-primary" Text="<%$ Resources:Attendance,Post Payroll %>"
                                                            ValidationGroup="Save" OnClick="btnpostpayroll1_Click" />
                                                        <cc1:ConfirmButtonExtender ID="ConfirmBtnApproval" runat="server" TargetControlID="btnpostpayroll"
                                                            ConfirmText="<%$ Resources:Attendance,Are you sure to Post Payroll ? %>">
                                                        </cc1:ConfirmButtonExtender>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>

                                    <div class="box box-warning box-solid" <%= gvEmpEditPayroll.Rows.Count>0?"style='display:block'":"style='display:none'"%>>
                                        <div class="box-body">
                                            <div class="row">
                                                <div class="col-md-12">
                                                    <div class="flow">
                                                        <asp:CustomValidator ID="CustomValidator6" ValidationGroup="Save" runat="server"
                                                            ErrorMessage="Please select at least one record." ClientValidationFunction="Grid_Validate"></asp:CustomValidator>
                                                        <asp:Label ID="lblSelectRecd" runat="server" Visible="false"></asp:Label>
                                                        <asp:GridView CssClass="table-striped table-bordered table table-hover" ID="gvEmpEditPayroll" runat="server" AllowPaging="True" AutoGenerateColumns="False"
                                                            OnPageIndexChanging="gvEmpEditPayroll_PageIndexChanging" OnSorting="gvEmpEditPayroll_Sorting"
                                                            AllowSorting="true" Width="100%" DataKeyNames="Emp_Id" PageSize='<%# int.Parse(Session["GridSize"].ToString()) %>'>
                                                            <Columns>
                                                                <asp:TemplateField>
                                                                    <ItemTemplate>
                                                                        <asp:CheckBox ID="chkgvSelect" runat="server" />
                                                                    </ItemTemplate>
                                                                    <HeaderTemplate>
                                                                        <asp:CheckBox ID="chkgvSelectAll" runat="server" OnCheckedChanged="chkgvSelectAll_CheckedChangedPayroll"
                                                                            AutoPostBack="true" />
                                                                    </HeaderTemplate>
                                                                    <ItemStyle HorizontalAlign="Center" />
                                                                </asp:TemplateField>

                                                                <asp:TemplateField>
                                                                    <ItemTemplate>
                                                                        <asp:LinkButton ID="imgBtnEmpEdit" runat="server"
                                                                            OnCommand="btnEmpEdit_Command" ToolTip="<%$ Resources:Attendance,Edit%>" CommandArgument='<%# Eval("Emp_Id") %>'><i class="fa fa-pencil"></i> </asp:LinkButton>

                                                                        <asp:LinkButton ID="imgBtnDetail" runat="server"
                                                                            OnCommand="imgBtnDetail_Command" ToolTip="<%$ Resources:Attendance,Detail%>" CommandArgument='<%# Eval("Emp_Id") %>'><i class="fa fa-search"></i> </asp:LinkButton>
                                                                    </ItemTemplate>
                                                                    <ItemStyle HorizontalAlign="Center" />
                                                                </asp:TemplateField>



                                                                <asp:TemplateField HeaderText="Code" SortExpression="Emp_Code">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblEmpCode" runat="server" Text='<%# Eval("Emp_Code") %>'></asp:Label>
                                                                        <asp:Label ID="lblEmpId" Visible="false" runat="server" Text='<%# Eval("Emp_Id") %>'></asp:Label>
                                                                    </ItemTemplate>
                                                                    <ItemStyle />
                                                                </asp:TemplateField>
                                                                <asp:BoundField DataField="Emp_Name" HeaderText="Name"
                                                                    SortExpression="Emp_Name">
                                                                    <ItemStyle />
                                                                </asp:BoundField>
                                                                <%-- <asp:BoundField DataField="Emp_Name_L" HeaderText="<%$ Resources:Attendance,Employee Name Local%>"
                                                                    SortExpression="Emp_Name_L" >
                                                                    <ItemStyle  />
                                                                </asp:BoundField>--%>
                                                                <asp:BoundField DataField="MonthName" HeaderText="<%$ Resources:Attendance,Month%>"
                                                                    SortExpression="MonthName">
                                                                    <ItemStyle />
                                                                </asp:BoundField>
                                                                <asp:BoundField DataField="Year" HeaderText="<%$ Resources:Attendance,Year%>" SortExpression="Year">
                                                                    <ItemStyle />
                                                                </asp:BoundField>
                                                                <asp:TemplateField HeaderText="<%$ Resources:Attendance,Basic Salary %>">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblbasicSalary" runat="server" Text='<%# Eval("BasicSalary") %>'></asp:Label>
                                                                    </ItemTemplate>
                                                                    <ItemStyle />
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="<%$ Resources:Attendance,Attendance Salary %>">
                                                                    <ItemTemplate>
                                                                        <%--<asp:Label ID="lblItemType" runat="server" Text='<%# Eval("Attendence_Salary") %>'></asp:Label>--%>
                                                                         <asp:LinkButton ID="linkDailySalary" ToolTip="Salary Report" OnCommand="imgBtnDetail_Command" CommandArgument='<%# Eval("Emp_Id") %>'
                                                    Text='<%# Eval("Attendence_Salary") %>' runat="server"></asp:LinkButton>
                                                                    </ItemTemplate>
                                                                    <ItemStyle />
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="<%$ Resources:Attendance,Allowance %>">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblallowances" runat="server" Text='<%# Eval("Total_Allowance") %>'></asp:Label>
                                                                    </ItemTemplate>
                                                                    <ItemStyle />
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="<%$ Resources:Attendance,Claim %>">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblClaim" runat="server" Text='<%# Eval("Employee_Claim") %>'></asp:Label>
                                                                    </ItemTemplate>
                                                                    <ItemStyle />
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="<%$ Resources:Attendance,Deduction %>">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lbldeduction" runat="server" Text='<%# Eval("Total_Deduction") %>'></asp:Label>
                                                                    </ItemTemplate>
                                                                    <ItemStyle />
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="<%$ Resources:Attendance,Penalty %>">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblPenalty" runat="server" Text='<%# Eval("Employee_Penalty") %>'></asp:Label>
                                                                    </ItemTemplate>
                                                                    <ItemStyle />
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="<%$ Resources:Attendance,Loan %>">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblLoan" runat="server" Text='<%# Eval("Emlployee_Loan") %>'></asp:Label>
                                                                    </ItemTemplate>
                                                                    <ItemStyle />
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="<%$ Resources:Attendance,Previous Month Claim/Penalty %>">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lbladjustment" runat="server" Text='<%# Eval("Previous_Month_Balance") %>'></asp:Label>
                                                                    </ItemTemplate>
                                                                    <ItemStyle />
                                                                </asp:TemplateField>
                                                                <%--<asp:TemplateField HeaderText="<%$ Resources:Attendance,Employee PF %>">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblempPf" runat="server" Text='<%# Eval("Employee_PF") %>'></asp:Label>
                                                                    </ItemTemplate>
                                                                    <ItemStyle  />
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="<%$ Resources:Attendance,Employee ESIC %>">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblempesic" runat="server" Text='<%# Eval("Employee_ESIC") %>'></asp:Label>
                                                                    </ItemTemplate>
                                                                    <ItemStyle  />
                                                                </asp:TemplateField>--%>
                                                                <asp:TemplateField HeaderText="<%$ Resources:Attendance,Net Salary%>">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblGrossSalary" runat="server" Text='<%# Eval("Gross_Salary") %>'></asp:Label>
                                                                    </ItemTemplate>
                                                                    <ItemStyle />
                                                                </asp:TemplateField>
                                                            </Columns>


                                                            <PagerStyle CssClass="pagination-ys" />

                                                        </asp:GridView>
                                                        <asp:HiddenField ID="hdFSortgvEmpPayroll" runat="server" />
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
                
                 <div id="Div_Emp_Details" runat="server" style="display: none;">

                    <div id="Div_Payroll" runat="server" class="row">
                        <div style="margin-left: 50px;">
                            <asp:Button ID="lnkbtnback" runat="server" CssClass="btn btn-primary" Text="<%$ Resources:Attendance,Back %>" OnClick="lnkbtnback_Click" />

                            <asp:Button ID="btnLogReport" runat="server" CssClass="btn btn-primary" Text="Log Report" OnClick="btnLogReport_Click" />
                            <asp:Button ID="btnSalaryReport" runat="server" CssClass="btn btn-primary" Text="Salary Report" OnClick="btnSalaryReport_Click" />
                            <asp:Button ID="btnInOutReport" runat="server" CssClass="btn btn-primary" Text="In Out Report" OnClick="btnInOutReport_Click" />

                            <br />
                            <br />
                        </div>
                    </div>
                    <div id="Emp_Details" runat="server" class="row">
                        <div class="col-md-12">
                            <div class="box box-warning">
                                <div class="box-header with-border">
                                    <h3 class="box-title">Employee Details</h3>

                                    <div class="box-tools pull-right">
                                        <button type="button" class="btn btn-box-tool" data-widget="collapse">
                                            <i class="fa fa-minus"></i>
                                        </button>
                                    </div>
                                </div>
                                <div class="box-body">
                                    <div class="col-md-6">
                                        <asp:Label ID="Label51" runat="server" Font-Bold="true"
                                            Text="<%$ Resources:Attendance,Employee Code %>"></asp:Label>
                                        &nbsp:&nbsp<asp:Label ID="lblEmpCodeOT" runat="server"></asp:Label>
                                        <br />
                                    </div>
                                    <div class="col-md-6">
                                        <asp:Label ID="Label53" runat="server" Font-Bold="true"
                                            Text="<%$ Resources:Attendance,Employee Name %>"></asp:Label>
                                        &nbsp:&nbsp
                                    <asp:Label ID="lblEmpNameOT" runat="server"></asp:Label>
                                    </div>
                                    <div class="col-md-6">
                                        <asp:Label ID="Label34" runat="server" Font-Bold="true"
                                            Text="<%$ Resources:Attendance,Month %>"></asp:Label>
                                        &nbsp:&nbsp
                                    <asp:Label ID="lblMonthName" runat="server"></asp:Label>
                                    </div>
                                    <div class="col-md-6">
                                        <asp:Label ID="Label35" runat="server" Font-Bold="true"
                                            Text="<%$ Resources:Attendance,Year %>"></asp:Label>
                                        &nbsp:&nbsp
                                    <asp:Label ID="lblYearName" runat="server"></asp:Label>
                                    </div>
                                    <div class="col-md-6">
                                        <asp:Label ID="Label36" runat="server" Font-Bold="true" Text="<%$ Resources:Attendance,Final Paid Amount %>"></asp:Label>
                                        &nbsp:&nbsp
                                    <asp:Label ID="lblFinalAmount" Font-Bold="true" runat="server"></asp:Label>
                                    </div>
                                    <div style="display: none" class="col-md-12">
                                        <asp:Button ID="btnAllowance" runat="server" Text="<%$ Resources:Attendance,Allowance %>"
                                            BorderStyle="none" BackColor="Transparent" OnClick="btnAllowance1_Click"
                                            Style="padding-left: 10px; padding-top: 3px; background-repeat: no-repeat; height: 49px; background-position: 5px 15px; font: bold 14px Trebuchet MS; color: #000000;" />
                                        <asp:Button ID="btndeduction" runat="server" Text="<%$ Resources:Attendance,Deduction %>"
                                            BorderStyle="none" BackColor="Transparent" OnClick="btndeduction1_Click"
                                            Style="padding-left: 10px; padding-top: 3px; background-repeat: no-repeat; height: 49px; background-position: 5px 15px; font: bold 14px Trebuchet MS; color: #000000;" />
                                        <asp:Button ID="btnclaimpenalty" runat="server" Text="<%$ Resources:Attendance,Penalty/Claim %>"
                                            BorderStyle="none" BackColor="Transparent" OnClick="btnclaimpenalty1_Click"
                                            Style="padding-left: 10px; padding-top: 3px; background-repeat: no-repeat; height: 49px; background-position: 5px 15px; font: bold 14px Trebuchet MS; color: #000000;" />
                                        <asp:Button ID="btnloan" runat="server" Text="<%$ Resources:Attendance,Loan %>"
                                            BorderStyle="none" BackColor="Transparent" OnClick="btnloan1_Click" Style="padding-left: 10px; padding-top: 3px; background-repeat: no-repeat; height: 49px; background-position: 5px 15px; font: bold 14px Trebuchet MS; color: #000000;" />
                                        <asp:Button ID="btnattendance" runat="server" Text="<%$ Resources:Attendance,Attendance Detail %>"
                                            BorderStyle="none" BackColor="Transparent" OnClick="btnattendance1_Click"
                                            Style="padding-left: 10px; padding-top: 3px; background-repeat: no-repeat; height: 49px; background-position: 5px 15px; font: bold 14px Trebuchet MS; color: #000000;" />
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                    
                    <div id="Div_attnd" style="display: none;" runat="server" class="row">
                        <div class="col-md-12">
                            <div class="box box-warning">
                                <div class="box-header with-border">
                                    <h3 class="box-title">Attendance Salary Details</h3>
                                    <div class="box-tools pull-right">
                                        <button type="button" class="btn btn-box-tool" data-widget="collapse">
                                            <i class="fa fa-minus"></i>
                                        </button>
                                    </div>
                                </div>
                                <div class="box-body">
                                    <div class="col-md-12">
                                        <div class="box box-primary">
                                            <div class="box-header with-border" style="text-align: center">
                                                <h3 class="box-title">
                                                    <asp:Label ID="Label32" runat="server" Text="<%$ Resources:Attendance,Salary (Basic Salary)%>" />
                                                    &nbsp; - &nbsp;
                                                    <asp:Label ID="txtBasicSalary" runat="server"></asp:Label></h3>
                                            </div>
                                            <div class="box-body">
                                                <div class="col-md-3">
                                                    <asp:Label ID="Label2" runat="server" Text="<%$ Resources:Attendance,Worked Salary %>" Font-Bold="true" />
                                                    <asp:Label ID="lblmonth" runat="server" Visible="false" />
                                                    <asp:Label ID="lblyear" runat="server" Visible="false" />
                                                    <asp:Label ID="lbltrnsid" runat="server" Visible="false" />
                                                    <asp:TextBox ID="txtworkedminsal" MaxLength="10" CssClass="form-control" runat="server" Text='<%# Common.GetAmountDecimal(Eval("Worked_Min_Salary").ToString(),Session["DBConnection"].ToString(),Session["LocCurrnecyId"].ToString()) %>' AutoPostBack="true" OnTextChanged="txtworkedminsal_TextChanged" onblur="checkDec(this);"></asp:TextBox>
                                                    <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender3" runat="server" TargetControlID="txtworkedminsal" ValidChars="0,1,2,3,4,5,6,7,8,9,."></cc1:FilteredTextBoxExtender>
                                                    <br />
                                                </div>
                                                <div class="col-md-3">
                                                    <asp:Label ID="Label4" runat="server" Text="<%$ Resources:Attendance,Week Off OT Salary %>" Font-Bold="true" />
                                                    <asp:TextBox ID="txtWeekoffotminsal" MaxLength="10" CssClass="form-control" runat="server" Text='<%# Common.GetAmountDecimal(Eval("Week_Off_OT_Min_Salary").ToString(),Session["DBConnection"].ToString(),Session["LocCurrnecyId"].ToString()) %>' AutoPostBack="true" OnTextChanged="txtWeekoffotminsal_TextChanged" onblur="checkDec(this);"></asp:TextBox>
                                                    <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender5" runat="server" TargetControlID="txtWeekoffotminsal" ValidChars="0,1,2,3,4,5,6,7,8,9,."></cc1:FilteredTextBoxExtender>
                                                    <br />
                                                </div>
                                                <div class="col-md-3">
                                                    <asp:Label ID="Label9" runat="server" Text="<%$ Resources:Attendance,Week Off Salary %>" Font-Bold="true" />
                                                    <asp:TextBox ID="txtweekoffsal" MaxLength="10" CssClass="form-control" runat="server" Text='<%# Common.GetAmountDecimal(Eval("Week_Off_Salary").ToString(),Session["DBConnection"].ToString(),Session["LocCurrnecyId"].ToString()) %>' AutoPostBack="true" OnTextChanged="txtweekoffsal_TextChanged" onblur="checkDec(this);"></asp:TextBox>
                                                    <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender8" runat="server" TargetControlID="txtweekoffsal" ValidChars="0,1,2,3,4,5,6,7,8,9,."></cc1:FilteredTextBoxExtender>
                                                    <br />
                                                </div>

                                                <div class="col-md-3">
                                                    <asp:Label ID="Label7" runat="server" Text="<%$ Resources:Attendance,Holiday OT Salary %>" Font-Bold="true" />
                                                    <asp:TextBox ID="txthloidayotminsal" MaxLength="10" CssClass="form-control" runat="server" Text='<%# Common.GetAmountDecimal(Eval("Holiday_OT_Min_Salary").ToString(),Session["DBConnection"].ToString(),Session["LocCurrnecyId"].ToString()) %>' AutoPostBack="true" OnTextChanged="txthloidayotminsal_TextChanged" onblur="checkDec(this);"></asp:TextBox>
                                                    <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender6" runat="server" TargetControlID="txthloidayotminsal" ValidChars="0,1,2,3,4,5,6,7,8,9,."></cc1:FilteredTextBoxExtender>
                                                    <br />
                                                </div>
                                                <div class="col-md-3">
                                                    <asp:Label ID="Label11" runat="server" Text="<%$ Resources:Attendance,Holidays Salary %>" Font-Bold="true" />
                                                    <asp:TextBox ID="txtholidayssal" MaxLength="10" CssClass="form-control" runat="server" Text='<%# Common.GetAmountDecimal(Eval("Holidays_Salary").ToString(),Session["DBConnection"].ToString(),Session["LocCurrnecyId"].ToString()) %>' AutoPostBack="true" OnTextChanged="txtholidayssal_TextChanged" onblur="checkDec(this);"></asp:TextBox>
                                                    <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender9" runat="server" TargetControlID="txtholidayssal" ValidChars="0,1,2,3,4,5,6,7,8,9,."></cc1:FilteredTextBoxExtender>
                                                    <br />
                                                </div>
                                                <div class="col-md-3">
                                                    <asp:Label ID="Label3" runat="server" Text="<%$ Resources:Attendance,Normal OT Salary %>" Font-Bold="true" />
                                                    <asp:TextBox ID="txtnormalOtminsal" MaxLength="10" CssClass="form-control" runat="server" Text='<%#Common.GetAmountDecimal( Eval("Normal_OT_Min_Salary").ToString(),Session["DBConnection"].ToString(),Session["LocCurrnecyId"].ToString()) %>' AutoPostBack="true" OnTextChanged="txtnormalOtminsal_TextChanged" onblur="checkDec(this);"></asp:TextBox>
                                                    <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender4" runat="server" TargetControlID="txtnormalOtminsal" ValidChars="0,1,2,3,4,5,6,7,8,9,."></cc1:FilteredTextBoxExtender>
                                                    <br />
                                                </div>
                                                <div class="col-md-3">
                                                    <asp:Label ID="Label8" runat="server" Text="<%$ Resources:Attendance,Leave Days Salary %>" Font-Bold="true" />
                                                    <asp:TextBox ID="txtPayrolldayssal" MaxLength="10" CssClass="form-control" runat="server" Text='<%# Common.GetAmountDecimal(Eval("Leave_Days_Salary").ToString(),Session["DBConnection"].ToString(),Session["LocCurrnecyId"].ToString()) %>' AutoPostBack="true" OnTextChanged="txtPayrolldayssal_TextChanged" onblur="checkDec(this);"></asp:TextBox>
                                                    <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender7" runat="server" TargetControlID="txtPayrolldayssal" ValidChars="0,1,2,3,4,5,6,7,8,9,."></cc1:FilteredTextBoxExtender>
                                                    <br />
                                                </div>
                                            </div>
                                            <div class="box-footer" style="text-align: right">
                                                <%--<asp:Label ID="Label38" runat="server" Text=" + "  Font-Bold="true" />
                                                <asp:Label ID="Label17" runat="server" Text="<%$ Resources:Attendance,Total Salary%>"  Font-Bold="true" />
                                                &nbsp; : &nbsp;
                                                <asp:Label ID="Txt_Tot_Salary" Font-Bold="true" Text="0.00"  runat="server"></asp:Label>--%>

                                                <div class="col-md-12" style="text-align: right">
                                                    <div class="col-md-4"></div>
                                                    <div class="col-md-4"></div>
                                                    <div class="col-md-4" style="background-color: #EEEEEE">
                                                        <asp:Label ID="Label17" runat="server" Text=" + " Font-Bold="true" />
                                                        <asp:Label ID="Label33" runat="server" Text="<%$ Resources:Attendance,Total Attendance Salary%>" Font-Size="13" Font-Bold="true" />
                                                        <asp:Label ID="Label38" runat="server" Text=" = " Font-Bold="true" />
                                                        <asp:Label ID="Txt_Tot_Salary" Font-Bold="true" Text="0.00" Font-Size="13" runat="server"></asp:Label>
                                                    </div>
                                                    <br />
                                                </div>

                                            </div>
                                        </div>
                                    </div>
                                    <div class="col-md-12">
                                        <div class="box box-info">
                                            <div class="box-header with-border" style="text-align: center">
                                                <h3 class="box-title">Deduction</h3>
                                            </div>
                                            <div class="box-body">
                                                <div class="col-md-3">
                                                    <asp:Label ID="Label13" runat="server" Text="<%$ Resources:Attendance,Late Penalty%>" Font-Bold="true" />
                                                    <asp:TextBox ID="txtlateminsal" MaxLength="10" CssClass="form-control" runat="server" Text='<%# Common.GetAmountDecimal(Eval("Late_Min_Penalty").ToString(),Session["DBConnection"].ToString(),Session["LocCurrnecyId"].ToString()) %>' AutoPostBack="true" OnTextChanged="txtlateminsal_TextChanged" onblur="checkDec(this);"></asp:TextBox>
                                                    <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender11" runat="server" TargetControlID="txtlateminsal" ValidChars="0,1,2,3,4,5,6,7,8,9,."></cc1:FilteredTextBoxExtender>
                                                    <br />
                                                </div>
                                                <div class="col-md-3">
                                                    <asp:Label ID="Label14" runat="server" Text="<%$ Resources:Attendance,Early Penalty%>" Font-Bold="true" />
                                                    <asp:TextBox ID="txtearlyminsal" MaxLength="10" CssClass="form-control" runat="server" Text='<%# Common.GetAmountDecimal(Eval("Early_Min_Penalty").ToString(),Session["DBConnection"].ToString(),Session["LocCurrnecyId"].ToString()) %>' AutoPostBack="true" OnTextChanged="txtearlyminsal_TextChanged" onblur="checkDec(this);"></asp:TextBox>
                                                    <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender12" runat="server" TargetControlID="txtearlyminsal" ValidChars="0,1,2,3,4,5,6,7,8,9,."></cc1:FilteredTextBoxExtender>
                                                    <br />
                                                </div>
                                                <div class="col-md-3">
                                                    <asp:Label ID="Label12" runat="server" Text="<%$ Resources:Attendance,Absent Penalty%>" Font-Bold="true" />
                                                    <asp:TextBox ID="txtAbsaentsal" MaxLength="10" CssClass="form-control" runat="server" Text='<%# Common.GetAmountDecimal(Eval("Absent_Salary").ToString(),Session["DBConnection"].ToString(),Session["LocCurrnecyId"].ToString()) %>' AutoPostBack="true" OnTextChanged="txtAbsaentsal_TextChanged" onblur="checkDec(this);"></asp:TextBox>
                                                    <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender10" runat="server" TargetControlID="txtAbsaentsal" ValidChars="0,1,2,3,4,5,6,7,8,9,."></cc1:FilteredTextBoxExtender>
                                                    <br />
                                                </div>
                                                <div class="col-md-3">
                                                    <asp:Label ID="Label15" runat="server" Text="<%$ Resources:Attendance,Partial Violation Penalty%>" Font-Bold="true" />
                                                    <asp:TextBox ID="txtpatialvoisal" MaxLength="10" CssClass="form-control" runat="server" Text='<%# Common.GetAmountDecimal(Eval("Patial_Violation_Penalty").ToString(),Session["DBConnection"].ToString(),Session["LocCurrnecyId"].ToString()) %>' AutoPostBack="true" OnTextChanged="txtpatialvoisal_TextChanged" onblur="checkDec(this);"></asp:TextBox>
                                                    <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender13" runat="server" TargetControlID="txtpatialvoisal" ValidChars="0,1,2,3,4,5,6,7,8,9,."></cc1:FilteredTextBoxExtender>
                                                    <br />
                                                </div>
                                            </div>
                                            <div class="box-footer" style="text-align: right">
                                                <div class="col-md-12" style="text-align: right">
                                                    <div class="col-md-4"></div>
                                                    <div class="col-md-4"></div>
                                                    <div class="col-md-4" style="background-color: #EEEEEE">
                                                        <asp:Label ID="Label18" runat="server" Text=" - " Font-Bold="true" />
                                                        <asp:Label ID="Label39" runat="server" Text="<%$ Resources:Attendance,Total Attendance Deduction%>" Font-Size="13" Font-Bold="true" />
                                                        <asp:Label ID="Label40" runat="server" Text=" = " Font-Bold="true" />
                                                        <asp:Label ID="Txt_Tot_Deduction" Font-Bold="true" Text="0.00" Font-Size="13" runat="server"></asp:Label>
                                                    </div>
                                                    <br />
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="col-md-12">
                                        <div class="col-md-12">
                                            <div class="col-md-12" style="text-align: right">
                                                <div class="col-md-4"></div>
                                                <div class="col-md-4"></div>
                                                <div class="col-md-4" style="background-color: #EEEEEE">
                                                    <asp:Label ID="Label16" runat="server" Text="<%$ Resources:Attendance,Net Attendance Salary%>" Font-Size="14" Font-Bold="true" />
                                                    <asp:Label ID="Label37" runat="server" Text=" = " Font-Bold="true" />
                                                    <asp:Label ID="txtGrossAmount" OnDataBinding="txtDeducValue_TextChanged" Font-Bold="true" Text="0.00" Font-Size="14" runat="server"></asp:Label>
                                                </div>
                                                <br />
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div id="Div_gvallowance" style="display: none;" runat="server" class="row">
                        <div class="col-md-12">
                            <div class="box box-warning">
                                <div class="box-header with-border">
                                    <h3 class="box-title">Allowances</h3>
                                    <div class="box-tools pull-right">
                                        <button type="button" class="btn btn-box-tool" data-widget="collapse">
                                            <i class="fa fa-minus"></i>
                                        </button>
                                    </div>
                                </div>
                                <div class="box-body">
                                    <div class="flow">
                                        <asp:GridView CssClass="table-striped table-bordered table table-hover" ID="gvallowance" runat="server" AutoGenerateColumns="False" DataKeyNames="Emp_Id"
                                            PageSize='<%# int.Parse(Session["GridSize"].ToString()) %>' TabIndex="10" Width="100%" AllowPaging="True"
                                            OnPageIndexChanging="gvallowance_PageIndexChanging">
                                            <Columns>
                                                <asp:TemplateField HeaderStyle-HorizontalAlign="Left" HeaderText="<%$ Resources:Attendance,Allowance Type %>">
                                                    <ItemTemplate>
                                                        <asp:HiddenField ID="hdnEmpId" runat="server" Value='<%# Eval("Emp_Id") %>' />
                                                        <asp:HiddenField ID="hdnallowId" runat="server" Value='<%# Eval("Allowance_Id") %>' />
                                                        <asp:Label ID="lblType" runat="server" Text='<%# Eval("Allowance_Id") %>'></asp:Label>
                                                        <asp:HiddenField ID="hdntransId" runat="server" Value='<%# Eval("Trans_Id") %>' />
                                                    </ItemTemplate>
                                                    <ItemStyle />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderStyle-HorizontalAlign="Left" HeaderText="<%$ Resources:Attendance, Allowance Value %>">
                                                    <ItemTemplate>
                                                        <asp:HiddenField ID="hdnRefId" runat="server" Value='<%# Eval("Ref_Id") %>' />
                                                        <asp:Label ID="lblAllowValue" runat="server" Text='<%# Common.GetAmountDecimal(Eval("Allowance_Value").ToString(),Session["DBConnection"].ToString(),Session["CurrencyId"].ToString()) %>' />
                                                    </ItemTemplate>
                                                    <ItemStyle />

                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderStyle-HorizontalAlign="Left" HeaderText="<%$ Resources:Attendance,Act Allowance Value %>">
                                                    <ItemTemplate>
                                                        <%--onchange="javascript:Get_Total_Allowances(this)"--%>
                                                        <asp:TextBox ID="txtValue" MaxLength="10" AutoPostBack="true" OnTextChanged="txtValue_TextChanged" CssClass="form-control" runat="server" Visible="true" Text='<%# Common.GetAmountDecimal(Eval("Act_Allowance_Value").ToString(),Session["DBConnection"].ToString(),Session["CurrencyId"].ToString()) %>'></asp:TextBox>
                                                        <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtenderPAidPayroll" runat="server"
                                                            TargetControlID="txtValue" ValidChars="0,1,2,3,4,5,6,7,8,9,.">
                                                        </cc1:FilteredTextBoxExtender>
                                                    </ItemTemplate>
                                                    <ItemStyle />
                                                </asp:TemplateField>
                                                <asp:TemplateField Visible="false">
                                                    <ItemTemplate>
                                                        <asp:CheckBox ID="chkallowance" runat="server" Text="<%$ Resources:Attendance,IsMonthCarry%>" />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                            </Columns>


                                            <PagerStyle CssClass="pagination-ys" />

                                        </asp:GridView>
                                    </div>
                                </div>
                                <div class="box-footer" style="text-align: right">
                                    <div class="col-md-12" style="text-align: right">
                                        <div class="col-md-4"></div>
                                        <div class="col-md-4"></div>
                                        <div class="col-md-4" style="background-color: #EEEEEE">
                                            <asp:Label ID="Label42" runat="server" Text="<%$ Resources:Attendance,Total Allowances%>" Font-Size="13" Font-Bold="true" />
                                            <asp:Label ID="Label43" runat="server" Text=" = " Font-Bold="true" />
                                            <asp:Label ID="Lbl_Total_Allowances" Font-Bold="true" Text="0.00" Font-Size="13" runat="server"></asp:Label>
                                        </div>
                                        <br />
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div id="Div_gvdeduuc" style="display: none;" runat="server" class="row">
                        <div class="col-md-12">
                            <div class="box box-warning">
                                <div class="box-header with-border">
                                    <h3 class="box-title">Deductions</h3>
                                    <div class="box-tools pull-right">
                                        <button type="button" class="btn btn-box-tool" data-widget="collapse">
                                            <i class="fa fa-minus"></i>
                                        </button>
                                    </div>
                                </div>
                                <div class="box-body">
                                    <div class="flow">
                                        <asp:GridView CssClass="table-striped table-bordered table table-hover" ID="gvdeduction" runat="server" AutoGenerateColumns="False" DataKeyNames="Emp_Id"
                                            PageSize='<%# int.Parse(Session["GridSize"].ToString()) %>' TabIndex="10" Width="100%" AllowPaging="True"
                                            OnPageIndexChanging="gvdeduction_PageIndexChanging">
                                            <Columns>
                                                <asp:TemplateField HeaderStyle-HorizontalAlign="Left" HeaderText="<%$ Resources:Attendance,Deduction %>">
                                                    <ItemTemplate>
                                                        <asp:HiddenField ID="hdnEmpId" runat="server" Value='<%# Eval("Emp_Id") %>' />
                                                        <asp:HiddenField ID="hdnDeducId" runat="server" Value='<%# Eval("Deduction_Id") %>' />
                                                        <asp:Label ID="lblTypededuc" runat="server" Text='<%# Eval("Deduction_Id") %>'></asp:Label>
                                                        <asp:HiddenField ID="hdntransIddeduc" runat="server" Value='<%# Eval("Trans_Id") %>' />
                                                    </ItemTemplate>
                                                    <ItemStyle />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderStyle-HorizontalAlign="Left" HeaderText="<%$ Resources:Attendance,Deduction Value %>">
                                                    <ItemTemplate>
                                                        <asp:HiddenField ID="hdnRefId" runat="server" Value='<%# Eval("Ref_Id") %>' />
                                                        <asp:Label ID="lblDeductValue" runat="server" Text='<%# Common.GetAmountDecimal(Eval("Deduction_Value").ToString(),Session["DBConnection"].ToString(),Session["LocCurrnecyId"].ToString()) %>' />
                                                    </ItemTemplate>
                                                    <ItemStyle />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderStyle-HorizontalAlign="Left" HeaderText="<%$ Resources:Attendance,Act Deduction Value %>">
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="txtDeducValue" MaxLength="10" AutoPostBack="true" OnTextChanged="txtDeducValue_TextChanged" CssClass="form-control" runat="server" Text='<%# Common.GetAmountDecimal(Eval("Act_Deduction_Value").ToString(),Session["DBConnection"].ToString(),Session["LocCurrnecyId"].ToString()) %>'></asp:TextBox>
                                                        <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtenderPAidPayroll" runat="server"
                                                            TargetControlID="txtDeducValue" ValidChars="0,1,2,3,4,5,6,7,8,9,.">
                                                        </cc1:FilteredTextBoxExtender>
                                                    </ItemTemplate>
                                                    <ItemStyle />
                                                </asp:TemplateField>
                                                <asp:TemplateField Visible="false">
                                                    <ItemTemplate>
                                                        <asp:CheckBox ID="chkdeduc" runat="server" Text="<%$ Resources:Attendance,IsMonthCarry%>" />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                            </Columns>


                                            <PagerStyle CssClass="pagination-ys" />

                                        </asp:GridView>
                                    </div>
                                </div>
                                <div class="box-footer" style="text-align: right">
                                    <div class="col-md-12" style="text-align: right">
                                        <div class="col-md-4"></div>
                                        <div class="col-md-4"></div>
                                        <div class="col-md-4" style="background-color: #EEEEEE">
                                            <asp:Label ID="Label41" runat="server" Text="<%$ Resources:Attendance,Total Deductions%>" Font-Size="13" Font-Bold="true" />
                                            <asp:Label ID="Label44" runat="server" Text=" = " Font-Bold="true" />
                                            <asp:Label ID="Lbl_Total_Deductions" Font-Bold="true" Text="0.00" Font-Size="13" runat="server"></asp:Label>
                                        </div>
                                        <br />
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div id="Div_penaltyclaim" style="display: none;" runat="server" class="row">
                        <div class="col-md-12">
                            <div class="box box-warning">
                                <div class="box-header with-border">
                                    <h3 class="box-title">Penalties/Claims</h3>
                                    <div class="box-tools pull-right">
                                        <button type="button" class="btn btn-box-tool" data-widget="collapse">
                                            <i class="fa fa-minus"></i>
                                        </button>
                                    </div>
                                </div>
                                <div class="box-body">
                                    <div class="flow">
                                        <asp:GridView CssClass="table-striped table-bordered table table-hover" ID="gvPenalty" runat="server" AutoGenerateColumns="False" DataKeyNames="Emp_Id"
                                            PageSize='<%# int.Parse(Session["GridSize"].ToString()) %>' AllowPaging="false" TabIndex="10"
                                            Width="100%">
                                            <Columns>
                                                <asp:TemplateField Visible="false" HeaderText="<%$ Resources:Attendance,Penalty Id %>">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblPenaltyId1" runat="server" Text='<%# Eval("Penalty_Id") %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderStyle-HorizontalAlign="Left" HeaderText="<%$ Resources:Attendance,Penalty Name %>">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblPenaltyId2" runat="server" Text='<%# Eval("Penalty_Name") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <ItemStyle />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderStyle-HorizontalAlign="Left" HeaderText="<%$ Resources:Attendance,Actual Value %>">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblPenaltyValue1" runat="server" Text='<%# Common.GetAmountDecimal(Eval("Field1").ToString(),Session["DBConnection"].ToString(),Session["LocCurrnecyId"].ToString()) %>' />
                                                    </ItemTemplate>
                                                    <ItemStyle />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderStyle-HorizontalAlign="Left" HeaderText="<%$ Resources:Attendance,Update Value %>">
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="txtPenaltyValue" MaxLength="10" AutoPostBack="true" CssClass="form-control" OnTextChanged="txtPenaltyValue_OnTextChanged"
                                                            runat="server" Text='<%# Common.GetAmountDecimal(Eval("Field2").ToString(),Session["DBConnection"].ToString(),Session["LocCurrnecyId"].ToString()) %>'></asp:TextBox>
                                                        <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtenderPAidPayroll" runat="server"
                                                            TargetControlID="txtPenaltyValue" ValidChars="0,1,2,3,4,5,6,7,8,9,.">
                                                        </cc1:FilteredTextBoxExtender>
                                                    </ItemTemplate>
                                                    <ItemStyle />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderStyle-HorizontalAlign="Left" HeaderText="<%$ Resources:Attendance,Description %>">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblPenaltydesc" runat="server" Text='<%# Eval("Penalty_Description") %>' />
                                                    </ItemTemplate>
                                                    <ItemStyle />
                                                </asp:TemplateField>
                                            </Columns>


                                            <PagerStyle CssClass="pagination-ys" />

                                        </asp:GridView>
                                    </div>
                                    <div class="col-md-12">
                                        <div class="col-md-4">
                                            <asp:Label ID="lblpenalty" runat="server" Text="<%$ Resources:Attendance,Total Penalty %>"
                                                Font-Bold="true" />
                                            &nbsp:&nbsp<asp:Label ID="lbltotalpenalty" runat="server" Font-Bold="true"></asp:Label>
                                        </div>
                                        <div class="col-md-4">
                                            <asp:TextBox ID="txttotalpeanlty" ReadOnly="true" CssClass="form-control" runat="server" Text='<%# Common.GetAmountDecimal(Eval("Employee_Penalty").ToString(),Session["DBConnection"].ToString(),Session["LocCurrnecyId"].ToString()) %>'></asp:TextBox>
                                            <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtenderPAidPayroll" runat="server"
                                                TargetControlID="txttotalpeanlty" ValidChars="0,1,2,3,4,5,6,7,8,9,.">
                                            </cc1:FilteredTextBoxExtender>
                                        </div>
                                        <div class="col-md-4">
                                            <asp:CheckBox ID="chkpenalty" runat="server" Text="<%$ Resources:Attendance,IsMonthCarry%>" />
                                        </div>
                                    </div>
                                    <div class="col-md-12" class="flow">
                                        <asp:GridView CssClass="table-striped table-bordered table table-hover" ID="GvClaim" runat="server" AutoGenerateColumns="False" DataKeyNames="Emp_Id"
                                            PageSize='<%# int.Parse(Session["GridSize"].ToString()) %>' AllowPaging="false" TabIndex="10"
                                            Width="100%">
                                            <Columns>
                                                <asp:TemplateField Visible="false" HeaderText="<%$ Resources:Attendance,Claim Id %>">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblClaimId1" runat="server" Text='<%# Eval("Claim_Id") %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderStyle-HorizontalAlign="Left" HeaderText="<%$ Resources:Attendance,Claim Name %>">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblClaimId2" runat="server" Text='<%# Eval("Claim_Name") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <ItemStyle />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderStyle-HorizontalAlign="Left" HeaderText="<%$ Resources:Attendance,Actual Value %>">
                                                    <ItemTemplate>
                                                        <%-- <asp:Label ID="lblClaimValue1" runat="server" Text='<%# Common.GetAmountDecimal(Eval("Field1").ToString(),Session["DBConnection"].ToString(),Session["LocCurrnecyId"].ToString()) %>' />--%>
                                                        <asp:Label ID="lblClaimValue1" runat="server" Text='<%# Eval("Field1").ToString() %>' />
                                                    </ItemTemplate>
                                                    <ItemStyle />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderStyle-HorizontalAlign="Left" HeaderText="<%$ Resources:Attendance,Update Value %>">
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="txtClaimValue" MaxLength="10" AutoPostBack="true" CssClass="form-control" OnTextChanged="txtClaimValue_OnTextChanged"
                                                            runat="server" Text='<%# Eval("Field2").ToString()  %>'></asp:TextBox>
                                                        <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtenderPAidPayroll" runat="server"
                                                            TargetControlID="txtClaimValue" ValidChars="0,1,2,3,4,5,6,7,8,9,.">
                                                        </cc1:FilteredTextBoxExtender>
                                                    </ItemTemplate>
                                                    <ItemStyle />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderStyle-HorizontalAlign="Left" HeaderText="<%$ Resources:Attendance,Description %>">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblClaimdesc" runat="server" Text='<%# Eval("Claim_Description") %>' />
                                                    </ItemTemplate>
                                                    <ItemStyle />
                                                </asp:TemplateField>
                                            </Columns>


                                            <PagerStyle CssClass="pagination-ys" />

                                        </asp:GridView>
                                    </div>
                                    <div class="col-md-12">
                                        <div class="col-md-4">
                                            <asp:Label ID="lblclaim" runat="server" Text="<%$ Resources:Attendance,Total Claim%>"
                                                Font-Bold="true" />
                                            &nbsp:&nbsp<asp:Label ID="lbltotalclaim" runat="server" Font-Bold="true"></asp:Label>
                                        </div>
                                        <div class="col-md-4">

                                            <asp:TextBox ID="txttotalclaim" ReadOnly="true" CssClass="form-control" runat="server" Text='<%# Eval("Employee_Claim") %>'></asp:TextBox>
                                            <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" runat="server" TargetControlID="txttotalclaim"
                                                ValidChars="0,1,2,3,4,5,6,7,8,9,.">
                                            </cc1:FilteredTextBoxExtender>

                                        </div>
                                        <div class="col-md-4">
                                            <asp:CheckBox ID="chkclaim" runat="server" Text="<%$ Resources:Attendance,IsMonthCarry%>"></asp:CheckBox>

                                        </div>
                                    </div>
                                    <div class="col-md-12">
                                        <div class="col-md-4">
                                            <asp:Label ID="lblduepayamt" runat="server" Text="<%$ Resources:Attendance,Adjustment Amount%>"
                                                Font-Bold="true" />
                                            &nbsp:&nbsp<asp:Label ID="lbldueamt" runat="server" Text="" Font-Bold="true"></asp:Label>
                                        </div>
                                        <div class="col-md-4">

                                            <asp:TextBox ID="txtdueamt" MaxLength="10" CssClass="form-control" runat="server" ReadOnly="true" Text=""></asp:TextBox>
                                            <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender2" runat="server" TargetControlID="txtdueamt"
                                                ValidChars="0,1,2,3,4,5,6,7,8,9,-,.">
                                            </cc1:FilteredTextBoxExtender>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div id="Div_loan" style="display: none;" runat="server" class="row">
                        <div class="col-md-12">
                            <div class="box box-warning">
                                <div class="box-header with-border">
                                    <h3 class="box-title">Loans</h3>
                                    <div class="box-tools pull-right">
                                        <button type="button" class="btn btn-box-tool" data-widget="collapse">
                                            <i class="fa fa-minus"></i>
                                        </button>
                                    </div>
                                </div>
                                <div class="box-body">
                                    <div class="flow">
                                        <asp:GridView CssClass="table-striped table-bordered table table-hover" ID="gvloan" runat="server" AutoGenerateColumns="False" PageSize='<%# int.Parse(Session["GridSize"].ToString()) %>'
                                            TabIndex="10" Width="100%" AllowPaging="True" OnPageIndexChanging="gvloan_PageIndexChanging">
                                            <Columns>
                                                <asp:TemplateField HeaderText="<%$ Resources:Attendance,Loan %>">
                                                    <ItemTemplate>
                                                        <asp:HiddenField ID="hdnloanId" runat="server" Value='<%# Eval("Loan_Id") %>' />
                                                        <asp:Label ID="lblloanname" runat="server" Text='<%# Eval("Loan_Id") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <ItemStyle />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="<%$ Resources:Attendance,Loan Amount %>">
                                                    <ItemTemplate>
                                                        <asp:HiddenField ID="hdntrnasLoanId" runat="server" Value='<%# Eval("Trans_Id") %>' />
                                                        <%--<asp:HiddenField ID="hdnmonth" runat="server" Value='<%# Eval("Month") %>' />--%>
                                                        <%--<asp:Label ID="lblloanamt" Visible="false" runat="server" Text='<%#Common.GetAmountDecimal(Eval("Montly_Installment").ToString(),Session["DBConnection"].ToString(),Session["LocCurrnecyId"].ToString()) %>' />
                                                        <asp:Label ID="lblActualLOan" runat="server" Text='<%# Common.GetAmountDecimal(Eval("Total_Amount").ToString(),Session["DBConnection"].ToString(),Session["LocCurrnecyId"].ToString()) %>' />--%>

                                                        <asp:Label ID="lblloanamt" Visible="false" runat="server" Text='<%#(Eval("Montly_Installment").ToString()) %>' />
                                                        <asp:Label ID="lblActualLOan" runat="server" Text='<%# (Eval("Total_Amount").ToString()) %>' />
                                                    </ItemTemplate>
                                                    <ItemStyle />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="<%$ Resources:Attendance,Act Loan Amount %>">
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="txtloanamt" MaxLength="10" AutoPostBack="true" OnTextChanged="txtloanamt_TextChanged" runat="server" CssClass="form-control" Text='<%# Eval("Net_Pending_Amount") %>'></asp:TextBox>
                                                        <%-- <asp:HiddenField ID="hdnLoanAmount" runat="server" Value='<%# Common.GetAmountDecimal(Eval("Net_Pending_Amount").ToString(),Session["DBConnection"].ToString(),Session["LocCurrnecyId"].ToString()) %>' />--%>

                                                        <asp:HiddenField ID="hdnLoanAmount" runat="server" Value='<%# Eval("Net_Pending_Amount").ToString() %>' />

                                                        <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtenderPAidPayroll" runat="server"
                                                            TargetControlID="txtloanamt" ValidChars="0,1,2,3,4,5,6,7,8,9,.">
                                                        </cc1:FilteredTextBoxExtender>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                            </Columns>


                                            <PagerStyle CssClass="pagination-ys" />

                                        </asp:GridView>
                                    </div>
                                </div>
                                <div class="box-footer" style="text-align: right">
                                    <div class="col-md-12" style="text-align: right">
                                        <div class="col-md-4"></div>
                                        <div class="col-md-4"></div>
                                        <div class="col-md-4" style="background-color: #EEEEEE">
                                            <asp:Label ID="Label45" runat="server" Text="<%$ Resources:Attendance,Total Loan Amount%>" Font-Size="13" Font-Bold="true" />
                                            <asp:Label ID="Label46" runat="server" Text=" = " Font-Bold="true" />
                                            <asp:Label ID="Lbl_Total_Loan_Amount" Font-Bold="true" Text="0.00" Font-Size="13" runat="server"></asp:Label>
                                        </div>
                                        <br />
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>

                    <div id="Div_allsaveCode" style="display: none;" runat="server" class="row">
                        <div class="col-md-12">
                            <div class="box box-warning">
                                <div class="box-header with-border">
                                    <h3 class="box-title"></h3>
                                    <div class="box-tools pull-right">
                                        <button type="button" class="btn btn-box-tool" data-widget="collapse">
                                            <i class="fa fa-minus"></i>
                                        </button>
                                    </div>
                                </div>
                                <div class="box-body">
                                    <div class="col-md-4">
                                        <asp:Button ID="btnsaveallow" Visible="false" runat="server" CausesValidation="False"
                                            CssClass="btn btn-success" OnClick="btnsaveallow_Click" TabIndex="10" Text="<%$ Resources:Attendance,Save %>" />
                                        &nbsp;&nbsp;
                                    <asp:Button ID="btncencelallow" Visible="false" runat="server" CausesValidation="False"
                                        CssClass="btn btn-danger" TabIndex="9" Text="<%$ Resources:Attendance,Cancel %>"
                                        OnClick="btncencelallow_Click" />
                                    </div>
                                    <div class="col-md-4">
                                        <asp:Button ID="btnSave" Visible="false" runat="server" CausesValidation="False"
                                            CssClass="btn btn-success" OnClick="BtnSave_Click" TabIndex="10" Text="<%$ Resources:Attendance,Save %>" />
                                        &nbsp;&nbsp;
                                    <asp:Button ID="btncencelDeduc" Visible="false" runat="server" CausesValidation="False"
                                        CssClass="btn btn-danger" TabIndex="9" Text="<%$ Resources:Attendance,Cancel %>"
                                        OnClick="btncencelDeduc_Click" />
                                    </div>
                                    <div class="col-md-4">
                                        <asp:Button ID="btnsaveclaimpenalty" Visible="false" runat="server" CausesValidation="False"
                                            CssClass="btn btn-success" OnClick="btnsaveclaimpenalty_Click" TabIndex="10" Text="<%$ Resources:Attendance,Save %>" />
                                        &nbsp;&nbsp;
                                    <asp:Button ID="btncencelpenaltyclaim" Visible="false" runat="server" CausesValidation="False"
                                        CssClass="btn btn-danger" OnClick="btncencelpenaltyclaim_Click" TabIndex="9" Text="<%$ Resources:Attendance,Cancel %>" />
                                    </div>
                                    <div class="col-md-4">
                                        <asp:Button ID="btnsaveloan" Visible="false" runat="server" CausesValidation="False"
                                            CssClass="btn btn-success" OnClick="btnsaveloan_Click" TabIndex="2" Text="<%$ Resources:Attendance,Save %>" />
                                        &nbsp;&nbsp;
                                    <asp:Button ID="btncenellaon" Visible="false" runat="server" CausesValidation="False"
                                        CssClass="btn btn-danger" TabIndex="3" Text="<%$ Resources:Attendance,Cancel %>"
                                        OnClick="btncenellaon_Click" />
                                    </div>
                                    <div class="col-md-4">
                                        <asp:Button ID="btnsaveattend" Visible="false" runat="server" CausesValidation="False"
                                            CssClass="btn btn-success" OnClick="btnsaveattend_Click" Text="<%$ Resources:Attendance,Save %>" />
                                        &nbsp;&nbsp;
                                    <asp:Button ID="btncencelattend" Visible="false" runat="server" CausesValidation="False"
                                        CssClass="btn btn-danger" OnClick="btncencelattend_Click" Text="<%$ Resources:Attendance,Cancel %>" />
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div id="Div_gvallowance1" style="display: none;" runat="server" class="row">
                        <div class="col-md-12">
                            <div class="box box-warning">
                                <div class="box-header with-border">
                                    <h3 class="box-title"></h3>
                                    <div class="box-tools pull-right">
                                        <button type="button" class="btn btn-box-tool" data-widget="collapse">
                                            <i class="fa fa-minus"></i>
                                        </button>
                                    </div>
                                </div>
                                <div class="box-body">
                                    <div class="flow">
                                        <asp:GridView CssClass="table-striped table-bordered table table-hover" ID="gvallowance1" runat="server" AutoGenerateColumns="False" DataKeyNames="Emp_Id"
                                            TabIndex="10" Width="100%" PageSize='<%# int.Parse(Session["GridSize"].ToString()) %>' AllowPaging="True"
                                            OnPageIndexChanging="gvallowance1_PageIndexChanging">
                                            <Columns>
                                                <asp:TemplateField HeaderText="<%$ Resources:Attendance,Allowance Type %>">
                                                    <ItemTemplate>
                                                        <asp:HiddenField ID="hdnEmpId" runat="server" Value='<%# Eval("Emp_Id") %>' />
                                                        <asp:HiddenField ID="hdnallowId" runat="server" Value='<%# Eval("Allowance_Id") %>' />
                                                        <asp:Label ID="lblType" runat="server" Text='<%# Eval("Allowance_Id") %>'></asp:Label>
                                                        <asp:HiddenField ID="hdntransId" runat="server" Value='<%# Eval("Trans_Id") %>' />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="<%$ Resources:Attendance, Allowance Value %>">
                                                    <ItemTemplate>
                                                        <asp:HiddenField ID="hdnRefId" runat="server" Value='<%# Eval("Ref_Id") %>' />
                                                        <asp:Label ID="lblAllowValue" runat="server" Text='<%# Eval("Allowance_Value") %>' />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="<%$ Resources:Attendance,Act Allowance Value %>">
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="txtValue" MaxLength="10" CssClass="form-control" runat="server" Text='<%# Eval("Act_Allowance_Value") %>'></asp:TextBox>
                                                        <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtenderPAidPayroll" runat="server"
                                                            TargetControlID="txtValue" ValidChars="0,1,2,3,4,5,6,7,8,9,.">
                                                        </cc1:FilteredTextBoxExtender>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField Visible="false">
                                                    <ItemTemplate>
                                                        <asp:CheckBox ID="chkallowance" runat="server" Text="<%$ Resources:Attendance,IsMonthCarry%>" />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                            </Columns>


                                            <PagerStyle CssClass="pagination-ys" />

                                        </asp:GridView>
                                    </div>
                                    <div style="text-align: center;">
                                        <asp:Button ID="Button1" runat="server" CausesValidation="False" CssClass="btn btn-primary"
                                            OnClick="btnsaveallow1_Click" TabIndex="10" Text="<%$ Resources:Attendance,Save %>" />
                                        &nbsp;&nbsp;
                                    <asp:Button ID="Button2" runat="server" CausesValidation="False" CssClass="btn btn-primary"
                                        TabIndex="9" Text="<%$ Resources:Attendance,Cancel %>" OnClick="btncencelallow1_Click" />
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div id="Div_gvdeduuc1" style="display: none;" runat="server" class="row">
                        <div class="col-md-12">
                            <div class="box box-warning">
                                <div class="box-header with-border">
                                    <h3 class="box-title"></h3>
                                    <div class="box-tools pull-right">
                                        <button type="button" class="btn btn-box-tool" data-widget="collapse">
                                            <i class="fa fa-minus"></i>
                                        </button>
                                    </div>
                                </div>
                                <div class="box-body">
                                    <div style="overflow: auto">
                                        <asp:GridView CssClass="table-striped table-bordered table table-hover" ID="gvdeduction1" runat="server" AutoGenerateColumns="False" DataKeyNames="Emp_Id"
                                            PageSize='<%# int.Parse(Session["GridSize"].ToString()) %>' TabIndex="10" Width="100%" AllowPaging="True"
                                            OnPageIndexChanging="gvdeduction1_PageIndexChanging">
                                            <Columns>
                                                <asp:TemplateField HeaderText="<%$ Resources:Attendance,Deduction %>">
                                                    <ItemTemplate>
                                                        <asp:HiddenField ID="hdnEmpId" runat="server" Value='<%# Eval("Emp_Id") %>' />
                                                        <asp:HiddenField ID="hdnDeducId" runat="server" Value='<%# Eval("Deduction_Id") %>' />
                                                        <asp:Label ID="lblTypededuc" runat="server" Text='<%# Eval("Deduction_Id") %>'></asp:Label>
                                                        <asp:HiddenField ID="hdntransIddeduc" runat="server" Value='<%# Eval("Trans_Id") %>' />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="<%$ Resources:Attendance,Deduction Value %>">
                                                    <ItemTemplate>
                                                        <asp:HiddenField ID="hdnRefId" runat="server" Value='<%# Eval("Ref_Id") %>' />
                                                        <asp:Label ID="lblDeductValue" runat="server" Text='<%# Eval("Deduction_Value") %>' />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="<%$ Resources:Attendance,Act Deduction Value %>">
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="txtDeducValue" MaxLength="10" CssClass="form-control" runat="server" Text='<%# Eval("Act_Deduction_Value") %>'></asp:TextBox>
                                                        <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtenderPAidPayroll" runat="server"
                                                            TargetControlID="txtDeducValue" ValidChars="0,1,2,3,4,5,6,7,8,9,.">
                                                        </cc1:FilteredTextBoxExtender>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField Visible="false">
                                                    <ItemTemplate>
                                                        <asp:CheckBox ID="chkdeduc" runat="server" Text="<%$ Resources:Attendance,IsMonthCarry%>" />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                            </Columns>


                                            <PagerStyle CssClass="pagination-ys" />

                                        </asp:GridView>
                                    </div>
                                    <div style="text-align: center">
                                        <asp:Button ID="Button3" runat="server" CausesValidation="False" CssClass="btn btn-primary"
                                            OnClick="BtnSave1_Click" TabIndex="10" Text="<%$ Resources:Attendance,Save %>" />
                                        &nbsp;&nbsp;
                                    <asp:Button ID="Button4" runat="server" CausesValidation="False" CssClass="btn btn-primary"
                                        TabIndex="9" Text="<%$ Resources:Attendance,Cancel %>" OnClick="btncencelDeduc1_Click" />
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div id="Div_penaltyclaim1" style="display: none;" runat="server" class="row">
                        <div class="col-md-12">
                            <div class="box box-warning">
                                <div class="box-header with-border">
                                    <h3 class="box-title"></h3>
                                    <div class="box-tools pull-right">
                                        <button type="button" class="btn btn-box-tool" data-widget="collapse">
                                            <i class="fa fa-minus"></i>
                                        </button>
                                    </div>
                                </div>
                                <div class="box-body">
                                    <div style="overflow: auto">
                                        <asp:GridView CssClass="table-striped table-bordered table table-hover" ID="gvPenalty1" runat="server" AutoGenerateColumns="False" DataKeyNames="Emp_Id"
                                            PageSize='<%# int.Parse(Session["GridSize"].ToString()) %>' TabIndex="10" Width="100%">
                                            <Columns>
                                                <asp:TemplateField Visible="false" HeaderText="<%$ Resources:Attendance,Penalty Id %>">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblPenaltyId1" runat="server" Text='<%# Eval("Penalty_Id") %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="<%$ Resources:Attendance,Penalty Name %>">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblPenaltyId2" runat="server" Text='<%# Eval("Penalty_Name") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <ItemStyle />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="<%$ Resources:Attendance,Actual Value %>">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblPenaltyValue1" runat="server" Text='<%# Eval("Field1") %>' />
                                                    </ItemTemplate>
                                                    <ItemStyle />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="<%$ Resources:Attendance,Update Value %>">
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="txtPenaltyValue" MaxLength="10" CssClass="form-control" AutoPostBack="true" OnTextChanged="txtPenaltyValue1_OnTextChanged"
                                                            runat="server" Text='<%# Eval("Field2") %>'></asp:TextBox>
                                                        <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtenderPAidPayroll" runat="server"
                                                            TargetControlID="txtPenaltyValue" ValidChars="0,1,2,3,4,5,6,7,8,9,.">
                                                        </cc1:FilteredTextBoxExtender>
                                                    </ItemTemplate>
                                                    <ItemStyle />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderStyle-HorizontalAlign="Left" HeaderText="<%$ Resources:Attendance,Description %>">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblPenaltydesc" runat="server" Text='<%# Eval("Penalty_Description") %>' />
                                                    </ItemTemplate>
                                                    <ItemStyle />
                                                </asp:TemplateField>
                                            </Columns>


                                            <PagerStyle CssClass="pagination-ys" />

                                        </asp:GridView>
                                    </div>
                                    <div class="col-md-12">
                                        <div class="col-md-4">
                                            <asp:Label ID="lblpenalty1" runat="server" Text="<%$ Resources:Attendance,Total Penalty%>"
                                                Font-Bold="true" />
                                            &nbsp:&nbsp
                                            <asp:Label ID="lbltotalpenalty1" runat="server" Font-Bold="true"></asp:Label>
                                        </div>
                                        <div class="col-md-4">
                                            <asp:TextBox ID="txttotalpeanlty1" CssClass="form-control" ReadOnly="true" runat="server" Text='<%# Eval("Employee_Penalty") %>'></asp:TextBox>
                                            <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender15" runat="server" TargetControlID="txttotalpeanlty1"
                                                ValidChars="0,1,2,3,4,5,6,7,8,9,.">
                                            </cc1:FilteredTextBoxExtender>
                                        </div>
                                        <div style="display: none;" class="col-md-4">
                                            <asp:CheckBox ID="chkpenalty1" runat="server" Text="<%$ Resources:Attendance,IsMonthCarry%>" />
                                        </div>
                                    </div>
                                    <div class="col-md-12" style="overflow: auto">
                                        <asp:GridView CssClass="table-striped table-bordered table table-hover" ID="GvClaim1" runat="server" AutoGenerateColumns="False" DataKeyNames="Emp_Id"
                                            PageSize='<%# int.Parse(Session["GridSize"].ToString()) %>' TabIndex="10" Width="100%">
                                            <Columns>
                                                <asp:TemplateField Visible="false" HeaderText="<%$ Resources:Attendance,Claim Id %>">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblClaimId1" runat="server" Text='<%# Eval("Claim_Id") %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="<%$ Resources:Attendance,Claim Name %>">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblClaimId2" runat="server" Text='<%# Eval("Claim_Name") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <ItemStyle />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="<%$ Resources:Attendance,Actual Value %>">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblClaimValue1" runat="server" Text='<%# Eval("Field1") %>' />
                                                    </ItemTemplate>
                                                    <ItemStyle />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="<%$ Resources:Attendance,Update Value %>">
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="txtClaimValue" MaxLength="10" CssClass="form-control" AutoPostBack="true" OnTextChanged="txtClaimValue1_OnTextChanged"
                                                            runat="server" Text='<%# Eval("Field2") %>'></asp:TextBox>
                                                        <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtenderPAidPayroll" runat="server"
                                                            TargetControlID="txtClaimValue" ValidChars="0,1,2,3,4,5,6,7,8,9,.">
                                                        </cc1:FilteredTextBoxExtender>
                                                    </ItemTemplate>
                                                    <ItemStyle />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderStyle-HorizontalAlign="Left" HeaderText="<%$ Resources:Attendance,Description %>">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblClaimdesc" runat="server" Text='<%# Eval("Claim_Description") %>' />
                                                    </ItemTemplate>
                                                    <ItemStyle />
                                                </asp:TemplateField>
                                            </Columns>


                                            <PagerStyle CssClass="pagination-ys" />

                                        </asp:GridView>
                                    </div>
                                    <div class="col-md-12">
                                        <div class="col-md-4">
                                            <asp:Label ID="lblclaim1" runat="server" Text="<%$ Resources:Attendance,Total Claim%>"
                                                Font-Bold="true" />
                                            &nbsp:&nbsp<asp:Label ID="lbltotalclaim1" runat="server" Font-Bold="true"></asp:Label>
                                        </div>
                                        <div class="col-md-4">

                                            <asp:TextBox ID="txttotalclaim1" CssClass="form-control" ReadOnly="true" runat="server" Text='<%# Eval("Employee_Claim") %>'></asp:TextBox>
                                            <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender16" runat="server" TargetControlID="txttotalclaim1"
                                                ValidChars="0,1,2,3,4,5,6,7,8,9,.">
                                            </cc1:FilteredTextBoxExtender>
                                        </div>
                                        <div style="display: none;" class="col-md-4">
                                            <asp:CheckBox ID="chkclaim1" runat="server" Text="<%$ Resources:Attendance,IsMonthCarry%>"></asp:CheckBox>
                                        </div>
                                    </div>
                                    <div class="col-md-12">
                                        <div class="col-md-4">
                                            <asp:Label ID="lblduepayamt1" runat="server" Text="<%$ Resources:Attendance,Adjustment Amount%>"
                                                Font-Bold="true" />
                                            &nbsp:&nbsp<asp:Label ID="lbldueamt1" runat="server" Text="" Font-Bold="true"></asp:Label>
                                        </div>
                                        <div class="col-md-4">
                                            <asp:TextBox ID="txtdueamt1" CssClass="form-control" runat="server" Text="" ReadOnly="true"></asp:TextBox>
                                            <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender17" runat="server" TargetControlID="txtdueamt1"
                                                ValidChars="0,1,2,3,4,5,6,7,8,9,-,.">
                                            </cc1:FilteredTextBoxExtender>
                                        </div>
                                    </div>
                                    <div class="col-md-12" style="text-align: center">
                                        <asp:Button ID="Button5" runat="server" CausesValidation="False" CssClass="btn btn-primary"
                                            OnClick="btnsaveclaimpenalty1_Click" TabIndex="10" Text="<%$ Resources:Attendance,Save %>" />
                                        &nbsp;&nbsp;
                                    <asp:Button ID="Button6" runat="server" CausesValidation="False" CssClass="btn btn-primary"
                                        OnClick="btncencelpenaltyclaim1_Click" TabIndex="9" Text="<%$ Resources:Attendance,Cancel %>" />
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div id="Div_loan1" style="display: none;" runat="server" class="row">
                        <div class="col-md-12">
                            <div class="box box-warning">
                                <div class="box-header with-border">
                                    <h3 class="box-title"></h3>
                                    <div class="box-tools pull-right">
                                        <button type="button" class="btn btn-box-tool" data-widget="collapse">
                                            <i class="fa fa-minus"></i>
                                        </button>
                                    </div>
                                </div>
                                <div class="box-body">
                                    <div style="overflow: auto">
                                        <asp:GridView CssClass="table-striped table-bordered table table-hover" ID="gvloan1" runat="server" AutoGenerateColumns="False" PageSize='<%# int.Parse(Session["GridSize"].ToString()) %>'
                                            TabIndex="10" Width="100%" AllowPaging="True" OnPageIndexChanging="gvloan1_PageIndexChanging">
                                            <Columns>
                                                <asp:TemplateField HeaderText="<%$ Resources:Attendance,Loan %>">
                                                    <ItemTemplate>
                                                        <asp:HiddenField ID="hdnloanId" runat="server" Value='<%# Eval("Loan_Id") %>' />
                                                        <asp:Label ID="lblloanname" runat="server" Text='<%# Eval("Loan_Id") %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="<%$ Resources:Attendance,Loan Amount %>">
                                                    <ItemTemplate>
                                                        <asp:HiddenField ID="hdntrnasLoanId" runat="server" Value='<%# Eval("Trans_Id") %>' />
                                                        <%--<asp:HiddenField ID="hdnmonth" runat="server" Value='<%# Eval("Month") %>' />--%>
                                                        <asp:Label ID="lblloanamt" Visible="false" runat="server" Text='<%# Eval("Montly_Installment") %>' />
                                                        <asp:Label ID="lblActualLOan" runat="server" Text='<%# Eval("Total_Amount") %>' />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="<%$ Resources:Attendance,Act Loan Amount %>">
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="txtloanamt" MaxLength="10" CssClass="form-control" runat="server" Text='<%# Eval("Net_Pending_Amount") %>'></asp:TextBox>
                                                        <asp:HiddenField ID="hdnLoanAmount" runat="server" Value='<%# Eval("Net_Pending_Amount") %>' />
                                                        <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtenderPAidPayroll" runat="server"
                                                            TargetControlID="txtloanamt" ValidChars="0,1,2,3,4,5,6,7,8,9,.">
                                                        </cc1:FilteredTextBoxExtender>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                            </Columns>


                                            <PagerStyle CssClass="pagination-ys" />

                                        </asp:GridView>
                                    </div>
                                    <div style="text-align: center">
                                        <asp:Button ID="Button7" runat="server" CausesValidation="False" CssClass="btn btn-primary"
                                            OnClick="btnsaveloan1_Click" TabIndex="2" Text="<%$ Resources:Attendance,Save %>" />
                                        &nbsp;&nbsp;
                                    <asp:Button ID="Button8" runat="server" CausesValidation="False" CssClass="btn btn-primary"
                                        TabIndex="3" Text="<%$ Resources:Attendance,Cancel %>" OnClick="btncenellaon1_Click" />
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div id="Div_attnd1" style="display: none;" runat="server" class="row">
                        <div class="col-md-12">
                            <div class="box box-warning">
                                <div class="box-header with-border">
                                    <h3 class="box-title"></h3>
                                    <div class="box-tools pull-right">
                                        <button type="button" class="btn btn-box-tool" data-widget="collapse">
                                            <i class="fa fa-minus"></i>
                                        </button>
                                    </div>
                                </div>
                                <div class="box-body">
                                    <div class="col-md-4">
                                        <asp:Label ID="Label31" runat="server" Text="<%$ Resources:Attendance,Basic Salary %>"
                                            Font-Bold="true" />
                                        <asp:TextBox ID="txtBasicSalary1" CssClass="form-control" ReadOnly="true" runat="server"></asp:TextBox>
                                    </div>
                                    <div class="col-md-4">
                                        <asp:Label ID="Label19" runat="server" Text="<%$ Resources:Attendance,Worked Salary %>"
                                            Font-Bold="true" />
                                        <asp:Label ID="lblmonth1" runat="server" Visible="false" />
                                        <asp:Label ID="lblyear1" runat="server" Visible="false" />
                                        <asp:Label ID="lbltrnsid1" runat="server" Visible="false" />
                                        <asp:TextBox ID="txtworkedminsal1" MaxLength="10" CssClass="form-control" runat="server" Text='<%# Eval("Worked_Min_Salary") %>'
                                            AutoPostBack="true" OnTextChanged="txtworkedminsal1_TextChanged"
                                            onblur="checkDec(this);"></asp:TextBox>
                                        <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender18" runat="server" TargetControlID="txtworkedminsal1"
                                            ValidChars="0,1,2,3,4,5,6,7,8,9,.">
                                        </cc1:FilteredTextBoxExtender>
                                    </div>
                                    <div class="col-md-4">
                                        <asp:Label ID="Label20" runat="server" Text="<%$ Resources:Attendance,Normal OT Salary %>"
                                            Font-Bold="true" />
                                        <asp:TextBox ID="txtnormalOtminsal1" MaxLength="10" CssClass="form-control" runat="server" Text='<%# Eval("Normal_OT_Min_Salary") %>'
                                            AutoPostBack="true" OnTextChanged="txtnormalOtminsal1_TextChanged"
                                            onblur="checkDec(this);"></asp:TextBox>
                                        <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender19" runat="server" TargetControlID="txtnormalOtminsal1"
                                            ValidChars="0,1,2,3,4,5,6,7,8,9,.">
                                        </cc1:FilteredTextBoxExtender>
                                    </div>
                                    <div class="col-md-4">
                                        <asp:Label ID="Label21" runat="server" Text="<%$ Resources:Attendance,Week Off OT Salary %>"
                                            Font-Bold="true" />
                                        <asp:TextBox ID="txtWeekoffotminsal1" MaxLength="10" CssClass="form-control" runat="server" Text='<%# Eval("Week_Off_OT_Min_Salary") %>'
                                            AutoPostBack="true" OnTextChanged="txtWeekoffotminsal1_TextChanged"
                                            onblur="checkDec(this);"></asp:TextBox>
                                        <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender20" runat="server" TargetControlID="txtWeekoffotminsal1"
                                            ValidChars="0,1,2,3,4,5,6,7,8,9,.">
                                        </cc1:FilteredTextBoxExtender>
                                    </div>
                                    <div class="col-md-4">
                                        <asp:Label ID="Label22" runat="server" Text="<%$ Resources:Attendance,Holiday OT Salary %>"
                                            Font-Bold="true" />
                                        <asp:TextBox ID="txthloidayotminsal1" MaxLength="10" CssClass="form-control" runat="server" Text='<%# Eval("Holiday_OT_Min_Salary") %>'
                                            AutoPostBack="true" OnTextChanged="txthloidayotminsal1_TextChanged"
                                            onblur="checkDec(this);"></asp:TextBox>
                                        <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender21" runat="server" TargetControlID="txthloidayotminsal1"
                                            ValidChars="0,1,2,3,4,5,6,7,8,9,.">
                                        </cc1:FilteredTextBoxExtender>
                                    </div>
                                    <div class="col-md-4">
                                        <asp:Label ID="Label23" runat="server" Text="<%$ Resources:Attendance,Leave Days Salary %>"
                                            Font-Bold="true" />
                                        <asp:TextBox ID="txtPayrolldayssal1" MaxLength="10" CssClass="form-control" runat="server" Text='<%# Eval("Leave_Days_Salary") %>'
                                            AutoPostBack="true" OnTextChanged="txtPayrolldayssal1_TextChanged"
                                            onblur="checkDec(this);"></asp:TextBox>
                                        <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender22" runat="server" TargetControlID="txtPayrolldayssal1"
                                            ValidChars="0,1,2,3,4,5,6,7,8,9,.">
                                        </cc1:FilteredTextBoxExtender>
                                    </div>
                                    <div class="col-md-4">
                                        <asp:Label ID="Label24" runat="server" Text="<%$ Resources:Attendance,Week Off Salary %>"
                                            Font-Bold="true" />
                                        <asp:TextBox ID="txtweekoffsal1" MaxLength="10" CssClass="form-control" runat="server" Text='<%# Eval("Week_Off_Salary") %>'
                                            AutoPostBack="true" OnTextChanged="txtweekoffsal1_TextChanged"
                                            onblur="checkDec(this);"></asp:TextBox>
                                        <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender23" runat="server" TargetControlID="txtweekoffsal1"
                                            ValidChars="0,1,2,3,4,5,6,7,8,9,.">
                                        </cc1:FilteredTextBoxExtender>
                                    </div>
                                    <div class="col-md-4">
                                        <asp:Label ID="Label25" runat="server" Text="<%$ Resources:Attendance,Holidays Salary %>"
                                            Font-Bold="true" />
                                        <asp:TextBox ID="txtholidayssal1" MaxLength="10" CssClass="form-control" runat="server" Text='<%# Eval("Holidays_Salary") %>'
                                            AutoPostBack="true" OnTextChanged="txtholidayssal1_TextChanged"
                                            onblur="checkDec(this);"></asp:TextBox>
                                        <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender24" runat="server" TargetControlID="txtholidayssal1"
                                            ValidChars="0,1,2,3,4,5,6,7,8,9,.">
                                        </cc1:FilteredTextBoxExtender>
                                    </div>
                                    <div class="col-md-4">
                                        <asp:Label ID="Label26" runat="server" Text="<%$ Resources:Attendance,Absent Penalty%>"
                                            Font-Bold="true" />
                                        <asp:TextBox ID="txtAbsaentsal1" MaxLength="10" CssClass="form-control" runat="server" Text='<%# Eval("Absent_Salary") %>'
                                            AutoPostBack="true" OnTextChanged="txtAbsaentsal1_TextChanged"
                                            onblur="checkDec(this);"></asp:TextBox>
                                        <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender25" runat="server" TargetControlID="txtAbsaentsal1"
                                            ValidChars="0,1,2,3,4,5,6,7,8,9,.">
                                        </cc1:FilteredTextBoxExtender>
                                    </div>
                                    <div class="col-md-4">
                                        <asp:Label ID="Label27" runat="server" Text="<%$ Resources:Attendance,Late Penalty%>"
                                            Font-Bold="true" />
                                        <asp:TextBox ID="txtlateminsal1" MaxLength="10" CssClass="form-control" runat="server" Text='<%# Eval("Late_Min_Penalty") %>'
                                            AutoPostBack="true" OnTextChanged="txtlateminsal1_TextChanged"
                                            onblur="checkDec(this);"></asp:TextBox>
                                        <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender26" runat="server" TargetControlID="txtlateminsal1"
                                            ValidChars="0,1,2,3,4,5,6,7,8,9,.">
                                        </cc1:FilteredTextBoxExtender>
                                    </div>
                                    <div class="col-md-4">
                                        <asp:Label ID="Label28" runat="server" Text="<%$ Resources:Attendance,Early Penalty%>"
                                            Font-Bold="true" />
                                        <asp:TextBox ID="txtearlyminsal1" MaxLength="10" CssClass="form-control" runat="server" Text='<%# Eval("Early_Min_Penalty") %>'
                                            AutoPostBack="true" OnTextChanged="txtearlyminsal1_TextChanged"
                                            onblur="checkDec(this);"></asp:TextBox>
                                        <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender27" runat="server" TargetControlID="txtearlyminsal1"
                                            ValidChars="0,1,2,3,4,5,6,7,8,9,.">
                                        </cc1:FilteredTextBoxExtender>
                                    </div>
                                    <div class="col-md-4">
                                        <asp:Label ID="Label29" runat="server" Text="<%$ Resources:Attendance,Partial Violation Penalty%>"
                                            Font-Bold="true" />
                                        <asp:TextBox ID="txtpatialvoisal1" MaxLength="10" CssClass="form-control" runat="server" Text='<%# Eval("Patial_Violation_Penalty") %>'
                                            AutoPostBack="true" OnTextChanged="txtearlyminsal1_TextChanged"
                                            onblur="checkDec(this);"></asp:TextBox>
                                        <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender28" runat="server" TargetControlID="txtpatialvoisal1"
                                            ValidChars="0,1,2,3,4,5,6,7,8,9,.">
                                        </cc1:FilteredTextBoxExtender>
                                    </div>
                                    <div class="col-md-4">
                                        <asp:Label ID="Label30" runat="server" Text="<%$ Resources:Attendance,Gross Amount %>"
                                            Font-Bold="true" />
                                        <asp:TextBox ID="txtGrossAmount1" MaxLength="10" CssClass="form-control" runat="server" ReadOnly="true"></asp:TextBox>
                                        <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender29" runat="server" TargetControlID="txtGrossAmount1"
                                            ValidChars="0,1,2,3,4,5,6,7,8,9,.">
                                        </cc1:FilteredTextBoxExtender>
                                    </div>
                                    <div class="col-md-12" style="text-align: center">
                                        <asp:Button ID="Button9" runat="server" CausesValidation="False" CssClass="btn btn-primary"
                                            OnClick="btnsaveattend1_Click" TabIndex="10" Text="<%$ Resources:Attendance,Save %>" />
                                        &nbsp;&nbsp;
                                    <asp:Button ID="Button10" runat="server" CausesValidation="False" CssClass="btn btn-primary"
                                        OnClick="btncencelattend1_Click" TabIndex="9" Text="<%$ Resources:Attendance,Cancel %>" />
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>

                    <div class="col-md-12" style="text-align: right;">
                        <div class="col-md-4"></div>
                        <div class="col-md-4"></div>
                        <div class="col-md-4" style="background-color: #EEEEEE">
                            <asp:Label ID="Label47" runat="server" Text="<%$ Resources:Attendance,Total Final Paid Amount%>" Font-Size="13" Font-Bold="true" />
                            <asp:Label ID="Label48" runat="server" Text=" = " Font-Bold="true" />
                            <asp:Label ID="Lbl_Total_Final_Paid_Amount" Font-Bold="true" Text="0.00" Font-Size="13" runat="server"></asp:Label>
                        </div>
                        <br />
                    </div>
                    <div class="row">
                        <div class="col-md-12" style="text-align: center">
                            <asp:Button ID="btnSavePayroll" runat="server" CausesValidation="False" CssClass="btn btn-success"
                                OnClick="btnSavePayroll_Click" TabIndex="10" Text="<%$ Resources:Attendance,Save %>" />
                            <asp:Button ID="BtnCancelPayroll" runat="server" CausesValidation="False" CssClass="btn btn-danger"
                                OnClick="BtnCancelPayroll_Click" TabIndex="9" Text="<%$ Resources:Attendance,Cancel %>" />
                        </div>
                    </div>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>


    <asp:UpdateProgress ID="UpdateProgress1" runat="server" AssociatedUpdatePanelID="Update_Form">
        <ProgressTemplate>
            <div class="modal_Progress">
                <div class="center_Progress">
                    <img alt="" src="../Images/ajax-loader2.gif" style="vertical-align: middle; background-color: White; border-radius: 10px;" />
                </div>
            </div>
        </ProgressTemplate>
    </asp:UpdateProgress>


    <asp:Panel ID="Panel4" runat="server" Visible="false"></asp:Panel>
    <asp:Panel ID="pnlmenuloan" runat="server" Visible="false"></asp:Panel>
    <asp:Panel ID="pnlMenuDeduction" runat="server" Visible="false"></asp:Panel>
    <asp:Panel ID="pnlMenupenaltyclaim" runat="server" Visible="false"></asp:Panel>
    <asp:Panel ID="pnlMenuAllowance" runat="server" Visible="false"></asp:Panel>
    <asp:Panel ID="pnlattendance" runat="server" Visible="false"></asp:Panel>
    <asp:Panel ID="panel7" runat="server" Visible="false"></asp:Panel>
    <asp:Panel ID="panel8" runat="server" Visible="false"></asp:Panel>
    <asp:Panel ID="panel9" runat="server" Visible="false"></asp:Panel>
    <asp:Panel ID="panel10" runat="server" Visible="false"></asp:Panel>


</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="FooterContent" runat="Server">
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="AfterLoadScriptContent" runat="Server">
    <script src="../Script/common.js"></script>
    <script type="text/javascript">
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


    </script>
    <script>
        function checkDec(el) {
            var ex = /^[0-9]+\.?[0-9]*$/;
            if (ex.test(el.value) == false) {
                alert('Incorrect Number');
            }
        }

        <%--       function Get_Total_Allowances(id) {
            var sum = 0.00;
            var itemsum = 0.00;
            document.getElementById('<%= Lbl_Total_Allowances.ClientID %>').value = "0.00";
            var gridView = document.getElementById('<%= gvallowance.ClientID %>');            
           for (var i = 1; i < gridView.rows.length; i++) {
               if (!isNaN(gridView.rows[i].cells[2].getElementsByTagName("input")[0].value))
               {
                   itemsum = parseFloat(gridView.rows[i].cells[2].getElementsByTagName("input")[0].value);
                   sum = sum + itemsum;
                   document.getElementById('<%= Lbl_Total_Allowances.ClientID %>').innerHTML =common sum;
               }
            }
           Total_Final_paid_Amount();
       }



        function Get_Total_Deductions(id) {
            var sum = 0.00;
            var itemsum = 0.00;
            document.getElementById('<%= Lbl_Total_Deductions.ClientID %>').value = "0.00";
            var gridView = document.getElementById('<%= gvdeduction.ClientID %>');            
            for (var i = 1; i < gridView.rows.length; i++) {
                if (!isNaN(gridView.rows[i].cells[2].getElementsByTagName("input")[0].value))
                {
                    itemsum = parseFloat(gridView.rows[i].cells[2].getElementsByTagName("input")[0].value);
                    sum = sum + itemsum;
                    document.getElementById('<%= Lbl_Total_Deductions.ClientID %>').innerHTML = sum;
                }
            }
            Total_Final_paid_Amount();
        }

          function Get_Total_Loan(id) {
            var sum = 0.00;
            var itemsum = 0.00;
            document.getElementById('<%= Lbl_Total_Loan_Amount.ClientID %>').value = "0.00";
            var gridView = document.getElementById('<%= gvloan.ClientID %>');            
              for (var i = 1; i < gridView.rows.length; i++) {
                  if (!isNaN(gridView.rows[i].cells[2].getElementsByTagName("input")[0].value))
                  {
                      itemsum = parseFloat(gridView.rows[i].cells[2].getElementsByTagName("input")[0].value);
                      sum = sum + itemsum;
                      document.getElementById('<%= Lbl_Total_Loan_Amount.ClientID %>').innerHTML = sum;
                  }
            }
              Total_Final_paid_Amount();
          }

        function Total_Final_paid_Amount() {            
            var Net_Attendance_Salary = document.getElementById('<%= txtGrossAmount.ClientID %>').innerHTML;
            var Total_Allowances = document.getElementById('<%= Lbl_Total_Allowances.ClientID %>').innerHTML;
            var Total_Deductions = document.getElementById('<%= Lbl_Total_Deductions.ClientID %>').innerHTML;

            var Total_Claim_Penalty = document.getElementById('<%= txttotalpeanlty.ClientID %>').value;
            var Total_Claim = document.getElementById('<%= txttotalclaim.ClientID %>').value;

            var Total_Loan_Amount = document.getElementById('<%= Lbl_Total_Loan_Amount.ClientID %>').innerHTML;
                        
            var sum = parseFloat(Net_Attendance_Salary + Total_Allowances + Total_Claim);
            
            var deduct = parseFloat(Total_Deductions + Total_Claim_Penalty + Total_Loan_Amount);
            
            var tot = sum - deduct;
            alert(tot);
        }--%>
    </script>
</asp:Content>
