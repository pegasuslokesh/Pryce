<%@ Page Title="" Language="C#" MasterPageFile="~/ERPMaster.master" AutoEventWireup="true" CodeFile="ProjectScopeStatement.aspx.cs" Inherits="ProjectManagement_Report_ProjectScopeStatement" %>

<%@ Register Assembly="DevExpress.Web.ASPxTreeList.v18.1, Version=18.1.3.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web.ASPxTreeList" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.v18.1, Version=18.1.3.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Assembly="DevExpress.XtraReports.v18.1.Web.WebForms, Version=18.1.3.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.XtraReports.Web" TagPrefix="dx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="HeaderContent" runat="Server">
    <h1>
        <img src="../Images/product_category.png" alt="" />
        <asp:Label ID="lblHeader" runat="server" Text="<%$ Resources:Attendance,Project Scope Statement%>"></asp:Label>
    </h1>
    <ol class="breadcrumb">
        <li><a href="#"><i class="fa fa-dashboard"></i>
            <asp:Label ID="Label6" runat="server" Text="<%$ Resources:Attendance,Project Management%>"></asp:Label></a></li>
        <li><a href="#"><i class="fa fa-dashboard"></i>
            <asp:Label ID="Label10" runat="server" Text="<%$ Resources:Attendance,Project Report%>"></asp:Label></a></li>
        <li class="active">
            <asp:Label ID="Label5" runat="server" Text="<%$ Resources:Attendance,Project Scope Statement%>"></asp:Label></li>
    </ol>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="Server">

    <asp:UpdatePanel ID="Update_New" runat="server">
        <ContentTemplate>
            
            <div id="pnlProjectfilter" runat="server" class="row">
                <div class="col-md-12">
                    <div class="box box-primary">
                        <div class="box-body">
                            <div class="form-group">
                                <asp:HiddenField ID="hdnSelectedTask" runat="server" />
                                <div class="col-md-6">
                                    <asp:Label ID="lblProjectType" runat="server" Text="<%$ Resources:Attendance,Project Type %>"></asp:Label>
                                    <asp:TextBox ID="txtprojecttype" runat="server" CssClass="form-control" OnTextChanged="txtprojecttype_TextChanged"
                                        BackColor="#eeeeee" AutoPostBack="true"></asp:TextBox>
                                    <cc1:AutoCompleteExtender ID="AutoCompleteExtender1" runat="server" DelimiterCharacters=""
                                        Enabled="True" ServiceMethod="GetProjectType" ServicePath="" CompletionInterval="100"
                                        MinimumPrefixLength="1" CompletionSetCount="1" TargetControlID="txtprojecttype"
                                        UseContextKey="True" CompletionListCssClass="autoCompleteList" CompletionListItemCssClass="autoCompleteListItem" CompletionListHighlightedItemCssClass="autoCompleteSelectedListItem" OnClientShown="resetPosition">
                                    </cc1:AutoCompleteExtender>
                                    <br />
                                </div>
                                <div class="col-md-6">
                                    <asp:Label ID="Label21" runat="server" Text="Nature Of Task"></asp:Label>
                                    <asp:DropDownList ID="ddlTaskType" runat="server" CssClass="form-control">
                                        <asp:ListItem Text="Both" Value="Both"></asp:ListItem>
                                        <asp:ListItem Text="Internal" Value="Internal"></asp:ListItem>
                                        <asp:ListItem Text="External" Value="External"></asp:ListItem>
                                    </asp:DropDownList>
                                    <br />
                                </div>
                                <div class="col-md-12">
                                    <asp:Label ID="Label13" runat="server" Text="<%$ Resources:Attendance,Project Name %>"></asp:Label>
                                    <dx:ASPxComboBox ID="ddlprojectname" runat="server" CssClass="form-control" DropDownWidth="550"
                                        DropDownStyle="DropDownList" ValueField="Project_Id" OnSelectedIndexChanged="ddlprojectname_SelectedIndexChanged" AutoPostBack="true"
                                        ValueType="System.String" TextFormatString="{0}" EnableCallbackMode="true" IncrementalFilteringMode="Contains"
                                        CallbackPageSize="30">
                                        <Columns>
                                            <dx:ListBoxColumn FieldName="Project_Name" />

                                        </Columns>
                                    </dx:ASPxComboBox>
                                    <br />
                                </div>
                                <div class="col-md-12">
                                    <div class="col-md-2">
                                    </div>
                                    <div class="col-md-3">
                                        <asp:ListBox ID="lstLocation" runat="server" Style="width: 100%;" Height="200px"
                                            SelectionMode="Multiple" CssClass="form-control" Font-Names="Trebuchet MS" Font-Size="Small"
                                            ForeColor="Gray"></asp:ListBox>
                                    </div>
                                    <div class="col-lg-2" style="text-align: center">
                                        <div style="margin-top: 55px; margin-bottom: 40px;" class="btn-group-vertical">

                                            <asp:Button ID="btnPushDept" runat="server" CssClass="btn btn-info" Text=">" OnClick="btnPushDept_Click" />

                                            <asp:Button ID="btnPullDept" Text="<" runat="server" CssClass="btn btn-info" OnClick="btnPullDept_Click" />

                                            <asp:Button ID="btnPushAllDept" Text=">>" OnClick="btnPushAllDept_Click" runat="server" CssClass="btn btn-info" />

                                            <asp:Button ID="btnPullAllDept" Text="<<" OnClick="btnPullAllDept_Click" runat="server" CssClass="btn btn-info" />
                                        </div>
                                    </div>
                                    <div class="col-md-3">
                                        <asp:ListBox ID="lstLocationSelect" runat="server" Style="width: 100%;" Height="200px"
                                            SelectionMode="Multiple" CssClass="form-control" Font-Names="Trebuchet MS" Font-Size="Small"
                                            ForeColor="Gray"></asp:ListBox>
                                    </div>
                                    <div class="col-md-2">
                                    </div>
                                    <br />
                                </div>

                                <div class="col-md-4">
                                    <asp:Label runat="server" ID="lblFromDt" Text="From Date"></asp:Label>
                                    <asp:TextBox runat="server" ID="txtFromDate" CssClass="form-control" onchange="checkDateDiff();"></asp:TextBox>
                                    <cc1:CalendarExtender OnClientShown="showCalendar" ID="CalendarExtendertxtValueDate" runat="server" TargetControlID="txtFromDate" Format="dd-MMM-yyyy" />
                                </div>
                                <div class="col-md-4">
                                    <asp:Label runat="server" ID="lblToDt" Text="To Date"></asp:Label>
                                    <asp:TextBox runat="server" ID="txtToDate" CssClass="form-control" onchange="checkDateDiff();"></asp:TextBox>
                                    <cc1:CalendarExtender OnClientShown="showCalendar" ID="CalendarExtender1" runat="server" TargetControlID="txtToDate" Format="dd-MMM-yyyy" />
                                </div>
                                <div class="col-md-4">
                                    <asp:Label runat="server" ID="lblStatus" Text="Status"></asp:Label>
                                    <asp:DropDownList ID="ddlStatus" runat="server" CssClass="form-control">
                                        <asp:ListItem Text="All" Value="All"></asp:ListItem>
                                        <asp:ListItem Text="Assigned" Value="Assigned"></asp:ListItem>
                                        <asp:ListItem Text="ReAssigned" Value="ReAssigned"></asp:ListItem>
                                        <asp:ListItem Text="Closed" Value="Closed"></asp:ListItem>
                                        <asp:ListItem Text="Cancelled" Value="Cancelled"></asp:ListItem>
                                    </asp:DropDownList>
                                </div>

                                <div class="col-md-12" id="trassignee" runat="server">
                                    <br />
                                    <asp:Label ID="Label16" runat="server" Text="Assing To"></asp:Label>
                                    <asp:Panel ID="pnlBrand" runat="server" Style="max-height: 500px;" Width="100%" BorderStyle="Solid"
                                        BorderWidth="1px" BorderColor="#eeeeee" BackColor="White" ScrollBars="Auto">
                                        <asp:CheckBoxList ID="listtaskEmployee" runat="server" RepeatColumns="3" CellPadding="5"
                                            CellSpacing="10" Font-Names="Trebuchet MS" Font-Size="Small" ForeColor="Gray" />
                                    </asp:Panel>
                                    <br />
                                </div>

                                <div class="col-md-12">
                                    <br />
                                </div>
                                <div class="col-md-12" style="text-align: center">
                                    <asp:Button ID="btngetRecord" runat="server" CssClass="btn btn-primary" OnClick="btngetRecord_Click"
                                        Text="Get Record" />
                                    <asp:Button ID="btnreset" runat="server" CssClass="btn btn-primary" OnClick="btnreset_Click"
                                        Text="<%$ Resources:Attendance,Reset%>" />
                                    <br />
                                </div>
                                <div class="col-md-6">
                                    <asp:CheckBox ID="ChkselectAll" runat="server" Text="Select All" Visible="false" AutoPostBack="true" OnCheckedChanged="ChkselectAll_OnCheckedChanged" />
                                    <br />
                                </div>
                                <div class="col-md-12" style="overflow: auto; max-height: 500px;">
                                    <asp:GridView CssClass="table-striped table-bordered table table-hover" ID="GvrProjecttask" runat="server" AutoGenerateColumns="False" Width="100%"
                                        AllowPaging="True" OnPageIndexChanging="GvrProjecttask_PageIndexChanging" AllowSorting="True"
                                         OnSorting="GvrProjecttask_Sorting">
                                        

                                        <Columns>
                                            <%--   <asp:TemplateField>
                                                                <ItemTemplate>
                                                                    <asp:ImageButton ID="imgBtnaddtax" runat="server" ImageUrl="~/Images/plus.png" Width="30px"
                                                                        Height="30px" OnCommand="imgViewDetail_Command" ToolTip="View Product Detail" />
                                                                </ItemTemplate>
                                                                <ItemStyle  HorizontalAlign="Center" />
                                                            </asp:TemplateField>--%>
                                            <asp:TemplateField>
                                                <ItemTemplate>
                                                    <asp:CheckBox ID="chkgvSelect" runat="server" AutoPostBack="true" OnCheckedChanged="chkgvSelect_CheckedChanged" />
                                                    <asp:Label ID="lblProjectId" Visible="false" runat="server" Text='<%# Eval("Task_Id") %>'></asp:Label>
                                                </ItemTemplate>
                                                <HeaderTemplate>
                                                    <asp:CheckBox ID="chkgvSelectAll" runat="server" OnCheckedChanged="chkgvSelectAll_CheckedChanged"
                                                        AutoPostBack="true" />

                                                </HeaderTemplate>
                                                <ItemStyle HorizontalAlign="Left" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Nature Of Task" SortExpression="Task_Type">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblnatureofTask" runat="server" Text='<%# Eval("Task_Type") %>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle  />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Task" SortExpression="Subject">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblEmpnameList1" runat="server" Text='<%# Eval("Subject") %>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle  />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,Assign By %>" SortExpression="Emp_Name">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblprojectIdList12" runat="server" Text='<%# GetAssignBy(Eval("CreatedBy").ToString()) %>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle  />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,Assign Date %>" SortExpression="Assign_Date">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblEmpnameList2" runat="server" Text='<%# Formatdate(Eval("Assign_Date")) %>'></asp:Label>
                                                    <asp:Label ID="lblEmpnameList3" runat="server" Text='<%# FormatTime(Eval("Assign_Time")) %>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle  />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Expected Date" SortExpression="Emp_Close_Date">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblEmpnameList121" runat="server" Text='<%# Formatdate(Eval("Emp_Close_Date")) %>'></asp:Label>
                                                    <asp:Label ID="lblEmpnameList131" runat="server" Text='<%# FormatTime(Eval("Emp_Close_Time")) %>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle  />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,Status %>" SortExpression="Status">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblEmpnameList4" runat="server" Text='<%# Eval("Status") %>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle  />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,Priority %>" SortExpression="Field4">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblpriority" runat="server" Text='<%# Eval("Field4") %>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle  />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Completed %" SortExpression="Field5">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblcompletion" runat="server" Text='<%# Eval("Field5") %>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle  />
                                            </asp:TemplateField>


                                            <asp:TemplateField HeaderText="Task Type" SortExpression="TaskType_Name">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblBugs" runat="server" Text='<%# Eval("TaskType_Name") %>'></asp:Label>


                                                    <%--    <asp:Literal runat="server" ID="lit1" Text="<tr id='trGrid'><td colspan='10' align='Left'>" />
                                                                    <asp:GridView CssClass="table-striped table-bordered table table-hover" ID="gvchildGrid" runat="server" AutoGenerateColumns="false" Visible="false" DataKeyNames="Task_Id"
                                                                        ShowHeader="true" Width="100%" >
                                                                        <Columns>
                                                                            <asp:TemplateField>
                                                                                <ItemTemplate>
                                                                                    <asp:CheckBox ID="chkgvSelect" runat="server"  AutoPostBack="true" OnCheckedChanged="chkselecttask_OnCheckedChanged" />
                                                                                    <asp:Label ID="lblProjectId" Visible="false" runat="server" Text='<%# Eval("Task_Id") %>'></asp:Label>
                                                                                </ItemTemplate>
                                                                                <ItemStyle HorizontalAlign="Left"  />
                                                                            </asp:TemplateField>
                                                                            <asp:TemplateField HeaderText="Sub Task">
                                                                                <ItemTemplate>
                                                                                    <asp:Label ID="lblgvcategoryName" runat="server" Width="200px" Visible="true" Text='<%#Eval("Subject") %>' />
                                                                                </ItemTemplate>
                                                                                <ItemStyle HorizontalAlign="Left" />
                                                                            </asp:TemplateField>

                                                                             
                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,Assign By %>" SortExpression="Emp_Name">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblprojectIdList12" runat="server" Text='<%# GetAssignBy(Eval("CreatedBy").ToString()) %>'></asp:Label>
                                                                </ItemTemplate>
                                                                <ItemStyle  />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,Assign Date %>" SortExpression="Assign_Date">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblEmpnameList2" runat="server" Text='<%# Formatdate(Eval("Assign_Date")) %>'></asp:Label>
                                                                    <asp:Label ID="lblEmpnameList3" runat="server" Text='<%# FormatTime(Eval("Assign_Time")) %>'></asp:Label>
                                                                </ItemTemplate>
                                                                <ItemStyle  />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Expected Date" SortExpression="Emp_Close_Date">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblEmpnameList121" runat="server" Text='<%# Formatdate(Eval("Emp_Close_Date")) %>'></asp:Label>
                                                                    <asp:Label ID="lblEmpnameList131" runat="server" Text='<%# FormatTime(Eval("Emp_Close_Time")) %>'></asp:Label>
                                                                </ItemTemplate>
                                                                <ItemStyle  />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,Status %>" SortExpression="Status">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblEmpnameList4" runat="server" Text='<%# Eval("Status") %>'></asp:Label>
                                                                </ItemTemplate>
                                                                <ItemStyle  />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,Priority %>" SortExpression="Field4">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblpriority" runat="server" Text='<%# Eval("Field4") %>'></asp:Label>
                                                                </ItemTemplate>
                                                                <ItemStyle  />
                                                            </asp:TemplateField>

                                                            <asp:TemplateField HeaderText="Completed %" SortExpression="Field5">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblcompletion" runat="server" Text='<%# Eval("Field5") %>'></asp:Label>
                                                                </ItemTemplate>
                                                                <ItemStyle  />
                                                            </asp:TemplateField>
                                                              <asp:TemplateField HeaderText="Is Bug" SortExpression="IsBugs">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblBugs" runat="server" Text='<%# Eval("IsBugs") %>'></asp:Label>
                                                                    </ItemTemplate>
                                                                <ItemStyle  />
                                                            </asp:TemplateField>
                                                                        </Columns>
                                                                        
                                                                        <PagerStyle CssClass="pagination-ys" />
                                                                        
                                                                    </asp:GridView>
                                                                    <asp:Literal runat="server" ID="lit2" Text="</td></tr>" />--%>
                                                </ItemTemplate>
                                                <ItemStyle  />
                                            </asp:TemplateField>
                                        </Columns>
                                        
                                        <PagerStyle CssClass="pagination-ys" />
                                        
                                    </asp:GridView>
                                    <br />
                                </div>
                                <div class="col-md-12">
                                    <table class="OptionsTable OptionsBottomMargin" style="width: 100%">
                                        <tr>
                                            <td>
                                                <dx:ASPxCheckBox ID="chkRecursive" Visible="false" runat="server" Text="Recursive" AutoPostBack="true"
                                                    Wrap="False" />
                                            </td>
                                            <td>
                                                <dx:ASPxCheckBox ID="chkAllowAll" runat="server" Visible="false" Text="Show 'Select All' check box"
                                                    AutoPostBack="true" Wrap="False" />
                                            </td>
                                            <td style="width: 100%"></td>
                                            <td>
                                                <dx:ASPxComboBox ID="cmbMode" runat="server" Visible="false" AutoPostBack="true" SelectedIndex="0"
                                                    OnSelectedIndexChanged="cmbMode_SelectedIndexChanged" Caption="Allow select">
                                                    <Items>
                                                        <dx:ListEditItem Value="All nodes" />
                                                        <dx:ListEditItem Value="Child nodes" />
                                                        <dx:ListEditItem Value="Parent nodes" />
                                                        <dx:ListEditItem Value="Level > 2" />
                                                    </Items>
                                                </dx:ASPxComboBox>
                                            </td>
                                        </tr>
                                    </table>
                                    <br />
                                </div>
                                <div class="col-md-12">
                                    <table class="OptionsTable">
                                        <tr>
                                            <td>
                                                <dx:ASPxLabel ID="ASPxLabel1" runat="server" Text="Selected node count:" />
                                            </td>
                                            <td id="treeListCountCell">
                                                <asp:Literal ID="countLiteral" runat="server" />
                                            </td>
                                        </tr>
                                    </table>
                                    <dx:ASPxTreeList ID="treeList" runat="server" AutoGenerateColumns="False" EnableCallbacks="true"
                                        Width="100%"
                                        KeyFieldName="Task_Id" ParentFieldName="ParentTaskId" OnCustomDataCallback="treeList_CustomDataCallback"
                                        OnDataBound="treeList_DataBound">
                                        <SettingsSelection Enabled="true" AllowSelectAll="true" />
                                        <SettingsBehavior AllowDragDrop="true" />



                                        <Columns>




                                            <dx:TreeListDataColumn FieldName="Task_Type" Caption="Task Nature" VisibleIndex="2" Width="50px" />
                                            <dx:TreeListDataColumn FieldName="Subject" VisibleIndex="0" CellStyle-Wrap="True" Width="200px" />


                                            <dx:TreeListDataColumn FieldName="AssignDate1" Caption="Assign Date & Time" VisibleIndex="3" Width="50px" />

                                            <dx:TreeListDataColumn FieldName="Status" Caption="Status" VisibleIndex="4" Width="50px" />

                                            <dx:TreeListDataColumn FieldName="TaskType_Name" Caption="Task Type" VisibleIndex="5" Width="50px" />

                                            <%--<dx:TreeListDataColumn FieldName="Task_id" Visible="False" VisibleIndex="5">
                <CellStyle BackColor="#ffebb1" />
            </dx:TreeListDataColumn>
            <dx:TreeListDataColumn FieldName="ParentTaskId" Visible="False" VisibleIndex="6" >
                <CellStyle BackColor="#ffebb1" />
            </dx:TreeListDataColumn>--%>
                                        </Columns>
                                        <SettingsBehavior ExpandCollapseAction="NodeDblClick" />

                                    </dx:ASPxTreeList>
                                    <asp:Label ID="lblSelectRecord" runat="server" Visible="false"></asp:Label>
                                    <br />
                                </div>
                                <div id="trreportAction" runat="server" visible="false" class="col-md-12" style="text-align: center">
                                    <br />

                                    <asp:Button ID="btnprojectStatement" runat="server" CssClass="btn btn-primary" OnClick="btnsave_Click"
                                        Text="Project Statement" />
                                    <asp:Button ID="btnPrtTask" runat="server" CssClass="btn btn-primary" OnClick="btnsave_Click"
                                        Text="Print Task Contract" />

                                    <asp:Button ID="btnProjectProgrss" runat="server" CssClass="btn btn-primary" OnClick="btnsave_Click"
                                        Text="Task Check List" />
                                    <br />
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
            <div id="pnlReport" runat="server" class="row">
                <div class="col-md-12">
                    <div class="box box-primary">
                        <div class="box-body">
                            <div class="form-group">
                                <div class="col-md-6">
                                    <asp:LinkButton ID="lnkback" runat="server" CssClass="acc" OnClick="lnkback_Click"
                                        Text="<%$ Resources:Attendance,Back %>"></asp:LinkButton>
                                    <br />
                                </div>
                                <div class="col-md-12" style="overflow: auto; max-height: 500px;">
                                    <dx:ReportToolbar ID="rptToolBar" runat="server" ShowDefaultButtons="False" ReportViewer="<%# rptViewer %>"
                                        Width="100%" AccessibilityCompliant="True">
                                        <Items>
                                            <dx:ReportToolbarButton ItemKind="Search" />
                                            <dx:ReportToolbarSeparator />
                                            <dx:ReportToolbarButton ItemKind="PrintReport" />
                                            <dx:ReportToolbarButton ItemKind="PrintPage" />
                                            <dx:ReportToolbarSeparator />
                                            <dx:ReportToolbarButton Enabled="False" ItemKind="FirstPage" />
                                            <dx:ReportToolbarButton Enabled="False" ItemKind="PreviousPage" />
                                            <dx:ReportToolbarLabel ItemKind="PageLabel" />
                                            <dx:ReportToolbarComboBox ItemKind="PageNumber" Width="65px">
                                            </dx:ReportToolbarComboBox>
                                            <dx:ReportToolbarLabel ItemKind="OfLabel" />
                                            <dx:ReportToolbarTextBox IsReadOnly="True" ItemKind="PageCount" />
                                            <dx:ReportToolbarButton ItemKind="NextPage" />
                                            <dx:ReportToolbarButton ItemKind="LastPage" />
                                            <dx:ReportToolbarSeparator />
                                            <dx:ReportToolbarButton ItemKind="SaveToDisk" />
                                            <dx:ReportToolbarButton ItemKind="SaveToWindow" />
                                            <dx:ReportToolbarComboBox ItemKind="SaveFormat" Width="70px">
                                                <Elements>
                                                    <dx:ListElement Value="pdf" />
                                                    <dx:ListElement Value="xls" />
                                                    <dx:ListElement Value="xlsx" />
                                                    <dx:ListElement Value="rtf" />
                                                    <dx:ListElement Value="mht" />
                                                    <dx:ListElement Value="html" />
                                                    <dx:ListElement Value="txt" />
                                                    <dx:ListElement Value="csv" />
                                                    <dx:ListElement Value="png" />
                                                </Elements>
                                            </dx:ReportToolbarComboBox>
                                        </Items>
                                        <Styles>
                                            <LabelStyle>
                                                <Margins MarginLeft="3px" MarginRight="3px" />
                                            </LabelStyle>
                                        </Styles>
                                    </dx:ReportToolbar>
                                    <br />
                                </div>
                                <div class="col-md-12">
                                    <asp:Panel ID="pnlrptviewer" runat="server" Width="100%" Height="100%">
                                        <dx:ReportViewer ID="rptViewer" runat="server" AutoSize="false" Width="100%" Height="500px">
                                        </dx:ReportViewer>
                                    </asp:Panel>
                                    <br />
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>



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
<asp:Content ID="Content5" ContentPlaceHolderID="AfterLoadScriptContent" runat="Server"><script src="../Script/common.js"></script>
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

        function checkDateDiff() {
            var fromdt = document.getElementById('<%= txtFromDate.ClientID%>');
            var todt = document.getElementById('<%= txtToDate.ClientID%>');
            if (fromdt.value != "" && todt.value) {
                if ((Date.parse(fromdt.value) >= Date.parse(todt.value))) {
                    alert("To Date Should Be Greater Than From Date");
                    todt.value = "";
                }
            }
        }

    

    </script>
    <script type="text/javascript">
        function treeList_CustomDataCallback(s, e) {
            document.getElementById('treeListCountCell').innerHTML = e.result;
        }
        function treeList_SelectionChanged(s, e) {
            window.setTimeout(function () { s.PerformCustomDataCallback(''); }, 0)
        }

    
        function calendarShown(sender, args) {
            sender._popupBehavior._element.style.zIndex = 100004;
        }

      

    </script>
</asp:Content>
