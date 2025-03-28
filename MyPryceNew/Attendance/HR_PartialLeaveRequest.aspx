<%@ Page Title="" Language="C#" MasterPageFile="~/ERPMaster.master" AutoEventWireup="true" CodeFile="HR_PartialLeaveRequest.aspx.cs" Inherits="Attendance_HR_PartialLeaveRequest" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="Head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="HeaderContent" runat="Server">
    <h1>
        <img src="../Images/hr_partial_leave_request.png" alt="" />
        <asp:Label ID="lblHeader" runat="server" Text="<%$ Resources:Attendance,Partial Leave Setup%>"></asp:Label>
    </h1>
    <ol class="breadcrumb">

        <li><a href="#"><i class="fa fa-dashboard"></i>
            <asp:Label ID="Label6" runat="server" Text="<%$ Resources:Attendance,Attendance Module%>"></asp:Label></a></li>
        <li><a href="#"><i class="fa fa-dashboard"></i>
            <asp:Label ID="Label10" runat="server" Text="<%$ Resources:Attendance,Attendance Module%>"></asp:Label></a></li>
        <li class="active">
            <asp:Label ID="Label5" runat="server" Text="<%$ Resources:Attendance,Partial Leave Setup%>"></asp:Label></li>
    </ol>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="Server">
    <asp:UpdatePanel ID="Update_Button" runat="server">
        <ContentTemplate>
            <asp:Button ID="Btn_New" runat="server" Style="display: none;" Text="New" OnClick="btnNew_Click" />
            <asp:HiddenField runat="server" ID="hdnCanPrint" />
            <asp:HiddenField runat="server" ID="hdnCanEdit" />
        </ContentTemplate>
    </asp:UpdatePanel>
    <asp:UpdateProgress ID="Update_Progress1" runat="server" AssociatedUpdatePanelID="Update_Button">
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
                    <li id="Li_New" class="active"><a href="#New" data-toggle="tab">
                        <asp:UpdatePanel ID="Update_Li" runat="server">
                            <ContentTemplate>
                                <i class="fa fa-file"></i>&nbsp;&nbsp;
                                <asp:Label ID="Lbl_Tab_New" runat="server" Text="<%$ Resources:Attendance,New%>"></asp:Label>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </a></li>
                    <li id="Li_List"><a onclick="Li_List_Click()" href="#List" data-toggle="tab">
                        <i class="fa fa-list"></i>&nbsp;&nbsp;<asp:Label ID="Label1" runat="server" Text="<%$ Resources:Attendance,List %>"></asp:Label></a></li>
                </ul>
                <div class="tab-content">
                    <div class="tab-pane" id="List">
                        <asp:UpdatePanel ID="Update_List" runat="server">
                            <ContentTemplate>


                                <div class="box box-warning box-solid">
                                    <div class="box-body">

                                        <div class="row">
                                            <div class="col-md-12">
                                                <div id="Div2" runat="server" class="box box-info collapsed-box">
                                                    <div class="box-header with-border">
                                                        <h3 class="box-title">
                                                            <asp:Label ID="Label9" runat="server" Text="<%$ Resources:Attendance,Advance Search%>"></asp:Label></h3>
                                                        &nbsp;&nbsp;|&nbsp;&nbsp;
					 <asp:Label ID="lblTotalRecords" Font-Bold="true" runat="server" Text="<%$ Resources:Attendance,Total Records: 0 %>"></asp:Label>
                                                        <asp:Label ID="lblSelectedRecord" runat="server" Visible="false"></asp:Label>

                                                        <div class="box-tools pull-right">
                                                            <button type="button" class="btn btn-box-tool" data-widget="collapse">
                                                                <i id="I2" runat="server" class="fa fa-plus"></i>
                                                            </button>
                                                        </div>
                                                    </div>
                                                    <div class="box-body">



                                                        <div class="col-lg-6">
                                                            <asp:Label runat="server" ID="Label11" Text="<%$ Resources:Attendance,Location%>"></asp:Label>
                                                            <asp:DropDownList runat="server" ID="ddlLoc" OnSelectedIndexChanged="ddlLoc_SelectedIndexChanged" AutoPostBack="true" CssClass="form-control"></asp:DropDownList>
                                                            <br />
                                                        </div>

                                                        <div class="col-lg-6">
                                                            <asp:Label runat="server" ID="lblYear" Text="<%$ Resources:Attendance,Year%>"></asp:Label>
                                                            <asp:DropDownList runat="server" ID="ddlYearList" OnSelectedIndexChanged="ddlYearList_SelectedIndexChanged" AutoPostBack="true" CssClass="form-control"></asp:DropDownList>
                                                            <br />
                                                        </div>

                                                        <div class="col-lg-2">
                                                            <asp:DropDownList ID="ddlLeaveStatus" runat="server" CssClass="form-control" AutoPostBack="true" OnSelectedIndexChanged="ddlLeaveStatus_SelectedIndexChanged">
                                                                <asp:ListItem Text="All" Value="All"></asp:ListItem>
                                                                <asp:ListItem Text="Pending" Value="Pending" Selected="True"></asp:ListItem>
                                                                <asp:ListItem Text="Approved" Value="Approved"></asp:ListItem>
                                                                <asp:ListItem Text="Rejected" Value="Canceled"></asp:ListItem>
                                                            </asp:DropDownList>
                                                        </div>
                                                        <div class="col-lg-2">
                                                            <asp:DropDownList ID="ddlFieldName" runat="server" CssClass="form-control" OnSelectedIndexChanged="ddlFieldName_SelectedIndexChanged" AutoPostBack="true">
                                                                <asp:ListItem Selected="True" Text="<%$ Resources:Attendance,Employee Code %>" Value="Emp_Code"></asp:ListItem>
                                                                <asp:ListItem Text="<%$ Resources:Attendance,Employee Name %>" Value="Emp_Name"></asp:ListItem>
                                                                <asp:ListItem Text="<%$ Resources:Attendance,Type %>" Value="Partial_Leave_Type"></asp:ListItem>
                                                                <asp:ListItem Text="<%$ Resources:Attendance,Partial leave date%>" Value="Partial_Leave_Date"></asp:ListItem>

                                                            </asp:DropDownList>
                                                        </div>
                                                        <div class="col-lg-2">
                                                            <asp:DropDownList ID="DropDownList1" runat="server" CssClass="form-control">
                                                                <asp:ListItem Text="<%$ Resources:Attendance,--Select-- %>"></asp:ListItem>
                                                                <asp:ListItem Text="<%$ Resources:Attendance,Equal %>"></asp:ListItem>
                                                                <asp:ListItem Selected="True" Text="<%$ Resources:Attendance,Contains %>"></asp:ListItem>
                                                                <asp:ListItem Text="<%$ Resources:Attendance,Like %>"></asp:ListItem>
                                                            </asp:DropDownList>
                                                        </div>
                                                        <div class="col-lg-4">
                                                            <asp:Panel ID="Panel2" runat="server" DefaultButton="btnbind">
                                                                <asp:TextBox ID="txtValueFilter" runat="server" CssClass="form-control" placeholder="Search from Content"></asp:TextBox>
                                                                <asp:TextBox ID="txtValueDate" runat="server" CssClass="form-control" Visible="false" placeholder="Search from Date"></asp:TextBox>
                                                                <cc1:CalendarExtender OnClientShown="showCalendar" ID="CalendarExtendertxtValueDate" runat="server" TargetControlID="txtValueDate" />
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

                                        <div class="row">
                                            <div class="col-md-12" <%= gvLeaveStatus.Rows.Count>0?"style='display:block'":"style='display:none'"%>>
                                                <div class="flow">
                                                    <asp:HiddenField ID="HDFSort" runat="server" />
                                                    <asp:GridView CssClass="table-striped table-bordered table table-hover" ID="gvLeaveStatus" PageSize="<%# PageControlCommon.GetPageSize() %>" OnSorting="gvLeaveStatus_OnSorting"
                                                        runat="server" AutoGenerateColumns="False" Width="100%" OnPageIndexChanging="gvLeaveStatus_PageIndexChanging"
                                                        AllowPaging="true" AllowSorting="true">
                                                        <Columns>

                                                            <asp:TemplateField HeaderText="Action">
                                                                <ItemTemplate>
                                                                    <div class="dropdown" style="position: absolute;">
                                                                        <button class="btn btn-default dropdown-toggle" type="button" data-toggle="dropdown">
                                                                            <i class="fa fa-ellipsis-h" aria-hidden="true"></i>
                                                                        </button>
                                                                        <ul class="dropdown-menu">
                                                                            <li <%= hdnCanPrint.Value=="true"?"style='display:block'":"style='display:none'" %>>
                                                                                <asp:LinkButton ID="IbtnPrint" runat="server" CausesValidation="False" CommandArgument='<%# Eval("Trans_Id") %>' CommandName='<%# Eval("Emp_Id") %>' OnCommand="IbtnPrint_Command"><i class="fa fa-print"></i><%# Resources.Attendance.Print%></asp:LinkButton>
                                                                            </li>
                                                                            <%-- <li <%= hdnCanEdit.Value=="true"?"style='display:block'":"style='display:none'" %>>
                                                                                <asp:LinkButton ID="btnEdit" runat="server" CommandArgument='<%# Eval("Trans_Id") %>' OnCommand="btnEdit_Command" CausesValidation="False"><i class="fa fa-pencil"></i>Edit </asp:LinkButton>
                                                                            </li>--%>
                                                                        </ul>
                                                                    </div>

                                                                </ItemTemplate>
                                                            </asp:TemplateField>

                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,Employee Id %>" SortExpression="Emp_Code">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblEmpId" runat="server" Text='<%# Eval("Emp_Code") %>'></asp:Label>
                                                                    <asp:Label ID="lblEmployeeId" runat="server" Text='<%# Eval("Emp_Id") %>' Visible="false"></asp:Label>
                                                                </ItemTemplate>
                                                                <ItemStyle />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,Employee Name  %>" SortExpression="Emp_Name">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblEmpName" runat="server" Text='<%# Eval("Emp_Name") %>'></asp:Label>
                                                                </ItemTemplate>
                                                                <ItemStyle />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,Type %>" SortExpression="Partial_Leave_Type">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblScheduleType" runat="server" Text='<%# leavetype(Eval("Partial_Leave_Type").ToString()) %>'></asp:Label>
                                                                </ItemTemplate>
                                                                <ItemStyle />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,Apply Date %>" SortExpression="Partial_Leave_Date">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblApplicatonDate" runat="server" Text='<%# GetDate(Eval("Partial_Leave_Date")) %>'></asp:Label>
                                                                </ItemTemplate>
                                                                <ItemStyle />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,From Time %>" SortExpression="From_Time">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblFromDate" runat="server" Text='<%# Eval("From_Time") %>'></asp:Label>
                                                                </ItemTemplate>
                                                                <ItemStyle />
                                                            </asp:TemplateField>

                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,To Time %>" SortExpression="To_Time">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblTodays" runat="server" Text='<%# Eval("To_Time") %>'></asp:Label>
                                                                </ItemTemplate>
                                                                <ItemStyle />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,Status %>" SortExpression="Is_Confirmed">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblStatus" runat="server" Text='<%# Eval("Is_Confirmed")%>'></asp:Label>
                                                                    <asp:HiddenField ID="hdndesc" runat="server" Value='<%Eval("Description") %>' />
                                                                </ItemTemplate>
                                                                <ItemStyle />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField ItemStyle-HorizontalAlign="Center">
                                                                <ItemTemplate>
                                                                    <asp:UpdatePanel ID="UpdatePanel16" runat="server">
                                                                        <Triggers>
                                                                            <asp:PostBackTrigger ControlID="ImgDownloadList" />
                                                                        </Triggers>
                                                                        <ContentTemplate>
                                                                            <asp:Label ID="lblFileFullPathList" runat="server" Visible="false" Text='<%#Eval("FileUrl") %>' />
                                                                            <asp:Label ID="lblFileDownloadList" runat="server" Visible="false" Text='<%#Eval("FileName") %>' />
                                                                            <asp:LinkButton ID="ImgDownloadList" runat="server" CommandArgument='<%#Eval("Trans_id") %>' OnCommand="OnDownloadDocumentCommandList" ToolTip="<%$ Resources:Attendance,Download %>"><i class="fa fa-download"></i></asp:LinkButton>
                                                                        </ContentTemplate>
                                                                    </asp:UpdatePanel>
                                                                </ItemTemplate>
                                                                <HeaderStyle HorizontalAlign="Center" />
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
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </div>
                    <div class="tab-pane active" id="New">
                        <asp:UpdatePanel ID="Update_New" runat="server">
                            <ContentTemplate>
                                <div class="row">
                                    <div class="col-md-12">
                                        <div class="box box-primary">
                                            <div class="box-body">
                                                <div class="form-group">
                                                    <div class="col-md-12" style="text-align: center">
                                                        <asp:RadioButton ID="rbtnDIN" runat="server" GroupName="Method"
                                                            Text="<%$ Resources:Attendance,Direct In%>" AutoPostBack="true" />
                                                        <asp:RadioButton ID="rbtnDout" Style="margin-left: 50px;" runat="server" GroupName="Method"
                                                            Text="<%$ Resources:Attendance,Direct Out %>" AutoPostBack="true" />
                                                        <asp:RadioButton ID="rbtnBetween" Style="margin-left: 50px;" runat="server" GroupName="Method"
                                                            Text="<%$ Resources:Attendance,Between In Out %>" AutoPostBack="true" />
                                                        <br />
                                                    </div>

                                                    <div class="col-md-12" style="text-align: center">
                                                        <asp:RadioButton ID="rbtnPersonal" runat="server" GroupName="Partial"
                                                            Text="<%$ Resources:Attendance,Personal%>" AutoPostBack="true" OnCheckedChanged="rbtnPersonal_CheckedChanged" />
                                                        <asp:RadioButton ID="rbtnOfficial" Style="margin-left: 50px;" runat="server" GroupName="Partial"
                                                            Text="<%$ Resources:Attendance,Offical %>" AutoPostBack="true" OnCheckedChanged="rbtnOfficial_CheckedChanged" />
                                                        <br />
                                                    </div>
                                                    <div id="dvOfficial" runat="server" visible="false" class="col-md-12">
                                                        <div class="row">
                                                            <div>
                                                                <br />
                                                                <div class="col-md-6">
                                                                    <asp:Label ID="lblLocation" runat="server" Text="<%$ Resources:Attendance,Location %>"></asp:Label>

                                                                    <asp:DropDownList ID="ddlLocation" runat="server" CssClass="form-control" AutoPostBack="true"
                                                                        OnSelectedIndexChanged="dpLocation_SelectedIndexChanged">
                                                                    </asp:DropDownList>
                                                                </div>
                                                                <div class="col-md-6">
                                                                    <asp:Label ID="lblGroupByDept" runat="server" Text="<%$ Resources:Attendance,Department %>"></asp:Label>
                                                                    <div class="input-group">
                                                                        <asp:DropDownList ID="dpDepartment" runat="server" CssClass="form-control" AutoPostBack="true"
                                                                            OnSelectedIndexChanged="dpDepartment_SelectedIndexChanged">
                                                                        </asp:DropDownList>
                                                                        <div class="input-group-btn">
                                                                            <asp:LinkButton ID="ImageButton1" runat="server" CausesValidation="False" OnClick="btnAllRefresh_Click" ToolTip="<%$ Resources:Attendance,Refresh %>"><span class="fa fa-repeat"  style="font-size:25px;margin-left:5px"></span></asp:LinkButton>
                                                                            <br />
                                                                        </div>
                                                                    </div>
                                                                </div>
                                                            </div>
                                                        </div>
                                                        <br />


                                                        <div class="row">
                                                            <div class="col-md-12">
                                                                <%--class="box box-info collapsed-box"--%>
                                                                <div id="Div1" runat="server">
                                                                    <div class="box-header with-border">
                                                                        <h3 class="box-title">
                                                                            <asp:Label ID="Label22" runat="server" Text="Advance Search"></asp:Label></h3>
                                                                        &nbsp;&nbsp;|&nbsp;&nbsp;
					<asp:Label ID="lblTotalRecord" Font-Bold="true" Text="<%$ Resources:Attendance,Total Records: 0 %>" runat="server"></asp:Label>

                                                                        <div class="box-tools pull-right">
                                                                            <button type="button" class="btn btn-box-tool" data-widget="collapse">
                                                                                <%--<i id="I1" runat="server" class="fa fa-plus"></i>--%>
                                                                            </button>
                                                                        </div>
                                                                    </div>
                                                                    <div class="box-body">
                                                                        <div class="col-lg-3">
                                                                            <asp:DropDownList ID="ddlField" runat="server" class="form-control">
                                                                                <asp:ListItem Selected="True" Text="<%$ Resources:Attendance,Employee Code %>" Value="Emp_Code"></asp:ListItem>
                                                                                <asp:ListItem Text="<%$ Resources:Attendance,Employee Name %>" Value="Emp_Name"></asp:ListItem>
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
                                                                            <asp:Panel ID="Panel1" runat="server" DefaultButton="ImageButton8">
                                                                                <asp:TextBox ID="txtValue" runat="server" placeholder="Search from Content" class="form-control"></asp:TextBox>
                                                                            </asp:Panel>

                                                                        </div>
                                                                        <div class="col-lg-2">
                                                                            <asp:LinkButton ID="ImageButton8" runat="server" CausesValidation="False" OnClick="btnarybind_Click1" ToolTip="<%$ Resources:Attendance,Search %>"><span class="fa fa-search"  style="font-size:25px;"></span></asp:LinkButton>&nbsp;&nbsp;&nbsp;
                                                                            <asp:LinkButton ID="ImageButton9" runat="server" CausesValidation="False" OnClick="btnaryRefresh_Click1" ToolTip="<%$ Resources:Attendance,Refresh %>"><span class="fa fa-repeat"  style="font-size:25px;"></span></asp:LinkButton>&nbsp;&nbsp;&nbsp;
                                                                            <asp:LinkButton ID="ImageButton10" runat="server" OnClick="ImgbtnSelectAll_Clickary" ToolTip="<%$ Resources:Attendance, Select All %>"><span class="fas fa-th"  style="font-size:25px;"></span></asp:LinkButton>
                                                                        </div>

                                                                    </div>
                                                                </div>
                                                            </div>
                                                        </div>


                                                        <div class="box box-warning box-solid" <%= GvEmpList.Rows.Count>0?"style='display:block'":"style='display:none'"%>>
                                                            <div class="box-body">
                                                                <div class="row">
                                                                    <div class="col-md-12">
                                                                        <div class="flow">
                                                                            <asp:Label ID="lblSelectRecord" runat="server" Visible="false"></asp:Label>
                                                                            <asp:GridView CssClass="table-striped table-bordered table table-hover" ID="GvEmpList" runat="server" AutoGenerateColumns="False" AllowPaging="true"
                                                                                OnPageIndexChanging="GvEmpList_PageIndexChanging" Width="100%"
                                                                                DataKeyNames="Emp_Id" PageSize="<%# PageControlCommon.GetPageSize() %>">
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
                                                                                        SortExpression="Emp_Name" ItemStyle-Width="40%" />
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


                                                        <br />
                                                    </div>
                                                    <div id="dvEmp" runat="server" class="col-md-12">
                                                        <div class="col-md-12">
                                                            <div class="col-md-6">
                                                                <asp:HiddenField ID="hdnEmpId" runat="server" />
                                                                <asp:HiddenField ID="hdnEdit" runat="server" />
                                                                <asp:Label ID="Label3" runat="server" Text="<%$ Resources:Attendance,Employee Name %>"></asp:Label>
                                                                <asp:TextBox ID="txtEmpName" runat="server" CssClass="form-control" BackColor="#eeeeee"
                                                                    AutoPostBack="true" OnTextChanged="txtEmpName_textChanged" />
                                                                <cc1:AutoCompleteExtender ID="AutoCompleteExtender5" runat="server" DelimiterCharacters=""
                                                                    Enabled="True" ServiceMethod="GetCompletionListEmployeeName" ServicePath="" CompletionInterval="100"
                                                                    MinimumPrefixLength="1" CompletionSetCount="1" TargetControlID="txtEmpName" UseContextKey="True"
                                                                    CompletionListCssClass="autoCompleteList" CompletionListItemCssClass="autoCompleteListItem" CompletionListHighlightedItemCssClass="autoCompleteSelectedListItem" OnClientShown="resetPosition">
                                                                </cc1:AutoCompleteExtender>
                                                                <br />
                                                            </div>
                                                        </div>

                                                    </div>
                                                    <div class="col-md-12">
                                                        <div class="col-md-12">
                                                            <div class="col-md-6">
                                                                <asp:Label ID="Label2" runat="server" Text="<%$ Resources:Attendance,Apply Date %>"></asp:Label>
                                                                <asp:TextBox ID="txtApplyDate" runat="server" CssClass="form-control" />
                                                                <asp:HiddenField ID="hdnEditDate" runat="server" Value="0" />
                                                                <cc1:CalendarExtender OnClientShown="showCalendar" ID="CalendarExtender2" runat="server" Enabled="True" TargetControlID="txtApplyDate">
                                                                </cc1:CalendarExtender>
                                                                <br />
                                                            </div>
                                                            <div class="col-md-6">
                                                                <asp:Button ID="btnGetTimings" runat="server" Text="<%$ Resources:Attendance,Get Timings %>"
                                                                    Visible="true" CssClass="btn btn-primary" OnClick="btnGetTimings_Click" />
                                                            </div>
                                                        </div>

                                                        <div class="col-md-12">
                                                            <div class="col-md-3">
                                                                <asp:Label ID="Label4" runat="server" Text="<%$ Resources:Attendance,From Time %>"></asp:Label>
                                                                <asp:TextBox ID="txtInTime" runat="server" CssClass="form-control" />
                                                                <cc1:MaskedEditExtender ID="MaskedEditExtender1" runat="server" CultureAMPMPlaceholder=""
                                                                    CultureCurrencySymbolPlaceholder="" CultureDateFormat="" CultureDatePlaceholder=""
                                                                    CultureDecimalPlaceholder="" CultureThousandsPlaceholder="" CultureTimePlaceholder=""
                                                                    Enabled="True" ErrorTooltipEnabled="True" Mask="99:99" MaskType="Time" TargetControlID="txtInTime"
                                                                    UserTimeFormat="TwentyFourHour" MessageValidatorTip="true" InputDirection="LeftToRight">
                                                                </cc1:MaskedEditExtender>
                                                                <cc1:MaskedEditValidator ID="EarliestTimeValidator" runat="server" ControlExtender="MaskedEditExtender1"
                                                                    ControlToValidate="txtInTime" Display="Dynamic" InvalidValueMessage="Please enter a valid time."
                                                                    SetFocusOnError="True" />
                                                                <br />
                                                            </div>
                                                            <div class="col-md-3">
                                                                <asp:Label ID="Label7" runat="server" Text="<%$ Resources:Attendance,To Time %>"></asp:Label>
                                                                <asp:TextBox ID="txtOuttime" runat="server" CssClass="form-control" />
                                                                <cc1:MaskedEditExtender ID="txtOnDuty_MaskedEditExtender" runat="server" CultureAMPMPlaceholder=""
                                                                    CultureCurrencySymbolPlaceholder="" CultureDateFormat="" CultureDatePlaceholder=""
                                                                    CultureDecimalPlaceholder="" CultureThousandsPlaceholder="" CultureTimePlaceholder=""
                                                                    Enabled="True" ErrorTooltipEnabled="True" Mask="99:99" MaskType="Time" TargetControlID="txtOuttime"
                                                                    UserTimeFormat="TwentyFourHour" MessageValidatorTip="true" InputDirection="LeftToRight">
                                                                </cc1:MaskedEditExtender>
                                                                <cc1:MaskedEditValidator ID="MaskedEditValidator1" runat="server" ControlExtender="txtOnDuty_MaskedEditExtender"
                                                                    ControlToValidate="txtOuttime" Display="Dynamic" InvalidValueMessage="Please enter a valid time."
                                                                    SetFocusOnError="True" />
                                                                <br />
                                                            </div>
                                                            <div id="TrWithOutTime1" runat="server" visible="false" class="col-md-12">
                                                                <asp:Label ID="lblPLType" runat="server" Text="<%$ Resources:Attendance,PL Type %>" />
                                                                <asp:RadioButton ID="rbBegining" runat="server" Text="Begining"
                                                                    GroupName="RB" OnCheckedChanged="rbBegining_CheckedChanged" AutoPostBack="true" />
                                                                <asp:RadioButton ID="rbMiddle" runat="server" Text="Middle"
                                                                    GroupName="RB" OnCheckedChanged="rbBegining_CheckedChanged" AutoPostBack="true" />
                                                                <asp:RadioButton ID="rbEnding" runat="server" Text="Ending"
                                                                    GroupName="RB" OnCheckedChanged="rbBegining_CheckedChanged" AutoPostBack="true" />
                                                                <br />
                                                            </div>
                                                            <div id="TrWithOutTime2" runat="server" visible="false" class="col-md-12">
                                                                <asp:RadioButtonList ID="rbTimeTable" runat="server" />
                                                                <br />
                                                            </div>

                                                            <div class="col-md-12">
                                                                <asp:HiddenField ID="HiddenField1" runat="server" />
                                                                <asp:Label ID="Label8" runat="server" Text="<%$ Resources:Attendance,Description %>"></asp:Label>
                                                                <asp:TextBox ID="txtDescription" TextMode="MultiLine" runat="server"
                                                                    CssClass="form-control" />
                                                                <br />
                                                            </div>
                                                            <div class="col-md-12">
                                                                <%-- <asp:Label ID="lblEmpLogo" runat="server" Text="Leave Attachment"></asp:Label>--%>
                                                                <div class="input-group" style="width: 100%;">
                                                                    <cc1:AsyncFileUpload ID="FULogoPath1" OnClientUploadStarted="FuLogo_UploadStarted" OnClientUploadError="FuLogo_UploadError" OnClientUploadComplete="FuLogo_UploadComplete" OnUploadedComplete="FuLogo_FileUploadComplete" runat="server" CssClass="form-control" CompleteBackColor="White" UploaderStyle="Traditional" UploadingBackColor="#CCFFFF" ThrobberID="FULogo_ImgLoader1" Width="100%" />
                                                                    <div class="input-group-btn" style="border: solid 1px #d2d6de; width: 35px;">
                                                                        <asp:LinkButton ID="FULogo_Img_Right1" runat="server" Width="30px" Height="30px" Style="display: none"><i class="fa fa-check" style="font-size:30px;color:#22cb33"></i></asp:LinkButton>
                                                                        <asp:LinkButton ID="FULogo_Img_Wrong1" runat="server" Width="30px" Height="30px" Style="display: none"><i class="fa fa-remove" style="font-size:30px"></i></asp:LinkButton>
                                                                        <asp:Image ID="FULogo_ImgLoader1" runat="server" ImageUrl="../Images/loader.gif" />
                                                                    </div>
                                                                </div>
                                                                <br />
                                                            </div>
                                                            <div class="col-md-12" style="text-align: center">
                                                                <asp:HiddenField ID="HiddenField2" runat="server" />
                                                                <asp:Button ID="btnApply" runat="server" Text="<%$ Resources:Attendance,Apply %>"
                                                                    Visible="false" CssClass="btn btn-success" OnClick="btnApply_Click" />
                                                                &nbsp;&nbsp;
                                                    <asp:Button ID="btnReset" runat="server" Text="<%$ Resources:Attendance,Reset %>"
                                                        Visible="true" CssClass="btn btn-primary" OnClick="btnReset_Click" />
                                                                &nbsp;&nbsp;
                                                    <asp:Button ID="btnCancel" runat="server" Text="<%$ Resources:Attendance,Cancel %>"
                                                        Visible="true" CssClass="btn btn-danger" OnClick="btnCancel_Click" />
                                                                <br />
                                                            </div>

                                                            <div class="col-md-12" class="flow">
                                                                <div class="col-md-12">
                                                                    <br />
                                                                    <asp:GridView CssClass="table-striped table-bordered table table-hover" ID="gvLeaveSummary_PartialLeave" runat="server" AutoGenerateColumns="False"
                                                                        Width="100%">
                                                                        <Columns>
                                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,Total Leave %>">
                                                                                <ItemTemplate>
                                                                                    <asp:Label ID="lblTotalDays" runat="server" Text='<%# Eval("Total_Days") %>'></asp:Label>
                                                                                </ItemTemplate>
                                                                                <ItemStyle Width="25%" />
                                                                            </asp:TemplateField>
                                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,Used Leave %>">
                                                                                <ItemTemplate>
                                                                                    <asp:Label ID="lblUsedDays1" runat="server" Text='<%# Eval("Used_Days") %>'></asp:Label>
                                                                                </ItemTemplate>
                                                                                <ItemStyle Width="25%" />
                                                                            </asp:TemplateField>
                                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,Pending Leave %>">
                                                                                <ItemTemplate>
                                                                                    <asp:Label ID="lblUsedDays" runat="server" Text='<%# Eval("Pending_Days") %>'></asp:Label>
                                                                                </ItemTemplate>
                                                                                <ItemStyle Width="25%" />
                                                                            </asp:TemplateField>
                                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,Remaning Leave %>">
                                                                                <ItemTemplate>
                                                                                    <asp:Label ID="lblRemainingDays" runat="server" Text='<%# Eval("Remaning_Days") %>'></asp:Label>
                                                                                </ItemTemplate>
                                                                                <ItemStyle Width="25%" />
                                                                            </asp:TemplateField>
                                                                        </Columns>
                                                                        <PagerStyle CssClass="pagination-ys" />

                                                                    </asp:GridView>
                                                                    <br />
                                                                </div>
                                                            </div>
                                                            <div class="col-md-12" class="flow">
                                                                <div class="col-md-12">
                                                                    <asp:GridView CssClass="table-striped table-bordered table table-hover" ID="gvEmpPendingLeave" PageSize="<%# PageControlCommon.GetPageSize() %>"
                                                                        runat="server" AutoGenerateColumns="False" Width="100%" OnPageIndexChanging="gvEmpPendingLeave_PageIndexChanging"
                                                                        AllowPaging="true">
                                                                        <Columns>
                                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,Employee Id %>">
                                                                                <ItemTemplate>
                                                                                    <asp:Label ID="lblEmpId" runat="server" Text='<%# Eval("Emp_Code") %>'></asp:Label>
                                                                                    <asp:Label ID="lblEmployeeId" runat="server" Text='<%# Eval("Emp_Id") %>' Visible="false"></asp:Label>
                                                                                </ItemTemplate>
                                                                                <ItemStyle />
                                                                            </asp:TemplateField>
                                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,Employee Name  %>">
                                                                                <ItemTemplate>
                                                                                    <asp:Label ID="lblEmpName" runat="server" Text='<%# Eval("Emp_Name") %>'></asp:Label>
                                                                                </ItemTemplate>
                                                                                <ItemStyle />
                                                                            </asp:TemplateField>
                                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,Type %>">
                                                                                <ItemTemplate>
                                                                                    <asp:Label ID="lblScheduleType" runat="server" Text='<%# leavetype(Eval("Partial_Leave_Type").ToString()) %>'></asp:Label>
                                                                                </ItemTemplate>
                                                                                <ItemStyle />
                                                                            </asp:TemplateField>
                                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,Apply Date %>">
                                                                                <ItemTemplate>
                                                                                    <asp:Label ID="lblApplicatonDate" runat="server" Text='<%# GetDate(Eval("Partial_Leave_Date")) %>'></asp:Label>
                                                                                </ItemTemplate>
                                                                                <ItemStyle />
                                                                            </asp:TemplateField>
                                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,From Time %>">
                                                                                <ItemTemplate>
                                                                                    <asp:Label ID="lblFromDate" runat="server" Text='<%# Eval("From_Time") %>'></asp:Label>
                                                                                </ItemTemplate>
                                                                                <ItemStyle />
                                                                            </asp:TemplateField>
                                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,To Time %>">
                                                                                <ItemTemplate>
                                                                                    <asp:Label ID="lblTodays" runat="server" Text='<%# Eval("To_Time") %>'></asp:Label>
                                                                                </ItemTemplate>
                                                                                <ItemStyle />
                                                                            </asp:TemplateField>
                                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,PL Type %>">
                                                                                <ItemTemplate>
                                                                                    <asp:Label ID="lblgvPLType" runat="server" Text='<%# Eval("Partial_Leave_Type").ToString()=="0"?"Personal":"Official" %>'></asp:Label>
                                                                                </ItemTemplate>
                                                                                <ItemStyle />
                                                                            </asp:TemplateField>
                                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,Status %>">
                                                                                <ItemTemplate>
                                                                                    <asp:Label ID="lblStatus" runat="server" Text='<%# Eval("Is_Confirmed")%>'></asp:Label>
                                                                                    <asp:HiddenField ID="hdndesc" runat="server" Value='<%Eval("Description") %>' />
                                                                                </ItemTemplate>
                                                                                <ItemStyle />
                                                                            </asp:TemplateField>
                                                                            <asp:TemplateField ItemStyle-HorizontalAlign="Center">
                                                                                <ItemTemplate>
                                                                                    <asp:UpdatePanel ID="UpdatePanel17" runat="server">
                                                                                        <Triggers>
                                                                                            <asp:PostBackTrigger ControlID="ImgDownloadList1" />
                                                                                        </Triggers>
                                                                                        <ContentTemplate>
                                                                                            <asp:Label ID="lblFileFullPathList1" runat="server" Visible="false" Text='<%#Eval("FileUrl") %>' />
                                                                                            <asp:Label ID="lblFileDownloadList1" runat="server" Visible="false" Text='<%#Eval("FileName") %>' />
                                                                                            <asp:LinkButton ID="ImgDownloadList1" runat="server" CommandArgument='<%#Eval("Trans_id") %>' OnCommand="OnDownloadDocumentCommandList" ToolTip="<%$ Resources:Attendance,Download %>"><i class="fa fa-download"></i></asp:LinkButton>
                                                                                        </ContentTemplate>
                                                                                    </asp:UpdatePanel>
                                                                                </ItemTemplate>
                                                                                <HeaderStyle HorizontalAlign="Center" />
                                                                                <ItemStyle HorizontalAlign="Center" Width="60px" />
                                                                            </asp:TemplateField>
                                                                            <asp:TemplateField ItemStyle-HorizontalAlign="Center">
                                                                                <ItemTemplate>
                                                                                    <asp:Button ID="ChoosefileUpload" runat="server" OnClientClick='<%# string.Format("javascript:return OpenModel({0})", Eval("Trans_id"))%>' Text="Upload" />
                                                                                </ItemTemplate>
                                                                            </asp:TemplateField>





                                                                        </Columns>


                                                                        <PagerStyle CssClass="pagination-ys" />

                                                                    </asp:GridView>
                                                                    <br />
                                                                </div>
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
<div class="modal fade" id="myModal" role="dialog" aria-labelledby="myModalLabel" aria-hidden="true">
    <div class="modal-dialog">
        <div class="modal-content">
            <div class="modal-header">
                <button type="button" class="close" data-dismiss="modal" aria-hidden="true">&times;</button>
                <h4 class="modal-title">Upload File</h4>
                <asp:HiddenField ID="hidtransId" runat="server" Value="" />
            </div>

            <div class="modal-body">
                          <div class="col-md-12">
                                                              <%-- <asp:Label ID="lblEmpLogo" runat="server" Text="Leave Attachment"></asp:Label>--%>
                <div class="input-group" style="width: 100%;">
                     <cc1:AsyncFileUpload ID="FULogoPath" OnClientUploadStarted="FuLogo_UploadStarted" OnClientUploadError="FuLogo_UploadError" OnClientUploadComplete="FuLogo_UploadComplete" OnUploadedComplete="FuLogo_FileUploadComplete" runat="server" CssClass="form-control" CompleteBackColor="White" UploaderStyle="Traditional" UploadingBackColor="#CCFFFF" ThrobberID="FULogo_ImgLoader" Width="100%" />
                                                                <div class="input-group-btn" style="border: solid 1px #d2d6de; width: 35px;">
                                                                    <asp:LinkButton ID="FULogo_Img_Right" runat="server" Width="30px" Height="30px" Style="display: none"><i class="fa fa-check" style="font-size:30px;color:#22cb33"></i></asp:LinkButton>
                                                                    <asp:LinkButton ID="FULogo_Img_Wrong" runat="server" Width="30px" Height="30px" Style="display: none"><i class="fa fa-remove" style="font-size:30px"></i></asp:LinkButton>
                                                                    <asp:Image ID="FULogo_ImgLoader" runat="server" ImageUrl="../Images/loader.gif" />

                             </div>
                 </div>
                              <br />
                        </div>

            </div>
            <div class="modal-footer">
                <%--<button class="btn btn-info" id="CloseModel" data-dismiss="modal" aria-hidden="true">Close</button>  --%>         
              <asp:Button ID="Button2" runat="server" Text="<%$ Resources:Attendance,Close %>" CssClass="btn btn-primary" OnClick="btnClose_Click" AutoPostBack="true"/>
            </div>
        </div>
    </div>
</div>
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="AfterLoadScriptContent" runat="Server">
    <script src="../Script/common.js"></script>
    <script type="text/javascript">

        function Li_New_Click() {
            document.getElementById("<%=Btn_New.ClientID %>").click();
        }


        function Li_Edit_Active() {
            $('#Li_List').removeClass('active');
            $('#List').removeClass('active');

            $('#Li_New').addClass('active');
            $('#New').addClass('active');
        }

        function LI_List_Active() {
            $("#Li_List").addClass("active");
            $("#List").addClass("active");

            $("#Li_New").removeClass("active");
            $("#New").removeClass("active");
        }
    </script>

   <script>
function OpenModel(transId) {
  debugger;
            //$('#< %=hidtransId.ClientID% >').val(transId);
 $('#<% =hidtransId.ClientID %>').attr('value', transId);
            //document.getElementById('< %=hidtransId.ClientID%>').val = transId;
 $('#myModal').modal('show');
return true;
        }
</script>
       <script type="text/javascript">
           function FuLogo_UploadComplete(sender, args) {
               document.getElementById('<%= FULogo_Img_Wrong.ClientID %>').style.display = "none";
            document.getElementById('<%= FULogo_Img_Right.ClientID %>').style.display = ""; 
             document.getElementById('<%= FULogo_Img_Wrong1.ClientID %>').style.display = "none";
            document.getElementById('<%= FULogo_Img_Right1.ClientID %>').style.display = ""; 
            <%--var img = document.getElementById('<%= imgLogo.ClientID %>');
            img.src = "<%=ResolveUrl(FuLogo_UploadFolderPath) %>" + args.get_fileName();--%>
        }
           function FuLogo_UploadError(sender, args) {
               document.getElementById('<%= FULogo_Img_Right.ClientID %>').style.display = "none";
               document.getElementById('<%= FULogo_Img_Wrong.ClientID %>').style.display = "";
               document.getElementById('<%= FULogo_Img_Right1.ClientID %>').style.display = "none";
            document.getElementById('<%= FULogo_Img_Wrong1.ClientID %>').style.display = "";
           <%-- var img = document.getElementById('<%= imgLogo.ClientID %>');
            img.src = "../Bootstrap_Files/dist/img/NoImage.jpg";--%>
            alert('Invalid File Type, Select Only .png, .jpg, .jpeg extension file');
        }
        function FuLogo_UploadStarted(sender, args) {
            var filename = args.get_fileName();

            var filext = filename.substring(filename.lastIndexOf(".") + 1);
            filext = filext.toLowerCase();
            if (filext == "png" || filext == "jpg" || filext == "jpeg" || filext =="pdf") {
                return true;
            }
            else {
                throw {
                    name: "Invalid File Type",
                    level: "Error",
                    message: "Invalid File Type, Select Only .png, .jpg, .jpeg , .pdf extension file",
                    htmlMessage: "Invalid File Type, Select Only .png, .jpg, .jpeg , .pdf extension file"
                }
                return false;
            }
        }
       </script>
</asp:Content>


