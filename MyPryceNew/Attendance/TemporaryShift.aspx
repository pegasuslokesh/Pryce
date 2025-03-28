<%@ Page Title="" Language="C#" MasterPageFile="~/ERPMaster.master" AutoEventWireup="true" CodeFile="TemporaryShift.aspx.cs" Inherits="Attendance_TemporaryShift" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="Head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="HeaderContent" runat="Server">
    <h1>
        <i class="fas fa-calendar-week"></i>&nbsp;&nbsp;
        <asp:Label ID="lblHeader" runat="server" Text="<%$ Resources:Attendance,Temporary Shift%>"></asp:Label>
    </h1>
    <ol class="breadcrumb">
        <li><a href="#"><i class="fa fa-dashboard"></i>
            <asp:Label ID="Label6" runat="server" Text="<%$ Resources:Attendance,Attendance Module%>"></asp:Label></a></li>
        <li><a href="#"><i class="fa fa-dashboard"></i>
            <asp:Label ID="Label10" runat="server" Text="<%$ Resources:Attendance,Attendance Module%>"></asp:Label></a></li>
        <li class="active">
            <asp:Label ID="Label5" runat="server" Text="<%$ Resources:Attendance,Temporary Shift%>"></asp:Label></li>
    </ol>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="Server">
    <asp:UpdatePanel ID="Update_Button" runat="server">
        <ContentTemplate>
            <asp:Button ID="Btn_New" Style="display: none;" runat="server" OnClick="btnNew_Click" Text="New" />
            <asp:Button ID="Btn_Modal_View" Style="display: none;" runat="server" data-toggle="modal" data-target="#Modal_View" Text="Modal View" />
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
                    <li id="Li_New"><a onclick="Li_Tab_New()" href="#New" data-toggle="tab">
                        <asp:UpdatePanel ID="Update_Li" runat="server">
                            <ContentTemplate>
                                <i class="fa fa-file"></i>&nbsp;&nbsp;
                                <asp:Label ID="Lbl_Tab_New" runat="server" Text="<%$ Resources:Attendance,New%>"></asp:Label>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </a></li>
                    <li class="active" id="Li_List"><a href="#List" data-toggle="tab">
                        <i class="fa fa-list"></i>&nbsp;&nbsp;<asp:Label ID="Lbl_List" runat="server" Text="<%$ Resources:Attendance,List %>"></asp:Label></a></li>
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
                                                <div class="col-lg-12">
                                                    <asp:DropDownList runat="server" ID="ddlLoc" OnSelectedIndexChanged="ddlLoc_SelectedIndexChanged" AutoPostBack="true" CssClass="form-control"></asp:DropDownList>
                                                    <br />
                                                    </div>
                                                <div class="col-lg-3">
                                                    <asp:DropDownList ID="ddlFieldName" runat="server" CssClass="form-control">
                                                        <asp:ListItem Text="<%$ Resources:Attendance,Employee Code %>" Value="Emp_Code"></asp:ListItem>
                                                        <asp:ListItem Text="<%$ Resources:Attendance,Employee Name %>" Value="Emp_Name" Selected="True"></asp:ListItem>
                                                        <asp:ListItem Text="<%$ Resources:Attendance,Employee Name Local %>" Value="Emp_Name_L"></asp:ListItem>
                                                    </asp:DropDownList>
                                                </div>
                                                <div class="col-lg-2">
                                                    <asp:DropDownList ID="ddlOption" runat="server" CssClass="form-control">
                                                        <asp:ListItem Text="<%$ Resources:Attendance,--Select-- %>"></asp:ListItem>
                                                        <asp:ListItem Text="<%$ Resources:Attendance,Equal %>"></asp:ListItem>
                                                        <asp:ListItem Text="<%$ Resources:Attendance,Like %>"></asp:ListItem>
                                                        <asp:ListItem Selected="True" Text="<%$ Resources:Attendance,Contains %>"></asp:ListItem>
                                                    </asp:DropDownList>
                                                </div>
                                                <div class="col-lg-5">
                                                    <asp:Panel ID="Panel1" runat="server" DefaultButton="btnbind">
                                                        <asp:TextBox ID="txtValue" placeholder="Search from Content" runat="server" CssClass="form-control" />
                                                    </asp:Panel>
                                                </div>
                                                <div class="col-lg-2">
                                                    <asp:LinkButton ID="btnbind" runat="server" CausesValidation="False" OnClick="btnbind_Click" ToolTip="<%$ Resources:Attendance,Search %>"><span class="fa fa-search"  style="font-size:25px;"></span></asp:LinkButton>&nbsp;&nbsp;&nbsp;
                                                    <asp:LinkButton ID="btnRefresh" runat="server" CausesValidation="False" OnClick="btnRefresh_Click" ToolTip="<%$ Resources:Attendance,Refresh %>"><span class="fa fa-repeat"  style="font-size:25px;"></span></asp:LinkButton>&nbsp;&nbsp;&nbsp;
                                                </div>

                                            </div>
                                        </div>
                                    </div>
                                </div>
                                <div class="box box-warning box-solid" <%= gvEmp.Rows.Count>0?"style='display:block'":"style='display:none'"%>>
                                    <div class="box-body">
                                        <div class="row">
                                            <div class="col-md-12">
                                                <div class="flow">
                                                    <asp:GridView CssClass="table-striped table-bordered table table-hover" ID="gvEmp" runat="server" OnSorting="gvEmp_Sorting" AllowSorting="True" AllowPaging="True" AutoGenerateColumns="False"
                                                        OnPageIndexChanging="gvEmp_PageIndexChanging" Width="100%" DataKeyNames="Emp_Code"
                                                        PageSize="<%# PageControlCommon.GetPageSize() %>" Visible="true">
                                                        <Columns>

                                                            <asp:TemplateField Visible="false">
                                                                <ItemTemplate>
                                                                    <asp:LinkButton ID="btnEdit" runat="server" CausesValidation="False" CommandArgument='<%# Eval("Emp_Id") %>' OnCommand="btnEdit_Command"><span class="fa fa-eye" ></span></asp:LinkButton>
                                                                </ItemTemplate>
                                                                <ItemStyle HorizontalAlign="Center" />
                                                            </asp:TemplateField>

                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,Employee Code %>" SortExpression="Emp_Code">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblEmpCode" runat="server" Text='<%# Eval("Emp_Code") %>'></asp:Label>
                                                                </ItemTemplate>
                                                                <ItemStyle />
                                                            </asp:TemplateField>
                                                            <asp:BoundField DataField="Emp_Name" HeaderText="<%$ Resources:Attendance,Employee Name %>"
                                                                SortExpression="Emp_Name" ItemStyle-Width="20%" />
                                                            <asp:BoundField DataField="Emp_Name_L" HeaderText="<%$ Resources:Attendance,Employee Name Local%>"
                                                                SortExpression="Emp_Name_L" ItemStyle-Width="20%" />
                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,Email Id %>" SortExpression="Email_Id">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblProductCode" runat="server" Text='<%# Eval("Email_Id") %>'></asp:Label>
                                                                </ItemTemplate>
                                                                <ItemStyle />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,Phone No. %>" SortExpression="Phone_No">
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
                                                    <div class="form-group col-md-12" style="text-align: center">
                                                        <asp:HiddenField ID="editid" runat="server" />
                                                        <asp:RadioButton ID="rbtnGroup" OnCheckedChanged="EmpGroup_CheckedChanged"
                                                            runat="server" Text="<%$ Resources:Attendance,Group %>" Font-Bold="true" GroupName="EmpGroup"
                                                            AutoPostBack="true" />
                                                        <asp:RadioButton ID="rbtnEmp" Style="margin-left: 25px;" runat="server" AutoPostBack="true"
                                                            Text="<%$ Resources:Attendance,Employee %>" GroupName="EmpGroup" Font-Bold="true"
                                                            OnCheckedChanged="EmpGroup_CheckedChanged" />
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
                                                                <asp:LinkButton ID="ImgbtnAllRefresh" runat="server" CausesValidation="False" OnClick="btnAllRefresh_Click" ToolTip="<%$ Resources:Attendance,Refresh %>"><span class="fa fa-repeat"  style="font-size:25px;margin-left:5px"></span></asp:LinkButton>
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
                                                            <asp:Label ID="Label2" runat="server" Text="<%$ Resources:Attendance,Advance Search%>"></asp:Label></h3>
                                                        &nbsp;&nbsp;|&nbsp;&nbsp;
					<asp:Label ID="lblSelectRecord" Font-Bold="true" runat="server" Text="<%$ Resources:Attendance,Total Records: 0 %>"></asp:Label>

                                                        <div class="box-tools pull-right">
                                                            <button type="button" class="btn btn-box-tool" data-widget="collapse">
                                                                <i id="I2" runat="server" class="fa fa-plus"></i>
                                                            </button>
                                                        </div>
                                                    </div>
                                                    <div class="box-body">

                                                        <div class="col-lg-3">
                                                            <asp:DropDownList ID="ddlSelectField" runat="server" CssClass="form-control">
                                                                <asp:ListItem Text="<%$ Resources:Attendance,Employee Code%>" Value="Emp_Code"></asp:ListItem>
                                                                <asp:ListItem Text="<%$ Resources:Attendance,Employee Name %>" Value="Emp_Name"></asp:ListItem>
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
                                                        <div class="col-lg-2">
                                                            <asp:TextBox ID="txtval" runat="server" CssClass="form-control"></asp:TextBox>

                                                        </div>
                                                        <div class="col-lg-3">
                                                            <asp:LinkButton ID="btnEmp" runat="server" CausesValidation="False" OnClick="btnEmp_Click"><span class="fa fa-search"  style="font-size:25px;"></span></asp:LinkButton>&nbsp;&nbsp;&nbsp;
                                                            <asp:LinkButton ID="btnrefresh2" runat="server" CausesValidation="False" OnClick="btnRefresh2Report_Click"><span class="fa fa-repeat"  style="font-size:25px;"></span></asp:LinkButton>&nbsp;&nbsp;&nbsp;
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>

                                        <div class="box box-warning box-solid">
                                            <div class="box-body">
                                                <asp:CustomValidator ID="CustomValidator1" ValidationGroup="GvEmpList" runat="server" ErrorMessage="Please select at least one record."
                                                    ClientValidationFunction="Validate_GvEmpList" ForeColor="Red"></asp:CustomValidator>

                                                <div class="row">
                                                    <div class="col-md-12">
                                                        <div style="overflow: auto; max-height: 300px">
                                                            <asp:GridView CssClass="table-striped table-bordered table table-hover" ID="GvEmpList" OnSorting="GvEmpList_Sorting" AllowSorting="True" runat="server" AutoGenerateColumns="False" DataKeyNames="Emp_Code" OnRowDataBound="GvEmpList_RowDataBound"
                                                                Width="100%">
                                                                <Columns>
                                                                    <asp:TemplateField HeaderText="Select">
                                                                        <ItemTemplate>
                                                                            <asp:CheckBox ID="chkBxSelect" onclick="checkItem_All(this,0)" runat="server" />
                                                                            <asp:HiddenField ID="hdnFldId" runat="server" Value='<%# Eval("Emp_Id") %>' />
                                                                        </ItemTemplate>
                                                                        <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="50px" />
                                                                        <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="50px" />
                                                                        <HeaderTemplate>
                                                                            <asp:CheckBox ID="chkBxHeader" onclick="checkAll(this,0);" runat="server" />
                                                                        </HeaderTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="<%$ Resources:Attendance,Employee Code %>" SortExpression="Emp_Code">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblEmpCode" runat="server" Text='<%# Eval("Emp_Code") %>'></asp:Label>
                                                                            <asp:Label ID="lblEmpId" Visible="false" runat="server" Text='<%# Eval("Emp_Id") %>'></asp:Label>
                                                                        </ItemTemplate>
                                                                        <ItemStyle />
                                                                    </asp:TemplateField>
                                                                    <asp:BoundField DataField="Emp_Name" HeaderText="<%$ Resources:Attendance,Employee Name %>"
                                                                        SortExpression="Emp_Name" ItemStyle-Width="40%" />
                                                                    <asp:TemplateField HeaderText="<%$ Resources:Attendance,Email Id %>" SortExpression="Email_Id">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblProductCode" runat="server" Text='<%# Eval("Email_Id") %>'></asp:Label>
                                                                        </ItemTemplate>
                                                                        <ItemStyle />
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="<%$ Resources:Attendance,Designation Name %>" SortExpression="Designation">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblItemType" runat="server" Text='<%# Eval("Designation") %>'></asp:Label>
                                                                        </ItemTemplate>
                                                                        <ItemStyle />
                                                                    </asp:TemplateField>
                                                                </Columns>
                                                                <PagerStyle CssClass="pagination-ys" />

                                                            </asp:GridView>
                                                            <asp:HiddenField ID="hdnFldSelectedValues" runat="server" />
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
                                                                <asp:CustomValidator ID="CustomValidator2" ValidationGroup="GvEmpList" runat="server" ErrorMessage="Please select at least one record."
                                                                    ClientValidationFunction="Validate_GvEmpList" ForeColor="Red"></asp:CustomValidator>

                                                                <asp:ListBox ID="lbxGroup" runat="server" Height="200px" Style="width: 100%" SelectionMode="Multiple"
                                                                    AutoPostBack="true" OnSelectedIndexChanged="lbxGroup_SelectedIndexChanged" CssClass="list"
                                                                    Font-Names="Trebuchet MS" Font-Size="Small" ForeColor="Gray"></asp:ListBox>
                                                                <asp:Label ID="lblEmp" runat="server" Visible="false"></asp:Label>
                                                            </div>
                                                            <div class="col-md-9">
                                                                <div style="overflow: auto">
                                                                    <asp:GridView CssClass="table-striped table-bordered table table-hover" ID="gvEmployee" runat="server" AllowPaging="True" AutoGenerateColumns="False"
                                                                        OnPageIndexChanging="gvEmp1_PageIndexChanging" Width="100%"
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
                                                    <div class="form-group" style="text-align: center">

                                                        <asp:Button ID="btnCancel2" runat="server" CausesValidation="False" CssClass="btn btn-primary"
                                                            ImageUrl="~/Images/buttonCancel.png" OnClick="btnCancel2_Click"
                                                            Text="<%$ Resources:Attendance,Cancel %>" />

                                                        <asp:Button ID="btnNext" runat="server" Visible="false" ValidationGroup="GvEmpList" CssClass="btn btn-primary"
                                                            ImageUrl="~/Images/buttonCancel.png" OnClick="btnNext1_Click"
                                                            Text="<%$ Resources:Attendance,Next %>" />
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                                <div id="Div_After_Next" runat="server" visible="false">
                                    <div id="After_Next" runat="server">

                                        <div class="row">
                                            <div class="col-md-12">
                                                <div id="Div3" runat="server" class="box box-info collapsed-box">
                                                    <div class="box-header with-border">
                                                        <h3 class="box-title">
                                                            <asp:Label ID="Label3" runat="server" Text="Advance Search"></asp:Label></h3>
                                                        &nbsp;&nbsp;|&nbsp;&nbsp;
					<asp:Label ID="Label23" runat="server" Font-Bold="true" Text="<%$ Resources:Attendance,Total Records: 0 %>"></asp:Label>

                                                        <div class="box-tools pull-right">
                                                            <button type="button" class="btn btn-box-tool" data-widget="collapse">
                                                                <i id="I3" runat="server" class="fa fa-plus"></i>
                                                            </button>
                                                        </div>
                                                    </div>
                                                    <div class="box-body">
                                                        <div class="col-lg-2">
                                                            <h5>
                                                                <asp:Label ID="Label1" Font-Bold="true" runat="server"></asp:Label></h5>
                                                        </div>
                                                        <div class="col-lg-3">
                                                            <asp:DropDownList ID="ddlSelectField0" runat="server" CssClass="form-control">
                                                                <asp:ListItem Text="<%$ Resources:Attendance,Employee Code%>" Value="Emp_Code"></asp:ListItem>
                                                                <asp:ListItem Text="<%$ Resources:Attendance,Employee Name %>" Value="Emp_Name"></asp:ListItem>
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
                                                            <asp:LinkButton ID="btnEmp0" runat="server" CausesValidation="False" OnClick="btnEmp0_Click"><span class="fa fa-search"  style="font-size:25px;"></span></asp:LinkButton>&nbsp;&nbsp;&nbsp;
                                                        <asp:LinkButton ID="btnRefresh3" runat="server" CausesValidation="False" OnClick="btnRefresh3Report_Click"><span class="fa fa-repeat"  style="font-size:25px;"></span></asp:LinkButton>&nbsp;&nbsp;&nbsp;
                                                        </div>

                                                    </div>
                                                </div>
                                            </div>
                                        </div>

                                        <div class="box box-warning box-solid">
                                            <div class="box-body">
                                                <div class="row">
                                                    <div class="col-md-12">
                                                        <div class="flow">
                                                            <asp:GridView CssClass="table-striped table-bordered table table-hover" ID="GvEmpListSelected" runat="server" AutoGenerateColumns="False" DataKeyNames="Emp_Code"
                                                                Width="100%">

                                                                
                                                                <PagerStyle CssClass="pagination-ys" />

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

                                                            </asp:GridView>
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
                                                            <asp:Label ID="lblDateFrom" runat="server" Font-Names="Verdana" Font-Size="12px"
                                                                Text="<%$ Resources:Attendance,From Date %>"></asp:Label>
                                                            <a style="color: Red">*</a>
                                                            <asp:RequiredFieldValidator EnableClientScript="true" Style="float: right;" runat="server" ID="RequiredFieldValidator2" ValidationGroup="Time_Table"
                                                                Display="Dynamic" SetFocusOnError="true" ControlToValidate="txtFrom" ErrorMessage="<%$ Resources:Attendance,Enter From Date %>"></asp:RequiredFieldValidator>


                                                            <asp:TextBox ID="txtFrom" runat="server" CssClass="form-control"></asp:TextBox>
                                                            <cc1:CalendarExtender OnClientShown="showCalendar"
                                                                ID="txtFrom_CalendarExtender" Format="dd-MMM-yyyy" runat="server" Enabled="True" TargetControlID="txtFrom">
                                                            </cc1:CalendarExtender>
                                                        </div>
                                                        <div class="col-md-6">
                                                            <asp:Label ID="lblTo" runat="server" Text="<%$ Resources:Attendance,To Date %>" Font-Names="Verdana"
                                                                Font-Size="12px"></asp:Label>
                                                            <a style="color: Red">*</a>
                                                            <asp:RequiredFieldValidator EnableClientScript="true" Style="float: right;" runat="server" ID="RequiredFieldValidator4" ValidationGroup="Time_Table"
                                                                Display="Dynamic" SetFocusOnError="true" ControlToValidate="txtTo" ErrorMessage="<%$ Resources:Attendance,Enter To Date %>"></asp:RequiredFieldValidator>

                                                            <asp:TextBox ID="txtTo" runat="server" CssClass="form-control"></asp:TextBox>
                                                            <cc1:CalendarExtender OnClientShown="showCalendar" ID="txtTo_CalendarExtender" runat="server"
                                                                Enabled="True" TargetControlID="txtTo">
                                                            </cc1:CalendarExtender>
                                                        </div>
                                                        <div class="col-md-12" style="display: none">
                                                            <asp:Label ID="lblShift" runat="server" Text="<%$ Resources:Attendance,Shift %>"
                                                                Font-Names="Verdana" Font-Size="12px"></asp:Label>
                                                            <asp:DropDownList ID="ddlShift" runat="server" CssClass="form-control">
                                                            </asp:DropDownList>
                                                            <asp:Button ID="btnshowpopup" runat="server" Style="display: none" />
                                                        </div>
                                                        <div class="col-md-12" style="text-align: center">
                                                            <br />
                                                            <asp:Button ID="btnIsTemprary" ValidationGroup="Time_Table" runat="server" CssClass="btn btn-primary" OnClick="btnIsTemprary_Click"
                                                                Text="<%$ Resources:Attendance,Select Time Table %>" />

                                                            <asp:Button ID="btncancel1" runat="server" Text="<%$ Resources:Attendance,Cancel %>" OnClick="btnCancel1_Click"
                                                                CssClass="btn btn-primary" Visible="true" /><asp:HiddenField ID="HiddenField2"
                                                                    runat="server" />

                                                            <asp:Button ID="Button1" runat="server" CssClass="btn btn-primary" OnClick="BtnDelete_Click"
                                                                Text="<%$ Resources:Attendance,Delete %>" />
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                                <div id="Div_Last" runat="server" visible="false">
                                    <div class="row">
                                        <div class="col-md-12">
                                            <div class="box box-primary">
                                                <div class="box-body">
                                                    <div class="form-group">
                                                        <div class="col-md-6">
                                                            <div style="text-align: center">

                                                                <asp:CheckBox ID="chkOverWrite" runat="server" Text="Overwrite shift" />

                                                                <asp:Button ID="btnOk0" runat="server" CausesValidation="False" CssClass="btn btn-success"
                                                                    ImageUrl="~/Images/buttonSave.png" OnClick="btnsaveTempShift_Click"
                                                                    Text="<%$ Resources:Attendance,Save %>" Visible="true" />


                                                                <asp:Button ID="btnSelectAll" runat="server" CausesValidation="False" CssClass="btn btn-primary"
                                                                    ImageUrl="~/Images/buttonSave.png" OnClick="btnClearAll_Click"
                                                                    Text="Select All" />

                                                                <asp:Button ID="btnClearAll" runat="server" CausesValidation="False" CssClass="btn btn-primary"
                                                                    ImageUrl="~/Images/buttonSave.png" OnClick="btnClearAll_Click"
                                                                    Text="Clear All" />

                                                                <asp:Button ID="btnCancelPanel" runat="server" CausesValidation="False" CssClass="btn btn-danger"
                                                                    ImageUrl="~/Images/buttonCancel.png" OnClick="btnCancelPanel_Click1"
                                                                    Text="<%$ Resources:Attendance,Cancel %>" />
                                                            </div>
                                                            <br />
                                                            <asp:Label ID="lblSelectShift" runat="server" Text="<%$ Resources:Attendance,selectshiftcat %>"
                                                                Font-Bold="true"></asp:Label>
                                                            <div class="input-group">
                                                                <asp:TextBox ID="txtTimeTable" runat="server" CssClass="form-control" BackColor="#eeeeee"
                                                                    AutoPostBack="true" OnTextChanged="txtTimeTable_textChanged" />
                                                                <cc1:AutoCompleteExtender ID="AutoCompleteExtender5" runat="server" DelimiterCharacters=""
                                                                    Enabled="True" ServiceMethod="GetCompletionListTimeTableName" ServicePath="" CompletionInterval="100"
                                                                    MinimumPrefixLength="1" CompletionSetCount="1" TargetControlID="txtTimeTable" UseContextKey="True"
                                                                    CompletionListCssClass="autoCompleteList" CompletionListItemCssClass="autoCompleteListItem" CompletionListHighlightedItemCssClass="autoCompleteSelectedListItem" OnClientShown="resetPosition">
                                                                </cc1:AutoCompleteExtender>
                                                                <div class="input-group-btn">
                                                                    <asp:Button ID="btnAdd" runat="server" CssClass="btn btn-primary" OnClick="btnAdd_Click" Text="Add" /><br />
                                                                </div>
                                                            </div>
                                                            <br />
                                                            <div id="PnlTimeTableList" runat="server">
                                                                <asp:CheckBoxList ID="chkTimeTableList" runat="server" AutoPostBack="True" OnSelectedIndexChanged="chkTimeTableList_SelectedIndexChanged"
                                                                    RepeatColumns="1" Font-Names="Verdana" Font-Size="10pt">
                                                                </asp:CheckBoxList>
                                                            </div>
                                                        </div>
                                                        <div class="col-md-6">
                                                            <asp:Label ID="lblselectdate" runat="server" Font-Bold="true"
                                                                Text="<%$ Resources:Attendance,selectdate %>"></asp:Label>
                                                            <div id="pnlAddDays" runat="server">
                                                                <asp:CheckBoxList ID="chkDayUnderPeriod" runat="server" RepeatColumns="1"
                                                                    Font-Names="Verdana" Font-Size="10pt">
                                                                </asp:CheckBoxList>
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
                    </div>
                </div>
            </div>
        </div>
    </div>

    <div class="modal fade" id="Modal_View" tabindex="-1" role="dialog" aria-labelledby="Modal_ViewLabel"
        aria-hidden="true">
        <div class="modal-dialog">
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal">
                        <span aria-hidden="true">&times;</span><span class="sr-only">Close</span></button>
                    <h4 class="modal-title" id="Modal_ViewLabel">View Temporary Shift</h4>
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
                                                        Text="<%$ Resources:Attendance,Back To List %>" Style="display: none;"></asp:LinkButton>

                                                    <asp:Label ID="lblschheader" Font-Bold="true" runat="server" Text="<%$ Resources:Attendance,Schedule For %>"></asp:Label>
                                                    &nbsp:&nbsp<asp:Label ID="lblempname" Style="margin-left: 15px;" Font-Bold="true" runat="server"></asp:Label>
                                                    <br />
                                                    <br />
                                                </div>
                                                <div class="col-md-4">
                                                    <asp:Label ID="lblFromDate" runat="server" Text="<%$ Resources:Attendance,From Date  %>"></asp:Label>
                                                    <a style="color: Red">*</a>
                                                    <asp:RequiredFieldValidator EnableClientScript="true" Style="float: right;" runat="server" ID="RequiredFieldValidator3" ValidationGroup="F_Date"
                                                        Display="Dynamic" SetFocusOnError="true" ControlToValidate="txtFromDate" ErrorMessage="<%$ Resources:Attendance,Enter From Date %>"></asp:RequiredFieldValidator>

                                                    <asp:TextBox ID="txtFromDate" runat="server" CssClass="form-control"></asp:TextBox>
                                                    <cc1:CalendarExtender OnClientShown="showCalendar"
                                                        ID="txtFromDate_CalendarExtender" runat="server" Enabled="True" TargetControlID="txtFromDate">
                                                    </cc1:CalendarExtender>


                                                    <br />
                                                </div>
                                                <div class="col-md-4">
                                                    <asp:Label ID="lblToDate" runat="server" Text="<%$ Resources:Attendance,To Date  %>"></asp:Label>
                                                    <a style="color: Red">*</a>
                                                    <asp:RequiredFieldValidator EnableClientScript="true" Style="float: right;" runat="server" ID="RequiredFieldValidator1" ValidationGroup="F_Date"
                                                        Display="Dynamic" SetFocusOnError="true" ControlToValidate="txtToDate" ErrorMessage="<%$ Resources:Attendance,Enter To Date %>"></asp:RequiredFieldValidator>
                                                    <asp:TextBox ID="txtToDate" runat="server" CssClass="form-control"></asp:TextBox>
                                                    <cc1:CalendarExtender OnClientShown="showCalendar"
                                                        ID="txtToDate_CalendarExtender" runat="server" Enabled="True" TargetControlID="txtToDate">
                                                    </cc1:CalendarExtender>


                                                    <br />
                                                </div>
                                                <div class="col-md-4">
                                                    <br />
                                                    <asp:Button ID="btnsubmit" runat="server" Text="<%$ Resources:Attendance,Go %>"
                                                        OnClick="btnsubmit_Click" ValidationGroup="F_Date" CssClass="btn btn-primary" CausesValidation="False" />
                                                </div>
                                                <div class="col-md-12" style="max-height: 500px; overflow: auto">
                                                    <asp:GridView CssClass="table-striped table-bordered table table-hover" ID="GvShiftReport" runat="server" DataKeyNames="Att_Date,Emp_Id,OnDuty_Time,OffDuty_Time" OnRowDataBound="GvShiftreport_RowDataBound"
                                                        AutoGenerateColumns="False" Width="100%">
                                                        <Columns>

                                                            <asp:BoundField DataField="EmpName" HeaderText="<%$ Resources:Attendance,Name %>" />
                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,Date %>">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblDate" runat="server" Text='<%# Convert.ToDateTime(Eval("Att_Date")).ToString("dd-MMM-yyyy") %>'></asp:Label>
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
                                                            <asp:CheckBoxField DataField="Is_Off" HeaderText="<%$ Resources:Attendance,Is Off %>" />
                                                            <asp:CheckBoxField DataField="Is_Temp" HeaderText="<%$ Resources:Attendance,Is Temp %>" />
                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,Is Holiday %>">
                                                                <ItemTemplate>
                                                                    <asp:CheckBox ID="chkHoliday" runat="server" Enabled="false" />
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
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
                <div class="modal-footer">
                    <asp:UpdatePanel ID="Update_Modal_View_Button" runat="server">
                        <ContentTemplate>


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

    <asp:UpdateProgress ID="UpdateProgress4" runat="server" AssociatedUpdatePanelID="Update_Modal_View_Button">
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


</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="FooterContent" runat="Server">
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="AfterLoadScriptContent" runat="Server">
    <script src="../Script/common.js"></script>



    <script type="text/javascript">

        function Confirm() {
            var confirm_value = document.createElement("INPUT");
            confirm_value.type = "hidden";
            confirm_value.name = "confirm_value";
            if (confirm("Shift overlapping , Do you want to overwrite ?")) {
                confirm_value.value = "Yes";
            } else {
                confirm_value.value = "No";
            }
            document.forms[0].appendChild(confirm_value);

        }
        function Li_Tab_New() {
            document.getElementById('<%= Btn_New.ClientID %>').click();
        }

        function Modal_View_Show() {
            document.getElementById('<%= Btn_Modal_View.ClientID %>').click();
        }

        function LI_Edit_Active() {
            $("#Li_List").removeClass("active");
            $("#List").removeClass("active");

            $("#Li_New").addClass("active");
            $("#New").addClass("active");
        }




        function Validate_GvEmpList(sender, args) {
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

        function checkAll(GvEmpList, colIndex) {
            var GridView = GvEmpList.parentNode.parentNode.parentNode;
            var text = [];
            for (var i = 1; i < GridView.rows.length; i++) {
                var chb = GridView.rows[i].cells[colIndex].getElementsByTagName("input")[0];
                chb.checked = GvEmpList.checked;
            }
        }

        function checkItem_All(objRef, colIndex) {
            alert('True');
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
    </script>
</asp:Content>
