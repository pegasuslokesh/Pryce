<%@ Page Title="" Language="C#" MasterPageFile="~/ERPMaster.master" MaintainScrollPositionOnPostback="true" AutoEventWireup="true" CodeFile="EmailSetup.aspx.cs" Inherits="Attendance_EmailSetup" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit.HTMLEditor"
    TagPrefix="cc1" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="Head" runat="Server">
    <style>
        .grid td, .grid th {
            text-align: center;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="HeaderContent" runat="Server">
    <h1>
        <i class="fas fa-exclamation-triangle"></i>&nbsp;&nbsp;
        <asp:Label ID="lblHeader" runat="server" Text="<%$ Resources:Attendance,Alert Setup%>"></asp:Label>

        <%--<small>Control panel</small>--%>
    </h1>
    <ol class="breadcrumb">
        <li><a href="#"><i class="fa fa-dashboard"></i>
            <asp:Label ID="Label6" runat="server" Text="<%$ Resources:Attendance,Master Setup%>"></asp:Label></a></li>
        <li><a href="#"><i class="fa fa-dashboard"></i>
            <asp:Label ID="Label10" runat="server" Text="<%$ Resources:Attendance,Master Setup%>"></asp:Label></a></li>
        <li class="active">
            <asp:Label ID="Label5" runat="server" Text="<%$ Resources:Attendance,Alert Setup%>"></asp:Label></li>
    </ol>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="Server">
    <asp:UpdatePanel ID="Update_Button" runat="server">
        <ContentTemplate>
            <asp:Button ID="Btn_View_Modal" Style="display: none;" runat="server" data-toggle="modal" data-target="#myModal" Text="View Modal" />
            <asp:HiddenField runat="server" ID="hdnCanEdit" />
            <asp:HiddenField runat="server" ID="hdnCanDelete" />

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

    <asp:UpdateProgress ID="UpdateProgress8" runat="server" AssociatedUpdatePanelID="Update_List">
        <ProgressTemplate>
            <div class="modal_Progress">
                <div class="center_Progress">
                    <img alt="" src="../Images/ajax-loader2.gif" style="vertical-align: middle; background-color: White; border-radius: 10px;" />
                </div>
            </div>
        </ProgressTemplate>
    </asp:UpdateProgress>

    <asp:UpdateProgress ID="UpdateProgress9" runat="server" AssociatedUpdatePanelID="Update_New">
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
                    <li><a href="#Bin" data-toggle="tab">
                        <i class="fa fa-trash"></i>&nbsp;&nbsp;<asp:Label ID="Label13" runat="server" Text="<%$ Resources:Attendance,History%>"></asp:Label></a></li>
                    <li id="Li_New"><a href="#New" data-toggle="tab">
                        <asp:UpdatePanel ID="Update_Li" runat="server">
                            <ContentTemplate>
                                <i class="fa fa-file"></i>&nbsp;&nbsp;
                                <asp:Label ID="Lbl_Tab_New" runat="server" Text="<%$ Resources:Attendance,New%>"></asp:Label>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </a></li>
                    <li id="Li_List" class="active"><a href="#List" data-toggle="tab">
                        <i class="fa fa-list"></i>&nbsp;&nbsp;<asp:Label ID="Label2" runat="server" Text="<%$ Resources:Attendance,List %>"></asp:Label></a></li>
                </ul>
                <div class="tab-content">

                    <div class="tab-pane active" id="List">

                        <asp:UpdatePanel ID="Update_List" runat="server">
                            <ContentTemplate>

                                <div class="row">
                                    <div class="col-md-12">
                                        <div id="Div4" runat="server" class="box box-info collapsed-box">
                                            <div class="box-header with-border">
                                                <h3 class="box-title">
                                                    <asp:Label ID="Label22" runat="server" Text="<%$ Resources:Attendance,Advance Search%>"></asp:Label></h3>
                                                &nbsp;&nbsp;|&nbsp;&nbsp;
					<asp:Label ID="lblTotalRecords" runat="server" Font-Bold="true" Text="<%$ Resources:Attendance,Total Records: 0 %>"></asp:Label>
                                                <asp:Label ID="lblSelectedRecord" runat="server" Visible="false"></asp:Label>

                                                <div class="box-tools pull-right">
                                                    <button type="button" class="btn btn-box-tool" data-widget="collapse">
                                                        <i id="I1" runat="server" class="fa fa-plus"></i>
                                                    </button>
                                                </div>
                                            </div>
                                            <div class="box-body">

                                                <div class="col-lg-3">
                                                    <asp:DropDownList ID="ddlFieldName" runat="server" CssClass="form-control">
                                                        <asp:ListItem Selected="True" Text="<%$ Resources:Attendance,Group Name %>" Value="Group_Name"></asp:ListItem>
                                                        <asp:ListItem Text="<%$ Resources:Attendance,Schedule Date %>" Value="Schedule_date"></asp:ListItem>
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
                                                    <asp:Panel ID="Panel1" runat="server" DefaultButton="btnbind">
                                                        <asp:TextBox placeholder="Search from Content" ID="txtValue" runat="server" CssClass="form-control"></asp:TextBox>
                                                    </asp:Panel>
                                                </div>
                                                <div class="col-lg-2">
                                                    <asp:LinkButton ID="btnbind" runat="server" CausesValidation="False" OnClick="btnbind_Click" ToolTip="<%$ Resources:Attendance,Search %>"><span class="fa fa-search"  style="font-size:25px;"></span></asp:LinkButton>&nbsp;&nbsp;&nbsp;
                                                    <asp:LinkButton ID="btnRefresh" runat="server" CausesValidation="False" OnClick="btnRefresh_Click" ToolTip="<%$ Resources:Attendance,Refresh %>"><span class="fa fa-repeat"  style="font-size:25px;"></span></asp:LinkButton>&nbsp;&nbsp;&nbsp;
                                                    <asp:LinkButton ID="ImageButton3" runat="server" OnClick="ImgbtnSelectAll_Click1" ToolTip="<%$ Resources:Attendance, Select All %>"><span class="fas fa-th"  style="font-size:25px;"></span></asp:LinkButton>&nbsp;&nbsp;&nbsp;
                                                    <asp:LinkButton ID="ImageButton4" runat="server" OnClick="ImgbtnDeleteAll_Click1" ToolTip="<%$ Resources:Attendance, Delete %>" Visible="false"><span class="fa fa-remove"  style="font-size:25px;"></span></asp:LinkButton>
                                                </div>
                                                <asp:HiddenField ID="HDFSort" runat="server" />
                                            </div>
                                        </div>
                                    </div>
                                </div>

                                <div class="box box-warning box-solid" <%= gvBankMaster.Rows.Count>0?"style='display:block'":"style='display:none'"%>>
                                    <div class="box-body">
                                        <div class="row">
                                            <div class="col-md-12">
                                                <br />
                                                <div class="flow" class="grid">

                                                    <asp:GridView CssClass="table-striped table-bordered table table-hover" ID="gvBankMaster" PageSize="<%# PageControlCommon.GetPageSize() %>" runat="server"
                                                        AutoGenerateColumns="False" Width="100%" AllowPaging="True" AllowSorting="True" DataKeyNames="Trans_Id"
                                                        OnPageIndexChanging="gvBankMaster_PageIndexChanging" OnSorting="gvBankMaster_OnSorting">
                                                        <Columns>
                                                            <asp:TemplateField>
                                                                <ItemTemplate>
                                                                    <asp:CheckBox ID="chkgvSelect" runat="server" AutoPostBack="true" OnCheckedChanged="chkgvSelect_CheckedChanged" />
                                                                </ItemTemplate>
                                                                <HeaderTemplate>
                                                                    <asp:CheckBox ID="chkgvSelectAll" runat="server" OnCheckedChanged="chkgvSelectAll_CheckedChanged1"
                                                                        AutoPostBack="true" />
                                                                </HeaderTemplate>
                                                                <ItemStyle HorizontalAlign="Center" />
                                                            </asp:TemplateField>

                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,Action%>">
                                                                <ItemTemplate>
                                                                    <div class="dropdown" style="position: absolute;">
                                                                        <button class="btn btn-default dropdown-toggle" type="button" data-toggle="dropdown">
                                                                            <i class="fa fa-ellipsis-h" aria-hidden="true"></i>
                                                                        </button>
                                                                        <ul class="dropdown-menu">

                                                                            <li <%= hdnCanEdit.Value=="true"?"style='display:block'":"style='display:none'" %>>
                                                                                <asp:LinkButton ID="btnEdit" runat="server" CommandArgument='<%# Eval("Trans_Id") %>' CausesValidation="False" OnCommand="btnEdit_Command"><i class="fa fa-pencil"></i><%# Resources.Attendance.Edit%> </asp:LinkButton>
                                                                            </li>
                                                                            <li <%= hdnCanDelete.Value=="true"?"style='display:block'":"style='display:none'" %>>
                                                                                <asp:LinkButton ID="IbtnDelete" runat="server" CausesValidation="False" CommandArgument='<%# Eval("Trans_Id") %>' OnCommand="IbtnDelete_Command"><i class="fa fa-trash"></i><%# Resources.Attendance.Delete%></asp:LinkButton>
                                                                                <cc1:ConfirmButtonExtender ID="confirm2" runat="server" ConfirmText="<%$ Resources:Attendance,Are you sure you want to delete the record?%>" TargetControlID="IbtnDelete"></cc1:ConfirmButtonExtender>
                                                                            </li>
                                                                        </ul>
                                                                    </div>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Trans Id" SortExpression="Trans_Id" Visible="false">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblTransId" runat="server" Text='<%# Eval("Trans_Id") %>' />
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,Group Name %>" SortExpression="Bank_Id">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblGroupName" runat="server" Text='<%# Eval("Group_Name") %>'></asp:Label>
                                                                </ItemTemplate>
                                                                <ItemStyle />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Schedule date" SortExpression="Bank_Name">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblScheduledate" runat="server" Text='<%#GetDate(Eval("Schedule_date").ToString()) %>'></asp:Label>
                                                                </ItemTemplate>
                                                                <ItemStyle />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,Service Run Time %>" SortExpression="Bank_Name_L">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblBankNameL" runat="server" Text='<%#Convert.ToDateTime(Eval("Run_Time").ToString()).ToString("HH:mm") %>'></asp:Label>
                                                                </ItemTemplate>
                                                                <ItemStyle />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Total Email's">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lbltotalEmail" runat="server" Text='<%# GetTotalEmail(Eval("Trans_Id").ToString()) %>'></asp:Label>
                                                                </ItemTemplate>
                                                                <ItemStyle />
                                                            </asp:TemplateField>

                                                            <asp:TemplateField HeaderText="Pending">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblPendingEmail" runat="server" Text='<%# GetTotalPendingEmail(Eval("Trans_Id").ToString(),Eval("Schedule_date").ToString()) %>'></asp:Label>
                                                                </ItemTemplate>
                                                                <ItemStyle />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Failed">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblFailedEmail" runat="server" Text='<%# GetTotalFailedEmail(Eval("Trans_Id").ToString(),Eval("Schedule_date").ToString()) %>'></asp:Label>
                                                                </ItemTemplate>
                                                                <ItemStyle />
                                                            </asp:TemplateField>

                                                            <asp:TemplateField HeaderText="Sent">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblsentEmail" runat="server" Text='<%# GetTotalSentEmail(Eval("Trans_Id").ToString(),Eval("Schedule_date").ToString()) %>'></asp:Label>
                                                                </ItemTemplate>
                                                                <ItemStyle />
                                                            </asp:TemplateField>

                                                        </Columns>


                                                        <PagerStyle CssClass="pagination-ys" />

                                                    </asp:GridView>
                                                    <asp:HiddenField ID="hdnEditId" runat="server" />
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

                                <div id="pnlEmpAtt" runat="server">
                                    <div class="row" runat="server" visible="false">
                                        <div class="col-md-12">
                                            <div class="box box-info">
                                                <div class="box-body">
                                                    <div class="col-md-12" style="text-align: center" runat="server" visible="false">
                                                        <asp:RadioButton ID="rbtnGroupSal" OnCheckedChanged="EmpGroupSal_CheckedChanged"
                                                            runat="server" Text="<%$ Resources:Attendance,Group %>" Font-Bold="true" GroupName="EmpGroup"
                                                            AutoPostBack="true" />
                                                        <asp:RadioButton ID="rbtnEmpSal" runat="server" Style="margin-left: 20px;"
                                                            AutoPostBack="true" Text="<%$ Resources:Attendance,Employee %>" Checked="true" GroupName="EmpGroup"
                                                            Font-Bold="true" OnCheckedChanged="EmpGroupSal_CheckedChanged" />
                                                    </div>
                                                    <div class="col-md-12" runat="server" visible="false">
                                                        <hr />
                                                    </div>
                                                    <div id="Div_Employee_Search" runat="server">


                                                        <div class="col-md-4"></div>
                                                        <div class="col-md-4">
                                                            <div style="text-align: center" runat="server" visible="false">

                                                                <asp:RadioButton ID="rbtnSms" Visible="true" Style="padding: 0px 15px 0px 15px;" runat="server" Text="<%$ Resources:Attendance,SMS%>"
                                                                    Font-Bold="true" GroupName="EmpGroupSal" />

                                                                <asp:RadioButton ID="rbtnEmail" Style="padding: 0px 15px 0px 15px;" Visible="true" runat="server" Text="<%$ Resources:Attendance,Email%>"
                                                                    GroupName="EmpGroupSal" Font-Bold="true" />

                                                                <asp:RadioButton ID="rbtnBoth" Style="padding: 0px 15px 0px 15px;" Visible="true" runat="server" Text="<%$ Resources:Attendance,Both%>"
                                                                    GroupName="EmpGroupSal" Font-Bold="true" />
                                                            </div>
                                                            <div style="text-align: center">
                                                            </div>
                                                        </div>
                                                        <div class="col-md-4"></div>
                                                    </div>


                                                    <div id="Div_Group" visible="false" runat="server">
                                                        <div class="col-md-4"></div>
                                                        <div class="col-md-4">
                                                            <div style="text-align: center">
                                                                <asp:ListBox ID="lbxGroupSal" runat="server" Height="211px" Width="171px" SelectionMode="Multiple"
                                                                    AutoPostBack="true" OnSelectedIndexChanged="lbxGroupSal_SelectedIndexChanged"
                                                                    CssClass="list" Font-Names="Trebuchet MS" Font-Size="Small" ForeColor="Gray"></asp:ListBox>
                                                            </div>
                                                        </div>
                                                        <div class="col-md-4"></div>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <div id="pnlGroupSal" visible="false" runat="server" class="row">
                                        <div class="col-md-12">
                                            <div class="box box-info">
                                                <div class="box-body">
                                                    <div class="col-md-12">
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
                                                                <asp:TemplateField HeaderText="<%$ Resources:Attendance,Email Id %>">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblProductCode" runat="server" Text='<%# Eval("Email_Id") %>'></asp:Label>
                                                                    </ItemTemplate>
                                                                    <ItemStyle />
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="<%$ Resources:Attendance,Designation Name %>">
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
                                    <div id="pnlEmp" runat="server" class="row">
                                        <div class="col-md-12">

                                            <asp:RadioButton ID="rbtnManagerNotification" runat="server" Text="<%$ Resources:Attendance,Manager Notification%>" Checked="true" GroupName="Type" OnCheckedChanged="rbtnManagerNotification_CheckedChanged" AutoPostBack="true" />
                                            <asp:RadioButton ID="rbtnEmployeeNotification" runat="server" Text="<%$ Resources:Attendance,Employee Notification%>" Checked="false" GroupName="Type" OnCheckedChanged="rbtnManagerNotification_CheckedChanged" AutoPostBack="true" />
                                            <div class="box box-info">
                                                <div class="box-body">
                                                    <div class="col-md-12" runat="server">
                                                        <asp:Label ID="Label4" Font-Bold="true" Font-Size="16px" runat="server"
                                                            Text="<%$ Resources:Attendance,Step-1 : Email Notification of Employee%>"></asp:Label>

                                                    </div>
                                                    <div class="col-md-12" runat="server">

                                                        <br />
                                                        <div class="col-md-5">
                                                            <asp:Label ID="lblEmp" Visible="false" runat="server"></asp:Label>


                                                            <asp:Label ID="lblLocation" runat="server" Text="<%$ Resources:Attendance,Location %>"></asp:Label>
                                                            <asp:DropDownList ID="ddlLocation" runat="server" CssClass="form-control" AutoPostBack="true"
                                                                OnSelectedIndexChanged="dpLocation_SelectedIndexChanged">
                                                            </asp:DropDownList>
                                                        </div>
                                                        <div class="col-md-5">
                                                            <asp:Label ID="lblGroupByDept" runat="server" Text="<%$ Resources:Attendance,Department %>"></asp:Label>
                                                            <asp:ListBox ID="listEmpDept" SelectionMode="Multiple" runat="server" Style="width: 100%; height: 150px;"></asp:ListBox>
                                                            <br />

                                                        </div>
                                                        <div class="col-md-2">
                                                            <asp:LinkButton ID="ImageButton1" runat="server"  CausesValidation="False"  OnClick="btnbindDeptEmp_Click" ToolTip="<%$ Resources:Attendance,Search %>" ><span class="fa fa-search"  style="font-size:25px;"></span></asp:LinkButton>&nbsp;&nbsp;&nbsp;
                                                            <asp:LinkButton ID="ImageButton2" runat="server"  CausesValidation="False" OnClick="btnAllRefresh_Click" ToolTip="<%$ Resources:Attendance,Refresh %>" ><span class="fa fa-repeat"  style="font-size:25px;"></span></asp:LinkButton>
                                                        </div>
                                                    </div>


                                                    <div class="col-md-12" runat="server">
                                                        <br />
                                                    </div>
                                                    <div class="col-md-12">


                                                        <div class="row">
                                                            <div class="col-md-12">
                                                                <div id="Div5" runat="server" class="box box-info collapsed-box">
                                                                    <div class="box-header with-border">
                                                                        <h3 class="box-title">
                                                                            <asp:Label ID="Label16" runat="server" Text="<%$ Resources:Attendance,Advance Search%>"></asp:Label></h3>
                                                                        &nbsp;&nbsp;|&nbsp;&nbsp;
					<asp:Label ID="lblTotalRecord" Text="<%$ Resources:Attendance,Total Records: 0 %>" Visible="false" runat="server"></asp:Label>

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
                                                                            </asp:DropDownList>
                                                                        </div>
                                                                        <div class="col-lg-2">
                                                                            <asp:DropDownList ID="ddlOptionEmpFilter" runat="server" CssClass="form-control">
                                                                                <asp:ListItem Text="<%$ Resources:Attendance,--Select-- %>"></asp:ListItem>
                                                                                <asp:ListItem Text="<%$ Resources:Attendance,Equal %>"></asp:ListItem>
                                                                                <asp:ListItem Selected="True" Text="<%$ Resources:Attendance,Contains %>"></asp:ListItem>
                                                                                <asp:ListItem Text="<%$ Resources:Attendance,Like %>"></asp:ListItem>
                                                                            </asp:DropDownList>
                                                                        </div>
                                                                        <div class="col-lg-3">
                                                                            <asp:Panel ID="Panel2" runat="server" DefaultButton="ImageButton8">
                                                                                <asp:TextBox ID="txtValueEmpFilter" class="form-control" runat="server"></asp:TextBox>
                                                                            </asp:Panel>

                                                                        </div>
                                                                        <div class="col-lg-2" style="text-align: center;">
                                                                            <asp:LinkButton ID="ImageButton8"  runat="server" CausesValidation="False" OnClientClick="tab_3_open()"  OnClick="btnarybind_Click1" ToolTip="<%$ Resources:Attendance,Search %>"><span class="fa fa-search"  style="font-size:25px;"></span></asp:LinkButton>&nbsp;&nbsp;&nbsp;
                                                                            <asp:LinkButton ID="ImageButton9" runat="server" CausesValidation="False" OnClientClick="tab_3_open()" OnClick="btnaryRefresh_Click1" ToolTip="<%$ Resources:Attendance,Refresh %>"><span class="fa fa-repeat"  style="font-size:25px;"></span></asp:LinkButton>&nbsp;&nbsp;&nbsp;
                                                                            <asp:LinkButton ID="ImageButton10" runat="server" OnClick="ImgbtnSelectAll_Clickary" ToolTip="<%$ Resources:Attendance, Select All %>"><span class="fas fa-th"  style="font-size:25px;"></span></asp:LinkButton>
                                                                        </div>
                                                                        <div class="col-lg-2">
                                                                            <asp:Button ID="Button2" runat="server" ValidationGroup="Grid_Empl" CssClass="btn btn-primary"
                                                                                ImageUrl="~/Images/buttonCancel.png" OnClick="btnAddReportingList_Click"
                                                                                Text="<%$ Resources:Attendance,Add Employee%>" />
                                                                        </div>

                                                                    </div>
                                                                </div>
                                                            </div>
                                                        </div>
                                                        <div class="row">
                                                            <div class="col-md-12">
                                                                <asp:CustomValidator ID="CustomValidator1" ValidationGroup="Grid_Empl" runat="server" ErrorMessage="<%$ Resources:Attendance,Please select at least one record %>"
                                                                    ClientValidationFunction="Validate_Employee_Sal" ForeColor="Red"></asp:CustomValidator>
                                                                <div style="overflow: auto">
                                                                    <asp:HiddenField ID="Edit" runat="server" />
                                                                    <div style="overflow: auto; max-height: 200px">
                                                                        <asp:GridView CssClass="table-striped table-bordered table table-hover" ID="gvEmployee" runat="server" AllowPaging="false" AutoGenerateColumns="False"
                                                                            OnPageIndexChanging="gvEmp_PageIndexChanging"
                                                                            Width="100%" DataKeyNames="Emp_Id" PageSize="<%# PageControlCommon.GetPageSize() %>"
                                                                            Visible="true">
                                                                            <Columns>

                                                                                <asp:TemplateField>
                                                                                    <ItemTemplate>
                                                                                        <asp:CheckBox ID="chkgvSelect" runat="server" OnCheckedChanged="chkgvSelect_CheckedChanged1" AutoPostBack="false" />
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
                                                                                    SortExpression="Emp_Name" ItemStyle-Width="40%" />
                                                                                <asp:TemplateField HeaderText="<%$ Resources:Attendance,Email Id %>">
                                                                                    <ItemTemplate>
                                                                                        <asp:Label ID="lblProductCode" runat="server" Text='<%# Eval("Email_Id") %>'></asp:Label>
                                                                                    </ItemTemplate>
                                                                                    <ItemStyle />
                                                                                </asp:TemplateField>
                                                                                <asp:TemplateField HeaderText="<%$ Resources:Attendance,Department %>">
                                                                                    <ItemTemplate>
                                                                                        <asp:Label ID="lblItemType" runat="server" Text='<%# Eval("Department") %>'></asp:Label>
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
                                                        <%--</div>--%>
                                                        <%--  </div>--%>


                                                        <div class="col-md-12">
                                                            <div style="overflow: auto; max-height: 200px">
                                                                <asp:GridView CssClass="table-striped table-bordered table table-hover" ID="GvEmpListSelected" Enabled="true" runat="server" AutoGenerateColumns="false" DataKeyNames="Emp_Id"
                                                                    Width="100%">                                                                    
                                                                    <PagerStyle CssClass="pagination-ys" />

                                                                    <Columns>
                                                                        <asp:TemplateField HeaderText="<%$ Resources:Attendance,Delete %>">
                                                                            <ItemTemplate>
                                                                                <asp:ImageButton ID="IbtnDelete_GvEmpListSelected" runat="server" CausesValidation="False" CommandArgument='<%# Eval("Emp_Id") %>'
                                                                                    ImageUrl="~/Images/Erase.png" OnCommand="IbtnDelete_GvEmpListSelected_Command"
                                                                                    Width="16px" ToolTip="<%$ Resources:Attendance,Delete %>" />
                                                                            </ItemTemplate>
                                                                            <ItemStyle HorizontalAlign="Center" />
                                                                            <ItemStyle />
                                                                        </asp:TemplateField>
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
                                                                        <asp:TemplateField HeaderText="<%$ Resources:Attendance,Department %>">
                                                                            <ItemTemplate>
                                                                                <asp:Label ID="lblItemType" runat="server" Text='<%# Eval("Department") %>'></asp:Label>
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
                                    </div>
                                </div>
                                <div id="pnlReportType" runat="server">
                                    <div class="row">
                                        <div class="col-md-12">
                                            <div class="box box-info">
                                                <div class="box-body">
                                                    <div class="col-md-12">
                                                        <asp:Label ID="lblnew" Font-Bold="true" Font-Size="16px" runat="server"
                                                            Text="<%$ Resources:Attendance,Step-2 : Select Notification%>"></asp:Label>
                                                        <br />

                                                        <asp:CustomValidator ID="CustomValidator2" ValidationGroup="Emp22_Save" runat="server" ErrorMessage="Please select at least one record."
                                                            ClientValidationFunction="Validate_For_Report" ForeColor="Red"></asp:CustomValidator>

                                                        <asp:GridView CssClass="table-striped table-bordered table table-hover" ID="gvReportType" runat="server" AutoGenerateColumns="False" Width="100%">
                                                            <RowStyle />
                                                            <Columns>
                                                                <asp:TemplateField>
                                                                    <ItemTemplate>
                                                                        <asp:CheckBox ID="chkReportSel" runat="server" />
                                                                    </ItemTemplate>
                                                                    <HeaderTemplate>
                                                                        <asp:CheckBox ID="chkSelAll" runat="server" AutoPostBack="True" OnCheckedChanged="chkSelAll_CheckedChangedR" />
                                                                    </HeaderTemplate>
                                                                    <HeaderStyle HorizontalAlign="Left" />
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="<%$ Resources:Attendance,Notification Name%>">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblReportName" runat="server" Text='<%# Eval("Notification_Name") %>'></asp:Label>
                                                                        <asp:HiddenField ID="hdnNotificationId" runat="server" Value='<%# Eval("Notification_Id") %>' />
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="<%$ Resources:Attendance,Notification Type%>">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblReportType" runat="server" Text='<%# Eval("Notification_Type") %>'></asp:Label>
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
                                <div id="pnlEmpNf" runat="server">
                                    <div class="row">
                                        <div class="col-md-12">
                                            <div class="box box-info">
                                                <div class="box-body">
                                                    <div class="col-md-12" runat="server">
                                                        <asp:Label ID="Label7" Font-Bold="true" Font-Size="16px" runat="server"
                                                            Text="<%$ Resources:Attendance,Step-3 : Send Email Notification to Employee%>"></asp:Label>

                                                    </div>
                                                    <div class="col-md-12" runat="server">
                                                        <br />
                                                    </div>


                                                    <div class="col-md-12">
                                                        <div class="col-lg-1">


                                                            <asp:Label ID="Label1" runat="server" Text="<%$ Resources:Attendance,Employee %>"></asp:Label>


                                                            <asp:DropDownList ID="ddlFieldNF" runat="server" CssClass="form-control" Visible="false">
                                                                <asp:ListItem Text="<%$ Resources:Attendance,Employee Code %>" Value="Emp_Code"></asp:ListItem>
                                                                <asp:ListItem Text="<%$ Resources:Attendance,Employee Name %>" Value="Emp_Name" Selected="True"></asp:ListItem>
                                                            </asp:DropDownList>
                                                        </div>

                                                        <div class="col-lg-5">


                                                            <asp:TextBox ID="txtEmp" OnTextChanged="ddlEmp_TextChanged" AutoPostBack="true"
                                                                BackColor="#eeeeee" runat="server" CssClass="form-control" />
                                                            <cc1:AutoCompleteExtender ID="AutoCompleteExtender2" runat="server" DelimiterCharacters="" EnableCaching="true"
                                                                Enabled="True" ServiceMethod="GetCompletionList" ServicePath="" CompletionInterval="100"
                                                                MinimumPrefixLength="1" CompletionSetCount="1" TargetControlID="txtEmp" UseContextKey="True"
                                                                CompletionListCssClass="autoCompleteList" CompletionListItemCssClass="autoCompleteListItem" CompletionListHighlightedItemCssClass="autoCompleteSelectedListItem" OnClientShown="resetPosition">
                                                            </cc1:AutoCompleteExtender>

                                                            <asp:RequiredFieldValidator EnableClientScript="true" Style="float: right;" runat="server" ID="RequiredFieldValidator2" ValidationGroup="dEmp_Save"
                                                                Display="Dynamic" SetFocusOnError="true" ControlToValidate="txtEmp" ErrorMessage="<%$ Resources:Attendance,Enter Employee Name %>"></asp:RequiredFieldValidator>
                                                            <asp:TextBox ID="txtValueNF" runat="server" CssClass="form-control" Visible="false"></asp:TextBox>
                                                        </div>
                                                        <div class="col-lg-2" style="text-align: center;">


                                                            <asp:Button ID="btnAddList" runat="server" ValidationGroup="dEmp_Save" CssClass="btn btn-primary"
                                                                ImageUrl="~/Images/buttonCancel.png" OnClick="btnAddList_Click"
                                                                Text="<%$ Resources:Attendance,Add Employee%>" />

                                                        </div>

                                                        <div class="box-body">
                                                            <div class="row">
                                                                <div class="col-md-12">
                                                                    <asp:CustomValidator ID="CustomValidator3" ValidationGroup="Emp_Save" runat="server" Enabled="false" ErrorMessage="Please select at least one record."
                                                                        ClientValidationFunction="Validate_gvEmpNF" ForeColor="Red"></asp:CustomValidator>

                                                                    <div style="overflow: auto">
                                                                        <asp:GridView CssClass="table-striped table-bordered table table-hover" ID="gvEmpNF" runat="server" AutoGenerateColumns="False"
                                                                            Width="100%"
                                                                            DataKeyNames="Emp_Id" PageSize="<%# PageControlCommon.GetPageSize() %>" Visible="true">
                                                                            <Columns>

                                                                                <asp:TemplateField HeaderText="<%$ Resources:Attendance,Delete %>">
                                                                                    <ItemTemplate>
                                                                                        <asp:ImageButton ID="IbtnDelete_gvEmpNF" runat="server" CausesValidation="False" CommandArgument='<%# Eval("Emp_Id") %>'
                                                                                            ImageUrl="~/Images/Erase.png" OnCommand="IbtnDelete_gvEmpNF_Command"
                                                                                            Width="16px" ToolTip="<%$ Resources:Attendance,Delete %>" />
                                                                                    </ItemTemplate>
                                                                                    <ItemStyle HorizontalAlign="Center" />
                                                                                    <ItemStyle />
                                                                                </asp:TemplateField>

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
                                                                                <asp:TemplateField HeaderText="<%$ Resources:Attendance,Department%>">
                                                                                    <ItemTemplate>
                                                                                        <asp:Label ID="lblItemType" runat="server" Text='<%# Eval("Department") %>'></asp:Label>
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



                                                        <%-- </div>--%>
                                                    </div>

                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>

                                <div id="Div2" runat="server">
                                    <div class="row">
                                        <div class="col-md-12">
                                            <div class="box box-info">
                                                <div class="box-body">

                                                    <div class="col-md-12">
                                                        <asp:Label ID="Label11" Font-Bold="true" Font-Size="16px" runat="server"
                                                            Text="<%$ Resources:Attendance,Step-4 : Report Parameter%>"></asp:Label>
                                                        <br />
                                                    </div>

                                                    <div class="col-md-12">
                                                        <br />
                                                    </div>

                                                    <div class="col-md-12">
                                                        <div class="col-md-6">

                                                            <asp:Label ID="Label9" runat="server" Text="<%$ Resources:Attendance,Group Name %>"></asp:Label><a style="color: Red">*</a>
                                                            <asp:RequiredFieldValidator EnableClientScript="true" runat="server" ID="RequiredFieldValidator4" ValidationGroup="Emp_Save"
                                                                Display="Dynamic" SetFocusOnError="true" ControlToValidate="txtGroupName" ErrorMessage="Enter Group Name" />

                                                            <asp:TextBox ID="txtGroupName" runat="server" CssClass="form-control" OnTextChanged="txtGroupName_OnTextChanged" AutoPostBack="true" BackColor="#eeeeee"></asp:TextBox>
                                                            <cc1:AutoCompleteExtender ID="AutoCompleteExtender1" runat="server" DelimiterCharacters="" EnableCaching="true"
                                                                Enabled="True" ServiceMethod="GetCompletionListGroupName" ServicePath="" CompletionInterval="100"
                                                                MinimumPrefixLength="1" CompletionSetCount="1" TargetControlID="txtGroupName" UseContextKey="True"
                                                                CompletionListCssClass="autoCompleteList" CompletionListItemCssClass="autoCompleteListItem" CompletionListHighlightedItemCssClass="autoCompleteSelectedListItem" OnClientShown="resetPosition">
                                                            </cc1:AutoCompleteExtender>
                                                            <br />
                                                        </div>
                                                    </div>

                                                    <div class="col-md-12">
                                                        <div class="col-md-6">

                                                            <asp:Label ID="Label3" runat="server" Text="<%$ Resources:Attendance,Schedule Days %>"></asp:Label><a style="color: Red">*</a>
                                                            <asp:RequiredFieldValidator EnableClientScript="true" runat="server" ID="RequiredFieldValidator1" ValidationGroup="Emp_Save"
                                                                Display="Dynamic" SetFocusOnError="true" ControlToValidate="txtDays" ErrorMessage="<%$ Resources:Attendance,Enter Schedule Days %>" />

                                                            <asp:TextBox ID="txtDays" runat="server" CssClass="form-control"></asp:TextBox>
                                                            <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender5" runat="server" Enabled="True"
                                                                TargetControlID="txtDays" ValidChars="1,2,3,4,5,6,7,8,9,0">
                                                            </cc1:FilteredTextBoxExtender>
                                                            <br />
                                                        </div>
                                                        <div class="col-md-6">
                                                            <asp:Label ID="Label8" runat="server" Text="<%$ Resources:Attendance,Run Time%>"></asp:Label>
                                                            <a style="color: Red">*</a>
                                                            <asp:RequiredFieldValidator EnableClientScript="true" runat="server" ID="RequiredFieldValidator3" ValidationGroup="Emp_Save"
                                                                Display="Dynamic" SetFocusOnError="true" ControlToValidate="txtserviceRunTime" ErrorMessage="Enter Run Time" />



                                                            <asp:TextBox ID="txtserviceRunTime" runat="server" CssClass="form-control" />
                                                            <cc1:MaskedEditExtender ID="MaskedEditExtender1" runat="server" CultureAMPMPlaceholder=""
                                                                CultureCurrencySymbolPlaceholder="" CultureDateFormat="" CultureDatePlaceholder=""
                                                                CultureDecimalPlaceholder="" CultureThousandsPlaceholder="" CultureTimePlaceholder=""
                                                                Enabled="True" ErrorTooltipEnabled="True" Mask="99:99" MaskType="Time" TargetControlID="txtserviceRunTime"
                                                                UserTimeFormat="TwentyFourHour" MessageValidatorTip="true" InputDirection="LeftToRight">
                                                            </cc1:MaskedEditExtender>
                                                            <cc1:MaskedEditValidator ID="EarliestTimeValidator" runat="server" ControlExtender="MaskedEditExtender1"
                                                                ControlToValidate="txtserviceRunTime" Display="Dynamic" InvalidValueMessage="Please enter a valid time."
                                                                SetFocusOnError="True" />

                                                        </div>
                                                    </div>

                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>


                                <div id="Div_Body" runat="server">
                                    <div class="row">
                                        <div class="col-md-12">

                                            <cc1:TabContainer ID="TabContainer1" runat="server" ActiveTabIndex="0" CssClass="ajax__tab_yuitabview-theme">
                                                <cc1:TabPanel ID="TabPanel3" runat="server" HeaderText="<%$ Resources:Attendance,Help %>">
                                                    <ContentTemplate>
                                                        <asp:UpdatePanel ID="UpdatePanel_Help" runat="server">
                                                            <ContentTemplate>
                                                                <div class="row">
                                                                    <div class="col-md-12">
                                                                        <cc1:Editor ID="txtHelpContent" Enabled="false" runat="server" Width="100%" ActiveMode="Preview" />
                                                                    </div>
                                                                </div>
                                                            </ContentTemplate>
                                                        </asp:UpdatePanel>
                                                        <asp:UpdateProgress ID="UpdateProgress7" runat="server" AssociatedUpdatePanelID="UpdatePanel_Help">
                                                            <ProgressTemplate>
                                                                <div class="modal_Progress">
                                                                    <div class="center_Progress">
                                                                        <img alt="" src="../Images/ajax-loader2.gif" style="vertical-align: middle; background-color: White; border-radius: 10px;" />
                                                                    </div>
                                                                </div>
                                                            </ProgressTemplate>
                                                        </asp:UpdateProgress>
                                                    </ContentTemplate>
                                                </cc1:TabPanel>
                                                <cc1:TabPanel ID="Tab_Description" runat="server" HeaderText="<%$ Resources:Attendance,Subject %>">
                                                    <ContentTemplate>
                                                        <asp:UpdatePanel ID="Update_Tab_Subject" runat="server">
                                                            <ContentTemplate>
                                                                <div class="row">
                                                                    <div class="col-md-12">
                                                                        <asp:TextBox ID="txtSubjectContent" runat="server" Width="100%" Placeholder="Enter Email Subject" CssClass="form-control"></asp:TextBox>
                                                                        <%-- <cc1:Editor ID="txtSubjectContent" runat="server" Width="100%" />--%>
                                                                    </div>
                                                                </div>
                                                            </ContentTemplate>
                                                        </asp:UpdatePanel>
                                                        <asp:UpdateProgress ID="UpdateProgress4" runat="server" AssociatedUpdatePanelID="Update_Tab_Subject">
                                                            <ProgressTemplate>
                                                                <div class="modal_Progress">
                                                                    <div class="center_Progress">
                                                                        <img alt="" src="../Images/ajax-loader2.gif" style="vertical-align: middle; background-color: White; border-radius: 10px;" />
                                                                    </div>
                                                                </div>
                                                            </ProgressTemplate>
                                                        </asp:UpdateProgress>
                                                    </ContentTemplate>
                                                </cc1:TabPanel>
                                                <cc1:TabPanel ID="TabPanel1" runat="server" HeaderText="<%$ Resources:Attendance,Header %>">
                                                    <ContentTemplate>
                                                        <asp:UpdatePanel ID="Update_Tab_Header" runat="server">
                                                            <ContentTemplate>
                                                                <div class="row">
                                                                    <div class="col-md-12">
                                                                        <cc1:Editor ID="txtHeaderContent" runat="server" Width="100%" />
                                                                    </div>
                                                                </div>
                                                            </ContentTemplate>
                                                        </asp:UpdatePanel>
                                                        <asp:UpdateProgress ID="UpdateProgress5" runat="server" AssociatedUpdatePanelID="Update_Tab_Header">
                                                            <ProgressTemplate>
                                                                <div class="modal_Progress">
                                                                    <div class="center_Progress">
                                                                        <img alt="" src="../Images/ajax-loader2.gif" style="vertical-align: middle; background-color: White; border-radius: 10px;" />
                                                                    </div>
                                                                </div>
                                                            </ProgressTemplate>
                                                        </asp:UpdateProgress>
                                                    </ContentTemplate>
                                                </cc1:TabPanel>
                                                <cc1:TabPanel ID="TabPanel2" runat="server" HeaderText="<%$ Resources:Attendance,Footer %>">
                                                    <ContentTemplate>
                                                        <asp:UpdatePanel ID="Update_Tab_Footer" runat="server">
                                                            <ContentTemplate>
                                                                <div class="row">
                                                                    <div class="col-md-12">
                                                                        <cc1:Editor ID="txtFooterContent" runat="server" Width="100%" />
                                                                    </div>
                                                                </div>
                                                            </ContentTemplate>
                                                        </asp:UpdatePanel>
                                                        <asp:UpdateProgress ID="UpdateProgress6" runat="server" AssociatedUpdatePanelID="Update_Tab_Footer">
                                                            <ProgressTemplate>
                                                                <div class="modal_Progress">
                                                                    <div class="center_Progress">
                                                                        <img alt="" src="../Images/ajax-loader2.gif" style="vertical-align: middle; background-color: White; border-radius: 10px;" />
                                                                    </div>
                                                                </div>
                                                            </ProgressTemplate>
                                                        </asp:UpdateProgress>
                                                    </ContentTemplate>
                                                </cc1:TabPanel>

                                            </cc1:TabContainer>


                                        </div>
                                    </div>
                                </div>




                                <div id="Div1" runat="server" style="text-align: center">
                                    </br>
                                    <asp:Button ID="btnSave" runat="server" Visible="false" ValidationGroup="Emp_Save" Text="<%$ Resources:Attendance,Save %>" CssClass="btn btn-success"
                                        OnClick="btnSave_Click" />

                                    &nbsp;

                                                                     <asp:Button ID="Button1" runat="server" OnClick="btnReset_Click" Text="<%$ Resources:Attendance,Reset %>"
                                                                         CssClass="btn btn-primary" Visible="true" />
                                    &nbsp;
                                                                            <asp:Button ID="Button3" runat="server" Text="<%$ Resources:Attendance,Cancel %>"
                                                                                CssClass="btn btn-danger" OnClick="btnCancel_Click" />
                                    </br>

                                </div>

                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </div>
                    <!-- /.tab-pane -->
                    <div class="tab-pane" id="Bin">
                        <asp:UpdatePanel ID="Update_Bin" runat="server">

                            <ContentTemplate>
                                <div class="row">
                                    <div class="col-md-12">
                                        <br />

                                        <div class="col-md-6">
                                            <asp:Label ID="Label27" runat="server" Text="<%$ Resources:Attendance,From Date %>"></asp:Label>
                                            <a style="color: Red">*</a>
                                            <asp:RequiredFieldValidator EnableClientScript="true" runat="server" ID="RequiredFieldValidator5" ValidationGroup="btn_History"
                                                Display="Dynamic" SetFocusOnError="true" ControlToValidate="txtFromDate" ErrorMessage="Enter From date" />


                                            <asp:TextBox ID="txtFromDate" runat="server" CssClass="form-control"></asp:TextBox>
                                            <cc1:CalendarExtender OnClientShown="showCalendar" ID="txtFrom_CalendarExtender" runat="server" Enabled="True"
                                                TargetControlID="txtFromDate">
                                            </cc1:CalendarExtender>
                                            <br />
                                        </div>
                                        <div class="col-md-6">
                                            <asp:Label ID="Label12" runat="server" Text="<%$ Resources:Attendance,To Date %>"></asp:Label>
                                            <a style="color: Red">*</a>
                                            <asp:RequiredFieldValidator EnableClientScript="true" runat="server" ID="RequiredFieldValidator6" ValidationGroup="btn_History"
                                                Display="Dynamic" SetFocusOnError="true" ControlToValidate="txtToDate" ErrorMessage="Enter To date" />

                                            <asp:TextBox ID="txtToDate" runat="server" CssClass="form-control"></asp:TextBox>
                                            <cc1:CalendarExtender OnClientShown="showCalendar" ID="CalendarExtender1" runat="server" Enabled="True" TargetControlID="txtToDate">
                                            </cc1:CalendarExtender>
                                            <br />
                                        </div>
                                        <br />

                                    </div>

                                    <div class="col-md-12">

                                        <div class="col-md-6">
                                            <asp:Label ID="Label14" runat="server" Text="<%$ Resources:Attendance,Group Name %>"></asp:Label>



                                            <asp:DropDownList ID="ddlGroupName" runat="server" CssClass="form-control"></asp:DropDownList>

                                            <br />
                                        </div>
                                        <div class="col-md-6">
                                            <asp:Label ID="Label15" runat="server" Text="<%$ Resources:Attendance,Status %>"></asp:Label>

                                            <asp:DropDownList ID="ddlStatus" runat="server" CssClass="form-control">

                                                <asp:ListItem Text="--Select--" Value="0"></asp:ListItem>
                                                <asp:ListItem Text="Pending" Value="Pending"></asp:ListItem>
                                                <asp:ListItem Text="Failed" Value="Failed"></asp:ListItem>
                                                <asp:ListItem Text="Sent" Value="Send"></asp:ListItem>

                                            </asp:DropDownList>

                                            <br />
                                        </div>


                                    </div>




                                    <div id="Div3" runat="server" style="text-align: center">

                                        <asp:Button ID="btnHistory" runat="server" ValidationGroup="btn_History" Text="Get Record" CssClass="btn btn-primary"
                                            OnClick="btnHistory_Click" />


                                    </div>



                                    <div class="col-md-12">
                                        <div class="col-md-12" style="text-align: right">
                                            <asp:Label ID="Lbl_TotalRecords" Visible="false" Font-Bold="true" Text="<%$ Resources:Attendance,Total Records: 0 %>" runat="server"></asp:Label>
                                        </div>
                                        <br />

                                        <div class="col-md-12" style="overflow: auto; max-height: 200px">


                                            <asp:GridView CssClass="table-striped table-bordered table table-hover" ID="gvEmailStatus" PageSize="<%# PageControlCommon.GetPageSize() %>" runat="server"
                                                AutoGenerateColumns="False" Width="100%" AllowPaging="false" AllowSorting="True" DataKeyNames="Emp_id">
                                                <Columns>


                                                    <asp:TemplateField HeaderText="<%$ Resources:Attendance,Group Name %>">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblGroupName" runat="server" Text='<%# Eval("Group_Name") %>'></asp:Label>
                                                        </ItemTemplate>
                                                        <ItemStyle />
                                                    </asp:TemplateField>

                                                    <asp:TemplateField HeaderText="<%$ Resources:Attendance,Employee Code %>">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblEmpCode" runat="server" Text='<%# Eval("Emp_Code") %>'></asp:Label>
                                                        </ItemTemplate>
                                                        <ItemStyle />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="<%$ Resources:Attendance,Employee Name %>">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblEmpName" runat="server" Text='<%# Eval("Emp_Name") %>'></asp:Label>
                                                        </ItemTemplate>
                                                        <ItemStyle />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="<%$ Resources:Attendance,Email Id %>">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblEmailId" runat="server" Text='<%# Eval("Email_Id") %>'></asp:Label>
                                                        </ItemTemplate>
                                                        <ItemStyle />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Schedule date">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblScheduledate" runat="server" Text='<%#GetDate(Eval("Generate_date").ToString()) %>'></asp:Label>
                                                        </ItemTemplate>
                                                        <ItemStyle />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Status">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblStatus" runat="server" Text='<%# Eval("Status").ToString() %>'></asp:Label>
                                                        </ItemTemplate>
                                                        <ItemStyle />
                                                    </asp:TemplateField>
                                                </Columns>


                                                <PagerStyle CssClass="pagination-ys" />

                                            </asp:GridView>
                                        </div>
                                    </div>
                                </div>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </div>

                </div>
                <!-- /.tab-content -->
            </div>

        </div>
    </div>


    <asp:UpdateProgress ID="UpdateProgress2" runat="server" AssociatedUpdatePanelID="Update_List">
        <ProgressTemplate>
            <div class="modal_Progress">
                <div class="center_Progress">
                    <img alt="" src="../Images/ajax-loader2.gif" style="vertical-align: middle; background-color: White; border-radius: 10px;" />
                </div>
            </div>
        </ProgressTemplate>
    </asp:UpdateProgress>

    <asp:UpdateProgress ID="UpdateProgress1" runat="server" AssociatedUpdatePanelID="Update_New">
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


</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="FooterContent" runat="Server">
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="AfterLoadScriptContent" runat="Server">
    <script src="../Script/common.js"></script>
    <link href="../CSS/InvStyle.css" rel="stylesheet" type="text/css" />

    <script type="text/javascript">
        function LI_New_Active() {
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


    </script>
    <script type="text/javascript">

        function Modal_Close() {
            document.getElementById('<%= Btn_View_Modal.ClientID %>').click();
        }


        function Validate_Employee_Sal(sender, args) {
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




    </script>
</asp:Content>


