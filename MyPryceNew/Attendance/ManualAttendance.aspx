<%@ Page Title="" Language="C#" MasterPageFile="~/ERPMaster.master" AutoEventWireup="true" CodeFile="ManualAttendance.aspx.cs" Inherits="Attendance_ManualAttendance" %>


<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="Head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="HeaderContent" runat="Server">
    <h1>

        <i class="fas fa-hand-point-up"></i>&nbsp;&nbsp;
        <asp:Label ID="lblHeader" runat="server" Text="<%$ Resources:Attendance,Manual Attendance Setup %>"></asp:Label>
    </h1>
    <ol class="breadcrumb">
        <li><a href="#"><i class="fa fa-dashboard"></i>
            <asp:Label ID="Label6" runat="server" Text="<%$ Resources:Attendance,Attendance Module%>"></asp:Label></a></li>
        <li><a href="#"><i class="fa fa-dashboard"></i>
            <asp:Label ID="Label10" runat="server" Text="<%$ Resources:Attendance,Attendance Module%>"></asp:Label></a></li>
        <li class="active">
            <asp:Label ID="Label5" runat="server" Text="<%$ Resources:Attendance,Manual Attendance Setup %>"></asp:Label></li>
    </ol>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="Server">
    <asp:UpdatePanel ID="Update_Button" runat="server">
        <ContentTemplate>
            <asp:Button ID="Btn_List" Style="display: none;" runat="server" OnClick="btnList_Click" Text="List" />
            <asp:Button ID="Btn_New" Style="display: none;" runat="server" OnClick="btnNew_Click" Text="New" />
            <asp:Button ID="Btn_Bin" Style="display: none;" runat="server" OnClick="btnBin_Click" Text="Bin" />
            <asp:Button ID="Btn_Upload" Style="display: none;" runat="server" OnClick="btnUpload_Click" Text="Bin" />
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
                        <i class="fa fa-upload"></i>&nbsp;&nbsp;<asp:Label ID="Label2" runat="server" Text="<%$ Resources:Attendance,Upload %>"></asp:Label></a></li>
                    <li id="Li_Bin"><a href="#Bin" onclick="Li_Tab_Bin()" data-toggle="tab">
                        <i class="fa fa-trash"></i>&nbsp;&nbsp;<asp:Label ID="Label13" runat="server" Text="<%$ Resources:Attendance,Bin %>"></asp:Label></a></li>
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

                                                    <div class="col-md-3">
                                                        <asp:Label ID="lblLocationId" runat="server" Text="<%$ Resources:Attendance,Location%>"></asp:Label>
                                                        <asp:DropDownList ID="ddlLocationList" runat="server" Class="form-control">
                                                        </asp:DropDownList><br />
                                                    </div>

                                                    <div class="col-md-2">
                                                        <asp:Panel ID="Pnl_From_Date" runat="server" DefaultButton="btnSearch">
                                                            <asp:Label ID="lblFromDate" runat="server" Text="<%$ Resources:Attendance,From Date %>"></asp:Label>
                                                            <asp:TextBox ID="txtFromDate" CssClass="form-control" runat="server"></asp:TextBox>
                                                            <cc1:CalendarExtender OnClientShown="showCalendar" ID="CalendarExtender1" runat="server" Enabled="True" TargetControlID="txtFromDate">
                                                            </cc1:CalendarExtender>
                                                        </asp:Panel>
                                                        <br />
                                                    </div>
                                                    <div class="col-md-2">
                                                        <asp:Panel ID="Pnl_To_Date" runat="server" DefaultButton="btnSearch">
                                                            <asp:Label ID="lblToDate" runat="server" Text="<%$ Resources:Attendance,To Date %>"></asp:Label>
                                                            <asp:TextBox ID="txtToDate" CssClass="form-control" runat="server"></asp:TextBox>
                                                            <cc1:CalendarExtender OnClientShown="showCalendar" ID="CalendarExtender2" Format="dd-MMM-yyyy" runat="server"
                                                                Enabled="True" TargetControlID="txtToDate">
                                                            </cc1:CalendarExtender>
                                                        </asp:Panel>
                                                        <br />
                                                    </div>
                                                    <div class="col-md-2">
                                                        <asp:Label ID="lblType" runat="server" Text="<%$ Resources:Attendance,Type %>"></asp:Label>
                                                        <asp:DropDownList ID="ddlVerified" runat="server" CssClass="form-control">
                                                            <asp:ListItem Text="<%$ Resources:Attendance,All %>" Value="0"></asp:ListItem>
                                                            <asp:ListItem Text="<%$ Resources:Attendance,For Mismatch %>" Value="By Manual" Selected="True"></asp:ListItem>
                                                            <asp:ListItem Text="<%$ Resources:Attendance,For Device %>" Value="By Device"></asp:ListItem>
                                                            <asp:ListItem Text="<%$ Resources:Attendance,For Tour %>" Value="By Tour"></asp:ListItem>
                                                            <asp:ListItem Text="<%$ Resources:Attendance,For Half Day %>" Value="By Half Day"></asp:ListItem>
                                                            <asp:ListItem Text="<%$ Resources:Attendance,For Partial Leave %>" Value="By Partial Leave"></asp:ListItem>
                                                        </asp:DropDownList>
                                                        <br />
                                                    </div>
                                                    <div class="col-md-3">
                                                        <br />
                                                        <asp:Button ID="btnSearch" Visible="false" runat="server" OnClick="btnSearch_Click"
                                                            Text="<%$ Resources:Attendance,Search %>" CssClass="btn btn-primary" />

                                                        <asp:Button ID="btnResetLog" runat="server" OnClick="btnResetLog_Click" Text="<%$ Resources:Attendance,Reset %>"
                                                            CssClass="btn btn-primary" Visible="true" />
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
					<asp:Label ID="lblTotalRecord1" runat="server" Font-Bold="true" Text="<%$ Resources:Attendance,Total Records: 0 %>"></asp:Label>

                                                <div class="box-tools pull-right">
                                                    <button type="button" class="btn btn-box-tool" data-widget="collapse">
                                                        <i id="I1" runat="server" class="fa fa-plus"></i>
                                                    </button>
                                                </div>
                                            </div>
                                            <div class="box-body">

                                                <div class="col-lg-3">
                                                    <asp:DropDownList ID="ddlField1" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlField1_SelectedIndexChanged" CssClass="form-control">
                                                        <asp:ListItem Text="<%$ Resources:Attendance,Employee Code %>" Value="Emp_Code"></asp:ListItem>
                                                        <asp:ListItem Text="<%$ Resources:Attendance,Employee Name %>" Value="Emp_Name" Selected="True"></asp:ListItem>
                                                        <asp:ListItem Text="<%$ Resources:Attendance,Employee Name Local %>" Value="Emp_Name_L"></asp:ListItem>
                                                        <asp:ListItem Text="<%$ Resources:Attendance,Department %>" Value="Dep_Name"></asp:ListItem>
                                                        <asp:ListItem Text="<%$ Resources:Attendance,Date%>" Value="Event_Date"></asp:ListItem>
                                                        <asp:ListItem Text="<%$ Resources:Attendance,Type%>" Value="Type"></asp:ListItem>
                                                        <asp:ListItem Text="<%$ Resources:Attendance,Function Key %>" Value="Func_Code"></asp:ListItem>
                                                        <asp:ListItem Text="<%$ Resources:Attendance,Verified By%>" Value="Verified_Type"></asp:ListItem>
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
                                                <div class="col-lg-4">
                                                    <asp:Panel ID="Panel" runat="server" DefaultButton="ImgBtnBind1">
                                                        <asp:TextBox ID="txtVal1" runat="server" CssClass="form-control" placeholder="Search from Content" />
                                                        <asp:TextBox ID="Txt_Val_Date" Visible="false" CssClass="form-control" runat="server" placeholder="Search from Date"></asp:TextBox>
                                                        <cc1:CalendarExtender OnClientShown="showCalendar" ID="CalendarExtender5" Format="dd-MMM-yyyy" runat="server"
                                                            Enabled="True" TargetControlID="Txt_Val_Date">
                                                        </cc1:CalendarExtender>
                                                        <asp:DropDownList ID="Ddl_Val_Type" Visible="false" runat="server" CssClass="form-control"></asp:DropDownList>
                                                        <asp:DropDownList ID="Ddl_Val_Function_Key" Visible="false" runat="server" CssClass="form-control"></asp:DropDownList>
                                                        <asp:DropDownList ID="Ddl_Val_Verified_By" Visible="false" runat="server" CssClass="form-control"></asp:DropDownList>
                                                    </asp:Panel>
                                                </div>
                                                <div class="col-lg-3">
                                                    <asp:LinkButton ID="ImgBtnBind1" runat="server" CausesValidation="False" OnClick="btnarybind_Click" ToolTip="<%$ Resources:Attendance,Search %>"><span class="fa fa-search"  style="font-size:25px;"></span></asp:LinkButton>&nbsp;&nbsp;&nbsp;
                                                    <asp:LinkButton ID="ImgBtnRefreshList" runat="server" CausesValidation="False" OnClick="btnaryRefresh_Click" ToolTip="<%$ Resources:Attendance,Refresh %>"><span class="fa fa-repeat"  style="font-size:25px;"></span></asp:LinkButton>&nbsp;&nbsp;&nbsp;
                                                    <asp:LinkButton ID="ImageButton3" runat="server" OnClick="ImgbtnSelectAll_Click1" ToolTip="<%$ Resources:Attendance, Select All %>"><span class="fas fa-th"  style="font-size:25px;"></span></asp:LinkButton>&nbsp;&nbsp;&nbsp;
                                                    <asp:LinkButton ID="ImageButton1" runat="server" OnClick="ImgbtnDeleteAll_Click1" ToolTip="<%$ Resources:Attendance, Delete %>" Visible="false"><span class="fas fa-remove"  style="font-size:25px;"></span></asp:LinkButton>
                                                </div>

                                            </div>
                                        </div>
                                    </div>
                                </div>


                                <div class="box box-warning box-solid" <%= gvEmpLog.Rows.Count>0?"style='display:block'":"style='display:none'"%>>
                                    <div class="box-body">
                                        <div class="row">
                                            <div class="col-md-12">
                                                <asp:HiddenField ID="HDFSort" runat="server" />
                                                <div class="flow">
                                                    <asp:Label ID="lblSelectRecord1" runat="server" Visible="false"></asp:Label>
                                                    <asp:GridView CssClass="table-striped table-bordered table table-hover" ID="gvEmpLog" runat="server" AllowPaging="True" AutoGenerateColumns="False" OnSorting="gvEmpLog_OnSorting" AllowSorting="true"
                                                        OnPageIndexChanging="gvEmpLog_PageIndexChanging" Width="100%"
                                                        DataKeyNames="Trans_Id,Emp_Id" PageSize="<%# PageControlCommon.GetPageSize() %>"
                                                        Visible="true">
                                                        <Columns>
                                                            <asp:TemplateField>
                                                                <ItemTemplate>
                                                                    <asp:CheckBox ID="chkgvSelect" runat="server" AutoPostBack="true" OnCheckedChanged="chkgvSelect_CheckedChanged1" />
                                                                </ItemTemplate>
                                                                <HeaderTemplate>
                                                                    <asp:CheckBox ID="chkgvSelectAll" runat="server" OnCheckedChanged="chkgvSelectAll_CheckedChanged1"
                                                                        AutoPostBack="true" />
                                                                </HeaderTemplate>
                                                                <ItemStyle HorizontalAlign="Center" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,Employee Code %>" SortExpression="Emp_Code">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblEmpCode" runat="server" Text='<%# Eval("Emp_Code") %>'></asp:Label>
                                                                    <asp:Label ID="lblTransId" Visible="false" runat="server" Text='<%# Eval("Trans_Id") %>'></asp:Label>
                                                                    <asp:Label ID="lblEmpid" Visible="false" runat="server" Text='<%# Eval("Emp_Id") %>'></asp:Label>
                                                                </ItemTemplate>
                                                                <ItemStyle />
                                                            </asp:TemplateField>
                                                            <asp:BoundField DataField="Emp_Name" HeaderText="<%$ Resources:Attendance,Employee Name %>"
                                                                SortExpression="Emp_Name" ItemStyle-Width="20%" />
                                                            <asp:BoundField DataField="Emp_Name_L" HeaderText="<%$ Resources:Attendance,Employee Name Local%>"
                                                                SortExpression="Emp_Name_L" ItemStyle-Width="20%" />

                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,Department %>" SortExpression="Dep_Name">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblProductCode" runat="server" Text='<%# Eval("Dep_Name") %>'></asp:Label>
                                                                </ItemTemplate>
                                                                <ItemStyle />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,Date %>" SortExpression="Event_Date">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblItemType" runat="server" Text='<%# GetDate(Eval("Event_Date")) %>'></asp:Label>
                                                                </ItemTemplate>
                                                                <ItemStyle />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,Time %>">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblItemType1" runat="server" Text='<%# Convert.ToDateTime(Eval("Event_Time")).ToString("HH:mm") %>'></asp:Label>
                                                                </ItemTemplate>
                                                                <ItemStyle />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,Type %>" SortExpression="Type">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblItemType2" runat="server" Text='<%# Eval("Type") %>'></asp:Label>
                                                                </ItemTemplate>
                                                                <ItemStyle />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,Function Key %>" SortExpression="Func_Code">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblItemType3" runat="server" Text='<%# Eval("Func_Code") %>'></asp:Label>
                                                                </ItemTemplate>
                                                                <ItemStyle />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,Verified By %>" SortExpression="Verified_Type">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblVerifiedby1" runat="server" Text='<%# Eval("Verified_Type") %>'></asp:Label>
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
                                <div class="row">
                                    <div class="col-md-12">
                                        <div class="box box-primary">
                                            <div class="box-body">
                                                <div class="form-group">
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
                                                                <asp:LinkButton ID="btnAllRefresh" runat="server" CausesValidation="False" OnClick="btnAllRefresh_Click" ToolTip="<%$ Resources:Attendance,Refresh %>"><span class="fa fa-repeat"  style="font-size:25px;margin-left:5px"></span></asp:LinkButton>
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
                                            <div id="Div2" runat="server" class="box box-info collapsed-box">
                                                <div class="box-header with-border">
                                                    <h3 class="box-title">
                                                        <asp:Label ID="Label9" runat="server" Text="<%$ Resources:Attendance,Advance Search%>"></asp:Label></h3>
                                                    &nbsp;&nbsp;|&nbsp;&nbsp;
					 <asp:Label ID="lblTotalRecord" Font-Bold="true" runat="server" Text="<%$ Resources:Attendance,Total Records: 0 %>"></asp:Label>

                                                    <div class="box-tools pull-right">
                                                        <button type="button" class="btn btn-box-tool" data-widget="collapse">
                                                            <i id="I2" runat="server" class="fa fa-plus"></i>
                                                        </button>
                                                    </div>
                                                </div>
                                                <div class="box-body">
                                                    <div class="col-lg-3">
                                                        <asp:DropDownList ID="ddlField" runat="server" CssClass="form-control">
                                                            <asp:ListItem Text="<%$ Resources:Attendance,Employee Code %>" Value="Emp_Code"></asp:ListItem>
                                                            <asp:ListItem Text="<%$ Resources:Attendance,Employee Name %>" Value="Emp_Name" Selected="True"></asp:ListItem>
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
                                                        <asp:Panel ID="Panel1" runat="server" DefaultButton="ImageButton8">
                                                            <asp:TextBox placeholder="Search from Content" ID="txtValue" runat="server" CssClass="form-control"></asp:TextBox>
                                                        </asp:Panel>
                                                    </div>
                                                    <div class="col-lg-3">
                                                        <asp:LinkButton ID="ImageButton8" runat="server" CausesValidation="False" ToolTip="<%$ Resources:Attendance,Search %>" OnClick="btnarybind_Click1"><span class="fa fa-search"  style="font-size:25px;"></span></asp:LinkButton>&nbsp;&nbsp;&nbsp;
                                                        <asp:LinkButton ID="ImageButton9" runat="server" CausesValidation="False" ToolTip="<%$ Resources:Attendance,Refresh %>" OnClick="btnaryRefresh_Click1"><span class="fa fa-repeat"  style="font-size:25px;"></span></asp:LinkButton>&nbsp;&nbsp;&nbsp;
                                                        <asp:LinkButton ID="ImageButton10" runat="server" OnClick="ImgbtnSelectAll_Clickary" ToolTip="<%$ Resources:Attendance, Select All %>"><span class="fas fa-th"  style="font-size:25px;"></span></asp:LinkButton>
                                                    </div>

                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="box box-warning box-solid">
                                        <div class="box-body">
                                            <asp:CustomValidator ID="CustomValidator1" ValidationGroup="Grid_val" runat="server" ErrorMessage="Please select at least one record."
                                                ClientValidationFunction="Grid_Validate" ForeColor="Red"></asp:CustomValidator>
                                            <div class="row">
                                                <div class="col-md-12">
                                                    <div class="flow">
                                                        <asp:Label ID="lblSelectRecord" runat="server" Visible="false"></asp:Label>
                                                        <asp:GridView CssClass="table-striped table-bordered table table-hover" ID="gvEmployee" runat="server" AllowPaging="True" AutoGenerateColumns="False"
                                                            OnPageIndexChanging="gvEmp_PageIndexChanging" Width="100%" DataKeyNames="Emp_Id"
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
                                                                <asp:TemplateField HeaderText="<%$ Resources:Attendance,Department Name %>">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="blblDeptname" runat="server" Text='<%# Eval("Department") %>'></asp:Label>
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
                                                            <asp:CustomValidator ID="CustomValidator2" ValidationGroup="Grid_val" runat="server" ErrorMessage="Please select at least one record."
                                                                ClientValidationFunction="Grid_Validate" ForeColor="Red"></asp:CustomValidator>

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
                                                                            SortExpression="Emp_Name" ItemStyle-Width="20%" />
                                                                        <asp:BoundField DataField="Emp_Name_L" HeaderText="<%$ Resources:Attendance,Employee Name Local%>"
                                                                            SortExpression="Emp_Name_L" ItemStyle-Width="20%" />
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
                                                    <div class="col-md-6">
                                                        <asp:Label ID="Label27" runat="server" Text="<%$ Resources:Attendance,Date %>"></asp:Label>
                                                        <asp:TextBox ID="txtDate" runat="server" CssClass="form-control"></asp:TextBox>

                                                        <cc1:CalendarExtender OnClientShown="showCalendar" ID="txtFrom_CalendarExtender" Format="dd-MMM-yyyy" runat="server"
                                                            Enabled="True" TargetControlID="txtDate">
                                                        </cc1:CalendarExtender>
                                                        <br />
                                                    </div>
                                                    <div class="col-md-6">
                                                        <asp:Label ID="Label28" runat="server" Text="<%$ Resources:Attendance,Time %>"></asp:Label>
                                                        <asp:TextBox ID="txtOnDuty" CssClass="form-control" runat="server" Font-Names="Verdana" Font-Size="14px"></asp:TextBox>

                                                        <cc1:MaskedEditExtender ID="txtOnDuty_MaskedEditExtender" runat="server" CultureAMPMPlaceholder=""
                                                            CultureCurrencySymbolPlaceholder="" CultureDateFormat="" CultureDatePlaceholder=""
                                                            CultureDecimalPlaceholder="" CultureThousandsPlaceholder="" CultureTimePlaceholder=""
                                                            Enabled="True" ErrorTooltipEnabled="True" Mask="99:99" MaskType="Time" TargetControlID="txtOnDuty"
                                                            UserTimeFormat="TwentyFourHour"
                                                            MessageValidatorTip="true" InputDirection="LeftToRight">
                                                        </cc1:MaskedEditExtender>
                                                        <cc1:MaskedEditValidator ID="MaskedEditValidator5" runat="server" ControlExtender="txtOnDuty_MaskedEditExtender"
                                                            ControlToValidate="txtOnDuty" Display="Dynamic" InvalidValueMessage="Please enter a valid time."
                                                            SetFocusOnError="True" />
                                                        <br />
                                                    </div>
                                                    <div class="col-md-6">
                                                        <asp:Label ID="Label4" runat="server" Text="<%$ Resources:Attendance,Function Key %>"></asp:Label>
                                                        <asp:DropDownList ID="ddlFunction" CssClass="form-control" runat="server">
                                                        </asp:DropDownList>
                                                        <br />
                                                    </div>
                                                    <div class="col-md-6">
                                                        <asp:Label ID="Label7" runat="server" Text="<%$ Resources:Attendance,Type %>"></asp:Label>
                                                        <asp:DropDownList ID="ddlType" CssClass="form-control" runat="server">
                                                            <asp:ListItem Value="In" Text="In"></asp:ListItem>
                                                            <asp:ListItem Value="Out" Text="Out"></asp:ListItem>
                                                        </asp:DropDownList>
                                                        <br />
                                                    </div>
                                                    <div class="col-md-12">
                                                        <asp:Label ID="Label8" runat="server" Style="padding-right: 50px;" Text="<%$ Resources:Attendance,Attendance Type %>"></asp:Label>
                                                        <asp:RadioButton ID="rbtnByManual" Style="padding-right: 10px; padding-left: 10px;" Text="<%$ Resources:Attendance,For Mismatch %>"
                                                            runat="server" GroupName="AttType" />
                                                        <asp:RadioButton ID="rbtnByTour" Style="padding-right: 10px; padding-left: 10px;" Text="<%$ Resources:Attendance,For Tour %>"
                                                            runat="server" GroupName="AttType" />
                                                        <asp:RadioButton ID="rbtnByPartialLeave" Style="padding-right: 10px; padding-left: 10px;" Text="<%$ Resources:Attendance,For Partial Leave %>"
                                                            runat="server" GroupName="AttType" />
                                                        <asp:RadioButton ID="rbtnByHalfDay" Style="padding-right: 10px; padding-left: 10px;" Text="<%$ Resources:Attendance,For Half Day %>"
                                                            runat="server" GroupName="AttType" />
                                                        <br />
                                                    </div>
                                                    <div class="col-md-12" style="text-align: center">
                                                        <asp:Button ID="btnSave" ValidationGroup="Grid_val" runat="server" OnClick="btnSave_Click" Text="<%$ Resources:Attendance,Save %>"
                                                            CssClass="btn btn-success" Visible="false" />
                                                        &nbsp; &nbsp;
                                                                <asp:Button ID="btnReset" runat="server" OnClick="btnReset_Click" Text="<%$ Resources:Attendance,Reset %>"
                                                                    CssClass="btn btn-primary" Visible="true" />
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
                                        <div class="box box-primary">
                                            <div class="box-body">
                                                <div class="form-group">
                                                    <div class="col-md-3">
                                                        <asp:Label ID="Label3" runat="server" Text="<%$ Resources:Attendance,From Date %>"></asp:Label>
                                                        <asp:TextBox ID="txtFromBin" CssClass="form-control" runat="server"></asp:TextBox>
                                                        <cc1:CalendarExtender OnClientShown="showCalendar" ID="CalendarExtender3" Format="dd-MMM-yyyy" runat="server"
                                                            Enabled="True" TargetControlID="txtFromBin">
                                                        </cc1:CalendarExtender>
                                                    </div>
                                                    <div class="col-md-3">
                                                        <asp:Label ID="lblToDateBin" runat="server" Text="<%$ Resources:Attendance,To Date %>"></asp:Label>
                                                        <asp:TextBox ID="txtToBin" CssClass="form-control" runat="server"></asp:TextBox>
                                                        <cc1:CalendarExtender OnClientShown="showCalendar" ID="CalendarExtender4" Format="dd-MMM-yyyy" runat="server"
                                                            Enabled="True" TargetControlID="txtToBin">
                                                        </cc1:CalendarExtender>

                                                    </div>
                                                    <div class="col-md-3">
                                                        <asp:Label ID="lblTypeBin" runat="server" Text="<%$ Resources:Attendance,Type %>"></asp:Label>
                                                        <asp:DropDownList ID="ddlVerifiedBin" runat="server" CssClass="form-control">
                                                            <asp:ListItem Text="<%$ Resources:Attendance,All %>" Value="0"></asp:ListItem>
                                                            <asp:ListItem Text="<%$ Resources:Attendance,For Mismatch %>" Value="By Manual" Selected="True"></asp:ListItem>
                                                            <asp:ListItem Text="<%$ Resources:Attendance,For Device %>" Value="By Device"></asp:ListItem>
                                                            <asp:ListItem Text="<%$ Resources:Attendance,For Tour %>" Value="By Tour"></asp:ListItem>
                                                            <asp:ListItem Text="<%$ Resources:Attendance,For Half Day %>" Value="By Half Day"></asp:ListItem>
                                                            <asp:ListItem Text="<%$ Resources:Attendance,For Partial Leave %>" Value="By Partial Leave"></asp:ListItem>
                                                        </asp:DropDownList>
                                                    </div>
                                                    <div class="col-md-2">
                                                        <br />
                                                        <asp:LinkButton ID="btnSearchBin" runat="server" OnClick="btnSearchBin_Click" ToolTip="<%$ Resources:Attendance,Search %>"><span class="fa fa-search"  style="font-size:25px;"></span></asp:LinkButton>&nbsp;&nbsp;&nbsp;
                                                        <asp:LinkButton ID="btnResetLogBin" runat="server" OnClick="btnResetLogBin_Click" ToolTip="<%$ Resources:Attendance,Reset %>" Visible="true"><span class="fa fa-repeat"  style="font-size:25px;"></span></asp:LinkButton>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>

                                <div class="row">
                                    <div class="col-md-12">
                                        <div id="Div3" runat="server" class="box box-info collapsed-box">
                                            <div class="box-header with-border">
                                                <h3 class="box-title">
                                                    <asp:Label ID="Label11" runat="server" Text="<%$ Resources:Attendance,Advance Search%>"></asp:Label></h3>
                                                &nbsp;&nbsp;|&nbsp;&nbsp;
					<asp:Label ID="lblbinTotalRecords" runat="server" Font-Bold="true" Text="<%$ Resources:Attendance,Total Records: 0 %>"></asp:Label>

                                                <div class="box-tools pull-right">
                                                    <button type="button" class="btn btn-box-tool" data-widget="collapse">
                                                        <i id="I3" runat="server" class="fa fa-plus"></i>
                                                    </button>
                                                </div>
                                            </div>
                                            <div class="box-body">

                                                <div class="col-lg-3">
                                                    <asp:DropDownList ID="ddlbinFieldName" AutoPostBack="true" OnSelectedIndexChanged="ddlbinFieldName_SelectedIndexChanged" runat="server" CssClass="form-control">
                                                        <asp:ListItem Text="<%$ Resources:Attendance,Employee Code %>" Value="Emp_Code"></asp:ListItem>
                                                        <asp:ListItem Text="<%$ Resources:Attendance,Employee Name %>" Value="Emp_Name" Selected="True"></asp:ListItem>
                                                        <asp:ListItem Text="<%$ Resources:Attendance,Employee Name Local %>" Value="Emp_Name_L"></asp:ListItem>
                                                        <asp:ListItem Text="<%$ Resources:Attendance,Department %>" Value="Dep_Name"></asp:ListItem>
                                                        <asp:ListItem Text="<%$ Resources:Attendance,Date%>" Value="Event_Date"></asp:ListItem>
                                                        <asp:ListItem Text="<%$ Resources:Attendance,Type%>" Value="Type"></asp:ListItem>
                                                        <asp:ListItem Text="<%$ Resources:Attendance,Function Key %>" Value="Func_Code"></asp:ListItem>
                                                        <asp:ListItem Text="<%$ Resources:Attendance,Verified By%>" Value="Verified_Type"></asp:ListItem>
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
                                                <div class="col-lg-4">
                                                    <asp:Panel ID="Panel2" runat="server" DefaultButton="btnbinbind">
                                                        <asp:TextBox ID="txtbinValue" runat="server" CssClass="form-control" placeholder="Search from Content"></asp:TextBox>
                                                        <asp:TextBox ID="Txt_Bin_Val_Date" Visible="false" CssClass="form-control" runat="server" placeholder="Search from Date"></asp:TextBox>
                                                        <cc1:CalendarExtender OnClientShown="showCalendar" ID="CalendarExtender6" Format="dd-MMM-yyyy" runat="server"
                                                            Enabled="True" TargetControlID="Txt_Bin_Val_Date">
                                                        </cc1:CalendarExtender>
                                                        <asp:DropDownList ID="Ddl_Bin_Val_Type" Visible="false" runat="server" CssClass="form-control"></asp:DropDownList>
                                                        <asp:DropDownList ID="Ddl_Bin_Val_Function_Key" Visible="false" runat="server" CssClass="form-control"></asp:DropDownList>
                                                        <asp:DropDownList ID="Ddl_Bin_Val_Verified_By" Visible="false" runat="server" CssClass="form-control"></asp:DropDownList>
                                                    </asp:Panel>
                                                </div>
                                                <div class="col-lg-3">
                                                    <asp:LinkButton ID="btnbinbind" runat="server" CausesValidation="False" OnClick="btnbinbind_Click" ToolTip="<%$ Resources:Attendance,Search %>"><span class="fa fa-search"  style="font-size:25px;"></span></asp:LinkButton>&nbsp;&nbsp;&nbsp;
                                                    <asp:LinkButton ID="btnbinRefresh" runat="server" CausesValidation="False" OnClick="btnbinRefresh_Click" ToolTip="<%$ Resources:Attendance,Refresh %>"><span class="fa fa-repeat"  style="font-size:25px;"></span></asp:LinkButton>&nbsp;&nbsp;&nbsp;
                                                    <asp:LinkButton ID="imgBtnRestore" CausesValidation="False" Visible="false" runat="server" OnClick="imgBtnRestore_Click" ToolTip="<%$ Resources:Attendance, Active %>"><span class="far fa-lightbulb"  style="font-size:25px;"></span></asp:LinkButton>&nbsp;&nbsp;&nbsp;
                                                    <asp:LinkButton ID="ImgbtnSelectAll" Visible="false" runat="server" OnClick="ImgbtnSelectAll_Click" ToolTip="<%$ Resources:Attendance, Select All %>"><span class="fas fa-th"  style="font-size:25px;"></span></asp:LinkButton>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                                <div class="box box-warning box-solid" <%= gvLogBin.Rows.Count>0?"style='display:block'":"style='display:none'"%>>
                                    <div class="box-body">
                                        <div class="row">
                                            <div class="col-md-12">
                                                <div class="flow">
                                                    <asp:Label ID="lblSelectRecordBin" runat="server" Visible="false"></asp:Label>
                                                    <asp:GridView CssClass="table-striped table-bordered table table-hover" ID="gvLogBin" PageSize="<%# PageControlCommon.GetPageSize() %>" runat="server"
                                                        AutoGenerateColumns="False" DataKeyNames="Trans_Id" Width="100%" AllowPaging="True" OnSorting="gvLogBin_Sorting"
                                                        OnPageIndexChanging="gvLogBin_PageIndexChanging" AllowSorting="true">
                                                        <Columns>
                                                            <asp:TemplateField>
                                                                <ItemTemplate>
                                                                    <asp:CheckBox ID="chkgvSelect" runat="server" />
                                                                </ItemTemplate>
                                                                <HeaderTemplate>
                                                                    <asp:CheckBox ID="chkgvSelectAll" runat="server" OnCheckedChanged="chkgvSelectAllBin_CheckedChanged"
                                                                        AutoPostBack="true" />
                                                                </HeaderTemplate>
                                                                <ItemStyle HorizontalAlign="Center" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,Employee Code %>" SortExpression="Emp_Code">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblEmpCode" runat="server" Text='<%# Eval("Emp_Code") %>'></asp:Label>
                                                                    <asp:Label ID="lblTransId" Visible="false" runat="server" Text='<%# Eval("Trans_Id") %>'></asp:Label>
                                                                </ItemTemplate>
                                                                <ItemStyle />
                                                            </asp:TemplateField>
                                                            <asp:BoundField DataField="Emp_Name" HeaderText="<%$ Resources:Attendance,Employee Name %>"
                                                                SortExpression="Emp_Name" ItemStyle-Width="20%" />
                                                            <asp:BoundField DataField="Emp_Name_L" HeaderText="<%$ Resources:Attendance,Employee Name Local%>"
                                                                SortExpression="Emp_Name_L" ItemStyle-Width="20%" />
                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,Department %>" SortExpression="Dep_Name">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblProductCode" runat="server" Text='<%# Eval("Dep_Name") %>'></asp:Label>
                                                                </ItemTemplate>
                                                                <ItemStyle />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,Date %>" SortExpression="Event_Date">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblItemType" runat="server" Text='<%# GetDate(Eval("Event_Date")) %>'></asp:Label>
                                                                </ItemTemplate>
                                                                <ItemStyle />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,Time %>">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblItemType1" runat="server" Text='<%# Convert.ToDateTime(Eval("Event_Time")).ToString("HH:mm") %>'></asp:Label>
                                                                </ItemTemplate>
                                                                <ItemStyle />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,Type %>" SortExpression="Type">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblItemType2" runat="server" Text='<%# Eval("Type") %>'></asp:Label>
                                                                </ItemTemplate>
                                                                <ItemStyle />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,Function Key %>" SortExpression="Func_Code">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblItemType3" runat="server" Text='<%# Eval("Func_Code") %>'></asp:Label>
                                                                </ItemTemplate>
                                                                <ItemStyle />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,Verified By %>" SortExpression="Verified_Type">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblVerifiedby1" runat="server" Text='<%# Eval("Verified_Type") %>'></asp:Label>
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
                                        <div class="box box-info">
                                            <div class="box-header with-border">
                                                <h3 class="box-title">
                                                    <asp:Label ID="Label17" runat="server" Text="<%$ Resources:Attendance,Search%>"></asp:Label></h3>
                                                <div class="box-tools pull-right">
                                                    <button type="button" class="btn btn-box-tool" data-widget="collapse">
                                                        <i class="fa fa-minus"></i>
                                                    </button>
                                                </div>
                                            </div>
                                            <div class="box-body">
                                                <div class="form-group">
                                                    <div class="col-md-12">
                                                        <asp:HyperLink ID="HyperLink2" Visible="true" runat="server" NavigateUrl="~/CompanyResource/Upload_Manual_Log.xlsx" Text="<%$ Resources:Attendance,Download Excel For Upload log%>"></asp:HyperLink>
                                                    </div>
                                                    <div class="col-md-12">
                                                        <div class="col-md-4">
                                                        </div>
                                                        <div class="col-md-4">
                                                            <asp:Label runat="server" Text="<%$ Resources:Attendance,Browse Excel File%>" ID="Label66"></asp:Label>
                                                            <div class="input-group" style="width: 100%;">
                                                                <cc1:AsyncFileUpload ID="fileLoad" OnUploadedComplete="FileUploadComplete" OnClientUploadError="uploadError" OnClientUploadStarted="uploadStarted" OnClientUploadComplete="uploadComplete"
                                                                    runat="server" CssClass="form-control" CompleteBackColor="White" UploaderStyle="Traditional" UploadingBackColor="#CCFFFF" ThrobberID="imgLoader" Width="100%" />
                                                                <div class="input-group-btn" style="border: solid 1px #d2d6de; width: 35px;">
                                                                    <asp:LinkButton ID="Img_Right" runat="server" Width="30px" Height="30px" Style="display: none"><i class="fa fa-check" style="font-size:30px;color:#22cb33"></i></asp:LinkButton>
                                                                    <asp:LinkButton ID="Img_Wrong" runat="server" Width="30px" Height="30px" Style="display: none"><i class="fa fa-remove" style="font-size:30px"></i></asp:LinkButton>
                                                                    <asp:Image ID="imgLoader" runat="server" ImageUrl="../Images/loader.gif" />
                                                                </div>
                                                            </div>
                                                            <br />
                                                            <div style="text-align: center;">
                                                                <asp:Button ID="btnConnect" CssClass="btn btn-primary" runat="server" CausesValidation="False"
                                                                    OnClick="btnConnect_Click" Visible="true" Text="<%$ Resources:Attendance,Connect To DataBase %>" />
                                                            </div>
                                                            <br />
                                                            <asp:Label ID="Label67" runat="server" Text="<%$ Resources:Attendance,Select Sheet%>"></asp:Label>
                                                            <asp:DropDownList ID="ddlTables" CssClass="form-control" runat="server">
                                                            </asp:DropDownList>
                                                            <br />
                                                            <div style="text-align: center;">
                                                                <asp:Button ID="btnviewcolumns" CssClass="btn btn-primary" runat="server" CausesValidation="False"
                                                                    OnClick="btnviewcolumns_Click" Visible="true" Text="<%$ Resources:Attendance,Map Column %>" />
                                                            </div>
                                                            <br />
                                                            <div id="div_Grid" visible="false" runat="server" style="max-height: 500px; overflow: auto;">
                                                                <asp:GridView CssClass="table-striped table-bordered table table-hover" ID="gvFieldMapping" runat="server" AutoGenerateColumns="False" Width="100%"
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
                                                                                <asp:DropDownList ID="ddlExcelCol" CssClass="form-control" runat="server">
                                                                                </asp:DropDownList>
                                                                            </ItemTemplate>
                                                                        </asp:TemplateField>
                                                                    </Columns>

                                                                    
                                                                    <PagerStyle CssClass="pagination-ys" />

                                                                </asp:GridView>
                                                                <br />
                                                                <asp:Label ID="lblMessage" runat="server" ForeColor="Red"></asp:Label>
                                                                <div style="text-align: center;">
                                                                    <asp:Button ID="btnUploadTemp" CssClass="btn btn-primary" runat="server" OnClick="btnUpload_Click2"
                                                                        Text="<%$ Resources:Attendance,Show Data %>" />
                                                                    <asp:Button ID="btncancel1" CssClass="btn btn-primary" runat="server" OnClick="btncancel_Click"
                                                                        Text="<%$ Resources:Attendance,Reset %>" />
                                                                </div>
                                                            </div>
                                                        </div>
                                                        <div class="col-md-4">
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
                                                    <div class="col-md-12">
                                                        <div style="overflow: auto; max-height: 500px;">
                                                            <asp:GridView CssClass="table-striped table-bordered table table-hover" ID="gvSelected" runat="server" Width="100%">

                                                                
                                                                <PagerStyle CssClass="pagination-ys" />

                                                            </asp:GridView>
                                                        </div>
                                                        <br />
                                                    </div>
                                                    <div class="col-md-12" style="text-align: center">
                                                        <asp:Button ID="Button21" runat="server" CssClass="btn btn-primary" OnClick="btnUpload_Click1"
                                                            Text="<%$ Resources:Attendance,Upload Data %>" />
                                                        <cc1:ConfirmButtonExtender ID="ConfirmButtonExtender1" runat="server" TargetControlID="Button21"
                                                            ConfirmText="Are you sure to Save Records in Database.">
                                                        </cc1:ConfirmButtonExtender>

                                                        <asp:Button ID="btnBackToMapData" CssClass="btn btn-primary" runat="server" Text="<%$ Resources:Attendance,Back To FileUpload %>"
                                                            OnClick="btnBackToMapData_Click" />

                                                        <asp:DropDownList ID="ddlFiltercol" Visible="false" CssClass="form-control"
                                                            runat="server">
                                                        </asp:DropDownList>

                                                        <asp:TextBox ID="txtfiltercol" Visible="false" CssClass="form-control" runat="server"></asp:TextBox>

                                                        <asp:Button ID="btnFilter" CssClass="btn btn-primary" Visible="false" runat="server"
                                                            OnClick="btnFilter_Click" Text="<%$ Resources:Attendance,Filter %>" />

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
        function Li_Tab_New() {
            document.getElementById('<%= Btn_New.ClientID %>').click();
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
    </script>
</asp:Content>
