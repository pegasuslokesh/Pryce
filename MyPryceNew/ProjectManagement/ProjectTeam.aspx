<%@ Page Title="" Language="C#" MasterPageFile="~/ERPMaster.master" AutoEventWireup="true" CodeFile="ProjectTeam.aspx.cs" Inherits="ProjectManagement_ProjectTeam" %>

<%@ Register Assembly="DevExpress.Web.v18.1, Version=18.1.3.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Assembly="DevExpress.XtraReports.v18.1.Web.WebForms, Version=18.1.3.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.XtraReports.Web" TagPrefix="dx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="Head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="HeaderContent" runat="Server">
    <h1>
        <i class="fa fa-users"></i>
        <asp:Label ID="lblHeader" runat="server" Text="<%$ Resources:Attendance,Project Team%>"></asp:Label>
    </h1>
    <ol class="breadcrumb">
        <li><a href="#"><i class="fa fa-dashboard"></i>
            <asp:Label ID="Label6" runat="server" Text="<%$ Resources:Attendance,Project Management%>"></asp:Label></a></li>
        <li><a href="#"><i class="fa fa-dashboard"></i>
            <asp:Label ID="Label10" runat="server" Text="<%$ Resources:Attendance,Project Management%>"></asp:Label></a></li>
        <li class="active">
            <asp:Label ID="Label5" runat="server" Text="<%$ Resources:Attendance,Project Team%>"></asp:Label></li>
    </ol>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="Server">
    <asp:UpdatePanel ID="Update_Button" runat="server">
        <ContentTemplate>
            <asp:Button ID="Btn_List" Style="display: none;" runat="server" OnClick="btList_Click" Text="List" />
            <asp:Button ID="Btn_New" Style="display: none;" runat="server" OnClick="btnnew_Click" Text="New" />
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
                    <li id="Li_List" class="active"><a href="#List" onclick="Li_Tab_List()" data-toggle="tab">
                        <i class="fa fa-list"></i>&nbsp;&nbsp;<asp:Label ID="Label7" runat="server" Text="<%$ Resources:Attendance,List %>"></asp:Label></a></li>
                </ul>
                <div class="tab-content">
                    <div class="tab-pane active" id="List">
                        <asp:UpdatePanel ID="Update_List" runat="server">
                            <ContentTemplate>
                                <div class="row">
                                    <div class="col-md-12">
                                        <div id="Div3" runat="server" class="box box-info collapsed-box">
                                            <div class="box-header with-border">
                                                <h3 class="box-title">
                                                    <asp:Label ID="Label1" runat="server" Text="Advance Search"></asp:Label></h3>
                                                &nbsp;&nbsp;|&nbsp;&nbsp;
                                                <asp:Label ID="lblTotalRecords" Font-Bold="true" runat="server" Text="<%$ Resources:Attendance,Total Records: 0 %>"></asp:Label>
                                                <div class="box-tools pull-right">
                                                    <button type="button" class="btn btn-box-tool" data-widget="collapse">
                                                        <i id="I1" runat="server" class="fa fa-plus"></i>
                                                    </button>
                                                </div>
                                            </div>
                                            <div class="box-body">
                                                <div class="form-group">
                                                    <div class="row">
                                                        <div class="form-group">
                                                            <div class="col-lg-2">
                                                                <asp:DropDownList ID="ddlProjectStatus" runat="server" CssClass="form-control"
                                                                    AutoPostBack="true" OnSelectedIndexChanged="ddlProjectStatus_SelectedIndexChanged">
                                                                    <asp:ListItem Text="All" Value="All"></asp:ListItem>
                                                                    <asp:ListItem Text="Open" Value="Open" Selected="True"></asp:ListItem>
                                                                    <asp:ListItem Text="Close" Value="Close"></asp:ListItem>
                                                                </asp:DropDownList>
                                                            </div>
                                                            <div class="col-lg-2">
                                                                <asp:DropDownList ID="ddlFieldName" runat="server" CssClass="form-control">
                                                                    <asp:ListItem Text="Project No." Value="Field7" Selected="True"></asp:ListItem>
                                                                    <asp:ListItem Text="<%$ Resources:Attendance, Project Name %>" Value="Project_Name" />
                                                                    <asp:ListItem Text="Project Manager" Value="ManagerName" />
                                                                    <asp:ListItem Text="<%$ Resources:Attendance, Customer Name %>" Value="Customername" />
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
                                                            <div class="col-lg-4">
                                                                <asp:Panel ID="Panel1" runat="server" DefaultButton="btnbind">
                                                                    <asp:TextBox ID="txtValue" runat="server" CssClass="form-control" placeholder="Search from Content"></asp:TextBox>
                                                                </asp:Panel>
                                                            </div>
                                                            <div class="col-lg-2">
                                                                <asp:LinkButton ID="btnbind" runat="server" CausesValidation="False" OnClick="btnbindrpt_Click" ToolTip="<%$ Resources:Attendance,Search %>">
                                                                    <span class="fa fa-search" style="font-size:25px;"></span>
                                                                </asp:LinkButton>
                                                                &nbsp;&nbsp;&nbsp;
                                                                <asp:LinkButton ID="btnRefresh" runat="server" CausesValidation="False" ToolTip="<%$ Resources:Attendance,Refresh %>" OnClick="btnRefresh_Click">
                                                                    <span class="fa fa-repeat" style="font-size:25px;"></span>
                                                                </asp:LinkButton>
                                                            </div>
                                                        </div>
                                                    </div>
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
                                                    <asp:GridView CssClass="table-striped table-bordered table table-hover" ID="GvrProjectteam" runat="server" AutoGenerateColumns="False" Width="100%"
                                                        AllowPaging="True" OnPageIndexChanging="GvrProjectteam_PageIndexChanging" AllowSorting="True" OnSorting="GvrProjectteam_Sorting">
                                                        <Columns>
                                                            <asp:TemplateField Visible="false">
                                                                <ItemTemplate>
                                                                    <asp:LinkButton ID="btnEdit" runat="server" CommandArgument='<%# Eval("Project_Id") %>' OnCommand="btnEdit_Command" CausesValidation="False" ToolTip="Edit"><i class="fa fa-pencil"></i></asp:LinkButton>
                                                                </ItemTemplate>
                                                                <ItemStyle HorizontalAlign="Center" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Project No." SortExpression="Project_Id">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblProjectNo" runat="server" Text='<%# Eval("Field7") %>'></asp:Label>
                                                                </ItemTemplate>
                                                                <ItemStyle  Width="15%" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,Project Name %>" SortExpression="Project_Name">
                                                                <ItemTemplate>
                                                                    <asp:HiddenField ID="HiddeniD" runat="server" />
                                                                    <asp:HiddenField ID="hdnproID" runat="server" Value='<%# Eval("Project_Id") %>' />
                                                                    <asp:Label ID="lblprojectIdList" runat="server" Text='<%# Eval("Project_Name") %>'></asp:Label>
                                                                </ItemTemplate>
                                                                <ItemStyle  Width="30%" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Project Manager" SortExpression="ManagerName">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblprojectManager" runat="server" Text='<%# Eval("ManagerName") %>'></asp:Label>
                                                                </ItemTemplate>
                                                                <ItemStyle  Width="15%" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,Customer Name %>" SortExpression="Customername">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblcustname2" runat="server" Text='<%# Eval("Customername") %>'></asp:Label>
                                                                </ItemTemplate>
                                                                <ItemStyle  Width="15%" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Team Member" SortExpression="ProjectTeam">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblEmpnameList" runat="server" Text='<%# Eval("ProjectTeam") %>'></asp:Label>
                                                                </ItemTemplate>
                                                                <ItemStyle  Width="20%" />
                                                            </asp:TemplateField>
                                                        </Columns>
                                                        <PagerStyle CssClass="pagination-ys" />
                                                    </asp:GridView>
                                                    <asp:HiddenField ID="HDFSort" runat="server" />
                                                    <asp:HiddenField ID="HiddeniD" runat="server" />
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
                                <%--<div id="pnlteamdetials" runat="server" visible="false">--%>
                                <div class="row">
                                    <div class="col-md-12">
                                        <div class="box box-primary">
                                            <div class="box-body">
                                                <div class="form-group">
                                                    <div class="col-md-6">
                                                        <asp:Label ID="Label4" runat="server" Text="<%$ Resources:Attendance,Project Name  %>"></asp:Label>
                                                        <dx:ASPxComboBox ID="ddlprojectname" runat="server" CssClass="form-control" DropDownWidth="550"
                                                            OnSelectedIndexChanged="ddlprojectname_SelectedIndexChanged" DropDownStyle="DropDownList"
                                                            ValueField="Project_Id" ValueType="System.String" TextFormatString="{0}" EnableCallbackMode="true"
                                                            IncrementalFilteringMode="Contains" AutoPostBack="true" CallbackPageSize="30">
                                                            <Columns>
                                                                <dx:ListBoxColumn FieldName="Project_Name" />
                                                            </Columns>
                                                        </dx:ASPxComboBox>
                                                        <br />
                                                    </div>
                                                    <div class="col-md-6">
                                                        <asp:Label ID="Label13" runat="server" Text="<%$ Resources:Attendance,Employee Name %>"></asp:Label>
                                                        <dx:ASPxComboBox ID="ddlEmployeeName" runat="server" CssClass="form-control" DropDownWidth="550"
                                                            OnSelectedIndexChanged="ddlEmployeeName_SelectedIndexChanged" DropDownStyle="DropDownList"
                                                            ValueField="Emp_Id" ValueType="System.String" TextFormatString="{0}" EnableCallbackMode="true"
                                                            IncrementalFilteringMode="Contains" AutoPostBack="true" CallbackPageSize="30">
                                                            <Columns>
                                                                <dx:ListBoxColumn FieldName="Emp_Name" Caption="Employee Name" />
                                                                <dx:ListBoxColumn FieldName="Emp_Code" Caption="Employee Code" />
                                                                <dx:ListBoxColumn FieldName="Designation" />
                                                            </Columns>
                                                        </dx:ASPxComboBox>
                                                        <br />
                                                    </div>
                                                    <div class="col-md-6">

                                                        <asp:RequiredFieldValidator EnableClientScript="true" Style="float: right;" runat="server" ID="RequiredFieldValidator3" ValidationGroup="Save"
                                                            Display="Dynamic" SetFocusOnError="true" ControlToValidate="txtOnDutyTime" ErrorMessage="<%$ Resources:Attendance,Enter On Duty Time %>"></asp:RequiredFieldValidator>

                                                        <asp:TextBox ID="txtOnDutyTime" placeholder="On Duty Time" runat="server" onchange="validateTime(this)" CssClass="form-control" />
                                                        <cc1:MaskedEditExtender ID="MaskedEditExtender6" runat="server" CultureAMPMPlaceholder=""
                                                            CultureCurrencySymbolPlaceholder="" CultureDateFormat="" CultureDatePlaceholder=""
                                                            CultureDecimalPlaceholder="" CultureThousandsPlaceholder="" CultureTimePlaceholder=""
                                                            Enabled="True" Mask="99:99:99" MaskType="Time" TargetControlID="txtOnDutyTime"
                                                            UserTimeFormat="TwentyFourHour" MessageValidatorTip="true" InputDirection="LeftToRight">
                                                        </cc1:MaskedEditExtender>
                                                        <br />
                                                    </div>
                                                    <div class="col-md-6">
                                                        <asp:RequiredFieldValidator EnableClientScript="true" Style="float: right;" runat="server" ID="RequiredFieldValidator1" ValidationGroup="Save"
                                                            Display="Dynamic" SetFocusOnError="true" ControlToValidate="txtOffDutyTime" ErrorMessage="<%$ Resources:Attendance,Enter Off Duty Time %>"></asp:RequiredFieldValidator>

                                                        <asp:TextBox ID="txtOffDutyTime" placeholder="Off Duty Time" runat="server" onchange="validateTime(this)" CssClass="form-control" />
                                                        <cc1:MaskedEditExtender ID="MaskedEditExtender1" runat="server" CultureAMPMPlaceholder=""
                                                            CultureCurrencySymbolPlaceholder="" CultureDateFormat="" CultureDatePlaceholder=""
                                                            CultureDecimalPlaceholder="" CultureThousandsPlaceholder="" CultureTimePlaceholder=""
                                                            Enabled="True" Mask="99:99:99" MaskType="Time" TargetControlID="txtOffDutyTime"
                                                            UserTimeFormat="TwentyFourHour" MessageValidatorTip="true" InputDirection="LeftToRight">
                                                        </cc1:MaskedEditExtender>
                                                        <br />
                                                    </div>
                                                    <div class="col-md-6">
                                                        <%--<asp:Label ID="Label7" runat="server" Text="Task assigner"></asp:Label>--%>
                                                        <asp:CheckBox ID="chktaskvisibility" Text="Task assigner" CssClass="form-control" runat="server" />
                                                        <br />
                                                    </div>
                                                    <div class="col-md-6">
                                                        <asp:CheckBox ID="chkEligibleIncentive" Text="Eligible for Incentive" CssClass="form-control" runat="server" />
                                                        <br />
                                                    </div>
                                                    <div class="col-md-6">
                                                        <asp:Label ID="Label14" runat="server" Text="Basic salary(In Company Currency)" Visible="false"></asp:Label>
                                                        <asp:TextBox ID="txtBasicSalary" runat="server" Font-Names="Verdana" Visible="false"
                                                            CssClass="form-control"></asp:TextBox>
                                                        <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender18" runat="server" Enabled="True"
                                                            TargetControlID="txtBasicSalary" ValidChars="1,2,3,4,5,6,7,8,9,0,.">
                                                        </cc1:FilteredTextBoxExtender>
                                                        <br />
                                                    </div>
                                                    <div class="col-md-12">
                                                        <br />
                                                    </div>
                                                    <div class="col-md-12">
                                                        <asp:Label ID="lblNotes" runat="server" Text="Notes : On duty time and off duty time should be in 24 hours format. " Font-Italic="true"></asp:Label>
                                                        <br />
                                                        <br />
                                                    </div>
                                                    <div class="col-md-12" style="text-align: center">
                                                        <asp:Button ID="btnCheckTeamAvailable" runat="server" Text="Check Team Availability" Visible="false"
                                                            OnClick="btnCheckTeamAvailable_Click" CssClass="btn btn-warning" />

                                                        <asp:Button ID="btnsubmit" runat="server" Text="Save" Visible="false"
                                                            OnClick="btnsubmit_Click" CssClass="btn btn-success" />

                                                        <asp:Button ID="btnreset" runat="server" Text="Reset" OnClick="btnreset_Click"
                                                            CssClass="btn btn-danger" />

                                                        <asp:HiddenField ID="HidCustId" runat="server" />
                                                        <asp:HiddenField ID="hdnfileid" runat="server" />
                                                        <asp:HiddenField ID="hdnProjId" runat="server" />
                                                        <br />
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>

                                <div class="box box-warning box-solid">
                                    <div class="box-body">
                                        <div class="row">
                                            <div class="col-md-12">
                                                <div class="row">
                                                    <div class="col-md-12">
                                                        <div id="Div1" runat="server" class="box box-info collapsed-box">
                                                            <div class="box-header with-border">
                                                                <h3 class="box-title">
                                                                    <asp:Label ID="Label2" runat="server" Text="Advance Search"></asp:Label></h3>
                                                                &nbsp;&nbsp;|&nbsp;&nbsp;
                                                <asp:Label ID="lblTotalTeamRecords" Font-Bold="true" runat="server" Text="<%$ Resources:Attendance,Total Records: 0 %>"></asp:Label>
                                                                <div class="box-tools pull-right">
                                                                    <button type="button" class="btn btn-box-tool" data-widget="collapse">
                                                                        <i id="I2" runat="server" class="fa fa-plus"></i>
                                                                    </button>
                                                                </div>
                                                            </div>
                                                            <div class="box-body">
                                                                <div class="form-group">
                                                                    <div class="row">
                                                                        <div class="form-group">

                                                                            <div class="col-lg-3">
                                                                                <asp:DropDownList ID="ddlFieldName1" runat="server" CssClass="form-control">
                                                                                    <asp:ListItem Text="<%$ Resources:Attendance, Employee Code %>" Value="Emp_Code"
                                                                                        Selected="True" />
                                                                                    <asp:ListItem Text="<%$ Resources:Attendance, Employee Name %>" Value="Emp_Name" />
                                                                                    <asp:ListItem Text="Task assigner" Value="Task_Visibility" />
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
                                                                                <asp:Panel ID="Panel2" runat="server" DefaultButton="imgbtnTeam">
                                                                                    <asp:TextBox ID="txtValueteam" runat="server" CssClass="form-control"></asp:TextBox>
                                                                                </asp:Panel>
                                                                            </div>
                                                                            <div class="col-lg-2">
                                                                                <asp:ImageButton ID="imgbtnTeam" runat="server" CausesValidation="False" Style="margin-top: -5px;"
                                                                                    ImageUrl="~/Images/search.png" OnClick="btnbindrpt_Click1" ToolTip="<%$ Resources:Attendance,Search %>"></asp:ImageButton>
                                                                                <asp:ImageButton ID="imgRefTeam" runat="server" CausesValidation="False" Style="width: 33px;"
                                                                                    ImageUrl="~/Images/refresh.png" ToolTip="<%$ Resources:Attendance,Refresh %>"
                                                                                    OnClick="btnRefresh_Click1"></asp:ImageButton>
                                                                            </div>

                                                                        </div>
                                                                    </div>
                                                                </div>
                                                            </div>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="col-md-12">
                                                <div class="flow">
                                                    <asp:HiddenField ID="hdnCanEdit" runat="server" />
                                                    <asp:HiddenField ID="hdnCanDelete" runat="server" />
                                                    <asp:GridView CssClass="table-striped table-bordered table table-hover" ID="grvteamlistDetailrecord" runat="server" AllowSorting="true" AutoGenerateColumns="False" Width="100%" PageSize="<%# PageControlCommon.GetPageSize() %>"
                                                        OnPageIndexChanging="grvteamlistDetailrecord_PageIndexChanging">
                                                        <Columns>
                                                            <asp:TemplateField HeaderText="Action">
                                                                <ItemTemplate>


                                                                    <div class="dropdown" style="position: absolute;">
                                                                        <button class="btn btn-default dropdown-toggle"  type="button" data-toggle="dropdown">
                                                                            <i class="fa fa-ellipsis-h" aria-hidden="true"></i>
                                                                        </button>
                                                                        <ul class="dropdown-menu">
                                                                            <li <%= hdnCanEdit.Value=="true"?"style='display:block'":"style='display:none'" %>>
                                                                                <asp:LinkButton ID="btnEditGrid" runat="server" CommandArgument='<%# Eval("Trans_Id") %>' OnCommand="btnEditGrid_Command" CausesValidation="False"><i class="fa fa-pencil"></i>Edit</asp:LinkButton>
                                                                            </li>
                                                                            <li <%= hdnCanDelete.Value=="true"?"style='display:block'":"style='display:none'" %>>
                                                                                <asp:LinkButton ID="btnDelete" runat="server" CommandArgument='<%# Eval("Emp_Id") %>' CommandName='<%# Eval("Trans_Id") %>' OnCommand="btnDeleteGrid_Command" CausesValidation="False"><i class="fa fa-trash"></i>Delete</asp:LinkButton>
                                                                            </li>
                                                                        </ul>
                                                                    </div>

                                                                </ItemTemplate>
                                                            </asp:TemplateField>

                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,Employee Code %>">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblEmpIdList1" runat="server" Text='<%# Eval("Emp_Code") %>'></asp:Label>
                                                                </ItemTemplate>
                                                                <ItemStyle  />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,Employee Name %>">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblEmpIdList" runat="server" Text='<%# Eval("EmpName_Designation") %>'></asp:Label>
                                                                    <asp:HiddenField ID="hdnempid" runat="server" Value='<%# Eval("Emp_Id") %>' />
                                                                    <asp:HiddenField ID="hdntrans" runat="server" Value='<%# Eval("Trans_Id") %>' />
                                                                    <asp:HiddenField ID="hdnprojectid" runat="server" Value='<%# Eval("Project_Id") %>' />
                                                                </ItemTemplate>
                                                                <ItemStyle  />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,Designation %>">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblEmpIdList2" runat="server" Text='<%# Eval("Designation") %>'></asp:Label>
                                                                    <asp:Label ID="lblProjectManager" runat="server" Text='<%# Eval("Field1") %>' Visible="false"></asp:Label>
                                                                </ItemTemplate>
                                                                <ItemStyle  />
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
                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,On Duty Time %>">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblOndutytime" runat="server" Text='<%# Eval("Field2") %>'></asp:Label>
                                                                </ItemTemplate>
                                                                <ItemStyle  />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,Off Duty Time %>">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblOffdutytime" runat="server" Text='<%# Eval("Field3") %>'></asp:Label>
                                                                </ItemTemplate>
                                                                <ItemStyle  />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,Basic Salary%>" Visible="false">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblBasicsalary" runat="server" Text='<%# Eval("Field4") %>'></asp:Label>
                                                                </ItemTemplate>
                                                                <ItemStyle  />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Eligible for Incentive">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblIncentive" runat="server" Text='<%# Eval("Field5").ToString().ToLower()=="true"?"True":"False" %>'></asp:Label>
                                                                </ItemTemplate>
                                                                <ItemStyle  />
                                                            </asp:TemplateField>

                                                        </Columns>
                                                        <PagerStyle CssClass="pagination-ys" />
                                                    </asp:GridView>
                                                    <asp:HiddenField ID="HdfSortDetail" runat="server" />
                                                    <asp:HiddenField ID="hidTransId" runat="server" />
                                                    <asp:HiddenField ID="hidProId" runat="server" />
                                                    <asp:HiddenField ID="hdnemp" runat="server" />
                                                    <asp:HiddenField ID="hidemployeeid" runat="server" />
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                                <%--</div>--%>
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

    <asp:Panel ID="PanelList" runat="server" Visible="false"></asp:Panel>
    <asp:Panel ID="pnlnew" runat="server" Visible="false"></asp:Panel>
    <asp:Panel ID="pnllist" runat="server" Visible="false"></asp:Panel>
    <asp:Panel ID="pnlteamdetials" runat="server" Visible="false"></asp:Panel>
    <asp:Panel ID="pnlgrid" runat="server" Visible="false"></asp:Panel>

</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="FooterContent" runat="Server">
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="AfterLoadScriptContent" runat="Server"><script src="../Script/common.js"></script>
    <script type="text/javascript">
        function resetPosition(object, args) {
            $(object._completionListElement.children).each(function () {
                var data = $(this)[0];
                if (data != null) {
                    data.style.paddingLeft = "10px";
                    data.style.cursor = "pointer";
                    data.style.borderBottom = "solid 1px #e7e7e7";
                }
            });
            object._completionListElement.className = "scrollbar scrollbar-primary force-overflow";
            var tb = object._element;
            var tbposition = findPositionWithScrolling(tb);
            var xposition = tbposition[0] + 2;
            var yposition = tbposition[1] + 25;
            var ex = object._completionListElement;
            if (ex)
                $common.setLocation(ex, new Sys.UI.Point(xposition, yposition));
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
        function Li_Tab_List() {
            document.getElementById('<%= Btn_List.ClientID %>').click();
        }
        function Li_Tab_New() {
            document.getElementById('<%= Btn_New.ClientID %>').click();
        }
    </script>
    <script src="../Script/common.js"></script>
</asp:Content>
