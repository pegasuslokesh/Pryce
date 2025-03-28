<%@ Page Language="C#" MasterPageFile="~/ERPMaster.master" AutoEventWireup="true" CodeFile="ScrumDetail.aspx.cs" Inherits="ProjectManagement_ScrumDetail" %>

<%@ Register Assembly="DevExpress.Web.v18.1, Version=18.1.3.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Assembly="DevExpress.XtraReports.v18.1.Web.WebForms, Version=18.1.3.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.XtraReports.Web" TagPrefix="dx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="Head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="HeaderContent" runat="Server">
    <h1>
        <i class="fa fa-users"></i>
        <asp:Label ID="lblHeader" runat="server" Text="Scrum Master"></asp:Label>
    </h1>
    <ol class="breadcrumb">
        <li><a href="#"><i class="fa fa-dashboard"></i>
            <asp:Label ID="Label6" runat="server" Text="<%$ Resources:Attendance,Project Management%>"></asp:Label></a></li>
        <li><a href="#"><i class="fa fa-dashboard"></i>
            <asp:Label ID="Label10" runat="server" Text="<%$ Resources:Attendance,Project Management%>"></asp:Label></a></li>
        <li class="active">
            <asp:Label ID="Label5" runat="server" Text="Scrum Master"></asp:Label></li>
    </ol>
     <div class="modal fade" id="modelViewDetail" tabindex="-1" role="dialog" aria-labelledby="myModalLabel"
        aria-hidden="true">
        <div class="modal-dialog modal-lg">
            <div class="modal-content">
                <button type="button" id="" class="btn btn-danger" style="float:right"  data-dismiss="modal">
                    <span aria-hidden="true">&times;</span></button>  &nbsp;&nbsp;    <br />        
                <div class="modal-body">
                    <asp:UpdatePanel ID="Leave_List" runat="server">
                        <ContentTemplate>                        
                            <div class="box box-warning box-solid" <%= GvTaskList.Rows.Count>0?"style='display:block'":"style='display:none'"%>>

                                <div class="box-body">
                                    <div class="row">
                                        <div class="col-md-12">                                          
                                            <div class="flow">
                                                <h3>Task Detail</h3>
                                                <asp:HiddenField ID="RowScrumId" runat="server" Value="" />
                                                <asp:GridView CssClass="table-striped table-bordered table table-hover" AutoPostBack="True" ID="GvTaskList" AllowPaging="true" OnPageIndexChanging="GvTaskList_PageIndexChanging" PageSize="10" runat="server" AutoGenerateColumns="False" Width="100%">
                                                    <Columns>
                                                        <asp:TemplateField HeaderText="Project" SortExpression="Project">
                                                            <ItemTemplate>
                                                                <asp:Label ID="Label42" runat="server" Text='<%# Eval("Project") %>'></asp:Label>
                                                            </ItemTemplate>
                                                            <ItemStyle />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Task" SortExpression="Task">
                                                            <ItemTemplate>
                                                                <asp:Label ID="Label43" runat="server" Text='<%# Eval("Task") %>'></asp:Label>
                                                            </ItemTemplate>
                                                            <ItemStyle />
                                                        </asp:TemplateField>                                                       
                                                        <asp:TemplateField HeaderText="Close">
                                                            <HeaderTemplate>
                                                                <label><b>Close</b></label>
                                                                <asp:CheckBox ID="AllTaskClose" OnCheckedChanged="All_TaskClose" AutoPostBack="true"  runat="server" />
                                                            </HeaderTemplate>
                                                            <ItemTemplate>
                                                                <asp:HiddenField ID="RowTransId" runat="server" Value='<%# Eval("Trans_Id") %>' />                                                                
                                                                <asp:CheckBox ID="gvTaskChk" runat="server" />
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

                <div class="modal-footer">
                    <div class="col-md-6">                        
                           <asp:Button CssClass="btn btn-primary" ID="btnSaveTask" Text="Save" OnClick="btnSaveTask_Click" runat="server" />
                        &nbsp;
                        <button type="button" id="" class="btn btn-danger" data-dismiss="modal">
                        Cancel</button>
                    </div>                   
                    
                </div>
            </div>
        </div>
    </div>

    
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="Server">
    <asp:UpdatePanel ID="Update_Button" runat="server">
        <ContentTemplate>
            <asp:Button ID="Btn_List" Style="display: none;" runat="server" OnClick="btList_Click" Text="List" />
            <asp:Button ID="Btn_New" Style="display: none;" runat="server" OnClick="btnnew_Click" Text="New" />
            <asp:HiddenField runat="server" ID="hdnCanEdit" />
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
                       <li id="Li_Report"><a href="#Report" onclick="Li_Tab_List()" data-toggle="tab">
                        <i class="fas fa-book"></i>&nbsp;&nbsp;<asp:Label ID="Label2" runat="server" Text="<%$ Resources:Attendance,Report %>"></asp:Label></a></li>
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
                                                <div class="row">
                                                    <div class="col-md-8">
                                                        <strong>Running</strong>
                                                        <button type="button" style="border-radius: 0px;" class="btn btn-primary"></button>

                                                        <strong>Pass</strong>
                                                        <button type="button" style="border-radius: 0px;" class="btn btn-success"></button>

                                                        <strong>Failed</strong>
                                                        <button type="button" style="border-radius: 0px;" class="btn btn-danger"></button>

                                                        <strong>Pending</strong>
                                                        <button type="button" style="border-radius: 0px;" class="btn btn-warning"></button>


                                                        <strong>Delayed</strong>
                                                        <button type="button" style="border-radius: 0px;" class="btn btn-info"></button>

                                                    </div>
                                                </div>
                                                <br />
                                                <div class="form-group">
                                                    <div class="row">
                                                        <div class="form-group">
                                                            <%--   <div class="col-lg-2">
                                                                <asp:DropDownList ID="ddlProjectStatus" runat="server" CssClass="form-control"
                                                                    AutoPostBack="true" OnSelectedIndexChanged="ddlProjectStatus_SelectedIndexChanged">
                                                                    <asp:ListItem Text="All" Value="All"></asp:ListItem>
                                                                    <asp:ListItem Text="Open" Value="Open" Selected="True"></asp:ListItem>
                                                                    <asp:ListItem Text="Close" Value="Close"></asp:ListItem>
                                                                </asp:DropDownList>
                                                            </div>--%>
                                                            <div class="col-lg-2" >
                                                                <asp:DropDownList ID="ddlFieldName" OnSelectedIndexChanged="ddlFieldName_Changed" AutoPostBack="true" runat="server" CssClass="form-control">
                                                                    <asp:ListItem Text="Scrum No." Value="ScrumNumber" Selected="True"></asp:ListItem>
                                                                    <asp:ListItem Text="Scrum Name" Value="ScrumNAme" />
                                                                    <asp:ListItem Text="Assign To " Value="AssignTO" />
                                                                      <asp:ListItem Text="Scrum Status " Value="Status"/>
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
                                                            <div class="col-lg-2" visible="false"  id="pnlStatus" runat="server">  
                                                              <asp:DropDownList CssClass="form-control" runat="server" ID="ddlListStatus">
                                                                  <asp:ListItem Value="">All</asp:ListItem>
                                                                <asp:ListItem Value="1">Running</asp:ListItem>
                                                                <asp:ListItem Value="2">Failed</asp:ListItem>
                                                                <asp:ListItem Value="3">Pass</asp:ListItem>
                                                                <asp:ListItem Value="4">Pending</asp:ListItem>
                                                                <asp:ListItem Value="5">Delayed</asp:ListItem>
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
                                                                &nbsp;&nbsp;
                                                                <asp:LinkButton ID="btnTreeView"  runat="server" ToolTip="TreeView" OnClick="btnTreeView_Click" ><span  class="fa fa-sitemap" style="font-size:25px;"></span></asp:LinkButton>&nbsp;
                                                                <asp:LinkButton ID="btnListView" runat="server" Visible="false" ToolTip="List" OnClick="btnListView_Click"><span class="fa fa-list" style="font-size:25px;"></span></asp:LinkButton>
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
                                                    <asp:GridView CssClass="table-striped table-bordered table table-hover" ID="GvrScrumDetail"   AllowPaging="True" PageSize="<%# PageControlCommon.GetPageSize() %>" OnPageIndexChanging="GvrScrumIteam_PageIndexChanging"  runat="server" AutoGenerateColumns="False" Width="100%">
                                                        <Columns>
                                                            <asp:TemplateField HeaderText="Action">
                                                                <ItemTemplate>
                                                                    <div class="dropdown" style="position: absolute;">
                                                                        <button class="btn btn-default dropdown-toggle" type="button" data-toggle="dropdown">
                                                                            <i class="fa fa-ellipsis-h" aria-hidden="true"></i>
                                                                        </button>
                                                                        <ul class="dropdown-menu">
                                                                            <li <%= hdnCanEdit.Value=="true"?"style='display:block'":"style='display:none'" %>>
                                                                                <asp:LinkButton ID="IbtnEdit" runat="server" CausesValidation="False"  CommandArgument='<%# Eval("ScrumId") %>' OnCommand="IbtnEdit_Command"><i class="fa fa-pencil"></i>Edit</asp:LinkButton>
                                                                            </li>
                                                                            <li <%= hdnCanEdit.Value=="true"?"style='display:block'":"style='display:none'" %>>
                                                                                <asp:LinkButton ID="IbtnView" runat="server" CausesValidation="False" CommandArgument='<%# Eval("ScrumId") %>' OnCommand="IbtnView_Command"><i class="fa fa-eye"></i>View</asp:LinkButton>
                                                                            </li>
                                                                             <li <%= hdnCanEdit.Value=="true"?"style='display:block'":"style='display:none'" %>>
                                                                                <asp:LinkButton ID="IbtnPrint" runat="server" CausesValidation="False" CommandArgument='<%# Eval("ScrumId") %>' OnCommand="IbtnPrint_Command"><i class="fa fa-print"></i>Print</asp:LinkButton>
                                                                            </li>  
                                                                            <li <%= hdnCanEdit.Value=="true"?"style='display:block'":"style='display:none'" %>>
                                                                                <asp:LinkButton ID="LinkButton1" runat="server" CausesValidation="False" CommandArgument='<%# Eval("ScrumId") %>' OnCommand="IbtnClose_Command"><i class="fa fa-times"></i>Close</asp:LinkButton>
                                                                            </li>                                                                          
                                                                        </ul>
                                                                    </div>
                                                                </ItemTemplate>
                                                                <ItemStyle HorizontalAlign="Center" Width="90px" />
                                                               <HeaderStyle HorizontalAlign="Center" />
                                                            </asp:TemplateField>                                                           
                                                            <asp:TemplateField HeaderText="Scrum No" HeaderStyle-CssClass="text-center" SortExpression="ScrumNo">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblgvSInvNo" runat="server" Text='<%#Eval("ScrumNumber") %>' />
                                                                </ItemTemplate>
                                                                <ItemStyle HorizontalAlign="Center" Width="90px" />
                                                                <HeaderStyle HorizontalAlign="Center" Width="90px" />
                                                            </asp:TemplateField>
                                                             <asp:TemplateField HeaderText="Scrum Date" HeaderStyle-CssClass="text-center" SortExpression="ScrumDate">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblgvScrumDate" runat="server" Text='<%#Eval("ScrumDate") %>' />
                                                                </ItemTemplate>
                                                                <ItemStyle HorizontalAlign="Center" Width="90px" />
                                                                <HeaderStyle HorizontalAlign="Center" Width="90px" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Scrum Name" HeaderStyle-CssClass="text-center" SortExpression="ScrumName">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblgvSInvDate" runat="server" Text='<%#Eval("ScrumName").ToString()%>' />
                                                                </ItemTemplate>
                                                                <ItemStyle HorizontalAlign="Center" Width="80px" />
                                                                <HeaderStyle HorizontalAlign="Center" Width="80px" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Assign Date" HeaderStyle-CssClass="text-center" SortExpression="EndDate">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="gvListAssignDate" runat="server" Text='<%#Eval("AssignDate") %>' />
                                                                </ItemTemplate>
                                                                <ItemStyle HorizontalAlign="Center" Width="90px" />
                                                                <HeaderStyle HorizontalAlign="Center" Width="90px" />
                                                            </asp:TemplateField>
                                                             <asp:TemplateField HeaderText="Assign To" HeaderStyle-CssClass="text-center" SortExpression="AssignTo">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="gvListAssignTo" runat="server" Text='<%#Eval("AssignTo") %>' />
                                                                </ItemTemplate>
                                                                <ItemStyle HorizontalAlign="Center" Width="90px" />
                                                                <HeaderStyle HorizontalAlign="Center" Width="90px" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Start Date" HeaderStyle-CssClass="text-center" SortExpression="StartDate">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblgvRefType" runat="server" Text='<%#Eval("StartDate") %>' />
                                                                </ItemTemplate>
                                                                <ItemStyle HorizontalAlign="Center" Width="90px" />
                                                                <HeaderStyle HorizontalAlign="Center" Width="90px" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="End Date" HeaderStyle-CssClass="text-center" SortExpression="EndDate">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="gvListEndDate" runat="server" Text='<%#Eval("EndDate") %>' />
                                                                </ItemTemplate>
                                                                <ItemStyle HorizontalAlign="Center" Width="90px" />
                                                                <HeaderStyle HorizontalAlign="Center" Width="90px" />
                                                            </asp:TemplateField>
                                                           
                                                            <asp:TemplateField HeaderText="Status" HeaderStyle-CssClass="text-center" SortExpression="Status">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="gvListStatus" runat="server" Text='<%#Eval("Status") %>' />
                                                                </ItemTemplate>
                                                                <ItemStyle HorizontalAlign="Center" Width="90px" />
                                                                <HeaderStyle HorizontalAlign="Center" Width="90px" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Remark" HeaderStyle-CssClass="text-center" SortExpression="Remark">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="gvListRemark" runat="server" Text='<%#Eval("Remark") %>' />
                                                                </ItemTemplate>
                                                                <ItemStyle HorizontalAlign="Center" Width="90px" />
                                                                <HeaderStyle HorizontalAlign="Center" Width="90px" />
                                                            </asp:TemplateField>
                                                              <asp:TemplateField HeaderText="Total Hour" HeaderStyle-CssClass="text-center" SortExpression="NumberOfHours">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="gvListTotalHour" runat="server" Text='<%#Eval("NumberOfHours") %>' />
                                                                </ItemTemplate>
                                                                <ItemStyle HorizontalAlign="Center" Width="90px" />
                                                                <HeaderStyle HorizontalAlign="Center" Width="90px" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Percentage" HeaderStyle-CssClass="text-center" SortExpression="ProjectPercentage">
                                                                <ItemTemplate>
                                                                   <%-- <asp:Label ID="gvListPercentage" runat="server" Text='<%#Eval("ProjectPercentage") %>'></asp:Label>--%>
                                                                    <asp:Label ID="gvListPercentage" runat="server" Text='<%#Eval("ProjectPercentage") + "%" %>'></asp:Label>

                                                                </ItemTemplate>
                                                                  <ItemStyle HorizontalAlign="Center" Width="90px" />
                                                                <HeaderStyle HorizontalAlign="Center" Width="90px" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Created By" HeaderStyle-CssClass="text-center" SortExpression="CreatedBy">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="gvListCreatedBy" runat="server" Text='<%# GetUserId(Eval("CreatedBy").ToString()) %>' />
                                                                </ItemTemplate>
                                                                <ItemStyle HorizontalAlign="Center" Width="90px" />
                                                                <HeaderStyle HorizontalAlign="Center" Width="90px" />
                                                            </asp:TemplateField>
                                                        </Columns>
                                                        <PagerStyle CssClass="pagination-ys" />
                                                    </asp:GridView>
                                                    <asp:HiddenField ID="HDFSort" runat="server" />
                                                    <asp:HiddenField ID="HiddeniD" runat="server" />

                                                    <asp:TreeView ID="GvListTreeView" runat="server" Visible="false" ></asp:TreeView>


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
                                                    <div class="col-md-6">
                                                        <br></br>
                                                        <asp:RequiredFieldValidator EnableClientScript="true" Style="float: right;" runat="server" ID="RequiredFieldValidator1" ValidationGroup="Save"
                                                            Display="Dynamic" SetFocusOnError="true" ControlToValidate="txtSrmNo" ErrorMessage="Enter Scrum No."></asp:RequiredFieldValidator>
                                                        <asp:Label ID="Label4" runat="server" Text="Scrum Number"></asp:Label>
                                                        <asp:TextBox ID="txtSrmNo" runat="server" class="form-control"></asp:TextBox>
                                                    </div>
                                                    <div class="col-md-6">
                                                        <br></br>
                                                        <asp:Label ID="lblSrmDate" runat="server" Text="Scrum Date"></asp:Label>
                                                        <asp:TextBox ID="txtSrmdate" placeholder="Scrum Date" runat="server" CssClass="form-control"></asp:TextBox>
                                                        <cc1:CalendarExtender OnClientShown="showCalendar" Format="dd-MMM-yyyy" ID="txtFrom_CalendarExtender" runat="server" Enabled="True"
                                                            TargetControlID="txtSrmdate">
                                                        </cc1:CalendarExtender>
                                                        <asp:RequiredFieldValidator EnableClientScript="true" Style="float: right;" runat="server" ID="RequiredFieldValidator2" ValidationGroup="Save"
                                                            Display="Dynamic" SetFocusOnError="true" ControlToValidate="txtSrmdate" ErrorMessage="Enter Scrum Date."></asp:RequiredFieldValidator>
                                                    </div>
                                                    <div class="col-md-12">
                                                        <br></br>
                                                        <asp:Label ID="lblSrmName" runat="server" Text="Scrum Name"></asp:Label>
                                                        <asp:TextBox ID="txtSrmName" runat="server" CssClass="form-control"></asp:TextBox>
                                                        <asp:RequiredFieldValidator EnableClientScript="true" Style="float: right;" runat="server" ID="RequiredFieldValidator10" ValidationGroup="Save"
                                                            Display="Dynamic" SetFocusOnError="true" ControlToValidate="txtSrmName" ErrorMessage="Enter Scrum Scrum Name"></asp:RequiredFieldValidator>
                                                    </div>
                                                    <div class="col-md-6">
                                                        <br></br>
                                                        <asp:Label ID="lblStartDate" runat="server" Text="Start Date"></asp:Label>
                                                        <asp:TextBox ID="txtstartdate" placeholder="Start Date" runat="server" CssClass="form-control"></asp:TextBox>
                                                        <cc1:CalendarExtender OnClientShown="showCalendar" Format="dd-MMM-yyyy" ID="CalendarExtender1" runat="server" Enabled="True"
                                                            TargetControlID="txtstartdate">
                                                        </cc1:CalendarExtender>
                                                        <asp:RequiredFieldValidator EnableClientScript="true" Style="float: right;" runat="server" ID="RequiredFieldValidator8" ValidationGroup="Save"
                                                            Display="Dynamic" SetFocusOnError="true" ControlToValidate="txtstartdate" ErrorMessage="Enter Start Date"></asp:RequiredFieldValidator>
                                                    </div>

                                                    <div class="col-md-6">
                                                        <br></br>
                                                        <asp:Label ID="lblendDate" runat="server" Text="End Date"></asp:Label>
                                                        <asp:TextBox ID="txtexpenddate" runat="server" CssClass="form-control" placeholder="Exp End Date"></asp:TextBox>
                                                        <cc1:CalendarExtender OnClientShown="showCalendar" ID="CalendarExtender2" Format="dd-MMM-yyyy" runat="server"
                                                            Enabled="True" TargetControlID="txtexpenddate">
                                                        </cc1:CalendarExtender>
                                                        <asp:RequiredFieldValidator EnableClientScript="true" Style="float: right;" runat="server" ID="RequiredFieldValidator9" ValidationGroup="Save"
                                                            Display="Dynamic" SetFocusOnError="true" ControlToValidate="txtexpenddate" ErrorMessage="Enter Scrum End Date"></asp:RequiredFieldValidator>
                                                    </div>
                                                    <div class="col-md-12">
                                                        <br></br>
                                                        <div class="row">
                                                            <div class="col-md-12">
                                                                <div class="box box-primary">
                                                                    <div class="box-header with-border">
                                                                        <h3 class="box-title">
                                                                            <asp:Label ID="lnkAddExp" runat="server" Text="Scrum Detail"></asp:Label></h3>
                                                                        <div class="box-tools pull-right">
                                                                            <button type="button" class="btn btn-box-tool" data-widget="collapse">
                                                                                <i class="fa fa-minus"></i>
                                                                            </button>
                                                                        </div>
                                                                    </div>
                                                                    <div class="box-body">
                                                                        <div class="form-group">
                                                                            <div class="col-md-12" id="pnlTabInformation" runat="server">
                                                                                <cc1:TabContainer ID="tabContainer" runat="server" CssClass="ajax__tab_yuitabview-theme" ActiveTabIndex="1">
                                                                                    <cc1:TabPanel ID="TabPanelproduct" runat="server" Width="100%" HeaderText="Task Detail">
                                                                                        <ContentTemplate>
                                                                                            <asp:UpdatePanel ID="Update_TabPanelproduct" runat="server">
                                                                                                <ContentTemplate>
                                                                                                    <div class="row">
                                                                                                        <div class="col-md-12" style="overflow: auto; max-height: 500px;">
                                                                                                            <asp:GridView CssClass="table-striped table-bordered table table-hover" ID="gvproduct" ShowHeader="true" runat="server" AutoGenerateColumns="false" Width="100%"
                                                                                                                ShowFooter="true" OnRowDeleting="gvproduct_RowDeleting"
                                                                                                                OnRowCommand="gvproduct_RowCommand">
                                                                                                                <Columns>                                                                                                                    
                                                                                                                    <asp:TemplateField HeaderText="<%$ Resources:Attendance,Project %>" HeaderStyle-HorizontalAlign="Center" >
                                                                                                                        <ItemTemplate>
                                                                                                                            <asp:Label ID="lblProject" runat="server" Text='<%#Eval("Project") %>'></asp:Label>
                                                                                                                        </ItemTemplate>
                                                                                                                        <FooterTemplate>
                                                                                                                            <asp:TextBox ID="txtProject" runat="server" OnTextChanged="GetProjectId" AutoPostBack="true" CssClass="form-control" BackColor="#eeeeee"></asp:TextBox>
                                                                                                                            <cc1:AutoCompleteExtender ID="AutoCompleteExtender2" runat="server"
                                                                                                                                DelimiterCharacters="" Enabled="True" CompletionInterval="100" CompletionSetCount="1"
                                                                                                                                MinimumPrefixLength="0" ServiceMethod="GetCompletionListProject" ServicePath=""
                                                                                                                                TargetControlID="txtProject" UseContextKey="True" CompletionListCssClass="autoCompleteList" CompletionListItemCssClass="autoCompleteListItem" CompletionListHighlightedItemCssClass="autoCompleteSelectedListItem" OnClientShown="resetPosition">
                                                                                                                            </cc1:AutoCompleteExtender>
                                                                                                                            <asp:RequiredFieldValidator EnableClientScript="true" Style="float: right;" runat="server" ID="RequiredFieldValidator8" ValidationGroup="AddProduct"
                                                                                                                                Display="Dynamic" SetFocusOnError="true" ControlToValidate="txtProject" ErrorMessage="Enter Project"></asp:RequiredFieldValidator>
                                                                                                                        </FooterTemplate>
                                                                                                                        <ItemStyle HorizontalAlign="Left" />
                                                                                                                    </asp:TemplateField>
                                                                                                                    <asp:TemplateField HeaderText="<%$ Resources:Attendance,Task%>" HeaderStyle-HorizontalAlign="Center" >
                                                                                                                        <ItemTemplate>
                                                                                                                            <asp:Label ID="lblTaskName" runat="server" Text='<%#Eval("Task") %>'></asp:Label>
                                                                                                                        </ItemTemplate>
                                                                                                                        <FooterTemplate>
                                                                                                                            <asp:TextBox ID="txtTask" runat="server" CssClass="form-control" OnTextChanged="txtTask_Changed" AutoPostBack="true" BackColor="#eeeeee"></asp:TextBox>
                                                                                                                            <cc1:AutoCompleteExtender ID="AutoCompleteExtender3" runat="server"
                                                                                                                                DelimiterCharacters="" Enabled="True" CompletionInterval="100" CompletionSetCount="1"
                                                                                                                                MinimumPrefixLength="0" ServiceMethod="GetTaskList" ServicePath=""
                                                                                                                                TargetControlID="txtTask" UseContextKey="True" CompletionListCssClass="autoCompleteList" CompletionListItemCssClass="autoCompleteListItem" CompletionListHighlightedItemCssClass="autoCompleteSelectedListItem" OnClientShown="resetPosition">
                                                                                                                            </cc1:AutoCompleteExtender>
                                                                                                                            <asp:RequiredFieldValidator EnableClientScript="true" Style="float: right;" runat="server" ID="RequiredFieldValidator9" ValidationGroup="AddProduct"
                                                                                                                                Display="Dynamic" SetFocusOnError="true" ControlToValidate="txtTask" ErrorMessage="Enter Task"></asp:RequiredFieldValidator>
                                                                                                                        </FooterTemplate>
                                                                                                                        <ItemStyle HorizontalAlign="Left" />
                                                                                                                    </asp:TemplateField>  
                                                                                                                    <asp:TemplateField HeaderText="Task Hour" HeaderStyle-HorizontalAlign="Center">
                                                                                                                        <ItemTemplate>
                                                                                                                            <asp:Label ID="lblTaskHours"  runat="server" Text='<%# Eval("TaskHour")%>' ></asp:Label>
                                                                                                                        </ItemTemplate>
                                                                                                                        <FooterTemplate>
                                                                                                                            <asp:TextBox ID="txtTaskHours" CssClass="form-control" ReadOnly="true" runat="server" ></asp:TextBox>
                                                                                                                        </FooterTemplate>
                                                                                                                    </asp:TemplateField>                                                                                                                 
                                                                                                                    <asp:TemplateField HeaderText="Task Status" HeaderStyle-HorizontalAlign="Center" >
                                                                                                                        <ItemTemplate>
                                                                                                                            <asp:Label ID="lblTaskStatus" runat="server" Text='<%#Eval("StatusId") %>'></asp:Label>
                                                                                                                        </ItemTemplate>
                                                                                                                        <FooterTemplate>
                                                                                                                            <%--<asp:TextBox ID="txtTaskStatus" runat="server" CssClass="form-control" BackColor="#eeeeee"></asp:TextBox>--%>
                                                                                                                            <asp:DropDownList CssClass="form-control" runat="server" ID="txtTaskStatus">
                                                                                                                                <asp:ListItem Value="1">Running</asp:ListItem>
                                                                                                                                <asp:ListItem Value="2">Failed</asp:ListItem>
                                                                                                                                <asp:ListItem Value="3">Pass</asp:ListItem>
                                                                                                                                <asp:ListItem Value="4">Pending</asp:ListItem>
                                                                                                                                <asp:ListItem Value="5">Delayed</asp:ListItem>
                                                                                                                            </asp:DropDownList>
                                                                                                                        </FooterTemplate>
                                                                                                                        <ItemStyle HorizontalAlign="Left" />
                                                                                                                    </asp:TemplateField>
                                                                                                                    <asp:TemplateField HeaderText="Task Percentage" HeaderStyle-HorizontalAlign="Center">
                                                                                                                        <ItemTemplate>
                                                                                                                            <asp:Label ID="lblTaskPercentage" runat="server" Text='<%#Eval("TaskPercentage") %>'></asp:Label>
                                                                                                                        </ItemTemplate>
                                                                                                                        <FooterTemplate>
                                                                                                                            <asp:TextBox ID="txtTaskPercentage" type="number" runat="server" CssClass="form-control"  BackColor="#eeeeee"></asp:TextBox>
                                                                                                                        </FooterTemplate>
                                                                                                                        <ItemStyle HorizontalAlign="Left" />
                                                                                                                    </asp:TemplateField>
                                                                                                                    <asp:TemplateField HeaderText="Expected Cost" HeaderStyle-HorizontalAlign="Center">
                                                                                                                        <ItemTemplate>
                                                                                                                            <asp:Label ID="lblGvCost" runat="server" Text='<%#Eval("Cost") %>'></asp:Label>
                                                                                                                        </ItemTemplate>
                                                                                                                        <FooterTemplate>
                                                                                                                            <asp:TextBox ID="txtGvCost" runat="server" ReadOnly="true" CssClass="form-control" BackColor="#eeeeee"></asp:TextBox>
                                                                                                                        </FooterTemplate>
                                                                                                                    </asp:TemplateField>
                                                                                                                    <asp:TemplateField HeaderText="Task Date"  HeaderStyle-HorizontalAlign="Center">
                                                                                                                        <ItemTemplate>
                                                                                                                            <asp:Label ID="lblGvTaskDate" runat="server" Text='<%#Eval("Date") %>'></asp:Label>
                                                                                                                        </ItemTemplate>
                                                                                                                        <FooterTemplate>
                                                                                                                            <asp:TextBox ID="txtTaskDate" runat="server" ReadOnly="true" CssClass="form-control" BackColor="#eeeeee"></asp:TextBox>
                                                                                                                        </FooterTemplate>
                                                                                                                        </asp:TemplateField>
                                                                                                                    <asp:TemplateField HeaderText="Remark" HeaderStyle-HorizontalAlign="Center">
                                                                                                                        <ItemTemplate>
                                                                                                                            <asp:Label ID="lblGvRemark"  runat="server"  Text='<%#Eval("Remark") %>'></asp:Label>
                                                                                                                        </ItemTemplate>
                                                                                                                        <FooterTemplate>
                                                                                                                            <asp:TextBox ID="txtgvRemark"  runat="server"  CssClass="form-control" BackColor="#eeeeee"></asp:TextBox>
                                                                                                                        </FooterTemplate>
                                                                                                                        <ItemStyle HorizontalAlign="Left" />
                                                                                                                    </asp:TemplateField>
                                                                                                                    <asp:TemplateField HeaderText="Action" HeaderStyle-HorizontalAlign="Center" >
                                                                                                                        <EditItemTemplate>
                                                                                                                            <asp:Button ID="ButtonUpdate" runat="server" CssClass="btn btn-info" CommandName="Update" Text="Update" CausesValidation="true"
                                                                                                                                CommandArgument='<%#Eval("Trans_Id") %>' />
                                                                                                                            <asp:Button ID="ButtonCancel" runat="server" CssClass="btn btn-primary" CommandName="Cancel" Text="Cancel" />
                                                                                                                        </EditItemTemplate>
                                                                                                                        <ItemTemplate>
                                                                                                                            <asp:Button ID="ButtonEdit" runat="server" CommandName="Edit" Text="Edit" CssClass="btn btn-info" Visible="false" />
                                                                                                                            <asp:ImageButton ID="ButtonDelete" runat="server" CommandArgument='<%#Eval("ScrumId	") %>' CommandName="Delete" Width="14px" Height="14px" ImageUrl="~/Images/Delete1.png" />
                                                                                                                        </ItemTemplate>
                                                                                                                        <FooterTemplate>
                                                                                                                            <asp:Panel ID="pnlGridviewfeedback" runat="server" DefaultButton="ButtonAdd">

                                                                                                                                <%--  <asp:ImageButton ID="btnadd" runat="server" ImageUrl="~/Images/add.png" CommandName="AddNew" ToolTip="Add" />--%>

                                                                                                                                <asp:Button ID="ButtonAdd" runat="server" ValidationGroup="AddProduct" CssClass="btn btn-info" CommandName="AddNew" Text="Add Task" />
                                                                                                                                <%--<asp:Button ID="ButtonAdd" runat="server" CommandName="AddNew" Text="Add New Row" />--%>
                                                                                                                            </asp:Panel>
                                                                                                                        </FooterTemplate>
                                                                                                                    </asp:TemplateField>

                                                                                                                    
                                                                                                                </Columns>


                                                                                                            </asp:GridView>
                                                                                                            <br />
                                                                                                        </div>
                                                                                                    </div>
                                                                                                </ContentTemplate>
                                                                                            </asp:UpdatePanel>
                                                                                            <asp:UpdateProgress ID="UpdateProgress8" runat="server" AssociatedUpdatePanelID="Update_TabPanelproduct">
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
                                                                </div>
                                                            </div>
                                                        </div>
                                                        <br />
                                                    </div>
                                                    <div class="col-md-12">
                                                        <div class="col-md-6">
                                                            <br></br>
                                                            <asp:Label ID="lblStatus" runat="server" Text="Status"></asp:Label>
                                                            <asp:DropDownList CssClass="form-control" OnSelectedIndexChanged="ddlNewStatus_Changed" runat="server" ID="ddlStatus">
                                                                <asp:ListItem Value="1">Running</asp:ListItem>
                                                                <asp:ListItem Value="2">Failed</asp:ListItem>
                                                                <asp:ListItem Value="3">Pass</asp:ListItem>
                                                                <asp:ListItem Value="4">Pending</asp:ListItem>
                                                                <asp:ListItem Value="5">Delayed</asp:ListItem>
                                                            </asp:DropDownList>
                                                        </div>
                                                        <div class="col-md-6" ID="ExtendPanel" visible="false" runat="server">
                                                            <asp:Label ID="ExtendDate" runat="server" Text="Expected End Date" ></asp:Label>
                                                            <asp:TextBox ID="txtExtendDate" runat="server" CssClass="form-control" BackColor="#EEEEEE"></asp:TextBox>
                                                        </div>
                                                    </div>
                                                    <div class="col-md-12">
                                                        <div class="col-md-6">
                                                            <br></br>
                                                            <asp:Label ID="lblAsignTo" runat="server" Text="Assign To"></asp:Label>
                                                            <asp:TextBox ID="txtAssignTo" OnTextChanged="GetEmployeeId" runat="server" BackColor="#EEEEEE" CssClass="form-control"></asp:TextBox>
                                                            <cc1:AutoCompleteExtender ID="txtSalesPerson_AutoCompleteExtender" runat="server"
                                                                DelimiterCharacters="" Enabled="True" CompletionInterval="100" CompletionSetCount="1"
                                                                MinimumPrefixLength="1" ServiceMethod="GetCompletionListEmployeeName" ServicePath=""
                                                                TargetControlID="txtAssignTo" UseContextKey="True" CompletionListCssClass="autoCompleteList" CompletionListItemCssClass="autoCompleteListItem" CompletionListHighlightedItemCssClass="autoCompleteSelectedListItem" OnClientShown="resetPosition">
                                                            </cc1:AutoCompleteExtender>
                                                            <asp:RequiredFieldValidator EnableClientScript="true" Style="float: right;" runat="server" ID="RequireAssign" ValidationGroup="Save"
                                                                Display="Dynamic" SetFocusOnError="true" ControlToValidate="txtAssignTo" ErrorMessage="Enter  Assign Employee"></asp:RequiredFieldValidator>
                                                        </div>
                                                        <div class="col-md-6">
                                                            <br></br>
                                                            <asp:Label ID="lblAssignDate" runat="server" Text="Assign Date"></asp:Label>
                                                            <asp:TextBox ID="txtAssignDate" runat="server" CssClass="form-control" placeholder="Assign Date"></asp:TextBox>
                                                            <cc1:CalendarExtender OnClientShown="showCalendar" ID="CalendarExtender3" Format="dd-MMM-yyyy" runat="server"
                                                                Enabled="True" TargetControlID="txtAssignDate">
                                                            </cc1:CalendarExtender>
                                                            <asp:RequiredFieldValidator EnableClientScript="true" Style="float: right;" runat="server" ID="RequiredFieldValidator6" ValidationGroup="Save"
                                                                Display="Dynamic" SetFocusOnError="true" ControlToValidate="txtAssignDate" ErrorMessage="Enter  Assign Date"></asp:RequiredFieldValidator>
                                                        </div>
                                                    </div>
                                                    <div class="col-md-12">
                                                        <div class="col-md-6">
                                                            <br></br>
                                                            <asp:Label ID="lblNoHrs" Text="No Of Hours" runat="server"></asp:Label>
                                                            <asp:TextBox runat="server" ID="txtNoOfHrs" ReadOnly="true" CssClass="form-control"></asp:TextBox>

                                                            <%-- <cc1:MaskedEditExtender ID="MaskedEditExtender6" runat="server" CultureAMPMPlaceholder=""
                                                                CultureCurrencySymbolPlaceholder="" CultureDateFormat="" CultureDatePlaceholder=""
                                                                CultureDecimalPlaceholder="" CultureThousandsPlaceholder="" CultureTimePlaceholder=""
                                                                Enabled="True" Mask="99:99" MaskType="Time" TargetControlID="txtNoOfHrs">
                                                            </cc1:MaskedEditExtender>--%>
                                                            <asp:RequiredFieldValidator EnableClientScript="true" Style="float: right;" runat="server" ID="RequiredFieldValidator5" ValidationGroup="Save"
                                                                Display="Dynamic" SetFocusOnError="true" ControlToValidate="txtNoOfHrs" ErrorMessage="Enter  No. Of Hours"></asp:RequiredFieldValidator>
                                                        </div>
                                                        <div class="col-md-6">
                                                            <br></br>
                                                            <asp:Label ID="lblRemark" runat="server" Text="Remark"></asp:Label>
                                                            <asp:TextBox ID="txtRemark" CssClass="form-control" runat="server"></asp:TextBox>
                                                        </div>
                                                    </div>
                                                    <div class="col-md-12">
                                                        <div class="col-md-6">
                                                            <br></br>
                                                            <asp:Label Text="Priority" runat="server" ID="lblPriority"></asp:Label>
                                                            <asp:DropDownList CssClass="form-control" runat="server" ID="ddlPriority">
                                                                <asp:ListItem Value="1">High</asp:ListItem>
                                                                <asp:ListItem Value="2">Medium</asp:ListItem>
                                                                <asp:ListItem Value="3">Low</asp:ListItem>
                                                            </asp:DropDownList>
                                                        </div>
                                                        <div class="col-md-6">
                                                            <br></br>
                                                            <asp:Label ID="lblperScrum" Text="ParentScrum" runat="server"></asp:Label>
                                                            <asp:TextBox ID="txtPerScrum" OnTextChanged="GetParentScrumId" CssClass="form-control" BackColor="#EEEEEE" runat="server"></asp:TextBox>
                                                            <cc1:AutoCompleteExtender ID="txtPerant_AutoCompleteExtender1" runat="server"
                                                                DelimiterCharacters="" Enabled="True" CompletionInterval="100" CompletionSetCount="1"
                                                                MinimumPrefixLength="1" ServiceMethod="GetParentScrumList" ServicePath=""
                                                                TargetControlID="txtPerScrum" UseContextKey="True" CompletionListCssClass="autoCompleteList" CompletionListItemCssClass="autoCompleteListItem" CompletionListHighlightedItemCssClass="autoCompleteSelectedListItem" OnClientShown="resetPosition">
                                                            </cc1:AutoCompleteExtender>
                                                        </div>
                                                    </div>
                                                    <div class="col-md-12">
                                                        <div class="col-md-6">
                                                            <br></br>
                                                            <asp:Label ID="lblReleaseDate" Text="Release Date" runat="server"></asp:Label>
                                                            <asp:TextBox ID="txtReleaseDate" CssClass="form-control" runat="server"></asp:TextBox>
                                                            <cc1:CalendarExtender OnClientShown="showCalendar" ID="CalendarExtender4" Format="dd-MMM-yyyy" runat="server"
                                                                Enabled="True" TargetControlID="txtReleaseDate">
                                                            </cc1:CalendarExtender>
                                                            <%--                 <asp:RequiredFieldValidator EnableClientScript="true" Style="float: right;" runat="server" ID="RequiredFieldValidator4" ValidationGroup="Save"
                                                            Display="Dynamic" SetFocusOnError="true" ControlToValidate="txtReleaseDate" ErrorMessage="Enter Release Date"></asp:RequiredFieldValidator> --%>
                                                        </div>
                                                        <div class="col-md-6">
                                                            <br></br>
                                                            <asp:Label ID="lblPercentage" Text="Percentage" runat="server"></asp:Label>
                                                            <asp:TextBox ID="txtPercentage" type="number" CssClass="form-control" runat="server"></asp:TextBox>
                                                            <%-- <asp:RequiredFieldValidator EnableClientScript="true" Style="float: right;" runat="server" ID="RequiredFieldValidator3" ValidationGroup="Save"
                                                            Display="Dynamic" SetFocusOnError="true" ControlToValidate="txtPercentage" ErrorMessage="Enter Percentage"></asp:RequiredFieldValidator>--%>
                                                        </div>
                                                    </div>
                                                    <div class="col-md-12" style="text-align: center">
                                                        <br></br>
                                                        <asp:Button ID="btnsave" runat="server" Visible="true" CssClass="btn btn-success"
                                                            OnClick="btnsave_Click" ValidationGroup="Save" Text="<%$ Resources:Attendance,Save%>" />&nbsp;&nbsp;
                                               <asp:Button ID="btnreset" runat="server" CssClass="btn btn-primary" OnClick="btnreset_Click"
                                                   Text="<%$ Resources:Attendance,Reset%>" />&nbsp;&nbsp;
                                               <asp:Button ID="btncencel" runat="server" Text="<%$ Resources:Attendance,Cancel%>"
                                                   OnClick="btncencel_Click" CssClass="btn btn-danger" />
                                                        <br />
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
                    <div class="tab-pane" id="Report">
                        <asp:UpdatePanel ID="Update_Report" runat="server">
                            <Triggers>
                                <asp:PostBackTrigger ControlID="BtnExportPDF" />                                
                            </Triggers>
                            <ContentTemplate>                     
                                <div class="row">
                                    <div class="col-md-12">
                                        <div class="box box-primary">
                                            <div class="box-body">
                                                <div class="form-group">
                                                    <div visible="false" id="StaticsPanel" runat="server">

                                                        <div class="col-md-12">
                                                            <div class="col-md-6">
                                                                <asp:DropDownList CssClass="form-control" ID="ddlMonth" runat="server">
                                                                      <asp:ListItem Value="" Selected="True">All</asp:ListItem>
                                                                    <asp:ListItem Value="1">January</asp:ListItem>
                                                                    <asp:ListItem Value="2">February</asp:ListItem>
                                                                    <asp:ListItem Value="3">March</asp:ListItem>
                                                                    <asp:ListItem Value="4">April</asp:ListItem>
                                                                    <asp:ListItem Value="5">May</asp:ListItem>
                                                                    <asp:ListItem Value="6">June</asp:ListItem>
                                                                    <asp:ListItem Value="7">July</asp:ListItem>
                                                                    <asp:ListItem Value="8">August</asp:ListItem>
                                                                    <asp:ListItem Value="9">September</asp:ListItem>
                                                                    <asp:ListItem Value="10">October</asp:ListItem>
                                                                    <asp:ListItem Value="11">November</asp:ListItem>
                                                                    <asp:ListItem Value="12">December</asp:ListItem>
                                                                </asp:DropDownList>
                                                                <div class="col-md-6">
                                                                    <br />
                                                                    <asp:Button ID="btnStaticsReport" CssClass="btn btn-primary" runat="server" Text="Statics Report" OnClick="btnStaticsReport_Click" />&nbsp;
                                                                    <asp:Button ID="btnbackStatics" CssClass="btn btn-primary" runat="server" Text="Back" OnClick="btnbackReport_Click" />
                                                                </div>

                                                            </div>
                                                        </div>

                                                    </div>
                                                    <div id="reportpnl" runat="server">
                                                        <div class="col-md-12">
                                                               <asp:CheckBox ID="chkStaticsReport" AutoPostBack="true" OnCheckedChanged="chkStaticsReport_Click" runat="server" />
                                                        <asp:Label ID="lblchkstatics" runat="server" Text="Statics Report"></asp:Label>
                                                        </div>                                                    
                                                   
                                                    <div class="col-md-6">
                                                        <br />
                                                        <asp:Label ID="lblReportEmp" Text="Employee Name" CssClass="control-label" runat="server">
                                                        </asp:Label><br />
                                                        <asp:TextBox ID="txtReportEmp" CssClass="form-control" runat="server" ></asp:TextBox>
                                                          <cc1:AutoCompleteExtender ID="AutoCompleteExtender1" runat="server"
                                                                DelimiterCharacters="" Enabled="True" CompletionInterval="100" CompletionSetCount="1"
                                                                MinimumPrefixLength="1" ServiceMethod="GetCompletionListEmployeeName" ServicePath=""
                                                                TargetControlID="txtReportEmp" UseContextKey="True" CompletionListCssClass="autoCompleteList" CompletionListItemCssClass="autoCompleteListItem" CompletionListHighlightedItemCssClass="autoCompleteSelectedListItem" OnClientShown="resetPosition">
                                                            </cc1:AutoCompleteExtender>
                                                    </div>
                                                    <div class="col-md-6">
                                                        <br />
                                                        <asp:Label ID="LblReportStatus" runat="server" Text="Status"></asp:Label>
                                                        <br />
                                                        <asp:DropDownList CssClass="form-control" runat="server" ID="ddlReportStatus">
                                                            <asp:ListItem Value="">All</asp:ListItem>
                                                            <asp:ListItem Value="1">Running</asp:ListItem>
                                                            <asp:ListItem Value="2">Failed</asp:ListItem>
                                                            <asp:ListItem Value="3">Pass</asp:ListItem>
                                                            <asp:ListItem Value="4">Pending</asp:ListItem>
                                                            <asp:ListItem Value="5">Delayed</asp:ListItem>
                                                        </asp:DropDownList>
                                                    </div>
                                                    <div class="col-md-6">
                                                        <br />
                                                        <asp:Label ID="lblFromDate" Text="From Date" runat="server" CssClass="control-label"></asp:Label><br />
                                                        <asp:Textbox ID="txtFromDate" runat="server" CssClass="form-control"></asp:Textbox>
                                                         <cc1:CalendarExtender OnClientShown="showCalendar" ID="CalendarExtender5" Format="dd-MMM-yyyy" runat="server"
                                                                Enabled="True" TargetControlID="txtFromDate">
                                                            </cc1:CalendarExtender>
                                                    </div>
                                                    <div class="col-md-6"><br />
                                                        <asp:Label ID="lblToDate" Text="To Date" runat="server" CssClass="control-label"></asp:Label><br />
                                                        <asp:TextBox ID="txtToDate" runat="server" CssClass="form-control"></asp:TextBox>
                                                          <cc1:CalendarExtender OnClientShown="showCalendar" ID="CalendarExtender6" Format="dd-MMM-yyyy" runat="server"
                                                                Enabled="True" TargetControlID="txtToDate">
                                                            </cc1:CalendarExtender>
                                                    </div>   
                                                    <div class="col-md-12" style="align-items:center; text-align:center;">
                                                        <br />
                                                        <asp:Button ID="btnFindRecord" runat="server" OnClick="btnFindRecord_Click" Text="Search" CssClass="btn btn-primary" />&nbsp;
                                                         <asp:Button ID="BtnExportPDF"  CssClass="btn btn-primary" runat="server" Text="Export PDF" OnClick="BtnExportPDF_Click" />&nbsp;
                                                        <asp:Button ID="btnCancelReport" CssClass="btn btn-danger" runat="server" Text="Cancel" OnClick="btnCancelReport_Click" />
                                                      <%--   &nbsp;<asp:Button ID="btnPrintReportVoucher" CssClass="btn btn-default" runat="server" OnClick="voucherPrint" />--%>
                                                    </div>       
                                                 </div>                                      
                                                </div>
                                            </div>
                                            <div class="box-body">
                                                <div class="row">
                                        <div class="col-md-12">                                          
                                            <div class="flow">                                              
                                                <asp:HiddenField ID="HiddenField1" runat="server" Value="" />
                                                <asp:GridView CssClass="table-striped table-bordered table table-hover" AutoPostBack="True" ID="GvReport" AllowPaging="true" OnPageIndexChanging="GvTaskList_PageIndexChanging" PageSize="10" runat="server" AutoGenerateColumns="False" Width="100%">
                                                    <Columns>
                                                         <asp:TemplateField HeaderText="Scrum No" HeaderStyle-CssClass="text-center" SortExpression="ScrumNo">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblgvReportSNO" runat="server" Text='<%#Eval("ScrumNumber") %>' />
                                                                </ItemTemplate>
                                                                <ItemStyle HorizontalAlign="Center" Width="90px" />
                                                                <HeaderStyle HorizontalAlign="Center" Width="90px" />
                                                            </asp:TemplateField>
                                                             <asp:TemplateField HeaderText="Scrum Date" HeaderStyle-CssClass="text-center" SortExpression="ScrumDate">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblgvReportScrumDate" runat="server" Text='<%#Eval("ScrumDate") %>' />
                                                                </ItemTemplate>
                                                                <ItemStyle HorizontalAlign="Center" Width="90px" />
                                                                <HeaderStyle HorizontalAlign="Center" Width="90px" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Scrum Name" HeaderStyle-CssClass="text-center" SortExpression="ScrumName">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblgvReportScrumName" runat="server" Text='<%#Eval("ScrumName").ToString()%>' />
                                                                </ItemTemplate>
                                                                <ItemStyle HorizontalAlign="Center" Width="80px" />
                                                                <HeaderStyle HorizontalAlign="Center" Width="80px" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Assign Date" HeaderStyle-CssClass="text-center" SortExpression="EndDate">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblgvReportAssignDate" runat="server" Text='<%#Eval("AssignDate") %>' />
                                                                </ItemTemplate>
                                                                <ItemStyle HorizontalAlign="Center" Width="90px" />
                                                                <HeaderStyle HorizontalAlign="Center" Width="90px" />
                                                            </asp:TemplateField>
                                                             <asp:TemplateField HeaderText="Assign To" HeaderStyle-CssClass="text-center" SortExpression="AssignTo">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="gvReportAssignTo" runat="server" Text='<%#Eval("AssignTo") %>' />
                                                                </ItemTemplate>
                                                                <ItemStyle HorizontalAlign="Center" Width="90px" />
                                                                <HeaderStyle HorizontalAlign="Center" Width="90px" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Start Date" HeaderStyle-CssClass="text-center" SortExpression="StartDate">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblgvReportStartDate" runat="server" Text='<%#Eval("StartDate") %>' />
                                                                </ItemTemplate>
                                                                <ItemStyle HorizontalAlign="Center" Width="90px" />
                                                                <HeaderStyle HorizontalAlign="Center" Width="90px" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="End Date" HeaderStyle-CssClass="text-center" SortExpression="EndDate">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="gvReportEndDate" runat="server" Text='<%#Eval("EndDate") %>' />
                                                                </ItemTemplate>
                                                                <ItemStyle HorizontalAlign="Center" Width="90px" />
                                                                <HeaderStyle HorizontalAlign="Center" Width="90px" />
                                                            </asp:TemplateField>
                                                           
                                                            <asp:TemplateField HeaderText="Status" HeaderStyle-CssClass="text-center" SortExpression="Status">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="gvReportStatus" runat="server" Text='<%#Eval("Status") %>' />
                                                                </ItemTemplate>
                                                                <ItemStyle HorizontalAlign="Center" Width="90px" />
                                                                <HeaderStyle HorizontalAlign="Center" Width="90px" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Created By" HeaderStyle-CssClass="text-center" SortExpression="CreatedBy">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="gvReportCreatedBy" runat="server" Text='<%# GetUserId(Eval("CreatedBy").ToString()) %>' />
                                                                </ItemTemplate>
                                                                <ItemStyle HorizontalAlign="Center" Width="90px" />
                                                                <HeaderStyle HorizontalAlign="Center" Width="90px" />
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
                </div>

            </div>
        </div>
    </div>


</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="FooterContent" runat="Server">
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="AfterLoadScriptContent" runat="Server">
    <script src="../Script/common.js"></script>
    <script>
        function LI_List_Active() {
            $("#Li_List").addClass("active");
            $("#List").addClass("active");

            $("#Li_New").removeClass("active");
            $("#New").removeClass("active");
        }
        function LI_New_Active() {
            $("#Li_List").removeClass("active");
            $("#List").removeClass("active");

            $("#Li_New").addClass("active");
            $("#New").addClass("active");
        }
    </script>
</asp:Content>
