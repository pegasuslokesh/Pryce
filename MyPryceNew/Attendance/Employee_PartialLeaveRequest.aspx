<%@ Page Title="" Language="C#" MasterPageFile="~/ERPMaster.master" AutoEventWireup="true" CodeFile="Employee_PartialLeaveRequest.aspx.cs" Inherits="Attendance_Employee_PartialLeaveRequest" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="Head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="HeaderContent" runat="Server">
    <h1>
        <img src="../Images/employee_partial_leave_request.png" alt="" />
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
            <asp:Button ID="Btn_List" runat="server" Style="display: none;" Text="List" OnClick="btnList_Click" />
            <asp:Button ID="Btn_New" runat="server" Style="display: none;" Text="New" OnClick="btnNew_Click" />
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
                    <li id="Li_New"><a onclick="Li_New_Click()" href="#New" data-toggle="tab">
                        <asp:UpdatePanel ID="Update_Li" runat="server">
                            <ContentTemplate>
                                <i class="fa fa-file"></i>&nbsp;&nbsp;
                                <asp:Label ID="Lbl_Tab_New" runat="server" Text="<%$ Resources:Attendance,New%>"></asp:Label>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </a></li>
                    <li id="Li_List" class="active"><a onclick="Li_List_Click()" href="#List" data-toggle="tab">
                        <i class="fa fa-list"></i>&nbsp;&nbsp;<asp:Label ID="Label1" runat="server" Text="<%$ Resources:Attendance,List %>"></asp:Label></a></li>
                </ul>
                <div class="tab-content">
                    <div class="tab-pane active" id="List">
                        <asp:UpdatePanel ID="Update_List" runat="server">
                            <ContentTemplate>
                                <div class="box box-warning box-solid">
                                    <div class="box-header with-border">
                                        <h3 class="box-title"></h3>
                                    </div>
                                    <div class="box-body">
                                        <div class="row">
                                            <div class="col-md-12">
                                                <div class="flow">
                                                    <asp:GridView CssClass="table-striped table-bordered table table-hover" ID="GvPending" PageSize="<%# PageControlCommon.GetPageSize() %>" runat="server"
                                                        AutoGenerateColumns="False" Width="100%" OnPageIndexChanging="GvPending_PageIndexChanging"
                                                        AllowPaging="true" >
                                                        <Columns>
                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,Print %>" Visible="false">
                                                                <ItemTemplate>
                                                                    <asp:ImageButton ID="IbtnPrint" runat="server" CausesValidation="False" CommandArgument='<%# Eval("Trans_Id") %>'
                                                                        ImageUrl="~/Images/print.png" CommandName='<%# Eval("Emp_Id") %>' OnCommand="IbtnPrint_Command"
                                                                        ToolTip="<%$ Resources:Attendance,Print %>" Width="16px" Visible="true" />
                                                                </ItemTemplate>
                                                                <ItemStyle  HorizontalAlign="Center" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,Edit %>" Visible="false">
                                                                <ItemTemplate>
                                                                    <asp:ImageButton ID="btnEdit" runat="server" CommandArgument='<%# Eval("Trans_Id") %>'
                                                                        ImageUrl="~/Images/edit.png" OnCommand="btnEdit_Command" CausesValidation="False"
                                                                        ToolTip="<%$ Resources:Attendance,Edit %>" />
                                                                </ItemTemplate>
                                                                <ItemStyle HorizontalAlign="Center" />
                                                                <ItemStyle  />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,Employee Id %>">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblEmpId" runat="server" Text='<%# Eval("Emp_Code") %>'></asp:Label>
                                                                </ItemTemplate>
                                                                <ItemStyle  />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,Employee Name  %>">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblEmpName" runat="server" Text='<%# Eval("Emp_Name") %>'></asp:Label>
                                                                </ItemTemplate>
                                                                <ItemStyle  />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,Type %>">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblScheduleType" runat="server" Text='<%# leavetype(Eval("Partial_Leave_Type").ToString()) %>'></asp:Label>
                                                                </ItemTemplate>
                                                                <ItemStyle  />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,Apply Date %>">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblApplicatonDate" runat="server" Text='<%# GetDate(Eval("Partial_Leave_Date")) %>'></asp:Label>
                                                                </ItemTemplate>
                                                                <ItemStyle  />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,From Time %>">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblFromDate" runat="server" Text='<%# Eval("From_Time") %>'></asp:Label>
                                                                </ItemTemplate>
                                                                <ItemStyle  />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,To Time %>">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblTodays" runat="server" Text='<%# Eval("To_Time") %>'></asp:Label>
                                                                </ItemTemplate>
                                                                <ItemStyle  />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,PL Type %>">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblgvPLType" runat="server" Text='<%# GetPLType(Eval("Field1").ToString()) %>'></asp:Label>
                                                                </ItemTemplate>
                                                                <ItemStyle  />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,Status %>">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblStatus" runat="server" Text='<%# Eval("Is_Confirmed")%>'></asp:Label>
                                                                </ItemTemplate>
                                                                <ItemStyle  />
                                                            </asp:TemplateField>
                                                            <%--  <asp:TemplateField HeaderText="<%$ Resources:Attendance,Approve %>">
                                                    <ItemTemplate>
                                                        <asp:ImageButton ID="IbtnApprove" runat="server" CommandName='<%# Eval("Emp_Id") %>' CommandArgument='<%# Eval("Trans_Id") %>'
                                                            ImageUrl="~/Images/approve.png" CausesValidation="False" OnCommand="btnApprove_Command"
                                                            Visible="false" ToolTip="<%$ Resources:Attendance,Approve %>" /></ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Center" />
                                                 <ItemStyle  />
                                                </asp:TemplateField>
                                                 <asp:TemplateField HeaderText="<%$ Resources:Attendance,Reject %>">
                                                    <ItemTemplate>
                                                        <asp:ImageButton ID="IbtnReject"  Visible="false"  runat="server" CausesValidation="False"   CommandArgument='<%# Eval("Trans_Id") %>'
                                                            ImageUrl="~/Images/disapprove.png" OnCommand="IbtnReject_Command" 
                                                            ToolTip="<%$ Resources:Attendance,Reject %>" />
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Center" />
                                                    <ItemStyle  />
                                                </asp:TemplateField>--%>
                                                        </Columns>
                                                        
                                                        
                                                        <PagerStyle CssClass="pagination-ys" />
                                                        
                                                    </asp:GridView>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                                <div class="box box-warning box-solid">
                                    <div class="box-header with-border">
                                        <h3 class="box-title"></h3>
                                    </div>
                                    <div class="box-body">
                                        <div class="row">
                                            <div class="col-md-12">
                                                <div class="flow">
                                                    <asp:GridView CssClass="table-striped table-bordered table table-hover" ID="gvLeaveStatus" PageSize="<%# PageControlCommon.GetPageSize() %>"
                                                        runat="server" AutoGenerateColumns="False" Width="100%" OnPageIndexChanging="gvLeaveStatus_PageIndexChanging"
                                                        AllowPaging="true" >
                                                        <Columns>
                                                            <%-- <asp:TemplateField HeaderText="<%$ Resources:Attendance,Edit %>" Visible="false">
                                                    <ItemTemplate>
                                                        <asp:ImageButton ID="btnEdit" runat="server" CommandArgument='<%# Eval("Trans_Id") %>'
                                                            ImageUrl="~/Images/edit.png" OnCommand="btnEdit_Command" CausesValidation="False"
                                                            ToolTip="<%$ Resources:Attendance,Edit %>" />
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Center" />
                                                    <ItemStyle  />
                                                </asp:TemplateField>--%>
                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,Employee Id %>">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblEmpId" runat="server" Text='<%# Eval("Emp_Code") %>'></asp:Label>
                                                                </ItemTemplate>
                                                                <ItemStyle  />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,Employee Name  %>">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblEmpName" runat="server" Text='<%# Eval("Emp_Name") %>'></asp:Label>
                                                                </ItemTemplate>
                                                                <ItemStyle  />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,Type %>">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblScheduleType" runat="server" Text='<%# leavetype(Eval("Partial_Leave_Type").ToString()) %>'></asp:Label>
                                                                </ItemTemplate>
                                                                <ItemStyle  />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,Apply Date %>">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblApplicatonDate" runat="server" Text='<%# GetDate(Eval("Partial_Leave_Date")) %>'></asp:Label>
                                                                </ItemTemplate>
                                                                <ItemStyle  />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,To Time %>">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblFromDate" runat="server" Text='<%# Eval("From_Time") %>'></asp:Label>
                                                                </ItemTemplate>
                                                                <ItemStyle  />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,From Time %>">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblTodays" runat="server" Text='<%# Eval("To_Time") %>'></asp:Label>
                                                                </ItemTemplate>
                                                                <ItemStyle  />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,PL Type %>">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblgvPLType" runat="server" Text='<%# GetPLType(Eval("Field1").ToString()) %>'></asp:Label>
                                                                </ItemTemplate>
                                                                <ItemStyle  />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,Status %>">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblStatus" runat="server" Text='<%# Eval("Is_Confirmed")%>'></asp:Label>
                                                                </ItemTemplate>
                                                                <ItemStyle  />
                                                            </asp:TemplateField>
                                                            <%--  <asp:TemplateField HeaderText="<%$ Resources:Attendance,Approve %>">
                                                    <ItemTemplate>
                                                        <asp:ImageButton ID="IbtnApprove" runat="server" CommandName='<%# Eval("Emp_Id") %>' CommandArgument='<%# Eval("Trans_Id") %>'
                                                            ImageUrl="~/Images/approve.png" CausesValidation="False" OnCommand="btnApprove_Command"
                                                            Visible="false" ToolTip="<%$ Resources:Attendance,Approve %>" /></ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Center" />
                                                 <ItemStyle  />
                                                </asp:TemplateField>
                                                 <asp:TemplateField HeaderText="<%$ Resources:Attendance,Reject %>">
                                                    <ItemTemplate>
                                                        <asp:ImageButton ID="IbtnReject"  Visible="false"  runat="server" CausesValidation="False"   CommandArgument='<%# Eval("Trans_Id") %>'
                                                            ImageUrl="~/Images/disapprove.png" OnCommand="IbtnReject_Command" 
                                                            ToolTip="<%$ Resources:Attendance,Reject %>" />
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Center" />
                                                    <ItemStyle  />
                                                </asp:TemplateField>--%>
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
                                <div class="row">
                                    <div class="col-md-12">
                                        <div class="box box-primary">
                                            <div class="box-body">
                                                <div class="form-group">
                                                    <div class="col-md-12" style="text-align: center">
                                                        <asp:RadioButton ID="rbtnPersonal"  runat="server" GroupName="Partial"
                                                            Text="<%$ Resources:Attendance,Personal%>" AutoPostBack="true" OnCheckedChanged="rbtnPersonal_CheckedChanged" />
                                                        <asp:RadioButton ID="rbtnOfficial" Style="margin-left: 50px;"  runat="server" GroupName="Partial"
                                                            Text="<%$ Resources:Attendance,Offical %>" AutoPostBack="true" OnCheckedChanged="rbtnOfficial_CheckedChanged" />
                                                        <hr />
                                                    </div>
                                                    <div class="col-md-12">
                                                        <div class="col-md-6">
                                                            <asp:Label ID="Label2" runat="server"  Text="<%$ Resources:Attendance,Apply Date %>"></asp:Label>
                                                            <asp:TextBox ID="txtApplyDate" runat="server" CssClass="form-control" />
                                                            <asp:HiddenField ID="hdnEditDate" runat="server" Value="0" />
                                                            <cc1:CalendarExtender  OnClientShown="showCalendar"   ID="CalendarExtender2" runat="server" Enabled="True" TargetControlID="txtApplyDate">
                                                            </cc1:CalendarExtender>
                                                            <br />
                                                        </div>
                                                    </div>
                                                    <div class="col-md-12" id="TrWithTime1" runat="server" visible="false">
                                                        <div class="col-md-6">
                                                            <asp:Label ID="Label3" runat="server"  Text="<%$ Resources:Attendance,From Time %>"></asp:Label>
                                                            <asp:TextBox ID="txtInTime" runat="server" CssClass="form-control" />
                                                            <cc1:MaskedEditExtender ID="MaskedEditExtender1" runat="server" CultureAMPMPlaceholder=""
                                                                CultureCurrencySymbolPlaceholder="" CultureDateFormat="" CultureDatePlaceholder=""
                                                                CultureDecimalPlaceholder="" CultureThousandsPlaceholder="" CultureTimePlaceholder=""
                                                                Enabled="True" ErrorTooltipEnabled="True" Mask="99:99" MaskType="Time" TargetControlID="txtInTime"
                                                                UserTimeFormat="TwentyFourHour" MessageValidatorTip="true" InputDirection="LeftToRight">
                                                            </cc1:MaskedEditExtender>
                                                            <cc1:MaskedEditValidator ID="MaskedEditValidator1" runat="server" ControlExtender="MaskedEditExtender1"
                                                                ControlToValidate="txtInTime" Display="Dynamic" InvalidValueMessage="Please enter a valid time."
                                                                SetFocusOnError="True" />
                                                            <br />
                                                        </div>
                                                        <div class="col-md-6">
                                                            <asp:Label ID="Label4" runat="server"  Text="<%$ Resources:Attendance,To Time %>"></asp:Label>
                                                            <asp:TextBox ID="txtOuttime" runat="server" CssClass="form-control" />
                                                            <cc1:MaskedEditExtender ID="txtOnDuty_MaskedEditExtender" runat="server" CultureAMPMPlaceholder=""
                                                                CultureCurrencySymbolPlaceholder="" CultureDateFormat="" CultureDatePlaceholder=""
                                                                CultureDecimalPlaceholder="" CultureThousandsPlaceholder="" CultureTimePlaceholder=""
                                                                Enabled="True" ErrorTooltipEnabled="True" Mask="99:99" MaskType="Time" TargetControlID="txtOuttime"
                                                                UserTimeFormat="TwentyFourHour" MessageValidatorTip="true" InputDirection="LeftToRight">
                                                            </cc1:MaskedEditExtender>
                                                            <cc1:MaskedEditValidator ID="MaskedEditValidator2" runat="server" ControlExtender="txtOnDuty_MaskedEditExtender"
                                                                ControlToValidate="txtOuttime" Display="Dynamic" InvalidValueMessage="Please enter a valid time."
                                                                SetFocusOnError="True" />
                                                            <br />
                                                        </div>
                                                    </div>

                                            <div id="TrWithOutTime1" runat="server" visible="false" class="col-md-12">                                                        
                                                        <asp:Label ID="lblPLType" runat="server"  Text="<%$ Resources:Attendance,PL Type %>" />
                                                        <asp:RadioButton ID="rbBegining"  runat="server" Text="Begining"
                                                            GroupName="RB" OnCheckedChanged="rbBegining_CheckedChanged" AutoPostBack="true" />
                                                        <asp:RadioButton ID="rbMiddle"  runat="server" Text="Middle"
                                                            GroupName="RB" OnCheckedChanged="rbBegining_CheckedChanged" AutoPostBack="true" />
                                                        <asp:RadioButton ID="rbEnding" runat="server"  Text="Ending"
                                                            GroupName="RB" OnCheckedChanged="rbBegining_CheckedChanged" AutoPostBack="true" />
                                                        <br />
                                                    </div>
                                                    <div id="TrWithOutTime2" runat="server" visible="false" class="col-md-12">
                                                        <asp:RadioButtonList ID="rbTimeTable" runat="server"  />
                                                        <br />
                                                    </div>
                                                    <div class="col-md-12">
                                                        <div class="col-md-12">
                                                        <asp:HiddenField ID="HiddenField1" runat="server" />
                                                        <asp:Label ID="Label7" runat="server"  Text="<%$ Resources:Attendance,Description %>"></asp:Label>
                                                        <asp:TextBox ID="txtDescription" TextMode="MultiLine" runat="server"
                                                            CssClass="form-control" />
                                                        <br />
                                                    </div>
                                                        </div>
                                                    <div class="col-md-12">
                                                    <div class="col-md-12" style="text-align:center">
                                                        <asp:HiddenField ID="hdnEmpId" runat="server" />
                                                        <asp:HiddenField ID="hdnEdit" runat="server" />
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
                                                        </div>

                                                    <div class="col-md-12" class="flow">
                                                        <br />
                                                        <div class="col-md-12">
                                                        <asp:GridView CssClass="table-striped table-bordered table table-hover" ID="gvLeaveSummary_PartialLeave" runat="server" AutoGenerateColumns="False"
                                                            Visible="false"  Width="100%">
                                                            <Columns>
                                                                <asp:TemplateField HeaderText="<%$ Resources:Attendance,Total Leave %>">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblTotalDays" runat="server" Text='<%# Eval("Total_Days") %>'></asp:Label>
                                                                    </ItemTemplate>
                                                                    <ItemStyle  Width="25%" />
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="<%$ Resources:Attendance,Used Leave %>">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblUsedDays" runat="server" Text='<%# Eval("Used_Days") %>'></asp:Label>
                                                                    </ItemTemplate>
                                                                    <ItemStyle  Width="25%" />
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="<%$ Resources:Attendance,Pending Leave %>">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblUsedDays" runat="server" Text='<%# Eval("Pending_Days") %>'></asp:Label>
                                                                    </ItemTemplate>
                                                                    <ItemStyle  Width="25%" />
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="<%$ Resources:Attendance,Remaning Leave %>">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblUsedDays" runat="server" Text='<%# Eval("Remaning_Days") %>'></asp:Label>
                                                                    </ItemTemplate>
                                                                    <ItemStyle  Width="25%" />
                                                                </asp:TemplateField>
                                                            </Columns>
                                                           
                                                            <PagerStyle CssClass="pagination-ys" />
                                                            
                                                        </asp:GridView>
                                                            </div>
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

    <asp:Panel ID="pnlList" runat="server" Visible="false"></asp:Panel>
    <asp:Panel ID="pnlNew" runat="server" Visible="false"></asp:Panel>
    <asp:Panel ID="pnlMenuList" runat="server" Visible="false"></asp:Panel>
    <asp:Panel ID="pnlMenuNew" runat="server" Visible="false"></asp:Panel>

</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="FooterContent" runat="Server">
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="AfterLoadScriptContent" runat="Server"><script src="../Script/common.js"></script>
    <script type="text/javascript">
        function Li_List_Click() {
            document.getElementById("<%=Btn_List.ClientID %>").click();
        }

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
</asp:Content>


