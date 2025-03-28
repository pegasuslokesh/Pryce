<%@ Page Title="" Language="C#" MasterPageFile="~/ERPMaster.master" MaintainScrollPositionOnPostback="true" AutoEventWireup="true" EnableEventValidation="false" CodeFile="EmployeeMaster.aspx.cs" Inherits="MasterSetUp_EmployeeMaster" %>

<%@ Register Src="~/WebUserControl/TimeManLicense.ascx" TagPrefix="uc1" TagName="UpdateLicense" %>
<%@ Register Assembly="DevExpress.Web.v18.1, Version=18.1.3.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web" TagPrefix="dx" %>
<%@ Register Src="~/WebUserControl/AddressControl.ascx" TagName="Addressmaster" TagPrefix="UC" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Src="~/WebUserControl/FileUpload.ascx" TagPrefix="AT1" TagName="FileUpload1" %>
<%@ Register Src="~/WebUserControl/ucControlsSetting.ascx" TagName="ucCtlSetting" TagPrefix="UC" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Head" runat="Server">
    <script type="text/javascript">
        function SetOpeningBalance() {
            var DrAmt = $("#<%=txtOpeningDebitAmt.ClientID%>").val();
            var CrAmt = $("#<%=txtOpeningCreditAmt.ClientID%>").val();
            if (DrAmt > 0) {
                $("#<%=txtOpeningCreditAmt.ClientID%>").val("0");
            }
            else if (CrAmt > 0) {
                $("#<%=txtOpeningDebitAmt.ClientID%>").val("0");
            }
        }
        function EditOpeningBalance() {
            var DrAmt = $("#<%=txtEditOpeningDebitAmt.ClientID%>").val();
        var CrAmt = $("#<%=txtEditOpeningCreditAmt.ClientID%>").val();
        if (DrAmt > 0) {
            $("#<%=txtEditOpeningCreditAmt.ClientID%>").val("0");
        }
        else if (CrAmt > 0) {
            $("#<%=txtEditOpeningDebitAmt.ClientID%>").val("0");
            }

        }
        function resetPosition1() {

        }
        function resetPosition_popup() {

        }

    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="HeaderContent" runat="Server">

    <h1>

        <i class="fa fa-user"></i>&nbsp;&nbsp;
        <asp:Label ID="lblHeader" runat="server" Text="<%$ Resources:Attendance,Employee Setup%>"></asp:Label>
    </h1>
    <ol class="breadcrumb">
        <li><a href="#"><i class="fa fa-dashboard"></i>
            <asp:Label ID="Label6" runat="server" Text="<%$ Resources:Attendance,Master Setup%>"></asp:Label></a></li>
        <li><a href="#"><i class="fa fa-dashboard"></i>
            <asp:Label ID="Label10" runat="server" Text="<%$ Resources:Attendance,Master SetUp%>"></asp:Label></a></li>
        <li class="active">
            <asp:Label ID="Label5" runat="server" Text="<%$ Resources:Attendance,Employee Setup%>"></asp:Label></li>
    </ol>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="Server">

    <asp:UpdatePanel ID="Update_Button" runat="server">
        <ContentTemplate>
            <asp:Button ID="Btn_List" Style="display: none;" runat="server" OnClick="btnList_Click" Text="List" />
            <asp:Button ID="Btn_New" Style="display: none;" runat="server" OnClick="btnNew_Click" Text="New" />
            <asp:Button ID="Btn_Assign" Style="display: none;" runat="server" OnClick="btnAssign_Click" Text="Assign" />
            <asp:Button ID="Btn_Terminated" Style="display: none;" runat="server" OnClick="btnBin_Click" Text="Terminated" />
            <asp:Button ID="Btn_Leave" Style="display: none;" runat="server" OnClick="btnLeave_Click" Text="Leave" />

            <asp:Button ID="Btn_Alert" Style="display: none;" runat="server" OnClick="btnNotice_Click" Text="Alert" />
            <asp:Button ID="Btn_Salary" Style="display: none;" runat="server" OnClick="btnSalary_Click" Text="Salary" />
            <asp:Button ID="Btn_Ot_PL" Style="display: none;" runat="server" OnClick="btnOTPartial_Click" Text="OT/PL" />
            <asp:Button ID="Btn_Upload" Style="display: none;" runat="server" OnClick="btnUpload_Click" Text="Upload" />
            <asp:Button ID="Btn_Penalty" Style="display: none;" runat="server" OnClick="btnPanelty_Click" Text="Penalty" />
            <asp:Button ID="Btn_Help_New" Style="display: none;" runat="server" OnClick="btnHelp_Click" Text="Help" />

            <asp:Button ID="Btn_Address_Modal" Style="display: none;" runat="server" data-toggle="modal" data-target="#Modal_Address" Text="Address" />
            <asp:Button ID="Btn_Leave_Modal" Style="display: none;" runat="server" data-toggle="modal" data-target="#Modal_Leave" Text="Leave" />
            <asp:Button ID="Btn_Leave_View_Modal" Style="display: none;" runat="server" data-toggle="modal" data-target="#Modal_Leave_View" Text="Leave View" />
            <asp:Button ID="Btn_Alert_Modal" Style="display: none;" runat="server" data-toggle="modal" data-target="#Modal_Alert" Text="Alert" />
            <asp:Button ID="Btn_Salary_Modal" Style="display: none;" runat="server" data-toggle="modal" data-target="#Modal_Salary" Text="Salary" />
            <asp:Button ID="Btn_Hierarchy_Modal" Style="display: none;" runat="server" data-toggle="modal" data-target="#Modal_Hierarchy" Text="Hierarchy" />
            <asp:Button ID="Btn_OT_PL_Modal" Style="display: none;" runat="server" data-toggle="modal" data-target="#Modal_OT_PL" Text="OT PL" />
            <asp:Button ID="Btn_Penalty_Modal" Style="display: none;" runat="server" data-toggle="modal" data-target="#Modal_Penalty" Text="Penalty" />
            <asp:Button ID="Btn_Modal_FileUpload" Style="display: none;" runat="server" data-toggle="modal" data-target="#Fileupload123" Text="FileUpload" />
            <asp:Button ID="Btn_Number" Style="display: none;" data-toggle="modal" data-target="#NumberData" runat="server" Text="Number Data" />

            <asp:HiddenField runat="server" ID="hdnCanEdit" />
            <asp:HiddenField runat="server" ID="hdnCanDelete" />
            <asp:HiddenField runat="server" ID="hdnCanUpload" />

        </ContentTemplate>
    </asp:UpdatePanel>
    <div class="row">
        <div class="col-md-12">
            <div class="nav-tabs-custom">
                <ul class="nav nav-tabs pull-right bg-blue-gradient">
                    <li id="Li_Help" runat="server"><a onclick="Li_Tab_Help()" href="#ctl00_MainContent_Help" data-toggle="tab">
                        <i class="fas fa-info"></i>&nbsp;
                        <asp:Label ID="Label126" runat="server" Text="<%$ Resources:Attendance,Help%>"></asp:Label></a></li>
                    <li id="Li_Penalty" runat="server"><a onclick="Li_Tab_Penalty()" href="#ctl00_MainContent_Penalty" data-toggle="tab">
                        <i class="fas fa-ban"></i>&nbsp;
                        <asp:Label ID="Label124" runat="server" Text="<%$ Resources:Attendance,Penalty%>"></asp:Label></a></li>
                    <li id="Li_Upload" runat="server"><a onclick="Li_Tab_Upload()" href="#ctl00_MainContent_Upload" data-toggle="tab">
                        <i class="fa fa-upload"></i>&nbsp;<asp:Label ID="Label123" runat="server" Text="<%$ Resources:Attendance,Upload%>"></asp:Label></a></li>
                    <li id="Li_OT_PL" runat="server"><a onclick="Li_Tab_OT_PL()" href="#ctl00_MainContent_OT_PL" data-toggle="tab">
                        <i class="fa fa-hourglass"></i>&nbsp;
                        <asp:Label ID="Label122" runat="server" Text="<%$ Resources:Attendance,OT/PL%>"></asp:Label></a></li>
                    <li id="Li_Salary" runat="server"><a onclick="Li_Tab_Salary()" href="#ctl00_MainContent_Salary" data-toggle="tab">
                        <i class="fas fa-money-check-alt"></i>&nbsp;
                        <asp:Label ID="Label112" runat="server" Text="<%$ Resources:Attendance,Salary%>"></asp:Label></a></li>
                    <li id="Li_Alert" runat="server"><a onclick="Li_Tab_Alert()" href="#ctl00_MainContent_Alert" data-toggle="tab">
                        <i class="fas fa-exclamation-triangle"></i>&nbsp;
                        <asp:Label ID="Label163" runat="server" Text="<%$ Resources:Attendance,Alert%>"></asp:Label></a></li>
                    <li id="Li_Leave" runat="server"><a onclick="Li_Tab_Leave()" href="#ctl00_MainContent_Leave" data-toggle="tab">
                        <i class="far fa-calendar-alt"></i>&nbsp;
                        <asp:Label ID="Label2" runat="server" Text="<%$ Resources:Attendance,Leave%>"></asp:Label></a></li>
                    <li id="Li_Terminated"><a onclick="Li_Tab_Terminated()" href="#Terminated" data-toggle="tab">
                        <i class="fas fa-user-slash"></i>&nbsp;
                        <asp:Label ID="Label110" runat="server" Text="<%$ Resources:Attendance,Terminated%>"></asp:Label></a></li>
                    <li id="Li_Assign" runat="server"><a onclick="Li_Tab_Assign()" href="#ctl00_MainContent_Assign" data-toggle="tab">
                        <i class="fa fa-file"></i>&nbsp;<asp:Label ID="lblAssignDepmnt" runat="server" Text="Assign" /></a></li>
                    <li id="Li_New" class="active"><a onclick="Li_Tab_New()" href="#New" data-toggle="tab">
                        <asp:UpdatePanel ID="Update_Tab_Name" runat="server">
                            <ContentTemplate>
                                <i class="fa fa-file"></i>&nbsp;
                                <asp:Label ID="Lbl_Tab_Name" runat="server" Text="<%$ Resources:Attendance,New%>"></asp:Label>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </a></li>
                    <li id="Li_List"><a onclick="Li_Tab_List()" href="#List" data-toggle="tab">
                        <i class="fa fa-list"></i>&nbsp;<asp:Label ID="Label165" runat="server" Text="<%$ Resources:Attendance,List %>"></asp:Label></a></li>
                </ul>
                <div class="tab-content">
                    <div class="tab-pane" id="List">
                        <asp:UpdatePanel ID="Update_List" runat="server">
                            <Triggers>
                                <asp:PostBackTrigger ControlID="btnExport" />
                                <asp:PostBackTrigger ControlID="btnEmpexport" />
                            </Triggers>
                            <ContentTemplate>
                                <div class="row">
                                    <div class="col-md-12">
                                        <div id="Div2" runat="server" class="box box-info collapsed-box">
                                            <div class="box-header with-border">
                                                <h3 class="box-title">
                                                    <asp:Label ID="Label3" runat="server" Text="<%$ Resources:Attendance,Advance Search%>"></asp:Label></h3>
                                                &nbsp;&nbsp;|&nbsp;&nbsp;
					<asp:Label ID="lblTotalRecord" Text="<%$ Resources:Attendance,Total Records: 0 %>" runat="server"></asp:Label>

                                                <div class="box-tools pull-right">
                                                    <button type="button" class="btn btn-box-tool" data-widget="collapse">
                                                        <i id="I1" runat="server" class="fa fa-plus"></i>
                                                    </button>
                                                </div>
                                            </div>
                                            <div class="box-body">
                                                <div class="col-md-6">
                                                    <asp:HiddenField ID="txtconformmessageValue" runat="server" />
                                                    <asp:Label ID="lblLocationList" runat="server" Text="<%$ Resources:Attendance,Location %>"></asp:Label>
                                                    <asp:DropDownList ID="ddlLocationList" runat="server" CssClass="form-control" AutoPostBack="true"
                                                        OnSelectedIndexChanged="ddlLocationList_SelectedIndexChanged">
                                                    </asp:DropDownList>
                                                    <br />
                                                </div>

                                                <div class="col-md-6">
                                                    <asp:Label ID="lblDeptList" runat="server" Text="<%$ Resources:Attendance,Department %>"></asp:Label>
                                                    <asp:DropDownList ID="ddlDeptList" runat="server" CssClass="form-control" AutoPostBack="true"
                                                        OnSelectedIndexChanged="ddlDeptList_SelectedIndexChanged">
                                                    </asp:DropDownList>
                                                    <br />
                                                </div>


                                                <div class="col-lg-2">
                                                    <asp:DropDownList ID="ddlempTypeFilter" runat="server" CssClass="form-control" AutoPostBack="true" OnSelectedIndexChanged="ddlempTypeFilter_OnSelectedIndexChanged">
                                                        <asp:ListItem Text="<%$ Resources:Attendance,All %>" Value="0" Selected="True"></asp:ListItem>
                                                        <asp:ListItem Text="<%$ Resources:Attendance,On Role %>" Value="On Role"></asp:ListItem>
                                                        <asp:ListItem Text="<%$ Resources:Attendance,Off Role %>" Value="Off Role"></asp:ListItem>

                                                    </asp:DropDownList>
                                                </div>


                                                <div class="col-lg-2">
                                                    <asp:DropDownList ID="ddlFieldName" runat="server" CssClass="form-control">
                                                        <asp:ListItem Text="<%$ Resources:Attendance,Employee Code %>" Value="Emp_Code" Selected="True"></asp:ListItem>
                                                        <asp:ListItem Text="<%$ Resources:Attendance,Employee Name %>" Value="Emp_Name"></asp:ListItem>
                                                        <asp:ListItem Text="<%$ Resources:Attendance,Employee Name Local %>" Value="Emp_Name_L"></asp:ListItem>
                                                        <asp:ListItem Text="<%$ Resources:Attendance,Email Id %>" Value="Email_Id"></asp:ListItem>
                                                        <asp:ListItem Text="<%$ Resources:Attendance,Mobile No %>" Value="Phone_No"></asp:ListItem>
                                                        <asp:ListItem Text="<%$ Resources:Attendance,Designation%>" Value="Designation"></asp:ListItem>
                                                        <asp:ListItem Text="<%$ Resources:Attendance,Civil Id %>" Value="Civil_Id"></asp:ListItem>

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
                                                <div class="col-lg-3">
                                                    <asp:Panel ID="Panel1" runat="server" DefaultButton="btnbind">
                                                        <asp:TextBox placeholder="Search from Content" ID="txtValue" CausesValidation="True" runat="server" CssClass="form-control" />
                                                        <asp:RequiredFieldValidator EnableClientScript="true" runat="server" ID="reqName" ValidationGroup="R_Search" Display="Dynamic" SetFocusOnError="true" ControlToValidate="txtValue" ErrorMessage="<%$ Resources:Attendance,Please enter value %>" />
                                                    </asp:Panel>
                                                </div>
                                                <div class="col-lg-3" style="text-align: center; padding-left: 1px; padding-right: 1px;">

                                                    <asp:LinkButton ID="btnbind" runat="server" CausesValidation="true" OnClick="btnbind_Click" ToolTip="<%$ Resources:Attendance,Search %>"><span class="fa fa-search"  style="font-size:25px;"></span></asp:LinkButton>&nbsp;&nbsp;&nbsp;
                                                    <asp:LinkButton ID="btnRefresh" runat="server" CausesValidation="False" OnClick="btnRefresh_Click" ToolTip="<%$ Resources:Attendance,Refresh %>"><span class="fa fa-repeat"  style="font-size:25px;"></span></asp:LinkButton>&nbsp;&nbsp;&nbsp;

                                                    <asp:LinkButton ID="btnExport" runat="server" CommandArgument="2" CommandName="OP" OnCommand="btnExport_Click" ToolTip="Update Employee Information" Visible="false"><span class="fas fa-file-csv"  style="font-size:25px;"></span></asp:LinkButton>&nbsp;&nbsp;&nbsp;
                                                    <asp:LinkButton ID="imbBtnGrid" CausesValidation="False" runat="server" OnClick="imbBtnGrid_Click" ToolTip="<%$ Resources:Attendance, Grid View %>"><span class="fas fa-table"  style="font-size:25px;"></span></asp:LinkButton>&nbsp;&nbsp;&nbsp;
                                                    <asp:LinkButton ID="imgBtnDatalist" CausesValidation="False" runat="server" OnClick="imgBtnDatalist_Click" Visible="False" ToolTip="<%$ Resources:Attendance,List View %>"><span class="fas fa-list"  style="font-size:25px;"></span></asp:LinkButton>&nbsp;&nbsp;&nbsp;
                                                    <asp:LinkButton ID="btnTreeView" runat="server" CausesValidation="False" ToolTip="Employee Hierarchy" OnClick="btnTreeView_Click"><span class="fa fa-sitemap"  style="font-size:25px;"></span></asp:LinkButton>&nbsp;&nbsp;&nbsp;
                                                    <asp:LinkButton ID="btnEmpexport" Visible="false" runat="server" ToolTip="Download Employee Information" OnClick="btnEmpexport_Click"><span class="fas fa-file-csv"  style="font-size:25px;"></span></asp:LinkButton>
                                                    <asp:LinkButton ID="btnGvListSetting" ImageAlign="Right" ToolTip="List Settings" runat="server" OnClick="btnGvListSetting_Click" Visible="false"><span class="fa fa-wrench"  style="font-size:25px;"></span></asp:LinkButton>
                                                </div>

                                                <asp:HiddenField ID="HiddenField1" runat="server" />

                                            </div>
                                        </div>
                                    </div>
                                </div>

                                <div class="box box-warning box-solid" <%= gvEmp.Rows.Count>0?"style='display:block'":"style='display:none'"%>>
                                    <div class="box-body">
                                        <div class="row">
                                            <div class="col-md-12">
                                                <div class="flow">
                                                    <asp:HiddenField ID="hdnProductId" runat="server" />
                                                    <asp:HiddenField ID="Hdn_Edit" runat="server" />
                                                    <asp:GridView CssClass="table-striped table-bordered table table-hover" ID="gvEmp" PageSize="<%# PageControlCommon.GetPageSize() %>"
                                                        runat="server" AutoGenerateColumns="False" Width="100%" AllowPaging="True" AllowSorting="true"
                                                        OnPageIndexChanging="gvEmp_PageIndexChanging" OnSorting="gvEmp_Sorting" DataKeyNames="Emp_Id" Visible="false">
                                                        <Columns>

                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,Action%>">
                                                                <ItemTemplate>
                                                                    <div class="dropdown" style="position: absolute;">
                                                                        <button class="btn btn-default dropdown-toggle" type="button" data-toggle="dropdown">
                                                                            <i class="fa fa-ellipsis-h" aria-hidden="true"></i>
                                                                        </button>
                                                                        <ul class="dropdown-menu">

                                                                            <li <%= hdnCanEdit.Value=="true"?"style='display:block'":"style='display:none'" %>>
                                                                                <asp:LinkButton ID="btnEdit" runat="server" CausesValidation="False" CommandArgument='<%# Eval("Emp_Id") %>' OnCommand="btnEdit_Command"><i class="fa fa-pencil"></i><%# Resources.Attendance.Edit%> </asp:LinkButton>
                                                                            </li>
                                                                            <li <%= hdnCanDelete.Value=="true"?"style='display:block'":"style='display:none'" %>>
                                                                                <asp:LinkButton ID="btnDelete" runat="server" CausesValidation="False" CommandArgument='<%# Eval("Emp_Id") %>' OnCommand="btnDelete_Command"><i class="fa fa-trash"></i><%# Resources.Attendance.Delete%></asp:LinkButton>
                                                                                <cc1:ConfirmButtonExtender ID="ConfirmButtonExtender1" runat="server" ConfirmText="Are you sure to delete this Employee ?" TargetControlID="btnDelete"></cc1:ConfirmButtonExtender>
                                                                            </li>

                                                                            <li <%= hdnCanUpload.Value=="true"?"style='display:block'":"style='display:none'" %>>
                                                                                <asp:LinkButton ID="btnFileUpload" runat="server" CommandArgument='<%# Eval("Emp_Id") %>' CommandName='<%# Eval("Emp_Code") %>' OnCommand="btnFileUpload_Command" CausesValidation="False"><i class="fa fa-upload"></i><%# Resources.Attendance.File_Upload%></asp:LinkButton>
                                                                            </li>
                                                                        </ul>
                                                                    </div>

                                                                </ItemTemplate>
                                                            </asp:TemplateField>



                                                            <asp:TemplateField HeaderText="Code" SortExpression="Emp_Code">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblEmpCode" runat="server" Text='<%# Eval("Emp_Code") %>'></asp:Label>
                                                                    <asp:Label ID="lblempName" runat="server" Text='<%# Eval("Emp_Name") %>' Visible="false"></asp:Label>
                                                                </ItemTemplate>

                                                            </asp:TemplateField>
                                                            <asp:BoundField DataField="Emp_Name" HeaderText="<%$ Resources:Attendance,Employee Name %>"
                                                                SortExpression="Emp_Name" ItemStyle-Width="15%" />

                                                            <asp:BoundField DataField="Emp_Name_L" HeaderText="Employee Local Name"
                                                                SortExpression="Emp_Name_L" ItemStyle-Width="15%" />

                                                            <asp:TemplateField HeaderText="Doj" SortExpression="Doj">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblDoj" runat="server" Text='<%#getdate(Eval("Doj").ToString()) %>'></asp:Label>
                                                                </ItemTemplate>

                                                            </asp:TemplateField>
                                                            <asp:BoundField DataField="Designation" HeaderText="<%$ Resources:Attendance,Designation %>"
                                                                SortExpression="Designation" ItemStyle-Width="15%" />
                                                            <asp:BoundField DataField="Department" HeaderText="<%$ Resources:Attendance,Department %>"
                                                                SortExpression="Department" ItemStyle-Width="20%" />

                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,Email Id %>" SortExpression="Email_Id">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblProductCode" runat="server" Text='<%# Eval("Email_Id") %>'></asp:Label>
                                                                </ItemTemplate>

                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,Phone No. %>" SortExpression="Phone_No">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblItemType" runat="server" Text='<%# Eval("Phone_No") %>'></asp:Label>
                                                                </ItemTemplate>

                                                            </asp:TemplateField>
                                                        </Columns>
                                                        <PagerStyle CssClass="pagination-ys" />
                                                    </asp:GridView>
                                                    <asp:HiddenField ID="HDFSort" runat="server" />
                                                </div>
                                            </div>

                                            <div class="col-md-12">
                                                <div class="flow">
                                                    <asp:Repeater ID="rptCustomers" runat="server" OnItemCommand="lnkEditCommand">
                                                        <ItemTemplate>
                                                            <div class="col-md-4">
                                                                <div class="box box-primary">
                                                                    <div class="box-header with-border">
                                                                        <h4 class="products-list product-list-in-box">
                                                                            <span class="product-description">
                                                                                <asp:LinkButton
                                                                                    ID="lnkEmpname"
                                                                                    runat="server"
                                                                                    CommandName="cmd"
                                                                                    CommandArgument='<%# Eval("Emp_Id") %>' ToolTip='<%# GetEmployeeName(Eval("Emp_Id"))%>'
                                                                                    Text='<%# GetEmployeeName(Eval("Emp_Id"))%>'>
                                                                                </asp:LinkButton></span></h4>
                                                                    </div>
                                                                    <div class="box-body">
                                                                        <ul class="products-list product-list-in-box">
                                                                            <li class="item">
                                                                                <div class="product-img">
                                                                                    <img src='<%#getImageByEmpId( Eval("Emp_Id")) %>' style="width: 90px; height: 120px;" />
                                                                                </div>
                                                                                <div style="padding-left: 40px;" class="product-info">
                                                                                    <div class="product-description">
                                                                                        <span class="visible-lg visible-md" style="display: inline; float: left">Emp Code : </span>
                                                                                        <span style="display: inline;">
                                                                                            <asp:Label ID="Label2" Style="margin-left: 5px;" runat="server" Text='<%# Eval("Emp_Code") %>'></asp:Label></span>
                                                                                    </div>

                                                                                    <div class="product-description">
                                                                                        <span class="visible-lg visible-md" style="display: inline; float: left">Email : </span>
                                                                                        <span style="display: inline;">
                                                                                            <asp:Label ID="Label156" Style="margin-left: 5px;" runat="server" Text='<%# getEmailId(Eval("Emp_Id")) %>'></asp:Label></span>
                                                                                    </div>

                                                                                    <div class="product-description">
                                                                                        <span class="visible-lg visible-md" style="display: inline; float: left">Designation : </span>
                                                                                        <span style="display: inline;">
                                                                                            <asp:Label ID="Label157" Style="margin-left: 5px;" runat="server" ToolTip='<%# getdesg(Eval("Emp_Id")) %>' Text='<%# getdesg(Eval("Emp_Id")) %>'></asp:Label></span>
                                                                                    </div>

                                                                                    <div class="product-description">
                                                                                        <span class="visible-lg visible-md" style="display: inline; float: left">Department : </span>
                                                                                        <span style="display: inline;">
                                                                                            <asp:Label ID="Label158" Style="margin-left: 5px;" runat="server" ToolTip='<%# getdepartment(Eval("Emp_Id")) %>' Text='<%# getdepartment(Eval("Emp_Id")) %>'></asp:Label></span>
                                                                                    </div>

                                                                                    <div class="product-description">
                                                                                        <span class="visible-lg visible-md" style="display: inline; float: left">DOB : </span>
                                                                                        <span style="display: inline;">
                                                                                            <asp:Label ID="Label159" Style="margin-left: 5px;" runat="server" Text='<%# getdate(Eval("DOB")) %>'></asp:Label></span>
                                                                                    </div>

                                                                                    <div class="product-description">
                                                                                        <span class="visible-lg visible-md" style="display: inline; float: left">DOJ : </span>
                                                                                        <span style="display: inline;">
                                                                                            <asp:Label ID="Label160" Style="margin-left: 5px;" runat="server" Text='<%# getdate(Eval("DOJ")) %>'></asp:Label></span>
                                                                                    </div>

                                                                                    <div class="product-description">
                                                                                        <span class="visible-lg visible-md" style="display: inline; float: left">Mobile : </span>
                                                                                        <span style="display: inline;">
                                                                                            <asp:Label ID="Label161" Style="margin-left: 5px;" runat="server" Text='<%#getMobileNo( Eval("Emp_Id")) %>'></asp:Label></span>
                                                                                    </div>
                                                                                </div>
                                                                            </li>
                                                                        </ul>
                                                                    </div>
                                                                </div>
                                                            </div>
                                                        </ItemTemplate>
                                                    </asp:Repeater>
                                                </div>
                                            </div>
                                            <div id="div_Paging" runat="server" class="col-md-12" style="text-align: center">
                                                <asp:Button ID="Btn_First" Text="<<" Style="margin-left: 7px; margin-right: 7px;" OnClick="Btn_First_Click" runat="server" CssClass="btn btn-info" />

                                                <asp:Button ID="Btn_Previous" Style="margin-left: 7px; margin-right: 7px;" Text="<" runat="server" CssClass="btn btn-info" OnClick="Btn_Previous_Click" />

                                                <asp:Label ID="Lbl_Page_Index" runat="server" Width="30px" Text="1" Font-Bold="true"></asp:Label>

                                                <asp:Button ID="btn_Next" Style="margin-left: 7px; margin-right: 7px;" runat="server" CssClass="btn btn-info" Text=">" OnClick="btn_Next_Click" />

                                                <asp:Button ID="Btn_Last" Style="margin-left: 7px; margin-right: 7px;" Text=">>" OnClick="Btn_Last_Click" runat="server" CssClass="btn btn-info" />
                                            </div>
                                        </div>
                                        <div runat="server" visible="false" class="col-md-12" style="text-align: center;">
                                            <asp:DataList ID="dtlistEmp" Visible="false" runat="server" RepeatColumns="3" RepeatDirection="Horizontal">
                                                <ItemTemplate>
                                                    <div class="col-md-4" style="width: 100%; min-width: 395px;">
                                                        <div class="box box-widget widget-user-2">
                                                            <!-- Add the bg color to the header using any of the bg-* classes -->
                                                            <div class="widget-user-header bg-yellow">
                                                                <div class="widget-user-image">
                                                                    <img id="ImgBtnEmp" runat="server" class="img-circle" src='<%#getImageByEmpId( Eval("Emp_Id")) %>' />
                                                                </div>
                                                                <!-- /.widget-user-image -->
                                                                <h4 class="widget-user-username">
                                                                    <asp:LinkButton ID="lnkEmpname11" runat="server" CausesValidation="false" ForeColor="#1886b9"
                                                                        Font-Bold="true" Enabled="false" CommandArgument='<%# Eval("Emp_Id") %>' OnCommand="lnkEditCommand"
                                                                        Text='<%# GetEmployeeName(Eval("Emp_Id"))%>'></asp:LinkButton></h4>
                                                                <h5 class="widget-user-desc">
                                                                    <asp:Label ID="lblEmalid" runat="server" Text='<%# getEmailId(Eval("Emp_Id")) %>'></asp:Label></h5>
                                                            </div>
                                                            <div class="box-footer no-padding">
                                                                <ul class="nav nav-stacked">
                                                                    <li>
                                                                        <asp:Label ID="lblGvEmpId" runat="server" Text='<%# Eval("Emp_Code") %>'></asp:Label></li>
                                                                    <li>
                                                                        <asp:Label ID="lblDesignation" runat="server" Text='<%# getdesg(Eval("Emp_Id")) %>'></asp:Label></li>
                                                                    <li>
                                                                        <asp:Label ID="lblDepartment" runat="server" Text='<%# getdepartment(Eval("Emp_Id")) %>'></asp:Label></li>
                                                                    <li>DOB :
                                                                                        <asp:Label ID="Label15" runat="server" Text='<%# getdate(Eval("DOB")) %>' /></li>
                                                                    <li>DOJ :
                                                                                        <asp:Label ID="Label16" runat="server" Text='<%# getdate(Eval("DOJ")) %>'></asp:Label></li>
                                                                    <li>
                                                                        <asp:Label ID="lblMobile" runat="server" Text='<%#getMobileNo( Eval("Emp_Id")) %>'></asp:Label></li>
                                                                </ul>
                                                            </div>
                                                        </div>
                                                    </div>

                                                </ItemTemplate>
                                            </asp:DataList>
                                            <asp:LinkButton ID="lnkFirst" runat="server" Font-Size="Small" Font-Bold="True" ForeColor="#1886b9"
                                                CausesValidation="False" OnClick="lnkFirst_Click" Style="margin-left: 10px; text-decoration: none">First</asp:LinkButton>
                                            <asp:LinkButton ID="lnkPrev" runat="server" Font-Bold="True" Font-Size="Small" ForeColor="#1886b9"
                                                CausesValidation="False" OnClick="lnkPrev_Click" Style="margin-left: 10px; text-decoration: none">Prev</asp:LinkButton>
                                            <asp:LinkButton ID="lnkNext" runat="server" Font-Size="Small" Font-Bold="True" ForeColor="#1886b9"
                                                CausesValidation="False" OnClick="lnkNext_Click" Style="margin-left: 10px; text-decoration: none">Next</asp:LinkButton>
                                            <asp:LinkButton ID="lnkLast" runat="server" Font-Bold="True" Font-Size="Small" ForeColor="#1886b9"
                                                CausesValidation="False" OnClick="lnkLast_Click" Style="margin-left: 10px; text-decoration: none">Last</asp:LinkButton>
                                        </div>
                                    </div>
                                </div>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </div>


                    <div class="modal fade" id="Fileupload123" tabindex="-1" role="dialog" aria-labelledby="myModalLabel"
                        aria-hidden="true">
                        <div class="modal-dialog modal-lg">
                            <div class="modal-content">
                                <div class="modal-body">
                                    <AT1:FileUpload1 runat="server" ID="FUpload1" />
                                </div>
                                <div class="modal-footer">
                                    <button type="button" id="" class="btn btn-danger" data-dismiss="modal">
                                        Close</button>
                                </div>
                            </div>
                        </div>
                    </div>

                    <div class="modal fade" id="NumberData" tabindex="-1" role="dialog" aria-labelledby="NumberData_ModalLabel" aria-hidden="true">
                        <div class="modal-dialog modal-mg">
                            <div class="modal-content">
                                <div class="modal-header">
                                    <button type="button" class="close" data-dismiss="modal">
                                        <span aria-hidden="true">&times;</span><span class="sr-only">Close</span></button>
                                    <h4 class="modal-title" id="myNumbers">All Number List:</h4>
                                </div>
                                <div class="modal-body">
                                    <div id="AllNumberData">
                                    </div>
                                </div>
                                <div class="modal-footer">
                                    <button type="button" id="" class="btn btn-danger" data-dismiss="modal">
                                        Close</button>
                                </div>
                            </div>
                        </div>
                    </div>





                    <dx:ASPxPopupControl ID="ASPxPopupControl1" ClientInstanceName="popup" runat="server" Height="500px" Width="1000px" Modal="true" PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter">
                        <ClientSideEvents Shown="function(s,e){
                    fileManager.AdjustControl();
                }" />

                        <ContentCollection>
                            <dx:PopupControlContentControl runat="server" SupportsDisabledAttribute="True">

                                <dx:ASPxFileManager ID="ASPxFileManager1" runat="server" ClientInstanceName="fileManager">
                                    <Settings AllowedFileExtensions=".jpg,.jpeg,.gif,.rtf,.txt,.avi,.png,.mp3,.xml,.doc,.pdf" EnableMultiSelect="false" />
                                    <SettingsEditing AllowCreate="true" AllowDelete="true" AllowMove="true" AllowRename="true" AllowCopy="true" AllowDownload="true" />
                                    <SettingsToolbar ShowDownloadButton="true" />
                                    <SettingsFileList View="Thumbnails">
                                        <ThumbnailsViewSettings ThumbnailSize="100" />
                                        <DetailsViewSettings AllowColumnResize="true" AllowColumnDragDrop="true" AllowColumnSort="true" ShowHeaderFilterButton="false">
                                            <Columns>
                                                <dx:FileManagerDetailsColumn FileInfoType="Thumbnail" />
                                                <dx:FileManagerDetailsColumn FileInfoType="FileName" />
                                                <dx:FileManagerDetailsColumn FileInfoType="LastWriteTime" />
                                                <dx:FileManagerDetailsColumn FileInfoType="Size" />
                                            </Columns>
                                        </DetailsViewSettings>
                                    </SettingsFileList>
                                    <SettingsUpload Enabled="true" />
                                    <ClientSideEvents SelectedFileOpened="function(s, e) {
    // Check if e.file and e.file.extension are defined
                                  debugger;
    if (e.file.name) {
       
        var fileExtension = e.file.name.substring(e.file.name.lastIndexOf('.') + 1);

        // Check if the file is an image (you can customize the list of allowed extensions)
        var isImage = ['jpg', 'jpeg', 'png', 'gif'].includes(fileExtension);


        if (!isImage) {
            alert('Invalid file type. Only images are allowed.');
            return false;
        }
    } else {
        // Handle the case where e.file or e.file.extension is undefined
        alert('Unable to determine file type.');
        return false;
    }

    var image = $('#imgLogo')[0];
    image.src = e.file.imageSrc;

    var subproduct = s.currentPath;
    if (subproduct != '') {
        subproduct = '/' + subproduct;
        subproduct = subproduct.replaceAll('\\', '/');
    }

    var dataa = '~/' + s.rootFolderName + subproduct + '/' + e.file.name;
    dataa = dataa.replaceAll('/', '//');

    // Call the server-side method only if it's an image
    PageMethods.ASPxFileManager1_SelectedFileOpened(dataa, e.file.name, function(data) {}, function(data) {});

    popup.Hide();
    return false;
}" />

                                </dx:ASPxFileManager>

                            </dx:PopupControlContentControl>
                        </ContentCollection>
                    </dx:ASPxPopupControl>





                    <div class="tab-pane active" id="New">
                        <asp:UpdatePanel ID="Update_New" runat="server">
                            <Triggers>
                                <%--  <asp:PostBackTrigger ControlID="TabContainer2" />--%>
                            </Triggers>
                            <ContentTemplate>
                                <div class="row">
                                    <div class="col-md-12">
                                        <div id="Div_Employee_Info" class="box box-primary">
                                            <div class="box-header with-border">
                                                <h3 class="box-title">Employee Information</h3>
                                                <div class="box-tools pull-right">
                                                    <button type="button" class="btn btn-box-tool" data-widget="collapse">
                                                        <i id="Btn_Div_Employee_Info" class="fa fa-minus"></i>
                                                    </button>
                                                </div>
                                            </div>
                                            <div class="box-body">
                                                <div class="form-group">
                                                    <div class="col-md-12">
                                                        <asp:ImageButton ID="btnControlsSetting" ImageAlign="Right" ToolTip="Controls Setting" runat="server" ImageUrl="~/Images/setting.png" OnClick="btnControlsSetting_Click" Style="width: 32px; height: 32px" Visible="false" />
                                                    </div>
                                                    <div class="col-md-12" style="text-align: center;">
                                                        <asp:Label ID="Label98" runat="server" Text="<%$ Resources:Attendance,Employee Level %>"></asp:Label>
                                                        <asp:RadioButton ID="rbtnEmployee" Style="padding-left: 10px; padding-right: 10px;" GroupName="Level" runat="server" Text="<%$ Resources:Attendance,Employee %>" />
                                                        <asp:RadioButton ID="rbtnManager" Style="padding-left: 10px; padding-right: 10px;" GroupName="Level" runat="server" Text="<%$ Resources:Attendance,Manager %>" />
                                                        <asp:RadioButton ID="rbtnCEO" Style="padding-left: 10px; padding-right: 10px;" GroupName="Level" runat="server" Text="<%$ Resources:Attendance,CEO %>" />
                                                        <br />
                                                        <br />
                                                    </div>
                                                    <div class="col-md-12">
                                                        <div class="col-md-6">
                                                            <asp:Label ID="lblEmployeeCode" runat="server" Text="<%$ Resources:Attendance,Employee Code %>"></asp:Label><a style="color: Red">*</a>
                                                            <asp:RequiredFieldValidator EnableClientScript="true" Style="float: right;" runat="server" ID="RequiredFieldValidator1" ValidationGroup="E_Save" Display="Dynamic" SetFocusOnError="true" ControlToValidate="txtEmployeeCode" ErrorMessage="<%$ Resources:Attendance,Enter Employee Code %>" />
                                                            <asp:TextBox ID="txtEmployeeCode" runat="server" CssClass="form-control"
                                                                AutoPostBack="true" OnTextChanged="txtEmployeeCode_TextChanged" BackColor="#eeeeee"
                                                                MaxLength="11" />


                                                            <cc1:AutoCompleteExtender ID="txtCompCode_AutoCompleteExtender" runat="server" DelimiterCharacters=""
                                                                Enabled="True" ServiceMethod="GetCompletionListEmployeeCode" ServicePath="" CompletionInterval="100"
                                                                MinimumPrefixLength="1" CompletionSetCount="1" TargetControlID="txtEmployeeCode"
                                                                UseContextKey="True" CompletionListCssClass="autoCompleteList" CompletionListItemCssClass="autoCompleteListItem" CompletionListHighlightedItemCssClass="autoCompleteSelectedListItem" OnClientShown="resetPosition">
                                                            </cc1:AutoCompleteExtender>
                                                            <cc1:FilteredTextBoxExtender ID="txtEmployeeCode_FilteredTextBoxExtender" runat="server"
                                                                Enabled="True" TargetControlID="txtEmployeeCode" FilterType="Numbers">
                                                            </cc1:FilteredTextBoxExtender>
                                                            <br />
                                                            <asp:Label ID="lblEmployeeName" runat="server" Text="<%$ Resources:Attendance,Employee Name %>"></asp:Label><a style="color: Red">*</a>
                                                            <asp:RequiredFieldValidator EnableClientScript="true" Style="float: right;" runat="server" ID="RequiredFieldValidator2" ValidationGroup="E_Save" Display="Dynamic" SetFocusOnError="true" ControlToValidate="txtEmployeeName" ErrorMessage="<%$ Resources:Attendance,Enter Employee Name %>" />
                                                            <asp:TextBox ID="txtEmployeeName"
                                                                runat="server" CssClass="form-control" />
                                                            <br />
                                                            <asp:Label ID="lblEmployeeL" runat="server" Text="<%$ Resources:Attendance,Employee Name(Local) %>"></asp:Label>
                                                            <asp:TextBox ID="txtEmployeeL" runat="server" CssClass="form-control" />
                                                            <br />
                                                        </div>
                                                        <div class="col-md-6">
                                                            <asp:Label ID="lblEmpLogo" runat="server" Text="<%$ Resources:Attendance,Image  %>"></asp:Label>
                                                            <div class="input-group" style="width: 100%; display: none;">
                                                                <cc1:AsyncFileUpload ID="FULogoPath"
                                                                    OnClientUploadStarted="FuLogo_UploadStarted"
                                                                    OnClientUploadError="FuLogo_UploadError"
                                                                    OnClientUploadComplete="FuLogo_UploadComplete"
                                                                    OnUploadedComplete="FuLogo_FileUploadComplete"
                                                                    runat="server" CssClass="form-control"
                                                                    CompleteBackColor="White"
                                                                    UploaderStyle="Traditional"
                                                                    UploadingBackColor="#CCFFFF"
                                                                    ThrobberID="FULogo_ImgLoader" Width="100%" />
                                                                <div class="input-group-btn" style="border: solid 1px #d2d6de; width: 35px;">
                                                                    <asp:LinkButton ID="FULogo_Img_Right" runat="server" Width="30px" Height="30px" Style="display: none"><i class="fa fa-check" style="font-size:30px;color:#22cb33"></i></asp:LinkButton>
                                                                    <asp:LinkButton ID="FULogo_Img_Wrong" runat="server" Width="30px" Height="30px" Style="display: none"><i class="fa fa-remove" style="font-size:30px"></i></asp:LinkButton>
                                                                    <asp:Image ID="FULogo_ImgLoader" runat="server" ImageUrl="../Images/loader.gif" />
                                                                </div>
                                                            </div>
                                                            <br />
                                                            <div style="text-align: center;">
                                                                <asp:Image ID="imgLogo" ClientIDMode="Static" ImageUrl="../Bootstrap_Files/dist/img/Bavatar.png" runat="server" Style="width: 90px; height: 100px;" />
                                                                <br />
                                                                <asp:Button ID="btnUpload" Visible="false" Style="margin-top: 15px;" runat="server" Text="<%$ Resources:Attendance,Load %>"
                                                                    CssClass="btn btn-primary" OnClick="btnUpload1_Click" />


                                                                <a onclick="popup.Show();" runat="server" id="File_Manager_Employee" class="btn btn-primary" style="cursor: pointer; margin-top: 15px;">File Manager</a>

                                                                <%--<dx:ASPxButton ID="dxBtnFileUpload" runat="server" Text="File Manager"  CssClass="btn btn-primary" AutoPostBack="false">
                                                                    <ClientSideEvents Click="function(s, e) { popup.Show(); }" />
                                                                </dx:ASPxButton>--%>
                                                            </div>
                                                        </div>
                                                    </div>
                                                    <div class="col-md-12">
                                                        <div class="col-md-6">
                                                            <asp:Label ID="Label22" runat="server" Text="<%$ Resources:Attendance,Gender %>"></asp:Label>
                                                            <asp:DropDownList ID="ddlGender" runat="server" CssClass="form-control">
                                                                <asp:ListItem Text="<%$ Resources:Attendance,Male %>" Value="M"></asp:ListItem>
                                                                <asp:ListItem Text="<%$ Resources:Attendance,Female %>" Value="F"></asp:ListItem>
                                                            </asp:DropDownList>
                                                            <br />
                                                        </div>
                                                        <div class="col-md-6">
                                                            <asp:Label ID="Label150" runat="server" Text="IsMarried"></asp:Label>
                                                            <asp:RadioButtonList ID="rblIsMarried" CssClass="form-control" runat="server" RepeatDirection="Horizontal" CellPadding="3" CellSpacing="2" RepeatColumns="2">
                                                                <asp:ListItem style="padding-left: 10px; padding-right: 10px;" Text="Yes" Value="True" Selected="True"></asp:ListItem>
                                                                <asp:ListItem style="padding-left: 10px; padding-right: 10px;" Text="No" Value="False"></asp:ListItem>

                                                            </asp:RadioButtonList>
                                                            <br />
                                                        </div>
                                                    </div>
                                                    <div class="col-md-12">
                                                        <div class="col-md-12">
                                                            <asp:Label ID="lblAddressName" runat="server" Text="<%$ Resources:Attendance,Address Name %>"></asp:Label><a style="color: Red">*</a>
                                                            <asp:RequiredFieldValidator EnableClientScript="true" Style="float: right;" runat="server" ID="RequiredFieldValidator25" ValidationGroup="Address"
                                                                Display="Dynamic" SetFocusOnError="true" ControlToValidate="txtAddressName" ErrorMessage="<%$ Resources:Attendance,Enter Address Name %>" />
                                                            <div class="input-group">
                                                                <asp:TextBox ID="txtAddressName" AutoPostBack="true" BackColor="#eeeeee" OnTextChanged="txtAddressName_TextChanged"
                                                                    runat="server" CssClass="form-control"></asp:TextBox>
                                                                <cc1:AutoCompleteExtender ID="AutoCompleteExtender3" runat="server" CompletionInterval="100"
                                                                    CompletionSetCount="1" DelimiterCharacters="" Enabled="True" MinimumPrefixLength="1"
                                                                    ServiceMethod="GetCompletionListAddressName" ServicePath="" TargetControlID="txtAddressName"
                                                                    UseContextKey="True" CompletionListCssClass="autoCompleteList" CompletionListItemCssClass="autoCompleteListItem" CompletionListHighlightedItemCssClass="autoCompleteSelectedListItem" OnClientShown="resetPosition">
                                                                </cc1:AutoCompleteExtender>
                                                                <div class="input-group-btn">
                                                                    <asp:Button ID="imgAddAddressName" ValidationGroup="Address" Style="margin-left: 10px;" Visible="false"
                                                                        OnClick="imgAddAddressName_Click"
                                                                        CssClass="btn btn-info" runat="server" Text="<%$ Resources:Attendance,Add %>" />
                                                                    <asp:Button ID="btnAddNewAddress" Style="margin-left: 10px;"
                                                                        Visible="false" OnClick="btnAddNewAddress_Click" CssClass="btn btn-info"
                                                                        runat="server" Text="<%$ Resources:Attendance,New %>" />
                                                                    <asp:HiddenField ID="Hdn_Address_ID" runat="server" />
                                                                    <asp:HiddenField ID="hdnAddressId" runat="server" />
                                                                    <asp:HiddenField ID="hdnAddressName" runat="server" />
                                                                </div>
                                                            </div>
                                                        </div>
                                                    </div>
                                                    <div class="col-md-12">
                                                        <br />
                                                        <div class="col-md-12">
                                                            <div style="overflow: auto">
                                                                <asp:GridView CssClass="table-striped table-bordered table table-hover" ID="GvAddressName" runat="server" AutoGenerateColumns="False" Width="100%">

                                                                    <Columns>
                                                                        <asp:TemplateField HeaderText="Edit">
                                                                            <ItemTemplate>
                                                                                <asp:LinkButton ID="imgBtnAddressEdit" runat="server" CommandArgument='<%# Eval("Trans_Id") %>' OnCommand="btnAddressEdit_Command"><i class="fa fa-pencil"></i></asp:LinkButton>
                                                                            </ItemTemplate>
                                                                            <ItemStyle HorizontalAlign="Center" />
                                                                        </asp:TemplateField>
                                                                        <asp:TemplateField HeaderText="Delete">
                                                                            <ItemTemplate>
                                                                                <asp:LinkButton ID="imgBtnDelete" runat="server" CommandArgument='<%# Eval("Trans_Id") %>' OnCommand="btnAddressDelete_Command"><i class="fa fa-trash"></i></asp:LinkButton>
                                                                            </ItemTemplate>
                                                                            <ItemStyle HorizontalAlign="Center" />
                                                                        </asp:TemplateField>
                                                                        <asp:TemplateField HeaderText="<%$ Resources:Attendance,S No. %>">
                                                                            <ItemTemplate>
                                                                                <asp:Label ID="lblSNo" Width="40px" runat="server" Text='<%#Eval("Trans_Id") %>' />
                                                                            </ItemTemplate>
                                                                            <ItemStyle HorizontalAlign="Center" Width="40px" />
                                                                        </asp:TemplateField>
                                                                        <asp:TemplateField HeaderText="<%$ Resources:Attendance, Address Name %>">
                                                                            <ItemTemplate>
                                                                                <asp:Label ID="lblgvAddressName" Width="100px" runat="server" Text='<%#Eval("Address_Name") %>' />
                                                                            </ItemTemplate>
                                                                            <ItemStyle HorizontalAlign="Center" Width="100px" />
                                                                        </asp:TemplateField>
                                                                        <asp:TemplateField HeaderText="<%$ Resources:Attendance, Address %>">
                                                                            <ItemTemplate>
                                                                                <asp:Label ID="lblgvAddress" runat="server" Text='<%# Eval("FullAddress") %>' />
                                                                            </ItemTemplate>
                                                                            <ItemStyle HorizontalAlign="Center" />
                                                                        </asp:TemplateField>
                                                                        <asp:TemplateField HeaderText="<%$ Resources:Attendance,EmailId %>">
                                                                            <ItemTemplate>
                                                                                <asp:Label ID="lblgvEmailId" runat="server" CssClass="labelComman" Text='<%#GetContactEmailId(Eval("Address_Name").ToString()) %>' />
                                                                            </ItemTemplate>
                                                                            <ItemStyle HorizontalAlign="Center" Width="100px" />
                                                                        </asp:TemplateField>
                                                                        <asp:TemplateField HeaderText="<%$ Resources:Attendance,FaxNo. %>">
                                                                            <ItemTemplate>
                                                                                <asp:Label ID="lblgvFaxNo" Width="80px" runat="server" CssClass="labelComman" Text='<%#GetContactFaxNo(Eval("Address_Name").ToString()) %>' />
                                                                            </ItemTemplate>
                                                                            <ItemStyle HorizontalAlign="Center" Width="80px" />
                                                                        </asp:TemplateField>
                                                                        <asp:TemplateField HeaderText="<%$ Resources:Attendance,PhoneNo.%>">
                                                                            <ItemTemplate>
                                                                                <asp:Label ID="lblgvPhoneNo" runat="server" CssClass="labelComman" Text='<%#GetContactPhoneNo(Eval("Address_Name").ToString()) %>' />
                                                                            </ItemTemplate>
                                                                            <ItemStyle HorizontalAlign="Center" Width="100px" />
                                                                        </asp:TemplateField>
                                                                        <asp:TemplateField HeaderText="<%$ Resources:Attendance,MobileNo.%>">
                                                                            <ItemTemplate>
                                                                                <asp:Label ID="lblgvMobileNo" runat="server" CssClass="labelComman" Text='<%#GetContactMobileNo(Eval("Address_Name").ToString()) %>' />
                                                                                <asp:LinkButton ID="BtnMoreNumber" runat="server" OnCommand="BtnMoreNumber_Command" CommandArgument='<%#Eval("Address_Name") %>' Text='<%#IsVisible(Eval("Address_Name").ToString())%>' />
                                                                            </ItemTemplate>
                                                                            <ItemStyle HorizontalAlign="Center" Width="100px" />
                                                                        </asp:TemplateField>
                                                                        <asp:TemplateField HeaderText="<%$ Resources:Attendance,Is Default%>">
                                                                            <ItemTemplate>
                                                                                <asp:CheckBox ID="chkdefault" runat="server" Checked='<%#Eval("Is_Default") %>' AutoPostBack="true"
                                                                                    OnCheckedChanged="chkgvSelect_CheckedChangedDefault" />
                                                                            </ItemTemplate>
                                                                            <ItemStyle HorizontalAlign="Center" Width="100px" />
                                                                        </asp:TemplateField>
                                                                    </Columns>

                                                                    <PagerStyle CssClass="pagination-ys" />

                                                                </asp:GridView>
                                                            </div>
                                                        </div>
                                                        <br />
                                                    </div>
                                                    <div class="col-md-12">
                                                        <div class="col-md-6" id="ctlEmailId" runat="server">
                                                            <asp:Label ID="Label17" runat="server" Text="<%$ Resources:Attendance,OfficialEmailId %>"></asp:Label>
                                                            <asp:TextBox ID="txtEmailId" runat="server" CssClass="form-control" />
                                                            <br />
                                                        </div>
                                                        <div class="col-md-6" id="ctlPhone" runat="server">
                                                            <asp:Label ID="Label18" runat="server" Text="<%$ Resources:Attendance,OfficePhoneNo %>"></asp:Label>
                                                            <div style="width: 100%" class="input-group">
                                                                <asp:DropDownList ID="ddlCountryCode" Style="width: 30%" runat="server" CssClass="form-control"></asp:DropDownList>
                                                                <asp:TextBox ID="txtPhoneNo" Style="width: 70%" MaxLength="10" runat="server" CssClass="form-control" />
                                                                <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender18" runat="server" Enabled="True"
                                                                    TargetControlID="txtPhoneNo" ValidChars="1,2,3,4,5,6,7,8,9,0">
                                                                </cc1:FilteredTextBoxExtender>
                                                                <br />
                                                            </div>
                                                        </div>
                                                    </div>
                                                    <div class="col-md-12">
                                                        <div class="col-md-6">
                                                            <asp:Label ID="Label55" runat="server" Text="Company Mobile No."></asp:Label>
                                                            <asp:TextBox ID="txtCompanyMobileNo" runat="server" MaxLength="12" CssClass="form-control" />
                                                            <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender22" runat="server" Enabled="True"
                                                                TargetControlID="txtCompanyMobileNo" ValidChars="1,2,3,4,5,6,7,8,9,0">
                                                            </cc1:FilteredTextBoxExtender>
                                                            <br />
                                                        </div>
                                                        <div class="col-md-6">
                                                            <asp:Label ID="Label183" runat="server" Text="Grade"></asp:Label>
                                                            <asp:DropDownList ID="ddlGrade" runat="server" CssClass="form-control"></asp:DropDownList>
                                                        </div>
                                                    </div>
                                                    <div class="col-md-12">
                                                        <div class="col-md-6" id="ctlDob" runat="server">
                                                            <asp:Label ID="Label19" runat="server" Text="<%$ Resources:Attendance,Date of Birth %>"></asp:Label>
                                                            <asp:TextBox ID="txtDob" runat="server" CssClass="form-control" autocomplete="off" />
                                                            <cc1:CalendarExtender ID="CalendarExtender2" runat="server" Enabled="True" OnClientShown="showCalendar" TargetControlID="txtDob">
                                                            </cc1:CalendarExtender>
                                                            <br />
                                                        </div>
                                                        <div class="col-md-6" id="ctlDoj" runat="server">
                                                            <asp:Label ID="Label20" runat="server" Text="<%$ Resources:Attendance,Date of Joining %>"></asp:Label>
                                                            <asp:TextBox ID="txtDoj" runat="server" CssClass="form-control" />
                                                            <cc1:CalendarExtender ID="CalendarExtender1" runat="server" Enabled="True" OnClientShown="showCalendar" TargetControlID="txtDoj">
                                                            </cc1:CalendarExtender>
                                                            <br />
                                                        </div>
                                                    </div>
                                                    <div class="col-md-12">
                                                        <div class="col-md-6">
                                                            <asp:HiddenField ID="Hdn_Brand" runat="server" />
                                                            <asp:Label ID="Label85" runat="server" Text="<%$ Resources:Attendance,Brand %>"></asp:Label><a style="color: Red">*</a>
                                                            <asp:RequiredFieldValidator EnableClientScript="true" Style="float: right;" runat="server" ID="RequiredFieldValidator3" ValidationGroup="E_Save" Display="Dynamic" SetFocusOnError="true" ControlToValidate="ddlBrand" InitialValue="--Select--" ErrorMessage="<%$ Resources:Attendance,Select Brand %>" />
                                                            <asp:DropDownList ID="ddlBrand" AutoPostBack="true" OnSelectedIndexChanged="ddlBrand_SelectedIndexChanged"
                                                                runat="server" CssClass="form-control" />

                                                            <br />
                                                        </div>
                                                        <div class="col-md-6">
                                                            <asp:HiddenField ID="Hdn_Location" runat="server" />
                                                            <asp:Label ID="Label86" runat="server" Text="<%$ Resources:Attendance,Location %>"></asp:Label><a style="color: Red">*</a>
                                                            <asp:RequiredFieldValidator EnableClientScript="true" Style="float: right;" runat="server" ID="RequiredFieldValidator4" ValidationGroup="E_Save" Display="Dynamic" SetFocusOnError="true" ControlToValidate="ddlLocation" InitialValue="--Select--" ErrorMessage="<%$ Resources:Attendance,Select Location%>" />
                                                            <asp:DropDownList ID="ddlLocation" AutoPostBack="true" OnSelectedIndexChanged="ddlLocation_SelectedIndexChanged"
                                                                runat="server" CssClass="form-control" />

                                                            <br />
                                                        </div>
                                                    </div>
                                                    <div class="col-md-12">
                                                        <div class="col-md-6" id="ctlDepartment" runat="server">
                                                            <asp:HiddenField ID="Hdn_Department" runat="server" />
                                                            <asp:Label ID="lblCountry" runat="server" Text="<%$ Resources:Attendance,Department %>"></asp:Label>
                                                            <asp:DropDownList ID="ddlDepartment" runat="server" CssClass="form-control" />
                                                            <br />
                                                        </div>
                                                        <div class="col-md-6">
                                                            <asp:Label ID="Label24" runat="server" Text="Team Leader"></asp:Label>
                                                            <asp:DropDownList ID="ddlTeamLeader" runat="server" CssClass="form-control"></asp:DropDownList>
                                                            <br />
                                                        </div>
                                                    </div>
                                                    <div class="col-md-12">
                                                        <div class="col-md-6">
                                                            <asp:CheckBox ID="chkCreateUser" runat="server" Class="form-control" Text="Create User" />
                                                            <br />
                                                        </div>
                                                        <div class="col-md-6">
                                                            <asp:DropDownList ID="ddlRole" runat="server" CssClass="form-control" />
                                                            <br />
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>

                                    <div class="col-md-12">
                                        <div id="Div_Additional_Info" class="box box-primary collapsed-box">
                                            <div class="box-header with-border">
                                                <h3 class="box-title">Additional Information</h3>
                                                <div class="box-tools pull-right">
                                                    <button type="button" class="btn btn-box-tool" data-widget="collapse">
                                                        <i id="Btn_Div_Additional_Info" class="fa fa-plus"></i>
                                                    </button>
                                                </div>
                                            </div>
                                            <div class="box-body">
                                                <div class="form-group">
                                                    <div class="col-md-12">

                                                        <div class="col-md-6">
                                                            <asp:Label ID="lblParentCompany" runat="server" Text="<%$ Resources:Attendance,Designation%>"></asp:Label><a style="color: Red">*</a>
                                                            <asp:RequiredFieldValidator EnableClientScript="true" Style="float: right;" runat="server" ID="RequiredFieldValidator5" ValidationGroup="E_Save" Display="Dynamic" SetFocusOnError="true" ControlToValidate="ddlDesignation" InitialValue="--Select--" ErrorMessage="<%$ Resources:Attendance,Select Designation %>" Enabled="false" />
                                                            <asp:DropDownList ID="ddlDesignation" runat="server"
                                                                CssClass="form-control" />

                                                            <br />
                                                        </div>
                                                    </div>
                                                    <div class="col-md-12">
                                                        <div class="col-md-6" id="ctlCivilId" runat="server">
                                                            <asp:Label ID="Label14" runat="server" Text="<%$ Resources:Attendance,Civil Id %>"></asp:Label>
                                                            <asp:TextBox ID="txtCivilId" runat="server" CssClass="form-control" />
                                                            <br />
                                                        </div>
                                                        <div class="col-md-6">
                                                            <asp:Label ID="lblCiviIdExpiryDate" runat="server" Text="Civil Id Expiry Date"></asp:Label>
                                                            <asp:TextBox ID="txtCivilIdExpiryDate" runat="server" CssClass="form-control"></asp:TextBox>
                                                            <cc1:CalendarExtender ID="CalendarExtender4" runat="server" Enabled="True"
                                                                TargetControlID="txtCivilIdExpiryDate">
                                                            </cc1:CalendarExtender>
                                                            <br />
                                                        </div>
                                                    </div>
                                                    <div class="col-md-12">
                                                        <div class="col-md-6">
                                                            <asp:Label ID="lblPassport" runat="server" Text="Passport No."></asp:Label>
                                                            <asp:TextBox ID="txtPassportNo" runat="server" CssClass="form-control"></asp:TextBox>
                                                            <br />
                                                        </div>
                                                        <div class="col-md-6">
                                                            <asp:Label ID="lblPassportexpiryDate" runat="server" Text="Passport Expiry Date"></asp:Label>
                                                            <asp:TextBox ID="txtPassportExpiryDate" runat="server" CssClass="form-control"></asp:TextBox>
                                                            <cc1:CalendarExtender ID="CalendarExtender3" runat="server" Enabled="True"
                                                                TargetControlID="txtPassportExpiryDate">
                                                            </cc1:CalendarExtender>
                                                            <br />
                                                        </div>
                                                    </div>
                                                    <div class="col-md-12">
                                                        <div class="col-md-6" id="ctlReligion" runat="server">
                                                            <asp:Label ID="lblCurrency" runat="server" Text="<%$ Resources:Attendance,Religion%>"></asp:Label>
                                                            <asp:DropDownList ID="ddlReligion" runat="server" CssClass="form-control"></asp:DropDownList>
                                                            <br />
                                                        </div>
                                                        <div class="col-md-6">
                                                            <asp:Label ID="lblQualification" runat="server" Text="<%$ Resources:Attendance,Qualification%>"></asp:Label><a style="color: Red">*</a>
                                                            <asp:RequiredFieldValidator EnableClientScript="true" runat="server" Style="float: right;" ID="RequiredFieldValidator6" ValidationGroup="E_Save" Display="Dynamic" SetFocusOnError="true" ControlToValidate="ddlQualification" InitialValue="--Select--" ErrorMessage="<%$ Resources:Attendance,Select Qualification %>" Enabled="false" />
                                                            <asp:DropDownList ID="ddlQualification" runat="server"
                                                                CssClass="form-control">
                                                            </asp:DropDownList>

                                                            <br />
                                                        </div>
                                                    </div>
                                                    <div class="col-md-12">
                                                        <div class="col-md-6">
                                                            <asp:Label ID="lblManager" runat="server" Text="<%$ Resources:Attendance,Nationality%>"></asp:Label><a style="color: Red">*</a>
                                                            <asp:RequiredFieldValidator EnableClientScript="true" Style="float: right;" runat="server" ID="RequiredFieldValidator7" ValidationGroup="E_Save" Display="Dynamic" SetFocusOnError="true" ControlToValidate="ddlNationality" InitialValue="--Select--" ErrorMessage="<%$ Resources:Attendance,Select Nationality %>" Enabled="false" />
                                                            <asp:DropDownList ID="ddlNationality" runat="server"
                                                                CssClass="form-control">
                                                            </asp:DropDownList>

                                                            <br />
                                                        </div>
                                                        <div class="col-md-6">
                                                            <asp:Label ID="Label21" runat="server" Text="<%$ Resources:Attendance,Employee Type %>"></asp:Label>
                                                            <asp:DropDownList ID="ddlEmpType" runat="server" CssClass="form-control">
                                                                <asp:ListItem Text="<%$ Resources:Attendance,On Role %>" Value="On Role"></asp:ListItem>
                                                                <asp:ListItem Text="<%$ Resources:Attendance,Off Role %>" Value="Off Role"></asp:ListItem>
                                                            </asp:DropDownList>
                                                            <br />
                                                        </div>
                                                    </div>
                                                    <div class="col-md-12">
                                                        <div class="col-md-6">
                                                            <asp:Label ID="Label23" runat="server" Text="<%$ Resources:Attendance,Termination Date %>"></asp:Label>
                                                            <asp:TextBox ID="txtTermDate" runat="server" CssClass="form-control" Enabled="true" />
                                                            <cc1:CalendarExtender ID="txtTermDate_CalendarExtender" runat="server" Enabled="True"
                                                                TargetControlID="txtTermDate">
                                                            </cc1:CalendarExtender>
                                                            <br />
                                                        </div>

                                                        <div class="col-md-6">
                                                            <asp:Label ID="Label168" runat="server" Text="<%$ Resources:Attendance,Device Group %>"></asp:Label>
                                                            <asp:DropDownList ID="ddldeviceGroup" runat="server" CssClass="form-control" />

                                                            <br />
                                                        </div>
                                                    </div>
                                                    <div class="col-md-12">
                                                        <div class="col-md-6">
                                                            <asp:Label ID="Label102" runat="server" Text="<%$ Resources:Attendance,Category %>"></asp:Label>
                                                            <asp:DropDownList ID="ddlCategoryNewTab" runat="server" CssClass="form-control">
                                                                <asp:ListItem Text="<%$ Resources:Attendance,Fresher %>" Value="Fresher"></asp:ListItem>
                                                                <asp:ListItem Text="<%$ Resources:Attendance,Experience %>" Value="Experience"></asp:ListItem>
                                                            </asp:DropDownList>
                                                            <br />
                                                        </div>
                                                        <div class="col-md-6">
                                                            <asp:Label ID="Label149" runat="server" Text="Father Name"></asp:Label>
                                                            <asp:TextBox ID="txtFatherName" runat="server" CssClass="form-control" />
                                                            <br />
                                                        </div>
                                                    </div>
                                                    <div class="col-md-12">
                                                    </div>
                                                    <div class="col-md-12">
                                                        <div class="col-md-6">
                                                            <asp:Label ID="Label151" runat="server" Text="PAN No."></asp:Label>
                                                            <asp:TextBox ID="txtPanNo" runat="server" CssClass="form-control" MaxLength="10" />
                                                            <br />
                                                        </div>
                                                        <div class="col-md-6">
                                                            <asp:Label ID="Label152" runat="server" Text="Driving License No."></asp:Label>
                                                            <asp:TextBox ID="txtDLNo" runat="server" CssClass="form-control" MaxLength="20" />
                                                        </div>
                                                    </div>
                                                    <div class="col-md-12">
                                                        <div class="col-md-6">
                                                            <asp:Label ID="Label113" runat="server" Text="<%$ Resources:Attendance,Upload Signature%>"></asp:Label>
                                                            <div class="input-group" style="width: 100%;">
                                                                <cc1:AsyncFileUpload ID="FULogoPathEmployeeSign"
                                                                    OnClientUploadStarted="EmployeeSign_UploadStarted"
                                                                    OnClientUploadError="EmployeeSign_UploadError"
                                                                    OnClientUploadComplete="EmployeeSign_UploadComplete"
                                                                    OnUploadedComplete="EmployeeSign_FileUploadComplete"
                                                                    runat="server" CssClass="form-control"
                                                                    CompleteBackColor="White"
                                                                    UploaderStyle="Traditional"
                                                                    UploadingBackColor="#CCFFFF"
                                                                    ThrobberID="EmployeeSign_ImgLoader" Width="100%" />
                                                                <div class="input-group-btn" style="border: solid 1px #d2d6de; width: 35px;">
                                                                    <asp:Image ID="EmployeeSign_Img_Right" runat="server" Width="30px" Height="30px" ImageUrl="../Images/Allow.png" Style="display: none" />
                                                                    <asp:Image ID="EmployeeSign_Img_Wrong" runat="server" Width="30px" Height="30px" ImageUrl="../Images/Delete1.png" Style="display: none" />
                                                                    <asp:Image ID="EmployeeSign_ImgLoader" runat="server" ImageUrl="../Images/loader.gif" />
                                                                </div>
                                                            </div>
                                                            <br />
                                                            <div style="text-align: center">
                                                                <asp:Image ID="ImgEmpSignature" Height="90px" Width="150px" runat="server" />
                                                            </div>
                                                            <br />
                                                            <div style="text-align: center">
                                                                <asp:Button ID="btnuploadsignature" runat="server" Text="<%$ Resources:Attendance,Load %>"
                                                                    CssClass="btn btn-primary" OnClick="btnuploadsignature_Click" />
                                                            </div>
                                                            <br />
                                                        </div>

                                                        <div class="col-md-6">
                                                            <asp:Label ID="Label130" runat="server" Text="Device Synchronization"></asp:Label>
                                                            <asp:ListBox ID="listEmpSync" runat="server" Style="width: 100%; height: 150px;" SelectionMode="Multiple"
                                                                CssClass="form-control" Font-Names="Trebuchet MS" Font-Size="Small" ForeColor="Gray"></asp:ListBox>
                                                            <br />
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>

                                    <div class="col-md-12">
                                        <div id="Div_General_Info" runat="server" class="box box-primary collapsed-box">
                                            <div class="box-header with-border">
                                                <h3 class="box-title">General Information</h3>
                                                <div class="box-tools pull-right">
                                                    <button type="button" class="btn btn-box-tool" data-widget="collapse">
                                                        <i id="Btn_Div_General_Info" runat="server" class="fa fa-plus"></i>
                                                    </button>
                                                </div>
                                            </div>
                                            <div class="box-body">
                                                <div class="form-group">
                                                    <div class="col-md-12">
                                                        <cc1:TabContainer ID="TabContainer2" runat="server" ActiveTabIndex="0" CssClass="ajax__tab_yuitabview-theme">
                                                            <cc1:TabPanel ID="TabGenral" runat="server" HeaderText="<%$ Resources:Attendance,General %>">
                                                                <ContentTemplate>
                                                                    <div class="row">
                                                                        <div class="col-md-12">
                                                                            <div class="col-md-6">
                                                                                <asp:Label ID="Label76" runat="server" Text="<%$ Resources:Attendance,Bank Name %>"></asp:Label>
                                                                                <asp:TextBox ID="txtBankName" BackColor="#eeeeee" runat="server"
                                                                                    AutoPostBack="True" OnTextChanged="txtBankName_TextChanged" CssClass="form-control" />
                                                                                <cc1:AutoCompleteExtender ID="AutoCompleteExtender2" runat="server" DelimiterCharacters=""
                                                                                    Enabled="True" ServiceMethod="GetCompletionListBankName" ServicePath="" CompletionInterval="100"
                                                                                    MinimumPrefixLength="1" CompletionSetCount="1" TargetControlID="txtBankName"
                                                                                    UseContextKey="True" CompletionListCssClass="autoCompleteList" CompletionListItemCssClass="autoCompleteListItem" CompletionListHighlightedItemCssClass="autoCompleteSelectedListItem" OnClientShown="resetPosition">
                                                                                </cc1:AutoCompleteExtender>
                                                                                <br />
                                                                            </div>
                                                                            <div class="col-md-6">
                                                                                <asp:Label ID="Label77" runat="server" Text="<%$ Resources:Attendance,Account Type %>"></asp:Label>
                                                                                <asp:DropDownList ID="ddlAcountType" runat="server"
                                                                                    CssClass="form-control">
                                                                                    <asp:ListItem Value="Current"></asp:ListItem>
                                                                                    <asp:ListItem Value="Saving"></asp:ListItem>
                                                                                </asp:DropDownList>
                                                                                <br />
                                                                            </div>
                                                                            <div class="col-md-6">
                                                                                <asp:Label ID="Label78" runat="server" Text="<%$ Resources:Attendance,Account No. %>"></asp:Label>
                                                                                <asp:TextBox ID="txtAccountNo" runat="server" CssClass="form-control" />
                                                                                <br />
                                                                            </div>
                                                                            <div class="col-md-6">
                                                                                <asp:Label ID="Label167" runat="server" Text="<%$ Resources:Attendance,Swift Code%>"></asp:Label>
                                                                                <asp:TextBox ID="Txt_Swift_Code" MaxLength="20" runat="server" CssClass="form-control" />
                                                                                <br />
                                                                            </div>
                                                                            <div class="col-md-6">
                                                                                <asp:Label ID="Label153" runat="server" Text="<%$ Resources:Attendance,IFSC Code%>"></asp:Label>
                                                                                <asp:TextBox ID="Txt_Ifsc_Code" MaxLength="20" runat="server" CssClass="form-control" />
                                                                                <br />
                                                                            </div>
                                                                            <div class="col-md-6">
                                                                                <asp:Label ID="Label166" runat="server" Text="<%$ Resources:Attendance,IBAN Code%>"></asp:Label>
                                                                                <asp:TextBox ID="Txt_IBAN_Code" MaxLength="20" runat="server" CssClass="form-control" />
                                                                                <br />
                                                                            </div>
                                                                            <div class="col-md-6">
                                                                                <asp:Label ID="Label154" runat="server" Text="<%$ Resources:Attendance,Branch Name %>"></asp:Label>
                                                                                <asp:TextBox ID="Txt_Branch_Code" MaxLength="50" runat="server" CssClass="form-control" />
                                                                                <br />
                                                                            </div>
                                                                        </div>
                                                                    </div>
                                                                </ContentTemplate>
                                                            </cc1:TabPanel>

                                                        </cc1:TabContainer>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>

                                    <div class="col-md-12">
                                        <div class="box box-primary">
                                            <div class="box-body">
                                                <div class="form-group">
                                                    <div class="col-md-12" style="text-align: center;">
                                                        <br />
                                                        <asp:Button ID="btnSave" runat="server" ValidationGroup="E_Save" CssClass="btn btn-success" OnClick="btnSave_Click"
                                                            Text="<%$ Resources:Attendance,Save %>" Visible="false" />

                                                        <asp:Button ID="btnReset" runat="server" CausesValidation="False" CssClass="btn btn-primary"
                                                            OnClick="btnReset_Click" Text="<%$ Resources:Attendance,Reset %>" />

                                                        <asp:Button ID="btnCancel" runat="server" CausesValidation="False" CssClass="btn btn-danger"
                                                            OnClick="btnCancel_Click" Text="<%$ Resources:Attendance,Cancel %>" />
                                                        <asp:HiddenField ID="editid" runat="server" />
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </div>

                    <div class="tab-pane" id="Assign" runat="server">
                        <asp:UpdatePanel ID="Update_Assign" runat="server">
                            <Triggers>
                                <asp:PostBackTrigger ControlID="lnkempassignexport" />
                            </Triggers>
                            <ContentTemplate>

                                <div class="row">
                                    <div class="col-md-12">
                                        <div id="Div3" runat="server" class="box box-info collapsed-box">
                                            <div class="box-header with-border">
                                                <h3 class="box-title">
                                                    <asp:Label ID="Label176" runat="server" Text="Advance Search"></asp:Label></h3>
                                                &nbsp;&nbsp;|&nbsp;&nbsp;
					<asp:Label ID="lblTotalRecordAssign" Text="<%$ Resources:Attendance,Total Records: 0 %>" runat="server"></asp:Label>

                                                <div class="box-tools pull-right">
                                                    <button type="button" class="btn btn-box-tool" data-widget="collapse">
                                                        <i id="I2" runat="server" class="fa fa-plus"></i>
                                                    </button>
                                                </div>
                                            </div>
                                            <div class="box-body">

                                                <div class="col-lg-3">
                                                    <asp:DropDownList ID="ddlFieldNameAssign" runat="server" CssClass="form-control">
                                                        <asp:ListItem Text="<%$ Resources:Attendance,Employee Code %>" Value="Emp_Code" Selected="True"></asp:ListItem>
                                                        <asp:ListItem Text="<%$ Resources:Attendance,Employee Name %>" Value="Emp_Name"></asp:ListItem>
                                                        <asp:ListItem Text="<%$ Resources:Attendance,Employee Name Local %>" Value="Emp_Name_L"></asp:ListItem>
                                                        <asp:ListItem Text="<%$ Resources:Attendance,Email Id %>" Value="Email_Id"></asp:ListItem>
                                                        <asp:ListItem Text="<%$ Resources:Attendance,Mobile No %>" Value="Phone_No"></asp:ListItem>
                                                        <asp:ListItem Text="<%$ Resources:Attendance,Designation%>" Value="Designation"></asp:ListItem>
                                                        <asp:ListItem Text="<%$ Resources:Attendance,Civil Id %>" Value="Civil_Id"></asp:ListItem>

                                                    </asp:DropDownList>
                                                </div>
                                                <div class="col-lg-2">
                                                    <asp:DropDownList ID="ddlOptionAssign" runat="server" CssClass="form-control">
                                                        <asp:ListItem Text="<%$ Resources:Attendance,--Select-- %>"></asp:ListItem>
                                                        <asp:ListItem Text="<%$ Resources:Attendance,Equal %>"></asp:ListItem>
                                                        <asp:ListItem Text="<%$ Resources:Attendance,Like %>"></asp:ListItem>
                                                        <asp:ListItem Selected="True" Text="<%$ Resources:Attendance,Contains %>"></asp:ListItem>
                                                    </asp:DropDownList>
                                                </div>
                                                <div class="col-lg-5">
                                                    <asp:Panel ID="Panel2" runat="server" DefaultButton="btnbindAssign">
                                                        <asp:TextBox placeholder="Search from Content" ID="txtValueAssign" CausesValidation="True" runat="server" CssClass="form-control" />
                                                        <asp:RequiredFieldValidator EnableClientScript="true" runat="server" ID="reqNameAssign" ValidationGroup="R_Search" Display="Dynamic" SetFocusOnError="true" ControlToValidate="txtValueAssign" ErrorMessage="<%$ Resources:Attendance,Please enter value %>" />
                                                    </asp:Panel>
                                                </div>
                                                <div class="col-lg-2" style="text-align: center; padding-left: 1px; padding-right: 1px;">
                                                    <asp:LinkButton ID="btnbindAssign" runat="server" CausesValidation="false" OnClick="btnbindAssign_Click" ToolTip="<%$ Resources:Attendance,Search %>"><span class="fa fa-search"  style="font-size:25px;"></span></asp:LinkButton>&nbsp;&nbsp;&nbsp;
                                                    <asp:LinkButton ID="btnRefreshAssign" runat="server" CausesValidation="False" OnClick="btnRefreshAssign_Click" ToolTip="<%$ Resources:Attendance,Refresh %>"><span class="fa fa-repeat"  style="font-size:25px;"></span></asp:LinkButton>&nbsp;&nbsp;&nbsp;
                                                    <asp:LinkButton ID="ImageButton3" runat="server" OnClick="ImgbtnSelectAll_Click1" ToolTip="<%$ Resources:Attendance, Select All %>"><span class="fas fa-th"  style="font-size:25px;"></span></asp:LinkButton>&nbsp;&nbsp;&nbsp;
                                                    <asp:LinkButton ID="lnkempassignexport" runat="server" CommandArgument="2" CommandName="OP" OnCommand="lnkempassignexport_Command" ToolTip="Update Employee Information"><span class="fas fa-file-csv"  style="font-size:25px;"></span></asp:LinkButton>

                                                </div>

                                                <asp:HiddenField ID="HiddenFieldAssign" runat="server" />

                                            </div>
                                        </div>
                                    </div>
                                </div>


                                <div class="box box-warning box-solid" <%= gvEmpAssign.Rows.Count>0?"style='display:block'":"style='display:none'"%>>
                                    <div class="box-body">
                                        <div class="row">
                                            <div class="col-md-12">
                                                <div class="flow">
                                                    <asp:HiddenField ID="hdnProductIdAssign" runat="server" />
                                                    <asp:HiddenField ID="Hdn_EditAssign" runat="server" />
                                                    <asp:GridView CssClass="table-striped table-bordered table table-hover" ID="gvEmpAssign" Style="width: 100%;" runat="server" AllowPaging="True" AutoGenerateColumns="False"
                                                        OnPageIndexChanging="gvEmpAssign_PageIndexChanging" DataKeyNames="Emp_Id"
                                                        PageSize="<%# PageControlCommon.GetPageSize() %>" Visible="false">
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

                                                            <asp:TemplateField Visible="false">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblTransIdAssign" runat="server" Text='<%# Eval("Emp_Id") %>' />
                                                                </ItemTemplate>
                                                            </asp:TemplateField>

                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,Employee Code %>">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblEmpCodeAssign" runat="server" Text='<%# Eval("Emp_Code") %>'></asp:Label>
                                                                </ItemTemplate>
                                                                <ItemStyle />
                                                            </asp:TemplateField>
                                                            <asp:BoundField DataField="Emp_Name" HeaderText="<%$ Resources:Attendance,Employee Name %>"
                                                                SortExpression="Emp_Name" />

                                                            <asp:BoundField DataField="Emp_Name_L" HeaderText="<%$ Resources:Attendance,Employee Name Local %>"
                                                                SortExpression="Emp_Name_L" />
                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,Email Id %>">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblProductCodeAssign" runat="server" Text='<%# Eval("Email_Id") %>'></asp:Label>
                                                                </ItemTemplate>
                                                                <ItemStyle />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,Phone No. %>">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblItemTypeAssign" runat="server" Text='<%# Eval("Phone_No") %>'></asp:Label>
                                                                </ItemTemplate>
                                                                <ItemStyle />
                                                            </asp:TemplateField>
                                                        </Columns>


                                                        <PagerStyle CssClass="pagination-ys" />

                                                    </asp:GridView>
                                                    <asp:HiddenField ID="HDFSortAssign" runat="server" />
                                                    <br />
                                                </div>
                                            </div>
                                            <div class="col-md-12">

                                                <div class="col-md-6">
                                                    <br />
                                                    <asp:Label ID="lblLocationAssign" runat="server" Text="Location"></asp:Label>
                                                    <asp:DropDownList ID="ddlLocationAssign" class="form-control" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlLocationAssign_SelectedIndexChanged"></asp:DropDownList>
                                                    <br />
                                                </div>

                                                <div class="col-md-6">
                                                    <br />
                                                    <asp:Label ID="lblDepartmentAssign" runat="server" Text="Department"></asp:Label>
                                                    <asp:DropDownList ID="ddlDepartmentAssign" class="form-control" runat="server"></asp:DropDownList>
                                                    <br />
                                                </div>
                                            </div>
                                            <div class="col-md-12" style="text-align: center">

                                                <asp:Button ID="btnSaveAssign" runat="server" Text="Save" CausesValidation="False" CssClass="btn btn-primary" OnClick="btnSaveAssign_Click" />

                                                <asp:Button ID="btnCancelAssign" runat="server" CausesValidation="False" CssClass="btn btn-primary"
                                                    OnClick="btnCancelAssign_Click" Text="<%$ Resources:Attendance,Cancel %>" />
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </div>

                    <div class="tab-pane" id="Terminated">
                        <asp:UpdatePanel ID="Update_Terminated" runat="server">
                            <Triggers>
                                <asp:PostBackTrigger ControlID="IbtnDownloadterminated" />
                            </Triggers>
                            <ContentTemplate>


                                <div class="row">
                                    <div class="col-md-12">
                                        <div id="Div4" runat="server" class="box box-info collapsed-box">
                                            <div class="box-header with-border">
                                                <h3 class="box-title">
                                                    <asp:Label ID="Label177" runat="server" Text="Advance Search"></asp:Label></h3>
                                                &nbsp;&nbsp;|&nbsp;&nbsp;
					<asp:Label ID="lblbinTotalRecord" Text="<%$ Resources:Attendance,Total Records: 0 %>" runat="server"></asp:Label>
                                                <asp:Label ID="lblSelectedRecord" runat="server" Visible="false"></asp:Label>

                                                <div class="box-tools pull-right">
                                                    <button type="button" class="btn btn-box-tool" data-widget="collapse">
                                                        <i id="I3" runat="server" class="fa fa-plus"></i>
                                                    </button>
                                                </div>
                                            </div>
                                            <div class="box-body">
                                                <div class="col-md-6">
                                                    <asp:HiddenField ID="HiddenField4" runat="server" />
                                                    <asp:Label ID="Label101" runat="server" Text="<%$ Resources:Attendance,Location %>"></asp:Label>
                                                    <asp:DropDownList ID="ddlLocationList_Bin" runat="server" CssClass="form-control" AutoPostBack="true"
                                                        OnSelectedIndexChanged="ddlLocationList_Bin_SelectedIndexChanged">
                                                    </asp:DropDownList>
                                                    <br />
                                                </div>

                                                <div class="col-md-6">
                                                    <asp:Label ID="Label174" runat="server" Text="<%$ Resources:Attendance,Department %>"></asp:Label>
                                                    <asp:DropDownList ID="ddlDeptList_Bin" runat="server" CssClass="form-control" AutoPostBack="true"
                                                        OnSelectedIndexChanged="ddlDeptList_Bin_SelectedIndexChanged">
                                                    </asp:DropDownList>

                                                    <br />

                                                </div>
                                                <div class="col-lg-2">
                                                    <asp:DropDownList ID="ddlbinFieldName" runat="server" CssClass="form-control">
                                                        <asp:ListItem Text="<%$ Resources:Attendance,Employee Code %>" Value="Emp_Code"></asp:ListItem>
                                                        <asp:ListItem Text="<%$ Resources:Attendance,Employee Name %>" Value="Emp_Name"></asp:ListItem>
                                                        <asp:ListItem Text="<%$ Resources:Attendance,Employee Name Local %>" Value="Emp_Name_L"></asp:ListItem>
                                                        <asp:ListItem Text="<%$ Resources:Attendance,Email Id %>" Value="Email_Id"></asp:ListItem>
                                                        <asp:ListItem Text="<%$ Resources:Attendance,Mobile No %>" Value="Phone_No"></asp:ListItem>
                                                        <asp:ListItem Text="<%$ Resources:Attendance,Designation%>" Value="Designation"></asp:ListItem>
                                                        <asp:ListItem Text="<%$ Resources:Attendance,Civil Id %>" Value="Civil_Id"></asp:ListItem>
                                                    </asp:DropDownList>
                                                </div>
                                                <div class="col-lg-2">
                                                    <asp:DropDownList ID="ddlbinOption" runat="server" CssClass="form-control">
                                                        <asp:ListItem Text="<%$ Resources:Attendance,--Select-- %>"></asp:ListItem>
                                                        <asp:ListItem Text="<%$ Resources:Attendance,Equal %>"></asp:ListItem>
                                                        <asp:ListItem Text="<%$ Resources:Attendance,Like %>"></asp:ListItem>
                                                        <asp:ListItem Selected="True" Text="<%$ Resources:Attendance,Contains %>"></asp:ListItem>

                                                    </asp:DropDownList>
                                                </div>
                                                <div class="col-lg-4">
                                                    <asp:Panel ID="Panel8" runat="server" DefaultButton="btnbinbind">
                                                        <asp:TextBox placeholder="Search from Content" ID="txtbinVal" runat="server" CssClass="form-control" />
                                                        <asp:RequiredFieldValidator EnableClientScript="true" runat="server" ID="RequiredFieldValidator8" ValidationGroup="T_Search" Display="Dynamic" SetFocusOnError="true" ControlToValidate="txtbinVal" ErrorMessage="<%$ Resources:Attendance,Please enter value %>" />
                                                    </asp:Panel>
                                                </div>
                                                <div class="col-lg-3" style="text-align: center; padding-left: 1px; padding-right: 1px;">

                                                    <asp:LinkButton ID="btnbinbind" ValidationGroup="T_Search" runat="server" CausesValidation="true" OnClick="btnbinbind_Click" ToolTip="<%$ Resources:Attendance,Search %>"><span class="fa fa-search"  style="font-size:25px;"></span></asp:LinkButton>&nbsp;&nbsp;&nbsp;                                                    
                                                     <asp:LinkButton ID="btnbinRefresh" runat="server" CausesValidation="False" OnClick="btnbinRefresh_Click" ToolTip="<%$ Resources:Attendance,Refresh %>"><span class="fa fa-repeat"  style="font-size:25px;"></span></asp:LinkButton>&nbsp;&nbsp;&nbsp;
                                                    <asp:LinkButton ID="imgBtnbinGrid" CausesValidation="true" runat="server" OnClick="imgBtnbinGrid_Click" ToolTip="<%$ Resources:Attendance, Grid View %>"><span class="fa fa-table"  style="font-size:25px;"></span></asp:LinkButton>&nbsp;&nbsp;&nbsp;
                                                    <asp:LinkButton ID="imgBtnbinDatalist" CausesValidation="true" runat="server" OnClick="imgbtnbinDatalist_Click" ToolTip="<%$ Resources:Attendance,List View %>" Visible="False"><span class="fa fa-list"  style="font-size:25px;"></span></asp:LinkButton>&nbsp;&nbsp;&nbsp;
                                                    <asp:LinkButton ID="imgBtnRestore" ValidationGroup="T_Restore" CausesValidation="true" runat="server" OnClick="imgBtnRestore_Click" ToolTip="<%$ Resources:Attendance, Active %>" Visible="false"><span class="far fa-lightbulb"  style="font-size:25px;"></span></asp:LinkButton>&nbsp;&nbsp
                                                    <asp:LinkButton ID="ImgbtnSelectAll" Visible="false" runat="server" OnClick="ImgbtnSelectAll_Click" ToolTip="<%$ Resources:Attendance, Select All %>"><span class="fas fa-th"  style="font-size:25px;"></span></asp:LinkButton>&nbsp;&nbsp;&nbsp;
                                                    <asp:LinkButton ID="IbtnDownloadterminated" runat="server" CommandArgument="2" CommandName="OP" OnCommand="IbtnDownloadterminated_Click" ToolTip="Download"><span class="fas fa-file-csv"  style="font-size:25px;"></span></asp:LinkButton>&nbsp;&nbsp;&nbsp;
                                                    <asp:LinkButton ID="btnTerDel" runat="server" OnClick="btnTerDel_Click"><span class="fas fa-remove"  style="font-size:30px;"></span></asp:LinkButton>
                                                </div>

                                            </div>
                                        </div>
                                    </div>
                                </div>

                                <div class="box box-warning box-solid" <%= gvBinEmp.Rows.Count>0?"style='display:block'":"style='display:none'"%>>
                                    <div class="box-body">
                                        <div class="row">
                                            <div class="col-md-12">
                                                <div class="flow">
                                                    <asp:CustomValidator ID="CustomValidator1" ValidationGroup="T_Restore" runat="server" ErrorMessage="Please select at least one record."
                                                        ClientValidationFunction="Validate" ForeColor="Red"></asp:CustomValidator>
                                                    <asp:GridView CssClass="table-striped table-bordered table table-hover" ID="gvBinEmp" runat="server" AllowPaging="True" AutoGenerateColumns="False"
                                                        OnPageIndexChanging="gvBinEmp_PageIndexChanging" Width="100%"
                                                        DataKeyNames="Emp_Id" PageSize="<%# PageControlCommon.GetPageSize() %>" Visible="false">
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
                                                                SortExpression="Emp_Name" />
                                                            <asp:BoundField DataField="Emp_Name_L" HeaderText="<%$ Resources:Attendance,Employee Name Local %>"
                                                                SortExpression="Emp_Name_L" />
                                                            <asp:BoundField DataField="DepartmentName" HeaderText="<%$ Resources:Attendance,Department %>"
                                                                SortExpression="DepartmentName" ItemStyle-Width="20%" />
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
                                                    <asp:DataList ID="dtlistbinEmp" runat="server" RepeatColumns="3" RepeatDirection="Horizontal">
                                                        <ItemTemplate>
                                                            <div class="col-md-4" style="width: 100%; min-width: 395px;">
                                                                <div class="box box-widget widget-user-2">
                                                                    <!-- Add the bg color to the header using any of the bg-* classes -->
                                                                    <div class="widget-user-header bg-yellow">
                                                                        <div class="widget-user-image">
                                                                            <img id="ImgBtnEmp" runat="server" class="img-circle" src='<%#getImageByEmpId( Eval("Emp_Id")) %>'>
                                                                        </div>
                                                                        <!-- /.widget-user-image -->
                                                                        <h3 class="widget-user-username">
                                                                            <asp:LinkButton ID="lnkEmpname" runat="server" CausesValidation="false" ForeColor="#1886b9"
                                                                                Font-Bold="true" Enabled="false" CommandArgument='<%# Eval("Emp_Id") %>' OnCommand="lnkEditCommand"
                                                                                Text='<%# GetEmployeeName(Eval("Emp_Id"))%>'></asp:LinkButton></h3>
                                                                        <h5 class="widget-user-desc">
                                                                            <asp:Label ID="lblEmalid" runat="server" Text='<%# getEmailId(Eval("Emp_Id")) %>'></asp:Label></h5>
                                                                    </div>
                                                                    <div class="box-footer no-padding">
                                                                        <ul class="nav nav-stacked">
                                                                            <li>
                                                                                <asp:HiddenField ID="hdnChbActive" runat="server" Value='<%# Eval("Emp_Id")%>' />
                                                                                <asp:Label ID="lblGvEmpId" runat="server" Text='<%# Eval("Emp_Code") %>'></asp:Label></li>
                                                                            <li>
                                                                                <asp:Label ID="lblDesignation" runat="server" Text='<%# getdesg(Eval("Emp_Id")) %>'></asp:Label></li>
                                                                            <li>
                                                                                <asp:Label ID="lblDepartment" runat="server" Text='<%# getdepartment(Eval("Emp_Id")) %>'></asp:Label></li>
                                                                            <li>DOB :
                                                                                        <asp:Label ID="Label15" runat="server" Text='<%# getdate(Eval("DOB")) %>' /></li>
                                                                            <li>DOJ :
                                                                                        <asp:Label ID="Label16" runat="server" Text='<%# getdate(Eval("DOJ")) %>'></asp:Label></li>
                                                                            <li>
                                                                                <asp:Label ID="lblMobile" runat="server" Text='<%#getMobileNo( Eval("Emp_Id")) %>'></asp:Label></li>
                                                                            <li>
                                                                                <asp:CheckBox ID="chbAcInctive" runat="server" AutoPostBack="True"
                                                                                    OnCheckedChanged="chbAcInctive_CheckedChanged" Text="Active" /></li>
                                                                        </ul>
                                                                    </div>
                                                                </div>
                                                            </div>

                                                        </ItemTemplate>
                                                    </asp:DataList>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="col-md-12" style="text-align: center;">
                                            <asp:LinkButton ID="lnkbinFirst" runat="server" Font-Size="Small" Font-Bold="True" ForeColor="#1886b9"
                                                CausesValidation="False" OnClick="lnkbinFirst_Click" Style="margin-left: 10px; text-decoration: none">First</asp:LinkButton>
                                            <asp:LinkButton ID="lnkbinPrev" runat="server" Font-Bold="True" Font-Size="Small" ForeColor="#1886b9"
                                                CausesValidation="False" OnClick="lnkbinPrev_Click" Style="margin-left: 10px; text-decoration: none">Prev</asp:LinkButton>
                                            <asp:LinkButton ID="lnkbinNext" runat="server" Font-Size="Small" Font-Bold="True" ForeColor="#1886b9"
                                                CausesValidation="False" OnClick="lnkbinNext_Click" Style="margin-left: 10px; text-decoration: none">Next</asp:LinkButton>
                                            <asp:LinkButton ID="lnkbinLast" runat="server" Font-Bold="True" Font-Size="Small" ForeColor="#1886b9"
                                                CausesValidation="False" OnClick="lnkbinLast_Click" Style="margin-left: 10px; text-decoration: none">Last</asp:LinkButton>
                                        </div>
                                    </div>
                                </div>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </div>

                    <div class="tab-pane" id="Leave" runat="server">
                        <asp:UpdatePanel ID="Update_Leave" runat="server">
                            <ContentTemplate>
                                <div class="row">
                                    <div class="col-md-12">
                                        <div class="box box-primary">
                                            <div class="box-body">
                                                <div class="form-group">
                                                    <div class="col-md-12">
                                                        <asp:RadioButton ID="rbtnGroup" OnCheckedChanged="EmpGroup_CheckedChanged" runat="server"
                                                            Text="<%$ Resources:Attendance,Group %>" Font-Bold="true" GroupName="EmpGroup"
                                                            AutoPostBack="true" />
                                                        <asp:RadioButton ID="rbtnEmp" Checked="true" Style="margin-left: 20px;" runat="server" AutoPostBack="true" Text="<%$ Resources:Attendance,Employee %>"
                                                            GroupName="EmpGroup" Font-Bold="true" OnCheckedChanged="EmpGroup_CheckedChanged" />
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                                <div class="col-md-12">
                                    <div id="Panel_Leave_Employee" runat="server" class="row">

                                        <div class="row">
                                            <div class="col-md-12">
                                                <div id="Div5" runat="server" class="box box-info collapsed-box">
                                                    <div class="box-header with-border">
                                                        <h3 class="box-title">
                                                            <asp:Label ID="Label178" runat="server" Text="Advance Search"></asp:Label></h3>
                                                        &nbsp;&nbsp;|&nbsp;&nbsp;
					<asp:Label ID="lblTotalRecordsLeave" runat="server" Text="<%$ Resources:Attendance,Total Records: 0 %>"></asp:Label>

                                                        <div class="box-tools pull-right">
                                                            <button type="button" class="btn btn-box-tool" data-widget="collapse">
                                                                <i id="I4" runat="server" class="fa fa-plus"></i>
                                                            </button>
                                                        </div>
                                                    </div>
                                                    <div class="box-body">

                                                        <div class="col-md-6">
                                                            <asp:Label ID="lblLocationLeave" runat="server" Text="<%$ Resources:Attendance,Location %>"></asp:Label>
                                                            <asp:DropDownList ID="ddlLocationLeave" runat="server" CssClass="form-control"
                                                                AutoPostBack="true" OnSelectedIndexChanged="ddlLocationLeave_SelectedIndexChanged">
                                                            </asp:DropDownList>
                                                            <br />
                                                        </div>
                                                        <div class="col-md-6">
                                                            <asp:Label ID="lblDepartmentLeave" runat="server" Text="<%$ Resources:Attendance,Department %>"></asp:Label>

                                                            <asp:DropDownList ID="ddlDepartmentLeave" runat="server" CssClass="form-control"
                                                                AutoPostBack="true" OnSelectedIndexChanged="ddlDepartmentLeave_SelectedIndexChanged">
                                                            </asp:DropDownList>
                                                            <br />
                                                        </div>
                                                        <div class="col-lg-3">
                                                            <asp:DropDownList ID="ddlField1" runat="server" CssClass="form-control">
                                                                <asp:ListItem Text="<%$ Resources:Attendance,Employee Code %>" Value="Emp_Code" Selected="True"></asp:ListItem>
                                                                <asp:ListItem Text="<%$ Resources:Attendance,Employee Name %>" Value="Emp_Name"></asp:ListItem>
                                                                <asp:ListItem Text="<%$ Resources:Attendance,Employee Name Local %>" Value="Emp_Name_L"></asp:ListItem>
                                                                <asp:ListItem Text="<%$ Resources:Attendance,Email Id %>" Value="Email_Id"></asp:ListItem>
                                                                <asp:ListItem Text="<%$ Resources:Attendance,Phone No. %>" Value="Phone_No"></asp:ListItem>
                                                                <asp:ListItem Text="<%$ Resources:Attendance,Civil Id %>" Value="Civil_Id"></asp:ListItem>
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
                                                            <asp:Panel ID="Panel3" runat="server" DefaultButton="imgBtnLeaveBind">
                                                                <asp:TextBox ID="txtValue1" placeholder="Search from Content" runat="server" CssClass="form-control"></asp:TextBox>
                                                                <asp:RequiredFieldValidator EnableClientScript="true" runat="server" ID="RequiredFieldValidator9" ValidationGroup="L_Search" Display="Dynamic" SetFocusOnError="true" ControlToValidate="txtValue1" ErrorMessage="<%$ Resources:Attendance,Please enter value %>" />
                                                            </asp:Panel>
                                                        </div>
                                                        <div class="col-lg-2" style="text-align: center; padding-left: 1px; padding-right: 1px;">
                                                            <asp:LinkButton ID="imgBtnLeaveBind" ValidationGroup="L_Search" runat="server" CausesValidation="true" OnClick="btnLeavebind_Click" ToolTip="<%$ Resources:Attendance,Search %>"><span class="fa fa-search"  style="font-size:25px;"></span></asp:LinkButton>&nbsp;&nbsp;&nbsp;
                                                            <asp:LinkButton ID="Img_Button_Refresh" runat="server" CausesValidation="False" OnClick="btnLeaveRefresh_Click" ToolTip="<%$ Resources:Attendance,Refresh %>"><span class="fa fa-repeat"  style="font-size:25px;"></span></asp:LinkButton>&nbsp;&nbsp;&nbsp;
                                                            <asp:LinkButton ID="ImageButton26" runat="server" OnClick="ImgbtnSelectAll_ClickLeave" ToolTip="<%$ Resources:Attendance, Select All %>"><span class="fas fa-th"  style="font-size:25px;"></span></asp:LinkButton>
                                                        </div>

                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="box box-warning box-solid" <%= gvEmpLeave.Rows.Count>0?"style='display:block'":"style='display:none'"%>>
                                            <div class="box-body">
                                                <div class="row">
                                                    <div class="col-md-12" style="width: 100%;">
                                                        <div style="width: 100%; overflow: auto;">
                                                            <asp:Label ID="lblSelectRecd" runat="server" Visible="false"></asp:Label>
                                                            <asp:CustomValidator ID="CustomValidator2" ValidationGroup="L_Grid_Save" runat="server" ErrorMessage="Please select at least one record."
                                                                ClientValidationFunction="Leave_Validate" ForeColor="Red"></asp:CustomValidator>
                                                            <asp:GridView CssClass="table-striped table-bordered table table-hover" ID="gvEmpLeave" Width="100%" runat="server" AllowPaging="True" AutoGenerateColumns="False"
                                                                OnPageIndexChanging="gvEmpLeave_PageIndexChanging"
                                                                DataKeyNames="Emp_Id" PageSize="<%# PageControlCommon.GetPageSize() %>" Visible="false">
                                                                <Columns>
                                                                    <asp:TemplateField>
                                                                        <ItemTemplate>
                                                                            <asp:CheckBox ID="chkgvSelect" runat="server" />
                                                                        </ItemTemplate>
                                                                        <HeaderTemplate>
                                                                            <asp:CheckBox ID="chkgvSelectAll" runat="server" OnCheckedChanged="chkgvSelectAll_CheckedChangedLeave"
                                                                                AutoPostBack="true" />
                                                                        </HeaderTemplate>
                                                                        <ItemStyle HorizontalAlign="Center" />
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="<%$ Resources:Attendance,View %>">
                                                                        <ItemTemplate>
                                                                            <asp:LinkButton ID="lnkViewDetail" runat="server" Width="16px" OnClientClick="show_Leave_View_Popup" CommandArgument='<%# Eval("Emp_Id") %>' Visible='<%# hdnCanEdit.Value=="true"?true:false%>' ToolTip="View" OnCommand="lnkLeaveViewDetail_Command" CausesValidation="False"><i class="fa fa-eye"></i></asp:LinkButton>
                                                                        </ItemTemplate>
                                                                        <ItemStyle HorizontalAlign="Center" />
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="<%$ Resources:Attendance,Edit %>">
                                                                        <ItemTemplate>
                                                                            <asp:LinkButton ID="btnEdit" runat="server" CausesValidation="False" CommandArgument='<%# Eval("Emp_Id") %>' Visible='<%# hdnCanEdit.Value=="true"?true:false%>' OnCommand="btnEditLeave_Command"><i class="fa fa-pencil"></i></asp:LinkButton>
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
                                                                        SortExpression="Emp_Name" />
                                                                    <asp:BoundField DataField="Emp_Name_L" HeaderText="<%$ Resources:Attendance,Employee Name Local%>"
                                                                        SortExpression="Emp_Name_L" />
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
                                    <div id="pnlGroup" visible="false" runat="server" class="row">
                                        <div class="row">
                                            <div class="col-md-12">
                                                <div class="box box-primary">
                                                    <div class="box-body">
                                                        <div class="form-group">
                                                            <div class="col-md-6">
                                                                <asp:CustomValidator ID="CustomValidator8" ValidationGroup="L_Grid_Save" runat="server" ErrorMessage="Please select at least one record."
                                                                    ClientValidationFunction="Leave_Validate" ForeColor="Red"></asp:CustomValidator>
                                                                <asp:ListBox ID="lbxGroup" runat="server" OnSelectedIndexChanged="lbxGroup_SelectedIndexChanged" AutoPostBack="true" Style="width: 100%; height: 150px;" SelectionMode="Multiple"
                                                                    CssClass="form-control" Font-Names="Trebuchet MS" Font-Size="Small" ForeColor="Gray"></asp:ListBox>
                                                            </div>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="col-md-12">
                                            <div class="flow">
                                                <asp:GridView CssClass="table-striped table-bordered table table-hover" ID="gvEmployee" runat="server" Width="100%" AllowPaging="True" AutoGenerateColumns="False"
                                                    OnPageIndexChanging="gvEmp1_PageIndexChanging" PageSize="<%# PageControlCommon.GetPageSize() %>">
                                                    <Columns>
                                                        <asp:TemplateField HeaderText="<%$ Resources:Attendance,Employee Code %>">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblEmpCode" runat="server" Text='<%# Eval("Emp_Code") %>'></asp:Label>
                                                                <asp:Label ID="lblEmpId" Visible="false" runat="server" Text='<%# Eval("Emp_Id") %>'></asp:Label>
                                                            </ItemTemplate>
                                                            <ItemStyle />
                                                        </asp:TemplateField>
                                                        <asp:BoundField DataField="Emp_Name" HeaderText="<%$ Resources:Attendance,Employee Name %>"
                                                            SortExpression="Emp_Name" />
                                                        <asp:BoundField DataField="Emp_Name_L" HeaderText="<%$ Resources:Attendance,Employee Name Local%>"
                                                            SortExpression="Emp_Name_L" />
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
                                                <asp:Label ID="lblEmp" runat="server" Visible="false"></asp:Label>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="col-md-12">
                                        <div class="box box-primary">
                                            <div class="box-body">
                                                <div class="form-group">
                                                    <div class="col-md-12">
                                                        <div class="col-md-6">
                                                            <br />
                                                            <asp:Label ID="Label99" runat="server" Text="<%$ Resources:Attendance,Yearly Half Day Allow %>"></asp:Label>
                                                            <div class="input-group" style="width: 100%">
                                                                <asp:TextBox ID="txtHalfDayCount" Style="width: 85%;" CssClass="form-control" runat="server"></asp:TextBox>
                                                                <cc1:FilteredTextBoxExtender ID="FilterHalfday" TargetControlID="txtHalfDayCount"
                                                                    ValidChars="0,1,2,3,4,5,6,7,8,9" runat="server">
                                                                </cc1:FilteredTextBoxExtender>
                                                                <asp:Button ID="btnSaveHalfday" Style="margin-left: 10px;" runat="server" CssClass="btn btn-success"
                                                                    OnClick="btnSaveHalfday_Click" Text="<%$ Resources:Attendance,Save %>" Visible="false" />
                                                            </div>
                                                            <br />
                                                        </div>

                                                    </div>
                                                    <div class="col-md-12">
                                                        <div class="col-md-6">
                                                            <asp:Label ID="lblLeaveType" runat="server" Text="<%$ Resources:Attendance,Leave Type %>"></asp:Label><a style="color: Red">*</a>
                                                            <asp:RequiredFieldValidator EnableClientScript="true" Style="float: right;" runat="server" ID="RequiredFieldValidator16" ValidationGroup="L_Grid_Save"
                                                                Display="Dynamic" SetFocusOnError="true" ControlToValidate="ddlLeaveType" InitialValue="--Select--" ErrorMessage="<%$ Resources:Attendance,Select Leave Type%>" />
                                                            <asp:DropDownList ID="ddlLeaveType" runat="server" class="form-control" AutoPostBack="true" OnSelectedIndexChanged="ddlLeaveType_OnSelectedIndexChanged">
                                                            </asp:DropDownList>
                                                            <br />
                                                        </div>
                                                        <div class="col-md-6">
                                                            <asp:Label ID="lblTotalLeave" runat="server" Text="<%$ Resources:Attendance,Total Leave Day %>"></asp:Label><a style="color: Red">*</a>
                                                            <asp:RequiredFieldValidator EnableClientScript="true" Style="float: right;" runat="server" ID="RequiredFieldValidator17" ValidationGroup="L_Grid_Save"
                                                                Display="Dynamic" SetFocusOnError="true" ControlToValidate="txtTotalLeave" ErrorMessage="<%$ Resources:Attendance,Enter Total Leave Day %>" />
                                                            <asp:TextBox ID="txtTotalLeave" class="form-control" runat="server" MaxLength="3"
                                                                onkeypress="return isNumber(event)"></asp:TextBox>
                                                            <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender5" runat="server" Enabled="True"
                                                                TargetControlID="txtTotalLeave" ValidChars="1,2,3,4,5,6,7,8,9,0,.">
                                                            </cc1:FilteredTextBoxExtender>
                                                            <br />
                                                        </div>
                                                        <div class="col-md-6">
                                                            <asp:Label ID="lblPaidLeave" runat="server" Text="<%$ Resources:Attendance,Paid Leave Day %>"></asp:Label>
                                                            <a style="color: Red">*</a>
                                                            <asp:RequiredFieldValidator EnableClientScript="true" Style="float: right;" runat="server" ID="RequiredFieldValidator18" ValidationGroup="L_Grid_Save"
                                                                Display="Dynamic" SetFocusOnError="true" ControlToValidate="txtPaidLeave" ErrorMessage="<%$ Resources:Attendance,Enter Paid Leave Day %>" />

                                                            <asp:TextBox ID="txtPaidLeave" runat="server" class="form-control" MaxLength="3" onkeypress="return isNumber(event)"></asp:TextBox>
                                                            <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender4" runat="server" Enabled="True"
                                                                TargetControlID="txtPaidLeave" ValidChars="1,2,3,4,5,6,7,8,9,0,.">
                                                            </cc1:FilteredTextBoxExtender>
                                                            <br />
                                                        </div>
                                                        <div class="col-md-6">
                                                            <asp:Label ID="lblSchType" runat="server" Text="<%$ Resources:Attendance,Schedule Type %>"></asp:Label><a style="color: Red">*</a>
                                                            <asp:RequiredFieldValidator EnableClientScript="true" Style="float: right;" runat="server" ID="RequiredFieldValidator19" ValidationGroup="L_Grid_Save"
                                                                Display="Dynamic" SetFocusOnError="true" ControlToValidate="ddlSchType" InitialValue="0" ErrorMessage="<%$ Resources:Attendance,Select Schedule Type %>" />

                                                            <asp:DropDownList ID="ddlSchType" class="form-control" runat="server" AutoPostBack="True"
                                                                OnSelectedIndexChanged="ddlSchedule_SelectedIndexChanged">
                                                                <%--<asp:ListItem Value="0">--Select--</asp:ListItem>
                                                                <asp:ListItem Value="Monthly">Monthly</asp:ListItem>--%>
                                                                <asp:ListItem Value="Yearly" Selected="True">Yearly</asp:ListItem>
                                                            </asp:DropDownList>
                                                            <br />
                                                        </div>
                                                        <div class="col-md-6">

                                                            <asp:CheckBox ID="chkYearCarry" Style="margin-left: 10px; margin-right: 10px;" runat="server" Text="Is Year Carry" />
                                                            <asp:CheckBox ID="chkIsAuto" Style="margin-left: 10px; margin-right: 10px;" runat="server" Text="Is Auto" />

                                                            <%--   <asp:RadioButtonList ID="chkYearCarry" runat="server" RepeatDirection="Horizontal">
                                                                <asp:ListItem style="margin-right: 10px;" Text="Is Year Carry" Value="1"></asp:ListItem>
                                                                <asp:ListItem style="margin-left: 10px;" Text="Is Auto" Value="2"></asp:ListItem>
                                                            </asp:RadioButtonList>--%>
                                                            <br />
                                                        </div>
                                                        <div class="col-md-6">
                                                            <asp:CheckBox ID="ChkIsRule" runat="server" Text="Is Rule" />
                                                            <br />
                                                        </div>

                                                        <div class="col-md-6">
                                                            <asp:CheckBox ID="chkMonthCarry" Visible="false" runat="server"
                                                                Text="<%$ Resources:Attendance,IsMonthCarry %>" />
                                                            <br />
                                                        </div>
                                                    </div>
                                                    <div id="Tr1" visible="false" runat="server" class="col-md-12">
                                                        <div class="col-md-6">
                                                            <asp:Label ID="lblPerOfSal" runat="server" Text="<%$ Resources:Attendance,Percentage Of Salary %>"></asp:Label>
                                                            <asp:TextBox ID="txtPerSal" runat="server"
                                                                Text="100"></asp:TextBox>
                                                            <cc1:FilteredTextBoxExtender ID="txtMobile_FilteredTextBoxExtender" runat="server"
                                                                Enabled="True" TargetControlID="txtPerSal" ValidChars="1,2,3,4,5,6,7,8,9,0,.">
                                                            </cc1:FilteredTextBoxExtender>
                                                        </div>
                                                    </div>
                                                    <div id="trdeductionSlab" runat="server" visible="false" class="col-md-12">
                                                        <asp:Label ID="Label1" runat="server" CssClass="labelComman" Text="Deduction slab after finish paid days" Font-Bold="true"></asp:Label>
                                                        <asp:Label ID="Label115" runat="server" CssClass="labelComman" Text="Exceed Days From"></asp:Label>
                                                        <asp:TextBox ID="txtexceedDays" runat="server" CssClass="textComman" Enabled="false" />
                                                        <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtendertxtexceedDays" runat="server" Enabled="True"
                                                            TargetControlID="txtexceedDays" FilterType="Numbers">
                                                        </cc1:FilteredTextBoxExtender>
                                                        <asp:Label ID="Label108" runat="server" CssClass="labelComman" Text="Exceed Days To"></asp:Label>
                                                        <asp:TextBox ID="txtexceedDaysto" runat="server" CssClass="textComman" />

                                                        <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtendertxtexceedDaysto" runat="server" Enabled="True"
                                                            TargetControlID="txtexceedDaysto" FilterType="Numbers">
                                                        </cc1:FilteredTextBoxExtender>
                                                        <asp:Label ID="Label109" runat="server" CssClass="labelComman" Text="Deduction(%)"></asp:Label>
                                                        <asp:TextBox ID="txtdeduction" runat="server" CssClass="textComman" />
                                                        <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtendertxtdeduction" runat="server" Enabled="True"
                                                            TargetControlID="txtdeduction" FilterType="Numbers">
                                                        </cc1:FilteredTextBoxExtender>
                                                        <asp:Button ID="btndeduction" runat="server" Width="70px" Text="<%$ Resources:Attendance,Add %>"
                                                            CssClass="buttonCommman" OnClick="btndeduction_Click" />

                                                        <asp:Button ID="btndeductionCancel" runat="server" Width="70px" Text="<%$ Resources:Attendance,Cancel %>"
                                                            CssClass="buttonCommman" OnClick="btndeductionCancel_Click" />

                                                        <asp:GridView CssClass="table-striped table-bordered table table-hover" ID="GvDeductionDetail" runat="server" AutoGenerateColumns="False"
                                                            Width="100%" DataKeyNames="Trans_Id" ShowFooter="false">

                                                            <Columns>
                                                                <asp:TemplateField HeaderText="Edit">
                                                                    <ItemTemplate>
                                                                        <asp:ImageButton ID="imgBtnEmployeeEdit" runat="server" CommandArgument='<%# Eval("Trans_Id") %>'
                                                                            Height="16px" ImageUrl="~/Images/Edit.png"
                                                                            Width="16px" OnCommand="imgBtnEmployeeEdit_Command" />
                                                                    </ItemTemplate>
                                                                    <ItemStyle HorizontalAlign="Center" Width="16px" />
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="Delete">
                                                                    <ItemTemplate>
                                                                        <asp:ImageButton ID="imgBtnEmpoloyeeDelete" runat="server" CommandArgument='<%# Eval("Trans_Id") %>'
                                                                            Height="16px" ImageUrl="~/Images/Erase.png" Width="16px" OnCommand="imgBtnEmpoloyeeDelete_Command" />
                                                                    </ItemTemplate>
                                                                    <ItemStyle HorizontalAlign="Center" Width="16px" />
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="Exceed From (In Days) ">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblDaysFrom" runat="server" Text='<%#Eval("DaysFrom") %>' />
                                                                        <asp:Label ID="lblTransId" Visible="false" runat="server" Text='<%#Eval("Trans_Id") %>' />
                                                                    </ItemTemplate>
                                                                    <ItemStyle HorizontalAlign="Center" Width="60px" />
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="Exceed to (In Days)">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblDaysTo" runat="server" Text='<%#Eval("DaysTo") %>' />
                                                                    </ItemTemplate>
                                                                    <ItemStyle HorizontalAlign="Center" Width="60px" />
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="deduction(%)">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lbldeductionpercentage" runat="server" Text='<%#Eval("Deduction_Percentage") %>' />
                                                                    </ItemTemplate>
                                                                    <ItemStyle HorizontalAlign="Center" Width="60px" />
                                                                </asp:TemplateField>
                                                            </Columns>

                                                            <PagerStyle CssClass="pagination-ys" />

                                                        </asp:GridView>
                                                        <asp:HiddenField ID="hdndeductionTransId" runat="server" />
                                                    </div>
                                                    <div class="col-md-12" style="text-align: center;">
                                                        <asp:Button ID="btnSaveLeave" runat="server" CssClass="btn btn-success" OnClick="btnSaveLeave_Click"
                                                            Text="<%$ Resources:Attendance,Save %>" ValidationGroup="L_Grid_Save" CausesValidation="true" Visible="false" />

                                                        <asp:Button ID="btnResetLeave" runat="server" CausesValidation="False" CssClass="btn btn-primary"
                                                            OnClick="btnResetLeave_Click" Text="<%$ Resources:Attendance,Reset %>" />

                                                        <asp:Button ID="btnCancelLeave" runat="server" CausesValidation="False" CssClass="btn btn-danger"
                                                            OnClick="btnCancelLeave_Click" Text="<%$ Resources:Attendance,Cancel %>" />
                                                    </div>
                                                    <div class="col-md-12">
                                                        <div class="flow">
                                                            <asp:GridView CssClass="table-striped table-bordered table table-hover" ID="gridEmpLeave" Width="100%" runat="server" AutoGenerateColumns="False" DataKeyNames="Trans_No"
                                                                PageSize="<%# PageControlCommon.GetPageSize() %>">
                                                                <Columns>
                                                                    <asp:TemplateField HeaderText="<%$ Resources:Attendance,LeaveType %>">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblleavetype0" runat="server" Text='<%# GetLeaveTypeName(Eval("LeaveType_Id")) %>'></asp:Label>
                                                                            <asp:HiddenField ID="hdnLeaveTypeId" runat="server" Value='<%# Eval("LeaveType_Id") %>' />
                                                                            <asp:HiddenField ID="hdnTranNo" runat="server" Value='<%# Eval("Trans_No") %>' />
                                                                            <asp:HiddenField ID="hdnEmpId1" runat="server" Value='<%# Eval("Emp_Id") %>' />
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="<%$ Resources:Attendance,Total Leave %>">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="txtNoOfLeave0" runat="server" Text='<%# Eval("Total_Leave") %>'></asp:Label>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="<%$ Resources:Attendance,Paid Leave %>">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="txtPAidLEave1" runat="server" Text='<%# Eval("Paid_Leave") %>'></asp:Label>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="<%$ Resources:Attendance,Percentage Of Salary %>">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="TextBox1" runat="server" Text='<%# Eval("Percentage_Of_Salary") %>'></asp:Label>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="<%$ Resources:Attendance,Schedule Type %>">
                                                                        <ItemTemplate>
                                                                            <asp:DropDownList ID="ddlSchType0" Enabled="false" runat="server" SelectedValue='<%# Eval("Shedule_Type") %>'
                                                                                AutoPostBack="true" OnSelectedIndexChanged="ddlScheduleGrid_SelectedIndexChanged">
                                                                                <asp:ListItem Value="Monthly">Monthly</asp:ListItem>
                                                                                <asp:ListItem Value="Yearly">Yearly</asp:ListItem>
                                                                            </asp:DropDownList>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="<%$ Resources:Attendance,IsYearCarry %>">
                                                                        <ItemTemplate>
                                                                            <asp:CheckBox ID="chkYearCarry0" Enabled="false" runat="server" Checked='<%# Eval("Is_YearCarry") %>'
                                                                                Text="<%$ Resources:Attendance,IsYearCarry %>" />
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="<%$ Resources:Attendance,Is Rule %>">
                                                                        <ItemTemplate>
                                                                            <asp:CheckBox ID="chkIsRule" runat="server" Enabled="false" Checked='<%# Eval("IsRule") %>'
                                                                                Text="<%$ Resources:Attendance,Is Rule %>" />
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="<%$ Resources:Attendance,Is Auto %>">
                                                                        <ItemTemplate>
                                                                            <asp:CheckBox ID="chkIsAuto" runat="server" Enabled="false" Checked='<%# Eval("IsAuto") %>' AutoPostBack="true" OnCheckedChanged="chkIsAuto_OnCheckedChanged"
                                                                                Text="<%$ Resources:Attendance,Is Auto %>" />
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

                    <div class="tab-pane" id="Alert" runat="server">
                        <asp:UpdatePanel ID="Update_Alert" runat="server">
                            <ContentTemplate>

                                <div class="row">
                                    <div class="col-md-12">
                                        <div class="box box-primary">
                                            <div class="box-body">
                                                <div class="form-group">
                                                    <div class="col-md-12">
                                                        <asp:RadioButton ID="rbtnGroupNF" OnCheckedChanged="EmpGroupNF_CheckedChanged" runat="server"
                                                            Text="<%$ Resources:Attendance,Group %>" Font-Bold="true" GroupName="Alert_EmpGroup"
                                                            AutoPostBack="true" />
                                                        <asp:RadioButton ID="rbtnEmpNF" Checked="true" Style="margin-left: 20px;" runat="server" AutoPostBack="true" Text="<%$ Resources:Attendance,Employee %>"
                                                            GroupName="Alert_EmpGroup" Font-Bold="true" OnCheckedChanged="EmpGroupNF_CheckedChanged" />
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>

                                <div class="col-md-12">
                                    <div id="Div_Alert_Employee" runat="server" class="row">



                                        <div class="row">
                                            <div class="col-md-12">
                                                <div id="Div6" runat="server" class="box box-info collapsed-box">
                                                    <div class="box-header with-border">
                                                        <h3 class="box-title">
                                                            <asp:Label ID="Label179" runat="server" Text="Advance Search"></asp:Label></h3>
                                                        &nbsp;&nbsp;|&nbsp;&nbsp;
					<asp:Label ID="lblTotalRecordNF" runat="server" Text="<%$ Resources:Attendance,Total Records: 0 %>"></asp:Label>

                                                        <div class="box-tools pull-right">
                                                            <button type="button" class="btn btn-box-tool" data-widget="collapse">
                                                                <i id="I5" runat="server" class="fa fa-plus"></i>
                                                            </button>
                                                        </div>
                                                    </div>
                                                    <div class="box-body">


                                                        <div class="col-md-6">
                                                            <asp:Label ID="lblLocationAlert" runat="server" Text="<%$ Resources:Attendance,Location %>"></asp:Label>
                                                            <asp:DropDownList ID="ddlLocationAlert" runat="server" CssClass="form-control"
                                                                AutoPostBack="true" OnSelectedIndexChanged="ddlLocationAlert_SelectedIndexChanged">
                                                            </asp:DropDownList>
                                                            <br />
                                                        </div>
                                                        <div class="col-md-6">
                                                            <asp:Label ID="lblDepDeptAlert" runat="server" Text="<%$ Resources:Attendance,Department %>"></asp:Label>

                                                            <asp:DropDownList ID="ddlDeptAlert" runat="server" CssClass="form-control"
                                                                AutoPostBack="true" OnSelectedIndexChanged="ddlDeptAlert_SelectedIndexChanged">
                                                            </asp:DropDownList>

                                                            <br />

                                                        </div>
                                                        <div class="col-lg-3">
                                                            <asp:DropDownList ID="ddlFieldNF" runat="server" CssClass="form-control">
                                                                <asp:ListItem Text="<%$ Resources:Attendance,Employee Code %>" Value="Emp_Code"></asp:ListItem>
                                                                <asp:ListItem Text="<%$ Resources:Attendance,Employee Name %>" Value="Emp_Name" Selected="True"></asp:ListItem>
                                                                <asp:ListItem Text="<%$ Resources:Attendance,Employee Name Local %>" Value="Emp_Name_L"></asp:ListItem>
                                                                <asp:ListItem Text="<%$ Resources:Attendance,Email Id %>" Value="Email_Id"></asp:ListItem>
                                                                <asp:ListItem Text="<%$ Resources:Attendance,Phone No. %>" Value="Phone_No"></asp:ListItem>
                                                                <asp:ListItem Text="<%$ Resources:Attendance,Civil Id %>" Value="Civil_Id"></asp:ListItem>
                                                            </asp:DropDownList>
                                                        </div>
                                                        <div class="col-lg-2">
                                                            <asp:DropDownList ID="ddlOptionNF" runat="server" CssClass="form-control">
                                                                <asp:ListItem Text="<%$ Resources:Attendance,--Select-- %>"></asp:ListItem>
                                                                <asp:ListItem Text="<%$ Resources:Attendance,Equal %>"></asp:ListItem>
                                                                <asp:ListItem Selected="True" Text="<%$ Resources:Attendance,Contains %>"></asp:ListItem>
                                                                <asp:ListItem Text="<%$ Resources:Attendance,Like %>"></asp:ListItem>
                                                            </asp:DropDownList>
                                                        </div>
                                                        <div class="col-lg-5">
                                                            <asp:Panel ID="Panel4" runat="server" DefaultButton="ImageButton6">
                                                                <asp:TextBox placeholder="Search from Content" ID="txtValueNF" runat="server" CssClass="form-control"></asp:TextBox>
                                                                <asp:RequiredFieldValidator EnableClientScript="true" runat="server" ID="RequiredFieldValidator10" ValidationGroup="A_Search" Display="Dynamic" SetFocusOnError="true" ControlToValidate="txtValueNF" ErrorMessage="<%$ Resources:Attendance,Please enter value %>" />
                                                            </asp:Panel>
                                                        </div>
                                                        <div class="col-lg-2" style="text-align: center; padding-left: 1px; padding-right: 1px;">
                                                            <asp:LinkButton ID="ImageButton6" runat="server" CausesValidation="true" ValidationGroup="A_Search" OnClick="btnNFbind_Click" ToolTip="<%$ Resources:Attendance,Search %>"><span class="fa fa-search"  style="font-size:25px;"></span></asp:LinkButton>&nbsp;&nbsp;&nbsp;
                                                            <asp:LinkButton ID="ImageButton21" runat="server" CausesValidation="False" OnClick="btnNFRefresh_Click" ToolTip="<%$ Resources:Attendance,Refresh %>"><span class="fa fa-repeat"  style="font-size:25px;"></span></asp:LinkButton>&nbsp;&nbsp;&nbsp;
                                                            <asp:LinkButton ID="ImageButton22" runat="server" OnClick="ImgbtnSelectAll_ClickNF" ToolTip="<%$ Resources:Attendance, Select All %>"><span class="fas fa-th"  style="font-size:25px;"></span></asp:LinkButton>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>

                                        <div class="box box-warning box-solid" <%= gvEmpNF.Rows.Count>0?"style='display:block'":"style='display:none'"%>>
                                            <div class="box-body">
                                                <div class="row">
                                                    <div class="col-md-12">
                                                        <div class="flow">
                                                            <asp:Label ID="lblSelectRecordNF" runat="server" Visible="false"></asp:Label>
                                                            <asp:CustomValidator ID="CustomValidator3" ValidationGroup="A_Grid_Save" runat="server" ErrorMessage="Please select at least one record."
                                                                ClientValidationFunction="Alert_Validate" ForeColor="Red"></asp:CustomValidator>

                                                            <asp:GridView CssClass="table-striped table-bordered table table-hover" ID="gvEmpNF" runat="server" AllowPaging="True" AutoGenerateColumns="False"
                                                                OnPageIndexChanging="gvEmpNF_PageIndexChanging" Width="100%"
                                                                DataKeyNames="Emp_Id" PageSize="<%# PageControlCommon.GetPageSize() %>" Visible="false">
                                                                <Columns>
                                                                    <asp:TemplateField>
                                                                        <ItemTemplate>
                                                                            <asp:CheckBox ID="chkgvSelect" runat="server" />
                                                                        </ItemTemplate>
                                                                        <HeaderTemplate>
                                                                            <asp:CheckBox ID="chkgvSelectAll" runat="server" OnCheckedChanged="chkgvSelectAll_CheckedChangedNF"
                                                                                AutoPostBack="true" />
                                                                        </HeaderTemplate>
                                                                        <ItemStyle HorizontalAlign="Center" />
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="<%$ Resources:Attendance,Edit %>">
                                                                        <ItemTemplate>
                                                                            <asp:LinkButton ID="btnEdit" runat="server" OnClientClick="show_Alert_Popup()" CausesValidation="False" Visible='<%# hdnCanEdit.Value=="true"?true:false%>' CommandArgument='<%# Eval("Emp_Id") %>' OnCommand="btnEditNF_Command"><i class="fa fa-pencil"></i></asp:LinkButton>
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
                                                                        SortExpression="Emp_Name" ItemStyle-Width="20%" />
                                                                    <asp:BoundField DataField="Emp_Name_L" HeaderText="<%$ Resources:Attendance,Employee Name Local%>"
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
                                    <div id="Div_Alert_Group" visible="false" runat="server" class="row">
                                        <div class="row">
                                            <div class="col-md-12">
                                                <div class="box box-primary">
                                                    <div class="box-body">
                                                        <div class="form-group">
                                                            <div class="col-md-6">
                                                                <asp:CustomValidator ID="CustomValidator9" ValidationGroup="A_Grid_Save" runat="server" ErrorMessage="Please select at least one record."
                                                                    ClientValidationFunction="Alert_Validate" ForeColor="Red"></asp:CustomValidator>
                                                                <asp:ListBox ID="lbxGroupNF" runat="server" OnSelectedIndexChanged="lbxGroupNF_SelectedIndexChanged" Style="width: 100%; height: 150px;" SelectionMode="Multiple"
                                                                    CssClass="form-control" Font-Names="Trebuchet MS" Font-Size="Small" ForeColor="Gray"></asp:ListBox>
                                                            </div>
                                                            <div class="col-md-12">
                                                                <div class="flow">
                                                                    <asp:GridView CssClass="table-striped table-bordered table table-hover" ID="gvEmployeeNF" runat="server" AllowPaging="True" AutoGenerateColumns="False"
                                                                        OnPageIndexChanging="gvEmployeeNF_PageIndexChanging" Width="100%"
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
                                                                    <asp:Label ID="lblEmpNf" runat="server" Visible="false"></asp:Label>
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
                                        <div class="box box-primary">
                                            <div class="box-header with-border">
                                                <h3 class="box-title">
                                                    <asp:Label ID="Label25" runat="server" ForeColor="#666666" Font-Names="arial" Font-Size="13px"
                                                        Font-Bold="true" Text="<%$ Resources:Attendance,Report %>"></asp:Label></h3>
                                            </div>
                                            <div class="box-body">
                                                <div class="form-group">
                                                    <div class="col-md-12">
                                                        <asp:CheckBoxList ID="ChkReportList" runat="server" RepeatColumns="4" Font-Names="Trebuchet MS" Width="100%"
                                                            Font-Size="Small" ForeColor="Gray">
                                                        </asp:CheckBoxList>
                                                    </div>
                                                    <br />
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                                <div style="display: none;" class="row">
                                    <div class="col-md-12">
                                        <div class="box box-primary">
                                            <div class="box-body">
                                                <div class="form-group">
                                                    <div class="col-md-12">
                                                        <asp:Label ID="Label26" runat="server" ForeColor="#666666" Font-Names="arial" Font-Size="13px"
                                                            Font-Bold="true" Text="<%$ Resources:Attendance,SMS %>"></asp:Label>
                                                        <br />
                                                        <asp:CheckBoxList ID="ChkSmsList" runat="server" RepeatColumns="4" Font-Names="Trebuchet MS" Width="100%"
                                                            Font-Size="Small" ForeColor="Gray">
                                                        </asp:CheckBoxList>
                                                        <br />

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
                                                    <div class="col-md-12">
                                                        <asp:Label ID="Label107" runat="server" ForeColor="#666666" Font-Names="arial" Font-Size="13px"
                                                            Font-Bold="true" Text="<%$ Resources:Attendance,Email %>"></asp:Label>
                                                        <br />
                                                        <asp:CheckBoxList ID="chkEmailList" runat="server" RepeatColumns="4" Font-Names="Trebuchet MS" Width="100%"
                                                            Font-Size="Small" ForeColor="Gray">
                                                        </asp:CheckBoxList>
                                                        <br />
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
                                                    <div class="col-md-12">
                                                        <div class="col-md-6"></div>
                                                        <div class="col-md-6">
                                                            <asp:Label ID="Label111" runat="server" Text="Schedule Days"></asp:Label><a style="color: Red">*</a>
                                                            <asp:RequiredFieldValidator EnableClientScript="true" Style="float: right;" runat="server" ID="RequiredFieldValidator20" ValidationGroup="A_Grid_Save"
                                                                Display="Dynamic" SetFocusOnError="true" ControlToValidate="txtScheduleDays" ErrorMessage="<%$ Resources:Attendance,Enter Schedule Days %>" />
                                                            <asp:TextBox ID="txtScheduleDays" runat="server" CssClass="form-control"></asp:TextBox>
                                                            <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender24" runat="server" Enabled="True"
                                                                TargetControlID="txtScheduleDays" ValidChars="1,2,3,4,5,6,7,8,9,0">
                                                            </cc1:FilteredTextBoxExtender>
                                                            <br />
                                                        </div>
                                                    </div>

                                                    <div class="col-md-12" style="text-align: center;">
                                                        <asp:Button ID="Btn_Alert_Save" runat="server" CssClass="btn btn-success" ValidationGroup="A_Grid_Save" CausesValidation="true" OnClick="btnSaveNF_Click"
                                                            Text="<%$ Resources:Attendance,Save %>" Visible="false" />

                                                        <asp:Button ID="Btn_Alert_Reset" runat="server" CausesValidation="False" CssClass="btn btn-primary"
                                                            OnClick="btnResetNF_Click" Text="<%$ Resources:Attendance,Reset %>" />

                                                        <asp:Button ID="Btn_Alert_Cancel" runat="server" CausesValidation="False" CssClass="btn btn-danger"
                                                            OnClick="btnCancelNF_Click" Text="<%$ Resources:Attendance,Cancel %>" />
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </div>

                    <div class="tab-pane" id="Salary" runat="server">
                        <asp:UpdatePanel ID="Update_Salary" runat="server">
                            <ContentTemplate>
                                <div class="row">
                                    <div class="col-md-12">
                                        <div class="box box-primary">
                                            <div class="box-body">
                                                <div class="form-group">
                                                    <div class="col-md-12">
                                                        <asp:RadioButton ID="rbtnGroupSal" OnCheckedChanged="EmpGroup_CheckedChanged" runat="server"
                                                            Text="<%$ Resources:Attendance,Group %>" Font-Bold="true" GroupName="EmpGroup_Sal"
                                                            AutoPostBack="true" />
                                                        <asp:RadioButton ID="rbtnEmpSal" Checked="true" Style="margin-left: 20px;" runat="server" AutoPostBack="true" Text="<%$ Resources:Attendance,Employee %>"
                                                            GroupName="EmpGroup_Sal" Font-Bold="true" OnCheckedChanged="EmpGroup_CheckedChanged" />
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>

                                <div class="col-md-12">
                                    <div id="pnlEmpSal" runat="server" class="row">
                                        <div class="row">
                                            <div class="col-md-12">
                                                <div id="Div7" runat="server" class="box box-info collapsed-box">
                                                    <div class="box-header with-border">
                                                        <h3 class="box-title">
                                                            <asp:Label ID="Label180" runat="server" Text="Advance Search"></asp:Label></h3>
                                                        &nbsp;&nbsp;|&nbsp;&nbsp;
					<asp:Label ID="lblTotalRecordSal" runat="server" Text="<%$ Resources:Attendance,Total Records: 0 %>"></asp:Label>

                                                        <div class="box-tools pull-right">
                                                            <button type="button" class="btn btn-box-tool" data-widget="collapse">
                                                                <i id="I6" runat="server" class="fa fa-plus"></i>
                                                            </button>
                                                        </div>
                                                    </div>
                                                    <div class="box-body">

                                                        <div class="col-md-6">
                                                            <asp:Label ID="lblLocationSalary" runat="server" Text="<%$ Resources:Attendance,Location %>"></asp:Label>
                                                            <asp:DropDownList ID="ddlLocationSalary" runat="server" CssClass="form-control"
                                                                AutoPostBack="true" OnSelectedIndexChanged="ddlLocationSalary_SelectedIndexChanged">
                                                            </asp:DropDownList>
                                                            <br />
                                                        </div>
                                                        <div class="col-md-6">
                                                            <asp:Label ID="lblDeptSalary" runat="server" Text="<%$ Resources:Attendance,Department %>"></asp:Label>
                                                            <asp:DropDownList ID="ddlDeptSalary" runat="server" CssClass="form-control"
                                                                AutoPostBack="true" OnSelectedIndexChanged="ddlDeptSalary_SelectedIndexChanged">
                                                            </asp:DropDownList>
                                                            <br />
                                                        </div>

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
                                                            <asp:Panel ID="Panel5" runat="server" DefaultButton="btn_Salarybind">
                                                                <asp:TextBox ID="txtValueSal" placeholder="Search from Content" runat="server" CssClass="form-control"></asp:TextBox>
                                                                <asp:RequiredFieldValidator EnableClientScript="true" runat="server" ID="RequiredFieldValidator11" ValidationGroup="S_Search" Display="Dynamic" SetFocusOnError="true" ControlToValidate="txtValueSal" ErrorMessage="<%$ Resources:Attendance,Please enter value %>" />
                                                            </asp:Panel>
                                                        </div>
                                                        <div class="col-lg-2" style="text-align: center; padding-left: 1px; padding-right: 1px;">
                                                            <asp:LinkButton ID="btn_Salarybind" runat="server" CausesValidation="true" ValidationGroup="S_Search" OnClick="btnSalarybind_Click" ToolTip="<%$ Resources:Attendance,Search %>"><span class="fa fa-search" style="font-size: 25px;"></span></asp:LinkButton>&nbsp;&nbsp;&nbsp;                                                            
                                                            <asp:LinkButton ID="btn_SalaryRefresh" runat="server" CausesValidation="False" OnClick="btnSalaryRefresh_Click" ToolTip="<%$ Resources:Attendance,Refresh %>"><span class="fa fa-repeat"  style="font-size:25px;"></span></asp:LinkButton>&nbsp;&nbsp;&nbsp;
                                                            <asp:LinkButton ID="Img_btnSelectAll" runat="server" OnClick="ImgbtnSelectAll_ClickSalary" ToolTip="<%$ Resources:Attendance, Select All %>"><span class="fas fa-th"  style="font-size:25px;"></span></asp:LinkButton>
                                                        </div>

                                                    </div>
                                                </div>
                                            </div>
                                        </div>


                                        <div class="box box-warning box-solid" <%= gvEmpSalary.Rows.Count>0?"style='display:block'":"style='display:none'"%>>
                                            <div class="box-body">
                                                <div class="row">
                                                    <div class="col-md-12">
                                                        <div class="flow">
                                                            <asp:Label ID="lblSelectRecordSal" runat="server" Visible="false"></asp:Label>
                                                            <asp:CustomValidator ID="CustomValidator4" ValidationGroup="S_Save" runat="server" ErrorMessage="Please select at least one record."
                                                                ClientValidationFunction="Salary_Validate" ForeColor="Red"></asp:CustomValidator>

                                                            <asp:GridView CssClass="table-striped table-bordered table table-hover" ID="gvEmpSalary" runat="server" AllowPaging="True" AutoGenerateColumns="False"
                                                                OnPageIndexChanging="gvEmpSal_PageIndexChanging" Width="100%"
                                                                DataKeyNames="Emp_Id" PageSize="<%# PageControlCommon.GetPageSize() %>">
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
                                                                    <asp:TemplateField HeaderText="<%$ Resources:Attendance,Edit %>">
                                                                        <ItemTemplate>
                                                                            <asp:LinkButton ID="btnEdit" Visible='<%# hdnCanEdit.Value=="true"?true:false%>' OnClientClick="showPopup()" runat="server" CausesValidation="False" CommandArgument='<%# Eval("Emp_Id") %>' OnCommand="btnEditSalary_Command"><i class="fa fa-pencil"></i></asp:LinkButton>
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
                                    <div id="pnlGroupSal" visible="false" runat="server" class="row">
                                        <div class="row">
                                            <div class="col-md-12">
                                                <div class="box box-primary">
                                                    <div class="box-body">
                                                        <div class="form-group">
                                                            <div class="col-md-6">
                                                                <asp:CustomValidator ID="CustomValidator10" ValidationGroup="S_Save" runat="server" ErrorMessage="Please select at least one record."
                                                                    ClientValidationFunction="Salary_Validate" ForeColor="Red"></asp:CustomValidator>

                                                                <asp:ListBox ID="lbxGroupSal" runat="server" OnSelectedIndexChanged="lbxGroupSal_SelectedIndexChanged" AutoPostBack="true" Style="width: 100%; height: 150px;" SelectionMode="Multiple"
                                                                    CssClass="form-control" Font-Names="Trebuchet MS" Font-Size="Small" ForeColor="Gray"></asp:ListBox>
                                                            </div>
                                                            <div class="col-md-12">
                                                                <div class="flow">
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
                                                                            <asp:BoundField DataField="Emp_Name_L" HeaderText="<%$ Resources:Attendance,Employee Name Local %>"
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
                                                                    <asp:Label ID="Lbl_gvEmployeeSal" runat="server" Visible="false"></asp:Label>
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
                                        <div class="box box-primary">
                                            <div class="box-body">
                                                <div class="form-group">
                                                    <div class="col-md-12">
                                                        <div class="col-md-6">
                                                            <asp:Label ID="Label27" runat="server" Text="<%$ Resources:Attendance,Basic Salary %>"></asp:Label><a style="color: Red">*</a>
                                                            <asp:RequiredFieldValidator EnableClientScript="true" Style="float: right;" runat="server" ID="RequiredFieldValidator14" ValidationGroup="S_Save" Display="Dynamic" SetFocusOnError="true" ControlToValidate="txtBasic" ErrorMessage="<%$ Resources:Attendance,Enter Basic Salary %>" />
                                                            <asp:TextBox ID="txtBasic" CssClass="form-control" runat="server"></asp:TextBox>
                                                            <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender2" runat="server" Enabled="True"
                                                                TargetControlID="txtBasic" ValidChars="1,2,3,4,5,6,7,8,9,0,.">
                                                            </cc1:FilteredTextBoxExtender>
                                                            <br />
                                                        </div>
                                                        <div id="Div_Salary_Plan" runat="server" class="col-md-6">
                                                            <asp:Label ID="Label129" runat="server" Text="Salary Plan"></asp:Label><a style="color: Red">*</a>
                                                            <asp:RequiredFieldValidator EnableClientScript="true" Style="float: right;" runat="server" ID="RequiredFieldValidator26" ValidationGroup="S_Save"
                                                                Display="Dynamic" SetFocusOnError="true" ControlToValidate="ddlSalaryPlan" InitialValue="--Select--" ErrorMessage="<%$ Resources:Attendance,Select Salary Plan%>" />
                                                            <asp:DropDownList ID="ddlSalaryPlan" runat="server" CssClass="form-control"></asp:DropDownList>
                                                            <br />
                                                        </div>
                                                        <div class="col-md-6">
                                                            <asp:Label ID="Label31" runat="server" Text="<%$ Resources:Attendance,Work Minute %>"></asp:Label><a style="color: Red">*</a>
                                                            <asp:RequiredFieldValidator EnableClientScript="true" Style="float: right;" runat="server" ID="RequiredFieldValidator15" ValidationGroup="S_Save"
                                                                Display="Dynamic" SetFocusOnError="true" ControlToValidate="txtWorkMinute" ErrorMessage="<%$ Resources:Attendance,Enter Work Minute %>" />
                                                            <asp:TextBox ID="txtWorkMinute" CssClass="form-control" MaxLength="4" runat="server"></asp:TextBox>
                                                            <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender3" runat="server" Enabled="True"
                                                                TargetControlID="txtWorkMinute" ValidChars="1,2,3,4,5,6,7,8,9,0,.">
                                                            </cc1:FilteredTextBoxExtender>
                                                            <br />
                                                        </div>
                                                        <div class="col-md-6">
                                                            <asp:Label ID="Label28" runat="server" Text="<%$ Resources:Attendance,Payment Type %>"></asp:Label><a style="color: Red">*</a>
                                                            <asp:DropDownList ID="ddlPayment" runat="server" CssClass="form-control">
                                                                <%--  <asp:ListItem Value="Hourly">Hourly</asp:ListItem>
                                                                        <asp:ListItem Value="Daily">Daily</asp:ListItem>
                                                                        <asp:ListItem Value="Weekly">Weekly</asp:ListItem>--%>
                                                                <asp:ListItem Value="Monthly">Monthly</asp:ListItem>
                                                            </asp:DropDownList>
                                                            <br />
                                                        </div>
                                                        <div style="display: none;" class="col-md-6">
                                                            <asp:Label ID="Label29" runat="server" Text="<%$ Resources:Attendance,Currency %>"></asp:Label>
                                                            <asp:DropDownList ID="ddlCurrency" runat="server" CssClass="form-control"></asp:DropDownList>
                                                            <br />
                                                        </div>
                                                        <div class="col-md-6">
                                                            <asp:Label ID="Label32" runat="server" Text="<%$ Resources:Attendance,Work Calculation Method %>"></asp:Label><a style="color: Red">*</a>
                                                            <asp:DropDownList ID="ddlWorkCalMethod" runat="server" CssClass="form-control">
                                                                <asp:ListItem Value="PairWise">PairWise</asp:ListItem>
                                                                <asp:ListItem Value="InOut">InOut</asp:ListItem>
                                                            </asp:DropDownList>
                                                            <br />
                                                        </div>
                                                        <div class="col-md-6">
                                                            <asp:Label ID="Label11" runat="server" Text="Mobile Bill Limit"></asp:Label>
                                                            <asp:TextBox ID="txtMobilleBillLimit" CssClass="form-control" MaxLength="8" runat="server"></asp:TextBox>
                                                            <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender20" runat="server" Enabled="True"
                                                                TargetControlID="txtMobilleBillLimit" ValidChars="1,2,3,4,5,6,7,8,9,0,.">
                                                            </cc1:FilteredTextBoxExtender>
                                                            <br />
                                                        </div>
                                                        <div class="col-md-6">
                                                            <asp:Label ID="Label162" runat="server" Text="<%$ Resources:Attendance,Salary Payment Option%>"></asp:Label>
                                                            <asp:TextBox ID="Txt_Salary_Payment_Option" runat="server" CssClass="form-control" BackColor="#eeeeee"
                                                                AutoPostBack="true" OnTextChanged="Txt_Salary_Payment_Option_TextChanged"></asp:TextBox>
                                                            <cc1:AutoCompleteExtender ID="AutoCompleteExtender4" runat="server" DelimiterCharacters=""
                                                                Enabled="True" ServiceMethod="GetCompletionListAccountName" ServicePath="" CompletionInterval="100"
                                                                MinimumPrefixLength="1" CompletionSetCount="1" TargetControlID="Txt_Salary_Payment_Option"
                                                                UseContextKey="True" CompletionListCssClass="autoCompleteList" CompletionListItemCssClass="autoCompleteListItem" CompletionListHighlightedItemCssClass="autoCompleteSelectedListItem" OnClientShown="resetPosition">
                                                            </cc1:AutoCompleteExtender>
                                                            <br />
                                                        </div>
                                                        <div class="col-md-12">
                                                            <div class="col-md-6">
                                                                <br />
                                                                <asp:CheckBox ID="chkisCtcEmployee" runat="server" />
                                                                <asp:Label ID="Label131" runat="server" Text="Is Ctc Employee"></asp:Label>
                                                            </div>
                                                            <div class="col-md-6">
                                                                <br />
                                                                <asp:CheckBox ID="chkEmpINPayroll" runat="server" AutoPostBack="true" OnCheckedChanged="chkEmpINPayroll_OnCheckedChanged" />
                                                                <asp:Label ID="Label30" runat="server" Text="<%$ Resources:Attendance,Employee In Payroll %>"></asp:Label>
                                                            </div>
                                                            <div class="col-md-6">
                                                                <br />
                                                                <asp:CheckBox ID="chkEmpPf" runat="server" />
                                                                <asp:Label ID="Label80" runat="server" Text="<%$ Resources:Attendance,Employee PF %>"></asp:Label>
                                                                <br />
                                                            </div>
                                                            <div class="col-md-6">
                                                                <br />
                                                                <asp:CheckBox ID="chkEmpEsic" runat="server" />
                                                                <asp:Label ID="Label81" runat="server" Text="<%$ Resources:Attendance,Employee ESIC %>"></asp:Label>
                                                                <br />
                                                            </div>
                                                        </div>
                                                    </div>
                                                    <div class="col-md-12">
                                                        <hr />
                                                        <div id="Div_Box_AddSalary" class="box box-info collapsed-box">
                                                            <asp:Label ID="Label87" runat="server" Text="<%$ Resources:Attendance,Percentage of salary increment %>" Visible="false"></asp:Label>
                                                            <div class="box-header with-border">
                                                                <h3 class="box-title">Salary Increment</h3>
                                                                <div class="box-tools pull-right">
                                                                    <button type="button" class="btn btn-box-tool" data-widget="collapse">
                                                                        <i id="Btn_AddSalary_Div" class="fa fa-plus"></i>
                                                                    </button>
                                                                </div>
                                                            </div>
                                                            <div class="box-body">
                                                                <div class="form-group">
                                                                    <div class="col-md-6">
                                                                        <asp:Label ID="Label96" runat="server" Text="<%$ Resources:Attendance,Category %>"></asp:Label>
                                                                        <asp:DropDownList ID="ddlCategory" runat="server" CssClass="form-control"
                                                                            AutoPostBack="true" OnSelectedIndexChanged="ddlCategory_OnSelectedIndexChanged">
                                                                            <asp:ListItem Text="<%$ Resources:Attendance,Fresher %>" Value="Fresher"></asp:ListItem>
                                                                            <asp:ListItem Text="<%$ Resources:Attendance,Experience %>" Value="Experience"></asp:ListItem>
                                                                        </asp:DropDownList>
                                                                        <br />
                                                                    </div>
                                                                    <div class="col-md-12"></div>
                                                                    <div class="col-md-6">
                                                                        <asp:Label ID="lblDurOfSal" runat="server" Text="<%$ Resources:Attendance,Duration of salary increment %>"></asp:Label>
                                                                        <div class="input-group">
                                                                            <asp:TextBox ID="txtSalIncrDuration" MaxLength="2" CssClass="form-control" runat="server" />
                                                                            <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender8" runat="server" Enabled="True"
                                                                                TargetControlID="txtSalIncrDuration" ValidChars="1,2,3,4,5,6,7,8,9,0">
                                                                            </cc1:FilteredTextBoxExtender>
                                                                            <%--<asp:RangeValidator ID="val" runat="server" ErrorMessage="Enter Month Between 1 to 12" SetFocusOnError="true" ValidationGroup="S_Save"
                                                                                    ControlToValidate="txtSalIncrDuration" Display="Dynamic" ForeColor="red" Type="Integer"
                                                                                    MinimumValue="0" MaximumValue="1">
                                                                                </asp:RangeValidator>--%>
                                                                            <span class="input-group-addon">
                                                                                <asp:Label ID="Label90" runat="server" Text="<%$ Resources:Attendance,In Months %>"></asp:Label></span>
                                                                        </div>
                                                                        <br />
                                                                    </div>
                                                                    <div class="col-md-12"></div>
                                                                    <div class="col-md-6">
                                                                        <asp:Label ID="Label88" runat="server" Text="<%$ Resources:Attendance,From %>"></asp:Label>
                                                                        <div class="input-group">
                                                                            <asp:TextBox ID="txtIncrementPerFrom" MaxLength="2" CssClass="form-control" runat="server" />
                                                                            <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender25" runat="server" Enabled="True"
                                                                                TargetControlID="txtIncrementPerFrom" ValidChars="1,2,3,4,5,6,7,8,9,0">
                                                                            </cc1:FilteredTextBoxExtender>
                                                                            <span class="input-group-addon">In %</span>
                                                                        </div>
                                                                        <br />

                                                                    </div>
                                                                    <div class="col-md-6">
                                                                        <asp:Label ID="Label89" runat="server" Text="<%$ Resources:Attendance,To %>"></asp:Label>
                                                                        <div class="input-group">
                                                                            <asp:TextBox ID="txtIncrementPerTo" MaxLength="2" CssClass="form-control" runat="server" />
                                                                            <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender27" runat="server" Enabled="True"
                                                                                TargetControlID="txtIncrementPerTo" ValidChars="1,2,3,4,5,6,7,8,9,0">
                                                                            </cc1:FilteredTextBoxExtender>
                                                                            <span class="input-group-addon">In %</span>
                                                                        </div>
                                                                        <br />
                                                                    </div>
                                                                </div>
                                                            </div>
                                                        </div>
                                                    </div>

                                                    <%-- Indemnity/Gratuity Disable by Ghanshyam Suthar on 15-11-2017 according by Neelkanth Sir--%>
                                                    <div class="col-md-12">
                                                        <div class="col-md-4"></div>
                                                        <div id="Div_Box_AddIndemnity" style="display: none" class="col-md-4 box box-info collapsed-box">
                                                            <div class="box-header with-border">
                                                                <h3 class="box-title">Indemnity/Gratuity</h3>
                                                                <div class="box-tools pull-right">
                                                                    <button type="button" class="btn btn-box-tool" data-widget="collapse">
                                                                        <i id="Btn_AddIndemnity_Div" class="fa fa-plus"></i>
                                                                    </button>
                                                                </div>
                                                            </div>
                                                            <div class="box-body">
                                                                <div class="form-group">
                                                                    <asp:Label ID="Label117" Font-Bold="true" Font-Size="13px" Font-Names="Arial" runat="server"
                                                                        Text="<%$ Resources:Attendance,Indemnity %>" Visible="false"></asp:Label>

                                                                    <asp:Label ID="Label118" runat="server" Text="<%$ Resources:Attendance,Indemnity Functionality %>"></asp:Label>
                                                                    <asp:RadioButton ID="rbnIndemnity1" GroupName="Indemnity" Text="<%$ Resources:Attendance,Enable %>"
                                                                        runat="server" AutoPostBack="true" OnCheckedChanged="rbnIndemnity1_OnCheckedChanged" />
                                                                    <asp:RadioButton ID="rbnIndemnity2" Style="margin: 15px;" GroupName="Indemnity" Text="<%$ Resources:Attendance,Disable %>"
                                                                        runat="server" AutoPostBack="true" OnCheckedChanged="rbnIndemnity2_OnCheckedChanged" />
                                                                    <br />
                                                                    <asp:Label ID="Label114" runat="server" Text="<%$ Resources:Attendance,Indemnity Duration(Year)%>"></asp:Label>
                                                                    <asp:TextBox ID="txtIndemnityYear" runat="server" CssClass="form-control"></asp:TextBox>
                                                                    <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender31" runat="server" TargetControlID="txtIndemnityYear"
                                                                        FilterType="Numbers">
                                                                    </cc1:FilteredTextBoxExtender>
                                                                    <br />
                                                                    <asp:Label ID="Label116" runat="server" Text="<%$ Resources:Attendance,Given Leave Salary Days(Year)%>"></asp:Label>
                                                                    <asp:TextBox ID="txtIndemnityDays" runat="server" CssClass="form-control" MaxLength="4"></asp:TextBox>
                                                                    <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender33" runat="server" Enabled="True"
                                                                        TargetControlID="txtIndemnityDays" FilterType="Numbers">
                                                                    </cc1:FilteredTextBoxExtender>
                                                                    <br />
                                                                </div>
                                                            </div>
                                                            <div class="col-md-4"></div>
                                                        </div>

                                                        <div class="row">
                                                            <div class="col-md-12">
                                                                <div id="Div_Box_Add" class="box box-info collapsed-box">
                                                                    <div class="box-header with-border">
                                                                        <h3 class="box-title">Add Previous Employer and Opening Balance information</h3>
                                                                        <div class="box-tools pull-right">
                                                                            <button type="button" class="btn btn-box-tool" data-widget="collapse">
                                                                                <i id="Btn_Add_Div" class="fa fa-plus"></i>
                                                                            </button>
                                                                        </div>
                                                                    </div>
                                                                    <div class="box-body">
                                                                        <div class="form-group">
                                                                            <div class="col-md-6">
                                                                                <asp:Label ID="Label58" runat="server" Text="Opening Credit Amount"></asp:Label>


                                                                                <asp:TextBox ID="txtOpeningCreditAmt" runat="server" Enabled="false" CssClass="form-control" OnBlur="SetOpeningBalance();"></asp:TextBox>
                                                                                <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender26" runat="server" Enabled="True"
                                                                                    TargetControlID="txtOpeningCreditAmt" ValidChars="1,2,3,4,5,6,7,8,9,0,.">
                                                                                </cc1:FilteredTextBoxExtender>
                                                                                <br />
                                                                            </div>
                                                                            <div class="col-md-6">
                                                                                <asp:Label ID="Label60" runat="server" Text="Opening Debit Amount"></asp:Label>

                                                                                <asp:TextBox ID="txtOpeningDebitAmt" Enabled="false" runat="server" CssClass="form-control" OnBlur="SetOpeningBalance();"></asp:TextBox>
                                                                                <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" runat="server" Enabled="True"
                                                                                    TargetControlID="txtOpeningDebitAmt" ValidChars="1,2,3,4,5,6,7,8,9,0,.">
                                                                                </cc1:FilteredTextBoxExtender>
                                                                                <br />
                                                                            </div>

                                                                            <div class="col-md-6">
                                                                                <asp:Label ID="Label93" runat="server" Text="Total Earning (Current Financial Year)"></asp:Label>
                                                                                <asp:TextBox ID="txtEmployerTotalEarning" Enabled="false" runat="server" CssClass="form-control"></asp:TextBox>
                                                                                <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender28" runat="server" Enabled="True"
                                                                                    TargetControlID="txtEmployerTotalEarning" ValidChars="1,2,3,4,5,6,7,8,9,0">
                                                                                </cc1:FilteredTextBoxExtender>
                                                                                <br />
                                                                            </div>
                                                                            <div class="col-md-6">
                                                                                <asp:Label ID="Label133" runat="server" Text="Total TDS (Current Financial Year)"></asp:Label>

                                                                                <asp:TextBox ID="txttOtalTDS" Enabled="false" runat="server" CssClass="form-control"></asp:TextBox>
                                                                                <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender35" runat="server" Enabled="True"
                                                                                    TargetControlID="txttOtalTDS" ValidChars="1,2,3,4,5,6,7,8,9,0">
                                                                                </cc1:FilteredTextBoxExtender>
                                                                                <br />
                                                                            </div>

                                                                        </div>
                                                                    </div>
                                                                </div>
                                                            </div>
                                                        </div>
                                                        <div class="col-md-12" style="text-align: center;">
                                                            <asp:Button ID="btnSaveSal" runat="server" CssClass="btn btn-success" OnClick="btnSaveSal_Click"
                                                                Text="<%$ Resources:Attendance,Save %>" ValidationGroup="S_Save" Visible="false" />

                                                            <asp:Button ID="btnResetSal" runat="server" CausesValidation="False" CssClass="btn btn-primary"
                                                                OnClick="btnResetSal_Click" Text="<%$ Resources:Attendance,Reset %>" />

                                                            <asp:Button ID="btnCancelSal" runat="server" CausesValidation="False" CssClass="btn btn-danger"
                                                                OnClick="btnCancelSal_Click" Text="<%$ Resources:Attendance,Cancel %>" />
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


                    <div class="tab-pane" id="OT_PL" runat="server">
                        <asp:UpdatePanel ID="Update_OT_PL" runat="server">
                            <ContentTemplate>
                                <div class="row">
                                    <div class="col-md-12">
                                        <div class="box box-primary">
                                            <div class="box-body">
                                                <div class="form-group">
                                                    <div class="col-md-12">
                                                        <asp:RadioButton ID="rbtnGroupOT" OnCheckedChanged="EmpGroupOT_CheckedChanged" runat="server"
                                                            Text="<%$ Resources:Attendance,Group %>" Font-Bold="true" GroupName="EmpGroup_OTPL"
                                                            AutoPostBack="true" />
                                                        <asp:RadioButton ID="rbtnEmpOT" Checked="true" Style="margin-left: 20px;" runat="server" AutoPostBack="true" Text="<%$ Resources:Attendance,Employee %>"
                                                            GroupName="EmpGroup_OTPL" Font-Bold="true" OnCheckedChanged="EmpGroupOT_CheckedChanged" />
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                                <div class="col-md-12">
                                    <div id="pnlEmpOT" runat="server" class="row">


                                        <div class="row">
                                            <div class="col-md-12">
                                                <div id="Div8" runat="server" class="box box-info collapsed-box">
                                                    <div class="box-header with-border">
                                                        <h3 class="box-title">
                                                            <asp:Label ID="Label181" runat="server" Text="Advance Search"></asp:Label></h3>
                                                        &nbsp;&nbsp;|&nbsp;&nbsp;
					 <asp:Label ID="lblTotalRecordOT" runat="server" Text="<%$ Resources:Attendance,Total Records: 0 %>"></asp:Label>

                                                        <div class="box-tools pull-right">
                                                            <button type="button" class="btn btn-box-tool" data-widget="collapse">
                                                                <i id="I7" runat="server" class="fa fa-plus"></i>
                                                            </button>
                                                        </div>
                                                    </div>
                                                    <div class="box-body">
                                                        <div class="col-md-6">
                                                            <asp:Label ID="lblLocationOTPL" runat="server" Text="<%$ Resources:Attendance,Location %>"></asp:Label>
                                                            <asp:DropDownList ID="ddlLocationOTPL" runat="server" CssClass="form-control"
                                                                AutoPostBack="true" OnSelectedIndexChanged="ddlLocationOTPL_SelectedIndexChanged">
                                                            </asp:DropDownList>
                                                            <br />
                                                        </div>
                                                        <div class="col-md-6">
                                                            <asp:Label ID="lblGroupByDept" runat="server" Text="<%$ Resources:Attendance,Department %>"></asp:Label>

                                                            <asp:DropDownList ID="ddlDepartmentOTPL" runat="server" CssClass="form-control"
                                                                AutoPostBack="true" OnSelectedIndexChanged="ddlDepartmentOTPL_SelectedIndexChanged">
                                                            </asp:DropDownList>
                                                            <br />
                                                        </div>
                                                        <div class="col-lg-3">
                                                            <asp:DropDownList ID="ddlFieldOT" runat="server" CssClass="form-control">
                                                                <asp:ListItem Text="<%$ Resources:Attendance,Employee Code %>" Value="Emp_Code"></asp:ListItem>
                                                                <asp:ListItem Text="<%$ Resources:Attendance,Employee Name %>" Value="Emp_Name" Selected="True"></asp:ListItem>
                                                                <asp:ListItem Text="<%$ Resources:Attendance,Employee Name Local %>" Value="Emp_Name_L"></asp:ListItem>
                                                                <asp:ListItem Text="<%$ Resources:Attendance,Email Id %>" Value="Email_Id"></asp:ListItem>
                                                                <asp:ListItem Text="<%$ Resources:Attendance,Phone No. %>" Value="Phone_No"></asp:ListItem>
                                                                <asp:ListItem Text="<%$ Resources:Attendance,Civil Id %>" Value="Civil_Id"></asp:ListItem>
                                                            </asp:DropDownList>
                                                        </div>
                                                        <div class="col-lg-2">
                                                            <asp:DropDownList ID="ddlOptionOT" runat="server" CssClass="form-control">
                                                                <asp:ListItem Text="<%$ Resources:Attendance,--Select-- %>"></asp:ListItem>
                                                                <asp:ListItem Text="<%$ Resources:Attendance,Equal %>"></asp:ListItem>
                                                                <asp:ListItem Selected="True" Text="<%$ Resources:Attendance,Contains %>"></asp:ListItem>
                                                                <asp:ListItem Text="<%$ Resources:Attendance,Like %>"></asp:ListItem>
                                                            </asp:DropDownList>
                                                        </div>
                                                        <div class="col-lg-5">
                                                            <asp:Panel ID="Panel6" runat="server" DefaultButton="btnOTbind">
                                                                <asp:TextBox ID="txtValueOT" runat="server" placeholder="Search from Content" CssClass="form-control"></asp:TextBox>
                                                                <asp:RequiredFieldValidator EnableClientScript="true" runat="server" ID="RequiredFieldValidator12" ValidationGroup="OT_Search" Display="Dynamic" SetFocusOnError="true" ControlToValidate="txtValueOT" ErrorMessage="<%$ Resources:Attendance,Please enter value %>" />
                                                            </asp:Panel>
                                                        </div>
                                                        <div class="col-lg-2" style="text-align: center; padding-left: 1px; padding-right: 1px;">
                                                            <asp:LinkButton ID="btnOTbind" runat="server" CausesValidation="true" ValidationGroup="OT_Search" OnClick="btnOTbind_Click" ToolTip="<%$ Resources:Attendance,Search %>"><span class="fa fa-search"  style="font-size:25px;"></span></asp:LinkButton>&nbsp;&nbsp;&nbsp;
                                                            <asp:LinkButton ID="btnOTRefresh" runat="server" CausesValidation="False" OnClick="btnOTRefresh_Click" ToolTip="<%$ Resources:Attendance,Refresh %>"><span class="fa fa-repeat"  style="font-size:25px;"></span></asp:LinkButton>&nbsp;&nbsp;&nbsp;
                                                            <asp:LinkButton ID="ImgbtnSelectAll_OT" runat="server" OnClick="ImgbtnSelectAll_ClickOT" ToolTip="<%$ Resources:Attendance, Select All %>"><span class="fas fa-th"  style="font-size:25px;"></span></asp:LinkButton>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>

                                        <div class="box box-warning box-solid" <%= gvEmpOT.Rows.Count>0?"style='display:block'":"style='display:none'"%>>
                                            <div class="box-body">
                                                <div class="row">
                                                    <div class="col-md-12">
                                                        <div class="flow">
                                                            <asp:Label ID="lblSelectRecordOT" runat="server" Visible="false"></asp:Label>

                                                            <asp:CustomValidator ID="CustomValidator5" ValidationGroup="OTPL_Grid_Save" runat="server" ErrorMessage="Please select at least one record."
                                                                ClientValidationFunction="OTPL_Validate" ForeColor="Red"></asp:CustomValidator>

                                                            <asp:GridView CssClass="table-striped table-bordered table table-hover" ID="gvEmpOT" runat="server" AllowPaging="True" AutoGenerateColumns="False"
                                                                OnPageIndexChanging="gvEmpOT_PageIndexChanging" Width="100%"
                                                                DataKeyNames="Emp_Id" PageSize="<%# PageControlCommon.GetPageSize() %>" Visible="false">
                                                                <Columns>
                                                                    <asp:TemplateField>
                                                                        <ItemTemplate>
                                                                            <asp:CheckBox ID="chkgvSelect" runat="server" />
                                                                        </ItemTemplate>
                                                                        <HeaderTemplate>
                                                                            <asp:CheckBox ID="chkgvSelectAll" runat="server" OnCheckedChanged="chkgvSelectAll_CheckedChangedOT"
                                                                                AutoPostBack="true" />
                                                                        </HeaderTemplate>
                                                                        <ItemStyle HorizontalAlign="Center" />
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="<%$ Resources:Attendance,Edit %>">
                                                                        <ItemTemplate>
                                                                            <asp:LinkButton ID="btnEdit" runat="server" OnClientClick="show_OTPL_Popup()" CausesValidation="False" CommandArgument='<%# Eval("Emp_Id") %>' Visible='<%# hdnCanEdit.Value=="true"?true:false%>' OnCommand="btnEditOT_Command"><i class="fa fa-pencil"></i></asp:LinkButton>
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
                                                                        SortExpression="Emp_Name" ItemStyle-Width="20%" />
                                                                    <asp:BoundField DataField="Emp_Name_L" HeaderText="<%$ Resources:Attendance,Employee Name Local%>"
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
                                    <div id="pnlGroupOT" visible="false" runat="server" class="row">
                                        <div class="row">
                                            <div class="col-md-12">
                                                <div class="box box-primary">
                                                    <div class="box-body">
                                                        <div class="form-group">
                                                            <div class="col-md-6">
                                                                <asp:CustomValidator ID="CustomValidator11" ValidationGroup="OTPL_Grid_Save" runat="server" ErrorMessage="Please select at least one record."
                                                                    ClientValidationFunction="OTPL_Validate" ForeColor="Red"></asp:CustomValidator>

                                                                <asp:ListBox ID="lbxGroupOT" runat="server" OnSelectedIndexChanged="lbxGroupOT_SelectedIndexChanged" Style="width: 100%; height: 150px;" SelectionMode="Multiple"
                                                                    CssClass="form-control" Font-Names="Trebuchet MS" Font-Size="Small" ForeColor="Gray"></asp:ListBox>
                                                            </div>
                                                            <div class="col-md-12">
                                                                <div class="flow">
                                                                    <asp:GridView CssClass="table-striped table-bordered table table-hover" ID="gvEmployeeOT" runat="server" AllowPaging="True" AutoGenerateColumns="False"
                                                                        OnPageIndexChanging="gvEmployeeOT_PageIndexChanging" Width="100%"
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
                                                                    <asp:Label ID="lblEmpOT" runat="server" Visible="false"></asp:Label>
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
                                        <div class="box box-primary">
                                            <div class="box-body">
                                                <div class="form-group">
                                                    <div class="col-md-12">

                                                        <div class="col-md-6">
                                                            <asp:Label ID="Label48" Font-Bold="true" Font-Size="13px" Font-Names="Arial" runat="server"
                                                                Text="<%$ Resources:Attendance,Partial Leave %>"></asp:Label>
                                                            <br />
                                                            <br />
                                                            <asp:Label ID="Label35" runat="server" Text="<%$ Resources:Attendance,Partial Leave Functionality %>"></asp:Label>
                                                            <asp:RadioButton ID="rbtnPartialEnable" Checked="true" Style="margin-left: 10px; margin-right: 10px;" GroupName="New_Partial" Text="<%$ Resources:Attendance,Enable %>" runat="server" AutoPostBack="true" OnCheckedChanged="rbtPartial_OnCheckedChanged" />
                                                            <asp:RadioButton ID="rbtnPartialDisable" Style="margin-left: 10px; margin-right: 10px;" GroupName="New_Partial" Text="<%$ Resources:Attendance,Disable %>" runat="server" AutoPostBack="true" OnCheckedChanged="rbtPartial_OnCheckedChanged" />
                                                            <br />
                                                            <asp:Label ID="Label36" runat="server" Text="<%$ Resources:Attendance,Total Minutes For Month  %>"></asp:Label><a style="color: Red">*</a>
                                                            <asp:RequiredFieldValidator EnableClientScript="true" Style="float: right;" runat="server" ID="RequiredFieldValidator22" ValidationGroup="OTPL_Grid_Save"
                                                                Display="Dynamic" SetFocusOnError="true" ControlToValidate="txtTotalMinutes" ErrorMessage="<%$ Resources:Attendance,Enter Total Minutes For Month %>" />


                                                            <asp:TextBox ID="txtTotalMinutes" runat="server" CssClass="form-control" />
                                                            <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender9" runat="server" Enabled="True"
                                                                TargetControlID="txtTotalMinutes" FilterType="Numbers">
                                                            </cc1:FilteredTextBoxExtender>
                                                            <br />
                                                            <asp:Label ID="Label41" runat="server" Text="<%$ Resources:Attendance,Minute Use in a Day One Time %>"></asp:Label><a style="color: Red">*</a>
                                                            <asp:RequiredFieldValidator EnableClientScript="true" Style="float: right;" runat="server" ID="RequiredFieldValidator23" ValidationGroup="OTPL_Grid_Save"
                                                                Display="Dynamic" SetFocusOnError="true" ControlToValidate="txtMinuteday" ErrorMessage="<%$ Resources:Attendance,Enter Minute Use in a Day One Time%>" />
                                                            <asp:TextBox ID="txtMinuteday" runat="server" CssClass="form-control" />
                                                            <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender10" runat="server" Enabled="True"
                                                                TargetControlID="txtMinuteday" FilterType="Numbers">
                                                            </cc1:FilteredTextBoxExtender>
                                                            <br />
                                                            <div id="Tr2" runat="server" visible="false">
                                                                <asp:Label ID="Label42" runat="server" Text="<%$ Resources:Attendance,Carry Forward Minutes %>"></asp:Label>
                                                                <asp:RadioButton ID="rbtnCarryYes" GroupName="SMS" Text="<%$ Resources:Attendance,Yes %>"
                                                                    runat="server" />
                                                                &nbsp;&nbsp;
                                                                                    <asp:RadioButton ID="rbtnCarryNo" GroupName="SMS" Text="<%$ Resources:Attendance,No %>"
                                                                                        runat="server" />
                                                            </div>
                                                            <br />
                                                        </div>
                                                        <div class="col-md-6">
                                                            <asp:Label ID="Label49" Font-Bold="true" Font-Size="13px" Font-Names="Arial" runat="server"
                                                                Text="<%$ Resources:Attendance,Over Time %>"></asp:Label>
                                                            <br />
                                                            <br />
                                                            <asp:Label ID="Label43" runat="server" Text="<%$ Resources:Attendance,Over Time Functionality %>"></asp:Label>
                                                            <asp:RadioButton ID="rbtnOTEnable" Checked="true" Style="margin-left: 10px; margin-right: 10px;" GroupName="New_OT" Text="<%$ Resources:Attendance,Enable %>" runat="server" AutoPostBack="true" OnCheckedChanged="rbtOT_OnCheckedChanged" />
                                                            <asp:RadioButton ID="rbtnOTDisable" Style="margin-left: 10px; margin-right: 10px;" GroupName="New_OT" Text="<%$ Resources:Attendance,Disable %>" runat="server" AutoPostBack="true" OnCheckedChanged="rbtOT_OnCheckedChanged" />

                                                            <br />
                                                            <asp:Label ID="Label44" runat="server" Text="<%$ Resources:Attendance,Calculation Method %>"></asp:Label>
                                                            <asp:DropDownList ID="ddlOTCalc" runat="server" CssClass="form-control">
                                                                <asp:ListItem Text="In" Value="In"></asp:ListItem>
                                                                <asp:ListItem Text="Out" Value="Out"></asp:ListItem>
                                                                <asp:ListItem Text="Both" Value="Both"></asp:ListItem>
                                                                <asp:ListItem Text="Work Hour" Value="Work Hour"></asp:ListItem>
                                                            </asp:DropDownList>
                                                            <br />
                                                            <asp:Label ID="Label45" runat="server" Text="<%$ Resources:Attendance,Value(Normal Day) %>"></asp:Label>
                                                            <div class="input-group" style="width: 100%">
                                                                <asp:TextBox ID="txtNoralType" Style="width: 70%;" runat="server" CssClass="form-control" />&nbsp;
                                                                                    <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender11" runat="server" Enabled="True"
                                                                                        TargetControlID="txtNoralType" ValidChars="1,2,3,4,5,6,7,8,9,0,.">
                                                                                    </cc1:FilteredTextBoxExtender>
                                                                <asp:DropDownList ID="ddlNormalType" Style="width: 28%;" runat="server" CssClass="form-control">
                                                                    <asp:ListItem Text="Fixed" Value="1"></asp:ListItem>
                                                                    <asp:ListItem Text="%" Value="2"></asp:ListItem>
                                                                </asp:DropDownList>
                                                            </div>
                                                            <br />
                                                            <asp:Label ID="Label46" runat="server" Text="<%$ Resources:Attendance,Value(Week Off) %>"></asp:Label>
                                                            <div class="input-group" style="width: 100%">
                                                                <asp:TextBox ID="txtWeekOffValue" Style="width: 70%;" runat="server" CssClass="form-control" />&nbsp;
                                                                                    <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender12" runat="server" Enabled="True"
                                                                                        TargetControlID="txtWeekOffValue" ValidChars="1,2,3,4,5,6,7,8,9,0,.">
                                                                                    </cc1:FilteredTextBoxExtender>
                                                                <asp:DropDownList ID="ddlWeekOffType" Style="width: 28%;" runat="server" CssClass="form-control">
                                                                    <asp:ListItem Text="Fixed" Value="1"></asp:ListItem>
                                                                    <asp:ListItem Text="%" Value="2"></asp:ListItem>
                                                                </asp:DropDownList>
                                                            </div>
                                                            <br />
                                                            <asp:HiddenField ID="hdnEmpIdOt" runat="server" />
                                                            <asp:Label ID="Label47" runat="server" Text="<%$ Resources:Attendance,Value(Holiday) %>"></asp:Label>
                                                            <div class="input-group" style="width: 100%">
                                                                <asp:TextBox ID="txtHolidayValue" Style="width: 70%;" runat="server" CssClass="form-control" />&nbsp;
                                                                                    <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender23" runat="server" Enabled="True"
                                                                                        TargetControlID="txtHolidayValue" ValidChars="1,2,3,4,5,6,7,8,9,0,.">
                                                                                    </cc1:FilteredTextBoxExtender>

                                                                <asp:DropDownList ID="ddlHolidayType" Style="width: 28%;" runat="server" CssClass="form-control">
                                                                    <asp:ListItem Text="Fixed" Value="1"></asp:ListItem>
                                                                    <asp:ListItem Text="%" Value="2"></asp:ListItem>
                                                                </asp:DropDownList>
                                                            </div>
                                                            <br />
                                                        </div>
                                                    </div>

                                                    <div class="col-md-12" style="text-align: center;">
                                                        <asp:Button ID="btnSaveOT" runat="server" CssClass="btn btn-success" ValidationGroup="OTPL_Grid_Save" CausesValidation="true" OnClick="btnSaveOT_Click"
                                                            Text="<%$ Resources:Attendance,Save %>" Visible="false" />

                                                        <asp:Button ID="btnResetOT" runat="server" CausesValidation="False" CssClass="btn btn-primary"
                                                            OnClick="btnResetOT_Click" Text="<%$ Resources:Attendance,Reset %>" />

                                                        <asp:Button ID="btnCancelOT" runat="server" CausesValidation="False" CssClass="btn btn-danger"
                                                            OnClick="btnCancelOT_Click" Text="<%$ Resources:Attendance,Cancel %>" />
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </div>

                    <div class="tab-pane" id="Upload" runat="server">
                        <asp:UpdatePanel ID="Update_Upload" runat="server">
                            <Triggers>
                                <asp:PostBackTrigger ControlID="Lnk_UploadEmo_Name" />
                                <asp:PostBackTrigger ControlID="btndownloadInvalid" />
                                <asp:PostBackTrigger ControlID="btndeviceOpExport" />

                            </Triggers>
                            <ContentTemplate>
                                <div class="row">
                                    <div class="col-md-12">
                                        <div class="box box-primary">
                                            <div class="box-body">
                                                <div class="form-group">


                                                    <div id="Div_device_Information" runat="server" class="col-md-12">

                                                        <div class="col-md-12" style="text-align: center;">
                                                            <asp:RadioButton ID="rbtnupdateoption" runat="server" Text="Upload" GroupName="UPD" AutoPostBack="true" OnCheckedChanged="rbtnupdateoption_CheckedChanged" />
                                                            <asp:RadioButton ID="rbtnReportoption" runat="server" Text="Report" GroupName="UPD" AutoPostBack="true" OnCheckedChanged="rbtnupdateoption_CheckedChanged" />

                                                        </div>


                                                        <div class="row" id="Div_device_Report_operation" runat="server" visible="false">
                                                            <br />
                                                            <div class="col-md-2">

                                                                <asp:Label ID="Label171" runat="server" class="control-label" Text="<%$ Resources:Attendance,Reference No %>"></asp:Label>

                                                                <asp:TextBox ID="txtReferenceReport" runat="server" CssClass="form-control" OnTextChanged="txtReferenceReport_TextChanged" AutoPostBack="true"></asp:TextBox>
                                                                <cc1:AutoCompleteExtender ID="AutoCompleteExtender6" runat="server" DelimiterCharacters="" EnableCaching="true"
                                                                    Enabled="True" ServiceMethod="GetCompletionListRefName" ServicePath="" CompletionInterval="100"
                                                                    MinimumPrefixLength="1" CompletionSetCount="1" TargetControlID="txtReferenceReport" UseContextKey="True"
                                                                    CompletionListCssClass="autoCompleteList" CompletionListItemCssClass="autoCompleteListItem" CompletionListHighlightedItemCssClass="autoCompleteSelectedListItem" OnClientShown="resetPosition">
                                                                </cc1:AutoCompleteExtender>
                                                            </div>
                                                            <div class="col-md-2">


                                                                <asp:Label ID="Label172" runat="server" class="control-label" Text="<%$ Resources:Attendance,Status %>"></asp:Label>
                                                                <asp:DropDownList ID="ddlStatus" runat="server" CssClass="form-control">
                                                                    <asp:ListItem Text="All" Value="0"></asp:ListItem>
                                                                    <asp:ListItem Text="Pending" Value="Pending"></asp:ListItem>
                                                                    <asp:ListItem Text="Done" Value="Done"></asp:ListItem>
                                                                    <asp:ListItem Text="Not Done" Value="Not Done"></asp:ListItem>
                                                                </asp:DropDownList>

                                                            </div>

                                                            <div class="col-md-2">


                                                                <asp:Label ID="Label173" runat="server" class="control-label" Text="Operation"></asp:Label>
                                                                <asp:DropDownList ID="ddldeviceOp" runat="server" CssClass="form-control">
                                                                    <asp:ListItem Text="All" Value="0"></asp:ListItem>
                                                                    <asp:ListItem Text="Upload" Value="Upload"></asp:ListItem>
                                                                    <asp:ListItem Text="Transfer" Value="Transfer"></asp:ListItem>

                                                                    <asp:ListItem Text="Terminate" Value="Terminate"></asp:ListItem>
                                                                    <asp:ListItem Text="Delete" Value="Delete"></asp:ListItem>
                                                                </asp:DropDownList>

                                                            </div>
                                                            <div class="col-md-4">
                                                                <br />
                                                                <asp:Button ID="btnGet" runat="server" Text="<%$ Resources:Attendance,Get%>" CssClass="btn btn-primary" OnClick="btnGet_Click" />
                                                                <asp:Button ID="btnRefRefresh" runat="server" Text="<%$ Resources:Attendance,Reset%>" CssClass="btn btn-primary" OnClick="btnRefRefresh_Click" />
                                                                <asp:Button ID="btnRefDelete" runat="server" Text="<%$ Resources:Attendance,Delete%>" CssClass="btn btn-primary" OnClick="btnRefDelete_Click" ValidationGroup="DeviceOperation_Grid_delete" />
                                                                <cc1:ConfirmButtonExtender ID="confirm2" runat="server" ConfirmText="<%$ Resources:Attendance,Are you sure you want to delete the record?%>"
                                                                    TargetControlID="btnRefDelete">
                                                                </cc1:ConfirmButtonExtender>
                                                                <asp:Button ID="btndeviceOpExport" runat="server" Text="<%$ Resources:Attendance,Download%>" CssClass="btn btn-primary" OnClick="btndeviceOpExport_Click" />
                                                            </div>
                                                            <div class="col-md-2" style="text-align: right;">
                                                                <br />
                                                                <asp:Label ID="lbltotalrefRecords" runat="server"></asp:Label>
                                                            </div>
                                                            <div class="col-md-12">
                                                                <br />
                                                                <asp:CustomValidator ID="CustomValidator12" ValidationGroup="DeviceOperation_Grid_delete" runat="server" ErrorMessage="Please select at least one record."
                                                                    ClientValidationFunction="DeviceOperation_Validate" ForeColor="Red"></asp:CustomValidator>
                                                                <asp:Panel runat="server" ID="pnlExport">
                                                                    <div style="overflow: auto; max-height: 500px">
                                                                        <asp:GridView CssClass="table-striped table-bordered table table-hover" ID="gvDeviceOpHistory" AllowSorting="true" AllowPaging="false" Width="100%" ClientIDMode="Static" runat="server" AutoGenerateColumns="false" OnSorting="gvDeviceOpHistory_OnSorting">
                                                                            <Columns>
                                                                                <asp:TemplateField>
                                                                                    <HeaderTemplate>
                                                                                        <asp:CheckBox ID="chkSelectAll" runat="server" onclick="selectAllCheckbox_Click(this)" />
                                                                                    </HeaderTemplate>
                                                                                    <ItemTemplate>
                                                                                        <asp:CheckBox ID="chkgvselect" runat="server" onclick="selectCheckBox_Click(this)" />
                                                                                    </ItemTemplate>
                                                                                </asp:TemplateField>

                                                                                <asp:TemplateField HeaderText="<%$ Resources:Attendance,Reference No %>" SortExpression="Reference_No">
                                                                                    <ItemTemplate>
                                                                                        <asp:Label ID="lblRefrenceNo" runat="server" Text='<%#Eval("Reference_No") %>' />
                                                                                        <asp:Label ID="lblTransId" runat="server" Text='<%#Eval("Trans_Id") %>' Visible="false" />
                                                                                    </ItemTemplate>
                                                                                </asp:TemplateField>

                                                                                <asp:TemplateField HeaderText="<%$ Resources:Attendance,Employee Code %>" SortExpression="Emp_Code">
                                                                                    <ItemTemplate>
                                                                                        <asp:Label ID="lblEmpCode" runat="server" Text='<%#Eval("Emp_Code") %>' />
                                                                                    </ItemTemplate>
                                                                                </asp:TemplateField>
                                                                                <asp:TemplateField HeaderText="<%$ Resources:Attendance,Employee Name %>" SortExpression="Emp_Name">
                                                                                    <ItemTemplate>
                                                                                        <asp:Label ID="lblEmpName" runat="server" Text='<%#Eval("Emp_Name") %>' />
                                                                                    </ItemTemplate>
                                                                                </asp:TemplateField>

                                                                                <asp:TemplateField HeaderText="Old Group" SortExpression="Old_Group_Name">
                                                                                    <ItemTemplate>
                                                                                        <asp:Label ID="lblOldGroupName" runat="server" Text='<%#Eval("Old_Group_Name") %>' />
                                                                                    </ItemTemplate>
                                                                                </asp:TemplateField>

                                                                                <asp:TemplateField HeaderText="New Group" SortExpression="Group_Name">
                                                                                    <ItemTemplate>
                                                                                        <asp:Label ID="lblGroupName" runat="server" Text='<%#Eval("Group_Name") %>' />
                                                                                    </ItemTemplate>
                                                                                </asp:TemplateField>
                                                                                <asp:TemplateField HeaderText="<%$ Resources:Attendance,Device Name %>" SortExpression="Device_Name">
                                                                                    <ItemTemplate>
                                                                                        <asp:Label ID="lblDeviceName" runat="server" Text='<%#Eval("Device_Name") %>' />
                                                                                    </ItemTemplate>
                                                                                </asp:TemplateField>

                                                                                <asp:TemplateField HeaderText="Effective Date" SortExpression="Effective_date">
                                                                                    <ItemTemplate>
                                                                                        <asp:Label ID="lbleffectivedate" runat="server" Text='<%#getdate(Eval("Effective_date").ToString()) %>' />
                                                                                    </ItemTemplate>
                                                                                </asp:TemplateField>
                                                                                <asp:TemplateField HeaderText="Operation" SortExpression="Operation_Type">
                                                                                    <ItemTemplate>
                                                                                        <asp:Label ID="lbloperationtype" runat="server" Text='<%#Eval("Operation_Type") %>' />
                                                                                    </ItemTemplate>
                                                                                </asp:TemplateField>

                                                                                <asp:TemplateField HeaderText="Status" SortExpression="Status">
                                                                                    <ItemTemplate>
                                                                                        <asp:Label ID="lblStatus" runat="server" Text='<%#Eval("Status") %>' />
                                                                                    </ItemTemplate>
                                                                                </asp:TemplateField>


                                                                                <asp:TemplateField HeaderText="Remarks" SortExpression="Remarks">
                                                                                    <ItemTemplate>
                                                                                        <asp:Label ID="lblRemark" runat="server" Text='<%#Eval("Remarks") %>' />
                                                                                    </ItemTemplate>
                                                                                </asp:TemplateField>


                                                                            </Columns>

                                                                        </asp:GridView>
                                                                    </div>
                                                                </asp:Panel>

                                                            </div>
                                                        </div>



                                                        <div class="row" id="Div_device_upload_operation" runat="server">
                                                            <div class="col-md-12" style="text-align: center;">
                                                                <br />
                                                                <asp:HyperLink ID="uploadEmpInfo" runat="server" Font-Bold="true" Font-Size="15px"
                                                                    NavigateUrl="~/CompanyResource/Upload_Operation.xlsx" Text="Download sample format for update information" Font-Underline="true"></asp:HyperLink>
                                                                <br />
                                                            </div>
                                                            <div class="col-md-6" style="text-align: center;">
                                                                <br />
                                                                <asp:Label runat="server" Text="Browse Excel File" ID="Label169"></asp:Label>
                                                                <div class="input-group" style="width: 100%;">
                                                                    <cc1:AsyncFileUpload ID="fileLoad" OnUploadedComplete="FileUploadComplete" OnClientUploadError="uploadError" OnClientUploadStarted="uploadStarted" OnClientUploadComplete="uploadComplete"
                                                                        runat="server" CssClass="form-control" CompleteBackColor="White" UploaderStyle="Traditional" UploadingBackColor="#CCFFFF" ThrobberID="imgLoader" Width="100%" />
                                                                    <div class="input-group-btn" style="border: solid 1px #d2d6de; width: 35px;">
                                                                        <asp:Image ID="Img_Right" runat="server" Width="30px" Height="30px" ImageUrl="../Images/Allow.png" Style="display: none" />
                                                                        <asp:Image ID="Img_Wrong" runat="server" Width="30px" Height="30px" ImageUrl="../Images/Delete1.png" Style="display: none" />
                                                                        <asp:Image ID="imgLoader" runat="server" ImageUrl="../Images/loader.gif" />
                                                                    </div>

                                                                </div>
                                                                <br />

                                                                <asp:Button ID="btnGetSheet" CssClass="btn btn-primary" runat="server" CausesValidation="False"
                                                                    OnClick="btnGetSheet_Click" Visible="true" Text="<%$ Resources:Attendance,Connect To DataBase %>" />

                                                            </div>
                                                            <div class="col-md-6" style="text-align: center;">
                                                                <br />
                                                                <asp:Label runat="server" Text="Select Sheet" ID="Label170"></asp:Label>
                                                                <asp:DropDownList ID="ddlTables" CssClass="form-control" runat="server">
                                                                </asp:DropDownList>
                                                                <br />

                                                                <asp:CheckBox ID="chkisautoinsert" runat="server" Text="Is Auto Insert" />
                                                                <asp:Button ID="Button6" CssClass="btn btn-primary" runat="server" CausesValidation="False"
                                                                    OnClick="btnConnect_Click" Visible="true" Text="GetRecord" />

                                                                <asp:Button ID="btnRunservice" CssClass="btn btn-primary" runat="server" CausesValidation="False"
                                                                    OnClick="btnRunservice_Click" Visible="true" Text="Start Service" />
                                                                </br>
                                                             <br />
                                                            </div>
                                                        </div>

                                                        <div class="row" id="uploadEmpdetail" runat="server" visible="false">

                                                            <div class="col-md-6" style="text-align: left">
                                                                <asp:RadioButton ID="rbtnupdall" Style="margin-left: 20px; margin-right: 20px;" runat="server" GroupName="upd" AutoPostBack="true" Checked="true" OnCheckedChanged="rbtnupdall_OnCheckedChanged" Text="All" />
                                                                <asp:RadioButton ID="rbtnupdValid" Style="margin-left: 20px; margin-right: 20px;" runat="server" GroupName="upd" AutoPostBack="true" Text="Valid" OnCheckedChanged="rbtnupdall_OnCheckedChanged" />
                                                                <asp:RadioButton ID="rbtnupdInValid" Style="margin-left: 20px; margin-right: 20px;" runat="server" GroupName="upd" AutoPostBack="true" Text="Invalid" OnCheckedChanged="rbtnupdall_OnCheckedChanged" />
                                                            </div>
                                                            <div class="col-md-6" style="text-align: right;">
                                                                <asp:Label ID="lbltotaluploadRecord" runat="server"></asp:Label>
                                                            </div>
                                                            <div class="col-md-12">
                                                                <div style="overflow: auto; max-height: 300px;">
                                                                    <asp:GridView CssClass="table-striped table-bordered table table-hover" ID="gvSelected" runat="server" Width="100%">


                                                                        <PagerStyle CssClass="pagination-ys" />

                                                                    </asp:GridView>
                                                                </div>
                                                                <br />
                                                            </div>


                                                            <div class="col-md-6">
                                                                <asp:Label ID="Label16" runat="server" Text="<%$ Resources:Attendance,Reference No %>"></asp:Label>
                                                                <asp:TextBox ID="txtuploadReferenceNo" runat="server" CssClass="form-control" ReadOnly="true" />

                                                                <br />
                                                            </div>
                                                            <div class="col-md-6" style="text-align: center">
                                                                <br />



                                                                <asp:Button ID="btnUploadEmpInfo" runat="server" CssClass="btn btn-primary" OnClick="btnUploadEmpInfo_Click"
                                                                    Text="<%$ Resources:Attendance,Upload Data %>" />

                                                                <cc1:ConfirmButtonExtender ID="ConfirmButtonExtender2" runat="server" TargetControlID="btnUploadEmpInfo"
                                                                    ConfirmText="Are you sure to Save Records in Database.">
                                                                </cc1:ConfirmButtonExtender>
                                                                <asp:Button ID="btnResetEmpInfo" runat="server" CssClass="btn btn-primary" OnClick="btnResetEmpInfo_Click"
                                                                    Text="<%$ Resources:Attendance,Reset %>" />


                                                                <asp:Button ID="btndownloadInvalid" CssClass="btn btn-primary" runat="server" Text="Download Invalid Record" CausesValidation="False"
                                                                    OnClick="btndownloadInvalid_Click" />
                                                            </div>
                                                        </div>

                                                    </div>


                                                    <div id="Div_Main_Upload" runat="server" class="col-md-12" style="display: none;">





                                                        <div class="col-md-4">
                                                            <asp:HyperLink ID="HyperLink1" Visible="false" runat="server"
                                                                NavigateUrl="~/CompanyResource/EmpWithNesField.xls" Text="<%$ Resources:Attendance,Download Excel Format For Necassary Field%>"></asp:HyperLink>
                                                            <br />
                                                            <%--<asp:LinkButton ID="lnkuploadEmpName" runat="server" CausesValidation="false"  Text="Download Excel Format For Update Employee Name"   ForeColor="Blue" Font-Underline="true" OnClick="lnkuploadEmpName_OnClick"></asp:LinkButton>--%>
                                                        </div>
                                                        <div class="col-md-4">
                                                            <asp:LinkButton ID="Lnk_UploadEmo_Name" runat="server" CausesValidation="false" OnClick="Lnk_UploadEmo_Name_Click" Text="Download Excel Format For Update Employee Name"></asp:LinkButton>

                                                            <asp:HyperLink ID="HyperLink3" Visible="false" runat="server"
                                                                NavigateUrl="~/CompanyResource/EmpWithNesField.xls" Text="<%$ Resources:Attendance,Download Excel Format For Necassary Field%>"></asp:HyperLink>
                                                            <br />
                                                            <br />

                                                            <asp:Label runat="server" Text="Browse Excel File" ID="Label66"></asp:Label><a style="color: Red">*</a>
                                                            <div class="input-group" style="width: 100%;">
                                                                <cc1:AsyncFileUpload ID="fileLoad1"
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

                                                            <br />
                                                            <div style="text-align: center;">
                                                                <asp:Button ID="btnConnect" CssClass="btn btn-primary" runat="server" CausesValidation="true" ValidationGroup="U_Save"
                                                                    OnClick="btnConnect_Click" Visible="false" Text="<%$ Resources:Attendance,Connect To DataBase %>" />
                                                            </div>
                                                            <br />
                                                            <asp:Label ID="Label67" runat="server" Text="Select Sheet"></asp:Label>
                                                            <asp:DropDownList ID="ddlTables1" CssClass="form-control" runat="server"></asp:DropDownList>
                                                            <br />
                                                            <div style="text-align: center;">
                                                                <asp:CheckBox ID="chkNecField" runat="server" Text="<%$ Resources:Attendance,Only Necessary Field%>" />
                                                                <br />
                                                                <asp:Button ID="btnviewcolumns" CssClass="btn btn-primary" runat="server" CausesValidation="False"
                                                                    OnClick="btnviewcolumns_Click" Visible="false" Text="<%$ Resources:Attendance,Map Column %>" />
                                                            </div>
                                                            <br />
                                                        </div>
                                                        <div class="col-md-4">
                                                            <asp:HyperLink ID="HyperLink2" Visible="false" runat="server"
                                                                NavigateUrl="~/CompanyResource/EmpWithAllField.xlsx" Text="<%$ Resources:Attendance,Download Excel Format For All Field%>"></asp:HyperLink>
                                                            <br />
                                                        </div>
                                                    </div>

                                                    <div id="pnlMap" runat="server" class="col-md-12" style="display: none;">
                                                        <br />
                                                        <div style="overflow: auto">
                                                            <asp:GridView CssClass="table-striped table-bordered table table-hover" ID="gvFieldMapping" Width="100%" runat="server" AutoGenerateColumns="False"
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
                                                                            <asp:DropDownList ID="ddlExcelCol" runat="server">
                                                                            </asp:DropDownList>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                </Columns>
                                                                <PagerStyle CssClass="pagination-ys" />

                                                            </asp:GridView>
                                                        </div>
                                                        <div class="col-md-12" style="text-align: center;">
                                                            <br />
                                                            <asp:Button ID="btnUploadTemp" CssClass="btn btn-primary" runat="server" OnClick="btnUpload_Click2"
                                                                Text="<%$ Resources:Attendance,Show Data %>" />
                                                            <asp:Button ID="btncancel1" CssClass="btn btn-primary" runat="server" OnClick="btncancel_Click"
                                                                Text="<%$ Resources:Attendance,Reset %>" />
                                                        </div>
                                                    </div>

                                                    <br />
                                                    <div id="pnlshowdata" runat="server" class="col-md-12" style="display: none;">
                                                        <br />
                                                        <div class="col-md-6">
                                                            <asp:DropDownList ID="ddlFiltercol" CssClass="form-control" runat="server">
                                                            </asp:DropDownList>
                                                            <br />
                                                        </div>
                                                        <div class="col-md-6">
                                                            <asp:TextBox ID="txtfiltercol" CssClass="form-control" runat="server"></asp:TextBox>
                                                            <br />
                                                        </div>
                                                        <div class="col-md-12" style="text-align: center">
                                                            <asp:Button ID="btnFilter" CssClass="btn btn-primary" runat="server" OnClick="btnFilter_Click"
                                                                Text="<%$ Resources:Attendance,Filter %>" />

                                                            <asp:Button ID="btnresetgv" CssClass="btn btn-primary" runat="server" OnClick="btnresetgv_Click"
                                                                Text="<%$ Resources:Attendance,Reset %>" />

                                                            <asp:Button ID="btnBackToMapData" CssClass="btn btn-primary" runat="server"
                                                                Text="<%$ Resources:Attendance,Back To FileUpload %>" OnClick="btnBackToMapData_Click" />
                                                            <br />
                                                        </div>

                                                        <div class="col-md-12" style="text-align: center">
                                                            <br />
                                                            <asp:Button ID="Button21" runat="server" CssClass="btn btn-primary" OnClick="btnUpload_Click1"
                                                                Text="<%$ Resources:Attendance,Upload Data %>" />
                                                            <cc1:ConfirmButtonExtender ID="ConfirmButtonExtender1" runat="server" TargetControlID="Button21"
                                                                ConfirmText="Are you sure to Save Records in Database.">
                                                            </cc1:ConfirmButtonExtender>
                                                            <br />
                                                        </div>
                                                    </div>

                                                    <div class="col-md-12" style="overflow: auto; display: none;">
                                                        <br />
                                                        <asp:GridView CssClass="table-striped table-bordered table table-hover" ID="gvSelected1" Width="100%" runat="server">


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

                    <div class="tab-pane" id="Penalty" runat="server">
                        <asp:UpdatePanel ID="Update_Penalty" runat="server">
                            <ContentTemplate>
                                <div class="row">
                                    <div class="col-md-12">
                                        <div class="box box-primary">
                                            <div class="box-body">
                                                <div class="form-group">
                                                    <div class="col-md-12">
                                                        <asp:RadioButton ID="rbtnPenaltyGroup" OnCheckedChanged="EmpPenalty_CheckedChanged" runat="server"
                                                            Text="<%$ Resources:Attendance,Group %>" Font-Bold="true" GroupName="EmpPenalty"
                                                            AutoPostBack="true" />
                                                        <asp:RadioButton ID="rbtnPenaltyEmp" Checked="true" Style="margin-left: 20px;" runat="server" AutoPostBack="true" Text="<%$ Resources:Attendance,Employee %>"
                                                            GroupName="EmpPenalty" Font-Bold="true" OnCheckedChanged="EmpPenalty_CheckedChanged" />
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                                <div class="col-md-12">
                                    <div id="PnlPenaltyEmp" runat="server" class="row">


                                        <div class="row">
                                            <div class="col-md-12">
                                                <div id="Div9" runat="server" class="box box-info collapsed-box">
                                                    <div class="box-header with-border">
                                                        <h3 class="box-title">
                                                            <asp:Label ID="Label182" runat="server" Text="Advance Search"></asp:Label></h3>
                                                        &nbsp;&nbsp;|&nbsp;&nbsp;
					  <asp:Label ID="lblTotalRecordPenalty" runat="server" Text="<%$ Resources:Attendance,Total Records: 0 %>"></asp:Label>
                                                        <asp:Label ID="lblEmpPenalty" runat="server" Visible="false"></asp:Label>

                                                        <div class="box-tools pull-right">
                                                            <button type="button" class="btn btn-box-tool" data-widget="collapse">
                                                                <i id="I8" runat="server" class="fa fa-plus"></i>
                                                            </button>
                                                        </div>
                                                    </div>
                                                    <div class="box-body">

                                                        <div class="col-md-6">
                                                            <asp:Label ID="lblLocationPenalty" runat="server" Text="<%$ Resources:Attendance,Location %>"></asp:Label>
                                                            <asp:DropDownList ID="ddlLocationPenalty" runat="server" CssClass="form-control"
                                                                AutoPostBack="true" OnSelectedIndexChanged="ddlLocationPenalty_SelectedIndexChanged">
                                                            </asp:DropDownList>
                                                            <br />
                                                        </div>
                                                        <div class="col-md-6">
                                                            <asp:Label ID="lblDeptPenalty" runat="server" Text="<%$ Resources:Attendance,Department %>"></asp:Label>

                                                            <asp:DropDownList ID="ddlDeptPenalty" runat="server" CssClass="form-control"
                                                                AutoPostBack="true" OnSelectedIndexChanged="ddlDeptPenalty_SelectedIndexChanged">
                                                            </asp:DropDownList>
                                                            <br />
                                                        </div>

                                                        <div class="col-lg-3">
                                                            <asp:DropDownList ID="ddlFieldPenalty" runat="server" CssClass="form-control">
                                                                <asp:ListItem Text="<%$ Resources:Attendance,Employee Code %>" Value="Emp_Code"></asp:ListItem>
                                                                <asp:ListItem Text="<%$ Resources:Attendance,Employee Name %>" Value="Emp_Name" Selected="True"></asp:ListItem>
                                                                <asp:ListItem Text="<%$ Resources:Attendance,Employee Name Local %>" Value="Emp_Name_L"></asp:ListItem>
                                                                <asp:ListItem Text="<%$ Resources:Attendance,Email Id %>" Value="Email_Id"></asp:ListItem>
                                                                <asp:ListItem Text="<%$ Resources:Attendance,Phone No. %>" Value="Phone_No"></asp:ListItem>
                                                                <asp:ListItem Text="<%$ Resources:Attendance,Civil Id %>" Value="Civil_Id"></asp:ListItem>
                                                            </asp:DropDownList>
                                                        </div>
                                                        <div class="col-lg-2">
                                                            <asp:DropDownList ID="ddlOptionPenalty" runat="server" CssClass="form-control">
                                                                <asp:ListItem Text="<%$ Resources:Attendance,--Select-- %>"></asp:ListItem>
                                                                <asp:ListItem Text="<%$ Resources:Attendance,Equal %>"></asp:ListItem>
                                                                <asp:ListItem Selected="True" Text="<%$ Resources:Attendance,Contains %>"></asp:ListItem>
                                                                <asp:ListItem Text="<%$ Resources:Attendance,Like %>"></asp:ListItem>
                                                            </asp:DropDownList>
                                                        </div>
                                                        <div class="col-lg-5">
                                                            <asp:Panel ID="Panel7" runat="server" DefaultButton="btnPenaltybind">
                                                                <asp:TextBox ID="txtValuePenalty" runat="server" CssClass="form-control"></asp:TextBox>
                                                                <asp:RequiredFieldValidator EnableClientScript="true" runat="server" ID="RequiredFieldValidator13" ValidationGroup="P_Search" Display="Dynamic" SetFocusOnError="true" ControlToValidate="txtValuePenalty" ErrorMessage="<%$ Resources:Attendance,Please enter value %>" />
                                                            </asp:Panel>
                                                        </div>
                                                        <div class="col-lg-2" style="text-align: center; padding-left: 1px; padding-right: 1px;">
                                                            <asp:LinkButton ID="btnPenaltybind" runat="server" CausesValidation="true" ValidationGroup="P_Search" OnClick="btnPenaltybind_Click" ToolTip="<%$ Resources:Attendance,Search %>"><span class="fa fa-search"  style="font-size:25px;"></span></asp:LinkButton>&nbsp;&nbsp;&nbsp;
                                                            <asp:LinkButton ID="btnPenaltyRefresh" runat="server" CausesValidation="False" OnClick="btnPenaltyRefresh_Click" ToolTip="<%$ Resources:Attendance,Refresh %>"><span class="fa fa-repeat"  style="font-size:25px;"></span></asp:LinkButton>&nbsp;&nbsp;&nbsp;
                                                            <asp:LinkButton ID="ImgbtnSelectAll_Penalty" runat="server" OnClick="ImgbtnSelectAll_ClickPenalty" ToolTip="<%$ Resources:Attendance, Select All %>"><span class="fas fa-th"  style="font-size:25px;"></span></asp:LinkButton>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="box box-warning box-solid" <%= gvEmpPenalty.Rows.Count>0?"style='display:block'":"style='display:none'"%>>
                                            <div class="box-body">
                                                <div class="row">
                                                    <div class="col-md-12">
                                                        <div class="flow">
                                                            <asp:Label ID="lblSelectRecordPenalty" runat="server" Visible="false"></asp:Label>
                                                            <asp:CustomValidator ID="CustomValidator6" ValidationGroup="Penalty_Grid_Save" runat="server" ErrorMessage="Please select at least one record."
                                                                ClientValidationFunction="Penalty_Validate" ForeColor="Red"></asp:CustomValidator>
                                                            <asp:GridView CssClass="table-striped table-bordered table table-hover" ID="gvEmpPenalty" runat="server" AllowPaging="True" AutoGenerateColumns="False"
                                                                OnPageIndexChanging="gvEmpPenalty_PageIndexChanging" Width="100%"
                                                                DataKeyNames="Emp_Id" PageSize="<%# PageControlCommon.GetPageSize() %>">
                                                                <Columns>
                                                                    <asp:TemplateField>
                                                                        <ItemTemplate>
                                                                            <asp:CheckBox ID="chkgvSelect" runat="server" />
                                                                        </ItemTemplate>
                                                                        <HeaderTemplate>
                                                                            <asp:CheckBox ID="chkgvSelectAll" runat="server" OnCheckedChanged="chkgvSelectAll_CheckedChangedPenalty"
                                                                                AutoPostBack="true" />
                                                                        </HeaderTemplate>
                                                                        <ItemStyle HorizontalAlign="Center" />
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="<%$ Resources:Attendance,Edit %>">
                                                                        <ItemTemplate>
                                                                            <asp:LinkButton ID="btnEdit" runat="server" CausesValidation="False" CommandArgument='<%# Eval("Emp_Id") %>' Visible='<%# hdnCanEdit.Value=="true"?true:false%>' OnCommand="btnEditPenalty_Command"><i class="fa fa-pencil"></i></asp:LinkButton>
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
                                                                        SortExpression="Emp_Name" />
                                                                    <asp:BoundField DataField="Emp_Name_L" HeaderText="<%$ Resources:Attendance,Employee Name Local%>"
                                                                        SortExpression="Emp_Name_L" />
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
                                </div>
                                <div class="col-md-12">
                                    <div id="PnlGroupPenalty" visible="false" runat="server" class="row">
                                        <div class="row">
                                            <div class="col-md-12">
                                                <div class="box box-primary">
                                                    <div class="box-body">
                                                        <div class="form-group">
                                                            <div class="col-md-12">
                                                                <div class="col-md-6">
                                                                    <asp:CustomValidator ID="CustomValidator7" ValidationGroup="Penalty_Grid_Save" runat="server" ErrorMessage="<%$ Resources:Attendance,Please select at least one record%>"
                                                                        ClientValidationFunction="Penalty_Validate" ForeColor="Red"></asp:CustomValidator>
                                                                    <asp:ListBox ID="lbxGroupPenalty" runat="server" OnSelectedIndexChanged="lbxGroupPenalty_SelectedIndexChanged" Style="width: 100%; height: 150px;" SelectionMode="Multiple" AutoPostBack="true"
                                                                        CssClass="form-control" Font-Names="Trebuchet MS" Font-Size="Small" ForeColor="Gray"></asp:ListBox>
                                                                </div>

                                                            </div>

                                                            <div class="col-md-12">
                                                                <div class="flow">
                                                                    <asp:GridView CssClass="table-striped table-bordered table table-hover" ID="gvEmployeePenalty" runat="server" Width="100%" AllowPaging="True" AutoGenerateColumns="False"
                                                                        OnPageIndexChanging="gvEmployeePenalty_PageIndexChanging"
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
                                                                                SortExpression="Emp_Name" />
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
                                </div>
                                <div class="row">
                                    <div class="col-md-12">
                                        <div class="box box-primary">
                                            <div class="box-body">
                                                <div class="form-group">
                                                    <div class="col-md-12">
                                                        <div class="col-md-4"></div>
                                                        <div class="col-md-4">
                                                            <table style="width: 100%">
                                                                <tr>
                                                                    <td>
                                                                        <asp:Label ID="Label120" runat="server" Text="<%$ Resources:Attendance,Late In Functionality %>"></asp:Label>
                                                                    </td>
                                                                    <td>
                                                                        <asp:RadioButton ID="rbtLateInEnable" GroupName="Late" Text="<%$ Resources:Attendance,Enable %>" runat="server" />
                                                                    </td>
                                                                    <td>
                                                                        <asp:RadioButton ID="rbtLateInDisable" GroupName="Late" Text="<%$ Resources:Attendance,Disable %>" runat="server" />
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td>
                                                                        <asp:Label ID="Label175" runat="server" Text="<%$ Resources:Attendance,Early Out Functionality %>"></asp:Label>
                                                                    </td>
                                                                    <td>
                                                                        <asp:RadioButton ID="rbtEarlyOutEnable" GroupName="Early" Text="<%$ Resources:Attendance,Enable %>" runat="server" />
                                                                    </td>
                                                                    <td>
                                                                        <asp:RadioButton ID="rbtEarlyOutDisable" GroupName="Early" Text="<%$ Resources:Attendance,Disable %>" runat="server" />
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td>
                                                                        <asp:Label ID="Label121" runat="server" Text="<%$ Resources:Attendance,Absent Functionality %>"></asp:Label>
                                                                    </td>
                                                                    <td>
                                                                        <asp:RadioButton ID="rbtnAbsentEnable" GroupName="absent" Text="<%$ Resources:Attendance,Enable %>" runat="server" />
                                                                    </td>
                                                                    <td>
                                                                        <asp:RadioButton ID="rbtnAbsentDisable" GroupName="absent" Text="<%$ Resources:Attendance,Disable %>" runat="server" />
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td>
                                                                        <asp:Label ID="Label125" runat="server" Text="Partial Leave Functionality"></asp:Label>
                                                                    </td>
                                                                    <td>
                                                                        <asp:RadioButton ID="rbtnPartialLeaveEnable" GroupName="Partial" Text="<%$ Resources:Attendance,Enable %>" runat="server" />
                                                                    </td>
                                                                    <td>
                                                                        <asp:RadioButton ID="rbtnPartialLeaveDisable" GroupName="Partial" Text="<%$ Resources:Attendance,Disable %>" runat="server" />
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                            <br />
                                                        </div>
                                                        <div class="col-md-4"></div>
                                                    </div>
                                                    <div class="col-md-12" style="text-align: center;">
                                                        <asp:HiddenField ID="hdnEmpIdPenalty" runat="server" />
                                                        <asp:Button ID="btnSavePenalty" runat="server" CssClass="btn btn-success" ValidationGroup="Penalty_Grid_Save" CausesValidation="true" OnClick="btnSavePenalty_Click"
                                                            Text="<%$ Resources:Attendance,Save %>" Visible="false" />

                                                        <asp:Button ID="btnResetPenalty" runat="server" CausesValidation="False" CssClass="btn btn-primary"
                                                            OnClick="btnResetPenalty_Click" Text="<%$ Resources:Attendance,Reset %>" />

                                                        <asp:Button ID="btnCancelPenalty" runat="server" CausesValidation="False" CssClass="btn btn-danger"
                                                            OnClick="btnCancelPenalty_Click" Text="<%$ Resources:Attendance,Cancel %>" />
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </div>

                    <div class="tab-pane" id="Help" runat="server">
                        <asp:UpdatePanel ID="Update_Help" runat="server">
                            <ContentTemplate>
                                <div class="row">
                                    <div class="col-md-12">
                                        <div class="box box-primary">
                                            <div class="box-body">
                                                <div class="form-group">
                                                    <div style="text-align: center;" class="col-md-12">
                                                        <asp:Button ID="Btn_Help" CssClass="btn btn-primary" Text="Help" OnClick="btnHelp_Click" runat="server" />
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
    <div class="modal fade" id="Modal_Address" tabindex="-1" role="dialog" aria-labelledby="Modal_AddressLabel"
        aria-hidden="true">
        <div class="modal-dialog modal-lg">
            <div class="modal-content">

                <div class="modal-body">
                    <asp:UpdatePanel ID="Update_Modal_Address" runat="server">
                        <ContentTemplate>
                            <asp:HiddenField ID="hdntxtaddressid" runat="server" />
                            <UC:Addressmaster ID="addaddress" runat="server" />
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
                <div class="modal-footer">
                    <button type="button" class="btn btn-danger" data-dismiss="modal">
                        Close</button>
                </div>
            </div>
        </div>
    </div>

    <div class="modal fade" id="Modal_Leave" tabindex="-1" role="dialog" aria-labelledby="Modal_LeaveLabel"
        aria-hidden="true">
        <div class="modal-dialog modal-lg">
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal">
                        <span aria-hidden="true">&times;</span><span class="sr-only">Close</span></button>
                    <h4 class="modal-title" id="Modal_LeaveLabel">
                        <asp:Label ID="Label127" runat="server" Font-Size="14px" Font-Bold="true" CssClass="labelComman"
                            Text="<%$ Resources:Attendance,Edit %>"></asp:Label></h4>
                </div>
                <div class="modal-body">
                    <asp:UpdatePanel ID="Update_Modal_Leave" runat="server">
                        <ContentTemplate>
                            <div class="row">
                                <div class="col-md-12">
                                    <div class="box box-primary">
                                        <div class="box-body">
                                            <div class="form-group">
                                                <div class="col-md-6">
                                                    <asp:Label ID="Label61" runat="server" Font-Size="14px" Font-Bold="true"
                                                        Text="<%$ Resources:Attendance,Employee Code %>"></asp:Label>
                                                    &nbsp : &nbsp
                                                            <asp:Label ID="lblEmpCodeLeave" runat="server" Font-Size="14px" Font-Bold="true"></asp:Label>
                                                    <br />
                                                </div>
                                                <div class="col-md-6">
                                                    <asp:Label ID="Label62" runat="server" Font-Size="14px" Font-Bold="true"
                                                        Text="<%$ Resources:Attendance,Employee Name %>"></asp:Label>
                                                    &nbsp : &nbsp
                                                            <asp:Label ID="lblEmpNameLeave" runat="server" Font-Size="14px" Font-Bold="true"></asp:Label>
                                                    <br />
                                                </div>
                                                <div class="col-md-6">
                                                    <asp:Label ID="Label100" runat="server" Text="<%$ Resources:Attendance,Yearly Half Day Allow %>"></asp:Label>
                                                    <div class="input-group">
                                                        <asp:TextBox onkeydown="return (event.keyCode!=13);" ID="txtHalfDAy" CssClass="form-control" runat="server"></asp:TextBox>
                                                        <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender7" TargetControlID="txtHalfDAy"
                                                            ValidChars="0,1,2,3,4,5,6,7,8,9" runat="server">
                                                        </cc1:FilteredTextBoxExtender>
                                                        <div class="input-group-btn">
                                                            <asp:Button ID="btnUpdateHalfday" Style="margin-left: 10px;" runat="server" CssClass="btn btn-primary"
                                                                OnClick="btnUpdateHalfday_Click" Text="<%$ Resources:Attendance,Save %>" Visible="false" />
                                                            <asp:HiddenField ID="hdnEmpIdHalfDay" runat="server" />
                                                        </div>
                                                    </div>
                                                    <br />
                                                </div>
                                                <div class="col-md-12" style="overflow: auto">
                                                    <asp:GridView CssClass="table-striped table-bordered table table-hover" ID="gvLeaveEmp" Width="100%" runat="server" AutoGenerateColumns="False" DataKeyNames="Trans_No"
                                                        PageSize="<%# PageControlCommon.GetPageSize() %>">
                                                        <Columns>
                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,Delete %>">
                                                                <ItemTemplate>
                                                                    <asp:ImageButton ID="IbtnDelete" runat="server" CausesValidation="False" CommandName='<%# Eval("Emp_Id") %>'
                                                                        CommandArgument='<%# Eval("LeaveType_Id") %>' ImageUrl="~/Images/Erase.png" OnCommand="btnDeleteLeave_Command"
                                                                        Visible="true" ToolTip="<%$ Resources:Attendance,Delete %>" />


                                                                    <cc1:ConfirmButtonExtender ID="confirm2" runat="server" ConfirmText="<%$ Resources:Attendance,Are you sure you want to delete the record?%>"
                                                                        TargetControlID="IbtnDelete">
                                                                    </cc1:ConfirmButtonExtender>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,LeaveType %>">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblleavetype0" Width="100px" runat="server" Text='<%# GetLeaveTypeName(Eval("LeaveType_Id")) %>'></asp:Label>
                                                                    <asp:HiddenField ID="hdnLeaveTypeId" runat="server" Value='<%# Eval("LeaveType_Id") %>' />
                                                                    <asp:HiddenField ID="hdnTranNo" runat="server" Value='<%# Eval("Trans_No") %>' />
                                                                    <asp:HiddenField ID="hdnEmpId1" runat="server" Value='<%# Eval("Emp_Id") %>' />
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,Total Leave %>">
                                                                <ItemTemplate>
                                                                    <asp:TextBox onkeydown="return (event.keyCode!=13);" ID="txtNoOfLeave0" Width="100px" runat="server" Text='<%# Eval("Total_Leave") %>'></asp:TextBox>
                                                                    <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" runat="server" TargetControlID="txtNoOfLeave0"
                                                                        ValidChars="0,1,2,3,4,5,6,7,8,9">
                                                                    </cc1:FilteredTextBoxExtender>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,Paid Leave %>">
                                                                <ItemTemplate>
                                                                    <asp:TextBox onkeydown="return (event.keyCode!=13);" ID="txtPAidLEave1" Width="100px" runat="server" Text='<%# Eval("Paid_Leave") %>'></asp:TextBox>
                                                                    <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtenderPAidLeave" runat="server"
                                                                        TargetControlID="txtPAidLEave1" ValidChars="0,1,2,3,4,5,6,7,8,9">
                                                                    </cc1:FilteredTextBoxExtender>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,Percentage Of Salary %>" Visible="false">
                                                                <ItemTemplate>
                                                                    <asp:TextBox onkeydown="return (event.keyCode!=13);" ID="TextBox1" Width="100px" runat="server" Text='<%# Eval("Percentage_Of_Salary") %>'></asp:TextBox>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,Schedule Type %>">
                                                                <ItemTemplate>
                                                                    <asp:DropDownList ID="ddlSchType0" Width="100px" runat="server" Enabled="false" SelectedValue='<%# Eval("Shedule_Type") %>'
                                                                        AutoPostBack="true" OnSelectedIndexChanged="ddlScheduleGrid_SelectedIndexChanged">
                                                                        <asp:ListItem Value="Monthly">Monthly</asp:ListItem>
                                                                        <asp:ListItem Value="Yearly">Yearly</asp:ListItem>
                                                                    </asp:DropDownList>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,IsYearCarry %>">
                                                                <ItemTemplate>
                                                                    <asp:CheckBox ID="chkYearCarry0" Width="100px" runat="server" Checked='<%# Eval("Is_YearCarry") %>'
                                                                        Text="<%$ Resources:Attendance,IsYearCarry %>" />
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,Is Rule %>">
                                                                <ItemTemplate>
                                                                    <asp:CheckBox ID="chkIsRule" Width="100px" runat="server" Checked='<%# Eval("IsRule") %>'
                                                                        Text="<%$ Resources:Attendance,Is Rule %>" />
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,Is Auto %>">
                                                                <ItemTemplate>
                                                                    <asp:CheckBox ID="chkIsAuto" Width="100px" runat="server" Checked='<%# Eval("IsAuto") %>'
                                                                        Text="<%$ Resources:Attendance,Is Auto %>" />
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
                    <asp:UpdatePanel ID="Modal_Buttopn_Leave" runat="server">
                        <ContentTemplate>
                            <asp:Button ID="Button1" runat="server" CssClass="btn btn-primary" OnClick="btnUpdateLeave_Click"
                                Text="<%$ Resources:Attendance,Save %>" />

                            <asp:Button ID="Button2" runat="server" CausesValidation="False" CssClass="btn btn-primary"
                                OnClick="btnCancelPopLeave_Click" Text="<%$ Resources:Attendance,Reset %>" />

                            <button type="button" class="btn btn-danger" data-dismiss="modal">
                                Close</button>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
            </div>
        </div>
    </div>

    <div class="modal fade" id="Modal_Leave_View" tabindex="-1" role="dialog" aria-labelledby="Modal_Leave_ViewLabel"
        aria-hidden="true">
        <div class="modal-dialog modal-lg">
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal">
                        <span aria-hidden="true">&times;</span><span class="sr-only">Close</span></button>
                    <h4 class="modal-title" id="Modal_Leave_ViewLabel">
                        <asp:Label ID="Label128" runat="server" Font-Size="14px" Font-Bold="true" CssClass="labelComman"
                            Text="<%$ Resources:Attendance,Edit %>"></asp:Label></h4>
                </div>
                <div class="modal-body">
                    <asp:UpdatePanel ID="Update_Modal_Leave_View" runat="server">
                        <ContentTemplate>
                            <div class="row">
                                <div class="col-md-12">
                                    <div class="box box-primary">
                                        <div class="box-body">
                                            <div class="form-group">
                                                <div class="col-md-12">
                                                    <div class="col-md-6">
                                                        <asp:Label ID="Label59" runat="server" Font-Size="14px" Font-Bold="true"
                                                            Text="<%$ Resources:Attendance,Employee Code %>"></asp:Label>
                                                        &nbsp : &nbsp
                                                            <asp:Label ID="lblEmpCodeLeaveView" runat="server" Font-Size="14px" Font-Bold="true"></asp:Label>
                                                        <br />
                                                    </div>
                                                    <div class="col-md-6">
                                                        <asp:Label ID="Label84" runat="server" Font-Size="14px" Font-Bold="true"
                                                            Text="<%$ Resources:Attendance,Employee Name %>"></asp:Label>
                                                        &nbsp : &nbsp
                                                            <asp:Label ID="lblEmpNameLeaveView" runat="server" Font-Size="14px" Font-Bold="true"></asp:Label>
                                                        <br />
                                                    </div>
                                                </div>
                                                <div class="col-md-12">
                                                    <div class="col-md-6">
                                                        <asp:Label ID="Label92" runat="server" Text="<%$ Resources:Attendance,Yearly Half Day Allow %>"></asp:Label>
                                                        <div class="input-group" style="width: 100%">
                                                            <asp:TextBox onkeydown="return (event.keyCode!=13);" ID="txtHalfDAyView" Style="width: 85%;" CssClass="form-control" runat="server"></asp:TextBox>
                                                        </div>
                                                        <br />
                                                    </div>

                                                </div>
                                                <div class="col-md-12">
                                                    <div class="flow">
                                                        <asp:GridView CssClass="table-striped table-bordered table table-hover" ID="gvLeaveEmpView" Width="100%" runat="server" AutoGenerateColumns="False" DataKeyNames="Trans_No"
                                                            PageSize="<%# PageControlCommon.GetPageSize() %>">
                                                            <Columns>
                                                                <asp:TemplateField HeaderText="<%$ Resources:Attendance,LeaveType %>">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblleavetype0" Width="100px" runat="server" Text='<%# GetLeaveTypeName(Eval("LeaveType_Id")) %>'></asp:Label>
                                                                        <asp:HiddenField ID="hdnLeaveTypeId" runat="server" Value='<%# Eval("LeaveType_Id") %>' />
                                                                        <asp:HiddenField ID="hdnTranNo" runat="server" Value='<%# Eval("Trans_No") %>' />
                                                                        <asp:HiddenField ID="hdnEmpId1" runat="server" Value='<%# Eval("Emp_Id") %>' />
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="<%$ Resources:Attendance,Total Leave %>">
                                                                    <ItemTemplate>
                                                                        <asp:TextBox onkeydown="return (event.keyCode!=13);" Width="100px" ID="txtNoOfLeave0" Enabled="false" runat="server" Text='<%# Eval("Total_Leave") %>'></asp:TextBox>
                                                                        <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" runat="server" TargetControlID="txtNoOfLeave0"
                                                                            ValidChars="0,1,2,3,4,5,6,7,8,9">
                                                                        </cc1:FilteredTextBoxExtender>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="<%$ Resources:Attendance,Paid Leave %>">
                                                                    <ItemTemplate>
                                                                        <asp:TextBox onkeydown="return (event.keyCode!=13);" Width="100px" ID="txtPAidLEave1" Enabled="false" runat="server" Text='<%# Eval("Paid_Leave") %>'></asp:TextBox>
                                                                        <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtenderPAidLeave" runat="server"
                                                                            TargetControlID="txtPAidLEave1" ValidChars="0,1,2,3,4,5,6,7,8,9">
                                                                        </cc1:FilteredTextBoxExtender>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="<%$ Resources:Attendance,Percentage Of Salary %>">
                                                                    <ItemTemplate>
                                                                        <asp:TextBox onkeydown="return (event.keyCode!=13);" ID="TextBox1" Width="100px" Enabled="false" runat="server" Text='<%# Eval("Percentage_Of_Salary") %>'></asp:TextBox>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="<%$ Resources:Attendance,Schedule Type %>">
                                                                    <ItemTemplate>
                                                                        <asp:DropDownList ID="ddlSchType0" Width="100px" runat="server" Enabled="false" SelectedValue='<%# Eval("Shedule_Type") %>'
                                                                            AutoPostBack="true">
                                                                            <asp:ListItem Value="Monthly">Monthly</asp:ListItem>
                                                                            <asp:ListItem Value="Yearly">Yearly</asp:ListItem>
                                                                        </asp:DropDownList>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="<%$ Resources:Attendance,IsYearCarry %>">
                                                                    <ItemTemplate>
                                                                        <asp:CheckBox ID="chkYearCarry0" Width="100px" runat="server" Enabled="false" Checked='<%# Eval("Is_YearCarry") %>'
                                                                            Text="<%$ Resources:Attendance,IsYearCarry %>" />
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="<%$ Resources:Attendance,Is Rule %>">
                                                                    <ItemTemplate>
                                                                        <asp:CheckBox ID="chkIsRule" runat="server" Width="100px" Enabled="false" Checked='<%# Eval("IsRule") %>'
                                                                            Text="<%$ Resources:Attendance,Is Rule %>" />
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="<%$ Resources:Attendance,Is Auto %>">
                                                                    <ItemTemplate>
                                                                        <asp:CheckBox ID="chkIsAuto" Width="100px" runat="server" Enabled="false" Checked='<%# Eval("IsAuto") %>' AutoPostBack="true" OnCheckedChanged="chkIsAuto_OnCheckedChanged"
                                                                            Text="<%$ Resources:Attendance,Is Auto %>" />
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                            </Columns>


                                                            <PagerStyle CssClass="pagination-ys" />

                                                        </asp:GridView>
                                                    </div>
                                                    <div class="flow">
                                                        <asp:GridView CssClass="table-striped table-bordered table table-hover" ID="gvLeavededuction" runat="server" AutoGenerateColumns="False"
                                                            Width="100%" DataKeyNames="Trans_Id" ShowFooter="false">

                                                            <Columns>
                                                                <asp:TemplateField HeaderText="Leave Type">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblLeaveType" runat="server" Text='<%#Eval("Leave_Name") %>' />
                                                                    </ItemTemplate>
                                                                    <ItemStyle HorizontalAlign="Center" Width="60px" />
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="Exceed From (In Days) ">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblDaysFrom" runat="server" Text='<%#Eval("DaysFrom") %>' />
                                                                        <asp:Label ID="lblTransId" Visible="false" runat="server" Text='<%#Eval("Trans_Id") %>' />
                                                                    </ItemTemplate>
                                                                    <ItemStyle HorizontalAlign="Center" Width="60px" />
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="Exceed to (In Days)">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblDaysTo" runat="server" Text='<%#Eval("DaysTo") %>' />
                                                                    </ItemTemplate>
                                                                    <ItemStyle HorizontalAlign="Center" Width="60px" />
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="deduction(%)">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lbldeductionpercentage" runat="server" Text='<%#Eval("Deduction_Percentage") %>' />
                                                                    </ItemTemplate>
                                                                    <ItemStyle HorizontalAlign="Center" Width="60px" />
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
                    <asp:UpdatePanel ID="Modal_Buttopn_Leave_View" runat="server">
                        <ContentTemplate>
                            <asp:Button ID="Button15" runat="server" CausesValidation="False" CssClass="btn btn-primary"
                                OnClick="btnCancelPopLeaveView_Click" Text="<%$ Resources:Attendance,Reset %>" />
                            <button type="button" class="btn btn-danger" data-dismiss="modal">
                                Close</button>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
            </div>
        </div>
    </div>

    <div class="modal fade" id="Modal_Alert" tabindex="-1" role="dialog" aria-labelledby="Modal_AlertLabel"
        aria-hidden="true">
        <div class="modal-dialog modal-lg">
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal">
                        <span aria-hidden="true">&times;</span><span class="sr-only">Close</span></button>
                    <h4 class="modal-title" id="Modal_AlertLabel">
                        <asp:Label ID="Label4" runat="server" Font-Size="14px" Font-Bold="true" CssClass="labelComman"
                            Text="<%$ Resources:Attendance,Edit %>"></asp:Label></h4>
                </div>
                <div class="modal-body">
                    <asp:UpdatePanel ID="Update_Modal_Alert" runat="server">
                        <ContentTemplate>
                            <div class="col-md-12">
                                <div class="box box-warning box-solid">
                                    <div class="box-header with-border">
                                        <h3 class="box-title">
                                            <asp:Label ID="Label51" runat="server" Font-Size="14px" Font-Bold="true" CssClass="labelComman"
                                                Text="<%$ Resources:Attendance,Edit %>"></asp:Label>
                                        </h3>
                                        <div class="box-tools pull-right">
                                            <asp:Button ID="Btn_Alert_Close" runat="server" CausesValidation="False" CssClass="btn btn-box-tool"
                                                OnClick="btnClosePanel_ClickNf" Text="X" />
                                        </div>
                                    </div>
                                    <div class="box-body">
                                        <div class="row">
                                            <div class="col-md-12">
                                                <div class="col-md-6">
                                                    <asp:Label ID="Label56" runat="server" ForeColor="#666666" Font-Bold="true" Font-Names="arial"
                                                        Font-Size="13px" Text="<%$ Resources:Attendance,Employee Code %>"></asp:Label>
                                                    &nbsp : &nbsp 
                                                    <asp:Label ID="lblEmpCodeNF" runat="server" ForeColor="#666666" Font-Bold="true"
                                                        Font-Names="arial" Font-Size="13px"></asp:Label>
                                                    <br />
                                                </div>
                                                <div class="col-md-6">
                                                    <asp:Label ID="Label57" runat="server" ForeColor="#666666" Font-Bold="true" Font-Names="arial"
                                                        Font-Size="13px" Text="<%$ Resources:Attendance,Employee Name %>"></asp:Label>
                                                    &nbsp : &nbsp
                                                    <asp:Label ID="lblEmpNameNf" runat="server" ForeColor="#666666" Font-Bold="true"
                                                        Font-Names="arial" Font-Size="13px"></asp:Label>
                                                    <br />
                                                </div>
                                            </div>
                                            <div class="col-md-12">
                                                <br />
                                            </div>
                                            <div class="col-md-12">
                                                <div class="box box-primary">
                                                    <div class="box-body">
                                                        <div class="form-group">
                                                            <%--<div class="col-md-12">--%>
                                                            <asp:Label ID="Label53" runat="server" ForeColor="#666666" Font-Names="arial" Font-Size="13px"
                                                                Font-Bold="true" Text="<%$ Resources:Attendance,Report %>"></asp:Label>
                                                            <br />
                                                            <asp:CheckBoxList ID="ChkReportList_popup" runat="server" RepeatColumns="4" Font-Names="Trebuchet MS" Width="100%"
                                                                CellSpacing="8" Font-Size="Small" ForeColor="Gray">
                                                            </asp:CheckBoxList>
                                                            <%--</div>--%>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="col-md-12">
                                                <div class="box box-primary">
                                                    <div class="box-body">
                                                        <div class="form-group">
                                                            <asp:Label ID="Label54" runat="server" ForeColor="#666666" Font-Names="arial" Font-Size="13px"
                                                                Font-Bold="true" Text="<%$ Resources:Attendance,SMS %>"></asp:Label>
                                                            <br />
                                                            <asp:CheckBoxList ID="ChkSmsList_popup" runat="server" RepeatColumns="4" Font-Names="Trebuchet MS" Width="100%"
                                                                Font-Size="Small" CellSpacing="15" ForeColor="Gray">
                                                            </asp:CheckBoxList>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="col-md-12">
                                                <div class="box box-primary">
                                                    <div class="box-body">
                                                        <div class="form-group">
                                                            <asp:Label ID="Label82" runat="server" ForeColor="#666666" Font-Names="arial" Font-Size="13px"
                                                                Font-Bold="true" Text="<%$ Resources:Attendance,Email %>"></asp:Label>
                                                            <br />
                                                            <asp:CheckBoxList ID="ChkEmailList_popup" runat="server" RepeatColumns="4" Font-Names="Trebuchet MS" Width="100%"
                                                                Font-Size="Small" CellSpacing="15" ForeColor="Gray">
                                                            </asp:CheckBoxList>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                            <div style="display: none;" class="col-md-12">
                                                <div class="col-md-6"></div>
                                                <div class="col-md-6">
                                                    <asp:Label ID="Label83" runat="server" Text="Schedule Days"></asp:Label>
                                                    <asp:TextBox onkeydown="return (event.keyCode!=13);" ID="txtScheduleDays_Popup" runat="server" CssClass="form-control"></asp:TextBox>
                                                    <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender6" runat="server" Enabled="True"
                                                        TargetControlID="txtScheduleDays_Popup" ValidChars="1,2,3,4,5,6,7,8,9,0">
                                                    </cc1:FilteredTextBoxExtender>
                                                    <br />
                                                    <asp:CheckBox ID="chkprevious" runat="server" Text="Delete Previous Report setup" />
                                                    <br />
                                                </div>
                                            </div>
                                            <asp:HiddenField ID="hdnEmpIdNF" runat="server" />
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
                <div class="modal-footer">
                    <asp:UpdatePanel ID="Update_Modal_Alert_Button" runat="server">
                        <ContentTemplate>
                            <asp:Button ID="Button4" runat="server" CssClass="btn btn-primary" OnClick="btnSaveNF_Click"
                                Text="<%$ Resources:Attendance,Save %>" Visible="false" />

                            <asp:Button ID="Button7" runat="server" CssClass="btn btn-primary"
                                OnClick="btnUpdateNotice_Click" Text="<%$ Resources:Attendance,Save %>" />

                            <asp:Button ID="Button8" runat="server" CausesValidation="False" CssClass="btn btn-primary" Visible="false"
                                OnClick="btnCancelPopLeave_Click1" Text="<%$ Resources:Attendance,Cancel %>" />

                            <button type="button" class="btn btn-danger" data-dismiss="modal">
                                Close</button>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
            </div>
        </div>
    </div>


    <div class="modal fade" id="Modal_Salary" tabindex="-1" role="dialog" aria-labelledby="Modal_SalaryLabel"
        aria-hidden="true">
        <div class="modal-dialog modal-lg">
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal">
                        <span aria-hidden="true">&times;</span><span class="sr-only">Close</span></button>
                    <h4 class="modal-title" id="Modal_SalaryLabel">
                        <asp:Label ID="Label13" runat="server" Font-Size="14px" Font-Bold="true" CssClass="labelComman"
                            Text="<%$ Resources:Attendance,Edit %>"></asp:Label></h4>
                </div>
                <div class="modal-body">
                    <asp:UpdatePanel ID="Update_Modal_Salary" runat="server">
                        <ContentTemplate>
                            <div class="row">
                                <div class="col-md-12">
                                    <div class="col-md-6">
                                        <asp:Label ID="Label40" runat="server" ForeColor="#666666" Font-Bold="true" Font-Names="arial"
                                            Font-Size="13px" Text="<%$ Resources:Attendance,Employee Code %>"></asp:Label>
                                        &nbsp : &nbsp
                                                    <asp:Label ID="lblEmpCodeSal" runat="server" ForeColor="#666666" Font-Bold="true"
                                                        Font-Names="arial" Font-Size="13px"></asp:Label>
                                        <br />
                                    </div>
                                    <div class="col-md-6">
                                        <asp:Label ID="Label50" runat="server" ForeColor="#666666" Font-Bold="true" Font-Names="arial"
                                            Font-Size="13px" Text="<%$ Resources:Attendance,Employee Name %>"></asp:Label>
                                        &nbsp : &nbsp
                                                    <asp:Label ID="lblEmpNameSal" runat="server" ForeColor="#666666" Font-Bold="true"
                                                        Font-Names="arial" Font-Size="13px"></asp:Label>
                                        <br />
                                    </div>
                                </div>

                                <div class="col-md-12">
                                    <div class="col-md-6" id="Div_GrossSalary_1" runat="server">
                                        <asp:Label ID="Label9" runat="server" Text="<%$ Resources:Attendance,Gross Salary %>"></asp:Label><a style="color: Red">*</a>
                                        <asp:RequiredFieldValidator EnableClientScript="true" Style="float: right;" runat="server" ID="RequiredFieldValidator24" ValidationGroup="S_update" Display="Dynamic" SetFocusOnError="true" ControlToValidate="txtGrossSalary" ErrorMessage="Enter Gross Salary" />
                                        <asp:TextBox onkeydown="return (event.keyCode!=13);" ID="txtGrossSalary" OnTextChanged="txtGrossSalary_TextChanged" AutoPostBack="true" CssClass="form-control" runat="server"></asp:TextBox>
                                        <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender19" runat="server" Enabled="True"
                                            TargetControlID="txtGrossSalary" ValidChars="1,2,3,4,5,6,7,8,9,0,.">
                                        </cc1:FilteredTextBoxExtender>
                                        <br />
                                    </div>
                                    <div id="Div_Salary_Plan1" runat="server" class="col-md-6">
                                        <asp:Label ID="Label12" runat="server" Text="Salary Plan"></asp:Label><a style="color: Red">*</a>
                                        <asp:RequiredFieldValidator EnableClientScript="true" Style="float: right;" runat="server" ID="RequiredFieldValidator27" ValidationGroup="S_update"
                                            Display="Dynamic" SetFocusOnError="true" ControlToValidate="ddlSalaryPlan1" InitialValue="--Select--" ErrorMessage="<%$ Resources:Attendance,Select Salary Plan%>" />
                                        <asp:DropDownList ID="ddlSalaryPlan1" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlSalaryPlan1_SelectedIndexChanged" CssClass="form-control"></asp:DropDownList>
                                        <br />
                                    </div>

                                    <div id="Div_Basic_Salary" runat="server" class="col-md-6">
                                        <asp:Label ID="Label33" runat="server" Text="<%$ Resources:Attendance,Basic Salary %>"></asp:Label>
                                        <asp:TextBox onkeydown="return (event.keyCode!=13);" ID="txtBasic1" CssClass="form-control" runat="server"></asp:TextBox>
                                        <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender29" runat="server" Enabled="True"
                                            TargetControlID="txtBasic1" ValidChars="1,2,3,4,5,6,7,8,9,0,.">
                                        </cc1:FilteredTextBoxExtender>
                                        <br />
                                    </div>
                                    <div class="col-md-6">
                                        <asp:Label ID="Label34" runat="server" Text="<%$ Resources:Attendance,Work Minute %>"></asp:Label><a style="color: Red">*</a>
                                        <asp:RequiredFieldValidator EnableClientScript="true" Style="float: right;" runat="server" ID="RequiredFieldValidator21" ValidationGroup="S_update"
                                            Display="Dynamic" SetFocusOnError="true" ControlToValidate="txtWorkMin1" ErrorMessage="<%$ Resources:Attendance,Enter Work Minute %>" />
                                        <asp:TextBox ID="txtWorkMin1" CssClass="form-control" onkeydown="return (event.keyCode!=13);" MaxLength="4" runat="server"></asp:TextBox>
                                        <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender30" runat="server" Enabled="True"
                                            TargetControlID="txtWorkMin1" ValidChars="1,2,3,4,5,6,7,8,9,0,.">
                                        </cc1:FilteredTextBoxExtender>
                                        <br />
                                    </div>

                                    <div class="col-md-6">
                                        <asp:Label ID="Label38" runat="server" Text="<%$ Resources:Attendance,Payment Type %>"></asp:Label><a style="color: Red">*</a>
                                        <asp:DropDownList ID="ddlPaymentType1" runat="server" CssClass="form-control">
                                            <%--  <asp:ListItem Value="Hourly">Hourly</asp:ListItem>
                                                                        <asp:ListItem Value="Daily">Daily</asp:ListItem>
                                                                        <asp:ListItem Value="Weekly">Weekly</asp:ListItem>--%>
                                            <asp:ListItem Value="Monthly">Monthly</asp:ListItem>
                                        </asp:DropDownList>
                                        <br />
                                    </div>
                                    <div class="col-md-6">
                                        <asp:Label ID="Label52" runat="server" Text="<%$ Resources:Attendance,Work Calculation Method %>"></asp:Label><a style="color: Red">*</a>
                                        <asp:DropDownList ID="ddlWorkCal1" runat="server" CssClass="form-control">
                                            <asp:ListItem Value="PairWise">PairWise</asp:ListItem>
                                            <asp:ListItem Value="InOut">InOut</asp:ListItem>
                                        </asp:DropDownList>
                                        <br />
                                    </div>







                                    <div style="display: none;" class="col-md-6">
                                        <asp:Label ID="Label37" runat="server" Text="<%$ Resources:Attendance,Currency %>"></asp:Label>
                                        <asp:DropDownList ID="ddlCurrency1" runat="server" CssClass="form-control">
                                        </asp:DropDownList>
                                        <br />
                                    </div>
                                    <div class="col-md-6">
                                        <asp:Label ID="Label39" runat="server" Text="Mobile Bill Limit"></asp:Label>
                                        <asp:TextBox onkeydown="return (event.keyCode!=13);" ID="txtMobilleBillLimit1" CssClass="form-control" MaxLength="8" runat="server"></asp:TextBox>
                                        <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender21" runat="server" Enabled="True"
                                            TargetControlID="txtMobilleBillLimit1" ValidChars="1,2,3,4,5,6,7,8,9,0,.">
                                        </cc1:FilteredTextBoxExtender>
                                        <br />
                                    </div>

                                    <div class="col-md-6">
                                        <asp:Label ID="Label164" runat="server" Text="<%$ Resources:Attendance,Salary Payment Option%>"></asp:Label>
                                        <asp:TextBox onkeydown="return (event.keyCode!=13);" ID="Txt_Salary_Payment_Option_M" runat="server" CssClass="form-control" BackColor="#eeeeee"
                                            AutoPostBack="true" OnTextChanged="Txt_Salary_Payment_Option_TextChanged"></asp:TextBox>
                                        <cc1:AutoCompleteExtender ID="AutoCompleteExtender5" runat="server" DelimiterCharacters=""
                                            Enabled="True" ServiceMethod="GetCompletionListAccountName" ServicePath="" CompletionInterval="100"
                                            MinimumPrefixLength="1" CompletionSetCount="1" TargetControlID="Txt_Salary_Payment_Option_M"
                                            UseContextKey="True" CompletionListCssClass="autoCompleteList" CompletionListItemCssClass="autoCompleteListItem" CompletionListHighlightedItemCssClass="autoCompleteSelectedListItem" OnClientShown="resetPosition_popup">
                                        </cc1:AutoCompleteExtender>
                                        <br />
                                    </div>
                                </div>
                                <div class="col-md-12">
                                    <div class="col-md-6">
                                        <br />
                                        <asp:CheckBox ID="chkisCtcEmployee1" runat="server" />
                                        <asp:Label ID="Label132" runat="server" Text="Is Ctc Employee"></asp:Label>
                                    </div>
                                    <div class="col-md-6">
                                        <br />
                                        <asp:CheckBox ID="chkEmpINPayroll1" runat="server" AutoPostBack="true" OnCheckedChanged="chkEmpINPayroll_OnCheckedChanged" />
                                        <asp:Label ID="Label139" runat="server" Text="<%$ Resources:Attendance,Employee In Payroll %>"></asp:Label>
                                    </div>
                                </div>

                                <div class="col-md-12">
                                    <div class="col-md-6">
                                        <br />
                                        <asp:CheckBox ID="chkEmpPf1" runat="server" />
                                        <asp:Label ID="Label140" runat="server" Text="<%$ Resources:Attendance,Employee PF %>"></asp:Label>
                                    </div>
                                    <div class="col-md-6">
                                        <br />
                                        <asp:CheckBox ID="chkEmpEsic1" runat="server" />
                                        <asp:Label ID="Label141" runat="server" Text="<%$ Resources:Attendance,Employee ESIC %>"></asp:Label>
                                    </div>
                                </div>

                                <div class="col-md-12">
                                    <hr />
                                    <div id="Div_Box_Salary" class="box box-info collapsed-box">
                                        <asp:Label ID="Label79" runat="server" Text="<%$ Resources:Attendance,Percentage of salary increment %>" Visible="false"></asp:Label>

                                        <div class="box-header with-border">
                                            <h3 class="box-title">Salary Increment</h3>
                                            <div class="box-tools pull-right">
                                                <button type="button" class="btn btn-box-tool" data-widget="collapse">
                                                    <i id="Btn_Salary_Div" class="fa fa-plus"></i>
                                                </button>
                                            </div>
                                        </div>
                                        <div class="box-body">
                                            <div class="form-group">
                                                <div class="col-md-6">
                                                    <asp:Label ID="Label119" runat="server" Text="<%$ Resources:Attendance,Category %>"></asp:Label>
                                                    <asp:DropDownList ID="ddlCategory1" runat="server" CssClass="form-control"
                                                        AutoPostBack="true" OnSelectedIndexChanged="ddlCategory1_OnSelectedIndexChanged">
                                                        <asp:ListItem Text="<%$ Resources:Attendance,Fresher %>" Value="Fresher"></asp:ListItem>
                                                        <asp:ListItem Text="<%$ Resources:Attendance,Experience %>" Value="Experience"></asp:ListItem>
                                                    </asp:DropDownList>
                                                    <br />
                                                </div>
                                                <div class="col-md-12">
                                                </div>
                                                <div class="col-md-6">
                                                    <asp:Label ID="Label135" runat="server" Text="<%$ Resources:Attendance,Duration of salary increment %>"></asp:Label>
                                                    <div class="input-group">
                                                        <asp:TextBox onkeydown="return (event.keyCode!=13);" ID="txtSalIncrDuration1" MaxLength="2" CssClass="form-control" runat="server" />
                                                        <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender32" runat="server" Enabled="True"
                                                            TargetControlID="txtSalIncrDuration1" ValidChars="1,2,3,4,5,6,7,8,9,0">
                                                        </cc1:FilteredTextBoxExtender>
                                                        <span class="input-group-addon">
                                                            <asp:Label ID="Label136" runat="server" Text="<%$ Resources:Attendance,In Months %>"></asp:Label></span>
                                                    </div>
                                                    <br />
                                                </div>
                                                <div class="col-md-12">
                                                </div>
                                                <div class="col-md-6">
                                                    <asp:Label ID="Label137" runat="server" Text="<%$ Resources:Attendance,From %>"></asp:Label>
                                                    <div class="input-group">
                                                        <asp:TextBox onkeydown="return (event.keyCode!=13);" ID="txtIncrementPerFrom1" MaxLength="2" CssClass="form-control" runat="server" />
                                                        <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender34" runat="server" Enabled="True"
                                                            TargetControlID="txtIncrementPerFrom1" ValidChars="1,2,3,4,5,6,7,8,9,0">
                                                        </cc1:FilteredTextBoxExtender>
                                                        <span class="input-group-addon">In %</span>
                                                    </div>
                                                    <br />
                                                </div>
                                                <div class="col-md-6">
                                                    <asp:Label ID="Label138" runat="server" Text="<%$ Resources:Attendance,To %>"></asp:Label>
                                                    <div class="input-group">
                                                        <asp:TextBox onkeydown="return (event.keyCode!=13);" ID="txtIncrementPerTo1" MaxLength="2" CssClass="form-control" runat="server" />
                                                        <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender36" runat="server" Enabled="True"
                                                            TargetControlID="txtIncrementPerTo1" ValidChars="1,2,3,4,5,6,7,8,9,0">
                                                        </cc1:FilteredTextBoxExtender>
                                                        <span class="input-group-addon">In %</span>
                                                    </div>
                                                    <br />
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>

                                <%-- Indemnity/Gratuity Disable by Ghanshyam Suthar on 15-11-2017 according by Neelkanth Sir--%>
                                <div style="display: none" class="col-md-12">
                                    <div class="col-md-4"></div>
                                    <div id="Div_Box_Indemnity" style="display: none;" class="col-md-4 box box-info collapsed-box">
                                        <div class="box-header with-border">
                                            <h3 class="box-title">Indemnity/Gratuity</h3>
                                            <div class="box-tools pull-right">
                                                <button type="button" class="btn btn-box-tool" data-widget="collapse">
                                                    <i id="Btn_Indemnity_Div" class="fa fa-plus"></i>
                                                </button>
                                            </div>
                                        </div>
                                        <div class="box-body">
                                            <div class="form-group">
                                                <asp:Label ID="Label142" Font-Bold="true" Font-Size="13px" Font-Names="Arial" runat="server" Visible="false"
                                                    Text="<%$ Resources:Attendance,Indemnity %>"></asp:Label>
                                                <asp:Label ID="Label143" runat="server" Text="<%$ Resources:Attendance,Indemnity Functionality %>"></asp:Label>
                                                <asp:RadioButton ID="rbnIndemnity1PopUp" GroupName="Indemnity_Popup" Text="<%$ Resources:Attendance,Enable %>"
                                                    runat="server" AutoPostBack="true" OnCheckedChanged="rbnIndemnity1PopUp_OnCheckedChanged" />
                                                <asp:RadioButton ID="rbnIndemnity2PopUp" Style="margin: 15px;" GroupName="Indemnity_Popup" Text="<%$ Resources:Attendance,Disable %>"
                                                    runat="server" AutoPostBack="true" OnCheckedChanged="rbnIndemnity2PopUp_OnCheckedChanged" />
                                                <br />
                                                <asp:Label ID="Label144" runat="server" Text="<%$ Resources:Attendance,Indemnity Duration(Year)%>"></asp:Label>
                                                <asp:TextBox onkeydown="return (event.keyCode!=13);" ID="txtIndemnityYearPopUp" runat="server" CssClass="form-control"></asp:TextBox>
                                                <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender37" runat="server" TargetControlID="txtIndemnityYearPopUp"
                                                    FilterType="Numbers">
                                                </cc1:FilteredTextBoxExtender>
                                                <br />
                                                <asp:Label ID="Label145" runat="server" Text="<%$ Resources:Attendance,Given Leave Salary Days(Year)%>"></asp:Label>
                                                <asp:TextBox onkeydown="return (event.keyCode!=13);" ID="txtIndemnityDaysPopUP" runat="server" CssClass="form-control" MaxLength="4"></asp:TextBox>
                                                <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender38" runat="server" Enabled="True"
                                                    TargetControlID="txtIndemnityDaysPopUP" FilterType="Numbers">
                                                </cc1:FilteredTextBoxExtender>
                                                <br />
                                            </div>
                                        </div>
                                    </div>
                                    <div class="col-md-4"></div>
                                </div>
                                <%--------------------------------------------------------------------------------------------%>
                            </div>


                            <div class="row">
                                <div class="col-md-12">
                                    <div id="Div_Box_Add1" class="box box-info collapsed-box">
                                        <div class="box-header with-border">
                                            <h3 class="box-title">Add Previous Employer and Opening Balance information</h3>
                                            <div class="box-tools pull-right">
                                                <button type="button" class="btn btn-box-tool" data-widget="collapse">
                                                    <i id="Btn_Add_Div1" class="fa fa-plus"></i>
                                                </button>
                                            </div>
                                        </div>
                                        <div class="box-body">
                                            <div class="form-group">
                                                <div class="col-md-6">
                                                    <asp:Label ID="Label134" runat="server" Text="Opening Credit Amount"></asp:Label>


                                                    <asp:TextBox onkeydown="return (event.keyCode!=13);" ID="txtEditOpeningCreditAmt" Enabled="false" runat="server" CssClass="form-control" OnBlur="EditOpeningBalance();"></asp:TextBox>
                                                    <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender39" runat="server" Enabled="True"
                                                        TargetControlID="txtEditOpeningCreditAmt" ValidChars="1,2,3,4,5,6,7,8,9,0,.">
                                                    </cc1:FilteredTextBoxExtender>
                                                    <br />
                                                </div>
                                                <div class="col-md-6">
                                                    <asp:Label ID="Label146" runat="server" Text="Opening Debit Amount"></asp:Label>

                                                    <asp:TextBox onkeydown="return (event.keyCode!=13);" ID="txtEditOpeningDebitAmt" Enabled="false" runat="server" CssClass="form-control" OnBlur="EditOpeningBalance();"></asp:TextBox>
                                                    <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender40" runat="server" Enabled="True"
                                                        TargetControlID="txtEditOpeningDebitAmt" ValidChars="1,2,3,4,5,6,7,8,9,0,.">
                                                    </cc1:FilteredTextBoxExtender>
                                                    <br />
                                                </div>

                                                <div class="col-md-6">
                                                    <asp:Label ID="Label147" runat="server" Text="Total Earning (Current Financial Year)"></asp:Label>
                                                    <asp:TextBox onkeydown="return (event.keyCode!=13);" ID="txtEditEmployerTotalEarning" Enabled="false" runat="server" CssClass="form-control"></asp:TextBox>
                                                    <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender41" runat="server" Enabled="True"
                                                        TargetControlID="txtEditEmployerTotalEarning" ValidChars="1,2,3,4,5,6,7,8,9,0,.">
                                                    </cc1:FilteredTextBoxExtender>
                                                    <br />
                                                </div>
                                                <div class="col-md-6">
                                                    <asp:Label ID="Label148" runat="server" Text="Total TDS (Current Financial Year)"></asp:Label>

                                                    <asp:TextBox onkeydown="return (event.keyCode!=13);" ID="txtEdittOtalTDS" runat="server" Enabled="false" CssClass="form-control"></asp:TextBox>
                                                    <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender42" runat="server" Enabled="True"
                                                        TargetControlID="txtEdittOtalTDS" ValidChars="1,2,3,4,5,6,7,8,9,0,.">
                                                    </cc1:FilteredTextBoxExtender>
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
                    <asp:UpdatePanel ID="Update_Modal_Button_Salary" runat="server">
                        <ContentTemplate>
                            <asp:Button ID="Button3" runat="server" CssClass="btn btn-primary" OnClick="btnUpdateSal_Click" ValidationGroup="S_update"
                                Text="<%$ Resources:Attendance,Save %>" />

                            <asp:Button ID="Button5" runat="server" CausesValidation="False" CssClass="btn btn-primary"
                                OnClick="btnCancelPopSal_Click" Text="<%$ Resources:Attendance,Reset %>" />

                            <asp:HiddenField ID="hdnEmpIdSal" runat="server" />

                            <button type="button" class="btn btn-danger" data-dismiss="modal">
                                Close</button>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
            </div>
        </div>
    </div>




    <div class="modal fade" id="Modal_Hierarchy" tabindex="-1" role="dialog" aria-labelledby="Modal_SalaryLabel"
        aria-hidden="true">
        <div class="modal-dialog modal-lg">
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal">
                        <span aria-hidden="true">&times;</span><span class="sr-only">Close</span></button>
                    <h4 class="modal-title" id="Modal_HierarchyLabel">
                        <asp:Label ID="Label155" runat="server" Font-Size="14px" Font-Bold="true" CssClass="labelComman"
                            Text="Employee Hierarchy"></asp:Label></h4>
                </div>
                <div class="modal-body">
                    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                        <ContentTemplate>
                            <div class="row">
                                <div class="col-md-12">
                                    <asp:TreeView ID="TreeView_hierarchy" runat="server">
                                    </asp:TreeView>
                                </div>
                                <%--------------------------------------------------------------------------------------------%>
                            </div>



                        </ContentTemplate>
                    </asp:UpdatePanel>

                </div>
                <div class="modal-footer">
                    <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                        <ContentTemplate>


                            <button type="button" class="btn btn-danger" data-dismiss="modal">
                                Close</button>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
            </div>
        </div>
    </div>



    <div class="modal fade" id="Modal_OT_PL" tabindex="-1" role="dialog" aria-labelledby="Modal_OT_PLLabel"
        aria-hidden="true">
        <div class="modal-dialog modal-lg">
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal">
                        <span aria-hidden="true">&times;</span><span class="sr-only">Close</span></button>
                    <h4 class="modal-title" id="Modal_OT_PLLabel">
                        <asp:Label ID="Label7" runat="server" Font-Size="14px" Font-Bold="true" CssClass="labelComman"
                            Text="<%$ Resources:Attendance,Edit %>"></asp:Label></h4>
                </div>
                <div class="modal-body">
                    <asp:UpdatePanel ID="Update_Modal_OT_PL" runat="server">
                        <ContentTemplate>
                            <div class="row">
                                <div class="col-md-12">
                                    <div class="col-md-6">
                                        <asp:Label ID="Label91" runat="server" Font-Size="14px" Font-Bold="true" Text="<%$ Resources:Attendance,Employee Code %>"></asp:Label>
                                        &nbsp : &nbsp<asp:Label ID="lblEmpCodeOT" runat="server" Font-Size="14px" Font-Bold="true"></asp:Label>
                                        <br />
                                    </div>
                                    <div class="col-md-6">
                                        <asp:Label ID="Label94" runat="server" Font-Size="14px" Font-Bold="true" Text="<%$ Resources:Attendance,Employee Name %>"></asp:Label>
                                        &nbsp : &nbsp<asp:Label ID="lblEmpNameOT" runat="server" Font-Size="14px" Font-Bold="true"></asp:Label>
                                        <br />
                                    </div>
                                </div>
                                <div class="col-md-12">
                                    <hr />
                                </div>
                                <div class="col-md-12">

                                    <div class="col-md-6">
                                        <asp:Label ID="Label63" Font-Bold="true" Font-Size="13px" Font-Names="Arial" runat="server"
                                            Text="<%$ Resources:Attendance,Partial Leave %>"></asp:Label>
                                        <br />
                                        <asp:Label ID="Label64" runat="server" Text="<%$ Resources:Attendance,Partial Leave Functionality %>"></asp:Label>
                                        <asp:RadioButton ID="rbtnPartialEnable1" Checked="true" Style="margin-left: 10px; margin-right: 10px;" GroupName="Edit_Partial" Text="<%$ Resources:Attendance,Enable %>" runat="server" AutoPostBack="true" OnCheckedChanged="rbtPartial1_OnCheckedChanged" />
                                        <asp:RadioButton ID="rbtnPartialDisable1" Style="margin-left: 10px; margin-right: 10px;" GroupName="Edit_Partial" Text="<%$ Resources:Attendance,Disable %>" runat="server" AutoPostBack="true" OnCheckedChanged="rbtPartial1_OnCheckedChanged" />
                                        <br />
                                        <asp:Label ID="Label65" runat="server" Text="<%$ Resources:Attendance,Total Minutes For Month  %>"></asp:Label>
                                        <asp:TextBox onkeydown="return (event.keyCode!=13);" ID="txtTotalMinutesP1" runat="server" CssClass="form-control" />
                                        <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender13" runat="server" Enabled="True"
                                            TargetControlID="txtTotalMinutesP1" FilterType="Numbers">
                                        </cc1:FilteredTextBoxExtender>
                                        <br />
                                        <asp:Label ID="Label68" runat="server" Text="<%$ Resources:Attendance,Minute Use in a Day One Time %>"></asp:Label>
                                        <asp:TextBox onkeydown="return (event.keyCode!=13);" ID="txtMinuteOTOne" runat="server" CssClass="form-control" />
                                        <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender14" runat="server" Enabled="True"
                                            TargetControlID="txtMinuteOTOne" FilterType="Numbers">
                                        </cc1:FilteredTextBoxExtender>
                                        <br />
                                        <div id="Div1" runat="server" visible="false">
                                            <asp:Label ID="Label69" runat="server" Text="<%$ Resources:Attendance,Carry Forward Minutes %>"></asp:Label>
                                            <asp:RadioButton ID="rbtnCarryYes1" Checked="true" Style="margin-left: 10px; margin-right: 10px;" GroupName="Carry1" Text="<%$ Resources:Attendance,Yes %>" runat="server" />
                                            <asp:RadioButton ID="rbtnCarryNo1" Style="margin-left: 10px; margin-right: 10px;" GroupName="Carry1" Text="<%$ Resources:Attendance,No %>" runat="server" />
                                        </div>
                                        <br />
                                    </div>
                                    <div class="col-md-6">
                                        <asp:Label ID="Label70" Font-Bold="true" Font-Size="13px" Font-Names="Arial" runat="server"
                                            Text="<%$ Resources:Attendance,Over Time %>"></asp:Label>
                                        <br />
                                        <asp:Label ID="Label71" runat="server" Text="<%$ Resources:Attendance,Over Time Functionality %>"></asp:Label>
                                        <asp:RadioButton ID="rbtnOTEnable1" Checked="true" Style="margin-left: 10px; margin-right: 10px;" GroupName="Edit_OT" Text="<%$ Resources:Attendance,Enable %>" runat="server" AutoPostBack="true" OnCheckedChanged="rbtnOTEnable1_CheckedChanged" />
                                        <asp:RadioButton ID="rbtnOTDisable1" Style="margin-left: 10px; margin-right: 10px;" GroupName="Edit_OT" Text="<%$ Resources:Attendance,Disable %>" runat="server" AutoPostBack="true" OnCheckedChanged="rbtnOTEnable1_CheckedChanged" />
                                        <br />
                                        <asp:Label ID="Label72" runat="server" Text="<%$ Resources:Attendance,Calculation Method %>"></asp:Label>
                                        <asp:DropDownList ID="ddlOTCalc1" runat="server" CssClass="form-control">
                                            <asp:ListItem Text="In" Value="In"></asp:ListItem>
                                            <asp:ListItem Text="Out" Value="Out"></asp:ListItem>
                                            <asp:ListItem Text="Both" Value="Both"></asp:ListItem>
                                            <asp:ListItem Text="Work Hour" Value="Work Hour"></asp:ListItem>
                                        </asp:DropDownList>
                                        <br />
                                        <asp:Label ID="Label73" runat="server" Text="<%$ Resources:Attendance,Value(Normal Day) %>"></asp:Label>
                                        <div class="input-group" style="width: 100%">
                                            <asp:TextBox onkeydown="return (event.keyCode!=13);" ID="txtNormal1" Style="width: 70%;" runat="server" CssClass="form-control" />&nbsp;
                                                                                    <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender15" runat="server" Enabled="True"
                                                                                        TargetControlID="txtNormal1" ValidChars="1,2,3,4,5,6,7,8,9,0,.">
                                                                                    </cc1:FilteredTextBoxExtender>
                                            <asp:DropDownList ID="ddlNormalType1" Style="width: 28%;" runat="server" CssClass="form-control">
                                                <asp:ListItem Text="Fixed" Value="1"></asp:ListItem>
                                                <asp:ListItem Text="%" Value="2"></asp:ListItem>
                                            </asp:DropDownList>
                                        </div>
                                        <br />
                                        <asp:Label ID="Label74" runat="server" Text="<%$ Resources:Attendance,Value(Week Off) %>"></asp:Label>
                                        <div class="input-group" style="width: 100%">
                                            <asp:TextBox onkeydown="return (event.keyCode!=13);" ID="txtWeekOffValue1" Style="width: 70%;" runat="server" CssClass="form-control" />&nbsp;
                                                                                    <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender16" runat="server" Enabled="True"
                                                                                        TargetControlID="txtWeekOffValue1" ValidChars="1,2,3,4,5,6,7,8,9,0,.">
                                                                                    </cc1:FilteredTextBoxExtender>
                                            <asp:DropDownList ID="ddlWeekOffType1" Style="width: 28%;" runat="server" CssClass="form-control">
                                                <asp:ListItem Text="Fixed" Value="1"></asp:ListItem>
                                                <asp:ListItem Text="%" Value="2"></asp:ListItem>
                                            </asp:DropDownList>
                                        </div>
                                        <br />
                                        <asp:HiddenField ID="HiddenField2" runat="server" />
                                        <asp:Label ID="Label75" runat="server" Text="<%$ Resources:Attendance,Value(Holiday) %>"></asp:Label>
                                        <div class="input-group" style="width: 100%">
                                            <asp:TextBox onkeydown="return (event.keyCode!=13);" ID="txtHolidayValue1" Style="width: 70%;" runat="server" CssClass="form-control" />&nbsp;
                                                                                    <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender17" runat="server" Enabled="True"
                                                                                        TargetControlID="txtHolidayValue1" ValidChars="1,2,3,4,5,6,7,8,9,0,.">
                                                                                    </cc1:FilteredTextBoxExtender>
                                            <asp:DropDownList ID="ddlHolidayValue1" Style="width: 28%;" runat="server" CssClass="form-control">
                                                <asp:ListItem Text="Fixed" Value="1"></asp:ListItem>
                                                <asp:ListItem Text="%" Value="2"></asp:ListItem>
                                            </asp:DropDownList>
                                        </div>
                                        <br />
                                    </div>
                                </div>
                            </div>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
                <div class="modal-footer">
                    <asp:UpdatePanel ID="Update_Modal_Button_OT_PL" runat="server">
                        <ContentTemplate>
                            <asp:Button ID="btnUpdateOt" runat="server" CssClass="btn btn-success" OnClick="btnUpdateOT_Click"
                                Text="<%$ Resources:Attendance,Save %>" />

                            <asp:Button ID="Button14" runat="server" CausesValidation="False" CssClass="btn btn-primary"
                                OnClick="btnCancelPopOT_Click" Text="<%$ Resources:Attendance,Reset %>" />

                            <button type="button" class="btn btn-danger" data-dismiss="modal">
                                Close</button>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
            </div>
        </div>
    </div>

    <div class="modal fade" id="Modal_Penalty" tabindex="-1" role="dialog" aria-labelledby="Modal_PenaltyLabel"
        aria-hidden="true">
        <div class="modal-dialog modal-lg">
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal">
                        <span aria-hidden="true">&times;</span><span class="sr-only">Close</span></button>
                    <h4 class="modal-title" id="Modal_PenaltyLabel">
                        <asp:Label ID="Label8" runat="server" Font-Size="14px" Font-Bold="true" CssClass="labelComman"
                            Text="<%$ Resources:Attendance,Edit %>"></asp:Label></h4>
                </div>
                <div class="modal-body">
                    <asp:UpdatePanel ID="Update_Modal_Penalty" runat="server">
                        <ContentTemplate>
                            <div class="row">
                                <div class="col-md-12">
                                    <div class="col-md-6">
                                        <asp:Label ID="Label95" runat="server" Font-Size="14px" Font-Bold="true"
                                            Text="<%$ Resources:Attendance,Employee Code %>"></asp:Label>
                                        &nbsp : &nbsp
                                                            <asp:Label ID="lblEmpCodePen" runat="server" Font-Size="14px" Font-Bold="true"></asp:Label>
                                        <br />
                                    </div>
                                    <div class="col-md-6">
                                        <asp:Label ID="Label103" runat="server" Font-Size="14px" Font-Bold="true"
                                            Text="<%$ Resources:Attendance,Employee Name %>"></asp:Label>
                                        &nbsp : &nbsp
                                                            <asp:Label ID="lblEmpNamePen" runat="server" Font-Size="14px" Font-Bold="true"></asp:Label>
                                        <br />
                                    </div>
                                </div>
                                <div class="col-md-12">
                                    <div class="col-md-3"></div>
                                    <div class="col-md-6">
                                        <br />
                                        <table style="width: 100%">
                                            <tr>
                                                <td>
                                                    <asp:Label ID="Label97" runat="server" Text="<%$ Resources:Attendance,Late In Functionality %>"></asp:Label>
                                                </td>
                                                <td>
                                                    <asp:RadioButton ID="rbtnLateInEnable" GroupName="Late1" Text="<%$ Resources:Attendance,Enable %>" runat="server" />
                                                </td>
                                                <td>
                                                    <asp:RadioButton ID="rbtnLateInDisable" GroupName="Late1" Text="<%$ Resources:Attendance,Disable %>" runat="server" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:Label ID="Label104" runat="server" Text="<%$ Resources:Attendance,Early Out Functionality %>"></asp:Label>
                                                </td>
                                                <td>
                                                    <asp:RadioButton ID="rbtnEarlyEnable" GroupName="Early1" Text="<%$ Resources:Attendance,Enable %>" runat="server" />
                                                </td>
                                                <td>
                                                    <asp:RadioButton ID="rbtnEarlyDisable" GroupName="Early1" Text="<%$ Resources:Attendance,Disable %>" runat="server" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:Label ID="Label105" runat="server" Text="<%$ Resources:Attendance,Absent Functionality %>"></asp:Label>
                                                </td>
                                                <td>
                                                    <asp:RadioButton ID="rbtnAbsentEnableP" GroupName="absent1" Text="<%$ Resources:Attendance,Enable %>" runat="server" />
                                                </td>
                                                <td>
                                                    <asp:RadioButton ID="rbtnAbsentDisableP" GroupName="absent1" Text="<%$ Resources:Attendance,Disable %>" runat="server" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:Label ID="Label106" runat="server" Text="Partial Leave Functionality"></asp:Label>
                                                </td>
                                                <td>
                                                    <asp:RadioButton ID="PnlPlEnable" GroupName="Partial1" Text="<%$ Resources:Attendance,Enable %>" runat="server" />
                                                </td>
                                                <td>
                                                    <asp:RadioButton ID="PnlPlDisable" GroupName="Partial1" Text="<%$ Resources:Attendance,Disable %>" runat="server" />
                                                </td>
                                            </tr>
                                        </table>
                                        <br />
                                    </div>
                                    <div class="col-md-3"></div>
                                </div>
                            </div>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
                <div class="modal-footer">
                    <asp:UpdatePanel ID="Update_Modal_Button_Penalty" runat="server">
                        <ContentTemplate>
                            <asp:HiddenField ID="HiddenField3" runat="server" />
                            <asp:Button ID="Button16" runat="server" CssClass="btn btn-success" OnClick="btnUpdatePenalty_Click"
                                Text="<%$ Resources:Attendance,Save %>" />

                            <asp:Button ID="Button18" runat="server" CausesValidation="False" CssClass="btn btn-primary"
                                OnClick="btnCancelPopPenalty_Click" Text="<%$ Resources:Attendance,Reset %>" />
                            <button type="button" class="btn btn-danger" data-dismiss="modal">
                                Close</button>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
            </div>
        </div>
    </div>


    <div class="modal fade" id="ModelUpdateLicense" role="dialog">
        <div class="modal-dialog modal-lg">

            <!-- Modal content-->
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal">&times;</button>
                    <div class="modal-title" style="text-align: left;">
                        <img src="https://www.pegasustech.net/image/catalog/logo.png" class="img-responsive" width="120px" />
                    </div>
                </div>

                <div>
                    <uc1:UpdateLicense ID="UC_LicenseInfo" runat="server" />
                </div>
                <div class="modal-footer">
                </div>
            </div>

        </div>
    </div>


    <asp:UpdateProgress ID="UpdateProgress20" runat="server" AssociatedUpdatePanelID="Modal_Buttopn_Leave">
        <ProgressTemplate>
            <div class="modal_Progress">
                <div class="center_Progress">
                    <img alt="" src="../Images/ajax-loader2.gif" style="vertical-align: middle; background-color: White; border-radius: 10px;" />
                </div>
            </div>
        </ProgressTemplate>
    </asp:UpdateProgress>

    <asp:UpdateProgress ID="UpdateProgress21" runat="server" AssociatedUpdatePanelID="Modal_Buttopn_Leave_View">
        <ProgressTemplate>
            <div class="modal_Progress">
                <div class="center_Progress">
                    <img alt="" src="../Images/ajax-loader2.gif" style="vertical-align: middle; background-color: White; border-radius: 10px;" />
                </div>
            </div>
        </ProgressTemplate>
    </asp:UpdateProgress>

    <asp:UpdateProgress ID="UpdateProgress22" runat="server" AssociatedUpdatePanelID="Update_Modal_Button_Salary">
        <ProgressTemplate>
            <div class="modal_Progress">
                <div class="center_Progress">
                    <img alt="" src="../Images/ajax-loader2.gif" style="vertical-align: middle; background-color: White; border-radius: 10px;" />
                </div>
            </div>
        </ProgressTemplate>
    </asp:UpdateProgress>

    <asp:UpdateProgress ID="UpdateProgress23" runat="server" AssociatedUpdatePanelID="Update_Modal_Button_OT_PL">
        <ProgressTemplate>
            <div class="modal_Progress">
                <div class="center_Progress">
                    <img alt="" src="../Images/ajax-loader2.gif" style="vertical-align: middle; background-color: White; border-radius: 10px;" />
                </div>
            </div>
        </ProgressTemplate>
    </asp:UpdateProgress>

    <asp:UpdateProgress ID="UpdateProgress24" runat="server" AssociatedUpdatePanelID="Update_Modal_Button_Penalty">
        <ProgressTemplate>
            <div class="modal_Progress">
                <div class="center_Progress">
                    <img alt="" src="../Images/ajax-loader2.gif" style="vertical-align: middle; background-color: White; border-radius: 10px;" />
                </div>
            </div>
        </ProgressTemplate>
    </asp:UpdateProgress>

    <asp:UpdateProgress ID="UpdateProgress2" runat="server" AssociatedUpdatePanelID="Update_Button">
        <ProgressTemplate>
            <div class="modal_Progress">
                <div class="center_Progress">
                    <img alt="" src="../Images/ajax-loader2.gif" style="vertical-align: middle; background-color: White; border-radius: 10px;" />
                </div>
            </div>
        </ProgressTemplate>
    </asp:UpdateProgress>

    <asp:UpdateProgress ID="UpdateProgress1" runat="server" AssociatedUpdatePanelID="Update_Tab_Name">
        <ProgressTemplate>
            <div class="modal_Progress">
                <div class="center_Progress">
                    <img alt="" src="../Images/ajax-loader2.gif" style="vertical-align: middle; background-color: White; border-radius: 10px;" />
                </div>
            </div>
        </ProgressTemplate>
    </asp:UpdateProgress>

    <asp:UpdateProgress ID="UpdateProgress3" runat="server" AssociatedUpdatePanelID="Update_List">
        <ProgressTemplate>
            <div class="modal_Progress">
                <div class="center_Progress">
                    <img alt="" src="../Images/ajax-loader2.gif" style="vertical-align: middle; background-color: White; border-radius: 10px;" />
                </div>
            </div>
        </ProgressTemplate>
    </asp:UpdateProgress>

    <asp:UpdateProgress ID="UpdateProgress4" runat="server" AssociatedUpdatePanelID="Update_New">
        <ProgressTemplate>
            <div class="modal_Progress">
                <div class="center_Progress">
                    <img alt="" src="../Images/ajax-loader2.gif" style="vertical-align: middle; background-color: White; border-radius: 10px;" />
                </div>
            </div>
        </ProgressTemplate>
    </asp:UpdateProgress>

    <asp:UpdateProgress ID="UpdateProgress5" runat="server" AssociatedUpdatePanelID="Update_Terminated">
        <ProgressTemplate>
            <div class="modal_Progress">
                <div class="center_Progress">
                    <img alt="" src="../Images/ajax-loader2.gif" style="vertical-align: middle; background-color: White; border-radius: 10px;" />
                </div>
            </div>
        </ProgressTemplate>
    </asp:UpdateProgress>

    <asp:UpdateProgress ID="UpdateProgress6" runat="server" AssociatedUpdatePanelID="Update_Leave">
        <ProgressTemplate>
            <div class="modal_Progress">
                <div class="center_Progress">
                    <img alt="" src="../Images/ajax-loader2.gif" style="vertical-align: middle; background-color: White; border-radius: 10px;" />
                </div>
            </div>
        </ProgressTemplate>
    </asp:UpdateProgress>

    <asp:UpdateProgress ID="UpdateProgress7" runat="server" AssociatedUpdatePanelID="Update_Alert">
        <ProgressTemplate>
            <div class="modal_Progress">
                <div class="center_Progress">
                    <img alt="" src="../Images/ajax-loader2.gif" style="vertical-align: middle; background-color: White; border-radius: 10px;" />
                </div>
            </div>
        </ProgressTemplate>
    </asp:UpdateProgress>

    <asp:UpdateProgress ID="UpdateProgress8" runat="server" AssociatedUpdatePanelID="Update_Salary">
        <ProgressTemplate>
            <div class="modal_Progress">
                <div class="center_Progress">
                    <img alt="" src="../Images/ajax-loader2.gif" style="vertical-align: middle; background-color: White; border-radius: 10px;" />
                </div>
            </div>
        </ProgressTemplate>
    </asp:UpdateProgress>

    <asp:UpdateProgress ID="UpdateProgress9" runat="server" AssociatedUpdatePanelID="Update_OT_PL">
        <ProgressTemplate>
            <div class="modal_Progress">
                <div class="center_Progress">
                    <img alt="" src="../Images/ajax-loader2.gif" style="vertical-align: middle; background-color: White; border-radius: 10px;" />
                </div>
            </div>
        </ProgressTemplate>
    </asp:UpdateProgress>

    <asp:UpdateProgress ID="UpdateProgress10" runat="server" AssociatedUpdatePanelID="Update_Upload">
        <ProgressTemplate>
            <div class="modal_Progress">
                <div class="center_Progress">
                    <img alt="" src="../Images/ajax-loader2.gif" style="vertical-align: middle; background-color: White; border-radius: 10px;" />
                </div>
            </div>
        </ProgressTemplate>
    </asp:UpdateProgress>

    <asp:UpdateProgress ID="UpdateProgress11" runat="server" AssociatedUpdatePanelID="Update_Penalty">
        <ProgressTemplate>
            <div class="modal_Progress">
                <div class="center_Progress">
                    <img alt="" src="../Images/ajax-loader2.gif" style="vertical-align: middle; background-color: White; border-radius: 10px;" />
                </div>
            </div>
        </ProgressTemplate>
    </asp:UpdateProgress>

    <asp:UpdateProgress ID="UpdateProgress12" runat="server" AssociatedUpdatePanelID="Update_Help">
        <ProgressTemplate>
            <div class="modal_Progress">
                <div class="center_Progress">
                    <img alt="" src="../Images/ajax-loader2.gif" style="vertical-align: middle; background-color: White; border-radius: 10px;" />
                </div>
            </div>
        </ProgressTemplate>
    </asp:UpdateProgress>

    <asp:UpdateProgress ID="UpdateProgress13" runat="server" AssociatedUpdatePanelID="Update_Modal_Address">
        <ProgressTemplate>
            <div class="modal_Progress">
                <div class="center_Progress">
                    <img alt="" src="../Images/ajax-loader2.gif" style="vertical-align: middle; background-color: White; border-radius: 10px;" />
                </div>
            </div>
        </ProgressTemplate>
    </asp:UpdateProgress>

    <asp:UpdateProgress ID="UpdateProgress14" runat="server" AssociatedUpdatePanelID="Update_Modal_Leave">
        <ProgressTemplate>
            <div class="modal_Progress">
                <div class="center_Progress">
                    <img alt="" src="../Images/ajax-loader2.gif" style="vertical-align: middle; background-color: White; border-radius: 10px;" />
                </div>
            </div>
        </ProgressTemplate>
    </asp:UpdateProgress>

    <asp:UpdateProgress ID="UpdateProgress15" runat="server" AssociatedUpdatePanelID="Update_Modal_Leave_View">
        <ProgressTemplate>
            <div class="modal_Progress">
                <div class="center_Progress">
                    <img alt="" src="../Images/ajax-loader2.gif" style="vertical-align: middle; background-color: White; border-radius: 10px;" />
                </div>
            </div>
        </ProgressTemplate>
    </asp:UpdateProgress>

    <asp:UpdateProgress ID="UpdateProgress16" runat="server" AssociatedUpdatePanelID="Update_Modal_Alert">
        <ProgressTemplate>
            <div class="modal_Progress">
                <div class="center_Progress">
                    <img alt="" src="../Images/ajax-loader2.gif" style="vertical-align: middle; background-color: White; border-radius: 10px;" />
                </div>
            </div>
        </ProgressTemplate>
    </asp:UpdateProgress>

    <asp:UpdateProgress ID="UpdateProgress25" runat="server" AssociatedUpdatePanelID="Update_Modal_Alert_Button">
        <ProgressTemplate>
            <div class="modal_Progress">
                <div class="center_Progress">
                    <img alt="" src="../Images/ajax-loader2.gif" style="vertical-align: middle; background-color: White; border-radius: 10px;" />
                </div>
            </div>
        </ProgressTemplate>
    </asp:UpdateProgress>

    <asp:UpdateProgress ID="UpdateProgress17" runat="server" AssociatedUpdatePanelID="Update_Modal_Salary">
        <ProgressTemplate>
            <div class="modal_Progress">
                <div class="center_Progress">
                    <img alt="" src="../Images/ajax-loader2.gif" style="vertical-align: middle; background-color: White; border-radius: 10px;" />
                </div>
            </div>
        </ProgressTemplate>
    </asp:UpdateProgress>

    <asp:UpdateProgress ID="UpdateProgress18" runat="server" AssociatedUpdatePanelID="Update_Modal_OT_PL">
        <ProgressTemplate>
            <div class="modal_Progress">
                <div class="center_Progress">
                    <img alt="" src="../Images/ajax-loader2.gif" style="vertical-align: middle; background-color: White; border-radius: 10px;" />
                </div>
            </div>
        </ProgressTemplate>
    </asp:UpdateProgress>

    <asp:UpdateProgress ID="UpdateProgress19" runat="server" AssociatedUpdatePanelID="Update_Modal_Penalty">
        <ProgressTemplate>
            <div class="modal_Progress">
                <div class="center_Progress">
                    <img alt="" src="../Images/ajax-loader2.gif" style="vertical-align: middle; background-color: White; border-radius: 10px;" />
                </div>
            </div>
        </ProgressTemplate>
    </asp:UpdateProgress>

    <asp:Panel ID="PnlEmpSalary" runat="server" Visible="false"></asp:Panel>
    <%--not use --%>

    <div class="modal fade" id="ControlSettingModal" tabindex="-1" role="dialog" aria-labelledby="ControlSetting_ModalLabel" aria-hidden="true">
        <div class="modal-dialog modal-md">
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal">
                        <span aria-hidden="true">&times;</span><span class="sr-only">Close</span></button>
                    <h4 class="modal-title">
                        <asp:Label ID="lblUcSettingsTitle" runat="server" Text="Set Columns Visibility" />
                    </h4>
                </div>
                <div class="modal-body">
                    <UC:ucCtlSetting ID="ucCtlSetting" runat="server" />
                </div>
            </div>
        </div>
    </div>

    <table style="display: none;" cellpadding="0" cellspacing="0">
        <tr>
            <td>
                <asp:Panel ID="pnlMenuList" runat="server" CssClass="a">
                    <asp:Button ID="btnList" runat="server" Text="<%$ Resources:Attendance,List %>"
                        BorderStyle="none" BackColor="Transparent" OnClick="btnList_Click" Style="padding-left: 40px; padding-top: 3px; background-image: url('../Images/List.png' ); background-repeat: no-repeat; height: 49px; background-position: 5px; font: bold 14px Trebuchet MS; color: #000000;" />
                </asp:Panel>
            </td>
            <td>
                <asp:Panel ID="pnlMenuNew" runat="server" CssClass="a">
                    <asp:Button ID="btnNew" runat="server" Text="<%$ Resources:Attendance,New %>"
                        BorderStyle="none" BackColor="Transparent" OnClick="btnNew_Click" Style="padding-left: 40px; padding-top: 3px; background-image: url('../Images/New.png' ); background-repeat: no-repeat; height: 49px; background-position: 5px; font: bold 14px Trebuchet MS; color: #000000;" />
                </asp:Panel>
            </td>
            <td>
                <asp:Panel ID="pnlMenuAssign" runat="server" CssClass="a">
                    <asp:Button ID="btnAssign" runat="server" Text="Assign"
                        BorderStyle="none" BackColor="Transparent" OnClick="btnAssign_Click" Style="padding-left: 40px; padding-top: 3px; background-image: url('../Images/New.png' ); background-repeat: no-repeat; height: 49px; background-position: 5px; font: bold 14px Trebuchet MS; color: #000000;" />
                </asp:Panel>
            </td>
            <td>
                <asp:Panel ID="pnlMenuBin" runat="server" CssClass="a">
                    <asp:Button ID="btnBin" runat="server" Text="<%$ Resources:Attendance,Terminated %>"
                        BorderStyle="none" BackColor="Transparent" OnClick="btnBin_Click" Style="padding-left: 30px; padding-top: 3px; background-image: url('../Images/terminate.png' ); background-repeat: no-repeat; height: 49px; background-position: 5px; font: bold 14px Trebuchet MS; color: #000000;" />
                </asp:Panel>
            </td>
            <td>
                <asp:Panel ID="pnlLeave" runat="server" CssClass="a">
                    <asp:Button ID="btnLeave" runat="server" Text="<%$ Resources:Attendance,Leave %>"
                        BorderStyle="none" BackColor="Transparent" OnClick="btnLeave_Click"
                        Style="padding-left: 40px; padding-top: 3px; background-image: url('../Images/leave.png' ); background-repeat: no-repeat; height: 49px; background-position: 5px; font: bold 14px Trebuchet MS; color: #000000;" />
                </asp:Panel>
            </td>
            <td>
                <asp:Panel ID="pnlNotice" runat="server" CssClass="a">
                    <asp:Button ID="btnNot" runat="server" Text="<%$ Resources:Attendance,Alert %>"
                        BorderStyle="none" BackColor="Transparent" OnClick="btnNotice_Click" Style="padding-left: 40px; padding-top: 3px; background-image: url('../Images/alert.png' ); background-repeat: no-repeat; height: 49px; background-position: 5px; font: bold 14px Trebuchet MS; color: #000000;" />
                </asp:Panel>
            </td>
            <td>
                <asp:Panel ID="pnlSalary" runat="server" CssClass="a">
                    <asp:Button ID="btnSal3" runat="server" Text="<%$ Resources:Attendance,Salary %>"
                        BorderStyle="none" BackColor="Transparent" OnClick="btnSalary_Click"
                        Style="padding-left: 40px; padding-top: 3px; background-image: url('../Images/salary.png' ); background-repeat: no-repeat; height: 49px; background-position: 5px; font: bold 14px Trebuchet MS; color: #000000;" />
                </asp:Panel>
            </td>
            <td>
                <asp:Panel ID="pnlOTPartial" runat="server" CssClass="a">
                    <asp:Button ID="btnOt7" runat="server" Text="<%$ Resources:Attendance,OT/PL %>"
                        BorderStyle="none" BackColor="Transparent" OnClick="btnOTPartial_Click" Style="padding-left: 40px; padding-top: 3px; background-image: url('../Images/ot_pl.png' ); background-repeat: no-repeat; height: 49px; background-position: 5px; font: bold 14px Trebuchet MS; color: #000000;" />
                </asp:Panel>
            </td>
            <td>
                <asp:Panel ID="pnlUpload" runat="server" CssClass="a">
                    <asp:Button ID="btnUpload3" runat="server" Text="<%$ Resources:Attendance,Upload %>"
                        BorderStyle="none" BackColor="Transparent" OnClick="btnUpload_Click"
                        Style="padding-left: 40px; padding-top: 3px; background-image: url('../Images/upload.png' ); background-repeat: no-repeat; height: 49px; background-position: 5px; font: bold 14px Trebuchet MS; color: #000000;" />
                </asp:Panel>
            </td>
            <td>
                <asp:Panel ID="pnlPanelty" runat="server" CssClass="a">
                    <asp:Button ID="btnPen" runat="server" Text="<%$ Resources:Attendance,Penalty  %>"
                        BorderStyle="none" BackColor="Transparent" OnClick="btnPanelty_Click"
                        Style="padding-left: 40px; padding-top: 3px; background-image: url('../Images/penalty.png' ); background-repeat: no-repeat; height: 49px; background-position: 5px; font: bold 14px Trebuchet MS; color: #000000;" />
                </asp:Panel>
            </td>
            <td>
                <asp:Panel ID="PnlHelp" runat="server" CssClass="a">
                    <asp:Button ID="btnHelp" runat="server" Text="<%$ Resources:Attendance,Help %>"
                        BorderStyle="none" BackColor="Transparent" OnClick="btnHelp_Click" Style="padding-left: 30px; padding-top: 3px; background-image: url(  '../Images/help.PNG' ); background-repeat: no-repeat; height: 49px; background-position: 5px; font: bold 14px Trebuchet MS; color: #000000;" />
                </asp:Panel>
            </td>
        </tr>
    </table>

</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="FooterContent" runat="Server">
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="AfterLoadScriptContent" runat="Server">
    <script src="../Script/common.js"></script>
    <script type="text/javascript">
        function Validate(sender, args) {
            var gridView = document.getElementById("<%=gvBinEmp.ClientID %>");
            var checkBoxes = gridView.getElementsByTagName("input");
            for (var i = 0; i < checkBoxes.length; i++) {
                if (checkBoxes[i].type == "checkbox" && checkBoxes[i].checked) {
                    args.IsValid = true;
                    return;
                }
            }
            args.IsValid = false;
        }



        function Leave_Validate(sender, args) {
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
                var gridView = document.getElementById("<%=gvEmpLeave.ClientID %>");
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

        function Alert_Validate(sender, args) {
            if (document.getElementById("<%=rbtnGroupNF.ClientID %>").checked) {
                var groupbox = document.getElementById("<%=lbxGroupNF.ClientID %>");
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
                var gridView = document.getElementById("<%=gvEmpNF.ClientID %>");
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

        function Salary_Validate(sender, args) {
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
        }

        function OTPL_Validate(sender, args) {
            if (document.getElementById("<%=rbtnGroupOT.ClientID %>").checked) {
                var groupbox = document.getElementById("<%=lbxGroupOT.ClientID %>");
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
                var gridView = document.getElementById("<%=gvEmpOT.ClientID %>");
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

        function Penalty_Validate(sender, args) {
            if (document.getElementById("<%=rbtnPenaltyGroup.ClientID %>").checked) {
                var groupbox = document.getElementById("<%=lbxGroupPenalty.ClientID %>");
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
                var gridView = document.getElementById("<%=gvEmpPenalty.ClientID %>");
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



        function DeviceOperation_Validate(sender, args) {


            var gridView = document.getElementById("<%=gvDeviceOpHistory.ClientID %>");
            var checkBoxes = gridView.getElementsByTagName("input");
            for (var i = 0; i < checkBoxes.length; i++) {
                if (checkBoxes[i].type == "checkbox" && checkBoxes[i].checked) {
                    args.IsValid = true;
                    return;
                }
            }
            args.IsValid = false;

        }





        function isNumber(evt) {
            evt = (evt) ? evt : window.event;
            var charCode = (evt.which) ? evt.which : evt.keyCode;
            if (charCode > 31 && (charCode < 48 || charCode > 57)) {
                return false;
            }
            return true;
        }


        function ConfirmMessage() {
            var selectedvalue = confirm("Do you want to save data?");
            if (selectedvalue) {
                document.getElementById('<%=txtconformmessageValue.ClientID %>').value = "Yes";


            } else {
                document.getElementById('<%=txtconformmessageValue.ClientID %>').value = "No";
            }
        }

        function Li_Tab_List() {
            document.getElementById('<%= Btn_List.ClientID %>').click();
        }
        function Li_Tab_New() {
            document.getElementById('<%= Btn_New.ClientID %>').click();
        }
        function Li_Tab_Assign() {
            document.getElementById('<%= Btn_Assign.ClientID %>').click();
        }
        function Li_Tab_Terminated() {
            document.getElementById('<%= Btn_Terminated.ClientID %>').click();
        }
        function Li_Tab_Leave() {
            document.getElementById('<%= Btn_Leave.ClientID %>').click();
        }
        function Li_Tab_Alert() {
            document.getElementById('<%= Btn_Alert.ClientID %>').click();
        }

        function Li_Tab_Salary() {
            document.getElementById('<%= Btn_Salary.ClientID %>').click();
        }
        function Li_Tab_OT_PL() {
            document.getElementById('<%= Btn_Ot_PL.ClientID %>').click();
        }
        function Li_Tab_Upload() {
            document.getElementById('<%= Btn_Upload.ClientID %>').click();
        }
        function Li_Tab_Penalty() {
            document.getElementById('<%= Btn_Penalty.ClientID %>').click();
        }
        function Li_Tab_Help() {
            document.getElementById('<%= Btn_Help_New.ClientID %>').click();
        }
        function Modal_Address_Open() {
            document.getElementById('<%= Btn_Address_Modal.ClientID %>').click();
        }
        function Modal_Address_Close() {
            document.getElementById('<%= Btn_Address_Modal.ClientID %>').click();
            document.getElementById('<%= imgAddAddressName.ClientID %>').click();
        }

        function Show_Modal_Leave() {
            document.getElementById('<%= Btn_Leave_Modal.ClientID %>').click();
        }

        function Show_Modal_Leave_View() {
            document.getElementById('<%= Btn_Leave_View_Modal.ClientID %>').click();
        }

        function Show_Modal_Alert() {
            document.getElementById('<%= Btn_Alert_Modal.ClientID %>').click();
        }

        function Show_Modal_Salary() {
            document.getElementById('<%= Btn_Salary_Modal.ClientID %>').click();
        }


        function Show_Modal_Hierarchy() {
            document.getElementById('<%= Btn_Hierarchy_Modal.ClientID %>').click();
        }

        function Show_Modal_OT_PL() {
            document.getElementById('<%= Btn_OT_PL_Modal.ClientID %>').click();
        }

        function Show_Modal_Penalty() {
            document.getElementById('<%= Btn_Penalty_Modal.ClientID %>').click();
        }


        function LI_New_Active() {
            $("#Li_List").removeClass("active");
            $("#List").removeClass("active");

            $("#Li_New").addClass("active");
            $("#New").addClass("active");
        }

        function LI_List_New_Active() {
            $("#Li_New").removeClass("active");
            $("#New").removeClass("active");

            $("#Li_List").addClass("active");
            $("#List").addClass("active");
        }

        function LI_List_Active() {
            $("#Li_List").addClass("active");
            $("#List").addClass("active");

            $("#Li_Leave").removeClass("active");
            $("#Leave").removeClass("active");
        }


        function LI_Upload_Active() {

            $("#Li_List").removeClass("active");
            $("#List").removeClass("active");

            $("#Li_New").removeClass("active");
            $("#New").removeClass("active");

            $("#Li_Terminated").removeClass("active");
            $("#Terminated").removeClass("active");


            $("#Li_Upload").addClass("active");
            $("#Upload").addClass("active");


        }


        function LI_Assign() {
            $("#Li_Assign").removeClass("active");
            $("#Assign").removeClass("active");

            $("#Li_List").addClass("active");
            $("#List").addClass("active");
        }

        function Modal_UpdateLicense_Open() {
            $('#ModelUpdateLicense').modal('show')
        }

        function resetPositionCalendar(object, args) {
            var tb = object._popupDiv;
            var tbposition = findPositionWithScrolling(tb);
            var xposition = tbposition[0] + 2;
            var yposition = tbposition[1] + 25;
            var ex = tb;
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

    </script>
    <script type="text/javascript" language="javascript">

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
        }

        function Div_General_Info_Open() {
            $("#Btn_Div_General_Info").removeClass("fa fa-plus");
            $("#Div_General_Info").removeClass("box box-primary collapsed-box");

            $("#Btn_Div_General_Info").addClass("fa fa-minus");
            $("#Div_General_Info").addClass("box box-primary");
        }

        function Div_Additional_Info_Open() {
            $("#Btn_Div_Additional_Info").removeClass("fa fa-plus");
            $("#Div_Additional_Info").removeClass("box box-primary collapsed-box");

            $("#Btn_Div_Additional_Info").addClass("fa fa-minus");
            $("#Div_Additional_Info").addClass("box box-primary");
        }

        function Div_Salary_Increment_Open() {
            $("#Btn_AddSalary_Div").removeClass("fa fa-plus");
            $("#Div_Box_AddSalary").removeClass("box box-primary collapsed-box");

            $("#Btn_AddSalary_Div").addClass("fa fa-minus");
            $("#Div_Box_AddSalary").addClass("box box-primary");
        }

        function Div_Indemnity_Gratuity_Open() {
            $("#Btn_AddIndemnity_Div").removeClass("fa fa-plus");
            $("#Div_Box_AddIndemnity").removeClass("box box-primary collapsed-box");

            $("#Btn_AddIndemnity_Div").addClass("fa fa-minus");
            $("#Div_Box_AddIndemnity").addClass("box box-primary");
        }

        function Div_Salary_Increment_Open_Popup() {
            //if ($("#Div_Box_Add1").hasClass("box box-info collapsed-box")) {
            //    alert("True");
            //}

            $("#Btn_Salary_Div").removeClass("fa fa-plus");
            $("#Div_Box_Salary").removeClass("box box-primary collapsed-box");

            $("#Btn_Salary_Div").addClass("fa fa-minus");
            $("#Div_Box_Salary").addClass("box box-primary");
        }

        function Div_Indemnity_Gratuity_Open_Popup() {
            $("#Btn_Indemnity_Div").removeClass("fa fa-plus");
            $("#Div_Box_Indemnity").removeClass("box box-primary collapsed-box");

            $("#Btn_Indemnity_Div").addClass("fa fa-minus");
            $("#Div_Box_Indemnity").addClass("box box-primary");
        }
    </script>
    <script type="text/javascript">
        function FuLogo_UploadComplete(sender, args) {
            document.getElementById('<%= FULogo_Img_Wrong.ClientID %>').style.display = "none";
            document.getElementById('<%= FULogo_Img_Right.ClientID %>').style.display = "";

            <%--var img = document.getElementById('<%= imgLogo.ClientID %>');
            img.src = "<%=ResolveUrl(FuLogo_UploadFolderPath) %>" + args.get_fileName();--%>
        }
        function FuLogo_UploadError(sender, args) {
            document.getElementById('<%= FULogo_Img_Right.ClientID %>').style.display = "none";
            document.getElementById('<%= FULogo_Img_Wrong.ClientID %>').style.display = "";
            var img = document.getElementById('<%= imgLogo.ClientID %>');
            img.src = "../Bootstrap_Files/dist/img/NoImage.jpg";
            alert('Invalid File Type, Select Only .png, .jpg, .jpge extension file');
        }
        function FuLogo_UploadStarted(sender, args) {
            var filename = args.get_fileName();

            var filext = filename.substring(filename.lastIndexOf(".") + 1);
            filext = filext.toLowerCase();
            if (filext == "png" || filext == "jpg" || filext == "jpge") {
                return true;
            }
            else {
                throw {
                    name: "Invalid File Type",
                    level: "Error",
                    message: "Invalid File Type, Select Only .png, .jpg, .jpge extension file",
                    htmlMessage: "Invalid File Type, Select Only .png, .jpg, .jpge extension file"
                }
                return false;
            }
        }

        function selectAllCheckbox_Click(id) {

            var gridView = document.getElementById('<%=gvDeviceOpHistory.ClientID%>');
            for (var i = 1; i < gridView.rows.length; i++) {
                var cell = gridView.rows[i].cells[0];
                cell.getElementsByTagName("INPUT")[0].checked = id.checked;
            }
            //SetEmpList();
            //SelectedEmpCount();
        }

        function selectCheckBox_Click(id) {

            var gridView = document.getElementById('<%=gvDeviceOpHistory.ClientID%>');
            var AtLeastOneCheck = false;
            var cell;
            for (var i = 1; i < gridView.rows.length; i++) {
                cell = gridView.rows[i].cells[0];
                if (cell.getElementsByTagName("INPUT")[0].checked == false) {
                    AtLeastOneCheck = true;
                    break;
                }
            }
            gridView.rows[0].cells[0].getElementsByTagName("INPUT")[0].checked = !AtLeastOneCheck;

            //SelectedEmpCount();
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









    <script type="text/javascript">
        function EmployeeSign_UploadComplete(sender, args) {
            document.getElementById('<%= EmployeeSign_Img_Wrong.ClientID %>').style.display = "none";
            document.getElementById('<%= EmployeeSign_Img_Right.ClientID %>').style.display = "";

            <%--var img = document.getElementById('<%= imgLogo.ClientID %>');
            img.src = "<%=ResolveUrl(EmployeeSign_UploadFolderPath) %>" + args.get_fileName();--%>
        }
        function EmployeeSign_UploadError(sender, args) {
            document.getElementById('<%= EmployeeSign_Img_Right.ClientID %>').style.display = "none";
            document.getElementById('<%= EmployeeSign_Img_Wrong.ClientID %>').style.display = "";
            var img = document.getElementById('<%= ImgEmpSignature.ClientID %>');
            img.src = "../Bootstrap_Files/dist/img/NoImage.jpg";
            alert('Invalid File Type, Select Only .png, .jpg, .jpge extension file');
        }
        function EmployeeSign_UploadStarted(sender, args) {
            var filename = args.get_fileName();

            var filext = filename.substring(filename.lastIndexOf(".") + 1);
            filext = filext.toLowerCase();
            if (filext == "png" || filext == "jpg" || filext == "jpge") {
                return true;
            }
            else {
                throw {
                    name: "Invalid File Type",
                    level: "Error",
                    message: "Invalid File Type, Select Only .png, .jpg, .jpge extension file",
                    htmlMessage: "Invalid File Type, Select Only .png, .jpg, .jpge extension file"
                }
                return false;
            }
        }

        <%--function FUAll_UploadComplete(sender, args) {
            document.getElementById('<%= FUAll_Img_Wrong.ClientID %>').style.display = "none";
            document.getElementById('<%= FUAll_Img_Right.ClientID %>').style.display = "";
        }
        function FUAll_UploadError(sender, args) {
            document.getElementById('<%= FUAll_Img_Right.ClientID %>').style.display = "none";
            document.getElementById('<%= FUAll_Img_Wrong.ClientID %>').style.display = "";
        }--%>
        function FUAll_UploadStarted(sender, args) {

        }
        function Modal_Open_FileUpload() {
            document.getElementById('<%= Btn_Modal_FileUpload.ClientID %>').click();
        }
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
        function Modal_Number_Open(data) {
            document.getElementById('AllNumberData').innerHTML = data;
            document.getElementById('<%= Btn_Number.ClientID %>').click();
        }
        function validation1() {

            var valIsCheck = document.getElementById('<%= addaddress.FindControl("ContactNo").FindControl("ddlContactType").ClientID %>').value;

            if (valIsCheck == "LandLine") {
                document.getElementById('<%=addaddress.FindControl("ContactNo").FindControl("txtExtensionNumber").ClientID%>').disabled = false;
            }
            else {
                document.getElementById('<%=addaddress.FindControl("ContactNo").FindControl("txtExtensionNumber").ClientID%>').disabled = true;
                document.getElementById('<%=addaddress.FindControl("ContactNo").FindControl("txtExtensionNumber").ClientID%>').value = "";
            }
        }
        function showUcControlsSettings() {
            $('#ControlSettingModal').modal('show');
        }

    </script>


    <script src="../Script/common.js"></script>
</asp:Content>
