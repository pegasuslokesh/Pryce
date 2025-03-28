<%@ Page Title="" Language="C#" MasterPageFile="~/ERPMaster.master" AutoEventWireup="true" CodeFile="HalfDayRequest.aspx.cs" Inherits="Attendance_HalfDayRequest" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Assembly="DevExpress.XtraReports.v18.1.Web.WebForms, Version=18.1.3.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.XtraReports.Web" TagPrefix="dx" %>


<asp:Content ID="Content1" ContentPlaceHolderID="Head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="HeaderContent" runat="Server">
    <asp:UpdatePanel runat="server" ID="upReport">
        <ContentTemplate>
            <asp:Button runat="server" ID="btnHalfDayRequestReport" Style="display: none;" runat="server" data-toggle="modal" data-target="#HalfDayRequest" />
            <asp:HiddenField runat="server" ID="hdnCanPrint" />
            <asp:HiddenField runat="server" ID="hdnCanEdit" />
        </ContentTemplate>
    </asp:UpdatePanel>
    <h1>
        <img src="../Images/hr_halfday_leave_request.png" alt="" />
        <asp:Label ID="lblHeader" runat="server" Text="<%$ Resources:Attendance,Half Day Leave Request%>"></asp:Label>
    </h1>
    <ol class="breadcrumb">
        <li><a href="#"><i class="fa fa-dashboard"></i>
            <asp:Label ID="Label6" runat="server" Text="<%$ Resources:Attendance,Attendance Module%>"></asp:Label></a></li>
        <li><a href="#"><i class="fa fa-dashboard"></i>
            <asp:Label ID="Label10" runat="server" Text="<%$ Resources:Attendance,Attendance Module%>"></asp:Label></a></li>
        <li class="active">
            <asp:Label ID="Label5" runat="server" Text="<%$ Resources:Attendance,Half Day Leave Request%>"></asp:Label></li>
    </ol>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="Server">
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
                    <li id="Li_List"><a href="#List" data-toggle="tab">
                        <i class="fa fa-list"></i>&nbsp;&nbsp;<asp:Label ID="Label1" runat="server" Text="<%$ Resources:Attendance,List %>"></asp:Label></a></li>
                </ul>
                <div class="tab-content">
                    <div class="tab-pane" id="List">
                        <asp:UpdatePanel ID="Update_List" runat="server" UpdateMode="Conditional">
                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="btnApply" EventName="Click" />
                                <asp:AsyncPostBackTrigger ControlID="btnReset" EventName="Click" />
                                <asp:AsyncPostBackTrigger ControlID="btnCancel" EventName="Click" />
                            </Triggers>
                            <ContentTemplate>
                                <div class="box box-solid">
                                    <div class="box-body">

                                        <div class="row">
                                            <div class="col-md-12">
                                                <div id="Div1" runat="server" class="box box-info collapsed-box">
                                                    <div class="box-header with-border">
                                                        <h3 class="box-title">
                                                            <asp:Label ID="Label22" runat="server" Text="<%$ Resources:Attendance,Advance Search%>"></asp:Label></h3>
                                                        &nbsp;&nbsp;|&nbsp;&nbsp;
					 <asp:Label ID="lblTotalRecords" Font-Bold="true" runat="server" Text="<%$ Resources:Attendance,Total Records: 0 %>"></asp:Label>
                                                        <asp:Label ID="lblSelectedRecord" runat="server" Visible="false"></asp:Label>

                                                        <div class="box-tools pull-right">
                                                            <button type="button" class="btn btn-box-tool" data-widget="collapse">
                                                                <i id="I1" runat="server" class="fa fa-plus"></i>
                                                            </button>
                                                        </div>
                                                    </div>
                                                    <div class="box-body">



                                                        <div class="col-lg-6">
                                                            <asp:Label runat="server" ID="Label11" Text="Location"></asp:Label>
                                                            <asp:DropDownList runat="server" ID="ddlLoc" OnSelectedIndexChanged="ddlLoc_SelectedIndexChanged" AutoPostBack="true" CssClass="form-control"></asp:DropDownList>
                                                            <br />
                                                        </div>

                                                        <div class="col-lg-6">
                                                            <asp:Label runat="server" ID="lblYear" Text="Year"></asp:Label>
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
                                                                <asp:ListItem Text="<%$ Resources:Attendance,HalfDay Date%>" Value="HalfDay_Date"></asp:ListItem>

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
                                            <div class="col-md-12">
                                                <div class="flow">
                                                    <asp:HiddenField runat="server" ID="hdnReportId" />
                                                    <asp:HiddenField runat="server" ID="hdnPrintTransId" />
                                                    <asp:HiddenField runat="server" ID="hdnEmpidVal" />
                                                    <asp:HiddenField runat="server" ID="hdnLocationId" />
                                                    <asp:GridView CssClass="table-striped table-bordered table table-hover" ID="gvLeaveStatus" PageSize="<%# PageControlCommon.GetPageSize() %>"
                                                        runat="server" AutoGenerateColumns="False" Width="100%" OnPageIndexChanging="gvLeaveStatus_PageIndexChanging"
                                                        AllowPaging="true">
                                                        <Columns>
                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,Action%>">
                                                                <ItemTemplate>
                                                                    <div class="dropdown" style="position: absolute;">
                                                                        <button class="btn btn-default dropdown-toggle" type="button" data-toggle="dropdown">
                                                                            <i class="fa fa-ellipsis-h" aria-hidden="true"></i>
                                                                        </button>
                                                                        <ul class="dropdown-menu">
                                                                            <li >
                                                                                <asp:LinkButton ID="btnPrint" runat="server" CommandName="75" CommandArgument='<%# Eval("Trans_Id") %>' OnCommand="btnPrint_Command" CausesValidation="False"><i class="fa fa-print"></i><asp:Label ID="lbledit" runat="server" Text = "<%$ Resources:Attendance,Print %>"></asp:Label></asp:LinkButton>
                                                                            </li>
                                                                            <%--     <li <%= hdnCanEdit.Value=="true"?"style='display:block'":"style='display:none'" %>>
                                                                                <asp:LinkButton ID="btnEdit" runat="server" CommandArgument='<%# Eval("Trans_Id") %>' OnCommand="btnEdit_Command" CausesValidation="False"><i class="fa fa-pencil"></i>Edit </asp:LinkButton>
                                                                            </li>--%>
                                                                        </ul>
                                                                    </div>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>

                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,Employee Id  %>">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblEmpId" runat="server" Text='<%# Eval("Emp_Code") %>'></asp:Label>
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
                                                                    <asp:Label ID="lblScheduleType" runat="server" Text='<%# Eval("HalfDay_Type") %>'></asp:Label>
                                                                </ItemTemplate>
                                                                <ItemStyle />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,Apply Date %>">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblApplicatonDate" runat="server" Text='<%# GetDate(Eval("HalfDay_Date")) %>'></asp:Label>
                                                                </ItemTemplate>
                                                                <ItemStyle />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,Status %>">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblStatus" runat="server" Text='<%# Eval("Is_Confirmed")%>'></asp:Label>
                                                                    <asp:Label ID="lblLocationId" runat="server" Text='<%# Eval("Location_Id")%>' Visible="false"></asp:Label>
                                                                    <asp:Label ID="lblEmpId1" runat="server" Text='<%# Eval("Emp_Id")%>' Visible="false"></asp:Label>
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
                                                                            <asp:Label ID="lblFileFullPathList" runat="server" Visible="false" Text='<%#Eval("Field5") %>' />
                                                                            <asp:Label ID="lblFileDownloadList" runat="server" Visible="false" Text='<%#Eval("Field4") %>' />
                                                                            <asp:LinkButton ID="ImgDownloadList" runat="server" CommandArgument='<%#Eval("Trans_id") %>' OnCommand="OnDownloadDocumentCommandList" ToolTip="<%$ Resources:Attendance,Download %>"><i class="fa fa-download"></i></asp:LinkButton>
                                                                        </ContentTemplate>
                                                                    </asp:UpdatePanel>
                                                                </ItemTemplate>
                                                                <HeaderStyle HorizontalAlign="Center" />
                                                                <ItemStyle HorizontalAlign="Center" Width="60px" />
                                                            </asp:TemplateField>
                                                             <asp:TemplateField ItemStyle-HorizontalAlign="Center">
                                                                 <ItemTemplate>
                                                                       <asp:Button ID="ChoosefileUpload" runat="server" OnClientClick='<%# string.Format("javascript:return OpenModel({0})", Eval("Trans_id"))%>' Text="Upload"/>                                                                          
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
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </div>
                    <div class="tab-pane active" id="New">
                        <asp:UpdatePanel ID="Update_New" runat="server" UpdateMode="Conditional">
                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="gvLeaveStatus" />
                            </Triggers>
                            <ContentTemplate>
                                <div class="row">
                                    <div class="col-md-12">
                                        <div class="box box-primary">
                                            <div class="box-body">
                                                <div class="form-group">

                                                    <div class="col-md-6">
                                                        <asp:HiddenField ID="hdnEmpId" runat="server" />
                                                        <asp:HiddenField ID="hdnEdit" runat="server" />
                                                        <asp:Label ID="Label3" runat="server" Text="<%$ Resources:Attendance,Employee Name %>"></asp:Label>
                                                        <asp:TextBox ID="txtEmpName" runat="server" CssClass="form-control"
                                                            BackColor="#eeeeee" AutoPostBack="true" OnTextChanged="txtEmpName_textChanged" />

                                                        <cc1:AutoCompleteExtender ID="AutoCompleteExtender5" runat="server" DelimiterCharacters=""
                                                            Enabled="True" ServiceMethod="GetCompletionListEmployeeName" ServicePath="" CompletionInterval="100"
                                                            MinimumPrefixLength="1" CompletionSetCount="1" TargetControlID="txtEmpName" UseContextKey="True"
                                                            CompletionListCssClass="autoCompleteList" CompletionListItemCssClass="autoCompleteListItem" CompletionListHighlightedItemCssClass="autoCompleteSelectedListItem" OnClientShown="resetPosition">
                                                        </cc1:AutoCompleteExtender>
                                                        <br />
                                                    </div>
                                                    <div class="col-md-6">
                                                        <asp:Label ID="Label2" runat="server" Text="<%$ Resources:Attendance,Apply Date %>"></asp:Label>
                                                        <asp:TextBox ID="txtApplyDate" runat="server" CssClass="form-control" />

                                                        <cc1:CalendarExtender OnClientShown="showCalendar" ID="CalendarExtender2" runat="server" Enabled="True" TargetControlID="txtApplyDate">
                                                        </cc1:CalendarExtender>
                                                        <br />
                                                    </div>
                                                    <div class="col-md-6">
                                                        <asp:Label ID="Label4" runat="server" Text="<%$ Resources:Attendance,Half Day Type %>"></asp:Label>
                                                        <asp:RadioButton Style="margin-left: 10px;" ID="rbtnHalfDayMorning" runat="server"
                                                            GroupName="HalfDay" Text="<%$ Resources:Attendance,Morning%>" AutoPostBack="true"
                                                            OnCheckedChanged="rbtnHalfDayMorning_OnCheckedChanged" />
                                                        <asp:RadioButton Style="margin-left: 10px;" ID="rbtnHalfDayEvening" runat="server"
                                                            GroupName="HalfDay" Text="<%$ Resources:Attendance,Evening %>" AutoPostBack="true"
                                                            OnCheckedChanged="rbtnHalfDayMorning_OnCheckedChanged" />
                                                        <br />
                                                    </div>
                                                    <div id="panel_inout_time" runat="server" visible="false" class="col-md-12">
                                                        <br />
                                                        <div class="col-md-3">
                                                            <asp:Label ID="Label7" runat="server" Text="<%$ Resources:Attendance,From Time %>"></asp:Label>
                                                            <asp:TextBox ID="txtInTime" runat="server" Enabled="false" CssClass="form-control" />
                                                            <br />
                                                        </div>
                                                        <div class="col-md-3">
                                                            <asp:Label ID="Label8" runat="server" Text="<%$ Resources:Attendance,To Time %>"></asp:Label>
                                                            <asp:TextBox ID="txtOuttime" Enabled="false" runat="server" CssClass="form-control" />

                                                        </div>
                                                    </div>

                                                    <div class="col-md-12">
                                                        <br />
                                                        <asp:HiddenField ID="HiddenField1" runat="server" />
                                                        <asp:Label ID="Label9" runat="server" Text="<%$ Resources:Attendance,Description %>"></asp:Label>
                                                        <asp:TextBox ID="txtDescription" TextMode="MultiLine"
                                                            runat="server" CssClass="form-control" />
                                                        <br />
                                                    </div>
                                                    <div class="col-md-12">
                                                            <%-- <asp:Label ID="lblEmpLogo" runat="server" Text="Leave Attachment"></asp:Label>--%>
                                                         <div class="input-group" style="width: 100%;">
                                                                <cc1:AsyncFileUpload ID="FULogoPath1" OnClientUploadStarted="FuLogo_UploadStarted"  OnClientUploadError="FuLogo_UploadError"   OnClientUploadComplete="FuLogo_UploadComplete" OnUploadedComplete="FuLogo_FileUploadComplete" runat="server" CssClass="form-control" CompleteBackColor="White" UploaderStyle="Traditional" UploadingBackColor="#CCFFFF" ThrobberID="FULogo_ImgLoader1" Width="100%" />
                                                                <div class="input-group-btn" style="border: solid 1px #d2d6de; width: 35px;">
                                                                    <asp:LinkButton ID="FULogo_Img_Right1" runat="server" Width="30px" Height="30px" Style="display: none"><i class="fa fa-check" style="font-size:30px;color:#22cb33"></i></asp:LinkButton>
                                                                    <asp:LinkButton ID="FULogo_Img_Wrong1" runat="server" Width="30px" Height="30px" Style="display: none"><i class="fa fa-remove" style="font-size:30px"></i></asp:LinkButton>
                                                                    <asp:Image ID="FULogo_ImgLoader1" runat="server" ImageUrl="../Images/loader.gif" />
                                                                </div>
                                                            </div>
                                                            <br/>
                                                     </div>
                                                    <div class="col-md-12" style="text-align: center">
                                                        <br />
                                                        <asp:Button ID="btnApply" runat="server" Text="<%$ Resources:Attendance,Apply %>"
                                                            Visible="false" CssClass="btn btn-primary" OnClick="btnApply_Click" />
                                                        &nbsp;&nbsp;
                                                    <asp:Button ID="btnReset" runat="server" Text="<%$ Resources:Attendance,Reset %>"
                                                        Visible="true" CssClass="btn btn-primary" OnClick="btnReset_Click" />
                                                        &nbsp;&nbsp;
                                                    <asp:Button ID="btnCancel" runat="server" Text="<%$ Resources:Attendance,Cancel %>"
                                                        Visible="true" CssClass="btn btn-primary" OnClick="btnCancel_Click" />
                                                        <br />
                                                    </div>
                                                    <div class="col-md-12">
                                                        <div class="col-md-12" style="overflow: auto">
                                                            <br />
                                                            <asp:GridView CssClass="table-striped table-bordered table table-hover" ID="gvLeaveSummary" PageSize="<%# PageControlCommon.GetPageSize() %>"
                                                                runat="server" AutoGenerateColumns="False" Width="100%">
                                                                <Columns>
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
                                                                    <asp:TemplateField HeaderText="<%$ Resources:Attendance,Year %>">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblMonthName" runat="server" Text='<%# Eval("Year") %>'></asp:Label>
                                                                        </ItemTemplate>
                                                                        <ItemStyle />
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="<%$ Resources:Attendance,Total Leave %>">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblTotalDays" runat="server" Text='<%# Eval("Total_Days") %>'></asp:Label>
                                                                        </ItemTemplate>
                                                                        <ItemStyle />
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="<%$ Resources:Attendance,Used Leave %>">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblUsedDays" runat="server" Text='<%# Eval("Used_Days") %>'></asp:Label>
                                                                        </ItemTemplate>
                                                                        <ItemStyle />
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="<%$ Resources:Attendance,Remaining Leave %>">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblRemainingDays" runat="server" Text='<%# Eval("Remaining_Days") %>'></asp:Label>
                                                                        </ItemTemplate>
                                                                        <ItemStyle />
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Pending Leave">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblPendingDays" runat="server" Text='<%# Eval("Pending_Days") %>'></asp:Label>
                                                                        </ItemTemplate>
                                                                        <ItemStyle />
                                                                    </asp:TemplateField>
                                                                </Columns>
                                                                <PagerStyle CssClass="pagination-ys" />
                                                            </asp:GridView>

                                                        </div>
                                                        <div class="col-md-12" style="overflow: auto">
                                                            <br />
                                                            <asp:GridView CssClass="table-striped table-bordered table table-hover" ID="gvEmpPendingLeave" PageSize="<%# PageControlCommon.GetPageSize() %>"
                                                                runat="server" AutoGenerateColumns="False" Width="100%" OnPageIndexChanging="gvEmpPendingLeave_PageIndexChanging"
                                                                AllowPaging="true">
                                                                <Columns>
                                                                    <asp:TemplateField HeaderText="<%$ Resources:Attendance,Employee Id  %>">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblEmpId" runat="server" Text='<%# Eval("Emp_Code") %>'></asp:Label>
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
                                                                            <asp:Label ID="lblScheduleType" runat="server" Text='<%# Eval("HalfDay_Type") %>'></asp:Label>
                                                                        </ItemTemplate>
                                                                        <ItemStyle />
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="<%$ Resources:Attendance,Apply Date %>">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblApplicatonDate" runat="server" Text='<%# GetDate(Eval("HalfDay_Date")) %>'></asp:Label>
                                                                        </ItemTemplate>
                                                                        <ItemStyle />
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="<%$ Resources:Attendance,Status %>">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblStatus" runat="server" Text='<%# Eval("Is_Confirmed")%>'></asp:Label>
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
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </div>
                </div>
            </div>
        </div>
    </div>

    <div class="modal fade" id="HalfDayRequest" tabindex="-1" role="dialog" aria-labelledby="myModalLabel" data-backdrop="static" aria-hidden="true">
        <div class="modal-dialog modal-lg">
            <div class="modal-content">
                <div class="modal-body">
                    <asp:UpdatePanel ID="uptaskContrcat" runat="server">
                        <ContentTemplate>
                            <dx:ASPxWebDocumentViewer ID="ReportViewer1" runat="server" Width="100%"></dx:ASPxWebDocumentViewer>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
                <div class="modal-footer">
                    <button type="button" id="" class="btn btn-danger" data-dismiss="modal" onclick="btnCloseReport()">
                        Close</button>
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


        function btnCloseReport() {
            document.getElementById('<%= hdnReportId.ClientID%>').value = "";
        }

        function openReport() {
            document.getElementById('<%= btnHalfDayRequestReport.ClientID%>').click();
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
            if (filext == "png" || filext == "jpg" || filext == "jpeg" || filext == "pdf") {
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
           function OpenModel(transId) {
  debugger;
            //$('#< %=hidtransId.ClientID% >').val(transId);
 $('#<% =hidtransId.ClientID %>').attr('value', transId);
            //document.getElementById('< %=hidtransId.ClientID%>').val = transId;
 $('#myModal').modal('show');
return true;
        }
    </script>
</asp:Content>


