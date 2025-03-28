<%@ Page Title="" Language="C#" MasterPageFile="~/ERPMaster.master" AutoEventWireup="true" CodeFile="HolidayMaster.aspx.cs" Inherits="MasterSetUp_HolidayMaster" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Src="~/WebUserControl/FileUpload.ascx" TagPrefix="AT1" TagName="FileUpload1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="Head" runat="Server">
    <link href="../Bootstrap_Files/Additional/Popup_Style.css" rel="stylesheet" type="text/css" />
    <link href="../Bootstrap_Files/Additional/Button_Style.css" rel="stylesheet" type="text/css" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="HeaderContent" runat="Server">
    <h1>
        <i class="fas fa-umbrella-beach"></i>&nbsp;&nbsp;
        <asp:Label ID="lblHeader" runat="server" Text="<%$ Resources:Attendance,Holiday Setup%>"></asp:Label>
    </h1>
    <ol class="breadcrumb">
        <li><a href="#"><i class="fa fa-dashboard"></i>
            <asp:Label ID="Label6" runat="server" Text="<%$ Resources:Attendance,Master Setup%>"></asp:Label></a></li>
        <li><a href="#"><i class="fa fa-dashboard"></i>
            <asp:Label ID="Label10" runat="server" Text="<%$ Resources:Attendance,Master SetUp%>"></asp:Label></a></li>
        <li class="active">
            <asp:Label ID="Label5" runat="server" Text="<%$ Resources:Attendance,Holiday Setup%>"></asp:Label></li>
    </ol>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="Server">

    <asp:UpdatePanel ID="Update_Button" runat="server">
        <ContentTemplate>
            <asp:Button ID="Btn_Bin" Style="display: none;" runat="server" OnClick="btnBin_Click" Text="Bin" />
            <asp:Button ID="Btn_Address_Modal" Style="display: none;" runat="server" data-toggle="modal" data-target="#myModal" Text="Address" />
            <asp:Button ID="Btn_Modal_FileUpload" Style="display: none;" runat="server" data-toggle="modal" data-target="#Fileupload123" Text="FileUpload" />
            <asp:HiddenField runat="server" ID="hdnCanEdit" />
            <asp:HiddenField runat="server" ID="hdnCanDelete" />
            <asp:HiddenField runat="server" ID="hdnCanUpload" />
        </ContentTemplate>
    </asp:UpdatePanel>
    <div class="row">
        <div class="col-md-12">
            <div class="nav-tabs-custom">
                <ul class="nav nav-tabs pull-right bg-blue-gradient">
                    <li id="Li_Bin"><a onclick="Li_Tab_Bin()" href="#Bin" data-toggle="tab">
                        <i class="fa fa-trash"></i>&nbsp;&nbsp;<asp:Label ID="Label9" runat="server" Text="<%$ Resources:Attendance,Bin%>"></asp:Label></a></li>
                    <li id="Li_New"><a href="#New" data-toggle="tab">
                        <asp:UpdatePanel ID="Update_Tab_Name" runat="server">
                            <ContentTemplate>
                                <i class="fa fa-file"></i>&nbsp;&nbsp;
                                <asp:Label ID="Lbl_Tab_New" runat="server" Text="<%$ Resources:Attendance,New%>"></asp:Label>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </a></li>
                    <li id="Li_List" class="active"><a href="#List" data-toggle="tab">
                        <i class="fa fa-list"></i>&nbsp;&nbsp;<asp:Label ID="Label165" runat="server" Text="<%$ Resources:Attendance,List %>"></asp:Label></a></li>
                </ul>
                <div class="tab-content">

                    <div class="tab-pane active" id="List">
                        <asp:UpdatePanel ID="Update_List" runat="server">
                            <ContentTemplate>
                                <div class="row">
                                    <div class="col-md-12">

                                        <div class="row">
                                            <div class="col-md-12">
                                                <div id="Div1" runat="server" class="box box-info collapsed-box">
                                                    <div class="box-header with-border">
                                                        <h3 class="box-title">
                                                            <asp:Label ID="Label22" runat="server" Text="<%$ Resources:Attendance,Advance Search%>"></asp:Label></h3>
                                                        &nbsp;&nbsp;|&nbsp;&nbsp;
					<asp:Label ID="lblTotalRecords" Text="<%$ Resources:Attendance,Total Records: 0 %>" runat="server"></asp:Label>
                                                        <asp:Label ID="lblSelectedRecord" runat="server" Visible="false"></asp:Label>

                                                        <div class="box-tools pull-right">
                                                            <button type="button" class="btn btn-box-tool" data-widget="collapse">
                                                                <i id="I1" runat="server" class="fa fa-plus"></i>
                                                            </button>
                                                        </div>
                                                    </div>
                                                    <div class="box-body">
                                                        <div class="col-lg-12">
                                                            
                                        <asp:Label ID="Label4" runat="server" Text="<%$ Resources:Attendance,Location %>"></asp:Label>
                                        <asp:DropDownList ID="ddlLocation" runat="server" CssClass="form-control" AutoPostBack="true"
                                            OnSelectedIndexChanged="ddlLocation_SelectedIndexChanged">
                                        </asp:DropDownList>
                                    <br />
                                                        </div>
                                                        <div class="col-lg-3">
                                                            <asp:DropDownList ID="ddlFieldName" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlFieldName_SelectedIndexChanged" CssClass="form-control">
                                                                <asp:ListItem Selected="True" Text="<%$ Resources:Attendance,Holiday Name %>" Value="Holiday_Name"></asp:ListItem>
                                                                <asp:ListItem Text="<%$ Resources:Attendance,Holiday Name(Local) %>" Value="Holiday_Name_L"></asp:ListItem>
                                                                <asp:ListItem Text="<%$ Resources:Attendance,From Date %>" Value="From_Date"></asp:ListItem>
                                                                <asp:ListItem Text="<%$ Resources:Attendance,To Date %>" Value="To_Date"></asp:ListItem>
                                                                <asp:ListItem Text="<%$ Resources:Attendance,Holiday Id %>" Value="Holiday_Id"></asp:ListItem>
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
                                                                <asp:TextBox ID="txtValue" runat="server" CssClass="form-control" placeholder="Search from Content"></asp:TextBox>
                                                                <asp:TextBox ID="TxtValueDate" runat="server" Visible="false" class="form-control" placeholder="Search from Date"></asp:TextBox>
                                                                <cc1:CalendarExtender OnClientShown="showCalendar" Format="dd-MMM-yyyy" ID="CalendarExtender3" runat="server" Enabled="True" TargetControlID="TxtValueDate">
                                                                </cc1:CalendarExtender>
                                                            </asp:Panel>
                                                        </div>
                                                        <div class="col-lg-2" style="text-align: center;">
                                                            <asp:LinkButton ID="btnbind" runat="server" CausesValidation="False" OnClick="btnbind_Click" ToolTip="<%$ Resources:Attendance,Search %>"><span class="fa fa-search"  style="font-size:25px;"></span></asp:LinkButton>&nbsp;&nbsp;&nbsp;
                                                            <asp:LinkButton ID="ImageButton1" runat="server" CausesValidation="False" OnClick="btnRefresh_Click" ToolTip="<%$ Resources:Attendance,Refresh %>"><span class="fa fa-repeat"  style="font-size:25px;"></span></asp:LinkButton>
                                                        </div>
                                                        <asp:HiddenField ID="HDFSort" runat="server" />
                                                        <asp:HiddenField ID="hdntxtaddressid" runat="server" />
                                                    </div>
                                                </div>
                                            </div>
                                        </div>

                                        <div class="box box-warning box-solid" <%= gvHolidayMaster.Rows.Count>0?"style='display:block'":"style='display:none'"%>>
                                            <div class="box-body">
                                                <div class="row">
                                                    <div class="col-md-12">
                                                        <div style="overflow: auto">
                                                            <asp:GridView CssClass="table-striped table-bordered table table-hover" ID="gvHolidayMaster" PageSize="<%# PageControlCommon.GetPageSize() %>"
                                                                runat="server" AutoGenerateColumns="False" Width="100%" AllowPaging="True" AllowSorting="True"
                                                                OnPageIndexChanging="gvHolidayMaster_PageIndexChanging" OnSorting="gvHolidayMaster_OnSorting">
                                                                <Columns>
                                                                    <asp:TemplateField HeaderText="<%$ Resources:Attendance,Action%>">
                                                                        <ItemTemplate>
                                                                            <div class="dropdown" style="position: absolute;">
                                                                                <button class="btn btn-default dropdown-toggle" type="button" data-toggle="dropdown">
                                                                                    <i class="fa fa-ellipsis-h" aria-hidden="true"></i>
                                                                                </button>
                                                                                <ul class="dropdown-menu">
                                                                                    <li <%= hdnCanEdit.Value=="true"?"style='display:block'":"style='display:none'" %>>
                                                                                        <asp:LinkButton ID="btnEdit" runat="server" CommandArgument='<%# Eval("Holiday_Id") %>' CausesValidation="False" OnCommand="btnEdit_Command"><i class="fa fa-pencil"></i><%# Resources.Attendance.Edit%></asp:LinkButton>
                                                                                    </li>
                                                                                    <li <%= hdnCanDelete.Value=="true"?"style='display:block'":"style='display:none'" %>>
                                                                                        <asp:LinkButton ID="IbtnDelete" runat="server" CausesValidation="False" CommandArgument='<%# Eval("Holiday_Id") %>' OnCommand="IbtnDelete_Command"><i class="fa fa-trash"></i><%# Resources.Attendance.Delete%></asp:LinkButton>
                                                                                        <cc1:ConfirmButtonExtender ID="confirm1" runat="server" ConfirmText="<%$ Resources:Attendance,Are you sure you want to delete the record?%>" TargetControlID="IbtnDelete"></cc1:ConfirmButtonExtender>
                                                                                    </li>
                                                                                    <li <%= hdnCanUpload.Value=="true"?"style='display:block'":"style='display:none'" %>>
                                                                                        <asp:LinkButton ID="btnFileUpload" runat="server" CommandArgument='<%# Eval("Holiday_Id") %>' CommandName='<%# Eval("Holiday_Name") %>' OnCommand="btnFileUpload_Command" CausesValidation="False"><i class="fa fa-upload"></i><%# Resources.Attendance.File_Upload%></asp:LinkButton>
                                                                                    </li>
                                                                                </ul>
                                                                            </div>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>

                                                                    <asp:TemplateField HeaderText="<%$ Resources:Attendance,Holiday Id %>" SortExpression="Holiday_Id">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblHolidayId1" runat="server" Text='<%# Eval("Holiday_Id") %>'></asp:Label>
                                                                        </ItemTemplate>
                                                                        <ItemStyle />
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="<%$ Resources:Attendance,Holiday Name %>" SortExpression="Holiday_Name">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lbHolidayName" runat="server" Text='<%# Eval("Holiday_Name") %>'></asp:Label>
                                                                        </ItemTemplate>
                                                                        <ItemStyle />
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="<%$ Resources:Attendance,Holiday Name(Local) %>" SortExpression="Holiday_Name_L">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblHolidayNameL" runat="server" Text='<%# Eval("Holiday_Name_L") %>'></asp:Label>
                                                                        </ItemTemplate>
                                                                        <ItemStyle />
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="<%$ Resources:Attendance,From Date %>" SortExpression="From_Date">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblFromDate" runat="server" Text='<%# GetDate(Eval("From_Date")) %>'></asp:Label>
                                                                        </ItemTemplate>
                                                                        <ItemStyle />
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="<%$ Resources:Attendance,To Date %>" SortExpression="To_Date">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lbltoDate" runat="server" Text='<%# GetDate(Eval("To_Date")) %>'></asp:Label>
                                                                        </ItemTemplate>
                                                                        <ItemStyle />
                                                                    </asp:TemplateField>
                                                                      <asp:TemplateField HeaderText="<%$ Resources:Attendance,Created By %>" SortExpression="Created_User">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblCreated_User" runat="server" Text='<%# Eval("Created_User") %>'></asp:Label>
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
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </div>

                    <div class="tab-pane" id="New">
                        <div runat="server" id="pnlLoc" class="row">
                            <div class="col-md-12">
                                <div class="col-md-12">
                                    <div class="box box-danger">
                                        <div class="box-header with-border">
                                            <asp:UpdatePanel ID="Update_Holiday" runat="server">
                                                <ContentTemplate>

                                                    <div id="pnlHoliday" runat="server">

                                                        <div class="col-md-6">
                                                            <asp:Label ID="lblHolidayName" runat="server" Text="<%$ Resources:Attendance,Holiday Name %>"></asp:Label><a style="color: Red">*</a>
                                                            <asp:RequiredFieldValidator EnableClientScript="true" Style="float: right;" runat="server" ID="RequiredFieldValidator1" ValidationGroup="Holiday_Save"
                                                                Display="Dynamic" SetFocusOnError="true" ControlToValidate="txtHolidayName" ErrorMessage="<%$ Resources:Attendance,Enter Holiday Name%>" />

                                                            <asp:TextBox ID="txtHolidayName" BackColor="#eeeeee" runat="server" CssClass="form-control"
                                                                AutoPostBack="true" OnTextChanged="txtHolidayName_OnTextChanged" />
                                                            <cc1:AutoCompleteExtender ID="AutoCompleteExtender1" runat="server" DelimiterCharacters=""
                                                                Enabled="True" ServiceMethod="GetCompletionListHolidayName" ServicePath="" CompletionInterval="100"
                                                                MinimumPrefixLength="1" CompletionSetCount="1" TargetControlID="txtHolidayName"
                                                                UseContextKey="True" CompletionListCssClass="autoCompleteList" CompletionListItemCssClass="autoCompleteListItem" CompletionListHighlightedItemCssClass="autoCompleteSelectedListItem" OnClientShown="resetPosition">
                                                            </cc1:AutoCompleteExtender>
                                                            <br />
                                                            <asp:Label ID="lblHolidayCode" runat="server" Text="<%$ Resources:Attendance,From Date %>"></asp:Label>
                                                            <a style="color: Red">*</a>
                                                            <asp:RequiredFieldValidator EnableClientScript="true" Style="float: right;" runat="server" ID="RequiredFieldValidator2" ValidationGroup="Holiday_Save"
                                                                Display="Dynamic" SetFocusOnError="true" ControlToValidate="txtFromDate" ErrorMessage="<%$ Resources:Attendance,Enter From Date %>" />
                                                            <asp:TextBox ID="txtFromDate" runat="server" CssClass="form-control" />
                                                            <cc1:CalendarExtender OnClientShown="showCalendar" ID="CalendarExtender1" runat="server" Enabled="True" TargetControlID="txtFromDate">
                                                            </cc1:CalendarExtender>
                                                            <br />
                                                        </div>
                                                        <div class="col-md-6">
                                                            <asp:Label ID="lblHolidayNameL" runat="server" Text="<%$ Resources:Attendance,Holiday Name(Local) %>"></asp:Label>
                                                            <asp:TextBox ID="txtHolidayNameL" runat="server" CssClass="form-control" />
                                                            <br />
                                                            <asp:Label ID="Label3" runat="server" Text="<%$ Resources:Attendance,To Date %>"></asp:Label>
                                                            <a style="color: Red">*</a>
                                                            <asp:RequiredFieldValidator EnableClientScript="true" Style="float: right;" runat="server" ID="RequiredFieldValidator3" ValidationGroup="Holiday_Save"
                                                                Display="Dynamic" SetFocusOnError="true" ControlToValidate="txtToDate" ErrorMessage="<%$ Resources:Attendance,Enter To Date %>" />
                                                            <asp:TextBox ID="txtToDate" runat="server" CssClass="form-control" />
                                                            <cc1:CalendarExtender OnClientShown="showCalendar" ID="CalendarExtender2" runat="server" Enabled="True" TargetControlID="txtToDate">
                                                            </cc1:CalendarExtender>
                                                            <br />
                                                        </div>

                                                        <div class="col-md-12">
                                                            <div style="text-align: center">
                                                                <br />
                                                                <asp:Button ID="btnSave" OnClick="btnSave_Click" CssClass="btn btn-success" runat="server"
                                                                    Text="<%$ Resources:Attendance,Save %>" ValidationGroup="Holiday_Save" Visible="false" />
                                                                <asp:Button ID="btnReset" OnClick="btnReset_Click" CausesValidation="False" Text="<%$ Resources:Attendance,Reset %>"
                                                                    Style="margin-left: 15px;" CssClass="btn btn-primary" runat="server" />
                                                                <asp:Button ID="btnCancel" OnClick="btnCancel_Click" CausesValidation="False" Text="<%$ Resources:Attendance,Cancel %>"
                                                                    Style="margin-left: 15px;" CssClass="btn btn-danger" runat="server" />
                                                                <asp:HiddenField ID="editid" runat="server" />
                                                            </div>
                                                        </div>
                                                    </div>
                                                </ContentTemplate>
                                            </asp:UpdatePanel>
                                            <asp:UpdatePanel ID="Update_Holiday_Group" runat="server">
                                                <ContentTemplate>
                                                    <div id="pnlHolidayGroup" visible="false" runat="server">
                                                        <div class="col-md-12" style="text-align: center">
                                                            <asp:RadioButton ID="rbtnGroup" OnCheckedChanged="EmpGroup_CheckedChanged" Style="margin-left: 20px; margin-right: 20px;" runat="server"
                                                                Text="<%$ Resources:Attendance,Group %>" Font-Bold="true" GroupName="EmpGroup"
                                                                AutoPostBack="true" />
                                                            <asp:RadioButton ID="rbtnEmp" runat="server" AutoPostBack="true" Style="margin-left: 20px; margin-right: 20px;" Text="<%$ Resources:Attendance,Employee %>"
                                                                GroupName="EmpGroup" Font-Bold="true" OnCheckedChanged="EmpGroup_CheckedChanged" />
                                                            <br />
                                                        </div>
                                                    </div>
                                                </ContentTemplate>
                                            </asp:UpdatePanel>
                                            <asp:UpdatePanel ID="Update_Group" runat="server">
                                                <ContentTemplate>
                                                    <div id="pnlGroup" visible="false" runat="server">
                                                        <asp:CustomValidator ID="CustomValidator2" ValidationGroup="Save_Holiday" runat="server" ErrorMessage="Please select at least one record."
                                                            ClientValidationFunction="Validate" ForeColor="Red"></asp:CustomValidator>
                                                        <div class="col-md-12" style="text-align: center;">

                                                            <asp:ListBox ID="lbxGroup" runat="server" Height="211px" Width="171px" SelectionMode="Multiple"
                                                                AutoPostBack="true" OnSelectedIndexChanged="lbxGroup_SelectedIndexChanged" CssClass="list"
                                                                Font-Names="Trebuchet MS" Font-Size="Small" ForeColor="Gray"></asp:ListBox>
                                                            <br />
                                                            <br />
                                                            <div id="Div_Group" runat="server" class="box box-warning box-solid">
                                                                <div class="box-header with-border">
                                                                    <h3 class="box-title"></h3>
                                                                </div>
                                                                <div class="box-body">
                                                                    <div class="row">
                                                                        <div class="col-md-12">

                                                                            <div style="overflow: auto">
                                                                                <asp:GridView CssClass="table-striped table-bordered table table-hover" ID="gvEmp" runat="server" AllowPaging="True" AutoGenerateColumns="False"
                                                                                    OnPageIndexChanging="gvEmp_PageIndexChanging" Width="100%" PageSize="<%# PageControlCommon.GetPageSize() %>">
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
                                                            <div style="text-align: center;">
                                                                <br />
                                                                <asp:Button ID="btnSaveHoliday" runat="server" Text="<%$ Resources:Attendance,Save %>"
                                                                    Visible="true" CssClass="btn btn-success" ValidationGroup="Save_Holiday" OnClick="btnSaveHoliday_Click" />
                                                                <asp:Button ID="btnCancelHoliday" runat="server" Text="<%$ Resources:Attendance,Cancel %>"
                                                                    CssClass="btn btn-danger" CausesValidation="False" OnClick="btnCancelHoliday_Click" />
                                                                <asp:Button ID="btnDelete" runat="server" Text="<%$ Resources:Attendance,Delete %>"
                                                                    CssClass="btn btn-primary" CausesValidation="False" OnClick="btnDeleteHoliday_Click" />
                                                            </div>
                                                        </div>
                                                    </div>
                                                </ContentTemplate>
                                            </asp:UpdatePanel>
                                            <asp:UpdatePanel ID="Update_Emp" runat="server">
                                                <ContentTemplate>
                                                    <div id="pnlEmp" visible="false" runat="server">

                                                        <div class="col-md-12">
                                                            <div class="box box-primary box-solid">
                                                                <div class="box-body">
                                                                    <div class="form-group">
                                                                        <div class="col-md-12">
                                                                            <asp:Label ID="lblGroupByDept" runat="server" Text="<%$ Resources:Attendance,Location %>"></asp:Label>
                                                                            <asp:ListBox ID="listEmpLocation" SelectionMode="Multiple" runat="server" Style="width: 100%; height: 150px;"></asp:ListBox>
                                                                            <br />
                                                                        </div>
                                                                        <div class="col-md-12" style="text-align: center">
                                                                            <asp:LinkButton ID="btnbindLOCEmp" OnClick="btnbindLOCEmp_Click" runat="server" CausesValidation="False" ToolTip="<%$ Resources:Attendance,Search %>"><span class="fa fa-search"  style="font-size:25px;"></span></asp:LinkButton>
                                                                        </div>
                                                                    </div>
                                                                </div>
                                                            </div>
                                                        </div>

                                                        <div class="col-md-12">

                                                            <div class="row">
                                                                <div class="col-md-12">
                                                                    <div id="Div2" runat="server" class="box box-info collapsed-box">
                                                                        <div class="box-header with-border">
                                                                            <h3 class="box-title">
                                                                                <asp:Label ID="Label1" runat="server" Text="<%$ Resources:Attendance,Advance Search%>"></asp:Label></h3>
                                                                            &nbsp;&nbsp;|&nbsp;&nbsp;
					<asp:Label ID="lblTotalRecordEmp" Text="<%$ Resources:Attendance,Total Records: 0 %>" runat="server"></asp:Label>
                                                                            <asp:Label ID="Label7" runat="server" Visible="false"></asp:Label>

                                                                            <div class="box-tools pull-right">
                                                                                <button type="button" class="btn btn-box-tool" data-widget="collapse">
                                                                                    <i id="I2" runat="server" class="fa fa-plus"></i>
                                                                                </button>
                                                                            </div>
                                                                        </div>
                                                                        <div class="box-body">
                                                                            <div class="col-lg-3">
                                                                                <asp:Label ID="lblEmp" runat="server" Visible="false"></asp:Label>
                                                                                <asp:DropDownList ID="ddlFieldName1" runat="server" CssClass="form-control">
                                                                                    <asp:ListItem Text="<%$ Resources:Attendance,Employee Code %>" Value="Emp_Code"></asp:ListItem>
                                                                                    <asp:ListItem Text="<%$ Resources:Attendance,Employee Name %>" Value="Emp_Name" Selected="True"></asp:ListItem>
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
                                                                                <asp:TextBox ID="txtVal1" runat="server" CssClass="form-control"></asp:TextBox>
                                                                            </div>
                                                                            <div class="col-lg-2" style="text-align: center;">
                                                                                <asp:LinkButton ID="imgBtnBindEmp" runat="server" CausesValidation="False" OnClick="btnbindEmp_Click" ToolTip="<%$ Resources:Attendance,Search %>"><span class="fa fa-search"  style="font-size:25px;"></span></asp:LinkButton>&nbsp;&nbsp;&nbsp;
                                                                                <asp:LinkButton ID="ImageButton2" runat="server" CausesValidation="False" OnClick="btnEmpRefresh_Click" ToolTip="<%$ Resources:Attendance,Refresh %>"><span class="fa fa-repeat"  style="font-size:25px;"></span></asp:LinkButton>&nbsp;&nbsp;&nbsp;
                                                                                <asp:LinkButton ID="imgbtnSelectAll1" runat="server" OnClick="ImgbtnSelectAll_Click1" ToolTip="<%$ Resources:Attendance, Select All %>"><span class="fas fa-th"  style="font-size:25px;"></span></asp:LinkButton>
                                                                            </div>

                                                                        </div>
                                                                    </div>
                                                                </div>
                                                            </div>
                                                            <div class="box box-warning box-solid">
                                                                <div class="box-body">
                                                                    <div class="row">
                                                                        <div class="col-md-12">
                                                                            <asp:CustomValidator ID="CustomValidator1" ValidationGroup="Save_Holiday" runat="server" ErrorMessage="Please select at least one record."
                                                                                ClientValidationFunction="Validate" ForeColor="Red"></asp:CustomValidator>

                                                                            <div style="overflow: auto">
                                                                                <asp:GridView CssClass="table-striped table-bordered table table-hover" ID="gvEmployee" runat="server" AllowPaging="True" AutoGenerateColumns="False"
                                                                                    OnPageIndexChanging="gvEmployee_PageIndexChanging" Width="100%"
                                                                                    AllowSorting="True" DataKeyNames="Emp_Id" PageSize="<%# PageControlCommon.GetPageSize() %>"
                                                                                    OnSorting="gvEmployee_Sorting">
                                                                                    <Columns>
                                                                                        <asp:TemplateField>
                                                                                            <ItemTemplate>
                                                                                                <asp:CheckBox ID="chkgvSelect" runat="server" />
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
                                                                                <asp:HiddenField ID="HDFSortEmployee" runat="server" />
                                                                            </div>
                                                                        </div>
                                                                    </div>
                                                                </div>
                                                            </div>
                                                            <div style="text-align: center;">
                                                                <asp:Button ID="Button1" runat="server" Text="<%$ Resources:Attendance,Save %>" Visible="true"
                                                                    CssClass="btn btn-success" ValidationGroup="Save_Holiday" OnClick="btnSaveHoliday_Click" />
                                                                <asp:Button ID="Button2" runat="server" Text="<%$ Resources:Attendance,Cancel %>"
                                                                    CssClass="btn btn-danger" CausesValidation="False" OnClick="btnCancelHoliday_Click" />
                                                                <asp:Button ID="Button3" runat="server" Text="<%$ Resources:Attendance,Delete %>"
                                                                    CssClass="btn btn-primary" CausesValidation="False" OnClick="btnDeleteHoliday_Click" />
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
                    </div>

                    <div class="tab-pane" id="Bin">
                        <asp:UpdatePanel ID="Update_Bin" runat="server">
                            <ContentTemplate>
                                <div class="row">
                                    <div class="col-md-12">



                                        <div class="row">
                                            <div class="col-md-12">
                                                <div id="Div3" runat="server" class="box box-info collapsed-box">
                                                    <div class="box-header with-border">
                                                        <h3 class="box-title">
                                                            <asp:Label ID="Label2" runat="server" Text="Advance Search"></asp:Label></h3>
                                                        &nbsp;&nbsp;|&nbsp;&nbsp;
					  <asp:Label ID="lblbinTotalRecords" Text="<%$ Resources:Attendance,Total Records: 0 %>" runat="server"></asp:Label>

                                                        <div class="box-tools pull-right">
                                                            <button type="button" class="btn btn-box-tool" data-widget="collapse">
                                                                <i id="I3" runat="server" class="fa fa-plus"></i>
                                                            </button>
                                                        </div>
                                                    </div>
                                                    <div class="box-body">

                                                        <div class="col-lg-3">
                                                            <asp:DropDownList ID="ddlbinFieldName" AutoPostBack="true" OnSelectedIndexChanged="ddlbinFieldName_SelectedIndexChanged" runat="server" CssClass="form-control">
                                                                <asp:ListItem Selected="True" Text="<%$ Resources:Attendance,Holiday Name %>" Value="Holiday_Name"></asp:ListItem>
                                                                <asp:ListItem Text="<%$ Resources:Attendance,Holiday Name(Local) %>" Value="Holiday_Name_L"></asp:ListItem>
                                                                <asp:ListItem Text="<%$ Resources:Attendance,From Date %>" Value="From_Date"></asp:ListItem>
                                                                <asp:ListItem Text="<%$ Resources:Attendance,To Date %>" Value="To_Date"></asp:ListItem>
                                                                <asp:ListItem Text="<%$ Resources:Attendance,Holiday Id %>" Value="Holiday_Id"></asp:ListItem>
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
                                                        <div class="col-lg-3">
                                                            <asp:Panel ID="Panel2" runat="server" DefaultButton="btnbinbind">
                                                                <asp:TextBox ID="txtbinValue" class="form-control" runat="server" placeholder="Search from Content"></asp:TextBox>
                                                                <asp:TextBox ID="txtbinValueDate" runat="server" Visible="false" class="form-control" placeholder="Search from Date"></asp:TextBox>
                                                                <cc1:CalendarExtender OnClientShown="showCalendar" Format="dd-MMM-yyyy" ID="CalendarExtender4" runat="server" Enabled="True" TargetControlID="txtbinValueDate">
                                                                </cc1:CalendarExtender>
                                                            </asp:Panel>
                                                        </div>
                                                        <div class="col-lg-2" style="text-align: center;">
                                                            <asp:LinkButton ID="btnbinbind" runat="server" CausesValidation="False" OnClientClick="tab_3_open()" OnClick="btnbinbind_Click" ToolTip="<%$ Resources:Attendance,Search %>"><span class="fa fa-search"  style="font-size:25px;"></span></asp:LinkButton>&nbsp;&nbsp;&nbsp;
                                                            <asp:LinkButton ID="btnbinRefresh" runat="server" Style="width: 30px;" CausesValidation="False" OnClientClick="tab_3_open()" OnClick="btnbinRefresh_Click" ToolTip="<%$ Resources:Attendance,Refresh %>"><span class="fa fa-repeat"  style="font-size:25px;"></span></asp:LinkButton>&nbsp;&nbsp;&nbsp;
                                                            <asp:LinkButton ID="imgBtnRestore" Style="width: 30px;" CausesValidation="False" Visible="false" runat="server" OnClick="imgBtnRestore_Click" ToolTip="<%$ Resources:Attendance, Active %>"><span class="far fa-lightbulb"  style="font-size:25px;"></span></asp:LinkButton>&nbsp;&nbsp;&nbsp;
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="box box-warning box-solid" <%= gvHolidayMasterBin.Rows.Count>0?"style='display:block'":"style='display:none'"%>>
                                            <div class="box-body">
                                                <div class="row">
                                                    <div class="col-md-12">
                                                        <div style="overflow: auto">
                                                            <asp:GridView CssClass="table-striped table-bordered table table-hover" ID="gvHolidayMasterBin" PageSize="<%# PageControlCommon.GetPageSize() %>"
                                                                runat="server" AutoGenerateColumns="False" DataKeyNames="Holiday_Id" Width="100%"
                                                                AllowPaging="True" OnPageIndexChanging="gvHolidayMasterBin_PageIndexChanging"
                                                                OnSorting="gvHolidayMasterBin_OnSorting" AllowSorting="true">
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
                                                                    <asp:TemplateField HeaderText="<%$ Resources:Attendance,Holiday Id %>" SortExpression="Holiday_Id">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblHolidayId1" runat="server" Text='<%# Eval("Holiday_Id") %>'></asp:Label>
                                                                        </ItemTemplate>
                                                                        <ItemStyle />
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="<%$ Resources:Attendance,Holiday Name %>" SortExpression="Holiday_Name">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lbHolidayName" runat="server" Text='<%# Eval("Holiday_Name") %>'></asp:Label>
                                                                            <asp:Label ID="lblHolidayId" Visible="false" runat="server" Text='<%# Eval("Holiday_Id") %>'></asp:Label>
                                                                        </ItemTemplate>
                                                                        <ItemStyle />
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="<%$ Resources:Attendance,Holiday Name(Local) %>" SortExpression="Holiday_Name_L">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblHolidayNameL" runat="server" Text='<%# Eval("Holiday_Name_L") %>'></asp:Label>
                                                                        </ItemTemplate>
                                                                        <ItemStyle />
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="<%$ Resources:Attendance,From Date %>" SortExpression="From_Date">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblFromDate" runat="server" Text='<%# GetDate(Eval("From_Date")) %>'></asp:Label>
                                                                        </ItemTemplate>
                                                                        <ItemStyle />
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="<%$ Resources:Attendance,To Date %>" SortExpression="To_Date">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lbltoDate" runat="server" Text='<%# GetDate(Eval("To_Date")) %>'></asp:Label>
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
                                    </div>
                                </div>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </div>

                </div>
            </div>
        </div>
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



    <asp:UpdateProgress ID="UpdateProgress1" runat="server" AssociatedUpdatePanelID="Update_Button">
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

    <asp:UpdateProgress ID="UpdateProgress3" runat="server" AssociatedUpdatePanelID="Update_Holiday">
        <ProgressTemplate>
            <div class="modal_Progress">
                <div class="center_Progress">
                    <img alt="" src="../Images/ajax-loader2.gif" style="vertical-align: middle; background-color: White; border-radius: 10px;" />
                </div>
            </div>
        </ProgressTemplate>
    </asp:UpdateProgress>

    <asp:UpdateProgress ID="UpdateProgress5" runat="server" AssociatedUpdatePanelID="Update_Holiday_Group">
        <ProgressTemplate>
            <div class="modal_Progress">
                <div class="center_Progress">
                    <img alt="" src="../Images/ajax-loader2.gif" style="vertical-align: middle; background-color: White; border-radius: 10px;" />
                </div>
            </div>
        </ProgressTemplate>
    </asp:UpdateProgress>

    <asp:UpdateProgress ID="UpdateProgress8" runat="server" AssociatedUpdatePanelID="Update_Group">
        <ProgressTemplate>
            <div class="modal_Progress">
                <div class="center_Progress">
                    <img alt="" src="../Images/ajax-loader2.gif" style="vertical-align: middle; background-color: White; border-radius: 10px;" />
                </div>
            </div>
        </ProgressTemplate>
    </asp:UpdateProgress>

    <asp:UpdateProgress ID="UpdateProgress6" runat="server" AssociatedUpdatePanelID="Update_Emp">
        <ProgressTemplate>
            <div class="modal_Progress">
                <div class="center_Progress">
                    <img alt="" src="../Images/ajax-loader2.gif" style="vertical-align: middle; background-color: White; border-radius: 10px;" />
                </div>
            </div>
        </ProgressTemplate>
    </asp:UpdateProgress>

    <asp:UpdateProgress ID="UpdateProgress4" runat="server" AssociatedUpdatePanelID="Update_Bin">
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
    <script>
        function Validate(sender, args) {
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

        function isNumber(evt) {
            evt = (evt) ? evt : window.event;
            var charCode = (evt.which) ? evt.which : evt.keyCode;
            if (charCode > 31 && (charCode < 48 || charCode > 57)) {
                return false;
            }
            return true;
        }

        function postBackByObject() {
            var o = window.event.srcElement;
            if (o.tagName == "INPUT" && o.type == "checkbox") {
                __doPostBack("", "");
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
        function Li_Tab_Bin() {
            document.getElementById('<%= Btn_Bin.ClientID %>').click();
        }
        function Add_Address() {
            document.getElementById('<%= Btn_Address_Modal.ClientID %>').click();
        }
        function LI_New_Active() {
            $("#Li_List").removeClass("active");
            $("#List").removeClass("active");

            $("#Li_New").addClass("active");
            $("#New").addClass("active");
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

        function LI_List_Active() {
            $("#Li_List").addClass("active");
            $("#List").addClass("active");

            $("#Li_New").removeClass("active");
            $("#New").removeClass("active");
        }
        
        function Modal_Open_FileUpload() {
            document.getElementById('<%= Btn_Modal_FileUpload.ClientID %>').click();
        }
    </script>
</asp:Content>
